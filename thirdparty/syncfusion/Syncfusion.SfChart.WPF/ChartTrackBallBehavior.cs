// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTrackBallBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartTrackBallBehavior : ChartBehavior
{
  private const int seriesTipHeight = 6;
  private const int axisTipHeight = 6;
  private const int trackLabelSpacing = 8;
  public static readonly DependencyProperty AxisLabelAlignmentProperty = DependencyProperty.Register(nameof (AxisLabelAlignment), typeof (ChartAlignment), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) ChartAlignment.Center));
  public static readonly DependencyProperty LabelDisplayModeProperty = DependencyProperty.Register(nameof (LabelDisplayMode), typeof (TrackballLabelDisplayMode), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) TrackballLabelDisplayMode.FloatAllPoints));
  internal static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register(nameof (LabelBackground), typeof (Brush), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
  public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(nameof (LineStyle), typeof (Style), typeof (ChartTrackBallBehavior), new PropertyMetadata(ChartDictionaries.GenericCommonDictionary[(object) "trackBallLineStyle"]));
  public static readonly DependencyProperty ShowLineProperty = DependencyProperty.Register(nameof (ShowLine), typeof (bool), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartTrackBallBehavior.OnShowLinePropertyChanged)));
  public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register(nameof (LabelVerticalAlignment), typeof (ChartAlignment), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) ChartAlignment.Auto));
  public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register(nameof (LabelHorizontalAlignment), typeof (ChartAlignment), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) ChartAlignment.Auto));
  public static readonly DependencyProperty ChartTrackBallStyleProperty = DependencyProperty.Register(nameof (ChartTrackBallStyle), typeof (Style), typeof (ChartTrackBallBehavior), (PropertyMetadata) null);
  public static readonly DependencyProperty UseSeriesPaletteProperty = DependencyProperty.Register(nameof (UseSeriesPalette), typeof (bool), typeof (ChartTrackBallBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartTrackBallBehavior.OnLayoutUpdated)));
  internal string labelXValue;
  internal string labelYValue;
  internal bool isOpposedAxis;
  private string currentXLabel = string.Empty;
  private string currentYLabel = string.Empty;
  private bool isActivated;
  private bool isCancel;
  private int seriesCount;
  private Border border;
  private Line line;
  private List<FrameworkElement> elements;
  private ObservableCollection<ChartPointInfo> pointInfos;
  private List<ChartTrackBallControl> trackBalls;
  private ObservableCollection<ChartPointInfo> previousPointInfos;
  private double trackballWidth;
  private Dictionary<double, List<double>> groupedYValues;
  private List<double> seriesYVal;
  private List<double> indicatorYVal;
  private List<double> yValues;
  private double tempXPos = double.MinValue;
  private List<double> Values = new List<double>();
  private bool isLeftButtonPressed;
  private bool isTrackBallUpdateDispatched;
  private List<ContentControl> labelElements;
  private List<ContentControl> axisLabelElements;
  private List<ContentControl> groupLabelElements;
  private Dictionary<ChartAxis, ChartPointInfo> axisLabels;

  internal string previousXLabel { get; set; }

  internal string previousYLabel { get; set; }

  public ChartTrackBallBehavior()
  {
    this.elements = new List<FrameworkElement>();
    this.pointInfos = new ObservableCollection<ChartPointInfo>();
    this.line = new Line();
    this.labelElements = new List<ContentControl>();
    this.groupLabelElements = new List<ContentControl>();
    this.axisLabelElements = new List<ContentControl>();
    this.axisLabels = new Dictionary<ChartAxis, ChartPointInfo>();
    this.trackBalls = new List<ChartTrackBallControl>();
    this.previousXLabel = string.Empty;
    this.previousYLabel = string.Empty;
  }

  public event EventHandler<PositionChangingEventArgs> PositionChanging;

  public event EventHandler<PositionChangedEventArgs> PositionChanged;

  public ObservableCollection<ChartPointInfo> PointInfos
  {
    get => this.pointInfos;
    internal set => this.pointInfos = value;
  }

  public ChartAlignment AxisLabelAlignment
  {
    get => (ChartAlignment) this.GetValue(ChartTrackBallBehavior.AxisLabelAlignmentProperty);
    set => this.SetValue(ChartTrackBallBehavior.AxisLabelAlignmentProperty, (object) value);
  }

  public Style LineStyle
  {
    get => (Style) this.GetValue(ChartTrackBallBehavior.LineStyleProperty);
    set => this.SetValue(ChartTrackBallBehavior.LineStyleProperty, (object) value);
  }

  public TrackballLabelDisplayMode LabelDisplayMode
  {
    get
    {
      return (TrackballLabelDisplayMode) this.GetValue(ChartTrackBallBehavior.LabelDisplayModeProperty);
    }
    set => this.SetValue(ChartTrackBallBehavior.LabelDisplayModeProperty, (object) value);
  }

  internal Brush LabelBackground
  {
    get => (Brush) this.GetValue(ChartTrackBallBehavior.LabelBackgroundProperty);
    set => this.SetValue(ChartTrackBallBehavior.LabelBackgroundProperty, (object) value);
  }

  public bool ShowLine
  {
    get => (bool) this.GetValue(ChartTrackBallBehavior.ShowLineProperty);
    set => this.SetValue(ChartTrackBallBehavior.ShowLineProperty, (object) value);
  }

  public ChartAlignment LabelVerticalAlignment
  {
    get => (ChartAlignment) this.GetValue(ChartTrackBallBehavior.LabelVerticalAlignmentProperty);
    set => this.SetValue(ChartTrackBallBehavior.LabelVerticalAlignmentProperty, (object) value);
  }

  public ChartAlignment LabelHorizontalAlignment
  {
    get => (ChartAlignment) this.GetValue(ChartTrackBallBehavior.LabelHorizontalAlignmentProperty);
    set => this.SetValue(ChartTrackBallBehavior.LabelHorizontalAlignmentProperty, (object) value);
  }

  public Style ChartTrackBallStyle
  {
    get => (Style) this.GetValue(ChartTrackBallBehavior.ChartTrackBallStyleProperty);
    set => this.SetValue(ChartTrackBallBehavior.ChartTrackBallStyleProperty, (object) value);
  }

  public bool UseSeriesPalette
  {
    get => (bool) this.GetValue(ChartTrackBallBehavior.UseSeriesPaletteProperty);
    set => this.SetValue(ChartTrackBallBehavior.UseSeriesPaletteProperty, (object) value);
  }

  internal Point CurrentPoint { get; set; }

  protected internal bool IsActivated
  {
    get => this.isActivated;
    set
    {
      this.isActivated = value;
      this.Activate(this.isActivated);
    }
  }

  internal void ScheduleTrackBallUpdate()
  {
    if (this.isTrackBallUpdateDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnPointerPositionChanged));
    this.isTrackBallUpdateDispatched = true;
  }

  private void ValidateTrackballNaNValueForLinearData(
    double yValue,
    ChartSeriesBase series,
    double stackedYValue,
    double k,
    double tempValue,
    ref double x,
    ref double y,
    ref double xVal)
  {
    bool flag1 = series.ActualXAxis is CategoryAxis && !(series.ActualXAxis as CategoryAxis).IsIndexed;
    bool flag2 = series is RangeColumnSeries && !series.IsMultipleYPathRequired;
    bool flag3 = series is StackingSeriesBase;
    int index;
    if (flag1)
    {
      switch (series)
      {
        case StackingColumn100Series _:
        case StackingBar100Series _:
        case StackingArea100Series _:
        case WaterfallSeries _:
        case ErrorBarSeries _:
          break;
        default:
          index = (int) xVal - 1;
          goto label_4;
      }
    }
    index = series.NearestSegmentIndex - 1;
label_4:
    List<double> xvalues = series.GetXValues();
    if (xvalues == null)
      return;
    for (; double.IsNaN(yValue) && index >= 0 && xvalues.Count > index; --index)
    {
      xVal = xvalues[index];
      yValue = series.ActualSeriesYValues[0][index];
    }
    if (double.IsNaN(yValue))
      return;
    if (flag1)
    {
      switch (series)
      {
        case StackingColumn100Series _:
        case StackingBar100Series _:
        case StackingArea100Series _:
        case WaterfallSeries _:
        case ErrorBarSeries _:
          break;
        default:
          if (flag2)
          {
            double singleYvaluePosition = ChartTrackBallBehavior.GetRangeColumnSingleYValuePosition(series, xVal);
            y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, singleYvaluePosition);
            goto label_27;
          }
          y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, flag3 ? tempValue : yValue);
          goto label_27;
      }
    }
    if (series is WaterfallSeries)
    {
      if (xvalues.IndexOf(xVal) == -1)
        return;
      WaterfallSegment segment = series.Segments[xvalues.IndexOf(xVal)] as WaterfallSegment;
      if (series.Segments.IndexOf((ChartSegment) segment) == 0)
        y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, yValue);
      else if (segment.SegmentType == WaterfallSegmentType.Sum && (series as WaterfallSeries).AllowAutoSum)
      {
        yValue = segment.WaterfallSum;
        y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, yValue);
      }
      else
        y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, segment.Sum);
    }
    else
    {
      if (flag3 && k != 0.0)
        stackedYValue += yValue;
      if (flag2)
      {
        double singleYvaluePosition = ChartTrackBallBehavior.GetRangeColumnSingleYValuePosition(series, xVal);
        y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, singleYvaluePosition);
      }
      else
        y = this.ChartArea.ValueToLogPoint(series.ActualYAxis, flag3 ? (double.IsNaN(stackedYValue) ? yValue : stackedYValue) : yValue);
    }
