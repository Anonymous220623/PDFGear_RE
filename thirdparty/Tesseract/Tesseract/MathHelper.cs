// Decompiled with JetBrains decompiler
// Type: Tesseract.MathHelper
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;

#nullable disable
namespace Tesseract;

public static class MathHelper
{
  public static float ToRadians(float angleInDegrees)
  {
    return (float) MathHelper.ToRadians((double) angleInDegrees);
  }

  public static double ToRadians(double angleInDegrees) => angleInDegrees * Math.PI / 180.0;

  public static int DivRoundUp(int dividend, int divisor)
  {
    int num = dividend / divisor;
    return dividend % divisor == 0 || divisor > 0 != dividend > 0 ? num : num + 1;
  }
}
