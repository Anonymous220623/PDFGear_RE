// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartSerieImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartSerieImpl : 
  CommonObject,
  IOfficeChartSerie,
  IParentApplication,
  ISerializableNamedObject,
  INamedObject,
  IReparse
{
  public const int DEF_FORMAT_ALLPOINTS_INDEX = 65535 /*0xFFFF*/;
  private const int DEF_SURFACE_POINT_NUMBER = 65532;
  public const int DEF_CHART_GROUP = -1;
  private const string DEF_RADAR_START_TYPE = "Radar";
  public const string DEF_TRUE = "TRUE";
  public const string DEF_FALSE = "FALSE";
  private Dictionary<int, object> m_dataLabelCellsValues;
  private string m_serieName;
  private IRange m_ValueRange;
  private IRange m_CategoryRange;
  private IRange m_BubbleRange;
  private string m_strName;
  private Ptg[] m_nameTokens;
  private Dictionary<ChartAIRecord.LinkIndex, ChartAIRecord> m_hashAi = new Dictionary<ChartAIRecord.LinkIndex, ChartAIRecord>();
  private int m_iChartGroup;
  private WorkbookImpl m_book;
  private ChartSeriesRecord m_series;
  private ChartImpl m_chart;
  private ChartSeriesCollection m_seriesColl;
  private int m_iIndex;
  private int m_iOrder;
  private bool m_bDefaultName = true;
  private ChartDataPointsCollection m_dataPoints;
  private OfficeChartType m_serieType;
  private List<BiffRecordRaw> m_valueEnteredDirectly = new List<BiffRecordRaw>();
  private List<BiffRecordRaw> m_categoryEnteredDirectly = new List<BiffRecordRaw>();
  private List<BiffRecordRaw> m_bubbleEnteredDirectly = new List<BiffRecordRaw>();
  private object[] m_enteredDirectlyValue;
  private object[] m_enteredDirectlyCategory;
  private object[] m_enteredDirectlyBubble;
  private IRange m_nameRange;
  private string m_seriesText;
  private ChartErrorBarsImpl m_errorBarY;
  private ChartErrorBarsImpl m_errorBarX;
  private ChartTrendLineCollection m_trendLines;
  private bool? m_bInvertIfNegative = new bool?();
  private string m_strRefFormula;
  private string m_numRefFormula;
  private string m_MulLvlStrRefFormula;
  private Stream m_dropLinesStream;
  private bool m_IsFiltered;
  private string m_categoryFilteredRange;
  private string m_categoryValue;
  private string m_grouping;
  private int m_gapWidth;
  private int m_overlap;
  private bool m_bShowGapWidth;
  private int[] m_serieIndexList;
  private int[] m_categoryIndexList;
  private string m_formatCodeForNum;
  internal Stream extentsStream;
  private bool _hasColumnShape;
  private ChartColor m_invertIfNegativeColor = (ChartColor) ColorExtension.White;
  internal Stream m_invertFillFormatStream;
  private Dictionary<int, object[]> m_multiLevelStringCache;
  private Dictionary<int, string> m_formatValueCodes;
  private Dictionary<int, string> m_formatCategoryCodes;
  private int m_pointCount;
  private bool m_hasLeaderLines;
  private ChartBorderImpl m_leaderLines;
  private bool m_IsValidValueRange = true;
  private bool m_IsValidCategoryRange = true;
  private int m_paretoLineFormatIndex = -1;
  private bool m_isParetoLineHidden;
  private ChartFrameFormatImpl m_paretoLineFormat;
  private bool m_isRowWiseCategory;
  private bool m_isRowWiseSeries;
  private string m_formatCode;
  private string m_categoryFormatCode;

  public event ValueChangedEventHandler ValueRangeChanged;

  public ChartSerieImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.InitializeCollections();
  }

  [CLSCompliant(false)]
  public ChartSerieImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent)
  {
    this.Parse(data, ref iPos);
  }

  public string SerieName
  {
    get => this.m_serieName;
    set => this.m_serieName = value;
  }

  public string Name
  {
    get => this.DetectSerieName();
    set
    {
      if (!(this.m_strName != value))
        return;
      this.OnNameChanged(value);
      this.m_strName = value;
      this.m_bDefaultName = false;
      if (value != null && value.Length > 0 && value[0] == '=' && ChartImpl.TryAndModifyToValidFormula(this.m_strName))
        this.m_nameTokens = this.GetNameTokens();
      if (this.m_chart.Loading || this.m_chart.ChartTitle == null || !(this.m_chart.ChartTitle == "Chart Title") && this.m_chart.ChartTitle.Length != 0)
        return;
      ChartFormatImpl.GetStartSerieType(this.m_chart.ChartType);
      if (this.m_bDefaultName)
        return;
      bool? hasAutoTitle = this.m_chart.HasAutoTitle;
      if ((hasAutoTitle.GetValueOrDefault() ? 0 : (hasAutoTitle.HasValue ? 1 : 0)) == 0 || this.ParentSeries.Count != 1)
        return;
      this.m_chart.ChartTitle = this.Name;
    }
  }

  public IRange NameRangeIRange
  {
    get
    {
      this.DetectSerieName();
      return this.m_nameRange;
    }
  }

  public IOfficeDataRange NameRange
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.NameRangeIRange
      };
    }
  }

  public IRange ValuesIRange
  {
    get => this.m_ValueRange;
    set
    {
      if (this.m_ValueRange == value)
        return;
      this.m_valueEnteredDirectly.Clear();
      ValueChangedEventArgs e = new ValueChangedEventArgs((object) this.m_ValueRange, (object) value, "ValueRange");
      this.m_ValueRange = value;
      this.UpdateChartExSerieRangesMembers(true);
      this.OnValueRangeChanged(e);
    }
  }

  internal bool IsValidValueRange
  {
    get => this.m_IsValidValueRange;
    set => this.m_IsValidValueRange = value;
  }

  public IOfficeDataRange Values
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.ValuesIRange
      };
    }
    set
    {
      this.ValuesIRange = (value as ChartDataRange).SheetImpl[value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public IRange CategoryLabelsIRange
  {
    get => this.m_CategoryRange;
    set
    {
      if (this.m_CategoryRange == value)
        return;
      this.m_categoryEnteredDirectly.Clear();
      this.m_CategoryRange = value;
      if (!this.InnerWorkbook.IsWorkbookOpening && !this.ParentChart.IsSeriesInRows && value != null && value.Columns.Length > 1)
      {
        this.m_MulLvlStrRefFormula = value.AddressGlobal;
        (this.m_CategoryRange as RangeImpl).IsMultiReference = true;
      }
      else if (!this.InnerWorkbook.IsWorkbookOpening && (this.m_MulLvlStrRefFormula != null || value == null))
        this.m_MulLvlStrRefFormula = (string) null;
      this.UpdateChartExSerieRangesMembers(false);
      this.OnCategoryRangeChanged();
    }
  }

  internal bool IsValidCategoryRange
  {
    get => this.m_IsValidCategoryRange;
    set => this.m_IsValidCategoryRange = value;
  }

  public IOfficeDataRange CategoryLabels
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.CategoryLabelsIRange
      };
    }
    set
    {
      ChartDataRange chartDataRange = value as ChartDataRange;
      if (chartDataRange.Range == null)
      {
        this.CategoryLabelsIRange = (IRange) null;
      }
      else
      {
        int firstRow = value.FirstRow;
        int firstColumn = value.FirstColumn;
        int lastRow = value.LastRow;
        int lastColumn = value.LastColumn;
        this.CategoryLabelsIRange = chartDataRange.SheetImpl[firstRow, firstColumn, lastRow, lastColumn];
      }
    }
  }

  public IRange BubblesIRange
  {
    get => this.m_BubbleRange;
    set
    {
      if (this.m_BubbleRange == value)
        return;
      this.m_bubbleEnteredDirectly.Clear();
      this.m_BubbleRange = value;
      this.OnBubbleRangeChanged();
    }
  }

  public IOfficeDataRange Bubbles
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.BubblesIRange
      };
    }
    set
    {
      this.BubblesIRange = (value as ChartDataRange).SheetImpl[value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public int RealIndex
  {
    get => this.Index;
    set => this.Index = value;
  }

  public IOfficeChartDataPoints DataPoints
  {
    get
    {
      if (this.m_dataPoints == null)
        this.m_dataPoints = new ChartDataPointsCollection(this.Application, (object) this);
      return (IOfficeChartDataPoints) this.m_dataPoints;
    }
  }

  public IOfficeChartSerieDataFormat SerieFormat => this.m_dataPoints.DefaultDataPoint.DataFormat;

  public OfficeChartType SerieType
  {
    get => this.DetectSerieType();
    set
    {
      this.ChangeSeriesType(value, false);
      (this.SerieFormat as ChartSerieDataFormatImpl).HasMarkerProperties = true;
    }
  }

  public bool UsePrimaryAxis
  {
    get => this.GetCommonSerieFormat().IsPrimaryAxis;
    set
    {
      if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_CHANGE_SERIE, this.SerieType) == -1)
        throw new NotSupportedException("Property not supported for current serie type");
      if (value != this.UsePrimaryAxis)
      {
        this.ChangeAxis(value);
        this.m_chart.IsManuallyFormatted = true;
      }
      if (!value)
        this.m_chart.SecondaryParentAxis.UpdateSecondaryAxis(true);
      if (this.m_seriesColl.HasSecondary())
        return;
      this.m_chart.RemoveSecondaryAxes();
    }
  }

  internal object[] EnteredDirectlyValues
  {
    get
    {
      if (this.m_enteredDirectlyValue == null)
        this.m_enteredDirectlyValue = this.GetEnteredDirectlyValues(this.m_valueEnteredDirectly);
      return this.m_enteredDirectlyValue;
    }
    set => this.m_enteredDirectlyValue = value;
  }

  internal object[] EnteredDirectlyCategoryLabels
  {
    get
    {
      if (this.m_enteredDirectlyCategory == null)
        this.m_enteredDirectlyCategory = this.GetEnteredDirectlyValues(this.m_categoryEnteredDirectly);
      return this.m_enteredDirectlyCategory;
    }
    set => this.m_enteredDirectlyCategory = value;
  }

  internal object[] EnteredDirectlyBubbles
  {
    get
    {
      if (this.m_enteredDirectlyBubble == null)
        this.m_enteredDirectlyBubble = this.GetEnteredDirectlyValues(this.m_bubbleEnteredDirectly);
      return this.m_enteredDirectlyBubble;
    }
    set => this.m_enteredDirectlyBubble = value;
  }

  internal Dictionary<int, object> DataLabelCellsValues
  {
    get => this.m_dataLabelCellsValues;
    set => this.m_dataLabelCellsValues = value;
  }

  internal bool HasColumnShape
  {
    get => this._hasColumnShape;
    set => this._hasColumnShape = value;
  }

  public IOfficeChartErrorBars ErrorBarsY
  {
    get
    {
      return this.m_errorBarY != null ? (IOfficeChartErrorBars) this.m_errorBarY : throw new ApplicationException("Use HasErrorBarsY property to create error bars.");
    }
  }

  public bool HasErrorBarsY
  {
    get => this.m_errorBarY != null;
    set
    {
      if (this.HasErrorBarsY == value)
        return;
      if (!value)
      {
        this.m_errorBarY = (ChartErrorBarsImpl) null;
      }
      else
      {
        string startSerieType = ChartFormatImpl.GetStartSerieType(this.SerieType);
        if (this.m_chart.IsChart3D || Array.IndexOf<string>(ChartImpl.DEF_SUPPORT_ERROR_BARS, startSerieType) == -1)
          throw new NotSupportedException("Current serie doesnot support Y error bars.");
        if (this.m_errorBarY != null)
          return;
        this.m_errorBarY = new ChartErrorBarsImpl(this.Application, (object) this, true);
      }
    }
  }

  public IOfficeChartErrorBars ErrorBarsX
  {
    get
    {
      return this.m_errorBarX != null ? (IOfficeChartErrorBars) this.m_errorBarX : throw new ApplicationException("Use HasErrorBarsX property to create error bars.");
    }
  }

  public bool HasErrorBarsX
  {
    get => this.m_errorBarX != null;
    set
    {
      if (this.HasErrorBarsX == value)
        return;
      if (!value)
      {
        this.m_errorBarX = (ChartErrorBarsImpl) null;
      }
      else
      {
        if (ChartFormatImpl.GetStartSerieType(this.SerieType) != "Scatter" && ChartFormatImpl.GetStartSerieType(this.SerieType) != "Bubble")
          throw new NotSupportedException("Current serie doesnot support X error bars.");
        if (this.m_errorBarX != null)
          return;
        this.m_errorBarX = new ChartErrorBarsImpl(this.Application, (object) this, false);
      }
    }
  }

  public IOfficeChartTrendLines TrendLines => (IOfficeChartTrendLines) this.m_trendLines;

  internal string Grouping
  {
    get => this.m_grouping;
    set => this.m_grouping = value;
  }

  internal int GapWidth
  {
    get => this.m_gapWidth;
    set => this.m_gapWidth = value;
  }

  internal int Overlap
  {
    get => this.m_overlap;
    set => this.m_overlap = value;
  }

  internal bool ShowGapWidth
  {
    get => this.m_bShowGapWidth;
    set => this.m_bShowGapWidth = value;
  }

  public string FormatCode
  {
    get => this.m_formatCodeForNum;
    set
    {
      if (string.IsNullOrEmpty(value))
        return;
      this.m_formatCodeForNum = value;
    }
  }

  internal Dictionary<int, string> FormatValueCodes
  {
    get
    {
      if (this.m_formatValueCodes == null)
        this.m_formatValueCodes = new Dictionary<int, string>();
      return this.m_formatValueCodes;
    }
    set => this.m_formatValueCodes = value;
  }

  internal Dictionary<int, string> FormatCategoryCodes
  {
    get
    {
      if (this.m_formatCategoryCodes == null)
        this.m_formatCategoryCodes = new Dictionary<int, string>();
      return this.m_formatCategoryCodes;
    }
    set => this.m_formatCategoryCodes = value;
  }

  internal Dictionary<int, object[]> MultiLevelStrCache
  {
    get
    {
      if (this.m_multiLevelStringCache == null)
        this.m_multiLevelStringCache = new Dictionary<int, object[]>();
      return this.m_multiLevelStringCache;
    }
    set => this.m_multiLevelStringCache = value;
  }

  internal int PointCount
  {
    get => this.m_pointCount;
    set => this.m_pointCount = value;
  }

  public IOfficeChartErrorBars ErrorBar(bool bIsY)
  {
    return this.ErrorBar(bIsY, OfficeErrorBarInclude.Both);
  }

  public IOfficeChartErrorBars ErrorBar(bool bIsY, OfficeErrorBarInclude include)
  {
    return this.ErrorBar(bIsY, include, OfficeErrorBarType.Fixed);
  }

  public IOfficeChartErrorBars ErrorBar(
    bool bIsY,
    OfficeErrorBarInclude include,
    OfficeErrorBarType type)
  {
    double numberValue = bIsY ? 10.0 : 1.0;
    return this.ErrorBar(bIsY, include, type, numberValue);
  }

  public IOfficeChartErrorBars ErrorBar(
    bool bIsY,
    OfficeErrorBarInclude include,
    OfficeErrorBarType type,
    double numberValue)
  {
    if (type == OfficeErrorBarType.Custom)
      throw new ArgumentException("For sets custom type use another overload method");
    ChartErrorBarsImpl chartErrorBarsImpl;
    if (bIsY)
    {
      this.HasErrorBarsY = true;
      chartErrorBarsImpl = this.m_errorBarY;
    }
    else
    {
      this.HasErrorBarsX = true;
      chartErrorBarsImpl = this.m_errorBarX;
    }
    chartErrorBarsImpl.Type = type;
    chartErrorBarsImpl.Include = include;
    chartErrorBarsImpl.NumberValue = numberValue;
    chartErrorBarsImpl.Border.AutoFormat = true;
    chartErrorBarsImpl.HasCap = true;
    return (IOfficeChartErrorBars) chartErrorBarsImpl;
  }

  public IOfficeChartErrorBars ErrorBar(
    bool bIsY,
    IOfficeDataRange plusRange,
    IOfficeDataRange minusRange)
  {
    bool flag1 = plusRange != null;
    bool flag2 = minusRange != null;
    if (!flag1 && !flag2)
      throw new ArgumentException("Plus range and minus range are null referance.");
    ChartErrorBarsImpl chartErrorBarsImpl;
    if (bIsY)
    {
      this.HasErrorBarsY = true;
      chartErrorBarsImpl = this.m_errorBarY;
      chartErrorBarsImpl.NumberValue = 10.0;
    }
    else
    {
      this.HasErrorBarsX = true;
      chartErrorBarsImpl = this.m_errorBarX;
      chartErrorBarsImpl.NumberValue = 1.0;
    }
    if (flag1)
    {
      chartErrorBarsImpl.PlusIRange = (plusRange as ChartDataRange).Range;
      List<object> objectList = new List<object>();
      for (int index = 0; index < (plusRange as ChartDataRange).Range.Count; ++index)
        objectList.Add((object) double.Parse((plusRange as ChartDataRange).Range.Cells[index].DisplayText));
      object[] array = objectList.ToArray();
      chartErrorBarsImpl.PlusRangeValues = array;
    }
    if (flag2)
    {
      chartErrorBarsImpl.MinusIRange = (minusRange as ChartDataRange).Range;
      List<object> objectList = new List<object>();
      for (int index = 0; index < (minusRange as ChartDataRange).Range.Count; ++index)
        objectList.Add((object) double.Parse((minusRange as ChartDataRange).Range.Cells[index].DisplayText));
      object[] array = objectList.ToArray();
      chartErrorBarsImpl.MinusRangeValues = array;
    }
    chartErrorBarsImpl.Border.AutoFormat = true;
    chartErrorBarsImpl.HasCap = true;
    return (IOfficeChartErrorBars) chartErrorBarsImpl;
  }

  private void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartSeries);
    this.ParseSeriesRecord((ChartSeriesRecord) data[iPos]);
    ++iPos;
    if (data[iPos].TypeCode != TBIFFRecord.Begin)
      throw new ArgumentOutOfRangeException("Begin record was expected.");
    ++iPos;
    this.m_hashAi.Clear();
    this.m_dataPoints.Clear();
    for (BiffRecordRaw biffRecordRaw = data[iPos]; biffRecordRaw.TypeCode != TBIFFRecord.End; biffRecordRaw = data[iPos])
    {
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.ChartDataFormat:
          this.m_iOrder = (int) ((ChartDataFormatRecord) biffRecordRaw).SeriesNumber;
          ChartSerieDataFormatImpl dataFormat = new ChartSerieDataFormatImpl(this.Application, (object) this);
          iPos = dataFormat.Parse(data, iPos);
          int pointNumber = (int) dataFormat.DataFormat.PointNumber;
          this.SetDataFormat(dataFormat);
          break;
        case TBIFFRecord.ChartSeriesText:
          if (this.m_seriesColl.Count == 0)
            this.m_seriesText = ((ChartSeriesTextRecord) biffRecordRaw).Text;
          ++iPos;
          break;
        case TBIFFRecord.ChartLegendxn:
          this.ParseLegendEntries(data, ref iPos);
          break;
        case TBIFFRecord.ChartSertocrt:
          this.ParseSertoCrt(data, ref iPos);
          break;
        case TBIFFRecord.ChartAI:
          this.ParseAIRecord(data, ref iPos);
          break;
        default:
          ++iPos;
          break;
      }
    }
    ++this.m_seriesColl.TrendIndex;
    ++iPos;
    this.Reparse();
  }

  private void ParseSeriesRecord(ChartSeriesRecord series) => this.m_series = series;

  private void ParseAIRecord(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    ChartAIRecord recordAi = biffRecordRaw.TypeCode == TBIFFRecord.ChartAI ? (ChartAIRecord) biffRecordRaw : throw new ArgumentOutOfRangeException("ChartAI record was expected.");
    ++iPos;
    if (this.m_hashAi.ContainsKey(recordAi.IndexIdentifier))
      throw new ArgumentException("AI record with such IndexIdentifier was already read.");
    if (recordAi.IndexIdentifier == ChartAIRecord.LinkIndex.LinkToTitleOrText)
      this.GetTitle(recordAi, data, ref iPos);
    this.m_hashAi.Add(recordAi.IndexIdentifier, recordAi);
  }

  private void ParseSertoCrt(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartSertocrt);
    this.m_iChartGroup = (int) ((ChartSertocrtRecord) biffRecordRaw).ChartGroup;
    ++iPos;
  }

  private void GetTitle(ChartAIRecord recordAi, IList<BiffRecordRaw> data, ref int iPos)
  {
    if (recordAi.Reference == ChartAIRecord.ReferenceType.EnteredDirectly || recordAi.Reference == ChartAIRecord.ReferenceType.NotUsed)
    {
      BiffRecordRaw biffRecordRaw = data[iPos];
      if (biffRecordRaw.TypeCode == TBIFFRecord.ChartSeriesText)
      {
        this.m_strName = ((ChartSeriesTextRecord) biffRecordRaw).Text;
        this.m_seriesText = this.m_strName;
        this.m_bDefaultName = false;
        ++iPos;
      }
    }
    if (recordAi.Reference != ChartAIRecord.ReferenceType.Worksheet)
      return;
    Ptg[] parsedExpression = recordAi.ParsedExpression;
    this.m_strName = "=" + this.m_book.FormulaUtil.ParsePtgArray(parsedExpression);
    this.m_nameTokens = parsedExpression;
    this.m_bDefaultName = false;
  }

  private void ParseLegendEntries(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.m_chart.HasLegend = true;
    ChartLegendEntriesColl legendEntries = (ChartLegendEntriesColl) this.m_chart.Legend.LegendEntries;
    ChartLegendEntryImpl entry = new ChartLegendEntryImpl(this.Application, (object) legendEntries, 0);
    entry.Parse(data, ref iPos);
    int iIndex = entry.LegendEntityIndex == (int) ushort.MaxValue ? this.m_seriesColl.Count : entry.LegendEntityIndex;
    legendEntries.Add(iIndex, entry);
  }

  public void ParseErrorBars(IList<BiffRecordRaw> data)
  {
    ChartErrorBarsImpl bar = data != null ? new ChartErrorBarsImpl(this.Application, (object) this, data) : throw new ArgumentNullException(nameof (data));
    if (bar.IsY)
      this.UpdateErrorBar(bar, ref this.m_errorBarY);
    else
      this.UpdateErrorBar(bar, ref this.m_errorBarX);
  }

  internal void SetLeaderLines(bool value) => this.m_hasLeaderLines = value;

  private void SetParents()
  {
    this.m_book = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ArgumentNullException("Can't find parent workbook."));
    this.m_chart = (ChartImpl) (this.FindParent(typeof (ChartImpl)) ?? throw new ArgumentNullException("Can't find parent chart."));
    this.m_seriesColl = (ChartSeriesCollection) (this.FindParent(typeof (ChartSeriesCollection)) ?? throw new ArgumentNullException("Can't find parent series collection."));
  }

  private void InitializeHashAIMember()
  {
    ChartAIRecord record = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
    record.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToTitleOrText;
    record.Reference = ChartAIRecord.ReferenceType.NotUsed;
    this.m_hashAi.Add(record.IndexIdentifier, record);
    ChartAIRecord chartAiRecord1 = (ChartAIRecord) record.Clone();
    chartAiRecord1.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToCategories;
    chartAiRecord1.Reference = ChartAIRecord.ReferenceType.DefaultCategories;
    this.m_hashAi.Add(chartAiRecord1.IndexIdentifier, chartAiRecord1);
    ChartAIRecord chartAiRecord2 = (ChartAIRecord) chartAiRecord1.Clone();
    chartAiRecord2.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToValues;
    chartAiRecord2.Reference = ChartAIRecord.ReferenceType.NotUsed;
    this.m_hashAi.Add(chartAiRecord2.IndexIdentifier, chartAiRecord2);
    ChartAIRecord chartAiRecord3 = (ChartAIRecord) chartAiRecord2.Clone();
    chartAiRecord3.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToBubbles;
    chartAiRecord3.Reference = ChartAIRecord.ReferenceType.NotUsed;
    this.m_hashAi.Add(chartAiRecord3.IndexIdentifier, chartAiRecord3);
  }

  private void InitializeCollections()
  {
    this.InitializeHashAIMember();
    this.m_series = (ChartSeriesRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeries);
    this.SetDataFormat(new ChartSerieDataFormatImpl(this.Application, (object) this)
    {
      DataFormat = {
        PointNumber = ushort.MaxValue
      }
    });
    if (!this.ParentBook.IsLoaded)
    {
      switch (this.m_chart.ChartType)
      {
        case OfficeChartType.Pareto:
          this.m_paretoLineFormat = new ChartFrameFormatImpl(this.Application, (object) this);
          break;
        case OfficeChartType.TreeMap:
        case OfficeChartType.SunBurst:
          this.DataPoints.DefaultDataPoint.DataLabels.IsCategoryName = true;
          this.DataPoints.DefaultDataPoint.DataLabels.Delimiter = ",";
          this.DataPoints.DefaultDataPoint.DataLabels.Position = OfficeDataLabelPosition.Inside;
          break;
      }
    }
    this.m_trendLines = new ChartTrendLineCollection(this.Application, (object) this);
  }

  private void SetDataFormat(ChartSerieDataFormatImpl dataFormat)
  {
    if (dataFormat == null)
      throw new ArgumentNullException(nameof (dataFormat));
    ChartDataPointImpl dataPoint = (ChartDataPointImpl) this.DataPoints[(int) dataFormat.DataFormat.PointNumber];
    dataPoint.InnerDataFormat = dataFormat;
    dataFormat.SetParent((object) dataPoint);
  }

  [CLSCompliant(false)]
  public Chart3DDataFormatRecord Get3DDataFormat()
  {
    Chart3DDataFormatRecord record = (Chart3DDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
    switch (this.m_chart.ChartType)
    {
      case OfficeChartType.Cylinder_Clustered:
      case OfficeChartType.Cylinder_Stacked:
      case OfficeChartType.Cylinder_Stacked_100:
      case OfficeChartType.Cylinder_Bar_Clustered:
      case OfficeChartType.Cylinder_Bar_Stacked:
      case OfficeChartType.Cylinder_Bar_Stacked_100:
      case OfficeChartType.Cylinder_Clustered_3D:
        record.DataFormatBase = OfficeBaseFormat.Circle;
        record.DataFormatTop = OfficeTopFormat.Straight;
        break;
      case OfficeChartType.Cone_Clustered:
      case OfficeChartType.Cone_Stacked:
      case OfficeChartType.Cone_Bar_Clustered:
      case OfficeChartType.Cone_Bar_Stacked:
      case OfficeChartType.Cone_Clustered_3D:
        record.DataFormatBase = OfficeBaseFormat.Circle;
        record.DataFormatTop = OfficeTopFormat.Sharp;
        break;
      case OfficeChartType.Cone_Stacked_100:
      case OfficeChartType.Cone_Bar_Stacked_100:
        record.DataFormatBase = OfficeBaseFormat.Circle;
        record.DataFormatTop = OfficeTopFormat.Trunc;
        break;
      case OfficeChartType.Pyramid_Clustered:
      case OfficeChartType.Pyramid_Stacked:
      case OfficeChartType.Pyramid_Bar_Clustered:
      case OfficeChartType.Pyramid_Bar_Stacked:
      case OfficeChartType.Pyramid_Clustered_3D:
        record.DataFormatBase = OfficeBaseFormat.Rectangle;
        record.DataFormatTop = OfficeTopFormat.Sharp;
        break;
      case OfficeChartType.Pyramid_Stacked_100:
      case OfficeChartType.Pyramid_Bar_Stacked_100:
        record.DataFormatBase = OfficeBaseFormat.Rectangle;
        record.DataFormatTop = OfficeTopFormat.Trunc;
        break;
    }
    return record;
  }

  private void OnNameChanged(string value)
  {
    if (value != null && value.Length > 0 && value[0] == '=')
    {
      value = value.Substring(1);
      try
      {
        this.m_nameRange = ((IRangeGetter) this.m_book.FormulaUtil.ParseString(value)[0]).GetRange((IWorkbook) this.m_book, (IWorksheet) null);
      }
      catch
      {
      }
      if (this.m_nameRange != null && this.m_nameRange.Row != this.m_nameRange.LastRow && this.m_nameRange.Column != this.m_nameRange.LastColumn)
        throw new NotSupportedException("Reference must be a single cell, row, or column.");
    }
    else
    {
      if (this.m_nameRange == null || this.m_nameRange is ExternalRange)
        return;
      this.m_nameRange.Value2 = (object) value;
    }
  }

  private void OnValueRangeChanged(ValueChangedEventArgs e)
  {
    this.SetAIRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToValues], this.m_ValueRange, ChartAIRecord.ReferenceType.EnteredDirectly);
    if (this.ValueRangeChanged == null)
      return;
    this.ValueRangeChanged((object) this, e);
  }

  private void OnCategoryRangeChanged()
  {
    this.SetAIRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToCategories], this.m_CategoryRange, ChartAIRecord.ReferenceType.DefaultCategories);
  }

  private void OnBubbleRangeChanged()
  {
    this.SetAIRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToBubbles], this.m_BubbleRange, ChartAIRecord.ReferenceType.DefaultCategories);
  }

  public ChartSerieImpl Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
  {
    ChartSerieImpl parent1 = new ChartSerieImpl(this.Application, parent);
    parent1.m_bDefaultName = this.m_bDefaultName;
    parent1.m_bIsDisposed = this.m_bIsDisposed;
    parent1.m_hashAi = new Dictionary<ChartAIRecord.LinkIndex, ChartAIRecord>();
    parent1.InitializeHashAIMember();
    if (this.GetSerieNameRange() != null)
      parent1.m_nameRange = ((ICombinedRange) this.m_nameRange).Clone((object) parent1, hashNewNames, parent1.m_book);
    if (this.m_ValueRange != null)
    {
      IRange range = ((ICombinedRange) this.m_ValueRange).Clone((object) parent1, hashNewNames, parent1.m_book);
      parent1.ValuesIRange = range;
      if (this.m_ValueRange is RangeImpl)
      {
        (parent1.ValuesIRange as RangeImpl).IsMultiReference = (this.m_ValueRange as RangeImpl).IsMultiReference;
        (parent1.ValuesIRange as RangeImpl).IsNumReference = (this.m_ValueRange as RangeImpl).IsNumReference;
        (parent1.ValuesIRange as RangeImpl).IsStringReference = (this.m_ValueRange as RangeImpl).IsStringReference;
      }
      else if (this.m_ValueRange is NameImpl)
      {
        (parent1.ValuesIRange as NameImpl).IsMultiReference = (this.m_ValueRange as NameImpl).IsMultiReference;
        (parent1.ValuesIRange as NameImpl).IsNumReference = (this.m_ValueRange as NameImpl).IsNumReference;
        (parent1.ValuesIRange as NameImpl).IsStringReference = (this.m_ValueRange as NameImpl).IsStringReference;
      }
      else if (this.m_ValueRange is ExternalRange)
      {
        (parent1.ValuesIRange as ExternalRange).IsMultiReference = (this.m_ValueRange as ExternalRange).IsMultiReference;
        (parent1.ValuesIRange as ExternalRange).IsNumReference = (this.m_ValueRange as ExternalRange).IsNumReference;
        (parent1.ValuesIRange as ExternalRange).IsStringReference = (this.m_ValueRange as ExternalRange).IsStringReference;
      }
    }
    if (this.m_BubbleRange != null)
      parent1.BubblesIRange = ((ICombinedRange) this.m_BubbleRange).Clone((object) parent1, hashNewNames, parent1.m_book);
    if (this.m_CategoryRange != null)
    {
      parent1.CategoryLabelsIRange = ((ICombinedRange) this.m_CategoryRange).Clone((object) parent1, hashNewNames, parent1.m_book);
      if (this.m_CategoryRange is RangeImpl)
      {
        (parent1.CategoryLabelsIRange as RangeImpl).IsMultiReference = (this.m_CategoryRange as RangeImpl).IsMultiReference;
        (parent1.CategoryLabelsIRange as RangeImpl).IsNumReference = (this.m_CategoryRange as RangeImpl).IsNumReference;
        (parent1.CategoryLabelsIRange as RangeImpl).IsStringReference = (this.m_CategoryRange as RangeImpl).IsStringReference;
      }
      else if (this.m_CategoryRange is NameImpl)
      {
        (parent1.CategoryLabelsIRange as NameImpl).IsMultiReference = (this.m_CategoryRange as NameImpl).IsMultiReference;
        (parent1.CategoryLabelsIRange as NameImpl).IsNumReference = (this.m_CategoryRange as NameImpl).IsNumReference;
        (parent1.CategoryLabelsIRange as NameImpl).IsStringReference = (this.m_CategoryRange as NameImpl).IsStringReference;
      }
      else if (this.m_CategoryRange is ExternalRange)
      {
        (parent1.CategoryLabelsIRange as ExternalRange).IsMultiReference = (this.m_CategoryRange as ExternalRange).IsMultiReference;
        (parent1.CategoryLabelsIRange as ExternalRange).IsNumReference = (this.m_CategoryRange as ExternalRange).IsNumReference;
        (parent1.CategoryLabelsIRange as ExternalRange).IsStringReference = (this.m_CategoryRange as ExternalRange).IsStringReference;
      }
    }
    if (this.m_nameRange != null)
      parent1.m_nameRange = ((ICombinedRange) this.m_nameRange).Clone((object) parent1, hashNewNames, parent1.m_book);
    if (this.m_dataPoints != null)
      parent1.m_dataPoints = (ChartDataPointsCollection) this.m_dataPoints.Clone((object) parent1, parent1.m_book, dicFontIndexes, hashNewNames);
    parent1.m_valueEnteredDirectly = CloneUtils.CloneCloneable(this.m_valueEnteredDirectly);
    parent1.m_categoryEnteredDirectly = CloneUtils.CloneCloneable(this.m_categoryEnteredDirectly);
    parent1.m_bubbleEnteredDirectly = CloneUtils.CloneCloneable(this.m_bubbleEnteredDirectly);
    parent1.m_enteredDirectlyValue = CloneUtils.CloneArray(this.m_enteredDirectlyValue);
    parent1.m_enteredDirectlyCategory = CloneUtils.CloneArray(this.m_enteredDirectlyCategory);
    parent1.m_enteredDirectlyBubble = CloneUtils.CloneArray(this.m_enteredDirectlyBubble);
    parent1.NumRefFormula = this.m_numRefFormula;
    parent1.m_bInvertIfNegative = this.m_bInvertIfNegative;
    parent1.m_iChartGroup = this.m_iChartGroup;
    parent1.m_iIndex = this.m_iIndex;
    parent1.m_iOrder = this.m_iOrder;
    parent1.m_series = (ChartSeriesRecord) this.m_series.Clone();
    parent1.m_strName = this.m_strName;
    parent1.m_trendLines = this.m_trendLines.Clone((object) parent1, dicFontIndexes, hashNewNames);
    parent1.m_serieName = this.m_serieName;
    if (this.m_errorBarX != null)
      parent1.m_errorBarX = this.m_errorBarX.Clone((object) parent1, hashNewNames);
    if (this.m_leaderLines != null)
      parent1.m_leaderLines = this.m_leaderLines.Clone((object) parent1);
    if (this.m_errorBarY != null)
      parent1.m_errorBarY = this.m_errorBarY.Clone((object) parent1, hashNewNames);
    if (this.m_nameTokens != null)
      parent1.m_nameTokens = CloneUtils.ClonePtgArray(this.m_nameTokens);
    if (this.extentsStream != null)
    {
      this.extentsStream.Position = 0L;
      parent1.extentsStream = CloneUtils.CloneStream(this.extentsStream);
    }
    if (this.m_invertFillFormatStream != null)
    {
      this.m_invertFillFormatStream.Position = 0L;
      parent1.m_invertFillFormatStream = CloneUtils.CloneStream(this.m_invertFillFormatStream);
    }
    if (this.m_dropLinesStream != null)
    {
      this.m_dropLinesStream.Position = 0L;
      parent1.m_dropLinesStream = CloneUtils.CloneStream(this.m_dropLinesStream);
    }
    if (this.m_multiLevelStringCache != null)
      parent1.m_multiLevelStringCache = CloneUtils.CloneHash<int, object[]>(this.m_multiLevelStringCache);
    if (this.m_invertIfNegativeColor != (ChartColor) null)
      parent1.m_invertIfNegativeColor = this.m_invertIfNegativeColor.Clone();
    if (this.m_book != null)
      parent1.m_book = (WorkbookImpl) this.m_book.Clone();
    parent1.m_serieType = this.m_serieType;
    parent1.m_hasLeaderLines = this.m_hasLeaderLines;
    parent1.m_pointCount = this.m_pointCount;
    parent1.m_IsValidCategoryRange = this.m_IsValidCategoryRange;
    parent1.m_IsValidValueRange = this.m_IsValidValueRange;
    return parent1;
  }

  private string GetWorkSheetNameByAddress(string strAddress)
  {
    int num = strAddress != null ? strAddress.IndexOf("'!") : throw new ArgumentNullException(nameof (strAddress));
    return strAddress.Substring(1, num - 1);
  }

  private void ChangeAxis(bool bToPrimary)
  {
    if (!bToPrimary && this.m_seriesColl.Count == 1)
      throw new ArgumentException("Can't set current serie to secondary axis");
    int chartGroup = this.ChartGroup;
    ChartFormatImpl commonSerieFormat = this.GetCommonSerieFormat();
    int newOrder = this.GetNewOrder(bToPrimary);
    int sameDrawingOrder = this.m_seriesColl.GetCountOfSeriesWithSameDrawingOrder(chartGroup);
    int withSameStartType = this.m_seriesColl.GetCountOfSeriesWithSameStartType(this.SerieType);
    if (newOrder < 0)
      throw new ApplicationException("Can't set current serie to secondary axis");
    if (sameDrawingOrder != withSameStartType)
    {
      OfficeChartType serieType = this.SerieType;
      ChartFormatCollection parent = bToPrimary ? this.m_chart.PrimaryFormats : this.m_chart.SecondaryFormats;
      ChartFormatImpl formatToAdd = (ChartFormatImpl) commonSerieFormat.Clone((object) parent);
      this.ChartGroup = parent.FindOrAdd(formatToAdd).DrawingZOrder;
      if (sameDrawingOrder == 1)
        this.m_chart.PrimaryParentAxis.Formats.Remove(commonSerieFormat);
      if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_CHANGE_INTIMATE, serieType) != -1)
      {
        this.SerieType = serieType;
      }
      else
      {
        if (this.m_chart.ChartType != OfficeChartType.Combination_Chart)
          return;
        this.ChangeSeriesType(serieType, false, true);
      }
    }
    else
    {
      bool bAdd = sameDrawingOrder != 1;
      this.m_chart.PrimaryParentAxis.Formats.ChangeShallowAxis(bToPrimary, chartGroup, bAdd, newOrder);
      if (!bAdd)
        return;
      this.ChartGroup = newOrder;
    }
  }

  private int GetNewOrder(bool bToPrimary)
  {
    ChartGlobalFormatsCollection formats = this.m_chart.PrimaryParentAxis.Formats;
    ChartFormatCollection primaryFormats = formats.PrimaryFormats;
    ChartFormatCollection secondaryFormats = formats.SecondaryFormats;
    int newOrder = -1;
    for (int index = 0; index < 8; ++index)
    {
      if (!primaryFormats.ContainsIndex(index) && !secondaryFormats.ContainsIndex(index))
      {
        newOrder = index;
        break;
      }
    }
    return newOrder;
  }

  [CLSCompliant(false)]
  public void AddEnteredRecord(int siIndex, ICellPositionFormat record)
  {
    if (siIndex > 3 || siIndex < 1)
      throw new ArgumentOutOfRangeException(nameof (siIndex));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (siIndex)
    {
      case 1:
        if (this.m_ValueRange != null)
          break;
        this.m_valueEnteredDirectly.Add(record as BiffRecordRaw);
        break;
      case 2:
        if (this.m_CategoryRange != null)
          break;
        this.m_categoryEnteredDirectly.Add(record as BiffRecordRaw);
        break;
      case 3:
        if (this.m_BubbleRange != null)
          break;
        this.m_bubbleEnteredDirectly.Add(record as BiffRecordRaw);
        break;
    }
  }

  public List<BiffRecordRaw> GetArray(int siIndex)
  {
    if (siIndex > 3 || siIndex < 1)
      throw new ArgumentOutOfRangeException(nameof (siIndex));
    List<BiffRecordRaw> biffRecordRawList = (List<BiffRecordRaw>) null;
    switch (siIndex)
    {
      case 1:
        biffRecordRawList = this.m_valueEnteredDirectly;
        break;
      case 2:
        biffRecordRawList = this.m_categoryEnteredDirectly;
        break;
      case 3:
        biffRecordRawList = this.m_bubbleEnteredDirectly;
        break;
    }
    return biffRecordRawList == null || biffRecordRawList.Count <= 0 ? (List<BiffRecordRaw>) null : biffRecordRawList;
  }

  public object[] GetEnteredDirectlyValues(List<BiffRecordRaw> array)
  {
    int capacity = array != null ? array.Count : throw new ArgumentNullException(nameof (array));
    if (capacity == 0)
      return (object[]) null;
    List<object> objectList = new List<object>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      BiffRecordRaw biffRecordRaw = array[index];
      object obj = (object) null;
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.Number:
          obj = (object) ((NumberRecord) biffRecordRaw).Value;
          break;
        case TBIFFRecord.Label:
          obj = (object) ((LabelRecord) biffRecordRaw).Label;
          break;
      }
      objectList.Add(obj);
    }
    return objectList.ToArray();
  }

  private bool GetEnteredDirectlyType(object[] array)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] is string)
        return false;
    }
    return true;
  }

  private List<BiffRecordRaw> GetArrayRecordsByValues(bool bIsNumber, object[] values)
  {
    int capacity = values != null ? values.Length : throw new ArgumentNullException(nameof (values));
    List<BiffRecordRaw> arrayRecordsByValues = new List<BiffRecordRaw>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      object obj = values[index];
      if (obj == null)
        throw new ApplicationException($"Null referance value in values array at {index.ToString()} position");
      if (obj is bool && !bIsNumber)
        obj = (bool) obj ? (object) "TRUE" : (object) "FALSE";
      ICellPositionFormat cellPositionFormat;
      if (bIsNumber)
      {
        NumberRecord record = (NumberRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Number);
        record.Value = obj is IConvertible ? Convert.ToDouble(obj) : throw new ApplicationException($"Bad value in values array at {index.ToString()} position");
        cellPositionFormat = (ICellPositionFormat) record;
      }
      else
      {
        LabelRecord record = (LabelRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Label);
        record.Label = obj is IConvertible ? Convert.ToString(obj) : throw new ApplicationException($"Bad value in values array at {index.ToString()} position");
        cellPositionFormat = (ICellPositionFormat) record;
      }
      cellPositionFormat.Column = (int) (ushort) this.Index;
      cellPositionFormat.Row = (int) (ushort) index;
      arrayRecordsByValues.Add((BiffRecordRaw) cellPositionFormat);
    }
    return arrayRecordsByValues;
  }

  private void UpdateSerieIndexesInEnteredDirectlyValues(List<BiffRecordRaw> arrayToUpdate)
  {
    if (arrayToUpdate == null)
      throw new ArgumentNullException(nameof (arrayToUpdate));
    int index1 = this.Index;
    int index2 = 0;
    for (int count = arrayToUpdate.Count; index2 < count; ++index2)
      ((ICellPositionFormat) arrayToUpdate[index2]).Column = (int) (ushort) index1;
  }

  private string DetectSerieName()
  {
    if (this.m_strName == null || this.m_strName.Length == 0 || this.m_strName[0] != '=')
      return this.m_strName;
    string str;
    if (this.m_nameRange != null)
    {
      str = this.GetTextRangeValue(this.m_nameRange);
    }
    else
    {
      this.m_strName.Substring(1);
      try
      {
        if (this.m_nameTokens != null)
        {
          this.m_nameRange = ((IRangeGetter) this.m_nameTokens[0]).GetRange((IWorkbook) this.m_book, (IWorksheet) null);
          str = this.m_nameRange != null ? this.GetTextRangeValue(this.m_nameRange) : "#REF!";
        }
        else
          str = this.m_strName = "#REF!";
      }
      catch
      {
        str = this.m_strName = "#REF!";
      }
    }
    return str;
  }

  private string GetTextRangeValue(IRange range)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    string str1 = "";
    int column1 = range.Column;
    int lastColumn = range.LastColumn;
    int row1 = range.Row;
    int lastRow = range.LastRow;
    if (range.Worksheet is WorksheetImpl)
    {
      IMigrantRange migrantRange = range.Worksheet.MigrantRange;
      int iColumn = column1;
      for (int index1 = lastColumn; iColumn <= index1; ++iColumn)
      {
        int iRow = row1;
        for (int index2 = lastRow; iRow <= index2; ++iRow)
        {
          migrantRange.ResetRowColumn(iRow, iColumn);
          string str2 = migrantRange.Value;
          if (str2 != null && str2.Length > 0)
            str1 = $"{str1}{str2} ";
        }
      }
    }
    else if (column1 != -1 && row1 != -1)
    {
      int column2 = column1;
      for (int index3 = lastColumn; column2 <= index3; ++column2)
      {
        int row2 = row1;
        for (int index4 = lastRow; row2 <= index4; ++row2)
        {
          string str3 = range[row2, column2].Value;
          if (str3 != null && str3.Length > 0)
            str1 = $"{str1}{str3} ";
        }
      }
    }
    return !(str1 == "") ? str1.Substring(0, str1.Length - 1) : "";
  }

  internal void UpdateChartExSerieRangesMembers(bool isValues)
  {
    if (isValues)
    {
      if (this.m_ValueRange == null || this.m_ValueRange is ExternalRange)
        return;
      if (this.m_ValueRange.LastColumn - this.m_ValueRange.Column > 0 || this.m_ValueRange.Column == this.m_ValueRange.LastColumn && this.m_ValueRange.Row == this.m_ValueRange.LastRow)
        this.m_isRowWiseSeries = true;
      else
        this.m_isRowWiseSeries = false;
    }
    else
    {
      if (this.m_CategoryRange == null || this.m_CategoryRange is ExternalRange)
        return;
      if (this.m_CategoryRange.LastColumn - this.m_CategoryRange.Column > this.m_CategoryRange.LastRow - this.m_CategoryRange.Row)
        this.m_isRowWiseCategory = true;
      else
        this.m_isRowWiseCategory = false;
    }
  }

  public void SetDefaultName(string strName, bool isClearNameRange)
  {
    this.m_strName = strName != null && strName.Length != 0 ? strName : throw new ArgumentNullException(nameof (strName));
    this.m_bDefaultName = true;
    if (!isClearNameRange)
      return;
    this.m_nameRange = (IRange) null;
  }

  public IRange GetSerieNameRange()
  {
    string name = this.Name;
    return this.m_nameRange;
  }

  public OfficeChartType DetectSerieType()
  {
    this.m_serieType = (OfficeChartType) Enum.Parse(typeof (OfficeChartType), this.DetectSerieTypeString(), true);
    return this.m_serieType;
  }

  public string DetectSerieTypeString()
  {
    ChartFormatCollection primaryFormats = this.m_chart.PrimaryFormats;
    if (this.m_chart.Loading && primaryFormats.Count == 0)
      return "Column_Clustered";
    ChartFormatImpl commonSerieFormat = this.GetCommonSerieFormat();
    switch (commonSerieFormat.FormatRecordType)
    {
      case TBIFFRecord.ChartBar:
        return this.DetectBarSerie(commonSerieFormat);
      case TBIFFRecord.ChartLine:
        return this.DetectLineSerie(commonSerieFormat);
      case TBIFFRecord.ChartPie:
        return this.DetectPieSerie(commonSerieFormat);
      case TBIFFRecord.ChartArea:
        return this.DetectAreaSerie(commonSerieFormat);
      case TBIFFRecord.ChartScatter:
        return this.DetectScatterSerie(commonSerieFormat);
      case TBIFFRecord.ChartRadar:
        return this.DetectRadarSerie(commonSerieFormat);
      case TBIFFRecord.ChartSurface:
        return this.DetectSurfaceSerie(commonSerieFormat);
      case TBIFFRecord.ChartRadarArea:
        return "Radar_Filled";
      case TBIFFRecord.ChartBoppop:
        return this.DetectBoppopSerie(commonSerieFormat);
      default:
        throw new ArgumentOutOfRangeException("Can't detect serie type.");
    }
  }

  public string DetectSerieTypeStart()
  {
    ChartFormatCollection primaryFormats = this.m_chart.PrimaryFormats;
    if (this.m_chart.Loading && primaryFormats.Count == 0)
      return "Column";
    ChartFormatImpl commonSerieFormat = this.GetCommonSerieFormat();
    switch (commonSerieFormat.FormatRecordType)
    {
      case TBIFFRecord.ChartBar:
        return this.GetBarStartString(commonSerieFormat);
      case TBIFFRecord.ChartLine:
        return "Line";
      case TBIFFRecord.ChartPie:
        return commonSerieFormat.DoughnutHoleSize == 0 ? "Pie" : "Doughnut";
      case TBIFFRecord.ChartArea:
        return "Area";
      case TBIFFRecord.ChartScatter:
        return (commonSerieFormat.DataFormatOrNull == null || commonSerieFormat.DataFormatOrNull.SerieFormatOrNull == null || !commonSerieFormat.DataFormatOrNull.Is3DBubbles) && !commonSerieFormat.IsBubbles ? "Scatter" : "Bubble";
      case TBIFFRecord.ChartRadar:
      case TBIFFRecord.ChartRadarArea:
        return "Radar";
      case TBIFFRecord.ChartSurface:
        return "Surface";
      case TBIFFRecord.ChartBoppop:
        return "Pie";
      default:
        throw new ArgumentOutOfRangeException("Can't detect serie type.");
    }
  }

  private string DetectBarSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartBar)
      throw new ArgumentException(nameof (format));
    string empty = string.Empty;
    string barStartString = this.GetBarStartString(format);
    if (format.IsChartExType)
      return barStartString;
    string str;
    if (format.StackValuesBar)
    {
      str = barStartString + "_Stacked";
    }
    else
    {
      if (barStartString == "Column" && format.Is3D && !format.RightAngleAxes && !format.IsClustered)
        return "Column_3D";
      str = barStartString + "_Clustered";
    }
    if (format.ShowAsPercentsBar)
      str += "_100";
    bool flag = str.IndexOf("Cone") != -1 || str.IndexOf("Cylinder") != -1 || str.IndexOf("Pyramid") != -1;
    if (format.Is3D && !flag)
      str += "_3D";
    if (flag && !format.IsClustered && !format.StackValuesBar)
      str += "_3D";
    return str;
  }

  private string GetBarStartString(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartBar)
      throw new ArgumentException(nameof (format));
    string result = (string) null;
    return !format.IsChartExType ? (this.ParentChart.Loading || (this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull == null || !(this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull.IsFormatted || (this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull.BarShapeBase == OfficeBaseFormat.Rectangle && (this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull.BarShapeTop == OfficeTopFormat.Straight ? (format.DataFormatOrNull == null || format.DataFormatOrNull.Serie3DdDataFormatOrNull == null ? (format.IsHorizontalBar ? "Bar" : "Column") : this.GetBarShapeString(format, result, format.DataFormatOrNull)) : this.GetBarShapeString(format, result, this.SerieFormat as ChartSerieDataFormatImpl)) : this.m_chart.m_chartType.ToString();
  }

  private string GetBarShapeString(
    ChartFormatImpl format,
    string result,
    ChartSerieDataFormatImpl dataFormat)
  {
    if (dataFormat.BarShapeBase == OfficeBaseFormat.Circle)
    {
      result = dataFormat.BarShapeTop == OfficeTopFormat.Straight ? "Cylinder" : "Cone";
      if (format.IsHorizontalBar)
        result += "_Bar";
    }
    else if (dataFormat.BarShapeTop == OfficeTopFormat.Straight)
    {
      result = format.IsHorizontalBar ? "Bar" : "Column";
    }
    else
    {
      result = "Pyramid";
      if (format.IsHorizontalBar)
        result += "_Bar";
    }
    return result;
  }

  private string DetectPieSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartPie)
      throw new ArgumentException(nameof (format));
    string str = format.DoughnutHoleSize != 0 ? "Doughnut" : "Pie";
    if (!this.ParentChart.Loading && str == "Doughnut" && (this.ParentSeries.Count == 0 ? 0 : ((this.ParentSeries[this.ParentSeries.Count - 1].DataPoints as ChartDataPointsCollection).DefPointFormatOrNull == null ? 0 : ((this.ParentSeries[this.ParentSeries.Count - 1].DataPoints as ChartDataPointsCollection).DefPointFormatOrNull.Percent > 0 ? 1 : 0))) != 0)
      str += "_Exploded";
    else if (!this.ParentChart.Loading && str == "Pie" && (this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull != null && (this.DataPoints as ChartDataPointsCollection).DefPointFormatOrNull.Percent > 0)
      str += "_Exploded";
    else if (format.DataFormatOrNull != null)
    {
      ChartSerieDataFormatImpl dataFormatOrNull = format.DataFormatOrNull;
      if (dataFormatOrNull.PieFormatOrNull != null && dataFormatOrNull.Percent > 0)
        str += "_Exploded";
    }
    if (format.Is3D)
      str += "_3D";
    return str;
  }

  private string DetectAreaSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartArea)
      throw new ArgumentException(nameof (format));
    if (format.Is3D && !format.RightAngleAxes && !format.IsStacked)
      return "Area_3D";
    string str = "Area";
    if (format.IsStacked)
      str += "_Stacked";
    if (format.IsCategoryBrokenDown)
      str += "_100";
    if (format.Is3D)
      str += "_3D";
    return str;
  }

  private string DetectSurfaceSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartSurface)
      throw new ArgumentException(nameof (format));
    string str = "Surface";
    if (!format.IsFillSurface)
      str += "_NoColor";
    return format.Rotation != 0 || format.Elevation != 90 || format.Perspective != 0 ? str + "_3D" : str + "_Contour";
  }

  private string DetectBoppopSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartBoppop)
      throw new ArgumentException(nameof (format));
    switch (format.PieChartType)
    {
      case OfficePieType.Normal:
        return "Pie";
      case OfficePieType.Pie:
        return "PieOfPie";
      case OfficePieType.Bar:
        return "Pie_Bar";
      default:
        throw new ApplicationException("Can't detect boppop serie type.");
    }
  }

  private string DetectRadarSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartRadar)
      throw new ArgumentException(nameof (format));
    string str = "Radar";
    if (format.IsMarker)
      str += "_Markers";
    ChartSerieDataFormatImpl pointFormatOrNull = ((ChartDataPointsCollection) this.DataPoints).DefPointFormatOrNull;
    if (pointFormatOrNull != null && pointFormatOrNull.ContainsLineProperties)
    {
      str = "Radar";
      if (pointFormatOrNull.IsMarker)
        str += "_Markers";
    }
    return str;
  }

  private string DetectLineSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartLine)
      throw new ArgumentException(nameof (format));
    if (format.Is3D)
      return "Line_3D";
    bool flag = false;
    ChartSerieDataFormatImpl pointFormatOrNull = ((ChartDataPointsCollection) this.DataPoints).DefPointFormatOrNull;
    if (pointFormatOrNull != null && pointFormatOrNull.ContainsLineProperties)
      flag = true;
    string str = "Line";
    ChartImpl parentChart = this.ParentChart;
    if (parentChart != null && parentChart.LineChartCount == 1 && parentChart.Series[0].Equals((object) this) && (!flag && format.IsMarker || flag && pointFormatOrNull.IsMarker) || parentChart.LineChartCount == 1 && parentChart.Series.Count > 0 && !parentChart.Series[0].Equals((object) this) && (parentChart.Series[0] as ChartSerieImpl).m_serieType.ToString().Contains("Markers") || parentChart.LineChartCount != 1 && (!flag && format.IsMarker || flag && pointFormatOrNull.IsMarker))
      str += "_Markers";
    if (format.StackValuesLine)
      str += "_Stacked";
    if (format.ShowAsPercentsLine)
      str += "_100";
    return str;
  }

  private string DetectScatterSerie(ChartFormatImpl format)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    if (format.FormatRecordType != TBIFFRecord.ChartScatter)
      throw new ArgumentException(nameof (format));
    if (format.DataFormatOrNull != null)
    {
      ChartSerieDataFormatImpl dataFormatOrNull = format.DataFormatOrNull;
      if (dataFormatOrNull.SerieFormatOrNull != null && dataFormatOrNull.Is3DBubbles)
        return "Bubble_3D";
    }
    if (format.IsBubbles)
      return "Bubble";
    ChartSerieDataFormatImpl pointFormatOrNull = ((ChartDataPointsCollection) this.DataPoints).DefPointFormatOrNull;
    bool containsLineProperties = pointFormatOrNull.ContainsLineProperties;
    string str = "Scatter";
    if (containsLineProperties && pointFormatOrNull.IsSmoothed || !containsLineProperties && format.IsSmoothed)
      str += "_SmoothedLine";
    else if (containsLineProperties && pointFormatOrNull.IsLine || !containsLineProperties && format.IsLine)
      str += "_Line";
    ChartImpl parentChart = this.ParentChart;
    if (containsLineProperties && pointFormatOrNull.IsMarker || !containsLineProperties && format.IsMarker || parentChart != null && parentChart.Series.Count > 0 && (parentChart.Series[0] as ChartSerieImpl).m_serieType.ToString().Contains("Markers"))
      str += "_Markers";
    if (str == "Scatter")
      str = OfficeChartType.Scatter_Line.ToString();
    return str;
  }

  internal void ChangeSeriesType(OfficeChartType seriesType, bool isSeriesCreation)
  {
    this.ChangeSeriesType(seriesType, isSeriesCreation, false);
  }

  internal void ChangeSeriesType(
    OfficeChartType seriesType,
    bool isSeriesCreation,
    bool forceChange)
  {
    OfficeChartType serieType = this.SerieType;
    if (!this.m_book.IsWorkbookOpening && serieType == seriesType && !forceChange)
      return;
    this.m_chart.TypeChanging = true;
    this.OnSerieTypeChange(seriesType, serieType, isSeriesCreation, forceChange);
    this.m_serieType = seriesType;
    this.m_chart.TypeChanging = false;
  }

  private void OnSerieTypeChange(
    OfficeChartType type,
    OfficeChartType oldType,
    bool isSeriesCreation,
    bool forceChange)
  {
    if (ChartImpl.IsChartExSerieType(type))
    {
      this.m_chart.ChangeToChartExType(this.SerieType, type, isSeriesCreation);
    }
    else
    {
      if ((isSeriesCreation ? 1 : (type == oldType ? 0 : (this.m_chart.Loading || forceChange || ChartImpl.IsChartExSerieType(oldType) || Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_3D, type) != -1 || Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, type) != -1 ? 1 : (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NEED_VIEW_3D, type) != -1 ? 1 : 0)))) != 0)
        this.m_dataPoints.Clear();
      else if (type != oldType)
        this.m_dataPoints.ClearWithExistingFormats(type);
      if (!ChartImpl.CheckDataTablePossibility(ChartFormatImpl.GetStartSerieType(type), false))
        this.m_chart.HasDataTable = false;
      this.HasErrorBarsX = false;
      this.HasErrorBarsY = false;
      this.m_trendLines.Clear();
      OfficeChartType chartType = this.m_chart.ChartType;
      if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_COMBINATION_CHART, chartType) != -1)
        this.ChangeCombinationType(type);
      else if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_CHANGE_SERIE, chartType) == -1 || this.m_chart.Series.Count == 1)
      {
        this.m_chart.ChangeChartType(type, isSeriesCreation);
      }
      else
      {
        if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_CHANGE_SERIE, type) == -1)
          throw new ArgumentException("Cannot change serie type.");
        if (this.ChangeIntimateType(this.GetCommonSerieFormat(), type))
          return;
        if (type == OfficeChartType.Bubble || type == OfficeChartType.Bubble_3D || chartType == OfficeChartType.Bubble_3D || chartType == OfficeChartType.Bubble)
          throw new ArgumentException("Cannot change serie type.");
        if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NOT_SUPPORT_GRIDLINES, type) != -1)
          this.m_chart.PrimaryParentAxis.ClearGridLines();
        this.ChangeNotIntimateType(type);
      }
    }
  }

  private void ChangeNotIntimateType(OfficeChartType type)
  {
    this.m_chart.IsManuallyFormatted = true;
    this.ChartGroup = this.m_chart.PrimaryParentAxis.Formats.ChangeNotIntimateSerieType(type, this.SerieType, this.Application, this.m_chart, this).DrawingZOrder;
  }

  private bool ChangeIntimateType(ChartFormatImpl format, OfficeChartType TypeToChange)
  {
    if (format == (ChartFormatImpl) null)
      throw new ArgumentNullException(nameof (format));
    string startSerieType1 = ChartFormatImpl.GetStartSerieType(this.SerieType);
    string startSerieType2 = ChartFormatImpl.GetStartSerieType(TypeToChange);
    if (startSerieType1 != startSerieType2)
      return false;
    string strTypeToChange = TypeToChange.ToString();
    ChartDataPointImpl defaultDataPoint = (ChartDataPointImpl) this.m_dataPoints.DefaultDataPoint;
    ChartSerieDataFormatImpl dataFormatOrNull = defaultDataPoint.DataFormatOrNull;
    if (startSerieType1 == "Radar" && this.SerieType != OfficeChartType.Radar_Filled && TypeToChange != OfficeChartType.Radar_Filled)
    {
      dataFormatOrNull.ChangeRadarDataFormat(TypeToChange);
      return true;
    }
    switch (startSerieType1)
    {
      case "Scatter":
        dataFormatOrNull.ChangeScatterDataFormat(TypeToChange);
        return true;
      case "Bubble":
        defaultDataPoint.ChangeIntimateBuble(TypeToChange);
        return true;
      case "Line":
        this.ChangeIntimateLine(format, TypeToChange, strTypeToChange);
        return true;
      default:
        if (this.SerieType != TypeToChange)
          format.ChangeSerieType(TypeToChange, false);
        return true;
    }
  }

  private void ChangeIntimateLine(
    ChartFormatImpl format,
    OfficeChartType TypeToChange,
    string strTypeToChange)
  {
    bool flag = strTypeToChange.IndexOf("_Markers") != -1;
    if (format.IsMarker == flag)
    {
      format.ChangeSerieType(TypeToChange, false);
    }
    else
    {
      Dictionary<OfficeChartType, OfficeChartType> hashToInit = new Dictionary<OfficeChartType, OfficeChartType>(7);
      this.InitalizeChangeLineTypeHash(hashToInit);
      format.ChangeSerieType(hashToInit[TypeToChange], false);
      ((ChartSerieDataFormatImpl) ((ChartDataPointImpl) this.DataPoints.DefaultDataPoint).DataFormat).ChangeLineDataFormat(TypeToChange);
    }
  }

  private void InitalizeChangeLineTypeHash(
    Dictionary<OfficeChartType, OfficeChartType> hashToInit)
  {
    if (hashToInit == null)
      throw new ArgumentNullException(nameof (hashToInit));
    hashToInit.Add(OfficeChartType.Line, OfficeChartType.Line_Markers);
    hashToInit.Add(OfficeChartType.Line_Stacked, OfficeChartType.Line_Markers_Stacked);
    hashToInit.Add(OfficeChartType.Line_Stacked_100, OfficeChartType.Line_Markers_Stacked_100);
    hashToInit.Add(OfficeChartType.Line_Markers_Stacked, OfficeChartType.Line_Stacked);
    hashToInit.Add(OfficeChartType.Line_Markers_Stacked_100, OfficeChartType.Line_Stacked_100);
    hashToInit.Add(OfficeChartType.Line_Markers, OfficeChartType.Line);
  }

  private void ChangeCombinationType(OfficeChartType type)
  {
    ChartFormatImpl intimateFormatByType = this.FindIntimateFormatByType(type, this.UsePrimaryAxis, true);
    ChartGlobalFormatsCollection formats = this.m_chart.PrimaryParentAxis.Formats;
    ChartFormatImpl format = (ChartFormatImpl) null;
    int chartGroup = this.ChartGroup;
    if (formats.PrimaryFormats.ContainsIndex(chartGroup) || formats.SecondaryFormats.ContainsIndex(chartGroup))
      format = this.GetCommonSerieFormat();
    if (intimateFormatByType != (ChartFormatImpl) null)
    {
      this.ChartGroup = intimateFormatByType.DrawingZOrder;
      if (this.m_seriesColl.GetCountOfSeriesWithSameDrawingOrder(chartGroup) == 0 && format != (ChartFormatImpl) null)
        this.m_chart.PrimaryParentAxis.Formats.Remove(format);
      this.ChangeIntimateType(intimateFormatByType, type);
      if (this.m_seriesColl.GetCountOfSeriesWithSameType(type, this.UsePrimaryAxis) != this.m_seriesColl.Count)
        return;
      this.m_chart.ChartType = this.SerieType;
    }
    else
    {
      bool flag1 = this.m_seriesColl.GetCountOfSeriesWithSameDrawingOrder(chartGroup) == 1;
      bool flag2 = Array.IndexOf<OfficeChartType>(ChartImpl.DEF_NEED_SECONDARY_AXIS, type) != -1;
      int count = this.m_chart.SecondaryFormats.Count;
      if (flag2 && count != 0)
        throw new ArgumentException("cannot change serie type.");
      ChartFormatCollection parent;
      if (flag2)
      {
        parent = this.m_chart.SecondaryFormats;
        this.m_chart.SecondaryParentAxis.UpdateSecondaryAxis(true);
      }
      else
        parent = this.m_chart.PrimaryFormats;
      ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) parent);
      if (flag1)
      {
        this.ChartGroup = this.ChartGroup == 0 ? 1 : 0;
        this.m_chart.PrimaryParentAxis.Formats.Remove(format);
      }
      formatToAdd.ChangeSerieType(type, false);
      formatToAdd.DrawingZOrder = this.m_seriesColl.FindOrderByType(type);
      parent.Add(formatToAdd, false);
      this.ChartGroup = formatToAdd.DrawingZOrder;
    }
  }

  public ChartFormatImpl FindIntimateFormatByType(
    OfficeChartType type,
    bool bPrimaryAxis,
    bool bPreferSameAxis)
  {
    string startSerieType = ChartFormatImpl.GetStartSerieType(type);
    List<int> intList = new List<int>(6);
    ChartFormatImpl intimateFormatByType = (ChartFormatImpl) null;
    int index = 0;
    for (int count = this.m_seriesColl.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.m_seriesColl[index];
      int chartGroup = chartSerieImpl.ChartGroup;
      if (!intList.Contains(chartGroup))
      {
        if (startSerieType == ChartFormatImpl.GetStartSerieType(chartSerieImpl.SerieType))
        {
          if (startSerieType != "Radar")
            intimateFormatByType = chartSerieImpl.GetCommonSerieFormat();
          if (bPreferSameAxis && bPrimaryAxis == chartSerieImpl.UsePrimaryAxis || !bPreferSameAxis)
            break;
        }
        intList.Add(chartGroup);
      }
    }
    return intimateFormatByType;
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.IsValidCategoryRange = this.m_IsValidCategoryRange;
    this.IsValidValueRange = this.m_IsValidValueRange;
    if (this.m_ValueRange != null)
      this.ValuesIRange = this.UpdateRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToValues], iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
    if (this.m_CategoryRange != null)
      this.CategoryLabelsIRange = this.UpdateRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToCategories], iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
    if (this.m_BubbleRange == null)
      return;
    this.BubblesIRange = this.UpdateRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToBubbles], iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  private IRange UpdateRange(
    ChartAIRecord chartAI,
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    if (chartAI == null)
      return (IRange) null;
    Ptg[] parsedExpression = chartAI.ParsedExpression;
    List<Ptg> ptgList = new List<Ptg>();
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      Ptg[] collection = this.UpdateToken(parsedExpression[index], iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
      ptgList.AddRange((IEnumerable<Ptg>) collection);
    }
    chartAI.ParsedExpression = ptgList.ToArray();
    return this.GetRange(chartAI);
  }

  private Ptg[] UpdateToken(
    Ptg token,
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    if (token == null)
      throw new ArgumentNullException(nameof (token));
    if (!(token is IReference reference))
      return new Ptg[1]{ token };
    int refIndex = (int) reference.RefIndex;
    if (refIndex != iSourceIndex && refIndex == iDestIndex)
      return new Ptg[1]{ token };
    if (!(token is IRectGetter rectGetter))
      return new Ptg[1]{ token };
    Rectangle rectangle = rectGetter.GetRectangle();
    if (refIndex == iSourceIndex)
    {
      if (sourceRect.Contains(rectangle))
        return new Ptg[1]
        {
          token.Offset(iCurIndex, -1, -1, iSourceIndex, sourceRect, iDestIndex, destRect, out bool _, this.m_book)
        };
      if (iSourceIndex == iDestIndex && UtilityMethods.Intersects(sourceRect, rectangle))
        return new Ptg[1]
        {
          this.PartialTokenMove(token, iSourceIndex, rectangle, sourceRect, destRect)
        };
    }
    if (refIndex == iDestIndex)
    {
      if (destRect.Contains(rectangle))
        return new Ptg[0];
      if (UtilityMethods.Intersects(destRect, rectangle))
        return new Ptg[1]
        {
          this.PartialRemove(token, refIndex, rectangle, sourceRect, destRect)
        };
    }
    return new Ptg[1]{ token };
  }

  private Ptg PartialTokenMove(
    Ptg token,
    int iSourceIndex,
    Rectangle rectRange,
    Rectangle sourceRect,
    Rectangle destRect)
  {
    bool flag1 = UtilityMethods.Contains(sourceRect, rectRange.Left, rectRange.Top);
    bool flag2 = UtilityMethods.Contains(sourceRect, rectRange.Right, rectRange.Bottom);
    if (!flag1 && !flag2)
      return token;
    int num1 = destRect.Left - sourceRect.Left;
    int num2 = destRect.Top - sourceRect.Top;
    if (rectRange.Height == 0 && num2 == 0)
    {
      int num3;
      int num4;
      if (flag1)
      {
        num3 = Math.Min(rectRange.Left + num1, sourceRect.Right + 1);
        num4 = Math.Max(rectRange.Left + num1, rectRange.Right);
      }
      else
      {
        num3 = Math.Min(rectRange.Right + num1, rectRange.Left);
        num4 = Math.Max(rectRange.Right + num1, sourceRect.Left - 1);
      }
      return FormulaUtil.CreatePtg(token.TokenCode, (object) iSourceIndex, (object) rectRange.Top, (object) num3, (object) rectRange.Bottom, (object) num4, (object) (byte) 0, (object) (byte) 0);
    }
    if (rectRange.Width != 0 || num1 != 0)
      return token;
    int num5;
    int num6;
    if (flag1)
    {
      num5 = Math.Min(rectRange.Top + num2, sourceRect.Bottom + 1);
      num6 = Math.Max(rectRange.Top + num2, rectRange.Bottom);
    }
    else
    {
      num5 = Math.Min(rectRange.Bottom + num2, rectRange.Top);
      num6 = Math.Max(rectRange.Bottom + num2, sourceRect.Top - 1);
    }
    return FormulaUtil.CreatePtg(token.TokenCode, (object) iSourceIndex, (object) num5, (object) rectRange.Left, (object) num6, (object) rectRange.Right, (object) (byte) 0, (object) (byte) 0);
  }

  private Ptg PartialRemove(
    Ptg token,
    int iSheetIndex,
    Rectangle rectRange,
    Rectangle sourceRect,
    Rectangle destRect)
  {
    if (token == null)
      throw new ArgumentNullException(nameof (token));
    bool flag1 = UtilityMethods.Contains(destRect, rectRange.Left, rectRange.Top);
    bool flag2 = UtilityMethods.Contains(destRect, rectRange.Right, rectRange.Bottom);
    if (!flag1 && !flag2)
      return token;
    int num1 = rectRange.Top;
    int num2 = rectRange.Bottom;
    int num3 = rectRange.Left;
    int num4 = rectRange.Right;
    if (rectRange.Left == rectRange.Right)
    {
      if (flag1)
        num1 = destRect.Bottom + 1;
      else
        num2 = destRect.Top - 1;
    }
    else if (rectRange.Top == rectRange.Bottom)
    {
      if (flag1)
        num3 = destRect.Right + 1;
      else
        num4 = destRect.Left - 1;
    }
    return FormulaUtil.CreatePtg(token.TokenCode, (object) iSheetIndex, (object) num1, (object) num3, (object) num2, (object) num4, (object) (byte) 0, (object) (byte) 0);
  }

  private void UpdateErrorBar(ChartErrorBarsImpl bar, ref ChartErrorBarsImpl barToUpdate)
  {
    if (bar == null)
      throw new ArgumentNullException(nameof (bar));
    if (barToUpdate == null)
      barToUpdate = bar;
    else if (bar.Type == OfficeErrorBarType.Custom)
    {
      if (barToUpdate.Type != OfficeErrorBarType.Custom)
        throw new ApplicationException("Cannot parse error bars. Different types.");
      IRange plusIrange = bar.PlusIRange;
      IRange minusIrange = bar.MinusIRange;
      if (minusIrange != null)
        barToUpdate.MinusIRange = minusIrange;
      if (plusIrange != null)
        barToUpdate.PlusIRange = plusIrange;
      if ((bar.Include != OfficeErrorBarInclude.Minus || barToUpdate.Include != OfficeErrorBarInclude.Plus) && (bar.Include != OfficeErrorBarInclude.Plus || barToUpdate.Include != OfficeErrorBarInclude.Minus))
        return;
      barToUpdate.Include = OfficeErrorBarInclude.Both;
    }
    else
      barToUpdate.Include = OfficeErrorBarInclude.Both;
  }

  public ChartFormatImpl GetCommonSerieFormat()
  {
    return this.m_chart.SecondaryFormats.ContainsIndex(this.m_iChartGroup) ? this.m_chart.SecondaryFormats[this.m_iChartGroup] : this.m_chart.PrimaryFormats[this.m_iChartGroup];
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    foreach (ChartAIRecord chartAiRecord in this.m_hashAi.Values)
      FormulaUtil.MarkUsedReferences(chartAiRecord.ParsedExpression, usedItems);
    if (this.m_errorBarX != null)
      this.m_errorBarX.MarkUsedReferences(usedItems);
    if (this.m_errorBarY != null)
      this.m_errorBarY.MarkUsedReferences(usedItems);
    if (this.m_trendLines == null)
      return;
    this.m_trendLines.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    foreach (ChartAIRecord chartAiRecord in this.m_hashAi.Values)
    {
      Ptg[] parsedExpression = chartAiRecord.ParsedExpression;
      if (FormulaUtil.UpdateReferenceIndexes(parsedExpression, arrUpdatedIndexes))
        chartAiRecord.ParsedExpression = parsedExpression;
    }
    if (this.m_errorBarX != null)
      this.m_errorBarX.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_errorBarY != null)
      this.m_errorBarY.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_trendLines == null)
      return;
    this.m_trendLines.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.SerializeSerie(records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    this.SerializeChartAi(records);
    this.m_dataPoints.SerializeDataFormats(records);
    ChartSertocrtRecord record = (ChartSertocrtRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSertocrt);
    record.ChartGroup = (ushort) this.ChartGroup;
    records.Add((IBiffStorage) record);
    this.SerializeLegendEntries(records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
    if (this.m_valueEnteredDirectly.Count != 0)
      this.UpdateSerieIndexesInEnteredDirectlyValues(this.m_valueEnteredDirectly);
    if (this.m_categoryEnteredDirectly.Count != 0)
      this.UpdateSerieIndexesInEnteredDirectlyValues(this.m_categoryEnteredDirectly);
    if (this.m_bubbleEnteredDirectly.Count != 0)
      this.UpdateSerieIndexesInEnteredDirectlyValues(this.m_bubbleEnteredDirectly);
    if (this.HasErrorBarsY)
      this.m_errorBarY.Serialize(this.m_seriesColl.TrendErrorList);
    if (this.HasErrorBarsX)
      this.m_errorBarX.Serialize(this.m_seriesColl.TrendErrorList);
    this.m_trendLines.Serialize(this.m_seriesColl.TrendErrorList);
  }

  private void SerializeChartAi(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.SeriealizeSerieName(records);
    ChartAIRecord record1 = this.m_hashAi[ChartAIRecord.LinkIndex.LinkToValues];
    this.SetAIRange(record1, this.m_ValueRange, ChartAIRecord.ReferenceType.EnteredDirectly);
    if (this.m_ValueRange != null)
      record1.NumberFormatIndex = (ushort) 2;
    if (this.m_valueEnteredDirectly.Count != 0)
    {
      record1.ParsedExpression = (Ptg[]) null;
      record1.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    }
    records.Add((IBiffStorage) record1.Clone());
    ChartAIRecord record2 = this.m_hashAi[ChartAIRecord.LinkIndex.LinkToCategories];
    this.SetAIRange(record2, this.m_CategoryRange, ChartAIRecord.ReferenceType.DefaultCategories);
    if (this.m_categoryEnteredDirectly.Count != 0)
    {
      record2.ParsedExpression = (Ptg[]) null;
      record2.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    }
    records.Add((IBiffStorage) record2.Clone());
    ChartAIRecord record3 = this.m_hashAi[ChartAIRecord.LinkIndex.LinkToBubbles];
    this.SetAIRange(record3, this.m_BubbleRange, ChartAIRecord.ReferenceType.EnteredDirectly);
    if (this.m_bubbleEnteredDirectly.Count != 0)
    {
      record3.ParsedExpression = (Ptg[]) null;
      record3.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    }
    records.Add((IBiffStorage) record3.Clone());
  }

  private void SetAIRange(ChartAIRecord record, IRange range, ChartAIRecord.ReferenceType onNull)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (range == null)
    {
      record.Reference = onNull;
      record.ParsedExpression = (Ptg[]) null;
    }
    else
    {
      record.Reference = ChartAIRecord.ReferenceType.Worksheet;
      if (range.GetType() == typeof (RangeImpl))
        ((RangeImpl) range).SetWorkbook(this.m_book);
      record.ParsedExpression = ((INativePTG) range).GetNativePtg();
    }
  }

  private void SerializeSerie(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.m_series.StdX = ChartSeriesRecord.DataType.Numeric;
    this.m_series.StdY = ChartSeriesRecord.DataType.Numeric;
    this.m_series.BubbleDataType = ChartSeriesRecord.DataType.Numeric;
    if (this.m_valueEnteredDirectly.Count == 0)
    {
      this.m_series.ValuesCount = this.m_ValueRange != null ? (ushort) this.m_ValueRange.Count : (ushort) 0;
    }
    else
    {
      this.m_series.ValuesCount = (ushort) this.m_valueEnteredDirectly.Count;
      if (this.m_valueEnteredDirectly[0] is LabelRecord)
        this.m_series.StdY = ChartSeriesRecord.DataType.Text;
    }
    if (this.m_categoryEnteredDirectly.Count == 0)
    {
      this.m_series.CategoriesCount = this.m_CategoryRange != null ? (ushort) this.m_CategoryRange.Count : this.m_series.ValuesCount;
    }
    else
    {
      this.m_series.CategoriesCount = (ushort) this.m_categoryEnteredDirectly.Count;
      if (this.m_categoryEnteredDirectly[0] is LabelRecord)
        this.m_series.StdX = ChartSeriesRecord.DataType.Text;
    }
    if (this.m_bubbleEnteredDirectly.Count == 0)
    {
      this.m_series.BubbleSeriesCount = this.m_BubbleRange != null ? (ushort) this.m_BubbleRange.Count : (ushort) 0;
    }
    else
    {
      this.m_series.BubbleSeriesCount = (ushort) this.m_bubbleEnteredDirectly.Count;
      if (this.m_bubbleEnteredDirectly[0] is LabelRecord)
        this.m_series.BubbleDataType = ChartSeriesRecord.DataType.Text;
    }
    this.CheckLimits();
    records.Add((IBiffStorage) this.m_series.Clone());
  }

  public void CheckLimits()
  {
    int num = this.m_categoryEnteredDirectly.Count != 0 ? this.m_categoryEnteredDirectly.Count : (this.m_CategoryRange != null ? (int) (ushort) this.m_CategoryRange.Count : (int) this.m_series.ValuesCount);
    switch (this.m_book.Version)
    {
      case OfficeVersion.Excel97to2003:
      case OfficeVersion.Excel2007:
        if ((!this.m_chart.IsChart3D || num <= 4000) && num <= 32000)
          break;
        throw new ApplicationException("The maximum number of data points you can use in a data series for a 2-D chart is 32000, for a 3-D chart is 4000.If you want to use more data points, you must create two or more series or use Excel 2010.");
    }
  }

  [CLSCompliant(false)]
  public void SerializeDataLabels(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_dataPoints == null)
      return;
    this.m_dataPoints.SerializeDataLabels(records);
  }

  private void SerializeLegendEntries(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (!this.m_chart.HasLegend)
      return;
    string startSerieType = ChartFormatImpl.GetStartSerieType(this.m_chart.ChartType);
    ChartLegendEntriesColl legendEntries = (ChartLegendEntriesColl) this.m_chart.Legend.LegendEntries;
    if (Array.IndexOf<string>(ChartImpl.DEF_LEGEND_NEED_DATA_POINT, startSerieType) == -1)
    {
      if (!legendEntries.Contains(this.Index))
        return;
      ((ChartLegendEntryImpl) legendEntries[this.Index]).Serialize((IList<IBiffStorage>) records);
    }
    else
    {
      if (this.Index != 0)
        return;
      int iIndex = 0;
      for (int count = legendEntries.Count; iIndex < count; ++iIndex)
      {
        if (legendEntries.Contains(iIndex))
          ((ChartLegendEntryImpl) legendEntries[iIndex]).Serialize((IList<IBiffStorage>) records);
      }
    }
  }

  private void SeriealizeSerieName(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    string name = this.Name;
    ChartAIRecord chartAiRecord = this.m_hashAi[ChartAIRecord.LinkIndex.LinkToTitleOrText];
    if ((this.m_strName == null || this.m_strName.Length == 0) && this.m_nameRange == null || this.m_strName[0] != '=')
    {
      chartAiRecord.ParsedExpression = (Ptg[]) null;
      chartAiRecord.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    }
    else
    {
      Ptg[] nameTokens = this.GetNameTokens();
      chartAiRecord.ParsedExpression = nameTokens;
      chartAiRecord.Reference = ChartAIRecord.ReferenceType.Worksheet;
    }
    records.Add((IBiffStorage) chartAiRecord.Clone());
    if (this.m_bDefaultName)
      return;
    ChartSeriesTextRecord record = (ChartSeriesTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesText);
    record.Text = name == null ? "" : name;
    records.Add((IBiffStorage) record);
  }

  private Ptg[] GetNameTokens()
  {
    return this.m_nameRange != null || this.m_strName == null || this.m_strName[0] != '=' ? ((INativePTG) this.m_nameRange).GetNativePtg() : this.m_book.FormulaUtil.ParseString(UtilityMethods.RemoveFirstCharUnsafe(this.m_strName));
  }

  public event ValueChangedEventHandler NameChanged;

  public IOfficeChartFrameFormat ParetoLineFormat
  {
    get => (IOfficeChartFrameFormat) this.m_paretoLineFormat;
    internal set => this.m_paretoLineFormat = value as ChartFrameFormatImpl;
  }

  internal bool HasLeaderLines
  {
    get => this.m_hasLeaderLines;
    set
    {
      this.m_hasLeaderLines = value;
      if (this.ParentChart == null || !this.ParentChart.IsChartPie)
        return;
      this.DataPoints.DefaultDataPoint.DataLabels.ShowLeaderLines = value;
    }
  }

  internal IOfficeChartBorder LeaderLines
  {
    get
    {
      if (this.m_leaderLines == null)
        this.m_leaderLines = new ChartBorderImpl(this.Application, (object) this);
      this.m_leaderLines.HasLineProperties = true;
      return (IOfficeChartBorder) this.m_leaderLines;
    }
  }

  public int Index
  {
    get => this.m_iIndex;
    set
    {
      this.m_iIndex = value;
      this.m_dataPoints.UpdateSerieIndex();
    }
  }

  public bool IsFiltered
  {
    get => this.m_IsFiltered;
    set => this.m_IsFiltered = value;
  }

  public int Number
  {
    get => this.m_iOrder;
    set => this.m_iOrder = value;
  }

  public int ChartGroup
  {
    get => this.m_iChartGroup;
    set => this.m_iChartGroup = value;
  }

  public ChartImpl InnerChart => this.m_chart;

  public bool IsDefaultName
  {
    get => this.m_bDefaultName;
    set => this.m_bDefaultName = value;
  }

  public int PointNumber
  {
    get
    {
      if ("Surface" == ChartFormatImpl.GetStartSerieType(this.m_chart.ChartType))
        return 65532;
      return this.m_ValueRange == null ? 0 : ((ICombinedRange) this.m_ValueRange).CellsCount;
    }
  }

  public WorkbookImpl InnerWorkbook => this.m_book;

  public string FilteredCategory
  {
    get => this.m_categoryFilteredRange;
    set => this.m_categoryFilteredRange = value;
  }

  public string FilteredValue
  {
    get => this.m_categoryValue;
    set => this.m_categoryValue = value;
  }

  public string StartType => this.DetectSerieTypeStart();

  public string ParseSerieNotDefaultText => this.m_seriesText;

  public ChartSeriesCollection ParentSeries => this.m_seriesColl;

  public ChartImpl ParentChart => this.m_chart;

  internal WorkbookImpl ParentBook => this.m_book;

  public bool IsPie => ChartImpl.GetIsChartPie(this.SerieType);

  public string NameOrFormula
  {
    get
    {
      if (this.m_nameRange != null && this.m_strName.Substring(1) != this.m_nameRange.AddressGlobal)
        this.m_strName = "=" + this.m_nameRange.AddressGlobal;
      return this.m_strName == null || this.m_strName.Length <= 0 || this.m_strName[0] != '=' || this.m_nameTokens == null ? this.m_strName : '='.ToString() + this.m_book.FormulaUtil.ParsePtgArray(this.m_nameTokens);
    }
  }

  public bool InvertIfNegative
  {
    get => !this.m_bInvertIfNegative.HasValue || this.m_bInvertIfNegative.Value;
    set => this.m_bInvertIfNegative = new bool?(value);
  }

  internal bool? InvertNegative => this.m_bInvertIfNegative;

  public string StrRefFormula
  {
    get => this.m_strRefFormula;
    set => this.m_strRefFormula = value;
  }

  internal string NumRefFormula
  {
    get => this.m_numRefFormula;
    set => this.m_numRefFormula = value;
  }

  internal string MulLvlStrRefFormula
  {
    get => this.m_MulLvlStrRefFormula;
    set => this.m_MulLvlStrRefFormula = value;
  }

  public ChartColor InvertIfNegativeColor
  {
    get
    {
      if (this.m_invertIfNegativeColor == (ChartColor) ColorExtension.White && this.m_invertFillFormatStream != null)
        this.m_invertIfNegativeColor = ChartParser.ParseInvertSolidFillFormat(this.m_invertFillFormatStream, this);
      return this.m_invertIfNegativeColor;
    }
    set
    {
      if (this.SerieFormat.Fill.FillType != OfficeFillType.SolidColor || this.SerieType != OfficeChartType.Bar_Clustered && this.SerieType != OfficeChartType.Bar_Clustered_3D && this.SerieType != OfficeChartType.Bar_Stacked && this.SerieType != OfficeChartType.Bar_Stacked_100 && this.SerieType != OfficeChartType.Bar_Stacked_100_3D && this.SerieType != OfficeChartType.Bar_Stacked_3D && this.SerieType != OfficeChartType.Column_3D && this.SerieType != OfficeChartType.Column_Clustered && this.SerieType != OfficeChartType.Column_Clustered_3D && this.SerieType != OfficeChartType.Column_Stacked && this.SerieType != OfficeChartType.Column_Stacked_100 && this.SerieType != OfficeChartType.Column_Stacked_100_3D && this.SerieType != OfficeChartType.Column_Stacked_3D)
        throw new ArgumentException("Property not supported for current serie type");
      this.m_invertIfNegativeColor = value;
    }
  }

  internal Stream DropLinesStream
  {
    get => this.m_dropLinesStream;
    set => this.m_dropLinesStream = value;
  }

  internal bool IsParetoLineHidden
  {
    get => this.m_isParetoLineHidden;
    set => this.m_isParetoLineHidden = value;
  }

  internal bool IsSeriesHidden
  {
    get => this.m_IsFiltered;
    set => this.m_IsFiltered = value;
  }

  internal int ParetoLineFormatIndex
  {
    get => this.m_paretoLineFormatIndex;
    set => this.m_paretoLineFormatIndex = value;
  }

  internal bool IsRowWiseCategory
  {
    get => this.m_isRowWiseCategory;
    set => this.m_isRowWiseCategory = value;
  }

  internal string CategoriesFormatCode
  {
    get => this.m_categoryFormatCode;
    set => this.m_categoryFormatCode = value;
  }

  internal bool IsRowWiseSeries
  {
    get => this.m_isRowWiseSeries;
    set => this.m_isRowWiseSeries = value;
  }

  public void Reparse()
  {
    this.m_ValueRange = this.GetRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToValues]);
    this.m_BubbleRange = this.GetRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToBubbles]);
    this.m_CategoryRange = this.GetRange(this.m_hashAi[ChartAIRecord.LinkIndex.LinkToCategories]);
  }

  [CLSCompliant(false)]
  public IRange GetRange(ChartAIRecord chartAi)
  {
    if (chartAi == null)
      throw new ArgumentNullException(nameof (chartAi));
    if (chartAi.ParsedExpression == null || chartAi.ParsedExpression.Length <= 1)
      return chartAi.Reference == ChartAIRecord.ReferenceType.Worksheet || chartAi.ParsedExpression != null ? this.GetRangeFromOnePTG(chartAi.ParsedExpression[0]) : (IRange) null;
    IRanges ranges = (IRanges) null;
    int index = 0;
    for (int length = chartAi.ParsedExpression.Length; index < length; ++index)
    {
      Ptg currentPtg = chartAi.ParsedExpression[index];
      if (currentPtg is IRangeGetter)
      {
        IRange rangeFromOnePtg = this.GetRangeFromOnePTG(currentPtg);
        if (rangeFromOnePtg != null)
        {
          if (ranges == null && rangeFromOnePtg.Worksheet != null)
            ranges = rangeFromOnePtg.Worksheet.CreateRangesCollection();
          ranges?.Add(rangeFromOnePtg);
        }
      }
    }
    return ranges != null && ranges.Count != 0 ? (IRange) ranges : (IRange) null;
  }

  private IRange GetRangeFromOnePTG(Ptg currentPtg)
  {
    if (currentPtg == null)
      throw new ArgumentNullException(nameof (currentPtg));
    if (!(currentPtg is IRangeGetter))
      throw new ParseException(nameof (currentPtg));
    return !currentPtg.IsOperation && currentPtg.ToString(this.m_book.FormulaUtil, 0, 0, false).IndexOf("#REF") != -1 ? (IRange) null : ((IRangeGetter) currentPtg).GetRange((IWorkbook) this.m_book, (IWorksheet) null);
  }

  internal void SetInvertIfNegative(bool value) => this.m_bInvertIfNegative = new bool?(value);

  internal bool? GetInvertIfNegative() => this.m_bInvertIfNegative;
}
