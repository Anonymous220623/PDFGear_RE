// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.PageSetupOption
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class PageSetupOption
{
  private const string Hash = "#";
  private readonly Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter _excelToPdfConverter;
  private readonly List<int> _horizontalIds;
  private readonly WorksheetImpl _sheet;
  private readonly IRange _usedRange;
  private readonly List<int> _verticalIds;
  private string[] _colCollections;
  private List<int> _colIndexes;
  private List<IRange> _printAreas;
  private string[] _rowCollections;
  private List<int> _rowIndexes;
  private float _titleColWidth;
  private float _titleRowHeight;
  private bool _isInvalidTitleRowOrColumn;

  public PageSetupOption(WorksheetImpl sheet, IRange actualUsedRange)
    : this(sheet, actualUsedRange, (Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter) null)
  {
  }

  public PageSetupOption(
    WorksheetImpl sheet,
    IRange actualUsedRange,
    Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter excelToPdfConverter)
  {
    this._excelToPdfConverter = excelToPdfConverter;
    this._usedRange = actualUsedRange;
    this._sheet = sheet;
    this._verticalIds = new List<int>();
    this._horizontalIds = new List<int>();
    if (this._sheet.PageSetup.PrintTitleRows != null)
      this.ParseTitleRows();
    if (this._sheet.PageSetup.PrintTitleColumns != null)
      this.ParseTitleColumns();
    if (this.HasPrintArea)
      this.ParsePrintArea();
    if (this.HasVerticalBreak)
      this.ParseVerticalBreaks();
    if (!this.HasHorizontalBreak)
      return;
    this.ParseHorizontalBreaks();
  }

  public PageSetupOption()
  {
  }

  private void ParseHorizontalBreaks()
  {
    if (this._sheet.HPageBreaks.Count == 0)
      return;
    for (int Index = 0; Index < this._sheet.HPageBreaks.Count; ++Index)
    {
      int num = this._sheet.HPageBreaks[Index].Location.Row - 1;
      if (!this._horizontalIds.Contains(num))
        this._horizontalIds.Add(num);
    }
  }

  private void ParseVerticalBreaks()
  {
    if (this._sheet.VPageBreaks.Count == 0)
      return;
    for (int Index = 0; Index < this._sheet.VPageBreaks.Count; ++Index)
    {
      int num = this._sheet.VPageBreaks[Index].Location.Column - 1;
      if (!this._verticalIds.Contains(num))
        this._verticalIds.Add(num);
    }
  }

  internal int FitPagesWide => this._sheet.PageSetup.FitToPagesWide;

  internal int FitPagesTall => this._sheet.PageSetup.FitToPagesTall;

  internal List<int> HorizontalBreaks => this._horizontalIds;

  internal List<int> VerticalBreaks => this._verticalIds;

  internal int PrintTitleFirstRow
  {
    get
    {
      return this._rowCollections == null ? 0 : (int) Convert.ToInt16(this._rowCollections[0].Remove(0, 1));
    }
  }

  internal int PrintTitleFirstColumn
  {
    get
    {
      return this._colCollections == null ? 0 : (int) Convert.ToInt16((int) Convert.ToInt16(this._colCollections[0].Remove(0, 1).ToCharArray()[0]) - 65 + 1);
    }
  }

  internal int PrintTitleLastRow
  {
    get
    {
      return this._rowCollections == null ? 0 : (int) Convert.ToInt16(this._rowCollections[1].Remove(0, 1));
    }
  }

  internal int PrintTitleLastColumn
  {
    get
    {
      return this._colCollections == null ? 0 : (int) Convert.ToInt16((int) Convert.ToInt16(this._colCollections[1].Remove(0, 1).ToCharArray()[0]) - 65 + 1);
    }
  }

  private bool IsPrintTitleValid(string[] cellNames)
  {
    string strRow = (string) null;
    string strColumn = (string) null;
    foreach (string cellName in cellNames)
    {
      if (!string.IsNullOrEmpty(cellName))
      {
        FormulaUtil.IsCell(cellName, FormulaUtil.IsR1C1(cellName), out strRow, out strColumn);
        if (strRow != null || strColumn != null)
          return false;
      }
    }
    return true;
  }

  internal bool HasPrintTitleRows
  {
    get
    {
      return this._sheet.PageSetup.PrintTitleRows != null && this._rowCollections != null && !this._isInvalidTitleRowOrColumn;
    }
  }

  internal bool HasPrintTitleColumns
  {
    get
    {
      return this._sheet.PageSetup.PrintTitleColumns != null && this._colCollections != null && !this._isInvalidTitleRowOrColumn;
    }
  }

  internal bool HasPrintArea => this._sheet.PageSetup.PrintArea != null;

  internal bool HasVerticalBreak => this._sheet.VPageBreaks.Count != 0;

  internal bool HasHorizontalBreak => this._sheet.HPageBreaks.Count != 0;

  internal float TitleRowHeight
  {
    get
    {
      if ((double) this._titleRowHeight != 0.0)
        return this._titleRowHeight;
      ItemSizeHelper rowHeightGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this._sheet.GetRowHeightInPixels));
      if (this._excelToPdfConverter != null)
      {
        rowHeightGetter.ScaledCellHeight = (double) this._excelToPdfConverter.ScaledCellHeight;
        rowHeightGetter.ScaledCellWidth = (double) this._excelToPdfConverter.ScaledCellWidth;
      }
      return this.GetRowHeight(new PdfUnitConvertor(), rowHeightGetter);
    }
  }

  internal float TitleColumnWidth
  {
    get
    {
      if ((double) this._titleColWidth != 0.0)
        return this._titleColWidth;
      ItemSizeHelper columnWidthGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this._sheet.GetColumnWidthInPixels));
      if (this._excelToPdfConverter != null)
      {
        columnWidthGetter.ScaledCellHeight = (double) this._excelToPdfConverter.ScaledCellHeight;
        columnWidthGetter.ScaledCellWidth = (double) this._excelToPdfConverter.ScaledCellWidth;
      }
      return this.GetColumnWidth(new PdfUnitConvertor(), columnWidthGetter);
    }
  }

  internal List<int> RowIndexes
  {
    get
    {
      if (this._rowIndexes == null)
        this._rowIndexes = new List<int>();
      return this._rowIndexes;
    }
  }

  internal List<int> ColumnIndexes
  {
    get
    {
      if (this._colIndexes == null)
        this._colIndexes = new List<int>();
      return this._colIndexes;
    }
  }

  internal IRange[] PrintAreas
  {
    get
    {
      if (this._printAreas == null)
        this._printAreas = new List<IRange>();
      List<IRange> rangeList = new List<IRange>(this._printAreas.Count);
      foreach (IRange printArea in this._printAreas)
      {
        if (printArea is RangesCollection)
          rangeList.AddRange((IEnumerable<IRange>) (printArea as RangesCollection).InnerList);
        else
          rangeList.Add(printArea);
      }
      return rangeList.ToArray();
    }
  }

  internal WorksheetImpl Worksheet => this._sheet;

  internal IPageSetup PageSetup => this._sheet.PageSetup;

  private void ParseTitleRows()
  {
    string printTitleRows = this._sheet.PageSetup.PrintTitleRows;
    if (printTitleRows == string.Empty)
      return;
    string[] splittedTitle = this.GetSplittedTitle(printTitleRows);
    if (int.TryParse(splittedTitle[0].Remove(0, 1), out int _) && this.IsPrintTitleValid(splittedTitle))
      this._rowCollections = this.GetSplittedTitle(printTitleRows);
    else
      this._isInvalidTitleRowOrColumn = true;
  }

  private void ParseTitleColumns()
  {
    string printTitleColumns = this._sheet.PageSetup.PrintTitleColumns;
    if (printTitleColumns == string.Empty)
      return;
    string[] splittedTitle = this.GetSplittedTitle(printTitleColumns);
    if (!splittedTitle[0].Contains("#") && this.IsPrintTitleValid(splittedTitle))
      this._colCollections = this.GetSplittedTitle(printTitleColumns);
    else
      this._isInvalidTitleRowOrColumn = true;
  }

  private void ParsePrintArea()
  {
    WorkbookImpl workbook = this.Worksheet.Workbook as WorkbookImpl;
    Ptg[] ptgArray = workbook.FormulaUtil.ParseString(this._sheet.PageSetup.PrintArea);
    this._printAreas = new List<IRange>();
    if (ptgArray.Length == 1)
    {
      IRange validRange = ptgArray[0] is IRangeGetter rangeHolder ? this.GetValidRange(rangeHolder, workbook, this.Worksheet) : (IRange) null;
      if (validRange != null)
        this._printAreas.Add(this.UpdateRange(validRange));
      else
        this._printAreas.Add(this._usedRange);
    }
    else
    {
      IRange validRange1 = ptgArray[0] is IRangeGetter rangeHolder1 ? this.GetValidRange(rangeHolder1, workbook, this.Worksheet) : (IRange) null;
      if (validRange1 != null)
      {
        this._printAreas.Add(this.UpdateRange(validRange1));
        for (int index = 1; index < ptgArray.Length; ++index)
        {
          if (!ptgArray[index].IsOperation)
          {
            IRange validRange2 = ptgArray[index] is IRangeGetter rangeHolder2 ? this.GetValidRange(rangeHolder2, workbook, this.Worksheet) : (IRange) null;
            if (validRange2 != null)
            {
              this._printAreas.Add(this.UpdateRange(validRange2));
            }
            else
            {
              this._printAreas = new List<IRange>();
              this._printAreas.Add(this._usedRange);
              break;
            }
          }
        }
      }
      else
        this._printAreas.Add(this._usedRange);
    }
  }

  private IRange GetValidRange(
    IRangeGetter rangeHolder,
    WorkbookImpl workbook,
    WorksheetImpl worksheet)
  {
    IRange validRange = rangeHolder.GetRange((IWorkbook) workbook, (IWorksheet) worksheet);
    if ((rangeHolder is NamePtg || rangeHolder is NameXPtg) && validRange != null)
    {
      IName name = validRange as IName;
      if (validRange.Worksheet == null)
        validRange = workbook.TryParseTableOrNamedRange(name.Name, this.Worksheet);
      else if (validRange.Worksheet == worksheet)
        validRange = name.RefersToRange;
    }
    if (validRange != null && validRange.Worksheet != worksheet)
      validRange = (IRange) null;
    return validRange;
  }

  private string[] GetSplittedTitle(string value) => value.Split('!')[1].Split(':');

  private float GetRowHeight(PdfUnitConvertor converter, ItemSizeHelper rowHeightGetter)
  {
    this._titleRowHeight = 0.0f;
    if (!this.HasPrintTitleRows)
      return 0.0f;
    for (int printTitleFirstRow = this.PrintTitleFirstRow; printTitleFirstRow <= this.PrintTitleLastRow; ++printTitleFirstRow)
      this._titleRowHeight += converter.ConvertFromPixels(rowHeightGetter.GetSize(printTitleFirstRow), PdfGraphicsUnit.Point);
    return this._titleRowHeight;
  }

  private float GetColumnWidth(PdfUnitConvertor converter, ItemSizeHelper columnWidthGetter)
  {
    this._titleColWidth = 0.0f;
    if (!this.HasPrintTitleColumns)
      return this._titleColWidth;
    for (int titleFirstColumn = this.PrintTitleFirstColumn; titleFirstColumn <= this.PrintTitleLastColumn; ++titleFirstColumn)
      this._titleColWidth += converter.ConvertFromPixels(columnWidthGetter.GetSize(titleFirstColumn), PdfGraphicsUnit.Point);
    return this._titleColWidth;
  }

  private IRange UpdateRange(IRange range)
  {
    if (range.Row == 1 && range.LastRow == this._sheet.Workbook.MaxRowCount)
    {
      range = this._sheet[this._usedRange.Row, range.Column, this._usedRange.LastRow, range.LastColumn];
      IRange actualUsedRange = this._excelToPdfConverter.GetActualUsedRange((IWorksheet) this._sheet, range, true, false);
      range = this._sheet[actualUsedRange.Row, range.Column, actualUsedRange.LastRow, range.LastColumn];
    }
    else if (range.Column == 1 && range.LastColumn == this._sheet.Workbook.MaxColumnCount)
    {
      range = this._sheet[range.Row, this._usedRange.Column, range.LastRow, this._usedRange.LastColumn];
      IRange actualUsedRange = this._excelToPdfConverter.GetActualUsedRange((IWorksheet) this._sheet, range, false, true);
      range = this._sheet[range.Row, actualUsedRange.Column, range.LastRow, actualUsedRange.LastColumn];
    }
    else if (range.LastColumn != this._usedRange.LastColumn && range.LastRow != this._usedRange.LastRow && range.LastRow == this._sheet.Workbook.MaxRowCount && range.LastColumn == this._sheet.Workbook.MaxColumnCount)
      range = this._sheet[range.Row, this._usedRange.Column - 2, range.LastRow, this._usedRange.LastColumn];
    return range;
  }

  internal bool CheckRowBounds(int startIndex, int endIndex)
  {
    if (this.PrintTitleLastRow < startIndex)
      return false;
    return this.PrintTitleFirstRow >= startIndex || this.PrintTitleLastRow < startIndex || this.PrintTitleLastRow >= endIndex;
  }

  internal bool CheckColumnBounds(int startIndex, int endIndex)
  {
    return this.PrintTitleFirstColumn < startIndex;
  }

  internal IRange[] GetBreakRanges(LayoutOptions option, IRange[] actualUsedRange)
  {
    return actualUsedRange;
  }

  private void GetHorizontalBreaks(List<IRange> ranges)
  {
    IRange[] array = new IRange[ranges.Count];
    ranges.CopyTo(array);
    ranges.Clear();
    for (int index = 0; index < array.Length; ++index)
    {
      bool flag = false;
      int row = array[index].Row;
      int lastRow = array[index].LastRow;
      int column = array[index].Column;
      int lastColumn = array[index].LastColumn;
      foreach (int horizontalId in this._horizontalIds)
      {
        if (horizontalId >= row && horizontalId <= lastRow)
        {
          if (flag)
            row = ranges[ranges.Count - 1].LastRow + 1;
          IRange range = this._sheet[row, column, horizontalId, lastColumn];
          ranges.Add(range);
          flag = true;
        }
      }
      if (ranges.Count != 0 && ranges[ranges.Count - 1].LastRow < lastRow)
      {
        IRange range = ranges[ranges.Count - 1];
        ranges.Add(this._sheet[range.LastRow + 1, range.Column, lastRow, range.LastColumn]);
      }
      else if (!this.CheckRange(ranges, array[index]))
        ranges.Add(array[index]);
    }
  }

  private void GetVerticalBreaks(List<IRange> veriRange, int finalColumn)
  {
    IRange[] array = new IRange[veriRange.Count];
    veriRange.CopyTo(array);
    veriRange.Clear();
    for (int index = 0; index < array.Length; ++index)
    {
      bool flag = false;
      int column = array[index].Column;
      int lastColumn = array[index].LastColumn;
      foreach (int verticalId in this._verticalIds)
      {
        if (verticalId >= column && verticalId <= lastColumn)
        {
          if (flag)
            column = veriRange[veriRange.Count - 1].LastColumn + 1;
          IRange range = this._sheet.Range[array[index].Row, column, array[index].LastRow, verticalId];
          veriRange.Add(range);
          flag = true;
        }
      }
      if (veriRange.Count != 0 && veriRange[veriRange.Count - 1].LastColumn < finalColumn && !this._sheet.Range[array[index].Row, veriRange[veriRange.Count - 1].LastColumn + 1, array[index].LastRow, finalColumn].IsBlank)
      {
        IRange range = this._sheet[array[index].Row, veriRange[veriRange.Count - 1].LastColumn + 1, array[index].LastRow, finalColumn];
        veriRange.Add(range);
      }
      else if (!this.CheckRange(veriRange, array[index]) && this._verticalIds.Count == 0 || veriRange.Count == 0)
        veriRange.Add(array[index]);
    }
  }

  private bool CheckRange(List<IRange> veriRange, IRange range)
  {
    foreach (IRange range1 in veriRange)
    {
      if (range1.AddressLocal == range.AddressLocal)
        return true;
    }
    return false;
  }
}
