// Decompiled with JetBrains decompiler
// Type: PDFKit.PRCollection
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit;

internal class PRCollection : Dictionary<PdfPage, PRItem>
{
  private PdfBitmap _canvasBitmap = (PdfBitmap) null;
  private int _waitTime;
  private long _prevTicks;

  internal event EventHandler<EventArgs<Int32Rect>> PaintBackground = null;

  public PdfBitmap CanvasBitmap => this._canvasBitmap;

  public Helpers.Int32Size CanvasSize { get; private set; }

  public void InitCanvas(Helpers.Int32Size size)
  {
    if (this._canvasBitmap == null)
    {
      this._canvasBitmap = new PdfBitmap(size.Width, size.Height, false);
      this.CanvasSize = size;
    }
    this._waitTime = 70;
    this._prevTicks = DateTime.Now.Ticks;
  }

  public void ReleaseCanvas()
  {
    foreach (KeyValuePair<PdfPage, PRItem> keyValuePair in (Dictionary<PdfPage, PRItem>) this)
      this.ReleasePage(keyValuePair.Key);
    this.Clear();
    if (this._canvasBitmap != null)
      this._canvasBitmap.Dispose();
    this._canvasBitmap = (PdfBitmap) null;
  }

  public bool IsNeedContinuePaint
  {
    get
    {
      foreach (KeyValuePair<PdfPage, PRItem> keyValuePair in (Dictionary<PdfPage, PRItem>) this)
      {
        if (keyValuePair.Value.status == ProgressiveStatus.ToBeContinued || keyValuePair.Value.status == ProgressiveStatus.Ready)
          return true;
      }
      return false;
    }
  }

  internal bool IsNeedPause(PdfPage page)
  {
    return this.ContainsKey(page) && TimeSpan.FromTicks(DateTime.Now.Ticks - this._prevTicks).Milliseconds > this._waitTime;
  }

  internal bool RenderPage(
    PdfPage page,
    Int32Rect pageRect,
    PageRotate pageRotate,
    RenderFlags renderFlags,
    bool useProgressiveRender)
  {
    if (!this.ContainsKey(page))
    {
      this.ProcessNew(page);
      if (this.PaintBackground != null)
        this.PaintBackground((object) this, new EventArgs<Int32Rect>(pageRect));
    }
    if ((renderFlags & RenderFlags.FPDF_THUMBNAIL) != 0)
      this[page].status = (ProgressiveStatus) 6;
    else if (!useProgressiveRender)
      this[page].status = (ProgressiveStatus) 5;
    PdfBitmap bitmap = this[page].Bitmap;
    ProgressiveStatus status = this[page].status;
    if (bitmap != null && (status == ProgressiveStatus.Done || status == (ProgressiveStatus) 4 || status == ProgressiveStatus.Failed))
      return true;
    bool flag = this.ProcessExisting(bitmap ?? this.CanvasBitmap, page, pageRect, pageRotate, renderFlags);
    if (bitmap != null)
      Pdfium.FPDFBitmap_CompositeBitmap(this.CanvasBitmap.Handle, pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height, bitmap.Handle, pageRect.X, pageRect.Y);
    return flag;
  }

  private bool ProcessExisting(
    PdfBitmap bitmap,
    PdfPage page,
    Int32Rect pageRect,
    PageRotate pageRotate,
    RenderFlags renderFlags)
  {
    lock (page)
    {
      PRItem prItem;
      if (!this.TryGetValue(page, out prItem))
        return false;
      switch (prItem.status)
      {
        case ProgressiveStatus.Ready:
          prItem.status = page.StartProgressiveRender(bitmap, pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height, pageRotate, renderFlags, (byte[]) null);
          return prItem.status == ProgressiveStatus.Done;
        case ProgressiveStatus.ToBeContinued:
          prItem.status = page.ContinueProgressiveRender();
          return false;
        case ProgressiveStatus.Done:
          page.CancelProgressiveRender();
          prItem.status = (ProgressiveStatus) 4;
          return true;
        case (ProgressiveStatus) 4:
          return true;
        case (ProgressiveStatus) 5:
          prItem.status = (ProgressiveStatus) 4;
          page.RenderEx(bitmap, pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height, pageRotate, renderFlags);
          return true;
        case (ProgressiveStatus) 6:
          prItem.status = (ProgressiveStatus) 4;
          this.DrawThumbnail(bitmap, page, pageRect, pageRotate, renderFlags);
          return true;
        default:
          bitmap.FillRectEx(pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height, Helpers.ToArgb(Colors.Red));
          bitmap.FillRectEx(pageRect.X + 5, pageRect.Y + 5, pageRect.Width - 10, pageRect.Height - 10, Helpers.ToArgb(Colors.White));
          page.CancelProgressiveRender();
          prItem.status = (ProgressiveStatus) 4;
          return true;
      }
    }
  }

  private void DrawThumbnail(
    PdfBitmap bitmap,
    PdfPage page,
    Int32Rect pageRect,
    PageRotate pageRotate,
    RenderFlags renderFlags)
  {
    int width = Math.Max((int) page.Width, pageRect.Width);
    int height = Math.Max((int) page.Height, pageRect.Height);
    using (PdfBitmap bitmap1 = new PdfBitmap(width, height, true))
    {
      page.RenderEx(bitmap1, 0, 0, width, height, pageRotate, renderFlags);
      using (Graphics graphics = Graphics.FromImage(bitmap.Image))
        graphics.DrawImage(bitmap1.Image, pageRect.X, pageRect.Y, pageRect.Width, pageRect.Height);
    }
  }

  private void ProcessNew(PdfPage page)
  {
    PRItem prItem = new PRItem(ProgressiveStatus.Ready, page.IsTransparency ? this.CanvasSize : new Helpers.Int32Size(0, 0));
    this.Add(page, prItem);
    page.Disposed += new EventHandler(this.Page_Disposed);
  }

  private void PageRemove(PdfPage page)
  {
    if (!this.ContainsKey(page))
      return;
    this.ReleasePage(page);
    this.Remove(page);
  }

  private void ReleasePage(PdfPage page)
  {
    if (this[page].status == ProgressiveStatus.ToBeContinued)
      page.CancelProgressiveRender();
    this[page].Dispose();
    page.Disposed -= new EventHandler(this.Page_Disposed);
  }

  private void Page_Disposed(object sender, EventArgs e) => this.PageRemove(sender as PdfPage);
}
