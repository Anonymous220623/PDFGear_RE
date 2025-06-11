// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.QuadraticBezierSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class QuadraticBezierSegment : PathSegment
{
  public Point Point1 { get; set; }

  public Point Point2 { get; set; }

  public override PathSegment Clone()
  {
    return (PathSegment) new QuadraticBezierSegment()
    {
      Point1 = this.Point1,
      Point2 = this.Point2
    };
  }

  public override void Transform(Matrix transformMatrix)
  {
    this.Point1 = transformMatrix.Transform(this.Point1);
    this.Point2 = transformMatrix.Transform(this.Point2);
  }
}
