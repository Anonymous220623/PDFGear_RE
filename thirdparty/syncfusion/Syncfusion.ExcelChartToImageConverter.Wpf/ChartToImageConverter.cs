// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ChartToImageConverter
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.Calculate;
using Syncfusion.UI.Xaml.Charts;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

public class ChartToImageConverter : IChartToImageConverter
{
  private IChart _chart;
  private ScalingMode m_scalingMode = ScalingMode.Normal;
  private GetChartSeries converter = new GetChartSeries();

  public ScalingMode ScalingMode
  {
    get => this.m_scalingMode;
    set => this.m_scalingMode = value;
  }

  private void GetChartStream(object stream)
  {
    this.converter.m_isForImage = true;
    if (this.Is3DConversionSupported())
    {
      SfChart3D chart3D = this.GetChart3D(this._chart);
      if (chart3D.Series.Count >= 0)
      {
        chart3D.Save((Stream) new MemoryStream(), (BitmapEncoder) new PngBitmapEncoder());
        this.Save((ChartBase) chart3D, stream);
      }
      this.converter.SecondayAxisAchived = false;
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries3D>) chart3D.Series)
      {
        (chartSeriesBase.ItemsSource as ObservableCollection<ChartPoint>).Clear();
        chartSeriesBase.ItemsSource = (object) null;
      }
    }
    else
    {
      SfChart chart = this.GetChart(this._chart);
      if (chart.Series.Count >= 0)
      {
        chart.Save((Stream) new MemoryStream(), (BitmapEncoder) new PngBitmapEncoder());
        this.Save((ChartBase) chart, stream);
      }
      this.converter.SecondayAxisAchived = false;
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) chart.Series)
      {
        (chartSeriesBase.ItemsSource as ObservableCollection<ChartPoint>).Clear();
        chartSeriesBase.ItemsSource = (object) null;
      }
    }
    this.converter.m_isForImage = false;
    if (Application.Current != null)
      return;
    Dispatcher.CurrentDispatcher.InvokeShutdown();
  }

  private bool Is3DConversionSupported()
  {
    switch (this._chart.ChartType)
    {
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Column_3D:
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Line_3D:
      case ExcelChartType.Pie_3D:
      case ExcelChartType.Pie_Exploded_3D:
      case ExcelChartType.Area_3D:
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cylinder_Clustered_3D:
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Cone_Clustered_3D:
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Stacked:
      case ExcelChartType.Pyramid_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Clustered_3D:
        return true;
      default:
        return false;
    }
  }

  public SfChart GetChart(IChart excelChart)
  {
    bool isPie = false;
    bool isStock = false;
    bool isRadar = false;
    this.converter.IsChart3D = false;
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisBorders = new Dictionary<ChartAxis, Tuple<Border, PointF>>(4);
    Dictionary<ChartAxis, Tuple<Border, PointF>> dictionary = new Dictionary<ChartAxis, Tuple<Border, PointF>>(4);
    this._chart = excelChart;
    this.converter.SetChartSize(this._chart);
    ChartImpl chartImpl = this._chart is ChartImpl ? this._chart as ChartImpl : (this._chart as ChartShapeImpl).ChartObject;
    this.converter.parentWorkbook = chartImpl.ParentWorkbook;
    SfChartExt chart = new SfChartExt();
    this.converter.SfChart = chart;
    bool isCombinationChart = this._chart.ChartType.ToString().Contains("Combination_Chart");
    this.converter.IsChartEx = ChartImpl.IsChartExSerieType(this._chart.ChartType);
    if (this.converter.IsChartEx)
    {
      chart.PrimaryAxis = (ChartAxisBase2D) new CustomCategoryAxis();
    }
    else
    {
      if (Array.IndexOf<ExcelChartType>(ChartImpl.CHARTS_SCATTER, this._chart.ChartType) != -1 || Array.IndexOf<ExcelChartType>(ChartImpl.CHARTS_BUBBLE, this._chart.ChartType) != -1)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomNumericalAxis();
      else if ((this._chart.PrimaryCategoryAxis as ChartCategoryAxisImpl).IsChartBubbleOrScatter)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomNumericalAxis();
      else if (this._chart.PrimaryCategoryAxis.CategoryType == ExcelCategoryType.Time)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomDateTimeAxis();
      else
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomCategoryAxis();
      if (this._chart.PrimaryValueAxis.IsLogScale)
        chart.SecondaryAxis = (RangeAxisBase) new LogarithmicAxis();
      else
        chart.SecondaryAxis = (RangeAxisBase) new CustomNumericalAxis();
      chart.SecondaryAxis.MaximumLabels = 5;
    }
    PropertyInfo property = typeof (ChartAxis).GetProperty("AxisLabelCoefficientRoundOffValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
    if (chart.PrimaryAxis != null)
      property.SetValue((object) chart.PrimaryAxis, (object) 3, new object[0]);
    if (chart.SecondaryAxis != null)
      property.SetValue((object) chart.SecondaryAxis, (object) 3, new object[0]);
    chart.Name = this._chart.ChartType.ToString();
    List<IChartSerie> chartSerieList = new List<IChartSerie>((IEnumerable<IChartSerie>) this._chart.Series);
    if (isCombinationChart)
    {
      chartSerieList = this._chart.Series.OrderByType();
      if (chartSerieList.Count == 0 && this._chart.Series.Count == this._chart.Series.Count<IChartSerie>((Func<IChartSerie, bool>) (x => x.SerieType.ToString().Contains("Bubble"))))
        chartSerieList = this._chart.Series.ToList<IChartSerie>();
    }
    Binding binding1 = (Binding) null;
    Binding binding2 = (Binding) null;
    ChartAxisBase2D chartAxisBase2D = (ChartAxisBase2D) null;
    RangeAxisBase rangeAxisBase = (RangeAxisBase) null;
    foreach (IChartSerie chartSerie in chartSerieList)
    {
      if (!chartSerie.IsFiltered || this.converter.IsChartEx)
      {
        ChartSerieImpl serie = chartSerie as ChartSerieImpl;
        bool isNullSerie = serie.Values == null && serie.EnteredDirectlyValues == null;
        if (isRadar && isCombinationChart)
        {
          if (chartImpl.PrimaryFormats.Count == 1)
          {
            if (chartImpl.SecondaryFormats.Count != 0)
              break;
          }
          else
            break;
        }
        this.GetChartSerie(isCombinationChart ? chartSerie.SerieType : this._chart.ChartType, chart, serie, out isPie, out isStock, out isRadar, isNullSerie);
        if (chart.Series.Count > 0 && !this.converter.IsChart3D && !chartSerie.UsePrimaryAxis)
        {
          if (binding1 != null || binding2 != null)
          {
            if (chart.Series[chart.Series.Count - 1] is RadarSeries)
            {
              chart.Series[chart.Series.Count - 1].SetBinding(PolarRadarSeriesBase.XAxisProperty, (BindingBase) binding1);
              chart.Series[chart.Series.Count - 1].SetBinding(PolarRadarSeriesBase.YAxisProperty, (BindingBase) binding2);
            }
            else
            {
              chart.Series[chart.Series.Count - 1].SetBinding(CartesianSeries.XAxisProperty, (BindingBase) binding1);
              chart.Series[chart.Series.Count - 1].SetBinding(CartesianSeries.YAxisProperty, (BindingBase) binding2);
            }
          }
          else
          {
            if (chart.Series[chart.Series.Count - 1] is RadarSeries)
            {
              RadarSeries radarSeries = chart.Series[chart.Series.Count - 1] as RadarSeries;
              chartAxisBase2D = radarSeries.XAxis;
              rangeAxisBase = radarSeries.YAxis;
            }
            else
            {
              CartesianSeries cartesianSeries = chart.Series[chart.Series.Count - 1] as CartesianSeries;
              chartAxisBase2D = cartesianSeries.XAxis;
              rangeAxisBase = cartesianSeries.YAxis;
            }
            binding1 = new Binding();
            binding2 = new Binding();
            binding1.Source = (object) chart.Series[chart.Series.Count - 1];
            binding1.Path = new PropertyPath("XAxis", new object[0]);
            binding2.Source = (object) chart.Series[chart.Series.Count - 1];
            binding2.Path = new PropertyPath("YAxis", new object[0]);
          }
        }
        if (!isPie)
        {
          if (!isStock)
          {
            if (this.converter.IsChartEx)
              break;
          }
          else
            break;
        }
        else
          break;
      }
    }
    Border textBlock = (Border) null;
    RectangleF manualRect1 = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if ((chartImpl.HasTitle ? 1 : (this.IsAutoChartTitle(chartImpl) ? 1 : 0)) != 0)
      textBlock = this.converter.SfChartTitle((IChart) chartImpl, out manualRect1);
    RectangleF manualRect2;
    bool isInnerLayoutTarget;
    if (isRadar)
    {
      manualRect2 = new RectangleF(-1f, -1f, 0.0f, 0.0f);
      isInnerLayoutTarget = false;
    }
    else if (chart.Series.Count > 0 && chart.Series[chart.Series.Count - 1] is CircularSeriesBase)
    {
      if ((bool) chart.Series[chart.Series.Count - 1].GetValue(AccumulationSeriesBase.ExplodeAllProperty) && (double) chart.Series[chart.Series.Count - 1].GetValue(CircularSeriesBase.ExplodeRadiusProperty) > 20.0)
      {
        manualRect2 = new RectangleF(-1f, -1f, 0.0f, 0.0f);
        isInnerLayoutTarget = false;
      }
      else
        manualRect2 = this.TryAndSetPlotAreaSize(chartImpl, out isInnerLayoutTarget);
    }
    else
      manualRect2 = this.TryAndSetPlotAreaSize(chartImpl, out isInnerLayoutTarget);
    if (this._chart.Series.Count > 0)
    {
      if (isRadar && isCombinationChart && this.converter.SecondayAxisAchived)
      {
        chart.SecondaryAxis = (RangeAxisBase) null;
        chart.PrimaryAxis = (ChartAxisBase2D) null;
      }
      else if (!isPie)
      {
        ChartCategoryAxisImpl primaryCategoryAxis = this._chart.PrimaryCategoryAxis as ChartCategoryAxisImpl;
        ChartValueAxisImpl primaryValueAxis = this._chart.PrimaryValueAxis as ChartValueAxisImpl;
        if (!this.converter.IsChartEx && primaryValueAxis.IsLogScale)
          this.converter.SfLogerthmicAxis(chart.SecondaryAxis as LogarithmicAxis, primaryValueAxis, primaryCategoryAxis);
        else if (this._chart.ChartType == ExcelChartType.Funnel)
        {
          System.Windows.Media.Brush brush = (System.Windows.Media.Brush) null;
          Style emptyLineStyle = this.GetEmptyLineStyle(out brush);
          SfChartExt sfChartExt = chart;
          CustomNumericalAxis customNumericalAxis1 = new CustomNumericalAxis();
          customNumericalAxis1.Minimum = new double?(0.0);
          customNumericalAxis1.Maximum = new double?(1.0);
          customNumericalAxis1.AxisLineStyle = emptyLineStyle;
          customNumericalAxis1.ShowGridLines = false;
          customNumericalAxis1.TickLineSize = 0.0;
          customNumericalAxis1.Foreground = brush;
          customNumericalAxis1.FontSize = 3.0;
          CustomNumericalAxis customNumericalAxis2 = customNumericalAxis1;
          sfChartExt.SecondaryAxis = (RangeAxisBase) customNumericalAxis2;
          chart.PrimaryAxis.IsInversed = true;
        }
        else
        {
          if (this.converter.IsChartEx)
            chart.SecondaryAxis = (RangeAxisBase) new CustomNumericalAxis();
          this.converter.SfNumericalAxis(chart.SecondaryAxis as CustomNumericalAxis, primaryValueAxis, (ChartValueAxisImpl) primaryCategoryAxis, true);
        }
        CustomCategoryAxis primaryAxis1 = chart.PrimaryAxis as CustomCategoryAxis;
        CustomDateTimeAxis primaryAxis2 = chart.PrimaryAxis as CustomDateTimeAxis;
        if (primaryAxis1 != null)
          this.converter.SfCategoryAxis(primaryAxis1, primaryCategoryAxis, primaryValueAxis, true);
        else if (primaryAxis2 != null)
        {
          if (this.converter.FirstSeriesPoints != null && this.converter.FirstSeriesPoints.Count > 0 && this.converter.FirstSeriesPoints[0].X is DateTime)
          {
            this.converter.IsDateTimeCategoryAxis = true;
            this.converter.ItemSource = this.converter.FirstSeriesPoints;
            this.converter.SfDateTimeAxis(primaryAxis2, primaryCategoryAxis, primaryValueAxis, true);
          }
          else
          {
            chart.PrimaryAxis = (ChartAxisBase2D) new CustomCategoryAxis();
            this.converter.SfCategoryAxis(chart.PrimaryAxis as CustomCategoryAxis, primaryCategoryAxis, primaryValueAxis, true);
          }
        }
        else
          this.converter.SfNumericalAxis(chart.PrimaryAxis as CustomNumericalAxis, (ChartValueAxisImpl) primaryCategoryAxis, primaryValueAxis, true);
        this.converter.TryAndSetChartAxisTitle((ChartAxis) chart.PrimaryAxis, (ChartValueAxisImpl) primaryCategoryAxis, axisBorders, dictionary, (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0 && (primaryValueAxis.IsAutoCross || primaryValueAxis.IsMaxCross), manualRect2, isRadar);
        this.converter.TryAndSetChartAxisTitle((ChartAxis) chart.SecondaryAxis, primaryValueAxis, axisBorders, dictionary, (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0 && (primaryCategoryAxis.IsAutoCross || primaryCategoryAxis.IsMaxCross), manualRect2, isRadar);
      }
    }
    else
    {
      chart.PrimaryAxis.Visibility = Visibility.Collapsed;
      chart.SecondaryAxis.Visibility = Visibility.Collapsed;
      chart.PrimaryAxis.ShowGridLines = false;
      chart.SecondaryAxis.ShowGridLines = false;
    }
    if (this.converter.SecondayAxisAchived)
    {
      if (chartAxisBase2D != null && chartAxisBase2D.Visibility == Visibility.Visible)
      {
        ChartValueAxisImpl secondaryCategoryAxis = excelChart.SecondaryCategoryAxis as ChartValueAxisImpl;
        ChartValueAxisImpl secondaryValueAxis = excelChart.SecondaryValueAxis as ChartValueAxisImpl;
        this.converter.TryAndSetChartAxisTitle((ChartAxis) chartAxisBase2D, secondaryCategoryAxis, axisBorders, dictionary, (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0 && (secondaryValueAxis.IsAutoCross || secondaryValueAxis.IsMaxCross), manualRect2, isRadar);
      }
      if (rangeAxisBase != null && rangeAxisBase.Visibility == Visibility.Visible)
      {
        ChartValueAxisImpl secondaryCategoryAxis = excelChart.SecondaryCategoryAxis as ChartValueAxisImpl;
        ChartValueAxisImpl secondaryValueAxis = excelChart.SecondaryValueAxis as ChartValueAxisImpl;
        this.converter.TryAndSetChartAxisTitle((ChartAxis) rangeAxisBase, secondaryValueAxis, axisBorders, dictionary, (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0 && (secondaryCategoryAxis.IsAutoCross || secondaryCategoryAxis.IsMaxCross), manualRect2, isRadar);
      }
      this.TryCloneSecondaryAxes((SfChart) chart, (ChartAxis) chartAxisBase2D, (ChartAxis) rangeAxisBase);
    }
    if ((double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0 && this.converter.m_TextToMeasure != null)
    {
      this.converter.m_TextToMeasure.Clear();
      this.converter.m_TextToMeasure = (Dictionary<ChartAxis, Tuple<string, double, bool, bool>>) null;
    }
    int[] sortedLegendOrders = (int[]) null;
    ChartLegend emptyLegend = (ChartLegend) null;
    ChartLegend legend;
    if (this._chart.HasLegend)
    {
      legend = this.converter.SfLegend((ChartBase) chart, chartImpl, out sortedLegendOrders, (double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0, out emptyLegend);
    }
    else
    {
      ChartLegend chartLegend = new ChartLegend();
      chartLegend.Visibility = Visibility.Hidden;
      legend = chartLegend;
      legend.DockPosition = ChartDock.Floating;
    }
    chart.Legend = (object) legend;
    if (this._chart.HasPlotArea && this._chart.Series.Count > 0)
      this.converter.SfPloatArea(chart, this._chart.PlotArea as ChartFrameFormatImpl, chartImpl);
    else if (this.converter.IsChartEx && !this._chart.HasPlotArea)
      chart.AreaBorderThickness = new Thickness(0.0);
    this.converter.SfChartArea(chart, this._chart.ChartArea as ChartFrameFormatImpl, chartImpl);
    if (isRadar)
      this.converter.DetachEventsForRadarAxes(chart);
    chart.Width = (double) this.converter.ChartWidth;
    chart.Height = (double) this.converter.ChartHeight;
    bool overlay = (this._chart.ChartTitleArea as ChartTextAreaImpl).Overlay;
    if (textBlock != null)
    {
      if ((double) manualRect1.X >= 0.0 || (double) manualRect1.Y >= 0.0)
      {
        if (!overlay && ((double) manualRect2.X < 0.0 || (double) manualRect2.Y < 0.0))
        {
          textBlock.Measure(new System.Windows.Size((double) this.converter.ChartWidth, double.MaxValue));
          Border border = new Border();
          border.Height = textBlock.DesiredSize.Height + 2.0;
          chart.Header = (object) border;
          if (legend.Visibility == Visibility.Visible && legend.DockPosition == ChartDock.Bottom && legend.LegendPosition == LegendPosition.Outside)
            legend.LegendPosition = LegendPosition.Inside;
        }
      }
      else if (!overlay)
      {
        chart.Header = (object) textBlock;
        if ((double) manualRect2.X >= 0.0 && (double) manualRect2.Y >= 0.0)
        {
          textBlock.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
          manualRect2.Y -= (float) textBlock.DesiredSize.Height;
          if ((double) manualRect2.Y < 0.0)
            manualRect2.Y = 0.0f;
          manualRect2.Height += (float) textBlock.DesiredSize.Height;
          if ((double) manualRect2.Height + (double) manualRect2.Y > (double) this.converter.ChartHeight)
            manualRect2.Height = (float) this.converter.ChartHeight - manualRect2.Y;
        }
      }
    }
    chart.Measure(new System.Windows.Size((double) this.converter.ChartWidth, (double) this.converter.ChartHeight));
    if (this.converter.TrendlineFormulas != null && this.converter.TrendlineFormulas.Count > 0)
    {
      CalcEngine calcEngine = (CalcEngine) null;
      if (this.converter.parentWorkbook != null)
      {
        if (this.converter.parentWorkbook.Worksheets[0].CalcEngine == null)
          this.converter.parentWorkbook.Worksheets[0].EnableSheetCalculations();
        calcEngine = this.converter.parentWorkbook.Worksheets[0].CalcEngine;
      }
      foreach (KeyValuePair<Trendline, TrendLineBorder> trendlineFormula in this.converter.TrendlineFormulas)
        chart.SetTrendLineLabels(trendlineFormula.Key, trendlineFormula.Value, calcEngine, this.converter);
    }
    bool isChanged = false;
    this.converter.TryAndSetPlotAreaMargins(this._chart, chart, legend, manualRect2, isInnerLayoutTarget, isPie || isRadar);
    if (axisBorders.Count > 0)
      chart.TrySetManualAxisTextElements(axisBorders, false);
    if (dictionary.Count > 0)
      chart.TrySetManualAxisTextElements(dictionary, true);
    if (textBlock != null && ((double) manualRect1.X >= 0.0 && (double) manualRect1.Y >= 0.0 || overlay))
      chart.AddTextBlockInCustomCanvas(textBlock, manualRect1, overlay, (double) manualRect2.X < 0.0 || (double) manualRect2.Y < 0.0);
    this.converter.TryAndSortLegendItems((ChartBase) chart, sortedLegendOrders);
    if (this.converter.IsChartEx)
      chart.PrimaryAxis.ShowAxisNextToOrigin = false;
    else if (chart.PrimaryAxis is CategoryAxis)
    {
      this.SetAxisReplacement(chart, this._chart, out isChanged);
      if (!isChanged)
        chart.PrimaryAxis.ShowAxisNextToOrigin = true;
    }
    if (chart.Series.Count != 0 && chart.Series.Count<ChartSeries>((Func<ChartSeries, bool>) (x => x is BubbleSeries)) == chart.Series.Count)
      this.SetBubbleSizeInCharts(chart, chartImpl, isCombinationChart);
    if (!this.converter.IsChartEx && this.IsAdornmentsHidden((ChartBase) chart))
      chart.AddHandlerLayoutUpdate(true);
    chart.Arrange(new Rect(0.0, 0.0, chart.Width, chart.Height));
    this.ResetChartCommonData();
    return (SfChart) chart;
  }

  private void SetBubbleSizeInCharts(
    SfChartExt sfChart,
    ChartImpl chartImpl,
    bool isCombinationChart)
  {
    double val1 = Math.Ceiling(sfChart.PlotAreaSize.Width);
    double val2 = Math.Ceiling(sfChart.PlotAreaSize.Height);
    double expectedSize = Math.Pow((val2 + val1) / 2.0, 2.0) / (2.0 * Math.Max(val1, val2));
    this.SetMaxMinRadiusForBubbles(sfChart, chartImpl, true, expectedSize);
    if (!isCombinationChart)
      return;
    this.SetMaxMinRadiusForBubbles(sfChart, chartImpl, false, expectedSize);
  }

  private void SetMaxMinRadiusForBubbles(
    SfChartExt sfChart,
    ChartImpl chartImpl,
    bool isPrimary,
    double expectedSize)
  {
    int num1 = 100;
    bool flag = false;
    for (int index = 0; index < chartImpl.Series.Count; ++index)
    {
      if (chartImpl.Series[index].UsePrimaryAxis == isPrimary)
      {
        num1 = chartImpl.Series[index].SerieFormat.CommonSerieOptions.BubbleScale;
        if (num1 > 300)
          num1 = 300;
        flag = chartImpl.Series[index].SerieFormat.CommonSerieOptions.SizeRepresents == ExcelBubbleSize.Width;
        break;
      }
    }
    expectedSize = num1 > 100 ? (num1 > 200 ? expectedSize * 0.5 * ((double) num1 / 300.0) : expectedSize * 0.375 * ((double) num1 / 200.0)) : expectedSize * 0.25 * ((double) num1 / 100.0);
    IEnumerable<\u003C\u003Ef__AnonymousType2<double, double, BubbleSeries>> source = sfChart.Series.Select(serie => new
    {
      serie = serie,
      bSerie = serie as BubbleSeries
    }).Select(_param0 => new
    {
      \u003C\u003Eh__TransparentIdentifier6 = _param0,
      items = _param0.serie.ItemsSource as ObservableCollection<ChartPoint>
    }).Where(_param1 =>
    {
      if (_param1.items.Count <= 0)
        return false;
      return _param1.\u003C\u003Eh__TransparentIdentifier6.bSerie.YAxis != null ? !isPrimary : isPrimary;
    }).Select(_param0 => new
    {
      MaxSz = Convert.ToDouble(_param0.items.Max<ChartPoint, object>((Func<ChartPoint, object>) (x => x.Size))),
      MinSz = Convert.ToDouble(_param0.items.Min<ChartPoint, object>((Func<ChartPoint, object>) (x => x.Size))),
      Serie = _param0.\u003C\u003Eh__TransparentIdentifier6.bSerie
    });
    double num2 = source.Max(x => x.MaxSz);
    double num3 = source.Min(x => x.MinSz);
    List<double> doubleList = new List<double>(source.Count());
    foreach (var data in source)
      doubleList.Add(data.MaxSz / num2);
    double num4 = num3 / (8.0 * num2);
    double num5 = num2 / num2;
    double num6 = 0.0;
    double num7;
    if (!flag)
    {
      num6 = Math.PI * Math.Pow(expectedSize, 2.0);
      num7 = Math.Sqrt(num6 * num4 / Math.PI);
    }
    else
      num7 = expectedSize * num4;
    int index1 = 0;
    foreach (var data in source)
    {
      if (index1 >= doubleList.Count)
        break;
      BubbleSeries serie = data.Serie;
      if (flag)
      {
        serie.MaximumRadius = expectedSize * doubleList[index1];
        serie.MinimumRadius = num7;
      }
      else
      {
        serie.MaximumRadius = Math.Sqrt(num6 * doubleList[index1] / Math.PI);
        serie.MinimumRadius = num7;
      }
      ++index1;
    }
  }

  private bool IsAdornmentsHidden(ChartBase sfChart)
  {
    if (sfChart.VisibleSeries.Count == 0 || sfChart.VisibleSeries.Select(series => new
    {
      series = series,
      items = series.ItemsSource as ObservableCollection<ChartPoint>
    }).Select(_param0 => new{ count = _param0.items.Count }).Max(x => x.count) > 1000)
      return false;
    if (sfChart is SfChart3DExt sfChart3Dext && sfChart3Dext.Series.Count<ChartSeries3D>((Func<ChartSeries3D, bool>) (x => x.AdornmentsInfo != null && x.AdornmentsInfo.ShowLabel && x.Adornments != null)) > 0)
      return sfChart3Dext.Rotation > 10.0 || sfChart3Dext.Tilt > 10.0;
    string name1 = sfChart.VisibleSeries[0].GetType().Name;
    if ((name1.Contains("StackingColumn") || name1.Contains("StackingBar")) && sfChart.VisibleSeries[0].Adornments != null && sfChart.VisibleSeries[0] is AdornmentSeries adornmentSeries1 && adornmentSeries1.AdornmentsInfo != null && adornmentSeries1.AdornmentsInfo.ShowLabel)
      return true;
    if (sfChart.VisibleSeries.Count < 2)
      return false;
    int num1 = 0;
    string[] source1 = new string[4]
    {
      "ColumnSeries",
      "BarSeries",
      "FastBarBitmapSeries",
      "FastColumnBitmapSeries"
    };
    string[] source2 = new string[7]
    {
      "LineSeries",
      "FastLineSeries",
      "SplineSeries",
      "ScatterSeries",
      "FastScatterBitmapSeries",
      "RadarSeries",
      "AreaSeries"
    };
    int num2 = 0;
    int num3 = 0;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) sfChart.VisibleSeries)
    {
      string name2 = chartSeriesBase.GetType().Name;
      if (chartSeriesBase.Adornments != null && chartSeriesBase is AdornmentSeries adornmentSeries2 && adornmentSeries2.AdornmentsInfo != null && adornmentSeries2.AdornmentsInfo.ShowLabel)
      {
        if (name2.Contains("StackingColumn") || name2.Contains("StackingBar"))
          return true;
        ++num1;
      }
      if (((IEnumerable<string>) source1).Contains<string>(name2))
        ++num2;
      else if (((IEnumerable<string>) source2).Contains<string>(name2))
        ++num3;
    }
    return num1 >= 1 && num3 != 0 && num2 != sfChart.VisibleSeries.Count && (num3 > 1 || num3 == 1 && num2 > 0);
  }

  private bool IsAutoChartTitle(ChartImpl chartImpl)
  {
    return chartImpl.IsTitleAreaInitialized && (chartImpl.ChartTitleArea as ChartTextAreaImpl).TextRecord.IsAutoText;
  }

  private RectangleF TryAndSetPlotAreaSize(ChartImpl chartImpl, out bool isInnerLayoutTarget)
  {
    isInnerLayoutTarget = false;
    RectangleF rectangleF = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if (chartImpl.Series.Count > 0)
    {
      bool flag = false;
      if (!chartImpl.HasPlotArea && chartImpl.PlotAreaLayout != null && chartImpl.PlotAreaLayout.X > 0.0 && chartImpl.PlotAreaLayout.Y > 0.0 && chartImpl.PlotAreaLayout.WXMode != LayoutModes.auto && chartImpl.PlotAreaLayout.WYMode != LayoutModes.auto)
      {
        chartImpl.HasPlotArea = true;
        ChartManualLayoutImpl manualLayout = (this._chart.PlotArea.Layout as ChartLayoutImpl).ManualLayout as ChartManualLayoutImpl;
        manualLayout.FlagOptions = (byte) 3;
        if (chartImpl.PlotAreaLayout.Dx > 0.0 && chartImpl.PlotAreaLayout.Dy > 0.0)
          manualLayout.FlagOptions = (byte) 31 /*0x1F*/;
        manualLayout.PlotAreaLayout = chartImpl.PlotAreaLayout;
        flag = true;
      }
      if (chartImpl.HasPlotArea)
      {
        ChartLayoutImpl layout = this._chart.PlotArea.Layout as ChartLayoutImpl;
        if (layout.IsManualLayout)
        {
          ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
          if (manualLayout.FlagOptions != (byte) 0 && manualLayout.FlagOptions != (byte) 16 /*0x10*/ && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
            rectangleF = this.converter.CalculateManualLayout(manualLayout, out isInnerLayoutTarget, false);
        }
      }
      if (flag)
        chartImpl.HasPlotArea = false;
    }
    return rectangleF;
  }

  private void TryCloneSecondaryAxes(SfChart sfChart, ChartAxis xAxis, ChartAxis yAxis)
  {
    if (xAxis != null && xAxis.Visibility != Visibility.Visible && sfChart.PrimaryAxis != null && sfChart.PrimaryAxis.Visibility == Visibility.Visible && xAxis is CustomNumericalAxis customNumericalAxis1 && sfChart.PrimaryAxis is CustomNumericalAxis)
    {
      CustomNumericalAxis primaryAxis = sfChart.PrimaryAxis as CustomNumericalAxis;
      if (primaryAxis.Minimum.HasValue && !customNumericalAxis1.Minimum.HasValue)
        customNumericalAxis1.Minimum = primaryAxis.Minimum;
      if (primaryAxis.Maximum.HasValue && !customNumericalAxis1.Maximum.HasValue)
        customNumericalAxis1.Maximum = primaryAxis.Maximum;
    }
    if (yAxis == null || yAxis.Visibility == Visibility.Visible || sfChart.SecondaryAxis == null || sfChart.SecondaryAxis.Visibility != Visibility.Visible || !(yAxis is CustomNumericalAxis customNumericalAxis2) || !(sfChart.SecondaryAxis is CustomNumericalAxis))
      return;
    CustomNumericalAxis secondaryAxis = sfChart.SecondaryAxis as CustomNumericalAxis;
    if (secondaryAxis.Minimum.HasValue && !customNumericalAxis2.Minimum.HasValue)
      customNumericalAxis2.Minimum = secondaryAxis.Minimum;
    if (!secondaryAxis.Maximum.HasValue || customNumericalAxis2.Maximum.HasValue)
      return;
    customNumericalAxis2.Maximum = secondaryAxis.Maximum;
  }

  private void ResetChartCommonData()
  {
    this.converter.SortedIndexes = (int[]) null;
    this.converter.FirstSeriesPoints = (ObservableCollection<ChartPoint>) null;
    this.converter.IsChart3D = false;
    this.converter.SecondayAxisAchived = false;
    this.converter.FontName = (string) null;
    this.converter.DateTimeIntervalType = DateTimeIntervalType.Auto;
    this.converter.parentWorkbook = (WorkbookImpl) null;
    this.converter.IsLegendManualLayout = false;
    this.converter.IsChartEx = false;
    this.converter.AxisTitleRotationAngle = 90;
    this.converter.m_TextToMeasure = (Dictionary<ChartAxis, Tuple<string, double, bool, bool>>) null;
    this.converter.SfChart = (SfChartExt) null;
    this.converter.ItemSource = (ObservableCollection<ChartPoint>) null;
    this.converter.HasNewSeries = false;
    this.converter.NewSeriesIndex = 0;
    this.converter.IsCategoryAxisLine = true;
    this.converter.IsCategoryLabel = true;
    this.converter.IsDateTimeCategoryAxis = false;
    if (this.converter.engine != null)
    {
      this.converter.engine.Dispose();
      this.converter.engine = (ExcelEngine) null;
    }
    if (this.converter.TrendlineFormulas == null)
      return;
    this.converter.TrendlineFormulas.Clear();
    this.converter.TrendlineFormulas = (Dictionary<Trendline, TrendLineBorder>) null;
  }

  private Style GetEmptyLineStyle(out System.Windows.Media.Brush brush)
  {
    Style emptyLineStyle = new Style()
    {
      TargetType = typeof (Line)
    };
    brush = (System.Windows.Media.Brush) null;
    brush = (System.Windows.Media.Brush) new SolidColorBrush(this.converter.SfColor(System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
    Setter setter1 = new Setter()
    {
      Property = Shape.StrokeProperty,
      Value = (object) brush
    };
    emptyLineStyle.Setters.Add((SetterBase) setter1);
    Setter setter2 = new Setter()
    {
      Property = Shape.StrokeThicknessProperty,
      Value = (object) 1.0
    };
    emptyLineStyle.Setters.Add((SetterBase) setter2);
    return emptyLineStyle;
  }

  private void SetAxisReplacement(SfChartExt sfChart, IChart chart, out bool isChanged)
  {
    isChanged = true;
    if (sfChart.SecondaryAxis.VisibleLabels.Count < 2 || sfChart.PrimaryAxis.VisibleLabels.Count < 2)
      return;
    ObservableCollection<ChartAxisLabel> visibleLabels1 = sfChart.PrimaryAxis.VisibleLabels;
    ObservableCollection<ChartAxisLabel> visibleLabels2 = sfChart.SecondaryAxis.VisibleLabels;
    double origin = sfChart.PrimaryAxis.Origin;
    int index1 = sfChart.SecondaryAxis.IsInversed ? visibleLabels2.Count - 1 : 0;
    int index2 = sfChart.SecondaryAxis.IsInversed ? 0 : visibleLabels2.Count - 1;
    double position1 = visibleLabels2[index1].Position;
    double position2 = visibleLabels2[index2].Position;
    bool flag1 = position1 < 0.0;
    bool flag2 = origin > position1;
    if (sfChart.PrimaryAxis.Visibility != Visibility.Visible || !flag1 && !flag2 || sfChart.PrimaryAxis.OpposedPosition || chart.PrimaryCategoryAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_Low || chart.PrimaryValueAxis.IsMaxCross || !chart.PrimaryValueAxis.IsAutoCross && origin == position1)
      return;
    Style style = new Style() { TargetType = typeof (Line) };
    System.Windows.Media.Brush axisLineColor = (System.Windows.Media.Brush) new SolidColorBrush();
    double thickness = 0.0;
    isChanged = true;
    double num1 = origin != position2 ? 0.0 : (visibleLabels2[1].Position - visibleLabels2[0].Position) / 100.0;
    sfChart.PrimaryAxis.ShowAxisNextToOrigin = false;
    LineAnnotation lineAnnotation = new LineAnnotation();
    lineAnnotation.Y1 = lineAnnotation.Y2 = (object) (origin - num1);
    lineAnnotation.X1 = (object) ((sfChart.PrimaryAxis.IsInversed ? visibleLabels1[visibleLabels1.Count - 1].Position : visibleLabels1[0].Position) - 0.5);
    lineAnnotation.X2 = (object) ((sfChart.PrimaryAxis.IsInversed ? visibleLabels1[0].Position : visibleLabels1[visibleLabels1.Count - 1].Position) + 0.495);
    if (sfChart.PrimaryAxis.AxisLineStyle.Setters.Count > 0)
    {
      axisLineColor = lineAnnotation.Stroke = (System.Windows.Media.Brush) (sfChart.PrimaryAxis.AxisLineStyle.Setters[0] as Setter).Value;
      thickness = lineAnnotation.StrokeThickness = (double) (sfChart.PrimaryAxis.AxisLineStyle.Setters[1] as Setter).Value;
    }
    sfChart.Annotations.Add((Annotation) lineAnnotation);
    sfChart.PrimaryAxis.AxisLineStyle = style;
    sfChart.PrimaryAxis.MajorTickLineStyle = style;
    sfChart.PrimaryAxis.MinorTickLineStyle = style;
    if (chart.PrimaryCategoryAxis.MajorTickMark != ExcelTickMark.TickMark_None)
    {
      double num2 = (visibleLabels2[1].Position - visibleLabels2[0].Position) / 5.0;
      double topValue = origin + (chart.PrimaryCategoryAxis.MajorTickMark == ExcelTickMark.TickMark_Outside ? 0.0 : num2) - num1;
      double bottomValue = origin + (chart.PrimaryCategoryAxis.MajorTickMark == ExcelTickMark.TickMark_Inside ? 0.0 : -num2) - num1;
      int interval = chart.PrimaryCategoryAxis.TickMarkSpacing <= 0 ? 1 : chart.PrimaryCategoryAxis.TickMarkSpacing;
      this.AddTickMarks(sfChart, topValue, bottomValue, interval, axisLineColor, thickness, true);
    }
    if (chart.PrimaryCategoryAxis.MinorTickMark == ExcelTickMark.TickMark_None)
      return;
    double num3 = (visibleLabels2[1].Position - visibleLabels2[0].Position) / 7.0;
    double topValue1 = origin + (chart.PrimaryCategoryAxis.MinorTickMark == ExcelTickMark.TickMark_Outside ? 0.0 : num3) - num1;
    double bottomValue1 = origin + (chart.PrimaryCategoryAxis.MinorTickMark == ExcelTickMark.TickMark_Inside ? 0.0 : -num3) - num1;
    int interval1 = chart.PrimaryCategoryAxis.TickMarkSpacing <= 0 ? 1 : chart.PrimaryCategoryAxis.TickMarkSpacing;
    this.AddTickMarks(sfChart, topValue1, bottomValue1, interval1, axisLineColor, thickness, false);
  }

  private void AddTickMarks(
    SfChartExt sfChart,
    double topValue,
    double bottomValue,
    int interval,
    System.Windows.Media.Brush axisLineColor,
    double thickness,
    bool isMajor)
  {
    ObservableCollection<ChartAxisLabel> visibleLabels = sfChart.PrimaryAxis.VisibleLabels;
    for (int index = 0; index < visibleLabels.Count; index += interval)
    {
      if (index < visibleLabels.Count)
      {
        LineAnnotation lineAnnotation = new LineAnnotation();
        lineAnnotation.Y1 = (object) topValue;
        lineAnnotation.Y2 = (object) bottomValue;
        double num = isMajor ? (index == 0 ? -0.497 : -0.51) : (double) (interval - 1) * 0.493;
        lineAnnotation.X1 = lineAnnotation.X2 = (object) (visibleLabels[index].Position + num);
        lineAnnotation.ShowLine = true;
        lineAnnotation.Stroke = axisLineColor;
        lineAnnotation.StrokeThickness = thickness;
        sfChart.Annotations.Add((Annotation) lineAnnotation);
      }
    }
  }

  public SfChart3D GetChart3D(IChart excelChart)
  {
    bool isPie = false;
    bool isStock = false;
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisBorders = new Dictionary<ChartAxis, Tuple<Border, PointF>>(4);
    Dictionary<ChartAxis, Tuple<Border, PointF>> dictionary = new Dictionary<ChartAxis, Tuple<Border, PointF>>(4);
    this.converter.IsChart3D = true;
    this._chart = excelChart;
    this.converter.SetChartSize(this._chart);
    ChartImpl chartImpl = this._chart is ChartImpl ? this._chart as ChartImpl : (this._chart as ChartShapeImpl).ChartObject;
    this.converter.parentWorkbook = chartImpl.ParentWorkbook;
    SfChart3DExt chart3D = new SfChart3DExt();
    chart3D.WallSize = 1.0;
    if (this._chart.ChartType.ToString().Contains("Column_3D"))
      chart3D.SideBySideSeriesPlacement = false;
    if (this._chart.PrimaryCategoryAxis.CategoryType == ExcelCategoryType.Time)
      chart3D.PrimaryAxis = (ChartAxisBase3D) new DateTimeAxis3D();
    else
      chart3D.PrimaryAxis = (ChartAxisBase3D) new CategoryAxis3D();
    if (this._chart.PrimaryValueAxis.IsLogScale)
      chart3D.SecondaryAxis = (RangeAxisBase3D) new LogarithmicAxis3D();
    else
      chart3D.SecondaryAxis = (RangeAxisBase3D) new CustomNumericalAxis3D();
    if (Array.IndexOf<ExcelChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, this._chart.ChartType) >= 0)
      this.converter.SfWall((SfChart3D) chart3D, chartImpl);
    this.converter.SfRotation3D(chartImpl, (SfChart3D) chart3D);
    chart3D.SecondaryAxis.MaximumLabels = 5;
    chart3D.Name = this._chart.ChartType.ToString();
    PropertyInfo property = typeof (ChartAxis).GetProperty("AxisLabelCoefficientRoundOffValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
    if (chart3D.PrimaryAxis != null)
      property.SetValue((object) chart3D.PrimaryAxis, (object) 3, new object[0]);
    if (chart3D.SecondaryAxis != null)
      property.SetValue((object) chart3D.SecondaryAxis, (object) 3, new object[0]);
    foreach (IChartSerie chartSerie in new List<IChartSerie>((IEnumerable<IChartSerie>) this._chart.Series))
    {
      if (!chartSerie.IsFiltered)
      {
        ChartSerieImpl serie = chartSerie as ChartSerieImpl;
        bool isNullSerie = serie.Values == null && serie.EnteredDirectlyValues == null;
        this.GetChartSerie3D(this._chart.ChartType, (SfChart3D) chart3D, serie, out isPie, out isStock, isNullSerie);
        if (!isPie)
        {
          if (isStock)
            break;
        }
        else
          break;
      }
    }
    Border textBlock = (Border) null;
    RectangleF manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    if ((chartImpl.HasTitle ? 1 : (this.IsAutoChartTitle(chartImpl) ? 1 : 0)) != 0)
      textBlock = this.converter.SfChartTitle((IChart) chartImpl, out manualRect);
    bool overlay = (this._chart.ChartTitleArea as ChartTextAreaImpl).Overlay;
    if (textBlock != null && ((double) manualRect.X < 0.0 || (double) manualRect.Y < 0.0) && !overlay)
      chart3D.Header = (object) textBlock;
    if (this._chart.Series.Count > 0 && !isPie)
    {
      ChartValueAxisImpl primaryValueAxis = this._chart.PrimaryValueAxis as ChartValueAxisImpl;
      ChartCategoryAxisImpl primaryCategoryAxis = this._chart.PrimaryCategoryAxis as ChartCategoryAxisImpl;
      if (this._chart.PrimaryValueAxis.IsLogScale)
        this.converter.SfLogerthmicAxis3D(chart3D.SecondaryAxis as LogarithmicAxis3D, primaryValueAxis, primaryCategoryAxis);
      else
        this.converter.SfNumericalAxis3D(chart3D.SecondaryAxis as CustomNumericalAxis3D, primaryValueAxis, primaryCategoryAxis);
      if (chart3D.PrimaryAxis is CategoryAxis3D)
        this.converter.SfCategoryAxis3D(chart3D.PrimaryAxis as CategoryAxis3D, primaryCategoryAxis, primaryValueAxis);
      else if (chart3D.PrimaryAxis is DateTimeAxis3D)
      {
        if (this.converter.FirstSeriesPoints != null && this.converter.FirstSeriesPoints.Count > 0 && this.converter.FirstSeriesPoints[0].X is DateTime)
        {
          this.converter.SfDateTimeAxis3D(chart3D.PrimaryAxis as DateTimeAxis3D, primaryCategoryAxis, primaryValueAxis);
        }
        else
        {
          chart3D.PrimaryAxis = (ChartAxisBase3D) new CategoryAxis3D();
          this.converter.SfCategoryAxis3D(chart3D.PrimaryAxis as CategoryAxis3D, primaryCategoryAxis, primaryValueAxis);
        }
      }
      else
        this.converter.SfNumericalAxis3D(chart3D.PrimaryAxis as CustomNumericalAxis3D, (ChartValueAxisImpl) primaryCategoryAxis, primaryCategoryAxis);
      this.converter.TryAndSetChartAxisTitle((ChartAxis) chart3D.PrimaryAxis, (ChartValueAxisImpl) primaryCategoryAxis, axisBorders, dictionary, false, new RectangleF(-1f, -1f, 0.0f, 0.0f), false);
      this.converter.TryAndSetChartAxisTitle((ChartAxis) chart3D.SecondaryAxis, primaryValueAxis, axisBorders, dictionary, false, new RectangleF(-1f, -1f, 0.0f, 0.0f), false);
    }
    else
    {
      chart3D.PrimaryAxis.Visibility = Visibility.Collapsed;
      chart3D.SecondaryAxis.Visibility = Visibility.Collapsed;
      chart3D.PrimaryAxis.ShowGridLines = false;
      chart3D.SecondaryAxis.ShowGridLines = false;
    }
    int[] sortedLegendOrders = (int[]) null;
    ChartLegend chartLegend1;
    if (this._chart.HasLegend)
    {
      chartLegend1 = this.converter.SfLegend((ChartBase) chart3D, chartImpl, out sortedLegendOrders, false, out ChartLegend _);
    }
    else
    {
      ChartLegend chartLegend2 = new ChartLegend();
      chartLegend2.Visibility = Visibility.Hidden;
      chartLegend1 = chartLegend2;
    }
    chart3D.Legend = (object) chartLegend1;
    this.converter.SfChartArea3D(chart3D, this._chart.ChartArea as ChartFrameFormatImpl, chartImpl);
    chart3D.Width = (double) this.converter.ChartWidth;
    chart3D.Height = (double) this.converter.ChartHeight;
    chart3D.Measure(new System.Windows.Size((double) this.converter.ChartWidth, (double) this.converter.ChartHeight));
    if (axisBorders.Count > 0)
      chart3D.TrySetManualAxisTextElements(axisBorders, false);
    if (dictionary.Count > 0)
      chart3D.TrySetManualAxisTextElements(dictionary, true);
    if (textBlock != null && ((double) manualRect.X >= 0.0 && (double) manualRect.Y >= 0.0 || overlay))
      chart3D.AddTextBlockInCustomCanvas(textBlock, manualRect, overlay);
    this.converter.TryAndSortLegendItems((ChartBase) chart3D, sortedLegendOrders);
    if (this.IsAdornmentsHidden((ChartBase) chart3D))
      chart3D.AddHandlerLayoutUpdate();
    chart3D.Arrange(new Rect(0.0, 0.0, chart3D.Width, chart3D.Height));
    this.ResetChartCommonData();
    return (SfChart3D) chart3D;
  }

  public void SaveAsImage(IChart excelChart, Stream imageAsStream)
  {
    this._chart = excelChart;
    this.converter.SetChartSize(this._chart);
    Dictionary<int, string> names;
    this.converter.ListofPoints = this.converter.GetListofPoints(this._chart is ChartImpl ? this._chart as ChartImpl : (this._chart as ChartShapeImpl).ChartObject, out names);
    this.converter.Names = names;
    if (Application.Current == null)
    {
      Thread thread = new Thread(new ParameterizedThreadStart(this.GetChartStream));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start((object) imageAsStream);
      thread.Join();
    }
    else
    {
      Exception exception = UIDispatcher.Execute((Action) (() => this.GetChartStream((object) imageAsStream)));
      if (exception != null)
        throw exception;
    }
  }

  private void Save(ChartBase sfchart, object stream)
  {
    System.Windows.Size size = new System.Windows.Size((double) (int) sfchart.ActualWidth, (double) (int) sfchart.ActualHeight);
    sfchart.RenderTransform = (Transform) new ScaleTransform(1.0, 1.0, 0.5, 0.5);
    switch (sfchart)
    {
      case SfChartExt sfChartExt:
        if (sfChartExt.Series.Any<ChartSeries>((Func<ChartSeries, bool>) (x =>
        {
          string name = x.GetType().Name;
          return name.Contains("Candle") || name.Contains("Column") || name.Contains("Bar") || name.Contains("Waterfall");
        })))
        {
          RenderOptions.SetEdgeMode((DependencyObject) sfchart, EdgeMode.Aliased);
          break;
        }
        break;
    }
    switch (this.ScalingMode)
    {
      case ScalingMode.Normal:
        RenderOptions.SetBitmapScalingMode((DependencyObject) sfchart, BitmapScalingMode.LowQuality);
        break;
      default:
        RenderOptions.SetBitmapScalingMode((DependencyObject) sfchart, BitmapScalingMode.HighQuality);
        break;
    }
    sfchart.Measure(size);
    Rect finalRect = new Rect(size);
    sfchart.Arrange(finalRect);
    this.Rendered((Visual) sfchart, size, stream);
  }

  private void Rendered(Visual visual, System.Windows.Size size, object stream)
  {
    RenderTargetBitmap renderTargetBitmap = this.GetRenderTargetBitmap(size);
    renderTargetBitmap.Render(visual);
    BitmapEncoder bitmapEncoder = (BitmapEncoder) new JpegBitmapEncoder();
    ChartBase chartBase = visual as ChartBase;
    if (chartBase.Background == null || (chartBase.Background as SolidColorBrush).Color.A != byte.MaxValue || chartBase.BorderBrush == null || (chartBase.BorderBrush as SolidColorBrush).Color.A != byte.MaxValue)
      bitmapEncoder = (BitmapEncoder) new PngBitmapEncoder();
    bitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) renderTargetBitmap));
    bitmapEncoder.Save((Stream) stream);
  }

  public RenderTargetBitmap GetRenderTargetBitmap(System.Windows.Size size)
  {
    RenderTargetBitmap renderTargetBitmap;
    switch (this.ScalingMode)
    {
      case ScalingMode.Normal:
        double num1 = (double) this.ScalingMode / 96.0;
        renderTargetBitmap = new RenderTargetBitmap((int) (size.Width * num1), (int) (size.Height * num1), (double) this.ScalingMode, (double) this.ScalingMode, PixelFormats.Pbgra32);
        break;
      default:
        double num2 = (double) this.ScalingMode / 96.0;
        renderTargetBitmap = new RenderTargetBitmap((int) (size.Width * num2), (int) (size.Height * num2), (double) this.ScalingMode, (double) this.ScalingMode, PixelFormats.Default);
        break;
    }
    return renderTargetBitmap;
  }

  private void GetChartSerie(
    ExcelChartType serieType,
    SfChartExt sfChart,
    ChartSerieImpl serie,
    out bool isPie,
    out bool isStock,
    out bool isRadar,
    bool isNullSerie)
  {
    isPie = false;
    isStock = false;
    isRadar = false;
    switch (serieType)
    {
      case ExcelChartType.Column_Clustered:
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_3D:
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Clustered_3D:
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Clustered_3D:
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Clustered_3D:
        ChartSeries sfChartSerie1;
        if (isNullSerie)
        {
          ColumnSeries columnSeries = new ColumnSeries();
          columnSeries.XBindingPath = "X";
          columnSeries.YBindingPath = "Value";
          sfChartSerie1 = (ChartSeries) columnSeries;
          sfChartSerie1.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie1, serie);
        }
        else
          sfChartSerie1 = this.converter.SfColumnSeries(serie);
        sfChart.Series.Add(sfChartSerie1);
        break;
      case ExcelChartType.Column_Stacked:
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Pyramid_Stacked:
        ChartSeries sfChartSerie2;
        if (isNullSerie)
        {
          StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
          stackingColumnSeries.XBindingPath = "X";
          stackingColumnSeries.YBindingPath = "Value";
          sfChartSerie2 = (ChartSeries) stackingColumnSeries;
          sfChartSerie2.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie2, serie);
        }
        else
          sfChartSerie2 = this.converter.SfStackedColumnSeries(serie);
        sfChart.Series.Add(sfChartSerie2);
        break;
      case ExcelChartType.Column_Stacked_100:
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Pyramid_Stacked_100:
        StackingColumn100Series sfChartSerie3;
        if (isNullSerie)
        {
          StackingColumn100Series stackingColumn100Series = new StackingColumn100Series();
          stackingColumn100Series.XBindingPath = "X";
          stackingColumn100Series.YBindingPath = "Value";
          sfChartSerie3 = stackingColumn100Series;
          sfChartSerie3.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie3, serie);
        }
        else
          sfChartSerie3 = this.converter.SfStackColum100Series(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie3);
        break;
      case ExcelChartType.Bar_Clustered:
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Clustered:
        ChartSeries sfChartSerie4;
        if (isNullSerie)
        {
          BarSeries barSeries = new BarSeries();
          barSeries.XBindingPath = "X";
          barSeries.YBindingPath = "Value";
          sfChartSerie4 = (ChartSeries) barSeries;
          sfChartSerie4.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie4, serie);
        }
        else
          sfChartSerie4 = this.converter.SfBarseries(serie);
        sfChart.Series.Add(sfChartSerie4);
        break;
      case ExcelChartType.Bar_Stacked:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked:
        StackingBarSeries sfChartSerie5;
        if (isNullSerie)
        {
          StackingBarSeries stackingBarSeries = new StackingBarSeries();
          stackingBarSeries.XBindingPath = "X";
          stackingBarSeries.YBindingPath = "Value";
          sfChartSerie5 = stackingBarSeries;
          sfChartSerie5.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie5, serie);
        }
        else
          sfChartSerie5 = this.converter.SfStackBarSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie5);
        break;
      case ExcelChartType.Bar_Stacked_100:
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
        StackingBar100Series sfChartSerie6;
        if (isNullSerie)
        {
          StackingBar100Series stackingBar100Series = new StackingBar100Series();
          stackingBar100Series.XBindingPath = "X";
          stackingBar100Series.YBindingPath = "Value";
          sfChartSerie6 = stackingBar100Series;
          sfChartSerie6.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie6, serie);
        }
        else
          sfChartSerie6 = this.converter.SfStackBar100Series(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie6);
        break;
      case ExcelChartType.Line:
      case ExcelChartType.Line_Stacked:
      case ExcelChartType.Line_Stacked_100:
      case ExcelChartType.Line_Markers:
      case ExcelChartType.Line_Markers_Stacked:
      case ExcelChartType.Line_Markers_Stacked_100:
      case ExcelChartType.Line_3D:
        if (isNullSerie)
        {
          LineSeries lineSeries = new LineSeries();
          lineSeries.XBindingPath = "X";
          lineSeries.YBindingPath = "Value";
          LineSeries sfChartSerie7 = lineSeries;
          sfChartSerie7.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie7, serie);
          sfChart.Series.Add((ChartSeries) sfChartSerie7);
          break;
        }
        if ((serie.SerieFormat as ChartSerieDataFormatImpl).IsSmoothed)
        {
          SplineSeries splineSeries = this.converter.SfSplineSeries(serie);
          sfChart.Series.Add((ChartSeries) splineSeries);
          break;
        }
        ChartSeries chartSeries = this.converter.SfLineSeries(serie);
        sfChart.Series.Add(chartSeries);
        break;
      case ExcelChartType.Pie:
      case ExcelChartType.Pie_3D:
      case ExcelChartType.PieOfPie:
      case ExcelChartType.Pie_Exploded:
      case ExcelChartType.Pie_Exploded_3D:
      case ExcelChartType.Pie_Bar:
        PieSeries sfChartSerie8;
        if (isNullSerie)
        {
          PieSeries pieSeries = new PieSeries();
          pieSeries.XBindingPath = "X";
          pieSeries.YBindingPath = "Value";
          sfChartSerie8 = pieSeries;
          sfChartSerie8.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie8, serie);
        }
        else
          sfChartSerie8 = this.converter.SfPieSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie8);
        isPie = true;
        break;
      case ExcelChartType.Scatter_Markers:
        ChartSeries sfChartSerie9;
        if (isNullSerie)
        {
          ScatterSeries scatterSeries = new ScatterSeries();
          scatterSeries.XBindingPath = "X";
          scatterSeries.YBindingPath = "Value";
          sfChartSerie9 = (ChartSeries) scatterSeries;
          sfChartSerie9.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie9, serie);
        }
        else
          sfChartSerie9 = this.converter.SfScatterrSeries(serie);
        sfChart.Series.Add(sfChartSerie9);
        break;
      case ExcelChartType.Scatter_SmoothedLine_Markers:
      case ExcelChartType.Scatter_SmoothedLine:
        SplineSeries sfChartSerie10;
        if (isNullSerie)
        {
          SplineSeries splineSeries = new SplineSeries();
          splineSeries.XBindingPath = "X";
          splineSeries.YBindingPath = "Value";
          sfChartSerie10 = splineSeries;
          sfChartSerie10.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie10, serie);
        }
        else
          sfChartSerie10 = this.converter.SfSplineSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie10);
        break;
      case ExcelChartType.Scatter_Line_Markers:
      case ExcelChartType.Scatter_Line:
        ChartSeries sfChartSerie11;
        if (isNullSerie)
        {
          LineSeries lineSeries = new LineSeries();
          lineSeries.XBindingPath = "X";
          lineSeries.YBindingPath = "Value";
          sfChartSerie11 = (ChartSeries) lineSeries;
          sfChartSerie11.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie11, serie);
        }
        else
          sfChartSerie11 = this.converter.SfLineSeries(serie);
        sfChart.Series.Add(sfChartSerie11);
        break;
      case ExcelChartType.Area:
      case ExcelChartType.Area_3D:
        AreaSeries sfChartSerie12;
        if (isNullSerie)
        {
          AreaSeries areaSeries = new AreaSeries();
          areaSeries.XBindingPath = "X";
          areaSeries.YBindingPath = "Value";
          sfChartSerie12 = areaSeries;
          sfChartSerie12.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie12, serie);
        }
        else
          sfChartSerie12 = this.converter.SfAreaSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie12);
        break;
      case ExcelChartType.Area_Stacked:
      case ExcelChartType.Area_Stacked_3D:
        StackingAreaSeries sfChartSerie13;
        if (isNullSerie)
        {
          StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
          stackingAreaSeries.XBindingPath = "X";
          stackingAreaSeries.YBindingPath = "Value";
          sfChartSerie13 = stackingAreaSeries;
          sfChartSerie13.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie13, serie);
        }
        else
          sfChartSerie13 = this.converter.SfStackAreaSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie13);
        break;
      case ExcelChartType.Area_Stacked_100:
      case ExcelChartType.Area_Stacked_100_3D:
        StackingArea100Series sfChartSerie14;
        if (isNullSerie)
        {
          StackingArea100Series stackingArea100Series = new StackingArea100Series();
          stackingArea100Series.XBindingPath = "X";
          stackingArea100Series.YBindingPath = "Value";
          sfChartSerie14 = stackingArea100Series;
          sfChartSerie14.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie14, serie);
        }
        else
          sfChartSerie14 = this.converter.SfStackArea100Series(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie14);
        break;
      case ExcelChartType.Doughnut:
      case ExcelChartType.Doughnut_Exploded:
        DoughnutSeries sfChartSerie15;
        if (isNullSerie)
        {
          DoughnutSeries doughnutSeries = new DoughnutSeries();
          doughnutSeries.XBindingPath = "X";
          doughnutSeries.YBindingPath = "Value";
          sfChartSerie15 = doughnutSeries;
          sfChartSerie15.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie15, serie);
        }
        else
          sfChartSerie15 = this.converter.SfDoughnutSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie15);
        break;
      case ExcelChartType.Radar:
      case ExcelChartType.Radar_Markers:
      case ExcelChartType.Radar_Filled:
        RadarSeries sfChartSerie16;
        if (isNullSerie)
        {
          RadarSeries radarSeries = new RadarSeries();
          radarSeries.XBindingPath = "X";
          radarSeries.YBindingPath = "Value";
          sfChartSerie16 = radarSeries;
          sfChartSerie16.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie16, serie);
        }
        else
          sfChartSerie16 = this.converter.SfRadarSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie16);
        isRadar = true;
        break;
      case ExcelChartType.Bubble:
      case ExcelChartType.Bubble_3D:
        BubbleSeries sfChartSerie17;
        if (isNullSerie)
        {
          BubbleSeries bubbleSeries = new BubbleSeries();
          bubbleSeries.XBindingPath = "X";
          bubbleSeries.YBindingPath = "Value";
          sfChartSerie17 = bubbleSeries;
          sfChartSerie17.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie17, serie);
        }
        else
          sfChartSerie17 = this.converter.SfBubbleSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie17);
        break;
      case ExcelChartType.Stock_HighLowClose:
      case ExcelChartType.Stock_VolumeHighLowClose:
        HiLoSeries sfChartSerie18;
        if (isNullSerie)
        {
          HiLoSeries hiLoSeries = new HiLoSeries();
          hiLoSeries.XBindingPath = "X";
          hiLoSeries.High = "High";
          hiLoSeries.Low = "Low";
          sfChartSerie18 = hiLoSeries;
          sfChartSerie18.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie18, serie);
        }
        else
          sfChartSerie18 = this.converter.SfStockHiLoSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie18);
        isStock = true;
        break;
      case ExcelChartType.Stock_OpenHighLowClose:
      case ExcelChartType.Stock_VolumeOpenHighLowClose:
        CandleSeries sfChartSerie19;
        if (isNullSerie)
        {
          CandleSeries candleSeries = new CandleSeries();
          candleSeries.XBindingPath = "X";
          candleSeries.High = "High";
          candleSeries.Low = "Low";
          candleSeries.Open = "Open";
          candleSeries.Close = "Close";
          sfChartSerie19 = candleSeries;
          sfChartSerie19.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie19, serie);
        }
        else
          sfChartSerie19 = this.converter.SfCandleSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie19);
        isStock = true;
        break;
      case ExcelChartType.Funnel:
        RangeColumnSeries sfChartSerie20;
        if (isNullSerie)
        {
          RangeColumnSeries rangeColumnSeries = new RangeColumnSeries();
          rangeColumnSeries.XBindingPath = "X";
          rangeColumnSeries.High = "High";
          rangeColumnSeries.Low = "Low";
          rangeColumnSeries.IsTransposed = true;
          sfChartSerie20 = rangeColumnSeries;
          sfChartSerie20.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie20, serie);
        }
        else
          sfChartSerie20 = this.converter.SfFunnelSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie20);
        break;
      case ExcelChartType.WaterFall:
        CustomWaterfallSeries sfChartSerie21;
        if (isNullSerie)
        {
          CustomWaterfallSeries customWaterfallSeries = new CustomWaterfallSeries();
          customWaterfallSeries.XBindingPath = "X";
          customWaterfallSeries.SummaryBindingPath = "IsSummary";
          customWaterfallSeries.YBindingPath = "Value";
          customWaterfallSeries.AllowAutoSum = false;
          sfChartSerie21 = customWaterfallSeries;
          sfChartSerie21.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie21, serie);
        }
        else
          sfChartSerie21 = this.converter.SfWaterfallSeries(serie);
        sfChart.Series.Add((ChartSeries) sfChartSerie21);
        break;
      case ExcelChartType.Histogram:
      case ExcelChartType.Pareto:
        if (isNullSerie)
        {
          ColumnSeries columnSeries = new ColumnSeries();
          columnSeries.XBindingPath = "X";
          columnSeries.YBindingPath = "Value";
          ColumnSeries sfChartSerie22 = columnSeries;
          sfChartSerie22.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie22, serie);
          break;
        }
        LineSeries paretoLine;
        ColumnSeries columnSeries1 = this.converter.SfHistogramOrPareto(serie, serie.ParentChart, out paretoLine);
        sfChart.Series.Add((ChartSeries) columnSeries1);
        if (paretoLine == null)
          break;
        sfChart.Series.Add((ChartSeries) paretoLine);
        break;
    }
  }

  private void SetEmptySeriesValues(ChartSeriesBase sfChartSerie, ChartSerieImpl chartSerieImpl)
  {
    if (chartSerieImpl.ParentSeries.Count == 1 || chartSerieImpl.ParentSeries.Count<IChartSerie>((Func<IChartSerie, bool>) (x => x.EnteredDirectlyValues == null && x.Values == null)) == chartSerieImpl.ParentSeries.Count)
    {
      ViewModel viewModel = new ViewModel(2);
      viewModel.Products[0].Value = double.NaN;
      viewModel.Products[1].Value = double.NaN;
      viewModel.Products[0].X = (object) "";
      if ((chartSerieImpl.EnteredDirectlyCategoryLabels == null ? 1 : (chartSerieImpl.EnteredDirectlyCategoryLabels.Length == 0 ? 1 : 0)) != 0 && (chartSerieImpl.CategoryLabels == null ? 1 : (chartSerieImpl.CategoryLabels is ExternalRange ? 1 : (chartSerieImpl.CategoryLabels.Cells.Length == 0 ? 1 : 0))) != 0)
      {
        viewModel.Products[1].X = (object) "1";
      }
      else
      {
        object obj = (object) "1";
        if (chartSerieImpl.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelNone && !this.converter.IsChartEx)
        {
          ChartCategoryAxisImpl categoryAxisImpl = (chartSerieImpl.UsePrimaryAxis ? chartSerieImpl.ParentChart.PrimaryCategoryAxis : chartSerieImpl.ParentChart.SecondaryCategoryAxis) as ChartCategoryAxisImpl;
          IRange categoryLabels = chartSerieImpl.CategoryLabels;
          if (this.converter.CheckIfValidValueRange(categoryLabels) && categoryLabels.Cells.Length > 0)
          {
            IRange cell = categoryLabels.Cells[0];
            if (categoryAxisImpl.IsSourceLinked)
              obj = (object) cell.DisplayText;
            else if (!string.IsNullOrEmpty(categoryAxisImpl.NumberFormat))
              obj = (object) this.converter.ApplyNumberFormat(cell.Value2, categoryAxisImpl.NumberFormat);
          }
          else if (chartSerieImpl.EnteredDirectlyCategoryLabels != null && chartSerieImpl.EnteredDirectlyCategoryLabels.Length > 0)
          {
            obj = chartSerieImpl.EnteredDirectlyCategoryLabels[0];
            if (!categoryAxisImpl.IsSourceLinked && !string.IsNullOrEmpty(categoryAxisImpl.NumberFormat))
              obj = (object) this.converter.ApplyNumberFormat(categoryLabels.Value2, categoryAxisImpl.NumberFormat);
          }
        }
        viewModel.Products[1].X = (object) obj.ToString();
      }
      sfChartSerie.ItemsSource = (object) viewModel.Products;
    }
    else
      sfChartSerie.ItemsSource = (object) new ViewModel(0).Products;
    sfChartSerie.Label = this.converter.Names.ContainsKey(chartSerieImpl.Index) ? this.converter.Names[chartSerieImpl.Index] : this.converter.GetSerieName(chartSerieImpl);
    switch (sfChartSerie)
    {
      case LineSeries _:
      case SplineSeries _:
        sfChartSerie.LegendIcon = ChartLegendIcon.StraightLine;
        this.converter.GetBorderOnLineSeries(chartSerieImpl, sfChartSerie as ChartSeries);
        return;
      case CandleSeries _:
        sfChartSerie.LegendIcon = ChartLegendIcon.None;
        return;
      case BubbleSeries _:
        sfChartSerie.LegendIcon = ChartLegendIcon.Circle;
        break;
      case ScatterSeries _:
        sfChartSerie.LegendIcon = this.converter.GetChartLegendIcon(chartSerieImpl.SerieFormat as ChartSerieDataFormatImpl, chartSerieImpl.Index);
        break;
    }
    bool flag = true;
    if (this.converter.parentWorkbook.Version == ExcelVersion.Excel97to2003 && !this.converter.IsChartEx)
      flag = false;
    System.Windows.Media.Brush brush = !(chartSerieImpl.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || !flag ? this.converter.GetBrushFromDataFormat((IChartFillBorder) chartSerieImpl.SerieFormat) : (System.Windows.Media.Brush) new SolidColorBrush(this.converter.SfColor(chartSerieImpl.ParentChart.GetChartColor(chartSerieImpl.Number == -1 ? 0 : chartSerieImpl.Number, chartSerieImpl.ParentSeries.Count, false, false)));
    ChartColorModel chartColorModel = new ChartColorModel(ChartColorPalette.Custom)
    {
      CustomBrushes = new List<System.Windows.Media.Brush>(1)
    };
    chartColorModel.CustomBrushes.Add(brush);
    sfChartSerie.ColorModel = chartColorModel;
    sfChartSerie.Palette = ChartColorPalette.Custom;
  }

  private void GetChartSerie3D(
    ExcelChartType serieType,
    SfChart3D sfChart,
    ChartSerieImpl serie,
    out bool isPie,
    out bool isStock,
    bool isNullSerie)
  {
    isPie = false;
    isStock = false;
    switch (serieType)
    {
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_3D:
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Clustered_3D:
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Clustered_3D:
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Clustered_3D:
        ChartSeries3D sfChartSerie1;
        if (isNullSerie)
        {
          ColumnSeries3D columnSeries3D = new ColumnSeries3D();
          columnSeries3D.XBindingPath = "X";
          columnSeries3D.YBindingPath = "Value";
          sfChartSerie1 = (ChartSeries3D) columnSeries3D;
          sfChartSerie1.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie1, serie);
        }
        else
          sfChartSerie1 = this.converter.SfColumnSeries3D(serie);
        sfChart.Series.Add(sfChartSerie1);
        break;
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Pyramid_Stacked:
        ChartSeries3D sfChartSerie2;
        if (isNullSerie)
        {
          StackingColumnSeries3D stackingColumnSeries3D = new StackingColumnSeries3D();
          stackingColumnSeries3D.XBindingPath = "X";
          stackingColumnSeries3D.YBindingPath = "Value";
          sfChartSerie2 = (ChartSeries3D) stackingColumnSeries3D;
          sfChartSerie2.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie2, serie);
        }
        else
          sfChartSerie2 = (ChartSeries3D) this.converter.SfStackedColumnSeries3D(serie);
        sfChart.Series.Add(sfChartSerie2);
        break;
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Pyramid_Stacked_100:
        StackingColumn100Series3D sfChartSerie3;
        if (isNullSerie)
        {
          StackingColumn100Series3D column100Series3D = new StackingColumn100Series3D();
          column100Series3D.XBindingPath = "X";
          column100Series3D.YBindingPath = "Value";
          sfChartSerie3 = column100Series3D;
          sfChartSerie3.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie3, serie);
        }
        else
          sfChartSerie3 = this.converter.SfStackColum100Series3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie3);
        break;
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Clustered:
        BarSeries3D sfChartSerie4;
        if (isNullSerie)
        {
          BarSeries3D barSeries3D = new BarSeries3D();
          barSeries3D.XBindingPath = "X";
          barSeries3D.YBindingPath = "Value";
          sfChartSerie4 = barSeries3D;
          sfChartSerie4.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie4, serie);
        }
        else
          sfChartSerie4 = this.converter.SfBarseries3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie4);
        break;
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked:
        StackingBarSeries3D sfChartSerie5;
        if (isNullSerie)
        {
          StackingBarSeries3D stackingBarSeries3D = new StackingBarSeries3D();
          stackingBarSeries3D.XBindingPath = "X";
          stackingBarSeries3D.YBindingPath = "Value";
          sfChartSerie5 = stackingBarSeries3D;
          sfChartSerie5.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie5, serie);
        }
        else
          sfChartSerie5 = this.converter.SfStackBarSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie5);
        break;
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
        StackingBar100Series3D sfChartSerie6;
        if (isNullSerie)
        {
          StackingBar100Series3D stackingBar100Series3D = new StackingBar100Series3D();
          stackingBar100Series3D.XBindingPath = "X";
          stackingBar100Series3D.YBindingPath = "Value";
          sfChartSerie6 = stackingBar100Series3D;
          sfChartSerie6.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie6, serie);
        }
        else
          sfChartSerie6 = this.converter.SfStackBar100Series3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie6);
        break;
      case ExcelChartType.Line_3D:
        LineSeries3D sfChartSerie7;
        if (isNullSerie)
        {
          LineSeries3D lineSeries3D = new LineSeries3D();
          lineSeries3D.XBindingPath = "X";
          lineSeries3D.YBindingPath = "Value";
          sfChartSerie7 = lineSeries3D;
          sfChartSerie7.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie7, serie);
        }
        else
          sfChartSerie7 = this.converter.SfLineSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie7);
        break;
      case ExcelChartType.Pie_3D:
      case ExcelChartType.Pie_Exploded_3D:
        PieSeries3D sfChartSerie8;
        if (isNullSerie)
        {
          PieSeries3D pieSeries3D = new PieSeries3D();
          pieSeries3D.XBindingPath = "X";
          pieSeries3D.YBindingPath = "Value";
          sfChartSerie8 = pieSeries3D;
          sfChartSerie8.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie8, serie);
        }
        else
          sfChartSerie8 = this.converter.SfPieSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie8);
        isPie = true;
        break;
      case ExcelChartType.Area_3D:
        AreaSeries3D sfChartSerie9;
        if (isNullSerie)
        {
          AreaSeries3D areaSeries3D = new AreaSeries3D();
          areaSeries3D.XBindingPath = "X";
          areaSeries3D.YBindingPath = "Value";
          sfChartSerie9 = areaSeries3D;
          sfChartSerie9.LegendIcon = ChartLegendIcon.Rectangle;
          this.SetEmptySeriesValues((ChartSeriesBase) sfChartSerie9, serie);
        }
        else
          sfChartSerie9 = this.converter.SfAreaSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) sfChartSerie9);
        break;
    }
  }
}
