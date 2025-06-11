// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTextLayoutResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfTextLayoutResult : PdfLayoutResult
{
  private string m_remainder;
  private RectangleF m_lastLineBounds;

  public string Remainder => this.m_remainder;

  public RectangleF LastLineBounds => this.m_lastLineBounds;

  internal PdfTextLayoutResult(
    PdfPage page,
    RectangleF bounds,
    string remainder,
    RectangleF lastLineBounds)
    : base(page, bounds)
  {
    this.m_remainder = remainder;
    this.m_lastLineBounds = lastLineBounds;
  }
}
