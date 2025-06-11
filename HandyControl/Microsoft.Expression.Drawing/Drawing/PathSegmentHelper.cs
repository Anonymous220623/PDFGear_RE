// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PathSegmentHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class PathSegmentHelper
{
  public static PathSegment ApplyTransform(
    PathSegment segment,
    Point start,
    GeneralTransform transform)
  {
    return PathSegmentHelper.PathSegmentImplementation.Create(segment, start).ApplyTransform(transform);
  }

  public static PathSegment ArcToBezierSegments(ArcSegment arcSegment, Point startPoint)
  {
    bool isStroked = arcSegment.IsStroked();
    double x1 = startPoint.X;
    double y1 = startPoint.Y;
    Size size = arcSegment.Size;
    double width = size.Width;
    size = arcSegment.Size;
    double height = size.Height;
    double rotationAngle = arcSegment.RotationAngle;
    int num1 = arcSegment.IsLargeArc ? 1 : 0;
    int num2 = arcSegment.SweepDirection == SweepDirection.Clockwise ? 1 : 0;
    Point point = arcSegment.Point;
    double x2 = point.X;
    point = arcSegment.Point;
    double y2 = point.Y;
    Point[] points;
    ref Point[] local1 = ref points;
    int num3;
    ref int local2 = ref num3;
    PathSegmentHelper.ArcToBezierHelper.ArcToBezier(x1, y1, width, height, rotationAngle, num1 != 0, num2 != 0, x2, y2, out local1, out local2);
    PathSegment bezierSegments;
    switch (num3)
    {
      case -1:
        bezierSegments = (PathSegment) null;
        break;
      case 0:
        bezierSegments = (PathSegment) PathSegmentHelper.CreateLineSegment(arcSegment.Point, isStroked);
        break;
      case 1:
        bezierSegments = (PathSegment) PathSegmentHelper.CreateBezierSegment(points[0], points[1], points[2], isStroked);
        break;
      default:
        bezierSegments = (PathSegment) PathSegmentHelper.CreatePolyBezierSegment((IList<Point>) points, 0, num3 * 3, isStroked);
        break;
    }
    return bezierSegments;
  }

  public static ArcSegment CreateArcSegment(
    Point point,
    Size size,
    bool isLargeArc,
    bool clockwise,
    double rotationAngle = 0.0,
    bool isStroked = true)
  {
    ArcSegment arcSegment = new ArcSegment();
    arcSegment.SetIfDifferent(ArcSegment.PointProperty, (object) point);
    arcSegment.SetIfDifferent(ArcSegment.SizeProperty, (object) size);
    arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, (object) isLargeArc);
    arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, (object) (SweepDirection) (clockwise ? 1 : 0));
    arcSegment.SetIfDifferent(ArcSegment.RotationAngleProperty, (object) rotationAngle);
    arcSegment.SetIsStroked(isStroked);
    return arcSegment;
  }

  public static BezierSegment CreateBezierSegment(
    Point point1,
    Point point2,
    Point point3,
    bool isStroked = true)
  {
    BezierSegment segment = new BezierSegment();
    segment.Point1 = point1;
    segment.Point2 = point2;
    segment.Point3 = point3;
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static LineSegment CreateLineSegment(Point point, bool isStroked = true)
  {
    LineSegment segment = new LineSegment();
    segment.Point = point;
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static PolyBezierSegment CreatePolyBezierSegment(
    IList<Point> points,
    int start,
    int count,
    bool isStroked = true)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    count = count / 3 * 3;
    if (count < 0 || points.Count < start + count)
      throw new ArgumentOutOfRangeException(nameof (count));
    PolyBezierSegment segment = new PolyBezierSegment()
    {
      Points = new PointCollection()
    };
    for (int index = 0; index < count; ++index)
      segment.Points.Add(points[start + index]);
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static PolyLineSegment CreatePolylineSegment(
    IList<Point> points,
    int start,
    int count,
    bool isStroked = true)
  {
    if (count < 0 || points.Count < start + count)
      throw new ArgumentOutOfRangeException(nameof (count));
    PolyLineSegment segment = new PolyLineSegment()
    {
      Points = new PointCollection()
    };
    for (int index = 0; index < count; ++index)
      segment.Points.Add(points[start + index]);
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static PolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(
    IList<Point> points,
    int start,
    int count,
    bool isStroked = true)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    count = count / 2 * 2;
    if (count < 0 || points.Count < start + count)
      throw new ArgumentOutOfRangeException(nameof (count));
    PolyQuadraticBezierSegment segment = new PolyQuadraticBezierSegment()
    {
      Points = new PointCollection()
    };
    for (int index = 0; index < count; ++index)
      segment.Points.Add(points[start + index]);
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static QuadraticBezierSegment CreateQuadraticBezierSegment(
    Point point1,
    Point point2,
    bool isStroked = true)
  {
    QuadraticBezierSegment segment = new QuadraticBezierSegment();
    segment.Point1 = point1;
    segment.Point2 = point2;
    segment.SetIsStroked(isStroked);
    return segment;
  }

  public static void FlattenSegment(
    this PathSegment segment,
    IList<Point> points,
    Point start,
    double tolerance)
  {
    PathSegmentHelper.PathSegmentImplementation.Create(segment, start).Flatten(points, tolerance);
  }

  public static Point GetLastPoint(this PathSegment segment) => segment.GetPoint(-1);

  public static Point GetPoint(this PathSegment segment, int index)
  {
    return PathSegmentHelper.PathSegmentImplementation.Create(segment).GetPoint(index);
  }

  public static int GetPointCount(this PathSegment segment)
  {
    switch (segment)
    {
      case ArcSegment _:
        return 1;
      case LineSegment _:
        return 1;
      case QuadraticBezierSegment _:
        return 2;
      case BezierSegment _:
        return 3;
      case PolyLineSegment polyLineSegment:
        return polyLineSegment.Points.Count;
      case PolyQuadraticBezierSegment quadraticBezierSegment:
        return quadraticBezierSegment.Points.Count / 2 * 2;
      case PolyBezierSegment polyBezierSegment:
        return polyBezierSegment.Points.Count / 3 * 3;
      default:
        return 0;
    }
  }

  public static IEnumerable<SimpleSegment> GetSimpleSegments(this PathSegment segment, Point start)
  {
    return PathSegmentHelper.PathSegmentImplementation.Create(segment, start).GetSimpleSegments();
  }

  public static bool IsEmpty(this PathSegment segment) => segment.GetPointCount() == 0;

  private static void SetIsStroked(this PathSegment segment, bool isStroked)
  {
    if (segment.IsStroked == isStroked)
      return;
    segment.IsStroked = isStroked;
  }

  public static bool SyncPolyBezierSegment(
    PathSegmentCollection collection,
    int index,
    IList<Point> points,
    int start,
    int count)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    if (index < 0 || index >= collection.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (start < 0)
      throw new ArgumentOutOfRangeException(nameof (start));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (points.Count < start + count)
      throw new ArgumentOutOfRangeException(nameof (count));
    bool flag = false;
    count = count / 3 * 3;
    if (!(collection[index] is PolyBezierSegment polyBezierSegment))
    {
      collection[index] = (PathSegment) (polyBezierSegment = new PolyBezierSegment());
      flag = true;
    }
    polyBezierSegment.Points.EnsureListCount<Point>(count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      if (polyBezierSegment.Points[index1] != points[index1 + start])
      {
        polyBezierSegment.Points[index1] = points[index1 + start];
        flag = true;
      }
    }
    return flag;
  }

  public static bool SyncPolylineSegment(
    PathSegmentCollection collection,
    int index,
    IList<Point> points,
    int start,
    int count)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    if (index < 0 || index >= collection.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (start < 0)
      throw new ArgumentOutOfRangeException(nameof (start));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (points.Count < start + count)
      throw new ArgumentOutOfRangeException(nameof (count));
    bool flag1 = false;
    if (!(collection[index] is PolyLineSegment polyLineSegment))
    {
      collection[index] = (PathSegment) (polyLineSegment = new PolyLineSegment());
      flag1 = true;
    }
    bool flag2 = flag1 | polyLineSegment.Points.EnsureListCount<Point>(count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      if (polyLineSegment.Points[index1] != points[index1 + start])
      {
        polyLineSegment.Points[index1] = points[index1 + start];
        flag2 = true;
      }
    }
    return flag2;
  }

  private class ArcSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private ArcSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this._segment, this.Start);
      if (bezierSegments != null)
        return PathSegmentHelper.ApplyTransform(bezierSegments, this.Start, transform);
      this._segment.Point = transform.Transform(this._segment.Point);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(ArcSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.ArcSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this._segment, this.Start);
      if (bezierSegments == null)
        return;
      bezierSegments.FlattenSegment(points, this.Start, tolerance);
    }

    public override Point GetPoint(int index)
    {
      if (index < -1 || index > 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this._segment.Point;
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this._segment, this.Start);
      return bezierSegments != null ? bezierSegments.GetSimpleSegments(this.Start) : Enumerable.Empty<SimpleSegment>();
    }
  }

  private static class ArcToBezierHelper
  {
    private static bool AcceptRadius(double rHalfChord2, double rFuzz2, ref double rRadius)
    {
      int num = rRadius * rRadius > rHalfChord2 * rFuzz2 ? 1 : 0;
      if (num == 0)
        return num != 0;
      if (rRadius >= 0.0)
        return num != 0;
      rRadius = -rRadius;
      return num != 0;
    }

    public static void ArcToBezier(
      double xStart,
      double yStart,
      double xRadius,
      double yRadius,
      double rRotation,
      bool fLargeArc,
      bool fSweepUp,
      double xEnd,
      double yEnd,
      out Point[] pPt,
      out int cPieces)
    {
      double num1 = 1E-06;
      pPt = new Point[12];
      double rFuzz2 = num1 * num1;
      bool flag = false;
      cPieces = -1;
      double num2 = 0.5 * (xEnd - xStart);
      double num3 = 0.5 * (yEnd - yStart);
      double rHalfChord2 = num2 * num2 + num3 * num3;
      if (rHalfChord2 < rFuzz2)
        return;
      if (!PathSegmentHelper.ArcToBezierHelper.AcceptRadius(rHalfChord2, rFuzz2, ref xRadius) || !PathSegmentHelper.ArcToBezierHelper.AcceptRadius(rHalfChord2, rFuzz2, ref yRadius))
      {
        cPieces = 0;
      }
      else
      {
        double num4;
        double num5;
        if (Math.Abs(rRotation) < num1)
        {
          num4 = 1.0;
          num5 = 0.0;
        }
        else
        {
          rRotation = -rRotation * Math.PI / 180.0;
          num4 = Math.Cos(rRotation);
          num5 = Math.Sin(rRotation);
          double num6 = num2 * num4 - num3 * num5;
          num3 = num2 * num5 + num3 * num4;
          num2 = num6;
        }
        double num7 = num2 / xRadius;
        double num8 = num3 / yRadius;
        double d = num7 * num7 + num8 * num8;
        double num9;
        double num10;
        if (d > 1.0)
        {
          double num11 = Math.Sqrt(d);
          xRadius *= num11;
          yRadius *= num11;
          num10 = num9 = 0.0;
          flag = true;
          num7 /= num11;
          num8 /= num11;
        }
        else
        {
          double num12 = Math.Sqrt((1.0 - d) / d);
          if (fLargeArc != fSweepUp)
          {
            num10 = -num12 * num8;
            num9 = num12 * num7;
          }
          else
          {
            num10 = num12 * num8;
            num9 = -num12 * num7;
          }
        }
        Point point1 = new Point(-num7 - num10, -num8 - num9);
        Point point2 = new Point(num7 - num10, num8 - num9);
        Matrix matrix = new Matrix(num4 * xRadius, -num5 * xRadius, num5 * yRadius, num4 * yRadius, 0.5 * (xEnd + xStart), 0.5 * (yEnd + yStart));
        if (!flag)
        {
          matrix.OffsetX += matrix.M11 * num10 + matrix.M21 * num9;
          matrix.OffsetY += matrix.M12 * num10 + matrix.M22 * num9;
        }
        double rCosArcAngle;
        double rSinArcAngle;
        PathSegmentHelper.ArcToBezierHelper.GetArcAngle(point1, point2, fLargeArc, fSweepUp, out rCosArcAngle, out rSinArcAngle, out cPieces);
        double num13 = PathSegmentHelper.ArcToBezierHelper.GetBezierDistance(rCosArcAngle);
        if (!fSweepUp)
          num13 = -num13;
        Point rhs1 = new Point(-num13 * point1.Y, num13 * point1.X);
        int num14 = 0;
        pPt = new Point[cPieces * 3];
        Point rhs2;
        for (int index1 = 1; index1 < cPieces; ++index1)
        {
          Point point3 = new Point(point1.X * rCosArcAngle - point1.Y * rSinArcAngle, point1.X * rSinArcAngle + point1.Y * rCosArcAngle);
          rhs2 = new Point(-num13 * point3.Y, num13 * point3.X);
          Point[] pointArray1 = pPt;
          int index2 = num14;
          int num15 = index2 + 1;
          Point point4 = matrix.Transform(point1.Plus(rhs1));
          pointArray1[index2] = point4;
          Point[] pointArray2 = pPt;
          int index3 = num15;
          int num16 = index3 + 1;
          Point point5 = matrix.Transform(point3.Minus(rhs2));
          pointArray2[index3] = point5;
          Point[] pointArray3 = pPt;
          int index4 = num16;
          num14 = index4 + 1;
          Point point6 = matrix.Transform(point3);
          pointArray3[index4] = point6;
          point1 = point3;
          rhs1 = rhs2;
        }
        rhs2 = new Point(-num13 * point2.Y, num13 * point2.X);
        Point[] pointArray4 = pPt;
        int index5 = num14;
        int num17 = index5 + 1;
        Point point7 = matrix.Transform(point1.Plus(rhs1));
        pointArray4[index5] = point7;
        Point[] pointArray5 = pPt;
        int index6 = num17;
        int index7 = index6 + 1;
        Point point8 = matrix.Transform(point2.Minus(rhs2));
        pointArray5[index6] = point8;
        pPt[index7] = new Point(xEnd, yEnd);
      }
    }

    private static void GetArcAngle(
      Point ptStart,
      Point ptEnd,
      bool fLargeArc,
      bool fSweepUp,
      out double rCosArcAngle,
      out double rSinArcAngle,
      out int cPieces)
    {
      rCosArcAngle = GeometryHelper.Dot(ptStart, ptEnd);
      rSinArcAngle = GeometryHelper.Determinant(ptStart, ptEnd);
      if (rCosArcAngle >= 0.0)
      {
        if (!fLargeArc)
        {
          cPieces = 1;
          return;
        }
        cPieces = 4;
      }
      else
        cPieces = !fLargeArc ? 2 : 3;
      double num1 = Math.Atan2(rSinArcAngle, rCosArcAngle);
      if (fSweepUp)
      {
        if (num1 < 0.0)
          num1 += 2.0 * Math.PI;
      }
      else if (num1 > 0.0)
        num1 -= 2.0 * Math.PI;
      double num2 = num1 / (double) cPieces;
      rCosArcAngle = Math.Cos(num2);
      rSinArcAngle = Math.Sin(num2);
    }

    private static double GetBezierDistance(double rDot, double rRadius = 1.0)
    {
      double num1 = rRadius * rRadius;
      double bezierDistance = 0.0;
      double d1 = 0.5 * (num1 + rDot);
      if (d1 < 0.0)
        return bezierDistance;
      double d2 = num1 - d1;
      if (d2 <= 0.0)
        return bezierDistance;
      double num2 = Math.Sqrt(d2);
      double num3 = 4.0 * (rRadius - Math.Sqrt(d1)) / 3.0;
      return num3 <= num2 * 1E-06 ? 0.0 : num3 / num2;
    }
  }

  private class BezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private BezierSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Point1 = transform.Transform(this._segment.Point1);
      this._segment.Point2 = transform.Transform(this._segment.Point2);
      this._segment.Point3 = transform.Transform(this._segment.Point3);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(BezierSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.BezierSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      Point[] controlPoints = new Point[4]
      {
        this.Start,
        this._segment.Point1,
        this._segment.Point2,
        this._segment.Point3
      };
      List<Point> pointList = new List<Point>();
      BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, (ICollection<Point>) pointList, true);
      points.AddRange<Point>((IEnumerable<Point>) pointList);
    }

    public override Point GetPoint(int index)
    {
      if (index < -1 || index > 2)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (index == 0)
        return this._segment.Point1;
      return index == 1 ? this._segment.Point2 : this._segment.Point3;
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      PathSegmentHelper.BezierSegmentImplementation segmentImplementation = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = SimpleSegment.Create(segmentImplementation.Start, segmentImplementation._segment.Point1, segmentImplementation._segment.Point2, segmentImplementation._segment.Point3);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }
  }

  private class LineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private LineSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Point = transform.Transform(this._segment.Point);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(LineSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.LineSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      points.Add(this._segment.Point);
    }

    public override Point GetPoint(int index)
    {
      if (index < -1 || index > 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this._segment.Point;
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      PathSegmentHelper.LineSegmentImplementation segmentImplementation = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = SimpleSegment.Create(segmentImplementation.Start, segmentImplementation._segment.Point);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }
  }

  private abstract class PathSegmentImplementation
  {
    protected Point Start { get; private set; }

    public abstract PathSegment ApplyTransform(GeneralTransform transform);

    public static PathSegmentHelper.PathSegmentImplementation Create(PathSegment segment)
    {
      PathSegmentHelper.PathSegmentImplementation segmentImplementation;
      if ((segmentImplementation = PathSegmentHelper.BezierSegmentImplementation.Create(segment as BezierSegment)) == null && (segmentImplementation = PathSegmentHelper.LineSegmentImplementation.Create(segment as LineSegment)) == null && (segmentImplementation = PathSegmentHelper.ArcSegmentImplementation.Create(segment as ArcSegment)) == null && (segmentImplementation = PathSegmentHelper.PolyLineSegmentImplementation.Create(segment as PolyLineSegment)) == null && (segmentImplementation = PathSegmentHelper.PolyBezierSegmentImplementation.Create(segment as PolyBezierSegment)) == null && (segmentImplementation = PathSegmentHelper.QuadraticBezierSegmentImplementation.Create(segment as QuadraticBezierSegment)) == null && (segmentImplementation = PathSegmentHelper.PolyQuadraticBezierSegmentImplementation.Create(segment as PolyQuadraticBezierSegment)) == null)
        throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeNotSupported, new object[1]
        {
          (object) segment.GetType().FullName
        }));
      return segmentImplementation;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(
      PathSegment segment,
      Point start)
    {
      PathSegmentHelper.PathSegmentImplementation segmentImplementation = PathSegmentHelper.PathSegmentImplementation.Create(segment);
      segmentImplementation.Start = start;
      return segmentImplementation;
    }

    public abstract void Flatten(IList<Point> points, double tolerance);

    public abstract Point GetPoint(int index);

    public abstract IEnumerable<SimpleSegment> GetSimpleSegments();
  }

  private class PolyBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private PolyBezierSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Points.ApplyTransform(transform);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(PolyBezierSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.PolyBezierSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      Point point = this.Start;
      int num = this._segment.Points.Count / 3 * 3;
      for (int index = 0; index < num; index += 3)
      {
        Point[] controlPoints = new Point[4]
        {
          point,
          this._segment.Points[index],
          this._segment.Points[index + 1],
          this._segment.Points[index + 2]
        };
        List<Point> pointList = new List<Point>();
        BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, (ICollection<Point>) pointList, true);
        points.AddRange<Point>((IEnumerable<Point>) pointList);
        point = this._segment.Points[index + 2];
      }
    }

    public override Point GetPoint(int index)
    {
      int num = this._segment.Points.Count / 3 * 3;
      if (index < -1 || index > num - 1)
        throw new ArgumentOutOfRangeException(nameof (index));
      return index != -1 ? this._segment.Points[index] : this._segment.Points[num - 1];
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      PathSegmentHelper.PolyBezierSegmentImplementation segmentImplementation = this;
      Point start = segmentImplementation.Start;
      IList<Point> points = (IList<Point>) segmentImplementation._segment.Points;
      int iteratorVariable2 = segmentImplementation._segment.Points.Count / 3;
      for (int iteratorVariable3 = 0; iteratorVariable3 < iteratorVariable2; ++iteratorVariable3)
      {
        int iteratorVariable4 = iteratorVariable3 * 3;
        yield return SimpleSegment.Create(start, points[iteratorVariable4], points[iteratorVariable4 + 1], points[iteratorVariable4 + 2]);
        start = points[iteratorVariable4 + 2];
      }
    }
  }

  private class PolyLineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private PolyLineSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Points.ApplyTransform(transform);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(PolyLineSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.PolyLineSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      points.AddRange<Point>((IEnumerable<Point>) this._segment.Points);
    }

    public override Point GetPoint(int index)
    {
      if (index < -1 || index > this._segment.Points.Count - 1)
        throw new ArgumentOutOfRangeException(nameof (index));
      return index != -1 ? this._segment.Points[index] : this._segment.Points.Last<Point>();
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      PathSegmentHelper.PolyLineSegmentImplementation segmentImplementation = this;
      Point point0 = segmentImplementation.Start;
      PointCollection.Enumerator enumerator = segmentImplementation._segment.Points.GetEnumerator();
      while (enumerator.MoveNext())
      {
        Point current = enumerator.Current;
        yield return SimpleSegment.Create(point0, current);
        point0 = current;
        current = new Point();
      }
    }
  }

  private class PolyQuadraticBezierSegmentImplementation : 
    PathSegmentHelper.PathSegmentImplementation
  {
    private PolyQuadraticBezierSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Points.ApplyTransform(transform);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(
      PolyQuadraticBezierSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.PolyQuadraticBezierSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      Point point = this.Start;
      int num = this._segment.Points.Count / 2 * 2;
      for (int index = 0; index < num; index += 2)
      {
        Point[] controlPoints = new Point[3]
        {
          point,
          this._segment.Points[index],
          this._segment.Points[index + 1]
        };
        List<Point> pointList = new List<Point>();
        BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, (ICollection<Point>) pointList, true);
        points.AddRange<Point>((IEnumerable<Point>) pointList);
        point = this._segment.Points[index + 1];
      }
    }

    public override Point GetPoint(int index)
    {
      int num = this._segment.Points.Count / 2 * 2;
      if (index < -1 || index > num - 1)
        throw new ArgumentOutOfRangeException(nameof (index));
      return index != -1 ? this._segment.Points[index] : this._segment.Points[num - 1];
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      PathSegmentHelper.PolyQuadraticBezierSegmentImplementation segmentImplementation = this;
      Point start = segmentImplementation.Start;
      IList<Point> points = (IList<Point>) segmentImplementation._segment.Points;
      int iteratorVariable2 = segmentImplementation._segment.Points.Count / 2;
      for (int iteratorVariable3 = 0; iteratorVariable3 < iteratorVariable2; ++iteratorVariable3)
      {
        int iteratorVariable4 = iteratorVariable3 * 2;
        yield return SimpleSegment.Create(start, points[iteratorVariable4], points[iteratorVariable4 + 1]);
        start = points[iteratorVariable4 + 1];
      }
    }
  }

  private class QuadraticBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
  {
    private QuadraticBezierSegment _segment;

    public override PathSegment ApplyTransform(GeneralTransform transform)
    {
      this._segment.Point1 = transform.Transform(this._segment.Point1);
      this._segment.Point2 = transform.Transform(this._segment.Point2);
      return (PathSegment) this._segment;
    }

    public static PathSegmentHelper.PathSegmentImplementation Create(QuadraticBezierSegment source)
    {
      if (source == null)
        return (PathSegmentHelper.PathSegmentImplementation) null;
      return (PathSegmentHelper.PathSegmentImplementation) new PathSegmentHelper.QuadraticBezierSegmentImplementation()
      {
        _segment = source
      };
    }

    public override void Flatten(IList<Point> points, double tolerance)
    {
      Point[] controlPoints = new Point[3]
      {
        this.Start,
        this._segment.Point1,
        this._segment.Point2
      };
      List<Point> pointList = new List<Point>();
      BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, (ICollection<Point>) pointList, true);
      points.AddRange<Point>((IEnumerable<Point>) pointList);
    }

    public override Point GetPoint(int index)
    {
      if (index < -1 || index > 1)
        throw new ArgumentOutOfRangeException(nameof (index));
      return index != 0 ? this._segment.Point2 : this._segment.Point1;
    }

    public override IEnumerable<SimpleSegment> GetSimpleSegments()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      PathSegmentHelper.QuadraticBezierSegmentImplementation segmentImplementation = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = SimpleSegment.Create(segmentImplementation.Start, segmentImplementation._segment.Point1, segmentImplementation._segment.Point2);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }
  }
}
