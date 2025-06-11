// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfRectangleArea
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfRectangleArea : PdfFillElement
{
  private RectangleF m_rect;

  protected PdfRectangleArea()
  {
  }

  protected PdfRectangleArea(float x, float y, float width, float height)
    : this()
  {
    this.m_rect = new RectangleF(x, y, width, height);
  }

  protected PdfRectangleArea(RectangleF rectangle)
    : this()
  {
    this.m_rect = rectangle;
  }

  protected PdfRectangleArea(
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height)
    : base(pen, brush)
  {
    this.m_rect = new RectangleF(x, y, width, height);
  }

  protected PdfRectangleArea(PdfPen pen, PdfBrush brush, RectangleF rectangle)
    : base(pen, brush)
  {
    this.m_rect = rectangle;
  }

  public float X
  {
    get => this.m_rect.X;
    set => this.m_rect.X = value;
  }

  public float Y
  {
    get => this.m_rect.Y;
    set => this.m_rect.Y = value;
  }

  public float Width
  {
    get => this.m_rect.Width;
    set => this.m_rect.Width = value;
  }

  public float Height
  {
    get => this.m_rect.Height;
    set => this.m_rect.Height = value;
  }

  public SizeF Size
  {
    get => this.m_rect.Size;
    set => this.m_rect.Size = value;
  }

  public RectangleF Bounds
  {
    get => this.m_rect;
    set => this.m_rect = value;
  }

  protected override RectangleF GetBoundsInternal() => this.Bounds;
}
