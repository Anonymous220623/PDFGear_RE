// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public static class ColorHelper
{
  public static Color FromArgb(uint argb)
  {
    return Color.FromArgb((byte) ((4278190080U /*0xFF000000*/ & argb) >> 24), (byte) ((16711680U /*0xFF0000*/ & argb) >> 16 /*0x10*/), (byte) ((65280U & argb) >> 8), (byte) ((uint) byte.MaxValue & argb));
  }

  public static Color FromRgb(uint rgb)
  {
    return Color.FromArgb(byte.MaxValue, (byte) ((16711680U /*0xFF0000*/ & rgb) >> 16 /*0x10*/), (byte) ((65280U & rgb) >> 8), (byte) ((uint) byte.MaxValue & rgb));
  }

  public static int ToColorDialogValue(this Color color)
  {
    int num1 = (int) color.B << 16 /*0x10*/;
    int num2 = (int) color.G << 8;
    int r = (int) color.R;
    int num3 = num2;
    return num1 | num3 | r;
  }
}
