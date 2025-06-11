// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartBoppopRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartBoppop)]
public class ChartBoppopRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 22;
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
  [BiffRecordPos(12, 8, true)]
  private long m_uiNumSplitValue;
  [BiffRecordPos(20, 0, TFieldType.Bit)]
  private bool m_bHasShadow;
  private bool m_bShowLeaderLines;

  public ExcelPieType PieChartType
  {
    get => (ExcelPieType) this.m_PieType;
    set => this.m_PieType = (byte) value;
  }

  public bool UseDefaultSplitValue
  {
    get => this.m_bUseDefaultSplit;
    set => this.m_bUseDefaultSplit = value;
  }

  public ExcelSplitType ChartSplitType
  {
    get => (ExcelSplitType) this.m_usSplitType;
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
    get => (int) this.m_uiNumSplitValue;
    set
    {
      if ((long) value == this.m_uiNumSplitValue)
        return;
      this.m_uiNumSplitValue = (long) value;
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
    ExcelVersion version)
  {
    this.m_PieType = provider.ReadByte(iOffset);
    this.m_UseDefaultSplit = provider.ReadByte(iOffset + 1);
    this.m_bUseDefaultSplit = provider.ReadBit(iOffset + 1, 0);
    this.m_usSplitType = provider.ReadUInt16(iOffset + 2);
    this.m_usSplitPos = provider.ReadUInt16(iOffset + 4);
    this.m_usSplitPercent = provider.ReadUInt16(iOffset + 6);
    this.m_usPie2Size = provider.ReadUInt16(iOffset + 8);
    this.m_usGap = provider.ReadUInt16(iOffset + 10);
    this.m_uiNumSplitValue = provider.ReadInt64(iOffset + 12);
    this.m_bHasShadow = provider.ReadBit(iOffset + 16 /*0x10*/, 0);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_UseDefaultSplit &= (byte) 1;
    provider.WriteByte(iOffset, this.m_PieType);
    provider.WriteByte(iOffset + 1, this.m_UseDefaultSplit);
    provider.WriteBit(iOffset + 1, this.m_bUseDefaultSplit, 0);
    provider.WriteUInt16(iOffset + 2, this.m_usSplitType);
    provider.WriteUInt16(iOffset + 4, this.m_usSplitPos);
    provider.WriteUInt16(iOffset + 6, this.m_usSplitPercent);
    provider.WriteUInt16(iOffset + 8, this.m_usPie2Size);
    provider.WriteUInt16(iOffset + 10, this.m_usGap);
    provider.WriteInt64(iOffset + 12, this.m_uiNumSplitValue);
    provider.WriteBit(iOffset + 16 /*0x10*/, this.m_bHasShadow, 0);
    this.m_iLength = 22;
  }

  public override int GetStoreSize(ExcelVersion version) => 22;
}
