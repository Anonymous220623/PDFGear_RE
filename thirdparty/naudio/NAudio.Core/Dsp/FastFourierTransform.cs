// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.FastFourierTransform
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Dsp;

public static class FastFourierTransform
{
  public static void FFT(bool forward, int m, Complex[] data)
  {
    int num1 = 1;
    for (int index = 0; index < m; ++index)
      num1 *= 2;
    int num2 = num1 >> 1;
    int index1 = 0;
    for (int index2 = 0; index2 < num1 - 1; ++index2)
    {
      if (index2 < index1)
      {
        float x = data[index2].X;
        float y = data[index2].Y;
        data[index2].X = data[index1].X;
        data[index2].Y = data[index1].Y;
        data[index1].X = x;
        data[index1].Y = y;
      }
      int num3;
      for (num3 = num2; num3 <= index1; num3 >>= 1)
        index1 -= num3;
      index1 += num3;
    }
    float num4 = -1f;
    float num5 = 0.0f;
    int num6 = 1;
    for (int index3 = 0; index3 < m; ++index3)
    {
      int num7 = num6;
      num6 <<= 1;
      float num8 = 1f;
      float num9 = 0.0f;
      for (int index4 = 0; index4 < num7; ++index4)
      {
        for (int index5 = index4; index5 < num1; index5 += num6)
        {
          int index6 = index5 + num7;
          float num10 = (float) ((double) num8 * (double) data[index6].X - (double) num9 * (double) data[index6].Y);
          float num11 = (float) ((double) num8 * (double) data[index6].Y + (double) num9 * (double) data[index6].X);
          data[index6].X = data[index5].X - num10;
          data[index6].Y = data[index5].Y - num11;
          data[index5].X += num10;
          data[index5].Y += num11;
        }
        float num12 = (float) ((double) num8 * (double) num4 - (double) num9 * (double) num5);
        num9 = (float) ((double) num8 * (double) num5 + (double) num9 * (double) num4);
        num8 = num12;
      }
      num5 = (float) Math.Sqrt((1.0 - (double) num4) / 2.0);
      if (forward)
        num5 = -num5;
      num4 = (float) Math.Sqrt((1.0 + (double) num4) / 2.0);
    }
    if (!forward)
      return;
    for (int index7 = 0; index7 < num1; ++index7)
    {
      data[index7].X /= (float) num1;
      data[index7].Y /= (float) num1;
    }
  }

  public static double HammingWindow(int n, int frameSize)
  {
    return 0.54 - 0.46 * Math.Cos(2.0 * Math.PI * (double) n / (double) (frameSize - 1));
  }

  public static double HannWindow(int n, int frameSize)
  {
    return 0.5 * (1.0 - Math.Cos(2.0 * Math.PI * (double) n / (double) (frameSize - 1)));
  }

  public static double BlackmannHarrisWindow(int n, int frameSize)
  {
    return 287.0 / 800.0 - 0.48829 * Math.Cos(2.0 * Math.PI * (double) n / (double) (frameSize - 1)) + 0.14128 * Math.Cos(4.0 * Math.PI * (double) n / (double) (frameSize - 1)) - 0.01168 * Math.Cos(6.0 * Math.PI * (double) n / (double) (frameSize - 1));
  }
}
