// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.HsvColor
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public struct HsvColor(double h, double s, double v)
{
  public double H = h;
  public double S = s;
  public double V = v;

  public static HsvColor ConvertRgbToHsv(int r, int b, int g)
  {
    double num1 = 0.0;
    double num2 = (double) Math.Min(Math.Min(r, g), b);
    double num3 = (double) Math.Max(Math.Max(r, g), b);
    double num4 = num3 - num2;
    double num5 = num3 != 0.0 ? num4 / num3 : 0.0;
    double num6;
    if (num5 == 0.0)
    {
      num6 = 0.0;
    }
    else
    {
      if ((double) r == num3)
        num1 = (double) (g - b) / num4;
      else if ((double) g == num3)
        num1 = 2.0 + (double) (b - r) / num4;
      else if ((double) b == num3)
        num1 = 4.0 + (double) (r - g) / num4;
      num6 = num1 * 60.0;
      if (num6 < 0.0)
        num6 += 360.0;
    }
    return new HsvColor()
    {
      H = num6,
      S = num5,
      V = num3 / (double) byte.MaxValue
    };
  }

  public static Color ConvertHsvToRgb(double h, double s, double v)
  {
    double num1;
    double num2;
    double num3;
    if (s == 0.0)
    {
      num1 = v;
      num2 = v;
      num3 = v;
    }
    else
    {
      if (h == 360.0)
        h = 0.0;
      else
        h /= 60.0;
      int num4 = (int) Math.Truncate(h);
      double num5 = h - (double) num4;
      double num6 = v * (1.0 - s);
      double num7 = v * (1.0 - s * num5);
      double num8 = v * (1.0 - s * (1.0 - num5));
      switch (num4)
      {
        case 0:
          num1 = v;
          num2 = num8;
          num3 = num6;
          break;
        case 1:
          num1 = num7;
          num2 = v;
          num3 = num6;
          break;
        case 2:
          num1 = num6;
          num2 = v;
          num3 = num8;
          break;
        case 3:
          num1 = num6;
          num2 = num7;
          num3 = v;
          break;
        case 4:
          num1 = num8;
          num2 = num6;
          num3 = v;
          break;
        default:
          num1 = v;
          num2 = num6;
          num3 = num7;
          break;
      }
    }
    return Color.FromArgb(byte.MaxValue, (byte) (num1 * (double) byte.MaxValue), (byte) (num2 * (double) byte.MaxValue), (byte) (num3 * (double) byte.MaxValue));
  }
}
