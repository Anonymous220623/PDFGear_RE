// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAxesUsedRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAxesUsed)]
[CLSCompliant(false)]
internal class ChartAxesUsedRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usAxes;

  public ushort NumberOfAxes
  {
    get => this.m_usAxes;
    set
    {
      if ((int) value == (int) this.m_usAxes)
        return;
      this.m_usAxes = value;
    }
  }

  public ChartAxesUsedRecord()
  {
  }

  public ChartAxesUsedRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxesUsedRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usAxes = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usAxes);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
