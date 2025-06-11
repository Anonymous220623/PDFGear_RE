// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartPieRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartPie)]
[CLSCompliant(false)]
public class ChartPieRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usStartAngle;
  [BiffRecordPos(2, 2)]
  private ushort m_usDonutHoleSize;
  [BiffRecordPos(4, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(4, 0, TFieldType.Bit)]
  private bool m_bHasShadow;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bShowLeaderLines;

  public ushort StartAngle
  {
    get => this.m_usStartAngle;
    set => this.m_usStartAngle = value;
  }

  public ushort DonutHoleSize
  {
    get => this.m_usDonutHoleSize;
    set => this.m_usDonutHoleSize = value;
  }

  public ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
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

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  public ChartPieRecord()
  {
  }

  public ChartPieRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartPieRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usStartAngle = provider.ReadUInt16(iOffset);
    this.m_usDonutHoleSize = provider.ReadUInt16(iOffset + 2);
    this.m_usOptions = provider.ReadUInt16(iOffset + 4);
    this.m_bHasShadow = provider.ReadBit(iOffset + 4, 0);
    this.m_bShowLeaderLines = provider.ReadBit(iOffset + 4, 1);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usStartAngle);
    provider.WriteUInt16(iOffset + 2, this.m_usDonutHoleSize);
    provider.WriteUInt16(iOffset + 4, this.m_usOptions);
    provider.WriteBit(iOffset + 4, this.m_bHasShadow, 0);
    provider.WriteBit(iOffset + 4, this.m_bShowLeaderLines, 1);
    this.m_iLength = 6;
  }
}
