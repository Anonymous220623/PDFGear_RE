// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaNumericField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaNumericField : PdfLoadedXfaStyledField
{
  private double m_numericValue = double.NaN;
  internal string fullName = string.Empty;
  private PdfXfaNumericType m_fieldType;
  private int m_combLength;
  private string m_patternString = string.Empty;
  private PdfMargins m_innerMargin;
  private bool isSkipDefaultMargin;

  public double NumericValue
  {
    get => this.m_numericValue;
    set => this.m_numericValue = value;
  }

  public int CombLenght
  {
    get => this.m_combLength;
    set
    {
      if (value < 0)
        return;
      this.m_combLength = value;
    }
  }

  public PdfXfaNumericType FieldType
  {
    get => this.m_fieldType;
    internal set => this.m_fieldType = value;
  }

  public string PatternString => this.m_patternString;

  internal void Read(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (!(node.Name == "field"))
      return;
    this.ReadCommonProperties(this.currentNode);
    if (node["value"] != null)
    {
      float result = 0.0f;
      if (node["value"]["float"] != null)
      {
        if (node["value"]["float"].Value != null)
          float.TryParse(node["value"]["float"].Value, out result);
        else if (!string.IsNullOrEmpty(node["value"]["float"].InnerText))
          float.TryParse(node["value"]["float"].InnerText, out result);
        this.m_fieldType = PdfXfaNumericType.Float;
      }
      else if (node["value"]["decimal"] != null)
      {
        if (node["value"]["decimal"].Value != null)
          float.TryParse(node["value"]["decimal"].Value, out result);
        else if (!string.IsNullOrEmpty(node["value"]["decimal"].InnerText))
          float.TryParse(node["value"]["decimal"].InnerText, out result);
        this.m_fieldType = PdfXfaNumericType.Decimal;
      }
      else if (node["value"]["integer"] != null)
      {
        if (node["value"]["integer"].Value != null)
          float.TryParse(node["value"]["integer"].Value, out result);
        else if (!string.IsNullOrEmpty(node["value"]["integer"].InnerText))
          float.TryParse(node["value"]["integer"].InnerText, out result);
        this.m_fieldType = PdfXfaNumericType.Integer;
      }
      this.NumericValue = (double) result;
    }
    if (node["format"] != null && node["format"]["picture"] != null)
    {
      this.m_patternString = node["format"]["picture"].InnerText;
      if (this.m_patternString.Contains("$") || this.m_patternString.Contains("num.currency"))
        this.m_fieldType = PdfXfaNumericType.Currency;
      else if (this.m_patternString.Contains("%") || this.m_patternString.Contains("num.percent"))
        this.m_fieldType = PdfXfaNumericType.Percent;
      else if (this.m_patternString.Contains("num.integer"))
        this.m_fieldType = PdfXfaNumericType.Integer;
      else if (this.m_patternString.Contains("num.decimal"))
        this.m_fieldType = PdfXfaNumericType.Decimal;
    }
    if (node["ui"]["numericEdit"] != null)
    {
      if (node["ui"]["numericEdit"]["comb"] != null)
      {
        XmlNode xmlNode = (XmlNode) node["ui"]["numericEdit"]["comb"];
        if (xmlNode.Attributes["numberOfCells"] != null)
        {
          string s = xmlNode.Attributes["numberOfCells"].Value;
          if (!string.IsNullOrEmpty(s))
          {
            int result = 0;
            int.TryParse(s, out result);
            this.m_combLength = result;
          }
        }
      }
      if (node["ui"]["numericEdit"]["border"] != null)
        this.ReadBorder((XmlNode) node["ui"]["numericEdit"]["border"], false);
      if (node["ui"]["numericEdit"]["margin"] != null)
      {
        this.m_innerMargin = new PdfMargins();
        this.ReadMargin((XmlNode) node["ui"]["numericEdit"]["margin"], this.m_innerMargin);
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
          double result = 0.0;
          XmlNode xmlNode1 = dataSetDoc.SelectSingleNode(xpath1);
          if (xmlNode1 != null)
          {
            if (xmlNode1.FirstChild != null)
            {
              if (!string.IsNullOrEmpty(Regex.Replace(xmlNode1.FirstChild.InnerText, "[^0-9.]", "")))
              {
                double.TryParse(xmlNode1.FirstChild.InnerText, out result);
                this.NumericValue = result;
              }
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
                {
                  if (!string.IsNullOrEmpty(Regex.Replace(xmlNode2.FirstChild.InnerText, "[^0-9.]", "")))
                  {
                    double.TryParse(xmlNode2.FirstChild.InnerText, out result);
                    this.NumericValue = result;
                  }
                }
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
    if (xmlNode3 == null || xmlNode3.FirstChild == null || !(xmlNode3.FirstChild.InnerText != string.Empty))
      return;
    double result1 = 0.0;
    double.TryParse(xmlNode3.FirstChild.InnerText, out result1);
    this.NumericValue = result1;
  }

  internal void DrawField(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    SizeF size = this.GetSize();
    PdfStringFormat format1 = new PdfStringFormat();
    format1.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    format1.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    if ((double) bounds1.Height <= 0.0)
      return;
    bool flag = false;
    if (this.Border == null && this.parent is PdfLoadedXfaForm && (this.parent as PdfLoadedXfaForm).FlowDirection == PdfLoadedXfaFlowDirection.Row)
      flag = true;
    if (this.CompleteBorder != null && this.CompleteBorder.Visibility != PdfXfaVisibility.Hidden && this.CompleteBorder.Visibility != PdfXfaVisibility.Invisible && !flag)
      this.CompleteBorder.DrawBorder(graphics, bounds);
    if (this.Visibility != PdfXfaVisibility.Hidden && this.Caption != null)
    {
      if (this.Caption.Font == null)
        this.Caption.Font = this.Font;
      if ((double) bounds1.Height < (double) this.Caption.Font.Height && (double) bounds1.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) >= (double) this.Caption.Font.Height)
        bounds1.Height = this.Caption.Font.Height;
      this.Caption.DrawText(graphics, bounds1, this.GetRotationAngle());
    }
    RectangleF bounds2 = this.GetBounds(bounds1, this.Rotate, this.Caption);
    if (this.Visibility == PdfXfaVisibility.Hidden)
      return;
    if (this.ForeColor.IsEmpty)
      this.ForeColor = (PdfColor) Color.Black;
    string format2 = string.Empty;
    if (!double.IsNaN(this.NumericValue))
    {
      if ((double) bounds2.Height < (double) this.Font.Height && (double) bounds2.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) >= (double) this.Font.Height)
      {
        bounds2.Height = this.Font.Height;
        this.isSkipDefaultMargin = true;
      }
      if (this.PatternString != string.Empty)
      {
        string[] strArray = this.PatternString.Split('.');
        if (strArray.Length > 1)
        {
          format2 = "0.";
          foreach (int num in strArray[1])
            format2 += "0";
        }
      }
    }
    string s = string.Empty;
    if (!double.IsNaN(this.NumericValue))
      s = format2 != string.Empty ? this.NumericValue.ToString(format2) : this.NumericValue.ToString();
    if (this.CombLenght > 0)
    {
      RectangleF tempBounds = bounds2;
      bounds2.Location = PointF.Empty;
      PdfTemplate template = new PdfTemplate(bounds2.Size);
      float width = bounds2.Width / (float) this.CombLenght;
      PdfPen pen = (PdfPen) null;
      PdfBrush brush = (PdfBrush) null;
      if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
      {
        pen = this.Border.GetFlattenPen();
        brush = this.Border.GetBrush(bounds);
      }
      char[] chArray = (char[]) null;
      if (s != null && s != string.Empty)
        chArray = s.ToCharArray();
      RectangleF rectangleF = new RectangleF(bounds2.X, bounds2.Y, width, bounds2.Height);
      for (int index = 0; index < this.CombLenght; ++index)
      {
        if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
          template.Graphics.DrawRectangle(pen, brush, rectangleF);
        if (chArray != null && chArray.Length > index)
        {
          format1.Alignment = PdfTextAlignment.Center;
          template.Graphics.DrawString(chArray[index].ToString(), this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), rectangleF, format1);
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
      if (this.Rotate == PdfXfaRotateAngle.RotateAngle0)
      {
        graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), this.isSkipDefaultMargin ? bounds2 : this.GetBounds(bounds2), format1);
      }
      else
      {
        graphics.Save();
        if (!this.isSkipDefaultMargin)
          bounds2 = this.GetBounds(bounds2);
        graphics.TranslateTransform(bounds2.X, bounds2.Y);
        graphics.RotateTransform((float) -this.GetRotationAngle());
        RectangleF renderingRect = this.GetRenderingRect(bounds2);
        graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), renderingRect, format1);
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

  internal SizeF GetNumericFieldSize()
  {
    if ((double) this.Height <= 0.0)
    {
      if (this.currentNode.Attributes["maxH"] != null)
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["maxH"].Value);
      if (this.currentNode.Attributes["minH"] != null)
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["minH"].Value);
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

  internal new void Save()
  {
    base.Save();
    if (this.currentNode["value"] != null)
    {
      if (this.currentNode[this.FieldType.ToString().ToLower()] == null)
        this.currentNode["value"].AppendChild(this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, this.FieldType.ToString().ToLower(), ""));
    }
    else
    {
      XmlNode node1 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "value", "");
      XmlNode node2 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, this.FieldType.ToString().ToLower(), "");
      node1.AppendChild(node2);
      this.currentNode.AppendChild(node1);
    }
    if (this.currentNode["ui"]["numericEdit"] != null)
    {
      if (this.currentNode["ui"]["numericEdit"]["comb"] != null)
      {
        XmlNode xmlNode = (XmlNode) this.currentNode["ui"]["numericEdit"]["comb"];
        if (this.CombLenght <= 0)
          this.currentNode["ui"]["numericEdit"].RemoveChild(xmlNode);
        else if (xmlNode.Attributes["numberOfCells"] != null)
          xmlNode.Attributes["numberOfCells"].Value = this.CombLenght.ToString();
        else
          this.SetNewAttribute(xmlNode, "numberOfCells", this.CombLenght.ToString());
      }
      else if (this.CombLenght > 0)
      {
        XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "comb", "");
        this.SetNewAttribute(node, "numberOfCells", this.CombLenght.ToString());
        this.currentNode["ui"]["numericEdit"].AppendChild(node);
      }
    }
    if (this.Border == null)
      return;
    if (this.currentNode["ui"]["numericEdit"]["border"] != null)
      this.Border.Save((XmlNode) this.currentNode["ui"]["numericEdit"]["border"]);
    else if (this.currentNode["border"] != null)
    {
      this.Border.Save((XmlNode) this.currentNode["border"]);
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
      this.Border.Save(node);
      this.currentNode["ui"]["numericEdit"].AppendChild(node);
    }
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    if (form.acroForm.GetField(this.nodeName) is PdfLoadedTextBoxField field && !double.IsNaN(this.NumericValue))
    {
      string str = this.NumericValue.ToString();
      field.Text = str;
    }
    if (string.IsNullOrEmpty(this.Name))
      return;
    dataSetWriter.WriteStartElement(this.Name);
    if (!double.IsNaN(this.NumericValue))
      dataSetWriter.WriteString(this.NumericValue.ToString());
    this.Save();
    dataSetWriter.WriteEndElement();
  }
}
