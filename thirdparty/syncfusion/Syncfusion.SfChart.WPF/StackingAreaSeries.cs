// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingAreaSeries
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

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StackingAreaSeries : StackingSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof (IsClosed), typeof (bool), typeof (StackingAreaSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(StackingAreaSeries.OnPropertyChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (StackingAreaSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(StackingAreaSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (StackingAreaSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingAreaSeries.OnSegmentSelectionBrush)));
  private Storyboard sb;
  private Point hitPoint = new Point();
  private RectangleGeometry geometry;

  public bool IsClosed
  {
    get => (bool) this.GetValue(StackingAreaSeries.IsClosedProperty);
    set => this.SetValue(StackingAreaSeries.IsClosedProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(StackingAreaSeries.SelectedIndexProperty);
    set => this.SetValue(StackingAreaSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(StackingAreaSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(StackingAreaSeries.SegmentSelectionBrushProperty, (object) value);
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

  protected override bool IsStacked => true;

  private StackingAreaSegment Segment { get; set; }

  public override void CreateSegments()
  {
    this.ClearUnUsedAdornments(this.DataCount);
    List<double> doubleList1 = new List<double>();
    List<double> doubleList2 = new List<double>();
    List<double> yVals = new List<double>();
    double Origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      Origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    List<double> doubleList3;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      this.Adornments.Clear();
      doubleList3 = this.GroupedXValuesIndexes;
    }
    else
      doubleList3 = this.GetXValues();
    StackingValues cumulativeStackValues = this.GetCumulativeStackValues((ChartSeriesBase) this);
    if (cumulativeStackValues == null)
      return;
    this.YRangeStartValues = cumulativeStackValues.StartValues;
    this.YRangeEndValues = cumulativeStackValues.EndValues;
    if (this.YRangeStartValues != null)
    {
      doubleList2.AddRange((IEnumerable<double>) doubleList3);
      yVals.AddRange((IEnumerable<double>) this.YRangeStartValues);
    }
    else
    {
      doubleList2.AddRange((IEnumerable<double>) doubleList3);
      yVals = doubleList3.Select<double, double>((Func<double, double>) (val => Origin)).ToList<double>();
    }
    doubleList2.AddRange((IEnumerable<double>) doubleList3.Select<double, double>((Func<double, double>) (val => val)).Reverse<double>().ToList<double>());
    yVals.AddRange((IEnumerable<double>) this.YRangeEndValues.Select<double, double>((Func<double, double>) (val => val)).Reverse<double>().ToList<double>());
    if (this.Segment == null || this.Segments.Count == 0)
    {
      this.Segment = this.CreateSegment() as StackingAreaSegment;
      if (this.Segment != null)
      {
        this.Segment.Series = (ChartSeriesBase) this;
        this.Segment.Item = (object) this.ActualData;
        this.Segment.SetData((IList<double>) doubleList2, (IList<double>) yVals);
        this.Segments.Add((ChartSegment) this.Segment);
      }
    }
    else
    {
      this.Segment.Item = (object) this.ActualData;
      this.Segment.SetData((IList<double>) doubleList2, (IList<double>) yVals);
    }
    if (this.ShowEmptyPoints)
      this.UpdateEmptyPointSegments(doubleList2, false);
    if (this.AdornmentsInfo == null)
      return;
    this.AddStackingAreaAdornments(this.YRangeEndValues);
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
    for (int index = startIndex; index < endIndex && index < this.YValues.Count; ++index)
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
    if (this.ActualData.Count > index1)
      this.dataPoint.Item = this.ActualData[index1];
    return this.dataPoint;
  }

  internal override bool GetAnimationIsActive() => this.geometry != null;

  internal override void Animate()
  {
    if (this.geometry != null)
    {
      this.geometry = (RectangleGeometry) null;
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
    if (this.AdornmentsInfo == null)
      return;
    string path1 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
    string path2 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";
    this.sb = new Storyboard();
    double num1 = this.AnimationDuration.TotalSeconds / (double) this.YValues.Count;
    int num2 = 0;
    foreach (FrameworkElement labelPresenter in this.AdornmentsInfo.LabelPresenters)
    {
      TransformGroup renderTransform = labelPresenter.RenderTransform as TransformGroup;
      ScaleTransform scaleTransform = new ScaleTransform()
      {
        ScaleX = 0.0,
        ScaleY = 0.0
      };
      if (renderTransform != null)
      {
        if (renderTransform.Children.Count > 0 && renderTransform.Children[0] is ScaleTransform)
          renderTransform.Children[0] = (Transform) scaleTransform;
        else
          renderTransform.Children.Insert(0, (Transform) scaleTransform);
      }
      labelPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      doubleAnimation1.From = new double?(0.3);
      doubleAnimation1.To = new double?(1.0);
      doubleAnimation1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      DoubleAnimation element1 = doubleAnimation1;
      element1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(path1, new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimation doubleAnimation2 = new DoubleAnimation();
      doubleAnimation2.From = new double?(0.3);
      doubleAnimation2.To = new double?(1.0);
      doubleAnimation2.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      DoubleAnimation element2 = doubleAnimation2;
      element2.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(path2, new object[0]));
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element2);
      ++num2;
    }
    this.sb.Begin();
  }

  internal override void UpdateTooltip(object originalSource)
  {
    if (!this.ShowTooltip)
      return;
    FrameworkElement source = originalSource as FrameworkElement;
    object obj1 = (object) null;
    int num = -1;
    if (source != null)
      obj1 = !(source.Tag is ChartSegment) ? (this.Segments.Count <= 0 ? (ChartExtensionUtils.GetAdornmentIndex((object) source) != -1 ? (object) new StackingAreaSegment() : (object) (StackingAreaSegment) null) : (object) this.Segments[0]) : source.Tag;
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
      int index = !(this.ActualXAxis is CategoryAxis) || !(this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues().IndexOf(x) : this.YValues.IndexOf(y);
      foreach (ChartSeries chartSeries in (Collection<ChartSeries>) this.Area.Series)
      {
        if (chartSeries == this && index > 0)
          obj2 = this.ActualData[index];
      }
    }
    if (obj2 == null)
      return;
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    StackingAreaSegment stackingAreaSegment = obj1 as StackingAreaSegment;
    stackingAreaSegment.Item = obj2;
    stackingAreaSegment.XData = x;
    stackingAreaSegment.YData = y;
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
          adorningCanvas.Children.Add((UIElement) chartTooltip1);
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

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    StackingAreaSegment toolTipTag = this.ToolTipTag as StackingAreaSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, this.YRangeEndValues[this.ActualData.IndexOf(toolTipTag.Item)]);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new StackingAreaSegment();

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (StackingAreaSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new StackingAreaSeries()
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
    (d as StackingAreaSeries).UpdateArea();
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
    (d as StackingAreaSeries).UpdateArea();
  }

  private void AddStackingAreaAdornments(IList<double> yValues)
  {
    IList<double> doubleList1 = this.YValues;
    List<double> doubleList2;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      doubleList2 = this.GroupedXValuesIndexes;
      doubleList1 = this.GroupedSeriesYValues[0];
    }
    else
      doubleList2 = this.GetXValues();
    for (int index = 0; index < doubleList2.Count; ++index)
    {
      double num = doubleList2[index];
      double yValue = yValues[index];
      if (index < this.Adornments.Count)
        this.Adornments[index].SetData(num, doubleList1[index], num, yValue);
      else
        this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num, doubleList1[index], num, yValue));
      this.Adornments[index].Item = this.ActualData[index];
    }
  }
}
