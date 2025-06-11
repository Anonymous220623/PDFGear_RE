// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartSerAuxTrendRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartSerAuxTrend)]
internal class ChartSerAuxTrendRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 28;
  public static readonly byte[] DEF_NAN_BYTE_ARRAY = new byte[8]
  {
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 0,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue
  };
  public static readonly double DEF_NAN_VALUE = BitConverter.ToDouble(ChartSerAuxTrendRecord.DEF_NAN_BYTE_ARRAY, 0);
  [BiffRecordPos(0, 1)]
  private byte m_RegType;
  [BiffRecordPos(1, 1)]
  private byte m_Order = 1;
  [BiffRecordPos(2, 8, TFieldType.Float)]
  private double m_numIntercept = ChartSerAuxTrendRecord.DEF_NAN_VALUE;
  [BiffRecordPos(10, 1)]
  private byte m_bEquation;
  [BiffRecordPos(11, 1)]
  private byte m_bRSquared;
  [BiffRecordPos(12, 8, TFieldType.Float)]
  private double m_NumForecast;
  [BiffRecordPos(20, 8, TFieldType.Float)]
  private double m_NumBackcast;

  public ChartSerAuxTrendRecord.TRegression RegressionType
  {
    get => (ChartSerAuxTrendRecord.TRegression) this.m_RegType;
    set => this.m_RegType = (byte) value;
  }

  public byte Order
  {
    get => this.m_Order;
    set => this.m_Order = value;
  }

  public double NumIntercept
  {
    get => this.m_numIntercept;
    set => this.m_numIntercept = value;
  }

  public bool IsEquation
  {
    get => this.m_bEquation == (byte) 1;
    set => this.m_bEquation = value ? (byte) 1 : (byte) 0;
  }

  public bool IsRSquared
  {
    get => this.m_bRSquared == (byte) 1;
    set => this.m_bRSquared = value ? (byte) 1 : (byte) 0;
  }

  public double NumForecast
  {
    get => this.m_NumForecast;
    set => this.m_NumForecast = value;
  }

  public double NumBackcast
  {
    get => this.m_NumBackcast;
    set => this.m_NumBackcast = value;
  }

  public override int MinimumRecordSize => 28;

  public override int MaximumRecordSize => 28;

  public void UpdateType(OfficeTrendLineType type)
  {
    if (type != OfficeTrendLineType.Linear)
      this.m_RegType = (byte) type;
    else
      this.m_RegType = (byte) 0;
  }

  public ChartSerAuxTrendRecord()
  {
  }

  public ChartSerAuxTrendRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSerAuxTrendRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_RegType = provider.ReadByte(iOffset);
    this.m_Order = provider.ReadByte(iOffset + 1);
    this.m_numIntercept = provider.ReadDouble(iOffset + 2);
    this.m_bEquation = provider.ReadByte(iOffset + 10);
    this.m_bRSquared = provider.ReadByte(iOffset + 11);
    this.m_NumForecast = provider.ReadDouble(iOffset + 12);
    this.m_NumBackcast = provider.ReadDouble(iOffset + 20);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteByte(iOffset, this.m_RegType);
    provider.WriteByte(iOffset + 1, this.m_Order);
    provider.WriteDouble(iOffset + 2, this.m_numIntercept);
    provider.WriteByte(iOffset + 10, this.m_bEquation);
    provider.WriteByte(iOffset + 11, this.m_bRSquared);
    provider.WriteDouble(iOffset + 12, this.m_NumForecast);
    provider.WriteDouble(iOffset + 20, this.m_NumBackcast);
    this.m_iLength = 28;
  }

  public enum TRegression
  {
    Polynomial,
    Exponential,
    Logarithmic,
    Power,
    MovingAverage,
  }
}
