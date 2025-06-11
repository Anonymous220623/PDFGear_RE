// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTextElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfTextElement : PdfLayoutElement
{
  private string m_text = string.Empty;
  private string m_value = string.Empty;
  private PdfPen m_pen;
  private PdfBrush m_brush;
  private PdfFont m_font;
  private PdfStringFormat m_format;
  internal bool ispdfTextElement;
  internal bool m_pdfHtmlTextElement;

  public PdfTextElement()
  {
  }

  public PdfTextElement(string text)
  {
    this.m_text = text != null ? text : throw new ArgumentNullException(nameof (text));
    text = PdfStandardFont.Convert(text);
    this.m_value = text;
  }

  public PdfTextElement(string text, PdfFont font)
    : this(text)
  {
    this.m_font = font != null ? font : throw new ArgumentNullException(nameof (font));
    if (this.m_font is PdfStandardFont && (this.m_font as PdfStandardFont).fontEncoding != null)
      this.m_value = PdfStandardFont.Convert(this.m_text, (this.m_font as PdfStandardFont).fontEncoding);
    else if (this.m_font is PdfStandardFont)
      this.m_value = PdfStandardFont.Convert(this.m_text);
    else
      this.m_value = this.m_text;
  }

  public PdfTextElement(string text, PdfFont font, PdfPen pen)
    : this(text, font)
  {
    this.m_pen = pen;
  }

  public PdfTextElement(string text, PdfFont font, PdfBrush brush)
    : this(text, font)
  {
    this.m_brush = brush;
  }

  public PdfTextElement(
    string text,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    PdfStringFormat format)
    : this(text, font, pen)
  {
    this.m_brush = brush;
    this.m_format = format;
  }

  public string Text
  {
    get => this.m_text;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
      this.m_value = this.m_font == null || !(this.m_font is PdfStandardFont) || (this.m_font as PdfStandardFont).fontEncoding == null ? (this.m_font == null || this.m_font is PdfStandardFont ? PdfStandardFont.Convert(value) : value) : PdfStandardFont.Convert(value, (this.m_font as PdfStandardFont).fontEncoding);
      this.m_text = value;
    }
  }

  internal string Value => this.m_value;

  public PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set
    {
      this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
      if (this.m_font is PdfStandardFont && (this.m_font as PdfStandardFont).fontEncoding != null && this.m_text != null)
        this.m_value = PdfStandardFont.Convert(this.m_text, (this.m_font as PdfStandardFont).fontEncoding);
      else if (this.m_font is PdfStandardFont && this.m_text != null)
        this.m_value = PdfStandardFont.Convert(this.m_text);
      else
        this.m_value = this.m_text;
    }
  }

  public PdfStringFormat StringFormat
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public PdfTextLayoutResult Draw(PdfPage page, PointF location, PdfLayoutFormat format)
  {
    RectangleF layoutRectangle = new RectangleF(location, SizeF.Empty);
    return this.Draw(page, layoutRectangle, format);
  }

  public PdfTextLayoutResult Draw(
    PdfPage page,
    PointF location,
    float width,
    PdfLayoutFormat format)
  {
    RectangleF layoutRectangle = new RectangleF(location.X, location.Y, width, 0.0f);
    return this.Draw(page, layoutRectangle, format);
  }

  public PdfTextLayoutResult Draw(PdfPage page, RectangleF layoutRectangle, PdfLayoutFormat format)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    return this.Layout(new PdfLayoutParams()
    {
      Page = page,
      Bounds = layoutRectangle,
      Format = format != null ? format : new PdfLayoutFormat()
    }) as PdfTextLayoutResult;
  }

  internal PdfBrush ObtainBrush() => this.m_brush != null ? this.m_brush : PdfBrushes.Black;

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (this.Font == null)
      throw new ArgumentNullException("Font can't be null");
    if (this.PdfTag != null)
      graphics.Tag = this.PdfTag;
    graphics.DrawString(this.Value, this.Font, this.Pen, this.ObtainBrush(), PointF.Empty, this.StringFormat);
  }

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    if (this.Font == null)
      throw new ArgumentNullException("Font can't be null");
    return new TextLayouter(this).Layout(param);
  }
}
