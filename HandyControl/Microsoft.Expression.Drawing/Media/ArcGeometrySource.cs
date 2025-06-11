// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.ArcGeometrySource
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Media;

internal class ArcGeometrySource : GeometrySource<IArcGeometrySourceParameters>
{
  private double _absoluteThickness;
  private double _relativeThickness;

  private static bool AreCloseEnough(double angleA, double angleB)
  {
    return Math.Abs(angleA - angleB) < 0.001;
  }

  internal static double[] ComputeAngleRanges(
    double radiusX,
    double radiusY,
    double intersect,
    double start,
    double end)
  {
    List<double> values = new List<double>()
    {
      start,
      end,
      intersect,
      180.0 - intersect,
      180.0 + intersect,
      360.0 - intersect,
      360.0 + intersect,
      540.0 - intersect,
      540.0 + intersect,
      720.0 - intersect
    };
    values.Sort();
    int index1 = values.IndexOf(start);
    int index2 = values.IndexOf(end);
    if (index2 == index1)
      ++index2;
    else if (start < end)
    {
      ArcGeometrySource.IncreaseDuplicatedIndex((IList<double>) values, ref index1);
      ArcGeometrySource.DecreaseDuplicatedIndex((IList<double>) values, ref index2);
    }
    else if (start > end)
    {
      ArcGeometrySource.DecreaseDuplicatedIndex((IList<double>) values, ref index1);
      ArcGeometrySource.IncreaseDuplicatedIndex((IList<double>) values, ref index2);
    }
    List<double> list = new List<double>();
    if (index1 < index2)
    {
      for (int index3 = index1; index3 <= index2; ++index3)
        list.Add(values[index3]);
    }
    else
    {
      for (int index4 = index1; index4 >= index2; --index4)
        list.Add(values[index4]);
    }
    double num = ArcGeometrySource.EnsureFirstQuadrant((list[0] + list[1]) / 2.0);
    if (radiusX < radiusY && num < intersect || radiusX > radiusY && num > intersect)
      list.RemoveAt(0);
    if (list.Count % 2 == 1)
      list.RemoveLast<double>();
    if (list.Count == 0)
    {
      int index5 = Math.Min(index1, index2) - 1;
      if (index5 < 0)
        index5 = Math.Max(index1, index2) + 1;
      list.Add(values[index5]);
      list.Add(values[index5]);
    }
    return list.ToArray();
  }

  protected override Rect ComputeLogicalBounds(
    Rect layoutBounds,
    IGeometrySourceParameters parameters)
  {
    return GeometryHelper.GetStretchBound(base.ComputeLogicalBounds(layoutBounds, parameters), parameters.Stretch, new Size(1.0, 1.0));
  }

