// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfEllipsePart
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfEllipsePart : PdfRectangleArea
{
  private float m_startAngle;
  private float m_sweepAngle;

  public float StartAngle
  {
    get => this.m_startAngle;
    set => this.m_startAngle = value;
  }

  public float SweepAngle
  {
    get => this.m_sweepAngle;
    set => this.m_sweepAngle = value;
  }

  protected PdfEllipsePart()
  {
  }

  protected PdfEllipsePart(
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
    : base(x, y, width, height)
  {
    this.m_startAngle = startAngle;
    this.m_sweepAngle = sweepAngle;
  }

  protected PdfEllipsePart(RectangleF rectangle, float startAngle, float sweepAngle)
    : base(rectangle)
  {
    this.m_startAngle = startAngle;
    this.m_sweepAngle = sweepAngle;
  }

  protected PdfEllipsePart(
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
    : base(pen, brush, x, y, width, height)
  {
    this.m_startAngle = startAngle;
    this.m_sweepAngle = sweepAngle;
  }

  protected PdfEllipsePart(
    PdfPen pen,
    PdfBrush brush,
    RectangleF rectangle,
    float startAngle,
    float sweepAngle)
    : base(pen, brush, rectangle)
  {
    this.m_startAngle = startAngle;
    this.m_sweepAngle = sweepAngle;
  }
}
