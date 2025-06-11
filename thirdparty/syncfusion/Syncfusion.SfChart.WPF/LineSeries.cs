// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineSeries
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
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LineSeries : XySeriesDraggingBase, ISegmentSelectable
{
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (LineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(LineSeries.OnCustomTemplateChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (LineSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(LineSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (LineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(LineSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (LineSeries), new PropertyMetadata((PropertyChangedCallback) null));
  private Storyboard sb;
  private Line previewCurrLine;
  private Line previewPreLine;
  private double offsetPosition;
  private double initialPosition;
  private PointCollection pointCollection;
  private bool isReversed;
  private bool isDragged;
  private bool hasTemplate;
  private Point hitPoint = new Point();
  private RectangleGeometry geometry;

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(LineSeries.CustomTemplateProperty);
    set => this.SetValue(LineSeries.CustomTemplateProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(LineSeries.SelectedIndexProperty);
    set => this.SetValue(LineSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(LineSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(LineSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(LineSeries.StrokeDashArrayProperty);
    set => this.SetValue(LineSeries.StrokeDashArrayProperty, (object) value);
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
        if (this.GroupedSeriesYValues != null)
        {
          if (index2 < xValues.Count)
            this.CreateSegment(new double[4]
            {
              xValues[index1],
              this.GroupedSeriesYValues[0][index1],
              xValues[index2],
              this.GroupedSeriesYValues[0][index2]
            }, this.ActualData[index1]);
          if (this.AdornmentsInfo != null)
          {
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index1], this.GroupedSeriesYValues[0][index1], xValues[index1], this.GroupedSeriesYValues[0][index1]));
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
            this.Segments[index3].SetData(xValues[index3], this.YValues[index3], xValues[index4], this.YValues[index4]);
            (this.Segments[index3] as LineSegment).Item = this.ActualData[index3];
            (this.Segments[index3] as LineSegment).YData = this.YValues[index3];
            if (this.SegmentColorPath != null && !this.Segments[index3].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index3].IsSelectedSegment)
              this.Segments[index3].Interior = this.Interior != null ? this.Interior : this.ColorValues[index3];
          }
          else
            this.Segments.RemoveAt(index3);
        }
        else if (index4 < this.DataCount)
          this.CreateSegment(new double[4]
          {
            xValues[index3],
            this.YValues[index3],
            xValues[index4],
            this.YValues[index4]
          }, this.ActualData[index3]);
        if (this.AdornmentsInfo != null)
        {
          if (index3 < this.Adornments.Count)
            this.Adornments[index3].SetData(xValues[index3], this.YValues[index3], xValues[index3], this.YValues[index3]);
          else
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index3], this.YValues[index3], xValues[index3], this.YValues[index3]));
          this.Adornments[index3].Item = this.ActualData[index3];
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
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
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
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
            LineSegment lineSegment = new LineSegment();
            lineSegment.X1Value = adornment.XData;
            lineSegment.Y1Value = adornment.YData;
            lineSegment.X1Data = adornment.XData;
            lineSegment.Y1Data = adornment.YData;
            lineSegment.X1 = adornment.XData;
            lineSegment.Y1 = adornment.YData;
            lineSegment.XData = adornment.XData;
            lineSegment.YData = adornment.YData;
            lineSegment.Item = adornment.Item;
            lineSegment.Series = adornment.Series;
            lineSegment.Interior = adornment.Interior;
            lineSegment.IsAddedToVisualTree = adornment.IsAddedToVisualTree;
            lineSegment.IsEmptySegmentInterior = adornment.IsEmptySegmentInterior;
            lineSegment.IsSegmentVisible = adornment.IsSegmentVisible;
            lineSegment.IsSelectedSegment = adornment.IsSelectedSegment;
            lineSegment.PolygonPoints = adornment.PolygonPoints;
            lineSegment.Stroke = adornment.Stroke;
            lineSegment.StrokeDashArray = adornment.StrokeDashArray;
            lineSegment.StrokeThickness = adornment.StrokeThickness;
            lineSegment.XRange = adornment.XRange;
            lineSegment.YRange = adornment.YRange;
            obj = (object) lineSegment;
          }
        }
      }
    }
    if (!(obj is ChartSegment chartSegment) || chartSegment is TrendlineSegment || chartSegment.Item is Trendline)
      return;
    if (this.TooltipTemplate != null && chartSegment.Item == this.ActualData[this.Segments.Count] && this.Segments.Contains(chartSegment) && this.Adornments != null && this.Adornments.Count == 0)
    {
      LineSegment lineSegment1 = chartSegment as LineSegment;
      LineSegment lineSegment2 = new LineSegment();
      lineSegment2.X1Value = lineSegment1.X1Value;
      lineSegment2.Y1Value = lineSegment1.Y1Value;
      lineSegment2.Y2Value = lineSegment1.Y2Value;
      lineSegment2.X1Data = lineSegment1.X1Data;
      lineSegment2.Y1Data = lineSegment1.Y1Data;
      lineSegment2.X1 = lineSegment1.X1;
      lineSegment2.Y1 = lineSegment1.Y1;
      lineSegment2.XData = lineSegment1.XData;
      lineSegment2.YData = lineSegment1.YData;
      lineSegment2.Item = lineSegment1.Item;
      lineSegment2.Series = lineSegment1.Series;
      lineSegment2.Interior = lineSegment1.Interior;
      lineSegment2.IsAddedToVisualTree = lineSegment1.IsAddedToVisualTree;
      lineSegment2.IsEmptySegmentInterior = lineSegment1.IsEmptySegmentInterior;
      lineSegment2.IsSegmentVisible = lineSegment1.IsSegmentVisible;
      lineSegment2.IsSelectedSegment = lineSegment1.IsSelectedSegment;
      lineSegment2.PolygonPoints = lineSegment1.PolygonPoints;
      lineSegment2.Stroke = lineSegment1.Stroke;
      lineSegment2.StrokeDashArray = lineSegment1.StrokeDashArray;
      lineSegment2.StrokeThickness = lineSegment1.StrokeThickness;
      lineSegment2.X2 = lineSegment1.X2;
      lineSegment2.X2Value = lineSegment1.X2Value;
      lineSegment2.XRange = lineSegment1.XRange;
      lineSegment2.Y2 = lineSegment1.Y2;
      lineSegment2.YRange = lineSegment1.YRange;
      obj = (object) lineSegment2;
    }
    LineSegment lineSegment3 = obj as LineSegment;
    this.SetTooltipSegmentItem(lineSegment3);
    this.SetTooltipDuration();
    this.ToolTipTag = (object) lineSegment3;
    ChartTooltip chartTooltip1 = this.Area.Tooltip;
    if (chartTooltip1 == null)
      return;
    chartTooltip1.PolygonPath = " ";
    chartTooltip1.DataContext = (object) lineSegment3;
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      if (ChartTooltip.GetActualInitialShowDelay(this.ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay((DependencyObject) this)) == 0)
      {
        this.HastoolTip = true;
        adorningCanvas.Children.Add((UIElement) chartTooltip1);
      }
      chartTooltip1.ContentTemplate = this.GetTooltipTemplate();
      this.AddTooltip();
      if (ChartTooltip.GetActualEnableAnimation(this.ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation((UIElement) this)))
        this.SetDoubleAnimation(chartTooltip1);
      Canvas.SetLeft((UIElement) chartTooltip1, chartTooltip1.LeftOffset);
      Canvas.SetTop((UIElement) chartTooltip1, chartTooltip1.TopOffset);
      this._stopwatch.Start();
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

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    LineSegment toolTipTag = this.ToolTipTag as LineSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  private void SetTooltipSegmentItem(LineSegment lineSegment)
  {
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    Point point = new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top);
    this.FindNearestChartPoint(point, out x, out y, out stackedYValue);
    if (double.IsNaN(x))
      return;
    if (lineSegment != null)
      lineSegment.YData = y == lineSegment.Y1Value ? lineSegment.Y1Value : lineSegment.Y2Value;
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
    if (this.ToolTipTag == null)
      return;
    int num = this.ActualData.IndexOf((this.ToolTipTag as LineSegment).Item);
    if (index1 == num)
      return;
    this.RemoveTooltip();
    this.Timer.Stop();
    this.ActualArea.Tooltip = new ChartTooltip();
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
      doubleAnimation1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      doubleAnimation1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      DoubleAnimation element1 = doubleAnimation1;
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(path1, new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimation doubleAnimation2 = new DoubleAnimation();
      doubleAnimation2.From = new double?(0.6);
      doubleAnimation2.To = new double?(1.0);
      doubleAnimation2.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
      doubleAnimation2.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num2 * num1));
      DoubleAnimation element2 = doubleAnimation2;
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(path2, new object[0]));
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) labelPresenter);
      this.sb.Children.Add((Timeline) element2);
      ++num2;
    }
    this.sb.Begin();
  }

  internal override void ActivateDragging(Point mousePos, object element)
  {
    try
    {
      if (this.previewCurrLine != null || this.PreviewSeries != null || this.hasTemplate)
        return;
      FrameworkElement element1 = element as FrameworkElement;
      this.DraggingSegment = (ChartSegment) null;
      if (element1 != null)
      {
        if (element1.Tag is LineSegment)
          this.DraggingSegment = (ChartSegment) (element1.Tag as LineSegment);
        else if (element1.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is LineSegment)
          this.DraggingSegment = (ChartSegment) (templatedParent.Content as LineSegment);
      }
      this.isReversed = false;
      if (element1 != null && this.DraggingSegment == null && !(element1.DataContext is ChartAdornmentContainer) && !(element1.DataContext is ChartAdornment))
        return;
      base.ActivateDragging(mousePos, (object) element1);
      if (this.SegmentIndex < 0)
        return;
      if (this.EnableSeriesDragging && this.CustomTemplate == null)
      {
        Line renderedVisual1 = this.Segments[0].GetRenderedVisual() as Line;
        if (this.pointCollection == null)
        {
          Polyline polyline = new Polyline();
          polyline.Opacity = 0.6;
          polyline.Stroke = renderedVisual1.Stroke;
          polyline.StrokeThickness = renderedVisual1.StrokeThickness;
          this.PreviewSeries = (UIElement) polyline;
          this.SeriesPanel.Children.Add(this.PreviewSeries);
          this.pointCollection = new PointCollection();
          (this.PreviewSeries as Polyline).Points = this.pointCollection;
        }
        this.pointCollection.Clear();
        this.pointCollection.Add(new Point(renderedVisual1.X1, renderedVisual1.Y1));
        foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
        {
          Line renderedVisual2 = segment.GetRenderedVisual() as Line;
          this.pointCollection.Add(new Point(renderedVisual2.X2, renderedVisual2.Y2));
        }
        this.offsetPosition = this.initialPosition = this.IsActualTransposed ? mousePos.X : mousePos.Y;
      }
      else if (this.EnableSeriesDragging && (this.DraggingSegment != null || element1.DataContext is ChartAdornmentInfo || element1.DataContext is ChartAdornmentContainer || element1.DataContext is ChartAdornment))
      {
        this.hasTemplate = true;
        this.PreviewSeries = (UIElement) new Polyline();
        this.offsetPosition = this.initialPosition = this.IsActualTransposed ? mousePos.X : mousePos.Y;
      }
      else
      {
        if (this.DraggingSegment == null && !(element1.DataContext is ChartAdornmentContainer) && !(element1.DataContext is ChartAdornmentInfo) && !(element1.DataContext is ChartAdornment))
          return;
        double point = this.Area.ValueToPoint(this.ActualXAxis, (double) this.SegmentIndex);
        if (mousePos.X <= point)
          this.isReversed = true;
        if (this.SegmentIndex == this.Segments.Count)
          this.DraggingSegment = (ChartSegment) (this.Segments[this.SegmentIndex - 1] as LineSegment);
        else
          this.DraggingSegment = (ChartSegment) (this.Segments[this.SegmentIndex] as LineSegment);
        this.isReversed = false;
      }
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }

  internal override void UpdatePreivewSeriesDragging(Point mousePos)
  {
    if (this.CustomTemplate == null)
    {
      if (this.IsActualTransposed)
      {
        double num1 = this.Area.PointToValue(this.ActualXAxis, new Point(mousePos.Y, mousePos.X));
        this.DraggedValue = this.Area.PointToValue(this.ActualXAxis, new Point(0.0, this.initialPosition)) - num1;
        XySeriesDragEventArgs seriesDragEventArgs = new XySeriesDragEventArgs();
        seriesDragEventArgs.Delta = this.DraggedValue;
        seriesDragEventArgs.BaseXValue = (object) this.SegmentIndex;
        XySeriesDragEventArgs args = seriesDragEventArgs;
        this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
        if (args.Cancel)
        {
          this.ResetDraggingElements("Cancel", true);
        }
        else
        {
          double num2 = mousePos.X - this.offsetPosition;
          for (int index = 0; index < this.pointCollection.Count; ++index)
          {
            double x = this.pointCollection[index].X;
            double y = this.pointCollection[index].Y;
            this.pointCollection[index] = new Point(x + num2, y);
          }
          this.offsetPosition = mousePos.X;
          this.isDragged = true;
          this.UpdateSeriesDragValueToolTip(mousePos, this.Segments[0].Interior, this.DraggedValue, 0.0, this.Area.ValueToPoint(this.ActualYAxis, this.YValues[0]));
        }
      }
      else
      {
        this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos) - this.Area.PointToValue(this.ActualYAxis, new Point(0.0, this.initialPosition));
        XySeriesDragEventArgs seriesDragEventArgs = new XySeriesDragEventArgs();
        seriesDragEventArgs.Delta = this.DraggedValue;
        seriesDragEventArgs.BaseXValue = (object) this.SegmentIndex;
        XySeriesDragEventArgs args = seriesDragEventArgs;
        this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
        if (args.Cancel)
        {
          this.ResetDraggingElements("Cancel", true);
        }
        else
        {
          double num = mousePos.Y - this.offsetPosition;
          for (int index = 0; index < this.pointCollection.Count; ++index)
          {
            double x = this.pointCollection[index].X;
            double y = this.pointCollection[index].Y;
            this.pointCollection[index] = new Point(x, y + num);
          }
          this.offsetPosition = mousePos.Y;
          this.isDragged = true;
          this.UpdateSeriesDragValueToolTip(mousePos, this.Segments[0].Interior, this.DraggedValue, this.YValues[0], this.pointCollection[0].X);
        }
      }
    }
    else if (this.IsActualTransposed)
    {
      double num = this.Area.PointToValue(this.ActualXAxis, new Point(mousePos.Y, mousePos.X));
      this.DraggedValue = this.Area.PointToValue(this.ActualXAxis, new Point(0.0, this.initialPosition)) - num;
      XySeriesDragEventArgs seriesDragEventArgs = new XySeriesDragEventArgs();
      seriesDragEventArgs.Delta = this.DraggedValue;
      seriesDragEventArgs.BaseXValue = (object) this.SegmentIndex;
      XySeriesDragEventArgs args = seriesDragEventArgs;
      this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("Cancel", true);
      }
      else
      {
        this.offsetPosition = mousePos.X;
        this.isDragged = true;
        this.UpdateSeriesDragValueToolTip(mousePos, this.Segments[0].Interior, this.DraggedValue, 0.0, this.Area.ValueToPoint(this.ActualYAxis, this.YValues[0]));
      }
    }
    else
    {
      this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos) - this.Area.PointToValue(this.ActualYAxis, new Point(0.0, this.initialPosition));
      XySeriesDragEventArgs seriesDragEventArgs = new XySeriesDragEventArgs();
      seriesDragEventArgs.Delta = this.DraggedValue;
      seriesDragEventArgs.BaseXValue = (object) this.SegmentIndex;
      XySeriesDragEventArgs args = seriesDragEventArgs;
      this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("Cancel", true);
      }
      else
      {
        this.isDragged = true;
        this.offsetPosition = mousePos.Y;
        this.UpdateSeriesDragValueToolTip(mousePos, this.Segments[0].Interior, this.DraggedValue, this.YValues[0], (this.Segments[0] as LineSegment).X1);
      }
    }
  }

  internal override void UpdatePreviewSegmentDragging(Point mousePos)
  {
    try
    {
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
      double num1 = 0.0;
      if (this.CustomTemplate != null)
      {
        this.hasTemplate = true;
      }
      else
      {
        Line renderedVisual1 = this.DraggingSegment.GetRenderedVisual() as Line;
        if (this.previewCurrLine == null)
        {
          Line line1 = new Line();
          line1.Opacity = 0.6;
          line1.Stroke = renderedVisual1.Stroke;
          line1.StrokeThickness = renderedVisual1.StrokeThickness;
          this.previewCurrLine = line1;
          this.SeriesPanel.Children.Add((UIElement) this.previewCurrLine);
          Line line2 = new Line();
          line2.Opacity = 0.6;
          line2.Stroke = renderedVisual1.Stroke;
          line2.StrokeThickness = renderedVisual1.StrokeThickness;
          this.previewPreLine = line2;
          this.SeriesPanel.Children.Add((UIElement) this.previewPreLine);
        }
        if (!this.IsActualTransposed)
        {
          if (this.SegmentIndex == 0)
          {
            this.previewCurrLine.Y2 = renderedVisual1.Y2;
            this.previewCurrLine.Y1 = num1 = mousePos.Y;
          }
          else if (this.SegmentIndex == this.Segments.Count)
          {
            this.previewCurrLine.Y2 = num1 = mousePos.Y;
            this.previewCurrLine.Y1 = renderedVisual1.Y1;
          }
          else
          {
            Line renderedVisual2;
            if (!this.isReversed)
            {
              renderedVisual2 = this.Segments[this.SegmentIndex - 1].GetRenderedVisual() as Line;
              Line previewCurrLine = this.previewCurrLine;
              Line previewPreLine = this.previewPreLine;
              double y;
              num1 = y = mousePos.Y;
              double num2 = y;
              previewPreLine.Y2 = y;
              double num3 = num2;
              previewCurrLine.Y1 = num3;
              this.previewCurrLine.Y2 = renderedVisual1.Y2;
              this.previewPreLine.Y1 = renderedVisual2.Y1;
            }
            else
            {
              renderedVisual2 = this.Segments[this.SegmentIndex].GetRenderedVisual() as Line;
              this.previewCurrLine.Y1 = renderedVisual1.Y1;
              Line previewCurrLine = this.previewCurrLine;
              Line previewPreLine = this.previewPreLine;
              double y;
              num1 = y = mousePos.Y;
              double num4 = y;
              previewPreLine.Y1 = y;
              double num5 = num4;
              previewCurrLine.Y2 = num5;
              this.previewPreLine.Y2 = renderedVisual2.Y2;
            }
            this.previewPreLine.X2 = renderedVisual2.X2;
            this.previewPreLine.X1 = renderedVisual2.X1;
          }
          this.previewCurrLine.X1 = renderedVisual1.X1;
          this.previewCurrLine.X2 = renderedVisual1.X2;
        }
        else
        {
          if (this.SegmentIndex == 0)
          {
            this.previewCurrLine.X2 = renderedVisual1.X2;
            this.previewCurrLine.X1 = num1 = mousePos.X;
          }
          else if (this.SegmentIndex == this.Segments.Count)
          {
            this.previewCurrLine.X2 = num1 = mousePos.X;
            this.previewCurrLine.X1 = renderedVisual1.X1;
          }
          else
          {
            Line renderedVisual3;
            if (!this.isReversed)
            {
              renderedVisual3 = this.Segments[this.SegmentIndex - 1].GetRenderedVisual() as Line;
              Line previewCurrLine = this.previewCurrLine;
              Line previewPreLine = this.previewPreLine;
              double x;
              num1 = x = mousePos.X;
              double num6 = x;
              previewPreLine.X2 = x;
              double num7 = num6;
              previewCurrLine.X1 = num7;
              this.previewCurrLine.X2 = renderedVisual1.X2;
              this.previewPreLine.X1 = renderedVisual3.X1;
            }
            else
            {
              renderedVisual3 = this.Segments[this.SegmentIndex].GetRenderedVisual() as Line;
              this.previewCurrLine.X1 = renderedVisual1.X1;
              Line previewCurrLine = this.previewCurrLine;
              Line previewPreLine = this.previewPreLine;
              double x;
              num1 = x = mousePos.X;
              double num8 = x;
              previewPreLine.X1 = x;
              double num9 = num8;
              previewCurrLine.X2 = num9;
              this.previewPreLine.X2 = renderedVisual3.X2;
            }
            this.previewPreLine.Y2 = renderedVisual3.Y2;
            this.previewPreLine.Y1 = renderedVisual3.Y1;
          }
          this.previewCurrLine.Y1 = renderedVisual1.Y1;
          this.previewCurrLine.Y2 = renderedVisual1.Y2;
        }
      }
      this.isDragged = true;
      double num10 = this.Segments.Count == this.SegmentIndex ? (this.DraggingSegment as LineSegment).X2Value : (this.DraggingSegment as LineSegment).X1Value;
      if (this.DraggingPointIndicator != null)
      {
        double offsetX = this.DraggingPointIndicator.ActualWidth / 2.0;
        double offsetY = this.DraggingPointIndicator.ActualHeight / 2.0;
        Canvas.SetTop((UIElement) this.DraggingPointIndicator, num1 - offsetY);
        this.UpdateSegmentDragValueToolTip(new Point((this.DraggingSegment as LineSegment).X1Value, mousePos.Y), this.DraggingSegment, 0.0, this.DraggedValue, offsetX, offsetY);
      }
      else if (this.IsActualTransposed)
        this.UpdateSegmentDragValueToolTip(new Point(mousePos.X, this.Area.ValueToPoint(this.ActualXAxis, num10)), this.DraggingSegment, 0.0, this.DraggedValue, 0.0, 0.0);
      else
        this.UpdateSegmentDragValueToolTip(new Point(this.Area.ValueToPoint(this.ActualXAxis, num10), mousePos.Y), this.DraggingSegment, 0.0, this.DraggedValue, 0.0, 0.0);
    }
    catch
    {
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
            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newIndex].Item,
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
          this.selectionChangedEventArgs.PreviousSelectedIndex = eventArgs.SelectedIndex;
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
            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : (ChartSeriesBase) null,
            NewPointInfo = this.Segments[newIndex - 1].Item,
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
          this.selectionChangedEventArgs.PreviousSelectedIndex = eventArgs.SelectedIndex;
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
          PreviousSelectedSeries = (ChartSeriesBase) this,
          OldPointInfo = this.Segments[oldIndex].Item,
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
          PreviousSelectedSeries = (ChartSeriesBase) this,
          OldPointInfo = this.Segments[oldIndex - 1].Item,
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new LineSegment();

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
    base.OnMouseMove(e);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new LineSeries()
    {
      CustomTemplate = this.CustomTemplate,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  protected override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.hasTemplate = false;
    if (this.SeriesPanel == null)
      return;
    base.ResetDraggingElements(reason, dragEndEvent);
    if (this.SeriesPanel.Children.Contains((UIElement) this.previewPreLine))
    {
      this.SeriesPanel.Children.Remove((UIElement) this.previewPreLine);
      this.SeriesPanel.Children.Remove((UIElement) this.previewCurrLine);
    }
    if (this.SeriesPanel.Children.Contains(this.PreviewSeries))
    {
      (this.PreviewSeries as Polyline).Points.Clear();
      this.SeriesPanel.Children.Remove(this.PreviewSeries);
    }
    this.pointCollection = (PointCollection) null;
    this.previewPreLine = (Line) null;
    this.previewCurrLine = (Line) null;
    this.DraggingSegment = (ChartSegment) null;
    this.PreviewSeries = (UIElement) null;
    this.isDragged = false;
    this.DraggedValue = 0.0;
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

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    LineSeries lineSeries = d as LineSeries;
    if (lineSeries.Area == null)
      return;
    lineSeries.Segments.Clear();
    lineSeries.Area.ScheduleUpdate();
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
    (d as LineSeries).UpdateArea();
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
        if (this.PreviewSeries != null)
        {
          for (int index = 0; index < this.YValues.Count; ++index)
            this.YValues[index] = this.GetSnapToPoint(this.YValues[index] + this.DraggedValue);
          if (this.UpdateSource)
            this.UpdateUnderLayingModel(this.YBindingPath, this.YValues);
        }
        else
        {
          this.DraggedValue = this.GetSnapToPoint(this.DraggedValue);
          this.YValues[this.SegmentIndex] = this.DraggedValue;
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
