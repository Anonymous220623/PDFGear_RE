// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AxisMarker
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class AxisMarker : ShapeAnnotation
{
  internal ContentControl markerContent;

  internal Canvas MarkerCanvas { get; set; }

  internal ShapeAnnotation ParentAnnotation { get; set; }

  public override void UpdateAnnotation()
  {
    if (this.markerContent == null)
      return;
    if (this.XAxis != null && this.YAxis != null && this.X1 != null && this.Y1 != null && this.X2 != null && this.Y2 != null)
    {
      this.markerContent.Visibility = Visibility.Visible;
      if (this.XAxis.Orientation == Orientation.Vertical)
      {
        if (this.XAxis is LogarithmicAxis && this.ParentAnnotation is VerticalLineAnnotation)
        {
          this.x1 = Convert.ToDouble(this.X1);
          this.x2 = Convert.ToDouble(this.X2);
        }
        else
        {
          this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
          this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
        }
        if (this.YAxis is LogarithmicAxis && this.ParentAnnotation is HorizontalLineAnnotation)
        {
          this.y1 = Convert.ToDouble(this.Y1);
          this.y2 = Convert.ToDouble(this.Y2);
        }
        else
        {
          this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
          this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
        }
      }
      else
      {
        if (this.XAxis is LogarithmicAxis && this.ParentAnnotation is HorizontalLineAnnotation)
        {
          this.x1 = Convert.ToDouble(this.X1);
          this.x2 = Convert.ToDouble(this.X2);
        }
        else
        {
          this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
          this.x2 = Annotation.ConvertData(this.X2, this.XAxis);
        }
        if (this.YAxis is LogarithmicAxis && this.ParentAnnotation is VerticalLineAnnotation)
        {
          this.y1 = Convert.ToDouble(this.Y1);
          this.y2 = Convert.ToDouble(this.Y2);
        }
        else
        {
          this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
          this.y2 = Annotation.ConvertData(this.Y2, this.YAxis);
        }
      }
      Point point1 = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y1), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x1));
      Point point2 = this.XAxis.Orientation == Orientation.Horizontal ? new Point(this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2), this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2)) : new Point(this.Chart.ValueToPointRelativeToAnnotation(this.YAxis, this.y2), this.Chart.ValueToPointRelativeToAnnotation(this.XAxis, this.x2));
      point1.Y = double.IsNaN(point1.Y) ? 0.0 : point1.Y;
      point1.X = double.IsNaN(point1.X) ? 0.0 : point1.X;
      point2.Y = double.IsNaN(point2.Y) ? 0.0 : point2.Y;
      point2.X = double.IsNaN(point2.X) ? 0.0 : point2.X;
      ChartAxis chartAxis;
      if (this.ParentAnnotation is VerticalLineAnnotation)
      {
        this.markerContent.Content = this.GetXAxisContent();
        chartAxis = this.XAxis.Orientation == Orientation.Horizontal ? this.XAxis : this.YAxis;
      }
      else
      {
        chartAxis = this.XAxis.Orientation == Orientation.Vertical ? this.XAxis : this.YAxis;
        this.markerContent.Content = this.GetYAxisContent();
      }
      Rect rect = new Rect(point1, point2);
      Point originalPosition = this.EnsurePoint(point1, point2);
      Size desiredSize = new Size(rect.Width, rect.Height);
      Point elementPosition = this.GetElementPosition(new Size(rect.Width, rect.Height), originalPosition);
      this.markerContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      Point axisLabelPosition = this.GetAxisLabelPosition(desiredSize, elementPosition, new Size(this.markerContent.DesiredSize.Width, this.markerContent.DesiredSize.Height));
      if (chartAxis.Visibility != Visibility.Collapsed)
      {
        Canvas.SetLeft((UIElement) this.markerContent, axisLabelPosition.X);
        Canvas.SetTop((UIElement) this.markerContent, axisLabelPosition.Y);
      }
      else
        this.markerContent.Visibility = Visibility.Collapsed;
      this.RotatedRect = new Rect(axisLabelPosition.X, axisLabelPosition.Y, this.markerContent.DesiredSize.Width, this.markerContent.DesiredSize.Height);
    }
    else
      this.markerContent.Visibility = Visibility.Collapsed;
  }

  internal Point GetAxisLabelPosition(Size desiredSize, Point originalPosition, Size textSize)
  {
    Point axisLabelPosition = originalPosition;
    if (this.ParentAnnotation is HorizontalLineAnnotation && this.XAxis.Orientation == Orientation.Horizontal || this.ParentAnnotation is VerticalLineAnnotation && this.XAxis.Orientation == Orientation.Vertical)
    {
      ChartAxis axis = this.XAxis.Orientation == Orientation.Vertical ? this.XAxis : this.YAxis;
      ChartAxis chartAxis = this.XAxis.Orientation != Orientation.Vertical ? this.XAxis : this.YAxis;
      axisLabelPosition.Y -= this.GetHorizontalAxisLabelAlignment(textSize);
      double num = 0.0;
      IEnumerable<ChartAxis> source = this.Chart.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axes => axes.Orientation == axis.Orientation)).Where<ChartAxis>((Func<ChartAxis, bool>) (position => !position.OpposedPosition));
      axisLabelPosition.X -= chartAxis.GetActualPlotOffsetStart();
      if (axis.OpposedPosition)
      {
        if (axis.axisLabelsPanel != null)
          axisLabelPosition.X += axis.LabelsPosition == AxisElementPosition.Inside ? axis.axisLabelsPanel.DesiredSize.Width : 0.0;
        axisLabelPosition.X += axis.TickLinesPosition == AxisElementPosition.Inside ? axis.TickLineSize : 0.0;
        if (source.Count<ChartAxis>() > 0)
          num = source.ElementAt<ChartAxis>(0).RenderedRect.Right;
        axisLabelPosition.X += axis.RenderedRect.Left - num;
        axisLabelPosition.X -= (this.ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0.0 : textSize.Width;
      }
      else
      {
        if (source.Count<ChartAxis>() > 0)
          num = source.ElementAt<ChartAxis>(0).RenderedRect.Left;
        axisLabelPosition.X -= textSize.Width + (num - axis.RenderedRect.Left);
        axisLabelPosition.X += (this.ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0.0 : textSize.Width;
      }
    }
    else
    {
      ChartAxis axis = this.XAxis.Orientation == Orientation.Horizontal ? this.XAxis : this.YAxis;
      ChartAxis chartAxis = this.XAxis.Orientation != Orientation.Horizontal ? this.XAxis : this.YAxis;
      axisLabelPosition.X += desiredSize.Width / 2.0;
      axisLabelPosition.X -= this.GetVerticalAxisLabelAlignment(textSize);
      axisLabelPosition.Y = axis.OpposedPosition ? axisLabelPosition.Y : 0.0;
      IEnumerable<ChartAxis> source = this.Chart.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axes => axes.Orientation == axis.Orientation)).Where<ChartAxis>((Func<ChartAxis, bool>) (position => position.OpposedPosition));
      double num = source.Count<ChartAxis>() > 0 ? source.ElementAt<ChartAxis>(0).RenderedRect.Bottom : 0.0;
      if (axis.OpposedPosition)
      {
        axisLabelPosition.Y -= textSize.Height;
        if (this.Chart.Axes.IndexOf(axis) != 0)
          axisLabelPosition.Y -= num - axis.RenderedRect.Bottom;
        axisLabelPosition.Y -= chartAxis.GetActualPlotOffsetEnd();
        axisLabelPosition.Y += (this.ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0.0 : textSize.Height;
      }
      else
      {
        if (axis.axisLabelsPanel != null)
          axisLabelPosition.Y += axis.LabelsPosition == AxisElementPosition.Inside ? axis.axisLabelsPanel.DesiredSize.Height : 0.0;
        axisLabelPosition.Y += axis.TickLinesPosition == AxisElementPosition.Inside ? axis.TickLineSize : 0.0;
        axisLabelPosition.Y += axis.RenderedRect.Top - num;
        axisLabelPosition.Y -= (this.ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0.0 : textSize.Height;
      }
    }
    return axisLabelPosition;
  }

  internal override UIElement CreateAnnotation()
  {
    if (this.MarkerCanvas == null)
    {
      this.MarkerCanvas = new Canvas();
      this.markerContent = new ContentControl();
      this.markerContent.ContentTemplate = (this.ParentAnnotation as StraightLineAnnotation).AxisLabelTemplate != null ? (this.ParentAnnotation as StraightLineAnnotation).AxisLabelTemplate : ChartDictionaries.GenericCommonDictionary[(object) "AxisLabel"] as DataTemplate;
      this.CanDrag = this.ParentAnnotation.CanDrag;
      this.SetBindings();
      this.MarkerCanvas.Children.Add((UIElement) this.markerContent);
    }
    return (UIElement) this.MarkerCanvas;
  }

  protected override void SetBindings()
  {
    Binding binding = new Binding()
    {
      Path = new PropertyPath("Cursor", new object[0]),
      Source = (object) this.ParentAnnotation
    };
    this.markerContent.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding);
  }

  private static bool CheckPointRange(double point, ChartAxis axis)
  {
    return point <= axis.VisibleRange.End && point >= axis.VisibleRange.Start;
  }

  private object GetXAxisContent()
  {
    this.markerContent.Visibility = !AxisMarker.CheckPointRange(this.x1, this.XAxis) ? Visibility.Collapsed : Visibility.Visible;
    return !(this.XAxis is NumericalAxis) && !(this.XAxis is LogarithmicAxis) ? this.XAxis.GetLabelContent(this.x1) : (object) Convert.ToDecimal(this.X1).ToString("0.##");
  }

  private object GetYAxisContent()
  {
    this.markerContent.Visibility = !AxisMarker.CheckPointRange(this.y1, this.YAxis) ? Visibility.Collapsed : Visibility.Visible;
    return !(this.YAxis is NumericalAxis) && !(this.YAxis is LogarithmicAxis) ? this.XAxis.GetLabelContent(this.y1) : (object) Convert.ToDecimal(this.Y1).ToString("0.##");
  }

  private double GetVerticalAxisLabelAlignment(Size textSize)
  {
    double axisLabelAlignment = 0.0;
    switch ((this.ParentAnnotation as StraightLineAnnotation).AxisLabelAlignment)
    {
      case LabelAlignment.Center:
        axisLabelAlignment = textSize.Width / 2.0;
        break;
      case LabelAlignment.Near:
        axisLabelAlignment = textSize.Width;
        break;
    }
    return axisLabelAlignment;
  }

  private double GetHorizontalAxisLabelAlignment(Size textSize)
  {
    double axisLabelAlignment = 0.0;
    switch ((this.ParentAnnotation as StraightLineAnnotation).AxisLabelAlignment)
    {
      case LabelAlignment.Center:
        axisLabelAlignment = textSize.Height / 2.0;
        break;
      case LabelAlignment.Near:
        axisLabelAlignment = textSize.Height;
        break;
    }
    return axisLabelAlignment;
  }
}
