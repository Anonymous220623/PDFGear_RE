// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StepAreaSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StepAreaSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof (IsClosed), typeof (bool), typeof (StepAreaSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(StepAreaSeries.OnPropertyChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (StepAreaSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(StepAreaSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (StepAreaSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StepAreaSeries.OnSegmentSelectionBrush)));
  private Storyboard sb;
  private Point hitPoint = new Point();
  private RectangleGeometry geometry;

  public bool IsClosed
  {
    get => (bool) this.GetValue(StepAreaSeries.IsClosedProperty);
    set => this.SetValue(StepAreaSeries.IsClosedProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(StepAreaSeries.SelectedIndexProperty);
    set => this.SetValue(StepAreaSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(StepAreaSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(StepAreaSeries.SegmentSelectionBrushProperty, (object) value);
  }

  protected internal override bool IsLinear => true;

  protected internal override bool IsAreaTypeSeries => true;

  protected internal override List<ChartSegment> SelectedSegments
  {
    get
    {
      if (this.SelectedSegmentsIndexes.Count <= 0)
        return (List<ChartSegment>) null;
      this.selectedSegments.Clear();
      foreach (int selectedSegmentsIndex in (Collection<int>) this.SelectedSegmentsIndexes)
        this.selectedSegments.Add((ChartSegment) this.GetDataPoint(selectedSegmentsIndex));
      return this.selectedSegments;
    }
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> doubleList = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList.Count == 0)
      return;
    double origin = this.ActualXAxis.Origin;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      List<ChartPoint> AreaPoints = new List<ChartPoint>()
      {
        new ChartPoint(doubleList[doubleList.Count - 1], origin),
        new ChartPoint(doubleList[0], origin)
      };
      for (int index = 0; index < doubleList.Count; ++index)
      {
        AreaPoints.Add(new ChartPoint(doubleList[index], this.GroupedSeriesYValues[0][index]));
        if (index != doubleList.Count - 1)
          AreaPoints.Add(new ChartPoint(doubleList[index + 1], this.GroupedSeriesYValues[0][index]));
      }
      if (this.Segments.Count == 0 && this.CreateSegment() is StepAreaSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        segment.Item = (object) this.ActualData;
        segment.SetData(AreaPoints);
        this.Segments.Add((ChartSegment) segment);
      }
      if (this.AdornmentsInfo == null)
        return;
      this.AddAreaAdornments(this.GroupedSeriesYValues[0]);
    }
    else
    {
      this.ClearUnUsedAdornments(this.DataCount);
      List<ChartPoint> AreaPoints = new List<ChartPoint>()
      {
        new ChartPoint(doubleList[this.DataCount - 1], origin),
        new ChartPoint(doubleList[0], origin)
      };
      for (int index = 0; index < this.DataCount; ++index)
      {
        AreaPoints.Add(new ChartPoint(doubleList[index], this.YValues[index]));
        if (index != this.DataCount - 1)
          AreaPoints.Add(new ChartPoint(doubleList[index + 1], this.YValues[index]));
      }
      if (this.Segments.Count == 0)
      {
        if (this.CreateSegment() is StepAreaSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.Item = (object) this.ActualData;
          segment.SetData(AreaPoints);
          this.Segments.Add((ChartSegment) segment);
        }
      }
      else
      {
        this.Segments[0].Item = (object) this.ActualData;
        this.Segments[0].SetData(AreaPoints);
      }
      if (this.AdornmentsInfo == null)
        return;
      this.AddAreaAdornments(this.YValues);
    }
  }

  internal override void ResetAdornmentAnimationState()
  {
    if (this.adornmentInfo == null)
      return;
    foreach (object child in this.AdornmentPresenter.Children)
    {
      FrameworkElement frameworkElement = child as FrameworkElement;
      frameworkElement.ClearValue(UIElement.RenderTransformProperty);
      frameworkElement.ClearValue(UIElement.OpacityProperty);
    }
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    StepAreaSegment toolTipTag = this.ToolTipTag as StepAreaSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  internal override bool GetAnimationIsActive() => this.geometry != null;

  internal override void Animate()
  {
    if (this.geometry != null)
    {
      this.geometry = (RectangleGeometry) null;
      if (this.sb != null)
        this.sb.Stop();
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    Rect seriesClipRect = this.Area.SeriesClipRect;
    this.geometry = new RectangleGeometry();
    this.SeriesRootPanel.Clip = (Geometry) this.geometry;
    Rect rect1 = this.IsActualTransposed ? new Rect(0.0, seriesClipRect.Bottom, seriesClipRect.Width, seriesClipRect.Height) : new Rect(0.0, seriesClipRect.Y, 0.0, seriesClipRect.Height);
    Rect rect2 = this.IsActualTransposed ? new Rect(0.0, seriesClipRect.Y, seriesClipRect.Width, seriesClipRect.Height) : new Rect(0.0, seriesClipRect.Y, seriesClipRect.Width, seriesClipRect.Height);
    RectAnimationUsingKeyFrames animation = new RectAnimationUsingKeyFrames();
    SplineRectKeyFrame keyFrame1 = new SplineRectKeyFrame(rect1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0)));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame1);
    SplineRectKeyFrame keyFrame2 = new SplineRectKeyFrame(rect2, KeyTime.FromTimeSpan(this.AnimationDuration));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame2);
    keyFrame2.KeySpline = new KeySpline(0.65, 0.84, 0.67, 0.95);
    this.geometry.BeginAnimation(RectangleGeometry.RectProperty, (AnimationTimeline) animation);
    this.AnimateAdornments();
  }

  internal override void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem = (int) e.NewItems[0];
        if (newItem < 0 || !this.ActualArea.GetEnableSegmentSelection())
          break;
        if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newItem);
        if (this.ActualArea != null && this.Segments.Count != 0)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            NewPointInfo = (object) this.GetDataPoint(newItem),
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (previousSelectedIndex != -1)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(previousSelectedIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        this.triggerSelectionChangedEventOnLoad = true;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int oldItem = (int) e.OldItems[0];
        ChartSelectionChangedEventArgs eventArgs1 = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = oldItem,
          PreviousSelectedIndex = this.PreviousSelectedIndex,
          OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex),
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1)
        {
          eventArgs1.PreviousSelectedSegment = this.Segments[0];
          eventArgs1.OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex);
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs1);
        this.OnResetSegment(oldItem);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  internal override void OnResetSegment(int index)
  {
    if (index < 0 || this.adornmentInfo == null)
      return;
    this.AdornmentPresenter.ResetAdornmentSelection(new int?(index), false);
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    List<int> intList = new List<int>();
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    int startIndex;
    int endIndex;
    Rect rect;
    this.CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);
    for (int index = startIndex; index <= endIndex; ++index)
    {
      this.hitPoint.X = this.IsIndexed ? (double) index : doubleList[index];
      this.hitPoint.Y = this.YValues[index];
      if (rect.Contains(this.hitPoint))
        intList.Add(index);
    }
    if (intList.Count <= 0)
      return this.dataPoint;
    int index1 = intList[intList.Count / 2];
    this.dataPoint = new ChartDataPointInfo();
    this.dataPoint.Index = index1;
    this.dataPoint.XData = doubleList[index1];
    this.dataPoint.YData = this.YValues[index1];
    this.dataPoint.Series = (ChartSeriesBase) this;
    if (index1 > -1 && this.ActualData.Count > index1)
      this.dataPoint.Item = this.ActualData[index1];
    return this.dataPoint;
  }

  internal override void UpdateTooltip(object originalSource)
  {
    if (!this.ShowTooltip)
      return;
    FrameworkElement source = originalSource as FrameworkElement;
    object obj1 = (object) null;
    int num = -1;
    if (source != null)
      obj1 = !(source.Tag is ChartSegment) ? (this.Segments.Count <= 0 ? (ChartExtensionUtils.GetAdornmentIndex((object) source) != -1 ? (object) new StepAreaSegment() : (object) (StepAreaSegment) null) : (object) this.Segments[0]) : source.Tag;
    if (!(obj1 is ChartSegment chartSegment) || chartSegment is TrendlineSegment || chartSegment.Item is Trendline)
      return;
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    object obj2 = (object) null;
    num = 0;
    if (this.Area.SeriesClipRect.Contains(this.mousePos))
    {
      this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      if (double.IsNaN(x))
        return;
      obj2 = this.ActualData[this.GetXValues().IndexOf(x)];
    }
    if (obj2 == null)
      return;
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    StepAreaSegment stepAreaSegment = obj1 as StepAreaSegment;
    stepAreaSegment.Item = obj2;
    stepAreaSegment.XData = x;
    stepAreaSegment.YData = y;
    if (chartTooltip1 == null)
      return;
    this.ToolTipTag = obj1;
    chartTooltip1.PolygonPath = " ";
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      chartTooltip1.DataContext = obj1;
      if (chartTooltip1.DataContext == null)
      {
        this.RemoveTooltip();
      }
      else
      {
        if (ChartTooltip.GetActualInitialShowDelay(this.ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this)) == 0)
        {
          this.HastoolTip = true;
          adorningCanvas.Children.Add((UIElement) chartTooltip1);
        }
        chartTooltip1.ContentTemplate = this.GetTooltipTemplate();
        this.AddTooltip();
        if (ChartTooltip.GetActualEnableAnimation(this.ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation((UIElement) this)))
        {
          this.SetDoubleAnimation(chartTooltip1);
          this._stopwatch.Start();
          Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
          Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
        }
        else
        {
          Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
          Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
          this._stopwatch.Start();
        }
      }
    }
    else
    {
      foreach (object child in adorningCanvas.Children)
      {
        if (child is ChartTooltip chartTooltip2)
          chartTooltip1 = chartTooltip2;
      }
      chartTooltip1.DataContext = obj1;
      if (chartTooltip1.DataContext == null)
      {
        this.RemoveTooltip();
      }
      else
      {
        this.AddTooltip();
        Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
        Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
      }
    }
  }

  internal override void Dispose()
  {
    if (this.geometry != null)
      this.geometry = (RectangleGeometry) null;
    if (this.sb != null)
    {
      this.sb.Stop();
      this.sb.Children.Clear();
      this.sb = (Storyboard) null;
    }
    base.Dispose();
  }

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea != null && this.ActualArea.SelectionBehaviour != null)
    {
      if (this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
      {
        if (this.SelectedSegmentsIndexes.Contains(oldIndex))
          this.SelectedSegmentsIndexes.Remove(oldIndex);
        this.OnResetSegment(oldIndex);
      }
      if (this.IsItemsSourceChanged)
        return;
      if (newIndex >= 0 && this.ActualArea.GetEnableSegmentSelection())
      {
        if (!this.SelectedSegmentsIndexes.Contains(newIndex))
          this.SelectedSegmentsIndexes.Add(newIndex);
        if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newIndex);
        if (this.ActualArea != null && this.Segments.Count != 0)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            NewPointInfo = (object) this.GetDataPoint(newIndex),
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex != -1)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newIndex;
        }
        else
          this.triggerSelectionChangedEventOnLoad = true;
      }
      else
      {
        if (newIndex != -1 || this.ActualArea == null)
          return;
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          OldPointInfo = (object) this.GetDataPoint(oldIndex),
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (oldIndex != -1)
        {
          eventArgs.PreviousSelectedSegment = this.Segments[0];
          eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
      }
    }
    else
    {
      if (newIndex < 0 || this.Segments.Count != 0)
        return;
      this.triggerSelectionChangedEventOnLoad = true;
    }
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new StepAreaSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new StepAreaSeries()
    {
      IsClosed = this.IsClosed,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as StepAreaSeries).UpdateArea();
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

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as StepAreaSeries).UpdateArea();
  }

  private void AnimateAdornments()
  {
    if (this.AdornmentsInfo == null)
      return;
    this.sb = new Storyboard();
    double totalSeconds = this.AnimationDuration.TotalSeconds;
    foreach (object child in this.AdornmentPresenter.Children)
    {
      DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
      SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
      keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
      keyFrame1.Value = 0.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds));
      keyFrame2.Value = 0.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
      keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds + 1.0));
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
    this.sb.Begin();
  }
}
