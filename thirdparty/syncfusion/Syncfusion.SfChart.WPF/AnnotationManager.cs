// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AnnotationManager
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class AnnotationManager : DependencyObject
{
  private Annotation previouseSelectedAnnotation;
  private DataTemplate contentTemplate;
  private double toolTipDuration;
  private DataTemplate defaultToolTipTemplate;
  private Annotation selectedAnnotation;
  private DispatcherTimer timer;
  private AnnotationDragCompletedEventArgs dragCompletedArgs = new AnnotationDragCompletedEventArgs();
  private Annotation previousAnnotation;
  private AnnotationCollection annotations;
  private SfChart chart;
  private Point currentPoint;
  private Point previousPoint;
  private bool isButtonPressed;

  public AnnotationManager()
  {
    this.PreviousPoints = new Position();
    this.CurrentPoints = new Position();
    this.DragDeltaArgs = new AnnotationDragDeltaEventArgs();
  }

  public Annotation SelectedAnnotation
  {
    get => this.selectedAnnotation;
    set
    {
      if (this.selectedAnnotation == value)
        return;
      this.selectedAnnotation = value;
      this.OnSelectionChanged();
    }
  }

  internal AnnotationCollection Annotations
  {
    get => this.annotations;
    set
    {
      if (this.annotations != value)
      {
        if (this.annotations != null)
        {
          this.annotations.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
          this.SelectedAnnotation = (Annotation) null;
          foreach (Annotation annotation in (Collection<Annotation>) this.annotations)
            this.AddOrRemoveAnnotations(annotation, true);
        }
        this.annotations = value;
        if (this.annotations != null)
        {
          this.annotations.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnAnnotationsCollectionChanged);
          this.AddAnnotations();
        }
      }
      if (this.chart == null)
        return;
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.chart.Axes)
      {
        ax.AxisBoundsChanged -= new EventHandler<ChartAxisBoundsEventArgs>(this.OnAxisBoundsChanged);
        ax.VisibleRangeChanged -= new EventHandler<VisibleRangeChangedEventArgs>(this.OnAxisVisibleRangeChanged);
      }
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.chart.Axes)
      {
        ax.AxisBoundsChanged += new EventHandler<ChartAxisBoundsEventArgs>(this.OnAxisBoundsChanged);
        ax.VisibleRangeChanged += new EventHandler<VisibleRangeChangedEventArgs>(this.OnAxisVisibleRangeChanged);
      }
    }
  }

  internal SfChart Chart
  {
    get => this.chart;
    set
    {
      if (this.chart != null)
      {
        this.chart.SeriesBoundsChanged -= new EventHandler<ChartSeriesBoundsEventArgs>(this.OnSeriesBoundsChanged);
        this.chart.SizeChanged -= new SizeChangedEventHandler(this.OnChartSizeChanged);
        this.chart.Axes.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnAxesCollectionChanged);
        if (this.chart.ChartAnnotationCanvas.Children.Contains((UIElement) this.Tooltip))
          this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) this.Tooltip);
        this.chart.MouseLeftButtonDown -= new MouseButtonEventHandler(this.OnMouseLeftButtonDown);
        this.chart.MouseLeftButtonUp -= new MouseButtonEventHandler(this.OnMouseLeftButtonUp);
        this.chart.MouseLeave -= new MouseEventHandler(this.OnMouseLeave);
        this.chart.MouseMove -= new MouseEventHandler(this.OnMouseMove);
        this.chart.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(this.OnManipulationDelta);
      }
      this.chart = value;
      if (this.chart == null)
        return;
      this.Tooltip = new ChartTooltip(true);
      this.Tooltip.IsHitTestVisible = false;
      this.chart.ChartAnnotationCanvas.Children.Add((UIElement) this.Tooltip);
      this.chart.SeriesBoundsChanged += new EventHandler<ChartSeriesBoundsEventArgs>(this.OnSeriesBoundsChanged);
      this.chart.SizeChanged += new SizeChangedEventHandler(this.OnChartSizeChanged);
      this.chart.Axes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnAxesCollectionChanged);
      this.chart.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnMouseLeftButtonDown);
      this.chart.MouseLeftButtonUp += new MouseButtonEventHandler(this.OnMouseLeftButtonUp);
      this.chart.MouseLeave += new MouseEventHandler(this.OnMouseLeave);
      this.chart.MouseMove += new MouseEventHandler(this.OnMouseMove);
      this.chart.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.OnManipulationDelta);
    }
  }

  internal TextBox TextBox { get; set; }

  internal Annotation TextAnnotation { get; set; }

  internal bool IsEditing { get; set; }

  internal ContentControl EditAnnotation { get; set; }

  internal ChartTooltip Tooltip { get; set; }

  internal bool IsDragStarted { get; set; }

  internal Annotation CurrentAnnotation { get; set; }

  internal AnnotationResizer AnnotationResizer { get; set; }

  internal Position PreviousPoints { get; set; }

  internal Position CurrentPoints { get; set; }

  internal AnnotationDragDeltaEventArgs DragDeltaArgs { get; set; }

  internal static void SetPosition(Position position, object x1, object x2, object y1, object y2)
  {
    if (position == null)
      return;
    position.X1 = x1 != null ? x1 : (object) 0;
    position.X2 = x2 != null ? x2 : (object) 0;
    position.Y1 = y1 != null ? y1 : (object) 0;
    position.Y2 = y2 != null ? y2 : (object) 0;
  }

  internal void HideLineResizer()
  {
    (this.previouseSelectedAnnotation as LineAnnotation).nearThumb.Visibility = Visibility.Collapsed;
    (this.previouseSelectedAnnotation as LineAnnotation).farThumb.Visibility = Visibility.Collapsed;
  }

  internal void OnAnnotationSelected()
  {
    if (this.SelectedAnnotation is SolidShapeAnnotation && (this.SelectedAnnotation as ShapeAnnotation).CanResize)
    {
      this.AnnotationResizer = new AnnotationResizer();
      this.AnnotationResizer.X1 = this.SelectedAnnotation.X1;
      this.AnnotationResizer.Y1 = this.SelectedAnnotation.Y1;
      this.AnnotationResizer.X2 = (this.SelectedAnnotation as ShapeAnnotation).X2;
      this.AnnotationResizer.Y2 = (this.SelectedAnnotation as ShapeAnnotation).Y2;
      this.AnnotationResizer.XAxisName = this.SelectedAnnotation.XAxisName;
      this.AnnotationResizer.YAxisName = this.SelectedAnnotation.YAxisName;
      this.AnnotationResizer.XAxis = this.selectedAnnotation.XAxis;
      this.AnnotationResizer.YAxis = this.selectedAnnotation.YAxis;
      this.AnnotationResizer.CoordinateUnit = this.SelectedAnnotation.CoordinateUnit;
      this.AnnotationResizer.Angle = (this.SelectedAnnotation as SolidShapeAnnotation).Angle;
      this.AnnotationResizer.InternalHorizontalAlignment = this.SelectedAnnotation.InternalHorizontalAlignment;
      this.AnnotationResizer.InternalVerticalAlignment = this.SelectedAnnotation.InternalVerticalAlignment;
      if (this.SelectedAnnotation is SolidShapeAnnotation)
        this.AnnotationResizer.ResizingMode = (this.SelectedAnnotation as SolidShapeAnnotation).ResizingMode;
      this.AddOrRemoveAnnotationResizer((Annotation) this.AnnotationResizer, false);
    }
    else
    {
      if (!(this.SelectedAnnotation is LineAnnotation) || !(this.SelectedAnnotation as LineAnnotation).CanResize)
        return;
      if ((this.SelectedAnnotation as LineAnnotation).nearThumb == null)
        (this.selectedAnnotation as LineAnnotation).AddThumb();
      (this.SelectedAnnotation as LineAnnotation).nearThumb.Visibility = Visibility.Visible;
      (this.SelectedAnnotation as LineAnnotation).farThumb.Visibility = Visibility.Visible;
      (this.SelectedAnnotation as LineAnnotation).UpdateAnnotation();
    }
  }

  internal void OnTextEditing()
  {
    if (!this.IsEditing)
      return;
    this.contentTemplate = ChartDictionaries.GenericCommonDictionary[(object) "textBlockAnnotation"] as DataTemplate;
    TextBlock textBlock = this.contentTemplate.LoadContent() as TextBlock;
    this.EditAnnotation.SetValue(ContentControl.ContentProperty, (object) textBlock);
    Binding binding = new Binding()
    {
      Path = new PropertyPath("Text", new object[0]),
      Source = (object) this.TextBox
    };
    textBlock.SetBinding(TextBlock.TextProperty, (BindingBase) binding);
    this.TextAnnotation.Text = textBlock.Text;
    this.IsEditing = false;
  }

  internal void RaiseDragStarted()
  {
    if (this.IsDragStarted || this.CurrentPoints.X1.Equals(this.PreviousPoints.X1) && this.CurrentPoints.X2.Equals(this.PreviousPoints.X2) && this.CurrentPoints.Y1.Equals(this.PreviousPoints.Y1) && this.CurrentPoints.Y2.Equals(this.PreviousPoints.Y2))
      return;
    (this.selectedAnnotation as ShapeAnnotation).OnDragStarted(new EventArgs());
    this.IsDragStarted = true;
  }

  internal void RaiseDragDelta()
  {
    ShapeAnnotation selectedAnnotation = this.SelectedAnnotation as ShapeAnnotation;
    if (this.CurrentPoints.X1.Equals(this.PreviousPoints.X1) && this.CurrentPoints.X2.Equals(this.PreviousPoints.X2) && this.CurrentPoints.Y1.Equals(this.PreviousPoints.Y1) && this.CurrentPoints.Y2.Equals(this.PreviousPoints.Y2))
      return;
    this.DragDeltaArgs.NewValue = this.CurrentPoints;
    this.DragDeltaArgs.OldValue = this.PreviousPoints;
    this.DragDeltaArgs.Cancel = false;
    selectedAnnotation.OnDragDelta(this.DragDeltaArgs);
  }

  internal void RaiseDragCompleted()
  {
    if (!this.IsDragStarted)
      return;
    this.dragCompletedArgs.NewValue = this.CurrentPoints;
    (this.SelectedAnnotation as ShapeAnnotation).OnDragCompleted(this.dragCompletedArgs);
    this.IsDragStarted = false;
  }

  internal void AddOrRemoveAnnotationResizer(Annotation annotation, bool isRemoval)
  {
    annotation.Chart = this.chart;
    UIElement element = !isRemoval ? annotation.CreateAnnotation() : annotation.GetRenderedAnnotation();
    if (element == null)
      return;
    switch (annotation.CoordinateUnit)
    {
      case CoordinateUnit.Pixel:
        if (isRemoval)
        {
          if (this.chart.ChartAnnotationCanvas.Children.Contains((UIElement) annotation))
            this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) annotation);
          Grid renderedAnnotation = this.previouseSelectedAnnotation.GetRenderedAnnotation() as Grid;
          if (!renderedAnnotation.Children.Contains(element))
            break;
          renderedAnnotation.Children.Remove(element);
          break;
        }
        Grid renderedAnnotation1 = this.SelectedAnnotation.GetRenderedAnnotation() as Grid;
        this.chart.ChartAnnotationCanvas.Children.Add((UIElement) annotation);
        renderedAnnotation1.Children.Add(element);
        annotation.UpdateAnnotation();
        break;
      case CoordinateUnit.Axis:
        if (isRemoval)
        {
          if (this.chart.SeriesAnnotationCanvas.Children.Contains((UIElement) annotation))
          {
            this.chart.SeriesAnnotationCanvas.Children.Remove((UIElement) annotation);
            this.RemoveAxisMarker(annotation);
          }
          Grid renderedAnnotation2 = this.previouseSelectedAnnotation.GetRenderedAnnotation() as Grid;
          if (!renderedAnnotation2.Children.Contains(element))
            break;
          renderedAnnotation2.Children.Remove(element);
          break;
        }
        Grid renderedAnnotation3 = this.SelectedAnnotation.GetRenderedAnnotation() as Grid;
        this.chart.SeriesAnnotationCanvas.Children.Add((UIElement) annotation);
        renderedAnnotation3.Children.Add(element);
        annotation.UpdateAnnotation();
        break;
    }
  }

  internal void AddOrRemoveAnnotations(Annotation annotation, bool isRemoval)
  {
    annotation.Chart = this.chart;
    UIElement uiElement = !annotation.IsVisbilityChanged ? (!isRemoval ? annotation.CreateAnnotation() : annotation.GetRenderedAnnotation()) : annotation.GetRenderedAnnotation();
    AxisMarker axisMarker = annotation as AxisMarker;
    if (uiElement != null && axisMarker == null)
    {
      switch (annotation.CoordinateUnit)
      {
        case CoordinateUnit.Pixel:
          if (this.chart.ChartAnnotationCanvas.Children.Contains(uiElement) && isRemoval)
          {
            this.RemoveChartAnnotation(annotation, uiElement);
            break;
          }
          if (annotation.Visibility == Visibility.Visible)
          {
            if (this.chart.SeriesAnnotationCanvas.Children.Contains(uiElement))
              this.RemoveSeriesAnnotation(annotation, uiElement);
            if (!this.chart.ChartAnnotationCanvas.Children.Contains((UIElement) annotation))
              this.chart.ChartAnnotationCanvas.Children.Add((UIElement) annotation);
            this.chart.ChartAnnotationCanvas.Children.Add(uiElement);
            annotation.UpdateAnnotation();
            break;
          }
          break;
        case CoordinateUnit.Axis:
          if (this.chart.SeriesAnnotationCanvas.Children.Contains(uiElement) && isRemoval)
          {
            this.RemoveSeriesAnnotation(annotation, uiElement);
            break;
          }
          if (annotation.Visibility == Visibility.Visible)
          {
            if (this.chart.ChartAnnotationCanvas.Children.Contains(uiElement))
              this.RemoveChartAnnotation(annotation, uiElement);
            if (!this.chart.SeriesAnnotationCanvas.Children.Contains((UIElement) annotation))
              this.chart.SeriesAnnotationCanvas.Children.Add((UIElement) annotation);
            this.chart.SeriesAnnotationCanvas.Children.Add(uiElement);
            annotation.UpdateAnnotation();
            break;
          }
          break;
      }
    }
    else if (axisMarker != null)
    {
      if (this.chart.ChartAnnotationCanvas.Children.Contains((UIElement) axisMarker.MarkerCanvas) && isRemoval)
        this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) axisMarker.MarkerCanvas);
      else if (axisMarker.ParentAnnotation.Visibility != Visibility.Collapsed)
      {
        this.chart.ChartAnnotationCanvas.Children.Add(uiElement);
        annotation.UpdateAnnotation();
      }
    }
    annotation.IsVisbilityChanged = false;
  }

  internal void Dispose()
  {
    if (this.chart != null)
    {
      foreach (ChartAxis ax in (Collection<ChartAxis>) this.chart.Axes)
      {
        ax.AxisBoundsChanged -= new EventHandler<ChartAxisBoundsEventArgs>(this.OnAxisBoundsChanged);
        ax.VisibleRangeChanged -= new EventHandler<VisibleRangeChangedEventArgs>(this.OnAxisVisibleRangeChanged);
      }
    }
    this.Chart = (SfChart) null;
    this.previouseSelectedAnnotation = (Annotation) null;
    this.selectedAnnotation = (Annotation) null;
    this.previousAnnotation = (Annotation) null;
    this.TextAnnotation = (Annotation) null;
    this.EditAnnotation = (ContentControl) null;
    this.CurrentAnnotation = (Annotation) null;
    this.AnnotationResizer = (AnnotationResizer) null;
    if (this.Annotations == null)
      return;
    this.Annotations.Clear();
    this.Annotations = (AnnotationCollection) null;
  }

  private static bool CheckPointInsideAnnotation(Annotation annotation, Point currentPosition)
  {
    return !(annotation is LineAnnotation lineAnnotation) ? annotation.RotatedRect.Contains(currentPosition) : lineAnnotation.IsPointInsideRectangle(currentPosition);
  }

  private void OnSelectionChanged()
  {
    if (this.SelectedAnnotation == null && this.AnnotationResizer != null || this.AnnotationResizer != null || this.previouseSelectedAnnotation is LineAnnotation && (this.previouseSelectedAnnotation as LineAnnotation).CanResize)
    {
      if (this.previouseSelectedAnnotation is LineAnnotation)
      {
        this.HideLineResizer();
        this.previouseSelectedAnnotation = (Annotation) null;
      }
      else
      {
        this.AddOrRemoveAnnotationResizer((Annotation) this.AnnotationResizer, true);
        this.AnnotationResizer = (AnnotationResizer) null;
      }
    }
    if (this.SelectedAnnotation == null)
      return;
    this.OnAnnotationSelected();
    this.previouseSelectedAnnotation = this.SelectedAnnotation;
  }

  private bool OnEnableDrag(Point point)
  {
    if (this.SelectedAnnotation != null)
    {
      if (this.selectedAnnotation.CoordinateUnit != CoordinateUnit.Axis || !this.selectedAnnotation.EnableClipping)
        return true;
      double left = this.Chart.SeriesClipRect.Left;
      double top = this.Chart.SeriesClipRect.Top;
      double num1 = this.Chart.PointToValue(this.selectedAnnotation.XAxis, new Point(point.X - left, point.Y - top));
      double num2 = this.Chart.PointToValue(this.selectedAnnotation.YAxis, new Point(point.X - left, point.Y - top));
      if (this.selectedAnnotation.XAxis.VisibleRange.Inside(num1) && this.selectedAnnotation.YAxis.VisibleRange.Inside(num2))
        return true;
    }
    return false;
  }

  private void OnMouseLeave(object sender, MouseEventArgs e) => this.isButtonPressed = false;

  private void OnMouseMove(object sender, MouseEventArgs e)
  {
    this.ShowToolTip(e.GetPosition((IInputElement) this.Chart.ChartAnnotationCanvas), e.OriginalSource);
    this.currentPoint = e.GetPosition((IInputElement) this.Chart.ChartAnnotationCanvas);
    TranslateTransform translateTransform = ChartMath.Translate(this.previousPoint, this.currentPoint);
    if (this.SelectedAnnotation != null && this.isButtonPressed && this.OnEnableDrag(this.currentPoint))
    {
      if (this.SelectedAnnotation.XAxis != null && this.SelectedAnnotation.XAxis.Orientation == Orientation.Vertical && this.SelectedAnnotation.CoordinateUnit == CoordinateUnit.Axis)
        this.AnnotationDrag(-translateTransform.Y, -translateTransform.X);
      else
        this.AnnotationDrag(translateTransform.X, translateTransform.Y);
    }
    this.previousPoint = this.currentPoint;
  }

  private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
  {
    if (this.SelectedAnnotation == null)
      return;
    if (this.SelectedAnnotation.XAxis != null && this.SelectedAnnotation.XAxis.Orientation == Orientation.Vertical && this.SelectedAnnotation.CoordinateUnit == CoordinateUnit.Axis)
      this.AnnotationDrag(-e.DeltaManipulation.Translation.Y, -e.DeltaManipulation.Translation.X);
    else
      this.AnnotationDrag(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
  }

  private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.MouseUp();
    this.isButtonPressed = false;
  }

  private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.OnTextEditing();
    this.MouseDown(e.GetPosition((IInputElement) this.Chart.ChartAnnotationCanvas), e.GetPosition((IInputElement) this.Chart.SeriesAnnotationCanvas));
    this.isButtonPressed = true;
  }

  private void MouseDown(Point pixelPos, Point axisPos)
  {
    Annotation annotation = (Annotation) null;
    this.CurrentAnnotation = (Annotation) null;
    this.IsDragStarted = false;
    List<Annotation> list1 = this.Annotations.Where<Annotation>((Func<Annotation, bool>) (axis => axis.CoordinateUnit == CoordinateUnit.Axis)).ToList<Annotation>();
    if (list1.Any<Annotation>())
      annotation = this.GetSelectedAnnotation((IEnumerable<Annotation>) list1, axisPos);
    List<Annotation> list2 = this.Annotations.Where<Annotation>((Func<Annotation, bool>) (pixel => pixel.CoordinateUnit == CoordinateUnit.Pixel)).ToList<Annotation>();
    if (list2.Any<Annotation>() && annotation == null)
      annotation = this.GetSelectedAnnotation((IEnumerable<Annotation>) list2, pixelPos);
    this.SelectedAnnotation = annotation;
    this.RaiseSelectionChanged();
    this.UnHoldPanning(false);
  }

  private void UnHoldPanning(bool value)
  {
    if (!(this.SelectedAnnotation is ShapeAnnotation))
      return;
    ShapeAnnotation selectedAnnotation = this.SelectedAnnotation as ShapeAnnotation;
    if (!selectedAnnotation.CanDrag && !selectedAnnotation.CanResize)
      return;
    foreach (ChartZoomPanBehavior chartZoomPanBehavior in this.chart.Behaviors.OfType<ChartZoomPanBehavior>())
    {
      chartZoomPanBehavior.InternalEnablePanning = value;
      chartZoomPanBehavior.InternalEnableSelectionZooming = value;
    }
  }

  private void MouseUp()
  {
    if (this.AnnotationResizer != null)
      this.AnnotationResizer.IsResizing = false;
    else if (this.SelectedAnnotation is LineAnnotation)
      (this.SelectedAnnotation as LineAnnotation).IsResizing = false;
    this.UnHoldPanning(true);
    if (!(this.SelectedAnnotation is ShapeAnnotation))
      return;
    this.RaiseDragCompleted();
  }

  private void RaiseSelectionChanged()
  {
    if (this.CurrentAnnotation == this.previousAnnotation)
      return;
    if (this.previousAnnotation != null && this.CurrentAnnotation != this.previousAnnotation)
      this.previousAnnotation.OnUnSelected(new EventArgs());
    if (this.CurrentAnnotation != null && this.CurrentAnnotation != this.previousAnnotation)
      this.CurrentAnnotation.OnSelected(new EventArgs());
    this.previousAnnotation = this.CurrentAnnotation;
  }

  private void ShowToolTip(Point currentPoint, object source)
  {
    if (!(source is FrameworkElement reference))
      return;
    Annotation annotation = (Annotation) null;
    if (source is Shape)
      annotation = (source as Shape).Tag as Annotation;
    else if (source is Image)
      annotation = (source as Image).Tag as Annotation;
    else if (this.CheckBounds(currentPoint) != null)
    {
      for (FrameworkElement parent = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement; parent != null; parent = VisualTreeHelper.GetParent((DependencyObject) parent) as FrameworkElement)
      {
        if (parent.Tag is Annotation)
        {
          annotation = parent.Tag as Annotation;
          break;
        }
      }
    }
    if (annotation != null && annotation.ShowToolTip)
    {
      if (this.defaultToolTipTemplate == null)
        this.defaultToolTipTemplate = ChartDictionaries.GenericCommonDictionary[(object) "AnnotationTooltipTemplate"] as DataTemplate;
      this.Tooltip.Visibility = Visibility.Visible;
      this.toolTipDuration = annotation.ToolTipShowDuration;
      if (!double.IsNaN(this.toolTipDuration))
        this.ResetTimer();
      Point toolTipPosition = this.GetToolTipPosition(currentPoint, annotation);
      this.Tooltip.ContentTemplate = annotation.ToolTipTemplate == null ? this.defaultToolTipTemplate : annotation.ToolTipTemplate;
      this.Tooltip.Content = annotation.ToolTipContent;
      Canvas.SetLeft((UIElement) this.Tooltip, toolTipPosition.X);
      Canvas.SetTop((UIElement) this.Tooltip, toolTipPosition.Y);
      Panel.SetZIndex((UIElement) this.Tooltip, 1);
    }
    else
      this.Tooltip.Visibility = Visibility.Collapsed;
  }

  private object CheckBounds(Point currentPoint)
  {
    Annotation annotation1 = (Annotation) null;
    Annotation annotation2 = (Annotation) null;
    IEnumerable<Annotation> source1 = this.Annotations.Where<Annotation>((Func<Annotation, bool>) (axis => axis.CoordinateUnit == CoordinateUnit.Axis)).Where<Annotation>((Func<Annotation, bool>) (annotation => annotation is Syncfusion.UI.Xaml.Charts.TextAnnotation));
    if (source1.Count<Annotation>() > 0)
    {
      foreach (Annotation annotation3 in source1)
      {
        if (annotation3.RotatedRect.Contains(currentPoint))
          annotation1 = annotation3;
      }
    }
    IEnumerable<Annotation> source2 = this.Annotations.Where<Annotation>((Func<Annotation, bool>) (pixel => pixel.CoordinateUnit == CoordinateUnit.Pixel)).Where<Annotation>((Func<Annotation, bool>) (annotation => annotation is Syncfusion.UI.Xaml.Charts.TextAnnotation));
    if (source2.Count<Annotation>() > 0)
    {
      foreach (Annotation annotation4 in source2)
      {
        if (annotation4.RotatedRect.Contains(currentPoint))
          annotation2 = annotation4;
      }
    }
    return (object) annotation2 ?? (object) annotation1;
  }

  private void ResetTimer()
  {
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer.Interval = TimeSpan.FromMilliseconds(this.toolTipDuration);
      this.timer.Start();
    }
    else
    {
      this.timer = new DispatcherTimer();
      this.timer.Tick += new EventHandler(this.OnTimeout);
      this.timer.Start();
    }
  }

  private void OnTimeout(object sender, object e)
  {
    if (this.Tooltip == null)
      return;
    this.Tooltip.Visibility = Visibility.Collapsed;
  }

  private Point GetToolTipPosition(Point currentPosition, Annotation annotation)
  {
    this.Tooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    switch (annotation.ToolTipPlacement)
    {
      case ToolTipLabelPlacement.Left:
        currentPosition.X -= this.Tooltip.DesiredSize.Width;
        break;
      case ToolTipLabelPlacement.Right:
        this.Tooltip.Margin = new Thickness().GetThickness(10.0, 0.0, 0.0, 0.0);
        break;
      case ToolTipLabelPlacement.Top:
        currentPosition.Y -= this.Tooltip.DesiredSize.Height;
        break;
      case ToolTipLabelPlacement.Bottom:
        currentPosition.Y += this.Tooltip.DesiredSize.Height;
        break;
    }
    return currentPosition;
  }

  private Annotation GetSelectedAnnotation(
    IEnumerable<Annotation> annotations,
    Point currentPosition)
  {
    Annotation selectedAnnotation = (Annotation) null;
    foreach (Annotation annotation in annotations)
    {
      bool flag = AnnotationManager.CheckPointInsideAnnotation(annotation, currentPosition);
      annotation.IsSelected = flag && annotation is ShapeAnnotation;
      if (annotation.IsSelected)
        selectedAnnotation = annotation;
      else if (annotation is VerticalLineAnnotation && (annotation as VerticalLineAnnotation).AxisMarkerObject != null && (annotation as VerticalLineAnnotation).AxisMarkerObject.RotatedRect.Contains(currentPosition))
        selectedAnnotation = (Annotation) (annotation as VerticalLineAnnotation).AxisMarkerObject;
      else if (annotation is HorizontalLineAnnotation && (annotation as HorizontalLineAnnotation).AxisMarkerObject != null && (annotation as HorizontalLineAnnotation).AxisMarkerObject.RotatedRect.Contains(currentPosition))
        selectedAnnotation = (Annotation) (annotation as HorizontalLineAnnotation).AxisMarkerObject;
      if (selectedAnnotation == null && flag)
        this.CurrentAnnotation = annotation;
      else if (selectedAnnotation != null)
        this.CurrentAnnotation = selectedAnnotation;
    }
    return selectedAnnotation;
  }

  private void AnnotationDrag(double xTranslate, double yTranslate)
  {
    ShapeAnnotation selectedAnnotation = this.SelectedAnnotation as ShapeAnnotation;
    LineAnnotation lineAnnotation = selectedAnnotation as LineAnnotation;
    bool flag1 = !selectedAnnotation.CanResize || this.AnnotationResizer == null ? lineAnnotation == null || !lineAnnotation.IsResizing : !this.AnnotationResizer.IsResizing;
    if (!selectedAnnotation.CanDrag || !flag1)
      return;
    selectedAnnotation.IsDragging = true;
    bool flag2 = selectedAnnotation.DraggingMode == AxisMode.Horizontal;
    bool flag3 = selectedAnnotation.DraggingMode == AxisMode.Vertical;
    bool flag4 = selectedAnnotation.DraggingMode == AxisMode.All;
    object x1;
    object x2;
    object y1;
    object y2;
    if (selectedAnnotation.CoordinateUnit == CoordinateUnit.Pixel)
    {
      x1 = flag4 || flag2 ? (object) (Convert.ToDouble(selectedAnnotation.X1) + xTranslate) : selectedAnnotation.X1;
      x2 = flag4 || flag2 ? (object) (Convert.ToDouble(selectedAnnotation.X2) + xTranslate) : selectedAnnotation.X2;
      y1 = flag4 || flag3 ? (object) (Convert.ToDouble(selectedAnnotation.Y1) + yTranslate) : selectedAnnotation.Y1;
      y2 = flag4 || flag3 ? (object) (Convert.ToDouble(selectedAnnotation.Y2) + yTranslate) : selectedAnnotation.Y2;
    }
    else
    {
      xTranslate = selectedAnnotation.XAxis.IsInversed ? -xTranslate : xTranslate;
      yTranslate = selectedAnnotation.YAxis.IsInversed ? -yTranslate : yTranslate;
      double coefficientValue1 = selectedAnnotation.XAxis.PixelToCoefficientValue(xTranslate);
      double coefficientValue2 = selectedAnnotation.YAxis.PixelToCoefficientValue(yTranslate);
      if (selectedAnnotation.XAxis.Orientation == Orientation.Horizontal)
      {
        x2 = flag4 || flag2 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.X2, coefficientValue1, true, false), selectedAnnotation.XAxis) : selectedAnnotation.X2;
        x1 = flag4 || flag2 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.X1, coefficientValue1, true, false), selectedAnnotation.XAxis) : selectedAnnotation.X1;
        y2 = flag4 || flag3 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.Y2, coefficientValue2, false, false), selectedAnnotation.YAxis) : selectedAnnotation.Y2;
        y1 = flag4 || flag3 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.Y1, coefficientValue2, false, false), selectedAnnotation.YAxis) : selectedAnnotation.Y1;
      }
      else
      {
        x2 = flag4 || flag3 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.X2, -coefficientValue1, false, false), selectedAnnotation.XAxis) : selectedAnnotation.X2;
        x1 = flag4 || flag3 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.X1, -coefficientValue1, false, false), selectedAnnotation.XAxis) : selectedAnnotation.X1;
        y2 = flag4 || flag2 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.Y2, -coefficientValue2, true, false), selectedAnnotation.YAxis) : selectedAnnotation.Y2;
        y1 = flag4 || flag2 ? Annotation.ConvertToObject(this.CalculatePointValue(selectedAnnotation.Y1, -coefficientValue2, true, false), selectedAnnotation.YAxis) : selectedAnnotation.Y1;
      }
    }
    AnnotationManager.SetPosition(this.PreviousPoints, selectedAnnotation.X1, selectedAnnotation.X2, selectedAnnotation.Y1, selectedAnnotation.Y2);
    AnnotationManager.SetPosition(this.CurrentPoints, x1, x2, y1, y2);
    this.RaiseDragStarted();
    this.RaiseDragDelta();
    if (!this.DragDeltaArgs.Cancel)
    {
      if (this.AnnotationResizer != null && !this.AnnotationResizer.IsResizing)
      {
        selectedAnnotation.X1 = this.AnnotationResizer.X1 = x1;
        selectedAnnotation.X2 = this.AnnotationResizer.X2 = x2;
        selectedAnnotation.Y1 = this.AnnotationResizer.Y1 = y1;
        selectedAnnotation.Y2 = this.AnnotationResizer.Y2 = y2;
      }
      else
      {
        selectedAnnotation.X1 = x1;
        selectedAnnotation.X2 = x2;
        selectedAnnotation.Y1 = y1;
        selectedAnnotation.Y2 = y2;
      }
      if (selectedAnnotation is AxisMarker axisMarker)
      {
        if (axisMarker.ParentAnnotation is VerticalLineAnnotation)
        {
          if (axisMarker.ParentAnnotation.XAxis.Orientation == Orientation.Horizontal)
            axisMarker.ParentAnnotation.X1 = selectedAnnotation.X1;
          else
            axisMarker.ParentAnnotation.Y1 = selectedAnnotation.Y1;
        }
        else if (axisMarker.ParentAnnotation.XAxis.Orientation == Orientation.Vertical)
          axisMarker.ParentAnnotation.X1 = selectedAnnotation.X1;
        else
          axisMarker.ParentAnnotation.Y1 = selectedAnnotation.Y1;
      }
      if (this.AnnotationResizer != null)
        this.AnnotationResizer.MapActualValueToPixels();
    }
    selectedAnnotation.IsDragging = false;
  }

  private double CalculatePointValue(
    object value,
    double change,
    bool isXAxis,
    bool isAngleInPhone)
  {
    ShapeAnnotation selectedAnnotation = this.SelectedAnnotation is ShapeAnnotation ? this.SelectedAnnotation as ShapeAnnotation : (ShapeAnnotation) null;
    change = isAngleInPhone ? change * -1.0 : change;
    return isXAxis ? Annotation.ConvertData(value, selectedAnnotation.XAxis) + change : Annotation.ConvertData(value, selectedAnnotation.YAxis) - change;
  }

  private void OnAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
      annotation.SetAxisFromName();
  }

  private void OnChartSizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.chart.ChartAnnotationCanvas.ClipToBounds = true;
  }

  private void OnSeriesBoundsChanged(object sender, ChartSeriesBoundsEventArgs e)
  {
    this.chart.SeriesAnnotationCanvas.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = this.chart.SeriesClipRect
    };
  }

  private void OnAxisVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
  {
    ChartAxis chartAxis = sender as ChartAxis;
    if (this.Annotations != null)
    {
      foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
      {
        if (annotation.XAxis == chartAxis || annotation.YAxis == chartAxis)
          annotation.UpdateAnnotation();
      }
    }
    if (this.AnnotationResizer == null || this.AnnotationResizer.XAxis != chartAxis && this.AnnotationResizer.YAxis != chartAxis)
      return;
    this.AnnotationResizer.UpdateAnnotation();
    this.AnnotationResizer.MapActualValueToPixels();
  }

  private void OnAxisBoundsChanged(object sender, ChartAxisBoundsEventArgs e)
  {
    ChartAxis chartAxis = sender as ChartAxis;
    foreach (Annotation annotation in (Collection<Annotation>) this.Annotations)
    {
      if (annotation.XAxis == chartAxis || annotation.YAxis == chartAxis)
        annotation.UpdateAnnotation();
    }
    if (this.AnnotationResizer == null || this.AnnotationResizer.XAxis != chartAxis && this.AnnotationResizer.YAxis != chartAxis)
      return;
    this.AnnotationResizer.UpdateAnnotation();
    this.AnnotationResizer.MapActualValueToPixels();
  }

  private void AddAnnotations()
  {
    foreach (Annotation annotation1 in (Collection<Annotation>) this.Annotations)
    {
      UIElement annotation2 = annotation1.CreateAnnotation();
      annotation1.Chart = this.chart;
      if (annotation1.Visibility != Visibility.Collapsed)
      {
        if (annotation2 != null && !(annotation1 is AxisMarker))
        {
          if (annotation1.Parent is Panel)
            (annotation1.Parent as Panel).Children.Remove((UIElement) annotation1);
          if (annotation2 is FrameworkElement && (annotation2 as FrameworkElement).Parent is Panel)
            ((annotation2 as FrameworkElement).Parent as Panel).Children.Remove(annotation2);
          switch (annotation1.CoordinateUnit)
          {
            case CoordinateUnit.Pixel:
              this.chart.ChartAnnotationCanvas.Children.Add((UIElement) annotation1);
              if (annotation1.Visibility == Visibility.Visible)
              {
                this.chart.ChartAnnotationCanvas.Children.Add(annotation2);
                break;
              }
              break;
            case CoordinateUnit.Axis:
              this.chart.SeriesAnnotationCanvas.Children.Add((UIElement) annotation1);
              if (annotation1.Visibility == Visibility.Visible)
              {
                this.chart.SeriesAnnotationCanvas.Children.Add(annotation2);
                break;
              }
              break;
          }
        }
        else
          this.chart.ChartAnnotationCanvas.Children.Add(annotation2);
        annotation1.UpdateAnnotation();
      }
    }
  }

  private void RemoveChartAnnotation(Annotation annotation, UIElement annotationElement)
  {
    if (!annotation.IsVisbilityChanged)
      this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) annotation);
    this.chart.ChartAnnotationCanvas.Children.Remove(annotationElement);
  }

  private void RemoveSeriesAnnotation(Annotation annotation, UIElement annotationElement)
  {
    if (!annotation.IsVisbilityChanged)
      this.chart.SeriesAnnotationCanvas.Children.Remove((UIElement) annotation);
    this.RemoveAxisMarker(annotation);
    this.chart.SeriesAnnotationCanvas.Children.Remove(annotationElement);
  }

  private void RemoveAxisMarker(Annotation annotation)
  {
    switch (annotation)
    {
      case VerticalLineAnnotation _ when (annotation as VerticalLineAnnotation).ShowAxisLabel:
        this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) (annotation as VerticalLineAnnotation).AxisMarkerObject.MarkerCanvas);
        break;
      case HorizontalLineAnnotation _ when (annotation as HorizontalLineAnnotation).ShowAxisLabel:
        this.chart.ChartAnnotationCanvas.Children.Remove((UIElement) (annotation as HorizontalLineAnnotation).AxisMarkerObject.MarkerCanvas);
        break;
    }
  }

  private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.chart == null || this.chart.ChartAnnotationCanvas == null)
      return;
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        IEnumerator enumerator1 = e.NewItems.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
            this.AddOrRemoveAnnotations((Annotation) enumerator1.Current, false);
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Remove:
        IEnumerator enumerator2 = e.OldItems.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
          {
            Annotation current = (Annotation) enumerator2.Current;
            this.AddOrRemoveAnnotations(current, true);
            if (this.SelectedAnnotation == current)
              this.SelectedAnnotation = (Annotation) null;
          }
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Replace:
        foreach (Annotation oldItem in (IEnumerable) e.OldItems)
        {
          this.AddOrRemoveAnnotations(oldItem, true);
          if (this.SelectedAnnotation == oldItem)
            this.SelectedAnnotation = (Annotation) null;
        }
        IEnumerator enumerator3 = e.NewItems.GetEnumerator();
        try
        {
          while (enumerator3.MoveNext())
            this.AddOrRemoveAnnotations((Annotation) enumerator3.Current, false);
          break;
        }
        finally
        {
          if (enumerator3 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Reset:
        UIElementCollection children1 = this.chart.ChartAnnotationCanvas.Children;
        if (children1.Count > 0)
        {
          List<UIElement> uiElementList = new List<UIElement>();
          foreach (UIElement uiElement in children1)
            uiElementList.Add(uiElement);
          foreach (UIElement uiElement in uiElementList)
          {
            if (uiElement is Annotation annotation)
            {
              UIElement renderedAnnotation = annotation.GetRenderedAnnotation();
              if (this.chart.ChartAnnotationCanvas.Children.Contains(renderedAnnotation))
                this.RemoveChartAnnotation(annotation, renderedAnnotation);
            }
          }
        }
        UIElementCollection children2 = this.chart.SeriesAnnotationCanvas.Children;
        if (children2.Count > 0)
        {
          List<UIElement> uiElementList = new List<UIElement>();
          foreach (UIElement uiElement in children2)
            uiElementList.Add(uiElement);
          foreach (UIElement uiElement in uiElementList)
          {
            if (uiElement is Annotation annotation)
            {
              UIElement renderedAnnotation = annotation.GetRenderedAnnotation();
              if (this.chart.SeriesAnnotationCanvas.Children.Contains(renderedAnnotation))
                this.RemoveSeriesAnnotation(annotation, renderedAnnotation);
            }
          }
        }
        this.SelectedAnnotation = (Annotation) null;
        break;
    }
  }
}
