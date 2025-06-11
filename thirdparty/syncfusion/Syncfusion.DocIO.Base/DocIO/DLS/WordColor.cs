// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WordColor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

[CLSCompliant(false)]
public class WordColor
{
  internal const byte MaxRGB = 255 /*0xFF*/;
  internal const int MaxHue = 360;
  private static readonly int[] WordKnownColors = new int[15]
  {
    0,
    -16777216 /*0xFF000000*/,
    -16776961,
    -16711681,
    -16744448 /*0xFF008000*/,
    -65281,
    -65536,
    -256,
    -1,
    -16777077,
    -16741493,
    -16751616,
    -7667573,
    -7667712,
    -256
  };
  internal static readonly uint[] ArgbArray = new uint[17]
  {
    4278190080U /*0xFF000000*/,
    WordColor.ConvertColorToRGB(Color.Black),
    WordColor.ConvertColorToRGB(Color.Blue),
    WordColor.ConvertColorToRGB(Color.Cyan),
    WordColor.ConvertColorToRGB(Color.Green),
    WordColor.ConvertColorToRGB(Color.Magenta),
    WordColor.ConvertColorToRGB(Color.Red),
    WordColor.ConvertColorToRGB(Color.Yellow),
    WordColor.ConvertColorToRGB(Color.White),
    WordColor.ConvertColorToRGB(Color.DarkBlue),
    WordColor.ConvertColorToRGB(Color.DarkCyan),
    WordColor.ConvertColorToRGB(Color.DarkGreen),
    WordColor.ConvertColorToRGB(Color.DarkMagenta),
    WordColor.ConvertColorToRGB(Color.DarkRed),
    WordColor.ConvertColorToRGB(Color.Gold),
    8421504U /*0x808080*/,
    WordColor.ConvertColorToRGB(Color.LightGray)
  };
  internal static readonly Color[] ColorsArray = new Color[17]
  {
    Color.Empty,
    Color.Black,
    Color.Blue,
    Color.Cyan,
    Color.Green,
    Color.Magenta,
    Color.Red,
    Color.Yellow,
    Color.White,
    Color.DarkBlue,
    Color.DarkCyan,
    Color.DarkGreen,
    Color.DarkMagenta,
    Color.DarkRed,
    Color.Gold,
    Color.FromArgb(8421504 /*0x808080*/),
    Color.LightGray
  };
  private Color m_color;
  private byte m_colorId;

  public static WordColor Empty => new WordColor((byte) 0);

  public static WordColor Black => new WordColor((byte) 1);

  public static WordColor Blue => new WordColor((byte) 2);

  public static WordColor Cyan => new WordColor((byte) 3);

  public static WordColor Green => new WordColor((byte) 4);

  public static WordColor Magenta => new WordColor((byte) 5);

  public static WordColor Red => new WordColor((byte) 6);

  public static WordColor Yellow => new WordColor((byte) 7);

  public static WordColor White => new WordColor((byte) 8);

  public static WordColor DarkBlue => new WordColor((byte) 9);

  public static WordColor DarkCyan => new WordColor((byte) 10);

  public static WordColor DarkGreen => new WordColor((byte) 11);

  public static WordColor DarkMagenta => new WordColor((byte) 12);

  public static WordColor DarkRed => new WordColor((byte) 13);

  public static WordColor DarkYellow => new WordColor((byte) 14);

  public static WordColor DarkGray => new WordColor((byte) 15);

  public static WordColor LightGray => new WordColor((byte) 16 /*0x10*/);

  public Color Color => this.m_color;

  public byte ColorId => this.m_colorId;

  public int RGB => WordColor.WordKnownColors[(int) this.m_colorId];

  public WordColor(byte index)
  {
    this.m_color = Color.FromArgb(WordColor.WordKnownColors[(int) index]);
    this.m_colorId = index;
  }

  internal static uint ConvertHSLToRGB(double hue, double saturation, double luminance)
  {
    return WordColor.ConvertColorToRGB(WordColor.ConvertHSLToColor(hue, saturation, luminance), false);
  }

  internal static Color ConvertColorByShade(Color color, double shade)
  {
    double colorValue1 = (double) color.R / (double) byte.MaxValue;
    double colorValue2 = (double) color.G / (double) byte.MaxValue;
    double colorValue3 = (double) color.B / (double) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(WordColor.ApplyShade(colorValue1, shade) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyShade(colorValue2, shade) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyShade(colorValue3, shade) * (double) byte.MaxValue));
  }

  private static double ApplyShade(double colorValue, double shade)
  {
    colorValue = WordColor.ConvertsRGBtoLinearRGB(colorValue) * shade;
    if (colorValue < 0.0)
      colorValue = 0.0;
    else if (colorValue > 1.0)
      colorValue = 1.0;
    colorValue = WordColor.ConvertsLinearRGBtoRGB(colorValue);
    return colorValue;
  }

  internal static Color ConvertColorByTint(Color color, double tint)
  {
    double colorValue1 = (double) color.R / (double) byte.MaxValue;
    double colorValue2 = (double) color.G / (double) byte.MaxValue;
    double colorValue3 = (double) color.B / (double) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(WordColor.ApplyTint(colorValue1, tint) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyTint(colorValue2, tint) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyTint(colorValue3, tint) * (double) byte.MaxValue));
  }

  private static double ApplyTint(double colorValue, double tint)
  {
    colorValue = WordColor.ConvertsRGBtoLinearRGB(colorValue);
    if (tint > 0.0)
      colorValue = (colorValue - 1.0) * tint + 1.0;
    else
      colorValue *= 2.0 - tint;
    colorValue = WordColor.ConvertsLinearRGBtoRGB(colorValue);
    return colorValue;
  }

  internal static byte ConvertbyModulation(byte value, double percent)
  {
    return (byte) Math.Round(WordColor.ConvertsLinearRGBtoRGB(WordColor.ConvertsRGBtoLinearRGB((double) value / (double) byte.MaxValue) * (percent / 100.0)) * (double) byte.MaxValue);
  }

  internal static byte ConvertbyOffset(byte value, double percent)
  {
    return (byte) Math.Round(WordColor.ConvertsLinearRGBtoRGB(WordColor.ConvertsRGBtoLinearRGB((double) value / (double) byte.MaxValue) + percent / 100.0) * (double) byte.MaxValue);
  }

  internal static void ConvertbyHue(ref Color color, double angle)
  {
    double saturation;
    double luminance;
    WordColor.ConvertColortoHSL(color, out double _, out saturation, out luminance);
    double hue = angle / 360.0;
    color = WordColor.ConvertHSLToColor(hue, saturation, luminance);
  }

  internal static void ConvertbyHueMod(ref Color color, double ratio)
  {
    double hue1;
    double saturation;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue1, out saturation, out luminance);
    double hue2 = hue1 * ratio;
    color = WordColor.ConvertHSLToColor(hue2, saturation, luminance);
  }

  internal static void ConvertbyHueOffset(ref Color color, double angle)
  {
    double hue1;
    double saturation;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue1, out saturation, out luminance);
    double hue2 = hue1 + angle / 360.0;
    color = WordColor.ConvertHSLToColor(hue2, saturation, luminance);
  }

  internal static void ConvertbyLum(ref Color color, double percent)
  {
    double hue;
    double saturation;
    WordColor.ConvertColortoHSL(color, out hue, out saturation, out double _);
    double luminance = percent / 100.0;
    color = WordColor.ConvertHSLToColor(hue, saturation, luminance);
  }

  internal static void ConvertbyLumMod(ref Color color, double percent)
  {
    double hue;
    double saturation;
    double luminance1;
    WordColor.ConvertColortoHSL(color, out hue, out saturation, out luminance1);
    double luminance2 = luminance1 * (percent / 100.0);
    color = WordColor.ConvertHSLToColor(hue, saturation, luminance2);
  }

  internal static void ConvertbyLumOffset(ref Color color, double percent)
  {
    double hue;
    double saturation;
    double luminance1;
    WordColor.ConvertColortoHSL(color, out hue, out saturation, out luminance1);
    double luminance2 = luminance1 + percent / 100.0;
    color = WordColor.ConvertHSLToColor(hue, saturation, luminance2);
  }

  internal static void ConvertbySat(ref Color color, double percent)
  {
    double hue;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue, out double _, out luminance);
    double saturation = percent / 100.0;
    color = WordColor.ConvertHSLToColor(hue, saturation, luminance);
  }

  internal static void ConvertbySatMod(ref Color color, double percent)
  {
    double hue;
    double saturation1;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue, out saturation1, out luminance);
    double saturation2 = saturation1 * (percent / 100.0);
    color = WordColor.ConvertHSLToColor(hue, saturation2, luminance);
  }

  internal static void ConvertbySatOffset(ref Color color, double percent)
  {
    double hue;
    double saturation1;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue, out saturation1, out luminance);
    double saturation2 = saturation1 + percent / 100.0;
    color = WordColor.ConvertHSLToColor(hue, saturation2, luminance);
  }

  internal static Color ComplementColor(Color color)
  {
    double hue1;
    double saturation;
    double luminance;
    WordColor.ConvertColortoHSL(color, out hue1, out saturation, out luminance);
    double hue2 = hue1 + 0.5;
    if (hue2 > 1.0)
      --hue2;
    return WordColor.ConvertHSLToColor(hue2, saturation, luminance);
  }

  internal static Color InverseGammaColor(Color color)
  {
    double num1 = (double) color.R / (double) byte.MaxValue;
    double num2 = (double) color.G / (double) byte.MaxValue;
    double num3 = (double) color.B / (double) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(WordColor.ConvertsRGBtoLinearRGB(num1) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ConvertsRGBtoLinearRGB(num2) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ConvertsRGBtoLinearRGB(num3) * (double) byte.MaxValue));
  }

  internal static double ConvertsRGBtoLinearRGB(double value)
  {
    if (value < 0.0)
      return 0.0;
    if (value <= 0.04045)
      return value / 12.92;
    return value <= 1.0 ? Math.Pow((value + 0.055) / 1.055, 2.4) : 1.0;
  }

  internal static Color GammaColor(Color color)
  {
    double num1 = (double) color.R / (double) byte.MaxValue;
    double num2 = (double) color.G / (double) byte.MaxValue;
    double num3 = (double) color.B / (double) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(WordColor.ConvertsLinearRGBtoRGB(num1) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ConvertsLinearRGBtoRGB(num2) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ConvertsLinearRGBtoRGB(num3) * (double) byte.MaxValue));
  }

  internal static double ConvertsLinearRGBtoRGB(double value)
  {
    if (value < 0.0)
      return 0.0;
    if (value <= 0.0031308)
      return value * 12.92;
    return value <= 1.0 ? 1.055 * Math.Pow(value, 5.0 / 12.0) - 0.055 : 1.0;
  }

  internal static Color GrayColor(Color color)
  {
    double num = Math.Round((double) color.R * 0.2126 + (double) color.G * (447.0 / 625.0) + (double) color.B * 0.0722);
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) num, (int) (byte) num, (int) (byte) num);
  }

  public static Color InverseColor(Color color)
  {
    double colorValue1 = (double) color.R / (double) byte.MaxValue;
    double colorValue2 = (double) color.G / (double) byte.MaxValue;
    double colorValue3 = (double) color.B / (double) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(WordColor.ApplyInverse(colorValue1) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyInverse(colorValue2) * (double) byte.MaxValue), (int) (byte) Math.Round(WordColor.ApplyInverse(colorValue3) * (double) byte.MaxValue));
  }

  private static double ApplyInverse(double colorValue)
  {
    colorValue = 1.0 - WordColor.ConvertsRGBtoLinearRGB(colorValue);
    if (colorValue < 0.0)
      colorValue = 0.0;
    else if (colorValue > 1.0)
      colorValue = 1.0;
    colorValue = WordColor.ConvertsLinearRGBtoRGB(colorValue);
    return colorValue;
  }

  internal static void ConvertColortoHSL(
    Color color,
    out double hue,
    out double saturation,
    out double luminance)
  {
    hue = 0.0;
    luminance = 0.0;
    saturation = 0.0;
    double val1_1 = (double) color.R / (double) byte.MaxValue;
    double val1_2 = (double) color.G / (double) byte.MaxValue;
    double val2 = (double) color.B / (double) byte.MaxValue;
    double num1 = Math.Min(val1_1, Math.Min(val1_2, val2));
    double num2 = Math.Max(val1_1, Math.Max(val1_2, val2));
    double num3 = num2 - num1;
    double num4 = num2 + num1;
    luminance = (num2 + num1) / 2.0;
    if (num2 == num1)
    {
      saturation = 0.0;
      hue = 0.0;
    }
    else
    {
      saturation = luminance >= 0.5 ? num3 / (2.0 - num4) : num3 / num4;
      if (val1_1 == num2)
        hue = 1.0 / 6.0 * (val1_2 - val2) / num3 - (val2 > val1_2 ? 1.0 : 0.0);
      else if (val1_2 == num2)
        hue = 1.0 / 6.0 * (val2 - val1_1) / num3 + 1.0 / 3.0;
      else if (val2 == num2)
        hue = 1.0 / 6.0 * (val1_1 - val1_2) / num3 + 2.0 / 3.0;
      if (hue < 0.0)
        ++hue;
      if (hue > 1.0)
        --hue;
    }
    if (saturation < 0.0)
      saturation = 0.0;
    if (saturation > 1.0)
      saturation = 1.0;
    if (luminance < 0.0)
      luminance = 0.0;
    if (luminance <= 1.0)
      return;
    luminance = 1.0;
  }

  internal static Color ConvertHSLToColor(double hue, double saturation, double luminance)
  {
    int blue;
    int red;
    int green;
    if (saturation == 0.0)
    {
      blue = (int) Math.Round(luminance * (double) byte.MaxValue);
      red = blue;
      green = blue;
    }
    else
    {
      double n2 = luminance >= 0.5 ? luminance + saturation - luminance * saturation : luminance * (1.0 + saturation);
      double n1 = 2.0 * luminance - n2;
      double hue1 = hue > 2.0 / 3.0 ? hue - 2.0 / 3.0 : hue + 1.0 / 3.0;
      red = (int) Math.Round((double) byte.MaxValue * WordColor.HueToRGB(n1, n2, hue1));
      green = (int) Math.Round((double) byte.MaxValue * WordColor.HueToRGB(n1, n2, hue));
      double hue2 = hue < 1.0 / 3.0 ? hue + 2.0 / 3.0 : hue - 1.0 / 3.0;
      blue = (int) Math.Round((double) byte.MaxValue * WordColor.HueToRGB(n1, n2, hue2));
    }
    if (red < 0)
      red = 0;
    if (green < 0)
      green = 0;
    if (blue < 0)
      blue = 0;
    if (red > (int) byte.MaxValue)
      red = (int) byte.MaxValue;
    if (green > (int) byte.MaxValue)
      green = (int) byte.MaxValue;
    if (blue > (int) byte.MaxValue)
      blue = (int) byte.MaxValue;
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) red, (int) (byte) green, (int) (byte) blue);
  }

  internal static double HueToRGB(double n1, double n2, double hue)
  {
    if (hue < 0.0)
      ++hue;
    if (hue > 1.0)
      --hue;
    return 6.0 * hue >= 1.0 ? (2.0 * hue >= 1.0 ? (3.0 * hue >= 2.0 ? n1 : n1 + (n2 - n1) * (2.0 / 3.0 - hue) * 6.0) : n2) : n1 + (n2 - n1) * 6.0 * hue;
  }

  public static Color IdToColor(int wordColorId)
  {
    return wordColorId < 0 || wordColorId > WordColor.WordKnownColors.Length - 1 ? Color.Empty : Color.FromArgb(WordColor.WordKnownColors[wordColorId]);
  }

  public static int ColorToId(Color color)
  {
    int argb = color.ToArgb();
    for (int id = 0; id < WordColor.WordKnownColors.Length; ++id)
    {
      if (WordColor.WordKnownColors[id] == argb)
        return id;
    }
    return WordColor.ReduceColor(color);
  }

  public static uint ConvertColorToRGB(Color color) => WordColor.ConvertColorToRGB(color, false);

  public static uint ConvertColorToRGB(Color color, bool ignoreAlpha)
  {
    uint num = 0U | (uint) color.R | (uint) color.G << 8 | (uint) color.B << 16 /*0x10*/;
    return ignoreAlpha ? num : num | (uint) ~color.A << 24;
  }

  public static uint ConvertIdToRGB(int colorId)
  {
    return colorId < WordColor.ArgbArray.Length ? WordColor.ArgbArray[colorId] : 4278190080U /*0xFF000000*/;
  }

  public static int ConvertRGBToId(uint rgb)
  {
    if (rgb == 4278190080U /*0xFF000000*/)
      return 0;
    double num = double.PositiveInfinity;
    int id = 1;
    for (int index = 0; index < WordColor.ArgbArray.Length; ++index)
    {
      double similarColor = WordColor.FindSimilarColor(WordColor.ArgbArray[index], rgb);
      if (similarColor <= num)
      {
        id = index;
        num = similarColor;
      }
    }
    return id;
  }

  public static Color ConvertRGBToColor(uint rgb)
  {
    if (rgb == 4278190080U /*0xFF000000*/)
      return Color.Empty;
    byte red = (byte) (rgb & (uint) byte.MaxValue);
    byte green = (byte) ((rgb & 65280U) >> 8);
    byte blue = (byte) ((rgb & 16711680U /*0xFF0000*/) >> 16 /*0x10*/);
    return Color.FromArgb((int) (byte) ~((rgb & 4278190080U /*0xFF000000*/) >> 24), (int) red, (int) green, (int) blue);
  }

  public static int ConvertColorToId(Color color)
  {
    return WordColor.ConvertRGBToId(WordColor.ConvertColorToRGB(color));
  }

  public static Color ConvertIdToColor(int id)
  {
    return WordColor.ConvertRGBToColor(WordColor.ConvertIdToRGB(id));
  }

  private static int ReduceColor(Color color)
  {
    int num1 = int.MaxValue;
    int num2 = 0;
    for (int index = 0; index < WordColor.WordKnownColors.Length; ++index)
    {
      Color color1 = Color.FromArgb(WordColor.WordKnownColors[index]);
      int num3 = Math.Abs((int) color1.R - (int) color.R) + Math.Abs((int) color1.B - (int) color.B) + Math.Abs((int) color1.G - (int) color.G);
      if (num3 < num1)
      {
        num2 = index;
        num1 = num3;
      }
    }
    return num2;
  }

  private static double FindSimilarColor(uint wordColor, uint rgbColor)
  {
    byte num1 = (byte) (wordColor & (uint) byte.MaxValue);
    byte num2 = (byte) ((wordColor & 65280U) >> 8);
    byte num3 = (byte) ((wordColor & 16711680U /*0xFF0000*/) >> 16 /*0x10*/);
    byte num4 = (byte) (rgbColor & (uint) byte.MaxValue);
    byte num5 = (byte) ((rgbColor & 65280U) >> 8);
    byte num6 = (byte) ((rgbColor & 16711680U /*0xFF0000*/) >> 16 /*0x10*/);
    return (double) (Math.Abs((int) num1 - (int) num4) + Math.Abs((int) num2 - (int) num5) + Math.Abs((int) num3 - (int) num6));
  }

  internal static bool IsVeryDarkColor(int r, int g, int b)
  {
    return !WordColor.IsNotVeryDarkColor(Color.FromArgb((int) byte.MaxValue, r, g, b));
  }

  internal static bool IsNotVeryDarkColor(Color backColor)
  {
    if (backColor.R >= (byte) 204)
      return true;
    switch (backColor.R)
    {
      case 0:
        return WordColor.CheckR0((int) backColor.G, (int) backColor.B);
      case 1:
        return WordColor.CheckR1((int) backColor.G, (int) backColor.B);
      case 2:
        return WordColor.CheckR2((int) backColor.G, (int) backColor.B);
      case 3:
        return WordColor.CheckR3((int) backColor.G, (int) backColor.B);
      case 4:
        return WordColor.CheckR4((int) backColor.G, (int) backColor.B);
      case 5:
        return WordColor.CheckR5((int) backColor.G, (int) backColor.B);
      case 6:
        return WordColor.CheckR6((int) backColor.G, (int) backColor.B);
      case 7:
        return WordColor.CheckR7((int) backColor.G, (int) backColor.B);
      case 8:
        return WordColor.CheckR8((int) backColor.G, (int) backColor.B);
      case 9:
        return WordColor.CheckR9((int) backColor.G, (int) backColor.B);
      case 10:
        return WordColor.CheckR10((int) backColor.G, (int) backColor.B);
      case 11:
        return WordColor.CheckR11((int) backColor.G, (int) backColor.B);
      case 12:
        return WordColor.CheckR12((int) backColor.G, (int) backColor.B);
      case 13:
        return WordColor.CheckR13((int) backColor.G, (int) backColor.B);
      case 14:
        return WordColor.CheckR14((int) backColor.G, (int) backColor.B);
      case 15:
        return WordColor.CheckR15((int) backColor.G, (int) backColor.B);
      case 16 /*0x10*/:
        return WordColor.CheckR16((int) backColor.G, (int) backColor.B);
      case 17:
        return WordColor.CheckR17((int) backColor.G, (int) backColor.B);
      case 18:
        return WordColor.CheckR18((int) backColor.G, (int) backColor.B);
      case 19:
        return WordColor.CheckR19((int) backColor.G, (int) backColor.B);
      case 20:
        return WordColor.CheckR20((int) backColor.G, (int) backColor.B);
      case 21:
        return WordColor.CheckR21((int) backColor.G, (int) backColor.B);
      case 22:
        return WordColor.CheckR22((int) backColor.G, (int) backColor.B);
      case 23:
        return WordColor.CheckR23((int) backColor.G, (int) backColor.B);
      case 24:
        return WordColor.CheckR24((int) backColor.G, (int) backColor.B);
      case 25:
        return WordColor.CheckR25((int) backColor.G, (int) backColor.B);
      case 26:
        return WordColor.CheckR26((int) backColor.G, (int) backColor.B);
      case 27:
        return WordColor.CheckR27((int) backColor.G, (int) backColor.B);
      case 28:
        return WordColor.CheckR28((int) backColor.G, (int) backColor.B);
      case 29:
        return WordColor.CheckR29((int) backColor.G, (int) backColor.B);
      case 30:
        return WordColor.CheckR30((int) backColor.G, (int) backColor.B);
      case 31 /*0x1F*/:
        return WordColor.CheckR31((int) backColor.G, (int) backColor.B);
      case 32 /*0x20*/:
        return WordColor.CheckR32((int) backColor.G, (int) backColor.B);
      case 33:
        return WordColor.CheckR33((int) backColor.G, (int) backColor.B);
      case 34:
        return WordColor.CheckR34((int) backColor.G, (int) backColor.B);
      case 35:
        return WordColor.CheckR35((int) backColor.G, (int) backColor.B);
      case 36:
        return WordColor.CheckR36((int) backColor.G, (int) backColor.B);
      case 37:
        return WordColor.CheckR37((int) backColor.G, (int) backColor.B);
      case 38:
        return WordColor.CheckR38((int) backColor.G, (int) backColor.B);
      case 39:
        return WordColor.CheckR39((int) backColor.G, (int) backColor.B);
      case 40:
        return WordColor.CheckR40((int) backColor.G, (int) backColor.B);
      case 41:
        return WordColor.CheckR41((int) backColor.G, (int) backColor.B);
      case 42:
        return WordColor.CheckR42((int) backColor.G, (int) backColor.B);
      case 43:
        return WordColor.CheckR43((int) backColor.G, (int) backColor.B);
      case 44:
        return WordColor.CheckR44((int) backColor.G, (int) backColor.B);
      case 45:
        return WordColor.CheckR45((int) backColor.G, (int) backColor.B);
      case 46:
        return WordColor.CheckR46((int) backColor.G, (int) backColor.B);
      case 47:
        return WordColor.CheckR47((int) backColor.G, (int) backColor.B);
      case 48 /*0x30*/:
        return WordColor.CheckR48((int) backColor.G, (int) backColor.B);
      case 49:
        return WordColor.CheckR49((int) backColor.G, (int) backColor.B);
      case 50:
        return WordColor.CheckR50((int) backColor.G, (int) backColor.B);
      case 51:
        return WordColor.CheckR51((int) backColor.G, (int) backColor.B);
      case 52:
        return WordColor.CheckR52((int) backColor.G, (int) backColor.B);
      case 53:
        return WordColor.CheckR53((int) backColor.G, (int) backColor.B);
      case 54:
        return WordColor.CheckR54((int) backColor.G, (int) backColor.B);
      case 55:
        return WordColor.CheckR55((int) backColor.G, (int) backColor.B);
      case 56:
        return WordColor.CheckR56((int) backColor.G, (int) backColor.B);
      case 57:
        return WordColor.CheckR57((int) backColor.G, (int) backColor.B);
      case 58:
        return WordColor.CheckR58((int) backColor.G, (int) backColor.B);
      case 59:
        return WordColor.CheckR59((int) backColor.G, (int) backColor.B);
      case 60:
        return WordColor.CheckR60((int) backColor.G, (int) backColor.B);
      case 61:
        return WordColor.CheckR61((int) backColor.G, (int) backColor.B);
      case 62:
        return WordColor.CheckR62((int) backColor.G, (int) backColor.B);
      case 63 /*0x3F*/:
        return WordColor.CheckR63((int) backColor.G, (int) backColor.B);
      case 64 /*0x40*/:
        return WordColor.CheckR64((int) backColor.G, (int) backColor.B);
      case 65:
        return WordColor.CheckR65((int) backColor.G, (int) backColor.B);
      case 66:
        return WordColor.CheckR66((int) backColor.G, (int) backColor.B);
      case 67:
        return WordColor.CheckR67((int) backColor.G, (int) backColor.B);
      case 68:
        return WordColor.CheckR68((int) backColor.G, (int) backColor.B);
      case 69:
        return WordColor.CheckR69((int) backColor.G, (int) backColor.B);
      case 70:
        return WordColor.CheckR70((int) backColor.G, (int) backColor.B);
      case 71:
        return WordColor.CheckR71((int) backColor.G, (int) backColor.B);
      case 72:
        return WordColor.CheckR72((int) backColor.G, (int) backColor.B);
      case 73:
        return WordColor.CheckR73((int) backColor.G, (int) backColor.B);
      case 74:
        return WordColor.CheckR74((int) backColor.G, (int) backColor.B);
      case 75:
        return WordColor.CheckR75((int) backColor.G, (int) backColor.B);
      case 76:
        return WordColor.CheckR76((int) backColor.G, (int) backColor.B);
      case 77:
        return WordColor.CheckR77((int) backColor.G, (int) backColor.B);
      case 78:
        return WordColor.CheckR78((int) backColor.G, (int) backColor.B);
      case 79:
        return WordColor.CheckR79((int) backColor.G, (int) backColor.B);
      case 80 /*0x50*/:
        return WordColor.CheckR80((int) backColor.G, (int) backColor.B);
      case 81:
        return WordColor.CheckR81((int) backColor.G, (int) backColor.B);
      case 82:
        return WordColor.CheckR82((int) backColor.G, (int) backColor.B);
      case 83:
        return WordColor.CheckR83((int) backColor.G, (int) backColor.B);
      case 84:
        return WordColor.CheckR84((int) backColor.G, (int) backColor.B);
      case 85:
        return WordColor.CheckR85((int) backColor.G, (int) backColor.B);
      case 86:
        return WordColor.CheckR86((int) backColor.G, (int) backColor.B);
      case 87:
        return WordColor.CheckR87((int) backColor.G, (int) backColor.B);
      case 88:
        return WordColor.CheckR88((int) backColor.G, (int) backColor.B);
      case 89:
        return WordColor.CheckR89((int) backColor.G, (int) backColor.B);
      case 90:
        return WordColor.CheckR90((int) backColor.G, (int) backColor.B);
      case 91:
        return WordColor.CheckR91((int) backColor.G, (int) backColor.B);
      case 92:
        return WordColor.CheckR92((int) backColor.G, (int) backColor.B);
      case 93:
        return WordColor.CheckR93((int) backColor.G, (int) backColor.B);
      case 94:
        return WordColor.CheckR94((int) backColor.G, (int) backColor.B);
      case 95:
        return WordColor.CheckR95((int) backColor.G, (int) backColor.B);
      case 96 /*0x60*/:
        return WordColor.CheckR96((int) backColor.G, (int) backColor.B);
      case 97:
        return WordColor.CheckR97((int) backColor.G, (int) backColor.B);
      case 98:
        return WordColor.CheckR98((int) backColor.G, (int) backColor.B);
      case 99:
        return WordColor.CheckR99((int) backColor.G, (int) backColor.B);
      case 100:
        return WordColor.CheckR100((int) backColor.G, (int) backColor.B);
      case 101:
        return WordColor.CheckR101((int) backColor.G, (int) backColor.B);
      case 102:
        return WordColor.CheckR102((int) backColor.G, (int) backColor.B);
      case 103:
        return WordColor.CheckR103((int) backColor.G, (int) backColor.B);
      case 104:
        return WordColor.CheckR104((int) backColor.G, (int) backColor.B);
      case 105:
        return WordColor.CheckR105((int) backColor.G, (int) backColor.B);
      case 106:
        return WordColor.CheckR106((int) backColor.G, (int) backColor.B);
      case 107:
        return WordColor.CheckR107((int) backColor.G, (int) backColor.B);
      case 108:
        return WordColor.CheckR108((int) backColor.G, (int) backColor.B);
      case 109:
        return WordColor.CheckR109((int) backColor.G, (int) backColor.B);
      case 110:
        return WordColor.CheckR110((int) backColor.G, (int) backColor.B);
      case 111:
        return WordColor.CheckR111((int) backColor.G, (int) backColor.B);
      case 112 /*0x70*/:
        return WordColor.CheckR112((int) backColor.G, (int) backColor.B);
      case 113:
        return WordColor.CheckR113((int) backColor.G, (int) backColor.B);
      case 114:
        return WordColor.CheckR114((int) backColor.G, (int) backColor.B);
      case 115:
        return WordColor.CheckR115((int) backColor.G, (int) backColor.B);
      case 116:
        return WordColor.CheckR116((int) backColor.G, (int) backColor.B);
      case 117:
        return WordColor.CheckR117((int) backColor.G, (int) backColor.B);
      case 118:
        return WordColor.CheckR118((int) backColor.G, (int) backColor.B);
      case 119:
        return WordColor.CheckR119((int) backColor.G, (int) backColor.B);
      case 120:
        return WordColor.CheckR120((int) backColor.G, (int) backColor.B);
      case 121:
        return WordColor.CheckR121((int) backColor.G, (int) backColor.B);
      case 122:
        return WordColor.CheckR122((int) backColor.G, (int) backColor.B);
      case 123:
        return WordColor.CheckR123((int) backColor.G, (int) backColor.B);
      case 124:
        return WordColor.CheckR124((int) backColor.G, (int) backColor.B);
      case 125:
        return WordColor.CheckR125((int) backColor.G, (int) backColor.B);
      case 126:
        return WordColor.CheckR126((int) backColor.G, (int) backColor.B);
      case 127 /*0x7F*/:
        return WordColor.CheckR127((int) backColor.G, (int) backColor.B);
      case 128 /*0x80*/:
        return WordColor.CheckR128((int) backColor.G, (int) backColor.B);
      case 129:
        return WordColor.CheckR129((int) backColor.G, (int) backColor.B);
      case 130:
        return WordColor.CheckR130((int) backColor.G, (int) backColor.B);
      case 131:
        return WordColor.CheckR131((int) backColor.G, (int) backColor.B);
      case 132:
        return WordColor.CheckR132((int) backColor.G, (int) backColor.B);
      case 133:
        return WordColor.CheckR133((int) backColor.G, (int) backColor.B);
      case 134:
        return WordColor.CheckR134((int) backColor.G, (int) backColor.B);
      case 135:
        return WordColor.CheckR135((int) backColor.G, (int) backColor.B);
      case 136:
        return WordColor.CheckR136((int) backColor.G, (int) backColor.B);
      case 137:
        return WordColor.CheckR137((int) backColor.G, (int) backColor.B);
      case 138:
        return WordColor.CheckR138((int) backColor.G, (int) backColor.B);
      case 139:
        return WordColor.CheckR139((int) backColor.G, (int) backColor.B);
      case 140:
        return WordColor.CheckR140((int) backColor.G, (int) backColor.B);
      case 141:
        return WordColor.CheckR141((int) backColor.G, (int) backColor.B);
      case 142:
        return WordColor.CheckR142((int) backColor.G, (int) backColor.B);
      case 143:
        return WordColor.CheckR143((int) backColor.G, (int) backColor.B);
      case 144 /*0x90*/:
        return WordColor.CheckR144((int) backColor.G, (int) backColor.B);
      case 145:
        return WordColor.CheckR145((int) backColor.G, (int) backColor.B);
      case 146:
        return WordColor.CheckR146((int) backColor.G, (int) backColor.B);
      case 147:
        return WordColor.CheckR147((int) backColor.G, (int) backColor.B);
      case 148:
        return WordColor.CheckR148((int) backColor.G, (int) backColor.B);
      case 149:
        return WordColor.CheckR149((int) backColor.G, (int) backColor.B);
      case 150:
        return WordColor.CheckR150((int) backColor.G, (int) backColor.B);
      case 151:
        return WordColor.CheckR151((int) backColor.G, (int) backColor.B);
      case 152:
        return WordColor.CheckR152((int) backColor.G, (int) backColor.B);
      case 153:
        return WordColor.CheckR153((int) backColor.G, (int) backColor.B);
      case 154:
        return WordColor.CheckR154((int) backColor.G, (int) backColor.B);
      case 155:
        return WordColor.CheckR155((int) backColor.G, (int) backColor.B);
      case 156:
        return WordColor.CheckR156((int) backColor.G, (int) backColor.B);
      case 157:
        return WordColor.CheckR157((int) backColor.G, (int) backColor.B);
      case 158:
        return WordColor.CheckR158((int) backColor.G, (int) backColor.B);
      case 159:
        return WordColor.CheckR159((int) backColor.G, (int) backColor.B);
      case 160 /*0xA0*/:
        return WordColor.CheckR160((int) backColor.G, (int) backColor.B);
      case 161:
        return WordColor.CheckR161((int) backColor.G, (int) backColor.B);
      case 162:
        return WordColor.CheckR162((int) backColor.G, (int) backColor.B);
      case 163:
        return WordColor.CheckR163((int) backColor.G, (int) backColor.B);
      case 164:
        return WordColor.CheckR164((int) backColor.G, (int) backColor.B);
      case 165:
        return WordColor.CheckR165((int) backColor.G, (int) backColor.B);
      case 166:
        return WordColor.CheckR166((int) backColor.G, (int) backColor.B);
      case 167:
        return WordColor.CheckR167((int) backColor.G, (int) backColor.B);
      case 168:
        return WordColor.CheckR168((int) backColor.G, (int) backColor.B);
      case 169:
        return WordColor.CheckR169((int) backColor.G, (int) backColor.B);
      case 170:
        return WordColor.CheckR170((int) backColor.G, (int) backColor.B);
      case 171:
        return WordColor.CheckR171((int) backColor.G, (int) backColor.B);
      case 172:
        return WordColor.CheckR172((int) backColor.G, (int) backColor.B);
      case 173:
        return WordColor.CheckR173((int) backColor.G, (int) backColor.B);
      case 174:
        return WordColor.CheckR174((int) backColor.G, (int) backColor.B);
      case 175:
        return WordColor.CheckR175((int) backColor.G, (int) backColor.B);
      case 176 /*0xB0*/:
        return WordColor.CheckR176((int) backColor.G, (int) backColor.B);
      case 177:
        return WordColor.CheckR177((int) backColor.G, (int) backColor.B);
      case 178:
        return WordColor.CheckR178((int) backColor.G, (int) backColor.B);
      case 179:
        return WordColor.CheckR179((int) backColor.G, (int) backColor.B);
      case 180:
        return WordColor.CheckR180((int) backColor.G, (int) backColor.B);
      case 181:
        return WordColor.CheckR181((int) backColor.G, (int) backColor.B);
      case 182:
        return WordColor.CheckR182((int) backColor.G, (int) backColor.B);
      case 183:
        return WordColor.CheckR183((int) backColor.G, (int) backColor.B);
      case 184:
        return WordColor.CheckR184((int) backColor.G, (int) backColor.B);
      case 185:
        return WordColor.CheckR185((int) backColor.G, (int) backColor.B);
      case 186:
        return WordColor.CheckR186((int) backColor.G, (int) backColor.B);
      case 187:
        return WordColor.CheckR187((int) backColor.G, (int) backColor.B);
      case 188:
        return WordColor.CheckR188((int) backColor.G, (int) backColor.B);
      case 189:
        return WordColor.CheckR189((int) backColor.G, (int) backColor.B);
      case 190:
        return WordColor.CheckR190((int) backColor.G, (int) backColor.B);
      case 191:
        return WordColor.CheckR191((int) backColor.G, (int) backColor.B);
      case 192 /*0xC0*/:
        return WordColor.CheckR192((int) backColor.G, (int) backColor.B);
      case 193:
        return WordColor.CheckR193((int) backColor.G, (int) backColor.B);
      case 194:
        return WordColor.CheckR194((int) backColor.G, (int) backColor.B);
      case 195:
        return WordColor.CheckR195((int) backColor.G, (int) backColor.B);
      case 196:
        return WordColor.CheckR196((int) backColor.G, (int) backColor.B);
      case 197:
        return WordColor.CheckR197((int) backColor.G, (int) backColor.B);
      case 198:
        return WordColor.CheckR198((int) backColor.G, (int) backColor.B);
      case 199:
        return WordColor.CheckR199((int) backColor.G, (int) backColor.B);
      case 200:
        return WordColor.CheckR200((int) backColor.G, (int) backColor.B);
      case 201:
        return WordColor.CheckR201((int) backColor.G, (int) backColor.B);
      case 202:
        return WordColor.CheckR202((int) backColor.G, (int) backColor.B);
      case 203:
        return WordColor.CheckR203((int) backColor.G, (int) backColor.B);
      default:
        return false;
    }
  }

  private static bool CheckR0(int g, int b)
  {
    switch (g)
    {
      case 54:
        return b >= (int) byte.MaxValue;
      case 55:
        return b >= 250;
      case 56:
        return b >= 245;
      case 57:
        return b >= 240 /*0xF0*/;
      case 58:
        return b >= 235;
      case 59:
        return b >= 230;
      case 60:
        return b >= 225;
      case 61:
        return b >= 219;
      case 62:
        return b >= 214;
      case 63 /*0x3F*/:
        return b >= 209;
      case 64 /*0x40*/:
        return b >= 204;
      case 65:
        return b >= 199;
      case 66:
        return b >= 194;
      case 67:
        return b >= 189;
      case 68:
        return b >= 183;
      case 69:
        return b >= 178;
      case 70:
        return b >= 173;
      case 71:
        return b >= 168;
      case 72:
        return b >= 163;
      case 73:
        return b >= 158;
      case 74:
        return b >= 152;
      case 75:
        return b >= 147;
      case 76:
        return b >= 142;
      case 77:
        return b >= 137;
      case 78:
        return b >= 132;
      case 79:
        return b >= (int) sbyte.MaxValue;
      case 80 /*0x50*/:
        return b >= 122;
      case 81:
        return b >= 116;
      case 82:
        return b >= 111;
      case 83:
        return b >= 106;
      case 84:
        return b >= 101;
      case 85:
        return b >= 96 /*0x60*/;
      case 86:
        return b >= 91;
      case 87:
        return b >= 86;
      case 88:
        return b >= 80 /*0x50*/;
      case 89:
        return b >= 75;
      case 90:
        return b >= 70;
      case 91:
        return b >= 65;
      case 92:
        return b >= 60;
      case 93:
        return b >= 55;
      case 94:
        return b >= 49;
      case 95:
        return b >= 44;
      case 96 /*0x60*/:
        return b >= 39;
      case 97:
        return b >= 34;
      case 98:
        return b >= 29;
      case 99:
        return b >= 24;
      case 100:
        return b >= 19;
      case 101:
        return b >= 13;
      case 102:
        return b >= 8;
      case 103:
        return b >= 3;
      default:
        return g >= 104;
    }
  }

  private static bool CheckR1(int g, int b)
  {
    switch (g)
    {
      case 54:
        return b >= 253;
      case 55:
        return b >= 248;
      case 56:
        return b >= 243;
      case 57:
        return b >= 237;
      case 58:
        return b >= 232;
      case 59:
        return b >= 227;
      case 60:
        return b >= 222;
      case 61:
        return b >= 217;
      case 62:
        return b >= 212;
      case 63 /*0x3F*/:
        return b >= 206;
      case 64 /*0x40*/:
        return b >= 201;
      case 65:
        return b >= 196;
      case 66:
        return b >= 191;
      case 67:
        return b >= 186;
      case 68:
        return b >= 181;
      case 69:
        return b >= 176 /*0xB0*/;
      case 70:
        return b >= 170;
      case 71:
        return b >= 165;
      case 72:
        return b >= 160 /*0xA0*/;
      case 73:
        return b >= 155;
      case 74:
        return b >= 150;
      case 75:
        return b >= 145;
      case 76:
        return b >= 140;
      case 77:
        return b >= 134;
      case 78:
        return b >= 129;
      case 79:
        return b >= 124;
      case 80 /*0x50*/:
        return b >= 119;
      case 81:
        return b >= 114;
      case 82:
        return b >= 109;
      case 83:
        return b >= 103;
      case 84:
        return b >= 98;
      case 85:
        return b >= 93;
      case 86:
        return b >= 88;
      case 87:
        return b >= 83;
      case 88:
        return b >= 78;
      case 89:
        return b >= 73;
      case 90:
        return b >= 67;
      case 91:
        return b >= 62;
      case 92:
        return b >= 57;
      case 93:
        return b >= 52;
      case 94:
        return b >= 47;
      case 95:
        return b >= 42;
      case 96 /*0x60*/:
        return b >= 37;
      case 97:
        return b >= 31 /*0x1F*/;
      case 98:
        return b >= 26;
      case 99:
        return b >= 21;
      case 100:
        return b >= 16 /*0x10*/;
      case 101:
        return b >= 11;
      case 102:
        return b >= 6;
      case 103:
        return b >= 1;
      default:
        return g >= 104;
    }
  }

  private static bool CheckR2(int g, int b)
  {
    switch (g)
    {
      case 53:
        return b >= (int) byte.MaxValue;
      case 54:
        return b >= 250;
      case 55:
        return b >= 245;
      case 56:
        return b >= 240 /*0xF0*/;
      case 57:
        return b >= 235;
      case 58:
        return b >= 230;
      case 59:
        return b >= 224 /*0xE0*/;
      case 60:
        return b >= 219;
      case 61:
        return b >= 214;
      case 62:
        return b >= 209;
      case 63 /*0x3F*/:
        return b >= 204;
      case 64 /*0x40*/:
        return b >= 199;
      case 65:
        return b >= 194;
      case 66:
        return b >= 188;
      case 67:
        return b >= 183;
      case 68:
        return b >= 178;
      case 69:
        return b >= 173;
      case 70:
        return b >= 168;
      case 71:
        return b >= 163;
      case 72:
        return b >= 158;
      case 73:
        return b >= 152;
      case 74:
        return b >= 147;
      case 75:
        return b >= 142;
      case 76:
        return b >= 137;
      case 77:
        return b >= 132;
      case 78:
        return b >= (int) sbyte.MaxValue;
      case 79:
        return b >= 121;
      case 80 /*0x50*/:
        return b >= 116;
      case 81:
        return b >= 111;
      case 82:
        return b >= 106;
      case 83:
        return b >= 101;
      case 84:
        return b >= 96 /*0x60*/;
      case 85:
        return b >= 91;
      case 86:
        return b >= 85;
      case 87:
        return b >= 80 /*0x50*/;
      case 88:
        return b >= 75;
      case 89:
        return b >= 70;
      case 90:
        return b >= 65;
      case 91:
        return b >= 60;
      case 92:
        return b >= 55;
      case 93:
        return b >= 49;
      case 94:
        return b >= 44;
      case 95:
        return b >= 39;
      case 96 /*0x60*/:
        return b >= 34;
      case 97:
        return b >= 29;
      case 98:
        return b >= 24;
      case 99:
        return b >= 18;
      case 100:
        return b >= 13;
      case 101:
        return b >= 8;
      case 102:
        return b >= 3;
      default:
        return g >= 103;
    }
  }

  private static bool CheckR3(int g, int b)
  {
    switch (g)
    {
      case 53:
        return b >= 253;
      case 54:
        return b >= 248;
      case 55:
        return b >= 242;
      case 56:
        return b >= 237;
      case 57:
        return b >= 232;
      case 58:
        return b >= 227;
      case 59:
        return b >= 222;
      case 60:
        return b >= 217;
      case 61:
        return b >= 212;
      case 62:
        return b >= 206;
      case 63 /*0x3F*/:
        return b >= 201;
      case 64 /*0x40*/:
        return b >= 196;
      case 65:
        return b >= 191;
      case 66:
        return b >= 186;
      case 67:
        return b >= 181;
      case 68:
        return b >= 175;
      case 69:
        return b >= 170;
      case 70:
        return b >= 165;
      case 71:
        return b >= 160 /*0xA0*/;
      case 72:
        return b >= 155;
      case 73:
        return b >= 150;
      case 74:
        return b >= 145;
      case 75:
        return b >= 139;
      case 76:
        return b >= 134;
      case 77:
        return b >= 129;
      case 78:
        return b >= 124;
      case 79:
        return b >= 119;
      case 80 /*0x50*/:
        return b >= 114;
      case 81:
        return b >= 109;
      case 82:
        return b >= 103;
      case 83:
        return b >= 98;
      case 84:
        return b >= 93;
      case 85:
        return b >= 88;
      case 86:
        return b >= 83;
      case 87:
        return b >= 78;
      case 88:
        return b >= 73;
      case 89:
        return b >= 67;
      case 90:
        return b >= 62;
      case 91:
        return b >= 57;
      case 92:
        return b >= 52;
      case 93:
        return b >= 47;
      case 94:
        return b >= 42;
      case 95:
        return b >= 36;
      case 96 /*0x60*/:
        return b >= 31 /*0x1F*/;
      case 97:
        return b >= 26;
      case 98:
        return b >= 21;
      case 99:
        return b >= 16 /*0x10*/;
      case 100:
        return b >= 11;
      case 101:
        return b >= 6;
      default:
        return g >= 102;
    }
  }

  private static bool CheckR4(int g, int b)
  {
    switch (g)
    {
      case 52:
        return b >= (int) byte.MaxValue;
      case 53:
        return b >= 250;
      case 54:
        return b >= 245;
      case 55:
        return b >= 240 /*0xF0*/;
      case 56:
        return b >= 235;
      case 57:
        return b >= 230;
      case 58:
        return b >= 224 /*0xE0*/;
      case 59:
        return b >= 219;
      case 60:
        return b >= 214;
      case 61:
        return b >= 209;
      case 62:
        return b >= 204;
      case 63 /*0x3F*/:
        return b >= 199;
      case 64 /*0x40*/:
        return b >= 193;
      case 65:
        return b >= 188;
      case 66:
        return b >= 183;
      case 67:
        return b >= 178;
      case 68:
        return b >= 173;
      case 69:
        return b >= 168;
      case 70:
        return b >= 163;
      case 71:
        return b >= 157;
      case 72:
        return b >= 152;
      case 73:
        return b >= 147;
      case 74:
        return b >= 142;
      case 75:
        return b >= 137;
      case 76:
        return b >= 132;
      case 77:
        return b >= (int) sbyte.MaxValue;
      case 78:
        return b >= 121;
      case 79:
        return b >= 116;
      case 80 /*0x50*/:
        return b >= 111;
      case 81:
        return b >= 106;
      case 82:
        return b >= 101;
      case 83:
        return b >= 96 /*0x60*/;
      case 84:
        return b >= 90;
      case 85:
        return b >= 85;
      case 86:
        return b >= 80 /*0x50*/;
      case 87:
        return b >= 75;
      case 88:
        return b >= 70;
      case 89:
        return b >= 65;
      case 90:
        return b >= 60;
      case 91:
        return b >= 54;
      case 92:
        return b >= 49;
      case 93:
        return b >= 44;
      case 94:
        return b >= 39;
      case 95:
        return b >= 34;
      case 96 /*0x60*/:
        return b >= 29;
      case 97:
        return b >= 24;
      case 98:
        return b >= 18;
      case 99:
        return b >= 13;
      case 100:
        return b >= 8;
      case 101:
        return b >= 3;
      default:
        return g >= 102;
    }
  }

  private static bool CheckR5(int g, int b)
  {
    switch (g)
    {
      case 52:
        return b >= 253;
      case 53:
        return b >= 247;
      case 54:
        return b >= 242;
      case 55:
        return b >= 237;
      case 56:
        return b >= 232;
      case 57:
        return b >= 227;
      case 58:
        return b >= 222;
      case 59:
        return b >= 217;
      case 60:
        return b >= 211;
      case 61:
        return b >= 206;
      case 62:
        return b >= 201;
      case 63 /*0x3F*/:
        return b >= 196;
      case 64 /*0x40*/:
        return b >= 191;
      case 65:
        return b >= 186;
      case 66:
        return b >= 181;
      case 67:
        return b >= 175;
      case 68:
        return b >= 170;
      case 69:
        return b >= 165;
      case 70:
        return b >= 160 /*0xA0*/;
      case 71:
        return b >= 155;
      case 72:
        return b >= 150;
      case 73:
        return b >= 144 /*0x90*/;
      case 74:
        return b >= 139;
      case 75:
        return b >= 134;
      case 76:
        return b >= 129;
      case 77:
        return b >= 124;
      case 78:
        return b >= 119;
      case 79:
        return b >= 114;
      case 80 /*0x50*/:
        return b >= 108;
      case 81:
        return b >= 103;
      case 82:
        return b >= 98;
      case 83:
        return b >= 93;
      case 84:
        return b >= 88;
      case 85:
        return b >= 83;
      case 86:
        return b >= 78;
      case 87:
        return b >= 72;
      case 88:
        return b >= 67;
      case 89:
        return b >= 62;
      case 90:
        return b >= 57;
      case 91:
        return b >= 52;
      case 92:
        return b >= 47;
      case 93:
        return b >= 42;
      case 94:
        return b >= 36;
      case 95:
        return b >= 31 /*0x1F*/;
      case 96 /*0x60*/:
        return b >= 26;
      case 97:
        return b >= 21;
      case 98:
        return b >= 16 /*0x10*/;
      case 99:
        return b >= 11;
      case 100:
        return b >= 5;
      default:
        return g >= 101;
    }
  }

  private static bool CheckR6(int g, int b)
  {
    switch (g)
    {
      case 51:
        return b >= (int) byte.MaxValue;
      case 52:
        return b >= 250;
      case 53:
        return b >= 245;
      case 54:
        return b >= 240 /*0xF0*/;
      case 55:
        return b >= 235;
      case 56:
        return b >= 229;
      case 57:
        return b >= 224 /*0xE0*/;
      case 58:
        return b >= 219;
      case 59:
        return b >= 214;
      case 60:
        return b >= 209;
      case 61:
        return b >= 204;
      case 62:
        return b >= 199;
      case 63 /*0x3F*/:
        return b >= 193;
      case 64 /*0x40*/:
        return b >= 188;
      case 65:
        return b >= 183;
      case 66:
        return b >= 178;
      case 67:
        return b >= 173;
      case 68:
        return b >= 168;
      case 69:
        return b >= 162;
      case 70:
        return b >= 157;
      case 71:
        return b >= 152;
      case 72:
        return b >= 147;
      case 73:
        return b >= 142;
      case 74:
        return b >= 137;
      case 75:
        return b >= 132;
      case 76:
        return b >= 126;
      case 77:
        return b >= 121;
      case 78:
        return b >= 116;
      case 79:
        return b >= 111;
      case 80 /*0x50*/:
        return b >= 106;
      case 81:
        return b >= 101;
      case 82:
        return b >= 96 /*0x60*/;
      case 83:
        return b >= 90;
      case 84:
        return b >= 85;
      case 85:
        return b >= 80 /*0x50*/;
      case 86:
        return b >= 75;
      case 87:
        return b >= 70;
      case 88:
        return b >= 65;
      case 89:
        return b >= 59;
      case 90:
        return b >= 54;
      case 91:
        return b >= 49;
      case 92:
        return b >= 44;
      case 93:
        return b >= 39;
      case 94:
        return b >= 34;
      case 95:
        return b >= 29;
      case 96 /*0x60*/:
        return b >= 23;
      case 97:
        return b >= 18;
      case 98:
        return b >= 13;
      case 99:
        return b >= 8;
      case 100:
        return b >= 3;
      default:
        return g >= 101;
    }
  }

  private static bool CheckR7(int g, int b)
  {
    switch (g)
    {
      case 51:
        return b >= 253;
      case 52:
        return b >= 247;
      case 53:
        return b >= 242;
      case 54:
        return b >= 237;
      case 55:
        return b >= 232;
      case 56:
        return b >= 227;
      case 57:
        return b >= 222;
      case 58:
        return b >= 216;
      case 59:
        return b >= 211;
      case 60:
        return b >= 206;
      case 61:
        return b >= 201;
      case 62:
        return b >= 196;
      case 63 /*0x3F*/:
        return b >= 191;
      case 64 /*0x40*/:
        return b >= 186;
      case 65:
        return b >= 180;
      case 66:
        return b >= 175;
      case 67:
        return b >= 170;
      case 68:
        return b >= 165;
      case 69:
        return b >= 160 /*0xA0*/;
      case 70:
        return b >= 155;
      case 71:
        return b >= 150;
      case 72:
        return b >= 144 /*0x90*/;
      case 73:
        return b >= 139;
      case 74:
        return b >= 134;
      case 75:
        return b >= 129;
      case 76:
        return b >= 124;
      case 77:
        return b >= 119;
      case 78:
        return b >= 114;
      case 79:
        return b >= 108;
      case 80 /*0x50*/:
        return b >= 103;
      case 81:
        return b >= 98;
      case 82:
        return b >= 93;
      case 83:
        return b >= 88;
      case 84:
        return b >= 83;
      case 85:
        return b >= 77;
      case 86:
        return b >= 72;
      case 87:
        return b >= 67;
      case 88:
        return b >= 62;
      case 89:
        return b >= 57;
      case 90:
        return b >= 52;
      case 91:
        return b >= 47;
      case 92:
        return b >= 41;
      case 93:
        return b >= 36;
      case 94:
        return b >= 31 /*0x1F*/;
      case 95:
        return b >= 26;
      case 96 /*0x60*/:
        return b >= 21;
      case 97:
        return b >= 16 /*0x10*/;
      case 98:
        return b >= 11;
      case 99:
        return b >= 5;
      default:
        return g >= 100;
    }
  }

  private static bool CheckR8(int g, int b)
  {
    switch (g)
    {
      case 50:
        return b >= (int) byte.MaxValue;
      case 51:
        return b >= 250;
      case 52:
        return b >= 245;
      case 53:
        return b >= 240 /*0xF0*/;
      case 54:
        return b >= 234;
      case 55:
        return b >= 229;
      case 56:
        return b >= 224 /*0xE0*/;
      case 57:
        return b >= 219;
      case 58:
        return b >= 214;
      case 59:
        return b >= 209;
      case 60:
        return b >= 204;
      case 61:
        return b >= 198;
      case 62:
        return b >= 193;
      case 63 /*0x3F*/:
        return b >= 188;
      case 64 /*0x40*/:
        return b >= 183;
      case 65:
        return b >= 178;
      case 66:
        return b >= 173;
      case 67:
        return b >= 168;
      case 68:
        return b >= 162;
      case 69:
        return b >= 157;
      case 70:
        return b >= 152;
      case 71:
        return b >= 147;
      case 72:
        return b >= 142;
      case 73:
        return b >= 137;
      case 74:
        return b >= 131;
      case 75:
        return b >= 126;
      case 76:
        return b >= 121;
      case 77:
        return b >= 116;
      case 78:
        return b >= 111;
      case 79:
        return b >= 106;
      case 80 /*0x50*/:
        return b >= 101;
      case 81:
        return b >= 95;
      case 82:
        return b >= 90;
      case 83:
        return b >= 85;
      case 84:
        return b >= 80 /*0x50*/;
      case 85:
        return b >= 75;
      case 86:
        return b >= 70;
      case 87:
        return b >= 65;
      case 88:
        return b >= 59;
      case 89:
        return b >= 54;
      case 90:
        return b >= 49;
      case 91:
        return b >= 44;
      case 92:
        return b >= 39;
      case 93:
        return b >= 34;
      case 94:
        return b >= 28;
      case 95:
        return b >= 23;
      case 96 /*0x60*/:
        return b >= 18;
      case 97:
        return b >= 13;
      case 98:
        return b >= 8;
      case 99:
        return b >= 3;
      default:
        return g >= 100;
    }
  }

  private static bool CheckR9(int g, int b)
  {
    switch (g)
    {
      case 50:
        return b >= 252;
      case 51:
        return b >= 247;
      case 52:
        return b >= 242;
      case 53:
        return b >= 237;
      case 54:
        return b >= 232;
      case 55:
        return b >= 227;
      case 56:
        return b >= 222;
      case 57:
        return b >= 216;
      case 58:
        return b >= 211;
      case 59:
        return b >= 206;
      case 60:
        return b >= 201;
      case 61:
        return b >= 196;
      case 62:
        return b >= 191;
      case 63 /*0x3F*/:
        return b >= 185;
      case 64 /*0x40*/:
        return b >= 180;
      case 65:
        return b >= 175;
      case 66:
        return b >= 170;
      case 67:
        return b >= 165;
      case 68:
        return b >= 160 /*0xA0*/;
      case 69:
        return b >= 155;
      case 70:
        return b >= 149;
      case 71:
        return b >= 144 /*0x90*/;
      case 72:
        return b >= 139;
      case 73:
        return b >= 134;
      case 74:
        return b >= 129;
      case 75:
        return b >= 124;
      case 76:
        return b >= 119;
      case 77:
        return b >= 113;
      case 78:
        return b >= 108;
      case 79:
        return b >= 103;
      case 80 /*0x50*/:
        return b >= 98;
      case 81:
        return b >= 93;
      case 82:
        return b >= 88;
      case 83:
        return b >= 83;
      case 84:
        return b >= 77;
      case 85:
        return b >= 72;
      case 86:
        return b >= 67;
      case 87:
        return b >= 62;
      case 88:
        return b >= 57;
      case 89:
        return b >= 52;
      case 90:
        return b >= 46;
      case 91:
        return b >= 41;
      case 92:
        return b >= 36;
      case 93:
        return b >= 31 /*0x1F*/;
      case 94:
        return b >= 26;
      case 95:
        return b >= 21;
      case 96 /*0x60*/:
        return b >= 16 /*0x10*/;
      case 97:
        return b >= 10;
      case 98:
        return b >= 5;
      default:
        return g >= 99;
    }
  }

  private static bool CheckR10(int g, int b)
  {
    switch (g)
    {
      case 49:
        return b >= (int) byte.MaxValue;
      case 50:
        return b >= 250;
      case 51:
        return b >= 245;
      case 52:
        return b >= 240 /*0xF0*/;
      case 53:
        return b >= 234;
      case 54:
        return b >= 229;
      case 55:
        return b >= 224 /*0xE0*/;
      case 56:
        return b >= 219;
      case 57:
        return b >= 214;
      case 58:
        return b >= 209;
      case 59:
        return b >= 203;
      case 60:
        return b >= 198;
      case 61:
        return b >= 193;
      case 62:
        return b >= 188;
      case 63 /*0x3F*/:
        return b >= 183;
      case 64 /*0x40*/:
        return b >= 178;
      case 65:
        return b >= 173;
      case 66:
        return b >= 167;
      case 67:
        return b >= 162;
      case 68:
        return b >= 157;
      case 69:
        return b >= 152;
      case 70:
        return b >= 147;
      case 71:
        return b >= 142;
      case 72:
        return b >= 137;
      case 73:
        return b >= 131;
      case 74:
        return b >= 126;
      case 75:
        return b >= 121;
      case 76:
        return b >= 116;
      case 77:
        return b >= 111;
      case 78:
        return b >= 106;
      case 79:
        return b >= 100;
      case 80 /*0x50*/:
        return b >= 95;
      case 81:
        return b >= 90;
      case 82:
        return b >= 85;
      case 83:
        return b >= 80 /*0x50*/;
      case 84:
        return b >= 75;
      case 85:
        return b >= 70;
      case 86:
        return b >= 64 /*0x40*/;
      case 87:
        return b >= 59;
      case 88:
        return b >= 54;
      case 89:
        return b >= 49;
      case 90:
        return b >= 44;
      case 91:
        return b >= 39;
      case 92:
        return b >= 34;
      case 93:
        return b >= 28;
      case 94:
        return b >= 23;
      case 95:
        return b >= 18;
      case 96 /*0x60*/:
        return b >= 13;
      case 97:
        return b >= 8;
      case 98:
        return b >= 3;
      default:
        return g >= 99;
    }
  }

  private static bool CheckR11(int g, int b)
  {
    switch (g)
    {
      case 49:
        return b >= 252;
      case 50:
        return b >= 247;
      case 51:
        return b >= 242;
      case 52:
        return b >= 237;
      case 53:
        return b >= 232;
      case 54:
        return b >= 227;
      case 55:
        return b >= 221;
      case 56:
        return b >= 216;
      case 57:
        return b >= 211;
      case 58:
        return b >= 206;
      case 59:
        return b >= 201;
      case 60:
        return b >= 196;
      case 61:
        return b >= 191;
      case 62:
        return b >= 185;
      case 63 /*0x3F*/:
        return b >= 180;
      case 64 /*0x40*/:
        return b >= 175;
      case 65:
        return b >= 170;
      case 66:
        return b >= 165;
      case 67:
        return b >= 160 /*0xA0*/;
      case 68:
        return b >= 155;
      case 69:
        return b >= 149;
      case 70:
        return b >= 144 /*0x90*/;
      case 71:
        return b >= 139;
      case 72:
        return b >= 134;
      case 73:
        return b >= 129;
      case 74:
        return b >= 124;
      case 75:
        return b >= 118;
      case 76:
        return b >= 113;
      case 77:
        return b >= 108;
      case 78:
        return b >= 103;
      case 79:
        return b >= 98;
      case 80 /*0x50*/:
        return b >= 93;
      case 81:
        return b >= 88;
      case 82:
        return b >= 82;
      case 83:
        return b >= 77;
      case 84:
        return b >= 72;
      case 85:
        return b >= 67;
      case 86:
        return b >= 62;
      case 87:
        return b >= 57;
      case 88:
        return b >= 52;
      case 89:
        return b >= 46;
      case 90:
        return b >= 41;
      case 91:
        return b >= 36;
      case 92:
        return b >= 31 /*0x1F*/;
      case 93:
        return b >= 26;
      case 94:
        return b >= 21;
      case 95:
        return b >= 15;
      case 96 /*0x60*/:
        return b >= 10;
      case 97:
        return b >= 5;
      default:
        return g >= 98;
    }
  }

  private static bool CheckR12(int g, int b)
  {
    switch (g)
    {
      case 48 /*0x30*/:
        return b >= (int) byte.MaxValue;
      case 49:
        return b >= 250;
      case 50:
        return b >= 245;
      case 51:
        return b >= 239;
      case 52:
        return b >= 234;
      case 53:
        return b >= 229;
      case 54:
        return b >= 224 /*0xE0*/;
      case 55:
        return b >= 219;
      case 56:
        return b >= 214;
      case 57:
        return b >= 209;
      case 58:
        return b >= 203;
      case 59:
        return b >= 198;
      case 60:
        return b >= 193;
      case 61:
        return b >= 188;
      case 62:
        return b >= 183;
      case 63 /*0x3F*/:
        return b >= 178;
      case 64 /*0x40*/:
        return b >= 172;
      case 65:
        return b >= 167;
      case 66:
        return b >= 162;
      case 67:
        return b >= 157;
      case 68:
        return b >= 152;
      case 69:
        return b >= 147;
      case 70:
        return b >= 142;
      case 71:
        return b >= 136;
      case 72:
        return b >= 131;
      case 73:
        return b >= 126;
      case 74:
        return b >= 121;
      case 75:
        return b >= 116;
      case 76:
        return b >= 111;
      case 77:
        return b >= 106;
      case 78:
        return b >= 100;
      case 79:
        return b >= 95;
      case 80 /*0x50*/:
        return b >= 90;
      case 81:
        return b >= 85;
      case 82:
        return b >= 80 /*0x50*/;
      case 83:
        return b >= 75;
      case 84:
        return b >= 69;
      case 85:
        return b >= 64 /*0x40*/;
      case 86:
        return b >= 59;
      case 87:
        return b >= 54;
      case 88:
        return b >= 49;
      case 89:
        return b >= 44;
      case 90:
        return b >= 39;
      case 91:
        return b >= 33;
      case 92:
        return b >= 28;
      case 93:
        return b >= 23;
      case 94:
        return b >= 18;
      case 95:
        return b >= 13;
      case 96 /*0x60*/:
        return b >= 8;
      case 97:
        return b >= 3;
      default:
        return g >= 98;
    }
  }

  private static bool CheckR13(int g, int b)
  {
    switch (g)
    {
      case 48 /*0x30*/:
        return b >= 252;
      case 49:
        return b >= 247;
      case 50:
        return b >= 242;
      case 51:
        return b >= 237;
      case 52:
        return b >= 232;
      case 53:
        return b >= 226;
      case 54:
        return b >= 221;
      case 55:
        return b >= 216;
      case 56:
        return b >= 211;
      case 57:
        return b >= 206;
      case 58:
        return b >= 201;
      case 59:
        return b >= 196;
      case 60:
        return b >= 190;
      case 61:
        return b >= 185;
      case 62:
        return b >= 180;
      case 63 /*0x3F*/:
        return b >= 175;
      case 64 /*0x40*/:
        return b >= 170;
      case 65:
        return b >= 165;
      case 66:
        return b >= 160 /*0xA0*/;
      case 67:
        return b >= 154;
      case 68:
        return b >= 149;
      case 69:
        return b >= 144 /*0x90*/;
      case 70:
        return b >= 139;
      case 71:
        return b >= 134;
      case 72:
        return b >= 129;
      case 73:
        return b >= 124;
      case 74:
        return b >= 118;
      case 75:
        return b >= 113;
      case 76:
        return b >= 108;
      case 77:
        return b >= 103;
      case 78:
        return b >= 98;
      case 79:
        return b >= 93;
      case 80 /*0x50*/:
        return b >= 87;
      case 81:
        return b >= 82;
      case 82:
        return b >= 77;
      case 83:
        return b >= 72;
      case 84:
        return b >= 67;
      case 85:
        return b >= 62;
      case 86:
        return b >= 57;
      case 87:
        return b >= 51;
      case 88:
        return b >= 46;
      case 89:
        return b >= 41;
      case 90:
        return b >= 36;
      case 91:
        return b >= 31 /*0x1F*/;
      case 92:
        return b >= 26;
      case 93:
        return b >= 21;
      case 94:
        return b >= 15;
      case 95:
        return b >= 10;
      case 96 /*0x60*/:
        return b >= 5;
      default:
        return g >= 97;
    }
  }

  private static bool CheckR14(int g, int b)
  {
    switch (g)
    {
      case 47:
        return b >= (int) byte.MaxValue;
      case 48 /*0x30*/:
        return b >= 250;
      case 49:
        return b >= 244;
      case 50:
        return b >= 239;
      case 51:
        return b >= 234;
      case 52:
        return b >= 229;
      case 53:
        return b >= 224 /*0xE0*/;
      case 54:
        return b >= 219;
      case 55:
        return b >= 214;
      case 56:
        return b >= 208 /*0xD0*/;
      case 57:
        return b >= 203;
      case 58:
        return b >= 198;
      case 59:
        return b >= 193;
      case 60:
        return b >= 188;
      case 61:
        return b >= 183;
      case 62:
        return b >= 178;
      case 63 /*0x3F*/:
        return b >= 172;
      case 64 /*0x40*/:
        return b >= 167;
      case 65:
        return b >= 162;
      case 66:
        return b >= 157;
      case 67:
        return b >= 152;
      case 68:
        return b >= 147;
      case 69:
        return b >= 141;
      case 70:
        return b >= 136;
      case 71:
        return b >= 131;
      case 72:
        return b >= 126;
      case 73:
        return b >= 121;
      case 74:
        return b >= 116;
      case 75:
        return b >= 111;
      case 76:
        return b >= 105;
      case 77:
        return b >= 100;
      case 78:
        return b >= 95;
      case 79:
        return b >= 90;
      case 80 /*0x50*/:
        return b >= 85;
      case 81:
        return b >= 80 /*0x50*/;
      case 82:
        return b >= 75;
      case 83:
        return b >= 69;
      case 84:
        return b >= 64 /*0x40*/;
      case 85:
        return b >= 59;
      case 86:
        return b >= 54;
      case 87:
        return b >= 49;
      case 88:
        return b >= 44;
      case 89:
        return b >= 39;
      case 90:
        return b >= 33;
      case 91:
        return b >= 28;
      case 92:
        return b >= 23;
      case 93:
        return b >= 18;
      case 94:
        return b >= 13;
      case 95:
        return b >= 8;
      case 96 /*0x60*/:
        return b >= 2;
      default:
        return g >= 97;
    }
  }

  private static bool CheckR15(int g, int b)
  {
    switch (g)
    {
      case 47:
        return b >= 252;
      case 48 /*0x30*/:
        return b >= 247;
      case 49:
        return b >= 242;
      case 50:
        return b >= 237;
      case 51:
        return b >= 232;
      case 52:
        return b >= 226;
      case 53:
        return b >= 221;
      case 54:
        return b >= 216;
      case 55:
        return b >= 211;
      case 56:
        return b >= 206;
      case 57:
        return b >= 201;
      case 58:
        return b >= 196;
      case 59:
        return b >= 190;
      case 60:
        return b >= 185;
      case 61:
        return b >= 180;
      case 62:
        return b >= 175;
      case 63 /*0x3F*/:
        return b >= 170;
      case 64 /*0x40*/:
        return b >= 165;
      case 65:
        return b >= 159;
      case 66:
        return b >= 154;
      case 67:
        return b >= 149;
      case 68:
        return b >= 144 /*0x90*/;
      case 69:
        return b >= 139;
      case 70:
        return b >= 134;
      case 71:
        return b >= 129;
      case 72:
        return b >= 123;
      case 73:
        return b >= 118;
      case 74:
        return b >= 113;
      case 75:
        return b >= 108;
      case 76:
        return b >= 103;
      case 77:
        return b >= 98;
      case 78:
        return b >= 93;
      case 79:
        return b >= 87;
      case 80 /*0x50*/:
        return b >= 82;
      case 81:
        return b >= 77;
      case 82:
        return b >= 72;
      case 83:
        return b >= 67;
      case 84:
        return b >= 62;
      case 85:
        return b >= 56;
      case 86:
        return b >= 51;
      case 87:
        return b >= 46;
      case 88:
        return b >= 41;
      case 89:
        return b >= 36;
      case 90:
        return b >= 31 /*0x1F*/;
      case 91:
        return b >= 26;
      case 92:
        return b >= 20;
      case 93:
        return b >= 15;
      case 94:
        return b >= 10;
      case 95:
        return b >= 5;
      default:
        return g >= 96 /*0x60*/;
    }
  }

  private static bool CheckR16(int g, int b)
  {
    switch (g)
    {
      case 46:
        return b >= (int) byte.MaxValue;
      case 47:
        return b >= 250;
      case 48 /*0x30*/:
        return b >= 244;
      case 49:
        return b >= 239;
      case 50:
        return b >= 234;
      case 51:
        return b >= 229;
      case 52:
        return b >= 224 /*0xE0*/;
      case 53:
        return b >= 219;
      case 54:
        return b >= 213;
      case 55:
        return b >= 208 /*0xD0*/;
      case 56:
        return b >= 203;
      case 57:
        return b >= 198;
      case 58:
        return b >= 193;
      case 59:
        return b >= 188;
      case 60:
        return b >= 183;
      case 61:
        return b >= 177;
      case 62:
        return b >= 172;
      case 63 /*0x3F*/:
        return b >= 167;
      case 64 /*0x40*/:
        return b >= 162;
      case 65:
        return b >= 157;
      case 66:
        return b >= 152;
      case 67:
        return b >= 147;
      case 68:
        return b >= 141;
      case 69:
        return b >= 136;
      case 70:
        return b >= 131;
      case 71:
        return b >= 126;
      case 72:
        return b >= 121;
      case 73:
        return b >= 116;
      case 74:
        return b >= 110;
      case 75:
        return b >= 105;
      case 76:
        return b >= 100;
      case 77:
        return b >= 95;
      case 78:
        return b >= 90;
      case 79:
        return b >= 85;
      case 80 /*0x50*/:
        return b >= 80 /*0x50*/;
      case 81:
        return b >= 74;
      case 82:
        return b >= 69;
      case 83:
        return b >= 64 /*0x40*/;
      case 84:
        return b >= 59;
      case 85:
        return b >= 54;
      case 86:
        return b >= 49;
      case 87:
        return b >= 44;
      case 88:
        return b >= 38;
      case 89:
        return b >= 33;
      case 90:
        return b >= 28;
      case 91:
        return b >= 23;
      case 92:
        return b >= 18;
      case 93:
        return b >= 13;
      case 94:
        return b >= 8;
      case 95:
        return b >= 2;
      default:
        return g >= 96 /*0x60*/;
    }
  }

  private static bool CheckR17(int g, int b)
  {
    switch (g)
    {
      case 46:
        return b >= 252;
      case 47:
        return b >= 247;
      case 48 /*0x30*/:
        return b >= 242;
      case 49:
        return b >= 237;
      case 50:
        return b >= 231;
      case 51:
        return b >= 226;
      case 52:
        return b >= 221;
      case 53:
        return b >= 216;
      case 54:
        return b >= 211;
      case 55:
        return b >= 206;
      case 56:
        return b >= 201;
      case 57:
        return b >= 195;
      case 58:
        return b >= 190;
      case 59:
        return b >= 185;
      case 60:
        return b >= 180;
      case 61:
        return b >= 175;
      case 62:
        return b >= 170;
      case 63 /*0x3F*/:
        return b >= 165;
      case 64 /*0x40*/:
        return b >= 159;
      case 65:
        return b >= 154;
      case 66:
        return b >= 149;
      case 67:
        return b >= 144 /*0x90*/;
      case 68:
        return b >= 139;
      case 69:
        return b >= 134;
      case 70:
        return b >= 128 /*0x80*/;
      case 71:
        return b >= 123;
      case 72:
        return b >= 118;
      case 73:
        return b >= 113;
      case 74:
        return b >= 108;
      case 75:
        return b >= 103;
      case 76:
        return b >= 98;
      case 77:
        return b >= 92;
      case 78:
        return b >= 87;
      case 79:
        return b >= 82;
      case 80 /*0x50*/:
        return b >= 77;
      case 81:
        return b >= 72;
      case 82:
        return b >= 67;
      case 83:
        return b >= 62;
      case 84:
        return b >= 56;
      case 85:
        return b >= 51;
      case 86:
        return b >= 46;
      case 87:
        return b >= 41;
      case 88:
        return b >= 36;
      case 89:
        return b >= 31 /*0x1F*/;
      case 90:
        return b >= 25;
      case 91:
        return b >= 20;
      case 92:
        return b >= 15;
      case 93:
        return b >= 10;
      case 94:
        return b >= 5;
      default:
        return g >= 95;
    }
  }

  private static bool CheckR18(int g, int b)
  {
    switch (g)
    {
      case 45:
        return b >= (int) byte.MaxValue;
      case 46:
        return b >= 249;
      case 47:
        return b >= 244;
      case 48 /*0x30*/:
        return b >= 239;
      case 49:
        return b >= 234;
      case 50:
        return b >= 229;
      case 51:
        return b >= 224 /*0xE0*/;
      case 52:
        return b >= 219;
      case 53:
        return b >= 213;
      case 54:
        return b >= 208 /*0xD0*/;
      case 55:
        return b >= 203;
      case 56:
        return b >= 198;
      case 57:
        return b >= 193;
      case 58:
        return b >= 188;
      case 59:
        return b >= 182;
      case 60:
        return b >= 177;
      case 61:
        return b >= 172;
      case 62:
        return b >= 167;
      case 63 /*0x3F*/:
        return b >= 162;
      case 64 /*0x40*/:
        return b >= 157;
      case 65:
        return b >= 152;
      case 66:
        return b >= 146;
      case 67:
        return b >= 141;
      case 68:
        return b >= 136;
      case 69:
        return b >= 131;
      case 70:
        return b >= 126;
      case 71:
        return b >= 121;
      case 72:
        return b >= 116;
      case 73:
        return b >= 110;
      case 74:
        return b >= 105;
      case 75:
        return b >= 100;
      case 76:
        return b >= 95;
      case 77:
        return b >= 90;
      case 78:
        return b >= 85;
      case 79:
        return b >= 80 /*0x50*/;
      case 80 /*0x50*/:
        return b >= 74;
      case 81:
        return b >= 69;
      case 82:
        return b >= 64 /*0x40*/;
      case 83:
        return b >= 59;
      case 84:
        return b >= 54;
      case 85:
        return b >= 49;
      case 86:
        return b >= 43;
      case 87:
        return b >= 38;
      case 88:
        return b >= 33;
      case 89:
        return b >= 28;
      case 90:
        return b >= 23;
      case 91:
        return b >= 18;
      case 92:
        return b >= 13;
      case 93:
        return b >= 7;
      case 94:
        return b >= 2;
      default:
        return g >= 95;
    }
  }

  private static bool CheckR19(int g, int b)
  {
    switch (g)
    {
      case 45:
        return b >= 252;
      case 46:
        return b >= 247;
      case 47:
        return b >= 242;
      case 48 /*0x30*/:
        return b >= 237;
      case 49:
        return b >= 231;
      case 50:
        return b >= 226;
      case 51:
        return b >= 221;
      case 52:
        return b >= 216;
      case 53:
        return b >= 211;
      case 54:
        return b >= 206;
      case 55:
        return b >= 200;
      case 56:
        return b >= 195;
      case 57:
        return b >= 190;
      case 58:
        return b >= 185;
      case 59:
        return b >= 180;
      case 60:
        return b >= 175;
      case 61:
        return b >= 170;
      case 62:
        return b >= 164;
      case 63 /*0x3F*/:
        return b >= 159;
      case 64 /*0x40*/:
        return b >= 154;
      case 65:
        return b >= 149;
      case 66:
        return b >= 144 /*0x90*/;
      case 67:
        return b >= 139;
      case 68:
        return b >= 134;
      case 69:
        return b >= 128 /*0x80*/;
      case 70:
        return b >= 123;
      case 71:
        return b >= 118;
      case 72:
        return b >= 113;
      case 73:
        return b >= 108;
      case 74:
        return b >= 103;
      case 75:
        return b >= 97;
      case 76:
        return b >= 92;
      case 77:
        return b >= 87;
      case 78:
        return b >= 82;
      case 79:
        return b >= 77;
      case 80 /*0x50*/:
        return b >= 72;
      case 81:
        return b >= 67;
      case 82:
        return b >= 61;
      case 83:
        return b >= 56;
      case 84:
        return b >= 51;
      case 85:
        return b >= 46;
      case 86:
        return b >= 41;
      case 87:
        return b >= 36;
      case 88:
        return b >= 31 /*0x1F*/;
      case 89:
        return b >= 25;
      case 90:
        return b >= 20;
      case 91:
        return b >= 15;
      case 92:
        return b >= 10;
      case 93:
        return b >= 5;
      default:
        return g >= 94;
    }
  }

  private static bool CheckR20(int g, int b)
  {
    switch (g)
    {
      case 44:
        return b >= 254;
      case 45:
        return b >= 249;
      case 46:
        return b >= 244;
      case 47:
        return b >= 239;
      case 48 /*0x30*/:
        return b >= 234;
      case 49:
        return b >= 229;
      case 50:
        return b >= 224 /*0xE0*/;
      case 51:
        return b >= 218;
      case 52:
        return b >= 213;
      case 53:
        return b >= 208 /*0xD0*/;
      case 54:
        return b >= 203;
      case 55:
        return b >= 198;
      case 56:
        return b >= 193;
      case 57:
        return b >= 188;
      case 58:
        return b >= 182;
      case 59:
        return b >= 177;
      case 60:
        return b >= 172;
      case 61:
        return b >= 167;
      case 62:
        return b >= 162;
      case 63 /*0x3F*/:
        return b >= 157;
      case 64 /*0x40*/:
        return b >= 151;
      case 65:
        return b >= 146;
      case 66:
        return b >= 141;
      case 67:
        return b >= 136;
      case 68:
        return b >= 131;
      case 69:
        return b >= 126;
      case 70:
        return b >= 121;
      case 71:
        return b >= 115;
      case 72:
        return b >= 110;
      case 73:
        return b >= 105;
      case 74:
        return b >= 100;
      case 75:
        return b >= 95;
      case 76:
        return b >= 90;
      case 77:
        return b >= 85;
      case 78:
        return b >= 79;
      case 79:
        return b >= 74;
      case 80 /*0x50*/:
        return b >= 69;
      case 81:
        return b >= 64 /*0x40*/;
      case 82:
        return b >= 59;
      case 83:
        return b >= 54;
      case 84:
        return b >= 49;
      case 85:
        return b >= 43;
      case 86:
        return b >= 38;
      case 87:
        return b >= 33;
      case 88:
        return b >= 28;
      case 89:
        return b >= 23;
      case 90:
        return b >= 18;
      case 91:
        return b >= 12;
      case 92:
        return b >= 7;
      case 93:
        return b >= 2;
      default:
        return g >= 94;
    }
  }

  private static bool CheckR21(int g, int b)
  {
    switch (g)
    {
      case 44:
        return b >= 252;
      case 45:
        return b >= 247;
      case 46:
        return b >= 242;
      case 47:
        return b >= 236;
      case 48 /*0x30*/:
        return b >= 231;
      case 49:
        return b >= 226;
      case 50:
        return b >= 221;
      case 51:
        return b >= 216;
      case 52:
        return b >= 211;
      case 53:
        return b >= 206;
      case 54:
        return b >= 200;
      case 55:
        return b >= 195;
      case 56:
        return b >= 190;
      case 57:
        return b >= 185;
      case 58:
        return b >= 180;
      case 59:
        return b >= 175;
      case 60:
        return b >= 169;
      case 61:
        return b >= 164;
      case 62:
        return b >= 159;
      case 63 /*0x3F*/:
        return b >= 154;
      case 64 /*0x40*/:
        return b >= 149;
      case 65:
        return b >= 144 /*0x90*/;
      case 66:
        return b >= 139;
      case 67:
        return b >= 133;
      case 68:
        return b >= 128 /*0x80*/;
      case 69:
        return b >= 123;
      case 70:
        return b >= 118;
      case 71:
        return b >= 113;
      case 72:
        return b >= 108;
      case 73:
        return b >= 103;
      case 74:
        return b >= 97;
      case 75:
        return b >= 92;
      case 76:
        return b >= 87;
      case 77:
        return b >= 82;
      case 78:
        return b >= 77;
      case 79:
        return b >= 72;
      case 80 /*0x50*/:
        return b >= 66;
      case 81:
        return b >= 61;
      case 82:
        return b >= 56;
      case 83:
        return b >= 51;
      case 84:
        return b >= 46;
      case 85:
        return b >= 41;
      case 86:
        return b >= 36;
      case 87:
        return b >= 30;
      case 88:
        return b >= 25;
      case 89:
        return b >= 20;
      case 90:
        return b >= 15;
      case 91:
        return b >= 10;
      case 92:
        return b >= 5;
      default:
        return g >= 93;
    }
  }

  private static bool CheckR22(int g, int b)
  {
    switch (g)
    {
      case 43:
        return b >= 254;
      case 44:
        return b >= 249;
      case 45:
        return b >= 244;
      case 46:
        return b >= 239;
      case 47:
        return b >= 234;
      case 48 /*0x30*/:
        return b >= 229;
      case 49:
        return b >= 223;
      case 50:
        return b >= 218;
      case 51:
        return b >= 213;
      case 52:
        return b >= 208 /*0xD0*/;
      case 53:
        return b >= 203;
      case 54:
        return b >= 198;
      case 55:
        return b >= 193;
      case 56:
        return b >= 187;
      case 57:
        return b >= 182;
      case 58:
        return b >= 177;
      case 59:
        return b >= 172;
      case 60:
        return b >= 167;
      case 61:
        return b >= 162;
      case 62:
        return b >= 157;
      case 63 /*0x3F*/:
        return b >= 151;
      case 64 /*0x40*/:
        return b >= 146;
      case 65:
        return b >= 141;
      case 66:
        return b >= 136;
      case 67:
        return b >= 131;
      case 68:
        return b >= 126;
      case 69:
        return b >= 121;
      case 70:
        return b >= 115;
      case 71:
        return b >= 110;
      case 72:
        return b >= 105;
      case 73:
        return b >= 100;
      case 74:
        return b >= 95;
      case 75:
        return b >= 90;
      case 76:
        return b >= 84;
      case 77:
        return b >= 79;
      case 78:
        return b >= 74;
      case 79:
        return b >= 69;
      case 80 /*0x50*/:
        return b >= 64 /*0x40*/;
      case 81:
        return b >= 59;
      case 82:
        return b >= 54;
      case 83:
        return b >= 48 /*0x30*/;
      case 84:
        return b >= 43;
      case 85:
        return b >= 38;
      case 86:
        return b >= 33;
      case 87:
        return b >= 28;
      case 88:
        return b >= 23;
      case 89:
        return b >= 18;
      case 90:
        return b >= 12;
      case 91:
        return b >= 7;
      case 92:
        return b >= 2;
      default:
        return g >= 93;
    }
  }

  private static bool CheckR23(int g, int b)
  {
    switch (g)
    {
      case 43:
        return b >= 252;
      case 44:
        return b >= 247;
      case 45:
        return b >= 241;
      case 46:
        return b >= 236;
      case 47:
        return b >= 231;
      case 48 /*0x30*/:
        return b >= 226;
      case 49:
        return b >= 221;
      case 50:
        return b >= 216;
      case 51:
        return b >= 211;
      case 52:
        return b >= 205;
      case 53:
        return b >= 200;
      case 54:
        return b >= 195;
      case 55:
        return b >= 190;
      case 56:
        return b >= 185;
      case 57:
        return b >= 180;
      case 58:
        return b >= 175;
      case 59:
        return b >= 169;
      case 60:
        return b >= 164;
      case 61:
        return b >= 159;
      case 62:
        return b >= 154;
      case 63 /*0x3F*/:
        return b >= 149;
      case 64 /*0x40*/:
        return b >= 144 /*0x90*/;
      case 65:
        return b >= 138;
      case 66:
        return b >= 133;
      case 67:
        return b >= 128 /*0x80*/;
      case 68:
        return b >= 123;
      case 69:
        return b >= 118;
      case 70:
        return b >= 113;
      case 71:
        return b >= 108;
      case 72:
        return b >= 102;
      case 73:
        return b >= 97;
      case 74:
        return b >= 92;
      case 75:
        return b >= 87;
      case 76:
        return b >= 82;
      case 77:
        return b >= 77;
      case 78:
        return b >= 72;
      case 79:
        return b >= 66;
      case 80 /*0x50*/:
        return b >= 61;
      case 81:
        return b >= 56;
      case 82:
        return b >= 51;
      case 83:
        return b >= 46;
      case 84:
        return b >= 41;
      case 85:
        return b >= 35;
      case 86:
        return b >= 30;
      case 87:
        return b >= 25;
      case 88:
        return b >= 20;
      case 89:
        return b >= 15;
      case 90:
        return b >= 10;
      case 91:
        return b >= 5;
      default:
        return g >= 92;
    }
  }

  private static bool CheckR24(int g, int b)
  {
    switch (g)
    {
      case 42:
        return b >= 254;
      case 43:
        return b >= 249;
      case 44:
        return b >= 244;
      case 45:
        return b >= 239;
      case 46:
        return b >= 234;
      case 47:
        return b >= 229;
      case 48 /*0x30*/:
        return b >= 223;
      case 49:
        return b >= 218;
      case 50:
        return b >= 213;
      case 51:
        return b >= 208 /*0xD0*/;
      case 52:
        return b >= 203;
      case 53:
        return b >= 198;
      case 54:
        return b >= 192 /*0xC0*/;
      case 55:
        return b >= 187;
      case 56:
        return b >= 182;
      case 57:
        return b >= 177;
      case 58:
        return b >= 172;
      case 59:
        return b >= 167;
      case 60:
        return b >= 162;
      case 61:
        return b >= 156;
      case 62:
        return b >= 151;
      case 63 /*0x3F*/:
        return b >= 146;
      case 64 /*0x40*/:
        return b >= 141;
      case 65:
        return b >= 136;
      case 66:
        return b >= 131;
      case 67:
        return b >= 126;
      case 68:
        return b >= 120;
      case 69:
        return b >= 115;
      case 70:
        return b >= 110;
      case 71:
        return b >= 105;
      case 72:
        return b >= 100;
      case 73:
        return b >= 95;
      case 74:
        return b >= 90;
      case 75:
        return b >= 84;
      case 76:
        return b >= 79;
      case 77:
        return b >= 74;
      case 78:
        return b >= 69;
      case 79:
        return b >= 64 /*0x40*/;
      case 80 /*0x50*/:
        return b >= 59;
      case 81:
        return b >= 53;
      case 82:
        return b >= 48 /*0x30*/;
      case 83:
        return b >= 43;
      case 84:
        return b >= 38;
      case 85:
        return b >= 33;
      case 86:
        return b >= 28;
      case 87:
        return b >= 23;
      case 88:
        return b >= 17;
      case 89:
        return b >= 12;
      case 90:
        return b >= 7;
      case 91:
        return b >= 2;
      default:
        return g >= 92;
    }
  }

  private static bool CheckR25(int g, int b)
  {
    switch (g)
    {
      case 42:
        return b >= 252;
      case 43:
        return b >= 247;
      case 44:
        return b >= 241;
      case 45:
        return b >= 236;
      case 46:
        return b >= 231;
      case 47:
        return b >= 226;
      case 48 /*0x30*/:
        return b >= 221;
      case 49:
        return b >= 216;
      case 50:
        return b >= 210;
      case 51:
        return b >= 205;
      case 52:
        return b >= 200;
      case 53:
        return b >= 195;
      case 54:
        return b >= 190;
      case 55:
        return b >= 185;
      case 56:
        return b >= 180;
      case 57:
        return b >= 174;
      case 58:
        return b >= 169;
      case 59:
        return b >= 164;
      case 60:
        return b >= 159;
      case 61:
        return b >= 154;
      case 62:
        return b >= 149;
      case 63 /*0x3F*/:
        return b >= 144 /*0x90*/;
      case 64 /*0x40*/:
        return b >= 138;
      case 65:
        return b >= 133;
      case 66:
        return b >= 128 /*0x80*/;
      case 67:
        return b >= 123;
      case 68:
        return b >= 118;
      case 69:
        return b >= 113;
      case 70:
        return b >= 107;
      case 71:
        return b >= 102;
      case 72:
        return b >= 97;
      case 73:
        return b >= 92;
      case 74:
        return b >= 87;
      case 75:
        return b >= 82;
      case 76:
        return b >= 77;
      case 77:
        return b >= 71;
      case 78:
        return b >= 66;
      case 79:
        return b >= 61;
      case 80 /*0x50*/:
        return b >= 56;
      case 81:
        return b >= 51;
      case 82:
        return b >= 46;
      case 83:
        return b >= 41;
      case 84:
        return b >= 35;
      case 85:
        return b >= 30;
      case 86:
        return b >= 25;
      case 87:
        return b >= 20;
      case 88:
        return b >= 15;
      case 89:
        return b >= 10;
      case 90:
        return b >= 5;
      default:
        return g >= 91;
    }
  }

  private static bool CheckR26(int g, int b)
  {
    switch (g)
    {
      case 41:
        return b >= 254;
      case 42:
        return b >= 249;
      case 43:
        return b >= 244;
      case 44:
        return b >= 239;
      case 45:
        return b >= 234;
      case 46:
        return b >= 228;
      case 47:
        return b >= 223;
      case 48 /*0x30*/:
        return b >= 218;
      case 49:
        return b >= 213;
      case 50:
        return b >= 208 /*0xD0*/;
      case 51:
        return b >= 203;
      case 52:
        return b >= 198;
      case 53:
        return b >= 192 /*0xC0*/;
      case 54:
        return b >= 187;
      case 55:
        return b >= 182;
      case 56:
        return b >= 177;
      case 57:
        return b >= 172;
      case 58:
        return b >= 167;
      case 59:
        return b >= 162;
      case 60:
        return b >= 156;
      case 61:
        return b >= 151;
      case 62:
        return b >= 146;
      case 63 /*0x3F*/:
        return b >= 141;
      case 64 /*0x40*/:
        return b >= 136;
      case 65:
        return b >= 131;
      case 66:
        return b >= 125;
      case 67:
        return b >= 120;
      case 68:
        return b >= 115;
      case 69:
        return b >= 110;
      case 70:
        return b >= 105;
      case 71:
        return b >= 100;
      case 72:
        return b >= 95;
      case 73:
        return b >= 89;
      case 74:
        return b >= 84;
      case 75:
        return b >= 79;
      case 76:
        return b >= 74;
      case 77:
        return b >= 69;
      case 78:
        return b >= 64 /*0x40*/;
      case 79:
        return b >= 59;
      case 80 /*0x50*/:
        return b >= 53;
      case 81:
        return b >= 48 /*0x30*/;
      case 82:
        return b >= 43;
      case 83:
        return b >= 38;
      case 84:
        return b >= 33;
      case 85:
        return b >= 28;
      case 86:
        return b >= 22;
      case 87:
        return b >= 17;
      case 88:
        return b >= 12;
      case 89:
        return b >= 7;
      case 90:
        return b >= 2;
      default:
        return g >= 91;
    }
  }

  private static bool CheckR27(int g, int b)
  {
    switch (g)
    {
      case 41:
        return b >= 252;
      case 42:
        return b >= 246;
      case 43:
        return b >= 241;
      case 44:
        return b >= 236;
      case 45:
        return b >= 231;
      case 46:
        return b >= 226;
      case 47:
        return b >= 221;
      case 48 /*0x30*/:
        return b >= 216;
      case 49:
        return b >= 210;
      case 50:
        return b >= 205;
      case 51:
        return b >= 200;
      case 52:
        return b >= 195;
      case 53:
        return b >= 190;
      case 54:
        return b >= 185;
      case 55:
        return b >= 179;
      case 56:
        return b >= 174;
      case 57:
        return b >= 169;
      case 58:
        return b >= 164;
      case 59:
        return b >= 159;
      case 60:
        return b >= 154;
      case 61:
        return b >= 149;
      case 62:
        return b >= 143;
      case 63 /*0x3F*/:
        return b >= 138;
      case 64 /*0x40*/:
        return b >= 133;
      case 65:
        return b >= 128 /*0x80*/;
      case 66:
        return b >= 123;
      case 67:
        return b >= 118;
      case 68:
        return b >= 113;
      case 69:
        return b >= 107;
      case 70:
        return b >= 102;
      case 71:
        return b >= 97;
      case 72:
        return b >= 92;
      case 73:
        return b >= 87;
      case 74:
        return b >= 82;
      case 75:
        return b >= 76;
      case 76:
        return b >= 71;
      case 77:
        return b >= 66;
      case 78:
        return b >= 61;
      case 79:
        return b >= 56;
      case 80 /*0x50*/:
        return b >= 51;
      case 81:
        return b >= 46;
      case 82:
        return b >= 40;
      case 83:
        return b >= 35;
      case 84:
        return b >= 30;
      case 85:
        return b >= 25;
      case 86:
        return b >= 20;
      case 87:
        return b >= 15;
      case 88:
        return b >= 10;
      case 89:
        return b >= 4;
      default:
        return g >= 90;
    }
  }

  private static bool CheckR28(int g, int b)
  {
    switch (g)
    {
      case 40:
        return b >= 254;
      case 41:
        return b >= 249;
      case 42:
        return b >= 244;
      case 43:
        return b >= 239;
      case 44:
        return b >= 233;
      case 45:
        return b >= 228;
      case 46:
        return b >= 223;
      case 47:
        return b >= 218;
      case 48 /*0x30*/:
        return b >= 213;
      case 49:
        return b >= 208 /*0xD0*/;
      case 50:
        return b >= 203;
      case 51:
        return b >= 197;
      case 52:
        return b >= 192 /*0xC0*/;
      case 53:
        return b >= 187;
      case 54:
        return b >= 182;
      case 55:
        return b >= 177;
      case 56:
        return b >= 172;
      case 57:
        return b >= 167;
      case 58:
        return b >= 161;
      case 59:
        return b >= 156;
      case 60:
        return b >= 151;
      case 61:
        return b >= 146;
      case 62:
        return b >= 141;
      case 63 /*0x3F*/:
        return b >= 136;
      case 64 /*0x40*/:
        return b >= 131;
      case 65:
        return b >= 125;
      case 66:
        return b >= 120;
      case 67:
        return b >= 115;
      case 68:
        return b >= 110;
      case 69:
        return b >= 105;
      case 70:
        return b >= 100;
      case 71:
        return b >= 94;
      case 72:
        return b >= 89;
      case 73:
        return b >= 84;
      case 74:
        return b >= 79;
      case 75:
        return b >= 74;
      case 76:
        return b >= 69;
      case 77:
        return b >= 64 /*0x40*/;
      case 78:
        return b >= 58;
      case 79:
        return b >= 53;
      case 80 /*0x50*/:
        return b >= 48 /*0x30*/;
      case 81:
        return b >= 43;
      case 82:
        return b >= 38;
      case 83:
        return b >= 33;
      case 84:
        return b >= 28;
      case 85:
        return b >= 22;
      case 86:
        return b >= 17;
      case 87:
        return b >= 12;
      case 88:
        return b >= 7;
      case 89:
        return b >= 2;
      default:
        return g >= 90;
    }
  }

  private static bool CheckR29(int g, int b)
  {
    switch (g)
    {
      case 40:
        return b >= 251;
      case 41:
        return b >= 246;
      case 42:
        return b >= 241;
      case 43:
        return b >= 236;
      case 44:
        return b >= 231;
      case 45:
        return b >= 226;
      case 46:
        return b >= 221;
      case 47:
        return b >= 215;
      case 48 /*0x30*/:
        return b >= 210;
      case 49:
        return b >= 205;
      case 50:
        return b >= 200;
      case 51:
        return b >= 195;
      case 52:
        return b >= 190;
      case 53:
        return b >= 185;
      case 54:
        return b >= 179;
      case 55:
        return b >= 174;
      case 56:
        return b >= 169;
      case 57:
        return b >= 164;
      case 58:
        return b >= 159;
      case 59:
        return b >= 154;
      case 60:
        return b >= 148;
      case 61:
        return b >= 143;
      case 62:
        return b >= 138;
      case 63 /*0x3F*/:
        return b >= 133;
      case 64 /*0x40*/:
        return b >= 128 /*0x80*/;
      case 65:
        return b >= 123;
      case 66:
        return b >= 118;
      case 67:
        return b >= 112 /*0x70*/;
      case 68:
        return b >= 107;
      case 69:
        return b >= 102;
      case 70:
        return b >= 97;
      case 71:
        return b >= 92;
      case 72:
        return b >= 87;
      case 73:
        return b >= 82;
      case 74:
        return b >= 76;
      case 75:
        return b >= 71;
      case 76:
        return b >= 66;
      case 77:
        return b >= 61;
      case 78:
        return b >= 56;
      case 79:
        return b >= 51;
      case 80 /*0x50*/:
        return b >= 46;
      case 81:
        return b >= 40;
      case 82:
        return b >= 35;
      case 83:
        return b >= 30;
      case 84:
        return b >= 25;
      case 85:
        return b >= 20;
      case 86:
        return b >= 15;
      case 87:
        return b >= 9;
      case 88:
        return b >= 4;
      default:
        return g >= 89;
    }
  }

  private static bool CheckR30(int g, int b)
  {
    switch (g)
    {
      case 39:
        return b >= 254;
      case 40:
        return b >= 249;
      case 41:
        return b >= 244;
      case 42:
        return b >= 239;
      case 43:
        return b >= 233;
      case 44:
        return b >= 228;
      case 45:
        return b >= 223;
      case 46:
        return b >= 218;
      case 47:
        return b >= 213;
      case 48 /*0x30*/:
        return b >= 208 /*0xD0*/;
      case 49:
        return b >= 203;
      case 50:
        return b >= 197;
      case 51:
        return b >= 192 /*0xC0*/;
      case 52:
        return b >= 187;
      case 53:
        return b >= 182;
      case 54:
        return b >= 177;
      case 55:
        return b >= 172;
      case 56:
        return b >= 166;
      case 57:
        return b >= 161;
      case 58:
        return b >= 156;
      case 59:
        return b >= 151;
      case 60:
        return b >= 146;
      case 61:
        return b >= 141;
      case 62:
        return b >= 136;
      case 63 /*0x3F*/:
        return b >= 130;
      case 64 /*0x40*/:
        return b >= 125;
      case 65:
        return b >= 120;
      case 66:
        return b >= 115;
      case 67:
        return b >= 110;
      case 68:
        return b >= 105;
      case 69:
        return b >= 100;
      case 70:
        return b >= 94;
      case 71:
        return b >= 89;
      case 72:
        return b >= 84;
      case 73:
        return b >= 79;
      case 74:
        return b >= 74;
      case 75:
        return b >= 69;
      case 76:
        return b >= 63 /*0x3F*/;
      case 77:
        return b >= 58;
      case 78:
        return b >= 53;
      case 79:
        return b >= 48 /*0x30*/;
      case 80 /*0x50*/:
        return b >= 43;
      case 81:
        return b >= 38;
      case 82:
        return b >= 33;
      case 83:
        return b >= 27;
      case 84:
        return b >= 22;
      case 85:
        return b >= 17;
      case 86:
        return b >= 12;
      case 87:
        return b >= 7;
      case 88:
        return b >= 2;
      default:
        return g >= 89;
    }
  }

  private static bool CheckR31(int g, int b)
  {
    switch (g)
    {
      case 39:
        return b >= 251;
      case 40:
        return b >= 246;
      case 41:
        return b >= 241;
      case 42:
        return b >= 236;
      case 43:
        return b >= 231;
      case 44:
        return b >= 226;
      case 45:
        return b >= 220;
      case 46:
        return b >= 215;
      case 47:
        return b >= 210;
      case 48 /*0x30*/:
        return b >= 205;
      case 49:
        return b >= 200;
      case 50:
        return b >= 195;
      case 51:
        return b >= 190;
      case 52:
        return b >= 184;
      case 53:
        return b >= 179;
      case 54:
        return b >= 174;
      case 55:
        return b >= 169;
      case 56:
        return b >= 164;
      case 57:
        return b >= 159;
      case 58:
        return b >= 154;
      case 59:
        return b >= 148;
      case 60:
        return b >= 143;
      case 61:
        return b >= 138;
      case 62:
        return b >= 133;
      case 63 /*0x3F*/:
        return b >= 128 /*0x80*/;
      case 64 /*0x40*/:
        return b >= 123;
      case 65:
        return b >= 117;
      case 66:
        return b >= 112 /*0x70*/;
      case 67:
        return b >= 107;
      case 68:
        return b >= 102;
      case 69:
        return b >= 97;
      case 70:
        return b >= 92;
      case 71:
        return b >= 87;
      case 72:
        return b >= 81;
      case 73:
        return b >= 76;
      case 74:
        return b >= 71;
      case 75:
        return b >= 66;
      case 76:
        return b >= 61;
      case 77:
        return b >= 56;
      case 78:
        return b >= 51;
      case 79:
        return b >= 45;
      case 80 /*0x50*/:
        return b >= 40;
      case 81:
        return b >= 35;
      case 82:
        return b >= 30;
      case 83:
        return b >= 25;
      case 84:
        return b >= 20;
      case 85:
        return b >= 15;
      case 86:
        return b >= 9;
      case 87:
        return b >= 4;
      default:
        return g >= 88;
    }
  }

  private static bool CheckR32(int g, int b)
  {
    switch (g)
    {
      case 38:
        return b >= 254;
      case 39:
        return b >= 249;
      case 40:
        return b >= 244;
      case 41:
        return b >= 238;
      case 42:
        return b >= 233;
      case 43:
        return b >= 228;
      case 44:
        return b >= 223;
      case 45:
        return b >= 218;
      case 46:
        return b >= 213;
      case 47:
        return b >= 208 /*0xD0*/;
      case 48 /*0x30*/:
        return b >= 202;
      case 49:
        return b >= 197;
      case 50:
        return b >= 192 /*0xC0*/;
      case 51:
        return b >= 187;
      case 52:
        return b >= 182;
      case 53:
        return b >= 177;
      case 54:
        return b >= 172;
      case 55:
        return b >= 166;
      case 56:
        return b >= 161;
      case 57:
        return b >= 156;
      case 58:
        return b >= 151;
      case 59:
        return b >= 146;
      case 60:
        return b >= 141;
      case 61:
        return b >= 135;
      case 62:
        return b >= 130;
      case 63 /*0x3F*/:
        return b >= 125;
      case 64 /*0x40*/:
        return b >= 120;
      case 65:
        return b >= 115;
      case 66:
        return b >= 110;
      case 67:
        return b >= 105;
      case 68:
        return b >= 99;
      case 69:
        return b >= 94;
      case 70:
        return b >= 89;
      case 71:
        return b >= 84;
      case 72:
        return b >= 79;
      case 73:
        return b >= 74;
      case 74:
        return b >= 69;
      case 75:
        return b >= 63 /*0x3F*/;
      case 76:
        return b >= 58;
      case 77:
        return b >= 53;
      case 78:
        return b >= 48 /*0x30*/;
      case 79:
        return b >= 43;
      case 80 /*0x50*/:
        return b >= 38;
      case 81:
        return b >= 32 /*0x20*/;
      case 82:
        return b >= 27;
      case 83:
        return b >= 22;
      case 84:
        return b >= 17;
      case 85:
        return b >= 12;
      case 86:
        return b >= 7;
      case 87:
        return b >= 2;
      default:
        return g >= 88;
    }
  }

  private static bool CheckR33(int g, int b)
  {
    switch (g)
    {
      case 38:
        return b >= 251;
      case 39:
        return b >= 246;
      case 40:
        return b >= 241;
      case 41:
        return b >= 236;
      case 42:
        return b >= 231;
      case 43:
        return b >= 226;
      case 44:
        return b >= 220;
      case 45:
        return b >= 215;
      case 46:
        return b >= 210;
      case 47:
        return b >= 205;
      case 48 /*0x30*/:
        return b >= 200;
      case 49:
        return b >= 195;
      case 50:
        return b >= 189;
      case 51:
        return b >= 184;
      case 52:
        return b >= 179;
      case 53:
        return b >= 174;
      case 54:
        return b >= 169;
      case 55:
        return b >= 164;
      case 56:
        return b >= 159;
      case 57:
        return b >= 153;
      case 58:
        return b >= 148;
      case 59:
        return b >= 143;
      case 60:
        return b >= 138;
      case 61:
        return b >= 133;
      case 62:
        return b >= 128 /*0x80*/;
      case 63 /*0x3F*/:
        return b >= 123;
      case 64 /*0x40*/:
        return b >= 117;
      case 65:
        return b >= 112 /*0x70*/;
      case 66:
        return b >= 107;
      case 67:
        return b >= 102;
      case 68:
        return b >= 97;
      case 69:
        return b >= 92;
      case 70:
        return b >= 87;
      case 71:
        return b >= 81;
      case 72:
        return b >= 76;
      case 73:
        return b >= 71;
      case 74:
        return b >= 66;
      case 75:
        return b >= 61;
      case 76:
        return b >= 56;
      case 77:
        return b >= 50;
      case 78:
        return b >= 45;
      case 79:
        return b >= 40;
      case 80 /*0x50*/:
        return b >= 35;
      case 81:
        return b >= 30;
      case 82:
        return b >= 25;
      case 83:
        return b >= 20;
      case 84:
        return b >= 14;
      case 85:
        return b >= 9;
      case 86:
        return b >= 4;
      default:
        return g >= 87;
    }
  }

  private static bool CheckR34(int g, int b)
  {
    switch (g)
    {
      case 37:
        return b >= 254;
      case 38:
        return b >= 249;
      case 39:
        return b >= 244;
      case 40:
        return b >= 238;
      case 41:
        return b >= 233;
      case 42:
        return b >= 228;
      case 43:
        return b >= 223;
      case 44:
        return b >= 218;
      case 45:
        return b >= 213;
      case 46:
        return b >= 207;
      case 47:
        return b >= 202;
      case 48 /*0x30*/:
        return b >= 197;
      case 49:
        return b >= 192 /*0xC0*/;
      case 50:
        return b >= 187;
      case 51:
        return b >= 182;
      case 52:
        return b >= 177;
      case 53:
        return b >= 171;
      case 54:
        return b >= 166;
      case 55:
        return b >= 161;
      case 56:
        return b >= 156;
      case 57:
        return b >= 151;
      case 58:
        return b >= 146;
      case 59:
        return b >= 141;
      case 60:
        return b >= 135;
      case 61:
        return b >= 130;
      case 62:
        return b >= 125;
      case 63 /*0x3F*/:
        return b >= 120;
      case 64 /*0x40*/:
        return b >= 115;
      case 65:
        return b >= 110;
      case 66:
        return b >= 104;
      case 67:
        return b >= 99;
      case 68:
        return b >= 94;
      case 69:
        return b >= 89;
      case 70:
        return b >= 84;
      case 71:
        return b >= 79;
      case 72:
        return b >= 74;
      case 73:
        return b >= 68;
      case 74:
        return b >= 63 /*0x3F*/;
      case 75:
        return b >= 58;
      case 76:
        return b >= 53;
      case 77:
        return b >= 48 /*0x30*/;
      case 78:
        return b >= 43;
      case 79:
        return b >= 38;
      case 80 /*0x50*/:
        return b >= 32 /*0x20*/;
      case 81:
        return b >= 27;
      case 82:
        return b >= 22;
      case 83:
        return b >= 17;
      case 84:
        return b >= 12;
      case 85:
        return b >= 7;
      case 86:
        return b >= 1;
      default:
        return g >= 87;
    }
  }

  private static bool CheckR35(int g, int b)
  {
    switch (g)
    {
      case 37:
        return b >= 251;
      case 38:
        return b >= 246;
      case 39:
        return b >= 241;
      case 40:
        return b >= 236;
      case 41:
        return b >= 231;
      case 42:
        return b >= 225;
      case 43:
        return b >= 220;
      case 44:
        return b >= 215;
      case 45:
        return b >= 210;
      case 46:
        return b >= 205;
      case 47:
        return b >= 200;
      case 48 /*0x30*/:
        return b >= 195;
      case 49:
        return b >= 189;
      case 50:
        return b >= 184;
      case 51:
        return b >= 179;
      case 52:
        return b >= 174;
      case 53:
        return b >= 169;
      case 54:
        return b >= 164;
      case 55:
        return b >= 158;
      case 56:
        return b >= 153;
      case 57:
        return b >= 148;
      case 58:
        return b >= 143;
      case 59:
        return b >= 138;
      case 60:
        return b >= 133;
      case 61:
        return b >= 128 /*0x80*/;
      case 62:
        return b >= 122;
      case 63 /*0x3F*/:
        return b >= 117;
      case 64 /*0x40*/:
        return b >= 112 /*0x70*/;
      case 65:
        return b >= 107;
      case 66:
        return b >= 102;
      case 67:
        return b >= 97;
      case 68:
        return b >= 92;
      case 69:
        return b >= 86;
      case 70:
        return b >= 81;
      case 71:
        return b >= 76;
      case 72:
        return b >= 71;
      case 73:
        return b >= 66;
      case 74:
        return b >= 61;
      case 75:
        return b >= 56;
      case 76:
        return b >= 50;
      case 77:
        return b >= 45;
      case 78:
        return b >= 40;
      case 79:
        return b >= 35;
      case 80 /*0x50*/:
        return b >= 30;
      case 81:
        return b >= 25;
      case 82:
        return b >= 19;
      case 83:
        return b >= 14;
      case 84:
        return b >= 9;
      case 85:
        return b >= 4;
      default:
        return g >= 86;
    }
  }

  private static bool CheckR36(int g, int b)
  {
    switch (g)
    {
      case 36:
        return b >= 254;
      case 37:
        return b >= 249;
      case 38:
        return b >= 243;
      case 39:
        return b >= 238;
      case 40:
        return b >= 233;
      case 41:
        return b >= 228;
      case 42:
        return b >= 223;
      case 43:
        return b >= 218;
      case 44:
        return b >= 213;
      case 45:
        return b >= 207;
      case 46:
        return b >= 202;
      case 47:
        return b >= 197;
      case 48 /*0x30*/:
        return b >= 192 /*0xC0*/;
      case 49:
        return b >= 187;
      case 50:
        return b >= 182;
      case 51:
        return b >= 176 /*0xB0*/;
      case 52:
        return b >= 171;
      case 53:
        return b >= 166;
      case 54:
        return b >= 161;
      case 55:
        return b >= 156;
      case 56:
        return b >= 151;
      case 57:
        return b >= 146;
      case 58:
        return b >= 140;
      case 59:
        return b >= 135;
      case 60:
        return b >= 130;
      case 61:
        return b >= 125;
      case 62:
        return b >= 120;
      case 63 /*0x3F*/:
        return b >= 115;
      case 64 /*0x40*/:
        return b >= 110;
      case 65:
        return b >= 104;
      case 66:
        return b >= 99;
      case 67:
        return b >= 94;
      case 68:
        return b >= 89;
      case 69:
        return b >= 84;
      case 70:
        return b >= 79;
      case 71:
        return b >= 73;
      case 72:
        return b >= 68;
      case 73:
        return b >= 63 /*0x3F*/;
      case 74:
        return b >= 58;
      case 75:
        return b >= 53;
      case 76:
        return b >= 48 /*0x30*/;
      case 77:
        return b >= 43;
      case 78:
        return b >= 37;
      case 79:
        return b >= 32 /*0x20*/;
      case 80 /*0x50*/:
        return b >= 27;
      case 81:
        return b >= 22;
      case 82:
        return b >= 17;
      case 83:
        return b >= 12;
      case 84:
        return b >= 7;
      case 85:
        return b >= 1;
      default:
        return g >= 86;
    }
  }

  private static bool CheckR37(int g, int b)
  {
    switch (g)
    {
      case 36:
        return b >= 251;
      case 37:
        return b >= 246;
      case 38:
        return b >= 241;
      case 39:
        return b >= 236;
      case 40:
        return b >= 230;
      case 41:
        return b >= 225;
      case 42:
        return b >= 220;
      case 43:
        return b >= 215;
      case 44:
        return b >= 210;
      case 45:
        return b >= 205;
      case 46:
        return b >= 200;
      case 47:
        return b >= 194;
      case 48 /*0x30*/:
        return b >= 189;
      case 49:
        return b >= 184;
      case 50:
        return b >= 179;
      case 51:
        return b >= 174;
      case 52:
        return b >= 169;
      case 53:
        return b >= 164;
      case 54:
        return b >= 158;
      case 55:
        return b >= 153;
      case 56:
        return b >= 148;
      case 57:
        return b >= 143;
      case 58:
        return b >= 138;
      case 59:
        return b >= 133;
      case 60:
        return b >= 128 /*0x80*/;
      case 61:
        return b >= 122;
      case 62:
        return b >= 117;
      case 63 /*0x3F*/:
        return b >= 112 /*0x70*/;
      case 64 /*0x40*/:
        return b >= 107;
      case 65:
        return b >= 102;
      case 66:
        return b >= 97;
      case 67:
        return b >= 91;
      case 68:
        return b >= 86;
      case 69:
        return b >= 81;
      case 70:
        return b >= 76;
      case 71:
        return b >= 71;
      case 72:
        return b >= 66;
      case 73:
        return b >= 61;
      case 74:
        return b >= 55;
      case 75:
        return b >= 50;
      case 76:
        return b >= 45;
      case 77:
        return b >= 40;
      case 78:
        return b >= 35;
      case 79:
        return b >= 30;
      case 80 /*0x50*/:
        return b >= 25;
      case 81:
        return b >= 19;
      case 82:
        return b >= 14;
      case 83:
        return b >= 9;
      case 84:
        return b >= 4;
      default:
        return g >= 85;
    }
  }

  private static bool CheckR38(int g, int b)
  {
    switch (g)
    {
      case 35:
        return b >= 254;
      case 36:
        return b >= 248;
      case 37:
        return b >= 243;
      case 38:
        return b >= 238;
      case 39:
        return b >= 233;
      case 40:
        return b >= 228;
      case 41:
        return b >= 223;
      case 42:
        return b >= 218;
      case 43:
        return b >= 212;
      case 44:
        return b >= 207;
      case 45:
        return b >= 202;
      case 46:
        return b >= 197;
      case 47:
        return b >= 192 /*0xC0*/;
      case 48 /*0x30*/:
        return b >= 187;
      case 49:
        return b >= 182;
      case 50:
        return b >= 176 /*0xB0*/;
      case 51:
        return b >= 171;
      case 52:
        return b >= 166;
      case 53:
        return b >= 161;
      case 54:
        return b >= 156;
      case 55:
        return b >= 151;
      case 56:
        return b >= 145;
      case 57:
        return b >= 140;
      case 58:
        return b >= 135;
      case 59:
        return b >= 130;
      case 60:
        return b >= 125;
      case 61:
        return b >= 120;
      case 62:
        return b >= 115;
      case 63 /*0x3F*/:
        return b >= 109;
      case 64 /*0x40*/:
        return b >= 104;
      case 65:
        return b >= 99;
      case 66:
        return b >= 94;
      case 67:
        return b >= 89;
      case 68:
        return b >= 84;
      case 69:
        return b >= 79;
      case 70:
        return b >= 73;
      case 71:
        return b >= 68;
      case 72:
        return b >= 63 /*0x3F*/;
      case 73:
        return b >= 58;
      case 74:
        return b >= 53;
      case 75:
        return b >= 48 /*0x30*/;
      case 76:
        return b >= 42;
      case 77:
        return b >= 37;
      case 78:
        return b >= 32 /*0x20*/;
      case 79:
        return b >= 27;
      case 80 /*0x50*/:
        return b >= 22;
      case 81:
        return b >= 17;
      case 82:
        return b >= 12;
      case 83:
        return b >= 6;
      case 84:
        return b >= 1;
      default:
        return g >= 85;
    }
  }

  private static bool CheckR39(int g, int b)
  {
    switch (g)
    {
      case 35:
        return b >= 251;
      case 36:
        return b >= 246;
      case 37:
        return b >= 241;
      case 38:
        return b >= 236;
      case 39:
        return b >= 230;
      case 40:
        return b >= 225;
      case 41:
        return b >= 220;
      case 42:
        return b >= 215;
      case 43:
        return b >= 210;
      case 44:
        return b >= 205;
      case 45:
        return b >= 199;
      case 46:
        return b >= 194;
      case 47:
        return b >= 189;
      case 48 /*0x30*/:
        return b >= 184;
      case 49:
        return b >= 179;
      case 50:
        return b >= 174;
      case 51:
        return b >= 169;
      case 52:
        return b >= 163;
      case 53:
        return b >= 158;
      case 54:
        return b >= 153;
      case 55:
        return b >= 148;
      case 56:
        return b >= 143;
      case 57:
        return b >= 138;
      case 58:
        return b >= 133;
      case 59:
        return b >= (int) sbyte.MaxValue;
      case 60:
        return b >= 122;
      case 61:
        return b >= 117;
      case 62:
        return b >= 112 /*0x70*/;
      case 63 /*0x3F*/:
        return b >= 107;
      case 64 /*0x40*/:
        return b >= 102;
      case 65:
        return b >= 97;
      case 66:
        return b >= 91;
      case 67:
        return b >= 86;
      case 68:
        return b >= 81;
      case 69:
        return b >= 76;
      case 70:
        return b >= 71;
      case 71:
        return b >= 66;
      case 72:
        return b >= 60;
      case 73:
        return b >= 55;
      case 74:
        return b >= 50;
      case 75:
        return b >= 45;
      case 76:
        return b >= 40;
      case 77:
        return b >= 35;
      case 78:
        return b >= 30;
      case 79:
        return b >= 24;
      case 80 /*0x50*/:
        return b >= 19;
      case 81:
        return b >= 14;
      case 82:
        return b >= 9;
      case 83:
        return b >= 4;
      default:
        return g >= 84;
    }
  }

  private static bool CheckR40(int g, int b)
  {
    switch (g)
    {
      case 34:
        return b >= 254;
      case 35:
        return b >= 248;
      case 36:
        return b >= 243;
      case 37:
        return b >= 238;
      case 38:
        return b >= 233;
      case 39:
        return b >= 228;
      case 40:
        return b >= 223;
      case 41:
        return b >= 217;
      case 42:
        return b >= 212;
      case 43:
        return b >= 207;
      case 44:
        return b >= 202;
      case 45:
        return b >= 197;
      case 46:
        return b >= 192 /*0xC0*/;
      case 47:
        return b >= 187;
      case 48 /*0x30*/:
        return b >= 181;
      case 49:
        return b >= 176 /*0xB0*/;
      case 50:
        return b >= 171;
      case 51:
        return b >= 166;
      case 52:
        return b >= 161;
      case 53:
        return b >= 156;
      case 54:
        return b >= 151;
      case 55:
        return b >= 145;
      case 56:
        return b >= 140;
      case 57:
        return b >= 135;
      case 58:
        return b >= 130;
      case 59:
        return b >= 125;
      case 60:
        return b >= 120;
      case 61:
        return b >= 114;
      case 62:
        return b >= 109;
      case 63 /*0x3F*/:
        return b >= 104;
      case 64 /*0x40*/:
        return b >= 99;
      case 65:
        return b >= 94;
      case 66:
        return b >= 89;
      case 67:
        return b >= 84;
      case 68:
        return b >= 78;
      case 69:
        return b >= 73;
      case 70:
        return b >= 68;
      case 71:
        return b >= 63 /*0x3F*/;
      case 72:
        return b >= 58;
      case 73:
        return b >= 53;
      case 74:
        return b >= 48 /*0x30*/;
      case 75:
        return b >= 42;
      case 76:
        return b >= 37;
      case 77:
        return b >= 32 /*0x20*/;
      case 78:
        return b >= 27;
      case 79:
        return b >= 22;
      case 80 /*0x50*/:
        return b >= 17;
      case 81:
        return b >= 12;
      case 82:
        return b >= 6;
      case 83:
        return b >= 1;
      default:
        return g >= 84;
    }
  }

  private static bool CheckR41(int g, int b)
  {
    switch (g)
    {
      case 34:
        return b >= 251;
      case 35:
        return b >= 246;
      case 36:
        return b >= 241;
      case 37:
        return b >= 235;
      case 38:
        return b >= 230;
      case 39:
        return b >= 225;
      case 40:
        return b >= 220;
      case 41:
        return b >= 215;
      case 42:
        return b >= 210;
      case 43:
        return b >= 205;
      case 44:
        return b >= 199;
      case 45:
        return b >= 194;
      case 46:
        return b >= 189;
      case 47:
        return b >= 184;
      case 48 /*0x30*/:
        return b >= 179;
      case 49:
        return b >= 174;
      case 50:
        return b >= 169;
      case 51:
        return b >= 163;
      case 52:
        return b >= 158;
      case 53:
        return b >= 153;
      case 54:
        return b >= 148;
      case 55:
        return b >= 143;
      case 56:
        return b >= 138;
      case 57:
        return b >= 132;
      case 58:
        return b >= (int) sbyte.MaxValue;
      case 59:
        return b >= 122;
      case 60:
        return b >= 117;
      case 61:
        return b >= 112 /*0x70*/;
      case 62:
        return b >= 107;
      case 63 /*0x3F*/:
        return b >= 102;
      case 64 /*0x40*/:
        return b >= 96 /*0x60*/;
      case 65:
        return b >= 91;
      case 66:
        return b >= 86;
      case 67:
        return b >= 81;
      case 68:
        return b >= 76;
      case 69:
        return b >= 71;
      case 70:
        return b >= 66;
      case 71:
        return b >= 60;
      case 72:
        return b >= 55;
      case 73:
        return b >= 50;
      case 74:
        return b >= 45;
      case 75:
        return b >= 40;
      case 76:
        return b >= 35;
      case 77:
        return b >= 29;
      case 78:
        return b >= 24;
      case 79:
        return b >= 19;
      case 80 /*0x50*/:
        return b >= 14;
      case 81:
        return b >= 9;
      case 82:
        return b >= 4;
      default:
        return g >= 83;
    }
  }

  private static bool CheckR42(int g, int b)
  {
    switch (g)
    {
      case 33:
        return b >= 253;
      case 34:
        return b >= 248;
      case 35:
        return b >= 243;
      case 36:
        return b >= 238;
      case 37:
        return b >= 233;
      case 38:
        return b >= 228;
      case 39:
        return b >= 223;
      case 40:
        return b >= 217;
      case 41:
        return b >= 212;
      case 42:
        return b >= 207;
      case 43:
        return b >= 202;
      case 44:
        return b >= 197;
      case 45:
        return b >= 192 /*0xC0*/;
      case 46:
        return b >= 186;
      case 47:
        return b >= 181;
      case 48 /*0x30*/:
        return b >= 176 /*0xB0*/;
      case 49:
        return b >= 171;
      case 50:
        return b >= 166;
      case 51:
        return b >= 161;
      case 52:
        return b >= 156;
      case 53:
        return b >= 150;
      case 54:
        return b >= 145;
      case 55:
        return b >= 140;
      case 56:
        return b >= 135;
      case 57:
        return b >= 130;
      case 58:
        return b >= 125;
      case 59:
        return b >= 120;
      case 60:
        return b >= 114;
      case 61:
        return b >= 109;
      case 62:
        return b >= 104;
      case 63 /*0x3F*/:
        return b >= 99;
      case 64 /*0x40*/:
        return b >= 94;
      case 65:
        return b >= 89;
      case 66:
        return b >= 83;
      case 67:
        return b >= 78;
      case 68:
        return b >= 73;
      case 69:
        return b >= 68;
      case 70:
        return b >= 63 /*0x3F*/;
      case 71:
        return b >= 58;
      case 72:
        return b >= 53;
      case 73:
        return b >= 47;
      case 74:
        return b >= 42;
      case 75:
        return b >= 37;
      case 76:
        return b >= 32 /*0x20*/;
      case 77:
        return b >= 27;
      case 78:
        return b >= 22;
      case 79:
        return b >= 17;
      case 80 /*0x50*/:
        return b >= 11;
      case 81:
        return b >= 6;
      case 82:
        return b >= 1;
      default:
        return g >= 83;
    }
  }

  private static bool CheckR43(int g, int b)
  {
    switch (g)
    {
      case 33:
        return b >= 251;
      case 34:
        return b >= 246;
      case 35:
        return b >= 240 /*0xF0*/;
      case 36:
        return b >= 235;
      case 37:
        return b >= 230;
      case 38:
        return b >= 225;
      case 39:
        return b >= 220;
      case 40:
        return b >= 215;
      case 41:
        return b >= 210;
      case 42:
        return b >= 204;
      case 43:
        return b >= 199;
      case 44:
        return b >= 194;
      case 45:
        return b >= 189;
      case 46:
        return b >= 184;
      case 47:
        return b >= 179;
      case 48 /*0x30*/:
        return b >= 174;
      case 49:
        return b >= 168;
      case 50:
        return b >= 163;
      case 51:
        return b >= 158;
      case 52:
        return b >= 153;
      case 53:
        return b >= 148;
      case 54:
        return b >= 143;
      case 55:
        return b >= 138;
      case 56:
        return b >= 132;
      case 57:
        return b >= (int) sbyte.MaxValue;
      case 58:
        return b >= 122;
      case 59:
        return b >= 117;
      case 60:
        return b >= 112 /*0x70*/;
      case 61:
        return b >= 107;
      case 62:
        return b >= 101;
      case 63 /*0x3F*/:
        return b >= 96 /*0x60*/;
      case 64 /*0x40*/:
        return b >= 91;
      case 65:
        return b >= 86;
      case 66:
        return b >= 81;
      case 67:
        return b >= 76;
      case 68:
        return b >= 71;
      case 69:
        return b >= 65;
      case 70:
        return b >= 60;
      case 71:
        return b >= 55;
      case 72:
        return b >= 50;
      case 73:
        return b >= 45;
      case 74:
        return b >= 40;
      case 75:
        return b >= 35;
      case 76:
        return b >= 29;
      case 77:
        return b >= 24;
      case 78:
        return b >= 19;
      case 79:
        return b >= 14;
      case 80 /*0x50*/:
        return b >= 9;
      case 81:
        return b >= 4;
      default:
        return g >= 82;
    }
  }

  private static bool CheckR44(int g, int b)
  {
    switch (g)
    {
      case 32 /*0x20*/:
        return b >= 253;
      case 33:
        return b >= 248;
      case 34:
        return b >= 243;
      case 35:
        return b >= 238;
      case 36:
        return b >= 233;
      case 37:
        return b >= 228;
      case 38:
        return b >= 222;
      case 39:
        return b >= 217;
      case 40:
        return b >= 212;
      case 41:
        return b >= 207;
      case 42:
        return b >= 202;
      case 43:
        return b >= 197;
      case 44:
        return b >= 192 /*0xC0*/;
      case 45:
        return b >= 186;
      case 46:
        return b >= 181;
      case 47:
        return b >= 176 /*0xB0*/;
      case 48 /*0x30*/:
        return b >= 171;
      case 49:
        return b >= 166;
      case 50:
        return b >= 161;
      case 51:
        return b >= 155;
      case 52:
        return b >= 150;
      case 53:
        return b >= 145;
      case 54:
        return b >= 140;
      case 55:
        return b >= 135;
      case 56:
        return b >= 130;
      case 57:
        return b >= 125;
      case 58:
        return b >= 119;
      case 59:
        return b >= 114;
      case 60:
        return b >= 109;
      case 61:
        return b >= 104;
      case 62:
        return b >= 99;
      case 63 /*0x3F*/:
        return b >= 94;
      case 64 /*0x40*/:
        return b >= 89;
      case 65:
        return b >= 83;
      case 66:
        return b >= 78;
      case 67:
        return b >= 73;
      case 68:
        return b >= 68;
      case 69:
        return b >= 63 /*0x3F*/;
      case 70:
        return b >= 58;
      case 71:
        return b >= 53;
      case 72:
        return b >= 47;
      case 73:
        return b >= 42;
      case 74:
        return b >= 37;
      case 75:
        return b >= 32 /*0x20*/;
      case 76:
        return b >= 27;
      case 77:
        return b >= 22;
      case 78:
        return b >= 16 /*0x10*/;
      case 79:
        return b >= 11;
      case 80 /*0x50*/:
        return b >= 6;
      case 81:
        return b >= 1;
      default:
        return g >= 82;
    }
  }

  private static bool CheckR45(int g, int b)
  {
    switch (g)
    {
      case 32 /*0x20*/:
        return b >= 251;
      case 33:
        return b >= 246;
      case 34:
        return b >= 240 /*0xF0*/;
      case 35:
        return b >= 235;
      case 36:
        return b >= 230;
      case 37:
        return b >= 225;
      case 38:
        return b >= 220;
      case 39:
        return b >= 215;
      case 40:
        return b >= 210;
      case 41:
        return b >= 204;
      case 42:
        return b >= 199;
      case 43:
        return b >= 194;
      case 44:
        return b >= 189;
      case 45:
        return b >= 184;
      case 46:
        return b >= 179;
      case 47:
        return b >= 173;
      case 48 /*0x30*/:
        return b >= 168;
      case 49:
        return b >= 163;
      case 50:
        return b >= 158;
      case 51:
        return b >= 153;
      case 52:
        return b >= 148;
      case 53:
        return b >= 143;
      case 54:
        return b >= 137;
      case 55:
        return b >= 132;
      case 56:
        return b >= (int) sbyte.MaxValue;
      case 57:
        return b >= 122;
      case 58:
        return b >= 117;
      case 59:
        return b >= 112 /*0x70*/;
      case 60:
        return b >= 107;
      case 61:
        return b >= 101;
      case 62:
        return b >= 96 /*0x60*/;
      case 63 /*0x3F*/:
        return b >= 91;
      case 64 /*0x40*/:
        return b >= 86;
      case 65:
        return b >= 81;
      case 66:
        return b >= 76;
      case 67:
        return b >= 70;
      case 68:
        return b >= 65;
      case 69:
        return b >= 60;
      case 70:
        return b >= 55;
      case 71:
        return b >= 50;
      case 72:
        return b >= 45;
      case 73:
        return b >= 40;
      case 74:
        return b >= 34;
      case 75:
        return b >= 29;
      case 76:
        return b >= 24;
      case 77:
        return b >= 19;
      case 78:
        return b >= 14;
      case 79:
        return b >= 9;
      case 80 /*0x50*/:
        return b >= 4;
      default:
        return g >= 81;
    }
  }

  private static bool CheckR46(int g, int b)
  {
    switch (g)
    {
      case 31 /*0x1F*/:
        return b >= 253;
      case 32 /*0x20*/:
        return b >= 248;
      case 33:
        return b >= 243;
      case 34:
        return b >= 238;
      case 35:
        return b >= 233;
      case 36:
        return b >= 227;
      case 37:
        return b >= 222;
      case 38:
        return b >= 217;
      case 39:
        return b >= 212;
      case 40:
        return b >= 207;
      case 41:
        return b >= 202;
      case 42:
        return b >= 197;
      case 43:
        return b >= 191;
      case 44:
        return b >= 186;
      case 45:
        return b >= 181;
      case 46:
        return b >= 176 /*0xB0*/;
      case 47:
        return b >= 171;
      case 48 /*0x30*/:
        return b >= 166;
      case 49:
        return b >= 161;
      case 50:
        return b >= 155;
      case 51:
        return b >= 150;
      case 52:
        return b >= 145;
      case 53:
        return b >= 140;
      case 54:
        return b >= 135;
      case 55:
        return b >= 130;
      case 56:
        return b >= 124;
      case 57:
        return b >= 119;
      case 58:
        return b >= 114;
      case 59:
        return b >= 109;
      case 60:
        return b >= 104;
      case 61:
        return b >= 99;
      case 62:
        return b >= 94;
      case 63 /*0x3F*/:
        return b >= 88;
      case 64 /*0x40*/:
        return b >= 83;
      case 65:
        return b >= 78;
      case 66:
        return b >= 73;
      case 67:
        return b >= 68;
      case 68:
        return b >= 63 /*0x3F*/;
      case 69:
        return b >= 58;
      case 70:
        return b >= 52;
      case 71:
        return b >= 47;
      case 72:
        return b >= 42;
      case 73:
        return b >= 37;
      case 74:
        return b >= 32 /*0x20*/;
      case 75:
        return b >= 27;
      case 76:
        return b >= 22;
      case 77:
        return b >= 16 /*0x10*/;
      case 78:
        return b >= 11;
      case 79:
        return b >= 6;
      case 80 /*0x50*/:
        return b >= 1;
      default:
        return g >= 81;
    }
  }

  private static bool CheckR47(int g, int b)
  {
    switch (g)
    {
      case 31 /*0x1F*/:
        return b >= 251;
      case 32 /*0x20*/:
        return b >= 245;
      case 33:
        return b >= 240 /*0xF0*/;
      case 34:
        return b >= 235;
      case 35:
        return b >= 230;
      case 36:
        return b >= 225;
      case 37:
        return b >= 220;
      case 38:
        return b >= 215;
      case 39:
        return b >= 209;
      case 40:
        return b >= 204;
      case 41:
        return b >= 199;
      case 42:
        return b >= 194;
      case 43:
        return b >= 189;
      case 44:
        return b >= 184;
      case 45:
        return b >= 179;
      case 46:
        return b >= 173;
      case 47:
        return b >= 168;
      case 48 /*0x30*/:
        return b >= 163;
      case 49:
        return b >= 158;
      case 50:
        return b >= 153;
      case 51:
        return b >= 148;
      case 52:
        return b >= 142;
      case 53:
        return b >= 137;
      case 54:
        return b >= 132;
      case 55:
        return b >= (int) sbyte.MaxValue;
      case 56:
        return b >= 122;
      case 57:
        return b >= 117;
      case 58:
        return b >= 112 /*0x70*/;
      case 59:
        return b >= 106;
      case 60:
        return b >= 101;
      case 61:
        return b >= 96 /*0x60*/;
      case 62:
        return b >= 91;
      case 63 /*0x3F*/:
        return b >= 86;
      case 64 /*0x40*/:
        return b >= 81;
      case 65:
        return b >= 76;
      case 66:
        return b >= 70;
      case 67:
        return b >= 65;
      case 68:
        return b >= 60;
      case 69:
        return b >= 55;
      case 70:
        return b >= 50;
      case 71:
        return b >= 45;
      case 72:
        return b >= 39;
      case 73:
        return b >= 34;
      case 74:
        return b >= 29;
      case 75:
        return b >= 24;
      case 76:
        return b >= 19;
      case 77:
        return b >= 14;
      case 78:
        return b >= 9;
      case 79:
        return b >= 3;
      default:
        return g >= 80 /*0x50*/;
    }
  }

  private static bool CheckR48(int g, int b)
  {
    switch (g)
    {
      case 30:
        return b >= 253;
      case 31 /*0x1F*/:
        return b >= 248;
      case 32 /*0x20*/:
        return b >= 243;
      case 33:
        return b >= 238;
      case 34:
        return b >= 233;
      case 35:
        return b >= 227;
      case 36:
        return b >= 222;
      case 37:
        return b >= 217;
      case 38:
        return b >= 212;
      case 39:
        return b >= 207;
      case 40:
        return b >= 202;
      case 41:
        return b >= 196;
      case 42:
        return b >= 191;
      case 43:
        return b >= 186;
      case 44:
        return b >= 181;
      case 45:
        return b >= 176 /*0xB0*/;
      case 46:
        return b >= 171;
      case 47:
        return b >= 166;
      case 48 /*0x30*/:
        return b >= 160 /*0xA0*/;
      case 49:
        return b >= 155;
      case 50:
        return b >= 150;
      case 51:
        return b >= 145;
      case 52:
        return b >= 140;
      case 53:
        return b >= 135;
      case 54:
        return b >= 130;
      case 55:
        return b >= 124;
      case 56:
        return b >= 119;
      case 57:
        return b >= 114;
      case 58:
        return b >= 109;
      case 59:
        return b >= 104;
      case 60:
        return b >= 99;
      case 61:
        return b >= 94;
      case 62:
        return b >= 88;
      case 63 /*0x3F*/:
        return b >= 83;
      case 64 /*0x40*/:
        return b >= 78;
      case 65:
        return b >= 73;
      case 66:
        return b >= 68;
      case 67:
        return b >= 63 /*0x3F*/;
      case 68:
        return b >= 57;
      case 69:
        return b >= 52;
      case 70:
        return b >= 47;
      case 71:
        return b >= 42;
      case 72:
        return b >= 37;
      case 73:
        return b >= 32 /*0x20*/;
      case 74:
        return b >= 27;
      case 75:
        return b >= 21;
      case 76:
        return b >= 16 /*0x10*/;
      case 77:
        return b >= 11;
      case 78:
        return b >= 6;
      case 79:
        return b >= 1;
      default:
        return g >= 80 /*0x50*/;
    }
  }

  private static bool CheckR49(int g, int b)
  {
    switch (g)
    {
      case 30:
        return b >= 251;
      case 31 /*0x1F*/:
        return b >= 245;
      case 32 /*0x20*/:
        return b >= 240 /*0xF0*/;
      case 33:
        return b >= 235;
      case 34:
        return b >= 230;
      case 35:
        return b >= 225;
      case 36:
        return b >= 220;
      case 37:
        return b >= 214;
      case 38:
        return b >= 209;
      case 39:
        return b >= 204;
      case 40:
        return b >= 199;
      case 41:
        return b >= 194;
      case 42:
        return b >= 189;
      case 43:
        return b >= 184;
      case 44:
        return b >= 178;
      case 45:
        return b >= 173;
      case 46:
        return b >= 168;
      case 47:
        return b >= 163;
      case 48 /*0x30*/:
        return b >= 158;
      case 49:
        return b >= 153;
      case 50:
        return b >= 148;
      case 51:
        return b >= 142;
      case 52:
        return b >= 137;
      case 53:
        return b >= 132;
      case 54:
        return b >= (int) sbyte.MaxValue;
      case 55:
        return b >= 122;
      case 56:
        return b >= 117;
      case 57:
        return b >= 111;
      case 58:
        return b >= 106;
      case 59:
        return b >= 101;
      case 60:
        return b >= 96 /*0x60*/;
      case 61:
        return b >= 91;
      case 62:
        return b >= 86;
      case 63 /*0x3F*/:
        return b >= 81;
      case 64 /*0x40*/:
        return b >= 75;
      case 65:
        return b >= 70;
      case 66:
        return b >= 65;
      case 67:
        return b >= 60;
      case 68:
        return b >= 55;
      case 69:
        return b >= 50;
      case 70:
        return b >= 45;
      case 71:
        return b >= 39;
      case 72:
        return b >= 34;
      case 73:
        return b >= 29;
      case 74:
        return b >= 24;
      case 75:
        return b >= 19;
      case 76:
        return b >= 14;
      case 77:
        return b >= 8;
      case 78:
        return b >= 3;
      default:
        return g >= 79;
    }
  }

  private static bool CheckR50(int g, int b)
  {
    switch (g)
    {
      case 29:
        return b >= 253;
      case 30:
        return b >= 248;
      case 31 /*0x1F*/:
        return b >= 243;
      case 32 /*0x20*/:
        return b >= 238;
      case 33:
        return b >= 232;
      case 34:
        return b >= 227;
      case 35:
        return b >= 222;
      case 36:
        return b >= 217;
      case 37:
        return b >= 212;
      case 38:
        return b >= 207;
      case 39:
        return b >= 202;
      case 40:
        return b >= 196;
      case 41:
        return b >= 191;
      case 42:
        return b >= 186;
      case 43:
        return b >= 181;
      case 44:
        return b >= 176 /*0xB0*/;
      case 45:
        return b >= 171;
      case 46:
        return b >= 165;
      case 47:
        return b >= 160 /*0xA0*/;
      case 48 /*0x30*/:
        return b >= 155;
      case 49:
        return b >= 150;
      case 50:
        return b >= 145;
      case 51:
        return b >= 140;
      case 52:
        return b >= 135;
      case 53:
        return b >= 129;
      case 54:
        return b >= 124;
      case 55:
        return b >= 119;
      case 56:
        return b >= 114;
      case 57:
        return b >= 109;
      case 58:
        return b >= 104;
      case 59:
        return b >= 99;
      case 60:
        return b >= 93;
      case 61:
        return b >= 88;
      case 62:
        return b >= 83;
      case 63 /*0x3F*/:
        return b >= 78;
      case 64 /*0x40*/:
        return b >= 73;
      case 65:
        return b >= 68;
      case 66:
        return b >= 63 /*0x3F*/;
      case 67:
        return b >= 57;
      case 68:
        return b >= 52;
      case 69:
        return b >= 47;
      case 70:
        return b >= 42;
      case 71:
        return b >= 37;
      case 72:
        return b >= 32 /*0x20*/;
      case 73:
        return b >= 26;
      case 74:
        return b >= 21;
      case 75:
        return b >= 16 /*0x10*/;
      case 76:
        return b >= 11;
      case 77:
        return b >= 6;
      case 78:
        return b >= 1;
      default:
        return g >= 79;
    }
  }

  private static bool CheckR51(int g, int b)
  {
    switch (g)
    {
      case 29:
        return b >= 250;
      case 30:
        return b >= 245;
      case 31 /*0x1F*/:
        return b >= 240 /*0xF0*/;
      case 32 /*0x20*/:
        return b >= 235;
      case 33:
        return b >= 230;
      case 34:
        return b >= 225;
      case 35:
        return b >= 220;
      case 36:
        return b >= 214;
      case 37:
        return b >= 209;
      case 38:
        return b >= 204;
      case 39:
        return b >= 199;
      case 40:
        return b >= 194;
      case 41:
        return b >= 189;
      case 42:
        return b >= 183;
      case 43:
        return b >= 178;
      case 44:
        return b >= 173;
      case 45:
        return b >= 168;
      case 46:
        return b >= 163;
      case 47:
        return b >= 158;
      case 48 /*0x30*/:
        return b >= 153;
      case 49:
        return b >= 147;
      case 50:
        return b >= 142;
      case 51:
        return b >= 137;
      case 52:
        return b >= 132;
      case 53:
        return b >= (int) sbyte.MaxValue;
      case 54:
        return b >= 122;
      case 55:
        return b >= 117;
      case 56:
        return b >= 111;
      case 57:
        return b >= 106;
      case 58:
        return b >= 101;
      case 59:
        return b >= 96 /*0x60*/;
      case 60:
        return b >= 91;
      case 61:
        return b >= 86;
      case 62:
        return b >= 80 /*0x50*/;
      case 63 /*0x3F*/:
        return b >= 75;
      case 64 /*0x40*/:
        return b >= 70;
      case 65:
        return b >= 65;
      case 66:
        return b >= 60;
      case 67:
        return b >= 55;
      case 68:
        return b >= 50;
      case 69:
        return b >= 44;
      case 70:
        return b >= 39;
      case 71:
        return b >= 34;
      case 72:
        return b >= 29;
      case 73:
        return b >= 24;
      case 74:
        return b >= 19;
      case 75:
        return b >= 14;
      case 76:
        return b >= 8;
      case 77:
        return b >= 3;
      default:
        return g >= 78;
    }
  }

  private static bool CheckR52(int g, int b)
  {
    switch (g)
    {
      case 28:
        return b >= 253;
      case 29:
        return b >= 248;
      case 30:
        return b >= 243;
      case 31 /*0x1F*/:
        return b >= 237;
      case 32 /*0x20*/:
        return b >= 232;
      case 33:
        return b >= 227;
      case 34:
        return b >= 222;
      case 35:
        return b >= 217;
      case 36:
        return b >= 212;
      case 37:
        return b >= 207;
      case 38:
        return b >= 201;
      case 39:
        return b >= 196;
      case 40:
        return b >= 191;
      case 41:
        return b >= 186;
      case 42:
        return b >= 181;
      case 43:
        return b >= 176 /*0xB0*/;
      case 44:
        return b >= 171;
      case 45:
        return b >= 165;
      case 46:
        return b >= 160 /*0xA0*/;
      case 47:
        return b >= 155;
      case 48 /*0x30*/:
        return b >= 150;
      case 49:
        return b >= 145;
      case 50:
        return b >= 140;
      case 51:
        return b >= 135;
      case 52:
        return b >= 129;
      case 53:
        return b >= 124;
      case 54:
        return b >= 119;
      case 55:
        return b >= 114;
      case 56:
        return b >= 109;
      case 57:
        return b >= 104;
      case 58:
        return b >= 98;
      case 59:
        return b >= 93;
      case 60:
        return b >= 88;
      case 61:
        return b >= 83;
      case 62:
        return b >= 78;
      case 63 /*0x3F*/:
        return b >= 73;
      case 64 /*0x40*/:
        return b >= 68;
      case 65:
        return b >= 62;
      case 66:
        return b >= 57;
      case 67:
        return b >= 52;
      case 68:
        return b >= 47;
      case 69:
        return b >= 42;
      case 70:
        return b >= 37;
      case 71:
        return b >= 32 /*0x20*/;
      case 72:
        return b >= 26;
      case 73:
        return b >= 21;
      case 74:
        return b >= 16 /*0x10*/;
      case 75:
        return b >= 11;
      case 76:
        return b >= 6;
      case 77:
        return b >= 1;
      default:
        return g >= 78;
    }
  }

  private static bool CheckR53(int g, int b)
  {
    switch (g)
    {
      case 27:
        return b >= (int) byte.MaxValue;
      case 28:
        return b >= 250;
      case 29:
        return b >= 245;
      case 30:
        return b >= 240 /*0xF0*/;
      case 31 /*0x1F*/:
        return b >= 235;
      case 32 /*0x20*/:
        return b >= 230;
      case 33:
        return b >= 225;
      case 34:
        return b >= 219;
      case 35:
        return b >= 214;
      case 36:
        return b >= 209;
      case 37:
        return b >= 204;
      case 38:
        return b >= 199;
      case 39:
        return b >= 194;
      case 40:
        return b >= 189;
      case 41:
        return b >= 183;
      case 42:
        return b >= 178;
      case 43:
        return b >= 173;
      case 44:
        return b >= 168;
      case 45:
        return b >= 163;
      case 46:
        return b >= 158;
      case 47:
        return b >= 152;
      case 48 /*0x30*/:
        return b >= 147;
      case 49:
        return b >= 142;
      case 50:
        return b >= 137;
      case 51:
        return b >= 132;
      case 52:
        return b >= (int) sbyte.MaxValue;
      case 53:
        return b >= 122;
      case 54:
        return b >= 116;
      case 55:
        return b >= 111;
      case 56:
        return b >= 106;
      case 57:
        return b >= 101;
      case 58:
        return b >= 96 /*0x60*/;
      case 59:
        return b >= 91;
      case 60:
        return b >= 86;
      case 61:
        return b >= 80 /*0x50*/;
      case 62:
        return b >= 75;
      case 63 /*0x3F*/:
        return b >= 70;
      case 64 /*0x40*/:
        return b >= 65;
      case 65:
        return b >= 60;
      case 66:
        return b >= 55;
      case 67:
        return b >= 49;
      case 68:
        return b >= 44;
      case 69:
        return b >= 39;
      case 70:
        return b >= 34;
      case 71:
        return b >= 29;
      case 72:
        return b >= 24;
      case 73:
        return b >= 19;
      case 74:
        return b >= 13;
      case 75:
        return b >= 8;
      case 76:
        return b >= 3;
      default:
        return g >= 77;
    }
  }

  private static bool CheckR54(int g, int b)
  {
    switch (g)
    {
      case 27:
        return b >= 253;
      case 28:
        return b >= 248;
      case 29:
        return b >= 243;
      case 30:
        return b >= 237;
      case 31 /*0x1F*/:
        return b >= 232;
      case 32 /*0x20*/:
        return b >= 227;
      case 33:
        return b >= 222;
      case 34:
        return b >= 217;
      case 35:
        return b >= 212;
      case 36:
        return b >= 206;
      case 37:
        return b >= 201;
      case 38:
        return b >= 196;
      case 39:
        return b >= 191;
      case 40:
        return b >= 186;
      case 41:
        return b >= 181;
      case 42:
        return b >= 176 /*0xB0*/;
      case 43:
        return b >= 170;
      case 44:
        return b >= 165;
      case 45:
        return b >= 160 /*0xA0*/;
      case 46:
        return b >= 155;
      case 47:
        return b >= 150;
      case 48 /*0x30*/:
        return b >= 145;
      case 49:
        return b >= 140;
      case 50:
        return b >= 134;
      case 51:
        return b >= 129;
      case 52:
        return b >= 124;
      case 53:
        return b >= 119;
      case 54:
        return b >= 114;
      case 55:
        return b >= 109;
      case 56:
        return b >= 104;
      case 57:
        return b >= 98;
      case 58:
        return b >= 93;
      case 59:
        return b >= 88;
      case 60:
        return b >= 83;
      case 61:
        return b >= 78;
      case 62:
        return b >= 73;
      case 63 /*0x3F*/:
        return b >= 67;
      case 64 /*0x40*/:
        return b >= 62;
      case 65:
        return b >= 57;
      case 66:
        return b >= 52;
      case 67:
        return b >= 47;
      case 68:
        return b >= 42;
      case 69:
        return b >= 37;
      case 70:
        return b >= 31 /*0x1F*/;
      case 71:
        return b >= 26;
      case 72:
        return b >= 21;
      case 73:
        return b >= 16 /*0x10*/;
      case 74:
        return b >= 11;
      case 75:
        return b >= 6;
      case 76:
        return b >= 1;
      default:
        return g >= 77;
    }
  }

  private static bool CheckR55(int g, int b)
  {
    switch (g)
    {
      case 26:
        return b >= (int) byte.MaxValue;
      case 27:
        return b >= 250;
      case 28:
        return b >= 245;
      case 29:
        return b >= 240 /*0xF0*/;
      case 30:
        return b >= 235;
      case 31 /*0x1F*/:
        return b >= 230;
      case 32 /*0x20*/:
        return b >= 224 /*0xE0*/;
      case 33:
        return b >= 219;
      case 34:
        return b >= 214;
      case 35:
        return b >= 209;
      case 36:
        return b >= 204;
      case 37:
        return b >= 199;
      case 38:
        return b >= 194;
      case 39:
        return b >= 188;
      case 40:
        return b >= 183;
      case 41:
        return b >= 178;
      case 42:
        return b >= 173;
      case 43:
        return b >= 168;
      case 44:
        return b >= 163;
      case 45:
        return b >= 158;
      case 46:
        return b >= 152;
      case 47:
        return b >= 147;
      case 48 /*0x30*/:
        return b >= 142;
      case 49:
        return b >= 137;
      case 50:
        return b >= 132;
      case 51:
        return b >= (int) sbyte.MaxValue;
      case 52:
        return b >= 121;
      case 53:
        return b >= 116;
      case 54:
        return b >= 111;
      case 55:
        return b >= 106;
      case 56:
        return b >= 101;
      case 57:
        return b >= 96 /*0x60*/;
      case 58:
        return b >= 91;
      case 59:
        return b >= 85;
      case 60:
        return b >= 80 /*0x50*/;
      case 61:
        return b >= 75;
      case 62:
        return b >= 70;
      case 63 /*0x3F*/:
        return b >= 65;
      case 64 /*0x40*/:
        return b >= 60;
      case 65:
        return b >= 55;
      case 66:
        return b >= 49;
      case 67:
        return b >= 44;
      case 68:
        return b >= 39;
      case 69:
        return b >= 34;
      case 70:
        return b >= 29;
      case 71:
        return b >= 24;
      case 72:
        return b >= 19;
      case 73:
        return b >= 13;
      case 74:
        return b >= 8;
      case 75:
        return b >= 3;
      default:
        return g >= 76;
    }
  }

  private static bool CheckR56(int g, int b)
  {
    switch (g)
    {
      case 26:
        return b >= 253;
      case 27:
        return b >= 248;
      case 28:
        return b >= 242;
      case 29:
        return b >= 237;
      case 30:
        return b >= 232;
      case 31 /*0x1F*/:
        return b >= 227;
      case 32 /*0x20*/:
        return b >= 222;
      case 33:
        return b >= 217;
      case 34:
        return b >= 212;
      case 35:
        return b >= 206;
      case 36:
        return b >= 201;
      case 37:
        return b >= 196;
      case 38:
        return b >= 191;
      case 39:
        return b >= 186;
      case 40:
        return b >= 181;
      case 41:
        return b >= 176 /*0xB0*/;
      case 42:
        return b >= 170;
      case 43:
        return b >= 165;
      case 44:
        return b >= 160 /*0xA0*/;
      case 45:
        return b >= 155;
      case 46:
        return b >= 150;
      case 47:
        return b >= 145;
      case 48 /*0x30*/:
        return b >= 139;
      case 49:
        return b >= 134;
      case 50:
        return b >= 129;
      case 51:
        return b >= 124;
      case 52:
        return b >= 119;
      case 53:
        return b >= 114;
      case 54:
        return b >= 109;
      case 55:
        return b >= 103;
      case 56:
        return b >= 98;
      case 57:
        return b >= 93;
      case 58:
        return b >= 88;
      case 59:
        return b >= 83;
      case 60:
        return b >= 78;
      case 61:
        return b >= 73;
      case 62:
        return b >= 67;
      case 63 /*0x3F*/:
        return b >= 62;
      case 64 /*0x40*/:
        return b >= 57;
      case 65:
        return b >= 52;
      case 66:
        return b >= 47;
      case 67:
        return b >= 42;
      case 68:
        return b >= 36;
      case 69:
        return b >= 31 /*0x1F*/;
      case 70:
        return b >= 26;
      case 71:
        return b >= 21;
      case 72:
        return b >= 16 /*0x10*/;
      case 73:
        return b >= 11;
      case 74:
        return b >= 6;
      default:
        return g >= 75;
    }
  }

  private static bool CheckR57(int g, int b)
  {
    switch (g)
    {
      case 25:
        return b >= (int) byte.MaxValue;
      case 26:
        return b >= 250;
      case 27:
        return b >= 245;
      case 28:
        return b >= 240 /*0xF0*/;
      case 29:
        return b >= 235;
      case 30:
        return b >= 230;
      case 31 /*0x1F*/:
        return b >= 224 /*0xE0*/;
      case 32 /*0x20*/:
        return b >= 219;
      case 33:
        return b >= 214;
      case 34:
        return b >= 209;
      case 35:
        return b >= 204;
      case 36:
        return b >= 199;
      case 37:
        return b >= 193;
      case 38:
        return b >= 188;
      case 39:
        return b >= 183;
      case 40:
        return b >= 178;
      case 41:
        return b >= 173;
      case 42:
        return b >= 168;
      case 43:
        return b >= 163;
      case 44:
        return b >= 157;
      case 45:
        return b >= 152;
      case 46:
        return b >= 147;
      case 47:
        return b >= 142;
      case 48 /*0x30*/:
        return b >= 137;
      case 49:
        return b >= 132;
      case 50:
        return b >= (int) sbyte.MaxValue;
      case 51:
        return b >= 121;
      case 52:
        return b >= 116;
      case 53:
        return b >= 111;
      case 54:
        return b >= 106;
      case 55:
        return b >= 101;
      case 56:
        return b >= 96 /*0x60*/;
      case 57:
        return b >= 90;
      case 58:
        return b >= 85;
      case 59:
        return b >= 80 /*0x50*/;
      case 60:
        return b >= 75;
      case 61:
        return b >= 70;
      case 62:
        return b >= 65;
      case 63 /*0x3F*/:
        return b >= 60;
      case 64 /*0x40*/:
        return b >= 54;
      case 65:
        return b >= 49;
      case 66:
        return b >= 44;
      case 67:
        return b >= 39;
      case 68:
        return b >= 34;
      case 69:
        return b >= 29;
      case 70:
        return b >= 24;
      case 71:
        return b >= 18;
      case 72:
        return b >= 13;
      case 73:
        return b >= 8;
      case 74:
        return b >= 3;
      default:
        return g >= 75;
    }
  }

  private static bool CheckR58(int g, int b)
  {
    switch (g)
    {
      case 25:
        return b >= 253;
      case 26:
        return b >= 247;
      case 27:
        return b >= 242;
      case 28:
        return b >= 237;
      case 29:
        return b >= 232;
      case 30:
        return b >= 227;
      case 31 /*0x1F*/:
        return b >= 222;
      case 32 /*0x20*/:
        return b >= 217;
      case 33:
        return b >= 211;
      case 34:
        return b >= 206;
      case 35:
        return b >= 201;
      case 36:
        return b >= 196;
      case 37:
        return b >= 191;
      case 38:
        return b >= 186;
      case 39:
        return b >= 181;
      case 40:
        return b >= 175;
      case 41:
        return b >= 170;
      case 42:
        return b >= 165;
      case 43:
        return b >= 160 /*0xA0*/;
      case 44:
        return b >= 155;
      case 45:
        return b >= 150;
      case 46:
        return b >= 145;
      case 47:
        return b >= 139;
      case 48 /*0x30*/:
        return b >= 134;
      case 49:
        return b >= 129;
      case 50:
        return b >= 124;
      case 51:
        return b >= 119;
      case 52:
        return b >= 114;
      case 53:
        return b >= 108;
      case 54:
        return b >= 103;
      case 55:
        return b >= 98;
      case 56:
        return b >= 93;
      case 57:
        return b >= 88;
      case 58:
        return b >= 83;
      case 59:
        return b >= 78;
      case 60:
        return b >= 72;
      case 61:
        return b >= 67;
      case 62:
        return b >= 62;
      case 63 /*0x3F*/:
        return b >= 57;
      case 64 /*0x40*/:
        return b >= 52;
      case 65:
        return b >= 47;
      case 66:
        return b >= 42;
      case 67:
        return b >= 36;
      case 68:
        return b >= 31 /*0x1F*/;
      case 69:
        return b >= 26;
      case 70:
        return b >= 21;
      case 71:
        return b >= 16 /*0x10*/;
      case 72:
        return b >= 11;
      case 73:
        return b >= 5;
      default:
        return g >= 74;
    }
  }

  private static bool CheckR59(int g, int b)
  {
    switch (g)
    {
      case 24:
        return b >= (int) byte.MaxValue;
      case 25:
        return b >= 250;
      case 26:
        return b >= 245;
      case 27:
        return b >= 240 /*0xF0*/;
      case 28:
        return b >= 235;
      case 29:
        return b >= 229;
      case 30:
        return b >= 224 /*0xE0*/;
      case 31 /*0x1F*/:
        return b >= 219;
      case 32 /*0x20*/:
        return b >= 214;
      case 33:
        return b >= 209;
      case 34:
        return b >= 204;
      case 35:
        return b >= 199;
      case 36:
        return b >= 193;
      case 37:
        return b >= 188;
      case 38:
        return b >= 183;
      case 39:
        return b >= 178;
      case 40:
        return b >= 173;
      case 41:
        return b >= 168;
      case 42:
        return b >= 162;
      case 43:
        return b >= 157;
      case 44:
        return b >= 152;
      case 45:
        return b >= 147;
      case 46:
        return b >= 142;
      case 47:
        return b >= 137;
      case 48 /*0x30*/:
        return b >= 132;
      case 49:
        return b >= 126;
      case 50:
        return b >= 121;
      case 51:
        return b >= 116;
      case 52:
        return b >= 111;
      case 53:
        return b >= 106;
      case 54:
        return b >= 101;
      case 55:
        return b >= 96 /*0x60*/;
      case 56:
        return b >= 90;
      case 57:
        return b >= 85;
      case 58:
        return b >= 80 /*0x50*/;
      case 59:
        return b >= 75;
      case 60:
        return b >= 70;
      case 61:
        return b >= 65;
      case 62:
        return b >= 60;
      case 63 /*0x3F*/:
        return b >= 54;
      case 64 /*0x40*/:
        return b >= 49;
      case 65:
        return b >= 44;
      case 66:
        return b >= 39;
      case 67:
        return b >= 34;
      case 68:
        return b >= 29;
      case 69:
        return b >= 23;
      case 70:
        return b >= 18;
      case 71:
        return b >= 13;
      case 72:
        return b >= 8;
      case 73:
        return b >= 3;
      default:
        return g >= 74;
    }
  }

  private static bool CheckR60(int g, int b)
  {
    switch (g)
    {
      case 24:
        return b >= 253;
      case 25:
        return b >= 247;
      case 26:
        return b >= 242;
      case 27:
        return b >= 237;
      case 28:
        return b >= 232;
      case 29:
        return b >= 227;
      case 30:
        return b >= 222;
      case 31 /*0x1F*/:
        return b >= 217;
      case 32 /*0x20*/:
        return b >= 211;
      case 33:
        return b >= 206;
      case 34:
        return b >= 201;
      case 35:
        return b >= 196;
      case 36:
        return b >= 191;
      case 37:
        return b >= 186;
      case 38:
        return b >= 180;
      case 39:
        return b >= 175;
      case 40:
        return b >= 170;
      case 41:
        return b >= 165;
      case 42:
        return b >= 160 /*0xA0*/;
      case 43:
        return b >= 155;
      case 44:
        return b >= 150;
      case 45:
        return b >= 144 /*0x90*/;
      case 46:
        return b >= 139;
      case 47:
        return b >= 134;
      case 48 /*0x30*/:
        return b >= 129;
      case 49:
        return b >= 124;
      case 50:
        return b >= 119;
      case 51:
        return b >= 114;
      case 52:
        return b >= 108;
      case 53:
        return b >= 103;
      case 54:
        return b >= 98;
      case 55:
        return b >= 93;
      case 56:
        return b >= 88;
      case 57:
        return b >= 83;
      case 58:
        return b >= 77;
      case 59:
        return b >= 72;
      case 60:
        return b >= 67;
      case 61:
        return b >= 62;
      case 62:
        return b >= 57;
      case 63 /*0x3F*/:
        return b >= 52;
      case 64 /*0x40*/:
        return b >= 47;
      case 65:
        return b >= 41;
      case 66:
        return b >= 36;
      case 67:
        return b >= 31 /*0x1F*/;
      case 68:
        return b >= 26;
      case 69:
        return b >= 21;
      case 70:
        return b >= 16 /*0x10*/;
      case 71:
        return b >= 11;
      case 72:
        return b >= 5;
      default:
        return g >= 73;
    }
  }

  private static bool CheckR61(int g, int b)
  {
    switch (g)
    {
      case 23:
        return b >= (int) byte.MaxValue;
      case 24:
        return b >= 250;
      case 25:
        return b >= 245;
      case 26:
        return b >= 240 /*0xF0*/;
      case 27:
        return b >= 234;
      case 28:
        return b >= 229;
      case 29:
        return b >= 224 /*0xE0*/;
      case 30:
        return b >= 219;
      case 31 /*0x1F*/:
        return b >= 214;
      case 32 /*0x20*/:
        return b >= 209;
      case 33:
        return b >= 204;
      case 34:
        return b >= 198;
      case 35:
        return b >= 193;
      case 36:
        return b >= 188;
      case 37:
        return b >= 183;
      case 38:
        return b >= 178;
      case 39:
        return b >= 173;
      case 40:
        return b >= 168;
      case 41:
        return b >= 162;
      case 42:
        return b >= 157;
      case 43:
        return b >= 152;
      case 44:
        return b >= 147;
      case 45:
        return b >= 142;
      case 46:
        return b >= 137;
      case 47:
        return b >= 131;
      case 48 /*0x30*/:
        return b >= 126;
      case 49:
        return b >= 121;
      case 50:
        return b >= 116;
      case 51:
        return b >= 111;
      case 52:
        return b >= 106;
      case 53:
        return b >= 101;
      case 54:
        return b >= 95;
      case 55:
        return b >= 90;
      case 56:
        return b >= 85;
      case 57:
        return b >= 80 /*0x50*/;
      case 58:
        return b >= 75;
      case 59:
        return b >= 70;
      case 60:
        return b >= 65;
      case 61:
        return b >= 59;
      case 62:
        return b >= 54;
      case 63 /*0x3F*/:
        return b >= 49;
      case 64 /*0x40*/:
        return b >= 44;
      case 65:
        return b >= 39;
      case 66:
        return b >= 34;
      case 67:
        return b >= 29;
      case 68:
        return b >= 23;
      case 69:
        return b >= 18;
      case 70:
        return b >= 13;
      case 71:
        return b >= 8;
      case 72:
        return b >= 3;
      default:
        return g >= 73;
    }
  }

  private static bool CheckR62(int g, int b)
  {
    switch (g)
    {
      case 23:
        return b >= 252;
      case 24:
        return b >= 247;
      case 25:
        return b >= 242;
      case 26:
        return b >= 237;
      case 27:
        return b >= 232;
      case 28:
        return b >= 227;
      case 29:
        return b >= 222;
      case 30:
        return b >= 216;
      case 31 /*0x1F*/:
        return b >= 211;
      case 32 /*0x20*/:
        return b >= 206;
      case 33:
        return b >= 201;
      case 34:
        return b >= 196;
      case 35:
        return b >= 191;
      case 36:
        return b >= 186;
      case 37:
        return b >= 180;
      case 38:
        return b >= 175;
      case 39:
        return b >= 170;
      case 40:
        return b >= 165;
      case 41:
        return b >= 160 /*0xA0*/;
      case 42:
        return b >= 155;
      case 43:
        return b >= 149;
      case 44:
        return b >= 144 /*0x90*/;
      case 45:
        return b >= 139;
      case 46:
        return b >= 134;
      case 47:
        return b >= 129;
      case 48 /*0x30*/:
        return b >= 124;
      case 49:
        return b >= 119;
      case 50:
        return b >= 113;
      case 51:
        return b >= 108;
      case 52:
        return b >= 103;
      case 53:
        return b >= 98;
      case 54:
        return b >= 93;
      case 55:
        return b >= 88;
      case 56:
        return b >= 83;
      case 57:
        return b >= 77;
      case 58:
        return b >= 72;
      case 59:
        return b >= 67;
      case 60:
        return b >= 62;
      case 61:
        return b >= 57;
      case 62:
        return b >= 52;
      case 63 /*0x3F*/:
        return b >= 46;
      case 64 /*0x40*/:
        return b >= 41;
      case 65:
        return b >= 36;
      case 66:
        return b >= 31 /*0x1F*/;
      case 67:
        return b >= 26;
      case 68:
        return b >= 21;
      case 69:
        return b >= 16 /*0x10*/;
      case 70:
        return b >= 10;
      case 71:
        return b >= 5;
      default:
        return g >= 72;
    }
  }

  private static bool CheckR63(int g, int b)
  {
    switch (g)
    {
      case 22:
        return b >= (int) byte.MaxValue;
      case 23:
        return b >= 250;
      case 24:
        return b >= 245;
      case 25:
        return b >= 240 /*0xF0*/;
      case 26:
        return b >= 234;
      case 27:
        return b >= 229;
      case 28:
        return b >= 224 /*0xE0*/;
      case 29:
        return b >= 219;
      case 30:
        return b >= 214;
      case 31 /*0x1F*/:
        return b >= 209;
      case 32 /*0x20*/:
        return b >= 203;
      case 33:
        return b >= 198;
      case 34:
        return b >= 193;
      case 35:
        return b >= 188;
      case 36:
        return b >= 183;
      case 37:
        return b >= 178;
      case 38:
        return b >= 173;
      case 39:
        return b >= 167;
      case 40:
        return b >= 162;
      case 41:
        return b >= 157;
      case 42:
        return b >= 152;
      case 43:
        return b >= 147;
      case 44:
        return b >= 142;
      case 45:
        return b >= 137;
      case 46:
        return b >= 131;
      case 47:
        return b >= 126;
      case 48 /*0x30*/:
        return b >= 121;
      case 49:
        return b >= 116;
      case 50:
        return b >= 111;
      case 51:
        return b >= 106;
      case 52:
        return b >= 101;
      case 53:
        return b >= 95;
      case 54:
        return b >= 90;
      case 55:
        return b >= 85;
      case 56:
        return b >= 80 /*0x50*/;
      case 57:
        return b >= 75;
      case 58:
        return b >= 70;
      case 59:
        return b >= 64 /*0x40*/;
      case 60:
        return b >= 59;
      case 61:
        return b >= 54;
      case 62:
        return b >= 49;
      case 63 /*0x3F*/:
        return b >= 44;
      case 64 /*0x40*/:
        return b >= 39;
      case 65:
        return b >= 34;
      case 66:
        return b >= 28;
      case 67:
        return b >= 23;
      case 68:
        return b >= 18;
      case 69:
        return b >= 13;
      case 70:
        return b >= 8;
      case 71:
        return b >= 3;
      default:
        return g >= 72;
    }
  }

  private static bool CheckR64(int g, int b)
  {
    switch (g)
    {
      case 22:
        return b >= 252;
      case 23:
        return b >= 247;
      case 24:
        return b >= 242;
      case 25:
        return b >= 237;
      case 26:
        return b >= 232;
      case 27:
        return b >= 227;
      case 28:
        return b >= 221;
      case 29:
        return b >= 216;
      case 30:
        return b >= 211;
      case 31 /*0x1F*/:
        return b >= 206;
      case 32 /*0x20*/:
        return b >= 201;
      case 33:
        return b >= 196;
      case 34:
        return b >= 191;
      case 35:
        return b >= 185;
      case 36:
        return b >= 180;
      case 37:
        return b >= 175;
      case 38:
        return b >= 170;
      case 39:
        return b >= 165;
      case 40:
        return b >= 160 /*0xA0*/;
      case 41:
        return b >= 155;
      case 42:
        return b >= 149;
      case 43:
        return b >= 144 /*0x90*/;
      case 44:
        return b >= 139;
      case 45:
        return b >= 134;
      case 46:
        return b >= 129;
      case 47:
        return b >= 124;
      case 48 /*0x30*/:
        return b >= 118;
      case 49:
        return b >= 113;
      case 50:
        return b >= 108;
      case 51:
        return b >= 103;
      case 52:
        return b >= 98;
      case 53:
        return b >= 93;
      case 54:
        return b >= 88;
      case 55:
        return b >= 82;
      case 56:
        return b >= 77;
      case 57:
        return b >= 72;
      case 58:
        return b >= 67;
      case 59:
        return b >= 62;
      case 60:
        return b >= 57;
      case 61:
        return b >= 52;
      case 62:
        return b >= 46;
      case 63 /*0x3F*/:
        return b >= 41;
      case 64 /*0x40*/:
        return b >= 36;
      case 65:
        return b >= 31 /*0x1F*/;
      case 66:
        return b >= 26;
      case 67:
        return b >= 21;
      case 68:
        return b >= 15;
      case 69:
        return b >= 10;
      case 70:
        return b >= 5;
      default:
        return g >= 71;
    }
  }

  private static bool CheckR65(int g, int b)
  {
    switch (g)
    {
      case 21:
        return b >= (int) byte.MaxValue;
      case 22:
        return b >= 250;
      case 23:
        return b >= 245;
      case 24:
        return b >= 239;
      case 25:
        return b >= 234;
      case 26:
        return b >= 229;
      case 27:
        return b >= 224 /*0xE0*/;
      case 28:
        return b >= 219;
      case 29:
        return b >= 214;
      case 30:
        return b >= 209;
      case 31 /*0x1F*/:
        return b >= 203;
      case 32 /*0x20*/:
        return b >= 198;
      case 33:
        return b >= 193;
      case 34:
        return b >= 188;
      case 35:
        return b >= 183;
      case 36:
        return b >= 178;
      case 37:
        return b >= 172;
      case 38:
        return b >= 167;
      case 39:
        return b >= 162;
      case 40:
        return b >= 157;
      case 41:
        return b >= 152;
      case 42:
        return b >= 147;
      case 43:
        return b >= 142;
      case 44:
        return b >= 136;
      case 45:
        return b >= 131;
      case 46:
        return b >= 126;
      case 47:
        return b >= 121;
      case 48 /*0x30*/:
        return b >= 116;
      case 49:
        return b >= 111;
      case 50:
        return b >= 106;
      case 51:
        return b >= 100;
      case 52:
        return b >= 95;
      case 53:
        return b >= 90;
      case 54:
        return b >= 85;
      case 55:
        return b >= 80 /*0x50*/;
      case 56:
        return b >= 75;
      case 57:
        return b >= 70;
      case 58:
        return b >= 64 /*0x40*/;
      case 59:
        return b >= 59;
      case 60:
        return b >= 54;
      case 61:
        return b >= 49;
      case 62:
        return b >= 44;
      case 63 /*0x3F*/:
        return b >= 39;
      case 64 /*0x40*/:
        return b >= 33;
      case 65:
        return b >= 28;
      case 66:
        return b >= 23;
      case 67:
        return b >= 18;
      case 68:
        return b >= 13;
      case 69:
        return b >= 8;
      case 70:
        return b >= 3;
      default:
        return g >= 71;
    }
  }

  private static bool CheckR66(int g, int b)
  {
    switch (g)
    {
      case 21:
        return b >= 252;
      case 22:
        return b >= 247;
      case 23:
        return b >= 242;
      case 24:
        return b >= 237;
      case 25:
        return b >= 232;
      case 26:
        return b >= 227;
      case 27:
        return b >= 221;
      case 28:
        return b >= 216;
      case 29:
        return b >= 211;
      case 30:
        return b >= 206;
      case 31 /*0x1F*/:
        return b >= 201;
      case 32 /*0x20*/:
        return b >= 196;
      case 33:
        return b >= 190;
      case 34:
        return b >= 185;
      case 35:
        return b >= 180;
      case 36:
        return b >= 175;
      case 37:
        return b >= 170;
      case 38:
        return b >= 165;
      case 39:
        return b >= 160 /*0xA0*/;
      case 40:
        return b >= 154;
      case 41:
        return b >= 149;
      case 42:
        return b >= 144 /*0x90*/;
      case 43:
        return b >= 139;
      case 44:
        return b >= 134;
      case 45:
        return b >= 129;
      case 46:
        return b >= 124;
      case 47:
        return b >= 118;
      case 48 /*0x30*/:
        return b >= 113;
      case 49:
        return b >= 108;
      case 50:
        return b >= 103;
      case 51:
        return b >= 98;
      case 52:
        return b >= 93;
      case 53:
        return b >= 87;
      case 54:
        return b >= 82;
      case 55:
        return b >= 77;
      case 56:
        return b >= 72;
      case 57:
        return b >= 67;
      case 58:
        return b >= 62;
      case 59:
        return b >= 57;
      case 60:
        return b >= 51;
      case 61:
        return b >= 46;
      case 62:
        return b >= 41;
      case 63 /*0x3F*/:
        return b >= 36;
      case 64 /*0x40*/:
        return b >= 31 /*0x1F*/;
      case 65:
        return b >= 26;
      case 66:
        return b >= 21;
      case 67:
        return b >= 15;
      case 68:
        return b >= 10;
      case 69:
        return b >= 5;
      default:
        return g >= 70;
    }
  }

  private static bool CheckR67(int g, int b)
  {
    switch (g)
    {
      case 20:
        return b >= (int) byte.MaxValue;
      case 21:
        return b >= 250;
      case 22:
        return b >= 244;
      case 23:
        return b >= 239;
      case 24:
        return b >= 234;
      case 25:
        return b >= 229;
      case 26:
        return b >= 224 /*0xE0*/;
      case 27:
        return b >= 219;
      case 28:
        return b >= 214;
      case 29:
        return b >= 208 /*0xD0*/;
      case 30:
        return b >= 203;
      case 31 /*0x1F*/:
        return b >= 198;
      case 32 /*0x20*/:
        return b >= 193;
      case 33:
        return b >= 188;
      case 34:
        return b >= 183;
      case 35:
        return b >= 178;
      case 36:
        return b >= 172;
      case 37:
        return b >= 167;
      case 38:
        return b >= 162;
      case 39:
        return b >= 157;
      case 40:
        return b >= 152;
      case 41:
        return b >= 147;
      case 42:
        return b >= 142;
      case 43:
        return b >= 136;
      case 44:
        return b >= 131;
      case 45:
        return b >= 126;
      case 46:
        return b >= 121;
      case 47:
        return b >= 116;
      case 48 /*0x30*/:
        return b >= 111;
      case 49:
        return b >= 105;
      case 50:
        return b >= 100;
      case 51:
        return b >= 95;
      case 52:
        return b >= 90;
      case 53:
        return b >= 85;
      case 54:
        return b >= 80 /*0x50*/;
      case 55:
        return b >= 75;
      case 56:
        return b >= 69;
      case 57:
        return b >= 64 /*0x40*/;
      case 58:
        return b >= 59;
      case 59:
        return b >= 54;
      case 60:
        return b >= 49;
      case 61:
        return b >= 44;
      case 62:
        return b >= 39;
      case 63 /*0x3F*/:
        return b >= 33;
      case 64 /*0x40*/:
        return b >= 28;
      case 65:
        return b >= 23;
      case 66:
        return b >= 18;
      case 67:
        return b >= 13;
      case 68:
        return b >= 8;
      case 69:
        return b >= 2;
      default:
        return g >= 70;
    }
  }

  private static bool CheckR68(int g, int b)
  {
    switch (g)
    {
      case 20:
        return b >= 252;
      case 21:
        return b >= 247;
      case 22:
        return b >= 242;
      case 23:
        return b >= 237;
      case 24:
        return b >= 232;
      case 25:
        return b >= 226;
      case 26:
        return b >= 221;
      case 27:
        return b >= 216;
      case 28:
        return b >= 211;
      case 29:
        return b >= 206;
      case 30:
        return b >= 201;
      case 31 /*0x1F*/:
        return b >= 196;
      case 32 /*0x20*/:
        return b >= 190;
      case 33:
        return b >= 185;
      case 34:
        return b >= 180;
      case 35:
        return b >= 175;
      case 36:
        return b >= 170;
      case 37:
        return b >= 165;
      case 38:
        return b >= 159;
      case 39:
        return b >= 154;
      case 40:
        return b >= 149;
      case 41:
        return b >= 144 /*0x90*/;
      case 42:
        return b >= 139;
      case 43:
        return b >= 134;
      case 44:
        return b >= 129;
      case 45:
        return b >= 123;
      case 46:
        return b >= 118;
      case 47:
        return b >= 113;
      case 48 /*0x30*/:
        return b >= 108;
      case 49:
        return b >= 103;
      case 50:
        return b >= 98;
      case 51:
        return b >= 93;
      case 52:
        return b >= 87;
      case 53:
        return b >= 82;
      case 54:
        return b >= 77;
      case 55:
        return b >= 72;
      case 56:
        return b >= 67;
      case 57:
        return b >= 62;
      case 58:
        return b >= 56;
      case 59:
        return b >= 51;
      case 60:
        return b >= 46;
      case 61:
        return b >= 41;
      case 62:
        return b >= 36;
      case 63 /*0x3F*/:
        return b >= 31 /*0x1F*/;
      case 64 /*0x40*/:
        return b >= 26;
      case 65:
        return b >= 20;
      case 66:
        return b >= 15;
      case 67:
        return b >= 10;
      case 68:
        return b >= 5;
      default:
        return g >= 69;
    }
  }

  private static bool CheckR69(int g, int b)
  {
    switch (g)
    {
      case 19:
        return b >= (int) byte.MaxValue;
      case 20:
        return b >= 250;
      case 21:
        return b >= 244;
      case 22:
        return b >= 239;
      case 23:
        return b >= 234;
      case 24:
        return b >= 229;
      case 25:
        return b >= 224 /*0xE0*/;
      case 26:
        return b >= 219;
      case 27:
        return b >= 213;
      case 28:
        return b >= 208 /*0xD0*/;
      case 29:
        return b >= 203;
      case 30:
        return b >= 198;
      case 31 /*0x1F*/:
        return b >= 193;
      case 32 /*0x20*/:
        return b >= 188;
      case 33:
        return b >= 183;
      case 34:
        return b >= 177;
      case 35:
        return b >= 172;
      case 36:
        return b >= 167;
      case 37:
        return b >= 162;
      case 38:
        return b >= 157;
      case 39:
        return b >= 152;
      case 40:
        return b >= 147;
      case 41:
        return b >= 141;
      case 42:
        return b >= 136;
      case 43:
        return b >= 131;
      case 44:
        return b >= 126;
      case 45:
        return b >= 121;
      case 46:
        return b >= 116;
      case 47:
        return b >= 111;
      case 48 /*0x30*/:
        return b >= 105;
      case 49:
        return b >= 100;
      case 50:
        return b >= 95;
      case 51:
        return b >= 90;
      case 52:
        return b >= 85;
      case 53:
        return b >= 80 /*0x50*/;
      case 54:
        return b >= 74;
      case 55:
        return b >= 69;
      case 56:
        return b >= 64 /*0x40*/;
      case 57:
        return b >= 59;
      case 58:
        return b >= 54;
      case 59:
        return b >= 49;
      case 60:
        return b >= 44;
      case 61:
        return b >= 38;
      case 62:
        return b >= 33;
      case 63 /*0x3F*/:
        return b >= 28;
      case 64 /*0x40*/:
        return b >= 23;
      case 65:
        return b >= 18;
      case 66:
        return b >= 13;
      case 67:
        return b >= 8;
      case 68:
        return b >= 2;
      default:
        return g >= 69;
    }
  }

  private static bool CheckR70(int g, int b)
  {
    switch (g)
    {
      case 19:
        return b >= 252;
      case 20:
        return b >= 247;
      case 21:
        return b >= 242;
      case 22:
        return b >= 237;
      case 23:
        return b >= 231;
      case 24:
        return b >= 226;
      case 25:
        return b >= 221;
      case 26:
        return b >= 216;
      case 27:
        return b >= 211;
      case 28:
        return b >= 206;
      case 29:
        return b >= 201;
      case 30:
        return b >= 195;
      case 31 /*0x1F*/:
        return b >= 190;
      case 32 /*0x20*/:
        return b >= 185;
      case 33:
        return b >= 180;
      case 34:
        return b >= 175;
      case 35:
        return b >= 170;
      case 36:
        return b >= 165;
      case 37:
        return b >= 159;
      case 38:
        return b >= 154;
      case 39:
        return b >= 149;
      case 40:
        return b >= 144 /*0x90*/;
      case 41:
        return b >= 139;
      case 42:
        return b >= 134;
      case 43:
        return b >= 128 /*0x80*/;
      case 44:
        return b >= 123;
      case 45:
        return b >= 118;
      case 46:
        return b >= 113;
      case 47:
        return b >= 108;
      case 48 /*0x30*/:
        return b >= 103;
      case 49:
        return b >= 98;
      case 50:
        return b >= 92;
      case 51:
        return b >= 87;
      case 52:
        return b >= 82;
      case 53:
        return b >= 77;
      case 54:
        return b >= 72;
      case 55:
        return b >= 67;
      case 56:
        return b >= 62;
      case 57:
        return b >= 56;
      case 58:
        return b >= 51;
      case 59:
        return b >= 46;
      case 60:
        return b >= 41;
      case 61:
        return b >= 36;
      case 62:
        return b >= 31 /*0x1F*/;
      case 63 /*0x3F*/:
        return b >= 26;
      case 64 /*0x40*/:
        return b >= 20;
      case 65:
        return b >= 15;
      case 66:
        return b >= 10;
      case 67:
        return b >= 5;
      default:
        return g >= 68;
    }
  }

  private static bool CheckR71(int g, int b)
  {
    switch (g)
    {
      case 18:
        return b >= (int) byte.MaxValue;
      case 19:
        return b >= 249;
      case 20:
        return b >= 244;
      case 21:
        return b >= 239;
      case 22:
        return b >= 234;
      case 23:
        return b >= 229;
      case 24:
        return b >= 224 /*0xE0*/;
      case 25:
        return b >= 219;
      case 26:
        return b >= 213;
      case 27:
        return b >= 208 /*0xD0*/;
      case 28:
        return b >= 203;
      case 29:
        return b >= 198;
      case 30:
        return b >= 193;
      case 31 /*0x1F*/:
        return b >= 188;
      case 32 /*0x20*/:
        return b >= 183;
      case 33:
        return b >= 177;
      case 34:
        return b >= 172;
      case 35:
        return b >= 167;
      case 36:
        return b >= 162;
      case 37:
        return b >= 157;
      case 38:
        return b >= 152;
      case 39:
        return b >= 146;
      case 40:
        return b >= 141;
      case 41:
        return b >= 136;
      case 42:
        return b >= 131;
      case 43:
        return b >= 126;
      case 44:
        return b >= 121;
      case 45:
        return b >= 116;
      case 46:
        return b >= 110;
      case 47:
        return b >= 105;
      case 48 /*0x30*/:
        return b >= 100;
      case 49:
        return b >= 95;
      case 50:
        return b >= 90;
      case 51:
        return b >= 85;
      case 52:
        return b >= 80 /*0x50*/;
      case 53:
        return b >= 74;
      case 54:
        return b >= 69;
      case 55:
        return b >= 64 /*0x40*/;
      case 56:
        return b >= 59;
      case 57:
        return b >= 54;
      case 58:
        return b >= 49;
      case 59:
        return b >= 43;
      case 60:
        return b >= 38;
      case 61:
        return b >= 33;
      case 62:
        return b >= 28;
      case 63 /*0x3F*/:
        return b >= 23;
      case 64 /*0x40*/:
        return b >= 18;
      case 65:
        return b >= 13;
      case 66:
        return b >= 7;
      case 67:
        return b >= 2;
      default:
        return g >= 68;
    }
  }

  private static bool CheckR72(int g, int b)
  {
    switch (g)
    {
      case 18:
        return b >= 252;
      case 19:
        return b >= 247;
      case 20:
        return b >= 242;
      case 21:
        return b >= 237;
      case 22:
        return b >= 231;
      case 23:
        return b >= 226;
      case 24:
        return b >= 221;
      case 25:
        return b >= 216;
      case 26:
        return b >= 211;
      case 27:
        return b >= 206;
      case 28:
        return b >= 200;
      case 29:
        return b >= 195;
      case 30:
        return b >= 190;
      case 31 /*0x1F*/:
        return b >= 185;
      case 32 /*0x20*/:
        return b >= 180;
      case 33:
        return b >= 175;
      case 34:
        return b >= 170;
      case 35:
        return b >= 164;
      case 36:
        return b >= 159;
      case 37:
        return b >= 154;
      case 38:
        return b >= 149;
      case 39:
        return b >= 144 /*0x90*/;
      case 40:
        return b >= 139;
      case 41:
        return b >= 134;
      case 42:
        return b >= 128 /*0x80*/;
      case 43:
        return b >= 123;
      case 44:
        return b >= 118;
      case 45:
        return b >= 113;
      case 46:
        return b >= 108;
      case 47:
        return b >= 103;
      case 48 /*0x30*/:
        return b >= 97;
      case 49:
        return b >= 92;
      case 50:
        return b >= 87;
      case 51:
        return b >= 82;
      case 52:
        return b >= 77;
      case 53:
        return b >= 72;
      case 54:
        return b >= 67;
      case 55:
        return b >= 61;
      case 56:
        return b >= 56;
      case 57:
        return b >= 51;
      case 58:
        return b >= 46;
      case 59:
        return b >= 41;
      case 60:
        return b >= 36;
      case 61:
        return b >= 31 /*0x1F*/;
      case 62:
        return b >= 25;
      case 63 /*0x3F*/:
        return b >= 20;
      case 64 /*0x40*/:
        return b >= 15;
      case 65:
        return b >= 10;
      case 66:
        return b >= 5;
      default:
        return g >= 67;
    }
  }

  private static bool CheckR73(int g, int b)
  {
    switch (g)
    {
      case 17:
        return b >= 254;
      case 18:
        return b >= 249;
      case 19:
        return b >= 244;
      case 20:
        return b >= 239;
      case 21:
        return b >= 234;
      case 22:
        return b >= 229;
      case 23:
        return b >= 224 /*0xE0*/;
      case 24:
        return b >= 218;
      case 25:
        return b >= 213;
      case 26:
        return b >= 208 /*0xD0*/;
      case 27:
        return b >= 203;
      case 28:
        return b >= 198;
      case 29:
        return b >= 193;
      case 30:
        return b >= 188;
      case 31 /*0x1F*/:
        return b >= 182;
      case 32 /*0x20*/:
        return b >= 177;
      case 33:
        return b >= 172;
      case 34:
        return b >= 167;
      case 35:
        return b >= 162;
      case 36:
        return b >= 157;
      case 37:
        return b >= 152;
      case 38:
        return b >= 146;
      case 39:
        return b >= 141;
      case 40:
        return b >= 136;
      case 41:
        return b >= 131;
      case 42:
        return b >= 126;
      case 43:
        return b >= 121;
      case 44:
        return b >= 115;
      case 45:
        return b >= 110;
      case 46:
        return b >= 105;
      case 47:
        return b >= 100;
      case 48 /*0x30*/:
        return b >= 95;
      case 49:
        return b >= 90;
      case 50:
        return b >= 85;
      case 51:
        return b >= 79;
      case 52:
        return b >= 74;
      case 53:
        return b >= 69;
      case 54:
        return b >= 64 /*0x40*/;
      case 55:
        return b >= 59;
      case 56:
        return b >= 54;
      case 57:
        return b >= 49;
      case 58:
        return b >= 43;
      case 59:
        return b >= 38;
      case 60:
        return b >= 33;
      case 61:
        return b >= 28;
      case 62:
        return b >= 23;
      case 63 /*0x3F*/:
        return b >= 18;
      case 64 /*0x40*/:
        return b >= 12;
      case 65:
        return b >= 7;
      case 66:
        return b >= 2;
      default:
        return g >= 67;
    }
  }

  private static bool CheckR74(int g, int b)
  {
    switch (g)
    {
      case 17:
        return b >= 252;
      case 18:
        return b >= 247;
      case 19:
        return b >= 242;
      case 20:
        return b >= 236;
      case 21:
        return b >= 231;
      case 22:
        return b >= 226;
      case 23:
        return b >= 221;
      case 24:
        return b >= 216;
      case 25:
        return b >= 211;
      case 26:
        return b >= 206;
      case 27:
        return b >= 200;
      case 28:
        return b >= 195;
      case 29:
        return b >= 190;
      case 30:
        return b >= 185;
      case 31 /*0x1F*/:
        return b >= 180;
      case 32 /*0x20*/:
        return b >= 175;
      case 33:
        return b >= 169;
      case 34:
        return b >= 164;
      case 35:
        return b >= 159;
      case 36:
        return b >= 154;
      case 37:
        return b >= 149;
      case 38:
        return b >= 144 /*0x90*/;
      case 39:
        return b >= 139;
      case 40:
        return b >= 133;
      case 41:
        return b >= 128 /*0x80*/;
      case 42:
        return b >= 123;
      case 43:
        return b >= 118;
      case 44:
        return b >= 113;
      case 45:
        return b >= 108;
      case 46:
        return b >= 103;
      case 47:
        return b >= 97;
      case 48 /*0x30*/:
        return b >= 92;
      case 49:
        return b >= 87;
      case 50:
        return b >= 82;
      case 51:
        return b >= 77;
      case 52:
        return b >= 72;
      case 53:
        return b >= 67;
      case 54:
        return b >= 61;
      case 55:
        return b >= 56;
      case 56:
        return b >= 51;
      case 57:
        return b >= 46;
      case 58:
        return b >= 41;
      case 59:
        return b >= 36;
      case 60:
        return b >= 30;
      case 61:
        return b >= 25;
      case 62:
        return b >= 20;
      case 63 /*0x3F*/:
        return b >= 15;
      case 64 /*0x40*/:
        return b >= 10;
      case 65:
        return b >= 5;
      default:
        return g >= 66;
    }
  }

  private static bool CheckR75(int g, int b)
  {
    switch (g)
    {
      case 16 /*0x10*/:
        return b >= 254;
      case 17:
        return b >= 249;
      case 18:
        return b >= 244;
      case 19:
        return b >= 239;
      case 20:
        return b >= 234;
      case 21:
        return b >= 229;
      case 22:
        return b >= 224 /*0xE0*/;
      case 23:
        return b >= 218;
      case 24:
        return b >= 213;
      case 25:
        return b >= 208 /*0xD0*/;
      case 26:
        return b >= 203;
      case 27:
        return b >= 198;
      case 28:
        return b >= 193;
      case 29:
        return b >= 187;
      case 30:
        return b >= 182;
      case 31 /*0x1F*/:
        return b >= 177;
      case 32 /*0x20*/:
        return b >= 172;
      case 33:
        return b >= 167;
      case 34:
        return b >= 162;
      case 35:
        return b >= 157;
      case 36:
        return b >= 151;
      case 37:
        return b >= 146;
      case 38:
        return b >= 141;
      case 39:
        return b >= 136;
      case 40:
        return b >= 131;
      case 41:
        return b >= 126;
      case 42:
        return b >= 121;
      case 43:
        return b >= 115;
      case 44:
        return b >= 110;
      case 45:
        return b >= 105;
      case 46:
        return b >= 100;
      case 47:
        return b >= 95;
      case 48 /*0x30*/:
        return b >= 90;
      case 49:
        return b >= 84;
      case 50:
        return b >= 79;
      case 51:
        return b >= 74;
      case 52:
        return b >= 69;
      case 53:
        return b >= 64 /*0x40*/;
      case 54:
        return b >= 59;
      case 55:
        return b >= 54;
      case 56:
        return b >= 48 /*0x30*/;
      case 57:
        return b >= 43;
      case 58:
        return b >= 38;
      case 59:
        return b >= 33;
      case 60:
        return b >= 28;
      case 61:
        return b >= 23;
      case 62:
        return b >= 18;
      case 63 /*0x3F*/:
        return b >= 12;
      case 64 /*0x40*/:
        return b >= 7;
      case 65:
        return b >= 2;
      default:
        return g >= 66;
    }
  }

  private static bool CheckR76(int g, int b)
  {
    switch (g)
    {
      case 16 /*0x10*/:
        return b >= 252;
      case 17:
        return b >= 247;
      case 18:
        return b >= 241;
      case 19:
        return b >= 236;
      case 20:
        return b >= 231;
      case 21:
        return b >= 226;
      case 22:
        return b >= 221;
      case 23:
        return b >= 216;
      case 24:
        return b >= 211;
      case 25:
        return b >= 205;
      case 26:
        return b >= 200;
      case 27:
        return b >= 195;
      case 28:
        return b >= 190;
      case 29:
        return b >= 185;
      case 30:
        return b >= 180;
      case 31 /*0x1F*/:
        return b >= 175;
      case 32 /*0x20*/:
        return b >= 169;
      case 33:
        return b >= 164;
      case 34:
        return b >= 159;
      case 35:
        return b >= 154;
      case 36:
        return b >= 149;
      case 37:
        return b >= 144 /*0x90*/;
      case 38:
        return b >= 138;
      case 39:
        return b >= 133;
      case 40:
        return b >= 128 /*0x80*/;
      case 41:
        return b >= 123;
      case 42:
        return b >= 118;
      case 43:
        return b >= 113;
      case 44:
        return b >= 108;
      case 45:
        return b >= 102;
      case 46:
        return b >= 97;
      case 47:
        return b >= 92;
      case 48 /*0x30*/:
        return b >= 87;
      case 49:
        return b >= 82;
      case 50:
        return b >= 77;
      case 51:
        return b >= 72;
      case 52:
        return b >= 66;
      case 53:
        return b >= 61;
      case 54:
        return b >= 56;
      case 55:
        return b >= 51;
      case 56:
        return b >= 46;
      case 57:
        return b >= 41;
      case 58:
        return b >= 36;
      case 59:
        return b >= 30;
      case 60:
        return b >= 25;
      case 61:
        return b >= 20;
      case 62:
        return b >= 15;
      case 63 /*0x3F*/:
        return b >= 10;
      case 64 /*0x40*/:
        return b >= 5;
      default:
        return g >= 65;
    }
  }

  private static bool CheckR77(int g, int b)
  {
    switch (g)
    {
      case 15:
        return b >= 254;
      case 16 /*0x10*/:
        return b >= 249;
      case 17:
        return b >= 244;
      case 18:
        return b >= 239;
      case 19:
        return b >= 234;
      case 20:
        return b >= 229;
      case 21:
        return b >= 223;
      case 22:
        return b >= 218;
      case 23:
        return b >= 213;
      case 24:
        return b >= 208 /*0xD0*/;
      case 25:
        return b >= 203;
      case 26:
        return b >= 198;
      case 27:
        return b >= 193;
      case 28:
        return b >= 187;
      case 29:
        return b >= 182;
      case 30:
        return b >= 177;
      case 31 /*0x1F*/:
        return b >= 172;
      case 32 /*0x20*/:
        return b >= 167;
      case 33:
        return b >= 162;
      case 34:
        return b >= 156;
      case 35:
        return b >= 151;
      case 36:
        return b >= 146;
      case 37:
        return b >= 141;
      case 38:
        return b >= 136;
      case 39:
        return b >= 131;
      case 40:
        return b >= 126;
      case 41:
        return b >= 120;
      case 42:
        return b >= 115;
      case 43:
        return b >= 110;
      case 44:
        return b >= 105;
      case 45:
        return b >= 100;
      case 46:
        return b >= 95;
      case 47:
        return b >= 90;
      case 48 /*0x30*/:
        return b >= 84;
      case 49:
        return b >= 79;
      case 50:
        return b >= 74;
      case 51:
        return b >= 69;
      case 52:
        return b >= 64 /*0x40*/;
      case 53:
        return b >= 59;
      case 54:
        return b >= 53;
      case 55:
        return b >= 48 /*0x30*/;
      case 56:
        return b >= 43;
      case 57:
        return b >= 38;
      case 58:
        return b >= 33;
      case 59:
        return b >= 28;
      case 60:
        return b >= 23;
      case 61:
        return b >= 17;
      case 62:
        return b >= 12;
      case 63 /*0x3F*/:
        return b >= 7;
      case 64 /*0x40*/:
        return b >= 2;
      default:
        return g >= 65;
    }
  }

  private static bool CheckR78(int g, int b)
  {
    switch (g)
    {
      case 15:
        return b >= 252;
      case 16 /*0x10*/:
        return b >= 247;
      case 17:
        return b >= 241;
      case 18:
        return b >= 236;
      case 19:
        return b >= 231;
      case 20:
        return b >= 226;
      case 21:
        return b >= 221;
      case 22:
        return b >= 216;
      case 23:
        return b >= 210;
      case 24:
        return b >= 205;
      case 25:
        return b >= 200;
      case 26:
        return b >= 195;
      case 27:
        return b >= 190;
      case 28:
        return b >= 185;
      case 29:
        return b >= 180;
      case 30:
        return b >= 174;
      case 31 /*0x1F*/:
        return b >= 169;
      case 32 /*0x20*/:
        return b >= 164;
      case 33:
        return b >= 159;
      case 34:
        return b >= 154;
      case 35:
        return b >= 149;
      case 36:
        return b >= 144 /*0x90*/;
      case 37:
        return b >= 138;
      case 38:
        return b >= 133;
      case 39:
        return b >= 128 /*0x80*/;
      case 40:
        return b >= 123;
      case 41:
        return b >= 118;
      case 42:
        return b >= 113;
      case 43:
        return b >= 108;
      case 44:
        return b >= 102;
      case 45:
        return b >= 97;
      case 46:
        return b >= 92;
      case 47:
        return b >= 87;
      case 48 /*0x30*/:
        return b >= 82;
      case 49:
        return b >= 77;
      case 50:
        return b >= 71;
      case 51:
        return b >= 66;
      case 52:
        return b >= 61;
      case 53:
        return b >= 56;
      case 54:
        return b >= 51;
      case 55:
        return b >= 46;
      case 56:
        return b >= 41;
      case 57:
        return b >= 35;
      case 58:
        return b >= 30;
      case 59:
        return b >= 25;
      case 60:
        return b >= 20;
      case 61:
        return b >= 15;
      case 62:
        return b >= 10;
      case 63 /*0x3F*/:
        return b >= 5;
      default:
        return g >= 64 /*0x40*/;
    }
  }

  private static bool CheckR79(int g, int b)
  {
    switch (g)
    {
      case 14:
        return b >= 254;
      case 15:
        return b >= 249;
      case 16 /*0x10*/:
        return b >= 244;
      case 17:
        return b >= 239;
      case 18:
        return b >= 234;
      case 19:
        return b >= 228;
      case 20:
        return b >= 223;
      case 21:
        return b >= 218;
      case 22:
        return b >= 213;
      case 23:
        return b >= 208 /*0xD0*/;
      case 24:
        return b >= 203;
      case 25:
        return b >= 198;
      case 26:
        return b >= 192 /*0xC0*/;
      case 27:
        return b >= 187;
      case 28:
        return b >= 182;
      case 29:
        return b >= 177;
      case 30:
        return b >= 172;
      case 31 /*0x1F*/:
        return b >= 167;
      case 32 /*0x20*/:
        return b >= 162;
      case 33:
        return b >= 156;
      case 34:
        return b >= 151;
      case 35:
        return b >= 146;
      case 36:
        return b >= 141;
      case 37:
        return b >= 136;
      case 38:
        return b >= 131;
      case 39:
        return b >= 125;
      case 40:
        return b >= 120;
      case 41:
        return b >= 115;
      case 42:
        return b >= 110;
      case 43:
        return b >= 105;
      case 44:
        return b >= 100;
      case 45:
        return b >= 95;
      case 46:
        return b >= 89;
      case 47:
        return b >= 84;
      case 48 /*0x30*/:
        return b >= 79;
      case 49:
        return b >= 74;
      case 50:
        return b >= 69;
      case 51:
        return b >= 64 /*0x40*/;
      case 52:
        return b >= 59;
      case 53:
        return b >= 53;
      case 54:
        return b >= 48 /*0x30*/;
      case 55:
        return b >= 43;
      case 56:
        return b >= 38;
      case 57:
        return b >= 33;
      case 58:
        return b >= 28;
      case 59:
        return b >= 22;
      case 60:
        return b >= 17;
      case 61:
        return b >= 12;
      case 62:
        return b >= 7;
      case 63 /*0x3F*/:
        return b >= 2;
      default:
        return g >= 64 /*0x40*/;
    }
  }

  private static bool CheckR80(int g, int b)
  {
    switch (g)
    {
      case 14:
        return b >= 252;
      case 15:
        return b >= 246;
      case 16 /*0x10*/:
        return b >= 241;
      case 17:
        return b >= 236;
      case 18:
        return b >= 231;
      case 19:
        return b >= 226;
      case 20:
        return b >= 221;
      case 21:
        return b >= 216;
      case 22:
        return b >= 210;
      case 23:
        return b >= 205;
      case 24:
        return b >= 200;
      case 25:
        return b >= 195;
      case 26:
        return b >= 190;
      case 27:
        return b >= 185;
      case 28:
        return b >= 179;
      case 29:
        return b >= 174;
      case 30:
        return b >= 169;
      case 31 /*0x1F*/:
        return b >= 164;
      case 32 /*0x20*/:
        return b >= 159;
      case 33:
        return b >= 154;
      case 34:
        return b >= 149;
      case 35:
        return b >= 143;
      case 36:
        return b >= 138;
      case 37:
        return b >= 133;
      case 38:
        return b >= 128 /*0x80*/;
      case 39:
        return b >= 123;
      case 40:
        return b >= 118;
      case 41:
        return b >= 113;
      case 42:
        return b >= 107;
      case 43:
        return b >= 102;
      case 44:
        return b >= 97;
      case 45:
        return b >= 92;
      case 46:
        return b >= 87;
      case 47:
        return b >= 82;
      case 48 /*0x30*/:
        return b >= 77;
      case 49:
        return b >= 71;
      case 50:
        return b >= 66;
      case 51:
        return b >= 61;
      case 52:
        return b >= 56;
      case 53:
        return b >= 51;
      case 54:
        return b >= 46;
      case 55:
        return b >= 40;
      case 56:
        return b >= 35;
      case 57:
        return b >= 30;
      case 58:
        return b >= 25;
      case 59:
        return b >= 20;
      case 60:
        return b >= 15;
      case 61:
        return b >= 10;
      case 62:
        return b >= 4;
      default:
        return g >= 63 /*0x3F*/;
    }
  }

  private static bool CheckR81(int g, int b)
  {
    switch (g)
    {
      case 13:
        return b >= 254;
      case 14:
        return b >= 249;
      case 15:
        return b >= 244;
      case 16 /*0x10*/:
        return b >= 239;
      case 17:
        return b >= 234;
      case 18:
        return b >= 228;
      case 19:
        return b >= 223;
      case 20:
        return b >= 218;
      case 21:
        return b >= 213;
      case 22:
        return b >= 208 /*0xD0*/;
      case 23:
        return b >= 203;
      case 24:
        return b >= 197;
      case 25:
        return b >= 192 /*0xC0*/;
      case 26:
        return b >= 187;
      case 27:
        return b >= 182;
      case 28:
        return b >= 177;
      case 29:
        return b >= 172;
      case 30:
        return b >= 167;
      case 31 /*0x1F*/:
        return b >= 161;
      case 32 /*0x20*/:
        return b >= 156;
      case 33:
        return b >= 151;
      case 34:
        return b >= 146;
      case 35:
        return b >= 141;
      case 36:
        return b >= 136;
      case 37:
        return b >= 131;
      case 38:
        return b >= 125;
      case 39:
        return b >= 120;
      case 40:
        return b >= 115;
      case 41:
        return b >= 110;
      case 42:
        return b >= 105;
      case 43:
        return b >= 100;
      case 44:
        return b >= 94;
      case 45:
        return b >= 89;
      case 46:
        return b >= 84;
      case 47:
        return b >= 79;
      case 48 /*0x30*/:
        return b >= 74;
      case 49:
        return b >= 69;
      case 50:
        return b >= 64 /*0x40*/;
      case 51:
        return b >= 58;
      case 52:
        return b >= 53;
      case 53:
        return b >= 48 /*0x30*/;
      case 54:
        return b >= 43;
      case 55:
        return b >= 38;
      case 56:
        return b >= 33;
      case 57:
        return b >= 28;
      case 58:
        return b >= 22;
      case 59:
        return b >= 17;
      case 60:
        return b >= 12;
      case 61:
        return b >= 7;
      case 62:
        return b >= 2;
      default:
        return g >= 63 /*0x3F*/;
    }
  }

  private static bool CheckR82(int g, int b)
  {
    switch (g)
    {
      case 13:
        return b >= 251;
      case 14:
        return b >= 246;
      case 15:
        return b >= 241;
      case 16 /*0x10*/:
        return b >= 236;
      case 17:
        return b >= 231;
      case 18:
        return b >= 226;
      case 19:
        return b >= 221;
      case 20:
        return b >= 215;
      case 21:
        return b >= 210;
      case 22:
        return b >= 205;
      case 23:
        return b >= 200;
      case 24:
        return b >= 195;
      case 25:
        return b >= 190;
      case 26:
        return b >= 185;
      case 27:
        return b >= 179;
      case 28:
        return b >= 174;
      case 29:
        return b >= 169;
      case 30:
        return b >= 164;
      case 31 /*0x1F*/:
        return b >= 159;
      case 32 /*0x20*/:
        return b >= 154;
      case 33:
        return b >= 149;
      case 34:
        return b >= 143;
      case 35:
        return b >= 138;
      case 36:
        return b >= 133;
      case 37:
        return b >= 128 /*0x80*/;
      case 38:
        return b >= 123;
      case 39:
        return b >= 118;
      case 40:
        return b >= 112 /*0x70*/;
      case 41:
        return b >= 107;
      case 42:
        return b >= 102;
      case 43:
        return b >= 97;
      case 44:
        return b >= 92;
      case 45:
        return b >= 87;
      case 46:
        return b >= 82;
      case 47:
        return b >= 76;
      case 48 /*0x30*/:
        return b >= 71;
      case 49:
        return b >= 66;
      case 50:
        return b >= 61;
      case 51:
        return b >= 56;
      case 52:
        return b >= 51;
      case 53:
        return b >= 46;
      case 54:
        return b >= 40;
      case 55:
        return b >= 35;
      case 56:
        return b >= 30;
      case 57:
        return b >= 25;
      case 58:
        return b >= 20;
      case 59:
        return b >= 15;
      case 60:
        return b >= 9;
      case 61:
        return b >= 4;
      default:
        return g >= 62;
    }
  }

  private static bool CheckR83(int g, int b)
  {
    switch (g)
    {
      case 12:
        return b >= 254;
      case 13:
        return b >= 249;
      case 14:
        return b >= 244;
      case 15:
        return b >= 239;
      case 16 /*0x10*/:
        return b >= 233;
      case 17:
        return b >= 228;
      case 18:
        return b >= 223;
      case 19:
        return b >= 218;
      case 20:
        return b >= 213;
      case 21:
        return b >= 208 /*0xD0*/;
      case 22:
        return b >= 203;
      case 23:
        return b >= 197;
      case 24:
        return b >= 192 /*0xC0*/;
      case 25:
        return b >= 187;
      case 26:
        return b >= 182;
      case 27:
        return b >= 177;
      case 28:
        return b >= 172;
      case 29:
        return b >= 166;
      case 30:
        return b >= 161;
      case 31 /*0x1F*/:
        return b >= 156;
      case 32 /*0x20*/:
        return b >= 151;
      case 33:
        return b >= 146;
      case 34:
        return b >= 141;
      case 35:
        return b >= 136;
      case 36:
        return b >= 130;
      case 37:
        return b >= 125;
      case 38:
        return b >= 120;
      case 39:
        return b >= 115;
      case 40:
        return b >= 110;
      case 41:
        return b >= 105;
      case 42:
        return b >= 100;
      case 43:
        return b >= 94;
      case 44:
        return b >= 89;
      case 45:
        return b >= 84;
      case 46:
        return b >= 79;
      case 47:
        return b >= 74;
      case 48 /*0x30*/:
        return b >= 69;
      case 49:
        return b >= 63 /*0x3F*/;
      case 50:
        return b >= 58;
      case 51:
        return b >= 53;
      case 52:
        return b >= 48 /*0x30*/;
      case 53:
        return b >= 43;
      case 54:
        return b >= 38;
      case 55:
        return b >= 33;
      case 56:
        return b >= 27;
      case 57:
        return b >= 22;
      case 58:
        return b >= 17;
      case 59:
        return b >= 12;
      case 60:
        return b >= 7;
      case 61:
        return b >= 2;
      default:
        return g >= 62;
    }
  }

  private static bool CheckR84(int g, int b)
  {
    switch (g)
    {
      case 12:
        return b >= 251;
      case 13:
        return b >= 246;
      case 14:
        return b >= 241;
      case 15:
        return b >= 236;
      case 16 /*0x10*/:
        return b >= 231;
      case 17:
        return b >= 226;
      case 18:
        return b >= 220;
      case 19:
        return b >= 215;
      case 20:
        return b >= 210;
      case 21:
        return b >= 205;
      case 22:
        return b >= 200;
      case 23:
        return b >= 195;
      case 24:
        return b >= 190;
      case 25:
        return b >= 184;
      case 26:
        return b >= 179;
      case 27:
        return b >= 174;
      case 28:
        return b >= 169;
      case 29:
        return b >= 164;
      case 30:
        return b >= 159;
      case 31 /*0x1F*/:
        return b >= 154;
      case 32 /*0x20*/:
        return b >= 148;
      case 33:
        return b >= 143;
      case 34:
        return b >= 138;
      case 35:
        return b >= 133;
      case 36:
        return b >= 128 /*0x80*/;
      case 37:
        return b >= 123;
      case 38:
        return b >= 118;
      case 39:
        return b >= 112 /*0x70*/;
      case 40:
        return b >= 107;
      case 41:
        return b >= 102;
      case 42:
        return b >= 97;
      case 43:
        return b >= 92;
      case 44:
        return b >= 87;
      case 45:
        return b >= 81;
      case 46:
        return b >= 76;
      case 47:
        return b >= 71;
      case 48 /*0x30*/:
        return b >= 66;
      case 49:
        return b >= 61;
      case 50:
        return b >= 56;
      case 51:
        return b >= 51;
      case 52:
        return b >= 45;
      case 53:
        return b >= 40;
      case 54:
        return b >= 35;
      case 55:
        return b >= 30;
      case 56:
        return b >= 25;
      case 57:
        return b >= 20;
      case 58:
        return b >= 15;
      case 59:
        return b >= 9;
      case 60:
        return b >= 4;
      default:
        return g >= 61;
    }
  }

  private static bool CheckR85(int g, int b)
  {
    switch (g)
    {
      case 11:
        return b >= 254;
      case 12:
        return b >= 249;
      case 13:
        return b >= 244;
      case 14:
        return b >= 238;
      case 15:
        return b >= 233;
      case 16 /*0x10*/:
        return b >= 228;
      case 17:
        return b >= 223;
      case 18:
        return b >= 218;
      case 19:
        return b >= 213;
      case 20:
        return b >= 208 /*0xD0*/;
      case 21:
        return b >= 202;
      case 22:
        return b >= 197;
      case 23:
        return b >= 192 /*0xC0*/;
      case 24:
        return b >= 187;
      case 25:
        return b >= 182;
      case 26:
        return b >= 177;
      case 27:
        return b >= 172;
      case 28:
        return b >= 166;
      case 29:
        return b >= 161;
      case 30:
        return b >= 156;
      case 31 /*0x1F*/:
        return b >= 151;
      case 32 /*0x20*/:
        return b >= 146;
      case 33:
        return b >= 141;
      case 34:
        return b >= 135;
      case 35:
        return b >= 130;
      case 36:
        return b >= 125;
      case 37:
        return b >= 120;
      case 38:
        return b >= 115;
      case 39:
        return b >= 110;
      case 40:
        return b >= 105;
      case 41:
        return b >= 99;
      case 42:
        return b >= 94;
      case 43:
        return b >= 89;
      case 44:
        return b >= 84;
      case 45:
        return b >= 79;
      case 46:
        return b >= 74;
      case 47:
        return b >= 69;
      case 48 /*0x30*/:
        return b >= 63 /*0x3F*/;
      case 49:
        return b >= 58;
      case 50:
        return b >= 53;
      case 51:
        return b >= 48 /*0x30*/;
      case 52:
        return b >= 43;
      case 53:
        return b >= 38;
      case 54:
        return b >= 33;
      case 55:
        return b >= 27;
      case 56:
        return b >= 22;
      case 57:
        return b >= 17;
      case 58:
        return b >= 12;
      case 59:
        return b >= 7;
      case 60:
        return b >= 2;
      default:
        return g >= 61;
    }
  }

  private static bool CheckR86(int g, int b)
  {
    switch (g)
    {
      case 11:
        return b >= 251;
      case 12:
        return b >= 246;
      case 13:
        return b >= 241;
      case 14:
        return b >= 236;
      case 15:
        return b >= 231;
      case 16 /*0x10*/:
        return b >= 226;
      case 17:
        return b >= 220;
      case 18:
        return b >= 215;
      case 19:
        return b >= 210;
      case 20:
        return b >= 205;
      case 21:
        return b >= 200;
      case 22:
        return b >= 195;
      case 23:
        return b >= 190;
      case 24:
        return b >= 184;
      case 25:
        return b >= 179;
      case 26:
        return b >= 174;
      case 27:
        return b >= 169;
      case 28:
        return b >= 164;
      case 29:
        return b >= 159;
      case 30:
        return b >= 153;
      case 31 /*0x1F*/:
        return b >= 148;
      case 32 /*0x20*/:
        return b >= 143;
      case 33:
        return b >= 138;
      case 34:
        return b >= 133;
      case 35:
        return b >= 128 /*0x80*/;
      case 36:
        return b >= 123;
      case 37:
        return b >= 117;
      case 38:
        return b >= 112 /*0x70*/;
      case 39:
        return b >= 107;
      case 40:
        return b >= 102;
      case 41:
        return b >= 97;
      case 42:
        return b >= 92;
      case 43:
        return b >= 87;
      case 44:
        return b >= 81;
      case 45:
        return b >= 76;
      case 46:
        return b >= 71;
      case 47:
        return b >= 66;
      case 48 /*0x30*/:
        return b >= 61;
      case 49:
        return b >= 56;
      case 50:
        return b >= 50;
      case 51:
        return b >= 45;
      case 52:
        return b >= 40;
      case 53:
        return b >= 35;
      case 54:
        return b >= 30;
      case 55:
        return b >= 25;
      case 56:
        return b >= 20;
      case 57:
        return b >= 14;
      case 58:
        return b >= 9;
      case 59:
        return b >= 4;
      default:
        return g >= 60;
    }
  }

  private static bool CheckR87(int g, int b)
  {
    switch (g)
    {
      case 10:
        return b >= 254;
      case 11:
        return b >= 249;
      case 12:
        return b >= 244;
      case 13:
        return b >= 238;
      case 14:
        return b >= 233;
      case 15:
        return b >= 228;
      case 16 /*0x10*/:
        return b >= 223;
      case 17:
        return b >= 218;
      case 18:
        return b >= 213;
      case 19:
        return b >= 207;
      case 20:
        return b >= 202;
      case 21:
        return b >= 197;
      case 22:
        return b >= 192 /*0xC0*/;
      case 23:
        return b >= 187;
      case 24:
        return b >= 182;
      case 25:
        return b >= 177;
      case 26:
        return b >= 171;
      case 27:
        return b >= 166;
      case 28:
        return b >= 161;
      case 29:
        return b >= 156;
      case 30:
        return b >= 151;
      case 31 /*0x1F*/:
        return b >= 146;
      case 32 /*0x20*/:
        return b >= 141;
      case 33:
        return b >= 135;
      case 34:
        return b >= 130;
      case 35:
        return b >= 125;
      case 36:
        return b >= 120;
      case 37:
        return b >= 115;
      case 38:
        return b >= 110;
      case 39:
        return b >= 104;
      case 40:
        return b >= 99;
      case 41:
        return b >= 94;
      case 42:
        return b >= 89;
      case 43:
        return b >= 84;
      case 44:
        return b >= 79;
      case 45:
        return b >= 74;
      case 46:
        return b >= 68;
      case 47:
        return b >= 63 /*0x3F*/;
      case 48 /*0x30*/:
        return b >= 58;
      case 49:
        return b >= 53;
      case 50:
        return b >= 48 /*0x30*/;
      case 51:
        return b >= 43;
      case 52:
        return b >= 38;
      case 53:
        return b >= 32 /*0x20*/;
      case 54:
        return b >= 27;
      case 55:
        return b >= 22;
      case 56:
        return b >= 17;
      case 57:
        return b >= 12;
      case 58:
        return b >= 7;
      case 59:
        return b >= 2;
      default:
        return g >= 60;
    }
  }

  private static bool CheckR88(int g, int b)
  {
    switch (g)
    {
      case 10:
        return b >= 251;
      case 11:
        return b >= 246;
      case 12:
        return b >= 241;
      case 13:
        return b >= 236;
      case 14:
        return b >= 231;
      case 15:
        return b >= 225;
      case 16 /*0x10*/:
        return b >= 220;
      case 17:
        return b >= 215;
      case 18:
        return b >= 210;
      case 19:
        return b >= 205;
      case 20:
        return b >= 200;
      case 21:
        return b >= 195;
      case 22:
        return b >= 189;
      case 23:
        return b >= 184;
      case 24:
        return b >= 179;
      case 25:
        return b >= 174;
      case 26:
        return b >= 169;
      case 27:
        return b >= 164;
      case 28:
        return b >= 159;
      case 29:
        return b >= 153;
      case 30:
        return b >= 148;
      case 31 /*0x1F*/:
        return b >= 143;
      case 32 /*0x20*/:
        return b >= 138;
      case 33:
        return b >= 133;
      case 34:
        return b >= 128 /*0x80*/;
      case 35:
        return b >= 122;
      case 36:
        return b >= 117;
      case 37:
        return b >= 112 /*0x70*/;
      case 38:
        return b >= 107;
      case 39:
        return b >= 102;
      case 40:
        return b >= 97;
      case 41:
        return b >= 92;
      case 42:
        return b >= 86;
      case 43:
        return b >= 81;
      case 44:
        return b >= 76;
      case 45:
        return b >= 71;
      case 46:
        return b >= 66;
      case 47:
        return b >= 61;
      case 48 /*0x30*/:
        return b >= 56;
      case 49:
        return b >= 50;
      case 50:
        return b >= 45;
      case 51:
        return b >= 40;
      case 52:
        return b >= 35;
      case 53:
        return b >= 30;
      case 54:
        return b >= 25;
      case 55:
        return b >= 19;
      case 56:
        return b >= 14;
      case 57:
        return b >= 9;
      case 58:
        return b >= 4;
      default:
        return g >= 59;
    }
  }

  private static bool CheckR89(int g, int b)
  {
    switch (g)
    {
      case 9:
        return b >= 254;
      case 10:
        return b >= 249;
      case 11:
        return b >= 243;
      case 12:
        return b >= 238;
      case 13:
        return b >= 233;
      case 14:
        return b >= 228;
      case 15:
        return b >= 223;
      case 16 /*0x10*/:
        return b >= 218;
      case 17:
        return b >= 213;
      case 18:
        return b >= 207;
      case 19:
        return b >= 202;
      case 20:
        return b >= 197;
      case 21:
        return b >= 192 /*0xC0*/;
      case 22:
        return b >= 187;
      case 23:
        return b >= 182;
      case 24:
        return b >= 176 /*0xB0*/;
      case 25:
        return b >= 171;
      case 26:
        return b >= 166;
      case 27:
        return b >= 161;
      case 28:
        return b >= 156;
      case 29:
        return b >= 151;
      case 30:
        return b >= 146;
      case 31 /*0x1F*/:
        return b >= 140;
      case 32 /*0x20*/:
        return b >= 135;
      case 33:
        return b >= 130;
      case 34:
        return b >= 125;
      case 35:
        return b >= 120;
      case 36:
        return b >= 115;
      case 37:
        return b >= 110;
      case 38:
        return b >= 104;
      case 39:
        return b >= 99;
      case 40:
        return b >= 94;
      case 41:
        return b >= 89;
      case 42:
        return b >= 84;
      case 43:
        return b >= 79;
      case 44:
        return b >= 74;
      case 45:
        return b >= 68;
      case 46:
        return b >= 63 /*0x3F*/;
      case 47:
        return b >= 58;
      case 48 /*0x30*/:
        return b >= 53;
      case 49:
        return b >= 48 /*0x30*/;
      case 50:
        return b >= 43;
      case 51:
        return b >= 37;
      case 52:
        return b >= 32 /*0x20*/;
      case 53:
        return b >= 27;
      case 54:
        return b >= 22;
      case 55:
        return b >= 17;
      case 56:
        return b >= 12;
      case 57:
        return b >= 7;
      case 58:
        return b >= 1;
      default:
        return g >= 59;
    }
  }

  private static bool CheckR90(int g, int b)
  {
    switch (g)
    {
      case 9:
        return b >= 251;
      case 10:
        return b >= 246;
      case 11:
        return b >= 241;
      case 12:
        return b >= 236;
      case 13:
        return b >= 231;
      case 14:
        return b >= 225;
      case 15:
        return b >= 220;
      case 16 /*0x10*/:
        return b >= 215;
      case 17:
        return b >= 210;
      case 18:
        return b >= 205;
      case 19:
        return b >= 200;
      case 20:
        return b >= 194;
      case 21:
        return b >= 189;
      case 22:
        return b >= 184;
      case 23:
        return b >= 179;
      case 24:
        return b >= 174;
      case 25:
        return b >= 169;
      case 26:
        return b >= 164;
      case 27:
        return b >= 158;
      case 28:
        return b >= 153;
      case 29:
        return b >= 148;
      case 30:
        return b >= 143;
      case 31 /*0x1F*/:
        return b >= 138;
      case 32 /*0x20*/:
        return b >= 133;
      case 33:
        return b >= 128 /*0x80*/;
      case 34:
        return b >= 122;
      case 35:
        return b >= 117;
      case 36:
        return b >= 112 /*0x70*/;
      case 37:
        return b >= 107;
      case 38:
        return b >= 102;
      case 39:
        return b >= 97;
      case 40:
        return b >= 91;
      case 41:
        return b >= 86;
      case 42:
        return b >= 81;
      case 43:
        return b >= 76;
      case 44:
        return b >= 71;
      case 45:
        return b >= 66;
      case 46:
        return b >= 61;
      case 47:
        return b >= 55;
      case 48 /*0x30*/:
        return b >= 50;
      case 49:
        return b >= 45;
      case 50:
        return b >= 40;
      case 51:
        return b >= 35;
      case 52:
        return b >= 30;
      case 53:
        return b >= 25;
      case 54:
        return b >= 19;
      case 55:
        return b >= 14;
      case 56:
        return b >= 9;
      case 57:
        return b >= 4;
      default:
        return g >= 58;
    }
  }

  private static bool CheckR91(int g, int b)
  {
    switch (g)
    {
      case 8:
        return b >= 254;
      case 9:
        return b >= 248;
      case 10:
        return b >= 243;
      case 11:
        return b >= 238;
      case 12:
        return b >= 233;
      case 13:
        return b >= 228;
      case 14:
        return b >= 223;
      case 15:
        return b >= 218;
      case 16 /*0x10*/:
        return b >= 212;
      case 17:
        return b >= 207;
      case 18:
        return b >= 202;
      case 19:
        return b >= 197;
      case 20:
        return b >= 192 /*0xC0*/;
      case 21:
        return b >= 187;
      case 22:
        return b >= 182;
      case 23:
        return b >= 176 /*0xB0*/;
      case 24:
        return b >= 171;
      case 25:
        return b >= 166;
      case 26:
        return b >= 161;
      case 27:
        return b >= 156;
      case 28:
        return b >= 151;
      case 29:
        return b >= 145;
      case 30:
        return b >= 140;
      case 31 /*0x1F*/:
        return b >= 135;
      case 32 /*0x20*/:
        return b >= 130;
      case 33:
        return b >= 125;
      case 34:
        return b >= 120;
      case 35:
        return b >= 115;
      case 36:
        return b >= 109;
      case 37:
        return b >= 104;
      case 38:
        return b >= 99;
      case 39:
        return b >= 94;
      case 40:
        return b >= 89;
      case 41:
        return b >= 84;
      case 42:
        return b >= 79;
      case 43:
        return b >= 73;
      case 44:
        return b >= 68;
      case 45:
        return b >= 63 /*0x3F*/;
      case 46:
        return b >= 58;
      case 47:
        return b >= 53;
      case 48 /*0x30*/:
        return b >= 48 /*0x30*/;
      case 49:
        return b >= 43;
      case 50:
        return b >= 37;
      case 51:
        return b >= 32 /*0x20*/;
      case 52:
        return b >= 27;
      case 53:
        return b >= 22;
      case 54:
        return b >= 17;
      case 55:
        return b >= 12;
      case 56:
        return b >= 6;
      case 57:
        return b >= 1;
      default:
        return g >= 58;
    }
  }

  private static bool CheckR92(int g, int b)
  {
    switch (g)
    {
      case 8:
        return b >= 251;
      case 9:
        return b >= 246;
      case 10:
        return b >= 241;
      case 11:
        return b >= 236;
      case 12:
        return b >= 230;
      case 13:
        return b >= 225;
      case 14:
        return b >= 220;
      case 15:
        return b >= 215;
      case 16 /*0x10*/:
        return b >= 210;
      case 17:
        return b >= 205;
      case 18:
        return b >= 200;
      case 19:
        return b >= 194;
      case 20:
        return b >= 189;
      case 21:
        return b >= 184;
      case 22:
        return b >= 179;
      case 23:
        return b >= 174;
      case 24:
        return b >= 169;
      case 25:
        return b >= 163;
      case 26:
        return b >= 158;
      case 27:
        return b >= 153;
      case 28:
        return b >= 148;
      case 29:
        return b >= 143;
      case 30:
        return b >= 138;
      case 31 /*0x1F*/:
        return b >= 133;
      case 32 /*0x20*/:
        return b >= (int) sbyte.MaxValue;
      case 33:
        return b >= 122;
      case 34:
        return b >= 117;
      case 35:
        return b >= 112 /*0x70*/;
      case 36:
        return b >= 107;
      case 37:
        return b >= 102;
      case 38:
        return b >= 97;
      case 39:
        return b >= 91;
      case 40:
        return b >= 86;
      case 41:
        return b >= 81;
      case 42:
        return b >= 76;
      case 43:
        return b >= 71;
      case 44:
        return b >= 66;
      case 45:
        return b >= 60;
      case 46:
        return b >= 55;
      case 47:
        return b >= 50;
      case 48 /*0x30*/:
        return b >= 45;
      case 49:
        return b >= 40;
      case 50:
        return b >= 35;
      case 51:
        return b >= 30;
      case 52:
        return b >= 24;
      case 53:
        return b >= 19;
      case 54:
        return b >= 14;
      case 55:
        return b >= 9;
      case 56:
        return b >= 4;
      default:
        return g >= 57;
    }
  }

  private static bool CheckR93(int g, int b)
  {
    switch (g)
    {
      case 7:
        return b >= 254;
      case 8:
        return b >= 248;
      case 9:
        return b >= 243;
      case 10:
        return b >= 238;
      case 11:
        return b >= 233;
      case 12:
        return b >= 228;
      case 13:
        return b >= 223;
      case 14:
        return b >= 217;
      case 15:
        return b >= 212;
      case 16 /*0x10*/:
        return b >= 207;
      case 17:
        return b >= 202;
      case 18:
        return b >= 197;
      case 19:
        return b >= 192 /*0xC0*/;
      case 20:
        return b >= 187;
      case 21:
        return b >= 181;
      case 22:
        return b >= 176 /*0xB0*/;
      case 23:
        return b >= 171;
      case 24:
        return b >= 166;
      case 25:
        return b >= 161;
      case 26:
        return b >= 156;
      case 27:
        return b >= 151;
      case 28:
        return b >= 145;
      case 29:
        return b >= 140;
      case 30:
        return b >= 135;
      case 31 /*0x1F*/:
        return b >= 130;
      case 32 /*0x20*/:
        return b >= 125;
      case 33:
        return b >= 120;
      case 34:
        return b >= 115;
      case 35:
        return b >= 109;
      case 36:
        return b >= 104;
      case 37:
        return b >= 99;
      case 38:
        return b >= 94;
      case 39:
        return b >= 89;
      case 40:
        return b >= 84;
      case 41:
        return b >= 78;
      case 42:
        return b >= 73;
      case 43:
        return b >= 68;
      case 44:
        return b >= 63 /*0x3F*/;
      case 45:
        return b >= 58;
      case 46:
        return b >= 53;
      case 47:
        return b >= 48 /*0x30*/;
      case 48 /*0x30*/:
        return b >= 42;
      case 49:
        return b >= 37;
      case 50:
        return b >= 32 /*0x20*/;
      case 51:
        return b >= 27;
      case 52:
        return b >= 22;
      case 53:
        return b >= 17;
      case 54:
        return b >= 12;
      case 55:
        return b >= 6;
      case 56:
        return b >= 1;
      default:
        return g >= 57;
    }
  }

  private static bool CheckR94(int g, int b)
  {
    switch (g)
    {
      case 7:
        return b >= 251;
      case 8:
        return b >= 246;
      case 9:
        return b >= 241;
      case 10:
        return b >= 235;
      case 11:
        return b >= 230;
      case 12:
        return b >= 225;
      case 13:
        return b >= 220;
      case 14:
        return b >= 215;
      case 15:
        return b >= 210;
      case 16 /*0x10*/:
        return b >= 205;
      case 17:
        return b >= 199;
      case 18:
        return b >= 194;
      case 19:
        return b >= 189;
      case 20:
        return b >= 184;
      case 21:
        return b >= 179;
      case 22:
        return b >= 174;
      case 23:
        return b >= 169;
      case 24:
        return b >= 163;
      case 25:
        return b >= 158;
      case 26:
        return b >= 153;
      case 27:
        return b >= 148;
      case 28:
        return b >= 143;
      case 29:
        return b >= 138;
      case 30:
        return b >= 132;
      case 31 /*0x1F*/:
        return b >= (int) sbyte.MaxValue;
      case 32 /*0x20*/:
        return b >= 122;
      case 33:
        return b >= 117;
      case 34:
        return b >= 112 /*0x70*/;
      case 35:
        return b >= 107;
      case 36:
        return b >= 102;
      case 37:
        return b >= 96 /*0x60*/;
      case 38:
        return b >= 91;
      case 39:
        return b >= 86;
      case 40:
        return b >= 81;
      case 41:
        return b >= 76;
      case 42:
        return b >= 71;
      case 43:
        return b >= 66;
      case 44:
        return b >= 60;
      case 45:
        return b >= 55;
      case 46:
        return b >= 50;
      case 47:
        return b >= 45;
      case 48 /*0x30*/:
        return b >= 40;
      case 49:
        return b >= 35;
      case 50:
        return b >= 29;
      case 51:
        return b >= 24;
      case 52:
        return b >= 19;
      case 53:
        return b >= 14;
      case 54:
        return b >= 9;
      case 55:
        return b >= 4;
      default:
        return g >= 56;
    }
  }

  private static bool CheckR95(int g, int b)
  {
    switch (g)
    {
      case 6:
        return b >= 253;
      case 7:
        return b >= 248;
      case 8:
        return b >= 243;
      case 9:
        return b >= 238;
      case 10:
        return b >= 233;
      case 11:
        return b >= 228;
      case 12:
        return b >= 223;
      case 13:
        return b >= 217;
      case 14:
        return b >= 212;
      case 15:
        return b >= 207;
      case 16 /*0x10*/:
        return b >= 202;
      case 17:
        return b >= 197;
      case 18:
        return b >= 192 /*0xC0*/;
      case 19:
        return b >= 186;
      case 20:
        return b >= 181;
      case 21:
        return b >= 176 /*0xB0*/;
      case 22:
        return b >= 171;
      case 23:
        return b >= 166;
      case 24:
        return b >= 161;
      case 25:
        return b >= 156;
      case 26:
        return b >= 150;
      case 27:
        return b >= 145;
      case 28:
        return b >= 140;
      case 29:
        return b >= 135;
      case 30:
        return b >= 130;
      case 31 /*0x1F*/:
        return b >= 125;
      case 32 /*0x20*/:
        return b >= 120;
      case 33:
        return b >= 114;
      case 34:
        return b >= 109;
      case 35:
        return b >= 104;
      case 36:
        return b >= 99;
      case 37:
        return b >= 94;
      case 38:
        return b >= 89;
      case 39:
        return b >= 84;
      case 40:
        return b >= 78;
      case 41:
        return b >= 73;
      case 42:
        return b >= 68;
      case 43:
        return b >= 63 /*0x3F*/;
      case 44:
        return b >= 58;
      case 45:
        return b >= 53;
      case 46:
        return b >= 47;
      case 47:
        return b >= 42;
      case 48 /*0x30*/:
        return b >= 37;
      case 49:
        return b >= 32 /*0x20*/;
      case 50:
        return b >= 27;
      case 51:
        return b >= 22;
      case 52:
        return b >= 17;
      case 53:
        return b >= 11;
      case 54:
        return b >= 6;
      case 55:
        return b >= 1;
      default:
        return g >= 56;
    }
  }

  private static bool CheckR96(int g, int b)
  {
    switch (g)
    {
      case 6:
        return b >= 251;
      case 7:
        return b >= 246;
      case 8:
        return b >= 241;
      case 9:
        return b >= 235;
      case 10:
        return b >= 230;
      case 11:
        return b >= 225;
      case 12:
        return b >= 220;
      case 13:
        return b >= 215;
      case 14:
        return b >= 210;
      case 15:
        return b >= 204;
      case 16 /*0x10*/:
        return b >= 199;
      case 17:
        return b >= 194;
      case 18:
        return b >= 189;
      case 19:
        return b >= 184;
      case 20:
        return b >= 179;
      case 21:
        return b >= 174;
      case 22:
        return b >= 168;
      case 23:
        return b >= 163;
      case 24:
        return b >= 158;
      case 25:
        return b >= 153;
      case 26:
        return b >= 148;
      case 27:
        return b >= 143;
      case 28:
        return b >= 138;
      case 29:
        return b >= 132;
      case 30:
        return b >= (int) sbyte.MaxValue;
      case 31 /*0x1F*/:
        return b >= 122;
      case 32 /*0x20*/:
        return b >= 117;
      case 33:
        return b >= 112 /*0x70*/;
      case 34:
        return b >= 107;
      case 35:
        return b >= 101;
      case 36:
        return b >= 96 /*0x60*/;
      case 37:
        return b >= 91;
      case 38:
        return b >= 86;
      case 39:
        return b >= 81;
      case 40:
        return b >= 76;
      case 41:
        return b >= 71;
      case 42:
        return b >= 65;
      case 43:
        return b >= 60;
      case 44:
        return b >= 55;
      case 45:
        return b >= 50;
      case 46:
        return b >= 45;
      case 47:
        return b >= 40;
      case 48 /*0x30*/:
        return b >= 35;
      case 49:
        return b >= 29;
      case 50:
        return b >= 24;
      case 51:
        return b >= 19;
      case 52:
        return b >= 14;
      case 53:
        return b >= 9;
      case 54:
        return b >= 4;
      default:
        return g >= 55;
    }
  }

  private static bool CheckR97(int g, int b)
  {
    switch (g)
    {
      case 5:
        return b >= 253;
      case 6:
        return b >= 248;
      case 7:
        return b >= 243;
      case 8:
        return b >= 238;
      case 9:
        return b >= 233;
      case 10:
        return b >= 228;
      case 11:
        return b >= 222;
      case 12:
        return b >= 217;
      case 13:
        return b >= 212;
      case 14:
        return b >= 207;
      case 15:
        return b >= 202;
      case 16 /*0x10*/:
        return b >= 197;
      case 17:
        return b >= 192 /*0xC0*/;
      case 18:
        return b >= 186;
      case 19:
        return b >= 181;
      case 20:
        return b >= 176 /*0xB0*/;
      case 21:
        return b >= 171;
      case 22:
        return b >= 166;
      case 23:
        return b >= 161;
      case 24:
        return b >= 156;
      case 25:
        return b >= 150;
      case 26:
        return b >= 145;
      case 27:
        return b >= 140;
      case 28:
        return b >= 135;
      case 29:
        return b >= 130;
      case 30:
        return b >= 125;
      case 31 /*0x1F*/:
        return b >= 119;
      case 32 /*0x20*/:
        return b >= 114;
      case 33:
        return b >= 109;
      case 34:
        return b >= 104;
      case 35:
        return b >= 99;
      case 36:
        return b >= 94;
      case 37:
        return b >= 89;
      case 38:
        return b >= 83;
      case 39:
        return b >= 78;
      case 40:
        return b >= 73;
      case 41:
        return b >= 68;
      case 42:
        return b >= 63 /*0x3F*/;
      case 43:
        return b >= 58;
      case 44:
        return b >= 53;
      case 45:
        return b >= 47;
      case 46:
        return b >= 42;
      case 47:
        return b >= 37;
      case 48 /*0x30*/:
        return b >= 32 /*0x20*/;
      case 49:
        return b >= 27;
      case 50:
        return b >= 22;
      case 51:
        return b >= 16 /*0x10*/;
      case 52:
        return b >= 11;
      case 53:
        return b >= 6;
      case 54:
        return b >= 1;
      default:
        return g >= 55;
    }
  }

  private static bool CheckR98(int g, int b)
  {
    switch (g)
    {
      case 5:
        return b >= 251;
      case 6:
        return b >= 246;
      case 7:
        return b >= 240 /*0xF0*/;
      case 8:
        return b >= 235;
      case 9:
        return b >= 230;
      case 10:
        return b >= 225;
      case 11:
        return b >= 220;
      case 12:
        return b >= 215;
      case 13:
        return b >= 210;
      case 14:
        return b >= 204;
      case 15:
        return b >= 199;
      case 16 /*0x10*/:
        return b >= 194;
      case 17:
        return b >= 189;
      case 18:
        return b >= 184;
      case 19:
        return b >= 179;
      case 20:
        return b >= 173;
      case 21:
        return b >= 168;
      case 22:
        return b >= 163;
      case 23:
        return b >= 158;
      case 24:
        return b >= 153;
      case 25:
        return b >= 148;
      case 26:
        return b >= 143;
      case 27:
        return b >= 137;
      case 28:
        return b >= 132;
      case 29:
        return b >= (int) sbyte.MaxValue;
      case 30:
        return b >= 122;
      case 31 /*0x1F*/:
        return b >= 117;
      case 32 /*0x20*/:
        return b >= 112 /*0x70*/;
      case 33:
        return b >= 107;
      case 34:
        return b >= 101;
      case 35:
        return b >= 96 /*0x60*/;
      case 36:
        return b >= 91;
      case 37:
        return b >= 86;
      case 38:
        return b >= 81;
      case 39:
        return b >= 76;
      case 40:
        return b >= 70;
      case 41:
        return b >= 65;
      case 42:
        return b >= 60;
      case 43:
        return b >= 55;
      case 44:
        return b >= 50;
      case 45:
        return b >= 45;
      case 46:
        return b >= 40;
      case 47:
        return b >= 34;
      case 48 /*0x30*/:
        return b >= 29;
      case 49:
        return b >= 24;
      case 50:
        return b >= 19;
      case 51:
        return b >= 14;
      case 52:
        return b >= 9;
      case 53:
        return b >= 4;
      default:
        return g >= 54;
    }
  }

  private static bool CheckR99(int g, int b)
  {
    switch (g)
    {
      case 4:
        return b >= 253;
      case 5:
        return b >= 248;
      case 6:
        return b >= 243;
      case 7:
        return b >= 238;
      case 8:
        return b >= 233;
      case 9:
        return b >= 227;
      case 10:
        return b >= 222;
      case 11:
        return b >= 217;
      case 12:
        return b >= 212;
      case 13:
        return b >= 207;
      case 14:
        return b >= 202;
      case 15:
        return b >= 197;
      case 16 /*0x10*/:
        return b >= 191;
      case 17:
        return b >= 186;
      case 18:
        return b >= 181;
      case 19:
        return b >= 176 /*0xB0*/;
      case 20:
        return b >= 171;
      case 21:
        return b >= 166;
      case 22:
        return b >= 161;
      case 23:
        return b >= 155;
      case 24:
        return b >= 150;
      case 25:
        return b >= 145;
      case 26:
        return b >= 140;
      case 27:
        return b >= 135;
      case 28:
        return b >= 130;
      case 29:
        return b >= 125;
      case 30:
        return b >= 119;
      case 31 /*0x1F*/:
        return b >= 114;
      case 32 /*0x20*/:
        return b >= 109;
      case 33:
        return b >= 104;
      case 34:
        return b >= 99;
      case 35:
        return b >= 94;
      case 36:
        return b >= 88;
      case 37:
        return b >= 83;
      case 38:
        return b >= 78;
      case 39:
        return b >= 73;
      case 40:
        return b >= 68;
      case 41:
        return b >= 63 /*0x3F*/;
      case 42:
        return b >= 58;
      case 43:
        return b >= 52;
      case 44:
        return b >= 47;
      case 45:
        return b >= 42;
      case 46:
        return b >= 37;
      case 47:
        return b >= 32 /*0x20*/;
      case 48 /*0x30*/:
        return b >= 27;
      case 49:
        return b >= 22;
      case 50:
        return b >= 16 /*0x10*/;
      case 51:
        return b >= 11;
      case 52:
        return b >= 6;
      case 53:
        return b >= 1;
      default:
        return g >= 54;
    }
  }

  private static bool CheckR100(int g, int b)
  {
    switch (g)
    {
      case 4:
        return b >= 251;
      case 5:
        return b >= 245;
      case 6:
        return b >= 240 /*0xF0*/;
      case 7:
        return b >= 235;
      case 8:
        return b >= 230;
      case 9:
        return b >= 225;
      case 10:
        return b >= 220;
      case 11:
        return b >= 215;
      case 12:
        return b >= 209;
      case 13:
        return b >= 204;
      case 14:
        return b >= 199;
      case 15:
        return b >= 194;
      case 16 /*0x10*/:
        return b >= 189;
      case 17:
        return b >= 184;
      case 18:
        return b >= 179;
      case 19:
        return b >= 173;
      case 20:
        return b >= 168;
      case 21:
        return b >= 163;
      case 22:
        return b >= 158;
      case 23:
        return b >= 153;
      case 24:
        return b >= 148;
      case 25:
        return b >= 142;
      case 26:
        return b >= 137;
      case 27:
        return b >= 132;
      case 28:
        return b >= (int) sbyte.MaxValue;
      case 29:
        return b >= 122;
      case 30:
        return b >= 117;
      case 31 /*0x1F*/:
        return b >= 112 /*0x70*/;
      case 32 /*0x20*/:
        return b >= 106;
      case 33:
        return b >= 101;
      case 34:
        return b >= 96 /*0x60*/;
      case 35:
        return b >= 91;
      case 36:
        return b >= 86;
      case 37:
        return b >= 81;
      case 38:
        return b >= 76;
      case 39:
        return b >= 70;
      case 40:
        return b >= 65;
      case 41:
        return b >= 60;
      case 42:
        return b >= 55;
      case 43:
        return b >= 50;
      case 44:
        return b >= 45;
      case 45:
        return b >= 40;
      case 46:
        return b >= 34;
      case 47:
        return b >= 29;
      case 48 /*0x30*/:
        return b >= 24;
      case 49:
        return b >= 19;
      case 50:
        return b >= 14;
      case 51:
        return b >= 9;
      case 52:
        return b >= 3;
      default:
        return g >= 53;
    }
  }

  private static bool CheckR101(int g, int b)
  {
    switch (g)
    {
      case 3:
        return b >= 253;
      case 4:
        return b >= 248;
      case 5:
        return b >= 243;
      case 6:
        return b >= 238;
      case 7:
        return b >= 233;
      case 8:
        return b >= 227;
      case 9:
        return b >= 222;
      case 10:
        return b >= 217;
      case 11:
        return b >= 212;
      case 12:
        return b >= 207;
      case 13:
        return b >= 202;
      case 14:
        return b >= 197;
      case 15:
        return b >= 191;
      case 16 /*0x10*/:
        return b >= 186;
      case 17:
        return b >= 181;
      case 18:
        return b >= 176 /*0xB0*/;
      case 19:
        return b >= 171;
      case 20:
        return b >= 166;
      case 21:
        return b >= 160 /*0xA0*/;
      case 22:
        return b >= 155;
      case 23:
        return b >= 150;
      case 24:
        return b >= 145;
      case 25:
        return b >= 140;
      case 26:
        return b >= 135;
      case 27:
        return b >= 130;
      case 28:
        return b >= 124;
      case 29:
        return b >= 119;
      case 30:
        return b >= 114;
      case 31 /*0x1F*/:
        return b >= 109;
      case 32 /*0x20*/:
        return b >= 104;
      case 33:
        return b >= 99;
      case 34:
        return b >= 94;
      case 35:
        return b >= 88;
      case 36:
        return b >= 83;
      case 37:
        return b >= 78;
      case 38:
        return b >= 73;
      case 39:
        return b >= 68;
      case 40:
        return b >= 63 /*0x3F*/;
      case 41:
        return b >= 57;
      case 42:
        return b >= 52;
      case 43:
        return b >= 47;
      case 44:
        return b >= 42;
      case 45:
        return b >= 37;
      case 46:
        return b >= 32 /*0x20*/;
      case 47:
        return b >= 27;
      case 48 /*0x30*/:
        return b >= 21;
      case 49:
        return b >= 16 /*0x10*/;
      case 50:
        return b >= 11;
      case 51:
        return b >= 6;
      case 52:
        return b >= 1;
      default:
        return g >= 53;
    }
  }

  private static bool CheckR102(int g, int b)
  {
    switch (g)
    {
      case 3:
        return b >= 251;
      case 4:
        return b >= 245;
      case 5:
        return b >= 240 /*0xF0*/;
      case 6:
        return b >= 235;
      case 7:
        return b >= 230;
      case 8:
        return b >= 225;
      case 9:
        return b >= 220;
      case 10:
        return b >= 214;
      case 11:
        return b >= 209;
      case 12:
        return b >= 204;
      case 13:
        return b >= 199;
      case 14:
        return b >= 194;
      case 15:
        return b >= 189;
      case 16 /*0x10*/:
        return b >= 184;
      case 17:
        return b >= 178;
      case 18:
        return b >= 173;
      case 19:
        return b >= 168;
      case 20:
        return b >= 163;
      case 21:
        return b >= 158;
      case 22:
        return b >= 153;
      case 23:
        return b >= 148;
      case 24:
        return b >= 142;
      case 25:
        return b >= 137;
      case 26:
        return b >= 132;
      case 27:
        return b >= (int) sbyte.MaxValue;
      case 28:
        return b >= 122;
      case 29:
        return b >= 117;
      case 30:
        return b >= 111;
      case 31 /*0x1F*/:
        return b >= 106;
      case 32 /*0x20*/:
        return b >= 101;
      case 33:
        return b >= 96 /*0x60*/;
      case 34:
        return b >= 91;
      case 35:
        return b >= 86;
      case 36:
        return b >= 81;
      case 37:
        return b >= 75;
      case 38:
        return b >= 70;
      case 39:
        return b >= 65;
      case 40:
        return b >= 60;
      case 41:
        return b >= 55;
      case 42:
        return b >= 50;
      case 43:
        return b >= 45;
      case 44:
        return b >= 39;
      case 45:
        return b >= 34;
      case 46:
        return b >= 29;
      case 47:
        return b >= 24;
      case 48 /*0x30*/:
        return b >= 19;
      case 49:
        return b >= 14;
      case 50:
        return b >= 9;
      case 51:
        return b >= 3;
      default:
        return g >= 52;
    }
  }

  private static bool CheckR103(int g, int b)
  {
    switch (g)
    {
      case 2:
        return b >= 253;
      case 3:
        return b >= 248;
      case 4:
        return b >= 243;
      case 5:
        return b >= 238;
      case 6:
        return b >= 232;
      case 7:
        return b >= 227;
      case 8:
        return b >= 222;
      case 9:
        return b >= 217;
      case 10:
        return b >= 212;
      case 11:
        return b >= 207;
      case 12:
        return b >= 202;
      case 13:
        return b >= 196;
      case 14:
        return b >= 191;
      case 15:
        return b >= 186;
      case 16 /*0x10*/:
        return b >= 181;
      case 17:
        return b >= 176 /*0xB0*/;
      case 18:
        return b >= 171;
      case 19:
        return b >= 166;
      case 20:
        return b >= 160 /*0xA0*/;
      case 21:
        return b >= 155;
      case 22:
        return b >= 150;
      case 23:
        return b >= 145;
      case 24:
        return b >= 140;
      case 25:
        return b >= 135;
      case 26:
        return b >= 129;
      case 27:
        return b >= 124;
      case 28:
        return b >= 119;
      case 29:
        return b >= 114;
      case 30:
        return b >= 109;
      case 31 /*0x1F*/:
        return b >= 104;
      case 32 /*0x20*/:
        return b >= 99;
      case 33:
        return b >= 93;
      case 34:
        return b >= 88;
      case 35:
        return b >= 83;
      case 36:
        return b >= 78;
      case 37:
        return b >= 73;
      case 38:
        return b >= 68;
      case 39:
        return b >= 63 /*0x3F*/;
      case 40:
        return b >= 57;
      case 41:
        return b >= 52;
      case 42:
        return b >= 47;
      case 43:
        return b >= 42;
      case 44:
        return b >= 37;
      case 45:
        return b >= 32 /*0x20*/;
      case 46:
        return b >= 26;
      case 47:
        return b >= 21;
      case 48 /*0x30*/:
        return b >= 16 /*0x10*/;
      case 49:
        return b >= 11;
      case 50:
        return b >= 6;
      case 51:
        return b >= 1;
      default:
        return g >= 52;
    }
  }

  private static bool CheckR104(int g, int b)
  {
    switch (g)
    {
      case 2:
        return b >= 250;
      case 3:
        return b >= 245;
      case 4:
        return b >= 240 /*0xF0*/;
      case 5:
        return b >= 235;
      case 6:
        return b >= 230;
      case 7:
        return b >= 225;
      case 8:
        return b >= 220;
      case 9:
        return b >= 214;
      case 10:
        return b >= 209;
      case 11:
        return b >= 204;
      case 12:
        return b >= 199;
      case 13:
        return b >= 194;
      case 14:
        return b >= 189;
      case 15:
        return b >= 183;
      case 16 /*0x10*/:
        return b >= 178;
      case 17:
        return b >= 173;
      case 18:
        return b >= 168;
      case 19:
        return b >= 163;
      case 20:
        return b >= 158;
      case 21:
        return b >= 153;
      case 22:
        return b >= 147;
      case 23:
        return b >= 142;
      case 24:
        return b >= 137;
      case 25:
        return b >= 132;
      case 26:
        return b >= (int) sbyte.MaxValue;
      case 27:
        return b >= 122;
      case 28:
        return b >= 117;
      case 29:
        return b >= 111;
      case 30:
        return b >= 106;
      case 31 /*0x1F*/:
        return b >= 101;
      case 32 /*0x20*/:
        return b >= 96 /*0x60*/;
      case 33:
        return b >= 91;
      case 34:
        return b >= 86;
      case 35:
        return b >= 81;
      case 36:
        return b >= 75;
      case 37:
        return b >= 70;
      case 38:
        return b >= 65;
      case 39:
        return b >= 60;
      case 40:
        return b >= 55;
      case 41:
        return b >= 50;
      case 42:
        return b >= 44;
      case 43:
        return b >= 39;
      case 44:
        return b >= 34;
      case 45:
        return b >= 29;
      case 46:
        return b >= 24;
      case 47:
        return b >= 19;
      case 48 /*0x30*/:
        return b >= 14;
      case 49:
        return b >= 8;
      case 50:
        return b >= 3;
      default:
        return g >= 51;
    }
  }

  private static bool CheckR105(int g, int b)
  {
    switch (g)
    {
      case 1:
        return b >= 253;
      case 2:
        return b >= 248;
      case 3:
        return b >= 243;
      case 4:
        return b >= 238;
      case 5:
        return b >= 232;
      case 6:
        return b >= 227;
      case 7:
        return b >= 222;
      case 8:
        return b >= 217;
      case 9:
        return b >= 212;
      case 10:
        return b >= 207;
      case 11:
        return b >= 201;
      case 12:
        return b >= 196;
      case 13:
        return b >= 191;
      case 14:
        return b >= 186;
      case 15:
        return b >= 181;
      case 16 /*0x10*/:
        return b >= 176 /*0xB0*/;
      case 17:
        return b >= 171;
      case 18:
        return b >= 165;
      case 19:
        return b >= 160 /*0xA0*/;
      case 20:
        return b >= 155;
      case 21:
        return b >= 150;
      case 22:
        return b >= 145;
      case 23:
        return b >= 140;
      case 24:
        return b >= 135;
      case 25:
        return b >= 129;
      case 26:
        return b >= 124;
      case 27:
        return b >= 119;
      case 28:
        return b >= 114;
      case 29:
        return b >= 109;
      case 30:
        return b >= 104;
      case 31 /*0x1F*/:
        return b >= 98;
      case 32 /*0x20*/:
        return b >= 93;
      case 33:
        return b >= 88;
      case 34:
        return b >= 83;
      case 35:
        return b >= 78;
      case 36:
        return b >= 73;
      case 37:
        return b >= 68;
      case 38:
        return b >= 62;
      case 39:
        return b >= 57;
      case 40:
        return b >= 52;
      case 41:
        return b >= 47;
      case 42:
        return b >= 42;
      case 43:
        return b >= 37;
      case 44:
        return b >= 32 /*0x20*/;
      case 45:
        return b >= 26;
      case 46:
        return b >= 21;
      case 47:
        return b >= 16 /*0x10*/;
      case 48 /*0x30*/:
        return b >= 11;
      case 49:
        return b >= 6;
      case 50:
        return b >= 1;
      default:
        return g >= 51;
    }
  }

  private static bool CheckR106(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= (int) byte.MaxValue;
      case 1:
        return b >= 250;
      case 2:
        return b >= 245;
      case 3:
        return b >= 240 /*0xF0*/;
      case 4:
        return b >= 235;
      case 5:
        return b >= 230;
      case 6:
        return b >= 225;
      case 7:
        return b >= 219;
      case 8:
        return b >= 214;
      case 9:
        return b >= 209;
      case 10:
        return b >= 204;
      case 11:
        return b >= 199;
      case 12:
        return b >= 194;
      case 13:
        return b >= 189;
      case 14:
        return b >= 183;
      case 15:
        return b >= 178;
      case 16 /*0x10*/:
        return b >= 173;
      case 17:
        return b >= 168;
      case 18:
        return b >= 163;
      case 19:
        return b >= 158;
      case 20:
        return b >= 152;
      case 21:
        return b >= 147;
      case 22:
        return b >= 142;
      case 23:
        return b >= 137;
      case 24:
        return b >= 132;
      case 25:
        return b >= (int) sbyte.MaxValue;
      case 26:
        return b >= 122;
      case 27:
        return b >= 116;
      case 28:
        return b >= 111;
      case 29:
        return b >= 106;
      case 30:
        return b >= 101;
      case 31 /*0x1F*/:
        return b >= 96 /*0x60*/;
      case 32 /*0x20*/:
        return b >= 91;
      case 33:
        return b >= 86;
      case 34:
        return b >= 80 /*0x50*/;
      case 35:
        return b >= 75;
      case 36:
        return b >= 70;
      case 37:
        return b >= 65;
      case 38:
        return b >= 60;
      case 39:
        return b >= 55;
      case 40:
        return b >= 50;
      case 41:
        return b >= 44;
      case 42:
        return b >= 39;
      case 43:
        return b >= 34;
      case 44:
        return b >= 29;
      case 45:
        return b >= 24;
      case 46:
        return b >= 19;
      case 47:
        return b >= 13;
      case 48 /*0x30*/:
        return b >= 8;
      case 49:
        return b >= 3;
      default:
        return g >= 50;
    }
  }

  private static bool CheckR107(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 253;
      case 1:
        return b >= 248;
      case 2:
        return b >= 243;
      case 3:
        return b >= 237;
      case 4:
        return b >= 232;
      case 5:
        return b >= 227;
      case 6:
        return b >= 222;
      case 7:
        return b >= 217;
      case 8:
        return b >= 212;
      case 9:
        return b >= 207;
      case 10:
        return b >= 201;
      case 11:
        return b >= 196;
      case 12:
        return b >= 191;
      case 13:
        return b >= 186;
      case 14:
        return b >= 181;
      case 15:
        return b >= 176 /*0xB0*/;
      case 16 /*0x10*/:
        return b >= 170;
      case 17:
        return b >= 165;
      case 18:
        return b >= 160 /*0xA0*/;
      case 19:
        return b >= 155;
      case 20:
        return b >= 150;
      case 21:
        return b >= 145;
      case 22:
        return b >= 140;
      case 23:
        return b >= 134;
      case 24:
        return b >= 129;
      case 25:
        return b >= 124;
      case 26:
        return b >= 119;
      case 27:
        return b >= 114;
      case 28:
        return b >= 109;
      case 29:
        return b >= 104;
      case 30:
        return b >= 98;
      case 31 /*0x1F*/:
        return b >= 93;
      case 32 /*0x20*/:
        return b >= 88;
      case 33:
        return b >= 83;
      case 34:
        return b >= 78;
      case 35:
        return b >= 73;
      case 36:
        return b >= 67;
      case 37:
        return b >= 62;
      case 38:
        return b >= 57;
      case 39:
        return b >= 52;
      case 40:
        return b >= 47;
      case 41:
        return b >= 42;
      case 42:
        return b >= 37;
      case 43:
        return b >= 31 /*0x1F*/;
      case 44:
        return b >= 26;
      case 45:
        return b >= 21;
      case 46:
        return b >= 16 /*0x10*/;
      case 47:
        return b >= 11;
      case 48 /*0x30*/:
        return b >= 6;
      case 49:
        return b >= 1;
      default:
        return g >= 50;
    }
  }

  private static bool CheckR108(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 250;
      case 1:
        return b >= 245;
      case 2:
        return b >= 240 /*0xF0*/;
      case 3:
        return b >= 235;
      case 4:
        return b >= 230;
      case 5:
        return b >= 224 /*0xE0*/;
      case 6:
        return b >= 219;
      case 7:
        return b >= 214;
      case 8:
        return b >= 209;
      case 9:
        return b >= 204;
      case 10:
        return b >= 199;
      case 11:
        return b >= 194;
      case 12:
        return b >= 188;
      case 13:
        return b >= 183;
      case 14:
        return b >= 178;
      case 15:
        return b >= 173;
      case 16 /*0x10*/:
        return b >= 168;
      case 17:
        return b >= 163;
      case 18:
        return b >= 158;
      case 19:
        return b >= 152;
      case 20:
        return b >= 147;
      case 21:
        return b >= 142;
      case 22:
        return b >= 137;
      case 23:
        return b >= 132;
      case 24:
        return b >= (int) sbyte.MaxValue;
      case 25:
        return b >= 122;
      case 26:
        return b >= 116;
      case 27:
        return b >= 111;
      case 28:
        return b >= 106;
      case 29:
        return b >= 101;
      case 30:
        return b >= 96 /*0x60*/;
      case 31 /*0x1F*/:
        return b >= 91;
      case 32 /*0x20*/:
        return b >= 85;
      case 33:
        return b >= 80 /*0x50*/;
      case 34:
        return b >= 75;
      case 35:
        return b >= 70;
      case 36:
        return b >= 65;
      case 37:
        return b >= 60;
      case 38:
        return b >= 55;
      case 39:
        return b >= 49;
      case 40:
        return b >= 44;
      case 41:
        return b >= 39;
      case 42:
        return b >= 34;
      case 43:
        return b >= 29;
      case 44:
        return b >= 24;
      case 45:
        return b >= 19;
      case 46:
        return b >= 13;
      case 47:
        return b >= 8;
      case 48 /*0x30*/:
        return b >= 3;
      default:
        return g >= 49;
    }
  }

  private static bool CheckR109(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 248;
      case 1:
        return b >= 242;
      case 2:
        return b >= 237;
      case 3:
        return b >= 232;
      case 4:
        return b >= 227;
      case 5:
        return b >= 222;
      case 6:
        return b >= 217;
      case 7:
        return b >= 212;
      case 8:
        return b >= 206;
      case 9:
        return b >= 201;
      case 10:
        return b >= 196;
      case 11:
        return b >= 191;
      case 12:
        return b >= 186;
      case 13:
        return b >= 181;
      case 14:
        return b >= 176 /*0xB0*/;
      case 15:
        return b >= 170;
      case 16 /*0x10*/:
        return b >= 165;
      case 17:
        return b >= 160 /*0xA0*/;
      case 18:
        return b >= 155;
      case 19:
        return b >= 150;
      case 20:
        return b >= 145;
      case 21:
        return b >= 139;
      case 22:
        return b >= 134;
      case 23:
        return b >= 129;
      case 24:
        return b >= 124;
      case 25:
        return b >= 119;
      case 26:
        return b >= 114;
      case 27:
        return b >= 109;
      case 28:
        return b >= 103;
      case 29:
        return b >= 98;
      case 30:
        return b >= 93;
      case 31 /*0x1F*/:
        return b >= 88;
      case 32 /*0x20*/:
        return b >= 83;
      case 33:
        return b >= 78;
      case 34:
        return b >= 73;
      case 35:
        return b >= 67;
      case 36:
        return b >= 62;
      case 37:
        return b >= 57;
      case 38:
        return b >= 52;
      case 39:
        return b >= 47;
      case 40:
        return b >= 42;
      case 41:
        return b >= 36;
      case 42:
        return b >= 31 /*0x1F*/;
      case 43:
        return b >= 26;
      case 44:
        return b >= 21;
      case 45:
        return b >= 16 /*0x10*/;
      case 46:
        return b >= 11;
      case 47:
        return b >= 6;
      default:
        return g >= 48 /*0x30*/;
    }
  }

  private static bool CheckR110(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 245;
      case 1:
        return b >= 240 /*0xF0*/;
      case 2:
        return b >= 235;
      case 3:
        return b >= 230;
      case 4:
        return b >= 224 /*0xE0*/;
      case 5:
        return b >= 219;
      case 6:
        return b >= 214;
      case 7:
        return b >= 209;
      case 8:
        return b >= 204;
      case 9:
        return b >= 199;
      case 10:
        return b >= 193;
      case 11:
        return b >= 188;
      case 12:
        return b >= 183;
      case 13:
        return b >= 178;
      case 14:
        return b >= 173;
      case 15:
        return b >= 168;
      case 16 /*0x10*/:
        return b >= 163;
      case 17:
        return b >= 157;
      case 18:
        return b >= 152;
      case 19:
        return b >= 147;
      case 20:
        return b >= 142;
      case 21:
        return b >= 137;
      case 22:
        return b >= 132;
      case 23:
        return b >= (int) sbyte.MaxValue;
      case 24:
        return b >= 121;
      case 25:
        return b >= 116;
      case 26:
        return b >= 111;
      case 27:
        return b >= 106;
      case 28:
        return b >= 101;
      case 29:
        return b >= 96 /*0x60*/;
      case 30:
        return b >= 91;
      case 31 /*0x1F*/:
        return b >= 85;
      case 32 /*0x20*/:
        return b >= 80 /*0x50*/;
      case 33:
        return b >= 75;
      case 34:
        return b >= 70;
      case 35:
        return b >= 65;
      case 36:
        return b >= 60;
      case 37:
        return b >= 54;
      case 38:
        return b >= 49;
      case 39:
        return b >= 44;
      case 40:
        return b >= 39;
      case 41:
        return b >= 34;
      case 42:
        return b >= 29;
      case 43:
        return b >= 24;
      case 44:
        return b >= 18;
      case 45:
        return b >= 13;
      case 46:
        return b >= 8;
      case 47:
        return b >= 3;
      default:
        return g >= 48 /*0x30*/;
    }
  }

  private static bool CheckR111(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 242;
      case 1:
        return b >= 237;
      case 2:
        return b >= 232;
      case 3:
        return b >= 227;
      case 4:
        return b >= 222;
      case 5:
        return b >= 217;
      case 6:
        return b >= 211;
      case 7:
        return b >= 206;
      case 8:
        return b >= 201;
      case 9:
        return b >= 196;
      case 10:
        return b >= 191;
      case 11:
        return b >= 186;
      case 12:
        return b >= 181;
      case 13:
        return b >= 175;
      case 14:
        return b >= 170;
      case 15:
        return b >= 165;
      case 16 /*0x10*/:
        return b >= 160 /*0xA0*/;
      case 17:
        return b >= 155;
      case 18:
        return b >= 150;
      case 19:
        return b >= 145;
      case 20:
        return b >= 139;
      case 21:
        return b >= 134;
      case 22:
        return b >= 129;
      case 23:
        return b >= 124;
      case 24:
        return b >= 119;
      case 25:
        return b >= 114;
      case 26:
        return b >= 108;
      case 27:
        return b >= 103;
      case 28:
        return b >= 98;
      case 29:
        return b >= 93;
      case 30:
        return b >= 88;
      case 31 /*0x1F*/:
        return b >= 83;
      case 32 /*0x20*/:
        return b >= 78;
      case 33:
        return b >= 72;
      case 34:
        return b >= 67;
      case 35:
        return b >= 62;
      case 36:
        return b >= 57;
      case 37:
        return b >= 52;
      case 38:
        return b >= 47;
      case 39:
        return b >= 42;
      case 40:
        return b >= 36;
      case 41:
        return b >= 31 /*0x1F*/;
      case 42:
        return b >= 26;
      case 43:
        return b >= 21;
      case 44:
        return b >= 16 /*0x10*/;
      case 45:
        return b >= 11;
      case 46:
        return b >= 6;
      default:
        return g >= 47;
    }
  }

  private static bool CheckR112(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 240 /*0xF0*/;
      case 1:
        return b >= 235;
      case 2:
        return b >= 229;
      case 3:
        return b >= 224 /*0xE0*/;
      case 4:
        return b >= 219;
      case 5:
        return b >= 214;
      case 6:
        return b >= 209;
      case 7:
        return b >= 204;
      case 8:
        return b >= 199;
      case 9:
        return b >= 193;
      case 10:
        return b >= 188;
      case 11:
        return b >= 183;
      case 12:
        return b >= 178;
      case 13:
        return b >= 173;
      case 14:
        return b >= 168;
      case 15:
        return b >= 163;
      case 16 /*0x10*/:
        return b >= 157;
      case 17:
        return b >= 152;
      case 18:
        return b >= 147;
      case 19:
        return b >= 142;
      case 20:
        return b >= 137;
      case 21:
        return b >= 132;
      case 22:
        return b >= 126;
      case 23:
        return b >= 121;
      case 24:
        return b >= 116;
      case 25:
        return b >= 111;
      case 26:
        return b >= 106;
      case 27:
        return b >= 101;
      case 28:
        return b >= 96 /*0x60*/;
      case 29:
        return b >= 90;
      case 30:
        return b >= 85;
      case 31 /*0x1F*/:
        return b >= 80 /*0x50*/;
      case 32 /*0x20*/:
        return b >= 75;
      case 33:
        return b >= 70;
      case 34:
        return b >= 65;
      case 35:
        return b >= 60;
      case 36:
        return b >= 54;
      case 37:
        return b >= 49;
      case 38:
        return b >= 44;
      case 39:
        return b >= 39;
      case 40:
        return b >= 34;
      case 41:
        return b >= 29;
      case 42:
        return b >= 23;
      case 43:
        return b >= 18;
      case 44:
        return b >= 13;
      case 45:
        return b >= 8;
      case 46:
        return b >= 3;
      default:
        return g >= 47;
    }
  }

  private static bool CheckR113(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 237;
      case 1:
        return b >= 232;
      case 2:
        return b >= 227;
      case 3:
        return b >= 222;
      case 4:
        return b >= 217;
      case 5:
        return b >= 211;
      case 6:
        return b >= 206;
      case 7:
        return b >= 201;
      case 8:
        return b >= 196;
      case 9:
        return b >= 191;
      case 10:
        return b >= 186;
      case 11:
        return b >= 180;
      case 12:
        return b >= 175;
      case 13:
        return b >= 170;
      case 14:
        return b >= 165;
      case 15:
        return b >= 160 /*0xA0*/;
      case 16 /*0x10*/:
        return b >= 155;
      case 17:
        return b >= 150;
      case 18:
        return b >= 144 /*0x90*/;
      case 19:
        return b >= 139;
      case 20:
        return b >= 134;
      case 21:
        return b >= 129;
      case 22:
        return b >= 124;
      case 23:
        return b >= 119;
      case 24:
        return b >= 114;
      case 25:
        return b >= 108;
      case 26:
        return b >= 103;
      case 27:
        return b >= 98;
      case 28:
        return b >= 93;
      case 29:
        return b >= 88;
      case 30:
        return b >= 83;
      case 31 /*0x1F*/:
        return b >= 77;
      case 32 /*0x20*/:
        return b >= 72;
      case 33:
        return b >= 67;
      case 34:
        return b >= 62;
      case 35:
        return b >= 57;
      case 36:
        return b >= 52;
      case 37:
        return b >= 47;
      case 38:
        return b >= 41;
      case 39:
        return b >= 36;
      case 40:
        return b >= 31 /*0x1F*/;
      case 41:
        return b >= 26;
      case 42:
        return b >= 21;
      case 43:
        return b >= 16 /*0x10*/;
      case 44:
        return b >= 11;
      case 45:
        return b >= 5;
      default:
        return g >= 46;
    }
  }

  private static bool CheckR114(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 234;
      case 1:
        return b >= 229;
      case 2:
        return b >= 224 /*0xE0*/;
      case 3:
        return b >= 219;
      case 4:
        return b >= 214;
      case 5:
        return b >= 209;
      case 6:
        return b >= 204;
      case 7:
        return b >= 198;
      case 8:
        return b >= 193;
      case 9:
        return b >= 188;
      case 10:
        return b >= 183;
      case 11:
        return b >= 178;
      case 12:
        return b >= 173;
      case 13:
        return b >= 168;
      case 14:
        return b >= 162;
      case 15:
        return b >= 157;
      case 16 /*0x10*/:
        return b >= 152;
      case 17:
        return b >= 147;
      case 18:
        return b >= 142;
      case 19:
        return b >= 137;
      case 20:
        return b >= 132;
      case 21:
        return b >= 126;
      case 22:
        return b >= 121;
      case 23:
        return b >= 116;
      case 24:
        return b >= 111;
      case 25:
        return b >= 106;
      case 26:
        return b >= 101;
      case 27:
        return b >= 95;
      case 28:
        return b >= 90;
      case 29:
        return b >= 85;
      case 30:
        return b >= 80 /*0x50*/;
      case 31 /*0x1F*/:
        return b >= 75;
      case 32 /*0x20*/:
        return b >= 70;
      case 33:
        return b >= 65;
      case 34:
        return b >= 59;
      case 35:
        return b >= 54;
      case 36:
        return b >= 49;
      case 37:
        return b >= 44;
      case 38:
        return b >= 39;
      case 39:
        return b >= 34;
      case 40:
        return b >= 29;
      case 41:
        return b >= 23;
      case 42:
        return b >= 18;
      case 43:
        return b >= 13;
      case 44:
        return b >= 8;
      case 45:
        return b >= 3;
      default:
        return g >= 46;
    }
  }

  private static bool CheckR115(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 232;
      case 1:
        return b >= 227;
      case 2:
        return b >= 222;
      case 3:
        return b >= 216;
      case 4:
        return b >= 211;
      case 5:
        return b >= 206;
      case 6:
        return b >= 201;
      case 7:
        return b >= 196;
      case 8:
        return b >= 191;
      case 9:
        return b >= 186;
      case 10:
        return b >= 180;
      case 11:
        return b >= 175;
      case 12:
        return b >= 170;
      case 13:
        return b >= 165;
      case 14:
        return b >= 160 /*0xA0*/;
      case 15:
        return b >= 155;
      case 16 /*0x10*/:
        return b >= 149;
      case 17:
        return b >= 144 /*0x90*/;
      case 18:
        return b >= 139;
      case 19:
        return b >= 134;
      case 20:
        return b >= 129;
      case 21:
        return b >= 124;
      case 22:
        return b >= 119;
      case 23:
        return b >= 113;
      case 24:
        return b >= 108;
      case 25:
        return b >= 103;
      case 26:
        return b >= 98;
      case 27:
        return b >= 93;
      case 28:
        return b >= 88;
      case 29:
        return b >= 83;
      case 30:
        return b >= 77;
      case 31 /*0x1F*/:
        return b >= 72;
      case 32 /*0x20*/:
        return b >= 67;
      case 33:
        return b >= 62;
      case 34:
        return b >= 57;
      case 35:
        return b >= 52;
      case 36:
        return b >= 47;
      case 37:
        return b >= 41;
      case 38:
        return b >= 36;
      case 39:
        return b >= 31 /*0x1F*/;
      case 40:
        return b >= 26;
      case 41:
        return b >= 21;
      case 42:
        return b >= 16 /*0x10*/;
      case 43:
        return b >= 10;
      case 44:
        return b >= 5;
      default:
        return g >= 45;
    }
  }

  private static bool CheckR116(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 229;
      case 1:
        return b >= 224 /*0xE0*/;
      case 2:
        return b >= 219;
      case 3:
        return b >= 214;
      case 4:
        return b >= 209;
      case 5:
        return b >= 204;
      case 6:
        return b >= 198;
      case 7:
        return b >= 193;
      case 8:
        return b >= 188;
      case 9:
        return b >= 183;
      case 10:
        return b >= 178;
      case 11:
        return b >= 173;
      case 12:
        return b >= 167;
      case 13:
        return b >= 162;
      case 14:
        return b >= 157;
      case 15:
        return b >= 152;
      case 16 /*0x10*/:
        return b >= 147;
      case 17:
        return b >= 142;
      case 18:
        return b >= 137;
      case 19:
        return b >= 131;
      case 20:
        return b >= 126;
      case 21:
        return b >= 121;
      case 22:
        return b >= 116;
      case 23:
        return b >= 111;
      case 24:
        return b >= 106;
      case 25:
        return b >= 101;
      case 26:
        return b >= 95;
      case 27:
        return b >= 90;
      case 28:
        return b >= 85;
      case 29:
        return b >= 80 /*0x50*/;
      case 30:
        return b >= 75;
      case 31 /*0x1F*/:
        return b >= 70;
      case 32 /*0x20*/:
        return b >= 64 /*0x40*/;
      case 33:
        return b >= 59;
      case 34:
        return b >= 54;
      case 35:
        return b >= 49;
      case 36:
        return b >= 44;
      case 37:
        return b >= 39;
      case 38:
        return b >= 34;
      case 39:
        return b >= 28;
      case 40:
        return b >= 23;
      case 41:
        return b >= 18;
      case 42:
        return b >= 13;
      case 43:
        return b >= 8;
      case 44:
        return b >= 3;
      default:
        return g >= 45;
    }
  }

  private static bool CheckR117(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 227;
      case 1:
        return b >= 221;
      case 2:
        return b >= 216;
      case 3:
        return b >= 211;
      case 4:
        return b >= 206;
      case 5:
        return b >= 201;
      case 6:
        return b >= 196;
      case 7:
        return b >= 191;
      case 8:
        return b >= 185;
      case 9:
        return b >= 180;
      case 10:
        return b >= 175;
      case 11:
        return b >= 170;
      case 12:
        return b >= 165;
      case 13:
        return b >= 160 /*0xA0*/;
      case 14:
        return b >= 155;
      case 15:
        return b >= 149;
      case 16 /*0x10*/:
        return b >= 144 /*0x90*/;
      case 17:
        return b >= 139;
      case 18:
        return b >= 134;
      case 19:
        return b >= 129;
      case 20:
        return b >= 124;
      case 21:
        return b >= 118;
      case 22:
        return b >= 113;
      case 23:
        return b >= 108;
      case 24:
        return b >= 103;
      case 25:
        return b >= 98;
      case 26:
        return b >= 93;
      case 27:
        return b >= 88;
      case 28:
        return b >= 82;
      case 29:
        return b >= 77;
      case 30:
        return b >= 72;
      case 31 /*0x1F*/:
        return b >= 67;
      case 32 /*0x20*/:
        return b >= 62;
      case 33:
        return b >= 57;
      case 34:
        return b >= 52;
      case 35:
        return b >= 46;
      case 36:
        return b >= 41;
      case 37:
        return b >= 36;
      case 38:
        return b >= 31 /*0x1F*/;
      case 39:
        return b >= 26;
      case 40:
        return b >= 21;
      case 41:
        return b >= 16 /*0x10*/;
      case 42:
        return b >= 10;
      case 43:
        return b >= 5;
      default:
        return g >= 44;
    }
  }

  private static bool CheckR118(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 224 /*0xE0*/;
      case 1:
        return b >= 219;
      case 2:
        return b >= 214;
      case 3:
        return b >= 209;
      case 4:
        return b >= 203;
      case 5:
        return b >= 198;
      case 6:
        return b >= 193;
      case 7:
        return b >= 188;
      case 8:
        return b >= 183;
      case 9:
        return b >= 178;
      case 10:
        return b >= 173;
      case 11:
        return b >= 167;
      case 12:
        return b >= 162;
      case 13:
        return b >= 157;
      case 14:
        return b >= 152;
      case 15:
        return b >= 147;
      case 16 /*0x10*/:
        return b >= 142;
      case 17:
        return b >= 136;
      case 18:
        return b >= 131;
      case 19:
        return b >= 126;
      case 20:
        return b >= 121;
      case 21:
        return b >= 116;
      case 22:
        return b >= 111;
      case 23:
        return b >= 106;
      case 24:
        return b >= 100;
      case 25:
        return b >= 95;
      case 26:
        return b >= 90;
      case 27:
        return b >= 85;
      case 28:
        return b >= 80 /*0x50*/;
      case 29:
        return b >= 75;
      case 30:
        return b >= 70;
      case 31 /*0x1F*/:
        return b >= 64 /*0x40*/;
      case 32 /*0x20*/:
        return b >= 59;
      case 33:
        return b >= 54;
      case 34:
        return b >= 49;
      case 35:
        return b >= 44;
      case 36:
        return b >= 39;
      case 37:
        return b >= 33;
      case 38:
        return b >= 28;
      case 39:
        return b >= 23;
      case 40:
        return b >= 18;
      case 41:
        return b >= 13;
      case 42:
        return b >= 8;
      case 43:
        return b >= 3;
      default:
        return g >= 44;
    }
  }

  private static bool CheckR119(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 221;
      case 1:
        return b >= 216;
      case 2:
        return b >= 211;
      case 3:
        return b >= 206;
      case 4:
        return b >= 201;
      case 5:
        return b >= 196;
      case 6:
        return b >= 190;
      case 7:
        return b >= 185;
      case 8:
        return b >= 180;
      case 9:
        return b >= 175;
      case 10:
        return b >= 170;
      case 11:
        return b >= 165;
      case 12:
        return b >= 160 /*0xA0*/;
      case 13:
        return b >= 154;
      case 14:
        return b >= 149;
      case 15:
        return b >= 144 /*0x90*/;
      case 16 /*0x10*/:
        return b >= 139;
      case 17:
        return b >= 134;
      case 18:
        return b >= 129;
      case 19:
        return b >= 124;
      case 20:
        return b >= 118;
      case 21:
        return b >= 113;
      case 22:
        return b >= 108;
      case 23:
        return b >= 103;
      case 24:
        return b >= 98;
      case 25:
        return b >= 93;
      case 26:
        return b >= 88;
      case 27:
        return b >= 82;
      case 28:
        return b >= 77;
      case 29:
        return b >= 72;
      case 30:
        return b >= 67;
      case 31 /*0x1F*/:
        return b >= 62;
      case 32 /*0x20*/:
        return b >= 57;
      case 33:
        return b >= 51;
      case 34:
        return b >= 46;
      case 35:
        return b >= 41;
      case 36:
        return b >= 36;
      case 37:
        return b >= 31 /*0x1F*/;
      case 38:
        return b >= 26;
      case 39:
        return b >= 21;
      case 40:
        return b >= 15;
      case 41:
        return b >= 10;
      case 42:
        return b >= 5;
      default:
        return g >= 43;
    }
  }

  private static bool CheckR120(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 219;
      case 1:
        return b >= 214;
      case 2:
        return b >= 208 /*0xD0*/;
      case 3:
        return b >= 203;
      case 4:
        return b >= 198;
      case 5:
        return b >= 193;
      case 6:
        return b >= 188;
      case 7:
        return b >= 183;
      case 8:
        return b >= 178;
      case 9:
        return b >= 172;
      case 10:
        return b >= 167;
      case 11:
        return b >= 162;
      case 12:
        return b >= 157;
      case 13:
        return b >= 152;
      case 14:
        return b >= 147;
      case 15:
        return b >= 142;
      case 16 /*0x10*/:
        return b >= 136;
      case 17:
        return b >= 131;
      case 18:
        return b >= 126;
      case 19:
        return b >= 121;
      case 20:
        return b >= 116;
      case 21:
        return b >= 111;
      case 22:
        return b >= 105;
      case 23:
        return b >= 100;
      case 24:
        return b >= 95;
      case 25:
        return b >= 90;
      case 26:
        return b >= 85;
      case 27:
        return b >= 80 /*0x50*/;
      case 28:
        return b >= 75;
      case 29:
        return b >= 69;
      case 30:
        return b >= 64 /*0x40*/;
      case 31 /*0x1F*/:
        return b >= 59;
      case 32 /*0x20*/:
        return b >= 54;
      case 33:
        return b >= 49;
      case 34:
        return b >= 44;
      case 35:
        return b >= 39;
      case 36:
        return b >= 33;
      case 37:
        return b >= 28;
      case 38:
        return b >= 23;
      case 39:
        return b >= 18;
      case 40:
        return b >= 13;
      case 41:
        return b >= 8;
      case 42:
        return b >= 2;
      default:
        return g >= 43;
    }
  }

  private static bool CheckR121(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 216;
      case 1:
        return b >= 211;
      case 2:
        return b >= 206;
      case 3:
        return b >= 201;
      case 4:
        return b >= 196;
      case 5:
        return b >= 190;
      case 6:
        return b >= 185;
      case 7:
        return b >= 180;
      case 8:
        return b >= 175;
      case 9:
        return b >= 170;
      case 10:
        return b >= 165;
      case 11:
        return b >= 159;
      case 12:
        return b >= 154;
      case 13:
        return b >= 149;
      case 14:
        return b >= 144 /*0x90*/;
      case 15:
        return b >= 139;
      case 16 /*0x10*/:
        return b >= 134;
      case 17:
        return b >= 129;
      case 18:
        return b >= 123;
      case 19:
        return b >= 118;
      case 20:
        return b >= 113;
      case 21:
        return b >= 108;
      case 22:
        return b >= 103;
      case 23:
        return b >= 98;
      case 24:
        return b >= 93;
      case 25:
        return b >= 87;
      case 26:
        return b >= 82;
      case 27:
        return b >= 77;
      case 28:
        return b >= 72;
      case 29:
        return b >= 67;
      case 30:
        return b >= 62;
      case 31 /*0x1F*/:
        return b >= 57;
      case 32 /*0x20*/:
        return b >= 51;
      case 33:
        return b >= 46;
      case 34:
        return b >= 41;
      case 35:
        return b >= 36;
      case 36:
        return b >= 31 /*0x1F*/;
      case 37:
        return b >= 26;
      case 38:
        return b >= 20;
      case 39:
        return b >= 15;
      case 40:
        return b >= 10;
      case 41:
        return b >= 5;
      default:
        return g >= 42;
    }
  }

  private static bool CheckR122(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 214;
      case 1:
        return b >= 208 /*0xD0*/;
      case 2:
        return b >= 203;
      case 3:
        return b >= 198;
      case 4:
        return b >= 193;
      case 5:
        return b >= 188;
      case 6:
        return b >= 183;
      case 7:
        return b >= 177;
      case 8:
        return b >= 172;
      case 9:
        return b >= 167;
      case 10:
        return b >= 162;
      case 11:
        return b >= 157;
      case 12:
        return b >= 152;
      case 13:
        return b >= 147;
      case 14:
        return b >= 141;
      case 15:
        return b >= 136;
      case 16 /*0x10*/:
        return b >= 131;
      case 17:
        return b >= 126;
      case 18:
        return b >= 121;
      case 19:
        return b >= 116;
      case 20:
        return b >= 111;
      case 21:
        return b >= 105;
      case 22:
        return b >= 100;
      case 23:
        return b >= 95;
      case 24:
        return b >= 90;
      case 25:
        return b >= 85;
      case 26:
        return b >= 80 /*0x50*/;
      case 27:
        return b >= 74;
      case 28:
        return b >= 69;
      case 29:
        return b >= 64 /*0x40*/;
      case 30:
        return b >= 59;
      case 31 /*0x1F*/:
        return b >= 54;
      case 32 /*0x20*/:
        return b >= 49;
      case 33:
        return b >= 44;
      case 34:
        return b >= 38;
      case 35:
        return b >= 33;
      case 36:
        return b >= 28;
      case 37:
        return b >= 23;
      case 38:
        return b >= 18;
      case 39:
        return b >= 13;
      case 40:
        return b >= 8;
      case 41:
        return b >= 2;
      default:
        return g >= 42;
    }
  }

  private static bool CheckR123(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 211;
      case 1:
        return b >= 206;
      case 2:
        return b >= 201;
      case 3:
        return b >= 195;
      case 4:
        return b >= 190;
      case 5:
        return b >= 185;
      case 6:
        return b >= 180;
      case 7:
        return b >= 175;
      case 8:
        return b >= 170;
      case 9:
        return b >= 165;
      case 10:
        return b >= 159;
      case 11:
        return b >= 154;
      case 12:
        return b >= 149;
      case 13:
        return b >= 144 /*0x90*/;
      case 14:
        return b >= 139;
      case 15:
        return b >= 134;
      case 16 /*0x10*/:
        return b >= 129;
      case 17:
        return b >= 123;
      case 18:
        return b >= 118;
      case 19:
        return b >= 113;
      case 20:
        return b >= 108;
      case 21:
        return b >= 103;
      case 22:
        return b >= 98;
      case 23:
        return b >= 92;
      case 24:
        return b >= 87;
      case 25:
        return b >= 82;
      case 26:
        return b >= 77;
      case 27:
        return b >= 72;
      case 28:
        return b >= 67;
      case 29:
        return b >= 62;
      case 30:
        return b >= 56;
      case 31 /*0x1F*/:
        return b >= 51;
      case 32 /*0x20*/:
        return b >= 46;
      case 33:
        return b >= 41;
      case 34:
        return b >= 36;
      case 35:
        return b >= 31 /*0x1F*/;
      case 36:
        return b >= 26;
      case 37:
        return b >= 20;
      case 38:
        return b >= 15;
      case 39:
        return b >= 10;
      case 40:
        return b >= 5;
      default:
        return g >= 41;
    }
  }

  private static bool CheckR124(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 208 /*0xD0*/;
      case 1:
        return b >= 203;
      case 2:
        return b >= 198;
      case 3:
        return b >= 193;
      case 4:
        return b >= 188;
      case 5:
        return b >= 183;
      case 6:
        return b >= 177;
      case 7:
        return b >= 172;
      case 8:
        return b >= 167;
      case 9:
        return b >= 162;
      case 10:
        return b >= 157;
      case 11:
        return b >= 152;
      case 12:
        return b >= 146;
      case 13:
        return b >= 141;
      case 14:
        return b >= 136;
      case 15:
        return b >= 131;
      case 16 /*0x10*/:
        return b >= 126;
      case 17:
        return b >= 121;
      case 18:
        return b >= 116;
      case 19:
        return b >= 110;
      case 20:
        return b >= 105;
      case 21:
        return b >= 100;
      case 22:
        return b >= 95;
      case 23:
        return b >= 90;
      case 24:
        return b >= 85;
      case 25:
        return b >= 80 /*0x50*/;
      case 26:
        return b >= 74;
      case 27:
        return b >= 69;
      case 28:
        return b >= 64 /*0x40*/;
      case 29:
        return b >= 59;
      case 30:
        return b >= 54;
      case 31 /*0x1F*/:
        return b >= 49;
      case 32 /*0x20*/:
        return b >= 43;
      case 33:
        return b >= 38;
      case 34:
        return b >= 33;
      case 35:
        return b >= 28;
      case 36:
        return b >= 23;
      case 37:
        return b >= 18;
      case 38:
        return b >= 13;
      case 39:
        return b >= 7;
      case 40:
        return b >= 2;
      default:
        return g >= 41;
    }
  }

  private static bool CheckR125(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 206;
      case 1:
        return b >= 200;
      case 2:
        return b >= 195;
      case 3:
        return b >= 190;
      case 4:
        return b >= 185;
      case 5:
        return b >= 180;
      case 6:
        return b >= 175;
      case 7:
        return b >= 170;
      case 8:
        return b >= 164;
      case 9:
        return b >= 159;
      case 10:
        return b >= 154;
      case 11:
        return b >= 149;
      case 12:
        return b >= 144 /*0x90*/;
      case 13:
        return b >= 139;
      case 14:
        return b >= 134;
      case 15:
        return b >= 128 /*0x80*/;
      case 16 /*0x10*/:
        return b >= 123;
      case 17:
        return b >= 118;
      case 18:
        return b >= 113;
      case 19:
        return b >= 108;
      case 20:
        return b >= 103;
      case 21:
        return b >= 98;
      case 22:
        return b >= 92;
      case 23:
        return b >= 87;
      case 24:
        return b >= 82;
      case 25:
        return b >= 77;
      case 26:
        return b >= 72;
      case 27:
        return b >= 67;
      case 28:
        return b >= 61;
      case 29:
        return b >= 56;
      case 30:
        return b >= 51;
      case 31 /*0x1F*/:
        return b >= 46;
      case 32 /*0x20*/:
        return b >= 41;
      case 33:
        return b >= 36;
      case 34:
        return b >= 31 /*0x1F*/;
      case 35:
        return b >= 25;
      case 36:
        return b >= 20;
      case 37:
        return b >= 15;
      case 38:
        return b >= 10;
      case 39:
        return b >= 5;
      default:
        return g >= 40;
    }
  }

  private static bool CheckR126(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 203;
      case 1:
        return b >= 198;
      case 2:
        return b >= 193;
      case 3:
        return b >= 188;
      case 4:
        return b >= 182;
      case 5:
        return b >= 177;
      case 6:
        return b >= 172;
      case 7:
        return b >= 167;
      case 8:
        return b >= 162;
      case 9:
        return b >= 157;
      case 10:
        return b >= 152;
      case 11:
        return b >= 146;
      case 12:
        return b >= 141;
      case 13:
        return b >= 136;
      case 14:
        return b >= 131;
      case 15:
        return b >= 126;
      case 16 /*0x10*/:
        return b >= 121;
      case 17:
        return b >= 115;
      case 18:
        return b >= 110;
      case 19:
        return b >= 105;
      case 20:
        return b >= 100;
      case 21:
        return b >= 95;
      case 22:
        return b >= 90;
      case 23:
        return b >= 85;
      case 24:
        return b >= 79;
      case 25:
        return b >= 74;
      case 26:
        return b >= 69;
      case 27:
        return b >= 64 /*0x40*/;
      case 28:
        return b >= 59;
      case 29:
        return b >= 54;
      case 30:
        return b >= 49;
      case 31 /*0x1F*/:
        return b >= 43;
      case 32 /*0x20*/:
        return b >= 38;
      case 33:
        return b >= 33;
      case 34:
        return b >= 28;
      case 35:
        return b >= 23;
      case 36:
        return b >= 18;
      case 37:
        return b >= 13;
      case 38:
        return b >= 7;
      case 39:
        return b >= 2;
      default:
        return g >= 40;
    }
  }

  private static bool CheckR127(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 200;
      case 1:
        return b >= 195;
      case 2:
        return b >= 190;
      case 3:
        return b >= 185;
      case 4:
        return b >= 180;
      case 5:
        return b >= 175;
      case 6:
        return b >= 170;
      case 7:
        return b >= 164;
      case 8:
        return b >= 159;
      case 9:
        return b >= 154;
      case 10:
        return b >= 149;
      case 11:
        return b >= 144 /*0x90*/;
      case 12:
        return b >= 139;
      case 13:
        return b >= 133;
      case 14:
        return b >= 128 /*0x80*/;
      case 15:
        return b >= 123;
      case 16 /*0x10*/:
        return b >= 118;
      case 17:
        return b >= 113;
      case 18:
        return b >= 108;
      case 19:
        return b >= 103;
      case 20:
        return b >= 97;
      case 21:
        return b >= 92;
      case 22:
        return b >= 87;
      case 23:
        return b >= 82;
      case 24:
        return b >= 77;
      case 25:
        return b >= 72;
      case 26:
        return b >= 67;
      case 27:
        return b >= 61;
      case 28:
        return b >= 56;
      case 29:
        return b >= 51;
      case 30:
        return b >= 46;
      case 31 /*0x1F*/:
        return b >= 41;
      case 32 /*0x20*/:
        return b >= 36;
      case 33:
        return b >= 30;
      case 34:
        return b >= 25;
      case 35:
        return b >= 20;
      case 36:
        return b >= 15;
      case 37:
        return b >= 10;
      case 38:
        return b >= 5;
      default:
        return g >= 39;
    }
  }

  private static bool CheckR128(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 198;
      case 1:
        return b >= 193;
      case 2:
        return b >= 187;
      case 3:
        return b >= 182;
      case 4:
        return b >= 177;
      case 5:
        return b >= 172;
      case 6:
        return b >= 167;
      case 7:
        return b >= 162;
      case 8:
        return b >= 157;
      case 9:
        return b >= 151;
      case 10:
        return b >= 146;
      case 11:
        return b >= 141;
      case 12:
        return b >= 136;
      case 13:
        return b >= 131;
      case 14:
        return b >= 126;
      case 15:
        return b >= 121;
      case 16 /*0x10*/:
        return b >= 115;
      case 17:
        return b >= 110;
      case 18:
        return b >= 105;
      case 19:
        return b >= 100;
      case 20:
        return b >= 95;
      case 21:
        return b >= 90;
      case 22:
        return b >= 84;
      case 23:
        return b >= 79;
      case 24:
        return b >= 74;
      case 25:
        return b >= 69;
      case 26:
        return b >= 64 /*0x40*/;
      case 27:
        return b >= 59;
      case 28:
        return b >= 54;
      case 29:
        return b >= 48 /*0x30*/;
      case 30:
        return b >= 43;
      case 31 /*0x1F*/:
        return b >= 38;
      case 32 /*0x20*/:
        return b >= 33;
      case 33:
        return b >= 28;
      case 34:
        return b >= 23;
      case 35:
        return b >= 18;
      case 36:
        return b >= 12;
      case 37:
        return b >= 7;
      case 38:
        return b >= 2;
      default:
        return g >= 39;
    }
  }

  private static bool CheckR129(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 195;
      case 1:
        return b >= 190;
      case 2:
        return b >= 185;
      case 3:
        return b >= 180;
      case 4:
        return b >= 175;
      case 5:
        return b >= 169;
      case 6:
        return b >= 164;
      case 7:
        return b >= 159;
      case 8:
        return b >= 154;
      case 9:
        return b >= 149;
      case 10:
        return b >= 144 /*0x90*/;
      case 11:
        return b >= 139;
      case 12:
        return b >= 133;
      case 13:
        return b >= 128 /*0x80*/;
      case 14:
        return b >= 123;
      case 15:
        return b >= 118;
      case 16 /*0x10*/:
        return b >= 113;
      case 17:
        return b >= 108;
      case 18:
        return b >= 102;
      case 19:
        return b >= 97;
      case 20:
        return b >= 92;
      case 21:
        return b >= 87;
      case 22:
        return b >= 82;
      case 23:
        return b >= 77;
      case 24:
        return b >= 72;
      case 25:
        return b >= 66;
      case 26:
        return b >= 61;
      case 27:
        return b >= 56;
      case 28:
        return b >= 51;
      case 29:
        return b >= 46;
      case 30:
        return b >= 41;
      case 31 /*0x1F*/:
        return b >= 36;
      case 32 /*0x20*/:
        return b >= 30;
      case 33:
        return b >= 25;
      case 34:
        return b >= 20;
      case 35:
        return b >= 15;
      case 36:
        return b >= 10;
      case 37:
        return b >= 5;
      default:
        return g >= 38;
    }
  }

  private static bool CheckR130(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 193;
      case 1:
        return b >= 187;
      case 2:
        return b >= 182;
      case 3:
        return b >= 177;
      case 4:
        return b >= 172;
      case 5:
        return b >= 167;
      case 6:
        return b >= 162;
      case 7:
        return b >= 156;
      case 8:
        return b >= 151;
      case 9:
        return b >= 146;
      case 10:
        return b >= 141;
      case 11:
        return b >= 136;
      case 12:
        return b >= 131;
      case 13:
        return b >= 126;
      case 14:
        return b >= 120;
      case 15:
        return b >= 115;
      case 16 /*0x10*/:
        return b >= 110;
      case 17:
        return b >= 105;
      case 18:
        return b >= 100;
      case 19:
        return b >= 95;
      case 20:
        return b >= 90;
      case 21:
        return b >= 84;
      case 22:
        return b >= 79;
      case 23:
        return b >= 74;
      case 24:
        return b >= 69;
      case 25:
        return b >= 64 /*0x40*/;
      case 26:
        return b >= 59;
      case 27:
        return b >= 54;
      case 28:
        return b >= 48 /*0x30*/;
      case 29:
        return b >= 43;
      case 30:
        return b >= 38;
      case 31 /*0x1F*/:
        return b >= 33;
      case 32 /*0x20*/:
        return b >= 28;
      case 33:
        return b >= 23;
      case 34:
        return b >= 17;
      case 35:
        return b >= 12;
      case 36:
        return b >= 7;
      case 37:
        return b >= 2;
      default:
        return g >= 38;
    }
  }

  private static bool CheckR131(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 190;
      case 1:
        return b >= 185;
      case 2:
        return b >= 180;
      case 3:
        return b >= 174;
      case 4:
        return b >= 169;
      case 5:
        return b >= 164;
      case 6:
        return b >= 159;
      case 7:
        return b >= 154;
      case 8:
        return b >= 149;
      case 9:
        return b >= 144 /*0x90*/;
      case 10:
        return b >= 138;
      case 11:
        return b >= 133;
      case 12:
        return b >= 128 /*0x80*/;
      case 13:
        return b >= 123;
      case 14:
        return b >= 118;
      case 15:
        return b >= 113;
      case 16 /*0x10*/:
        return b >= 108;
      case 17:
        return b >= 102;
      case 18:
        return b >= 97;
      case 19:
        return b >= 92;
      case 20:
        return b >= 87;
      case 21:
        return b >= 82;
      case 22:
        return b >= 77;
      case 23:
        return b >= 71;
      case 24:
        return b >= 66;
      case 25:
        return b >= 61;
      case 26:
        return b >= 56;
      case 27:
        return b >= 51;
      case 28:
        return b >= 46;
      case 29:
        return b >= 41;
      case 30:
        return b >= 35;
      case 31 /*0x1F*/:
        return b >= 30;
      case 32 /*0x20*/:
        return b >= 25;
      case 33:
        return b >= 20;
      case 34:
        return b >= 15;
      case 35:
        return b >= 10;
      case 36:
        return b >= 5;
      default:
        return g >= 37;
    }
  }

  private static bool CheckR132(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 187;
      case 1:
        return b >= 182;
      case 2:
        return b >= 177;
      case 3:
        return b >= 172;
      case 4:
        return b >= 167;
      case 5:
        return b >= 162;
      case 6:
        return b >= 156;
      case 7:
        return b >= 151;
      case 8:
        return b >= 146;
      case 9:
        return b >= 141;
      case 10:
        return b >= 136;
      case 11:
        return b >= 131;
      case 12:
        return b >= 125;
      case 13:
        return b >= 120;
      case 14:
        return b >= 115;
      case 15:
        return b >= 110;
      case 16 /*0x10*/:
        return b >= 105;
      case 17:
        return b >= 100;
      case 18:
        return b >= 95;
      case 19:
        return b >= 89;
      case 20:
        return b >= 84;
      case 21:
        return b >= 79;
      case 22:
        return b >= 74;
      case 23:
        return b >= 69;
      case 24:
        return b >= 64 /*0x40*/;
      case 25:
        return b >= 59;
      case 26:
        return b >= 53;
      case 27:
        return b >= 48 /*0x30*/;
      case 28:
        return b >= 43;
      case 29:
        return b >= 38;
      case 30:
        return b >= 33;
      case 31 /*0x1F*/:
        return b >= 28;
      case 32 /*0x20*/:
        return b >= 23;
      case 33:
        return b >= 17;
      case 34:
        return b >= 12;
      case 35:
        return b >= 7;
      case 36:
        return b >= 2;
      default:
        return g >= 37;
    }
  }

  private static bool CheckR133(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 185;
      case 1:
        return b >= 180;
      case 2:
        return b >= 174;
      case 3:
        return b >= 169;
      case 4:
        return b >= 164;
      case 5:
        return b >= 159;
      case 6:
        return b >= 154;
      case 7:
        return b >= 149;
      case 8:
        return b >= 143;
      case 9:
        return b >= 138;
      case 10:
        return b >= 133;
      case 11:
        return b >= 128 /*0x80*/;
      case 12:
        return b >= 123;
      case 13:
        return b >= 118;
      case 14:
        return b >= 113;
      case 15:
        return b >= 107;
      case 16 /*0x10*/:
        return b >= 102;
      case 17:
        return b >= 97;
      case 18:
        return b >= 92;
      case 19:
        return b >= 87;
      case 20:
        return b >= 82;
      case 21:
        return b >= 77;
      case 22:
        return b >= 71;
      case 23:
        return b >= 66;
      case 24:
        return b >= 61;
      case 25:
        return b >= 56;
      case 26:
        return b >= 51;
      case 27:
        return b >= 46;
      case 28:
        return b >= 40;
      case 29:
        return b >= 35;
      case 30:
        return b >= 30;
      case 31 /*0x1F*/:
        return b >= 25;
      case 32 /*0x20*/:
        return b >= 20;
      case 33:
        return b >= 15;
      case 34:
        return b >= 10;
      case 35:
        return b >= 4;
      default:
        return g >= 36;
    }
  }

  private static bool CheckR134(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 182;
      case 1:
        return b >= 177;
      case 2:
        return b >= 172;
      case 3:
        return b >= 167;
      case 4:
        return b >= 161;
      case 5:
        return b >= 156;
      case 6:
        return b >= 151;
      case 7:
        return b >= 146;
      case 8:
        return b >= 141;
      case 9:
        return b >= 136;
      case 10:
        return b >= 131;
      case 11:
        return b >= 125;
      case 12:
        return b >= 120;
      case 13:
        return b >= 115;
      case 14:
        return b >= 110;
      case 15:
        return b >= 105;
      case 16 /*0x10*/:
        return b >= 100;
      case 17:
        return b >= 95;
      case 18:
        return b >= 89;
      case 19:
        return b >= 84;
      case 20:
        return b >= 79;
      case 21:
        return b >= 74;
      case 22:
        return b >= 69;
      case 23:
        return b >= 64 /*0x40*/;
      case 24:
        return b >= 58;
      case 25:
        return b >= 53;
      case 26:
        return b >= 48 /*0x30*/;
      case 27:
        return b >= 43;
      case 28:
        return b >= 38;
      case 29:
        return b >= 33;
      case 30:
        return b >= 28;
      case 31 /*0x1F*/:
        return b >= 22;
      case 32 /*0x20*/:
        return b >= 17;
      case 33:
        return b >= 12;
      case 34:
        return b >= 7;
      case 35:
        return b >= 2;
      default:
        return g >= 36;
    }
  }

  private static bool CheckR135(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 179;
      case 1:
        return b >= 174;
      case 2:
        return b >= 169;
      case 3:
        return b >= 164;
      case 4:
        return b >= 159;
      case 5:
        return b >= 154;
      case 6:
        return b >= 149;
      case 7:
        return b >= 143;
      case 8:
        return b >= 138;
      case 9:
        return b >= 133;
      case 10:
        return b >= 128 /*0x80*/;
      case 11:
        return b >= 123;
      case 12:
        return b >= 118;
      case 13:
        return b >= 112 /*0x70*/;
      case 14:
        return b >= 107;
      case 15:
        return b >= 102;
      case 16 /*0x10*/:
        return b >= 97;
      case 17:
        return b >= 92;
      case 18:
        return b >= 87;
      case 19:
        return b >= 82;
      case 20:
        return b >= 76;
      case 21:
        return b >= 71;
      case 22:
        return b >= 66;
      case 23:
        return b >= 61;
      case 24:
        return b >= 56;
      case 25:
        return b >= 51;
      case 26:
        return b >= 46;
      case 27:
        return b >= 40;
      case 28:
        return b >= 35;
      case 29:
        return b >= 30;
      case 30:
        return b >= 25;
      case 31 /*0x1F*/:
        return b >= 20;
      case 32 /*0x20*/:
        return b >= 15;
      case 33:
        return b >= 9;
      case 34:
        return b >= 4;
      default:
        return g >= 35;
    }
  }

  private static bool CheckR136(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 177;
      case 1:
        return b >= 172;
      case 2:
        return b >= 166;
      case 3:
        return b >= 161;
      case 4:
        return b >= 156;
      case 5:
        return b >= 151;
      case 6:
        return b >= 146;
      case 7:
        return b >= 141;
      case 8:
        return b >= 136;
      case 9:
        return b >= 130;
      case 10:
        return b >= 125;
      case 11:
        return b >= 120;
      case 12:
        return b >= 115;
      case 13:
        return b >= 110;
      case 14:
        return b >= 105;
      case 15:
        return b >= 100;
      case 16 /*0x10*/:
        return b >= 94;
      case 17:
        return b >= 89;
      case 18:
        return b >= 84;
      case 19:
        return b >= 79;
      case 20:
        return b >= 74;
      case 21:
        return b >= 69;
      case 22:
        return b >= 64 /*0x40*/;
      case 23:
        return b >= 58;
      case 24:
        return b >= 53;
      case 25:
        return b >= 48 /*0x30*/;
      case 26:
        return b >= 43;
      case 27:
        return b >= 38;
      case 28:
        return b >= 33;
      case 29:
        return b >= 27;
      case 30:
        return b >= 22;
      case 31 /*0x1F*/:
        return b >= 17;
      case 32 /*0x20*/:
        return b >= 12;
      case 33:
        return b >= 7;
      case 34:
        return b >= 2;
      default:
        return g >= 35;
    }
  }

  private static bool CheckR137(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 174;
      case 1:
        return b >= 169;
      case 2:
        return b >= 164;
      case 3:
        return b >= 159;
      case 4:
        return b >= 154;
      case 5:
        return b >= 148;
      case 6:
        return b >= 143;
      case 7:
        return b >= 138;
      case 8:
        return b >= 133;
      case 9:
        return b >= 128 /*0x80*/;
      case 10:
        return b >= 123;
      case 11:
        return b >= 118;
      case 12:
        return b >= 112 /*0x70*/;
      case 13:
        return b >= 107;
      case 14:
        return b >= 102;
      case 15:
        return b >= 97;
      case 16 /*0x10*/:
        return b >= 92;
      case 17:
        return b >= 87;
      case 18:
        return b >= 81;
      case 19:
        return b >= 76;
      case 20:
        return b >= 71;
      case 21:
        return b >= 66;
      case 22:
        return b >= 61;
      case 23:
        return b >= 56;
      case 24:
        return b >= 51;
      case 25:
        return b >= 45;
      case 26:
        return b >= 40;
      case 27:
        return b >= 35;
      case 28:
        return b >= 30;
      case 29:
        return b >= 25;
      case 30:
        return b >= 20;
      case 31 /*0x1F*/:
        return b >= 15;
      case 32 /*0x20*/:
        return b >= 9;
      case 33:
        return b >= 4;
      default:
        return g >= 34;
    }
  }

  private static bool CheckR138(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 172;
      case 1:
        return b >= 166;
      case 2:
        return b >= 161;
      case 3:
        return b >= 156;
      case 4:
        return b >= 151;
      case 5:
        return b >= 146;
      case 6:
        return b >= 141;
      case 7:
        return b >= 136;
      case 8:
        return b >= 130;
      case 9:
        return b >= 125;
      case 10:
        return b >= 120;
      case 11:
        return b >= 115;
      case 12:
        return b >= 110;
      case 13:
        return b >= 105;
      case 14:
        return b >= 99;
      case 15:
        return b >= 94;
      case 16 /*0x10*/:
        return b >= 89;
      case 17:
        return b >= 84;
      case 18:
        return b >= 79;
      case 19:
        return b >= 74;
      case 20:
        return b >= 69;
      case 21:
        return b >= 63 /*0x3F*/;
      case 22:
        return b >= 58;
      case 23:
        return b >= 53;
      case 24:
        return b >= 48 /*0x30*/;
      case 25:
        return b >= 43;
      case 26:
        return b >= 38;
      case 27:
        return b >= 33;
      case 28:
        return b >= 27;
      case 29:
        return b >= 22;
      case 30:
        return b >= 17;
      case 31 /*0x1F*/:
        return b >= 12;
      case 32 /*0x20*/:
        return b >= 7;
      case 33:
        return b >= 2;
      default:
        return g >= 34;
    }
  }

  private static bool CheckR139(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 169;
      case 1:
        return b >= 164;
      case 2:
        return b >= 159;
      case 3:
        return b >= 153;
      case 4:
        return b >= 148;
      case 5:
        return b >= 143;
      case 6:
        return b >= 138;
      case 7:
        return b >= 133;
      case 8:
        return b >= 128 /*0x80*/;
      case 9:
        return b >= 123;
      case 10:
        return b >= 117;
      case 11:
        return b >= 112 /*0x70*/;
      case 12:
        return b >= 107;
      case 13:
        return b >= 102;
      case 14:
        return b >= 97;
      case 15:
        return b >= 92;
      case 16 /*0x10*/:
        return b >= 87;
      case 17:
        return b >= 81;
      case 18:
        return b >= 76;
      case 19:
        return b >= 71;
      case 20:
        return b >= 66;
      case 21:
        return b >= 61;
      case 22:
        return b >= 56;
      case 23:
        return b >= 50;
      case 24:
        return b >= 45;
      case 25:
        return b >= 40;
      case 26:
        return b >= 35;
      case 27:
        return b >= 30;
      case 28:
        return b >= 25;
      case 29:
        return b >= 20;
      case 30:
        return b >= 14;
      case 31 /*0x1F*/:
        return b >= 9;
      case 32 /*0x20*/:
        return b >= 4;
      default:
        return g >= 33;
    }
  }

  private static bool CheckR140(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 166;
      case 1:
        return b >= 161;
      case 2:
        return b >= 156;
      case 3:
        return b >= 151;
      case 4:
        return b >= 146;
      case 5:
        return b >= 141;
      case 6:
        return b >= 135;
      case 7:
        return b >= 130;
      case 8:
        return b >= 125;
      case 9:
        return b >= 120;
      case 10:
        return b >= 115;
      case 11:
        return b >= 110;
      case 12:
        return b >= 105;
      case 13:
        return b >= 99;
      case 14:
        return b >= 94;
      case 15:
        return b >= 89;
      case 16 /*0x10*/:
        return b >= 84;
      case 17:
        return b >= 79;
      case 18:
        return b >= 74;
      case 19:
        return b >= 68;
      case 20:
        return b >= 63 /*0x3F*/;
      case 21:
        return b >= 58;
      case 22:
        return b >= 53;
      case 23:
        return b >= 48 /*0x30*/;
      case 24:
        return b >= 43;
      case 25:
        return b >= 38;
      case 26:
        return b >= 32 /*0x20*/;
      case 27:
        return b >= 27;
      case 28:
        return b >= 22;
      case 29:
        return b >= 17;
      case 30:
        return b >= 12;
      case 31 /*0x1F*/:
        return b >= 7;
      case 32 /*0x20*/:
        return b >= 2;
      default:
        return g >= 33;
    }
  }

  private static bool CheckR141(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 164;
      case 1:
        return b >= 159;
      case 2:
        return b >= 153;
      case 3:
        return b >= 148;
      case 4:
        return b >= 143;
      case 5:
        return b >= 138;
      case 6:
        return b >= 133;
      case 7:
        return b >= 128 /*0x80*/;
      case 8:
        return b >= 122;
      case 9:
        return b >= 117;
      case 10:
        return b >= 112 /*0x70*/;
      case 11:
        return b >= 107;
      case 12:
        return b >= 102;
      case 13:
        return b >= 97;
      case 14:
        return b >= 92;
      case 15:
        return b >= 86;
      case 16 /*0x10*/:
        return b >= 81;
      case 17:
        return b >= 76;
      case 18:
        return b >= 71;
      case 19:
        return b >= 66;
      case 20:
        return b >= 61;
      case 21:
        return b >= 56;
      case 22:
        return b >= 50;
      case 23:
        return b >= 45;
      case 24:
        return b >= 40;
      case 25:
        return b >= 35;
      case 26:
        return b >= 30;
      case 27:
        return b >= 25;
      case 28:
        return b >= 20;
      case 29:
        return b >= 14;
      case 30:
        return b >= 9;
      case 31 /*0x1F*/:
        return b >= 4;
      default:
        return g >= 32 /*0x20*/;
    }
  }

  private static bool CheckR142(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 161;
      case 1:
        return b >= 156;
      case 2:
        return b >= 151;
      case 3:
        return b >= 146;
      case 4:
        return b >= 140;
      case 5:
        return b >= 135;
      case 6:
        return b >= 130;
      case 7:
        return b >= 125;
      case 8:
        return b >= 120;
      case 9:
        return b >= 115;
      case 10:
        return b >= 110;
      case 11:
        return b >= 104;
      case 12:
        return b >= 99;
      case 13:
        return b >= 94;
      case 14:
        return b >= 89;
      case 15:
        return b >= 84;
      case 16 /*0x10*/:
        return b >= 79;
      case 17:
        return b >= 74;
      case 18:
        return b >= 68;
      case 19:
        return b >= 63 /*0x3F*/;
      case 20:
        return b >= 58;
      case 21:
        return b >= 53;
      case 22:
        return b >= 48 /*0x30*/;
      case 23:
        return b >= 43;
      case 24:
        return b >= 37;
      case 25:
        return b >= 32 /*0x20*/;
      case 26:
        return b >= 27;
      case 27:
        return b >= 22;
      case 28:
        return b >= 17;
      case 29:
        return b >= 12;
      case 30:
        return b >= 7;
      case 31 /*0x1F*/:
        return b >= 1;
      default:
        return g >= 32 /*0x20*/;
    }
  }

  private static bool CheckR143(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 158;
      case 1:
        return b >= 153;
      case 2:
        return b >= 148;
      case 3:
        return b >= 143;
      case 4:
        return b >= 138;
      case 5:
        return b >= 133;
      case 6:
        return b >= 128 /*0x80*/;
      case 7:
        return b >= 122;
      case 8:
        return b >= 117;
      case 9:
        return b >= 112 /*0x70*/;
      case 10:
        return b >= 107;
      case 11:
        return b >= 102;
      case 12:
        return b >= 97;
      case 13:
        return b >= 91;
      case 14:
        return b >= 86;
      case 15:
        return b >= 81;
      case 16 /*0x10*/:
        return b >= 76;
      case 17:
        return b >= 71;
      case 18:
        return b >= 66;
      case 19:
        return b >= 61;
      case 20:
        return b >= 55;
      case 21:
        return b >= 50;
      case 22:
        return b >= 45;
      case 23:
        return b >= 40;
      case 24:
        return b >= 35;
      case 25:
        return b >= 30;
      case 26:
        return b >= 25;
      case 27:
        return b >= 19;
      case 28:
        return b >= 14;
      case 29:
        return b >= 9;
      case 30:
        return b >= 4;
      default:
        return g >= 31 /*0x1F*/;
    }
  }

  private static bool CheckR144(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 156;
      case 1:
        return b >= 151;
      case 2:
        return b >= 146;
      case 3:
        return b >= 140;
      case 4:
        return b >= 135;
      case 5:
        return b >= 130;
      case 6:
        return b >= 125;
      case 7:
        return b >= 120;
      case 8:
        return b >= 115;
      case 9:
        return b >= 109;
      case 10:
        return b >= 104;
      case 11:
        return b >= 99;
      case 12:
        return b >= 94;
      case 13:
        return b >= 89;
      case 14:
        return b >= 84;
      case 15:
        return b >= 79;
      case 16 /*0x10*/:
        return b >= 73;
      case 17:
        return b >= 68;
      case 18:
        return b >= 63 /*0x3F*/;
      case 19:
        return b >= 58;
      case 20:
        return b >= 53;
      case 21:
        return b >= 48 /*0x30*/;
      case 22:
        return b >= 43;
      case 23:
        return b >= 37;
      case 24:
        return b >= 32 /*0x20*/;
      case 25:
        return b >= 27;
      case 26:
        return b >= 22;
      case 27:
        return b >= 17;
      case 28:
        return b >= 12;
      case 29:
        return b >= 6;
      case 30:
        return b >= 1;
      default:
        return g >= 31 /*0x1F*/;
    }
  }

  private static bool CheckR145(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 153;
      case 1:
        return b >= 148;
      case 2:
        return b >= 143;
      case 3:
        return b >= 138;
      case 4:
        return b >= 133;
      case 5:
        return b >= (int) sbyte.MaxValue;
      case 6:
        return b >= 122;
      case 7:
        return b >= 117;
      case 8:
        return b >= 112 /*0x70*/;
      case 9:
        return b >= 107;
      case 10:
        return b >= 102;
      case 11:
        return b >= 97;
      case 12:
        return b >= 91;
      case 13:
        return b >= 86;
      case 14:
        return b >= 81;
      case 15:
        return b >= 76;
      case 16 /*0x10*/:
        return b >= 71;
      case 17:
        return b >= 66;
      case 18:
        return b >= 61;
      case 19:
        return b >= 55;
      case 20:
        return b >= 50;
      case 21:
        return b >= 45;
      case 22:
        return b >= 40;
      case 23:
        return b >= 35;
      case 24:
        return b >= 30;
      case 25:
        return b >= 24;
      case 26:
        return b >= 19;
      case 27:
        return b >= 14;
      case 28:
        return b >= 9;
      case 29:
        return b >= 4;
      default:
        return g >= 30;
    }
  }

  private static bool CheckR146(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 151;
      case 1:
        return b >= 145;
      case 2:
        return b >= 140;
      case 3:
        return b >= 135;
      case 4:
        return b >= 130;
      case 5:
        return b >= 125;
      case 6:
        return b >= 120;
      case 7:
        return b >= 115;
      case 8:
        return b >= 109;
      case 9:
        return b >= 104;
      case 10:
        return b >= 99;
      case 11:
        return b >= 94;
      case 12:
        return b >= 89;
      case 13:
        return b >= 84;
      case 14:
        return b >= 78;
      case 15:
        return b >= 73;
      case 16 /*0x10*/:
        return b >= 68;
      case 17:
        return b >= 63 /*0x3F*/;
      case 18:
        return b >= 58;
      case 19:
        return b >= 53;
      case 20:
        return b >= 48 /*0x30*/;
      case 21:
        return b >= 42;
      case 22:
        return b >= 37;
      case 23:
        return b >= 32 /*0x20*/;
      case 24:
        return b >= 27;
      case 25:
        return b >= 22;
      case 26:
        return b >= 17;
      case 27:
        return b >= 12;
      case 28:
        return b >= 6;
      case 29:
        return b >= 1;
      default:
        return g >= 30;
    }
  }

  private static bool CheckR147(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 148;
      case 1:
        return b >= 143;
      case 2:
        return b >= 138;
      case 3:
        return b >= 132;
      case 4:
        return b >= (int) sbyte.MaxValue;
      case 5:
        return b >= 122;
      case 6:
        return b >= 117;
      case 7:
        return b >= 112 /*0x70*/;
      case 8:
        return b >= 107;
      case 9:
        return b >= 102;
      case 10:
        return b >= 96 /*0x60*/;
      case 11:
        return b >= 91;
      case 12:
        return b >= 86;
      case 13:
        return b >= 81;
      case 14:
        return b >= 76;
      case 15:
        return b >= 71;
      case 16 /*0x10*/:
        return b >= 66;
      case 17:
        return b >= 60;
      case 18:
        return b >= 55;
      case 19:
        return b >= 50;
      case 20:
        return b >= 45;
      case 21:
        return b >= 40;
      case 22:
        return b >= 35;
      case 23:
        return b >= 30;
      case 24:
        return b >= 24;
      case 25:
        return b >= 19;
      case 26:
        return b >= 14;
      case 27:
        return b >= 9;
      case 28:
        return b >= 4;
      default:
        return g >= 29;
    }
  }

  private static bool CheckR148(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 145;
      case 1:
        return b >= 140;
      case 2:
        return b >= 135;
      case 3:
        return b >= 130;
      case 4:
        return b >= 125;
      case 5:
        return b >= 120;
      case 6:
        return b >= 114;
      case 7:
        return b >= 109;
      case 8:
        return b >= 104;
      case 9:
        return b >= 99;
      case 10:
        return b >= 94;
      case 11:
        return b >= 89;
      case 12:
        return b >= 84;
      case 13:
        return b >= 78;
      case 14:
        return b >= 73;
      case 15:
        return b >= 68;
      case 16 /*0x10*/:
        return b >= 63 /*0x3F*/;
      case 17:
        return b >= 58;
      case 18:
        return b >= 53;
      case 19:
        return b >= 47;
      case 20:
        return b >= 42;
      case 21:
        return b >= 37;
      case 22:
        return b >= 32 /*0x20*/;
      case 23:
        return b >= 27;
      case 24:
        return b >= 22;
      case 25:
        return b >= 17;
      case 26:
        return b >= 11;
      case 27:
        return b >= 6;
      case 28:
        return b >= 1;
      default:
        return g >= 29;
    }
  }

  private static bool CheckR149(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 143;
      case 1:
        return b >= 138;
      case 2:
        return b >= 132;
      case 3:
        return b >= (int) sbyte.MaxValue;
      case 4:
        return b >= 122;
      case 5:
        return b >= 117;
      case 6:
        return b >= 112 /*0x70*/;
      case 7:
        return b >= 107;
      case 8:
        return b >= 102;
      case 9:
        return b >= 96 /*0x60*/;
      case 10:
        return b >= 91;
      case 11:
        return b >= 86;
      case 12:
        return b >= 81;
      case 13:
        return b >= 76;
      case 14:
        return b >= 71;
      case 15:
        return b >= 65;
      case 16 /*0x10*/:
        return b >= 60;
      case 17:
        return b >= 55;
      case 18:
        return b >= 50;
      case 19:
        return b >= 45;
      case 20:
        return b >= 40;
      case 21:
        return b >= 35;
      case 22:
        return b >= 29;
      case 23:
        return b >= 24;
      case 24:
        return b >= 19;
      case 25:
        return b >= 14;
      case 26:
        return b >= 9;
      case 27:
        return b >= 4;
      default:
        return g >= 28;
    }
  }

  private static bool CheckR150(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 140;
      case 1:
        return b >= 135;
      case 2:
        return b >= 130;
      case 3:
        return b >= 125;
      case 4:
        return b >= 119;
      case 5:
        return b >= 114;
      case 6:
        return b >= 109;
      case 7:
        return b >= 104;
      case 8:
        return b >= 99;
      case 9:
        return b >= 94;
      case 10:
        return b >= 89;
      case 11:
        return b >= 83;
      case 12:
        return b >= 78;
      case 13:
        return b >= 73;
      case 14:
        return b >= 68;
      case 15:
        return b >= 63 /*0x3F*/;
      case 16 /*0x10*/:
        return b >= 58;
      case 17:
        return b >= 53;
      case 18:
        return b >= 47;
      case 19:
        return b >= 42;
      case 20:
        return b >= 37;
      case 21:
        return b >= 32 /*0x20*/;
      case 22:
        return b >= 27;
      case 23:
        return b >= 22;
      case 24:
        return b >= 16 /*0x10*/;
      case 25:
        return b >= 11;
      case 26:
        return b >= 6;
      case 27:
        return b >= 1;
      default:
        return g >= 28;
    }
  }

  private static bool CheckR151(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 137;
      case 1:
        return b >= 132;
      case 2:
        return b >= (int) sbyte.MaxValue;
      case 3:
        return b >= 122;
      case 4:
        return b >= 117;
      case 5:
        return b >= 112 /*0x70*/;
      case 6:
        return b >= 107;
      case 7:
        return b >= 101;
      case 8:
        return b >= 96 /*0x60*/;
      case 9:
        return b >= 91;
      case 10:
        return b >= 86;
      case 11:
        return b >= 81;
      case 12:
        return b >= 76;
      case 13:
        return b >= 71;
      case 14:
        return b >= 65;
      case 15:
        return b >= 60;
      case 16 /*0x10*/:
        return b >= 55;
      case 17:
        return b >= 50;
      case 18:
        return b >= 45;
      case 19:
        return b >= 40;
      case 20:
        return b >= 34;
      case 21:
        return b >= 29;
      case 22:
        return b >= 24;
      case 23:
        return b >= 19;
      case 24:
        return b >= 14;
      case 25:
        return b >= 9;
      case 26:
        return b >= 4;
      default:
        return g >= 27;
    }
  }

  private static bool CheckR152(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 135;
      case 1:
        return b >= 130;
      case 2:
        return b >= 125;
      case 3:
        return b >= 119;
      case 4:
        return b >= 114;
      case 5:
        return b >= 109;
      case 6:
        return b >= 104;
      case 7:
        return b >= 99;
      case 8:
        return b >= 94;
      case 9:
        return b >= 88;
      case 10:
        return b >= 83;
      case 11:
        return b >= 78;
      case 12:
        return b >= 73;
      case 13:
        return b >= 68;
      case 14:
        return b >= 63 /*0x3F*/;
      case 15:
        return b >= 58;
      case 16 /*0x10*/:
        return b >= 52;
      case 17:
        return b >= 47;
      case 18:
        return b >= 42;
      case 19:
        return b >= 37;
      case 20:
        return b >= 32 /*0x20*/;
      case 21:
        return b >= 27;
      case 22:
        return b >= 22;
      case 23:
        return b >= 16 /*0x10*/;
      case 24:
        return b >= 11;
      case 25:
        return b >= 6;
      case 26:
        return b >= 1;
      default:
        return g >= 27;
    }
  }

  private static bool CheckR153(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 132;
      case 1:
        return b >= (int) sbyte.MaxValue;
      case 2:
        return b >= 122;
      case 3:
        return b >= 117;
      case 4:
        return b >= 112 /*0x70*/;
      case 5:
        return b >= 106;
      case 6:
        return b >= 101;
      case 7:
        return b >= 96 /*0x60*/;
      case 8:
        return b >= 91;
      case 9:
        return b >= 86;
      case 10:
        return b >= 81;
      case 11:
        return b >= 76;
      case 12:
        return b >= 70;
      case 13:
        return b >= 65;
      case 14:
        return b >= 60;
      case 15:
        return b >= 55;
      case 16 /*0x10*/:
        return b >= 50;
      case 17:
        return b >= 45;
      case 18:
        return b >= 40;
      case 19:
        return b >= 34;
      case 20:
        return b >= 29;
      case 21:
        return b >= 24;
      case 22:
        return b >= 19;
      case 23:
        return b >= 14;
      case 24:
        return b >= 9;
      case 25:
        return b >= 3;
      default:
        return g >= 26;
    }
  }

  private static bool CheckR154(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 130;
      case 1:
        return b >= 124;
      case 2:
        return b >= 119;
      case 3:
        return b >= 114;
      case 4:
        return b >= 109;
      case 5:
        return b >= 104;
      case 6:
        return b >= 99;
      case 7:
        return b >= 94;
      case 8:
        return b >= 88;
      case 9:
        return b >= 83;
      case 10:
        return b >= 78;
      case 11:
        return b >= 73;
      case 12:
        return b >= 68;
      case 13:
        return b >= 63 /*0x3F*/;
      case 14:
        return b >= 57;
      case 15:
        return b >= 52;
      case 16 /*0x10*/:
        return b >= 47;
      case 17:
        return b >= 42;
      case 18:
        return b >= 37;
      case 19:
        return b >= 32 /*0x20*/;
      case 20:
        return b >= 27;
      case 21:
        return b >= 21;
      case 22:
        return b >= 16 /*0x10*/;
      case 23:
        return b >= 11;
      case 24:
        return b >= 6;
      case 25:
        return b >= 1;
      default:
        return g >= 26;
    }
  }

  private static bool CheckR155(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= (int) sbyte.MaxValue;
      case 1:
        return b >= 122;
      case 2:
        return b >= 117;
      case 3:
        return b >= 112 /*0x70*/;
      case 4:
        return b >= 106;
      case 5:
        return b >= 101;
      case 6:
        return b >= 96 /*0x60*/;
      case 7:
        return b >= 91;
      case 8:
        return b >= 86;
      case 9:
        return b >= 81;
      case 10:
        return b >= 75;
      case 11:
        return b >= 70;
      case 12:
        return b >= 65;
      case 13:
        return b >= 60;
      case 14:
        return b >= 55;
      case 15:
        return b >= 50;
      case 16 /*0x10*/:
        return b >= 45;
      case 17:
        return b >= 39;
      case 18:
        return b >= 34;
      case 19:
        return b >= 29;
      case 20:
        return b >= 24;
      case 21:
        return b >= 19;
      case 22:
        return b >= 14;
      case 23:
        return b >= 9;
      case 24:
        return b >= 3;
      default:
        return g >= 25;
    }
  }

  private static bool CheckR156(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 124;
      case 1:
        return b >= 119;
      case 2:
        return b >= 114;
      case 3:
        return b >= 109;
      case 4:
        return b >= 104;
      case 5:
        return b >= 99;
      case 6:
        return b >= 93;
      case 7:
        return b >= 88;
      case 8:
        return b >= 83;
      case 9:
        return b >= 78;
      case 10:
        return b >= 73;
      case 11:
        return b >= 68;
      case 12:
        return b >= 63 /*0x3F*/;
      case 13:
        return b >= 57;
      case 14:
        return b >= 52;
      case 15:
        return b >= 47;
      case 16 /*0x10*/:
        return b >= 42;
      case 17:
        return b >= 37;
      case 18:
        return b >= 32 /*0x20*/;
      case 19:
        return b >= 27;
      case 20:
        return b >= 21;
      case 21:
        return b >= 16 /*0x10*/;
      case 22:
        return b >= 11;
      case 23:
        return b >= 6;
      case 24:
        return b >= 1;
      default:
        return g >= 25;
    }
  }

  private static bool CheckR157(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 122;
      case 1:
        return b >= 117;
      case 2:
        return b >= 111;
      case 3:
        return b >= 106;
      case 4:
        return b >= 101;
      case 5:
        return b >= 96 /*0x60*/;
      case 6:
        return b >= 91;
      case 7:
        return b >= 86;
      case 8:
        return b >= 81;
      case 9:
        return b >= 75;
      case 10:
        return b >= 70;
      case 11:
        return b >= 65;
      case 12:
        return b >= 60;
      case 13:
        return b >= 55;
      case 14:
        return b >= 50;
      case 15:
        return b >= 44;
      case 16 /*0x10*/:
        return b >= 39;
      case 17:
        return b >= 34;
      case 18:
        return b >= 29;
      case 19:
        return b >= 24;
      case 20:
        return b >= 19;
      case 21:
        return b >= 14;
      case 22:
        return b >= 8;
      case 23:
        return b >= 3;
      default:
        return g >= 24;
    }
  }

  private static bool CheckR158(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 119;
      case 1:
        return b >= 114;
      case 2:
        return b >= 109;
      case 3:
        return b >= 104;
      case 4:
        return b >= 98;
      case 5:
        return b >= 93;
      case 6:
        return b >= 88;
      case 7:
        return b >= 83;
      case 8:
        return b >= 78;
      case 9:
        return b >= 73;
      case 10:
        return b >= 68;
      case 11:
        return b >= 62;
      case 12:
        return b >= 57;
      case 13:
        return b >= 52;
      case 14:
        return b >= 47;
      case 15:
        return b >= 42;
      case 16 /*0x10*/:
        return b >= 37;
      case 17:
        return b >= 32 /*0x20*/;
      case 18:
        return b >= 26;
      case 19:
        return b >= 21;
      case 20:
        return b >= 16 /*0x10*/;
      case 21:
        return b >= 11;
      case 22:
        return b >= 6;
      case 23:
        return b >= 1;
      default:
        return g >= 24;
    }
  }

  private static bool CheckR159(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 116;
      case 1:
        return b >= 111;
      case 2:
        return b >= 106;
      case 3:
        return b >= 101;
      case 4:
        return b >= 96 /*0x60*/;
      case 5:
        return b >= 91;
      case 6:
        return b >= 86;
      case 7:
        return b >= 80 /*0x50*/;
      case 8:
        return b >= 75;
      case 9:
        return b >= 70;
      case 10:
        return b >= 65;
      case 11:
        return b >= 60;
      case 12:
        return b >= 55;
      case 13:
        return b >= 50;
      case 14:
        return b >= 44;
      case 15:
        return b >= 39;
      case 16 /*0x10*/:
        return b >= 34;
      case 17:
        return b >= 29;
      case 18:
        return b >= 24;
      case 19:
        return b >= 19;
      case 20:
        return b >= 13;
      case 21:
        return b >= 8;
      case 22:
        return b >= 3;
      default:
        return g >= 23;
    }
  }

  private static bool CheckR160(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 114;
      case 1:
        return b >= 109;
      case 2:
        return b >= 104;
      case 3:
        return b >= 98;
      case 4:
        return b >= 93;
      case 5:
        return b >= 88;
      case 6:
        return b >= 83;
      case 7:
        return b >= 78;
      case 8:
        return b >= 73;
      case 9:
        return b >= 68;
      case 10:
        return b >= 62;
      case 11:
        return b >= 57;
      case 12:
        return b >= 52;
      case 13:
        return b >= 47;
      case 14:
        return b >= 42;
      case 15:
        return b >= 37;
      case 16 /*0x10*/:
        return b >= 31 /*0x1F*/;
      case 17:
        return b >= 26;
      case 18:
        return b >= 21;
      case 19:
        return b >= 16 /*0x10*/;
      case 20:
        return b >= 11;
      case 21:
        return b >= 6;
      case 22:
        return b >= 1;
      default:
        return g >= 23;
    }
  }

  private static bool CheckR161(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 111;
      case 1:
        return b >= 106;
      case 2:
        return b >= 101;
      case 3:
        return b >= 96 /*0x60*/;
      case 4:
        return b >= 91;
      case 5:
        return b >= 85;
      case 6:
        return b >= 80 /*0x50*/;
      case 7:
        return b >= 75;
      case 8:
        return b >= 70;
      case 9:
        return b >= 65;
      case 10:
        return b >= 60;
      case 11:
        return b >= 55;
      case 12:
        return b >= 49;
      case 13:
        return b >= 44;
      case 14:
        return b >= 39;
      case 15:
        return b >= 34;
      case 16 /*0x10*/:
        return b >= 29;
      case 17:
        return b >= 24;
      case 18:
        return b >= 19;
      case 19:
        return b >= 13;
      case 20:
        return b >= 8;
      case 21:
        return b >= 3;
      default:
        return g >= 22;
    }
  }

  private static bool CheckR162(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 109;
      case 1:
        return b >= 103;
      case 2:
        return b >= 98;
      case 3:
        return b >= 93;
      case 4:
        return b >= 88;
      case 5:
        return b >= 83;
      case 6:
        return b >= 78;
      case 7:
        return b >= 73;
      case 8:
        return b >= 67;
      case 9:
        return b >= 62;
      case 10:
        return b >= 57;
      case 11:
        return b >= 52;
      case 12:
        return b >= 47;
      case 13:
        return b >= 42;
      case 14:
        return b >= 37;
      case 15:
        return b >= 31 /*0x1F*/;
      case 16 /*0x10*/:
        return b >= 26;
      case 17:
        return b >= 21;
      case 18:
        return b >= 16 /*0x10*/;
      case 19:
        return b >= 11;
      case 20:
        return b >= 6;
      default:
        return g >= 21;
    }
  }

  private static bool CheckR163(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 106;
      case 1:
        return b >= 101;
      case 2:
        return b >= 96 /*0x60*/;
      case 3:
        return b >= 91;
      case 4:
        return b >= 85;
      case 5:
        return b >= 80 /*0x50*/;
      case 6:
        return b >= 75;
      case 7:
        return b >= 70;
      case 8:
        return b >= 65;
      case 9:
        return b >= 60;
      case 10:
        return b >= 54;
      case 11:
        return b >= 49;
      case 12:
        return b >= 44;
      case 13:
        return b >= 39;
      case 14:
        return b >= 34;
      case 15:
        return b >= 29;
      case 16 /*0x10*/:
        return b >= 24;
      case 17:
        return b >= 18;
      case 18:
        return b >= 13;
      case 19:
        return b >= 8;
      case 20:
        return b >= 3;
      default:
        return g >= 21;
    }
  }

  private static bool CheckR164(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 103;
      case 1:
        return b >= 98;
      case 2:
        return b >= 93;
      case 3:
        return b >= 88;
      case 4:
        return b >= 83;
      case 5:
        return b >= 78;
      case 6:
        return b >= 72;
      case 7:
        return b >= 67;
      case 8:
        return b >= 62;
      case 9:
        return b >= 57;
      case 10:
        return b >= 52;
      case 11:
        return b >= 47;
      case 12:
        return b >= 42;
      case 13:
        return b >= 36;
      case 14:
        return b >= 31 /*0x1F*/;
      case 15:
        return b >= 26;
      case 16 /*0x10*/:
        return b >= 21;
      case 17:
        return b >= 16 /*0x10*/;
      case 18:
        return b >= 11;
      case 19:
        return b >= 6;
      default:
        return g >= 20;
    }
  }

  private static bool CheckR165(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 101;
      case 1:
        return b >= 96 /*0x60*/;
      case 2:
        return b >= 90;
      case 3:
        return b >= 85;
      case 4:
        return b >= 80 /*0x50*/;
      case 5:
        return b >= 75;
      case 6:
        return b >= 70;
      case 7:
        return b >= 65;
      case 8:
        return b >= 60;
      case 9:
        return b >= 54;
      case 10:
        return b >= 49;
      case 11:
        return b >= 44;
      case 12:
        return b >= 39;
      case 13:
        return b >= 34;
      case 14:
        return b >= 29;
      case 15:
        return b >= 23;
      case 16 /*0x10*/:
        return b >= 18;
      case 17:
        return b >= 13;
      case 18:
        return b >= 8;
      case 19:
        return b >= 3;
      default:
        return g >= 20;
    }
  }

  private static bool CheckR166(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 98;
      case 1:
        return b >= 93;
      case 2:
        return b >= 88;
      case 3:
        return b >= 83;
      case 4:
        return b >= 78;
      case 5:
        return b >= 72;
      case 6:
        return b >= 67;
      case 7:
        return b >= 62;
      case 8:
        return b >= 57;
      case 9:
        return b >= 52;
      case 10:
        return b >= 47;
      case 11:
        return b >= 41;
      case 12:
        return b >= 36;
      case 13:
        return b >= 31 /*0x1F*/;
      case 14:
        return b >= 26;
      case 15:
        return b >= 21;
      case 16 /*0x10*/:
        return b >= 16 /*0x10*/;
      case 17:
        return b >= 11;
      case 18:
        return b >= 5;
      default:
        return g >= 19;
    }
  }

  private static bool CheckR167(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 95;
      case 1:
        return b >= 90;
      case 2:
        return b >= 85;
      case 3:
        return b >= 80 /*0x50*/;
      case 4:
        return b >= 75;
      case 5:
        return b >= 70;
      case 6:
        return b >= 65;
      case 7:
        return b >= 59;
      case 8:
        return b >= 54;
      case 9:
        return b >= 49;
      case 10:
        return b >= 44;
      case 11:
        return b >= 39;
      case 12:
        return b >= 34;
      case 13:
        return b >= 29;
      case 14:
        return b >= 23;
      case 15:
        return b >= 18;
      case 16 /*0x10*/:
        return b >= 13;
      case 17:
        return b >= 8;
      case 18:
        return b >= 3;
      default:
        return g >= 19;
    }
  }

  private static bool CheckR168(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 93;
      case 1:
        return b >= 88;
      case 2:
        return b >= 83;
      case 3:
        return b >= 77;
      case 4:
        return b >= 72;
      case 5:
        return b >= 67;
      case 6:
        return b >= 62;
      case 7:
        return b >= 57;
      case 8:
        return b >= 52;
      case 9:
        return b >= 47;
      case 10:
        return b >= 41;
      case 11:
        return b >= 36;
      case 12:
        return b >= 31 /*0x1F*/;
      case 13:
        return b >= 26;
      case 14:
        return b >= 21;
      case 15:
        return b >= 16 /*0x10*/;
      case 16 /*0x10*/:
        return b >= 10;
      case 17:
        return b >= 5;
      default:
        return g >= 18;
    }
  }

  private static bool CheckR169(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 90;
      case 1:
        return b >= 85;
      case 2:
        return b >= 80 /*0x50*/;
      case 3:
        return b >= 75;
      case 4:
        return b >= 70;
      case 5:
        return b >= 64 /*0x40*/;
      case 6:
        return b >= 59;
      case 7:
        return b >= 54;
      case 8:
        return b >= 49;
      case 9:
        return b >= 44;
      case 10:
        return b >= 39;
      case 11:
        return b >= 34;
      case 12:
        return b >= 28;
      case 13:
        return b >= 23;
      case 14:
        return b >= 18;
      case 15:
        return b >= 13;
      case 16 /*0x10*/:
        return b >= 8;
      case 17:
        return b >= 3;
      default:
        return g >= 18;
    }
  }

  private static bool CheckR170(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 88;
      case 1:
        return b >= 82;
      case 2:
        return b >= 77;
      case 3:
        return b >= 72;
      case 4:
        return b >= 67;
      case 5:
        return b >= 62;
      case 6:
        return b >= 57;
      case 7:
        return b >= 52;
      case 8:
        return b >= 46;
      case 9:
        return b >= 41;
      case 10:
        return b >= 36;
      case 11:
        return b >= 31 /*0x1F*/;
      case 12:
        return b >= 26;
      case 13:
        return b >= 21;
      case 14:
        return b >= 16 /*0x10*/;
      case 15:
        return b >= 10;
      case 16 /*0x10*/:
        return b >= 5;
      default:
        return g >= 17;
    }
  }

  private static bool CheckR171(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 85;
      case 1:
        return b >= 80 /*0x50*/;
      case 2:
        return b >= 75;
      case 3:
        return b >= 70;
      case 4:
        return b >= 64 /*0x40*/;
      case 5:
        return b >= 59;
      case 6:
        return b >= 54;
      case 7:
        return b >= 49;
      case 8:
        return b >= 44;
      case 9:
        return b >= 39;
      case 10:
        return b >= 34;
      case 11:
        return b >= 28;
      case 12:
        return b >= 23;
      case 13:
        return b >= 18;
      case 14:
        return b >= 13;
      case 15:
        return b >= 8;
      case 16 /*0x10*/:
        return b >= 3;
      default:
        return g >= 17;
    }
  }

  private static bool CheckR172(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 82;
      case 1:
        return b >= 77;
      case 2:
        return b >= 72;
      case 3:
        return b >= 67;
      case 4:
        return b >= 62;
      case 5:
        return b >= 57;
      case 6:
        return b >= 51;
      case 7:
        return b >= 46;
      case 8:
        return b >= 41;
      case 9:
        return b >= 36;
      case 10:
        return b >= 31 /*0x1F*/;
      case 11:
        return b >= 26;
      case 12:
        return b >= 21;
      case 13:
        return b >= 15;
      case 14:
        return b >= 10;
      case 15:
        return b >= 5;
      default:
        return g >= 16 /*0x10*/;
    }
  }

  private static bool CheckR173(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 80 /*0x50*/;
      case 1:
        return b >= 75;
      case 2:
        return b >= 69;
      case 3:
        return b >= 64 /*0x40*/;
      case 4:
        return b >= 59;
      case 5:
        return b >= 54;
      case 6:
        return b >= 49;
      case 7:
        return b >= 44;
      case 8:
        return b >= 39;
      case 9:
        return b >= 33;
      case 10:
        return b >= 28;
      case 11:
        return b >= 23;
      case 12:
        return b >= 18;
      case 13:
        return b >= 13;
      case 14:
        return b >= 8;
      case 15:
        return b >= 3;
      default:
        return g >= 16 /*0x10*/;
    }
  }

  private static bool CheckR174(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 77;
      case 1:
        return b >= 72;
      case 2:
        return b >= 67;
      case 3:
        return b >= 62;
      case 4:
        return b >= 57;
      case 5:
        return b >= 51;
      case 6:
        return b >= 46;
      case 7:
        return b >= 41;
      case 8:
        return b >= 36;
      case 9:
        return b >= 31 /*0x1F*/;
      case 10:
        return b >= 26;
      case 11:
        return b >= 20;
      case 12:
        return b >= 15;
      case 13:
        return b >= 10;
      case 14:
        return b >= 5;
      default:
        return g >= 15;
    }
  }

  private static bool CheckR175(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 75;
      case 1:
        return b >= 69;
      case 2:
        return b >= 64 /*0x40*/;
      case 3:
        return b >= 59;
      case 4:
        return b >= 54;
      case 5:
        return b >= 49;
      case 6:
        return b >= 44;
      case 7:
        return b >= 38;
      case 8:
        return b >= 33;
      case 9:
        return b >= 28;
      case 10:
        return b >= 23;
      case 11:
        return b >= 18;
      case 12:
        return b >= 13;
      case 13:
        return b >= 8;
      case 14:
        return b >= 2;
      default:
        return g >= 15;
    }
  }

  private static bool CheckR176(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 72;
      case 1:
        return b >= 67;
      case 2:
        return b >= 62;
      case 3:
        return b >= 56;
      case 4:
        return b >= 51;
      case 5:
        return b >= 46;
      case 6:
        return b >= 41;
      case 7:
        return b >= 36;
      case 8:
        return b >= 31 /*0x1F*/;
      case 9:
        return b >= 26;
      case 10:
        return b >= 20;
      case 11:
        return b >= 15;
      case 12:
        return b >= 10;
      case 13:
        return b >= 5;
      default:
        return g >= 14;
    }
  }

  private static bool CheckR177(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 69;
      case 1:
        return b >= 64 /*0x40*/;
      case 2:
        return b >= 59;
      case 3:
        return b >= 54;
      case 4:
        return b >= 49;
      case 5:
        return b >= 44;
      case 6:
        return b >= 38;
      case 7:
        return b >= 33;
      case 8:
        return b >= 28;
      case 9:
        return b >= 23;
      case 10:
        return b >= 18;
      case 11:
        return b >= 13;
      case 12:
        return b >= 7;
      case 13:
        return b >= 2;
      default:
        return g >= 14;
    }
  }

  private static bool CheckR178(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 67;
      case 1:
        return b >= 61;
      case 2:
        return b >= 56;
      case 3:
        return b >= 51;
      case 4:
        return b >= 46;
      case 5:
        return b >= 41;
      case 6:
        return b >= 36;
      case 7:
        return b >= 31 /*0x1F*/;
      case 8:
        return b >= 25;
      case 9:
        return b >= 20;
      case 10:
        return b >= 15;
      case 11:
        return b >= 10;
      case 12:
        return b >= 5;
      default:
        return g >= 13;
    }
  }

  private static bool CheckR179(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 64 /*0x40*/;
      case 1:
        return b >= 59;
      case 2:
        return b >= 54;
      case 3:
        return b >= 49;
      case 4:
        return b >= 43;
      case 5:
        return b >= 38;
      case 6:
        return b >= 33;
      case 7:
        return b >= 28;
      case 8:
        return b >= 23;
      case 9:
        return b >= 18;
      case 10:
        return b >= 13;
      case 11:
        return b >= 7;
      case 12:
        return b >= 2;
      default:
        return g >= 13;
    }
  }

  private static bool CheckR180(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 61;
      case 1:
        return b >= 56;
      case 2:
        return b >= 51;
      case 3:
        return b >= 46;
      case 4:
        return b >= 41;
      case 5:
        return b >= 36;
      case 6:
        return b >= 30;
      case 7:
        return b >= 25;
      case 8:
        return b >= 20;
      case 9:
        return b >= 15;
      case 10:
        return b >= 10;
      case 11:
        return b >= 5;
      default:
        return g >= 12;
    }
  }

  private static bool CheckR181(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 59;
      case 1:
        return b >= 54;
      case 2:
        return b >= 48 /*0x30*/;
      case 3:
        return b >= 43;
      case 4:
        return b >= 38;
      case 5:
        return b >= 33;
      case 6:
        return b >= 28;
      case 7:
        return b >= 23;
      case 8:
        return b >= 18;
      case 9:
        return b >= 12;
      case 10:
        return b >= 7;
      case 11:
        return b >= 2;
      default:
        return g >= 12;
    }
  }

  private static bool CheckR182(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 56;
      case 1:
        return b >= 51;
      case 2:
        return b >= 46;
      case 3:
        return b >= 41;
      case 4:
        return b >= 36;
      case 5:
        return b >= 30;
      case 6:
        return b >= 25;
      case 7:
        return b >= 20;
      case 8:
        return b >= 15;
      case 9:
        return b >= 10;
      case 10:
        return b >= 5;
      default:
        return g >= 11;
    }
  }

  private static bool CheckR183(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 54;
      case 1:
        return b >= 48 /*0x30*/;
      case 2:
        return b >= 43;
      case 3:
        return b >= 38;
      case 4:
        return b >= 33;
      case 5:
        return b >= 28;
      case 6:
        return b >= 23;
      case 7:
        return b >= 17;
      case 8:
        return b >= 12;
      case 9:
        return b >= 7;
      case 10:
        return b >= 2;
      default:
        return g >= 11;
    }
  }

  private static bool CheckR184(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 51;
      case 1:
        return b >= 46;
      case 2:
        return b >= 41;
      case 3:
        return b >= 35;
      case 4:
        return b >= 30;
      case 5:
        return b >= 25;
      case 6:
        return b >= 20;
      case 7:
        return b >= 15;
      case 8:
        return b >= 10;
      case 9:
        return b >= 5;
      default:
        return g >= 10;
    }
  }

  private static bool CheckR185(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 48 /*0x30*/;
      case 1:
        return b >= 43;
      case 2:
        return b >= 38;
      case 3:
        return b >= 33;
      case 4:
        return b >= 28;
      case 5:
        return b >= 23;
      case 6:
        return b >= 17;
      case 7:
        return b >= 12;
      case 8:
        return b >= 7;
      case 9:
        return b >= 2;
      default:
        return g >= 10;
    }
  }

  private static bool CheckR186(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 46;
      case 1:
        return b >= 41;
      case 2:
        return b >= 35;
      case 3:
        return b >= 30;
      case 4:
        return b >= 25;
      case 5:
        return b >= 20;
      case 6:
        return b >= 15;
      case 7:
        return b >= 10;
      case 8:
        return b >= 4;
      default:
        return g >= 9;
    }
  }

  private static bool CheckR187(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 43;
      case 1:
        return b >= 38;
      case 2:
        return b >= 33;
      case 3:
        return b >= 28;
      case 4:
        return b >= 22;
      case 5:
        return b >= 17;
      case 6:
        return b >= 12;
      case 7:
        return b >= 7;
      case 8:
        return b >= 2;
      default:
        return g >= 9;
    }
  }

  private static bool CheckR188(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 40;
      case 1:
        return b >= 35;
      case 2:
        return b >= 30;
      case 3:
        return b >= 25;
      case 4:
        return b >= 20;
      case 5:
        return b >= 15;
      case 6:
        return b >= 10;
      case 7:
        return b >= 4;
      default:
        return g >= 8;
    }
  }

  private static bool CheckR189(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 38;
      case 1:
        return b >= 33;
      case 2:
        return b >= 27;
      case 3:
        return b >= 22;
      case 4:
        return b >= 17;
      case 5:
        return b >= 12;
      case 6:
        return b >= 7;
      case 7:
        return b >= 2;
      default:
        return g >= 8;
    }
  }

  private static bool CheckR190(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 35;
      case 1:
        return b >= 30;
      case 2:
        return b >= 25;
      case 3:
        return b >= 20;
      case 4:
        return b >= 15;
      case 5:
        return b >= 9;
      case 6:
        return b >= 4;
      default:
        return g >= 7;
    }
  }

  private static bool CheckR191(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 33;
      case 1:
        return b >= 27;
      case 2:
        return b >= 22;
      case 3:
        return b >= 17;
      case 4:
        return b >= 12;
      case 5:
        return b >= 7;
      case 6:
        return b >= 2;
      default:
        return g >= 7;
    }
  }

  private static bool CheckR192(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 30;
      case 1:
        return b >= 25;
      case 2:
        return b >= 20;
      case 3:
        return b >= 14;
      case 4:
        return b >= 9;
      case 5:
        return b >= 4;
      default:
        return g >= 6;
    }
  }

  private static bool CheckR193(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 27;
      case 1:
        return b >= 22;
      case 2:
        return b >= 17;
      case 3:
        return b >= 12;
      case 4:
        return b >= 7;
      case 5:
        return b >= 2;
      default:
        return g >= 6;
    }
  }

  private static bool CheckR194(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 25;
      case 1:
        return b >= 20;
      case 2:
        return b >= 14;
      case 3:
        return b >= 9;
      case 4:
        return b >= 4;
      default:
        return g >= 5;
    }
  }

  private static bool CheckR195(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 22;
      case 1:
        return b >= 17;
      case 2:
        return b >= 12;
      case 3:
        return b >= 7;
      case 4:
        return b >= 1;
      default:
        return g >= 5;
    }
  }

  private static bool CheckR196(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 19;
      case 1:
        return b >= 14;
      case 2:
        return b >= 9;
      case 3:
        return b >= 4;
      default:
        return g >= 4;
    }
  }

  private static bool CheckR197(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 17;
      case 1:
        return b >= 12;
      case 2:
        return b >= 7;
      case 3:
        return b >= 1;
      default:
        return g >= 4;
    }
  }

  private static bool CheckR198(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 14;
      case 1:
        return b >= 9;
      case 2:
        return b >= 4;
      default:
        return g >= 3;
    }
  }

  private static bool CheckR199(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 12;
      case 1:
        return b >= 6;
      case 2:
        return b >= 1;
      default:
        return g >= 3;
    }
  }

  private static bool CheckR200(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 9;
      case 1:
        return b >= 4;
      default:
        return g >= 2;
    }
  }

  private static bool CheckR201(int g, int b)
  {
    switch (g)
    {
      case 0:
        return b >= 6;
      case 1:
        return b >= 1;
      default:
        return g >= 2;
    }
  }

  private static bool CheckR202(int g, int b) => g == 0 ? b >= 4 : g >= 1;

  private static bool CheckR203(int g, int b) => g == 0 ? b >= 1 : g >= 1;
}
