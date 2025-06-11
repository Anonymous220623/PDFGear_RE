// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.HtmlToXaml.HtmlFromXamlConverter
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;

internal static class HtmlFromXamlConverter
{
  internal static string ConvertXamlToHtml(string xamlString)
  {
    XmlTextReader xamlReader = new XmlTextReader((TextReader) new StringReader(xamlString));
    StringBuilder sb = new StringBuilder(100);
    XmlTextWriter htmlWriter = (XmlTextWriter) new HtmlEncodedTextWriter((TextWriter) new StringWriter(sb));
    return !HtmlFromXamlConverter.WriteFlowDocument(xamlReader, htmlWriter) ? "" : sb.ToString();
  }

  private static bool WriteFlowDocument(XmlTextReader xamlReader, XmlTextWriter htmlWriter)
  {
    if (!HtmlFromXamlConverter.ReadNextToken((XmlReader) xamlReader) || xamlReader.NodeType != XmlNodeType.Element || xamlReader.Name != "FlowDocument")
      return false;
    StringBuilder inlineStyle = new StringBuilder();
    htmlWriter.WriteStartElement("html");
    htmlWriter.WriteStartElement("body");
    HtmlFromXamlConverter.WriteFormattingProperties(xamlReader, htmlWriter, inlineStyle);
    HtmlFromXamlConverter.WriteElementContent(xamlReader, htmlWriter, inlineStyle);
    htmlWriter.WriteEndElement();
    htmlWriter.WriteEndElement();
    return true;
  }

  private static void WriteFormattingProperties(
    XmlTextReader xamlReader,
    XmlTextWriter htmlWriter,
    StringBuilder inlineStyle)
  {
    inlineStyle.Remove(0, inlineStyle.Length);
    if (!xamlReader.HasAttributes)
      return;
    bool flag = false;
    while (xamlReader.MoveToNextAttribute())
    {
      string str = (string) null;
      switch (xamlReader.Name)
      {
        case "Background":
          str = $"background-color:{HtmlFromXamlConverter.ParseXamlColor(xamlReader.Value)};";
          break;
        case "BorderBrush":
          str = $"border-color:{HtmlFromXamlConverter.ParseXamlColor(xamlReader.Value)};";
          flag = true;
          break;
        case "BorderThickness":
          str = $"border-width:{HtmlFromXamlConverter.ParseXamlThickness(xamlReader.Value)};";
          flag = true;
          break;
        case "ColumnSpan":
          htmlWriter.WriteAttributeString("colspan", xamlReader.Value);
          break;
        case "FontFamily":
          str = $"font-family:{xamlReader.Value};";
          break;
        case "FontSize":
          str = $"font-size:{xamlReader.Value};";
          break;
        case "FontStyle":
          str = $"font-style:{xamlReader.Value.ToLower()};";
          break;
        case "FontWeight":
          str = $"font-weight:{xamlReader.Value.ToLower()};";
          break;
        case "Foreground":
          str = $"color:{HtmlFromXamlConverter.ParseXamlColor(xamlReader.Value)};";
          break;
        case "Margin":
          str = $"margin:{HtmlFromXamlConverter.ParseXamlThickness(xamlReader.Value)};";
          break;
        case "Padding":
          str = $"padding:{HtmlFromXamlConverter.ParseXamlThickness(xamlReader.Value)};";
          break;
        case "RowSpan":
          htmlWriter.WriteAttributeString("rowspan", xamlReader.Value);
          break;
        case "TextAlignment":
          str = $"text-align:{xamlReader.Value};";
          break;
        case "TextDecorations":
          str = "text-decoration:underline;";
          break;
        case "TextIndent":
          str = $"text-indent:{xamlReader.Value};";
          break;
        case "Width":
          str = $"width:{xamlReader.Value};";
          break;
      }
      if (str != null)
        inlineStyle.Append(str);
    }
    if (flag)
      inlineStyle.Append("border-style:solid;mso-element:para-border-div;");
    xamlReader.MoveToElement();
  }

  private static string ParseXamlColor(string color)
  {
    if (color.StartsWith("#"))
      color = "#" + color.Substring(3);
    return color;
  }

