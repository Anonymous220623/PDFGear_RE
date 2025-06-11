// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartDatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartDat)]
[CLSCompliant(false)]
public class ChartDatRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bHasHorizontalBorders;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bHasVerticalBorders;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bHasBorders;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bShowSeriesKeys;

  public ushort Options => this.m_usOptions;

  public bool HasHorizontalBorders
  {
    get => this.m_bHasHorizontalBorders;
    set => this.m_bHasHorizontalBorders = value;
  }

  public bool HasVerticalBorders
  {
    get => this.m_bHasVerticalBorders;
    set => this.m_bHasVerticalBorders = value;
  }

  public bool HasBorders
  {
    get => this.m_bHasBorders;
    set => this.m_bHasBorders = value;
  }

  public bool ShowSeriesKeys
  {
    get => this.m_bShowSeriesKeys;
    set => this.m_bShowSeriesKeys = value;
  }

  public ChartDatRecord()
  {
  }

  public ChartDatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDatRecord(int iReserve)
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
    this.m_bHasHorizontalBorders = provider.ReadBit(iOffset, 0);
    this.m_bHasVerticalBorders = provider.ReadBit(iOffset, 1);
    this.m_bHasBorders = provider.ReadBit(iOffset, 2);
    this.m_bShowSeriesKeys = provider.ReadBit(iOffset, 3);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 15;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bHasHorizontalBorders, 0);
    provider.WriteBit(iOffset, this.m_bHasVerticalBorders, 1);
    provider.WriteBit(iOffset, this.m_bHasBorders, 2);
    provider.WriteBit(iOffset, this.m_bShowSeriesKeys, 3);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
