// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.Extensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.Collections;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

public static class Extensions
{
  public static FS_RECTF GetRECT(this PdfAnnotation annotation)
  {
    if ((PdfWrapper) annotation == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (annotation));
    try
    {
      PdfTypeBase array;
      if (annotation.Dictionary.TryGetValue("Rect", out array) && array.Is<PdfTypeArray>())
        return new FS_RECTF(array);
    }
    catch
    {
    }
    try
    {
      if (annotation.NormalAppearance != null && annotation.NormalAppearance.Count > 0)
      {
        FS_RECTF rect = AnnotDrawingHelper.CalcBBox((IEnumerable) annotation.NormalAppearance);
        annotation.Rectangle = rect;
        return rect;
      }
    }
    catch
    {
    }
    return new FS_RECTF();
  }

  public static Rect GetDeviceBounds(this PdfAnnotation annotation)
  {
    if (annotation?.Page?.Document == null)
      return new Rect();
    PdfControl pdfControl = PdfControl.GetPdfControl(annotation.Page.Document);
    return pdfControl == null ? new Rect() : Extensions.GetDeviceBounds(pdfControl.Viewer, annotation);
  }

  public static bool TrySetBounds(this PdfAnnotation annotation, Rect deviceBounds)
  {
    if (annotation?.Page?.Document == null)
      return false;
    PdfControl pdfControl = PdfControl.GetPdfControl(annotation.Page.Document);
    if (pdfControl == null)
      return false;
    FS_RECTF? pageBounds = Extensions.GetPageBounds(pdfControl.Viewer, annotation, deviceBounds);
    if (pageBounds.HasValue)
      annotation.Rectangle = pageBounds.Value;
    return false;
  }

  internal static Rect GetDeviceBounds(PdfViewer viewer, PdfAnnotation annotation)
  {
    if (viewer == null || annotation?.Page == null)
      return new Rect();
    if (!annotation.Dictionary.ContainsKey("Rect") || !annotation.Dictionary["Rect"].Is<PdfTypeArray>() || annotation.Dictionary["Rect"].As<PdfTypeArray>().Count == 0)
      return new Rect();
    FS_RECTF rect = annotation.GetRECT();
    Rect clientRect1;
    if (annotation.Flags.HasFlag((Enum) AnnotationFlags.NoZoom))
    {
      int dpi = Helpers.GetDpi((DependencyObject) viewer);
      double width = (double) Math.Abs(rect.Width) * 96.0 / (double) dpi;
      double height = (double) Math.Abs(rect.Height) * 96.0 / (double) dpi;
      Rect clientRect2;
      if (!viewer.TryGetClientRect(annotation.Page.PageIndex, rect, out clientRect2))
        return new Rect();
      switch (annotation.Flags.HasFlag((Enum) AnnotationFlags.NoRotate) ? (int) annotation.Page.Rotation : 0)
      {
        case 1:
          clientRect1 = new Rect(clientRect2.Left + width, clientRect2.Bottom - height, width, height);
          break;
        case 2:
          clientRect1 = new Rect(clientRect2.Left + width, clientRect2.Bottom, width, height);
          break;
        case 3:
          clientRect1 = new Rect(clientRect2.Left, clientRect2.Bottom, width, height);
          break;
        default:
          clientRect1 = new Rect(clientRect2.Left, clientRect2.Bottom - height, width, height);
          break;
      }
    }
    else if (!viewer.TryGetClientRect(annotation.Page.PageIndex, rect, out clientRect1))
      return new Rect();
    return clientRect1;
  }

  internal static FS_RECTF? GetPageBounds(
    PdfViewer viewer,
    PdfAnnotation annotation,
    Rect deviceBounds)
  {
    if (viewer == null || annotation?.Page == null || deviceBounds.IsEmpty)
      return new FS_RECTF?();
    if (annotation.Flags.HasFlag((Enum) AnnotationFlags.NoZoom))
    {
      int dpi = Helpers.GetDpi((DependencyObject) viewer);
      double num1 = deviceBounds.Width / 96.0 * (double) dpi;
      double num2 = deviceBounds.Height / 96.0 * (double) dpi;
      FS_RECTF pageRect;
      if (!viewer.TryGetPageRect(annotation.Page.PageIndex, deviceBounds, out pageRect))
        return new FS_RECTF?();
      PageRotate pageRotate = annotation.Flags.HasFlag((Enum) AnnotationFlags.NoRotate) ? annotation.Page.Rotation : PageRotate.Normal;
      FS_RECTF fsRectf = new FS_RECTF();
      switch (pageRotate)
      {
        case PageRotate.Rotate90:
          fsRectf = new FS_RECTF((double) pageRect.right - num1, (double) pageRect.bottom + num2 - (double) pageRect.Height, (double) pageRect.right, (double) pageRect.bottom - (double) pageRect.Height);
          break;
        case PageRotate.Rotate180:
          fsRectf = new FS_RECTF((double) pageRect.right - num1 + (double) pageRect.Width, (double) pageRect.bottom, (double) pageRect.right + (double) pageRect.Width, (double) pageRect.bottom - num2);
          break;
        case PageRotate.Rotate270:
          fsRectf = new FS_RECTF((double) pageRect.right, (double) pageRect.top, (double) pageRect.right + num1, (double) pageRect.top - num2);
          break;
        default:
          fsRectf = new FS_RECTF((double) pageRect.left, (double) pageRect.bottom + num2, (double) pageRect.left + num1, (double) pageRect.bottom);
          break;
      }
      return new FS_RECTF?(fsRectf);
    }
    FS_RECTF pageRect1;
    return !viewer.TryGetPageRect(annotation.Page.PageIndex, deviceBounds, out pageRect1) ? new FS_RECTF?() : new FS_RECTF?(pageRect1);
  }
}
