// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.ImagePdfGenerator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.GenerateImagePdf;

public class ImagePdfGenerator
{
  public ImagePdfGenerator() => this.DefaultSettings = new ImagePdfGenerateSettings();

  public ImagePdfGenerateSettings DefaultSettings { get; set; }

  public async Task<bool> GeneratePdfStreamAsync(
    Stream outputStream,
    System.Collections.Generic.IReadOnlyList<ImagePdfGenerateItem> items,
    IProgress<double> progress,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    bool pdfStreamAsync;
    using (PdfDocument pdfDocument = PdfDocument.CreateNew())
    {
      bool flag = false;
      for (int i = 0; i < items.Count; ++i)
      {
        int pageIdx = pdfDocument.Pages.Count;
        PdfPage page = pdfDocument.Pages.InsertPageAt(pageIdx, 100f, 100f);
        if (await this.DrawImageToPageAsync(page, items[i], cancellationToken))
        {
          page.GenerateContent(true);
          flag = true;
        }
        else
          pdfDocument.Pages.DeleteAt(pageIdx);
        progress?.Report(((double) i + 1.0) / (double) items.Count * 0.9);
        page = (PdfPage) null;
      }
      if (flag)
      {
        pdfDocument.Save(outputStream, SaveFlags.NoIncremental | SaveFlags.RemoveUnusedObjects);
        progress?.Report(1.0);
      }
      pdfStreamAsync = flag;
    }
    return pdfStreamAsync;
  }

