// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartShtpropsRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartShtprops)]
internal class ChartShtpropsRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 4;
  public const int DEF_MIN_RECORD_SIZE = 3;
  [BiffRecordPos(0, 2)]
  private ushort m_usFlags;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bManSerAlloc;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bPlotVisOnly = true;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bNotSizeWith = true;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bManPlotArea = true;
  [BiffRecordPos(0, 4, TFieldType.Bit)]
  private bool m_bAlwaysAutoPlotArea;
  [BiffRecordPos(2, 1)]
  private byte m_plotBlank;
  [BiffRecordPos(3, 1)]
  private byte m_notUsed;

  public ushort Flags => this.m_usFlags;

  public bool IsManSerAlloc
  {
    get => this.m_bManSerAlloc;
    set => this.m_bManSerAlloc = value;
  }

  public bool IsPlotVisOnly
  {
    get => this.m_bPlotVisOnly;
    set => this.m_bPlotVisOnly = value;
  }

  public bool IsNotSizeWith
  {
    get => this.m_bNotSizeWith;
    set => this.m_bNotSizeWith = value;
  }

  public bool IsManPlotArea
  {
    get => this.m_bManPlotArea;
    set => this.m_bManPlotArea = value;
  }

  public bool IsAlwaysAutoPlotArea
  {
    get => this.m_bAlwaysAutoPlotArea;
    set => this.m_bAlwaysAutoPlotArea = value;
  }

  public OfficeChartPlotEmpty PlotBlank
  {
    get => (OfficeChartPlotEmpty) this.m_plotBlank;
    set => this.m_plotBlank = (byte) value;
  }

  public byte Reserved
  {
    get => this.m_notUsed;
    set => this.m_notUsed = value;
  }

  public override int MinimumRecordSize => 3;

  public override int MaximumRecordSize => 4;

  public ChartShtpropsRecord()
  {
  }

  public ChartShtpropsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartShtpropsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usFlags = provider.ReadUInt16(iOffset);
    this.m_bManSerAlloc = provider.ReadBit(iOffset, 0);
    this.m_bPlotVisOnly = provider.ReadBit(iOffset, 1);
    this.m_bNotSizeWith = provider.ReadBit(iOffset, 2);
    this.m_bManPlotArea = provider.ReadBit(iOffset, 3);
    this.m_bAlwaysAutoPlotArea = provider.ReadBit(iOffset, 4);
    this.m_plotBlank = provider.ReadByte(iOffset + 2);
    if (iLength <= 3)
      return;
    this.m_notUsed = provider.ReadByte(iOffset + 3);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFlags);
    provider.WriteBit(iOffset, this.m_bManSerAlloc, 0);
    provider.WriteBit(iOffset, this.m_bPlotVisOnly, 1);
    provider.WriteBit(iOffset, this.m_bNotSizeWith, 2);
    provider.WriteBit(iOffset, this.m_bManPlotArea, 3);
    provider.WriteBit(iOffset, this.m_bAlwaysAutoPlotArea, 4);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_plotBlank);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_notUsed);
    this.m_iLength = this.GetStoreSize(version);
  }

  public override int GetStoreSize(OfficeVersion version) => 4;
}
