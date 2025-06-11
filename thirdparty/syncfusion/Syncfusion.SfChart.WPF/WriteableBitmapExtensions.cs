// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.WriteableBitmapExtensions
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class WriteableBitmapExtensions
{
  public static void BeginWrite(this WriteableBitmap bmp) => bmp.Lock();

  public static unsafe int* GetPixels(this WriteableBitmap bmp) => (int*) (void*) bmp.BackBuffer;

  public static void EndWrite(this WriteableBitmap bmp)
  {
    bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
    bmp.Unlock();
  }

  public static int GetLength(this WriteableBitmap bmp)
  {
    return (int) ((double) (bmp.BackBufferStride / 4) * (double) bmp.PixelHeight);
  }

  [TargetedPatchingOptOut("Candidate for inlining across NGen boundaries for performance reasons")]
  public static void Clear(this WriteableBitmap bmp)
  {
    NativeMethods.SetUnmanagedMemory(bmp.BackBuffer, 0, bmp.BackBufferStride * bmp.PixelHeight);
  }

  private static int ConvertColor(Color color)
  {
    int num = (int) color.A + 1;
    return (int) color.A << 24 | (int) (byte) ((int) color.R * num >> 8) << 16 /*0x10*/ | (int) (byte) ((int) color.G * num >> 8) << 8 | (int) (byte) ((int) color.B * num >> 8);
  }

  public static void DrawLineBresenham(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color,
    List<int> pixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawLineBresenham(x1, y1, x2, y2, color1, pixels);
  }

  public static unsafe void DrawLineBresenham(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color col,
    List<int> seriesPixels,
    Rect clip)
  {
    int num1 = WriteableBitmapExtensions.ConvertColor(col);
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int top = (int) clip.Top;
    int bottom = (int) clip.Bottom;
    int left = (int) clip.Left;
    int right = (int) clip.Right;
    int* pixels = bmp.GetPixels();
    if (x1 == 0 && x2 == 0 || x1 == x2 && y1 == y2 || y1 < 0 && y2 < 0 || y1 > pixelHeight && y2 > pixelHeight || x1 < 0 && x2 < 0 || x1 > pixelWidth && x2 > pixelWidth)
      return;
    double num2 = (double) y1;
    double num3 = (double) y2;
    double num4 = (double) x1;
    double num5 = (double) x2;
    if ((double) y2 < -0.5)
    {
      double num6 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num6))
        x2 = (int) (-WriteableBitmapExtensions.Intersect(num5, num3, num6) / num6);
      y2 = 0;
    }
    if ((double) y1 < -0.5)
    {
      double num7 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num7))
        x1 = (int) (-WriteableBitmapExtensions.Intersect(num4, num2, num7) / num7);
      y1 = 0;
    }
    if ((double) x1 < -0.5)
    {
      double num8 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num8))
        y1 = (int) WriteableBitmapExtensions.Intersect(num4, num2, num8);
      x1 = 0;
    }
    if ((double) x2 < -0.5)
    {
      double num9 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num9))
        y2 = (int) WriteableBitmapExtensions.Intersect(num5, num3, num9);
      x2 = 0;
    }
    if ((double) x1 > (double) pixelWidth + 0.5)
    {
      double num10 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num10))
      {
        double num11 = WriteableBitmapExtensions.Intersect(num4, num2, num10);
        y1 = (int) (num10 * (double) pixelWidth + num11);
      }
      x1 = pixelWidth;
    }
    if ((double) x2 > (double) pixelWidth + 0.5)
    {
      double num12 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num12))
      {
        double num13 = WriteableBitmapExtensions.Intersect(num5, num3, num12);
        y2 = (int) (num12 * (double) pixelWidth + num13);
      }
      x2 = pixelWidth;
    }
    if ((double) y1 > (double) pixelHeight + 0.5)
    {
      double num14 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num14))
      {
        double num15 = WriteableBitmapExtensions.Intersect(num4, num2, num14);
        x1 = (int) (((double) pixelHeight - num15) / num14);
      }
      y1 = pixelHeight;
    }
    if ((double) y2 > (double) pixelHeight + 0.5)
    {
      double num16 = WriteableBitmapExtensions.Slope(num4, num2, num5, num3);
      if (!double.IsInfinity(num16))
      {
        double num17 = WriteableBitmapExtensions.Intersect(num5, num3, num16);
        x2 = (int) (((double) pixelHeight - num17) / num16);
      }
      y2 = pixelHeight;
    }
    int num18 = x2 - x1;
    int num19 = y2 - y1;
    int num20 = 0;
    if (num18 < 0)
    {
      num18 = -num18;
      num20 = -1;
    }
    else if (num18 > 0)
      num20 = 1;
    int num21 = 0;
    if (num19 < 0)
    {
      num19 = -num19;
      num21 = -1;
    }
    else if (num19 > 0)
      num21 = 1;
    int num22;
    int num23;
    int num24;
    int num25;
    int num26;
    int num27;
    if (num18 > num19)
    {
      num22 = num20;
      num23 = 0;
      num24 = num20;
      num25 = num21;
      num26 = num19;
      num27 = num18;
    }
    else
    {
      num22 = 0;
      num23 = num21;
      num24 = num20;
      num25 = num21;
      num26 = num18;
      num27 = num19;
    }
    int num28 = x1;
    int num29 = y1;
    int num30 = num27 >> 1;
    if (num29 < pixelHeight && num29 >= 0 && num28 < pixelWidth && num28 >= 0 && num29 < bottom && num29 >= top && num28 < right && num28 >= left)
    {
      int index = num29 * pixelWidth + num28;
      if (pixels[index] != num1)
      {
        pixels[index] = num1;
        seriesPixels.Add(index);
      }
    }
    for (int index1 = 0; index1 < num27; ++index1)
    {
      num30 -= num26;
      if (num30 < 0)
      {
        num30 += num27;
        num28 += num24;
        num29 += num25;
      }
      else
      {
        num28 += num22;
        num29 += num23;
      }
      if (num29 < pixelHeight && num29 >= 0 && num28 < pixelWidth && num28 >= 0 && num29 < bottom && num29 > top && num28 < right && num28 >= left)
      {
        int index2 = num29 * pixelWidth + num28;
        if (pixels[index2] != num1)
        {
          pixels[index2] = num1;
          seriesPixels.Add(index2);
        }
      }
    }
  }

  public static unsafe void DrawLineBresenham(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int* pixels = bmp.GetPixels();
    if (x1 == 0 && x2 == 0 || x1 == x2 && y1 == y2 || y1 < 0 && y2 < 0 || y1 > pixelHeight && y2 > pixelHeight || x1 < 0 && x2 < 0 || x1 > pixelWidth && x2 > pixelWidth)
      return;
    double num1 = (double) y1;
    double num2 = (double) y2;
    double num3 = (double) x1;
    double num4 = (double) x2;
    if ((double) y2 < -0.5)
    {
      double num5 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num5))
        x2 = (int) (-WriteableBitmapExtensions.Intersect(num4, num2, num5) / num5);
      y2 = 0;
    }
    if ((double) y1 < -0.5)
    {
      double num6 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num6))
        x1 = (int) (-WriteableBitmapExtensions.Intersect(num3, num1, num6) / num6);
      y1 = 0;
    }
    if ((double) x1 < -0.5)
    {
      double num7 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num7))
        y1 = (int) WriteableBitmapExtensions.Intersect(num3, num1, num7);
      x1 = 0;
    }
    if ((double) x2 < -0.5)
    {
      double num8 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num8))
        y2 = (int) WriteableBitmapExtensions.Intersect(num4, num2, num8);
      x2 = 0;
    }
    if ((double) x1 > (double) pixelWidth + 0.5)
    {
      double num9 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num9))
      {
        double num10 = WriteableBitmapExtensions.Intersect(num3, num1, num9);
        y1 = (int) (num9 * (double) pixelWidth + num10);
      }
      x1 = pixelWidth;
    }
    if ((double) x2 > (double) pixelWidth + 0.5)
    {
      double num11 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num11))
      {
        double num12 = WriteableBitmapExtensions.Intersect(num4, num2, num11);
        y2 = (int) (num11 * (double) pixelWidth + num12);
      }
      x2 = pixelWidth;
    }
    if ((double) y1 > (double) pixelHeight + 0.5)
    {
      double num13 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num13))
      {
        double num14 = WriteableBitmapExtensions.Intersect(num3, num1, num13);
        x1 = (int) (((double) pixelHeight - num14) / num13);
      }
      y1 = pixelHeight;
    }
    if ((double) y2 > (double) pixelHeight + 0.5)
    {
      double num15 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num15))
      {
        double num16 = WriteableBitmapExtensions.Intersect(num4, num2, num15);
        x2 = (int) (((double) pixelHeight - num16) / num15);
      }
      y2 = pixelHeight;
    }
    int num17 = x2 - x1;
    int num18 = y2 - y1;
    int num19 = 0;
    if (num17 < 0)
    {
      num17 = -num17;
      num19 = -1;
    }
    else if (num17 > 0)
      num19 = 1;
    int num20 = 0;
    if (num18 < 0)
    {
      num18 = -num18;
      num20 = -1;
    }
    else if (num18 > 0)
      num20 = 1;
    int num21;
    int num22;
    int num23;
    int num24;
    int num25;
    int num26;
    if (num17 > num18)
    {
      num21 = num19;
      num22 = 0;
      num23 = num19;
      num24 = num20;
      num25 = num18;
      num26 = num17;
    }
    else
    {
      num21 = 0;
      num22 = num20;
      num23 = num19;
      num24 = num20;
      num25 = num17;
      num26 = num18;
    }
    int num27 = x1;
    int num28 = y1;
    int num29 = num26 >> 1;
    if (num28 < pixelHeight && num28 >= 0 && num27 < pixelWidth && num27 >= 0)
    {
      int index = num28 * pixelWidth + num27;
      if (pixels[index] != color)
      {
        pixels[index] = color;
        seriesPixels.Add(index);
      }
    }
    for (int index1 = 0; index1 < num26; ++index1)
    {
      num29 -= num25;
      if (num29 < 0)
      {
        num29 += num26;
        num27 += num23;
        num28 += num24;
      }
      else
      {
        num27 += num21;
        num28 += num22;
      }
      if (num28 < pixelHeight && num28 >= 0 && num27 < pixelWidth && num27 >= 0)
      {
        int index2 = num28 * pixelWidth + num27;
        if (pixels[index2] != color)
        {
          pixels[index2] = color;
          seriesPixels.Add(index2);
        }
      }
    }
  }

  public static void DrawLineDDA(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawLineDDA(x1, y1, x2, y2, color1);
  }

  public static unsafe void DrawLineDDA(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int* pixels = bmp.GetPixels();
    int num1 = x2 - x1;
    int num2 = y2 - y1;
    int num3 = num2 >= 0 ? num2 : -num2;
    int num4 = num1 >= 0 ? num1 : -num1;
    if (num4 > num3)
      num3 = num4;
    if (num3 == 0)
      return;
    float num5 = (float) num1 / (float) num3;
    float num6 = (float) num2 / (float) num3;
    float num7 = (float) x1;
    float num8 = (float) y1;
    for (int index = 0; index < num3; ++index)
    {
      if ((double) num8 < (double) pixelHeight && (double) num8 >= 0.0 && (double) num7 < (double) pixelWidth && (double) num7 >= 0.0)
        pixels[(int) num8 * pixelWidth + (int) num7] = color;
      num7 += num5;
      num8 += num6;
    }
  }

  public static void DrawLine(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawLine(x1, y1, x2, y2, color1);
  }

  public static void DrawLine(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color)
  {
    WriteableBitmapExtensions.DrawLine(bmp, bmp.PixelWidth, bmp.PixelHeight, x1, y1, x2, y2, color);
  }

  public static unsafe void DrawLine(
    WriteableBitmap bmp,
    int pixelWidth,
    int pixelHeight,
    int x1,
    int y1,
    int x2,
    int y2,
    int color)
  {
    int* pixels = bmp.GetPixels();
    int num1 = x2 - x1;
    int num2 = y2 - y1;
    int num3 = num2 < 0 ? -num2 : num2;
    if ((num1 < 0 ? -num1 : num1) > num3)
    {
      if (num1 < 0)
      {
        int num4 = x1;
        x1 = x2;
        x2 = num4;
        int num5 = y1;
        y1 = y2;
        y2 = num5;
      }
      int num6 = (num2 << 8) / num1;
      int num7 = y1 << 8;
      int num8 = y2 << 8;
      int num9 = pixelHeight << 8;
      if (y1 < y2)
      {
        if (y1 >= pixelHeight || y2 < 0)
          return;
        if (num7 < 0)
        {
          if (num6 == 0)
            return;
          int num10 = num7;
          num7 = num6 - 1 + (num7 + 1) % num6;
          x1 += (num7 - num10) / num6;
        }
        if (num8 >= num9 && num6 != 0)
        {
          int num11 = num9 - 1 - (num9 - 1 - num7) % num6;
          x2 = x1 + (num11 - num7) / num6;
        }
      }
      else
      {
        if (y2 >= pixelHeight || y1 < 0)
          return;
        if (num7 >= num9)
        {
          if (num6 == 0)
            return;
          int num12 = num7;
          num7 = num9 - 1 + (num6 - (num9 - 1 - num12) % num6);
          x1 += (num7 - num12) / num6;
        }
        if (num8 < 0 && num6 != 0)
        {
          int num13 = num7 % num6;
          x2 = x1 + (num13 - num7) / num6;
        }
      }
      if (x1 < 0)
      {
        num7 -= num6 * x1;
        x1 = 0;
      }
      if (x2 >= pixelWidth)
        x2 = pixelWidth - 1;
      int num14 = num7;
      int num15 = num14 >> 8;
      int num16 = num15;
      int index1 = x1 + num15 * pixelWidth;
      int num17 = num6 < 0 ? 1 - pixelWidth : 1 + pixelWidth;
      for (int index2 = x1; index2 <= x2; ++index2)
      {
        pixels[index1] = color;
        num14 += num6;
        int num18 = num14 >> 8;
        if (num18 != num16)
        {
          num16 = num18;
          index1 += num17;
        }
        else
          ++index1;
      }
    }
    else
    {
      if (num3 == 0)
        return;
      if (num2 < 0)
      {
        int num19 = x1;
        x1 = x2;
        x2 = num19;
        int num20 = y1;
        y1 = y2;
        y2 = num20;
      }
      int num21 = x1 << 8;
      int num22 = x2 << 8;
      int num23 = pixelWidth << 8;
      int num24 = (num1 << 8) / num2;
      if (x1 < x2)
      {
        if (x1 >= pixelWidth || x2 < 0)
          return;
        if (num21 < 0)
        {
          if (num24 == 0)
            return;
          int num25 = num21;
          num21 = num24 - 1 + (num21 + 1) % num24;
          y1 += (num21 - num25) / num24;
        }
        if (num22 >= num23 && num24 != 0)
        {
          int num26 = num23 - 1 - (num23 - 1 - num21) % num24;
          y2 = y1 + (num26 - num21) / num24;
        }
      }
      else
      {
        if (x2 >= pixelWidth || x1 < 0)
          return;
        if (num21 >= num23)
        {
          if (num24 == 0)
            return;
          int num27 = num21;
          num21 = num23 - 1 + (num24 - (num23 - 1 - num27) % num24);
          y1 += (num21 - num27) / num24;
        }
        if (num22 < 0 && num24 != 0)
        {
          int num28 = num21 % num24;
          y2 = y1 + (num28 - num21) / num24;
        }
      }
      if (y1 < 0)
      {
        num21 -= num24 * y1;
        y1 = 0;
      }
      if (y2 >= pixelHeight)
        y2 = pixelHeight - 1;
      int num29 = num21 + (y1 * pixelWidth << 8);
      int num30 = (pixelWidth << 8) + num24;
      for (int index = y1; index <= y2; ++index)
      {
        pixels[num29 >> 8] = color;
        num29 += num30;
      }
    }
  }

  public static void DrawLineAa(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color,
    List<int> seriesPixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawLineAa(x1, y1, x2, y2, color1, seriesPixels);
  }

  public static void DrawLineAa(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels)
  {
    WriteableBitmapExtensions.DrawLineAa(bmp, bmp.PixelWidth, bmp.PixelHeight, x1, y1, x2, y2, color, seriesPixels);
  }

  public static void DrawLineAa(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color col,
    List<int> seriesPixels,
    Rect clip)
  {
    int color = WriteableBitmapExtensions.ConvertColor(col);
    WriteableBitmapExtensions.DrawLineAa(bmp, bmp.PixelWidth, bmp.PixelHeight, x1, y1, x2, y2, color, seriesPixels, clip);
  }

  public static void DrawLineAa(
    WriteableBitmap bmp,
    int pixelWidth,
    int pixelHeight,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels,
    Rect clip)
  {
    int top = (int) clip.Top;
    int bottom = (int) clip.Bottom;
    int left = (int) clip.Left;
    int right = (int) clip.Right;
    if (x1 == 0 && x2 == 0 || x1 == x2 && y1 == y2 || y1 < 0 && y2 < 0 || y1 > pixelHeight && y2 > pixelHeight || x1 < 0 && x2 < 0 || x1 > pixelWidth && x2 > pixelWidth)
      return;
    double num1 = (double) y1;
    double num2 = (double) y2;
    double num3 = (double) x1;
    double num4 = (double) x2;
    if ((double) y2 < -0.5)
    {
      double num5 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num5))
        x2 = (int) (-WriteableBitmapExtensions.Intersect(num4, num2, num5) / num5);
      y2 = 0;
    }
    if ((double) y1 < -0.5)
    {
      double num6 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num6))
        x1 = (int) (-WriteableBitmapExtensions.Intersect(num3, num1, num6) / num6);
      y1 = 0;
    }
    if ((double) x1 < -0.5)
    {
      double num7 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num7))
        y1 = (int) WriteableBitmapExtensions.Intersect(num3, num1, num7);
      x1 = 0;
    }
    if ((double) x2 < -0.5)
    {
      double num8 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num8))
        y2 = (int) WriteableBitmapExtensions.Intersect(num4, num2, num8);
      x2 = 0;
    }
    if ((double) x1 > (double) pixelWidth + 0.5)
    {
      double num9 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num9))
      {
        double num10 = WriteableBitmapExtensions.Intersect(num3, num1, num9);
        y1 = (int) (num9 * (double) pixelWidth + num10);
      }
      x1 = pixelWidth;
    }
    if ((double) x2 > (double) pixelWidth + 0.5)
    {
      double num11 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num11))
      {
        double num12 = WriteableBitmapExtensions.Intersect(num4, num2, num11);
        y2 = (int) (num11 * (double) pixelWidth + num12);
      }
      x2 = pixelWidth;
    }
    if ((double) y1 > (double) pixelHeight + 0.5)
    {
      double num13 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num13))
      {
        double num14 = WriteableBitmapExtensions.Intersect(num3, num1, num13);
        x1 = (int) (((double) pixelHeight - num14) / num13);
      }
      y1 = pixelHeight;
    }
    if ((double) y2 > (double) pixelHeight + 0.5)
    {
      double num15 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num15))
      {
        double num16 = WriteableBitmapExtensions.Intersect(num4, num2, num15);
        x2 = (int) (((double) pixelHeight - num16) / num15);
      }
      y2 = pixelHeight;
    }
    if (y1 > bottom)
      y1 = bottom;
    if (y2 < top)
      y2 = top;
    if (y1 < top)
      y1 = top;
    if (y2 > bottom)
      y2 = bottom;
    if (x1 > right)
      x1 = right;
    if (x1 < left)
      x1 = left;
    if (x2 < left)
      x2 = left;
    if (x2 > right)
      x2 = right;
    int num17 = y1 * pixelWidth + x1;
    int index1 = num17 >= 0 ? num17 : 1;
    int num18 = x2 - x1;
    int num19 = y2 - y1;
    int num20 = color >> 24 & (int) byte.MaxValue;
    uint srb = (uint) (color & 16711935);
    uint sg = (uint) (color >> 8 & (int) byte.MaxValue);
    int num21 = num18;
    int num22 = num19;
    if (num18 < 0)
      num21 = -num18;
    if (num19 < 0)
      num22 = -num19;
    int num23;
    int num24;
    int num25;
    int num26;
    int num27;
    int num28;
    if (num21 > num22)
    {
      num23 = num21;
      num24 = num22;
      num25 = x2;
      num26 = y2;
      num27 = 1;
      num28 = pixelWidth;
      if (num18 < 0)
        num27 = -num27;
      if (num19 < 0)
        num28 = -num28;
    }
    else
    {
      num23 = num22;
      num24 = num21;
      num25 = y2;
      num26 = x2;
      num27 = pixelWidth;
      num28 = 1;
      if (num19 < 0)
        num27 = -num27;
      if (num18 < 0)
        num28 = -num28;
    }
    int num29 = num25 + num23;
    int num30 = (num24 << 1) - num23;
    int num31 = num24 << 1;
    int num32 = num24 - num23 << 1;
    double num33 = 1.0 / (4.0 * Math.Sqrt((double) (num23 * num23 + num24 * num24)));
    double num34 = 0.75 - 2.0 * ((double) num23 * num33);
    int num35 = (int) (num33 * 1024.0);
    int num36 = (int) (num34 * 1024.0 * (double) num20);
    int num37 = (int) (768.0 * (double) num20);
    int num38 = num35 * num20;
    int num39 = num23 * num38;
    int num40 = num30 * num38;
    int num41 = 0;
    int num42 = num31 * num38;
    int num43 = num32 * num38;
    do
    {
      int index2 = index1 + num28 >= 0 ? index1 + num28 : 0;
      int index3 = index1 - num28 >= 0 ? index1 - num28 : 0;
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index1, num37 - num41 >> 10, srb, sg, seriesPixels);
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index2, num36 + num41 >> 10, srb, sg, seriesPixels);
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index3, num36 - num41 >> 10, srb, sg, seriesPixels);
      if (num30 < 0)
      {
        num41 = num40 + num39;
        num30 += num31;
        num40 += num42;
      }
      else
      {
        num41 = num40 - num39;
        num30 += num32;
        num40 += num43;
        ++num26;
        index1 += num28;
      }
      ++num25;
      index1 += num27;
    }
    while (num25 < num29);
  }

  public static void DrawLineAa(
    WriteableBitmap bmp,
    int pixelWidth,
    int pixelHeight,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels)
  {
    if (x1 == 0 && x2 == 0 || x1 == x2 && y1 == y2 || y1 < 0 && y2 < 0 || y1 > pixelHeight && y2 > pixelHeight || x1 < 0 && x2 < 0 || x1 > pixelWidth && x2 > pixelWidth)
      return;
    double num1 = (double) y1;
    double num2 = (double) y2;
    double num3 = (double) x1;
    double num4 = (double) x2;
    if ((double) y2 < -0.5)
    {
      double num5 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num5))
        x2 = (int) (-WriteableBitmapExtensions.Intersect(num4, num2, num5) / num5);
      y2 = 0;
    }
    if ((double) y1 < -0.5)
    {
      double num6 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num6))
        x1 = (int) (-WriteableBitmapExtensions.Intersect(num3, num1, num6) / num6);
      y1 = 0;
    }
    if ((double) x1 < -0.5)
    {
      double num7 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num7))
        y1 = (int) WriteableBitmapExtensions.Intersect(num3, num1, num7);
      x1 = 0;
    }
    if ((double) x2 < -0.5)
    {
      double num8 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num8))
        y2 = (int) WriteableBitmapExtensions.Intersect(num4, num2, num8);
      x2 = 0;
    }
    if ((double) x1 > (double) pixelWidth + 0.5)
    {
      double num9 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num9))
      {
        double num10 = WriteableBitmapExtensions.Intersect(num3, num1, num9);
        y1 = (int) (num9 * (double) pixelWidth + num10);
      }
      x1 = pixelWidth;
    }
    if ((double) x2 > (double) pixelWidth + 0.5)
    {
      double num11 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num11))
      {
        double num12 = WriteableBitmapExtensions.Intersect(num4, num2, num11);
        y2 = (int) (num11 * (double) pixelWidth + num12);
      }
      x2 = pixelWidth;
    }
    if ((double) y1 > (double) pixelHeight + 0.5)
    {
      double num13 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num13))
      {
        double num14 = WriteableBitmapExtensions.Intersect(num3, num1, num13);
        x1 = (int) (((double) pixelHeight - num14) / num13);
      }
      y1 = pixelHeight;
    }
    if ((double) y2 > (double) pixelHeight + 0.5)
    {
      double num15 = WriteableBitmapExtensions.Slope(num3, num1, num4, num2);
      if (!double.IsInfinity(num15))
      {
        double num16 = WriteableBitmapExtensions.Intersect(num4, num2, num15);
        x2 = (int) (((double) pixelHeight - num16) / num15);
      }
      y2 = pixelHeight;
    }
    int num17 = y1 * pixelWidth + x1;
    int index1 = num17 >= 0 ? num17 : 1;
    int num18 = x2 - x1;
    int num19 = y2 - y1;
    int num20 = color >> 24 & (int) byte.MaxValue;
    uint srb = (uint) (color & 16711935);
    uint sg = (uint) (color >> 8 & (int) byte.MaxValue);
    int num21 = num18;
    int num22 = num19;
    if (num18 < 0)
      num21 = -num18;
    if (num19 < 0)
      num22 = -num19;
    int num23;
    int num24;
    int num25;
    int num26;
    int num27;
    int num28;
    if (num21 > num22)
    {
      num23 = num21;
      num24 = num22;
      num25 = x2;
      num26 = y2;
      num27 = 1;
      num28 = pixelWidth;
      if (num18 < 0)
        num27 = -num27;
      if (num19 < 0)
        num28 = -num28;
    }
    else
    {
      num23 = num22;
      num24 = num21;
      num25 = y2;
      num26 = x2;
      num27 = pixelWidth;
      num28 = 1;
      if (num19 < 0)
        num27 = -num27;
      if (num18 < 0)
        num28 = -num28;
    }
    int num29 = num25 + num23;
    int num30 = (num24 << 1) - num23;
    int num31 = num24 << 1;
    int num32 = num24 - num23 << 1;
    double num33 = 1.0 / (4.0 * Math.Sqrt((double) (num23 * num23 + num24 * num24)));
    double num34 = 0.75 - 2.0 * ((double) num23 * num33);
    int num35 = (int) (num33 * 1024.0);
    int num36 = (int) (num34 * 1024.0 * (double) num20);
    int num37 = (int) (768.0 * (double) num20);
    int num38 = num35 * num20;
    int num39 = num23 * num38;
    int num40 = num30 * num38;
    int num41 = 0;
    int num42 = num31 * num38;
    int num43 = num32 * num38;
    do
    {
      int index2 = index1 + num28 >= 0 ? index1 + num28 : 0;
      int index3 = index1 - num28 >= 0 ? index1 - num28 : 0;
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index1, num37 - num41 >> 10, srb, sg, seriesPixels);
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index2, num36 + num41 >> 10, srb, sg, seriesPixels);
      WriteableBitmapExtensions.AlphaBlendNormalOnPremultiplied(bmp, index3, num36 - num41 >> 10, srb, sg, seriesPixels);
      if (num30 < 0)
      {
        num41 = num40 + num39;
        num30 += num31;
        num40 += num42;
      }
      else
      {
        num41 = num40 - num39;
        num30 += num32;
        num40 += num43;
        ++num26;
        index1 += num28;
      }
      ++num25;
      index1 += num27;
    }
    while (num25 < num29);
  }

  public static double Slope(double x1, double y1, double x2, double y2) => (y2 - y1) / (x2 - x1);

  public static double Intersect(double x, double y, double slope) => y - slope * x;

  [HandleProcessCorruptedStateExceptions]
  [SecurityCritical]
  private static unsafe void AlphaBlendNormalOnPremultiplied(
    WriteableBitmap bmp,
    int index,
    int sa,
    uint srb,
    uint sg,
    List<int> seriesPixels)
  {
    try
    {
      if (bmp.GetLength() < index || index < 0)
        return;
      int* pixels = bmp.GetPixels();
      uint num1 = (uint) pixels[index];
      uint num2 = num1 >> 24;
      uint num3 = num1 >> 8 & (uint) byte.MaxValue;
      uint num4 = num1 & 16711935U;
      int num5 = (int) ((long) sa + ((long) num2 * (long) ((int) byte.MaxValue - sa) * 32897L >> 23) << 24 | (long) (sg - num3) * (long) sa + (long) (num3 << 8) & 4294967040L | ((long) (srb - num4) * (long) sa >> 8) + (long) num4 & 16711935L);
      if (pixels[index] == num5)
        return;
      pixels[index] = num5;
      seriesPixels.Add(index);
    }
    catch (Exception ex)
    {
    }
  }

  public static void DrawPolyline(this WriteableBitmap bmp, int[] points, Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawPolyline(points, color1);
  }

  public static void DrawPolyline(this WriteableBitmap bmp, int[] points, int color)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int x1 = points[0];
    int y1 = points[1];
    for (int index = 2; index < points.Length; index += 2)
    {
      int point1 = points[index];
      int point2 = points[index + 1];
      WriteableBitmapExtensions.DrawLine(bmp, pixelWidth, pixelHeight, x1, y1, point1, point2, color);
      x1 = point1;
      y1 = point2;
    }
  }

  public static void DrawTriangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int x3,
    int y3,
    Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawTriangle(x1, y1, x2, y2, x3, y3, color1);
  }

  public static void DrawTriangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int x3,
    int y3,
    int color)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    WriteableBitmapExtensions.DrawLine(bmp, pixelWidth, pixelHeight, x1, y1, x2, y2, color);
    WriteableBitmapExtensions.DrawLine(bmp, pixelWidth, pixelHeight, x2, y2, x3, y3, color);
    WriteableBitmapExtensions.DrawLine(bmp, pixelWidth, pixelHeight, x3, y3, x1, y1, color);
  }

  public static void DrawRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color,
    List<int> seriesPixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawRectangle(x1, y1, x2, y2, color1, seriesPixels);
  }

  public static unsafe void DrawRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int* pixels = bmp.GetPixels();
    if (x1 < 0 && x2 < 0 || y1 < 0 && y2 < 0 || x1 >= pixelWidth && x2 >= pixelWidth || y1 >= pixelHeight && y2 >= pixelHeight)
      return;
    if (x1 < 0)
      x1 = 0;
    if (y1 < 0)
      y1 = 0;
    if (x2 < 0)
      x2 = 0;
    if (y2 < 0)
      y2 = 0;
    if (x1 > pixelWidth)
      x1 = pixelWidth;
    if (y1 > pixelHeight)
      y1 = pixelHeight;
    if (x2 > pixelWidth)
      x2 = pixelWidth;
    if (y2 > pixelHeight)
      y2 = pixelHeight;
    int num1 = y1 * pixelWidth;
    int index1 = y2 * pixelWidth - pixelWidth + x1;
    int num2 = num1 + x2;
    int num3 = num1 + x1;
    int num4 = num1 + x2;
    int num5 = x2 - x1;
    int num6 = num3;
    int num7 = y2 * pixelWidth + x1;
    for (int index2 = num3; index2 < num2; ++index2)
    {
      pixels[index2] = color;
      pixels[index1] = color;
      ++index1;
    }
    int index3 = num3 + pixelWidth;
    int num8 = index1 - pixelWidth;
    for (int index4 = num1 + x2 - 1 + pixelWidth; index4 < num8; index4 += pixelWidth)
    {
      pixels[index4] = color;
      pixels[index3] = color;
      index3 += pixelWidth;
    }
    for (int index5 = num6 + pixelWidth; index5 <= num7; index5 += pixelWidth)
    {
      for (int index6 = num3; index6 < num4; ++index6)
        seriesPixels.Add(index6);
      num3 = index5;
      num4 = num3 + num5;
    }
  }

  public static void FillRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color,
    List<int> seriesPixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.FillRectangle(x1, y1, x2, y2, color1, seriesPixels);
  }

  public static unsafe void FillRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color,
    List<int> seriesPixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int* pixels = bmp.GetPixels();
    if (x1 < 0 && x2 < 0 || y1 < 0 && y2 < 0 || x1 >= pixelWidth && x2 >= pixelWidth || y1 >= pixelHeight && y2 >= pixelHeight)
      return;
    if (x1 < 0)
      x1 = 0;
    if (y1 < 0)
      y1 = 0;
    if (x2 < 0)
      x2 = 0;
    if (y2 < 0)
      y2 = 0;
    if (x1 > pixelWidth)
      x1 = pixelWidth;
    if (y1 > pixelHeight)
      y1 = pixelHeight;
    if (x2 > pixelWidth)
      x2 = pixelWidth;
    if (y2 > pixelHeight)
      y2 = pixelHeight;
    if (y2 - y1 == 0)
      ++y2;
    if (x2 >= pixelWidth)
      --x2;
    int num1 = y1 * pixelWidth;
    int num2 = num1 + x1;
    int num3 = num1 + x2;
    int num4 = x2 - x1;
    int num5 = num2;
    int num6 = y2 * pixelWidth + x1;
    for (int index1 = num5 + pixelWidth; index1 <= num6; index1 += pixelWidth)
    {
      for (int index2 = num2; index2 <= num3; ++index2)
      {
        if (pixels[index2] != color)
        {
          pixels[index2] = color;
          seriesPixels.Add(index2);
        }
      }
      num2 = index1;
      num3 = num2 + num4;
    }
  }

  public static void FillPolygon(
    this WriteableBitmap bmp,
    int[] points,
    Color color,
    List<int> pixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.FillPolygon(points, color1, pixels);
  }

  public static unsafe void FillPolygon(
    this WriteableBitmap bmp,
    int[] points,
    Color col,
    List<int> seriesPixels,
    Rect clip)
  {
    int num1 = WriteableBitmapExtensions.ConvertColor(col);
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int top = (int) clip.Top;
    int bottom = (int) clip.Bottom;
    int left = (int) clip.Left;
    int right = (int) clip.Right;
    int* pixels = bmp.GetPixels();
    int length = points.Length;
    int[] numArray = new int[points.Length >> 1];
    int num2 = pixelHeight;
    int num3 = 0;
    for (int index = 1; index < length; index += 2)
    {
      int point = points[index];
      if (point < num2)
        num2 = point;
      if (point > num3)
        num3 = point;
    }
    if (num2 < 0)
      num2 = 0;
    if (num3 >= pixelHeight)
      num3 = pixelHeight - 1;
    if (num2 < top)
      num2 = top;
    if (num3 >= bottom)
      num3 = bottom - 1;
    for (int index1 = num2; index1 <= num3; ++index1)
    {
      float num4 = (float) points[0];
      float num5 = (float) points[1];
      int num6 = 0;
      for (int index2 = 2; index2 < length; index2 += 2)
      {
        float point1 = (float) points[index2];
        float point2 = (float) points[index2 + 1];
        if ((double) num5 < (double) index1 && (double) point2 >= (double) index1 || (double) point2 < (double) index1 && (double) num5 >= (double) index1)
          numArray[num6++] = (int) ((double) num4 + ((double) index1 - (double) num5) / ((double) point2 - (double) num5) * ((double) point1 - (double) num4));
        num4 = point1;
        num5 = point2;
      }
      for (int index3 = 1; index3 < num6; ++index3)
      {
        int num7 = numArray[index3];
        int index4;
        for (index4 = index3; index4 > 0 && numArray[index4 - 1] > num7; --index4)
          numArray[index4] = numArray[index4 - 1];
        numArray[index4] = num7;
      }
      for (int index5 = 0; index5 < num6 - 1; index5 += 2)
      {
        int num8 = numArray[index5];
        int num9 = numArray[index5 + 1];
        if (num9 > 0 && num8 < pixelWidth && num9 > left && num8 < right)
        {
          if (num8 < 0)
            num8 = 0;
          if (num9 >= pixelWidth)
            num9 = pixelWidth - 1;
          if (num8 < left)
            num8 = left;
          if (num9 >= right)
            num9 = right - 1;
          for (int index6 = num8; index6 <= num9; ++index6)
          {
            int index7 = index1 * pixelWidth + index6;
            if (pixels[index7] != num1)
            {
              pixels[index7] = num1;
              seriesPixels.Add(index7);
            }
          }
        }
      }
    }
  }

  public static unsafe void FillPolygon(
    this WriteableBitmap bmp,
    int[] points,
    int color,
    List<int> seriesPixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    int* pixels = bmp.GetPixels();
    int length = points.Length;
    int[] numArray = new int[points.Length >> 1];
    int num1 = pixelHeight;
    int num2 = 0;
    for (int index = 1; index < length; index += 2)
    {
      int point = points[index];
      if (point < num1)
        num1 = point;
      if (point > num2)
        num2 = point;
    }
    if (num1 < 0)
      num1 = 0;
    if (num2 >= pixelHeight)
      num2 = pixelHeight - 1;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      float num3 = (float) points[0];
      float num4 = (float) points[1];
      int num5 = 0;
      for (int index2 = 2; index2 < length; index2 += 2)
      {
        float point1 = (float) points[index2];
        float point2 = (float) points[index2 + 1];
        if ((double) num4 < (double) index1 && (double) point2 >= (double) index1 || (double) point2 < (double) index1 && (double) num4 >= (double) index1)
          numArray[num5++] = (int) ((double) num3 + ((double) index1 - (double) num4) / ((double) point2 - (double) num4) * ((double) point1 - (double) num3));
        num3 = point1;
        num4 = point2;
      }
      for (int index3 = 1; index3 < num5; ++index3)
      {
        int num6 = numArray[index3];
        int index4;
        for (index4 = index3; index4 > 0 && numArray[index4 - 1] > num6; --index4)
          numArray[index4] = numArray[index4 - 1];
        numArray[index4] = num6;
      }
      for (int index5 = 0; index5 < num5 - 1; index5 += 2)
      {
        int num7 = numArray[index5];
        int num8 = numArray[index5 + 1];
        if (num8 > 0 && num7 < pixelWidth)
        {
          if (num7 < 0)
            num7 = 0;
          if (num8 >= pixelWidth)
            num8 = pixelWidth - 1;
          for (int index6 = num7; index6 <= num8; ++index6)
          {
            int index7 = index1 * pixelWidth + index6;
            if (pixels[index7] != color)
            {
              pixels[index7] = color;
              seriesPixels.Add(index7);
            }
          }
        }
      }
    }
  }

  public static void DrawEllipse(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawEllipse(x1, y1, x2, y2, color1);
  }

  public static void DrawEllipse(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    int color)
  {
    int xr = x2 - x1 >> 1;
    int yr = y2 - y1 >> 1;
    int xc = x1 + xr;
    int yc = y1 + yr;
    bmp.DrawEllipseCentered(xc, yc, xr, yr, color);
  }

  public static void DrawEllipseCentered(
    this WriteableBitmap bmp,
    int xc,
    int yc,
    int xr,
    int yr,
    Color color)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.DrawEllipseCentered(xc, yc, xr, yr, color1);
  }

  public static unsafe void DrawEllipseCentered(
    this WriteableBitmap bmp,
    int xc,
    int yc,
    int xr,
    int yr,
    int color)
  {
    int* pixels = bmp.GetPixels();
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    if (xr < 1 || yr < 1)
      return;
    int num1 = xr;
    int num2 = 0;
    int num3 = xr * xr << 1;
    int num4 = yr * yr << 1;
    int num5 = yr * yr * (1 - (xr << 1));
    int num6 = xr * xr;
    int num7 = 0;
    int num8 = num4 * xr;
    int num9 = 0;
    while (num8 >= num9)
    {
      int num10 = yc + num2;
      int num11 = yc - num2;
      if (num10 < 0)
        num10 = 0;
      if (num10 >= pixelHeight)
        num10 = pixelHeight - 1;
      if (num11 < 0)
        num11 = 0;
      if (num11 >= pixelHeight)
        num11 = pixelHeight - 1;
      int num12 = num10 * pixelWidth;
      int num13 = num11 * pixelWidth;
      int num14 = xc + num1;
      int num15 = xc - num1;
      if (num14 < 0)
        num14 = 0;
      if (num14 >= pixelWidth)
        num14 = pixelWidth - 1;
      if (num15 < 0)
        num15 = 0;
      if (num15 >= pixelWidth)
        num15 = pixelWidth - 1;
      pixels[num14 + num12] = color;
      pixels[num15 + num12] = color;
      pixels[num15 + num13] = color;
      pixels[num14 + num13] = color;
      ++num2;
      num9 += num3;
      num7 += num6;
      num6 += num3;
      if (num5 + (num7 << 1) > 0)
      {
        --num1;
        num8 -= num4;
        num7 += num5;
        num5 += num4;
      }
    }
    int num16 = 0;
    int num17 = yr;
    int num18 = yc + num17;
    int num19 = yc - num17;
    if (num18 < 0)
      num18 = 0;
    if (num18 >= pixelHeight)
      num18 = pixelHeight - 1;
    if (num19 < 0)
      num19 = 0;
    if (num19 >= pixelHeight)
      num19 = pixelHeight - 1;
    int num20 = num18 * pixelWidth;
    int num21 = num19 * pixelWidth;
    int num22 = yr * yr;
    int num23 = xr * xr * (1 - (yr << 1));
    int num24 = 0;
    int num25 = 0;
    int num26 = num3 * yr;
    while (num25 <= num26)
    {
      int num27 = xc + num16;
      int num28 = xc - num16;
      if (num27 < 0)
        num27 = 0;
      if (num27 >= pixelWidth)
        num27 = pixelWidth - 1;
      if (num28 < 0)
        num28 = 0;
      if (num28 >= pixelWidth)
        num28 = pixelWidth - 1;
      pixels[num27 + num20] = color;
      pixels[num28 + num20] = color;
      pixels[num28 + num21] = color;
      pixels[num27 + num21] = color;
      ++num16;
      num25 += num4;
      num24 += num22;
      num22 += num4;
      if (num23 + (num24 << 1) > 0)
      {
        --num17;
        int num29 = yc + num17;
        int num30 = yc - num17;
        if (num29 < 0)
          num29 = 0;
        if (num29 >= pixelHeight)
          num29 = pixelHeight - 1;
        if (num30 < 0)
          num30 = 0;
        if (num30 >= pixelHeight)
          num30 = pixelHeight - 1;
        num20 = num29 * pixelWidth;
        num21 = num30 * pixelWidth;
        num26 -= num3;
        num24 += num23;
        num23 += num3;
      }
    }
  }

  public static void FillEllipseCentered(
    this WriteableBitmap bmp,
    int xc,
    int yc,
    int xr,
    int yr,
    Color color,
    List<int> seriesPixels)
  {
    int color1 = WriteableBitmapExtensions.ConvertColor(color);
    bmp.FillEllipseCentered(xc, yc, xr, yr, color1, seriesPixels);
  }

  public static unsafe void FillEllipseCentered(
    this WriteableBitmap bmp,
    int xc,
    int yc,
    int xr,
    int yr,
    int color,
    List<int> seriesPixels)
  {
    int* pixels = bmp.GetPixels();
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    xr >>= 1;
    yr >>= 1;
    if (xr < 1 || yr < 1 || xc - xr >= pixelWidth || xc + xr < 0 || yc - yr >= pixelHeight || yc + yr < 0)
      return;
    int num1 = xr;
    int num2 = 0;
    int num3 = xr * xr << 1;
    int num4 = yr * yr << 1;
    int num5 = yr * yr * (1 - (xr << 1));
    int num6 = xr * xr;
    int num7 = 0;
    int num8 = num4 * xr;
    int num9 = 0;
    while (num8 >= num9)
    {
      int num10 = yc + num2;
      int num11 = yc - num2 - 1;
      int num12 = xc + num1;
      for (int index1 = xc - num1; index1 <= num12; ++index1)
      {
        int num13 = num10 * pixelWidth;
        int index2 = index1 + num13;
        if (0 <= num10 && num10 < pixelHeight && 0 <= index1 && index1 < pixelWidth && pixels[index2] != color)
        {
          pixels[index2] = color;
          seriesPixels.Add(index2);
        }
        int num14 = num11 * pixelWidth;
        int index3 = index1 + num14;
        if (0 <= num11 && num11 < pixelHeight && 0 <= index1 && index1 < pixelWidth && pixels[index3] != color)
        {
          pixels[index3] = color;
          seriesPixels.Add(index3);
        }
      }
      ++num2;
      num9 += num3;
      num7 += num6;
      num6 += num3;
      if (num5 + (num7 << 1) > 0)
      {
        --num1;
        num8 -= num4;
        num7 += num5;
        num5 += num4;
      }
    }
    int num15 = 0;
    int num16 = yr;
    int num17 = yc + num16;
    int num18 = yc - num16;
    int num19 = num17 * pixelWidth;
    int num20 = num18 * pixelWidth;
    int num21 = yr * yr;
    int num22 = xr * xr * (1 - (yr << 1));
    int num23 = 0;
    int num24 = 0;
    int num25 = num3 * yr;
    while (num24 <= num25)
    {
      int num26 = xc + num15;
      for (int index4 = xc - num15; index4 <= num26; ++index4)
      {
        int index5 = index4 + num19;
        if (0 <= index4 && index4 < pixelWidth && 0 <= num17 && num17 < pixelHeight && pixels[index5] != color)
        {
          pixels[index5] = color;
          seriesPixels.Add(index5);
        }
        int index6 = index4 + num20;
        if (0 <= index4 && index4 < pixelWidth && 0 <= num18 && num18 < pixelHeight && pixels[index6] != color)
        {
          pixels[index4 + num20] = color;
          seriesPixels.Add(index6);
        }
      }
      ++num15;
      num24 += num4;
      num23 += num21;
      num21 += num4;
      if (num22 + (num23 << 1) > 0)
      {
        --num16;
        num17 = yc + num16;
        num18 = yc - num16;
        num19 = num17 * pixelWidth;
        num20 = num18 * pixelWidth;
        num25 -= num3;
        num23 += num22;
        num22 += num3;
      }
    }
  }

  public static List<int> GetEllipseCentered(
    this WriteableBitmap bmp,
    int xc,
    int yc,
    int xr,
    int yr,
    List<int> pixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    if (xr < 1 || yr < 1)
      return pixels;
    int num1 = xr;
    int num2 = 0;
    int num3 = xr * xr << 1;
    int num4 = yr * yr << 1;
    int num5 = yr * yr * (1 - (xr << 1));
    int num6 = xr * xr;
    int num7 = 0;
    int num8 = num4 * xr;
    int num9 = 0;
    while (num8 >= num9)
    {
      int num10 = yc + num2;
      int num11 = yc - num2;
      if (num10 < 0)
        num10 = 0;
      if (num10 >= pixelHeight)
        num10 = pixelHeight - 1;
      if (num11 < 0)
        num11 = 0;
      if (num11 >= pixelHeight)
        num11 = pixelHeight - 1;
      int num12 = num10 * pixelWidth;
      int num13 = num11 * pixelWidth;
      int num14 = xc + num1;
      int num15 = xc - num1;
      if (num14 < 0)
        num14 = 0;
      if (num14 >= pixelWidth)
        num14 = pixelWidth - 1;
      if (num15 < 0)
        num15 = 0;
      if (num15 >= pixelWidth)
        num15 = pixelWidth - 1;
      for (int index = num15; index <= num14; ++index)
      {
        pixels.Add(index + num12);
        pixels.Add(index + num13);
      }
      ++num2;
      num9 += num3;
      num7 += num6;
      num6 += num3;
      if (num5 + (num7 << 1) > 0)
      {
        --num1;
        num8 -= num4;
        num7 += num5;
        num5 += num4;
      }
    }
    int num16 = 0;
    int num17 = yr;
    int num18 = yc + num17;
    int num19 = yc - num17;
    if (num18 < 0)
      num18 = 0;
    if (num18 >= pixelHeight)
      num18 = pixelHeight - 1;
    if (num19 < 0)
      num19 = 0;
    if (num19 >= pixelHeight)
      num19 = pixelHeight - 1;
    int num20 = num18 * pixelWidth;
    int num21 = num19 * pixelWidth;
    int num22 = yr * yr;
    int num23 = xr * xr * (1 - (yr << 1));
    int num24 = 0;
    int num25 = 0;
    int num26 = num3 * yr;
    while (num25 <= num26)
    {
      int num27 = xc + num16;
      int num28 = xc - num16;
      if (num27 < 0)
        num27 = 0;
      if (num27 >= pixelWidth)
        num27 = pixelWidth - 1;
      if (num28 < 0)
        num28 = 0;
      if (num28 >= pixelWidth)
        num28 = pixelWidth - 1;
      for (int index = num28; index <= num27; ++index)
      {
        pixels.Add(index + num20);
        pixels.Add(index + num21);
      }
      ++num16;
      num25 += num4;
      num24 += num22;
      num22 += num4;
      if (num23 + (num24 << 1) > 0)
      {
        --num17;
        int num29 = yc + num17;
        int num30 = yc - num17;
        if (num29 < 0)
          num29 = 0;
        if (num29 >= pixelHeight)
          num29 = pixelHeight - 1;
        if (num30 < 0)
          num30 = 0;
        if (num30 >= pixelHeight)
          num30 = pixelHeight - 1;
        num20 = num29 * pixelWidth;
        num21 = num30 * pixelWidth;
        num26 -= num3;
        num24 += num23;
        num23 += num3;
      }
    }
    return pixels;
  }

  public static List<int> GetRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    List<int> pixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    if (x1 < 0 && x2 < 0 || y1 < 0 && y2 < 0 || x1 >= pixelWidth && x2 >= pixelWidth || y1 >= pixelHeight && y2 >= pixelHeight)
      return pixels;
    if (x1 < 0)
      x1 = 0;
    if (y1 < 0)
      y1 = 0;
    if (x2 < 0)
      x2 = 0;
    if (y2 < 0)
      y2 = 0;
    if (x1 > pixelWidth)
      x1 = pixelWidth;
    if (y1 > pixelHeight)
      y1 = pixelHeight;
    if (x2 > pixelWidth)
      x2 = pixelWidth;
    if (y2 > pixelHeight)
      y2 = pixelHeight;
    int num1 = y1 * pixelWidth;
    int num2 = num1 + x1;
    int num3 = num1 + x2;
    int num4 = x2 - x1;
    int num5 = num2;
    int num6 = y2 * pixelWidth + x1;
    for (int index1 = num5 + pixelWidth; index1 <= num6; index1 += pixelWidth)
    {
      for (int index2 = num2; index2 < num3; ++index2)
        pixels.Add(index2);
      num2 = index1;
      num3 = num2 + num4;
    }
    return pixels;
  }

  internal static List<int> GetDrawRectangle(
    this WriteableBitmap bmp,
    int x1,
    int y1,
    int x2,
    int y2,
    List<int> pixels)
  {
    int pixelWidth = bmp.PixelWidth;
    int pixelHeight = bmp.PixelHeight;
    if (x1 < 0 && x2 < 0 || y1 < 0 && y2 < 0 || x1 >= pixelWidth && x2 >= pixelWidth || y1 >= pixelHeight && y2 >= pixelHeight)
      return pixels;
    if (x1 < 0)
      x1 = 0;
    if (y1 < 0)
      y1 = 0;
    if (x2 < 0)
      x2 = 0;
    if (y2 < 0)
      y2 = 0;
    if (x1 > pixelWidth)
      x1 = pixelWidth;
    if (y1 > pixelHeight)
      y1 = pixelHeight;
    if (x2 > pixelWidth)
      x2 = pixelWidth;
    if (y2 > pixelHeight)
      y2 = pixelHeight;
    int num1 = y1 * pixelWidth;
    int num2 = num1 + x1;
    int num3 = num1 + x2;
    int num4 = y2 * pixelWidth + x1;
    int num5 = y2 * pixelWidth - pixelWidth + x1;
    for (int index = num2; index < num3; ++index)
    {
      pixels.Add(index);
      pixels.Add(num5);
      ++num5;
    }
    int num6 = num2 + pixelWidth;
    int num7 = num4 - pixelWidth;
    for (int index = num1 + x2 - 1 + pixelWidth; index < num7; index += pixelWidth)
    {
      pixels.Add(index);
      pixels.Add(num6);
      num6 += pixelWidth;
    }
    return pixels;
  }
}
