// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCompressionOptions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfCompressionOptions
{
  private bool m_removeMetadata = true;
  private bool m_compressImages = true;
  private int m_imageQuality = 50;
  private bool m_optimizePageContents = true;
  private bool m_optimizeFont = true;

  public bool RemoveMetadata
  {
    get => this.m_removeMetadata;
    set => this.m_removeMetadata = value;
  }

  public bool CompressImages
  {
    get => this.m_compressImages;
    set => this.m_compressImages = value;
  }

  public int ImageQuality
  {
    get => this.m_imageQuality;
    set => this.m_imageQuality = value;
  }

  public bool OptimizePageContents
  {
    get => this.m_optimizePageContents;
    set => this.m_optimizePageContents = value;
  }

  public bool OptimizeFont
  {
    get => this.m_optimizeFont;
    set => this.m_optimizeFont = value;
  }
}
