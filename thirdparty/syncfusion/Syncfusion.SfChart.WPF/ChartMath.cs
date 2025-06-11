// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartMath
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class ChartMath
{
  internal const float MARGINS_RATIO = 0.03f;
  public const double ToDegree = 57.295779513082323;
  public const double ToRadial = 0.017453292519943295;
  public const double Percent = 0.01;
  public const double DoublePI = 6.2831853071795862;
  public const double HalfPI = 1.5707963267948966;
  public const double OneAndHalfPI = 4.71238898038469;
  public const double Epsilon = 1E-05;

  internal static object GetField(this DataRow dataRow, string ColumnName)
  {
    return dataRow.Field<object>(ColumnName);
  }

  public static bool IntersectWith(this IList<Rect> rectCollection, Rect newRect)
  {
    foreach (Rect rect in rectCollection.Reverse<Rect>())
    {
      if (rect.IntersectsWith(newRect))
        return true;
    }
    return false;
  }

  internal static double GetInterpolarationPoint(
    double x1,
    double x2,
    double y1,
    double y2,
    double x)
  {
    double num = x2 == x1 ? 1.0 : x2 - x1;
    return (y2 - y1) / num * (x - x1) + y1;
  }

  public static Vector3D GetNormal(Vector3D v1, Vector3D v2, Vector3D v3)
  {
    Vector3D vector3D = (v1 - v2) * (v3 - v2);
    double num = vector3D.GetLength();
    if (num < 1E-05)
      num = 0.0;
    return new Vector3D(vector3D.X / num, vector3D.Y / num, vector3D.Z / num);
  }

  public static TranslateTransform Translate(Point startPoint, Point currentPoint)
  {
    return new TranslateTransform()
    {
      X = currentPoint.X - startPoint.X,
      Y = currentPoint.Y - startPoint.Y
    };
  }

  public static bool SolveQuadraticEquation(
    double a,
    double b,
    double c,
    out double root1,
    out double root2)
  {
    root1 = 0.0;
    root2 = 0.0;
    if (a != 0.0)
    {
      double d = b * b - 4.0 * a * c;
      if (d >= 0.0)
      {
        double num = Math.Sqrt(d);
        root1 = (-b - num) / (2.0 * a);
        root2 = (-b + num) / (2.0 * a);
        return true;
      }
    }
    else if (b != 0.0)
    {
      root1 = -c / b;
      root2 = -c / b;
      return true;
    }
    return false;
  }

  public static double MinMax(double value, double min, double max)
  {
    if (value > max)
      return max;
    return value >= min ? value : min;
  }

  public static double Min(params double[] values)
  {
    double val1 = values[0];
    for (int index = 1; index < values.Length; ++index)
      val1 = Math.Min(val1, values[index]);
    return val1;
  }

  public static double Max(params double[] values)
  {
    double val1 = values[0];
    for (int index = 1; index < values.Length; ++index)
      val1 = Math.Max(val1, values[index]);
    return val1;
  }

  public static double MaxZero(double value) => value <= 0.0 ? 0.0 : value;

  public static double MinZero(double value) => value >= 0.0 ? 0.0 : value;

  public static double Round(double x, double div, bool up)
  {
    return (up ? (double) (int) Math.Ceiling(x / div) : (double) (int) Math.Floor(x / div)) * div;
  }

  internal static ChartPoint? GetCrossPoint(
    ChartPoint p11,
    ChartPoint p12,
    ChartPoint p21,
    ChartPoint p22)
  {
    ChartPoint chartPoint = new ChartPoint();
    double num1 = (p12.Y - p11.Y) * (p21.X - p22.X) - (p21.Y - p22.Y) * (p12.X - p11.X);
    double num2 = (p12.Y - p11.Y) * (p21.X - p11.X) - (p21.Y - p11.Y) * (p12.X - p11.X);
    double num3 = (p21.Y - p11.Y) * (p21.X - p22.X) - (p21.Y - p22.Y) * (p21.X - p11.X);
    if (num1 == 0.0 && num2 == 0.0 && num3 == 0.0)
      return new ChartPoint?();
    double num4 = num2 / num1;
    double num5 = num3 / num1;
    chartPoint.X = p11.X + (p12.X - p11.X) * num5;
    chartPoint.Y = p11.Y + (p12.Y - p11.Y) * num5;
    return 0.0 <= num4 && num4 <= 1.0 && 0.0 <= num5 && num5 <= 1.0 ? new ChartPoint?(chartPoint) : new ChartPoint?();
  }

  internal static double GetAngle(Point startPoint, Point endPoint)
  {
    double num = Math.Atan2(-(endPoint.Y - startPoint.Y), endPoint.X - startPoint.X);
    return (num < 0.0 ? Math.Abs(num) : 2.0 * Math.PI - num) * (180.0 / Math.PI);
  }

  private static double CalcPerpendicularDistance(Point Point1, Point Point2, Point Point)
  {
    return Math.Abs(0.5 * (Point1.X * Point2.Y + Point2.X * Point.Y + Point.X * Point1.Y - Point2.X * Point1.Y - Point.X * Point2.Y - Point1.X * Point.Y)) / Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2.0) + Math.Pow(Point1.Y - Point2.Y, 2.0)) * 2.0;
  }

  public static Point GeneralPointRotation(Point originpoint, Point endpoint, double angle)
  {
    double num1 = angle * Math.PI / 180.0;
    double num2 = endpoint.X / 2.0;
    endpoint.X = num2 * Math.Cos(num1);
    endpoint.Y = num2 * Math.Sin(num1);
    return endpoint;
  }

  internal static bool IsPointInsideRectangle(Point point, Point[] rectanglePoints)
  {
    double rectangleArea = ChartMath.CalculateRectangleArea(rectanglePoints[0], rectanglePoints[1], rectanglePoints[2], rectanglePoints[3]);
    double triangleArea1 = ChartMath.CalculateTriangleArea(point, rectanglePoints[0], rectanglePoints[1]);
    double triangleArea2 = ChartMath.CalculateTriangleArea(point, rectanglePoints[1], rectanglePoints[2]);
    double triangleArea3 = ChartMath.CalculateTriangleArea(point, rectanglePoints[2], rectanglePoints[3]);
    double triangleArea4 = ChartMath.CalculateTriangleArea(point, rectanglePoints[3], rectanglePoints[0]);
    return Math.Round(rectangleArea) >= Math.Round(triangleArea1 + triangleArea2 + triangleArea3 + triangleArea4);
  }

  internal static double CalculateTriangleArea(Point p1, Point p2, Point p3)
  {
    return 0.5 * Math.Abs(p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y));
  }

  internal static double CalculateRectangleArea(Point p1, Point p2, Point p3, Point p4)
  {
    return 0.5 * Math.Abs((p1.Y - p3.Y) * (p4.X - p2.X) + (p2.Y - p4.Y) * (p1.X - p3.X));
  }

  internal static double CalculateDistanceBetweenTwoPoints(Point p1, Point p2)
  {
    return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
  }

  internal static Point CalculatePerpendicularDistantPoint(Point p1, Point p2, double d)
  {
    double num1 = p1.X - p2.X;
    double num2 = p1.Y - p2.Y;
    double betweenTwoPoints = ChartMath.CalculateDistanceBetweenTwoPoints(p1, p2);
    double num3 = num1 / betweenTwoPoints;
    double num4 = num2 / betweenTwoPoints;
    return new Point(p1.X + d * num4, p1.Y - d * num3);
  }

  internal static bool IsPointInsideCircle(Point circleCenter, double radius, Point testPoint)
  {
    double num1 = testPoint.X - circleCenter.X;
    double num2 = testPoint.Y - circleCenter.Y;
    return Math.Sqrt(num1 * num1 + num2 * num2) < radius;
  }

  internal static double ParseAtInvarientCulture(double value)
  {
    double result;
    return !double.TryParse(value.ToString((IFormatProvider) CultureInfo.InvariantCulture), out result) ? value : result;
  }
}
