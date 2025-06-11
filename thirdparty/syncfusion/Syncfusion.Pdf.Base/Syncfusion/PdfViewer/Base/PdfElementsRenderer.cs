// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PdfElementsRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PdfElementsRenderer
{
  private Dictionary<int, Dictionary<ushort, GraphicsPath>> glyphsCache = new Dictionary<int, Dictionary<ushort, GraphicsPath>>();
  private Dictionary<PathGeometry, GraphicsPath> shapesCache = new Dictionary<PathGeometry, GraphicsPath>();
  private readonly object syncObj = new object();

  public static System.Drawing.Drawing2D.Matrix GetTransformationMatrix(Matrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  public GraphicsPath GetGeometry(Path path) => this.GetGeometry(path.Data);

  public GraphicsPath GetGeometry(PathGeometry geometry)
  {
    lock (this.syncObj)
    {
      GraphicsPath geometry1;
      if (!this.shapesCache.TryGetValue(geometry, out geometry1))
      {
        geometry1 = this.GetGeometry(geometry, Matrix.Identity);
        this.shapesCache[geometry] = geometry1;
      }
      return (GraphicsPath) geometry1.Clone();
    }
  }

  public GraphicsPath GetGeometry(PathGeometry geometry, Matrix transform)
  {
    lock (this.syncObj)
    {
      System.Drawing.Drawing2D.Matrix transformationMatrix = PdfElementsRenderer.GetTransformationMatrix(transform);
      GraphicsPath graphicsPath = new GraphicsPath();
      foreach (PathFigure figure in geometry.Figures)
      {
        graphicsPath.StartFigure();
        PointF pointF = new PointF((float) figure.StartPoint.X, (float) figure.StartPoint.Y);
        foreach (PathSegment segment in figure.Segments)
        {
          if (segment is LineSegment)
          {
            LineSegment lineSegment = (LineSegment) segment;
            PointF[] pts = new PointF[2]
            {
              pointF,
              new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y)
            };
            transformationMatrix.TransformPoints(pts);
            graphicsPath.AddLine(pts[0], pts[1]);
            pointF = new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
          }
          else if (segment is BezierSegment)
          {
            BezierSegment bezierSegment = segment as BezierSegment;
            PointF[] pts = new PointF[4]
            {
              pointF,
              new PointF((float) bezierSegment.Point1.X, (float) bezierSegment.Point1.Y),
              new PointF((float) bezierSegment.Point2.X, (float) bezierSegment.Point2.Y),
              new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y)
            };
            transformationMatrix.TransformPoints(pts);
            graphicsPath.AddBezier(pts[0], pts[1], pts[2], pts[3]);
            pointF = new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y);
          }
        }
        if (figure.IsClosed)
          graphicsPath.CloseFigure();
      }
      return (GraphicsPath) graphicsPath.Clone();
    }
  }

  public GraphicsPath RenderGlyph(Glyph glyph)
  {
    lock (this.syncObj)
    {
      Dictionary<ushort, GraphicsPath> dictionary;
      if (!this.glyphsCache.TryGetValue(glyph.FontId, out dictionary))
      {
        dictionary = new Dictionary<ushort, GraphicsPath>();
        this.glyphsCache[glyph.FontId] = dictionary;
      }
      GraphicsPath graphicsPath = (GraphicsPath) null;
      if (!dictionary.TryGetValue(glyph.GlyphId, out graphicsPath))
      {
        graphicsPath = this.RenderGlyph(glyph, Matrix.Identity);
        dictionary[glyph.GlyphId] = graphicsPath;
      }
      return (GraphicsPath) graphicsPath.Clone();
    }
  }

  private GraphicsPath RenderGlyph(Glyph g, Matrix transform)
  {
    GraphicsPath graphicsPath1;
    lock (this.syncObj)
    {
      System.Drawing.Drawing2D.Matrix transformationMatrix = PdfElementsRenderer.GetTransformationMatrix(transform);
      GraphicsPath graphicsPath2 = new GraphicsPath();
      if (g.Outlines == null)
      {
        graphicsPath1 = graphicsPath2;
      }
      else
      {
        foreach (PathFigure outline in (List<PathFigure>) g.Outlines)
        {
          graphicsPath2.StartFigure();
          PointF pointF = new PointF((float) outline.StartPoint.X, (float) outline.StartPoint.Y);
          foreach (PathSegment segment in outline.Segments)
          {
            switch (segment)
            {
              case LineSegment _:
                LineSegment lineSegment = (LineSegment) segment;
                PointF[] pts1 = new PointF[2]
                {
                  pointF,
                  new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y)
                };
                transformationMatrix.TransformPoints(pts1);
                graphicsPath2.AddLine(pts1[0], pts1[1]);
                pointF = new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
                continue;
              case BezierSegment _:
                BezierSegment bezierSegment = segment as BezierSegment;
                PointF[] pts2 = new PointF[4]
                {
                  pointF,
                  new PointF((float) bezierSegment.Point1.X, (float) bezierSegment.Point1.Y),
                  new PointF((float) bezierSegment.Point2.X, (float) bezierSegment.Point2.Y),
                  new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y)
                };
                transformationMatrix.TransformPoints(pts2);
                graphicsPath2.AddBezier(pts2[0], pts2[1], pts2[2], pts2[3]);
                pointF = new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y);
                continue;
              case QuadraticBezierSegment _:
                QuadraticBezierSegment quadraticBezierSegment = segment as QuadraticBezierSegment;
                PointF[] pts3 = new PointF[3]
                {
                  pointF,
                  new PointF((float) quadraticBezierSegment.Point1.X, (float) quadraticBezierSegment.Point1.Y),
                  new PointF((float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y)
                };
                transformationMatrix.TransformPoints(pts3);
                graphicsPath2.AddBezier(pts3[0], pts3[1], pts3[2], pts3[2]);
                pointF = new PointF((float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y);
                continue;
              default:
                continue;
            }
          }
          if (outline.IsClosed)
            graphicsPath2.CloseFigure();
        }
        graphicsPath1 = (GraphicsPath) graphicsPath2.Clone();
      }
    }
    return graphicsPath1;
  }

  public void ClearCache()
  {
    this.glyphsCache.Clear();
    this.shapesCache.Clear();
  }
}
