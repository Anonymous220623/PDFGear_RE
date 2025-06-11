// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.MathUtil
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class MathUtil
{
  public static int log2(int x)
  {
    int num1 = x > 0 ? x : throw new ArgumentException(x.ToString() + " <= 0");
    int num2 = -1;
    while (num1 > 0)
    {
      num1 >>= 1;
      ++num2;
    }
    return num2;
  }

  public static int lcm(int x1, int x2)
  {
    if (x1 <= 0 || x2 <= 0)
      throw new ArgumentException("Cannot compute the least common multiple of two numbers if one, at least,is negative.");
    int num1;
    int num2;
    if (x1 > x2)
    {
      num1 = x1;
      num2 = x2;
    }
    else
    {
      num1 = x2;
      num2 = x1;
    }
    for (int index = 1; index <= num2; ++index)
    {
      if (num1 * index % num2 == 0)
        return index * num1;
    }
    return -1;
  }

  public static int lcm(int[] x)
  {
    int length = x.Length;
    int x1 = MathUtil.lcm(x[x.Length - 1], x[x.Length - 2]);
    for (int index = x.Length - 3; index >= 0; --index)
    {
      if (x[index] <= 0)
        throw new ArgumentException("Cannot compute the least common multiple of several numbers where one, at least,is negative.");
      x1 = MathUtil.lcm(x1, x[index]);
    }
    return x1;
  }

  public static int gcd(int x1, int x2)
  {
    if (x1 < 0 || x2 < 0)
      throw new ArgumentException("Cannot compute the GCD if one integer is negative.");
    int num1;
    int num2;
    if (x1 > x2)
    {
      num1 = x1;
      num2 = x2;
    }
    else
    {
      num1 = x2;
      num2 = x1;
    }
    if (num2 == 0)
      return 0;
    int num3;
    for (int index = num2; index != 0; index = num3)
    {
      num3 = num1 % index;
      num1 = index;
    }
    return num1;
  }

  public static int gcd(int[] x)
  {
    int length = x.Length;
    int x1 = MathUtil.gcd(x[x.Length - 1], x[x.Length - 2]);
    for (int index = x.Length - 3; index >= 0; --index)
    {
      if (x[index] < 0)
        throw new ArgumentException("Cannot compute the least common multiple of several numbers where one, at least,is negative.");
      x1 = MathUtil.gcd(x1, x[index]);
    }
    return x1;
  }
}
