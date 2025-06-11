// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfDocumentPaginator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit;

internal class PdfDocumentPaginator : DocumentPaginator, IDocumentPaginatorSource
{
  private PdfDocument _doc = (PdfDocument) null;
  private DocumentPage _prevPage = (DocumentPage) null;
  private MemoryStream _mem = (MemoryStream) null;
  private bool _isValidPageCount = true;
  private int _pageCount = 0;
  private PageRange _pageRange;

  public event EventHandler<PagePrintedEventArgs> PagePrinted = null;

  public PdfDocumentPaginator(PdfDocument document, PageRange pageRange)
  {
    this._doc = document;
    this._pageCount = pageRange.PageTo - pageRange.PageFrom + 1;
    this._pageRange = pageRange;
  }

  public DocumentPaginator DocumentPaginator => (DocumentPaginator) this;

  public override bool IsPageCountValid => this._isValidPageCount;

  public override int PageCount => this._pageCount;

  public override Size PageSize { get; set; }

  public override IDocumentPaginatorSource Source => (IDocumentPaginatorSource) this;

  public PrintTicket PrinterTicket { get; set; }

  public override DocumentPage GetPage(int pageNumber)
  {
    pageNumber = pageNumber + this._pageRange.PageFrom - 1;
    if (this._prevPage != null)
      this._prevPage.Dispose();
    double width = (double) this._doc.Pages[pageNumber].Width;
    double height = (double) this._doc.Pages[pageNumber].Height;
    if (this.PageRotation(this._doc.Pages[pageNumber]) == PageRotate.Rotate270 || this.PageRotation(this._doc.Pages[pageNumber]) == PageRotate.Rotate90)
      this.PrinterTicket.PageOrientation = new PageOrientation?(PageOrientation.ReverseLandscape);
    DrawingVisual visual = new DrawingVisual();
    DocumentPage page = new DocumentPage((Visual) visual);
    page.PageDestroyed += new EventHandler(this.Page_PageDestroyed);
    this.RenderPage(pageNumber, visual);
    this._prevPage = page;
    if (this.PagePrinted != null)
    {
      PagePrintedEventArgs e = new PagePrintedEventArgs(pageNumber - this._pageRange.PageFrom + 1, this._pageCount);
      this.PagePrinted((object) this, e);
      if (e.Cancel)
        this._pageCount = 0;
    }
    return page;
  }

  private PageRotate PageRotation(PdfPage pdfPage)
  {
    int num = pdfPage.Rotation - pdfPage.OriginalRotation;
    if (num < 0)
      num = 4 + num;
    return (PageRotate) num;
  }

  private Size GetRenderSize(Size pageSize, Size fitSize)
  {
    double width1 = pageSize.Width;
    double height1 = pageSize.Height;
    double height2 = fitSize.Height;
    double width2 = width1 * height2 / height1;
    if (width2 > fitSize.Width)
    {
      width2 = fitSize.Width;
      height2 = height1 * width2 / width1;
    }
    return new Size(width2, height2);
  }

  private void Page_PageDestroyed(object sender, EventArgs e)
  {
    if (this._mem == null)
      return;
    this._mem.Close();
  }

  private void RenderPage(int pageNumber, DrawingVisual visual)
  {
    int num1 = this.PrinterTicket.PageResolution.X ?? 96 /*0x60*/;
    int num2 = this.PrinterTicket.PageResolution.Y ?? 96 /*0x60*/;
    Size fitSize = new Size()
    {
      Width = this.PageSize.Width / 96.0,
      Height = this.PageSize.Height / 96.0
    };
    Size pageSize = new Size()
    {
      Width = (double) this._doc.Pages[pageNumber].Width / 72.0,
      Height = (double) this._doc.Pages[pageNumber].Height / 72.0
    };
    if (this._doc.Pages[pageNumber].OriginalRotation == PageRotate.Rotate270 || this._doc.Pages[pageNumber].OriginalRotation == PageRotate.Rotate90)
      fitSize = new Size(fitSize.Height, fitSize.Width);
    Size renderSize = this.GetRenderSize(pageSize, fitSize);
    int width = (int) (renderSize.Width * (double) num1);
    int height = (int) (renderSize.Height * (double) num2);
    using (PdfBitmap bitmap = new PdfBitmap(width, height, true))
    {
      this._doc.Pages[pageNumber].RenderEx(bitmap, 0, 0, width, height, PageRotate.Normal, RenderFlags.FPDF_ANNOT | RenderFlags.FPDF_PRINTING);
      PdfBitmap pdfBitmap = (PdfBitmap) null;
      if (this.PageRotation(this._doc.Pages[pageNumber]) == PageRotate.Rotate270)
        pdfBitmap = bitmap.SwapXY(false, true);
      else if (this.PageRotation(this._doc.Pages[pageNumber]) == PageRotate.Rotate180)
        pdfBitmap = bitmap.FlipXY(true, true);
      else if (this.PageRotation(this._doc.Pages[pageNumber]) == PageRotate.Rotate90)
        pdfBitmap = bitmap.SwapXY(true, false);
      int num3 = pdfBitmap == null ? bitmap.Stride : pdfBitmap.Stride;
      int num4 = pdfBitmap == null ? bitmap.Width : pdfBitmap.Width;
      int num5 = pdfBitmap == null ? bitmap.Height : pdfBitmap.Height;
      IntPtr num6 = pdfBitmap == null ? bitmap.Buffer : pdfBitmap.Buffer;
      BitmapSource imageSource = this.CreateImageSource(pdfBitmap ?? bitmap);
      pdfBitmap?.Dispose();
      DrawingContext drawingContext = visual.RenderOpen();
      drawingContext.DrawImage((ImageSource) imageSource, new Rect(0.0, 0.0, (double) imageSource.PixelWidth / ((double) num1 / 96.0), imageSource.Height / ((double) num2 / 90.0)));
      drawingContext.Close();
    }
  }

  private BitmapSource CreateImageSource(PdfBitmap bmp)
  {
    this._mem = new MemoryStream();
    bmp.Image.Save((Stream) this._mem, ImageFormat.Png);
    BitmapImage imageSource = new BitmapImage();
    imageSource.BeginInit();
    imageSource.CacheOption = BitmapCacheOption.None;
    imageSource.StreamSource = (Stream) this._mem;
    imageSource.EndInit();
    imageSource.Freeze();
    return (BitmapSource) imageSource;
  }
}
