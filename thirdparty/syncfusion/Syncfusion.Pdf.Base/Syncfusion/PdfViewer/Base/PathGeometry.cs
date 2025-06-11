// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PathGeometry
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PathGeometry
{
  public List<PathFigure> Figures { get; set; }

  public FillRule FillRule { get; set; }

  public Matrix TransformMatrix { get; set; }

  public bool IsEmpty => this.Figures == null || this.Figures.Count == 0;

  internal static PathGeometry CreateRectangle(Rect rect)
  {
    return new PathGeometry()
    {
      Figures = {
        new PathFigure()
        {
          StartPoint = new Point(rect.Left, rect.Top),
          IsClosed = true,
          IsFilled = true,
          Segments = {
            (PathSegment) new LineSegment()
            {
              Point = new Point(rect.Right, rect.Top)
            },
            (PathSegment) new LineSegment()
            {
              Point = new Point(rect.Right, rect.Bottom)
            },
            (PathSegment) new LineSegment()
            {
              Point = new Point(rect.Left, rect.Bottom)
            }
          }
        }
      }
    };
  }

  private static void Compare(
    Point point,
    ref double minX,
    ref double maxX,
    ref double minY,
    ref double maxY)
  {
    minX = Math.Min(point.X, minX);
    maxX = Math.Max(point.X, maxX);
    minY = Math.Min(point.Y, minY);
    maxY = Math.Max(point.Y, maxY);
  }

  public PathGeometry()
  {
    this.Figures = new List<PathFigure>();
    this.TransformMatrix = Matrix.Identity;
  }

  public PathGeometry Clone()
  {
    PathGeometry pathGeometry = new PathGeometry();
    pathGeometry.FillRule = this.FillRule;
    pathGeometry.TransformMatrix = this.TransformMatrix;
    foreach (PathFigure figure in this.Figures)
      pathGeometry.Figures.Add(figure.Clone());
    return pathGeometry;
  }

  public Rect GetBoundingRect()
  {
    double minX = double.PositiveInfinity;
    double minY = double.PositiveInfinity;
    double maxX = double.NegativeInfinity;
    double maxY = double.NegativeInfinity;
    foreach (PathFigure figure in this.Figures)
    {
      PathGeometry.Compare(figure.StartPoint, ref minX, ref maxX, ref minY, ref maxY);
      foreach (PathSegment segment in figure.Segments)
      {
        if (segment is LineSegment)
          PathGeometry.Compare(((LineSegment) segment).Point, ref minX, ref maxX, ref minY, ref maxY);
        else if (segment is BezierSegment)
        {
          BezierSegment bezierSegment = (BezierSegment) segment;
          PathGeometry.Compare(bezierSegment.Point1, ref minX, ref maxX, ref minY, ref maxY);
          PathGeometry.Compare(bezierSegment.Point2, ref minX, ref maxX, ref minY, ref maxY);
          PathGeometry.Compare(bezierSegment.Point3, ref minX, ref maxX, ref minY, ref maxY);
        }
      }
    }
    return new Rect(new Point(minX, minY), new Point(maxX, maxY));
  }
}
