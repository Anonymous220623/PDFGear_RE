// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeAreaSeries
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

public class RangeAreaSeries : RangeSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty HighValueInteriorProperty = DependencyProperty.Register(nameof (HighValueInterior), typeof (Brush), typeof (RangeAreaSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeAreaSeries.OnHighValueChanged)));
  public static readonly DependencyProperty LowValueInteriorProperty = DependencyProperty.Register(nameof (LowValueInterior), typeof (Brush), typeof (RangeAreaSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeAreaSeries.OnLowValueChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (RangeAreaSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(RangeAreaSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (RangeAreaSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeAreaSeries.OnSegmentSelectionBrush)));
  private Storyboard sb;
  private RectangleGeometry geometry;
  private Point y1Value = new Point();
  private Point y2Value = new Point();

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(RangeAreaSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(RangeAreaSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public Brush HighValueInterior
  {
    get => (Brush) this.GetValue(RangeAreaSeries.HighValueInteriorProperty);
    set => this.SetValue(RangeAreaSeries.HighValueInteriorProperty, (object) value);
  }

  public Brush LowValueInterior
  {
    get => (Brush) this.GetValue(RangeAreaSeries.LowValueInteriorProperty);
    set => this.SetValue(RangeAreaSeries.LowValueInteriorProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(RangeAreaSeries.SelectedIndexProperty);
    set => this.SetValue(RangeAreaSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

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
    ChartPoint? nullable = new ChartPoint?();
    List<ChartPoint> segPoints1 = new List<ChartPoint>();
    List<double> doubleList = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList == null)
      return;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if (double.IsNaN(this.GroupedSeriesYValues[1][0]) || !double.IsNaN(this.GroupedSeriesYValues[0][0]))
      {
        segPoints1.Add(new ChartPoint(doubleList[0], this.GroupedSeriesYValues[1][0]));
        segPoints1.Add(new ChartPoint(doubleList[0], this.GroupedSeriesYValues[0][0]));
        this.AddSegment(this.GroupedSeriesYValues[0][0], this.GroupedSeriesYValues[1][0], this.ActualData[0], false, segPoints1);
      }
      List<ChartPoint> segPoints2 = new List<ChartPoint>();
      int index1;
      for (index1 = 0; index1 < doubleList.Count - 1; ++index1)
      {
        if (!double.IsNaN(this.GroupedSeriesYValues[1][index1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1]))
        {
          ChartPoint p11 = new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[1][index1]);
          ChartPoint p21 = new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[0][index1]);
          if (index1 == 0 || index1 < doubleList.Count - 1 && (double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) || double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1])))
          {
            segPoints2.Add(p11);
            segPoints2.Add(p21);
          }
          if (!double.IsNaN(this.GroupedSeriesYValues[1][index1 + 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 + 1]))
          {
            ChartPoint p12 = new ChartPoint(doubleList[index1 + 1], this.GroupedSeriesYValues[1][index1 + 1]);
            ChartPoint p22 = new ChartPoint(doubleList[index1 + 1], this.GroupedSeriesYValues[0][index1 + 1]);
            ChartPoint? crossPoint = ChartMath.GetCrossPoint(p11, p12, p21, p22);
            if (crossPoint.HasValue)
            {
              ChartPoint chartPoint = new ChartPoint(crossPoint.Value.X, crossPoint.Value.Y);
              segPoints2.Add(chartPoint);
              segPoints2.Add(chartPoint);
              this.AddSegment(this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1], this.GroupedSeriesYValues[1][index1] > this.GroupedSeriesYValues[0][index1], segPoints2);
              segPoints2 = new List<ChartPoint>();
              segPoints2.Add(chartPoint);
              segPoints2.Add(chartPoint);
            }
            segPoints2.Add(p12);
            segPoints2.Add(p22);
          }
          else if (index1 != 0 && !double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1]))
          {
            segPoints2.Add(p11);
            segPoints2.Add(p21);
          }
        }
        else
        {
          if (segPoints2.Count > 0 && !double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1]))
            this.AddSegment(this.GroupedSeriesYValues[0][index1 - 1], this.GroupedSeriesYValues[1][index1 - 1], this.ActualData[index1 - 1], false, segPoints2);
          segPoints2 = new List<ChartPoint>();
        }
      }
      if (segPoints2.Count > 0)
        this.AddSegment(this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1], this.GroupedSeriesYValues[1][index1] > this.GroupedSeriesYValues[0][index1], segPoints2);
      else if (index1 == doubleList.Count - 1 && (double.IsNaN(this.GroupedSeriesYValues[1][index1]) || double.IsNaN(this.GroupedSeriesYValues[0][index1])))
      {
        segPoints2.Add(new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[1][index1]));
        segPoints2.Add(new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[0][index1]));
        this.AddSegment(this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1], false, segPoints2);
      }
      for (int index2 = 0; index2 < doubleList.Count; ++index2)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(doubleList[index2], 0.0, this.GroupedSeriesYValues[0][index2], this.GroupedSeriesYValues[1][index2], index2);
      }
    }
    else
    {
      this.Segments.Clear();
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      if (doubleList != null)
      {
        if (double.IsNaN(this.LowValues[0]) || !double.IsNaN(this.HighValues[0]))
        {
          segPoints1.Add(new ChartPoint(doubleList[0], this.LowValues[0]));
          segPoints1.Add(new ChartPoint(doubleList[0], this.HighValues[0]));
          this.AddSegment(this.HighValues[0], this.LowValues[0], this.ActualData[0], false, segPoints1);
        }
        List<ChartPoint> segPoints3 = new List<ChartPoint>();
        int index;
        for (index = 0; index < this.DataCount - 1; ++index)
        {
          if (!double.IsNaN(this.LowValues[index]) && !double.IsNaN(this.HighValues[index]))
          {
            ChartPoint p11 = new ChartPoint(doubleList[index], this.LowValues[index]);
            ChartPoint p21 = new ChartPoint(doubleList[index], this.HighValues[index]);
            if (index == 0 || index < this.DataCount - 1 && (double.IsNaN(this.LowValues[index - 1]) || double.IsNaN(this.HighValues[index - 1])))
            {
              segPoints3.Add(p11);
              segPoints3.Add(p21);
            }
            if (!double.IsNaN(this.LowValues[index + 1]) && !double.IsNaN(this.HighValues[index + 1]))
            {
              ChartPoint p12 = new ChartPoint(doubleList[index + 1], this.LowValues[index + 1]);
              ChartPoint p22 = new ChartPoint(doubleList[index + 1], this.HighValues[index + 1]);
              ChartPoint? crossPoint = ChartMath.GetCrossPoint(p11, p12, p21, p22);
              if (crossPoint.HasValue)
              {
                ChartPoint chartPoint = new ChartPoint(crossPoint.Value.X, crossPoint.Value.Y);
                segPoints3.Add(chartPoint);
                segPoints3.Add(chartPoint);
                this.AddSegment(this.HighValues[index], this.LowValues[index], this.ActualData[index], this.LowValues[index] > this.HighValues[index], segPoints3);
                segPoints3 = new List<ChartPoint>();
                segPoints3.Add(chartPoint);
                segPoints3.Add(chartPoint);
              }
              segPoints3.Add(p12);
              segPoints3.Add(p22);
            }
            else if (index != 0 && !double.IsNaN(this.LowValues[index - 1]) && !double.IsNaN(this.HighValues[index - 1]))
            {
              segPoints3.Add(p11);
              segPoints3.Add(p21);
            }
          }
          else
          {
            if (segPoints3.Count > 0 && !double.IsNaN(this.LowValues[index - 1]) && !double.IsNaN(this.HighValues[index - 1]))
              this.AddSegment(this.HighValues[index - 1], this.LowValues[index - 1], this.ActualData[index - 1], false, segPoints3);
            segPoints3 = new List<ChartPoint>();
          }
        }
        if (segPoints3.Count > 0)
          this.AddSegment(this.HighValues[index], this.LowValues[index], this.ActualData[index], this.LowValues[index] > this.HighValues[index], segPoints3);
        else if (index == this.DataCount - 1 && (double.IsNaN(this.LowValues[index]) || double.IsNaN(this.HighValues[index])))
        {
          segPoints3.Add(new ChartPoint(doubleList[index], this.LowValues[index]));
          segPoints3.Add(new ChartPoint(doubleList[index], this.HighValues[index]));
          this.AddSegment(this.HighValues[index], this.LowValues[index], this.ActualData[index], false, segPoints3);
        }
      }
      for (int index = 0; index < doubleList.Count; ++index)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(doubleList[index], 0.0, this.HighValues[index], this.LowValues[index], index);
      }
    }
  }

  internal override void ResetAdornmentAnimationState()
  {
    if (this.adornmentInfo == null)
      return;
    foreach (object child in this.AdornmentPresenter.Children)
    {
      if (child is FrameworkElement frameworkElement)
      {
        frameworkElement.ClearValue(UIElement.RenderTransformProperty);
        frameworkElement.ClearValue(UIElement.OpacityProperty);
      }
    }
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
            PreviousSelectedSegment = (ChartSegment) null,
            NewPointInfo = (object) this.GetDataPoint(newItem),
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (previousSelectedIndex != -1 && previousSelectedIndex < this.ActualData.Count)
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
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.ActualData.Count)
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
    for (int index = startIndex; index < endIndex; ++index)
    {
      this.y1Value.X = this.IsIndexed ? (double) index : doubleList[index];
      this.y1Value.Y = this.ActualSeriesYValues[0][index];
      this.y2Value.X = this.IsIndexed ? (double) index : doubleList[index];
      this.y2Value.Y = this.ActualSeriesYValues[1][index];
      if (rect.Contains(this.y1Value) || rect.Contains(this.y2Value))
        intList.Add(index);
    }
    if (intList.Count <= 0)
      return this.dataPoint;
    int index1 = intList[intList.Count / 2];
    this.dataPoint = new ChartDataPointInfo();
    this.dataPoint.Index = index1;
    this.dataPoint.XData = doubleList[index1];
    this.dataPoint.High = this.ActualSeriesYValues[0][index1];
    this.dataPoint.Low = this.ActualSeriesYValues[1][index1];
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
    if (source != null)
      obj1 = !(source.Tag is ChartSegment) ? (this.Segments.Count <= 0 ? (ChartExtensionUtils.GetAdornmentIndex((object) source) != -1 ? (object) new RangeAreaSegment() : (object) (RangeAreaSegment) null) : (object) this.Segments[0]) : source.Tag;
    if (!(obj1 is ChartSegment chartSegment) || chartSegment is TrendlineSegment || chartSegment.Item is Trendline)
      return;
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    object obj2 = (object) null;
    int index = 0;
    if (this.Area.SeriesClipRect.Contains(this.mousePos))
    {
      this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      if (double.IsNaN(x))
        return;
      index = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues().IndexOf(x) : this.GroupedSeriesYValues[0].IndexOf(y);
      obj2 = this.ActualData[index];
    }
    if (obj2 == null)
      return;
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    RangeAreaSegment rangeAreaSegment = obj1 as RangeAreaSegment;
    rangeAreaSegment.Item = obj2;
    if (((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>() > 1)
    {
      rangeAreaSegment.High = this.ActualSeriesYValues[0][index];
      rangeAreaSegment.Low = this.ActualSeriesYValues[1][index];
    }
    if (chartTooltip1 == null)
      return;
    this.ToolTipTag = (object) rangeAreaSegment;
    chartTooltip1.PolygonPath = " ";
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      chartTooltip1.DataContext = (object) rangeAreaSegment;
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
      chartTooltip1.DataContext = (object) rangeAreaSegment;
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
    RangeAreaSegment toolTipTag = this.ToolTipTag as RangeAreaSegment;
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    Point dataPointPosition = new Point();
    if (!this.Area.SeriesClipRect.Contains(this.mousePos))
      return dataPointPosition;
    this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
    if (double.IsNaN(x))
      return dataPointPosition;
    Point visible = this.ChartTransformer.TransformToVisible(x, toolTipTag.High);
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
          if (oldIndex != -1 && oldIndex < this.ActualData.Count)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
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
        if (oldIndex != -1 && oldIndex < this.ActualData.Count)
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new RangeAreaSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new RangeAreaSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      HighValueInterior = this.HighValueInterior,
      LowValueInterior = this.LowValueInterior
    });
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  protected Point? GetCrossPoint(ChartPoint p11, ChartPoint p12, ChartPoint p21, ChartPoint p22)
  {
    Point point = new Point();
    double num1 = (p12.Y - p11.Y) * (p21.X - p22.X) - (p21.Y - p22.Y) * (p12.X - p11.X);
    double num2 = (p12.Y - p11.Y) * (p21.X - p11.X) - (p21.Y - p11.Y) * (p12.X - p11.X);
    double num3 = (p21.Y - p11.Y) * (p21.X - p22.X) - (p21.Y - p22.Y) * (p21.X - p11.X);
    if (num1 == 0.0 && num2 == 0.0 && num3 == 0.0)
      return new Point?();
    double num4 = num2 / num1;
    double num5 = num3 / num1;
    point.X = p11.X + (p12.X - p11.X) * num5;
    point.Y = p11.Y + (p12.Y - p11.Y) * num5;
    return 0.0 <= num4 && num4 <= 1.0 && 0.0 <= num5 && num5 <= 1.0 ? new Point?(point) : new Point?();
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
    (d as RangeAreaSeries).UpdateArea();
  }

  private static void OnHighValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    RangeAreaSeries rangeAreaSeries = d as RangeAreaSeries;
    foreach (ChartSegment segment in (Collection<ChartSegment>) rangeAreaSeries.Segments)
      (segment as RangeAreaSegment).HighValueInterior = rangeAreaSeries.HighValueInterior;
  }

  private static void OnLowValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    RangeAreaSeries rangeAreaSeries = d as RangeAreaSeries;
    foreach (ChartSegment segment in (Collection<ChartSegment>) rangeAreaSeries.Segments)
      (segment as RangeAreaSegment).LowValueInterior = rangeAreaSeries.LowValueInterior;
  }

  private void AddSegment(
    double highValue,
    double lowValue,
    object actualData,
    bool isHighLow,
    List<ChartPoint> segPoints)
  {
    if (!(this.CreateSegment() is RangeAreaSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.High = highValue;
    segment.Low = lowValue;
    segment.Item = actualData;
    segment.IsHighLow = isHighLow;
    segment.HighValueInterior = this.HighValueInterior;
    segment.LowValueInterior = this.LowValueInterior;
    segment.SetData(segPoints);
    this.Segments.Add((ChartSegment) segment);
  }

  private void AnimateAdornments()
  {
    if (this.AdornmentsInfo == null)
      return;
    string path1 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
    string path2 = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";
    this.sb = new Storyboard();
    double num1 = this.AnimationDuration.TotalSeconds / (double) this.HighValues.Count;
    int num2 = 0;
    int num3 = 0;
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
      AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
      DoubleAnimation element1 = new DoubleAnimation();
      element1.From = new double?(0.3);
      element1.To = new double?(1.0);
      element1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      if (adornmentPosition == AdornmentsPosition.TopAndBottom)
        element1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num3 * num1));
      else
        element1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(path1, new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimation element2 = new DoubleAnimation();
      element2.From = new double?(0.3);
      element2.To = new double?(1.0);
      element2.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      if (adornmentPosition == AdornmentsPosition.TopAndBottom)
        element2.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num3 * num1));
      else
        element2.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(path2, new object[0]));
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element2);
      ++num2;
      if (adornmentPosition == AdornmentsPosition.TopAndBottom && num2 % 2 == 0)
        ++num3;
    }
    this.sb.Begin();
  }
}
