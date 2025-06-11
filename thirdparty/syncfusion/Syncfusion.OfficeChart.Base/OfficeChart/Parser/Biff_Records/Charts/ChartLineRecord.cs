// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartLineRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartLine)]
[CLSCompliant(false)]
internal class ChartLineRecord : BiffRecordRaw, IChartType
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bStackValues;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bAsPercents;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bHasShadow;

  public ushort Options => this.m_usOptions;

  public bool StackValues
  {
    get => this.m_bStackValues;
    set => this.m_bStackValues = value;
  }

  public bool ShowAsPercents
  {
    get => this.m_bAsPercents;
    set => this.m_bAsPercents = value;
  }

  public bool HasShadow
  {
    get => this.m_bHasShadow;
    set => this.m_bHasShadow = value;
  }

  public ChartLineRecord()
  {
  }

  public ChartLineRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartLineRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bStackValues = provider.ReadBit(iOffset, 0);
    this.m_bAsPercents = provider.ReadBit(iOffset, 1);
    this.m_bHasShadow = provider.ReadBit(iOffset, 2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usOptions &= (ushort) 7;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bStackValues, 0);
    provider.WriteBit(iOffset, this.m_bAsPercents, 1);
    provider.WriteBit(iOffset, this.m_bHasShadow, 2);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(OfficeVersion version) => 2;
}
