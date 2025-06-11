// Decompiled with JetBrains decompiler
// Type: SoundTouch.Net.NAudioSupport.DoubleUtil
// Assembly: SoundTouch.Net.NAudioSupport, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 99206FE3-DB71-4C89-91A8-76F439C9BC37
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.NAudioSupport.dll

using System;

#nullable disable
namespace SoundTouch.Net.NAudioSupport;

internal static class DoubleUtil
{
  internal const double DBL_EPSILON = 1.1102230246251568E-16;

  public static bool AreClose(double value1, double value2)
  {
    if (value1 == value2)
      return true;
    double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 1.1102230246251568E-16;
    double num2 = value1 - value2;
    return -num1 < num2 && num1 > num2;
  }
}
