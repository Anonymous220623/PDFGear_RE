// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PageObjectHitTestCache
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Utils.PageContents;

internal class PageObjectHitTestCache
{
  private object locker = new object();
  private WeakReference<PdfDocument> weakDocument;
  private Dictionary<WeakReference<PdfPage>, PageObjectHitTestCacheItem> cacheItems;

  public PageObjectHitTestCache(PdfDocument doc)
  {
    this.weakDocument = doc != null ? new WeakReference<PdfDocument>(doc) : throw new ArgumentException(nameof (doc));
    this.cacheItems = new Dictionary<WeakReference<PdfPage>, PageObjectHitTestCacheItem>();
  }

  public PdfDocument Document
  {
    get
    {
      PdfDocument target;
      return this.weakDocument.TryGetTarget(out target) ? target : (PdfDocument) null;
    }
  }

  public int[] GetPointObjectIndexes(int pageIndex, double x, double y)
  {
    return this.GetItemCore(pageIndex)?.GetPointObject(x, y) ?? Array.Empty<int>();
  }

  public int[] GetIntersectingObjectIndexes(int pageIndex, FS_RECTF rect)
  {
    return this.GetItemCore(pageIndex)?.GetIntersecteObject(rect) ?? Array.Empty<int>();
  }

  private PageObjectHitTestCacheItem GetItemCore(int pageIndex)
  {
    if (pageIndex < 0)
      return (PageObjectHitTestCacheItem) null;
    PdfDocument document = this.Document;
    if (document == null || pageIndex >= document.Pages.Count)
      return (PageObjectHitTestCacheItem) null;
    PdfPage page = document.Pages[pageIndex];
    return this.TryGetItemCore(page) ?? this.AddOrUpdateItemCore(page);
  }

  private PageObjectHitTestCacheItem TryGetItemCore(PdfPage page)
  {
    if (!this.ValidPage(page))
      return (PageObjectHitTestCacheItem) null;
    lock (this.locker)
    {
      foreach (KeyValuePair<WeakReference<PdfPage>, PageObjectHitTestCacheItem> cacheItem in this.cacheItems)
      {
        PdfPage target;
        if (cacheItem.Key.TryGetTarget(out target) && target.PageIndex == page.PageIndex)
          return cacheItem.Value;
      }
    }
    return (PageObjectHitTestCacheItem) null;
  }

  internal PageObjectHitTestCacheItem AddOrUpdateItemCore(PdfPage page)
  {
    if (!this.ValidPage(page))
      return (PageObjectHitTestCacheItem) null;
    lock (this.locker)
    {
      this.RemovePage(page);
      PageObjectHitTestCacheItem hitTestCacheItem = PageObjectHitTestCacheItem.Create(page);
      if (hitTestCacheItem != null)
      {
        this.cacheItems[new WeakReference<PdfPage>(page)] = hitTestCacheItem;
        return hitTestCacheItem;
      }
    }
    return (PageObjectHitTestCacheItem) null;
  }

  private void RemovePage(PdfPage page)
  {
    if (!this.ValidPage(page))
      return;
    lock (this.locker)
    {
      foreach (WeakReference<PdfPage> key in this.cacheItems.Keys.ToArray<WeakReference<PdfPage>>())
      {
        PdfPage target;
        if (key.TryGetTarget(out target))
        {
          if (target.PageIndex == page.PageIndex)
            this.cacheItems.Remove(key);
        }
        else
          this.cacheItems.Remove(key);
      }
    }
  }

  private bool ValidPage(PdfPage page)
  {
    if (page == null || page.Document == null || page.Document.IsDisposed || page.Document.Handle == IntPtr.Zero)
      return false;
    PdfDocument document = this.Document;
    return document != null && !document.IsDisposed && !(document.Handle == IntPtr.Zero) && document.Handle == page.Document.Handle;
  }
}
