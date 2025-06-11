// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataValidationImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class DataValidationImpl : 
  IInternalDataValidation,
  IDataValidation,
  IParentApplication,
  IOptimizedUpdate,
  IReparse,
  ICloneParent
{
  private const int DEF_MAX_LIST_LENGTH = 256 /*0x0100*/;
  private const int TitleLimit = 32 /*0x20*/;
  private const int TextLimit = 225;
  private const int InputTextLimit = 255 /*0xFF*/;
  private const int DEF_WRONG_DATE = 61;
  private readonly Type[] DATARANGETYPES = new Type[4]
  {
    typeof (Ref3DPtg),
    typeof (RefPtg),
    typeof (AreaPtg),
    typeof (Area3DPtg)
  };
  private static object m_lock = new object();
  internal IRange EntireDVRange;
  private DVRecord m_dvRecord;
  private string m_strFirstFormula = string.Empty;
  private string m_strSecondFormula = string.Empty;
  private DataValidationCollection m_DVCollection;
  private RangesOperations m_cells = new RangesOperations();
  private FormulaUtil m_formulaUtil;

  public DataValidationImpl(DataValidationCollection parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    this.m_dvRecord = (DVRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DV);
    this.m_DVCollection = parent;
  }

  [CLSCompliant(false)]
  public DataValidationImpl(DataValidationCollection parent, DVRecord dv)
    : this(parent)
  {
    this.m_dvRecord = (DVRecord) dv.Clone();
    this.FillCells(this.m_dvRecord);
    try
    {
      this.Reparse();
    }
    catch (ParseException ex)
    {
      WorkbookImpl workbook = this.Workbook;
      if (workbook == null)
        throw new ArgumentNullException("Can't find parent workbook");
      if (workbook.Loading)
        workbook.AddForReparse((IReparse) this);
      else
        throw;
    }
  }

  public void AddRange(DataValidationImpl dv)
  {
    List<Rectangle> cellList = dv.m_cells.CellList;
    int count1 = this.m_cells.CellList.Count;
    this.m_cells.AddCells((IList<Rectangle>) cellList);
    int count2 = this.m_cells.CellList.Count;
    if (count1 == count2)
      return;
    this.m_dvRecord.ClearAddressList();
  }

  public void AddRange(RangeImpl range) => this.m_cells.AddRange((IRange) range);

  [CLSCompliant(false)]
  public void AddRange(TAddr tAddr) => this.m_cells.AddRange(tAddr.GetRectangle());

  [CLSCompliant(false)]
  internal void AddRanges(TAddr tAddr) => this.m_cells.AddRanges(tAddr.GetRectangle());

  [CLSCompliant(false)]
  public void AddRange(ICombinedRange range)
  {
    this.m_cells.AddRectangles((IList<Rectangle>) range.GetRectangles());
  }

  public void RemoveRange(RangeImpl range) => this.RemoveRange(range.GetRectangles());

  public void RemoveRange(Rectangle[] rectangles)
  {
    this.m_cells.Remove(rectangles);
    if (this.m_cells.CellList.Count != 0)
      return;
    this.m_DVCollection.Remove(this);
  }

  private void FillFromCells()
  {
    this.m_dvRecord.AddrList = new TAddr[0];
    List<Rectangle> cellList = this.m_cells.CellList;
    int index = 0;
    for (int count = cellList.Count; index < count; ++index)
      this.m_dvRecord.Add(new TAddr(cellList[index]));
  }

  private void FillCells(DVRecord dv)
  {
    TAddr[] addrList = dv.AddrList;
    int index = 0;
    for (int length = addrList.Length; index < length; ++index)
      this.m_cells.AddRange(addrList[index].GetRectangle());
  }

  private Ptg[] ConvertFromDateTime(DateTime value)
  {
    double num = this.AllowType != ExcelDataType.Time ? value.ToOADate() : value.TimeOfDay.TotalDays;
    DoublePtg ptg = (DoublePtg) FormulaUtil.CreatePtg(FormulaToken.tNumber, (object) num);
    ptg.Value = num;
    return new Ptg[1]{ (Ptg) ptg };
  }

  private DateTime ConvertToDateTime(Ptg[] arrPtgs)
  {
    DateTime dateTime = DateTime.MinValue;
    if (arrPtgs != null && arrPtgs.Length == 1)
    {
      Ptg arrPtg = arrPtgs[0];
      switch (arrPtg)
      {
        case IntegerPtg _:
          dateTime = DateTime.FromOADate((double) ((IntegerPtg) arrPtg).Value);
          break;
        case DoublePtg _:
          double d = ((DoublePtg) arrPtg).Value;
          if (this.Workbook.Date1904)
            d += 1462.0;
          else if (d < 61.0)
            ++d;
          dateTime = DateTime.FromOADate(d);
          break;
      }
    }
    return dateTime;
  }

  public static Ptg[] GetFormulaPtg(
    ref string value,
    FormulaUtil formulaUtil,
    WorksheetImpl sheet,
    int row,
    int column)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (value.Length == 0)
      throw new ArgumentOutOfRangeException("Value cannot be empty.");
    string str = value;
    if (str[0] == '=')
      str = UtilityMethods.RemoveFirstCharUnsafe(str);
    NumberFormatInfo provider = (NumberFormatInfo) null;
    if (formulaUtil != null)
      provider = formulaUtil.NumberFormat;
    double result;
    Ptg[] formulaPtg;
    if (double.TryParse(str, NumberStyles.Any, (IFormatProvider) provider, out result))
    {
      DoublePtg ptg = (DoublePtg) FormulaUtil.CreatePtg(FormulaToken.tNumber);
      ptg.Value = result;
      formulaPtg = new Ptg[1]{ (Ptg) ptg };
      if (ptg.Value == 0.0)
        ;
    }
    else
      formulaPtg = DataValidationImpl.ParseFormula(str, sheet, formulaUtil, row, column);
    return formulaPtg;
  }

  public static Ptg[] ParseFormula(
    string strFormula,
    WorksheetImpl sheet,
    FormulaUtil formulaUtil,
    int row,
    int column)
  {
    Dictionary<Type, ReferenceIndexAttribute> indexes = new Dictionary<Type, ReferenceIndexAttribute>();
    indexes.Add(typeof (AreaPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (RefNPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (RefPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (Area3DPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (Ref3DPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (NamePtg), new ReferenceIndexAttribute(1));
    WorkbookImpl parentWorkbook = sheet.ParentWorkbook;
    DataValidationImpl.RegisterFunctions(true);
    Ptg[] parsedFormula = formulaUtil != null ? formulaUtil.ParseString(strFormula, (IWorksheet) sheet, indexes, 0, (Dictionary<string, string>) null, ExcelParseFormulaOptions.None, row, column) : parentWorkbook.FormulaUtil.ParseString(strFormula, (IWorksheet) null, indexes, 0, (Dictionary<string, string>) null, ExcelParseFormulaOptions.None, row, column);
    DataValidationImpl.RegisterFunctions(false);
    if (DataValidationImpl.IsZeroValuePtg(parsedFormula))
    {
      parsedFormula = new Ptg[1]{ parsedFormula[0] };
      strFormula = "0";
    }
    return parsedFormula;
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public static void RegisterFunctions(bool isRefNPtg)
  {
    lock (DataValidationImpl.m_lock)
    {
      if (isRefNPtg)
      {
        ReferenceIndexAttribute[] paramIndexes = new ReferenceIndexAttribute[1]
        {
          new ReferenceIndexAttribute(typeof (RefNPtg), 2)
        };
        FormulaUtil.EditRegisteredFunction("ISNUMBER", ExcelFunction.ISNUMBER, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISBLANK", ExcelFunction.ISBLANK, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISERR", ExcelFunction.ISERR, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISEVEN", ExcelFunction.ISEVEN, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISLOGICAL", ExcelFunction.ISLOGICAL, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISNA", ExcelFunction.ISNA, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISNONTEXT", ExcelFunction.ISNONTEXT, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISODD", ExcelFunction.ISODD, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISPMT", ExcelFunction.ISPMT, paramIndexes, 4);
        FormulaUtil.EditRegisteredFunction("ISREF", ExcelFunction.ISREF, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISTEXT", ExcelFunction.ISTEXT, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("OR", ExcelFunction.OR, paramIndexes, -1);
        FormulaUtil.EditRegisteredFunction("MOD", ExcelFunction.MOD, paramIndexes, 2);
        FormulaUtil.EditRegisteredFunction("MOD", ExcelFunction.MOD, paramIndexes, 2);
      }
      else
      {
        ReferenceIndexAttribute[] paramIndexes = new ReferenceIndexAttribute[1]
        {
          new ReferenceIndexAttribute(typeof (RefPtg), 2)
        };
        FormulaUtil.EditRegisteredFunction("ISNUMBER", ExcelFunction.ISNUMBER, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISBLANK", ExcelFunction.ISBLANK, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISERR", ExcelFunction.ISERR, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISEVEN", ExcelFunction.ISEVEN, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISLOGICAL", ExcelFunction.ISLOGICAL, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISNA", ExcelFunction.ISNA, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISNONTEXT", ExcelFunction.ISNONTEXT, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISODD", ExcelFunction.ISODD, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISPMT", ExcelFunction.ISPMT, paramIndexes, 4);
        FormulaUtil.EditRegisteredFunction("ISREF", ExcelFunction.ISREF, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("ISTEXT", ExcelFunction.ISTEXT, paramIndexes, 1);
        FormulaUtil.EditRegisteredFunction("OR", ExcelFunction.OR, paramIndexes, -1);
        FormulaUtil.EditRegisteredFunction("MOD", ExcelFunction.MOD, paramIndexes, 2);
        FormulaUtil.EditRegisteredFunction("MOD", ExcelFunction.MOD, paramIndexes, 2);
      }
    }
  }

  public void SetFormulaOneTwoValue(string value, FormulaUtil formulaUtil, bool isFormulaOne)
  {
    Ptg[] formulaPtg = DataValidationImpl.GetFormulaPtg(ref value, formulaUtil, this.m_DVCollection.Worksheet, 0, 0);
    if (isFormulaOne)
    {
      this.m_dvRecord.FirstFormulaTokens = formulaPtg;
      this.m_strFirstFormula = value;
    }
    else
    {
      this.m_dvRecord.SecondFormulaTokens = formulaPtg;
      this.m_strSecondFormula = value;
    }
  }

  public void SetFormulaValue(
    string value,
    FormulaUtil formulaUtil,
    TAddr taddr,
    bool isFormulaOne)
  {
    Ptg[] ptgArray = formulaUtil.ParseString(value, (IWorksheet) this.m_DVCollection.Worksheet, (Dictionary<string, string>) null, taddr.FirstRow, taddr.FirstCol, true);
    if (isFormulaOne)
    {
      this.m_dvRecord.FirstFormulaTokens = ptgArray;
      this.m_strFirstFormula = value;
    }
    else
    {
      this.m_dvRecord.SecondFormulaTokens = ptgArray;
      this.m_strSecondFormula = value;
    }
  }

  private static bool IsZeroValuePtg(Ptg[] parsedFormula)
  {
    bool flag = true;
    if (parsedFormula[0] is DoublePtg doublePtg)
    {
      if (doublePtg.Value == 0.0)
      {
        int index = 1;
        for (int length = parsedFormula.Length; index < length; ++index)
        {
          switch (parsedFormula[index].TokenCode)
          {
            case FormulaToken.tUnaryPlus:
            case FormulaToken.tUnaryMinus:
              continue;
            default:
              flag = false;
              goto label_8;
          }
        }
      }
    }
    else
      flag = false;
label_8:
    return flag;
  }

  public WorkbookImpl Workbook => this.m_DVCollection.Workbook;

  public WorksheetImpl Worksheet => this.m_DVCollection.Worksheet;

  [CLSCompliant(false)]
  public DVRecord DVRecord => this.m_dvRecord;

  public DataValidationCollection ParentCollection
  {
    get => this.m_DVCollection;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.m_DVCollection == value)
        return;
      this.m_DVCollection = value;
    }
  }

  public string[] DVRanges
  {
    get
    {
      this.FillFromCells();
      List<string> stringList = new List<string>();
      TAddr[] addrList = this.m_dvRecord.AddrList;
      int addrListSize = (int) this.m_dvRecord.AddrListSize;
      for (int index = 0; index < addrListSize; ++index)
      {
        TAddr taddr = addrList[index];
        string addressLocal = RangeImpl.GetAddressLocal(taddr.FirstRow + 1, taddr.FirstCol + 1, taddr.LastRow + 1, taddr.LastCol + 1);
        stringList.Add(addressLocal);
      }
      return stringList.ToArray();
    }
  }

  public int ShapesCount => this.m_cells.CellList.Count;

  public IApplication Application => this.m_DVCollection.Application;

  public object Parent => (object) this.m_DVCollection;

  public string PromptBoxTitle
  {
    get => this.m_dvRecord.PromtBoxTitle;
    set
    {
      if (!this.Workbook.Loading)
        this.CheckLimit(nameof (PromptBoxTitle), value, 32 /*0x20*/);
      if (!(this.m_dvRecord.PromtBoxTitle != value))
        return;
      this.m_dvRecord.PromtBoxTitle = value;
    }
  }

  public string PromptBoxText
  {
    get => this.m_dvRecord.PromtBoxText;
    set
    {
      if (!this.Workbook.Loading)
        this.CheckLimit(nameof (PromptBoxText), value, (int) byte.MaxValue);
      if (!(this.m_dvRecord.PromtBoxText != value))
        return;
      this.m_dvRecord.PromtBoxText = value;
    }
  }

  public string ErrorBoxTitle
  {
    get => this.m_dvRecord.ErrorBoxTitle;
    set
    {
      if (!this.Workbook.Loading && !this.Worksheet.IsParsing)
        this.CheckLimit(nameof (ErrorBoxTitle), value, 32 /*0x20*/);
      if (!(this.m_dvRecord.ErrorBoxTitle != value))
        return;
      this.m_dvRecord.ErrorBoxTitle = value;
    }
  }

  public string ErrorBoxText
  {
    get => this.m_dvRecord.ErrorBoxText;
    set
    {
      if (!this.Workbook.Loading)
        this.CheckLimit(nameof (ErrorBoxText), value, 225);
      if (!(this.m_dvRecord.ErrorBoxText != value))
        return;
      this.m_dvRecord.ErrorBoxText = value;
    }
  }

  public string FirstFormula
  {
    get => this.m_strFirstFormula;
    set
    {
      if (!(this.m_strFirstFormula != value))
        return;
      this.m_strFirstFormula = value;
    }
  }

  public DateTime FirstDateTime
  {
    get => this.ConvertToDateTime(this.m_dvRecord.FirstFormulaTokens);
    set => this.m_dvRecord.FirstFormulaTokens = this.ConvertFromDateTime(value);
  }

  public string SecondFormula
  {
    get => this.m_strSecondFormula;
    set
    {
      if (!(this.m_strSecondFormula != value))
        return;
      this.m_strSecondFormula = value;
    }
  }

  internal bool IsFormulaOrChoice
  {
    get => this.m_dvRecord.IsFormulaOrChoice;
    set => this.m_dvRecord.IsFormulaOrChoice = value;
  }

  public DateTime SecondDateTime
  {
    get => this.ConvertToDateTime(this.m_dvRecord.SecondFormulaTokens);
    set => this.m_dvRecord.SecondFormulaTokens = this.ConvertFromDateTime(value);
  }

  public ExcelDataType AllowType
  {
    get => this.m_dvRecord.DataType;
    set => this.m_dvRecord.DataType = value;
  }

  public ExcelDataValidationComparisonOperator CompareOperator
  {
    get => this.m_dvRecord.Condition;
    set => this.m_dvRecord.Condition = value;
  }

  public bool IsListInFormula
  {
    get => this.m_dvRecord.IsStrListExplicit;
    set => this.m_dvRecord.IsStrListExplicit = value;
  }

  public bool IsEmptyCellAllowed
  {
    get => this.m_dvRecord.IsEmptyCell;
    set => this.m_dvRecord.IsEmptyCell = value;
  }

  public bool IsSuppressDropDownArrow
  {
    get => this.m_dvRecord.IsSuppressArrow;
    set => this.m_dvRecord.IsSuppressArrow = value;
  }

  public bool ShowPromptBox
  {
    get => this.m_dvRecord.IsShowPromptBox;
    set => this.m_dvRecord.IsShowPromptBox = value;
  }

  public bool ShowErrorBox
  {
    get => this.m_dvRecord.IsShowErrorBox;
    set => this.m_dvRecord.IsShowErrorBox = value;
  }

  public int PromptBoxHPosition
  {
    get => this.m_DVCollection.PromptBoxHPosition;
    set => this.m_DVCollection.PromptBoxHPosition = value;
  }

  public int PromptBoxVPosition
  {
    get => this.m_DVCollection.PromptBoxVPosition;
    set => this.m_DVCollection.PromptBoxVPosition = value;
  }

  public bool IsPromptBoxVisible
  {
    get => this.m_DVCollection.IsPromptBoxVisible;
    set => this.m_DVCollection.IsPromptBoxVisible = value;
  }

  public bool IsPromptBoxPositionFixed
  {
    get => this.m_DVCollection.IsPromptBoxPositionFixed;
    set => this.m_DVCollection.IsPromptBoxPositionFixed = value;
  }

  public ExcelErrorStyle ErrorStyle
  {
    get => this.m_dvRecord.ErrorStyle;
    set => this.m_dvRecord.ErrorStyle = value;
  }

  public string[] ListOfValues
  {
    get
    {
      if (this.AllowType == ExcelDataType.User)
      {
        Ptg[] firstFormulaTokens = this.m_dvRecord.FirstFormulaTokens;
        if (firstFormulaTokens != null && firstFormulaTokens.Length == 1 && firstFormulaTokens[0] is StringConstantPtg)
        {
          StringConstantPtg stringConstantPtg = firstFormulaTokens[0] as StringConstantPtg;
          string[] listOfValues;
          if (this.IsFormulaOrChoice)
            listOfValues = stringConstantPtg.Value.Split(',');
          else
            listOfValues = stringConstantPtg.Value.Split(this.Application.ArgumentsSeparator);
          if (listOfValues != null && listOfValues.Length > 0)
            return listOfValues;
        }
        else if (firstFormulaTokens != null && firstFormulaTokens.Length == 1 && firstFormulaTokens[0] is AreaPtg)
        {
          AreaPtg areaPtg = firstFormulaTokens[0] as AreaPtg;
          if (areaPtg.TokenCode != FormulaToken.tAreaErr1 && areaPtg.TokenCode != FormulaToken.tAreaErr2 && areaPtg.TokenCode != FormulaToken.tAreaErr3)
          {
            IWorksheet sheet = !(areaPtg is ISheetReference) ? (IWorksheet) this.Worksheet : this.Workbook.GetSheetByReference((int) (areaPtg as ISheetReference).RefIndex);
            IRange range = areaPtg.GetRange((IWorkbook) this.Workbook, sheet);
            int row1 = range.Row;
            int column1 = range.Column;
            int lastRow = range.LastRow;
            int lastColumn = range.LastColumn;
            bool flag1 = row1 == lastRow;
            bool flag2 = column1 == lastColumn;
            string[] listOfValues = (string[]) null;
            int index = 0;
            if (flag1)
            {
              listOfValues = new string[lastColumn - column1 + 1];
              for (int column2 = column1; column2 <= lastColumn; ++column2)
              {
                listOfValues[index] = sheet[row1, column2].DisplayText;
                ++index;
              }
            }
            else if (flag2)
            {
              listOfValues = new string[lastRow - row1 + 1];
              for (int row2 = row1; row2 <= lastRow; ++row2)
              {
                listOfValues[index] = sheet[row2, column1].DisplayText;
                ++index;
              }
            }
            if (listOfValues != null && listOfValues.Length > 0)
              return listOfValues;
          }
        }
      }
      return (string[]) null;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      StringBuilder stringBuilder = new StringBuilder("\"");
      int index = 0;
      for (int length = value.Length; index < length; ++index)
        stringBuilder.Append(index + 1 == length ? value[index].Replace("\"", "\"\"") : value[index].Replace("\"", "\"\"") + CultureInfo.CurrentCulture.TextInfo.ListSeparator);
      if (stringBuilder.Length > 256 /*0x0100*/)
        throw new ArgumentOutOfRangeException(nameof (value), "Too many strings in the array or strings are too long.");
      stringBuilder.Append("\"");
      this.SetFormulaOneTwoValue(stringBuilder.ToString(), new FormulaUtil(this.m_DVCollection.Workbook.Application, (object) this.m_DVCollection.Workbook, NumberFormatInfo.InvariantInfo, ',', ';'), true);
      this.IsListInFormula = true;
      this.IsFormulaOrChoice = false;
      this.IsSuppressDropDownArrow = false;
      this.AllowType = ExcelDataType.User;
      this.CompareOperator = ExcelDataValidationComparisonOperator.NotEqual;
      this.ErrorStyle = ExcelErrorStyle.Stop;
      this.ShowErrorBox = true;
    }
  }

  public IRange DataRange
  {
    get
    {
      if (this.AllowType == ExcelDataType.User)
      {
        Ptg[] firstFormulaTokens = this.m_dvRecord.FirstFormulaTokens;
        WorkbookImpl workbook = this.Workbook;
        if (firstFormulaTokens != null && firstFormulaTokens.Length == 1 && Array.IndexOf<Type>(this.DATARANGETYPES, firstFormulaTokens[0].GetType()) != -1)
        {
          bool r1C1ReferenceMode = workbook.CalculationOptions.R1C1ReferenceMode;
          return this.Worksheet.Range[firstFormulaTokens[0].ToString(this.Workbook.FormulaUtil, 0, 0, r1C1ReferenceMode)];
        }
      }
      return (IRange) null;
    }
    set
    {
      RangeImpl rangeImpl = value != null ? (RangeImpl) value : throw new ArgumentNullException(nameof (DataRange));
      this.FirstFormula = value.Worksheet != this.Worksheet ? rangeImpl.AddressGlobal : rangeImpl.AddressGlobalWithoutSheetName;
      this.SetFormulaOneTwoValue(this.FirstFormula, new FormulaUtil(this.m_DVCollection.Workbook.Application, (object) this.m_DVCollection.Workbook, NumberFormatInfo.InvariantInfo, ',', ';'), true);
      this.SecondFormula = "";
      this.CompareOperator = ExcelDataValidationComparisonOperator.NotEqual;
      this.AllowType = ExcelDataType.User;
      this.ShowPromptBox = true;
      this.ShowErrorBox = true;
      this.IsListInFormula = false;
      this.IsEmptyCellAllowed = true;
      this.IsSuppressDropDownArrow = false;
    }
  }

  public Ptg[] FirstFormulaTokens
  {
    get => this.m_dvRecord.FirstFormulaTokens;
    set => this.m_dvRecord.FirstFormulaTokens = value;
  }

  public Ptg[] SecondFormulaTokens
  {
    get => this.m_dvRecord.SecondFormulaTokens;
    set => this.m_dvRecord.SecondFormulaTokens = value;
  }

  internal string ChoiceTokens
  {
    get => this.m_dvRecord.ChoiceTokens;
    set => this.m_dvRecord.ChoiceTokens = value;
  }

  private void UpdateFirstFormulaString()
  {
    FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
    Ptg[] firstFormulaTokens = this.m_dvRecord.FirstFormulaTokens;
    int iRow;
    int iColumn;
    this.GetRowColumn(out iRow, out iColumn);
    if (firstFormulaTokens == null || firstFormulaTokens.Length <= 0)
      return;
    this.m_strFirstFormula = formulaUtil.ParsePtgArray(firstFormulaTokens, iRow, iColumn, false, false);
  }

  private void UpdateSecondFormulaString()
  {
    FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
    Ptg[] secondFormulaTokens = this.m_dvRecord.SecondFormulaTokens;
    int iRow;
    int iColumn;
    this.GetRowColumn(out iRow, out iColumn);
    if (secondFormulaTokens == null || secondFormulaTokens.Length <= 0)
      return;
    this.m_strSecondFormula = formulaUtil.ParsePtgArray(secondFormulaTokens, iRow, iColumn, false, false);
  }

  private void GetRowColumn(out int iRow, out int iColumn)
  {
    iRow = 0;
    iColumn = 0;
    if (this.m_cells == null || this.m_cells.CellList.Count <= 0)
      return;
    Rectangle cell = this.m_cells.CellList[0];
    iRow = cell.Top + 1;
    iColumn = cell.Left + 1;
  }

  public void Reparse()
  {
    this.UpdateFirstFormulaString();
    this.UpdateSecondFormulaString();
  }

  public string GetFirstSecondFormula(FormulaUtil formulaUtil, bool bIsFirstFormula)
  {
    Ptg[] ptgs = bIsFirstFormula ? this.m_dvRecord.FirstFormulaTokens : this.m_dvRecord.SecondFormulaTokens;
    List<Rectangle> cellList = this.m_cells.CellList;
    Rectangle rectangle = cellList.Count > 0 ? cellList[0] : Rectangle.Empty;
    return ptgs == null ? string.Empty : formulaUtil.ParsePtgArray(ptgs, rectangle.Top + 1, rectangle.Left + 1, false, false);
  }

  public string GetR1C1FirstSecondFormula(FormulaUtil formulaUtil, bool bIsFirstFormula)
  {
    Ptg[] ptgs = bIsFirstFormula ? this.m_dvRecord.FirstFormulaTokens : this.m_dvRecord.SecondFormulaTokens;
    List<Rectangle> cellList = this.m_cells.CellList;
    Rectangle rectangle = cellList.Count > 0 ? cellList[0] : Rectangle.Empty;
    return ptgs == null ? string.Empty : formulaUtil.ParsePtgArray(ptgs, rectangle.Top, rectangle.Left, true, false);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    this.FillFromCells();
    records.Add((IBiffStorage) this.m_dvRecord);
  }

  public object Clone(object parent)
  {
    DataValidationImpl dataValidationImpl = (DataValidationImpl) this.MemberwiseClone();
    dataValidationImpl.SetParent(parent as DataValidationCollection);
    dataValidationImpl.m_dvRecord = (DVRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dvRecord);
    dataValidationImpl.m_cells = this.m_cells.Clone();
    return (object) dataValidationImpl;
  }

  private void SetParent(DataValidationCollection dataValidationCollection)
  {
    this.m_DVCollection = dataValidationCollection != null ? dataValidationCollection : throw new ArgumentNullException(nameof (dataValidationCollection));
  }

  public bool ContainsCell(long lCellIndex)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(lCellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(lCellIndex);
    return this.m_cells.Contains(Rectangle.FromLTRB(columnFromCellIndex - 1, rowFromCellIndex - 1, columnFromCellIndex - 1, rowFromCellIndex - 1));
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
    formulaUtil.UpdateNameIndex(this.m_dvRecord.FirstFormulaTokens, arrNewIndex);
    formulaUtil.UpdateNameIndex(this.m_dvRecord.SecondFormulaTokens, arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
    formulaUtil.UpdateNameIndex(this.m_dvRecord.FirstFormulaTokens, dicNewIndex);
    formulaUtil.UpdateNameIndex(this.m_dvRecord.SecondFormulaTokens, dicNewIndex);
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    FormulaUtil.MarkUsedReferences(this.m_dvRecord.FirstFormulaTokens, usedItems);
    FormulaUtil.MarkUsedReferences(this.m_dvRecord.SecondFormulaTokens, usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    Ptg[] firstFormulaTokens = this.m_dvRecord.FirstFormulaTokens;
    if (FormulaUtil.UpdateReferenceIndexes(firstFormulaTokens, arrUpdatedIndexes))
      this.m_dvRecord.FirstFormulaTokens = firstFormulaTokens;
    Ptg[] secondFormulaTokens = this.m_dvRecord.SecondFormulaTokens;
    if (!FormulaUtil.UpdateReferenceIndexes(secondFormulaTokens, arrUpdatedIndexes))
      return;
    this.m_dvRecord.SecondFormulaTokens = secondFormulaTokens;
  }

  internal DataValidationImpl Clone(
    DataValidationCollection dataValidationCollection,
    int iSourceRow,
    int iSourceColumn,
    int iRowDelta,
    int iColumnDelta,
    int iRowCount,
    int iColumnCount)
  {
    DataValidationImpl dataValidationImpl = (DataValidationImpl) this.Clone((object) this.m_DVCollection);
    WorkbookImpl workbook = dataValidationImpl.Workbook;
    Rectangle[] rectangles = new Rectangle[4]
    {
      Rectangle.FromLTRB(0, 0, workbook.MaxColumnCount - 1, iSourceRow - 2),
      Rectangle.FromLTRB(0, iSourceRow - 1, iSourceColumn - 2, iSourceRow + iRowCount - 1),
      Rectangle.FromLTRB(0, iSourceRow + iRowCount - 1, workbook.MaxColumnCount - 1, workbook.MaxRowCount - 1),
      Rectangle.FromLTRB(iSourceColumn + iColumnCount - 1, iSourceRow - 1, workbook.MaxColumnCount - 1, iSourceRow + iRowCount - 1)
    };
    dataValidationImpl.RemoveRange(rectangles);
    dataValidationImpl.m_cells.Offset(iRowDelta, iColumnDelta, dataValidationImpl.m_DVCollection.Workbook);
    return dataValidationImpl;
  }

  private void CheckLimit(string propertyName, string value, int limit)
  {
    if (value != null && value.Length > limit)
      throw new ArgumentOutOfRangeException($"{propertyName} cannot exceed {limit} characters.");
  }
}
