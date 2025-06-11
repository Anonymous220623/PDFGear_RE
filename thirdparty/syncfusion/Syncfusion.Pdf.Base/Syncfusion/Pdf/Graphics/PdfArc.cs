// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfArc
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfArc : PdfEllipsePart
{
  public PdfArc(float width, float height, float startAngle, float sweepAngle)
    : this(0.0f, 0.0f, width, height, startAngle, sweepAngle)
  {
  }

  public PdfArc(PdfPen pen, float width, float height, float startAngle, float sweepAngle)
    : this(pen, 0.0f, 0.0f, width, height, startAngle, sweepAngle)
  {
  }

  public PdfArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
    : base(x, y, width, height, startAngle, sweepAngle)
  {
  }

  public PdfArc(RectangleF rectangle, float startAngle, float sweepAngle)
    : base(rectangle, startAngle, sweepAngle)
  {
  }

  public PdfArc(
    PdfPen pen,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
    : base(pen, (PdfBrush) null, x, y, width, height, startAngle, sweepAngle)
  {
  }

  public PdfArc(PdfPen pen, RectangleF rectangle, float startAngle, float sweepAngle)
    : base(pen, (PdfBrush) null, rectangle, startAngle, sweepAngle)
  {
  }

  protected PdfArc()
  {
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawArc(this.ObtainPen(), this.Bounds, this.StartAngle, this.SweepAngle);
  }
}
