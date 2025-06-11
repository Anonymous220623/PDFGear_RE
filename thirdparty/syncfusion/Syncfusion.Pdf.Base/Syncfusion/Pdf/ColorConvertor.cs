// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorConvertor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal struct ColorConvertor(byte a, byte r, byte g, byte b)
{
  private byte m_alpha = a;
  private byte m_red = r;
  private byte m_green = g;
  private byte m_blue = b;

  public byte Alpha
  {
    get => this.m_alpha;
    set => this.m_alpha = value;
  }

  public byte Red
  {
    get => this.m_red;
    set => this.m_red = value;
  }

  public byte Green
  {
    get => this.m_green;
    set => this.m_green = value;
  }

  public byte Blue
  {
    get => this.m_blue;
    set => this.m_blue = value;
  }

  public ColorConvertor(byte r, byte g, byte b)
    : this(byte.MaxValue, r, g, b)
  {
  }

  public static ColorConvertor FromGray(double gray)
  {
    return ColorConvertor.FromGray(ColorConvertor.ConvertColorToByte(gray));
  }

  public static ColorConvertor FromGray(int gray, int bits)
  {
    return ColorConvertor.FromGray(ColorConvertor.ConvertColorToByte(gray, bits));
  }

  public static ColorConvertor FromGray(byte gray) => new ColorConvertor(gray, gray, gray);

  public static ColorConvertor FromCmyk(int cyan, int magenta, int yellow, int black, int bits)
  {
    return ColorConvertor.FromCmyk(ColorConvertor.ConvertColorToByte(cyan, bits), ColorConvertor.ConvertColorToByte(magenta, bits), ColorConvertor.ConvertColorToByte(yellow, bits), ColorConvertor.ConvertColorToByte(black, bits));
  }

  public static ColorConvertor FromCmyk(double cyan, double magenta, double yellow, double black)
  {
    return ColorConvertor.FromCmyk(ColorConvertor.ConvertColorToByte(cyan), ColorConvertor.ConvertColorToByte(magenta), ColorConvertor.ConvertColorToByte(yellow), ColorConvertor.ConvertColorToByte(black));
  }

  public static ColorConvertor FromCmyk(byte cyan, byte magenta, byte yellow, byte black)
  {
    return new ColorConvertor((byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) cyan + (int) black)), (byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) magenta + (int) black)), (byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) yellow + (int) black)));
  }

  public static ColorConvertor FromArgb(int alpha, int red, int green, int blue, int bits)
  {
    return ColorConvertor.FromArgb(ColorConvertor.ConvertColorToByte(alpha, bits), ColorConvertor.ConvertColorToByte(red, bits), ColorConvertor.ConvertColorToByte(green, bits), ColorConvertor.ConvertColorToByte(blue, bits));
  }

  public static ColorConvertor FromArgb(double alpha, double red, double green, double blue)
  {
    return ColorConvertor.FromArgb(ColorConvertor.ConvertColorToByte(alpha), ColorConvertor.ConvertColorToByte(red), ColorConvertor.ConvertColorToByte(green), ColorConvertor.ConvertColorToByte(blue));
  }

  public static ColorConvertor FromArgb(byte alpha, byte red, byte green, byte blue)
  {
    return new ColorConvertor(alpha, red, green, blue);
  }

  private static byte ConvertColorToByte(int component, int bits)
  {
    double num = (double) ((1 << bits) - 1);
    return ColorConvertor.ConvertColorToByte((double) component / num);
  }

  public static byte ConvertColorToByte(double component)
  {
    if (component > 1.0)
      return byte.MaxValue;
    return component < 0.0 ? (byte) 0 : (byte) (component * (double) byte.MaxValue);
  }

  public byte GetGrayComponent() => this.m_red;

  public int PixelConversion()
  {
    return (int) this.m_alpha << 24 | (int) this.m_red << 16 /*0x10*/ | (int) this.m_green << 8 | (int) this.m_blue;
  }
}
