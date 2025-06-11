// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExternalRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ExternalRange : 
  INativePTG,
  ICombinedRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable
{
  private ExternWorksheetImpl m_sheet;
  private int m_iFirstRow;
  private int m_iFirstColumn;
  private int m_iLastRow;
  private int m_iLastColumn;
  private bool m_bIsNumReference;
  private bool m_bIsMultiReference;
  private bool m_bIsStringReference;

  public string Address => throw new Exception("The method or operation is not implemented.");

  public string AddressLocal
  {
    get
    {
      return RangeImpl.GetAddressLocal(this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn);
    }
  }

  public string AddressGlobal
  {
    get
    {
      return $"'[{this.m_sheet.Workbook.Index + 1}]{this.m_sheet.Name}'!{RangeImpl.GetAddressLocal(this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn)}";
    }
  }

  public string AddressR1C1 => throw new Exception("The method or operation is not implemented.");

  public string AddressR1C1Local
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool Boolean
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IBorders Borders => throw new Exception("The method or operation is not implemented.");

  public IRange[] Cells => throw new Exception("The method or operation is not implemented.");

  public int Column => this.m_iFirstColumn;

  public int ColumnGroupLevel => throw new Exception("The method or operation is not implemented.");

  public double ColumnWidth
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int Count
  {
    get
    {
      return (this.m_iLastColumn - this.m_iFirstColumn + 1) * (this.m_iLastRow - this.m_iFirstRow + 1);
    }
  }

  public DateTime DateTime
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string DisplayText => throw new Exception("The method or operation is not implemented.");

  public IRange End => throw new Exception("The method or operation is not implemented.");

  public IRange EntireColumn => throw new Exception("The method or operation is not implemented.");

  public IRange EntireRow => throw new Exception("The method or operation is not implemented.");

  public string Error
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string Formula
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string FormulaArray
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string FormulaArrayR1C1
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool FormulaHidden
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public DateTime FormulaDateTime
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string FormulaR1C1
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool FormulaBoolValue
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string FormulaErrorValue
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasDataValidation
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasBoolean => throw new Exception("The method or operation is not implemented.");

  public bool HasDateTime => throw new Exception("The method or operation is not implemented.");

  public bool HasFormula => throw new Exception("The method or operation is not implemented.");

  public bool HasFormulaArray => throw new Exception("The method or operation is not implemented.");

  public bool HasNumber => throw new Exception("The method or operation is not implemented.");

  public bool HasRichText => throw new Exception("The method or operation is not implemented.");

  public bool HasString
  {
    get
    {
      return this.m_sheet.CellRecords.GetCellType(this.Row, this.Column) == WorksheetImpl.TRangeValueType.String;
    }
  }

  public bool HasStyle => throw new Exception("The method or operation is not implemented.");

  public ExcelHAlign HorizontalAlignment
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IHyperLinks Hyperlinks
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public int IndentLevel
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsBlank => throw new Exception("The method or operation is not implemented.");

  public bool IsBoolean => throw new Exception("The method or operation is not implemented.");

  public bool IsError => throw new Exception("The method or operation is not implemented.");

  public bool IsGroupedByColumn
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsGroupedByRow => throw new Exception("The method or operation is not implemented.");

  public bool IsInitialized => throw new Exception("The method or operation is not implemented.");

  public int LastColumn => this.m_iLastColumn;

  public int LastRow => this.m_iLastRow;

  public double Number
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string NumberFormat
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public int Row => this.m_iFirstRow;

  public int RowGroupLevel => throw new Exception("The method or operation is not implemented.");

  public double RowHeight
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] Rows => throw new Exception("The method or operation is not implemented.");

  public IRange[] Columns => throw new Exception("The method or operation is not implemented.");

  public IStyle CellStyle
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string CellStyleName
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string Text
  {
    get => this.m_sheet.CellRecords.GetText(RangeImpl.GetCellIndex(this.Column, this.Row));
    set => throw new Exception("The method or operation is not implemented.");
  }

  public TimeSpan TimeSpan
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public string Value
  {
    get
    {
      long cellIndex = RangeImpl.GetCellIndex(this.m_iFirstRow, this.m_iFirstColumn);
      return !this.IsSingleCell ? (string) null : this.m_sheet.CellRecords.GetText(cellIndex);
    }
    set => throw new Exception("The method or operation is not implemented.");
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
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public ExcelVAlign VerticalAlignment
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IWorksheet Worksheet => (IWorksheet) this.m_sheet;

  public IRange this[int row, int column]
  {
    get => (IRange) new ExternalRange(this.m_sheet, row, column);
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRange this[string name]
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRange this[string name, bool IsR1C1Notation]
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IConditionalFormats ConditionalFormats
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IDataValidation DataValidation
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public string FormulaStringValue
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public double FormulaNumberValue
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasFormulaBoolValue
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasFormulaErrorValue
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasFormulaDateTime
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasFormulaNumberValue
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasFormulaStringValue
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ICommentShape Comment
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public IRichTextString RichText
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsMerged => throw new Exception("The method or operation is not implemented.");

  public IRange MergeArea => throw new Exception("The method or operation is not implemented.");

  public bool WrapText
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool HasExternalFormula
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public bool? IsStringsPreserved
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public BuiltInStyles? BuiltInStyle
  {
    get => throw new Exception("The method or operation is not implemented.");
    set => throw new Exception("The method or operation is not implemented.");
  }

  public IRange Activate() => throw new Exception("The method or operation is not implemented.");

  public IRange Activate(bool scroll)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Group(ExcelGroupBy groupBy)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Group(ExcelGroupBy groupBy, bool bCollapsed)
  {
    throw new Exception("The method or operation is not implemented.");
  }

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

  public void Merge() => throw new Exception("The method or operation is not implemented.");

  public void Merge(bool clearCells)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Ungroup(ExcelGroupBy groupBy)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void UnMerge() => throw new Exception("The method or operation is not implemented.");

  public void FreezePanes() => throw new Exception("The method or operation is not implemented.");

  public void Clear(ExcelClearOptions option)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Clear() => throw new Exception("The method or operation is not implemented.");

  public void Clear(bool isClearFormat)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Clear(ExcelMoveDirection direction)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void Clear(ExcelMoveDirection direction, ExcelCopyRangeOptions options)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void MoveTo(IRange destination)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange CopyTo(IRange destination)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options, bool skipBlank)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange IntersectWith(IRange range)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange CopyTo(IRange destination, bool pasteLink)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange MergeWith(IRange range)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void AutofitRows() => throw new Exception("The method or operation is not implemented.");

  public void AutofitColumns()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public ICommentShape AddComment()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(bool findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(DateTime findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(bool findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(DateTime findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    throw new Exception("The method or operation is not implemented.");
  }

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

  public void BorderAround() => throw new Exception("The method or operation is not implemented.");

  public void BorderAround(ExcelLineStyle borderLine)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderAround(ExcelLineStyle borderLine, Color borderColor)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderInside() => throw new Exception("The method or operation is not implemented.");

  public void BorderInside(ExcelLineStyle borderLine)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderInside(ExcelLineStyle borderLine, Color borderColor)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderInside(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void BorderNone() => throw new Exception("The method or operation is not implemented.");

  public void CollapseGroup(ExcelGroupBy groupBy)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ExpandGroup(ExcelGroupBy groupBy)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public void ExpandGroup(ExcelGroupBy groupBy, ExpandCollapseFlags flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string AddressGlobal2007 => this.AddressGlobal;

  public IRange[] GetDependents() => throw new NotImplementedException();

  public IRange[] GetDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetPrecedents() => throw new NotImplementedException();

  public IRange[] GetPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectDependents() => throw new NotImplementedException();

  public IRange[] GetDirectDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents() => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange Offset(int row, int column) => throw new NotImplementedException();

  public IRange Resize(int row, int column) => throw new NotImplementedException();

  public IApplication Application => this.m_sheet.Application;

  public object Parent => (object) this.m_sheet;

  public ExternalRange(ExternWorksheetImpl sheet, int row, int column)
    : this(sheet, row, column, row, column)
  {
  }

  public ExternalRange(
    ExternWorksheetImpl sheet,
    int row,
    int column,
    int lastRow,
    int lastColumn)
  {
    this.m_sheet = sheet != null ? sheet : throw new ArgumentNullException(nameof (sheet));
    this.m_iFirstRow = row;
    this.m_iFirstColumn = column;
    this.m_iLastRow = lastRow;
    this.m_iLastColumn = lastColumn;
  }

  public Ptg[] GetNativePtg()
  {
    int referenceIndex = this.m_sheet.ReferenceIndex;
    Ptg ptg1;
    if (this.IsSingleCell)
    {
      Ref3DPtg ptg2 = (Ref3DPtg) FormulaUtil.CreatePtg(FormulaToken.tRef3d1);
      ptg2.RefIndex = (ushort) referenceIndex;
      ptg2.RowIndex = this.m_iFirstRow - 1;
      ptg2.ColumnIndex = this.m_iFirstColumn - 1;
      ptg1 = (Ptg) ptg2;
    }
    else
    {
      Area3DPtg ptg3 = (Area3DPtg) FormulaUtil.CreatePtg(FormulaToken.tArea3d1);
      ptg3.RefIndex = (ushort) referenceIndex;
      ptg3.FirstRow = this.m_iFirstRow - 1;
      ptg3.FirstColumn = this.m_iFirstColumn - 1;
      ptg3.LastRow = this.m_iLastRow - 1;
      ptg3.LastColumn = this.m_iLastColumn - 1;
      ptg1 = (Ptg) ptg3;
    }
    return new Ptg[1]{ ptg1 };
  }

  public bool IsSingleCell
  {
    get => this.m_iFirstColumn == this.m_iLastColumn && this.m_iFirstRow == this.m_iLastRow;
  }

  public string GetNewAddress(Dictionary<string, string> names, out string strSheetName)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book)
  {
    int index = this.m_sheet.Workbook.Index;
    return (IRange) new ExternalRange(book.ExternWorkbooks[index].Worksheets[this.m_sheet.Index], this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn);
  }

  public void ClearConditionalFormats() => throw new NotSupportedException();

  public Rectangle[] GetRectangles()
  {
    return new Rectangle[1]
    {
      Rectangle.FromLTRB(this.m_iFirstColumn, this.m_iFirstRow, this.m_iLastColumn, this.m_iLastRow)
    };
  }

  public int GetRectanglesCount() => 1;

  public int CellsCount => this.Count;

  public string WorksheetName => this.Worksheet.Name;

  public IEnumerator<IRange> GetEnumerator() => throw new NotImplementedException();

  IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

  public string HtmlString
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public ExternWorksheetImpl ExternSheet => this.m_sheet;

  internal bool IsNumReference
  {
    get => this.m_bIsNumReference;
    set => this.m_bIsNumReference = value;
  }

  internal bool IsStringReference
  {
    get => this.m_bIsStringReference;
    set => this.m_bIsStringReference = value;
  }

  internal bool IsMultiReference
  {
    get => this.m_bIsMultiReference;
    set => this.m_bIsMultiReference = value;
  }
}
