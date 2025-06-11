// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartEndDispUnitRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartEndDispUnit)]
internal class ChartEndDispUnitRecord : BiffRecordRaw
{
  public const int DEF_MIN_RECORD_SIZE = 6;
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(4, 4, TFieldType.Bit)]
  private bool m_bIsShowLabel;

  public ChartEndDispUnitRecord()
  {
  }

  public ChartEndDispUnitRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartEndDispUnitRecord(int iReserve)
    : base(iReserve)
  {
  }

  public bool IsShowLabels
  {
    get => this.m_bIsShowLabel;
    set => this.m_bIsShowLabel = value;
  }

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 12;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    iOffset += 4;
    this.m_bIsShowLabel = provider.ReadBit(iOffset, 4);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    int offset = iOffset;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteBit(offset, this.m_bIsShowLabel, 4);
  }

  public override int GetStoreSize(OfficeVersion version) => 12;
}