  private static string ParseXamlThickness(string thickness)
  {
    string[] strArray = thickness.Split(',');
    for (int index = 0; index < strArray.Length; ++index)
    {
      double result;
      strArray[index] = !double.TryParse(strArray[index], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? "1" : Math.Ceiling(result).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
    string xamlThickness;
    switch (strArray.Length)
    {
      case 1:
        xamlThickness = thickness;
        break;
      case 2:
        xamlThickness = $"{strArray[1]} {strArray[0]}";
        break;
      case 4:
        xamlThickness = $"{strArray[1]} {strArray[2]} {strArray[3]} {strArray[0]}";
        break;
      default:
        xamlThickness = strArray[0];
        break;
    }
    return xamlThickness;
  }

  private static void WriteElementContent(
    XmlTextReader xamlReader,
    XmlTextWriter htmlWriter,
    StringBuilder inlineStyle)
  {
    bool flag = false;
    if (xamlReader.IsEmptyElement)
    {
      if (htmlWriter != null && !flag && inlineStyle.Length > 0)
      {
        htmlWriter.WriteAttributeString("STYLE", inlineStyle.ToString());
        inlineStyle.Remove(0, inlineStyle.Length);
      }
    }
    else
    {
      while (HtmlFromXamlConverter.ReadNextToken((XmlReader) xamlReader) && xamlReader.NodeType != XmlNodeType.EndElement)
      {
        switch (xamlReader.NodeType)
        {
          case XmlNodeType.Element:
            if (xamlReader.Name.Contains("."))
            {
              HtmlFromXamlConverter.AddComplexProperty(xamlReader, inlineStyle);
              break;
            }
            if (htmlWriter != null && !flag && inlineStyle.Length > 0)
            {
              htmlWriter.WriteAttributeString("style", inlineStyle.ToString());
              inlineStyle.Remove(0, inlineStyle.Length);
            }
            flag = true;
            HtmlFromXamlConverter.WriteElement(xamlReader, htmlWriter, inlineStyle);
            break;
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.SignificantWhitespace:
            if (htmlWriter != null)
            {
              if (!flag && inlineStyle.Length > 0)
                htmlWriter.WriteAttributeString("style", inlineStyle.ToString());
              htmlWriter.WriteString(xamlReader.Value.Replace(" ", " "));
            }
            flag = true;
            break;
          case XmlNodeType.Comment:
            if (htmlWriter != null)
            {
              if (!flag && inlineStyle.Length > 0)
                htmlWriter.WriteAttributeString("style", inlineStyle.ToString());
              htmlWriter.WriteComment(xamlReader.Value);
            }
            flag = true;
            break;
        }
      }
    }
  }

  private static void AddComplexProperty(XmlTextReader xamlReader, StringBuilder inlineStyle)
  {
    if (inlineStyle != null && xamlReader.Name.EndsWith(".TextDecorations"))
      inlineStyle.Append("text-decoration:underline;");
    HtmlFromXamlConverter.WriteElementContent(xamlReader, (XmlTextWriter) null, (StringBuilder) null);
  }

  private static void WriteElement(
    XmlTextReader xamlReader,
    XmlTextWriter htmlWriter,
    StringBuilder inlineStyle)
  {
    if (htmlWriter == null)
    {
      HtmlFromXamlConverter.WriteElementContent(xamlReader, (XmlTextWriter) null, (StringBuilder) null);
    }
    else
    {
      string localName;
      switch (xamlReader.Name)
      {
        case "BlockUIContainer":
          localName = "div";
          break;
        case "Bold":
          localName = "b";
          break;
        case "InlineUIContainer":
          localName = "span";
          break;
        case "Italic":
          localName = "i";
          break;
        case "List":
          string attribute = xamlReader.GetAttribute("MarkerStyle");
          localName = attribute != null && !(attribute == "None") && !(attribute == "Disc") && !(attribute == "Circle") && !(attribute == "Square") && !(attribute == "Box") ? "ol" : "ul";
          break;
        case "ListItem":
          localName = "li";
          break;
        case "Paragraph":
          localName = "p";
          break;
        case "Run":
        case "Span":
          localName = "span";
          break;
        case "Section":
          localName = "div";
          break;
        case "Table":
          localName = "table";
          break;
        case "TableCell":
          localName = "td";
          break;
        case "TableColumn":
          localName = "col";
          break;
        case "TableRow":
          localName = "tr";
          break;
        case "TableRowGroup":
          localName = "tbody";
          break;
        default:
          localName = (string) null;
          break;
      }
      if (htmlWriter != null && localName != null)
      {
        htmlWriter.WriteStartElement(localName);
        HtmlFromXamlConverter.WriteFormattingProperties(xamlReader, htmlWriter, inlineStyle);
        HtmlFromXamlConverter.WriteElementContent(xamlReader, htmlWriter, inlineStyle);
        htmlWriter.WriteEndElement();
      }
      else
        HtmlFromXamlConverter.WriteElementContent(xamlReader, (XmlTextWriter) null, (StringBuilder) null);
    }
  }

  private static bool ReadNextToken(XmlReader xamlReader)
  {
    while (xamlReader.Read())
    {
      switch (xamlReader.NodeType)
      {
        case XmlNodeType.None:
        case XmlNodeType.Element:
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.SignificantWhitespace:
        case XmlNodeType.EndElement:
          return true;
        case XmlNodeType.Comment:
          return true;
        case XmlNodeType.Whitespace:
          if (xamlReader.XmlSpace == XmlSpace.Preserve)
            return true;
          break;
      }
    }
    return false;
  }
}
