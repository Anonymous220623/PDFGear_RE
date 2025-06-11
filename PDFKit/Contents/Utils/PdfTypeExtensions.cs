// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.PdfTypeExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Contents.Utils;

internal static class PdfTypeExtensions
{
  internal static bool IsEmpty(this FS_RECTF rect)
  {
    return (double) rect.left >= (double) rect.right || (double) rect.bottom >= (double) rect.top;
  }

  public static FS_RECTF Union(this FS_RECTF rect1, FS_RECTF rect2)
  {
    return new FS_RECTF(Math.Min(rect1.left, rect2.left), Math.Max(rect1.top, rect2.top), Math.Max(rect1.right, rect2.right), Math.Min(rect1.bottom, rect2.bottom));
  }

  public static bool Contains(this FS_RECTF rect, FS_POINTF point)
  {
    return (double) rect.left <= (double) point.X && (double) rect.right >= (double) point.X && (double) rect.top >= (double) point.Y && (double) rect.bottom <= (double) point.Y;
  }

  public static FS_POINTF ToPdfPoint(this Point point) => new FS_POINTF(point.X, point.Y);

  public static Point ToPoint(this FS_POINTF point)
  {
    return new Point((double) point.X, (double) point.Y);
  }

  public static FS_MATRIX MatrixFromPage(this PdfPageObject obj)
  {
    if (obj.Container?.Form == null)
      return obj.Matrix;
    Matrix matrix = obj.MatrixFromPage2();
    return new FS_MATRIX(matrix.M11, matrix.M21, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
  }

  public static Matrix MatrixFromPage2(this PdfPageObject obj)
  {
    Matrix identity = Matrix.Identity;
    for (PdfPageObject pdfPageObject = obj; pdfPageObject != null; pdfPageObject = (PdfPageObject) pdfPageObject.Container?.Form)
    {
      FS_MATRIX matrix = pdfPageObject.Matrix;
      identity *= new Matrix((double) matrix.a, (double) matrix.b, (double) matrix.c, (double) matrix.d, (double) matrix.e, (double) matrix.f);
    }
    return identity;
  }
}
