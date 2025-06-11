// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.WatermarkAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.WatermarkUtils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class WatermarkAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfWatermarkAnnotation>(annotationCanvas)
{
  private WatermarkParam watermarkParam;
  private const double HTileSpan = 50.0;
  private const double VTileSpan = 50.0;
  private Point startPoint;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
  }

  public override bool OnPropertyChanged(string propertyName) => false;

  protected override void OnCancel() => this.startPoint = new Point();

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfWatermarkAnnotation>> OnCompleteCreateNewAsync()
  {
    MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
    this.watermarkParam = vm.AnnotationToolbar.WatermarkParam;
    if (this.watermarkParam.PageRange == null || this.watermarkParam.PageRange.Length == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfWatermarkAnnotation>) null;
    for (int p = 0; p < this.watermarkParam.PageRange.Length; ++p)
    {
      PdfPage page = vm.Document.Pages[p];
      if (page.Annots == null)
        page.CreateAnnotations();
      float num1 = 72f;
      if (page.Dictionary.ContainsKey("UserUnit"))
        num1 = page.Dictionary["UserUnit"].As<PdfTypeNumber>().FloatValue / 72f;
      float num2 = 96f;
      float num3 = 96f;
      int num4 = (int) ((double) page.Width / (double) num1 * (double) num2);
      int num5 = (int) ((double) page.Height / (double) num1 * (double) num3);
      switch (vm.AnnotationToolbar.WatermarkModel)
      {
        case WatermarkAnnonationModel.Text:
          WatermarkTextModel textWatermarkModel = vm.AnnotationToolbar.TextWatermarkModel;
          PdfFont stock = PdfFont.CreateStock(page.Document, FontStockNames.Arial);
          Color color1 = (Color) ColorConverter.ConvertFromString(textWatermarkModel.Foreground);
          FS_COLOR color2 = new FS_COLOR((int) (byte) ((double) color1.A * (double) this.watermarkParam.Opacity), (int) color1.R, (int) color1.G, (int) color1.B);
          PdfTextObject pdfTextObject = PdfTextObject.Create(textWatermarkModel.Content, 0.0f, 0.0f, stock, textWatermarkModel.FontSize);
          pdfTextObject.FillColor = color2;
          float width = pdfTextObject.BoundingBox.Width;
          float height = pdfTextObject.BoundingBox.Height;
          if (this.watermarkParam.IsTile)
          {
            int int32_1 = Convert.ToInt32((double) num4 / ((double) width + 50.0));
            int int32_2 = Convert.ToInt32((double) num5 / ((double) height + 50.0));
            for (int index1 = 0; index1 < int32_2; ++index1)
            {
              for (int index2 = 0; index2 < int32_1; ++index2)
              {
                List<PdfTextObject> watermarkTextObjects = WatermarkUtil.CreateWatermarkTextObjects(page.Document, textWatermarkModel.Content, color2, stock, textWatermarkModel.FontSize);
                for (int index3 = 0; index3 < watermarkTextObjects.Count; ++index3)
                {
                  PdfTextObject text = watermarkTextObjects[index3];
                  FS_MATRIX fsMatrix = text.Matrix ?? new FS_MATRIX();
                  fsMatrix.SetIdentity();
                  text.FillColor = color2;
                  fsMatrix.Rotate((float) ((double) this.watermarkParam.Rotation * 3.1400001049041748 / 180.0));
                  text.Matrix = fsMatrix;
                  double x = (double) index2 * ((double) width + 50.0);
                  float y = page.Height - (float) index1 * 50f;
                  text.Location = new FS_POINTF(x, (double) y);
                  PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(page, text);
                  watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
                  page.Annots.Add((PdfAnnotation) watermarkAnnotation);
                }
              }
            }
          }
          else
          {
            List<PdfTextObject> watermarkTextObjects = WatermarkUtil.CreateWatermarkTextObjects(page.Document, textWatermarkModel.Content, color2, stock, textWatermarkModel.FontSize);
            for (int index = 0; index < watermarkTextObjects.Count; ++index)
            {
              PdfTextObject text = watermarkTextObjects[index];
              FS_MATRIX fsMatrix = text.Matrix ?? new FS_MATRIX();
              fsMatrix.SetIdentity();
              fsMatrix.Rotate((float) ((double) this.watermarkParam.Rotation * 3.1400001049041748 / 180.0));
              text.Matrix = fsMatrix;
              this.SeTextObjectLocation(text, this.watermarkParam.Alignment, (double) page.Width, (double) page.Height);
              PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(page, text, this.watermarkParam.Alignment);
              watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
              watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
              page.Annots.Add((PdfAnnotation) watermarkAnnotation);
            }
          }
          await page.TryRedrawPageAsync();
          break;
        case WatermarkAnnonationModel.Image:
          WatermarkImageModel imageWatermarkModel = vm.AnnotationToolbar.ImageWatermarkModel;
          WriteableBitmap writeableBitmap = (WriteableBitmap) null;
          if (imageWatermarkModel?.WatermarkImageSource != null)
            writeableBitmap = !(imageWatermarkModel.WatermarkImageSource.Format == PixelFormats.Bgra32) ? new WriteableBitmap((BitmapSource) new FormatConvertedBitmap(imageWatermarkModel.WatermarkImageSource, PixelFormats.Bgra32, (BitmapPalette) null, 0.0)) : new WriteableBitmap(imageWatermarkModel.WatermarkImageSource);
          using (PdfBitmap pdfBitmap = new PdfBitmap(writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, true))
          {
            int bufferSize = pdfBitmap.Stride * pdfBitmap.Height;
            writeableBitmap.CopyPixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pdfBitmap.Buffer, bufferSize, pdfBitmap.Stride);
            PdfImageObject pdfImageObject1 = PdfImageObject.Create(page.Document, pdfBitmap, 0.0f, 0.0f);
            float scale = (double) writeableBitmap.PixelWidth > (double) page.Width / 2.0 || (double) writeableBitmap.PixelHeight > (double) page.Height / 2.0 ? 0.1f : 0.5f;
            if (this.watermarkParam.IsTile)
            {
              float num6 = (float) pdfBitmap.Width * scale;
              float num7 = (float) pdfBitmap.Height * scale;
              int int32_3 = Convert.ToInt32((double) num4 / ((double) num6 + 50.0));
              int int32_4 = Convert.ToInt32((double) num5 / ((double) num7 + 50.0));
              for (int index4 = 0; index4 < int32_4; ++index4)
              {
                for (int index5 = 0; index5 < int32_3; ++index5)
                {
                  float x = (float) index5 * (num6 + 50f);
                  float y = page.Height - (float) index4 * (num7 + 50f);
                  PdfImageObject pdfImageObject2 = PdfImageObject.Create(page.Document, pdfBitmap, x, y);
                  this.SetImageObjectSoftMask(pdfImageObject2, this.watermarkParam.Opacity);
                  this.SetImageRotate(pdfImageObject2, this.watermarkParam.Rotation, scale, true, x, y);
                  PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(page, pdfImageObject2);
                  watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
                  watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
                  page.Annots.Add((PdfAnnotation) watermarkAnnotation);
                }
              }
            }
            else
            {
              this.SetImageObjectSoftMask(pdfImageObject1, this.watermarkParam.Opacity);
              FS_POINTF fsPointf = this.SeImgObjectLocation(pdfImageObject1, this.watermarkParam.Alignment, (double) page.Width, (double) page.Height, scale);
              this.SetImageRotate(pdfImageObject1, this.watermarkParam.Rotation, scale, false, fsPointf.X, fsPointf.Y);
              PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(page, pdfImageObject1);
              watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
              watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
              page.Annots.Add((PdfAnnotation) watermarkAnnotation);
            }
            await page.TryRedrawPageAsync();
          }
          break;
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfWatermarkAnnotation>) null;
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if ((double) pagePoint.X < 0.0 || (double) pagePoint.X > (double) page.Width || (double) pagePoint.Y < 0.0 || (double) pagePoint.Y > (double) page.Height)
      return;
    this.startPoint = pagePoint.ToPoint();
  }

  protected override bool OnSelecting(PdfWatermarkAnnotation annotation, bool afterCreate) => false;

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return false;
    this.startPoint = clientPoint;
    return true;
  }

  private void SeTextObjectLocation(
    PdfTextObject text,
    PdfContentAlignment alignment,
    double pageWidth,
    double pageHeight)
  {
    float width = text.BoundingBox.Width;
    float height = text.BoundingBox.Height;
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        text.Location = new FS_POINTF(5.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.TopCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.TopRight:
        text.Location = new FS_POINTF(pageWidth - (double) width - 5.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.MiddleLeft:
        text.Location = new FS_POINTF(5.0, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.MiddleCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.MiddleRight:
        text.Location = new FS_POINTF(pageWidth - (double) width, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.BottomLeft:
        text.Location = new FS_POINTF(5f, 0.0f);
        break;
      case PdfContentAlignment.BottomCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, 0.0);
        break;
      case PdfContentAlignment.BottomRight:
        text.Location = new FS_POINTF(pageWidth - (double) width, 0.0);
        break;
    }
  }

  private FS_POINTF SeImgObjectLocation(
    PdfImageObject img,
    PdfContentAlignment alignment,
    double pageWidth,
    double pageHeight,
    float scale)
  {
    float num1 = img.BoundingBox.Width * scale;
    float num2 = img.BoundingBox.Height * scale;
    FS_POINTF fsPointf = new FS_POINTF();
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.TopCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.TopRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1 - 5.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.MiddleLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.MiddleCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.MiddleRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.BottomLeft:
        fsPointf = new FS_POINTF(5f, num1 / 2f);
        break;
      case PdfContentAlignment.BottomCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, 0.0);
        break;
      case PdfContentAlignment.BottomRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1, 0.0);
        break;
    }
    return fsPointf;
  }

  private void SetImageRotate(
    PdfImageObject img,
    float rotation,
    float scale,
    bool isTile,
    float x,
    float y)
  {
    FS_MATRIX fsMatrix = img.Matrix ?? new FS_MATRIX();
    float sx = (float) img.Bitmap.Width * scale;
    float sy = (float) img.Bitmap.Height * scale;
    float angle = (float) ((double) rotation * 3.1400001049041748 / 180.0);
    fsMatrix.SetIdentity();
    fsMatrix.Scale(sx, sy);
    fsMatrix.Translate((float) (-(double) sx / 2.0), (float) (-(double) sy / 2.0));
    fsMatrix.Rotate(angle);
    fsMatrix.Translate(sx / 2f, sy / 2f);
    fsMatrix.Translate(x, y);
    img.Matrix = fsMatrix;
  }

  private void SetImageObjectSoftMask(PdfImageObject img, float Opacity)
  {
    PdfTypeStream pdfTypeStream = (PdfTypeStream) null;
    if (img.SoftMask != null && img.SoftMask.Is<PdfTypeStream>())
    {
      pdfTypeStream = img.SoftMask.As<PdfTypeStream>();
    }
    else
    {
      PdfTypeBase pdfTypeBase;
      if (img.Stream.Dictionary != null && img.Stream.Dictionary.TryGetValue("SMask", out pdfTypeBase) && pdfTypeBase != null && pdfTypeBase.Is<PdfTypeStream>())
        pdfTypeStream = pdfTypeBase.As<PdfTypeStream>();
    }
    byte[] data = new byte[pdfTypeStream.Content.Length];
    for (int index = 0; index < pdfTypeStream.Content.Length; ++index)
      data[index] = (byte) ((double) pdfTypeStream.Content[index] * (double) Opacity);
    pdfTypeStream.SetContent(data, false);
    img.SoftMask = (PdfTypeBase) pdfTypeStream;
  }
}
