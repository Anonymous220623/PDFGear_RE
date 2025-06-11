// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.CellRecordCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class CellRecordCollection : CommonObject, IDictionary, ICollection, IEnumerable
{
  private RecordTable m_dicRecords;
  private SFTable m_colRanges;
  private IInternalWorksheet m_worksheet;
  private WorkbookImpl m_book;
  private bool m_bUseCache;
  private RecordExtractor m_recordExtractor;

  public CellRecordCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_colRanges = new SFTable(this.m_book.MaxRowCount, this.m_book.MaxColumnCount);
    this.m_dicRecords = new RecordTable(this.m_book.MaxRowCount, this.m_worksheet);
    this.m_recordExtractor = new RecordExtractor();
  }

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (IInternalWorksheet)) as IInternalWorksheet;
    this.m_book = this.m_worksheet != null ? this.m_worksheet.ParentWorkbook : throw new ArgumentOutOfRangeException("parent", "Can't find parent worksheet");
  }

  public int FirstRow => this.m_worksheet.FirstRow;

  public int LastRow => this.m_worksheet.LastRow;

  public int FirstColumn => this.m_worksheet.FirstColumn;

  public int LastColumn => this.m_worksheet.LastColumn;

  internal IInternalWorksheet Sheet
  {
    get => this.m_worksheet is ExternWorksheetImpl ? this.m_worksheet : this.m_worksheet;
  }

  public RecordTable Table
  {
    [DebuggerStepThrough] get => this.m_dicRecords;
    [DebuggerStepThrough] set => this.m_dicRecords = value;
  }

  public bool UseCache
  {
    get => this.m_bUseCache;
    set
    {
      if (value == this.m_bUseCache)
        return;
      if (value)
        this.CreateRangesCollection();
      else
        this.m_colRanges = (SFTable) null;
      this.m_bUseCache = value;
    }
  }

  public OfficeVersion Version
  {
    get => throw new NotImplementedException();
    set
    {
      this.m_dicRecords.RowCount = this.m_book.MaxRowCount;
      if (this.FirstRow < 0)
        return;
      int num = -1;
      int rowIndex = this.FirstRow - 1;
      for (int lastRow = this.LastRow; rowIndex < lastRow; ++rowIndex)
      {
        ApplicationImpl appImplementation = this.m_book.AppImplementation;
        RowStorage row = this.m_dicRecords.GetOrCreateRow(rowIndex, appImplementation.StandardHeightInRowUnits, false, value);
        if (row != null)
        {
          row.SetVersion(value, this.AppImplementation.RowStorageAllocationBlockSize);
          num = rowIndex;
        }
      }
      if (num < 0)
        return;
      this.m_worksheet.LastRow = num + 1;
    }
  }

  public RecordExtractor RecordExtractor => this.m_recordExtractor;

  public int Count => throw new NotImplementedException();

  public bool IsFixedSize => false;

  public bool IsReadOnly => false;

  public ICollection Keys => throw new NotSupportedException();

  public ICollection Values => throw new NotSupportedException();

  public object this[object key]
  {
    get
    {
      return key is long key1 ? (object) this[key1] : throw new NotSupportedException("Non Int64 keys are not support");
    }
    set
    {
      if (!(key is long key1))
        throw new NotSupportedException("Non Int64 keys are not support");
      this[key1] = value as ICellPositionFormat;
    }
  }

  [CLSCompliant(false)]
  public ICellPositionFormat this[long key]
  {
    get
    {
      return this.m_dicRecords[RangeImpl.GetRowFromCellIndex(key) - 1, RangeImpl.GetColumnFromCellIndex(key) - 1] as ICellPositionFormat;
    }
    set => this[RangeImpl.GetRowFromCellIndex(key), RangeImpl.GetColumnFromCellIndex(key)] = value;
  }

  [CLSCompliant(false)]
  public ICellPositionFormat this[int iRow, int iColumn]
  {
    get => this.m_dicRecords[iRow - 1, iColumn - 1] as ICellPositionFormat;
    set
    {
      if (value == null)
      {
        this.Remove(iRow, iColumn);
      }
      else
      {
        this.m_dicRecords[iRow - 1, iColumn - 1] = (object) value;
        WorksheetHelper.AccessColumn(this.m_worksheet, iColumn);
        WorksheetHelper.AccessRow(this.m_worksheet, iRow);
      }
    }
  }

  public void Clear()
  {
    if (this.m_dicRecords == null)
      return;
    this.m_dicRecords.Clear();
  }

  public void Add(object key, object value)
  {
    if (!(key is long key1))
      return;
    this.Add(key1, value as ICellPositionFormat);
  }

  public IDictionaryEnumerator GetEnumerator()
  {
    return (IDictionaryEnumerator) new RecordTableEnumerator(this);
  }

  public void Remove(object key)
  {
    if (!(key is long key1))
      return;
    this.Remove(key1);
  }

  public bool Contains(object key) => this.Contains((long) key);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new RecordTableEnumerator(this);

  [CLSCompliant(false)]
  public void Add(long key, ICellPositionFormat value) => this.Add(value);

  [CLSCompliant(false)]
  public void Add(ICellPositionFormat value)
  {
    int row = value.Row;
    int column = value.Column;
    if (this.m_dicRecords.Contains(row, column))
      throw new ArgumentOutOfRangeException("Collection already contains such member.");
    this.SetCellRecord(row + 1, column + 1, value);
  }

  public void Remove(long key)
  {
    this.m_dicRecords[RangeImpl.GetRowFromCellIndex(key) - 1, RangeImpl.GetColumnFromCellIndex(key) - 1] = (object) null;
  }

  public void Remove(int iRow, int iColumn)
  {
    this.m_dicRecords[iRow - 1, iColumn - 1] = (object) null;
  }

  public bool ContainsRow(int iRowIndex) => this.m_dicRecords.ContainsRow(iRowIndex);

  public bool Contains(long key)
  {
    return this.m_dicRecords.Contains(RangeImpl.GetRowFromCellIndex(key) - 1, RangeImpl.GetColumnFromCellIndex(key) - 1);
  }

  public bool Contains(int iRow, int iColumn) => this.m_dicRecords.Contains(iRow - 1, iColumn - 1);

  public bool IsSynchronized => throw new NotSupportedException();

  public object SyncRoot => throw new NotSupportedException();

  public void CopyTo(Array array, int index) => throw new NotSupportedException();

  [CLSCompliant(false)]
  public int Serialize(OffsetArrayList records, List<DBCellRecord> arrDBCells)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    OfficeVersion version = OfficeVersion.Excel97to2003;
    int num1 = 0;
    if (this.FirstRow < 0)
      return num1;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    List<RowStorage> ranges = new List<RowStorage>(32 /*0x20*/);
    int num2;
    for (int i = firstRow; i <= lastRow; i = num2 + 1)
    {
      int iRowRecSize = 0;
      int iFirstRowOffset = 0;
      num2 = this.PrepareNextRowsBlock(records, ranges, i, ref iRowRecSize, ref iFirstRowOffset, lastRow, firstColumn, lastColumn, OfficeVersion.Excel97to2003);
      DBCellRecord record = (DBCellRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DBCell);
      int count = ranges.Count;
      record.CellOffsets = new ushort[count];
      arrDBCells.Add(record);
      if (count > 0)
      {
        record.CellOffsets[0] = (ushort) iRowRecSize;
        int index1 = 0;
        for (int index2 = count - 1; index1 < index2; ++index1)
        {
          RowStorage rowStorage = ranges[index1];
          int storeSize = rowStorage != null ? rowStorage.GetStoreSize(version) : 0;
          if (storeSize != 0)
            storeSize += 4;
          record.CellOffsets[index1 + 1] = (ushort) storeSize;
          if (rowStorage != null && storeSize > 0)
            records.Add((IBiffStorage) rowStorage);
          iFirstRowOffset += storeSize;
        }
        record.RowOffset = iFirstRowOffset;
        RowStorage rowStorage1 = ranges[count - 1];
        if (rowStorage1 != null && rowStorage1.GetStoreSize(version) > 0)
        {
          records.Add((IBiffStorage) rowStorage1);
          record.RowOffset += rowStorage1.GetStoreSize(version) + 4;
        }
      }
      records.Add((IBiffStorage) record);
      ++num1;
      ranges.Clear();
    }
    return num1;
  }

  private int PrepareNextRowsBlock(
    OffsetArrayList records,
    List<RowStorage> ranges,
    int i,
    ref int iRowRecSize,
    ref int iFirstRowOffset,
    int iLastRow,
    int iFirstSheetCol,
    int iLastSheetCol,
    OfficeVersion version)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (ranges == null)
      throw new ArgumentNullException(nameof (ranges));
    int num1 = 0;
    int num2 = i;
    int defaultRowHeight = this.m_worksheet.DefaultRowHeight;
    for (; i <= iLastRow; ++i)
    {
      RowStorage rowData = this.GetRowData(i, iFirstSheetCol, iLastSheetCol, out int _, out int _, version);
      RowRecord rowRecord;
      if (rowData != null)
      {
        rowRecord = rowData.CreateRowRecord(this.m_book);
        rowRecord.Worksheet = this.m_worksheet as WorksheetImpl;
        rowRecord.RowNumber = (ushort) (i - 1);
      }
      else
        rowRecord = (RowRecord) null;
      if (rowData != null && rowData.UsedSize > 0)
      {
        ranges.Add(rowData);
        if (rowRecord == null)
        {
          rowRecord = (RowRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Row);
          rowRecord.RowNumber = (ushort) (i - 1);
          rowRecord.Height = (ushort) defaultRowHeight;
          rowRecord.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        }
        rowRecord.FirstColumn = (ushort) rowData.FirstColumn;
        rowRecord.LastColumn = (ushort) rowData.LastColumn;
        int num3 = rowRecord.MaximumRecordSize + 4;
        if (i != num2)
          iRowRecSize += num3;
        iFirstRowOffset += num3;
        records.Add((IBiffStorage) rowRecord);
      }
      else if (rowRecord != null)
      {
        records.Add((IBiffStorage) rowRecord);
        ranges.Add(rowData);
        int num4 = rowRecord.GetStoreSize(version) + 4;
        iFirstRowOffset += num4;
        if (i != num2)
          iRowRecSize += num4;
      }
      else if (i == num2)
        ++num2;
      ++num1;
      if (num1 == 32 /*0x20*/)
        break;
    }
    return i;
  }

  [CLSCompliant(false)]
  protected RowStorage GetRowData(
    int index,
    int iFirstColumn,
    int iLastColumn,
    out int min,
    out int max,
    OfficeVersion version)
  {
    RowStorage rowData = this.m_dicRecords.Rows[index - 1];
    min = int.MinValue;
    max = int.MaxValue;
    if (rowData != null)
    {
      if (version != rowData.Version)
      {
        rowData = (RowStorage) rowData.Clone(IntPtr.Zero);
        rowData.SetVersion(version, this.AppImplementation.RowStorageAllocationBlockSize);
      }
      min = rowData.FirstColumn + 1;
      max = rowData.LastColumn + 1;
    }
    return rowData;
  }

  [CLSCompliant(false)]
  public bool ExtractRangesFast(
    IndexRecord index,
    BiffReader reader,
    bool bIgnoreStyles,
    Dictionary<int, int> hashNewXFIndexes)
  {
    return this.m_dicRecords.ExtractRangesFast(index, reader, bIgnoreStyles, this.m_book.InnerSST, (WorksheetImpl) this.m_worksheet);
  }

  [CLSCompliant(false)]
  public void AddRecord(BiffRecordRaw recordToAdd, bool bIgnoreStyles)
  {
    if (recordToAdd == null)
      throw new ArgumentNullException(nameof (recordToAdd));
    switch (recordToAdd.TypeCode)
    {
      case TBIFFRecord.MulRK:
        this.AddRecord((MulRKRecord) recordToAdd, bIgnoreStyles);
        break;
      case TBIFFRecord.MulBlank:
        this.AddRecord((MulBlankRecord) recordToAdd, bIgnoreStyles);
        break;
      default:
        this.AddRecord((ICellPositionFormat) recordToAdd, bIgnoreStyles);
        break;
    }
  }

  [CLSCompliant(false)]
  public void AddRecord(ICellPositionFormat cell, bool bIgnoreStyles)
  {
    if (cell == null)
      throw new ArgumentNullException(nameof (cell));
    if (bIgnoreStyles)
      cell.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    this.SetCellRecord(cell.Row + 1, cell.Column + 1, cell);
  }

  private void AddRecord(MulRKRecord mulRK, bool bIgnoreStyles)
  {
    List<MulRKRecord.RkRec> rkRecList = mulRK != null ? mulRK.Records : throw new ArgumentNullException(nameof (mulRK));
    int row = mulRK.Row;
    int firstColumn = mulRK.FirstColumn;
    int index = 0;
    int lastColumn = mulRK.LastColumn;
    while (firstColumn <= lastColumn)
    {
      RKRecord record = (RKRecord) this.m_recordExtractor.GetRecord(638);
      record.SetRKRecord(rkRecList[index]);
      record.Row = row;
      record.Column = firstColumn;
      this.AddRecord((ICellPositionFormat) record, bIgnoreStyles);
      ++firstColumn;
      ++index;
    }
  }

  private void AddRecord(MulBlankRecord mulBlank, bool bIgnoreStyles)
  {
    if (mulBlank == null)
      throw new ArgumentNullException(nameof (mulBlank));
    if (bIgnoreStyles)
      return;
    int firstColumn = mulBlank.FirstColumn;
    for (int lastColumn = mulBlank.LastColumn; firstColumn <= lastColumn; ++firstColumn)
      this.AddRecord((ICellPositionFormat) mulBlank.GetBlankRecord(firstColumn), bIgnoreStyles);
  }

  private void AddRecord(FormulaRecord formula, StringRecord stringRecord, bool bIgnoreStyles)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    this.AddRecord((ICellPositionFormat) formula, bIgnoreStyles);
  }

  internal bool IsRequireRange(FormulaRecord formula)
  {
    Ptg[] ptgArray = formula != null ? formula.ParsedExpression : throw new ArgumentNullException(nameof (formula));
    int index = 0;
    for (int length = ptgArray.Length; index < length; ++index)
    {
      FormulaToken tokenCode = ptgArray[index].TokenCode;
      if (FormulaUtil.IndexOf(FormulaUtil.NameCodes, tokenCode) != -1 || FormulaUtil.IndexOf(FormulaUtil.NameXCodes, tokenCode) != -1)
        return true;
    }
    return false;
  }

  internal void UpdateRows(int rowCount) => this.m_dicRecords.UpdateRows(rowCount);

  public CellRecordCollection Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    CellRecordCollection recordCollection = (CellRecordCollection) this.MemberwiseClone();
    recordCollection.SetParent(parent);
    recordCollection.SetParents();
    recordCollection.m_dicRecords = (RecordTable) this.m_dicRecords.Clone(recordCollection.m_worksheet);
    recordCollection.m_colRanges = new SFTable(this.m_book.MaxRowCount, this.m_book.MaxColumnCount);
    return recordCollection;
  }

  [CLSCompliant(false)]
  public void SetRange(long key, RangeImpl range)
  {
    if (!this.m_bUseCache)
      return;
    this.SetRange(RangeImpl.GetRowFromCellIndex(key), RangeImpl.GetColumnFromCellIndex(key), range);
  }

  [CLSCompliant(false)]
  public void SetRange(int iRow, int iColumn, RangeImpl range)
  {
    if (!this.m_bUseCache)
      return;
    this.m_colRanges[iRow - 1, iColumn - 1] = (object) range;
  }

  public RangeImpl GetRange(long key)
  {
    return this.GetRange(RangeImpl.GetRowFromCellIndex(key), RangeImpl.GetColumnFromCellIndex(key));
  }

  public RangeImpl GetRange(int iRow, int iColumn)
  {
    RangeImpl range = (RangeImpl) null;
    if (this.m_bUseCache)
      range = this.m_colRanges[iRow - 1, iColumn - 1] as RangeImpl;
    return range;
  }

  [CLSCompliant(false)]
  public void SetCellRecord(long key, ICellPositionFormat cell) => this[key] = cell;

  [CLSCompliant(false)]
  public void SetCellRecord(int iRow, int iColumn, ICellPositionFormat cell)
  {
    this[iRow, iColumn] = cell;
  }

  [CLSCompliant(false)]
  public ICellPositionFormat GetCellRecord(long key) => this[key];

  [CLSCompliant(false)]
  public ICellPositionFormat GetCellRecord(int iRow, int iColumn) => this[iRow, iColumn];

  public void ClearRange(Rectangle rect)
  {
    int top = rect.Top;
    int left = rect.Left;
    int bottom = rect.Bottom;
    int right = rect.Right;
    int allocationBlockSize = this.m_book.Application.RowStorageAllocationBlockSize;
    for (int index = top; index <= bottom; ++index)
      this.m_dicRecords.Rows[index]?.Remove(left, right, allocationBlockSize);
  }

  public void CopyCells(
    CellRecordCollection sourceCells,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<int, int> dicNewNameIndexes,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dictExternSheet)
  {
    SSTDictionary innerSst1 = sourceCells.m_book.InnerSST;
    SSTDictionary innerSst2 = this.m_book.InnerSST;
    this.Clear();
    this.m_dicRecords.CopyCells(sourceCells.m_dicRecords, innerSst1, innerSst2, hashExtFormatIndexes, hashWorksheetNames, dicNewNameIndexes, dicFontIndexes, dictExternSheet);
  }

  public RichTextString GetRTFString(long cellIndex, bool bAutofitRows)
  {
    ICellPositionFormat cellRecord = this.GetCellRecord(cellIndex);
    if (cellRecord == null)
      return (RichTextString) null;
    switch (cellRecord.TypeCode)
    {
      case TBIFFRecord.Formula:
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        TextWithFormat text = new TextWithFormat();
        string formulaStringValue = this.GetFormulaStringValue(cellIndex);
        if (formulaStringValue != null)
        {
          text.Text = formulaStringValue;
        }
        else
        {
          double number = this.GetNumber(cellIndex);
          if (cellRecord.TypeCode == TBIFFRecord.Formula && double.IsNaN(number))
          {
            text.Text = this.GetFormulaErrorBoolText(cellRecord as FormulaRecord);
          }
          else
          {
            FormatImpl format = this.GetFormat(cellIndex);
            text.Text = format.ApplyFormat(number, true);
          }
        }
        return (RichTextString) new RangeRichTextString(this.Application, (object) this.m_worksheet, cellIndex, text);
      case TBIFFRecord.LabelSST:
        return this.GetLabelSSTRTFString(cellIndex, bAutofitRows);
      case TBIFFRecord.BoolErr:
        return (RichTextString) new RangeRichTextString(this.Application, (object) this.m_worksheet, cellIndex, new TextWithFormat()
        {
          Text = RangeImpl.ParseBoolError((BoolErrRecord) cellRecord)
        });
      default:
        return (RichTextString) null;
    }
  }

  public void FillRTFString(long cellIndex, bool bAutofitRows, RichTextString richText)
  {
    richText.ClearFormatting();
    richText.Text = string.Empty;
    ICellPositionFormat cellRecord = this.GetCellRecord(cellIndex);
    if (cellRecord == null)
      return;
    switch (cellRecord.TypeCode)
    {
      case TBIFFRecord.Formula:
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        string str = this.GetFormulaStringValue(cellIndex);
        ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex];
        richText.DefaultFontIndex = innerExtFormat.FontIndex;
        if (str == null)
        {
          double number = this.GetNumber(cellIndex);
          str = cellRecord.TypeCode != TBIFFRecord.Formula || !double.IsNaN(number) ? (double.IsNaN(number) ? string.Empty : innerExtFormat.NumberFormatObject.ApplyFormat(number, true)) : this.GetFormulaErrorBoolText(cellRecord as FormulaRecord);
        }
        richText.ClearFormatting();
        richText.Text = str;
        break;
      case TBIFFRecord.LabelSST:
        this.FillLabelSSTRTFString((LabelSSTRecord) cellRecord, bAutofitRows, richText);
        break;
      case TBIFFRecord.BoolErr:
        richText.ClearFormatting();
        richText.Text = RangeImpl.ParseBoolError((BoolErrRecord) cellRecord);
        break;
    }
  }

  public RichTextString GetLabelSSTRTFString(long cellIndex, bool bAutofitRows)
  {
    RangeRichTextString labelSstrtfString1 = ((WorksheetImpl) this.Sheet).CreateLabelSSTRTFString(cellIndex);
    string str = this.GetFormat(cellIndex).ApplyFormat(labelSstrtfString1.Text, true);
    if (str == labelSstrtfString1.Text || bAutofitRows)
      return (RichTextString) labelSstrtfString1;
    IOfficeFont font = this.GetFont(cellIndex);
    RichTextString labelSstrtfString2 = new RichTextString(this.Application, (object) this.m_book, false, true);
    labelSstrtfString2.Text = str;
    labelSstrtfString2.SetFont(0, str.Length - 1, font);
    return labelSstrtfString2;
  }

  [CLSCompliant(false)]
  public void FillLabelSSTRTFString(
    LabelSSTRecord labelSST,
    bool bAutofitRows,
    RichTextString richText)
  {
    this.FillRichText(richText, labelSST.SSTIndex);
    ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[(int) labelSST.ExtendedFormatIndex];
    string str = this.m_book.InnerFormats[innerExtFormat.NumberFormatIndex].ApplyFormat(richText.Text, true);
    if (!(str == richText.Text) && !bAutofitRows)
    {
      IOfficeFont font = innerExtFormat.Font;
      richText.Text = str;
      richText.SetFont(0, str.Length - 1, font);
    }
    else
      richText.DefaultFontIndex = innerExtFormat.FontIndex;
  }

  public string GetText(long cellIndex)
  {
    ICellPositionFormat cellRecord = this.GetCellRecord(cellIndex);
    if (cellRecord != null)
    {
      if (cellRecord.TypeCode == TBIFFRecord.Label)
        return ((LabelRecord) cellRecord).Label;
      if (cellRecord.TypeCode == TBIFFRecord.LabelSST)
        return this.m_book.InnerSST[((LabelSSTRecord) cellRecord).SSTIndex].Text;
    }
    return (string) null;
  }

  public string GetError(long cellIndex)
  {
    if (!(this.GetCellRecord(cellIndex) is BoolErrRecord cellRecord) || !cellRecord.IsErrorCode)
      return (string) null;
    string error = "#N/A";
    int boolOrError = (int) cellRecord.BoolOrError;
    if (FormulaUtil.ErrorCodeToName.ContainsKey(boolOrError))
      error = FormulaUtil.ErrorCodeToName[boolOrError];
    return error;
  }

  public bool GetBool(long cellIndex, out bool value)
  {
    BoolErrRecord cellRecord = this.GetCellRecord(cellIndex) as BoolErrRecord;
    value = false;
    if (cellRecord == null || cellRecord.IsErrorCode)
      return false;
    value = cellRecord.BoolOrError > (byte) 0;
    return true;
  }

  public bool ContainNumber(long cellIndex) => this.GetCellRecord(cellIndex) is IDoubleValue;

  public bool ContainBoolOrError(long cellIndex) => this.GetCellRecord(cellIndex) is BoolErrRecord;

  public bool ContainFormulaNumber(long cellIndex)
  {
    return this.GetCellRecord(cellIndex) is FormulaRecord cellRecord && !cellRecord.IsBool && !cellRecord.IsError;
  }

  public bool ContainFormulaBoolOrError(long cellIndex)
  {
    if (!(this.GetCellRecord(cellIndex) is FormulaRecord cellRecord))
      return false;
    return cellRecord.IsBool || cellRecord.IsError;
  }

  public double GetNumber(long cellIndex)
  {
    return !(this.GetCellRecord(cellIndex) is IDoubleValue cellRecord) ? double.MinValue : cellRecord.DoubleValue;
  }

  public double GetNumberWithoutFormula(long cellIndex)
  {
    return !(this.GetCellRecord(cellIndex) is IDoubleValue cellRecord) || cellRecord.TypeCode == TBIFFRecord.Formula ? double.MinValue : cellRecord.DoubleValue;
  }

  public double GetFormulaNumberValue(long cellIndex)
  {
    return this.GetCellRecord(cellIndex) is FormulaRecord cellRecord ? cellRecord.Value : double.MinValue;
  }

  public void SetStringValue(long cellIndex, string strValue)
  {
    int index = RangeImpl.GetRowFromCellIndex(cellIndex) - 1;
    int iColumnIndex = RangeImpl.GetColumnFromCellIndex(cellIndex) - 1;
    (this.m_dicRecords.Rows[index] ?? throw new NotSupportedException("This property is only for formula ranges.")).SetFormulaStringValue(iColumnIndex, strValue, this.Application.RowStorageAllocationBlockSize);
  }

  public string GetFormulaStringValue(long cellIndex)
  {
    return this.m_dicRecords.Rows[RangeImpl.GetRowFromCellIndex(cellIndex) - 1].GetFormulaStringValue(RangeImpl.GetColumnFromCellIndex(cellIndex) - 1);
  }

  public DateTime GetDateTime(long cellIndex)
  {
    double numberWithoutFormula = this.GetNumberWithoutFormula(cellIndex);
    return numberWithoutFormula == double.MinValue || this.GetFormat(cellIndex).GetFormatType(numberWithoutFormula) != OfficeFormatType.DateTime ? DateTime.MinValue : DateTime.FromOADate(numberWithoutFormula);
  }

  public RecordTable CacheIntersection(
    IRange destination,
    IRange source,
    out Rectangle rectIntersection)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination.Worksheet != source.Worksheet)
    {
      rectIntersection = Rectangle.FromLTRB(-1, -1, -1, -1);
      return (RecordTable) null;
    }
    int column = source.Column;
    int row1 = source.Row;
    int height = source.LastRow - row1 + 1;
    int width = source.LastColumn - column + 1;
    Rectangle rectangle1 = new Rectangle(destination.Column, destination.Row, width, height);
    Rectangle rectangle2 = new Rectangle(column, row1, width, height);
    if (!UtilityMethods.Intersects(rectangle1, rectangle2))
    {
      rectIntersection = Rectangle.FromLTRB(-1, -1, -1, -1);
      return (RecordTable) null;
    }
    rectIntersection = Rectangle.Intersect(rectangle1, rectangle2);
    if (rectIntersection.Width == 0 || rectIntersection.Height == 0)
    {
      rectIntersection = Rectangle.FromLTRB(-1, -1, -1, -1);
      return (RecordTable) null;
    }
    RecordTable recordTable = new RecordTable(this.m_book.MaxRowCount, this.m_worksheet);
    for (int top = rectIntersection.Top; top < rectIntersection.Bottom; ++top)
    {
      RowStorage row2 = this.m_dicRecords.Rows[top - 1];
      if (row2 != null)
      {
        RowStorage row3 = row2.Clone(rectIntersection.Left - 1, rectIntersection.Right - 1, this.Application.RowStorageAllocationBlockSize);
        recordTable.SetRow(top - 1, row3);
      }
    }
    return recordTable;
  }

  public int GetMinimumRowIndex(int iStartColumn, int iEndColumn)
  {
    int firstRow = this.m_worksheet.FirstRow;
    int minimumRowIndex = this.m_worksheet.LastRow;
    for (int index1 = firstRow; index1 < minimumRowIndex; ++index1)
    {
      for (int index2 = iStartColumn; index2 <= iEndColumn; ++index2)
      {
        if (this.m_dicRecords.Contains(index1 - 1, index2 - 1))
        {
          minimumRowIndex = index1;
          break;
        }
      }
    }
    return minimumRowIndex;
  }

  public int GetMaximumRowIndex(int iStartColumn, int iEndColumn)
  {
    int lastRow = this.m_worksheet.LastRow;
    int maximumRowIndex = this.m_worksheet.FirstRow;
    for (int firstRow = lastRow; firstRow >= maximumRowIndex; --firstRow)
    {
      for (int firstColumn = iStartColumn; firstColumn <= iEndColumn; ++firstColumn)
      {
        if (this.Contains(RangeImpl.GetCellIndex(firstColumn, firstRow)))
        {
          maximumRowIndex = firstRow;
          break;
        }
      }
    }
    return maximumRowIndex;
  }

  public int GetMinimumColumnIndex(int iStartRow, int iEndRow)
  {
    return this.m_dicRecords.GetMinimumColumnIndex(iStartRow - 1, iEndRow - 1) + 1;
  }

  public int GetMaximumColumnIndex(int iStartRow, int iEndRow)
  {
    return this.m_dicRecords.GetMaximumColumnIndex(iStartRow - 1, iEndRow - 1) + 1;
  }

  public string GetFormula(long cellIndex) => this.GetFormula(cellIndex, false);

  public string GetFormula(long cellIndex, bool isR1C1)
  {
    return this.GetFormula(cellIndex, isR1C1, (NumberFormatInfo) null);
  }

  public string GetFormula(long cellIndex, bool isR1C1, NumberFormatInfo numberInfo)
  {
    if (!(this.GetCellRecord(cellIndex) is FormulaRecord cellRecord))
      return (string) null;
    try
    {
      return "=" + this.m_book.FormulaUtil.ParsePtgArray(cellRecord.ParsedExpression, cellRecord.Row, cellRecord.Column, isR1C1, numberInfo, false);
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }

  public string GetValue(long cellIndex, int row, int column, IRange range, string seperator)
  {
    if (!this.Contains(cellIndex))
      return string.Empty;
    string str = "";
    bool flag1 = false;
    if (this.GetFormula(cellIndex) != null)
      return range[row, column].DisplayText;
    string text = this.GetText(cellIndex);
    if (text != null)
    {
      str = text;
      flag1 = true;
    }
    if (flag1)
    {
      if (str.StartsWith("\""))
        str = '"'.ToString() + str.Replace("\"", "\"\"") + (object) '"';
      else if (str.Contains("\""))
        return '"'.ToString() + str.Replace("\"", "\"\"") + (object) '"';
      if (str.Contains(seperator))
        str = $"\"{str}\"";
      return str;
    }
    string error = this.GetError(cellIndex);
    if (error != null)
      str = error;
    bool flag2;
    if (this.GetBool(cellIndex, out flag2))
      str = flag2.ToString();
    double numberWithoutFormula = this.GetNumberWithoutFormula(cellIndex);
    if (numberWithoutFormula != double.MinValue)
      str = this.GetFormat(cellIndex).ApplyFormat(numberWithoutFormula);
    if (this.GetDateTime(cellIndex) != DateTime.MinValue)
      str = range[row, column].DisplayText;
    return str;
  }

  public int GetExtendedFormatIndex(long cellIndex)
  {
    return this.GetExtendedFormatIndex(RangeImpl.GetRowFromCellIndex(cellIndex), RangeImpl.GetColumnFromCellIndex(cellIndex));
  }

  public int GetExtendedFormatIndex(int row, int column)
  {
    --row;
    --column;
    RowStorage row1 = this.Table.Rows[row];
    int extendedFormatIndex = int.MinValue;
    if (row1 != null)
      extendedFormatIndex = row1.GetXFIndexByColumn(column);
    return extendedFormatIndex;
  }

  public int GetExtendedFormatIndexByRow(int row)
  {
    --row;
    RowStorage row1 = this.Table.Rows[row];
    int formatIndexByRow = int.MinValue;
    if (row1 != null)
      formatIndexByRow = (int) row1.ExtendedFormatIndex;
    return formatIndexByRow;
  }

  public int GetExtendedFormatIndexByColumn(int column)
  {
    int formatIndexByColumn = int.MinValue;
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    record.FirstColumn = (ushort) (column - 1);
    if (record != null)
      formatIndexByColumn = (int) record.ExtendedFormatIndex;
    return formatIndexByColumn;
  }

  public IOfficeFont GetCellFont(long cellIndex)
  {
    int extendedFormatIndex = this.GetExtendedFormatIndex(cellIndex);
    return extendedFormatIndex < 0 ? (IOfficeFont) null : this.m_book.InnerExtFormats[extendedFormatIndex].Font;
  }

  public void CopyStyle(int iSourceRow, int iSourceColumn, int iDestRow, int iDestColumn)
  {
    ICellPositionFormat cellRecord = this.GetCellRecord(iSourceRow, iSourceColumn);
    if (cellRecord == null)
      return;
    ushort extendedFormatIndex = cellRecord.ExtendedFormatIndex;
    this.SetCellStyle(iDestRow, iDestColumn, (int) extendedFormatIndex);
  }

  [CLSCompliant(false)]
  public ICellPositionFormat CreateCellNoAdd(int iRow, int iColumn, TBIFFRecord recordType)
  {
    ICellPositionFormat record = (ICellPositionFormat) BiffRecordFactory.GetRecord(recordType);
    record.Row = iRow - 1;
    record.Column = iColumn - 1;
    return record;
  }

  [CLSCompliant(false)]
  public ICellPositionFormat CreateCell(int iRow, int iColumn, TBIFFRecord recordType)
  {
    ICellPositionFormat cellNoAdd = this.CreateCellNoAdd(iRow, iColumn, recordType);
    this.SetCellRecord(iRow, iColumn, cellNoAdd);
    return cellNoAdd;
  }

  public IStyle GetCellStyle(long cellIndex)
  {
    return (IStyle) this.m_book.InnerStyles.GetByXFIndex((int) this.GetCellRecord(cellIndex).ExtendedFormatIndex);
  }

  public IExtendedFormat GetCellFormatting(long cellIndex)
  {
    return (IExtendedFormat) this.m_book.InnerExtFormats[(int) this.GetCellRecord(cellIndex).ExtendedFormatIndex];
  }

  public void SetNumberValue(int iRow, int iCol, double dValue)
  {
    this.SetNumberValue(iCol, iRow, dValue, this.m_book.DefaultXFIndex);
  }

  public void SetNumberValue(long cellIndex, double dValue)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    this.SetNumberValue(RangeImpl.GetColumnFromCellIndex(cellIndex), rowFromCellIndex, dValue);
  }

  public void SetNumberValue(int iRow, int iCol, double dValue, int iXFIndex)
  {
    NumberRecord record = (NumberRecord) this.m_recordExtractor.GetRecord(515);
    record.Value = dValue;
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    this[iRow, iCol] = (ICellPositionFormat) record;
  }

  public void SetBooleanValue(int iRow, int iCol, bool bValue)
  {
    this.SetBooleanValue(iCol, iRow, bValue, this.m_book.DefaultXFIndex);
  }

  public void SetBooleanValue(long cellIndex, bool bValue)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    this.SetBooleanValue(RangeImpl.GetColumnFromCellIndex(cellIndex), rowFromCellIndex, bValue);
  }

  public void SetBooleanValue(int iRow, int iCol, bool bValue, int iXFIndex)
  {
    BoolErrRecord record = (BoolErrRecord) this.m_recordExtractor.GetRecord(517);
    record.IsErrorCode = false;
    record.BoolOrError = bValue ? (byte) 1 : (byte) 0;
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    this.SetCellRecord(RangeImpl.GetCellIndex(iCol, iRow), (ICellPositionFormat) record);
  }

  public void SetErrorValue(int iRow, int iCol, string strValue)
  {
    this.SetErrorValue(iCol, iRow, strValue, this.m_book.DefaultXFIndex);
  }

  public void SetErrorValue(long cellIndex, string strValue)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    this.SetErrorValue(RangeImpl.GetColumnFromCellIndex(cellIndex), rowFromCellIndex, strValue);
  }

  public void SetErrorValue(int iRow, int iCol, string strValue, int iXFIndex)
  {
    int errorCode;
    if (!FormulaUtil.ErrorNameToCode.TryGetValue(strValue, out errorCode))
      throw new ArgumentOutOfRangeException(nameof (strValue));
    this.SetErrorValue(iRow, iCol, (byte) errorCode, iXFIndex);
  }

  public void SetErrorValue(int iRow, int iCol, byte errorCode, int iXFIndex)
  {
    BoolErrRecord record = (BoolErrRecord) this.m_recordExtractor.GetRecord(517);
    record.IsErrorCode = true;
    record.BoolOrError = errorCode;
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    this.SetCellRecord(RangeImpl.GetCellIndex(iCol, iRow), (ICellPositionFormat) record);
  }

  public void SetFormula(int iRow, int iCol, string strValue, int iXFIndex)
  {
    this.SetFormula(iRow, iCol, strValue, iXFIndex, false);
  }

  public void SetFormula(
    int iRow,
    int iCol,
    string strValue,
    int iXFIndex,
    bool isR1C1,
    NumberFormatInfo formatInfo)
  {
    this.SetFormula(iRow, iCol, strValue, iXFIndex, isR1C1, true, formatInfo);
  }

  public void SetFormula(int iRow, int iCol, string strValue, int iXFIndex, bool isR1C1)
  {
    this.SetFormula(iRow, iCol, strValue, iXFIndex, isR1C1, true, (NumberFormatInfo) null);
  }

  public void SetFormula(
    int iRow,
    int iCol,
    string strValue,
    int iXFIndex,
    bool isR1C1,
    bool bParse,
    NumberFormatInfo formatInfo)
  {
    FormulaRecord record = (FormulaRecord) this.m_recordExtractor.GetRecord(6);
    strValue = strValue.Substring(1);
    FormulaUtil formulaUtil = this.m_worksheet.ParentWorkbook.FormulaUtil;
    if (bParse)
    {
      formulaUtil.NumberFormat = NumberFormatInfo.InvariantInfo;
      record.ParsedExpression = formulaUtil.ParseString(strValue, (IWorksheet) this.Sheet, (Dictionary<string, string>) null, iRow - 1, iCol - 1, isR1C1);
      formulaUtil.NumberFormat = (NumberFormatInfo) null;
    }
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    this.SetCellRecord(RangeImpl.GetCellIndex(iCol, iRow), (ICellPositionFormat) record);
  }

  public void SetBlank(int iRow, int iCol, int iXFIndex)
  {
    BlankRecord record = (BlankRecord) this.m_recordExtractor.GetRecord(513);
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    this.SetCellRecord(iRow, iCol, (ICellPositionFormat) record);
  }

  public void SetRTF(int iRow, int iCol, int iXFIndex, TextWithFormat rtf)
  {
    if (rtf == null)
      throw new ArgumentNullException(nameof (rtf));
    LabelSSTRecord record = (LabelSSTRecord) this.m_recordExtractor.GetRecord(253);
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    SortedList<int, int> innerFormattingRuns = rtf.InnerFormattingRuns;
    if (innerFormattingRuns != null && innerFormattingRuns.Count <= 1)
      rtf.FormattingRuns.Clear();
    record.SSTIndex = this.m_book.InnerSST.AddIncrease((object) rtf);
    this.SetCellRecord(iRow, iCol, (ICellPositionFormat) record);
  }

  public void SetSingleStringValue(int iRow, int iCol, int iXFIndex, int iSSTIndex)
  {
    LabelSSTRecord record = (LabelSSTRecord) this.m_recordExtractor.GetRecord(253);
    record.Row = iRow - 1;
    record.Column = iCol - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    record.SSTIndex = iSSTIndex;
    this.m_book.InnerSST.AddIncrease(iSSTIndex);
    this.Add((ICellPositionFormat) record);
  }

  internal void SetNonSSTString(int row, int column, int iXFIndex, string strValue)
  {
    LabelRecord record = (LabelRecord) this.m_recordExtractor.GetRecord(516);
    record.Row = row - 1;
    record.Column = column - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    record.Label = strValue;
    this.SetCellRecord(row, column, (ICellPositionFormat) record);
  }

  public void FreeRange(int iRow, int iColumn)
  {
    this.SetRange(iRow, iColumn, (RangeImpl) null);
    ICellPositionFormat cellRecord = this.GetCellRecord(iRow, iColumn);
    if (cellRecord == null || cellRecord.TypeCode != TBIFFRecord.Blank)
      return;
    int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
    int num = this.m_book.DefaultXFIndex;
    IOutline rowOutline = WorksheetHelper.GetRowOutline(this.Sheet, iRow);
    ColumnInfoRecord columnInfoRecord = this.Sheet is WorksheetImpl sheet ? sheet.ColumnInformation[iColumn] : (ColumnInfoRecord) null;
    if (columnInfoRecord != null)
      num = (int) columnInfoRecord.ExtendedFormatIndex;
    if (rowOutline != null)
      num = (int) rowOutline.ExtendedFormatIndex;
    if (extendedFormatIndex != num)
      return;
    this.Remove(iRow, iColumn);
  }

  public void ClearData()
  {
    if (this.m_dicRecords == null)
      return;
    int index = 0;
    for (int rowCount = this.m_dicRecords.RowCount; index < rowCount; ++index)
    {
      RowStorage row = this.m_dicRecords.Rows[index];
      if (row != null)
      {
        row.ClearData();
        row.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      }
    }
  }

  [CLSCompliant(false)]
  public void SetArrayFormula(ArrayRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.m_dicRecords.Rows[record.FirstRow].SetArrayRecord(record.FirstColumn, record, this.Application.RowStorageAllocationBlockSize);
  }

  [CLSCompliant(false)]
  public ArrayRecord GetArrayRecord(int iRow, int iColumn)
  {
    --iRow;
    --iColumn;
    RowStorage row = this.m_dicRecords.Rows[iRow];
    if (row == null)
      return (ArrayRecord) null;
    FormulaRecord record = row.HasFormulaRecord(iColumn) ? row.GetRecord(iColumn, this.Application.RowStorageAllocationBlockSize) as FormulaRecord : (FormulaRecord) null;
    if (record == null)
      return (ArrayRecord) null;
    Ptg[] parsedExpression = record.ParsedExpression;
    if (parsedExpression.Length != 1)
      return (ArrayRecord) null;
    Ptg ptg = parsedExpression[0];
    if (ptg.TokenCode != FormulaToken.tExp)
      return (ArrayRecord) null;
    ControlPtg controlPtg = ptg as ControlPtg;
    if (controlPtg.RowIndex != iRow)
      row = this.m_dicRecords.Rows[controlPtg.RowIndex];
    return row.GetArrayRecord(controlPtg.ColumnIndex);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.m_dicRecords.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public void RemoveLastColumn(int iColumnIndex)
  {
    this.m_dicRecords.RemoveLastColumn(iColumnIndex - 1);
  }

  public void RemoveRow(int iRowIndex) => this.m_dicRecords.RemoveRow(iRowIndex - 1);

  public void UpdateNameIndexes(WorkbookImpl book, int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    this.m_dicRecords.UpdateNameIndexes(book, arrNewIndex);
  }

  public void UpdateNameIndexes(WorkbookImpl book, IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    this.m_dicRecords.UpdateNameIndexes(book, dicNewIndex);
  }

  [CLSCompliant(false)]
  public void ReplaceSharedFormula() => this.m_dicRecords.ReplaceSharedFormula(this.m_book);

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    this.m_dicRecords.UpdateStringIndexes(arrNewIndexes);
  }

  public List<long> Find(IRange range, string findValue, OfficeFindType flags, bool bIsFindFirst)
  {
    return this.m_dicRecords.Find(range, findValue, flags, bIsFindFirst, this.m_book);
  }

  public List<long> Find(
    IRange range,
    string findValue,
    OfficeFindType flags,
    OfficeFindOptions findOptions,
    bool bIsFindFirst)
  {
    return this.m_dicRecords.Find(range, findValue, flags, findOptions, bIsFindFirst, this.m_book);
  }

  public List<long> Find(IRange range, double findValue, OfficeFindType flags, bool bIsFindFirst)
  {
    return this.m_dicRecords.Find(range, findValue, flags, bIsFindFirst, this.m_book);
  }

  public List<long> Find(IRange range, byte findValue, bool bIsError, bool bIsFindFirst)
  {
    return this.m_dicRecords.Find(range, findValue, bIsError, bIsFindFirst, this.m_book);
  }

  public List<long> Find(Dictionary<int, object> dictIndexes)
  {
    return this.m_dicRecords.Find(dictIndexes);
  }

  public RecordTable CacheAndRemove(
    RangeImpl sourceRange,
    int iDeltaRow,
    int iDeltaColumn,
    ref int iMaxRow,
    ref int iMaxColumn)
  {
    RecordTable recordTable = this.m_dicRecords.CacheAndRemove(sourceRange.GetRectangles()[0], iDeltaRow, iDeltaColumn, ref iMaxRow, ref iMaxColumn);
    ++iMaxRow;
    ++iMaxColumn;
    return recordTable;
  }

  public void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats)
  {
    this.m_dicRecords.UpdateExtendedFormatIndex(dictFormats);
  }

  public void UpdateExtendedFormatIndex(int[] arrFormats)
  {
    this.m_dicRecords.UpdateExtendedFormatIndex(arrFormats);
  }

  public void UpdateExtendedFormatIndex(int maxCount)
  {
    this.m_dicRecords.UpdateExtendedFormatIndex(maxCount);
  }

  public void SetCellStyle(int iRow, int iColumn, int iXFIndex)
  {
    this.Table.GetOrCreateRow(iRow - 1, this.AppImplementation.StandardHeightInRowUnits, true, this.m_worksheet.Version).SetCellStyle(iRow - 1, iColumn - 1, iXFIndex, this.Application.RowStorageAllocationBlockSize);
    if (iXFIndex == this.m_book.DefaultXFIndex)
      return;
    WorksheetHelper.AccessColumn(this.m_worksheet, iColumn);
    WorksheetHelper.AccessRow(this.m_worksheet, iRow);
  }

  public void SetCellStyle(int iRow, int index)
  {
    RowStorage row = this.Table.GetOrCreateRow(iRow - 1, this.AppImplementation.StandardHeightInRowUnits, true, this.m_worksheet.Version);
    row.ExtendedFormatIndex = (ushort) index;
    row.IsFormatted = true;
  }

  public void ReAddAllStrings()
  {
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
    {
      ApplicationImpl appImplementation = this.m_book.AppImplementation;
      this.m_dicRecords.GetOrCreateRow(firstRow - 1, appImplementation.StandardHeightInRowUnits, false, OfficeVersion.Excel97to2003)?.ReAddAllStrings(this.m_book.InnerSST);
    }
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    this.m_dicRecords.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    this.m_dicRecords.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  private void InsertIntoDefaultRows(int iRowIndex, int iRowCount)
  {
    this.Table.InsertIntoDefaultRows(iRowIndex, iRowCount);
  }

  private string GetFormulaErrorBoolText(FormulaRecord formula)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    if (!double.IsNaN(formula.Value))
      throw new ArgumentException("Formula record doesnot support error or bool.");
    if (formula.IsBool)
      return formula.BooleanValue.ToString().ToUpper();
    string str;
    return !FormulaUtil.ErrorCodeToName.TryGetValue((int) formula.ErrorValue, out str) ? "#N/A" : str;
  }

  private void CreateRangesCollection()
  {
    this.m_colRanges = new SFTable(this.m_book.MaxRowCount, this.m_book.MaxColumnCount);
  }

  private void UpdateSheetReferences(
    FormulaRecord formula,
    IDictionary dicSheetNames,
    WorkbookImpl book)
  {
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    Ptg[] parsedExpression = formula.ParsedExpression;
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      Ptg ptg = parsedExpression[index];
      if (ptg is ISheetReference)
      {
        ISheetReference sheetReference = (ISheetReference) ptg;
        ushort refIndex = sheetReference.RefIndex;
        string str = book.GetSheetNameByReference((int) refIndex);
        if (dicSheetNames != null && dicSheetNames.Contains((object) str))
          str = (string) dicSheetNames[(object) str];
        int num = this.m_book.AddSheetReference(str);
        sheetReference.RefIndex = (ushort) num;
      }
    }
  }

  private void CopyStrings(IDictionary dicSourceStrings)
  {
  }

  private FormatImpl GetFormat(long cellIndex)
  {
    ICellPositionFormat cellRecord = this.GetCellRecord(cellIndex);
    return cellRecord != null ? this.m_book.InnerFormats[this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex].NumberFormatIndex] : (FormatImpl) null;
  }

  private IOfficeFont GetFont(long cellIndex)
  {
    ICellPositionFormat cellRecord = this.GetCellRecord(cellIndex);
    return cellRecord != null ? this.m_book.InnerFonts[this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex].FontIndex] : (IOfficeFont) null;
  }

  private int GetXFIndex(int iOldIndex, IDictionary dicXFIndexes, OfficeCopyRangeOptions options)
  {
    if ((options & OfficeCopyRangeOptions.CopyStyles) == OfficeCopyRangeOptions.None)
      iOldIndex = this.m_book.DefaultXFIndex;
    else if (dicXFIndexes != null && dicXFIndexes.Contains((object) iOldIndex))
      iOldIndex = (int) dicXFIndexes[(object) iOldIndex];
    return iOldIndex;
  }

  internal void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes, IncreaseIndex method)
  {
    this.m_dicRecords.UpdateLabelSSTIndexes(dictUpdatedIndexes, method);
  }

  private void FillRichText(RichTextString richText, int sstIndex)
  {
    object commentText = (object) this.m_book.InnerSST[sstIndex];
    if (commentText is TextWithFormat textWithFormat)
    {
      richText.SetTextObject(textWithFormat.TypedClone());
    }
    else
    {
      TextWithFormat textObject = richText.TextObject;
      if (textObject != null)
      {
        textObject.ClearFormatting();
        textObject.Text = commentText as string;
      }
      else
        richText.SetTextObject((TextWithFormat) commentText);
    }
  }

  internal WorksheetImpl.TRangeValueType GetCellType(int row, int column)
  {
    RowStorage row1 = this.m_dicRecords.Rows[row - 1];
    return row1 == null ? WorksheetImpl.TRangeValueType.Blank : row1.GetCellType(column - 1, false);
  }

  protected override void OnDispose()
  {
    if (!this.m_bIsDisposed)
    {
      if (this.m_dicRecords != null)
      {
        this.m_dicRecords.Dispose();
        this.m_dicRecords = (RecordTable) null;
      }
      if (this.m_colRanges != null)
      {
        this.m_colRanges.Clear();
        this.m_colRanges = (SFTable) null;
      }
      this.m_book = (WorkbookImpl) null;
      this.m_worksheet = (IInternalWorksheet) null;
    }
    base.OnDispose();
  }

  public int FindRecord(TBIFFRecord recordType, int iRow, int iCol, int iLastCol)
  {
    RowStorage row = this.m_dicRecords.Rows[iRow - 1];
    return row == null ? iLastCol + 1 : row.FindRecord(recordType, iCol - 1, iLastCol - 1) + 1;
  }
}
