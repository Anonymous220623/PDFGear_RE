// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.DoubleUtil
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Runtime.InteropServices;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

public static class DoubleUtil
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

  public static int DoubleToInt(double val) => 0.0 >= val ? (int) (val - 0.5) : (int) (val + 0.5);

  public static bool IsNaN(double value)
  {
    DoubleUtil.NanUnion nanUnion = new DoubleUtil.NanUnion();
    nanUnion.DoubleValue = value;
    ulong num1 = nanUnion.UintValue & 18442240474082181120UL /*0xFFF0000000000000*/;
    ulong num2 = nanUnion.UintValue & 4503599627370495UL /*0x0FFFFFFFFFFFFF*/;
    return (num1 == 9218868437227405312UL /*0x7FF0000000000000*/ || num1 == 18442240474082181120UL /*0xFFF0000000000000*/) && num2 > 0UL;
  }

  [StructLayout(LayoutKind.Explicit)]
  private struct NanUnion
  {
    [FieldOffset(0)]
    internal double DoubleValue;
    [FieldOffset(0)]
    internal ulong UintValue;
  }
}
