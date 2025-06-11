// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Themes.ThemeUtilities
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation.Themes;

internal class ThemeUtilities
{
  internal static int ApplyAlpha(int color, double doubleValue)
  {
    return ColorObject.FromArgb((int) Math.Round((double) byte.MaxValue * (1.0 - doubleValue), 0), (IColor) (ColorObject.FromArgb(color) as ColorObject)).ToArgb();
  }

  internal static int ApplyAlphaMod(int colorValue, double doubleValue)
  {
    ColorObject baseColor = ColorObject.FromArgb(colorValue) as ColorObject;
    return ColorObject.FromArgb((int) Math.Round((double) byte.MaxValue * (1.0 - (1.0 - (double) baseColor.A / (double) byte.MaxValue) * doubleValue), 0), (IColor) baseColor).ToArgb();
  }

  internal static int ApplyAlphaOff(int colorValue, double doubleValue)
  {
    ColorObject baseColor = ColorObject.FromArgb(colorValue) as ColorObject;
    return ColorObject.FromArgb((int) Math.Round((double) byte.MaxValue * (1.0 - (1.0 - (double) baseColor.A / (double) byte.MaxValue) - doubleValue), 0), (IColor) baseColor).ToArgb();
  }

  internal static int ApplyBlue(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int blue = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) byte.MaxValue) * doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, (int) colorObject.G, blue).ToArgb();
  }

  internal static int ApplyBlueMod(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int blue = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.B) * doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, (int) colorObject.G, blue).ToArgb();
  }

  internal static int ApplyBlueOff(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int blue = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.B) + doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, (int) colorObject.G, blue).ToArgb();
  }

  internal static int ApplyComp(int color)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    if (currentHsl.CurrentHue() < 180.0)
      currentHsl.RoundOffHue(currentHsl.CurrentHue() + 180.0);
    else
      currentHsl.RoundOffHue(currentHsl.CurrentHue() - 180.0);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyGamma(int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int[] numArray = new int[3]
    {
      (int) colorObject.R,
      (int) colorObject.G,
      (int) colorObject.B
    };
    for (int index = 0; index < 3; ++index)
      numArray[index] = (int) Math.Round(Math.Min((double) byte.MaxValue, (double) byte.MaxValue * Math.Pow((double) numArray[index] / (double) byte.MaxValue, 5.0 / 11.0) + 0.5), 0);
    return ColorObject.FromArgb(numArray[0], numArray[1], numArray[2]).ToArgb();
  }

  internal static int ApplyGray(int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int num = (int) Math.Round((double) colorObject.R * 0.2126 + (double) colorObject.G * (447.0 / 625.0) + (double) colorObject.B * 0.0722, 0);
    return ColorObject.FromArgb(num, num, num).ToArgb();
  }

  internal static int ApplyGreen(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int green = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) byte.MaxValue) * doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, green, (int) colorObject.B).ToArgb();
  }

  internal static int ApplyGreenMod(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int green = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.G) * doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, green, (int) colorObject.B).ToArgb();
  }

  internal static int ApplyGreenOff(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int green = ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.G) + doubleValue);
    return ColorObject.FromArgb((int) colorObject.R, green, (int) colorObject.B).ToArgb();
  }

  internal static int ApplyHue(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffHue(doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyHueMod(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffHue(currentHsl.CurrentHue() * doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyHueOff(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffHue(currentHsl.CurrentHue() + doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyInv(int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int[] numArray = new int[3]
    {
      (int) colorObject.R,
      (int) colorObject.G,
      (int) colorObject.B
    };
    for (int index = 0; index < 3; ++index)
    {
      double doubleValue = 1.0 - ThemeUtilities.CalcDouble(numArray[index]);
      numArray[index] = ThemeUtilities.CalcInt(doubleValue);
    }
    return ColorObject.FromArgb(numArray[0], numArray[1], numArray[2]).ToArgb();
  }

  internal static int ApplyInvGamma(int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int[] numArray = new int[3]
    {
      (int) colorObject.R,
      (int) colorObject.G,
      (int) colorObject.B
    };
    for (int index = 0; index < 3; ++index)
      numArray[index] = (int) Math.Round((double) byte.MaxValue * Math.Pow((double) numArray[index] / (double) byte.MaxValue, 2.2), 0);
    return ColorObject.FromArgb(numArray[0], numArray[1], numArray[2]).ToArgb();
  }

  internal static int ApplyLum(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffLuminance(doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyLumMod(int color, double lumModValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffLuminance(currentHsl.CurrentLuminance() * lumModValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyLumOff(int color, double lumOffValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffLuminance(currentHsl.CurrentLuminance() + lumOffValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyRed(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    return ColorObject.FromArgb(ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) byte.MaxValue) * doubleValue), (int) colorObject.G, (int) colorObject.B).ToArgb();
  }

  internal static int ApplyRedMod(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    return ColorObject.FromArgb(ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.R) * doubleValue), (int) colorObject.G, (int) colorObject.B).ToArgb();
  }

  internal static int ApplyRedOff(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    return ColorObject.FromArgb(ThemeUtilities.CalcInt(ThemeUtilities.CalcDouble((int) colorObject.R) + doubleValue), (int) colorObject.G, (int) colorObject.B).ToArgb();
  }

  internal static int ApplySat(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffSaturation(doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplySatMod(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffSaturation(currentHsl.CurrentSaturation() * doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplySatOff(int color, double doubleValue)
  {
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    currentHsl.RoundOffSaturation(currentHsl.CurrentSaturation() + doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }

  internal static int ApplyShade(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int[] numArray = new int[3]
    {
      (int) colorObject.R,
      (int) colorObject.G,
      (int) colorObject.B
    };
    for (int index = 0; index < 3; ++index)
    {
      double doubleValue1 = ThemeUtilities.CalcDouble(numArray[index]) * doubleValue;
      if (doubleValue1 < 0.0)
        doubleValue1 = 0.0;
      else if (doubleValue1 > 1.0)
        doubleValue1 = 1.0;
      numArray[index] = ThemeUtilities.CalcInt(doubleValue1);
    }
    return ColorObject.FromArgb(numArray[0], numArray[1], numArray[2]).ToArgb();
  }

  internal static int ApplyTint(int colorValue, double doubleValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    int[] numArray = new int[3]
    {
      (int) colorObject.R,
      (int) colorObject.G,
      (int) colorObject.B
    };
    for (int index = 0; index < 3; ++index)
    {
      double num = ThemeUtilities.CalcDouble(numArray[index]);
      double doubleValue1 = num * (1.0 + (1.0 - doubleValue));
      if (doubleValue > 0.0)
        doubleValue1 = num * (1.0 - (1.0 - doubleValue)) + (1.0 - doubleValue);
      numArray[index] = ThemeUtilities.CalcInt(doubleValue1);
    }
    return ColorObject.FromArgb(numArray[0], numArray[1], numArray[2]).ToArgb();
  }

  internal static int HsltoRgb(CurrentHsl currentHsl, int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    if (currentHsl.CurrentSaturation() == 0.0)
    {
      int num = (int) Math.Round(currentHsl.CurrentLuminance() * (double) byte.MaxValue, 0);
      return colorObject.A != byte.MaxValue ? ColorObject.FromArgb((int) colorObject.A, num, num, num).ToArgb() : ColorObject.FromArgb(num, num, num).ToArgb();
    }
    double num1 = currentHsl.CurrentLuminance() < 0.5 ? currentHsl.CurrentLuminance() * (1.0 + currentHsl.CurrentSaturation()) : currentHsl.CurrentLuminance() + currentHsl.CurrentSaturation() - currentHsl.CurrentLuminance() * currentHsl.CurrentSaturation();
    double num2 = 2.0 * currentHsl.CurrentLuminance() - num1;
    double num3 = currentHsl.CurrentHue() / 360.0;
    double[] numArray = new double[3]
    {
      num3 + 1.0 / 3.0,
      num3,
      num3 - 1.0 / 3.0
    };
    for (int index = 0; index < 3; ++index)
    {
      if (numArray[index] < 0.0)
        ++numArray[index];
      if (numArray[index] > 1.0)
        --numArray[index];
      numArray[index] = numArray[index] * 6.0 >= 1.0 ? (numArray[index] * 2.0 >= 1.0 ? (numArray[index] * 3.0 >= 2.0 ? num2 : num2 + (num1 - num2) * (2.0 / 3.0 - numArray[index]) * 6.0) : num1) : num2 + (num1 - num2) * 6.0 * numArray[index];
    }
    return ColorObject.FromArgb((int) colorObject.A, (int) Math.Round(numArray[0] * (double) byte.MaxValue, 0), (int) Math.Round(numArray[1] * (double) byte.MaxValue, 0), (int) Math.Round(numArray[2] * (double) byte.MaxValue)).ToArgb();
  }

  internal static CurrentHsl RgbtoHsl(int colorValue)
  {
    ColorObject colorObject = ColorObject.FromArgb(colorValue) as ColorObject;
    double doubleValue1 = 0.0;
    double doubleValue2 = 0.0;
    double val1_1 = (double) colorObject.R / (double) byte.MaxValue;
    double val1_2 = (double) colorObject.G / (double) byte.MaxValue;
    double val2 = (double) colorObject.B / (double) byte.MaxValue;
    double num1 = Math.Max(val1_1, Math.Max(val1_2, val2));
    double num2 = Math.Min(val1_1, Math.Min(val1_2, val2));
    if (num1 == num2)
      doubleValue1 = 0.0;
    else if (num1 == val1_1 && val1_2 >= val2)
      doubleValue1 = 60.0 * (val1_2 - val2) / (num1 - num2);
    else if (num1 == val1_1 && val1_2 < val2)
      doubleValue1 = 60.0 * (val1_2 - val2) / (num1 - num2) + 360.0;
    else if (num1 == val1_2)
      doubleValue1 = 60.0 * (val2 - val1_1) / (num1 - num2) + 120.0;
    else if (num1 == val2)
      doubleValue1 = 60.0 * (val1_1 - val1_2) / (num1 - num2) + 240.0;
    double doubleValue3 = (num1 + num2) / 2.0;
    if (doubleValue3 != 0.0 && num1 != num2)
    {
      if (0.0 < doubleValue3 && doubleValue3 <= 0.5)
        doubleValue2 = (num1 - num2) / (num1 + num2);
      else if (doubleValue3 > 0.5)
        doubleValue2 = (num1 - num2) / (2.0 - (num1 + num2));
    }
    else
      doubleValue2 = 0.0;
    return new CurrentHsl(doubleValue1, doubleValue2, doubleValue3);
  }

  internal static double CalcDouble(int integer)
  {
    double num = (double) integer / (double) byte.MaxValue;
    if (num < 0.0)
      return 0.0;
    if (num <= 0.04045)
      return num / 12.92;
    return num > 1.0 ? 1.0 : Math.Pow((num + 0.055) / 1.055, 2.4);
  }

  internal static int CalcInt(double doubleValue)
  {
    return (int) Math.Round((doubleValue >= 0.0 ? (doubleValue > 0.0031308 ? (doubleValue >= 1.0 ? 1.0 : 1.055 * Math.Pow(doubleValue, 5.0 / 12.0) - 0.055) : doubleValue * 12.92) : 0.0) * (double) byte.MaxValue, 0);
  }

  internal static int GetColFromValue(int color, double doubleValue)
  {
    if (doubleValue == 0.0)
      return color;
    CurrentHsl currentHsl = ThemeUtilities.RgbtoHsl(color);
    if (doubleValue < 0.0)
      currentHsl.RoundOffLuminance(currentHsl.CurrentLuminance() * (1.0 + doubleValue));
    else if (doubleValue > 0.0)
      currentHsl.RoundOffLuminance(currentHsl.CurrentLuminance() * (1.0 - doubleValue) + doubleValue);
    return ThemeUtilities.HsltoRgb(currentHsl, color);
  }
}
