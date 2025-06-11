// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.StampAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.StampUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class StampAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfStampAnnotation>(annotationCanvas)
{
  private StampAnnotationDragControl dragControl;
  private SignatureDragControl signatureDragControl;
  private Point startPoint;
  private PdfPage processingPage;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State != AnnotationHolderState.Selected)
      return;
    this.dragControl?.OnPageClientBoundsChanged();
    this.signatureDragControl?.OnPageClientBoundsChanged();
  }

  public override bool OnPropertyChanged(string propertyName) => false;

  protected override void OnCancel()
  {
    Mouse.Captured?.ReleaseMouseCapture();
    if (this.dragControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.dragControl);
      this.dragControl = (StampAnnotationDragControl) null;
    }
    if (this.signatureDragControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.signatureDragControl);
      this.signatureDragControl = (SignatureDragControl) null;
    }
    this.processingPage = (PdfPage) null;
    this.startPoint = new Point();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>> OnCompleteCreateNewAsync()
  {
    StampAnnotationHolder annotationHolder = this;
    try
    {
      // ISSUE: explicit non-virtual call
      MainViewModel dataContext = __nonvirtual (annotationHolder.AnnotationCanvas).DataContext as MainViewModel;
      FS_POINTF positionFromDocument = dataContext.ViewerOperationModel.PositionFromDocument;
      FS_SIZEF sizeInDocument = dataContext.ViewerOperationModel.SizeInDocument;
      FS_RECTF rect = new FS_RECTF(positionFromDocument.X, positionFromDocument.Y, positionFromDocument.X + sizeInDocument.Width, positionFromDocument.Y - sizeInDocument.Height);
      if (annotationHolder.processingPage.Rotation == PageRotate.Rotate90)
        rect = new FS_RECTF(positionFromDocument.X, positionFromDocument.Y, positionFromDocument.X + sizeInDocument.Height, positionFromDocument.Y + sizeInDocument.Width);
      else if (annotationHolder.processingPage.Rotation == PageRotate.Rotate180)
        rect = new FS_RECTF(positionFromDocument.X - sizeInDocument.Width, positionFromDocument.Y, positionFromDocument.X, positionFromDocument.Y + sizeInDocument.Height);
      else if (annotationHolder.processingPage.Rotation == PageRotate.Rotate270)
        rect = new FS_RECTF(positionFromDocument.X - sizeInDocument.Height, positionFromDocument.Y - sizeInDocument.Width, positionFromDocument.X, positionFromDocument.Y);
      PdfPage processingPage = annotationHolder.processingPage;
      if (dataContext.AnnotationMode == AnnotationMode.Signature && !annotationHolder.ShowIAPWindow())
        return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
      if (processingPage.Annots == null)
        processingPage.CreateAnnotations();
      switch (dataContext.ViewerOperationModel?.Data)
      {
        case IStampTextModel textModel:
          return await annotationHolder.CreateTextStampAsync(textModel, processingPage, positionFromDocument, rect);
        case StampImageModel imageModel:
          return await annotationHolder.CreateImageStampAsync(imageModel, processingPage, positionFromDocument, rect);
        case StampFormControlModel formControlModel:
          return await annotationHolder.CreateFormControlStampAsync(formControlModel, processingPage, positionFromDocument, rect);
        default:
          dataContext.AnnotationMode = AnnotationMode.None;
          return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
      }
    }
    finally
    {
      annotationHolder.processingPage = (PdfPage) null;
    }
  }

  private bool ShowIAPWindow()
  {
    Ioc.Default.GetRequiredService<MainViewModel>();
    bool signatureExistFlag = ConfigManager.GetSignatureExistFlag();
    if (IAPUtils.IsPaidUserWrapper() || !signatureExistFlag)
      return true;
    IAPUtils.ShowPurchaseWindows("Signature", ".pdf");
    return false;
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if ((double) pagePoint.X < 0.0 || (double) pagePoint.X > (double) page.Width || (double) pagePoint.Y < 0.0 || (double) pagePoint.Y > (double) page.Height)
      return;
    this.processingPage = page;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return;
    double x = clientPoint.X;
    double y = clientPoint.Y;
    this.startPoint = pagePoint.ToPoint();
  }

  protected override bool OnSelecting(PdfStampAnnotation annotation, bool afterCreate)
  {
    if (!string.IsNullOrEmpty(annotation.Subject))
    {
      if (annotation.Subject == "Signature")
      {
        this.signatureDragControl = new SignatureDragControl(annotation, (IAnnotationHolder) this);
        this.AnnotationCanvas.Children.Add((UIElement) this.signatureDragControl);
      }
      else
      {
        this.dragControl = new StampAnnotationDragControl(annotation, (IAnnotationHolder) this);
        this.AnnotationCanvas.Children.Add((UIElement) this.dragControl);
      }
    }
    else
    {
      this.signatureDragControl = new SignatureDragControl(annotation, (IAnnotationHolder) this);
      this.AnnotationCanvas.Children.Add((UIElement) this.signatureDragControl);
    }
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    this.processingPage = page;
    return true;
  }

  internal static Size GetStampPageSize(
    double bitmapWidth,
    double bitmapHeight,
    FS_RECTF pageBounds)
  {
    Size stampPageSize = new Size(bitmapWidth * 96.0 / 72.0, bitmapHeight * 96.0 / 72.0);
    if (stampPageSize.Width > (double) pageBounds.Width / 2.0 || stampPageSize.Height > (double) pageBounds.Height / 2.0)
    {
      double num = Math.Max(stampPageSize.Width / ((double) pageBounds.Width / 2.0), stampPageSize.Height / ((double) pageBounds.Height / 2.0));
      stampPageSize = new Size(stampPageSize.Width / num, stampPageSize.Height / num);
    }
    else if (stampPageSize.Width < 10.0 && stampPageSize.Height < 10.0)
    {
      double num = Math.Min(stampPageSize.Width / 10.0, stampPageSize.Height / 10.0);
      stampPageSize = new Size(stampPageSize.Width / num, stampPageSize.Height / num);
    }
    return stampPageSize;
  }

  internal static Size GetSignaturePageSize(
    double bitmapWidth,
    double bitmapHeight,
    FS_RECTF pageBounds)
  {
    Size signaturePageSize = new Size(bitmapWidth * 96.0 / 72.0, bitmapHeight * 96.0 / 72.0);
    if (signaturePageSize.Width != 200.0)
    {
      double num = 200.0 / signaturePageSize.Width;
      signaturePageSize = new Size(200.0, signaturePageSize.Height * num);
    }
    if (signaturePageSize.Height > (double) pageBounds.Height / 2.0)
    {
      double num = signaturePageSize.Height / ((double) pageBounds.Height / 2.0);
      signaturePageSize = new Size(signaturePageSize.Width / num, (double) pageBounds.Height / 2.0);
    }
    return signaturePageSize;
  }

  private async Task<System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>> CreateTextStampAsync(
    IStampTextModel textModel,
    PdfPage page,
    FS_POINTF point,
    FS_RECTF rect)
  {
    StampAnnotationHolder annotationHolder = this;
    if (textModel == null)
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    // ISSUE: explicit non-virtual call
    MainViewModel dataContext = __nonvirtual (annotationHolder.AnnotationCanvas).DataContext as MainViewModel;
    Color color1 = (Color) ColorConverter.ConvertFromString(textModel.FontColor);
    FS_COLOR color2 = new FS_COLOR((int) color1.A, (int) color1.R, (int) color1.G, (int) color1.B);
    string textContent = textModel?.TextContent;
    PdfStampAnnotation stamptextAnnot;
    if (textModel.IsPreset)
    {
      stamptextAnnot = new PdfStampAnnotation(page, ((PresetStampTextModel) textModel).IconName, point.X, point.Y, color2);
    }
    else
    {
      stamptextAnnot = new PdfStampAnnotation(page, textModel.TextContent, point.X, point.Y, color2);
      stamptextAnnot.Contents = (string) null;
    }
    PDFExtStampDictionary dict = new PDFExtStampDictionary()
    {
      Type = "Stamp",
      Template = "Default"
    };
    dict.SetContentDictionary(new Dictionary<string, string>()
    {
      ["ContentText"] = textContent
    });
    StampUtil.SetPDFXStampDictionary(stamptextAnnot, dict);
    FS_RECTF rotatedRect = PdfRotateUtils.GetRotatedRect(rect, point, page.Rotation);
    if (page.Rotation != PageRotate.Normal)
      stamptextAnnot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create((int) page.Rotation * 90);
    stamptextAnnot.Rectangle = rotatedRect;
    stamptextAnnot.ExtendedIconName = textModel.TextContent;
    stamptextAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
    stamptextAnnot.CreationDate = stamptextAnnot.ModificationDate;
    stamptextAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
    stamptextAnnot.Subject = "Stamp";
    stamptextAnnot.Flags |= AnnotationFlags.Print;
    page.Annots.Add((PdfAnnotation) stamptextAnnot);
    stamptextAnnot.TryRedrawAnnotation();
    await dataContext.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) stamptextAnnot);
    await page.TryRedrawPageAsync();
    if (!((PdfWrapper) stamptextAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    annotationHolder.startPoint = new Point();
    return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) new PdfStampAnnotation[1]
    {
      stamptextAnnot
    };
  }

  private async Task<System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>> CreateImageStampAsync(
    StampImageModel imageModel,
    PdfPage page,
    FS_POINTF point,
    FS_RECTF rect)
  {
    StampAnnotationHolder annotationHolder = this;
    if (imageModel == null)
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    // ISSUE: explicit non-virtual call
    MainViewModel vm = __nonvirtual (annotationHolder.AnnotationCanvas).DataContext as MainViewModel;
    PdfStampAnnotation imgStamp = (PdfStampAnnotation) null;
    WriteableBitmap writeableBitmap = (WriteableBitmap) null;
    if (imageModel.ImageStampControlModel.StampImageSource != null)
      writeableBitmap = !(imageModel.ImageStampControlModel.StampImageSource.Format == PixelFormats.Bgra32) ? new WriteableBitmap((BitmapSource) new FormatConvertedBitmap(imageModel.ImageStampControlModel.StampImageSource, PixelFormats.Bgra32, (BitmapPalette) null, 0.0)) : new WriteableBitmap(imageModel.ImageStampControlModel.StampImageSource);
    Size size = new Size((double) imageModel.ImageStampControlModel.PageSize.Width, (double) imageModel.ImageStampControlModel.PageSize.Height);
    using (PdfBitmap pdfBitmap = new PdfBitmap(writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, true))
    {
      int bufferSize = pdfBitmap.Stride * pdfBitmap.Height;
      writeableBitmap.CopyPixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pdfBitmap.Buffer, bufferSize, pdfBitmap.Stride);
      PdfImageObject img = PdfImageObject.Create(vm.Document, pdfBitmap, point.X, point.Y - (float) size.Height);
      img.Matrix = new FS_MATRIX(size.Width, 0.0, 0.0, size.Height, (double) point.X, (double) (point.Y - (float) size.Height));
      imgStamp = new PdfStampAnnotation(page, "", point.X, point.Y, FS_COLOR.SteelBlue);
      bool flag = imageModel.RemoveBackground;
      if (!flag)
        flag = await ConfigManager.IsRemoveSignatureBg(imageModel.ImageFilePath);
      if (flag)
      {
        img.BlendMode = BlendTypes.FXDIB_BLEND_MULTIPLY;
        imgStamp.Dictionary["IsRemoveBg"] = (PdfTypeBase) PdfTypeBoolean.Create(true);
      }
      imgStamp.Flags |= AnnotationFlags.Print;
      imgStamp.NormalAppearance.Clear();
      imgStamp.NormalAppearance.Add((PdfPageObject) img);
    }
    if (imageModel.IsSignature)
      imgStamp.Subject = "Signature";
    else
      imgStamp.Subject = "Stamp";
    imgStamp.GenerateAppearance(AppearanceStreamModes.Normal);
    rect = imgStamp.GetRECT();
    FS_RECTF rotatedRect = PdfRotateUtils.GetRotatedRect(rect, point, page.Rotation);
    if (page.Rotation != PageRotate.Normal)
      imgStamp.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create((int) page.Rotation * 90);
    imgStamp.Rectangle = rotatedRect;
    imgStamp.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
    imgStamp.CreationDate = imgStamp.ModificationDate;
    imgStamp.Text = AnnotationAuthorUtil.GetAuthorName();
    imgStamp.RegenerateAppearancesAdvance();
    page.Annots.Add((PdfAnnotation) imgStamp);
    await vm.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) imgStamp);
    await page.TryRedrawPageAsync();
    if (!((PdfWrapper) imgStamp != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    annotationHolder.startPoint = new Point();
    if (imageModel.IsSignature && !string.IsNullOrEmpty(imageModel.ImageFilePath) && File.Exists(imageModel.ImageFilePath))
    {
      ConfigManager.SetSignatureExistFlag(true);
      vm.IsSaveBySignature = true;
      imgStamp.Dictionary["ImgSource"] = (PdfTypeBase) PdfTypeString.Create(imageModel.ImageFilePath);
    }
    vm.AnnotationMode = AnnotationMode.None;
    return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) new PdfStampAnnotation[1]
    {
      imgStamp
    };
  }

  private async Task<System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>> CreateFormControlStampAsync(
    StampFormControlModel formControlModel,
    PdfPage page,
    FS_POINTF point,
    FS_RECTF rect)
  {
    StampAnnotationHolder annotationHolder = this;
    if (formControlModel == null)
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    // ISSUE: explicit non-virtual call
    MainViewModel dataContext = __nonvirtual (annotationHolder.AnnotationCanvas).DataContext as MainViewModel;
    PdfStampAnnotation stampAnnot = new PdfStampAnnotation(page, StampIconNames.Draft, point.X, point.Y, FS_COLOR.Black);
    FS_RECTF rotatedRect = PdfRotateUtils.GetRotatedRect(rect, point, page.Rotation);
    if (page.Rotation != PageRotate.Normal)
      stampAnnot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create((int) page.Rotation * 90);
    stampAnnot.Rectangle = rotatedRect;
    stampAnnot.ExtendedIconName = formControlModel.Name;
    stampAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
    stampAnnot.CreationDate = stampAnnot.ModificationDate;
    stampAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
    stampAnnot.Subject = "FormControl";
    stampAnnot.Flags |= AnnotationFlags.Print;
    StampUtil.SetPDFXStampDictionary(stampAnnot, new PDFExtStampDictionary()
    {
      Type = "FormControl"
    });
    page.Annots.Add((PdfAnnotation) stampAnnot);
    stampAnnot.TryRedrawAnnotation();
    await dataContext.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) stampAnnot);
    await page.TryRedrawPageAsync();
    if (!((PdfWrapper) stampAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) null;
    annotationHolder.startPoint = new Point();
    return (System.Collections.Generic.IReadOnlyList<PdfStampAnnotation>) new PdfStampAnnotation[1]
    {
      stampAnnot
    };
  }

  public static int[] bytesToInt(byte[] src, int offset)
  {
    int[] numArray = new int[src.Length / 4 + 1];
    for (int index = 0; index < src.Length / 4; ++index)
    {
      int num = (int) src[offset] & (int) byte.MaxValue | ((int) src[offset + 1] & (int) byte.MaxValue) << 8 | ((int) src[offset + 2] & (int) byte.MaxValue) << 16 /*0x10*/ | ((int) src[offset + 3] & (int) byte.MaxValue) << 24;
      numArray[index] = num;
      offset += 4;
    }
    if (offset + 3 == src.Length - 1)
      numArray[numArray.Length - 1] |= (int) src[offset + 3] << 24;
    if (offset + 2 == src.Length - 1)
      numArray[numArray.Length - 1] |= (int) src[offset + 2] << 16 /*0x10*/;
    if (offset + 1 == src.Length - 1)
      numArray[numArray.Length - 1] |= (int) src[offset + 1] << 8;
    if (offset == src.Length - 1)
      numArray[numArray.Length - 1] |= (int) src[offset];
    return numArray;
  }

  public static byte[] intToBytes(int[] src, int offset)
  {
    byte[] bytes = new byte[src.Length * 4];
    for (int index = 0; index < src.Length; ++index)
    {
      bytes[offset + 3] = (byte) (src[index] >> 24 & (int) byte.MaxValue);
      bytes[offset + 2] = (byte) (src[index] >> 16 /*0x10*/ & (int) byte.MaxValue);
      bytes[offset + 1] = (byte) (src[index] >> 8 & (int) byte.MaxValue);
      bytes[offset] = (byte) (src[index] & (int) byte.MaxValue);
      offset += 4;
    }
    return bytes;
  }

  public static void SetDefaultTextStampContent(PdfStampAnnotation stampAnnot, string content)
  {
    if ((PdfWrapper) stampAnnot == (PdfWrapper) null)
      return;
    content = content ?? "";
    if (stampAnnot.ExtendedIconName != content)
      stampAnnot.ExtendedIconName = content;
    PDFExtStampDictionary extStampDictionary = StampUtil.GetPDFExtStampDictionary(stampAnnot);
    if (extStampDictionary == null)
      return;
    Dictionary<string, string> contentDictionary = extStampDictionary.GetContentDictionary();
    if (contentDictionary == null || !contentDictionary.ContainsKey("ContentText"))
      return;
    contentDictionary["ContentText"] = content;
    extStampDictionary.SetContentDictionary(contentDictionary);
    StampUtil.SetPDFXStampDictionary(stampAnnot, extStampDictionary);
  }
}
