// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.LineSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class LineSegment : PathSegment
{
  public Point Point { get; set; }

  public override PathSegment Clone()
  {
    return (PathSegment) new LineSegment()
    {
      Point = this.Point
    };
  }

  public override void Transform(Matrix transformMatrix)
  {
    this.Point = transformMatrix.Transform(this.Point);
  }
}
