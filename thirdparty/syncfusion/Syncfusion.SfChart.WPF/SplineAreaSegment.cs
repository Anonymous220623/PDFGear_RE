// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SplineAreaSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SplineAreaSegment : AreaSegment
{
  internal List<ChartPoint> segmentPoints = new List<ChartPoint>();
  private Canvas segmentCanvas;
  private bool isEmpty;
  private Path segPath;
  private bool isSegmentUpdated;
  private PathGeometry strokeGeometry;
  private PathFigure strokeFigure;
  private Path strokePath;

  public SplineAreaSegment()
  {
  }

  [Obsolete("Use SplineAreaSegment(List<ChartPoint> Points, List<double> xValues, IList<double> yValues, SplineAreaSeries series): base(xValues, yValues)")]
  public SplineAreaSegment(
    List<Point> Points,
    List<double> xValues,
    IList<double> yValues,
    SplineAreaSeries series)
    : base(xValues, yValues)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = (object) series.ActualData;
    this.SetData(Points, xValues, yValues);
  }

  public SplineAreaSegment(
    List<ChartPoint> Points,
    List<double> xValues,
    IList<double> yValues,
    SplineAreaSeries series)
    : base(xValues, yValues)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = (object) series.ActualData;
    this.SetData(Points, xValues, yValues);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segmentCanvas = new Canvas();
    this.segPath = new Path();
    this.segPath.Tag = (object) this;
    this.SetVisualBindings((Shape) this.segPath);
    this.segmentCanvas.Children.Add((UIElement) this.segPath);
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segPath;

  public override void Update(IChartTransformer transformer)
  {
    SplineAreaSeries series = this.Series as SplineAreaSeries;
    if (this.isSegmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double end = cartesianTransformer.XAxis.VisibleRange.End;
    DoubleRange visibleRange = cartesianTransformer.XAxis.VisibleRange;
    PathFigure pathFigure = new PathFigure();
    PathGeometry pathGeometry = new PathGeometry();
    double y = series.ActualXAxis != null ? series.ActualXAxis.Origin : 0.0;
    pathFigure.StartPoint = transformer.TransformToVisible(this.segmentPoints[0].X, this.segmentPoints[0].Y);
    pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
    {
      Point = transformer.TransformToVisible(this.segmentPoints[1].X, this.segmentPoints[1].Y)
    });
    this.strokeGeometry = new PathGeometry();
    this.strokeFigure = new PathFigure();
    this.strokePath = new Path();
    if (series.IsClosed && this.isEmpty)
    {
      this.AddStroke(pathFigure.StartPoint);
      this.strokeFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = transformer.TransformToVisible(this.segmentPoints[1].X, this.segmentPoints[1].Y)
      });
    }
    else if (!series.IsClosed)
      this.AddStroke(transformer.TransformToVisible(this.segmentPoints[1].X, this.segmentPoints[1].Y));
    int index;
    for (index = 2; index < this.segmentPoints.Count; index += 3)
    {
      double x = this.segmentPoints[index].X;
      if (x >= visibleRange.Start && x <= visibleRange.End || end >= visibleRange.Start && end <= visibleRange.End)
      {
        if (this.Series.ShowEmptyPoints || !double.IsNaN(this.segmentPoints[index].Y) && !double.IsNaN(this.segmentPoints[index + 1].Y) && !double.IsNaN(this.segmentPoints[index + 2].Y))
        {
          BezierSegment bezierSegment = new BezierSegment();
          bezierSegment.Point1 = transformer.TransformToVisible(this.segmentPoints[index].X, this.segmentPoints[index].Y);
          bezierSegment.Point2 = transformer.TransformToVisible(this.segmentPoints[index + 1].X, this.segmentPoints[index + 1].Y);
          bezierSegment.Point3 = transformer.TransformToVisible(this.segmentPoints[index + 2].X, this.segmentPoints[index + 2].Y);
          pathFigure.Segments.Add((PathSegment) bezierSegment);
          if (this.isEmpty && !this.Series.ShowEmptyPoints || !series.IsClosed)
            this.strokeFigure.Segments.Add((PathSegment) new BezierSegment()
            {
              Point1 = bezierSegment.Point1,
              Point2 = bezierSegment.Point2,
              Point3 = bezierSegment.Point3
            });
        }
        else if (double.IsNaN(this.segmentPoints[index].Y) && double.IsNaN(this.segmentPoints[index + 1].Y) && double.IsNaN(this.segmentPoints[index + 2].Y))
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.segmentPoints[index - 1].X, y)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.segmentPoints[index + 2].X, y)
          });
        }
        else if (index > 0 && (double.IsNaN(this.segmentPoints[index - 1].Y) || double.IsNaN(this.segmentPoints[index].Y)))
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.segmentPoints[index + 2].X, y)
          });
          System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
          lineSegment.Point = transformer.TransformToVisible(this.segmentPoints[index + 2].X, double.IsNaN(this.segmentPoints[index + 2].Y) ? y : this.segmentPoints[index + 2].Y);
          if (!this.Series.ShowEmptyPoints && !double.IsNaN(this.segmentPoints[index + 2].Y) || !series.IsClosed)
          {
            this.strokeFigure = new PathFigure();
            this.strokeFigure.StartPoint = lineSegment.Point;
            this.strokeGeometry.Figures.Add(this.strokeFigure);
          }
          pathFigure.Segments.Add((PathSegment) lineSegment);
        }
      }
    }
    Point visible = transformer.TransformToVisible(this.segmentPoints[index - 1].X, y);
    pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
    {
      Point = visible
    });
    if (series.IsClosed && !double.IsNaN(this.segmentPoints[index - 1].Y))
      this.strokeFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = visible
      });
    this.isSegmentUpdated = true;
    pathGeometry.Figures.Add(pathFigure);
    this.segPath.Data = (Geometry) pathGeometry;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  [Obsolete("Use SetData(List<ChartPoint> points, List<double> xValues, IList<double> yValues)")]
  public void SetData(List<Point> points, List<double> xValues, IList<double> yValues)
  {
    this.SetData((IList<double>) xValues, yValues);
    List<ChartPoint> chartPointList = new List<ChartPoint>();
    foreach (Point point in points)
      chartPointList.Add(new ChartPoint(point.X, point.Y));
    this.segmentPoints = chartPointList;
    double d = points.Min<Point>((Func<Point, double>) (item => item.Y));
    double start = double.IsNaN(d) ? points.Where<Point>((Func<Point, bool>) (item => !double.IsNaN(item.Y))).Min<Point>((Func<Point, double>) (item => item.Y)) : d;
    this.isEmpty = double.IsNaN(d);
    this.XRange = new DoubleRange(points.Min<Point>((Func<Point, double>) (item => item.X)), points.Max<Point>((Func<Point, double>) (item => item.X)));
    this.YRange = new DoubleRange(start, points.Max<Point>((Func<Point, double>) (item => item.Y)));
    if (this.isEmpty || this.segPath == null)
      return;
    this.segPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  public void SetData(List<ChartPoint> points, List<double> xValues, IList<double> yValues)
  {
    this.SetData((IList<double>) xValues, yValues);
    this.segmentPoints = points;
    double d = points.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    double start = double.IsNaN(d) ? points.Where<ChartPoint>((Func<ChartPoint, bool>) (item => !double.IsNaN(item.Y))).Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y)) : d;
    this.isEmpty = double.IsNaN(d);
    this.XRange = new DoubleRange(points.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.X)), points.Max<ChartPoint>((Func<ChartPoint, double>) (item => item.X)));
    this.YRange = new DoubleRange(start, points.Max<ChartPoint>((Func<ChartPoint, double>) (item => item.Y)));
    if (this.isEmpty || this.segPath == null)
      return;
    this.segPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  protected override void SetVisualBindings(Shape element)
  {
    base.SetVisualBindings(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
  }

  private void AddStroke(Point startPoint)
  {
    if (this.segmentCanvas.Children.Count > 1)
      this.segmentCanvas.Children.RemoveAt(1);
    this.segPath.StrokeThickness = 0.0;
    this.strokeFigure.StartPoint = startPoint;
    this.strokeGeometry.Figures.Add(this.strokeFigure);
    this.strokePath.Data = (Geometry) this.strokeGeometry;
    this.strokePath.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
    this.strokePath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
    this.segmentCanvas.Children.Add((UIElement) this.strokePath);
  }

  internal override void Dispose()
  {
    if (this.segmentCanvas != null)
    {
      this.segmentCanvas.Children.Clear();
      this.segmentCanvas = (Canvas) null;
    }
    if (this.segPath != null)
    {
      this.segPath.Tag = (object) null;
      this.segPath = (Path) null;
    }
    base.Dispose();
  }
}
