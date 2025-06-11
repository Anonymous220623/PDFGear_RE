// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartChartFormatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartChartFormat)]
[CLSCompliant(false)]
public class ChartChartFormatRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 20;
  [BiffRecordPos(0, 4, true)]
  private int m_iReserved0;
  [BiffRecordPos(4, 4, true)]
  private int m_iReserved1;
  [BiffRecordPos(8, 4, true)]
  private int m_iReserved2;
  [BiffRecordPos(12, 4, true)]
  private int m_iReserved3;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(16 /*0x10*/, 0, TFieldType.Bit)]
  private bool m_bIsVaryColor;
  [BiffRecordPos(18, 2)]
  private ushort m_usZOrder;

  public int Reserved0 => this.m_iReserved0;

  public int Reserved1 => this.m_iReserved1;

  public int Reserved2 => this.m_iReserved2;

  public int Reserved3 => this.m_iReserved3;

  public ushort Options => this.m_usOptions;

  public bool IsVaryColor
  {
    get => this.m_bIsVaryColor;
    set => this.m_bIsVaryColor = value;
  }

  public ushort DrawingZOrder
  {
    get => this.m_usZOrder;
    set
    {
      if ((int) value == (int) this.m_usZOrder)
        return;
      this.m_usZOrder = value;
    }
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  public ChartChartFormatRecord()
  {
  }

  public ChartChartFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartChartFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iReserved0 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iReserved1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iReserved2 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iReserved3 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bIsVaryColor = provider.ReadBit(iOffset, 0);
    iOffset += 2;
    this.m_usZOrder = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iReserved0 = this.m_iReserved1 = this.m_iReserved2 = this.m_iReserved3 = 0;
    this.m_usOptions &= (ushort) 1;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt32(iOffset, this.m_iReserved0);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iReserved1);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iReserved2);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iReserved3);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bIsVaryColor, 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usZOrder);
  }

  public override int GetStoreSize(ExcelVersion version) => 20;

  internal bool EqualsWithoutOrder(ChartChartFormatRecord chartFormatRecord)
  {
    return (int) this.m_usOptions == (int) chartFormatRecord.m_usOptions && this.m_bIsVaryColor == chartFormatRecord.m_bIsVaryColor;
  }
}
