// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartScatterRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartScatter)]
[CLSCompliant(false)]
public class ChartScatterRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usBubleSizeRation;
  [BiffRecordPos(2, 2)]
  private ushort m_usBubleSize;
  [BiffRecordPos(4, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(4, 0, TFieldType.Bit)]
  private bool m_bBubbles;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bShowNegBubbles;
  [BiffRecordPos(4, 2, TFieldType.Bit)]
  private bool m_bHasShadow;

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  public ushort BubleSizeRation
  {
    get => this.m_usBubleSizeRation;
    set => this.m_usBubleSizeRation = value;
  }

  public ExcelBubbleSize BubleSize
  {
    get => (ExcelBubbleSize) this.m_usBubleSize;
    set => this.m_usBubleSize = (ushort) value;
  }

  public ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  public bool IsBubbles
  {
    get => this.m_bBubbles;
    set => this.m_bBubbles = value;
  }

  public bool IsShowNegBubbles
  {
    get => this.m_bShowNegBubbles;
    set => this.m_bShowNegBubbles = value;
  }

  public bool HasShadow
  {
    get => this.m_bHasShadow;
    set => this.m_bHasShadow = value;
  }

  public ChartScatterRecord()
  {
  }

  public ChartScatterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartScatterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usBubleSizeRation = provider.ReadUInt16(iOffset);
    this.m_usBubleSize = provider.ReadUInt16(iOffset + 2);
    this.m_usOptions = provider.ReadUInt16(iOffset + 4);
    this.m_bBubbles = provider.ReadBit(iOffset + 4, 0);
    this.m_bShowNegBubbles = provider.ReadBit(iOffset + 4, 1);
    this.m_bHasShadow = provider.ReadBit(iOffset + 4, 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usBubleSizeRation);
    provider.WriteUInt16(iOffset + 2, this.m_usBubleSize);
    provider.WriteUInt16(iOffset + 4, this.m_usOptions);
    provider.WriteBit(iOffset + 4, this.m_bBubbles, 0);
    provider.WriteBit(iOffset + 4, this.m_bShowNegBubbles, 1);
    provider.WriteBit(iOffset + 4, this.m_bHasShadow, 2);
    this.m_iLength = 6;
  }

  public override int GetStoreSize(ExcelVersion version) => 6;
}
