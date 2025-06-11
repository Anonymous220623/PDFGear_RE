// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfColorMask
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfColorMask : PdfMask
{
  private PdfColor m_startColor;
  private PdfColor m_endColor;

  public PdfColor StartColor
  {
    get => this.m_startColor;
    set => this.m_startColor = value;
  }

  public PdfColor EndColor
  {
    get => this.m_endColor;
    set => this.m_endColor = value;
  }

  public PdfColorMask(PdfColor startColor, PdfColor endColor)
  {
    this.m_endColor = endColor;
    this.m_startColor = startColor;
  }
}
