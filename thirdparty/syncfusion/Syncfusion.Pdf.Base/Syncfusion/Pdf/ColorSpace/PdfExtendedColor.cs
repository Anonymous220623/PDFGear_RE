// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfExtendedColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public abstract class PdfExtendedColor
{
  protected PdfColorSpaces m_colorspace;

  public PdfExtendedColor(PdfColorSpaces colorspace) => this.m_colorspace = colorspace;

  public PdfColorSpaces ColorSpace => this.m_colorspace;
}
