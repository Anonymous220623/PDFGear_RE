// Decompiled with JetBrains decompiler
// Type: PDFKit.CachedCanvasBitmap
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit;

internal class CachedCanvasBitmap : IDisposable
{
  private readonly Int32Rect[] pageRects;
  public PdfBitmap CanvasBitmap;

  public CachedCanvasBitmap(
    PdfBitmap canvasBitmap,
    IPdfScrollInfo scrollInfo,
    Int32Rect[] pageRects)
  {
    if (canvasBitmap == null)
      throw new ArgumentNullException(nameof (canvasBitmap));
    this.ScrollX = scrollInfo != null ? ((IScrollInfo) scrollInfo).HorizontalOffset : throw new ArgumentNullException(nameof (scrollInfo));
    this.ScrollY = ((IScrollInfo) scrollInfo).VerticalOffset;
    this.ViewportWidth = ((IScrollInfo) scrollInfo).ViewportWidth;
    this.ViewportHeight = ((IScrollInfo) scrollInfo).ViewportHeight;
    this.Zoom = (double) scrollInfo.Zoom;
    this.pageRects = pageRects;
    if (pageRects != null && pageRects.Length != 0)
    {
      this.CanvasBitmap = new PdfBitmap(canvasBitmap.Width, canvasBitmap.Height, true);
      for (int index = 0; index < pageRects.Length; ++index)
      {
        Int32Rect pageRect = pageRects[index];
        Pdfium.FPDFBitmap_CompositeBitmap(this.CanvasBitmap.Handle, pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height, canvasBitmap.Handle, pageRect.X, pageRect.Y);
      }
    }
    else
      this.CanvasBitmap = canvasBitmap.Clone();
  }

  public double ScrollX { get; }

  public double ScrollY { get; }

  public double ViewportWidth { get; }

  public double ViewportHeight { get; }

  public double Zoom { get; }

  public void WriteToDeviceBitmap(
    WriteableBitmap bitmap,
    PdfBitmap formsBitmap,
    IPdfScrollInfo scrollInfo)
  {
    if (bitmap == null)
      throw new ArgumentNullException(nameof (bitmap));
    double num = scrollInfo != null ? ((IScrollInfo) scrollInfo).HorizontalOffset : throw new ArgumentNullException(nameof (scrollInfo));
    double verticalOffset = ((IScrollInfo) scrollInfo).VerticalOffset;
    int pixels1 = Helpers.UnitsToPixels((DependencyObject) scrollInfo, this.ScrollX - num);
    int pixels2 = Helpers.UnitsToPixels((DependencyObject) scrollInfo, this.ScrollY - verticalOffset);
    if (this.pageRects != null)
    {
      using (PdfBitmap pdfBitmap = new PdfBitmap(this.CanvasBitmap.Width, this.CanvasBitmap.Height, true))
      {
        if (formsBitmap != null)
          Pdfium.FPDFBitmap_CompositeBitmap(pdfBitmap.Handle, 0, 0, formsBitmap.Width, formsBitmap.Height, formsBitmap.Handle, 0, 0);
        Pdfium.FPDFBitmap_CompositeBitmap(pdfBitmap.Handle, pixels1, pixels2, this.CanvasBitmap.Width, this.CanvasBitmap.Height, this.CanvasBitmap.Handle, 0, 0);
        for (int index = 0; index < 3; ++index)
        {
          try
          {
            bitmap.WritePixels(new Int32Rect(0, 0, pdfBitmap.Width, pdfBitmap.Height), pdfBitmap.Buffer, pdfBitmap.Stride * pdfBitmap.Height, pdfBitmap.Stride);
            break;
          }
          catch
          {
          }
        }
      }
    }
    else
    {
      if (formsBitmap == null)
        return;
      bitmap.WritePixels(new Int32Rect(0, 0, formsBitmap.Width, formsBitmap.Height), formsBitmap.Buffer, formsBitmap.Stride * formsBitmap.Height, formsBitmap.Stride, 0, 0);
    }
  }

  public void Dispose()
  {
    this.CanvasBitmap?.Dispose();
    this.CanvasBitmap = (PdfBitmap) null;
  }
}
