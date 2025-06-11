// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.PrintedChartSizeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.PrintedChartSize)]
[CLSCompliant(false)]
public class PrintedChartSizeRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_prnChartSize = 3;

  public ExcelPrintedChartSize PrintedChartSize
  {
    get => (ExcelPrintedChartSize) this.m_prnChartSize;
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
    ExcelVersion version)
  {
    this.m_prnChartSize = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_prnChartSize);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
