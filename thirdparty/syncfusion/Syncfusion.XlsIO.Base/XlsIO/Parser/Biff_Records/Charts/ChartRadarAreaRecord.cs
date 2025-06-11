// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartRadarAreaRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartRadarArea)]
[CLSCompliant(false)]
public class ChartRadarAreaRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bRadarAxisLabel;
  [BiffRecordPos(2, 2)]
  private ushort m_usReserved;

  public ushort Options => this.m_usOptions;

  public bool IsRadarAxisLabel
  {
    get => this.m_bRadarAxisLabel;
    set => this.m_bRadarAxisLabel = value;
  }

  public ushort Resereved
  {
    get => this.m_usReserved;
    set => this.m_usReserved = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  public ChartRadarAreaRecord()
  {
  }

  public ChartRadarAreaRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartRadarAreaRecord(int iReserve)
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
    this.m_usReserved = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bRadarAxisLabel, 0);
    provider.WriteUInt16(iOffset + 2, this.m_usReserved);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
