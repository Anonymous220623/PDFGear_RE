// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartSeriesRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSeries)]
[CLSCompliant(false)]
public class ChartSeriesRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(0, 2)]
  private ushort m_usStdX;
  [BiffRecordPos(2, 2)]
  private ushort m_usStdY;
  [BiffRecordPos(4, 2)]
  private ushort m_usCatCount;
  [BiffRecordPos(6, 2)]
  private ushort m_usValCount;
  [BiffRecordPos(8, 2)]
  private ushort m_usBubbleDataType;
  [BiffRecordPos(10, 2)]
  private ushort m_usBubbleSeriesCount;

  public ChartSeriesRecord.DataType StdX
  {
    get => (ChartSeriesRecord.DataType) this.m_usStdX;
    set => this.m_usStdX = (ushort) value;
  }

  public ChartSeriesRecord.DataType StdY
  {
    get => (ChartSeriesRecord.DataType) this.m_usStdY;
    set => this.m_usStdY = (ushort) value;
  }

  public ushort CategoriesCount
  {
    get => this.m_usCatCount;
    set => this.m_usCatCount = value;
  }

  public ushort ValuesCount
  {
    get => this.m_usValCount;
    set => this.m_usValCount = value;
  }

  public ChartSeriesRecord.DataType BubbleDataType
  {
    get => (ChartSeriesRecord.DataType) this.m_usBubbleDataType;
    set => this.m_usBubbleDataType = (ushort) value;
  }

  public ushort BubbleSeriesCount
  {
    get => this.m_usBubbleSeriesCount;
    set => this.m_usBubbleSeriesCount = value;
  }

  public override int MinimumRecordSize => 12;

  public override int MaximumRecordSize => 12;

  public ChartSeriesRecord()
  {
  }

  public ChartSeriesRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSeriesRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usStdX = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usStdY = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usCatCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usValCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usBubbleDataType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usBubbleSeriesCount = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usStdX);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usStdY);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usCatCount);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usValCount);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBubbleDataType);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBubbleSeriesCount);
  }

  public override int GetStoreSize(ExcelVersion version) => 12;

  public enum DataType
  {
    Date,
    Numeric,
    Sequence,
    Text,
  }
}
