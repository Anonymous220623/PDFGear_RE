// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineSeries3D
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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LineSeries3D : XyDataSeries3D
{
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (int), typeof (LineSeries3D), new PropertyMetadata((object) 6, new PropertyChangedCallback(LineSeries3D.OnStrokeThicknessValueChanged)));
  private Point hitPoint = new Point();

  public LineSeries3D() => this.IsAnimated = false;

  public int StrokeThickness
  {
    get => (int) this.GetValue(LineSeries3D.StrokeThicknessProperty);
    set => this.SetValue(LineSeries3D.StrokeThicknessProperty, (object) value);
  }

  public LineSegment3D Segment { get; set; }

  internal bool IsAnimated { get; set; }

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

  protected internal override ChartSegment SelectedSegment
  {
    get => (ChartSegment) this.GetDataPoint(this.SelectedIndex);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    DoubleRange segmentDepth = this.GetSegmentDepth(this.Area.Depth);
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if (this.GroupedSeriesYValues != null && this.GroupedSeriesYValues[0].Contains(double.NaN))
        this.CreateEmptyPointSegments(this.GroupedSeriesYValues[0], out List<List<double>> _, out List<List<double>> _);
      else if (xValues != null && (this.Segment == null || this.Segments.Count == 0))
      {
        this.Segment = new LineSegment3D(xValues, this.GroupedSeriesYValues[0], segmentDepth.Start, segmentDepth.End, this);
        this.Segment.SetData(xValues, this.GroupedSeriesYValues[0], segmentDepth.Start, segmentDepth.End);
        this.Segments.Add((ChartSegment) this.Segment);
      }
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], this.GroupedSeriesYValues[0][index], index, segmentDepth.Start);
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      if (this.YValues.Contains(double.NaN))
        this.CreateEmptyPointSegments(this.YValues, out List<List<double>> _, out List<List<double>> _);
      else if (xValues != null)
      {
        if (this.Segment == null || this.Segments.Count == 0)
        {
          this.Segment = new LineSegment3D(xValues, this.YValues, segmentDepth.Start, segmentDepth.End, this);
          this.Segment.SetData(xValues, this.YValues, segmentDepth.Start, segmentDepth.End);
          this.Segments.Add((ChartSegment) this.Segment);
        }
        else
          this.Segment.SetData(xValues, this.YValues, segmentDepth.Start, segmentDepth.End);
      }
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], this.YValues[index], index, segmentDepth.Start);
      }
    }
  }

  public override void CreateEmptyPointSegments(
    IList<double> yValues,
    out List<List<double>> yValList,
    out List<List<double>> xValList)
  {
    IList<double> doubleList = !(this.ActualXValues is IList<double>) || this.IsIndexed ? (IList<double>) this.GetXValues() : this.ActualXValues as IList<double>;
    DoubleRange segmentDepth = this.GetSegmentDepth(this.Area.Depth);
    base.CreateEmptyPointSegments(yValues, out yValList, out xValList);
    int index1 = 0;
    if (this.Segments.Count != yValList.Count)
      this.Segments.Clear();
    if (this.Segment == null || this.Segments.Count == 0)
    {
      for (int index2 = 0; index2 < yValList.Count && index2 < xValList.Count; ++index2)
      {
        if (index2 < xValList.Count && index2 < yValList.Count && xValList[index2].Count > 0 && yValList[index2].Count > 0)
        {
          this.Segment = new LineSegment3D(xValList[index2], (IList<double>) yValList[index2], segmentDepth.Start, segmentDepth.End, this);
          this.Segment.SetData(xValList[index2], (IList<double>) yValList[index2], segmentDepth.Start, segmentDepth.End);
          this.Segments.Add((ChartSegment) this.Segment);
        }
      }
    }
    else
    {
      if (doubleList == null)
        return;
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      {
        if (index1 < xValList.Count && index1 < yValList.Count && xValList[index1].Count > 0 && yValList[index1].Count > 0)
          segment.SetData(xValList[index1], (IList<double>) yValList[index1], segmentDepth.Start, segmentDepth.End);
        ++index1;
      }
    }
  }

  internal override bool GetAnimationIsActive()
  {
    return this.AnimationStoryboard != null && this.AnimationStoryboard.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    int num1 = 0;
    if (this.AnimationStoryboard != null)
    {
      this.AnimationStoryboard.Stop();
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    this.AnimationStoryboard = new Storyboard();
    foreach (LineSegment3D segment in (Collection<ChartSegment>) this.Segments)
    {
      DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
      double d = this.YValues.Min();
      SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
      splineDoubleKeyFrame1.Value = double.IsNaN(d) ? this.YValues.Where<double>((Func<double, bool>) (e => !double.IsNaN(e))).Min() : d;
      SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
      keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
      splineDoubleKeyFrame2.Value = this.YValues.Max();
      SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
      keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
      KeySpline keySpline = new KeySpline()
      {
        ControlPoint1 = new Point(0.64, 0.84),
        ControlPoint2 = new Point(0.67, 0.95)
      };
      keyFrame2.KeySpline = keySpline;
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) LineSegment3D.YProperty));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) segment);
      this.AnimationStoryboard.Children.Add((Timeline) element1);
      if (this.AdornmentsInfo != null)
      {
        double num2 = this.AnimationDuration.TotalSeconds / (double) this.YValues.Count;
        foreach (FrameworkElement labelPresenter in this.AdornmentsInfo.LabelPresenters)
        {
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(0.5);
          doubleAnimation.To = new double?(1.0);
          doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds((double) num1 * num2));
          DoubleAnimation element2 = doubleAnimation;
          element2.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds / 2.0));
          Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) UIElement.OpacityProperty));
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) labelPresenter);
          this.AnimationStoryboard.Children.Add((Timeline) element2);
          ++num1;
        }
      }
    }
    this.AnimationStoryboard.Begin();
  }

  internal override ChartDataPointInfo GetDataPoint(Point point)
  {
    Polygon3D plane1 = new Polygon3D(new Vector3D(0.0, 0.0, 1.0), 0.0);
    ChartTransform.ChartTransform3D transform = (this.ActualArea as SfChart3D).Graphics3D.Transform;
    plane1.Transform(transform.View);
    Vector3D plane2 = transform.ToPlane(point, plane1);
    List<int> intList = new List<int>();
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    int startIndex;
    int endIndex;
    Rect rect;
    this.CalculateHittestRect(new Point(plane2.X, plane2.Y), out startIndex, out endIndex, out rect);
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
    this.dataPoint.Series = (ChartSeriesBase) this;
    this.dataPoint.XData = doubleList[index1];
    this.dataPoint.YData = this.YValues[index1];
    if (this.ActualData.Count > index1)
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
        if (e.NewItems == null || (this.ActualArea as SfChart3D).SelectionStyle == SelectionStyle3D.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem = (int) e.NewItems[0];
        if (newItem < 0 || !(this.ActualArea as SfChart3D).EnableSegmentSelection)
          break;
        if (this.Segments.Count > 0)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.SelectedSegment,
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.Area.PreviousSelectedSeries;
          if (previousSelectedIndex != -1 && previousSelectedIndex < this.Segments.Count)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(previousSelectedIndex);
          }
          (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.Segments.Count != 0)
          break;
        this.triggerSelectionChangedEventOnLoad = true;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || (this.ActualArea as SfChart3D).SelectionStyle == SelectionStyle3D.Single)
          break;
        int oldItem = (int) e.OldItems[0];
        ChartSelectionChangedEventArgs eventArgs1 = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedIndex = oldItem,
          PreviousSelectedIndex = this.PreviousSelectedIndex,
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.Segments.Count)
        {
          eventArgs1.PreviousSelectedSegment = this.Segments[0];
          eventArgs1.OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex);
        }
        (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs1);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea == null)
      return;
    if (newIndex >= 0 && (this.ActualArea as SfChart3D).EnableSegmentSelection)
    {
      if (this.Segments.Count > 0)
      {
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = this.SelectedSegment,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) this,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          IsSelected = true
        };
        eventArgs.PreviousSelectedSeries = this.Area.PreviousSelectedSeries;
        if (oldIndex != -1 && oldIndex < this.Segments.Count)
        {
          eventArgs.PreviousSelectedSegment = this.Segments[0];
          eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
        }
        (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
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
      if (newIndex != -1 || this.ActualArea == null)
        return;
      ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSeries = (ChartSeriesBase) null,
        SelectedSegments = this.Area.SelectedSegments,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        PreviousSelectedSegment = (ChartSegment) null,
        PreviousSelectedSeries = (ChartSeriesBase) this,
        IsSelected = false
      };
      if (oldIndex != -1 && oldIndex < this.Segments.Count)
      {
        eventArgs.PreviousSelectedSegment = this.Segments[0];
        eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
      }
      (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
      this.PreviousSelectedIndex = newIndex;
    }
  }

  protected internal override void OnSeriesMouseMove(object source, Point pos)
  {
    if (this.SelectionMode == SelectionMode.MouseMove)
      this.OnMouseMoveSelection(source as FrameworkElement, pos);
    this.mousePos = pos;
    this.UpdateToolTip(source, pos);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    LineSeries3D lineSeries3D = new LineSeries3D();
    lineSeries3D.StrokeThickness = this.StrokeThickness;
    lineSeries3D.SegmentSelectionBrush = this.SegmentSelectionBrush;
    lineSeries3D.SelectedIndex = this.SelectedIndex;
    return base.CloneSeries((DependencyObject) lineSeries3D);
  }

  private static void OnStrokeThicknessValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    LineSeries3D lineSeries3D = d as LineSeries3D;
    if (lineSeries3D.Area == null)
      return;
    lineSeries3D.Area.ScheduleUpdate();
  }

  private void UpdateToolTip(object source, Point pos)
  {
    if (!this.ShowTooltip)
      return;
    this.SetTooltipDuration();
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    this.mousePos = pos;
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    object obj = (object) null;
    LineSegment3D lineSegment3D = (!(source is Shape shape) || !(shape.Tag is ChartSegment3D) ? (object) this.Segments[0] : shape.Tag) as LineSegment3D;
    if (this.Area.SeriesClipRect.Contains(this.mousePos))
    {
      this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      if (double.IsNaN(x))
        return;
      obj = this.ActualData[!(this.ActualXAxis is CategoryAxis3D) || (this.ActualXAxis as CategoryAxis3D).IsIndexed ? this.GetXValues().IndexOf(x) : (int) this.GroupedXValuesIndexes[(int) x]];
    }
    if (this.Area.Tooltip == null)
      this.Area.Tooltip = new ChartTooltip();
    ChartTooltip element = this.Area.Tooltip;
    lineSegment3D.Item = obj;
    lineSegment3D.XData = x;
    lineSegment3D.YData = y;
    if (element == null)
      return;
    element.ClearValue(ContentControl.ContentProperty);
    element.ClearValue(ContentControl.ContentTemplateProperty);
    element.PolygonPath = " ";
    this.ToolTipTag = (object) lineSegment3D;
    if (adorningCanvas.Children.Count == 0 || adorningCanvas.Children.Count > 0 && !this.IsTooltipAvailable(adorningCanvas))
    {
      element.DataContext = (object) lineSegment3D;
      if (element.DataContext == null)
        return;
      if (ChartTooltip.GetInitialShowDelay((DependencyObject) this) == 0)
      {
        this.HastoolTip = true;
        adorningCanvas.Children.Add((UIElement) element);
      }
      element.ContentTemplate = this.GetTooltipTemplate();
      this.AddTooltip();
      Canvas.SetLeft((UIElement) element, element.LeftOffset);
      Canvas.SetTop((UIElement) element, element.TopOffset);
    }
    else
    {
      foreach (object child in adorningCanvas.Children)
      {
        if (child is ChartTooltip chartTooltip)
          element = chartTooltip;
      }
      element.DataContext = (object) lineSegment3D;
      if (element.DataContext == null)
      {
        this.RemoveTooltip();
      }
      else
      {
        element.ContentTemplate = this.GetTooltipTemplate();
        this.AddTooltip();
        Canvas.SetLeft((UIElement) element, element.LeftOffset);
        Canvas.SetTop((UIElement) element, element.TopOffset);
      }
    }
  }
}
