// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartRadarRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartRadar)]
public class ChartRadarRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bRadarAxisLabel;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bHasShadow;
  [BiffRecordPos(2, 2)]
  private ushort m_usReserved;

  public ushort Options => this.m_usOptions;

  public bool IsRadarAxisLabel
  {
    get => this.m_bRadarAxisLabel;
    set => this.m_bRadarAxisLabel = value;
  }

  public bool HasShadow
  {
    get => this.m_bHasShadow;
    set => this.m_bHasShadow = value;
  }

  public ushort Reserved
  {
    get => this.m_usReserved;
    set => this.m_usReserved = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  public ChartRadarRecord()
  {
  }

  public ChartRadarRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartRadarRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bRadarAxisLabel = provider.ReadBit(iOffset, 0);
    this.m_bHasShadow = provider.ReadBit(iOffset, 1);
    this.m_usReserved = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bRadarAxisLabel, 0);
    provider.WriteBit(iOffset, this.m_bHasShadow, 1);
    provider.WriteUInt16(iOffset + 2, this.m_usReserved);
    this.m_iLength = 4;
  }
}
