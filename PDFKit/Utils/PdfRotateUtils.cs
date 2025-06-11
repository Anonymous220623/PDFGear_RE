// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRotateUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Linq;

#nullable disable
namespace PDFKit.Utils;

public static class PdfRotateUtils
{
  public static FS_POINTF GetAnchorPoint(FS_RECTF rect, PageRotate rotate)
  {
    switch (rotate)
    {
      case PageRotate.Normal:
        return new FS_POINTF(rect.left, rect.top);
      case PageRotate.Rotate90:
        return new FS_POINTF(rect.left, rect.bottom);
      case PageRotate.Rotate180:
        return new FS_POINTF(rect.right, rect.bottom);
      case PageRotate.Rotate270:
        return new FS_POINTF(rect.right, rect.top);
      default:
        return new FS_POINTF(rect.left, rect.top);
    }
  }

  public static FS_RECTF GetOriginalRectangle(FS_RECTF rect, PageRotate rotate)
  {
    if (rotate == PageRotate.Normal)
      return rect;
    FS_MATRIX fsMatrix = new FS_MATRIX();
    fsMatrix.SetIdentity();
    fsMatrix.Translate((float) (-(double) rect.left - (double) rect.Width / 2.0), (float) (-(double) rect.bottom - (double) rect.Height / 2.0));
    if (rotate > PageRotate.Normal && rotate <= PageRotate.Rotate270)
      fsMatrix.Rotate((float) ((double) ((int) rotate * 90) * Math.PI / 180.0));
    fsMatrix.Translate(rect.left + rect.Width / 2f, rect.bottom + rect.Height / 2f);
    fsMatrix.TransformRect(ref rect);
    return rect;
  }

  public static void RotateMatrix(PageRotate _rotate, FS_MATRIX _matrix)
  {
    double angle = 0.0;
    switch (_rotate)
    {
      case PageRotate.Rotate90:
        angle = Math.PI / 2.0;
        break;
      case PageRotate.Rotate180:
        angle = Math.PI;
        break;
      case PageRotate.Rotate270:
        angle = 3.0 * Math.PI / 2.0;
        break;
    }
    _matrix.Rotate((float) angle);
  }

  public static FS_RECTF GetRotatedRect(
    FS_RECTF pageRect,
    FS_POINTF anchorPoint,
    PageRotate rotate)
  {
    float width = pageRect.Width;
    float height = pageRect.Height;
    switch (rotate)
    {
      case PageRotate.Normal:
        return new FS_RECTF(anchorPoint.X, anchorPoint.Y, anchorPoint.X + width, anchorPoint.Y - height);
      case PageRotate.Rotate90:
        return new FS_RECTF(anchorPoint.X, anchorPoint.Y + height, anchorPoint.X + width, anchorPoint.Y);
      case PageRotate.Rotate180:
        return new FS_RECTF(anchorPoint.X - width, anchorPoint.Y + height, anchorPoint.X, anchorPoint.Y);
      case PageRotate.Rotate270:
        return new FS_RECTF(anchorPoint.X - width, anchorPoint.Y, anchorPoint.X, anchorPoint.Y - height);
      default:
        return pageRect;
    }
  }

  public static float GetRotate(this PdfMarkupAnnotation annot)
  {
    return annot.GetRotate(out PageRotate? _, out PageRotate _);
  }

