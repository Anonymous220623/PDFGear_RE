// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartLineFormatRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartLineFormat)]
internal class ChartLineFormatRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(0, 4, true)]
  private int m_rgbColor;
  [BiffRecordPos(4, 2)]
  private ushort m_usLinePattern;
  [BiffRecordPos(6, 2)]
  private ushort m_usLineWeight;
  [BiffRecordPos(8, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(8, 0, TFieldType.Bit)]
  private bool m_bAutoFormat = true;
  [BiffRecordPos(8, 2, TFieldType.Bit)]
  private bool m_bDrawTickLabels;
  [BiffRecordPos(8, 3, TFieldType.Bit)]
  private bool m_bIsAutoLineColor = true;
  [BiffRecordPos(10, 2)]
  private ushort m_usColorIndex;

  public int LineColor
  {
    get => this.m_rgbColor;
    set
    {
      if (value == this.m_rgbColor)
        return;
      this.m_rgbColor = value;
    }
  }

  public OfficeChartLinePattern LinePattern
  {
    get => (OfficeChartLinePattern) this.m_usLinePattern;
    set => this.m_usLinePattern = (ushort) value;
  }

  public OfficeChartLineWeight LineWeight
  {
    get => (OfficeChartLineWeight) this.m_usLineWeight;
    set => this.m_usLineWeight = (ushort) value;
  }

  public ushort Options => this.m_usOptions;

  public bool AutoFormat
  {
    get => this.m_bAutoFormat;
    set => this.m_bAutoFormat = value;
  }

  public bool DrawTickLabels
  {
    get => this.m_bDrawTickLabels;
    set => this.m_bDrawTickLabels = value;
  }

  public bool IsAutoLineColor
  {
    get => this.m_bIsAutoLineColor;
    set => this.m_bIsAutoLineColor = value;
  }

  public ushort ColorIndex
  {
    get => this.m_usColorIndex;
    set => this.m_usColorIndex = value;
  }

  public override int MinimumRecordSize => 12;

  public override int MaximumRecordSize => 12;

  public ChartLineFormatRecord()
  {
  }

  public ChartLineFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartLineFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_rgbColor = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usLinePattern = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLineWeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bAutoFormat = provider.ReadBit(iOffset, 0);
    this.m_bDrawTickLabels = provider.ReadBit(iOffset, 2);
    this.m_bIsAutoLineColor = provider.ReadBit(iOffset, 3);
    iOffset += 2;
    this.m_usColorIndex = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteInt32(iOffset, this.m_rgbColor);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usLinePattern);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLineWeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bAutoFormat, 0);
    provider.WriteBit(iOffset, this.m_bDrawTickLabels, 2);
    provider.WriteBit(iOffset, this.m_bIsAutoLineColor, 3);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usColorIndex);
    this.m_iLength = this.GetStoreSize(version);
  }

  public override int GetStoreSize(OfficeVersion version) => 12;
}
