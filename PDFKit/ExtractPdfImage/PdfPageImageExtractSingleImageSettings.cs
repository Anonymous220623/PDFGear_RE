// Decompiled with JetBrains decompiler
// Type: PDFKit.ExtractPdfImage.PdfPageImageExtractSingleImageSettings
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Drawing;

#nullable disable
namespace PDFKit.ExtractPdfImage;

public class PdfPageImageExtractSingleImageSettings : PdfPageImageExtractSettings
{
  public PdfPageImageExtractSingleImageSettings()
  {
    this.ExtractIntoSingleImageOrientation = PdfPageImageExtractOrientation.Vertical;
    this.ImageBackgroundColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    this.BorderColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
  }

  public PdfPageImageExtractOrientation ExtractIntoSingleImageOrientation { get; set; }

  public string TempFileFullName { get; set; }

  public int BorderThickness { get; set; }

  public Color ImageBackgroundColor { get; set; }

  public Color BorderColor { get; set; }
}
