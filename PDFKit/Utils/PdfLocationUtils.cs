// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfLocationUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace PDFKit.Utils;

public static class PdfLocationUtils
{
  public static FS_RECTF GetEffectiveBox(this PdfPage page, PageRotate rotate = PageRotate.Normal, bool userUnitAware = false)
  {
    return PdfLocationUtils.GetEffectiveBox(page.Document.Handle, page.Handle, page.PageIndex, rotate, userUnitAware);
  }

  public static FS_RECTF GetEffectiveBox(
    IntPtr documentHandle,
    IntPtr pageHandle,
    int pageIndex,
    PageRotate rotate = PageRotate.Normal,
    bool userUnitAware = false)
  {
    using (PdfTypeDictionary pageDictionary = PdfLocationUtils.GetPageDictionary(documentHandle, pageHandle, pageIndex))
    {
      (float left, float num1, float right, float num2) = PdfLocationUtils.GetEffectiveBoxCore(pageHandle, pageDictionary, rotate);
      PdfTypeBase pdfTypeBase;
      if (!userUnitAware && pageDictionary != null && pageDictionary.TryGetValue("UserUnit", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeNumber>())
      {
        float floatValue = pdfTypeBase.As<PdfTypeNumber>().FloatValue;
        left *= floatValue;
        num1 *= floatValue;
        right *= floatValue;
        num2 *= floatValue;
      }
      return PdfLocationUtils.NormalizeRect(left, num1, right, num2);
    }
  }

  public static FS_SIZEF GetEffectiveSize(this PdfPage page, PageRotate rotate = PageRotate.Normal, bool userUnitAware = false)
  {
    return PdfLocationUtils.GetEffectiveSize(page.Document.Handle, page.Handle, page.PageIndex, rotate, userUnitAware);
  }

  public static FS_SIZEF GetEffectiveSize(
    IntPtr documentHandle,
    IntPtr pageHandle,
    int pageIndex,
    PageRotate rotate = PageRotate.Normal,
    bool userUnitAware = false)
  {
    using (PdfTypeDictionary pageDictionary = PdfLocationUtils.GetPageDictionary(documentHandle, pageHandle, pageIndex))
    {
      FS_RECTF effectiveBoxCore = PdfLocationUtils.GetEffectiveBoxCore(pageHandle, pageDictionary, rotate);
      float width = Math.Abs(effectiveBoxCore.Width);
      float height = Math.Abs(effectiveBoxCore.Height);
      PdfTypeBase pdfTypeBase;
      if (!userUnitAware && pageDictionary != null && pageDictionary.TryGetValue("UserUnit", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeNumber>())
      {
        float floatValue = pdfTypeBase.As<PdfTypeNumber>().FloatValue;
        width *= floatValue;
        height *= floatValue;
      }
      return new FS_SIZEF(width, height);
    }
  }

  public static FS_POINTF EffectiveBoxPositionToMediaBox(PdfPage page, FS_POINTF point)
  {
    return PdfLocationUtils.EffectiveBoxPositionToMediaBox(page.Handle, page.Dictionary, point);
  }

  public static FS_POINTF EffectiveBoxPositionToMediaBox(
    IntPtr pageHandle,
    PdfTypeDictionary pageDict,
    FS_POINTF point)
  {
    FS_RECTF trimBox;
    return PdfLocationUtils.TryGetEffectiveBox(pageHandle, pageDict, out trimBox) ? new FS_POINTF(point.X + trimBox.left, point.Y + trimBox.bottom) : point;
  }

  public static FS_POINTF MediaBoxPositionToEffectiveBox(PdfPage page, FS_POINTF point)
  {
    return PdfLocationUtils.MediaBoxPositionToEffectiveBox(page.Handle, page.Dictionary, point);
  }

  public static FS_POINTF MediaBoxPositionToEffectiveBox(
    IntPtr pageHandle,
    PdfTypeDictionary pageDict,
    FS_POINTF point)
  {
    FS_RECTF trimBox;
    return PdfLocationUtils.TryGetEffectiveBox(pageHandle, pageDict, out trimBox) ? new FS_POINTF(point.X - trimBox.left, point.Y - trimBox.bottom) : point;
  }

  public static FS_RECTF EffectiveBoxRectToMediaBox(PdfPage page, FS_RECTF rect)
  {
    FS_POINTF mediaBox1 = PdfLocationUtils.EffectiveBoxPositionToMediaBox(page, new FS_POINTF(rect.left, rect.top));
    FS_POINTF mediaBox2 = PdfLocationUtils.EffectiveBoxPositionToMediaBox(page, new FS_POINTF(rect.right, rect.bottom));
    return PdfLocationUtils.NormalizeRect(mediaBox1.X, mediaBox1.Y, mediaBox2.X, mediaBox2.Y);
  }

  public static FS_RECTF MediaBoxRectToEffectiveBox(PdfPage page, FS_RECTF rect)
  {
    FS_POINTF effectiveBox1 = PdfLocationUtils.MediaBoxPositionToEffectiveBox(page, new FS_POINTF(rect.left, rect.top));
    FS_POINTF effectiveBox2 = PdfLocationUtils.MediaBoxPositionToEffectiveBox(page, new FS_POINTF(rect.right, rect.bottom));
    return PdfLocationUtils.NormalizeRect(effectiveBox1.X, effectiveBox1.Y, effectiveBox2.X, effectiveBox2.Y);
  }

  public static void Deconstruct(this FS_SIZEF size, out float width, out float height)
  {
    width = size.Width;
    height = size.Height;
  }

  public static void Deconstruct(
    this FS_RECTF rect,
    out float left,
    out float top,
    out float right,
    out float bottom)
  {
    left = rect.left;
    top = rect.top;
    right = rect.right;
    bottom = rect.bottom;
  }

  private static FS_RECTF GetEffectiveBoxCore(
    IntPtr pageHandle,
    PdfTypeDictionary pageDict,
    PageRotate rotate)
  {
    FS_RECTF mediaBox = PdfLocationUtils.GetMediaBox(pageHandle);
    FS_RECTF trimBox;
    return PdfLocationUtils.TryGetEffectiveBox(pageHandle, pageDict, out trimBox) ? PdfLocationUtils.RotateEffectiveBox(trimBox, mediaBox, rotate) : PdfLocationUtils.RotateEffectiveBox(mediaBox, mediaBox, rotate);
  }

  private static FS_RECTF RotateEffectiveBox(
    FS_RECTF trimBox,
    FS_RECTF mediaBox,
    PageRotate rotate)
  {
    if (rotate == PageRotate.Normal)
      return trimBox;
    float left = trimBox.left;
    float num1 = mediaBox.top - trimBox.top;
    float num2 = mediaBox.right - mediaBox.right;
    float bottom = trimBox.bottom;
    if (rotate == PageRotate.Rotate90)
      return PdfLocationUtils.NormalizeRect(bottom, mediaBox.Width - left, mediaBox.Height - num1, num2);
    if (rotate == PageRotate.Rotate180)
      return PdfLocationUtils.NormalizeRect(num2, mediaBox.Height - bottom, mediaBox.Width - left, num1);
    if (rotate == PageRotate.Rotate270)
      return PdfLocationUtils.NormalizeRect(num1, mediaBox.Width - num2, mediaBox.Height - bottom, left);
    throw new ArgumentException(nameof (rotate));
  }

  private static bool TryGetEffectiveBox(
    IntPtr pageHandle,
    PdfTypeDictionary pageDict,
    out FS_RECTF trimBox)
  {
    trimBox = new FS_RECTF();
    float left1;
    float bottom1;
    float right1;
    float top1;
    if (pageDict.ContainsKey("TrimBox") && Pdfium.FPDFPage_GetTrimBox(pageHandle, out left1, out bottom1, out right1, out top1) && (double) left1 != 0.0 && (double) bottom1 != 0.0 && (double) right1 != 0.0 && (double) top1 != 0.0)
    {
      trimBox = PdfLocationUtils.NormalizeRect(left1, top1, right1, bottom1);
      return true;
    }
    float left2;
    float bottom2;
    float right2;
    float top2;
    if (!pageDict.ContainsKey("CropBox") || !Pdfium.FPDFPage_GetCropBox(pageHandle, out left2, out bottom2, out right2, out top2))
      return false;
    trimBox = PdfLocationUtils.NormalizeRect(left2, top2, right2, bottom2);
    return true;
  }

  private static FS_RECTF GetMediaBox(IntPtr pageHandle)
  {
    float left;
    float bottom;
    float right;
    float top;
    if (!Pdfium.FPDFPage_GetMediaBox(pageHandle, out left, out bottom, out right, out top))
      throw new ArgumentException(nameof (pageHandle));
    return PdfLocationUtils.NormalizeRect(left, bottom, right, top);
  }

  private static PdfTypeDictionary GetPageDictionary(
    IntPtr documentHandle,
    IntPtr pageHandle,
    int pageIndex)
  {
    try
    {
      if (pageHandle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr pageDictionary = Pdfium.FPDF_GetPageDictionary(documentHandle, pageIndex);
      return pageDictionary == IntPtr.Zero ? (PdfTypeDictionary) null : new PdfTypeDictionary(pageDictionary);
    }
    catch
    {
    }
    return (PdfTypeDictionary) null;
  }

  public static void SetPageCropBox(this PdfPage page, FS_RECTF boxSize)
  {
    try
    {
      Pdfium.FPDFPage_SetCropBox(page.Handle, boxSize.left, boxSize.bottom, boxSize.right, boxSize.top);
      Pdfium.FPDFPage_SetTrimBox(page.Handle, boxSize.left, boxSize.bottom, boxSize.right, boxSize.top);
    }
    catch
    {
    }
  }

  private static FS_RECTF NormalizeRect(float left, float bottom, float right, float top)
  {
    return new FS_RECTF(Math.Min(left, right), Math.Max(top, bottom), Math.Max(left, right), Math.Min(top, bottom));
  }
}
