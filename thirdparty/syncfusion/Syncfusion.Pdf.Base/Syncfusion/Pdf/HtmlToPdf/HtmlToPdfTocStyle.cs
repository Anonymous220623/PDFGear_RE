// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlToPdfTocStyle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

public class HtmlToPdfTocStyle
{
  private PdfBrush m_backgroundColor;
  private PdfFont m_font;
  private PdfBrush m_foreColor;
  private PdfPaddings m_Padding;

  public PdfBrush BackgroundColor
  {
    get => this.m_backgroundColor;
    set => this.m_backgroundColor = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public PdfBrush ForeColor
  {
    get => this.m_foreColor;
    set => this.m_foreColor = value;
  }

  public PdfPaddings Padding
  {
    get => this.m_Padding;
    set => this.m_Padding = value;
  }
}
