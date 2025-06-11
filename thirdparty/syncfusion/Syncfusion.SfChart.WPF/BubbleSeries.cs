// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BubbleSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class BubbleSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty ShowZeroBubblesProperty = DependencyProperty.Register(nameof (ShowZeroBubbles), typeof (bool), typeof (BubbleSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(BubbleSeries.OnShowZeroBubblesPropertyChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (BubbleSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(BubbleSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty MinimumRadiusProperty = DependencyProperty.Register(nameof (MinimumRadius), typeof (double), typeof (BubbleSeries), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(BubbleSeries.OnSizeChanged)));
  public static readonly DependencyProperty MaximumRadiusProperty = DependencyProperty.Register(nameof (MaximumRadius), typeof (double), typeof (BubbleSeries), new PropertyMetadata((object) 30.0, new PropertyChangedCallback(BubbleSeries.OnSizeChanged)));
  public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof (Size), typeof (string), typeof (BubbleSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(BubbleSeries.OnSizeChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (BubbleSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(BubbleSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (BubbleSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(BubbleSeries.OnCustomTemplateChanged)));
  private List<double> sizeValues;
  private Storyboard sb;
  private bool IsSegmentAtEdge;

  public BubbleSeries() => this.sizeValues = new List<double>();

  public bool ShowZeroBubbles
  {
    get => (bool) this.GetValue(BubbleSeries.ShowZeroBubblesProperty);
    set => this.SetValue(BubbleSeries.ShowZeroBubblesProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(BubbleSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(BubbleSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public double MinimumRadius
  {
    get => (double) this.GetValue(BubbleSeries.MinimumRadiusProperty);
    set => this.SetValue(BubbleSeries.MinimumRadiusProperty, (object) value);
  }

  public double MaximumRadius
  {
    get => (double) this.GetValue(BubbleSeries.MaximumRadiusProperty);
    set => this.SetValue(BubbleSeries.MaximumRadiusProperty, (object) value);
  }

  public string Size
  {
    get => (string) this.GetValue(BubbleSeries.SizeProperty);
    set => this.SetValue(BubbleSeries.SizeProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(BubbleSeries.SelectedIndexProperty);
    set => this.SetValue(BubbleSeries.SelectedIndexProperty, (object) value);
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(BubbleSeries.CustomTemplateProperty);
    set => this.SetValue(BubbleSeries.CustomTemplateProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    double num1 = this.sizeValues.Select<double, double>((Func<double, double>) (val => val)).Max();
    double minimumRadius = this.MinimumRadius;
    double num2 = this.MaximumRadius - minimumRadius;
    if (xValues == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index = 0; index < xValues.Count; ++index)
      {
        double d = num2 * Math.Abs(this.sizeValues[index] / num1);
        double num3 = double.IsNaN(d) ? 0.0 : d;
        double num4 = !this.ShowZeroBubbles ? (this.sizeValues[index] != 0.0 ? minimumRadius + num3 : 0.0) : minimumRadius + num3;
        if (index < xValues.Count && this.GroupedSeriesYValues[0].Count > index && this.CreateSegment() is BubbleSegment segment)
        {
          segment.SegmentRadius = num4;
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(xValues[index], this.GroupedSeriesYValues[0][index]);
          segment.Item = this.ActualData[index];
          segment.Size = this.sizeValues[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornmentAtXY(xValues[index], this.GroupedSeriesYValues[0][index], index);
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index = 0; index < this.DataCount; ++index)
      {
        double d = num2 * Math.Abs(this.sizeValues[index] / num1);
        double num5 = double.IsNaN(d) ? 0.0 : d;
        double num6 = !this.ShowZeroBubbles ? (this.sizeValues[index] != 0.0 ? minimumRadius + num5 : 0.0) : minimumRadius + num5;
        if (index < this.Segments.Count)
        {
          this.Segments[index].SetData(xValues[index], this.YValues[index]);
          (this.Segments[index] as BubbleSegment).SegmentRadius = num6;
          (this.Segments[index] as BubbleSegment).Item = this.ActualData[index];
          (this.Segments[index] as BubbleSegment).Size = this.sizeValues[index];
          (this.Segments[index] as BubbleSegment).YData = this.YValues[index];
          (this.Segments[index] as BubbleSegment).XData = xValues[index];
          if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
            this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        }
        else if (this.CreateSegment() is BubbleSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SegmentRadius = num6;
          segment.SetData(xValues[index], this.YValues[index]);
          segment.Item = this.ActualData[index];
          segment.Size = this.sizeValues[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornmentAtXY(xValues[index], this.YValues[index], index);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
  }

  internal override int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    point.X = point.X - num1 - this.Area.SeriesClipRect.Left + this.Area.Margin.Left;
    point.Y = point.Y - num2 - this.Area.SeriesClipRect.Top + this.Area.Margin.Top;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (segment.GetRenderedVisual() is Ellipse renderedVisual && BubbleSeries.EllipseContainsPoint(renderedVisual, point))
        return this.Segments.IndexOf(segment);
    }
    return -1;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    this.IsSegmentAtEdge = false;
    BubbleSegment toolTipTag = this.ToolTipTag as BubbleSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top - toolTipTag.SegmentRadius;
    if (dataPointPosition.Y - tooltip.DesiredSize.Height < this.ActualArea.SeriesClipRect.Top)
    {
      dataPointPosition.Y += toolTipTag.SegmentRadius * 2.0 + tooltip.DesiredSize.Height + 8.0;
      this.IsSegmentAtEdge = true;
    }
    return dataPointPosition;
  }

  internal override VerticalPosition GetVerticalPosition(VerticalPosition verticalPosition)
  {
    return !this.IsSegmentAtEdge ? base.GetVerticalPosition(verticalPosition) : VerticalPosition.Top;
  }

  internal override void Animate()
  {
    int index = 0;
    Random random = new Random();
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
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) (random.Next(0, 20) * 20));
      FrameworkElement renderedVisual = (FrameworkElement) segment.GetRenderedVisual();
      renderedVisual.RenderTransform = (Transform) null;
      renderedVisual.RenderTransform = (Transform) new ScaleTransform()
      {
        ScaleY = 0.0,
        ScaleX = 0.0
      };
      renderedVisual.RenderTransformOrigin = new Point(0.5, 0.5);
      DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
      element1.BeginTime = new TimeSpan?(timeSpan);
      SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
      keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
      keyFrame1.Value = 0.0;
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 50.0 / 100.0));
      keyFrame2.KeySpline = new KeySpline()
      {
        ControlPoint1 = new Point(0.64, 0.84),
        ControlPoint2 = new Point(0.67, 0.95)
      };
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      keyFrame2.Value = 1.0;
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) renderedVisual);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
      element2.BeginTime = new TimeSpan?(timeSpan);
      SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
      keyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
      keyFrame3.Value = 0.0;
      element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
      SplineDoubleKeyFrame keyFrame4 = new SplineDoubleKeyFrame();
      keyFrame4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 50.0 / 100.0));
      keyFrame4.KeySpline = new KeySpline()
      {
        ControlPoint1 = new Point(0.64, 0.84),
        ControlPoint2 = new Point(0.67, 0.95)
      };
      element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
      keyFrame4.Value = 1.0;
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", new object[0]));
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) renderedVisual);
      this.sb.Children.Add((Timeline) element2);
      if (this.AdornmentsInfo != null && this.AdornmentsInfo.ShowLabel)
      {
        UIElement labelPresenter = (UIElement) this.AdornmentsInfo.LabelPresenters[index];
        labelPresenter.Opacity = 0.0;
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.To = new double?(1.0);
        doubleAnimation.From = new double?(0.0);
        doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(timeSpan.TotalSeconds + (double) (timeSpan.Seconds * 90 / 100)));
        DoubleAnimation element3 = doubleAnimation;
        element3.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
        Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
        Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
        this.sb.Children.Add((Timeline) element3);
      }
      ++index;
    }
    this.sb.Begin();
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
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
    this.sizeValues.Clear();
    this.GeneratePoints(new string[2]
    {
      this.YBindingPath,
      this.Size
    }, this.YValues, (IList<double>) this.sizeValues);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new BubbleSegment();

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.YValues.Clear();
    this.sizeValues.Clear();
    this.GeneratePoints(new string[2]
    {
      this.YBindingPath,
      this.Size
    }, this.YValues, (IList<double>) this.sizeValues);
    this.UpdateArea();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new BubbleSeries()
    {
      MaximumRadius = this.MaximumRadius,
      MinimumRadius = this.MinimumRadius,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      ShowZeroBubbles = this.ShowZeroBubbles,
      Size = this.Size,
      CustomTemplate = this.CustomTemplate
    });
  }

  private static bool EllipseContainsPoint(Ellipse Ellipse, Point point)
  {
    Point point1 = new Point(Canvas.GetLeft((UIElement) Ellipse) + Ellipse.Width / 2.0, Canvas.GetTop((UIElement) Ellipse) + Ellipse.Height / 2.0);
    double num1 = Ellipse.Width / 2.0;
    double num2 = Ellipse.Height / 2.0;
    if (num1 <= 0.0 || num2 <= 0.0)
      return false;
    Point point2 = new Point(point.X - point1.X, point.Y - point1.Y);
    return point2.X * point2.X / (num1 * num1) + point2.Y * point2.Y / (num2 * num2) <= 1.0;
  }

  private static void OnShowZeroBubblesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as BubbleSeries).UpdateArea();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as BubbleSeries).UpdateArea();
  }

  private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as BubbleSeries).OnBindingPathChanged(e);
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
    chartSeries.OnPropertyChanged("SelectedIndex");
    if (chartSeries.ActualArea == null || chartSeries.ActualArea.SelectionBehaviour == null)
      return;
    if (chartSeries.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
    {
      chartSeries.SelectedIndexChanged((int) e.NewValue, (int) e.OldValue);
    }
    else
    {
      if ((int) e.NewValue == -1)
        return;
      chartSeries.SelectedSegmentsIndexes.Add((int) e.NewValue);
    }
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    BubbleSeries bubbleSeries = d as BubbleSeries;
    if (bubbleSeries.Area == null)
      return;
    bubbleSeries.Segments.Clear();
    bubbleSeries.Area.ScheduleUpdate();
  }
}
