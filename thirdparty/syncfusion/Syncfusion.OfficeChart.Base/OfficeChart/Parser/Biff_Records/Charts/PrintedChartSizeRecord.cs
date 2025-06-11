// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.PrintedChartSizeRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.PrintedChartSize)]
[CLSCompliant(false)]
internal class PrintedChartSizeRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_prnChartSize = 3;

  public OfficePrintedChartSize PrintedChartSize
  {
    get => (OfficePrintedChartSize) this.m_prnChartSize;
    set => this.m_prnChartSize = (ushort) value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public PrintedChartSizeRecord()
  {
  }

  public PrintedChartSizeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PrintedChartSizeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_prnChartSize = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_prnChartSize);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
