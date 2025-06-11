// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MarginRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.BottomMargin)]
[Biff(TBIFFRecord.LeftMargin)]
[Biff(TBIFFRecord.RightMargin)]
[CLSCompliant(false)]
[Biff(TBIFFRecord.TopMargin)]
public class MarginRecord : BiffRecordRaw
{
  public const double DEFAULT_VALUE = 0.0;
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 8, TFieldType.Float)]
  private double m_dbMargin;

  public double Margin
  {
    get => this.m_dbMargin;
    set => this.m_dbMargin = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public MarginRecord()
  {
  }

  public MarginRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public MarginRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_dbMargin = provider.ReadDouble(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 8;
    provider.WriteDouble(iOffset, this.m_dbMargin);
  }
}
