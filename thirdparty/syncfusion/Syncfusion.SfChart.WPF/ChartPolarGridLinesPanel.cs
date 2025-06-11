// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPolarGridLinesPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPolarGridLinesPanel : ILayoutCalculator
{
  private Size desiredSize;
  private Panel panel;
  private UIElementsRecycler<Ellipse> ellipseRecycler;
  private UIElementsRecycler<Line> linesRecycler;
  private UIElementsRecycler<Line> ylinesRecycler;
  private UIElementsRecycler<Path> pathRecycler;

  public ChartPolarGridLinesPanel(Panel panel)
  {
    this.panel = panel != null ? panel : throw new ArgumentNullException();
    this.ellipseRecycler = new UIElementsRecycler<Ellipse>(panel);
    this.linesRecycler = new UIElementsRecycler<Line>(panel);
    this.ylinesRecycler = new UIElementsRecycler<Line>(panel);
    this.pathRecycler = new UIElementsRecycler<Path>(panel);
  }

  public bool IsRadar
  {
    get
    {
      return this.Area != null && this.Area.VisibleSeries != null && this.Area.VisibleSeries.Count > 0 && this.Area.VisibleSeries[0] is RadarSeries;
    }
  }

  public Panel Panel => this.panel;

  internal SfChart Area { get; set; }

  public ChartAxis XAxis => this.Area.InternalPrimaryAxis;

  public ChartAxis YAxis => this.Area.InternalSecondaryAxis;

  public Size DesiredSize => this.desiredSize;

  public List<UIElement> Children
  {
    get
    {
      return this.panel != null ? this.panel.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  public double Left { get; set; }

  public double Top { get; set; }

  public Size Measure(Size availableSize)
  {
    this.desiredSize = new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height);
    if (!this.IsRadar)
      this.RenderCircles();
    return availableSize;
  }

  public Size Arrange(Size finalSize)
  {
    this.RenderGridLines();
    return finalSize;
  }

  public void DetachElements()
  {
    if (this.ellipseRecycler != null)
      this.ellipseRecycler.Clear();
    if (this.linesRecycler != null)
      this.linesRecycler.Clear();
    if (this.ylinesRecycler != null)
      this.ylinesRecycler.Clear();
    this.panel = (Panel) null;
  }

  public void UpdateElements()
  {
    int count1 = 0;
    if (this.YAxis != null)
      count1 = this.YAxis.VisibleLabels.Count;
    if (!this.linesRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty) && this.Area.InternalPrimaryAxis != null)
      this.linesRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, new Binding()
      {
        Path = new PropertyPath("MajorGridLineStyle", new object[0]),
        Source = (object) this.Area.InternalPrimaryAxis
      });
    if (!this.IsRadar)
    {
      if (this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.MajorGridLineStyle.TargetType == typeof (Ellipse) && !this.ellipseRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
      {
        this.ellipseRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, new Binding()
        {
          Path = new PropertyPath("MajorGridLineStyle", new object[0]),
          Source = (object) this.Area.InternalSecondaryAxis
        });
        this.ellipseRecycler.GenerateElements(count1);
      }
      else
      {
        this.ylinesRecycler.Clear();
        this.ellipseRecycler.GenerateElements(count1);
        foreach (Ellipse ellipse in this.ellipseRecycler)
        {
          ellipse.Stroke = (Brush) new SolidColorBrush(Colors.Gray);
          ellipse.StrokeThickness = 1.0;
        }
      }
    }
    else if (this.XAxis != null)
    {
      this.ellipseRecycler.Clear();
      int count2 = count1 * this.XAxis.VisibleLabels.Count;
      if (this.Area.InternalSecondaryAxis != null && !this.ylinesRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
        this.ylinesRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, new Binding()
        {
          Path = new PropertyPath("MajorGridLineStyle", new object[0]),
          Source = (object) this.Area.InternalSecondaryAxis
        });
      this.ylinesRecycler.GenerateElements(count2);
    }
    if (this.XAxis != null)
      count1 = this.XAxis.VisibleLabels.Count;
    this.linesRecycler.GenerateElements(count1);
  }

  internal void Dispose()
  {
    this.Area = (SfChart) null;
    if (this.Children.Count > 0)
      this.Children.Clear();
    if (this.ellipseRecycler != null && this.ellipseRecycler.Count > 0)
      this.ellipseRecycler.Clear();
    if (this.linesRecycler != null && this.linesRecycler.Count > 0)
      this.linesRecycler.Clear();
    if (this.ylinesRecycler != null && this.ylinesRecycler.Count > 0)
      this.ylinesRecycler.Clear();
    if (this.pathRecycler != null && this.pathRecycler.Count > 0)
      this.pathRecycler.Clear();
    this.ellipseRecycler = (UIElementsRecycler<Ellipse>) null;
    this.linesRecycler = (UIElementsRecycler<Line>) null;
    this.ylinesRecycler = (UIElementsRecycler<Line>) null;
    this.pathRecycler = (UIElementsRecycler<Path>) null;
  }

  internal void RenderCircles()
  {
    ChartAxis yaxis = this.YAxis;
    double num1 = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point point = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    if (yaxis == null || !yaxis.ShowGridLines || this.ellipseRecycler.Count <= 0)
      return;
    int index = 0;
    foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) yaxis.VisibleLabels)
    {
      double num2 = num1 * yaxis.ValueToCoefficientCalc(visibleLabel.Position);
      Ellipse element = this.ellipseRecycler[index];
      element.Width = num2 * 2.0;
      element.Height = num2 * 2.0;
      Canvas.SetLeft((UIElement) element, point.X - num2);
      Canvas.SetTop((UIElement) element, point.Y - num2);
      ++index;
    }
  }

  internal void UpdateStripLines()
  {
    this.pathRecycler.Clear();
    if (!this.IsRadar)
      this.RenderPolarStripLines();
    else
      this.RenderRadarStripLines();
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

  private static void RenderEllipse(
    Point center,
    double start,
    double end,
    out GeometryGroup group)
  {
    EllipseGeometry ellipseGeometry1 = new EllipseGeometry()
    {
      Center = center,
      RadiusX = end,
      RadiusY = end
    };
    EllipseGeometry ellipseGeometry2 = new EllipseGeometry()
    {
      Center = center,
      RadiusX = start,
      RadiusY = start
    };
    group = new GeometryGroup();
    group.Children.Add((Geometry) ellipseGeometry2);
    group.Children.Add((Geometry) ellipseGeometry1);
  }

  private static void RenderSegmentedPath(
    double start,
    double end,
    double angle,
    Point center,
    Point vector1,
    Point vector2,
    ArcSegment innerArc,
    ArcSegment outerArc,
    out PathGeometry pathGeometry)
  {
    Point point1 = new Point(center.X + start * vector1.X, center.Y + start * vector1.Y);
    Point point2 = new Point(center.X + start * vector2.X, center.Y + start * vector2.Y);
    Point point3 = new Point(center.X + end * vector1.X, center.Y + end * vector1.Y);
    Point point4 = new Point(center.X + end * vector2.X, center.Y + end * vector2.Y);
    pathGeometry = new PathGeometry();
    PathFigure pathFigure = new PathFigure();
    pathFigure.StartPoint = point1;
    ChartPolarGridLinesPanel.RenderArc(start, angle, point2, out innerArc, SweepDirection.Clockwise);
    System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
    lineSegment.Point = point4;
    ChartPolarGridLinesPanel.RenderArc(end, angle, point3, out outerArc, SweepDirection.Counterclockwise);
    pathFigure.IsClosed = true;
    pathFigure.Segments.Add((PathSegment) innerArc);
    pathFigure.Segments.Add((PathSegment) lineSegment);
    pathFigure.Segments.Add((PathSegment) outerArc);
    pathGeometry.Figures.Add(pathFigure);
  }

  private static void CalculateAngle(Point vector1, Point vector2, out double angle)
  {
    angle = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
    angle = angle * 180.0 / Math.PI;
    if (angle >= 0.0)
      return;
    angle += 360.0;
  }

  private static void RenderArc(
    double radius,
    double angle,
    Point point,
    out ArcSegment arc,
    SweepDirection direction)
  {
    arc = new ArcSegment();
    arc.Size = new Size(radius, radius);
    arc.SweepDirection = direction;
    if (angle > 180.0)
      arc.IsLargeArc = true;
    arc.Point = point;
  }

  private void RenderStripLine(Geometry data, ChartStripLine stripLine)
  {
    Path newInstance = this.pathRecycler.CreateNewInstance();
    newInstance.Data = data;
    newInstance.SetBinding(Shape.FillProperty, (BindingBase) ChartPolarGridLinesPanel.CreateBinding("Background", (object) stripLine));
    newInstance.SetBinding(Shape.StrokeProperty, (BindingBase) ChartPolarGridLinesPanel.CreateBinding("BorderBrush", (object) stripLine));
    newInstance.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) ChartPolarGridLinesPanel.CreateBinding("BorderThickness", (object) stripLine));
    newInstance.SetBinding(UIElement.OpacityProperty, (BindingBase) ChartPolarGridLinesPanel.CreateBinding("Opacity", (object) stripLine));
    newInstance.SetBinding(UIElement.VisibilityProperty, (BindingBase) ChartPolarGridLinesPanel.CreateBinding("Visibility", (object) stripLine));
  }

  private void UpdatePolarVerticalStripLine(ChartAxis axis)
  {
    double num1 = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point center = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    double num2 = num1 * axis.ValueToCoefficientCalc(axis.VisibleRange.Start);
    double num3 = num1 * axis.ValueToCoefficientCalc(axis.VisibleRange.End);
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) (axis as ChartAxisBase2D).StripLines)
    {
      double start1 = stripLine.Start;
      if (double.IsNaN(start1))
        break;
      double num4 = start1 > axis.VisibleRange.End ? axis.VisibleRange.End : start1;
      double num5 = double.IsNaN(stripLine.RepeatUntil) ? axis.VisibleRange.End : stripLine.RepeatUntil;
      double num6 = num5 > axis.VisibleRange.End ? axis.VisibleRange.End : (num5 < axis.VisibleRange.Start ? axis.VisibleRange.Start : num5);
      double repeatEvery = stripLine.RepeatEvery;
      double num7 = !double.IsNaN(stripLine.RepeatUntil) ? (num4 == stripLine.RepeatUntil ? num4 : (num4 < stripLine.RepeatUntil ? num4 + stripLine.Width : num4 - stripLine.Width)) : num4 + stripLine.Width;
      double num8 = num7 < axis.VisibleRange.End ? num7 : axis.VisibleRange.End;
      GeometryGroup group;
      if (!stripLine.IsSegmented)
      {
        do
        {
          double num9 = num1 * axis.ValueToCoefficientCalc(num4);
          double num10 = num1 * axis.ValueToCoefficientCalc(num8);
          if (stripLine.IsPixelWidth)
            num10 = num10 > num9 ? num9 + stripLine.Width : num9 - stripLine.Width;
          double start2 = num9 <= num2 ? num2 : (num9 >= num3 ? num3 : num9);
          double end = num10 <= num2 ? num2 : (num10 >= num3 ? num3 : num10);
          ChartPolarGridLinesPanel.RenderEllipse(center, start2, end, out group);
          this.RenderStripLine((Geometry) group, stripLine);
          num4 = num4 < num6 ? num4 + repeatEvery : num4 - repeatEvery;
          double num11 = !double.IsNaN(stripLine.RepeatUntil) ? (num4 == stripLine.RepeatUntil ? num4 : (num4 < stripLine.RepeatUntil ? num4 + stripLine.Width : num4 - stripLine.Width)) : num4 + stripLine.Width;
          num8 = num11 < axis.VisibleRange.End ? num11 : axis.VisibleRange.End;
        }
        while (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num4 >= num6 ? 0 : (num8 <= num6 ? 1 : 0)) : (num4 <= num6 ? 0 : (num8 >= num6 ? 1 : 0))) != 0);
      }
      else
      {
label_8:
        ArcSegment innerArc = (ArcSegment) null;
        ArcSegment outerArc = (ArcSegment) null;
        double num12 = this.XAxis.VisibleRange.End + this.XAxis.VisibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
        double start3 = this.XAxis.VisibleRange.Start;
        double num13 = num1 * axis.ValueToCoefficientCalc(num4);
        double num14 = num1 * axis.ValueToCoefficientCalc(num8);
        double segmentStartValue = stripLine.SegmentStartValue;
        double segmentEndValue = stripLine.SegmentEndValue;
        double num15 = !(this.XAxis is NumericalAxis) ? (segmentStartValue > num12 ? num12 : (segmentStartValue < start3 ? start3 : segmentStartValue)) : (segmentStartValue > this.XAxis.VisibleRange.End ? this.XAxis.VisibleRange.End : (segmentStartValue < start3 ? start3 : segmentStartValue));
        double num16 = !(this.XAxis is NumericalAxis) ? (segmentEndValue > num12 ? num12 : (segmentEndValue < start3 ? start3 : segmentEndValue)) : (segmentEndValue > this.XAxis.VisibleRange.End ? this.XAxis.VisibleRange.End : (segmentEndValue < start3 ? start3 : segmentEndValue));
        if (stripLine.IsPixelWidth)
          num14 = num14 > num13 ? num13 + stripLine.Width : num13 - stripLine.Width;
        double start4 = num13 <= num2 ? num2 : (num13 >= num3 ? num3 : num13);
        double end = num14 <= num2 ? num2 : (num14 >= num3 ? num3 : num14);
        if (num15 > num16)
        {
          double num17 = num15;
          num15 = num16;
          num16 = num17;
        }
        Point vector1 = ChartTransform.ValueToVector(this.XAxis, num15);
        Point vector2 = ChartTransform.ValueToVector(this.XAxis, num16);
        double angle;
        ChartPolarGridLinesPanel.CalculateAngle(vector1, vector2, out angle);
        if (angle == 360.0)
        {
          ChartPolarGridLinesPanel.RenderEllipse(center, start4, end, out group);
          this.RenderStripLine((Geometry) group, stripLine);
        }
        else
        {
          PathGeometry pathGeometry;
          ChartPolarGridLinesPanel.RenderSegmentedPath(start4, end, angle, center, vector1, vector2, innerArc, outerArc, out pathGeometry);
          this.RenderStripLine((Geometry) pathGeometry, stripLine);
        }
        num4 = num4 < num6 ? num4 + repeatEvery : num4 - repeatEvery;
        double num18 = !double.IsNaN(stripLine.RepeatUntil) ? (num4 == stripLine.RepeatUntil ? num4 : (num4 < stripLine.RepeatUntil ? num4 + stripLine.Width : num4 - stripLine.Width)) : num4 + stripLine.Width;
        num8 = num18 < axis.VisibleRange.End ? num18 : axis.VisibleRange.End;
        if (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num4 >= num6 ? 0 : (num8 <= num6 ? 1 : 0)) : (num4 <= num6 ? 0 : (num8 >= num6 ? 1 : 0))) != 0)
          goto label_8;
      }
    }
  }

  private void UpdatePolarHorizontalStripLine(ChartAxis axis)
  {
    double num1 = axis.VisibleRange.End + axis.VisibleRange.Delta / (double) (axis.VisibleLabels.Count - 1);
    double radius = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point center = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) (axis as ChartAxisBase2D).StripLines)
    {
      bool flag = axis is NumericalAxis;
      double start1 = stripLine.Start;
      if (double.IsNaN(start1))
        break;
      double num2 = start1 < axis.VisibleRange.Start ? axis.VisibleRange.Start : (start1 > axis.VisibleRange.End ? (!flag ? axis.VisibleRange.End + 1.0 : axis.VisibleRange.End) : start1);
      double num3 = double.IsNaN(stripLine.RepeatUntil) ? axis.VisibleRange.End : stripLine.RepeatUntil;
      double num4 = !flag ? (num3 > num1 ? num1 : num3) : (num3 > axis.VisibleRange.End ? axis.VisibleRange.End : num3);
      double repeatEvery = stripLine.RepeatEvery;
      double num5 = !double.IsNaN(stripLine.RepeatUntil) ? (num2 == stripLine.RepeatUntil ? num2 : (num2 < stripLine.RepeatUntil ? num2 + stripLine.Width : num2 - stripLine.Width)) : num2 + stripLine.Width;
      double num6 = !flag ? (num5 > num1 ? num1 : num5) : (num5 > axis.VisibleRange.End ? axis.VisibleRange.End : num5);
      double angle;
      PathGeometry pathGeometry;
      if (!stripLine.IsSegmented)
      {
        do
        {
          Point vector1 = ChartTransform.ValueToVector(this.XAxis, num2);
          Point vector2 = ChartTransform.ValueToVector(this.XAxis, num6);
          if (num2 > num6)
          {
            Point point = vector1;
            vector1 = vector2;
            vector2 = point;
          }
          PathFigure pathFigure = new PathFigure();
          ArcSegment arc = new ArcSegment();
          ChartPolarGridLinesPanel.CalculateAngle(vector1, vector2, out angle);
          if (angle == 360.0)
          {
            this.RenderStripLine((Geometry) new EllipseGeometry()
            {
              Center = center,
              RadiusX = radius,
              RadiusY = radius
            }, stripLine);
          }
          else
          {
            System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
            pathGeometry = new PathGeometry();
            lineSegment.Point = new Point(center.X + radius * vector1.X, center.Y + radius * vector1.Y);
            pathFigure.StartPoint = new Point(center.X, center.Y);
            Point point = new Point(center.X + radius * vector2.X, center.Y + radius * vector2.Y);
            ChartPolarGridLinesPanel.RenderArc(radius, angle, point, out arc, SweepDirection.Clockwise);
            pathFigure.IsClosed = true;
            pathFigure.Segments.Add((PathSegment) lineSegment);
            pathFigure.Segments.Add((PathSegment) arc);
            pathGeometry.Figures.Add(pathFigure);
            this.RenderStripLine((Geometry) pathGeometry, stripLine);
          }
          num2 = num2 < num4 ? num2 + repeatEvery : num2 - repeatEvery;
          double num7 = !double.IsNaN(stripLine.RepeatUntil) ? (num2 == stripLine.RepeatUntil ? num2 : (num2 < stripLine.RepeatUntil ? num2 + stripLine.Width : num2 - stripLine.Width)) : num2 + stripLine.Width;
          num6 = !flag ? (num7 > num1 ? num1 : num7) : (num7 > axis.VisibleRange.End ? axis.VisibleRange.End : num7);
        }
        while (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num2 >= num4 ? 0 : (num6 <= num4 ? 1 : 0)) : (num2 <= num4 ? 0 : (num6 >= num4 ? 1 : 0))) != 0);
      }
      else
      {
label_11:
        ArcSegment innerArc = (ArcSegment) null;
        ArcSegment outerArc = (ArcSegment) null;
        double segmentStartValue = stripLine.SegmentStartValue;
        double segmentEndValue = stripLine.SegmentEndValue;
        double num8 = segmentEndValue > this.YAxis.VisibleRange.End ? this.YAxis.VisibleRange.End : (segmentEndValue < this.YAxis.VisibleRange.Start ? this.YAxis.VisibleRange.Start : segmentEndValue);
        double num9 = segmentStartValue < this.YAxis.VisibleRange.Start ? this.YAxis.VisibleRange.Start : (segmentStartValue > this.YAxis.VisibleRange.End ? this.YAxis.VisibleRange.End : segmentStartValue);
        Point vector1 = ChartTransform.ValueToVector(this.XAxis, num2);
        Point vector2 = ChartTransform.ValueToVector(this.XAxis, num6);
        if (num2 > num6)
        {
          Point point = vector1;
          vector1 = vector2;
          vector2 = point;
        }
        double num10 = radius * this.YAxis.ValueToCoefficientCalc(num9);
        double num11 = radius * this.YAxis.ValueToCoefficientCalc(num8);
        double start2 = num10 < 0.0 ? 0.0 : num10;
        double end = num11 < 0.0 ? 0.0 : num11;
        ChartPolarGridLinesPanel.CalculateAngle(vector1, vector2, out angle);
        if (angle == 360.0)
        {
          GeometryGroup group;
          ChartPolarGridLinesPanel.RenderEllipse(center, start2, end, out group);
          this.RenderStripLine((Geometry) group, stripLine);
        }
        else
        {
          ChartPolarGridLinesPanel.RenderSegmentedPath(start2, end, angle, center, vector1, vector2, innerArc, outerArc, out pathGeometry);
          this.RenderStripLine((Geometry) pathGeometry, stripLine);
        }
        num2 = num2 < num4 ? num2 + repeatEvery : num2 - repeatEvery;
        double num12 = !double.IsNaN(stripLine.RepeatUntil) ? (num2 == stripLine.RepeatUntil ? num2 : (num2 < stripLine.RepeatUntil ? num2 + stripLine.Width : num2 - stripLine.Width)) : num2 + stripLine.Width;
        num6 = num12 > axis.VisibleRange.End ? (!flag ? num12 : axis.VisibleRange.End) : num12;
        if (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num2 >= num4 ? 0 : (num6 <= num4 ? 1 : 0)) : (num2 <= num4 ? 0 : (num6 >= num4 ? 1 : 0))) != 0)
          goto label_11;
      }
    }
  }

  private void RenderPolarStripLines()
  {
    foreach (ChartAxis axis in this.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axis => axis != null && !axis.VisibleRange.IsEmpty && (axis as ChartAxisBase2D).StripLines != null && (axis as ChartAxisBase2D).StripLines.Count > 0)))
    {
      if (axis.Orientation == Orientation.Vertical)
        this.UpdatePolarVerticalStripLine(axis);
      else if (axis.Orientation == Orientation.Horizontal)
        this.UpdatePolarHorizontalStripLine(axis);
    }
  }

  private void RenderGridLines()
  {
    ChartAxis xaxis = this.XAxis;
    ChartAxis yaxis = this.YAxis;
    double num1 = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point point1 = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    int index1 = 0;
    if (this.IsRadar && yaxis != null && yaxis.ShowGridLines)
    {
      foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) yaxis.VisibleLabels)
      {
        double num2 = num1 * yaxis.ValueToCoefficientCalc(visibleLabel.Position);
        for (int index2 = 0; index2 < xaxis.VisibleLabels.Count; ++index2)
        {
          Point vector = ChartTransform.ValueToVector(xaxis, xaxis.VisibleLabels[index2].Position);
          Point point2 = new Point();
          Point point3 = index2 + 1 >= xaxis.VisibleLabels.Count ? ChartTransform.ValueToVector(xaxis, xaxis.VisibleLabels[0].Position) : ChartTransform.ValueToVector(xaxis, xaxis.VisibleLabels[index2 + 1].Position);
          Point point4 = new Point(point1.X + num2 * vector.X, point1.Y + num2 * vector.Y);
          Point point5 = new Point(point1.X + num2 * point3.X, point1.Y + num2 * point3.Y);
          Line line = this.ylinesRecycler[index1];
          line.X1 = point4.X;
          line.Y1 = point4.Y;
          line.X2 = point5.X;
          line.Y2 = point5.Y;
          ++index1;
        }
      }
    }
    if (xaxis != null && xaxis.ShowGridLines)
    {
      int index3 = 0;
      foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) xaxis.VisibleLabels)
      {
        Point vector = ChartTransform.ValueToVector(xaxis, visibleLabel.Position);
        Line line = this.linesRecycler[index3];
        line.X1 = point1.X;
        line.Y1 = point1.Y;
        line.X2 = point1.X + num1 * vector.X;
        line.Y2 = point1.Y + num1 * vector.Y;
        ++index3;
      }
    }
    this.UpdateStripLines();
  }

  private void RenderRadarStripLines()
  {
    foreach (ChartAxis axis in this.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axis => axis != null && !axis.VisibleRange.IsEmpty && (axis as ChartAxisBase2D).StripLines != null && (axis as ChartAxisBase2D).StripLines.Count > 0)))
    {
      if (axis.Orientation == Orientation.Vertical)
        this.UpdateRadarVerticalStripLine(axis);
      else if (axis.Orientation == Orientation.Horizontal)
        this.UpdateRadarHorizontalStripLine(axis);
    }
  }

  private void UpdateRadarHorizontalStripLine(ChartAxis axis)
  {
    double num1 = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point point1 = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    bool flag1 = this.XAxis is NumericalAxis;
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) (axis as ChartAxisBase2D).StripLines)
    {
      bool flag2 = axis is NumericalAxis;
      int num2 = 0;
      int num3 = 0;
      bool flag3 = false;
      DoubleRange visibleRange = this.XAxis.VisibleRange;
      double num4 = visibleRange.End + this.XAxis.VisibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
      double start1 = stripLine.Start;
      if (double.IsNaN(start1))
        break;
      double num5 = start1 < axis.VisibleRange.Start ? axis.VisibleRange.Start : start1;
      double num6 = double.IsNaN(stripLine.RepeatUntil) ? (!flag2 ? num4 : axis.VisibleRange.End) : stripLine.RepeatUntil;
      double num7;
      if (flag1)
      {
        double num8 = num6;
        visibleRange = this.XAxis.VisibleRange;
        double end = visibleRange.End;
        if (num8 <= end)
        {
          num7 = num6;
        }
        else
        {
          visibleRange = this.XAxis.VisibleRange;
          num7 = visibleRange.End;
        }
      }
      else
        num7 = num6 > num4 ? num4 : num6;
      double num9 = num7;
      double repeatEvery = stripLine.RepeatEvery;
      double num10 = !double.IsNaN(stripLine.RepeatUntil) ? (num5 == stripLine.RepeatUntil ? num5 : (num5 < stripLine.RepeatUntil ? num5 + stripLine.Width : num5 - stripLine.Width)) : num5 + stripLine.Width;
      double num11;
      if (flag2)
      {
        double num12 = num10;
        visibleRange = axis.VisibleRange;
        double end = visibleRange.End;
        if (num12 <= end)
        {
          num11 = num10;
        }
        else
        {
          visibleRange = axis.VisibleRange;
          num11 = visibleRange.End;
        }
      }
      else
        num11 = num10 > num4 ? num4 : num10;
      double num13 = num11;
      if (!stripLine.IsSegmented)
      {
        do
        {
          int num14 = 0;
          foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.XAxis.VisibleLabels)
          {
            if (Math.Abs(num5 - visibleLabel.Position) < 0.001)
              num2 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
            if (Math.Abs(num13 - visibleLabel.Position) < 0.001)
              num3 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
          }
          PathFigure pathFigure = new PathFigure();
          PathGeometry data = new PathGeometry();
          if (!flag1 && num5 <= this.XAxis.VisibleRange.End)
            num3 = num13 > this.XAxis.VisibleRange.End ? this.XAxis.VisibleLabels.Count : num3;
          if (num5 > num13)
          {
            int num15 = num2;
            num2 = num3;
            num3 = num15;
          }
          if (!flag1 && num5 == num4 && num13 == this.XAxis.VisibleRange.End)
            num3 = this.XAxis.VisibleLabels.Count;
          for (int index1 = num2; index1 < num3; ++index1)
          {
            int index2 = index1;
            if (index2 >= this.XAxis.VisibleLabels.Count - 1)
            {
              index2 -= this.XAxis.VisibleLabels.Count - 1;
              flag3 = true;
            }
            int index3 = index2 + 1;
            if (!flag1)
            {
              double num16 = num5;
              visibleRange = axis.VisibleRange;
              double end1 = visibleRange.End;
              if (num16 == end1)
              {
                double num17 = num13;
                visibleRange = axis.VisibleRange;
                double end2 = visibleRange.End;
                if (num17 > end2)
                  goto label_38;
              }
              if (num5 == num4)
              {
                double num18 = num13;
                visibleRange = axis.VisibleRange;
                double end3 = visibleRange.End;
                if (num18 != end3)
                  goto label_39;
              }
              else
                goto label_39;
label_38:
              index2 = axis.VisibleLabels.Count - 1;
              index3 = 0;
              flag3 = false;
            }
label_39:
            Point vector1 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index2].Position);
            Point vector2 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index3].Position);
            Point point2 = new Point(point1.X + num1 * vector1.X, point1.Y + num1 * vector1.Y);
            Point point3 = new Point(point1.X + num1 * vector2.X, point1.Y + num1 * vector2.Y);
            System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
            if (num14 == 0)
              pathFigure.StartPoint = point1;
            lineSegment.Point = point2;
            pathFigure.Segments.Add((PathSegment) lineSegment);
            if (!flag3)
              pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
              {
                Point = point3
              });
            ++num14;
            flag3 = false;
          }
          data.Figures.Add(pathFigure);
          pathFigure.IsClosed = true;
          this.RenderStripLine((Geometry) data, stripLine);
          num5 = num5 < num9 ? num5 + repeatEvery : num5 - repeatEvery;
          double num19 = !double.IsNaN(stripLine.RepeatUntil) ? (num5 == stripLine.RepeatUntil ? num5 : (num5 < stripLine.RepeatUntil ? num5 + stripLine.Width : num5 - stripLine.Width)) : num5 + stripLine.Width;
          double num20;
          if (flag2)
          {
            double num21 = num19;
            visibleRange = axis.VisibleRange;
            double end = visibleRange.End;
            if (num21 <= end)
            {
              num20 = num19;
            }
            else
            {
              visibleRange = axis.VisibleRange;
              num20 = visibleRange.End;
            }
          }
          else
            num20 = num19 > num4 ? num4 : num19;
          num13 = num20;
        }
        while (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num5 >= num9 ? 0 : (num13 <= num9 ? 1 : 0)) : (num5 <= num9 ? 0 : (num13 >= num9 ? 1 : 0))) != 0);
      }
      else
      {
label_51:
        Point point4 = new Point(0.0, 0.0);
        Point point5 = new Point(0.0, 0.0);
        PathFigure pathFigure = new PathFigure();
        PathGeometry data = new PathGeometry();
        bool flag4 = false;
        int num22 = 0;
        foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.XAxis.VisibleLabels)
        {
          if (Math.Abs(num5 - visibleLabel.Position) < 0.001)
            num2 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
          if (Math.Abs(num13 - visibleLabel.Position) < 0.001)
            num3 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
        }
        if (!flag1 && num5 <= this.XAxis.VisibleRange.End)
          num3 = num13 > this.XAxis.VisibleRange.End ? this.XAxis.VisibleLabels.Count : num3;
        if (num5 > num13)
        {
          int num23 = num2;
          num2 = num3;
          num3 = num23;
        }
        if (!flag1 && num5 == num4 && num13 == this.XAxis.VisibleRange.End)
          num3 = this.XAxis.VisibleLabels.Count;
        double segmentStartValue = stripLine.SegmentStartValue;
        double segmentEndValue = stripLine.SegmentEndValue;
        double num24;
        if (segmentEndValue <= this.YAxis.VisibleRange.End)
        {
          if (segmentEndValue >= this.YAxis.VisibleRange.Start)
          {
            num24 = segmentEndValue;
          }
          else
          {
            visibleRange = this.YAxis.VisibleRange;
            num24 = visibleRange.Start;
          }
        }
        else
        {
          visibleRange = this.YAxis.VisibleRange;
          num24 = visibleRange.End;
        }
        double num25 = num24;
        double num26 = segmentStartValue;
        visibleRange = this.YAxis.VisibleRange;
        double start2 = visibleRange.Start;
        double num27;
        if (num26 >= start2)
        {
          double num28 = segmentStartValue;
          visibleRange = this.YAxis.VisibleRange;
          double end = visibleRange.End;
          if (num28 <= end)
          {
            num27 = segmentStartValue;
          }
          else
          {
            visibleRange = this.YAxis.VisibleRange;
            num27 = visibleRange.End;
          }
        }
        else
        {
          visibleRange = this.YAxis.VisibleRange;
          num27 = visibleRange.Start;
        }
        double num29 = num27;
        double num30 = num1 * this.YAxis.ValueToCoefficientCalc(num29);
        double num31 = num1 * this.YAxis.ValueToCoefficientCalc(num25);
        for (int index4 = num2; index4 < num3; ++index4)
        {
          int index5 = index4;
          flag3 = false;
          if (index5 >= this.XAxis.VisibleLabels.Count - 1)
          {
            index5 -= this.XAxis.VisibleLabels.Count - 1;
            if (!flag1)
              flag3 = true;
          }
          int index6 = index5 + 1;
          num30 = num30 < 0.0 ? 0.0 : num30;
          num31 = num31 < 0.0 ? 0.0 : num31;
          if (!flag1)
          {
            double num32 = num5;
            visibleRange = axis.VisibleRange;
            double end4 = visibleRange.End;
            if (num32 == end4)
            {
              double num33 = num13;
              visibleRange = axis.VisibleRange;
              double end5 = visibleRange.End;
              if (num33 > end5)
                goto label_87;
            }
            if (num5 == num4)
            {
              double num34 = num13;
              visibleRange = axis.VisibleRange;
              double end6 = visibleRange.End;
              if (num34 != end6)
                goto label_88;
            }
            else
              goto label_88;
label_87:
            index5 = axis.VisibleLabels.Count - 1;
            index6 = 0;
            flag3 = false;
          }
label_88:
          Point vector3 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index5].Position);
          Point vector4 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index6].Position);
          Point point6 = new Point(point1.X + num30 * vector3.X, point1.Y + num30 * vector3.Y);
          Point point7 = new Point(point1.X + num30 * vector4.X, point1.Y + num30 * vector4.Y);
          point4 = new Point(point1.X + num31 * vector3.X, point1.Y + num31 * vector3.Y);
          point5 = new Point(point1.X + num31 * vector4.X, point1.Y + num31 * vector4.Y);
          System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
          lineSegment.Point = flag3 ? point6 : point7;
          if (num22 == 0)
            pathFigure.StartPoint = point6;
          pathFigure.Segments.Add((PathSegment) lineSegment);
          ++num22;
        }
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = (flag1 || !flag3 ? point5 : point4)
        });
        for (int index7 = num3; index7 > num2; --index7)
        {
          int index8 = index7;
          bool flag5 = false;
          if (index8 > this.XAxis.VisibleLabels.Count - 1)
          {
            index8 -= this.XAxis.VisibleLabels.Count - 1;
            if (!flag1)
              flag5 = true;
          }
          int index9 = index8 - 1;
          if (!flag1)
          {
            double num35 = num5;
            visibleRange = axis.VisibleRange;
            double end7 = visibleRange.End;
            if (num35 == end7)
            {
              double num36 = num13;
              visibleRange = axis.VisibleRange;
              double start3 = visibleRange.Start;
              if (num36 != start3)
              {
                double num37 = num13;
                visibleRange = axis.VisibleRange;
                double end8 = visibleRange.End;
                if (num37 > end8)
                  goto label_102;
              }
              else
                goto label_102;
            }
            if (num5 == num4)
            {
              double num38 = num13;
              visibleRange = axis.VisibleRange;
              double end9 = visibleRange.End;
              if (num38 != end9)
                goto label_103;
            }
            else
              goto label_103;
label_102:
            index9 = 0;
            index8 = axis.VisibleLabels.Count - 1;
            flag4 = true;
            flag5 = false;
          }
label_103:
          Point vector5 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index8].Position);
          Point vector6 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index9].Position);
          point4 = new Point(point1.X + num31 * vector5.X, point1.Y + num31 * vector5.Y);
          point5 = new Point(point1.X + num31 * vector6.X, point1.Y + num31 * vector6.Y);
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = (flag1 || flag5 ? point5 : point4)
          });
          flag3 = false;
        }
        if (!flag1 && !flag4)
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = point5
          });
        pathFigure.IsClosed = true;
        data.Figures.Add(pathFigure);
        this.RenderStripLine((Geometry) data, stripLine);
        num5 = num5 < num9 ? num5 + repeatEvery : num5 - repeatEvery;
        double num39 = !double.IsNaN(stripLine.RepeatUntil) ? (num5 == stripLine.RepeatUntil ? num5 : (num5 < stripLine.RepeatUntil ? num5 + stripLine.Width : num5 - stripLine.Width)) : num5 + stripLine.Width;
        double num40;
        if (flag2)
        {
          double num41 = num39;
          visibleRange = axis.VisibleRange;
          double end = visibleRange.End;
          if (num41 <= end)
          {
            num40 = num39;
          }
          else
          {
            visibleRange = axis.VisibleRange;
            num40 = visibleRange.End;
          }
        }
        else
          num40 = num39 > num4 ? num4 : num39;
        num13 = num40;
        if (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num5 >= num9 ? 0 : (num13 <= num9 ? 1 : 0)) : (num5 <= num9 ? 0 : (num13 >= num9 ? 1 : 0))) != 0)
          goto label_51;
      }
    }
  }

  private void UpdateRadarVerticalStripLine(ChartAxis axis)
  {
    double num1 = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2.0;
    Point point1 = new Point(this.DesiredSize.Width / 2.0, this.DesiredSize.Height / 2.0);
    double num2 = num1 * axis.ValueToCoefficientCalc(axis.VisibleRange.Start);
    double num3 = num1 * axis.ValueToCoefficientCalc(axis.VisibleRange.End);
    bool flag1 = this.XAxis is NumericalAxis;
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) (axis as ChartAxisBase2D).StripLines)
    {
      double start = stripLine.Start;
      if (double.IsNaN(start))
        break;
      double num4 = start;
      DoubleRange visibleRange = axis.VisibleRange;
      double end1 = visibleRange.End;
      double num5;
      if (num4 <= end1)
      {
        num5 = start;
      }
      else
      {
        visibleRange = axis.VisibleRange;
        num5 = visibleRange.End;
      }
      double num6 = num5;
      double num7;
      if (!double.IsNaN(stripLine.RepeatUntil))
      {
        num7 = stripLine.RepeatUntil;
      }
      else
      {
        visibleRange = axis.VisibleRange;
        num7 = visibleRange.End;
      }
      double num8 = num7;
      double repeatEvery = stripLine.RepeatEvery;
      double num9 = num8;
      visibleRange = axis.VisibleRange;
      double end2 = visibleRange.End;
      double num10;
      if (num9 <= end2)
      {
        num10 = num8;
      }
      else
      {
        visibleRange = axis.VisibleRange;
        num10 = visibleRange.End;
      }
      double num11 = num10;
      double num12 = !double.IsNaN(stripLine.RepeatUntil) ? (num6 == stripLine.RepeatUntil ? num6 : (num6 < stripLine.RepeatUntil ? num6 + stripLine.Width : num6 - stripLine.Width)) : num6 + stripLine.Width;
      double num13 = num12;
      visibleRange = axis.VisibleRange;
      double end3 = visibleRange.End;
      double num14;
      if (num13 >= end3)
      {
        visibleRange = axis.VisibleRange;
        num14 = visibleRange.End;
      }
      else
        num14 = num12;
      double num15 = num14;
      if (!stripLine.IsSegmented)
      {
        do
        {
          PathFigure pathFigure1 = new PathFigure();
          PathFigure pathFigure2 = new PathFigure();
          PathGeometry pathGeometry1 = new PathGeometry();
          PathGeometry pathGeometry2 = new PathGeometry();
          GeometryGroup data = new GeometryGroup();
          for (int index = 0; index < this.XAxis.VisibleLabels.Count; ++index)
          {
            double num16 = num1 * axis.ValueToCoefficientCalc(num6);
            double num17 = num1 * axis.ValueToCoefficientCalc(num15);
            if (stripLine.IsPixelWidth)
              num17 = num16 + stripLine.Width;
            double num18 = num16 <= num2 ? num2 : (num16 >= num3 ? num3 : num16);
            double num19 = num17 <= num2 ? num2 : (num17 >= num3 ? num3 : num17);
            Point vector1 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index].Position);
            Point point2 = new Point();
            Point vector2 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[0].Position);
            Point point3 = new Point(point1.X + num18 * vector2.X, point1.Y + num18 * vector2.Y);
            Point point4 = new Point(point1.X + num19 * vector2.X, point1.Y + num19 * vector2.Y);
            Point point5 = index + 1 >= this.XAxis.VisibleLabels.Count ? ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[0].Position) : ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index + 1].Position);
            Point point6 = new Point(point1.X + num18 * vector1.X, point1.Y + num18 * vector1.Y);
            Point point7 = new Point(point1.X + num18 * point5.X, point1.Y + num18 * point5.Y);
            Point point8 = new Point(point1.X + num19 * vector1.X, point1.Y + num19 * vector1.Y);
            Point point9 = new Point(point1.X + num19 * point5.X, point1.Y + num19 * point5.Y);
            pathFigure2.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
            {
              Point = point8
            });
            System.Windows.Media.LineSegment lineSegment1 = new System.Windows.Media.LineSegment();
            lineSegment1.Point = point9;
            pathFigure2.StartPoint = point4;
            pathFigure2.Segments.Add((PathSegment) lineSegment1);
            pathFigure1.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
            {
              Point = point6
            });
            System.Windows.Media.LineSegment lineSegment2 = new System.Windows.Media.LineSegment();
            lineSegment2.Point = point7;
            pathFigure1.StartPoint = point3;
            pathFigure1.Segments.Add((PathSegment) lineSegment2);
          }
          num6 = num6 < num11 ? num6 + repeatEvery : num6 - repeatEvery;
          double num20 = !double.IsNaN(stripLine.RepeatUntil) ? (num6 == stripLine.RepeatUntil ? num6 : (num6 < stripLine.RepeatUntil ? num6 + stripLine.Width : num6 - stripLine.Width)) : num6 + stripLine.Width;
          pathGeometry1.Figures.Add(pathFigure1);
          pathGeometry2.Figures.Add(pathFigure2);
          data.Children.Add((Geometry) pathGeometry1);
          data.Children.Add((Geometry) pathGeometry2);
          this.RenderStripLine((Geometry) data, stripLine);
          double num21 = num20;
          visibleRange = axis.VisibleRange;
          double end4 = visibleRange.End;
          double num22;
          if (num21 >= end4)
          {
            visibleRange = axis.VisibleRange;
            num22 = visibleRange.End;
          }
          else
            num22 = num20;
          num15 = num22;
        }
        while (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num6 >= num11 ? 0 : (num15 <= num11 ? 1 : 0)) : (num6 <= num11 ? 0 : (num15 >= num11 ? 1 : 0))) != 0);
      }
      else
      {
label_25:
        Point point10 = new Point(0.0, 0.0);
        Point point11 = new Point(0.0, 0.0);
        int num23 = 0;
        int num24 = 0;
        int num25 = 0;
        PathFigure pathFigure = new PathFigure();
        PathGeometry data = new PathGeometry();
        bool flag2 = false;
        double segmentStartValue = stripLine.SegmentStartValue;
        double segmentEndValue = stripLine.SegmentEndValue;
        double num26;
        if (flag1)
        {
          double num27 = segmentStartValue;
          visibleRange = this.XAxis.VisibleRange;
          double end5 = visibleRange.End;
          if (num27 <= end5)
          {
            num26 = segmentStartValue;
          }
          else
          {
            visibleRange = this.XAxis.VisibleRange;
            num26 = visibleRange.End;
          }
        }
        else
        {
          double num28 = segmentStartValue;
          visibleRange = this.XAxis.VisibleRange;
          double end6 = visibleRange.End;
          visibleRange = this.XAxis.VisibleRange;
          double num29 = visibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
          double num30 = end6 + num29;
          if (num28 <= num30)
          {
            num26 = segmentStartValue;
          }
          else
          {
            visibleRange = this.XAxis.VisibleRange;
            double end7 = visibleRange.End;
            visibleRange = this.XAxis.VisibleRange;
            double num31 = visibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
            num26 = end7 + num31;
          }
        }
        double num32 = num26;
        double num33;
        if (flag1)
        {
          double num34 = segmentEndValue;
          visibleRange = this.XAxis.VisibleRange;
          double end8 = visibleRange.End;
          if (num34 <= end8)
          {
            num33 = segmentEndValue;
          }
          else
          {
            visibleRange = this.XAxis.VisibleRange;
            num33 = visibleRange.End;
          }
        }
        else
        {
          double num35 = segmentEndValue;
          visibleRange = this.XAxis.VisibleRange;
          double end9 = visibleRange.End;
          visibleRange = this.XAxis.VisibleRange;
          double num36 = visibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
          double num37 = end9 + num36;
          if (num35 <= num37)
          {
            num33 = segmentEndValue;
          }
          else
          {
            visibleRange = this.XAxis.VisibleRange;
            double end10 = visibleRange.End;
            visibleRange = this.XAxis.VisibleRange;
            double num38 = visibleRange.Delta / (double) (this.XAxis.VisibleLabels.Count - 1);
            num33 = end10 + num38;
          }
        }
        double num39 = num33;
        if (num32 > num39)
        {
          double num40 = num32;
          num32 = num39;
          num39 = num40;
        }
        foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.XAxis.VisibleLabels)
        {
          if (Math.Abs(num32 - visibleLabel.Position) < 0.001)
            num23 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
          if (Math.Abs(num39 - visibleLabel.Position) < 0.001)
            num24 = this.XAxis.VisibleLabels.IndexOf(visibleLabel);
        }
        if (!flag1 && num32 <= this.XAxis.VisibleRange.End)
          num24 = num39 > this.XAxis.VisibleRange.End ? this.XAxis.VisibleLabels.Count : num24;
        double num41 = num1 * axis.ValueToCoefficientCalc(num6);
        double num42 = num1 * axis.ValueToCoefficientCalc(num15);
        for (int index1 = num23; index1 < num24; ++index1)
        {
          int index2 = index1;
          flag2 = false;
          if (index2 >= this.XAxis.VisibleLabels.Count - 1)
          {
            index2 -= this.XAxis.VisibleLabels.Count - 1;
            if (!flag1)
              flag2 = true;
          }
          if (stripLine.IsPixelWidth)
            num42 = num42 > num41 ? num41 + stripLine.Width : num41 - stripLine.Width;
          int index3 = index2 + 1;
          if (!flag1 && (double) num23 == this.XAxis.VisibleRange.End && (double) num24 > this.XAxis.VisibleRange.End)
          {
            index2 = num23;
            index3 = 0;
          }
          num41 = num41 <= num2 ? num2 : (num41 >= num3 ? num3 : num41);
          num42 = num42 <= num2 ? num2 : (num42 >= num3 ? num3 : num42);
          Point vector3 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index2].Position);
          Point vector4 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index3].Position);
          Point point12 = new Point(point1.X + num41 * vector3.X, point1.Y + num41 * vector3.Y);
          Point point13 = new Point(point1.X + num41 * vector4.X, point1.Y + num41 * vector4.Y);
          point10 = new Point(point1.X + num42 * vector3.X, point1.Y + num42 * vector3.Y);
          point11 = new Point(point1.X + num42 * vector4.X, point1.Y + num42 * vector4.Y);
          System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
          lineSegment.Point = flag1 || index2 != 0 || !flag2 ? point13 : point12;
          if (num25 == 0)
            pathFigure.StartPoint = point12;
          pathFigure.Segments.Add((PathSegment) lineSegment);
          ++num25;
        }
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = (flag1 || !flag2 || (double) num23 == this.XAxis.VisibleRange.End ? point11 : point10)
        });
        bool flag3 = false;
        for (int index4 = num24; index4 > num23; --index4)
        {
          int index5 = index4;
          if (index5 >= this.XAxis.VisibleLabels.Count)
            index5 -= this.XAxis.VisibleLabels.Count - 1;
          if (!flag1 && index5 == this.XAxis.VisibleLabels.Count - 1)
            flag3 = true;
          if (stripLine.IsPixelWidth)
            num42 = num42 > num41 ? num41 + stripLine.Width : num41 - stripLine.Width;
          num42 = num42 <= num2 ? num2 : (num42 >= num3 ? num3 : num42);
          int index6 = index5 - 1;
          if (!flag1 && (double) num23 == this.XAxis.VisibleRange.End && (double) num24 > this.XAxis.VisibleRange.End)
          {
            index5 = 0;
            index6 = this.XAxis.VisibleLabels.Count - 1;
          }
          Point vector5 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index5].Position);
          Point vector6 = ChartTransform.ValueToVector(this.XAxis, this.XAxis.VisibleLabels[index6].Position);
          point10 = new Point(point1.X + num42 * vector5.X, point1.Y + num42 * vector5.Y);
          point11 = new Point(point1.X + num42 * vector6.X, point1.Y + num42 * vector6.Y);
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = (flag1 || !flag3 ? point11 : point10)
          });
        }
        if (!flag1)
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = point11
          });
        pathFigure.IsClosed = true;
        data.Figures.Add(pathFigure);
        this.RenderStripLine((Geometry) data, stripLine);
        num6 = num6 < num11 ? num6 + repeatEvery : num6 - repeatEvery;
        double num43 = !double.IsNaN(stripLine.RepeatUntil) ? (num6 == stripLine.RepeatUntil ? num6 : (num6 < stripLine.RepeatUntil ? num6 + stripLine.Width : num6 - stripLine.Width)) : num6 + stripLine.Width;
        double num44;
        if (num43 >= axis.VisibleRange.End)
        {
          visibleRange = axis.VisibleRange;
          num44 = visibleRange.End;
        }
        else
          num44 = num43;
        num15 = num44;
        if (repeatEvery != 0.0 && !double.IsNaN(stripLine.RepeatUntil) && (stripLine.Start < stripLine.RepeatUntil ? (num6 >= num11 ? 0 : (num15 <= num11 ? 1 : 0)) : (num6 <= num11 ? 0 : (num15 >= num11 ? 1 : 0))) != 0)
          goto label_25;
      }
    }
  }
}
