// Decompiled with JetBrains decompiler
// Type: PDFKit.ExtractPdfImage.PdfPageImageExtractor
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.ExtractPdfImage;

public static class PdfPageImageExtractor
{
  public static Bitmap ExtractPageImage(
    PdfDocument document,
    int pageIndex,
    PdfPageImageExtractSettings settings)
  {
    using (PdfPageImageExtractor.PdfPageProperties pageProperties = PdfPageImageExtractor.CreatePageProperties(document, pageIndex, settings))
    {
      if (pageProperties != null)
      {
        System.Drawing.Imaging.PixelFormat format1 = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
        System.Drawing.Size pageSizeInPixels = pageProperties.RotatedPageSizeInPixels;
        int width = pageSizeInPixels.Width;
        pageSizeInPixels = pageProperties.RotatedPageSizeInPixels;
        int height = pageSizeInPixels.Height;
        int format2 = (int) format1;
        Bitmap pageImage = new Bitmap(width, height, (System.Drawing.Imaging.PixelFormat) format2);
        try
        {
          pageImage.SetResolution(pageProperties.Settings.ImageDpi, pageProperties.Settings.ImageDpi);
          BitmapData bitmapdata = pageImage.LockBits(new Rectangle(0, 0, pageImage.Width, pageImage.Height), ImageLockMode.WriteOnly, format1);
          try
          {
            using (PdfBitmap pdfBitmap = new PdfBitmap(bitmapdata.Width, bitmapdata.Height, BitmapFormats.FXDIB_Argb, bitmapdata.Scan0, bitmapdata.Stride))
              PdfPageImageExtractor.RenderPage(pdfBitmap, pageProperties, 0, 0, bitmapdata.Width, bitmapdata.Height);
          }
          finally
          {
            pageImage.UnlockBits(bitmapdata);
          }
          return pageImage;
        }
        catch
        {
          pageImage.Dispose();
          throw;
        }
      }
    }
    return (Bitmap) null;
  }

