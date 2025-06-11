// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartSeriesAxisImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartSeriesAxisImpl : 
  ChartAxisImpl,
  IOfficeChartSeriesAxis,
  IOfficeChartAxis,
  IScalable
{
  private const int DEF_MAX_SPACING_VALUE = 31999;
  private ChartCatserRangeRecord m_chartCatserRange;

  public ChartSeriesAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.AxisId = 63149376;
  }

  public ChartSeriesAxisImpl(IApplication application, object parent, OfficeAxisType axisType)
    : this(application, parent, axisType, true)
  {
  }

  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    OfficeAxisType axisType,
    bool bIsPrimary)
    : base(application, parent, axisType, bIsPrimary)
  {
    this.AxisId = 63149376;
  }

  [CLSCompliant(false)]
  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent, data, ref iPos, true)
  {
  }

  [CLSCompliant(false)]
  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    bool isPrimary)
    : base(application, parent, data, ref iPos, isPrimary)
  {
    this.AxisId = 63149376;
  }

  public int LabelFrequency
  {
    get => this.TickLabelSpacing;
    set => this.TickLabelSpacing = value;
  }

  public int TickLabelSpacing
  {
    get => (int) this.m_chartCatserRange.LabelsFrequency;
    set
    {
      this.m_chartCatserRange.LabelsFrequency = value >= 0 && value <= 31999 ? (ushort) value : throw new ArgumentOutOfRangeException("Value must be great then 0 and less then 31999.");
    }
  }

  public int TickMarksFrequency
  {
    get => this.TickMarkSpacing;
    set => this.TickMarkSpacing = value;
  }

  public int TickMarkSpacing
  {
    get => (int) this.m_chartCatserRange.TickMarksFrequency;
    set
    {
      this.m_chartCatserRange.TickMarksFrequency = value >= 0 && value <= 31999 ? (ushort) value : throw new ArgumentOutOfRangeException("Value must be great then 0 and less then 31999.");
    }
  }

  public override bool ReversePlotOrder
  {
    get => this.m_chartCatserRange.IsReverse;
    set => this.m_chartCatserRange.IsReverse = value;
  }

  [CLSCompliant(false)]
  protected override ExcelObjectTextLink TextLinkType => ExcelObjectTextLink.ZAxis;

  public int CrossesAt
  {
    get => (int) this.m_chartCatserRange.CrossingPoint;
    set => this.m_chartCatserRange.CrossingPoint = (ushort) value;
  }

  public bool IsBetween
  {
    get => this.m_chartCatserRange.IsBetween;
    set => this.m_chartCatserRange.IsBetween = value;
  }

  private void ParseMaxCross(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    record.CheckTypeCode(TBIFFRecord.ChartCatserRange);
    this.m_chartCatserRange = (ChartCatserRangeRecord) record;
  }

  protected override void ParseWallsOrFloor(IList<BiffRecordRaw> data, ref int iPos)
  {
    throw new NotSupportedException("Current axis type doesn't support walls or floors");
  }

  [CLSCompliant(false)]
  protected override void ParseData(BiffRecordRaw record, IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (record.TypeCode != TBIFFRecord.ChartCatserRange)
      return;
    this.ParseMaxCross(record);
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    ChartAxisRecord record = (ChartAxisRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxis);
    record.AxisType = ChartAxisRecord.ChartAxisType.SeriesAxis;
    records.Add((IBiffStorage) record);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    records.Add((IBiffStorage) this.m_chartCatserRange.Clone());
    this.SerializeTickRecord(records);
    this.SerializeNumberFormat(records);
    this.SerializeFont(records);
    this.SerializeAxisBorder(records);
    this.SerializeGridLines(records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  protected override void InitializeVariables()
  {
    this.m_chartCatserRange = (ChartCatserRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartCatserRange);
    base.InitializeVariables();
  }

  public override ChartAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartSeriesAxisImpl chartSeriesAxisImpl = (ChartSeriesAxisImpl) base.Clone(parent, dicFontIndexes, dicNewSheetNames);
    if (this.m_chartCatserRange != null)
      chartSeriesAxisImpl.m_chartCatserRange = (ChartCatserRangeRecord) this.m_chartCatserRange.Clone();
    return (ChartAxisImpl) chartSeriesAxisImpl;
  }

  public bool IsLogScale
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public double MaximumValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public double MinimumValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }
}
