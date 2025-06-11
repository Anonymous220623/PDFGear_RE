// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.BitmapImageSource
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace PDFKit.GenerateImagePdf;

public abstract class BitmapImageSource : IDisposable
{
  private bool isDisposed;
  private CancellationTokenSource cancellationTokenSource;
  private SemaphoreSlim locker = new SemaphoreSlim(1, 1);

  protected bool IsDisposed => this.isDisposed;

  public async Task<Bitmap> CreateAsync(
    int decodePixelWidth,
    int decodePixelHeight,
    CancellationToken cancellationToken)
  {
    if (this.IsDisposed)
      throw new ObjectDisposedException(nameof (BitmapImageSource));
    if (this.cancellationTokenSource == null)
      this.cancellationTokenSource = new CancellationTokenSource();
    CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(this.cancellationTokenSource.Token, cancellationToken);
    await this.locker.WaitAsync(cts.Token);
    Bitmap async;
    try
    {
      async = await this.CreateCore(decodePixelWidth, decodePixelHeight, cts.Token).ConfigureAwait(false);
    }
    finally
    {
      this.locker.Release();
    }
    cts = (CancellationTokenSource) null;
    return async;
  }

  protected abstract Task<Bitmap> CreateCore(
    int decodePixelWidth,
    int decodePixelHeight,
    CancellationToken cancellationToken);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private void Dispose(bool disposing)
  {
    if (this.isDisposed)
      return;
    this.isDisposed = true;
    this.cancellationTokenSource?.Cancel();
    this.locker.Wait();
    try
    {
      this.DisposeCore(disposing);
    }
    finally
    {
      this.locker.Release();
    }
  }

  protected virtual void DisposeCore(bool disposing)
  {
  }

  public static BitmapImageSource CreateFromSystemDrawingBitmap(Bitmap bitmap, bool leaveOpen)
  {
    return (BitmapImageSource) new SystemDrawingBitmapImageSource(bitmap, leaveOpen);
  }
}
