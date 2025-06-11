// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfDrawElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfDrawElement : PdfShapeElement
{
  private PdfPen m_pen;

  protected PdfDrawElement()
  {
  }

  protected PdfDrawElement(PdfPen pen)
    : this()
  {
    this.m_pen = pen;
  }

  public PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  protected virtual PdfPen ObtainPen() => this.m_pen != null ? this.m_pen : PdfPens.Black;
}
