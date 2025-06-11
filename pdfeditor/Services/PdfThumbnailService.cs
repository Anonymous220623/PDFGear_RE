// Decompiled with JetBrains decompiler
// Type: pdfeditor.Services.PdfThumbnailService
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using LruCacheNet;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Services;

public class PdfThumbnailService
{
  public const int DefaultThumbnailWidth = 150;
  private LruCache<PdfThumbnailService.PdfThumbnailCacheKey, WriteableBitmap> thumbnailCache;
  private int thumbnailWidth;

  public PdfThumbnailService()
    : this(150, 50)
  {
  }

  public PdfThumbnailService(int thumbnailWidth, int cacheSize)
  {
    if (thumbnailWidth <= 0)
      throw new ArgumentException(nameof (thumbnailWidth));
    if (cacheSize <= 0)
      throw new ArgumentException(nameof (cacheSize));
    this.thumbnailWidth = thumbnailWidth;
    this.thumbnailCache = new LruCache<PdfThumbnailService.PdfThumbnailCacheKey, WriteableBitmap>(cacheSize);
  }

  public int ThumbnailWidth => this.thumbnailWidth;

  public void ClearCache()
  {
    lock (this.thumbnailCache)
    {
      if (this.thumbnailCache.Count <= 0)
        return;
      this.thumbnailCache.Clear();
    }
  }

  public System.Windows.Size GetThumbnailImageSize(
    PdfPage page,
    PageRotate rotate,
    int width = 0,
    int height = 0)
  {
    FS_SIZEF fsSizef = page != null ? page.GetEffectiveSize(rotate) : throw new ArgumentNullException(nameof (page));
    if (width == 0 && height == 0)
      width = this.ThumbnailWidth;
    if (height == 0)
      height = (int) ((double) Math.Abs(fsSizef.Height) * (double) width * 1.0 / (double) Math.Abs(fsSizef.Width));
    else
      width = (int) ((double) Math.Abs(fsSizef.Width) * (double) height * 1.0 / (double) Math.Abs(fsSizef.Height));
    return new System.Windows.Size((double) width, (double) height);
  }

  public async Task<WriteableBitmap> TryGetPdfBitmapAsync(
    PdfPage page,
    System.Windows.Media.Color background,
    PageRotate rotate,
    CancellationToken cancellationToken)
  {
    return await this.TryGetPdfBitmapAsync(page, background, rotate, 0, 0, cancellationToken).ConfigureAwait(false);
  }

