// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAxisOffsetRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAxisOffset)]
internal class ChartAxisOffsetRecord : BiffRecordRaw
{
  public const int DEF_MIN_RECORD_SIZE = 10;
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(4, 2)]
  private ushort m_usOffset;

  public ChartAxisOffsetRecord()
  {
  }

  public ChartAxisOffsetRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisOffsetRecord(int iReserve)
    : base(iReserve)
  {
  }

  public int Offset
  {
    get => (int) this.m_usOffset;
    set => this.m_usOffset = (ushort) value;
  }

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 12;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    iOffset += 4;
    this.m_usOffset = provider.ReadUInt16(iOffset);
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
    provider.WriteUInt16(iOffset, this.m_usOffset);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 2);
    iOffset += 2;
    provider.WriteUInt32(iOffset, 0U);
  }

  public override int GetStoreSize(OfficeVersion version) => 12;
}
