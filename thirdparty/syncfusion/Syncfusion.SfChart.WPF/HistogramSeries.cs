// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HistogramSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HistogramSeries : AdornmentSeries, ISupportAxes2D, ISupportAxes
{
  private const int C_distributionPointsCount = 500;
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (HistogramSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(HistogramSeries.OnYPathChanged)));
  public static readonly DependencyProperty HistogramIntervalProperty = DependencyProperty.Register(nameof (HistogramInterval), typeof (double), typeof (HistogramSeries), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(HistogramSeries.OnPropertyChanged)));
  public static readonly DependencyProperty CurveLineStyleProperty = DependencyProperty.Register(nameof (CurveLineStyle), typeof (Style), typeof (HistogramSeries), new PropertyMetadata((object) null, (PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowNormalDistributionCurveProperty = DependencyProperty.Register(nameof (ShowNormalDistributionCurve), typeof (bool), typeof (HistogramSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(HistogramSeries.OnPropertyChanged)));
  public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (ChartAxisBase2D), typeof (HistogramSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(HistogramSeries.OnXAxisChanged)));
  public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (RangeAxisBase), typeof (HistogramSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(HistogramSeries.OnYAxisChanged)));
  private static readonly double c_sqrtDoublePI = Math.Sqrt(2.0 * Math.PI);
  private List<double> yValues;
  private Storyboard sb;
  private Point hitPoint = new Point();
  private Point startPoint = new Point();
  private Point endPoint = new Point();

  public HistogramSeries()
  {
    this.yValues = new List<double>();
    this.ActualYValues = new List<double>();
    this.CurveLineStyle = ChartDictionaries.GenericCommonDictionary[(object) "defaultHistogramSeriesCurveLineStyle"] as Style;
  }

  public string YBindingPath
  {
    get => (string) this.GetValue(HistogramSeries.YBindingPathProperty);
    set => this.SetValue(HistogramSeries.YBindingPathProperty, (object) value);
  }

  public double HistogramInterval
  {
    get => (double) this.GetValue(HistogramSeries.HistogramIntervalProperty);
    set => this.SetValue(HistogramSeries.HistogramIntervalProperty, (object) value);
  }

  public Style CurveLineStyle
  {
    get => (Style) this.GetValue(HistogramSeries.CurveLineStyleProperty);
    set => this.SetValue(HistogramSeries.CurveLineStyleProperty, (object) value);
  }

  public bool ShowNormalDistributionCurve
  {
    get => (bool) this.GetValue(HistogramSeries.ShowNormalDistributionCurveProperty);
    set => this.SetValue(HistogramSeries.ShowNormalDistributionCurveProperty, (object) value);
  }

  public ChartAxisBase2D XAxis
  {
    get => (ChartAxisBase2D) this.GetValue(HistogramSeries.XAxisProperty);
    set => this.SetValue(HistogramSeries.XAxisProperty, (object) value);
  }

  public RangeAxisBase YAxis
  {
    get => (RangeAxisBase) this.GetValue(HistogramSeries.YAxisProperty);
    set => this.SetValue(HistogramSeries.YAxisProperty, (object) value);
  }

  ChartAxis ISupportAxes.ActualXAxis => this.ActualXAxis;

  ChartAxis ISupportAxes.ActualYAxis => this.ActualYAxis;

  public DoubleRange XRange { get; internal set; }

  public DoubleRange YRange { get; internal set; }

  internal List<double> ActualYValues { get; set; }

  public override void CreateSegments()
  {
    List<double> xvalues = this.GetXValues();
    int num1 = 0;
    double num2 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      num2 = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    if (xvalues == null)
      return;
    this.Segments.Clear();
    this.ClearUnUsedAdornments(this.Segments.Count);
    this.ActualYValues.Clear();
    List<Point> points = new List<Point>();
    List<ChartSegment> chartSegmentList = new List<ChartSegment>();
    for (int index = 0; index < this.DataCount; ++index)
      points.Add(new Point(xvalues[index], this.yValues[index]));
    Array.Sort<Point>(points.ToArray(), (IComparer<Point>) new PointsSortByXComparer());
    double histogramInterval = this.HistogramInterval;
    double num3 = 0.0;
    if (points.Count > 0)
      num3 = ChartMath.Round(points[0].X, histogramInterval, false);
    double num4 = num3;
    int num5 = 0;
    List<Point> pointList = new List<Point>();
    List<object> objectList1 = new List<object>();
    int index1 = 0;
    for (int count1 = points.Count; index1 < count1; ++index1)
    {
      Point point = points[index1];
      while (point.X > num4 + histogramInterval)
      {
        int count2 = pointList.Count;
        if (count2 > 0)
        {
          if (this.CreateSegment() is HistogramSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.SetData(num4, (double) count2, num4 + histogramInterval, 0.0);
            segment.XData = (double) num1;
            segment.YData = (double) count2;
            segment.Data = objectList1;
            objectList1 = new List<object>();
            chartSegmentList.Add((ChartSegment) segment);
          }
          this.ActualYValues.Add((double) count2);
          pointList.Clear();
          ++num1;
        }
        num4 += histogramInterval;
        ++num5;
      }
      pointList.Add(points[index1]);
      objectList1.Add(this.ActualData[index1]);
    }
    int count = pointList.Count;
    if (count > 0)
    {
      ++num5;
      if (this.CreateSegment() is HistogramSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        segment.SetData(num4, (double) count, num4 + histogramInterval, 0.0);
        segment.XData = (double) num1;
        segment.YData = (double) count;
        segment.Data = objectList1;
        List<object> objectList2 = new List<object>();
        chartSegmentList.Add((ChartSegment) segment);
      }
      this.ActualYValues.Add((double) count);
      pointList.Clear();
    }
    float num6 = (float) histogramInterval / 2f;
    if (this.AdornmentsInfo != null)
    {
      for (int index2 = 0; index2 < chartSegmentList.Count; ++index2)
      {
        double start = (chartSegmentList[index2] as HistogramSegment).XRange.Start;
        double ydata = (chartSegmentList[index2] as HistogramSegment).YData;
        double num7 = num2;
        switch (this.adornmentInfo.GetAdornmentPosition())
        {
          case AdornmentsPosition.Top:
            this.AddColumnAdornments((chartSegmentList[index2] as HistogramSegment).XData, (chartSegmentList[index2] as HistogramSegment).YData, start, ydata, (double) index2, (double) num6);
            break;
          case AdornmentsPosition.Bottom:
            this.AddColumnAdornments((chartSegmentList[index2] as HistogramSegment).XData, (chartSegmentList[index2] as HistogramSegment).YData, start, num7, (double) index2, (double) num6);
            break;
          default:
            this.AddColumnAdornments((chartSegmentList[index2] as HistogramSegment).XData, (chartSegmentList[index2] as HistogramSegment).YData, start, ydata + (num7 - ydata) / 2.0, (double) index2, (double) num6);
            break;
        }
      }
    }
    if (this.ShowNormalDistributionCurve)
    {
      double mean;
      double standartDeviation;
      HistogramSeries.GetHistogramMeanAndDeviation(points, out mean, out standartDeviation);
      PointCollection distributionPoints = new PointCollection();
      double num8 = num3;
      double num9 = (num3 + (double) num5 * histogramInterval - num8) / 499.0;
      for (int index3 = 0; index3 < 500; ++index3)
      {
        double x = num8 + (double) index3 * num9;
        double y = HistogramSeries.NormalDistribution(x, mean, standartDeviation) * (double) points.Count * histogramInterval;
        distributionPoints.Add(new Point(x, y));
      }
      chartSegmentList.Add((ChartSegment) new HistogramDistributionSegment(distributionPoints, this));
    }
    foreach (ChartSegment chartSegment in chartSegmentList)
      this.Segments.Add(chartSegment);
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  internal override object GetTooltipTag(FrameworkElement element)
  {
    object tooltipTag = (object) null;
    if (element.Tag is ChartSegment)
      tooltipTag = element.Tag;
    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
      tooltipTag = element.DataContext;
    else if (element.DataContext is ChartAdornmentContainer)
    {
      if (this.Segments.Count > 0)
        tooltipTag = (object) this.Segments[ChartExtensionUtils.GetAdornmentIndex((object) element)];
    }
    else if (VisualTreeHelper.GetParent((DependencyObject) element) is ContentPresenter parent && parent.Content is ChartAdornment)
    {
      tooltipTag = this.GetHistogramSegment(parent.Content as ChartAdornment);
    }
    else
    {
      int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) element);
      if (adornmentIndex != -1 && adornmentIndex < this.Adornments.Count && adornmentIndex < this.Segments.Count)
        tooltipTag = this.GetSegment(this.Adornments[adornmentIndex].Item);
    }
    return tooltipTag;
  }

  private object GetHistogramSegment(ChartAdornment adornment)
  {
    return (object) this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is HistogramSegment && (segment as HistogramSegment).XData == adornment.XData && (segment as HistogramSegment).YData == adornment.YData)).FirstOrDefault<ChartSegment>();
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    HistogramSegment toolTipTag = this.ToolTipTag as HistogramSegment;
    Point dataPointPosition = new Point();
    if (toolTipTag != null)
    {
      Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XRange.Median, toolTipTag.YData);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    }
    return dataPointPosition;
  }

  internal override void ResetAdornmentAnimationState()
  {
    if (this.adornmentInfo == null)
      return;
    foreach (FrameworkElement child in this.AdornmentPresenter.Children)
    {
      child.ClearValue(UIElement.RenderTransformProperty);
      child.ClearValue(UIElement.OpacityProperty);
    }
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    if (this.sb != null)
      this.sb.Stop();
    if (!this.canAnimate)
    {
      this.ResetAdornmentAnimationState();
    }
    else
    {
      this.sb = new Storyboard();
      if (this.AdornmentsInfo != null)
      {
        foreach (UIElement child in this.AdornmentPresenter.Children)
          this.OpacityAnimation(child);
      }
      string path = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      {
        FrameworkElement renderedVisual = (FrameworkElement) segment.GetRenderedVisual();
        if (renderedVisual == null)
          return;
        if (!(segment is HistogramDistributionSegment))
        {
          renderedVisual.RenderTransform = (Transform) new ScaleTransform();
          renderedVisual.RenderTransformOrigin = new Point(1.0, 1.0);
          DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
          keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          keyFrame1.Value = 0.0;
          element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
          SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
          keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds));
          keyFrame2.Value = 1.0;
          element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
          keyFrame2.KeySpline = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(path, new object[0]));
          Storyboard.SetTarget((DependencyObject) element, (DependencyObject) renderedVisual);
          this.sb.Children.Add((Timeline) element);
        }
        else
          this.OpacityAnimation((UIElement) renderedVisual);
      }
      this.sb.Begin();
    }
  }

  internal override void UpdateRange()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    foreach (ChartSegment chartSegment in this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (item => item is HistogramSegment)))
    {
      this.XRange += chartSegment.XRange;
      this.YRange += chartSegment.YRange;
    }
  }

  internal override void Dispose()
  {
    if (this.sb != null)
    {
      this.sb.Stop();
      this.sb.Children.Clear();
      this.sb = (Storyboard) null;
    }
    base.Dispose();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.YBindingPath }, (IList<double>) this.yValues);
  }

  protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalPrimaryAxis != oldAxis && this.Area.InternalSecondaryAxis != oldAxis)
      {
        this.Area.Axes.RemoveItem(oldAxis, this.Area.DependentSeriesAxes.Contains(oldAxis));
        this.Area.DependentSeriesAxes.Remove(oldAxis);
      }
    }
    else if (this.ActualArea != null && this.ActualArea.InternalSecondaryAxis != null && this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null)
    {
      if (this.Area != null && !this.Area.Axes.Contains(newAxis))
      {
        this.Area.Axes.Add(newAxis);
        this.Area.DependentSeriesAxes.Add(newAxis);
      }
      newAxis.Area = (ChartBase) this.Area;
      newAxis.Orientation = Orientation.Vertical;
      if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
        newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalPrimaryAxis != oldAxis && this.Area.InternalSecondaryAxis != oldAxis)
      {
        this.Area.Axes.RemoveItem(oldAxis, this.Area.DependentSeriesAxes.Contains(oldAxis));
        this.Area.DependentSeriesAxes.Remove(oldAxis);
      }
    }
    else if (this.ActualArea != null && this.ActualArea.InternalPrimaryAxis != null && this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null)
    {
      if (this.Area != null && !this.Area.Axes.Contains(newAxis))
      {
        this.Area.Axes.Add(newAxis);
        this.Area.DependentSeriesAxes.Add(newAxis);
      }
      newAxis.Area = (ChartBase) this.Area;
      newAxis.Orientation = Orientation.Horizontal;
      if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
        newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new HistogramSegment();

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.yValues.Clear();
    this.GeneratePoints(new string[1]{ this.YBindingPath }, (IList<double>) this.yValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.yValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (!this.ShowTooltip || e.OriginalSource is Polyline)
      return;
    base.OnMouseMove(e);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new HistogramSeries()
    {
      XAxis = this.XAxis,
      YAxis = this.YAxis,
      HistogramInterval = this.HistogramInterval,
      ShowNormalDistributionCurve = this.ShowNormalDistributionCurve,
      YBindingPath = this.YBindingPath
    });
  }

  private static void GetHistogramMeanAndDeviation(
    List<Point> points,
    out double mean,
    out double standartDeviation)
  {
    int count = points.Count;
    double num1 = 0.0;
    for (int index = 0; index < count; ++index)
      num1 += points[index].X;
    mean = num1 / (double) count;
    double num2 = 0.0;
    for (int index = 0; index < count; ++index)
    {
      double num3 = points[index].X - mean;
      num2 += num3 * num3;
    }
    standartDeviation = Math.Sqrt(num2 / (double) count);
  }

  private static double NormalDistribution(double x, double m, double sigma)
  {
    return Math.Exp(-(x - m) * (x - m) / (2.0 * sigma * sigma)) / (sigma * HistogramSeries.c_sqrtDoublePI);
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as HistogramSeries).OnBindingPathChanged(e);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    HistogramSeries histogramSeries = d as HistogramSeries;
    if (histogramSeries.Area == null)
      return;
    histogramSeries.Area.ScheduleUpdate();
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as HistogramSeries).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as HistogramSeries).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private void OpacityAnimation(UIElement child)
  {
    DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
    SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
    keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
    keyFrame1.Value = 0.0;
    element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
    keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
    keyFrame2.Value = 0.0;
    element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
    keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds + 1.0));
    keyFrame3.Value = 1.0;
    element.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
    KeySpline keySpline = new KeySpline();
    keySpline.ControlPoint1 = new Point(0.64, 0.84);
    keySpline.ControlPoint2 = new Point(0.67, 0.95);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("Opacity", new object[0]));
    keyFrame3.KeySpline = keySpline;
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) child);
    this.sb.Children.Add((Timeline) element);
  }

  private ChartDataPointInfo GetCurveDataPoint(Point mousePos, object tag)
  {
    this.hitPoint.X = mousePos.X - this.Area.SeriesClipRect.Left;
    this.hitPoint.Y = mousePos.Y - this.Area.SeriesClipRect.Top;
    double num1 = Math.Floor(this.Area.SeriesClipRect.Width * 0.025);
    double num2 = Math.Floor(this.Area.SeriesClipRect.Height * 0.025);
    this.hitPoint.X -= num1;
    this.hitPoint.Y -= num2;
    this.startPoint.X = this.ActualArea.PointToValue(this.ActualXAxis, this.hitPoint);
    this.startPoint.Y = this.ActualArea.PointToValue(this.ActualYAxis, this.hitPoint);
    this.hitPoint.X += 2.0 * num1;
    this.hitPoint.Y += 2.0 * num2;
    this.endPoint.X = this.ActualArea.PointToValue(this.ActualXAxis, this.hitPoint);
    this.endPoint.Y = this.ActualArea.PointToValue(this.ActualYAxis, this.hitPoint);
    Rect rect = new Rect(this.startPoint, this.endPoint);
    this.dataPoint = (ChartDataPointInfo) null;
    PointCollection distributionPoints = (tag as HistogramDistributionSegment).distributionPoints;
    for (int index = 0; index < distributionPoints.Count; ++index)
    {
      this.hitPoint.X = distributionPoints[index].X;
      this.hitPoint.Y = distributionPoints[index].Y;
      if (rect.Contains(this.hitPoint))
      {
        this.dataPoint = new ChartDataPointInfo();
        this.dataPoint.Index = index;
        this.dataPoint.XData = distributionPoints[index].X;
        this.dataPoint.YData = distributionPoints[index].Y;
        this.dataPoint.Series = (ChartSeriesBase) this;
        if (index > -1 && this.ActualData.Count > index)
        {
          this.dataPoint.Item = this.ActualData[index];
          break;
        }
        break;
      }
    }
    return this.dataPoint;
  }
}
