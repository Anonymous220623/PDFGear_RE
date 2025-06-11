// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ChartCommon
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class ChartCommon
{
  internal const string ExcelNumberFormat = "General";
  internal const string DefaultFont = "Calibri";
  internal const double DEF_BORDER_WIDTH_EXCEL = 12700.0;
  internal const double DEF_LINE_THICKNESS_XML = 2.0;
  internal const double DEF_LINE_THICKNESS_BINARY = 1.0;
  private SfChartExt m_sfChart;
  private bool m_isAxisLine = true;
  private bool m_isLabel = true;
  private bool m_hasNewSeries;
  private int m_newSeriesIndex;
  private ObservableCollection<ChartPoint> m_itemSource;
  private bool m_isDateTimeCategoryAxis;
  internal bool m_isForImage;
  private Dictionary<int, ObservableCollection<ChartPoint>> m_listOfPoints = new Dictionary<int, ObservableCollection<ChartPoint>>();
  private Dictionary<int, string> m_names = new Dictionary<int, string>();
  internal Dictionary<Trendline, TrendLineBorder> TrendlineFormulas;
  internal Dictionary<ChartAxis, Tuple<string, double, bool, bool>> m_TextToMeasure;
  internal int AxisTitleRotationAngle = 90;
  internal bool SecondayAxisAchived;
  internal bool IsChart3D;
  internal bool IsChartEx;
  internal string FontName;
  internal DateTimeIntervalType DateTimeIntervalType;
  internal int ChartWidth;
  internal int ChartHeight;
  private Regex _percentagePattern = new Regex("(^(0)(\\.[0]+)*)%$");
  internal WorkbookImpl parentWorkbook;
  internal int[] m_def_MarkerType_Array = new int[9]
  {
    2,
    1,
    3,
    4,
    5,
    8,
    9,
    6,
    7
  };
  internal ExcelEngine engine;

  internal Dictionary<int, ObservableCollection<ChartPoint>> ListofPoints
  {
    get => this.m_listOfPoints;
    set => this.m_listOfPoints = value;
  }

  internal Dictionary<int, string> Names
  {
    get => this.m_names;
    set => this.m_names = value;
  }

  internal bool HasNewSeries
  {
    get => this.m_hasNewSeries;
    set => this.m_hasNewSeries = value;
  }

  internal int NewSeriesIndex
  {
    get => this.m_newSeriesIndex;
    set => this.m_newSeriesIndex = value;
  }

  internal bool IsCategoryLabel
  {
    get => this.m_isLabel;
    set => this.m_isLabel = value;
  }

  internal bool IsCategoryAxisLine
  {
    get => this.m_isAxisLine;
    set => this.m_isAxisLine = value;
  }

  internal ObservableCollection<ChartPoint> ItemSource
  {
    get => this.m_itemSource;
    set => this.m_itemSource = value;
  }

  internal SfChartExt SfChart
  {
    get => this.m_sfChart;
    set => this.m_sfChart = value;
  }

  internal bool IsDateTimeCategoryAxis
  {
    get => this.m_isDateTimeCategoryAxis;
    set => this.m_isDateTimeCategoryAxis = value;
  }

  internal bool IsLegendManualLayout { get; set; }

  internal IWorksheet Worksheet
  {
    get
    {
      if (this.engine == null)
      {
        this.engine = new ExcelEngine();
        this.engine.Excel.Workbooks.Create(1);
      }
      return this.engine.Excel.Workbooks[0].Worksheets[0];
    }
  }

  protected ChartCommon()
  {
  }

  internal void SetChartSize(IChart _chart)
  {
    if (!(_chart is ChartShapeImpl))
    {
      this.ChartWidth = (int) _chart.Width;
      this.ChartHeight = (int) _chart.Height;
    }
    else
    {
      this.ChartWidth = (_chart as ChartShapeImpl).Width;
      this.ChartHeight = (_chart as ChartShapeImpl).Height;
    }
    if (this.ChartHeight == 0)
      this.ChartHeight = _chart is ChartShapeImpl ? 288 : 660;
    if (this.ChartWidth != 0)
      return;
    this.ChartWidth = _chart is ChartShapeImpl ? 480 : 910;
  }

  internal string ApplyNumberFormat(object value, string numberFormat)
  {
    RangeImpl rangeImpl = this.Worksheet["A1"] as RangeImpl;
    rangeImpl.m_bAutofitText = true;
    rangeImpl.Value2 = value;
    rangeImpl.NumberFormat = numberFormat;
    return rangeImpl.DisplayText;
  }

  private void SfNumericalAxisCommon(
    ChartAxis sfaxis,
    ChartValueAxisImpl serieValAxis,
    ChartValueAxisImpl oppositeAxis,
    bool condition)
  {
    sfaxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Center;
    sfaxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Visible;
    double num = 1.0;
    if ((oppositeAxis is ChartCategoryAxisImpl categoryAxisImpl ? (categoryAxisImpl.IsChartBubbleOrScatter ? 1 : 0) : 1) != 0)
      num = this.GetDisplayUnitValue(oppositeAxis);
    bool flag = false;
    if (!oppositeAxis.IsAutoCross)
    {
      sfaxis.Origin = oppositeAxis.CrossesAt / num - ((categoryAxisImpl != null ? (categoryAxisImpl.IsChartBubbleOrScatter ? 1 : 0) : 1) != 0 ? 0.0 : 1.5);
      if (serieValAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low && serieValAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_High)
        sfaxis.ShowAxisNextToOrigin = true;
      else
        sfaxis.ShowOrigin = true;
      if (serieValAxis.ParentChart.ChartType == ExcelChartType.Combination_Chart && serieValAxis.IsAutoCross && serieValAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_NextToAxis && oppositeAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_None)
      {
        sfaxis.ShowOrigin = true;
        sfaxis.ShowAxisNextToOrigin = false;
        flag = true;
      }
    }
    else
    {
      sfaxis.Origin = 0.0;
      if ((categoryAxisImpl != null ? (categoryAxisImpl.IsChartBubbleOrScatter ? 1 : 0) : (oppositeAxis.AxisType != ExcelAxisType.Value || !(serieValAxis is ChartCategoryAxisImpl) ? 0 : ((serieValAxis as ChartCategoryAxisImpl).IsChartBubbleOrScatter ? 1 : 0))) != 0 && serieValAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low)
        sfaxis.ShowAxisNextToOrigin = true;
    }
    if (sfaxis.ShowOrigin)
    {
      Style lineStyle = (Style) null;
      this.CheckAndApplyAxisLineStyle(serieValAxis.Border as ChartBorderImpl, out lineStyle, serieValAxis.ParentChart, ChartElementsEnum.AxisLine, this.m_isAxisLine);
      sfaxis.OriginLineStyle = lineStyle;
      sfaxis.AxisLineStyle = !flag ? (Style) null : lineStyle;
    }
    else
    {
      Style lineStyle;
      this.CheckAndApplyAxisLineStyle(serieValAxis.Border as ChartBorderImpl, out lineStyle, serieValAxis.ParentChart, ChartElementsEnum.AxisLine, this.m_isAxisLine);
      sfaxis.AxisLineStyle = lineStyle;
    }
    this.SfTickLines((ChartAxisImpl) serieValAxis, sfaxis);
    this.SfGridLines((ChartAxisImpl) serieValAxis, sfaxis);
    sfaxis.LabelRotationAngle = (double) serieValAxis.TextRotationAngle;
    if (condition && ((serieValAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_High || oppositeAxis.ReversePlotOrder) && (serieValAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_High || !oppositeAxis.ReversePlotOrder) || oppositeAxis.IsMaxCross))
    {
      if (!oppositeAxis.IsMaxCross && oppositeAxis.IsAutoCross && !this.IsChart3D && serieValAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_High)
        this.CreateNewLineSeries(sfaxis, serieValAxis, oppositeAxis);
      else
        sfaxis.OpposedPosition = true;
    }
    if (serieValAxis.ReversePlotOrder)
      sfaxis.IsInversed = true;
    if (!serieValAxis.Visible)
      sfaxis.Visibility = Visibility.Collapsed;
    this.TrySetValueAxisNumberFormat(serieValAxis, sfaxis, this.m_isLabel);
    if (this.m_isLabel)
      return;
    this.m_isLabel = true;
  }

  internal void CreateNewLineSeries(
    ChartAxis sfaxis,
    ChartValueAxisImpl serieValAxis,
    ChartValueAxisImpl oppositeAxis)
  {
    if (this.ItemSource == null)
      return;
    LineSeries series = (LineSeries) ChartSeriesFactory.CreateSeries(typeof (LineSeries));
    series.ItemsSource = (object) this.ItemSource;
    LineSeries lineSeries1 = series;
    CustomNumericalAxis customNumericalAxis1 = new CustomNumericalAxis();
    customNumericalAxis1.ShowGridLines = false;
    customNumericalAxis1.RangePadding = NumericalPadding.Normal;
    customNumericalAxis1.OpposedPosition = true;
    CustomNumericalAxis customNumericalAxis2 = customNumericalAxis1;
    lineSeries1.YAxis = (RangeAxisBase) customNumericalAxis2;
    LineSeries lineSeries2 = series;
    CustomCategoryAxis customCategoryAxis1 = new CustomCategoryAxis();
    customCategoryAxis1.ShowGridLines = false;
    customCategoryAxis1.OpposedPosition = true;
    customCategoryAxis1.Visibility = Visibility.Collapsed;
    CustomCategoryAxis customCategoryAxis2 = customCategoryAxis1;
    lineSeries2.XAxis = (ChartAxisBase2D) customCategoryAxis2;
    this.m_isAxisLine = false;
    this.SfNumericalAxis(series.YAxis as CustomNumericalAxis, serieValAxis, oppositeAxis, false);
    this.m_isLabel = false;
    this.m_isAxisLine = true;
    series.Visibility = Visibility.Collapsed;
    series.VisibilityOnLegend = Visibility.Collapsed;
    series.YAxis.MaximumLabels = sfaxis.MaximumLabels;
    this.m_sfChart.Series.Add((ChartSeries) series);
    this.m_hasNewSeries = true;
    this.m_newSeriesIndex = this.m_sfChart.Series.Count - 1;
    sfaxis.OpposedPosition = false;
  }

  private void TrySetValueAxisNumberFormat(
    ChartValueAxisImpl xlsiovalue,
    ChartAxis sfaxis,
    bool isLabel)
  {
    sfaxis.LabelTemplate = this.GetLabelTemplate((ChartAxisImpl) xlsiovalue, isLabel);
    bool flag = false;
    if (this.IsStacked100AxisFormat((ChartAxisImpl) xlsiovalue) && xlsiovalue.NumberFormat == "General")
    {
      flag = true;
    }
    else
    {
      switch (sfaxis)
      {
        case CustomNumericalAxis _:
        case CustomNumericalAxis3D _:
          sfaxis.AxisBoundsChanged += new EventHandler<ChartAxisBoundsEventArgs>(this.NumericalAxisBoundChanged);
          break;
      }
    }
    AxisLabelConverter axisLabelConverter = new AxisLabelConverter();
    if (xlsiovalue.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_None)
    {
      axisLabelConverter.AxisTypeInByte = (byte) 8;
    }
    else
    {
      switch (sfaxis)
      {
        case LogarithmicAxis _:
        case LogarithmicAxis3D _:
          axisLabelConverter.AxisTypeInByte = (byte) 1;
          break;
        default:
          axisLabelConverter.NumberFormatApplyEvent += new Func<object, string, string>(this.ApplyNumberFormat);
          if (flag || this.IsStacked100AxisFormat((ChartAxisImpl) xlsiovalue))
          {
            axisLabelConverter.AxisTypeInByte = (byte) 2;
            axisLabelConverter.NumberFormat = flag ? "0%" : xlsiovalue.NumberFormat;
          }
          else
            axisLabelConverter.NumberFormat = !xlsiovalue.IsSourceLinked || this.IsCategoryAxisAuto((ChartAxisImpl) xlsiovalue) ? (xlsiovalue.NumberFormat == null || !(xlsiovalue.NumberFormat.ToLower() != "General".ToLower()) || !(xlsiovalue.NumberFormat.ToLower() != "standard") ? "General" : xlsiovalue.NumberFormat) : this.GetSourceNumberFormat(xlsiovalue);
          ColorConverter formatColorConverter = this.GetNumberFormatColorConverter(axisLabelConverter.NumberFormat, xlsiovalue.Font.RGBColor);
          if (formatColorConverter != null)
          {
            Binding binding = new Binding()
            {
              Converter = (IValueConverter) formatColorConverter
            };
            sfaxis.LabelTemplate.VisualTree.SetBinding(TextBlock.ForegroundProperty, (BindingBase) binding);
            break;
          }
          break;
      }
    }
    Binding binding1 = new Binding()
    {
      Converter = (IValueConverter) axisLabelConverter
    };
    sfaxis.LabelTemplate.VisualTree.SetBinding(TextBlock.TextProperty, (BindingBase) binding1);
  }

  private bool IsCategoryAxisAuto(ChartAxisImpl axis)
  {
    int axisId = axis.AxisId;
    ChartCategoryAxisImpl primaryCategoryAxis = axis.ParentChart.PrimaryCategoryAxis as ChartCategoryAxisImpl;
    ChartCategoryAxisImpl secondaryCategoryAxis = axis.ParentChart.IsSecondaryValueAxisAvail ? axis.ParentChart.SecondaryCategoryAxis as ChartCategoryAxisImpl : (ChartCategoryAxisImpl) null;
    if (axisId == primaryCategoryAxis.AxisId)
    {
      if (primaryCategoryAxis.CategoryLabels != null)
        return false;
    }
    else if (secondaryCategoryAxis == null || axisId != secondaryCategoryAxis.AxisId || secondaryCategoryAxis.CategoryLabels != null)
      return false;
    return true;
  }

  private bool IsStacked100AxisFormat(ChartAxisImpl axis)
  {
    ChartFormatImpl chartFormat = (axis.Parent as ChartParentAxisImpl).ChartFormats[0];
    return chartFormat.FormatRecordType == TBIFFRecord.ChartArea && chartFormat.IsCategoryBrokenDown || chartFormat.FormatRecordType == TBIFFRecord.ChartBar && chartFormat.ShowAsPercentsBar;
  }

  private string GetSourceNumberFormat(ChartValueAxisImpl axis)
  {
    string sourceNumberFormat = "General";
    IEnumerable<IChartSerie> source = axis.ParentChart.Series.Where<IChartSerie>((Func<IChartSerie, bool>) (item => !item.IsFiltered && item.UsePrimaryAxis == axis.IsPrimary));
    if (source != null && source.Count<IChartSerie>() > 0)
    {
      ChartSerieImpl firstSerie = source.First<IChartSerie>() as ChartSerieImpl;
      if (axis is ChartCategoryAxisImpl)
      {
        IRange categoryLabels = firstSerie.CategoryLabels;
        if (this.CheckIfValidValueRange(categoryLabels))
          sourceNumberFormat = this.TryAndGetFirstCellNumberFormat(categoryLabels);
        else if (firstSerie.CategoriesFormatCode != null && firstSerie.CategoriesFormatCode != "General")
        {
          string categoriesFormatCode = source.All<IChartSerie>((Func<IChartSerie, bool>) (x => (x as ChartSerieImpl).CategoriesFormatCode == firstSerie.CategoriesFormatCode)) ? firstSerie.CategoriesFormatCode : (string) null;
          if (categoriesFormatCode != null)
            sourceNumberFormat = categoriesFormatCode;
        }
      }
      else
      {
        IRange values = firstSerie.Values;
        if (this.CheckIfValidValueRange(values))
          sourceNumberFormat = this.TryAndGetFirstCellNumberFormat(values);
        else if (firstSerie.FormatCode != null && firstSerie.FormatCode != "General")
        {
          string formatCode = source.All<IChartSerie>((Func<IChartSerie, bool>) (x => (x as ChartSerieImpl).FormatCode == firstSerie.FormatCode)) ? firstSerie.FormatCode : (string) null;
          if (formatCode != null)
            sourceNumberFormat = formatCode;
        }
      }
    }
    return sourceNumberFormat;
  }

  private string TryAndGetFirstCellNumberFormat(IRange range)
  {
    string numberFormat = range.Cells[0].NumberFormat;
    WorksheetImpl worksheet = range.Worksheet as WorksheetImpl;
    if (this.IsRowOrColumnIsHidden(range.Cells[0], worksheet))
    {
      for (int index = 1; index < range.Cells.Length; ++index)
      {
        if (!this.IsRowOrColumnIsHidden(range.Cells[index], worksheet))
        {
          numberFormat = range.Cells[index].NumberFormat;
          break;
        }
      }
    }
    return numberFormat;
  }

  protected internal bool CheckIfValidValueRange(IRange range)
  {
    NameImpl nameImpl1;
    switch (range)
    {
      case null:
        return false;
      case ExternalRange _:
        return false;
      case RangesCollection _:
        if ((range as RangesCollection).Any<IRange>((Func<IRange, bool>) (x => x is ExternalRange)))
          return false;
        goto label_9;
      case null:
        nameImpl1 = (NameImpl) null;
        break;
      default:
        nameImpl1 = range as NameImpl;
        break;
    }
    NameImpl nameImpl2 = nameImpl1;
    if (nameImpl2 != null && (nameImpl2.RefersToRange == null || nameImpl2.RefersToRange is ExternalRange))
      return false;
label_9:
    return true;
  }

  protected bool IsRowOrColumnIsHidden(IRange valueCell, WorksheetImpl workSheet)
  {
    int row1 = valueCell.Row;
    int column = valueCell.Column;
    RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) workSheet, row1 - 1, false);
    if (row2 != null && row2.IsHidden)
      return true;
    ColumnInfoRecord columnInfoRecord = workSheet.ColumnInformation[column];
    return columnInfoRecord != null && columnInfoRecord.IsHidden;
  }

  internal void SfNumericalAxis(
    CustomNumericalAxis sfaxis,
    ChartValueAxisImpl xlsiovalue,
    ChartValueAxisImpl categoryAxis,
    bool condition)
  {
    this.SfNumericalAxisCommon((ChartAxis) sfaxis, xlsiovalue, categoryAxis, condition);
    sfaxis.RangePadding = NumericalPadding.Normal;
    double num1 = 1.0;
    bool flag1 = this.IsStacked100AxisFormat((ChartAxisImpl) xlsiovalue);
    if (!flag1)
      num1 = this.GetDisplayUnitValue(xlsiovalue);
    if (xlsiovalue.MajorUnit > 0.0)
      sfaxis.Interval = new double?(xlsiovalue.MajorUnit / num1);
    if (xlsiovalue.MajorUnit > 0.0 && xlsiovalue.MinorUnit > 0.0)
    {
      double num2 = xlsiovalue.MajorUnit / xlsiovalue.MinorUnit;
      sfaxis.SmallTicksPerInterval = num2 >= 2.0 ? (int) (num2 - 1.0) : (num2 == 1.0 ? 0 : (int) num2);
    }
    else
      sfaxis.SmallTicksPerInterval = 3;
    if (!xlsiovalue.IsAutoMax)
    {
      sfaxis.Maximum = new double?(xlsiovalue.MaximumValue / num1);
      if (xlsiovalue.MajorUnit > 0.0 && sfaxis.Maximum.Value % sfaxis.Interval.Value > 0.0)
        sfaxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Default;
    }
    if (!xlsiovalue.IsAutoMin)
      sfaxis.Minimum = new double?(xlsiovalue.MinimumValue / num1);
    if (flag1)
    {
      if (sfaxis.Minimum.HasValue)
        sfaxis.Minimum = new double?(sfaxis.Minimum.Value * 100.0);
      if (sfaxis.Maximum.HasValue)
        sfaxis.Maximum = new double?(sfaxis.Maximum.Value * 100.0);
      else
        sfaxis.Maximum = new double?(100.0);
      if (!sfaxis.Interval.HasValue)
        return;
      sfaxis.Interval = new double?(sfaxis.Interval.Value * 100.0);
    }
    else
    {
      if (this.IsChartEx || sfaxis.Maximum.HasValue || sfaxis.Minimum.HasValue)
        return;
      if (this.m_TextToMeasure == null)
        this.m_TextToMeasure = new Dictionary<ChartAxis, Tuple<string, double, bool, bool>>(4);
      string lower = xlsiovalue.ParentChart.ChartType.ToString().ToLower();
      bool flag2 = lower.Contains("radar") || lower.Contains("bar");
      if (!flag2)
      {
        IList<ChartFormatImpl> chartFormats = (IList<ChartFormatImpl>) (xlsiovalue.Parent as ChartParentAxisImpl).ChartFormats;
        flag2 = chartFormats == null || chartFormats.Count<ChartFormatImpl>((Func<ChartFormatImpl, bool>) (x => x.FormatRecordType == TBIFFRecord.ChartBar && x.IsHorizontalBar || x.FormatRecordType == TBIFFRecord.ChartRadarArea || x.FormatRecordType == TBIFFRecord.ChartRadar || x.FormatRecordType == TBIFFRecord.ChartPie)) > 0;
      }
      if (flag2)
        return;
      double num3 = ApplicationImpl.ConvertUnitsStatic(xlsiovalue.Font.Size, MeasureUnits.Point, MeasureUnits.Pixel);
      Tuple<string, double, bool, bool> tuple = new Tuple<string, double, bool, bool>(xlsiovalue.Font.FontName, Math.Floor(num3 * 0.95), xlsiovalue.Font.Bold, xlsiovalue.Font.Italic);
      if (this.m_TextToMeasure.ContainsKey((ChartAxis) sfaxis))
        return;
      this.m_TextToMeasure.Add((ChartAxis) sfaxis, tuple);
    }
  }

  protected double GetDisplayUnitValue(ChartValueAxisImpl valueAxis)
  {
    if (valueAxis.DisplayUnit == ExcelChartDisplayUnit.Custom)
      return valueAxis.DisplayUnitCustom;
    switch (valueAxis.DisplayUnit)
    {
      case ExcelChartDisplayUnit.None:
        return 1.0;
      case ExcelChartDisplayUnit.Hundreds:
        return 100.0;
      case ExcelChartDisplayUnit.Thousands:
        return 1000.0;
      case ExcelChartDisplayUnit.TenThousands:
        return 10000.0;
      case ExcelChartDisplayUnit.HundredThousands:
        return 100000.0;
      case ExcelChartDisplayUnit.Millions:
        return 1000000.0;
      case ExcelChartDisplayUnit.TenMillions:
        return 10000000.0;
      case ExcelChartDisplayUnit.HundredMillions:
        return 100000000.0;
      case ExcelChartDisplayUnit.ThousandMillions:
        return 1000000000.0;
      case ExcelChartDisplayUnit.MillionMillions:
        return 1000000000000.0;
      default:
        return 1.0;
    }
  }

  internal void SfNumericalAxis3D(
    CustomNumericalAxis3D sfaxis,
    ChartValueAxisImpl xlsiovalue,
    ChartCategoryAxisImpl categoryAxis)
  {
    this.SfNumericalAxisCommon((ChartAxis) sfaxis, xlsiovalue, (ChartValueAxisImpl) categoryAxis, true);
    sfaxis.RangePadding = NumericalPadding.Normal;
    double num1 = 1.0;
    bool flag = this.IsStacked100AxisFormat((ChartAxisImpl) xlsiovalue);
    if (!flag)
      num1 = this.GetDisplayUnitValue(xlsiovalue);
    if (xlsiovalue.MajorUnit > 0.0)
      sfaxis.Interval = new double?(xlsiovalue.MajorUnit / num1);
    if (xlsiovalue.MajorUnit > 0.0 && xlsiovalue.MinorUnit > 0.0)
    {
      double num2 = xlsiovalue.MajorUnit / xlsiovalue.MinorUnit;
      sfaxis.SmallTicksPerInterval = num2 >= 2.0 ? (int) (num2 - 1.0) : (num2 == 1.0 ? 0 : (int) num2);
    }
    else
      sfaxis.SmallTicksPerInterval = 3;
    if (!xlsiovalue.IsAutoMax)
    {
      sfaxis.Maximum = new double?(xlsiovalue.MaximumValue / num1);
      if (xlsiovalue.MajorUnit > 0.0 && sfaxis.Maximum.Value % sfaxis.Interval.Value > 0.0)
        sfaxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Default;
    }
    if (!xlsiovalue.IsAutoMin)
      sfaxis.Minimum = new double?(xlsiovalue.MinimumValue / num1);
    if (!flag)
      return;
    sfaxis.Maximum = new double?(100.0);
  }

  private void NumericalAxisBoundChanged(object sender, ChartAxisBoundsEventArgs e)
  {
    ChartAxis chartAxis = sender as ChartAxis;
    if (chartAxis.VisibleLabels.Count < 2)
      return;
    double? nullable1 = new double?();
    double? nullable2 = new double?();
    if (this.m_TextToMeasure != null && this.m_TextToMeasure.ContainsKey(chartAxis) && chartAxis.VisibleLabels.Count >= 12)
    {
      int index = (int) Math.Ceiling((double) chartAxis.VisibleLabels.Count / 2.0);
      string str = chartAxis.VisibleLabels[index].LabelContent.ToString();
      TextBlock textBlock = new TextBlock();
      textBlock.FontFamily = new System.Windows.Media.FontFamily(this.m_TextToMeasure[chartAxis].Item1);
      textBlock.FontSize = this.m_TextToMeasure[chartAxis].Item2;
      if (this.m_TextToMeasure[chartAxis].Item3)
        textBlock.FontWeight = FontWeights.Bold;
      if (this.m_TextToMeasure[chartAxis].Item4)
        textBlock.FontStyle = FontStyles.Italic;
      textBlock.Text = str;
      textBlock.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
      this.SetMaximumLabelCount(chartAxis, textBlock.DesiredSize);
    }
    else if (chartAxis is CustomNumericalAxis)
    {
      CustomNumericalAxis customNumericalAxis = chartAxis as CustomNumericalAxis;
      nullable1 = customNumericalAxis.Maximum;
      nullable2 = customNumericalAxis.Minimum;
    }
    else
    {
      CustomNumericalAxis3D customNumericalAxis3D = chartAxis as CustomNumericalAxis3D;
      nullable1 = customNumericalAxis3D.Maximum;
      nullable2 = customNumericalAxis3D.Minimum;
    }
    double num1 = chartAxis.VisibleLabels[1].Position - chartAxis.VisibleLabels[0].Position;
    if (nullable1.HasValue && nullable2.HasValue)
    {
      double num2 = nullable2.Value + num1 * (double) (chartAxis.VisibleLabels.Count - 1);
      double? nullable3 = nullable1;
      if ((num2 != nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
        return;
      chartAxis.LabelsIntersectAction = AxisLabelsIntersectAction.None;
    }
    else
      chartAxis.LabelsIntersectAction = AxisLabelsIntersectAction.None;
  }

  private void SetMaximumLabelCount(ChartAxis axis, System.Windows.Size size)
  {
    double num1 = Math.Ceiling(size.Height) * 1.5;
    if (!(axis is CustomNumericalAxis))
      return;
    double heightExt = (axis as CustomNumericalAxis).HeightExt;
    if (num1 >= 80.0)
      return;
    int num2 = 1;
    int num3 = 2;
    for (; num2 <= 4 && 100.0 / (num1 * (double) num2) >= 1.0; ++num2)
      num3 = num2;
    if (Math.Ceiling(heightExt / (double) (100 / num3)) > 12.0)
      num3 = (int) (100.0 / (heightExt / 12.0));
    if (num3 > 1)
      axis.MaximumLabels = num3;
    this.m_TextToMeasure.Remove(axis);
  }

  private DataTemplate GetLabelTemplate(ChartAxisImpl chartAxisImpl, bool isLabel)
  {
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (TextBlock));
    Binding binding = new Binding();
    frameworkElementFactory.SetBinding(TextBlock.TextProperty, (BindingBase) binding);
    frameworkElementFactory.SetValue(TextBlock.FontFamilyProperty, (object) new System.Windows.Media.FontFamily(chartAxisImpl.Font.FontName));
    double num = ApplicationImpl.ConvertUnitsStatic(chartAxisImpl.Font.Size, MeasureUnits.Point, MeasureUnits.Pixel);
    if (isLabel)
      frameworkElementFactory.SetValue(TextBlock.FontSizeProperty, (object) Math.Floor(num * 0.95));
    else
      frameworkElementFactory.SetValue(TextBlock.FontSizeProperty, (object) 0.01);
    if (chartAxisImpl.Font.Bold)
      frameworkElementFactory.SetValue(TextBlock.FontWeightProperty, (object) FontWeights.Bold);
    if (chartAxisImpl.Font.Italic)
      frameworkElementFactory.SetValue(TextBlock.FontStyleProperty, (object) FontStyles.Italic);
    if (chartAxisImpl.Font.Strikethrough)
      frameworkElementFactory.SetValue(TextBlock.TextDecorationsProperty, (object) TextDecorations.Strikethrough);
    if (chartAxisImpl.Font.Underline == ExcelUnderline.Single)
      frameworkElementFactory.SetValue(TextBlock.TextDecorationsProperty, (object) TextDecorations.Underline);
    System.Drawing.Color color = !chartAxisImpl.Font.IsAutoColor || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, chartAxisImpl.ParentChart, out color) || (int) color.R != (int) color.G || (int) color.B != (int) color.R || color.R != (byte) 0 ? chartAxisImpl.Font.RGBColor : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    frameworkElementFactory.SetValue(TextBlock.ForegroundProperty, (object) new SolidColorBrush(this.SfColor(color)));
    if (!(chartAxisImpl.FrameFormat as ChartFrameFormatImpl).IsAutomaticFormat && chartAxisImpl.FrameFormat.Interior.Pattern != ExcelPattern.None)
      frameworkElementFactory.SetValue(TextBlock.BackgroundProperty, (object) this.GetBrushFromDataFormat((IChartFillBorder) chartAxisImpl.FrameFormat));
    if (chartAxisImpl is ChartCategoryAxisImpl)
    {
      if (chartAxisImpl.IsWrapText)
        frameworkElementFactory.SetValue(TextBlock.TextWrappingProperty, (object) TextWrapping.Wrap);
      else
        frameworkElementFactory.SetValue(TextBlock.TextWrappingProperty, (object) TextWrapping.NoWrap);
      frameworkElementFactory.SetValue(TextBlock.TextTrimmingProperty, (object) TextTrimming.CharacterEllipsis);
    }
    DataTemplate labelTemplate = new DataTemplate((object) typeof (TextBlock));
    labelTemplate.VisualTree = frameworkElementFactory;
    return labelTemplate;
  }

  private void SfGridLines(ChartAxisImpl xlsioAxis, ChartAxis sfAxis)
  {
    Style lineStyle;
    if (xlsioAxis.HasMajorGridLines)
    {
      this.CheckAndApplyAxisLineStyle(xlsioAxis.MajorGridLines.Border as ChartBorderImpl, out lineStyle, xlsioAxis.ParentChart, ChartElementsEnum.MajorGridLines, true);
      sfAxis.MajorGridLineStyle = lineStyle;
    }
    else
      sfAxis.MajorGridLineStyle = new Style()
      {
        TargetType = typeof (Line)
      };
    if (xlsioAxis.HasMinorGridLines)
    {
      this.CheckAndApplyAxisLineStyle(xlsioAxis.MinorGridLines.Border as ChartBorderImpl, out lineStyle, xlsioAxis.ParentChart, ChartElementsEnum.MinorGridLines, true);
      sfAxis.MinorGridLineStyle = lineStyle;
    }
    else
      sfAxis.MinorGridLineStyle = new Style()
      {
        TargetType = typeof (Line)
      };
  }

  internal DataTemplate SfDataTemplate(
    DataTemplateInfo dataTemplateInfo,
    ChartFrameFormatImpl frameFormat,
    out System.Windows.Media.Brush fillBrush,
    out System.Windows.Media.Brush borderBrush,
    out Thickness borderThickness,
    string seriesType)
  {
    FrameworkElementFactory child = new FrameworkElementFactory(typeof (TextBlock));
    Binding binding = seriesType.Contains("RangeColumnSeries") ? new Binding("Value") : new Binding();
    child.SetBinding(TextBlock.TextProperty, (BindingBase) binding);
    child.SetValue(TextBlock.FontFamilyProperty, (object) new System.Windows.Media.FontFamily(dataTemplateInfo.FontName));
    child.SetValue(TextBlock.FontSizeProperty, (object) dataTemplateInfo.Size);
    if (dataTemplateInfo.Bold)
      child.SetValue(TextBlock.FontWeightProperty, (object) FontWeights.Bold);
    if (dataTemplateInfo.Italic)
      child.SetValue(TextBlock.FontStyleProperty, (object) FontStyles.Italic);
    if (dataTemplateInfo.Strikethrough)
      child.SetValue(TextBlock.TextDecorationsProperty, (object) TextDecorations.Strikethrough);
    if (dataTemplateInfo.Underline)
      child.SetValue(TextBlock.TextDecorationsProperty, (object) TextDecorations.Underline);
    child.SetValue(TextBlock.ForegroundProperty, (object) new SolidColorBrush(dataTemplateInfo.Color));
    child.SetValue(TextBlock.TextAlignmentProperty, (object) TextAlignment.Center);
    if (!frameFormat.IsAutomaticFormat && frameFormat.Interior.Pattern != ExcelPattern.None)
    {
      fillBrush = this.GetBrushFromDataFormat((IChartFillBorder) frameFormat);
      child.SetValue(TextBlock.BackgroundProperty, (object) fillBrush);
    }
    else
      fillBrush = (System.Windows.Media.Brush) null;
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Border));
    Border brushForTextElements = this.GetBrushForTextElements(frameFormat.Border as ChartBorderImpl);
    borderBrush = brushForTextElements.BorderBrush;
    frameworkElementFactory.SetValue(Border.BorderBrushProperty, (object) borderBrush);
    borderThickness = brushForTextElements.BorderThickness;
    frameworkElementFactory.SetValue(Border.BorderThicknessProperty, (object) borderThickness);
    frameworkElementFactory.SetValue(FrameworkElement.MarginProperty, (object) brushForTextElements.Margin);
    frameworkElementFactory.AppendChild(child);
    DataTemplate dataTemplate = new DataTemplate((object) typeof (Border));
    dataTemplate.VisualTree = frameworkElementFactory;
    return dataTemplate;
  }

  private void SfCategoryAxisCommon(
    ChartAxis sfCatAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis,
    bool condition)
  {
    sfCatAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Center;
    if (sfCatAxis is CustomCategoryAxis)
      sfCatAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Visible;
    sfCatAxis.FontFamily = new System.Windows.Media.FontFamily(xlsioCatAxis.Font.FontName);
    sfCatAxis.FontSize = xlsioCatAxis.Font.Size;
    if (xlsioCatAxis.Font.Bold)
      sfCatAxis.FontWeight = FontWeights.Bold;
    if (xlsioCatAxis.Font.Italic)
      sfCatAxis.FontStyle = FontStyles.Italic;
    System.Drawing.Color color = !xlsioCatAxis.Font.IsAutoColor || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, xlsioCatAxis.ParentChart, out color) || (int) color.R != (int) color.G || (int) color.B != (int) color.R || color.R != (byte) 0 ? xlsioCatAxis.Font.RGBColor : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    sfCatAxis.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    Style lineStyle;
    this.CheckAndApplyAxisLineStyle(xlsioCatAxis.Border as ChartBorderImpl, out lineStyle, xlsioCatAxis.ParentChart, ChartElementsEnum.AxisLine, this.m_isAxisLine);
    sfCatAxis.AxisLineStyle = lineStyle;
    if (!xlsioValAxis.IsAutoCross)
    {
      if (this.IsStacked100AxisFormat((ChartAxisImpl) xlsioCatAxis) && xlsioValAxis.CrossesAt != 0.0)
      {
        double num1 = xlsioValAxis.IsAutoMin ? 0.0 : xlsioValAxis.MinimumValue;
        double num2 = xlsioValAxis.IsAutoMax ? 0.0 : xlsioValAxis.MaximumValue;
        if ((xlsioValAxis.CrossesAt < num2 || xlsioValAxis.CrossesAt == num2) && (xlsioValAxis.CrossesAt > num1 || xlsioValAxis.CrossesAt == num1))
        {
          sfCatAxis.Origin = xlsioValAxis.CrossesAt * 100.0;
          if (xlsioCatAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low)
            sfCatAxis.ShowAxisNextToOrigin = true;
        }
        else
        {
          sfCatAxis.Origin = 0.0;
          sfCatAxis.ShowAxisNextToOrigin = false;
        }
      }
      else
      {
        sfCatAxis.Origin = xlsioValAxis.CrossesAt;
        if (xlsioCatAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low)
          sfCatAxis.ShowAxisNextToOrigin = true;
      }
    }
    else
    {
      sfCatAxis.Origin = 0.0;
      if (xlsioCatAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low)
        sfCatAxis.ShowAxisNextToOrigin = true;
    }
    this.SfTickLines((ChartAxisImpl) xlsioCatAxis, sfCatAxis);
    if (xlsioCatAxis.ParentChart.IsChartRadar)
      this.SfGridLines((ChartAxisImpl) xlsioValAxis, sfCatAxis);
    else if (xlsioCatAxis.ParentChart.ChartType == ExcelChartType.Funnel)
      sfCatAxis.MajorGridLineStyle = new Style()
      {
        TargetType = typeof (Line)
      };
    else
      this.SfGridLines((ChartAxisImpl) xlsioCatAxis, sfCatAxis);
    sfCatAxis.LabelRotationAngle = (double) xlsioCatAxis.TextRotationAngle;
    if (xlsioCatAxis.ReversePlotOrder)
      sfCatAxis.IsInversed = true;
    if (condition && ((xlsioCatAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_High || xlsioValAxis.ReversePlotOrder) && (xlsioCatAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_High || !xlsioValAxis.ReversePlotOrder) || xlsioValAxis.IsMaxCross))
    {
      if (!xlsioValAxis.IsMaxCross && xlsioValAxis.IsAutoCross && !this.IsChart3D && xlsioCatAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_High)
      {
        this.CreateNewLineSeries(sfCatAxis, xlsioCatAxis, xlsioValAxis);
      }
      else
      {
        sfCatAxis.Origin = 0.0;
        sfCatAxis.OpposedPosition = true;
      }
    }
    if (xlsioCatAxis.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_None)
    {
      FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (TextBlock));
      DataTemplate dataTemplate = new DataTemplate((object) typeof (TextBlock));
      dataTemplate.VisualTree = frameworkElementFactory;
      sfCatAxis.LabelTemplate = dataTemplate;
      Binding binding = new Binding()
      {
        Converter = (IValueConverter) new AxisLabelConverter()
        {
          AxisTypeInByte = (byte) 8
        }
      };
      sfCatAxis.LabelTemplate.VisualTree.SetBinding(TextBlock.TextProperty, (BindingBase) binding);
    }
    else
    {
      sfCatAxis.LabelTemplate = this.GetLabelTemplate((ChartAxisImpl) xlsioCatAxis, this.m_isLabel);
      AxisLabelConverter axisLabelConverter = new AxisLabelConverter();
      if (xlsioCatAxis.ParentChart.IsHistogramOrPareto && !xlsioCatAxis.HistogramAxisFormatProperty.IsBinningByCategory && xlsioCatAxis.NumberFormat != "General")
      {
        axisLabelConverter.NumberFormat = xlsioCatAxis.NumberFormat;
        axisLabelConverter.NumberFormatApplyEvent += new Func<object, string, string>(this.ApplyNumberFormat);
        axisLabelConverter.AxisTypeInByte = (byte) 1;
      }
      else if (sfCatAxis is CustomDateTimeAxis || sfCatAxis is DateTimeAxis3D)
      {
        axisLabelConverter.AxisTypeInByte = (byte) 4;
        axisLabelConverter.NumberFormatApplyEvent += new Func<object, string, string>(this.ApplyNumberFormat);
        axisLabelConverter.NumberFormat = !xlsioCatAxis.IsSourceLinked || this.IsCategoryAxisAuto((ChartAxisImpl) xlsioCatAxis) ? (xlsioCatAxis.NumberFormat == null || !(xlsioCatAxis.NumberFormat.ToLower() != "General".ToLower()) || !(xlsioCatAxis.NumberFormat.ToLower() != "standard") ? "General" : xlsioCatAxis.NumberFormat) : this.GetSourceNumberFormat((ChartValueAxisImpl) xlsioCatAxis);
      }
      else
      {
        if (xlsioCatAxis.ParentChart.IsChartRadar)
        {
          WidthRegulationConverter regulationConverter1 = new WidthRegulationConverter();
          Binding binding1 = new Binding()
          {
            Converter = (IValueConverter) regulationConverter1
          };
          sfCatAxis.LabelTemplate.VisualTree.SetBinding(FrameworkElement.MaxWidthProperty, (BindingBase) binding1);
          HeightRegulationConverter regulationConverter2 = new HeightRegulationConverter();
          Binding binding2 = new Binding()
          {
            Converter = (IValueConverter) regulationConverter2
          };
          sfCatAxis.LabelTemplate.VisualTree.SetBinding(FrameworkElement.MaxHeightProperty, (BindingBase) binding2);
        }
        else if (sfCatAxis.LabelRotationAngle == 0.0)
          sfCatAxis.LabelTemplate.VisualTree.SetValue(FrameworkElement.MaxWidthProperty, (object) ((double) this.ChartWidth * (this.IsVerticalAxis((ChartAxisImpl) xlsioCatAxis) ? 0.22 : 0.48)));
        axisLabelConverter.AxisTypeInByte = (byte) 1;
      }
      Binding binding = new Binding()
      {
        Converter = (IValueConverter) axisLabelConverter
      };
      sfCatAxis.LabelTemplate.VisualTree.SetBinding(TextBlock.TextProperty, (BindingBase) binding);
    }
    if (!xlsioCatAxis.Visible)
      sfCatAxis.Visibility = Visibility.Collapsed;
    if (this.m_isLabel)
      return;
    this.m_isLabel = true;
  }

  private bool IsBarChartAxis(ChartAxisImpl chartAxisImpl)
  {
    IList<ChartFormatImpl> chartFormats = (IList<ChartFormatImpl>) (chartAxisImpl.Parent as ChartParentAxisImpl).ChartFormats;
    return chartFormats != null && chartFormats.Count<ChartFormatImpl>((Func<ChartFormatImpl, bool>) (x => x.FormatRecordType == TBIFFRecord.ChartBar && x.IsHorizontalBar)) > 0;
  }

  internal void CreateNewLineSeries(
    ChartAxis sfCatAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis)
  {
    if (this.ItemSource == null)
      return;
    LineSeries series = (LineSeries) ChartSeriesFactory.CreateSeries(typeof (LineSeries));
    series.ItemsSource = (object) this.ItemSource;
    LineSeries lineSeries1 = series;
    CustomNumericalAxis customNumericalAxis1 = new CustomNumericalAxis();
    customNumericalAxis1.ShowGridLines = false;
    customNumericalAxis1.RangePadding = NumericalPadding.Normal;
    customNumericalAxis1.OpposedPosition = true;
    customNumericalAxis1.Visibility = Visibility.Collapsed;
    CustomNumericalAxis customNumericalAxis2 = customNumericalAxis1;
    lineSeries1.YAxis = (RangeAxisBase) customNumericalAxis2;
    if (this.IsDateTimeCategoryAxis)
    {
      LineSeries lineSeries2 = series;
      CustomDateTimeAxis customDateTimeAxis1 = new CustomDateTimeAxis();
      customDateTimeAxis1.ShowGridLines = false;
      customDateTimeAxis1.OpposedPosition = true;
      CustomDateTimeAxis customDateTimeAxis2 = customDateTimeAxis1;
      lineSeries2.XAxis = (ChartAxisBase2D) customDateTimeAxis2;
    }
    else
    {
      LineSeries lineSeries3 = series;
      CustomCategoryAxis customCategoryAxis1 = new CustomCategoryAxis();
      customCategoryAxis1.ShowGridLines = false;
      customCategoryAxis1.OpposedPosition = true;
      CustomCategoryAxis customCategoryAxis2 = customCategoryAxis1;
      lineSeries3.XAxis = (ChartAxisBase2D) customCategoryAxis2;
    }
    this.m_isAxisLine = false;
    if (this.IsDateTimeCategoryAxis)
      this.SfDateTimeAxis(series.XAxis as CustomDateTimeAxis, xlsioCatAxis, xlsioValAxis, false);
    else
      this.SfCategoryAxis(series.XAxis as CustomCategoryAxis, xlsioCatAxis, xlsioValAxis, false);
    this.m_isAxisLine = true;
    this.m_isLabel = false;
    series.Visibility = Visibility.Collapsed;
    series.VisibilityOnLegend = Visibility.Collapsed;
    this.m_sfChart.Series.Add((ChartSeries) series);
    this.m_hasNewSeries = true;
    this.m_newSeriesIndex = this.m_sfChart.Series.Count - 1;
    sfCatAxis.OpposedPosition = false;
  }

  internal void SfCategoryAxis(
    CustomCategoryAxis categoryAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis,
    bool condition)
  {
    this.SfCategoryAxisCommon((ChartAxis) categoryAxis, xlsioCatAxis, xlsioValAxis, condition);
    if (xlsioCatAxis.IsAutoTextRotation)
    {
      categoryAxis.LabelsIntersectAction = AxisLabelsIntersectAction.Auto;
      categoryAxis.AxisBoundsChanged += new EventHandler<ChartAxisBoundsEventArgs>(this.CategoryAxis_AxisBoundsChanged);
      categoryAxis.IsVerticalAxis = this.IsVerticalCategoryAxis((ChartAxisImpl) xlsioCatAxis);
    }
    else
      categoryAxis.LabelCreated += new EventHandler<LabelCreatedEventArgs>(this.OnAxisLabelCreated);
    categoryAxis.Interval = new double?((double) xlsioCatAxis.TickLabelSpacing);
    if (!xlsioCatAxis.IsBetween)
      return;
    categoryAxis.LabelPlacement = LabelPlacement.BetweenTicks;
  }

  internal void SfCategoryAxis3D(
    CategoryAxis3D categoryAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis)
  {
    categoryAxis.LabelCreated += new EventHandler<LabelCreatedEventArgs>(this.OnAxisLabelCreated);
    this.SfCategoryAxisCommon((ChartAxis) categoryAxis, xlsioCatAxis, xlsioValAxis, true);
    categoryAxis.Interval = new double?((double) xlsioCatAxis.TickLabelSpacing);
    if (!xlsioCatAxis.IsBetween)
      return;
    categoryAxis.LabelPlacement = LabelPlacement.BetweenTicks;
  }

  private void OnAxisLabelCreated(object sender, LabelCreatedEventArgs labelCreatedEventArgs)
  {
    ChartAxis chartAxis = sender as ChartAxis;
    if (!(sender is ChartAxisBase3D) || chartAxis.LabelsIntersectAction == AxisLabelsIntersectAction.None)
    {
      if (chartAxis.LabelsIntersectAction != AxisLabelsIntersectAction.Hide)
        return;
      switch (chartAxis)
      {
        case CustomCategoryAxis _:
        case CustomDateTimeAxis _:
          break;
        default:
          return;
      }
    }
    if (!(labelCreatedEventArgs.AxisLabel.LabelContent.ToString() == string.Empty))
      return;
    chartAxis.LabelsIntersectAction = AxisLabelsIntersectAction.None;
  }

  internal void CategoryAxis_AxisBoundsChanged(object sender, ChartAxisBoundsEventArgs e)
  {
    CustomCategoryAxis customCategoryAxis = sender as CustomCategoryAxis;
    bool flag = false;
    Canvas labelsCanvas;
    bool isVerticalAxis;
    if (customCategoryAxis != null)
    {
      labelsCanvas = customCategoryAxis.LabelsCanvas;
      flag = customCategoryAxis.LabelPlacement == LabelPlacement.BetweenTicks;
      isVerticalAxis = customCategoryAxis.IsVerticalAxis;
    }
    else
    {
      CustomDateTimeAxis customDateTimeAxis = sender as CustomDateTimeAxis;
      labelsCanvas = customDateTimeAxis.LabelsCanvas;
      isVerticalAxis = customDateTimeAxis.IsVerticalAxis;
    }
    double num1 = (isVerticalAxis ? (double) this.ChartWidth : (double) this.ChartHeight) * 0.3;
    if (labelsCanvas == null)
      return;
    double num2 = -1.0;
    for (int index = 0; index < labelsCanvas.Children.Count; ++index)
    {
      if (labelsCanvas.Children[index].Visibility == Visibility.Visible && labelsCanvas.Children[index] is FrameworkElement child1 && child1.RenderTransform != null && child1.RenderTransform is TransformGroup renderTransform && renderTransform.Children[0] is RotateTransform child2)
      {
        if (child2.Angle == 45.0 || child2.Angle == 90.0)
        {
          num2 = -child2.Angle;
          if (!this.m_isForImage)
            child2.Angle = num2;
        }
        if (num2 != -1.0 && num1 < child1.DesiredSize.Width)
          child1.SetValue(FrameworkElement.WidthProperty, (object) num1);
      }
    }
    if (num2 == -1.0)
      return;
    ChartAxis chartAxis = sender as ChartAxis;
    chartAxis.LabelRotationAngle = num2;
    chartAxis.AxisBoundsChanged -= new EventHandler<ChartAxisBoundsEventArgs>(this.CategoryAxis_AxisBoundsChanged);
    chartAxis.LabelsIntersectAction = AxisLabelsIntersectAction.Hide;
    if (!flag || customCategoryAxis == null)
      return;
    chartAxis.AxisBoundsChanged += new EventHandler<ChartAxisBoundsEventArgs>(this.PositioningLabelsOnAxisLabels);
  }

  private void PositioningLabelsOnAxisLabels(object sender, ChartAxisBoundsEventArgs e)
  {
    CustomCategoryAxis customCategoryAxis = sender as CustomCategoryAxis;
    Canvas canvas = (Canvas) null;
    if (customCategoryAxis != null)
      canvas = customCategoryAxis.LabelsCanvas;
    if (canvas == null)
      return;
    for (int index = 0; index < canvas.Children.Count; ++index)
    {
      if (canvas.Children[index].Visibility == Visibility.Visible && canvas.Children[index] is FrameworkElement child && child.RenderTransform != null && child.RenderTransform is TransformGroup renderTransform)
      {
        RotateTransform child1 = renderTransform.Children[0] as RotateTransform;
        TranslateTransform child2 = renderTransform.Children[1] as TranslateTransform;
        if (child1 != null && child2 != null)
        {
          if (child1.Angle == -45.0)
          {
            if (child2.X == 0.0)
            {
              if (canvas.Children[index] is ContentControl child3)
                child2.X = -child3.DesiredSize.Width / 2.5;
            }
            else
              child2.X = -child2.X;
          }
          else if (child1.Angle == 90.0)
            child2.X = -child2.X;
        }
      }
    }
  }

  private void SfLogerthmicAxisCommon(
    ChartAxis sfaxis,
    ChartValueAxisImpl xlsiovalue,
    ChartCategoryAxisImpl xlsiocat)
  {
    if (!xlsiocat.IsAutoCross)
    {
      sfaxis.Origin = xlsiocat.CrossesAt - 2.0 + 0.5;
      sfaxis.ShowAxisNextToOrigin = true;
    }
    Style lineStyle;
    this.CheckAndApplyAxisLineStyle(xlsiovalue.Border as ChartBorderImpl, out lineStyle, xlsiovalue.ParentChart, ChartElementsEnum.AxisLine, true);
    sfaxis.AxisLineStyle = lineStyle;
    this.SfTickLines((ChartAxisImpl) xlsiovalue, sfaxis);
    if (!xlsiovalue.HasMajorGridLines)
      sfaxis.ShowGridLines = false;
    sfaxis.LabelRotationAngle = (double) xlsiovalue.TextRotationAngle;
    if ((xlsiovalue.TickLabelPosition == ExcelTickLabelPosition.TickLabelPosition_High || xlsiocat.ReversePlotOrder) && (xlsiovalue.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_High || !xlsiocat.ReversePlotOrder) || xlsiocat.IsMaxCross)
      sfaxis.OpposedPosition = true;
    this.TrySetValueAxisNumberFormat(xlsiovalue, sfaxis, true);
    if (xlsiovalue.ReversePlotOrder)
      sfaxis.IsInversed = true;
    if (!(xlsiovalue.NumberFormat != "General"))
      return;
    sfaxis.LabelFormat = xlsiovalue.NumberFormat;
  }

  internal void SfLogerthmicAxis(
    LogarithmicAxis sfaxis,
    ChartValueAxisImpl xlsiovalue,
    ChartCategoryAxisImpl xlsiocat)
  {
    this.SfLogerthmicAxisCommon((ChartAxis) sfaxis, xlsiovalue, xlsiocat);
    if (xlsiovalue.HasMinorGridLines)
      sfaxis.SmallTicksPerInterval = 3;
    if (xlsiovalue.MajorUnit <= 0.0)
      return;
    sfaxis.Interval = new double?(xlsiovalue.MajorUnit);
  }

  internal void SfLogerthmicAxis3D(
    LogarithmicAxis3D sfaxis,
    ChartValueAxisImpl xlsiovalue,
    ChartCategoryAxisImpl xlsiocat)
  {
    this.SfLogerthmicAxisCommon((ChartAxis) sfaxis, xlsiovalue, xlsiocat);
    if (xlsiovalue.HasMinorGridLines)
      sfaxis.SmallTicksPerInterval = 3;
    if (xlsiovalue.MajorUnit <= 0.0)
      return;
    sfaxis.Interval = new double?(xlsiovalue.MajorUnit);
  }

  internal void SfDateTimeAxis(
    CustomDateTimeAxis sfDateTimeAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis,
    bool condition)
  {
    this.SfCategoryAxisCommon((ChartAxis) sfDateTimeAxis, xlsioCatAxis, xlsioValAxis, condition);
    sfDateTimeAxis.LabelCreated += new EventHandler<LabelCreatedEventArgs>(this.OnAxisLabelCreated);
    if (xlsioCatAxis.IsAutoTextRotation)
    {
      sfDateTimeAxis.LabelsIntersectAction = AxisLabelsIntersectAction.Auto;
      sfDateTimeAxis.AxisBoundsChanged += new EventHandler<ChartAxisBoundsEventArgs>(this.CategoryAxis_AxisBoundsChanged);
      sfDateTimeAxis.IsVerticalAxis = this.IsVerticalCategoryAxis((ChartAxisImpl) xlsioCatAxis);
    }
    if (this.DateTimeIntervalType != DateTimeIntervalType.Auto)
      sfDateTimeAxis.IntervalType = this.DateTimeIntervalType;
    else if (!xlsioCatAxis.MajorUnitScaleIsAuto)
    {
      switch (xlsioCatAxis.MajorUnitScale)
      {
        case ExcelChartBaseUnit.Day:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Days;
          break;
        case ExcelChartBaseUnit.Month:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Months;
          break;
        case ExcelChartBaseUnit.Year:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Years;
          break;
      }
    }
    else if (!xlsioCatAxis.BaseUnitIsAuto)
    {
      switch (xlsioCatAxis.BaseUnit)
      {
        case ExcelChartBaseUnit.Day:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Days;
          break;
        case ExcelChartBaseUnit.Month:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Months;
          break;
        case ExcelChartBaseUnit.Year:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Years;
          break;
      }
    }
    if (!xlsioCatAxis.IsAutoMajor)
      sfDateTimeAxis.Interval = xlsioCatAxis.MajorUnit;
    else if (this.DateTimeIntervalType != DateTimeIntervalType.Auto || sfDateTimeAxis.IntervalType != DateTimeIntervalType.Auto)
      sfDateTimeAxis.Interval = 1.0;
    if (sfDateTimeAxis.IntervalType != DateTimeIntervalType.Auto && !xlsioCatAxis.IsAutoMinor && !xlsioCatAxis.MinorUnitScaleIsAuto)
    {
      int num = (int) (sfDateTimeAxis.IntervalType - 5);
      int minorUnitScale = (int) xlsioCatAxis.MinorUnitScale;
      if (num == minorUnitScale && minorUnitScale != 0)
      {
        sfDateTimeAxis.SmallTicksPerInterval = num / minorUnitScale - 1;
      }
      else
      {
        switch (num)
        {
          case 1:
            if (minorUnitScale == 0)
            {
              sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 30.0 / xlsioCatAxis.MinorUnit);
              break;
            }
            break;
          case 2:
            switch (minorUnitScale)
            {
              case 0:
                sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 365.0 / xlsioCatAxis.MinorUnit);
                break;
              case 1:
                sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 12.0 / xlsioCatAxis.MinorUnit);
                break;
            }
            break;
        }
      }
    }
    if (xlsioCatAxis.ChartValueRange == null)
      return;
    if (!xlsioCatAxis.IsAutoMax)
      sfDateTimeAxis.Maximum = new DateTime?(DateTime.FromOADate(xlsioCatAxis.ChartValueRange.NumMax));
    if (xlsioCatAxis.IsAutoMin)
      return;
    sfDateTimeAxis.Minimum = new DateTime?(DateTime.FromOADate(xlsioCatAxis.ChartValueRange.NumMin));
  }

  internal void SfDateTimeAxis3D(
    DateTimeAxis3D sfDateTimeAxis,
    ChartCategoryAxisImpl xlsioCatAxis,
    ChartValueAxisImpl xlsioValAxis)
  {
    this.SfCategoryAxisCommon((ChartAxis) sfDateTimeAxis, xlsioCatAxis, xlsioValAxis, true);
    sfDateTimeAxis.LabelCreated += new EventHandler<LabelCreatedEventArgs>(this.OnAxisLabelCreated);
    if (this.DateTimeIntervalType != DateTimeIntervalType.Auto)
      sfDateTimeAxis.IntervalType = this.DateTimeIntervalType;
    else if (!xlsioCatAxis.MajorUnitScaleIsAuto)
    {
      switch (xlsioCatAxis.MajorUnitScale)
      {
        case ExcelChartBaseUnit.Day:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Days;
          break;
        case ExcelChartBaseUnit.Month:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Months;
          break;
        case ExcelChartBaseUnit.Year:
          sfDateTimeAxis.IntervalType = DateTimeIntervalType.Years;
          break;
      }
    }
    if (!xlsioCatAxis.IsAutoMajor)
      sfDateTimeAxis.Interval = xlsioCatAxis.MajorUnit;
    else if (this.DateTimeIntervalType != DateTimeIntervalType.Auto)
      sfDateTimeAxis.Interval = 1.0;
    if (sfDateTimeAxis.IntervalType != DateTimeIntervalType.Auto && !xlsioCatAxis.IsAutoMinor && !xlsioCatAxis.MinorUnitScaleIsAuto)
    {
      int num = (int) (sfDateTimeAxis.IntervalType - 5);
      int minorUnitScale = (int) xlsioCatAxis.MinorUnitScale;
      if (num == minorUnitScale)
      {
        sfDateTimeAxis.SmallTicksPerInterval = num / minorUnitScale - 1;
      }
      else
      {
        switch (num)
        {
          case 1:
            if (minorUnitScale == 0)
            {
              sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 30.0 / xlsioCatAxis.MinorUnit);
              break;
            }
            break;
          case 2:
            switch (minorUnitScale)
            {
              case 0:
                sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 365.0 / xlsioCatAxis.MinorUnit);
                break;
              case 1:
                sfDateTimeAxis.SmallTicksPerInterval = (int) (sfDateTimeAxis.Interval * 12.0 / xlsioCatAxis.MinorUnit);
                break;
            }
            break;
        }
      }
    }
    if (xlsioCatAxis.ChartValueRange == null)
      return;
    if (!xlsioCatAxis.IsAutoMax)
      sfDateTimeAxis.Maximum = new DateTime?(DateTime.FromOADate(xlsioCatAxis.ChartValueRange.NumMax));
    if (xlsioCatAxis.IsAutoMin)
      return;
    sfDateTimeAxis.Minimum = new DateTime?(DateTime.FromOADate(xlsioCatAxis.ChartValueRange.NumMin));
  }

  private void SfTickLines(ChartAxisImpl xlsioAxis, ChartAxis sfAxis)
  {
    switch (xlsioAxis.MinorTickMark)
    {
      case ExcelTickMark.TickMark_None:
        sfAxis.MinorTickLineStyle = new Style()
        {
          TargetType = typeof (Line)
        };
        break;
      case ExcelTickMark.TickMark_Inside:
        sfAxis.TickLinesPosition = AxisElementPosition.Inside;
        sfAxis.MinorTickLineStyle = sfAxis.AxisLineStyle;
        break;
      case ExcelTickMark.TickMark_Outside:
      case ExcelTickMark.TickMark_Cross:
        sfAxis.TickLinesPosition = AxisElementPosition.Outside;
        sfAxis.MinorTickLineStyle = sfAxis.AxisLineStyle;
        break;
    }
    switch (xlsioAxis.MajorTickMark)
    {
      case ExcelTickMark.TickMark_None:
        sfAxis.MajorTickLineStyle = new Style()
        {
          TargetType = typeof (Line)
        };
        break;
      case ExcelTickMark.TickMark_Inside:
        sfAxis.TickLinesPosition = AxisElementPosition.Inside;
        sfAxis.MajorTickLineStyle = sfAxis.AxisLineStyle;
        break;
      case ExcelTickMark.TickMark_Outside:
      case ExcelTickMark.TickMark_Cross:
        sfAxis.TickLinesPosition = AxisElementPosition.Outside;
        sfAxis.MajorTickLineStyle = sfAxis.AxisLineStyle;
        break;
    }
  }

  internal Border GetSfAxisTitle(
    ChartAxisImpl axis,
    out RectangleF manualRect,
    out bool isVertical)
  {
    ChartTextAreaImpl titleArea = axis.TitleArea as ChartTextAreaImpl;
    ChartLayoutImpl layout = titleArea.Layout as ChartLayoutImpl;
    manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if (layout.IsManualLayout)
    {
      ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
      if (manualLayout.FlagOptions != (byte) 0 && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
        manualRect = this.CalculateManualLayout(manualLayout, out bool _, false);
    }
    Border brushForTextElements = this.GetBrushForTextElements(titleArea.FrameFormat.Border as ChartBorderImpl);
    TextBlock textBlock = new TextBlock();
    textBlock.Margin = new Thickness(2.0);
    isVertical = this.SetTransformAndBackGround(brushForTextElements, titleArea, this.IsChartEx ? (ChartAxisImpl) null : axis);
    if (titleArea.RichText != null && titleArea.RichText.FormattingRuns.Length > 1)
    {
      this.SetTextBlockInlines(titleArea, textBlock);
    }
    else
    {
      this.SfTextBlock(textBlock, axis.TitleArea as ChartTextAreaImpl);
      textBlock.Text = axis.Title != null ? (titleArea == null || !titleArea.IsFormula ? axis.Title : this.GetTextFromRange(axis.Title, titleArea.StringCache)) : "Axis Title";
    }
    if (brushForTextElements != null)
      brushForTextElements.Child = (UIElement) textBlock;
    return brushForTextElements;
  }

  private int GetAxisLayoutTransformForTitle(ChartAxisImpl axis, out bool isVertical)
  {
    isVertical = false;
    bool flag = axis.ParentChart.ChartType.ToString().ToLower().Contains("bar");
    ChartTextAreaImpl titleArea = axis.TitleArea as ChartTextAreaImpl;
    int num;
    if (!axis.AxisPosition.HasValue)
    {
      num = flag ? (axis is ChartCategoryAxisImpl ? 1 : 0) : (!(axis is ChartValueAxisImpl) ? 0 : (this.IsVerticalAxis(axis) ? 1 : 0));
    }
    else
    {
      ChartAxisPos? axisPosition1 = axis.AxisPosition;
      if ((axisPosition1.GetValueOrDefault() != ChartAxisPos.l ? 0 : (axisPosition1.HasValue ? 1 : 0)) == 0)
      {
        ChartAxisPos? axisPosition2 = axis.AxisPosition;
        num = axisPosition2.GetValueOrDefault() != ChartAxisPos.r ? 0 : (axisPosition2.HasValue ? 1 : 0);
      }
      else
        num = 1;
    }
    if (num == 0)
      return axis.TitleArea.TextRotationAngle;
    isVertical = true;
    return this.AxisTitleRotationAngle + (titleArea.HasTextRotation ? titleArea.TextRotationAngle : -90);
  }

  private bool IsVerticalAxis(ChartAxisImpl axis)
  {
    ChartValueAxisImpl primaryValueAxis = axis.ParentChart.PrimaryValueAxis as ChartValueAxisImpl;
    ChartValueAxisImpl secondaryValueAxis = axis.ParentChart.IsSecondaryValueAxisAvail ? axis.ParentChart.SecondaryValueAxis as ChartValueAxisImpl : (ChartValueAxisImpl) null;
    return axis.AxisId == primaryValueAxis.AxisId || secondaryValueAxis != null && secondaryValueAxis.AxisId == axis.AxisId;
  }

  private bool IsVerticalCategoryAxis(ChartAxisImpl axis)
  {
    ChartFormatImpl chartFormat = (axis.Parent as ChartParentAxisImpl).ChartFormats[0];
    return chartFormat.FormatRecordType == TBIFFRecord.ChartBar && chartFormat.IsHorizontalBar || axis.ParentChart.ChartType.ToString().Contains("Funnel");
  }

  internal ChartLegend SfLegend(
    ChartBase sfchart,
    ChartImpl xlsioChart,
    out int[] sortedLegendOrders,
    bool isPlotAreaManual,
    out ChartLegend emptyLegend)
  {
    ChartLegendImpl legend = xlsioChart.Legend as ChartLegendImpl;
    this.TryAndUpdateLegendItemsInWaterFall(sfchart, (IChart) xlsioChart);
    bool isDoughnut = !this.IsChart3D && !this.IsChartEx && sfchart.VisibleSeries.Count > 1 && sfchart.VisibleSeries.All<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x => x is DoughnutSeries));
    ChartLegend chartLegend1 = new ChartLegend();
    chartLegend1.FontSize = 12.0;
    chartLegend1.IconHeight = 9.75;
    chartLegend1.IconWidth = 9.75;
    if (!this.IsChartEx && sfchart.VisibleSeries.Count == 1 && xlsioChart.Series.Count == 1 && this.IsVaryColorSupported(xlsioChart.Series[0] as ChartSerieImpl) || isDoughnut)
      this.SetLegendItemsOnCategories(sfchart, isDoughnut);
    SfChart3D sfChart3D = sfchart as SfChart3D;
    emptyLegend = (ChartLegend) null;
    SfChartExt sfChart2D = sfchart as SfChartExt;
    ChartLayoutImpl layout = xlsioChart.Legend.Layout as ChartLayoutImpl;
    RectangleF manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if (layout.IsManualLayout)
    {
      ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
      if (manualLayout.FlagOptions != (byte) 0 && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
        manualRect = this.CalculateManualLayout(manualLayout, out bool _, false);
    }
    if ((double) manualRect.X < 0.0 || (double) manualRect.Y < 0.0)
    {
      this.SetLegendPosition((IChart) xlsioChart, chartLegend1);
      if (xlsioChart.Legend.IsVerticalLegend)
        chartLegend1.Orientation = ChartOrientation.Vertical;
    }
    if (!(xlsioChart.Legend.FrameFormat as ChartFrameFormatImpl).IsAutomaticFormat && xlsioChart.Legend.FrameFormat.Interior.Pattern != ExcelPattern.None)
      chartLegend1.Background = this.GetBrushFromDataFormat((IChartFillBorder) xlsioChart.Legend.FrameFormat);
    double size = xlsioChart.Legend.TextArea.Size;
    if (xlsioChart.Legend.TextArea.RGBColor.ToArgb() != System.Drawing.Color.Empty.ToArgb())
    {
      ChartTextAreaImpl textArea = xlsioChart.Legend.TextArea as ChartTextAreaImpl;
      System.Drawing.Color color = !textArea.IsAutoColor || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, xlsioChart, out color) || (int) color.R != (int) color.G || (int) color.B != (int) color.R || color.R != (byte) 0 ? textArea.Font.RGBColor : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      chartLegend1.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    }
    double num1 = ApplicationImpl.ConvertUnitsStatic(size, MeasureUnits.Point, MeasureUnits.Pixel);
    chartLegend1.FontSize = num1;
    this.FontName = xlsioChart.Legend.TextArea.FontName;
    if (this.FontName.StartsWith("+"))
    {
      bool flag = this.FontName.Contains("+mn-");
      if ((flag ? (this.parentWorkbook.MinorFonts != null ? 1 : 0) : (this.parentWorkbook.MajorFonts != null ? 1 : 0)) != 0)
      {
        string key = this.FontName.Replace(flag ? "+mn-" : "+mj-", "");
        if ((flag ? (this.parentWorkbook.MinorFonts.ContainsKey(key) ? 1 : 0) : (this.parentWorkbook.MajorFonts.ContainsKey(key) ? 1 : 0)) != 0)
          this.FontName = (flag ? this.parentWorkbook.MinorFonts[key] : this.parentWorkbook.MajorFonts[key]).FontName;
        else if (flag && key == "lt" && this.parentWorkbook.MinorFonts.ContainsKey("latin"))
          this.FontName = this.parentWorkbook.MinorFonts["latin"].FontName;
      }
      if (this.FontName.StartsWith("+"))
        this.FontName = "Calibri";
    }
    chartLegend1.FontFamily = new System.Windows.Media.FontFamily(this.FontName);
    ChartBorderImpl border = xlsioChart.Legend.FrameFormat.Border as ChartBorderImpl;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      if (border.HasLineProperties)
      {
        double borderThickness = this.GetBorderThickness(border);
        chartLegend1.BorderThickness = new Thickness(borderThickness < 1.0 ? 1.0 : borderThickness);
      }
      if (border.IsAutoLineColor && !border.HasGradientFill && this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
      {
        chartLegend1.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
        chartLegend1.BorderThickness = new Thickness(1.0);
      }
      else
        chartLegend1.BorderBrush = this.GetBrushFromBorder(border);
    }
    else
      chartLegend1.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    string str = xlsioChart.ChartType.ToString();
    if (str.Contains("Line"))
    {
      chartLegend1.IconWidth = num1 * 2.0;
      chartLegend1.IconHeight = num1;
    }
    else if (!str.Contains("Combination"))
      chartLegend1.IconHeight = chartLegend1.IconWidth = num1 / 2.0;
    TextBlock block = new TextBlock();
    block.FontFamily = chartLegend1.FontFamily;
    block.FontSize = chartLegend1.FontSize;
    block.FontWeight = chartLegend1.FontWeight;
    block.FontStyle = chartLegend1.FontStyle;
    if (!this.IsChartEx && (double) manualRect.X >= 0.0 && (double) manualRect.Y >= 0.0)
    {
      chartLegend1.LegendPosition = LegendPosition.Inside;
      chartLegend1.DockPosition = ChartDock.Left;
      chartLegend1.VerticalAlignment = VerticalAlignment.Top;
      chartLegend1.HorizontalAlignment = HorizontalAlignment.Left;
      chartLegend1.Margin = new Thickness((double) manualRect.X, (double) manualRect.Y, 0.0, 0.0);
      this.IsLegendManualLayout = true;
      int position = (int) xlsioChart.Legend.Position;
      if ((double) manualRect.Width <= 0.0 || (double) manualRect.Height <= 0.0)
        chartLegend1.Orientation = position == 0 || position == 2 ? ChartOrientation.Horizontal : ChartOrientation.Vertical;
    }
    if (this.IsChartEx)
    {
      if (xlsioChart.Legend.IncludeInLayout)
        chartLegend1.LegendPosition = LegendPosition.Inside;
    }
    else if (isPlotAreaManual || !xlsioChart.Legend.IncludeInLayout)
      chartLegend1.LegendPosition = LegendPosition.Inside;
    sortedLegendOrders = sfchart.VisibleSeries.Count > 1 ? this.GetOrderOfLegendItems(sfchart, (IChart) xlsioChart, chartLegend1) : (int[]) null;
    Dictionary<int, ChartLegendEntryImpl> hashEntries = (legend.LegendEntries as ChartLegendEntriesColl).HashEntries;
    int count = hashEntries.Count;
    if (hashEntries.Count > 0)
    {
      List<IChartSerie> chartSerieList = xlsioChart.Series.OrderByType();
      IEnumerable<IGrouping<int, IChartSerie>> groupings = xlsioChart.Series.Where<IChartSerie>((Func<IChartSerie, bool>) (x => !x.IsFiltered)).GroupBy<IChartSerie, int, IChartSerie>((Func<IChartSerie, int>) (x => (x as ChartSerieImpl).ChartGroup), (Func<IChartSerie, IChartSerie>) (x => x));
      if (xlsioChart.ChartSerieGroupsBeforesorting != null)
        groupings = xlsioChart.ChartSerieGroupsBeforesorting;
      foreach (KeyValuePair<int, ChartLegendEntryImpl> keyValuePair in hashEntries)
      {
        if (keyValuePair.Value.IsDeleted)
        {
          int num2 = 0;
          foreach (IGrouping<int, IChartSerie> source in groupings)
          {
            int num3 = source.Count<IChartSerie>();
            num2 += num3;
            if (keyValuePair.Key < num2)
            {
              int index1 = keyValuePair.Key - (num2 - num3);
              IChartSerie chartSerie = source.ElementAt<IChartSerie>(index1);
              if (chartSerie != null)
              {
                for (int index2 = 0; index2 < chartSerieList.Count; ++index2)
                {
                  if (chartSerieList[index2].Name == chartSerie.Name)
                  {
                    index1 = index2;
                    break;
                  }
                }
                if (this.HasNewSeries && index1 >= this.NewSeriesIndex)
                  ++index1;
                if (sfChart3D != null && index1 < sfChart3D.Series.Count)
                  sfChart3D.Series[index1].VisibilityOnLegend = Visibility.Collapsed;
                else if (sfChart2D != null && index1 < sfChart2D.Series.Count)
                  sfChart2D.Series[index1].VisibilityOnLegend = Visibility.Collapsed;
                --count;
                break;
              }
              break;
            }
          }
        }
      }
      this.HasNewSeries = false;
      this.NewSeriesIndex = 0;
    }
    if ((double) manualRect.X < 0.0 || (double) manualRect.Y < 0.0)
      chartLegend1.Margin = new Thickness(3.0, 10.0, 3.0, 2.0);
    if (this.IsLegendManualLayout && (double) manualRect.Width > 0.0 && (double) manualRect.Height > 0.0)
    {
      this.SetLegendManualLayoutSize(chartLegend1, sfChart2D, sfChart3D, block, manualRect);
    }
    else
    {
      bool flag = sortedLegendOrders != null && sfchart is SfChart3D && (chartLegend1.DockPosition == ChartDock.Left || chartLegend1.DockPosition == ChartDock.Right);
      if (chartLegend1.DockPosition != ChartDock.Left && chartLegend1.DockPosition != ChartDock.Right || flag)
      {
        ChartLegend chartLegend2 = chartLegend1;
        ItemsPanelTemplate itemsPanelTemplate1 = new ItemsPanelTemplate();
        itemsPanelTemplate1.VisualTree = new FrameworkElementFactory(typeof (WrapPanel));
        ItemsPanelTemplate itemsPanelTemplate2 = itemsPanelTemplate1;
        chartLegend2.ItemsPanel = itemsPanelTemplate2;
        double serieWidth = 0.0;
        double totalWidth = 0.0;
        int totalCount = 0;
        double num4 = 4.0 / 3.0;
        this.CalculateLegendSize(sfChart2D, sfChart3D, block, out serieWidth, out double _, out totalWidth, out totalCount);
        double num5 = chartLegend1.IconWidth + 10.0 + 2.0 * chartLegend1.BorderThickness.Left;
        if ((double) this.ChartWidth < totalWidth + (double) totalCount * num5 || flag)
        {
          chartLegend1.ItemTemplate = new DataTemplate((object) typeof (StackPanel));
          FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (StackPanel));
          chartLegend1.ItemTemplate.VisualTree = frameworkElementFactory;
          frameworkElementFactory.SetValue(StackPanel.OrientationProperty, (object) Orientation.Horizontal);
          frameworkElementFactory.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding("VisibilityOnLegend"));
          frameworkElementFactory.SetBinding(FrameworkElement.MarginProperty, (BindingBase) new Binding("ItemMargin"));
          frameworkElementFactory.SetValue(FrameworkElement.WidthProperty, (object) (serieWidth + num5));
          FrameworkElementFactory child1 = new FrameworkElementFactory(typeof (ContentPresenter));
          child1.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding("IconWidth"));
          child1.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding("IconHeight"));
          child1.SetBinding(ContentPresenter.ContentProperty, (BindingBase) new Binding());
          child1.SetValue(FrameworkElement.MarginProperty, (object) new Thickness(2.0));
          child1.SetBinding(ContentPresenter.ContentTemplateProperty, (BindingBase) new Binding("LegendIconTemplate"));
          FrameworkElementFactory child2 = new FrameworkElementFactory(typeof (TextBlock));
          child2.SetBinding(TextBlock.TextProperty, (BindingBase) new Binding("Label"));
          child2.SetValue(FrameworkElement.MarginProperty, (object) new Thickness(2.0));
          child2.SetValue(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Center);
          FrameworkElementFactory child3 = new FrameworkElementFactory(typeof (Grid));
          child3.AppendChild(child1);
          frameworkElementFactory.AppendChild(child3);
          frameworkElementFactory.AppendChild(child2);
          if (flag)
            chartLegend1.ItemsPanel.VisualTree.SetValue(FrameworkElement.WidthProperty, (object) (serieWidth + num5 * num4));
          else
            chartLegend1.ItemsPanel.VisualTree.SetValue(FrameworkElement.WidthProperty, (object) ((serieWidth + num5) * (double) (int) ((double) this.ChartWidth / (serieWidth + num5 * num4))));
        }
      }
      else if (chartLegend1.VerticalAlignment != VerticalAlignment.Top && count > 1 && (chartLegend1.DockPosition == ChartDock.Left || chartLegend1.DockPosition == ChartDock.Right))
      {
        block.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
        if ((double) count * (block.DesiredSize.Height + 3.0) > 0.7 * (double) this.ChartHeight)
          chartLegend1.VerticalAlignment = VerticalAlignment.Top;
      }
    }
    return chartLegend1;
  }

  private void SetLegendItemsOnCategories(ChartBase sfchart, bool isDoughnut)
  {
    ChartSeriesBase chartSeriesBase1 = sfchart.VisibleSeries[0];
    if (!isDoughnut)
    {
      switch (chartSeriesBase1)
      {
        case CircularSeriesBase _:
          return;
        case CircularSeriesBase3D _:
          return;
      }
    }
    ObservableCollection<ChartPoint> itemsSource = chartSeriesBase1.ItemsSource as ObservableCollection<ChartPoint>;
    bool flag1 = chartSeriesBase1.ColorModel != null && chartSeriesBase1.Palette == ChartColorPalette.Custom && chartSeriesBase1.ColorModel.CustomBrushes.Count == itemsSource.Count;
    bool flag2 = itemsSource.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.SegmentBrush != null)) == itemsSource.Count;
    if (!flag1 && !flag2)
      return;
    chartSeriesBase1.VisibilityOnLegend = Visibility.Collapsed;
    if (isDoughnut)
    {
      foreach (ChartSeriesBase chartSeriesBase2 in (Collection<ChartSeriesBase>) sfchart.VisibleSeries)
        chartSeriesBase2.VisibilityOnLegend = Visibility.Collapsed;
    }
    List<System.Windows.Media.Brush> customBrushes = chartSeriesBase1.ColorModel.CustomBrushes;
    string str = chartSeriesBase1.GetType().ToString();
    ViewModel viewModel = new ViewModel(0);
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    if (!isDoughnut)
    {
      if (chartSeriesBase1 is CartesianSeries || chartSeriesBase1 is CartesianSeries3D)
      {
        if (!str.Contains("Bar"))
          flag4 = true;
        else
          flag5 = true;
      }
      else if (str.Contains("Radar"))
        flag3 = true;
    }
    for (int index = 0; index < itemsSource.Count; ++index)
    {
      System.Windows.Media.Brush brush = flag2 ? itemsSource[index].SegmentBrush : (index < customBrushes.Count ? customBrushes[index] : chartSeriesBase1.Interior);
      ChartSeriesBase chartSeriesBase3 = (ChartSeriesBase) null;
      if (this.IsChart3D)
      {
        if (flag4)
        {
          chartSeriesBase3 = (ChartSeriesBase) new LineSeries3D();
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
        else if (flag5)
        {
          chartSeriesBase3 = (ChartSeriesBase) new BarSeries3D();
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
      }
      else
      {
        ChartSeries chartSeries = chartSeriesBase1 as ChartSeries;
        if (isDoughnut)
        {
          chartSeriesBase3 = (ChartSeriesBase) new DoughnutSeries();
          if (chartSeries != null)
          {
            chartSeriesBase3.SetValue(ChartSeries.StrokeProperty, (object) chartSeries.Stroke);
            chartSeriesBase3.SetValue(ChartSeries.StrokeThicknessProperty, (object) chartSeries.StrokeThickness);
          }
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
        else if (flag4)
        {
          chartSeriesBase3 = (ChartSeriesBase) new LineSeries();
          if (chartSeries != null)
          {
            chartSeriesBase3.SetValue(ChartSeries.StrokeProperty, (object) chartSeries.Stroke);
            chartSeriesBase3.SetValue(ChartSeries.StrokeThicknessProperty, (object) chartSeries.StrokeThickness);
          }
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
        else if (flag5)
        {
          chartSeriesBase3 = (ChartSeriesBase) new BarSeries();
          if (chartSeries != null)
          {
            chartSeriesBase3.SetValue(ChartSeries.StrokeProperty, (object) chartSeries.Stroke);
            chartSeriesBase3.SetValue(ChartSeries.StrokeThicknessProperty, (object) chartSeries.StrokeThickness);
          }
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
        else if (flag3)
        {
          chartSeriesBase3 = (ChartSeriesBase) new RadarSeries();
          if (chartSeries != null)
          {
            chartSeriesBase3.SetValue(ChartSeries.StrokeProperty, (object) chartSeries.Stroke);
            chartSeriesBase3.SetValue(ChartSeries.StrokeThicknessProperty, (object) 1.0);
          }
          chartSeriesBase3.SetValue(ChartSeriesBase.IsSeriesVisibleProperty, (object) false);
        }
      }
      if (chartSeriesBase3 != null)
      {
        chartSeriesBase3.SetValue(ChartSeriesBase.LabelProperty, itemsSource[index].X == null ? (object) "" : (object) itemsSource[index].X.ToString());
        chartSeriesBase3.SetValue(ChartSeriesBase.InteriorProperty, (object) brush);
        chartSeriesBase3.SetValue(ChartSeriesBase.ItemsSourceProperty, (object) viewModel.Products);
        chartSeriesBase3.SetValue(ChartSeriesBase.LegendIconProperty, (object) chartSeriesBase1.LegendIcon);
        if (this.IsChart3D)
        {
          (sfchart as SfChart3D).Series.Add(chartSeriesBase3 as ChartSeries3D);
          if (chartSeriesBase1 is LineSeries3D)
            chartSeriesBase3.SetValue(ChartSeriesBase.LegendIconTemplateProperty, (object) chartSeriesBase1.LegendIconTemplate);
        }
        else
          (sfchart as SfChartExt).Series.Add(chartSeriesBase3 as ChartSeries);
      }
    }
  }

  private void SetLegendManualLayoutSize(
    ChartLegend leg,
    SfChartExt sfChart2D,
    SfChart3D sfChart3D,
    TextBlock block,
    RectangleF manualRect)
  {
    double serieWidth = 0.0;
    double totalWidth = 0.0;
    int totalCount = 0;
    double serieHeight;
    this.CalculateLegendSize(sfChart2D, sfChart3D, block, out serieWidth, out serieHeight, out totalWidth, out totalCount);
    double a = leg.IconWidth + 4.0 + 2.0 * leg.BorderThickness.Left;
    leg.Width = (double) manualRect.Width;
    leg.Height = serieHeight > (double) manualRect.Height ? serieHeight + leg.BorderThickness.Top : (double) manualRect.Height;
    double left = leg.Width > totalWidth + (double) totalCount * a ? (leg.Width - (totalWidth + (double) totalCount * a)) / (double) (totalCount + 1) : 0.0;
    leg.Padding = new Thickness(0.0);
    leg.ItemMargin = new Thickness(left, 0.0, 0.0, 0.0);
    double num = -1.0;
    FrameworkElementFactory root = new FrameworkElementFactory(typeof (CustomWrapPanel));
    ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(root);
    root.SetValue(FrameworkElement.WidthProperty, (object) leg.Width);
    root.SetValue(FrameworkElement.HeightProperty, (object) leg.Height);
    if (serieWidth + a >= leg.Width)
    {
      num = leg.Width - Math.Ceiling(a);
      root.SetValue(CustomWrapPanel.IsTextWrappedProperty, (object) true);
    }
    leg.ItemsPanel = itemsPanelTemplate;
    block.Text = "a";
    block.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
    string str = Math.Ceiling(block.BaselineOffset - leg.IconHeight).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    StringBuilder sb = new StringBuilder();
    System.Windows.Media.Brush brush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 1.0));
    sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
    sb.Append($"<WrapPanel Visibility=\"{{Binding VisibilityOnLegend}}\" Margin=\"{{Binding ItemMargin}}\" Background=\"{brush.ToString()}\"  HorizontalAlignment=\"Left\" VerticalAlignment=\"Center\" Orientation=\"Horizontal\">");
    sb.Append($"<ContentPresenter Width=\"{{Binding IconWidth}}\"  Height=\"{{Binding IconHeight}}\" Content=\"{{Binding}}\"  Margin=\"2,{str},2,{str}\" ContentTemplate=\"{{Binding LegendIconTemplate}}\" VerticalAlignment=\"Top\" />");
    sb.Append($"<Label FontSize=\"{block.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}\" FontFamily=\"{block.FontFamily.ToString()}\" Foreground=\"{leg.Foreground.ToString()}\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Left\"  Margin=\"0\"  Background=\"{brush.ToString()}\" {(num == -1.0 ? "" : $" Width=\"{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}\" ")}Padding=\"0\"><Label.Content><AccessText  Text=\"{{Binding Label}}\"  TextWrapping=\"Wrap\"/></Label.Content></Label></WrapPanel></DataTemplate>");
    leg.ItemTemplate = (DataTemplate) XamlReader.Load(this.GenerateStreamFromXamlContent(sb));
  }

  internal Stream GenerateStreamFromXamlContent(StringBuilder sb)
  {
    MemoryStream streamFromXamlContent = new MemoryStream();
    StreamWriter streamWriter = new StreamWriter((Stream) streamFromXamlContent);
    streamWriter.Write(sb.ToString());
    streamWriter.Flush();
    streamFromXamlContent.Position = 0L;
    return (Stream) streamFromXamlContent;
  }

  private void CalculateLegendSize(
    SfChartExt sfChart2D,
    SfChart3D sfChart3D,
    TextBlock block,
    out double serieWidth,
    out double serieHeight,
    out double totalWidth,
    out int totalCount)
  {
    totalWidth = 0.0;
    serieWidth = 0.0;
    serieHeight = 0.0;
    totalCount = 0;
    if (sfChart2D != null)
    {
      if (sfChart2D.Series.Count == 1 && sfChart2D.Series[0] is CircularSeriesBase)
      {
        foreach (ChartPoint chartPoint in (Collection<ChartPoint>) (sfChart2D.Series[0].ItemsSource as ObservableCollection<ChartPoint>))
        {
          block.Text = chartPoint.X != null ? chartPoint.X.ToString() : "";
          block.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
          if (block.DesiredSize.Width > serieWidth)
            serieWidth = block.DesiredSize.Width;
          if (block.DesiredSize.Height > serieHeight)
            serieHeight = block.DesiredSize.Height;
          totalWidth += block.DesiredSize.Width;
          ++totalCount;
        }
      }
      else
      {
        foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) sfChart2D.Series)
        {
          if (chartSeriesBase.VisibilityOnLegend == Visibility.Visible)
          {
            block.Text = chartSeriesBase.Label;
            block.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            if (block.DesiredSize.Width > serieWidth)
              serieWidth = block.DesiredSize.Width;
            if (block.DesiredSize.Height > serieHeight)
              serieHeight = block.DesiredSize.Height;
            totalWidth += block.DesiredSize.Width;
            ++totalCount;
          }
        }
      }
    }
    else
    {
      if (sfChart3D == null)
        return;
      if (sfChart3D.Series.Count == 1 && sfChart3D.Series[0] is CircularSeriesBase3D)
      {
        foreach (ChartPoint chartPoint in (Collection<ChartPoint>) (sfChart3D.Series[0].ItemsSource as ObservableCollection<ChartPoint>))
        {
          block.Text = chartPoint.X != null ? chartPoint.X.ToString() : "";
          block.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
          if (block.DesiredSize.Width > serieWidth)
            serieWidth = block.DesiredSize.Width;
          if (block.DesiredSize.Height > serieHeight)
            serieHeight = block.DesiredSize.Height;
          totalWidth += block.DesiredSize.Width;
          ++totalCount;
        }
      }
      else
      {
        foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries3D>) sfChart3D.Series)
        {
          if (chartSeriesBase.VisibilityOnLegend == Visibility.Visible)
          {
            block.Text = chartSeriesBase.Label;
            block.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            if (block.DesiredSize.Width > serieWidth)
              serieWidth = block.DesiredSize.Width;
            if (block.DesiredSize.Height > serieHeight)
              serieHeight = block.DesiredSize.Height;
            totalWidth += block.DesiredSize.Width;
            ++totalCount;
          }
        }
      }
    }
  }

  private void SetLegendPosition(IChart xlsioChart, ChartLegend leg)
  {
    switch (xlsioChart.Legend.Position)
    {
      case ExcelLegendPosition.Bottom:
        leg.DockPosition = ChartDock.Bottom;
        leg.VerticalAlignment = VerticalAlignment.Bottom;
        break;
      case ExcelLegendPosition.Corner:
        leg.VerticalAlignment = VerticalAlignment.Top;
        leg.HorizontalAlignment = HorizontalAlignment.Right;
        leg.Orientation = ChartOrientation.Vertical;
        leg.DockPosition = ChartDock.Right;
        break;
      case ExcelLegendPosition.Top:
        leg.DockPosition = ChartDock.Top;
        break;
      case ExcelLegendPosition.Right:
        leg.DockPosition = ChartDock.Right;
        break;
      case ExcelLegendPosition.Left:
        leg.DockPosition = ChartDock.Left;
        break;
    }
  }

  private int[] GetOrderOfLegendItems(ChartBase sfchart, IChart chart, ChartLegend legend)
  {
    int[] orderOfLegendItems = (int[]) null;
    string str = chart.ChartType.ToString();
    int count = sfchart.VisibleSeries.Count;
    bool flag1 = false;
    if (chart.Legend.Position == ExcelLegendPosition.Corner || chart.Legend.Position == ExcelLegendPosition.Right || chart.Legend.Position == ExcelLegendPosition.Left)
      flag1 = true;
    if (str.Contains("Bar") && str.Contains("Clustered"))
    {
      orderOfLegendItems = new int[count];
      for (int index = 0; index < count; ++index)
        orderOfLegendItems[index] = count - 1 - index;
    }
    else if ((this.IsLegendManualLayout ? (!chart.Legend.IsVerticalLegend ? 0 : (flag1 ? 1 : 0)) : (legend.DockPosition == ChartDock.Left ? 1 : (legend.DockPosition == ChartDock.Right ? 1 : 0))) != 0 && sfchart.VisibleSeries.Count<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x => x.GetType().ToString().Contains("StackingColumn") || x.GetType().ToString().Contains("StackingArea"))) > 1)
    {
      bool flag2 = sfchart is SfChart3D;
      orderOfLegendItems = new int[count];
      if (flag2)
      {
        for (int index = 0; index < count; ++index)
          orderOfLegendItems[index] = count - 1 - index;
      }
      else
      {
        IList<ChartSeriesBase> list1 = (IList<ChartSeriesBase>) sfchart.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x => x.GetType().ToString().Contains("StackingArea") && x is CartesianSeries && (x as CartesianSeries).YAxis == null && (x as CartesianSeries).XAxis == null)).ToList<ChartSeriesBase>();
        IList<ChartSeriesBase> list2 = (IList<ChartSeriesBase>) sfchart.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x => x.GetType().ToString().Contains("StackingColumn") && x is CartesianSeries && (x as CartesianSeries).YAxis == null && (x as CartesianSeries).XAxis == null)).ToList<ChartSeriesBase>();
        IList<ChartSeriesBase> list3 = (IList<ChartSeriesBase>) sfchart.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x =>
        {
          if (!x.GetType().ToString().Contains("StackingArea") || !(x is CartesianSeries))
            return false;
          return (x as CartesianSeries).YAxis != null || (x as CartesianSeries).XAxis != null;
        })).ToList<ChartSeriesBase>();
        IList<ChartSeriesBase> list4 = (IList<ChartSeriesBase>) sfchart.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (x =>
        {
          if (!x.GetType().ToString().Contains("StackingColumn") || !(x is CartesianSeries))
            return false;
          return (x as CartesianSeries).YAxis != null || (x as CartesianSeries).XAxis != null;
        })).ToList<ChartSeriesBase>();
        int index1 = 0;
        this.UpdateOrderArray(list1, sfchart, index1, out index1, orderOfLegendItems);
        this.UpdateOrderArray(list3, sfchart, index1, out index1, orderOfLegendItems);
        this.UpdateOrderArray(list2, sfchart, index1, out index1, orderOfLegendItems);
        this.UpdateOrderArray(list4, sfchart, index1, out index1, orderOfLegendItems);
        if (index1 < count)
        {
          for (int index2 = 0; index2 < count; ++index2)
          {
            if (!((IEnumerable<int>) orderOfLegendItems).Contains<int>(index2))
            {
              orderOfLegendItems[index1] = index2;
              ++index1;
            }
            if (index1 >= count)
              break;
          }
        }
      }
    }
    return orderOfLegendItems;
  }

  private void UpdateOrderArray(
    IList<ChartSeriesBase> seriesList,
    ChartBase sfchart,
    int i,
    out int index,
    int[] orderResult)
  {
    if (seriesList != null && seriesList.Count > 0)
    {
      for (int index1 = seriesList.Count - 1; index1 >= 0; --index1)
      {
        orderResult[i] = sfchart.VisibleSeries.IndexOf(seriesList[index1]);
        ++i;
      }
    }
    index = i;
  }

  internal RectangleF CalculateManualLayout(
    ChartManualLayoutImpl layoutImpl,
    out bool isInnerLayout,
    bool isTrendLine)
  {
    isInnerLayout = false;
    if (this.IsChartEx)
      return new RectangleF(-1f, -1f, -1f, -1f);
    float x = -1f;
    float y = -1f;
    if (((int) layoutImpl.FlagOptions & 1) != 0)
    {
      if (layoutImpl.TopMode == LayoutModes.edge)
        y = (float) Math.Floor((double) this.ChartHeight * layoutImpl.Top);
      else if (!isTrendLine)
      {
        if (layoutImpl.Top >= -0.5 && layoutImpl.Top <= 0.5)
          y = (float) Math.Floor((double) this.ChartHeight * 0.9 * (0.5 + layoutImpl.Top));
      }
      else
      {
        float num = (float) Math.Floor((double) this.ChartHeight * 0.4 * (0.5 + layoutImpl.Top) + (double) this.ChartHeight * layoutImpl.Top);
        y = (double) num <= 0.0 ? 0.0f : num;
      }
    }
    if (((int) layoutImpl.FlagOptions & 2) != 0)
    {
      if (layoutImpl.LeftMode == LayoutModes.edge)
        x = (float) Math.Floor((double) this.ChartWidth * layoutImpl.Left);
      else if (!isTrendLine)
      {
        if (layoutImpl.Left >= -1.0 && layoutImpl.Left <= 0.0)
          x = (float) Math.Floor((double) this.ChartWidth * 0.8 * (1.0 + layoutImpl.Left));
      }
      else
      {
        float num = (float) Math.Floor((double) this.ChartWidth * 0.7 * (1.0 + layoutImpl.Left) + (double) this.ChartWidth * layoutImpl.Left);
        x = (double) num <= 0.0 ? 0.0f : num;
      }
    }
    float width;
    if (((int) layoutImpl.FlagOptions & 8) != 0)
    {
      width = (float) Math.Floor((double) this.ChartWidth * layoutImpl.Width);
      if (layoutImpl.WidthMode == LayoutModes.edge)
        width -= x;
      if ((double) width == 0.0)
        width = -1f;
    }
    else
      width = 0.0f;
    float height;
    if (((int) layoutImpl.FlagOptions & 4) != 0)
    {
      height = (float) Math.Floor((double) this.ChartHeight * layoutImpl.Height);
      if (layoutImpl.HeightMode == LayoutModes.edge)
        height -= y;
      if ((double) height == 0.0)
        height = -1f;
    }
    else
      height = 0.0f;
    if (((int) layoutImpl.FlagOptions & 16 /*0x10*/) != 0 && layoutImpl.LayoutTarget == LayoutTargets.inner)
      isInnerLayout = true;
    return (double) x < 0.0 || (double) y < 0.0 || (double) width < 0.0 || (double) height < 0.0 ? new RectangleF(-1f, -1f, -1f, -1f) : new RectangleF(x, y, width, height);
  }

  private void TryAndUpdateLegendItemsInWaterFall(ChartBase sfchart, IChart xlsioChart)
  {
    if (xlsioChart.ChartType != ExcelChartType.WaterFall || sfchart.VisibleSeries.Count <= 0 || !(sfchart.VisibleSeries[0] is CustomWaterfallSeries) || !(sfchart is SfChartExt sfChartExt))
      return;
    ViewModel viewModel = new ViewModel(0);
    CustomWaterfallSeries customWaterfallSeries = sfChartExt.Series[0] as CustomWaterfallSeries;
    bool flag = customWaterfallSeries.DefaultSummaryBrush == null && customWaterfallSeries.DefaultNegativeBrush == null;
    sfChartExt.Series[0].VisibilityOnLegend = Visibility.Collapsed;
    Syncfusion.UI.Xaml.Charts.ChartSeriesCollection series1 = sfChartExt.Series;
    LineSeries lineSeries1 = new LineSeries();
    lineSeries1.Interior = customWaterfallSeries.DefaultInterior;
    lineSeries1.Stroke = customWaterfallSeries.Stroke;
    lineSeries1.ItemsSource = (object) viewModel.Products;
    lineSeries1.StrokeThickness = customWaterfallSeries.StrokeThickness;
    lineSeries1.Visibility = Visibility.Collapsed;
    lineSeries1.Label = "Increase";
    LineSeries lineSeries2 = lineSeries1;
    series1.Add((ChartSeries) lineSeries2);
    Syncfusion.UI.Xaml.Charts.ChartSeriesCollection series2 = sfChartExt.Series;
    LineSeries lineSeries3 = new LineSeries();
    lineSeries3.Interior = flag ? customWaterfallSeries.DefaultInterior : customWaterfallSeries.DefaultNegativeBrush;
    lineSeries3.ItemsSource = (object) viewModel.Products;
    lineSeries3.Stroke = customWaterfallSeries.Stroke;
    lineSeries3.StrokeThickness = customWaterfallSeries.StrokeThickness;
    lineSeries3.Visibility = Visibility.Collapsed;
    lineSeries3.Label = "Decrease";
    LineSeries lineSeries4 = lineSeries3;
    series2.Add((ChartSeries) lineSeries4);
    Syncfusion.UI.Xaml.Charts.ChartSeriesCollection series3 = sfChartExt.Series;
    LineSeries lineSeries5 = new LineSeries();
    lineSeries5.Interior = flag ? customWaterfallSeries.DefaultInterior : customWaterfallSeries.DefaultSummaryBrush;
    lineSeries5.ItemsSource = (object) viewModel.Products;
    lineSeries5.Stroke = customWaterfallSeries.Stroke;
    lineSeries5.StrokeThickness = customWaterfallSeries.StrokeThickness;
    lineSeries5.Visibility = Visibility.Collapsed;
    lineSeries5.Label = "Total";
    LineSeries lineSeries6 = lineSeries5;
    series3.Add((ChartSeries) lineSeries6);
  }

  internal void SfTextBlock(TextBlock sfTextArea, ChartTextAreaImpl textArea)
  {
    this.FontName = textArea.FontName;
    System.Drawing.Color color = !textArea.IsAutoColor || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, textArea.FindParent(typeof (ChartImpl)) as ChartImpl, out color) || (int) color.R != (int) color.G || (int) color.B != (int) color.R || color.R != (byte) 0 ? textArea.Font.RGBColor : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    if (this.FontName.StartsWith("+"))
      this.FontName = "Calibri";
    sfTextArea.FontFamily = new System.Windows.Media.FontFamily(textArea.FontName);
    sfTextArea.FontSize = ApplicationImpl.ConvertUnitsStatic(textArea.Size, MeasureUnits.Point, MeasureUnits.Pixel);
    if (textArea.Bold)
      sfTextArea.FontWeight = FontWeights.Bold;
    if (textArea.Italic)
      sfTextArea.FontStyle = FontStyles.Italic;
    sfTextArea.TextAlignment = TextAlignment.Center;
    if (textArea.CapitalizationType == TextCapsType.All || textArea.CapitalizationType == TextCapsType.Small)
      sfTextArea.Typography.Capitals = FontCapitals.AllSmallCaps;
    if (textArea.Underline == ExcelUnderline.Single)
      sfTextArea.TextDecorations.Add(new TextDecoration()
      {
        Location = TextDecorationLocation.Underline
      });
    if (textArea.Strikethrough)
      sfTextArea.TextDecorations.Add(new TextDecoration()
      {
        Location = TextDecorationLocation.Strikethrough
      });
    if (color.ToArgb() == System.Drawing.Color.Empty.ToArgb())
      return;
    sfTextArea.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
  }

  internal bool SetTransformAndBackGround(
    Border sfTextArea,
    ChartTextAreaImpl textArea,
    ChartAxisImpl axis)
  {
    bool isVertical = false;
    if (axis != null)
    {
      RotateTransform rotateTransform = new RotateTransform()
      {
        Angle = (double) this.GetAxisLayoutTransformForTitle(axis, out isVertical)
      };
      sfTextArea.LayoutTransform = (Transform) rotateTransform;
    }
    else if (!(textArea.Parent is ChartAxisImpl))
      sfTextArea.LayoutTransform = (Transform) new RotateTransform((double) textArea.TextRotationAngle);
    if (!(textArea.FrameFormat as ChartFrameFormatImpl).IsAutomaticFormat && textArea.FrameFormat.Interior.Pattern != ExcelPattern.None)
      sfTextArea.Background = this.GetBrushFromDataFormat((IChartFillBorder) textArea.FrameFormat);
    return isVertical;
  }

  internal Border SfChartTitle(IChart xlsioChart, out RectangleF manualRect)
  {
    ChartTextAreaImpl chartTitleArea = xlsioChart.ChartTitleArea as ChartTextAreaImpl;
    ChartLayoutImpl layout = chartTitleArea.Layout as ChartLayoutImpl;
    Border brushForTextElements = this.GetBrushForTextElements(chartTitleArea.FrameFormat.Border as ChartBorderImpl);
    ChartImpl chartImpl = xlsioChart is ChartImpl ? xlsioChart as ChartImpl : (xlsioChart as ChartShapeImpl).ChartObject;
    if (brushForTextElements != null && chartImpl != null && chartImpl.Shapes.Count > 0)
    {
      if (xlsioChart.ChartType == ExcelChartType.Pie || xlsioChart.ChartType == ExcelChartType.PieOfPie || xlsioChart.ChartType == ExcelChartType.Pie_3D || xlsioChart.ChartType == ExcelChartType.Doughnut || xlsioChart.ChartType == ExcelChartType.Doughnut_Exploded || xlsioChart.ChartType == ExcelChartType.Pie_Bar || xlsioChart.ChartType == ExcelChartType.Pie_Exploded || xlsioChart.ChartType == ExcelChartType.Pie_Exploded_3D || (xlsioChart.Legend != null ? (xlsioChart.Legend.Position == ExcelLegendPosition.Top ? 1 : 0) : 0) != 0)
        brushForTextElements.Margin = new Thickness(8.0, 4.0, 8.0, 4.0);
      else
        brushForTextElements.Margin = new Thickness(8.0, 8.0, 8.0, 8.0);
    }
    manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if (layout.IsManualLayout)
    {
      ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
      if (manualLayout.FlagOptions != (byte) 0 && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
        manualRect = this.CalculateManualLayout(manualLayout, out bool _, false);
    }
    bool flag = (xlsioChart.Series.Count == 1 || xlsioChart.ChartType.ToString().Contains("Pie")) && !(xlsioChart.Series[0] as ChartSerieImpl).IsDefaultName;
    TextBlock textBlock = new TextBlock();
    textBlock.Margin = new Thickness(2.0);
    this.SetTransformAndBackGround(brushForTextElements, chartTitleArea, (ChartAxisImpl) null);
    if (chartTitleArea.RichText != null && chartTitleArea.RichText.FormattingRuns.Length > 1)
    {
      this.SetTextBlockInlines(chartTitleArea, textBlock);
    }
    else
    {
      this.SfTextBlock(textBlock, chartTitleArea);
      textBlock.Text = xlsioChart.ChartTitle != null ? (chartTitleArea == null || !chartTitleArea.IsFormula ? xlsioChart.ChartTitle : this.GetTextFromRange(xlsioChart.ChartTitle, chartTitleArea.StringCache)) : (flag ? (this.Names.ContainsKey((xlsioChart.Series[0] as ChartSerieImpl).Index) ? this.Names[(xlsioChart.Series[0] as ChartSerieImpl).Index] : this.GetSerieName(xlsioChart.Series[0] as ChartSerieImpl)) : "Chart Title");
    }
    textBlock.SetValue(TextBlock.TextWrappingProperty, (object) TextWrapping.Wrap);
    if (brushForTextElements != null)
      brushForTextElements.Child = (UIElement) textBlock;
    return brushForTextElements;
  }

  internal void SetTextBlockInlines(ChartTextAreaImpl textArea, TextBlock block)
  {
    ChartAlrunsRecord.TRuns[] formattingRuns = textArea.RichText.FormattingRuns;
    FontsCollection innerFonts = this.parentWorkbook.InnerFonts;
    string text1 = textArea.Text;
    block.TextAlignment = TextAlignment.Center;
    for (int index = 0; index < formattingRuns.Length; ++index)
    {
      string text2 = text1.Substring((int) formattingRuns[index].FirstCharIndex, (index != formattingRuns.Length - 1 ? (int) formattingRuns[index + 1].FirstCharIndex : text1.Length) - (int) formattingRuns[index].FirstCharIndex);
      int fontIndex = (int) formattingRuns[index].FontIndex;
      Run run = new Run(text2);
      if (fontIndex >= 0 && fontIndex < innerFonts.Count)
      {
        IFont font = innerFonts[fontIndex];
        this.SetFormattingsInRun(run, font);
      }
      block.Inlines.Add((Inline) run);
    }
  }

  internal void SetFormattingsInRun(Run run, IFont font)
  {
    if (font.Bold)
      run.FontWeight = FontWeights.Bold;
    if (font.Italic)
      run.FontStyle = FontStyles.Italic;
    if (font.Underline == ExcelUnderline.Single)
      run.TextDecorations.Add(new TextDecoration()
      {
        Location = TextDecorationLocation.Underline
      });
    if (font.Strikethrough)
      run.TextDecorations.Add(new TextDecoration()
      {
        Location = TextDecorationLocation.Strikethrough
      });
    if (font.Subscript)
      run.BaselineAlignment = BaselineAlignment.Subscript;
    else if (font.Superscript)
      run.BaselineAlignment = BaselineAlignment.Superscript;
    if (!font.IsAutoColor)
      run.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(font.RGBColor));
    run.FontSize = ApplicationImpl.ConvertUnitsStatic(font.Size, MeasureUnits.Point, MeasureUnits.Pixel);
    run.FontFamily = new System.Windows.Media.FontFamily(font.FontName);
  }

  internal string GetTextFromRange(string address, string[] cache)
  {
    IRange range = ChartParser.GetRange(this.parentWorkbook, address);
    string textFromRange = "";
    if (range != null && !(range is ExternalRange))
    {
      if (range.Cells.Length != 1)
      {
        bool flag = range.LastRow - range.Row > 1;
        int num1 = flag ? range.Row : range.Column;
        int num2 = flag ? range.LastRow : range.LastColumn;
        int num3 = flag ? range.Column : range.Row;
        for (int index = num1; index <= num2; ++index)
        {
          textFromRange += flag ? range[index, num3].DisplayText : range[num3, index].DisplayText;
          if (index != num2)
            textFromRange += " ";
        }
      }
      else
        textFromRange = range.DisplayText;
    }
    else if (cache != null && cache.Length > 0)
    {
      for (int index = 0; index < cache.Length; ++index)
      {
        textFromRange += cache[index];
        if (index != cache.Length - 1)
          textFromRange += " ";
      }
    }
    else
      textFromRange = " ";
    return textFromRange;
  }

  internal ChartAdornmentInfoBase SfChartDataLabel(
    ChartSerieImpl serie,
    ChartSeriesBase sfChartSeries)
  {
    ChartDataLabelsImpl label = (ChartDataLabelsImpl) null;
    if ((serie.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels)
      label = serie.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
    bool flag1 = sfChartSeries.ToString().Contains("Stacking");
    ObservableCollection<ChartPoint> itemsSource = sfChartSeries.ItemsSource as ObservableCollection<ChartPoint>;
    List<int> averageIndexes = (List<int>) null;
    bool isCircularSeries = sfChartSeries is CircularSeriesBase || sfChartSeries is CircularSeriesBase3D;
    bool flag2 = (serie.DataPoints as ChartDataPointsCollection).CheckDPDataLabels();
    if (label != null && !label.IsDelete && (label.IsCategoryName || label.IsSeriesName || label.IsValue || label.IsPercentage || label.IsValueFromCells) || serie.SerieFormat.IsMarkerSupported && (serie.SerieFormat as ChartSerieDataFormatImpl).MarkerFormat.MarkerType != ExcelChartMarkerType.None || flag2)
    {
      ChartAdornmentInfoBase info = !this.IsChart3D ? (ChartAdornmentInfoBase) new ChartAdornmentInfo() : (ChartAdornmentInfoBase) new ChartAdornmentInfo3D();
      info.SegmentLabelContent = LabelContent.LabelContentPath;
      if (label != null && !label.IsDelete)
      {
        info.LabelRotationAngle = (double) label.TextRotationAngle;
        if (label.IsCategoryName || label.IsSeriesName || label.IsValue || label.IsPercentage || label.IsValueFromCells)
          info.ShowLabel = true;
        this.SetLabelPosition(sfChartSeries, info, label);
      }
      if (!info.ShowLabel && flag2)
      {
        info.ShowLabel = true;
        info.LabelPosition = AdornmentsLabelPosition.Auto;
      }
      ChartSerieDataFormatImpl serieFormat = serie.SerieFormat as ChartSerieDataFormatImpl;
      if (sfChartSeries.ShowEmptyPoints && sfChartSeries.EmptyPointValue == EmptyPointValue.Average && itemsSource.Any<ChartPoint>((Func<ChartPoint, bool>) (x => x.IsSummary && x.Value.ToString() == double.NaN.ToString())))
      {
        averageIndexes = new List<int>(itemsSource.Count);
        for (int index = 0; index < itemsSource.Count; ++index)
        {
          if (itemsSource[index].IsSummary)
            averageIndexes.Add(index);
        }
      }
      if (sfChartSeries.ToString().Contains("Bubble") && !serie.SerieFormat.CommonSerieOptions.ShowNegativeBubbles)
      {
        if (averageIndexes == null)
          averageIndexes = new List<int>(itemsSource.Count);
        for (int index = 0; index < itemsSource.Count; ++index)
        {
          if (itemsSource[index].Size.Equals((object) 0))
            averageIndexes.Add(index);
        }
      }
      if (serieFormat.IsMarkerSupported)
        this.SetMarkerFormattings(serie, averageIndexes, serieFormat, sfChartSeries, info);
      if (label != null && !label.IsDelete || flag2)
      {
        bool flag3 = false;
        if ((label == null || label.IsDelete) && flag2)
        {
          label = serie.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
          flag3 = true;
        }
        DataTemplateInfo dataTemplateInfo = new DataTemplateInfo();
        System.Windows.Media.Brush borderBrush = (System.Windows.Media.Brush) null;
        Thickness borderThickness = new Thickness(0.0);
        System.Drawing.Color color = !label.IsAutoColor || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, serie.ParentChart, out color) || (int) color.R != (int) color.G || (int) color.B != (int) color.R || color.R != (byte) 0 ? label.RGBColor : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        dataTemplateInfo.SetValues(label.FontName, label.Size, label.Bold, label.Italic, label.Underline != ExcelUnderline.None, label.Strikethrough, this.SfColor(color));
        System.Windows.Media.Brush fillBrush;
        info.LabelTemplate = this.SfDataTemplate(dataTemplateInfo, label.FrameFormat as ChartFrameFormatImpl, out fillBrush, out borderBrush, out borderThickness, sfChartSeries.GetType().ToString());
        LabelConvertor labelConvertor = new LabelConvertor();
        labelConvertor.BlankIndexes = averageIndexes;
        labelConvertor.NumberFormatApplyEvent += new Func<object, string, string>(this.ApplyNumberFormat);
        ChartDataLabelsImpl dataLabelsImpl = label;
        DataLabelSetting dataLabelSetting1 = new DataLabelSetting(dataLabelsImpl, isCircularSeries);
        if (dataLabelsImpl.IsFormula)
          dataLabelSetting1.CustomText = this.GetTextFromRange(dataLabelSetting1.CustomText, dataLabelsImpl.StringCache);
        Dictionary<int, ChartDataPointImpl> dictionary = (serie.DataPoints as ChartDataPointsCollection).m_hashDataPoints.Where<KeyValuePair<int, ChartDataPointImpl>>((Func<KeyValuePair<int, ChartDataPointImpl>, bool>) (x => x.Value.HasDataLabels)).ToDictionary<KeyValuePair<int, ChartDataPointImpl>, int, ChartDataPointImpl>((Func<KeyValuePair<int, ChartDataPointImpl>, int>) (x => x.Key), (Func<KeyValuePair<int, ChartDataPointImpl>, ChartDataPointImpl>) (x => x.Value));
        labelConvertor.ValuesFromCells = serie.DataLabelCellsValues == null || serie.DataLabelCellsValues.Count <= 0 ? (Dictionary<int, object>) null : serie.DataLabelCellsValues;
        labelConvertor.SeriesName = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
        labelConvertor.CategoryNames = new object[itemsSource.Count];
        bool flag4 = sfChartSeries is CandleSeries || sfChartSeries is HiLoSeries;
        bool flag5 = itemsSource.Count <= 0 || (flag4 ? !(itemsSource[0].Size is string) : !(itemsSource[0].Close is string));
        for (int index = 0; index < itemsSource.Count; ++index)
          labelConvertor.CategoryNames[index] = !flag5 ? (!flag4 ? itemsSource[index].Close : itemsSource[index].Size) : itemsSource[index].X;
        if (isCircularSeries)
        {
          foreach (ChartPoint chartPoint in (Collection<ChartPoint>) itemsSource)
          {
            if (!double.IsNaN(chartPoint.Value))
              labelConvertor.Percentage += chartPoint.Value;
          }
        }
        if (label.IsSourceLinked && serie.Values != null && !(serie.Values is ExternalRange) && !(serie.Values is NameImpl))
        {
          dataLabelSetting1.NumberFormat = serie.Values.Cells[0].NumberFormat;
          dataLabelSetting1.IsSourceLinked = true;
        }
        else if (label.IsSourceLinked && serie.Values != null && serie.Values is NameImpl && (serie.Values as NameImpl).AddressLocal != null)
        {
          dataLabelSetting1.NumberFormat = serie.Values.Cells[0].NumberFormat;
          dataLabelSetting1.IsSourceLinked = true;
        }
        else
          dataLabelSetting1.NumberFormat = label.NumberFormat == null || !(label.NumberFormat.ToLower() != "General".ToLower()) ? "General" : label.NumberFormat;
        ColorConverter colorConverter1 = this.GetNumberFormatColorConverter(dataLabelSetting1.NumberFormat, label.RGBColor);
        if (dictionary.Count > 0)
        {
          FontConverter fontConverter1 = new FontConverter();
          fontConverter1.Names = new Dictionary<int, string>(dictionary.Count);
          fontConverter1.DefaultName = label.FontName;
          FontConverter fontConverter2 = new FontConverter();
          fontConverter2.Sizes = new Dictionary<int, double>(dictionary.Count);
          fontConverter2.DefaultSize = label.Size;
          FontConverter fontConverter3 = new FontConverter();
          fontConverter3.Bolds = new Dictionary<int, bool>(dictionary.Count);
          fontConverter3.DefaultBold = label.Bold;
          FontConverter fontConverter4 = new FontConverter();
          fontConverter4.Italics = new Dictionary<int, bool>(dictionary.Count);
          fontConverter4.DefaultItalic = label.Italic;
          FontConverter fontConverter5 = new FontConverter();
          fontConverter5.Underlines = new Dictionary<int, bool>(dictionary.Count);
          fontConverter5.DefaultUnderline = label.Underline != ExcelUnderline.None;
          FontConverter fontConverter6 = new FontConverter();
          fontConverter6.Strikethroughs = new Dictionary<int, bool>(dictionary.Count);
          fontConverter6.DefaultStrikethrough = label.Strikethrough;
          ThicknessConverter thicknessConverter = new ThicknessConverter();
          thicknessConverter.LabelThicknesses = new Dictionary<int, Thickness>(dictionary.Count);
          ChartFrameFormatImpl frameFormat1 = label.FrameFormat as ChartFrameFormatImpl;
          thicknessConverter.DefaultThickness = this.GetBrushForTextElements(frameFormat1.Border as ChartBorderImpl).BorderThickness;
          ColorConverter colorConverter2 = new ColorConverter();
          colorConverter2.LabelBrushes = new Dictionary<int, System.Windows.Media.Brush>(dictionary.Count);
          colorConverter2.DefaultBrush = this.GetBrushForTextElements(frameFormat1.Border as ChartBorderImpl).BorderBrush;
          ColorConverter colorConverter3 = new ColorConverter();
          colorConverter3.LabelBrushes = new Dictionary<int, System.Windows.Media.Brush>(dictionary.Count);
          if (!frameFormat1.IsAutomaticFormat && frameFormat1.Interior.Pattern != ExcelPattern.None)
            colorConverter3.DefaultBrush = this.GetBrushFromDataFormat((IChartFillBorder) frameFormat1);
          colorConverter1 = colorConverter1 ?? new ColorConverter();
          colorConverter1.LabelBrushes = new Dictionary<int, System.Windows.Media.Brush>(dictionary.Count);
          colorConverter1.DefaultBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(label.RGBColor));
          foreach (KeyValuePair<int, ChartDataPointImpl> keyValuePair in dictionary)
          {
            if (keyValuePair.Key < itemsSource.Count)
            {
              ChartDataLabelsImpl dataLabels = keyValuePair.Value.DataLabels as ChartDataLabelsImpl;
              ChartFrameFormatImpl frameFormat2 = keyValuePair.Value.DataLabels.FrameFormat as ChartFrameFormatImpl;
              Border brushForTextElements = this.GetBrushForTextElements(frameFormat2.Border as ChartBorderImpl);
              if (dataLabels.IsDelete)
              {
                colorConverter3.LabelBrushes.Add(keyValuePair.Value.Index, (System.Windows.Media.Brush) null);
                colorConverter2.LabelBrushes.Add(keyValuePair.Value.Index, (System.Windows.Media.Brush) null);
                thicknessConverter.LabelThicknesses.Add(keyValuePair.Value.Index, new Thickness(0.0));
              }
              else if (!frameFormat2.IsAutomaticFormat)
              {
                colorConverter3.LabelBrushes.Add(keyValuePair.Value.Index, frameFormat2.Interior.Pattern != ExcelPattern.None ? this.GetBrushFromDataFormat((IChartFillBorder) frameFormat2) : (System.Windows.Media.Brush) null);
                colorConverter2.LabelBrushes.Add(keyValuePair.Value.Index, brushForTextElements.BorderBrush);
                thicknessConverter.LabelThicknesses.Add(keyValuePair.Value.Index, brushForTextElements.BorderThickness);
              }
              else
              {
                colorConverter3.LabelBrushes.Add(keyValuePair.Value.Index, fillBrush);
                colorConverter2.LabelBrushes.Add(keyValuePair.Value.Index, borderBrush);
                thicknessConverter.LabelThicknesses.Add(keyValuePair.Value.Index, borderThickness);
              }
              if (dataLabels.ShowTextProperties)
              {
                colorConverter1.LabelBrushes.Add(keyValuePair.Value.Index, (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(dataLabels.TextArea.RGBColor)));
                fontConverter1.Names.Add(keyValuePair.Value.Index, dataLabels.FontName);
                fontConverter2.Sizes.Add(keyValuePair.Value.Index, dataLabels.Size);
                fontConverter3.Bolds.Add(keyValuePair.Value.Index, dataLabels.Bold);
                fontConverter4.Italics.Add(keyValuePair.Value.Index, dataLabels.Italic);
                fontConverter6.Strikethroughs.Add(keyValuePair.Value.Index, dataLabels.Strikethrough);
                fontConverter5.Underlines.Add(keyValuePair.Value.Index, dataLabels.Underline != ExcelUnderline.None);
              }
              else if (dataLabels.IsDelete)
              {
                colorConverter1.LabelBrushes.Add(keyValuePair.Value.Index, (System.Windows.Media.Brush) null);
              }
              else
              {
                colorConverter1.LabelBrushes.Add(keyValuePair.Value.Index, (System.Windows.Media.Brush) new SolidColorBrush(dataTemplateInfo.Color));
                fontConverter1.Names.Add(keyValuePair.Value.Index, dataTemplateInfo.FontName);
                fontConverter2.Sizes.Add(keyValuePair.Value.Index, dataTemplateInfo.Size);
                fontConverter3.Bolds.Add(keyValuePair.Value.Index, dataTemplateInfo.Bold);
                fontConverter4.Italics.Add(keyValuePair.Value.Index, dataTemplateInfo.Italic);
                fontConverter6.Strikethroughs.Add(keyValuePair.Value.Index, dataTemplateInfo.Strikethrough);
                fontConverter5.Underlines.Add(keyValuePair.Value.Index, dataTemplateInfo.Underline);
              }
              DataLabelSetting dataLabelSetting2 = new DataLabelSetting(dataLabels, isCircularSeries);
              if (dataLabels.IsFormula)
                dataLabelSetting2.CustomText = this.GetTextFromRange(dataLabelSetting2.CustomText, dataLabels.StringCache);
              dataLabelSetting2.NumberFormat = !dataLabelSetting2.IsSourceLinked || serie.Values == null || serie.Values is ExternalRange || dataLabels.NumberFormat == null ? dataLabelSetting1.NumberFormat : serie.Values.Cells[keyValuePair.Value.Index].NumberFormat;
              labelConvertor.DataLabelSettings.Add(keyValuePair.Value.Index, dataLabelSetting2);
            }
          }
          Binding binding1 = new Binding()
          {
            Converter = (IValueConverter) thicknessConverter
          };
          info.LabelTemplate.VisualTree.SetBinding(Border.BorderThicknessProperty, (BindingBase) binding1);
          Binding binding2 = new Binding()
          {
            Converter = (IValueConverter) colorConverter2
          };
          info.LabelTemplate.VisualTree.SetBinding(Border.BorderBrushProperty, (BindingBase) binding2);
          Binding binding3 = new Binding()
          {
            Converter = (IValueConverter) fontConverter1
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(FrameworkElement.NameProperty, (BindingBase) binding3);
          Binding binding4 = new Binding()
          {
            Converter = (IValueConverter) fontConverter2
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.FontSizeProperty, (BindingBase) binding4);
          Binding binding5 = new Binding()
          {
            Converter = (IValueConverter) fontConverter3
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.FontWeightProperty, (BindingBase) binding5);
          Binding binding6 = new Binding()
          {
            Converter = (IValueConverter) fontConverter4
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.FontStyleProperty, (BindingBase) binding6);
          Binding binding7 = new Binding()
          {
            Converter = (IValueConverter) fontConverter5
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.TextDecorationsProperty, (BindingBase) binding7);
          Binding binding8 = new Binding()
          {
            Converter = (IValueConverter) fontConverter6
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.TextDecorationsProperty, (BindingBase) binding8);
          Binding binding9 = new Binding()
          {
            Converter = (IValueConverter) colorConverter3
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.BackgroundProperty, (BindingBase) binding9);
        }
        if (colorConverter1 != null)
        {
          Binding binding = new Binding()
          {
            Converter = (IValueConverter) colorConverter1
          };
          info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.ForegroundProperty, (BindingBase) binding);
        }
        labelConvertor.CommonDataLabelSetting = dataLabelSetting1;
        Binding binding10 = new Binding()
        {
          Converter = (IValueConverter) labelConvertor
        };
        info.LabelTemplate.VisualTree.FirstChild.SetBinding(TextBlock.TextProperty, (BindingBase) binding10);
        if (sfChartSeries is RangeColumnSeries)
          info.LabelTemplate.VisualTree.SetBinding(FrameworkElement.MarginProperty, (BindingBase) new Binding()
          {
            Converter = (IValueConverter) new FunnelDataLabelPositionConverter()
            {
              LabelConverterObject = labelConvertor
            }
          });
        if (flag3)
          (serie.DataPoints.DefaultDataPoint as ChartDataPointImpl).DataLabels = (IChartDataLabels) null;
      }
      return info;
    }
    if (!flag1)
      return (ChartAdornmentInfoBase) null;
    ChartAdornmentInfoBase adornmentInfoBase = !this.IsChart3D ? (ChartAdornmentInfoBase) new ChartAdornmentInfo() : (ChartAdornmentInfoBase) new ChartAdornmentInfo3D();
    adornmentInfoBase.ShowMarker = false;
    adornmentInfoBase.ShowLabel = false;
    adornmentInfoBase.ShowConnectorLine = false;
    return adornmentInfoBase;
  }

  private void SetMarkerFormattings(
    ChartSerieImpl chartSerieImpl,
    List<int> averageIndexes,
    ChartSerieDataFormatImpl dataFormat,
    ChartSeriesBase sfChartSerie,
    ChartAdornmentInfoBase info)
  {
    IEnumerable<\u003C\u003Ef__AnonymousType6<int, ChartSerieDataFormatImpl>> source = (chartSerieImpl.DataPoints as ChartDataPointsCollection).m_hashDataPoints.Select(point => new
    {
      point = point,
      x = point.Value.DataFormatOrNull
    }).Where(_param0 => _param0.x != null && _param0.x.MarkerFormatOrNull != null && !_param0.x.IsAutoMarker).Select(_param0 => new
    {
      Key = _param0.point.Key,
      Value = _param0.x
    });
    if (dataFormat.MarkerFormatOrNull != null && !dataFormat.IsAutoMarker && (dataFormat.MarkerFormat.MarkerType == ExcelChartMarkerType.None ? (source.Count() == 0 ? 1 : 0) : 0) != 0)
      return;
    bool isvaryColor = !this.IsChartEx && this.IsVaryColorSupported(chartSerieImpl);
    ObservableCollection<ChartPoint> itemsSource = sfChartSerie.ItemsSource as ObservableCollection<ChartPoint>;
    bool flag = itemsSource.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.SegmentBrush == null)) == itemsSource.Count;
    int number = chartSerieImpl.Number;
    int count1 = dataFormat.ParentChart.Series.Count;
    if (dataFormat.MarkerFormatOrNull == null)
      return;
    info.ShowMarker = true;
    MarkerSetting markerSettings1 = this.GetMarkerSettings(chartSerieImpl, dataFormat, (MarkerSetting) null, number, count1, false);
    if (sfChartSerie is ScatterSeries || sfChartSerie is FastScatterBitmapSeries)
      sfChartSerie.Interior = markerSettings1.FillBrush;
    if (dataFormat.MarkerFormat.MarkerType != ExcelChartMarkerType.None && markerSettings1.FillBrush != null && (markerSettings1.FillBrush as SolidColorBrush).Color.ToString() == "#00FFFFFF" && (chartSerieImpl.SerieType == ExcelChartType.Scatter_Markers || markerSettings1.BorderBrush != null && (markerSettings1.BorderBrush as SolidColorBrush).Color.ToString() == "#FF000000"))
      sfChartSerie.Interior = markerSettings1.BorderBrush;
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (System.Windows.Shapes.Path));
    if (source.Count() != 0 || isvaryColor)
    {
      MarkerConverter markerConverter = new MarkerConverter();
      markerConverter.CommonMarkerSetting = markerSettings1;
      markerConverter.MarkerSettings = new Dictionary<int, MarkerSetting>(source.Count());
      int count2 = (sfChartSerie.ItemsSource as ObservableCollection<ChartPoint>).Count;
      for (int i = 0; i < count2; ++i)
      {
        var data = source.FirstOrDefault(x => x.Key == i);
        if (isvaryColor || data != null)
        {
          MarkerSetting markerSettings2 = this.GetMarkerSettings(chartSerieImpl, data != null ? data.Value : dataFormat, markerSettings1, isvaryColor ? i : number, isvaryColor ? count2 : count1, isvaryColor);
          markerConverter.MarkerSettings.Add(data != null ? data.Key : i, markerSettings2);
          if (isvaryColor && flag && i < itemsSource.Count && itemsSource[i].SegmentBrush == null)
            itemsSource[i].SegmentBrush = markerSettings2.FillBrush;
        }
      }
      Binding binding1 = new Binding()
      {
        Converter = (IValueConverter) markerConverter
      };
      frameworkElementFactory.SetBinding(System.Windows.Shapes.Path.DataProperty, (BindingBase) binding1);
      Binding binding2 = new Binding()
      {
        Converter = (IValueConverter) markerConverter
      };
      frameworkElementFactory.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding2);
      Binding binding3 = new Binding()
      {
        Converter = (IValueConverter) markerConverter,
        ConverterParameter = (object) true
      };
      frameworkElementFactory.SetBinding(Shape.FillProperty, (BindingBase) binding3);
      Binding binding4 = new Binding()
      {
        Converter = (IValueConverter) markerConverter,
        ConverterParameter = (object) false
      };
      frameworkElementFactory.SetBinding(Shape.StrokeProperty, (BindingBase) binding4);
      markerConverter.AverageMarkerIndexes = averageIndexes;
    }
    else if (averageIndexes != null)
    {
      MarkerConverter markerConverter = new MarkerConverter();
      markerConverter.CommonMarkerSetting = markerSettings1;
      markerConverter.MarkerSettings = new Dictionary<int, MarkerSetting>(1);
      markerConverter.AverageMarkerIndexes = averageIndexes;
      Binding binding5 = new Binding()
      {
        Converter = (IValueConverter) markerConverter
      };
      frameworkElementFactory.SetBinding(System.Windows.Shapes.Path.DataProperty, (BindingBase) binding5);
      Binding binding6 = new Binding()
      {
        Converter = (IValueConverter) markerConverter
      };
      frameworkElementFactory.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding6);
      Binding binding7 = new Binding()
      {
        Converter = (IValueConverter) markerConverter,
        ConverterParameter = (object) true
      };
      frameworkElementFactory.SetBinding(Shape.FillProperty, (BindingBase) binding7);
      Binding binding8 = new Binding()
      {
        Converter = (IValueConverter) markerConverter,
        ConverterParameter = (object) false
      };
      frameworkElementFactory.SetBinding(Shape.StrokeProperty, (BindingBase) binding8);
    }
    else
    {
      frameworkElementFactory.SetValue(System.Windows.Shapes.Path.DataProperty, (object) ChartCommon.GetMarkerSymbolGeometry(markerSettings1.MarkerTypeInInt, markerSettings1.MarkerSize));
      frameworkElementFactory.SetValue(Shape.StrokeProperty, (object) markerSettings1.BorderBrush);
      frameworkElementFactory.SetValue(Shape.StrokeThicknessProperty, (object) markerSettings1.BorderThickness);
      frameworkElementFactory.SetValue(Shape.FillProperty, (object) markerSettings1.FillBrush);
    }
    ChartAdornmentInfoBase adornmentInfoBase = info;
    DataTemplate dataTemplate1 = new DataTemplate((object) typeof (System.Windows.Shapes.Path));
    dataTemplate1.VisualTree = frameworkElementFactory;
    DataTemplate dataTemplate2 = dataTemplate1;
    adornmentInfoBase.SymbolTemplate = dataTemplate2;
  }

  private MarkerSetting GetMarkerSettings(
    ChartSerieImpl chartSerieImpl,
    ChartSerieDataFormatImpl dataFormat,
    MarkerSetting parentMarkerSttings,
    int index,
    int count,
    bool isvaryColor)
  {
    MarkerSetting markerSettings = new MarkerSetting();
    ChartBorderImpl lineProperties = chartSerieImpl.SerieFormat.LineProperties as ChartBorderImpl;
    short? nullable = new short?();
    if (lineProperties.LineWeightString != null)
      nullable = new short?((short) lineProperties.LineWeight);
    double thickness;
    markerSettings.MarkerSize = !dataFormat.IsAutoMarker || !this.TryAndGetThicknessBasedOnElement(ChartElementsEnum.MarkerThickness, chartSerieImpl.ParentChart, out thickness, nullable) ? (double) dataFormat.MarkerSize : thickness;
    int num = (int) dataFormat.MarkerStyle;
    if (dataFormat.MarkerFormatOrNull == null || dataFormat.IsAutoMarker)
      num = this.m_def_MarkerType_Array[(isvaryColor ? index : chartSerieImpl.Index) % this.m_def_MarkerType_Array.Length];
    else if (dataFormat.MarkerStyle == ExcelChartMarkerType.None)
      num = -1;
    if (num == -1)
    {
      markerSettings.FillBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      markerSettings.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      return markerSettings;
    }
    markerSettings.MarkerTypeInInt = num;
    bool isBinary = this.parentWorkbook.Version == ExcelVersion.Excel97to2003;
    System.Drawing.Color color;
    System.Windows.Media.Brush brush = !this.TryAndGetFillOrLineColorBasedOnPattern(chartSerieImpl.ParentChart, false, index, count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(dataFormat.ParentChart.GetChartColor(index, count, isBinary, true))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    if (!dataFormat.IsAutoMarker)
    {
      markerSettings.FillBrush = dataFormat.MarkerFormat.IsNotShowInt ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0)) : (isBinary || ((int) dataFormat.MarkerFormat.FlagOptions & 1) != 0 && (int) dataFormat.MarkerFormat.FillColorIndex == (int) (ushort) dataFormat.MarkerBackColorObject.GetIndexed((IWorkbook) this.parentWorkbook) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(dataFormat.MarkerBackgroundColor)) : brush);
      if (!dataFormat.MarkerFormat.IsNotShowBrd)
        markerSettings.BorderBrush = isBinary || ((int) dataFormat.MarkerFormat.FlagOptions & 2) != 0 && dataFormat.MarkerFormat.HasLineProperties ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(dataFormat.MarkerForegroundColor)) : brush;
    }
    else if (isvaryColor)
    {
      markerSettings.BorderBrush = brush;
      markerSettings.FillBrush = brush;
    }
    else if (parentMarkerSttings != null)
    {
      markerSettings.BorderBrush = parentMarkerSttings.BorderBrush;
      markerSettings.FillBrush = parentMarkerSttings.FillBrush;
    }
    else if (num == 4 || num == 5 || num == 9)
    {
      markerSettings.BorderBrush = brush;
      markerSettings.FillBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    }
    else
    {
      markerSettings.FillBrush = brush;
      markerSettings.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    }
    markerSettings.BorderThickness = dataFormat.MarkerLineWidth;
    return markerSettings;
  }

  internal bool IsVaryColorSupported(ChartSerieImpl serie)
  {
    return serie.GetCommonSerieFormat().IsVaryColor && serie.ParentChart.Series.Count == 1;
  }

  private void SetLabelPosition(
    ChartSeriesBase sfChartSeries,
    ChartAdornmentInfoBase info,
    ChartDataLabelsImpl label)
  {
    bool flag = true;
    Type type = sfChartSeries.GetType();
    if (sfChartSeries is RangeColumnSeries)
    {
      info.AdornmentsPosition = AdornmentsPosition.Top;
      info.VerticalAlignment = VerticalAlignment.Center;
      info.SegmentLabelContent = LabelContent.LabelContentPath;
      info.HorizontalAlignment = HorizontalAlignment.Center;
    }
    else
    {
      if (type == typeof (AreaSeries) || type == typeof (StackingAreaSeries) || type == typeof (StackingArea100Series) || type == typeof (RadarSeries) || type == typeof (DoughnutSeries))
        flag = false;
      CircularSeriesBase circularSeriesBase = sfChartSeries as CircularSeriesBase;
      CircularSeriesBase3D circularSeriesBase3D = sfChartSeries as CircularSeriesBase3D;
      info.HorizontalAlignment = HorizontalAlignment.Center;
      if (label.Position != ExcelDataLabelPosition.Automatic && flag)
      {
        switch (label.Position)
        {
          case ExcelDataLabelPosition.Outside:
            if (circularSeriesBase != null)
            {
              circularSeriesBase.EnableSmartLabels = false;
              circularSeriesBase.LabelPosition = CircularSeriesLabelPosition.OutsideExtended;
            }
            else if (circularSeriesBase3D != null)
            {
              circularSeriesBase3D.EnableSmartLabels = false;
              circularSeriesBase3D.LabelPosition = CircularSeriesLabelPosition.OutsideExtended;
            }
            info.LabelPosition = AdornmentsLabelPosition.Outer;
            break;
          case ExcelDataLabelPosition.Inside:
          case ExcelDataLabelPosition.BestFit:
            if (circularSeriesBase != null)
            {
              circularSeriesBase.EnableSmartLabels = false;
              circularSeriesBase.LabelPosition = CircularSeriesLabelPosition.Outside;
            }
            else if (circularSeriesBase3D != null)
            {
              circularSeriesBase3D.EnableSmartLabels = false;
              circularSeriesBase3D.LabelPosition = CircularSeriesLabelPosition.Outside;
            }
            info.LabelPosition = AdornmentsLabelPosition.Inner;
            break;
          case ExcelDataLabelPosition.OutsideBase:
            if (circularSeriesBase != null)
              circularSeriesBase.EnableSmartLabels = false;
            else if (circularSeriesBase3D != null)
              circularSeriesBase3D.EnableSmartLabels = false;
            else
              info.LabelPosition = AdornmentsLabelPosition.Inner;
            info.AdornmentsPosition = AdornmentsPosition.Bottom;
            break;
          case ExcelDataLabelPosition.Above:
            if (type == typeof (BubbleSeries))
            {
              info.LabelPosition = AdornmentsLabelPosition.Outer;
              break;
            }
            info.AdornmentsPosition = AdornmentsPosition.Top;
            info.VerticalAlignment = VerticalAlignment.Top;
            break;
          case ExcelDataLabelPosition.Below:
            info.AdornmentsPosition = AdornmentsPosition.Bottom;
            info.VerticalAlignment = VerticalAlignment.Bottom;
            break;
          case ExcelDataLabelPosition.Left:
            info.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            info.HorizontalAlignment = HorizontalAlignment.Left;
            break;
          case ExcelDataLabelPosition.Right:
            info.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            info.HorizontalAlignment = HorizontalAlignment.Right;
            break;
          default:
            if (circularSeriesBase != null)
            {
              circularSeriesBase.EnableSmartLabels = false;
              info.LabelPosition = AdornmentsLabelPosition.Center;
              circularSeriesBase.LabelPosition = CircularSeriesLabelPosition.Inside;
              break;
            }
            if (circularSeriesBase3D != null)
            {
              circularSeriesBase3D.EnableSmartLabels = false;
              info.LabelPosition = AdornmentsLabelPosition.Center;
              circularSeriesBase3D.LabelPosition = CircularSeriesLabelPosition.Inside;
              break;
            }
            info.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            break;
        }
      }
      else if (type == typeof (PieSeries))
      {
        circularSeriesBase.EnableSmartLabels = false;
        circularSeriesBase.LabelPosition = CircularSeriesLabelPosition.Outside;
        info.LabelPosition = AdornmentsLabelPosition.Inner;
      }
      else if (type == typeof (PieSeries3D))
      {
        circularSeriesBase3D.EnableSmartLabels = false;
        circularSeriesBase3D.LabelPosition = CircularSeriesLabelPosition.Outside;
        info.LabelPosition = AdornmentsLabelPosition.Inner;
      }
      else if (type == typeof (AreaSeries) || type == typeof (StackingAreaSeries) || type == typeof (StackingArea100Series))
      {
        info.AdornmentsPosition = AdornmentsPosition.Bottom;
        info.VerticalAlignment = VerticalAlignment.Bottom;
        info.HorizontalAlignment = HorizontalAlignment.Center;
      }
      else if (type == typeof (RadarSeries) || type == typeof (BarSeries) || type == typeof (ColumnSeries) || type == typeof (BubbleSeries) || type == typeof (ColumnSeries3D) || type == typeof (BarSeries3D))
        info.LabelPosition = AdornmentsLabelPosition.Outer;
      else if (type == typeof (ScatterSeries) || type == typeof (SplineSeries) || type == typeof (LineSeries) || type == typeof (CandleSeries) || type == typeof (HiLoSeries))
      {
        info.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
        info.HorizontalAlignment = HorizontalAlignment.Right;
      }
      else
        info.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
    }
  }

  internal static Geometry GetMarkerSymbolGeometry(int markerTypeInInt, double markerSize)
  {
    double num1 = Math.Round(markerSize < 2.0 ? 1.0 : markerSize / 2.8, 1);
    Geometry markerSymbolGeometry;
    switch (markerTypeInInt)
    {
      case 1:
        double num2 = 4.0;
        markerSymbolGeometry = (Geometry) new RectangleGeometry()
        {
          Rect = new Rect(0.0, 0.0, num2 * num1, num2 * num1)
        };
        break;
      case 2:
        double num3 = 2.0;
        markerSymbolGeometry = (Geometry) new PathGeometry()
        {
          Figures = {
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(num3 * num1, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(num3 * num1 * 2.0, num3 * num1)
                },
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(num3 * num1, num3 * num1 * 2.0)
                },
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(0.0, num3 * num1)
                },
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(num3 * num1, 0.0)
                }
              }
            }
          }
        };
        break;
      case 3:
        double num4 = 2.0;
        double num5 = 4.0;
        markerSymbolGeometry = (Geometry) new PathGeometry()
        {
          Figures = {
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(num4 * num1, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(0.0, num5 * num1)
                },
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(num4 * 2.0 * num1, num5 * num1)
                },
                (PathSegment) new System.Windows.Media.LineSegment()
                {
                  Point = new System.Windows.Point(num4 * num1, 0.0)
                }
              }
            }
          }
        };
        break;
      case 4:
        double num6 = 4.0;
        double num7 = 4.0;
        markerSymbolGeometry = (Geometry) new PathGeometry()
        {
          Figures = {
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num6 * num1, num7 * num1), true),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num6 * num1, 0.0), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, num7 * num1), true)
              }
            },
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num6 * num1, 0.0), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, num7 * num1), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num6 * num1, num7 * num1), false)
              }
            }
          }
        };
        break;
      case 5:
        double num8 = 4.0;
        double num9 = 2.0;
        markerSymbolGeometry = (Geometry) new PathGeometry()
        {
          Figures = {
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num9 * num1 * 2.0, num8 * num1), true),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num9 * num1 * 2.0, 0.0), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, num8 * num1), true)
              }
            },
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num9 * num1 * 2.0, 0.0), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, num8 * num1), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num9 * num1 * 2.0, num8 * num1), false)
              }
            },
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(num9 * num1, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num9 * num1, num8 * num1), true)
              }
            }
          }
        };
        break;
      case 6:
        double num10 = 2.0;
        double num11 = 1.5;
        markerSymbolGeometry = (Geometry) new RectangleGeometry()
        {
          Rect = new Rect(0.0, 0.0, num10 * num1, num11 * (num1 / 2.0))
        };
        break;
      case 7:
        double num12 = 4.0;
        double num13 = 1.5;
        markerSymbolGeometry = (Geometry) new RectangleGeometry()
        {
          Rect = new Rect(0.0, 0.0, num12 * num1, num13 * (num1 / 2.0))
        };
        break;
      case 9:
        double num14 = 2.0;
        double num15 = 4.0;
        markerSymbolGeometry = (Geometry) new PathGeometry()
        {
          Figures = {
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num14 * num1 * 2.0, 0.0), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num14 * num1 * 2.0, num15 * num1), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, num15 * num1), false),
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(0.0, 0.0), false)
              }
            },
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(num14 * num1, 0.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num14 * num1, num15 * num1), true)
              }
            },
            new PathFigure()
            {
              StartPoint = new System.Windows.Point(0.0, num15 * num1 / 2.0),
              Segments = {
                (PathSegment) new System.Windows.Media.LineSegment(new System.Windows.Point(num14 * num1 * 2.0, num15 * num1 / 2.0), true)
              }
            }
          }
        };
        break;
      default:
        double num16 = 2.0;
        markerSymbolGeometry = (Geometry) new EllipseGeometry()
        {
          Center = new System.Windows.Point(num16 * num1, num16 * num1),
          RadiusX = (num16 * num1),
          RadiusY = (num16 * num1)
        };
        break;
    }
    return markerSymbolGeometry;
  }

  private ColorConverter GetNumberFormatColorConverter(string numberFormat, System.Drawing.Color defaultColor)
  {
    ColorConverter formatColorConverter = (ColorConverter) null;
    Dictionary<char, System.Drawing.Color> fromNumberFormat = RangeImpl.GetColorsFromNumberFormat(numberFormat, defaultColor);
    if (fromNumberFormat != null && fromNumberFormat.Count > 0)
    {
      formatColorConverter = new ColorConverter();
      formatColorConverter.Brushes = new Dictionary<char, System.Windows.Media.Brush>(fromNumberFormat.Count);
      foreach (char key in fromNumberFormat.Keys)
        formatColorConverter.Brushes[key] = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(fromNumberFormat[key]));
    }
    return formatColorConverter;
  }

  internal System.Windows.Media.Color SfColor(byte R, byte G, byte B, double transparency)
  {
    return System.Windows.Media.Color.FromArgb((byte) ((double) byte.MaxValue - transparency * (double) byte.MaxValue), R, G, B);
  }

  internal System.Windows.Media.Color SfColor(byte R, byte G, byte B)
  {
    return System.Windows.Media.Color.FromArgb(byte.MaxValue, R, G, B);
  }

  internal System.Windows.Media.Color SfColor(System.Drawing.Color chartcolor)
  {
    return this.SfColor(chartcolor, 0.0);
  }

  internal System.Windows.Media.Color SfColor(System.Drawing.Color chartcolor, double transparency)
  {
    return System.Windows.Media.Color.FromArgb((byte) ((double) byte.MaxValue - transparency * (double) byte.MaxValue), chartcolor.R, chartcolor.G, chartcolor.B);
  }

  internal void SfPloatArea(SfChartExt sfChart, ChartFrameFormatImpl chartArea, ChartImpl chart)
  {
    string str = chart.ChartType.ToString();
    if (!chartArea.IsAutomaticFormat)
    {
      sfChart.AreaBackground = this.GetBrushFromDataFormat((IChartFillBorder) chartArea);
    }
    else
    {
      double transparency = 1.0;
      if (!str.Contains("Pie") && !str.Contains("Doughnut"))
        transparency = 0.0;
      System.Drawing.Color color;
      if (this.TryAndGetColorBasedOnElement(ChartElementsEnum.PlotAreaFill, chart, out color))
        sfChart.AreaBackground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color, transparency));
      else
        sfChart.AreaBackground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, transparency));
    }
    ChartBorderImpl border = chartArea.Border as ChartBorderImpl;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      double uniformLength = 0.0;
      if (border.HasLineProperties)
        uniformLength = this.GetBorderThickness(border);
      if (!border.IsAutoLineColor || border.HasGradientFill)
        sfChart.AreaBorderBrush = this.GetBrushFromBorder(border);
      else if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
        sfChart.AreaBorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
      else
        sfChart.AreaBorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      if (uniformLength < 1.0)
        uniformLength = 1.0;
      sfChart.AreaBorderThickness = new Thickness(uniformLength);
    }
    else
    {
      sfChart.AreaBorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      sfChart.AreaBorderThickness = new Thickness(0.0);
    }
  }

  internal void SfChartArea(SfChartExt sfChart, ChartFrameFormatImpl chartArea, ChartImpl chart)
  {
    System.Drawing.Color color;
    if (!chartArea.IsAutomaticFormat)
      sfChart.Background = this.GetBrushFromDataFormat((IChartFillBorder) chartArea);
    else if (this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, chart, out color))
      sfChart.Background = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    else
      sfChart.Background = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    ChartBorderImpl border = chartArea.Border as ChartBorderImpl;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      double uniformLength = 0.0;
      if (border.HasLineProperties)
        uniformLength = this.GetBorderThickness(border);
      if (!border.IsAutoLineColor || border.HasGradientFill)
      {
        sfChart.BorderBrush = this.GetBrushFromBorder(border);
        if (uniformLength < 1.0)
          uniformLength = 1.0;
      }
      else
      {
        if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
        else if (this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaLine, chart, out color))
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        else
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 134, (byte) 134, (byte) 134));
        if (uniformLength < 1.0)
          uniformLength = 1.0;
      }
      sfChart.BorderThickness = new Thickness(uniformLength);
    }
    else
    {
      sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      sfChart.BorderThickness = new Thickness(0.0);
    }
  }

  internal void SfChartArea3D(
    SfChart3DExt sfChart,
    ChartFrameFormatImpl chartArea,
    ChartImpl chart)
  {
    System.Windows.Media.Brush brush = (System.Windows.Media.Brush) null;
    System.Drawing.Color color;
    if (!chartArea.IsAutomaticFormat)
      brush = this.GetBrushFromDataFormat((IChartFillBorder) chartArea);
    else if (this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaFill, chart, out color))
      sfChart.Background = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    else
      brush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    sfChart.Background = brush;
    ChartBorderImpl border = chartArea.Border as ChartBorderImpl;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      double uniformLength = 0.0;
      if (border.HasLineProperties)
        uniformLength = this.GetBorderThickness(border);
      if (!border.IsAutoLineColor || border.HasGradientFill)
      {
        sfChart.BorderBrush = this.GetBrushFromBorder(border);
        if (uniformLength < 1.0)
          uniformLength = 1.0;
      }
      else
      {
        if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
        else if (this.TryAndGetColorBasedOnElement(ChartElementsEnum.ChartAreaLine, chart, out color))
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        else
          sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 134, (byte) 134, (byte) 134));
        if (uniformLength < 1.0)
          uniformLength = 1.0;
      }
      sfChart.BorderThickness = new Thickness(uniformLength);
    }
    else
    {
      sfChart.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
      sfChart.BorderThickness = new Thickness(0.0);
    }
  }

  internal bool IsLine(ExcelChartType chartType)
  {
    switch (chartType)
    {
      case ExcelChartType.Line:
      case ExcelChartType.Line_Stacked:
      case ExcelChartType.Line_Stacked_100:
      case ExcelChartType.Line_Markers:
      case ExcelChartType.Line_Markers_Stacked:
      case ExcelChartType.Line_Markers_Stacked_100:
      case ExcelChartType.Line_3D:
        return true;
      default:
        return false;
    }
  }

  internal void SfChartTrendLine(ChartSerieImpl serieImpl, CartesianSeries sfSerie)
  {
    if (serieImpl.TrendLines.Count <= 0)
      return;
    for (int iIndex = 0; iIndex < serieImpl.TrendLines.Count; ++iIndex)
    {
      ChartTrendLineImpl trendLine = serieImpl.TrendLines[iIndex] as ChartTrendLineImpl;
      Trendline sfTrendline = new Trendline();
      if (trendLine.Type == ExcelTrendLineType.Moving_Average)
        sfTrendline.Type = TrendlineType.Linear;
      else
        sfTrendline.Type = (TrendlineType) Enum.Parse(typeof (TrendlineType), trendLine.Type.ToString());
      if (trendLine.LegendEntry != null && trendLine.LegendEntry.IsDeleted)
        sfTrendline.VisibilityOnLegend = Visibility.Collapsed;
      sfTrendline.Label = trendLine.Name;
      if (trendLine.Type == ExcelTrendLineType.Polynomial && trendLine.Order >= 2 && trendLine.Order <= 6)
        sfTrendline.PolynomialOrder = trendLine.Order;
      else if (trendLine.Type != ExcelTrendLineType.Polynomial && trendLine.Order < 3)
        sfTrendline.PolynomialOrder = 3;
      else if (trendLine.Type != ExcelTrendLineType.Polynomial && trendLine.Order >= 3 && trendLine.Order <= 6)
        sfTrendline.PolynomialOrder = trendLine.Order;
      if (!string.IsNullOrEmpty(trendLine.TrendLineTextArea.Text))
        this.TrendlineTextAndFormat(sfTrendline, trendLine, false);
      else if (trendLine.Type != ExcelTrendLineType.Moving_Average || trendLine.DisplayEquation || trendLine.DisplayRSquared)
        this.TrendlineTextAndFormat(sfTrendline, trendLine, true);
      DoubleCollection patternValues = (DoubleCollection) null;
      if (this.GetStrokeDashArrayValues((short) trendLine.Border.LinePattern, out patternValues))
        sfTrendline.StrokeDashArray = patternValues;
      sfTrendline.LegendIcon = ChartLegendIcon.StraightLine;
      sfTrendline.ForwardForecast = trendLine.Forward;
      sfTrendline.BackwardForecast = trendLine.Backward;
      ChartBorderImpl border = trendLine.Border as ChartBorderImpl;
      System.Drawing.Color color;
      if (border.AutoFormat && this.TryAndGetColorBasedOnElement(ChartElementsEnum.OtherLines, serieImpl.ParentChart, out color))
        sfTrendline.Stroke = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
      else
        sfTrendline.Stroke = this.GetBrushFromBorder(border);
      if (border.AutoFormat)
      {
        sfTrendline.StrokeThickness = 1.0;
      }
      else
      {
        sfTrendline.StrokeThickness = this.GetBorderThickness(border);
        if (sfTrendline.StrokeThickness != 0.0 && sfTrendline.StrokeThickness < 1.0)
          sfTrendline.StrokeThickness = 1.0;
      }
      sfSerie.Trendlines.Add(sfTrendline);
    }
  }

  internal void TrendlineTextAndFormat(
    Trendline sfTrendline,
    ChartTrendLineImpl trendLineImpl,
    bool isDefaultText)
  {
    this.TrendlineFormulas = this.TrendlineFormulas ?? new Dictionary<Trendline, TrendLineBorder>();
    TextBlock textBlock = new TextBlock();
    textBlock.Margin = new Thickness(2.0);
    this.SfTextBlock(textBlock, trendLineImpl.TrendLineTextArea as ChartTextAreaImpl);
    TrendLineBorder trendLineBorder = new TrendLineBorder();
    RectangleF manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    trendLineBorder.TrendLineBorderAndLayout(trendLineImpl, out manualRect, textBlock, this, isDefaultText);
    trendLineBorder.TrendLineLayout = manualRect;
    trendLineBorder.Border = this.GetBrushForTextElements(trendLineImpl.TrendLineTextArea.FrameFormat.Border as ChartBorderImpl);
    this.SetTransformAndBackGround(trendLineBorder.Border, trendLineImpl.TrendLineTextArea as ChartTextAreaImpl, (ChartAxisImpl) null);
    trendLineBorder.Border.Child = (UIElement) textBlock;
    this.TrendlineFormulas.Add(sfTrendline, trendLineBorder);
  }

  internal void SfSecondaryAxis(
    ChartSerieImpl xlsioSerie,
    ChartImpl xlsioChart,
    ChartSeriesBase chartSerie)
  {
    if (xlsioSerie.UsePrimaryAxis || this.SecondayAxisAchived)
      return;
    if (chartSerie is RadarSeries)
    {
      RadarSeries radarSeries1 = chartSerie as RadarSeries;
      RadarSeries radarSeries2 = radarSeries1;
      CustomNumericalAxis customNumericalAxis1 = new CustomNumericalAxis();
      customNumericalAxis1.ShowGridLines = false;
      customNumericalAxis1.RangePadding = NumericalPadding.Normal;
      customNumericalAxis1.OpposedPosition = true;
      CustomNumericalAxis customNumericalAxis2 = customNumericalAxis1;
      radarSeries2.YAxis = (RangeAxisBase) customNumericalAxis2;
      RadarSeries radarSeries3 = radarSeries1;
      CustomCategoryAxis customCategoryAxis1 = new CustomCategoryAxis();
      customCategoryAxis1.ShowGridLines = false;
      customCategoryAxis1.OpposedPosition = true;
      CustomCategoryAxis customCategoryAxis2 = customCategoryAxis1;
      radarSeries3.XAxis = (ChartAxisBase2D) customCategoryAxis2;
      if ((xlsioChart.SecondaryValueAxis as ChartAxisImpl).Deleted)
        radarSeries1.YAxis.Visibility = Visibility.Collapsed;
      if ((xlsioChart.SecondaryCategoryAxis as ChartAxisImpl).Deleted)
        radarSeries1.XAxis.Visibility = Visibility.Collapsed;
      this.SfSecondaryAxisCommon((IChart) xlsioChart, radarSeries1.XAxis, radarSeries1.YAxis);
    }
    else
    {
      CartesianSeries cartesianSeries1 = chartSerie as CartesianSeries;
      CartesianSeries cartesianSeries2 = cartesianSeries1;
      CustomNumericalAxis customNumericalAxis3 = new CustomNumericalAxis();
      customNumericalAxis3.ShowGridLines = false;
      customNumericalAxis3.RangePadding = NumericalPadding.Normal;
      customNumericalAxis3.OpposedPosition = true;
      CustomNumericalAxis customNumericalAxis4 = customNumericalAxis3;
      cartesianSeries2.YAxis = (RangeAxisBase) customNumericalAxis4;
      if ((xlsioChart.SecondaryValueAxis as ChartAxisImpl).Deleted)
        cartesianSeries1.YAxis.Visibility = Visibility.Collapsed;
      if ((xlsioChart.SecondaryCategoryAxis as ChartCategoryAxisImpl).IsChartBubbleOrScatter)
      {
        CartesianSeries cartesianSeries3 = cartesianSeries1;
        CustomNumericalAxis customNumericalAxis5 = new CustomNumericalAxis();
        customNumericalAxis5.ShowGridLines = false;
        customNumericalAxis5.OpposedPosition = true;
        CustomNumericalAxis customNumericalAxis6 = customNumericalAxis5;
        cartesianSeries3.XAxis = (ChartAxisBase2D) customNumericalAxis6;
      }
      else
      {
        CartesianSeries cartesianSeries4 = cartesianSeries1;
        CustomCategoryAxis customCategoryAxis3 = new CustomCategoryAxis();
        customCategoryAxis3.ShowGridLines = false;
        customCategoryAxis3.OpposedPosition = true;
        CustomCategoryAxis customCategoryAxis4 = customCategoryAxis3;
        cartesianSeries4.XAxis = (ChartAxisBase2D) customCategoryAxis4;
      }
      if ((xlsioChart.SecondaryCategoryAxis as ChartAxisImpl).Deleted)
        cartesianSeries1.XAxis.Visibility = Visibility.Collapsed;
      this.SfSecondaryAxisCommon((IChart) xlsioChart, cartesianSeries1.XAxis, cartesianSeries1.YAxis);
    }
  }

  protected void SfSecondaryAxisCommon(
    IChart xlsioChart,
    ChartAxisBase2D sfXAxis,
    RangeAxisBase sfYAxis)
  {
    ChartValueAxisImpl secondaryValueAxis = xlsioChart.SecondaryValueAxis as ChartValueAxisImpl;
    ChartCategoryAxisImpl secondaryCategoryAxis = xlsioChart.SecondaryCategoryAxis as ChartCategoryAxisImpl;
    if (this.SecondayAxisAchived)
      return;
    if (sfYAxis != null)
    {
      if (sfYAxis.Visibility == Visibility.Visible)
      {
        this.SfNumericalAxis(sfYAxis as CustomNumericalAxis, secondaryValueAxis, (ChartValueAxisImpl) secondaryCategoryAxis, true);
        if (!secondaryCategoryAxis.IsMaxCross)
          sfYAxis.OpposedPosition = false;
      }
      else if (secondaryValueAxis.ReversePlotOrder)
        sfYAxis.IsInversed = true;
    }
    if (sfXAxis != null)
    {
      if (sfXAxis.Visibility == Visibility.Visible)
      {
        if (sfXAxis is CustomNumericalAxis)
          this.SfNumericalAxis(sfXAxis as CustomNumericalAxis, (ChartValueAxisImpl) secondaryCategoryAxis, secondaryValueAxis, true);
        else
          this.SfCategoryAxis(sfXAxis as CustomCategoryAxis, secondaryCategoryAxis, secondaryValueAxis, true);
      }
      else
      {
        if (secondaryCategoryAxis.ReversePlotOrder)
          sfXAxis.IsInversed = true;
        sfXAxis.Origin = 0.0;
        if (sfXAxis is CustomCategoryAxis)
        {
          CustomCategoryAxis customCategoryAxis = sfXAxis as CustomCategoryAxis;
          if (secondaryCategoryAxis.IsBetween)
            customCategoryAxis.LabelPlacement = LabelPlacement.BetweenTicks;
        }
      }
    }
    if (sfXAxis == null && sfYAxis == null)
      return;
    this.SecondayAxisAchived = true;
  }

  internal string GetSerieName(ChartSerieImpl serie)
  {
    if (serie.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      return "Series" + (serie.Index + 1).ToString();
    if (serie.NameRange != null)
    {
      if (serie.NameRange is RangeImpl)
        return serie.GetSeriesValueFromRange(serie.NameRange as RangeImpl);
      return serie.NameCache == null ? serie.Name : serie.NameCache;
    }
    return !serie.IsDefaultName ? serie.Name : "Series" + (serie.Index + 1).ToString();
  }

  internal bool GetStrokeDashArrayValues(short LinePatternFlag, out DoubleCollection patternValues)
  {
    patternValues = (DoubleCollection) null;
    if (LinePatternFlag == (short) 10)
      LinePatternFlag = (short) 1;
    else if (LinePatternFlag == (short) 11)
      LinePatternFlag = (short) 3;
    else if (LinePatternFlag == (short) 12)
      LinePatternFlag = (short) 4;
    switch ((int) LinePatternFlag - 1)
    {
      case 0:
        patternValues = new DoubleCollection() { 6.0, 2.0 };
        break;
      case 1:
        patternValues = new DoubleCollection() { 2.0, 1.0 };
        break;
      case 2:
        patternValues = new DoubleCollection()
        {
          6.0,
          2.0,
          1.0,
          2.0
        };
        break;
      case 3:
        patternValues = new DoubleCollection()
        {
          6.0,
          2.0,
          1.0,
          2.0,
          1.0,
          2.0
        };
        break;
      case 8:
        patternValues = new DoubleCollection() { 1.0, 1.0 };
        break;
    }
    return patternValues != null;
  }

  internal void SetGapWidthandOverlap(ChartSeriesBase seriesBase, ChartSerieImpl serie)
  {
    double overlap = double.NaN;
    if (!this.IsChart3D)
    {
      if (!this.SetGapWidthOnSingleSeries(seriesBase, serie, out overlap, false))
      {
        double num = (double) serie.SerieFormat.CommonSerieOptions.GapWidth * 0.75 / 500.0;
        overlap = -((double) serie.SerieFormat.CommonSerieOptions.Overlap * 0.75 / 100.0);
        if (overlap > 0.0)
          num = num - overlap == 1.0 ? 0.9 : (!serie.ParentChart.IsSeriesInRows || serie.ParentChart.Series.Count <= 3 ? num : (num - overlap > 0.0 ? num - overlap : num));
        seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) num);
      }
      if (double.IsNaN(overlap))
        return;
      switch (seriesBase.GetType().Name)
      {
        case "ColumnSeries":
          seriesBase.SetValue(ColumnSeries.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingColumnSeries":
          seriesBase.SetValue(StackingColumnSeries.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingColumn100Series":
          seriesBase.SetValue(StackingColumnSeries.SegmentSpacingProperty, (object) overlap);
          break;
        case "BarSeries":
          seriesBase.SetValue(BarSeries.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingBarSeries":
          seriesBase.SetValue(StackingBarSeries.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingBar100Series":
          seriesBase.SetValue(StackingBarSeries.SegmentSpacingProperty, (object) overlap);
          break;
      }
    }
    else
    {
      if (!this.SetGapWidthOnSingleSeries(seriesBase, serie, out overlap, true) || double.IsNaN(overlap))
        return;
      switch (seriesBase.GetType().Name)
      {
        case "ColumnSeries3D":
          seriesBase.SetValue(ColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingColumnSeries3D":
          seriesBase.SetValue(StackingColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingColumn100Series3D":
          seriesBase.SetValue(StackingColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
        case "BarSeries3D":
          seriesBase.SetValue(ColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingBarSeries3D":
          seriesBase.SetValue(StackingColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
        case "StackingBar100Series3D":
          seriesBase.SetValue(StackingColumnSeries3D.SegmentSpacingProperty, (object) overlap);
          break;
      }
    }
  }

  private bool SetGapWidthOnSingleSeries(
    ChartSeriesBase seriesBase,
    ChartSerieImpl serie,
    out double overlap,
    bool isChart3D)
  {
    ExcelChartType serieType = serie.SerieType;
    overlap = double.NaN;
    double num1 = isChart3D ? 100.0 : (double) serie.SerieFormat.CommonSerieOptions.Overlap;
    bool flag1 = serie.ParentChart.IsStacked || serie.ParentChart.ChartType == ExcelChartType.Combination_Chart && serieType.ToString().Contains("Stacked");
    bool flag2 = this.IsChartEx || serie.ParentChart.IsClustered && serie.ParentChart.Series.Count == 1 || !isChart3D && serie.ParentSeries.Count > 1 && num1 == 100.0 && serie.SerieFormat.CommonSerieOptions.GapWidth == 0 || serie.ParentChart.ChartType == ExcelChartType.Combination_Chart && serie.ParentSeries.Count<IChartSerie>((Func<IChartSerie, bool>) (x => x.SerieType == serieType && x.UsePrimaryAxis == serie.UsePrimaryAxis)) == 1;
    double gapWidth = (double) serie.SerieFormat.CommonSerieOptions.GapWidth;
    if (flag1)
    {
      if (gapWidth <= 50.0)
        seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) (0.05 + 0.006 * gapWidth));
      else if (gapWidth <= 200.0)
        seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) (0.35 + 0.002 * (gapWidth - 50.0)));
      else
        seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) (0.65 + 0.0005 * (gapWidth - 200.0)));
      return true;
    }
    if (!flag2 && !isChart3D)
      return false;
    if (gapWidth == 0.0)
    {
      if (this.IsChartEx)
      {
        if (serie.ParentChart.IsHistogramOrPareto && seriesBase.ItemsSource != null && (seriesBase.ItemsSource as ObservableCollection<ChartPoint>).Count == 1)
          seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) 0.0);
        else
          seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) 0.05);
      }
      else
      {
        if (!isChart3D && num1 == 100.0 && serie.SerieFormat.CommonSerieOptions.GapWidth == 0)
        {
          int num2 = serie.ParentSeries.Count<IChartSerie>((Func<IChartSerie, bool>) (x => x.SerieType == serieType));
          seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) -((double) num2 - 1.01));
          overlap = 0.0;
          return true;
        }
        seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) 0.001);
      }
    }
    else
    {
      overlap = gapWidth >= 100.0 ? 0.1 + gapWidth / 100.0 * 0.1 : 0.0;
      double num3 = gapWidth <= 100.0 ? 0.01 + 0.005 * gapWidth : 0.25 + gapWidth / 100.0 * 0.1;
      seriesBase.SetValue(ChartSeriesBase.SpacingProperty, (object) num3);
    }
    if (isChart3D && !flag2)
      overlap = double.NaN;
    return true;
  }

  internal double GetBorderThickness(ChartBorderImpl border)
  {
    double borderThickness = 0.0;
    if ((!this.parentWorkbook.IsCreated ? (!border.AutoFormat ? 1 : 0) : 1) != 0 && (short) border.LinePattern != (short) 5)
      borderThickness = border.LineWeightString == null ? ((short) border.LineWeight != (short) -1 ? (double) ((int) (short) border.LineWeight + 1) : 0.75) : (!(border.LineWeightString != "0") ? 0.5 : (double) int.Parse(border.LineWeightString) / 12700.0);
    return borderThickness;
  }

  internal void SfRotation3D(ChartImpl chartImpl, SfChart3D sfChart)
  {
    if (!chartImpl.IsChartPie)
    {
      int rotation = chartImpl.Rotation;
      if (chartImpl.RightAngleAxes)
      {
        sfChart.PerspectiveAngle = 0.0;
        sfChart.Rotation = rotation > 90 ? (rotation <= 90 || rotation >= 180 ? (rotation != 180 ? (rotation > 270 ? (double) (-(360 - rotation) / 3) : (double) ((270 - rotation) / 3 - 30)) : 0.0) : (double) ((180 - rotation) / 3)) : (double) (rotation / 3);
        sfChart.Tilt = (double) (chartImpl.Elevation / 3);
      }
      else
      {
        sfChart.Rotation = (double) rotation;
        sfChart.PerspectiveAngle = (double) chartImpl.Perspective;
        sfChart.Tilt = chartImpl.Elevation < 0 ? (double) (360 + chartImpl.Elevation) : (chartImpl.Elevation >= 2 ? (double) chartImpl.Elevation : 2.0);
      }
      double num = chartImpl.RightAngleAxes ? 40.0 : 0.0;
      sfChart.Depth = (double) chartImpl.DepthPercent + num;
      if (chartImpl.Elevation >= 0)
        return;
      sfChart.PrimaryAxis.Visibility = Visibility.Collapsed;
    }
    else
    {
      sfChart.PerspectiveAngle = (double) chartImpl.Perspective;
      sfChart.Depth = 30.0;
      sfChart.Tilt = (double) (chartImpl.Elevation - 90);
    }
  }

  internal void SfWall(SfChart3D sfChart3D, ChartImpl chart)
  {
    sfChart3D.BackWallBrush = this.GetWallBrush(chart, chart.BackWall as ChartWallOrFloorImpl, false);
    sfChart3D.LeftWallBrush = this.GetWallBrush(chart, chart.SideWall as ChartWallOrFloorImpl, false);
    sfChart3D.RightWallBrush = this.GetWallBrush(chart, chart.SideWall as ChartWallOrFloorImpl, false);
    sfChart3D.BottomWallBrush = this.GetWallBrush(chart, chart.Floor as ChartWallOrFloorImpl, true);
  }

  private System.Windows.Media.Brush GetWallBrush(
    ChartImpl chart,
    ChartWallOrFloorImpl wall,
    bool isFloor)
  {
    if (!wall.IsAutomaticFormat && wall.Interior.Pattern != ExcelPattern.None)
      return this.GetBrushFromDataFormat((IChartFillBorder) wall);
    if (isFloor && this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
      return (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/));
    System.Drawing.Color color;
    return this.TryAndGetColorBasedOnElement(isFloor ? ChartElementsEnum.FloorFill : ChartElementsEnum.WallsFill, chart, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color, (chart.Style > 100 ? (chart.Style < 133 ? 1 : 0) : (chart.Style < 33 ? 1 : 0)) != 0 ? 1.0 : 0.0)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, this.parentWorkbook.Version == ExcelVersion.Excel97to2003 ? 0.0 : 1.0));
  }

  protected System.Windows.Media.Brush GetBrushFromBorder(ChartBorderImpl border)
  {
    System.Drawing.Color.FromArgb(0, 0, 0, 0);
    return (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(border.Fill == null || (short) border.Fill.FillType != (short) 7 ? border.LineColor : border.Fill.ForeColor, border.Transparency));
  }

  internal System.Windows.Media.Brush GetBrushFromDataFormat(IChartFillBorder fillFormat)
  {
    System.Drawing.Color foreColor = fillFormat.Fill.ForeColor;
    short fillType = (short) fillFormat.Fill.FillType;
    System.Windows.Media.Brush brushFromDataFormat = (System.Windows.Media.Brush) null;
    switch (fillType)
    {
      case 0:
        brushFromDataFormat = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(foreColor, fillFormat.Fill.Transparency));
        break;
      case 1:
        brushFromDataFormat = (short) fillFormat.Fill.Pattern != (short) 0 || (short) fillFormat.Interior.Pattern != (short) 0 ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(foreColor)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(foreColor, 1.0));
        break;
      case 7:
        if (fillFormat.Fill is ChartFillImpl fill && fill.PreservedGradient != null)
        {
          GradientStopImpl maxGradientStop = this.GetMaxGradientStop(fill.PreservedGradient);
          double transparency = 0.0;
          if (maxGradientStop.Transparency != 0)
            transparency = (double) (100000 - maxGradientStop.Transparency) / 100000.0;
          brushFromDataFormat = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(maxGradientStop.ColorObject.GetRGB((IWorkbook) this.parentWorkbook), transparency));
          break;
        }
        brushFromDataFormat = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(foreColor));
        break;
    }
    return brushFromDataFormat;
  }

  private GradientStopImpl GetMaxGradientStop(GradientStops gradientStops)
  {
    int num1 = 0;
    int index1 = 0;
    List<int> intList = new List<int>(gradientStops.Count);
    for (int index2 = 0; index2 < gradientStops.Count; ++index2)
      intList.Add(gradientStops[index2].Position / 1000);
    intList.Sort();
    for (int index3 = 0; index3 < gradientStops.Count; ++index3)
    {
      int num2 = gradientStops[index3].Position / 1000;
      int num3 = intList.IndexOf(num2);
      int num4 = num2 - (num3 == 0 ? 0 : intList[num3 - 1]);
      int num5 = (num3 == intList.Count - 1 ? 100 : intList[num3 + 1]) - num2;
      if (num5 > num1)
      {
        index1 = index3;
        num1 = num5;
      }
      if (num4 > num1)
      {
        index1 = index3;
        num1 = num4;
      }
      if (index3 == gradientStops.Count - 1 && num1 == num4 && num5 == 0)
        index1 = index3;
    }
    intList.Clear();
    return gradientStops[index1];
  }

  private void CheckAndApplyAxisLineStyle(
    ChartBorderImpl border,
    out Style lineStyle,
    ChartImpl chart,
    ChartElementsEnum elementEnum,
    bool isAxisLine)
  {
    lineStyle = new Style() { TargetType = typeof (Line) };
    System.Windows.Media.Brush brush = (System.Windows.Media.Brush) null;
    double num = 0.0;
    if (isAxisLine)
    {
      if (border.LinePattern != ExcelChartLinePattern.None)
      {
        num = this.GetBorderThickness(border);
        if (border.IsAutoLineColor && !border.HasGradientFill)
        {
          System.Drawing.Color color;
          brush = this.parentWorkbook.Version != ExcelVersion.Excel97to2003 ? (!this.TryAndGetColorBasedOnElement(elementEnum, chart, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 134, (byte) 134, (byte) 134)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
          num = 1.0;
        }
        else
          brush = this.GetBrushFromBorder(border);
      }
      else
        brush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    }
    Setter setter1 = new Setter()
    {
      Property = Shape.StrokeProperty,
      Value = (object) brush
    };
    lineStyle.Setters.Add((SetterBase) setter1);
    Setter setter2 = new Setter()
    {
      Property = Shape.StrokeThicknessProperty,
      Value = (object) num
    };
    lineStyle.Setters.Add((SetterBase) setter2);
  }

  internal Border GetBrushForTextElements(ChartBorderImpl border)
  {
    double uniformLength = 0.0;
    Border brushForTextElements = (Border) null;
    System.Windows.Media.Brush brush;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      if (border.HasLineProperties)
        uniformLength = this.GetBorderThickness(border);
      brush = !border.IsAutoLineColor || border.HasGradientFill ? this.GetBrushFromBorder(border) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    }
    else
      brush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1.0));
    if (brush != null)
    {
      if (uniformLength != 0.0)
        uniformLength = uniformLength < 1.0 ? 1.0 : uniformLength + 1.0;
      brushForTextElements = new Border();
      brushForTextElements.BorderThickness = new Thickness(uniformLength);
      brushForTextElements.BorderBrush = brush;
      brushForTextElements.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
    }
    return brushForTextElements;
  }

  internal void TryAndSetChartAxisTitle(
    ChartAxis sfAxis,
    ChartValueAxisImpl axisImpl,
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisBorders,
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisDisplayUnitBorders,
    bool axisChangedOnPlotArea,
    RectangleF manualRect,
    bool isRadar)
  {
    if (sfAxis == null)
      return;
    Border axisTitleBorder = (Border) null;
    Border border1 = (Border) null;
    RectangleF manualRect1 = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    RectangleF manualRect2 = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if (sfAxis.OpposedPosition)
      this.AxisTitleRotationAngle = 270;
    if (isRadar)
      axisChangedOnPlotArea = false;
    bool isVertical = false;
    if (!isRadar && axisImpl.HasAxisTitle)
      axisTitleBorder = this.GetSfAxisTitle((ChartAxisImpl) axisImpl, out manualRect2, out isVertical);
    bool flag = !(axisImpl is ChartCategoryAxisImpl) || (axisImpl as ChartCategoryAxisImpl).IsChartBubbleOrScatter;
    if (axisImpl.Visible && flag && axisImpl.HasDisplayUnitLabel && axisImpl.DisplayUnit != ExcelChartDisplayUnit.None && (!flag || !this.IsChartEx || !(axisImpl.DisplayUnit == ExcelChartDisplayUnit.Custom & axisImpl.ParentChart.ChartType.ToString().Contains("Pareto")) || axisImpl.DisplayUnitLabel.Text != null))
      border1 = this.GetDisplayUnitLabel(axisImpl, sfAxis, out manualRect1, out isVertical);
    if ((border1 == null ? (axisTitleBorder != null ? 1 : 0) : ((double) manualRect1.X < 0.0 ? 0 : ((double) manualRect1.Y >= 0.0 ? 1 : 0))) != 0)
    {
      if (axisTitleBorder != null)
      {
        if (((double) manualRect2.X < 0.0 || (double) manualRect2.Y < 0.0) && !axisChangedOnPlotArea)
        {
          sfAxis.Header = (object) axisTitleBorder;
        }
        else
        {
          if (isVertical)
            this.RetainAxisTitleRotation(axisTitleBorder, axisImpl.TitleArea as ChartTextAreaImpl);
          axisBorders.Add(sfAxis, new Tuple<Border, PointF>(axisTitleBorder, new PointF(manualRect2.X, manualRect2.Y)));
        }
      }
      if (border1 != null)
      {
        if (isVertical)
          this.RetainAxisTitleRotation(border1, axisImpl.DisplayUnitLabel as ChartTextAreaImpl);
        axisDisplayUnitBorders.Add(sfAxis, new Tuple<Border, PointF>(border1, new PointF(manualRect1.X, manualRect1.Y)));
      }
    }
    else if (border1 != null)
    {
      if (axisChangedOnPlotArea)
      {
        if (isVertical)
        {
          this.RetainAxisTitleRotation(border1, axisImpl.DisplayUnitLabel as ChartTextAreaImpl);
          if (axisTitleBorder != null)
            this.RetainAxisTitleRotation(axisTitleBorder, axisImpl.TitleArea as ChartTextAreaImpl);
        }
        axisDisplayUnitBorders.Add(sfAxis, new Tuple<Border, PointF>(border1, new PointF(manualRect1.X, manualRect1.Y)));
        if (axisTitleBorder != null)
          axisBorders.Add(sfAxis, new Tuple<Border, PointF>(axisTitleBorder, new PointF(manualRect2.X, manualRect2.Y)));
      }
      else
      {
        if ((axisTitleBorder != null ? ((double) manualRect2.X < 0.0 ? 1 : ((double) manualRect2.Y < 0.0 ? 1 : 0)) : 1) != 0)
          sfAxis.HeaderTemplate = this.GetAxisTitleTemplate(sfAxis, axisTitleBorder, border1, isVertical, manualRect);
        if (axisTitleBorder != null && (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0)
        {
          if (isVertical)
            this.RetainAxisTitleRotation(axisTitleBorder, axisImpl.TitleArea as ChartTextAreaImpl);
          axisBorders.Add(sfAxis, new Tuple<Border, PointF>(axisTitleBorder, new PointF(manualRect2.X, manualRect2.Y)));
        }
      }
    }
    if (sfAxis.Header == null && (double) manualRect.X < 0.0 && (double) manualRect.Y < 0.0)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      if (axisTitleBorder != null)
      {
        axisTitleBorder.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
        num1 += axisTitleBorder.DesiredSize.Width;
        num2 += axisTitleBorder.DesiredSize.Height;
      }
      if (border1 != null)
      {
        border1.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
        num1 += border1.DesiredSize.Width;
        num2 += border1.DesiredSize.Height;
      }
      ChartAxis chartAxis = sfAxis;
      Border border2 = new Border();
      border2.Width = isVertical ? num2 : num1;
      border2.Height = isVertical ? num1 : num2;
      Border border3 = border2;
      chartAxis.Header = (object) border3;
    }
    this.AxisTitleRotationAngle = 90;
  }

  internal DataTemplate GetAxisTitleTemplate(
    ChartAxis sfAxis,
    Border axisTitleBorder,
    Border displayUnitBorder,
    bool isVertical,
    RectangleF manualRect)
  {
    DataTemplate axisTitleTemplate = new DataTemplate((object) typeof (Grid));
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Grid));
    double num1 = 0.0;
    double num2 = 0.0;
    FrameworkElementFactory child1 = (FrameworkElementFactory) null;
    if (axisTitleBorder != null)
      child1 = this.GenrateFrameworkElementFactoryObject(axisTitleBorder);
    FrameworkElementFactory child2 = this.GenrateFrameworkElementFactoryObject(displayUnitBorder);
    if (axisTitleBorder != null)
    {
      axisTitleBorder.LayoutTransform = (Transform) new RotateTransform()
      {
        Angle = 0.0
      };
      axisTitleBorder.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
      double width = axisTitleBorder.DesiredSize.Width;
      num1 = axisTitleBorder.DesiredSize.Height;
    }
    if (displayUnitBorder != null)
    {
      displayUnitBorder.LayoutTransform = (Transform) new RotateTransform()
      {
        Angle = 0.0
      };
      displayUnitBorder.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
      double width = displayUnitBorder.DesiredSize.Width;
      num2 = displayUnitBorder.DesiredSize.Height;
    }
    FrameworkElementFactory child3 = (FrameworkElementFactory) null;
    FrameworkElementFactory child4 = new FrameworkElementFactory(typeof (RowDefinition));
    frameworkElementFactory.AppendChild(child4);
    if (!this.IsChart3D && axisTitleBorder != null)
    {
      child3 = new FrameworkElementFactory(typeof (RowDefinition));
      frameworkElementFactory.AppendChild(child3);
    }
    FrameworkElementFactory child5 = new FrameworkElementFactory(typeof (ColumnDefinition));
    child5.SetValue(ColumnDefinition.WidthProperty, (object) new GridLength(1.0, GridUnitType.Star));
    frameworkElementFactory.AppendChild(child5);
    Binding binding = new Binding();
    binding.Source = (object) sfAxis;
    binding.Mode = BindingMode.OneWay;
    if (isVertical)
    {
      binding.Path = new PropertyPath("HeightExt", new object[0]);
      frameworkElementFactory.SetBinding(FrameworkElement.WidthProperty, (BindingBase) binding);
    }
    else
    {
      binding.Path = new PropertyPath("WidthExt", new object[0]);
      frameworkElementFactory.SetBinding(FrameworkElement.WidthProperty, (BindingBase) binding);
    }
    frameworkElementFactory.SetValue(FrameworkElement.MaxHeightProperty, (object) (num2 + num1));
    if (axisTitleBorder != null)
    {
      child1.SetValue(Border.PaddingProperty, (object) new Thickness(0.0));
      child1.SetValue(FrameworkElement.HorizontalAlignmentProperty, (object) HorizontalAlignment.Center);
      child1.SetValue(FrameworkElement.MarginProperty, (object) new Thickness(0.0));
      child1.SetValue(Grid.ColumnProperty, (object) 0);
      int num3 = this.IsChart3D ? 0 : (isVertical || sfAxis.OpposedPosition ? 0 : 1);
      child1.SetValue(Grid.RowProperty, (object) num3);
      if (!this.IsChart3D)
      {
        if (num3 == 0)
          child4.SetValue(RowDefinition.HeightProperty, (object) new GridLength(num1));
        else
          child3.SetValue(RowDefinition.HeightProperty, (object) new GridLength(num1));
      }
      frameworkElementFactory.AppendChild(child1);
    }
    child2.SetValue(FrameworkElement.HorizontalAlignmentProperty, (object) (HorizontalAlignment) (!sfAxis.OpposedPosition || !isVertical ? 2 : 0));
    child2.SetValue(Border.PaddingProperty, (object) new Thickness(0.0));
    child2.SetValue(FrameworkElement.MarginProperty, (object) new Thickness(0.0));
    child2.SetValue(Grid.ColumnProperty, (object) 0);
    int num4 = this.IsChart3D ? 0 : (axisTitleBorder == null ? 0 : (isVertical || sfAxis.OpposedPosition ? 1 : 0));
    child2.SetValue(Grid.RowProperty, (object) num4);
    if (!this.IsChart3D)
    {
      if (num4 == 0)
        child4.SetValue(RowDefinition.HeightProperty, (object) new GridLength(num2));
      else
        child3.SetValue(RowDefinition.HeightProperty, (object) new GridLength(num2));
    }
    else
    {
      child4.SetValue(RowDefinition.HeightProperty, (object) new GridLength(Math.Max(num2, num1)));
      if (axisTitleBorder != null)
        child1.SetValue(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Bottom);
      child2.SetValue(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Bottom);
    }
    frameworkElementFactory.AppendChild(child2);
    axisTitleTemplate.VisualTree = frameworkElementFactory;
    return axisTitleTemplate;
  }

  internal FrameworkElementFactory GenrateFrameworkElementFactoryObject(Border borderUI)
  {
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Border));
    frameworkElementFactory.SetValue(Border.BorderThicknessProperty, (object) borderUI.BorderThickness);
    frameworkElementFactory.SetValue(Border.BorderBrushProperty, (object) borderUI.BorderBrush);
    frameworkElementFactory.SetValue(FrameworkElement.MarginProperty, (object) borderUI.Margin);
    frameworkElementFactory.SetValue(FrameworkElement.LayoutTransformProperty, (object) borderUI.LayoutTransform);
    frameworkElementFactory.SetValue(Border.BackgroundProperty, (object) borderUI.Background);
    frameworkElementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Center);
    TextBlock child1 = borderUI.Child as TextBlock;
    FrameworkElementFactory child2 = new FrameworkElementFactory(typeof (TextBlockExt));
    child2.SetValue(TextBlock.TextAlignmentProperty, (object) child1.TextAlignment);
    if (child1.Inlines != null && child1.Inlines.Count > 1)
    {
      ObservableCollection<Inline> observableCollection = new ObservableCollection<Inline>();
      foreach (Inline inline in (TextElementCollection<Inline>) child1.Inlines)
        observableCollection.Add(inline);
      child2.SetValue(TextBlockExt.InlineListProperty, (object) observableCollection);
    }
    else
    {
      child2.SetValue(TextBlock.TextProperty, (object) child1.Text);
      child2.SetValue(TextBlock.FontSizeProperty, (object) child1.FontSize);
      child2.SetValue(TextBlock.FontFamilyProperty, (object) child1.FontFamily);
      child2.SetValue(TextBlock.TextDecorationsProperty, (object) child1.TextDecorations);
      child2.SetValue(TextBlock.FontStyleProperty, (object) child1.FontStyle);
      child2.SetValue(TextBlock.FontWeightProperty, (object) child1.FontWeight);
      child2.SetValue(TextBlock.ForegroundProperty, (object) child1.Foreground);
    }
    child2.SetValue(TextBlock.TextWrappingProperty, (object) TextWrapping.Wrap);
    child2.SetValue(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Center);
    frameworkElementFactory.AppendChild(child2);
    return frameworkElementFactory;
  }

  internal void RetainAxisTitleRotation(Border axisTitleBorder, ChartTextAreaImpl textArea)
  {
    RotateTransform rotateTransform1 = (RotateTransform) null;
    rotateTransform1 = axisTitleBorder.LayoutTransform as RotateTransform;
    RotateTransform rotateTransform2 = new RotateTransform()
    {
      Angle = (double) textArea.TextRotationAngle
    };
    axisTitleBorder.LayoutTransform = (Transform) rotateTransform2;
  }

  internal void TryAndSetPlotAreaMargins(
    IChart _chart,
    SfChartExt sfChart,
    ChartLegend legend,
    RectangleF manualRect,
    bool isInnerLayoutTarget,
    bool ignoreTLBR)
  {
    bool isLegendOutside = false;
    if (_chart.HasLegend)
    {
      if (this.IsChartEx)
      {
        if (!_chart.Legend.IncludeInLayout)
          isLegendOutside = true;
      }
      else if (_chart.Legend.IncludeInLayout)
        isLegendOutside = true;
    }
    else
      isLegendOutside = false;
    if (this.IsChartEx || (double) manualRect.X < 0.0 || (double) manualRect.Y < 0.0)
    {
      bool flag = false;
      Thickness marginThickness;
      if (ignoreTLBR)
      {
        marginThickness = new Thickness(0.0);
      }
      else
      {
        byte emptySpacesAdded = this.GetEmptySpacesAdded(sfChart, _chart.ChartType.ToString().Contains("Funnel"), this.IsLegendManualLayout ? (ChartLegend) null : legend);
        double top = ((int) emptySpacesAdded & 1) == 0 ? 10.0 : 5.0;
        double left = ((int) emptySpacesAdded & 2) == 0 ? 10.0 : 5.0;
        double bottom = ((int) emptySpacesAdded & 4) == 0 ? 10.0 : 5.0;
        double right = ((int) emptySpacesAdded & 8) == 0 ? 10.0 : 5.0;
        if (sfChart.Header != null && (sfChart.Header as Border).Child == null && legend != null && legend.DockPosition == ChartDock.Bottom && !this.IsLegendManualLayout)
        {
          bottom = legend.DesiredSize.Height + 5.0;
          flag = true;
        }
        marginThickness = new Thickness(left, top, right, bottom);
      }
      sfChart.SetMargin(marginThickness, false, isLegendOutside, this.IsLegendManualLayout, flag ? -1 : (_chart.HasLegend ? (int) _chart.Legend.Position : 0));
      if (sfChart == null || ignoreTLBR || this.IsLegendManualLayout || (legend.ItemTemplate == null ? 1 : (legend.ItemTemplate.VisualTree == null ? 1 : 0)) == 0 || legend.DockPosition != ChartDock.Bottom && legend.DockPosition != ChartDock.Top)
        return;
      sfChart.AddHandlerLayoutUpdate(false);
    }
    else
    {
      Thickness marginThickness = new Thickness((double) manualRect.X, (double) manualRect.Y, (double) this.ChartWidth - ((double) manualRect.Width + (double) manualRect.X), (double) this.ChartHeight - ((double) manualRect.Height + (double) manualRect.Y));
      sfChart.SetMargin(marginThickness, isInnerLayoutTarget, isLegendOutside, this.IsLegendManualLayout, !ignoreTLBR || this.IsLegendManualLayout ? -1 : 0);
    }
  }

  internal byte GetEmptySpacesAdded(SfChartExt sfChart, bool isFunnel, ChartLegend legend)
  {
    byte emptySpacesAdded = 0;
    bool flag1 = !this.SecondayAxisAchived;
    bool flag2 = false;
    if (sfChart.Header != null)
      emptySpacesAdded |= (byte) 1;
    if (sfChart.Series.Count == 0)
      return emptySpacesAdded;
    if (legend != null && legend.Visibility == Visibility.Visible && legend.LegendPosition == LegendPosition.Outside)
    {
      switch (legend.DockPosition)
      {
        case ChartDock.Left:
          emptySpacesAdded |= (byte) 2;
          break;
        case ChartDock.Top:
          emptySpacesAdded |= (byte) 1;
          break;
        case ChartDock.Right:
          if (legend.VerticalAlignment == VerticalAlignment.Top && legend.HorizontalAlignment == HorizontalAlignment.Right && legend.Orientation == ChartOrientation.Vertical)
          {
            emptySpacesAdded |= (byte) 8;
            break;
          }
          emptySpacesAdded |= (byte) 9;
          break;
        case ChartDock.Bottom:
          emptySpacesAdded |= (byte) 4;
          break;
      }
    }
    int num = flag1 ? 1 : sfChart.Series.Count;
    for (int index = 0; index < num; ++index)
    {
      if (sfChart.Series[index] is CartesianSeries cartesianSeries)
      {
        if (!flag1 && cartesianSeries != null && (cartesianSeries.XAxis != null || cartesianSeries.YAxis != null))
        {
          bool flag3 = isFunnel || cartesianSeries.GetType().ToString().Contains("Bar");
          ChartAxisBase2D xaxis = cartesianSeries.XAxis;
          ChartAxisBase2D yaxis = (ChartAxisBase2D) cartesianSeries.YAxis;
          if (xaxis != null && xaxis.Visibility == Visibility.Visible && !xaxis.ShowAxisNextToOrigin)
          {
            if (flag3)
              emptySpacesAdded |= xaxis.OpposedPosition ? (byte) 8 : (byte) 2;
            else
              emptySpacesAdded |= xaxis.OpposedPosition ? (byte) 1 : (byte) 4;
          }
          if (yaxis != null && yaxis.Visibility == Visibility.Visible && !yaxis.ShowAxisNextToOrigin)
          {
            if (flag3)
              emptySpacesAdded |= yaxis.OpposedPosition ? (byte) 1 : (byte) 4;
            else
              emptySpacesAdded |= yaxis.OpposedPosition ? (byte) 8 : (byte) 2;
          }
          flag1 = true;
        }
        else
        {
          flag2 = true;
          bool flag4 = isFunnel || cartesianSeries.GetType().ToString().Contains("Bar");
          ChartAxisBase2D primaryAxis = sfChart.PrimaryAxis;
          ChartAxisBase2D secondaryAxis = (ChartAxisBase2D) sfChart.SecondaryAxis;
          if (primaryAxis != null && primaryAxis.Visibility == Visibility.Visible && !primaryAxis.ShowAxisNextToOrigin)
          {
            if (flag4)
              emptySpacesAdded |= primaryAxis.OpposedPosition ? (byte) 8 : (byte) 2;
            else
              emptySpacesAdded |= primaryAxis.OpposedPosition ? (byte) 1 : (byte) 4;
          }
          if (secondaryAxis != null && secondaryAxis.Visibility == Visibility.Visible && !secondaryAxis.ShowAxisNextToOrigin)
          {
            if (flag4)
              emptySpacesAdded |= secondaryAxis.OpposedPosition ? (byte) 1 : (byte) 4;
            else
              emptySpacesAdded |= secondaryAxis.OpposedPosition ? (byte) 8 : (byte) 2;
          }
        }
        if (flag1 && flag2)
          break;
      }
    }
    return emptySpacesAdded;
  }

  private Border GetDisplayUnitLabel(
    ChartValueAxisImpl axis,
    ChartAxis numericalAxis,
    out RectangleF manualRect,
    out bool isVertical)
  {
    isVertical = false;
    manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    switch (numericalAxis)
    {
      case CustomNumericalAxis3D _:
      case CustomNumericalAxis _:
      case LogarithmicAxis _:
      case LogarithmicAxis3D _:
        ChartTextAreaImpl displayUnitLabel = axis.DisplayUnitLabel as ChartTextAreaImpl;
        ChartLayoutImpl layout = displayUnitLabel.Layout as ChartLayoutImpl;
        if (layout.IsManualLayout)
        {
          ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
          if (manualLayout.FlagOptions != (byte) 0 && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
            manualRect = this.CalculateManualLayout(manualLayout, out bool _, false);
        }
        Border brushForTextElements = this.GetBrushForTextElements(displayUnitLabel.FrameFormat.Border as ChartBorderImpl);
        TextBlock textBlock = new TextBlock();
        isVertical = this.SetTransformAndBackGround(brushForTextElements, displayUnitLabel, (ChartAxisImpl) axis);
        if (isVertical && !displayUnitLabel.HasTextRotation)
          brushForTextElements.LayoutTransform = (Transform) new RotateTransform()
          {
            Angle = -90.0
          };
        if (displayUnitLabel.RichText != null && displayUnitLabel.RichText.FormattingRuns.Length > 1)
        {
          this.SetTextBlockInlines(displayUnitLabel, textBlock);
        }
        else
        {
          if (displayUnitLabel.ParagraphType == ChartParagraphType.Default)
          {
            textBlock.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            textBlock.FontSize = 10.0;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(System.Drawing.Color.FromArgb(0, 0, 0)));
          }
          else
            this.SfTextBlock(textBlock, displayUnitLabel);
          textBlock.Text = this.GetTextFromDisplayUnits(axis);
        }
        if (brushForTextElements != null)
          brushForTextElements.Child = (UIElement) textBlock;
        return brushForTextElements;
      default:
        return (Border) null;
    }
  }

  private string GetTextFromDisplayUnits(ChartValueAxisImpl axis)
  {
    int displayUnit = (int) axis.DisplayUnit;
    string fromDisplayUnits = string.Empty;
    if (axis.DisplayUnitLabel.Text == null)
    {
      switch (displayUnit)
      {
        case 1:
        case 2:
        case 5:
          fromDisplayUnits = axis.DisplayUnit.ToString();
          break;
        case 3:
          fromDisplayUnits = "x10000";
          break;
        case 4:
          fromDisplayUnits = "x100000";
          break;
        case 6:
          fromDisplayUnits = "x10000000";
          break;
        case 7:
          fromDisplayUnits = "x100000000";
          break;
        case 8:
          fromDisplayUnits = "Billions";
          break;
        case 9:
          fromDisplayUnits = "Trillions";
          break;
        case (int) ushort.MaxValue:
          fromDisplayUnits = "x" + axis.DisplayUnitCustom.ToString();
          break;
      }
    }
    else
    {
      ChartTextAreaImpl displayUnitLabel = axis.DisplayUnitLabel as ChartTextAreaImpl;
      fromDisplayUnits = displayUnitLabel.IsFormula ? this.GetTextFromRange(displayUnitLabel.Text, displayUnitLabel.StringCache) : displayUnitLabel.Text;
    }
    return fromDisplayUnits;
  }

  internal bool TryAndGetColorBasedOnElement(
    ChartElementsEnum inputelement,
    ChartImpl chart,
    out System.Drawing.Color color)
  {
    int style = chart.Style;
    color = System.Drawing.Color.FromArgb(0, 0, 0, 0);
    if (style > 100)
      style -= 100;
    if (style <= 0 || style > 48 /*0x30*/)
      return false;
    System.Drawing.Color mixColor1 = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    System.Drawing.Color mixColor2 = System.Drawing.Color.FromArgb(0, 0, 0, 0);
    switch (inputelement)
    {
      case ChartElementsEnum.AxisLine:
      case ChartElementsEnum.MajorGridLines:
        color = System.Drawing.Color.FromArgb(0, 134, 134, 134);
        break;
      case ChartElementsEnum.ChartAreaLine:
      case ChartElementsEnum.FloorLine:
        color = style >= 33 ? (style < 33 || style >= 41 ? System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue) : System.Drawing.Color.FromArgb(0, 134, 134, 134)) : System.Drawing.Color.FromArgb(0, 134, 134, 134);
        break;
      case ChartElementsEnum.ChartAreaFill:
        color = style >= 33 ? (style < 33 || style >= 41 ? mixColor2 : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)) : System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        break;
      case ChartElementsEnum.UpBarFill:
        if (style == 2 || style == 10 || style == 18 || style == 26)
        {
          color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
          color = this.GetPercentageTintOrShadeOfColor(color, 0.05, mixColor1);
          break;
        }
        if (style == 1 || style == 9 || style == 17 || style == 25 || style == 41)
        {
          color = System.Drawing.Color.FromArgb(0, 134, 134, 134);
          color = this.GetPercentageTintOrShadeOfColor(color, 0.25, mixColor1);
          break;
        }
        if (style >= 33 && style <= 40 || style == 42)
        {
          color = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        }
        int num1 = style;
        int index1 = 4 + (num1 >= 9 ? (num1 >= 17 ? (num1 >= 25 ? (num1 >= 33 ? num1 - 43 : num1 - 27) : num1 - 19) : num1 - 11) : num1 - 3);
        color = chart.GetChartThemeColorByColorIndex(index1);
        color = this.GetPercentageTintOrShadeOfColor(color, 0.25, mixColor1);
        break;
      case ChartElementsEnum.UpBarLine:
      case ChartElementsEnum.DownBarLine:
        if (style < 17)
        {
          color = chart.GetChartThemeColorByColorIndex(1);
          break;
        }
        if (style >= 17 && style < 33 || style >= 41 && style <= 48 /*0x30*/)
          return false;
        if (style == 33 || style == 34)
        {
          color = chart.GetChartThemeColorByColorIndex(1);
          break;
        }
        int index2 = 4 + (style - 35);
        color = chart.GetChartThemeColorByColorIndex(index2);
        color = this.GetPercentageTintOrShadeOfColor(color, 0.5, mixColor2);
        break;
      case ChartElementsEnum.DownBarFill:
        switch (style)
        {
          case 1:
          case 9:
          case 17:
          case 25:
          case 33:
          case 41:
            color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
            color = this.GetPercentageTintOrShadeOfColor(color, 0.85, mixColor1);
            break;
          case 2:
          case 10:
          case 18:
          case 26:
          case 34:
            color = System.Drawing.Color.FromArgb(0, 56, 56, 56);
            color = this.GetPercentageTintOrShadeOfColor(color, 0.95, mixColor1);
            break;
          case 42:
            color = mixColor2;
            break;
          default:
            int num2 = style;
            int index3 = 4 + (num2 >= 9 ? (num2 >= 17 ? (num2 >= 25 ? (num2 >= 33 ? (num2 >= 41 ? num2 - 43 : num2 - 35) : num2 - 27) : num2 - 19) : num2 - 11) : num2 - 3);
            color = chart.GetChartThemeColorByColorIndex(index3);
            color = this.GetPercentageTintOrShadeOfColor(color, 0.5, mixColor2);
            break;
        }
        break;
      case ChartElementsEnum.FloorFill:
      case ChartElementsEnum.WallsFill:
      case ChartElementsEnum.PlotAreaFill:
        if (style < 33)
        {
          color = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        }
        if (style == 33 || style == 34)
        {
          color = System.Drawing.Color.FromArgb(0, 134, 134, 134);
          color = this.GetPercentageTintOrShadeOfColor(color, 0.2, mixColor1);
          break;
        }
        if (style > 34 && style < 41)
        {
          int index4 = 4 + (style - 35);
          color = chart.GetChartThemeColorByColorIndex(index4);
          color = this.GetPercentageTintOrShadeOfColor(color, 0.2, mixColor1);
          break;
        }
        color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
        color = this.GetPercentageTintOrShadeOfColor(color, 0.95, mixColor1);
        break;
      case ChartElementsEnum.MinorGridLines:
        if (style < 41)
        {
          color = System.Drawing.Color.FromArgb(0, 134, 134, 134);
          color = this.GetPercentageTintOrShadeOfColor(color, 0.5, mixColor1);
          break;
        }
        color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
        color = this.GetPercentageTintOrShadeOfColor(color, 0.9, mixColor1);
        break;
      case ChartElementsEnum.OtherLines:
        if (style < 33)
        {
          color = mixColor2;
          break;
        }
        switch (style)
        {
          case 33:
          case 34:
            color = mixColor2;
            break;
          case 35:
          case 36:
          case 37:
          case 38:
          case 39:
          case 40:
            color = mixColor2;
            color = this.GetPercentageTintOrShadeOfColor(color, 0.25, mixColor2);
            break;
          default:
            color = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
        }
        break;
    }
    return true;
  }

  internal bool TryAndGetThicknessBasedOnElement(
    ChartElementsEnum inputelement,
    ChartImpl chart,
    out double thickness,
    short? value)
  {
    thickness = this.parentWorkbook.DefaultThemeVersion == "124226" ? 0.75 : 0.5;
    int style = chart.Style;
    if (style > 100)
      style -= 100;
    if (style <= 0 || style > 48 /*0x30*/)
      return false;
    switch (inputelement)
    {
      case ChartElementsEnum.DataPointLineThickness:
        if (style <= 8)
          thickness *= 3.0;
        else if (style >= 25 && style <= 32 /*0x20*/)
          thickness *= 7.0;
        else
          thickness *= 5.0;
        thickness = ApplicationImpl.ConvertUnitsStatic(thickness, MeasureUnits.Point, MeasureUnits.Pixel);
        break;
      case ChartElementsEnum.MarkerThickness:
        short? nullable = value;
        if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        {
          thickness = !(this.parentWorkbook.DefaultThemeVersion == "124226") ? (style > 8 ? (style < 25 || style > 32 /*0x20*/ ? 7.0 : 9.0) : 5.0) : (style > 8 ? (style < 25 || style > 32 /*0x20*/ ? 9.0 : 13.0) : 7.0);
          break;
        }
        thickness = 5.0;
        thickness += (double) ((int) value.Value * 2);
        break;
    }
    return true;
  }

  private System.Drawing.Color GetPercentageTintOrShadeOfColor(
    System.Drawing.Color inputColor,
    double percentValue,
    System.Drawing.Color mixColor)
  {
    return System.Drawing.Color.FromArgb(0, (int) (byte) Math.Truncate((double) inputColor.R * percentValue + (double) mixColor.R * (1.0 - percentValue)), (int) (byte) Math.Truncate((double) inputColor.G * percentValue + (double) mixColor.G * (1.0 - percentValue)), (int) (byte) Math.Truncate((double) inputColor.B * percentValue + (double) mixColor.B * (1.0 - percentValue)));
  }

  internal bool TryAndGetFillOrLineColorBasedOnPattern(
    ChartImpl chart,
    bool isLine,
    int formattingIndex,
    int highFormattingIndex,
    out System.Drawing.Color color)
  {
    int style = chart.Style;
    color = System.Drawing.Color.FromArgb(0, 0, 0, 0);
    if (this.IsChartEx)
      return false;
    if (style > 100)
      style -= 100;
    if (style <= 0 || style > 48 /*0x30*/)
      return false;
    System.Drawing.Color mixColor1 = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    System.Drawing.Color mixColor2 = System.Drawing.Color.FromArgb(0, 70, 70, 70);
    int[] array = new int[12]
    {
      1,
      9,
      17,
      25,
      33,
      2,
      10,
      18,
      26,
      34,
      42,
      41
    };
    if ((isLine ? (style != 34 ? 1 : 0) : (Array.IndexOf<int>(array, style) == -1 ? 1 : 0)) != 0)
    {
      if (isLine)
      {
        if (style >= 9 && style <= 16 /*0x10*/)
          color = System.Drawing.Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        else if (style == 33)
        {
          color = this.GetPercentageTintOrShadeOfColor(color, 0.5, mixColor2);
        }
        else
        {
          if (style < 35 || style > 40)
            return false;
          int index = 4 + (style - 35);
          color = chart.GetChartThemeColorByColorIndex(index);
        }
      }
      else if (style >= 3 && style <= 8 || style >= 11 && style <= 16 /*0x10*/ || style >= 19 && style <= 24 || style >= 27 && style <= 32 /*0x20*/ || style >= 35 && style <= 40 || style >= 43 && style <= 48 /*0x30*/)
      {
        int num1 = style;
        int index = 4 + (num1 >= 9 ? (num1 >= 17 ? (num1 >= 25 ? (num1 >= 33 ? (num1 >= 41 ? num1 - 43 : num1 - 35) : num1 - 27) : num1 - 19) : num1 - 11) : num1 - 3);
        color = chart.GetChartThemeColorByColorIndex(index);
        double num2 = 0.0;
        if (highFormattingIndex != 0)
          num2 = 70.0 * ((double) formattingIndex / (double) (highFormattingIndex + 1)) - 35.0;
        double num3 = num2 / 100.0;
        if (num3 != 0.0)
          color = this.GetPercentageTintOrShadeOfColor(color, 1.0 - (num3 < 0.0 ? -1.0 * num3 : num3), num3 < 0.0 ? mixColor2 : mixColor1);
      }
    }
    else
    {
      int num4;
      if (isLine)
      {
        num4 = 3;
      }
      else
      {
        int num5 = Array.IndexOf<int>(array, style);
        if (num5 <= 4)
        {
          num4 = 1;
        }
        else
        {
          if (num5 != 11)
            return false;
          num4 = 4;
        }
      }
      double percentValue = 1.0;
      double num6 = 0.0;
      if (highFormattingIndex > 5)
        num6 = (70.0 * ((double) (formattingIndex / 6) / (double) ((highFormattingIndex + 1) / 6)) - 35.0) / 100.0;
      switch (num4)
      {
        case 1:
          color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
          switch (formattingIndex % 6)
          {
            case 0:
              percentValue = 0.885;
              break;
            case 1:
              percentValue = 0.55;
              break;
            case 2:
              percentValue = 0.78;
              break;
            case 3:
              percentValue = 0.925;
              break;
            case 4:
              percentValue = 0.7;
              break;
            case 5:
              percentValue = 0.3;
              break;
          }
          color = this.GetPercentageTintOrShadeOfColor(color, percentValue, mixColor1);
          break;
        case 2:
        case 3:
          int index = 4 + formattingIndex % 6;
          color = chart.GetChartThemeColorByColorIndex(index);
          if (num4 == 3)
          {
            color = this.GetPercentageTintOrShadeOfColor(color, 0.5, mixColor2);
            break;
          }
          break;
        case 4:
          color = System.Drawing.Color.FromArgb(0, 70, 70, 70);
          switch (formattingIndex % 6)
          {
            case 1:
              percentValue = 0.5;
              break;
            case 2:
              percentValue = 0.55;
              break;
            case 3:
              percentValue = 0.78;
              break;
            case 4:
              percentValue = 0.15;
              break;
            case 5:
              percentValue = 0.7;
              break;
            case 6:
              percentValue = 0.3;
              break;
          }
          color = this.GetPercentageTintOrShadeOfColor(color, percentValue, mixColor1);
          break;
      }
      if (num6 != 0.0)
        color = this.GetPercentageTintOrShadeOfColor(color, 1.0 - (num6 < 0.0 ? num6 * -1.0 : num6), num6 < 0.0 ? mixColor2 : mixColor1);
    }
    return true;
  }
}
