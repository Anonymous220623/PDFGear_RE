// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartIfmtRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartIfmt)]
[CLSCompliant(false)]
internal class ChartIfmtRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usNumberIndex;

  public ushort FormatIndex
  {
    get => this.m_usNumberIndex;
    set
    {
      if ((int) value == (int) this.m_usNumberIndex)
        return;
      this.m_usNumberIndex = value;
    }
  }

  public ChartIfmtRecord()
  {
  }

  public ChartIfmtRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartIfmtRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usNumberIndex = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usNumberIndex);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
