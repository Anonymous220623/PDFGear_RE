// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfRectangle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfRectangle : PdfRectangleArea
{
  public PdfRectangle(float width, float height)
    : this(0.0f, 0.0f, width, height)
  {
  }

  public PdfRectangle(PdfPen pen, float width, float height)
    : this(pen, 0.0f, 0.0f, width, height)
  {
  }

  public PdfRectangle(PdfBrush brush, float width, float height)
    : this(brush, 0.0f, 0.0f, width, height)
  {
  }

  public PdfRectangle(PdfPen pen, PdfBrush brush, float width, float height)
    : this(pen, brush, 0.0f, 0.0f, width, height)
  {
  }

  public PdfRectangle(float x, float y, float width, float height)
    : base(x, y, width, height)
  {
  }

  public PdfRectangle(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfRectangle(PdfPen pen, float x, float y, float width, float height)
    : base(pen, (PdfBrush) null, x, y, width, height)
  {
  }

  public PdfRectangle(PdfPen pen, RectangleF rectangle)
    : base(pen, (PdfBrush) null, rectangle)
  {
  }

  public PdfRectangle(PdfBrush brush, float x, float y, float width, float height)
    : base((PdfPen) null, brush, x, y, width, height)
  {
  }

  public PdfRectangle(PdfBrush brush, RectangleF rectangle)
    : base((PdfPen) null, brush, rectangle)
  {
  }

  public PdfRectangle(PdfPen pen, PdfBrush brush, float x, float y, float width, float height)
    : base(pen, brush, x, y, width, height)
  {
  }

  public PdfRectangle(PdfPen pen, PdfBrush brush, RectangleF rectangle)
    : base(pen, brush, rectangle)
  {
  }

  private PdfRectangle()
  {
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawRectangle(this.ObtainPen(), this.Brush, this.Bounds);
  }
}
