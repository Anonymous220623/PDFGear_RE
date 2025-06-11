// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.DoubleUtil
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

internal static class DoubleUtil
{
  internal const double DBL_EPSILON = 2.2204460492503131E-16;
  internal const float FLT_MIN = 1.17549435E-38f;

  public static bool AreClose(double value1, double value2)
  {
    if (value1 == value2)
      return true;
    double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
    double num2 = value1 - value2;
    return -num1 < num2 && num1 > num2;
  }

  public static bool LessThan(double value1, double value2)
  {
    return value1 < value2 && !DoubleUtil.AreClose(value1, value2);
  }

  public static bool GreaterThan(double value1, double value2)
  {
    return value1 > value2 && !DoubleUtil.AreClose(value1, value2);
  }

  public static bool LessThanOrClose(double value1, double value2)
  {
    return value1 < value2 || DoubleUtil.AreClose(value1, value2);
  }

  public static bool GreaterThanOrClose(double value1, double value2)
  {
    return value1 > value2 || DoubleUtil.AreClose(value1, value2);
  }

  public static bool IsOne(double value) => Math.Abs(value - 1.0) < 2.2204460492503131E-15;

  public static bool IsZero(double value) => Math.Abs(value) < 2.2204460492503131E-15;

  public static bool AreClose(Vector vector1, Vector vector2)
  {
    return DoubleUtil.AreClose(vector1.X, vector2.X) && DoubleUtil.AreClose(vector1.Y, vector2.Y);
  }

  public static bool IsBetweenZeroAndOne(double val)
  {
    return DoubleUtil.GreaterThanOrClose(val, 0.0) && DoubleUtil.LessThanOrClose(val, 1.0);
  }

  public static int DoubleToInt(double val) => 0.0 < val ? (int) (val + 0.5) : (int) (val - 0.5);
}
