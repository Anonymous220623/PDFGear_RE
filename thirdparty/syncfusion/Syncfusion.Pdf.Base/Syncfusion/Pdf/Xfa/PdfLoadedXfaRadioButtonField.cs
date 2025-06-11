// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaRadioButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaRadioButtonField : PdfLoadedXfaStyledField
{
  internal bool m_isChecked;
  internal string vText;
  internal string iText;
  private PdfXfaCheckedStyle m_checkedStyle;
  private PdfXfaCheckBoxAppearance m_radioButtonAppearance = PdfXfaCheckBoxAppearance.Round;
  private float m_radioButtonSize;

  public bool IsChecked
  {
    get => this.m_isChecked;
    set
    {
      if (value && this.parent is PdfLoadedXfaRadioButtonGroup)
        (this.parent as PdfLoadedXfaRadioButtonGroup).ResetSelection();
      this.m_isChecked = value;
    }
  }

  public float RadioButtonSize
  {
    get => this.m_radioButtonSize;
    set => this.m_radioButtonSize = value;
  }

  public PdfXfaCheckedStyle CheckedStyle
  {
    set => this.m_checkedStyle = value;
    get => this.m_checkedStyle;
  }

  public PdfXfaCheckBoxAppearance RadioButtonAppearance
  {
    set => this.m_radioButtonAppearance = value;
    get => this.m_radioButtonAppearance;
  }

  internal void ReadField(XmlNode node)
  {
    this.currentNode = node;
    if (!(node.Name == "field"))
      return;
    this.ReadCommonProperties(this.currentNode);
    if (node["ui"]["checkButton"] != null)
    {
      XmlAttributeCollection attributes = node["ui"]["checkButton"].Attributes;
      if (attributes["shape"] != null)
        this.RadioButtonAppearance = attributes["shape"].Value == "square" ? PdfXfaCheckBoxAppearance.Square : PdfXfaCheckBoxAppearance.Round;
      if (attributes["mark"] != null)
      {
        switch (attributes["mark"].Value)
        {
          case "check":
            this.CheckedStyle = PdfXfaCheckedStyle.Check;
            break;
          case "cross":
            this.CheckedStyle = PdfXfaCheckedStyle.Cross;
            break;
          case "circle":
            this.CheckedStyle = PdfXfaCheckedStyle.Circle;
            break;
          case "diamond":
            this.CheckedStyle = PdfXfaCheckedStyle.Diamond;
            break;
          case "square":
            this.CheckedStyle = PdfXfaCheckedStyle.Square;
            break;
          case "star":
            this.CheckedStyle = PdfXfaCheckedStyle.Star;
            break;
          case "default":
            this.CheckedStyle = PdfXfaCheckedStyle.Default;
            break;
        }
      }
      if (attributes["size"] != null)
        this.RadioButtonSize = this.ConvertToPoint(attributes["size"].Value);
      if (node["ui"]["checkButton"]["border"] != null)
        this.ReadBorder((XmlNode) node["ui"]["checkButton"]["border"], false);
    }
    this.nodeName = $"{this.parent.nodeName}.{this.Name}";
    if (node["value"] != null)
    {
      if (node["value"]["text"] != null)
        this.vText = node["value"]["text"].InnerText;
      if (node["value"]["items"] != null)
      {
        if (node["value"]["items"]["text"] != null)
          this.iText = node["value"]["items"]["text"].InnerText;
        else if (node["value"]["items"]["integer"] != null)
          this.iText = node["value"]["items"]["integer"].InnerText;
      }
    }
    if (node["items"] == null)
      return;
    if (node["items"]["text"] != null)
    {
      this.iText = node["items"]["text"].InnerText;
    }
    else
    {
      if (node["items"]["integer"] == null)
        return;
      this.iText = node["items"]["integer"].InnerText;
    }
  }

  internal void DrawRadioButton(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    SizeF size = this.GetSize();
    PdfStringFormat pdfStringFormat = new PdfStringFormat()
    {
      LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment,
      Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment)
    };
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    bool flag = false;
    if (this.Border == null && this.parent is PdfLoadedXfaForm && (this.parent as PdfLoadedXfaForm).FlowDirection == PdfLoadedXfaFlowDirection.Row)
      flag = true;
    if (this.CompleteBorder != null && !flag)
      this.CompleteBorder.DrawBorder(graphics, bounds1);
    if (this.Visibility != PdfXfaVisibility.Invisible && this.Caption != null)
    {
      if (this.Caption.Font == null)
        this.Caption.Font = this.Font;
      if (this.Caption.Font != null && (double) bounds1.Height < (double) this.Caption.Font.Height && (double) bounds1.Height + (double) this.Margins.Bottom + (double) this.Margins.Top > (double) this.Caption.Font.Height)
        bounds1.Height = this.Caption.Font.Height;
      if ((double) this.Caption.Width == 0.0 && this.Caption.Text != null && this.Caption.Text != string.Empty)
      {
        float num = 10f;
        double radioButtonSize = (double) this.RadioButtonSize;
        if ((double) this.RadioButtonSize > 0.0)
          num = this.RadioButtonSize;
        if (this.Caption.Position == PdfXfaPosition.Left || this.Caption.Position == PdfXfaPosition.Right)
          this.Caption.Width = bounds1.Width - num;
        else
          this.Caption.Width = bounds1.Height - num;
      }
      this.Caption.DrawText(graphics, bounds1, this.GetRotationAngle());
    }
    RectangleF bounds2 = this.GetBounds(bounds1, this.Rotate, this.Caption);
    PdfBrush foreBrush = PdfBrushes.Black;
    if (!this.ForeColor.IsEmpty)
      foreBrush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    PdfBrush backBrush = (PdfBrush) null;
    PdfPen borderPen = (PdfPen) null;
    PdfBorderStyle style1 = PdfBorderStyle.Solid;
    int borderWidth = 0;
    if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden && this.Border.Visibility != PdfXfaVisibility.Invisible)
    {
      backBrush = this.Border.GetBrush(bounds);
      borderPen = this.Border.GetFlattenPen();
      style1 = this.Border.GetBorderStyle();
      borderWidth = (int) this.Border.Width;
      if (borderWidth == 0 && (double) borderPen.Width > 0.0)
        borderWidth = 1;
    }
    PdfCheckFieldState state = PdfCheckFieldState.Unchecked;
    if (this.IsChecked)
      state = PdfCheckFieldState.Checked;
    float num1 = 10f;
    double radioButtonSize1 = (double) this.RadioButtonSize;
    if ((double) this.RadioButtonSize > 0.0)
      num1 = this.RadioButtonSize;
    float num2 = bounds2.Height - num1;
    float num3 = bounds2.Width - num1;
    if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
      bounds2.Y += num2 / 2f;
    else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
      bounds2.Y += num2;
    if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
      bounds2.X += num3 / 2f;
    else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
      bounds2.X += num3;
    bounds2.Width = num1;
    bounds2.Height = num1;
    PdfCheckBoxStyle style2 = this.GetStyle(this.m_checkedStyle);
    PdfBrush shadowBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Empty);
    graphics.Save();
    PaintParams paintParams = new PaintParams(bounds2, backBrush, foreBrush, borderPen, style1, (float) borderWidth, shadowBrush, this.GetRotationAngle());
    FieldPainter.DrawRadioButton(graphics, paintParams, this.StyleToString(style2), state);
    graphics.Restore();
  }

  internal new void Save()
  {
    base.Save();
    if (this.currentNode["ui"]["checkButton"] == null)
      return;
    XmlAttributeCollection attributes = this.currentNode["ui"]["checkButton"].Attributes;
    if (attributes["shape"] != null)
    {
      if (attributes["shape"].Value != this.RadioButtonAppearance.ToString().ToLower())
        attributes["shape"].Value = this.RadioButtonAppearance.ToString().ToLower();
    }
    else
    {
      XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("shape");
      attribute.Value = this.RadioButtonAppearance.ToString().ToLower();
      attributes.Append(attribute);
    }
    if (attributes["mark"] != null)
    {
      if (attributes["mark"].Value != this.CheckedStyle.ToString().ToLower())
        attributes["mark"].Value = this.CheckedStyle.ToString().ToLower();
    }
    else
    {
      XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("mark");
      attribute.Value = this.CheckedStyle.ToString().ToLower();
      attributes.Append(attribute);
    }
    if ((double) this.RadioButtonSize > 0.0)
    {
      if (attributes["size"] != null)
      {
        attributes["size"].Value = this.RadioButtonSize.ToString() + "pt";
      }
      else
      {
        XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("size");
        attribute.Value = this.RadioButtonSize.ToString() + "pt";
        attributes.Append(attribute);
      }
    }
    if (this.Border == null)
      return;
    if (this.currentNode["ui"]["checkButton"]["border"] != null)
      this.Border.Save((XmlNode) this.currentNode["ui"]["checkButton"]["border"]);
    else if (this.currentNode["border"] != null)
    {
      this.Border.Save((XmlNode) this.currentNode["border"]);
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
      this.Border.Save(node);
      this.currentNode["ui"]["checkButton"].AppendChild(node);
    }
  }
}
