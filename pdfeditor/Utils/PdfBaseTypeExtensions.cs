// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfBaseTypeExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public static class PdfBaseTypeExtensions
{
  public static bool IntersectsWith(this FS_RECTF rect, FS_RECTF rect2)
  {
    return (double) rect.left <= (double) rect2.right && (double) rect.right >= (double) rect2.left && (double) rect.top >= (double) rect2.bottom && (double) rect.bottom <= (double) rect2.top;
  }

  public static Point ToPoint(this FS_POINTF point)
  {
    return new Point((double) point.X, (double) point.Y);
  }

  public static FS_POINTF ToPdfPoint(this Point point) => new FS_POINTF(point.X, point.Y);

  public static bool Equals(this FS_POINTF point, Point point2)
  {
    return DoubleUtil.AreClose((double) point.X, point2.X) && DoubleUtil.AreClose((double) point.Y, point2.Y);
  }

  public static bool Equals(this Point point, FS_POINTF point2) => point2.Equals((object) point);

  public static Color ToColor(this FS_COLOR color)
  {
    return Color.FromArgb((byte) color.A, (byte) color.R, (byte) color.G, (byte) color.B);
  }

  public static FS_COLOR ToPdfColor(this Color color)
  {
    return new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
  }

  public static bool Equals(this FS_COLOR color, Color color2)
  {
    return color.A == (int) color2.A && color.R == (int) color2.R && color.G == (int) color2.G && color.B == (int) color2.B;
  }

  public static bool Equals(this Color color, FS_COLOR color2) => color2.Equals((object) color);
}
