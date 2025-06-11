// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfCustomLineCap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfCustomLineCap
{
  private PdfPath m_fillPath;
  private PdfPath m_strokePath;
  private PdfLineCap m_baseCap;
  private PdfLineJoin m_strokeJoin;
  private float m_widthScale = 1f;
  private PdfLineCap m_startCap;
  private PdfLineCap m_endCap;
  private float m_baseInset;

  public PdfCustomLineCap(PdfPath fillPath, PdfPath strokePath)
  {
    this.m_fillPath = fillPath;
    this.m_strokePath = strokePath;
  }

  public PdfCustomLineCap(PdfPath fillPath, PdfPath strokePath, PdfLineCap baseCap)
  {
    this.m_fillPath = fillPath;
    this.m_strokePath = strokePath;
    this.m_baseCap = baseCap;
  }

  public PdfCustomLineCap(
    PdfPath fillPath,
    PdfPath strokePath,
    PdfLineCap baseCap,
    float baseInset)
  {
    this.m_fillPath = fillPath;
    this.m_strokePath = strokePath;
    this.m_baseCap = baseCap;
    this.m_baseInset = baseInset;
  }

  public PdfLineCap BaseCap
  {
    get => this.m_baseCap;
    set => this.m_baseCap = value;
  }

  public float BaseInset
  {
    get => this.m_baseInset;
    set => this.m_baseInset = value;
  }

  public PdfLineJoin StrokeJoin
  {
    get => this.m_strokeJoin;
    set => this.m_strokeJoin = value;
  }

  public float WidthScale
  {
    get => this.m_widthScale;
    set => this.m_widthScale = value;
  }

  public void SetStrokeCaps(PdfLineCap startCap, PdfLineCap endCap)
  {
    this.m_startCap = startCap;
    this.m_endCap = endCap;
  }

  internal void DrawCustomCap(PdfGraphics graphics, PointF[] points, PdfPen pen, bool isStartCap)
  {
    PointF[] pointFArray = (PointF[]) null;
    if (this.m_fillPath != null)
      pointFArray = this.m_fillPath.PathPoints;
    else if (this.m_strokePath != null)
      pointFArray = this.m_strokePath.PathPoints;
    if (pointFArray == null)
      return;
    PdfGraphicsState state = graphics.Save();
    PointF empty1 = PointF.Empty;
    PointF empty2 = PointF.Empty;
    PointF point1;
    PointF point2;
    if (isStartCap)
    {
      point1 = points[1];
      point2 = points[0];
    }
    else
    {
      point1 = points[points.Length - 2];
      point2 = points[points.Length - 1];
    }
    float num1 = point1.X - point2.X;
    float num2 = point1.Y - point2.Y;
    float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    double num4 = (double) num1 / (double) num2;
    float num5 = (float) Math.Atan((double) (num2 / num1));
    if ((double) (num1 / num3) < 0.0)
      num5 += 3.14159274f;
    graphics.TranslateTransform(point2.X, point2.Y);
    graphics.RotateTransform((float) ((double) num5 * 180.0 / Math.PI) + 90f);
    float num6 = pen.Width / 2f;
    if (this.m_fillPath != null)
    {
      this.m_fillPath.Scale(num6, num6);
      graphics.DrawPath((PdfBrush) new PdfSolidBrush(pen.Color), this.m_fillPath);
    }
    if (this.m_strokePath != null)
    {
      this.m_strokePath.Scale(num6, num6);
      graphics.DrawPath(new PdfPen((PdfBrush) new PdfSolidBrush(pen.Color), pen.Width)
      {
        LineCap = pen.LineCap,
        LineJoin = pen.LineJoin,
        EndCap = pen.EndCap,
        StartCap = pen.StartCap
      }, this.m_strokePath);
    }
    graphics.Restore(state);
  }
}
