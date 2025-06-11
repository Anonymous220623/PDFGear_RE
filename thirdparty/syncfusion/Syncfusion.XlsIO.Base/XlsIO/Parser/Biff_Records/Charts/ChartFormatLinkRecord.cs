// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartFormatLinkRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartFormatLink)]
public class ChartFormatLinkRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 10;
  public static byte[] UNKNOWN_BYTES = new byte[10]
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
    ExcelVersion version)
  {
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 10;
    provider.WriteBytes(iOffset, ChartFormatLinkRecord.UNKNOWN_BYTES, 0, this.m_iLength);
  }

  public override int GetStoreSize(ExcelVersion version) => 10;
}
