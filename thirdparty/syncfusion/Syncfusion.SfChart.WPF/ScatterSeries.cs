// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ScatterSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ScatterSeries : XySegmentDraggingBase, ISegmentSelectable
{
  public static readonly DependencyProperty DragDirectionProperty = DependencyProperty.Register(nameof (DragDirection), typeof (DragType), typeof (ScatterSeries), new PropertyMetadata((object) DragType.XY));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (ScatterSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ScatterSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty ScatterWidthProperty = DependencyProperty.Register(nameof (ScatterWidth), typeof (double), typeof (ScatterSeries), new PropertyMetadata((object) 20.0, new PropertyChangedCallback(ScatterSeries.OnScatterWidthChanged)));
  public static readonly DependencyProperty ScatterHeightProperty = DependencyProperty.Register(nameof (ScatterHeight), typeof (double), typeof (ScatterSeries), new PropertyMetadata((object) 20.0, new PropertyChangedCallback(ScatterSeries.OnScatterHeightChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (ScatterSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(ScatterSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (ScatterSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ScatterSeries.OnCustomTemplateChanged)));
  private Storyboard sb;
  private UIElement previewElement;
  private bool isDragged;
  private ScatterSegment draggingSegment;
  private Point leftButtonDownPoint;

  public DragType DragDirection
  {
    get => (DragType) this.GetValue(ScatterSeries.DragDirectionProperty);
    set => this.SetValue(ScatterSeries.DragDirectionProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(ScatterSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(ScatterSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public double ScatterWidth
  {
    get => (double) this.GetValue(ScatterSeries.ScatterWidthProperty);
    set => this.SetValue(ScatterSeries.ScatterWidthProperty, (object) value);
  }

  public double ScatterHeight
  {
    get => (double) this.GetValue(ScatterSeries.ScatterHeightProperty);
    set => this.SetValue(ScatterSeries.ScatterHeightProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(ScatterSeries.SelectedIndexProperty);
    set => this.SetValue(ScatterSeries.SelectedIndexProperty, (object) value);
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(ScatterSeries.CustomTemplateProperty);
    set => this.SetValue(ScatterSeries.CustomTemplateProperty, (object) value);
  }

  internal bool IsDraggingActivateOnly { get; set; }

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
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (index < this.GroupedSeriesYValues[0].Count)
        {
          if (this.CreateSegment() is ScatterSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.CustomTemplate = this.CustomTemplate;
            segment.SetData(xValues[index], this.GroupedSeriesYValues[0][index]);
            segment.YData = this.GroupedSeriesYValues[0][index];
            segment.XData = xValues[index];
            segment.Item = this.ActualData[index];
            this.Segments.Add((ChartSegment) segment);
          }
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index], this.GroupedSeriesYValues[0][index], index);
        }
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (index < this.Segments.Count)
        {
          this.Segments[index].Item = this.ActualData[index];
          this.Segments[index].SetData(xValues[index], this.YValues[index]);
          (this.Segments[index] as ScatterSegment).XData = xValues[index];
          (this.Segments[index] as ScatterSegment).Item = this.ActualData[index];
          if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
            this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        }
        else if (this.CreateSegment() is ScatterSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.CustomTemplate = this.CustomTemplate;
          segment.SetData(xValues[index], this.YValues[index]);
          segment.YData = this.YValues[index];
          segment.XData = xValues[index];
          segment.Item = this.ActualData[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], this.YValues[index], index);
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
      if (segment.GetRenderedVisual() is Ellipse renderedVisual && ScatterSeries.EllipseContainsPoint(renderedVisual, point))
        return this.Segments.IndexOf(segment);
    }
    return -1;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ScatterSegment toolTipTag = this.ToolTipTag as ScatterSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top - toolTipTag.ScatterHeight / 2.0;
    if (dataPointPosition.Y - tooltip.DesiredSize.Height < this.ActualArea.SeriesClipRect.Top)
      dataPointPosition.Y += toolTipTag.ScatterHeight;
    return dataPointPosition;
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
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
        foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
          (segment.GetRenderedVisual() as FrameworkElement).ClearValue(UIElement.RenderTransformProperty);
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    this.sb = new Storyboard();
    foreach (ScatterSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) (random.Next(0, 50) * 20));
      FrameworkElement reference = (FrameworkElement) segment.GetRenderedVisual();
      if (this.CustomTemplate != null && VisualTreeHelper.GetChild(VisualTreeHelper.GetChild((DependencyObject) reference, 0), 0) is Canvas child1 && VisualTreeHelper.GetChild((DependencyObject) child1, 0) is FrameworkElement child2)
        reference = child2;
      reference.RenderTransform = (Transform) new ScaleTransform()
      {
        ScaleY = 0.0,
        ScaleX = 0.0
      };
      reference.RenderTransformOrigin = new Point(0.5, 0.5);
      DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
      element1.BeginTime = new TimeSpan?(timeSpan);
      SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
      keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
      keyFrame1.Value = 0.0;
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 30.0 / 100.0));
      keyFrame2.KeySpline = new KeySpline()
      {
        ControlPoint1 = new Point(0.64, 0.84),
        ControlPoint2 = new Point(0.67, 0.95)
      };
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      keyFrame2.Value = 1.0;
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) reference);
      this.sb.Children.Add((Timeline) element1);
      DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
      element2.BeginTime = new TimeSpan?(timeSpan);
      SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
      keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
      keyFrame3.Value = 0.0;
      element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
      SplineDoubleKeyFrame keyFrame4 = new SplineDoubleKeyFrame();
      keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 30.0 / 100.0));
      keyFrame4.KeySpline = new KeySpline()
      {
        ControlPoint1 = new Point(0.64, 0.84),
        ControlPoint2 = new Point(0.67, 0.95)
      };
      element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
      keyFrame4.Value = 1.0;
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", new object[0]));
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) reference);
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
        element3.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 50.0 / 100.0));
        Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
        Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
        this.sb.Children.Add((Timeline) element3);
      }
      ++index;
    }
    this.sb.Begin();
  }

  internal override void ActivateDragging(Point mousePos, object element)
  {
    try
    {
      Keyboard.Focus((IInputElement) this);
      this.IsDraggingActivateOnly = true;
      this.DeltaX = 0.0;
      this.delta = 0.0;
      this.KeyDown += new KeyEventHandler(((XySegmentDraggingBase) this).CoreWindow_KeyDown);
      ChartXyDragStartEventArgs dragStartEventArgs = new ChartXyDragStartEventArgs();
      dragStartEventArgs.BaseXValue = this.GetActualXValue(this.SegmentIndex);
      dragStartEventArgs.BaseYValue = this.draggingSegment.YData;
      ChartDragStartEventArgs args = (ChartDragStartEventArgs) dragStartEventArgs;
      if (this.EmptyPointIndexes != null)
      {
        foreach (int num in this.EmptyPointIndexes[0])
        {
          if (this.SegmentIndex == num)
          {
            args.EmptyPoint = true;
            break;
          }
        }
      }
      this.RaiseDragStart(args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("Cancel", true);
        this.SegmentIndex = -1;
      }
      this.UnHoldPanning(false);
      if (this.draggingSegment == null || this.previewElement != null)
        return;
      this.previewElement = this.GetPreviewEllipse();
      this.previewElement.Opacity = 0.5;
      this.SeriesPanel.Children.Add(this.previewElement);
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new ScatterSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    ScatterSeries scatterSeries = new ScatterSeries();
    scatterSeries.ScatterHeight = this.ScatterHeight;
    scatterSeries.ScatterWidth = this.ScatterWidth;
    scatterSeries.SelectedIndex = this.SelectedIndex;
    scatterSeries.SegmentSelectionBrush = this.SegmentSelectionBrush;
    scatterSeries.CustomTemplate = this.CustomTemplate;
    scatterSeries.EnableSegmentDragging = this.EnableSegmentDragging;
    scatterSeries.DragDirection = this.DragDirection;
    return base.CloneSeries((DependencyObject) scatterSeries);
  }

  protected override void OnChartDragStart(Point mousePos, object originalSource)
  {
    this.leftButtonDownPoint = mousePos;
    this.ActivateDragging(mousePos, originalSource);
  }

  protected override void OnTouchDown(TouchEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource))
      return;
    bool isAdornment = false;
    int segmentIndex = this.SegmentIndex;
    this.draggingSegment = (ScatterSegment) null;
    this.SetScatterDraggingSegment(originalSource, ref isAdornment);
    if (this.draggingSegment == null)
      return;
    if (segmentIndex != this.SegmentIndex)
    {
      this.prevDraggedXValue = 0.0;
      this.prevDraggedValue = 0.0;
    }
    base.OnTouchDown(e);
  }

  protected override void OnChartDragEntered(Point mousePos, object originalSource)
  {
    if (!(originalSource is FrameworkElement frameworkElement))
      return;
    this.UpdatePreviewIndicatorPosition(mousePos, frameworkElement);
    base.OnChartDragEntered(mousePos, originalSource);
  }

  protected override void OnChartDragDelta(Point mousePos, object originalSource)
  {
    if (this.leftButtonDownPoint == mousePos)
      this.ResetDraggingElements("MouseMoveSamePoint", false);
    this.SegmentPreview(mousePos);
  }

  protected override void OnChartDragEnd(Point mousePos, object originalSource)
  {
    if (this.isDragged)
      this.UpdateDraggedSource();
    this.ResetDraggingElements("", false);
  }

  protected override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.ResetDraggingindicators();
    this.isDragged = false;
    base.ResetDraggingElements(reason, dragEndEvent);
    if (this.previewElement == null)
      return;
    this.SeriesPanel.Children.Remove(this.previewElement);
    this.previewElement = (UIElement) null;
    this.DraggedXValue = 0.0;
    this.DraggedValue = 0.0;
  }

  protected override void OnChartDragExited(Point mousePos, object originalSource)
  {
    if (!this.EnableSegmentDragging)
      return;
    this.ResetDraggingindicators();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ScatterSeries).UpdateArea();
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
    ScatterSeries scatterSeries = d as ScatterSeries;
    if (scatterSeries.Area == null)
      return;
    scatterSeries.Segments.Clear();
    scatterSeries.Area.ScheduleUpdate();
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

  private static void OnScatterHeightChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ScatterSeries scatterSeries = d as ScatterSeries;
    foreach (ChartSegment segment in (Collection<ChartSegment>) scatterSeries.Segments)
      (segment as ScatterSegment).ScatterHeight = scatterSeries.ScatterHeight;
    scatterSeries?.UpdateArea();
  }

  private static void OnScatterWidthChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ScatterSeries scatterSeries = d as ScatterSeries;
    foreach (ChartSegment segment in (Collection<ChartSegment>) scatterSeries.Segments)
      (segment as ScatterSegment).ScatterWidth = scatterSeries.ScatterWidth;
    scatterSeries?.UpdateArea();
  }

  private void AddAdornments(double x, double yValue, int i)
  {
    double num1 = x;
    double num2 = yValue;
    if (i < this.Adornments.Count)
      this.Adornments[i].SetData(num1, num2, num1, num2);
    else
      this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num1, num2, num1, num2));
    this.Adornments[i].Item = this.ActualData[i];
  }

  private UIElement GetPreviewEllipse()
  {
    ScatterSegment target = new ScatterSegment();
    target.SetData(this.draggingSegment.XData, this.draggingSegment.YData);
    target.CustomTemplate = this.CustomTemplate;
    target.Series = (ChartSeriesBase) this;
    UIElement segmentVisual = target.CreateSegmentVisual(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Binding binding = new Binding()
    {
      Source = (object) this.Segments[this.SegmentIndex],
      Path = new PropertyPath("Interior", new object[0])
    };
    BindingOperations.SetBinding((DependencyObject) target, ChartSegment.InteriorProperty, (BindingBase) binding);
    target.Update(this.ChartTransformer);
    return segmentVisual;
  }

  private void UpdatePreviewIndicatorPosition(Point mousePos, FrameworkElement frameworkElement)
  {
    if (this.previewElement != null)
      return;
    bool isAdornment = false;
    int segmentIndex = this.SegmentIndex;
    this.draggingSegment = (ScatterSegment) null;
    this.SetScatterDraggingSegment(frameworkElement, ref isAdornment);
    if (this.draggingSegment == null)
      return;
    if (segmentIndex != this.SegmentIndex)
    {
      this.prevDraggedXValue = 0.0;
      this.prevDraggedValue = 0.0;
    }
    XySegmentEnterEventArgs args = new XySegmentEnterEventArgs()
    {
      XValue = this.GetActualXValue(this.SegmentIndex),
      YValue = (object) this.draggingSegment.YData,
      SegmentIndex = this.SegmentIndex,
      CanDrag = true
    };
    this.RaiseDragEnter(args);
    if (!args.CanDrag)
      return;
    double logPoint1 = this.Area.ValueToLogPoint(this.ActualYAxis, this.draggingSegment.YData);
    double logPoint2 = this.Area.ValueToLogPoint(this.ActualXAxis, this.draggingSegment.XData);
    if (this.CustomTemplate != null)
    {
      this.AnimationElement = (FrameworkElement) (this.GetPreviewEllipse() as ContentControl);
      this.AnimateSegmentTemplate(logPoint2, logPoint1, (ChartSegment) this.draggingSegment);
    }
    else if (isAdornment)
    {
      if (this.AdornmentsInfo.Symbol.ToString() == "Custom")
        this.AnimateAdornmentSymbolTemplate(logPoint2, logPoint1);
      else
        this.AddAnimationEllipse(ChartDictionaries.GenericSymbolDictionary[(object) ("Animation" + this.AdornmentsInfo.Symbol.ToString())] as ControlTemplate, this.AdornmentsInfo.SymbolHeight, this.AdornmentsInfo.SymbolWidth, logPoint2, logPoint1, (object) this.Adornments[this.SegmentIndex], true);
    }
    else
      this.AddAnimationEllipse(ChartDictionaries.GenericSymbolDictionary[(object) "AnimationEllipse"] as ControlTemplate, this.ScatterHeight, this.ScatterWidth, logPoint2, logPoint1, (object) this.Segments[this.SegmentIndex], false);
  }

  private void SetScatterDraggingSegment(FrameworkElement frameworkElement, ref bool isAdornment)
  {
    if (frameworkElement.Tag is EmptyPointSegment || frameworkElement.DataContext is EmptyPointSegment)
      return;
    if (frameworkElement.Tag is ScatterSegment || frameworkElement.DataContext is ScatterSegment)
    {
      this.draggingSegment = XySegmentDraggingBase.GetDraggingSegment((object) frameworkElement) as ScatterSegment;
      if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
        this.SegmentIndex = this.ActualData.IndexOf(this.draggingSegment.Item);
      else
        this.SegmentIndex = this.Segments.IndexOf((ChartSegment) this.draggingSegment);
    }
    else
    {
      isAdornment = true;
      this.SegmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) frameworkElement);
      if (this.SegmentIndex <= -1)
        return;
      this.draggingSegment = this.Segments[this.SegmentIndex] as ScatterSegment;
    }
  }

  private void SegmentPreview(Point mousePos)
  {
    try
    {
      if (this.previewElement == null)
        return;
      this.IsDraggingActivateOnly = false;
      double x = mousePos.X;
      double y = mousePos.Y;
      this.DraggedXValue = this.DragDirection == DragType.Y || this.IsIndexed ? this.draggingSegment.XData : this.Area.PointToValue(this.ActualXAxis, mousePos);
      this.DraggedValue = this.DragDirection == DragType.X ? this.draggingSegment.YData : this.Area.PointToValue(this.ActualYAxis, mousePos);
      object obj = this.IsIndexed ? (object) 0.0 : this.GetActualXDelta(this.prevDraggedXValue, this.DraggedXValue, ref this.DeltaX);
      double actualDelta = this.GetActualDelta();
      object actualXvalue = this.GetActualXValue(this.SegmentIndex);
      XyDeltaDragEventArgs deltaDragEventArgs = new XyDeltaDragEventArgs();
      deltaDragEventArgs.NewXValue = this.IsIndexed ? actualXvalue : this.GetDraggedActualXValue(this.DraggedXValue);
      deltaDragEventArgs.NewYValue = this.DraggedValue;
      deltaDragEventArgs.BaseXValue = actualXvalue;
      deltaDragEventArgs.BaseYValue = this.draggingSegment.YData;
      deltaDragEventArgs.Segment = (ChartSegment) this.draggingSegment;
      deltaDragEventArgs.Delta = actualDelta;
      deltaDragEventArgs.DeltaX = obj;
      XySegmentDragEventArgs args = (XySegmentDragEventArgs) deltaDragEventArgs;
      this.prevDraggedXValue = this.DraggedXValue;
      this.prevDraggedValue = this.DraggedValue;
      this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("Cancel", true);
      }
      else
      {
        if (this.DragDirection == DragType.Y || this.IsIndexed)
        {
          if (this.IsActualTransposed)
            y = this.Area.ValueToLogPoint(this.ActualXAxis, this.draggingSegment.XData);
          else
            x = this.Area.ValueToLogPoint(this.ActualXAxis, this.draggingSegment.XData);
        }
        if (this.DragDirection == DragType.X)
        {
          if (this.IsActualTransposed)
            x = this.Area.ValueToLogPoint(this.ActualYAxis, this.draggingSegment.YData);
          else
            y = this.Area.ValueToLogPoint(this.ActualYAxis, this.draggingSegment.YData);
        }
        if (this.CustomTemplate != null)
        {
          ScatterSegment content = (this.previewElement as ContentControl).Content as ScatterSegment;
          content.RectY = y - this.ScatterHeight / 2.0;
          content.RectX = x - this.ScatterWidth / 2.0;
        }
        else
        {
          this.previewElement.SetValue(Canvas.TopProperty, (object) (y - this.ScatterHeight / 2.0));
          this.previewElement.SetValue(Canvas.LeftProperty, (object) (x - this.ScatterWidth / 2.0));
        }
        this.UpdateSegmentDragValueToolTip(new Point(x, y), (ChartSegment) this.draggingSegment, this.DraggedXValue, this.DraggedValue, this.ScatterWidth / 2.0, this.ScatterHeight / 2.0);
        this.ResetDraggingindicators();
        this.isDragged = true;
      }
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
      this.DraggedXValue = this.GetSnapToPoint(this.DraggedXValue);
      this.DraggedValue = this.GetSnapToPoint(this.DraggedValue);
      object draggedActualXvalue = this.GetDraggedActualXValue(this.DraggedXValue);
      double ydata = this.draggingSegment.YData;
      object actualXvalue = this.GetActualXValue(this.SegmentIndex);
      bool flag = this.UpdateSource && !this.IsSortData;
      if (!this.IsIndexed && (this.DragDirection == DragType.X || this.DragDirection == DragType.XY))
      {
        if (this.ActualXValues is List<double> actualXvalues)
          actualXvalues[this.SegmentIndex] = this.DraggedXValue;
        if (flag)
          this.UpdateUnderLayingModel(this.XBindingPath, this.SegmentIndex, draggedActualXvalue);
      }
      if (this.DragDirection == DragType.Y || this.DragDirection == DragType.XY)
      {
        this.YValues[this.SegmentIndex] = this.DraggedValue;
        if (flag)
          this.UpdateUnderLayingModel(this.YBindingPath, this.SegmentIndex, (object) this.DraggedValue);
      }
      this.UpdateArea();
      ChartXyDragEndEventArgs args = new ChartXyDragEndEventArgs();
      args.BaseYValue = ydata;
      args.NewYValue = this.DraggedValue;
      args.BaseXValue = actualXvalue;
      args.NewXValue = this.IsIndexed ? actualXvalue : draggedActualXvalue;
      this.RaiseDragEnd((ChartDragEndEventArgs) args);
      this.ResetDraggingElements("CaptureReleased", false);
    }
    catch
    {
      this.ResetDraggingElements("CaptureReleased", true);
    }
  }

  private void ResetDraggingindicators()
  {
    if (this.AnimationElement == null || !this.SeriesPanel.Children.Contains((UIElement) this.AnimationElement))
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.AnimationElement);
    this.AnimationElement = (FrameworkElement) null;
  }
}
