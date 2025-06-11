// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.BezierCurveFlattener
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class BezierCurveFlattener
{
  public const double StandardFlatteningTolerance = 0.25;

  private static void DoCubicForwardDifferencing(
    Point[] controlPoints,
    double leftParameter,
    double rightParameter,
    double inverseErrorTolerance,
    ICollection<Point> resultPolyline,
    ICollection<double> resultParameters)
  {
    double num1 = controlPoints[1].X - controlPoints[0].X;
    double num2 = controlPoints[1].Y - controlPoints[0].Y;
    double num3 = controlPoints[2].X - controlPoints[1].X;
    double num4 = controlPoints[2].Y - controlPoints[1].Y;
    double num5 = controlPoints[3].X - controlPoints[2].X;
    double num6 = controlPoints[3].Y - controlPoints[2].Y;
    double num7 = num3 - num1;
    double num8 = num4 - num2;
    double num9 = num5 - num3;
    double num10 = num4;
    double num11 = num6 - num10;
    double num12 = num9 - num7;
    double num13 = num11 - num8;
    Vector vector = controlPoints[3].Subtract(controlPoints[0]);
    double length = vector.Length;
    double num14 = MathHelper.IsVerySmall(length) ? Math.Max(0.0, Math.Max(GeometryHelper.Distance(controlPoints[1], controlPoints[0]), GeometryHelper.Distance(controlPoints[2], controlPoints[0]))) : Math.Max(0.0, Math.Max(Math.Abs((num7 * vector.Y - num8 * vector.X) / length), Math.Abs((num9 * vector.Y - num11 * vector.X) / length)));
    uint num15 = 0;
    if (num14 > 0.0)
    {
      double d = num14 * inverseErrorTolerance;
      num15 = d < (double) int.MaxValue ? BezierCurveFlattener.Log4UnsignedInt32((uint) (d + 0.5)) : BezierCurveFlattener.Log4Double(d);
    }
    int exp1 = (int) -num15;
    int exp2 = exp1 + exp1;
    int exp3 = exp2 + exp1;
    double num16 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num7, exp2);
    double num17 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num8, exp2);
    double num18 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num12, exp3);
    double num19 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num13, exp3);
    double num20 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num1, exp1) + num16 + 1.0 / 6.0 * num18;
    double num21 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num2, exp1) + num17 + 1.0 / 6.0 * num19;
    double num22 = 2.0 * num16 + num18;
    double num23 = 2.0 * num17 + num19;
    double x = controlPoints[0].X;
    double y = controlPoints[0].Y;
    Point point = new Point(0.0, 0.0);
    int num24 = 1 << (int) num15;
    double num25 = num24 > 0 ? (rightParameter - leftParameter) / (double) num24 : 0.0;
    double num26 = leftParameter;
    for (int index = 1; index < num24; ++index)
    {
      x += num20;
      y += num21;
      point.X = x;
      point.Y = y;
      resultPolyline.Add(point);
      num26 += num25;
      resultParameters?.Add(num26);
      num20 += num22;
      num21 += num23;
      num22 += num18;
      num23 += num19;
    }
  }

  private static void DoCubicMidpointSubdivision(
    Point[] controlPoints,
    uint depth,
    double leftParameter,
    double rightParameter,
    double inverseErrorTolerance,
    ICollection<Point> resultPolyline,
    ICollection<double> resultParameters)
  {
    Point[] controlPoints1 = new Point[4]
    {
      controlPoints[0],
      controlPoints[1],
      controlPoints[2],
      controlPoints[3]
    };
    Point[] controlPoints2 = new Point[4];
    controlPoints2[3] = controlPoints1[3];
    controlPoints1[3] = GeometryHelper.Midpoint(controlPoints1[3], controlPoints1[2]);
    controlPoints1[2] = GeometryHelper.Midpoint(controlPoints1[2], controlPoints1[1]);
    controlPoints1[1] = GeometryHelper.Midpoint(controlPoints1[1], controlPoints1[0]);
    controlPoints2[2] = controlPoints1[3];
    controlPoints1[3] = GeometryHelper.Midpoint(controlPoints1[3], controlPoints1[2]);
    controlPoints1[2] = GeometryHelper.Midpoint(controlPoints1[2], controlPoints1[1]);
    controlPoints2[1] = controlPoints1[3];
    controlPoints1[3] = GeometryHelper.Midpoint(controlPoints1[3], controlPoints1[2]);
    controlPoints2[0] = controlPoints1[3];
    --depth;
    double num = (leftParameter + rightParameter) * 0.5;
    if (depth > 0U)
    {
      BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints1, depth, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
      resultPolyline.Add(controlPoints2[0]);
      resultParameters?.Add(num);
      BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints2, depth, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
    }
    else
    {
      BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints1, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
      resultPolyline.Add(controlPoints2[0]);
      resultParameters?.Add(num);
      BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints2, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
    }
  }

  private static void EnsureErrorTolerance(ref double errorTolerance)
  {
    if (errorTolerance > 0.0)
      return;
    errorTolerance = 0.25;
  }

  public static void FlattenCubic(
    Point[] controlPoints,
    double errorTolerance,
    ICollection<Point> resultPolyline,
    bool skipFirstPoint,
    ICollection<double> resultParameters = null)
  {
    if (resultPolyline == null)
      throw new ArgumentNullException(nameof (resultPolyline));
    if (controlPoints == null)
      throw new ArgumentNullException(nameof (controlPoints));
    if (controlPoints.Length != 4)
      throw new ArgumentOutOfRangeException(nameof (controlPoints));
    BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
    if (!skipFirstPoint)
    {
      resultPolyline.Add(controlPoints[0]);
      resultParameters?.Add(0.0);
    }
    if (BezierCurveFlattener.IsCubicChordMonotone(controlPoints, errorTolerance * errorTolerance))
    {
      BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener differencingCubicFlattener = new BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener(controlPoints, errorTolerance, errorTolerance, true);
      Point p = new Point();
      double u = 0.0;
      while (differencingCubicFlattener.Next(ref p, ref u))
      {
        resultPolyline.Add(p);
        resultParameters?.Add(u);
      }
    }
    else
    {
      double x = controlPoints[3].X - controlPoints[2].X + controlPoints[1].X - controlPoints[0].X;
      double num1 = controlPoints[3].Y - controlPoints[2].Y + controlPoints[1].Y - controlPoints[0].Y;
      double num2 = 1.0 / errorTolerance;
      double y = num1;
      uint depth = BezierCurveFlattener.Log8UnsignedInt32((uint) (MathHelper.Hypotenuse(x, y) * num2 + 0.5));
      if (depth > 0U)
        --depth;
      if (depth > 0U)
        BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints, depth, 0.0, 1.0, 0.75 * num2, resultPolyline, resultParameters);
      else
        BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints, 0.0, 1.0, 0.75 * num2, resultPolyline, resultParameters);
    }
    resultPolyline.Add(controlPoints[3]);
    resultParameters?.Add(1.0);
  }

  public static void FlattenQuadratic(
    Point[] controlPoints,
    double errorTolerance,
    ICollection<Point> resultPolyline,
    bool skipFirstPoint,
    ICollection<double> resultParameters = null)
  {
    if (resultPolyline == null)
      throw new ArgumentNullException(nameof (resultPolyline));
    if (controlPoints == null)
      throw new ArgumentNullException(nameof (controlPoints));
    if (controlPoints.Length != 3)
      throw new ArgumentOutOfRangeException(nameof (controlPoints));
    BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
    BezierCurveFlattener.FlattenCubic(new Point[4]
    {
      controlPoints[0],
      GeometryHelper.Lerp(controlPoints[0], controlPoints[1], 2.0 / 3.0),
      GeometryHelper.Lerp(controlPoints[1], controlPoints[2], 1.0 / 3.0),
      controlPoints[2]
    }, errorTolerance, resultPolyline, skipFirstPoint, resultParameters);
  }

  private static bool IsCubicChordMonotone(Point[] controlPoints, double squaredTolerance)
  {
    double num1 = GeometryHelper.SquaredDistance(controlPoints[0], controlPoints[3]);
    if (num1 <= squaredTolerance)
      return false;
    Vector lhs = controlPoints[3].Subtract(controlPoints[0]);
    Vector rhs1 = controlPoints[1].Subtract(controlPoints[0]);
    double num2 = GeometryHelper.Dot(lhs, rhs1);
    if (num2 < 0.0 || num2 > num1)
      return false;
    Vector rhs2 = controlPoints[2].Subtract(controlPoints[0]);
    double num3 = GeometryHelper.Dot(lhs, rhs2);
    return num3 >= 0.0 && num3 <= num1 && num2 <= num3;
  }

  private static uint Log4Double(double d)
  {
    uint num = 0;
    while (d > 1.0)
    {
      d *= 0.25;
      ++num;
    }
    return num;
  }

  private static uint Log4UnsignedInt32(uint i)
  {
    uint num = 0;
    while (i > 0U)
    {
      i >>= 2;
      ++num;
    }
    return num;
  }

  private static uint Log8UnsignedInt32(uint i)
  {
    uint num = 0;
    while (i > 0U)
    {
      i >>= 3;
      ++num;
    }
    return num;
  }

  private class AdaptiveForwardDifferencingCubicFlattener
  {
    private double _aX;
    private double _aY;
    private double _bX;
    private double _bY;
    private double _cX;
    private double _cY;
    private readonly double _distanceTolerance;
    private readonly bool _doParameters;
    private double _dParameter;
    private double _dX;
    private double _dY;
    private readonly double _flatnessTolerance;
    private int _numSteps;
    private double _parameter;

    internal AdaptiveForwardDifferencingCubicFlattener(
      Point[] controlPoints,
      double flatnessTolerance,
      double distanceTolerance,
      bool doParameters)
    {
      this._numSteps = 1;
      this._dParameter = 1.0;
      this._flatnessTolerance = 3.0 * flatnessTolerance;
      this._distanceTolerance = distanceTolerance;
      this._doParameters = doParameters;
      this._aX = -controlPoints[0].X + 3.0 * (controlPoints[1].X - controlPoints[2].X) + controlPoints[3].X;
      this._aY = -controlPoints[0].Y + 3.0 * (controlPoints[1].Y - controlPoints[2].Y) + controlPoints[3].Y;
      this._bX = 3.0 * (controlPoints[0].X - 2.0 * controlPoints[1].X + controlPoints[2].X);
      this._bY = 3.0 * (controlPoints[0].Y - 2.0 * controlPoints[1].Y + controlPoints[2].Y);
      this._cX = 3.0 * (-controlPoints[0].X + controlPoints[1].X);
      this._cY = 3.0 * (-controlPoints[0].Y + controlPoints[1].Y);
      this._dX = controlPoints[0].X;
      this._dY = controlPoints[0].Y;
    }

    private void DoubleStepSize()
    {
      this._aX *= 8.0;
      this._aY *= 8.0;
      this._bX *= 4.0;
      this._bY *= 4.0;
      this._cX += this._cX;
      this._cY += this._cY;
      if (this._doParameters)
        this._dParameter *= 2.0;
      this._numSteps >>= 1;
    }

    private void HalveStepSize()
    {
      this._aX *= 0.125;
      this._aY *= 0.125;
      this._bX *= 0.25;
      this._bY *= 0.25;
      this._cX *= 0.5;
      this._cY *= 0.5;
      if (this._doParameters)
        this._dParameter *= 0.5;
      this._numSteps <<= 1;
    }

    private void IncrementDifferencesAndParameter()
    {
      this._dX = this._aX + this._bX + this._cX + this._dX;
      this._dY = this._aY + this._bY + this._cY + this._dY;
      this._cX = this._aX + this._aX + this._aX + this._bX + this._bX + this._cX;
      this._cY = this._aY + this._aY + this._aY + this._bY + this._bY + this._cY;
      this._bX = this._aX + this._aX + this._aX + this._bX;
      this._bY = this._aY + this._aY + this._aY + this._bY;
      --this._numSteps;
      this._parameter += this._dParameter;
    }

    private bool MustSubdivide(double flatnessTolerance)
    {
      double num1 = -(this._aY + this._bY + this._cY);
      double num2 = this._aX + this._bX + this._cX;
      double num3 = Math.Abs(num1) + Math.Abs(num2);
      if (num3 <= this._distanceTolerance)
        return false;
      double num4 = num3 * flatnessTolerance;
      return Math.Abs(this._cX * num1 + this._cY * num2) > num4 || Math.Abs((this._bX + this._cX + this._cX) * num1 + (this._bY + this._cY + this._cY) * num2) > num4;
    }

    internal bool Next(ref Point p, ref double u)
    {
      while (this.MustSubdivide(this._flatnessTolerance))
        this.HalveStepSize();
      if ((this._numSteps & 1) == 0)
      {
        while (this._numSteps > 1 && !this.MustSubdivide(this._flatnessTolerance * 0.25))
          this.DoubleStepSize();
      }
      this.IncrementDifferencesAndParameter();
      p.X = this._dX;
      p.Y = this._dY;
      u = this._parameter;
      return this._numSteps != 0;
    }
  }
}
