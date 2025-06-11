// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PdfHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class PdfHelper
{
  public static Matrix CalculateTextMatrix(Matrix m, Glyph glyph)
  {
    return new Matrix(1.0, 0.0, 0.0, 1.0, (glyph.Width * glyph.FontSize + glyph.CharSpacing + glyph.WordSpacing) * (glyph.HorizontalScaling / 100.0), 0.0) * m;
  }

  public static bool UnboxDouble(object obj, out double res)
  {
    res = 0.0;
    switch (obj)
    {
      case null:
        return false;
      case byte num1:
        res = (double) num1;
        return true;
      case int num2:
        res = (double) num2;
        return true;
      case double num3:
        res = num3;
        return true;
      default:
        return false;
    }
  }

  public static bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  public static double GetDistance(Point p1, Point p2)
  {
    return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
  }

  public static bool UnboxBool(object obj, out bool res)
  {
    res = false;
    if (obj == null || !(obj is bool flag))
      return false;
    res = flag;
    return true;
  }

  public static bool UnboxInt(object obj, out int res)
  {
    res = 0;
    switch (obj)
    {
      case null:
        return false;
      case byte num1:
        res = (int) num1;
        return true;
      case int num2:
        res = num2;
        return true;
      case double num3:
        res = (int) num3;
        return true;
      default:
        return false;
    }
  }

  public static bool EnumTryParse<TEnum>(string valueAsString, out TEnum value, bool ignoreCase) where TEnum : struct
  {
    try
    {
      value = (TEnum) Enum.Parse(typeof (TEnum), valueAsString, ignoreCase);
      return true;
    }
    catch
    {
      value = default (TEnum);
      return false;
    }
  }

  public static Rect GetBoundingRect(Rect rect, Matrix matrix)
  {
    if (matrix.IsIdentity())
      return rect;
    Point[] points = new Point[4]
    {
      new Point(rect.Left, rect.Top),
      new Point(rect.Right, rect.Top),
      new Point(rect.Right, rect.Bottom),
      new Point(rect.Left, rect.Bottom)
    };
    PdfHelper.TransformPoints(matrix, points);
    double x1 = Math.Min(Math.Min(points[0].X, points[1].X), Math.Min(points[2].X, points[3].X));
    double x2 = Math.Max(Math.Max(points[0].X, points[1].X), Math.Max(points[2].X, points[3].X));
    double y1 = Math.Min(Math.Min(points[0].Y, points[1].Y), Math.Min(points[2].Y, points[3].Y));
    double y2 = Math.Max(Math.Max(points[0].Y, points[1].Y), Math.Max(points[2].Y, points[3].Y));
    return new Rect(new Point(x1, y1), new Point(x2, y2));
  }

  private static void TransformPoints(Matrix matrix, Point[] points)
  {
    for (int index = 0; index < points.Length; ++index)
    {
      Point point = matrix.Transform(points[index]);
      points[index].X = point.X;
      points[index].Y = point.Y;
    }
  }
}
