// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLineSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLineSegment : SystemFontPathSegment
{
  public Syncfusion.PdfViewer.Base.Point Point { get; set; }

  public override SystemFontPathSegment Clone()
  {
    return (SystemFontPathSegment) new SystemFontLineSegment()
    {
      Point = this.Point
    };
  }

  public override void Transform(SystemFontMatrix transformMatrix)
  {
    this.Point = (Syncfusion.PdfViewer.Base.Point) transformMatrix.Transform((System.Drawing.Point) this.Point);
  }
}
