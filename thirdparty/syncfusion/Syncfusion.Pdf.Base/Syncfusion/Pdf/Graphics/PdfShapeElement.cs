// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfShapeElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.HtmlToPdf;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfShapeElement : PdfLayoutElement
{
  public RectangleF GetBounds() => this.GetBoundsInternal();

  protected abstract RectangleF GetBoundsInternal();

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    return param != null ? new ShapeLayouter(this).Layout(param) : throw new ArgumentNullException(nameof (param));
  }

  internal override PdfLayoutResult Layout(HtmlToPdfParams param)
  {
    return param != null ? new ShapeLayouter(this).Layout(param) : throw new ArgumentNullException(nameof (param));
  }
}
