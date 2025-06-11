// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Resizer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public sealed class Resizer : Control
{
  private Thumb resizeTopLeft;
  private Thumb resizeMiddleLeft;
  private Thumb resizeBottomLeft;
  private Thumb resizeTopMiddle;
  private Thumb resizeBottomMiddle;
  private Thumb resizeBottomRight;
  private Thumb resizeTopRight;
  private Thumb resizeMiddleRight;
  private SfChart chart;
  private bool isSwapY;
  private bool isSwapX;
  private bool isAxis;
  private bool isRotated;
  private double x1;
  private double y1;
  private double x2;
  private double y2;

  public Resizer() => this.DefaultStyleKey = (object) typeof (Resizer);

  internal AnnotationResizer AnnotationResizer { get; set; }

  private ChartAxis XAxis => this.AnnotationResizer.XAxis;

  private ChartAxis YAxis => this.AnnotationResizer.YAxis;

  private double ActualX1
  {
    get
    {
      return this.isAxis ? this.chart.ValueToPointRelativeToAnnotation(this.XAxis, Annotation.ConvertData(this.AnnotationResizer.X1, this.XAxis)) : Convert.ToDouble(this.AnnotationResizer.X1);
    }
    set => this.AnnotationResizer.X1 = (object) value;
  }

  private double ActualX2
  {
    get
    {
      return this.isAxis ? this.chart.ValueToPointRelativeToAnnotation(this.XAxis, Annotation.ConvertData(this.AnnotationResizer.X2, this.XAxis)) : Convert.ToDouble(this.AnnotationResizer.X2);
    }
    set => this.AnnotationResizer.X2 = (object) value;
  }

  private double ActualY1
  {
    get
    {
      return this.isAxis ? this.chart.ValueToPointRelativeToAnnotation(this.YAxis, Annotation.ConvertData(this.AnnotationResizer.Y1, this.YAxis)) : Convert.ToDouble(this.AnnotationResizer.Y1);
    }
    set => this.AnnotationResizer.Y1 = (object) value;
  }

  private double ActualY2
  {
    get
    {
      return this.isAxis ? this.chart.ValueToPointRelativeToAnnotation(this.YAxis, Annotation.ConvertData(this.AnnotationResizer.Y2, this.YAxis)) : Convert.ToDouble(this.AnnotationResizer.Y2);
    }
    set => this.AnnotationResizer.Y2 = (object) value;
  }

  internal void Dispose()
  {
    this.chart = (SfChart) null;
    this.AnnotationResizer = (AnnotationResizer) null;
  }

  internal void ChangeView()
  {
    Resizer resizerControl = this.AnnotationResizer.ResizerControl;
    if (resizerControl == null)
      return;
    if (this.AnnotationResizer.ResizingMode == AxisMode.Horizontal)
    {
      resizerControl.resizeBottomLeft.Visibility = Visibility.Collapsed;
      resizerControl.resizeBottomRight.Visibility = Visibility.Collapsed;
      resizerControl.resizeBottomMiddle.Visibility = Visibility.Collapsed;
      resizerControl.resizeMiddleLeft.Visibility = Visibility.Visible;
      resizerControl.resizeMiddleRight.Visibility = Visibility.Visible;
      resizerControl.resizeTopLeft.Visibility = Visibility.Collapsed;
      resizerControl.resizeTopMiddle.Visibility = Visibility.Collapsed;
      resizerControl.resizeTopRight.Visibility = Visibility.Collapsed;
    }
    else if (this.AnnotationResizer.ResizingMode == AxisMode.Vertical)
    {
      resizerControl.resizeBottomLeft.Visibility = Visibility.Collapsed;
      resizerControl.resizeBottomRight.Visibility = Visibility.Collapsed;
      resizerControl.resizeBottomMiddle.Visibility = Visibility.Visible;
      resizerControl.resizeMiddleLeft.Visibility = Visibility.Collapsed;
      resizerControl.resizeMiddleRight.Visibility = Visibility.Collapsed;
      resizerControl.resizeTopLeft.Visibility = Visibility.Collapsed;
      resizerControl.resizeTopMiddle.Visibility = Visibility.Visible;
      resizerControl.resizeTopRight.Visibility = Visibility.Collapsed;
    }
    else
    {
      resizerControl.resizeBottomLeft.Visibility = Visibility.Visible;
      resizerControl.resizeBottomRight.Visibility = Visibility.Visible;
      resizerControl.resizeBottomMiddle.Visibility = Visibility.Visible;
      resizerControl.resizeMiddleLeft.Visibility = Visibility.Visible;
      resizerControl.resizeMiddleRight.Visibility = Visibility.Visible;
      resizerControl.resizeTopLeft.Visibility = Visibility.Visible;
      resizerControl.resizeTopMiddle.Visibility = Visibility.Visible;
      resizerControl.resizeTopRight.Visibility = Visibility.Visible;
    }
  }

  internal void MapActualValueToPixels()
  {
    if (this.ActualX1 < this.ActualX2)
    {
      this.x1 = this.ActualX1;
      this.x2 = this.ActualX2;
    }
    else
    {
      this.x1 = this.ActualX2;
      this.x2 = this.ActualX1;
    }
    if (this.ActualY1 < this.ActualY2)
    {
      this.y1 = this.ActualY1;
      this.y2 = this.ActualY2;
    }
    else
    {
      this.y1 = this.ActualY2;
      this.y2 = this.ActualY1;
    }
  }

  public override void OnApplyTemplate()
  {
    this.isAxis = this.AnnotationResizer.CoordinateUnit == CoordinateUnit.Axis;
    this.isRotated = this.isAxis && this.XAxis.Orientation != Orientation.Horizontal;
    this.resizeTopLeft = this.GetTemplateChild("resizeTopLeft") as Thumb;
    this.resizeMiddleLeft = this.GetTemplateChild("resizeMiddleRight") as Thumb;
    this.resizeBottomLeft = this.GetTemplateChild("resizeBottomLeft") as Thumb;
    this.resizeTopMiddle = this.GetTemplateChild("resizeTopMiddle") as Thumb;
    this.resizeBottomMiddle = this.GetTemplateChild("resizeBottomMiddle") as Thumb;
    this.resizeTopRight = this.GetTemplateChild("resizeTopRight") as Thumb;
    this.resizeMiddleRight = this.GetTemplateChild("resizeMiddleLeft") as Thumb;
    this.resizeBottomRight = this.GetTemplateChild("resizeBottomRight") as Thumb;
    this.resizeBottomLeft.DragDelta += new DragDeltaEventHandler(this.ResizeBottomLeft_DragDelta);
    this.resizeBottomMiddle.DragDelta += new DragDeltaEventHandler(this.ResizeBottomMiddle_DragDelta);
    this.resizeBottomRight.DragDelta += new DragDeltaEventHandler(this.ResizeBottomRight_DragDelta);
    this.resizeMiddleLeft.DragDelta += new DragDeltaEventHandler(this.ResizeMiddleLeft_DragDelta);
    this.resizeMiddleRight.DragDelta += new DragDeltaEventHandler(this.ResizeMiddleRight_DragDelta);
    this.resizeTopLeft.DragDelta += new DragDeltaEventHandler(this.ResizeTopLeft_DragDelta);
    this.resizeTopMiddle.DragDelta += new DragDeltaEventHandler(this.ResizeTopMiddle_DragDelta);
    this.resizeTopRight.DragDelta += new DragDeltaEventHandler(this.ResizeTopRight_DragDelta);
    this.resizeBottomLeft.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeTopRight.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeTopMiddle.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeTopLeft.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeMiddleRight.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeMiddleLeft.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeBottomMiddle.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.resizeBottomRight.DragCompleted += new DragCompletedEventHandler(this.OnDragCompleted);
    this.chart = this.AnnotationResizer.Chart;
    this.CheckCoordinateValue();
    this.MapActualValueToPixels();
    this.ChangeView();
  }

  private void OnDragCompleted(object sender, DragCompletedEventArgs e)
  {
    this.AnnotationResizer.IsResizing = false;
    this.chart.AnnotationManager.RaiseDragCompleted();
  }

  private void CheckCoordinateValue()
  {
    if (this.ActualX1 > this.ActualX2)
      this.isSwapX = true;
    if (this.ActualY1 <= this.ActualY2)
      return;
    this.isSwapY = true;
  }

  private void Move(double horizontalChange, double verticalChange, bool isLeft, bool isTop)
  {
    if (this.isRotated)
    {
      double num = horizontalChange;
      horizontalChange = verticalChange;
      verticalChange = num;
    }
    this.x1 = isLeft ? (this.x1 + horizontalChange < this.x2 ? this.x1 + horizontalChange : this.x1) : this.x1;
    this.x2 = !isLeft ? (this.x2 + horizontalChange > this.x1 ? this.x2 + horizontalChange : this.x2) : this.x2;
    this.y1 = isTop ? (this.y1 + verticalChange < this.y2 ? this.y1 + verticalChange : this.y1) : this.y1;
    this.y2 = !isTop ? (this.y2 + verticalChange > this.y1 ? this.y2 + verticalChange : this.y2) : this.y2;
    this.MapPixelToActualValue();
  }

  private void ResizeTopRight_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, e.VerticalChange, this.isRotated, !this.isRotated);
  }

  private void MapPixelToActualValue()
  {
    double x1;
    double x2;
    if (this.isSwapX)
    {
      x1 = this.isAxis ? SfChart.PointToAnnotationValue(this.XAxis, this.isRotated ? new Point(0.0, this.x2) : new Point(this.x2, 0.0)) : this.x2;
      x2 = this.isAxis ? SfChart.PointToAnnotationValue(this.XAxis, this.isRotated ? new Point(0.0, this.x1) : new Point(this.x1, 0.0)) : this.x1;
    }
    else
    {
      x1 = this.isAxis ? SfChart.PointToAnnotationValue(this.XAxis, this.isRotated ? new Point(0.0, this.x1) : new Point(this.x1, 0.0)) : this.x1;
      x2 = this.isAxis ? SfChart.PointToAnnotationValue(this.XAxis, this.isRotated ? new Point(0.0, this.x2) : new Point(this.x2, 0.0)) : this.x2;
    }
    double y2;
    double y1;
    if (this.isSwapY)
    {
      y2 = this.isAxis ? SfChart.PointToAnnotationValue(this.YAxis, this.isRotated ? new Point(this.y1, 0.0) : new Point(0.0, this.y1)) : this.y1;
      y1 = this.isAxis ? SfChart.PointToAnnotationValue(this.YAxis, this.isRotated ? new Point(this.y2, 0.0) : new Point(0.0, this.y2)) : this.y2;
    }
    else
    {
      y2 = this.isAxis ? SfChart.PointToAnnotationValue(this.YAxis, this.isRotated ? new Point(this.y2, 0.0) : new Point(0.0, this.y2)) : this.y2;
      y1 = this.isAxis ? SfChart.PointToAnnotationValue(this.YAxis, this.isRotated ? new Point(this.y1, 0.0) : new Point(0.0, this.y1)) : this.y1;
    }
    AnnotationManager annotationManager = this.chart.AnnotationManager;
    ShapeAnnotation currentAnnotation = annotationManager.CurrentAnnotation as ShapeAnnotation;
    AnnotationManager.SetPosition(annotationManager.PreviousPoints, currentAnnotation.X1, currentAnnotation.X2, currentAnnotation.Y1, currentAnnotation.Y2);
    AnnotationManager.SetPosition(annotationManager.CurrentPoints, (object) x1, (object) x2, (object) y1, (object) y2);
    annotationManager.RaiseDragStarted();
    annotationManager.RaiseDragDelta();
    if (annotationManager.DragDeltaArgs.Cancel)
      return;
    this.ActualX1 = x1;
    this.ActualY1 = y1;
    this.ActualX2 = x2;
    this.ActualY2 = y2;
  }

  private void ResizeTopMiddle_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(0.0, e.VerticalChange, true, true);
  }

  private void ResizeTopLeft_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, e.VerticalChange, true, true);
  }

  private void ResizeMiddleRight_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, 0.0, false, false);
  }

  private void ResizeMiddleLeft_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, 0.0, true, true);
  }

  private void ResizeBottomRight_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, e.VerticalChange, false, false);
  }

  private void ResizeBottomMiddle_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(0.0, e.VerticalChange, false, false);
  }

  private void ResizeBottomLeft_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.AnnotationResizer.IsResizing = true;
    this.Move(e.HorizontalChange, e.VerticalChange, !this.isRotated, this.isRotated);
  }
}
