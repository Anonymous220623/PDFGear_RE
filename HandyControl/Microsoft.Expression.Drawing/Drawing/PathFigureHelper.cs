// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PathFigureHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class PathFigureHelper
{
  public static IEnumerable<PathSegmentData> AllSegments(this PathFigure figure)
  {
    if (figure != null && figure.Segments.Count > 0)
    {
      Point startPoint = figure.StartPoint;
      PathSegmentCollection.Enumerator enumerator = figure.Segments.GetEnumerator();
      while (enumerator.MoveNext())
      {
        PathSegment current = enumerator.Current;
        Point lastPoint = current.GetLastPoint();
        yield return new PathSegmentData(startPoint, current);
        startPoint = lastPoint;
        lastPoint = new Point();
      }
      enumerator = new PathSegmentCollection.Enumerator();
    }
  }

  internal static void ApplyTransform(this PathFigure figure, GeneralTransform transform)
  {
    figure.StartPoint = transform.Transform(figure.StartPoint);
    for (int index = 0; index < figure.Segments.Count; ++index)
    {
      PathSegment objB = PathSegmentHelper.ApplyTransform(figure.Segments[index], figure.StartPoint, transform);
      if (!object.Equals((object) figure.Segments[index], (object) objB))
        figure.Segments[index] = objB;
    }
  }

  internal static void FlattenFigure(
    PathFigure figure,
    IList<Point> points,
    double tolerance,
    bool removeRepeat)
  {
    if (figure == null)
      throw new ArgumentNullException(nameof (figure));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (tolerance < 0.0)
      throw new ArgumentOutOfRangeException(nameof (tolerance));
    IList<Point> points1 = removeRepeat ? (IList<Point>) new List<Point>() : points;
    points1.Add(figure.StartPoint);
    foreach (PathSegmentData allSegment in figure.AllSegments())
      allSegment.PathSegment.FlattenSegment(points1, allSegment.StartPoint, tolerance);
    if (figure.IsClosed)
      points1.Add(figure.StartPoint);
    if (!removeRepeat || points1.Count <= 0)
      return;
    points.Add(points1[0]);
    for (int index = 1; index < points1.Count; ++index)
    {
      if (!MathHelper.IsVerySmall(GeometryHelper.SquaredDistance(points.Last<Point>(), points1[index])))
        points.Add(points1[index]);
    }
  }

  internal static bool SyncEllipseFigure(
    PathFigure figure,
    Rect bounds,
    SweepDirection sweepDirection,
    bool isFilled = true)
  {
    Point[] pointArray = new Point[2];
    Size size = new Size(bounds.Width / 2.0, bounds.Height / 2.0);
    Point point = bounds.Center();
    if (size.Width > size.Height)
    {
      pointArray[0] = new Point(bounds.Left, point.Y);
      pointArray[1] = new Point(bounds.Right, point.Y);
    }
    else
    {
      pointArray[0] = new Point(point.X, bounds.Top);
      pointArray[1] = new Point(point.X, bounds.Bottom);
    }
    ArcSegment result;
    return (0 | (figure.SetIfDifferent(PathFigure.IsClosedProperty, (object) true) ? 1 : 0) | (figure.SetIfDifferent(PathFigure.IsFilledProperty, (object) isFilled) ? 1 : 0) | (figure.SetIfDifferent(PathFigure.StartPointProperty, (object) pointArray[0]) ? 1 : 0) | (figure.Segments.EnsureListCount<PathSegment>(2, (Func<PathSegment>) (() => (PathSegment) new ArcSegment())) ? 1 : 0) | (GeometryHelper.EnsureSegmentType<ArcSegment>(out result, (IList<PathSegment>) figure.Segments, 0, (Func<ArcSegment>) (() => new ArcSegment())) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.PointProperty, (object) pointArray[1]) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.SizeProperty, (object) size) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) false) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) sweepDirection) ? 1 : 0) | (GeometryHelper.EnsureSegmentType<ArcSegment>(out result, (IList<PathSegment>) figure.Segments, 1, (Func<ArcSegment>) (() => new ArcSegment())) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.PointProperty, (object) pointArray[0]) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.SizeProperty, (object) size) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) false) ? 1 : 0) | (result.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) sweepDirection) ? 1 : 0)) != 0;
  }

  internal static bool SyncPolylineFigure(
    PathFigure figure,
    IList<Point> points,
    bool isClosed,
    bool isFilled = true)
  {
    if (figure == null)
      throw new ArgumentNullException(nameof (figure));
    bool flag = false;
    return (points == null || points.Count == 0 ? flag | figure.ClearIfSet(PathFigure.StartPointProperty) | figure.Segments.EnsureListCount<PathSegment>(0) : flag | figure.SetIfDifferent(PathFigure.StartPointProperty, (object) points[0]) | figure.Segments.EnsureListCount<PathSegment>(1, (Func<PathSegment>) (() => (PathSegment) new PolyLineSegment())) | PathSegmentHelper.SyncPolylineSegment(figure.Segments, 0, points, 1, points.Count - 1)) | figure.SetIfDifferent(PathFigure.IsClosedProperty, (object) isClosed) | figure.SetIfDifferent(PathFigure.IsFilledProperty, (object) isFilled);
  }
}
