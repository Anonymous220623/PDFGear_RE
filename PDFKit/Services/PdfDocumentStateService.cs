// Decompiled with JetBrains decompiler
// Type: PDFKit.Services.PdfDocumentStateService
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using PDFKit.Utils;
using System;

#nullable disable
namespace PDFKit.Services;

public class PdfDocumentStateService
{
  public static bool CanDisposePage(PdfPage page)
  {
    if (page == null)
      return false;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(page?.Document);
    if (pdfControl == null)
      return false;
    bool res = false;
    pdfControl.Dispatcher.Invoke((Action) (() =>
    {
      if (pdfControl.IsEditing)
      {
        if (!pdfControl.Editor.CanDisposePage(page.PageIndex) || pdfControl.Editor._prPages == null)
          return;
        PRCollection prPages = pdfControl.Editor._prPages;
        lock (prPages)
          res = !prPages.ContainsKey(page);
      }
      else if (pdfControl.Viewer.CanDisposePage(page.PageIndex) && pdfControl.Viewer._prPages != null)
      {
        PRCollection prPages = pdfControl.Viewer._prPages;
        lock (prPages)
          res = !prPages.ContainsKey(page);
      }
    }));
    return res;
  }

  public static void TryRedrawViewerCurrentPage(PdfPage page)
  {
    if (page == null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(page?.Document);
    if (pdfControl == null)
      return;
    pdfControl.Dispatcher.Invoke((Action) (() =>
    {
      (int startPage2, int endPage2) = pdfControl.GetVisiblePageRange();
      if (startPage2 == -1)
        startPage2 = pdfControl.PageIndex - 5;
      if (endPage2 == -1)
        endPage2 = pdfControl.PageIndex + 5;
      if (page.PageIndex < startPage2 || page.PageIndex > endPage2)
        return;
      pdfControl.Redraw(true);
    }));
  }
}
