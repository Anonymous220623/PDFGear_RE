// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfPrintDocument
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Properties;
using PDFKit.Utils;
using System;
using System.Drawing;
using System.Drawing.Printing;

#nullable disable
namespace PDFKit;

public class PdfPrintDocument : PrintDocument
{
  private PdfDocument _pdfDoc;
  private bool _isAnnotationVisible = true;
  private int _pageForPrint;
  private IntPtr _printHandle;
  private IntPtr _docForPrint;
  private bool _useDP;
  private int _scale = 100;
  public static bool IsCancel;

  public event EventHandler<BeforeRenderPageEventArgs> BeforeRenderPage;

  public event EventHandler<BeforeRenderPageEventArgs> AfterRenderPage;

  public bool AutoRotate { get; set; }

  public bool AutoCenter { get; set; }

  public PdfDocument Document
  {
    get => this._pdfDoc;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Document));
      if (this._pdfDoc == value)
        return;
      this._pdfDoc = value;
      this.PrinterSettings.MinimumPage = 1;
      this.PrinterSettings.MaximumPage = this._pdfDoc.Pages.Count;
      this.PrinterSettings.FromPage = this.PrinterSettings.MinimumPage;
      this.PrinterSettings.ToPage = this.PrinterSettings.MaximumPage;
    }
  }

  public PrintSizeMode PrintSizeMode { get; set; }

  public int Scale
  {
    get => this._scale;
    set
    {
      if (value == this._scale)
        return;
      this._scale = value >= 1 && value <= 1000 ? value : throw new ArgumentOutOfRangeException("Value", (object) value, string.Format(Resources.err0003, (object) 1, (object) 1000));
      this.PrintSizeMode = PrintSizeMode.CustomScale;
    }
  }

  public RenderFlags RenderFlags { get; set; }

  public PdfPrintDocument()
  {
    this._useDP = false;
    this.AutoRotate = true;
    this.AutoCenter = false;
    this.RenderFlags = RenderFlags.FPDF_ANNOT | RenderFlags.FPDF_PRINTING;
    PdfPrintDocument.IsCancel = false;
    this.PrinterSettings.MinimumPage = 1;
    this.PrinterSettings.MaximumPage = 1;
    this.PrinterSettings.FromPage = this.PrinterSettings.MinimumPage;
    this.PrinterSettings.ToPage = this.PrinterSettings.MaximumPage;
  }

  public PdfPrintDocument(PdfDocument Document, int mode = 0, bool isAnnotationVisible = true)
  {
    if (Document == null)
      throw new ArgumentNullException(nameof (Document));
    PdfPrintDocument.IsCancel = false;
    this._pdfDoc = Document;
    this._isAnnotationVisible = isAnnotationVisible;
    this._useDP = mode == 1;
    this.AutoRotate = true;
    this.AutoCenter = false;
    this.RenderFlags = RenderFlags.FPDF_ANNOT | RenderFlags.FPDF_PRINTING;
    this.PrinterSettings.MinimumPage = 1;
    this.PrinterSettings.MaximumPage = this._pdfDoc.Pages.Count;
    this.PrinterSettings.FromPage = this.PrinterSettings.MinimumPage;
    this.PrinterSettings.ToPage = this.PrinterSettings.MaximumPage;
  }

  protected virtual void OnBeforeRenderPage(
    Graphics g,
    PdfPage page,
    ref int x,
    ref int y,
    ref int width,
    ref int height,
    PageRotate rotation)
  {
    if (this.BeforeRenderPage == null)
      return;
    BeforeRenderPageEventArgs e = new BeforeRenderPageEventArgs(g, page, x, y, width, height, rotation);
    this.BeforeRenderPage((object) this, e);
    x = e.X;
    y = e.Y;
    width = e.Width;
    height = e.Height;
  }

  protected virtual void OnAfterRenderPage(
    Graphics g,
    PdfPage page,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotation)
  {
    if (this.AfterRenderPage == null)
      return;
    this.AfterRenderPage((object) this, new BeforeRenderPageEventArgs(g, page, x, y, width, height, rotation));
  }

  protected override void OnBeginPrint(PrintEventArgs e)
  {
    base.OnBeginPrint(e);
    if (this._pdfDoc == null)
      throw new ArgumentNullException("Document");
    switch (this.PrinterSettings.PrintRange)
    {
      case PrintRange.Selection:
      case PrintRange.CurrentPage:
        this.PrinterSettings.FromPage = this._pdfDoc.Pages.CurrentIndex + 1;
        this.PrinterSettings.ToPage = this._pdfDoc.Pages.CurrentIndex + 1;
        goto case PrintRange.SomePages;
      case PrintRange.SomePages:
        this._docForPrint = this.InitDocument();
        if (this._docForPrint == IntPtr.Zero)
        {
          e.Cancel = true;
          break;
        }
        this._pageForPrint = this._useDP ? 0 : this.PrinterSettings.FromPage - 1;
        break;
      default:
        this.PrinterSettings.FromPage = this.PrinterSettings.MinimumPage;
        this.PrinterSettings.ToPage = this.PrinterSettings.MaximumPage;
        goto case PrintRange.SomePages;
    }
  }

  protected override void OnEndPrint(PrintEventArgs e)
  {
    base.OnEndPrint(e);
    if (this._printHandle != IntPtr.Zero)
      Pdfium.FPDFPRINT_Close(this._printHandle);
    this._printHandle = IntPtr.Zero;
  }

  protected override void OnQueryPageSettings(QueryPageSettingsEventArgs e)
  {
    if (this.AutoRotate)
    {
      IntPtr page = Pdfium.FPDF_StartLoadPage(this._docForPrint, this._pageForPrint);
      if (page == IntPtr.Zero)
      {
        e.Cancel = true;
        return;
      }
      double pageWidth = Pdfium.FPDF_GetPageWidth(page);
      double pageHeight = Pdfium.FPDF_GetPageHeight(page);
      Pdfium.FPDFPage_GetRotation(page);
      bool flag = pageWidth > pageHeight;
      e.PageSettings.Landscape = flag;
      if (page != IntPtr.Zero)
        Pdfium.FPDF_ClosePage(page);
    }
    base.OnQueryPageSettings(e);
  }

  protected override void OnPrintPage(PrintPageEventArgs e)
  {
    base.OnPrintPage(e);
    if (this._pdfDoc == null)
      throw new ArgumentNullException("Document");
    IntPtr hdc = IntPtr.Zero;
    IntPtr num = IntPtr.Zero;
    try
    {
      if (PdfPrintDocument.IsCancel)
      {
        GAManager.SendEvent(nameof (PdfPrintDocument), "PrintIsCancelled", "Count", 1L);
        e.Cancel = true;
      }
      if (e.Cancel)
        return;
      num = Pdfium.FPDF_LoadPage(this._docForPrint, this._pageForPrint);
      if (num == IntPtr.Zero)
      {
        e.Cancel = true;
      }
      else
      {
        double dpiX = (double) e.Graphics.DpiX;
        double dpiY = (double) e.Graphics.DpiY;
        double width1 = Pdfium.FPDF_GetPageWidth(num) / 72.0 * dpiX;
        double height1 = Pdfium.FPDF_GetPageHeight(num) / 72.0 * dpiY;
        double x1;
        double y1;
        Rectangle clipRect;
        this.CalcSize(dpiX, dpiY, e.PageSettings.PrintableArea, e.MarginBounds, new PointF(e.PageSettings.HardMarginX, e.PageSettings.HardMarginY), e.PageSettings.Landscape, ref width1, ref height1, out x1, out y1, out clipRect);
        int x2 = (int) x1;
        int y2 = (int) y1;
        int width2 = (int) width1;
        int height2 = (int) height1;
        PdfRenderHelper.AnnotationFlagContext annotationFlagContext = new PdfRenderHelper.AnnotationFlagContext();
        try
        {
          using (PdfPage page = PdfPage.FromHandle(this._pdfDoc, num, this._pageForPrint))
          {
            this.OnBeforeRenderPage(e.Graphics, page, ref x2, ref y2, ref width2, ref height2, PageRotate.Normal);
            if (!this._isAnnotationVisible)
              annotationFlagContext = (PdfRenderHelper.AnnotationFlagContext) PdfRenderHelper.CreateHideFlagContext(page, this._isAnnotationVisible) with
              {
                Page = (PdfPage) null
              };
          }
          hdc = e.Graphics.GetHdc();
          if (this.OriginAtMargins)
            Pdfium.IntersectClipRect(hdc, clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom);
          Pdfium.FPDF_RenderPage(hdc, num, x2, y2, width2, height2, PageRotate.Normal, this.RenderFlags);
          if (hdc != IntPtr.Zero)
            e.Graphics.ReleaseHdc(hdc);
          hdc = IntPtr.Zero;
          using (PdfPage page = PdfPage.FromHandle(this._pdfDoc, num, this._pageForPrint))
          {
            this.OnAfterRenderPage(e.Graphics, page, x2, y2, width2, height2, PageRotate.Normal);
            if (!this._isAnnotationVisible)
            {
              annotationFlagContext.Page = page;
              annotationFlagContext.Dispose();
              annotationFlagContext = new PdfRenderHelper.AnnotationFlagContext();
            }
          }
        }
        finally
        {
          annotationFlagContext.Dispose();
        }
        if (this._pageForPrint >= this.PrinterSettings.ToPage - (this._useDP ? this.PrinterSettings.FromPage : 1))
          return;
        ++this._pageForPrint;
        e.HasMorePages = true;
      }
    }
    finally
    {
      if (hdc != IntPtr.Zero)
        e.Graphics.ReleaseHdc(hdc);
      if (num != IntPtr.Zero)
        Pdfium.FPDF_ClosePage(num);
    }
  }

  private IntPtr InitDocument()
  {
    if (!this._useDP)
      return this._pdfDoc.Handle;
    IntPtr handle = this._pdfDoc.Handle;
    string pageRange = $"{this.PrinterSettings.FromPage}-{this.PrinterSettings.ToPage}";
    int paperWidth = this.DefaultPageSettings.PaperSize.Width / 100 * 72;
    int paperHeight = this.DefaultPageSettings.PaperSize.Height / 100 * 72;
    RectangleF printableArea = this.DefaultPageSettings.PrintableArea;
    int printableAreaLeft = (int) ((double) printableArea.X / 100.0 * 72.0);
    printableArea = this.DefaultPageSettings.PrintableArea;
    int printableAreaTop = (int) ((double) printableArea.Y / 100.0 * 72.0);
    printableArea = this.DefaultPageSettings.PrintableArea;
    int printableAreaRight = (int) ((double) printableArea.Width / 100.0 * 72.0);
    printableArea = this.DefaultPageSettings.PrintableArea;
    int printableAreaBottom = (int) ((double) printableArea.Height / 100.0 * 72.0);
    this._printHandle = Pdfium.FPDFPRINT_Open(handle, pageRange, paperWidth, paperHeight, printableAreaLeft, printableAreaTop, printableAreaRight, printableAreaBottom, PrintScallingMode.PrintableArea);
    if (this._printHandle == IntPtr.Zero)
      return IntPtr.Zero;
    this._docForPrint = Pdfium.FPDFPRINT_GetDocument(this._printHandle);
    return this._docForPrint == IntPtr.Zero ? IntPtr.Zero : this._docForPrint;
  }

  private void CalcSize(
    double dpiX,
    double dpiY,
    RectangleF printableArea,
    Rectangle marginBounds,
    PointF hardMargin,
    bool isLandscape,
    ref double width,
    ref double height,
    out double x,
    out double y,
    out Rectangle clipRect)
  {
    x = y = 0.0;
    clipRect = Rectangle.Empty;
    if (this._useDP)
      return;
    RectangleF rectangleF = !this.OriginAtMargins ? new RectangleF(printableArea.X, printableArea.Y, printableArea.Width, printableArea.Height) : new RectangleF((float) marginBounds.X, (float) marginBounds.Y, (float) marginBounds.Width, (float) marginBounds.Height);
    if (isLandscape)
      rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Height, rectangleF.Width);
    SizeF fitSize = new SizeF((float) (dpiX * (double) rectangleF.Width / 100.0), (float) (dpiY * (double) rectangleF.Height / 100.0));
    SizeF pageSize = new SizeF((float) width, (float) height);
    if (this.OriginAtMargins & isLandscape)
      fitSize = new SizeF(fitSize.Height, fitSize.Width);
    switch (this.PrintSizeMode)
    {
      case PrintSizeMode.Fit:
        SizeF renderSize = this.GetRenderSize(pageSize, fitSize);
        width = (double) renderSize.Width;
        height = (double) renderSize.Height;
        break;
      case PrintSizeMode.CustomScale:
        width *= (double) this.Scale / 100.0;
        height *= (double) this.Scale / 100.0;
        break;
    }
    x = (double) rectangleF.X * dpiX / 100.0 - (double) hardMargin.X * dpiX / 100.0;
    y = (double) rectangleF.Y * dpiY / 100.0 - (double) hardMargin.Y * dpiY / 100.0;
    if (this.AutoCenter)
    {
      x += ((double) fitSize.Width - width) / 2.0;
      y += ((double) fitSize.Height - height) / 2.0;
    }
    clipRect = new Rectangle((int) ((double) marginBounds.Left * dpiX / 100.0), (int) ((double) marginBounds.Top * dpiY / 100.0), (int) ((double) marginBounds.Width * dpiX / 100.0), (int) ((double) marginBounds.Height * dpiX / 100.0));
  }

  private SizeF GetRenderSize(SizeF pageSize, SizeF fitSize)
  {
    double width1 = (double) pageSize.Width;
    double height1 = (double) pageSize.Height;
    double height2 = (double) fitSize.Height;
    double width2 = width1 * height2 / height1;
    if (width2 > (double) fitSize.Width)
    {
      width2 = (double) fitSize.Width;
      height2 = height1 * width2 / width1;
    }
    return new SizeF((float) width2, (float) height2);
  }
}
