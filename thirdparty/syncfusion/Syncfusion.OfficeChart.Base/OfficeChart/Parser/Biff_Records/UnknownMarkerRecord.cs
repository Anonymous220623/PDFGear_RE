// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.UnknownMarkerRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.UnkMarker)]
[CLSCompliant(false)]
internal class UnknownMarkerRecord : BiffRecordRaw
{
  private const ushort DEF_RESERVED1 = 55;
  [BiffRecordPos(0, 2)]
  private ushort m_usReserved0;
  [BiffRecordPos(2, 2)]
  private ushort m_usReserved1 = 55;
  [BiffRecordPos(4, 2)]
  private ushort m_usReserved2;

  public ushort Reserved0
  {
    get => this.m_usReserved0;
    set => this.m_usReserved0 = value;
  }

  public ushort Reserved1
  {
    get => this.m_usReserved1;
    set => this.m_usReserved1 = value;
  }

  public ushort Reserved2
  {
    get => this.m_usReserved2;
    set => this.m_usReserved2 = value;
  }

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  public UnknownMarkerRecord()
  {
  }

  public UnknownMarkerRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public UnknownMarkerRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usReserved0 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usReserved1 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usReserved2 = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usReserved0 = (ushort) 0;
    this.m_usReserved1 = (ushort) 55;
    this.m_usReserved2 = (ushort) 0;
    provider.WriteUInt16(iOffset, this.m_usReserved0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usReserved1);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usReserved2);
  }
}
