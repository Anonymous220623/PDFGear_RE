// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.VerticalLineAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class VerticalLineAnnotation : StraightLineAnnotation
{
  public new static readonly DependencyProperty HorizontalTextAlignmentProperty = DependencyProperty.Register(nameof (HorizontalTextAlignment), typeof (HorizontalAlignment), typeof (VerticalLineAnnotation), new PropertyMetadata((object) HorizontalAlignment.Right, new PropertyChangedCallback(VerticalLineAnnotation.OnTextAlignmentPropertyChanged)));
  public new static readonly DependencyProperty VerticalTextAlignmentProperty = DependencyProperty.Register(nameof (VerticalTextAlignment), typeof (VerticalAlignment), typeof (VerticalLineAnnotation), new PropertyMetadata((object) VerticalAlignment.Center, new PropertyChangedCallback(VerticalLineAnnotation.OnTextAlignmentPropertyChanged)));

  public new HorizontalAlignment HorizontalTextAlignment
  {
    get
    {
      return (HorizontalAlignment) this.GetValue(VerticalLineAnnotation.HorizontalTextAlignmentProperty);
    }
    set => this.SetValue(VerticalLineAnnotation.HorizontalTextAlignmentProperty, (object) value);
  }

  public new VerticalAlignment VerticalTextAlignment
  {
    get => (VerticalAlignment) this.GetValue(VerticalLineAnnotation.VerticalTextAlignmentProperty);
    set => this.SetValue(VerticalLineAnnotation.VerticalTextAlignmentProperty, (object) value);
  }

  public override void UpdateAnnotation()
  {
    if (this.shape == null)
      return;
    this.ValidateSelection();
    switch (this.CoordinateUnit)
    {
      case CoordinateUnit.Pixel:
        if (this.ShowLine && this.Chart != null && this.X1 != null)
        {
          this.DraggingMode = AxisMode.Horizontal;
          this.X2 = this.X1;
          this.Y1 = this.Y1 == null ? (object) 0 : this.Y1;
          if (this.Chart.DesiredSize.Height != 0.0)
            this.Y2 = this.Y2 == null ? (object) this.Chart.DesiredSize.Height : this.Y2;
          this.DrawLine(new Point(Convert.ToDouble(this.X1), Convert.ToDouble(this.Y1)), new Point(Convert.ToDouble(this.X2), Convert.ToDouble(this.Y2)), this.shape);
          break;
        }
        break;
      case CoordinateUnit.Axis:
        this.SetAxisFromName();
        if (this.XAxis != null && this.YAxis != null)
        {
          if (this.Chart.AnnotationManager != null && this.ShowAxisLabel && !this.Chart.ChartAnnotationCanvas.Children.Contains((UIElement) this.AxisMarkerObject.MarkerCanvas))
            this.Chart.AnnotationManager.AddOrRemoveAnnotations((Annotation) this.AxisMarkerObject, false);
          if (this.XAxis.Orientation == Orientation.Horizontal)
          {
            if (this.X1 != null)
            {
              this.y1 = this.Y1 == null ? this.YAxis.VisibleRange.Start : Annotation.ConvertData(this.Y1, this.YAxis);
              this.y2 = this.Y2 == null ? this.YAxis.VisibleRange.End : Annotation.ConvertData(this.Y2, this.YAxis);
              this.X2 = this.X1;
              this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
              this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
              if (this.ShowAxisLabel)
                this.SetAxisMarkerValue(this.X1, this.X2, (object) this.YAxis.VisibleRange.Start, (object) this.YAxis.VisibleRange.End, AxisMode.Horizontal);
              this.DraggingMode = AxisMode.Horizontal;
            }
            else
              break;
          }
          else if (this.X1 != null)
          {
            this.y1 = this.Y1 == null ? this.YAxis.VisibleRange.Start : Annotation.ConvertData(this.Y1, this.YAxis);
            this.y2 = this.Y2 == null ? this.YAxis.VisibleRange.End : Annotation.ConvertData(this.Y2, this.YAxis);
            this.X2 = this.X1;
            this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
            this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
            if (this.ShowAxisLabel)
              this.SetAxisMarkerValue(this.X1, this.X2, (object) this.YAxis.VisibleRange.Start, (object) this.YAxis.VisibleRange.End, AxisMode.Vertical);
            this.DraggingMode = AxisMode.Vertical;
          }
          else
            break;
          if (this.ShowAxisLabel)
            this.AxisMarkerObject.UpdateAnnotation();
          if (this.ShowLine)
          {
            if (this.CoordinateUnit == CoordinateUnit.Axis && this.EnableClipping)
            {
              this.x1 = this.GetClippingValues(this.x1, this.XAxis);
              this.y1 = this.GetClippingValues(this.y1, this.YAxis);
              this.x2 = this.GetClippingValues(this.x2, this.XAxis);
              this.y2 = this.GetClippingValues(this.y2, this.YAxis);
            }
            this.DrawLine(this.XAxis.Orientation.Equals((object) Orientation.Horizontal) ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1) + this.YAxis.GetActualPlotOffsetStart()) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1) + this.XAxis.GetActualPlotOffsetStart(), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1)), this.XAxis.Orientation.Equals((object) Orientation.Horizontal) ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2) - this.YAxis.GetActualPlotOffsetEnd()) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2) + this.XAxis.GetActualPlotOffsetEnd(), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2)), this.shape);
            break;
          }
          break;
        }
        break;
    }
    if (this.YAxis == null || this.YAxis.Orientation != Orientation.Horizontal || this.Y1 != null || this.X1 != null)
      return;
    this.ClearValues();
    if (!this.ShowAxisLabel)
      return;
    this.AxisMarkerObject.ClearValue(Annotation.X1Property);
    this.AxisMarkerObject.ClearValue(ShapeAnnotation.X2Property);
    this.AxisMarkerObject.ClearValue(Annotation.Y1Property);
    this.AxisMarkerObject.ClearValue(ShapeAnnotation.Y2Property);
  }

  internal override void UpdateHitArea()
  {
    if (this.LinePoints == null)
      return;
    Point linePoint1 = this.LinePoints[0];
    Point linePoint2 = this.LinePoints[1];
    Point point = this.EnsurePoint(linePoint1, linePoint2);
    double num = Math.Abs(linePoint2.X - linePoint1.X);
    double height = Math.Abs(linePoint2.Y - linePoint1.Y);
    this.RotatedRect = new Rect(point.X - this.GrabExtent, point.Y, num + 2.0 * this.GrabExtent, height);
  }

  protected override void SetTextElementPosition(
    Point point,
    Point point2,
    Size desiredSize,
    Point positionedPoint,
    ContentControl TextElement)
  {
    RotateTransform rotateTransform = new RotateTransform();
    TextElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    double d = (point2.Y - point.Y) / (point2.X - point.X);
    rotateTransform.Angle = double.IsNaN(d) ? 0.0 : Math.Atan(d) * (180.0 / Math.PI);
    TextElement.RenderTransformOrigin = new Point(0.0, 0.0);
    TextElement.RenderTransform = (Transform) rotateTransform;
    if (this.XAxis != null && this.XAxis.Orientation == Orientation.Horizontal)
      this.SetHorizontalPosition(point, point2);
    else
      this.SetVerticalPosition(point, point2);
  }

  private void SetHorizontalPosition(Point point, Point point2)
  {
    if (this.HorizontalTextAlignment == HorizontalAlignment.Left)
      Canvas.SetLeft((UIElement) this.TextElement, point.X - 30.0);
    else if (this.HorizontalTextAlignment == HorizontalAlignment.Center)
      Canvas.SetLeft((UIElement) this.TextElement, point.X - 10.0);
    else
      Canvas.SetLeft((UIElement) this.TextElement, point.X);
    if (this.VerticalTextAlignment == VerticalAlignment.Bottom)
      Canvas.SetTop((UIElement) this.TextElement, point.Y);
    else if (this.VerticalTextAlignment == VerticalAlignment.Top)
      Canvas.SetTop((UIElement) this.TextElement, point2.Y + this.TextElement.DesiredSize.Width);
    else
      Canvas.SetTop((UIElement) this.TextElement, (point.Y + point2.Y) / 2.0 + this.TextElement.DesiredSize.Width / 2.0);
  }

  private void SetVerticalPosition(Point point, Point point2)
  {
    if (this.HorizontalTextAlignment == HorizontalAlignment.Left)
      Canvas.SetLeft((UIElement) this.TextElement, this.TextElement.DesiredSize.Width);
    else if (this.HorizontalTextAlignment == HorizontalAlignment.Center)
      Canvas.SetLeft((UIElement) this.TextElement, (point.X + point2.X) / 2.0 + this.TextElement.DesiredSize.Width / 2.0);
    else
      Canvas.SetLeft((UIElement) this.TextElement, point.X - this.TextElement.DesiredSize.Width);
    if (this.VerticalTextAlignment == VerticalAlignment.Bottom)
      Canvas.SetTop((UIElement) this.TextElement, point.Y);
    else if (this.VerticalTextAlignment == VerticalAlignment.Top)
      Canvas.SetTop((UIElement) this.TextElement, point2.Y - this.TextElement.DesiredSize.Height);
    else
      Canvas.SetTop((UIElement) this.TextElement, (point.Y + point2.Y) / 2.0 - this.TextElement.DesiredSize.Height / 2.0);
  }

  private static void OnTextAlignmentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    Annotation.OnTextAlignmentChanged(d, e);
  }
}
