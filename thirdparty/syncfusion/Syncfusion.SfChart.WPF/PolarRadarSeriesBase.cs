// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PolarRadarSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class PolarRadarSeriesBase : AdornmentSeries, ISupportAxes2D, ISupportAxes
{
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(PolarRadarSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof (IsClosed), typeof (bool), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) true, new PropertyChangedCallback(PolarRadarSeriesBase.OnDrawValueChanged)));
  public static readonly DependencyProperty DrawTypeProperty = DependencyProperty.Register(nameof (DrawType), typeof (ChartSeriesDrawType), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) ChartSeriesDrawType.Area, new PropertyChangedCallback(PolarRadarSeriesBase.OnDrawValueChanged)));
  public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (ChartAxisBase2D), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(PolarRadarSeriesBase.OnXAxisChanged)));
  public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (RangeAxisBase), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(PolarRadarSeriesBase.OnYAxisChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (PolarRadarSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(PolarRadarSeriesBase.OnDrawValueChanged)));
  private Storyboard sb;

  public PolarRadarSeriesBase() => this.YValues = (IList<double>) new List<double>();

  public string YBindingPath
  {
    get => (string) this.GetValue(PolarRadarSeriesBase.YBindingPathProperty);
    set => this.SetValue(PolarRadarSeriesBase.YBindingPathProperty, (object) value);
  }

  public bool IsClosed
  {
    get => (bool) this.GetValue(PolarRadarSeriesBase.IsClosedProperty);
    set => this.SetValue(PolarRadarSeriesBase.IsClosedProperty, (object) value);
  }

  public ChartSeriesDrawType DrawType
  {
    get => (ChartSeriesDrawType) this.GetValue(PolarRadarSeriesBase.DrawTypeProperty);
    set => this.SetValue(PolarRadarSeriesBase.DrawTypeProperty, (object) value);
  }

  public DoubleRange XRange { get; internal set; }

  public DoubleRange YRange { get; internal set; }

  public ChartAxisBase2D XAxis
  {
    get => (ChartAxisBase2D) this.GetValue(PolarRadarSeriesBase.XAxisProperty);
    set => this.SetValue(PolarRadarSeriesBase.XAxisProperty, (object) value);
  }

  public RangeAxisBase YAxis
  {
    get => (RangeAxisBase) this.GetValue(PolarRadarSeriesBase.YAxisProperty);
    set => this.SetValue(PolarRadarSeriesBase.YAxisProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(PolarRadarSeriesBase.StrokeDashArrayProperty);
    set => this.SetValue(PolarRadarSeriesBase.StrokeDashArrayProperty, (object) value);
  }

  ChartAxis ISupportAxes.ActualXAxis => this.ActualXAxis;

  ChartAxis ISupportAxes.ActualYAxis => this.ActualYAxis;

  protected IList<double> YValues { get; set; }

  protected ChartSegment Segment { get; set; }

  public override void FindNearestChartPoint(
    Point point,
    out double x,
    out double y,
    out double stackedYValue)
  {
    x = double.NaN;
    y = double.NaN;
    stackedYValue = double.NaN;
    double center = 0.5 * Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height);
    double a = this.Area.InternalPrimaryAxis.PolarCoefficientToValue(ChartTransform.RadianToPolarCoefficient(ChartTransform.PointToPolarRadian(point, center)));
    double start = this.ActualXAxis.VisibleRange.Start;
    double end = this.ActualXAxis.VisibleRange.End;
    if (a > end || a < start)
      return;
    if (this.IsIndexed || !(this.ActualXValues is IList<double>))
    {
      double num = Math.Round(a);
      List<double> xvalues = this.GetXValues();
      if (xvalues == null || num >= (double) xvalues.Count || num < 0.0 || this.YValues == null)
        return;
      x = (double) (this.NearestSegmentIndex = xvalues.IndexOf((double) (int) num));
      y = this.YValues[this.NearestSegmentIndex];
    }
    else
    {
      ChartPoint chartPoint = new ChartPoint();
      IList<double> actualXvalues = this.ActualXValues as IList<double>;
      chartPoint.X = start;
      chartPoint.Y = this.ActualYAxis.VisibleRange.Start;
      int index = 0;
      if (actualXvalues == null || this.YValues == null)
        return;
      foreach (double x1 in (IEnumerable<double>) actualXvalues)
      {
        double yvalue = this.YValues[index];
        if (Math.Abs(a - x1) <= Math.Abs(a - chartPoint.X))
        {
          chartPoint = new ChartPoint(x1, yvalue);
          x = x1;
          y = yvalue;
          this.NearestSegmentIndex = index;
        }
        ++index;
      }
    }
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
    {
      this.sb.Stop();
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    this.sb = new Storyboard();
    string propertyXPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleX)";
    string propertyYPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
    Panel gridLinesPanel = (this.ActualArea as SfChart).GridLinesPanel;
    Point point = new Point(gridLinesPanel.ActualWidth / 2.0, gridLinesPanel.ActualHeight / 2.0);
    if (this.DrawType == ChartSeriesDrawType.Area)
    {
      UIElement child = (this.Segment.GetRenderedVisual() as Canvas).Children[0];
      child.RenderTransform = (Transform) new ScaleTransform()
      {
        CenterX = point.X,
        CenterY = point.Y
      };
      this.AnimateElement(child, propertyXPath, propertyYPath);
    }
    else
    {
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      {
        UIElement renderedVisual = segment.GetRenderedVisual();
        renderedVisual.RenderTransform = (Transform) new ScaleTransform()
        {
          CenterX = point.X,
          CenterY = point.Y
        };
        this.AnimateElement(renderedVisual, propertyXPath, propertyYPath);
      }
    }
    this.AnimateAdornments();
    this.sb.Begin();
  }

  internal override void UpdateRange()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      this.XRange += segment.XRange;
      this.YRange += segment.YRange;
    }
  }

  internal override void ValidateYValues()
  {
    using (IEnumerator<double> enumerator = this.YValues.GetEnumerator())
    {
      if (!enumerator.MoveNext() || !double.IsNaN(enumerator.Current) || !this.ShowEmptyPoints)
        return;
      this.ValidateDataPoints(this.YValues);
    }
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    foreach (List<int> intList in emptyPointIndex)
    {
      foreach (int index in intList)
        this.YValues[index] = double.NaN;
    }
  }

  internal override void RemoveTooltip()
  {
    if (this.Area == null)
      return;
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    if (adorningCanvas == null || !adorningCanvas.Children.Contains((UIElement) this.Area.Tooltip))
      return;
    adorningCanvas.Children.Remove((UIElement) this.Area.Tooltip);
  }

  internal override int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    point.X = point.X - num1 - this.Area.SeriesClipRect.Left + this.Area.Margin.Left;
    point.Y = point.Y - num2 - this.Area.SeriesClipRect.Top + this.Area.Margin.Top;
    double start = this.ActualXAxis.VisibleRange.Start;
    double end = this.ActualXAxis.VisibleRange.End;
    int dataPointIndex = -1;
    double center = 0.5 * Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height);
    double num3 = Math.Round(this.Area.InternalPrimaryAxis.PolarCoefficientToValue(ChartTransform.RadianToPolarCoefficient(ChartTransform.PointToPolarRadian(point, center))));
    if (num3 <= end && num3 >= start)
      dataPointIndex = this.GetXValues().IndexOf((double) (int) num3);
    return dataPointIndex;
  }

  internal override void UpdateTooltip(object originalSource)
  {
    if (!this.ShowTooltip || !(originalSource is Shape shape) || shape != null && shape.Tag == null)
      return;
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    object obj = (object) null;
    double x = double.NaN;
    double y = double.NaN;
    double stackedYValue = double.NaN;
    if (this.Area.SeriesClipRect.Contains(this.mousePos))
    {
      this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      if (this.NearestSegmentIndex > -1 && this.NearestSegmentIndex < this.ActualData.Count)
        obj = this.ActualData[this.NearestSegmentIndex];
    }
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    if (this.DrawType == ChartSeriesDrawType.Area)
    {
      AreaSegment tag = shape.Tag as AreaSegment;
      tag.Item = obj;
      tag.XData = x;
      tag.YData = y;
    }
    else
    {
      LineSegment tag = shape.Tag as LineSegment;
      tag.Item = obj;
      tag.YData = y;
    }
    if (chartTooltip1 == null)
      return;
    object tag1 = shape.Tag;
    this.ToolTipTag = tag1;
    chartTooltip1.PolygonPath = " ";
    chartTooltip1.DataContext = tag1;
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      if (ChartTooltip.GetActualInitialShowDelay(this.ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this)) == 0)
        adorningCanvas.Children.Add((UIElement) chartTooltip1);
      chartTooltip1.ContentTemplate = this.GetTooltipTemplate();
      this.AddTooltip();
      if (ChartTooltip.GetActualEnableAnimation(this.ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation((UIElement) this)))
      {
        this.SetDoubleAnimation(chartTooltip1);
        Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
        Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
        this._stopwatch.Start();
      }
      else
      {
        Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
        Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
        this._stopwatch.Start();
      }
    }
    else
    {
      foreach (object child in adorningCanvas.Children)
      {
        if (child is ChartTooltip chartTooltip2)
          chartTooltip1 = chartTooltip2;
      }
      this.AddTooltip();
      Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
      Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
    }
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip) => this.mousePos;

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
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
  }

  protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (oldAxis != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis == oldAxis)
      {
        this.Area.Axes.Remove(oldAxis);
        if (this.Area.InternalSecondaryAxis.IsDefault)
        {
          this.Area.SecondaryAxis = (RangeAxisBase) null;
          if (this.Area.InternalPrimaryAxis != null && this.Area.InternalPrimaryAxis.AssociatedAxes.Contains(oldAxis))
            this.Area.InternalPrimaryAxis.AssociatedAxes.Remove(oldAxis);
        }
      }
    }
    if (newAxis != null && !newAxis.RegisteredSeries.Contains((ISupportAxes) this))
    {
      newAxis.Area = (ChartBase) this.Area;
      if (this.Area != null && !this.Area.Axes.Contains(newAxis))
        this.Area.Axes.Add(newAxis);
      newAxis.Orientation = Orientation.Vertical;
      newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (oldAxis != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count > 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalPrimaryAxis != null && this.Area.InternalPrimaryAxis == oldAxis)
      {
        this.Area.Axes.Remove(oldAxis);
        if (this.Area.InternalPrimaryAxis.IsDefault)
        {
          this.Area.PrimaryAxis = (ChartAxisBase2D) null;
          if (this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.AssociatedAxes.Contains(oldAxis))
            this.Area.InternalSecondaryAxis.AssociatedAxes.Remove(oldAxis);
        }
      }
    }
    if (newAxis != null)
    {
      if (this.Area != null && !this.Area.Axes.Contains(newAxis))
        this.Area.Axes.Add(newAxis);
      newAxis.Orientation = Orientation.Horizontal;
      if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
        newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.YValues.Clear();
    this.Segment = (ChartSegment) null;
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.isPointValidated = false;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.YValues.Clear();
    this.Segment = (ChartSegment) null;
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    PolarRadarSeriesBase polarRadarSeriesBase = obj as PolarRadarSeriesBase;
    polarRadarSeriesBase.IsClosed = this.IsClosed;
    polarRadarSeriesBase.YBindingPath = this.YBindingPath;
    polarRadarSeriesBase.DrawType = this.DrawType;
    polarRadarSeriesBase.IsClosed = this.IsClosed;
    return base.CloneSeries((DependencyObject) polarRadarSeriesBase);
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PolarRadarSeriesBase).OnBindingPathChanged(e);
  }

  private static void OnDrawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PolarRadarSeriesBase).UpdateArea();
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PolarRadarSeriesBase).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as PolarRadarSeriesBase).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private void timer_Tick(object sender, object e)
  {
    this.RemoveTooltip();
    this.Timer.Stop();
  }

  private void AnimateAdornments()
  {
    if (this.AdornmentsInfo == null)
      return;
    foreach (object child in this.AdornmentPresenter.Children)
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
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) (child as FrameworkElement));
      this.sb.Children.Add((Timeline) element);
    }
  }

  private void AnimateElement(UIElement element, string propertyXPath, string propertyYPath)
  {
    DoubleAnimation element1 = new DoubleAnimation();
    element1.From = new double?(0.0);
    element1.To = new double?(1.0);
    element1.Duration = new Duration().GetDuration(this.AnimationDuration);
    Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) element);
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(propertyXPath, new object[0]));
    this.sb.Children.Add((Timeline) element1);
    DoubleAnimation element2 = new DoubleAnimation();
    element2.From = new double?(0.0);
    element2.To = new double?(1.0);
    element2.Duration = new Duration().GetDuration(this.AnimationDuration);
    Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) element);
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(propertyYPath, new object[0]));
    this.sb.Children.Add((Timeline) element2);
  }
}
