// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.RecordTable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class RecordTable : ICloneable, IDisposable
{
  private int m_iRowCount;
  internal ArrayListEx m_arrRows = new ArrayListEx();
  private int m_iFirstRow = -1;
  private int m_iLastRow = -1;
  private bool m_bIsDisposed;
  private Dictionary<long, SharedFormulaRecord> m_arrShared;
  private WorkbookImpl m_book;
  internal IInternalWorksheet m_sheet;

  public RecordTable(int iRowCount, IInternalWorksheet sheet)
  {
    this.m_sheet = sheet;
    this.m_book = sheet.ParentWorkbook;
    this.m_iRowCount = iRowCount;
  }

  protected RecordTable(RecordTable data, bool clone, IInternalWorksheet sheet)
  {
    this.m_iRowCount = data.m_iRowCount;
    this.m_sheet = sheet;
    this.m_book = sheet.ParentWorkbook;
    if (!clone)
      return;
    this.m_iFirstRow = data.m_iFirstRow;
    this.m_iLastRow = data.m_iLastRow;
    ArrayListEx arrRows = data.m_arrRows;
    for (int index = 0; index < this.m_iRowCount; ++index)
    {
      RowStorage row = data.Rows[index];
      if (row != null)
        this.m_arrRows[index] = !(sheet is ExternWorksheetImpl) ? (RowStorage) row.Clone(this.m_book.HeapHandle) : (RowStorage) row.Clone(IntPtr.Zero);
    }
  }

  public void Dispose()
  {
    if (this.m_bIsDisposed)
      return;
    if (this.m_iFirstRow >= 0)
    {
      for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
        this.m_arrRows[iFirstRow]?.Dispose();
    }
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
    this.m_iRowCount = -1;
    this.m_arrRows = (ArrayListEx) null;
    this.m_bIsDisposed = true;
    this.m_book = (WorkbookImpl) null;
    if (this.m_arrShared != null)
    {
      this.m_arrShared.Clear();
      this.m_arrShared = (Dictionary<long, SharedFormulaRecord>) null;
    }
    GC.SuppressFinalize((object) this);
  }

  ~RecordTable() => this.Dispose();

  public virtual object Clone() => (object) new RecordTable(this, true, this.m_sheet);

  public virtual object Clone(IInternalWorksheet parentWorksheet)
  {
    return (object) new RecordTable(this, true, parentWorksheet);
  }

  public IApplication Application
  {
    [DebuggerStepThrough] get => this.m_book.Application;
  }

  public ApplicationImpl AppImplementation => this.m_book.AppImplementation;

  public ArrayListEx Rows => this.m_arrRows;

  public int RowCount
  {
    get => this.m_iRowCount;
    set
    {
      this.m_iRowCount = value;
      this.m_arrRows.ReduceSizeIfNecessary(value);
      this.m_iLastRow = Math.Min(this.m_iLastRow, value);
      this.m_iFirstRow = Math.Min(this.m_iFirstRow, value);
    }
  }

  public int FirstRow => this.m_iFirstRow;

  public int LastRow => this.m_iLastRow;

  public object this[int rowIndex, int colIndex]
  {
    get
    {
      if (rowIndex >= this.m_iRowCount || rowIndex < 0)
        return (object) null;
      RowStorage row = this.Rows[rowIndex];
      if (row == null)
        return (object) null;
      ICellPositionFormat record = row.GetRecord(colIndex, this.Application.RowStorageAllocationBlockSize);
      WorksheetImpl sheet = this.m_sheet as WorksheetImpl;
      if (row != null && sheet != null && sheet.IsInsertingSubTotal && sheet.MovedRows != null && sheet.MovedRows.ContainsKey(rowIndex + 1) && record != null)
        record.Row += sheet.MovedRows[rowIndex + 1];
      return (object) record;
    }
    set
    {
      if (rowIndex >= this.m_iRowCount || rowIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (rowIndex));
      RowStorage row = this.GetOrCreateRow(rowIndex, this.m_sheet.DefaultRowHeight, value != null, this.m_book.Version);
      if (row == null)
        return;
      row.SetWorkbook(this.m_book, rowIndex);
      row.SetRecord(colIndex, (ICellPositionFormat) value, this.Application.RowStorageAllocationBlockSize);
    }
  }

  [CLSCompliant(false)]
  public Dictionary<long, SharedFormulaRecord> SharedFormulas
  {
    get
    {
      if (this.m_arrShared == null)
        this.m_arrShared = new Dictionary<long, SharedFormulaRecord>();
      return this.m_arrShared;
    }
  }

  private int SharedCount => this.m_arrShared == null ? 0 : this.m_arrShared.Count;

  public WorkbookImpl Workbook => this.m_book;

  public void Clear()
  {
    if (this.m_iFirstRow == -1)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      RowStorage arrRow = this.m_arrRows[iFirstRow];
      this.m_arrRows[iFirstRow] = (RowStorage) null;
      arrRow?.Dispose();
    }
  }

  internal void UpdateRows(int rowCount) => this.m_arrRows.UpdateSize(rowCount);

  public virtual RowStorage CreateCellCollection(int iRowIndex, int height, ExcelVersion version)
  {
    RowStorage cellCollection = new RowStorage(iRowIndex, height, this.m_book.DefaultXFIndex);
    cellCollection.IsFormatted = false;
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        cellCollection.SetCellPositionSize(4, this.AppImplementation.RowStorageAllocationBlockSize, version);
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        cellCollection.SetCellPositionSize(8, this.AppImplementation.RowStorageAllocationBlockSize, version);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    return cellCollection;
  }

  public bool Contains(int rowIndex, int colIndex)
  {
    if (rowIndex < 0 || rowIndex >= this.m_iRowCount || this.m_arrRows == null)
      return false;
    RowStorage arrRow = this.m_arrRows[rowIndex];
    return arrRow != null && arrRow.Contains(colIndex);
  }

  [CLSCompliant(false)]
  public ArrayRecord GetArrayRecord(ICellPositionFormat cell)
  {
    if (cell == null)
      throw new ArgumentNullException(nameof (cell));
    if (cell.TypeCode != TBIFFRecord.Formula)
      return (ArrayRecord) null;
    Ptg[] parsedExpression = ((FormulaRecord) cell).ParsedExpression;
    if (parsedExpression == null || parsedExpression.Length != 1)
      return (ArrayRecord) null;
    if (!(parsedExpression[0] is ControlPtg controlPtg))
      return (ArrayRecord) null;
    return this.Rows[controlPtg.RowIndex]?.GetArrayRecord(controlPtg.ColumnIndex);
  }

  public void AccessRow(int iRowIndex)
  {
    if (this.m_iFirstRow < 0)
    {
      this.m_iFirstRow = iRowIndex;
      this.m_iLastRow = iRowIndex;
    }
    else
    {
      this.m_iFirstRow = Math.Min(this.m_iFirstRow, iRowIndex);
      this.m_iLastRow = Math.Max(this.m_iLastRow, iRowIndex);
    }
  }

  public void SetRow(int iRowIndex, RowStorage row)
  {
    this.Rows[iRowIndex] = row;
    this.AccessRow(iRowIndex);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    if (this.m_iFirstRow < 0 || this.m_arrRows == null)
      return;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, allocationBlockSize, this.m_book);
  }

  public void RemoveLastColumn(int iColumnIndex)
  {
    if (this.m_arrRows == null)
      return;
    int allocationBlockSize = this.m_book.Application.RowStorageAllocationBlockSize;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.Remove(iColumnIndex, iColumnIndex + 1, allocationBlockSize);
  }

  public void RemoveRow(int iRowIndex)
  {
    if (this.m_arrRows == null)
      return;
    this.m_arrRows[iRowIndex]?.Dispose();
    this.SetRow(iRowIndex, (RowStorage) null);
    bool flag = false;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      if (this.m_arrRows[iFirstRow] != null)
      {
        flag = true;
        this.m_iFirstRow = iFirstRow;
        break;
      }
    }
    for (int iLastRow = this.m_iLastRow; iLastRow >= this.m_iFirstRow; --iLastRow)
    {
      if (this.m_arrRows[iLastRow] != null)
      {
        flag = true;
        this.m_iLastRow = iLastRow;
        break;
      }
    }
    if (flag)
      return;
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
  }

  public void UpdateNameIndexes(WorkbookImpl book, int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    if (this.m_iFirstRow < 0)
      return;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateNameIndexes(book, arrNewIndex, allocationBlockSize);
  }

  public void UpdateNameIndexes(WorkbookImpl book, IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    if (this.m_iFirstRow < 0)
      return;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateNameIndexes(book, dicNewIndex, allocationBlockSize);
  }

  public void ReplaceSharedFormula(WorkbookImpl book)
  {
    if (this.SharedCount > 0)
    {
      foreach (KeyValuePair<long, SharedFormulaRecord> sharedFormula in this.SharedFormulas)
      {
        long key = sharedFormula.Key;
        SharedFormulaRecord shared = sharedFormula.Value;
        int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(key);
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(key);
        this.ReplaceSharedFormula(book, rowFromCellIndex, columnFromCellIndex, shared);
      }
    }
    this.m_arrShared = (Dictionary<long, SharedFormulaRecord>) null;
  }

  [CLSCompliant(false)]
  public void ReplaceSharedFormula(
    WorkbookImpl book,
    int row,
    int column,
    SharedFormulaRecord shared)
  {
    int firstRow = shared.FirstRow;
    for (int lastRow = shared.LastRow; firstRow <= lastRow; ++firstRow)
      this.m_arrRows[firstRow]?.ReplaceSharedFormula(book, row, column, shared);
  }

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    if (this.m_iFirstRow < 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateStringIndexes(arrNewIndexes);
  }

  public void CopyCells(
    RecordTable sourceCells,
    SSTDictionary sourceSST,
    SSTDictionary destSST,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicNameIndexes,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dictExternSheet)
  {
    this.m_iFirstRow = sourceCells.m_iFirstRow;
    this.m_iLastRow = sourceCells.m_iLastRow;
    this.m_arrRows = new ArrayListEx();
    if (sourceCells.m_iFirstRow < 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      RowStorage arrRow = sourceCells.m_arrRows[iFirstRow];
      if (arrRow != null)
      {
        RowStorage row = arrRow.Clone(sourceSST, destSST, hashExtFormatIndexes, hashWorksheetNames, dicNameIndexes, dicFontIndexes, dictExternSheet);
        this.SetRow(iFirstRow, row);
      }
    }
  }

  public List<long> Find(
    IRange range,
    string findValue,
    ExcelFindType flags,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    return this.Find(range, findValue, flags, ExcelFindOptions.None, bIsFindFirst, book);
  }

  public List<long> Find(
    IRange range,
    string findValue,
    ExcelFindType flags,
    ExcelFindOptions findOptions,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (findValue == null || findValue.Length == 0)
      return (List<long>) null;
    bool flag1 = FormulaUtil.ErrorNameToCode.ContainsKey(findValue);
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag4 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag5 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag6 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag7 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    if (!flag2 && !flag4 && !flag3 && !flag5 && !flag6 && !flag7)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    int iErrorCode = 0;
    bool flag8 = flag3 || flag1;
    List<long> longList = new List<long>();
    Regex wildCardRegex = (range.Worksheet as WorksheetImpl).GetWildCardRegex(findValue, string.Empty);
    if (flag8)
      iErrorCode = FormulaUtil.ErrorNameToCode[findValue];
    int iFirstColumn = range.Column - 1;
    int iLastColumn = range.LastColumn - 1;
    if (this.m_arrRows != null)
    {
      int index1 = range.Row - 1;
      for (int index2 = range.LastRow - 1; index1 <= index2; ++index1)
      {
        RowStorage arrRow = this.m_arrRows[index1];
        if (arrRow != null)
        {
          longList.AddRange((IEnumerable<long>) arrRow.Find(iFirstColumn, iLastColumn, findValue, flags, findOptions, iErrorCode, bIsFindFirst, book, range.Worksheet.Index, wildCardRegex));
          if (longList.Count > 0 && bIsFindFirst)
            break;
        }
      }
    }
    book.IsStartsOrEndsWith = new bool?();
    return longList;
  }

  public List<long> Find(
    IRange range,
    double findValue,
    ExcelFindType flags,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    bool flag1 = (flags & ExcelFindType.FormulaValue) != (ExcelFindType) 0;
    bool flag2 = (flags & ExcelFindType.Number) != (ExcelFindType) 0;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    List<long> longList = new List<long>();
    int iFirstColumn = range.Column - 1;
    int iLastColumn = range.LastColumn - 1;
    if (this.m_arrRows != null)
    {
      int index1 = range.Row - 1;
      for (int index2 = range.LastRow - 1; index1 <= index2; ++index1)
      {
        RowStorage arrRow = this.m_arrRows[index1];
        if (arrRow != null)
        {
          longList.AddRange((IEnumerable<long>) arrRow.Find(iFirstColumn, iLastColumn, findValue, flags, bIsFindFirst, book));
          if (bIsFindFirst && longList.Count > 0)
            break;
        }
      }
    }
    return longList;
  }

  public List<long> Find(
    IRange range,
    byte findValue,
    bool bErrorCode,
    bool bIsFindFirst,
    WorkbookImpl book)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    List<long> longList = new List<long>();
    int iFirstColumn = range.Column - 1;
    int iLastColumn = range.LastColumn - 1;
    if (this.m_arrRows != null)
    {
      int index1 = range.Row - 1;
      for (int index2 = range.LastRow - 1; index1 <= index2; ++index1)
      {
        RowStorage arrRow = this.m_arrRows[index1];
        if (arrRow != null)
          longList.AddRange((IEnumerable<long>) arrRow.Find(iFirstColumn, iLastColumn, findValue, bErrorCode, bIsFindFirst, book));
      }
    }
    return longList;
  }

  public int GetMinimumColumnIndex(int iStartRow, int iEndRow)
  {
    bool flag = false;
    int val1 = int.MaxValue;
    if (this.m_arrRows == null)
      return -1;
    for (int index = iStartRow; index <= iEndRow; ++index)
    {
      RowStorage arrRow = this.m_arrRows[index];
      if (arrRow != null && arrRow.UsedSize > 0)
      {
        val1 = Math.Min(val1, arrRow.FirstColumn);
        flag = true;
      }
    }
    return !flag ? -1 : val1;
  }

  public int GetMaximumColumnIndex(int iStartRow, int iEndRow)
  {
    bool flag = false;
    int val1 = int.MinValue;
    if (this.m_arrRows == null)
      return -1;
    for (int index = iStartRow; index <= iEndRow; ++index)
    {
      RowStorage arrRow = this.m_arrRows[index];
      if (arrRow != null && arrRow.UsedSize > 0)
      {
        val1 = Math.Max(val1, arrRow.LastColumn);
        flag = true;
      }
    }
    return !flag ? -1 : val1;
  }

  public bool ContainsRow(int iRowIndex)
  {
    return this.m_arrRows != null && iRowIndex >= 0 && iRowIndex <= this.m_arrRows.GetCount() - 1 && this.m_arrRows[iRowIndex] != null;
  }

  public List<long> Find(Dictionary<int, object> dictIndexes)
  {
    List<long> arrRanges = new List<long>();
    if (dictIndexes == null || dictIndexes.Count == 0 || this.m_arrRows == null)
      return arrRanges;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.Find(dictIndexes, arrRanges);
    return arrRanges;
  }

  public RecordTable CacheAndRemove(
    Rectangle rectSource,
    int iDeltaRow,
    int iDeltaColumn,
    ref int iMaxRow,
    ref int iMaxColumn,
    bool bInsert)
  {
    RecordTable recordTable = new RecordTable(this.m_iRowCount, this.m_sheet);
    int num1 = Math.Max(rectSource.Y, this.m_iFirstRow);
    int num2 = bInsert ? Math.Max(rectSource.Bottom, this.m_iLastRow) : Math.Min(rectSource.Bottom, this.m_iLastRow);
    int x = rectSource.X;
    int right = rectSource.Right;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    for (int index = num1; index <= num2; ++index)
    {
      RowStorage arrRow = this.m_arrRows[index];
      if (arrRow != null)
      {
        RowStorage row = arrRow.Clone(x, right, this.Application.RowStorageAllocationBlockSize);
        if (row != null)
        {
          row.RowColumnOffset(iDeltaRow, iDeltaColumn, allocationBlockSize);
          arrRow.Remove(x, right, allocationBlockSize);
          iMaxRow = Math.Max(iMaxRow, index + iDeltaRow);
          iMaxColumn = Math.Max(iMaxColumn, row.LastColumn);
        }
        recordTable.SetRow(index + iDeltaRow, row);
      }
    }
    recordTable.m_iFirstRow = rectSource.Y + iDeltaRow;
    recordTable.m_iLastRow = rectSource.Bottom + iDeltaRow;
    return recordTable;
  }

  public void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats)
  {
    if (this.m_arrRows == null || this.m_iFirstRow < 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateExtendedFormatIndex(dictFormats, this.Application.RowStorageAllocationBlockSize);
  }

  public void UpdateExtendedFormatIndex(int[] arrFormats)
  {
    if (this.m_arrRows == null || this.m_iFirstRow < 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateExtendedFormatIndex(arrFormats, this.Application.RowStorageAllocationBlockSize);
  }

  public void UpdateExtendedFormatIndex(int maxCount)
  {
    if (this.m_arrRows == null || this.m_iFirstRow < 0)
      return;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateExtendedFormatIndex(maxCount, defaultXfIndex);
  }

  internal void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes, IncreaseIndex method)
  {
    if (this.m_arrRows == null)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateLabelSSTIndexes(dictUpdatedIndexes, method);
  }

  [CLSCompliant(false)]
  public void ExtractRanges(
    BiffReader reader,
    bool bIgnoreStyles,
    SSTDictionary sst,
    WorksheetImpl sheet,
    IDecryptor decryptor)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Stream baseStream = reader.BaseReader.BaseStream;
    byte[] buffer = reader.Buffer;
    RowStorage rowStorage = (RowStorage) null;
    int num1 = -1;
    int num2 = 0;
    int num3 = 0;
    ByteArrayDataProvider arrayDataProvider = new ByteArrayDataProvider(buffer);
    long position;
    while (true)
    {
      short num4;
      do
      {
        short int16_1;
        short length1;
        do
        {
          position = baseStream.Position;
          baseStream.Read(buffer, 0, 4);
          TBIFFRecord int16_2 = (TBIFFRecord) BitConverter.ToInt16(buffer, 0);
          int16_1 = BitConverter.ToInt16(buffer, 2);
          if (int16_2 == TBIFFRecord.Unknown)
            throw new ArgumentOutOfRangeException("recordType");
          switch (int16_2)
          {
            case TBIFFRecord.Formula:
            case TBIFFRecord.RString:
            case TBIFFRecord.LabelSST:
            case TBIFFRecord.Blank:
            case TBIFFRecord.Number:
            case TBIFFRecord.Label:
            case TBIFFRecord.BoolErr:
            case TBIFFRecord.RK:
              baseStream.Read(buffer, 4, (int) int16_1);
              short length2 = (short) ((int) int16_1 + 4);
              decryptor?.Decrypt((DataProvider) arrayDataProvider, 4, (int) length2, position + 4L);
              num2 = (int) BitConverter.ToUInt16(buffer, 4);
              num3 = (int) BitConverter.ToUInt16(buffer, 6);
              int uint16_1 = (int) BitConverter.ToUInt16(buffer, 8);
              WorksheetHelper.AccessColumn(this.m_sheet, num3 + 1);
              if (num1 != num2)
              {
                int heightInRowUnits = this.m_book.AppImplementation.StandardHeightInRowUnits;
                rowStorage = this.GetOrCreateRow(num2, heightInRowUnits, true, ExcelVersion.Excel97to2003);
                num1 = num2;
              }
              if (int16_2 == TBIFFRecord.LabelSST)
              {
                int int32 = BitConverter.ToInt32(buffer, 10);
                sst.AddIncrease(int32);
              }
              if (rowStorage.LastColumn < num3)
                rowStorage.AppendRecordData((int) length2, buffer, this.Application.RowStorageAllocationBlockSize);
              else
                rowStorage.InsertRecordData(num3, (int) length2, buffer, this.Application.RowStorageAllocationBlockSize);
              if (this.m_book.GetExtFormat(uint16_1).WrapText)
                rowStorage.IsWrapText = true;
              rowStorage.UpdateColumnIndexes(num3, num3);
              continue;
            case TBIFFRecord.MulRK:
            case TBIFFRecord.MulBlank:
              baseStream.Read(buffer, 4, (int) int16_1);
              decryptor?.Decrypt((DataProvider) arrayDataProvider, 4, (int) int16_1, position + 4L);
              num2 = (int) BitConverter.ToUInt16(buffer, 4);
              num3 = (int) BitConverter.ToUInt16(buffer, 6);
              short length3 = (short) ((int) int16_1 + 4);
              int uint16_2 = (int) BitConverter.ToUInt16(buffer, (int) length3 - 2);
              WorksheetHelper.AccessColumn(this.m_sheet, uint16_2 + 1);
              if (num1 != num2)
              {
                rowStorage = this.GetOrCreateRow(num2, 0, true, ExcelVersion.Excel97to2003);
                num1 = num2;
              }
              rowStorage.HasMultiRkBlank = true;
              if (bIgnoreStyles)
                throw new NotImplementedException();
              if (rowStorage.LastColumn < num3)
                rowStorage.AppendRecordData((int) length3, buffer, this.Application.RowStorageAllocationBlockSize);
              else
                rowStorage.InsertRecordData(num3, (int) length3, buffer, this.Application.RowStorageAllocationBlockSize);
              rowStorage.UpdateColumnIndexes(num3, uint16_2);
              continue;
            case TBIFFRecord.DBCell:
              goto label_36;
            case TBIFFRecord.String:
            case TBIFFRecord.Array:
              baseStream.Read(buffer, 4, (int) int16_1);
              length1 = (short) ((int) int16_1 + 4);
              decryptor?.Decrypt((DataProvider) arrayDataProvider, 4, (int) length1, position + 4L);
              continue;
            case TBIFFRecord.Row:
              goto label_33;
            case TBIFFRecord.Table:
              goto label_37;
            case TBIFFRecord.SharedFormula2:
              goto label_32;
            default:
              goto label_39;
          }
        }
        while (rowStorage == null);
        rowStorage.AppendRecordData((int) length1, buffer, this.Application.RowStorageAllocationBlockSize);
        continue;
label_32:
        baseStream.Position = position;
        SharedFormulaRecord record1 = (SharedFormulaRecord) reader.GetRecord(decryptor);
        this.AddSharedFormula(num2, num3, record1);
        continue;
label_33:
        baseStream.Read(buffer, 4, (int) int16_1);
        decryptor?.Decrypt((DataProvider) arrayDataProvider, 4, (int) int16_1, position + 4L);
        RowRecord record2 = (RowRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Row);
        record2.ParseStructure((DataProvider) arrayDataProvider, 4, 0, ExcelVersion.Excel97to2003);
        sheet.ParseRowRecord(record2, bIgnoreStyles);
        continue;
label_36:
        baseStream.Position += (long) int16_1;
        continue;
label_37:
        baseStream.Read(buffer, 4, (int) int16_1);
        num4 = (short) ((int) int16_1 + 4);
        BiffRecordFactory.GetRecord(TBIFFRecord.Table).ParseStructure((DataProvider) arrayDataProvider, 4, (int) num4, ExcelVersion.Excel97to2003);
      }
      while (rowStorage == null);
      rowStorage.AppendRecordData((int) num4, buffer, this.Application.RowStorageAllocationBlockSize);
    }
label_39:
    baseStream.Position = position;
  }

  public void AddSharedFormula(int row, int column, SharedFormulaRecord shared)
  {
    this.SharedFormulas.Add(RangeImpl.GetCellIndex(column, row), shared);
  }

  [CLSCompliant(false)]
  public bool ExtractRangesFast(
    IndexRecord index,
    BiffReader reader,
    bool bIgnoreStyles,
    SSTDictionary sst,
    WorksheetImpl sheet)
  {
    if (index == null)
      return false;
    Stream stream = reader != null ? reader.BaseStream : throw new ArgumentNullException(nameof (reader));
    long position = stream.Position;
    BinaryReader baseReader = reader.BaseReader;
    DataProvider dataProvider = reader.DataProvider;
    byte[] buffer = reader.Buffer;
    DBCellRecord record = (DBCellRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DBCell);
    bool rangesFast = true;
    int[] dbCells = index.DbCells;
    int index1 = 0;
    for (int length = dbCells.Length; index1 < length && rangesFast; ++index1)
    {
      int num = dbCells[index1];
      stream.Position = (long) num;
      stream.Read(buffer, 0, 4);
      int int16_1 = (int) BitConverter.ToInt16(buffer, 0);
      int int16_2 = (int) BitConverter.ToInt16(buffer, 2);
      if (int16_1 != 215)
      {
        rangesFast = false;
        break;
      }
      stream.Read(buffer, 4, int16_2);
      record.Length = int16_2;
      record.ParseStructure(dataProvider, 4, int16_2, ExcelVersion.Excel97to2003);
      record.StreamPos = (long) num;
      rangesFast = this.ParseDBCellRecord(record, reader, bIgnoreStyles, sst, sheet);
    }
    if (!rangesFast)
    {
      stream.Position = position;
      this.Clear();
    }
    return rangesFast;
  }

  public RowStorage GetOrCreateRow(int rowIndex, int height, bool bCreate, ExcelVersion version)
  {
    RowStorage row = this.Rows[rowIndex];
    if (row == null && bCreate)
    {
      this.m_arrRows[rowIndex] = row = this.CreateCellCollection(rowIndex, height, version);
      this.AccessRow(rowIndex);
      WorksheetHelper.AccessRow(this.m_sheet, rowIndex + 1);
    }
    if (row != null && bCreate && row.Provider == null)
      row.CreateDataProvider(this.m_book.HeapHandle);
    return row;
  }

  public void EnsureSize(int iSize) => this.m_arrRows.UpdateSize(iSize);

  public void InsertIntoDefaultRows(int iRowIndex, int iRowCount)
  {
    if (iRowIndex > this.m_iLastRow)
      return;
    this.EnsureSize(this.m_iLastRow + iRowCount + 1);
    this.m_arrRows.Insert(iRowIndex, iRowCount, this.m_iLastRow - iRowIndex + 1);
    this.m_iLastRow += iRowCount;
  }

  private bool ParseDBCellRecord(
    DBCellRecord dbCell,
    BiffReader reader,
    bool bIgnoreStyles,
    SSTDictionary sst,
    WorksheetImpl sheet)
  {
    if (dbCell == null)
      throw new ArgumentNullException(nameof (dbCell));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    RowRecord record = (RowRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Row);
    ushort[] cellOffsets = dbCell.CellOffsets;
    int length = cellOffsets.Length;
    if (length == 0)
      return true;
    long num1 = dbCell.StreamPos - (long) dbCell.RowOffset;
    byte[] buffer = reader.Buffer;
    DataProvider dataProvider = reader.DataProvider;
    Stream baseStream = reader.BaseStream;
    long num2 = num1 + (long) cellOffsets[0] + 16L /*0x10*/ + 4L;
    bool dbCellRecord = true;
    int index1 = 1;
    for (int index2 = length; index1 < index2 && dbCellRecord; ++index1)
    {
      baseStream.Position = num1;
      int num3 = this.FillRowRecord(record, baseStream, buffer, dataProvider);
      if (num3 < 0)
        return false;
      num1 += (long) (num3 + 4);
      sheet.ParseRowRecord(record, bIgnoreStyles);
      baseStream.Position = num2;
      int iDataSize = (int) cellOffsets[index1];
      num2 += (long) iDataSize;
      dbCellRecord &= this.ReadStorageData(record, baseStream, iDataSize, buffer, sst);
    }
    if (dbCellRecord)
    {
      long num4 = length > 1 ? baseStream.Position : num1 + 16L /*0x10*/ + 4L;
      int iDataSize = (int) (dbCell.StreamPos - num4);
      baseStream.Position = num1;
      int num5 = this.FillRowRecord(record, baseStream, buffer, dataProvider);
      dbCellRecord &= num5 > 0;
      if (dbCellRecord)
      {
        sheet.ParseRowRecord(record, bIgnoreStyles);
        baseStream.Position = num2;
        dbCellRecord &= this.ReadStorageData(record, baseStream, iDataSize, buffer, sst);
      }
    }
    return dbCellRecord;
  }

  private int FillRowRecord(RowRecord row, Stream stream, byte[] arrBuffer, DataProvider provider)
  {
    stream.Read(arrBuffer, 0, 20);
    int int16_1 = (int) BitConverter.ToInt16(arrBuffer, 0);
    int int16_2 = (int) BitConverter.ToInt16(arrBuffer, 2);
    if (int16_1 != 520)
      return -1;
    if (16 /*0x10*/ < int16_2)
      stream.Read(arrBuffer, 20, int16_2 - 16 /*0x10*/);
    row.ParseStructure(provider, 4, int16_2, ExcelVersion.Excel97to2003);
    return int16_2;
  }

  private bool ReadStorageData(
    RowRecord row,
    Stream stream,
    int iDataSize,
    byte[] arrBuffer,
    SSTDictionary sst)
  {
    bool flag = true;
    if (iDataSize > 0)
    {
      RowStorage row1 = this.GetOrCreateRow((int) row.RowNumber, 0, true, ExcelVersion.Excel97to2003);
      row1.UpdateRowInfo(row, this.Application.UseFastRecordParsing);
      int length = arrBuffer.Length;
      int num;
      for (; iDataSize > 0; iDataSize -= num)
      {
        num = Math.Min(iDataSize, length);
        stream.Read(arrBuffer, 0, num);
        row1.AppendRecordData(num, arrBuffer, this.Application.RowStorageAllocationBlockSize);
      }
      flag = row1.PrepareRowData(sst, ref this.m_arrShared);
    }
    return flag;
  }

  public void UpdateFormulaFlags()
  {
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateFormulaFlags();
  }

  public int GetBoolValue(int iRow, int iCol)
  {
    RowStorage arrRow = this.m_arrRows[iRow - 1];
    return arrRow != null ? arrRow.GetBoolValue(iCol - 1) : 0;
  }

  public int GetFormulaBoolValue(int iRow, int iCol)
  {
    RowStorage arrRow = this.m_arrRows[iRow - 1];
    return arrRow != null ? arrRow.GetFormulaBoolValue(iCol - 1) : 0;
  }

  public string GetErrorValue(int iRow, int iCol) => this.Rows[iRow - 1]?.GetErrorValue(iCol - 1);

  internal string GetErrorValue(byte value, int iRow)
  {
    return this.Rows[iRow - 1]?.GetErrorString((int) value & (int) byte.MaxValue);
  }

  public string GetFormulaErrorValue(int iRow, int iCol)
  {
    return this.Rows[iRow - 1]?.GetFormulaErrorValue(iCol - 1);
  }

  public double GetNumberValue(int iRow, int iCol)
  {
    RowStorage arrRow = this.m_arrRows[iRow - 1];
    if (arrRow == null)
      return double.NaN;
    arrRow.SetWorkbook(this.m_book, iRow);
    return arrRow.GetNumberValue(iCol - 1, this.m_sheet.Index);
  }

  public double GetFormulaNumberValue(int iRow, int iCol)
  {
    RowStorage arrRow = this.m_arrRows[iRow - 1];
    return arrRow != null ? arrRow.GetFormulaNumberValue(iCol - 1) : double.NaN;
  }

  public string GetStringValue(int iRow, int iCol, SSTDictionary sst)
  {
    return this.m_arrRows[iRow - 1]?.GetStringValue(iCol - 1, sst);
  }

  public string GetFormulaStringValue(int iRow, int iCol, SSTDictionary sst)
  {
    return this.m_arrRows[iRow - 1]?.GetFormulaStringValue(iCol - 1);
  }

  public Ptg[] GetFormulaValue(int iRow, int iCol)
  {
    return this.m_arrRows[iRow - 1]?.GetFormulaValue(iCol - 1);
  }

  public bool HasFormulaRecord(int iRow, int iCol)
  {
    RowStorage row = this.Rows[iRow - 1];
    return row != null && row.HasFormulaRecord(iCol - 1);
  }

  public bool HasFormulaArrayRecord(int iRow, int iCol)
  {
    RowStorage row = this.Rows[iRow];
    return row != null && row.HasFormulaArrayRecord(iCol);
  }

  public WorksheetImpl.TRangeValueType GetCellType(int row, int column, bool bNeedFormulaSubType)
  {
    RowStorage row1 = this.Rows[row - 1];
    return row1 != null ? row1.GetCellType(column - 1, bNeedFormulaSubType) : WorksheetImpl.TRangeValueType.Blank;
  }

  [CLSCompliant(false)]
  public void SetFormulaValue(int iRow, int iColumn, double value, StringRecord strRecord)
  {
    (this.m_arrRows[iRow - 1] ?? throw new ApplicationException("Cannot sets formula value.")).SetFormulaValue(iColumn - 1, value, strRecord, this.Application.RowStorageAllocationBlockSize);
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      this.m_arrRows[iFirstRow]?.UpdateReferenceIndexes(arrUpdatedIndexes);
  }
}
