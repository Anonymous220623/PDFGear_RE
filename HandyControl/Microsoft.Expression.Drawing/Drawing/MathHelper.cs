// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.MathHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class MathHelper
{
  internal const double DBL_EPSILON = 2.2204460492503131E-16;

  public static bool AreClose(double value1, double value2)
  {
    return value1 == value2 || MathHelper.IsVerySmall(value1 - value2);
  }

  public static double Lerp(double x, double y, double alpha) => x * (1.0 - alpha) + y * alpha;

  public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;

  public static bool IsZero(double value) => Math.Abs(value) < 2.2204460492503131E-15;

  public static bool IsFiniteDouble(double x) => !double.IsInfinity(x) && !double.IsNaN(x);

  public static double DoubleFromMantissaAndExponent(double x, int exp)
  {
    return x * Math.Pow(2.0, (double) exp);
  }

  public static bool GreaterThan(double value1, double value2)
  {
    return value1 > value2 && !MathHelper.AreClose(value1, value2);
  }

  public static bool GreaterThanOrClose(double value1, double value2)
  {
    return value1 > value2 || MathHelper.AreClose(value1, value2);
  }

  public static double Hypotenuse(double x, double y) => Math.Sqrt(x * x + y * y);

  public static bool LessThan(double value1, double value2)
  {
    return value1 < value2 && !MathHelper.AreClose(value1, value2);
  }

  public static bool LessThanOrClose(double value1, double value2)
  {
    return value1 < value2 || MathHelper.AreClose(value1, value2);
  }

  public static double EnsureRange(double value, double? min, double? max)
  {
    if (min.HasValue && value < min.Value)
      return min.Value;
    return max.HasValue && value > max.Value ? max.Value : value;
  }

  public static double SafeDivide(double lhs, double rhs, double fallback)
  {
    return !MathHelper.IsVerySmall(rhs) ? lhs / rhs : fallback;
  }
}
