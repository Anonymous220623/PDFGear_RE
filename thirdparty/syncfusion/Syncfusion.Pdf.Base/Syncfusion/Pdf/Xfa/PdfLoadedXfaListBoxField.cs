// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaListBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaListBoxField : PdfLoadedXfaStyledField
{
  private List<string> m_items = new List<string>();
  private int m_selectedIndex = -1;
  private string m_selectedValue = string.Empty;
  private PdfXfaSelectionMode m_selectionMode;
  private string[] m_selectedItems;
  private PdfMargins m_innerMargin;

  public string[] SelectedItems
  {
    get => this.m_selectedItems;
    set
    {
      if (value == null || value.Length <= 0)
        return;
      this.m_selectedItems = value;
      this.m_selectedValue = this.m_selectedItems[0];
    }
  }

  public List<string> Items
  {
    get => this.m_items;
    set
    {
      if (value == null)
        return;
      this.m_items = value;
    }
  }

  public int SelectedIndex
  {
    get => this.m_selectedIndex;
    set
    {
      if (value < 0 || value >= this.m_items.Count)
        return;
      this.m_selectedIndex = value;
      this.m_selectedValue = this.m_items[value];
    }
  }

  public string SelectedValue
  {
    get => this.m_selectedValue;
    set
    {
      this.m_selectedValue = value != null && this.m_items.Contains(value) ? value : throw new PdfException("The Value doesn't exists");
    }
  }

  public PdfXfaSelectionMode SelectionMode => this.m_selectionMode;

  internal void ReadField(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (!(node.Name == "field"))
      return;
    this.ReadCommonProperties(node);
    if (node["value"] != null && node["value"]["text"] != null)
      this.m_selectedValue = node["value"]["text"].InnerText;
    if (node["items"] != null)
    {
      foreach (XmlNode xmlNode in (XmlNode) node["items"])
      {
        if (xmlNode.Name == "text")
          this.m_items.Add(xmlNode.InnerText);
      }
    }
    XmlAttributeCollection attributes = node["ui"]["choiceList"].Attributes;
    if (attributes["open"] != null && attributes["open"].Value.ToLower() == "multiselect")
      this.m_selectionMode = PdfXfaSelectionMode.Multiple;
    if (node["ui"]["choiceList"]["border"] != null)
      this.ReadBorder((XmlNode) node["ui"]["choiceList"]["border"], false);
    if (node["ui"]["choiceList"]["margin"] != null)
    {
      this.m_innerMargin = new PdfMargins();
      this.ReadMargin((XmlNode) node["ui"]["choiceList"]["margin"], this.m_innerMargin);
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
      if (xmlNodeList != null && xmlNodeList.Count > sameNameFieldsCount)
      {
        XmlNode xmlNode1 = xmlNodeList[sameNameFieldsCount];
        if (xmlNode1 != null && xmlNode1.FirstChild != null)
        {
          if (this.SelectionMode == PdfXfaSelectionMode.Multiple)
          {
            List<string> stringList = new List<string>();
            foreach (XmlNode xmlNode2 in xmlNode1)
            {
              if (xmlNode2.Name == "value")
                stringList.Add(xmlNode2.InnerText);
            }
            this.m_selectedItems = stringList.ToArray();
          }
          else
            this.m_selectedValue = xmlNode1.FirstChild.InnerText;
        }
      }
    }
    if (string.IsNullOrEmpty(this.m_selectedValue) || !this.m_items.Contains(this.m_selectedValue))
      return;
    this.m_selectedIndex = this.m_items.IndexOf(this.m_selectedValue);
  }

  internal new void Save()
  {
    base.Save();
    XmlElement xmlElement = this.currentNode["items"];
    if (xmlElement != null)
    {
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (xmlElement.ChildNodes.Count > index)
        {
          XmlNode childNode = xmlElement.ChildNodes[index];
          if (childNode.InnerText != this.Items[index])
            childNode.InnerText = this.Items[index];
        }
        else
        {
          XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
          node.InnerText = this.Items[index];
          xmlElement.AppendChild(node);
        }
      }
      if (xmlElement.ChildNodes.Count > this.Items.Count)
      {
        int count = xmlElement.ChildNodes.Count;
        for (int index = 0; index < count; ++index)
        {
          if (index >= this.Items.Count)
            xmlElement.RemoveChild(xmlElement.ChildNodes[this.Items.Count]);
        }
      }
    }
    XmlAttributeCollection attributes = this.currentNode["ui"]["choiceList"].Attributes;
    if (this.SelectedValue != null)
    {
      if (this.currentNode["value"] != null)
      {
        if (this.currentNode["value"]["text"] != null)
        {
          this.currentNode["value"]["text"].InnerText = this.SelectedValue;
        }
        else
        {
          XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
          node.InnerText = this.SelectedValue;
          this.currentNode["value"].AppendChild(node);
        }
      }
      else
      {
        XmlNode node1 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "value", "");
        XmlNode node2 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
        node2.InnerText = this.SelectedValue;
        node1.AppendChild(node2);
        this.currentNode.AppendChild(node1);
      }
    }
    if (this.Border == null)
      return;
    if (this.currentNode["ui"]["choiceList"]["border"] != null)
      this.Border.Save((XmlNode) this.currentNode["ui"]["choiceList"]["border"]);
    else if (this.currentNode["border"] != null)
    {
      this.Border.Save((XmlNode) this.currentNode["border"]);
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
      this.Border.Save(node);
      this.currentNode["ui"]["choiceList"].AppendChild(node);
    }
  }

  internal void DrawListBoxField(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    SizeF size = this.GetSize();
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    bool flag = false;
    if (this.Border == null && this.parent is PdfLoadedXfaForm && (this.parent as PdfLoadedXfaForm).FlowDirection == PdfLoadedXfaFlowDirection.Row)
      flag = true;
    if (this.CompleteBorder != null && this.CompleteBorder.Visibility != PdfXfaVisibility.Hidden && this.CompleteBorder.Visibility != PdfXfaVisibility.Invisible && !flag)
      this.CompleteBorder.DrawBorder(graphics, bounds);
    if (this.Visibility != PdfXfaVisibility.Invisible && this.Caption != null)
    {
      if (this.Caption.Font == null)
        this.Caption.Font = this.Font;
      if (this.Caption.Font != null && (double) bounds1.Height < (double) this.Caption.Font.Height && (double) bounds1.Height + (double) this.Margins.Bottom + (double) this.Margins.Top > (double) this.Caption.Font.Height)
        bounds1.Height = this.Caption.Font.Height;
      this.Caption.DrawText(graphics, bounds1, this.GetRotationAngle());
    }
    RectangleF bounds2 = this.GetBounds(bounds1, this.Rotate, this.Caption);
    RectangleF rectangleF1 = RectangleF.Empty;
    rectangleF1 = this.Rotate == PdfXfaRotateAngle.RotateAngle0 || this.Rotate == PdfXfaRotateAngle.RotateAngle180 ? new RectangleF(0.0f, 0.0f, bounds2.Width, bounds2.Height) : new RectangleF(0.0f, 0.0f, bounds2.Height, bounds2.Width);
    PdfTemplate template = new PdfTemplate(rectangleF1.Size);
    if (this.Visibility == PdfXfaVisibility.Invisible || this.Visibility == PdfXfaVisibility.Hidden)
      return;
    if (this.Border != null)
      this.Border.DrawBorder(template.Graphics, rectangleF1);
    if (this.Items != null && this.Items.Count > 0)
    {
      RectangleF bounds3 = this.GetBounds(rectangleF1);
      float height = this.Font.MeasureString(this.Items[0]).Height;
      float num1 = (float) ((double) bounds3.Height / (double) height + 0.5);
      float num2 = this.Items.Count > 0 ? num1 / (float) this.Items.Count : 0.0f;
      float y = bounds3.Y + num2;
      float num3 = 0.0f;
      PdfBrush brush = (PdfBrush) new PdfSolidBrush(new PdfColor((byte) 153, (byte) 193, (byte) 219));
      if (this.ForeColor.IsEmpty)
        this.ForeColor = (PdfColor) Color.Black;
      foreach (string s in this.Items)
      {
        if ((double) num3 + ((double) height + 0.5 + (double) num2) <= (double) bounds3.Height)
        {
          RectangleF rectangleF2 = new RectangleF(bounds3.X, y, bounds3.Width, height + 0.5f + num2);
          y += height + 0.5f + num2;
          num3 += height + 0.5f + num2;
          if (this.m_selectedValue != string.Empty && s == this.m_selectedValue)
          {
            template.Graphics.Save();
            template.Graphics.DrawRectangle(brush, rectangleF2);
            template.Graphics.Restore();
          }
          else if (this.SelectionMode == PdfXfaSelectionMode.Multiple && this.m_selectedItems != null)
          {
            for (int index = 0; index < this.m_selectedItems.Length; ++index)
            {
              if (this.m_selectedItems[index] == s)
              {
                template.Graphics.Save();
                template.Graphics.DrawRectangle(brush, rectangleF2);
                template.Graphics.Restore();
              }
            }
          }
          template.Graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), rectangleF2, format);
        }
      }
    }
    graphics.Save();
    graphics.TranslateTransform(bounds2.X, bounds2.Y);
    graphics.RotateTransform((float) -this.GetRotationAngle());
    RectangleF renderingRect = this.GetRenderingRect(bounds2);
    graphics.DrawPdfTemplate(template, renderingRect.Location);
    graphics.Restore();
  }

  private RectangleF GetBounds(RectangleF bounds1)
  {
    if (this.m_innerMargin != null)
    {
      bounds1.X += this.m_innerMargin.Left;
      bounds1.Y += this.m_innerMargin.Top;
      bounds1.Width -= this.m_innerMargin.Left + this.m_innerMargin.Right;
      bounds1.Height -= this.m_innerMargin.Top + this.m_innerMargin.Bottom;
    }
    return bounds1;
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    PdfLoadedListBoxField field = form.acroForm.GetField(this.nodeName) as PdfLoadedListBoxField;
    if (this.Name != string.Empty)
      dataSetWriter.WriteStartElement(this.Name);
    if (this.SelectionMode == PdfXfaSelectionMode.Multiple && this.SelectedItems != null && this.SelectedItems.Length > 0 && this.Name != string.Empty)
    {
      for (int index = 0; index < this.SelectedItems.Length; ++index)
      {
        dataSetWriter.WriteStartElement("value");
        dataSetWriter.WriteString(this.SelectedItems[index]);
        dataSetWriter.WriteEndElement();
      }
      if (field != null && field.Items.Count > 0)
        field.SelectedValue = this.SelectedItems;
    }
    else if (!string.IsNullOrEmpty(this.SelectedValue) && this.Name != string.Empty)
    {
      dataSetWriter.WriteString(this.SelectedValue);
      if (field != null && field.Items.Count > 0)
        field.SelectedValue = new string[1]
        {
          this.SelectedValue
        };
    }
    else if (this.SelectedIndex != -1 && this.Name != string.Empty)
    {
      dataSetWriter.WriteString(this.Items[this.SelectedIndex]);
      if (field != null && field.Items.Count > 0)
        field.SelectedIndex = new int[1]
        {
          this.SelectedIndex
        };
    }
    if (this.Name != string.Empty)
      dataSetWriter.WriteEndElement();
    this.Save();
  }
}
