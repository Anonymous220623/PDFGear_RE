// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HtmlToExcelConverter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class HtmlToExcelConverter
{
  private HtmlStringParser m_parser;

  internal HtmlToExcelConverter() => this.m_parser = new HtmlStringParser();

  internal void ParseHTMLTable(string htmlText, WorksheetImpl worksheetImpl, int row, int col)
  {
    string html = htmlText;
    int iColumn = col;
    string xml = this.m_parser.ReplaceHtmlSymbols(html);
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(xml);
    XmlNode xmlNode1 = (XmlNode) xmlDocument.DocumentElement;
    XmlNode style1 = (XmlNode) xmlDocument.DocumentElement;
    if (xmlNode1.LocalName.ToLower() == "html")
    {
      foreach (XmlNode childNode1 in xmlNode1.ChildNodes)
      {
        if (childNode1.LocalName.ToLower() == "head")
        {
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            if (childNode2.LocalName.ToLower() == "style")
            {
              style1 = childNode2;
              break;
            }
          }
        }
        if (childNode1.LocalName.ToLower() == "body")
        {
          xmlNode1 = childNode1;
          break;
        }
      }
    }
    Dictionary<string, CssStyle> cssStyle = this.ParseCssStyle(style1);
    List<int> intList1 = (List<int>) null;
    List<int> intList2 = (List<int>) null;
    foreach (XmlNode childNode in xmlNode1.ChildNodes)
    {
      if (childNode.LocalName.ToLower() == "table")
      {
        int tableRow = 1;
        TextFormat format1 = new TextFormat();
        TextFormat style2 = this.ParseStyle(childNode, format1);
        int maxMergedRow = row;
        intList2 = new List<int>();
        foreach (XmlNode xmlNode2 in childNode)
        {
          if (xmlNode2.LocalName == "Col")
          {
            if (intList1 == null)
              intList1 = new List<int>();
            if (xmlNode2.Attributes["width"] != null)
            {
              string s = xmlNode2.Attributes["width"].Value;
              int result = 0;
              if (s.EndsWith("px"))
                s = s.Replace("px", string.Empty);
              if (int.TryParse(s, out result))
                intList1.Add(result);
            }
          }
          if (xmlNode2.LocalName.ToLower() == "tr")
          {
            TextFormat format2 = style2.Clone();
            TextFormat style3 = this.ParseStyle(xmlNode2, format2);
            bool isColumnDisplay = true;
            this.ParseTableRow(xmlNode2, worksheetImpl, ref maxMergedRow, row, col, cssStyle, tableRow, style3, out isColumnDisplay);
            if (xmlNode2.Attributes["height"] != null)
            {
              int result = 0;
              string s = xmlNode2.Attributes["height"].Value;
              if (s.EndsWith("px"))
                s = s.Replace("px", string.Empty);
              if (int.TryParse(s, out result))
                worksheetImpl.SetRowHeightInPixels(row, (double) result);
            }
            if (!isColumnDisplay)
              intList2.Add(row);
            ++row;
            ++tableRow;
          }
        }
      }
      if (intList2 != null)
      {
        intList2.Reverse();
        foreach (int index in intList2)
          worksheetImpl.DeleteRow(index);
      }
      if (intList1 != null)
      {
        foreach (int num in intList1)
        {
          worksheetImpl.SetColumnWidthInPixels(iColumn, num);
          ++iColumn;
        }
      }
    }
  }

  private void ConvertCssStyle(CssStyle css, IStyle format)
  {
    if (!string.IsNullOrEmpty(css.BgColor))
      format.FillBackgroundRGB = this.m_parser.GetColor(css.BgColor);
    if (!string.IsNullOrEmpty(css.Color))
      format.Font.RGBColor = this.m_parser.GetColor(css.Color);
    if (!string.IsNullOrEmpty(css.FontSize))
    {
      string s = !css.FontSize.EndsWith("pt") ? css.FontSize : css.FontSize.Replace("pt", string.Empty);
      format.Font.Size = (double) int.Parse(s);
    }
    if (!string.IsNullOrEmpty(css.FontFamily))
      format.Font.FontName = css.FontFamily.Split(',')[0];
    if (!string.IsNullOrEmpty(css.TextAlign))
    {
      switch (css.TextAlign.ToLower().Trim())
      {
        case "left":
          format.HorizontalAlignment = ExcelHAlign.HAlignLeft;
          break;
        case "right":
          format.HorizontalAlignment = ExcelHAlign.HAlignRight;
          break;
        case "top":
          format.VerticalAlignment = ExcelVAlign.VAlignTop;
          break;
        case "bottom":
          format.VerticalAlignment = ExcelVAlign.VAlignBottom;
          break;
      }
    }
    if (!string.IsNullOrEmpty(css.Border))
    {
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeLeft], css.Border);
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeRight], css.Border);
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeBottom], css.Border);
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeTop], css.Border);
    }
    if (!string.IsNullOrEmpty(css.BottomBorder))
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeBottom], css.BottomBorder);
    if (!string.IsNullOrEmpty(css.TopBorder))
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeTop], css.TopBorder);
    if (!string.IsNullOrEmpty(css.LeftBorder))
      this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeLeft], css.LeftBorder);
    if (string.IsNullOrEmpty(css.RightBorder))
      return;
    this.ParseBorder(format.Borders[ExcelBordersIndex.EdgeRight], css.RightBorder);
  }

  private void ConvertTextFormat(TextFormat textFormat, IRange cellRange)
  {
    IStyle cellStyle = cellRange.CellStyle;
    if (textFormat.BackColor != Color.Empty)
      cellStyle.FillBackgroundRGB = textFormat.BackColor;
    if (textFormat.Bold)
      cellStyle.Font.Bold = true;
    if (textFormat.FontColor != Color.Empty)
      cellStyle.Font.RGBColor = textFormat.FontColor;
    if (!string.IsNullOrEmpty(textFormat.FontFamily))
      cellStyle.Font.FontName = textFormat.FontFamily;
    if ((double) textFormat.FontSize != 12.0)
      cellStyle.Font.Size = (double) textFormat.FontSize;
    if (textFormat.Italic)
      cellStyle.Font.Italic = true;
    if (textFormat.Strike)
      cellStyle.Font.Strikethrough = true;
    if (textFormat.SubScript)
      cellStyle.Font.Subscript = true;
    if (textFormat.SuperScript)
      cellStyle.Font.Superscript = true;
    if (textFormat.Underline)
      cellStyle.Font.Underline = ExcelUnderline.Single;
    if (!string.IsNullOrEmpty(textFormat.TextAlignment))
      cellRange.HorizontalAlignment = this.GetHorizontalAlign(textFormat.TextAlignment);
    if (!string.IsNullOrEmpty(textFormat.HorizantalAlignment))
      cellRange.HorizontalAlignment = this.GetHorizontalAlign(textFormat.HorizantalAlignment);
    if (string.IsNullOrEmpty(textFormat.VerticalAlignment))
      return;
    cellRange.VerticalAlignment = this.GetVerticalAlign(textFormat.VerticalAlignment);
  }

  internal ExcelHAlign GetHorizontalAlign(string htmlAlign)
  {
    switch (htmlAlign.Trim())
    {
      case "center":
        return ExcelHAlign.HAlignCenter;
      case "right":
        return ExcelHAlign.HAlignRight;
      default:
        return ExcelHAlign.HAlignLeft;
    }
  }

  internal ExcelVAlign GetVerticalAlign(string htmlAlign)
  {
    switch (htmlAlign.Trim())
    {
      case "top":
        return ExcelVAlign.VAlignTop;
      case "bottom":
        return ExcelVAlign.VAlignBottom;
      default:
        return ExcelVAlign.VAlignCenter;
    }
  }

  private IBorder ParseBorder(IBorder border, string borderPr)
  {
    string str1 = borderPr;
    char[] chArray = new char[1]{ ' ' };
    foreach (string str2 in str1.Split(chArray))
    {
      if (!str2.Trim().EndsWith("px"))
      {
        string str3 = str2.Trim();
        bool flag = true;
        switch (str3)
        {
          case "solid":
            border.LineStyle = ExcelLineStyle.Thin;
            break;
          case "dashed":
            border.LineStyle = ExcelLineStyle.Dashed;
            break;
          case "dotted":
            border.LineStyle = ExcelLineStyle.Dotted;
            break;
          case "double":
            border.LineStyle = ExcelLineStyle.Double;
            break;
          case "thin":
            border.LineStyle = ExcelLineStyle.Thin;
            break;
          default:
            flag = false;
            break;
        }
        if (!flag)
        {
          try
          {
            border.ColorRGB = this.m_parser.GetColor(str2.Trim());
          }
          catch
          {
          }
        }
      }
    }
    return border;
  }

  private Dictionary<string, CssStyle> ParseCssStyle(XmlNode style)
  {
    Dictionary<string, CssStyle> cssStyle1 = new Dictionary<string, CssStyle>();
    string str1 = style.InnerText.Replace("\r\n", "");
    char[] chArray1 = new char[1]{ '}' };
    foreach (string str2 in str1.Split(chArray1))
    {
      CssStyle cssStyle2 = new CssStyle();
      string[] strArray1 = str2.Split('{');
      string[] strArray2 = (string[]) null;
      if (strArray1.Length > 1)
        strArray2 = strArray1[0].Split(',');
      if (strArray2 != null)
      {
        string str3 = strArray1[1].TrimEnd('}');
        char[] chArray2 = new char[1]{ ';' };
        foreach (string str4 in str3.Split(chArray2))
        {
          char[] chArray3 = new char[1]{ ':' };
          string[] strArray3 = str4.Split(chArray3);
          switch (strArray3[0].Trim().ToLower())
          {
            case "border":
              cssStyle2.Border = strArray3[1];
              break;
            case "font-family":
              cssStyle2.FontFamily = strArray3[1];
              break;
            case "font-size":
              cssStyle2.FontSize = strArray3[1];
              break;
            case "color":
              cssStyle2.Color = strArray3[1];
              break;
            case "background-color":
              cssStyle2.BgColor = strArray3[1];
              break;
            case "border-bottom":
              cssStyle2.BottomBorder = strArray3[1];
              break;
            case "border-top":
              cssStyle2.TopBorder = strArray3[1];
              break;
            case "border-right":
              cssStyle2.RightBorder = strArray3[1];
              break;
            case "border-left":
              cssStyle2.LeftBorder = strArray3[1];
              break;
            case "border-collapse":
              cssStyle2.BorderCollapse = strArray3[1];
              break;
            case "width":
              cssStyle2.Width = strArray3[1];
              break;
            case "text-align":
              cssStyle2.TextAlign = strArray3[1];
              break;
          }
        }
        foreach (string str5 in strArray2)
        {
          string key = str5.Trim().Replace(".", string.Empty);
          cssStyle1.Add(key, cssStyle2);
        }
      }
    }
    return cssStyle1;
  }

  private void ParseTableRow(
    XmlNode rowNode,
    WorksheetImpl worksheetImpl,
    ref int maxMergedRow,
    int row,
    int col,
    Dictionary<string, CssStyle> styles,
    int tableRow,
    TextFormat format,
    out bool isColumnDisplay)
  {
    int num1 = 1;
    List<bool> boolList = new List<bool>();
    if (maxMergedRow > row)
      ++col;
    foreach (XmlNode xmlNode in rowNode)
    {
      TextFormat format1 = (TextFormat) null;
      string empty1 = string.Empty;
      if (xmlNode.Attributes["class"] != null)
        empty1 = xmlNode.Attributes["class"].Value;
      if (xmlNode.LocalName.ToLower() == "th" || xmlNode.LocalName.ToLower() == "td")
      {
        if (format != null)
          format1 = format.Clone();
        TextFormat style1 = this.ParseStyle(xmlNode, format1);
        if (xmlNode.ChildNodes.Count > 0 && xmlNode.ChildNodes[0].Name == "span")
          style1 = this.ParseStyle(xmlNode.ChildNodes[0], style1);
        int lastColumn = col;
        int lastRow = row;
        bool flag1 = false;
        bool flag2 = false;
        if (xmlNode.Attributes["colspan"] != null)
        {
          flag2 = true;
          lastColumn = int.Parse(xmlNode.Attributes["colspan"].Value) + col - 1;
        }
        if (xmlNode.Attributes["rowspan"] != null)
        {
          flag1 = true;
          lastRow = int.Parse(xmlNode.Attributes["rowspan"].Value) + row - 1;
        }
        if (style1.Display)
        {
          this.ParseTableColumn(xmlNode, worksheetImpl, row, col);
          if (style1 != null)
            this.ConvertTextFormat(style1, worksheetImpl[row, col]);
        }
        if (flag1 || flag2)
          worksheetImpl[row, col, lastRow, lastColumn].Merge();
        if (flag1 && maxMergedRow < lastRow)
          maxMergedRow = lastRow;
        boolList.Add(style1.Display);
        if (tableRow == 1)
        {
          worksheetImpl[row, col].CellStyle.Font.Bold = true;
          worksheetImpl[row, col].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
        }
        foreach (KeyValuePair<string, CssStyle> style2 in styles)
        {
          if (style2.Key.StartsWith("tr:nth-child"))
          {
            string empty2 = string.Empty;
            int num2 = style2.Key.IndexOf("(");
            int num3 = style2.Key.IndexOf(")");
            string s = style2.Key.Substring(num2 + 1, num3 - num2 - 1);
            if (s == "even" && tableRow % 2 == 0)
              this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            else if (s == "odd" && tableRow % 2 != 0)
            {
              this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            }
            else
            {
              int result;
              if (int.TryParse(s, out result) && tableRow == result)
                this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            }
          }
          else if (style2.Key.StartsWith("td:nth-child") || style2.Key.StartsWith("th:nth-child"))
          {
            string empty3 = string.Empty;
            int num4 = style2.Key.IndexOf("(");
            int num5 = style2.Key.IndexOf(")");
            string s = style2.Key.Substring(num4 + 1, num5 - num4 - 1);
            if (s == "even" && num1 % 2 == 0)
              this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            else if (s == "odd" && num1 % 2 != 0)
            {
              this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            }
            else
            {
              int result;
              if (int.TryParse(s, out result) && num1 == result)
                this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
            }
          }
          else if (style2.Key == "th" && xmlNode.LocalName.ToLower() == "th")
            this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
          else if (style2.Key == "td" && xmlNode.LocalName.ToLower() == "td")
            this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
          else if (style2.Key == "table")
            this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
          else if (style2.Key == empty1)
            this.ConvertCssStyle(style2.Value, worksheetImpl[row, col].CellStyle);
        }
        if (style1.Display)
        {
          ++col;
          ++num1;
        }
      }
    }
    int num6 = 0;
    foreach (bool flag in boolList)
    {
      if (!flag)
        ++num6;
    }
    if (num6 == boolList.Count)
      isColumnDisplay = false;
    else
      isColumnDisplay = true;
  }

  private void ParseTableColumn(XmlNode colNode, WorksheetImpl worksheetImpl, int row, int col)
  {
    worksheetImpl[row, col].Value = colNode.InnerText.Replace("\r\n", string.Empty).Trim();
    if (worksheetImpl.GetCellType(row, col, false) == WorksheetImpl.TRangeValueType.Number)
      return;
    worksheetImpl[row, col].HtmlString = colNode.InnerText.Length > 0 ? colNode.InnerXml.Replace(colNode.InnerText, colNode.InnerText.Replace("\r\n", string.Empty).Trim()) : colNode.InnerXml;
  }

  private TextFormat ParseStyle(XmlNode node, TextFormat format)
  {
    string attributeValue = this.m_parser.GetAttributeValue(node, "style");
    if (attributeValue.Length <= 0)
      return format;
    string[] strArray = attributeValue.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      char[] chArray = new char[2]{ '\'', '"' };
      string paramName = strArray[index].ToLower().Trim();
      string paramValue = strArray[index + 1].Trim().Trim(chArray);
      this.m_parser.GetFormat(format, paramName, paramValue, node);
    }
    return format;
  }

  private void ApplyTextFormatToStyle(TextFormat format, CellStyle style)
  {
    if (format == null)
      return;
    this.m_parser.ApplyTextFormatting(style.Font, format);
    style.Color = format.BackColor;
  }
}
