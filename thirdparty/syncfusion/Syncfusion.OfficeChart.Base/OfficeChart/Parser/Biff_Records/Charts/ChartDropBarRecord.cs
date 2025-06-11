// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartDropBarRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartDropBar)]
internal class ChartDropBarRecord : BiffRecordRaw
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
        throw new ArgumentOutOfRangeException("m_usGap", "Value cannot be less than 0 or greater than 100.");
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
    OfficeVersion version)
  {
    this.m_usGap = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usGap);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
