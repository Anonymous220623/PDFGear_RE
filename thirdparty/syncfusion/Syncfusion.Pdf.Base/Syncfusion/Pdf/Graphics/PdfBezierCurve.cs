// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBezierCurve
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfBezierCurve : PdfDrawElement
{
  private PointF m_startPoint = PointF.Empty;
  private PointF m_firstControlPoint = PointF.Empty;
  private PointF m_secondControlPoint = PointF.Empty;
  private PointF m_endPoint = PointF.Empty;

  public PdfBezierCurve(
    PointF startPoint,
    PointF firstControlPoint,
    PointF secondControlPoint,
    PointF endPoint)
  {
    this.m_startPoint = startPoint;
    this.m_firstControlPoint = firstControlPoint;
    this.m_secondControlPoint = secondControlPoint;
    this.m_endPoint = endPoint;
  }

  public PdfBezierCurve(
    float startPointX,
    float startPointY,
    float firstControlPointX,
    float firstControlPointY,
    float secondControlPointX,
    float secondControlPointY,
    float endPointX,
    float endPointY)
  {
    this.m_startPoint.X = startPointX;
    this.m_startPoint.Y = startPointY;
    this.m_firstControlPoint.X = firstControlPointX;
    this.m_firstControlPoint.Y = firstControlPointY;
    this.m_secondControlPoint.X = secondControlPointX;
    this.m_secondControlPoint.Y = secondControlPointY;
    this.m_endPoint.X = endPointX;
    this.m_endPoint.Y = endPointY;
  }

  public PdfBezierCurve(
    PdfPen pen,
    PointF startPoint,
    PointF firstControlPoint,
    PointF secondControlPoint,
    PointF endPoint)
    : base(pen)
  {
    this.m_startPoint = startPoint;
    this.m_firstControlPoint = firstControlPoint;
    this.m_secondControlPoint = secondControlPoint;
    this.m_endPoint = endPoint;
  }

  public PdfBezierCurve(
    PdfPen pen,
    float startPointX,
    float startPointY,
    float firstControlPointX,
    float firstControlPointY,
    float secondControlPointX,
    float secondControlPointY,
    float endPointX,
    float endPointY)
    : base(pen)
  {
    this.m_startPoint.X = startPointX;
    this.m_startPoint.Y = startPointY;
    this.m_firstControlPoint.X = firstControlPointX;
    this.m_firstControlPoint.Y = firstControlPointY;
    this.m_secondControlPoint.X = secondControlPointX;
    this.m_secondControlPoint.Y = secondControlPointY;
    this.m_endPoint.X = endPointX;
    this.m_endPoint.Y = endPointY;
  }

  protected PdfBezierCurve()
  {
  }

  public PointF StartPoint
  {
    get => this.m_startPoint;
    set => this.m_startPoint = value;
  }

  public PointF FirstControlPoint
  {
    get => this.m_firstControlPoint;
    set => this.m_firstControlPoint = value;
  }

  public PointF SecondControlPoint
  {
    get => this.m_secondControlPoint;
    set => this.m_secondControlPoint = value;
  }

  public PointF EndPoint
  {
    get => this.m_endPoint;
    set => this.m_endPoint = value;
  }

  protected override RectangleF GetBoundsInternal() => throw new NotImplementedException();

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawBezier(this.ObtainPen(), this.StartPoint, this.FirstControlPoint, this.SecondControlPoint, this.EndPoint);
  }
}
