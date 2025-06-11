// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaLinearBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaLinearBrush : PdfXfaBrush
{
  private PdfColor m_startColor;
  private PdfColor m_endColor;
  private PdfXfaLinearType m_type;

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

  public PdfXfaLinearType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public PdfXfaLinearBrush(PdfColor startColor, PdfColor endColor)
  {
    this.StartColor = startColor;
    this.EndColor = endColor;
  }

  public PdfXfaLinearBrush(PdfColor startColor, PdfColor endColor, PdfXfaLinearType type)
  {
    this.StartColor = startColor;
    this.EndColor = endColor;
    this.Type = type;
  }
}