  public static System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<PdfPageRange>> SplitPageRangesByImageSize(
    PdfDocument document,
    System.Collections.Generic.IReadOnlyList<PdfPageRange> pageRange,
    PdfPageImageExtractSingleImageSettings settings,
    IReadOnlyDictionary<int, PdfPageImageExtractPageSettings> pageSettings = null,
    PdfPageImageExtractor.PageProcessingEventHandler onPageProcessing = null)
  {
    return (System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<PdfPageRange>>) PdfPageImageExtractor.SplitPageRangesByImageSizeCore(document, pageRange, settings, pageSettings, onPageProcessing).Select<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int), System.Collections.Generic.IReadOnlyList<PdfPageRange>>((Func<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int), System.Collections.Generic.IReadOnlyList<PdfPageRange>>) (c => c.pageRanges)).ToArray<System.Collections.Generic.IReadOnlyList<PdfPageRange>>();
  }

  public static unsafe PdfPageImageExtractor.ExtractResult ExtractPagesIntoSingleImage(
    PdfDocument document,
    System.Collections.Generic.IReadOnlyList<PdfPageRange> pageRange,
    PdfPageImageExtractSingleImageSettings settings,
    IReadOnlyDictionary<int, PdfPageImageExtractPageSettings> pageSettings = null,
    PdfPageImageExtractor.PageProcessingEventHandler onPageProcessing = null)
  {
    if (document == null || pageRange == null || pageRange.Count == 0)
      return (PdfPageImageExtractor.ExtractResult) null;
    settings = settings ?? new PdfPageImageExtractSingleImageSettings();
    System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange> pageRanges, int width, int height)> tupleList = PdfPageImageExtractor.SplitPageRangesByImageSizeCore(document, pageRange, settings, pageSettings, onPageProcessing);
    if (tupleList.Count != 1 || tupleList[0].width <= 0 || tupleList[0].height <= 0)
      return (PdfPageImageExtractor.ExtractResult) null;
    int totalPageCount = 0;
    int width1 = tupleList[0].width;
    int height1 = tupleList[0].height;
    PdfPageRange pdfPageRange;
    for (int index = 0; index < pageRange.Count; ++index)
    {
      int num = totalPageCount;
      pdfPageRange = pageRange[index];
      int count = pdfPageRange.Count;
      totalPageCount = num + count;
    }
    Bitmap bitmap = (Bitmap) null;
    MemoryMappedFile memoryMappedFile = (MemoryMappedFile) null;
    MemoryMappedViewAccessor memoryMappedViewAccessor = (MemoryMappedViewAccessor) null;
    IntPtr num1 = IntPtr.Zero;
    try
    {
      if (!string.IsNullOrEmpty(settings.TempFileFullName))
      {
        memoryMappedFile = MemoryMappedFile.CreateFromFile(settings.TempFileFullName, FileMode.CreateNew, $"{Guid.NewGuid():N}", (long) (width1 * height1 * 4), MemoryMappedFileAccess.ReadWrite);
        memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor();
        byte* pointer = (byte*) null;
        memoryMappedViewAccessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
        num1 = (IntPtr) (void*) pointer;
      }
      bitmap = !(num1 == IntPtr.Zero) ? new Bitmap(width1, height1, width1 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, num1) : new Bitmap(width1, height1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
      if (settings.ImageBackgroundColor.A > (byte) 0)
      {
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          graphics.Clear(settings.ImageBackgroundColor);
      }
      bitmap.SetResolution(settings.ImageDpi, settings.ImageDpi);
      int x = 0;
      int y = 0;
      int pageIndexInSequence = 0;
      for (int index = 0; index < pageRange.Count; ++index)
      {
        pdfPageRange = pageRange[index];
        foreach (int num2 in pdfPageRange)
        {
          if (onPageProcessing != null)
          {
            PdfPageImageExtractor.PageProcessingEventArgs args = new PdfPageImageExtractor.PageProcessingEventArgs(PdfPageImageExtractor.PageProcessType.Render, num2, pageIndexInSequence, totalPageCount);
            onPageProcessing(args);
            if (args.Cancel)
              return (PdfPageImageExtractor.ExtractResult) null;
          }
          using (PdfPageImageExtractor.PdfPageProperties pageProperties = PdfPageImageExtractor.CreatePageProperties(document, num2, PdfPageImageExtractor.GetPageSettings(num2, settings, pageSettings)))
          {
            System.Drawing.Size pageSizeInPixels = pageProperties.RotatedPageSizeInPixels;
            int width2 = pageSizeInPixels.Width;
            pageSizeInPixels = pageProperties.RotatedPageSizeInPixels;
            int height2 = pageSizeInPixels.Height;
            if (pageIndexInSequence == 0)
            {
              int width3;
              int height3;
              if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
              {
                width3 = settings.BorderThickness;
                height3 = height1;
                x += settings.BorderThickness;
              }
              else
              {
                width3 = width1;
                height3 = settings.BorderThickness;
                y += settings.BorderThickness;
              }
              if (width3 > 0 && height3 > 0 && settings.BorderColor.A > (byte) 0)
              {
                BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width3, height3), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                try
                {
                  using (PdfBitmap pdfBitmap = new PdfBitmap(bitmapdata.Width, bitmapdata.Height, BitmapFormats.FXDIB_Argb, bitmapdata.Scan0, bitmapdata.Stride))
                    PdfPageImageExtractor.FillRect(pdfBitmap, 0, 0, width3, height3, settings.BorderColor);
                }
                finally
                {
                  bitmap.UnlockBits(bitmapdata);
                }
              }
            }
            int width4;
            int height4;
            if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
            {
              width4 = width2 + settings.BorderThickness;
              height4 = height1;
            }
            else
            {
              width4 = width1;
              height4 = height2 + settings.BorderThickness;
            }
            int offsetX = 0;
            int offsetY = 0;
            if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
              offsetY = (height4 - height2) / 2;
            else
              offsetX = (width4 - width2) / 2;
            BitmapData bitmapdata1 = bitmap.LockBits(new Rectangle(x, y, width4, height4), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
              using (PdfBitmap pdfBitmap = new PdfBitmap(bitmapdata1.Width, bitmapdata1.Height, BitmapFormats.FXDIB_Argb, bitmapdata1.Scan0, bitmapdata1.Stride))
              {
                if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
                {
                  PdfPageImageExtractor.FillRect(pdfBitmap, 0, 0, height4 - settings.BorderThickness, settings.BorderThickness, settings.BorderColor);
                  PdfPageImageExtractor.FillRect(pdfBitmap, 0, height4 - settings.BorderThickness, height4 - settings.BorderThickness, settings.BorderThickness, settings.BorderColor);
                  PdfPageImageExtractor.FillRect(pdfBitmap, width4 - settings.BorderThickness, 0, settings.BorderThickness, height1, settings.BorderColor);
                }
                else
                {
                  PdfPageImageExtractor.FillRect(pdfBitmap, 0, 0, settings.BorderThickness, height4 - settings.BorderThickness, settings.BorderColor);
                  PdfPageImageExtractor.FillRect(pdfBitmap, width4 - settings.BorderThickness, 0, settings.BorderThickness, height4 - settings.BorderThickness, settings.BorderColor);
                  PdfPageImageExtractor.FillRect(pdfBitmap, 0, height4 - settings.BorderThickness, width4, settings.BorderThickness, settings.BorderColor);
                }
                PdfPageImageExtractor.RenderPage(pdfBitmap, pageProperties, offsetX, offsetY, width2, height2);
              }
            }
            finally
            {
              bitmap.UnlockBits(bitmapdata1);
            }
            if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
              x += width4;
            else
              y += height4;
          }
          ++pageIndexInSequence;
        }
      }
      return memoryMappedFile != null ? new PdfPageImageExtractor.ExtractResult(bitmap, memoryMappedFile, memoryMappedViewAccessor, num1) : new PdfPageImageExtractor.ExtractResult(bitmap);
    }
    catch
    {
      bitmap?.Dispose();
      if (num1 != IntPtr.Zero)
      {
        IntPtr zero = IntPtr.Zero;
        memoryMappedViewAccessor.SafeMemoryMappedViewHandle.ReleasePointer();
      }
      memoryMappedViewAccessor?.Dispose();
      memoryMappedFile?.Dispose();
      throw;
    }
  }

  public static System.Drawing.Size GetImageSizeInPixels(
    PdfDocument document,
    int pageIndex,
    PdfPageImageExtractSettings settings)
  {
    if (settings != null && (double) settings.ImageDpi <= 0.0)
      return System.Drawing.Size.Empty;
    using (PdfPageImageExtractor.PdfPageProperties pageProperties = PdfPageImageExtractor.CreatePageProperties(document, pageIndex, settings))
      return pageProperties != null ? pageProperties.RotatedPageSizeInPixels : System.Drawing.Size.Empty;
  }

  public static BitmapSource GeneratePreview(
    PdfDocument document,
    int pageIndex,
    PdfPageImageExtractSettings settings)
  {
    using (PdfPageImageExtractor.PdfPageProperties pageProperties = PdfPageImageExtractor.CreatePageProperties(document, pageIndex, settings))
    {
      if (pageProperties != null)
      {
        WriteableBitmap preview = new WriteableBitmap(pageProperties.RotatedPageSizeInPixels.Width, pageProperties.RotatedPageSizeInPixels.Height, (double) pageProperties.Settings.ImageDpi, (double) pageProperties.Settings.ImageDpi, PixelFormats.Bgra32, (BitmapPalette) null);
        preview.Lock();
        try
        {
          using (PdfBitmap pdfBitmap = new PdfBitmap(preview.PixelWidth, preview.PixelHeight, BitmapFormats.FXDIB_Argb, preview.BackBuffer, preview.BackBufferStride))
            PdfPageImageExtractor.RenderPage(pdfBitmap, pageProperties, 0, 0, preview.PixelWidth, preview.PixelHeight);
          preview.AddDirtyRect(new Int32Rect(0, 0, preview.PixelWidth, preview.PixelHeight));
        }
        finally
        {
          preview.Unlock();
        }
        return (BitmapSource) preview;
      }
    }
    return (BitmapSource) null;
  }

  private static System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange> pageRanges, int width, int height)> SplitPageRangesByImageSizeCore(
    PdfDocument document,
    System.Collections.Generic.IReadOnlyList<PdfPageRange> pageRange,
    PdfPageImageExtractSingleImageSettings settings,
    IReadOnlyDictionary<int, PdfPageImageExtractPageSettings> pageSettings,
    PdfPageImageExtractor.PageProcessingEventHandler onPageProcessing)
  {
    int val1_1 = 0;
    int val1_2 = 0;
    int totalPageCount = 0;
    if (document == null || pageRange == null || pageRange.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)>) null;
    PdfPageRange pdfPageRange;
    for (int index = 0; index < pageRange.Count; ++index)
    {
      int num = totalPageCount;
      pdfPageRange = pageRange[index];
      int count = pdfPageRange.Count;
      totalPageCount = num + count;
    }
    settings = settings ?? new PdfPageImageExtractSingleImageSettings();
    int pageIndexInSequence = 0;
    List<int> pageIndexes = new List<int>();
    List<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)> valueTupleList = new List<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)>();
    for (int index = 0; index < pageRange.Count; ++index)
    {
      pdfPageRange = pageRange[index];
      foreach (int num in pdfPageRange)
      {
        if (onPageProcessing != null)
        {
          PdfPageImageExtractor.PageProcessingEventArgs args = new PdfPageImageExtractor.PageProcessingEventArgs(PdfPageImageExtractor.PageProcessType.Prepare, num, pageIndexInSequence, totalPageCount);
          onPageProcessing(args);
          if (args.Cancel)
            return (System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)>) null;
        }
        System.Drawing.Size imageSizeInPixels = PdfPageImageExtractor.GetImageSizeInPixels(document, num, PdfPageImageExtractor.GetPageSettings(num, settings, pageSettings));
        int width = imageSizeInPixels.Width;
        int height = imageSizeInPixels.Height;
        if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
        {
          if (val1_1 == 0)
            val1_1 = settings.BorderThickness;
          width += settings.BorderThickness;
          val1_2 = Math.Max(val1_2, imageSizeInPixels.Height + 2 * settings.BorderThickness);
          val1_1 += width;
        }
        else
        {
          if (val1_2 == 0)
            val1_2 = settings.BorderThickness;
          height += settings.BorderThickness;
          val1_1 = Math.Max(val1_1, imageSizeInPixels.Width + 2 * settings.BorderThickness);
          val1_2 += height;
        }
        if (val1_1 > (int) ushort.MaxValue || val1_2 > (int) ushort.MaxValue)
        {
          if (pageIndexes.Count == 0)
            return (System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)>) null;
          valueTupleList.Add((pageIndexes.ToPageRange(), val1_1, val1_2));
          pageIndexes.Clear();
          if (settings.ExtractIntoSingleImageOrientation == PdfPageImageExtractOrientation.Horizontal)
          {
            val1_1 = width + settings.BorderThickness;
            val1_2 = height;
          }
          else
          {
            val1_1 = width;
            val1_2 = height + settings.BorderThickness;
          }
        }
        ++pageIndexInSequence;
        pageIndexes.Add(num);
      }
    }
    if (pageIndexes.Count > 0)
      valueTupleList.Add((pageIndexes.ToPageRange(), val1_1, val1_2));
    return (System.Collections.Generic.IReadOnlyList<(System.Collections.Generic.IReadOnlyList<PdfPageRange>, int, int)>) valueTupleList;
  }

  private static PdfPageImageExtractSettings GetPageSettings(
    int pageIndex,
    PdfPageImageExtractSingleImageSettings settings,
    IReadOnlyDictionary<int, PdfPageImageExtractPageSettings> pageSettings)
  {
    PdfPageImageExtractSettings pageSettings1 = (PdfPageImageExtractSettings) settings;
    PdfPageImageExtractPageSettings extractPageSettings;
    if (pageSettings != null && pageSettings.TryGetValue(pageIndex, out extractPageSettings) && extractPageSettings != null)
      pageSettings1 = extractPageSettings.WithGlobalSettings(settings);
    return pageSettings1;
  }

  private static void RenderPage(
    PdfBitmap pdfBitmap,
    PdfPageImageExtractor.PdfPageProperties properties,
    int offsetX,
    int offsetY,
    int width,
    int height,
    bool clearType = false)
  {
    if (width == 0)
      width = properties.RotatedPageSizeInPixels.Width;
    if (height == 0)
      height = properties.RotatedPageSizeInPixels.Height;
    RenderFlags flags = RenderFlags.FPDF_NONE;
    if (clearType)
      flags |= RenderFlags.FPDF_LCD_TEXT;
    if (properties.Settings.ColorMode == PdfPageImageExtractColorMode.Gray)
      flags |= RenderFlags.FPDF_GRAYSCALE;
    if (properties.FormFillHandle == IntPtr.Zero && properties.Settings.RenderAnnotations)
      flags |= RenderFlags.FPDF_ANNOT;
    if (properties.Settings.PageBackgroundColor.A > (byte) 0)
      Pdfium.FPDFBitmap_FillRect(pdfBitmap.Handle, offsetX, offsetY, width, height, PdfPageImageExtractor.ToArgb(properties.Settings.PageBackgroundColor));
    Pdfium.FPDF_RenderPageBitmap(pdfBitmap.Handle, properties.PageHandle, offsetX, offsetY, width, height, properties.AdditionalRotate, flags);
    if (!(properties.FormFillHandle != IntPtr.Zero) || !properties.Settings.RenderAnnotations)
      return;
    Pdfium.FPDF_FFLDraw(properties.FormFillHandle, pdfBitmap.Handle, properties.PageHandle, offsetX, offsetY, width, height, properties.AdditionalRotate, flags);
  }

  private static void FillRect(
    PdfBitmap pdfBitmap,
    int offsetX,
    int offsetY,
    int width,
    int height,
    System.Drawing.Color color)
  {
    if (color.A <= (byte) 0 || width <= 0 || height <= 0)
      return;
    Pdfium.FPDFBitmap_FillRect(pdfBitmap.Handle, offsetX, offsetY, width, height, PdfPageImageExtractor.ToArgb(color));
  }

  private static int ToArgb(System.Drawing.Color color)
  {
    return (int) color.A << 24 | (int) color.R << 16 /*0x10*/ | (int) color.G << 8 | (int) color.B;
  }

  private static PdfPageImageExtractor.PdfPageProperties CreatePageProperties(
    PdfDocument document,
    int pageIndex,
    PdfPageImageExtractSettings settings)
  {
    settings = settings ?? new PdfPageImageExtractSettings();
    IntPtr num = Pdfium.FPDF_LoadPage(document.Handle, pageIndex);
    if (!(num != IntPtr.Zero))
      return (PdfPageImageExtractor.PdfPageProperties) null;
    PageRotate rotation = Pdfium.FPDFPage_GetRotation(num);
    PageRotate pageRotate = settings.GetPageRotate(rotation);
    FS_SIZEF effectiveSize = PdfLocationUtils.GetEffectiveSize(document.Handle, num, pageIndex, pageRotate, true);
    PdfForms formFill = document.FormFill;
    IntPtr formFillHandle = formFill != null ? formFill.Handle : IntPtr.Zero;
    return new PdfPageImageExtractor.PdfPageProperties(document.Handle, formFillHandle, pageIndex, num, rotation, pageRotate, effectiveSize, settings);
  }

  private class PdfPageProperties : IDisposable
  {
    private bool disposedValue;
    private IntPtr documentHandle;
    private IntPtr formFillHandle;
    private int pageIndex;
    private IntPtr pageHandle;
    private PageRotate originalRotate;
    private PageRotate finalRotate;
    private FS_SIZEF rotatedPageSize;
    private PdfPageImageExtractSettings settings;

    public PdfPageProperties(
      IntPtr documentHandle,
      IntPtr formFillHandle,
      int pageIndex,
      IntPtr pageHandle,
      PageRotate originalRotate,
      PageRotate finalRotate,
      FS_SIZEF rotatedPageSize,
      PdfPageImageExtractSettings settings)
    {
      this.documentHandle = documentHandle;
      this.formFillHandle = formFillHandle;
      this.pageHandle = pageHandle;
      this.originalRotate = originalRotate;
      this.finalRotate = finalRotate;
      this.rotatedPageSize = rotatedPageSize;
      this.settings = settings;
    }

    public IntPtr DocumentHandle
    {
      get => this.disposedValue ? this.ThrowObjectDisposedException<IntPtr>() : this.documentHandle;
    }

    public IntPtr FormFillHandle
    {
      get => this.disposedValue ? this.ThrowObjectDisposedException<IntPtr>() : this.formFillHandle;
    }

    public int PageIndex
    {
      get => this.disposedValue ? this.ThrowObjectDisposedException<int>() : this.pageIndex;
    }

    public IntPtr PageHandle
    {
      get => this.disposedValue ? this.ThrowObjectDisposedException<IntPtr>() : this.pageHandle;
    }

    public PageRotate OriginalRotate
    {
      get
      {
        return this.disposedValue ? this.ThrowObjectDisposedException<PageRotate>() : this.originalRotate;
      }
    }

    public PageRotate FinalRotate
    {
      get
      {
        return this.disposedValue ? this.ThrowObjectDisposedException<PageRotate>() : this.finalRotate;
      }
    }

    public PageRotate AdditionalRotate
    {
      get
      {
        return this.disposedValue ? this.ThrowObjectDisposedException<PageRotate>() : (PageRotate) this.settings.AdditionalRotation;
      }
    }

    public FS_SIZEF RotatedPageSize
    {
      get
      {
        return this.disposedValue ? this.ThrowObjectDisposedException<FS_SIZEF>() : this.rotatedPageSize;
      }
    }

    public PdfPageImageExtractSettings Settings
    {
      get
      {
        return this.disposedValue ? this.ThrowObjectDisposedException<PdfPageImageExtractSettings>() : this.settings;
      }
    }

    public System.Drawing.Size RotatedPageSizeInPixels
    {
      get
      {
        return new System.Drawing.Size((int) ((double) this.RotatedPageSize.Width * (double) this.settings.ImageDpi / 72.0), (int) ((double) this.RotatedPageSize.Height * (double) this.settings.ImageDpi / 72.0));
      }
    }

    private T ThrowObjectDisposedException<T>()
    {
      throw new ObjectDisposedException(nameof (PdfPageProperties));
    }

    public void Dispose()
    {
      if (this.disposedValue)
        return;
      IntPtr pageHandle = this.pageHandle;
      this.pageHandle = IntPtr.Zero;
      if (pageHandle != IntPtr.Zero)
        Pdfium.FPDF_ClosePage(pageHandle);
      this.disposedValue = true;
    }
  }

  public class ExtractResult : IDisposable
  {
    private bool disposedValue;

    internal ExtractResult(Bitmap bitmap) => this.Bitmap = bitmap;

    internal ExtractResult(
      Bitmap bitmap,
      MemoryMappedFile memoryMappedFile,
      MemoryMappedViewAccessor memoryMappedViewAccessor,
      IntPtr pointer)
    {
      this.Bitmap = bitmap;
      this.MemoryMappedFile = memoryMappedFile;
      this.MemoryMappedViewAccessor = memoryMappedViewAccessor;
      this.Pointer = pointer;
    }

    public Bitmap Bitmap { get; private set; }

    public MemoryMappedFile MemoryMappedFile { get; private set; }

    public MemoryMappedViewAccessor MemoryMappedViewAccessor { get; private set; }

    public IntPtr Pointer { get; private set; }

    public void Dispose()
    {
      if (this.disposedValue)
        return;
      this.Bitmap?.Dispose();
      this.Bitmap = (Bitmap) null;
      if (this.MemoryMappedViewAccessor != null)
      {
        this.MemoryMappedViewAccessor.SafeMemoryMappedViewHandle.ReleasePointer();
        this.MemoryMappedViewAccessor.Dispose();
        this.Pointer = IntPtr.Zero;
      }
      this.MemoryMappedFile?.Dispose();
      this.MemoryMappedFile = (MemoryMappedFile) null;
      this.disposedValue = true;
    }
  }

  public class PageProcessingEventArgs
  {
    internal PageProcessingEventArgs(
      PdfPageImageExtractor.PageProcessType pageProcessType,
      int pageIndexInDocument,
      int pageIndexInSequence,
      int totalPageCount)
    {
      this.PageProcessType = pageProcessType;
      this.PageIndexInDocument = pageIndexInDocument;
      this.PageIndexInSequence = pageIndexInSequence;
      this.TotalPageCount = totalPageCount;
    }

    public bool Cancel { get; set; }

    public PdfPageImageExtractor.PageProcessType PageProcessType { get; }

    public int PageIndexInDocument { get; }

    public int PageIndexInSequence { get; }

    public int TotalPageCount { get; }
  }

  public enum PageProcessType
  {
    Prepare,
    Render,
  }

  public delegate void PageProcessingEventHandler(PdfPageImageExtractor.PageProcessingEventArgs args);
}
