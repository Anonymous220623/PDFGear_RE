// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorExtension
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public static class ColorExtension
{
  public static Color Black = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
  public static Color White = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
  public static Color Empty = Color.FromArgb(0, 0, 0, 0);
  public static Color Red = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0);
  public static Color Blue = Color.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue);
  public static Color DarkGray = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
  public static Color Yellow = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
  public static Color Cyan = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
  public static Color Magenta = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
  public static Color Gray = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
  public static Color ChartForeground = SystemColors.WindowText;
  public static Color ChartBackground = SystemColors.Window;
  public static Color ChartNeutral = ColorExtension.Black;

  public static Color FromArgb(int value) => Color.FromArgb(value);

  public static Color FromName(string name) => Color.FromName(name);
}
