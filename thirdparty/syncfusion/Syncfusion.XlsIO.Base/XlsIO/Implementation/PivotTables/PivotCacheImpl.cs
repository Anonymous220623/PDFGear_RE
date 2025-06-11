// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCacheImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotCacheImpl : CommonObject, ICloneParent, IBiffStorage, IPivotCache
{
  private CacheDataRecord m_cacheData = (CacheDataRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CacheData);
  private CacheDataExRecord m_cacheDataEx = (CacheDataExRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CacheDataEx);
  private List<BiffRecordRaw> m_arrRecords = new List<BiffRecordRaw>();
  private MemoryStream m_preservedData = new MemoryStream();
  private PivotCacheFieldsCollection m_lstCacheFields = new PivotCacheFieldsCollection();
  private List<PivotIndexListRecord> m_lstPivotIndexes = new List<PivotIndexListRecord>();
  private IRange m_sourceRange;
  private string m_rangeName;
  private int m_iIndex = -1;
  private PivotCacheInfo m_info;
  private bool m_bsupportAdvancedDrill;
  private int m_iCreatedVersion;
  private int m_iMinRefreshableVersion;
  private int m_iRefreshedVersion;
  private bool m_bSupportSubQuery;
  private bool m_bUpgradeOnRefresh;
  private Dictionary<string, Stream> m_preservedElements;
  private Relation m_preservedExtenalRelation;
  private string m_relationId;
  private bool m_bHasCacheRecords;
  public RelationCollection preservedCacheRelations;
  private Stream m_consolidation;
  private double m_bMissingItemsLimit = double.MinValue;
  private bool m_bTupleCache;

  public PivotCacheImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  [CLSCompliant(false)]
  public PivotCacheImpl(
    IApplication application,
    object parent,
    BiffReader reader,
    IDecryptor decryptor,
    string streamName)
    : this(application, parent)
  {
    this.Parse(reader, decryptor, streamName);
  }

  public PivotCacheImpl(IApplication application, object parent, IRange dataRange)
    : base(application, parent)
  {
    this.m_cacheData.SourceType = ExcelDataSourceType.Worksheet;
    int row = dataRange.Row;
    int lastRow = dataRange.LastRow;
    int column1 = dataRange.Column;
    int lastColumn = dataRange.LastColumn;
    int num1 = lastColumn - column1 + 1;
    this.preservedCacheRelations = new RelationCollection();
    int column2 = column1;
    int num2 = 0;
    while (column2 <= lastColumn)
    {
      this.CreateField(dataRange.Worksheet, row, lastRow, column2);
      ++column2;
      ++num2;
    }
    this.RefreshDate = DateTime.Now;
    this.CreatedVersion = this.MinRefreshableVersion = this.RefreshedVersion = 0;
    this.m_sourceRange = dataRange;
    this.SourceType = ExcelDataSourceType.Worksheet;
  }

  private void CreateField(IWorksheet sheet, int row, int lastRow, int column)
  {
    PivotCacheFieldImpl pivotCacheFieldImpl = this.m_lstCacheFields.AddNewField(sheet[row, column].HasString ? sheet[row, column].Value : sheet[row, column].DisplayText);
    pivotCacheFieldImpl.ItemRange = sheet[row + 1, column, lastRow, column];
    pivotCacheFieldImpl.Fill(sheet, row, lastRow, column);
  }

  public int AddIndexes(byte[] indexes)
  {
    this.m_lstPivotIndexes.Add(new PivotIndexListRecord()
    {
      Indexes = indexes
    });
    return this.m_lstPivotIndexes.Count - 1;
  }

  public object GetValue(int fieldIndex, int row)
  {
    int index = (int) this.m_lstPivotIndexes[row].Indexes[fieldIndex];
    return this.m_lstCacheFields[fieldIndex].GetValue(index);
  }

  public byte PutValue(int fieldIndex, object value)
  {
    return (byte) this.m_lstCacheFields[fieldIndex].AddValue(value);
  }

  private int Parse(BiffRecordRaw[] data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length - 1");
    BiffRecordRaw biffRecordRaw1 = data[iPos];
    ++iPos;
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.CacheData);
    BiffRecordRaw biffRecordRaw2 = data[iPos];
    ++iPos;
    while (biffRecordRaw2.TypeCode != TBIFFRecord.EOF)
    {
      this.m_arrRecords.Add(biffRecordRaw2);
      biffRecordRaw2 = data[iPos];
      ++iPos;
    }
    return iPos;
  }

  private void Parse(BiffReader reader, IDecryptor decryptor, string streamCode)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.BaseStream.Length == 0L)
    {
      int result = 0;
      int.TryParse(streamCode, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result);
      ushort num = 213;
      if (result == (int) num)
      {
        this.m_cacheData.StreamId = num;
        return;
      }
    }
    if (reader.BaseStream.Length <= 0L)
      return;
    TBIFFRecord recordCode = reader.PeekRecordType();
    if (recordCode != TBIFFRecord.CacheData)
      throw new UnexpectedRecordException(recordCode);
    this.m_cacheData = (CacheDataRecord) reader.GetRecord(decryptor);
    BiffRecordRaw biffRecordRaw = (BiffRecordRaw) this.m_cacheData;
    while (biffRecordRaw.TypeCode != TBIFFRecord.EOF)
    {
      biffRecordRaw = reader.GetRecord(decryptor);
      this.m_arrRecords.Add(biffRecordRaw);
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_cacheData);
    records.AddList((IList) this.m_arrRecords);
  }

  public void Serialize(Stream stream, IEncryptor encryptor)
  {
    if (this.m_preservedData != null && this.m_preservedData.Length > 0L)
    {
      this.m_preservedData.WriteTo(stream);
    }
    else
    {
      if (this.m_arrRecords == null || this.m_arrRecords.Count <= 0)
        return;
      OffsetArrayList records = new OffsetArrayList();
      this.Serialize(records);
      using (BiffWriter biffWriter = new BiffWriter(stream, false))
        biffWriter.WriteRecord(records, encryptor);
    }
  }

  public void UpdateAfterInsertRemove(
    WorksheetImpl worksheet,
    int index,
    int count,
    bool isRow,
    bool isRemove)
  {
    if (this.m_sourceRange == null || this.m_sourceRange.Worksheet != worksheet)
      return;
    if (isRemove)
      this.RemoveRowColumn(worksheet, index, count, isRow);
    else
      this.InsertRowColumn(worksheet, index, count, isRow);
  }

  private void RemoveRowColumn(WorksheetImpl worksheet, int index, int count, bool isRow)
  {
    int row = this.m_sourceRange.Row;
    int column = this.m_sourceRange.Column;
    int lastRow = this.m_sourceRange.LastRow;
    int lastColumn = this.m_sourceRange.LastColumn;
    if (PivotCacheImpl.InRange(this.m_sourceRange, worksheet, index, count, isRow))
    {
      if (isRow)
        lastRow = Math.Max(lastRow - count, index - 1);
      else
        lastColumn = Math.Max(lastColumn - count, index - 1);
    }
    else if (isRow && index <= row)
    {
      row = Math.Max(index, row - count);
      lastRow -= count;
    }
    else if (!isRow && index <= column)
    {
      column = Math.Max(index, column - count);
      lastColumn -= count;
    }
    this.m_sourceRange = worksheet[row, column, lastRow, lastColumn];
  }

  private void InsertRowColumn(WorksheetImpl worksheet, int index, int count, bool isRow)
  {
    if (!PivotCacheImpl.InRange(this.m_sourceRange, worksheet, index, count, isRow))
      return;
    int row = this.m_sourceRange.Row;
    int column = this.m_sourceRange.Column;
    int lastRow = this.m_sourceRange.LastRow + count >= worksheet.Workbook.MaxRowCount ? worksheet.Workbook.MaxRowCount : this.m_sourceRange.LastRow + count;
    int lastColumn = this.m_sourceRange.LastColumn + count >= worksheet.Workbook.MaxColumnCount ? worksheet.Workbook.MaxColumnCount : this.m_sourceRange.LastColumn + count;
    if (isRow)
      this.m_sourceRange = worksheet[row, column, lastRow, lastColumn];
    else
      this.m_sourceRange = worksheet[row, column, lastRow, lastColumn];
  }

  private static bool InRange(
    IRange sourceRange,
    WorksheetImpl worksheet,
    int index,
    int count,
    bool isRow)
  {
    return ((sourceRange.Worksheet == worksheet ? 1 : 0) & (isRow ? (sourceRange.Row >= index ? 0 : (sourceRange.LastRow >= index ? 1 : 0)) : (sourceRange.Column >= index ? 0 : (sourceRange.LastColumn >= index ? 1 : 0)))) != 0;
  }

  [CLSCompliant(false)]
  public ushort StreamId
  {
    get => this.m_cacheData.StreamId;
    set => this.m_cacheData.StreamId = value;
  }

  public ExcelDataSourceType SourceType
  {
    get => this.m_cacheData.SourceType;
    set => this.m_cacheData.SourceType = value;
  }

  public bool IsUpgradeOnRefresh
  {
    get => this.m_bUpgradeOnRefresh;
    set => this.m_bUpgradeOnRefresh = value;
  }

  public string RefreshedBy
  {
    get => this.m_cacheData.UserName;
    set => this.m_cacheData.UserName = value;
  }

  public bool IsSupportSubQuery
  {
    get => this.m_bSupportSubQuery;
    set => this.m_bSupportSubQuery = value;
  }

  public bool IsSaveData
  {
    get => this.m_cacheData.IsSaveData;
    set => this.m_cacheData.IsSaveData = value;
  }

  public bool IsOptimizedCache
  {
    get => this.m_cacheData.IsOptimizeCache;
    set => this.m_cacheData.IsOptimizeCache = value;
  }

  public bool EnableRefresh
  {
    get => this.m_cacheData.IsEnableRefresh;
    set => this.m_cacheData.IsEnableRefresh = false;
  }

  public bool IsBackgroundQuery
  {
    get => this.m_cacheData.IsBackgroundQuery;
    set => this.m_cacheData.IsBackgroundQuery = value;
  }

  public int CreatedVersion
  {
    get => this.m_iCreatedVersion;
    set => this.m_iCreatedVersion = value;
  }

  public int MinRefreshableVersion
  {
    get => this.m_iMinRefreshableVersion;
    set => this.m_iMinRefreshableVersion = value;
  }

  public int RefreshedVersion
  {
    get => this.m_iRefreshedVersion;
    set => this.m_iRefreshedVersion = value;
  }

  public bool IsInvalidData
  {
    get => this.m_cacheData.IsInvalid;
    set => this.m_cacheData.IsInvalid = value;
  }

  public bool SupportAdvancedDrill
  {
    get => this.m_bsupportAdvancedDrill;
    set => this.m_bsupportAdvancedDrill = value;
  }

  public bool IsRefreshOnLoad
  {
    get => this.m_cacheData.IsRefreshOnLoad;
    set => this.m_cacheData.IsRefreshOnLoad = value;
  }

  public DateTime RefreshDate
  {
    get => DateTime.FromOADate(this.m_cacheDataEx.RefreshDate);
    set => this.m_cacheDataEx.RefreshDate = value.ToOADate();
  }

  public int RecordCount => this.m_lstPivotIndexes.Count;

  public Stream Consolidation
  {
    get => this.m_consolidation;
    set
    {
      if (value == null)
        return;
      this.m_consolidation = value;
    }
  }

  public IRange SourceRange
  {
    get => this.m_sourceRange;
    set => this.m_sourceRange = value;
  }

  public PivotCacheFieldsCollection CacheFields => this.m_lstCacheFields;

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  internal PivotCacheInfo Info
  {
    get => this.m_info;
    set => this.m_info = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  public string RangeName
  {
    get => this.m_rangeName;
    set => this.m_rangeName = value;
  }

  public bool HasNamedRange => this.m_rangeName != null;

  public int CalculatedItemIndex => 1;

  internal Relation PreservedExtenalRelation
  {
    get => this.m_preservedExtenalRelation;
    set => this.m_preservedExtenalRelation = value;
  }

  internal string RelationId
  {
    get => this.m_relationId;
    set => this.m_relationId = value;
  }

  internal bool HasCacheRecords
  {
    get => this.m_bHasCacheRecords;
    set => this.m_bHasCacheRecords = value;
  }

  internal double MissingItemsLimit
  {
    get => this.m_bMissingItemsLimit;
    set => this.m_bMissingItemsLimit = value;
  }

  internal bool TupleCache
  {
    get => this.m_bTupleCache;
    set => this.m_bTupleCache = value;
  }

  public object Clone(object parent) => this.Clone(parent, (Dictionary<string, string>) null);

  public object Clone(object parent, Dictionary<string, string> hashNewNames)
  {
    PivotCacheImpl pivotCacheImpl = (PivotCacheImpl) this.MemberwiseClone();
    pivotCacheImpl.SetParent(parent);
    pivotCacheImpl.m_cacheData = (CacheDataRecord) CloneUtils.CloneCloneable((ICloneable) this.m_cacheData);
    pivotCacheImpl.m_cacheDataEx = (CacheDataExRecord) CloneUtils.CloneCloneable((ICloneable) this.m_cacheDataEx);
    pivotCacheImpl.m_arrRecords = CloneUtils.CloneCloneable(this.m_arrRecords);
    pivotCacheImpl.m_info = (PivotCacheInfo) CloneUtils.CloneCloneable((ICloneable) this.m_info);
    pivotCacheImpl.m_preservedData = new MemoryStream();
    this.m_preservedData.WriteTo((Stream) pivotCacheImpl.m_preservedData);
    if (this.m_sourceRange != null)
    {
      WorkbookImpl parent1 = (WorkbookImpl) pivotCacheImpl.FindParent(typeof (WorkbookImpl));
      string name = this.m_sourceRange.Worksheet.Name;
      if (parent1 == this.m_sourceRange.Worksheet.Workbook)
      {
        pivotCacheImpl.m_sourceRange = ((ICombinedRange) this.m_sourceRange).Clone(parent, hashNewNames, parent1);
      }
      else
      {
        WorksheetImpl worksheet1 = this.m_sourceRange.Worksheet as WorksheetImpl;
        IWorksheet worksheet2 = parent1.Worksheets.AddCopy((IWorksheet) worksheet1, ExcelWorksheetCopyFlags.CopyCells);
        worksheet2.Visibility = WorksheetVisibility.StrongHidden;
        pivotCacheImpl.m_sourceRange = ((ICombinedRange) this.m_sourceRange).Clone(parent, new Dictionary<string, string>()
        {
          {
            worksheet1.Name,
            worksheet2.Name
          }
        }, parent1);
      }
    }
    return (object) pivotCacheImpl;
  }

  private IRange CreateExternalRange(object parent, WorkbookImpl book, IRange sourceRange)
  {
    string fileName = ((WorkbookImpl) sourceRange.Worksheet.Workbook).FullFileName ?? "Book1.xlsx";
    int index = book.ExternWorkbooks.Add(fileName, book, sourceRange);
    return (IRange) new ExternalRange(book.ExternWorkbooks[index].Worksheets[sourceRange.Worksheet.Index], sourceRange.Row, sourceRange.Column, sourceRange.LastRow, sourceRange.LastColumn);
  }

  private IRange CreateInvalidRange(object parent, WorkbookImpl book, IRange sourceRange)
  {
    return (IRange) new InvalidRange(parent, sourceRange);
  }

  public bool ComparePreservedData(PivotCacheImpl cache)
  {
    return this.m_preservedData.Length == cache.m_preservedData.Length && BiffRecordRaw.CompareArrays(this.m_preservedData.GetBuffer(), cache.m_preservedData.GetBuffer());
  }

  public TBIFFRecord TypeCode => TBIFFRecord.Unknown;

  public int RecordCode => 0;

  public bool NeedDataArray => true;

  public long StreamPos
  {
    get => 0;
    set
    {
    }
  }

  public int GetStoreSize(ExcelVersion version) => (int) this.m_preservedData.Length;

  public int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    this.m_preservedData.WriteTo(writer.BaseStream);
    return (int) this.m_preservedData.Length;
  }
}
