// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartDefaultTextRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartDefaultText)]
internal class ChartDefaultTextRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usObjectIdentifier;

  public ChartDefaultTextRecord.TextDefaults TextCharacteristics
  {
    get => (ChartDefaultTextRecord.TextDefaults) this.m_usObjectIdentifier;
    set => this.m_usObjectIdentifier = (ushort) value;
  }

  public ChartDefaultTextRecord()
  {
  }

  public ChartDefaultTextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDefaultTextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usObjectIdentifier = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usObjectIdentifier);
  }

  public override int GetStoreSize(OfficeVersion version) => 2;

  public enum TextDefaults
  {
    ShowLabels,
    ValueAndPercents,
    All,
  }
}
