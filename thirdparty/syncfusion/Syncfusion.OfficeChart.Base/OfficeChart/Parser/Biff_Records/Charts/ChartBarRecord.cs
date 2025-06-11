// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartBarRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartBar)]
internal class ChartBarRecord : BiffRecordRaw, IChartType
{
  private const int DEF_RECORD_SIZE = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usOverlap;
  [BiffRecordPos(2, 2)]
  private ushort m_usCategoriesSpace = 150;
  [BiffRecordPos(4, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(4, 0, TFieldType.Bit)]
  private bool m_bIsHorizontal;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bStackValues;
  [BiffRecordPos(4, 2, TFieldType.Bit)]
  private bool m_bAsPercents;
  [BiffRecordPos(4, 3, TFieldType.Bit)]
  private bool m_bHasShadow;

  public int Overlap
  {
    get => (int) -(short) this.m_usOverlap;
    set
    {
      if (value == this.Overlap)
        return;
      this.m_usOverlap = (ushort) -value;
    }
  }

  public ushort CategoriesSpace
  {
    get => this.m_usCategoriesSpace;
    set
    {
      if ((int) value == (int) this.m_usCategoriesSpace)
        return;
      this.m_usCategoriesSpace = value;
    }
  }

  public ushort Options => this.m_usOptions;

  public bool IsHorizontalBar
  {
    get => this.m_bIsHorizontal;
    set => this.m_bIsHorizontal = value;
  }

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

  public ChartBarRecord()
  {
  }

  public ChartBarRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartBarRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usOverlap = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usCategoriesSpace = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bIsHorizontal = provider.ReadBit(iOffset, 0);
    this.m_bStackValues = provider.ReadBit(iOffset, 1);
    this.m_bAsPercents = provider.ReadBit(iOffset, 2);
    this.m_bHasShadow = provider.ReadBit(iOffset, 3);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usOptions &= (ushort) 15;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usOverlap);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usCategoriesSpace);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bIsHorizontal, 0);
    provider.WriteBit(iOffset, this.m_bStackValues, 1);
    provider.WriteBit(iOffset, this.m_bAsPercents, 2);
    provider.WriteBit(iOffset, this.m_bHasShadow, 3);
  }

  public override int GetStoreSize(OfficeVersion version) => 6;
}
