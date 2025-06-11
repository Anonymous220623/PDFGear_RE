// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfIndexedColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfIndexedColor(PdfIndexedColorSpace colorspace) : PdfExtendedColor((PdfColorSpaces) colorspace)
{
  private int m_colorIndex;

  public int SelectColorIndex
  {
    get => this.m_colorIndex;
    set => this.m_colorIndex = value;
  }
}
