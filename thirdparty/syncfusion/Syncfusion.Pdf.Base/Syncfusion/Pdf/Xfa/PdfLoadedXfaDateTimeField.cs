// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaDateTimeField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaDateTimeField : PdfLoadedXfaStyledField
{
  internal DateTime? m_value;
  internal string fieldName = string.Empty;
  private PdfXfaDateTimeFormat m_format;
  internal string patternString = string.Empty;
  internal bool m_isSet;
  private string m_pattern = string.Empty;
  internal string m_bindedvalue = string.Empty;
  private PdfMargins m_innerMargin;
  private bool isSkipDefaultMargin;

  public DateTime Value
  {
    get => this.m_value ?? DateTime.MinValue;
    set
    {
      this.m_value = new DateTime?(value);
      this.m_isSet = true;
    }
  }

  internal void SetDate(DateTime? value)
  {
    this.m_value = value;
    this.m_isSet = true;
  }

  public PdfXfaDateTimeFormat Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public string Pattern
  {
    get
    {
      if (string.IsNullOrEmpty(this.m_pattern) && !string.IsNullOrEmpty(this.patternString))
        this.GetFieldPattern();
      return this.m_pattern;
    }
    set => this.m_pattern = value;
  }

  private void GetFieldPattern()
  {
    XfaWriter xfaWriter = new XfaWriter();
    string pattern1 = this.TrimDatePattern(this.patternString);
    string pattern2 = this.TrimTimePattern(this.patternString);
    switch (this.Format)
    {
      case PdfXfaDateTimeFormat.Date:
        this.m_pattern = xfaWriter.GetDatePattern(pattern1);
        break;
      case PdfXfaDateTimeFormat.Time:
        this.m_pattern = xfaWriter.GetTimePattern(pattern2);
        break;
      case PdfXfaDateTimeFormat.DateTime:
        this.m_pattern = $"{xfaWriter.GetDatePattern(pattern1)} {xfaWriter.GetTimePattern(pattern2)}";
        break;
    }
  }

  public void ClearValue()
  {
    this.m_value = new DateTime?();
    this.m_isSet = true;
  }

  internal void Read(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    if (node.Name == "field")
    {
      this.ReadCommonProperties(this.currentNode);
      if (node["value"] != null)
      {
        if (node["value"]["date"] != null)
        {
          this.Format = PdfXfaDateTimeFormat.Date;
          string innerText = node["value"]["date"].InnerText;
          if (!string.IsNullOrEmpty(innerText))
          {
            DateTime? date = this.ParseDate(innerText);
            if (date.HasValue)
              this.Value = date.Value;
          }
        }
        else if (node["value"]["dateTime"] != null)
        {
          this.Format = PdfXfaDateTimeFormat.DateTime;
          string innerText = node["value"]["dateTime"].InnerText;
          if (!string.IsNullOrEmpty(innerText))
          {
            DateTime? date = this.ParseDate(innerText);
            if (date.HasValue)
              this.Value = date.Value;
          }
        }
        else if (node["value"]["time"] != null)
        {
          this.Format = PdfXfaDateTimeFormat.Time;
          string innerText = node["value"]["time"].InnerText;
          if (!string.IsNullOrEmpty(innerText))
          {
            DateTime? date = this.ParseDate(innerText);
            if (date.HasValue)
              this.Value = date.Value;
          }
        }
      }
    }
    if (node["format"] != null && node["format"]["picture"] != null)
      this.patternString = node["format"]["picture"].InnerText;
    if (node["ui"]["dateTimeEdit"]["border"] != null)
      this.ReadBorder((XmlNode) node["ui"]["dateTimeEdit"]["border"], false);
    if (node["ui"]["dateTimeEdit"]["margin"] != null)
    {
      this.m_innerMargin = new PdfMargins();
      this.ReadMargin((XmlNode) node["ui"]["dateTimeEdit"]["margin"], this.m_innerMargin);
    }
    if (this.patternString == string.Empty)
    {
      if (this.patternString == "")
      {
        if (node["ui"]["dateTimeEdit"]["picture"] != null)
          this.patternString = node["ui"]["dateTimeEdit"]["picture"].InnerText;
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
              try
              {
                DateTimeFormatInfo provider = new DateTimeFormatInfo();
                provider.ShortDatePattern = this.GetPattern(this.Pattern);
                DateTime result = new DateTime();
                this.Value = !DateTime.TryParse(xmlNode1.FirstChild.InnerText, (IFormatProvider) provider, DateTimeStyles.None, out result) ? result : Convert.ToDateTime(xmlNode1.FirstChild.InnerText, (IFormatProvider) provider);
              }
              catch (Exception ex)
              {
                this.m_bindedvalue = xmlNode1.FirstChild.InnerText;
              }
            }
          }
          else if (this.parent.bindingName != null)
          {
            if (!flag1)
            {
              string xpath2 = "//" + $"{this.parent.nodeName.Split(chArray)[0].Replace("[0]", "")}/{$"{this.parent.bindingName}.{this.Name}".Replace(".", "/").Replace("$.", "")}";
              XmlNode xmlNode2 = dataSetDoc.SelectSingleNode(xpath2);
              if (xmlNode2 != null)
              {
                if (xmlNode2.FirstChild != null)
                {
                  try
                  {
                    DateTimeFormatInfo provider = new DateTimeFormatInfo();
                    provider.ShortDatePattern = this.GetPattern(this.Pattern);
                    DateTime result = new DateTime();
                    this.Value = !DateTime.TryParse(xmlNode2.FirstChild.InnerText, (IFormatProvider) provider, DateTimeStyles.None, out result) ? result : Convert.ToDateTime(xmlNode2.FirstChild.InnerText, (IFormatProvider) provider);
                  }
                  catch (Exception ex)
                  {
                    this.m_bindedvalue = xmlNode2.FirstChild.InnerText;
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
    if (xmlNode3 == null || xmlNode3.FirstChild == null)
      return;
    DateTime? date1 = this.ParseDate(xmlNode3.FirstChild.InnerText);
    if (!date1.HasValue)
      return;
    this.Value = date1.Value;
  }

  private DateTime? ParseDate(string text)
  {
    if (!(text != string.Empty))
      return new DateTime?();
    DateTime result;
    try
    {
      DateTimeFormatInfo provider = new DateTimeFormatInfo();
      provider.ShortDatePattern = this.GetPattern(this.Pattern);
      if (!string.IsNullOrEmpty(this.Pattern))
      {
        if (DateTime.TryParse(text, (IFormatProvider) provider, DateTimeStyles.None, out result))
          result = Convert.ToDateTime(text, (IFormatProvider) provider);
      }
      else if (DateTime.TryParse(text, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        return new DateTime?(result);
    }
    catch
    {
      if (DateTime.TryParse(text, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        return new DateTime?(result);
    }
    return new DateTime?(result);
  }

  internal new void Save()
  {
    base.Save();
    string str = this.Format.ToString().ToLower();
    if (this.Format == PdfXfaDateTimeFormat.DateTime)
      str = "dateTime";
    if (this.currentNode["value"] != null)
    {
      if (this.currentNode["value"]["date"] != null && str != "date")
      {
        this.currentNode["value"].RemoveChild((XmlNode) this.currentNode["value"]["date"]);
        this.setFormat(str);
      }
      if (this.currentNode["value"]["dateTime"] != null && str != "dateTime")
      {
        this.currentNode["value"].RemoveChild((XmlNode) this.currentNode["value"]["dateTime"]);
        this.setFormat(str);
      }
      if (this.currentNode["value"]["time"] != null && str != "time")
      {
        this.currentNode["value"].RemoveChild((XmlNode) this.currentNode["value"]["time"]);
        this.setFormat(str);
      }
    }
    else
    {
      XmlNode node1 = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "value", "");
      XmlNode node2 = node1.OwnerDocument.CreateNode(XmlNodeType.Element, str, "");
      node1.AppendChild(node2);
      this.currentNode.AppendChild(node1);
    }
    if (this.Border == null)
      return;
    if (this.currentNode["ui"]["dateTimeEdit"]["border"] != null)
      this.Border.Save((XmlNode) this.currentNode["ui"]["dateTimeEdit"]["border"]);
    else if (this.currentNode["border"] != null)
    {
      this.Border.Save((XmlNode) this.currentNode["border"]);
    }
    else
    {
      XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "border", "");
      this.Border.Save(node);
      this.currentNode["ui"]["dateTimeEdit"].AppendChild(node);
    }
  }

  private void setFormat(string formatText)
  {
    XmlNode node = this.currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, formatText, "");
    node.Value = this.m_value.ToString();
    this.currentNode["value"].AppendChild(node);
  }

  private string GetDate()
  {
    string empty = string.Empty;
    XfaWriter xfaWriter = new XfaWriter();
    switch (this.Format)
    {
      case PdfXfaDateTimeFormat.Date:
        string format1 = xfaWriter.GetDatePattern(this.patternString);
        if (!string.IsNullOrEmpty(this.Pattern))
          format1 = this.GetPattern(this.Pattern);
        empty = this.m_value.Value.ToString(format1, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case PdfXfaDateTimeFormat.Time:
        string format2 = xfaWriter.GetTimePattern(this.patternString);
        if (!string.IsNullOrEmpty(this.Pattern))
          format2 = this.GetPattern(this.Pattern);
        empty = this.m_value.Value.ToString(format2, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case PdfXfaDateTimeFormat.DateTime:
        empty = this.m_value.Value.Date.ToString();
        if (!string.IsNullOrEmpty(this.Pattern))
        {
          empty = this.m_value.Value.ToString(this.GetPattern(this.Pattern), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
        break;
    }
    return empty;
  }

  internal void DrawField(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else if ((double) this.Font.Height < 1.0)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
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
      if ((double) bounds1.Height < (double) this.Caption.Font.Height && (double) bounds1.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) >= (double) this.Caption.Font.Height)
        bounds1.Height = this.Caption.Font.Height;
      this.Caption.DrawText(graphics, bounds1, this.GetRotationAngle());
    }
    RectangleF bounds2 = this.GetBounds(bounds1, this.Rotate, this.Caption);
    if (this.ForeColor.IsEmpty)
      this.ForeColor = (PdfColor) Color.Black;
    if (this.Visibility == PdfXfaVisibility.Invisible || this.Visibility == PdfXfaVisibility.Hidden)
      return;
    if (this.Border != null && this.Border.Visibility != PdfXfaVisibility.Hidden)
      this.Border.DrawBorder(graphics, bounds2);
    string s = string.Empty;
    if (this.m_isSet)
      s = this.GetDate();
    else if (this.m_bindedvalue != string.Empty)
      s = this.m_bindedvalue;
    if (!(s != string.Empty))
      return;
    if ((double) bounds2.Height < (double) this.Font.Height && (double) bounds2.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) >= (double) this.Font.Height)
    {
      bounds2.Height = this.Font.Height;
      this.isSkipDefaultMargin = true;
    }
    if (this.Rotate == PdfXfaRotateAngle.RotateAngle0)
    {
      graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), this.isSkipDefaultMargin ? bounds2 : this.GetBounds(bounds2), format);
    }
    else
    {
      graphics.Save();
      if (!this.isSkipDefaultMargin)
        bounds2 = this.GetBounds(bounds2);
      graphics.TranslateTransform(bounds2.X, bounds2.Y);
      graphics.RotateTransform((float) -this.GetRotationAngle());
      RectangleF renderingRect = this.GetRenderingRect(bounds2);
      graphics.DrawString(s, this.Font, (PdfBrush) new PdfSolidBrush(this.ForeColor), renderingRect, format);
      graphics.Restore();
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

  internal SizeF GetFieldSize()
  {
    float num1 = 0.0f;
    if ((double) this.Width <= 0.0)
    {
      if (this.currentNode.Attributes["maxW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["maxW"].Value);
      if (this.currentNode.Attributes["minW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["minW"].Value);
    }
    if ((double) this.Height <= 0.0)
    {
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (this.currentNode.Attributes["minH"] != null)
        num2 = this.ConvertToPoint(this.currentNode.Attributes["minH"].Value);
      if (this.currentNode.Attributes["maxH"] != null)
        num3 = this.ConvertToPoint(this.currentNode.Attributes["maxH"].Value);
      if ((double) num2 > 0.0)
      {
        num1 = num2;
        if ((double) num3 > 0.0)
        {
          this.Height = num3;
        }
        else
        {
          if ((double) num1 < (double) num2)
            num1 = num2;
          if (this.Font != null && (double) num1 < (double) this.Font.Height)
            num1 = this.Font.Height + this.Margins.Top + this.Margins.Bottom;
        }
      }
      else if ((double) num3 > 0.0)
        num1 = num3;
    }
    else
      num1 = this.Height;
    if ((double) this.Height < (double) num1)
      this.Height = num1;
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Height, this.Width) : new SizeF(this.Width, this.Height);
  }

  internal void Fill(XmlWriter dataSetWriter, PdfLoadedXfaForm form)
  {
    PdfLoadedTextBoxField field = form.acroForm.GetField(this.nodeName) as PdfLoadedTextBoxField;
    string empty = string.Empty;
    if (!string.IsNullOrEmpty(this.Name))
      dataSetWriter.WriteStartElement(this.Name);
    if (this.m_isSet)
    {
      if (this.m_value.HasValue)
      {
        XfaWriter xfaWriter = new XfaWriter();
        switch (this.Format)
        {
          case PdfXfaDateTimeFormat.Date:
            string format1 = xfaWriter.GetDatePattern(this.patternString);
            if (!string.IsNullOrEmpty(this.Pattern))
              format1 = this.GetPattern(this.Pattern);
            empty = this.m_value.Value.ToString(format1, (IFormatProvider) CultureInfo.InvariantCulture);
            dataSetWriter.WriteString(empty);
            break;
          case PdfXfaDateTimeFormat.Time:
            string format2 = xfaWriter.GetTimePattern(this.patternString);
            if (!string.IsNullOrEmpty(this.Pattern))
              format2 = this.GetPattern(this.Pattern);
            empty = this.m_value.Value.ToString(format2, (IFormatProvider) CultureInfo.InvariantCulture);
            dataSetWriter.WriteString(empty);
            break;
          case PdfXfaDateTimeFormat.DateTime:
            empty = this.m_value.Value.Date.ToString();
            if (!string.IsNullOrEmpty(this.Pattern))
              empty = this.m_value.Value.ToString(this.GetPattern(this.Pattern), (IFormatProvider) CultureInfo.InvariantCulture);
            dataSetWriter.WriteString(empty);
            break;
        }
      }
      this.Save();
      if (field != null)
        field.Text = empty;
    }
    if (string.IsNullOrEmpty(this.Name))
      return;
    dataSetWriter.WriteEndElement();
  }
}
