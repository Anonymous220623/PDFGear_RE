// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfEllipse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfEllipse : PdfRectangleArea
{
  public PdfEllipse(float width, float height)
    : this(0.0f, 0.0f, width, height)
  {
  }

  public PdfEllipse(PdfPen pen, float width, float height)
    : this(pen, 0.0f, 0.0f, width, height)
  {
  }

  public PdfEllipse(PdfBrush brush, float width, float height)
    : this(brush, 0.0f, 0.0f, width, height)
  {
  }

  public PdfEllipse(PdfPen pen, PdfBrush brush, float width, float height)
    : this(pen, brush, 0.0f, 0.0f, width, height)
  {
  }

  public PdfEllipse(float x, float y, float width, float height)
    : base(x, y, width, height)
  {
  }

  public PdfEllipse(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfEllipse(PdfPen pen, float x, float y, float width, float height)
    : base(pen, (PdfBrush) null, x, y, width, height)
  {
  }

  public PdfEllipse(PdfPen pen, RectangleF rectangle)
    : base(pen, (PdfBrush) null, rectangle)
  {
  }

  public PdfEllipse(PdfBrush brush, float x, float y, float width, float height)
    : base((PdfPen) null, brush, x, y, width, height)
  {
  }

  public PdfEllipse(PdfBrush brush, RectangleF rectangle)
    : base((PdfPen) null, brush, rectangle)
  {
  }

  public PdfEllipse(PdfPen pen, PdfBrush brush, float x, float y, float width, float height)
    : base(pen, brush, x, y, width, height)
  {
  }

  public PdfEllipse(PdfPen pen, PdfBrush brush, RectangleF rectangle)
    : base(pen, brush, rectangle)
  {
  }

  protected PdfEllipse()
  {
  }

  public float RadiusX => this.Width / 2f;

  public float RadiusY => this.Height / 2f;

  public PointF Center => new PointF(this.X + this.RadiusX, this.Y + this.RadiusY);

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawEllipse(this.ObtainPen(), this.Brush, this.Bounds);
  }
}
