// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.SketchGeometryEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Media;

public sealed class SketchGeometryEffect : GeometryEffect
{
  private readonly long _randomSeed = DateTime.Now.Ticks;

  protected override GeometryEffect DeepCopy() => (GeometryEffect) new SketchGeometryEffect();

  private static void DisturbPoints(
    RandomEngine random,
    double scale,
    IList<Point> points,
    IList<Vector> normals)
  {
    int count = points.Count;
    for (int index = 1; index < count; ++index)
    {
      double num1 = random.NextGaussian(0.0, 1.0 * scale);
      double num2 = random.NextUniform(-0.5, 0.5) * scale;
      Point point1 = points[index];
      Vector normal1 = normals[index];
      Vector normal2 = normals[index];
      Point point2 = points[index];
      Vector normal3 = normals[index];
      Vector normal4 = normals[index];
      points[index] = new Point(point1.X + normal1.X * num2 - normal2.Y * num1, point2.Y + normal3.X * num1 + normal4.Y * num2);
    }
  }

  public override bool Equals(GeometryEffect geometryEffect)
  {
    return geometryEffect is SketchGeometryEffect;
  }

  private IEnumerable<SimpleSegment> GetEffectiveSegments(PathFigure pathFigure)
  {
    Point point0 = pathFigure.StartPoint;
    foreach (PathSegmentData allSegment in pathFigure.AllSegments())
    {
      foreach (SimpleSegment iteratorVariable2 in allSegment.PathSegment.GetSimpleSegments(allSegment.StartPoint))
      {
        yield return iteratorVariable2;
        point0 = ((IEnumerable<Point>) iteratorVariable2.Points).Last<Point>();
      }
    }
    if (pathFigure.IsClosed)
      yield return SimpleSegment.Create(point0, pathFigure.StartPoint);
  }

  protected override bool UpdateCachedGeometry(Geometry input)
  {
    bool flag = false;
    PathGeometry inputPath = input.AsPathGeometry();
    if (inputPath != null)
      return flag | this.UpdateSketchGeometry(inputPath);
    this.CachedGeometry = input;
    return flag;
  }

  private bool UpdateSketchGeometry(PathGeometry inputPath)
  {
    PathGeometry result;
    bool flag = false | GeometryHelper.EnsureGeometryType<PathGeometry>(out result, ref this.CachedGeometry, (Func<PathGeometry>) (() => new PathGeometry())) | result.Figures.EnsureListCount<PathFigure>(inputPath.Figures.Count, (Func<PathFigure>) (() => new PathFigure()));
    RandomEngine random = new RandomEngine(this._randomSeed);
    for (int index = 0; index < inputPath.Figures.Count; ++index)
    {
      PathFigure figure = inputPath.Figures[index];
      bool isClosed = figure.IsClosed;
      bool isFilled = figure.IsFilled;
      if (figure.Segments.Count == 0)
      {
        flag = flag | result.Figures[index].SetIfDifferent(PathFigure.StartPointProperty, (object) figure.StartPoint) | result.Figures[index].Segments.EnsureListCount<PathSegment>(0);
      }
      else
      {
        List<Point> pointList1 = new List<Point>(figure.Segments.Count * 3);
        foreach (SimpleSegment effectiveSegment in this.GetEffectiveSegments(figure))
        {
          List<Point> pointList2 = new List<Point>()
          {
            effectiveSegment.Points[0]
          };
          effectiveSegment.Flatten((IList<Point>) pointList2, 0.0, (IList<double>) null);
          PolylineData polyline = new PolylineData((IList<Point>) pointList2);
          if (pointList2.Count > 1 && polyline.TotalLength > 4.0)
          {
            int sampleCount = (int) Math.Max(2.0, Math.Ceiling(polyline.TotalLength / 8.0));
            double interval = polyline.TotalLength / (double) sampleCount;
            double scale = interval / 8.0;
            List<Point> samplePoints = new List<Point>(sampleCount);
            List<Vector> sampleNormals = new List<Vector>(sampleCount);
            int sampleIndex = 0;
            PolylineHelper.PathMarch(polyline, 0.0, 0.0, (Func<MarchLocation, double>) (location =>
            {
              if (location.Reason == MarchStopReason.CompletePolyline)
                return double.NaN;
              if (location.Reason != MarchStopReason.CompleteStep)
                return location.Remain;
              if (sampleIndex++ == sampleCount)
                return double.NaN;
              samplePoints.Add(location.GetPoint(polyline.Points));
              sampleNormals.Add(location.GetNormal(polyline));
              return interval;
            }));
            SketchGeometryEffect.DisturbPoints(random, scale, (IList<Point>) samplePoints, (IList<Vector>) sampleNormals);
            pointList1.AddRange((IEnumerable<Point>) samplePoints);
          }
          else
          {
            pointList1.AddRange((IEnumerable<Point>) pointList2);
            pointList1.RemoveLast<Point>();
          }
        }
        if (!isClosed)
          pointList1.Add(figure.Segments.Last<PathSegment>().GetLastPoint());
        flag |= PathFigureHelper.SyncPolylineFigure(result.Figures[index], (IList<Point>) pointList1, isClosed, isFilled);
      }
    }
    if (flag)
      this.CachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(this.CachedGeometry);
    return flag;
  }
}
