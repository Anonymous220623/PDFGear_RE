// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PathFigure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PathFigure
{
  public List<PathSegment> Segments { get; set; }

  public bool IsClosed { get; set; }

  public bool IsFilled { get; set; }

  public Point StartPoint { get; set; }

  public PathFigure() => this.Segments = new List<PathSegment>();

  public PathFigure Clone()
  {
    PathFigure pathFigure = new PathFigure();
    pathFigure.IsClosed = this.IsClosed;
    pathFigure.IsFilled = this.IsFilled;
    pathFigure.StartPoint = this.StartPoint;
    foreach (PathSegment segment in this.Segments)
      pathFigure.Segments.Add(segment.Clone());
    return pathFigure;
  }

  internal void Transform(Matrix transformMatrix)
  {
    this.StartPoint = transformMatrix.Transform(this.StartPoint);
    foreach (PathSegment segment in this.Segments)
      segment.Transform(transformMatrix);
  }
}
