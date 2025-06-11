// Decompiled with JetBrains decompiler
// Type: PDFKit.PRItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;

#nullable disable
namespace PDFKit;

internal class PRItem : IDisposable
{
  public ProgressiveStatus status;
  public PdfBitmap Bitmap;

  public PRItem(ProgressiveStatus status, Helpers.Int32Size canvasSize)
  {
    this.status = status;
    if (canvasSize.Width <= 0 || canvasSize.Height <= 0)
      return;
    this.Bitmap = new PdfBitmap(canvasSize.Width, canvasSize.Height, true);
  }

  public void Dispose()
  {
    if (this.Bitmap != null)
      this.Bitmap.Dispose();
    this.Bitmap = (PdfBitmap) null;
  }
}
