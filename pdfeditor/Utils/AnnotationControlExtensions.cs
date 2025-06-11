// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationControlExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Controls.Annotations;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Models.Annotations;
using PDFKit;
using PDFKit.Utils;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

public static class AnnotationControlExtensions
{
  public static Point? PageToDevice(this IAnnotationControl annotControl, Point point)
  {
    PdfViewer pdfViewer = annotControl?.ParentCanvas?.PdfViewer;
    PdfPage page = annotControl?.Annotation?.Page;
    if (pdfViewer == null || page == null)
      return new Point?();
    Point clientPoint;
    return pdfViewer.TryGetClientPoint(page.PageIndex, point, out clientPoint) ? new Point?(clientPoint) : new Point?();
  }

  public static Point? PageToDevice(this IAnnotationControl annotControl, double x, double y)
  {
    return annotControl.PageToDevice(new Point(x, y));
  }

  public static Point? DeviceToPage(this IAnnotationControl annotControl, Point point)
  {
    PdfViewer pdfViewer = annotControl?.ParentCanvas?.PdfViewer;
    PdfPage page = annotControl?.Annotation?.Page;
    if (pdfViewer == null || page == null)
      return new Point?();
    Point pagePoint;
    return pdfViewer.TryGetPagePoint(page.PageIndex, point, out pagePoint) ? new Point?(pagePoint) : new Point?();
  }

  public static Point? DeviceToPage(this IAnnotationControl annotControl, double x, double y)
  {
    return annotControl.DeviceToPage(new Point(x, y));
  }

  public static void TryRedrawAnnotation(this PdfMarkupAnnotation annot, bool removeEditor = false)
  {
    if (annot?.Page == null)
      return;
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(annot.Page.Document)?.Viewer;
    if (removeEditor && viewer != null)
    {
      AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
      int? pageIndex1 = annotationHolderManager?.CurrentHolder?.CurrentPage?.PageIndex;
      int pageIndex2 = annot.Page.PageIndex;
      if (pageIndex1.GetValueOrDefault() == pageIndex2 & pageIndex1.HasValue)
        annotationHolderManager.CurrentHolder.Cancel();
    }
    lock (annot)
    {
      if (AnnotationControlExtensions.TryRedrawAnnotationCore(annot, viewer))
        return;
      try
      {
        AnnotationFactory.Create((PdfAnnotation) annot).Apply((PdfAnnotation) annot);
        AnnotationControlExtensions.TryRedrawAnnotationCore(annot, viewer);
      }
      catch
      {
      }
    }
  }

  private static bool TryRedrawAnnotationCore(PdfMarkupAnnotation annot, PdfViewer viewer)
  {
    try
    {
      annot.Dispose();
      switch (annot)
      {
        case PdfHighlightAnnotation highlight:
          highlight.RegenerateAppearancesWithoutRound();
          break;
        case PdfFreeTextAnnotation annot1:
          annot1.RegenerateAppearancesWithRichText();
          break;
        case PdfStampAnnotation annot2:
          annot2.RegenerateAppearancesAdvance();
          break;
        case PdfTextAnnotation text:
          text.RegenerateAppearancesAdvance();
          break;
        default:
          annot.RegenerateAppearances();
          break;
      }
      if (!(annot is PdfFreeTextAnnotation) && viewer != null)
        viewer.InvalidateVisual();
      return true;
    }
    catch
    {
    }
    return false;
  }
}
