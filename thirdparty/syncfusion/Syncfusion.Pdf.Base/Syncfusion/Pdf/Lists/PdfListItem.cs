// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfListItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfListItem
{
  private PdfFont m_font;
  private string m_text;
  private PdfStringFormat m_format;
  private PdfPen m_pen;
  private PdfBrush m_brush;
  private PdfList m_list;
  private float m_textIndent;
  private PdfTag m_tag;

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value != null ? value : throw new ArgumentNullException("text");
  }

  public PdfStringFormat StringFormat
  {
    get => this.m_format;
    set => this.m_format = value;
  }

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

  public PdfList SubList
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  public float TextIndent
  {
    get => this.m_textIndent;
    set => this.m_textIndent = value;
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public PdfListItem()
    : this(string.Empty)
  {
  }

  public PdfListItem(string text)
    : this(text, (PdfFont) null, (PdfStringFormat) null, (PdfPen) null, (PdfBrush) null)
  {
  }

  public PdfListItem(string text, PdfFont font)
    : this(text, font, (PdfStringFormat) null, (PdfPen) null, (PdfBrush) null)
  {
  }

  public PdfListItem(string text, PdfFont font, PdfStringFormat format)
    : this(text, font, format, (PdfPen) null, (PdfBrush) null)
  {
  }

  public PdfListItem(
    string text,
    PdfFont font,
    PdfStringFormat format,
    PdfPen pen,
    PdfBrush brush)
  {
    this.m_text = text != null ? text : throw new ArgumentNullException(nameof (text));
    this.m_font = font;
    this.m_format = format;
    this.m_pen = pen;
    this.m_brush = brush;
  }
}
