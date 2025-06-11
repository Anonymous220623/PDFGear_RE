// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.MergeCellsImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class MergeCellsImpl : CommonObject, ICloneParent
{
  private WorksheetImpl m_sheet;
  private List<MergeCellsRecord> m_arrRecordsToParse = new List<MergeCellsRecord>();
  private bool m_bParsed = true;
  private List<Rectangle> m_arrCells = new List<Rectangle>();

  public MergeCellsImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (this.m_bParsed)
    {
      int mergeCount = this.MergeCount;
      if (mergeCount < 0)
        return;
      MergeCellsRecord.MergedRegion[] arrRegions = new MergeCellsRecord.MergedRegion[mergeCount];
      for (int index = 0; index < mergeCount; ++index)
      {
        MergeCellsRecord.MergedRegion mergeRegion = this.RectangleToMergeRegion(this.m_arrCells[index]);
        arrRegions[index] = mergeRegion;
      }
      int iCount;
      for (int iStartIndex = 0; iStartIndex != mergeCount; iStartIndex += iCount)
      {
        iCount = mergeCount - iStartIndex;
        if (iCount > 1027)
          iCount = 1027;
        MergeCellsRecord record = (MergeCellsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MergeCells);
        record.SetRegions(iStartIndex, iCount, arrRegions);
        records.Add((IBiffStorage) record);
      }
    }
    else
      records.AddRange((ICollection) this.m_arrRecordsToParse);
  }

  internal int MergeCount
  {
    get
    {
      this.Parse();
      return this.m_arrCells.Count;
    }
  }

  internal List<Rectangle> MergedRegions
  {
    get
    {
      this.Parse();
      return this.m_arrCells;
    }
  }

  private void FindParents()
  {
    this.m_sheet = (WorksheetImpl) (this.FindParent(typeof (WorksheetImpl)) ?? throw new ArgumentNullException("Can't find parent workbook"));
  }

  internal MergeCellsRecord.MergedRegion this[Rectangle rect] => this.FindMergedRegion(rect);

  internal Rectangle this[int index]
  {
    get
    {
      this.Parse();
      return this.m_arrCells[index];
    }
  }

  public IList<ExtendedFormatImpl> GetMergedExtendedFormats()
  {
    this.Parse();
    IList<ExtendedFormatImpl> mergedExtendedFormats = (IList<ExtendedFormatImpl>) new List<ExtendedFormatImpl>();
    CellRecordCollection cellRecords = this.m_sheet.CellRecords;
    int index = 0;
    for (int mergeCount = this.MergeCount; index < mergeCount; ++index)
    {
      ExtendedFormatImpl format = this.GetFormat(this.RectangleToMergeRegion(this.m_arrCells[index]));
      mergedExtendedFormats.Add(format);
    }
    return mergedExtendedFormats;
  }

  [CLSCompliant(false)]
  public ExtendedFormatImpl GetFormat(MergeCellsRecord.MergedRegion region)
  {
    WorkbookImpl workbook = (WorkbookImpl) this.m_sheet.Workbook;
    CellRecordCollection cellRecords = this.m_sheet.CellRecords;
    int iFirstXF = cellRecords.GetExtendedFormatIndex(region.RowFrom + 1, region.ColumnFrom + 1);
    int iEndXF = cellRecords.GetExtendedFormatIndex(region.RowTo + 1, region.ColumnTo + 1);
    if (iFirstXF < 0)
      iFirstXF = workbook.DefaultXFIndex;
    if (iEndXF < 0)
      iEndXF = workbook.DefaultXFIndex;
    return workbook.InnerExtFormats.GatherTwoFormats(iFirstXF, iEndXF);
  }

  public void AddMerge(RangeImpl range, OfficeMergeOperation operation)
  {
    int RowFrom = range.FirstRow - 1;
    int ColFrom = range.FirstColumn - 1;
    int RowTo = range.LastRow - 1;
    int ColTo = range.LastColumn - 1;
    this.Parse();
    this.AddMerge(RowFrom, RowTo, ColFrom, ColTo, operation);
  }

  private void AddMerge(MergeCellsRecord.MergedRegion region, OfficeMergeOperation operation)
  {
    int rowFrom = region.RowFrom;
    int columnFrom = region.ColumnFrom;
    int rowTo = region.RowTo;
    int columnTo = region.ColumnTo;
    this.AddMerge(rowFrom, rowTo, columnFrom, columnTo, operation);
  }

  public void AddMerge(
    int RowFrom,
    int RowTo,
    int ColFrom,
    int ColTo,
    OfficeMergeOperation operation)
  {
    Rectangle range = new Rectangle(ColFrom, RowFrom, ColTo - ColFrom, RowTo - RowFrom);
    if (operation == OfficeMergeOperation.Delete)
      this.DeleteMerge(range);
    this.m_arrCells.Add(range);
  }

  public void DeleteMerge(Rectangle range)
  {
    this.Parse();
    List<Rectangle> rectangleList = new List<Rectangle>();
    int index1 = 0;
    for (int mergeCount = this.MergeCount; index1 < mergeCount; ++index1)
    {
      Rectangle arrCell = this.m_arrCells[index1];
      if (UtilityMethods.Intersects(arrCell, range))
        rectangleList.Add(arrCell);
    }
    int index2 = 0;
    for (int count = rectangleList.Count; index2 < count; ++index2)
      this.m_arrCells.Remove(rectangleList[index2]);
  }

  public void Clear()
  {
    this.m_arrCells.Clear();
    this.m_arrRecordsToParse = (List<MergeCellsRecord>) null;
    this.m_bParsed = true;
  }

  [CLSCompliant(false)]
  public void AddMerge(MergeCellsRecord mergeRecord)
  {
    this.m_arrRecordsToParse.Add(mergeRecord);
    this.m_bParsed = false;
  }

  public void Parse()
  {
    if (this.m_bParsed)
      return;
    int index1 = 0;
    for (int count = this.m_arrRecordsToParse.Count; index1 < count; ++index1)
    {
      MergeCellsRecord.MergedRegion[] regions = this.m_arrRecordsToParse[index1].Regions;
      int index2 = 0;
      for (int length = regions.Length; index2 < length; ++index2)
        this.AddMerge(regions[index2], OfficeMergeOperation.Leave);
    }
    this.m_bParsed = true;
    this.m_arrRecordsToParse = (List<MergeCellsRecord>) null;
  }

  public void RemoveRow(int iRowIndex)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_sheet.Workbook.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex));
    this.InsertRemoveRow(iRowIndex, true, 1);
  }

  public void RemoveRow(int rowIndex, int count)
  {
    if (rowIndex < 1 || rowIndex > this.m_sheet.Workbook.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    this.InsertRemoveRow(rowIndex, true, count);
  }

  public void InsertRow(int iRowIndex, int iRowCount)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_sheet.Workbook.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex));
    this.InsertRemoveRow(iRowIndex, false, iRowCount);
  }

  public void RemoveColumn(int iColumnIndex) => this.InsertRemoveColumn(iColumnIndex, true, 1);

  public void RemoveColumn(int index, int count) => this.InsertRemoveColumn(index, true, count);

  public void InsertColumn(int iColumnIndex) => this.InsertRemoveColumn(iColumnIndex, false, 1);

  public void InsertColumn(int iColumnIndex, int iColumnCount)
  {
    this.InsertRemoveColumn(iColumnIndex, false, iColumnCount);
  }

  protected void InsertRemoveRow(int iRowIndex, bool isRemove, int iRowCount)
  {
    --iRowIndex;
    if (iRowIndex < 0 || iRowIndex > this.m_sheet.Workbook.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex));
    this.Parse();
    List<Rectangle> arrCells = this.m_arrCells;
    this.m_arrCells = new List<Rectangle>();
    int index1 = 0;
    for (int count = arrCells.Count; index1 < count; ++index1)
    {
      MergeCellsRecord.MergedRegion region = MergeCellsImpl.InsertRemoveRow(this.RectangleToMergeRegion(arrCells[index1]), iRowIndex, isRemove, iRowCount, this.m_sheet.Workbook);
      if (region != null)
      {
        this.AddMerge(region, OfficeMergeOperation.Delete);
        if (!isRemove && region.RowFrom + 1 == iRowIndex)
        {
          for (int index2 = 1; index2 <= iRowCount; ++index2)
          {
            ++region.RowFrom;
            ++region.RowTo;
            this.AddMerge(region, OfficeMergeOperation.Delete);
          }
        }
      }
    }
  }

  protected void InsertRemoveColumn(int iColumnIndex, bool isRemove, int iCount)
  {
    --iColumnIndex;
    if (iColumnIndex < 0 || iColumnIndex >= this.m_sheet.Workbook.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex));
    if (iCount < 0 || iCount >= this.m_sheet.Workbook.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    this.Parse();
    List<Rectangle> arrCells = this.m_arrCells;
    this.m_arrCells = new List<Rectangle>();
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
    {
      MergeCellsRecord.MergedRegion region = MergeCellsImpl.InsertRemoveColumn(this.RectangleToMergeRegion(arrCells[index]), iColumnIndex, isRemove, iCount, this.m_sheet.Workbook);
      if (region != null)
        this.AddMerge(region, OfficeMergeOperation.Delete);
    }
  }

  public void CopyMoveMerges(IRange destination, IRange source, bool bIsMove)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.Parse();
    int iRowDelta = destination.Row - source.Row;
    int iColDelta = destination.Column - source.Column;
    List<MergeCellsRecord.MergedRegion> lstRegions = new List<MergeCellsRecord.MergedRegion>();
    this.CacheMerges(source, lstRegions);
    if (bIsMove)
      this.RemoveCache(lstRegions);
    if (lstRegions.Count == 0)
      (destination as RangeImpl).InnerWorksheet.MergeCells.DeleteMerge(Rectangle.FromLTRB(destination.Column - 1, destination.Row - 1, destination.LastColumn - 1, destination.LastRow - 1));
    ((WorksheetImpl) source.Worksheet).MergeCells.AddCache(lstRegions, iRowDelta, iColDelta);
  }

  [CLSCompliant(false)]
  public List<MergeCellsRecord.MergedRegion> FindMergesToCopyMove(IRange range, bool bIsMove)
  {
    List<MergeCellsRecord.MergedRegion> lstRegions = new List<MergeCellsRecord.MergedRegion>();
    this.CacheMerges(range, lstRegions);
    if (bIsMove)
      this.RemoveCache(lstRegions);
    return lstRegions;
  }

  [CLSCompliant(false)]
  public void CacheMerges(IRange range, List<MergeCellsRecord.MergedRegion> lstRegions)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (lstRegions == null)
      throw new ArgumentNullException(nameof (lstRegions));
    this.Parse();
    int num1 = range.Row - 1;
    int num2 = range.Column - 1;
    int num3 = range.LastRow - 1;
    int num4 = range.LastColumn - 1;
    for (int y = num1; y <= num3; ++y)
    {
      for (int x = num2; x <= num4; ++x)
      {
        MergeCellsRecord.MergedRegion mergedRegion = this.FindMergedRegion(new Rectangle(x, y, 0, 0));
        if (mergedRegion != null && !lstRegions.Contains(mergedRegion))
          lstRegions.Add(mergedRegion);
      }
    }
  }

  private static void CheckRegion(MergeCellsRecord.MergedRegion region, IRange range)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    int num = range != null ? range.Row : throw new ArgumentNullException(nameof (range));
    int column = range.Column;
    int lastRow = range.LastRow;
    int lastColumn = range.LastColumn;
    if (region.ColumnFrom < column || region.ColumnTo > lastColumn || region.RowFrom < num || region.RowTo > lastRow)
      throw new ArgumentOutOfRangeException(nameof (region));
  }

  private void RemoveCache(List<MergeCellsRecord.MergedRegion> lstRegions)
  {
    if (lstRegions == null)
      throw new ArgumentNullException(nameof (lstRegions));
    this.Parse();
    foreach (MergeCellsRecord.MergedRegion lstRegion in lstRegions)
      this.m_arrCells.Remove(Rectangle.FromLTRB(lstRegion.ColumnFrom, lstRegion.RowFrom, lstRegion.ColumnTo, lstRegion.RowTo));
  }

  [CLSCompliant(false)]
  public void AddCache(
    List<MergeCellsRecord.MergedRegion> lstRegions,
    int iRowDelta,
    int iColDelta)
  {
    if (lstRegions == null)
      throw new ArgumentNullException(nameof (lstRegions));
    this.Parse();
    foreach (MergeCellsRecord.MergedRegion lstRegion in lstRegions)
    {
      lstRegion.MoveRegion(iRowDelta, iColDelta);
      this.AddMerge(lstRegion, OfficeMergeOperation.Delete);
    }
  }

  public void AddMerges(IDictionary dictMerges, int iRowDelta, int iColumnDelta)
  {
    if (dictMerges == null)
      throw new ArgumentNullException(nameof (dictMerges));
    this.Parse();
    foreach (DictionaryEntry dictMerge in dictMerges)
    {
      long key = (long) dictMerge.Key;
      long index = (long) dictMerge.Value;
      int rowFromCellIndex1 = RangeImpl.GetRowFromCellIndex(key);
      int columnFromCellIndex1 = RangeImpl.GetColumnFromCellIndex(key);
      int rowFromCellIndex2 = RangeImpl.GetRowFromCellIndex(index);
      int columnFromCellIndex2 = RangeImpl.GetColumnFromCellIndex(index);
      this.AddMerge(rowFromCellIndex1 + iRowDelta - 1, rowFromCellIndex2 + iRowDelta - 1, columnFromCellIndex1 + iColumnDelta - 1, columnFromCellIndex2 + iColumnDelta - 1, OfficeMergeOperation.Delete);
    }
  }

  public Rectangle GetLeftTopCell(Rectangle rect)
  {
    MergeCellsRecord.MergedRegion mergedRegion = this.FindMergedRegion(rect);
    return mergedRegion == null ? Rectangle.FromLTRB(-1, -1, -1, -1) : new Rectangle(mergedRegion.ColumnFrom, mergedRegion.RowFrom, 0, 0);
  }

  public object Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    MergeCellsImpl mergeCellsImpl = (MergeCellsImpl) this.MemberwiseClone();
    mergeCellsImpl.SetParent(parent);
    mergeCellsImpl.FindParents();
    mergeCellsImpl.m_arrCells = this.CloneList(this.m_arrCells);
    mergeCellsImpl.m_arrRecordsToParse = CloneUtils.CloneCloneable<MergeCellsRecord>(this.m_arrRecordsToParse);
    return (object) mergeCellsImpl;
  }

  private List<Rectangle> CloneList(List<Rectangle> list)
  {
    int count = list.Count;
    List<Rectangle> rectangleList = new List<Rectangle>(count);
    for (int index = 0; index < count; ++index)
      rectangleList.Add(list[index]);
    return rectangleList;
  }

  public void SetNewDimensions(int newRowCount, int newColumnCount)
  {
    --newRowCount;
    --newColumnCount;
    this.Parse();
    List<Rectangle> arrCells = this.m_arrCells;
    this.m_arrCells = new List<Rectangle>();
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
    {
      MergeCellsRecord.MergedRegion mergeRegion = this.RectangleToMergeRegion(arrCells[index]);
      mergeRegion.RowTo = Math.Min(mergeRegion.RowTo, newRowCount);
      mergeRegion.ColumnTo = Math.Min(mergeRegion.ColumnTo, newColumnCount);
      if (mergeRegion.CellsCount > 1)
        this.AddMerge(mergeRegion, OfficeMergeOperation.Delete);
    }
  }

  [CLSCompliant(false)]
  public MergeCellsRecord.MergedRegion FindMergedRegion(Rectangle rectangle)
  {
    MergeCellsRecord.MergedRegion mergedRegion = (MergeCellsRecord.MergedRegion) null;
    int index = 0;
    for (int mergeCount = this.MergeCount; index < mergeCount; ++index)
    {
      Rectangle arrCell = this.m_arrCells[index];
      if (UtilityMethods.Intersects(arrCell, rectangle))
      {
        mergedRegion = this.RectangleToMergeRegion(arrCell);
        break;
      }
    }
    return mergedRegion;
  }

  [CLSCompliant(false)]
  public MergeCellsRecord.MergedRegion RectangleToMergeRegion(Rectangle rect)
  {
    int x = rect.X;
    int y = rect.Y;
    int right = rect.Right;
    int bottom = rect.Bottom;
    return new MergeCellsRecord.MergedRegion(y, bottom, x, right);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveRowLower(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iRowIndex,
    int iRowCount,
    IWorkbook book)
  {
    if (iRowCount <= 0 || iRowCount > book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowCount));
    int num = isRemove ? -iRowCount : iRowCount;
    int iRowIndex1 = region.RowFrom + num;
    if (iRowIndex1 >= book.MaxRowCount)
      return (MergeCellsRecord.MergedRegion) null;
    if (iRowIndex1 < iRowIndex)
      iRowIndex1 = iRowIndex;
    int iRowIndex2 = region.RowTo + num;
    return iRowIndex2 < iRowIndex ? (MergeCellsRecord.MergedRegion) null : new MergeCellsRecord.MergedRegion(MergeCellsImpl.NormalizeRow(iRowIndex1, book), MergeCellsImpl.NormalizeRow(iRowIndex2, book), region.ColumnFrom, region.ColumnTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveRowStart(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iRowCount,
    IWorkbook book)
  {
    if (iRowCount <= 0 || iRowCount > book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowCount));
    int num = isRemove ? -iRowCount : iRowCount;
    int iRowIndex1 = region.RowTo + num;
    int iRowIndex2 = isRemove ? region.RowFrom : region.RowFrom + num;
    if (iRowIndex1 < region.RowFrom)
      return (MergeCellsRecord.MergedRegion) null;
    return iRowIndex2 >= book.MaxRowCount ? (MergeCellsRecord.MergedRegion) null : new MergeCellsRecord.MergedRegion(MergeCellsImpl.NormalizeRow(iRowIndex2, book), MergeCellsImpl.NormalizeRow(iRowIndex1, book), region.ColumnFrom, region.ColumnTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveRowMiddleEnd(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iRowIndex,
    int iRowCount,
    IWorkbook book)
  {
    if (iRowCount <= 0 || iRowCount > book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowCount));
    int num = isRemove ? -iRowCount : iRowCount;
    int iRowIndex1 = region.RowTo + num;
    if (iRowIndex1 < iRowIndex)
      iRowIndex1 = iRowIndex - 1;
    int rowTo = MergeCellsImpl.NormalizeRow(iRowIndex1, book);
    return new MergeCellsRecord.MergedRegion(region.RowFrom, rowTo, region.ColumnFrom, region.ColumnTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveRowAbove(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iRowCount)
  {
    return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, region.ColumnFrom, region.ColumnTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveRow(
    MergeCellsRecord.MergedRegion region,
    int iRowIndex,
    bool isRemove,
    int iRowCount,
    IWorkbook book)
  {
    if (region.RowFrom == 0 && region.RowTo == book.MaxRowCount - 1)
      return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, region.ColumnFrom, region.ColumnTo);
    if (region.RowFrom > iRowIndex)
      return MergeCellsImpl.InsertRemoveRowLower(region, isRemove, iRowIndex, iRowCount, book);
    if (region.RowFrom == iRowIndex)
      return MergeCellsImpl.InsertRemoveRowStart(region, isRemove, iRowCount, book);
    if (region.RowFrom < iRowIndex && region.RowTo > iRowIndex || region.RowTo == iRowIndex)
      return MergeCellsImpl.InsertRemoveRowMiddleEnd(region, isRemove, iRowIndex, iRowCount, book);
    return region.RowTo < iRowIndex ? MergeCellsImpl.InsertRemoveRowAbove(region, isRemove, iRowCount) : (MergeCellsRecord.MergedRegion) null;
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveColumnLower(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iColumnIndex,
    int iCount,
    IWorkbook book)
  {
    if (iCount <= 0 || iCount > book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    int num = isRemove ? -iCount : iCount;
    int iColumnIndex1 = region.ColumnFrom + num;
    if (iColumnIndex1 >= book.MaxColumnCount)
      return (MergeCellsRecord.MergedRegion) null;
    if (iColumnIndex1 < iColumnIndex)
      iColumnIndex1 = iColumnIndex;
    int iColumnIndex2 = region.ColumnTo + num;
    if (iColumnIndex2 < iColumnIndex)
      return (MergeCellsRecord.MergedRegion) null;
    int colFrom = MergeCellsImpl.NormalizeColumn(iColumnIndex1, book);
    int colTo = MergeCellsImpl.NormalizeColumn(iColumnIndex2, book);
    return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, colFrom, colTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveColumnStart(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iCount,
    IWorkbook book)
  {
    if (iCount <= 0 || iCount > book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    int num = isRemove ? -iCount : iCount;
    int iRowIndex1 = region.ColumnTo + num;
    int iRowIndex2 = isRemove ? region.ColumnFrom : region.ColumnFrom + num;
    if (iRowIndex1 < region.ColumnFrom)
      return (MergeCellsRecord.MergedRegion) null;
    if (iRowIndex2 >= book.MaxColumnCount)
      return (MergeCellsRecord.MergedRegion) null;
    int colFrom = MergeCellsImpl.NormalizeRow(iRowIndex2, book);
    int colTo = MergeCellsImpl.NormalizeRow(iRowIndex1, book);
    return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, colFrom, colTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveColumnMiddleEnd(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iColumnIndex,
    int iCount,
    IWorkbook book)
  {
    if (iCount <= 0 || iCount > book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    int num = isRemove ? -iCount : iCount;
    int iColumnIndex1 = region.ColumnTo + num;
    if (iColumnIndex1 < iColumnIndex)
      iColumnIndex1 = iColumnIndex - 1;
    int colTo = MergeCellsImpl.NormalizeColumn(iColumnIndex1, book);
    return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, region.ColumnFrom, colTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveColumnAbove(
    MergeCellsRecord.MergedRegion region,
    bool isRemove,
    int iCount)
  {
    return new MergeCellsRecord.MergedRegion(region.RowFrom, region.RowTo, region.ColumnFrom, region.ColumnTo);
  }

  [CLSCompliant(false)]
  public static MergeCellsRecord.MergedRegion InsertRemoveColumn(
    MergeCellsRecord.MergedRegion region,
    int iColumnIndex,
    bool isRemove,
    int iCount,
    IWorkbook book)
  {
    if (region.ColumnFrom == 0 && region.ColumnTo == book.MaxColumnCount - 1)
      return new MergeCellsRecord.MergedRegion(region);
    if (region.ColumnFrom > iColumnIndex)
      return MergeCellsImpl.InsertRemoveColumnLower(region, isRemove, iColumnIndex, iCount, book);
    if (region.ColumnFrom == iColumnIndex)
      return MergeCellsImpl.InsertRemoveColumnStart(region, isRemove, iCount, book);
    if (region.ColumnFrom < iColumnIndex && region.ColumnTo > iColumnIndex || region.ColumnTo == iColumnIndex)
      return MergeCellsImpl.InsertRemoveColumnMiddleEnd(region, isRemove, iColumnIndex, iCount, book);
    return region.ColumnTo < iColumnIndex ? MergeCellsImpl.InsertRemoveColumnAbove(region, isRemove, iCount) : (MergeCellsRecord.MergedRegion) null;
  }

  [CLSCompliant(false)]
  public static int NormalizeRow(int iRowIndex, IWorkbook book)
  {
    return iRowIndex < 0 ? 0 : Math.Min(iRowIndex, book.MaxRowCount - 1);
  }

  [CLSCompliant(false)]
  public static int NormalizeColumn(int iColumnIndex, IWorkbook book)
  {
    return iColumnIndex < 0 ? 0 : Math.Min(iColumnIndex, book.MaxColumnCount - 1);
  }
}
