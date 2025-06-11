// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StepLineSeries
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

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StepLineSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (StepLineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StepLineSeries.OnCustomTemplateChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (StepLineSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(StepLineSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (StepLineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StepLineSeries.OnSegmentSelectionBrush)));
  private Point hitPoint = new Point();
  private RectangleGeometry geometry;
  private Storyboard sb;

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(StepLineSeries.CustomTemplateProperty);
    set => this.SetValue(StepLineSeries.CustomTemplateProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(StepLineSeries.SelectedIndexProperty);
    set => this.SetValue(StepLineSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(StepLineSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(StepLineSeries.SegmentSelectionBrushProperty, (object) value);
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
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index1 = 0; index1 < xValues.Count; ++index1)
      {
        int index2 = index1 + 1;
        if (this.AdornmentsInfo != null)
        {
          this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index1], this.GroupedSeriesYValues[0][index1], xValues[index1], this.GroupedSeriesYValues[0][index1]));
          this.Adornments[index1].Item = this.ActualData[index1];
        }
        ChartPoint chartPoint1;
        ChartPoint chartPoint2;
        ChartPoint chartPoint3;
        if (index2 < xValues.Count)
        {
          chartPoint1 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
          chartPoint2 = new ChartPoint(xValues[index2], this.GroupedSeriesYValues[0][index1]);
          chartPoint3 = new ChartPoint(xValues[index2], this.GroupedSeriesYValues[0][index2]);
        }
        else
        {
          chartPoint1 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
          chartPoint2 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
          chartPoint3 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
        }
        if (this.CreateSegment() is StepLineSegment segment)
        {
          segment.Item = this.ActualData[index1];
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(new List<ChartPoint>()
          {
            chartPoint1,
            chartPoint3,
            chartPoint2
          });
          this.Segments.Add((ChartSegment) segment);
        }
      }
    }
    else
    {
      this.ClearUnUsedStepLineSegment(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        int index4 = index3 + 1;
        if (this.AdornmentsInfo != null)
        {
          if (index3 < this.Adornments.Count)
          {
            this.Adornments[index3].SetData(xValues[index3], this.YValues[index3], xValues[index3], this.YValues[index3]);
            this.Adornments[index3].Item = this.ActualData[index3];
          }
          else
          {
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index3], this.YValues[index3], xValues[index3], this.YValues[index3]));
            this.Adornments[index3].Item = this.ActualData[index3];
          }
        }
        ChartPoint chartPoint4;
        ChartPoint chartPoint5;
        ChartPoint chartPoint6;
        if (index4 < this.DataCount)
        {
          chartPoint4 = new ChartPoint(xValues[index3], this.YValues[index3]);
          chartPoint5 = new ChartPoint(xValues[index4], this.YValues[index3]);
          chartPoint6 = new ChartPoint(xValues[index4], this.YValues[index4]);
        }
        else
        {
          chartPoint4 = new ChartPoint(xValues[index3], this.YValues[index3]);
          chartPoint5 = new ChartPoint(xValues[index3], this.YValues[index3]);
          chartPoint6 = new ChartPoint(xValues[index3], this.YValues[index3]);
        }
        if (index3 < this.Segments.Count)
        {
          this.Segments[index3].SetData(new List<ChartPoint>()
          {
            chartPoint4,
            chartPoint6,
            chartPoint5
          });
          this.Segments[index3].Item = this.ActualData[index3];
          (this.Segments[index3] as StepLineSegment).YData = this.YValues[index3];
          if (this.SegmentColorPath != null && !this.Segments[index3].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index3].IsSelectedSegment)
            this.Segments[index3].Interior = this.Interior != null ? this.Interior : this.ColorValues[index3];
        }
        else if (this.CreateSegment() is StepLineSegment segment)
        {
          segment.Item = this.ActualData[index3];
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(new List<ChartPoint>()
          {
            chartPoint4,
            chartPoint6,
            chartPoint5
          });
          this.Segments.Add((ChartSegment) segment);
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
  }

  internal override bool GetAnimationIsActive() => this.geometry != null;

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
    Rect rect2 = this.IsActualTransposed ? new Rect(0.0, seriesClipRect.Y - seriesClipRect.Top, seriesClipRect.Width, seriesClipRect.Height) : new Rect(0.0, seriesClipRect.Y, seriesClipRect.Width, seriesClipRect.Height);
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
        if (this.ActualArea != null && newItem < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[newItem],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            NewPointInfo = this.Segments[newItem].Item,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = previousSelectedIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
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
            NewPointInfo = this.Segments[newItem - 1].Item,
            PreviousSelectedSeries = previousSelectedIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            PreviousSelectedSegment = (ChartSegment) null,
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
        (this.ActualArea as SfChart).OnSelectionChanged(new ChartSelectionChangedEventArgs()
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
        });
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
            StepLineSegment stepLineSegment = new StepLineSegment();
            stepLineSegment.X1Value = adornment.XData;
            stepLineSegment.Y1Value = adornment.YData;
            stepLineSegment.X1Data = adornment.XData;
            stepLineSegment.Y1Data = adornment.YData;
            stepLineSegment.X1 = adornment.XData;
            stepLineSegment.Y1 = adornment.YData;
            stepLineSegment.XData = adornment.XData;
            stepLineSegment.YData = adornment.YData;
            stepLineSegment.Item = adornment.Item;
            stepLineSegment.Series = adornment.Series;
            stepLineSegment.Interior = adornment.Interior;
            stepLineSegment.IsAddedToVisualTree = adornment.IsAddedToVisualTree;
            stepLineSegment.IsEmptySegmentInterior = adornment.IsEmptySegmentInterior;
            stepLineSegment.IsSegmentVisible = adornment.IsSegmentVisible;
            stepLineSegment.IsSelectedSegment = adornment.IsSelectedSegment;
            stepLineSegment.PolygonPoints = adornment.PolygonPoints;
            stepLineSegment.Stroke = adornment.Stroke;
            stepLineSegment.StrokeDashArray = adornment.StrokeDashArray;
            stepLineSegment.StrokeThickness = adornment.StrokeThickness;
            stepLineSegment.XRange = adornment.XRange;
            stepLineSegment.YRange = adornment.YRange;
            obj = (object) stepLineSegment;
          }
        }
      }
    }
    if (!(obj is ChartSegment chartSegment) || chartSegment is TrendlineSegment || chartSegment.Item is Trendline)
      return;
    if (this.TooltipTemplate != null && chartSegment.Item == this.ActualData[this.Segments.Count - 1] && this.Segments.Contains(chartSegment) && this.Adornments != null && this.Adornments.Count == 0)
    {
      StepLineSegment stepLineSegment1 = chartSegment as StepLineSegment;
      StepLineSegment stepLineSegment2 = new StepLineSegment();
      stepLineSegment2.X1Value = stepLineSegment1.X1Value;
      stepLineSegment2.Y1Value = stepLineSegment1.Y1Value;
      stepLineSegment2.Y2Value = stepLineSegment1.Y2Value;
      stepLineSegment2.X1Data = stepLineSegment1.X1Data;
      stepLineSegment2.Y1Data = stepLineSegment1.YData;
      stepLineSegment2.X1 = stepLineSegment1.X1;
      stepLineSegment2.Y1 = stepLineSegment1.Y1;
      stepLineSegment2.XData = stepLineSegment1.XData;
      stepLineSegment2.YData = stepLineSegment1.YData;
      stepLineSegment2.Item = stepLineSegment1.Item;
      stepLineSegment2.Series = stepLineSegment1.Series;
      stepLineSegment2.Interior = stepLineSegment1.Interior;
      stepLineSegment2.IsAddedToVisualTree = stepLineSegment1.IsAddedToVisualTree;
      stepLineSegment2.IsEmptySegmentInterior = stepLineSegment1.IsEmptySegmentInterior;
      stepLineSegment2.IsSegmentVisible = stepLineSegment1.IsSegmentVisible;
      stepLineSegment2.IsSelectedSegment = stepLineSegment1.IsSelectedSegment;
      stepLineSegment2.PolygonPoints = stepLineSegment1.PolygonPoints;
      stepLineSegment2.Stroke = stepLineSegment1.Stroke;
      stepLineSegment2.StrokeDashArray = stepLineSegment1.StrokeDashArray;
      stepLineSegment2.StrokeThickness = stepLineSegment1.StrokeThickness;
      stepLineSegment2.X2 = stepLineSegment1.X2;
      stepLineSegment2.X2Value = stepLineSegment1.X2Value;
      stepLineSegment2.XRange = stepLineSegment1.XRange;
      stepLineSegment2.Y2 = stepLineSegment1.Y2;
      stepLineSegment2.YRange = stepLineSegment1.YRange;
      stepLineSegment2.Points = stepLineSegment1.Points;
      stepLineSegment2.X3 = stepLineSegment1.X3;
      stepLineSegment2.Y3 = stepLineSegment1.Y3;
      obj = (object) stepLineSegment2;
    }
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    if (chartTooltip1 == null)
      return;
    StepLineSegment lineSegment = obj as StepLineSegment;
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
    StepLineSegment toolTipTag = this.ToolTipTag as StepLineSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  private void SetTooltipSegmentItem(StepLineSegment lineSegment)
  {
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    Point point = new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top);
    this.FindNearestChartPoint(point, out x, out y, out stackedYValue);
    if (double.IsNaN(x))
      return;
    if (lineSegment != null)
      lineSegment.YData = y == lineSegment.Y1Value ? lineSegment.Y1Value : lineSegment.Y1Data;
    lineSegment.XData = x;
    int index1 = this.GetXValues().IndexOf(x);
    if (!this.IsIndexed)
    {
      IList<double> xvalues = (IList<double>) this.GetXValues();
      int index2 = index1;
      double num1 = this.ActualSeriesYValues[0][index2];
      for (; xvalues.Count > index2 && xvalues[index2] == x; ++index2)
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
    lineSegment.Item = this.ActualData[index1];
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
            NewPointInfo = this.Segments[newIndex].Item,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            IsSelected = true
          };
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
            NewPointInfo = this.Segments[newIndex - 1].Item,
            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            PreviousSelectedSegment = (ChartSegment) null,
            IsSelected = true
          };
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
        this.PreviousSelectedIndex = newIndex;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new StepLineSegment();

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new StepLineSeries()
    {
      CustomTemplate = this.CustomTemplate,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    StepLineSeries stepLineSeries = d as StepLineSeries;
    if (stepLineSeries.Area == null)
      return;
    stepLineSeries.Segments.Clear();
    stepLineSeries.Area.ScheduleUpdate();
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
    (d as StepLineSeries).UpdateArea();
  }

  private void CreateSegment(double[] values)
  {
    if (!(this.CreateSegment() is StepLineSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.SetData(values);
    this.Segments.Add((ChartSegment) segment);
  }

  private void ClearUnUsedStepLineSegment(int startIndex)
  {
    List<ChartSegment> chartSegmentList = new List<ChartSegment>();
    foreach (ChartSegment chartSegment in this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (item => item is EmptyPointSegment)))
      chartSegmentList.Add(chartSegment);
    foreach (ChartSegment chartSegment in chartSegmentList)
      this.Segments.Remove(chartSegment);
    if (this.Segments.Count < startIndex || this.Segments.Count == 0)
      return;
    int count = this.Segments.Count;
    for (int index = startIndex; index <= count; ++index)
    {
      if (index == count)
        this.Segments.RemoveAt(startIndex - 1);
      else
        this.Segments.RemoveAt(startIndex);
    }
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
