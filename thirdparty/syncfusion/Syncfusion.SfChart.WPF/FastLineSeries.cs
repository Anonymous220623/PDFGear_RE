// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastLineSeries
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

public class FastLineSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (FastLineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastLineSeries.OnCustomTemplateChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (FastLineSeries), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register(nameof (StrokeDashOffset), typeof (double), typeof (FastLineSeries), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register(nameof (StrokeDashCap), typeof (PenLineCap), typeof (FastLineSeries), new PropertyMetadata((object) PenLineCap.Flat));
  public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(nameof (StrokeLineJoin), typeof (PenLineJoin), typeof (FastLineSeries), new PropertyMetadata((object) PenLineJoin.Miter));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastLineSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastLineSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastLineSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastLineSeries.OnSelectedIndexChanged)));
  private Storyboard sb;
  private RectangleGeometry geometry;
  private IList<double> xValues;
  private ChartDataPointInfo prevDataPoint;
  private bool isAdornmentPending;

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastLineSeries.SelectedIndexProperty);
    set => this.SetValue(FastLineSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastLineSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastLineSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(FastLineSeries.StrokeDashArrayProperty);
    set => this.SetValue(FastLineSeries.StrokeDashArrayProperty, (object) value);
  }

  public double StrokeDashOffset
  {
    get => (double) this.GetValue(FastLineSeries.StrokeDashOffsetProperty);
    set => this.SetValue(FastLineSeries.StrokeDashOffsetProperty, (object) value);
  }

  public PenLineCap StrokeDashCap
  {
    get => (PenLineCap) this.GetValue(FastLineSeries.StrokeDashCapProperty);
    set => this.SetValue(FastLineSeries.StrokeDashCapProperty, (object) value);
  }

  public PenLineJoin StrokeLineJoin
  {
    get => (PenLineJoin) this.GetValue(FastLineSeries.StrokeLineJoinProperty);
    set => this.SetValue(FastLineSeries.StrokeLineJoinProperty, (object) value);
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(FastLineSeries.CustomTemplateProperty);
    set => this.SetValue(FastLineSeries.CustomTemplateProperty, (object) value);
  }

  protected internal override bool IsLinear => true;

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

  private FastLineSegment Segment { get; set; }

  public override void CreateSegments()
  {
    if (this.GroupedSeriesYValues != null && this.GroupedSeriesYValues[0].Contains(double.NaN))
      this.CreateEmptyPointSegments(this.GroupedSeriesYValues[0], out List<List<double>> _, out List<List<double>> _);
    else if (this.YValues.Contains(double.NaN))
    {
      this.CreateEmptyPointSegments(this.YValues, out List<List<double>> _, out List<List<double>> _);
    }
    else
    {
      bool flag = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed;
      this.xValues = flag ? (!(this.ActualXValues is IList<double>) || this.IsIndexed ? (IList<double>) this.GetXValues() : this.ActualXValues as IList<double>) : (IList<double>) this.GroupedXValuesIndexes;
      if (!flag)
      {
        this.Segments.Clear();
        this.Adornments.Clear();
        if ((this.Segment == null || this.Segments.Count == 0) && this.CreateSegment() is FastLineSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.customTemplate = this.CustomTemplate;
          segment.SetData(this.xValues, this.GroupedSeriesYValues[0]);
          segment.Item = (object) this.ActualData;
          this.Segment = segment;
          this.Segments.Add((ChartSegment) segment);
        }
      }
      else
      {
        this.ClearUnUsedAdornments(this.DataCount);
        if (this.Segment == null || this.Segments.Count == 0)
        {
          if (this.CreateSegment() is FastLineSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.customTemplate = this.CustomTemplate;
            segment.SetData(this.xValues, this.YValues);
            segment.Item = (object) this.ActualData;
            this.Segment = segment;
            this.Segments.Add((ChartSegment) segment);
          }
        }
        else if (this.ActualXValues != null)
        {
          this.Segment.SetData(this.xValues, this.YValues);
          this.Segment.Item = (object) this.ActualData;
        }
      }
      this.isAdornmentPending = true;
    }
  }

  public override void CreateEmptyPointSegments(
    IList<double> YValues,
    out List<List<double>> yValList,
    out List<List<double>> xValList)
  {
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      this.xValues = (IList<double>) this.GroupedXValuesIndexes;
      base.CreateEmptyPointSegments(this.GroupedSeriesYValues[0], out yValList, out xValList);
    }
    else
    {
      this.xValues = !(this.ActualXValues is IList<double>) || this.IsIndexed ? (IList<double>) this.GetXValues() : this.ActualXValues as IList<double>;
      base.CreateEmptyPointSegments(YValues, out yValList, out xValList);
    }
    int index1 = 0;
    if (this.Segments.Count != yValList.Count)
      this.Segments.Clear();
    this.ClearUnUsedAdornments(this.DataCount);
    if (this.Segment == null || this.Segments.Count == 0)
    {
      for (int index2 = 0; index2 < yValList.Count && index2 < xValList.Count; ++index2)
      {
        if (index2 < xValList.Count && index2 < yValList.Count && xValList[index2].Count > 0 && yValList[index2].Count > 0)
        {
          this.Segment = new FastLineSegment((IList<double>) xValList[index2], (IList<double>) yValList[index2], (AdornmentSeries) this);
          this.Segments.Add((ChartSegment) this.Segment);
        }
      }
    }
    else if (this.xValues != null)
    {
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      {
        if (index1 < xValList.Count && index1 < yValList.Count && xValList[index1].Count > 0 && yValList[index1].Count > 0)
        {
          segment.SetData((IList<double>) xValList[index1], (IList<double>) yValList[index1]);
          (segment as FastLineSegment).SetRange();
        }
        ++index1;
      }
    }
    this.isAdornmentPending = true;
  }

  internal override void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    ChartSegment chartSegment1 = (ChartSegment) null;
    if (this.prevDataPoint != null && this.Segments.Count > 0)
      chartSegment1 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is FastLineSegment && (segment as FastLineSegment).xChartVals.Contains(this.prevDataPoint.XData))).FirstOrDefault<ChartSegment>();
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
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSegment chartSegment2 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is FastLineSegment && (segment as FastLineSegment).xChartVals.Contains(this.dataPoint.XData))).FirstOrDefault<ChartSegment>();
          if (chartSegment2 == null)
            break;
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = chartSegment2,
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
            eventArgs.PreviousSelectedSegment = chartSegment1;
            eventArgs.OldPointInfo = (object) this.GetDataPoint(previousSelectedIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          this.prevDataPoint = this.dataPoint;
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
          NewPointInfo = (object) null,
          OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex),
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1)
          eventArgs1.PreviousSelectedSegment = chartSegment1;
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
    ChartDataPointInfo customTag = new ChartDataPointInfo();
    if (originalSource as FrameworkElement is Polyline)
    {
      customTag = this.GetDataPoint(this.mousePos);
    }
    else
    {
      int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex(originalSource);
      if (adornmentIndex > -1)
      {
        customTag.Index = adornmentIndex;
        if (this.xValues.Count > adornmentIndex)
          customTag.XData = this.xValues[adornmentIndex];
        if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && this.GroupedSeriesYValues[0].Count > adornmentIndex)
          customTag.YData = this.GroupedSeriesYValues[0][adornmentIndex];
        else if (this.YValues.Count > adornmentIndex)
          customTag.YData = this.YValues[adornmentIndex];
        customTag.Series = (ChartSeriesBase) this;
        if (this.ActualData.Count > adornmentIndex)
          customTag.Item = this.ActualData[adornmentIndex];
      }
    }
    this.UpdateSeriesTooltip((object) customTag);
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    List<int> intList = new List<int>();
    double stackedYValue = double.NaN;
    this.CalculateHittestRect(mousePos, out int _, out int _, out Rect _);
    double x;
    double y;
    this.FindNearestChartPoint(new Point(mousePos.X - this.Area.SeriesClipRect.Left, mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
    int index = this.xValues.IndexOf(x);
    this.dataPoint = new ChartDataPointInfo();
    this.dataPoint.Index = index;
    this.dataPoint.XData = x;
    this.dataPoint.YData = y;
    if (index > -1 && this.ActualData.Count > index)
      this.dataPoint.Item = this.ActualData[index];
    this.dataPoint.Series = (ChartSeriesBase) this;
    return this.dataPoint;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
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
        ScaleX = 0.6,
        ScaleY = 0.6
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
    ChartSegment chartSegment1 = (ChartSegment) null;
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
      if (this.prevDataPoint != null && this.Segments.Count > 0)
        chartSegment1 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is FastLineSegment && (segment as FastLineSegment).xChartVals.Contains(this.prevDataPoint.XData))).FirstOrDefault<ChartSegment>();
      if (newIndex >= 0 && this.ActualArea.GetEnableSegmentSelection())
      {
        if (!this.SelectedSegmentsIndexes.Contains(newIndex))
          this.SelectedSegmentsIndexes.Add(newIndex);
        if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newIndex);
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSegment chartSegment2 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment is FastLineSegment && (segment as FastLineSegment).xChartVals.Contains(this.dataPoint.XData))).FirstOrDefault<ChartSegment>();
          if (chartSegment2 == null)
            return;
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = chartSegment2,
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
            eventArgs.PreviousSelectedSegment = chartSegment1;
            eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newIndex;
          this.prevDataPoint = this.dataPoint;
        }
        else
        {
          if (this.Segments.Count != 0)
            return;
          this.triggerSelectionChangedEventOnLoad = true;
        }
      }
      else
      {
        if (newIndex != -1)
          return;
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          NewPointInfo = (object) null,
          OldPointInfo = (object) this.GetDataPoint(oldIndex),
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (oldIndex != -1)
          eventArgs.PreviousSelectedSegment = chartSegment1;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastLineSegment();

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.RemovePreviousSeriesTooltip();
    this.UpdateTooltip(e.OriginalSource);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (FastLineSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
    if (this.AdornmentsInfo == null || !this.isAdornmentPending)
      return;
    if (this.xValues != null && this.ActualXAxis != null && !this.ActualXAxis.VisibleRange.IsEmpty)
    {
      double newBase = this.ActualXAxis.IsLogarithmic ? (this.ActualXAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      bool isLogarithmic = this.ActualXAxis.IsLogarithmic;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      for (int index = 0; index < this.DataCount; ++index)
      {
        double yvalue;
        double xValue;
        if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
        {
          if (index >= this.xValues.Count)
            return;
          yvalue = this.GroupedSeriesYValues[0][index];
          xValue = this.xValues[index];
        }
        else
        {
          xValue = this.xValues[index];
          yvalue = this.YValues[index];
        }
        double num = isLogarithmic ? Math.Log(xValue, newBase) : xValue;
        if (num >= start && num <= end && !double.IsNaN(yvalue))
        {
          if (index < this.Adornments.Count)
          {
            this.Adornments[index].SetData(xValue, yvalue, xValue, yvalue);
            this.Adornments[index].Item = this.ActualData[index];
          }
          else
          {
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValue, yvalue, xValue, yvalue));
            this.Adornments[this.Adornments.Count - 1].Item = this.ActualData[index];
          }
        }
      }
    }
    this.isAdornmentPending = false;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastLineSeries()
    {
      StrokeDashArray = this.StrokeDashArray,
      StrokeDashOffset = this.StrokeDashOffset,
      StrokeLineJoin = this.StrokeLineJoin,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      StrokeDashCap = this.StrokeDashCap
    });
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FastLineSeries fastLineSeries = d as FastLineSeries;
    if (fastLineSeries.Area == null)
      return;
    fastLineSeries.Segments.Clear();
    fastLineSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastLineSeries).UpdateArea();
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
}
