// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.CompressPDFUtils
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Exporting;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Utils;

public static class CompressPDFUtils
{
  public static async Task<bool> ComputePDFSize(
    string inputFile,
    int compressMode,
    string outputfileName,
    string password,
    IProgress<double> progress = null,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    cancellationToken.ThrowIfCancellationRequested();
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        cancellationToken.ThrowIfCancellationRequested();
        progress?.Report(0.0);
        double imageOffset = CompressPDFUtils.GetImageOffset(compressMode);
        PdfLoadedDocument pdfLoadedDocument = new PdfLoadedDocument(inputFile, password);
        pdfLoadedDocument.FileStructure.IncrementalUpdate = false;
        switch (compressMode)
        {
          case 0:
            pdfLoadedDocument.Compression = PdfCompressionLevel.Best;
            break;
          case 1:
            pdfLoadedDocument.Compression = PdfCompressionLevel.Normal;
            break;
          case 2:
            pdfLoadedDocument.Compression = PdfCompressionLevel.BelowNormal;
            break;
        }
        pdfLoadedDocument.CompressionOptions = new PdfCompressionOptions()
        {
          OptimizePageContents = true,
          RemoveMetadata = true,
          OptimizeFont = true,
          CompressImages = true
        };
        for (int index = 0; index < pdfLoadedDocument.Pages.Count; ++index)
        {
          PdfPageBase page = pdfLoadedDocument.Pages[index];
          PdfImageInfo[] imagesInfo = page.ImagesInfo;
          cancellationToken.ThrowIfCancellationRequested();
          for (int imageIndex = 0; imageIndex < imagesInfo.Length; ++imageIndex)
          {
            cancellationToken.ThrowIfCancellationRequested();
            PdfImageInfo pdfImageInfo = imagesInfo[imageIndex];
            RectangleF bounds = pdfImageInfo.Bounds;
            if ((double) bounds.Width >= 0.0)
            {
              bounds = pdfImageInfo.Bounds;
              if ((double) bounds.Height >= 0.0)
              {
                bounds = pdfImageInfo.Bounds;
                double num1 = (double) bounds.Width * 2.54 / 72.0;
                bounds = pdfImageInfo.Bounds;
                double num2 = (double) bounds.Height * 2.54 / 72.0;
                double num3 = 0.0;
                double num4 = 0.0;
                try
                {
                  num3 = (double) pdfImageInfo.Image.Width * 2.54 / num1;
                  num4 = (double) pdfImageInfo.Image.Height * 2.54 / num2;
                }
                catch
                {
                }
                if (num3 > 0.0 && num4 > 0.0)
                {
                  PdfBitmap image1 = (PdfBitmap) null;
                  Image image2 = (Image) null;
                  try
                  {
                    image2 = CompressPDFUtils.ResizeImage(imagesInfo[imageIndex].Image, pdfImageInfo.Bounds, imageOffset);
                    if (image2 != null)
                    {
                      image1 = new PdfBitmap(image2);
                      image1.Quality = 50L;
                      page.ReplaceImage(imageIndex, (PdfImage) image1);
                      image1.Dispose();
                      image1 = (PdfBitmap) null;
                      image2.Dispose();
                      image2 = (Image) null;
                    }
                  }
                  finally
                  {
                    try
                    {
                      imagesInfo[imageIndex]?.Image?.Dispose();
                    }
                    catch
                    {
                    }
                    try
                    {
                      image1?.Dispose();
                    }
                    catch
                    {
                    }
                    try
                    {
                      image2?.Dispose();
                    }
                    catch
                    {
                    }
                  }
                }
                progress?.Report(0.5 * ((double) index + ((double) imageIndex + 1.0) / (double) imagesInfo.Length) / (double) pdfLoadedDocument.Pages.Count);
              }
            }
          }
          progress?.Report(0.5 * ((double) index + 1.0) / (double) pdfLoadedDocument.Pages.Count);
        }
        try
        {
          double curProgress = 0.5;
          pdfLoadedDocument.SaveProgress += (PdfDocumentBase.ProgressEventHandler) ((s, a) =>
          {
            cancellationToken.ThrowIfCancellationRequested();
            double num = 0.5 + (double) a.Progress * 0.5;
            if (num <= curProgress)
              return;
            curProgress = num;
            progress?.Report(curProgress);
          });
          pdfLoadedDocument.Save(outputfileName);
          pdfLoadedDocument.Close(true);
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
        }
      })), cancellationToken);
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
      return false;
    }
    return true;
  }

  private static double GetImageOffset(int mode)
  {
    if (mode == 0)
      return 75.0;
    return mode == 1 ? 110.0 : 220.0;
  }

  private static Image ResizeImage(Image imgToResize, RectangleF bounds, double maxPPI)
  {
    int width1 = imgToResize.Width;
    int height1 = imgToResize.Height;
    double num1 = (double) bounds.Width * 2.54 / 72.0;
    double num2 = (double) bounds.Height * 2.54 / 72.0;
    double num3 = (double) imgToResize.Width * 2.54 / num1;
    double num4 = (double) imgToResize.Height * 2.54 / num2;
    double num5 = maxPPI;
    if (num3 - num5 < 2.0 && num4 - maxPPI < 2.0)
      return (Image) null;
    int width2 = (int) (num1 * maxPPI / 2.54);
    int height2 = (int) (num2 * maxPPI / 2.54);
    if (width2 <= 0 || height2 <= 0)
      return (Image) null;
    Bitmap bitmap = new Bitmap(width2, height2);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap);
    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    graphics.DrawImage(imgToResize, 0, 0, width2, height2);
    graphics.Dispose();
    return (Image) bitmap;
  }
}
