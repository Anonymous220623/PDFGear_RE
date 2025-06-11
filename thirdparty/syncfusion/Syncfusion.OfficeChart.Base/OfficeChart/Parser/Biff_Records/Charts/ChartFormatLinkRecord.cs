// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartFormatLinkRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartFormatLink)]
[CLSCompliant(false)]
internal class ChartFormatLinkRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 10;
  public static readonly byte[] UNKNOWN_BYTES = new byte[10]
  {
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 15,
    (byte) 0
  };

  public ChartFormatLinkRecord()
  {
  }

  public ChartFormatLinkRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartFormatLinkRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 10;
    provider.WriteBytes(iOffset, ChartFormatLinkRecord.UNKNOWN_BYTES, 0, this.m_iLength);
  }

  public override int GetStoreSize(OfficeVersion version) => 10;
}
