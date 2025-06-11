// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfMultipleNumberValueField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfMultipleNumberValueField : PdfMultipleValueField
{
  private PdfNumberStyle m_numberStyle = PdfNumberStyle.Numeric;

  public PdfMultipleNumberValueField()
  {
  }

  public PdfMultipleNumberValueField(PdfFont font)
    : base(font)
  {
  }

  public PdfMultipleNumberValueField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfMultipleNumberValueField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  public PdfNumberStyle NumberStyle
  {
    get => this.m_numberStyle;
    set => this.m_numberStyle = value;
  }
}
