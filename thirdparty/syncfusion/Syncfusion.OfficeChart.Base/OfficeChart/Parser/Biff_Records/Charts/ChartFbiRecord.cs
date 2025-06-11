// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartFbiRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartFbi)]
[CLSCompliant(false)]
internal class ChartFbiRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 10;
  [BiffRecordPos(0, 2)]
  private ushort m_usBasisWidth;
  [BiffRecordPos(2, 2)]
  private ushort m_usBasisHeight;
  [BiffRecordPos(4, 2)]
  private ushort m_usAppliedFontHeight;
  [BiffRecordPos(6, 2)]
  private ushort m_usScaleBasis;
  [BiffRecordPos(8, 2)]
  private ushort m_usFontIndex;

  public ushort BasisWidth
  {
    get => this.m_usBasisWidth;
    set
    {
      if ((int) value == (int) this.m_usBasisWidth)
        return;
      this.m_usBasisWidth = value;
    }
  }

  public ushort BasisHeight
  {
    get => this.m_usBasisHeight;
    set
    {
      if ((int) value == (int) this.m_usBasisHeight)
        return;
      this.m_usBasisHeight = value;
    }
  }

  public ushort AppliedFontHeight
  {
    get => this.m_usAppliedFontHeight;
    set
    {
      if ((int) value == (int) this.m_usAppliedFontHeight)
        return;
      this.m_usAppliedFontHeight = value;
    }
  }

  public ushort ScaleBasis
  {
    get => this.m_usScaleBasis;
    set
    {
      if ((int) value == (int) this.m_usScaleBasis)
        return;
      this.m_usScaleBasis = value;
    }
  }

  public ushort FontIndex
  {
    get => this.m_usFontIndex;
    set
    {
      if ((int) value == (int) this.m_usFontIndex)
        return;
      this.m_usFontIndex = value;
    }
  }

  public ChartFbiRecord()
  {
  }

  public ChartFbiRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartFbiRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usBasisWidth = provider.ReadUInt16(iOffset);
    this.m_usBasisHeight = provider.ReadUInt16(iOffset + 2);
    this.m_usAppliedFontHeight = provider.ReadUInt16(iOffset + 4);
    this.m_usScaleBasis = provider.ReadUInt16(iOffset + 6);
    this.m_usFontIndex = provider.ReadUInt16(iOffset + 8);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usBasisWidth);
    provider.WriteUInt16(iOffset + 2, this.m_usBasisHeight);
    provider.WriteUInt16(iOffset + 4, this.m_usAppliedFontHeight);
    provider.WriteUInt16(iOffset + 6, this.m_usScaleBasis);
    provider.WriteUInt16(iOffset + 8, this.m_usFontIndex);
  }

  public override int GetStoreSize(OfficeVersion version) => 10;
}
