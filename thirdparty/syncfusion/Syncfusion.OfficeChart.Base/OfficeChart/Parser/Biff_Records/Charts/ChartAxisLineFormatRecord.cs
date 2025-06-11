// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAxisLineFormatRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAxisLineFormat)]
internal class ChartAxisLineFormatRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usAxisLineId;

  public ExcelAxisLineIdentifier LineIdentifier
  {
    get => (ExcelAxisLineIdentifier) this.m_usAxisLineId;
    set => this.m_usAxisLineId = (ushort) value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public ChartAxisLineFormatRecord()
  {
  }

  public ChartAxisLineFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisLineFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usAxisLineId = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usAxisLineId &= (ushort) 3;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usAxisLineId);
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
