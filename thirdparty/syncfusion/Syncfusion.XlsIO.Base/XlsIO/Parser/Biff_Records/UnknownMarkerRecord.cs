// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.UnknownMarkerRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.UnkMarker)]
public class UnknownMarkerRecord : BiffRecordRaw
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
    ExcelVersion version)
  {
    this.m_usReserved0 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usReserved1 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usReserved2 = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
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
