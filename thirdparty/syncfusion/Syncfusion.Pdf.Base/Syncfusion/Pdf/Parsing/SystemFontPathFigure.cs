// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPathFigure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPathFigure
{
  public List<SystemFontPathSegment> Segments { get; set; }

  public bool IsClosed { get; set; }

  public bool IsFilled { get; set; }

  public Syncfusion.PdfViewer.Base.Point StartPoint { get; set; }

  public SystemFontPathFigure() => this.Segments = new List<SystemFontPathSegment>();

  public SystemFontPathFigure Clone()
  {
    SystemFontPathFigure systemFontPathFigure = new SystemFontPathFigure();
    systemFontPathFigure.IsClosed = this.IsClosed;
    systemFontPathFigure.IsFilled = this.IsFilled;
    systemFontPathFigure.StartPoint = this.StartPoint;
    foreach (SystemFontPathSegment segment in this.Segments)
      systemFontPathFigure.Segments.Add(segment.Clone());
    return systemFontPathFigure;
  }

  internal void Transform(SystemFontMatrix transformMatrix)
  {
    this.StartPoint = (Syncfusion.PdfViewer.Base.Point) transformMatrix.Transform((System.Drawing.Point) this.StartPoint);
    foreach (SystemFontPathSegment segment in this.Segments)
      segment.Transform(transformMatrix);
  }
}
