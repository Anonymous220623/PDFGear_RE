// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartCategoryAxisImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartCategoryAxisImpl : 
  ChartValueAxisImpl,
  IChartCategoryAxis,
  IChartValueAxis,
  IChartAxis
{
  private const string DEF_NOTSUPPORTED_PROPERTY = "This property is not supported for the current chart type";
  private const int DEF_AXIS_OFFSET = 100;
  private const int DEF_MONTH_COUNT = 12;
  private const ushort DEF_NOMULTILVLLBL_TAG = 46;
  private const ushort DEF_TICKLBLSKIP_TAG = 81;
  private static readonly DateTime DEF_MIN_DATE = new DateTime(1900, 1, 1);
  private ChartCatserRangeRecord m_chartCatser;
  private UnknownRecord m_chartMlFrt;
  private ChartAxcextRecord m_axcetRecord = (ChartAxcextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxcext);
  private ExcelCategoryType m_categoryType = ExcelCategoryType.Automatic;
  private int m_iOffset = 100;
  private bool m_bAutoTickLabelSpacing = true;
  private bool m_bnoMultiLvlLbl;
  internal bool m_showNoMultiLvlLbl;
  private bool m_majorUnitIsAuto;
  private bool m_minorUnitIsAuto;
  internal bool m_xmlTKLabelSkipFrt;
  private bool m_isChartAxisOffsetRecord;
  internal HistogramAxisFormat m_histogramAxisFormat;
  private bool m_changeDateTimeAxisValue;

  public ChartCategoryAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.AxisId = this.IsPrimary ? 59983360 : 62908672;
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  public ChartCategoryAxisImpl(IApplication application, object parent, ExcelAxisType axisType)
    : this(application, parent, axisType, true)
  {
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  public ChartCategoryAxisImpl(
    IApplication application,
    object parent,
    ExcelAxisType axisType,
    bool bIsPrimary)
    : base(application, parent, axisType, bIsPrimary)
  {
    this.AxisId = this.IsPrimary ? 59983360 : 62908672;
    if (!this.IsPrimary)
      this.Visible = false;
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  [CLSCompliant(false)]
  public ChartCategoryAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent, data, ref iPos, true)
  {
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  [CLSCompliant(false)]
  public ChartCategoryAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    bool isPrimary)
    : base(application, parent, data, ref iPos, isPrimary)
  {
    this.AxisId = this.IsPrimary ? 59983360 : 62908672;
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  public override bool IsMaxCross
  {
    get => this.IsChartBubbleOrScatter ? base.IsMaxCross : this.m_chartCatser.IsMaxCross;
    set
    {
      if (this.IsChartBubbleOrScatter)
        base.IsMaxCross = value;
      else
        this.m_chartCatser.IsMaxCross = value;
    }
  }

  public override double CrossesAt
  {
    get
    {
      return !this.IsChartBubbleOrScatter ? (double) this.m_chartCatser.CrossingPoint : base.CrossesAt;
    }
    set
    {
      if (this.IsChartBubbleOrScatter)
      {
        base.CrossesAt = value;
      }
      else
      {
        if ((value < 1.0 || value > 31999.0) && !this.ParentWorkbook.Loading)
          throw new ArgumentOutOfRangeException("For current chart type valid number must be between 1 to 3199");
        this.m_chartCatser.CrossingPoint = (ushort) value;
        this.IsAutoCross = false;
      }
    }
  }

  internal bool ChangeDateTimeAxisValue
  {
    get => this.m_changeDateTimeAxisValue;
    set => this.m_changeDateTimeAxisValue = value;
  }

  internal bool HasAutoTickLabelSpacing
  {
    get
    {
      if (this.EnteredDirectlyCategoryLabels == null)
        return false;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      foreach (object directlyCategoryLabel in this.EnteredDirectlyCategoryLabels)
      {
        if (directlyCategoryLabel != null)
        {
          if (DateTime.TryParse(directlyCategoryLabel.ToString(), out DateTime _))
            flag1 = true;
          else if (double.TryParse(directlyCategoryLabel.ToString(), out double _))
            flag2 = true;
          else
            flag3 = true;
        }
      }
      return flag1 && !flag3 && this.CategoryType != ExcelCategoryType.Category || flag2 && !flag3 && this.CategoryType == ExcelCategoryType.Time;
    }
  }

  public new bool AutoTickLabelSpacing
  {
    get
    {
      return this.TickLabelSpacing == 1 && !this.HasAutoTickLabelSpacing && this.m_bAutoTickLabelSpacing;
    }
    set
    {
      if (value)
      {
        this.m_chartCatser.LabelsFrequency = (ushort) 1;
        if (this.m_chartMlFrt != null)
          this.m_chartMlFrt.m_data[22] = (byte) 81;
      }
      else if (this.m_chartMlFrt != null)
        this.m_chartMlFrt.m_data[22] = (byte) 82;
      this.m_bAutoTickLabelSpacing = value;
    }
  }

  public int LabelFrequency
  {
    get => this.TickLabelSpacing;
    set => this.TickLabelSpacing = value;
  }

  public int TickLabelSpacing
  {
    get => (int) this.m_chartCatser.LabelsFrequency;
    set
    {
      this.m_chartCatser.LabelsFrequency = value >= 0 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0");
      this.AutoTickLabelSpacing = false;
    }
  }

  public int TickMarksFrequency
  {
    get => this.TickMarkSpacing;
    set
    {
      this.TickMarkSpacing = value;
      this.AutoTickMarkSpacing = false;
    }
  }

  public int TickMarkSpacing
  {
    get => (int) this.m_chartCatser.TickMarksFrequency;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0");
      this.AutoTickMarkSpacing = false;
      this.m_chartCatser.TickMarksFrequency = (ushort) value;
    }
  }

  [CLSCompliant(false)]
  protected override ExcelObjectTextLink TextLinkType => ExcelObjectTextLink.XAxis;

  public bool IsBetween
  {
    get => this.CatserRecord.IsBetween;
    set => this.CatserRecord.IsBetween = value;
  }

  public override bool ReversePlotOrder
  {
    get => this.IsChartBubbleOrScatter ? base.ReversePlotOrder : this.CatserRecord.IsReverse;
    set
    {
      if (this.IsChartBubbleOrScatter)
      {
        base.ReversePlotOrder = value;
      }
      else
      {
        this.CatserRecord.IsReverse = value;
        base.ReversePlotOrder = value;
      }
    }
  }

  public IRange CategoryLabels
  {
    get
    {
      ChartSeriesCollection series = (ChartSeriesCollection) this.ParentChart.Series;
      bool isPrimary = this.ParentAxis.IsPrimary;
      int index = 0;
      for (int count = series.Count; index < count; ++index)
      {
        if (isPrimary == series[index].UsePrimaryAxis)
          return series[index].CategoryLabels;
      }
      return (IRange) null;
    }
    set
    {
      ChartSeriesCollection series = (ChartSeriesCollection) this.ParentChart.Series;
      bool isPrimary = this.ParentAxis.IsPrimary;
      int index = 0;
      for (int count = series.Count; index < count; ++index)
      {
        if (series[index].UsePrimaryAxis == isPrimary)
          series[index].CategoryLabels = value;
      }
    }
  }

  public object[] EnteredDirectlyCategoryLabels
  {
    get
    {
      return this.ParentChart.Series.Count > 0 ? this.ParentChart.Series[0].EnteredDirectlyCategoryLabels : (object[]) null;
    }
    set
    {
      ChartSeriesCollection series = (ChartSeriesCollection) this.ParentChart.Series;
      int index = 0;
      for (int count = series.Count; index < count; ++index)
        series[index].EnteredDirectlyCategoryLabels = value;
    }
  }

  public ExcelCategoryType CategoryType
  {
    get => this.m_categoryType;
    set => this.m_categoryType = value;
  }

  public int Offset
  {
    get => this.m_iOffset;
    set
    {
      this.m_iOffset = value >= 0 && value <= 1000 ? value : throw new ArgumentOutOfRangeException("The value can be from 0 through 1000.");
    }
  }

  public ExcelChartBaseUnit BaseUnit
  {
    get
    {
      this.CheckTimeScaleProperties();
      return this.m_axcetRecord.BaseUnits;
    }
    set
    {
      this.CheckTimeScaleProperties();
      this.m_axcetRecord.BaseUnits = value;
      this.m_axcetRecord.UseDefaultBaseUnits = false;
    }
  }

  public bool BaseUnitIsAuto
  {
    get
    {
      this.CheckTimeScaleProperties();
      return this.m_axcetRecord.UseDefaultBaseUnits;
    }
    set
    {
      this.CheckTimeScaleProperties();
      this.m_axcetRecord.UseDefaultBaseUnits = value;
    }
  }

  internal bool MajorUnitScaleIsAuto
  {
    get => this.m_majorUnitIsAuto;
    set => this.m_majorUnitIsAuto = false;
  }

  internal bool MinorUnitScaleIsAuto
  {
    get => this.m_minorUnitIsAuto;
    set => this.m_minorUnitIsAuto = false;
  }

  public override bool IsAutoMajor
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.IsAutoMajor;
      return this.IsCategoryType || this.m_axcetRecord.UseDefaultMajorUnits;
    }
    set
    {
      if (!this.IsChartBubbleOrScatter && !this.IsCategoryType && !this.ParentWorkbook.Loading && !this.ParentWorkbook.IsLoaded && !this.ParentWorkbook.IsCreated && value != this.IsAutoMajor)
        throw new NotSupportedException("This property is not supported for the current chart type");
      base.IsAutoMajor = value;
      this.m_axcetRecord.UseDefaultMajorUnits = value;
    }
  }

  public override bool IsAutoMinor
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.IsAutoMinor;
      return this.IsCategoryType || this.m_axcetRecord.UseDefaultMinorUnits;
    }
    set
    {
      if (!this.IsChartBubbleOrScatter && !this.IsCategoryType && !this.ParentWorkbook.Loading && !this.ParentWorkbook.IsLoaded && value != this.IsAutoMinor)
        throw new NotSupportedException("This property is not supported for the current chart type");
      base.IsAutoMinor = value;
      this.m_axcetRecord.UseDefaultMinorUnits = value;
    }
  }

  public override bool IsAutoCross
  {
    get => this.IsChartBubbleOrScatter ? base.IsAutoCross : this.m_axcetRecord.UseDefaultCrossPoint;
    set
    {
      base.IsAutoCross = value;
      if (this.IsChartBubbleOrScatter)
        return;
      this.m_axcetRecord.UseDefaultCrossPoint = value;
      base.IsAutoCross = value;
    }
  }

  public override bool IsAutoMax
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.IsAutoMax;
      return this.IsCategoryType || this.m_axcetRecord.UseDefaultMaximum;
    }
    set
    {
      this.SetAutoMax(false, value);
      this.m_axcetRecord.UseDefaultMaximum = value;
      if (!this.IsChartBubbleOrScatter && this.IsCategoryType && !this.ParentWorkbook.Loading)
        throw new NotSupportedException("This property is not supported for the current chart type");
    }
  }

  public override bool IsAutoMin
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.IsAutoMin;
      return this.IsCategoryType || this.m_axcetRecord.UseDefaultMinimum;
    }
    set
    {
      this.SetAutoMin(false, value);
      this.m_axcetRecord.UseDefaultMinimum = value;
      if (!this.IsChartBubbleOrScatter && this.IsCategoryType && !this.ParentWorkbook.Loading)
        throw new NotSupportedException("This property is not supported for the current chart type");
    }
  }

  public override double MajorUnit
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.MajorUnit;
      if (this.IsCategoryType)
        throw new NotSupportedException("This property is not supported for the current chart type");
      return (double) this.m_axcetRecord.Major;
    }
    set
    {
      if (this.IsChartBubbleOrScatter)
      {
        base.MajorUnit = value;
      }
      else
      {
        if (this.IsCategoryType)
          throw new NotSupportedException("This property is not supported for the current chart type");
        if (value < 1.0 || !this.IsAutoMinor && value < this.MinorUnit)
          throw new ArgumentOutOfRangeException(nameof (MajorUnit));
        this.m_majorUnitIsAuto = false;
        this.m_axcetRecord.Major = (ushort) value;
        this.m_axcetRecord.UseDefaultMajorUnits = false;
      }
    }
  }

  public override double MinorUnit
  {
    get
    {
      if (this.IsChartBubbleOrScatter)
        return base.MinorUnit;
      if (this.IsCategoryType)
        throw new NotSupportedException("This property is not supported for the current chart type");
      return (double) this.m_axcetRecord.Minor;
    }
    set
    {
      if (this.IsChartBubbleOrScatter)
      {
        base.MinorUnit = value;
      }
      else
      {
        if (this.IsCategoryType)
          throw new NotSupportedException("This property is not supported for the current chart type");
        if ((value < 1.0 || !this.IsAutoMajor && value > this.MajorUnit) && !this.ParentWorkbook.Loading)
          throw new ArgumentOutOfRangeException(nameof (MinorUnit));
        this.m_axcetRecord.Minor = (ushort) value;
        this.m_axcetRecord.UseDefaultMinorUnits = false;
        this.m_minorUnitIsAuto = false;
      }
    }
  }

  public ExcelChartBaseUnit MajorUnitScale
  {
    get
    {
      this.CheckTimeScaleProperties();
      return this.m_axcetRecord.MajorUnits;
    }
    set
    {
      this.CheckTimeScaleProperties();
      if (!this.IsAutoMinor && value < this.MinorUnitScale)
        throw new ArgumentOutOfRangeException("This property is not supported for the current chart type");
      this.m_majorUnitIsAuto = false;
      this.m_axcetRecord.MajorUnits = value;
    }
  }

  public ExcelChartBaseUnit MinorUnitScale
  {
    get
    {
      this.CheckTimeScaleProperties();
      return this.m_axcetRecord.MinorUnits;
    }
    set
    {
      this.CheckTimeScaleProperties();
      if (!this.IsAutoMajor && this.MajorUnitScale < value)
        throw new ArgumentOutOfRangeException("This property is not supported for the current chart type");
      this.m_minorUnitIsAuto = false;
      this.m_axcetRecord.MinorUnits = value;
    }
  }

  public bool NoMultiLevelLabel
  {
    get => this.m_bnoMultiLvlLbl;
    set => this.m_bnoMultiLvlLbl = value;
  }

  public bool IsBinningByCategory
  {
    get => this.HistogramAxisFormatProperty.IsBinningByCategory;
    set => this.HistogramAxisFormatProperty.IsBinningByCategory = value;
  }

  public bool HasAutomaticBins
  {
    get => this.HistogramAxisFormatProperty.HasAutomaticBins;
    set => this.HistogramAxisFormatProperty.HasAutomaticBins = value;
  }

  public int NumberOfBins
  {
    get => this.HistogramAxisFormatProperty.NumberOfBins;
    set
    {
      if (!this.ParentWorkbook.Loading && value > 31999)
        value = 31999;
      this.HistogramAxisFormatProperty.NumberOfBins = value;
    }
  }

  public double BinWidth
  {
    get => this.HistogramAxisFormatProperty.BinWidth;
    set => this.HistogramAxisFormatProperty.BinWidth = value;
  }

  public double UnderflowBinValue
  {
    get => this.HistogramAxisFormatProperty.UnderflowBinValue;
    set => this.HistogramAxisFormatProperty.UnderflowBinValue = value;
  }

  public double OverflowBinValue
  {
    get => this.HistogramAxisFormatProperty.OverflowBinValue;
    set => this.HistogramAxisFormatProperty.OverflowBinValue = value;
  }

  internal HistogramAxisFormat HistogramAxisFormatProperty
  {
    get
    {
      if (this.m_histogramAxisFormat == null)
        this.m_histogramAxisFormat = new HistogramAxisFormat();
      return this.m_histogramAxisFormat;
    }
  }

  [CLSCompliant(false)]
  protected override void ParseData(BiffRecordRaw record, IList<BiffRecordRaw> data, ref int iPos)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartAxisOffset:
        this.m_iOffset = ((ChartAxisOffsetRecord) record).Offset;
        this.m_isChartAxisOffsetRecord = true;
        break;
      case TBIFFRecord.ChartMlFrt:
        this.m_chartMlFrt = (UnknownRecord) record;
        break;
      case TBIFFRecord.ChartValueRange:
      case TBIFFRecord.ChartCatserRange:
        this.ParseMaxCross(record);
        break;
      case TBIFFRecord.ChartAxcext:
        this.m_axcetRecord = (ChartAxcextRecord) record;
        this.ParseCategoryType(this.m_axcetRecord);
        break;
      default:
        base.ParseData(record, data, ref iPos);
        break;
    }
  }

  [CLSCompliant(false)]
  protected override void ParseMaxCross(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartValueRange:
        this.ChartValueRange = (ChartValueRangeRecord) record;
        break;
      case TBIFFRecord.ChartCatserRange:
        this.m_chartCatser = (ChartCatserRangeRecord) record;
        break;
      default:
        throw new ApplicationException("Unknown record type");
    }
  }

  protected override void ParseWallsOrFloor(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.ParentChart.Walls = (IChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.ParentChart, true, data, ref iPos);
  }

  private void ParseCategoryType(ChartAxcextRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (record.UseDefaultDateSettings)
      this.m_categoryType = ExcelCategoryType.Automatic;
    else
      this.m_categoryType = record.IsDateAxis ? ExcelCategoryType.Time : ExcelCategoryType.Category;
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.IsChartBubbleOrScatter)
      this.Serialize(records, ChartAxisRecord.ChartAxisType.CategoryAxis);
    else
      this.SerializeCategory(records);
  }

  private void SerializeCategory(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartAxisRecord record1 = (ChartAxisRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxis);
    record1.AxisType = ChartAxisRecord.ChartAxisType.CategoryAxis;
    records.Add((IBiffStorage) record1);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    records.Add((IBiffStorage) this.m_chartCatser.Clone());
    this.SerializeAxcetRecord(records);
    this.SerializeNumberFormat(records);
    if ((this.ParentChart.Workbook as WorkbookImpl).IsCreated || this.m_isChartAxisOffsetRecord)
    {
      ChartAxisOffsetRecord record2 = (ChartAxisOffsetRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisOffset);
      record2.Offset = this.Offset;
      records.Add((IBiffStorage) record2);
    }
    this.SerializeTickRecord(records);
    this.SerializeFont(records);
    this.SerializeAxisBorder(records);
    if (this.m_chartMlFrt != null)
      records.Add((IBiffStorage) this.m_chartMlFrt);
    if ((!this.ParentWorkbook.IsCreated ? (!this.AutoTickLabelSpacing ? 0 : (this.m_xmlTKLabelSkipFrt ? 1 : 0)) : (this.AutoTickLabelSpacing ? 1 : 0)) != 0)
    {
      UnknownRecord unknownRecord = new UnknownRecord();
      unknownRecord.RecordCode = 2206;
      unknownRecord.m_data = new byte[32 /*0x20*/]
      {
        (byte) 158,
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 12,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 81,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      unknownRecord.Length = unknownRecord.m_data.Length;
      records.Add((IBiffStorage) unknownRecord);
    }
    else
    {
      UnknownRecord unknownRecord = new UnknownRecord();
      unknownRecord.RecordCode = 2206;
      unknownRecord.m_data = new byte[32 /*0x20*/]
      {
        (byte) 158,
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 12,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 82,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      unknownRecord.Length = unknownRecord.m_data.Length;
      records.Add((IBiffStorage) unknownRecord);
    }
    if (this.NoMultiLevelLabel)
    {
      UnknownRecord unknownRecord = new UnknownRecord();
      unknownRecord.RecordCode = 2206;
      unknownRecord.m_data = new byte[30]
      {
        (byte) 158,
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 10,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        (byte) 46,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      unknownRecord.Length = unknownRecord.m_data.Length;
      records.Add((IBiffStorage) unknownRecord);
    }
    if (this.IsPrimary)
    {
      this.SerializeGridLines(records);
      this.SerializeWallsOrFloor(records);
    }
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  [CLSCompliant(false)]
  protected override void SerializeWallsOrFloor(OffsetArrayList records)
  {
    this.ParentChart.SerializeWalls(records);
  }

  private void SerializeAxcetRecord(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.m_axcetRecord.UseDefaultDateSettings = this.m_categoryType == ExcelCategoryType.Automatic;
    this.m_axcetRecord.IsDateAxis = this.m_categoryType == ExcelCategoryType.Time;
    records.Add((IBiffStorage) this.m_axcetRecord);
  }

  protected override void InitializeVariables()
  {
    this.m_chartCatser = (ChartCatserRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartCatserRange);
    this.ChartValueRange = (ChartValueRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartValueRange);
    base.InitializeVariables();
  }

  protected override bool CheckValueRangeRecord(bool throwException)
  {
    bool chartBubbleOrScatter = this.IsChartBubbleOrScatter;
    return !throwException || chartBubbleOrScatter ? chartBubbleOrScatter : throw new NotSupportedException("This property is supported only in bubble and scatter chart types");
  }

  public override ChartAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartCategoryAxisImpl categoryAxisImpl = (ChartCategoryAxisImpl) base.Clone(parent, dicFontIndexes, dicNewSheetNames);
    if (this.m_chartCatser != null)
      categoryAxisImpl.m_chartCatser = (ChartCatserRangeRecord) this.m_chartCatser.Clone();
    if (this.m_axcetRecord != null)
      categoryAxisImpl.m_axcetRecord = (ChartAxcextRecord) this.m_axcetRecord.Clone();
    return (ChartAxisImpl) categoryAxisImpl;
  }

  private string GetStartChartType()
  {
    IChartSeries series = this.ParentChart.Series;
    if (series.Count == 0)
      return ChartFormatImpl.GetStartSerieType(this.ParentChart.ChartType);
    string startType = (series[0] as ChartSerieImpl).StartType;
    int index = 1;
    for (int count = series.Count; index < count; ++index)
    {
      if (((ChartSerieImpl) series[index]).StartType != startType)
        return ChartFormatImpl.GetStartSerieType(ExcelChartType.Combination_Chart);
    }
    return startType;
  }

  private void CheckTimeScaleProperties()
  {
    if (this.IsCategoryType || this.IsChartBubbleOrScatter)
      throw new NotSupportedException("Current chart doesnot support this property.");
  }

  private ChartCatserRangeRecord CatserRecord
  {
    get
    {
      if (this.m_chartCatser == null)
        this.m_chartCatser = (ChartCatserRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartCatserRange);
      return this.m_chartCatser;
    }
  }

  internal bool IsChartBubbleOrScatter
  {
    get
    {
      string str = string.Empty;
      IChartSeries series = this.ParentChart.Series;
      if (series.Count == 0)
      {
        str = ChartFormatImpl.GetStartSerieType(this.ParentChart.ChartType);
      }
      else
      {
        bool isPrimary = (this.Parent as ChartParentAxisImpl).ChartFormats.IsPrimary;
        for (int index = 0; index < series.Count; ++index)
        {
          if (isPrimary == series[index].UsePrimaryAxis)
          {
            string startType = (series[index] as ChartSerieImpl).StartType;
            if (str == string.Empty)
              str = startType;
            else if (str != startType)
            {
              str = ChartFormatImpl.GetStartSerieType(ExcelChartType.Combination_Chart);
              break;
            }
          }
        }
      }
      return str == "Bubble" || str == "Scatter";
    }
  }

  private bool IsCategoryType => this.m_categoryType == ExcelCategoryType.Category;

  internal bool CheckForXmlTKOptions(BiffRecordRaw record)
  {
    bool flag = false;
    int int32 = BitConverter.ToInt32(record.Data, 12);
    byte[] destinationArray = new byte[int32];
    Array.Copy((Array) record.Data, 16 /*0x10*/, (Array) destinationArray, 0, int32);
    if (int32 > 9 && BitConverter.ToInt16(destinationArray, 2) == (short) 4)
    {
      ushort int16 = (ushort) BitConverter.ToInt16(destinationArray, 6);
      if (int16 == (ushort) 81 && this.AutoTickLabelSpacing)
      {
        this.m_xmlTKLabelSkipFrt = true;
        flag = true;
      }
      else if (int16 == (ushort) 46)
      {
        this.m_bnoMultiLvlLbl = BitConverter.ToBoolean(destinationArray, 8);
        flag = true;
      }
    }
    return flag;
  }

  internal void SwapAxisValues()
  {
    if (this.m_chartCatser.IsMaxCross)
      base.IsMaxCross = this.m_chartCatser.IsMaxCross;
    else if (this.m_axcetRecord.UseDefaultCrossPoint)
      base.IsAutoCross = this.m_axcetRecord.UseDefaultCrossPoint;
    else
      base.CrossesAt = (double) this.m_chartCatser.CrossingPoint;
    base.ReversePlotOrder = this.m_chartCatser.IsReverse;
  }
}
