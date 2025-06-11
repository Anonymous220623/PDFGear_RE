// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLayoutElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.HtmlToPdf;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfLayoutElement : PdfGraphicsElement
{
  private bool m_bEmbedFonts;
  private PdfTag m_tag;

  public event EndPageLayoutEventHandler EndPageLayout;

  public event BeginPageLayoutEventHandler BeginPageLayout;

  internal bool RaiseEndPageLayout => this.EndPageLayout != null;

  internal bool RaiseBeginPageLayout => this.BeginPageLayout != null;

  internal bool EmbedFontResource => this.m_bEmbedFonts;

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public PdfLayoutResult Draw(PdfPage page, PointF location)
  {
    return this.Draw(page, location.X, location.Y);
  }

  public PdfLayoutResult Draw(PdfPage page, float x, float y)
  {
    return this.Draw(page, x, y, (PdfLayoutFormat) null);
  }

  public PdfLayoutResult Draw(PdfPage page, RectangleF layoutRectangle)
  {
    return this.Draw(page, layoutRectangle, (PdfLayoutFormat) null);
  }

  internal PdfLayoutResult Draw(PdfPage page, RectangleF layoutRectangle, bool embedFonts)
  {
    this.m_bEmbedFonts = embedFonts;
    return this.Draw(page, layoutRectangle, (PdfLayoutFormat) null);
  }

  public PdfLayoutResult Draw(PdfPage page, PointF location, PdfLayoutFormat format)
  {
    return this.Draw(page, location.X, location.Y, format);
  }

  public PdfLayoutResult Draw(PdfPage page, float x, float y, PdfLayoutFormat format)
  {
    RectangleF layoutRectangle = new RectangleF(x, y, 0.0f, 0.0f);
    return this.Draw(page, layoutRectangle, format);
  }

  public PdfLayoutResult Draw(PdfPage page, RectangleF layoutRectangle, PdfLayoutFormat format)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    return this.Layout(new PdfLayoutParams()
    {
      Page = page,
      Bounds = layoutRectangle,
      Format = format != null ? format : new PdfLayoutFormat()
    });
  }

  internal virtual PdfLayoutResult Layout(HtmlToPdfParams param) => (PdfLayoutResult) null;

  internal PdfLayoutResult Draw(PdfPage page, HtmlToPdfFormat format, RectangleF layoutRectangle)
  {
    if (page == null)
      throw new ArgumentNullException("page cannot be null");
    return this.Layout(new HtmlToPdfParams()
    {
      Page = page,
      Bounds = layoutRectangle,
      Format = format != null ? format : new HtmlToPdfFormat()
    });
  }

  internal PdfLayoutResult Draw(
    PdfPage page,
    RectangleF bounds,
    float[] pageOffsets,
    PdfLayoutFormat format)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    return this.Layout(new HtmlToPdfLayoutParams()
    {
      VerticalOffsets = pageOffsets,
      Page = page,
      Bounds = bounds,
      Format = format != null ? format : new PdfLayoutFormat()
    });
  }

  protected abstract PdfLayoutResult Layout(PdfLayoutParams param);

  protected virtual PdfLayoutResult Layout(HtmlToPdfLayoutParams param) => (PdfLayoutResult) null;

  internal void OnEndPageLayout(EndPageLayoutEventArgs e)
  {
    if (this.EndPageLayout == null)
      return;
    this.EndPageLayout((object) this, e);
  }

  internal void OnBeginPageLayout(BeginPageLayoutEventArgs e)
  {
    if (this.BeginPageLayout == null)
      return;
    this.BeginPageLayout((object) this, e);
  }
}
