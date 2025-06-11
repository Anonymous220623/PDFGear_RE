// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontBezierSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontBezierSegment : SystemFontPathSegment
{
  public Syncfusion.PdfViewer.Base.Point Point1 { get; set; }

  public Syncfusion.PdfViewer.Base.Point Point2 { get; set; }

  public Syncfusion.PdfViewer.Base.Point Point3 { get; set; }

  public override SystemFontPathSegment Clone()
  {
    return (SystemFontPathSegment) new SystemFontBezierSegment()
    {
      Point1 = this.Point1,
      Point2 = this.Point2,
      Point3 = this.Point3
    };
  }

  public override void Transform(SystemFontMatrix transformMatrix)
  {
    this.Point1 = (Syncfusion.PdfViewer.Base.Point) transformMatrix.Transform((System.Drawing.Point) this.Point1);
    this.Point2 = (Syncfusion.PdfViewer.Base.Point) transformMatrix.Transform((System.Drawing.Point) this.Point2);
    this.Point3 = (Syncfusion.PdfViewer.Base.Point) transformMatrix.Transform((System.Drawing.Point) this.Point3);
  }
}
