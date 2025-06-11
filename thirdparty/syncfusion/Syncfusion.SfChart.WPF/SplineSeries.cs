// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SplineSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
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

public class SplineSeries : XySeriesDraggingBase, ISegmentSelectable
{
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (SplineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(SplineSeries.OnCustomTemplateChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (SplineSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(SplineSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (SplineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(SplineSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SplineTypeProperty = DependencyProperty.Register(nameof (SplineType), typeof (SplineType), typeof (SplineSeries), new PropertyMetadata((object) SplineType.Natural, new PropertyChangedCallback(SplineSeries.OnSplineTypeChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (SplineSeries), new PropertyMetadata((PropertyChangedCallback) null));
  private Storyboard sb;
  private double offsetPosition;
  private double initialPosition;
  private bool isDragged;
  private bool isSeriesCaptured;
  private List<SplineSegment> segments;
  private List<double> previewYValues;
  private Point hitPoint = new Point();
  private List<ChartPoint> startControlPoints;
  private List<ChartPoint> endControlPoints;
  private RectangleGeometry geometry;

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(SplineSeries.CustomTemplateProperty);
    set => this.SetValue(SplineSeries.CustomTemplateProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(SplineSeries.SelectedIndexProperty);
    set => this.SetValue(SplineSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(SplineSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(SplineSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public SplineType SplineType
  {
    get => (SplineType) this.GetValue(SplineSeries.SplineTypeProperty);
    set => this.SetValue(SplineSeries.SplineTypeProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(SplineSeries.StrokeDashArrayProperty);
    set => this.SetValue(SplineSeries.StrokeDashArrayProperty, (object) value);
  }

  protected internal override bool IsLinear => true;

  protected internal override List<ChartSegment> SelectedSegments
  {
    get
    {
      return this.SelectedSegmentsIndexes.Count > 0 && this.Segments.Count != 0 ? this.SelectedSegmentsIndexes.Where<int>((Func<int, bool>) (index => index <= this.Segments.Count)).Select<int, ChartSegment>((Func<int, ChartSegment>) (index => index != this.Segments.Count ? this.Segments[index] : this.Segments[index - 1])).ToList<ChartSegment>() : (List<ChartSegment>) null;
    }
  }

  public override void CreateSegments()
  {
    double[] ys2 = (double[]) null;
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if (this.SplineType == SplineType.Monotonic)
        this.GetMonotonicSpline(xValues, this.GroupedSeriesYValues[0]);
      else if (this.SplineType == SplineType.Cardinal)
        this.GetCardinalSpline(xValues, this.GroupedSeriesYValues[0]);
      else
        this.NaturalSpline(xValues, this.GroupedSeriesYValues[0], out ys2);
      for (int index1 = 0; index1 < xValues.Count; ++index1)
      {
        int index2 = index1 + 1;
        ChartPoint point1 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
        if (index2 < xValues.Count && index2 < this.GroupedSeriesYValues[0].Count)
        {
          ChartPoint chartPoint = new ChartPoint(xValues[index2], this.GroupedSeriesYValues[0][index2]);
          ChartPoint controlPoint1;
          ChartPoint controlPoint2;
          if (this.SplineType == SplineType.Monotonic)
          {
            controlPoint1 = this.startControlPoints[index2 - 1];
            controlPoint2 = this.endControlPoints[index2 - 1];
          }
          else if (this.SplineType == SplineType.Cardinal)
          {
            controlPoint1 = this.startControlPoints[index2 - 1];
            controlPoint2 = this.endControlPoints[index2 - 1];
          }
          else
            this.GetBezierControlPoints(point1, chartPoint, ys2[index1], ys2[index2], out controlPoint1, out controlPoint2);
          SplineSegment segment = this.CreateSegment() as SplineSegment;
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(point1, controlPoint1, controlPoint2, chartPoint);
          segment.X1 = xValues[index1];
          segment.X2 = xValues[index2];
          segment.Y1 = this.GroupedSeriesYValues[0][index1];
          segment.Y2 = this.GroupedSeriesYValues[0][index2];
          segment.Item = this.ActualData[index1];
          this.Segments.Add((ChartSegment) segment);
        }
        else if (index2 == this.Segments.Count)
          this.Segments.RemoveAt(index1);
        if (this.AdornmentsInfo != null)
          this.AddAdornmentAtXY(point1.X, point1.Y, index1);
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      if (this.SplineType == SplineType.Monotonic)
        this.GetMonotonicSpline(xValues, this.YValues);
      else if (this.SplineType == SplineType.Cardinal)
        this.GetCardinalSpline(xValues, this.YValues);
      else
        this.NaturalSpline(xValues, this.YValues, out ys2);
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        int index4 = index3 + 1;
        ChartPoint point1 = new ChartPoint(xValues[index3], this.YValues[index3]);
        if (index4 < this.DataCount)
        {
          ChartPoint chartPoint = new ChartPoint(xValues[index4], this.YValues[index4]);
          ChartPoint controlPoint1;
          ChartPoint controlPoint2;
          if (this.SplineType == SplineType.Monotonic)
          {
            controlPoint1 = this.startControlPoints[index4 - 1];
            controlPoint2 = this.endControlPoints[index4 - 1];
          }
          else if (this.SplineType == SplineType.Cardinal)
          {
            controlPoint1 = this.startControlPoints[index4 - 1];
            controlPoint2 = this.endControlPoints[index4 - 1];
          }
          else
            this.GetBezierControlPoints(point1, chartPoint, ys2[index3], ys2[index4], out controlPoint1, out controlPoint2);
          if (index3 < this.Segments.Count && this.Segments[index3] is SplineSegment)
          {
            this.Segments[index3].SetData(point1, controlPoint1, controlPoint2, chartPoint);
            (this.Segments[index3] as SplineSegment).X1 = xValues[index3];
            (this.Segments[index3] as SplineSegment).X2 = xValues[index4];
            (this.Segments[index3] as SplineSegment).Y1 = this.YValues[index3];
            (this.Segments[index3] as SplineSegment).Y2 = this.YValues[index4];
            (this.Segments[index3] as SplineSegment).Item = this.ActualData[index3];
            (this.Segments[index3] as SplineSegment).YData = this.YValues[index3];
            if (this.SegmentColorPath != null && !this.Segments[index3].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index3].IsSelectedSegment)
              this.Segments[index3].Interior = this.Interior != null ? this.Interior : this.ColorValues[index3];
          }
          else
          {
            SplineSegment segment = this.CreateSegment() as SplineSegment;
            segment.Series = (ChartSeriesBase) this;
            segment.SetData(point1, controlPoint1, controlPoint2, chartPoint);
            segment.X1 = xValues[index3];
            segment.X2 = xValues[index4];
            segment.Y1 = this.YValues[index3];
            segment.Y2 = this.YValues[index4];
            segment.Item = this.ActualData[index3];
            this.Segments.Add((ChartSegment) segment);
          }
        }
        else if (index4 == this.Segments.Count)
          this.Segments.RemoveAt(index3);
        if (this.AdornmentsInfo != null)
          this.AddAdornmentAtXY(point1.X, point1.Y, index3);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
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
            PreviousSelectedSeries = (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newItem].Item,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
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
            PreviousSelectedSeries = (ChartSeriesBase) null,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
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
          OldPointInfo = this.Segments[this.PreviousSelectedIndex].Item,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.Segments.Count)
          this.selectionChangedEventArgs.PreviousSelectedSegment = this.Segments[this.PreviousSelectedIndex];
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
    object obj = (object) null;
    if (source != null)
    {
      if (source.Tag is ChartSegment)
        obj = source.Tag;
      else if (source.DataContext is ChartSegment && !(source.DataContext is ChartAdornment))
      {
        obj = source.DataContext;
      }
      else
      {
        int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) source);
        if (adornmentIndex != -1)
        {
          if (adornmentIndex < this.Segments.Count)
            obj = (object) this.Segments[adornmentIndex];
          else if (adornmentIndex < this.Adornments.Count)
          {
            ChartAdornment adornment = this.Adornments[adornmentIndex];
            SplineSegment splineSegment = new SplineSegment();
            splineSegment.X1Data = adornment.XData;
            splineSegment.Y1Data = adornment.YData;
            splineSegment.X1 = adornment.XData;
            splineSegment.Y1 = adornment.YData;
            splineSegment.XData = adornment.XData;
            splineSegment.YData = adornment.YData;
            splineSegment.Item = adornment.Item;
            splineSegment.Series = adornment.Series;
            splineSegment.Interior = adornment.Interior;
            splineSegment.IsAddedToVisualTree = adornment.IsAddedToVisualTree;
            splineSegment.IsEmptySegmentInterior = adornment.IsEmptySegmentInterior;
            splineSegment.IsSegmentVisible = adornment.IsSegmentVisible;
            splineSegment.IsSelectedSegment = adornment.IsSelectedSegment;
            splineSegment.PolygonPoints = adornment.PolygonPoints;
            splineSegment.Stroke = adornment.Stroke;
            splineSegment.StrokeDashArray = adornment.StrokeDashArray;
            splineSegment.StrokeThickness = adornment.StrokeThickness;
            splineSegment.XRange = adornment.XRange;
            splineSegment.YRange = adornment.YRange;
            obj = (object) splineSegment;
          }
        }
      }
    }
    if (!(obj is ChartSegment chartSegment) || chartSegment is TrendlineSegment || chartSegment.Item is Trendline)
      return;
    if (this.TooltipTemplate != null && chartSegment.Item == this.ActualData[this.Segments.Count] && this.Segments.Contains(chartSegment) && this.Adornments != null && this.Adornments.Count == 0)
    {
      SplineSegment splineSegment1 = chartSegment as SplineSegment;
      SplineSegment splineSegment2 = new SplineSegment();
      splineSegment2.X1Data = splineSegment1.X1Data;
      splineSegment2.Y1Data = splineSegment1.Y1Data;
      splineSegment2.X1 = splineSegment1.X1;
      splineSegment2.Y1 = splineSegment1.Y1;
      splineSegment2.Y2 = splineSegment1.Y2;
      splineSegment2.XData = splineSegment1.XData;
      splineSegment2.YData = splineSegment1.YData;
      splineSegment2.Item = splineSegment1.Item;
      splineSegment2.Series = splineSegment1.Series;
      splineSegment2.Interior = splineSegment1.Interior;
      splineSegment2.IsAddedToVisualTree = splineSegment1.IsAddedToVisualTree;
      splineSegment2.IsEmptySegmentInterior = splineSegment1.IsEmptySegmentInterior;
      splineSegment2.IsSegmentVisible = splineSegment1.IsSegmentVisible;
      splineSegment2.IsSelectedSegment = splineSegment1.IsSelectedSegment;
      splineSegment2.PolygonPoints = splineSegment1.PolygonPoints;
      splineSegment2.Stroke = splineSegment1.Stroke;
      splineSegment2.StrokeDashArray = splineSegment1.StrokeDashArray;
      splineSegment2.StrokeThickness = splineSegment1.StrokeThickness;
      splineSegment2.X2 = splineSegment1.X2;
      splineSegment2.P1 = splineSegment1.P1;
      splineSegment2.P2 = splineSegment1.P2;
      splineSegment2.Q1 = splineSegment1.Q1;
      splineSegment2.XRange = splineSegment1.XRange;
      splineSegment2.Q2 = splineSegment1.Q2;
      splineSegment2.Data = splineSegment1.Data;
      splineSegment2.YRange = splineSegment1.YRange;
      obj = (object) splineSegment2;
    }
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    if (chartTooltip1 == null)
      return;
    SplineSegment lineSegment = obj as SplineSegment;
    this.SetTooltipSegmentItem(lineSegment);
    this.ToolTipTag = (object) lineSegment;
    chartTooltip1.PolygonPath = " ";
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      chartTooltip1.DataContext = (object) lineSegment;
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
      chartTooltip1.DataContext = (object) lineSegment;
      if (chartTooltip1.DataContext == null)
      {
        this.RemoveTooltip();
      }
      else
      {
        chartTooltip1.ContentTemplate = this.GetTooltipTemplate();
        this.AddTooltip();
        Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
        Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
      }
    }
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    SplineSegment toolTipTag = this.ToolTipTag as SplineSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  private void SetTooltipSegmentItem(SplineSegment lineSegment)
  {
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    Point point = new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top);
    this.FindNearestChartPoint(point, out x, out y, out stackedYValue);
    if (lineSegment != null)
      lineSegment.YData = y == lineSegment.Y1 ? lineSegment.Y1 : lineSegment.Y2;
    lineSegment.XData = x;
    int index1 = this.GetXValues().IndexOf(x);
    if (!this.IsIndexed)
    {
      IList<double> xvalues = (IList<double>) this.GetXValues();
      int index2 = index1;
      double num1 = this.ActualSeriesYValues[0][index2];
      for (; !this.IsIndexed && xvalues.Count > index2 && xvalues[index2] == x; ++index2)
      {
        double num2 = this.ActualArea.PointToValue(this.ActualYAxis, point);
        double num3 = this.ActualSeriesYValues[0][index2];
        if (Math.Abs(num2 - num3) <= Math.Abs(num2 - num1))
        {
          index1 = index2;
          num1 = num3;
        }
      }
    }
    if (index1 <= 0)
      return;
    lineSegment.Item = this.ActualData[index1];
  }

  internal override bool GetAnimationIsActive() => this.geometry != null;

  internal override void Animate()
  {
    Rect seriesClipRect = this.Area.SeriesClipRect;
    if (this.geometry != null)
    {
      this.geometry = (RectangleGeometry) null;
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
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

  internal override void ActivateDragging(Point mousePos, object chartElement)
  {
    if (this.DraggingSegment != null || this.segments != null || !(chartElement is FrameworkElement frameworkElement))
      return;
    base.ActivateDragging(mousePos, chartElement);
    if (this.SegmentIndex < 0)
      return;
    if (frameworkElement.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is SplineSegment)
      this.DraggingSegment = (ChartSegment) (templatedParent.Content as SplineSegment);
    else
      this.DraggingSegment = (ChartSegment) (frameworkElement.Tag as SplineSegment);
    if (this.EnableSeriesDragging && (this.DraggingSegment != null || frameworkElement.DataContext is ChartAdornmentInfo || frameworkElement.DataContext is ChartAdornmentContainer))
    {
      this.PreviewSeries = (UIElement) this;
      this.isSeriesCaptured = true;
      this.offsetPosition = this.initialPosition = this.IsActualTransposed ? mousePos.X : mousePos.Y;
    }
    else
    {
      if (this.DraggingSegment == null && !(frameworkElement.DataContext is ChartAdornmentInfo) && !(frameworkElement.DataContext is ChartAdornmentContainer))
        return;
      double x;
      this.FindNearestChartPoint(mousePos, out x, out double _, out double _);
      this.SegmentIndex = this.IsIndexed || this.ActualXValues is IList<string> ? (int) x : (int) (double) ((IList<double>) this.ActualXValues).IndexOf(x);
      if (this.SegmentIndex == this.Segments.Count)
        this.DraggingSegment = (ChartSegment) (this.Segments[this.SegmentIndex - 1] as SplineSegment);
      else
        this.DraggingSegment = (ChartSegment) (this.Segments[this.SegmentIndex] as SplineSegment);
    }
  }

  internal override void UpdatePreviewSegmentDragging(Point mousePos)
  {
    this.UpdatePreviewSegmentAndSeries(mousePos);
    base.UpdatePreviewSegmentDragging(mousePos);
  }

  internal override void UpdatePreivewSeriesDragging(Point mousePos)
  {
    this.UpdatePreviewSegmentAndSeries(mousePos);
    base.UpdatePreivewSeriesDragging(mousePos);
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
        if (this.ActualArea != null && newIndex < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newIndex],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newIndex].Item,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex >= 0 && oldIndex <= this.Segments.Count)
          {
            if (oldIndex == this.Segments.Count)
            {
              eventArgs.PreviousSelectedSegment = this.Segments[oldIndex - 1];
              eventArgs.OldPointInfo = this.Segments[oldIndex - 1].Item;
            }
            else
            {
              eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
              eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
            }
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newIndex;
        }
        else if (this.ActualArea != null && this.Segments.Count > 0 && newIndex == this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newIndex - 1],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = (ChartSeriesBase) null,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex >= 0 && oldIndex <= this.Segments.Count)
          {
            if (oldIndex == this.Segments.Count)
            {
              eventArgs.PreviousSelectedSegment = this.Segments[oldIndex - 1];
              eventArgs.OldPointInfo = this.Segments[oldIndex - 1].Item;
            }
            else
            {
              eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
              eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
            }
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newIndex;
        }
        else
        {
          if (this.Segments.Count != 0)
            return;
          this.triggerSelectionChangedEventOnLoad = true;
        }
      }
      else if (newIndex == -1 && this.ActualArea != null && oldIndex < this.Segments.Count)
      {
        (this.ActualArea as SfChart).OnSelectionChanged(new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          PreviousSelectedSegment = this.Segments[oldIndex],
          OldPointInfo = this.Segments[oldIndex].Item,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        });
      }
      else
      {
        if (newIndex != -1 || this.ActualArea == null || this.Segments.Count <= 0 || oldIndex != this.Segments.Count)
          return;
        (this.ActualArea as SfChart).OnSelectionChanged(new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          PreviousSelectedSegment = this.Segments[oldIndex - 1],
          OldPointInfo = this.Segments[oldIndex - 1].Item,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        });
      }
    }
    else
    {
      if (newIndex < 0 || this.Segments.Count != 0)
        return;
      this.triggerSelectionChangedEventOnLoad = true;
    }
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new SplineSegment();

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new SplineSeries()
    {
      CustomTemplate = this.CustomTemplate,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  protected override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    if (this.SeriesPanel == null)
      return;
    base.ResetDraggingElements(reason, dragEndEvent);
    if (this.segments != null)
    {
      foreach (ChartSegment segment in this.segments)
        this.SeriesPanel.Children.Remove(segment.GetRenderedVisual());
      this.segments.Clear();
      this.segments = (List<SplineSegment>) null;
    }
    this.isSeriesCaptured = false;
    this.DraggingSegment = (ChartSegment) null;
    this.isDragged = false;
    this.PreviewSeries = (UIElement) null;
  }

  protected override void OnChartDragStart(Point mousePos, object originalSource)
  {
    if (!this.EnableSeriesDragging && !this.EnableSegmentDragging)
      return;
    this.ActivateDragging(mousePos, originalSource);
  }

  protected override void OnChartDragEnd(Point mousePos, object originalSource)
  {
    this.UpdateDraggedSource();
    base.OnChartDragEnd(mousePos, originalSource);
  }

  protected void GetCardinalSpline(List<double> xValues, IList<double> yValues)
  {
    this.startControlPoints = new List<ChartPoint>(this.DataCount);
    this.endControlPoints = new List<ChartPoint>(this.DataCount);
    int length = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    double[] numArray1 = new double[length];
    double[] numArray2 = new double[length];
    for (int index = 0; index < length; ++index)
    {
      if (index == 0 && xValues.Count > 2)
        numArray1[index] = 0.5 * (xValues[index + 2] - xValues[index]);
      else if (index == length - 1 && length - 3 >= 0)
        numArray1[index] = 0.5 * (xValues[length - 1] - xValues[length - 3]);
      else if (index - 1 >= 0 && xValues.Count > index + 1)
        numArray1[index] = 0.5 * (xValues[index + 1] - xValues[index - 1]);
      if (double.IsNaN(numArray1[index]))
        numArray1[index] = 0.0;
      if (this.ActualXAxis is DateTimeAxis)
      {
        DateTime dateTime = xValues[index].FromOADate();
        if ((this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Auto || (this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Years)
        {
          int num = DateTime.IsLeapYear(dateTime.Year) ? 366 : 365;
          numArray2[index] = numArray1[index] / (double) num;
        }
        else if ((this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Months)
        {
          double num = (double) DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
          numArray2[index] = numArray1[index] / num;
        }
      }
      else if (this.ActualXAxis is LogarithmicAxis)
      {
        numArray1[index] = Math.Log(numArray1[index], (this.ActualXAxis as LogarithmicAxis).LogarithmicBase);
        numArray2[index] = numArray1[index];
      }
      else
        numArray2[index] = numArray1[index];
    }
    for (int index = 0; index < numArray1.Length - 1; ++index)
    {
      this.startControlPoints.Add(new ChartPoint(xValues[index] + numArray1[index] / 3.0, yValues[index] + numArray2[index] / 3.0));
      this.endControlPoints.Add(new ChartPoint(xValues[index + 1] - numArray1[index + 1] / 3.0, yValues[index + 1] - numArray2[index + 1] / 3.0));
    }
  }

  protected void GetMonotonicSpline(List<double> xValues, IList<double> yValues)
  {
    this.startControlPoints = new List<ChartPoint>(this.DataCount);
    this.endControlPoints = new List<ChartPoint>(this.DataCount);
    int num1 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    double[] numArray1 = new double[num1 - 1];
    double[] numArray2 = new double[num1 - 1];
    List<double> doubleList = new List<double>();
    for (int index = 0; index < num1 - 1; ++index)
    {
      if (!double.IsNaN(yValues[index + 1]) && !double.IsNaN(yValues[index]) && !double.IsNaN(xValues[index + 1]) && !double.IsNaN(xValues[index]))
      {
        numArray1[index] = xValues[index + 1] - xValues[index];
        numArray2[index] = (yValues[index + 1] - yValues[index]) / numArray1[index];
        if (double.IsInfinity(numArray2[index]))
          numArray2[index] = 0.0;
      }
    }
    if (numArray2.Length == 0)
      return;
    doubleList.Add(double.IsNaN(numArray2[0]) ? 0.0 : numArray2[0]);
    for (int index = 0; index < numArray1.Length - 1; ++index)
    {
      if (numArray2.Length > index + 1)
      {
        double num2 = numArray2[index];
        double num3 = numArray2[index + 1];
        if (num2 * num3 <= 0.0)
          doubleList.Add(0.0);
        else if (numArray1[index] == 0.0)
        {
          doubleList.Add(0.0);
        }
        else
        {
          double num4 = numArray1[index];
          double num5 = numArray1[index + 1];
          double num6 = num4 + num5;
          doubleList.Add(3.0 * num6 / ((num6 + num5) / num2 + (num6 + num4) / num3));
        }
      }
    }
    doubleList.Add(double.IsNaN(numArray2[numArray2.Length - 1]) ? 0.0 : numArray2[numArray2.Length - 1]);
    for (int index = 0; index < doubleList.Count; ++index)
    {
      if (index + 1 < doubleList.Count && numArray1.Length > 0)
      {
        double num7 = numArray1[index] / 3.0;
        this.startControlPoints.Add(new ChartPoint(xValues[index] + num7, yValues[index] + doubleList[index] * num7));
        this.endControlPoints.Add(new ChartPoint(xValues[index + 1] - num7, yValues[index + 1] - doubleList[index + 1] * num7));
      }
    }
  }

  protected void NaturalSpline(List<double> xValues, IList<double> yValues, out double[] ys2)
  {
    int length = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    ys2 = new double[length];
    double num1 = 6.0;
    double[] numArray = new double[length];
    if (this.SplineType == SplineType.Natural)
    {
      ys2[0] = numArray[0] = 0.0;
      ys2[length - 1] = 0.0;
    }
    else if (xValues.Count > 1)
    {
      double num2 = (xValues[1] - xValues[0]) / (yValues[1] - yValues[0]);
      double num3 = (xValues[length - 1] - xValues[length - 2]) / (yValues[length - 1] - yValues[length - 2]);
      numArray[0] = 0.5;
      ys2[0] = 3.0 * (yValues[1] - yValues[0]) / (xValues[1] - xValues[0]) - 3.0 * num2;
      ys2[length - 1] = 3.0 * num3 - 3.0 * (yValues[length - 1] - yValues[length - 2]) / (xValues[length - 1] - xValues[length - 2]);
      if (double.IsInfinity(ys2[0]) || double.IsNaN(ys2[0]))
        ys2[0] = 0.0;
      if (double.IsInfinity(ys2[length - 1]) || double.IsNaN(ys2[length - 1]))
        ys2[length - 1] = 0.0;
    }
    for (int index = 1; index < length - 1; ++index)
    {
      if (yValues.Count > index + 1 && !double.IsNaN(yValues[index + 1]) && !double.IsNaN(yValues[index - 1]) && !double.IsNaN(yValues[index]))
      {
        double num4 = xValues[index] - xValues[index - 1];
        double num5 = xValues[index + 1] - xValues[index - 1];
        double num6 = xValues[index + 1] - xValues[index];
        double num7 = yValues[index + 1] - yValues[index];
        double num8 = yValues[index] - yValues[index - 1];
        if (xValues[index] == xValues[index - 1] || xValues[index] == xValues[index + 1])
        {
          ys2[index] = 0.0;
          numArray[index] = 0.0;
        }
        else
        {
          double num9 = 1.0 / (num4 * ys2[index - 1] + 2.0 * num5);
          ys2[index] = -num9 * num6;
          numArray[index] = num9 * (num1 * (num7 / num6 - num8 / num4) - num4 * numArray[index - 1]);
        }
      }
    }
    for (int index = length - 2; index >= 0; --index)
      ys2[index] = ys2[index] * ys2[index + 1] + numArray[index];
  }

  protected void GetBezierControlPoints(
    ChartPoint point1,
    ChartPoint point2,
    double ys1,
    double ys2,
    out ChartPoint controlPoint1,
    out ChartPoint controlPoint2)
  {
    double num1 = point2.X - point1.X;
    double num2 = num1 * num1;
    double num3 = 2.0 * point1.X + point2.X;
    double num4 = point1.X + 2.0 * point2.X;
    double num5 = 2.0 * point1.Y + point2.Y;
    double num6 = point1.Y + 2.0 * point2.Y;
    double y1 = 1.0 / 3.0 * (num5 - 1.0 / 3.0 * num2 * (ys1 + 0.5 * ys2));
    double y2 = 1.0 / 3.0 * (num6 - 1.0 / 3.0 * num2 * (0.5 * ys1 + ys2));
    controlPoint1 = new ChartPoint(num3 * (1.0 / 3.0), y1);
    controlPoint2 = new ChartPoint(num4 * (1.0 / 3.0), y2);
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SplineSeries splineSeries = d as SplineSeries;
    if (splineSeries.Area == null)
      return;
    splineSeries.Segments.Clear();
    splineSeries.Area.ScheduleUpdate();
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
    (d as SplineSeries).UpdateArea();
  }

  private static void OnSplineTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ChartSeriesBase) d).UpdateArea();
  }

  private void UpdatePreviewSegmentAndSeries(Point mousePos)
  {
    try
    {
      Brush interior = (this.Segments[0] as SplineSegment).Interior;
      List<double> xvalues = this.GetXValues();
      double[] ys2 = (double[]) null;
      if (this.segments == null)
      {
        this.segments = new List<SplineSegment>();
        this.previewYValues = new List<double>();
        if (this.SplineType == SplineType.Monotonic)
          this.GetMonotonicSpline(xvalues, this.YValues);
        else if (this.SplineType == SplineType.Cardinal)
          this.GetCardinalSpline(xvalues, this.YValues);
        else
          this.NaturalSpline(xvalues, this.YValues, out ys2);
        IChartTransformer transformer = this.ActualArea != null ? this.CreateTransformer(this.GetAvailableSize(), true) : (IChartTransformer) null;
        if (transformer == null)
          return;
        for (int index1 = 0; index1 < this.DataCount; ++index1)
        {
          int index2 = index1 + 1;
          ChartPoint point1 = new ChartPoint(xvalues[index1], this.YValues[index1]);
          this.previewYValues.Add(this.YValues[index1]);
          if (index2 < this.DataCount)
          {
            ChartPoint chartPoint = new ChartPoint(xvalues[index2], this.YValues[index2]);
            ChartPoint controlPoint1;
            ChartPoint controlPoint2;
            if (this.SplineType == SplineType.Monotonic)
            {
              controlPoint1 = this.startControlPoints[index2 - 1];
              controlPoint2 = this.endControlPoints[index2 - 1];
            }
            else if (this.SplineType == SplineType.Cardinal)
            {
              controlPoint1 = this.startControlPoints[index2 - 1];
              controlPoint2 = this.endControlPoints[index2 - 1];
            }
            else
              this.GetBezierControlPoints(point1, chartPoint, ys2[index1], ys2[index2], out controlPoint1, out controlPoint2);
            SplineSegment splineSegment = new SplineSegment(point1, controlPoint1, controlPoint2, chartPoint, this);
            splineSegment.SetData(point1, controlPoint1, controlPoint2, chartPoint);
            UIElement visual = splineSegment.CreateVisual(Size.Empty);
            splineSegment.Update(transformer);
            if (this.CustomTemplate == null)
            {
              Path path = visual as Path;
              path.Stroke = ((Shape) this.Segments[0].GetRenderedVisual()).Stroke;
              path.StrokeThickness = this.StrokeThickness;
              visual.Opacity = ((Shape) this.Segments[0].GetRenderedVisual()).StrokeThickness;
              visual.Opacity = 0.5;
              this.segments.Add(splineSegment);
              this.SeriesPanel.Children.Add(visual);
            }
            else
            {
              this.segments.Add(splineSegment);
              this.SeriesPanel.Children.Add(visual);
            }
          }
        }
      }
      else
      {
        if (this.isSeriesCaptured)
        {
          double num = this.Area.PointToValue(this.ActualYAxis, new Point(mousePos.X, mousePos.Y)) - this.Area.PointToValue(this.ActualYAxis, this.IsActualTransposed ? new Point(this.offsetPosition, mousePos.Y) : new Point(mousePos.X, this.offsetPosition));
          for (int index = 0; index < this.previewYValues.Count; ++index)
            this.previewYValues[index] += num;
          this.offsetPosition = this.IsActualTransposed ? mousePos.X : mousePos.Y;
          if (this.IsActualTransposed)
            this.DraggedValue = this.Area.PointToValue(this.ActualXAxis, new Point(0.0, this.initialPosition)) - this.Area.PointToValue(this.ActualXAxis, new Point(mousePos.Y, mousePos.X));
          else
            this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos) - this.Area.PointToValue(this.ActualYAxis, new Point(0.0, this.initialPosition));
          XySeriesDragEventArgs seriesDragEventArgs = new XySeriesDragEventArgs();
          seriesDragEventArgs.Delta = this.DraggedValue;
          seriesDragEventArgs.BaseXValue = (object) this.SegmentIndex;
          XySeriesDragEventArgs args = seriesDragEventArgs;
          this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
          if (args.Cancel)
          {
            this.ResetDraggingElements("Cancel", true);
            return;
          }
          if (!this.IsActualTransposed)
            this.UpdateSeriesDragValueToolTip(mousePos, interior, this.DraggedValue, this.YValues[0], this.Area.ValueToPoint(this.ActualXAxis, 0.0));
          else
            this.UpdateSeriesDragValueToolTip(mousePos, interior, this.DraggedValue, 0.0, this.Area.ValueToPoint(this.ActualYAxis, this.YValues[0]));
        }
        else
        {
          double num = this.Segments.Count == this.SegmentIndex ? (this.DraggingSegment as SplineSegment).X2 : (this.DraggingSegment as SplineSegment).X1;
          this.previewYValues[this.SegmentIndex] = this.Area.PointToValue(this.ActualYAxis, mousePos);
          this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos);
          XySegmentDragEventArgs segmentDragEventArgs = new XySegmentDragEventArgs();
          segmentDragEventArgs.BaseYValue = this.YValues[this.SegmentIndex];
          segmentDragEventArgs.NewYValue = this.DraggedValue;
          segmentDragEventArgs.Segment = this.DraggingSegment;
          segmentDragEventArgs.Delta = this.GetActualDelta();
          XySegmentDragEventArgs args = segmentDragEventArgs;
          this.prevDraggedValue = this.DraggedValue;
          this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
          if (args.Cancel)
            return;
          if (this.IsActualTransposed)
            this.UpdateSegmentDragValueToolTip(new Point(mousePos.X, this.Area.ValueToPoint(this.ActualXAxis, num)), this.DraggingSegment, 0.0, this.DraggedValue, 0.0, 0.0);
          else
            this.UpdateSegmentDragValueToolTip(new Point(this.Area.ValueToPoint(this.ActualXAxis, num), mousePos.Y), this.DraggingSegment, 0.0, this.DraggedValue, 0.0, 0.0);
        }
        if (this.SplineType == SplineType.Monotonic)
          this.GetMonotonicSpline(xvalues, (IList<double>) this.previewYValues);
        else if (this.SplineType == SplineType.Cardinal)
          this.GetCardinalSpline(xvalues, this.YValues);
        else
          this.NaturalSpline(xvalues, (IList<double>) this.previewYValues, out ys2);
        IChartTransformer transformer = this.ActualArea != null ? this.CreateTransformer(this.GetAvailableSize(), true) : (IChartTransformer) null;
        if (transformer == null)
          return;
        for (int index3 = 0; index3 < this.DataCount; ++index3)
        {
          int index4 = index3 + 1;
          ChartPoint point1 = new ChartPoint(xvalues[index3], this.previewYValues[index3]);
          if (index4 < this.DataCount)
          {
            ChartPoint chartPoint = new ChartPoint(xvalues[index4], this.previewYValues[index4]);
            ChartPoint controlPoint1;
            ChartPoint controlPoint2;
            if (this.SplineType == SplineType.Monotonic)
            {
              controlPoint1 = this.startControlPoints[index4 - 1];
              controlPoint2 = this.endControlPoints[index4 - 1];
            }
            else if (this.SplineType == SplineType.Cardinal)
            {
              controlPoint1 = this.startControlPoints[index4 - 1];
              controlPoint2 = this.endControlPoints[index4 - 1];
            }
            else
              this.GetBezierControlPoints(point1, chartPoint, ys2[index3], ys2[index4], out controlPoint1, out controlPoint2);
            this.segments[index3].SetData(point1, controlPoint1, controlPoint2, chartPoint);
            this.segments[index3].Update(transformer);
          }
        }
      }
      this.isDragged = true;
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }

  private void UpdateDraggedSource()
  {
    try
    {
      if (this.isDragged)
      {
        double yvalue = this.YValues[this.SegmentIndex];
        XyPreviewEndEventArgs args = new XyPreviewEndEventArgs()
        {
          BaseYValue = yvalue,
          NewYValue = this.DraggedValue
        };
        this.RaisePreviewEnd(args);
        if (args.Cancel)
        {
          this.ResetDraggingElements("", false);
          return;
        }
        if (this.isSeriesCaptured)
        {
          for (int index = 0; index < this.YValues.Count; ++index)
            this.YValues[index] = this.GetSnapToPoint(this.YValues[index] + this.DraggedValue);
          if (this.UpdateSource)
            this.UpdateUnderLayingModel(this.YBindingPath, this.YValues);
        }
        else
        {
          this.DraggedValue = this.GetSnapToPoint(this.DraggedValue);
          this.ActualSeriesYValues[0][this.SegmentIndex] = this.DraggedValue;
          if (this.UpdateSource && !this.IsSortData)
            this.UpdateUnderLayingModel(this.YBindingPath, this.SegmentIndex, (object) this.DraggedValue);
        }
        this.UpdateArea();
        this.RaiseDragEnd(new ChartDragEndEventArgs()
        {
          BaseYValue = yvalue,
          NewYValue = this.DraggedValue
        });
      }
      this.ResetDraggingElements("", false);
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }
}
