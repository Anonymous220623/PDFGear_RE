// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfFeatures
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PdfFeatures
{
  private const double CHAR_SIZE_MULTIPLIER = 0.001;
  private const int c_gapBetweenPages = 8;
  internal float m_zoomFactor = 1f;
  private PdfUnitConvertor m_unitConvertor = new PdfUnitConvertor();
  private Syncfusion.PdfViewer.Base.DeviceCMYK m_cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
  private MatchedItemCollection m_searchObjects = new MatchedItemCollection();
  private Dictionary<int, string> m_pageTexts = new Dictionary<int, string>();
  private object m_lock = new object();
  private int m_pageCount;
  private PdfLoadedDocument m_loadedDocument;
  internal string searchstring;
  private List<Page> pagesForTextSearch;

  internal Bitmap ExportAsImage(PdfPageBase pageBase, PdfLoadedDocument loadedDocument)
  {
    Page page = new Page(pageBase);
    page.Initialize(pageBase, false);
    Bitmap bitmap = !(page.CropBox != RectangleF.Empty) || !(page.CropBox != page.MediaBox) ? new Bitmap((int) ((double) page.Bounds.Width * (double) this.m_zoomFactor), (int) ((double) page.Bounds.Height * (double) this.m_zoomFactor)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Width - page.CropBox.X, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor), (int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Height - page.CropBox.Y, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor));
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap))
    {
      g.TranslateTransform(0.0f, 0.0f);
      g.FillRectangle(Brushes.White, new RectangleF(0.0f, 0.0f, page.Bounds.Width, page.Bounds.Height));
      if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
        g.TranslateTransform(-(page.CropBox.X * 1.333f), -(page.CropBox.Y * 1.333f));
      else
        g.TranslateTransform(-(page.MediaBox.X * 1.333f), -(page.MediaBox.Y * 1.333f));
      g.ScaleTransform(this.m_zoomFactor, this.m_zoomFactor);
      if (page.RecordCollection == null)
        page.Initialize(pageBase, true);
      ImageRenderer imageRenderer;
      if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
      {
        float pixels = this.m_unitConvertor.ConvertToPixels((page.CropBox.Height - page.CropBox.Y) * this.m_zoomFactor, PdfGraphicsUnit.Point);
        imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, this.m_cmyk, pixels / this.m_zoomFactor);
      }
      else
        imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, this.m_cmyk, page.Height);
      imageRenderer.pageRotation = (float) page.Rotation;
      imageRenderer.zoomFactor = this.m_zoomFactor;
      imageRenderer.substitutedFontsList = loadedDocument.currentFont;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      imageRenderer.RenderAsImage();
      Thread.CurrentThread.CurrentCulture = currentCulture;
      if (page.Rotation == 90.0)
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
      else if (page.Rotation == 180.0)
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
      else if (page.Rotation == 270.0)
        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
    }
    return bitmap;
  }

  internal Metafile ExportAsMetafile(PdfPageBase pageBase)
  {
    Page page = new Page(pageBase);
    page.Initialize(pageBase, false);
    Bitmap bitmap = !(page.CropBox != RectangleF.Empty) || !(page.CropBox != page.MediaBox) ? new Bitmap((int) ((double) page.Bounds.Width * (double) this.m_zoomFactor), (int) ((double) page.Bounds.Height * (double) this.m_zoomFactor)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Width - page.CropBox.X, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor), (int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Height - page.CropBox.Y, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor));
    RectangleF frameRect = new RectangleF(0.0f, 0.0f, (float) bitmap.Size.Width, (float) bitmap.Size.Height);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap);
    IntPtr hdc = graphics.GetHdc();
    Metafile metafile;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      double num = (double) new PdfUnitConvertor().ConvertFromPixels(page.Height, PdfGraphicsUnit.Point);
      metafile = new Metafile((Stream) memoryStream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
      graphics.ReleaseHdc(hdc);
      System.Drawing.Graphics g;
      using (g = System.Drawing.Graphics.FromImage((Image) metafile))
      {
        g.TranslateTransform(0.0f, 0.0f);
        g.FillRectangle(Brushes.White, new RectangleF(0.0f, 0.0f, page.Bounds.Width, page.Bounds.Height));
        if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
          g.TranslateTransform(-(page.CropBox.X * 1.333f), -(page.CropBox.Y * 1.333f));
        else
          g.TranslateTransform(-(page.MediaBox.X * 1.333f), -(page.MediaBox.Y * 1.333f));
        g.ScaleTransform(this.m_zoomFactor, this.m_zoomFactor);
        if (page.RecordCollection == null)
          page.Initialize(pageBase, true);
        ImageRenderer imageRenderer;
        if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
        {
          float pixels = this.m_unitConvertor.ConvertToPixels(page.CropBox.Height - page.CropBox.Y, PdfGraphicsUnit.Point);
          imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, pixels, this.m_cmyk);
        }
        else
          imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, page.Height, this.m_cmyk);
        imageRenderer.pageRotation = (float) page.Rotation;
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        imageRenderer.RenderAsImage();
        Thread.CurrentThread.CurrentCulture = currentCulture;
        ImageRenderer.textDictonary.Clear();
      }
    }
    return metafile;
  }

  internal Bitmap ExportAsImage(
    PdfPageBase pageBase,
    float dpiX,
    float dpiY,
    PdfLoadedDocument loadedDocument)
  {
    Page page = new Page(pageBase);
    page.Initialize(pageBase, false);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
    Bitmap bitmap = !(page.CropBox != RectangleF.Empty) || !(page.CropBox != page.MediaBox) ? new Bitmap((int) ((double) page.Bounds.Width * (double) dpiX / (double) graphics.DpiX), (int) ((double) page.Bounds.Height * (double) dpiY / (double) graphics.DpiY)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Width - page.CropBox.X, PdfGraphicsUnit.Point) * (double) dpiX / 96.0), (int) ((double) this.m_unitConvertor.ConvertToPixels(page.CropBox.Height - page.CropBox.Y, PdfGraphicsUnit.Point) * (double) dpiY / 96.0));
    bitmap.SetResolution(dpiX, dpiY);
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap))
    {
      g.TranslateTransform(0.0f, 0.0f);
      g.FillRectangle(Brushes.White, new RectangleF(0.0f, 0.0f, page.Bounds.Width * dpiX / graphics.DpiX, page.Bounds.Height * dpiY / graphics.DpiY));
      if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
        g.TranslateTransform(-(page.CropBox.X * 1.333f), -(page.CropBox.Y * 1.333f));
      else
        g.TranslateTransform(-(page.MediaBox.X * 1.333f), -(page.MediaBox.Y * 1.333f));
      g.ScaleTransform(this.m_zoomFactor, this.m_zoomFactor);
      if (page.RecordCollection == null)
        page.Initialize(pageBase, true);
      ImageRenderer imageRenderer;
      if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
      {
        float pixels = this.m_unitConvertor.ConvertToPixels((page.CropBox.Height - page.CropBox.Y) * this.m_zoomFactor, PdfGraphicsUnit.Point);
        imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, this.m_cmyk, (float) ((double) pixels * (double) dpiY / 96.0));
      }
      else
        imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, this.m_cmyk, page.Height * dpiY / graphics.DpiY);
      imageRenderer.isExportAsImage = true;
      imageRenderer.pageRotation = (float) page.Rotation;
      imageRenderer.zoomFactor = this.m_zoomFactor;
      imageRenderer.substitutedFontsList = loadedDocument.currentFont;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      imageRenderer.RenderAsImage();
      imageRenderer.isExportAsImage = false;
      Thread.CurrentThread.CurrentCulture = currentCulture;
      if (page.Rotation == 90.0)
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
      else if (page.Rotation == 180.0)
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
      else if (page.Rotation == 270.0)
        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
      page.Clear();
      if (page.RecordCollection != null)
        page.RecordCollection.RecordCollection.Clear();
      if (page.Resources != null)
        page.Resources.Dispose();
      if (page.Graphics != null)
        page.Graphics.Dispose();
      imageRenderer.Clear(false);
    }
    return bitmap;
  }

  internal Bitmap ExportAsImage(
    PdfPageBase pageBase,
    SizeF customSize,
    bool keepAspectRatio,
    PdfLoadedDocument loadedDocument)
  {
    float num = 1f;
    PdfPageBase page1 = pageBase;
    Page page2 = new Page(page1);
    page2.Initialize(page1, false);
    double height = (double) page2.Bounds.Height;
    double width = (double) page2.Bounds.Width;
    float sx;
    float sy;
    if (!keepAspectRatio)
    {
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        sx = customSize.Width / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point);
        sy = customSize.Height / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point);
      }
      else
      {
        sx = customSize.Width / page2.Width;
        sy = customSize.Height / page2.Height;
      }
    }
    else
    {
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        sx = customSize.Width / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point);
        sy = customSize.Height / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point);
      }
      else
      {
        sx = customSize.Width / page2.Width;
        sy = customSize.Height / page2.Height;
      }
      num = (double) sx >= (double) sy ? sy : sx;
    }
    Bitmap bitmap = !keepAspectRatio ? (!(page2.CropBox != RectangleF.Empty) || !(page2.CropBox != page2.MediaBox) ? new Bitmap((int) ((double) page2.Bounds.Width * (double) sx), (int) ((double) page2.Bounds.Height * (double) sy)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point) * (double) sx), (int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point) * (double) sy))) : (!(page2.CropBox != RectangleF.Empty) || !(page2.CropBox != page2.MediaBox) ? new Bitmap((int) ((double) page2.Bounds.Width * (double) num), (int) ((double) page2.Bounds.Height * (double) num)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point) * (double) num), (int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point) * (double) num)));
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap))
    {
      if (keepAspectRatio)
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) ((double) page2.Bounds.Width * (double) num), (int) ((double) page2.Bounds.Height * (double) num)));
      else
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) ((double) page2.Bounds.Width * (double) sx), (int) ((double) page2.Bounds.Height * (double) sy)));
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
        g.TranslateTransform(-(page2.CropBox.X * 1.333f), -(page2.CropBox.Y * 1.333f));
      else
        g.TranslateTransform(-(page2.MediaBox.X * 1.333f), -(page2.MediaBox.Y * 1.333f));
      if (!keepAspectRatio)
        g.ScaleTransform(sx, sy);
      else
        g.ScaleTransform(num, num);
      if (page2.RecordCollection == null)
        page2.Initialize(page1, true);
      ImageRenderer imageRenderer;
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        float pixels = this.m_unitConvertor.ConvertToPixels((page2.CropBox.Height - page2.CropBox.Y) * num, PdfGraphicsUnit.Point);
        imageRenderer = new ImageRenderer(page2.RecordCollection, page2.Resources, g, true, this.m_cmyk, pixels / num);
      }
      else
        imageRenderer = new ImageRenderer(page2.RecordCollection, page2.Resources, g, true, this.m_cmyk, page2.Height);
      imageRenderer.pageRotation = (float) page2.Rotation;
      imageRenderer.zoomFactor = num;
      imageRenderer.substitutedFontsList = loadedDocument.currentFont;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      imageRenderer.RenderAsImage();
      Thread.CurrentThread.CurrentCulture = currentCulture;
      g.Dispose();
    }
    if (page2.Rotation == 90.0)
      bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
    else if (page2.Rotation == 270.0)
      bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
    else if (page2.Rotation == 180.0)
      bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
    return bitmap;
  }

  internal Bitmap ExportAsImage(
    PdfPageBase pageBase,
    SizeF customSize,
    float dpiX,
    float dpiY,
    bool keepAspectRatio,
    PdfLoadedDocument loadedDocument)
  {
    float num = 1f;
    PdfPageBase page1 = pageBase;
    Page page2 = new Page(page1);
    page2.Initialize(page1, false);
    double height = (double) page2.Bounds.Height;
    double width = (double) page2.Bounds.Width;
    float sx;
    float sy;
    if (!keepAspectRatio)
    {
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        sx = customSize.Width / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point);
        sy = customSize.Height / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point);
      }
      else
      {
        sx = customSize.Width / page2.Width;
        sy = customSize.Height / page2.Height;
      }
    }
    else
    {
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        sx = customSize.Width / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point);
        sy = customSize.Height / (float) (int) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point);
      }
      else
      {
        sx = customSize.Width / page2.Width;
        sy = customSize.Height / page2.Height;
      }
      num = (double) sx >= (double) sy ? sy : sx;
    }
    Bitmap bitmap;
    if (keepAspectRatio)
    {
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        bitmap = new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point) * (double) dpiX / 96.0 * (double) num), (int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point) * (double) dpiY / 96.0 * (double) num));
        bitmap.SetResolution(dpiX, dpiY);
      }
      else
      {
        bitmap = new Bitmap((int) ((double) page2.Bounds.Width * (double) dpiX / 96.0 * (double) num), (int) ((double) page2.Bounds.Height * (double) dpiY / 96.0 * (double) num));
        bitmap.SetResolution(dpiX, dpiY);
      }
    }
    else if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
    {
      bitmap = new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Width - page2.CropBox.X, PdfGraphicsUnit.Point) * (double) dpiX / 96.0 * (double) sx), (int) ((double) this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point) * (double) dpiY / 96.0 * (double) sy));
      bitmap.SetResolution(dpiX, dpiY);
    }
    else
    {
      bitmap = new Bitmap((int) ((double) page2.Bounds.Width * (double) dpiX / 96.0 * (double) sx), (int) ((double) page2.Bounds.Height * (double) dpiY / 96.0 * (double) sy));
      bitmap.SetResolution(dpiX, dpiY);
    }
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap))
    {
      if (keepAspectRatio)
        g.FillRectangle(Brushes.White, new RectangleF(0.0f, 0.0f, (float) ((double) page2.Bounds.Width * (double) dpiX / 96.0) * num, (float) ((double) page2.Bounds.Height * (double) dpiY / 96.0) * num));
      else
        g.FillRectangle(Brushes.White, new RectangleF(0.0f, 0.0f, (float) ((double) page2.Bounds.Width * (double) dpiX / 96.0) * sx, (float) ((double) page2.Bounds.Height * (double) dpiY / 96.0) * sy));
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
        g.TranslateTransform(-(page2.CropBox.X * 1.333f), -(page2.CropBox.Y * 1.333f));
      else
        g.TranslateTransform(-(page2.MediaBox.X * 1.333f), -(page2.MediaBox.Y * 1.333f));
      if (!keepAspectRatio)
        g.ScaleTransform(sx, sy);
      else
        g.ScaleTransform(num, num);
      if (page2.RecordCollection == null)
        page2.Initialize(page1, true);
      ImageRenderer imageRenderer;
      if (page2.CropBox != RectangleF.Empty && page2.CropBox != page2.MediaBox)
      {
        float pixels = this.m_unitConvertor.ConvertToPixels(page2.CropBox.Height - page2.CropBox.Y, PdfGraphicsUnit.Point);
        imageRenderer = new ImageRenderer(page2.RecordCollection, page2.Resources, g, true, this.m_cmyk, (float) ((double) pixels * (double) dpiY / 96.0));
      }
      else
        imageRenderer = new ImageRenderer(page2.RecordCollection, page2.Resources, g, true, this.m_cmyk, (float) ((double) page2.Height * (double) dpiY / 96.0));
      imageRenderer.pageRotation = (float) page2.Rotation;
      imageRenderer.zoomFactor = num;
      imageRenderer.substitutedFontsList = loadedDocument.currentFont;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      imageRenderer.RenderAsImage();
      Thread.CurrentThread.CurrentCulture = currentCulture;
      g.Dispose();
    }
    if (page2.Rotation == 90.0)
      bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
    else if (page2.Rotation == 270.0)
      bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
    else if (page2.Rotation == 180.0)
      bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
    return bitmap;
  }

  private void LoadPagesForTextSearch(PdfLoadedDocument loadedDocument)
  {
    this.pagesForTextSearch = new List<Page>();
    for (int index = 0; index < loadedDocument.PageCount; ++index)
    {
      PdfPageBase page1 = loadedDocument.Pages[index];
      Page page2 = new Page(page1);
      page2.Initialize(page1, false);
      this.pagesForTextSearch.Add(page2);
    }
  }

  internal bool FindText(
    PdfLoadedDocument loadedDocument,
    List<string> searchItems,
    int pageIndex,
    TextSearchOptions textSearchOption,
    out List<MatchedItem> searchResults)
  {
    this.SearchInBackground(loadedDocument, searchItems, textSearchOption, pageIndex, pageIndex);
    return this.GetSearchResults(out searchResults);
  }

  internal bool FindText(
    PdfLoadedDocument loadedDocument,
    List<string> listOfTerms,
    TextSearchOptions textSearchOption,
    out TextSearchResultCollection searchResult,
    bool isMultiThread)
  {
    int PageCount = loadedDocument.Pages.Count;
    if (isMultiThread)
    {
      if (PageCount >= 8)
      {
        int splitCount = PageCount / 8;
        Task.WaitAll(Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 0, splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, splitCount + 1, 2 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 2 * splitCount + 1, 3 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 3 * splitCount + 1, 4 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 4 * splitCount + 1, 5 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 5 * splitCount + 1, 6 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 6 * splitCount + 1, 7 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 7 * splitCount + 1, PageCount - 1))));
      }
      else if (PageCount >= 4)
      {
        int splitCount = PageCount / 4;
        Task.WaitAll(Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 0, splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, splitCount + 1, 2 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 2 * splitCount + 1, 3 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 3 * splitCount + 1, PageCount - 1))));
      }
      else if (PageCount >= 2)
        Parallel.Invoke((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 0, PageCount / 2)), (Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, PageCount / 2 + 1, PageCount - 1)));
      else
        this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 0, PageCount - 1);
    }
    else
      this.SearchInBackground(loadedDocument, listOfTerms, textSearchOption, 0, PageCount - 1);
    return this.GetSearchResults(out searchResult);
  }

  private bool GetSearchResults(out TextSearchResultCollection searchResult)
  {
    bool searchResults = false;
    searchResult = (TextSearchResultCollection) null;
    if (this.m_searchObjects.Count <= 0)
    {
      searchResult = (TextSearchResultCollection) null;
      this.m_pageTexts.Clear();
      return false;
    }
    this.m_searchObjects.Sort((Comparison<MatchedItem>) ((value1, value2) => value1.PageNumber.CompareTo(value2.PageNumber)));
    searchResult = new TextSearchResultCollection();
    int pageNumber = this.m_searchObjects[0].PageNumber;
    MatchedItemCollection capturedTermList = new MatchedItemCollection();
    foreach (MatchedItem searchObject in (List<MatchedItem>) this.m_searchObjects)
    {
      if (capturedTermList.Count <= 0 || !this.CheckOverlapOrLarger(searchObject.Bounds, searchObject.PageNumber, capturedTermList))
      {
        if (pageNumber == searchObject.PageNumber)
        {
          capturedTermList.Add(searchObject);
        }
        else
        {
          searchResult.Add(pageNumber, capturedTermList);
          capturedTermList = new MatchedItemCollection();
          pageNumber = searchObject.PageNumber;
          capturedTermList.Add(searchObject);
        }
      }
    }
    searchResult.Add(pageNumber, capturedTermList);
    if (this.m_searchObjects.Count > 0)
      searchResults = true;
    this.m_searchObjects.Clear();
    this.m_pageTexts.Clear();
    return searchResults;
  }

  private bool GetSearchResults(out List<MatchedItem> searchResult)
  {
    bool searchResults = false;
    searchResult = (List<MatchedItem>) null;
    if (this.m_searchObjects.Count <= 0)
    {
      searchResult = (List<MatchedItem>) null;
      this.m_pageTexts.Clear();
      return false;
    }
    searchResult = new List<MatchedItem>();
    int pageNumber = this.m_searchObjects[0].PageNumber;
    List<MatchedItem> capturedTermList = new List<MatchedItem>();
    foreach (MatchedItem searchObject in (List<MatchedItem>) this.m_searchObjects)
    {
      if (capturedTermList.Count <= 0 || !this.CheckOverlapOrLarger(searchObject.Bounds, searchObject.PageNumber, capturedTermList))
        capturedTermList.Add(searchObject);
    }
    for (int index = 0; index < capturedTermList.Count; ++index)
      searchResult.Add(capturedTermList[index]);
    if (this.m_searchObjects.Count > 0)
      searchResults = true;
    this.m_searchObjects.Clear();
    this.m_pageTexts.Clear();
    return searchResults;
  }

  internal bool FindText(
    PdfLoadedDocument loadedDocument,
    List<TextSearchItem> searchItems,
    int pageIndex,
    out List<MatchedItem> searchResults)
  {
    this.SearchInBackground(loadedDocument, searchItems, pageIndex, pageIndex);
    return this.GetSearchResults(out searchResults);
  }

  internal bool FindText(
    PdfLoadedDocument loadedDocument,
    List<TextSearchItem> listOfTerms,
    out TextSearchResultCollection searchResult,
    bool isMultiThread)
  {
    int PageCount = loadedDocument.Pages.Count;
    if (isMultiThread)
    {
      if (PageCount >= 8)
      {
        int splitCount = PageCount / 8;
        Task.WaitAll(Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 0, splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, splitCount + 1, 2 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 2 * splitCount + 1, 3 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 3 * splitCount + 1, 4 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 4 * splitCount + 1, 5 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 5 * splitCount + 1, 6 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 6 * splitCount + 1, 7 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 7 * splitCount + 1, PageCount - 1))));
      }
      else if (PageCount >= 4)
      {
        int splitCount = PageCount / 4;
        Task.WaitAll(Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 0, splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, splitCount + 1, 2 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 2 * splitCount + 1, 3 * splitCount))), Task.Factory.StartNew((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 3 * splitCount + 1, PageCount - 1))));
      }
      else if (PageCount >= 2)
        Parallel.Invoke((Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, 0, PageCount / 2)), (Action) (() => this.SearchInBackground(loadedDocument, listOfTerms, PageCount / 2 + 1, PageCount - 1)));
      else
        this.SearchInBackground(loadedDocument, listOfTerms, 0, PageCount - 1);
    }
    else
      this.SearchInBackground(loadedDocument, listOfTerms, 0, PageCount - 1);
    return this.GetSearchResults(out searchResult);
  }

  private void InvokeInParallel(params Action[] actions)
  {
    ManualResetEvent[] resetEvents = new ManualResetEvent[actions.Length];
    for (int state = 0; state < actions.Length; ++state)
    {
      resetEvents[state] = new ManualResetEvent(false);
      ThreadPool.QueueUserWorkItem((WaitCallback) (index =>
      {
        int index1 = (int) index;
        actions[index1]();
        resetEvents[index1].Set();
      }), (object) state);
    }
    foreach (WaitHandle waitHandle in resetEvents)
      waitHandle.WaitOne();
  }

  private bool CheckOverlapOrLarger(
    RectangleF thisRectangle,
    int thisPageNumber,
    List<MatchedItem> capturedTermList)
  {
    List<MatchedItem> matchedItemList = new List<MatchedItem>();
    foreach (MatchedItem capturedTerm in capturedTermList)
    {
      if ((double) thisRectangle.Top <= (double) capturedTerm.Bounds.Bottom && (double) thisRectangle.Bottom >= (double) capturedTerm.Bounds.Top && (double) thisRectangle.Left <= (double) capturedTerm.Bounds.Right && (double) thisRectangle.Right >= (double) capturedTerm.Bounds.Left && (double) thisRectangle.Width <= (double) capturedTerm.Bounds.Width && thisPageNumber == capturedTerm.PageNumber)
        return true;
      if ((double) capturedTerm.Bounds.Top <= (double) thisRectangle.Bottom && (double) capturedTerm.Bounds.Bottom >= (double) thisRectangle.Top && (double) capturedTerm.Bounds.Left <= (double) thisRectangle.Right && (double) capturedTerm.Bounds.Right >= (double) thisRectangle.Left && (double) capturedTerm.Bounds.Width <= (double) thisRectangle.Width && capturedTerm.PageNumber == thisPageNumber)
        matchedItemList.Add(capturedTerm);
    }
    if (matchedItemList.Count > 0)
    {
      for (int index = 0; index < matchedItemList.Count; ++index)
        capturedTermList.Remove(matchedItemList[index]);
    }
    matchedItemList.Clear();
    return false;
  }

  private bool CheckOverlapOrLarger(
    RectangleF thisRectangle,
    int thisPageNumber,
    MatchedItemCollection capturedTermList)
  {
    MatchedItemCollection matchedItemCollection = new MatchedItemCollection();
    foreach (MatchedItem capturedTerm in (List<MatchedItem>) capturedTermList)
    {
      if (capturedTerm.Bounds.Contains(thisRectangle) && thisPageNumber == capturedTerm.PageNumber)
        return true;
      if (thisRectangle.Contains(capturedTerm.Bounds) && capturedTerm.PageNumber == thisPageNumber)
        matchedItemCollection.Add(capturedTerm);
    }
    if (matchedItemCollection.Count > 0)
    {
      for (int index = 0; index < matchedItemCollection.Count; ++index)
        capturedTermList.Remove(matchedItemCollection[index]);
    }
    matchedItemCollection.Clear();
    return false;
  }

  private void SearchInBackground(
    PdfLoadedDocument LoadedDocument,
    List<string> listOfTerms,
    TextSearchOptions textSearchOption,
    int startIndex,
    int endIndex)
  {
    Dictionary<int, List<TextProperties>> matchRect = new Dictionary<int, List<TextProperties>>();
    for (int index = startIndex; index <= endIndex; ++index)
    {
      if (!this.m_pageTexts.ContainsKey(index))
        this.m_pageTexts.Add(index, LoadedDocument.Pages[index].ExtractText(true));
      foreach (string listOfTerm in listOfTerms)
      {
        if (!this.m_pageTexts.ContainsKey(index) || this.m_pageTexts[index] != null)
        {
          if ((textSearchOption & TextSearchOptions.CaseSensitive) != TextSearchOptions.CaseSensitive)
          {
            if (this.m_pageTexts.ContainsKey(index) && this.m_pageTexts[index].ToLower().Contains(listOfTerm.ToLower()))
              this.FindTextMatches(LoadedDocument, listOfTerm, textSearchOption, index, out matchRect);
            else
              continue;
          }
          else if (this.m_pageTexts.ContainsKey(index) && this.m_pageTexts[index].Contains(listOfTerm))
            this.FindTextMatches(LoadedDocument, listOfTerm, textSearchOption, index, out matchRect);
          else
            continue;
          foreach (KeyValuePair<int, List<TextProperties>> keyValuePair in matchRect)
          {
            foreach (TextProperties textProperties in keyValuePair.Value)
              this.m_searchObjects.Add(new MatchedItem()
              {
                PageNumber = keyValuePair.Key,
                Bounds = textProperties.Bounds,
                TextColor = textProperties.StrokingBrush,
                Text = listOfTerm
              });
          }
        }
      }
    }
  }

  private void SearchInBackground(
    PdfLoadedDocument LoadedDocument,
    List<TextSearchItem> listOfTerms,
    int startIndex,
    int endIndex)
  {
    Dictionary<int, List<TextProperties>> matchRect = new Dictionary<int, List<TextProperties>>();
    for (int index = startIndex; index <= endIndex; ++index)
    {
      if (!this.m_pageTexts.ContainsKey(index))
        this.m_pageTexts.Add(index, LoadedDocument.Pages[index].ExtractText(true));
      foreach (TextSearchItem listOfTerm in listOfTerms)
      {
        if (!this.m_pageTexts.ContainsKey(index) || this.m_pageTexts[index] != null)
        {
          if ((listOfTerm.SearchOption & TextSearchOptions.CaseSensitive) != TextSearchOptions.CaseSensitive)
          {
            if (this.m_pageTexts.ContainsKey(index) && this.m_pageTexts[index].ToLower().Contains(listOfTerm.SearchWord.ToLower()))
              this.FindTextMatches(LoadedDocument, listOfTerm.SearchWord, listOfTerm.SearchOption, index, out matchRect);
            else
              continue;
          }
          else if (this.m_pageTexts.ContainsKey(index) && this.m_pageTexts[index].Contains(listOfTerm.SearchWord))
            this.FindTextMatches(LoadedDocument, listOfTerm.SearchWord, listOfTerm.SearchOption, index, out matchRect);
          else
            continue;
          foreach (KeyValuePair<int, List<TextProperties>> keyValuePair in matchRect)
          {
            foreach (TextProperties textProperties in keyValuePair.Value)
              this.m_searchObjects.Add(new MatchedItem()
              {
                PageNumber = keyValuePair.Key,
                Bounds = textProperties.Bounds,
                TextColor = textProperties.StrokingBrush,
                Text = listOfTerm.SearchWord
              });
          }
        }
      }
    }
  }

  internal bool SearchText(
    Page page,
    PdfLoadedDocument loadedDocument,
    int pageIndex,
    string searchText,
    out List<Syncfusion.PdfViewer.Base.Glyph> texts)
  {
    this.searchstring = searchText;
    texts = new List<Syncfusion.PdfViewer.Base.Glyph>();
    bool flag = false;
    ImageRenderer.textDictonary.Clear();
    double height1 = (double) page.Height;
    double width1 = (double) page.Width;
    if (page.Rotation == 90.0 || page.Rotation == 270.0)
    {
      double height2 = (double) page.Height;
      double width2 = (double) page.Width;
    }
    Page page1 = page;
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(!(page1.CropBox != RectangleF.Empty) || !(page1.CropBox != page1.MediaBox) ? (Image) new Bitmap((int) ((double) page1.Bounds.Width * (double) this.m_zoomFactor), (int) ((double) page1.Bounds.Height * (double) this.m_zoomFactor)) : (Image) new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page1.CropBox.Width - page1.CropBox.X, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor), (int) ((double) this.m_unitConvertor.ConvertToPixels(page1.CropBox.Height - page1.CropBox.Y, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor))))
    {
      if (this.m_cmyk == null)
        this.m_cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
      if (page.RecordCollection == null)
        page.Initialize(loadedDocument.Pages[pageIndex], true);
      if (page.CropBox != RectangleF.Empty && page.CropBox != page.MediaBox)
        g.TranslateTransform(-this.m_unitConvertor.ConvertToPixels(page.CropBox.X, PdfGraphicsUnit.Point), -this.m_unitConvertor.ConvertToPixels(page.CropBox.Y, PdfGraphicsUnit.Point));
      ImageRenderer imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, false, page.Height, (float) page.CurrentLeftLocation, this.m_cmyk);
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      imageRenderer.pageRotation = (float) page.Rotation;
      imageRenderer.IsTextSearch = true;
      imageRenderer.isFindText = true;
      imageRenderer.RenderAsImage();
      imageRenderer.isFindText = false;
      imageRenderer.IsTextSearch = false;
      Thread.CurrentThread.CurrentCulture = currentCulture;
      foreach (Syncfusion.PdfViewer.Base.Glyph imageRenderGlyph in imageRenderer.imageRenderGlyphList)
      {
        if (imageRenderGlyph.ToString() != "" || !string.IsNullOrEmpty(imageRenderGlyph.ToString()))
          texts.Add(imageRenderGlyph);
      }
    }
    return flag;
  }

  internal bool FindTextMatches(
    PdfLoadedDocument loadedDocument,
    string text,
    TextSearchOptions textSearchOption,
    int pageIndex,
    out Dictionary<int, List<TextProperties>> matchRect)
  {
    bool textMatches = false;
    matchRect = new Dictionary<int, List<TextProperties>>();
    List<TextProperties> textPropertiesList = new List<TextProperties>();
    Page page = new Page(loadedDocument.Pages[pageIndex]);
    page.Initialize(loadedDocument.Pages[pageIndex], false);
    if (pageIndex >= 0)
    {
      string input = (string) null;
      List<int> intList = new List<int>();
      if (this.m_cmyk == null)
        this.m_cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
      List<Syncfusion.PdfViewer.Base.Glyph> texts;
      lock (this.m_lock)
        this.SearchText(page, loadedDocument, pageIndex, text, out texts);
      foreach (Syncfusion.PdfViewer.Base.Glyph glyph in texts)
        input += glyph.ToUnicode;
      if ((textSearchOption & TextSearchOptions.CaseSensitive) != TextSearchOptions.CaseSensitive)
      {
        if (!string.IsNullOrEmpty(text))
          text = text.ToLower();
        if (!string.IsNullOrEmpty(input))
          input = input.ToLower();
      }
      if (input != null && text != null && text != "")
      {
        while (input.Contains(text))
        {
          int num = input.IndexOf(text, StringComparison.Ordinal);
          if ((textSearchOption & TextSearchOptions.WholeWords) == TextSearchOptions.WholeWords)
          {
            if (Regex.IsMatch(input, $"\\b{text}\\b"))
              num = Regex.Match(input, $"\\b{text}\\b").Index;
            else
              break;
          }
          input = input.Substring(num + text.Length);
          if (intList.Count == 0)
            intList.Add(num);
          else
            intList.Add(intList[intList.Count - 1] + num + text.Length);
        }
      }
      if (intList.Count != 0)
      {
        foreach (int index1 in intList)
        {
          double x = texts[index1].BoundingRect._x;
          double y = texts[index1].BoundingRect._y;
          double width1 = 0.0;
          double height = texts[index1].BoundingRect._height;
          if (texts[index1].BoundingRect._y == texts[index1 + text.Length - 1].BoundingRect._y || Math.Abs(texts[index1].BoundingRect._y - texts[index1 + text.Length - 1].BoundingRect._y) < 0.001)
          {
            double width2;
            if (x > texts[index1 + text.Length - 1].BoundingRect._x)
            {
              double num = x - texts[index1 + text.Length - 1].BoundingRect._x + texts[index1 + text.Length - 1].BoundingRect._width;
              if (page.Rotation == 0.0 || page.Rotation == 180.0)
              {
                width2 = texts[index1].BoundingRect._height;
                for (int index2 = 0; index2 < text.Length; ++index2)
                {
                  height += texts[index1 + index2].BoundingRect._width;
                  if (texts[index1 + index2].BoundingRect._height > width2)
                    width2 = texts[index1 + index2].BoundingRect._height;
                }
              }
              else
              {
                width2 = texts[index1].BoundingRect.Width;
                for (int index3 = 0; index3 < text.Length; ++index3)
                {
                  height += texts[index1 + index3].BoundingRect._height;
                  if (texts[index1 + index3].BoundingRect._width > width2)
                    width2 = texts[index1 + index3].BoundingRect._width;
                }
              }
              x -= width2;
            }
            else
              width2 = texts[index1 + text.Length - 1].BoundingRect._x - x + texts[index1 + text.Length - 1].BoundingRect._width;
            RectangleF bounds = new RectangleF((float) x, (float) y, (float) width2, (float) height);
            textPropertiesList.Add(new TextProperties((texts[index1].Stroke as SolidBrush).Color, bounds));
            textMatches = true;
          }
          else if (texts[index1].BoundingRect._x == texts[index1 + text.Length - 1].BoundingRect._x || Math.Abs(texts[index1].BoundingRect._x - texts[index1 + text.Length - 1].BoundingRect._x) < 0.001)
          {
            if (texts[index1].BoundingRect._y != texts[index1 + text.Length - 1].BoundingRect._y && Math.Abs(texts[index1].BoundingRect._y - texts[index1 + text.Length - 1].BoundingRect._y) >= 0.001 || texts[index1].IsRotated)
            {
              height = 0.0;
              double num = 0.0;
              if (page.Rotation == 0.0 || page.Rotation == 180.0)
              {
                width1 = texts[index1].BoundingRect._height;
                for (int index4 = 0; index4 < text.Length; ++index4)
                {
                  height += texts[index1 + index4].BoundingRect._width;
                  if (texts[index1 + index4].BoundingRect._height > num)
                    width1 = texts[index1 + index4].BoundingRect._height;
                }
              }
              else
              {
                width1 = texts[index1].BoundingRect.Width;
                for (int index5 = 0; index5 < text.Length; ++index5)
                {
                  height += texts[index1 + index5].BoundingRect._height;
                  width1 = texts[index1 + index5].BoundingRect._width;
                }
              }
              if (y > texts[index1 + text.Length - 1].BoundingRect._y || texts[index1].RotationAngle == 270)
              {
                x = texts[index1].BoundingRect._x - width1 + 1.0;
                y = texts[index1].BoundingRect._y - height;
              }
              else if (y < texts[index1 + text.Length - 1].BoundingRect._y || texts[index1].RotationAngle == 90)
              {
                x = texts[index1].BoundingRect._x - 1.0;
                y = texts[index1].BoundingRect._y;
              }
            }
            RectangleF bounds = new RectangleF((float) x, (float) y, (float) width1, (float) height + 1f);
            textPropertiesList.Add(new TextProperties((texts[index1].Stroke as SolidBrush).Color, bounds));
            textMatches = true;
          }
        }
      }
    }
    matchRect.Add(pageIndex, textPropertiesList);
    return textMatches;
  }

  internal bool FindTextMatches(
    int pageIndex,
    PdfLoadedDocument loadedDocument,
    string text,
    out List<RectangleF> matchRectangles)
  {
    bool textMatches = false;
    matchRectangles = new List<RectangleF>();
    if (text != "")
    {
      int index1 = pageIndex;
      PdfPageBase page1 = loadedDocument.Pages[index1];
      Page page2 = new Page(page1);
      page2.Initialize(page1, false);
      page2.matchTextPositions.Clear();
      if (index1 < loadedDocument.Pages.Count && index1 >= 0)
      {
        Page page3 = page2;
        Bitmap bitmap = !(page3.CropBox != RectangleF.Empty) || !(page3.CropBox != page3.MediaBox) ? new Bitmap((int) ((double) page3.Bounds.Width * (double) this.m_zoomFactor), (int) ((double) page3.Bounds.Height * (double) this.m_zoomFactor)) : new Bitmap((int) ((double) this.m_unitConvertor.ConvertToPixels(page3.CropBox.Width - page3.CropBox.X, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor), (int) ((double) this.m_unitConvertor.ConvertToPixels(page3.CropBox.Height - page3.CropBox.Y, PdfGraphicsUnit.Point) * (double) this.m_zoomFactor));
        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap))
        {
          if (this.m_cmyk == null)
            this.m_cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
          if (page2.RecordCollection == null)
            page2.Initialize(loadedDocument.Pages[index1], true);
          Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
          ImageRenderer imageRenderer = new ImageRenderer(page2.RecordCollection, page2.Resources, g, false, page2.Height, (float) page2.CurrentLeftLocation, this.m_cmyk);
          ImageRenderer.textDictonary.Clear();
          imageRenderer.pageRotation = (float) page2.Rotation;
          imageRenderer.isFindText = true;
          imageRenderer.RenderAsImage();
          imageRenderer.isFindText = false;
          string str = string.Empty;
          List<int> intList = new List<int>();
          for (int index2 = 0; index2 < imageRenderer.imageRenderGlyphList.Count; ++index2)
          {
            if (imageRenderer.imageRenderGlyphList[index2].ToUnicode == "")
            {
              imageRenderer.imageRenderGlyphList.RemoveAt(index2);
              --index2;
            }
            else
              str += imageRenderer.imageRenderGlyphList[index2].ToUnicode;
          }
          if (text != "")
            text = text.ToLower();
          if (!string.IsNullOrEmpty(str))
            str = str.ToLower();
          if (!string.IsNullOrEmpty(str) && text != "")
          {
            while (str.Contains(text))
            {
              int num = str.IndexOf(text);
              str = str.Substring(num + text.Length);
              if (intList.Count == 0)
                intList.Add(num);
              else
                intList.Add(intList[intList.Count - 1] + num + text.Length);
            }
          }
          if (intList.Count != 0)
          {
            foreach (int index3 in intList)
            {
              double x = imageRenderer.imageRenderGlyphList[index3].BoundingRect._x;
              double y = imageRenderer.imageRenderGlyphList[index3].BoundingRect._y;
              double width1 = 0.0;
              double height = imageRenderer.imageRenderGlyphList[index3].BoundingRect._height;
              if (imageRenderer.imageRenderGlyphList[index3].BoundingRect._y == imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y || Math.Abs(imageRenderer.imageRenderGlyphList[index3].BoundingRect._y - imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y) < 0.001 || imageRenderer.imageRenderGlyphList[index3].BoundingRect.Top <= imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect.Top && imageRenderer.imageRenderGlyphList[index3].BoundingRect.Bottom >= imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect.Bottom)
              {
                double width2 = x <= imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._x ? imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._x - x + imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._width : x - imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._x + imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._width;
                matchRectangles.Add(new RectangleF((float) x, (float) y, (float) width2, (float) height));
                textMatches = true;
              }
              else if (imageRenderer.imageRenderGlyphList[index3].BoundingRect._x == imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._x || Math.Abs(imageRenderer.imageRenderGlyphList[index3].BoundingRect._x - imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._x) < 0.001)
              {
                if (imageRenderer.imageRenderGlyphList[index3].BoundingRect._y != imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y && Math.Abs(imageRenderer.imageRenderGlyphList[index3].BoundingRect._y - imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y) >= 0.001 || imageRenderer.imageRenderGlyphList[index3].IsRotated)
                {
                  height = 0.0;
                  double num = 0.0;
                  if (page2.Rotation == 0.0 || page2.Rotation == 180.0)
                  {
                    width1 = imageRenderer.imageRenderGlyphList[index3].BoundingRect._height;
                    for (int index4 = 0; index4 < text.Length; ++index4)
                    {
                      height += imageRenderer.imageRenderGlyphList[index3 + index4].BoundingRect._width;
                      if (imageRenderer.imageRenderGlyphList[index3 + index4].BoundingRect._height > num)
                        width1 = imageRenderer.imageRenderGlyphList[index3 + index4].BoundingRect._height;
                    }
                  }
                  else
                  {
                    width1 = imageRenderer.imageRenderGlyphList[index3].BoundingRect.Width;
                    for (int index5 = 0; index5 < text.Length; ++index5)
                    {
                      height += imageRenderer.imageRenderGlyphList[index3 + index5].BoundingRect._height;
                      width1 = imageRenderer.imageRenderGlyphList[index3 + index5].BoundingRect._width;
                    }
                  }
                  if (y > imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y || imageRenderer.imageRenderGlyphList[index3].RotationAngle == 270)
                  {
                    x = imageRenderer.imageRenderGlyphList[index3].BoundingRect._x - width1 + 1.0;
                    y = imageRenderer.imageRenderGlyphList[index3].BoundingRect._y - height;
                  }
                  else if (y < imageRenderer.imageRenderGlyphList[index3 + text.Length - 1].BoundingRect._y || imageRenderer.imageRenderGlyphList[index3].RotationAngle == 90)
                  {
                    x = imageRenderer.imageRenderGlyphList[index3].BoundingRect._x - 1.0;
                    y = imageRenderer.imageRenderGlyphList[index3].BoundingRect._y;
                  }
                }
                matchRectangles.Add(new RectangleF((float) x, (float) y, (float) width1, (float) height + 1f));
                textMatches = true;
              }
            }
          }
          imageRenderer.imageRenderGlyphList.Clear();
          imageRenderer.Clear(false);
          page2.Clear();
          page3.Clear();
          GC.Collect();
          GC.WaitForPendingFinalizers();
          int num1 = index1 + 1;
          bitmap.Dispose();
        }
      }
    }
    return textMatches;
  }

  internal bool FindTextMatches(
    PdfLoadedDocument loadedDocument,
    string text,
    out Dictionary<int, List<RectangleF>> matchTextPositionsDict)
  {
    if (text != "")
      this.LoadPagesForTextSearch(loadedDocument);
    bool textMatches = false;
    int num = 0;
    matchTextPositionsDict = new Dictionary<int, List<RectangleF>>();
    foreach (Page page in this.pagesForTextSearch)
      page.matchTextPositions.Clear();
    while (num < this.pagesForTextSearch.Count)
    {
      if (num < this.pagesForTextSearch.Count && num >= 0)
      {
        List<RectangleF> matchRectangles;
        if (this.FindTextMatches(num, loadedDocument, text, out matchRectangles))
          textMatches = true;
        matchTextPositionsDict.Add(num, matchRectangles);
        ++num;
      }
    }
    return textMatches;
  }
}