label_27:
    x = this.ChartArea.ValueToLogPoint(series.ActualXAxis, xVal);
  }

  protected internal virtual void OnPointerPositionChanged()
  {
    if (this.ChartArea == null)
      return;
    this.isTrackBallUpdateDispatched = false;
    if (!this.IsActivated)
      return;
    this.SetPositionChangingEventArgs();
    if (this.isCancel)
      return;
    this.previousPointInfos = new ObservableCollection<ChartPointInfo>((IEnumerable<ChartPointInfo>) this.pointInfos);
    Point point1 = this.CurrentPoint;
    int num1 = 0;
    double leastX = 0.0;
    IEnumerable<IGrouping<ChartAxis, ChartSeriesBase>> groupings = this.ChartArea.VisibleSeries.GroupBy<ChartSeriesBase, ChartAxis>((Func<ChartSeriesBase, ChartAxis>) (series => series.ActualXAxis));
    this.PointInfos.Clear();
    this.axisLabels.Clear();
    int index1 = 0;
    foreach (IGrouping<ChartAxis, ChartSeriesBase> source in groupings)
    {
      ChartAxis key = source.Key;
      this.seriesCount = source.Count<ChartSeriesBase>();
      if (key != null)
      {
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        double position = 0.0;
        double tempValue = 0.0;
        this.Values.Clear();
        Rect seriesClipRect;
        foreach (ChartSeriesBase chartSeriesBase in (IEnumerable<ChartSeriesBase>) source)
        {
          ChartSeriesBase series = chartSeriesBase;
          if (!(series is CartesianSeries) || (series as CartesianSeries).ShowTrackballInfo)
          {
            bool flag1 = series is RangeColumnSeries && !series.IsMultipleYPathRequired;
            bool isGrouping = series.ActualXAxis is CategoryAxis && !(series.ActualXAxis as CategoryAxis).IsIndexed;
            if (series.IsActualTransposed)
              this.IsReversed = true;
            else
              this.IsReversed = false;
            if (series.DataCount > 0)
            {
              double x1 = 0.0;
              double y1 = 0.0;
              double stackedYValue = double.NaN;
              this.yValues = new List<double>();
              this.groupedYValues = new Dictionary<double, List<double>>();
              bool flag2 = series is StackingSeriesBase;
              FinancialTechnicalIndicator technicalIndicator = this.ChartArea.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (indicator => indicator.ItemsSource == series.ItemsSource)).Select<ChartSeries, FinancialTechnicalIndicator>((Func<ChartSeries, FinancialTechnicalIndicator>) (indicator => indicator as FinancialTechnicalIndicator)).FirstOrDefault<FinancialTechnicalIndicator>();
              if (isGrouping && !(series is StackingColumn100Series) && !(series is StackingBar100Series) && !(series is StackingArea100Series) && !(series is WaterfallSeries) && !(series is ErrorBarSeries))
              {
                point1 = this.CurrentPoint;
                double start = series.ActualXAxis.VisibleRange.Start;
                double end = series.ActualXAxis.VisibleRange.End;
                point1 = new Point(series.ActualArea.PointToValue(series.ActualXAxis, point1), series.ActualArea.PointToValue(series.ActualYAxis, point1));
                double num5 = Math.Round(point1.X);
                if (num5 <= end && num5 >= start && num5 >= 0.0)
                {
                  x1 = num5;
                  if (series.DistinctValuesIndexes.Count > 0 && series.DistinctValuesIndexes.ContainsKey(x1))
                  {
                    List<int> distinctValuesIndex = series.DistinctValuesIndexes[x1];
                    for (int index2 = 0; index2 < distinctValuesIndex.Count; ++index2)
                    {
                      this.yValues.Add(series.GroupedSeriesYValues[0][distinctValuesIndex[index2]]);
                      if (series is FinancialSeriesBase || series is RangeSeriesBase)
                      {
                        if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
                        {
                          for (int index3 = 1; index3 < ((IEnumerable<IList<double>>) series.GroupedSeriesYValues).Count<IList<double>>(); ++index3)
                          {
                            if (flag1)
                            {
                              this.yValues.Add(series.GroupedSeriesYValues[0][distinctValuesIndex[index2]]);
                              break;
                            }
                            this.yValues.Add(series.GroupedSeriesYValues[index3][distinctValuesIndex[index2]]);
                          }
                        }
                        else
                        {
                          this.groupedYValues.Add((double) index2, new List<double>());
                          for (int index4 = 0; index4 < ((IEnumerable<IList<double>>) series.GroupedSeriesYValues).Count<IList<double>>(); ++index4)
                          {
                            this.groupedYValues[(double) index2].Add(series.GroupedSeriesYValues[index4][distinctValuesIndex[index2]]);
                            if (flag1)
                              break;
                          }
                        }
                      }
                    }
                  }
                  if (flag2)
                    this.yValues.Reverse();
                }
              }
              else if (series is FinancialSeriesBase || series is RangeSeriesBase)
              {
                series.FindNearestFinancialChartPoint(technicalIndicator, point1, out x1, out this.seriesYVal, out this.indicatorYVal);
                if (series is FinancialSeriesBase && technicalIndicator != null && technicalIndicator.ShowTrackballInfo || this.LabelDisplayMode != TrackballLabelDisplayMode.NearestPoint)
                  this.AddYValues(series);
                else if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
                {
                  if (series.IsIndexed || !(series.ActualXValues is IList<double>))
                  {
                    if (this.seriesYVal.Count > 0)
                      this.yValues = this.seriesYVal;
                  }
                  else
                    this.yValues = this.GetYValuesBasedOnValue(x1, series) as List<double>;
                }
              }
              else
              {
                series.FindNearestChartPoint(point1, out x1, out y1, out stackedYValue);
                if (series is BoxAndWhiskerSeries)
                {
                  if (!double.IsNaN(x1))
                  {
                    this.seriesYVal = ChartTrackBallBehavior.GetYValuesBasedOnCollection(x1, series);
                    if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
                    {
                      this.yValues = this.seriesYVal;
                    }
                    else
                    {
                      List<double> list = this.seriesYVal.ToList<double>();
                      if (list.Count > 0)
                        list.RemoveRange(0, 5);
                      if (this.LabelHorizontalAlignment != ChartAlignment.Center)
                        list.Insert(0, this.seriesYVal[3]);
                      else
                        list.Insert(0, this.seriesYVal[4]);
                      this.yValues = list;
                    }
                  }
                }
                else if (series.IsIndexed || !(series.ActualXValues is IList<double>))
                  this.yValues.Add(y1);
                else
                  this.yValues = this.GetYValuesBasedOnValue(x1, series) as List<double>;
              }
              double logPoint1 = this.ChartArea.ValueToLogPoint(series.ActualXAxis, x1);
              for (int index5 = 0; index5 < this.yValues.Count; ++index5)
              {
                double num6 = this.yValues[index5];
                double logPoint2;
                if (isGrouping && !(series is StackingColumn100Series) && !(series is StackingBar100Series) && !(series is StackingArea100Series) && !(series is WaterfallSeries) && !(series is ErrorBarSeries))
                {
                  if (series.ActualArea.VisibleSeries.IndexOf(series) == 0)
                    tempValue = double.IsNaN(num6) ? 0.0 : num6;
                  else if (this.Values.Count > 0)
                  {
                    tempValue = num6 + this.Values[index1];
                    ++index1;
                  }
                  else
                    tempValue = num6;
                  this.Values.Add(tempValue);
                  if (flag1)
                  {
                    double singleYvaluePosition = ChartTrackBallBehavior.GetRangeColumnSingleYValuePosition(series, x1);
                    logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, singleYvaluePosition);
                  }
                  else
                    logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, flag2 ? tempValue : num6);
                }
                else if (series is WaterfallSeries)
                {
                  List<double> xvalues = series.GetXValues();
                  if (xvalues.IndexOf(x1) != -1)
                  {
                    WaterfallSegment segment = series.Segments[xvalues.IndexOf(x1)] as WaterfallSegment;
                    if (series.Segments.IndexOf((ChartSegment) segment) == 0)
                      logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, num6);
                    else if (segment.SegmentType == WaterfallSegmentType.Sum && (series as WaterfallSeries).AllowAutoSum)
                    {
                      num6 = segment.WaterfallSum;
                      logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, num6);
                    }
                    else
                      logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, segment.Sum);
                  }
                  else
                    break;
                }
                else
                {
                  if (flag2 && index5 != 0)
                    stackedYValue += num6;
                  if (flag1)
                  {
                    double singleYvaluePosition = ChartTrackBallBehavior.GetRangeColumnSingleYValuePosition(series, x1);
                    logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, singleYvaluePosition);
                  }
                  else
                    logPoint2 = this.ChartArea.ValueToLogPoint(series.ActualYAxis, flag2 ? stackedYValue : num6);
                }
                if (double.IsNaN(num6) && series.isLinearData)
                  this.ValidateTrackballNaNValueForLinearData(num6, series, stackedYValue, (double) index5, tempValue, ref logPoint1, ref logPoint2, ref x1);
                if (!double.IsNaN(logPoint1) && !double.IsNaN(logPoint2))
                {
                  if (source.ElementAt<ChartSeriesBase>(0) != series && source.ElementAt<ChartSeriesBase>(source.Count<ChartSeriesBase>() - 1) != series || series.DataCount != 1 || source.Count<ChartSeriesBase>() == 1)
                  {
                    if (num1 == 0)
                      leastX = logPoint1;
                    if (num4 == 0.0)
                    {
                      num3 = logPoint2;
                      num2 = logPoint1;
                      position = x1;
                    }
                    if (Math.Abs(leastX - point1.X) > Math.Abs(point1.X - logPoint1))
                      leastX = logPoint1;
                    if (Math.Abs(num2 - point1.X) > Math.Abs(point1.X - logPoint1))
                    {
                      num2 = logPoint1;
                      position = x1;
                    }
                    if (Math.Abs(num3 - point1.Y) > Math.Abs(num3 - logPoint2))
                      num3 = logPoint2;
                  }
                  Rect rect;
                  ref Rect local1 = ref rect;
                  seriesClipRect = this.ChartArea.SeriesClipRect;
                  double x2 = seriesClipRect.Left - 1.0;
                  double y2 = this.ChartArea.SeriesClipRect.Top - 1.0;
                  seriesClipRect = this.ChartArea.SeriesClipRect;
                  double width = seriesClipRect.Width + 2.0;
                  seriesClipRect = this.ChartArea.SeriesClipRect;
                  double height1 = seriesClipRect.Height + 2.0;
                  local1 = new Rect(x2, y2, width, height1);
                  if (this.IsReversed)
                  {
                    ref Rect local2 = ref rect;
                    double num7 = num3;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double left = seriesClipRect.Left;
                    double x3 = num7 + left;
                    double num8 = logPoint1;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double top = seriesClipRect.Top;
                    double y3 = num8 + top;
                    Point point2 = new Point(x3, y3);
                    if (!local2.Contains(point2))
                      continue;
                  }
                  else
                  {
                    ref Rect local3 = ref rect;
                    double num9 = num2;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double left = seriesClipRect.Left;
                    double x4 = num9 + left;
                    double num10 = logPoint2;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double top = seriesClipRect.Top;
                    double y4 = num10 + top;
                    Point point3 = new Point(x4, y4);
                    if (!local3.Contains(point3))
                      continue;
                  }
                  ChartPointInfo chartPointInfo1 = new ChartPointInfo();
                  ChartPointInfo chartPointInfo2 = chartPointInfo1;
                  double num11 = logPoint1;
                  seriesClipRect = this.ChartArea.SeriesClipRect;
                  double left1 = seriesClipRect.Left;
                  double num12 = num11 + left1;
                  chartPointInfo2.X = num12;
                  ChartPointInfo chartPointInfo3 = chartPointInfo1;
                  double num13 = logPoint2;
                  seriesClipRect = this.ChartArea.SeriesClipRect;
                  double top1 = seriesClipRect.Top;
                  double num14 = num13 + top1;
                  chartPointInfo3.Y = num14;
                  chartPointInfo1.Series = series;
                  if (series is BoxAndWhiskerSeries && index5 > 0 && this.LabelDisplayMode != TrackballLabelDisplayMode.NearestPoint)
                    chartPointInfo1.isOutlier = true;
                  chartPointInfo1.ValueX = !series.IsIndexed ? series.ActualXAxis.GetLabelContent(x1).ToString() : series.ActualXAxis.GetLabelContent((double) (int) x1).ToString();
                  this.labelXValue = x1.ToString();
                  chartPointInfo1.SeriesValues.Add(x1.ToString());
                  int num15;
                  if (isGrouping && series.GroupedXValuesIndexes != null && series.GroupedXValuesIndexes.Count > 0 && !(series is StackingColumn100Series) && !(series is StackingBar100Series) && !(series is StackingArea100Series) && !(series is WaterfallSeries) && !(series is ErrorBarSeries) && series.DistinctValuesIndexes.ContainsKey(x1) && !(series is WaterfallSeries))
                  {
                    if (series.IsSideBySide && !(series is RangeSeriesBase) && !(series is FinancialSeriesBase))
                    {
                      num15 = (series.ActualXAxis as CategoryAxis).AggregateFunctions == AggregateFunctions.None ? series.DistinctValuesIndexes[x1][index5] : series.GroupedXValuesIndexes.IndexOf(x1);
                      chartPointInfo1.Item = series.GroupedActualData[num15];
                    }
                    else if ((series is FinancialSeriesBase || series is RangeSeriesBase) && this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
                    {
                      int num16 = ((IEnumerable<IList<double>>) series.GroupedSeriesYValues).Count<IList<double>>();
                      int index6 = index5 / num16;
                      num15 = series.DistinctValuesIndexes[x1][index6];
                    }
                    else
                      num15 = series.DistinctValuesIndexes[x1][index5];
                  }
                  else
                  {
                    num15 = series.GetXValues().IndexOf(x1);
                    chartPointInfo1.Item = series.ActualData[num15];
                  }
                  chartPointInfo1.Interior = !(series is FinancialSeriesBase) ? series.GetInteriorColor(num15) : series.GetFinancialSeriesInterior(num15);
                  chartPointInfo1.ValueY = series.ActualYAxis.GetLabelContent(num6).ToString();
                  this.labelYValue = num6.ToString();
                  if (series is FinancialSeriesBase && !this.IsReversed)
                  {
                    this.SetPointInfoValues(series, chartPointInfo1, isGrouping, index5);
                    if (technicalIndicator != null && technicalIndicator.ShowTrackballInfo)
                      technicalIndicator.SetIndicatorInfo(chartPointInfo1, this.indicatorYVal, this.UseSeriesPalette);
                    if (((IEnumerable<IList<double>>) series.ActualSeriesYValues).Count<IList<double>>() > 0 && series.ActualSeriesYValues[0].Contains(num6))
                    {
                      int index7 = series.ActualSeriesYValues[0].IndexOf(num6);
                      for (int index8 = 0; index8 < ((IEnumerable<IList<double>>) series.ActualSeriesYValues).Count<IList<double>>(); ++index8)
                        chartPointInfo1.SeriesValues.Add(series.ActualSeriesYValues[index8][index7].ToString());
                    }
                  }
                  else if (series is RangeSeriesBase && !this.IsReversed)
                  {
                    this.SetPointInfoValues(series, chartPointInfo1, isGrouping, index5);
                    if (((IEnumerable<IList<double>>) series.ActualSeriesYValues).Count<IList<double>>() > 0 && series.ActualSeriesYValues[0].Contains(num6))
                    {
                      int index9 = series.ActualSeriesYValues[0].IndexOf(num6);
                      for (int index10 = 0; index10 < ((IEnumerable<IList<double>>) series.ActualSeriesYValues).Count<IList<double>>(); ++index10)
                      {
                        chartPointInfo1.SeriesValues.Add(series.ActualSeriesYValues[index10][index9].ToString());
                        if (flag1)
                          break;
                      }
                    }
                  }
                  else if (series is BubbleSeries && !this.IsReversed)
                  {
                    chartPointInfo1.SeriesValues.Add(num6.ToString());
                    chartPointInfo1.SeriesValues.Add((series.Segments[0] as BubbleSegment).Size.ToString());
                  }
                  else if (series is BoxAndWhiskerSeries && this.IsReversed)
                    this.SetPointInfoValues(series, chartPointInfo1, isGrouping, index5);
                  else if (this.IsReversed)
                  {
                    this.SetPointInfoValues(series, chartPointInfo1, isGrouping, index5);
                    chartPointInfo1.SeriesValues.Add(num6.ToString());
                    ChartPointInfo chartPointInfo4 = chartPointInfo1;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double height2 = seriesClipRect.Height;
                    double num17 = logPoint1;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double top2 = seriesClipRect.Top;
                    double num18 = num17 + top2;
                    double num19 = height2 - num18;
                    chartPointInfo4.X = num19;
                    ChartPointInfo chartPointInfo5 = chartPointInfo1;
                    double num20 = logPoint2;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double left2 = seriesClipRect.Left;
                    double num21 = num20 + left2;
                    chartPointInfo5.Y = num21;
                  }
                  else
                    chartPointInfo1.SeriesValues.Add(num6.ToString());
                  if (this.UseSeriesPalette)
                  {
                    chartPointInfo1.Foreground = (Brush) new SolidColorBrush(Colors.White);
                    chartPointInfo1.BorderBrush = (Brush) new SolidColorBrush(Colors.White);
                  }
                  else
                  {
                    chartPointInfo1.Interior = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallBackground"] as SolidColorBrush);
                    chartPointInfo1.Foreground = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallForeground"] as SolidColorBrush);
                    chartPointInfo1.BorderBrush = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallBorderBrush"] as SolidColorBrush);
                  }
                  if (this.IsReversed)
                  {
                    double x5 = chartPointInfo1.X;
                    chartPointInfo1.BaseX = chartPointInfo1.Y;
                    ChartPointInfo chartPointInfo6 = chartPointInfo1;
                    seriesClipRect = this.ChartArea.SeriesClipRect;
                    double num22 = seriesClipRect.Height - x5;
                    chartPointInfo6.BaseY = num22;
                    chartPointInfo1.HorizontalAlignment = this.LabelVerticalAlignment == ChartAlignment.Auto ? (chartPointInfo1.Series is FinancialSeriesBase || chartPointInfo1.Series is RangeSeriesBase ? ChartAlignment.Near : ChartAlignment.Center) : this.LabelVerticalAlignment;
                    chartPointInfo1.VerticalAlignment = this.LabelHorizontalAlignment == ChartAlignment.Auto ? (chartPointInfo1.Series is FinancialSeriesBase || chartPointInfo1.Series is RangeSeriesBase ? ChartAlignment.Center : ChartAlignment.Far) : this.LabelHorizontalAlignment;
                  }
                  else
                  {
                    chartPointInfo1.BaseX = chartPointInfo1.X;
                    chartPointInfo1.BaseY = chartPointInfo1.Y;
                    chartPointInfo1.HorizontalAlignment = this.LabelHorizontalAlignment == ChartAlignment.Auto ? (chartPointInfo1.Series is FinancialSeriesBase || chartPointInfo1.Series is RangeSeriesBase ? ChartAlignment.Center : ChartAlignment.Far) : this.LabelHorizontalAlignment;
                    chartPointInfo1.VerticalAlignment = this.LabelVerticalAlignment == ChartAlignment.Auto ? (chartPointInfo1.Series is FinancialSeriesBase || chartPointInfo1.Series is RangeSeriesBase ? ChartAlignment.Near : ChartAlignment.Center) : this.LabelVerticalAlignment;
                  }
                  this.PointInfos.Add(chartPointInfo1);
                  if (this.ChartArea.Series.Count > 1 && this.ChartArea.Series.All<ChartSeries>((Func<ChartSeries, bool>) (chartSeries => chartSeries.DataCount == 1)))
                  {
                    position = x1;
                    leastX = num2 = logPoint1;
                  }
                  if (source.ElementAt<ChartSeriesBase>(0) != series && source.ElementAt<ChartSeriesBase>(source.Count<ChartSeriesBase>() - 1) != series || series.DataCount != 1 || source.Count<ChartSeriesBase>() == 1)
                  {
                    ++num1;
                    ++num4;
                  }
                }
              }
            }
          }
        }
        if (this.seriesCount > 1)
          this.pointInfos = !this.IsReversed ? new ObservableCollection<ChartPointInfo>(this.PointInfos.Where<ChartPointInfo>((Func<ChartPointInfo, bool>) (info => Math.Abs(info.X - (leastX + this.ChartArea.SeriesClipRect.Left)) < 0.0001))) : new ObservableCollection<ChartPointInfo>(this.PointInfos.Where<ChartPointInfo>((Func<ChartPointInfo, bool>) (info => info.X == this.ChartArea.SeriesClipRect.Height - (leastX + this.ChartArea.SeriesClipRect.Top))));
        if (this.PointInfos.Count != 0)
        {
          ChartPointInfo chartPointInfo7 = new ChartPointInfo();
          chartPointInfo7.Axis = key;
          if (this.IsReversed)
          {
            if (key.Orientation == Orientation.Vertical)
            {
              chartPointInfo7.ValueX = this.ChartArea.VisibleSeries.Count <= 0 || !this.ChartArea.VisibleSeries[0].IsIndexed ? key.GetLabelContent(position).ToString() : key.GetLabelContent((double) (int) position).ToString();
              if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
              {
                foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
                {
                  if (pointInfo.Y == this.GetNearestYValue())
                  {
                    chartPointInfo7.X = pointInfo.X;
                    chartPointInfo7.ValueX = pointInfo.ValueX;
                    this.currentYLabel = pointInfo.ValueY;
                  }
                }
              }
              else
              {
                ChartPointInfo chartPointInfo8 = chartPointInfo7;
                seriesClipRect = this.ChartArea.SeriesClipRect;
                double height = seriesClipRect.Height;
                double num23 = num2;
                seriesClipRect = this.ChartArea.SeriesClipRect;
                double top = seriesClipRect.Top;
                double num24 = num23 + top;
                double num25 = height - num24;
                chartPointInfo8.X = num25;
              }
            }
            else
            {
              chartPointInfo7.ValueY = key.GetLabelContent(position).ToString();
              chartPointInfo7.Y = point1.Y;
            }
          }
          else if (key.Orientation == Orientation.Horizontal)
          {
            chartPointInfo7.ValueX = this.ChartArea.VisibleSeries.Count <= 0 || !this.ChartArea.VisibleSeries[0].IsIndexed ? key.GetLabelContent(position).ToString() : key.GetLabelContent((double) (int) position).ToString();
            if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint)
            {
              foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
              {
                if (pointInfo.Y == this.GetNearestYValue())
                {
                  chartPointInfo7.X = pointInfo.X;
                  chartPointInfo7.ValueX = pointInfo.ValueX;
                  this.currentYLabel = pointInfo.ValueY;
                }
              }
            }
            else
              chartPointInfo7.X = num2 + this.ChartArea.SeriesClipRect.Left;
          }
          else
          {
            chartPointInfo7.ValueY = key.GetLabelContent(position).ToString();
            chartPointInfo7.Y = point1.Y;
          }
          this.currentXLabel = chartPointInfo7.ValueX;
          ChartTrackBallBehavior.ApplyDefaultBrushes((object) chartPointInfo7);
          if (this.UseSeriesPalette)
          {
            chartPointInfo7.Interior = (Brush) new SolidColorBrush(Colors.Black);
            chartPointInfo7.Foreground = (Brush) new SolidColorBrush(Colors.White);
            chartPointInfo7.BorderBrush = (Brush) new SolidColorBrush(Colors.White);
          }
          if (this.IsReversed)
          {
            double x = chartPointInfo7.X;
            chartPointInfo7.BaseX = chartPointInfo7.Y;
            ChartPointInfo chartPointInfo9 = chartPointInfo7;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double num26 = seriesClipRect.Height - x;
            chartPointInfo9.BaseY = num26;
          }
          else
          {
            chartPointInfo7.BaseX = chartPointInfo7.X;
            chartPointInfo7.BaseY = chartPointInfo7.Y;
          }
          this.isOpposedAxis = key.OpposedPosition;
          this.axisLabels.Add(key, chartPointInfo7);
        }
      }
    }
    if (this.previousXLabel == this.currentXLabel && this.tempXPos == leastX)
    {
      if (this.LabelDisplayMode != TrackballLabelDisplayMode.NearestPoint || !(this.previousYLabel != this.currentYLabel) || this.ChartArea.VisibleSeries.Count <= 1)
        return;
      this.previousYLabel = this.currentYLabel;
    }
    else
      this.previousXLabel = this.currentXLabel;
    this.tempXPos = leastX;
    this.ClearItems();
    this.GenerateAxisLabels();
    this.GenerateTrackBalls();
    if (this.trackBalls.Count > 0)
      this.trackballWidth = this.trackBalls[0].Width;
    double num27 = leastX + this.ChartArea.SeriesClipRect.Left;
    if (this.IsReversed && this.pointInfos.Count > 0)
    {
      double num28 = leastX + this.ChartArea.SeriesClipRect.Top;
      this.line.X1 = this.ChartArea.SeriesClipRect.Left;
      Line line = this.line;
      Rect seriesClipRect = this.ChartArea.SeriesClipRect;
      double left = seriesClipRect.Left;
      seriesClipRect = this.ChartArea.SeriesClipRect;
      double width = seriesClipRect.Width;
      double num29 = left + width;
      line.X2 = num29;
      this.line.Y2 = this.line.Y1 = num28;
    }
    else if (this.pointInfos.Count > 0)
    {
      this.line.Y1 = this.ChartArea.SeriesClipRect.Top;
      Line line = this.line;
      Rect seriesClipRect = this.ChartArea.SeriesClipRect;
      double top = seriesClipRect.Top;
      seriesClipRect = this.ChartArea.SeriesClipRect;
      double height = seriesClipRect.Height;
      double num30 = top + height;
      line.Y2 = num30;
      this.line.X1 = this.line.X2 = num27;
    }
    this.GenerateLabels();
    this.elements.Add((FrameworkElement) this.line);
    if (this.labelElements != null && this.labelElements.Count > 1 && this.labelElements[0].Content is ChartPointInfo)
    {
      this.labelElements = this.IsReversed ? new List<ContentControl>((IEnumerable<ContentControl>) this.labelElements.OrderBy<ContentControl, double>((Func<ContentControl, double>) (x => (x.Content as ChartPointInfo).X))) : new List<ContentControl>((IEnumerable<ContentControl>) this.labelElements.OrderByDescending<ContentControl, double>((Func<ContentControl, double>) (x => (x.Content as ChartPointInfo).Y)));
      this.SmartAlignLabels();
      this.RenderSeriesBeakForSmartAlignment();
    }
    this.SetPositionChangedEventArgs();
  }

  private static double GetRangeColumnSingleYValuePosition(ChartSeriesBase series, double xValue)
  {
    RangeSeriesBase rangeSeriesBase = series as RangeSeriesBase;
    int index = series.GetXValues().IndexOf(xValue);
    if (index < 0)
      return double.NaN;
    DoubleRange visibleRange = series.ActualYAxis.VisibleRange;
    return (visibleRange.End - Math.Abs(visibleRange.Start)) / 2.0 + (rangeSeriesBase.High == null ? rangeSeriesBase.LowValues[index] : rangeSeriesBase.HighValues[index]) / 2.0;
  }

  protected internal override void DetachElements()
  {
    if (this.AdorningCanvas == null)
      return;
    foreach (UIElement element in this.elements)
      this.AdorningCanvas.Children.Remove(element);
  }

  protected internal override void OnSizeChanged(SizeChangedEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    double logPoint1 = this.ChartArea.ValueToLogPoint(this.ChartArea.InternalSecondaryAxis, Convert.ToDouble(this.labelYValue));
    double logPoint2 = this.ChartArea.ValueToLogPoint(this.ChartArea.InternalPrimaryAxis, Convert.ToDouble(this.labelXValue));
    if (double.IsNaN(logPoint1) || double.IsNaN(logPoint2))
      return;
    foreach (ContentControl labelElement in this.labelElements)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) labelElement))
        this.AdorningCanvas.Children.Remove((UIElement) labelElement);
    }
    foreach (ContentControl axisLabelElement in this.axisLabelElements)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) axisLabelElement))
        this.AdorningCanvas.Children.Remove((UIElement) axisLabelElement);
    }
    foreach (Control trackBall in this.trackBalls)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) trackBall))
        this.AdorningCanvas.Children.Remove((UIElement) trackBall);
    }
    this.PointInfos.Clear();
    this.labelElements.Clear();
    this.axisLabelElements.Clear();
    this.trackBalls.Clear();
    this.axisLabels.Clear();
    this.elements.Clear();
    this.CurrentPoint = new Point(logPoint2, logPoint1);
    this.OnPointerPositionChanged();
    this.AdorningCanvas.ClipToBounds = true;
  }

  protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.isLeftButtonPressed = true;
    this.IsActivated = false;
  }

  protected internal override void OnLayoutUpdated()
  {
    if (!this.IsActivated)
      return;
    this.ScheduleTrackBallUpdate();
  }

  protected internal override void OnMouseMove(MouseEventArgs e)
  {
    if (!this.isLeftButtonPressed)
      this.IsActivated = true;
    if (this.ChartArea == null || this.ChartArea.AreaType != ChartAreaType.CartesianAxes || !this.IsActivated)
      return;
    Point point = e.GetPosition((IInputElement) this.AdorningCanvas);
    if (!this.ChartArea.SeriesClipRect.Contains(point))
      return;
    point = new Point(point.X - this.ChartArea.SeriesClipRect.Left, point.Y - this.ChartArea.SeriesClipRect.Top);
    if (this.CurrentPoint.X == point.X && this.CurrentPoint.Y == point.Y)
      return;
    this.CurrentPoint = point;
    this.ScheduleTrackBallUpdate();
  }

  protected internal override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.ChartArea == null || !this.IsActivated)
      return;
    this.IsActivated = false;
    this.ChartArea.HoldUpdate = false;
  }

  protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    if (this.IsActivated)
    {
      this.IsActivated = false;
      this.ChartArea.HoldUpdate = false;
    }
    this.isLeftButtonPressed = false;
  }

  protected internal override void AlignDefaultLabel(
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    double x,
    double y,
    ContentControl control)
  {
    if (control == null || double.IsInfinity(x) || double.IsInfinity(y) || double.IsNaN(x) || double.IsNaN(y))
      return;
    if (horizontalAlignment == ChartAlignment.Far && control != null)
    {
      if ((control.Content as ChartPointInfo).Series != null)
        x += this.trackballWidth * 0.75 + 6.0 - 2.0;
      (control.Content as ChartPointInfo).X = x;
    }
    switch (horizontalAlignment)
    {
      case ChartAlignment.Near:
        x -= control.DesiredSize.Width;
        if (control != null)
        {
          if ((control.Content as ChartPointInfo).Series != null)
            x -= this.trackballWidth * 0.75 + 6.0 - 2.0;
          (control.Content as ChartPointInfo).X = x;
          break;
        }
        break;
      case ChartAlignment.Center:
        x -= control.DesiredSize.Width / 2.0;
        if (control != null)
        {
          (control.Content as ChartPointInfo).X = x;
          break;
        }
        break;
    }
    if (verticalAlignemnt == ChartAlignment.Far && control != null)
    {
      if ((control.Content as ChartPointInfo).Series != null)
        y += this.trackballWidth * 0.75 + 6.0;
      (control.Content as ChartPointInfo).Y = y;
    }
    switch (verticalAlignemnt)
    {
      case ChartAlignment.Near:
        y -= control.DesiredSize.Height;
        if (control != null)
        {
          if ((control.Content as ChartPointInfo).Series != null)
            y -= this.trackballWidth * 0.75 + 6.0;
          (control.Content as ChartPointInfo).Y = y;
          break;
        }
        break;
      case ChartAlignment.Center:
        y -= control.DesiredSize.Height / 2.0;
        if (control != null)
        {
          (control.Content as ChartPointInfo).Y = y;
          break;
        }
        break;
    }
    Canvas.SetLeft((UIElement) control, x);
    Canvas.SetTop((UIElement) control, y);
  }

  protected override void AttachElements()
  {
    this.line.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("LineStyle", new object[0]),
      Source = (object) this
    });
    if (this.AdorningCanvas == null || this.AdorningCanvas.Children.Contains((UIElement) this.line) || !this.ShowLine)
      return;
    this.AdorningCanvas.Children.Add((UIElement) this.line);
    this.elements.Add((FrameworkElement) this.line);
  }

  protected override DependencyObject CloneBehavior(DependencyObject obj)
  {
    return base.CloneBehavior((DependencyObject) new ChartTrackBallBehavior()
    {
      CurrentPoint = this.CurrentPoint,
      AxisLabelAlignment = this.AxisLabelAlignment,
      LabelHorizontalAlignment = this.LabelHorizontalAlignment,
      LabelVerticalAlignment = this.LabelVerticalAlignment,
      ChartTrackBallStyle = this.ChartTrackBallStyle,
      LineStyle = this.LineStyle,
      UseSeriesPalette = this.UseSeriesPalette,
      LabelDisplayMode = this.LabelDisplayMode,
      ShowLine = this.ShowLine
    });
  }

  protected virtual void OnPointerPositionChanged(Point point)
  {
    this.CurrentPoint = point;
    this.OnPointerPositionChanged();
  }

  protected virtual void GenerateLabels()
  {
    if (this.PointInfos.Count == 0)
      return;
    if (this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
    {
      this.RearrangeStackingSeriesInfo();
      this.AddGroupLabels();
    }
    else
    {
      foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
      {
        if (pointInfo.Series.IsActualTransposed ? this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo.Y, pointInfo.X)) : this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo.X, pointInfo.Y)))
        {
          if (this.LabelDisplayMode == TrackballLabelDisplayMode.FloatAllPoints)
          {
            if (this.seriesCount > 1 && this.PointInfos.Any<ChartPointInfo>((Func<ChartPointInfo, bool>) (info => info.Series.IsSideBySide)))
            {
              this.AddGroupLabels();
              break;
            }
            this.AddLabel(pointInfo, pointInfo.VerticalAlignment, pointInfo.HorizontalAlignment, this.GetLabelTemplate(pointInfo));
          }
          else if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint && pointInfo.Y == this.GetNearestYValue())
          {
            if (this.IsReversed)
            {
              Line line1 = this.line;
              Line line2 = this.line;
              Rect seriesClipRect = this.ChartArea.SeriesClipRect;
              double num1;
              double num2 = num1 = seriesClipRect.Height - pointInfo.X;
              line2.Y2 = num1;
              double num3 = num2;
              line1.Y1 = num3;
            }
            else
              this.line.X1 = this.line.X2 = pointInfo.X;
            this.AddLabel(pointInfo, pointInfo.VerticalAlignment, pointInfo.HorizontalAlignment, this.GetLabelTemplate(pointInfo));
            break;
          }
        }
      }
    }
  }

  private void RearrangeStackingSeriesInfo()
  {
    if (this.pointInfos.Count == 0)
      return;
    bool flag = false;
    ObservableCollection<ChartPointInfo> observableCollection = new ObservableCollection<ChartPointInfo>();
    foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
    {
      if (pointInfo.Series is StackingSeriesBase && pointInfo.Series.ActualYAxis != null && !pointInfo.Series.ActualYAxis.IsInversed)
      {
        observableCollection.Insert(0, pointInfo);
        flag = true;
      }
      else
        observableCollection.Insert(observableCollection.Count == 0 ? 0 : observableCollection.Count, pointInfo);
    }
    if (!flag)
      return;
    this.PointInfos = observableCollection;
  }

  protected virtual void AddGroupLabels()
  {
    foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
    {
      ContentControl contentControl = new ContentControl();
      contentControl.Content = (object) pointInfo;
      pointInfo.Foreground = this.UseSeriesPalette ? pointInfo.Interior : pointInfo.Foreground;
      contentControl.ContentTemplate = pointInfo.Series is FinancialSeriesBase || pointInfo.Series is RangeSeriesBase || pointInfo.Series is BoxAndWhiskerSeries ? this.GetLabelTemplate(pointInfo) : (pointInfo.Series.GetTrackballTemplate() != null && this.UseSeriesPalette && pointInfo.Series.Tag != null && pointInfo.Series.Tag.Equals((object) "FromTheme") || pointInfo.Series.GetTrackballTemplate() == null ? ChartDictionaries.GenericCommonDictionary[(object) "groupLabel"] as DataTemplate : pointInfo.Series.GetTrackballTemplate());
      this.groupLabelElements.Add(contentControl);
    }
    StackPanel stackPanel = new StackPanel()
    {
      Orientation = Orientation.Vertical
    };
    stackPanel.Margin = new Thickness().GetThickness(3.0, 0.0, 3.0, 0.0);
    foreach (ContentControl groupLabelElement in this.groupLabelElements)
    {
      stackPanel.Children.Add((UIElement) groupLabelElement);
      if (this.groupLabelElements.Count > 1 && groupLabelElement != this.groupLabelElements.Last<ContentControl>())
      {
        Rectangle element = new Rectangle();
        element.Fill = (Brush) new SolidColorBrush(Colors.Gray);
        element.StrokeThickness = 1.0;
        element.Height = 0.5;
        groupLabelElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        element.Width = groupLabelElement.Width;
        stackPanel.Children.Add((UIElement) element);
      }
    }
    this.border = new Border();
    this.border.BorderBrush = (Brush) new SolidColorBrush(Colors.Black);
    this.border.BorderThickness = new Thickness().GetThickness(1.0, 1.0, 1.0, 1.0);
    this.border.Background = this.LabelBackground;
    this.border.CornerRadius = new CornerRadius(1.0);
    this.border.Child = (UIElement) stackPanel;
    this.AddElement((UIElement) this.border);
    this.ArrangeGroupLabel();
  }

  protected virtual void AddLabel(
    object obj,
    ChartAlignment verticalAlignment,
    ChartAlignment horizontalAlignment,
    DataTemplate labelTemplate,
    double x,
    double y)
  {
    if (labelTemplate == null)
      return;
    ContentControl contentControl = new ContentControl();
    contentControl.Content = obj;
    contentControl.IsHitTestVisible = false;
    contentControl.ContentTemplate = labelTemplate;
    this.AddElement((UIElement) contentControl);
    contentControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    ChartPointInfo content = contentControl.Content as ChartPointInfo;
    if (ChartTrackBallBehavior.CanApplyDefaultTemplate(obj))
    {
      if (content == null)
        return;
      if (content.Series != null)
      {
        this.labelElements.Add(contentControl);
        this.AlignDefaultLabel(verticalAlignment, horizontalAlignment, x, y, contentControl);
        this.ArrangeLabelsOnBounds(contentControl, true);
        this.AlignSeriesToolTipPolygon(contentControl);
      }
      else
      {
        if (content.Axis == null)
          return;
        this.axisLabelElements.Add(contentControl);
        if (this.IsReversed)
          this.AlignDefaultLabel(horizontalAlignment, verticalAlignment, y, this.ChartArea.SeriesClipRect.Height - x, contentControl);
        else
          this.AlignDefaultLabel(verticalAlignment, horizontalAlignment, x, y, contentControl);
        this.AlignAxisToolTipPolygon(contentControl, verticalAlignment, horizontalAlignment, x, y, (ChartBehavior) this);
      }
    }
    else if (content == null || content != null && content.Series == null && content.Axis == null)
    {
      this.labelElements.Add(contentControl);
      if (this.IsReversed)
        ChartTrackBallBehavior.AlignElement((Control) contentControl, horizontalAlignment, verticalAlignment, y, this.ChartArea.SeriesClipRect.Height - x);
      else
        ChartTrackBallBehavior.AlignElement((Control) contentControl, verticalAlignment, horizontalAlignment, x, y);
    }
    else
    {
      if (content == null)
        return;
      if (content.Series != null)
      {
        this.labelElements.Add(contentControl);
        ChartTrackBallBehavior.AlignElement((Control) contentControl, verticalAlignment, horizontalAlignment, x, y);
        this.ArrangeLabelsOnBounds(contentControl, false);
        this.AlignSeriesToolTipPolygon(contentControl);
      }
      else
      {
        if (content.Axis == null)
          return;
        this.axisLabelElements.Add(contentControl);
        if (this.IsReversed)
          ChartTrackBallBehavior.AlignElement((Control) contentControl, horizontalAlignment, verticalAlignment, y, this.ChartArea.SeriesClipRect.Height - x);
        else
          ChartTrackBallBehavior.AlignElement((Control) contentControl, verticalAlignment, horizontalAlignment, x, y);
        this.AlignAxisToolTipPolygon(contentControl, verticalAlignment, horizontalAlignment, x, y, (ChartBehavior) this);
      }
    }
  }

  protected virtual void GenerateTrackBalls()
  {
    foreach (ChartPointInfo pointInfo in (Collection<ChartPointInfo>) this.PointInfos)
    {
      if (pointInfo.Series.IsActualTransposed ? this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo.Y, pointInfo.X)) : this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo.X, pointInfo.Y)))
      {
        if (this.LabelDisplayMode == TrackballLabelDisplayMode.FloatAllPoints || this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
        {
          if (pointInfo.Series is FinancialSeriesBase || pointInfo.Series is RangeSeriesBase)
            this.GenerateAdditionalTrackball(pointInfo);
          else
            this.CallTrackball(pointInfo);
        }
        else if (this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint && pointInfo.Y == this.GetNearestYValue())
        {
          this.CallTrackball(pointInfo);
          break;
        }
      }
    }
  }

  internal void CallTrackball(ChartPointInfo pointInfo) => this.AddTrackBall(pointInfo);

  protected virtual void AddTrackBall(ChartPointInfo pointInfo)
  {
    ChartTrackBallControl element = new ChartTrackBallControl(pointInfo.Series);
    element.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ChartTrackBallStyle", new object[0])
    });
    this.trackBalls.Add(element);
    this.AddElement((UIElement) element);
    element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.IsReversed)
      ChartTrackBallBehavior.AlignElement((Control) element, ChartAlignment.Center, ChartAlignment.Center, pointInfo.Y, this.ChartArea.SeriesClipRect.Height - pointInfo.X);
    else
      ChartTrackBallBehavior.AlignElement((Control) element, ChartAlignment.Center, ChartAlignment.Center, pointInfo.X, pointInfo.Y);
  }

  protected IList<double> GetYValuesBasedOnValue(double x, ChartSeriesBase series)
  {
    List<double> yvaluesBasedOnValue = new List<double>();
    List<double> actualXvalues = series.ActualXValues as List<double>;
    for (int index = 0; index < series.DataCount; ++index)
    {
      if (actualXvalues[index] == x)
      {
        foreach (IList<double> actualSeriesYvalue in series.ActualSeriesYValues)
        {
          yvaluesBasedOnValue.Add(actualSeriesYvalue[index]);
          if (series is RangeSeriesBase rangeSeriesBase && rangeSeriesBase is RangeColumnSeries && (string.IsNullOrEmpty(rangeSeriesBase.High) || string.IsNullOrEmpty(rangeSeriesBase.Low)))
            break;
        }
      }
    }
    return (IList<double>) yvaluesBasedOnValue;
  }

  protected void ClearItems()
  {
    foreach (ContentControl labelElement in this.labelElements)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) labelElement))
        this.AdorningCanvas.Children.Remove((UIElement) labelElement);
    }
    foreach (ContentControl axisLabelElement in this.axisLabelElements)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) axisLabelElement))
        this.AdorningCanvas.Children.Remove((UIElement) axisLabelElement);
    }
    foreach (Control trackBall in this.trackBalls)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) trackBall))
        this.AdorningCanvas.Children.Remove((UIElement) trackBall);
    }
    foreach (ContentControl groupLabelElement in this.groupLabelElements)
    {
      if (this.AdorningCanvas.Children.Contains((UIElement) groupLabelElement))
        this.AdorningCanvas.Children.Remove((UIElement) groupLabelElement);
    }
    if (this.AdorningCanvas != null && this.AdorningCanvas.Children.Contains((UIElement) this.border))
      this.AdorningCanvas.Children.Remove((UIElement) this.border);
    this.groupLabelElements.Clear();
    this.labelElements.Clear();
    this.axisLabelElements.Clear();
    this.trackBalls.Clear();
    this.line.ClearUIValues();
    this.elements.Clear();
  }

  protected void AddLabel(
    ChartPointInfo obj,
    ChartAlignment verticalAlignment,
    ChartAlignment horizontalAlignment,
    DataTemplate template)
  {
    if (obj == null || template == null)
      return;
    if (obj.Series == null)
      this.AddLabel((object) obj, verticalAlignment, horizontalAlignment, template, obj.X, obj.Y);
    else
      this.AddLabel((object) obj, verticalAlignment, horizontalAlignment, template, obj.BaseX, obj.BaseY);
  }

  protected void AddElement(UIElement element)
  {
    if (this.AdorningCanvas.Children.Contains(element))
      return;
    this.AdorningCanvas.Children.Add(element);
    this.elements.Add(element as FrameworkElement);
  }

  private static void OnShowLinePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartTrackBallBehavior).OnShowLinePropertyChanged(e);
  }

  private static void OnLayoutUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartTrackBallBehavior).OnLayoutUpdated();
  }

  private static List<double> GetYValuesBasedOnCollection(double x, ChartSeriesBase series)
  {
    List<double> labelValues = new List<double>();
    List<double> actualXvalues = series.ActualXValues as List<double>;
    BoxAndWhiskerSeries andWhiskerSeries = series as BoxAndWhiskerSeries;
    IEnumerable<ChartSegment> chartSegments = series.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is BoxAndWhiskerSegment));
    int index = 0;
    if (andWhiskerSeries.IsIndexed || actualXvalues == null)
    {
      foreach (BoxAndWhiskerSegment boxWhiskerSegment in chartSegments)
      {
        if ((double) index == x)
        {
          ChartTrackBallBehavior.GetSegmentValues(boxWhiskerSegment, labelValues);
          break;
        }
        ++index;
      }
    }
    else
    {
      foreach (BoxAndWhiskerSegment boxWhiskerSegment in chartSegments)
      {
        if (actualXvalues[index] == x)
        {
          ChartTrackBallBehavior.GetSegmentValues(boxWhiskerSegment, labelValues);
          break;
        }
        ++index;
      }
    }
    return labelValues;
  }

  private static void GetSegmentValues(
    BoxAndWhiskerSegment boxWhiskerSegment,
    List<double> labelValues)
  {
    labelValues.Add(boxWhiskerSegment.Median);
    labelValues.Add(boxWhiskerSegment.LowerQuartile);
    labelValues.Add(boxWhiskerSegment.Minimum);
    labelValues.Add(boxWhiskerSegment.UppperQuartile);
    labelValues.Add(boxWhiskerSegment.Maximum);
    foreach (double outlier in boxWhiskerSegment.Outliers)
      labelValues.Add(outlier);
  }

  private static ChartAlignment GetChartAlignment(bool isOpposed, ChartAlignment alignment)
  {
    if (!isOpposed)
      return alignment;
    if (alignment == ChartAlignment.Near)
      return ChartAlignment.Far;
    return alignment == ChartAlignment.Far ? ChartAlignment.Near : ChartAlignment.Center;
  }

  private static bool CheckLabelCollision(ContentControl previousLabel, ContentControl currentLabel)
  {
    return ChartTrackBallBehavior.CheckLabelCollisionRect(ChartTrackBallBehavior.GetRenderedRect(previousLabel), ChartTrackBallBehavior.GetRenderedRect(currentLabel));
  }

  private static bool CheckLabelCollisionRect(Rect rect1, Rect rect2)
  {
    return Math.Round(rect1.Y + rect1.Height, 2) > Math.Round(rect2.Y, 2) && Math.Round(rect1.Y, 2) < Math.Round(rect2.Y + rect2.Height, 2) && Math.Round(rect1.X + rect1.Width, 2) > Math.Round(rect2.X, 2) && Math.Round(rect1.X, 2) < Math.Round(rect2.X + rect2.Width);
  }

  private static Rect GetRenderedRect(ContentControl control)
  {
    ChartPointInfo content = control.Content as ChartPointInfo;
    return new Rect(content.X, content.Y, control.DesiredSize.Width, control.DesiredSize.Height);
  }

  private static bool CanApplyDefaultTemplate(object obj)
  {
    return obj is ChartPointInfo chartPointInfo && chartPointInfo.Series != null && (chartPointInfo.Series.GetTrackballTemplate() != null && chartPointInfo.Series.Tag != null && chartPointInfo.Series.Tag.Equals((object) "FromTheme") || chartPointInfo.Series.GetTrackballTemplate() == null) || chartPointInfo != null && chartPointInfo.Axis != null && (chartPointInfo.Axis.GetTrackBallTemplate() != null && chartPointInfo.Axis.Tag != null && chartPointInfo.Axis.Tag.Equals((object) "FromTheme") || chartPointInfo.Axis.GetTrackBallTemplate() == null);
  }

  private static void ApplyDefaultBrushes(object obj)
  {
    ChartPointInfo chartPointInfo = obj as ChartPointInfo;
    chartPointInfo.Interior = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallBackground"] as SolidColorBrush);
    chartPointInfo.Foreground = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallForeground"] as SolidColorBrush);
    chartPointInfo.BorderBrush = (Brush) (ChartDictionaries.GenericCommonDictionary[(object) "trackBallBorderBrush"] as SolidColorBrush);
  }

  private static void AlignElement(
    Control control,
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    double x,
    double y)
  {
    if (control == null || double.IsInfinity(x) || double.IsInfinity(y) || double.IsNaN(x) || double.IsNaN(y))
      return;
    switch (horizontalAlignment)
    {
      case ChartAlignment.Near:
        x -= control.DesiredSize.Width;
        break;
      case ChartAlignment.Center:
        x -= control.DesiredSize.Width / 2.0;
        break;
    }
    switch (verticalAlignemnt)
    {
      case ChartAlignment.Near:
        y -= control.DesiredSize.Height;
        break;
      case ChartAlignment.Center:
        y -= control.DesiredSize.Height / 2.0;
        break;
    }
    if (control is ContentControl contentControl && contentControl.Content is ChartPointInfo content)
    {
      content.X = x;
      content.Y = y;
    }
    Canvas.SetLeft((UIElement) control, x);
    Canvas.SetTop((UIElement) control, y);
  }

  private void OnShowLinePropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == null)
      return;
    if ((bool) e.NewValue)
    {
      this.AttachElements();
    }
    else
    {
      if (this.AdorningCanvas == null)
        return;
      this.DetachElement((UIElement) this.line);
      this.elements.Remove((FrameworkElement) this.line);
    }
  }

  private void SetPositionChangedEventArgs()
  {
    PositionChangedEventArgs e = new PositionChangedEventArgs();
    e.CurrentPointInfos = this.PointInfos;
    e.PreviousPointInfos = this.previousPointInfos;
    if (this.PositionChanged == null)
      return;
    this.PositionChanged((object) this, e);
  }

  private void SetPositionChangingEventArgs()
  {
    PositionChangingEventArgs e = new PositionChangingEventArgs();
    e.PointInfos = this.PointInfos;
    if (this.PositionChanging != null)
      this.PositionChanging((object) this, e);
    this.isCancel = e.Cancel;
  }

  private void SetPointInfoValues(
    ChartSeriesBase series,
    ChartPointInfo pointInfo,
    bool isGrouping,
    int yCount)
  {
    bool flag = series is FinancialSeriesBase;
    if (flag || series is RangeSeriesBase)
    {
      if (series is RangeColumnSeries && !series.IsMultipleYPathRequired)
      {
        pointInfo.ValueY = this.yValues[0].ToString();
      }
      else
      {
        pointInfo.High = isGrouping ? (yCount < this.groupedYValues.Values.Count<List<double>>() ? this.groupedYValues[(double) yCount][0].ToString() : (string) null) : this.seriesYVal[0].ToString();
        pointInfo.Low = isGrouping ? (yCount < this.groupedYValues.Values.Count<List<double>>() ? this.groupedYValues[(double) yCount][1].ToString() : (string) null) : this.seriesYVal[1].ToString();
      }
      if (flag)
      {
        pointInfo.Open = isGrouping ? (yCount < this.groupedYValues.Values.Count<List<double>>() ? this.groupedYValues[(double) yCount][2].ToString() : (string) null) : this.seriesYVal[2].ToString();
        pointInfo.Close = isGrouping ? (yCount < this.groupedYValues.Values.Count<List<double>>() ? this.groupedYValues[(double) yCount][3].ToString() : (string) null) : this.seriesYVal[3].ToString();
      }
    }
    if (!(series is BoxAndWhiskerSeries) || this.seriesYVal.Count <= 0)
      return;
    pointInfo.Median = this.seriesYVal[0].ToString();
    pointInfo.Open = this.seriesYVal[1].ToString();
    pointInfo.Close = this.seriesYVal[3].ToString();
    pointInfo.Low = this.seriesYVal[2].ToString();
    pointInfo.High = this.seriesYVal[4].ToString();
  }

  private void AddYValues(ChartSeriesBase series)
  {
    if (this.seriesYVal.Count <= 0)
      return;
    if (this.LabelHorizontalAlignment != ChartAlignment.Center && series is FinancialSeriesBase)
    {
      if (this.seriesYVal[2] > this.seriesYVal[3])
        this.yValues.Add(this.seriesYVal[2]);
      else
        this.yValues.Add(this.seriesYVal[3]);
    }
    else
      this.yValues.Add(this.seriesYVal[0]);
  }

  private void RenderSeriesBeakForSmartAlignment()
  {
    foreach (ContentControl labelElement in this.labelElements)
      this.AlignSeriesToolTipPolygon(labelElement);
  }

  private void ArrangeLabelsOnBounds(ContentControl label, bool withPolygon)
  {
    bool flag1 = false;
    bool flag2 = false;
    ChartPointInfo content = label.Content as ChartPointInfo;
    double num1 = (this.trackballWidth * 0.75 + 6.0 - 2.0) * 2.0;
    double num2 = (this.trackballWidth * 0.75 + 6.0) * 2.0;
    double x = content.X;
    double y = content.Y;
    if (this.ChartArea.SeriesClipRect.Left > content.X)
      flag1 = true;
    if (this.ChartArea.SeriesClipRect.Right < content.X + label.DesiredSize.Width)
      flag2 = true;
    if (this.ChartArea.SeriesClipRect.Left > x)
    {
      if (content.HorizontalAlignment == ChartAlignment.Center)
        content.X += withPolygon ? (label.DesiredSize.Width + num1) / 2.0 : label.DesiredSize.Width / 2.0;
      else
        content.X += withPolygon ? label.DesiredSize.Width + num1 : label.DesiredSize.Width;
      x = content.X;
      content.HorizontalAlignment = ChartAlignment.Far;
    }
    if (this.ChartArea.SeriesClipRect.Top > y)
    {
      if (content.VerticalAlignment == ChartAlignment.Center)
        content.Y += withPolygon ? (label.DesiredSize.Height + num2) / 2.0 : label.DesiredSize.Height / 2.0;
      else
        content.Y += withPolygon ? label.DesiredSize.Height + num2 : label.DesiredSize.Height;
      y = content.Y;
      content.VerticalAlignment = ChartAlignment.Far;
      if (content.HorizontalAlignment == ChartAlignment.Center && !flag1)
      {
        if (!flag2)
        {
          content.X = withPolygon ? content.X + (label.DesiredSize.Width + num1) / 2.0 : content.X + label.DesiredSize.Width / 2.0;
          content.HorizontalAlignment = ChartAlignment.Far;
        }
        else
        {
          content.X = withPolygon ? content.X - (label.DesiredSize.Width + num1) / 2.0 : content.X - label.DesiredSize.Width / 2.0;
          content.HorizontalAlignment = ChartAlignment.Near;
        }
        x = content.X;
      }
    }
    if (this.ChartArea.SeriesClipRect.Right < x + label.DesiredSize.Width)
    {
      content.X = content.HorizontalAlignment != ChartAlignment.Center ? (withPolygon ? content.X - label.DesiredSize.Width - num1 : content.X - label.DesiredSize.Width) : (withPolygon ? content.X - (label.DesiredSize.Width + num1) / 2.0 : content.X - label.DesiredSize.Width / 2.0);
      x = content.X;
      content.HorizontalAlignment = ChartAlignment.Near;
    }
    if (this.ChartArea.SeriesClipRect.Bottom < y + label.DesiredSize.Height)
    {
      content.Y = content.VerticalAlignment != ChartAlignment.Center ? (withPolygon ? content.Y - label.DesiredSize.Height - num2 : content.Y - label.DesiredSize.Height) : (withPolygon ? content.Y - (label.DesiredSize.Height + num2) / 2.0 : content.Y - label.DesiredSize.Height / 2.0);
      y = content.Y;
      content.VerticalAlignment = ChartAlignment.Near;
    }
    Canvas.SetLeft((UIElement) label, x);
    Canvas.SetTop((UIElement) label, y);
  }

  private void ArrangeAxisLabelsOnBounds(ContentControl label)
  {
    ChartPointInfo content = label.Content as ChartPointInfo;
    if (!this.IsReversed)
    {
      if (this.ChartArea.SeriesClipRect.Left > content.X)
        content.X += this.ChartArea.SeriesClipRect.Left - content.X;
      if (this.ChartArea.SeriesClipRect.Right < content.X + label.DesiredSize.Width)
        content.X -= content.X + label.DesiredSize.Width - this.ChartArea.SeriesClipRect.Right;
    }
    else
    {
      if (this.ChartArea.SeriesClipRect.Top > content.Y)
        content.Y += this.ChartArea.SeriesClipRect.Top - content.Y;
      if (this.ChartArea.SeriesClipRect.Bottom < content.Y + label.DesiredSize.Height)
        content.Y -= content.Y + label.DesiredSize.Height - this.ChartArea.SeriesClipRect.Bottom;
    }
    Canvas.SetLeft((UIElement) label, content.X);
    Canvas.SetTop((UIElement) label, content.Y);
  }

  private void GenerateAxisLabels()
  {
    foreach (KeyValuePair<ChartAxis, ChartPointInfo> axisLabel in this.axisLabels)
    {
      ChartAxis key = axisLabel.Key;
      key.ActualTrackBallLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "axisTrackBallLabel"] as DataTemplate;
      DataTemplate template = key.GetTrackBallTemplate() ?? key.ActualTrackBallLabelTemplate;
      ChartPointInfo chartPointInfo = axisLabel.Value;
      if (key.GetTrackballInfo())
      {
        this.chartAxis = key;
        if (this.IsReversed)
        {
          if (key.Orientation == Orientation.Vertical)
          {
            if (key.GetTrackBallTemplate() == null || key.Tag != null && key.Tag.Equals((object) "FromTheme"))
            {
              chartPointInfo.Y = key.OpposedPosition ? key.ArrangeRect.Left + 6.0 : key.ArrangeRect.Right - 6.0;
              this.AddLabel(chartPointInfo, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Near), this.AxisLabelAlignment, template);
            }
            else
            {
              chartPointInfo.Y = key.OpposedPosition ? key.ArrangeRect.Right : key.ArrangeRect.Left;
              this.AddLabel(chartPointInfo, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Far), this.AxisLabelAlignment, template);
            }
          }
          else
          {
            chartPointInfo.X = key.OpposedPosition ? key.ArrangeRect.Left : key.ArrangeRect.Right;
            this.AddLabel(chartPointInfo, this.AxisLabelAlignment, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Near), template);
          }
        }
        else if (key.Orientation == Orientation.Vertical)
        {
          chartPointInfo.X = key.OpposedPosition ? key.ArrangeRect.Left : key.ArrangeRect.Right;
          this.AddLabel(chartPointInfo, this.AxisLabelAlignment, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Near), template);
        }
        else if (key.GetTrackBallTemplate() == null || key.Tag != null && key.Tag.Equals((object) "FromTheme"))
        {
          chartPointInfo.Y = key.OpposedPosition ? key.ArrangeRect.Bottom - 6.0 : key.ArrangeRect.Top + 6.0;
          this.AddLabel(chartPointInfo, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Far), this.AxisLabelAlignment, template);
        }
        else
        {
          chartPointInfo.Y = key.OpposedPosition ? key.ArrangeRect.Bottom : key.ArrangeRect.Top;
          this.AddLabel(chartPointInfo, ChartTrackBallBehavior.GetChartAlignment(key.OpposedPosition, ChartAlignment.Far), this.AxisLabelAlignment, template);
        }
      }
    }
  }

  private double GetNearestYValue()
  {
    double num1 = double.MaxValue;
    IEnumerable<double> doubles = this.PointInfos.Select<ChartPointInfo, double>((Func<ChartPointInfo, double>) (info => info.Y));
    double nearestYvalue = 0.0;
    foreach (double num2 in doubles)
    {
      double num3 = this.IsReversed ? Math.Abs(num2 - (this.CurrentPoint.X + this.ChartArea.SeriesClipRect.Left)) : Math.Abs(num2 - this.CurrentPoint.Y);
      if (num3 < num1)
      {
        nearestYvalue = num2;
        num1 = num3;
      }
    }
    return nearestYvalue;
  }

  private DataTemplate GetLabelTemplate(ChartPointInfo pointInfo)
  {
    FinancialTechnicalIndicator technicalIndicator1 = this.ChartArea.TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (indicator => indicator.ItemsSource == pointInfo.Series.ItemsSource)).Select<ChartSeries, FinancialTechnicalIndicator>((Func<ChartSeries, FinancialTechnicalIndicator>) (indicator => indicator as FinancialTechnicalIndicator)).FirstOrDefault<FinancialTechnicalIndicator>();
    if (pointInfo.Series is FinancialSeriesBase && technicalIndicator1 != null && technicalIndicator1.ShowTrackballInfo)
    {
      MACDTechnicalIndicator technicalIndicator2 = technicalIndicator1 as MACDTechnicalIndicator;
      if (technicalIndicator1 is BollingerBandIndicator)
        pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "bollingerBandTrackBallLabel"] as DataTemplate;
      else if (technicalIndicator1 is StochasticTechnicalIndicator)
        pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "stochasticTrackBallLabel"] as DataTemplate;
      else if (technicalIndicator2 != null)
      {
        if (technicalIndicator2.Type == MACDType.Both)
          pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "macd_both_TrackBallLabel"] as DataTemplate;
        if (technicalIndicator2.Type == MACDType.Line)
          pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "macd_line_TrackBallLabel"] as DataTemplate;
        if (technicalIndicator2.Type == MACDType.Histogram)
          pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "macd_histogram_TrackBallLabel"] as DataTemplate;
      }
      else
        pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary[(object) "defaultIndicatorTrackBallLabel"] as DataTemplate;
    }
    else
      pointInfo.Series.ActualTrackballLabelTemplate = !(pointInfo.Series is FinancialSeriesBase) || this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint ? (!(pointInfo.Series is RangeSeriesBase) || this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint ? (!(pointInfo.Series is BoxAndWhiskerSeries) || this.LabelDisplayMode == TrackballLabelDisplayMode.NearestPoint || pointInfo.isOutlier ? ChartDictionaries.GenericCommonDictionary[(object) "defaultTrackBallLabel"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "BoxWhiskerTrackBallLabel"] as DataTemplate) : (!(pointInfo.Series is RangeColumnSeries) || pointInfo.Series.IsMultipleYPathRequired ? ChartDictionaries.GenericCommonDictionary[(object) "rangeSeriesTrackBallLabel"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "defaultTrackBallLabel"] as DataTemplate)) : ChartDictionaries.GenericCommonDictionary[(object) "defaultFinancialTrackBallLabel"] as DataTemplate;
    return pointInfo.Series.GetTrackballTemplate() != null && this.UseSeriesPalette && pointInfo.Series.Tag != null && pointInfo.Series.Tag.Equals((object) "FromTheme") || pointInfo.Series.GetTrackballTemplate() == null ? pointInfo.Series.ActualTrackballLabelTemplate : pointInfo.Series.GetTrackballTemplate();
  }

  private void ArrangeGroupLabel()
  {
    this.border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    double xPos;
    double yPos;
    if (this.IsReversed)
    {
      xPos = this.pointInfos.Count != 1 ? this.ChartArea.SeriesClipRect.Left + (this.ChartArea.SeriesClipRect.Width / 2.0 - this.border.DesiredSize.Width / 2.0) : this.CalculateHorizontalAlignment(this.pointInfos[0]);
      yPos = this.CalculateVerticalAlignment(this.pointInfos[0]);
    }
    else
    {
      yPos = this.pointInfos.Count != 1 ? this.ChartArea.SeriesClipRect.Top + (this.ChartArea.SeriesClipRect.Height / 2.0 - this.border.DesiredSize.Height / 2.0) : this.CalculateVerticalAlignment(this.pointInfos[0]);
      xPos = this.CalculateHorizontalAlignment(this.pointInfos[0]);
    }
    if (this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
    {
      if (this.IsReversed)
        xPos = this.ChartArea.SeriesClipRect.Right - this.border.DesiredSize.Width;
      else
        yPos = this.ChartArea.SeriesClipRect.Top;
    }
    this.CheckCollision(xPos, yPos);
  }

  private double CalculateVerticalAlignment(ChartPointInfo chartPointInfo)
  {
    double verticalAlignment = chartPointInfo.VerticalAlignment != ChartAlignment.Center ? (chartPointInfo.VerticalAlignment != ChartAlignment.Near ? this.pointInfos[0].BaseY + 5.0 : this.pointInfos[0].BaseY - this.border.DesiredSize.Height - 5.0) : this.pointInfos[0].BaseY - this.border.DesiredSize.Height / 2.0;
    if (this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
      verticalAlignment = this.pointInfos[0].BaseY - this.border.DesiredSize.Height / 2.0;
    return verticalAlignment;
  }

  private double CalculateHorizontalAlignment(ChartPointInfo chartPointInfo)
  {
    double horizontalAlignment = chartPointInfo.HorizontalAlignment != ChartAlignment.Center ? (chartPointInfo.HorizontalAlignment != ChartAlignment.Near ? this.pointInfos[0].BaseX + 5.0 : this.pointInfos[0].BaseX - this.border.DesiredSize.Width - 5.0) : this.pointInfos[0].BaseX - this.border.DesiredSize.Width / 2.0;
    if (this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
      horizontalAlignment = this.pointInfos[0].BaseX - this.border.DesiredSize.Width / 2.0;
    return horizontalAlignment;
  }

  private void CheckCollision(double xPos, double yPos)
  {
    double length1 = xPos;
    double length2 = yPos;
    if (this.ChartArea.SeriesClipRect.Left > xPos)
    {
      if (this.pointInfos[0].HorizontalAlignment == ChartAlignment.Center || this.pointInfos[0].HorizontalAlignment == ChartAlignment.Auto)
        xPos += this.border.DesiredSize.Width / 2.0;
      else
        xPos += this.border.DesiredSize.Width + 10.0;
      if (!this.IsReversed && this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
        xPos = this.pointInfos[0].BaseX;
      length1 = xPos;
    }
    if (this.ChartArea.SeriesClipRect.Bottom < yPos + this.border.DesiredSize.Height)
    {
      if (this.pointInfos[0].VerticalAlignment == ChartAlignment.Center)
        yPos -= this.border.DesiredSize.Height / 2.0;
      else
        yPos = yPos - this.border.DesiredSize.Height - 10.0;
      length2 = yPos;
    }
    if (this.ChartArea.SeriesClipRect.Top > yPos)
    {
      if (this.pointInfos[0].VerticalAlignment == ChartAlignment.Center)
        yPos += this.border.DesiredSize.Height / 2.0;
      else
        yPos += this.border.DesiredSize.Height + 10.0;
      length2 = yPos;
    }
    if (this.ChartArea.SeriesClipRect.Right < xPos + this.border.DesiredSize.Width)
    {
      if (this.pointInfos[0].HorizontalAlignment == ChartAlignment.Center || this.pointInfos[0].HorizontalAlignment == ChartAlignment.Auto)
        xPos -= this.border.DesiredSize.Width / 2.0;
      else
        xPos = xPos - this.border.DesiredSize.Width - 10.0;
      if (!this.IsReversed && this.LabelDisplayMode == TrackballLabelDisplayMode.GroupAllPoints)
        xPos = this.pointInfos[0].BaseX - this.border.DesiredSize.Width;
      length1 = xPos;
    }
    Canvas.SetLeft((UIElement) this.border, length1);
    Canvas.SetTop((UIElement) this.border, length2);
  }

  private void SmartAlignLabels()
  {
    List<List<Control>> controlListList = new List<List<Control>>();
    List<Control> collection = new List<Control>();
    collection.Add((Control) this.labelElements[0]);
    for (int index = 0; index + 1 < this.labelElements.Count; ++index)
    {
      if (ChartTrackBallBehavior.CheckLabelCollision(this.labelElements[index], this.labelElements[index + 1]))
      {
        collection.Add((Control) this.labelElements[index + 1]);
      }
      else
      {
        controlListList.Add(new List<Control>((IEnumerable<Control>) collection));
        collection.Clear();
        collection.Add((Control) this.labelElements[index + 1]);
      }
    }
    if (collection.Count > 0)
    {
      controlListList.Add(new List<Control>((IEnumerable<Control>) collection));
      collection.Clear();
    }
    if (this.IsReversed)
    {
      foreach (List<Control> controlList in controlListList)
      {
        if (controlList.Count > 1)
        {
          ChartPointInfo content1 = (controlList[0] as ContentControl).Content as ChartPointInfo;
          ChartAlignment horizontalAlignment = content1.HorizontalAlignment;
          double x = content1.X;
          Size desiredSize;
          switch (horizontalAlignment)
          {
            case ChartAlignment.Near:
              double num1 = controlList[0].DesiredSize.Width * (double) controlList.Count + (double) (8 * (controlList.Count - 1));
              desiredSize = controlList[0].DesiredSize;
              double width1 = desiredSize.Width;
              double num2 = num1 - width1;
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double width2 = desiredSize.Width;
                ChartPointInfo content2 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content2.HorizontalAlignment = horizontalAlignment;
                content2.X = x - num2 + (double) index * (width2 + 8.0);
                Canvas.SetLeft((UIElement) controlList[index], content2.X);
              }
              continue;
            case ChartAlignment.Far:
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double width3 = desiredSize.Width;
                ChartPointInfo content3 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content3.HorizontalAlignment = horizontalAlignment;
                content3.X = x + (width3 + 8.0) * (double) index;
                Canvas.SetLeft((UIElement) controlList[index], content3.X);
              }
              continue;
            default:
              desiredSize = controlList[0].DesiredSize;
              double num3 = (desiredSize.Width * (double) controlList.Count + (double) (8 * (controlList.Count - 1))) / 2.0;
              desiredSize = controlList[0].DesiredSize;
              double num4 = desiredSize.Width / 2.0;
              double num5 = num3 - num4;
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double width4 = desiredSize.Width;
                ChartPointInfo content4 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content4.HorizontalAlignment = horizontalAlignment;
                content4.X = x - num5 + (double) index * (width4 + 8.0);
                Canvas.SetLeft((UIElement) controlList[index], content4.X);
              }
              continue;
          }
        }
      }
      if (this.isOpposedAxis)
      {
        this.ArrangeLowestLabelonBounds(controlListList[controlListList.Count - 1]);
        if (controlListList.Count <= 1)
          return;
        for (int index1 = controlListList.Count - 1; index1 - 1 > -1; --index1)
        {
          if (this.CheckGroupsCollision(controlListList[index1], controlListList[index1 - 1]))
          {
            double x = ((controlListList[index1][0] as ContentControl).Content as ChartPointInfo).X;
            double count = (double) controlListList[index1 - 1].Count;
            Size desiredSize = controlListList[index1 - 1][0].DesiredSize;
            double num6 = desiredSize.Width + 8.0;
            double num7 = count * num6;
            double num8 = x - num7;
            for (int index2 = 0; index2 < controlListList[index1 - 1].Count; ++index2)
            {
              desiredSize = controlListList[index1 - 1][index2].DesiredSize;
              double width = desiredSize.Width;
              ChartPointInfo content = (controlListList[index1 - 1][index2] as ContentControl).Content as ChartPointInfo;
              content.X = num8 + (double) index2 * (width + 8.0);
              Canvas.SetLeft((UIElement) controlListList[index1 - 1][index2], content.X);
            }
          }
        }
      }
      else
      {
        this.ArrangeLowestLabelonBounds(controlListList[0]);
        if (controlListList.Count <= 1)
          return;
        for (int index3 = 0; index3 + 1 < controlListList.Count; ++index3)
        {
          if (this.CheckGroupsCollision(controlListList[index3], controlListList[index3 + 1]))
          {
            double x = ((controlListList[index3][controlListList[index3].Count - 1] as ContentControl).Content as ChartPointInfo).X;
            Size desiredSize = controlListList[index3][0].DesiredSize;
            double num9 = desiredSize.Width + 8.0;
            double num10 = x + num9;
            for (int index4 = 0; index4 < controlListList[index3 + 1].Count; ++index4)
            {
              desiredSize = controlListList[index3 + 1][index4].DesiredSize;
              double width = desiredSize.Width;
              ChartPointInfo content = (controlListList[index3 + 1][index4] as ContentControl).Content as ChartPointInfo;
              content.X = num10 + (double) index4 * (width + 8.0);
              Canvas.SetLeft((UIElement) controlListList[index3 + 1][index4], content.X);
            }
          }
        }
      }
    }
    else
    {
      foreach (List<Control> controlList in controlListList)
      {
        if (controlList.Count > 1)
        {
          ChartPointInfo content5 = (controlList[0] as ContentControl).Content as ChartPointInfo;
          ChartAlignment verticalAlignment = content5.VerticalAlignment;
          double y = content5.Y;
          Size desiredSize;
          switch (verticalAlignment)
          {
            case ChartAlignment.Near:
              desiredSize = controlList[0].DesiredSize;
              double num11 = desiredSize.Height * (double) controlList.Count + (double) (8 * (controlList.Count - 1));
              desiredSize = controlList[0].DesiredSize;
              double height1 = desiredSize.Height;
              double num12 = num11 - height1;
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double height2 = desiredSize.Height;
                ChartPointInfo content6 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content6.VerticalAlignment = verticalAlignment;
                content6.Y = y + num12 - (double) index * (height2 + 8.0);
                Canvas.SetTop((UIElement) controlList[index], content6.Y);
              }
              continue;
            case ChartAlignment.Far:
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double height3 = desiredSize.Height;
                ChartPointInfo content7 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content7.VerticalAlignment = verticalAlignment;
                content7.Y = y + (height3 + 8.0) * (double) index;
                Canvas.SetTop((UIElement) controlList[index], content7.Y);
              }
              continue;
            default:
              desiredSize = controlList[0].DesiredSize;
              double num13 = (desiredSize.Height * (double) controlList.Count + (double) (8 * (controlList.Count - 1))) / 2.0;
              desiredSize = controlList[0].DesiredSize;
              double num14 = desiredSize.Height / 2.0;
              double num15 = num13 - num14;
              if (content5.Y - num15 <= 0.0)
                y += num15 - content5.Y + 8.0;
              for (int index = 0; index < controlList.Count; ++index)
              {
                desiredSize = controlList[index].DesiredSize;
                double height4 = desiredSize.Height;
                ChartPointInfo content8 = (controlList[index] as ContentControl).Content as ChartPointInfo;
                content8.VerticalAlignment = verticalAlignment;
                content8.Y = y + num15 - (double) index * (height4 + 8.0);
                Canvas.SetTop((UIElement) controlList[index], content8.Y);
              }
              continue;
          }
        }
      }
      if (this.isOpposedAxis)
      {
        this.ArrangeLowestLabelonBounds(controlListList[controlListList.Count - 1]);
        if (controlListList.Count <= 1)
          return;
        for (int index5 = controlListList.Count - 1; index5 - 1 > -1; --index5)
        {
          if (this.CheckGroupsCollision(controlListList[index5], controlListList[index5 - 1]))
          {
            double y = ((controlListList[index5][0] as ContentControl).Content as ChartPointInfo).Y;
            double count = (double) controlListList[index5 - 1].Count;
            Size desiredSize = controlListList[index5][0].DesiredSize;
            double num16 = desiredSize.Height + 8.0;
            double num17 = count * num16;
            double num18 = y + num17;
            for (int index6 = 0; index6 < controlListList[index5 - 1].Count; ++index6)
            {
              desiredSize = controlListList[index5 - 1][index6].DesiredSize;
              double height = desiredSize.Height;
              ChartPointInfo content = (controlListList[index5 - 1][index6] as ContentControl).Content as ChartPointInfo;
              content.Y = num18 - (double) index6 * (height + 8.0);
              Canvas.SetTop((UIElement) controlListList[index5 - 1][index6], content.Y);
            }
          }
        }
      }
      else
      {
        this.ArrangeLowestLabelonBounds(controlListList[0]);
        if (controlListList.Count <= 1)
          return;
        for (int index7 = 0; index7 + 1 < controlListList.Count; ++index7)
        {
          if (this.CheckGroupsCollision(controlListList[index7], controlListList[index7 + 1]))
          {
            double y = ((controlListList[index7][controlListList[index7].Count - 1] as ContentControl).Content as ChartPointInfo).Y;
            Size desiredSize = controlListList[index7 + 1][0].DesiredSize;
            double num19 = desiredSize.Height + 8.0;
            double num20 = y - num19;
            int num21 = 0;
            for (int index8 = 0; index8 < controlListList[index7 + 1].Count; ++index8)
            {
              desiredSize = controlListList[index7 + 1][index8].DesiredSize;
              double height = desiredSize.Height;
              ChartPointInfo content = (controlListList[index7 + 1][index8] as ContentControl).Content as ChartPointInfo;
              content.Y = num20 - (double) index8 * (height + 8.0);
              Canvas.SetTop((UIElement) controlListList[index7 + 1][index8], content.Y);
              ++num21;
            }
          }
        }
      }
    }
  }

  private void ArrangeLowestLabelonBounds(List<Control> list)
  {
    if (this.IsReversed)
    {
      if (this.isOpposedAxis)
      {
        ChartPointInfo content1 = (list[list.Count - 1] as ContentControl).Content as ChartPointInfo;
        double width = (list[list.Count - 1] as ContentControl).DesiredSize.Width;
        if (content1.X + width <= Math.Round(this.ChartArea.SeriesClipRect.Right, 2))
          return;
        double num = this.ChartArea.SeriesClipRect.Right - (double) list.Count * (width + 8.0);
        for (int index = 0; index < list.Count; ++index)
        {
          ChartPointInfo content2 = (list[index] as ContentControl).Content as ChartPointInfo;
          content2.X = num + (double) index * (width + 8.0);
          Canvas.SetLeft((UIElement) list[index], content2.X);
        }
      }
      else
      {
        ChartPointInfo content3 = (list[0] as ContentControl).Content as ChartPointInfo;
        double width = (list[0] as ContentControl).DesiredSize.Width;
        if (content3.X >= Math.Round(this.ChartArea.SeriesClipRect.Left, 2))
          return;
        double num = this.ChartArea.SeriesClipRect.Left + 8.0;
        for (int index = 0; index < list.Count; ++index)
        {
          ChartPointInfo content4 = (list[index] as ContentControl).Content as ChartPointInfo;
          content4.X = num + (double) index * (width + 8.0);
          Canvas.SetLeft((UIElement) list[index], content4.X);
        }
      }
    }
    else if (this.isOpposedAxis)
    {
      ChartPointInfo content5 = (list[list.Count - 1] as ContentControl).Content as ChartPointInfo;
      double height = (list[list.Count - 1] as ContentControl).DesiredSize.Height;
      if (content5.Y >= Math.Round(this.ChartArea.SeriesClipRect.Top, 2))
        return;
      double num = this.ChartArea.SeriesClipRect.Top + (double) (list.Count - 1) * height + (double) (list.Count * 8);
      for (int index = 0; index < list.Count; ++index)
      {
        ChartPointInfo content6 = (list[index] as ContentControl).Content as ChartPointInfo;
        content6.Y = num - (double) index * (height + 8.0);
        Canvas.SetTop((UIElement) list[index], content6.Y);
      }
    }
    else
    {
      ChartPointInfo content7 = (list[0] as ContentControl).Content as ChartPointInfo;
      double height = (list[0] as ContentControl).DesiredSize.Height;
      if (content7.Y + height <= Math.Round(this.ChartArea.SeriesClipRect.Bottom, 2))
        return;
      double num = this.ChartArea.SeriesClipRect.Bottom - (height + 8.0);
      for (int index = 0; index < list.Count; ++index)
      {
        ChartPointInfo content8 = (list[index] as ContentControl).Content as ChartPointInfo;
        content8.Y = num - (double) index * (height + 8.0);
        Canvas.SetTop((UIElement) list[index], content8.Y);
      }
    }
  }

  private bool CheckGroupsCollision(List<Control> list1, List<Control> list2)
  {
    ChartPointInfo content1 = (list1[0] as ContentControl).Content as ChartPointInfo;
    ChartPointInfo content2 = (list1[list1.Count - 1] as ContentControl).Content as ChartPointInfo;
    ChartPointInfo content3 = (list2[0] as ContentControl).Content as ChartPointInfo;
    ChartPointInfo content4 = (list2[list2.Count - 1] as ContentControl).Content as ChartPointInfo;
    Rect rect1;
    Rect rect2;
    if (this.IsReversed)
    {
      Point point1 = new Point(content1.X, content1.Y);
      Point point2 = new Point(content2.X + list1[list1.Count - 1].DesiredSize.Width, content2.Y + list1[list1.Count - 1].DesiredSize.Height);
      rect1 = new Rect(point1, point2);
      point1 = new Point(content3.X, content3.Y);
      point2 = new Point(content4.X + list2[list2.Count - 1].DesiredSize.Width, content4.Y + list2[list2.Count - 1].DesiredSize.Height);
      rect2 = new Rect(point1, point2);
    }
    else
    {
      Point point1 = new Point(content2.X, content2.Y);
      Point point2 = new Point(content1.X + list1[0].DesiredSize.Width, content1.Y + list1[0].DesiredSize.Height);
      rect1 = new Rect(point1, point2);
      point1 = new Point(content4.X, content4.Y);
      point2 = new Point(content3.X + list2[0].DesiredSize.Width, content3.Y + list2[0].DesiredSize.Height);
      rect2 = new Rect(point1, point2);
    }
    return ChartTrackBallBehavior.CheckLabelCollisionRect(rect1, rect2);
  }

  private void AlignSeriesToolTipPolygon(ContentControl control)
  {
    double height = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    ChartPointInfo content = control.Content as ChartPointInfo;
    double baseX = content.BaseX;
    double baseY = content.BaseY;
    (control.Content as ChartPointInfo).PolygonPoints = ChartDataUtils.GetTooltipPolygonPoints(new Rect(baseX - content.X, baseY - content.Y, width, height), 6.0, this.IsReversed, content.HorizontalAlignment, content.VerticalAlignment);
  }

  private void GenerateAdditionalTrackball(ChartPointInfo pointInfo)
  {
    ChartPointInfo chartPointInfo = new ChartPointInfo();
    List<double> doubleList = new List<double>();
    if (pointInfo.Series is RangeColumnSeries && !pointInfo.Series.IsMultipleYPathRequired)
    {
      DoubleRange visibleRange = pointInfo.Series.ActualYAxis.VisibleRange;
      double num = (visibleRange.End - Math.Abs(visibleRange.Start)) / 2.0 + Convert.ToDouble(pointInfo.ValueY) / 2.0;
      doubleList.Add(num);
    }
    else
    {
      doubleList.Add(Convert.ToDouble(pointInfo.High));
      doubleList.Add(Convert.ToDouble(pointInfo.Low));
    }
    if (pointInfo.Series is FinancialSeriesBase)
    {
      doubleList.Add(Convert.ToDouble(pointInfo.Open));
      doubleList.Add(Convert.ToDouble(pointInfo.Close));
    }
    foreach (double num in doubleList)
    {
      ChartPointInfo pointInfo1 = new ChartPointInfo();
      pointInfo1.Series = pointInfo.Series;
      pointInfo1.Y = this.ChartArea.ValueToLogPoint(pointInfo.Series.ActualYAxis, num);
      if (this.IsReversed)
        pointInfo1.Y += this.ChartArea.SeriesClipRect.Left;
      else
        pointInfo1.Y += this.ChartArea.SeriesClipRect.Top;
      pointInfo1.X = pointInfo.X;
      if (pointInfo.Series.IsActualTransposed && this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo1.Y, pointInfo1.X)) || !pointInfo.Series.IsActualTransposed && this.ChartArea.SeriesClipRect.Contains(new Point(pointInfo1.X, pointInfo1.Y)))
        this.CallTrackball(pointInfo1);
    }
  }

  internal void Activate(bool activate)
  {
    foreach (UIElement element in this.elements)
      element.Visibility = activate ? Visibility.Visible : Visibility.Collapsed;
  }

  internal bool HitTest(double pointX, double pointY)
  {
    double num = this.line.StrokeThickness >= 10.0 ? this.line.StrokeThickness / 2.0 : this.line.StrokeThickness / 2.0 + 10.0;
    return (!this.IsReversed ? new Rect(this.line.X1 - num, this.line.Y1, this.line.X2 + num - (this.line.X1 - num), this.line.Y2 - this.line.Y1) : new Rect(this.line.X1, this.line.Y1 - num, this.line.X2 - this.line.X1, this.line.Y2 + num - (this.line.Y1 - num))).Contains(new Point(pointX, pointY));
  }
}
