// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartDropBarRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartDropBar)]
[CLSCompliant(false)]
public class ChartDropBarRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usGap;

  public ushort Gap
  {
    get => this.m_usGap;
    set
    {
      if (value < (ushort) 0 || value > (ushort) 500)
        throw new ArgumentOutOfRangeException("m_usGap", "Value cannot be less than 0 or greater than 500.");
      if ((int) value == (int) this.m_usGap)
        return;
      this.m_usGap = value;
    }
  }

  public ChartDropBarRecord()
  {
  }

  public ChartDropBarRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDropBarRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usGap = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usGap);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
