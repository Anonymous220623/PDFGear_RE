// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ColorExtension
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal static class ColorExtension
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
