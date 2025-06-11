// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfPolygon
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfPolygon : PdfFillElement
{
  private List<PointF> m_points;

  public PdfPolygon(PointF[] points)
  {
    this.Points = points != null ? points : throw new ArgumentNullException(nameof (points));
  }

  public PdfPolygon(PdfPen pen, PointF[] points)
    : base(pen)
  {
    this.Points = points != null ? points : throw new ArgumentNullException(nameof (points));
  }

  public PdfPolygon(PdfBrush brush, PointF[] points)
    : base(brush)
  {
    this.Points = points != null ? points : throw new ArgumentNullException(nameof (points));
  }

  public PdfPolygon(PdfPen pen, PdfBrush brush, PointF[] points)
    : base(pen, brush)
  {
    this.Points = points != null ? points : throw new ArgumentNullException(nameof (points));
  }

  protected PdfPolygon() => this.m_points = new List<PointF>();

  public PointF[] Points
  {
    get
    {
      if (this.m_points == null)
        this.m_points = new List<PointF>();
      return this.m_points.ToArray();
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Points));
      if (this.m_points == null)
        this.m_points = new List<PointF>();
      this.m_points.Clear();
      this.m_points.AddRange((IEnumerable<PointF>) value);
    }
  }

  public int Count => this.Points.Length;

  public void AddPoint(PointF point) => this.m_points.Add(point);

  protected override RectangleF GetBoundsInternal()
  {
    RectangleF boundsInternal = RectangleF.Empty;
    if (this.Points.Length > 0)
    {
      PointF[] points = this.Points;
      float num1 = points[0].X;
      float val1_1 = points[0].X;
      float num2 = points[0].Y;
      float val1_2 = points[0].Y;
      for (int index = 1; index < points.Length; ++index)
      {
        PointF pointF = points[index];
        num1 = Math.Min(num1, pointF.X);
        val1_1 = Math.Max(val1_1, pointF.X);
        num2 = Math.Min(num2, pointF.Y);
        val1_2 = Math.Max(val1_2, pointF.Y);
      }
      boundsInternal = new RectangleF(num1, num2, val1_1 - num1, val1_2 - num2);
    }
    return boundsInternal;
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawPolygon(this.ObtainPen(), this.Brush, this.Points);
  }
}
