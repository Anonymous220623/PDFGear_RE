// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.AutoFiltersCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Tables;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class AutoFiltersCollection : CollectionBaseEx<object>, IAutoFilters, IParentApplication
{
  public const string DEF_AUTOFILTER_NAMEDRANGE = "_FilterDatabase";
  public const string DEF_EXCEL07_AUTOFILTER_NAMEDRANGE = "_xlnm._FilterDatabase";
  private IRange m_range;
  private WorksheetImpl m_worksheet;
  private IListObject m_listObject;
  private int m_topRow;
  private int m_leftColumn;
  private int m_bottomRow;
  private int m_rightColumn;
  private bool m_hasAdjacents;
  private IDataSort m_dataSorter;

  public AutoFiltersCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.Cleared += new CollectionBaseEx<object>.CollectionClear(this.AutoFiltersCollection_Cleared);
  }

  private void SetParents()
  {
    if (!(this.Parent is ListObject))
    {
      this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
      if (this.m_worksheet == null)
        throw new ArgumentException("Can't find parent worksheet.", "parent");
    }
    else
    {
      this.m_listObject = this.Parent as IListObject;
      this.m_worksheet = this.m_listObject.Worksheet as WorksheetImpl;
      if (this.m_listObject == null)
        throw new ArgumentException("Can't find parent ListObject.", "parent");
    }
  }

  public IRange FilterRange
  {
    get
    {
      return this.m_range != null && (this.Count > 0 || (this.m_range.Worksheet.Workbook as WorkbookImpl).Loading || (this.m_range.Worksheet.Workbook as WorkbookImpl).Saving) ? this.m_range : (IRange) null;
    }
    set
    {
      if (this.Parent is IListObject)
        return;
      this.UpdateRange(value);
    }
  }

  public IAutoFilter this[int columnIndex] => (IAutoFilter) this.InnerList[columnIndex];

  public string AddressR1C1
  {
    get
    {
      INames names = this.Worksheet.Names;
      return names.Contains("_FilterDatabase") ? names["_FilterDatabase"].ValueR1C1 : throw new ApplicationException("cannot find filtered range.");
    }
  }

  public IDataSort DataSorter
  {
    get
    {
      if (this.m_dataSorter == null)
      {
        this.m_dataSorter = (IDataSort) new Syncfusion.XlsIO.Implementation.Sorting.DataSorter((object) this);
        this.Worksheet.Workbook.CreateDataSorter();
      }
      return this.m_dataSorter;
    }
    internal set => this.m_dataSorter = value;
  }

  public WorksheetImpl Worksheet => this.m_worksheet;

  public bool IsFiltered
  {
    get
    {
      if (this.Count == 0)
        return false;
      for (int columnIndex = 0; columnIndex < this.Count; ++columnIndex)
      {
        if (this[columnIndex].IsFiltered)
          return true;
      }
      return false;
    }
  }

  public string DefaultNamedRangeName
  {
    get
    {
      switch (this.Worksheet.Version)
      {
        case ExcelVersion.Excel97to2003:
          return "_FilterDatabase";
        case ExcelVersion.Excel2007:
        case ExcelVersion.Excel2010:
        case ExcelVersion.Excel2013:
        case ExcelVersion.Excel2016:
        case ExcelVersion.Xlsx:
          return "_xlnm._FilterDatabase";
        default:
          throw new ArgumentOutOfRangeException("Unexpected excel version");
      }
    }
  }

  public void Parse(System.Collections.Generic.List<BiffRecordRaw> records)
  {
    if (records == null || records.Count == 0)
      return;
    int columnIndex = 0;
    if (!this.m_worksheet.Names.Contains("_FilterDatabase"))
      return;
    this.m_range = this.m_worksheet.Names["_FilterDatabase"].RefersToRange;
    if (this.m_range == null)
      return;
    int iRowIndex = 0;
    int iColumnIndex = 0;
    if (this.m_range != null)
    {
      iRowIndex = this.m_range.Row;
      iColumnIndex = this.m_range.Column;
      int num = 0;
      for (int index = this.m_range.LastColumn - this.m_range.Column + 1; num < index; ++num)
      {
        AutoFilterImpl autoFilterImpl = new AutoFilterImpl(this);
        autoFilterImpl.m_colIndex = iColumnIndex + num;
        this.InnerList.Add((object) autoFilterImpl);
        autoFilterImpl.Index = this.InnerList.Count - 1;
      }
    }
    int index1 = 0;
    for (int count = records.Count; index1 < count; ++index1)
    {
      BiffRecordRaw record1 = records[index1];
      switch (record1.TypeCode)
      {
        case TBIFFRecord.FilterMode:
          continue;
        case TBIFFRecord.AutoFilterInfo:
          columnIndex = 0;
          continue;
        case TBIFFRecord.AutoFilter:
          AutoFilterRecord record2 = (AutoFilterRecord) record1;
          if (this.InnerList.Count <= columnIndex)
            this.InnerList.Add((object) new AutoFilterImpl(this));
          ((AutoFilterImpl) this[columnIndex]).Parse(record2, iColumnIndex, iRowIndex);
          ++columnIndex;
          continue;
        default:
          throw new ArgumentOutOfRangeException("Unknown record");
      }
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    int count = this.Count;
    if (count == 0)
      return;
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    bool isFiltered = this.IsFiltered;
    if (isFiltered)
    {
      FilterModeRecord record = (FilterModeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.FilterMode);
      records.Add((IBiffStorage) record);
    }
    AutoFilterInfoRecord record1 = (AutoFilterInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.AutoFilterInfo);
    record1.ArrowsCount = (ushort) count;
    records.Add((IBiffStorage) record1);
    if (!isFiltered)
      return;
    for (int columnIndex = 0; columnIndex < count; ++columnIndex)
      ((AutoFilterImpl) this[columnIndex]).Serialize(records);
  }

  private void AutoFiltersCollection_Cleared()
  {
    INames names = this.Worksheet.Names;
    if (!names.Contains("_FilterDatabase"))
      return;
    names.Remove("_FilterDatabase");
  }

  public AutoFiltersCollection Clone(WorksheetImpl parent)
  {
    AutoFiltersCollection filtersCollection = parent != null ? (AutoFiltersCollection) this.Clone((object) parent) : throw new ArgumentNullException(nameof (parent));
    if (this.m_range != null)
      filtersCollection.m_range = parent[this.m_range.AddressLocal];
    return filtersCollection;
  }

  internal AutoFiltersCollection Clone(IListObject parent)
  {
    AutoFiltersCollection filtersCollection = parent != null ? (AutoFiltersCollection) this.Clone((object) parent) : throw new ArgumentNullException(nameof (parent));
    if (parent.Location != null)
      filtersCollection.m_range = parent.Location;
    return filtersCollection;
  }

  public void ChangeVersions(int iLastRow, int iLastColumn, ExcelVersion version)
  {
    if (version != ExcelVersion.Excel97to2003)
    {
      this.m_worksheet.Names["_FilterDatabase"].Name = this.DefaultNamedRangeName;
    }
    else
    {
      int lastColumn = this.FilterRange.LastColumn;
      int lastRow = this.FilterRange.LastRow;
      if (this.FilterRange.Column > iLastColumn || this.FilterRange.Row > iLastRow)
      {
        int columnIndex = 0;
        for (int count = this.Count; columnIndex < count; ++columnIndex)
          ((AutoFilterImpl) this[columnIndex]).Clear();
        this.Clear();
      }
      else
      {
        if (lastColumn > iLastColumn)
          lastColumn = iLastColumn;
        if (lastRow > iLastRow)
          lastRow = iLastRow;
        this.m_worksheet.Names["_xlnm._FilterDatabase"].Name = this.DefaultNamedRangeName;
        this.FilterRange = this.Worksheet[this.FilterRange.Row, this.FilterRange.Column, lastRow, lastColumn];
      }
    }
  }

  public void UpdateFilterRange()
  {
    IName name = this.m_worksheet.Names[this.DefaultNamedRangeName];
    if (name != null)
    {
      if (name.RefersToRange != null && name.RefersToRange.Worksheet == this.m_worksheet && !this.m_worksheet.IsInsertingRow && !this.m_worksheet.IsDeletingRow)
        this.FilterRange = name.RefersToRange;
      else
        this.m_range = name.RefersToRange;
    }
    else
      this.m_range = (IRange) null;
  }

  internal void UpdateRange(IRange value)
  {
    if (value != null && value.Worksheet != this.Worksheet)
      throw new ArgumentOutOfRangeException("Can't filter ranges from another worksheet");
    int columnIndex = 0;
    for (int count = this.Count; columnIndex < count; ++columnIndex)
      ((AutoFilterImpl) this[columnIndex]).Clear();
    if (value != null)
      this.Clear();
    if (!this.Worksheet.IsParsing && !(this.Parent is ListObject))
      value = this.UpdateFilterRange(value);
    this.m_range = value;
    if (!(this.Parent is IListObject))
    {
      INames names = this.Worksheet.Names;
      if (value == null)
      {
        IRange refersToRange = names[this.DefaultNamedRangeName].RefersToRange;
        for (int row1 = refersToRange.Row; row1 <= refersToRange.LastRow; ++row1)
        {
          RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.Worksheet, row1 - 1, true);
          if (row2.IsFilteredRow)
          {
            this.Worksheet.ShowRow(row1, true);
            row2.IsFilteredRow = false;
          }
        }
        names.Remove(this.DefaultNamedRangeName);
      }
      else if (names.Contains(this.DefaultNamedRangeName))
      {
        names[this.DefaultNamedRangeName].RefersToRange = this.m_range;
      }
      else
      {
        IName name = names.Add(this.DefaultNamedRangeName, this.m_range);
        name.Visible = false;
        ((NameImpl) name).IsBuiltIn = true;
      }
    }
    this.InnerList.Clear();
    if (value == null)
      return;
    int row = this.m_range.Row;
    int num = value.Column;
    int iLastColumn;
    for (int lastColumn = value.LastColumn; num <= lastColumn; num = iLastColumn + 1)
    {
      IRange mergeArea = this.m_worksheet[row, num].MergeArea;
      iLastColumn = mergeArea != null ? mergeArea.LastColumn : num;
      this.InnerList.Add((object) new AutoFilterImpl(this, num, iLastColumn, row));
      AutoFilterImpl inner = (AutoFilterImpl) this.InnerList[this.InnerList.Count - 1];
      inner.Index = this.InnerList.Count - 1;
      inner.m_colIndex = num;
    }
  }

  private IRange UpdateFilterRange(IRange filterRange)
  {
    if (filterRange != null)
    {
      this.InitializeFilterRange(filterRange.Row, filterRange.Column, filterRange.LastRow, filterRange.LastColumn);
      int topRow = this.m_topRow;
      int leftColumn = this.m_leftColumn;
      int bottomRow = this.m_bottomRow;
      int rightColumn = this.m_rightColumn;
      if ((filterRange as RangeImpl).IsSingleCell)
      {
        bool flag = false;
        if ((filterRange.Worksheet[this.m_topRow, this.m_leftColumn] as RangeImpl).CellType == RangeImpl.TCellType.Blank)
          flag = true;
        if (flag && !this.HasAdjacent(this.m_topRow, this.m_leftColumn, filterRange))
          throw new Exception("This can't be applied to the selected range. Select a single cell in a range and try again.");
        filterRange = this.IncludeAdjacents(this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn, filterRange, true);
        if (flag)
        {
          if (topRow == this.m_topRow && !this.IsRowNotEmpty(this.m_topRow, this.m_leftColumn, this.m_rightColumn, filterRange))
            ++this.m_topRow;
          if (leftColumn == this.m_leftColumn && !this.IsColumnNotEmpty(this.m_leftColumn, this.m_topRow, this.m_bottomRow, filterRange))
            ++this.m_leftColumn;
          if (bottomRow == this.m_bottomRow && !this.IsRowNotEmpty(this.m_bottomRow, this.m_leftColumn, this.m_rightColumn, filterRange))
            --this.m_bottomRow;
          if (rightColumn == this.m_rightColumn && !this.IsColumnNotEmpty(this.m_rightColumn, this.m_topRow, this.m_bottomRow, filterRange))
            --this.m_rightColumn;
          filterRange = filterRange.Worksheet[this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
        }
      }
      else
      {
        IRange usedRange = this.Worksheet.UsedRange;
        if (usedRange != null)
        {
          int row = usedRange.Row;
          int lastRow = usedRange.LastRow;
          int column = usedRange.Column;
          int lastColumn = usedRange.LastColumn;
          if (this.m_topRow < row && this.m_bottomRow >= row)
            this.m_topRow = row;
          if (this.m_bottomRow > lastRow && this.m_topRow <= lastRow)
            this.m_bottomRow = lastRow;
          if (this.m_leftColumn < column && this.m_rightColumn >= column)
            this.m_leftColumn = column;
          if (this.m_rightColumn > lastColumn && this.m_leftColumn <= lastColumn)
            this.m_rightColumn = lastColumn;
          if (this.m_topRow == topRow && this.m_leftColumn == leftColumn && this.m_bottomRow == bottomRow && this.m_rightColumn == rightColumn && (row > this.m_topRow || column > this.m_leftColumn || lastRow < this.m_bottomRow || lastColumn < this.m_rightColumn))
            throw new Exception("This can't be applied to the selected range. Select a single cell in a range and try again.");
          filterRange = filterRange.Worksheet[this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
        }
      }
    }
    return filterRange;
  }

  internal IRange IncludeAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange,
    bool isEnd)
  {
    this.InitializeFilterRange(topRow, leftColumn, bottomRow, rightColumn);
    this.m_hasAdjacents = false;
    this.GetTopAdjacents(topRow, leftColumn, bottomRow, rightColumn, filterRange);
    this.GetLeftAdjacents(topRow, leftColumn, bottomRow, rightColumn, filterRange);
    this.GetBottomAdjacents(topRow, leftColumn, bottomRow, rightColumn, filterRange);
    this.GetRightAdjacents(topRow, leftColumn, bottomRow, rightColumn, filterRange);
    filterRange = filterRange.Worksheet[this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
    if (this.m_hasAdjacents)
      filterRange = this.IncludeAdjacents(this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn, filterRange, false);
    if (isEnd)
    {
      bool flag = false;
      for (int column1 = filterRange.Column; column1 <= filterRange.LastColumn; ++column1)
      {
        if ((filterRange.Worksheet[filterRange.Row, column1] as RangeImpl).CellType == RangeImpl.TCellType.Blank)
        {
          for (int column2 = filterRange.Column; column2 <= filterRange.LastColumn; ++column2)
          {
            if ((filterRange.Worksheet[filterRange.Row + 1, column2] as RangeImpl).CellType == RangeImpl.TCellType.Blank)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            for (int column3 = filterRange.Column; column3 <= filterRange.LastColumn; ++column3)
            {
              if ((filterRange.Worksheet[filterRange.Row + 1, column3] as RangeImpl).WrapText || (filterRange.Worksheet[filterRange.Row + 1, column3] as RangeImpl).FormatType != ExcelFormatType.General || (filterRange.Worksheet[filterRange.Row + 1, column3] as RangeImpl).CellType != RangeImpl.TCellType.LabelSST || this.Worksheet.Workbook.Version == ExcelVersion.Excel97to2003 && (filterRange.Worksheet[filterRange.Row + 1, column3] as RangeImpl).CellType != RangeImpl.TCellType.Label)
              {
                filterRange = filterRange.Worksheet[this.m_topRow + 1, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
                break;
              }
              if ((filterRange.Worksheet[filterRange.Row + 2, column3] as RangeImpl).CellType != RangeImpl.TCellType.Blank && ((filterRange.Worksheet[filterRange.Row + 2, column3] as RangeImpl).FormatType != ExcelFormatType.General || (filterRange.Worksheet[filterRange.Row + 2, column3] as RangeImpl).CellType != RangeImpl.TCellType.LabelSST || this.Worksheet.Workbook.Version == ExcelVersion.Excel97to2003 && (filterRange.Worksheet[filterRange.Row + 2, column3] as RangeImpl).CellType != RangeImpl.TCellType.Label))
              {
                filterRange = filterRange.Worksheet[this.m_topRow + 1, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
                break;
              }
            }
            break;
          }
          break;
        }
      }
    }
    return filterRange;
  }

  internal IRange IncludeBottomAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange)
  {
    this.InitializeFilterRange(topRow, leftColumn, bottomRow, rightColumn);
    this.m_hasAdjacents = false;
    this.GetBottomAdjacents(topRow, leftColumn, bottomRow, rightColumn, filterRange);
    filterRange = filterRange.Worksheet[this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn];
    if (this.m_hasAdjacents)
      filterRange = this.IncludeBottomAdjacents(this.m_topRow, this.m_leftColumn, this.m_bottomRow, this.m_rightColumn, filterRange);
    return filterRange;
  }

  private void InitializeFilterRange(int topRow, int leftColumn, int bottomRow, int rightColumn)
  {
    this.m_topRow = topRow;
    this.m_leftColumn = leftColumn;
    this.m_bottomRow = bottomRow;
    this.m_rightColumn = rightColumn;
  }

  private void GetTopAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange)
  {
    if (topRow == 1)
      return;
    int row = topRow - 1;
    int maxColumnCount = this.Worksheet.Workbook.MaxColumnCount;
    for (int column = leftColumn != 1 ? leftColumn - 1 : leftColumn; column <= (rightColumn != maxColumnCount ? rightColumn + 1 : rightColumn); ++column)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
      {
        this.m_hasAdjacents = true;
        this.m_topRow = row;
        break;
      }
    }
  }

  private void GetLeftAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange)
  {
    if (leftColumn == 1)
      return;
    int column = leftColumn - 1;
    int maxRowCount = this.Worksheet.Workbook.MaxRowCount;
    for (int row = topRow != 1 ? topRow - 1 : topRow; row <= (bottomRow != maxRowCount ? bottomRow + 1 : bottomRow); ++row)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
      {
        this.m_hasAdjacents = true;
        this.m_leftColumn = column;
        break;
      }
    }
  }

  private void GetBottomAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange)
  {
    IWorkbook workbook = this.Worksheet.Workbook;
    if (bottomRow == workbook.MaxRowCount)
      return;
    int row = bottomRow + 1;
    int maxColumnCount = workbook.MaxColumnCount;
    for (int column = leftColumn != 1 ? leftColumn - 1 : leftColumn; column <= (rightColumn != maxColumnCount ? rightColumn + 1 : rightColumn); ++column)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
      {
        this.m_hasAdjacents = true;
        this.m_bottomRow = row;
        break;
      }
    }
  }

  private void GetRightAdjacents(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    IRange filterRange)
  {
    IWorkbook workbook = this.Worksheet.Workbook;
    if (rightColumn == workbook.MaxColumnCount)
      return;
    int column = rightColumn + 1;
    int maxRowCount = workbook.MaxRowCount;
    for (int row = topRow != 1 ? topRow - 1 : topRow; row <= (bottomRow != maxRowCount ? bottomRow + 1 : bottomRow); ++row)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
      {
        this.m_hasAdjacents = true;
        this.m_rightColumn = column;
        break;
      }
    }
  }

  private bool HasAdjacent(int row, int column, IRange filterRange)
  {
    IWorkbook workbook = this.Worksheet.Workbook;
    int maxColumnCount = workbook.MaxColumnCount;
    int maxRowCount = workbook.MaxRowCount;
    if (row > 1)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row - 1, column);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (row > 1 && column > 1)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row - 1, column - 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (row > 1 && column < maxColumnCount)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row - 1, column + 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (row < maxRowCount)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row + 1, column);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (row < maxRowCount && column > 1)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row + 1, column - 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (row < maxRowCount && column < maxColumnCount)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row + 1, column + 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (column > 1)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row, column - 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    if (column < maxColumnCount)
    {
      BiffRecordRaw record = (BiffRecordRaw) this.m_worksheet.GetRecord(row, column + 1);
      if (record != null && record.TypeCode != TBIFFRecord.Blank)
        return true;
    }
    return false;
  }

  private bool IsRowNotEmpty(int row, int left, int right, IRange filterRange)
  {
    for (int column = left; column <= right; ++column)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
        return true;
    }
    return false;
  }

  private bool IsColumnNotEmpty(int column, int top, int bottom, IRange filterRange)
  {
    for (int row = top; row <= bottom; ++row)
    {
      if ((filterRange.Worksheet[row, column] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
        return true;
    }
    return false;
  }
}