  public async Task<WriteableBitmap> TryGetPdfBitmapAsync(
    PdfPage page,
    System.Windows.Media.Color background,
    PageRotate rotate,
    int width,
    int height,
    CancellationToken cancellationToken)
  {
    if (page == null)
      return (WriteableBitmap) null;
    PdfThumbnailService.PdfThumbnailCacheKey cacheKey = new PdfThumbnailService.PdfThumbnailCacheKey(page.PageIndex, background, rotate);
    System.Windows.Size thumbnailImageSize = this.GetThumbnailImageSize(page, rotate, width, height);
    int _width = (int) thumbnailImageSize.Width;
    int _height = (int) thumbnailImageSize.Height;
    if (_width <= 0 || _height <= 0)
      return (WriteableBitmap) null;
    lock (this.thumbnailCache)
    {
      WriteableBitmap data;
      if (this.thumbnailCache.TryGetValue(cacheKey, out data))
      {
        if (data.PixelWidth == _width && data.PixelHeight == _height)
          return data;
        if (data.PixelWidth > _width || data.PixelHeight > _height)
        {
          using (Bitmap bitmap = new Bitmap(data.PixelWidth, data.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
          {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, data.PixelWidth, data.PixelHeight), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            data.CopyPixels(new Int32Rect(0, 0, data.PixelWidth, data.PixelHeight), bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            IntPtr hbitmap = bitmap.GetHbitmap();
            try
            {
              return new WriteableBitmap(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, new Int32Rect(0, 0, bitmap.Width, bitmap.Height), BitmapSizeOptions.FromWidthAndHeight(_width, _height)));
            }
            finally
            {
              try
              {
                if (hbitmap != IntPtr.Zero)
                  DrawUtils.DeleteObject(hbitmap);
              }
              catch
              {
              }
            }
          }
        }
        else
          this.thumbnailCache.Remove(cacheKey);
      }
    }
    return await Task.Run<WriteableBitmap>(TaskExceptionHelper.ExceptionBoundary<WriteableBitmap>((Func<Task<WriteableBitmap>>) (async () =>
    {
      WriteableBitmap pdfBitmapAsync;
      using (PdfBitmap pdfBitmap = new PdfBitmap(_width, _height, BitmapFormats.FXDIB_Argb))
      {
        for (int i = 0; i < 3 && !this.TryRender(pdfBitmap, page, background, rotate); ++i)
          await Task.Delay(50, cancellationToken);
        WriteableBitmap bitmap = (WriteableBitmap) null;
        try
        {
          bitmap = await pdfBitmap.ToWriteableBitmapAsync(cancellationToken).ConfigureAwait(false);
          lock (this.thumbnailCache)
            this.thumbnailCache[cacheKey] = bitmap;
        }
        finally
        {
          await page.TryRedrawPageAsync();
        }
        pdfBitmapAsync = bitmap;
      }
      return pdfBitmapAsync;
    })), cancellationToken).ConfigureAwait(false);
  }

  public void RefreshAllThumbnail() => this.RefreshThumbnail(-1);

  public void RefreshThumbnail(params int[] pageIndexes)
  {
    lock (this.thumbnailCache)
    {
      if (pageIndexes == null)
        return;
      List<int> intList = new List<int>();
      foreach (int pageIndex in pageIndexes)
      {
        if (pageIndex == -1)
        {
          this.ClearCache();
          StrongReferenceMessenger.Default.Send<ValueChangedMessage<int>, string>(new ValueChangedMessage<int>(-1), "MESSAGE_PAGE_ROTATE_CHANGED");
          return;
        }
        if (pageIndex >= 0)
          intList.Add(pageIndex);
      }
      Dictionary<int, PdfThumbnailService.PdfThumbnailCacheKey[]> dictionary = this.thumbnailCache.Keys.GroupBy<PdfThumbnailService.PdfThumbnailCacheKey, int>((Func<PdfThumbnailService.PdfThumbnailCacheKey, int>) (c => c.PageIndex)).ToDictionary<IGrouping<int, PdfThumbnailService.PdfThumbnailCacheKey>, int, PdfThumbnailService.PdfThumbnailCacheKey[]>((Func<IGrouping<int, PdfThumbnailService.PdfThumbnailCacheKey>, int>) (c => c.Key), (Func<IGrouping<int, PdfThumbnailService.PdfThumbnailCacheKey>, PdfThumbnailService.PdfThumbnailCacheKey[]>) (c => c.ToArray<PdfThumbnailService.PdfThumbnailCacheKey>()));
      foreach (int key1 in intList)
      {
        PdfThumbnailService.PdfThumbnailCacheKey[] thumbnailCacheKeyArray;
        if (dictionary.TryGetValue(key1, out thumbnailCacheKeyArray))
        {
          foreach (PdfThumbnailService.PdfThumbnailCacheKey key2 in thumbnailCacheKeyArray)
            this.thumbnailCache.Remove(key2);
        }
        StrongReferenceMessenger.Default.Send<ValueChangedMessage<int>, string>(new ValueChangedMessage<int>(key1), "MESSAGE_PAGE_ROTATE_CHANGED");
      }
    }
  }

  private bool TryRender(PdfBitmap pdfBitmap, PdfPage page, System.Windows.Media.Color background, PageRotate rotate)
  {
    if (pdfBitmap == null)
      throw new ArgumentNullException(nameof (pdfBitmap));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    IntPtr page1 = Pdfium.FPDF_LoadPage(page.Document.Handle, page.PageIndex);
    if (page1 == IntPtr.Zero)
      return false;
    int width = pdfBitmap.Width;
    int height = pdfBitmap.Height;
    try
    {
      pdfBitmap.FillRectEx(0, 0, width, height, PdfThumbnailService.ToArgb(background));
      lock (page)
        Pdfium.FPDF_RenderPageBitmap(pdfBitmap.Handle, page1, 0, 0, width, height, PdfThumbnailService.PageRotation(page, new PageRotate?(rotate)), RenderFlags.FPDF_ANNOT);
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    finally
    {
      Pdfium.FPDF_ClosePage(page1);
    }
    return false;
  }

  private static int ToArgb(System.Windows.Media.Color color)
  {
    return (int) color.A << 24 | (int) color.R << 16 /*0x10*/ | (int) color.G << 8 | (int) color.B;
  }

  private static PageRotate PageRotation(PdfPage pdfPage, PageRotate? rotate)
  {
    int num = (int) (((int) rotate ?? (int) pdfPage.Rotation) - pdfPage.Rotation);
    if (num < 0)
      num = 4 + num;
    return (PageRotate) num;
  }

  private struct PdfThumbnailCacheKey(int pageIndex, System.Windows.Media.Color background, PageRotate rotate) : 
    IEquatable<PdfThumbnailService.PdfThumbnailCacheKey>
  {
    private int _hashCode = HashCode.Combine<int, System.Windows.Media.Color, PageRotate>(pageIndex, background, rotate);

    public System.Windows.Media.Color Background { get; } = background;

    public PageRotate Rotate { get; } = rotate;

    public int PageIndex { get; } = pageIndex;

    public bool Equals(PdfThumbnailService.PdfThumbnailCacheKey other)
    {
      return this._hashCode == other._hashCode && this.PageIndex == other.PageIndex && this.Background == other.Background && this.Rotate == other.Rotate;
    }

    public override bool Equals(object obj)
    {
      return obj is PdfThumbnailService.PdfThumbnailCacheKey thumbnailCacheKey && thumbnailCacheKey.Equals(this);
    }

    public override int GetHashCode() => this._hashCode;

    public static bool operator ==(
      PdfThumbnailService.PdfThumbnailCacheKey left,
      PdfThumbnailService.PdfThumbnailCacheKey right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(
      PdfThumbnailService.PdfThumbnailCacheKey left,
      PdfThumbnailService.PdfThumbnailCacheKey right)
    {
      return !left.Equals(right);
    }
  }
}
