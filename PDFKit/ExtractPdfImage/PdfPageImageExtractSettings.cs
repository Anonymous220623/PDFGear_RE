// Decompiled with JetBrains decompiler
// Type: PDFKit.ExtractPdfImage.PdfPageImageExtractSettings
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System.Drawing;

#nullable disable
namespace PDFKit.ExtractPdfImage;

public class PdfPageImageExtractSettings
{
  public PdfPageImageExtractSettings()
  {
    this.ImageDpi = 72f;
    this.PageBackgroundColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    this.ColorMode = PdfPageImageExtractColorMode.RGB;
    this.AdditionalRotation = PdfPageImageExtractRotation.Rotate0;
    this.RenderAnnotations = true;
  }

  public float ImageDpi { get; set; }

  public Color PageBackgroundColor { get; set; }

  public PdfPageImageExtractColorMode ColorMode { get; set; }

  public PdfPageImageExtractRotation AdditionalRotation { get; set; }

  public bool RenderAnnotations { get; set; }

  internal PageRotate GetPageRotate(PageRotate originalRotate)
  {
    int pageRotate = (int) (originalRotate + (int) this.AdditionalRotation);
    while (pageRotate < 0)
      pageRotate += 4;
    while (pageRotate >= 4)
      pageRotate -= 4;
    return (PageRotate) pageRotate;
  }
}
