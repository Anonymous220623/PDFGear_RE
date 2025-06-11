// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.InvalidRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class InvalidRange : 
  ICombinedRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable
{
  private object m_parent;
  private IApplication m_application;
  private int m_iFirstRow;
  private int m_iLastRow;
  private int m_iFirstColumn;
  private int m_iLastColumn;

  public string GetNewAddress(Dictionary<string, string> names, out string strSheetName)
  {
    throw new NotImplementedException();
  }

  public IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book)
  {
    throw new NotImplementedException();
  }

  public void ClearConditionalFormats() => throw new NotImplementedException();

  public Rectangle[] GetRectangles() => throw new NotImplementedException();

  public int GetRectanglesCount() => throw new NotImplementedException();

  public IRange Offset(int row, int column) => throw new NotImplementedException();

  public IRange Resize(int row, int column) => throw new NotImplementedException();

  public int CellsCount => throw new NotImplementedException();

  public string AddressGlobal2007 => throw new NotImplementedException();

  public string HtmlString
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string Address => throw new NotImplementedException();

  public string AddressLocal
  {
    get
    {
      return RangeImpl.GetAddressLocal(this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn);
    }
  }

  public string AddressGlobal => throw new NotImplementedException();

  public string AddressR1C1 => throw new NotImplementedException();

  public string AddressR1C1Local => throw new NotImplementedException();

  public bool Boolean
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public IBorders Borders => throw new NotImplementedException();

  public IRange[] Cells => throw new NotImplementedException();

  public int Column => throw new NotImplementedException();

  public int ColumnGroupLevel => throw new NotImplementedException();

  public double ColumnWidth
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public int Count => throw new NotImplementedException();

  public DateTime DateTime
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string DisplayText => throw new NotImplementedException();

  public IRange End => throw new NotImplementedException();

  public IRange EntireColumn => throw new NotImplementedException();

  public IRange EntireRow => throw new NotImplementedException();

  public string Error
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string Formula
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string FormulaArray
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string FormulaArrayR1C1
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool FormulaHidden
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public DateTime FormulaDateTime
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string FormulaR1C1
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool FormulaBoolValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string FormulaErrorValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool HasDataValidation => throw new NotImplementedException();

  public bool HasBoolean => throw new NotImplementedException();

  public bool HasDateTime => throw new NotImplementedException();

  public bool HasFormula => throw new NotImplementedException();

  public bool HasFormulaArray => throw new NotImplementedException();

  public bool HasNumber => throw new NotImplementedException();

  public bool HasRichText => throw new NotImplementedException();

  public bool HasString => throw new NotImplementedException();

  public bool HasStyle => throw new NotImplementedException();

  public ExcelHAlign HorizontalAlignment
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public IHyperLinks Hyperlinks => throw new NotImplementedException();

  public int IndentLevel
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool IsBlank => throw new NotImplementedException();

  public bool IsBoolean => throw new NotImplementedException();

  public bool IsError => throw new NotImplementedException();

  public bool IsGroupedByColumn => throw new NotImplementedException();

  public bool IsGroupedByRow => throw new NotImplementedException();

  public bool IsInitialized => throw new NotImplementedException();

  public int LastColumn => throw new NotImplementedException();

  public int LastRow => throw new NotImplementedException();

  public double Number
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string NumberFormat
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public int Row => throw new NotImplementedException();

  public int RowGroupLevel => throw new NotImplementedException();

  public double RowHeight
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public IRange[] Rows => throw new NotImplementedException();

  public IRange[] Columns => throw new NotImplementedException();

  public IStyle CellStyle
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string CellStyleName
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string Text
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public TimeSpan TimeSpan
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string Value
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string CalculatedValue
  {
    get
    {
      return this.Parent is IWorksheet && ((IWorksheet) this.Parent).CalcEngine != null ? ((IWorksheet) this.Parent).CalcEngine.PullUpdatedValue(RangeInfo.GetAlphaLabel(this.Column) + this.Row.ToString()) : (string) null;
    }
  }

  public object Value2
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public ExcelVAlign VerticalAlignment
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public IWorksheet Worksheet => throw new NotImplementedException();

  public IRange this[int row, int column]
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get => throw new NotImplementedException();
  }

  public IRange this[string name] => throw new NotImplementedException();

  public IRange this[string name, bool IsR1C1Notation] => throw new NotImplementedException();

  public IConditionalFormats ConditionalFormats => throw new NotImplementedException();

  public IDataValidation DataValidation => throw new NotImplementedException();

  public string FormulaStringValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public double FormulaNumberValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool HasFormulaBoolValue => throw new NotImplementedException();

  public bool HasFormulaErrorValue => throw new NotImplementedException();

  public bool HasFormulaDateTime => throw new NotImplementedException();

  public bool HasFormulaNumberValue => throw new NotImplementedException();

  public bool HasFormulaStringValue => throw new NotImplementedException();

  public ICommentShape Comment => throw new NotImplementedException();

  public IRichTextString RichText => throw new NotImplementedException();

  public bool IsMerged => throw new NotImplementedException();

  public IRange MergeArea => throw new NotImplementedException();

  public bool WrapText
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool HasExternalFormula => throw new NotImplementedException();

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool? IsStringsPreserved
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public BuiltInStyles? BuiltInStyle
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public string WorksheetName => "#REF";

  public IRange Activate() => throw new NotImplementedException();

  public IRange Activate(bool scroll) => throw new NotImplementedException();

  public IRange Group(ExcelGroupBy groupBy) => throw new NotImplementedException();

  public IRange Group(ExcelGroupBy groupBy, bool bCollapsed) => throw new NotImplementedException();

  public void SubTotal(int groupBy, ConsolidationFunction function, int[] totalList)
  {
    throw new NotSupportedException();
  }

  public void SubTotal(
    int groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    throw new NotSupportedException();
  }

  public void SubTotal(
    int[] groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    throw new NotSupportedException();
  }

  public double Sum() => throw new Exception("The method or operation is not implemented.");

  public double Sum(bool considerDateAsNumber)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double Average() => throw new Exception("The method or operation is not implemented.");

  public double Average(bool considerDateAsNumber)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double Min() => throw new Exception("The method or operation is not implemented.");

  public double Min(bool considerDateAsNumber)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public double Max() => throw new Exception("The method or operation is not implemented.");

  public double Max(bool considerDateAsNumber)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Trim() => throw new Exception("The method or operation is not implemented.");

  public void Merge() => throw new NotImplementedException();

  public void Merge(bool clearCells) => throw new NotImplementedException();

  public IRange Ungroup(ExcelGroupBy groupBy) => throw new NotImplementedException();

  public void UnMerge() => throw new NotImplementedException();

  public void FreezePanes() => throw new NotImplementedException();

  public void Clear() => throw new NotImplementedException();

  public void Clear(bool isClearFormat) => throw new NotImplementedException();

  public void Clear(ExcelClearOptions option) => throw new NotImplementedException();

  public void Clear(ExcelMoveDirection direction) => throw new NotImplementedException();

  public void Clear(ExcelMoveDirection direction, ExcelCopyRangeOptions options)
  {
    throw new NotImplementedException();
  }

  public void MoveTo(IRange destination) => throw new NotImplementedException();

  public IRange CopyTo(IRange destination) => throw new NotImplementedException();

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options)
  {
    throw new NotImplementedException();
  }

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options, bool skipBlank)
  {
    throw new NotImplementedException();
  }

  public IRange CopyTo(IRange destination, bool pasteLink) => throw new NotImplementedException();

  public IRange IntersectWith(IRange range) => throw new NotImplementedException();

  public IRange MergeWith(IRange range) => throw new NotImplementedException();

  public void AutofitRows() => throw new NotImplementedException();

  public void AutofitColumns() => throw new NotImplementedException();

  public ICommentShape AddComment() => throw new NotImplementedException();

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange FindFirst(bool findValue) => throw new NotImplementedException();

  public IRange FindFirst(DateTime findValue) => throw new NotImplementedException();

  public IRange FindFirst(TimeSpan findValue) => throw new NotImplementedException();

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(bool findValue) => throw new NotImplementedException();

  public IRange[] FindAll(DateTime findValue) => throw new NotImplementedException();

  public IRange[] FindAll(TimeSpan findValue) => throw new NotImplementedException();

  public void Replace(string oldValue, string newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, double newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderAround() => throw new NotImplementedException();

  public void BorderAround(ExcelLineStyle borderLine) => throw new NotImplementedException();

  public void BorderAround(ExcelLineStyle borderLine, Color borderColor)
  {
    throw new NotImplementedException();
  }

  public void BorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    throw new NotImplementedException();
  }

  public void BorderInside() => throw new NotImplementedException();

  public void BorderInside(ExcelLineStyle borderLine) => throw new NotImplementedException();

  public void BorderInside(ExcelLineStyle borderLine, Color borderColor)
  {
    throw new NotImplementedException();
  }

  public void BorderInside(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    throw new NotImplementedException();
  }

  public void BorderNone() => throw new NotImplementedException();

  public void CollapseGroup(ExcelGroupBy groupBy) => throw new NotImplementedException();

  public void ExpandGroup(ExcelGroupBy groupBy) => throw new NotImplementedException();

  public void ExpandGroup(ExcelGroupBy groupBy, ExpandCollapseFlags flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] GetDependents() => throw new NotImplementedException();

  public IRange[] GetDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetPrecedents() => throw new NotImplementedException();

  public IRange[] GetPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectDependents() => throw new NotImplementedException();

  public IRange[] GetDirectDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents() => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IEnumerator<IRange> GetEnumerator() => throw new NotImplementedException();

  IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

  public IApplication Application => this.m_application;

  public object Parent => this.m_parent;

  public InvalidRange(object parent, IRange range)
  {
    this.m_parent = parent;
    this.m_application = range.Application;
    this.m_iFirstColumn = range.Column;
    this.m_iFirstRow = range.Row;
    this.m_iLastRow = range.LastRow;
    this.m_iLastColumn = range.LastColumn;
  }
}
