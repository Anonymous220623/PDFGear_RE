// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAxisRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAxis)]
[CLSCompliant(false)]
public class ChartAxisRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 18;
  [BiffRecordPos(0, 2)]
  private ushort m_usAxisType;
  [BiffRecordPos(2, 4, true)]
  private int m_Reserved0;
  [BiffRecordPos(6, 4, true)]
  private int m_Reserved1;
  [BiffRecordPos(10, 4, true)]
  private int m_Reserved2;
  [BiffRecordPos(14, 4, true)]
  private int m_Reserved3;

  public ChartAxisRecord.ChartAxisType AxisType
  {
    get => (ChartAxisRecord.ChartAxisType) this.m_usAxisType;
    set => this.m_usAxisType = (ushort) value;
  }

  public int Reserved0 => this.m_Reserved0;

  public int Reserved1 => this.m_Reserved1;

  public int Reserved2 => this.m_Reserved2;

  public int Reserved3 => this.m_Reserved3;

  public ChartAxisRecord()
  {
  }

  public ChartAxisRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usAxisType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_Reserved0 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_Reserved1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_Reserved2 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_Reserved3 = provider.ReadInt32(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_Reserved0 = this.m_Reserved1 = this.m_Reserved2 = this.m_Reserved3 = 0;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usAxisType);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_Reserved0);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_Reserved1);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_Reserved2);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_Reserved3);
  }

  public override int GetStoreSize(ExcelVersion version) => 18;

  public enum ChartAxisType
  {
    CategoryAxis,
    ValueAxis,
    SeriesAxis,
  }
}
