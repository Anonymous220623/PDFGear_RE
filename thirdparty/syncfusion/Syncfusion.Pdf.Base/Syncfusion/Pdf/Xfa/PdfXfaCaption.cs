// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaCaption
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaCaption
{
  private string m_text;
  internal float m_width;
  private PdfFont m_font;
  private PdfXfaHorizontalAlignment m_hAlign;
  private PdfXfaVerticalAlignment m_vAlign = PdfXfaVerticalAlignment.Middle;
  private PdfXfaPosition m_position;
  private PdfColor m_foreColor;
  internal XmlNode currentNode;
  private string hAlign;
  private string vAlign;
  private RectangleF m_bounds = RectangleF.Empty;
  private PdfPage m_page;
  internal PdfXfaField parent;

  public string Text
  {
    get => this.m_text;
    set
    {
      if (value == null)
        return;
      this.m_text = value;
    }
  }

  public PdfFont Font
  {
    get => this.m_font;
    set
    {
      if (value == null)
        return;
      this.m_font = value;
    }
  }

  public PdfXfaHorizontalAlignment HorizontalAlignment
  {
    get => this.m_hAlign;
    set => this.m_hAlign = value;
  }

  public PdfXfaVerticalAlignment VerticalAlignment
  {
    get => this.m_vAlign;
    set => this.m_vAlign = value;
  }

  public PdfXfaPosition Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public PdfColor ForeColor
  {
    get => this.m_foreColor;
    set => this.m_foreColor = value;
  }

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public PdfXfaCaption()
  {
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
  }

  internal PdfXfaCaption(bool flag)
  {
  }

  internal void Save(XfaWriter xfaWriter)
  {
    xfaWriter.Write.WriteStartElement("caption");
    xfaWriter.Write.WriteAttributeString("placement", this.Position.ToString().ToLower());
    if ((double) this.m_width > 0.0)
      xfaWriter.Write.WriteAttributeString("reserve", this.m_width.ToString() + "pt");
    xfaWriter.WriteValue(this.Text, 0);
    xfaWriter.WritePragraph(this.VerticalAlignment, this.HorizontalAlignment);
    xfaWriter.WriteFontInfo(this.Font, this.ForeColor);
    xfaWriter.Write.WriteEndElement();
  }

  internal void DrawText(PdfPageBase page, RectangleF bounds)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    PdfBrush brush = PdfBrushes.Black;
    if (this.ForeColor != PdfColor.Empty && ((double) this.ForeColor.Red != 0.0 || (double) this.ForeColor.Green != 0.0 || (double) this.ForeColor.Blue != 0.0))
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if ((double) this.m_width <= 0.0 || this.Text == null)
      return;
    if (this.Position == PdfXfaPosition.Top)
      page.Graphics.DrawString(this.Text, this.Font, brush, new RectangleF(bounds.Location, new SizeF(bounds.Width, this.m_width)), format);
    else if (this.Position == PdfXfaPosition.Bottom)
      page.Graphics.DrawString(this.Text, this.Font, brush, new RectangleF(new PointF(bounds.Location.X, bounds.Y + (bounds.Height - this.m_width)), new SizeF(bounds.Width, this.m_width)), format);
    else if (this.Position == PdfXfaPosition.Left)
      page.Graphics.DrawString(this.Text, this.Font, brush, new RectangleF(bounds.Location, new SizeF(this.m_width, bounds.Height)), format);
    else
      page.Graphics.DrawString(this.Text, this.Font, brush, new RectangleF(new PointF(bounds.Location.X + (bounds.Width - this.m_width), bounds.Location.Y), new SizeF(this.m_width, bounds.Height)), format);
  }

  internal void DrawText(PdfGraphics graphics, RectangleF bounds)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    PdfBrush brush = PdfBrushes.Black;
    if (this.ForeColor != PdfColor.Empty && ((double) this.ForeColor.Red != 0.0 || (double) this.ForeColor.Green != 0.0 || (double) this.ForeColor.Blue != 0.0))
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if ((double) this.m_width <= 0.0 || this.Text == null)
      return;
    if (this.Position == PdfXfaPosition.Top)
      graphics.DrawString(this.Text, this.Font, brush, new RectangleF(bounds.Location, new SizeF(bounds.Width, this.m_width)), format);
    else if (this.Position == PdfXfaPosition.Bottom)
      graphics.DrawString(this.Text, this.Font, brush, new RectangleF(new PointF(bounds.Location.X, bounds.Y + (bounds.Height - this.m_width)), new SizeF(bounds.Width, this.m_width)), format);
    else if (this.Position == PdfXfaPosition.Left)
      graphics.DrawString(this.Text, this.Font, brush, new RectangleF(bounds.Location, new SizeF(this.m_width, bounds.Height)), format);
    else
      graphics.DrawString(this.Text, this.Font, brush, new RectangleF(new PointF(bounds.Location.X + (bounds.Width - this.m_width), bounds.Location.Y), new SizeF(this.m_width, bounds.Height)), format);
  }

  internal void DrawText(PdfGraphics graphics, RectangleF bounds, int rotationAngle)
  {
    PdfStringFormat format = new PdfStringFormat(this.ConvertToPdfTextAlignment(this.HorizontalAlignment), (PdfVerticalAlignment) this.VerticalAlignment);
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    PdfBrush brush = PdfBrushes.Black;
    if (this.ForeColor != PdfColor.Empty && ((double) this.ForeColor.Red != 0.0 || (double) this.ForeColor.Green != 0.0 || (double) this.ForeColor.Blue != 0.0))
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    RectangleF rectangleF = RectangleF.Empty;
    if (this.Text == null || !(this.Text != string.Empty))
      return;
    if (PdfString.IsUnicode(this.Text) && this.Font is PdfTrueTypeFont)
      this.Text = PdfGraphics.NormalizeText(this.Font, this.Text);
    SizeF sizeF = SizeF.Empty;
    float num = 0.0f;
    bool flag = false;
    if (this.Font != null)
      sizeF = this.Font.MeasureString(this.Text);
    if ((this.Position == PdfXfaPosition.Top || this.Position == PdfXfaPosition.Bottom) && (double) this.m_width < (double) this.Font.Height)
    {
      num = this.m_width;
      if ((double) num > 0.0)
        flag = true;
      this.m_width = this.Font.Height;
    }
    switch (this.Position)
    {
      case PdfXfaPosition.Left:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(bounds.Location, new SizeF(this.m_width, bounds.Height));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), this.m_width, bounds.Width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y, this.m_width, bounds.Height);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Width);
            break;
        }
        break;
      case PdfXfaPosition.Right:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(new PointF(bounds.Location.X + (bounds.Width - this.m_width), bounds.Location.Y), new SizeF(this.m_width, bounds.Height));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Height);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), this.m_width, bounds.Width);
            break;
        }
        break;
      case PdfXfaPosition.Top:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(bounds.Location, new SizeF(bounds.Width, this.m_width));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), bounds.Height, this.m_width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - sizeF.Width), bounds.Y + (bounds.Height - this.m_width), bounds.Width, this.m_width);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y, bounds.Height, this.m_width);
            break;
        }
        break;
      case PdfXfaPosition.Bottom:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(new PointF(bounds.Location.X, bounds.Y + (bounds.Height - this.m_width)), new SizeF(bounds.Width, this.m_width));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y + (bounds.Height - this.m_width), bounds.Height, this.m_width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - sizeF.Width), bounds.Y, bounds.Width, this.m_width);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y, bounds.Height, this.m_width);
            break;
        }
        break;
    }
    PdfGraphics pdfGraphics = graphics;
    pdfGraphics.Save();
    pdfGraphics.TranslateTransform(rectangleF.X, rectangleF.Y);
    if (rotationAngle != 0)
      pdfGraphics.RotateTransform((float) -rotationAngle);
    RectangleF layoutRectangle = RectangleF.Empty;
    switch (rotationAngle)
    {
      case 0:
        layoutRectangle = new RectangleF(0.0f, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 90:
        layoutRectangle = new RectangleF(-this.m_width, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 180:
        layoutRectangle = new RectangleF(-rectangleF.Width, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
      case 270:
        layoutRectangle = new RectangleF(0.0f, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
    }
    pdfGraphics.DrawString(this.Text, this.Font, brush, layoutRectangle, format);
    pdfGraphics.Restore();
    if (!flag)
      return;
    this.m_width = num;
  }

  internal void DrawText(PdfPageBase page, RectangleF bounds, int rotationAngle)
  {
    PdfStringFormat format = new PdfStringFormat(this.ConvertToPdfTextAlignment(this.HorizontalAlignment), (PdfVerticalAlignment) this.VerticalAlignment);
    PdfBrush brush = PdfBrushes.Black;
    if (this.ForeColor != PdfColor.Empty && ((double) this.ForeColor.Red != 0.0 || (double) this.ForeColor.Green != 0.0 || (double) this.ForeColor.Blue != 0.0))
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    RectangleF rectangleF = RectangleF.Empty;
    if ((double) this.m_width <= 0.0 || this.Text == null || !(this.Text != string.Empty))
      return;
    SizeF sizeF = SizeF.Empty;
    if (this.Font != null)
      sizeF = this.Font.MeasureString(this.Text);
    switch (this.Position)
    {
      case PdfXfaPosition.Left:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(bounds.Location, new SizeF(this.m_width, bounds.Height));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), this.m_width, bounds.Width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y, this.m_width, bounds.Height);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Width);
            break;
        }
        break;
      case PdfXfaPosition.Right:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(new PointF(bounds.Location.X + (bounds.Width - this.m_width), bounds.Location.Y), new SizeF(this.m_width, bounds.Height));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X, bounds.Y, this.m_width, bounds.Height);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), this.m_width, bounds.Width);
            break;
        }
        break;
      case PdfXfaPosition.Top:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(bounds.Location, new SizeF(bounds.Width, this.m_width));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X, bounds.Y + (bounds.Height - this.m_width), bounds.Height, this.m_width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - sizeF.Width), bounds.Y + (bounds.Height - this.m_width), bounds.Width, this.m_width);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y, bounds.Height, this.m_width);
            break;
        }
        break;
      case PdfXfaPosition.Bottom:
        switch (rotationAngle)
        {
          case 0:
            rectangleF = new RectangleF(new PointF(bounds.Location.X, bounds.Y + (bounds.Height - this.m_width)), new SizeF(bounds.Width, this.m_width));
            break;
          case 90:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - this.m_width), bounds.Y + (bounds.Height - this.m_width), bounds.Height, this.m_width);
            break;
          case 180:
            rectangleF = new RectangleF(bounds.X + (bounds.Width - sizeF.Width), bounds.Y, bounds.Width, this.m_width);
            break;
          case 270:
            rectangleF = new RectangleF(bounds.X, bounds.Y, bounds.Height, this.m_width);
            break;
        }
        break;
    }
    PdfGraphics graphics = page.Graphics;
    graphics.Save();
    graphics.TranslateTransform(rectangleF.X, rectangleF.Y);
    if (rotationAngle != 0)
      graphics.RotateTransform((float) -rotationAngle);
    RectangleF layoutRectangle = RectangleF.Empty;
    switch (rotationAngle)
    {
      case 0:
        layoutRectangle = new RectangleF(0.0f, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 90:
        layoutRectangle = new RectangleF(-this.m_width, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 180:
        layoutRectangle = new RectangleF(-sizeF.Width, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
      case 270:
        layoutRectangle = new RectangleF(0.0f, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
    }
    graphics.DrawString(this.Text, this.Font, brush, layoutRectangle, format);
    graphics.Restore();
  }

  private PdfTextAlignment ConvertToPdfTextAlignment(PdfXfaHorizontalAlignment align)
  {
    PdfTextAlignment pdfTextAlignment = PdfTextAlignment.Center;
    switch (align)
    {
      case PdfXfaHorizontalAlignment.Left:
        pdfTextAlignment = PdfTextAlignment.Left;
        break;
      case PdfXfaHorizontalAlignment.Center:
        pdfTextAlignment = PdfTextAlignment.Center;
        break;
      case PdfXfaHorizontalAlignment.Right:
        pdfTextAlignment = PdfTextAlignment.Right;
        break;
      case PdfXfaHorizontalAlignment.Justify:
      case PdfXfaHorizontalAlignment.JustifyAll:
        pdfTextAlignment = PdfTextAlignment.Justify;
        break;
    }
    return pdfTextAlignment;
  }

  internal SizeF MeasureString() => this.MeasureString(this.Text);

  internal SizeF MeasureString(string text)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 12f, PdfFontStyle.Regular);
    return this.Text != null ? this.Font.MeasureString(text) : SizeF.Empty;
  }

  public object Clone() => this.MemberwiseClone();

  internal void Read(XmlNode node)
  {
    this.VerticalAlignment = PdfXfaVerticalAlignment.Top;
    this.currentNode = node;
    if (!(node.Name == "caption"))
      return;
    if (node.Attributes["placement"] != null)
    {
      switch (node.Attributes["placement"].Value)
      {
        case "left":
          this.Position = PdfXfaPosition.Left;
          break;
        case "right":
          this.Position = PdfXfaPosition.Right;
          break;
        case "top":
          this.Position = PdfXfaPosition.Top;
          break;
        case "bottom":
          this.Position = PdfXfaPosition.Bottom;
          break;
      }
    }
    if (node.Attributes["reserve"] != null)
      this.Width = this.ConvertToPoint(node.Attributes["reserve"].Value);
    if (node["font"] != null)
      this.ReadFontInfo((XmlNode) node["font"]);
    if (node["para"] != null)
    {
      if (node["para"].Attributes["hAlign"] != null)
      {
        this.hAlign = node["para"].Attributes["hAlign"].Value;
        switch (this.hAlign)
        {
          case "left":
            this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
            break;
          case "right":
            this.HorizontalAlignment = PdfXfaHorizontalAlignment.Right;
            break;
          case "center":
            this.HorizontalAlignment = PdfXfaHorizontalAlignment.Center;
            break;
          case "justify":
            this.HorizontalAlignment = PdfXfaHorizontalAlignment.Justify;
            break;
          case "justifyAll":
            this.HorizontalAlignment = PdfXfaHorizontalAlignment.JustifyAll;
            break;
        }
      }
      if (node["para"].Attributes["vAlign"] != null)
      {
        this.vAlign = node["para"].Attributes["vAlign"].Value;
        switch (this.vAlign)
        {
          case "bottom":
            this.VerticalAlignment = PdfXfaVerticalAlignment.Bottom;
            break;
          case "middle":
            this.VerticalAlignment = PdfXfaVerticalAlignment.Middle;
            break;
          case "top":
            this.VerticalAlignment = PdfXfaVerticalAlignment.Top;
            break;
        }
      }
    }
    if (node["value"] == null)
      return;
    if (node["value"]["text"] != null)
    {
      this.Text = node["value"]["text"].InnerText;
    }
    else
    {
      if (node["value"]["exData"] == null)
        return;
      this.Text = node["value"]["exData"].InnerText;
    }
  }

  internal void Save(XmlNode node)
  {
    if (this.Position != PdfXfaPosition.Left)
    {
      if (node.Attributes["placement"] != null)
        node.Attributes["placement"].Value = this.Position.ToString().ToLower();
      else
        this.SetNewAttribute(node, "placement", this.Position.ToString().ToLower());
    }
    if ((double) this.Width > 0.0)
    {
      if (node.Attributes["reserve"] != null)
        node.Attributes["reserve"].Value = this.Width.ToString() + "pt";
      else
        this.SetNewAttribute(node, "reserve", this.Width.ToString() + "pt");
    }
    if (this.Font != null && node["font"] != null)
    {
      XmlNode node1 = (XmlNode) node["font"];
      if (node1.Attributes["typeface"] != null)
        node1.Attributes["typeface"].Value = this.Font.Name;
      else
        this.SetNewAttribute(node1, "typeface", this.Font.Name);
      if ((double) this.Font.Size > 0.0 && (double) this.Font.Size != 0.10000000149011612)
      {
        if (node1.Attributes["size"] != null)
          node1.Attributes["size"].Value = this.Font.Size.ToString() + "pt";
        else
          this.SetNewAttribute(node1, "size", this.Font.Size.ToString() + "pt");
      }
      switch (this.Font.Style)
      {
        case PdfFontStyle.Bold:
          if (node1.Attributes["weight"] != null)
          {
            node1.Attributes["weight"].Value = this.Font.Style.ToString().ToLower();
            break;
          }
          this.SetNewAttribute(node1, "weight", this.Font.Style.ToString().ToLower());
          break;
        case PdfFontStyle.Italic:
          if (node1.Attributes["posture"] != null)
          {
            node1.Attributes["posture"].Value = this.Font.Style.ToString().ToLower();
            break;
          }
          this.SetNewAttribute(node1, "posture", this.Font.Style.ToString().ToLower());
          break;
        case PdfFontStyle.Underline:
          if (node1.Attributes["underline"] != null)
          {
            node1.Attributes["underline"].Value = "1";
            break;
          }
          this.SetNewAttribute(node1, "underline", "1");
          break;
        case PdfFontStyle.Strikeout:
          if (node1.Attributes["linethrough"] != null)
          {
            node1.Attributes["linethrough"].Value = "1";
            break;
          }
          this.SetNewAttribute(node1, "linethrough", "1");
          break;
      }
      PdfColor foreColor = this.ForeColor;
      if (this.ForeColor.R != (byte) 0 || this.ForeColor.G != (byte) 0 || this.ForeColor.B != (byte) 0)
      {
        string str = $"{this.ForeColor.R.ToString()},{this.ForeColor.G.ToString()},{this.ForeColor.B.ToString()}";
        if (node1["fill"] != null)
        {
          if (node1["fill"]["color"] != null)
          {
            if (node1["fill"]["color"].Attributes["value"] != null)
              node1["fill"]["color"].Attributes["value"].Value = str;
            else
              this.SetNewAttribute((XmlNode) node1["fill"]["color"], "value", str);
          }
          else
          {
            XmlNode node2 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, "color", "");
            this.SetNewAttribute(node2, "value", str);
            node1["fill"].AppendChild(node2);
          }
        }
        else
        {
          XmlNode node3 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, "fill", "");
          XmlNode node4 = node3.OwnerDocument.CreateNode(XmlNodeType.Element, "color", "");
          this.SetNewAttribute(node4, "value", str);
          node3.AppendChild(node4);
          node1.AppendChild(node3);
        }
      }
    }
    if (this.hAlign != null && this.hAlign != this.HorizontalAlignment.ToString().ToLower())
    {
      string str = this.HorizontalAlignment.ToString().ToLower();
      if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.JustifyAll)
        str = "justifyAll";
      if (node["para"] != null)
      {
        if (node["para"].Attributes["hAlign"] != null)
          node["para"].Attributes["hAlign"].Value = str;
        else
          this.SetNewAttribute((XmlNode) node["para"], "hAlign", str);
      }
      else
      {
        XmlNode node5 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "para", "");
        this.SetNewAttribute(node5, "hAlign", str);
        node.AppendChild(node5);
      }
    }
    if (this.vAlign != null && this.VerticalAlignment.ToString().ToLower() != this.vAlign)
    {
      if (node["para"] != null)
      {
        if (node["para"].Attributes["vAlign"] != null)
          node["para"].Attributes["vAlign"].Value = this.VerticalAlignment.ToString().ToLower();
        else
          this.SetNewAttribute((XmlNode) node["para"], "vAlign", this.VerticalAlignment.ToString().ToLower());
      }
      else
      {
        XmlNode node6 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "para", "");
        this.SetNewAttribute(node6, "vAlign", this.VerticalAlignment.ToString().ToLower());
        node.AppendChild(node6);
      }
    }
    if (this.Text == null || !(this.Text != ""))
      return;
    if (node["value"] != null)
    {
      if (node["value"]["text"] != null)
        node["value"]["text"].InnerText = this.Text;
      else if (node["value"]["exData"] != null)
      {
        if (!(node["value"]["exData"].InnerText != this.Text))
          return;
        node["value"]["exData"].InnerText = this.Text;
      }
      else
      {
        XmlNode node7 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
        node7.InnerText = this.Text;
        node["value"].AppendChild(node7);
      }
    }
    else
    {
      XmlNode node8 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "value", "");
      XmlNode node9 = node8.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
      node9.InnerText = this.Text;
      node8.AppendChild(node9);
      node.AppendChild(node8);
    }
  }

  private void SetNewAttribute(XmlNode node, string name, string value)
  {
    XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }

  private float ConvertToPoint(string value)
  {
    float point = 0.0f;
    if (value.Contains("pt"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.Contains("m"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture) * 2.83464575f;
    else if (value.Contains("in"))
      point = Convert.ToSingle(value.Trim('i', 'n'), (IFormatProvider) CultureInfo.InvariantCulture) * 72f;
    return point;
  }

  private void ReadFontInfo(XmlNode fNode)
  {
    string empty = string.Empty;
    float num = 10f;
    PdfFontStyle style = PdfFontStyle.Regular;
    if (fNode.Attributes["typeface"] != null)
      empty = fNode.Attributes["typeface"].Value;
    if (fNode.Attributes["size"] != null)
      num = this.ConvertToPoint(fNode.Attributes["size"].Value);
    if (fNode.Attributes["weight"] != null)
      style = PdfFontStyle.Bold;
    else if (fNode.Attributes["posture"] != null)
      style = PdfFontStyle.Italic;
    else if (fNode.Attributes["linethrough"] != null)
      style = PdfFontStyle.Strikeout;
    else if (fNode.Attributes["underline"] != null)
      style = PdfFontStyle.Underline;
    if (fNode["fill"] != null)
    {
      XmlNode xmlNode = (XmlNode) fNode["fill"]["color"];
      if (xmlNode != null && xmlNode.Attributes["value"] != null)
      {
        string[] strArray = xmlNode.Attributes["value"].Value.Split(',');
        this.ForeColor = new PdfColor(byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2]));
      }
    }
    if (empty == "Times New Roman" || empty == "TimesRoman" || empty == "Helvetica" || empty == "Courier" || empty == "Symbol" || empty == "ZapfDingbats")
    {
      switch (empty)
      {
        case "Times New Roman":
        case "TimesRoman":
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num, style);
          break;
        case "Helvetica":
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, num, style);
          break;
        case "Courier":
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, num, style);
          break;
        case "Symbol":
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, num, style);
          break;
        case "ZapfDingbats":
          this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, num, style);
          break;
      }
    }
    else if (empty != "")
      this.Font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(empty, num, (FontStyle) style));
    if (this.Font == null || !(this.Font.Name != empty))
      return;
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num, style);
  }
}
