// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfUnorderedMarker
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfUnorderedMarker : PdfMarker
{
  private string m_text;
  private PdfUnorderedMarkerStyle m_style;
  private PdfImage m_image;
  private PdfTemplate m_template;
  private SizeF m_size;
  private PdfFont m_unicodeFont;

  public PdfTemplate Template
  {
    get => this.m_template;
    set
    {
      this.m_template = value != null ? value : throw new ArgumentNullException("template");
      this.m_style = PdfUnorderedMarkerStyle.CustomTemplate;
    }
  }

  public PdfImage Image
  {
    get => this.m_image;
    set
    {
      this.m_image = value != null ? value : throw new ArgumentNullException("image");
      this.m_style = PdfUnorderedMarkerStyle.CustomImage;
    }
  }

  public string Text
  {
    get => this.m_text;
    set
    {
      this.m_text = value != null ? value : throw new ArgumentNullException("text");
      this.m_style = PdfUnorderedMarkerStyle.CustomString;
    }
  }

  public PdfUnorderedMarkerStyle Style
  {
    get => this.m_style;
    set => this.m_style = value;
  }

  internal SizeF Size
  {
    get => this.m_size;
    set => this.m_size = value;
  }

  internal PdfFont UnicodeFont
  {
    get => this.m_unicodeFont;
    set => this.m_unicodeFont = value;
  }

  public PdfUnorderedMarker(string text, PdfFont font)
  {
    this.Font = font;
    this.Text = text;
    this.m_style = PdfUnorderedMarkerStyle.CustomString;
  }

  public PdfUnorderedMarker(PdfUnorderedMarkerStyle style) => this.m_style = style;

  public PdfUnorderedMarker(PdfImage image)
  {
    this.Image = image;
    this.m_style = PdfUnorderedMarkerStyle.CustomImage;
  }

  public PdfUnorderedMarker(PdfTemplate template)
  {
    this.Template = template;
    this.m_style = PdfUnorderedMarkerStyle.CustomTemplate;
  }

  internal void Draw(PdfGraphics graphics, PointF point, PdfBrush brush, PdfPen pen)
  {
    PdfTemplate template = new PdfTemplate(this.m_size);
    template.Graphics.Tag = graphics.Tag;
    switch (this.m_style)
    {
      case PdfUnorderedMarkerStyle.CustomImage:
        template.Graphics.DrawImage(this.m_image, 1f, 1f, this.m_size.Width - 2f, this.m_size.Height - 2f);
        break;
      case PdfUnorderedMarkerStyle.CustomTemplate:
        template = new PdfTemplate(this.m_size);
        template.Graphics.DrawPdfTemplate(this.m_template, PointF.Empty, this.m_size);
        break;
      default:
        PointF empty = PointF.Empty;
        if (pen != null)
        {
          empty.X += pen.Width;
          empty.Y += pen.Width;
        }
        template.Graphics.DrawString(this.GetStyledText(), this.m_unicodeFont, pen, brush, empty);
        break;
    }
    graphics.DrawPdfTemplate(template, point);
  }

  internal void Draw(PdfPage page, PointF point, PdfBrush brush, PdfPen pen)
  {
    this.Draw(page.Graphics, point, brush, pen);
  }

  internal string GetStyledText()
  {
    string styledText = string.Empty;
    switch (this.m_style)
    {
      case PdfUnorderedMarkerStyle.Disk:
        styledText = "l";
        break;
      case PdfUnorderedMarkerStyle.Square:
        styledText = "n";
        break;
      case PdfUnorderedMarkerStyle.Asterisk:
        styledText = "]";
        break;
      case PdfUnorderedMarkerStyle.Circle:
        styledText = "m";
        break;
    }
    return styledText;
  }
}
