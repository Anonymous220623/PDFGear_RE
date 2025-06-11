// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaRadioButtonGroup
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

public class PdfLoadedXfaRadioButtonGroup : PdfLoadedXfaField
{
  private List<PdfLoadedXfaRadioButtonField> m_fields = new List<PdfLoadedXfaRadioButtonField>();
  internal int m_selectedItemIndex = -1;
  internal string vText;
  private float m_width;
  private float m_height;
  private PointF m_location;
  private PdfXfaVisibility m_visibility;
  private bool m_readOnly;
  internal PdfLoadedXfaFlowDirection m_flowDirection;
  internal SizeF Size = SizeF.Empty;

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public new PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
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

  public PdfLoadedXfaRadioButtonField[] Fields => this.m_fields.ToArray();

  internal void Add(PdfLoadedXfaRadioButtonField field) => this.m_fields.Add(field);

  internal void ReadField(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (!(node.Name == "exclGroup"))
      return;
    if (node.Attributes["name"] != null)
      this.Name = node.Attributes["name"].Value;
    if (node.Attributes["w"] != null)
      this.Width = this.ConvertToPoint(node.Attributes["w"].Value);
    if (node.Attributes["h"] != null)
      this.Height = this.ConvertToPoint(node.Attributes["h"].Value);
    if (node.Attributes["x"] != null)
      this.Location = new PointF(this.ConvertToPoint(node.Attributes["x"].Value), this.Location.Y);
    if (node.Attributes["y"] != null)
      this.Location = new PointF(this.Location.X, this.ConvertToPoint(node.Attributes["y"].Value));
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
        XmlNode xmlNode = xmlNodeList[sameNameFieldsCount];
        if (xmlNode != null && xmlNode.FirstChild != null)
          this.vText = xmlNode.FirstChild.InnerText;
      }
    }
    foreach (XmlNode node1 in node)
    {
      if (node1.Name == "field")
      {
        PdfLoadedXfaRadioButtonField radioButtonField = new PdfLoadedXfaRadioButtonField();
        radioButtonField.parent = (PdfLoadedXfaField) this;
        radioButtonField.ReadField(node1);
        this.m_fields.Add(radioButtonField);
        if (this.vText != null && radioButtonField.vText == this.vText)
          radioButtonField.IsChecked = true;
        else if (this.vText != null && this.vText == radioButtonField.iText)
          radioButtonField.IsChecked = true;
        else if (this.vText == null && radioButtonField.vText == radioButtonField.iText)
          radioButtonField.IsChecked = true;
      }
    }
    if (node.Attributes["layout"] == null)
      return;
    switch (node.Attributes["layout"].Value)
    {
      case "tb":
        this.m_flowDirection = PdfLoadedXfaFlowDirection.TopToBottom;
        break;
      case "lr-tb":
        this.m_flowDirection = PdfLoadedXfaFlowDirection.LeftToRight;
        break;
      case "rl-tb":
        this.m_flowDirection = PdfLoadedXfaFlowDirection.RightToLeft;
        break;
      case "row":
        this.m_flowDirection = PdfLoadedXfaFlowDirection.Row;
        break;
      case "table":
        this.m_flowDirection = PdfLoadedXfaFlowDirection.Table;
        break;
      default:
        this.m_flowDirection = PdfLoadedXfaFlowDirection.None;
        break;
    }
  }

  internal SizeF GetSize()
  {
    SizeF size = SizeF.Empty;
    if (this.m_flowDirection == PdfLoadedXfaFlowDirection.None)
    {
      if ((double) this.Width > 0.0 && (double) this.Height > 0.0)
      {
        size = new SizeF(this.Width, this.Height);
      }
      else
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
        {
          if ((double) num1 < (double) field.Location.X + (double) field.Width)
            num1 = field.Location.X + field.Width;
          if ((double) num2 < (double) field.Location.Y + (double) field.Height)
            num2 = field.Location.Y + field.Height;
        }
        size = new SizeF(num1 + this.Margins.Right, num2 + this.Margins.Bottom);
      }
    }
    else if (this.m_flowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
    {
      float num = 0.0f;
      size.Width = this.Width;
      foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
      {
        size.Height += field.Height;
        if ((double) num < (double) field.Width)
          num = field.Width;
      }
      if ((double) this.Width == 0.0)
        size.Width = num;
    }
    else if (this.m_flowDirection == PdfLoadedXfaFlowDirection.LeftToRight || this.m_flowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
    {
      float num = 0.0f;
      size.Width = this.Width;
      PointF pointF = new PointF(this.Margins.Left, this.Margins.Top);
      foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
      {
        if ((double) this.Width == 0.0)
          size.Width += field.Width;
        else if ((double) pointF.X + (double) field.Width > (double) this.Width - (double) this.Margins.Right)
        {
          pointF.X = this.Margins.Left;
          size.Height += num;
          num = 0.0f;
        }
        pointF.X += field.Width;
        if ((double) num < (double) field.Height)
          num = field.Height;
      }
      size.Height += num;
    }
    this.Width = size.Width;
    this.Height = size.Height;
    return size;
  }

  internal void DrawRadiButtonGroup(PdfGraphics graphics, RectangleF bounds)
  {
    bool flag = false;
    if (this.m_flowDirection == PdfLoadedXfaFlowDirection.None)
    {
      foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
      {
        if (field.Visibility != PdfXfaVisibility.Hidden && field.Visibility != PdfXfaVisibility.Invisible)
        {
          if (flag)
            field.IsChecked = false;
          RectangleF bounds1 = new RectangleF(new PointF(field.Location.X + bounds.Location.X, field.Location.Y + bounds.Location.Y), new SizeF(field.Width, field.Height));
          field.DrawRadioButton(graphics, bounds1);
          if (field.IsChecked)
            flag = true;
        }
      }
    }
    else if (this.m_flowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
    {
      PointF location = new PointF(bounds.Location.X, bounds.Location.Y);
      foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
      {
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if (flag)
            field.IsChecked = false;
          RectangleF bounds2 = new RectangleF(location, new SizeF(field.Width, field.Height));
          if (field.Visibility != PdfXfaVisibility.Invisible)
            field.DrawRadioButton(graphics, bounds2);
          location.Y += field.Height;
          if (field.IsChecked)
            flag = true;
        }
      }
    }
    else
    {
      if (this.m_flowDirection != PdfLoadedXfaFlowDirection.LeftToRight && this.m_flowDirection != PdfLoadedXfaFlowDirection.RightToLeft)
        return;
      PointF location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
      PointF empty = PointF.Empty;
      float num = 0.0f;
      foreach (PdfLoadedXfaRadioButtonField field in this.Fields)
      {
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if ((double) empty.X + (double) field.Width > (double) this.Width - (double) this.Margins.Right)
          {
            location.X = bounds.Location.X + this.Margins.Left;
            empty.X = this.Margins.Left;
            empty.Y += num;
            location.Y += num;
            num = 0.0f;
          }
          if (flag)
            field.IsChecked = false;
          RectangleF bounds3 = new RectangleF(location, new SizeF(field.Width, field.Height));
          if (field.Visibility != PdfXfaVisibility.Invisible)
            field.DrawRadioButton(graphics, bounds3);
          location.X += field.Width;
          empty.X += field.Width;
          if ((double) num < (double) field.Height)
            num = field.Height;
          if (field.IsChecked)
            flag = true;
        }
      }
    }
  }

  internal void SetIndex()
  {
    if (this.m_selectedItemIndex != -1)
      return;
    for (int index = 0; index < this.m_fields.Count; ++index)
    {
      if (this.m_fields[index].IsChecked)
        this.m_selectedItemIndex = index;
    }
  }

  internal void ResetSelection()
  {
    for (int index = 0; index < this.m_fields.Count; ++index)
      this.m_fields[index].m_isChecked = false;
  }

  internal void Save()
  {
    for (int index = 0; index < this.Fields.Length; ++index)
      this.Fields[index].Save();
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    PdfLoadedRadioButtonListField field = form.acroForm.GetField(this.nodeName) as PdfLoadedRadioButtonListField;
    this.Save();
    this.SetIndex();
    if (field != null && this.m_selectedItemIndex != -1)
      field.SelectedIndex = this.m_selectedItemIndex;
    if (this.Name != string.Empty)
      dataSetWriter.WriteStartElement(this.Name);
    if (this.m_selectedItemIndex >= 0)
    {
      if (!string.IsNullOrEmpty(this.Fields[this.m_selectedItemIndex].iText) && this.Name != string.Empty)
        dataSetWriter.WriteString(this.Fields[this.m_selectedItemIndex].iText);
      else if (!string.IsNullOrEmpty(this.Fields[this.m_selectedItemIndex].vText) && this.Name != string.Empty)
        dataSetWriter.WriteString(this.Fields[this.m_selectedItemIndex].vText);
    }
    else if (!string.IsNullOrEmpty(this.vText) && this.Name != string.Empty)
      dataSetWriter.WriteString(this.vText);
    if (!(this.Name != string.Empty))
      return;
    dataSetWriter.WriteEndElement();
  }
}
