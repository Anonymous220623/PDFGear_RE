// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartPlotGrowthRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartPlotGrowth)]
[CLSCompliant(false)]
internal class ChartPlotGrowthRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 4)]
  private uint m_uiHorzGrowth = 65536 /*0x010000*/;
  [BiffRecordPos(4, 4)]
  private uint m_uiVertGrowth = 65536 /*0x010000*/;

  public uint HorzGrowth
  {
    get => this.m_uiHorzGrowth;
    set => this.m_uiHorzGrowth = value;
  }

  public uint VertGrowth
  {
    get => this.m_uiVertGrowth;
    set => this.m_uiVertGrowth = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public ChartPlotGrowthRecord()
  {
  }

  public ChartPlotGrowthRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartPlotGrowthRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_uiHorzGrowth = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiVertGrowth = provider.ReadUInt32(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt32(iOffset, this.m_uiHorzGrowth);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiVertGrowth);
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
