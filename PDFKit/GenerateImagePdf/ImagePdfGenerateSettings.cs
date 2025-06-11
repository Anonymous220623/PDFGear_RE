// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.ImagePdfGenerateSettings
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Drawing;

#nullable disable
namespace PDFKit.GenerateImagePdf;

public class ImagePdfGenerateSettings
{
  public ImagePdfGenerateSettings()
  {
    this.PaperSize = new SizeF?(ImagePdfGeneratePaperSize.A4);
    this.PaperOrientation = ImagePdfGeneratePaperOrientation.Portrait;
    this.PaperMargin = new ImagePdfGeneratePaperMargin(LengthUnit.FromCentimeter(3.18f), LengthUnit.FromCentimeter(2.52f));
    this.ImageStretch = ImagePdfGenerateImageStretch.Uniform;
    this.ImageHorizontalAlignment = ImageHorizontalAlignment.Center;
    this.ImageVerticalAlignment = ImageVerticalAlignment.Center;
    this.ImageDpi = 150f;
    this.Rotate180Degrees = false;
  }

  public SizeF? PaperSize { get; set; }

  public ImagePdfGeneratePaperOrientation PaperOrientation { get; set; }

  public ImagePdfGeneratePaperMargin PaperMargin { get; set; }

  public ImagePdfGenerateImageStretch ImageStretch { get; set; }

  public ImageHorizontalAlignment ImageHorizontalAlignment { get; set; }

  public ImageVerticalAlignment ImageVerticalAlignment { get; set; }

  public float ImageDpi { get; set; }

  public bool Rotate180Degrees { get; set; }

  public ImagePdfGenerateSettings Clone()
  {
    return new ImagePdfGenerateSettings()
    {
      PaperSize = this.PaperSize,
      PaperOrientation = this.PaperOrientation,
      PaperMargin = this.PaperMargin,
      ImageStretch = this.ImageStretch,
      ImageDpi = this.ImageDpi,
      Rotate180Degrees = this.Rotate180Degrees
    };
  }
}