  public static float GetRotate(
    this PdfMarkupAnnotation annot,
    out PageRotate? rotate,
    out PageRotate adjustRotate)
  {
    rotate = new PageRotate?();
    adjustRotate = PageRotate.Normal;
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return 0.0f;
    try
    {
      if (annot.Dictionary != null)
      {
        PdfTypeBase pdfTypeBase1;
        if (annot.Dictionary.TryGetValue("Rotate", out pdfTypeBase1) && pdfTypeBase1.Is<PdfTypeNumber>())
        {
          float floatValue = pdfTypeBase1.As<PdfTypeNumber>().FloatValue;
          while ((double) floatValue > 360.0)
            floatValue -= 360f;
          while ((double) floatValue < 0.0)
            floatValue += 360f;
          if ((double) floatValue > 359.0 || (double) floatValue < 1.0)
            rotate = new PageRotate?(PageRotate.Normal);
          else if ((double) floatValue > 89.0 && (double) floatValue < 91.0)
            rotate = new PageRotate?(PageRotate.Rotate90);
          else if ((double) floatValue > 179.0 && (double) floatValue < 181.0)
            rotate = new PageRotate?(PageRotate.Rotate180);
          else if ((double) floatValue > 269.0 && (double) floatValue < 271.0)
            rotate = new PageRotate?(PageRotate.Rotate270);
          adjustRotate = !rotate.HasValue ? PdfRotateUtils.ToRotate(floatValue) : rotate.Value;
          return floatValue;
        }
        PdfTypeBase pdfTypeBase2;
        PdfTypeBase pdfTypeBase3;
        PdfTypeBase pdfTypeBase4;
        int num;
        if (annot.Dictionary.TryGetValue("AP", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeDictionary>() && pdfTypeBase2.As<PdfTypeDictionary>().TryGetValue("N", out pdfTypeBase3) && pdfTypeBase3.Is<PdfTypeStream>())
        {
          PdfTypeStream pdfTypeStream = pdfTypeBase3.As<PdfTypeStream>();
          if (pdfTypeStream != null && pdfTypeStream.Dictionary != null && pdfTypeStream.Dictionary.TryGetValue("Matrix", out pdfTypeBase4))
          {
            num = pdfTypeBase4.Is<PdfTypeArray>() ? 1 : 0;
            goto label_22;
          }
        }
        num = 0;
label_22:
        if (num != 0)
        {
          PdfTypeArray pdfTypeArray = pdfTypeBase4.As<PdfTypeArray>();
          if (pdfTypeArray.Count == 6 && pdfTypeArray.All<PdfTypeBase>((Func<PdfTypeBase, bool>) (c => c is PdfTypeNumber)))
          {
            FS_MATRIX matrix = new FS_MATRIX((PdfTypeBase) pdfTypeArray);
            if (EqualsCore(matrix, 1f, 0.0f, 0.0f, 1f))
            {
              rotate = new PageRotate?(PageRotate.Normal);
              adjustRotate = PageRotate.Normal;
              return 0.0f;
            }
            if (EqualsCore(matrix, 0.0f, 1f, -1f, 0.0f))
            {
              annot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create(90);
              rotate = new PageRotate?(PageRotate.Rotate90);
              adjustRotate = PageRotate.Rotate90;
              return 90f;
            }
            if (EqualsCore(matrix, -1f, 0.0f, 0.0f, -1f))
            {
              annot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create(180);
              rotate = new PageRotate?(PageRotate.Rotate180);
              adjustRotate = PageRotate.Rotate180;
              return 180f;
            }
            if (EqualsCore(matrix, 0.0f, -1f, 1f, 0.0f))
            {
              annot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create(270);
              rotate = new PageRotate?(PageRotate.Rotate270);
              adjustRotate = PageRotate.Rotate270;
              return 270f;
            }
          }
        }
      }
    }
    catch
    {
    }
    return 0.0f;

    static bool EqualsCore(FS_MATRIX matrix, float a, float b, float c, float d)
    {
      return EqualsCore2(matrix.a, a) && EqualsCore2(matrix.b, b) && EqualsCore2(matrix.c, c) && EqualsCore2(matrix.d, d);
    }

    static bool EqualsCore2(float left, float right)
    {
      return (double) left > (double) right - 0.01 && (double) left < (double) right + 0.01;
    }
  }

  public static PageRotate AnnotRotation(PageRotate annotRotate, PageRotate pageRotate)
  {
    int num = annotRotate - pageRotate;
    if (num < 0)
      num = 4 + num;
    return (PageRotate) num;
  }

  private static PageRotate ToRotate(float rotate)
  {
    while ((double) rotate > 360.0)
      rotate -= 360f;
    while ((double) rotate < 0.0)
      rotate += 360f;
    if ((double) rotate > 315.0 || (double) rotate <= 45.0)
      return PageRotate.Normal;
    if ((double) rotate > 45.0 && (double) rotate <= 135.0)
      return PageRotate.Rotate90;
    if ((double) rotate > 135.0 && (double) rotate <= 225.0)
      return PageRotate.Rotate180;
    return (double) rotate > 225.0 && (double) rotate <= 315.0 ? PageRotate.Rotate270 : PageRotate.Normal;
  }
}
