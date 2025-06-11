// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartLegendxnRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartLegendxn)]
[CLSCompliant(false)]
internal class ChartLegendxnRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usLegendEntityIndex = ushort.MaxValue;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_bIsDeleted;
  [BiffRecordPos(2, 1, TFieldType.Bit)]
  private bool m_bIsFormatted;

  public ushort LegendEntityIndex
  {
    get => this.m_usLegendEntityIndex;
    set
    {
      if ((int) value == (int) this.m_usLegendEntityIndex)
        return;
      this.m_usLegendEntityIndex = value;
    }
  }

  public ushort Options => this.m_usOptions;

  public bool IsDeleted
  {
    get => this.m_bIsDeleted;
    set => this.m_bIsDeleted = value;
  }

  public bool IsFormatted
  {
    get => this.m_bIsFormatted;
    set => this.m_bIsFormatted = value;
  }

  public ChartLegendxnRecord()
  {
  }

  public ChartLegendxnRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartLegendxnRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usLegendEntityIndex = provider.ReadUInt16(iOffset);
    this.m_usOptions = provider.ReadUInt16(iOffset + 2);
    this.m_bIsDeleted = provider.ReadBit(iOffset + 2, 0);
    this.m_bIsFormatted = provider.ReadBit(iOffset + 2, 1);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usOptions &= (ushort) 3;
    provider.WriteUInt16(iOffset, this.m_usLegendEntityIndex);
    provider.WriteUInt16(iOffset + 2, this.m_usOptions);
    provider.WriteBit(iOffset + 2, this.m_bIsDeleted, 0);
    provider.WriteBit(iOffset + 2, this.m_bIsFormatted, 1);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(OfficeVersion version) => 4;
}
