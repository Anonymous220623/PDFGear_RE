// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartFontxRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartFontx)]
public class ChartFontxRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usFontIndex;

  public ushort FontIndex
  {
    get => this.m_usFontIndex;
    set
    {
      if ((int) value == (int) this.m_usFontIndex)
        return;
      this.m_usFontIndex = value;
    }
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public ChartFontxRecord()
  {
  }

  public ChartFontxRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartFontxRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usFontIndex = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFontIndex);
    this.m_iLength = 2;
  }
}
