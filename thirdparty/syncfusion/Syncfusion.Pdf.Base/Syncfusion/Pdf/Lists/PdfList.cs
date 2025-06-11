// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public abstract class PdfList : PdfLayoutElement
{
  protected static readonly char[] c_splitChars = new char[1]
  {
    '\n'
  };
  private PdfListItemCollection m_items;
  private float m_indent = 10f;
  private float m_textIndent = 5f;
  private PdfFont m_font;
  private PdfPen m_pen;
  private PdfBrush m_brush;
  private PdfStringFormat m_format;

  protected static PdfListItemCollection CreateItems(string text)
  {
    return text != null ? new PdfListItemCollection(text.Split(PdfList.c_splitChars)) : throw new ArgumentNullException(nameof (text));
  }

  public PdfListItemCollection Items
  {
    get
    {
      if (this.m_items == null)
        this.m_items = new PdfListItemCollection();
      return this.m_items;
    }
  }

  public float Indent
  {
    get => this.m_indent;
    set => this.m_indent = value;
  }

  public float TextIndent
  {
    get => this.m_textIndent;
    set => this.m_textIndent = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value != null ? value : throw new ArgumentNullException("font");
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  public PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  public PdfStringFormat StringFormat
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  internal bool RiseBeginItemLayout => this.BeginItemLayout != null;

  internal bool RiseEndItemLayout => this.EndItemLayout != null;

  public event BeginItemLayoutEventHandler BeginItemLayout;

  public event EndItemLayoutEventHandler EndItemLayout;

  internal PdfList()
  {
  }

  internal PdfList(PdfListItemCollection items)
  {
    this.m_items = items != null ? items : throw new ArgumentException("Items collection can't be null", nameof (items));
  }

  internal PdfList(PdfFont font) => this.Font = font;

  public override void Draw(PdfGraphics graphics, float x, float y)
  {
    new PdfListLayouter(this).Layout(graphics, x, y);
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    new PdfListLayouter(this).Layout(graphics, PointF.Empty);
  }

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    return new PdfListLayouter(this).Layout(param);
  }

  internal void OnBeginItemLayout(BeginItemLayoutEventArgs args)
  {
    if (!this.RiseBeginItemLayout)
      return;
    this.BeginItemLayout((object) this, args);
  }

  internal void OnEndItemLayout(EndItemLayoutEventArgs args)
  {
    if (!this.RiseEndItemLayout)
      return;
    this.EndItemLayout((object) this, args);
  }
}
