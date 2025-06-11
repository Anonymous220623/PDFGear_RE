// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaStyledField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public abstract class PdfLoadedXfaStyledField : PdfLoadedXfaField
{
  private string m_toolTip;
  private PdfXfaCaption m_caption;
  private PdfFont m_font;
  private PdfColor m_foreColor;
  private PdfXfaBorder m_border;
  private string m_presence;
  private PdfXfaVerticalAlignment m_vAlign;
  private PdfXfaHorizontalAlignment m_hAlign;
  private string hAlign;
  private string vAlign;
  private float m_width;
  private float m_height;
  private PointF m_location;
  private bool m_readOnly;
  private PdfXfaRotateAngle m_rotate;
  private PdfXfaBorder m_completeBorder;
  internal bool m_isDefaultFont;
  internal float lineHeight;

  internal PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  internal PdfXfaBorder CompleteBorder
  {
    get => this.m_completeBorder;
    set => this.m_completeBorder = value;
  }

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public float Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  public PointF Location
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  public PdfFont Font
  {
    internal get => this.m_font;
    set
    {
      if (value == null)
        return;
      this.m_font = value;
    }
  }

  public string ToolTip
  {
    get => this.m_toolTip;
    set
    {
      if (value == null)
        return;
      this.m_toolTip = value;
    }
  }

  public PdfXfaCaption Caption
  {
    get => this.m_caption == null ? (PdfXfaCaption) null : this.m_caption;
    set
    {
      if (value == null)
        return;
      this.m_caption = value;
    }
  }

  public PdfColor ForeColor
  {
    get => this.m_foreColor;
    set => this.m_foreColor = value;
  }

  public PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
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

  internal SizeF GetSize()
  {
    if ((double) this.Height <= 0.0)
    {
      if (this.currentNode.Attributes["maxH"] != null)
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["maxH"].Value);
      if (this.currentNode.Attributes["minH"] != null)
      {
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["minH"].Value);
        if (this.Font != null && (double) this.Font.Height > (double) this.Height)
          this.Height = this.Font.Height + 0.5f;
      }
    }
    if ((double) this.Width <= 0.0)
    {
      if (this.currentNode.Attributes["maxW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["maxW"].Value);
      if (this.currentNode.Attributes["minW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["minW"].Value);
    }
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Height, this.Width) : new SizeF(this.Width, this.Height);
  }

  internal PdfFont GetFont()
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    return this.Font;
  }

  internal void ReadCommonProperties(XmlNode node)
  {
    if (node.Attributes["name"] != null)
      this.Name = node.Attributes["name"].Value;
    if (node.Attributes["x"] != null)
      this.Location = new PointF(this.ConvertToPoint(node.Attributes["x"].Value), this.Location.Y);
    if (node.Attributes["y"] != null)
      this.Location = new PointF(this.Location.X, this.ConvertToPoint(node.Attributes["y"].Value));
    if (node.Attributes["w"] != null)
      this.Width = this.ConvertToPoint(node.Attributes["w"].Value);
    if (node.Attributes["h"] != null)
      this.Height = this.ConvertToPoint(node.Attributes["h"].Value);
    if (node.Attributes["access"] != null && node.Attributes["access"].InnerText == "readOnly")
      this.ReadOnly = true;
    if (node.Attributes["presence"] != null)
    {
      this.m_presence = node.Attributes["presence"].Value;
      switch (this.m_presence.ToLower())
      {
        case "hidden":
          this.Visibility = PdfXfaVisibility.Hidden;
          break;
        case "visible":
          this.Visibility = PdfXfaVisibility.Visible;
          break;
        case "invisible":
          this.Visibility = PdfXfaVisibility.Invisible;
          break;
        case "inactive":
          this.Visibility = PdfXfaVisibility.Inactive;
          break;
        default:
          this.Visibility = PdfXfaVisibility.Visible;
          break;
      }
    }
    if (node.Attributes["rotate"] != null)
    {
      switch (node.Attributes["rotate"].Value)
      {
        case "90":
          this.Rotate = PdfXfaRotateAngle.RotateAngle90;
          break;
        case "180":
          this.Rotate = PdfXfaRotateAngle.RotateAngle180;
          break;
        case "270":
          this.Rotate = PdfXfaRotateAngle.RotateAngle270;
          break;
      }
    }
    if (node["caption"] != null)
      this.ReadCaption((XmlNode) node["caption"]);
    if (node["margin"] != null)
      this.ReadMargin((XmlNode) node["margin"]);
    if (node["assist"] != null && node["assist"]["toolTip"] != null)
      this.ToolTip = node["assist"]["toolTip"].InnerText;
    if (node["border"] != null)
      this.ReadBorder((XmlNode) node["border"], true);
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
      if (node["para"].Attributes["marginLeft"] != null)
        this.Margins.Left += this.ConvertToPoint(node["para"].Attributes["marginLeft"].Value);
      if (node["para"].Attributes["marginRight"] != null)
        this.Margins.Right += this.ConvertToPoint(node["para"].Attributes["marginRight"].Value);
      if (node["para"].Attributes["marginTop"] != null)
        this.Margins.Top += this.ConvertToPoint(node["para"].Attributes["marginTop"].Value);
      if (node["para"].Attributes["marginBottom"] != null)
        this.Margins.Bottom += this.ConvertToPoint(node["para"].Attributes["marginBottom"].Value);
      if (node["para"].Attributes["lineHeight"] != null)
        this.lineHeight = this.ConvertToPoint(node["para"].Attributes["lineHeight"].Value);
    }
    if (node["font"] == null)
      return;
    this.ReadFontInfo((XmlNode) node["font"]);
  }

  internal void ReadBorder(XmlNode node, bool complete)
  {
    if (complete)
    {
      this.CompleteBorder = new PdfXfaBorder();
      this.CompleteBorder.Read(node);
    }
    else
    {
      this.m_border = new PdfXfaBorder();
      this.m_border.Read(node);
    }
  }

  private void ReadCaption(XmlNode node)
  {
    this.m_caption = new PdfXfaCaption(true);
    this.m_caption.Read(node);
  }

  private void ReadFontInfo(XmlNode fNode)
  {
    string empty = string.Empty;
    float num = (double) this.lineHeight > 0.0 ? this.lineHeight : 10f;
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
    if (empty == "Times New Roman" || empty == "Helvetica" || empty == "Courier" || empty == "Symbol" || empty == "ZapfDingbats")
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
    else if (empty != "" && (double) num != 0.0)
      this.Font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(empty, num, (FontStyle) style), num, true);
    if (this.Font == null || !(this.Font.Name != empty))
      return;
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num, style);
    this.m_isDefaultFont = true;
  }

  internal void CheckUnicodeFont(string text)
  {
    if (text == null || !PdfString.IsUnicode(text) || this.Font == null || !(this.Font is PdfStandardFont))
      return;
    this.Font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(this.Font.Name, this.Font.Size, (FontStyle) this.Font.Style), this.Font.Size, true);
  }

  internal void Save()
  {
    if (this.ReadOnly)
    {
      if (this.currentNode.Attributes["access"] != null)
      {
        this.currentNode.Attributes["access"].Value = "readOnly";
      }
      else
      {
        XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("access");
        attribute.InnerText = "readOnly";
        this.currentNode.Attributes.Append(attribute);
      }
    }
    if ((double) this.Width > 0.0)
      this.SetSize(this.currentNode, "w", this.Width);
    if ((double) this.Height > 0.0)
      this.SetSize(this.currentNode, "h", this.Height);
    if (this.Caption != null)
    {
      if (this.currentNode["caption"] != null)
      {
        this.Caption.Save((XmlNode) this.currentNode["caption"]);
      }
      else
      {
        XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "caption", "");
        this.Caption.Save(node);
        this.currentNode.AppendChild(node);
      }
    }
    if ((this.m_presence != null || this.m_presence != "") && this.Visibility != PdfXfaVisibility.Visible && this.m_presence != this.Visibility.ToString().ToLower())
    {
      if (this.currentNode.Attributes["presence"] != null)
        this.currentNode.Attributes["presence"].Value = this.Visibility.ToString().ToLower();
      else
        this.SetNewAttribute(this.currentNode, "presence", this.Visibility.ToString().ToLower());
    }
    if (this.Font != null)
      this.SetFont(this.currentNode);
    this.SetMargin(this.currentNode);
    if (this.ToolTip != null && this.ToolTip != "")
    {
      if (this.currentNode["assist"] != null)
      {
        if (this.currentNode["assist"]["toolTip"] != null)
        {
          this.currentNode["assist"]["toolTip"].InnerText = this.ToolTip;
        }
        else
        {
          XmlNode node = this.currentNode["assist"].OwnerDocument.CreateNode(XmlNodeType.Element, "toolTip", "");
          node.InnerText = this.ToolTip;
          this.currentNode["assist"].AppendChild(node);
        }
      }
      else
      {
        XmlNode node1 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "assist", "");
        XmlNode node2 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, "toolTip", "");
        node2.InnerText = this.ToolTip;
        node1.AppendChild(node2);
        this.currentNode.AppendChild(node1);
      }
    }
    if (this.hAlign != null && this.hAlign != this.HorizontalAlignment.ToString().ToLower())
    {
      string str = this.HorizontalAlignment.ToString().ToLower();
      if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.JustifyAll)
        str = "justifyAll";
      if (this.currentNode["para"] != null)
      {
        if (this.currentNode["para"].Attributes["hAlign"] != null)
          this.currentNode["para"].Attributes["hAlign"].Value = str;
        else
          this.SetNewAttribute((XmlNode) this.currentNode["para"], "hAlign", str);
      }
      else
      {
        XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "para", "");
        this.SetNewAttribute(node, "hAlign", str);
        this.currentNode.AppendChild(node);
      }
    }
    if (this.vAlign == null || !(this.VerticalAlignment.ToString().ToLower() != this.vAlign))
      return;
    if (this.currentNode["para"] != null)
    {
      if (this.currentNode["para"].Attributes["vAlign"] != null)
        this.currentNode["para"].Attributes["vAlign"].Value = this.VerticalAlignment.ToString().ToLower();
      else
        this.SetNewAttribute((XmlNode) this.currentNode["para"], "vAlign", this.VerticalAlignment.ToString().ToLower());
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "para", "");
      this.SetNewAttribute(node, "vAlign", this.VerticalAlignment.ToString().ToLower());
      this.currentNode.AppendChild(node);
    }
  }

  private void SetMargin(XmlNode node)
  {
    if ((double) this.Margins.Bottom == 0.0 && (double) this.Margins.Top == 0.0 && (double) this.Margins.Left == 0.0 && (double) this.Margins.Right == 0.0)
      return;
    if (node["margin"] != null)
    {
      XmlAttributeCollection attributes = node["margin"].Attributes;
      if (attributes["leftInset"] != null)
        attributes["leftInset"].Value = this.Margins.Left.ToString() + "pt";
      else
        this.SetNewAttribute((XmlNode) node["margin"], "leftInset", this.Margins.Left.ToString() + "pt");
      if (attributes["rightInset"] != null)
        attributes["rightInset"].Value = this.Margins.Right.ToString() + "pt";
      else
        this.SetNewAttribute((XmlNode) node["margin"], "rightInset", this.Margins.Right.ToString() + "pt");
      if (attributes["bottomInset"] != null)
        attributes["bottomInset"].Value = this.Margins.Bottom.ToString() + "pt";
      else
        this.SetNewAttribute((XmlNode) node["margin"], "bottomInset", this.Margins.Bottom.ToString() + "pt");
      if (attributes["topInset"] != null)
        attributes["topInset"].Value = this.Margins.Top.ToString() + "pt";
      else
        this.SetNewAttribute((XmlNode) node["margin"], "topInset", this.Margins.Top.ToString() + "pt");
    }
    else
    {
      XmlNode node1 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "margin", "");
      this.SetNewAttribute(node1, "leftInset", this.Margins.Left.ToString() + "pt");
      this.SetNewAttribute(node1, "rightInset", this.Margins.Right.ToString() + "pt");
      this.SetNewAttribute(node1, "bottomInset", this.Margins.Bottom.ToString() + "pt");
      this.SetNewAttribute(node1, "topInset", this.Margins.Top.ToString() + "pt");
    }
  }

  internal PdfCheckBoxStyle GetStyle(PdfXfaCheckedStyle style)
  {
    PdfCheckBoxStyle style1 = PdfCheckBoxStyle.Check;
    switch (style)
    {
      case PdfXfaCheckedStyle.Check:
        style1 = PdfCheckBoxStyle.Check;
        break;
      case PdfXfaCheckedStyle.Circle:
        style1 = PdfCheckBoxStyle.Circle;
        break;
      case PdfXfaCheckedStyle.Cross:
        style1 = PdfCheckBoxStyle.Cross;
        break;
      case PdfXfaCheckedStyle.Diamond:
        style1 = PdfCheckBoxStyle.Diamond;
        break;
      case PdfXfaCheckedStyle.Square:
        style1 = PdfCheckBoxStyle.Square;
        break;
      case PdfXfaCheckedStyle.Star:
        style1 = PdfCheckBoxStyle.Star;
        break;
    }
    return style1;
  }

  internal string StyleToString(PdfCheckBoxStyle style)
  {
    switch (style)
    {
      case PdfCheckBoxStyle.Circle:
        return "l";
      case PdfCheckBoxStyle.Cross:
        return "8";
      case PdfCheckBoxStyle.Diamond:
        return "u";
      case PdfCheckBoxStyle.Square:
        return "n";
      case PdfCheckBoxStyle.Star:
        return "H";
      default:
        return "4";
    }
  }

  private void SetFont(XmlNode node)
  {
    if (this.Font == null || node["font"] == null)
      return;
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
    if (this.ForeColor.R == (byte) 0 && this.ForeColor.G == (byte) 0 && this.ForeColor.B == (byte) 0)
      return;
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

  internal void SetNewAttribute(XmlNode node, string name, string value)
  {
    XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }

  internal int GetRotationAngle()
  {
    int rotationAngle = 0;
    if (this.Rotate != PdfXfaRotateAngle.RotateAngle0)
    {
      switch (this.Rotate)
      {
        case PdfXfaRotateAngle.RotateAngle90:
          rotationAngle = 90;
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          rotationAngle = 180;
          break;
        case PdfXfaRotateAngle.RotateAngle270:
          rotationAngle = 270;
          break;
      }
    }
    return rotationAngle;
  }

  internal RectangleF GetRenderingRect(RectangleF tempBounds)
  {
    RectangleF renderingRect = new RectangleF();
    switch (this.GetRotationAngle())
    {
      case 0:
        renderingRect = new RectangleF(0.0f, 0.0f, tempBounds.Width, tempBounds.Height);
        break;
      case 90:
        renderingRect = new RectangleF(-tempBounds.Height, 0.0f, tempBounds.Height, tempBounds.Width);
        break;
      case 180:
        renderingRect = new RectangleF(-tempBounds.Width, -tempBounds.Height, tempBounds.Width, tempBounds.Height);
        break;
      case 270:
        renderingRect = new RectangleF(0.0f, -tempBounds.Width, tempBounds.Height, tempBounds.Width);
        break;
    }
    return renderingRect;
  }
}
