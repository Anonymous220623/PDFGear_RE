// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BOFRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.BOF)]
internal class BOFRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usVersion = 1536 /*0x0600*/;
  [BiffRecordPos(2, 2)]
  private ushort m_usType = 5;
  [BiffRecordPos(4, 2)]
  private ushort m_usBuild = 6214;
  [BiffRecordPos(6, 2)]
  private ushort m_usYear = 1997;
  [BiffRecordPos(8, 4, true)]
  private int m_iHistory = 98497;
  [BiffRecordPos(12, 4, true)]
  private int m_iRVersion = 1542;
  private bool m_bIsNested;

  public ushort Version
  {
    get => this.m_usVersion;
    set => this.m_usVersion = value;
  }

  public BOFRecord.TType Type
  {
    get => (BOFRecord.TType) this.m_usType;
    set => this.m_usType = (ushort) value;
  }

  public ushort Build
  {
    get => this.m_usBuild;
    set => this.m_usBuild = value;
  }

  public ushort Year
  {
    get => this.m_usYear;
    set => this.m_usYear = value;
  }

  public int History
  {
    get => this.m_iHistory;
    set => this.m_iHistory = value;
  }

  public int RequeredVersion
  {
    get => this.m_iRVersion;
    set => this.m_iRVersion = value;
  }

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public bool IsNested
  {
    get => this.m_bIsNested;
    set => this.m_bIsNested = value;
  }

  public override bool IsAllowShortData => true;

  public override bool NeedDecoding => false;

  public BOFRecord()
  {
  }

  public BOFRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public BOFRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usVersion = provider.ReadUInt16(iOffset);
    this.m_usType = provider.ReadUInt16(iOffset + 2);
    this.m_usBuild = provider.ReadUInt16(iOffset + 4);
    this.m_usYear = provider.ReadUInt16(iOffset + 6);
    this.m_iHistory = provider.ReadInt32(iOffset + 8);
    this.m_iRVersion = provider.ReadInt32(iOffset + 12);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 16 /*0x10*/;
    provider.WriteUInt16(iOffset, this.m_usVersion);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usType);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBuild);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usYear);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iHistory);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iRVersion);
    iOffset += 4;
  }

  public enum TType
  {
    TYPE_WORKBOOK = 5,
    TYPE_VB_MODULE = 6,
    TYPE_WORKSHEET = 16, // 0x00000010
    TYPE_CHART = 32, // 0x00000020
    TYPE_EXCEL_4_MACRO = 64, // 0x00000040
    TYPE_WORKSPACE_FILE = 256, // 0x00000100
  }
}
