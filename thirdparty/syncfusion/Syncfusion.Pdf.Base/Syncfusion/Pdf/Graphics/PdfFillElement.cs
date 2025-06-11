// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfFillElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfFillElement : PdfDrawElement
{
  private PdfBrush m_brush;

  protected PdfFillElement()
  {
  }

  protected PdfFillElement(PdfPen pen)
    : base(pen)
  {
  }

  protected PdfFillElement(PdfBrush brush)
    : this()
  {
    this.m_brush = brush;
  }

  protected PdfFillElement(PdfPen pen, PdfBrush brush)
    : this(pen)
  {
    this.m_brush = brush;
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  protected override PdfPen ObtainPen()
  {
    return this.m_brush != null || this.Pen != null ? this.Pen : PdfPens.Black;
  }
}
