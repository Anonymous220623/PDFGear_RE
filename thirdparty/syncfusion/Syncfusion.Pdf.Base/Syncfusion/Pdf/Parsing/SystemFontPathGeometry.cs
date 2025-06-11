// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPathGeometry
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPathGeometry
{
  public List<SystemFontPathFigure> Figures { get; set; }

  public SystemFontFillRule FillRule { get; set; }

  public SystemFontMatrix TransformMatrix { get; set; }

  public bool IsEmpty => this.Figures == null || this.Figures.Count == 0;

  internal static SystemFontPathGeometry CreateRectangle(Rect rect)
  {
    return new SystemFontPathGeometry()
    {
      Figures = {
        new SystemFontPathFigure()
        {
          StartPoint = new Point(rect.Left, rect.Top),
          IsClosed = true,
          IsFilled = true,
          Segments = {
            (SystemFontPathSegment) new SystemFontLineSegment()
            {
              Point = new Point(rect.Right, rect.Top)
            },
            (SystemFontPathSegment) new SystemFontLineSegment()
            {
              Point = new Point(rect.Right, rect.Bottom)
            },
            (SystemFontPathSegment) new SystemFontLineSegment()
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

  public SystemFontPathGeometry()
  {
    this.Figures = new List<SystemFontPathFigure>();
    this.TransformMatrix = SystemFontMatrix.Identity;
  }

  public SystemFontPathGeometry Clone()
  {
    SystemFontPathGeometry fontPathGeometry = new SystemFontPathGeometry();
    fontPathGeometry.FillRule = this.FillRule;
    fontPathGeometry.TransformMatrix = this.TransformMatrix;
    foreach (SystemFontPathFigure figure in this.Figures)
      fontPathGeometry.Figures.Add(figure.Clone());
    return fontPathGeometry;
  }

  public Rect GetBoundingRect()
  {
    double minX = double.PositiveInfinity;
    double minY = double.PositiveInfinity;
    double maxX = double.NegativeInfinity;
    double maxY = double.NegativeInfinity;
    foreach (SystemFontPathFigure figure in this.Figures)
    {
      SystemFontPathGeometry.Compare(figure.StartPoint, ref minX, ref maxX, ref minY, ref maxY);
      foreach (SystemFontPathSegment segment in figure.Segments)
      {
        if (segment is SystemFontLineSegment)
          SystemFontPathGeometry.Compare(((SystemFontLineSegment) segment).Point, ref minX, ref maxX, ref minY, ref maxY);
        else if (segment is SystemFontBezierSegment)
        {
          SystemFontBezierSegment fontBezierSegment = (SystemFontBezierSegment) segment;
          SystemFontPathGeometry.Compare(fontBezierSegment.Point1, ref minX, ref maxX, ref minY, ref maxY);
          SystemFontPathGeometry.Compare(fontBezierSegment.Point2, ref minX, ref maxX, ref minY, ref maxY);
          SystemFontPathGeometry.Compare(fontBezierSegment.Point3, ref minX, ref maxX, ref minY, ref maxY);
        }
      }
    }
    return new Rect(new Point(minX, minY), new Point(maxX, maxY));
  }
}
