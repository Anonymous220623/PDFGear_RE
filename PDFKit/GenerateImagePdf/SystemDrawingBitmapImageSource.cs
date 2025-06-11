// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.SystemDrawingBitmapImageSource
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace PDFKit.GenerateImagePdf;

internal class SystemDrawingBitmapImageSource : BitmapImageSource
{
  private readonly bool leaveOpen;
  private Bitmap bitmap;

  public SystemDrawingBitmapImageSource(Bitmap bitmap, bool leaveOpen)
  {
    this.bitmap = bitmap;
    this.leaveOpen = leaveOpen;
  }

  protected override Task<Bitmap> CreateCore(
    int decodePixelWidth,
    int decodePixelHeight,
    CancellationToken cancellationToken)
  {
    return Task.FromResult<Bitmap>((Bitmap) this.bitmap.Clone());
  }

  protected override void DisposeCore(bool disposing)
  {
    base.DisposeCore(disposing);
    if (!this.leaveOpen)
      this.bitmap?.Dispose();
    this.bitmap = (Bitmap) null;
  }
}
