// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataValidationWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class DataValidationWrapper : 
  CommonWrapper,
  IInternalDataValidation,
  IDataValidation,
  IParentApplication,
  IOptimizedUpdate
{
  private DataValidationImpl m_dataValidation;
  private RangeImpl m_range;
  private DataValidationImpl m_dvOld;
  internal IRange EntireDVRange;
  private double m_Number;

  internal DataValidationImpl Wrapped
  {
    get => this.m_dataValidation;
    set
    {
      if (value == this.m_dataValidation)
        return;
      this.m_dataValidation = value;
    }
  }

  public DataValidationWrapper(RangeImpl range, DataValidationImpl wrap)
  {
    if (wrap == null)
    {
      wrap = range.InnerWorksheet.DVTable.Add((DValRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DVal)).AddDVRecord((DVRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DV));
      wrap.AddRange(range);
    }
    this.m_dataValidation = wrap;
    this.m_range = range;
  }

  public string PromptBoxTitle
  {
    get => this.m_dataValidation.PromptBoxTitle;
    set
    {
      if (!(this.PromptBoxTitle != value))
        return;
      this.BeginUpdate();
      this.m_dataValidation.PromptBoxTitle = value;
      this.EndUpdate();
    }
  }

  public string PromptBoxText
  {
    get => this.m_dataValidation.PromptBoxText;
    set
    {
      if (!(this.PromptBoxText != value))
        return;
      this.BeginUpdate();
      this.m_dataValidation.PromptBoxText = value;
      this.EndUpdate();
    }
  }

  public string ErrorBoxTitle
  {
    get => this.m_dataValidation.ErrorBoxTitle;
    set
    {
      if (!(this.ErrorBoxTitle != value))
        return;
      this.BeginUpdate();
      this.m_dataValidation.ErrorBoxTitle = value;
      this.EndUpdate();
    }
  }

  public string ErrorBoxText
  {
    get => this.m_dataValidation.ErrorBoxText;
    set
    {
      if (!(this.ErrorBoxText != value))
        return;
      this.BeginUpdate();
      this.m_dataValidation.ErrorBoxText = value;
      this.EndUpdate();
    }
  }

  public string FirstFormula
  {
    get
    {
      FormulaUtil formulaUtil = this.m_dataValidation.Workbook.FormulaUtil;
      if (this.FirstFormulaTokens != null && this.FirstFormulaTokens.Length == 1 && (this.FirstFormulaTokens[0] is DoublePtg || this.FirstFormulaTokens[0] is IntegerPtg))
      {
        if (this.AllowType == ExcelDataType.Date)
          return this.m_dataValidation.FirstDateTime.ToShortDateString();
        if (this.AllowType == ExcelDataType.Time)
          return this.m_dataValidation.FirstDateTime.ToLongTimeString();
      }
      return formulaUtil.ParsePtgArray(this.FirstFormulaTokens, this.m_range.Row, this.m_range.Column, false, false);
    }
    set
    {
      if (!(this.FirstFormula != value))
        return;
      this.BeginUpdate();
      WorksheetImpl worksheet = this.m_range.Worksheet as WorksheetImpl;
      DateTime result;
      if ((this.AllowType == ExcelDataType.Time || this.AllowType == ExcelDataType.Date) && DateTime.TryParse(value, out result))
      {
        this.m_dataValidation.FirstDateTime = result;
      }
      else
      {
        this.m_dataValidation.DVRecord.FirstFormulaTokens = DataValidationImpl.GetFormulaPtg(ref value, (FormulaUtil) null, worksheet, this.m_range.Row - 1, this.m_range.Column - 1);
        this.m_dataValidation.FirstFormula = value;
        DVRecord dvRecord = this.m_dataValidation.DVRecord;
        Ptg[] firstFormulaTokens = dvRecord.FirstFormulaTokens;
        dvRecord.IsStrListExplicit = dvRecord.DataType == ExcelDataType.User && firstFormulaTokens != null && dvRecord.SecondFormulaTokens == null && firstFormulaTokens.Length == 1 && firstFormulaTokens[0].TokenCode == FormulaToken.tStringConstant;
      }
      this.EndUpdate();
    }
  }

  public DateTime FirstDateTime
  {
    get => this.m_dataValidation.FirstDateTime;
    set
    {
      if (!(this.FirstDateTime != value) && !(this.FirstDateTime == DateTime.MinValue))
        return;
      this.BeginUpdate();
      this.m_dataValidation.FirstDateTime = value;
      this.EndUpdate();
    }
  }

  public string SecondFormula
  {
    get
    {
      FormulaUtil formulaUtil = this.m_dataValidation.Workbook.FormulaUtil;
      if (this.SecondFormulaTokens != null && this.SecondFormulaTokens.Length == 1 && (this.SecondFormulaTokens[0] is DoublePtg || this.SecondFormulaTokens[0] is IntegerPtg))
      {
        if (this.AllowType == ExcelDataType.Date)
          return this.m_dataValidation.SecondDateTime.ToShortDateString();
        if (this.AllowType == ExcelDataType.Time)
          return this.m_dataValidation.SecondDateTime.ToLongTimeString();
      }
      return formulaUtil.ParsePtgArray(this.SecondFormulaTokens, this.m_range.Row, this.m_range.Column, false, false);
    }
    set
    {
      if (!(this.SecondFormula != value))
        return;
      this.BeginUpdate();
      WorksheetImpl worksheet = this.m_range.Worksheet as WorksheetImpl;
      DateTime result;
      if ((this.AllowType == ExcelDataType.Time || this.AllowType == ExcelDataType.Date) && DateTime.TryParse(value, out result))
      {
        this.m_dataValidation.SecondDateTime = result;
      }
      else
      {
        this.m_dataValidation.DVRecord.SecondFormulaTokens = DataValidationImpl.GetFormulaPtg(ref value, (FormulaUtil) null, worksheet, this.m_range.Row - 1, this.m_range.Column - 1);
        this.m_dataValidation.SecondFormula = value;
      }
      this.EndUpdate();
    }
  }

  public DateTime SecondDateTime
  {
    get => this.m_dataValidation.SecondDateTime;
    set
    {
      if (!(this.SecondDateTime != value) && !(this.FirstDateTime == DateTime.MinValue))
        return;
      this.BeginUpdate();
      this.m_dataValidation.SecondDateTime = value;
      this.EndUpdate();
    }
  }

  public ExcelDataType AllowType
  {
    get => this.m_dataValidation.AllowType;
    set
    {
      if (this.AllowType == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.AllowType = value;
      if (this.m_dataValidation.IsListInFormula && value != ExcelDataType.User)
        this.m_dataValidation.IsListInFormula = false;
      this.EndUpdate();
    }
  }

  public ExcelDataValidationComparisonOperator CompareOperator
  {
    get => this.m_dataValidation.CompareOperator;
    set
    {
      if (this.CompareOperator == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.CompareOperator = value;
      this.EndUpdate();
    }
  }

  public bool IsListInFormula
  {
    get => this.m_dataValidation.IsListInFormula;
    set
    {
      if (this.IsListInFormula == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.IsListInFormula = value;
      this.EndUpdate();
    }
  }

  public bool IsEmptyCellAllowed
  {
    get => this.m_dataValidation.IsEmptyCellAllowed;
    set
    {
      if (this.IsEmptyCellAllowed == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.IsEmptyCellAllowed = value;
      this.EndUpdate();
    }
  }

  public bool IsSuppressDropDownArrow
  {
    get => this.m_dataValidation.IsSuppressDropDownArrow;
    set
    {
      if (this.IsSuppressDropDownArrow == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.IsSuppressDropDownArrow = value;
      this.EndUpdate();
    }
  }

  public bool ShowPromptBox
  {
    get => this.m_dataValidation.ShowPromptBox;
    set
    {
      if (this.ShowPromptBox == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.ShowPromptBox = value;
      this.EndUpdate();
    }
  }

  public bool ShowErrorBox
  {
    get => this.m_dataValidation.ShowErrorBox;
    set
    {
      if (this.ShowErrorBox == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.ShowErrorBox = value;
      this.EndUpdate();
    }
  }

  public int PromptBoxHPosition
  {
    get => this.m_dataValidation.PromptBoxHPosition;
    set
    {
      if (this.PromptBoxHPosition == value)
        return;
      this.OnBeforeCollectionChange();
      this.m_dataValidation.PromptBoxHPosition = value;
      this.OnAfterCollectionChange();
    }
  }

  public int PromptBoxVPosition
  {
    get => this.m_dataValidation.PromptBoxVPosition;
    set
    {
      if (this.PromptBoxVPosition == value)
        return;
      this.OnBeforeCollectionChange();
      this.m_dataValidation.PromptBoxVPosition = value;
      this.OnAfterCollectionChange();
    }
  }

  public bool IsPromptBoxVisible
  {
    get => this.m_dataValidation.IsPromptBoxVisible;
    set
    {
      if (this.IsPromptBoxVisible == value)
        return;
      this.OnBeforeCollectionChange();
      this.m_dataValidation.IsPromptBoxVisible = value;
      this.OnAfterCollectionChange();
    }
  }

  public bool IsPromptBoxPositionFixed
  {
    get => this.m_dataValidation.IsPromptBoxPositionFixed;
    set
    {
      if (this.IsPromptBoxPositionFixed == value)
        return;
      this.OnBeforeCollectionChange();
      this.m_dataValidation.IsPromptBoxPositionFixed = value;
      this.OnAfterCollectionChange();
    }
  }

  public ExcelErrorStyle ErrorStyle
  {
    get => this.m_dataValidation.ErrorStyle;
    set
    {
      if (this.ErrorStyle == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.ErrorStyle = value;
      this.EndUpdate();
    }
  }

  public string[] ListOfValues
  {
    get => this.m_dataValidation.ListOfValues;
    set
    {
      this.BeginUpdate();
      this.m_dataValidation.ListOfValues = value;
      this.EndUpdate();
    }
  }

  public IRange DataRange
  {
    get => this.m_dataValidation.DataRange;
    set
    {
      if (this.DataRange == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.DataRange = value;
      this.EndUpdate();
    }
  }

  public Ptg[] FirstFormulaTokens
  {
    get => this.m_dataValidation.FirstFormulaTokens;
    set
    {
      if (this.FirstFormulaTokens == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.FirstFormulaTokens = value;
      this.EndUpdate();
    }
  }

  public Ptg[] SecondFormulaTokens
  {
    get => this.m_dataValidation.SecondFormulaTokens;
    set
    {
      if (this.SecondFormulaTokens == value)
        return;
      this.BeginUpdate();
      this.m_dataValidation.SecondFormulaTokens = value;
      this.EndUpdate();
    }
  }

  public IApplication Application => this.m_dataValidation.Application;

  public object Parent => this.m_dataValidation.Parent;

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
    {
      DataValidationImpl dataValidationImpl = new DataValidationImpl(this.m_dataValidation.ParentCollection, this.m_dataValidation.DVRecord);
      dataValidationImpl.EntireDVRange = this.EntireDVRange;
      if (dataValidationImpl.DVRecord.AddrList.Length > 0)
      {
        Rectangle[] rectangles = new Rectangle[dataValidationImpl.DVRecord.AddrList.Length];
        for (int index = 0; index < dataValidationImpl.DVRecord.AddrList.Length; ++index)
          rectangles[index] = dataValidationImpl.DVRecord.AddrList[index].GetRectangle();
        dataValidationImpl.RemoveRange(rectangles);
      }
      if ((dataValidationImpl.AllowType == ExcelDataType.User || dataValidationImpl.AllowType == ExcelDataType.Any) && this.EntireDVRange != null)
        this.m_range = this.EntireDVRange as RangeImpl;
      dataValidationImpl.AddRange(this.m_range);
      this.m_dvOld = this.m_dataValidation;
      this.m_dataValidation = dataValidationImpl;
    }
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_dataValidation = this.m_dataValidation.ParentCollection.Add(this.m_dataValidation);
    if (this.m_dvOld != null && this.m_dvOld != this.m_dataValidation)
      this.m_dvOld.RemoveRange(this.m_range);
    this.m_dvOld = (DataValidationImpl) null;
  }

  private void OnBeforeCollectionChange()
  {
  }

  private void OnAfterCollectionChange()
  {
  }
}