  public async Task<bool> DrawImageToPageAsync(
    PdfPage pdfPage,
    ImagePdfGenerateItem item,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (item == null)
      return false;
    ImagePdfGenerateSettings settings = (item.Settings ?? this.DefaultSettings ?? new ImagePdfGenerateSettings())?.Clone();
    int decodeSize = this.GetImageDecodeSize(settings, 1.0);
    Bitmap loadedBitmap = await item.BitmapImageSource.CreateAsync(decodeSize, decodeSize, cancellationToken);
    Bitmap bitmap = (Bitmap) null;
    try
    {
      if (loadedBitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
      {
        bitmap = loadedBitmap;
      }
      else
      {
        bitmap = new Bitmap(loadedBitmap.Width, loadedBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage((Image) bitmap))
          g.DrawImage((Image) loadedBitmap, new Rectangle(0, 0, loadedBitmap.Width, loadedBitmap.Height));
      }
      SizeF pageSize;
      RectangleF imageBounds;
      int imageRotate;
      if (ImagePdfGenerator.GetPaperAndImageBounds(bitmap, settings, out pageSize, out imageBounds, out imageRotate))
      {
        FS_RECTF imageBounds2 = new FS_RECTF(imageBounds.Left, pageSize.Height - imageBounds.Top, imageBounds.Right, pageSize.Height - imageBounds.Bottom);
        pdfPage.MediaBox = new FS_RECTF(0.0f, pageSize.Height, pageSize.Width, 0.0f);
        pdfPage.SetPageCropBox(pdfPage.MediaBox);
        using (PdfBitmap pdfBitmap = PdfBitmap.FromBitmap(bitmap))
        {
          int imageWidth = bitmap.Width;
          int imageHeight = bitmap.Height;
          if (imageRotate == 90 || imageRotate == 270)
          {
            int tmp = imageWidth;
            imageWidth = imageHeight;
            imageHeight = tmp;
          }
          FS_MATRIX matrix = new FS_MATRIX((float) bitmap.Width, 0.0f, 0.0f, (float) bitmap.Height, 0.0f, 0.0f);
          matrix.Translate(-bitmap.Width / 2, -bitmap.Height / 2);
          matrix.Rotate((float) ((double) (360 - imageRotate) * Math.PI / 180.0));
          matrix.Scale(imageBounds2.Width / (float) imageWidth, imageBounds2.Height / (float) imageHeight);
          matrix.Translate(imageBounds2.left + imageBounds2.Width / 2f, imageBounds2.bottom + imageBounds.Height / 2f);
          PdfImageObject imageObj = PdfImageObject.Create(pdfPage.Document, pdfBitmap, 0.0f, 0.0f);
          imageObj.Matrix = matrix;
          pdfPage.PageObjects.Add((PdfPageObject) imageObj);
          return true;
        }
      }
      pageSize = new SizeF();
      imageBounds = new RectangleF();
    }
    finally
    {
      if (loadedBitmap != bitmap)
        bitmap?.Dispose();
      bitmap = (Bitmap) null;
      loadedBitmap?.Dispose();
      loadedBitmap = (Bitmap) null;
    }
    return false;
  }

  public async Task<ImageSource> CreatePreviewImageAsync(
    ImagePdfGenerateItem item,
    double pixelsPerDip,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (item == null)
      return (ImageSource) null;
    ImagePdfGenerateSettings settings = (item.Settings ?? this.DefaultSettings ?? new ImagePdfGenerateSettings())?.Clone();
    int decodeSize = this.GetImageDecodeSize(settings, pixelsPerDip);
    Bitmap loadedBitmap = await item.BitmapImageSource.CreateAsync(decodeSize, decodeSize, cancellationToken);
    Bitmap bitmap = (Bitmap) null;
    try
    {
      if (loadedBitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
      {
        bitmap = loadedBitmap;
      }
      else
      {
        bitmap = new Bitmap(loadedBitmap.Width, loadedBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage((Image) bitmap))
          g.DrawImage((Image) loadedBitmap, new Rectangle(0, 0, loadedBitmap.Width, loadedBitmap.Height));
      }
      WriteableBitmap writeableImage = ImagePdfGenerator.CreateWriteableBitmap(bitmap);
      if (writeableImage != null)
      {
        SizeF pageSize;
        RectangleF imageBounds;
        int imageRotate;
        if (ImagePdfGenerator.GetPaperAndImageBounds(bitmap, settings, out pageSize, out imageBounds, out imageRotate))
        {
          DrawingVisual drawingVisual = new DrawingVisual();
          using (DrawingContext drawingContext = drawingVisual.RenderOpen())
          {
            drawingContext.PushTransform((Transform) new ScaleTransform(pixelsPerDip, pixelsPerDip));
            drawingContext.DrawRectangle((System.Windows.Media.Brush) System.Windows.Media.Brushes.Transparent, (System.Windows.Media.Pen) null, new Rect(0.0, 0.0, (double) pageSize.Width, (double) pageSize.Height));
            drawingContext.Pop();
            int imageWidth = bitmap.Width;
            int imageHeight = bitmap.Height;
            if (imageRotate == 90 || imageRotate == 270)
            {
              int tmp = imageWidth;
              imageWidth = imageHeight;
              imageHeight = tmp;
            }
            Matrix matrix = Matrix.Identity;
            matrix.Translate((double) (-bitmap.Width / 2), (double) (-bitmap.Height / 2));
            matrix.Rotate((double) imageRotate);
            matrix.Scale((double) imageBounds.Width / (double) imageWidth, (double) imageBounds.Height / (double) imageHeight);
            matrix.Translate((double) imageBounds.Left + (double) imageBounds.Width / 2.0, (double) imageBounds.Top + (double) imageBounds.Height / 2.0);
            drawingContext.PushTransform((Transform) new MatrixTransform(matrix));
            drawingContext.DrawImage((ImageSource) writeableImage, new Rect(0.0, 0.0, writeableImage.Width, writeableImage.Height));
            matrix = new Matrix();
          }
          RenderTargetBitmap rtb = new RenderTargetBitmap((int) ((double) pageSize.Width * pixelsPerDip), (int) ((double) pageSize.Height * pixelsPerDip), 96.0 * pixelsPerDip, 96.0 * pixelsPerDip, PixelFormats.Pbgra32);
          rtb.Render((Visual) drawingVisual);
          return (ImageSource) rtb;
        }
        pageSize = new SizeF();
        imageBounds = new RectangleF();
      }
      writeableImage = (WriteableBitmap) null;
    }
    finally
    {
      if (loadedBitmap != bitmap)
        bitmap?.Dispose();
      bitmap = (Bitmap) null;
      loadedBitmap?.Dispose();
      loadedBitmap = (Bitmap) null;
    }
    return (ImageSource) null;
  }

  private static unsafe WriteableBitmap CreateWriteableBitmap(Bitmap bitmap)
  {
    WriteableBitmap writeableBitmap = new WriteableBitmap(bitmap.Width, bitmap.Height, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
    writeableBitmap.Lock();
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
    try
    {
      if (new Span<byte>((void*) bitmapdata.Scan0, bitmapdata.Stride * bitmapdata.Height).TryCopyTo(new Span<byte>((void*) writeableBitmap.BackBuffer, writeableBitmap.BackBufferStride * bitmapdata.Height)))
      {
        writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.Width, bitmap.Height));
        return writeableBitmap;
      }
    }
    finally
    {
      writeableBitmap.Unlock();
      bitmap.UnlockBits(bitmapdata);
    }
    return (WriteableBitmap) null;
  }

  private int GetImageDecodeSize(ImagePdfGenerateSettings settings, double pixelsPerDip)
  {
    if (!settings.PaperSize.HasValue)
      return 0;
    ImagePdfGeneratePaperMargin paperMargin = settings.PaperMargin;
    double pixel1 = (double) paperMargin.Left.Pixel;
    paperMargin = settings.PaperMargin;
    double pixel2 = (double) paperMargin.Right.Pixel;
    double val1_1 = pixel1 + pixel2;
    paperMargin = settings.PaperMargin;
    double pixel3 = (double) paperMargin.Bottom.Pixel;
    paperMargin = settings.PaperMargin;
    double pixel4 = (double) paperMargin.Bottom.Pixel;
    double val2_1 = pixel3 + pixel4;
    float num = Math.Min((float) val1_1, (float) val2_1);
    SizeF? paperSize = settings.PaperSize;
    double val1_2 = (double) paperSize.Value.Width - (double) num;
    paperSize = settings.PaperSize;
    double val2_2 = (double) paperSize.Value.Height - (double) num;
    return (int) ((double) Math.Max(Math.Max((float) val1_2, (float) val2_2), 0.0f) * pixelsPerDip);
  }

  private static bool GetPaperAndImageBounds(
    Bitmap image,
    ImagePdfGenerateSettings settings,
    out SizeF pageSize,
    out RectangleF imageBounds,
    out int imageRotate)
  {
    pageSize = new SizeF();
    imageBounds = new RectangleF();
    imageRotate = 0;
    try
    {
      ImagePdfGeneratePaperOrientation paperOrientation = settings.PaperOrientation;
      if (paperOrientation == ImagePdfGeneratePaperOrientation.Auto)
        paperOrientation = image.Width > image.Height ? ImagePdfGeneratePaperOrientation.Landscape : ImagePdfGeneratePaperOrientation.Portrait;
      if (settings.PaperSize.HasValue)
      {
        pageSize = settings.PaperSize.Value;
        if (paperOrientation == ImagePdfGeneratePaperOrientation.Landscape && (double) pageSize.Width < (double) pageSize.Height)
          pageSize = new SizeF(pageSize.Height, pageSize.Width);
        else if (paperOrientation == ImagePdfGeneratePaperOrientation.Portrait && (double) pageSize.Width > (double) pageSize.Height)
          pageSize = new SizeF(pageSize.Height, pageSize.Width);
        if (settings.Rotate180Degrees)
          imageRotate = 180;
      }
      else
      {
        float imageDpi = settings.ImageDpi;
        float val1 = (float) image.Width * 72f / imageDpi;
        float val2 = (float) image.Height * 72f / imageDpi;
        float num1;
        float num2;
        if (paperOrientation == ImagePdfGeneratePaperOrientation.Landscape)
        {
          num1 = Math.Max(val1, val2);
          num2 = Math.Min(val1, val2);
        }
        else
        {
          num1 = Math.Min(val1, val2);
          num2 = Math.Max(val1, val2);
        }
        pageSize = new SizeF(num1 + settings.PaperMargin.Left.Pixel + settings.PaperMargin.Right.Pixel, num2 + settings.PaperMargin.Top.Pixel + settings.PaperMargin.Bottom.Pixel);
        if (paperOrientation == ImagePdfGeneratePaperOrientation.Landscape && image.Width < image.Height)
          imageRotate = settings.Rotate180Degrees ? 270 : 90;
        else if (paperOrientation == ImagePdfGeneratePaperOrientation.Portrait && image.Width > image.Height)
          imageRotate = settings.Rotate180Degrees ? 270 : 90;
      }
      float imageWidth = (float) image.Width * 72f / settings.ImageDpi;
      float imageHeight = (float) image.Height * 72f / settings.ImageDpi;
      if (imageRotate == 90 || imageRotate == 270)
      {
        float num = imageWidth;
        imageWidth = imageHeight;
        imageHeight = num;
      }
      imageBounds = ImagePdfGenerator.GetImageBounds(imageWidth, imageHeight, pageSize.Width, pageSize.Height, settings);
      return !imageBounds.IsEmpty;
    }
    catch
    {
    }
    return false;
  }

  private static RectangleF GetImageBounds(
    float imageWidth,
    float imageHeight,
    float pageRenderWidth,
    float pageRenderHeight,
    ImagePdfGenerateSettings settings)
  {
    ImagePdfGenerateImageStretch imageStretch = settings.ImageStretch;
    ImageHorizontalAlignment horizontalAlignment = settings.ImageHorizontalAlignment;
    ImageVerticalAlignment verticalAlignment = settings.ImageVerticalAlignment;
    ImagePdfGeneratePaperMargin paperMargin = settings.PaperMargin;
    float num1 = pageRenderWidth - paperMargin.Left.Pixel - paperMargin.Right.Pixel;
    float num2 = pageRenderHeight - paperMargin.Top.Pixel - paperMargin.Bottom.Pixel;
    if ((double) num1 <= 0.0 || (double) num2 <= 0.0)
      return RectangleF.Empty;
    double width = (double) imageWidth;
    double height = (double) imageHeight;
    bool flag = false;
    switch (imageStretch)
    {
      case ImagePdfGenerateImageStretch.Uniform:
        flag = true;
        break;
      case ImagePdfGenerateImageStretch.UniformIfNeeded:
        if (width > (double) num1 || height > (double) num2)
          flag = true;
        break;
    }
    if (flag)
    {
      width = (double) num1;
      height = (double) imageHeight / (double) imageWidth * (double) num1;
      if (height > (double) num2)
      {
        height = (double) num2;
        width = (double) imageWidth / (double) imageHeight * (double) num2;
      }
    }
    double x = (double) paperMargin.Left.Pixel;
    double y = (double) paperMargin.Top.Pixel;
    switch (horizontalAlignment)
    {
      case ImageHorizontalAlignment.Center:
        x = ((double) num1 - width) / 2.0 + (double) paperMargin.Left.Pixel;
        break;
      case ImageHorizontalAlignment.Right:
        x = (double) num1 - width - (double) paperMargin.Right.Pixel;
        break;
    }
    switch (verticalAlignment)
    {
      case ImageVerticalAlignment.Center:
        y = ((double) num2 - height) / 2.0 + (double) paperMargin.Top.Pixel;
        break;
      case ImageVerticalAlignment.Bottom:
        x = (double) num2 - height - (double) paperMargin.Bottom.Pixel;
        break;
    }
    return new RectangleF((float) x, (float) y, (float) width, (float) height);
  }
}
