// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.TexturePaint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class TexturePaint
{
  private int m_heights;
  private int m_widths;
  private string m_images;

  public int Height
  {
    get => this.m_heights;
    set => this.m_heights = value;
  }

  public int Width
  {
    get => this.m_widths;
    set => this.m_widths = value;
  }

  public string Image
  {
    get => this.m_images;
    set => this.m_images = value;
  }
}
