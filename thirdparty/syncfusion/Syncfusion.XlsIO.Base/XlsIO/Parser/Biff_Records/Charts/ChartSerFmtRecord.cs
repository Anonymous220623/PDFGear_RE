// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartSerFmtRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSerFmt)]
[CLSCompliant(false)]
public class ChartSerFmtRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bSmoothedLine;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_b3DBubbles;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bArShadow;

  public ushort Options => this.m_usOptions;

  public bool IsSmoothedLine
  {
    get => this.m_bSmoothedLine;
    set => this.m_bSmoothedLine = value;
  }

  public bool Is3DBubbles
  {
    get => this.m_b3DBubbles;
    set => this.m_b3DBubbles = value;
  }

  public bool IsArShadow
  {
    get => this.m_bArShadow;
    set => this.m_bArShadow = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public ChartSerFmtRecord()
  {
  }

  public ChartSerFmtRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSerFmtRecord(int iReserve)
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
    this.m_bSmoothedLine = provider.ReadBit(iOffset, 0);
    this.m_b3DBubbles = provider.ReadBit(iOffset, 1);
    this.m_bArShadow = provider.ReadBit(iOffset, 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bSmoothedLine, 0);
    provider.WriteBit(iOffset, this.m_b3DBubbles, 1);
    provider.WriteBit(iOffset, this.m_bArShadow, 2);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
