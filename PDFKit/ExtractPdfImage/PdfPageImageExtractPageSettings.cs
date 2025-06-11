// Decompiled with JetBrains decompiler
// Type: PDFKit.ExtractPdfImage.PdfPageImageExtractPageSettings
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Drawing;

#nullable disable
namespace PDFKit.ExtractPdfImage;

public class PdfPageImageExtractPageSettings
{
  public float? ImageDpi { get; set; }

  public Color? PageBackgroundColor { get; set; }

  public PdfPageImageExtractColorMode? ColorMode { get; set; }

  public PdfPageImageExtractRotation? AdditionalRotation { get; set; }

  public bool? RenderAnnotations { get; set; }

  internal PdfPageImageExtractSettings WithGlobalSettings(
    PdfPageImageExtractSingleImageSettings settings)
  {
    settings = settings ?? new PdfPageImageExtractSingleImageSettings();
    return new PdfPageImageExtractSettings()
    {
      ImageDpi = (float) ((double) this.ImageDpi ?? (double) settings.ImageDpi),
      PageBackgroundColor = this.PageBackgroundColor ?? settings.PageBackgroundColor,
      ColorMode = (PdfPageImageExtractColorMode) ((int) this.ColorMode ?? (int) settings.ColorMode),
      AdditionalRotation = (PdfPageImageExtractRotation) ((int) this.AdditionalRotation ?? (int) settings.AdditionalRotation),
      RenderAnnotations = ((int) this.RenderAnnotations ?? (settings.RenderAnnotations ? 1 : 0)) != 0
    };
  }
}
