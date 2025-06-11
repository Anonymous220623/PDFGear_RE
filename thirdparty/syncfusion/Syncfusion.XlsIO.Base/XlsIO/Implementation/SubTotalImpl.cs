// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SubTotalImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class SubTotalImpl
{
  private bool m_isGrandTotalRow;
  private bool m_replace;
  private bool m_pageBreaks;
  private bool m_summaryBelowData;
  private bool m_groupRows;
  private ConsolidationFunction m_consolidationFunction;
  private int[] m_totalList;
  private int m_iRow;
  private int m_groupBy;
  private int m_columnCount;
  private RecordTable m_recordTable;
  private string[] m_columnName;
  private string m_total = "Total";
  private string m_grandTotal = "Grand Total";
  private int m_height;
  private ExcelVersion m_version;
  private int m_blockSize;
  private WorksheetImpl m_worksheet;
  private HPageBreaksCollection m_hPageBreaks;
  private int m_firstRow;
  private int m_firstColumn;
  private int m_lastRow;
  private int m_lastColumn;
  private int m_noOfsubTotals;

  internal SubTotalImpl(WorksheetImpl worksheet)
  {
    this.m_groupRows = true;
    this.m_worksheet = worksheet;
    this.m_recordTable = worksheet.CellRecords.Table;
    this.m_height = worksheet.AppImplementation.StandardHeightInRowUnits;
    this.m_blockSize = worksheet.AppImplementation.RowStorageAllocationBlockSize;
    this.m_version = worksheet.Version;
    this.m_hPageBreaks = worksheet.HPageBreaks as HPageBreaksCollection;
  }

  public void GetSheetFormulas(IWorksheet sheet)
  {
    IRange usedRange = sheet.UsedRange;
    List<int> intList = new List<int>();
    for (int row = usedRange.Row; row <= usedRange.LastRow; ++row)
    {
      for (int column = usedRange.Column; column <= usedRange.LastColumn; ++column)
      {
        if ((sheet as WorksheetImpl).GetCellType(row, column, false) == WorksheetImpl.TRangeValueType.Formula)
        {
          intList.Add(row);
          column = usedRange.LastColumn + 1;
        }
      }
    }
    (this.m_worksheet.Workbook as WorkbookImpl).WorkbookFormulas.Add(sheet.Name, intList);
  }

  internal int CalculateSubTotal(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    int groupBy,
    ConsolidationFunction function,
    int noOfSubtotal,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData,
    int groupLength)
  {
    this.m_worksheet.IsInsertingSubTotal = true;
    this.m_firstRow = firstRow;
    this.m_firstColumn = firstColumn;
    this.m_lastRow = lastRow;
    this.m_lastColumn = lastColumn;
    this.m_noOfsubTotals = noOfSubtotal;
    this.m_groupBy = groupBy;
    this.m_consolidationFunction = function;
    this.m_totalList = totalList;
    this.m_replace = replace;
    this.m_pageBreaks = pageBreaks;
    bool bUpdateNumberFormat = this.m_consolidationFunction == ConsolidationFunction.Sum || this.m_consolidationFunction == ConsolidationFunction.Average || this.m_consolidationFunction == ConsolidationFunction.Max || this.m_consolidationFunction == ConsolidationFunction.Min;
    this.m_summaryBelowData = this.m_noOfsubTotals <= 1 || this.m_noOfsubTotals > 1 && replace ? (this.m_worksheet.PageSetup.IsSummaryRowBelow = summaryBelowData) : this.m_worksheet.PageSetup.IsSummaryRowBelow;
    string str;
    switch (function)
    {
      case ConsolidationFunction.CountNums:
        str = ConsolidationFunction.Count.ToString();
        break;
      case ConsolidationFunction.Sum:
        str = "Total";
        break;
      default:
        str = function.ToString();
        break;
    }
    this.m_total = str;
    this.m_grandTotal = "Grand " + this.m_total;
    this.m_columnCount = firstColumn + groupBy;
    bool flag1 = false;
    for (int index = totalList.Length - 1; index >= 0; --index)
    {
      if (totalList[index] == groupBy)
        flag1 = true;
    }
    if (flag1)
    {
      bool flag2 = false;
      for (int index1 = groupBy - 1; index1 >= 0; --index1)
      {
        flag2 = true;
        for (int index2 = totalList.Length - 1; index2 >= 0; --index2)
        {
          if (totalList[index2] == index1)
          {
            flag2 = false;
            break;
          }
        }
        if (flag2)
        {
          this.m_columnCount = firstColumn + index1;
          break;
        }
      }
      if (!flag2)
      {
        this.m_columnCount = firstColumn;
        this.m_worksheet.InsertColumn(firstColumn + 1);
        ++firstColumn;
        ++lastColumn;
      }
    }
    this.m_columnName = new string[totalList.Length];
    for (int index = 0; index < totalList.Length; ++index)
    {
      this.m_columnName[index] = RangeImpl.GetColumnName(totalList[index] + firstColumn + 1);
      totalList[index] += firstColumn;
    }
    this.m_iRow = firstRow;
    this.m_iRow = firstRow;
    this.m_groupBy = firstColumn + groupBy;
    if (!replace && summaryBelowData)
      summaryBelowData = !this.HasSubTotal(firstRow);
    else if (summaryBelowData)
    {
      this.m_iRow = firstRow;
      this.m_worksheet.IsRemovingSubTotal = true;
      for (; this.m_iRow <= lastRow; ++this.m_iRow)
      {
        RowStorage row = this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version);
        if (row != null)
        {
          for (int index = 0; index < totalList.Length; ++index)
          {
            ICellPositionFormat record = row.GetRecord(totalList[index], this.m_blockSize);
            if (record != null && record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
            {
              this.m_worksheet.DeleteRow(this.m_iRow + 1);
              --this.m_lastRow;
              --this.m_iRow;
              break;
            }
          }
        }
      }
      if (this.m_noOfsubTotals > 1 && this.m_groupRows && this.m_replace)
      {
        IRange range = this.m_worksheet[firstRow, firstColumn + 1, lastRow, lastColumn + 1];
        for (int index = 0; index < this.m_noOfsubTotals; ++index)
          range.Ungroup(ExcelGroupBy.ByRows);
      }
      this.m_worksheet.IsRemovingSubTotal = false;
    }
    (this.m_worksheet.Workbook as WorkbookImpl).WorkbookFormulas = new Dictionary<string, List<int>>();
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.m_worksheet.Workbook.Worksheets)
      this.GetSheetFormulas(worksheet);
    if (summaryBelowData)
      this.CreateTotalBelowData(bUpdateNumberFormat);
    else
      this.CreateTotalAboveData(bUpdateNumberFormat);
    return groupLength > 1 && summaryBelowData ? this.m_iRow : this.m_lastRow;
  }

  private int SubTotalColumnIndex(RowStorage rowStorage)
  {
    int num = -1;
    for (int firstColumn = this.m_firstColumn; firstColumn <= this.m_lastColumn; ++firstColumn)
    {
      ICellPositionFormat record = rowStorage.GetRecord(firstColumn, this.m_blockSize);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
      {
        if (record.TypeCode != TBIFFRecord.Formula && num == -1)
          num = firstColumn;
        if (record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
          return num;
      }
    }
    return -1;
  }

  private bool HasSubTotal(int irow)
  {
    RowStorage row = this.m_recordTable.GetOrCreateRow(irow, this.m_height, false, this.m_version);
    if (row != null)
    {
      for (int firstColumn = this.m_firstColumn; firstColumn <= this.m_lastColumn; ++firstColumn)
      {
        ICellPositionFormat record = row.GetRecord(firstColumn, this.m_blockSize);
        if (record != null && record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
          return true;
      }
    }
    return false;
  }

  private bool HasSubTotal(RowStorage rowStorage)
  {
    for (int firstColumn = this.m_firstColumn; firstColumn <= this.m_lastColumn; ++firstColumn)
    {
      ICellPositionFormat record = rowStorage.GetRecord(firstColumn, this.m_blockSize);
      if (record != null && record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
        return true;
    }
    return false;
  }

  public void UpdateRowValue(int row)
  {
    if (this.m_worksheet.InsertedRows == null)
      this.m_worksheet.InsertedRows = new List<int>();
    this.m_worksheet.InsertedRows.Add(row);
    List<int> intList1 = (List<int>) null;
    (this.m_worksheet.Workbook as WorkbookImpl).WorkbookFormulas.TryGetValue(this.m_worksheet.Name, out intList1);
    if (intList1 != null)
    {
      for (int index1 = 0; index1 < intList1.Count; ++index1)
      {
        if (row <= intList1[index1])
        {
          List<int> intList2;
          int index2;
          (intList2 = intList1)[index2 = index1] = intList2[index2] + 1;
        }
      }
    }
    if (intList1 == null || this.m_summaryBelowData || this.m_isGrandTotalRow)
      return;
    intList1.Add(row);
  }

  private void CreateTotalBelowData(bool bUpdateNumberFormat)
  {
    bool flag1 = false;
    int num1 = -1;
    string strB = "";
    int num2 = -1;
    string str1 = $"=SUBTOTAL({(object) (int) this.m_consolidationFunction}{(object) this.m_worksheet.Application.ArgumentsSeparator}";
    this.m_iRow = this.m_firstRow;
    while (this.m_iRow < this.m_lastRow)
    {
      bool flag2 = false;
      RowStorage row1 = this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version);
      if (row1 != null)
      {
        if (!(this.m_recordTable[this.m_iRow, this.m_groupBy] is ICellPositionFormat cellPositionFormat) || cellPositionFormat.TypeCode == TBIFFRecord.Blank)
        {
          if (this.m_replace)
          {
            ++this.m_iRow;
            continue;
          }
          if (this.m_lastRow == this.m_iRow)
          {
            flag1 = true;
          }
          else
          {
            if (this.SubTotalColumnIndex(row1) != -1)
            {
              num1 = this.m_iRow;
              flag2 = true;
            }
            if (!flag2)
            {
              ++this.m_iRow;
              continue;
            }
          }
        }
        if (!flag2)
        {
          bool flag3 = true;
          string strA = string.Empty;
          if (cellPositionFormat != null)
          {
            IRange range = this.m_worksheet[cellPositionFormat.Row + 1, cellPositionFormat.Column + 1];
            if (range != null)
            {
              if (this.m_worksheet.GetCellType(cellPositionFormat.Row + 1, cellPositionFormat.Column + 1, false) == WorksheetImpl.TRangeValueType.Formula)
              {
                (range as RangeImpl).UpdateRowOffSet();
                (range as RangeImpl).UpdateFormulas();
              }
              strA = range.DisplayText;
            }
          }
          if (num2 == -1)
          {
            strB = strA;
            num2 = this.m_iRow > this.m_firstRow ? this.m_firstRow : this.m_iRow;
          }
          else
          {
            if (string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) != 0 && num2 + 1 <= this.m_iRow)
            {
              if (!this.m_replace && this.HasSubTotal(this.m_iRow))
              {
                if (num1 == -1)
                {
                  num1 = this.m_iRow;
                }
                else
                {
                  if (this.m_iRow != this.m_lastRow)
                  {
                    ++this.m_iRow;
                    continue;
                  }
                  flag1 = true;
                }
              }
              flag3 = false;
            }
            if (flag3)
              strB = strA;
          }
          if (flag3)
          {
            ++this.m_iRow;
            continue;
          }
        }
        this.UpdateRowValue(this.m_iRow + 1);
        this.m_worksheet.InsertRow(this.m_iRow + 1);
        ++this.m_lastRow;
        for (int index = 0; index < this.m_totalList.Length; ++index)
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Append(this.m_columnName[index]);
          stringBuilder.Append(num2 + 1);
          stringBuilder.Append(':');
          stringBuilder.Append(this.m_columnName[index]);
          stringBuilder.Append(this.m_iRow);
          stringBuilder.Append(')');
          IRange range = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
          range.Formula = stringBuilder.ToString();
          if (bUpdateNumberFormat)
            range.NumberFormat = this.m_worksheet[range.Row - 1, range.Column].NumberFormat;
        }
        IRange range1 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
        IRange range2 = this.m_worksheet[this.m_iRow, this.m_columnCount + 1];
        string str2 = strB;
        if (str2.StartsWith("="))
        {
          range1.Formula = str2;
          IRange range3 = this.m_worksheet[this.m_iRow - this.m_noOfsubTotals, this.m_columnCount + 1];
          if (this.m_worksheet.GetCellType(this.m_iRow - this.m_noOfsubTotals, this.m_columnCount + 1, false) == WorksheetImpl.TRangeValueType.Formula)
          {
            (range3 as RangeImpl).UpdateRowOffSet();
            (range3 as RangeImpl).UpdateFormulas();
          }
          str2 = range3.DisplayText;
        }
        range1.Value = $"{str2} {this.m_total}";
        range1.CellStyle.Font.Bold = true;
        if (this.m_groupRows)
        {
          if (num1 != -1)
            this.m_worksheet[num2 + 1, this.m_firstColumn + 1, num1 + 1, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
          else
            this.m_worksheet[num2 + 1, this.m_firstColumn + 1, this.m_iRow, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
        }
        if (this.m_pageBreaks && !flag1 && this.SubTotalColumnIndex(this.m_recordTable.GetOrCreateRow(this.m_iRow + 1, this.m_height, false, this.m_version)) == -1)
          this.m_hPageBreaks.Add(this.m_worksheet[this.m_iRow + 2, 1]);
        if (cellPositionFormat != null)
        {
          IRange range4 = this.m_worksheet[cellPositionFormat.Row + 2, cellPositionFormat.Column + 1];
          if (range4 != null && this.m_worksheet.GetCellType(cellPositionFormat.Row + 2, cellPositionFormat.Column + 1, false) == WorksheetImpl.TRangeValueType.Formula)
          {
            (range4 as RangeImpl).UpdateRowOffSet();
            (range4 as RangeImpl).UpdateFormulas();
          }
          strB = range4.DisplayText;
        }
        if (num1 == -1)
        {
          ++this.m_iRow;
          num2 = this.m_iRow;
          if (!flag1)
          {
            ++this.m_iRow;
            continue;
          }
          break;
        }
        ++this.m_iRow;
        num2 = this.m_iRow + 1;
        for (++this.m_iRow; this.m_iRow < this.m_lastRow; ++this.m_iRow)
        {
          RowStorage row2 = this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version);
          if (row2 == null || !this.HasSubTotal(row2))
          {
            num2 = this.m_iRow;
            break;
          }
        }
        if (this.m_iRow == this.m_lastRow)
        {
          flag1 = true;
          if (this.m_noOfsubTotals > 1 && this.m_summaryBelowData)
          {
            --this.m_iRow;
            this.UpdateRowValue(this.m_iRow + 1);
            this.m_worksheet.InsertRow(this.m_iRow + 1, 1);
            ++this.m_lastRow;
            IRange range5 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
            range5.Value = this.m_grandTotal;
            range5.CellStyle.Font.Bold = true;
            for (int index = 0; index < this.m_totalList.Length; ++index)
            {
              StringBuilder stringBuilder = new StringBuilder(str1);
              stringBuilder.Append(this.m_columnName[index]);
              stringBuilder.Append(this.m_firstRow + 1);
              stringBuilder.Append(':');
              stringBuilder.Append(this.m_columnName[index]);
              stringBuilder.Append(this.m_iRow - 1);
              stringBuilder.Append(')');
              IRange range6 = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
              range6.Formula = stringBuilder.ToString();
              if (bUpdateNumberFormat)
                range6.NumberFormat = this.m_worksheet[range6.Row - 1, range6.Column].NumberFormat;
            }
            break;
          }
        }
        else
          --this.m_iRow;
        num1 = -1;
        if (flag1)
          break;
      }
      ++this.m_iRow;
    }
    if (!flag1 && num2 != -1)
    {
      this.UpdateRowValue(this.m_iRow + 1);
      this.m_worksheet.InsertRow(this.m_iRow + 1, 1);
      ++this.m_lastRow;
      IRange range7 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
      IRange range8 = this.m_worksheet[this.m_iRow, this.m_columnCount + 1];
      string str3 = strB;
      if (str3.StartsWith("="))
      {
        range7.Formula = str3;
        IRange range9 = this.m_worksheet[this.m_iRow - this.m_noOfsubTotals, this.m_columnCount + 1];
        if (this.m_worksheet.GetCellType(this.m_iRow - this.m_noOfsubTotals, this.m_columnCount + 1, false) == WorksheetImpl.TRangeValueType.Formula)
        {
          (range9 as RangeImpl).UpdateRowOffSet();
          (range9 as RangeImpl).UpdateFormulas();
        }
        str3 = range9.DisplayText;
      }
      range7.Value = $"{str3} {this.m_total}";
      range7.CellStyle.Font.Bold = true;
      if (this.m_groupRows)
        this.m_worksheet[num2 + 1, this.m_firstColumn + 1, this.m_iRow, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str1);
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(num2 + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(this.m_iRow);
        stringBuilder.Append(')');
        IRange range10 = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
        range10.Formula = stringBuilder.ToString();
        if (bUpdateNumberFormat)
          range10.NumberFormat = this.m_worksheet[range10.Row - 1, range10.Column].NumberFormat;
      }
      ++this.m_iRow;
      if (!this.m_replace && this.m_noOfsubTotals > 1)
        this.m_iRow += this.m_noOfsubTotals - 1;
      this.UpdateRowValue(this.m_iRow + 1);
      this.m_worksheet.InsertRow(this.m_iRow + 1, 1);
      ++this.m_lastRow;
      IRange range11 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
      range11.Value = this.m_grandTotal;
      range11.CellStyle.Font.Bold = true;
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str1);
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(this.m_firstRow + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(this.m_iRow - 1);
        stringBuilder.Append(')');
        IRange range12 = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
        range12.Formula = stringBuilder.ToString();
        if (bUpdateNumberFormat)
          range12.NumberFormat = this.m_worksheet[range12.Row - 1, range12.Column].NumberFormat;
      }
    }
    if (this.m_groupRows && !flag1)
      this.m_worksheet[this.m_firstRow + 1, this.m_firstColumn + 1, this.m_iRow, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
    if (!this.m_pageBreaks)
      return;
    RowStorage row = this.m_recordTable.GetOrCreateRow(this.m_iRow + 1, this.m_height, false, this.m_version);
    if (this.m_noOfsubTotals != 1 && (row == null || this.SubTotalColumnIndex(row) != -1))
      return;
    this.m_hPageBreaks.Add(this.m_worksheet[this.m_iRow + 2, 1]);
  }

  private void CreateTotalAboveData(bool bUpdateNumberFormat)
  {
    bool flag1 = false;
    int num1 = -1;
    string strB = "";
    int num2 = -1;
    string str = $"=SUBTOTAL({(object) (int) this.m_consolidationFunction}{(object) this.m_worksheet.Application.ArgumentsSeparator}";
    this.m_iRow = this.m_lastRow - 1;
    while (this.m_iRow >= this.m_firstRow)
    {
      bool flag2 = false;
      bool flag3 = false;
      string strA = string.Empty;
      ICellPositionFormat record = this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, true, this.m_version).GetRecord(this.m_groupBy, this.m_blockSize);
      if (record == null || record.TypeCode == TBIFFRecord.Blank)
      {
        if (this.m_replace)
        {
          --this.m_iRow;
          continue;
        }
        if (this.m_iRow == this.m_firstRow)
        {
          flag1 = true;
        }
        else
        {
          if (this.SubTotalColumnIndex(this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version)) != -1)
            flag2 = true;
          if (flag2)
          {
            if (num1 == -1)
              num1 = this.m_iRow;
            if (this.HasSubTotal(this.m_iRow - 1))
              flag3 = true;
          }
          else
          {
            --this.m_iRow;
            continue;
          }
        }
      }
      if (!flag2)
      {
        bool flag4 = true;
        if (record != null)
        {
          IRange range = this.m_worksheet[record.Row + 1, record.Column + 1];
          if (range != null)
          {
            if (this.m_worksheet.GetCellType(record.Row + 1, record.Column + 1, false) == WorksheetImpl.TRangeValueType.Formula)
            {
              (range as RangeImpl).UpdateRowOffSet();
              (range as RangeImpl).UpdateFormulas();
            }
            strA = range.DisplayText;
          }
        }
        if (num2 == -1)
        {
          strB = strA;
          num2 = this.m_iRow < this.m_lastRow ? this.m_lastRow - 1 : this.m_iRow;
          --this.m_iRow;
          continue;
        }
        if (string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) != 0 && this.m_iRow + 1 <= num2)
          flag4 = false;
        if (flag4)
          strB = strA;
        if (flag4)
        {
          --this.m_iRow;
          continue;
        }
      }
      int num3 = flag2 ? this.m_iRow : this.m_iRow + 1;
      int num4 = num2 + 1;
      this.UpdateRowValue(num3 + 1);
      this.m_worksheet.InsertRow(num3 + 1);
      ++this.m_lastRow;
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str);
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(num3 + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(num4);
        stringBuilder.Append(')');
        this.m_worksheet[num3 + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
        IRange range = this.m_worksheet[num3 + 1, this.m_totalList[index] + 1];
        if (bUpdateNumberFormat)
          range.NumberFormat = this.m_worksheet[range.Row + 1, range.Column].NumberFormat;
        if (this.m_pageBreaks && this.SubTotalColumnIndex(this.m_recordTable.GetOrCreateRow(num3 - 1, this.m_height, false, this.m_version)) == -1)
        {
          this.m_hPageBreaks.Add(this.m_worksheet[num3 + 1, 1]);
          int pageBreakIndex = this.m_hPageBreaks.GetPageBreakIndex(this.m_worksheet[num3 + 2, 1]);
          if (pageBreakIndex != -1)
            this.m_hPageBreaks.RemoveAt(pageBreakIndex);
        }
      }
      IRange range1 = this.m_worksheet[num3 + 1, this.m_columnCount + 1];
      range1.Value = $"{strB} {this.m_total}";
      range1.CellStyle.Font.Bold = true;
      if (this.m_groupRows)
      {
        if (flag3)
          this.m_worksheet[num1 + 1, this.m_firstColumn + 1, num4 + 1, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
        else if (num1 != -1)
          this.m_worksheet[num1 + 1 + 1, this.m_firstColumn + 1, num4 + 1, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
        else
          this.m_worksheet[num3 + 1 + 1, this.m_firstColumn + 1, num4 + 1, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
      }
      num1 = -1;
      if (!flag2)
      {
        strB = strA;
        num2 = this.m_iRow;
      }
      else
        num2 = this.m_iRow - 1;
      if (!flag1)
        --this.m_iRow;
      else
        break;
    }
    ++this.m_iRow;
    if (!flag1)
    {
      int num5 = num2 + 1;
      this.UpdateRowValue(this.m_iRow + 1);
      this.m_worksheet.InsertRow(this.m_iRow + 1, 1);
      ++this.m_lastRow;
      IRange range2 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
      range2.Value = $"{strB} {this.m_total}";
      range2.CellStyle.Font.Bold = true;
      if (this.m_groupRows)
        this.m_worksheet[this.m_iRow + 1 + 1, this.m_firstColumn + 1, num5 + 1, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str);
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(this.m_iRow + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.m_columnName[index]);
        stringBuilder.Append(num5);
        stringBuilder.Append(')');
        IRange range3 = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
        range3.Formula = stringBuilder.ToString();
        if (bUpdateNumberFormat)
          range3.NumberFormat = this.m_worksheet[range3.Row + 1, range3.Column].NumberFormat;
        if (this.m_pageBreaks && this.SubTotalColumnIndex(this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version)) == -1)
          this.m_hPageBreaks.Add(this.m_worksheet[this.m_iRow + 1, 1]);
      }
      if (this.m_replace || !this.m_summaryBelowData)
      {
        this.m_isGrandTotalRow = true;
        this.UpdateRowValue(this.m_iRow + 1);
        this.m_isGrandTotalRow = false;
        this.m_worksheet.InsertRow(this.m_iRow + 1, 1);
        ++this.m_lastRow;
        IRange range4 = this.m_worksheet[this.m_iRow + 1, this.m_columnCount + 1];
        range4.Value = this.m_grandTotal;
        range4.CellStyle.Font.Bold = true;
        for (int index = 0; index < this.m_totalList.Length; ++index)
        {
          StringBuilder stringBuilder = new StringBuilder(str);
          stringBuilder.Append(this.m_columnName[index]);
          stringBuilder.Append(this.m_firstRow + 3);
          stringBuilder.Append(':');
          stringBuilder.Append(this.m_columnName[index]);
          stringBuilder.Append(this.m_lastRow);
          stringBuilder.Append(')');
          IRange range5 = this.m_worksheet[this.m_iRow + 1, this.m_totalList[index] + 1];
          range5.Formula = stringBuilder.ToString();
          if (bUpdateNumberFormat)
            range5.NumberFormat = this.m_worksheet[range5.Row + 1, range5.Column].NumberFormat;
          if (this.m_pageBreaks && this.SubTotalColumnIndex(this.m_recordTable.GetOrCreateRow(this.m_iRow, this.m_height, false, this.m_version)) == -1)
            this.m_hPageBreaks.Add(this.m_worksheet[this.m_iRow + 1, 1]);
        }
      }
    }
    if (!this.m_groupRows || flag1)
      return;
    this.m_worksheet[this.m_iRow + 1 + 1, this.m_firstColumn + 1, this.m_lastRow, this.m_lastColumn + 1].Group(ExcelGroupBy.ByRows, false);
  }
}
