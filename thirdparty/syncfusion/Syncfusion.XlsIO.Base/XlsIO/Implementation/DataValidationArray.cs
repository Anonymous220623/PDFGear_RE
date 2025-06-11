// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataValidationArray
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class DataValidationArray : 
  CommonWrapper,
  IInternalDataValidation,
  IDataValidation,
  IParentApplication,
  IOptimizedUpdate
{
  private IRange m_range;
  private List<IDataValidation> m_arrValidationList = new List<IDataValidation>();
  internal IRange EntireDVRange;

  public DataValidationArray(IRange parent) => this.m_range = parent;

  public string PromptBoxTitle
  {
    get
    {
      string promptBoxTitle = this.m_range.Cells[0].DataValidation.PromptBoxTitle;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.PromptBoxTitle != promptBoxTitle)
          return (string) null;
      }
      return promptBoxTitle;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetPromptBoxTitle), (object) value);
    }
  }

  public string PromptBoxText
  {
    get
    {
      string promptBoxText = this.m_range.Cells[0].DataValidation.PromptBoxText;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.PromptBoxText != promptBoxText)
          return (string) null;
      }
      return promptBoxText;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetPromptBoxText), (object) value);
  }

  public string ErrorBoxTitle
  {
    get
    {
      string errorBoxTitle = this.m_range.Cells[0].DataValidation.ErrorBoxTitle;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.ErrorBoxTitle != errorBoxTitle)
          return (string) null;
      }
      return errorBoxTitle;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetErrorBoxTitle), (object) value);
  }

  public string ErrorBoxText
  {
    get
    {
      string errorBoxText = this.m_range.Cells[0].DataValidation.ErrorBoxText;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.ErrorBoxText != errorBoxText)
          return (string) null;
      }
      return errorBoxText;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetErrorBoxText), (object) value);
  }

  public string FirstFormula
  {
    get
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.m_range.Worksheet.Application, this.m_range.Worksheet);
      migrantRangeImpl.ResetRowColumn(this.m_range.Row, this.m_range.Column);
      string firstFormula = migrantRangeImpl.DataValidation.FirstFormula;
      Ptg[] firstFormulaTokens = (migrantRangeImpl.DataValidation as DataValidationWrapper).FirstFormulaTokens;
      int row = this.m_range.Row;
      for (int lastRow = this.m_range.LastRow; row <= lastRow; ++row)
      {
        int column = this.m_range.Column;
        for (int lastColumn = this.m_range.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.DataValidation.FirstFormula != firstFormula && (migrantRangeImpl.DataValidation as DataValidationWrapper).FirstFormulaTokens != firstFormulaTokens)
            return (string) null;
        }
      }
      return firstFormula;
    }
    set
    {
      FormulaUtil formulaUtil = ((WorkbookImpl) this.m_range.Worksheet.Workbook).FormulaUtil;
      if (value != null && value.Length > 0 && value[0] == '=')
        value = UtilityMethods.RemoveFirstCharUnsafe(value);
      DateTime result;
      if ((this.AllowType == ExcelDataType.Time || this.AllowType == ExcelDataType.Date) && DateTime.TryParse(value, out result))
        this.IterateDVs(new DataValidationArray.DVMethod(this.SetFirstDateTime), (object) result);
      else
        this.IterateDVs(new DataValidationArray.DVMethod(this.SetFirstFormulaTokens), (object) DataValidationImpl.GetFormulaPtg(ref value, (FormulaUtil) null, (WorksheetImpl) this.m_range.Worksheet, this.m_range.Row - 1, this.m_range.Column - 1));
    }
  }

  public DateTime FirstDateTime
  {
    get
    {
      DateTime firstDateTime = this.m_range.Cells[0].DataValidation.FirstDateTime;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.FirstDateTime != firstDateTime)
          return DateTime.MinValue;
      }
      return firstDateTime;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetFirstDateTime), (object) value);
  }

  public string SecondFormula
  {
    get
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.m_range.Worksheet.Application, this.m_range.Worksheet);
      migrantRangeImpl.ResetRowColumn(this.m_range.Row, this.m_range.Column);
      string secondFormula = migrantRangeImpl.DataValidation.SecondFormula;
      Ptg[] secondFormulaTokens = (migrantRangeImpl.DataValidation as DataValidationWrapper).SecondFormulaTokens;
      int row = this.m_range.Row;
      for (int lastRow = this.m_range.LastRow; row <= lastRow; ++row)
      {
        int column = this.m_range.Column;
        for (int lastColumn = this.m_range.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.DataValidation.SecondFormula != secondFormula && (migrantRangeImpl.DataValidation as DataValidationWrapper).SecondFormulaTokens != secondFormulaTokens)
            return (string) null;
        }
      }
      return secondFormula;
    }
    set
    {
      FormulaUtil formulaUtil = ((WorkbookImpl) this.m_range.Worksheet.Workbook).FormulaUtil;
      if (value != null && value.Length > 0 && value[0] == '=')
        value = UtilityMethods.RemoveFirstCharUnsafe(value);
      DateTime result;
      if ((this.AllowType == ExcelDataType.Time || this.AllowType == ExcelDataType.Date) && DateTime.TryParse(value, out result))
        this.IterateDVs(new DataValidationArray.DVMethod(this.SetSecondDateTime), (object) result);
      else
        this.IterateDVs(new DataValidationArray.DVMethod(this.SetSecondFormulaTokens), (object) DataValidationImpl.GetFormulaPtg(ref value, (FormulaUtil) null, (WorksheetImpl) this.m_range.Worksheet, this.m_range.Row - 1, this.m_range.Column - 1));
    }
  }

  public DateTime SecondDateTime
  {
    get
    {
      DateTime secondDateTime = this.m_range.Cells[0].DataValidation.SecondDateTime;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.SecondDateTime != secondDateTime)
          return DateTime.MinValue;
      }
      return secondDateTime;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetSecondDateTime), (object) value);
    }
  }

  public ExcelDataType AllowType
  {
    get
    {
      ExcelDataType allowType = this.m_range.Cells[0].DataValidation.AllowType;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.AllowType != allowType)
          return ExcelDataType.Any;
      }
      return allowType;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetAllowType), (object) value);
  }

  public ExcelDataValidationComparisonOperator CompareOperator
  {
    get
    {
      ExcelDataValidationComparisonOperator compareOperator = this.m_range.Cells[0].DataValidation.CompareOperator;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.CompareOperator != compareOperator)
          return ExcelDataValidationComparisonOperator.Between;
      }
      return compareOperator;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetCompareOperator), (object) value);
    }
  }

  public bool IsListInFormula
  {
    get
    {
      bool isListInFormula = this.m_range.Cells[0].DataValidation.IsListInFormula;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.IsListInFormula != isListInFormula)
          return false;
      }
      return isListInFormula;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetIsListInFormula), (object) value);
    }
  }

  public bool IsEmptyCellAllowed
  {
    get
    {
      bool emptyCellAllowed = this.m_range.Cells[0].DataValidation.IsEmptyCellAllowed;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.IsEmptyCellAllowed != emptyCellAllowed)
          return false;
      }
      return emptyCellAllowed;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetIsEmptyCellAllowed), (object) value);
    }
  }

  public bool IsSuppressDropDownArrow
  {
    get
    {
      bool suppressDropDownArrow = this.m_range.Cells[0].DataValidation.IsSuppressDropDownArrow;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.IsSuppressDropDownArrow != suppressDropDownArrow)
          return false;
      }
      return suppressDropDownArrow;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetIsSuppressDropDownArrow), (object) value);
    }
  }

  public bool ShowPromptBox
  {
    get
    {
      bool showPromptBox = this.m_range.Cells[0].DataValidation.ShowPromptBox;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.ShowPromptBox != showPromptBox)
          return false;
      }
      return showPromptBox;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetShowPromptBox), (object) value);
  }

  public bool ShowErrorBox
  {
    get
    {
      bool showErrorBox = this.m_range.Cells[0].DataValidation.ShowErrorBox;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.ShowErrorBox != showErrorBox)
          return false;
      }
      return showErrorBox;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetShowErrorBox), (object) value);
  }

  public int PromptBoxHPosition
  {
    get
    {
      int promptBoxHposition = this.m_range.Cells[0].DataValidation.PromptBoxHPosition;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.PromptBoxHPosition != promptBoxHposition)
          return int.MinValue;
      }
      return promptBoxHposition;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetPromptBoxHPosition), (object) value);
    }
  }

  public int PromptBoxVPosition
  {
    get
    {
      int promptBoxVposition = this.m_range.Cells[0].DataValidation.PromptBoxVPosition;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.PromptBoxVPosition != promptBoxVposition)
          return int.MinValue;
      }
      return promptBoxVposition;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetPromptBoxVPosition), (object) value);
    }
  }

  public bool IsPromptBoxVisible
  {
    get
    {
      bool promptBoxVisible = this.m_range.Cells[0].DataValidation.IsPromptBoxVisible;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.IsPromptBoxVisible != promptBoxVisible)
          return false;
      }
      return promptBoxVisible;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetIsPromptBoxVisible), (object) value);
    }
  }

  public bool IsPromptBoxPositionFixed
  {
    get
    {
      bool boxPositionFixed = this.m_range.Cells[0].DataValidation.IsPromptBoxPositionFixed;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.IsPromptBoxPositionFixed != boxPositionFixed)
          return false;
      }
      return boxPositionFixed;
    }
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetIsPromptBoxPositionFixed), (object) value);
    }
  }

  public ExcelErrorStyle ErrorStyle
  {
    get
    {
      ExcelErrorStyle errorStyle = this.m_range.Cells[0].DataValidation.ErrorStyle;
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].DataValidation.ErrorStyle != errorStyle)
          return ExcelErrorStyle.Stop;
      }
      return errorStyle;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetErrorStyle), (object) value);
  }

  public string[] ListOfValues
  {
    get
    {
      string[] listOfValues = this.m_range.Cells[0].DataValidation.ListOfValues;
      if (listOfValues == null)
        return (string[]) null;
      return this.FirstFormula == this.m_range.Cells[0].DataValidation.FirstFormula ? listOfValues : (string[]) null;
    }
    set => this.IterateDVs(new DataValidationArray.DVMethod(this.SetListOfValues), (object) value);
  }

  public IRange DataRange
  {
    get
    {
      IRange dataRange = this.m_range.Cells[0].DataValidation.DataRange;
      if (dataRange == null)
        return (IRange) null;
      return this.FirstFormula == this.m_range.Cells[0].DataValidation.FirstFormula ? dataRange : (IRange) null;
    }
    set
    {
      this.EntireDVRange = this.m_range;
      this.m_range = this.m_range.Cells[0];
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetDataRange), (object) value);
    }
  }

  public IApplication Application => this.m_range.Application;

  public object Parent => (object) this.m_range;

  public Ptg[] FirstFormulaTokens
  {
    get => throw new NotImplementedException();
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetFirstFormulaTokens), (object) value);
    }
  }

  public Ptg[] SecondFormulaTokens
  {
    get => throw new NotImplementedException();
    set
    {
      this.IterateDVs(new DataValidationArray.DVMethod(this.SetSecondFormulaTokens), (object) value);
    }
  }

  private void CopyFromFirstCell()
  {
    ICombinedRange range = this.m_range as ICombinedRange;
    WorksheetImpl worksheet = this.m_range.Worksheet as WorksheetImpl;
    DataValidationImpl wrapped = (worksheet[this.m_range.Row, this.m_range.Column].DataValidation as DataValidationWrapper).Wrapped;
    DataValidationTable innerDvTable = worksheet.InnerDVTable;
    Rectangle[] rectangles = range.GetRectangles();
    if (range.Row != range.LastRow || range.Column != range.LastColumn)
      innerDvTable.Remove(rectangles);
    wrapped.AddRange(range);
    if (innerDvTable.FindDataValidation(range.Row, range.Column) != null)
      return;
    wrapped.ParentCollection.Add(wrapped);
  }

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
      this.IterateCells(new DataValidationArray.CellMethod(this.BeginUpdate));
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    int index = 0;
    for (int count = this.m_arrValidationList.Count; index < count; ++index)
      this.m_arrValidationList[index].EndUpdate();
    this.m_arrValidationList.Clear();
  }

  private void IterateCells(DataValidationArray.CellMethod method)
  {
    Rectangle[] rectangles = (this.m_range as ICombinedRange).GetRectangles();
    int index = 0;
    for (int length = rectangles.Length; index < length; ++index)
      this.IterateRectangle(rectangles[index], method);
  }

  private void IterateRectangle(Rectangle rect, DataValidationArray.CellMethod method)
  {
    int num1 = rect.Top + 1;
    int num2 = rect.Bottom + 1;
    int num3 = rect.Left + 1;
    int num4 = rect.Right + 1;
    if (rect.Height >= rect.Width)
    {
      for (int column = num3; column <= num4; ++column)
      {
        for (int row = num1; row <= num2; ++row)
          method(row, column);
      }
    }
    else
    {
      for (int row = num1; row <= num2; ++row)
      {
        for (int column = num3; column <= num4; ++column)
          method(row, column);
      }
    }
  }

  private void BeginUpdate(int row, int column)
  {
    IDataValidation dataValidation = this.m_range.Worksheet[row, column].DataValidation;
    if (dataValidation is DataValidationWrapper)
      (dataValidation as DataValidationWrapper).EntireDVRange = this.EntireDVRange;
    dataValidation.BeginUpdate();
    this.m_arrValidationList.Add(dataValidation);
  }

  private void EndUpdate(int row, int column)
  {
  }

  private void IterateCells(DataValidationArray.DVMethod method, object value)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      IRange range = cells[index];
      method(range.DataValidation, value);
    }
  }

  private void IterateDVs(DataValidationArray.DVMethod method, object value)
  {
    this.BeginUpdate();
    int index = 0;
    for (int count = this.m_arrValidationList.Count; index < count; ++index)
    {
      IDataValidation arrValidation = this.m_arrValidationList[index];
      method(arrValidation, value);
    }
    this.EndUpdate();
  }

  private void SetPromptBoxTitle(IDataValidation dv, object value)
  {
    dv.PromptBoxTitle = value as string;
  }

  private void SetPromptBoxText(IDataValidation dv, object value)
  {
    dv.PromptBoxText = value as string;
  }

  private void SetErrorBoxTitle(IDataValidation dv, object value)
  {
    dv.ErrorBoxTitle = value as string;
  }

  private void SetErrorBoxText(IDataValidation dv, object value)
  {
    dv.ErrorBoxText = value as string;
  }

  private void SetFirstFormula(IDataValidation dv, object value)
  {
    dv.FirstFormula = value as string;
  }

  private void SetFirstDateTime(IDataValidation dv, object value)
  {
    dv.FirstDateTime = (DateTime) value;
  }

  private void SetSecondFormula(IDataValidation dv, object value)
  {
    dv.SecondFormula = value as string;
  }

  private void SetSecondDateTime(IDataValidation dv, object value)
  {
    dv.SecondDateTime = (DateTime) value;
  }

  private void SetAllowType(IDataValidation dv, object value)
  {
    dv.AllowType = (ExcelDataType) value;
  }

  private void SetCompareOperator(IDataValidation dv, object value)
  {
    dv.CompareOperator = (ExcelDataValidationComparisonOperator) value;
  }

  private void SetIsListInFormula(IDataValidation dv, object value)
  {
    dv.IsListInFormula = (bool) value;
  }

  private void SetIsEmptyCellAllowed(IDataValidation dv, object value)
  {
    dv.IsEmptyCellAllowed = (bool) value;
  }

  private void SetIsSuppressDropDownArrow(IDataValidation dv, object value)
  {
    dv.IsSuppressDropDownArrow = (bool) value;
  }

  private void SetShowPromptBox(IDataValidation dv, object value)
  {
    dv.ShowPromptBox = (bool) value;
  }

  private void SetShowErrorBox(IDataValidation dv, object value) => dv.ShowErrorBox = (bool) value;

  private void SetPromptBoxHPosition(IDataValidation dv, object value)
  {
    dv.PromptBoxHPosition = (int) value;
  }

  private void SetPromptBoxVPosition(IDataValidation dv, object value)
  {
    dv.PromptBoxVPosition = (int) value;
  }

  private void SetIsPromptBoxVisible(IDataValidation dv, object value)
  {
    dv.IsPromptBoxVisible = (bool) value;
  }

  private void SetIsPromptBoxPositionFixed(IDataValidation dv, object value)
  {
    dv.IsPromptBoxPositionFixed = (bool) value;
  }

  private void SetErrorStyle(IDataValidation dv, object value)
  {
    dv.ErrorStyle = (ExcelErrorStyle) value;
  }

  private void SetListOfValues(IDataValidation dv, object value)
  {
    string firstFormula = this.m_arrValidationList[0].FirstFormula;
    if (firstFormula != null)
    {
      dv.FirstFormula = firstFormula;
      dv.IsListInFormula = true;
      dv.IsSuppressDropDownArrow = false;
      dv.AllowType = ExcelDataType.User;
      dv.CompareOperator = ExcelDataValidationComparisonOperator.NotEqual;
      dv.ErrorStyle = ExcelErrorStyle.Stop;
      dv.ShowErrorBox = true;
    }
    else
      dv.ListOfValues = (string[]) value;
  }

  private void SetDataRange(IDataValidation dv, object value) => dv.DataRange = value as IRange;

  private void SetFirstFormulaTokens(IDataValidation dv, object value)
  {
    ((IInternalDataValidation) dv).FirstFormulaTokens = value as Ptg[];
  }

  private void SetSecondFormulaTokens(IDataValidation dv, object value)
  {
    ((IInternalDataValidation) dv).SecondFormulaTokens = value as Ptg[];
  }

  private delegate void CellMethod(int row, int column);

  private delegate void DVMethod(IDataValidation dv, object value);
}
