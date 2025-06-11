// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.DataSorter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class DataSorter : IDataSort
{
  private bool m_bIsCaseSensitive;
  private bool m_bHasHeader;
  private SortOrientation m_orientation;
  private ISortFields m_sortFields;
  private IRange m_sortRange;
  private SortingAlgorithms m_algorithm;
  private IWorkbook m_workBook;
  private IWorksheet m_worksheet;
  private ISortingAlgorithm m_customAlgorithm;
  private object m_parent;

  private ISortingAlgorithm CustomAlgorithm
  {
    get => this.m_customAlgorithm;
    set => this.m_customAlgorithm = value;
  }

  public bool IsCaseSensitive
  {
    get => this.m_bIsCaseSensitive;
    set => this.m_bIsCaseSensitive = value;
  }

  public bool HasHeader
  {
    get => this.m_bHasHeader;
    set => this.m_bHasHeader = value;
  }

  public SortOrientation Orientation
  {
    get => this.m_orientation;
    set => this.m_orientation = value;
  }

  public ISortFields SortFields
  {
    get => this.m_sortFields;
    set => this.m_sortFields = value;
  }

  public IRange SortRange
  {
    get => this.m_sortRange;
    set
    {
      if ((this.m_workBook as WorkbookImpl).Loading || this.m_parent is IAutoFilters)
        return;
      (this.m_sortFields as Syncfusion.XlsIO.Implementation.Sorting.SortFields).Clear();
      this.UpdateRange(value);
    }
  }

  public SortingAlgorithms Algorithm
  {
    get => this.m_algorithm;
    set => this.m_algorithm = value;
  }

  internal IWorksheet Worksheet => this.m_worksheet;

  internal DataSorter(object parentObject)
  {
    this.m_parent = parentObject;
    IWorkbook workbook = (IWorkbook) (parentObject as WorkbookImpl);
    if (workbook != null)
    {
      this.m_worksheet = workbook.ActiveSheet;
      this.m_workBook = workbook;
    }
    else
    {
      switch (parentObject)
      {
        case IWorksheet _:
          this.m_worksheet = parentObject as IWorksheet;
          this.m_workBook = this.m_worksheet.Workbook;
          break;
        case IAutoFilters _:
          this.m_worksheet = (IWorksheet) (parentObject as AutoFiltersCollection).Worksheet;
          this.m_workBook = (IWorkbook) (this.m_worksheet.Workbook as WorkbookImpl);
          break;
      }
    }
    this.m_sortFields = (ISortFields) new Syncfusion.XlsIO.Implementation.Sorting.SortFields(this.m_workBook.Application, parentObject);
    this.IsCaseSensitive = false;
    this.HasHeader = true;
    this.Orientation = SortOrientation.TopToBottom;
    this.Algorithm = SortingAlgorithms.QuickSort;
  }

  public void Sort()
  {
    if (this.SortRange == null && this.m_parent is IAutoFilters)
      this.UpdateRange((this.m_parent as IAutoFilters).FilterRange);
    if (this.SortRange == null)
      throw new ArgumentNullException("Sort Range");
    if (this.m_parent is WorkbookImpl)
    {
      WorksheetImpl worksheet = this.SortRange.Worksheet as WorksheetImpl;
      this.m_worksheet = (IWorksheet) worksheet;
      worksheet.DataSorter = (IDataSort) this;
      (worksheet.DataSorter as DataSorter).m_parent = (object) worksheet;
      worksheet.DataSorter.Sort();
    }
    int[] iColumns = new int[this.m_sortFields.Count];
    OrderBy[] orderBy = new OrderBy[iColumns.Length];
    Color[] colors = new Color[iColumns.Length];
    int index = 0;
    foreach (ISortField sortField in (IEnumerable) this.m_sortFields)
    {
      if (sortField.SortOn == SortOn.CellColor || sortField.SortOn == SortOn.FontColor)
        colors[index] = sortField.Color;
      iColumns[index] = sortField.Key;
      orderBy[index] = sortField.Order;
      ++index;
    }
    this.SortBy(iColumns, orderBy, colors);
  }

  public void SortBy(int[] iColumns, OrderBy[] orderBy, Color[] colors)
  {
    Type[] typeArray = new Type[iColumns.Length];
    object[][] data = this.Orientation != SortOrientation.TopToBottom ? this.SortableDataColumn(this.SortRange, typeArray, iColumns) : this.SortableData(this.SortRange, typeArray, iColumns, orderBy);
    if (this.m_sortFields[0].SortOn == SortOn.Values)
    {
      bool flag1 = typeArray.Length == 1;
      bool flag2 = false;
      int orientation = (int) this.m_orientation;
      if (!this.IsCaseSensitive && typeArray[0].Name == "String")
      {
        foreach (object[] objArray in data)
        {
          if (objArray[1] != null)
          {
            objArray[1] = (object) objArray[1].ToString().ToLower();
            if (!flag2)
              flag2 = true;
          }
        }
      }
      switch (this.Algorithm)
      {
        case SortingAlgorithms.QuickSort:
          SortingAlgorithm sortingAlgorithm1 = (SortingAlgorithm) new QuickSort3Impl(data, typeArray, orderBy, colors);
          sortingAlgorithm1.Sort(0, data.Length - 1, 1);
          if (flag1 && flag2)
            this.ArrangeSimilarData(sortingAlgorithm1.Data, orderBy[0]);
          if (this.m_orientation == SortOrientation.TopToBottom)
          {
            this.SwapManager(this.SortRange, sortingAlgorithm1.Data);
            break;
          }
          this.SwapManagerColumnWise(this.SortRange, sortingAlgorithm1.Data);
          break;
        case SortingAlgorithms.HeapSort:
          SortingAlgorithm sortingAlgorithm2 = (SortingAlgorithm) new HeapSortImpl(data, typeArray, orderBy, colors);
          sortingAlgorithm2.Sort(0, sortingAlgorithm2.Data.Length, 1);
          if (flag1 && flag2)
            this.ArrangeSimilarData(sortingAlgorithm2.Data, orderBy[0]);
          if (typeArray.Length > 1)
          {
            sortingAlgorithm2 = (SortingAlgorithm) new MergeSortImpl(sortingAlgorithm2.Data, typeArray, orderBy, colors);
            sortingAlgorithm2.Sort(0, sortingAlgorithm2.Data.Length - 1, 1);
          }
          if (this.m_orientation == SortOrientation.TopToBottom)
          {
            this.SwapManager(this.SortRange, sortingAlgorithm2.Data);
            break;
          }
          this.SwapManagerColumnWise(this.SortRange, sortingAlgorithm2.Data);
          break;
        case SortingAlgorithms.MergeSort:
          SortingAlgorithm sortingAlgorithm3 = (SortingAlgorithm) new MergeSortImpl(data, typeArray, orderBy, colors);
          sortingAlgorithm3.Sort(0, sortingAlgorithm3.Data.Length - 1, 1);
          if (flag1 && flag2)
            this.ArrangeSimilarData(sortingAlgorithm3.Data, orderBy[0]);
          if (this.m_orientation == SortOrientation.TopToBottom)
          {
            this.SwapManager(this.SortRange, sortingAlgorithm3.Data);
            break;
          }
          this.SwapManagerColumnWise(this.SortRange, sortingAlgorithm3.Data);
          break;
        case SortingAlgorithms.InsertionSort:
          SortingAlgorithm sortingAlgorithm4 = (SortingAlgorithm) new InsertionSortImpl(data, typeArray, orderBy, colors);
          sortingAlgorithm4.Sort(0, data.Length - 1, 1);
          if (flag1 && flag2)
            this.ArrangeSimilarData(sortingAlgorithm4.Data, orderBy[0]);
          if (this.m_orientation == SortOrientation.TopToBottom)
          {
            this.SwapManager(this.SortRange, sortingAlgorithm4.Data);
            break;
          }
          this.SwapManagerColumnWise(this.SortRange, sortingAlgorithm4.Data);
          break;
      }
    }
    else
    {
      SortingAlgorithm sortingAlgorithm = (SortingAlgorithm) new StyleSorting(data, typeArray, orderBy, colors);
      sortingAlgorithm.Sort(0, sortingAlgorithm.Data.Length - 1, 1);
      for (int index = 1; index < this.m_sortFields.Count; ++index)
      {
        if (this.m_sortFields[index].SortOn != SortOn.Values)
          sortingAlgorithm.Sort(0, sortingAlgorithm.Data.Length - 1, index + 1);
      }
      if (this.m_orientation == SortOrientation.TopToBottom)
        this.SwapManager(this.SortRange, sortingAlgorithm.Data);
      else
        this.SwapManagerColumnWise(this.SortRange, sortingAlgorithm.Data);
    }
  }

  private void ArrangeSimilarData(object[][] data, OrderBy order)
  {
    int num1 = 0;
    int num2 = 0;
    List<object> objectList = new List<object>();
    for (int index1 = 1; index1 <= data.Length; ++index1)
    {
      if (index1 != data.Length)
      {
        if (data[index1][1] != null && data[index1][1].Equals(data[index1 - 1][1]))
        {
          if ((int) data[index1 - 1][0] > (int) data[index1][0])
          {
            object[] objArray = data[index1 - 1];
            data[index1 - 1] = data[index1];
            data[index1] = objArray;
          }
          objectList.Add(data[index1 - 1][0]);
          ++num1;
          if (num1 == 1)
            num2 = index1 - 1;
        }
        else
        {
          if (num1 > 1)
          {
            objectList.Add(data[index1 - 1][0]);
            objectList.Sort();
            for (int index2 = 0; index2 < objectList.Count; ++index2)
              data[num2 + index2][0] = objectList[index2];
          }
          objectList.Clear();
          num1 = 0;
          num2 = 0;
        }
      }
      else if (objectList.Count > 1)
      {
        objectList.Add(data[index1 - 1][0]);
        objectList.Sort();
        for (int index3 = 0; index3 < objectList.Count - 1; ++index3)
          data[num2 + index3][0] = objectList[index3];
        objectList.Clear();
      }
    }
  }

  internal object[][] SortableData(
    IRange range,
    Type[] columnTypes,
    int[] iColumns,
    OrderBy[] orderBy)
  {
    int row = range.Row;
    IWorksheet worksheet = this.m_worksheet;
    int lastRow = range.LastRow;
    if (this.HasHeader && row < lastRow)
      ++row;
    int column = range.Column;
    int lastColumn = range.LastColumn;
    object[][] objArray = new object[lastRow - row + 1][];
    int index1 = 0;
    int index2 = 0;
    int[] numArray = new int[columnTypes.Length];
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(range.Worksheet.Application, range.Worksheet);
    foreach (int iColumn in iColumns)
    {
      migrantRangeImpl.ResetRowColumn(row, iColumn + 1);
      int iType;
      columnTypes[index2] = this.GetColumnType((IRange) migrantRangeImpl, out iType);
      numArray[index2++] = iType;
    }
    Syncfusion.XlsIO.Implementation.Sorting.SortFields sortFields = this.m_sortFields as Syncfusion.XlsIO.Implementation.Sorting.SortFields;
    int iRow = row;
    int num = 0;
    while (iRow <= lastRow)
    {
      objArray[index1] = new object[iColumns.Length + 1];
      objArray[index1][0] = (object) num;
      if (worksheet.GetRowHeight(iRow) == 0.0)
      {
        objArray[index1][1] = (object) "";
      }
      else
      {
        int index3 = 1;
        foreach (int iColumn in iColumns)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn + 1);
          switch (this.m_sortFields[sortFields.FindByKey(iColumn)].SortOn)
          {
            case SortOn.Values:
              object obj = this.GetValue((IRange) migrantRangeImpl, numArray[index3 - 1]);
              if (migrantRangeImpl.Value == string.Empty)
                obj = this.CheckMaxValue(obj, orderBy[index3 - 1]);
              objArray[index1][index3] = obj;
              break;
            case SortOn.CellColor:
              objArray[index1][index3] = (object) migrantRangeImpl.CellStyle.Color;
              break;
            case SortOn.FontColor:
              objArray[index1][index3] = (object) migrantRangeImpl.CellStyle.Font.RGBColor;
              break;
          }
          ++index3;
        }
      }
      ++iRow;
      ++index1;
      ++num;
    }
    return objArray;
  }

  private object CheckMaxValue(object value, OrderBy order)
  {
    value = !object.Equals(value, (object) DateTime.MinValue) ? (!object.Equals(value, (object) double.NaN) ? (order != OrderBy.Ascending ? (object) char.MinValue.ToString() : (object) 'ÿ'.ToString()) : (order != OrderBy.Ascending ? (object) double.MinValue : (object) double.MaxValue)) : (order != OrderBy.Ascending ? (object) DateTime.MinValue : (object) DateTime.MaxValue);
    return value;
  }

  internal object[][] SortableDataColumn(IRange range, Type[] rowTypes, int[] iRows)
  {
    int row = range.Row;
    int lastRow = range.LastRow;
    int column = range.Column;
    if (this.HasHeader)
      ++column;
    int length1 = range.LastColumn - column + 1;
    int length2 = iRows.Length;
    object[][] objArray = new object[length1][];
    int index1 = 0;
    int[] numArray = new int[rowTypes.Length];
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(range.Worksheet.Application, range.Worksheet);
    foreach (int iRow in iRows)
    {
      migrantRangeImpl.ResetRowColumn(iRow + 1, column);
      int iType;
      rowTypes[index1] = this.GetColumnType((IRange) migrantRangeImpl, out iType);
      numArray[index1++] = iType;
    }
    int num1 = 0;
    for (int index2 = 0; index2 < length2; ++index2)
    {
      int num2 = column;
      int iRow = iRows[index2];
      for (int index3 = 0; index3 < length1; ++index3)
      {
        migrantRangeImpl.ResetRowColumn(iRow + 1, num2++);
        objArray[index3] = new object[length2 + 1];
        objArray[index3][index2] = (object) num1++;
        switch (this.m_sortFields[index2].SortOn)
        {
          case SortOn.Values:
            objArray[index3][index2 + 1] = this.GetValue((IRange) migrantRangeImpl, numArray[index2]);
            break;
          case SortOn.CellColor:
            objArray[index3][index2 + 1] = (object) migrantRangeImpl.CellStyle.Color;
            break;
          case SortOn.FontColor:
            objArray[index3][index2 + 1] = (object) migrantRangeImpl.CellStyle.Font.RGBColor;
            break;
        }
        ++index1;
      }
    }
    return objArray;
  }

  internal object GetValue(IRange range, int type)
  {
    switch (type)
    {
      case 1:
        return (object) range.FormulaDateTime;
      case 2:
        return (object) range.FormulaNumberValue;
      case 3:
        return (object) range.FormulaStringValue;
      case 4:
        return (object) range.Number;
      case 5:
        return (object) range.DateTime;
      case 6:
        return (object) range.DisplayText;
      default:
        return (object) range.Value;
    }
  }

  internal void SwapManager(IRange range, object[][] sortedResult)
  {
    int startRow1 = 0;
    int row = range.Row;
    int lastRow = range.LastRow;
    if (this.HasHeader && startRow1 < lastRow)
      ++row;
    int num = lastRow - row + 1;
    int column = range.Column;
    IWorksheet worksheet = range.Worksheet;
    for (int index1 = 1; index1 <= num; ++index1)
    {
      if (worksheet.GetRowHeight(index1 + row - 1) == 0.0)
      {
        int index2 = sortedResult.Length - 1;
        while ((int) sortedResult[index2][0] != index1 - 1)
          --index2;
        for (; index2 >= index1; --index2)
        {
          sortedResult[index2][0] = sortedResult[index2 - 1][0];
          sortedResult[index2][1] = sortedResult[index2 - 1][1];
        }
        if (index2 == index1 - 1)
        {
          sortedResult[index2][0] = (object) (index1 - 1);
          sortedResult[index2][1] = (object) worksheet[index1 + row - 1, 1].DisplayText;
        }
      }
    }
    if ((int) sortedResult[0][0] != -1)
      this.SwapManager(range, sortedResult, startRow1);
    for (int startRow2 = 1; startRow2 < sortedResult.Length; ++startRow2)
    {
      if (sortedResult[startRow2] != null && (int) sortedResult[startRow2][0] != -1)
        this.SwapManager(range, sortedResult, startRow2);
    }
  }

  internal void SwapManagerColumnWise(IRange range, object[][] sortedResult)
  {
    int startRow1 = 0;
    if ((int) sortedResult[0][0] != -1)
      this.SwapManagerColumnWise(range, sortedResult, startRow1);
    for (int startRow2 = 0; startRow2 < sortedResult.Length; ++startRow2)
    {
      if ((int) sortedResult[startRow2][0] != -1)
        this.SwapManagerColumnWise(range, sortedResult, startRow2);
    }
  }

  internal void SwapManagerColumnWise(IRange range, object[][] sortedResult, int startRow)
  {
    int row = range.Row;
    int lastRow = range.LastRow;
    int column = range.Column;
    if (this.HasHeader)
      ++column;
    int lastColumn = range.LastColumn;
    IWorksheet worksheet = range.Worksheet;
    int num1 = lastColumn + 1;
    worksheet.InsertColumn(num1);
    IRange destination = worksheet[row, num1, lastRow, num1];
    worksheet[row, column + startRow, lastRow, column + startRow].CopyTo(destination, ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges | ExcelCopyRangeOptions.CopyStyles | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations);
    int index = startRow;
    int num2;
    do
    {
      num2 = (int) sortedResult[index][0];
      if (num2 != startRow && num2 != -1)
      {
        worksheet[row, column + num2, lastRow, column + num2].CopyTo(worksheet[row, column + index, lastRow, column + index], ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges | ExcelCopyRangeOptions.CopyStyles | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations);
        sortedResult[index][0] = (object) -1;
        index = num2;
      }
      else
        break;
    }
    while (num2 != startRow);
    destination.CopyTo(worksheet[row, column + index, lastRow, column + index]);
    worksheet.DeleteColumn(num1);
    sortedResult[index][0] = (object) -1;
  }

  internal void SwapManager(IRange range, object[][] sortedResult, int startRow)
  {
    int row1 = range.Row;
    int lastRow = range.LastRow;
    if (this.HasHeader && row1 < lastRow)
      ++row1;
    int column = range.Column;
    int lastColumn = range.LastColumn;
    IWorksheet worksheet = range.Worksheet;
    int num1 = range.LastRow + 1;
    RowStorage row2 = WorksheetHelper.GetOrCreateRow(this.m_worksheet as IInternalWorksheet, row1 + startRow - 1, false);
    if (row2 == null || row2.IsHidden)
      return;
    IRange destination = worksheet[num1, column, num1, lastColumn];
    worksheet.InsertRow(num1);
    worksheet[row1 + startRow, column, row1 + startRow, lastColumn].CopyTo(destination, ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges | ExcelCopyRangeOptions.CopyStyles | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations);
    int index = startRow;
    while (sortedResult[index] != null)
    {
      int num2 = (int) sortedResult[index][0];
      if (num2 != startRow && num2 != -1)
      {
        worksheet[num2 + row1, column, num2 + row1, lastColumn].CopyTo(worksheet[index + row1, column, index + row1, lastColumn], ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges | ExcelCopyRangeOptions.CopyStyles | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations);
        sortedResult[index][0] = (object) -1;
        index = num2;
        if (num2 == startRow)
          break;
      }
      else
        break;
    }
    destination.CopyTo(worksheet[index + row1, column, index + row1, lastColumn], ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.UpdateMerges | ExcelCopyRangeOptions.CopyStyles | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations);
    worksheet.DeleteRow(destination.Row);
    if (sortedResult[index] == null)
      return;
    sortedResult[index][0] = (object) -1;
  }

  internal Type GetColumnType(IRange range, out int iType)
  {
    if (range.HasFormula)
    {
      if (range.HasFormulaDateTime)
      {
        iType = 1;
        return typeof (DateTime);
      }
      if (range.HasFormulaStringValue)
      {
        iType = 3;
        return typeof (string);
      }
      if (range.HasFormulaNumberValue)
      {
        iType = 2;
        return typeof (double);
      }
    }
    if (range.HasNumber)
    {
      iType = 4;
      return typeof (double);
    }
    if (range.HasString)
    {
      iType = 6;
      return typeof (string);
    }
    if (range.HasDateTime)
    {
      iType = 5;
      return typeof (DateTime);
    }
    iType = 6;
    return typeof (string);
  }

  internal void UpdateRange(IRange value)
  {
    if (!(this.m_parent is WorkbookImpl) && value != null && value.Worksheet != this.Worksheet)
      throw new ArgumentOutOfRangeException("Can't Sort ranges from another worksheet");
    this.m_sortRange = value;
  }
}
