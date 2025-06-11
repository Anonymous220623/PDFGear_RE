// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.RowStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class RowStorage : IDisposable, IBiffStorage, ICloneable, IOutline
{
  private const int DEF_MEMORY_DELTA = 128 /*0x80*/;
  private const int DEF_MULRK_PERIOD = 6;
  private const int DEF_MULBLANK_PERIOD = 2;
  private const string DEF_DOT = ".";
  private static readonly short[] DEF_MULTIRECORDS = new short[2]
  {
    (short) 190,
    (short) 189
  };
  private static readonly TBIFFRecord[] DEF_MULTIRECORDS_SUBTYPES = new TBIFFRecord[4]
  {
    TBIFFRecord.MulBlank,
    TBIFFRecord.Blank,
    TBIFFRecord.MulRK,
    TBIFFRecord.RK
  };
  private int m_iFirstColumn = -1;
  private int m_iLastColumn = -1;
  private int m_iUsedSize;
  private DataProvider m_dataProvider;
  private RowStorage.StorageOptions m_options;
  private int m_iCurrentColumn = -1;
  private int m_iCurrentOffset = -1;
  private ExcelVersion m_version;
  private WorkbookImpl m_book;
  private int m_row;
  private bool m_isWrapText;
  private string[] dateFormats = new string[6]
  {
    "d",
    "dd",
    "m",
    "mm",
    "yy",
    "yyyy"
  };
  private bool m_hasRowHeight;
  private ushort m_usHeight;
  private RowRecord.OptionFlags m_optionFlags = RowRecord.OptionFlags.ShowOutlineGroups;
  private ushort m_usXFIndex;
  internal TableRecord m_tableRecord;
  internal bool m_isFilteredRow;
  internal List<int> m_columnFilterHideRow;
  private bool m_autoHeight;
  private double m_dyDescent;

  public RowStorage(int iRowNumber, int height, int xfIndex)
  {
    this.m_usHeight = (ushort) height;
    this.ExtendedFormatIndex = (ushort) xfIndex;
  }

  public void Dispose(bool disposing)
  {
    if (!disposing || this.Disposed)
      return;
    if (this.m_dataProvider != null)
    {
      this.m_dataProvider.Dispose();
      this.m_dataProvider = (DataProvider) null;
      this.m_iFirstColumn = -1;
      this.m_iLastColumn = -1;
      this.m_iUsedSize = -1;
    }
    this.Disposed = true;
    GC.SuppressFinalize((object) this);
  }

  public void Dispose() => this.Dispose(true);

  ~RowStorage() => this.Dispose(false);

  internal int Row
  {
    get => this.m_row;
    set => this.m_row = value;
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public int LastColumn
  {
    get => this.m_iLastColumn;
    set => this.m_iLastColumn = value;
  }

  public int UsedSize => this.m_iUsedSize;

  public int DataSize => this.m_dataProvider == null ? 0 : this.m_dataProvider.Capacity;

  public bool HasRkBlank
  {
    get
    {
      return (this.m_options & RowStorage.StorageOptions.HasRKBlank) != RowStorage.StorageOptions.None;
    }
    set
    {
      if (value)
        this.m_options |= RowStorage.StorageOptions.HasRKBlank;
      else
        this.m_options &= ~RowStorage.StorageOptions.HasRKBlank;
    }
  }

  public bool HasMultiRkBlank
  {
    get
    {
      return (this.m_options & RowStorage.StorageOptions.HasMultiRKBlank) != RowStorage.StorageOptions.None;
    }
    set
    {
      if (value)
        this.m_options |= RowStorage.StorageOptions.HasMultiRKBlank;
      else
        this.m_options &= ~RowStorage.StorageOptions.HasMultiRKBlank;
    }
  }

  private bool Disposed
  {
    get => (this.m_options & RowStorage.StorageOptions.Disposed) != RowStorage.StorageOptions.None;
    set
    {
      if (value)
        this.m_options |= RowStorage.StorageOptions.Disposed;
      else
        this.m_options &= ~RowStorage.StorageOptions.Disposed;
    }
  }

  public bool IsWrapText
  {
    get => this.m_isWrapText;
    set => this.m_isWrapText = value;
  }

  public DataProvider Provider => this.m_dataProvider;

  public int CellPositionSize => this.m_version != ExcelVersion.Excel97to2003 ? 8 : 4;

  public ExcelVersion Version => this.m_version;

  internal double DyDescent
  {
    get => this.m_dyDescent;
    set => this.m_dyDescent = value;
  }

  internal bool HasRowHeight
  {
    get => this.m_hasRowHeight;
    set => this.m_hasRowHeight = value;
  }

  internal bool AutoHeight
  {
    get => this.m_autoHeight;
    set => this.m_autoHeight = value;
  }

  public IEnumerator GetEnumerator(RecordExtractor recordExtractor)
  {
    return recordExtractor != null ? (IEnumerator) new RowStorageEnumerator(this, recordExtractor) : throw new ArgumentNullException(nameof (recordExtractor));
  }

  public void SetCellStyle(int iRow, int iColumn, int iXFIndex, int iBlockSize)
  {
    bool bFound;
    int num = this.LocateRecord(iColumn, out bFound);
    if (!bFound)
    {
      ICellPositionFormat cell = UtilityMethods.CreateCell(iRow, iColumn, TBIFFRecord.Blank);
      cell.ExtendedFormatIndex = (ushort) iXFIndex;
      this.SetRecord(iColumn, cell, iBlockSize);
    }
    else
    {
      switch ((TBIFFRecord) this.m_dataProvider.ReadInt16(num))
      {
        case TBIFFRecord.MulRK:
          this.SetXFIndexMulti(num, (ushort) iXFIndex, iColumn, 6);
          break;
        case TBIFFRecord.MulBlank:
          this.SetXFIndexMulti(num, (ushort) iXFIndex, iColumn, 2);
          break;
        default:
          this.SetXFIndex(num, (ushort) iXFIndex);
          break;
      }
    }
  }

  [CLSCompliant(false)]
  public BiffRecordRaw GetRecordAtOffset(int iOffset)
  {
    if (iOffset < 0 || iOffset >= this.m_iUsedSize)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    return BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
  }

  [CLSCompliant(false)]
  public ICellPositionFormat GetRecord(int iColumnIndex, int iBlockSize)
  {
    if (this.HasMultiRkBlank)
      this.Decompress(false, iBlockSize);
    if (this.m_iFirstColumn < 0 || iColumnIndex < this.m_iFirstColumn || iColumnIndex > this.m_iLastColumn)
      return (ICellPositionFormat) null;
    bool bFound;
    int iOffset = this.LocateRecord(iColumnIndex, out bFound);
    BiffRecordRaw record = (BiffRecordRaw) null;
    if (bFound)
    {
      int type = (int) this.m_dataProvider.ReadInt16(iOffset);
      int iLength = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      record = BiffRecordFactory.GetRecord(type);
      record.Length = iLength;
      record.ParseStructure(this.m_dataProvider, iOffset + 4, iLength, this.Version);
    }
    return record as ICellPositionFormat;
  }

  [CLSCompliant(false)]
  public ICellPositionFormat GetRecord(
    int iColumnIndex,
    int iBlockSize,
    RecordExtractor recordExtractor)
  {
    if (this.HasMultiRkBlank)
      this.Decompress(false, iBlockSize);
    if (this.m_iFirstColumn < 0 || iColumnIndex < this.m_iFirstColumn || iColumnIndex > this.m_iLastColumn)
      return (ICellPositionFormat) null;
    bool bFound;
    int iOffset = this.LocateRecord(iColumnIndex, out bFound);
    BiffRecordRaw record = (BiffRecordRaw) null;
    if (bFound)
    {
      int recordType = (int) this.m_dataProvider.ReadInt16(iOffset);
      int iLength = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      record = recordExtractor.GetRecord(recordType);
      record.Length = iLength;
      record.ParseStructure(this.m_dataProvider, iOffset + 4, iLength, this.Version);
    }
    return record as ICellPositionFormat;
  }

  [CLSCompliant(false)]
  public void SetRecord(int iColumnIndex, ICellPositionFormat cell, int iBlockSize)
  {
    if (this.HasMultiRkBlank)
      this.Decompress(false, iBlockSize);
    this.SetOrdinaryRecord(iColumnIndex, cell, iBlockSize);
  }

  public void ClearData()
  {
    this.m_iFirstColumn = -1;
    this.m_iLastColumn = -1;
    this.m_iUsedSize = 0;
    this.ExtendedFormatIndex = (ushort) 0;
  }

  public void SetFormulaStringValue(int iColumnIndex, string strValue, int iBlockSize)
  {
    int iFormulaRecordOffset;
    int iOffset = this.RemoveFormulaStringValue(iColumnIndex, out iFormulaRecordOffset);
    if (strValue == null)
      return;
    if (iOffset < 0)
      throw new NotSupportedException("Need formula cell to set FormulaStringValue.");
    FormulaRecord.SetStringValue(this.m_dataProvider, iFormulaRecordOffset, this.Version);
    StringRecord record = (StringRecord) BiffRecordFactory.GetRecord(TBIFFRecord.String);
    record.Value = strValue;
    int iRequiredSize = record.GetStoreSize(this.Version) + 4;
    this.InsertRecordData(iOffset, 0, iRequiredSize, (BiffRecordRaw) record, iBlockSize);
  }

  [CLSCompliant(false)]
  public void SetArrayRecord(int iColumnIndex, ArrayRecord array, int iBlockSize)
  {
    bool bFound;
    int iOffset1 = this.LocateRecord(iColumnIndex, out bFound);
    if (!bFound)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Cannot find record with specified index");
    int iOffset2 = this.m_dataProvider.ReadInt16(iOffset1) == (short) 6 ? this.MoveNext(iOffset1) : throw new ArgumentOutOfRangeException("RecordCode", "Cannot find FormulaRecord with specified column index");
    if (iOffset2 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(iOffset2) == (short) 545)
      this.RemoveRecord(iOffset2);
    if (array == null)
      return;
    int iRequiredSize = 4 + array.GetStoreSize(this.Version);
    this.InsertRecordData(iOffset2, 0, iRequiredSize, (BiffRecordRaw) array, iBlockSize);
  }

  [CLSCompliant(false)]
  public ArrayRecord GetArrayRecordByOffset(int iOffset)
  {
    if (this.m_dataProvider.ReadInt16(iOffset) != (short) 6)
      return (ArrayRecord) null;
    iOffset = this.MoveNext(iOffset);
    return iOffset < this.m_iUsedSize && this.m_dataProvider.ReadInt16(iOffset) == (short) 545 ? (ArrayRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) : (ArrayRecord) null;
  }

  [CLSCompliant(false)]
  public ArrayRecord GetArrayRecord(int iColumnIndex)
  {
    bool bFound;
    int iOffset = this.LocateRecord(iColumnIndex, out bFound);
    if (!bFound)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Cannot find record with specified index");
    return this.GetArrayRecordByOffset(iOffset);
  }

  public object Clone() => this.Clone(this.GetHeapHandle());

  private IntPtr GetHeapHandle()
  {
    return !(this.m_dataProvider is IntPtrDataProvider dataProvider) ? IntPtr.Zero : dataProvider.HeapHandle;
  }

  public object Clone(IntPtr heapHandle)
  {
    RowStorage rowStorage = new RowStorage(0, (int) this.m_usHeight, (int) this.m_usXFIndex);
    if (this.m_dataProvider != null && this.m_iUsedSize > 0 && this.m_iFirstColumn >= 0)
    {
      rowStorage.m_dataProvider = ApplicationImpl.CreateDataProvider(heapHandle);
      rowStorage.EnsureSize(this.DataSize, 1);
      this.m_dataProvider.CopyTo(0, rowStorage.m_dataProvider, 0, this.m_iUsedSize);
    }
    rowStorage.m_iFirstColumn = this.m_iFirstColumn;
    rowStorage.m_iLastColumn = this.m_iLastColumn;
    rowStorage.m_iUsedSize = this.m_iUsedSize;
    rowStorage.m_options = this.m_options;
    rowStorage.m_optionFlags = this.m_optionFlags;
    rowStorage.m_version = this.m_version;
    rowStorage.m_usXFIndex = this.m_usXFIndex;
    rowStorage.m_isWrapText = this.m_isWrapText;
    rowStorage.m_autoHeight = this.m_autoHeight;
    return (object) rowStorage;
  }

  public RowStorage Clone(int iStartColumn, int iEndColumn, int iBlockSize)
  {
    RowStorage rowStorage = new RowStorage(0, (int) this.m_usHeight, (int) this.m_usXFIndex);
    rowStorage.m_options = this.m_options;
    rowStorage.m_optionFlags = this.m_optionFlags;
    rowStorage.m_version = this.m_version;
    rowStorage.m_usXFIndex = this.m_usXFIndex;
    rowStorage.m_autoHeight = this.m_autoHeight;
    if (this.m_iUsedSize > 0)
    {
      iStartColumn = Math.Max(this.m_iFirstColumn, iStartColumn);
      iEndColumn = Math.Min(this.m_iLastColumn, iEndColumn);
      if (iStartColumn > iEndColumn)
        return (RowStorage) null;
      this.Decompress(false, iBlockSize);
      Point offsets = this.GetOffsets(iStartColumn, iEndColumn, out iStartColumn, out iEndColumn);
      if (iStartColumn < 0)
        return (RowStorage) null;
      int x = offsets.X;
      int num = offsets.Y - x;
      if (num > 0)
      {
        rowStorage.CreateDataProvider(this.GetHeapHandle());
        rowStorage.EnsureSize(num, iBlockSize);
        this.m_dataProvider.CopyTo(x, rowStorage.m_dataProvider, 0, num);
      }
      rowStorage.m_iFirstColumn = iStartColumn;
      rowStorage.m_iLastColumn = iEndColumn;
      rowStorage.m_iUsedSize = num;
    }
    rowStorage.m_iCurrentColumn = -1;
    rowStorage.m_iCurrentOffset = -1;
    return rowStorage;
  }

  public RowStorage Clone(
    SSTDictionary sourceSST,
    SSTDictionary destSST,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicNameIndexes,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dictExternSheets)
  {
    RowStorage rowStorage = new RowStorage(0, ((ApplicationImpl) destSST.Workbook.Application).StandardHeightInRowUnits, (int) this.m_usXFIndex);
    rowStorage.m_iFirstColumn = this.m_iFirstColumn;
    rowStorage.m_iLastColumn = this.m_iLastColumn;
    rowStorage.m_iUsedSize = this.m_iUsedSize;
    rowStorage.m_options = this.m_options;
    rowStorage.m_usHeight = this.m_usHeight;
    rowStorage.m_optionFlags = this.m_optionFlags;
    rowStorage.m_version = this.m_version;
    rowStorage.m_usXFIndex = this.m_usXFIndex;
    rowStorage.m_autoHeight = this.m_autoHeight;
    if (this.m_dataProvider != null && this.m_iUsedSize > 0 && this.m_iFirstColumn >= 0)
    {
      rowStorage.m_dataProvider = ApplicationImpl.CreateDataProvider(destSST.Workbook.HeapHandle);
      rowStorage.m_dataProvider.EnsureCapacity(this.m_iUsedSize);
      this.m_dataProvider.CopyTo(0, rowStorage.m_dataProvider, 0, this.m_iUsedSize);
    }
    rowStorage.UpdateRecordsAfterCopy(sourceSST, destSST, hashExtFormatIndexes, hashWorksheetNames, dicNameIndexes, dicFontIndexes, dictExternSheets);
    rowStorage.m_iCurrentColumn = -1;
    rowStorage.m_iCurrentOffset = -1;
    rowStorage.IsFormatted = this.IsFormatted;
    rowStorage.DyDescent = this.m_dyDescent;
    rowStorage.HasRowHeight = this.m_hasRowHeight;
    rowStorage.m_isWrapText = this.m_isWrapText;
    return rowStorage;
  }

  public void Remove(int iStartColumn, int iEndColumn, int blockSize)
  {
    if (this.m_iFirstColumn < 0)
      return;
    iStartColumn = Math.Max(this.m_iFirstColumn, iStartColumn);
    iEndColumn = Math.Min(this.m_iLastColumn, iEndColumn);
    if (iStartColumn > iEndColumn)
      return;
    this.Decompress(false, blockSize);
    Point offsets = this.GetOffsets(iStartColumn, iEndColumn, out iStartColumn, out iEndColumn);
    int x = offsets.X;
    int y = offsets.Y;
    int num = y - x;
    if (num <= 0)
      return;
    int iMemorySize = this.m_iUsedSize - y;
    if (iMemorySize > 0)
      this.m_dataProvider.MoveMemory(x, y, iMemorySize);
    this.m_iUsedSize -= num;
    this.UpdateColumns();
  }

  public void SetArrayFormulaIndex(int iColumn, int iArrayRow, int iArrayColumn, int iBlockSize)
  {
    bool bFound;
    int iOffset = this.LocateRecord(iColumn, out bFound);
    if (!bFound || this.m_dataProvider.ReadInt16(iOffset) != (short) 6)
      return;
    FormulaRecord record = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) as FormulaRecord;
    Ptg[] parsedExpression = record.ParsedExpression;
    if (parsedExpression == null || parsedExpression.Length == 0 || !(parsedExpression[0] is ControlPtg controlPtg))
      return;
    controlPtg.RowIndex = iArrayRow;
    controlPtg.ColumnIndex = iArrayColumn;
    record.ParsedExpression = parsedExpression;
    int num = record.GetStoreSize(this.Version) + 4;
    this.InsertRecordData(iOffset, num, num, (BiffRecordRaw) record, iBlockSize);
  }

  public bool Contains(int iColumn)
  {
    bool bFound;
    this.LocateRecord(iColumn, out bFound);
    return bFound;
  }

  public void InsertRowData(RowStorage sourceRow, int iBlockSize, IntPtr heapHandle)
  {
    if (sourceRow == null)
      throw new ArgumentNullException(nameof (sourceRow));
    if (sourceRow.m_iUsedSize <= 0)
      return;
    this.m_version = sourceRow.m_version;
    int firstColumn = sourceRow.FirstColumn;
    int lastColumn = sourceRow.LastColumn;
    if (this.m_dataProvider == null || this.m_iUsedSize <= 0)
    {
      this.m_iUsedSize = sourceRow.m_iUsedSize;
      this.CreateDataProvider(heapHandle);
      this.EnsureSize(this.m_iUsedSize, iBlockSize);
      sourceRow.m_dataProvider.CopyTo(0, this.m_dataProvider, 0, this.m_iUsedSize);
    }
    else
    {
      this.Remove(firstColumn, lastColumn, iBlockSize);
      int num = this.LocateRecord(firstColumn, out bool _);
      int iMemorySize = this.m_iUsedSize - num;
      int iUsedSize = sourceRow.m_iUsedSize;
      this.EnsureSize(this.m_iUsedSize + iUsedSize, iBlockSize);
      if (iMemorySize > 0)
        this.m_dataProvider.MoveMemory(num + iUsedSize, num, iMemorySize);
      sourceRow.m_dataProvider.CopyTo(0, this.m_dataProvider, num, iUsedSize);
      this.m_iUsedSize += iUsedSize;
    }
    if (this.m_iFirstColumn >= 0)
    {
      this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, firstColumn);
      this.m_iLastColumn = Math.Max(this.m_iLastColumn, lastColumn);
    }
    else
    {
      this.m_iFirstColumn = firstColumn;
      this.m_iLastColumn = lastColumn;
    }
    this.m_iCurrentOffset = -1;
    this.m_iCurrentColumn = -1;
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect,
    int iBlockSize,
    WorkbookImpl book)
  {
    if (this.m_dataProvider == null || this.m_iUsedSize <= 0 || this.m_iFirstColumn < 0)
      return;
    int iOffset = 0;
    int top1 = destRect.Top;
    int top2 = sourceRect.Top;
    int left1 = destRect.Left;
    int left2 = sourceRect.Left;
    int num;
    for (; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      switch (tbiffRecord)
      {
        case TBIFFRecord.Formula:
          FormulaRecord record1 = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) as FormulaRecord;
          Ptg[] parsedExpression = record1.ParsedExpression;
          record1.ParsedExpression = book.FormulaUtil.UpdateFormula(parsedExpression, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, record1.Row + 1, record1.Column + 1);
          this.InsertRecordData(iOffset, num + 4, record1.GetStoreSize(this.Version) + 4, (BiffRecordRaw) record1, iBlockSize);
          num = record1.GetStoreSize(this.Version);
          break;
        case TBIFFRecord.Array:
          ArrayRecord record2 = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) as ArrayRecord;
          Ptg[] formula = record2.Formula;
          record2.Formula = book.FormulaUtil.UpdateFormula(formula, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, record2.FirstRow + 1, record2.FirstColumn + 1);
          int iPreparedSize = num + 4;
          this.InsertRecordData(iOffset, iPreparedSize, record2.GetStoreSize(this.Version) + 4, (BiffRecordRaw) record2, iBlockSize);
          num = record2.GetStoreSize(this.Version);
          break;
      }
    }
  }

  internal void UpdateSubTotalFormula(
    List<int> insertedRows,
    int iCurIndex,
    int iSourceIndex,
    int iDestIndex,
    WorkbookImpl book)
  {
    int allocationBlockSize = book.Application.RowStorageAllocationBlockSize;
    int num;
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      FormulaRecord record1 = (FormulaRecord) null;
      Ptg[] arrPtgs = (Ptg[]) null;
      ArrayRecord record2 = (ArrayRecord) null;
      int iPreparedSize = 0;
      switch (tbiffRecord)
      {
        case TBIFFRecord.Formula:
          record1 = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) as FormulaRecord;
          break;
        case TBIFFRecord.Array:
          record2 = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version) as ArrayRecord;
          break;
      }
      if (tbiffRecord == TBIFFRecord.Formula || tbiffRecord == TBIFFRecord.Array)
      {
        arrPtgs = record1 != null ? record1.ParsedExpression : record2.Formula;
        foreach (int insertedRow in insertedRows)
        {
          Rectangle sourceRect = Rectangle.FromLTRB(0, insertedRow - 1, book.MaxColumnCount - 1, book.MaxRowCount - 1 - 1);
          Rectangle destRect = Rectangle.FromLTRB(0, insertedRow, book.MaxColumnCount - 1, book.MaxRowCount - 1);
          if (this.m_dataProvider == null || this.m_iUsedSize <= 0 || this.m_iFirstColumn < 0)
            return;
          int top1 = destRect.Top;
          int top2 = sourceRect.Top;
          int left1 = destRect.Left;
          int left2 = sourceRect.Left;
          if (record1 != null)
            arrPtgs = book.FormulaUtil.UpdateFormula(arrPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, record1.Row + 1, record1.Column + 1);
          if (record2 != null)
          {
            arrPtgs = book.FormulaUtil.UpdateFormula(arrPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, record2.FirstRow + 1, record2.FirstColumn + 1);
            iPreparedSize = num + 4;
          }
        }
      }
      if (record1 != null)
      {
        record1.ParsedExpression = arrPtgs;
        this.InsertRecordData(iOffset, num + 4, record1.GetStoreSize(this.Version) + 4, (BiffRecordRaw) record1, allocationBlockSize);
        num = record1.GetStoreSize(this.Version);
      }
      if (record2 != null)
      {
        record2.Formula = arrPtgs;
        this.InsertRecordData(iOffset, iPreparedSize, record2.GetStoreSize(this.Version) + 4, (BiffRecordRaw) record2, allocationBlockSize);
        num = record2.GetStoreSize(this.Version);
      }
    }
  }

  public void RowColumnOffset(int iDeltaRow, int iDeltaCol, int iBlockSize)
  {
    int num1 = 0;
    this.m_iFirstColumn += iDeltaCol;
    this.m_iLastColumn += iDeltaCol;
    int iLength;
    for (; num1 < this.m_iUsedSize; num1 += iLength + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(num1);
      iLength = (int) this.m_dataProvider.ReadInt16(num1 + 2);
      switch (tbiffRecord)
      {
        case TBIFFRecord.String:
          continue;
        case TBIFFRecord.Array:
          ArrayRecord record = (ArrayRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, num1, this.Version);
          record.FirstColumn += iDeltaCol;
          record.LastColumn += iDeltaCol;
          record.FirstRow += iDeltaRow;
          record.LastRow += iDeltaRow;
          int num2 = iLength + 4;
          this.InsertRecordData(num1, num2, num2, (BiffRecordRaw) record, iBlockSize);
          continue;
        default:
          int row = this.GetRow(num1);
          this.SetRow(num1, row + iDeltaRow);
          int column = this.GetColumn(num1);
          this.SetColumn(num1, column + iDeltaCol);
          if (tbiffRecord == TBIFFRecord.MulBlank || tbiffRecord == TBIFFRecord.MulRK)
          {
            MulBlankRecord.IncreaseLastColumn(this.m_dataProvider, num1, iLength, this.Version, iDeltaCol);
            continue;
          }
          continue;
      }
    }
  }

  public void UpdateNameIndexes(WorkbookImpl book, int[] arrNewIndex, int iBlockSize)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    int num;
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (tbiffRecord == TBIFFRecord.Formula || tbiffRecord == TBIFFRecord.Array)
      {
        IFormulaRecord record1 = (IFormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
        Ptg[] formula = record1.Formula;
        if (book.FormulaUtil.UpdateNameIndex(formula, arrNewIndex))
          record1.Formula = formula;
        BiffRecordRaw record2 = (BiffRecordRaw) record1;
        this.InsertRecordData(iOffset, num + 4, record2.GetStoreSize(this.Version) + 4, record2, iBlockSize);
        num = record2.GetStoreSize(this.Version);
      }
    }
  }

  public void UpdateNameIndexes(
    WorkbookImpl book,
    IDictionary<int, int> dicNewIndex,
    int iBlockSize)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    int num;
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (tbiffRecord == TBIFFRecord.Formula || tbiffRecord == TBIFFRecord.Array)
      {
        IFormulaRecord record1 = (IFormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
        Ptg[] formula = record1.Formula;
        if (book.FormulaUtil.UpdateNameIndex(formula, dicNewIndex))
          record1.Formula = formula;
        BiffRecordRaw record2 = (BiffRecordRaw) record1;
        this.InsertRecordData(iOffset, num + 4, record2.GetStoreSize(this.Version) + 4, record2, iBlockSize);
        num = record2.GetStoreSize(this.Version);
      }
    }
  }

  [CLSCompliant(false)]
  public void ReplaceSharedFormula(
    WorkbookImpl book,
    int row,
    int column,
    SharedFormulaRecord shared)
  {
    int allocationBlockSize = book.Application.RowStorageAllocationBlockSize;
    if (this.HasMultiRkBlank)
      this.Decompress(false, allocationBlockSize);
    if (this.m_iFirstColumn > shared.LastColumn || this.m_iLastColumn < shared.FirstColumn)
      return;
    int num1 = this.LocateRecord(shared.FirstColumn, out bool _);
    int num2;
    for (; num1 < this.m_iUsedSize; num1 += num2 + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(num1);
      num2 = (int) this.m_dataProvider.ReadInt16(num1 + 2);
      if (tbiffRecord != TBIFFRecord.Array && tbiffRecord != TBIFFRecord.String)
      {
        int column1 = this.GetColumn(num1);
        if (column1 > shared.LastColumn)
          break;
        if (column1 >= shared.FirstColumn && tbiffRecord == TBIFFRecord.Formula)
        {
          FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, num1, this.Version);
          record.CalculateOnOpen = true;
          record.RecalculateAlways = true;
          record.PartOfSharedFormula = false;
          Ptg[] parsedExpression = record.ParsedExpression;
          if (parsedExpression != null && parsedExpression.Length == 1 && parsedExpression[0].TokenCode == FormulaToken.tExp)
          {
            ControlPtg controlPtg = (ControlPtg) parsedExpression[0];
            if (controlPtg.RowIndex == row && controlPtg.ColumnIndex == column)
            {
              Ptg[] ptgArray = FormulaUtil.ConvertSharedFormulaTokens(shared, (IWorkbook) book, record.Row, column1);
              record.ParsedExpression = ptgArray;
              int storeSize = record.GetStoreSize(this.Version);
              this.InsertRecordData(num1, num2 + 4, storeSize + 4, (BiffRecordRaw) record, allocationBlockSize);
              num2 = storeSize;
            }
          }
        }
      }
    }
  }

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    int num;
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (tbiffRecord == TBIFFRecord.LabelSST)
      {
        int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, iOffset, this.Version);
        int arrNewIndex = arrNewIndexes[sstIndex];
        LabelSSTRecord.SetSSTIndex(this.m_dataProvider, iOffset, arrNewIndex, this.Version);
      }
    }
  }

  public List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    string findValue,
    ExcelFindType flags,
    int iErrorCode,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    return this.Find(iFirstColumn, iLastColumn, findValue, flags, ExcelFindOptions.None, iErrorCode, bIsFindFirst, book);
  }

  public List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    string findValue,
    ExcelFindType flags,
    ExcelFindOptions findOptions,
    int iErrorCode,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    return this.Find(iFirstColumn, iLastColumn, findValue, flags, findOptions, iErrorCode, bIsFindFirst, book, 0);
  }

  internal List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    string findValue,
    ExcelFindType flags,
    ExcelFindOptions findOptions,
    int iErrorCode,
    bool bIsFindFirst,
    WorkbookImpl book,
    int sheetIndex)
  {
    return this.Find(iFirstColumn, iLastColumn, findValue, flags, findOptions, iErrorCode, bIsFindFirst, book, 0, (Regex) null);
  }

  internal List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    string findValue,
    ExcelFindType flags,
    ExcelFindOptions findOptions,
    int iErrorCode,
    bool bIsFindFirst,
    WorkbookImpl book,
    int sheetIndex,
    Regex wildCardRegex)
  {
    bool flag1 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag2 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag3 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag4 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag5 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag6 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    if (!flag1 && !flag3 && !flag2 && !flag4 && !flag5 && !flag6)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    List<long> longList = new List<long>();
    if (this.m_dataProvider == null || this.m_iUsedSize <= 0 || iLastColumn < this.m_iFirstColumn || iFirstColumn > this.m_iLastColumn)
      return longList;
    if (this.HasMultiRkBlank)
      this.Decompress(false, book.Application.RowStorageAllocationBlockSize);
    for (int index = this.LocateRecord(iFirstColumn, out bool _); index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      TBIFFRecord tbiffRecord1 = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      int num = (int) this.m_dataProvider.ReadInt16(index + 2);
      int row = this.GetRow(index);
      int column = this.GetColumn(index);
      bool flag7 = false;
      if (column <= iLastColumn)
      {
        switch (tbiffRecord1)
        {
          case TBIFFRecord.Formula:
            FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version);
            string str1 = book.FormulaUtil.ParseFormulaRecord(record1);
            if (str1 == bool.TrueString || str1 == bool.FalseString)
              str1 = str1.ToUpper();
            string first = "=" + str1;
            if ((flag2 || flag1) && first != findValue)
              flag7 = record1.ErrorValue != (byte) 0 && (int) record1.ErrorValue == iErrorCode;
            if (flag3)
              flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(first, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(first.ToLower()) : first.ToLower().Contains(findValue.ToLower()));
            else if (flag5)
            {
              string str2 = record1.Value.ToString();
              flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(str2, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(str2) : str2.Contains(findValue));
            }
            else if (flag6)
            {
              IRange range = book.Worksheets[sheetIndex][record1.Row + 1, record1.Column + 1];
              if (range != null)
              {
                string displayText = range.DisplayText;
                flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(displayText, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(displayText.ToLower()) : displayText.ToLower().Contains(findValue.ToLower()));
              }
            }
            int iOffset = index + num + 4;
            if (iOffset < this.m_iUsedSize)
            {
              TBIFFRecord tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
              if (tbiffRecord2 == TBIFFRecord.Array)
              {
                index = iOffset;
                iOffset = this.MoveNext(iOffset);
                tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
              }
              if (tbiffRecord2 == TBIFFRecord.String)
              {
                index = iOffset;
                if (flag4)
                {
                  StringRecord record2 = (StringRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
                  flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(record2.Value, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(record2.Value) : record2.Value == findValue);
                  break;
                }
                break;
              }
              break;
            }
            break;
          case TBIFFRecord.LabelSST:
            int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, index, this.Version);
            string text = book.InnerSST[sstIndex].Text;
            flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(text, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(text.ToLower()) : text.ToLower().Contains(findValue.ToLower()));
            break;
          case TBIFFRecord.Number:
          case TBIFFRecord.RK:
            IRange range1 = book.Worksheets[sheetIndex][row + 1, column + 1];
            string str3 = string.Empty;
            if (range1 != null && (range1.HasDateTime || flag6))
            {
              if (flag3)
                str3 = range1.Value;
              else if (flag6)
                str3 = range1.DisplayText;
            }
            else
              str3 = ((IDoubleValue) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version)).DoubleValue.ToString();
            flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(str3, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(str3) : str3.Contains(findValue));
            break;
          case TBIFFRecord.Label:
            if (flag1 || flag6)
            {
              LabelRecord record3 = (LabelRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version);
              flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(record3.Label, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(record3.Label) : record3.Label.ToLower().Contains(findValue.ToLower()));
              break;
            }
            break;
          case TBIFFRecord.BoolErr:
            BoolErrRecord record4 = (BoolErrRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version);
            if (flag1 || flag3 || flag6)
              flag7 = record4.IsErrorCode && (int) record4.BoolOrError == iErrorCode;
            if (!record4.IsErrorCode)
            {
              string upper = record4.Value.ToString().ToUpper();
              flag7 = findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(upper, findValue, findOptions, book, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(upper.ToLower()) : upper.ToLower().Contains(findValue.ToLower()));
              break;
            }
            break;
        }
        if (flag7)
        {
          long cellIndex = RangeImpl.GetCellIndex(column + 1, row + 1);
          longList.Add(cellIndex);
          if (bIsFindFirst)
            break;
        }
      }
      else
        break;
    }
    return longList;
  }

  internal static bool CheckStringValue(
    string first,
    string second,
    ExcelFindOptions options,
    WorkbookImpl book,
    Regex wildcardRegex)
  {
    StringComparison comparisonType = (options & ExcelFindOptions.MatchCase) != ExcelFindOptions.None ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
    bool flag = (options & ExcelFindOptions.MatchEntireCellContent) != ExcelFindOptions.None;
    if (wildcardRegex != null)
    {
      if (!flag)
        return Regex.IsMatch(first, wildcardRegex.ToString(), RegexOptions.None);
      return wildcardRegex.IsMatch(first) && wildcardRegex.Match(first).Length.Equals(first.Length);
    }
    if (!book.IsStartsOrEndsWith.HasValue || flag)
      return !flag ? first.IndexOf(second, 0, comparisonType) != -1 : first.Equals(second, comparisonType);
    bool? startsOrEndsWith = book.IsStartsOrEndsWith;
    return (!startsOrEndsWith.GetValueOrDefault() ? 0 : (startsOrEndsWith.HasValue ? 1 : 0)) == 0 ? first.EndsWith(second, comparisonType) : first.StartsWith(second, comparisonType);
  }

  public List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    double findValue,
    ExcelFindType flags,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) != (ExcelFindType) 0;
    bool flag2 = (flags & ExcelFindType.Number) != (ExcelFindType) 0;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    List<long> longList = new List<long>();
    if (this.m_dataProvider == null || this.m_iUsedSize <= 0 || iLastColumn < this.m_iFirstColumn || iFirstColumn > this.m_iLastColumn)
      return longList;
    this.Decompress(false, book.Application.RowStorageAllocationBlockSize);
    bool bFound;
    for (int index = this.LocateRecord(iFirstColumn, out bFound); index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      TBIFFRecord tbiffRecord1 = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      int num = (int) this.m_dataProvider.ReadInt16(index + 2);
      int row = this.GetRow(index);
      int column = this.GetColumn(index);
      bFound = false;
      if (column <= iLastColumn)
      {
        switch (tbiffRecord1)
        {
          case TBIFFRecord.Formula:
            if (flag1 || flag2)
              bFound = ((FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version)).Value.ToString().Contains(findValue.ToString());
            int iOffset = index + num + 4;
            if (iOffset < this.m_iUsedSize)
            {
              for (TBIFFRecord tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset); tbiffRecord2 == TBIFFRecord.Array || tbiffRecord2 == TBIFFRecord.String; tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset))
              {
                index = iOffset;
                iOffset = this.MoveNext(iOffset);
              }
              break;
            }
            break;
          case TBIFFRecord.Number:
          case TBIFFRecord.RK:
            if (flag2)
            {
              bFound = ((IDoubleValue) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version)).DoubleValue.ToString().Contains(findValue.ToString());
              break;
            }
            break;
        }
        if (bFound)
        {
          long cellIndex = RangeImpl.GetCellIndex(column + 1, row + 1);
          longList.Add(cellIndex);
          if (bIsFindFirst)
            break;
        }
      }
      else
        break;
    }
    return longList;
  }

  public List<long> Find(
    int iFirstColumn,
    int iLastColumn,
    byte findValue,
    bool bError,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    List<long> longList = new List<long>();
    if (this.m_dataProvider == null || this.m_iUsedSize <= 0 || iLastColumn < this.m_iFirstColumn || iFirstColumn > this.m_iLastColumn)
      return longList;
    for (int index = this.LocateRecord(iFirstColumn, out bool _); index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      TBIFFRecord tbiffRecord1 = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      int num = (int) this.m_dataProvider.ReadInt16(index + 2);
      int row = this.GetRow(index);
      int column = this.GetColumn(index);
      bool flag = false;
      if (column <= iLastColumn)
      {
        switch (tbiffRecord1)
        {
          case TBIFFRecord.Formula:
            int iOffset = index + num + 4;
            if (iOffset < this.m_iUsedSize)
            {
              for (TBIFFRecord tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset); iOffset < this.m_iUsedSize && tbiffRecord2 == TBIFFRecord.Array || tbiffRecord2 == TBIFFRecord.String; tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset))
              {
                index = iOffset;
                iOffset = this.MoveNext(iOffset);
              }
              break;
            }
            break;
          case TBIFFRecord.BoolErr:
            BoolErrRecord record = (BoolErrRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, index, this.Version);
            flag = record.IsErrorCode == bError && (int) record.BoolOrError == (int) findValue;
            break;
        }
        if (flag)
        {
          long cellIndex = RangeImpl.GetCellIndex(column + 1, row + 1);
          longList.Add(cellIndex);
          if (bIsFindFirst)
            break;
        }
      }
      else
        break;
    }
    return longList;
  }

  public void Find(Dictionary<int, object> dictIndexes, List<long> arrRanges)
  {
    if (dictIndexes == null || dictIndexes.Count == 0 || this.m_iUsedSize <= 0)
      return;
    for (int index = 0; index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      if (this.m_dataProvider.ReadInt16(index) == (short) 253)
      {
        int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, index, this.Version);
        if (dictIndexes.ContainsKey(sstIndex))
        {
          int firstRow = this.GetRow(index) + 1;
          int firstColumn = this.GetColumn(index) + 1;
          arrRanges.Add(RangeImpl.GetCellIndex(firstColumn, firstRow));
        }
      }
    }
  }

  public int MoveNextCell(int iOffset)
  {
    if (iOffset < this.m_iUsedSize)
    {
      TBIFFRecord tbiffRecord;
      do
      {
        int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
        iOffset += 4 + num;
        if (iOffset < this.m_iUsedSize)
          tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
        else
          break;
      }
      while (iOffset < this.m_iUsedSize && (tbiffRecord == TBIFFRecord.Array || tbiffRecord == TBIFFRecord.String));
    }
    return iOffset;
  }

  public void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats, int iBlockSize)
  {
    if (this.m_iUsedSize <= 0)
      return;
    int xfIndex1;
    for (int index = 0; index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      switch ((TBIFFRecord) this.m_dataProvider.ReadInt16(index))
      {
        case TBIFFRecord.MulRK:
        case TBIFFRecord.MulBlank:
          throw new NotImplementedException();
        case TBIFFRecord.String:
        case TBIFFRecord.Array:
          continue;
        default:
          int xfIndex2 = (int) this.GetXFIndex(index, false);
          if (dictFormats.TryGetValue(xfIndex2, out xfIndex1))
          {
            this.SetXFIndex(index, (ushort) xfIndex1);
            continue;
          }
          continue;
      }
    }
    int extendedFormatIndex = (int) this.ExtendedFormatIndex;
    if (!dictFormats.TryGetValue(extendedFormatIndex, out xfIndex1))
      return;
    this.ExtendedFormatIndex = (ushort) xfIndex1;
  }

  public void UpdateExtendedFormatIndex(int[] arrFormats, int iBlockSize)
  {
    if (this.m_iUsedSize <= 0)
      return;
    for (int index = 0; index < this.m_iUsedSize; index = this.MoveNext(index))
    {
      switch ((TBIFFRecord) this.m_dataProvider.ReadInt16(index))
      {
        case TBIFFRecord.MulRK:
        case TBIFFRecord.MulBlank:
          throw new NotImplementedException();
        case TBIFFRecord.String:
        case TBIFFRecord.Array:
          continue;
        default:
          int xfIndex = (int) this.GetXFIndex(index, false);
          int arrFormat = arrFormats[xfIndex];
          this.SetXFIndex(index, (ushort) arrFormat);
          continue;
      }
    }
    int extendedFormatIndex = (int) this.ExtendedFormatIndex;
    this.ExtendedFormatIndex = (ushort) arrFormats[extendedFormatIndex];
  }

  public void UpdateExtendedFormatIndex(int maxCount, int defaultXF)
  {
    int num;
    for (int index = 0; index < this.m_iUsedSize; index += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      num = (int) this.m_dataProvider.ReadInt16(index + 2);
      if (tbiffRecord != TBIFFRecord.Array && tbiffRecord != TBIFFRecord.String && (int) this.GetXFIndex(index, false) >= maxCount)
        this.SetXFIndex(index, (ushort) defaultXF);
    }
  }

  public void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes, IncreaseIndex method)
  {
    if (dictUpdatedIndexes == null)
      throw new ArgumentNullException(nameof (dictUpdatedIndexes));
    if (dictUpdatedIndexes.Count == 0)
      return;
    int num1;
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset += num1 + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num1 = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (tbiffRecord == TBIFFRecord.LabelSST)
      {
        int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, iOffset, this.Version);
        int num2;
        if (dictUpdatedIndexes.TryGetValue(sstIndex, out num2))
        {
          int num3 = num2;
          LabelSSTRecord.SetSSTIndex(this.m_dataProvider, iOffset, num3, this.Version);
          method(num3);
        }
      }
    }
  }

  public void AppendRecordData(short type, short length, BiffRecordRaw data, int iBlockSize)
  {
    this.EnsureSize(this.m_iUsedSize + 4 + (int) length, iBlockSize);
    this.m_dataProvider.WriteInt16(this.m_iUsedSize, type);
    this.m_iUsedSize += 2;
    this.m_dataProvider.WriteInt16(this.m_iUsedSize, length);
    this.m_iUsedSize += 2;
    data.InfillInternalData(this.m_dataProvider, this.m_iUsedSize, this.Version);
    this.m_iUsedSize += (int) length;
  }

  [CLSCompliant(false)]
  public void AppendRecordData(short type, short length, byte[] data, int iBlockSize)
  {
    this.EnsureSize(this.m_iUsedSize + 4 + (int) length, iBlockSize);
    this.m_dataProvider.WriteInt16(this.m_iUsedSize, type);
    this.m_iUsedSize += 2;
    this.m_dataProvider.WriteInt16(this.m_iUsedSize, length);
    this.m_iUsedSize += 2;
    this.m_dataProvider.WriteBytes(this.m_iUsedSize, data, 0, (int) length);
    this.m_iUsedSize += (int) length;
  }

  public void AppendRecordData(int length, byte[] data, int iBlockSize)
  {
    this.EnsureSize(this.m_iUsedSize + length, iBlockSize);
    this.m_dataProvider.WriteBytes(this.m_iUsedSize, data, 0, length);
    this.m_iUsedSize += length;
  }

  public void InsertRecordData(int columnIndex, int length, byte[] data, int iBlockSize)
  {
    this.EnsureSize(this.m_iUsedSize + length, iBlockSize);
    bool bFound;
    int num = this.LocateRecord(columnIndex, out bFound);
    if (bFound)
    {
      this.RemoveRecord(num);
      this.InsertRecordData(columnIndex, length, data, iBlockSize);
    }
    else
    {
      this.m_dataProvider.MoveMemory(num + length, num, this.m_iUsedSize - num);
      this.m_dataProvider.WriteBytes(num, data, 0, length);
      this.m_iUsedSize += length;
    }
  }

  [CLSCompliant(false)]
  public void AppendRecordData(
    BiffRecordRaw[] records,
    byte[] arrBuffer,
    bool bIgnoreStyles,
    int iBlockSize)
  {
    int length = records.Length;
    int num = 0;
    for (int index = 0; index < length; ++index)
    {
      BiffRecordRaw record = records[index];
      num += 4 + record.GetStoreSize(this.Version);
    }
    this.EnsureSize(this.m_iUsedSize + num, iBlockSize);
    for (int index = 0; index < length; ++index)
    {
      BiffRecordRaw record = records[index];
      this.AppendRecordData((short) record.RecordCode, (short) record.GetStoreSize(this.Version), record, iBlockSize);
    }
  }

  public void Decompress(bool bIgnoreStyles, int iBlockSize)
  {
    if (!this.HasMultiRkBlank)
      return;
    MulBlankRecord record1 = (MulBlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulBlank);
    MulRKRecord record2 = (MulRKRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulRK);
    int iSizeDelta;
    List<int> multiRecordsOffsets = this.GetMultiRecordsOffsets(record1, record2, out iSizeDelta);
    if (iSizeDelta == 0)
      return;
    this.EnsureSize(this.m_iUsedSize + iSizeDelta, iBlockSize);
    this.DecompressStorage(multiRecordsOffsets, iSizeDelta, record1, record2, bIgnoreStyles);
    this.HasMultiRkBlank = false;
    this.HasRkBlank = true;
    this.m_iCurrentColumn = -1;
    this.m_iCurrentOffset = -1;
  }

  public void Compress()
  {
    if (this.m_iUsedSize <= 0 || !this.HasRkBlank)
      return;
    RowStorage.WriteData userData = new RowStorage.WriteData();
    userData.UsedSize = 0;
    this.DefragmentDataStorage(new RowStorage.DefragmentHelper(this.CompressRKRecords), new RowStorage.DefragmentHelper(this.CompressBlankRecords), new RowStorage.DefragmentHelper(this.CompressRecord), (object) userData);
    this.m_iUsedSize = userData.UsedSize;
    this.HasRkBlank = false;
  }

  public bool PrepareRowData(
    SSTDictionary sst,
    ref Dictionary<long, SharedFormulaRecord> arrShared)
  {
    if (sst == null)
      throw new ArgumentNullException(nameof (sst));
    int firstRow = -1;
    int firstColumn = -1;
    int num1 = 0;
    List<int> intList = new List<int>();
    bool flag = true;
    int num2;
    for (; num1 < this.m_iUsedSize; num1 += num2 + 4)
    {
      if (num1 < 0)
      {
        flag = false;
        break;
      }
      int num3 = this.m_dataProvider.ReadInt32(num1);
      TBIFFRecord tbiffRecord = (TBIFFRecord) (num3 & (int) ushort.MaxValue);
      num2 = num3 >> 16 /*0x10*/;
      switch (tbiffRecord)
      {
        case TBIFFRecord.Formula:
          firstRow = this.GetRow(num1);
          firstColumn = this.GetColumn(num1);
          break;
        case TBIFFRecord.MulRK:
        case TBIFFRecord.MulBlank:
          this.m_options |= RowStorage.StorageOptions.HasMultiRKBlank;
          break;
        case TBIFFRecord.LabelSST:
          int index = this.m_dataProvider.ReadInt32(num1 + 6 + 4);
          sst.AddIncrease(index);
          break;
        case TBIFFRecord.SharedFormula2:
          intList.Add(num1);
          if (arrShared == null)
            arrShared = new Dictionary<long, SharedFormulaRecord>();
          SharedFormulaRecord record = (SharedFormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, num1, this.Version);
          long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
          arrShared.Add(cellIndex, record);
          break;
      }
    }
    int count = intList.Count;
    if (count > 0)
    {
      for (int index = count - 1; index >= 0; --index)
        this.RemoveRecord(intList[index]);
    }
    return flag;
  }

  [CLSCompliant(false)]
  public void UpdateRowInfo(RowRecord row, bool useFastParsing)
  {
    if (useFastParsing)
    {
      this.m_iFirstColumn = (int) row.FirstColumn;
      this.m_iLastColumn = (int) row.LastColumn;
    }
    this.m_optionFlags = (RowRecord.OptionFlags) row.Options;
    this.m_usHeight = row.Height;
    this.m_usXFIndex = row.ExtendedFormatIndex;
  }

  [CLSCompliant(false)]
  public RowRecord CreateRowRecord(WorkbookImpl book)
  {
    RowRecord record = (RowRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Row);
    record.FirstColumn = (ushort) Math.Max(0, this.m_iFirstColumn);
    record.LastColumn = (ushort) Math.Max(0, this.m_iLastColumn);
    record.Height = this.m_usHeight;
    record.Options = (int) this.m_optionFlags;
    record.ExtendedFormatIndex = (int) this.m_usXFIndex > book.MaxXFCount ? (ushort) book.DefaultXFIndex : this.m_usXFIndex;
    return record;
  }

  public void CopyRowRecordFrom(RowStorage sourceRow)
  {
    this.m_usHeight = sourceRow.m_usHeight;
    this.m_optionFlags = sourceRow.m_optionFlags;
    this.m_usXFIndex = sourceRow.ExtendedFormatIndex;
    this.m_autoHeight = sourceRow.AutoHeight;
  }

  public void SetDefaultRowOptions()
  {
    this.m_optionFlags = RowRecord.OptionFlags.ShowOutlineGroups;
  }

  public void UpdateColumnIndexes(int iColumnIndex, int iLastColumnIndex)
  {
    this.m_iFirstColumn = this.m_iUsedSize == 0 || this.m_iFirstColumn < 0 ? iColumnIndex : (this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, iColumnIndex));
    this.m_iLastColumn = Math.Max(iLastColumnIndex, this.m_iLastColumn);
  }

  public void SetCellPositionSize(int newSize, int iBlockSize, ExcelVersion version)
  {
    if (this.CellPositionSize == newSize)
      return;
    switch (newSize)
    {
      case 4:
        this.ShrinkDataStorage();
        break;
      case 8:
        this.ExtendDataStorage(iBlockSize);
        break;
      default:
        throw new NotSupportedException();
    }
    this.m_version = version;
  }

  public int GetXFIndexByColumn(int column)
  {
    bool bFound;
    bool bMul;
    int recordStart = this.LocateRecord(column, out bFound, out bMul, true);
    return !bFound ? int.MinValue : (int) this.GetXFIndex(recordStart, bMul);
  }

  public void ReAddAllStrings(SSTDictionary sst)
  {
    int iOffset = 0;
    int num1 = 253;
    int num2;
    for (; iOffset < this.m_iUsedSize; iOffset += num2 + 4)
    {
      int num3 = (int) this.m_dataProvider.ReadInt16(iOffset);
      num2 = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (num3 == num1)
      {
        int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, iOffset, this.Version);
        sst.AddIncrease(sstIndex);
      }
    }
  }

  public void SetVersion(ExcelVersion version, int iBlockSize)
  {
    if (this.Version == version)
      return;
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        this.ShrinkDataStorage();
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        this.ExtendDataStorage(iBlockSize);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
    this.m_version = version;
  }

  private void UpdateColumns()
  {
    this.m_iFirstColumn = -1;
    this.m_iLastColumn = -1;
    int num;
    for (int index = 0; index < this.m_iUsedSize; index += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      num = (int) this.m_dataProvider.ReadInt16(index + 2);
      int column = this.GetColumn(index);
      if (tbiffRecord != TBIFFRecord.Array && tbiffRecord != TBIFFRecord.String)
      {
        this.AccessColumn(column);
        if (tbiffRecord == TBIFFRecord.MulRK || tbiffRecord == TBIFFRecord.MulBlank)
          this.AccessColumn((int) this.m_dataProvider.ReadInt16(index + num + 4 - 2));
      }
    }
  }

  public void IterateCells(RowStorage.CellMethod method, object data)
  {
    int num;
    for (int index = 0; index < this.m_iUsedSize; index += num + 4)
    {
      TBIFFRecord recordType = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
      num = (int) this.m_dataProvider.ReadInt16(index + 2);
      method(recordType, index, data);
    }
  }

  public void MarkCellUsedReferences(TBIFFRecord recordType, int offset, object data)
  {
    if (recordType != TBIFFRecord.Formula && recordType != TBIFFRecord.Array)
      return;
    FormulaUtil.MarkUsedReferences((BiffRecordFactory.GetRecord(this.m_dataProvider, offset, this.Version) as IFormulaRecord).Formula, (bool[]) data);
  }

  public void UpdateReferenceIndexes(TBIFFRecord recordType, int offset, object data)
  {
    if (recordType != TBIFFRecord.Formula && recordType != TBIFFRecord.Array)
      return;
    FormulaUtil.UpdateReferenceIndexes((BiffRecordFactory.GetRecord(this.m_dataProvider, offset, this.Version) as IFormulaRecord).Formula, (int[]) data);
  }

  internal void CreateDataProvider(IntPtr heapHandle)
  {
    if (this.m_dataProvider != null)
      return;
    this.m_dataProvider = ApplicationImpl.CreateDataProvider(heapHandle);
    if (this.m_book != null && this.m_book.MaxImportColumns > 1)
      this.m_dataProvider.EnsureCapacity(18 * this.m_book.MaxImportColumns);
    else
      this.m_dataProvider.EnsureCapacity(18);
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    this.IterateCells(new RowStorage.CellMethod(this.MarkCellUsedReferences), (object) usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    this.IterateCells(new RowStorage.CellMethod(this.UpdateReferenceIndexes), (object) arrUpdatedIndexes);
  }

  public int FindRecord(TBIFFRecord recordType, int startColumn, int endColumn)
  {
    if (startColumn > this.m_iLastColumn || endColumn > this.m_iLastColumn)
      return endColumn + 1;
    bool bFound;
    int num = this.LocateRecord(startColumn, out bFound);
    if (!bFound && num >= this.m_iUsedSize)
      return endColumn + 1;
    int record = bFound ? startColumn : this.GetColumn(num);
    if (num >= this.m_iUsedSize)
      record = endColumn + 1;
    TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(num);
    while (record <= endColumn && tbiffRecord != recordType)
    {
      num = this.MoveNext(num);
      if (num >= this.m_iUsedSize)
      {
        record = endColumn + 1;
        break;
      }
      tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(num);
      switch (tbiffRecord)
      {
        case TBIFFRecord.String:
        case TBIFFRecord.Array:
          continue;
        default:
          record = this.GetColumn(num);
          continue;
      }
    }
    if (record <= endColumn && num < this.m_iUsedSize)
    {
      this.m_iCurrentColumn = record;
      this.m_iCurrentOffset = num;
    }
    return record;
  }

  public int FindFirstCell(int startColumn, int endColumn)
  {
    int recordStart = this.LocateRecord(startColumn, out bool _);
    return recordStart < this.UsedSize ? this.GetColumn(recordStart) : endColumn + 1;
  }

  internal void GetUsedNames(Dictionary<int, object> result)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset = this.MoveNext(iOffset))
    {
      switch ((TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset))
      {
        case TBIFFRecord.Formula:
        case TBIFFRecord.Array:
          Ptg[] formula = ((IFormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version)).Formula;
          this.AddNamedRangeTokens(result, formula);
          break;
      }
    }
  }

  private void AddNamedRangeTokens(Dictionary<int, object> result, Ptg[] tokens)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (tokens == null)
      return;
    for (int index = tokens.Length - 1; index >= 0; --index)
    {
      if (tokens[index] is NamePtg token)
        result[token.ExternNameIndexInt - 1] = (object) null;
    }
  }

  private void InsertRecordData(int iOffset, BiffRecordRaw[] records)
  {
    int index = 0;
    for (int length = records.Length; index < length; ++index)
    {
      BiffRecordRaw record = records[index];
      this.m_dataProvider.WriteInt16(iOffset, (short) record.RecordCode);
      iOffset += 2;
      int storeSize = record.GetStoreSize(this.Version);
      this.m_dataProvider.WriteInt16(iOffset, (short) storeSize);
      iOffset += 2;
      record.InfillInternalData(this.m_dataProvider, iOffset, this.Version);
      iOffset += storeSize;
    }
  }

  private void ShrinkDataStorage()
  {
    if (this.CellPositionSize == 4 | this.m_iUsedSize == 0)
      return;
    int iOffset1 = 0;
    int num1 = 0;
    int maxValue1 = (int) ushort.MaxValue;
    int maxValue2 = (int) byte.MaxValue;
    if (this.m_iFirstColumn > maxValue2)
    {
      this.m_iFirstColumn = 0;
      this.m_iLastColumn = 0;
      this.m_iUsedSize = num1;
    }
    else
    {
      int num2;
      int iOffset2;
      for (; iOffset1 < this.m_iUsedSize; iOffset1 = iOffset2 + num2)
      {
        TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset1);
        int iOffset3 = iOffset1 + 2;
        num2 = (int) this.m_dataProvider.ReadInt16(iOffset3);
        iOffset2 = iOffset3 + 2;
        if (tbiffRecord != TBIFFRecord.Array && tbiffRecord != TBIFFRecord.String)
        {
          int num3 = this.m_dataProvider.ReadInt32(iOffset2);
          int num4 = this.m_dataProvider.ReadInt32(iOffset2 + 4);
          if (num4 <= maxValue2 && num3 <= maxValue1)
          {
            this.m_iLastColumn = num4;
            this.m_dataProvider.WriteInt16(num1, (short) tbiffRecord);
            int iOffset4 = num1 + 2;
            int num5;
            switch (tbiffRecord)
            {
              case TBIFFRecord.Formula:
                FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset2 - 4, ExcelVersion.Excel2007);
                int storeSize = record.GetStoreSize(ExcelVersion.Excel97to2003);
                this.m_dataProvider.WriteInt16(iOffset4, (short) storeSize);
                int iOffset5 = iOffset4 + 2;
                FormulaRecord.ConvertFormulaTokens(record.ParsedExpression, true);
                record.InfillInternalData(this.m_dataProvider, iOffset5, ExcelVersion.Excel97to2003);
                num1 = iOffset5 + storeSize;
                continue;
              case TBIFFRecord.MulRK:
                num5 = 1;
                break;
              default:
                num5 = tbiffRecord == TBIFFRecord.MulBlank ? 1 : 0;
                break;
            }
            bool flag = num5 != 0;
            int iLength = num2 - 4;
            if (flag)
              iLength -= 2;
            this.m_dataProvider.WriteInt16(iOffset4, (short) iLength);
            int iOffset6 = iOffset4 + 2;
            this.m_dataProvider.WriteUInt16(iOffset6, (ushort) num3);
            int iOffset7 = iOffset6 + 2;
            this.m_dataProvider.WriteInt16(iOffset7, (short) num4);
            int iDestOffset = iOffset7 + 2;
            this.m_dataProvider.CopyTo(iOffset2 + 8, this.m_dataProvider, iDestOffset, iLength);
            num1 = iDestOffset + (iLength - 4);
          }
          else
            break;
        }
        else if (tbiffRecord == TBIFFRecord.Array)
        {
          BiffRecordRaw record = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset2 - 4, ExcelVersion.Excel2007);
          int storeSize = record.GetStoreSize(ExcelVersion.Excel97to2003);
          this.m_dataProvider.WriteInt16(num1, (short) tbiffRecord);
          int iOffset8 = num1 + 2;
          this.m_dataProvider.WriteInt16(iOffset8, (short) storeSize);
          int iOffset9 = iOffset8 + 2;
          record.InfillInternalData(this.m_dataProvider, iOffset9, ExcelVersion.Excel97to2003);
          num1 = iOffset9 + storeSize;
        }
        else
        {
          this.m_dataProvider.CopyTo(iOffset2 - 4, this.m_dataProvider, num1, num2 + 4);
          num1 += num2 + 4;
        }
      }
      this.m_iUsedSize = num1;
    }
  }

  private void ExtendDataStorage(int iBlockSize)
  {
    if (this.CellPositionSize == 8 || this.m_iUsedSize == 0)
      return;
    int iOffset1 = 0;
    int iWriteOffset = 0;
    DataProvider dataProvider = ApplicationImpl.CreateDataProvider(this.GetHeapHandle());
    List<FormulaRecord> arrFormulas = new List<FormulaRecord>();
    int enlargedDataSize = this.GetEnlargedDataSize(arrFormulas);
    dataProvider.EnsureCapacity((enlargedDataSize / iBlockSize + 1) * iBlockSize);
    int index = 0;
    int iLength1;
    int num;
    for (; iOffset1 < this.m_iUsedSize; iOffset1 = num + iLength1)
    {
      TBIFFRecord code = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset1);
      int iOffset2 = iOffset1 + 2;
      iLength1 = (int) this.m_dataProvider.ReadInt16(iOffset2);
      num = iOffset2 + 2;
      if (code != TBIFFRecord.Array && code != TBIFFRecord.String)
      {
        int iRow = (int) this.m_dataProvider.ReadUInt16(num);
        int iColumn = (int) this.m_dataProvider.ReadUInt16(num + 2);
        dataProvider.WriteInt16(iWriteOffset, (short) code);
        iWriteOffset += 2;
        if (code != TBIFFRecord.Formula)
        {
          this.EnlargeCellRecord(dataProvider, ref iWriteOffset, num, code, iLength1, iRow, iColumn);
        }
        else
        {
          FormulaRecord formula = arrFormulas[index];
          FormulaRecord.ConvertFormulaTokens(formula.ParsedExpression, false);
          ++index;
          this.EnlargeFormulaRecord(dataProvider, ref iWriteOffset, formula);
        }
      }
      else if (code == TBIFFRecord.Array)
      {
        BiffRecordRaw record = BiffRecordFactory.GetRecord(this.m_dataProvider, num - 4, ExcelVersion.Excel97to2003);
        int storeSize = record.GetStoreSize(ExcelVersion.Excel2007);
        dataProvider.WriteInt16(iWriteOffset, (short) code);
        int iOffset3 = iWriteOffset + 2;
        dataProvider.WriteInt16(iOffset3, (short) storeSize);
        int iOffset4 = iOffset3 + 2;
        record.InfillInternalData(dataProvider, iOffset4, ExcelVersion.Excel2007);
        iWriteOffset = iOffset4 + storeSize;
      }
      else
      {
        int iLength2 = iLength1 + 4;
        this.m_dataProvider.CopyTo(num - 4, dataProvider, iWriteOffset, iLength2);
        iWriteOffset += iLength2;
      }
    }
    this.m_iUsedSize = enlargedDataSize == iWriteOffset ? iWriteOffset : throw new InvalidOperationException("Wrong offset");
    this.m_iCurrentColumn = -1;
    this.m_iCurrentOffset = -1;
    this.m_dataProvider.Dispose();
    this.m_dataProvider = dataProvider;
  }

  private void EnlargeCellRecord(
    DataProvider result,
    ref int iWriteOffset,
    int iReadOffset,
    TBIFFRecord code,
    int iLength,
    int iRow,
    int iColumn)
  {
    bool flag = code == TBIFFRecord.MulRK || code == TBIFFRecord.MulBlank;
    int num1 = iLength + 4;
    if (flag)
      num1 += 2;
    result.EnsureCapacity(iWriteOffset + num1 + 2);
    result.WriteInt16(iWriteOffset, (short) num1);
    iWriteOffset += 2;
    result.WriteInt32(iWriteOffset, iRow);
    iWriteOffset += 4;
    result.WriteInt32(iWriteOffset, iColumn);
    iWriteOffset += 4;
    this.m_dataProvider.CopyTo(iReadOffset + 4, result, iWriteOffset, iLength - 4);
    iWriteOffset += iLength - 4;
    if (!flag)
      return;
    int num2 = (int) this.m_dataProvider.ReadInt16(iReadOffset + iLength - 2);
    result.WriteInt32(iWriteOffset - 2, num2);
    iWriteOffset += 2;
  }

  private void EnlargeFormulaRecord(
    DataProvider result,
    ref int iWriteOffset,
    FormulaRecord formula)
  {
    int storeSize = formula.GetStoreSize(ExcelVersion.Excel2007);
    result.WriteInt16(iWriteOffset, (short) storeSize);
    iWriteOffset += 2;
    result.EnsureCapacity(iWriteOffset + storeSize);
    formula.InfillInternalData(result, iWriteOffset, ExcelVersion.Excel2007);
    iWriteOffset += storeSize;
  }

  private int GetEnlargedDataSize(List<FormulaRecord> arrFormulas)
  {
    if (arrFormulas == null)
      throw new ArgumentNullException(nameof (arrFormulas));
    int iOffset = 0;
    int enlargedDataSize = 0;
    int num;
    for (; iOffset < this.m_iUsedSize; iOffset += num + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      switch (tbiffRecord)
      {
        case TBIFFRecord.Formula:
          FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
          arrFormulas.Add(record1);
          enlargedDataSize += record1.GetStoreSize(ExcelVersion.Excel2007) + 4;
          break;
        case TBIFFRecord.MulRK:
        case TBIFFRecord.MulBlank:
          enlargedDataSize += num + 4 + 6;
          break;
        case TBIFFRecord.String:
          enlargedDataSize += num + 4;
          break;
        case TBIFFRecord.Array:
          ArrayRecord record2 = (ArrayRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
          enlargedDataSize += record2.GetStoreSize(ExcelVersion.Excel2007) + 4;
          break;
        default:
          enlargedDataSize += num + 4 + 4;
          break;
      }
    }
    return enlargedDataSize;
  }

  private int GetRecordCount()
  {
    int num1 = 0;
    int recordCount = 0;
    while (num1 < this.m_iUsedSize)
    {
      int num2 = (int) this.m_dataProvider.ReadInt16(num1 + 2);
      num1 += num2 + 4;
      ++recordCount;
    }
    return recordCount;
  }

  private void UpdateRecordsAfterCopy(
    SSTDictionary sourceSST,
    SSTDictionary destSST,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicNameIndexes,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dictExternSheets)
  {
    if (sourceSST == null)
      throw new ArgumentNullException(nameof (sourceSST));
    if (destSST == null)
      throw new ArgumentNullException(nameof (destSST));
    int num = 0;
    int extendedFormatIndex = (int) this.ExtendedFormatIndex;
    bool flag = sourceSST == destSST;
    WorkbookImpl workbook = destSST.Workbook;
    if (hashExtFormatIndexes.ContainsKey(extendedFormatIndex))
      this.ExtendedFormatIndex = this.IsFormatted ? (ushort) hashExtFormatIndexes[extendedFormatIndex] : (ushort) workbook.DefaultXFIndex;
    int iLength;
    for (; num < this.m_iUsedSize; num += iLength + 4)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(num);
      iLength = (int) this.m_dataProvider.ReadInt16(num + 2);
      if (tbiffRecord != TBIFFRecord.String && tbiffRecord != TBIFFRecord.Array)
      {
        if (!flag)
        {
          if (hashExtFormatIndexes != null && hashExtFormatIndexes.Count > 0)
          {
            switch (tbiffRecord)
            {
              case TBIFFRecord.MulRK:
                this.UpdateMulReference(hashExtFormatIndexes, num, iLength, true);
                break;
              case TBIFFRecord.MulBlank:
                this.UpdateMulReference(hashExtFormatIndexes, num, iLength, false);
                break;
              default:
                int xfIndex = (int) this.GetXFIndex(num, false);
                if (hashExtFormatIndexes.ContainsKey(xfIndex))
                {
                  int hashExtFormatIndex = hashExtFormatIndexes[xfIndex];
                  this.SetXFIndex(num, (ushort) hashExtFormatIndex);
                  break;
                }
                break;
            }
          }
          switch (tbiffRecord)
          {
            case TBIFFRecord.Formula:
              this.UpdateFormulaRefs(sourceSST, destSST, num, hashWorksheetNames, dicNameIndexes, iLength, dictExternSheets);
              continue;
            case TBIFFRecord.LabelSST:
              this.UpdateLabelSST(sourceSST, destSST, num, false, dicFontIndexes);
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (tbiffRecord)
          {
            case TBIFFRecord.Formula:
              this.UpdateFormulaRefs(sourceSST, destSST, num, hashWorksheetNames, dicNameIndexes, iLength, dictExternSheets);
              continue;
            case TBIFFRecord.LabelSST:
              this.UpdateLabelSST(sourceSST, destSST, num, true, dicFontIndexes);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private void UpdateMulReference(
    Dictionary<int, int> hashXFIndexes,
    int iOffset,
    int iLength,
    bool bIsMulRK)
  {
    iLength = iOffset + 4 + iLength - 2;
    iOffset += 8;
    int num = bIsMulRK ? 6 : 2;
    for (int iOffset1 = iOffset; iOffset1 < iLength; iOffset1 += num)
    {
      int key = (int) this.m_dataProvider.ReadInt16(iOffset1);
      if (hashXFIndexes.TryGetValue(key, out key))
        this.m_dataProvider.WriteInt16(iOffset1, (short) key);
    }
  }

  private void UpdateLabelSST(
    SSTDictionary sourceSST,
    SSTDictionary destSST,
    int iOffset,
    bool bIsLocal,
    Dictionary<int, int> dicFontIndexes)
  {
    if (sourceSST == null)
      throw new ArgumentNullException(nameof (sourceSST));
    if (destSST == null)
      throw new ArgumentNullException(nameof (destSST));
    int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, iOffset, this.Version);
    if (bIsLocal)
    {
      destSST.AddIncrease(sstIndex);
    }
    else
    {
      TextWithFormat textWithFormat = sourceSST[sstIndex];
      object key = textWithFormat.FormattingRunsCount == 0 ? (object) textWithFormat.Text : (object) textWithFormat.Clone(dicFontIndexes);
      int iNewIndex = destSST.AddIncrease(key);
      LabelSSTRecord.SetSSTIndex(this.m_dataProvider, iOffset, iNewIndex, this.Version);
    }
  }

  private void UpdateFormulaRefs(
    SSTDictionary sourceSST,
    SSTDictionary destSST,
    int iOffset,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicNameIndexes,
    int iLength,
    Dictionary<int, int> dictExternSheets)
  {
    FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
    record.ParseStructure(this.m_dataProvider, iOffset + 4, iLength, this.Version);
    WorkbookImpl workbook = destSST.Workbook;
    if (!this.UpdateNameSheetReferences(record, hashWorksheetNames, sourceSST.Workbook, workbook, dicNameIndexes, dictExternSheets))
      return;
    record.IsFillFromExpression = true;
    int storeSize = record.GetStoreSize(this.Version);
    this.InsertRecordData(iOffset, iLength + 4, storeSize + 4, (BiffRecordRaw) record, workbook.Application.RowStorageAllocationBlockSize);
  }

  private bool UpdateSheetReferences(
    FormulaRecord formula,
    IDictionary dicSheetNames,
    WorkbookImpl sourceBook,
    WorkbookImpl destBook)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    if (sourceBook == null)
      throw new ArgumentNullException("book");
    Ptg[] parsedExpression = formula.ParsedExpression;
    formula.GetStoreSize(this.Version);
    bool flag = false;
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      Ptg ptg = parsedExpression[index];
      if (ptg is ISheetReference)
      {
        ISheetReference sheetReference = (ISheetReference) ptg;
        ushort refIndex = sheetReference.RefIndex;
        string str = sourceBook.GetSheetNameByReference((int) refIndex);
        if (dicSheetNames != null && dicSheetNames.Contains((object) str))
          str = (string) dicSheetNames[(object) str];
        int num = destBook.AddSheetReference(str);
        sheetReference.RefIndex = (ushort) num;
        flag = true;
      }
    }
    return flag;
  }

  private bool UpdateNameReferences(FormulaRecord formula, Dictionary<int, int> dicNameIndexes)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    if (dicNameIndexes == null)
      throw new ArgumentNullException(nameof (dicNameIndexes));
    Ptg[] parsedExpression = formula.ParsedExpression;
    formula.GetStoreSize(this.Version);
    bool flag = false;
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      if (parsedExpression[index] is NamePtg namePtg)
      {
        int key = namePtg.ExternNameIndexInt - 1;
        int num = dicNameIndexes.ContainsKey(key) ? dicNameIndexes[key] : key;
        namePtg.ExternNameIndexInt = num + 1;
        flag = true;
      }
    }
    return flag;
  }

  private bool UpdateNameSheetReferences(
    FormulaRecord formula,
    Dictionary<string, string> dicSheetNames,
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    Dictionary<int, int> dicNameIndexes,
    Dictionary<int, int> dictExternSheets)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    if (sourceBook == null)
      throw new ArgumentNullException("book");
    if (dicNameIndexes == null)
      throw new ArgumentNullException(nameof (dicNameIndexes));
    Ptg[] ptgArray = formula.ParsedExpression;
    formula.GetStoreSize(this.Version);
    bool flag = false;
    ArrayRecord array = (ArrayRecord) null;
    int iColumnIndex = 0;
    if (ptgArray.Length > 0 && ptgArray[0] is ControlPtg)
    {
      iColumnIndex = (ptgArray[0] as ControlPtg).ColumnIndex;
      array = this.GetArrayRecord(iColumnIndex);
      if (array != null)
        ptgArray = array.Formula;
    }
    int index1 = 0;
    for (int length = ptgArray.Length; index1 < length; ++index1)
    {
      Ptg token1 = ptgArray[index1];
      switch (token1)
      {
        case ISheetReference _:
          ISheetReference sheetReference = (ISheetReference) token1;
          ushort refIndex1 = sheetReference.RefIndex;
          if (!sourceBook.IsExternalReference((int) refIndex1))
          {
            string sheetNameByReference = sourceBook.GetSheetNameByReference((int) refIndex1);
            string inputSheetName = sheetNameByReference;
            if (dicSheetNames != null && sheetNameByReference != null && sheetNameByReference != "#REF" && dicSheetNames.ContainsKey(sheetNameByReference))
              inputSheetName = dicSheetNames[sheetNameByReference];
            if (token1 is NameXPtg token2)
            {
              int refIndex2 = (int) token2.RefIndex;
              int num = dictExternSheets.ContainsKey(refIndex2) ? dictExternSheets[refIndex2] : refIndex2;
              int firstSheet = (int) sourceBook.ExternSheet.RefList[refIndex2].FirstSheet;
              if (sourceBook != destBook && sheetNameByReference != null && sheetNameByReference != "#REF" && !dicSheetNames.ContainsKey(sheetNameByReference))
              {
                num = RowStorage.ChangeExternSheet(sourceBook, destBook, firstSheet, num);
                int supBookIndex = (int) destBook.ExternSheet.RefList[num].SupBookIndex;
                ExternWorkbookImpl externWorkbook = destBook.ExternWorkbooks[supBookIndex];
                IName name = sourceBook.InnerNamesColection[(int) token2.NameIndex - 1];
                if (name != null && name.Name != null)
                {
                  int index2;
                  if (!externWorkbook.ExternNames.Contains(name.Name))
                  {
                    index2 = externWorkbook.ExternNames.Add(name.Name);
                    externWorkbook.ExternNames[index2].RefersTo = name.RefersTo;
                    externWorkbook.ExternNames[index2].sheetId = firstSheet != 65534 ? firstSheet : -1;
                  }
                  else
                    index2 = externWorkbook.ExternNames.GetNameIndex(name.Name);
                  token2.NameIndex = (ushort) (index2 + 1);
                }
              }
              else
                this.UpdateNameIndex((Ptg) token2, dicNameIndexes);
              token2.RefIndex = (ushort) num;
              flag = true;
              break;
            }
            if (inputSheetName != null)
            {
              int iNewRefIndex = destBook.AddSheetReference(inputSheetName);
              if (sourceBook != destBook && !dicSheetNames.ContainsKey(sheetNameByReference))
              {
                int firstSheet = (int) sourceBook.ExternSheet.RefList[(int) refIndex1].FirstSheet;
                iNewRefIndex = RowStorage.ChangeExternSheet(sourceBook, destBook, firstSheet, iNewRefIndex);
              }
              sheetReference.RefIndex = (ushort) iNewRefIndex;
              flag = true;
              break;
            }
            break;
          }
          if (sourceBook != destBook)
          {
            ExternWorkbookImpl externWorkbook = sourceBook.ExternWorkbooks[sourceBook.GetBookIndex((int) refIndex1)];
            if (externWorkbook.URL != null)
            {
              int firstSheet = (int) sourceBook.ExternSheet.RefList[(int) refIndex1].FirstSheet;
              int num = (int) sheetReference.RefIndex;
              string str = destBook.IsCreated ? destBook.GetWorkbookName(destBook) : destBook.FullFileName;
              if (str == externWorkbook.URL || sourceBook.FullFileName != null && str == sourceBook.GetFilePath(sourceBook.FullFileName) + externWorkbook.URL)
              {
                string sheetName = externWorkbook.GetSheetName(firstSheet);
                if (sheetName != null && sheetName != string.Empty)
                  num = destBook.AddSheetReference(sheetName);
                else if (firstSheet == 65534)
                {
                  int supIndex = destBook.ExternWorkbooks.InsertSelfSupbook();
                  num = destBook.ExternSheet.AddReference(supIndex, firstSheet, firstSheet);
                }
                if (token1 is NameXPtg nameXptg)
                {
                  int index3 = (int) nameXptg.NameIndex - 1;
                  int nameIndex = CellRecordCollection.GetNameIndex(externWorkbook.ExternNames[index3].Name, destBook, destBook.Worksheets[sheetName] as WorksheetImpl);
                  nameXptg.NameIndex = (ushort) (nameIndex + 1);
                }
              }
              else
              {
                int index4 = externWorkbook.Index;
                int supIndex = !destBook.ExternWorkbooks.Contains(externWorkbook) ? destBook.ExternWorkbooks.Add(externWorkbook) : destBook.ExternWorkbooks.IndexOf(externWorkbook);
                num = destBook.ExternSheet.AddReference(supIndex, firstSheet, firstSheet);
              }
              sheetReference.RefIndex = (ushort) num;
              flag = true;
              break;
            }
            if (token1 is NameXPtg)
            {
              int refIndex3 = (int) (token1 as NameXPtg).RefIndex;
              int num = dictExternSheets.ContainsKey(refIndex3) ? dictExternSheets[refIndex3] : refIndex3;
              sheetReference.RefIndex = (ushort) num;
              flag = true;
              break;
            }
            break;
          }
          break;
        case NamePtg _:
          this.UpdateNameIndex((Ptg) (token1 as NamePtg), dicNameIndexes);
          flag = true;
          break;
      }
    }
    if (flag && array != null)
    {
      this.SetArrayRecord(iColumnIndex, array, destBook.AppImplementation.RowStorageAllocationBlockSize);
      flag = false;
    }
    return flag;
  }

  internal static int ChangeExternSheet(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    int iSheetIndex,
    int iNewRefIndex)
  {
    int supBookIndex = (int) destBook.ExternSheet.RefList[iNewRefIndex].SupBookIndex;
    int index = RowStorage.GetExternBook(sourceBook, destBook, supBookIndex).Index;
    return destBook.ExternSheet.AddReference(index, iSheetIndex, iSheetIndex);
  }

  private static ExternWorkbookImpl GetExternBook(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    int bookIndex)
  {
    ExternWorkbookImpl externBook = destBook.ExternWorkbooks[bookIndex];
    if (externBook.IsInternalReference)
    {
      ExternWorkbookImpl externWorkbookImpl = (ExternWorkbookImpl) externBook.Clone((object) destBook.ExternWorkbooks);
      externWorkbookImpl.IsInternalReference = false;
      externWorkbookImpl.URL = sourceBook.FullFileName == null ? "Book1" : sourceBook.FullFileName;
      if (!destBook.ExternWorkbooks.Contains(externWorkbookImpl))
      {
        foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) sourceBook.Worksheets)
          externWorkbookImpl.AddWorksheet(worksheet.Name);
        externBook = externWorkbookImpl;
        externBook.Index = destBook.ExternWorkbooks.Add(externWorkbookImpl);
      }
      else
        externBook = destBook.ExternWorkbooks[destBook.ExternWorkbooks.IndexOf(externWorkbookImpl)];
    }
    return externBook;
  }

  private void UpdateNameIndex(Ptg token, Dictionary<int, int> dicNameIndexes)
  {
    NameXPtg nameXptg = token as NameXPtg;
    NamePtg namePtg = token as NamePtg;
    if (nameXptg != null)
    {
      int key = (int) nameXptg.NameIndex - 1;
      int num = dicNameIndexes.ContainsKey(key) ? dicNameIndexes[key] : key;
      nameXptg.NameIndex = (ushort) (num + 1);
    }
    else
    {
      if (namePtg == null)
        return;
      int key = namePtg.ExternNameIndexInt - 1;
      int num = dicNameIndexes.ContainsKey(key) ? dicNameIndexes[key] : key;
      namePtg.ExternNameIndexInt = num + 1;
    }
  }

  private bool IsSameSubType(ICellPositionFormat cell, int iOffset)
  {
    if (cell == null)
      throw new ArgumentNullException(nameof (cell));
    TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
    int index = 0;
    for (int length = RowStorage.DEF_MULTIRECORDS_SUBTYPES.Length; index < length; index += 2)
    {
      if (RowStorage.DEF_MULTIRECORDS_SUBTYPES[index] == tbiffRecord)
        return RowStorage.DEF_MULTIRECORDS_SUBTYPES[index + 1] == cell.TypeCode;
    }
    return false;
  }

  private int SplitRecord(int iOffset, int iColumnIndex, int iBlockSize)
  {
    BiffRecordRaw record1 = BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version);
    IMultiCellRecord multiCellRecord = (IMultiCellRecord) record1;
    int iPreparedSize = record1.GetStoreSize(this.Version) + 4;
    ICellPositionFormat[] cellPositionFormatArray = multiCellRecord.Split(iColumnIndex);
    int length = cellPositionFormatArray.Length;
    int num = 0;
    for (int index = 0; index < length; ++index)
    {
      BiffRecordRaw biffRecordRaw = (BiffRecordRaw) cellPositionFormatArray[index];
      if (record1 != null)
        num += record1.GetStoreSize(this.Version) + 4;
    }
    this.EnsureSize(this.m_iUsedSize + (num - iPreparedSize), iBlockSize);
    this.InsertRecordData(iOffset, iPreparedSize, 0, (BiffRecordRaw) null, iBlockSize);
    for (int index = 0; index < length; ++index)
    {
      BiffRecordRaw record2 = (BiffRecordRaw) cellPositionFormatArray[index];
      if (record2 != null)
      {
        int iRequiredSize = record2.GetStoreSize(this.Version) + 4;
        this.InsertRecordData(iOffset, 0, iRequiredSize, record2, iBlockSize);
        if (index == 0)
          iOffset += iRequiredSize;
      }
    }
    return iOffset;
  }

  private void InsertIntoRecord(int iOffset, ICellPositionFormat cell)
  {
    ((IMultiCellRecord) BiffRecordFactory.GetRecord(this.m_dataProvider, iOffset, this.Version)).Insert(cell);
  }

  private void InsertRecordData(
    int iOffset,
    int iPreparedSize,
    int iRequiredSize,
    BiffRecordRaw record,
    int iBlockSize)
  {
    int num = iRequiredSize / iBlockSize * iBlockSize + iBlockSize;
    if (this.m_dataProvider.Capacity - this.UsedSize < iRequiredSize)
      this.EnsureSize(num + this.m_iUsedSize, iBlockSize);
    if (iPreparedSize != iRequiredSize)
    {
      int iMemorySize = this.m_iUsedSize - iOffset - iPreparedSize;
      if (iMemorySize > 0)
        this.m_dataProvider.MoveMemory(iOffset + iRequiredSize, iOffset + iPreparedSize, iMemorySize);
      this.m_iUsedSize += iRequiredSize - iPreparedSize;
    }
    if (record == null)
      return;
    this.m_dataProvider.WriteInt16(iOffset, (short) record.TypeCode);
    ExcelVersion version = this.Version;
    this.m_dataProvider.WriteInt16(iOffset + 2, (short) record.GetStoreSize(version));
    record.InfillInternalData(this.m_dataProvider, iOffset + 4, version);
  }

  private void InsertRecordData(int iOffset, int iPreparedSize, IList arrRecords, int iBlockSize)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    int num1 = 0;
    int count = arrRecords.Count;
    int val2 = 0;
    for (int index = 0; index < count; ++index)
    {
      int storeSize = ((BiffRecordRaw) arrRecords[index]).GetStoreSize(this.Version);
      num1 += storeSize + 4;
      val2 = Math.Max(storeSize, val2);
    }
    int num2 = num1 - iPreparedSize;
    if (num2 > 0)
      this.EnsureSize(num2 + this.m_iUsedSize, iBlockSize);
    if (iPreparedSize != num1)
    {
      int iMemorySize = this.m_iUsedSize - iOffset - iPreparedSize;
      if (iMemorySize > 0)
        this.m_dataProvider.MoveMemory(iOffset + num1, iOffset + iPreparedSize, iMemorySize);
      this.m_iUsedSize += num1 - iPreparedSize;
    }
    if (count <= 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      BiffRecordRaw arrRecord = (BiffRecordRaw) arrRecords[index];
      this.m_dataProvider.WriteInt16(iOffset, (short) arrRecord.TypeCode);
      this.m_dataProvider.WriteInt16(iOffset + 2, (short) arrRecord.GetStoreSize(this.Version));
      arrRecord.InfillInternalData(this.m_dataProvider, iOffset, this.Version);
      iOffset += arrRecord.Length + 4;
    }
  }

  private int LocateRecord(int iColumnIndex, out bool bFound)
  {
    return this.LocateRecord(iColumnIndex, out bFound, out bool _, false);
  }

  private int LocateRecord(int iColumnIndex, out bool bFound, out bool bMul, bool bGetRkOffset)
  {
    bFound = false;
    bMul = false;
    if (iColumnIndex < this.m_iFirstColumn || this.m_iUsedSize <= 0)
      return 0;
    int iOffset;
    int iCurrentColumn;
    if (iColumnIndex > this.m_iLastColumn)
    {
      iOffset = this.m_iUsedSize;
      iCurrentColumn = int.MaxValue;
    }
    else
    {
      iOffset = 0;
      bool flag = iColumnIndex >= this.m_iCurrentColumn && this.m_iCurrentColumn >= 0;
      int iLength = -4;
      bool hasMultiRkBlank = this.HasMultiRkBlank;
      if (flag)
      {
        iOffset = this.m_iCurrentOffset;
        iCurrentColumn = this.m_iCurrentColumn;
      }
      else
        iCurrentColumn = this.m_iFirstColumn;
      do
      {
        iOffset += iLength + 4;
        if (iOffset >= this.m_iUsedSize)
        {
          iCurrentColumn = int.MaxValue;
          break;
        }
        long num1 = this.m_dataProvider.ReadInt64(iOffset);
        TBIFFRecord biffCode = (TBIFFRecord) (num1 & (long) ushort.MaxValue);
        long num2 = num1 >> 16 /*0x10*/;
        iLength = (int) (num2 & (long) ushort.MaxValue);
        if (biffCode != TBIFFRecord.String && biffCode != TBIFFRecord.Array)
        {
          long num3 = num2 >> 32 /*0x20*/;
          iCurrentColumn = this.CellPositionSize != 4 ? this.m_dataProvider.ReadInt32(iOffset + 4 + 4) : (int) (num3 & (long) ushort.MaxValue);
          if (hasMultiRkBlank && (biffCode == TBIFFRecord.MulRK || biffCode == TBIFFRecord.MulBlank) && this.GetOffsetToSubRecord(ref iOffset, iLength, iCurrentColumn, iColumnIndex, ref bMul, biffCode, bGetRkOffset))
            break;
        }
      }
      while (iCurrentColumn < iColumnIndex);
    }
    bFound = iCurrentColumn <= iColumnIndex;
    if (bFound && !bMul)
    {
      this.m_iCurrentColumn = iCurrentColumn;
      this.m_iCurrentOffset = iOffset;
    }
    return iOffset;
  }

  private bool GetOffsetToSubRecord(
    ref int iOffset,
    int iLength,
    int iCurrentColumn,
    int iColumnIndex,
    ref bool bMul,
    TBIFFRecord biffCode,
    bool bGetRkOffset)
  {
    int cellPositionSize = this.CellPositionSize;
    int iOffset1 = iOffset + 4 + iLength - cellPositionSize / 2;
    int num1 = cellPositionSize == 4 ? (int) this.m_dataProvider.ReadInt16(iOffset1) : this.m_dataProvider.ReadInt32(iOffset1);
    bool offsetToSubRecord = false;
    if (iCurrentColumn <= iColumnIndex && num1 >= iColumnIndex)
    {
      bMul = true;
      int num2 = biffCode == TBIFFRecord.MulRK ? 6 : 2;
      if (bGetRkOffset)
      {
        this.m_iCurrentColumn = iCurrentColumn;
        this.m_iCurrentOffset = iOffset;
        int num3 = iColumnIndex - iCurrentColumn;
        iOffset = iOffset + 4 + cellPositionSize + num3 * num2;
      }
      offsetToSubRecord = true;
    }
    return offsetToSubRecord;
  }

  private void EnsureSize(int iRequiredSize, int iBlockSize)
  {
    if (this.m_dataProvider == null)
      throw new NotImplementedException();
    int size = iRequiredSize / iBlockSize * iBlockSize + iBlockSize;
    if (this.m_book != null)
      this.m_dataProvider.EnsureCapacity(size, this.m_book.MaxImportColumns);
    else
      this.m_dataProvider.EnsureCapacity(size);
  }

  private void AccessColumn(int iColumnIndex)
  {
    if (iColumnIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Column index cannot be less than 0");
    this.m_iFirstColumn = this.m_iFirstColumn >= 0 ? Math.Min(this.m_iFirstColumn, iColumnIndex) : iColumnIndex;
    this.m_iLastColumn = Math.Max(this.m_iLastColumn, iColumnIndex);
  }

  private void AccessColumn(int iColumnIndex, ICellPositionFormat cell)
  {
    if (iColumnIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Column index cannot be less than 0");
    if (cell != null)
    {
      this.m_iFirstColumn = this.m_iFirstColumn >= 0 ? Math.Min(this.m_iFirstColumn, iColumnIndex) : iColumnIndex;
      this.m_iLastColumn = Math.Max(this.m_iLastColumn, iColumnIndex);
    }
    else if (iColumnIndex == this.m_iFirstColumn)
    {
      if (iColumnIndex == this.m_iLastColumn)
      {
        this.m_iLastColumn = this.m_iFirstColumn = -1;
        this.m_iUsedSize = 0;
      }
      else
        this.m_iFirstColumn = this.GetColumn(0);
    }
    else
    {
      if (iColumnIndex != this.m_iLastColumn)
        return;
      int num = -1;
      int iLength;
      for (int index = 0; index < this.m_iUsedSize; index += 4 + iLength)
      {
        TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(index);
        iLength = (int) this.m_dataProvider.ReadInt16(index + 2);
        switch (tbiffRecord)
        {
          case TBIFFRecord.MulRK:
          case TBIFFRecord.MulBlank:
            num = this.GetLastColumn(index, iLength);
            continue;
          case TBIFFRecord.String:
          case TBIFFRecord.Array:
            continue;
          default:
            num = this.GetColumn(index);
            continue;
        }
      }
      if (num >= 0)
      {
        this.m_iLastColumn = num;
      }
      else
      {
        this.m_iFirstColumn = -1;
        this.m_iLastColumn = -1;
        this.m_iUsedSize = 0;
      }
    }
  }

  private int RemoveFormulaStringValue(int iColumnIndex)
  {
    return this.RemoveFormulaStringValue(iColumnIndex, out int _);
  }

  private int RemoveFormulaStringValue(int iColumnIndex, out int iFormulaRecordOffset)
  {
    iFormulaRecordOffset = -1;
    if (iColumnIndex < this.m_iFirstColumn || iColumnIndex > this.m_iLastColumn)
      return -1;
    bool bFound;
    int iOffset1 = this.LocateRecord(iColumnIndex, out bFound);
    if (!bFound || this.m_dataProvider.ReadInt16(iOffset1) != (short) 6)
      return -1;
    iFormulaRecordOffset = iOffset1;
    int iOffset2 = this.MoveNext(iOffset1);
    if (iOffset2 >= this.m_iUsedSize)
      return iOffset2;
    TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset2);
    if (tbiffRecord == TBIFFRecord.Array)
    {
      iOffset2 = this.MoveNext(iOffset2);
      if (iOffset2 >= this.m_iUsedSize)
        return iOffset2;
      tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset2);
    }
    if (tbiffRecord == TBIFFRecord.String)
      this.RemoveRecord(iOffset2);
    return iOffset2;
  }

  private void RemoveRecord(int iOffset)
  {
    if (iOffset < this.m_iCurrentOffset)
    {
      this.m_iCurrentOffset = -1;
      this.m_iCurrentColumn = -1;
    }
    int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2) + 4;
    int iMemorySize = this.m_iUsedSize - iOffset - num;
    if (iMemorySize > 0)
      this.m_dataProvider.MoveMemory(iOffset, iOffset + num, iMemorySize);
    this.m_iUsedSize -= num;
  }

  private int MoveNext(int iOffset)
  {
    int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
    return iOffset + 4 + num;
  }

  private Point GetOffsets(
    int iStartColumn,
    int iEndColumn,
    out int iRealStartColumn,
    out int iRealEndColumn)
  {
    iRealStartColumn = -1;
    iRealEndColumn = -1;
    if (this.m_iFirstColumn < 0)
      return Point.Empty;
    iStartColumn = Math.Max(this.m_iFirstColumn, iStartColumn);
    iEndColumn = Math.Min(this.m_iLastColumn, iEndColumn);
    if (iStartColumn > iEndColumn)
      return Point.Empty;
    if (iStartColumn == this.m_iFirstColumn && iEndColumn == this.m_iLastColumn)
    {
      iRealStartColumn = iStartColumn;
      iRealEndColumn = iEndColumn;
      return new Point(0, this.m_iUsedSize);
    }
    int num1 = this.LocateRecord(iStartColumn, out bool _);
    if (num1 >= this.m_iUsedSize)
      return Point.Empty;
    iStartColumn = this.GetColumn(num1);
    iRealStartColumn = iStartColumn;
    iRealEndColumn = iStartColumn;
    int num2 = num1;
    if (iRealEndColumn == iEndColumn)
    {
      num2 = this.MoveAfterRecord(num2);
    }
    else
    {
      while (iRealEndColumn < iEndColumn && num2 < this.m_iUsedSize)
      {
        TBIFFRecord tbiffRecord1 = (TBIFFRecord) this.m_dataProvider.ReadInt16(num2);
        int num3 = (int) this.m_dataProvider.ReadInt16(num2 + 2);
        int column = this.GetColumn(num2);
        if (column <= iEndColumn)
        {
          num2 += 4 + num3;
          bool flag = tbiffRecord1 == TBIFFRecord.MulRK || tbiffRecord1 == TBIFFRecord.MulBlank;
          iRealEndColumn = flag ? (int) this.m_dataProvider.ReadInt16(num2 - 2) : column;
          if (num2 < this.m_iUsedSize)
          {
            TBIFFRecord tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(num2);
            if (tbiffRecord2 == TBIFFRecord.Array)
            {
              num2 = this.MoveNext(num2);
              if (num2 < this.m_iUsedSize)
                tbiffRecord2 = (TBIFFRecord) this.m_dataProvider.ReadInt16(num2);
            }
            if (tbiffRecord2 == TBIFFRecord.String)
              num2 = this.MoveNext(num2);
          }
        }
        else
          break;
      }
    }
    return new Point(num1, num2);
  }

  private int MoveAfterRecord(int iOffset)
  {
    if (iOffset < this.m_iUsedSize)
    {
      iOffset = this.MoveNext(iOffset);
      if (iOffset < this.m_iUsedSize)
      {
        TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
        if (tbiffRecord == TBIFFRecord.Array)
        {
          iOffset = this.MoveNext(iOffset);
          if (iOffset < this.m_iUsedSize)
            tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
        }
        if (tbiffRecord == TBIFFRecord.String)
          iOffset = this.MoveNext(iOffset);
      }
    }
    return iOffset;
  }

  private IMultiCellRecord CreateMultiRecord(TBIFFRecord subCode)
  {
    return (IMultiCellRecord) BiffRecordFactory.GetRecord(subCode == TBIFFRecord.RK ? TBIFFRecord.MulRK : TBIFFRecord.MulBlank);
  }

  private ICellPositionFormat GetNextColumnRecord(
    int iColumnIndex,
    ICellPositionFormat prevRecord,
    ref int iOffset,
    bool bMulti)
  {
    int num = iColumnIndex;
    ICellPositionFormat nextColumnRecord = (ICellPositionFormat) null;
    if (bMulti)
      num = this.GetLastColumnFromMultiRecord(iOffset);
    if (num >= iColumnIndex + 1)
    {
      nextColumnRecord = prevRecord;
    }
    else
    {
      if (prevRecord != null)
        iOffset += 4 + (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      if (iOffset < this.m_iUsedSize && this.GetColumn(iOffset) == iColumnIndex + 1)
        nextColumnRecord = (ICellPositionFormat) this.GetRecordAtOffset(iOffset);
    }
    return nextColumnRecord;
  }

  private int GetLastColumnFromMultiRecord(int iOffset)
  {
    int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
    return (int) this.m_dataProvider.ReadInt16(iOffset + 4 + num - 2);
  }

  private void SetOrdinaryRecord(int iColumnIndex, ICellPositionFormat cell, int iBlockSize)
  {
    if (cell != null && !this.HasRkBlank)
    {
      switch (cell.TypeCode)
      {
        case TBIFFRecord.Blank:
        case TBIFFRecord.RK:
          this.HasRkBlank = true;
          break;
      }
    }
    bool bFound;
    int iOffset1 = this.LocateRecord(iColumnIndex, out bFound);
    int iPreparedSize = 0;
    if (bFound)
    {
      iPreparedSize = (int) this.m_dataProvider.ReadInt16(iOffset1 + 2) + 4;
      int iOffset2 = iOffset1 + iPreparedSize;
      if (iOffset2 < this.m_iUsedSize)
      {
        TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset2);
        if (tbiffRecord == TBIFFRecord.Array)
        {
          int num = (int) this.m_dataProvider.ReadInt16(iOffset2 + 2) + 4;
          iOffset2 += num;
          iPreparedSize += num;
          if (iOffset2 < this.m_iUsedSize)
            tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset2);
        }
        if (tbiffRecord == TBIFFRecord.String && (cell == null || cell.TypeCode != TBIFFRecord.Formula))
        {
          int num = (int) this.m_dataProvider.ReadInt16(iOffset2 + 2);
          iPreparedSize += num + 4;
        }
      }
    }
    else
    {
      this.m_iCurrentOffset = -1;
      this.m_iCurrentColumn = -1;
    }
    BiffRecordRaw record = (BiffRecordRaw) cell;
    int iRequiredSize = record != null ? record.GetStoreSize(this.Version) + 4 : 0;
    this.InsertRecordData(iOffset1, iPreparedSize, iRequiredSize, record, iBlockSize);
    this.AccessColumn(iColumnIndex, cell);
  }

  private int DefragmentDataStorage(
    RowStorage.DefragmentHelper rkRecordHelper,
    RowStorage.DefragmentHelper blankRecordHelper,
    RowStorage.DefragmentHelper ordinaryHelper,
    object userData)
  {
    int iOffset = 0;
    while (iOffset < this.m_iUsedSize)
    {
      TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
      int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
      switch (tbiffRecord)
      {
        case TBIFFRecord.Blank:
          iOffset = blankRecordHelper(userData);
          continue;
        case TBIFFRecord.RK:
          iOffset = rkRecordHelper(userData);
          continue;
        default:
          iOffset = ordinaryHelper(userData);
          continue;
      }
    }
    return iOffset;
  }

  private int SkipRecord(object userData)
  {
    RowStorage.OffsetData offsetData = (RowStorage.OffsetData) userData;
    int startOffset = offsetData.StartOffset;
    int num1 = (int) this.m_dataProvider.ReadInt16(startOffset + 2) + 4;
    offsetData.UsedSize += num1;
    int num2 = startOffset + num1;
    offsetData.StartOffset = num2;
    return num2;
  }

  private int SkipRKRecords(object userData)
  {
    RowStorage.OffsetData offsetData = (RowStorage.OffsetData) userData;
    int startOffset = offsetData.StartOffset;
    int column1 = this.GetColumn(startOffset);
    int num1 = column1;
    int num2 = 4 + (int) this.m_dataProvider.ReadInt16(startOffset + 2);
    int num3 = startOffset + num2;
    int usedSize = offsetData.UsedSize;
    while (num3 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(num3) == (short) 638)
    {
      int column2 = this.GetColumn(num3);
      if (num1 + 1 == column2)
      {
        num3 += 4 + (int) this.m_dataProvider.ReadInt16(num3 + 2);
        num1 = column2;
      }
      else
        break;
    }
    int num4 = num1 - column1 + 1;
    int num5 = num4 <= 1 ? usedSize + num2 : usedSize + (6 * num4 + 6 + 4);
    offsetData.UsedSize = num5;
    offsetData.StartOffset = num3;
    return num3;
  }

  private int SkipBlankRecords(object userData)
  {
    RowStorage.OffsetData offsetData = (RowStorage.OffsetData) userData;
    int startOffset = offsetData.StartOffset;
    int column1 = this.GetColumn(startOffset);
    int num1 = column1;
    int num2 = 4 + (int) this.m_dataProvider.ReadInt16(startOffset + 2);
    int num3 = startOffset + num2;
    int usedSize = offsetData.UsedSize;
    for (; num3 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(num3) == (short) 513; num3 += 4 + (int) this.m_dataProvider.ReadInt16(num3 + 2))
    {
      int column2 = this.GetColumn(num3);
      if (num1 + 1 == column2)
        num1 = column2;
      else
        break;
    }
    int num4 = num1 - column1 + 1;
    int num5 = num4 <= 1 ? usedSize + num2 : usedSize + (2 * num4 + 6 + 4);
    offsetData.UsedSize = num5;
    offsetData.StartOffset = num3;
    return num3;
  }

  private int CompressRecord(object userData)
  {
    RowStorage.WriteData writeData = (RowStorage.WriteData) userData;
    int offset = writeData.Offset;
    int iMemorySize = (int) this.m_dataProvider.ReadInt16(offset + 2) + 4;
    this.m_dataProvider.MoveMemory(writeData.UsedSize, offset, iMemorySize);
    writeData.UsedSize += iMemorySize;
    int num = offset + iMemorySize;
    writeData.Offset = num;
    return num;
  }

  private int CompressRKRecords(object userData)
  {
    RowStorage.WriteData writeData = (RowStorage.WriteData) userData;
    int offset = writeData.Offset;
    int usedSize = writeData.UsedSize;
    int column1 = this.GetColumn(offset);
    int num1 = column1;
    int num2 = 4 + (int) this.m_dataProvider.ReadInt16(offset + 2);
    int num3 = offset + num2;
    while (num3 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(num3) == (short) 638)
    {
      int column2 = this.GetColumn(num3);
      if (num1 + 1 == column2)
      {
        num3 += 4 + (int) this.m_dataProvider.ReadInt16(num3 + 2);
        num1 = column2;
      }
      else
        break;
    }
    int iRecordsCount = num1 - column1 + 1;
    if (iRecordsCount > 1)
    {
      this.CreateMulRKRecord(writeData, iRecordsCount);
      this.HasMultiRkBlank = true;
    }
    else
      num3 = this.CompressRecord(userData);
    return num3;
  }

  private int CompressBlankRecords(object userData)
  {
    RowStorage.WriteData writeData = (RowStorage.WriteData) userData;
    int offset = writeData.Offset;
    int usedSize = writeData.UsedSize;
    int column1 = this.GetColumn(offset);
    int num1 = column1;
    int num2 = 4 + (int) this.m_dataProvider.ReadInt16(offset + 2);
    int num3 = offset + num2;
    while (num3 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(num3) == (short) 513)
    {
      int column2 = this.GetColumn(num3);
      if (num1 + 1 == column2)
      {
        num3 += 4 + (int) this.m_dataProvider.ReadInt16(num3 + 2);
        num1 = column2;
      }
      else
        break;
    }
    int iRecordsCount = num1 - column1 + 1;
    if (iRecordsCount > 1)
    {
      int num4 = 2 * iRecordsCount + 6;
      BiffRecordRaw mulBlankRecord = (BiffRecordRaw) this.CreateMulBlankRecord(writeData.Offset, iRecordsCount);
      this.m_dataProvider.WriteInt16(usedSize, (short) mulBlankRecord.RecordCode);
      int iOffset1 = usedSize + 2;
      this.m_dataProvider.WriteInt16(iOffset1, (short) num4);
      int iOffset2 = iOffset1 + 2;
      mulBlankRecord.InfillInternalData(this.m_dataProvider, iOffset2, this.Version);
      int num5 = iOffset2 + num4;
      int num6 = num4 + 4;
      writeData.UsedSize += num6;
      writeData.Offset = num3;
      this.HasMultiRkBlank = true;
    }
    else
      num3 = this.CompressRecord(userData);
    return num3;
  }

  private MulRKRecord CreateMulRKRecord(RowStorage.WriteData writeData, int iRecordsCount)
  {
    if (this.Version != ExcelVersion.Excel97to2003)
      throw new NotSupportedException("This method is supported only for Excel97-2003 file format");
    int offset1 = writeData.Offset;
    int usedSize = writeData.UsedSize;
    if (offset1 < this.m_iCurrentOffset)
    {
      this.m_iCurrentOffset = -1;
      this.m_iCurrentColumn = -1;
    }
    this.m_dataProvider.WriteInt16(usedSize, (short) 189);
    int num1 = offset1 + 2;
    int iOffset1 = usedSize + 2;
    int num2 = 6 + 6 * iRecordsCount;
    this.m_dataProvider.WriteInt16(iOffset1, (short) num2);
    int iOffset2 = num1 + 2;
    int offset2 = iOffset1 + 2;
    byte[] arrDest = new byte[6];
    this.m_dataProvider.ReadArray(iOffset2, arrDest, 4);
    short num3 = (short) ((int) this.m_dataProvider.ReadInt16(iOffset2 + 2) + iRecordsCount - 1);
    this.m_dataProvider.WriteBytes(offset2, arrDest, 0, 4);
    int num4 = offset2 + 4;
    int iOffset3 = writeData.Offset + 4 + 4;
    for (int index = 0; index < iRecordsCount; ++index)
    {
      this.m_dataProvider.ReadArray(iOffset3, arrDest);
      this.m_dataProvider.WriteBytes(num4, arrDest, 0, 6);
      iOffset3 += 14;
      num4 += 6;
    }
    this.m_dataProvider.WriteInt16(num4, num3);
    int num5 = num4 + 2;
    writeData.UsedSize = num5;
    writeData.Offset = iOffset3 - 4 - 4;
    return (MulRKRecord) null;
  }

  private MulRKRecord CreateMulRKRecord(int iOffset, int iRecordsCount)
  {
    if (iOffset < this.m_iCurrentOffset)
    {
      this.m_iCurrentOffset = -1;
      this.m_iCurrentColumn = -1;
    }
    MulRKRecord record = (MulRKRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulRK);
    record.Row = this.GetRow(iOffset);
    record.FirstColumn = this.GetColumn(iOffset);
    record.LastColumn = record.FirstColumn + iRecordsCount - 1;
    List<MulRKRecord.RkRec> rkRecList = new List<MulRKRecord.RkRec>(iRecordsCount);
    for (int index = 0; index < iRecordsCount; ++index)
    {
      MulRKRecord.RkRec rkRec = new MulRKRecord.RkRec(this.GetXFIndex(iOffset, false), this.m_dataProvider.ReadInt32(iOffset + 10));
      rkRecList.Add(rkRec);
      iOffset += 14;
    }
    record.Records = rkRecList;
    return record;
  }

  private MulBlankRecord CreateMulBlankRecord(int iOffset, int iRecordsCount)
  {
    if (iOffset < this.m_iCurrentOffset)
    {
      this.m_iCurrentOffset = -1;
      this.m_iCurrentColumn = -1;
    }
    MulBlankRecord record = (MulBlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulBlank);
    record.Row = this.GetRow(iOffset);
    record.FirstColumn = this.GetColumn(iOffset);
    record.LastColumn = record.FirstColumn + iRecordsCount - 1;
    List<ushort> ushortList = new List<ushort>(iRecordsCount);
    for (int index = 0; index < iRecordsCount; ++index)
    {
      ushort xfIndex = this.GetXFIndex(iOffset, false);
      ushortList.Add(xfIndex);
      iOffset += 10;
    }
    record.ExtendedFormatIndexes = ushortList;
    return record;
  }

  private List<int> GetMultiRecordsOffsets(
    MulBlankRecord mulBlank,
    MulRKRecord mulRK,
    out int iSizeDelta)
  {
    iSizeDelta = 0;
    int iOffset = 0;
    List<int> multiRecordsOffsets = new List<int>();
    int iLength;
    for (; iOffset < this.m_iUsedSize; iOffset += iLength)
    {
      IMultiCellRecord multiCellRecord = this.CreateMultiCellRecord(iOffset, mulBlank, mulRK, out iLength);
      if (multiCellRecord != null)
      {
        if (multiCellRecord.LastColumn != multiCellRecord.FirstColumn)
          iSizeDelta += (multiCellRecord.LastColumn - multiCellRecord.FirstColumn + 1) * multiCellRecord.GetSeparateSubRecordSize(this.Version) - iLength;
        multiRecordsOffsets.Add(iOffset);
      }
    }
    return multiRecordsOffsets;
  }

  private void DecompressStorage(
    List<int> arrOffsets,
    int iSizeDelta,
    MulBlankRecord mulBlank,
    MulRKRecord mulRK,
    bool bIgnoreStyles)
  {
    this.m_iUsedSize += iSizeDelta;
    int num1 = this.m_iUsedSize;
    for (int index = arrOffsets.Count - 1; index >= 0; --index)
    {
      int arrOffset = arrOffsets[index];
      int iLength;
      IMultiCellRecord multiCellRecord = this.CreateMultiCellRecord(arrOffset, mulBlank, mulRK, out iLength);
      int iSourceOffset = arrOffset + iLength;
      int iMemorySize = num1 - iSizeDelta - iSourceOffset;
      if (iMemorySize > 0)
      {
        if (arrOffsets.Count == 1 || multiCellRecord.FirstColumn != multiCellRecord.LastColumn)
          this.m_dataProvider.MoveMemory(iSourceOffset + iSizeDelta, iSourceOffset, iMemorySize);
        else if (this.UsedSize - iSourceOffset + 2 <= this.m_dataProvider.Capacity)
        {
          this.m_dataProvider.MoveMemory(iSourceOffset + iSizeDelta - 2, iSourceOffset, this.UsedSize - iSourceOffset + 2);
          this.m_iUsedSize -= 2;
        }
      }
      int num2 = (multiCellRecord.LastColumn - multiCellRecord.FirstColumn + 1) * multiCellRecord.GetSeparateSubRecordSize(this.Version);
      BiffRecordRaw[] records = multiCellRecord.Split(bIgnoreStyles);
      this.InsertRecordData(arrOffset, records);
      num1 = iSourceOffset + iSizeDelta;
      iSizeDelta -= num2 - iLength;
    }
  }

  private IMultiCellRecord CreateMultiCellRecord(
    int iOffset,
    MulBlankRecord mulBlank,
    MulRKRecord mulRK,
    out int iLength)
  {
    IMultiCellRecord multiCellRecord = (IMultiCellRecord) null;
    TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
    int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
    iLength = num + 4;
    switch (tbiffRecord)
    {
      case TBIFFRecord.MulRK:
        mulRK.Length = num;
        mulRK.ParseStructure(this.m_dataProvider, iOffset + 4, 0, this.Version);
        multiCellRecord = (IMultiCellRecord) mulRK;
        break;
      case TBIFFRecord.MulBlank:
        mulBlank.Length = num;
        mulBlank.ParseStructure(this.m_dataProvider, iOffset + 4, 0, this.Version);
        multiCellRecord = (IMultiCellRecord) mulBlank;
        break;
    }
    return multiCellRecord;
  }

  private void DecompressRecord(int iOffset, IMultiCellRecord multi, bool bIgnoreStyles)
  {
    BiffRecordRaw[] records = multi.Split(bIgnoreStyles);
    this.InsertRecordData(iOffset, records);
  }

  public int GetRow(int recordStart)
  {
    switch (this.Version)
    {
      case ExcelVersion.Excel97to2003:
        return (int) this.m_dataProvider.ReadUInt16(recordStart + 4);
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        return this.m_dataProvider.ReadInt32(recordStart + 4);
      default:
        throw new NotImplementedException();
    }
  }

  private void SetRow(int recordStart, int rowIndex)
  {
    switch (this.Version)
    {
      case ExcelVersion.Excel97to2003:
        this.m_dataProvider.WriteUInt16(recordStart + 4, (ushort) rowIndex);
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        this.m_dataProvider.WriteInt32(recordStart + 4, rowIndex);
        break;
      default:
        throw new NotImplementedException();
    }
  }

  public int GetColumn(int recordStart)
  {
    switch (this.Version)
    {
      case ExcelVersion.Excel97to2003:
        return (int) this.m_dataProvider.ReadUInt16(recordStart + 4 + 2);
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        return this.m_dataProvider.ReadInt32(recordStart + 4 + 4);
      default:
        throw new NotImplementedException();
    }
  }

  private void SetColumn(int recordStart, int columnIndex)
  {
    switch (this.Version)
    {
      case ExcelVersion.Excel97to2003:
        this.m_dataProvider.WriteUInt16(recordStart + 4 + 2, (ushort) columnIndex);
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        this.m_dataProvider.WriteInt32(recordStart + 4 + 4, columnIndex);
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private int GetLastColumn(int recordStart, int iLength)
  {
    switch (this.Version)
    {
      case ExcelVersion.Excel97to2003:
        return (int) this.m_dataProvider.ReadInt16(recordStart + 4 + iLength - 2);
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        return this.m_dataProvider.ReadInt32(recordStart + 4 + iLength - 4);
      default:
        throw new NotImplementedException();
    }
  }

  [CLSCompliant(false)]
  public ushort GetXFIndex(int recordStart, bool bMulti)
  {
    if (!bMulti)
    {
      recordStart += 8;
      if (this.Version != ExcelVersion.Excel97to2003)
        recordStart += 4;
    }
    return this.m_dataProvider.ReadUInt16(recordStart);
  }

  private void SetXFIndex(int recordStart, ushort xfIndex)
  {
    int iOffset = recordStart + 4 + 4;
    if (this.Version != ExcelVersion.Excel97to2003)
      iOffset += 4;
    this.m_dataProvider.WriteUInt16(iOffset, xfIndex);
  }

  private void SetXFIndexMulti(
    int recordStart,
    ushort xfIndex,
    int iColumnIndex,
    int subRecordSize)
  {
    int column = this.GetColumn(recordStart);
    recordStart += 8;
    if (this.Version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    recordStart += subRecordSize * (iColumnIndex - column);
    this.m_dataProvider.WriteUInt16(recordStart, xfIndex);
  }

  internal void UpdateFormulaFlags()
  {
    for (int iOffset = 0; iOffset < this.m_iUsedSize; iOffset = this.MoveNext(iOffset))
    {
      if (this.m_dataProvider.ReadInt16(iOffset) == (short) 6)
        FormulaRecord.UpdateOptions(this.m_dataProvider, iOffset);
    }
  }

  public TBIFFRecord TypeCode => ~TBIFFRecord.Unknown;

  public int RecordCode => -1;

  public bool NeedDataArray => true;

  public long StreamPos
  {
    get => -1;
    set
    {
    }
  }

  public int GetStoreSize(ExcelVersion version)
  {
    this.Compress();
    return Math.Max(0, this.m_iUsedSize - 4);
  }

  public int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ByteArrayDataProvider provider1 = (ByteArrayDataProvider) provider;
    byte[] internalBuffer = provider1.InternalBuffer;
    if (encryptor != null)
    {
      int iOffset1 = 0;
      int num1 = 0;
      int length = internalBuffer.Length;
      int num2;
      int iSourceOffset;
      for (; iOffset1 < this.m_iUsedSize; iOffset1 = iSourceOffset + num2)
      {
        short num3 = this.m_dataProvider.ReadInt16(iOffset1);
        num2 = (int) this.m_dataProvider.ReadUInt16(iOffset1 + 2);
        streamPosition += 4;
        iSourceOffset = iOffset1 + 4;
        if (length < num1 + num2 + 4)
        {
          provider1.WriteInto(writer, 0, num1, internalBuffer);
          num1 = 0;
        }
        provider1.WriteInt16(num1, num3);
        int iOffset2 = num1 + 2;
        provider1.WriteInt16(iOffset2, (short) num2);
        int num4 = iOffset2 + 2;
        this.m_dataProvider.CopyTo(iSourceOffset, internalBuffer, num4, num2);
        encryptor.Encrypt((DataProvider) provider1, num4, num2, (long) streamPosition);
        num1 = num4 + num2;
        streamPosition += num2;
      }
      if (num1 != 0)
        provider1.WriteInto(writer, 0, num1, internalBuffer);
    }
    else
      this.m_dataProvider.WriteInto(writer, 0, this.m_iUsedSize, internalBuffer);
    return this.m_iUsedSize;
  }

  public int GetBoolValue(int iCol)
  {
    bool bFound;
    int num = this.LocateRecord(iCol, out bFound);
    if (bFound && this.m_dataProvider.ReadInt16(num) == (short) 517)
    {
      int boolValue = BoolErrRecord.ReadValue(this.m_dataProvider, num, this.Version);
      if ((boolValue & 65280) == 0)
        return boolValue;
    }
    return 0;
  }

  public int GetFormulaBoolValue(int iCol)
  {
    bool bFound;
    int num1 = this.LocateRecord(iCol, out bFound);
    if (bFound && this.m_dataProvider.ReadInt16(num1) == (short) 6)
    {
      ulong num2 = (ulong) FormulaRecord.ReadInt64Value(this.m_dataProvider, num1, this.Version);
      if (((long) num2 & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710655L /*0xFFFF000000000001*/)
        return (int) ((long) num2 & 16711680L /*0xFF0000*/);
    }
    return 0;
  }

  public string GetErrorValue(int iCol)
  {
    bool bFound;
    int num1 = this.LocateRecord(iCol, out bFound);
    if (bFound && this.m_dataProvider.ReadInt16(num1) == (short) 517)
    {
      int num2 = BoolErrRecord.ReadValue(this.m_dataProvider, num1, this.Version);
      if ((num2 & 65280) != 0)
        return this.GetErrorString(num2 & (int) byte.MaxValue);
    }
    return (string) null;
  }

  public string GetFormulaErrorValue(int iCol)
  {
    bool bFound;
    int num1 = this.LocateRecord(iCol, out bFound);
    if (bFound && this.m_dataProvider.ReadInt16(num1) == (short) 6)
    {
      ulong num2 = (ulong) FormulaRecord.ReadInt64Value(this.m_dataProvider, num1, this.Version);
      if (((long) num2 & -281474976710401L /*0xFFFF0000000000FF*/) == -281474976710654L /*0xFFFF000000000002*/)
        return this.GetErrorString((int) ((num2 & 16711680UL /*0xFF0000*/) >> 16 /*0x10*/));
    }
    return (string) null;
  }

  public double GetNumberValue(int iCol, int sheetIndex)
  {
    bool bFound;
    bool bMul;
    int num = this.LocateRecord(iCol, out bFound, out bMul, true);
    if (bFound)
    {
      if (bMul)
        return RKRecord.EncodeRK(this.m_dataProvider.ReadInt32(num + 2));
      switch (this.m_dataProvider.ReadInt16(num))
      {
        case 253:
          string text = this.m_book.InnerSST[LabelSSTRecord.GetSSTIndex(this.m_dataProvider, num, this.Version)].Text;
          if (this.m_book.ActiveSheet != null)
          {
            string numberFormat = this.m_book.Worksheets[sheetIndex][this.m_row, iCol + 1].NumberFormat;
            RangeImpl rangeImpl = this.m_book.Worksheets[sheetIndex][this.m_row, iCol + 1] as RangeImpl;
            if (this.CheckFormat(numberFormat))
            {
              int startIndex = numberFormat.IndexOf(';');
              if (startIndex != -1)
                numberFormat.Remove(startIndex, numberFormat.Length - startIndex);
              DateTime result;
              if (!DateTime.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out result) && !DateTime.TryParseExact(text, CultureInfo.CurrentCulture.NumberFormat.ToString(), (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return double.NaN;
              if (DateTime.TryParse(text, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
              {
                if (rangeImpl != null)
                {
                  DateTime dtValue;
                  if (rangeImpl.TryGetDateTimeByCulture(text, text.Contains("."), out dtValue))
                    result = dtValue;
                }
                try
                {
                  return result.ToOADate();
                }
                catch (Exception ex)
                {
                  return double.NaN;
                }
              }
              else
              {
                DateTime dtValue;
                if (rangeImpl != null && rangeImpl.TryGetDateTimeByCulture(text, true, out dtValue))
                {
                  result = dtValue;
                  return result.ToOADate();
                }
                break;
              }
            }
            else
              break;
          }
          else
            break;
        case 515:
          return NumberRecord.ReadValue(this.m_dataProvider, num, this.Version);
        case 638:
          return RKRecord.EncodeRK(RKRecord.ReadValue(this.m_dataProvider, num, this.Version));
      }
    }
    return double.NaN;
  }

  private bool CheckFormat(string format)
  {
    int num = 0;
    foreach (string dateFormat in this.dateFormats)
    {
      if (format.Contains(dateFormat))
        ++num;
    }
    return num > 1;
  }

  public double GetFormulaNumberValue(int iCol)
  {
    bool bFound;
    int num = this.LocateRecord(iCol, out bFound);
    return bFound && this.m_dataProvider.ReadInt16(num) == (short) 6 ? FormulaRecord.ReadDoubleValue(this.m_dataProvider, num, this.Version) : double.NaN;
  }

  public string GetStringValue(int iColumn, SSTDictionary sst)
  {
    bool bFound;
    int iOffset = this.LocateRecord(iColumn, out bFound);
    if (!bFound)
      return (string) null;
    switch (this.m_dataProvider.ReadInt16(iOffset))
    {
      case 214:
      case 516:
        int num = 10;
        if (this.Version != ExcelVersion.Excel97to2003)
          num += 4;
        return this.m_dataProvider.ReadString16Bit(iOffset + num, out int _);
      case 253:
        int sstIndex = LabelSSTRecord.GetSSTIndex(this.m_dataProvider, iOffset, this.Version);
        return !sst[sstIndex].IsEncoded ? (string) sst[sstIndex] : string.Empty;
      default:
        return (string) null;
    }
  }

  public string GetFormulaStringValue(int iColumnIndex)
  {
    bool bFound;
    int iOffset = this.LocateRecord(iColumnIndex, out bFound);
    return !bFound ? (string) null : this.GetFormulaStringValueByOffset(iOffset);
  }

  public string GetFormulaStringValueByOffset(int iOffset)
  {
    if (this.m_dataProvider.ReadInt16(iOffset) != (short) 6)
      return (string) null;
    int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
    iOffset += 4 + num;
    if (iOffset >= this.m_iUsedSize)
      return (string) null;
    TBIFFRecord tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
    if (tbiffRecord == TBIFFRecord.Array)
    {
      iOffset = this.MoveNext(iOffset);
      tbiffRecord = (TBIFFRecord) this.m_dataProvider.ReadInt16(iOffset);
    }
    if (tbiffRecord != TBIFFRecord.String)
      return (string) null;
    iOffset += 4;
    int iStrLen = (int) this.m_dataProvider.ReadInt16(iOffset);
    return this.m_dataProvider.ReadString(iOffset + 2, iStrLen, out int _, false);
  }

  public Ptg[] GetFormulaValue(int iCol)
  {
    bool bFound;
    int num = this.LocateRecord(iCol, out bFound);
    return bFound && this.m_dataProvider.ReadInt16(num) == (short) 6 ? FormulaRecord.ReadValue(this.m_dataProvider, num, this.Version) : (Ptg[]) null;
  }

  public WorksheetImpl.TRangeValueType GetCellType(int iCol, bool bNeedFormulaSubType)
  {
    bool bFound;
    int num = this.LocateRecord(iCol, out bFound);
    if (!bFound)
      return WorksheetImpl.TRangeValueType.Blank;
    switch ((TBIFFRecord) this.m_dataProvider.ReadInt16(num))
    {
      case TBIFFRecord.Formula:
        return bNeedFormulaSubType ? this.GetSubFormulaType(num) : WorksheetImpl.TRangeValueType.Formula;
      case TBIFFRecord.MulRK:
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        return WorksheetImpl.TRangeValueType.Number;
      case TBIFFRecord.RString:
      case TBIFFRecord.LabelSST:
      case TBIFFRecord.Label:
        return WorksheetImpl.TRangeValueType.String;
      case TBIFFRecord.BoolErr:
        return (BoolErrRecord.ReadValue(this.m_dataProvider, num, this.Version) & 65280) == 0 ? WorksheetImpl.TRangeValueType.Boolean : WorksheetImpl.TRangeValueType.Error;
      default:
        return WorksheetImpl.TRangeValueType.Blank;
    }
  }

  private WorksheetImpl.TRangeValueType GetSubFormulaType(int iOffset)
  {
    WorksheetImpl.TRangeValueType trangeValueType = WorksheetImpl.TRangeValueType.Formula;
    WorksheetImpl.TRangeValueType subFormulaType;
    switch ((ulong) FormulaRecord.ReadInt64Value(this.m_dataProvider, iOffset, this.Version) & 18446462598732841215UL /*0xFFFF0000000000FF*/)
    {
      case 18446462598732840961 /*0xFFFF000000000001*/:
        subFormulaType = trangeValueType | WorksheetImpl.TRangeValueType.Boolean;
        break;
      case 18446462598732840962 /*0xFFFF000000000002*/:
        subFormulaType = trangeValueType | WorksheetImpl.TRangeValueType.Error;
        break;
      case 18446462598732840963 /*0xFFFF000000000003*/:
        subFormulaType = trangeValueType;
        break;
      default:
        int num = (int) this.m_dataProvider.ReadInt16(iOffset + 2);
        int iOffset1 = iOffset + 4 + num;
        if (this.m_dataProvider.ReadInt16(iOffset1) == (short) 545)
          iOffset1 = this.MoveNext(iOffset1);
        bool flag = iOffset1 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(iOffset1) == (short) 519;
        subFormulaType = trangeValueType | (flag ? WorksheetImpl.TRangeValueType.String : WorksheetImpl.TRangeValueType.Number);
        break;
    }
    return subFormulaType;
  }

  public bool HasFormulaRecord(int iColumn)
  {
    bool bFound;
    int iOffset = this.LocateRecord(iColumn, out bFound);
    return bFound && this.m_dataProvider.ReadInt16(iOffset) == (short) 6;
  }

  public bool HasFormulaArrayRecord(int iCol)
  {
    bool bFound;
    int iOffset1 = this.LocateRecord(iCol, out bFound);
    if (bFound && this.m_dataProvider.ReadInt16(iOffset1) == (short) 6)
    {
      int iOffset2 = this.MoveNext(iOffset1);
      if (iOffset2 < this.m_iUsedSize && this.m_dataProvider.ReadInt16(iOffset2) == (short) 545)
        return true;
    }
    return false;
  }

  internal string GetErrorString(int value)
  {
    IDictionary errorCodeToName = (IDictionary) FormulaUtil.ErrorCodeToName;
    return !errorCodeToName.Contains((object) value) ? (string) null : (string) errorCodeToName[(object) value];
  }

  internal void SetWorkbook(WorkbookImpl book, int iRow)
  {
    this.m_book = book;
    this.m_row = iRow;
  }

  [CLSCompliant(false)]
  public void SetFormulaValue(int iColumn, double value, StringRecord strRecord, int iBlockSize)
  {
    bool flag = false;
    bool bFound;
    int recordStart = this.LocateRecord(iColumn, out bFound);
    if (!bFound)
      throw new ApplicationException("Cannot set formula number.");
    FormulaRecord.WriteDoubleValue(this.m_dataProvider, recordStart, this.Version, value);
    int iOffset = recordStart + ((int) this.m_dataProvider.ReadInt16(recordStart + 2) + 4);
    if (iOffset < this.m_iUsedSize)
    {
      int num = (int) this.m_dataProvider.ReadInt16(iOffset);
      if (num == 545)
      {
        flag = true;
        iOffset = this.MoveNext(iOffset);
        num = (int) this.m_dataProvider.ReadInt16(iOffset);
      }
      if (num == 519 && !flag)
        this.RemoveRecord(iOffset);
    }
    if (strRecord == null)
      return;
    int iRequiredSize = strRecord.GetStoreSize(this.Version) + 4;
    this.InsertRecordData(iOffset, 0, iRequiredSize, (BiffRecordRaw) strRecord, iBlockSize);
  }

  [CLSCompliant(false)]
  public ushort Height
  {
    get => (double) this.m_usHeight > 8190.0 ? (ushort) 8180 : this.m_usHeight;
    set
    {
      this.m_usHeight = (double) value <= 8190.0 ? value : throw new ArgumentOutOfRangeException("Row Height should be less than " + (object) 409.5);
    }
  }

  [CLSCompliant(false)]
  public ushort ExtendedFormatIndex
  {
    get => this.m_usXFIndex;
    set
    {
      if ((int) this.m_usXFIndex == (int) value)
        return;
      this.m_usXFIndex = value;
      if (value == (ushort) 15)
        return;
      this.IsFormatted = true;
    }
  }

  [CLSCompliant(false)]
  public ushort OutlineLevel
  {
    get => (ushort) (this.m_optionFlags & (RowRecord.OptionFlags) 7);
    set
    {
      if (value > (ushort) 7)
        throw new ArgumentOutOfRangeException();
      this.m_optionFlags = this.m_optionFlags & (RowRecord.OptionFlags) -8 | (RowRecord.OptionFlags) ((int) value & 7);
    }
  }

  public bool IsCollapsed
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.Colapsed) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.Colapsed;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.Colapsed;
    }
  }

  public bool IsHidden
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.ZeroHeight) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.ZeroHeight;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.ZeroHeight;
    }
  }

  public bool IsBadFontHeight
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.BadFontHeight) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.BadFontHeight;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.BadFontHeight;
    }
  }

  public bool IsFormatted
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.Formatted) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.Formatted;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.Formatted;
    }
  }

  public bool IsSpaceAboveRow
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.SpaceAbove) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.SpaceAbove;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.SpaceAbove;
    }
  }

  public bool IsSpaceBelowRow
  {
    get => (this.m_optionFlags & RowRecord.OptionFlags.SpaceBelow) != (RowRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.SpaceBelow;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.SpaceBelow;
    }
  }

  public bool IsGroupShown
  {
    get
    {
      return (this.m_optionFlags & RowRecord.OptionFlags.ShowOutlineGroups) != (RowRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_optionFlags |= RowRecord.OptionFlags.ShowOutlineGroups;
      else
        this.m_optionFlags &= ~RowRecord.OptionFlags.ShowOutlineGroups;
    }
  }

  ushort IOutline.Index
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  internal bool IsFilteredRow
  {
    get => this.m_isFilteredRow;
    set => this.m_isFilteredRow = value;
  }

  internal List<int> ColumnFilterHideRow
  {
    get
    {
      if (this.m_columnFilterHideRow == null)
        this.m_columnFilterHideRow = new List<int>();
      if (!this.IsHidden)
        this.m_columnFilterHideRow.Clear();
      return this.m_columnFilterHideRow;
    }
  }

  [Flags]
  private enum StorageOptions
  {
    None = 0,
    HasRKBlank = 1,
    HasMultiRKBlank = 2,
    Disposed = 4,
  }

  public delegate void CellMethod(TBIFFRecord recordType, int offset, object data);

  private class OffsetData
  {
    public int StartOffset;
    public int UsedSize;
  }

  private class WriteData
  {
    public int Offset;
    public int UsedSize;
  }

  private delegate int DefragmentHelper(object helperData);
}
