// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaTextBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaTextBoxField : PdfLoadedXfaStyledField
{
  private string m_text;
  internal string Node = string.Empty;
  private float m_minW;
  private float m_maxW;
  private float m_minH;
  private float m_maxH;
  private char m_passwordChar;
  private int m_maxLength;
  private int m_combLength;
  private bool m_passwordEdit;
  private PdfXfaTextBoxType m_type;
  internal bool m_isExData;
  private string m_altText;
  private PdfMargins m_innerMargin;
  private bool isSkipDefaultMargin;

  public float MaximumWidth
  {
    get => this.m_maxW;
    set => this.m_maxW = value;
  }

  public float MaximumHeight
  {
    get => this.m_maxH;
    set => this.m_maxH = value;
  }

  public float MinimumWidth
  {
    get => this.m_minW;
    set => this.m_minW = value;
  }

  public float MinimumHeight
  {
    get => this.m_minH;
    set => this.m_minH = value;
  }

  public string Text
  {
    set => this.m_text = value;
    get => this.m_text;
  }

  public PdfXfaTextBoxType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public char PasswordCharacter
  {
    get => this.m_passwordChar;
    set => this.m_passwordChar = value;
  }

  public int MaximumLength
  {
    get => this.m_maxLength;
    set
    {
      if (value <= 0)
        return;
      this.m_maxLength = value;
    }
  }

  public int CombLength
  {
    get => this.m_combLength;
    set
    {
      if (value < 0)
        return;
      this.m_combLength = value;
    }
  }

  internal PdfLoadedXfaTextBoxField(string name) => this.Name = name;

  internal PdfLoadedXfaTextBoxField()
  {
  }

  internal PdfLoadedXfaTextBoxField(string name, string text)
  {
    this.Name = name;
    this.Text = text;
  }

  internal SizeF GetFieldSize()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if ((double) this.Width <= 0.0)
    {
      if ((double) this.MinimumWidth > 0.0)
        num2 = this.MinimumWidth;
      else if ((double) this.MaximumWidth > 0.0)
        num2 = this.MaximumWidth;
    }
    else
      num2 = this.Width;
    if ((double) this.Height <= 0.0)
    {
      if ((double) this.MinimumHeight > 0.0)
      {
        num1 = this.MinimumHeight;
        if ((double) num1 < (double) this.MinimumHeight)
          num1 = this.MinimumHeight;
        PdfLoadedXfaForm parent = this.parent as PdfLoadedXfaForm;
        if (!string.IsNullOrEmpty(this.Text) && this.Font != null && parent != null && parent.FlowDirection != PdfLoadedXfaFlowDirection.Row)
        {
          SizeF sizeF = SizeF.Empty;
          if (this.m_isExData)
          {
            if (this.m_altText != null)
              sizeF = this.Font.MeasureString(this.m_altText, num2);
          }
          else
          {
            sizeF = this.Font.MeasureString(this.Text);
            if ((double) sizeF.Width > (double) num2)
              sizeF = this.Font.MeasureString(this.Text, num2);
          }
          if ((double) num1 < (double) sizeF.Height)
          {
            num1 = sizeF.Height;
            if (parent.FlowDirection != PdfLoadedXfaFlowDirection.Row)
              num1 += this.Margins.Top + this.Margins.Bottom;
          }
          if ((double) this.MaximumHeight > 0.0 && (double) sizeF.Height > (double) this.MaximumHeight)
            num1 = this.MaximumHeight;
        }
      }
      else if (this.currentNode.Attributes["h"] == null)
      {
        if (this.Font != null)
        {
          if (this.m_isExData && !string.IsNullOrEmpty(this.m_altText))
            this.Height = this.Font.MeasureString(this.m_altText, this.Width).Height;
          else if (!string.IsNullOrEmpty(this.Text))
            this.Height = this.Font.MeasureString(this.Text, this.Width).Height;
          else if ((double) this.Font.Height > (double) this.Height)
            this.Height = this.Font.Height + 0.5f;
        }
      }
      else if ((double) this.MaximumHeight > 0.0)
        num1 = this.MaximumHeight;
    }
    else
      num1 = this.Height;
    if ((double) this.Width < (double) num2)
      this.Width = num2;
    if ((double) this.Height < (double) num1)
      this.Height = num1;
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(num1, num2) : new SizeF(num2, num1);
  }

  private float GetWidth()
  {
    float width = 0.0f;
    if ((double) this.Width <= 0.0)
    {
      if ((double) this.MaximumWidth > 0.0)
        width = this.MaximumWidth;
      if ((double) this.MinimumWidth > 0.0)
        width = this.MinimumWidth;
    }
    else
      width = this.Width;
    return width;
  }

  internal void Read(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (!(node.Name == "field"))
      return;
    this.ReadCommonProperties(node);
    if (node.Attributes["minW"] != null)
      this.MinimumWidth = this.ConvertToPoint(node.Attributes["minW"].Value);
    if (node.Attributes["minH"] != null)
      this.MinimumHeight = this.ConvertToPoint(node.Attributes["minH"].Value);
    if (node.Attributes["maxW"] != null)
      this.MaximumWidth = this.ConvertToPoint(node.Attributes["maxW"].Value);
    if (node.Attributes["maxH"] != null)
      this.MaximumHeight = this.ConvertToPoint(node.Attributes["maxH"].Value);
    if (node["value"] != null)
    {
      if (node["value"]["text"] != null)
      {
        this.Text = node["value"]["text"].Value;
        if (node["value"]["text"].Value == null)
          this.Text = node["value"]["text"].InnerText;
        if (node["value"]["text"].Attributes["maxChars"] != null)
          this.m_maxLength = int.Parse(node["value"]["text"].Attributes["maxChars"].InnerText);
      }
      else if (node["value"]["exData"] != null)
      {
        this.Text = node["value"]["exData"].InnerXml;
        this.m_altText = node["value"]["exData"].InnerText;
        this.m_isExData = true;
      }
    }
    if (node["ui"] != null)
    {
      if (node["ui"]["textEdit"] != null)
      {
        if (node["ui"]["textEdit"].Attributes["multiLine"] != null && int.Parse(node["ui"]["textEdit"].Attributes["multiLine"].InnerText) == 1)
          this.Type = PdfXfaTextBoxType.Multiline;
        if (node["ui"]["textEdit"]["comb"] != null && node["ui"]["textEdit"]["comb"].Attributes["numberOfCells"] != null)
        {
          this.CombLength = int.Parse(node["ui"]["textEdit"]["comb"].Attributes["numberOfCells"].InnerText);
          if (this.CombLength > 0)
            this.Type = PdfXfaTextBoxType.Comb;
        }
        if (node["ui"]["textEdit"]["border"] != null)
          this.ReadBorder((XmlNode) node["ui"]["textEdit"]["border"], false);
        if (node["ui"]["textEdit"]["margin"] != null)
        {
          this.m_innerMargin = new PdfMargins();
          this.ReadMargin((XmlNode) node["ui"]["textEdit"]["margin"], this.m_innerMargin);
        }
      }
      else if (node["ui"]["passwordEdit"] != null)
      {
        this.Type = PdfXfaTextBoxType.Password;
        if (node["ui"]["passwordEdit"].Attributes["passwordChar"] != null)
          this.m_passwordChar = char.Parse(node["ui"]["passwordEdit"].Attributes["passwordChar"].InnerText);
        if (node["ui"]["passwordEdit"]["border"] != null)
          this.ReadBorder((XmlNode) node["ui"]["passwordEdit"]["border"], false);
        this.m_passwordEdit = true;
        if (node["ui"]["passwordEdit"]["margin"] != null)
        {
          this.m_innerMargin = new PdfMargins();
          this.ReadMargin((XmlNode) node["ui"]["passwordEdit"]["margin"], this.m_innerMargin);
        }
      }
    }
    try
    {
      char[] chArray = new char[1]{ '.' };
      string str1 = string.Empty;
      bool flag1 = false;
      if (node["bind"] != null)
      {
        XmlNode xmlNode = (XmlNode) node["bind"];
        if (xmlNode.Attributes["match"] != null && xmlNode.Attributes["match"].Value != "none")
          flag1 = true;
        if (xmlNode.Attributes["ref"] != null)
        {
          string str2 = xmlNode.Attributes["ref"].Value;
          bool flag2 = false;
          if (str2.Contains("$record") || str2.Contains("$"))
            flag2 = true;
          str1 = str2.Replace("$record.", "").Replace("$.", "").Replace("[*]", "");
          if (!flag2 && xmlNode.Attributes["match"] != null && xmlNode.Attributes["match"].Value == "dataRef" && this.parent.bindingName != string.Empty)
            str1 = $"{this.parent.bindingName}.{str1}";
        }
        else if (xmlNode.Attributes["match"] != null && xmlNode.Attributes["match"].Value == "global")
          str1 = this.Name;
        else if (this.parent.bindingName != null && !flag1)
          str1 = $"{this.parent.bindingName}.{this.Name}".Replace("$.", "").Replace(".", "/");
      }
      if (str1 != string.Empty)
      {
        string str3 = str1.Replace("$.", "").Replace(".", "/");
        string xpath1 = "//" + $"{this.parent.nodeName.Split(chArray)[0].Replace("[0]", "")}/{str3}";
        if (dataSetDoc != null)
        {
          XmlNode xmlNode1 = dataSetDoc.SelectSingleNode(xpath1);
          if (xmlNode1 != null)
          {
            if (xmlNode1.FirstChild != null)
            {
              if (this.m_isExData)
              {
                this.Text = xmlNode1.FirstChild.OuterXml;
                this.m_altText = xmlNode1.FirstChild.InnerText;
              }
              else
                this.Text = xmlNode1.FirstChild.InnerText;
            }
          }
          else if (this.parent.bindingName != null)
          {
            if (!flag1)
            {
              string xpath2 = "//" + $"{this.parent.nodeName.Split(chArray)[0].Replace("[0]", "")}/{$"{this.parent.bindingName}.{this.Name}".Replace("$.", "").Replace(".", "/")}";
              XmlNode xmlNode2 = dataSetDoc.SelectSingleNode(xpath2);
              if (xmlNode2 != null)
              {
                if (xmlNode2.FirstChild != null)
                  this.Text = xmlNode2.FirstChild.InnerText;
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    this.nodeName = this.parent.nodeName;
    string empty = string.Empty;
    string str4 = !(this.nodeName != string.Empty) ? this.Name : $"{this.nodeName}.{this.Name}";
    if (dataSetDoc == null)
      return;
    string[] strArray = str4.Split('[');
    string str5 = string.Empty;
    foreach (string str6 in strArray)
    {
      if (str6.Contains("]"))
      {
        int startIndex = str6.IndexOf(']') + 2;
        if (str6.Length > startIndex)
          str5 = $"{str5}/{str6.Substring(startIndex)}";
      }
      else
        str5 += str6;
    }
    string xpath = "//" + str5;
    while (xpath.Contains("#"))
    {
      int startIndex1 = xpath.IndexOf("#");
      if (startIndex1 != -1)
      {
        string str7 = xpath.Substring(0, startIndex1 - 1);
        string str8 = xpath.Substring(startIndex1);
        int startIndex2 = str8.IndexOf("/");
        string str9 = string.Empty;
        if (startIndex2 != -1)
          str9 = str8.Substring(startIndex2);
        xpath = (str7 + str9).TrimEnd('/');
      }
    }
    XmlNodeList xmlNodeList = dataSetDoc.SelectNodes(xpath);
    int sameNameFieldsCount = this.parent.GetSameNameFieldsCount(this.Name);
    if (xmlNodeList == null || xmlNodeList.Count <= sameNameFieldsCount)
      return;
    XmlNode xmlNode3 = xmlNodeList[sameNameFieldsCount];
    if (xmlNode3 == null || xmlNode3.FirstChild == null)
      return;
    this.Text = xmlNode3.FirstChild.InnerText;
  }

  internal void DrawField(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    if (this.Text != string.Empty && this.m_isDefaultFont)
      this.CheckUnicodeFont(this.Text);
    SizeF fieldSize = this.GetFieldSize();
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(fieldSize.Width - (this.Margins.Right + this.Margins.Left), fieldSize.Height - (this.Margins.Top + this.Margins.Bottom));
    if ((double) bounds1.Height <= 0.0)
      return;
    bool flag = false;
    if (this.Border == null && this.parent is PdfLoadedXfaForm && (this.parent as PdfLoadedXfaForm).FlowDirection == PdfLoadedXfaFlowDirection.Row)
      flag = true;
    if (this.CompleteBorder != null && this.CompleteBorder.Visibility != PdfXfaVisibility.Hidden && this.CompleteBorder.Visibility != PdfXfaVisibility.Invisible && !flag)
      this.CompleteBorder.DrawBorder(graphics, bounds);
    if (this.Visibility != PdfXfaVisibility.Invisible && this.Caption != null)
    {
      if (this.Caption.Font == null)
        this.Caption.Font = this.Font;
      if ((double) bounds1.Height < (double) this.Caption.Font.Height)
        bounds1.Height = (double) bounds1.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) < (double) this.Caption.Font.Height ? this.Caption.Font.Height : this.Caption.Font.Height;
      this.Caption.DrawText(graphics, bounds1, this.GetRotationAngle());
    }
    RectangleF bounds2 = this.GetBounds(bounds1, this.Rotate, this.Caption);
    if (this.Text != null && this.Text != string.Empty)
    {
      if (this.ForeColor.IsEmpty)
        this.ForeColor = (PdfColor) Color.Black;
      if ((double) bounds2.Height < (double) this.Font.Height)
      {
        bounds2.Height = (double) bounds2.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) < (double) this.Font.Height ? this.Font.Height : this.Font.Height;
        this.isSkipDefaultMargin = true;
      }
    }
    if (this.Visibility == PdfXfaVisibility.Hidden)
      return;
    if (this.Type == PdfXfaTextBoxType.Comb && this.CombLength > 0)
    {
      RectangleF tempBounds = bounds2;
      bounds2.Location = PointF.Empty;
      PdfTemplate template = new PdfTemplate(bounds2.Size);
      float width = bounds2.Width / (float) this.CombLength;
      PdfPen pen = (PdfPen) null;
      PdfBrush brush = (PdfBrush) null;
      if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
      {
        pen = this.Border.GetFlattenPen();
        brush = this.Border.GetBrush(bounds);
      }
      char[] chArray = (char[]) null;
      if (this.Text != null && this.Text != string.Empty)
        chArray = this.Text.ToCharArray();
      RectangleF rectangleF = new RectangleF(bounds2.X, bounds2.Y, width, bounds2.Height);
      for (int index = 0; index < this.CombLength; ++index)
      {
        if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
          template.Graphics.DrawRectangle(pen, brush, rectangleF);
        if (chArray != null && chArray.Length > index)
        {
          format.Alignment = PdfTextAlignment.Center;
          template.Graphics.DrawString(chArray[index].ToString(), this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), rectangleF, format);
        }
        rectangleF.X += width;
      }
      graphics.Save();
      graphics.TranslateTransform(tempBounds.X, tempBounds.Y);
      graphics.RotateTransform((float) -this.GetRotationAngle());
      RectangleF renderingRect = this.GetRenderingRect(tempBounds);
      graphics.DrawPdfTemplate(template, renderingRect.Location);
      graphics.Restore();
    }
    else
    {
      if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
        this.Border.DrawBorder(graphics, bounds2);
      if (this.Text == null || !(this.Text != string.Empty))
        return;
      if (this.m_isExData)
        this.Text = this.m_altText;
      if (this.Type == PdfXfaTextBoxType.Password)
      {
        int length = this.Text.Length;
        if (length > 0)
        {
          string empty = string.Empty;
          if (this.m_passwordChar != char.MinValue)
          {
            for (int index = 0; index < length; ++index)
              empty += (string) (object) this.m_passwordChar;
          }
          else
          {
            for (int index = 0; index < length; ++index)
              empty += (string) (object) '*';
          }
          this.Text = empty;
        }
      }
      if (this.Rotate == PdfXfaRotateAngle.RotateAngle0)
      {
        graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), this.GetBounds(bounds2), format);
      }
      else
      {
        graphics.Save();
        RectangleF bounds3 = this.GetBounds(bounds2);
        graphics.TranslateTransform(bounds3.X, bounds3.Y);
        graphics.RotateTransform((float) -this.GetRotationAngle());
        RectangleF renderingRect = this.GetRenderingRect(bounds3);
        graphics.DrawString(this.Text, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), renderingRect, format);
        graphics.Restore();
      }
    }
  }

  private RectangleF GetBounds(RectangleF bounds1)
  {
    if (this.m_innerMargin != null)
    {
      bounds1.X += this.m_innerMargin.Left;
      bounds1.Width -= this.m_innerMargin.Left + this.m_innerMargin.Right;
      if (!this.isSkipDefaultMargin)
      {
        bounds1.Y += this.m_innerMargin.Top;
        bounds1.Height -= this.m_innerMargin.Top + this.m_innerMargin.Bottom;
      }
    }
    return bounds1;
  }

  internal new void Save()
  {
    base.Save();
    if ((double) this.MinimumHeight > 0.0)
      this.SetSize(this.currentNode, "minH", this.MinimumHeight);
    if ((double) this.MinimumWidth > 0.0)
      this.SetSize(this.currentNode, "minW", this.MinimumWidth);
    if ((double) this.MaximumHeight > 0.0)
      this.SetSize(this.currentNode, "maxH", this.MaximumHeight);
    if ((double) this.MaximumWidth > 0.0)
      this.SetSize(this.currentNode, "maxW", this.MaximumWidth);
    if (this.Text != null || this.MaximumLength > 0)
    {
      if (this.currentNode["value"] != null)
      {
        if (this.currentNode["value"]["text"] != null)
        {
          if (this.MaximumLength > 0)
          {
            XmlNode node = (XmlNode) this.currentNode["value"]["text"];
            if (node.Attributes["maxChars"] != null)
              node.Attributes["maxChars"].InnerText = this.MaximumLength.ToString();
            else
              this.SetNewAttribute(node, "maxChars", this.MaximumLength.ToString());
          }
        }
        else
        {
          XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
          if (this.MaximumLength > 0)
            this.SetNewAttribute(node, "maxChars", this.MaximumLength.ToString());
          this.currentNode["value"].AppendChild(node);
        }
      }
      else
      {
        XmlNode node1 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "value", "");
        XmlNode node2 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, "text", "");
        if (this.Text != null)
          node2.InnerText = this.Text;
        if (this.MaximumLength > 0)
          this.SetNewAttribute(node2, "maxChars", this.MaximumLength.ToString());
        node1.AppendChild(node2);
        this.currentNode.AppendChild(node1);
      }
    }
    if (this.PasswordCharacter != char.MinValue && this.Type == PdfXfaTextBoxType.Password && this.currentNode["ui"]["passwordEdit"] != null)
    {
      XmlNode node = (XmlNode) this.currentNode["ui"]["passwordEdit"];
      if (node.Attributes["passwordChar"] != null)
        node.Attributes["passwordChar"].Value = this.PasswordCharacter.ToString();
      else
        this.SetNewAttribute(node, "passwordChar", this.PasswordCharacter.ToString());
    }
    if (this.CombLength > 0 && this.Type == PdfXfaTextBoxType.Comb && this.currentNode["ui"]["textEdit"] != null)
    {
      XmlNode node3 = (XmlNode) this.currentNode["ui"]["textEdit"];
      if (node3.Attributes["hScrollPolicy"] != null)
        node3.Attributes["hScrollPolicy"].Value = "off";
      else
        this.SetNewAttribute(node3, "hScrollPolicy", "off");
      if (node3["comb"] != null)
      {
        if (this.CombLength <= 0)
          this.currentNode["ui"]["textEdit"].RemoveChild((XmlNode) node3["comb"]);
        else if (node3["comb"].Attributes["numberOfCells"] != null)
          node3["comb"].Attributes["numberOfCells"].Value = this.CombLength.ToString();
        else
          this.SetNewAttribute((XmlNode) node3["comb"], "numberOfCells", this.CombLength.ToString());
      }
      else if (this.CombLength > 0)
      {
        XmlNode node4 = node3.OwnerDocument.CreateNode(XmlNodeType.Element, "comb", "");
        this.SetNewAttribute(node4, "numberOfCells", this.CombLength.ToString());
        node3.AppendChild(node4);
      }
    }
    if (this.currentNode["ui"]["textEdit"] != null)
    {
      XmlNode node = (XmlNode) this.currentNode["ui"]["textEdit"];
      if (this.Type == PdfXfaTextBoxType.Multiline)
      {
        if (node.Attributes["multiLine"] != null)
          node.Attributes["multiLine"].Value = "1";
        else
          this.SetNewAttribute(node, "multiLine", "1");
      }
    }
    if (this.Border == null)
      return;
    if (this.m_passwordEdit)
    {
      if (this.currentNode["ui"]["passwordEdit"]["border"] != null)
        this.Border.Save((XmlNode) this.currentNode["ui"]["passwordEdit"]["border"]);
      else if (this.currentNode["border"] != null)
      {
        this.Border.Save((XmlNode) this.currentNode["border"]);
      }
      else
      {
        XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
        this.Border.Save(node);
        this.currentNode["ui"]["passwordEdit"].AppendChild(node);
      }
    }
    else if (this.currentNode["ui"]["textEdit"]["border"] != null)
      this.Border.Save((XmlNode) this.currentNode["ui"]["textEdit"]["border"]);
    else if (this.currentNode["border"] != null)
    {
      this.Border.Save((XmlNode) this.currentNode["border"]);
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
      this.Border.Save(node);
      this.currentNode["ui"]["textEdit"].AppendChild(node);
    }
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    if (form.acroForm.GetField(this.nodeName) is PdfLoadedTextBoxField field && this.Text != null)
      field.Text = this.Text;
    if (string.IsNullOrEmpty(this.Name))
      return;
    dataSetWriter.WriteStartElement(this.Name);
    if (this.Text != null)
      dataSetWriter.WriteString(this.Text);
    this.Save();
    dataSetWriter.WriteEndElement();
  }
}
