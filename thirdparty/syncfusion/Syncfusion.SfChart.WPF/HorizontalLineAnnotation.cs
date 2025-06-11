// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HorizontalLineAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HorizontalLineAnnotation : StraightLineAnnotation
{
  public override void UpdateAnnotation()
  {
    if (this.shape == null)
      return;
    this.ValidateSelection();
    switch (this.CoordinateUnit)
    {
      case CoordinateUnit.Pixel:
        if (this.ShowLine && this.Chart != null && this.Chart.AnnotationManager != null && this.Y1 != null)
        {
          this.DraggingMode = AxisMode.Vertical;
          if (this.Y1 == null)
            this.Y1 = (object) 0;
          this.Y2 = this.Y1;
          this.X1 = this.X1 == null ? (object) 0 : this.X1;
          this.X2 = this.X2 == null || Convert.ToDouble(this.X2) == 0.0 ? (object) this.Chart.DesiredSize.Width : this.X2;
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
          if (this.XAxis.Orientation.Equals((object) Orientation.Vertical))
          {
            if (this.Y1 != null)
            {
              this.x1 = this.X1 == null ? this.XAxis.VisibleRange.Start : Annotation.ConvertData(this.X1, this.XAxis);
              this.x2 = this.X2 == null ? this.XAxis.VisibleRange.End : Annotation.ConvertData(this.X2, this.XAxis);
              this.Y2 = this.Y1;
              this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
              this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
              if (this.ShowAxisLabel)
                this.SetAxisMarkerValue((object) this.XAxis.VisibleRange.End, (object) this.XAxis.VisibleRange.Start, this.Y1, this.Y2, AxisMode.Horizontal);
              this.DraggingMode = AxisMode.Horizontal;
            }
            else
              break;
          }
          else if (this.Y1 != null)
          {
            this.x1 = this.X1 == null ? this.XAxis.VisibleRange.Start : Annotation.ConvertData(this.X1, this.XAxis);
            this.x2 = this.X2 == null ? this.XAxis.VisibleRange.End : Annotation.ConvertData(this.X2, this.XAxis);
            this.Y2 = this.Y1;
            this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
            this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
            if (this.ShowAxisLabel)
              this.SetAxisMarkerValue((object) this.XAxis.VisibleRange.Start, (object) this.XAxis.VisibleRange.End, this.Y1, this.Y2, AxisMode.Vertical);
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
            this.DrawLine(this.XAxis.Orientation.Equals((object) Orientation.Horizontal) ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1) - this.XAxis.GetActualPlotOffsetStart(), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1) + this.YAxis.GetActualPlotOffsetStart()), this.XAxis.Orientation.Equals((object) Orientation.Horizontal) ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2) + this.XAxis.GetActualPlotOffsetEnd(), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2) - this.YAxis.GetActualPlotOffsetEnd()), this.shape);
            break;
          }
          break;
        }
        break;
    }
    if (this.XAxis == null || !this.XAxis.Orientation.Equals((object) Orientation.Vertical) || this.X1 != null || this.Y1 != null)
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
    double width = Math.Abs(linePoint2.X - linePoint1.X);
    double num = Math.Abs(linePoint2.Y - linePoint1.Y);
    this.RotatedRect = new Rect(point.X, point.Y - this.GrabExtent, width, num + 2.0 * this.GrabExtent);
  }
}
