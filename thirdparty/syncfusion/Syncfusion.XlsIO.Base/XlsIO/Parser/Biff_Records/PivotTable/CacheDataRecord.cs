// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.CacheDataRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CacheData)]
public class CacheDataRecord : BiffRecordRaw
{
  private const int DEF_USERNAME_OFFSET = 20;
  [BiffRecordPos(0, 4, true)]
  private int m_iRecordsNumber;
  [BiffRecordPos(4, 2)]
  private ushort m_usStreamId;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bSaveData;
  [BiffRecordPos(6, 1, TFieldType.Bit)]
  private bool m_bInvalid;
  [BiffRecordPos(6, 2, TFieldType.Bit)]
  private bool m_bRefreshOnLoad;
  [BiffRecordPos(6, 3, TFieldType.Bit)]
  private bool m_bOptimizeCache;
  [BiffRecordPos(6, 4, TFieldType.Bit)]
  private bool m_bBackgroundQuery;
  [BiffRecordPos(6, 5, TFieldType.Bit)]
  private bool m_bEnableRefresh;
  [BiffRecordPos(8, 2)]
  private ushort m_usRecordsInBlock;
  [BiffRecordPos(10, 2)]
  private ushort m_usBaseFieldsCount;
  [BiffRecordPos(12, 2)]
  private ushort m_usFieldsNumber;
  [BiffRecordPos(14, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usSourceType;
  [BiffRecordPos(18, 2)]
  private ushort m_usUserNameSize;
  private string m_strUserName;
  private bool m_bUserName16Bit;

  public CacheDataRecord()
  {
  }

  public CacheDataRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CacheDataRecord(int iReserve)
    : base(iReserve)
  {
  }

  public int RecordsNumber
  {
    get => this.m_iRecordsNumber;
    set => this.m_iRecordsNumber = value;
  }

  public ushort StreamId
  {
    get => this.m_usStreamId;
    set => this.m_usStreamId = value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsSaveData
  {
    get => this.m_bSaveData;
    set => this.m_bSaveData = value;
  }

  public bool IsInvalid
  {
    get => this.m_bInvalid;
    set => this.m_bInvalid = value;
  }

  public bool IsRefreshOnLoad
  {
    get => this.m_bRefreshOnLoad;
    set => this.m_bRefreshOnLoad = value;
  }

  public bool IsOptimizeCache
  {
    get => this.m_bOptimizeCache;
    set => this.m_bOptimizeCache = value;
  }

  public bool IsBackgroundQuery
  {
    get => this.m_bBackgroundQuery;
    set => this.m_bBackgroundQuery = value;
  }

  public bool IsEnableRefresh
  {
    get => this.m_bEnableRefresh;
    set => this.m_bEnableRefresh = value;
  }

  public ushort RecordsInBlock
  {
    get => this.m_usRecordsInBlock;
    set => this.m_usRecordsInBlock = value;
  }

  public ushort BaseFieldsCount
  {
    get => this.m_usBaseFieldsCount;
    set => this.m_usBaseFieldsCount = value;
  }

  public ushort FieldsNumber
  {
    get => this.m_usFieldsNumber;
    set => this.m_usFieldsNumber = value;
  }

  public ushort Reserved => this.m_usReserved;

  public ExcelDataSourceType SourceType
  {
    get => (ExcelDataSourceType) this.m_usSourceType;
    set => this.m_usSourceType = (ushort) value;
  }

  public ushort UserNameSize => this.m_usUserNameSize;

  public string UserName
  {
    get => this.m_strUserName;
    set
    {
      this.m_strUserName = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usUserNameSize = (ushort) this.m_strUserName.Length;
      this.m_bUserName16Bit = !BiffRecordRawWithArray.IsAsciiString(value);
    }
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iRecordsNumber = provider.ReadInt32(iOffset);
    this.m_usStreamId = provider.ReadUInt16(iOffset + 4);
    this.m_usOptions = provider.ReadUInt16(iOffset + 6);
    this.m_bSaveData = provider.ReadBit(iOffset + 6, 0);
    this.m_bInvalid = provider.ReadBit(iOffset + 6, 1);
    this.m_bRefreshOnLoad = provider.ReadBit(iOffset + 6, 2);
    this.m_bOptimizeCache = provider.ReadBit(iOffset + 6, 3);
    this.m_bBackgroundQuery = provider.ReadBit(iOffset + 6, 4);
    this.m_bEnableRefresh = provider.ReadBit(iOffset + 6, 5);
    this.m_usRecordsInBlock = provider.ReadUInt16(iOffset + 8);
    this.m_usBaseFieldsCount = provider.ReadUInt16(iOffset + 10);
    this.m_usFieldsNumber = provider.ReadUInt16(iOffset + 12);
    this.m_usReserved = provider.ReadUInt16(iOffset + 14);
    this.m_usSourceType = provider.ReadUInt16(iOffset + 16 /*0x10*/);
    this.m_usUserNameSize = provider.ReadUInt16(iOffset + 18);
    iOffset += 20;
    int iBytesInString;
    this.m_strUserName = provider.ReadString(iOffset, (int) this.m_usUserNameSize, out iBytesInString, false);
    this.m_bUserName16Bit = this.m_strUserName.Length * 2 + 3 >= iBytesInString;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    int num = iOffset;
    provider.WriteInt32(iOffset, this.m_iRecordsNumber);
    provider.WriteUInt16(iOffset + 4, this.m_usStreamId);
    provider.WriteUInt16(iOffset + 6, this.m_usOptions);
    provider.WriteBit(iOffset + 6, this.m_bSaveData, 0);
    provider.WriteBit(iOffset + 6, this.m_bInvalid, 1);
    provider.WriteBit(iOffset + 6, this.m_bRefreshOnLoad, 2);
    provider.WriteBit(iOffset + 6, this.m_bOptimizeCache, 3);
    provider.WriteBit(iOffset + 6, this.m_bBackgroundQuery, 4);
    provider.WriteBit(iOffset + 6, this.m_bEnableRefresh, 5);
    provider.WriteUInt16(iOffset + 8, this.m_usRecordsInBlock);
    provider.WriteUInt16(iOffset + 10, this.m_usBaseFieldsCount);
    provider.WriteUInt16(iOffset + 12, this.m_usFieldsNumber);
    provider.WriteUInt16(iOffset + 14, this.m_usReserved);
    provider.WriteUInt16(iOffset + 16 /*0x10*/, this.m_usSourceType);
    provider.WriteUInt16(iOffset + 18, this.m_usUserNameSize);
    iOffset += 20;
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strUserName, this.m_bUserName16Bit);
    this.m_iLength = iOffset - num;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int length = this.m_strUserName.Length;
    int storeSize = 20 + length + 1;
    if (this.m_bUserName16Bit)
      storeSize += length;
    return storeSize;
  }

  public void FillCache(IRange dataRange)
  {
    if (dataRange == null)
      throw new ArgumentNullException(nameof (dataRange));
  }
}
