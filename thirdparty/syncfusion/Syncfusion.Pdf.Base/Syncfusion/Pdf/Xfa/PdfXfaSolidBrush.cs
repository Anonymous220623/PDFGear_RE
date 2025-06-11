// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaSolidBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaSolidBrush : PdfXfaBrush
{
  private PdfColor m_color;

  public PdfColor Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public PdfXfaSolidBrush(PdfColor color) => this.m_color = color;
}
