// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.SubTotalImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class SubTotalImpl
{
  private bool m_replace;
  private bool m_pageBreaks;
  private bool m_summaryBelowData;
  private bool m_groupRows;
  private ConsolidationFunction consolidationFunction;
  private int[] m_totalList;
  private int irow;
  private int m_groupBy;
  private int columnCount;
  private RecordTable m_recordTable;
  private string[] columnName;
  private string total = "Total";
  private string grandTotal = "Grand Total";
  private int m_height;
  private OfficeVersion m_version;
  private int blockSize;
  private WorksheetImpl m_worksheet;
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
    this.blockSize = worksheet.AppImplementation.RowStorageAllocationBlockSize;
    this.m_version = worksheet.Version;
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
    bool summaryBelowData)
  {
    this.m_firstRow = firstRow;
    this.m_firstColumn = firstColumn;
    this.m_lastRow = lastRow;
    this.m_lastColumn = lastColumn;
    this.m_noOfsubTotals = noOfSubtotal;
    this.m_groupBy = groupBy;
    this.consolidationFunction = function;
    this.m_totalList = totalList;
    this.m_replace = replace;
    this.m_pageBreaks = pageBreaks;
    this.m_summaryBelowData = summaryBelowData;
    this.total = SubTotalImpl.GetEnumerationString(function);
    this.grandTotal = "Grand " + this.total;
    this.columnCount = firstColumn + groupBy;
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
          this.columnCount = firstColumn + index1;
          break;
        }
      }
      if (!flag2)
      {
        this.columnCount = firstColumn;
        this.m_worksheet.InsertColumn(firstColumn + 1);
        ++firstColumn;
        ++lastColumn;
      }
    }
    this.columnName = new string[totalList.Length];
    for (int index = 0; index < totalList.Length; ++index)
    {
      this.columnName[index] = RangeImpl.GetColumnName(totalList[index] + firstColumn + 1);
      totalList[index] += firstColumn;
    }
    this.irow = firstRow;
    this.irow = firstRow;
    this.m_groupBy = firstColumn + groupBy;
    if (!replace)
    {
      summaryBelowData = !this.HasSubTotal(firstRow);
    }
    else
    {
      for (this.irow = firstRow; this.irow <= lastRow; ++this.irow)
      {
        RowStorage row = this.m_recordTable.GetOrCreateRow(this.irow, this.m_height, false, this.m_version);
        if (row != null)
        {
          for (int index = 0; index < totalList.Length; ++index)
          {
            ICellPositionFormat record = row.GetRecord(totalList[index], this.blockSize);
            if (record != null && record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
            {
              this.m_worksheet.DeleteRow(this.irow + 1);
              --lastRow;
              break;
            }
          }
        }
      }
    }
    if (summaryBelowData)
      this.CreateTotalBelowData();
    else
      this.CreateTotalAboveData();
    return this.m_lastRow;
  }

  private int SubTotalColumnIndex(RowStorage rowStorage)
  {
    int num = -1;
    for (int firstColumn = this.m_firstColumn; firstColumn <= this.m_lastColumn; ++firstColumn)
    {
      ICellPositionFormat record = rowStorage.GetRecord(firstColumn, this.blockSize);
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
        ICellPositionFormat record = row.GetRecord(firstColumn, this.blockSize);
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
      ICellPositionFormat record = rowStorage.GetRecord(firstColumn, this.blockSize);
      if (record != null && record.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record, false).StartsWith("=SUBTOTAL("))
        return true;
    }
    return false;
  }

  private void CreateTotalBelowData()
  {
    bool flag1 = false;
    int lastRow = -1;
    string strB = "";
    int num1 = -1;
    string str1 = $"=SUBTOTAL({(object) (int) this.consolidationFunction},";
    for (this.irow = this.m_firstRow; this.irow < this.m_lastRow; ++this.irow)
    {
      bool flag2 = false;
      RowStorage row1 = this.m_recordTable.GetOrCreateRow(this.irow, this.m_height, false, this.m_version);
      if (row1 != null)
      {
        ICellPositionFormat record = row1.GetRecord(this.m_groupBy, this.blockSize);
        if (record == null || record.TypeCode == TBIFFRecord.Blank)
        {
          if (!this.m_replace)
          {
            if (this.m_lastRow == this.irow)
            {
              flag1 = true;
            }
            else
            {
              if (this.SubTotalColumnIndex(row1) != -1)
              {
                lastRow = this.irow;
                flag2 = true;
              }
              if (!flag2)
                continue;
            }
          }
          else
            continue;
        }
        if (!flag2)
        {
          string strA = this.m_worksheet.GetValue(record, true);
          if (num1 == -1)
          {
            strB = strA;
            num1 = this.irow > this.m_firstRow ? this.m_firstRow : this.irow;
            continue;
          }
          if (string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) != 0 && num1 + 1 <= this.irow)
          {
            if (!this.m_replace && this.HasSubTotal(this.irow))
            {
              if (lastRow == -1)
                lastRow = this.irow + 1;
              if (this.irow == this.m_lastRow)
                flag1 = true;
              else
                continue;
            }
          }
          else
          {
            strB = strA;
            continue;
          }
        }
        this.m_worksheet.InsertRow(this.irow + 1);
        ++this.m_lastRow;
        for (int index = 0; index < this.m_totalList.Length; ++index)
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Append(this.columnName[index]);
          stringBuilder.Append(num1 + 1);
          stringBuilder.Append(':');
          stringBuilder.Append(this.columnName[index]);
          stringBuilder.Append(this.irow);
          stringBuilder.Append(')');
          this.m_worksheet[this.irow + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
        }
        IRange range1 = this.m_worksheet[this.irow + 1, this.columnCount + 1];
        IRange range2 = this.m_worksheet[this.irow, this.columnCount + 1];
        string str2 = !range2.HasDateTime ? (!range2.HasFormula ? strB : range2.DisplayText) : range2.Value;
        if (str2.StartsWith("="))
        {
          range1.Formula = str2;
          str2 = this.m_worksheet[this.irow - this.m_noOfsubTotals, this.columnCount + 1].DisplayText;
        }
        range1.Value = $"{str2} {this.total}";
        range1.CellStyle.Font.Bold = true;
        if (this.m_groupRows)
        {
          if (lastRow != -1)
            this.m_worksheet[num1 + 1, this.m_firstColumn + 1, lastRow, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
          else
            this.m_worksheet[num1 + 1, this.m_firstColumn + 1, this.irow, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
        }
        if (this.m_pageBreaks)
        {
          int num2 = flag1 ? 1 : 0;
        }
        strB = this.m_worksheet.GetValue(record, true);
        if (lastRow != -1)
        {
          ++this.irow;
          num1 = this.irow + 1;
          for (++this.irow; this.irow < this.m_lastRow; ++this.irow)
          {
            RowStorage row2 = this.m_recordTable.GetOrCreateRow(this.irow, this.m_height, false, this.m_version);
            if (row2 == null || !this.HasSubTotal(row2))
            {
              num1 = this.irow;
              break;
            }
          }
          if (this.irow == this.m_lastRow)
            flag1 = true;
          else
            --this.irow;
        }
        else
        {
          ++this.irow;
          num1 = this.irow;
        }
        lastRow = -1;
        if (flag1)
          break;
      }
    }
    if (!flag1 && num1 != -1)
    {
      this.m_worksheet.InsertRow(this.irow + 1, 1);
      IRange range3 = this.m_worksheet[this.irow + 1, this.columnCount + 1];
      IRange range4 = this.m_worksheet[this.irow, this.columnCount + 1];
      string str3 = !range4.HasDateTime ? (!range4.HasFormula ? strB : range4.DisplayText) : range4.Value;
      if (str3.StartsWith("="))
      {
        range3.Formula = str3;
        str3 = this.m_worksheet[this.irow - this.m_noOfsubTotals, this.columnCount + 1].DisplayText;
      }
      range3.Value = $"{str3} {this.total}";
      range3.CellStyle.Font.Bold = true;
      if (this.m_groupRows)
        this.m_worksheet[num1 + 1, this.m_firstColumn + 1, this.irow, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str1);
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(num1 + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(this.irow);
        stringBuilder.Append(')');
        this.m_worksheet[this.irow + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
      }
      ++this.irow;
      if (!this.m_replace && this.m_noOfsubTotals > 1)
        this.irow += this.m_noOfsubTotals - 1;
      this.m_worksheet.InsertRow(this.irow + 1, 1);
      IRange range5 = this.m_worksheet[this.irow + 1, this.columnCount + 1];
      range5.Value = this.grandTotal;
      range5.CellStyle.Font.Bold = true;
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str1);
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(this.m_firstRow + 1);
        stringBuilder.Append(':');
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(this.m_lastRow + 1);
        stringBuilder.Append(')');
        this.m_worksheet[this.irow + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
      }
    }
    if (!this.m_groupRows || flag1)
      return;
    this.m_worksheet[this.m_firstRow + 1, this.m_firstColumn + 1, this.irow, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
  }

  private void CreateTotalAboveData()
  {
    bool flag1 = false;
    int num1 = -1;
    string str1 = "";
    int num2 = -1;
    string str2 = $"=SUBTOTAL({(object) (int) this.consolidationFunction},";
    for (this.irow = this.m_lastRow; this.irow >= this.m_firstRow; --this.irow)
    {
      ICellPositionFormat record1 = this.m_recordTable.GetOrCreateRow(this.irow, this.m_height, true, this.m_version).GetRecord(this.m_groupBy, this.blockSize);
      if (record1 != null && record1.TypeCode != TBIFFRecord.Blank)
      {
        string str3 = this.m_worksheet.GetValue(record1, true);
        if (num2 == -1)
        {
          str1 = str3;
          num2 = this.irow < this.m_lastRow ? this.m_lastRow : this.irow;
        }
        else if (!(str3 == str1))
        {
          if (!this.m_replace)
          {
            RowStorage row = this.m_recordTable.GetOrCreateRow(this.irow, this.m_height, false, this.m_version);
            bool flag2 = false;
            for (int index = 0; index < this.m_totalList.Length; ++index)
            {
              ICellPositionFormat record2 = row.GetRecord(this.m_totalList[index], this.blockSize);
              if (record2 != null && record2.TypeCode == TBIFFRecord.Formula && this.m_worksheet.GetValue(record2, false).StartsWith("=SUBTOTAL("))
              {
                flag2 = true;
                break;
              }
            }
            if (flag2)
            {
              if (num1 == -1)
                num1 = this.irow;
              if (!this.m_summaryBelowData && this.irow == this.m_firstRow)
                flag1 = true;
              else
                continue;
            }
          }
          int num3 = this.irow + 1;
          int num4 = num2 + 1;
          this.m_worksheet.InsertRow(num3 + 1);
          ++this.m_lastRow;
          for (int index = 0; index < this.m_totalList.Length; ++index)
          {
            StringBuilder stringBuilder = new StringBuilder(str2);
            stringBuilder.Append(this.columnName[index]);
            stringBuilder.Append(num3 + 2);
            stringBuilder.Append(':');
            stringBuilder.Append(this.columnName[index]);
            stringBuilder.Append(num4 + 1);
            stringBuilder.Append(')');
            this.m_worksheet[num3 + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
          }
          IRange range = this.m_worksheet[num3 + 1, this.columnCount + 1];
          range.Value = $"{str1} {this.total}";
          range.CellStyle.Font.Bold = true;
          if (this.m_groupRows)
          {
            if (num1 != -1)
              this.m_worksheet[num1 + 2 + 1, this.m_firstColumn + 1, num4 + 1, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
            else
              this.m_worksheet[num3 + 1 + 1, this.m_firstColumn + 1, num4 + 1, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
          }
          num1 = -1;
          if (this.m_pageBreaks)
          {
            int num5 = flag1 ? 1 : 0;
          }
          str1 = str3;
          num2 = this.irow;
          if (flag1)
            break;
        }
      }
    }
    ++this.irow;
    if (!flag1)
    {
      int num6 = num2 + 1;
      this.m_worksheet.InsertRow(this.irow + 1, 1);
      ++this.m_lastRow;
      IRange range1 = this.m_worksheet[this.irow + 1, this.columnCount + 1];
      range1.Value = $"{str1} {this.total}";
      range1.CellStyle.Font.Bold = true;
      if (this.m_groupRows)
        this.m_worksheet[this.irow + 1 + 1, this.m_firstColumn + 1, num6 + 1, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
      for (int index = 0; index < this.m_totalList.Length; ++index)
      {
        StringBuilder stringBuilder = new StringBuilder(str2);
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(this.irow + 2);
        stringBuilder.Append(':');
        stringBuilder.Append(this.columnName[index]);
        stringBuilder.Append(num6 + 1);
        stringBuilder.Append(')');
        this.m_worksheet[this.irow + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
      }
      if (this.m_replace)
      {
        this.m_worksheet.InsertRow(this.irow + 1, 1);
        ++this.m_lastRow;
        IRange range2 = this.m_worksheet[this.irow + 1, this.columnCount + 1];
        range2.Value = this.grandTotal;
        range2.CellStyle.Font.Bold = true;
        for (int index = 0; index < this.m_totalList.Length; ++index)
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Append(this.columnName[index]);
          stringBuilder.Append(this.m_firstRow + 2);
          stringBuilder.Append(':');
          stringBuilder.Append(this.columnName[index]);
          stringBuilder.Append(this.m_lastRow + 1);
          stringBuilder.Append(')');
          this.m_worksheet[this.irow + 1, this.m_totalList[index] + 1].Formula = stringBuilder.ToString();
        }
      }
    }
    if (!this.m_groupRows)
      return;
    this.m_worksheet[this.m_firstRow + 1 + 1, this.m_firstColumn + 1, this.m_lastRow + 1, this.m_lastColumn + 1].Group(OfficeGroupBy.ByRows, false);
  }

  private static string GetEnumerationString(ConsolidationFunction consolidationFunction)
  {
    switch (consolidationFunction)
    {
      case ConsolidationFunction.Average:
        return ConsolidationFunction.Average.ToString();
      case ConsolidationFunction.CountNums:
      case ConsolidationFunction.Count:
        return ConsolidationFunction.Count.ToString();
      case ConsolidationFunction.Max:
        return ConsolidationFunction.Max.ToString();
      case ConsolidationFunction.Min:
        return ConsolidationFunction.Min.ToString();
      case ConsolidationFunction.Product:
        return ConsolidationFunction.Product.ToString();
      case ConsolidationFunction.StdDev:
        return ConsolidationFunction.StdDev.ToString();
      case ConsolidationFunction.StdDevp:
        return ConsolidationFunction.StdDevp.ToString();
      case ConsolidationFunction.Var:
        return ConsolidationFunction.Var.ToString();
      case ConsolidationFunction.Varp:
        return ConsolidationFunction.Varp.ToString();
      default:
        return "Total";
    }
  }
}