  private static IList<Point> ComputeOneInnerCurve(
    double start,
    double end,
    Rect bounds,
    double offset)
  {
    double num1 = bounds.Width / 2.0;
    double num2 = bounds.Height / 2.0;
    Point point1 = bounds.Center();
    start = start * Math.PI / 180.0;
    end = end * Math.PI / 180.0;
    double num3 = Math.PI / 18.0;
    int capacity = Math.Max(2, (int) Math.Ceiling(Math.Abs(end - start) / num3));
    List<Point> pointList = new List<Point>(capacity);
    List<Vector> vectorList = new List<Vector>(capacity);
    Point point2 = new Point();
    Point point3 = new Point();
    Vector vector1 = new Vector();
    Vector vector2 = new Vector();
    Vector vector3 = new Vector();
    Vector vector4 = new Vector();
    for (int index = 0; index < capacity; ++index)
    {
      double num4 = MathHelper.Lerp(start, end, (double) index / (double) (capacity - 1));
      double num5 = Math.Sin(num4);
      double num6 = Math.Cos(num4);
      point2.X = point1.X + num1 * num5;
      point2.Y = point1.Y - num2 * num6;
      vector1.X = num1 * num6;
      vector1.Y = num2 * num5;
      vector2.X = -num2 * num5;
      vector2.Y = num1 * num6;
      double d = num2 * num2 * num5 * num5 + num1 * num1 * num6 * num6;
      double num7 = Math.Sqrt(d);
      double num8 = 2.0 * num5 * num6 * (num2 * num2 - num1 * num1);
      vector3.X = -num2 * num6;
      vector3.Y = -num1 * num5;
      point3.X = point2.X + offset * vector2.X / num7;
      point3.Y = point2.Y + offset * vector2.Y / num7;
      vector4.X = vector1.X + offset / num7 * (vector3.X - 0.5 * vector2.X / d * num8);
      vector4.Y = vector1.Y + offset / num7 * (vector3.Y - 0.5 * vector2.Y / d * num8);
      pointList.Add(point3);
      vectorList.Add(-vector4.Normalized());
    }
    List<Point> oneInnerCurve = new List<Point>(capacity * 3 + 1)
    {
      pointList[0]
    };
    for (int index = 1; index < capacity; ++index)
    {
      Point lhs = pointList[index - 1];
      Point rhs = pointList[index];
      double num9 = GeometryHelper.Distance(lhs, rhs) / 3.0;
      oneInnerCurve.Add(lhs + vectorList[index - 1] * num9);
      oneInnerCurve.Add(rhs - vectorList[index] * num9);
      oneInnerCurve.Add(rhs);
    }
    return (IList<Point>) oneInnerCurve;
  }

  private static void DecreaseDuplicatedIndex(IList<double> values, ref int index)
  {
    while (index > 0 && values[index] == values[index - 1])
      --index;
  }

