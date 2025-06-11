// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfHTMLTextElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfHTMLTextElement
{
  private PdfFont m_font;
  private PdfBrush m_brush;
  private string m_htmlText;
  private TextAlign m_textAlign;
  internal bool m_nativeRendering = true;
  internal bool m_isPdfGrid;
  internal RectangleF shapeBounds = RectangleF.Empty;
  internal float m_bottomCellpadding;
  private float m_height;
  private int m_htmlElementFont;
  private Color m_Color = Color.Black;
  private float m_htmlFontHeight;
  private float m_htmlFontElementHeight;
  private string m_stringFontName;
  private PdfFont m_currentFont;
  internal List<Htmltext> m_htmllist = new List<Htmltext>();
  private float xPosition;
  private float yPosition;
  private PdfBrush m_Htmlbrush;
  private PdfFontStyle m_style;
  private PdfFontStyle m_style1;
  private PdfFontFamily m_fontFace = PdfFontFamily.TimesRoman;
  private float m_initalXvalue;
  private float m_maxHeight = float.MinValue;
  private bool m_fontAttribute;

  public PdfHTMLTextElement()
  {
    this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 3f);
    this.m_brush = PdfBrushes.Black;
    this.m_htmlText = "";
    this.m_textAlign = TextAlign.Left;
  }

  public PdfHTMLTextElement(string htmlText, PdfFont font, PdfBrush brush)
  {
    this.m_htmlText = htmlText;
    this.m_font = font;
    this.m_brush = brush;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  internal float Height => this.m_height;

  public string HTMLText
  {
    get => this.m_htmlText;
    set => this.m_htmlText = value;
  }

  public TextAlign TextAlign
  {
    get => this.m_textAlign;
    set => this.m_textAlign = value;
  }

  public bool IsNativeRenderingEnabled
  {
    get => this.m_nativeRendering;
    set => this.m_nativeRendering = value;
  }

  public void Draw(PdfGraphics graphics, RectangleF layoutRectangle)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if ((double) layoutRectangle.Height < 0.0)
      throw new ArgumentNullException("height");
    RichTextBoxExt richTextBox = new RichTextBoxExt();
    richTextBox.RenderHTML(this.m_htmlText, this.m_font, this.m_brush);
    richTextBox.SelectAll();
    richTextBox.SelectionAlignment = this.m_textAlign;
    richTextBox.Width = (int) layoutRectangle.Width;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    float width = pdfUnitConvertor.ConvertUnits(layoutRectangle.Width, PdfGraphicsUnit.Point, PdfGraphicsUnit.Pixel);
    float height = pdfUnitConvertor.ConvertUnits(layoutRectangle.Height, PdfGraphicsUnit.Point, PdfGraphicsUnit.Pixel);
    PdfImage pdfImage = PdfImage.FromImage(RtfToImage.ConvertToMetafile(richTextBox, width, height));
    pdfImage.BeginPageLayout += new BeginPageLayoutEventHandler(this.img_BeginPageLayout);
    pdfImage.EndPageLayout += new EndPageLayoutEventHandler(this.img_EndPageLayout);
    richTextBox.Dispose();
    pdfImage.Draw(graphics, new PointF(layoutRectangle.X, layoutRectangle.Y));
  }

  public void Draw(PdfGraphics graphics, PointF location, float width, float height)
  {
    RectangleF layoutRectangle = new RectangleF(location, new SizeF(width, height));
    this.Draw(graphics, layoutRectangle);
  }

  public PdfLayoutResult Draw(
    PdfPage page,
    PointF location,
    float width,
    float height,
    PdfMetafileLayoutFormat format)
  {
    return this.Draw(page, new RectangleF(location.X, location.Y, width, height), format);
  }

  public PdfLayoutResult Draw(
    PdfPage page,
    PointF location,
    float width,
    PdfMetafileLayoutFormat format)
  {
    if (!this.IsNativeRenderingEnabled)
      return this.Draw(page, new RectangleF(location.X, location.Y, width, float.MinValue), format);
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    width = pdfUnitConvertor.ConvertToPixels(width, PdfGraphicsUnit.Point);
    RichTextBoxExt richTextBoxExt = new RichTextBoxExt();
    richTextBoxExt.RenderHTML(this.m_htmlText, this.m_font, this.m_brush);
    richTextBoxExt.SelectAll();
    richTextBoxExt.SelectionAlignment = this.m_textAlign;
    Image image = RtfToImage.ConvertToImage(richTextBoxExt.Rtf, width, -1f, PdfImageType.Metafile);
    RectangleF layoutRectangle = new RectangleF(location, new SizeF(pdfUnitConvertor.ConvertFromPixels(width, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertFromPixels((float) image.Height, PdfGraphicsUnit.Point)));
    richTextBoxExt.Dispose();
    if (this.shapeBounds != RectangleF.Empty)
      this.shapeBounds = new RectangleF(this.shapeBounds.X, this.shapeBounds.Y, layoutRectangle.Width, layoutRectangle.Height - this.shapeBounds.Y);
    return this.Draw(page, layoutRectangle, format);
  }

  public PdfLayoutResult Draw(
    PdfPage page,
    RectangleF layoutRectangle,
    PdfMetafileLayoutFormat format)
  {
    if (this.IsNativeRenderingEnabled)
    {
      if (page == null)
        throw new ArgumentNullException(nameof (page));
      if ((double) layoutRectangle.Height < 0.0)
        throw new ArgumentNullException("height");
      RichTextBoxExt richTextBox = new RichTextBoxExt();
      richTextBox.RenderHTML(this.m_htmlText, this.m_font, this.m_brush);
      richTextBox.SelectAll();
      richTextBox.SelectionAlignment = this.m_textAlign;
      richTextBox.Width = (int) layoutRectangle.Width;
      PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
      float width = pdfUnitConvertor.ConvertUnits(layoutRectangle.Width, PdfGraphicsUnit.Point, PdfGraphicsUnit.Pixel);
      float height = pdfUnitConvertor.ConvertUnits(layoutRectangle.Height, PdfGraphicsUnit.Point, PdfGraphicsUnit.Pixel);
      PdfImage pdfImage = PdfImage.FromImage(RtfToImage.ConvertToMetafile(richTextBox, width, height));
      pdfImage.BeginPageLayout += new BeginPageLayoutEventHandler(this.img_BeginPageLayout);
      pdfImage.EndPageLayout += new EndPageLayoutEventHandler(this.img_EndPageLayout);
      if (this.m_isPdfGrid)
      {
        PdfMetafile pdfMetafile = pdfImage as PdfMetafile;
        pdfMetafile.m_isPdfGrid = true;
        pdfMetafile.m_bottomCellpadding = this.m_bottomCellpadding;
        if (this.shapeBounds != RectangleF.Empty)
          pdfMetafile.shapeBounds = this.shapeBounds;
      }
      this.m_height = pdfUnitConvertor.ConvertUnits((float) pdfImage.Height, PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point);
      richTextBox.Dispose();
      return pdfImage.Draw(page, new PointF(layoutRectangle.X, layoutRectangle.Y), (PdfLayoutFormat) format);
    }
    this.m_htmllist.Clear();
    this.ParseHtml(this.HTMLText);
    PdfLayoutResult pdfLayoutResult1 = (PdfLayoutResult) null;
    PdfFont pdfFont = (PdfFont) null;
    RectangleF rectangleF1 = layoutRectangle;
    this.m_initalXvalue = layoutRectangle.X;
    this.m_maxHeight = layoutRectangle.Height;
    float width1 = layoutRectangle.Width;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    PdfStringFormat format1 = new PdfStringFormat();
    format1.MeasureTrailingSpaces = true;
    for (int index = 0; index < this.m_htmllist.Count; ++index)
    {
      Htmltext htmltext = this.m_htmllist[index];
      if (!htmltext.mbaseBrushColor)
      {
        this.m_Color = htmltext.mbrushcolor;
        this.m_Htmlbrush = (PdfBrush) new PdfSolidBrush((PdfColor) this.m_Color);
      }
      else
        this.m_Htmlbrush = this.Brush;
      SizeF sizeF;
      RectangleF bounds;
      if (index == 0)
      {
        this.xPosition = rectangleF1.X;
        this.yPosition = rectangleF1.Y;
        if ((double) rectangleF1.Height == -3.4028234663852886E+38)
        {
          sizeF = page.GetClientSize();
          this.m_maxHeight = sizeF.Height;
        }
      }
      else
      {
        bounds = pdfLayoutResult1.Bounds;
        this.xPosition = bounds.Right;
        this.yPosition = (pdfLayoutResult1 as PdfTextLayoutResult).LastLineBounds.Y;
        if ((double) layoutRectangle.Width <= (double) pdfLayoutResult1.Bounds.Right + (double) htmltext.mfont.MeasureString(" ", format1).Width - (double) this.m_initalXvalue && (double) pdfLayoutResult1.Bounds.Y == (double) (pdfLayoutResult1 as PdfTextLayoutResult).LastLineBounds.Y)
        {
          this.xPosition = this.m_initalXvalue;
          this.yPosition += htmltext.mfont.Height;
          num1 = 0.0f;
        }
        else if ((double) pdfLayoutResult1.Bounds.Y != (double) (pdfLayoutResult1 as PdfTextLayoutResult).LastLineBounds.Y)
        {
          this.xPosition = (pdfLayoutResult1 as PdfTextLayoutResult).LastLineBounds.Right;
          num1 = num3;
        }
        else if ((double) layoutRectangle.Width > (double) pdfLayoutResult1.Bounds.Right + (double) htmltext.mfont.MeasureString(" ", format1).Width - (double) this.m_initalXvalue)
          num1 = pdfLayoutResult1.Bounds.Width + (pdfLayoutResult1.Bounds.X - this.m_initalXvalue);
        if ((double) htmltext.mfont.Height != (double) pdfFont.Height)
          this.yPosition += pdfFont.Height - htmltext.mfont.Height;
        num2 = pdfLayoutResult1.Bounds.Bottom;
      }
      PdfTextElement pdfTextElement = new PdfTextElement(htmltext.minnerText, htmltext.mfont, this.m_Htmlbrush);
      pdfTextElement.StringFormat = format1;
      if ((double) layoutRectangle.Height != -3.4028234663852886E+38)
        pdfTextElement.m_pdfHtmlTextElement = true;
      pdfTextElement.BeginPageLayout += new BeginPageLayoutEventHandler(this.img_BeginPageLayout);
      pdfTextElement.EndPageLayout += new EndPageLayoutEventHandler(this.img_EndPageLayout);
      pdfFont = htmltext.mfont;
      RectangleF rectangleF2 = new RectangleF(this.xPosition, this.yPosition, width1 - num1, this.m_maxHeight - num2);
      page.Graphics.Save();
      PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
      PdfStringLayoutResult stringLayoutResult1 = pdfStringLayouter.Layout(htmltext.minnerText, htmltext.mfont, format1, rectangleF2, page.GetClientSize().Height);
      if (stringLayoutResult1.LineCount > 1)
      {
        if ((double) page.GetClientSize().Height >= (double) stringLayoutResult1.ActualSize.Height + (double) stringLayoutResult1.LineHeight || format.Break != PdfLayoutBreakType.FitElement || format.Layout != PdfLayoutType.OnePage)
        {
          if (!string.IsNullOrEmpty(stringLayoutResult1.Remainder) && (double) page.GetClientSize().Height < (double) stringLayoutResult1.ActualSize.Height + (double) stringLayoutResult1.LineHeight)
          {
            rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, width1, this.m_maxHeight);
            pdfLayoutResult1 = (PdfLayoutResult) pdfTextElement.Draw(page, rectangleF2, (PdfLayoutFormat) format);
          }
          else if (!string.IsNullOrEmpty(stringLayoutResult1.Remainder) && (double) this.m_maxHeight < (double) stringLayoutResult1.ActualSize.Height + (double) stringLayoutResult1.LineHeight)
          {
            rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, stringLayoutResult1.ActualSize.Width, stringLayoutResult1.ActualSize.Height);
            pdfLayoutResult1 = (PdfLayoutResult) pdfTextElement.Draw(page, rectangleF2, (PdfLayoutFormat) format);
          }
          else
          {
            pdfTextElement.Text = stringLayoutResult1.Lines[0].Text;
            string text1 = htmltext.minnerText.Remove(0, pdfTextElement.Text.Length);
            string text2 = stringLayoutResult1.Lines[1].Text;
            PdfStringLayoutResult stringLayoutResult2 = pdfStringLayouter.Layout(pdfTextElement.Text, htmltext.mfont, format1, rectangleF2, page.GetClientSize().Height);
            rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, stringLayoutResult2.ActualSize.Width, stringLayoutResult2.ActualSize.Height);
            PdfLayoutResult pdfLayoutResult2 = (PdfLayoutResult) pdfTextElement.Draw(page, rectangleF2, (PdfLayoutFormat) format);
            string str = text1.Remove(0, text1.Length - text2.Length);
            rectangleF2 = !(text2 != str) || !(str == "\n") ? new RectangleF(this.m_initalXvalue, pdfLayoutResult2.Bounds.Bottom, rectangleF1.Width, this.m_maxHeight) : new RectangleF(this.m_initalXvalue, pdfLayoutResult2.Bounds.Y, rectangleF1.Width, this.m_maxHeight);
            PdfStringLayoutResult stringLayoutResult3 = pdfStringLayouter.Layout(text1, htmltext.mfont, format1, rectangleF2, page.GetClientSize().Height);
            pdfTextElement.Text = text1;
            rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, stringLayoutResult3.ActualSize.Width, stringLayoutResult3.ActualSize.Height);
            pdfLayoutResult1 = (PdfLayoutResult) pdfTextElement.Draw(page, rectangleF2, (PdfLayoutFormat) format);
            num3 = stringLayoutResult3.Lines[stringLayoutResult3.LineCount - 1].Width;
          }
        }
        else
          break;
      }
      else
      {
        if ((double) stringLayoutResult1.ActualSize.Width > 0.0 && (double) stringLayoutResult1.ActualSize.Height > 0.0)
          rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, stringLayoutResult1.ActualSize.Width, stringLayoutResult1.ActualSize.Height);
        else if ((double) stringLayoutResult1.ActualSize.Width == 0.0 && (double) stringLayoutResult1.ActualSize.Height > 0.0 && htmltext.minnerText == "\n")
        {
          ref RectangleF local = ref rectangleF2;
          double initalXvalue = (double) this.m_initalXvalue;
          bounds = pdfLayoutResult1.Bounds;
          double bottom = (double) bounds.Bottom;
          sizeF = stringLayoutResult1.ActualSize;
          double width2 = (double) sizeF.Width;
          sizeF = stringLayoutResult1.ActualSize;
          double height = (double) sizeF.Height;
          local = new RectangleF((float) initalXvalue, (float) bottom, (float) width2, (float) height);
        }
        else
        {
          sizeF = stringLayoutResult1.ActualSize;
          if ((double) sizeF.Width == 0.0)
          {
            sizeF = stringLayoutResult1.ActualSize;
            if ((double) sizeF.Height > 0.0 && htmltext.minnerText != "\n")
            {
              ref RectangleF local = ref rectangleF2;
              double initalXvalue = (double) this.m_initalXvalue;
              bounds = pdfLayoutResult1.Bounds;
              double bottom = (double) bounds.Bottom;
              double width3 = (double) width1;
              double height = (double) this.m_maxHeight - (double) num2;
              local = new RectangleF((float) initalXvalue, (float) bottom, (float) width3, (float) height);
            }
          }
        }
        pdfLayoutResult1 = (PdfLayoutResult) pdfTextElement.Draw(page, rectangleF2, (PdfLayoutFormat) format);
      }
      page.Graphics.Restore();
    }
    return pdfLayoutResult1;
  }

  internal void ParseHtml(string text)
  {
    if (!text.StartsWith("<?xml version="))
      text = $"<?xml version=\"1.0\" encoding=\"utf-8\"?> <html>{text}</html>";
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(text);
    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("html");
    this.m_currentFont = this.Font != null ? this.Font : (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 14f);
    foreach (XmlNode parent in elementsByTagName)
    {
      if (parent != null)
        this.ParseElements(parent);
    }
  }

  private void ParseElements(XmlNode parent)
  {
    Htmltext htmltext1 = new Htmltext();
    foreach (XmlNode childNode1 in parent.ChildNodes)
    {
      if (!this.m_fontAttribute)
        this.m_style = this.m_style1 = PdfFontStyle.Regular;
      if (childNode1 is XmlText)
      {
        XmlText xmlText = childNode1 as XmlText;
        this.m_Color = Color.Black;
        bool flag = this.Brush != null;
        this.m_htmllist.Add(new Htmltext()
        {
          mtag = childNode1,
          minnerText = xmlText.InnerText,
          mfont = this.m_currentFont,
          mbrushcolor = this.m_Color,
          mbaseBrushColor = flag
        });
      }
      if (childNode1 is XmlElement)
      {
        XmlElement parent1 = childNode1 as XmlElement;
        this.m_htmlFontHeight = this.m_currentFont.Size;
        this.m_stringFontName = this.Font != null ? (string) null : this.m_currentFont.Name;
        if (!this.m_fontAttribute)
          this.m_Color = Color.Black;
        if (parent1.Name == "p")
        {
          this.ParseElements((XmlNode) parent1);
        }
        else
        {
          if (parent1.Name == "font")
          {
            foreach (XmlAttribute attribute in (XmlNamedNodeMap) parent1.Attributes)
            {
              if (attribute.Name == "color")
              {
                if (attribute.Value[0] != '#')
                {
                  Color color = Color.FromName(attribute.Value);
                  this.m_htmlElementFont = this.GetColorRgb(color);
                  this.m_Color = color;
                }
                else
                {
                  try
                  {
                    attribute.Value = attribute.Value;
                    Color color = Color.FromArgb((int) (byte) int.Parse(attribute.Value.Substring(1, 2), NumberStyles.HexNumber), (int) (byte) int.Parse(attribute.Value.Substring(3, 2), NumberStyles.HexNumber), (int) (byte) int.Parse(attribute.Value.Substring(5, 2), NumberStyles.HexNumber));
                    this.m_htmlElementFont = this.GetColorRgb(color);
                    this.m_Color = color;
                  }
                  catch
                  {
                  }
                }
              }
              else if (attribute.Name == "size")
              {
                this.m_htmlFontElementHeight = float.Parse(attribute.Value);
                this.m_htmlFontHeight = this.m_htmlFontElementHeight;
              }
              else if (attribute.Name == "face")
                this.m_stringFontName = attribute.Value;
            }
          }
          this.Pickstyle(parent1.Name.ToString());
          this.Pickstyle(this.m_stringFontName);
          if (!this.m_fontAttribute)
            this.m_style1 = this.m_style;
          else
            this.m_style1 |= this.m_style;
          Htmltext htmltext2;
          if (parent1.Name == "br")
          {
            htmltext2 = new Htmltext();
            htmltext2.mtag = childNode1;
            htmltext2.minnerText = "\n";
            htmltext2.mfont = (PdfFont) new PdfStandardFont(this.m_fontFace, this.m_htmlFontHeight, this.m_style1);
            htmltext2.mbrushcolor = this.m_Color;
            if (this.m_stringFontName == null && !(this.m_currentFont is PdfStandardFont) && this.m_currentFont is PdfTrueTypeFont)
            {
              htmltext2.mfont = (PdfFont) new PdfTrueTypeFont(this.m_currentFont as PdfTrueTypeFont, this.m_htmlFontHeight);
              htmltext2.mfont.Style = this.m_style1;
            }
          }
          else
          {
            htmltext2 = new Htmltext();
            htmltext2.mtag = childNode1;
            htmltext2.minnerText = parent1.InnerText;
            htmltext2.mfont = (PdfFont) new PdfStandardFont(this.m_fontFace, this.m_htmlFontHeight, this.m_style1);
            htmltext2.mbrushcolor = this.m_Color;
            if (this.m_stringFontName == null && !(this.m_currentFont is PdfStandardFont) && this.m_currentFont is PdfTrueTypeFont)
            {
              htmltext2.mfont = (PdfFont) new PdfTrueTypeFont(this.m_currentFont as PdfTrueTypeFont, this.m_htmlFontHeight);
              htmltext2.mfont.Style = this.m_style1;
            }
          }
          bool flag = false;
          foreach (XmlNode childNode2 in parent1.ChildNodes)
          {
            if (!(childNode2 is XmlText))
            {
              flag = true;
              this.m_fontAttribute = true;
              this.ParseElements((XmlNode) parent1);
              this.m_fontAttribute = false;
              break;
            }
          }
          if (!flag)
            this.m_htmllist.Add(htmltext2);
        }
      }
    }
  }

  private void Pickstyle(string chos)
  {
    switch (chos)
    {
      case "i":
      case "I":
        this.m_style = PdfFontStyle.Italic;
        break;
      case "b":
      case "B":
        this.m_style = PdfFontStyle.Bold;
        break;
      case "u":
      case "U":
        this.m_style = PdfFontStyle.Underline;
        break;
      case "Helvetica":
        this.m_fontFace = PdfFontFamily.Helvetica;
        break;
      case "Courier":
        this.m_fontFace = PdfFontFamily.Courier;
        break;
      case "TimesRoman":
        this.m_fontFace = PdfFontFamily.TimesRoman;
        break;
      case "Symbol":
        this.m_fontFace = PdfFontFamily.Symbol;
        break;
    }
  }

  private int GetColorRgb(Color color)
  {
    return this.GetCOLORREF((int) color.R, (int) color.G, (int) color.B);
  }

  private int GetCOLORREF(int r, int g, int b) => r | g << 8 | b << 16 /*0x10*/;

  private void img_EndPageLayout(object sender, EndPageLayoutEventArgs e)
  {
    if (this.EndPageLayout == null)
      return;
    this.EndPageLayout((object) this, e);
  }

  private void img_BeginPageLayout(object sender, BeginPageLayoutEventArgs e)
  {
    if (this.BeginPageLayout == null)
      return;
    this.BeginPageLayout((object) this, e);
  }

  internal bool RaiseEndPageLayout => this.EndPageLayout != null;

  internal bool RaiseBeginPageLayout => this.BeginPageLayout != null;

  public event EndPageLayoutEventHandler EndPageLayout;

  public event BeginPageLayoutEventHandler BeginPageLayout;

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
