// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.ImagePdfGenerateItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.GenerateImagePdf;

public class ImagePdfGenerateItem
{
  private readonly BitmapImageSource bitmapImageSource;

  public ImagePdfGenerateItem(BitmapImageSource bitmapImageSource)
  {
    this.bitmapImageSource = bitmapImageSource;
  }

  public ImagePdfGenerateSettings Settings { get; set; }

  internal BitmapImageSource BitmapImageSource => this.bitmapImageSource;
}