  internal static double EnsureFirstQuadrant(double angle)
  {
    angle = Math.Abs(angle % 180.0);
    return angle <= 90.0 ? angle : 180.0 - angle;
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  private static Size GetArcSize(Rect bound) => new Size(bound.Width / 2.0, bound.Height / 2.0);

  private static void IncreaseDuplicatedIndex(IList<double> values, ref int index)
  {
    while (index < values.Count - 1 && values[index] == values[index + 1])
      ++index;
  }

  internal static double InnerCurveSelfIntersect(double radiusX, double radiusY, double thickness)
  {
    double angleA1 = 0.0;
    double angleB = Math.PI / 2.0;
    bool flag = radiusX <= radiusY;
    Vector vector = new Vector();
    while (!ArcGeometrySource.AreCloseEnough(angleA1, angleB))
    {
      double num1 = (angleA1 + angleB) / 2.0;
      double num2 = Math.Cos(num1);
      double num3 = Math.Sin(num1);
      vector.X = radiusY * num3;
      vector.Y = radiusX * num2;
      vector.Normalize();
      if (flag)
      {
        double num4 = radiusX * num3 - vector.X * thickness;
        if (num4 > 0.0)
          angleB = num1;
        else if (num4 < 0.0)
          angleA1 = num1;
      }
      else
      {
        double num5 = radiusY * num2 - vector.Y * thickness;
        if (num5 < 0.0)
          angleB = num1;
        else if (num5 > 0.0)
          angleA1 = num1;
      }
    }
    double angleA2 = (angleA1 + angleB) / 2.0;
    if (ArcGeometrySource.AreCloseEnough(angleA2, 0.0))
      return 0.0;
    return !ArcGeometrySource.AreCloseEnough(angleA2, Math.PI / 2.0) ? angleA2 * 180.0 / Math.PI : 90.0;
  }

  private static double NormalizeAngle(double degree)
  {
    if (degree < 0.0 || degree > 360.0)
    {
      degree %= 360.0;
      if (degree < 0.0)
        degree += 360.0;
    }
    return degree;
  }

  private void NormalizeThickness(IArcGeometrySourceParameters parameters)
  {
    Rect logicalBounds = this.LogicalBounds;
    double val1 = logicalBounds.Width / 2.0;
    logicalBounds = this.LogicalBounds;
    double val2 = logicalBounds.Height / 2.0;
    double rhs = Math.Min(val1, val2);
    double lhs = parameters.ArcThickness;
    if (parameters.ArcThicknessUnit == UnitType.Pixel)
      lhs = MathHelper.SafeDivide(lhs, rhs, 0.0);
    this._relativeThickness = MathHelper.EnsureRange(lhs, new double?(0.0), new double?(1.0));
    this._absoluteThickness = rhs * this._relativeThickness;
  }

  private bool SyncPieceWiseInnerCurves(
    PathFigure figure,
    int index,
    ref Point firstPoint,
    params double[] angles)
  {
    bool flag1 = false;
    int length = angles.Length;
    Rect logicalBounds = this.LogicalBounds;
    double absoluteThickness = this._absoluteThickness;
    bool flag2 = flag1 | figure.Segments.EnsureListCount<PathSegment>(index + length / 2, (Func<PathSegment>) (() => (PathSegment) new PolyBezierSegment()));
    for (int index1 = 0; index1 < length / 2; ++index1)
    {
      IList<Point> oneInnerCurve = ArcGeometrySource.ComputeOneInnerCurve(angles[index1 * 2], angles[index1 * 2 + 1], logicalBounds, absoluteThickness);
      if (index1 == 0)
        firstPoint = oneInnerCurve[0];
      flag2 |= PathSegmentHelper.SyncPolyBezierSegment(figure.Segments, index + index1, oneInnerCurve, 1, oneInnerCurve.Count - 1);
    }
    return flag2;
  }

  protected override bool UpdateCachedGeometry(IArcGeometrySourceParameters parameters)
  {
    bool flag1 = false;
    this.NormalizeThickness(parameters);
    bool relativeMode = parameters.ArcThicknessUnit == UnitType.Percent;
    int num1 = MathHelper.AreClose(parameters.StartAngle, parameters.EndAngle) ? 1 : 0;
    double num2 = ArcGeometrySource.NormalizeAngle(parameters.StartAngle);
    double end = ArcGeometrySource.NormalizeAngle(parameters.EndAngle);
    if (end < num2)
      end += 360.0;
    bool isFilled = this._relativeThickness == 1.0;
    bool flag2 = this._relativeThickness == 0.0;
    if (num1 != 0)
      return flag1 | this.UpdateZeroAngleGeometry(relativeMode, num2);
    if (MathHelper.IsVerySmall((end - num2) % 360.0))
      return flag2 | isFilled ? flag1 | this.UpdateEllipseGeometry(isFilled) : flag1 | this.UpdateFullRingGeometry(relativeMode);
    if (isFilled)
      return flag1 | this.UpdatePieGeometry(num2, end);
    return flag2 ? flag1 | this.UpdateOpenArcGeometry(num2, end) : flag1 | this.UpdateRingArcGeometry(relativeMode, num2, end);
  }

  private bool UpdateEllipseGeometry(bool isFilled)
  {
    Rect logicalBounds = this.LogicalBounds;
    double top = logicalBounds.Top;
    logicalBounds = this.LogicalBounds;
    double bottom = logicalBounds.Bottom;
    double y = MathHelper.Lerp(top, bottom, 0.5);
    Point point1 = new Point(this.LogicalBounds.Left, y);
    Point point2 = new Point(this.LogicalBounds.Right, y);
    PathGeometry result1;
    int num1 = 0 | (GeometryHelper.EnsureGeometryType<PathGeometry>(out result1, ref this.CachedGeometry, (Func<PathGeometry>) (() => new PathGeometry())) ? 1 : 0) | (result1.Figures.EnsureListCount<PathFigure>(1, (Func<PathFigure>) (() => new PathFigure())) ? 1 : 0);
    PathFigure figure = result1.Figures[0];
    int num2 = figure.SetIfDifferent(PathFigure.IsClosedProperty, (object) true) ? 1 : 0;
    ArcSegment result2;
    ArcSegment result3;
    int num3 = num1 | num2 | (figure.SetIfDifferent(PathFigure.IsFilledProperty, (object) isFilled) ? 1 : 0) | (figure.Segments.EnsureListCount<PathSegment>(2, (Func<PathSegment>) (() => (PathSegment) new ArcSegment())) ? 1 : 0) | (figure.SetIfDifferent(PathFigure.StartPointProperty, (object) point1) ? 1 : 0) | (GeometryHelper.EnsureSegmentType<ArcSegment>(out result2, (IList<PathSegment>) figure.Segments, 0, (Func<ArcSegment>) (() => new ArcSegment())) ? 1 : 0) | (GeometryHelper.EnsureSegmentType<ArcSegment>(out result3, (IList<PathSegment>) figure.Segments, 1, (Func<ArcSegment>) (() => new ArcSegment())) ? 1 : 0);
    Size size = new Size(this.LogicalBounds.Width / 2.0, this.LogicalBounds.Height / 2.0);
    int num4 = result2.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) false) ? 1 : 0;
    return (num3 | num4 | (result2.SetIfDifferent(ArcSegment.SizeProperty, (object) size) ? 1 : 0) | (result2.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) SweepDirection.Clockwise) ? 1 : 0) | (result2.SetIfDifferent(ArcSegment.PointProperty, (object) point2) ? 1 : 0) | (result3.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) false) ? 1 : 0) | (result3.SetIfDifferent(ArcSegment.SizeProperty, (object) size) ? 1 : 0) | (result3.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) SweepDirection.Clockwise) ? 1 : 0) | (result3.SetIfDifferent(ArcSegment.PointProperty, (object) point1) ? 1 : 0)) != 0;
  }

  private bool UpdateFullRingGeometry(bool relativeMode)
  {
    PathGeometry result;
    bool flag1 = false | GeometryHelper.EnsureGeometryType<PathGeometry>(out result, ref this.CachedGeometry, (Func<PathGeometry>) (() => new PathGeometry())) | result.SetIfDifferent(PathGeometry.FillRuleProperty, (object) FillRule.EvenOdd) | result.Figures.EnsureListCount<PathFigure>(2, (Func<PathFigure>) (() => new PathFigure())) | PathFigureHelper.SyncEllipseFigure(result.Figures[0], this.LogicalBounds, SweepDirection.Clockwise);
    Rect logicalBounds = this.LogicalBounds;
    double radiusX = logicalBounds.Width / 2.0;
    double radiusY = logicalBounds.Height / 2.0;
    if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
    {
      Rect bounds = this.LogicalBounds.Resize(1.0 - this._relativeThickness);
      return flag1 | PathFigureHelper.SyncEllipseFigure(result.Figures[1], bounds, SweepDirection.Counterclockwise);
    }
    bool flag2 = flag1 | result.Figures[1].SetIfDifferent(PathFigure.IsClosedProperty, (object) true) | result.Figures[1].SetIfDifferent(PathFigure.IsFilledProperty, (object) true);
    Point firstPoint = new Point();
    double intersect = ArcGeometrySource.InnerCurveSelfIntersect(radiusX, radiusY, this._absoluteThickness);
    double[] angleRanges = ArcGeometrySource.ComputeAngleRanges(radiusX, radiusY, intersect, 360.0, 0.0);
    return flag2 | this.SyncPieceWiseInnerCurves(result.Figures[1], 0, ref firstPoint, angleRanges) | result.Figures[1].SetIfDifferent(PathFigure.StartPointProperty, (object) firstPoint);
  }

  private bool UpdateOpenArcGeometry(double start, double end)
  {
    bool flag = false;
    PathFigure dependencyObject1;
    if (!(this.CachedGeometry is PathGeometry cachedGeometry) || cachedGeometry.Figures.Count != 1 || (dependencyObject1 = cachedGeometry.Figures[0]).Segments.Count != 1 || !(dependencyObject1.Segments[0] is ArcSegment dependencyObject2))
    {
      dependencyObject1 = new PathFigure();
      this.CachedGeometry = (Geometry) new PathGeometry()
      {
        Figures = {
          dependencyObject1
        }
      };
      dependencyObject1.Segments.Add((PathSegment) (dependencyObject2 = new ArcSegment()));
      dependencyObject1.IsClosed = false;
      dependencyObject2.SweepDirection = SweepDirection.Clockwise;
      flag = true;
    }
    return flag | dependencyObject1.SetIfDifferent(PathFigure.StartPointProperty, (object) GeometryHelper.GetArcPoint(start, this.LogicalBounds)) | dependencyObject1.SetIfDifferent(PathFigure.IsFilledProperty, (object) false) | dependencyObject2.SetIfDifferent(ArcSegment.PointProperty, (object) GeometryHelper.GetArcPoint(end, this.LogicalBounds)) | dependencyObject2.SetIfDifferent(ArcSegment.SizeProperty, (object) ArcGeometrySource.GetArcSize(this.LogicalBounds)) | dependencyObject2.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) (end - start > 180.0));
  }

  private bool UpdatePieGeometry(double start, double end)
  {
    bool flag = false;
    PathFigure dependencyObject1;
    if (!(this.CachedGeometry is PathGeometry cachedGeometry) || cachedGeometry.Figures.Count != 1 || (dependencyObject1 = cachedGeometry.Figures[0]).Segments.Count != 2 || !(dependencyObject1.Segments[0] is ArcSegment dependencyObject2) || !(dependencyObject1.Segments[1] is LineSegment dependencyObject3))
    {
      dependencyObject1 = new PathFigure();
      this.CachedGeometry = (Geometry) new PathGeometry()
      {
        Figures = {
          dependencyObject1
        }
      };
      dependencyObject1.Segments.Add((PathSegment) (dependencyObject2 = new ArcSegment()));
      dependencyObject1.Segments.Add((PathSegment) (dependencyObject3 = new LineSegment()));
      dependencyObject1.IsClosed = true;
      dependencyObject2.SweepDirection = SweepDirection.Clockwise;
      flag = true;
    }
    return flag | dependencyObject1.SetIfDifferent(PathFigure.StartPointProperty, (object) GeometryHelper.GetArcPoint(start, this.LogicalBounds)) | dependencyObject2.SetIfDifferent(ArcSegment.PointProperty, (object) GeometryHelper.GetArcPoint(end, this.LogicalBounds)) | dependencyObject2.SetIfDifferent(ArcSegment.SizeProperty, (object) ArcGeometrySource.GetArcSize(this.LogicalBounds)) | dependencyObject2.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) (end - start > 180.0)) | dependencyObject3.SetIfDifferent(LineSegment.PointProperty, (object) this.LogicalBounds.Center());
  }

  private bool UpdateRingArcGeometry(bool relativeMode, double start, double end)
  {
    PathGeometry result1;
    bool flag1 = false | GeometryHelper.EnsureGeometryType<PathGeometry>(out result1, ref this.CachedGeometry, (Func<PathGeometry>) (() => new PathGeometry())) | result1.SetIfDifferent(PathGeometry.FillRuleProperty, (object) FillRule.Nonzero) | result1.Figures.EnsureListCount<PathFigure>(1, (Func<PathFigure>) (() => new PathFigure()));
    PathFigure figure = result1.Figures[0];
    ArcSegment result2;
    LineSegment result3;
    bool flag2 = flag1 | figure.SetIfDifferent(PathFigure.IsClosedProperty, (object) true) | figure.SetIfDifferent(PathFigure.IsFilledProperty, (object) true) | figure.SetIfDifferent(PathFigure.StartPointProperty, (object) GeometryHelper.GetArcPoint(start, this.LogicalBounds)) | figure.Segments.EnsureListCountAtLeast<PathSegment>(3, (Func<PathSegment>) (() => (PathSegment) new ArcSegment())) | GeometryHelper.EnsureSegmentType<ArcSegment>(out result2, (IList<PathSegment>) figure.Segments, 0, (Func<ArcSegment>) (() => new ArcSegment())) | result2.SetIfDifferent(ArcSegment.PointProperty, (object) GeometryHelper.GetArcPoint(end, this.LogicalBounds)) | result2.SetIfDifferent(ArcSegment.SizeProperty, (object) new Size(this.LogicalBounds.Width / 2.0, this.LogicalBounds.Height / 2.0)) | result2.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) (end - start > 180.0)) | result2.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) SweepDirection.Clockwise) | GeometryHelper.EnsureSegmentType<LineSegment>(out result3, (IList<PathSegment>) figure.Segments, 1, (Func<LineSegment>) (() => new LineSegment()));
    Rect logicalBounds = this.LogicalBounds;
    double radiusX = logicalBounds.Width / 2.0;
    double radiusY = logicalBounds.Height / 2.0;
    if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
    {
      Rect bound = this.LogicalBounds.Resize(1.0 - this._relativeThickness);
      ArcSegment result4;
      return flag2 | result3.SetIfDifferent(LineSegment.PointProperty, (object) GeometryHelper.GetArcPoint(end, bound)) | figure.Segments.EnsureListCount<PathSegment>(3, (Func<PathSegment>) (() => (PathSegment) new ArcSegment())) | GeometryHelper.EnsureSegmentType<ArcSegment>(out result4, (IList<PathSegment>) figure.Segments, 2, (Func<ArcSegment>) (() => new ArcSegment())) | result4.SetIfDifferent(ArcSegment.PointProperty, (object) GeometryHelper.GetArcPoint(start, bound)) | result4.SetIfDifferent(ArcSegment.SizeProperty, (object) ArcGeometrySource.GetArcSize(bound)) | result4.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) (end - start > 180.0)) | result4.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) SweepDirection.Counterclockwise);
    }
    Point firstPoint = new Point();
    double intersect = ArcGeometrySource.InnerCurveSelfIntersect(radiusX, radiusY, this._absoluteThickness);
    double[] angleRanges = ArcGeometrySource.ComputeAngleRanges(radiusX, radiusY, intersect, end, start);
    return flag2 | this.SyncPieceWiseInnerCurves(figure, 2, ref firstPoint, angleRanges) | result3.SetIfDifferent(LineSegment.PointProperty, (object) firstPoint);
  }

  private bool UpdateZeroAngleGeometry(bool relativeMode, double angle)
  {
    Point arcPoint = GeometryHelper.GetArcPoint(angle, this.LogicalBounds);
    Rect logicalBounds = this.LogicalBounds;
    double radiusX = logicalBounds.Width / 2.0;
    double radiusY = logicalBounds.Height / 2.0;
    Point point;
    if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
    {
      Rect bound = this.LogicalBounds.Resize(1.0 - this._relativeThickness);
      point = GeometryHelper.GetArcPoint(angle, bound);
    }
    else
    {
      double intersect = ArcGeometrySource.InnerCurveSelfIntersect(radiusX, radiusY, this._absoluteThickness);
      double[] angleRanges = ArcGeometrySource.ComputeAngleRanges(radiusX, radiusY, intersect, angle, angle);
      double num = angleRanges[0] * Math.PI / 180.0;
      Vector vector = new Vector(radiusY * Math.Sin(num), -radiusX * Math.Cos(num));
      point = GeometryHelper.GetArcPoint(angleRanges[0], this.LogicalBounds) - vector.Normalized() * this._absoluteThickness;
    }
    LineGeometry result;
    return (0 | (GeometryHelper.EnsureGeometryType<LineGeometry>(out result, ref this.CachedGeometry, (Func<LineGeometry>) (() => new LineGeometry())) ? 1 : 0) | (result.SetIfDifferent(LineGeometry.StartPointProperty, (object) arcPoint) ? 1 : 0) | (result.SetIfDifferent(LineGeometry.EndPointProperty, (object) point) ? 1 : 0)) != 0;
  }
}
