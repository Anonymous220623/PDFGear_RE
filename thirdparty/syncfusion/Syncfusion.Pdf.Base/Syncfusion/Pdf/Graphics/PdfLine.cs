// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLine
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfLine : PdfDrawElement
{
  private float m_x1;
  private float m_y1;
  private float m_x2;
  private float m_y2;

  public PdfLine(float x1, float y1, float x2, float y2)
  {
    this.m_x1 = x1;
    this.m_y1 = y1;
    this.m_x2 = x2;
    this.m_y2 = y2;
  }

  public PdfLine(PointF point1, PointF point2)
    : this(point1.X, point1.Y, point2.X, point2.Y)
  {
  }

  public PdfLine(PdfPen pen, float x1, float y1, float x2, float y2)
    : base(pen)
  {
    this.m_x1 = x1;
    this.m_y1 = y1;
    this.m_x2 = x2;
    this.m_y2 = y2;
  }

  public PdfLine(PdfPen pen, PointF point1, PointF point2)
    : base(pen)
  {
    this.m_x1 = point1.X;
    this.m_y1 = point1.Y;
    this.m_x2 = point2.X;
    this.m_y2 = point2.Y;
  }

  private PdfLine()
  {
  }

  public float X1
  {
    get => this.m_x1;
    set => this.m_x1 = value;
  }

  public float Y1
  {
    get => this.m_y1;
    set => this.m_y1 = value;
  }

  public float X2
  {
    get => this.m_x2;
    set => this.m_x2 = value;
  }

  public float Y2
  {
    get => this.m_y2;
    set => this.m_y2 = value;
  }

  protected override RectangleF GetBoundsInternal()
  {
    float x = Math.Min(this.X1, this.X2);
    float num1 = Math.Max(this.X1, this.X2);
    float y = Math.Min(this.Y1, this.Y2);
    float num2 = Math.Max(this.Y1, this.Y2);
    return new RectangleF(x, y, num1 - x, num2 - y);
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawLine(this.ObtainPen(), this.X1, this.Y1, this.X2, this.Y2);
  }
}
