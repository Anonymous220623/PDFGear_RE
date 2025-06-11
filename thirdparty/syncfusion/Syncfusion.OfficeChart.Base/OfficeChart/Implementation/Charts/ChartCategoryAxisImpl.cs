// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartCategoryAxisImpl
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

internal class ChartCategoryAxisImpl : 
  ChartValueAxisImpl,
  IOfficeChartCategoryAxis,
  IOfficeChartValueAxis,
  IOfficeChartAxis
{
  private const string DEF_NOTSUPPORTED_PROPERTY = "This property is not supported for the current chart type";
  private const int DEF_AXIS_OFFSET = 100;
  private const int DEF_MONTH_COUNT = 12;
  private static readonly DateTime DEF_MIN_DATE = new DateTime(1900, 1, 1);
  internal bool m_xmlTKLabelSkipFrt;
  private bool m_isChartAxisOffsetRecord;
  internal HistogramAxisFormat m_histogramAxisFormat;
  private ChartCatserRangeRecord m_chartCatser;
  private UnknownRecord m_chartMlFrt;
  private ChartAxcextRecord m_axcetRecord = (ChartAxcextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxcext);
  private OfficeCategoryType m_categoryType = OfficeCategoryType.Automatic;
  private int m_iOffset = 100;
  private bool m_bAutoTickLabelSpacing = true;
  private bool m_bnoMultiLvlLbl;
  internal bool m_showNoMultiLvlLbl;
  private bool m_majorUnitIsAuto;
  private bool m_minorUnitIsAuto;
  private bool m_changeDateTimeAxisValue;

  public ChartCategoryAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.AxisId = this.IsPrimary ? 59983360 : 62908672;
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  public ChartCategoryAxisImpl(IApplication application, object parent, OfficeAxisType axisType)
    : this(application, parent, axisType, true)
  {
    this.m_majorUnitIsAuto = true;
    this.m_minorUnitIsAuto = true;
  }

  public ChartCategoryAxisImpl(
    IApplication application,
    object parent,
    OfficeAxisType axisType,
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
        if ((value < 1.0 || value > 31999.0) && !this.ParentWorkbook.IsWorkbookOpening)
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
      return flag1 && !flag3 && this.CategoryType != OfficeCategoryType.Category || flag2 && !flag3 && this.CategoryType == OfficeCategoryType.Time;
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
        this.TickLabelSpacing = 1;
      this.m_bAutoTickLabelSpacing = value;
    }
  }

  internal int LabelFrequency
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

  internal int TickMarksFrequency
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

  public IOfficeDataRange CategoryLabels
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.ParentAxis.ParentChart)
      {
        Range = this.CategoryLabelsIRange
      };
    }
    set
    {
      if ((value as ChartDataRange).Range == null)
        this.CategoryLabelsIRange = (IRange) null;
      else
        this.CategoryLabelsIRange = this.ParentAxis.ParentChart.Workbook.Worksheets[0][value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public IRange CategoryLabelsIRange
  {
    get => (this.ParentChart.Series[0].CategoryLabels as ChartDataRange).Range;
    set
    {
      ChartSeriesCollection series = (ChartSeriesCollection) this.ParentChart.Series;
      int index = 0;
      for (int count = series.Count; index < count; ++index)
      {
        if (value == null)
        {
          series[index].CategoryLabels = (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.ParentAxis.ParentChart);
          ((ChartDataRange) series[index].CategoryLabels).Range = (IRange) null;
        }
        else
          series[index].CategoryLabels = this.ParentAxis.ParentChart.ChartData[value.Row, value.Column, value.LastRow, value.LastColumn];
      }
    }
  }

  public object[] EnteredDirectlyCategoryLabels
  {
    get
    {
      return this.ParentChart.Series.Count == 0 ? (object[]) null : (this.ParentChart.Series[0] as ChartSerieImpl).EnteredDirectlyCategoryLabels;
    }
    set
    {
      ChartSeriesCollection series = (ChartSeriesCollection) this.ParentChart.Series;
      int index = 0;
      for (int count = series.Count; index < count; ++index)
        (series[index] as ChartSerieImpl).EnteredDirectlyCategoryLabels = value;
    }
  }

  public OfficeCategoryType CategoryType
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

  public OfficeChartBaseUnit BaseUnit
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
      if (!this.IsChartBubbleOrScatter && !this.IsCategoryType && !this.ParentWorkbook.IsWorkbookOpening && !this.ParentWorkbook.IsLoaded && !this.ParentWorkbook.IsCreated && value != this.IsAutoMajor)
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
      if (!this.IsChartBubbleOrScatter && !this.IsCategoryType && !this.ParentWorkbook.IsWorkbookOpening && !this.ParentWorkbook.IsLoaded && value != this.IsAutoMinor)
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
      if (!this.IsChartBubbleOrScatter && this.IsCategoryType && !this.ParentWorkbook.IsWorkbookOpening)
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
      if (!this.IsChartBubbleOrScatter && this.IsCategoryType && !this.ParentWorkbook.IsWorkbookOpening)
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
        if ((value < 1.0 || !this.IsAutoMajor && value > this.MajorUnit) && !this.ParentWorkbook.IsWorkbookOpening)
          throw new ArgumentOutOfRangeException(nameof (MinorUnit));
        this.m_axcetRecord.Minor = (ushort) value;
        this.m_axcetRecord.UseDefaultMinorUnits = false;
        this.m_minorUnitIsAuto = false;
      }
    }
  }

  public OfficeChartBaseUnit MajorUnitScale
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

  public OfficeChartBaseUnit MinorUnitScale
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
      if (!this.ParentWorkbook.IsLoaded && value > 31999)
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
    this.ParentChart.Walls = (IOfficeChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.ParentChart, true, data, ref iPos);
  }

  private void ParseCategoryType(ChartAxcextRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (record.UseDefaultDateSettings)
      this.m_categoryType = OfficeCategoryType.Automatic;
    else
      this.m_categoryType = record.IsDateAxis ? OfficeCategoryType.Time : OfficeCategoryType.Category;
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
    ChartAxisOffsetRecord record2 = (ChartAxisOffsetRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisOffset);
    record2.Offset = this.Offset;
    records.Add((IBiffStorage) record2);
    this.SerializeTickRecord(records);
    this.SerializeFont(records);
    this.SerializeAxisBorder(records);
    if (this.m_chartMlFrt != null)
      records.Add((IBiffStorage) this.m_chartMlFrt);
    else if (this.AutoTickLabelSpacing)
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
    this.m_axcetRecord.UseDefaultDateSettings = this.m_categoryType == OfficeCategoryType.Automatic;
    this.m_axcetRecord.IsDateAxis = this.m_categoryType == OfficeCategoryType.Time;
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
    return !throwException || chartBubbleOrScatter ? chartBubbleOrScatter : throw new NotSupportedException("This property is not supported for the current chart type");
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
    categoryAxisImpl.m_categoryType = this.m_categoryType;
    if (this.m_chartMlFrt != null)
      categoryAxisImpl.m_chartMlFrt = (UnknownRecord) this.m_chartMlFrt.Clone();
    return (ChartAxisImpl) categoryAxisImpl;
  }

  private string GetStartChartType()
  {
    IOfficeChartSeries series = this.ParentChart.Series;
    if (series.Count == 0)
      return ChartFormatImpl.GetStartSerieType(this.ParentChart.ChartType);
    string startType = (series[0] as ChartSerieImpl).StartType;
    int index = 1;
    for (int count = series.Count; index < count; ++index)
    {
      if (((ChartSerieImpl) series[index]).StartType != startType)
        return ChartFormatImpl.GetStartSerieType(OfficeChartType.Combination_Chart);
    }
    return startType;
  }

  private void CheckTimeScaleProperties()
  {
    if (this.IsCategoryType || this.IsChartBubbleOrScatter)
      throw new NotSupportedException("Current chart doesnot support this property.");
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
      IOfficeChartSeries series = this.ParentChart.Series;
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
              str = ChartFormatImpl.GetStartSerieType(OfficeChartType.Combination_Chart);
              break;
            }
          }
        }
      }
      return str == "Bubble" || str == "Scatter";
    }
  }

  private bool IsCategoryType => this.m_categoryType == OfficeCategoryType.Category;
}
