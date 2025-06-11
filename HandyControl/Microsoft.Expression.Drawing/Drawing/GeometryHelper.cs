// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.GeometryHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Drawing;

public static class GeometryHelper
{
  internal static void ApplyTransform(this IList<Point> points, GeneralTransform transform)
  {
    for (int index = 0; index < points.Count; ++index)
      points[index] = transform.Transform(points[index]);
  }

  internal static Rect Bounds(this Size size) => new Rect(0.0, 0.0, size.Width, size.Height);

  internal static Point Center(this Rect rect)
  {
    return new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
  }

  internal static Vector Subtract(this Point lhs, Point rhs)
  {
    return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
  }

  internal static Thickness Subtract(this Rect lhs, Rect rhs)
  {
    return new Thickness(rhs.Left - lhs.Left, rhs.Top - lhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom);
  }

  internal static Point Lerp(Point pointA, Point pointB, double alpha)
  {
    return new Point(MathHelper.Lerp(pointA.X, pointB.X, alpha), MathHelper.Lerp(pointA.Y, pointB.Y, alpha));
  }

  internal static Vector Lerp(Vector vectorA, Vector vectorB, double alpha)
  {
    return new Vector(MathHelper.Lerp(vectorA.X, vectorB.X, alpha), MathHelper.Lerp(vectorA.Y, vectorB.Y, alpha));
  }

  internal static double Distance(Point lhs, Point rhs)
  {
    double num1 = lhs.X - rhs.X;
    double num2 = lhs.Y - rhs.Y;
    return Math.Sqrt(num1 * num1 + num2 * num2);
  }

  internal static Rect Resize(this Rect rect, double ratio) => rect.Resize(ratio, ratio);

  internal static Rect Resize(this Rect rect, double ratioX, double ratioY)
  {
    Point point = rect.Center();
    double width = rect.Width * ratioX;
    double height = rect.Height * ratioY;
    return new Rect(point.X - width / 2.0, point.Y - height / 2.0, width, height);
  }

  internal static Rect GetStretchBound(Rect logicalBound, Stretch stretch, Size aspectRatio)
  {
    if (stretch == Stretch.None)
      stretch = Stretch.Fill;
    if (stretch == Stretch.Fill || !aspectRatio.HasValidArea())
      return logicalBound;
    Point point = logicalBound.Center();
    switch (stretch)
    {
      case Stretch.Uniform:
        if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
        {
          logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
          break;
        }
        logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
        break;
      case Stretch.UniformToFill:
        if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
        {
          logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
          break;
        }
        logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
        break;
    }
    return new Rect(point.X - logicalBound.Width / 2.0, point.Y - logicalBound.Height / 2.0, logicalBound.Width, logicalBound.Height);
  }

  internal static Point GetArcPoint(double degree)
  {
    double num = degree * Math.PI / 180.0;
    return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  internal static Point GetArcPoint(double degree, Rect bound)
  {
    Point arcPoint = GeometryHelper.GetArcPoint(degree);
    return GeometryHelper.RelativeToAbsolutePoint(bound, arcPoint);
  }

  internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
  {
    return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
  }

  internal static double SquaredDistance(Point lhs, Point rhs)
  {
    double num1 = lhs.X - rhs.X;
    double num2 = lhs.Y - rhs.Y;
    return num1 * num1 + num2 * num2;
  }

  internal static Point Midpoint(Point lhs, Point rhs)
  {
    return new Point((lhs.X + rhs.X) / 2.0, (lhs.Y + rhs.Y) / 2.0);
  }

  internal static bool HasValidArea(this Size size)
  {
    return size.Width > 0.0 && size.Height > 0.0 && !double.IsInfinity(size.Width) && !double.IsInfinity(size.Height);
  }

  internal static Rect Inflate(Rect rect, double offset)
  {
    return GeometryHelper.Inflate(rect, new Thickness(offset));
  }

  internal static Rect Inflate(Rect rect, Thickness thickness)
  {
    double width = rect.Width + thickness.Left + thickness.Right;
    double height = rect.Height + thickness.Top + thickness.Bottom;
    double x = rect.X - thickness.Left;
    if (width < 0.0)
    {
      x += width / 2.0;
      width = 0.0;
    }
    double y = rect.Y - thickness.Top;
    if (height < 0.0)
    {
      y += height / 2.0;
      height = 0.0;
    }
    return new Rect(x, y, width, height);
  }

  internal static double Dot(Point lhs, Point rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

  internal static double Dot(Vector lhs, Vector rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

  internal static Point Plus(this Point lhs, Point rhs) => new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);

  internal static Point Minus(this Point lhs, Point rhs) => new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);

  internal static Vector Normal(Point lhs, Point rhs)
  {
    return new Vector(lhs.Y - rhs.Y, rhs.X - lhs.X).Normalized();
  }

  internal static Vector Normalized(this Vector vector)
  {
    Vector vector1 = new Vector(vector.X, vector.Y);
    double length = vector1.Length;
    return MathHelper.IsVerySmall(length) ? new Vector(0.0, 1.0) : vector1 / length;
  }

  internal static double Determinant(Point lhs, Point rhs) => lhs.X * rhs.Y - lhs.Y * rhs.X;

  internal static bool EnsureSegmentType<T>(
    out T result,
    IList<PathSegment> list,
    int index,
    Func<T> factory)
    where T : PathSegment
  {
    result = list[index] as T;
    if ((object) result != null)
      return false;
    list[index] = (PathSegment) (result = factory());
    return true;
  }

  internal static bool EnsureGeometryType<T>(out T result, ref Geometry value, Func<T> factory) where T : Geometry
  {
    result = value as T;
    if ((object) result != null)
      return false;
    value = (Geometry) (result = factory());
    return true;
  }
}
