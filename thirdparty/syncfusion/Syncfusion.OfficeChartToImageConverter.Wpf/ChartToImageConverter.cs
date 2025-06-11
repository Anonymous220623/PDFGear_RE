// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.ChartToImageConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

public class ChartToImageConverter : IOfficeChartToImageConverter
{
  private IOfficeChart _chart;
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
      case OfficeChartType.Column_Clustered_3D:
      case OfficeChartType.Column_Stacked_3D:
      case OfficeChartType.Column_Stacked_100_3D:
      case OfficeChartType.Column_3D:
      case OfficeChartType.Bar_Clustered_3D:
      case OfficeChartType.Bar_Stacked_3D:
      case OfficeChartType.Bar_Stacked_100_3D:
      case OfficeChartType.Line_3D:
      case OfficeChartType.Pie_3D:
      case OfficeChartType.Pie_Exploded_3D:
      case OfficeChartType.Area_3D:
      case OfficeChartType.Cylinder_Clustered:
      case OfficeChartType.Cylinder_Stacked:
      case OfficeChartType.Cylinder_Stacked_100:
      case OfficeChartType.Cylinder_Bar_Clustered:
      case OfficeChartType.Cylinder_Bar_Stacked:
      case OfficeChartType.Cylinder_Bar_Stacked_100:
      case OfficeChartType.Cylinder_Clustered_3D:
      case OfficeChartType.Cone_Clustered:
      case OfficeChartType.Cone_Stacked:
      case OfficeChartType.Cone_Stacked_100:
      case OfficeChartType.Cone_Bar_Clustered:
      case OfficeChartType.Cone_Bar_Stacked:
      case OfficeChartType.Cone_Bar_Stacked_100:
      case OfficeChartType.Cone_Clustered_3D:
      case OfficeChartType.Pyramid_Clustered:
      case OfficeChartType.Pyramid_Stacked:
      case OfficeChartType.Pyramid_Stacked_100:
      case OfficeChartType.Pyramid_Bar_Clustered:
      case OfficeChartType.Pyramid_Bar_Stacked:
      case OfficeChartType.Pyramid_Bar_Stacked_100:
      case OfficeChartType.Pyramid_Clustered_3D:
        return true;
      default:
        return false;
    }
  }

  public SfChart GetChart(IOfficeChart excelChart)
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
    if (this.converter.IsChartEx)
    {
      chart.PrimaryAxis = (ChartAxisBase2D) new CustomCategoryAxis();
    }
    else
    {
      if (Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_SCATTER, this._chart.ChartType) != -1 || Array.IndexOf<OfficeChartType>(ChartImpl.CHARTS_BUBBLE, this._chart.ChartType) != -1)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomNumericalAxis();
      else if ((this._chart.PrimaryCategoryAxis as ChartCategoryAxisImpl).IsChartBubbleOrScatter)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomNumericalAxis();
      else if (this._chart.PrimaryCategoryAxis.CategoryType == OfficeCategoryType.Time)
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomDateTimeAxis();
      else
        chart.PrimaryAxis = (ChartAxisBase2D) new CustomCategoryAxis();
      if (this._chart.PrimaryValueAxis.IsLogScale)
        chart.SecondaryAxis = (RangeAxisBase) new LogarithmicAxis();
      else
        chart.SecondaryAxis = (RangeAxisBase) new CustomNumericalAxis();
      chart.SecondaryAxis.MaximumLabels = 5;
    }
    chart.Name = this._chart.ChartType.ToString();
    List<IOfficeChartSerie> officeChartSerieList = this.GetOfficeChartSeries(this._chart.Series);
    if (isCombinationChart)
    {
      officeChartSerieList = this._chart.Series.OrderByType();
      if (officeChartSerieList.Count == 0 && this._chart.Series.Count == this._chart.Series.Count<IOfficeChartSerie>((Func<IOfficeChartSerie, bool>) (x => x.SerieType.ToString().Contains("Bubble"))))
        officeChartSerieList = this._chart.Series.ToList<IOfficeChartSerie>();
    }
    Binding binding1 = (Binding) null;
    Binding binding2 = (Binding) null;
    ChartAxisBase2D chartAxisBase2D = (ChartAxisBase2D) null;
    RangeAxisBase rangeAxisBase = (RangeAxisBase) null;
    foreach (IOfficeChartSerie officeChartSerie in officeChartSerieList)
    {
      if (!officeChartSerie.IsFiltered || this.converter.IsChartEx)
      {
        ChartSerieImpl serie = officeChartSerie as ChartSerieImpl;
        bool isNullSerie = serie.ValuesIRange == null && serie.EnteredDirectlyValues == null;
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
        this.GetChartSerie(isCombinationChart ? officeChartSerie.SerieType : this._chart.ChartType, (SfChart) chart, serie, out isPie, out isStock, out isRadar, isNullSerie);
        if (chart.Series.Count > 0 && !this.converter.IsChart3D && !officeChartSerie.UsePrimaryAxis)
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
      textBlock = this.converter.SfChartTitle((IOfficeChart) chartImpl, out manualRect1);
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
        {
          this.converter.SfLogerthmicAxis(chart.SecondaryAxis as LogarithmicAxis, primaryValueAxis, primaryCategoryAxis);
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
    else
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
        flag = chartImpl.Series[index].SerieFormat.CommonSerieOptions.SizeRepresents == ChartBubbleSize.Width;
        break;
      }
    }
    expectedSize = num1 > 100 ? (num1 > 200 ? expectedSize * 0.5 * ((double) num1 / 300.0) : expectedSize * 0.375 * ((double) num1 / 200.0)) : expectedSize * 0.25 * ((double) num1 / 100.0);
    IEnumerable<\u003C\u003Ef__AnonymousType4<double, double, BubbleSeries>> source = sfChart.Series.Select(serie => new
    {
      serie = serie,
      bSerie = serie as BubbleSeries
    }).Select(_param0 => new
    {
      \u003C\u003Eh__TransparentIdentifier5 = _param0,
      items = _param0.serie.ItemsSource as ObservableCollection<ChartPoint>
    }).Where(_param1 =>
    {
      if (_param1.items.Count <= 0)
        return false;
      return _param1.\u003C\u003Eh__TransparentIdentifier5.bSerie.YAxis != null ? !isPrimary : isPrimary;
    }).Select(_param0 => new
    {
      MaxSz = Convert.ToDouble(_param0.items.Max<ChartPoint, object>((Func<ChartPoint, object>) (x => x.Size))),
      MinSz = Convert.ToDouble(_param0.items.Min<ChartPoint, object>((Func<ChartPoint, object>) (x => x.Size))),
      Serie = _param0.\u003C\u003Eh__TransparentIdentifier5.bSerie
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

  private List<IOfficeChartSerie> GetOfficeChartSeries(IOfficeChartSeries officeChartSeries)
  {
    List<IOfficeChartSerie> officeChartSeries1 = new List<IOfficeChartSerie>((IEnumerable<IOfficeChartSerie>) officeChartSeries);
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) officeChartSeries)
    {
      if (chartSerieImpl.SerieName != null)
        chartSerieImpl.Name = chartSerieImpl.SerieName;
      object[] directlyCategoryLabels = chartSerieImpl.EnteredDirectlyCategoryLabels;
      object[] enteredDirectlyValues = chartSerieImpl.EnteredDirectlyValues;
      object[] enteredDirectlyBubbles = chartSerieImpl.EnteredDirectlyBubbles;
      if (directlyCategoryLabels != null && chartSerieImpl.CategoryLabelsIRange != null && chartSerieImpl.CategoryLabelsIRange is RangeImpl categoryLabelsIrange)
      {
        for (int index = 0; index < directlyCategoryLabels.Length; ++index)
        {
          if (index < categoryLabelsIrange.Cells.Length)
          {
            string str = categoryLabelsIrange.Cells[index].NumberFormat;
            if (str == "General" && chartSerieImpl.CategoriesFormatCode != null && chartSerieImpl.CategoriesFormatCode != str)
              str = chartSerieImpl.CategoriesFormatCode;
            categoryLabelsIrange.Cells[index].Value2 = directlyCategoryLabels[index];
            categoryLabelsIrange.Cells[index].NumberFormat = str;
          }
        }
      }
      if (enteredDirectlyValues != null && chartSerieImpl.ValuesIRange != null && chartSerieImpl.ValuesIRange is RangeImpl valuesIrange)
      {
        for (int index = 0; index < enteredDirectlyValues.Length; ++index)
        {
          if (index < valuesIrange.Cells.Length)
          {
            string str = valuesIrange.Cells[index].NumberFormat;
            if (str == "General" && chartSerieImpl.FormatCode != null && chartSerieImpl.FormatCode != str)
              str = chartSerieImpl.FormatCode;
            valuesIrange.Cells[index].Value2 = enteredDirectlyValues[index];
            valuesIrange.Cells[index].NumberFormat = str;
          }
        }
      }
      if (enteredDirectlyBubbles != null && chartSerieImpl.BubblesIRange != null && chartSerieImpl.BubblesIRange is RangeImpl bubblesIrange)
      {
        for (int index = 0; index < enteredDirectlyBubbles.Length; ++index)
        {
          if (bubblesIrange != null && index < bubblesIrange.Cells.Length)
            bubblesIrange.Cells[index].Value2 = enteredDirectlyBubbles[index];
        }
      }
    }
    return officeChartSeries1;
  }

  private bool IsAutoChartTitle(ChartImpl chartImpl) => false;

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
    if (this.converter.engine == null)
      return;
    this.converter.engine.Dispose();
    this.converter.engine = (ExcelEngine) null;
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

  private void SetAxisReplacement(SfChartExt sfChart, IOfficeChart chart, out bool isChanged)
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
    if (sfChart.PrimaryAxis.Visibility != Visibility.Visible || !flag1 && !flag2 || sfChart.PrimaryAxis.OpposedPosition || chart.PrimaryCategoryAxis.TickLabelPosition != OfficeTickLabelPosition.TickLabelPosition_Low || chart.PrimaryValueAxis.IsMaxCross || !chart.PrimaryValueAxis.IsAutoCross && origin == position1)
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
    if (chart.PrimaryCategoryAxis.MajorTickMark != OfficeTickMark.TickMark_None)
    {
      double num2 = (visibleLabels2[1].Position - visibleLabels2[0].Position) / 5.0;
      double topValue = origin + (chart.PrimaryCategoryAxis.MajorTickMark == OfficeTickMark.TickMark_Outside ? 0.0 : num2) - num1;
      double bottomValue = origin + (chart.PrimaryCategoryAxis.MajorTickMark == OfficeTickMark.TickMark_Inside ? 0.0 : -num2) - num1;
      int interval = chart.PrimaryCategoryAxis.TickMarkSpacing <= 0 ? 1 : chart.PrimaryCategoryAxis.TickMarkSpacing;
      this.AddTickMarks(sfChart, topValue, bottomValue, interval, axisLineColor, thickness, true);
    }
    if (chart.PrimaryCategoryAxis.MinorTickMark == OfficeTickMark.TickMark_None)
      return;
    double num3 = (visibleLabels2[1].Position - visibleLabels2[0].Position) / 7.0;
    double topValue1 = origin + (chart.PrimaryCategoryAxis.MinorTickMark == OfficeTickMark.TickMark_Outside ? 0.0 : num3) - num1;
    double bottomValue1 = origin + (chart.PrimaryCategoryAxis.MinorTickMark == OfficeTickMark.TickMark_Inside ? 0.0 : -num3) - num1;
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

  public SfChart3D GetChart3D(IOfficeChart excelChart)
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
    if (this._chart.PrimaryCategoryAxis.CategoryType == OfficeCategoryType.Time)
      chart3D.PrimaryAxis = (ChartAxisBase3D) new DateTimeAxis3D();
    else
      chart3D.PrimaryAxis = (ChartAxisBase3D) new CategoryAxis3D();
    if (this._chart.PrimaryValueAxis.IsLogScale)
      chart3D.SecondaryAxis = (RangeAxisBase3D) new LogarithmicAxis3D();
    else
      chart3D.SecondaryAxis = (RangeAxisBase3D) new CustomNumericalAxis3D();
    if (Array.IndexOf<OfficeChartType>(ChartImpl.DEF_WALLS_OR_FLOOR_TYPES, this._chart.ChartType) >= 0)
      this.converter.SfWall((SfChart3D) chart3D, chartImpl);
    this.converter.SfRotation3D(chartImpl, (SfChart3D) chart3D);
    chart3D.SecondaryAxis.MaximumLabels = 5;
    chart3D.Name = this._chart.ChartType.ToString();
    foreach (IOfficeChartSerie officeChartSerie in this.GetOfficeChartSeries(this._chart.Series))
    {
      if (!officeChartSerie.IsFiltered)
      {
        ChartSerieImpl serie = officeChartSerie as ChartSerieImpl;
        bool isNullSerie = serie.ValuesIRange == null && serie.EnteredDirectlyValues == null;
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
      textBlock = this.converter.SfChartTitle((IOfficeChart) chartImpl, out manualRect);
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

  public void SaveAsImage(IOfficeChart excelChart, Stream imageAsStream)
  {
    this._chart = excelChart;
    this.converter.SetChartSize(this._chart);
    if (!(this._chart is ChartImpl))
    {
      ChartImpl chartObject = (this._chart as ChartShapeImpl).ChartObject;
    }
    this.converter.ListofPoints = new Dictionary<int, ObservableCollection<ChartPoint>>(0);
    this.converter.Names = new Dictionary<int, string>(0);
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
    OfficeChartType serieType,
    SfChart sfChart,
    ChartSerieImpl serie,
    out bool isPie,
    out bool isStock,
    out bool isRadar,
    bool isNullSerie)
  {
    isPie = false;
    isStock = false;
    isRadar = false;
    if (isNullSerie || serie.ValuesIRange is ExternalRange && serie.EnteredDirectlyValues == null)
      return;
    switch (serieType)
    {
      case OfficeChartType.Column_Clustered:
      case OfficeChartType.Column_Clustered_3D:
      case OfficeChartType.Column_3D:
      case OfficeChartType.Cylinder_Clustered:
      case OfficeChartType.Cylinder_Clustered_3D:
      case OfficeChartType.Cone_Clustered:
      case OfficeChartType.Cone_Clustered_3D:
      case OfficeChartType.Pyramid_Clustered:
      case OfficeChartType.Pyramid_Clustered_3D:
        ChartSeries chartSeries1 = this.converter.SfColumnSeries(serie);
        if (chartSeries1 == null)
          break;
        sfChart.Series.Add(chartSeries1);
        break;
      case OfficeChartType.Column_Stacked:
      case OfficeChartType.Column_Stacked_3D:
      case OfficeChartType.Cylinder_Stacked:
      case OfficeChartType.Cone_Stacked:
      case OfficeChartType.Pyramid_Stacked:
        ChartSeries chartSeries2 = this.converter.SfStackedColumnSeries(serie);
        if (chartSeries2 == null)
          break;
        sfChart.Series.Add(chartSeries2);
        break;
      case OfficeChartType.Column_Stacked_100:
      case OfficeChartType.Column_Stacked_100_3D:
      case OfficeChartType.Cylinder_Stacked_100:
      case OfficeChartType.Cone_Stacked_100:
      case OfficeChartType.Pyramid_Stacked_100:
        StackingColumn100Series stackingColumn100Series = this.converter.SfStackColum100Series(serie);
        if (stackingColumn100Series == null)
          break;
        sfChart.Series.Add((ChartSeries) stackingColumn100Series);
        break;
      case OfficeChartType.Bar_Clustered:
      case OfficeChartType.Bar_Clustered_3D:
      case OfficeChartType.Cylinder_Bar_Clustered:
      case OfficeChartType.Cone_Bar_Clustered:
      case OfficeChartType.Pyramid_Bar_Clustered:
        ChartSeries chartSeries3 = this.converter.SfBarseries(serie);
        if (chartSeries3 == null)
          break;
        sfChart.Series.Add(chartSeries3);
        break;
      case OfficeChartType.Bar_Stacked:
      case OfficeChartType.Bar_Stacked_3D:
      case OfficeChartType.Cylinder_Bar_Stacked:
      case OfficeChartType.Cone_Bar_Stacked:
      case OfficeChartType.Pyramid_Bar_Stacked:
        StackingBarSeries stackingBarSeries = this.converter.SfStackBarSeries(serie);
        if (stackingBarSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) stackingBarSeries);
        break;
      case OfficeChartType.Bar_Stacked_100:
      case OfficeChartType.Bar_Stacked_100_3D:
      case OfficeChartType.Cylinder_Bar_Stacked_100:
      case OfficeChartType.Cone_Bar_Stacked_100:
      case OfficeChartType.Pyramid_Bar_Stacked_100:
        StackingBar100Series stackingBar100Series = this.converter.SfStackBar100Series(serie);
        if (stackingBar100Series == null)
          break;
        sfChart.Series.Add((ChartSeries) stackingBar100Series);
        break;
      case OfficeChartType.Line:
      case OfficeChartType.Line_Stacked:
      case OfficeChartType.Line_Markers:
      case OfficeChartType.Line_Markers_Stacked:
      case OfficeChartType.Line_Markers_Stacked_100:
      case OfficeChartType.Line_3D:
        if ((serie.SerieFormat as ChartSerieDataFormatImpl).IsSmoothed)
        {
          SplineSeries splineSeries = this.converter.SfSplineSeries(serie);
          if (splineSeries == null)
            break;
          sfChart.Series.Add((ChartSeries) splineSeries);
          break;
        }
        ChartSeries chartSeries4 = this.converter.SfLineSeries(serie);
        if (chartSeries4 == null)
          break;
        sfChart.Series.Add(chartSeries4);
        break;
      case OfficeChartType.Pie:
      case OfficeChartType.Pie_3D:
      case OfficeChartType.PieOfPie:
      case OfficeChartType.Pie_Exploded:
      case OfficeChartType.Pie_Exploded_3D:
        PieSeries pieSeries = this.converter.SfPieSeries(serie);
        if (pieSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) pieSeries);
        isPie = true;
        break;
      case OfficeChartType.Scatter_Markers:
        ChartSeries chartSeries5 = this.converter.SfScatterrSeries(serie);
        if (chartSeries5 == null)
          break;
        chartSeries5.IsSortData = true;
        chartSeries5.SortBy = SortingAxis.X;
        chartSeries5.SortDirection = Direction.Ascending;
        sfChart.Series.Add(chartSeries5);
        break;
      case OfficeChartType.Scatter_SmoothedLine_Markers:
      case OfficeChartType.Scatter_SmoothedLine:
        SplineSeries splineSeries1 = this.converter.SfSplineSeries(serie);
        if (splineSeries1 == null)
          break;
        sfChart.Series.Add((ChartSeries) splineSeries1);
        break;
      case OfficeChartType.Scatter_Line_Markers:
      case OfficeChartType.Scatter_Line:
        ChartSeries chartSeries6 = this.converter.SfLineSeries(serie);
        if (chartSeries6 == null)
          break;
        sfChart.Series.Add(chartSeries6);
        break;
      case OfficeChartType.Area:
      case OfficeChartType.Area_3D:
        AreaSeries areaSeries = this.converter.SfAreaSeries(serie);
        if (areaSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) areaSeries);
        break;
      case OfficeChartType.Area_Stacked:
      case OfficeChartType.Area_Stacked_3D:
        StackingAreaSeries stackingAreaSeries = this.converter.SfStackAreaSeries(serie);
        if (stackingAreaSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) stackingAreaSeries);
        break;
      case OfficeChartType.Area_Stacked_100:
      case OfficeChartType.Area_Stacked_100_3D:
        StackingArea100Series stackingArea100Series = this.converter.SfStackArea100Series(serie);
        if (stackingArea100Series == null)
          break;
        sfChart.Series.Add((ChartSeries) stackingArea100Series);
        break;
      case OfficeChartType.Doughnut:
      case OfficeChartType.Doughnut_Exploded:
        DoughnutSeries doughnutSeries = this.converter.SfDoughnutSeries(serie);
        if (doughnutSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) doughnutSeries);
        break;
      case OfficeChartType.Radar:
      case OfficeChartType.Radar_Markers:
      case OfficeChartType.Radar_Filled:
        RadarSeries radarSeries = this.converter.SfRadarSeries(serie);
        if (radarSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) radarSeries);
        isRadar = true;
        break;
      case OfficeChartType.Bubble:
      case OfficeChartType.Bubble_3D:
        BubbleSeries bubbleSeries = this.converter.SfBubbleSeries(serie);
        if (bubbleSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) bubbleSeries);
        break;
      case OfficeChartType.Stock_HighLowClose:
      case OfficeChartType.Stock_VolumeHighLowClose:
        HiLoSeries hiLoSeries = this.converter.SfStockHiLoSeries(serie);
        if (hiLoSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) hiLoSeries);
        isStock = true;
        break;
      case OfficeChartType.Stock_OpenHighLowClose:
      case OfficeChartType.Stock_VolumeOpenHighLowClose:
        CandleSeries candleSeries = this.converter.SfCandleSeries(serie);
        if (candleSeries == null)
          break;
        sfChart.Series.Add((ChartSeries) candleSeries);
        isStock = true;
        break;
    }
  }

  private void GetChartSerie3D(
    OfficeChartType serieType,
    SfChart3D sfChart,
    ChartSerieImpl serie,
    out bool isPie,
    out bool isStock,
    bool isNullSerie)
  {
    isPie = false;
    isStock = false;
    if (isNullSerie || serie.ValuesIRange is ExternalRange && serie.EnteredDirectlyValues == null)
      return;
    switch (serieType)
    {
      case OfficeChartType.Column_Clustered_3D:
      case OfficeChartType.Column_3D:
      case OfficeChartType.Cylinder_Clustered:
      case OfficeChartType.Cylinder_Clustered_3D:
      case OfficeChartType.Cone_Clustered:
      case OfficeChartType.Cone_Clustered_3D:
      case OfficeChartType.Pyramid_Clustered:
      case OfficeChartType.Pyramid_Clustered_3D:
        ChartSeries3D chartSeries3D1 = this.converter.SfColumnSeries3D(serie);
        sfChart.Series.Add(chartSeries3D1);
        break;
      case OfficeChartType.Column_Stacked_3D:
      case OfficeChartType.Cylinder_Stacked:
      case OfficeChartType.Cone_Stacked:
      case OfficeChartType.Pyramid_Stacked:
        ChartSeries3D chartSeries3D2 = (ChartSeries3D) this.converter.SfStackedColumnSeries3D(serie);
        sfChart.Series.Add(chartSeries3D2);
        break;
      case OfficeChartType.Column_Stacked_100_3D:
      case OfficeChartType.Cylinder_Stacked_100:
      case OfficeChartType.Cone_Stacked_100:
      case OfficeChartType.Pyramid_Stacked_100:
        StackingColumn100Series3D column100Series3D = this.converter.SfStackColum100Series3D(serie);
        sfChart.Series.Add((ChartSeries3D) column100Series3D);
        break;
      case OfficeChartType.Bar_Clustered_3D:
      case OfficeChartType.Cylinder_Bar_Clustered:
      case OfficeChartType.Cone_Bar_Clustered:
      case OfficeChartType.Pyramid_Bar_Clustered:
        BarSeries3D barSeries3D = this.converter.SfBarseries3D(serie);
        sfChart.Series.Add((ChartSeries3D) barSeries3D);
        break;
      case OfficeChartType.Bar_Stacked_3D:
      case OfficeChartType.Cylinder_Bar_Stacked:
      case OfficeChartType.Cone_Bar_Stacked:
      case OfficeChartType.Pyramid_Bar_Stacked:
        StackingBarSeries3D stackingBarSeries3D = this.converter.SfStackBarSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) stackingBarSeries3D);
        break;
      case OfficeChartType.Bar_Stacked_100_3D:
      case OfficeChartType.Cylinder_Bar_Stacked_100:
      case OfficeChartType.Cone_Bar_Stacked_100:
      case OfficeChartType.Pyramid_Bar_Stacked_100:
        StackingBar100Series3D stackingBar100Series3D = this.converter.SfStackBar100Series3D(serie);
        sfChart.Series.Add((ChartSeries3D) stackingBar100Series3D);
        break;
      case OfficeChartType.Line_3D:
        LineSeries3D lineSeries3D = this.converter.SfLineSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) lineSeries3D);
        break;
      case OfficeChartType.Pie_3D:
      case OfficeChartType.Pie_Exploded_3D:
        PieSeries3D pieSeries3D = this.converter.SfPieSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) pieSeries3D);
        isPie = true;
        break;
      case OfficeChartType.Area_3D:
        AreaSeries3D areaSeries3D = this.converter.SfAreaSeries3D(serie);
        sfChart.Series.Add((ChartSeries3D) areaSeries3D);
        break;
    }
  }
}
