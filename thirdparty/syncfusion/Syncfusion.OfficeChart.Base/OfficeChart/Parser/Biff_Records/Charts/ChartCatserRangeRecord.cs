// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartCatserRangeRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartCatserRange)]
[CLSCompliant(false)]
internal class ChartCatserRangeRecord : BiffRecordRaw, IMaxCross
{
  private const int DEF_MIN_CROSSPOINT = 1;
  private const int DEF_MAX_CROSSPOINT = 31999;
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usCrossingPoint = 1;
  [BiffRecordPos(2, 2)]
  private ushort m_usLabelsFrequency = 1;
  [BiffRecordPos(4, 2)]
  private ushort m_usTickMarksFrequency = 1;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions = 1;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bValueAxisCrossing = true;
  [BiffRecordPos(6, 1, TFieldType.Bit)]
  private bool m_bMaxCross;
  [BiffRecordPos(6, 2, TFieldType.Bit)]
  private bool m_bReverse;

  public ushort CrossingPoint
  {
    get => this.m_usCrossingPoint;
    set
    {
      if ((int) value == (int) this.m_usCrossingPoint)
        return;
      this.m_usCrossingPoint = value;
    }
  }

  public ushort LabelsFrequency
  {
    get => this.m_usLabelsFrequency;
    set
    {
      if (value < (ushort) 1 || value > (ushort) 31999)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less 1 and greater than 31999");
      if ((int) value == (int) this.m_usLabelsFrequency)
        return;
      this.m_usLabelsFrequency = value;
    }
  }

  public ushort TickMarksFrequency
  {
    get => this.m_usTickMarksFrequency;
    set
    {
      if (value < (ushort) 1 || value > (ushort) 31999)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less 1 and greater than 31999");
      if ((int) value == (int) this.m_usTickMarksFrequency)
        return;
      this.m_usTickMarksFrequency = value;
    }
  }

  public ushort Options => this.m_usOptions;

  public bool IsBetween
  {
    get => this.m_bValueAxisCrossing;
    set => this.m_bValueAxisCrossing = value;
  }

  public bool IsMaxCross
  {
    get => this.m_bMaxCross;
    set => this.m_bMaxCross = value;
  }

  public bool IsReverse
  {
    get => this.m_bReverse;
    set => this.m_bReverse = value;
  }

  public ChartCatserRangeRecord()
  {
  }

  public ChartCatserRangeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartCatserRangeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usCrossingPoint = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLabelsFrequency = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usTickMarksFrequency = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bValueAxisCrossing = provider.ReadBit(iOffset, 0);
    this.m_bMaxCross = provider.ReadBit(iOffset, 1);
    this.m_bReverse = provider.ReadBit(iOffset, 2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usCrossingPoint);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLabelsFrequency);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usTickMarksFrequency);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bValueAxisCrossing, 0);
    provider.WriteBit(iOffset, this.m_bMaxCross, 1);
    provider.WriteBit(iOffset, this.m_bReverse, 2);
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
