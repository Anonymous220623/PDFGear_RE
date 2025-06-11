// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SplineRangeAreaSegment
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

public class SplineRangeAreaSegment : RangeAreaSegment
{
  private List<ChartPoint> AreaPoints;
  private Path segPath;
  private Canvas segmentCanvas;

  public SplineRangeAreaSegment()
  {
  }

  [Obsolete("Use SplineRangeAreaSegment(List<ChartPoint> AreaPoints, SplineRangeAreaSeries series)")]
  public SplineRangeAreaSegment(List<Point> AreaPoints, SplineRangeAreaSeries series)
  {
    this.Series = (ChartSeriesBase) series;
  }

  public SplineRangeAreaSegment(List<ChartPoint> AreaPoints, SplineRangeAreaSeries series)
  {
    this.Series = (ChartSeriesBase) series;
  }

  [Obsolete("Use SetData(List<ChartPoint> AreaPoints)")]
  public override void SetData(List<Point> AreaPoints)
  {
    List<ChartPoint> chartPointList = new List<ChartPoint>();
    foreach (Point areaPoint in AreaPoints)
      chartPointList.Add(new ChartPoint(areaPoint.X, areaPoint.Y));
    this.AreaPoints = chartPointList;
    double end1 = AreaPoints.Max<Point>((Func<Point, double>) (x => x.X));
    double end2 = AreaPoints.Max<Point>((Func<Point, double>) (y => y.Y));
    double start1 = AreaPoints.Min<Point>((Func<Point, double>) (x => x.X));
    double d = AreaPoints.Min<Point>((Func<Point, double>) (item => item.Y));
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<Point> source = AreaPoints.Where<Point>((Func<Point, bool>) (item => !double.IsNaN(item.Y)));
      start2 = !source.Any<Point>() ? 0.0 : source.Min<Point>((Func<Point, double>) (item => item.Y));
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  public override void SetData(List<ChartPoint> AreaPoints)
  {
    this.AreaPoints = AreaPoints;
    double end1 = AreaPoints.Max<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double end2 = AreaPoints.Max<ChartPoint>((Func<ChartPoint, double>) (y => y.Y));
    double start1 = AreaPoints.Min<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double d = AreaPoints.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<ChartPoint> source = AreaPoints.Where<ChartPoint>((Func<ChartPoint, bool>) (item => !double.IsNaN(item.Y)));
      start2 = !source.Any<ChartPoint>() ? 0.0 : source.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
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
    PathFigure pathFigure = new PathFigure();
    if (this.AreaPoints.Count > 1)
    {
      int index1 = this.AreaPoints.Count - 1;
      System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
      pathFigure.StartPoint = transformer.TransformToVisible(this.AreaPoints[0].X, this.AreaPoints[0].Y);
      lineSegment.Point = transformer.TransformToVisible(this.AreaPoints[1].X, this.AreaPoints[1].Y);
      pathFigure.Segments.Add((PathSegment) lineSegment);
      for (int index2 = 2; index2 < this.AreaPoints.Count - 1; index2 += 6)
        pathFigure.Segments.Add((PathSegment) new BezierSegment()
        {
          Point1 = transformer.TransformToVisible(this.AreaPoints[index2].X, this.AreaPoints[index2].Y),
          Point2 = transformer.TransformToVisible(this.AreaPoints[index2 + 1].X, this.AreaPoints[index2 + 1].Y),
          Point3 = transformer.TransformToVisible(this.AreaPoints[index2 + 2].X, this.AreaPoints[index2 + 2].Y)
        });
      pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
      {
        Point = transformer.TransformToVisible(this.AreaPoints[index1].X, this.AreaPoints[index1].Y)
      });
      for (int index3 = index1 - 1; index3 > 1; index3 -= 6)
        pathFigure.Segments.Add((PathSegment) new BezierSegment()
        {
          Point1 = transformer.TransformToVisible(this.AreaPoints[index3].X, this.AreaPoints[index3].Y),
          Point2 = transformer.TransformToVisible(this.AreaPoints[index3 - 1].X, this.AreaPoints[index3 - 1].Y),
          Point3 = transformer.TransformToVisible(this.AreaPoints[index3 - 2].X, this.AreaPoints[index3 - 2].Y)
        });
    }
    this.segPath.Data = (Geometry) new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
  }

  public override void OnSizeChanged(Size size)
  {
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
