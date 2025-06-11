// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartBoppopRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartBoppop)]
internal class ChartBoppopRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 18;
  [BiffRecordPos(0, 1)]
  private byte m_PieType;
  [BiffRecordPos(1, 1)]
  private byte m_UseDefaultSplit;
  [BiffRecordPos(1, 0, TFieldType.Bit)]
  private bool m_bUseDefaultSplit;
  [BiffRecordPos(2, 2)]
  private ushort m_usSplitType;
  [BiffRecordPos(4, 2)]
  private ushort m_usSplitPos;
  [BiffRecordPos(6, 2)]
  private ushort m_usSplitPercent;
  [BiffRecordPos(8, 2)]
  private ushort m_usPie2Size;
  [BiffRecordPos(10, 2)]
  private ushort m_usGap;
  [BiffRecordPos(12, 4, true)]
  private int m_uiNumSplitValue;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usHasShadow;
  [BiffRecordPos(16 /*0x10*/, 0, TFieldType.Bit)]
  private bool m_bHasShadow;
  private bool m_bShowLeaderLines;

  public OfficePieType PieChartType
  {
    get => (OfficePieType) this.m_PieType;
    set => this.m_PieType = (byte) value;
  }

  public bool UseDefaultSplitValue
  {
    get => this.m_bUseDefaultSplit;
    set => this.m_bUseDefaultSplit = value;
  }

  public OfficeSplitType ChartSplitType
  {
    get => (OfficeSplitType) this.m_usSplitType;
    set => this.m_usSplitType = (ushort) value;
  }

  public ushort SplitPosition
  {
    get => this.m_usSplitPos;
    set
    {
      if ((int) value == (int) this.m_usSplitPos)
        return;
      this.m_usSplitPos = value;
    }
  }

  public ushort SplitPercent
  {
    get => this.m_usSplitPercent;
    set
    {
      if ((int) value == (int) this.m_usSplitPercent)
        return;
      this.m_usSplitPercent = value;
    }
  }

  public ushort Pie2Size
  {
    get => this.m_usPie2Size;
    set
    {
      if ((int) value == (int) this.m_usPie2Size)
        return;
      this.m_usPie2Size = value;
    }
  }

  public ushort Gap
  {
    get => this.m_usGap;
    set
    {
      if ((int) value == (int) this.m_usGap)
        return;
      this.m_usGap = value;
    }
  }

  public int NumSplitValue
  {
    get => this.m_uiNumSplitValue;
    set
    {
      if (value == this.m_uiNumSplitValue)
        return;
      this.m_uiNumSplitValue = value;
    }
  }

  public bool HasShadow
  {
    get => this.m_bHasShadow;
    set => this.m_bHasShadow = value;
  }

  public bool ShowLeaderLines
  {
    get => this.m_bShowLeaderLines;
    set => this.m_bShowLeaderLines = value;
  }

  public ChartBoppopRecord()
  {
  }

  public ChartBoppopRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartBoppopRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_PieType = provider.ReadByte(iOffset);
    this.m_UseDefaultSplit = provider.ReadByte(iOffset + 1);
    this.m_bUseDefaultSplit = provider.ReadBit(iOffset + 1, 0);
    this.m_usSplitType = provider.ReadUInt16(iOffset + 2);
    this.m_usSplitPos = provider.ReadUInt16(iOffset + 4);
    this.m_usSplitPercent = provider.ReadUInt16(iOffset + 6);
    this.m_usPie2Size = provider.ReadUInt16(iOffset + 8);
    this.m_usGap = provider.ReadUInt16(iOffset + 10);
    this.m_uiNumSplitValue = provider.ReadInt32(iOffset + 12);
    this.m_usHasShadow = provider.ReadUInt16(iOffset + 16 /*0x10*/);
    this.m_bHasShadow = provider.ReadBit(iOffset + 16 /*0x10*/, 0);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usHasShadow &= (ushort) 1;
    this.m_UseDefaultSplit &= (byte) 1;
    provider.WriteByte(iOffset, this.m_PieType);
    provider.WriteByte(iOffset + 1, this.m_UseDefaultSplit);
    provider.WriteBit(iOffset + 1, this.m_bUseDefaultSplit, 0);
    provider.WriteUInt16(iOffset + 2, this.m_usSplitType);
    provider.WriteUInt16(iOffset + 4, this.m_usSplitPos);
    provider.WriteUInt16(iOffset + 6, this.m_usSplitPercent);
    provider.WriteUInt16(iOffset + 8, this.m_usPie2Size);
    provider.WriteUInt16(iOffset + 10, this.m_usGap);
    provider.WriteInt32(iOffset + 12, this.m_uiNumSplitValue);
    provider.WriteUInt16(iOffset + 16 /*0x10*/, this.m_usHasShadow);
    provider.WriteBit(iOffset + 16 /*0x10*/, this.m_bHasShadow, 0);
    this.m_iLength = 18;
  }

  public override int GetStoreSize(OfficeVersion version) => 18;
}
