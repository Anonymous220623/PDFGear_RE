// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaCheckBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaCheckBoxField : PdfLoadedXfaStyledField
{
  private bool m_isChecked;
  private float m_checkBoxSize;
  private PdfXfaCheckedStyle m_checkedStyle;
  private PdfXfaCheckBoxAppearance m_checkBoxAppearance;
  internal string m_innerText = string.Empty;
  internal bool isItemText;

  public bool IsChecked
  {
    get => this.m_isChecked;
    set => this.m_isChecked = value;
  }

  public float CheckBoxSize
  {
    get => this.m_checkBoxSize;
    set
    {
      if ((double) value <= 0.0)
        return;
      this.m_checkBoxSize = value;
    }
  }

  public PdfXfaCheckedStyle CheckedStyle
  {
    set => this.m_checkedStyle = value;
    get => this.m_checkedStyle;
  }

  public PdfXfaCheckBoxAppearance CheckBoxAppearance
  {
    set => this.m_checkBoxAppearance = value;
    get => this.m_checkBoxAppearance;
  }

  internal void Read(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (!(node.Name == "field"))
      return;
    this.ReadCommonProperties(this.currentNode);
    if (node["ui"]["checkButton"] != null)
    {
      XmlAttributeCollection attributes = node["ui"]["checkButton"].Attributes;
      if (attributes["shape"] != null)
        this.CheckBoxAppearance = attributes["shape"].Value == "square" ? PdfXfaCheckBoxAppearance.Square : PdfXfaCheckBoxAppearance.Round;
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
        this.CheckBoxSize = this.ConvertToPoint(attributes["size"].Value);
      if (node["ui"]["checkButton"]["border"] != null)
        this.ReadBorder((XmlNode) node["ui"]["checkButton"]["border"], false);
    }
    if (node["items"] != null && node["items"]["text"] != null)
    {
      XmlNode xmlNode = (XmlNode) node["items"]["text"];
      if (xmlNode.InnerText != string.Empty)
        this.m_innerText = xmlNode.InnerText;
      this.isItemText = true;
    }
    this.nodeName = this.parent.nodeName;
    string empty = string.Empty;
    string str1 = !(this.nodeName != string.Empty) ? this.Name : $"{this.nodeName}.{this.Name}";
    if (dataSetDoc != null)
    {
      string[] strArray = str1.Split('[');
      string str2 = string.Empty;
      foreach (string str3 in strArray)
      {
        if (str3.Contains("]"))
        {
          int startIndex = str3.IndexOf(']') + 2;
          if (str3.Length > startIndex)
            str2 = $"{str2}/{str3.Substring(startIndex)}";
        }
        else
          str2 += str3;
      }
      string xpath = "//" + str2;
      while (xpath.Contains("#"))
      {
        int startIndex1 = xpath.IndexOf("#");
        if (startIndex1 != -1)
        {
          string str4 = xpath.Substring(0, startIndex1 - 1);
          string str5 = xpath.Substring(startIndex1);
          int startIndex2 = str5.IndexOf("/");
          string str6 = string.Empty;
          if (startIndex2 != -1)
            str6 = str5.Substring(startIndex2);
          xpath = (str4 + str6).TrimEnd('/');
        }
      }
      XmlNodeList xmlNodeList = dataSetDoc.SelectNodes(xpath);
      int sameNameFieldsCount = this.parent.GetSameNameFieldsCount(this.Name);
      if (xmlNodeList == null || xmlNodeList.Count <= sameNameFieldsCount)
        return;
      XmlNode xmlNode = xmlNodeList[sameNameFieldsCount];
      if (xmlNode == null || xmlNode.FirstChild == null || !(xmlNode.FirstChild.InnerText != string.Empty))
        return;
      this.IsChecked = int.Parse(xmlNode.FirstChild.InnerText) != 0;
    }
    else
    {
      if (node["value"] == null || node["value"]["integer"] == null)
        return;
      XmlNode xmlNode = (XmlNode) node["value"]["integer"];
      if (!(xmlNode.InnerText != string.Empty))
        return;
      this.IsChecked = int.Parse(xmlNode.InnerText) != 0;
    }
  }

  internal new void Save()
  {
    base.Save();
    if (this.currentNode["ui"]["checkButton"] == null)
      return;
    XmlAttributeCollection attributes = this.currentNode["ui"]["checkButton"].Attributes;
    if (attributes["shape"] != null)
    {
      if (attributes["shape"].Value != this.CheckBoxAppearance.ToString().ToLower())
        attributes["shape"].Value = this.CheckBoxAppearance.ToString().ToLower();
    }
    else
    {
      XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("shape");
      attribute.Value = this.CheckBoxAppearance.ToString().ToLower();
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
    if ((double) this.CheckBoxSize > 0.0)
    {
      if (attributes["size"] != null)
      {
        attributes["size"].Value = this.CheckBoxSize.ToString() + "pt";
      }
      else
      {
        XmlAttribute attribute = this.currentNode.OwnerDocument.CreateAttribute("size");
        attribute.Value = this.CheckBoxSize.ToString() + "pt";
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

  internal void DrawField(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    SizeF size1 = this.GetSize();
    PdfStringFormat pdfStringFormat = new PdfStringFormat()
    {
      LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment,
      Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment)
    };
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size1.Width - (this.Margins.Right + this.Margins.Left), size1.Height - (this.Margins.Top + this.Margins.Bottom));
    bool flag = false;
    if (this.Border == null && this.parent is PdfLoadedXfaForm && (this.parent as PdfLoadedXfaForm).FlowDirection == PdfLoadedXfaFlowDirection.Row)
      flag = true;
    if (this.CompleteBorder != null && !flag)
      this.CompleteBorder.DrawBorder(graphics, bounds);
    if (this.Visibility != PdfXfaVisibility.Invisible && this.Caption != null)
    {
      if (this.Caption.Font == null)
      {
        if (this.Caption.Text != null && this.Caption.Text != string.Empty && PdfString.IsUnicode(this.Caption.Text) && this.m_isDefaultFont)
          this.CheckUnicodeFont(this.Caption.Text);
        this.Caption.Font = this.Font;
      }
      if (this.Caption.Font != null && (double) bounds1.Height < (double) this.Caption.Font.Height && (double) bounds1.Height + (double) this.Margins.Bottom + (double) this.Margins.Top > (double) this.Caption.Font.Height)
        bounds1.Height = this.Caption.Font.Height;
      if ((double) this.Caption.Width == 0.0 && this.Caption.Text != null && this.Caption.Text != string.Empty)
      {
        float num = 10f;
        double checkBoxSize = (double) this.CheckBoxSize;
        if ((double) this.CheckBoxSize > 0.0)
          num = this.CheckBoxSize;
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
      borderWidth = (int) borderPen.Width;
      if (borderWidth == 0 && (double) borderPen.Width > 0.0)
        borderWidth = 1;
    }
    PdfCheckFieldState state = PdfCheckFieldState.Unchecked;
    if (this.IsChecked)
      state = PdfCheckFieldState.Checked;
    float size2 = 10f;
    double checkBoxSize1 = (double) this.CheckBoxSize;
    if ((double) this.CheckBoxSize > 0.0)
      size2 = this.CheckBoxSize;
    float num1 = bounds2.Height - size2;
    float num2 = bounds2.Width - size2;
    if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
      bounds2.Y += num1 / 2f;
    else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
      bounds2.Y += num1;
    if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
      bounds2.X += num2 / 2f;
    else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
      bounds2.X += num2;
    if (this.m_isDefaultFont && (double) this.Font.Size > (double) size2)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, size2, PdfFontStyle.Regular);
    bounds2.Width = size2;
    bounds2.Height = size2;
    PdfCheckBoxStyle style2 = this.GetStyle(this.m_checkedStyle);
    graphics.Save();
    PaintParams paintParams = new PaintParams(bounds2, backBrush, foreBrush, borderPen, style1, (float) borderWidth, (PdfBrush) null, this.GetRotationAngle());
    FieldPainter.DrawCheckBox(graphics, paintParams, this.StyleToString(style2), state);
    graphics.Restore();
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    string text = string.Empty;
    if (this.isItemText && form.GetSameNameFieldsCount(this.Name) > 0 && !this.IsChecked)
    {
      string[] strArray = this.nodeName.Split('.');
      if (int.Parse(strArray[strArray.Length - 1].Replace(this.Name + "[", "").Replace("]", "")) == 0)
      {
        foreach (PdfLoadedXfaCheckBoxField sameNameField in form.GetSameNameFields(this.Name, form))
        {
          if (sameNameField.IsChecked && sameNameField.currentNode["bind"] != null && sameNameField.currentNode["bind"].Attributes["match"] != null && sameNameField.currentNode["bind"].Attributes["match"].Value == "global")
            text = !(sameNameField.m_innerText != string.Empty) || !(sameNameField.Name != string.Empty) ? "1" : sameNameField.m_innerText;
        }
      }
    }
    if (form.acroForm.GetField(this.nodeName) is PdfLoadedCheckBoxField field)
      field.Checked = this.IsChecked;
    if (this.Name != string.Empty)
      dataSetWriter.WriteStartElement(this.Name);
    if (this.IsChecked)
    {
      if (this.m_innerText != string.Empty && this.Name != string.Empty)
        dataSetWriter.WriteString(this.m_innerText);
      else if (this.Name != string.Empty)
        dataSetWriter.WriteString("1");
    }
    else if (this.Name != string.Empty)
    {
      if (text != string.Empty)
        dataSetWriter.WriteString(text);
      else
        dataSetWriter.WriteString("0");
    }
    this.Save();
    if (!(this.Name != string.Empty))
      return;
    dataSetWriter.WriteEndElement();
  }
}
