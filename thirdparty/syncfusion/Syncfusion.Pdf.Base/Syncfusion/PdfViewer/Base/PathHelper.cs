// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PathHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class PathHelper
{
  public static Matrix CalculateTextMatrix(Matrix m, Glyph glyph)
  {
    return new Matrix(1.0, 0.0, 0.0, 1.0, (glyph.Width * glyph.FontSize + glyph.CharSpacing + glyph.WordSpacing) * (glyph.HorizontalScaling / 100.0), 0.0) * m;
  }

  public static bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  public static double GetDistance(Point p1, Point p2)
  {
    return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
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
    PathHelper.TransformPoints(matrix, points);
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
