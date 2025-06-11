// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingLineSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
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

public class StackingLineSeries : StackingSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (StackingLineSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(StackingLineSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (StackingLineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingLineSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (StackingLineSeries), new PropertyMetadata((PropertyChangedCallback) null));
  private Storyboard sb;
  private Point hitPoint = new Point();
  private RectangleGeometry geometry;

  public int SelectedIndex
  {
    get => (int) this.GetValue(StackingLineSeries.SelectedIndexProperty);
    set => this.SetValue(StackingLineSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(StackingLineSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(StackingLineSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(StackingLineSeries.StrokeDashArrayProperty);
    set => this.SetValue(StackingLineSeries.StrokeDashArrayProperty, (object) value);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new LineSegment();

  protected override bool IsStacked => true;

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    double Origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      Origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    StackingValues cumulativeStackValues = this.GetCumulativeStackValues((ChartSeriesBase) this);
    List<double> doubleList = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (cumulativeStackValues == null)
      return;
    this.YRangeStartValues = cumulativeStackValues.StartValues;
    this.YRangeEndValues = cumulativeStackValues.EndValues;
    if (this.YRangeStartValues == null)
      this.YRangeStartValues = (IList<double>) doubleList.Select<double, double>((Func<double, double>) (val => Origin)).ToList<double>();
    if (doubleList == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index1 = 0; index1 < doubleList.Count; ++index1)
      {
        int index2 = index1 + 1;
        if (this.GroupedSeriesYValues != null)
        {
          if (index2 < doubleList.Count)
            this.CreateSegment(new double[4]
            {
              doubleList[index1],
              this.YRangeEndValues[index1],
              doubleList[index2],
              this.YRangeEndValues[index2]
            }, this.ActualData[index1]);
          if (this.AdornmentsInfo != null)
          {
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, doubleList[index1], this.YRangeEndValues[index1], doubleList[index1], this.YRangeEndValues[index1]));
            this.Adornments[index1].Item = this.ActualData[index1];
          }
        }
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        int index4 = index3 + 1;
        if (index3 < this.Segments.Count)
        {
          this.Segments[index3].Item = this.ActualData[index3];
          if (index4 < this.DataCount)
          {
            this.Segments[index3].SetData(doubleList[index3], this.YRangeEndValues[index3], doubleList[index4], this.YRangeEndValues[index4]);
            (this.Segments[index3] as LineSegment).Item = this.ActualData[index3];
            (this.Segments[index3] as LineSegment).YData = this.YRangeEndValues[index3];
            if (this.SegmentColorPath != null && !this.Segments[index3].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index3].IsSelectedSegment)
              this.Segments[index3].Interior = this.Interior != null ? this.Interior : this.ColorValues[index3];
          }
          else
            this.Segments.RemoveAt(index3);
        }
        else if (index4 < this.DataCount)
          this.CreateSegment(new double[4]
          {
            doubleList[index3],
            this.YRangeEndValues[index3],
            doubleList[index4],
            this.YRangeEndValues[index4]
          }, this.ActualData[index3]);
        if (this.AdornmentsInfo != null)
        {
          if (index3 < this.Adornments.Count)
            this.Adornments[index3].SetData(doubleList[index3], this.YRangeEndValues[index3], doubleList[index3], this.YRangeEndValues[index3]);
          else
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, doubleList[index3], this.YRangeEndValues[index3], doubleList[index3], this.YRangeEndValues[index3]));
          this.Adornments[index3].Item = this.ActualData[index3];
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(doubleList, false);
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
        if (this.ActualArea != null && newItem < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newItem],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = previousSelectedIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newItem].Item,
            IsSelected = true
          };
          if (previousSelectedIndex != -1)
          {
            if (previousSelectedIndex == this.Segments.Count)
            {
              eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex - 1];
              eventArgs.OldPointInfo = this.Segments[previousSelectedIndex - 1].Item;
            }
            else
            {
              eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex];
              eventArgs.OldPointInfo = this.Segments[previousSelectedIndex].Item;
            }
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.ActualArea != null && this.Segments.Count > 0 && newItem == this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newItem - 1],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = previousSelectedIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newItem - 1].Item,
            IsSelected = true
          };
          if (previousSelectedIndex != -1)
          {
            if (previousSelectedIndex == this.Segments.Count)
            {
              eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex - 1];
              eventArgs.OldPointInfo = this.Segments[previousSelectedIndex - 1].Item;
            }
            else
            {
              eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex];
              eventArgs.OldPointInfo = this.Segments[previousSelectedIndex].Item;
            }
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.Segments.Count != 0)
          break;
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
          PreviousSelectedSegment = this.Segments[this.PreviousSelectedIndex],
          PreviousSelectedSeries = (ChartSeriesBase) this,
          OldPointInfo = this.Segments[this.PreviousSelectedIndex].Item,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.Segments.Count)
          this.selectionChangedEventArgs.PreviousSelectedSegment = (ChartSegment) this.GetDataPoint(this.PreviousSelectedIndex);
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

  internal override void UpdateTooltip(object originalSource)
  {
    if (!this.ShowTooltip)
      return;
    FrameworkElement source = originalSource as FrameworkElement;
    object obj1 = (object) null;
    int num = -1;
    if (source != null)
      obj1 = !(source.Tag is ChartSegment) ? (this.Segments.Count <= 0 ? (ChartExtensionUtils.GetAdornmentIndex((object) source) != -1 ? (object) new LineSegment() : (object) (LineSegment) null) : (object) this.Segments[0]) : source.Tag;
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
        if (chartSeries == this && index >= 0)
          obj2 = this.ActualData[index];
      }
    }
    if (obj2 == null)
      return;
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    LineSegment lineSegment = obj1 as LineSegment;
    lineSegment.Item = obj2;
    lineSegment.XData = x;
    lineSegment.YData = y;
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

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    LineSegment toolTipTag = this.ToolTipTag as LineSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, this.YRangeEndValues[this.ActualData.IndexOf(toolTipTag.Item)]);
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
    Rect rect2 = this.IsActualTransposed ? new Rect(0.0, seriesClipRect.Y - seriesClipRect.Top, seriesClipRect.Width, seriesClipRect.Height) : new Rect(0.0, seriesClipRect.Y, seriesClipRect.Width, seriesClipRect.Height);
    RectAnimationUsingKeyFrames animation = new RectAnimationUsingKeyFrames();
    SplineRectKeyFrame keyFrame1 = new SplineRectKeyFrame(rect1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0)));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame1);
    SplineRectKeyFrame keyFrame2 = new SplineRectKeyFrame(rect2, KeyTime.FromTimeSpan(this.AnimationDuration));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame2);
    keyFrame2.KeySpline = new KeySpline(0.65, 0.84, 0.67, 0.95);
    this.geometry.BeginAnimation(RectangleGeometry.RectProperty, (AnimationTimeline) animation);
    if (this.AdornmentsInfo == null)
      return;
    this.sb = new Storyboard();
    double num1 = this.AnimationDuration.TotalSeconds / (double) this.YValues.Count;
    string path1 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
    string path2 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";
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
      doubleAnimation1.From = new double?(0.6);
      doubleAnimation1.To = new double?(1.0);
      doubleAnimation1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      DoubleAnimation element1 = doubleAnimation1;
      element1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(path1, new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimation doubleAnimation2 = new DoubleAnimation();
      doubleAnimation2.From = new double?(0.6);
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

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new StackingLineSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
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

  private void CreateSegment(double[] values, object actualData)
  {
    if (!(this.CreateSegment() is LineSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.Item = actualData;
    segment.SetData(values);
    this.Segments.Add((ChartSegment) segment);
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
    (d as StackingLineSeries).UpdateArea();
  }
}
