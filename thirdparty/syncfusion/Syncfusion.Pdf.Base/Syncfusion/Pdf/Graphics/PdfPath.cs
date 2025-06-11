// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfPath
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfPath : PdfFillElement
{
  private List<PointF> m_points;
  private List<byte> m_pathTypes;
  private bool m_bStartFigure = true;
  private PdfFillMode m_fillMode;
  private bool isBeziers3;
  internal bool isXps;

  public PdfPath()
  {
  }

  public PdfPath(PointF[] points, byte[] pathTypes) => this.AddPath(points, pathTypes);

  public PdfPath(PdfPen pen)
    : base(pen)
  {
  }

  public PdfPath(PdfBrush brush)
    : base(brush)
  {
  }

  public PdfPath(PdfBrush brush, PdfFillMode fillMode)
    : this(brush)
  {
    this.FillMode = fillMode;
  }

  public PdfPath(PdfPen pen, PointF[] points, byte[] pathTypes)
    : base(pen)
  {
    this.AddPath(points, pathTypes);
  }

  public PdfPath(PdfBrush brush, PdfFillMode fillMode, PointF[] points, byte[] pathTypes)
    : base(brush)
  {
    this.AddPath(points, pathTypes);
    this.FillMode = fillMode;
  }

  public PdfPath(PdfPen pen, PdfBrush brush, PdfFillMode fillMode)
    : base(pen, brush)
  {
    this.FillMode = fillMode;
  }

  public PdfFillMode FillMode
  {
    get => this.m_fillMode;
    set => this.m_fillMode = value;
  }

  public PointF[] PathPoints => this.Points.ToArray();

  public byte[] PathTypes => this.Types.ToArray();

  public int PointCount
  {
    get
    {
      int pointCount = 0;
      if (this.m_points != null)
        pointCount = this.m_points.Count;
      return pointCount;
    }
  }

  public PointF LastPoint => this.GetLastPoint();

  internal List<PointF> Points
  {
    get
    {
      if (this.m_points == null)
        this.m_points = new List<PointF>();
      return this.m_points;
    }
  }

  internal List<byte> Types
  {
    get
    {
      if (this.m_pathTypes == null)
        this.m_pathTypes = new List<byte>();
      return this.m_pathTypes;
    }
  }

  public void AddArc(RectangleF rectangle, float startAngle, float sweepAngle)
  {
    this.AddArc(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, startAngle, sweepAngle);
  }

  public void AddArc(
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    List<float[]> bezierArcPoints = PdfGraphics.GetBezierArcPoints(x, y, x + width, y + height, startAngle, sweepAngle);
    List<float> points = new List<float>(8);
    for (int index = 0; index < bezierArcPoints.Count; ++index)
    {
      float[] collection = bezierArcPoints[index];
      points.Clear();
      points.AddRange((IEnumerable<float>) collection);
      this.AddPoints(points, PathPointType.Bezier);
    }
  }

  public void AddBezier(
    PointF startPoint,
    PointF firstControlPoint,
    PointF secondControlPoint,
    PointF endPoint)
  {
    this.AddBezier(startPoint.X, startPoint.Y, firstControlPoint.X, firstControlPoint.Y, secondControlPoint.X, secondControlPoint.Y, endPoint.X, endPoint.Y);
  }

  public void AddBezier(
    float startPointX,
    float startPointY,
    float firstControlPointX,
    float firstControlPointY,
    float secondControlPointX,
    float secondControlPointY,
    float endPointX,
    float endPointY)
  {
    this.AddPoints(new List<float>(8)
    {
      startPointX,
      startPointY,
      firstControlPointX,
      firstControlPointY,
      secondControlPointX,
      secondControlPointY,
      endPointX,
      endPointY
    }, PathPointType.Bezier);
  }

  internal void AddBeziers(List<PointF> points)
  {
    List<float> points1 = new List<float>();
    int num = 0;
    for (int index = 0; index < points.Count; ++index)
    {
      points1.Add(points[index].X);
      points1.Add(points[index].Y);
      if (num == 4)
      {
        this.isBeziers3 = true;
        this.AddPoints(points1, PathPointType.Bezier);
        points1 = new List<float>();
        num = 0;
        this.isBeziers3 = false;
      }
    }
    if (points1.Count <= 0)
      return;
    this.isBeziers3 = true;
    this.AddPoints(points1, PathPointType.Bezier);
  }

  public void AddEllipse(RectangleF rectangle)
  {
    this.AddEllipse(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void AddEllipse(float x, float y, float width, float height)
  {
    this.StartFigure();
    this.AddArc(x, y, width, height, 0.0f, 360f);
    this.CloseFigure();
  }

  public void AddLine(PointF point1, PointF point2)
  {
    this.AddLine(point1.X, point1.Y, point2.X, point2.Y);
  }

  public void AddLine(float x1, float y1, float x2, float y2)
  {
    this.AddPoints(new List<float>(4) { x1, y1, x2, y2 }, PathPointType.Line);
  }

  public void AddPath(PdfPath path) => this.AddPath(path.PathPoints, path.PathTypes);

  public void AddPath(PointF[] pathPoints, byte[] pathTypes)
  {
    if (pathPoints == null)
      throw new ArgumentNullException(nameof (pathPoints));
    if (pathTypes == null)
      throw new ArgumentNullException(nameof (pathTypes));
    if (pathPoints.Length != pathTypes.Length)
      throw new ArgumentException("The argument arrays should be of equal length.");
    this.Points.AddRange((IEnumerable<PointF>) pathPoints);
    this.Types.AddRange((IEnumerable<byte>) pathTypes);
  }

  public void AddPie(RectangleF rectangle, float startAngle, float sweepAngle)
  {
    this.AddPie(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, startAngle, sweepAngle);
  }

  public void AddPie(
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    this.StartFigure();
    this.AddArc(x, y, width, height, startAngle, sweepAngle);
    this.AddPoint(new PointF(x + width / 2f, y + height / 2f), PathPointType.Line);
    this.CloseFigure();
  }

  public void AddPolygon(PointF[] points)
  {
    List<float> points1 = new List<float>(points.Length * 2);
    this.StartFigure();
    foreach (PointF point in points)
    {
      points1.Add(point.X);
      points1.Add(point.Y);
    }
    this.AddPoints(points1, PathPointType.Line);
    this.CloseFigure();
  }

  public void AddRectangle(RectangleF rectangle)
  {
    this.AddRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void AddRectangle(float x, float y, float width, float height)
  {
    List<float> points = new List<float>();
    this.StartFigure();
    points.Add(x);
    points.Add(y);
    points.Add(x + width);
    points.Add(y);
    points.Add(x + width);
    points.Add(y + height);
    points.Add(x);
    points.Add(y + height);
    this.AddPoints(points, PathPointType.Line);
    this.CloseFigure();
  }

  public void StartFigure() => this.m_bStartFigure = true;

  public void CloseFigure()
  {
    if (this.PointCount > 0)
      this.CloseFigure(this.PointCount - 1);
    this.StartFigure();
  }

  public void CloseAllFigures()
  {
    PointF pathPoint = this.PathPoints[0];
    int index = 0;
    for (int count = this.m_pathTypes.Count; index < count; ++index)
    {
      PathPointType type = (PathPointType) this.Types[index];
      bool flag = false;
      if (index != 0 && type == PathPointType.Start)
        this.CloseFigure(index - 1);
      else if (index == this.m_pathTypes.Count - 1 && !flag && this.isXps && (double) pathPoint.X == (double) this.PathPoints[index].X)
        this.CloseFigure(index);
    }
  }

  public PointF GetLastPoint()
  {
    PointF lastPoint = PointF.Empty;
    int pointCount = this.PointCount;
    if (pointCount > 0 && this.m_points != null)
      lastPoint = this.m_points[pointCount - 1];
    return lastPoint;
  }

  internal void AddLines(PointF[] linePoints)
  {
    PointF point1 = linePoints[0];
    if (linePoints.Length == 1)
    {
      this.AddPoint(linePoints[0], PathPointType.Line);
    }
    else
    {
      for (int index = 1; index < linePoints.Length; ++index)
      {
        PointF linePoint = linePoints[index];
        this.AddLine(point1, linePoint);
        point1 = linePoint;
      }
    }
  }

  internal void Scale(float scaleX, float scaleY)
  {
    List<PointF> pointFList = new List<PointF>(this.m_points.Count);
    for (int index = 0; index < this.m_points.Count; ++index)
      pointFList.Add(new PointF(this.m_points[index].X * scaleX, this.m_points[index].Y * scaleY));
    this.m_points = pointFList;
  }

  internal void AddBeziers(PointF[] points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (points.Length < 4)
      throw new ArgumentException("Incorrect size of array", nameof (points));
    int num = 3;
    int index1 = 0;
    PointF startPoint = points[index1];
    int index2 = index1 + 1;
    while (index2 + num <= points.Length)
    {
      PointF point1 = points[index2];
      int index3 = index2 + 1;
      PointF point2 = points[index3];
      int index4 = index3 + 1;
      PointF point3 = points[index4];
      index2 = index4 + 1;
      this.AddBezier(startPoint, point1, point2, point3);
      startPoint = point3;
    }
  }

  protected override RectangleF GetBoundsInternal()
  {
    PointF[] pathPoints = this.PathPoints;
    RectangleF boundsInternal = RectangleF.Empty;
    if (pathPoints.Length > 0)
    {
      float num1 = pathPoints[0].X;
      float val2_1 = pathPoints[0].X;
      float num2 = pathPoints[0].Y;
      float val2_2 = pathPoints[0].Y;
      int index = 1;
      for (int length = pathPoints.Length; index < length; ++index)
      {
        PointF pointF = pathPoints[index];
        num1 = Math.Min(pointF.X, num1);
        val2_1 = Math.Max(pointF.X, val2_1);
        num2 = Math.Min(pointF.Y, num2);
        val2_2 = Math.Max(pointF.Y, val2_2);
      }
      boundsInternal = new RectangleF(num1, num2, val2_1 - num1, val2_2 - num2);
    }
    return boundsInternal;
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.PdfTag != null)
    {
      graphics.Tag = this.PdfTag;
      graphics.StructElementChanged += new PdfGraphics.StructElementEventHandler(graphics.ApplyTag);
      graphics.CurrentTagType = "Figure";
    }
    graphics.DrawPath(this.ObtainPen(), this.Brush, this);
  }

  private void AddPoints(List<float> points, PathPointType pointType)
  {
    this.AddPoints(points, pointType, 0, points.Count);
  }

  private void AddPoints(
    List<float> points,
    PathPointType pointType,
    int startIndex,
    int endIndex)
  {
    for (int index = startIndex; index < endIndex; index = index + 1 + 1)
    {
      PointF point = new PointF(points[index], points[index + 1]);
      if (index == startIndex)
      {
        if (this.PointCount <= 0 || this.m_bStartFigure)
        {
          this.AddPoint(point, PathPointType.Start);
          this.m_bStartFigure = false;
        }
        else if (point != this.LastPoint && !this.isBeziers3)
          this.AddPoint(point, PathPointType.Line);
        else if (point != this.LastPoint)
          this.AddPoint(point, PathPointType.Bezier);
      }
      else
        this.AddPoint(point, pointType);
    }
  }

  private void AddPoint(PointF point, PathPointType pointType)
  {
    this.Points.Add(point);
    this.Types.Add((byte) pointType);
  }

  private void CloseFigure(int index)
  {
    if (index < 0)
      throw new IndexOutOfRangeException();
    PathPointType pathPointType = (PathPointType) ((int) this.Types[index] | 128 /*0x80*/);
    this.Types[index] = (byte) pathPointType;
  }
}
