// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisScaleBreak
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAxisScaleBreak : FrameworkElement, INotifyPropertyChanged
{
  public static readonly DependencyProperty StartProperty = DependencyProperty.Register(nameof (Start), typeof (double), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartAxisScaleBreak.OnStartPropertyChanged)));
  public static readonly DependencyProperty EndProperty = DependencyProperty.Register(nameof (End), typeof (double), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartAxisScaleBreak.OnEndPropertyChanged)));
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof (Fill), typeof (Brush), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
  public static readonly DependencyProperty LineTypeProperty = DependencyProperty.Register(nameof (LineType), typeof (BreakLineType), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) BreakLineType.StraightLine, new PropertyChangedCallback(ChartAxisScaleBreak.OnLineTypeChanged)));
  public static readonly DependencyProperty BreakSpacingProperty = DependencyProperty.Register(nameof (BreakSpacing), typeof (double), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(ChartAxisScaleBreak.OnBreakSpacingChanged)));
  public static readonly DependencyProperty BreakPercentProperty = DependencyProperty.Register(nameof (BreakPercent), typeof (double), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) 50.0, new PropertyChangedCallback(ChartAxisScaleBreak.OnBreakPercentChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ChartAxisScaleBreak), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));

  public event PropertyChangedEventHandler PropertyChanged;

  public double Start
  {
    get => (double) this.GetValue(ChartAxisScaleBreak.StartProperty);
    set => this.SetValue(ChartAxisScaleBreak.StartProperty, (object) value);
  }

  public double End
  {
    get => (double) this.GetValue(ChartAxisScaleBreak.EndProperty);
    set => this.SetValue(ChartAxisScaleBreak.EndProperty, (object) value);
  }

  public Brush Fill
  {
    get => (Brush) this.GetValue(ChartAxisScaleBreak.FillProperty);
    set => this.SetValue(ChartAxisScaleBreak.FillProperty, (object) value);
  }

  public BreakLineType LineType
  {
    get => (BreakLineType) this.GetValue(ChartAxisScaleBreak.LineTypeProperty);
    set => this.SetValue(ChartAxisScaleBreak.LineTypeProperty, (object) value);
  }

  public double BreakSpacing
  {
    get => (double) this.GetValue(ChartAxisScaleBreak.BreakSpacingProperty);
    set => this.SetValue(ChartAxisScaleBreak.BreakSpacingProperty, (object) value);
  }

  public double BreakPercent
  {
    get => (double) this.GetValue(ChartAxisScaleBreak.BreakPercentProperty);
    set => this.SetValue(ChartAxisScaleBreak.BreakPercentProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ChartAxisScaleBreak.StrokeThicknessProperty);
    set => this.SetValue(ChartAxisScaleBreak.StrokeThicknessProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ChartAxisScaleBreak.StrokeProperty);
    set => this.SetValue(ChartAxisScaleBreak.StrokeProperty, (object) value);
  }

  public DependencyObject Clone() => this.CloneAxisBreaks((DependencyObject) null);

  internal static void DrawPath(NumericalAxis axis)
  {
    ISupportAxes supportAxes = (ISupportAxes) null;
    if (axis.RegisteredSeries != null && axis.RegisteredSeries.Count > 0)
      supportAxes = axis.RegisteredSeries[0];
    double top = 0.0;
    double bottom = 0.0;
    double left = 0.0;
    double right = 0.0;
    if (axis.BreakRanges == null)
      return;
    Rect seriesClipRect = axis.Area.SeriesClipRect;
    double actualPlotOffsetEnd = axis.GetActualPlotOffsetEnd();
    double width = axis.RenderedRect.Width;
    double height = axis.RenderedRect.Height;
    foreach (KeyValuePair<DoubleRange, ChartAxisScaleBreak> scaleBreak in axis.BreakRangesInfo)
    {
      if (scaleBreak.Key.Start > axis.VisibleRange.Start && scaleBreak.Key.End < axis.VisibleRange.End)
      {
        ChartCartesianAxisElementsPanel axisElementsPanel = axis.axisElementsPanel as ChartCartesianAxisElementsPanel;
        double strokeThickness = axisElementsPanel.MainAxisLine.StrokeThickness;
        CartesianSeries cartesianSeries = supportAxes as CartesianSeries;
        if (axis.Orientation == Orientation.Horizontal)
        {
          left = actualPlotOffsetEnd + seriesClipRect.Left + Math.Round(width * supportAxes.ActualYAxis.ValueToCoefficientCalc(scaleBreak.Key.Start));
          right = actualPlotOffsetEnd + seriesClipRect.Left + Math.Round(width * supportAxes.ActualYAxis.ValueToCoefficientCalc(scaleBreak.Key.End));
          if (cartesianSeries.YAxis != null)
            ChartAxisScaleBreak.DrawBreakLineOnAxis(left, top, right, bottom, axis, axisElementsPanel, scaleBreak);
          if (axis.OpposedPosition)
          {
            top = seriesClipRect.Top - strokeThickness;
            bottom = seriesClipRect.Top + seriesClipRect.Height + 1.5;
          }
          else
          {
            top = seriesClipRect.Top;
            bottom = seriesClipRect.Top + seriesClipRect.Height + strokeThickness + 1.5;
          }
        }
        else
        {
          top = seriesClipRect.Top + actualPlotOffsetEnd + 0.5 + Math.Round(height * (1.0 - supportAxes.ActualYAxis.ValueToCoefficientCalc(scaleBreak.Key.End)));
          bottom = seriesClipRect.Top + actualPlotOffsetEnd + 0.5 + Math.Round(height * (1.0 - supportAxes.ActualYAxis.ValueToCoefficientCalc(scaleBreak.Key.Start)));
          if (cartesianSeries.YAxis != null)
            ChartAxisScaleBreak.DrawBreakLineOnAxis(left, top, right, bottom, axis, axisElementsPanel, scaleBreak);
          if (axis.OpposedPosition)
          {
            left = seriesClipRect.Left - 0.5;
            right = seriesClipRect.Left + seriesClipRect.Width + strokeThickness + 0.5;
          }
          else
          {
            left = seriesClipRect.Left - strokeThickness - 0.5;
            right = seriesClipRect.Left + seriesClipRect.Width + 0.5;
          }
        }
        ChartAxisScaleBreak.CalculateDrawingPoints(axis, scaleBreak, left, top, right, bottom);
      }
    }
  }

  protected virtual DependencyObject CloneAxisBreaks(DependencyObject obj)
  {
    return (DependencyObject) new ChartAxisScaleBreak()
    {
      Start = this.Start,
      End = this.End,
      Fill = this.Fill,
      LineType = this.LineType,
      BreakSpacing = this.BreakSpacing,
      BreakPercent = this.BreakPercent,
      StrokeThickness = this.StrokeThickness,
      Stroke = this.Stroke
    };
  }

  private static void OnStartPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisScaleBreak).OnPropertyChanged("Start");
  }

  private static void OnEndPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisScaleBreak).OnPropertyChanged("End");
  }

  private static void OnLineTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisScaleBreak).OnPropertyChanged("LineType");
  }

  private static void OnBreakSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisScaleBreak).OnPropertyChanged("BreakSpacing");
  }

  private static void OnBreakPercentChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAxisScaleBreak).OnPropertyChanged("BreakPercent");
  }

  private static void DrawLine(
    double left,
    double top,
    double right,
    double bottom,
    NumericalAxis axis,
    KeyValuePair<DoubleRange, ChartAxisScaleBreak> scaleBreak)
  {
    Polyline element1 = new Polyline();
    Polyline element2 = new Polyline();
    Polyline element3 = new Polyline();
    if (axis.Orientation == Orientation.Horizontal)
    {
      element1.Points.Add(new Point(left, top));
      element1.Points.Add(new Point(left, bottom));
      element2.Points.Add(new Point(right, top));
      element2.Points.Add(new Point(right, bottom));
      element3.Points.Add(new Point(left, top));
      element3.Points.Add(new Point(left, bottom));
      element3.Points.Add(new Point(right, bottom));
      element3.Points.Add(new Point(right, top));
    }
    else
    {
      element1.Points.Add(new Point(left, top));
      element1.Points.Add(new Point(right, top));
      element2.Points.Add(new Point(left, bottom));
      element2.Points.Add(new Point(right, bottom));
      element3.Points.Add(new Point(left, top));
      element3.Points.Add(new Point(right, top));
      element3.Points.Add(new Point(right, bottom));
      element3.Points.Add(new Point(left, bottom));
    }
    element1.SetBinding(Shape.StrokeProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("Stroke", (object) scaleBreak.Value));
    element2.SetBinding(Shape.StrokeProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("Stroke", (object) scaleBreak.Value));
    element1.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("StrokeThickness", (object) scaleBreak.Value));
    element2.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("StrokeThickness", (object) scaleBreak.Value));
    element3.SetBinding(Shape.FillProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("Fill", (object) scaleBreak.Value));
    if (!axis.Area.AdorningCanvas.Children.Contains((UIElement) element3))
      axis.Area.AdorningCanvas.Children.Add((UIElement) element3);
    if (!axis.Area.AdorningCanvas.Children.Contains((UIElement) element2))
      axis.Area.AdorningCanvas.Children.Add((UIElement) element2);
    if (!axis.Area.AdorningCanvas.Children.Contains((UIElement) element1))
      axis.Area.AdorningCanvas.Children.Add((UIElement) element1);
    axis.BreakShapes.Add((FrameworkElement) element1);
    axis.BreakShapes.Add((FrameworkElement) element2);
    axis.BreakShapes.Add((FrameworkElement) element3);
  }

  private static void CalculateDrawingPoints(
    NumericalAxis axis,
    KeyValuePair<DoubleRange, ChartAxisScaleBreak> scaleBreak,
    double left,
    double top,
    double right,
    double bottom)
  {
    if (scaleBreak.Value.LineType == BreakLineType.StraightLine)
    {
      ChartAxisScaleBreak.DrawLine(left, top, right, bottom, axis, scaleBreak);
    }
    else
    {
      Path element = new Path();
      Point point1;
      Point point2;
      Point[] pointArray1;
      if (axis.Orientation == Orientation.Vertical)
      {
        if (axis.IsInversed)
        {
          point1 = new Point(left, bottom);
          point2 = new Point(right, bottom);
        }
        else
        {
          point1 = new Point(left, top);
          point2 = new Point(right, top);
        }
        pointArray1 = axis.OpposedPosition ? ChartAxisScaleBreak.GetWaveBeziersPoints(point2, point1, 10, 5f) : ChartAxisScaleBreak.GetWaveBeziersPoints(point1, point2, 10, 5f);
      }
      else
      {
        if (axis.IsInversed)
        {
          point1 = new Point(right, top);
          point2 = new Point(right, bottom);
        }
        else
        {
          point1 = new Point(left, top);
          point2 = new Point(left, bottom);
        }
        pointArray1 = axis.OpposedPosition ? ChartAxisScaleBreak.GetWaveBeziersPoints(point1, point2, 10, 5f) : ChartAxisScaleBreak.GetWaveBeziersPoints(point2, point1, 10, 5f);
      }
      Point[] pointArray2 = new Point[pointArray1.Length];
      Point[] pointArray3 = new Point[pointArray1.Length];
      double num1 = point2.X - point1.X;
      double num2 = point2.Y - point1.Y;
      double num3 = Math.Sqrt(num1 * num1 + num2 * num2);
      double breakSpacing = scaleBreak.Value.BreakSpacing;
      double num4 = breakSpacing * num2 / num3;
      double num5 = breakSpacing * num1 / num3;
      for (int index = 0; index < pointArray1.Length; ++index)
      {
        pointArray2[index] = new Point(pointArray1[index].X, pointArray1[index].Y);
        pointArray3[pointArray1.Length - index - 1] = new Point(pointArray1[index].X + num4, pointArray1[index].Y + num5);
      }
      PathFigure pathFigure = new PathFigure()
      {
        StartPoint = new Point(pointArray2[0].X, pointArray2[0].Y)
      };
      Point point3;
      Point point4;
      Point point5;
      for (int index = 1; index < pointArray2.Length; index += 3)
      {
        point3 = new Point(pointArray2[index].X, pointArray2[index].Y);
        point4 = new Point(pointArray2[index + 1].X, pointArray2[index + 1].Y);
        point5 = new Point(pointArray2[index + 2].X, pointArray2[index + 2].Y);
        pathFigure.Segments.Add((PathSegment) new BezierSegment()
        {
          Point1 = point3,
          Point2 = point4,
          Point3 = point5
        });
      }
      System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment()
      {
        Point = new Point(pointArray3[0].X, pointArray3[0].Y)
      };
      pathFigure.Segments.Add((PathSegment) lineSegment);
      for (int index = 1; index < pointArray2.Length; index += 3)
      {
        point3 = new Point(pointArray3[index].X, pointArray3[index].Y);
        point4 = new Point(pointArray3[index + 1].X, pointArray3[index + 1].Y);
        point5 = new Point(pointArray3[index + 2].X, pointArray3[index + 2].Y);
        pathFigure.Segments.Add((PathSegment) new BezierSegment()
        {
          Point1 = point3,
          Point2 = point4,
          Point3 = point5
        });
      }
      element.Data = (Geometry) new PathGeometry()
      {
        Figures = {
          pathFigure
        }
      };
      element.SetBinding(Shape.StrokeProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("Stroke", (object) scaleBreak.Value));
      element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("StrokeThickness", (object) scaleBreak.Value));
      element.SetBinding(Shape.FillProperty, (BindingBase) ChartAxisScaleBreak.CreateBinding("Fill", (object) scaleBreak.Value));
      if (!axis.Area.AdorningCanvas.Children.Contains((UIElement) element))
        axis.Area.AdorningCanvas.Children.Add((UIElement) element);
      axis.BreakShapes.Add((FrameworkElement) element);
    }
  }

  private static Binding CreateBinding(string path, object source)
  {
    return new Binding()
    {
      Path = new PropertyPath(path, new object[0]),
      Source = source,
      Mode = BindingMode.OneWay
    };
  }

  private static void DrawBreakLineOnAxis(
    double left,
    double top,
    double right,
    double bottom,
    NumericalAxis axis,
    ChartCartesianAxisElementsPanel panel,
    KeyValuePair<DoubleRange, ChartAxisScaleBreak> scaleBreak)
  {
    if (panel == null)
      return;
    Rect arrangeRect = axis.ArrangeRect;
    double width = axis.axisLabelsPanel.DesiredSize.Width;
    double height = axis.axisLabelsPanel.DesiredSize.Height;
    Line mainAxisLine = panel.MainAxisLine;
    if (axis.Orientation == Orientation.Horizontal)
    {
      if (axis.OpposedPosition)
      {
        if (axis.GetLabelPosition() == AxisElementPosition.Inside)
        {
          top = arrangeRect.Top + mainAxisLine.Y1 - 5.0;
          bottom = arrangeRect.Top + mainAxisLine.Y2 + 5.0;
        }
        else
        {
          top = arrangeRect.Top + mainAxisLine.Y1 + height - 5.0;
          bottom = arrangeRect.Top + mainAxisLine.Y2 + height + 5.0;
        }
      }
      else if (axis.GetLabelPosition() == AxisElementPosition.Outside)
      {
        top = arrangeRect.Top + mainAxisLine.Y1 - 5.0;
        bottom = arrangeRect.Top + mainAxisLine.Y2 + 5.0;
      }
      else
      {
        top = arrangeRect.Top + mainAxisLine.Y1 + height - 5.0;
        bottom = arrangeRect.Top + mainAxisLine.Y2 + height + 5.0;
      }
    }
    else if (axis.OpposedPosition)
    {
      if (axis.GetLabelPosition() == AxisElementPosition.Outside)
      {
        left = arrangeRect.Left + mainAxisLine.X1 - 5.0;
        right = arrangeRect.Left + panel.MainAxisLine.X2 + 5.0;
      }
      else
      {
        left = arrangeRect.Left + mainAxisLine.X1 + width - 5.0;
        right = arrangeRect.Left + mainAxisLine.X2 + width + 5.0;
      }
    }
    else if (axis.GetLabelPosition() == AxisElementPosition.Inside)
    {
      left = arrangeRect.Left + mainAxisLine.X1 - 5.0;
      right = arrangeRect.Left + mainAxisLine.X2 + 5.0;
    }
    else
    {
      left = arrangeRect.Left + mainAxisLine.X1 + width - 5.0;
      right = arrangeRect.Left + mainAxisLine.X2 + width + 5.0;
    }
    ChartAxisScaleBreak.DrawLine(left, top, right, bottom, axis, scaleBreak);
  }

  private static Point[] GetWaveBeziersPoints(Point pt1, Point pt2, int count, float fault)
  {
    double num1 = pt2.X - pt1.X;
    double num2 = pt2.Y - pt1.Y;
    double num3 = Math.Sqrt(num1 * num1 + num2 * num2);
    double num4 = (double) fault * num2 / num3;
    double num5 = (double) fault * num1 / num3;
    double num6 = num1 / (double) count;
    double num7 = num2 / (double) count;
    Point[] waveBeziersPoints = new Point[3 * count + 1];
    for (int index = 0; index < count; ++index)
    {
      waveBeziersPoints[3 * index] = new Point(pt1.X + num6 * (double) index, pt1.Y + num7 * (double) index);
      waveBeziersPoints[3 * index + 1] = new Point(pt1.X + num6 * ((double) index + 0.5) + num4, pt1.Y + num7 * ((double) index + 0.5) + num5);
      waveBeziersPoints[3 * index + 2] = new Point(pt1.X + num6 * ((double) index + 0.5) - num4, pt1.Y + num7 * ((double) index + 0.5) - num5);
    }
    waveBeziersPoints[waveBeziersPoints.Length - 1] = pt2;
    return waveBeziersPoints;
  }

  private void OnPropertyChanged(string name)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(name));
  }
}
