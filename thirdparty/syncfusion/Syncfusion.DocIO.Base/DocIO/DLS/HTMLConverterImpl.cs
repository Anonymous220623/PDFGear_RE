// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HTMLConverterImpl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Convertors;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class HTMLConverterImpl : IHtmlConverter
{
  private const string DEF_WHITESPACE = " ";
  private const float DEF_LH_INDENT = 35f;
  private const float DEF_MEDIUMVALUE = 3f;
  private const float DEF_THICKVALUE = 4.5f;
  private const float DEF_THINVALUE = 0.75f;
  private const float DEF_INDENT = 36f;
  private const string c_Xhtml1ScrictDocType = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n";
  private const string c_Xhtml1TRansitionalDocType = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n";
  private const float c_DefCellWidth = 3f;
  private byte m_bFlags;
  private static readonly Regex m_removeSpaces = new Regex("\\s+");
  private Stack<WField> m_fieldStack;
  private XmlDocument m_xmlDoc;
  private Stack<HTMLConverterImpl.TextFormat> m_styleStack = new Stack<HTMLConverterImpl.TextFormat>();
  private BodyItemCollection m_bodyItems;
  private WTextBody m_textBody;
  private Stack<BodyItemCollection> m_nestedBodyItems = new Stack<BodyItemCollection>();
  private Stack<WTable> m_nestedTable = new Stack<WTable>();
  private WParagraph m_currParagraph;
  private WTable m_currTable;
  private float cellSpacing;
  private string m_basePath;
  private int m_curListLevel = -1;
  private bool isPreTag;
  private List<int> m_listLevelNo = new List<int>();
  private bool checkFirstElement;
  private Stack<ListStyle> m_listStack;
  private Stack<string> m_lfoStack;
  [ThreadStatic]
  private static HTMLConverterImpl.TextFormat s_defFormat;
  internal float childTableWidth;
  private HorizontalAlignment m_horizontalAlignmentDefinedInCellNode;
  private HorizontalAlignment m_horizontalAlignmentDefinedInRowNode;
  private bool m_bBorderStyle;
  private int m_currTableFooterRowIndex = -1;
  private HTMLConverterImpl.TextFormat currDivFormat;
  private bool m_bIsInDiv;
  private Stack<bool> m_stackTableStyle = new Stack<bool>();
  private Stack<bool> m_stackRowStyle = new Stack<bool>();
  private Stack<bool> m_stackCellStyle = new Stack<bool>();
  public IWParagraphStyle m_userStyle;
  private HTMLConverterImpl.TableGrid tableGrid;
  private bool m_bIsInBlockquote;
  private int m_blockquoteLevel;
  private ListStyle m_userListStyle;
  private int m_divCount;
  private bool m_bIsAlignAttrDefinedInRowNode;
  private bool m_bIsAlignAttriDefinedInCellNode;
  private bool m_bIsVAlignAttriDefinedInRowNode;
  private VerticalAlignment m_verticalAlignmentDefinedInRowNode = VerticalAlignment.Middle;
  private bool m_bIsBorderCollapse;
  private Stack<float> m_listLeftIndentStack = new Stack<float>();
  private bool m_bIsWithinList;
  private Color m_hyperlinkcolor = Color.Empty;
  internal WSection m_currentSection;
  internal HTMLImportSettings HtmlImportSettings;
  private CSSStyle m_CSSStyle;
  private bool isPreserveBreakForInvalidStyles;
  private bool isLastLevelSkipped;
  private int lastSkippedLevelNo = -1;
  private int lastUsedLevelNo = -1;
  private int listCount;

  private bool IsPreviousItemFieldStart
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  private bool IsStyleFieldCode
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  private bool IsTableStyle
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  private bool IsRowStyle
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  private bool IsCellStyle
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal CSSStyle CSSStyle
  {
    get
    {
      if (this.m_CSSStyle == null)
        this.m_CSSStyle = new CSSStyle();
      return this.m_CSSStyle;
    }
  }

  internal float ClientWidth
  {
    get
    {
      float clientWidth = this.m_textBody.Document.LastSection.PageSetup.ClientWidth;
      if (this.m_textBody is WTableCell)
        clientWidth = (this.m_textBody as WTableCell).Width;
      else if (this.m_currentSection != null)
        clientWidth = this.m_currentSection.PageSetup.ClientWidth;
      return clientWidth;
    }
  }

  protected string BasePath
  {
    get => this.m_basePath;
    set => this.m_basePath = value;
  }

  private Stack<WField> FieldStack
  {
    get
    {
      if (this.m_fieldStack == null)
        this.m_fieldStack = new Stack<WField>();
      return this.m_fieldStack;
    }
  }

  private WField CurrentField
  {
    get
    {
      return this.m_fieldStack == null || this.m_fieldStack.Count <= 0 ? (WField) null : this.m_fieldStack.Peek();
    }
  }

  protected HTMLConverterImpl.TextFormat CurrentFormat
  {
    get
    {
      if (this.m_styleStack.Count > 0)
        return this.m_styleStack.Peek();
      if (HTMLConverterImpl.s_defFormat == null)
        HTMLConverterImpl.s_defFormat = new HTMLConverterImpl.TextFormat();
      return HTMLConverterImpl.s_defFormat;
    }
  }

  protected WParagraph CurrentPara
  {
    get
    {
      if (this.m_currParagraph == null)
      {
        this.m_currParagraph = new WParagraph((IWordDocument) this.m_bodyItems.Document);
        this.m_bodyItems.Add((IEntity) this.m_currParagraph);
        if (this.m_userStyle != null)
          this.m_currParagraph.ApplyStyle(this.m_userStyle, false);
      }
      return this.m_currParagraph;
    }
  }

  private Stack<string> LfoStack
  {
    get
    {
      if (this.m_lfoStack == null)
        this.m_lfoStack = new Stack<string>();
      return this.m_lfoStack;
    }
  }

  private Stack<ListStyle> ListStack
  {
    get
    {
      if (this.m_listStack == null)
        this.m_listStack = new Stack<ListStyle>();
      return this.m_listStack;
    }
  }

  private ListStyle CurrentListStyle
  {
    get
    {
      return this.m_listStack == null || this.m_listStack.Count == 0 ? (ListStyle) null : this.m_listStack.Peek();
    }
  }

  public void AppendToTextBody(
    ITextBody textBody,
    string html,
    int paragraphIndex,
    int paragraphItemIndex,
    IWParagraphStyle style,
    ListStyle listStyle)
  {
    if (style != null)
      this.m_userStyle = style;
    if (listStyle != null)
      this.m_userListStyle = listStyle;
    this.AppendToTextBody(textBody, html, paragraphIndex, paragraphItemIndex);
    this.m_userStyle = (IWParagraphStyle) null;
    this.m_userListStyle = (ListStyle) null;
  }

  public void AppendToTextBody(
    ITextBody textBody,
    string html,
    int paragraphIndex,
    int paragraphItemIndex)
  {
    textBody.Document.IsOpening = true;
    this.Init();
    this.m_basePath = textBody.Document.HtmlBaseUrl;
    this.m_textBody = textBody as WTextBody;
    this.m_currentSection = textBody.Owner as WSection;
    TextBodyPart textBodyPart = new TextBodyPart(textBody.Document);
    this.m_bodyItems = textBodyPart.BodyItems;
    this.m_currParagraph = (WParagraph) null;
    this.tableGrid = new HTMLConverterImpl.TableGrid();
    this.LoadXhtml(html);
    XmlNode node = (XmlNode) this.m_xmlDoc.DocumentElement;
    XmlNode documentElement = (XmlNode) this.m_xmlDoc.DocumentElement;
    if (node.LocalName.Equals(nameof (html), StringComparison.OrdinalIgnoreCase))
    {
      foreach (XmlNode childNode1 in node.ChildNodes)
      {
        if (childNode1.LocalName.Equals("head", StringComparison.OrdinalIgnoreCase) && childNode1.NodeType == XmlNodeType.Element)
        {
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            if (childNode2.LocalName.Equals("title", StringComparison.OrdinalIgnoreCase) && childNode2.NodeType == XmlNodeType.Element && this.m_bodyItems.Document.IsHTMLImport)
              this.m_bodyItems.Document.BuiltinDocumentProperties.Title = childNode2.InnerText;
            if (childNode2.LocalName.Equals("base", StringComparison.OrdinalIgnoreCase) && childNode2.NodeType == XmlNodeType.Element)
              this.BasePath = this.GetAttributeValue(childNode2, "href");
            else if (childNode2.LocalName.Equals("style", StringComparison.OrdinalIgnoreCase) && childNode2.NodeType == XmlNodeType.Element)
              this.ParseCssStyle(childNode2);
          }
        }
        if (childNode1.LocalName.Equals("body", StringComparison.OrdinalIgnoreCase))
        {
          this.ParseBodyAttributes(childNode1);
          node = childNode1;
          break;
        }
      }
    }
    bool bodyStyle = this.ParseBodyStyle(node, textBody, paragraphIndex);
    this.TraverseChildNodes(node.ChildNodes);
    this.LeaveStyle(bodyStyle);
    if (this.m_currParagraph != null)
      this.ApplyTextFormatting(this.m_currParagraph.BreakCharacterFormat);
    this.RemoveLastLineBreakFromParagraph(textBodyPart.BodyItems);
    textBody.Document.IsOpening = false;
    textBody.Document.IsHTMLImport = true;
    textBodyPart.PasteAt(textBody, paragraphIndex, paragraphItemIndex);
    textBody.Document.IsHTMLImport = false;
    if (textBodyPart.BodyItems.Count > 0 && textBodyPart.BodyItems[0].EntityType == EntityType.Paragraph)
    {
      WParagraph bodyItem = textBodyPart.BodyItems[0] as WParagraph;
      WParagraphFormat paragraphFormat = bodyItem.ParagraphFormat;
      (textBody.ChildEntities[paragraphIndex] as WParagraph).ParagraphFormat.ImportContainer((FormatBase) paragraphFormat);
      (textBody.ChildEntities[paragraphIndex] as WParagraph).ParagraphFormat.CopyProperties((FormatBase) paragraphFormat);
      if (!string.IsNullOrEmpty(bodyItem.StyleName))
        (textBody.ChildEntities[paragraphIndex] as WParagraph).ApplyStyle(bodyItem.StyleName);
      (textBody.ChildEntities[paragraphIndex] as WParagraph).BreakCharacterFormat.ImportContainer((FormatBase) bodyItem.BreakCharacterFormat);
    }
    if (textBodyPart.BodyItems.Count > 1 && textBodyPart.BodyItems[textBodyPart.BodyItems.Count - 1].EntityType == EntityType.Paragraph)
    {
      WParagraph bodyItem = textBodyPart.BodyItems[textBodyPart.BodyItems.Count - 1] as WParagraph;
      int index = paragraphIndex + textBodyPart.BodyItems.Count - 1;
      if (textBodyPart.BodyItems.Count > 0 && textBodyPart.BodyItems[0].EntityType == EntityType.Table)
        ++index;
      if (textBody.ChildEntities[index].EntityType == EntityType.Paragraph && bodyItem.StyleName != string.Empty)
        (textBody.ChildEntities[index] as WParagraph).ApplyStyle(bodyItem.StyleName);
    }
    this.SetNextStyleForParagraphStyle(this.m_bodyItems.Document);
    this.CSSStyle.Close();
  }

  private bool ParseBodyStyle(XmlNode node, ITextBody textBody, int paragraphIndex)
  {
    string attributeValue = this.GetAttributeValue(node, "style");
    if (attributeValue.Length == 0)
      return false;
    HTMLConverterImpl.TextFormat format = this.AddStyle();
    format.Borders = new HTMLConverterImpl.TableBorders();
    string[] strArray = attributeValue.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      char[] chArray = new char[2]{ '\'', '"' };
      string paramName = strArray[index].ToLower().Trim();
      string paramValue = strArray[index + 1].Trim().Trim(chArray);
      this.GetFormat(format, paramName, paramValue, node);
    }
    if ((node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase)) && (double) format.ListNumberWidth != 0.0 && (double) format.LeftMargin != 0.0 && (double) format.PaddingLeft != 0.0)
    {
      format.TextIndent = (float) -((double) format.PaddingLeft + (double) format.ListNumberWidth);
      format.LeftMargin = format.LeftMargin - format.TextIndent - format.ListNumberWidth;
    }
    if (textBody.ChildEntities.Count == 1 && textBody.ChildEntities[0] is WParagraph && paragraphIndex == 0)
    {
      this.ApplyPageBorder(format);
      this.ApplyPageFormat(format);
    }
    if (format.HasKey(7))
      format.BackColor = Color.Empty;
    if (format.HasKey(13))
      format.BottomMargin = 0.0f;
    if (format.HasKey(11))
      format.TopMargin = 0.0f;
    if (format.HasKey(12))
      format.LeftMargin = 0.0f;
    if (format.HasKey(14))
      format.RightMargin = 0.0f;
    format.Borders = new HTMLConverterImpl.TableBorders();
    return true;
  }

  private void ApplyPageFormat(HTMLConverterImpl.TextFormat format)
  {
    this.m_currentSection.PageSetup.Margins.Left += format.LeftMargin + (this.m_currentSection.Document.DOP.GutterAtTop ? 0.0f : this.m_currentSection.PageSetup.Margins.Gutter);
    this.m_currentSection.PageSetup.Margins.Right += format.RightMargin;
    this.m_currentSection.PageSetup.Margins.Top += format.TopMargin + (this.m_currentSection.Document.DOP.GutterAtTop ? this.m_currentSection.PageSetup.Margins.Gutter : 0.0f);
    this.m_currentSection.PageSetup.Margins.Bottom += format.BottomMargin;
    if (!format.HasKey(7))
      return;
    this.m_bodyItems.Document.Background.Type = BackgroundType.Color;
    this.m_bodyItems.Document.Background.SetBackgroundColor(format.BackColor);
  }

  private void ApplyPageBorder(HTMLConverterImpl.TextFormat format)
  {
    if (format.Borders.AllStyle != BorderStyle.None)
    {
      this.m_currentSection.PageSetup.Borders.Bottom.BorderType = this.m_currentSection.PageSetup.Borders.Top.BorderType = this.m_currentSection.PageSetup.Borders.Left.BorderType = this.m_currentSection.PageSetup.Borders.Right.BorderType = format.Borders.AllStyle;
      if (format.Borders.AllColor != Color.Empty)
        this.m_currentSection.PageSetup.Borders.Bottom.Color = this.m_currentSection.PageSetup.Borders.Top.Color = this.m_currentSection.PageSetup.Borders.Left.Color = this.m_currentSection.PageSetup.Borders.Right.Color = format.Borders.AllColor;
      if ((double) format.Borders.AllWidth != -1.0)
        this.m_currentSection.PageSetup.Borders.Bottom.LineWidth = this.m_currentSection.PageSetup.Borders.Top.LineWidth = this.m_currentSection.PageSetup.Borders.Left.LineWidth = this.m_currentSection.PageSetup.Borders.Right.LineWidth = format.Borders.AllWidth;
    }
    if (format.Borders.BottomStyle != BorderStyle.None)
    {
      this.m_currentSection.PageSetup.Borders.Bottom.BorderType = format.Borders.BottomStyle;
      this.m_currentSection.PageSetup.Borders.Bottom.LineWidth = format.Borders.BottomWidth;
      this.m_currentSection.PageSetup.Borders.Bottom.Color = format.Borders.BottomColor;
    }
    if (format.Borders.TopStyle != BorderStyle.None)
    {
      this.m_currentSection.PageSetup.Borders.Top.BorderType = format.Borders.TopStyle;
      this.m_currentSection.PageSetup.Borders.Top.LineWidth = format.Borders.TopWidth;
      this.m_currentSection.PageSetup.Borders.Top.Color = format.Borders.TopColor;
    }
    if (format.Borders.LeftStyle != BorderStyle.None)
    {
      this.m_currentSection.PageSetup.Borders.Left.BorderType = format.Borders.LeftStyle;
      this.m_currentSection.PageSetup.Borders.Left.LineWidth = format.Borders.LeftWidth;
      this.m_currentSection.PageSetup.Borders.Left.Color = format.Borders.LeftColor;
    }
    if (format.Borders.RightStyle != BorderStyle.None)
    {
      this.m_currentSection.PageSetup.Borders.Right.BorderType = format.Borders.RightStyle;
      this.m_currentSection.PageSetup.Borders.Right.LineWidth = format.Borders.RightWidth;
      this.m_currentSection.PageSetup.Borders.Right.Color = format.Borders.RightColor;
    }
    if ((double) format.Borders.TopWidth > 0.0)
      this.m_currentSection.PageSetup.Borders.Top.LineWidth = format.Borders.TopWidth;
    if ((double) format.Borders.RightWidth > 0.0)
      this.m_currentSection.PageSetup.Borders.Right.LineWidth = format.Borders.RightWidth;
    if ((double) format.Borders.LeftWidth > 0.0)
      this.m_currentSection.PageSetup.Borders.Left.LineWidth = format.Borders.LeftWidth;
    if ((double) format.Borders.BottomWidth <= 0.0)
      return;
    this.m_currentSection.PageSetup.Borders.Bottom.LineWidth = format.Borders.BottomWidth;
  }

  private void SetNextStyleForParagraphStyle(WordDocument document)
  {
    foreach (Style style in (IEnumerable) document.Styles)
    {
      if (style.StyleType == StyleType.ParagraphStyle)
        style.NextStyle = style.Name.Replace(" ", string.Empty);
    }
  }

  private void ParseBodyAttributes(XmlNode node)
  {
    string empty = string.Empty;
    foreach (XmlNode attribute in (XmlNamedNodeMap) node.Attributes)
    {
      switch (attribute.Name.ToLower())
      {
        case "link":
          string attributeValue1 = this.GetAttributeValue(node, "link");
          if (attributeValue1 != string.Empty)
          {
            IStyle style = this.m_bodyItems.Document.Styles.FindByName(BuiltinStyle.Hyperlink.ToString());
            if (style == null)
            {
              style = Style.CreateBuiltinStyle(BuiltinStyle.Hyperlink, StyleType.CharacterStyle, this.m_bodyItems.Document);
              this.m_bodyItems.Document.Styles.Add(style);
            }
            (style as Style).CharacterFormat.TextColor = this.GetColor(attributeValue1);
            this.m_hyperlinkcolor = this.GetColor(attributeValue1);
            continue;
          }
          continue;
        case "vlink":
          string attributeValue2 = this.GetAttributeValue(node, "vlink");
          if (attributeValue2 != string.Empty)
          {
            IStyle style = this.m_bodyItems.Document.Styles.FindByName(BuiltinStyle.FollowedHyperlink.ToString());
            if (style == null)
            {
              style = Style.CreateBuiltinStyle(BuiltinStyle.FollowedHyperlink, StyleType.CharacterStyle, this.m_bodyItems.Document);
              this.m_bodyItems.Document.Styles.Add(style);
            }
            (style as Style).CharacterFormat.TextColor = this.GetColor(attributeValue2);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void RemoveLastLineBreakFromParagraph(BodyItemCollection itemCollection)
  {
    foreach (TextBodyItem textBodyItem in (CollectionImpl) itemCollection)
    {
      if (textBodyItem.EntityType == EntityType.Paragraph)
      {
        WParagraph wparagraph = textBodyItem as WParagraph;
        if (wparagraph.Items.Count > 0 && wparagraph.Items[wparagraph.Items.Count - 1].EntityType == EntityType.Break && (wparagraph.Items[wparagraph.Items.Count - 1] as Break).BreakType == BreakType.LineBreak && (wparagraph.Items[wparagraph.Items.Count - 1] as Break).HtmlToDocLayoutInfo.RemoveLineBreak)
          wparagraph.Items.RemoveAt(wparagraph.Items.Count - 1);
        if (wparagraph.Items.Count > 0 && wparagraph.Items[wparagraph.Items.Count - 1].EntityType == EntityType.Break && (wparagraph.Items[wparagraph.Items.Count - 1] as Break).BreakType == BreakType.LineBreak && (wparagraph.Items[wparagraph.Items.Count - 1] as Break).HtmlToDocLayoutInfo.RemoveLineBreak)
          wparagraph.Items.RemoveAt(wparagraph.Items.Count - 1);
      }
      else if (textBodyItem.EntityType == EntityType.Table)
      {
        foreach (WTableRow row in (CollectionImpl) (textBodyItem as WTable).Rows)
        {
          foreach (WTextBody cell in (CollectionImpl) row.Cells)
            this.RemoveLastLineBreakFromParagraph(cell.Items);
        }
      }
    }
  }

  public bool IsValid(string html, XHTMLValidationType type)
  {
    return this.IsValid(html, type, out string _);
  }

  public bool IsValid(string html, XHTMLValidationType type, out string exceptionMessage)
  {
    exceptionMessage = string.Empty;
    Assembly.GetExecutingAssembly();
    XmlSchema schema = (XmlSchema) null;
    html = this.ReplaceHtmlConstantByUnicodeChar(html);
    switch (type)
    {
      case XHTMLValidationType.Strict:
        schema = XmlSchema.Read(this.GetManifestResourceStream("xhtml1-strict.xsd"), new ValidationEventHandler(this.OnValidation));
        break;
      case XHTMLValidationType.Transitional:
        schema = XmlSchema.Read(this.GetManifestResourceStream("xhtml1-transitional.xsd"), new ValidationEventHandler(this.OnValidation));
        break;
    }
    this.m_xmlDoc = new XmlDocument();
    this.m_xmlDoc.PreserveWhitespace = true;
    html = this.PrepareHtml(html, schema);
    try
    {
      if (type == XHTMLValidationType.None)
      {
        this.m_xmlDoc.LoadXml(html);
        return true;
      }
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.Schemas.Add(schema);
      XmlReader reader = XmlReader.Create((TextReader) new StringReader(html), settings);
      this.m_xmlDoc.Load(reader);
      reader.Close();
    }
    catch (Exception ex)
    {
      exceptionMessage = ex.Message;
      return false;
    }
    return true;
  }

  private string ReplaceHtmlConstantByUnicodeChar(string html)
  {
    html = this.ReplaceHtmlSpecialCharacters(html);
    html = this.ReplaceHtmlSymbols(html);
    html = this.ReplaceHtmlCharacters(html);
    html = this.ReplaceHtmlMathSymbols(html);
    html = this.ReplaceHtmlGreekLetters(html);
    html = this.ReplaceHtmlOtherEntities(html);
    html = this.ReplaceAmpersand(html);
    return html;
  }

  private string ReplaceHtmlSpecialCharacters(string html)
  {
    html = html.Replace("&quot;", "&#34;");
    html = html.Replace("&apos;", "&#39;");
    html = html.Replace("&amp;", "&#38;");
    html = html.Replace("&lt;", "&#60;");
    html = html.Replace("&gt;", "&#62;");
    html = html.Replace('Â'.ToString(), "&#194;");
    html = html.Replace('«'.ToString(), "&#171;");
    html = html.Replace('»'.ToString(), "&#187;");
    return html;
  }

  private string ReplaceHtmlSymbols(string html)
  {
    html = html.Replace("&nbsp;", "&#160;");
    html = html.Replace("&iexcl;", "&#161;");
    html = html.Replace("&cent;", "&#162;");
    html = html.Replace("&pound;", "&#163;");
    html = html.Replace("&curren;", "&#164;");
    html = html.Replace("&yen;", "&#165;");
    html = html.Replace("&brvbar;", "&#166;");
    html = html.Replace("&sect;", "&#167;");
    html = html.Replace("&uml;", "&#168;");
    html = html.Replace("&copy;", "&#169;");
    html = html.Replace("&ordf;", "&#170;");
    html = html.Replace("&laquo;", "&#171;");
    html = html.Replace("&not;", "&#172;");
    html = html.Replace("&shy;", "&#173;");
    html = html.Replace("&reg;", "&#174;");
    html = html.Replace("&macr;", "&#175;");
    html = html.Replace("&deg;", "&#176;");
    html = html.Replace("&plusmn;", "&#177;");
    html = html.Replace("&sup2;", "&#178;");
    html = html.Replace("&sup3;", "&#179;");
    html = html.Replace("&acute;", "&#180;");
    html = html.Replace("&micro;", "&#181;");
    html = html.Replace("&para;", "&#182;");
    html = html.Replace("&middot;", "&#183;");
    html = html.Replace("&cedil;", "&#184;");
    html = html.Replace("&sup1;", "&#185;");
    html = html.Replace("&ordm;", "&#186;");
    html = html.Replace("&raquo;", "&#187;");
    html = html.Replace("&frac14;", "&#188;");
    html = html.Replace("&frac12;", "&#189;");
    html = html.Replace("&frac34;", "&#190;");
    html = html.Replace("&iquest;", "&#191;");
    html = html.Replace("&times;", "&#215;");
    html = html.Replace("&divide;", "&#247;");
    return html;
  }

  private string ReplaceHtmlCharacters(string html)
  {
    html = html.Replace("&Agrave;", "&#192;");
    html = html.Replace("&Aacute;", "&#193;");
    html = html.Replace("&Acirc;", "&#194;");
    html = html.Replace("&Atilde;", "&#195;");
    html = html.Replace("&Auml;", "&#196;");
    html = html.Replace("&Aring;", "&#197;");
    html = html.Replace("&AElig;", "&#198;");
    html = html.Replace("&Ccedil;", "&#199;");
    html = html.Replace("&Egrave;", "&#200;");
    html = html.Replace("&Eacute;", "&#201;");
    html = html.Replace("&Ecirc;", "&#202;");
    html = html.Replace("&Euml;", "&#203;");
    html = html.Replace("&Igrave;", "&#204;");
    html = html.Replace("&Iacute;", "&#205;");
    html = html.Replace("&Icirc;", "&#206;");
    html = html.Replace("&Iuml;", "&#207;");
    html = html.Replace("&ETH;", "&#208;");
    html = html.Replace("&Ntilde;", "&#209;");
    html = html.Replace("&Ograve;", "&#210;");
    html = html.Replace("&Oacute;", "&#211;");
    html = html.Replace("&Ocirc;", "&#212;");
    html = html.Replace("&Otilde;", "&#213;");
    html = html.Replace("&Ouml;", "&#214;");
    html = html.Replace("&Oslash;", "&#216;");
    html = html.Replace("&Ugrave;", "&#217;");
    html = html.Replace("&Uacute;", "&#218;");
    html = html.Replace("&Ucirc;", "&#219;");
    html = html.Replace("&Uuml;", "&#220;");
    html = html.Replace("&Yacute;", "&#221;");
    html = html.Replace("&THORN;", "&#222;");
    html = html.Replace("&szlig;", "&#223;");
    html = html.Replace("&agrave;", "&#224;");
    html = html.Replace("&aacute;", "&#225;");
    html = html.Replace("&acirc;", "&#226;");
    html = html.Replace("&atilde;", "&#227;");
    html = html.Replace("&auml;", "&#228;");
    html = html.Replace("&aring;", "&#229;");
    html = html.Replace("&aelig;", "&#230;");
    html = html.Replace("&ccedil;", "&#231;");
    html = html.Replace("&egrave;", "&#232;");
    html = html.Replace("&eacute;", "&#233;");
    html = html.Replace("&ecirc;", "&#234;");
    html = html.Replace("&euml;", "&#235;");
    html = html.Replace("&igrave;", "&#236;");
    html = html.Replace("&iacute;", "&#237;");
    html = html.Replace("&icirc;", "&#238;");
    html = html.Replace("&iuml;", "&#239;");
    html = html.Replace("&eth;", "&#240;");
    html = html.Replace("&ntilde;", "&#241;");
    html = html.Replace("&ograve;", "&#242;");
    html = html.Replace("&oacute;", "&#243;");
    html = html.Replace("&ocirc;", "&#244;");
    html = html.Replace("&otilde;", "&#245;");
    html = html.Replace("&ouml;", "&#246;");
    html = html.Replace("&oslash;", "&#248;");
    html = html.Replace("&ugrave;", "&#249;");
    html = html.Replace("&uacute;", "&#250;");
    html = html.Replace("&ucirc;", "&#251;");
    html = html.Replace("&uuml;", "&#252;");
    html = html.Replace("&yacute;", "&#253;");
    html = html.Replace("&thorn;", "&#254;");
    html = html.Replace("&yuml;", "&#255;");
    return html;
  }

  private string ReplaceHtmlMathSymbols(string html)
  {
    html = html.Replace("&forall;", "&#8704;");
    html = html.Replace("&part;", "&#8706;");
    html = html.Replace("&exist;", "&#8707;");
    html = html.Replace("&empty;", "&#8709;");
    html = html.Replace("&nabla;", "&#8711;");
    html = html.Replace("&isin;", "&#8712;");
    html = html.Replace("&notin;", "&#8713;");
    html = html.Replace("&ni;", "&#8715;");
    html = html.Replace("&prod;", "&#8719;");
    html = html.Replace("&sum;", "&#8721;");
    html = html.Replace("&minus;", "&#8722;");
    html = html.Replace("&lowast;", "&#8727;");
    html = html.Replace("&radic;", "&#8730;");
    html = html.Replace("&prop;", "&#8733;");
    html = html.Replace("&infin;", "&#8734;");
    html = html.Replace("&ang;", "&#8736;");
    html = html.Replace("&and;", "&#8743;");
    html = html.Replace("&or;", "&#8744;");
    html = html.Replace("&cap;", "&#8745;");
    html = html.Replace("&cup;", "&#8746;");
    html = html.Replace("&int;", "&#8747;");
    html = html.Replace("&there4;", "&#8756;");
    html = html.Replace("&sim;", "&#8764;");
    html = html.Replace("&cong;", "&#8773;");
    html = html.Replace("&asymp;", "&#8776;");
    html = html.Replace("&ne;", "&#8800;");
    html = html.Replace("&equiv;", "&#8801;");
    html = html.Replace("&le;", "&#8804;");
    html = html.Replace("&ge;", "&#8805;");
    html = html.Replace("&sub;", "&#8834;");
    html = html.Replace("&sup;", "&#8835;");
    html = html.Replace("&nsub;", "&#8836;");
    html = html.Replace("&sube;", "&#8838;");
    html = html.Replace("&supe;", "&#8839;");
    html = html.Replace("&oplus;", "&#8853;");
    html = html.Replace("&otimes;", "&#8855;");
    html = html.Replace("&perp;", "&#8869;");
    html = html.Replace("&sdot;", "&#8901;");
    html = html.Replace("&frasl;", "&#8260;");
    return html;
  }

  private string ReplaceHtmlGreekLetters(string html)
  {
    html = html.Replace("&Alpha;", "&#913;");
    html = html.Replace("&Beta;", "&#914;");
    html = html.Replace("&Gamma;", "&#915;");
    html = html.Replace("&Delta;", "&#916;");
    html = html.Replace("&Epsilon;", "&#917;");
    html = html.Replace("&Zeta;", "&#918;");
    html = html.Replace("&Eta;", "&#919;");
    html = html.Replace("&Theta;", "&#920;");
    html = html.Replace("&Iota;", "&#921;");
    html = html.Replace("&Kappa;", "&#922;");
    html = html.Replace("&Lambda;", "&#923;");
    html = html.Replace("&Mu;", "&#924;");
    html = html.Replace("&Nu;", "&#925;");
    html = html.Replace("&Xi;", "&#926;");
    html = html.Replace("&Omicron;", "&#927;");
    html = html.Replace("&Pi;", "&#928;");
    html = html.Replace("&Rho;", "&#929;");
    html = html.Replace("&Sigma;", "&#931;");
    html = html.Replace("&Tau;", "&#932;");
    html = html.Replace("&Upsilon;", "&#933;");
    html = html.Replace("&Phi;", "&#934;");
    html = html.Replace("&Chi;", "&#935;");
    html = html.Replace("&Psi;", "&#936;");
    html = html.Replace("&Omega;", "&#937;");
    html = html.Replace("&alpha;", "&#945;");
    html = html.Replace("&beta;", "&#946;");
    html = html.Replace("&gamma;", "&#947;");
    html = html.Replace("&delta;", "&#948;");
    html = html.Replace("&epsilon;", "&#949;");
    html = html.Replace("&zeta;", "&#950;");
    html = html.Replace("&eta;", "&#951;");
    html = html.Replace("&theta;", "&#952;");
    html = html.Replace("&iota;", "&#953;");
    html = html.Replace("&kappa;", "&#954;");
    html = html.Replace("&lambda;", "&#955;");
    html = html.Replace("&mu;", "&#956;");
    html = html.Replace("&nu;", "&#957;");
    html = html.Replace("&xi;", "&#958;");
    html = html.Replace("&omicron;", "&#959;");
    html = html.Replace("&pi;", "&#960;");
    html = html.Replace("&rho;", "&#961;");
    html = html.Replace("&sigmaf;", "&#962;");
    html = html.Replace("&sigma;", "&#963;");
    html = html.Replace("&tau;", "&#964;");
    html = html.Replace("&upsilon;", "&#965;");
    html = html.Replace("&phi;", "&#966;");
    html = html.Replace("&chi;", "&#967;");
    html = html.Replace("&psi;", "&#968;");
    html = html.Replace("&omega;", "&#969;");
    html = html.Replace("&thetasym;", "&#977;");
    html = html.Replace("&upsih;", "&#978;");
    html = html.Replace("&piv;", "&#982;");
    return html;
  }

  private string ReplaceHtmlOtherEntities(string html)
  {
    html = html.Replace("&OElig;", "&#338;");
    html = html.Replace("&oelig;", "&#339;");
    html = html.Replace("&Scaron;", "&#352;");
    html = html.Replace("&scaron;", "&#353;");
    html = html.Replace("&Yuml;", "&#376;");
    html = html.Replace("&fnof;", "&#402;");
    html = html.Replace("&circ;", "&#710;");
    html = html.Replace("&tilde;", "&#732;");
    html = html.Replace("&ensp;", "&#8194;");
    html = html.Replace("&emsp;", "&#8195;");
    html = html.Replace("&thinsp;", "&#8201;");
    html = html.Replace("&zwnj;", "&#8204;");
    html = html.Replace("&zwj;", "&#8205;");
    html = html.Replace("&lrm;", "&#8206;");
    html = html.Replace("&rlm;", "&#8207;");
    html = html.Replace("&ndash;", "&#8211;");
    html = html.Replace("&mdash;", "&#8212;");
    html = html.Replace("&lsquo;", "&#8216;");
    html = html.Replace("&rsquo;", "&#8217;");
    html = html.Replace("&sbquo;", "&#8218;");
    html = html.Replace("&ldquo;", "&#8220;");
    html = html.Replace("&rdquo;", "&#8221;");
    html = html.Replace("&bdquo;", "&#8222;");
    html = html.Replace("&dagger;", "&#8224;");
    html = html.Replace("&Dagger;", "&#8225;");
    html = html.Replace("&bull;", "&#8226;");
    html = html.Replace("&hellip;", "&#8230;");
    html = html.Replace("&permil;", "&#8240;");
    html = html.Replace("&prime;", "&#8242;");
    html = html.Replace("&Prime;", "&#8243;");
    html = html.Replace("&lsaquo;", "&#8249;");
    html = html.Replace("&rsaquo;", "&#8250;");
    html = html.Replace("&oline;", "&#8254;");
    html = html.Replace("&euro;", "&#8364;");
    html = html.Replace("&trade;", "&#8482;");
    html = html.Replace("&larr;", "&#8592;");
    html = html.Replace("&uarr;", "&#8593;");
    html = html.Replace("&rarr;", "&#8594;");
    html = html.Replace("&darr;", "&#8595;");
    html = html.Replace("&harr;", "&#8596;");
    html = html.Replace("&crarr;", "&#8629;");
    html = html.Replace("&lArr;", "&#8656;");
    html = html.Replace("&uArr;", "&#8657;");
    html = html.Replace("&rArr;", "&#8658;");
    html = html.Replace("&dArr;", "&#8659;");
    html = html.Replace("&hArr;", "&#8660;");
    html = html.Replace("&lceil;", "&#8968;");
    html = html.Replace("&rceil;", "&#8969;");
    html = html.Replace("&lfloor;", "&#8970;");
    html = html.Replace("&rfloor;", "&#8971;");
    html = html.Replace("&loz;", "&#9674;");
    html = html.Replace("&spades;", "&#9824;");
    html = html.Replace("&clubs;", "&#9827;");
    html = html.Replace("&hearts;", "&#9829;");
    html = html.Replace("&diams;", "&#9830;");
    html = html.Replace("&lang;", "&#9001;");
    html = html.Replace("&rang;", "&#9002;");
    return html;
  }

  private string ReplaceAmpersand(string html)
  {
    List<int> positions = this.GetPositions(html, "&");
    int num = 0;
    for (int index = 0; index < positions.Count; ++index)
    {
      int startIndex = positions[index] + num;
      if (html.Length > startIndex + 1 && html[startIndex + 1] != '#')
      {
        html = html.Remove(startIndex, 1);
        html = html.Insert(startIndex, "&#38;");
        num += 4;
      }
    }
    return html;
  }

  private List<int> GetPositions(string source, string searchString)
  {
    List<int> positions = new List<int>();
    int length = searchString.Length;
    int num = -1;
    while (true)
    {
      num = source.IndexOf(searchString, num + 1);
      if (num != -1)
        positions.Add(num);
      else
        break;
    }
    return positions;
  }

  private void LoadXhtml(string html)
  {
    XmlSchema schema = (XmlSchema) null;
    html = html.Replace("&nbsp;", "nbsp;");
    html = html.Replace("&#160;", "nbsp;");
    switch (this.m_bodyItems.Document.XHTMLValidateOption)
    {
      case XHTMLValidationType.Strict:
        schema = XmlSchema.Read(this.GetManifestResourceStream("xhtml1-strict.xsd"), new ValidationEventHandler(this.OnValidation));
        break;
      case XHTMLValidationType.Transitional:
        schema = XmlSchema.Read(this.GetManifestResourceStream("xhtml1-transitional.xsd"), new ValidationEventHandler(this.OnValidation));
        break;
    }
    try
    {
      this.m_xmlDoc = new XmlDocument();
      this.m_xmlDoc.PreserveWhitespace = true;
      this.LoadXhtml(html, schema);
    }
    catch (XmlException ex)
    {
      throw new NotSupportedException("DocIO support only welformatted xhtml \nDetails:\n" + ex.Message, (Exception) ex);
    }
  }

  private void LoadXhtml(string html, XmlSchema schema)
  {
    html = this.PrepareHtml(html, schema);
    if (schema != null)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.Schemas.Add(schema);
      settings.ValidationEventHandler += new ValidationEventHandler(this.readerSettings_ValidationEventHandler);
      XmlReader reader = XmlReader.Create((TextReader) new StringReader(html), settings);
      this.m_xmlDoc.Load(reader);
      reader.Close();
    }
    else
      this.m_xmlDoc.LoadXml(html);
  }

  private void readerSettings_ValidationEventHandler(object sender, ValidationEventArgs e)
  {
    throw new NotSupportedException("DocIO support only welformatted xhtml \nDetails:\n" + e.Exception.Message, (Exception) e.Exception);
  }

  private string PrepareHtml(string html, XmlSchema schema)
  {
    html = this.ReplaceHtmlConstantByUnicodeChar(html);
    html = this.RemoveXmlAndDocTypeElement(html);
    html = this.InsertHtmlElement(html, schema);
    return html;
  }

  private string RemoveXmlAndDocTypeElement(string html)
  {
    while (true)
    {
      html = html.TrimStart();
      if (html.StartsWith("<?xml version", StringComparison.OrdinalIgnoreCase) || html.StartsWith("<?xmlversion", StringComparison.OrdinalIgnoreCase) || html.StartsWith("<xml", StringComparison.OrdinalIgnoreCase))
      {
        int num = html.IndexOf(">");
        html = html.Remove(0, num + 1);
      }
      else if (html.StartsWith("<!doctype", StringComparison.OrdinalIgnoreCase))
      {
        int num = html.IndexOf(">");
        html = html.Remove(0, num + 1);
      }
      else if (html.StartsWith(ControlChar.CarriegeReturn) || html.StartsWith(ControlChar.LineFeed))
        html = html.Remove(0, 1);
      else
        break;
    }
    return html;
  }

  private string InsertHtmlElement(string html, XmlSchema schema)
  {
    string str = "<html>";
    if (schema != null)
      str = $"<html xmlns=\"{schema.TargetNamespace}\">";
    if (html.StartsWith("<html", StringComparison.OrdinalIgnoreCase))
    {
      if (schema != null)
      {
        int count = html.ToLower().IndexOf("<body");
        if (count != -1)
          html = str + $"<head><title>{this.GetDocumentTitle()}</title></head>" + html.Remove(0, count);
      }
    }
    else
      html = !html.StartsWith("<head", StringComparison.OrdinalIgnoreCase) ? (!html.StartsWith("<body", StringComparison.OrdinalIgnoreCase) ? $"{str}{$"<head><title>{this.GetDocumentTitle()}</title></head><body>"}{html}</body></html>" : $"{str}{$"<head><title>{this.GetDocumentTitle()}</title></head>"}{html}</html>") : $"{str}{html}</html>";
    return html;
  }

  private string GetDocumentTitle()
  {
    return this.m_bodyItems != null && this.m_bodyItems.Document != null && this.m_bodyItems.Document.BuiltinDocumentProperties != null ? this.m_bodyItems.Document.BuiltinDocumentProperties.Title : string.Empty;
  }

  private void TraverseChildNodes(XmlNodeList nodes)
  {
    XmlNode prevNode = (XmlNode) null;
    foreach (XmlNode node in nodes)
    {
      if (node.NodeType == XmlNodeType.Text)
        this.TraverseTextWithinTag(node, prevNode);
      else if (node.NodeType == XmlNodeType.Comment)
        this.TraverseComments(node);
      else if (node.NodeType == XmlNodeType.Element)
        this.ParseTags(node);
      else if (node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.SignificantWhitespace)
      {
        if ((this.StartsWithExt(node.Value, " ") || this.StartsWithExt(node.Value, ControlChar.Tab)) && this.m_currParagraph != null)
        {
          if (!this.isPreTag)
            this.TraverseTextWithinTag(node, prevNode);
          else
            continue;
        }
        if ((this.isPreTag || this.CurrentFormat.IsPreserveWhiteSpace) && (this.StartsWithExt(node.Value, ControlChar.CrLf) || this.StartsWithExt(node.Value, ControlChar.LineFeed)))
          this.TraverseTextWithinTag(node, prevNode);
      }
      prevNode = node;
    }
    if (nodes.Count != 0 || (double) this.CurrentFormat.TabWidth == 0.0 || !this.CurrentFormat.IsInlineBlock)
      return;
    this.ApplyTextFormatting(this.CurrentPara.AppendText("\t").CharacterFormat);
  }

  private void TraverseTextWithinTag(XmlNode node, XmlNode prevNode)
  {
    if (this.CurrentFormat != null && this.CurrentFormat.IsNonBreakingSpace && node.InnerText.Contains(ControlChar.NonBreakingSpace))
      node.InnerText = node.InnerText.Replace(ControlChar.NonBreakingSpace, ControlChar.Space);
    if (this.IsPargraphNeedToBeAdded(prevNode) && !this.isPreTag)
      this.AddNewParagraphToTextBody(node);
    else if (this.m_currParagraph == null && (node.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) && this.IsFirstNode(node) || node.ParentNode.LocalName.Equals("span", StringComparison.OrdinalIgnoreCase) || node.ParentNode.LocalName.Equals("blockquote", StringComparison.OrdinalIgnoreCase)))
      this.AddNewParagraphToTextBody(node);
    if (this.CurrentFormat != null && this.CurrentFormat.IsListTab)
    {
      if ((double) this.CurrentFormat.TabPosition == 0.0 || this.m_currParagraph == null)
        return;
      this.m_currParagraph.ParagraphFormat.Tabs.AddTab(this.CurrentFormat.TabPosition, this.CurrentFormat.TabJustification, this.CurrentFormat.TabLeader);
    }
    else
    {
      string attributeValue = this.GetAttributeValue(node.ParentNode, "id");
      if (attributeValue != string.Empty)
        this.CurrentPara.AppendBookmarkStart(attributeValue);
      if (this.IsPreviousItemFieldStart || this.CurrentField != null)
      {
        this.ParseField(node);
      }
      else
      {
        if (this.isPreTag)
        {
          if (node.NodeType == XmlNodeType.Whitespace && node.Value.Equals(ControlChar.CrLf))
          {
            if (this.IsPargraphNeedToBeAdded(node.PreviousSibling))
            {
              this.AddNewParagraph(node);
              this.ApplyTextFormatting(this.CurrentPara.BreakCharacterFormat);
              this.ApplyParagraphFormat(node.ParentNode);
            }
          }
          else
          {
            string innerText = node.InnerText;
            string[] array = Regex.Split(innerText, ControlChar.CrLf);
            if (innerText.EndsWith(ControlChar.CrLf))
              Array.Resize<string>(ref array, array.Length - 1);
            if (!innerText.Contains(ControlChar.CrLf))
            {
              this.ApplyTextFormatting(this.CurrentPara.AppendText(node.InnerText).CharacterFormat);
              this.ApplyTextFormatting(this.CurrentPara.BreakCharacterFormat);
              this.ApplyParagraphFormat(node.ParentNode);
            }
            else
            {
              string text1 = array[0];
              if (this.IsTabText(text1))
                text1 = "\t";
              this.ApplyTextFormatting(this.CurrentPara.AppendText(text1).CharacterFormat);
              this.ApplyTextFormatting(this.CurrentPara.BreakCharacterFormat);
              this.ApplyParagraphFormat(node.ParentNode);
              for (int index = 1; index < array.Length; ++index)
              {
                this.AddNewParagraph(node);
                this.ApplyParagraphStyle();
                string text2 = array[index];
                if (this.IsTabText(text2))
                  text2 = "\t";
                this.ApplyTextFormatting(this.CurrentPara.AppendText(text2).CharacterFormat);
                this.ApplyTextFormatting(this.CurrentPara.BreakCharacterFormat);
                this.ApplyParagraphFormat(node.ParentNode);
              }
            }
          }
        }
        else
        {
          string str = node.ParentNode.LocalName.Equals("title", StringComparison.OrdinalIgnoreCase) ? node.InnerText : this.RemoveNewLineCharacter(node.InnerText);
          if (node.ParentNode.LocalName.Equals("title", StringComparison.OrdinalIgnoreCase))
            this.m_bodyItems.Document.BuiltinDocumentProperties.Title = str;
          else if (!(node.ParentNode.LocalName == "body") || node.PreviousSibling != null)
          {
            if (node.ParentNode.LocalName == "p" && this.checkFirstElement)
            {
              this.AddNewParagraph(node);
              this.checkFirstElement = true;
            }
            if (node.ParentNode.LocalName == "body")
            {
              bool spaceAfterAuto = this.m_currParagraph.ParagraphFormat.SpaceAfterAuto;
              bool spaceBeforeAuto = this.m_currParagraph.ParagraphFormat.SpaceBeforeAuto;
              this.ApplyParagraphStyle();
              if ((!spaceBeforeAuto || !spaceAfterAuto) && this.m_userStyle == null && this.m_currParagraph.StyleName == "Normal (Web)")
                this.m_currParagraph.ApplyStyle("Normal");
            }
            if (this.IsTabText(node.InnerText))
              this.ApplyTextFormatting(this.CurrentPara.AppendText("\t").CharacterFormat);
            else if (str != string.Empty && (!(str == " ") || !(node.ParentNode.LocalName == "br")))
            {
              string text = str;
              if (this.IsTabText(node.InnerText))
                text = "\t";
              this.ApplyTextFormatting(this.CurrentPara.AppendText(text).CharacterFormat);
            }
            if (node.ParentNode.LocalName == "span")
              this.ApplySpanParagraphFormat();
          }
          else
          {
            string text = str;
            if (this.IsTabText(node.InnerText))
              text = "\t";
            this.ApplyTextFormatting(this.CurrentPara.AppendText(text).CharacterFormat);
          }
        }
        if (!(attributeValue != string.Empty))
          return;
        this.CurrentPara.AppendBookmarkEnd(attributeValue);
      }
    }
  }

  private bool IsTabText(string text)
  {
    if (!string.IsNullOrEmpty(text))
    {
      if (text.Trim(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar, '.', '_', '-') == string.Empty && this.CurrentFormat != null && (double) this.CurrentFormat.TabWidth != 0.0 && this.CurrentFormat.IsInlineBlock)
        return true;
    }
    return false;
  }

  private void AddNewParagraphToTextBody(XmlNode node)
  {
    this.AddNewParagraph(node);
    if (!this.m_bIsInBlockquote)
      return;
    this.CurrentPara.ParagraphFormat.SetPropertyValue(2, (object) (float) ((double) this.CurrentPara.ParagraphFormat.LeftIndent + (double) (this.m_blockquoteLevel * 36)));
  }

  private void ApplySpanParagraphFormat()
  {
    if (this.CurrentPara == null)
      return;
    WParagraphFormat paragraphFormat = this.CurrentPara.ParagraphFormat;
    HTMLConverterImpl.TextFormat currentFormat = this.CurrentFormat;
    if (currentFormat.HasValue(10))
      paragraphFormat.HorizontalAlignment = currentFormat.TextAlign;
    if (currentFormat.HasValue(8))
    {
      paragraphFormat.LineSpacingRule = currentFormat.LineSpacingRule;
      paragraphFormat.SetPropertyValue(52, (object) currentFormat.LineHeight);
    }
    if (this.m_bIsWithinList && (!this.m_currParagraph.IsInCell || this.m_currParagraph.ListFormat.CurrentListStyle != null))
      paragraphFormat.SetPropertyValue(2, (object) (float) ((double) currentFormat.LeftMargin + (this.m_listLeftIndentStack.Count > 0 ? (double) this.m_listLeftIndentStack.Peek() : 0.0)));
    else if (currentFormat.HasValue(12))
      paragraphFormat.SetPropertyValue(2, (object) currentFormat.LeftMargin);
    if (currentFormat.HasValue(15))
      paragraphFormat.SetPropertyValue(5, (object) currentFormat.TextIndent);
    if (currentFormat.HasValue(14))
      paragraphFormat.SetPropertyValue(3, (object) currentFormat.RightMargin);
    if (currentFormat.HasValue(17))
    {
      paragraphFormat.PageBreakBefore = currentFormat.PageBreakBefore;
      paragraphFormat.KeepFollow = true;
    }
    if (currentFormat.HasValue(18))
      paragraphFormat.PageBreakAfter = currentFormat.PageBreakAfter;
    if (currentFormat.HasValue(22))
      paragraphFormat.WordWrap = currentFormat.WordWrap;
    if ((double) currentFormat.TabPosition == 0.0)
      return;
    paragraphFormat.Tabs.AddTab(currentFormat.TabPosition, currentFormat.TabJustification, currentFormat.TabLeader);
  }

  private bool IsPargraphNeedToBeAdded(XmlNode node)
  {
    return node != null && (node.LocalName.Equals("dt", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("dd", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h1", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h2", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h3", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h4", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h5", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("h6", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("p", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("table", StringComparison.OrdinalIgnoreCase));
  }

  private string RemoveWhiteSpacesAtParagraphBegin(string text, WParagraph CurrentPara)
  {
    if (this.StartsWithExt(text, " "))
    {
      if (CurrentPara.ChildEntities.LastItem != null && CurrentPara.ChildEntities.LastItem.EntityType == EntityType.Break)
        text = text.TrimStart();
      else if (CurrentPara.Text == "" || CurrentPara.Text == null)
        text = text.TrimStart();
    }
    return text;
  }

  private void AddNewParagraph(XmlNode node)
  {
    this.m_currParagraph = new WParagraph((IWordDocument) this.m_bodyItems.Document);
    this.m_bodyItems.Add((IEntity) this.m_currParagraph);
    if (this.isPreTag && !node.LocalName.Equals("p", StringComparison.OrdinalIgnoreCase))
      this.m_currParagraph.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
    if (this.currDivFormat == null || this.m_bIsInDiv && this.m_currParagraph.IsInCell && !this.NodeIsInDiv(node))
      return;
    this.ApplyDivParagraphFormat(node);
  }

  private bool NodeIsInDiv(XmlNode node)
  {
    for (XmlNode parentNode = node.ParentNode; parentNode != null && !parentNode.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase) && !parentNode.LocalName.Equals("th", StringComparison.OrdinalIgnoreCase); parentNode = parentNode.ParentNode)
    {
      if (parentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase))
        return true;
    }
    return false;
  }

  private void TraverseParagraphTag(XmlNode node)
  {
    this.ApplyParagraphStyle();
    this.ApplyParagraphFormat(node);
    if (this.m_currParagraph != null)
      this.ApplyTextFormatting(this.m_currParagraph.BreakCharacterFormat);
    this.TraverseChildNodes(node.ChildNodes);
  }

  private bool IsFirstNode(XmlNode node)
  {
    while (node.PreviousSibling != null)
    {
      if (node.PreviousSibling.Name == "#whitespace" || node.PreviousSibling.Name == "#significant-whitespace")
      {
        node = node.PreviousSibling;
      }
      else
      {
        if (node.PreviousSibling.NodeType == XmlNodeType.Text || !this.IsEmptyNode(node.PreviousSibling))
          return false;
        node = node.PreviousSibling;
      }
    }
    return true;
  }

  private void ParseTags(XmlNode node)
  {
    string lower = node.Name.ToLower();
    HTMLConverterImpl.TextFormat textFormat;
    switch (lower)
    {
      case "dir":
      case "body":
        this.TraverseChildNodes(node.ChildNodes);
        break;
      case "p":
        this.ParseParagraphTag(node);
        break;
      case "li":
      case "dt":
      case "dd":
      case "lh":
        if (lower == "li")
        {
          this.m_bIsWithinList = true;
          ++this.listCount;
        }
        bool style1 = this.ParseStyle(node);
        this.WriteParagraph(node);
        this.LeaveStyle(style1);
        if (lower == "li")
        {
          --this.listCount;
          if (this.listCount == 0)
            this.m_bIsWithinList = false;
        }
        this.m_currParagraph = (WParagraph) null;
        break;
      case "div":
        this.OnDivBegin(node);
        this.TraverseChildNodes(node.ChildNodes);
        this.OnDivEnd();
        break;
      case "h1":
      case "h2":
      case "h3":
      case "h4":
      case "h5":
      case "h6":
      case "h7":
        if (this.IsEmptyNode(node))
          break;
        this.ParseHeadingTag(this.EnsureStyle(node), node);
        break;
      case "table":
        this.OnTableBegin();
        this.ParseTable(node);
        this.OnTableEnd();
        break;
      case "img":
        textFormat = this.EnsureStyle(node);
        this.WriteImage(node);
        this.LeaveStyle(true);
        if (this.m_currParagraph == null)
          break;
        this.ApplyTextFormatting(this.m_currParagraph.BreakCharacterFormat);
        break;
      case "a":
        string name = string.Empty;
        textFormat = this.EnsureStyle(node);
        if (this.IsDefinedInline(node, "id"))
          name = this.GetAttributeValue(node, "id");
        else if (this.IsDefinedInline(node, "name"))
          name = this.GetAttributeValue(node, "name");
        if (this.GetAttributeValue(node, "href") == string.Empty && this.GetAttributeValue(node, "target") == string.Empty && name != string.Empty)
        {
          this.CurrentPara.AppendBookmarkStart(name);
          this.TraverseChildNodes(node.ChildNodes);
          this.CurrentPara.AppendBookmarkEnd(name);
        }
        else
          this.WriteHyperlink(node);
        this.LeaveStyle(true);
        if (this.m_currParagraph == null)
          break;
        this.ApplyTextFormatting(this.m_currParagraph.BreakCharacterFormat);
        break;
      case "br":
        bool style2 = this.ParseStyle(node);
        if (!this.IsDefinedInline(node, "page-break-before") && !this.IsDefinedInline(node, "page-break-after") && !this.IsDefinedInline(node, "page-break-inside"))
        {
          Break @break = this.CurrentPara.AppendBreak(BreakType.LineBreak);
          this.TraverseChildNodes(node.ChildNodes);
          string attributeValue = this.GetAttributeValue(node, "clear");
          if (attributeValue == "all" || attributeValue == "left" || attributeValue == "right" || node.ParentNode.LocalName.Equals("span", StringComparison.OrdinalIgnoreCase) && this.isPreserveBreakForInvalidStyles)
            @break.HtmlToDocLayoutInfo.RemoveLineBreak = false;
        }
        this.LeaveStyle(style2);
        break;
      case "blockquote":
        this.OnBlockquoteBegin(node);
        this.TraverseChildNodes(node.ChildNodes);
        this.OnBlockquoteEnd();
        break;
      case "title":
        this.TraverseChildNodes(node.ChildNodes);
        break;
      case "form":
        break;
      case "script":
        break;
      case "style":
        this.ParseCssStyle(node);
        break;
      case "select":
      case "input":
        this.ParseFormField(node);
        break;
      default:
        this.ParseFormattingTags(node);
        break;
    }
  }

  private void ParseFormField(XmlNode node)
  {
    if (this.m_currParagraph == null)
      this.AddNewParagraphToTextBody(node);
    int fieldType = -1;
    if (node.Name.Equals("select", StringComparison.OrdinalIgnoreCase))
      fieldType = 2;
    else if (node.Name.Equals("input", StringComparison.OrdinalIgnoreCase))
    {
      if (this.GetAttributeValue(node, "type").Equals("checkbox", StringComparison.OrdinalIgnoreCase))
        fieldType = 1;
      else if (this.GetAttributeValue(node, "type").Equals("text", StringComparison.OrdinalIgnoreCase))
        fieldType = 0;
    }
    WFormField dropDownFormField = this.InsertFormField(node, fieldType);
    if (dropDownFormField == null)
      return;
    switch (dropDownFormField.FormFieldType)
    {
      case FormFieldType.CheckBox:
        WCheckBox checkbox = dropDownFormField as WCheckBox;
        bool checkBoxProperties = this.ParseCheckBoxProperties(node, checkbox);
        this.ApplyTextFormatting(checkbox.CharacterFormat);
        this.LeaveStyle(checkBoxProperties);
        break;
      case FormFieldType.DropDown:
        this.ParseDropDownField(node, dropDownFormField as WDropDownFormField);
        break;
      default:
        string attributeValue = this.GetAttributeValue(node, "value");
        if (attributeValue != string.Empty)
        {
          (dropDownFormField as WTextFormField).Text = attributeValue;
          break;
        }
        (dropDownFormField as WTextFormField).Text = "     ";
        break;
    }
    this.CurrentPara.Items.OnInsertFormFieldComplete(this.CurrentPara.Items.Count - 1, (Entity) dropDownFormField);
  }

  private bool ParseCheckBoxProperties(XmlNode node, WCheckBox checkbox)
  {
    checkbox.Checked = this.IsNodeContainAttribute(node, "checked");
    string attributeValue = this.GetAttributeValue(node, "style");
    if (attributeValue.Length == 0 && this.CSSStyle == null)
      return false;
    HTMLConverterImpl.TextFormat textFormat = this.AddStyle();
    string[] strArray = attributeValue.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      string paramName = strArray[index].ToLower().Trim();
      string paramValue = strArray[index + 1].ToLower().Trim();
      switch (paramName)
      {
        case "-sf-size-type":
          continue;
        case "width":
        case "height":
          if (attributeValue.Contains("-sf-size-type"))
          {
            string styleAttributeValue = this.GetStyleAttributeValue(attributeValue, "-sf-size-type");
            checkbox.SizeType = styleAttributeValue == "auto" ? CheckBoxSizeType.Auto : CheckBoxSizeType.Exactly;
          }
          if (checkbox.SizeType == CheckBoxSizeType.Exactly)
          {
            checkbox.CheckBoxSize = (int) Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
            continue;
          }
          continue;
        default:
          this.GetFormat(textFormat, paramName, paramValue, node);
          continue;
      }
    }
    this.FindCSSstyleItem(node, textFormat);
    return true;
  }

  private WFormField InsertFormField(XmlNode node, int fieldType)
  {
    string attributeValue = this.GetAttributeValue(node, "name");
    WFormField formField = (WFormField) null;
    switch (fieldType)
    {
      case 0:
        formField = this.CurrentPara.AppendField(attributeValue, FieldType.FieldFormTextInput) as WFormField;
        break;
      case 1:
        formField = this.CurrentPara.AppendField(attributeValue, FieldType.FieldFormCheckBox) as WFormField;
        break;
      case 2:
        formField = this.CurrentPara.AppendField(attributeValue, FieldType.FieldFormDropDown) as WFormField;
        break;
    }
    if (formField != null)
      this.OnInsertFormField(formField);
    return formField;
  }

  internal void OnInsertFormField(WFormField formField)
  {
    switch (formField.FormFieldType)
    {
      case FormFieldType.TextInput:
        WTextFormField wtextFormField = formField as WTextFormField;
        if (wtextFormField.Name == null || wtextFormField.Name == string.Empty)
        {
          string str = "Text_" + Guid.NewGuid().ToString().Replace("-", "_");
          wtextFormField.Name = str.Substring(0, 20);
        }
        if (wtextFormField.DefaultText == null || wtextFormField.DefaultText == string.Empty)
        {
          wtextFormField.DefaultText = "     ";
          break;
        }
        break;
      case FormFieldType.CheckBox:
        WCheckBox wcheckBox = formField as WCheckBox;
        if (wcheckBox.Name == null || wcheckBox.Name == string.Empty)
        {
          string str = "Check_" + Guid.NewGuid().ToString().Replace("-", "_");
          wcheckBox.Name = str.Substring(0, 20);
          break;
        }
        break;
      case FormFieldType.DropDown:
        WDropDownFormField wdropDownFormField = formField as WDropDownFormField;
        if (wdropDownFormField.Name == null || wdropDownFormField.Name == string.Empty)
        {
          string str = "Drop_" + Guid.NewGuid().ToString().Replace("-", "_");
          wdropDownFormField.Name = str.Substring(0, 20);
          break;
        }
        break;
    }
    if (this.CurrentPara.Document == null)
      return;
    this.CurrentPara.Items.Insert(this.CurrentPara.ChildEntities.Count - 1, (IEntity) new BookmarkStart((IWordDocument) this.CurrentPara.Document, formField.Name));
  }

  private void ParseDropDownField(XmlNode node, WDropDownFormField dropDownFormField)
  {
    foreach (XmlNode childNode in node.ChildNodes)
    {
      switch (childNode.Name.ToLower())
      {
        case "option":
          if (childNode.InnerText == string.Empty)
            dropDownFormField.DropDownItems.Add(" ");
          else
            dropDownFormField.DropDownItems.Add(childNode.InnerText);
          if (this.IsNodeContainAttribute(childNode, "selected"))
          {
            dropDownFormField.DropDownSelectedIndex = dropDownFormField.DropDownItems.Count - 1;
            continue;
          }
          continue;
        default:
          this.ParseDropDownField(childNode, dropDownFormField);
          continue;
      }
    }
  }

  private bool IsNodeContainAttribute(XmlNode node, string attrName)
  {
    attrName = attrName.ToLower();
    for (int i = 0; i < node.Attributes.Count; ++i)
    {
      if (node.Attributes[i].LocalName.Equals(attrName, StringComparison.OrdinalIgnoreCase))
        return true;
    }
    return false;
  }

  private void ParseParagraphTag(XmlNode node)
  {
    if (!this.IsEmptyNode(node))
    {
      if ((!node.ParentNode.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase) || !this.IsFirstNode(node)) && (!node.ParentNode.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase) || !this.IsFirstNode(node)) && !this.isPreTag)
        this.AddNewParagraph(node);
      bool style = this.ParseStyle(node);
      this.TraverseParagraphTag(node);
      this.LeaveStyle(style);
    }
    this.m_currParagraph = (WParagraph) null;
  }

  private bool IsEmptyNode(XmlNode node)
  {
    bool flag = true;
    if (node.LocalName.Equals("img", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("a", StringComparison.OrdinalIgnoreCase))
      return false;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType != XmlNodeType.Whitespace && childNode.NodeType != XmlNodeType.SignificantWhitespace)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void ParseHeadingTag(HTMLConverterImpl.TextFormat tf, XmlNode node)
  {
    switch (node.LocalName.ToLower())
    {
      case "h1":
        tf.Style = BuiltinStyle.Heading1;
        if (!tf.HasKey(0))
          tf.FontSize = 24f;
        if (!tf.HasKey(1))
        {
          tf.FontFamily = this.isPreTag ? "Courier New" : "Times New Roman";
          break;
        }
        break;
      case "h2":
        tf.Style = BuiltinStyle.Heading2;
        if (!tf.HasKey(0))
          tf.FontSize = 18f;
        if (!tf.HasKey(1))
        {
          tf.FontFamily = this.isPreTag ? "Courier New" : "Times New Roman";
          break;
        }
        break;
      case "h3":
        tf.Style = BuiltinStyle.Heading3;
        if (!tf.HasKey(0))
          tf.FontSize = 13f;
        if (!tf.HasKey(1))
        {
          tf.FontFamily = this.isPreTag ? "Courier New" : "Times New Roman";
          break;
        }
        break;
      case "h4":
        tf.Style = BuiltinStyle.Heading4;
        if (!tf.HasKey(0))
          tf.FontSize = 12f;
        if (!tf.HasKey(1) && this.isPreTag)
        {
          tf.FontFamily = "Courier New";
          break;
        }
        break;
      case "h5":
        tf.Style = BuiltinStyle.Heading5;
        if (!tf.HasKey(0))
          tf.FontSize = 10f;
        if (!tf.HasKey(1) && this.isPreTag)
        {
          tf.FontFamily = "Courier New";
          break;
        }
        break;
      case "h6":
        tf.Style = BuiltinStyle.Heading6;
        if (!tf.HasKey(0))
          tf.FontSize = 7f;
        if (!tf.HasKey(1) && this.isPreTag)
        {
          tf.FontFamily = "Courier New";
          break;
        }
        break;
      case "h7":
        tf.Style = BuiltinStyle.Heading7;
        if (!tf.HasKey(0))
          tf.FontSize = 12f;
        if (!tf.HasKey(1) && this.isPreTag)
        {
          tf.FontFamily = "Courier New";
          break;
        }
        break;
    }
    this.WriteParagraph(node);
    this.LeaveStyle(true);
    this.m_currParagraph = (WParagraph) null;
  }

  private void OnBlockquoteBegin(XmlNode node)
  {
    this.m_currParagraph = (WParagraph) null;
    this.m_bIsInBlockquote = true;
    ++this.m_blockquoteLevel;
    this.OnDivBegin(node);
  }

  private void OnBlockquoteEnd()
  {
    --this.m_blockquoteLevel;
    if (this.m_blockquoteLevel == 0)
      this.m_bIsInBlockquote = false;
    this.OnDivEnd();
    this.m_currParagraph = (WParagraph) null;
  }

  private void OnDivBegin(XmlNode node)
  {
    this.currDivFormat = (HTMLConverterImpl.TextFormat) null;
    ++this.m_divCount;
    if (this.m_bIsInDiv)
      this.m_currParagraph = (WParagraph) null;
    this.m_bIsInDiv = true;
    HorizontalAlignment horizontalAlignment = this.GetHorizontalAlignment(this.GetAttributeValue(node, "align"));
    if (this.ParseStyle(node))
      this.currDivFormat = this.m_styleStack.Peek();
    else if (this.m_bIsInDiv && this.m_styleStack.Count > 0)
    {
      this.m_styleStack.Push(this.m_styleStack.Peek().Clone());
      this.currDivFormat = this.m_styleStack.Peek();
    }
    if (this.currDivFormat != null && !string.IsNullOrEmpty(this.GetAttributeValue(node, "align")))
    {
      this.currDivFormat.TextAlign = horizontalAlignment;
    }
    else
    {
      if (this.currDivFormat != null || horizontalAlignment == HorizontalAlignment.Left)
        return;
      this.currDivFormat = new HTMLConverterImpl.TextFormat();
      if (this.currDivFormat.HasKey(10))
        return;
      this.currDivFormat.TextAlign = horizontalAlignment;
    }
  }

  private void OnDivEnd()
  {
    --this.m_divCount;
    this.m_bIsInDiv = this.m_divCount != 0;
    if (this.m_styleStack.Count > 0)
      this.m_styleStack.Pop();
    this.currDivFormat = this.m_styleStack.Count <= 0 || !this.m_bIsInDiv ? (HTMLConverterImpl.TextFormat) null : this.m_styleStack.Peek();
    this.m_currParagraph = (WParagraph) null;
  }

  private void OnTableEnd()
  {
    if (this.m_currTable != null)
      this.m_currTable.UpdateGridSpan();
    if (this.m_currTable.DocxTableFormat.StyleName == null)
      this.m_currTable.DocxTableFormat.StyleName = string.Empty;
    this.m_currTable = this.m_nestedTable.Pop();
    this.m_bodyItems = this.m_nestedBodyItems.Pop();
    this.IsCellStyle = this.m_stackCellStyle.Pop();
    this.IsRowStyle = this.m_stackRowStyle.Pop();
    this.IsTableStyle = this.m_stackTableStyle.Pop();
    this.m_currParagraph = (WParagraph) null;
  }

  private void OnTableBegin()
  {
    this.m_nestedBodyItems.Push(this.m_bodyItems);
    this.m_nestedTable.Push(this.m_currTable);
    this.m_stackCellStyle.Push(this.IsCellStyle);
    this.m_stackRowStyle.Push(this.IsRowStyle);
    this.m_stackTableStyle.Push(this.IsTableStyle);
  }

  private void WriteHyperlink(XmlNode node)
  {
    string text = this.GetAttributeValue(node, "href");
    IWField wfield = this.TraverseHyperlinkField(node);
    HyperlinkType hyperlinkType;
    if (this.StartsWithExt(text, "#"))
    {
      hyperlinkType = HyperlinkType.Bookmark;
      text = text.Replace("#", string.Empty);
    }
    else
      hyperlinkType = !this.StartsWithExt(text, "mailto:") ? (this.StartsWithExt(text, "http") || this.StartsWithExt(text, "www") ? HyperlinkType.WebLink : HyperlinkType.FileLink) : HyperlinkType.EMailLink;
    Hyperlink hyperlink = new Hyperlink(wfield as WField);
    hyperlink.Type = hyperlinkType;
    if (hyperlinkType == HyperlinkType.WebLink || hyperlinkType == HyperlinkType.EMailLink)
      hyperlink.Uri = text;
    else if (hyperlink.Type == HyperlinkType.Bookmark)
      hyperlink.BookmarkName = text;
    else if (hyperlink.Type == HyperlinkType.FileLink)
      hyperlink.FilePath = text;
    this.ApplyHyperlinkStyle(wfield as WField);
  }

  private IWField TraverseHyperlinkField(XmlNode node)
  {
    WField wfield = new WField((IWordDocument) this.CurrentPara.Document);
    wfield.FieldType = FieldType.FieldHyperlink;
    this.CurrentPara.Items.Add((IEntity) wfield);
    this.CurrentPara.AppendText(string.Empty);
    this.CurrentPara.AppendFieldMark(FieldMarkType.FieldSeparator);
    wfield.FieldSeparator = this.CurrentPara.LastItem as WFieldMark;
    this.TraverseChildNodes(node.ChildNodes);
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.CurrentPara.Document, FieldMarkType.FieldEnd);
    this.CurrentPara.Items.Add((IEntity) wfieldMark);
    wfield.FieldEnd = wfieldMark;
    return (IWField) wfield;
  }

  private void ApplyHyperlinkStyle(WField field)
  {
    WParagraph ownerParagraph = field.OwnerParagraph;
    int num = ownerParagraph.Items.IndexOf((IEntity) field);
    bool flag = false;
    for (int index = num; index < ownerParagraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem = ownerParagraph.Items[index];
      if (paragraphItem.EntityType == EntityType.FieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldSeparator)
        flag = true;
      else if (paragraphItem.EntityType == EntityType.TextRange && flag)
      {
        if (this.m_hyperlinkcolor != Color.Empty)
          (paragraphItem as WTextRange).CharacterFormat.TextColor = this.m_hyperlinkcolor;
        if (this.m_bodyItems.Document.Styles.FindByName(BuiltinStyle.Hyperlink.ToString()) == null)
          this.m_bodyItems.Document.Styles.Add(Style.CreateBuiltinStyle(BuiltinStyle.Hyperlink, StyleType.CharacterStyle, this.m_bodyItems.Document));
        (paragraphItem as WTextRange).CharacterFormat.CharStyleName = BuiltinStyle.Hyperlink.ToString();
        this.ApplyTextFormatting((paragraphItem as WTextRange).CharacterFormat);
      }
      else if (paragraphItem.EntityType == EntityType.FieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd)
        break;
    }
  }

  private void ParseImageAttribute(
    XmlNode node,
    IWPicture pic,
    ref bool isHeightSpecified,
    ref bool isWidthSpecified)
  {
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
    {
      switch (attribute.Name.ToLower())
      {
        case "height":
          if (attribute.Value != "auto" && attribute.Value != "inherit" && attribute.Value != "initial")
          {
            pic.Height = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
            isHeightSpecified = true;
            continue;
          }
          continue;
        case "width":
          if (attribute.Value != "auto" && attribute.Value != "inherit" && attribute.Value != "initial")
          {
            pic.Width = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
            isWidthSpecified = true;
            continue;
          }
          continue;
        case "style":
          this.ParseImageStyleAttribute(attribute, pic, ref isHeightSpecified, ref isWidthSpecified);
          continue;
        case "align":
          switch (attribute.Value)
          {
            case "top":
              pic.VerticalAlignment = ShapeVerticalAlignment.Top;
              continue;
            case "bottom":
              pic.VerticalAlignment = ShapeVerticalAlignment.Bottom;
              continue;
            case "middle":
              pic.HorizontalAlignment = ShapeHorizontalAlignment.Center;
              continue;
            case "left":
              pic.HorizontalAlignment = ShapeHorizontalAlignment.Left;
              continue;
            case "right":
              pic.HorizontalAlignment = ShapeHorizontalAlignment.Right;
              (pic as WPicture).SetTextWrappingStyleValue(TextWrappingStyle.Square);
              continue;
            default:
              continue;
          }
        case "alt":
          if (attribute.Value != null && attribute.Value != string.Empty)
          {
            pic.AlternativeText = attribute.Value.Trim();
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void ParseImageStyleAttribute(
    XmlAttribute attr,
    IWPicture pic,
    ref bool isHeightSpecified,
    ref bool isWidthSpecified)
  {
    if (!attr.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
      return;
    string[] strArray = attr.Value.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      string str1 = strArray[index].ToLower().Trim();
      string str2 = strArray[index + 1].ToLower().Trim();
      switch (str1)
      {
        case "height":
          if (str2 != "auto" && str2 != "inherit" && str2 != "initial")
          {
            pic.Height = Convert.ToSingle(this.ExtractValue(str2), (IFormatProvider) CultureInfo.InvariantCulture);
            isHeightSpecified = true;
            break;
          }
          break;
        case "width":
          if (str2 != "auto" && str2 != "inherit" && str2 != "initial")
          {
            pic.Width = Convert.ToSingle(this.ExtractValue(str2), (IFormatProvider) CultureInfo.InvariantCulture);
            isWidthSpecified = true;
            break;
          }
          break;
      }
    }
  }

  private void WriteImage(XmlNode node)
  {
    if (this.m_currParagraph == null && this.m_bIsInDiv && (node.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) && this.IsFirstNode(node) || node.ParentNode.LocalName.Equals("span", StringComparison.OrdinalIgnoreCase)))
      this.AddNewParagraph(node);
    string attributeValue = this.GetAttributeValue(node, "src");
    bool isHeightSpecified = false;
    bool isWidthSpecified = false;
    IWPicture pic = (IWPicture) new WPicture((IWordDocument) this.m_bodyItems.Document);
    if (WordDocument.EnablePartialTrustCode)
    {
      this.GetImageForPartialTrustMode(attributeValue, pic);
      if (pic.ImageBytes != null)
      {
        this.CurrentPara.Items.Add((IEntity) pic);
        this.ParseImageAttribute(node, pic, ref isHeightSpecified, ref isWidthSpecified);
      }
    }
    else
    {
      pic = this.CurrentPara.AppendPicture(this.GetImage(attributeValue));
      pic.Width = (float) pic.Image.Width * 0.75f;
      pic.Height = (float) pic.Image.Height * 0.75f;
      this.ParseImageAttribute(node, pic, ref isHeightSpecified, ref isWidthSpecified);
    }
    this.UpdateHeightAndWidth(pic as WPicture, isHeightSpecified, isWidthSpecified);
    this.ApplyTextFormatting((pic as ParagraphItem).ParaItemCharFormat);
  }

  private void GetImageForPartialTrustMode(string src, IWPicture pic)
  {
    try
    {
      if (this.HtmlImportSettings.IsEventSubscribed)
      {
        Image image = Image.FromStream(this.HtmlImportSettings.ExecuteImageNodeVisitedEvent((Stream) null, src).ImageStream);
        MemoryStream memoryStream = new MemoryStream();
        image.Save((Stream) memoryStream, image.RawFormat);
        byte[] array = memoryStream.ToArray();
        memoryStream.Close();
        pic.LoadImage(array);
      }
      else if (this.StartsWithExt(src, "data:image/"))
      {
        int num = src.IndexOf(",");
        src = src.Substring(num + 1);
        byte[] imageBytes = Convert.FromBase64String(src);
        pic.LoadImage(imageBytes);
      }
      else if (this.StartsWithExt(src, "http") || this.StartsWithExt(src, "ftp"))
      {
        byte[] imageBytes = new WebClient().DownloadData(src);
        pic.LoadImage(imageBytes);
      }
      else if (System.IO.File.Exists(src))
      {
        byte[] imageBytes = this.ReadImageFile(src);
        pic.LoadImage(imageBytes);
      }
      else if (System.IO.File.Exists($"{this.BasePath}\\{src}"))
      {
        byte[] imageBytes = this.ReadImageFile($"{this.BasePath}\\{src}");
        pic.LoadImage(imageBytes);
      }
    }
    catch
    {
      Stream manifestResourceStream = this.GetManifestResourceStream("ImageNotFound.jpg");
      pic.LoadImage(this.ReadFully(manifestResourceStream));
      manifestResourceStream.Dispose();
    }
    if (pic.ImageBytes != null)
      return;
    Stream manifestResourceStream1 = this.GetManifestResourceStream("ImageNotFound.jpg");
    pic.LoadImage(this.ReadFully(manifestResourceStream1));
    manifestResourceStream1.Dispose();
  }

  private byte[] ReadFully(Stream input)
  {
    byte[] buffer = new byte[input.Length];
    using (MemoryStream memoryStream = new MemoryStream())
    {
      int count;
      while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
        memoryStream.Write(buffer, 0, count);
      return memoryStream.ToArray();
    }
  }

  private byte[] ReadImageFile(string imageLocation)
  {
    long length = new FileInfo(imageLocation).Length;
    return new BinaryReader((Stream) new FileStream(imageLocation, FileMode.Open, FileAccess.Read)).ReadBytes((int) length);
  }

  private Image GetImage(string src)
  {
    Image image = (Image) null;
    try
    {
      if (this.HtmlImportSettings.IsEventSubscribed)
        image = Image.FromStream(this.HtmlImportSettings.ExecuteImageNodeVisitedEvent((Stream) null, src).ImageStream);
      else if (this.StartsWithExt(src, "data:image/"))
      {
        int num = src.IndexOf(",");
        src = src.Substring(num + 1);
        image = Image.FromStream((Stream) new MemoryStream(Convert.FromBase64String(src)));
      }
      else if (this.StartsWithExt(src, "http") || this.StartsWithExt(src, "ftp"))
      {
        WebRequest webRequest = WebRequest.Create(src);
        webRequest.Method = "GET";
        image = Image.FromStream(webRequest.GetResponse().GetResponseStream());
      }
      else if (System.IO.File.Exists(src))
        image = Image.FromFile(src);
      else if (System.IO.File.Exists($"{this.BasePath}\\{src}"))
        image = Image.FromFile($"{this.BasePath}\\{src}");
    }
    catch
    {
      if (this.StartsWithExt(src, "http") || this.StartsWithExt(src, "ftp"))
      {
        if (this.HtmlImportSettings != null)
        {
          ImageDownloadingFailedEventArgs args = this.HtmlImportSettings.ExecuteImageDownloadingFailedEvent(src);
          if (!string.IsNullOrEmpty(args.UserName))
            return this.TryDownloadingFailedImage(src, args);
        }
      }
      image = Image.FromStream(this.GetManifestResourceStream("ImageNotFound.jpg"));
    }
    if (image == null)
      image = Image.FromStream(this.GetManifestResourceStream("ImageNotFound.jpg"));
    return image;
  }

  private Image TryDownloadingFailedImage(string src, ImageDownloadingFailedEventArgs args)
  {
    try
    {
      WebRequest webRequest = WebRequest.Create(src);
      webRequest.Method = "GET";
      webRequest.Credentials = (ICredentials) new NetworkCredential(args.UserName, args.Password);
      return Image.FromStream(webRequest.GetResponse().GetResponseStream());
    }
    catch
    {
      return Image.FromStream(this.GetManifestResourceStream("ImageNotFound.jpg"));
    }
  }

  private Stream GetManifestResourceStream(string fileName)
  {
    Assembly executingAssembly = Assembly.GetExecutingAssembly();
    foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
    {
      if (manifestResourceName.EndsWith("." + fileName))
      {
        fileName = manifestResourceName;
        break;
      }
    }
    return executingAssembly.GetManifestResourceStream(fileName);
  }

  private void ParseFormattingTags(XmlNode tag)
  {
    if (this.m_curListLevel < 0)
      this.m_listLeftIndentStack.Clear();
    HTMLConverterImpl.TextFormat format = this.EnsureStyle(tag);
    switch (tag.Name.ToLower())
    {
      case "b":
      case "strong":
        format.Bold = true;
        break;
      case "i":
      case "em":
      case "cite":
      case "dfn":
      case "var":
        format.Italic = true;
        break;
      case "u":
        format.Underline = true;
        break;
      case "s":
      case "strike":
        format.Strike = true;
        break;
      case "small":
        if ((double) format.FontSize < 0.0)
          format.FontSize = 10f;
        format.FontSize -= 2f;
        break;
      case "big":
        format.FontSize += 2f;
        break;
      case "code":
      case "tt":
      case "samp":
        format.FontFamily = "Courier New";
        format.FontSize = 10f;
        break;
      case "pre":
        this.isPreTag = true;
        format.Style = BuiltinStyle.HtmlPreformatted;
        this.AddNewParagraph(tag);
        this.ApplyParagraphStyle();
        break;
      case "font":
        string attributeValue1 = this.GetAttributeValue(tag, "color");
        string attributeValue2 = this.GetAttributeValue(tag, "face");
        if (attributeValue1.Length > 0)
          format.FontColor = this.GetColor(attributeValue1);
        if (attributeValue2.Length > 0)
          format.FontFamily = attributeValue2;
        string attributeValue3 = this.GetAttributeValue(tag, "size");
        if (attributeValue3.Length > 0)
        {
          this.ApplyFontSize(attributeValue3, format);
          break;
        }
        break;
      case "ul":
        ++this.m_curListLevel;
        this.SetListMode(true, tag, format);
        if (!this.IsDefinedInline(tag, "margin-left") && !this.IsDefinedInline(tag, "margin") && this.m_curListLevel >= 0)
          this.UpdateListLeftIndentStack(0.0f, false);
        if (this.m_curListLevel == 0 || this.isLastLevelSkipped && this.lastSkippedLevelNo != -1 && this.lastSkippedLevelNo <= this.lastUsedLevelNo && this.m_curListLevel <= this.lastUsedLevelNo)
        {
          this.CreateListStyle(tag);
          break;
        }
        break;
      case "ol":
        ++this.m_curListLevel;
        this.SetListMode(false, tag, format);
        if (!this.IsDefinedInline(tag, "margin-left") && !this.IsDefinedInline(tag, "margin") && this.m_curListLevel >= 0)
          this.UpdateListLeftIndentStack(0.0f, false);
        if (this.m_curListLevel == 0 || this.isLastLevelSkipped && this.lastSkippedLevelNo != -1 && this.lastSkippedLevelNo <= this.lastUsedLevelNo && this.m_curListLevel <= this.lastUsedLevelNo)
        {
          this.CreateListStyle(tag);
          break;
        }
        break;
      case "a":
        format.FontColor = Color.Blue;
        format.Underline = true;
        break;
      case "sup":
        format.SubSuperScript = SubSuperScript.SuperScript;
        break;
      case "sub":
        format.SubSuperScript = SubSuperScript.SubScript;
        break;
      case "del":
        format.Strike = true;
        break;
    }
    this.TraverseChildNodes(tag.ChildNodes);
    if (this.isPreserveBreakForInvalidStyles)
      this.isPreserveBreakForInvalidStyles = false;
    if (this.currDivFormat != null && tag.LocalName.Equals("label", StringComparison.OrdinalIgnoreCase) && this.m_currParagraph != null && (!this.m_bIsInDiv || !this.m_currParagraph.IsInCell || this.NodeIsInDiv(tag)))
      this.ApplyDivParagraphFormat(tag);
    if (tag.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase) || tag.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase))
    {
      --this.m_curListLevel;
      if (this.m_listLeftIndentStack.Count > 0)
      {
        double num = (double) this.m_listLeftIndentStack.Pop();
      }
      if (this.m_curListLevel < 0 && this.ListStack.Count > 0)
        this.ListStack.Pop();
      if (this.LfoStack.Count > 0)
        this.LfoStack.Pop();
      if (this.m_curListLevel < 0)
        this.m_listLevelNo.Clear();
    }
    if (tag.LocalName.Equals("pre", StringComparison.OrdinalIgnoreCase))
      this.isPreTag = false;
    this.LeaveStyle(true);
  }

  private void UpdateListLeftIndentStack(float leftIndent, bool isInlineLeftIndent)
  {
    if (this.m_listLeftIndentStack.Count > 0)
    {
      if (isInlineLeftIndent)
        this.m_listLeftIndentStack.Push(this.m_listLeftIndentStack.Peek() + leftIndent);
      else
        this.m_listLeftIndentStack.Push(this.m_listLeftIndentStack.Peek() + 36f);
    }
    else if (isInlineLeftIndent)
      this.m_listLeftIndentStack.Push(leftIndent);
    else
      this.m_listLeftIndentStack.Push(36f);
  }

  private void ApplyFontSize(string value, HTMLConverterImpl.TextFormat format)
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    if (this.StartsWithExt(value, "+"))
    {
      flag1 = true;
      value = value.Substring(1, value.Length - 1);
    }
    else if (this.StartsWithExt(value, "-"))
    {
      flag2 = true;
      value = value.Substring(1, value.Length - 1);
    }
    for (int index = 0; index < value.Length; ++index)
    {
      if (char.IsDigit(value[index]))
      {
        flag3 = false;
      }
      else
      {
        flag3 = true;
        break;
      }
    }
    if (flag3)
    {
      format.FontSize = 12f;
    }
    else
    {
      int num = Convert.ToInt32(value);
      if (flag1)
        num = 3 + num;
      else if (flag2)
        num = 3 - num;
      if (num <= 1)
        format.FontSize = 7.5f;
      else if (num >= 7)
      {
        format.FontSize = 36f;
      }
      else
      {
        switch (num - 2)
        {
          case 0:
            format.FontSize = 10f;
            break;
          case 1:
            format.FontSize = 12f;
            break;
          case 2:
            format.FontSize = 13.5f;
            break;
          case 3:
            format.FontSize = 18f;
            break;
          case 4:
            format.FontSize = 24f;
            break;
        }
      }
    }
  }

  private void SetListMode(bool isBulleted, XmlNode node, HTMLConverterImpl.TextFormat format)
  {
    if (isBulleted)
      format.DefBulleted = true;
    else
      format.NumBulleted = true;
  }

  private void WriteParagraph(XmlNode node)
  {
    if (node.ParentNode.LocalName != "li" && (!node.ParentNode.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase) || !this.IsFirstNode(node)) || this.IsHeadingStyle() && this.m_currParagraph == null)
      this.AddNewParagraph(node);
    this.TraverseParagraphTag(node);
  }

  private bool IsHeadingStyle()
  {
    HTMLConverterImpl.TextFormat currentFormat = this.CurrentFormat;
    bool flag = false;
    switch (currentFormat.Style)
    {
      case BuiltinStyle.Heading1:
      case BuiltinStyle.Heading2:
      case BuiltinStyle.Heading3:
      case BuiltinStyle.Heading4:
      case BuiltinStyle.Heading5:
      case BuiltinStyle.Heading6:
      case BuiltinStyle.Heading7:
        flag = true;
        break;
    }
    return flag;
  }

  private void ApplyParagraphStyle()
  {
    HTMLConverterImpl.TextFormat currentFormat = this.CurrentFormat;
    if (this.m_currParagraph == null)
      return;
    if (currentFormat.Style != BuiltinStyle.Normal)
    {
      int count = this.m_currParagraph.Document.Styles.Count;
      this.m_currParagraph.ApplyStyle(currentFormat.Style, false);
      IWParagraphStyle paraStyle = this.m_currParagraph.ParaStyle;
      if (paraStyle.ParagraphFormat.KeepFollow)
        paraStyle.ParagraphFormat.KeepFollow = false;
      if (!this.IsHeadingStyle())
        return;
      if (count == this.m_currParagraph.Document.Styles.Count)
      {
        if (!currentFormat.HasKey(2))
          currentFormat.Bold = currentFormat.Style != BuiltinStyle.Heading7;
        if (currentFormat.HasKey(4))
          return;
        currentFormat.Italic = false;
      }
      else
      {
        bool flag = currentFormat.Style != BuiltinStyle.Heading7;
        paraStyle.CharacterFormat.Bold = flag;
        paraStyle.CharacterFormat.BoldBidi = flag;
        paraStyle.CharacterFormat.Italic = false;
        paraStyle.CharacterFormat.ItalicBidi = false;
      }
    }
    else if (this.m_userStyle != null)
      this.m_currParagraph.ApplyStyle(this.m_userStyle, false);
    else
      this.m_currParagraph.ApplyStyle(BuiltinStyle.NormalWeb, false);
  }

  private void ApplyParagraphFormat(XmlNode node)
  {
    if (this.m_currParagraph == null)
      return;
    if (!this.m_bIsInDiv || !this.m_currParagraph.IsInCell)
      this.ApplyDivParagraphFormat(node);
    WParagraphFormat paragraphFormat = this.m_currParagraph.ParagraphFormat;
    HTMLConverterImpl.TextFormat currentFormat = this.CurrentFormat;
    if (!node.Name.Equals("th", StringComparison.OrdinalIgnoreCase) && !node.Name.Equals("td", StringComparison.OrdinalIgnoreCase))
      this.ApplyParagraphBorder(paragraphFormat, currentFormat);
    currentFormat.Borders = new HTMLConverterImpl.TableBorders();
    if (node.LocalName.Equals("dd", StringComparison.OrdinalIgnoreCase))
      this.m_currParagraph.ParagraphFormat.SetPropertyValue(2, (object) 36f);
    if (currentFormat.HasValue(10))
      paragraphFormat.HorizontalAlignment = currentFormat.TextAlign;
    if (this.m_currParagraph.IsInCell && currentFormat.HasValue(10))
      this.m_currParagraph.ParagraphFormat.HorizontalAlignment = currentFormat.TextAlign;
    this.ApplyListFormatting(paragraphFormat, currentFormat, node);
    if (currentFormat.HasValue(8))
    {
      paragraphFormat.LineSpacingRule = currentFormat.LineSpacingRule;
      paragraphFormat.SetPropertyValue(52, (object) currentFormat.LineHeight);
    }
    if (currentFormat.HasValue(25))
      paragraphFormat.Keep = currentFormat.KeepLinesTogether;
    if (currentFormat.HasValue(26))
      paragraphFormat.KeepFollow = currentFormat.KeepWithNext;
    if (this.m_bIsWithinList)
    {
      if (!this.m_currParagraph.IsInCell || this.m_currParagraph.ListFormat.CurrentListStyle != null)
        paragraphFormat.SetPropertyValue(2, (object) this.AdjustLeftIndentForList(node, currentFormat));
    }
    else if (currentFormat.HasValue(12) && (double) currentFormat.LeftMargin > 0.0)
      paragraphFormat.SetPropertyValue(2, (object) currentFormat.LeftMargin);
    if (currentFormat.HasValue(15))
      paragraphFormat.SetPropertyValue(5, (object) currentFormat.TextIndent);
    if (currentFormat.HasValue(14))
      paragraphFormat.SetPropertyValue(3, (object) currentFormat.RightMargin);
    if (this.IsBottomMarginNeedToBePreserved(node, currentFormat))
    {
      if (currentFormat.HasValue(27))
      {
        paragraphFormat.SpaceAfterAuto = currentFormat.AfterSpaceAuto;
      }
      else
      {
        paragraphFormat.SetPropertyValue(9, (object) currentFormat.BottomMargin);
        paragraphFormat.SpaceAfterAuto = false;
      }
    }
    else if (!node.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase) && !this.isPreTag)
      paragraphFormat.SpaceAfterAuto = true;
    if (this.IsTopMarginNeedToBePreserved(node, currentFormat))
    {
      if (currentFormat.HasValue(28))
      {
        paragraphFormat.SpaceBeforeAuto = currentFormat.BeforeSpaceAuto;
      }
      else
      {
        paragraphFormat.SetPropertyValue(8, (object) currentFormat.TopMargin);
        paragraphFormat.SpaceBeforeAuto = false;
      }
    }
    else if (!node.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase) && !this.isPreTag || node.LocalName.Equals("p", StringComparison.OrdinalIgnoreCase))
      paragraphFormat.SpaceBeforeAuto = true;
    if (currentFormat.HasValue(17))
    {
      paragraphFormat.PageBreakBefore = currentFormat.PageBreakBefore;
      paragraphFormat.KeepFollow = true;
    }
    if (currentFormat.HasValue(18))
      paragraphFormat.PageBreakAfter = currentFormat.PageBreakAfter;
    if (currentFormat.HasValue(22))
      paragraphFormat.WordWrap = currentFormat.WordWrap;
    if (currentFormat.HasValue(7))
      paragraphFormat.BackColor = currentFormat.BackColor;
    this.UpdateParaFormat(node, paragraphFormat);
    if (this.m_bIsInBlockquote && !node.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase))
      paragraphFormat.SetPropertyValue(2, (object) (float) ((double) this.CurrentPara.ParagraphFormat.LeftIndent + (double) this.m_blockquoteLevel * 36.0));
    if (!node.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase))
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.LocalName.Equals("#text", StringComparison.OrdinalIgnoreCase) || childNode.LocalName.Equals("strong", StringComparison.OrdinalIgnoreCase))
        break;
      if (childNode.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase))
      {
        paragraphFormat.SetPropertyValue(2, (object) (float) ((double) this.CurrentPara.ParagraphFormat.LeftIndent + 36.0));
        break;
      }
    }
  }

  private float AdjustLeftIndentForList(XmlNode node, HTMLConverterImpl.TextFormat format)
  {
    if (!this.IsDefinedInline(node, "margin-left"))
      format.LeftMargin = 0.0f;
    return format.LeftMargin + (this.m_listLeftIndentStack.Count > 0 ? this.m_listLeftIndentStack.Peek() : 0.0f);
  }

  private bool IsBottomMarginNeedToBePreserved(XmlNode node, HTMLConverterImpl.TextFormat format)
  {
    if (format.HasKey(13))
    {
      if (this.IsDefinedInline(node, "margin-bottom") || this.IsDefinedInline(node, "margin") || this.IsDefinedInline(node, "padding") || this.IsDefinedInline(node, "padding-bottom"))
        return true;
      if (!node.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) || !this.IsLastNode(node))
        return false;
      for (XmlNode parentNode = node.ParentNode; parentNode != null && parentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase); parentNode = parentNode.ParentNode)
      {
        if (this.IsDefinedInline(parentNode, "margin-bottom") || this.IsDefinedInline(parentNode, "margin") || this.IsDefinedInline(parentNode, "padding") || this.IsDefinedInline(parentNode, "padding-bottom"))
          return true;
        if (!parentNode.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) || !this.IsLastNode(parentNode))
          return false;
      }
    }
    return false;
  }

  private bool IsTopMarginNeedToBePreserved(XmlNode node, HTMLConverterImpl.TextFormat format)
  {
    if (format.HasKey(11))
    {
      if (this.IsDefinedInline(node, "margin-top") || this.IsDefinedInline(node, "margin") || this.IsDefinedInline(node, "padding") || this.IsDefinedInline(node, "padding-top"))
        return true;
      if (!node.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) || !this.IsFirstNode(node))
        return false;
      for (XmlNode parentNode = node.ParentNode; parentNode != null && parentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase); parentNode = parentNode.ParentNode)
      {
        if (this.IsDefinedInline(parentNode, "margin-top") || this.IsDefinedInline(parentNode, "margin") || this.IsDefinedInline(parentNode, "padding") || this.IsDefinedInline(parentNode, "padding-top"))
          return true;
        if (!parentNode.ParentNode.LocalName.Equals("div", StringComparison.OrdinalIgnoreCase) || !this.IsFirstNode(parentNode))
          return false;
      }
    }
    return false;
  }

  private bool IsLastNode(XmlNode node)
  {
    while (node.NextSibling != null)
    {
      if (node.NextSibling.Name == "#whitespace" || node.NextSibling.Name == "#significant-whitespace")
      {
        node = node.NextSibling;
      }
      else
      {
        if (node.NextSibling.NodeType == XmlNodeType.Text || !this.IsEmptyNode(node.NextSibling))
          return false;
        node = node.NextSibling;
      }
    }
    return true;
  }

  private bool IsDefinedInline(XmlNode node, string attName)
  {
    string empty = string.Empty;
    if (node.Attributes != null && node.Attributes.Count > 0)
    {
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
      {
        if (attribute.LocalName.Equals("style", StringComparison.OrdinalIgnoreCase))
        {
          string[] strArray = this.GetAttributeValue(node, "style").Split(';', ':');
          int index = 0;
          for (int length = strArray.Length; index < length - 1; index += 2)
          {
            if (strArray[index].ToLower().Trim() == attName)
              return true;
          }
        }
        else if (attribute.LocalName.Equals(attName, StringComparison.OrdinalIgnoreCase))
          return true;
      }
    }
    return false;
  }

  private void ApplyListFormatting(
    WParagraphFormat pformat,
    HTMLConverterImpl.TextFormat format,
    XmlNode node)
  {
    if (format.NumBulleted && node.Name.ToUpper() != "LH" && node.Name.Equals("li", StringComparison.OrdinalIgnoreCase) && node.ParentNode.Name.Equals("ol", StringComparison.OrdinalIgnoreCase))
    {
      bool flag = false;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.ParentNode.Attributes)
      {
        if (attribute.Name.Equals("type", StringComparison.OrdinalIgnoreCase))
        {
          this.BuildListStyle(this.GetListPatternType(attribute.Value), node);
          flag = true;
        }
        else if (attribute.Name.Equals("style", StringComparison.OrdinalIgnoreCase) && this.IsDefinedInline(node.ParentNode, "list-style-type"))
        {
          ListPatternType listPatternType = this.GetListPatternType(this.GetStyleAttributeValue(attribute.Value.ToLower(), "list-style-type"));
          if (listPatternType != ListPatternType.None)
            this.BuildListStyle(listPatternType, node);
          flag = true;
        }
        else if (pformat.Document.HTMLImportSettings != null && pformat.Document.HTMLImportSettings.IsConsiderListStyleAttribute && attribute.Name.Equals("style", StringComparison.OrdinalIgnoreCase) && attribute.Value.Equals("list-style:none", StringComparison.OrdinalIgnoreCase))
        {
          this.lastSkippedLevelNo = this.m_curListLevel;
          this.isLastLevelSkipped = true;
          flag = true;
        }
      }
      if (flag)
        return;
      this.BuildListStyle(ListPatternType.Arabic, node);
    }
    else if (node.ParentNode.Name.Equals("ul", StringComparison.OrdinalIgnoreCase) && format.DefBulleted && node.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("lh", StringComparison.OrdinalIgnoreCase))
    {
      bool flag = false;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.ParentNode.Attributes)
      {
        if (attribute.Name.Equals("style", StringComparison.OrdinalIgnoreCase) && this.IsDefinedInline(node.ParentNode, "list-style-image"))
        {
          this.BuildListStyle(ListPatternType.Bullet, node);
          string src = this.GetStyleAttributeValue(attribute.Value.ToLower(), "list-style-image").Replace("url('", string.Empty).Replace("')", string.Empty);
          WPicture wpicture = new WPicture((IWordDocument) this.m_bodyItems.Document);
          Image image = this.GetImage(src);
          wpicture.LoadImage(image);
          this.m_currParagraph.ListFormat.CurrentListLevel.PicBullet = wpicture;
          flag = true;
        }
        else if (pformat.Document.HTMLImportSettings != null && pformat.Document.HTMLImportSettings.IsConsiderListStyleAttribute && attribute.Name.Equals("style", StringComparison.OrdinalIgnoreCase) && (attribute.Value.Equals("list-style:none", StringComparison.OrdinalIgnoreCase) || attribute.Value.Equals("list-style-type:none", StringComparison.OrdinalIgnoreCase)))
        {
          this.lastSkippedLevelNo = this.m_curListLevel;
          this.isLastLevelSkipped = true;
          flag = true;
        }
      }
      if (flag)
        return;
      this.BuildListStyle(ListPatternType.Bullet, node);
    }
    else
    {
      if (!(node.Name.ToUpper() == "LH"))
        return;
      pformat.SetPropertyValue(2, (object) 35f);
    }
  }

  private ListPatternType GetListPatternType(string attrValue)
  {
    switch (attrValue)
    {
      case "lower-alpha":
      case "a":
        return ListPatternType.LowLetter;
      case "upper-alpha":
      case "A":
        return ListPatternType.UpLetter;
      case "lower-roman":
      case "i":
        return ListPatternType.LowRoman;
      case "upper-roman":
      case "I":
        return ListPatternType.UpRoman;
      case "decimal-leading-zero":
      case "decimal":
        return ListPatternType.Arabic;
      case "none":
        return ListPatternType.None;
      default:
        return ListPatternType.Arabic;
    }
  }

  private void ApplyParagraphBorder(WParagraphFormat pformat, HTMLConverterImpl.TextFormat format)
  {
    if (format.Borders.AllStyle != BorderStyle.None)
    {
      pformat.Borders.Bottom.BorderType = pformat.Borders.Top.BorderType = pformat.Borders.Left.BorderType = pformat.Borders.Right.BorderType = format.Borders.AllStyle;
      pformat.Borders.Left.Space = format.Borders.LeftSpace;
      pformat.Borders.Right.Space = format.Borders.RightSpace;
      pformat.Borders.Bottom.Space = format.Borders.BottomSpace;
      pformat.Borders.Top.Space = format.Borders.TopSpace;
      if (format.Borders.AllColor != Color.Empty)
        pformat.Borders.Bottom.Color = pformat.Borders.Top.Color = pformat.Borders.Left.Color = pformat.Borders.Right.Color = format.Borders.AllColor;
      if ((double) format.Borders.AllWidth != -1.0)
        pformat.Borders.Bottom.LineWidth = pformat.Borders.Top.LineWidth = pformat.Borders.Left.LineWidth = pformat.Borders.Right.LineWidth = format.Borders.AllWidth;
    }
    if (format.Borders.BottomStyle != BorderStyle.None)
    {
      pformat.Borders.Bottom.BorderType = format.Borders.BottomStyle;
      pformat.Borders.Bottom.LineWidth = format.Borders.BottomWidth;
      pformat.Borders.Bottom.Color = format.Borders.BottomColor;
      pformat.Borders.Bottom.Space = format.Borders.BottomSpace;
    }
    if (format.Borders.TopStyle != BorderStyle.None)
    {
      pformat.Borders.Top.BorderType = format.Borders.TopStyle;
      pformat.Borders.Top.LineWidth = format.Borders.TopWidth;
      pformat.Borders.Top.Color = format.Borders.TopColor;
      pformat.Borders.Top.Space = format.Borders.TopSpace;
    }
    if (format.Borders.LeftStyle != BorderStyle.None)
    {
      pformat.Borders.Left.BorderType = format.Borders.LeftStyle;
      pformat.Borders.Left.LineWidth = format.Borders.LeftWidth;
      pformat.Borders.Left.Color = format.Borders.LeftColor;
      pformat.Borders.Left.Space = format.Borders.LeftSpace;
    }
    if (format.Borders.RightStyle != BorderStyle.None)
    {
      pformat.Borders.Right.BorderType = format.Borders.RightStyle;
      pformat.Borders.Right.LineWidth = format.Borders.RightWidth;
      pformat.Borders.Right.Color = format.Borders.RightColor;
      pformat.Borders.Right.Space = format.Borders.RightSpace;
    }
    if ((double) format.Borders.TopWidth > 0.0)
      pformat.Borders.Top.LineWidth = format.Borders.TopWidth;
    if ((double) format.Borders.RightWidth > 0.0)
      pformat.Borders.Right.LineWidth = format.Borders.RightWidth;
    if ((double) format.Borders.LeftWidth > 0.0)
      pformat.Borders.Left.LineWidth = format.Borders.LeftWidth;
    if ((double) format.Borders.BottomWidth <= 0.0)
      return;
    pformat.Borders.Bottom.LineWidth = format.Borders.BottomWidth;
  }

  private void ApplyDivParagraphFormat(XmlNode node)
  {
    if (this.currDivFormat == null)
      return;
    WParagraphFormat paragraphFormat = this.m_currParagraph.ParagraphFormat;
    if (this.currDivFormat.HasValue(7))
      paragraphFormat.BackColor = this.currDivFormat.BackColor;
    if (this.currDivFormat.HasValue(12))
      paragraphFormat.SetPropertyValue(2, (object) this.currDivFormat.LeftMargin);
    if (this.currDivFormat.HasValue(14))
      paragraphFormat.SetPropertyValue(3, (object) this.currDivFormat.RightMargin);
    if (this.currDivFormat.HasValue(10))
      paragraphFormat.HorizontalAlignment = this.currDivFormat.TextAlign;
    if (this.IsBottomMarginNeedToBePreserved(node, this.currDivFormat))
      paragraphFormat.SetPropertyValue(9, (object) this.currDivFormat.BottomMargin);
    if (this.IsTopMarginNeedToBePreserved(node, this.currDivFormat))
      paragraphFormat.SetPropertyValue(8, (object) this.currDivFormat.TopMargin);
    if (this.currDivFormat.HasValue(8))
    {
      paragraphFormat.LineSpacingRule = this.currDivFormat.LineSpacingRule;
      paragraphFormat.SetPropertyValue(52, (object) this.currDivFormat.LineHeight);
    }
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    foreach (HTMLConverterImpl.TextFormat style in this.m_styleStack)
    {
      if (style != null)
      {
        if (style.Borders.AllStyle != BorderStyle.None)
        {
          flag1 = true;
          paragraphFormat.Borders.Bottom.BorderType = paragraphFormat.Borders.Top.BorderType = paragraphFormat.Borders.Left.BorderType = paragraphFormat.Borders.Right.BorderType = style.Borders.AllStyle;
          if (style.Borders.AllColor != Color.Empty)
            paragraphFormat.Borders.Bottom.Color = paragraphFormat.Borders.Top.Color = paragraphFormat.Borders.Left.Color = paragraphFormat.Borders.Right.Color = style.Borders.AllColor;
          if ((double) style.Borders.AllWidth != -1.0)
            paragraphFormat.Borders.Bottom.LineWidth = paragraphFormat.Borders.Top.LineWidth = paragraphFormat.Borders.Left.LineWidth = paragraphFormat.Borders.Right.LineWidth = style.Borders.AllWidth;
        }
        if (style.Borders.BottomStyle != BorderStyle.None && !flag2)
        {
          flag2 = true;
          paragraphFormat.Borders.Bottom.BorderType = style.Borders.BottomStyle;
          paragraphFormat.Borders.Bottom.LineWidth = style.Borders.BottomWidth;
          paragraphFormat.Borders.Bottom.Color = style.Borders.BottomColor;
        }
        if (style.Borders.TopStyle != BorderStyle.None && !flag3)
        {
          flag3 = true;
          paragraphFormat.Borders.Top.BorderType = style.Borders.TopStyle;
          paragraphFormat.Borders.Top.LineWidth = style.Borders.TopWidth;
          paragraphFormat.Borders.Top.Color = style.Borders.TopColor;
        }
        if (style.Borders.LeftStyle != BorderStyle.None && !flag4)
        {
          flag4 = true;
          paragraphFormat.Borders.Left.BorderType = style.Borders.LeftStyle;
          paragraphFormat.Borders.Left.LineWidth = style.Borders.LeftWidth;
          paragraphFormat.Borders.Left.Color = style.Borders.LeftColor;
        }
        if (style.Borders.RightStyle != BorderStyle.None && !flag5)
        {
          flag5 = true;
          paragraphFormat.Borders.Right.BorderType = style.Borders.RightStyle;
          paragraphFormat.Borders.Right.LineWidth = style.Borders.RightWidth;
          paragraphFormat.Borders.Right.Color = style.Borders.RightColor;
        }
        if (flag1 || flag2 && flag3 && flag4 && flag5)
          break;
      }
    }
  }

  private void ApplyTextFormatting(WCharacterFormat charFormat)
  {
    if (!this.m_bIsInDiv || !this.m_currParagraph.IsInCell)
      this.ApplyDivCharacterFormat(charFormat);
    HTMLConverterImpl.TextFormat currentFormat = this.CurrentFormat;
    if (currentFormat.HasValue(2))
    {
      charFormat.Bold = currentFormat.Bold;
      charFormat.BoldBidi = currentFormat.Bold;
    }
    if (currentFormat.HasValue(29))
      charFormat.LocaleIdASCII = currentFormat.LocaleIdASCII;
    if (currentFormat.HasValue(4))
    {
      charFormat.Italic = currentFormat.Italic;
      charFormat.ItalicBidi = currentFormat.Italic;
    }
    if (currentFormat.HasValue(3) && currentFormat.Underline)
      charFormat.UnderlineStyle = UnderlineStyle.Single;
    if (currentFormat.HasValue(5))
      charFormat.Strikeout = currentFormat.Strike;
    if (currentFormat.HasValue(6) && currentFormat.FontColor != Color.Empty)
      charFormat.TextColor = currentFormat.FontColor;
    if (currentFormat.HasValue(1) && currentFormat.FontFamily.Length > 0)
    {
      char[] chArray = new char[2]{ '\'', '"' };
      charFormat.FontName = currentFormat.FontFamily.Trim(chArray);
      charFormat.FontNameAscii = charFormat.FontName;
      charFormat.FontNameBidi = charFormat.FontName;
      charFormat.FontNameFarEast = charFormat.FontName;
      charFormat.FontNameNonFarEast = charFormat.FontName;
    }
    if (currentFormat.HasValue(0))
      charFormat.SetPropertyValue(3, (object) currentFormat.FontSize);
    else if (this.CurrentPara.ParaStyle != null && this.CurrentPara.ParaStyle.CharacterFormat.HasValue(3))
      charFormat.SetPropertyValue(3, (object) this.CurrentPara.ParaStyle.CharacterFormat.FontSize);
    else if (!charFormat.HasValue(3) && this.m_bodyItems.Document.Styles.FindByName("Normal (Web)") is WParagraphStyle byName)
      charFormat.SetPropertyValue(3, (object) (float) ((double) byName.CharacterFormat.FontSize != 12.0 ? (double) byName.CharacterFormat.FontSize : 12.0));
    if (currentFormat.HasValue(7) && currentFormat.BackColor != Color.Empty)
      charFormat.TextBackgroundColor = currentFormat.BackColor;
    if (currentFormat.SubSuperScript != SubSuperScript.None)
      charFormat.SubSuperScript = currentFormat.SubSuperScript;
    if (currentFormat.HasValue(19))
      charFormat.SetPropertyValue(18, (object) currentFormat.CharacterSpacing);
    if (currentFormat.HasValue(20))
      charFormat.AllCaps = currentFormat.AllCaps;
    if (currentFormat.HasValue(24))
      charFormat.SmallCaps = currentFormat.SmallCaps;
    if (currentFormat.HasValue(23))
      charFormat.Hidden = currentFormat.Hidden;
    else if (this.CurrentPara.IsInCell)
    {
      WTableCell ownerEntity = this.CurrentPara.GetOwnerEntity() as WTableCell;
      if (ownerEntity.CellFormat.Hidden)
        charFormat.Hidden = true;
      else if (ownerEntity.OwnerRow.RowFormat.HasValue(121))
        charFormat.Hidden = ownerEntity.OwnerRow.RowFormat.Hidden;
      else if (ownerEntity.OwnerRow.OwnerTable.TableFormat.HasValue(121))
        charFormat.Hidden = ownerEntity.OwnerRow.OwnerTable.TableFormat.Hidden;
    }
    if (currentFormat.Borders.TopStyle == BorderStyle.None)
      return;
    charFormat.Border.BorderType = currentFormat.Borders.TopStyle;
    charFormat.Border.Color = currentFormat.Borders.TopColor;
    charFormat.Border.LineWidth = currentFormat.Borders.TopWidth;
    charFormat.Border.Space = currentFormat.Borders.TopSpace;
  }

  private void ApplyDivCharacterFormat(WCharacterFormat charFormat)
  {
    if (this.currDivFormat == null)
      return;
    if ((double) this.currDivFormat.FontSize > 0.0)
      charFormat.SetPropertyValue(3, (object) this.currDivFormat.FontSize);
    if (this.currDivFormat.FontFamily.Length > 0)
      charFormat.FontName = this.currDivFormat.FontFamily;
    if (this.currDivFormat.HasValue(6) && this.currDivFormat.FontColor != Color.Empty)
      charFormat.ForeColor = this.currDivFormat.FontColor;
    if (this.currDivFormat.HasValue(2))
    {
      charFormat.Bold = this.currDivFormat.Bold;
      charFormat.BoldBidi = this.currDivFormat.Bold;
    }
    if (this.currDivFormat.HasValue(3))
      charFormat.UnderlineStyle = this.currDivFormat.Underline ? UnderlineStyle.Single : UnderlineStyle.None;
    if (this.currDivFormat.HasValue(5))
      charFormat.Strikeout = this.currDivFormat.Strike;
    if (this.currDivFormat.HasValue(4))
    {
      charFormat.Italic = this.currDivFormat.Italic;
      charFormat.ItalicBidi = this.currDivFormat.Italic;
    }
    if (!this.currDivFormat.HasValue(16 /*0x10*/))
      return;
    charFormat.SubSuperScript = this.currDivFormat.SubSuperScript;
  }

  private HTMLConverterImpl.TextFormat EnsureStyle(XmlNode node)
  {
    bool style = this.ParseStyle(node);
    if (!style && !node.Name.Equals("pre", StringComparison.OrdinalIgnoreCase))
      return this.AddStyle();
    if (style || !node.Name.Equals("pre", StringComparison.OrdinalIgnoreCase))
      return this.CurrentFormat;
    HTMLConverterImpl.TextFormat textFormat = this.m_styleStack.Count > 0 ? this.m_styleStack.Peek().Clone() : new HTMLConverterImpl.TextFormat();
    if (textFormat.HasKey(0))
      textFormat.m_propertiesHash.Remove(0);
    if (textFormat.HasKey(1))
      textFormat.m_propertiesHash.Remove(1);
    this.m_styleStack.Push(textFormat);
    return textFormat;
  }

  internal string ExtractValue(string value)
  {
    float result = 0.0f;
    if (value.EndsWith("pt"))
      return value.Replace("pt", string.Empty);
    if (value.EndsWith("%"))
    {
      float.TryParse(value.Replace("%", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      float num = this.m_currTable == null ? (this.m_currParagraph == null ? 0.0f : this.m_currParagraph.Document.Sections[0].PageSetup.ClientWidth) : this.m_currTable.GetTableClientWidth(this.m_currTable.GetOwnerWidth());
      return (result / 100f * num).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
    float num1;
    if (value.EndsWith("em"))
    {
      float.TryParse(value.Replace("em", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = result * 12f;
    }
    else if (value.EndsWith("in"))
    {
      float.TryParse(value.Replace("in", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = PointsConverter.FromInch(result);
    }
    else if (value.EndsWith("cm"))
    {
      float.TryParse(value.Replace("cm", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = PointsConverter.FromCm(result);
    }
    else if (value.EndsWith("pc"))
    {
      float.TryParse(value.Replace("pc", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = result * 12f;
    }
    else if (value.EndsWith("mm"))
    {
      float.TryParse(value.Replace("mm", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = (float) UnitsConvertor.Instance.ConvertUnits((double) result, PrintUnits.Millimeter, PrintUnits.Point);
    }
    else
    {
      float.TryParse(value.Replace("px", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      num1 = result * 0.75f;
    }
    return num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  private bool ParseStyle(XmlNode node)
  {
    string attributeValue1 = this.GetAttributeValue(node, "style");
    string attributeValue2 = this.GetAttributeValue(node, "lang");
    bool flag = !string.IsNullOrEmpty(attributeValue2) && DocxParser.IsEnumDefined(ref attributeValue2);
    if (attributeValue1.Length == 0 && this.CSSStyle == null && !flag)
      return false;
    HTMLConverterImpl.TextFormat textFormat = this.AddStyle();
    HTMLConverterImpl.TableBorders borders = textFormat.Borders;
    textFormat.Borders = new HTMLConverterImpl.TableBorders();
    string[] strArray = attributeValue1.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      char[] chArray = new char[2]{ '\'', '"' };
      string paramName = strArray[index].ToLower().Trim();
      string paramValue = strArray[index + 1].Trim().Trim(chArray);
      this.GetFormat(textFormat, paramName, paramValue, node);
    }
    if (attributeValue1 == string.Empty && node.Name == "b" && this.IsDefaultBorderFormat(textFormat.Borders) && !this.IsDefaultBorderFormat(borders))
      textFormat.Borders = borders;
    if (flag)
      textFormat.LocaleIdASCII = (short) (LocaleIDs) System.Enum.Parse(typeof (LocaleIDs), attributeValue2);
    if (node.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase) && (double) textFormat.ListNumberWidth != 0.0 && (double) textFormat.LeftMargin != 0.0 && (double) textFormat.PaddingLeft != 0.0)
    {
      textFormat.TextIndent = (float) -((double) textFormat.PaddingLeft + (double) textFormat.ListNumberWidth);
      textFormat.LeftMargin = textFormat.LeftMargin - textFormat.TextIndent - textFormat.ListNumberWidth;
    }
    this.FindCSSstyleItem(node, textFormat);
    return true;
  }

  private bool IsDefaultBorderFormat(HTMLConverterImpl.TableBorders borders)
  {
    return borders.AllColor == Color.Empty && borders.AllStyle == BorderStyle.None && (double) borders.AllWidth == -1.0 && borders.BottomColor == Color.Empty && (double) borders.BottomSpace == 0.0 && borders.BottomStyle == BorderStyle.None && (double) borders.BottomWidth == -1.0 && borders.LeftColor == Color.Empty && (double) borders.LeftSpace == 0.0 && borders.LeftStyle == BorderStyle.None && (double) borders.LeftWidth == -1.0 && borders.RightColor == Color.Empty && (double) borders.RightSpace == 0.0 && borders.RightStyle == BorderStyle.None && (double) borders.RightWidth == -1.0 && borders.TopColor == Color.Empty && (double) borders.TopSpace == 0.0 && borders.TopStyle == BorderStyle.None && (double) borders.TopWidth == -1.0;
  }

  private void GetFormat(
    HTMLConverterImpl.TextFormat format,
    string paramName,
    string paramValue,
    XmlNode node)
  {
    if (!(paramName == "mso-field-code") && !(paramName == "font-family"))
      paramValue = paramValue.ToLower();
    char[] chArray = new char[1]{ ' ' };
    if (paramValue.ToLower().Contains("inherit") && !paramName.Equals("page-break-before", StringComparison.OrdinalIgnoreCase) && !paramName.Equals("page-break-after", StringComparison.OrdinalIgnoreCase) && paramName != "page-break-inside")
      return;
    if (node.LocalName.Equals("span", StringComparison.OrdinalIgnoreCase))
    {
      if (this.isPreserveBreakForInvalidStyles)
        this.isPreserveBreakForInvalidStyles = false;
    }
    try
    {
      switch (paramName)
      {
        case "display":
          if (!(node.LocalName != "label"))
            break;
          switch (paramValue)
          {
            case "none":
              format.Hidden = true;
              return;
            case "inline-block":
              format.IsInlineBlock = true;
              return;
            default:
              format.Hidden = false;
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "white-space":
          switch (paramValue)
          {
            case "pre":
            case "pre-wrap":
              format.IsPreserveWhiteSpace = true;
              return;
            case "normal":
            case "no-wrap":
            case "pre-line":
            case "initial":
            case "inherit":
              format.IsPreserveWhiteSpace = false;
              return;
            default:
              format.IsPreserveWhiteSpace = false;
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "text-transform":
          switch (paramValue)
          {
            case "uppercase":
              format.AllCaps = true;
              return;
            case "none":
              format.AllCaps = false;
              return;
            default:
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "letter-spacing":
          if (paramValue == "normal")
          {
            format.CharacterSpacing = 0.0f;
            break;
          }
          format.CharacterSpacing = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "font":
          if (string.IsNullOrEmpty(paramValue))
            break;
          this.GetFont(paramValue, format);
          break;
        case "font-family":
          format.FontFamily = this.GetFontName(paramValue);
          break;
        case "font-style":
          switch (paramValue)
          {
            case "italic":
            case "oblique":
              format.Italic = true;
              return;
            case "strike":
              format.Strike = true;
              return;
            case "normal":
              format.Italic = false;
              return;
            default:
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "font-weight":
          int result;
          bool flag = int.TryParse(paramValue, out result);
          if (paramValue == "normal" || paramValue == "lighter" || flag && (result < 550 || result > 1000))
          {
            format.Bold = false;
            break;
          }
          format.Bold = true;
          this.isPreserveBreakForInvalidStyles = true;
          break;
        case "font-size":
          if (paramValue == "smaller")
          {
            format.FontSize = 10f;
            break;
          }
          format.FontSize = (float) this.ConvertSize(paramValue, format.FontSize);
          break;
        case "line-height":
          this.ParseLineHeight(paramValue, format, ref this.isPreserveBreakForInvalidStyles);
          break;
        case "-sf-tabstop-align":
          switch (paramValue.ToLower())
          {
            case null:
              return;
            case "left":
              format.TabJustification = TabJustification.Left;
              return;
            case "centered":
              format.TabJustification = TabJustification.Centered;
              return;
            case "right":
              format.TabJustification = TabJustification.Right;
              return;
            case "decimal":
              format.TabJustification = TabJustification.Decimal;
              return;
            case "bar":
              format.TabJustification = TabJustification.Bar;
              return;
            case "list":
              format.TabJustification = TabJustification.List;
              return;
            default:
              return;
          }
        case "-sf-tabstop-leader":
          switch (paramValue.ToLower())
          {
            case null:
              return;
            case "noLeader":
              format.TabLeader = TabLeader.NoLeader;
              return;
            case "dotted":
              format.TabLeader = TabLeader.Dotted;
              return;
            case "hyphenated":
              format.TabLeader = TabLeader.Hyphenated;
              return;
            case "single":
              format.TabLeader = TabLeader.Single;
              return;
            case "heavy":
              format.TabLeader = TabLeader.Heavy;
              return;
            default:
              return;
          }
        case "-sf-tabstop-pos":
          if (!paramValue.EndsWith("pt") && !paramValue.EndsWith("px") && !paramValue.EndsWith("em") && !paramValue.EndsWith("cm") && !paramValue.EndsWith("pc"))
            break;
          format.TabPosition = float.Parse(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "width":
          if (!paramValue.EndsWith("pt") && !paramValue.EndsWith("px") && !paramValue.EndsWith("em") && !paramValue.EndsWith("cm") && !paramValue.EndsWith("pc"))
            break;
          format.TabWidth = float.Parse(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "mso-spacerun":
          if (!(paramValue == "yes"))
            break;
          format.IsNonBreakingSpace = true;
          break;
        case "-sf-listtab":
          if (!(paramValue == "yes"))
            break;
          format.IsListTab = true;
          break;
        case "-sf-number-width":
          if (!paramValue.EndsWith("pt") && !paramValue.EndsWith("px") && !paramValue.EndsWith("em") && !paramValue.EndsWith("cm") && !paramValue.EndsWith("pc"))
            break;
          format.ListNumberWidth = float.Parse(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "text-align":
          format.TextAlign = this.GetHorizontalAlignment(paramValue);
          break;
        case "text-decoration":
        case "text-decoration-line":
          switch (paramValue)
          {
            case "underline":
              format.Underline = true;
              return;
            case "line-through":
              format.Strike = true;
              return;
            case "none":
              format.Underline = false;
              format.Strike = false;
              return;
            default:
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "color":
          format.FontColor = this.GetColor(paramValue);
          break;
        case "background":
        case "background-color":
          string name = node.Name;
          if (!name.Equals("table", StringComparison.OrdinalIgnoreCase) && !name.Equals("th", StringComparison.OrdinalIgnoreCase) && !name.Equals("td", StringComparison.OrdinalIgnoreCase))
            format.BackColor = this.GetColor(paramValue);
          if (!(paramValue == "transparent"))
            break;
          format.BackColor = Color.Empty;
          break;
        case "margin-left":
          if (!paramValue.Equals("auto", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("initial", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
            format.LeftMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          if (!node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase))
            break;
          this.UpdateListLeftIndentStack(format.LeftMargin, true);
          break;
        case "text-indent":
          if (paramValue.Equals("auto", StringComparison.OrdinalIgnoreCase) || paramValue.Equals("initial", StringComparison.OrdinalIgnoreCase) || paramValue.Equals("inherit", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) || node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase))
            break;
          format.TextIndent = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "margin-right":
          if (paramValue.Equals("auto", StringComparison.OrdinalIgnoreCase) || paramValue.Equals("initial", StringComparison.OrdinalIgnoreCase) || paramValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
            break;
          format.RightMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "-sf-before-space-auto":
          if (!(paramValue == "yes"))
            break;
          format.BeforeSpaceAuto = true;
          break;
        case "-sf-after-space-auto":
          if (!(paramValue == "yes"))
            break;
          format.AfterSpaceAuto = true;
          break;
        case "margin-top":
          if (!paramValue.Equals("auto", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("initial", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
          {
            format.TopMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          }
          format.TopMargin = -1f;
          break;
        case "margin-bottom":
          if (!paramValue.Equals("auto", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("initial", StringComparison.OrdinalIgnoreCase) && !paramValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
          {
            format.BottomMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          }
          format.BottomMargin = -1f;
          break;
        case "margin":
          string[] strArray1 = paramValue.Split(chArray);
          switch (strArray1.Length)
          {
            case 1:
              if (strArray1[0] != "auto" && strArray1[0] != "initial" && strArray1[0] != "inherit")
              {
                float single = Convert.ToSingle(this.ExtractValue(strArray1[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                format.TopMargin = single;
                format.RightMargin = single;
                format.BottomMargin = single;
                format.LeftMargin = single;
              }
              if (!node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase))
                return;
              this.UpdateListLeftIndentStack(format.LeftMargin, true);
              return;
            case 2:
              if (strArray1[0] != "auto" && strArray1[0] != "initial" && strArray1[0] != "inherit")
                format.TopMargin = Convert.ToSingle(this.ExtractValue(strArray1[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[1] != "auto" && strArray1[1] != "initial" && strArray1[1] != "inherit")
                format.RightMargin = Convert.ToSingle(this.ExtractValue(strArray1[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (!node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase))
                return;
              this.UpdateListLeftIndentStack(format.LeftMargin, false);
              return;
            case 3:
              if (strArray1[0] != "auto" && strArray1[0] != "initial" && strArray1[0] != "inherit")
                format.TopMargin = Convert.ToSingle(this.ExtractValue(strArray1[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[1] != "auto" && strArray1[1] != "initial" && strArray1[1] != "inherit")
                format.RightMargin = Convert.ToSingle(this.ExtractValue(strArray1[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[2] != "auto" && strArray1[2] != "initial" && strArray1[2] != "inherit")
                format.BottomMargin = Convert.ToSingle(this.ExtractValue(strArray1[2]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (!node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase))
                return;
              this.UpdateListLeftIndentStack(format.LeftMargin, false);
              return;
            case 4:
              if (strArray1[0] != "auto" && strArray1[0] != "initial" && strArray1[0] != "inherit")
                format.TopMargin = Convert.ToSingle(this.ExtractValue(strArray1[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[1] != "auto" && strArray1[1] != "initial" && strArray1[1] != "inherit")
                format.RightMargin = Convert.ToSingle(this.ExtractValue(strArray1[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[2] != "auto" && strArray1[2] != "initial" && strArray1[2] != "inherit")
                format.BottomMargin = Convert.ToSingle(this.ExtractValue(strArray1[2]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray1[3] != "auto" && strArray1[3] != "initial" && strArray1[3] != "inherit")
                format.LeftMargin = Convert.ToSingle(this.ExtractValue(strArray1[3]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (!node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase) && !node.LocalName.Equals("ol", StringComparison.OrdinalIgnoreCase))
                return;
              this.UpdateListLeftIndentStack(format.LeftMargin, true);
              return;
            default:
              return;
          }
        case "border-bottom":
          this.ParseParagraphBorder(paramValue, ref format.Borders.BottomColor, ref format.Borders.BottomWidth, ref format.Borders.BottomStyle);
          break;
        case "border-top":
          this.ParseParagraphBorder(paramValue, ref format.Borders.TopColor, ref format.Borders.TopWidth, ref format.Borders.TopStyle);
          break;
        case "border-left":
          this.ParseParagraphBorder(paramValue, ref format.Borders.LeftColor, ref format.Borders.LeftWidth, ref format.Borders.LeftStyle);
          break;
        case "border-right":
          this.ParseParagraphBorder(paramValue, ref format.Borders.RightColor, ref format.Borders.RightWidth, ref format.Borders.RightStyle);
          break;
        case "outline-color":
        case "border-color":
          string[] strArray2 = paramValue.Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray2.Length == 1)
          {
            format.Borders.AllColor = this.GetColor(paramValue);
            break;
          }
          if (strArray2.Length == 2)
          {
            format.Borders.TopColor = format.Borders.BottomColor = this.GetColor(strArray2[0]);
            format.Borders.LeftColor = format.Borders.RightColor = this.GetColor(strArray2[1]);
            break;
          }
          if (strArray2.Length == 3)
          {
            format.Borders.TopColor = this.GetColor(strArray2[0]);
            format.Borders.LeftColor = format.Borders.RightColor = this.GetColor(strArray2[1]);
            format.Borders.BottomColor = this.GetColor(strArray2[2]);
            break;
          }
          if (strArray2.Length != 4)
            break;
          format.Borders.TopColor = this.GetColor(strArray2[0]);
          format.Borders.RightColor = this.GetColor(strArray2[1]);
          format.Borders.BottomColor = this.GetColor(strArray2[2]);
          format.Borders.LeftColor = this.GetColor(strArray2[3]);
          break;
        case "border-left-color":
          format.Borders.LeftColor = this.GetColor(paramValue);
          break;
        case "border-right-color":
          format.Borders.RightColor = this.GetColor(paramValue);
          break;
        case "border-top-color":
          format.Borders.TopColor = this.GetColor(paramValue);
          break;
        case "border-bottom-color":
          format.Borders.BottomColor = this.GetColor(paramValue);
          break;
        case "outline-width":
        case "border-width":
          string[] strArray3 = paramValue.Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray3.Length == 1)
          {
            format.Borders.AllWidth = this.CalculateBorderWidth(paramValue);
            break;
          }
          if (strArray3.Length == 2)
          {
            format.Borders.TopWidth = format.Borders.BottomWidth = this.CalculateBorderWidth(strArray3[0]);
            format.Borders.LeftWidth = format.Borders.RightWidth = this.CalculateBorderWidth(strArray3[1]);
            break;
          }
          if (strArray3.Length == 3)
          {
            format.Borders.TopWidth = this.CalculateBorderWidth(strArray3[0]);
            format.Borders.LeftWidth = format.Borders.RightWidth = this.CalculateBorderWidth(strArray3[1]);
            format.Borders.BottomWidth = this.CalculateBorderWidth(strArray3[2]);
            break;
          }
          if (strArray3.Length != 4)
            break;
          format.Borders.TopWidth = this.CalculateBorderWidth(strArray3[0]);
          format.Borders.RightWidth = this.CalculateBorderWidth(strArray3[1]);
          format.Borders.BottomWidth = this.CalculateBorderWidth(strArray3[2]);
          format.Borders.LeftWidth = this.CalculateBorderWidth(strArray3[3]);
          break;
        case "border-left-width":
          format.Borders.LeftWidth = this.CalculateBorderWidth(paramValue);
          break;
        case "border-right-width":
          format.Borders.RightWidth = this.CalculateBorderWidth(paramValue);
          break;
        case "border-top-width":
          format.Borders.TopWidth = this.CalculateBorderWidth(paramValue);
          break;
        case "border-bottom-width":
          format.Borders.BottomWidth = this.CalculateBorderWidth(paramValue);
          break;
        case "outline-style":
        case "border-style":
          string[] strArray4 = paramValue.Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray4.Length == 1)
          {
            format.Borders.AllStyle = this.ToBorderType(paramValue);
            break;
          }
          if (strArray4.Length == 2)
          {
            format.Borders.TopStyle = format.Borders.BottomStyle = this.ToBorderType(strArray4[0]);
            format.Borders.LeftStyle = format.Borders.RightStyle = this.ToBorderType(strArray4[1]);
            break;
          }
          if (strArray4.Length == 3)
          {
            format.Borders.TopStyle = this.ToBorderType(strArray4[0]);
            format.Borders.LeftStyle = format.Borders.RightStyle = this.ToBorderType(strArray4[1]);
            format.Borders.BottomStyle = this.ToBorderType(strArray4[2]);
            break;
          }
          if (strArray4.Length != 4)
            break;
          format.Borders.TopStyle = this.ToBorderType(strArray4[0]);
          format.Borders.RightStyle = this.ToBorderType(strArray4[1]);
          format.Borders.BottomStyle = this.ToBorderType(strArray4[2]);
          format.Borders.LeftStyle = this.ToBorderType(strArray4[3]);
          break;
        case "border-left-style":
          format.Borders.LeftStyle = this.ToBorderType(paramValue);
          break;
        case "border-right-style":
          format.Borders.RightStyle = this.ToBorderType(paramValue);
          break;
        case "border-top-style":
          format.Borders.TopStyle = this.ToBorderType(paramValue);
          break;
        case "border-bottom-style":
          format.Borders.BottomStyle = this.ToBorderType(paramValue);
          break;
        case "border":
          this.ParseParagraphBorder(paramValue, ref format.Borders.RightColor, ref format.Borders.RightWidth, ref format.Borders.RightStyle);
          this.ParseParagraphBorder(paramValue, ref format.Borders.LeftColor, ref format.Borders.LeftWidth, ref format.Borders.LeftStyle);
          this.ParseParagraphBorder(paramValue, ref format.Borders.BottomColor, ref format.Borders.BottomWidth, ref format.Borders.BottomStyle);
          this.ParseParagraphBorder(paramValue, ref format.Borders.TopColor, ref format.Borders.TopWidth, ref format.Borders.TopStyle);
          break;
        case "page-break-before":
          if (node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase))
          {
            Break break1 = (Break) null;
            if (paramValue == "always")
            {
              break1 = this.CurrentPara.AppendBreak(BreakType.PageBreak);
              break;
            }
            Break break2 = this.CurrentPara.AppendBreak(BreakType.LineBreak);
            if (!(paramValue == "avoid") && !(paramValue == "inherit"))
              break;
            string attributeValue = this.GetAttributeValue(node, "clear");
            if (!(attributeValue == "all") && !(attributeValue == "left") && !(attributeValue == "right"))
              break;
            break2.HtmlToDocLayoutInfo.RemoveLineBreak = false;
            break;
          }
          switch (paramValue)
          {
            case "always":
              format.PageBreakBefore = true;
              return;
            case "auto":
              format.PageBreakBefore = false;
              return;
            default:
              return;
          }
        case "page-break-after":
          if (node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase))
          {
            Break break3 = (Break) null;
            if (paramValue == "always")
            {
              break3 = this.CurrentPara.AppendBreak(BreakType.PageBreak);
              break;
            }
            Break break4 = this.CurrentPara.AppendBreak(BreakType.LineBreak);
            if (!(paramValue == "avoid") && !(paramValue == "inherit"))
              break;
            string attributeValue = this.GetAttributeValue(node, "clear");
            if (!(attributeValue == "all") && !(attributeValue == "left") && !(attributeValue == "right"))
              break;
            break4.HtmlToDocLayoutInfo.RemoveLineBreak = false;
            break;
          }
          switch (paramValue)
          {
            case "auto":
              format.PageBreakAfter = false;
              return;
            case "avoid":
              format.KeepWithNext = true;
              return;
            default:
              return;
          }
        case "page-break-inside":
          if (node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase))
          {
            Break @break = this.CurrentPara.AppendBreak(BreakType.LineBreak);
            string attributeValue = this.GetAttributeValue(node, "clear");
            if (!(attributeValue == "all") && !(attributeValue == "left") && !(attributeValue == "right"))
              break;
            @break.HtmlToDocLayoutInfo.RemoveLineBreak = false;
            break;
          }
          if (!(paramValue == "avoid"))
            break;
          format.KeepLinesTogether = true;
          break;
        case "padding":
          string[] strArray5 = paramValue.Split(chArray);
          switch (strArray5.Length)
          {
            case 1:
              if (strArray5[0] != "initial" && strArray5[0] != "inherit")
                format.Borders.LeftSpace = Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              format.Borders.TopSpace = format.Borders.BottomSpace = format.Borders.RightSpace = format.Borders.LeftSpace;
              return;
            case 2:
              if (strArray5[0] != "initial" && strArray5[0] != "inherit")
                format.Borders.TopSpace = Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              format.Borders.BottomSpace = format.Borders.TopSpace;
              if (strArray5[1] != "initial" && strArray5[1] != "inherit")
                format.Borders.LeftSpace = Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              format.Borders.RightSpace = format.Borders.LeftSpace;
              return;
            case 3:
              if (strArray5[0] != "initial" && strArray5[0] != "inherit")
                format.Borders.TopSpace = Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray5[1] != "initial" && strArray5[1] != "inherit")
                format.Borders.LeftSpace = Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              format.Borders.RightSpace = format.Borders.LeftSpace;
              if (!(strArray5[2] != "initial") || !(strArray5[2] != "inherit"))
                return;
              format.Borders.BottomSpace = Convert.ToSingle(this.ExtractValue(strArray5[2]), (IFormatProvider) CultureInfo.InvariantCulture);
              return;
            case 4:
              if (strArray5[0] != "initial" && strArray5[0] != "inherit")
                format.Borders.TopSpace = Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray5[1] != "initial" && strArray5[1] != "inherit")
                format.Borders.RightSpace = Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (strArray5[2] != "initial" && strArray5[2] != "inherit")
                format.Borders.BottomSpace = Convert.ToSingle(this.ExtractValue(strArray5[2]), (IFormatProvider) CultureInfo.InvariantCulture);
              if (!(strArray5[3] != "initial") || !(strArray5[3] != "inherit"))
                return;
              format.Borders.LeftSpace = Convert.ToSingle(this.ExtractValue(strArray5[3]), (IFormatProvider) CultureInfo.InvariantCulture);
              return;
            default:
              return;
          }
        case "padding-left":
          if (!(paramValue != "initial") || !(paramValue != "inherit"))
            break;
          float single1 = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          if (node.LocalName.Equals("li", StringComparison.OrdinalIgnoreCase))
          {
            format.PaddingLeft = single1;
            break;
          }
          format.LeftMargin = single1;
          break;
        case "padding-top":
          if (!(paramValue != "initial") || !(paramValue != "inherit"))
            break;
          format.TopMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "padding-right":
          if (!(paramValue != "initial") || !(paramValue != "inherit"))
            break;
          format.RightMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "padding-bottom":
          if (!(paramValue != "initial") || !(paramValue != "inherit"))
            break;
          format.BottomMargin = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case "word-break":
          if (!(paramValue == "break-all"))
            break;
          format.WordWrap = false;
          break;
        case "mso-element":
          switch (paramValue)
          {
            case null:
              return;
            case "field-begin":
              this.IsPreviousItemFieldStart = true;
              return;
            case "field-separator":
              this.ParseFieldSeparator();
              return;
            case "field-end":
              this.ParseFieldEnd();
              return;
            default:
              return;
          }
        case "mso-field-code":
          string fieldCode = paramValue.Trim().Replace("\"", string.Empty);
          this.ParseFieldCode(FieldTypeDefiner.GetFieldType(fieldCode), fieldCode);
          break;
        case "vertical-align":
          switch (paramValue)
          {
            case "super":
              format.SubSuperScript = SubSuperScript.SuperScript;
              return;
            case "sub":
              format.SubSuperScript = SubSuperScript.SubScript;
              return;
            default:
              format.SubSuperScript = SubSuperScript.None;
              return;
          }
      }
    }
    catch
    {
      this.isPreserveBreakForInvalidStyles = true;
    }
  }

  private void GetTextFormat(
    HTMLConverterImpl.TextFormat format,
    string paramName,
    string paramValue)
  {
    switch (paramName)
    {
      case "text-transform":
        switch (paramValue)
        {
          case null:
            return;
          case "uppercase":
            format.AllCaps = true;
            return;
          case "none":
            format.AllCaps = false;
            return;
          default:
            return;
        }
      case "letter-spacing":
        if (paramValue == "normal")
        {
          format.CharacterSpacing = 0.0f;
          break;
        }
        format.CharacterSpacing = Convert.ToSingle(this.ExtractValue(paramValue), (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case "font-family":
        format.FontFamily = this.GetFontName(paramValue);
        break;
      case "font-style":
        switch (paramValue)
        {
          case null:
            return;
          case "italic":
          case "oblique":
            format.Italic = true;
            return;
          case "strike":
            format.Strike = true;
            return;
          case "normal":
            format.Italic = false;
            return;
          default:
            return;
        }
      case "font-weight":
        int result;
        bool flag = int.TryParse(paramValue, out result);
        if (paramValue == "normal" || paramValue == "lighter" || flag && (result < 550 || result > 1000))
        {
          format.Bold = false;
          break;
        }
        format.Bold = true;
        break;
      case "font-size":
        if (paramValue == "smaller")
        {
          format.FontSize = 10f;
          break;
        }
        format.FontSize = (float) this.ConvertSize(paramValue, format.FontSize);
        break;
      case "text-align":
        format.TextAlign = this.GetHorizontalAlignment(paramValue);
        break;
      case "text-decoration":
      case "text-decoration-line":
        switch (paramValue)
        {
          case null:
            return;
          case "underline":
            format.Underline = true;
            return;
          case "line-through":
            format.Strike = true;
            return;
          case "none":
            format.Underline = false;
            format.Strike = false;
            return;
          default:
            return;
        }
      case "color":
        format.FontColor = this.GetColor(paramValue);
        break;
    }
  }

  private void ParseLineHeight(
    string paramValue,
    HTMLConverterImpl.TextFormat format,
    ref bool isPreserveBreakForInvalidStyles)
  {
    if (paramValue != "normal")
    {
      if (paramValue.EndsWith("pt") || paramValue.EndsWith("px") || paramValue.EndsWith("em") || paramValue.EndsWith("cm") || paramValue.EndsWith("pc"))
      {
        float result;
        if (!float.TryParse(this.ExtractValue(paramValue), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return;
        format.LineSpacingRule = LineSpacingRule.AtLeast;
        format.LineHeight = result;
      }
      else if (paramValue.EndsWith("%"))
      {
        paramValue = paramValue.Replace("%", string.Empty);
        float result;
        if (!float.TryParse(paramValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return;
        format.LineSpacingRule = LineSpacingRule.Multiple;
        format.LineHeight = result / 100f * 12f;
      }
      else
      {
        int result;
        if (!(paramValue != "initial") || !(paramValue != "inherit") || !int.TryParse(paramValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return;
        isPreserveBreakForInvalidStyles = true;
        format.LineSpacingRule = LineSpacingRule.Multiple;
        format.LineHeight = (float) (result * 12);
      }
    }
    else
    {
      format.LineSpacingRule = LineSpacingRule.Multiple;
      format.LineHeight = 12f;
    }
  }

  private Color GetRGBColor(string[] value, ref int j)
  {
    Color rgbColor = Color.Empty;
    string str = value[j];
    for (int index = j + 1; index < value.Length; ++index)
    {
      if (value[index].Contains(")"))
      {
        rgbColor = this.GetColor(str + value[index]);
        j += index - j;
        break;
      }
      str += value[index];
    }
    return rgbColor;
  }

  private Color GetColor(string attValue)
  {
    if (!this.StartsWithExt(attValue, "rgb"))
      return ColorTranslator.FromHtml(attValue);
    string[] strArray = attValue.Replace("rgb", string.Empty).Trim('(', ')', ' ').Split(new char[1]
    {
      ','
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length != 3)
      return Color.Empty;
    int result1;
    int.TryParse(strArray[0], out result1);
    int result2;
    int.TryParse(strArray[1], out result2);
    int result3;
    int.TryParse(strArray[2], out result3);
    return Color.FromArgb(result1, result2, result3);
  }

  private string GetFontName(string paramValue)
  {
    string fontName = paramValue;
    if (paramValue.Trim().Contains(","))
    {
      int length = paramValue.Trim().IndexOf(',');
      fontName = paramValue.Trim().Substring(0, length);
    }
    return fontName;
  }

  private void GetFont(string paramValue, HTMLConverterImpl.TextFormat format)
  {
    char[] chArray1 = new char[1]{ ' ' };
    string[] strArray1 = paramValue.Split(chArray1);
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    for (int index = 0; index < strArray1.Length; ++index)
    {
      switch (strArray1[index])
      {
        case "italic":
        case "oblique":
          if (dictionary.Count == 0)
          {
            dictionary.Add("font-style", strArray1[index]);
            break;
          }
          if (dictionary.Count == 1 && (dictionary.ContainsKey("font-weight") || dictionary.ContainsKey("font-variant")))
          {
            dictionary.Add("font-style", strArray1[index]);
            break;
          }
          if (dictionary.Count == 2 && dictionary.ContainsKey("font-weight") && dictionary.ContainsKey("font-variant"))
          {
            dictionary.Add("font-style", strArray1[index]);
            break;
          }
          break;
        case "small-caps":
          if (dictionary.Count == 0)
          {
            dictionary.Add("font-variant", strArray1[index]);
            break;
          }
          if (dictionary.Count == 1 && (dictionary.ContainsKey("font-weight") || dictionary.ContainsKey("font-style")))
          {
            dictionary.Add("font-variant", strArray1[index]);
            break;
          }
          if (dictionary.Count == 2 && dictionary.ContainsKey("font-weight") && dictionary.ContainsKey("font-style"))
          {
            dictionary.Add("font-variant", strArray1[index]);
            break;
          }
          break;
        case "bold":
          if (dictionary.Count == 0)
          {
            dictionary.Add("font-weight", strArray1[index]);
            break;
          }
          if (dictionary.Count == 1 && (dictionary.ContainsKey("font-variant") || dictionary.ContainsKey("font-style")))
          {
            dictionary.Add("font-weight", strArray1[index]);
            break;
          }
          if (dictionary.Count == 2 && dictionary.ContainsKey("font-variant") && dictionary.ContainsKey("font-style"))
          {
            dictionary.Add("font-weight", strArray1[index]);
            break;
          }
          break;
        default:
          if (this.IsFontSize(strArray1[index]))
          {
            char[] chArray2 = new char[1]{ '/' };
            string[] strArray2 = strArray1[index].Split(chArray2);
            dictionary.Add("font-size", this.ConvertSize(strArray2[0], format.FontSize).ToString());
            if (strArray2.Length == 2)
              dictionary.Add("line-height", strArray2[1].ToString());
            ++index;
            string str = (string) null;
            for (; index < strArray1.Length; ++index)
              str = $"{str}{strArray1[index]} ";
            if (!string.IsNullOrEmpty(str))
            {
              dictionary.Add("font-name", str);
              break;
            }
            break;
          }
          if (index < strArray1.Length && !dictionary.ContainsKey("font-size"))
          {
            dictionary.Clear();
            break;
          }
          break;
      }
    }
    if (dictionary.Count <= 0)
      return;
    foreach (string key in dictionary.Keys)
    {
      switch (key)
      {
        case "font-style":
          format.Italic = true;
          continue;
        case "font-weight":
          format.Bold = true;
          continue;
        case "font-variant":
          format.SmallCaps = true;
          continue;
        case "font-size":
          float result;
          float.TryParse(dictionary[key], NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          format.FontSize = result;
          continue;
        case "line-height":
          this.ParseLineHeight(dictionary[key], format, ref this.isPreserveBreakForInvalidStyles);
          continue;
        case "font-name":
          format.FontFamily = dictionary[key];
          continue;
        default:
          continue;
      }
    }
  }

  private float CalculateFontSize(string value)
  {
    float fontSize = 12f;
    if (value.EndsWith("pt") || value.EndsWith("px") || value.EndsWith("em") || value.EndsWith("cm") || value.EndsWith("pc") || value.EndsWith("in"))
      fontSize = Convert.ToSingle(this.ExtractValue(value), (IFormatProvider) CultureInfo.InvariantCulture);
    return fontSize;
  }

  private bool IsFontSize(string value)
  {
    return value.EndsWith("pt") || value.EndsWith("px") || value.EndsWith("in") || value.EndsWith("em") || value.EndsWith("cm") || value.EndsWith("pc") || value.Contains("/");
  }

  private void ParseParagraphBorder(
    string paramValue,
    ref Color borderColor,
    ref float borderWidth,
    ref BorderStyle style)
  {
    string[] strArray1 = new string[9]
    {
      "dashed",
      "dotted",
      "double",
      "groove",
      "inset",
      "outset",
      "ridge",
      "solid",
      "hidden"
    };
    string[] strArray2 = paramValue.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    if (paramValue == "none" || paramValue == "medium none")
    {
      borderColor = Color.Empty;
      style = BorderStyle.Cleared;
      borderWidth = 0.0f;
    }
    else
    {
      for (int j = 0; j < strArray2.Length; ++j)
      {
        if (this.StartsWithExt(strArray2[j], "#"))
        {
          strArray2[j] = strArray2[j].Replace("#", string.Empty);
          strArray2[j] = this.GetValidRGBHexedecimal(strArray2[j]);
          int red = int.Parse(strArray2[j].Substring(0, 2), NumberStyles.AllowHexSpecifier);
          int green = int.Parse(strArray2[j].Substring(2, 2), NumberStyles.AllowHexSpecifier);
          int blue = int.Parse(strArray2[j].Substring(4, 2), NumberStyles.AllowHexSpecifier);
          borderColor = Color.FromArgb(red, green, blue);
        }
        else if (this.IsBorderWidth(strArray2[j]))
        {
          borderWidth = this.CalculateBorderWidth(strArray2[j]);
        }
        else
        {
          foreach (string str in strArray1)
          {
            if (strArray2[j] == str)
              this.m_bBorderStyle = true;
          }
          if (this.m_bBorderStyle)
          {
            style = this.ToBorderType(strArray2[j]);
            this.m_bBorderStyle = false;
          }
          else
          {
            borderColor = this.GetColor(strArray2[j]);
            if (borderColor.IsEmpty && this.StartsWithExt(strArray2[j], "rgb("))
              borderColor = this.GetRGBColor(strArray2, ref j);
          }
        }
      }
      if ((double) borderWidth >= 0.0)
        return;
      borderWidth = 3f;
    }
  }

  private void ParseBorder(string paramValue, Border border)
  {
    string[] strArray1 = new string[9]
    {
      "dashed",
      "dotted",
      "double",
      "groove",
      "inset",
      "outset",
      "ridge",
      "solid",
      "hidden"
    };
    string[] strArray2 = paramValue.Split(' ');
    if (paramValue == "none" || paramValue == "medium none")
    {
      border.Color = Color.Empty;
      border.BorderType = BorderStyle.None;
      border.LineWidth = 0.0f;
    }
    else
    {
      for (int j = 0; j < strArray2.Length; ++j)
      {
        if (this.StartsWithExt(strArray2[j], "#"))
        {
          strArray2[j] = strArray2[j].Replace("#", string.Empty);
          strArray2[j] = this.GetValidRGBHexedecimal(strArray2[j]);
          int red = int.Parse(strArray2[j].Substring(0, 2), NumberStyles.AllowHexSpecifier);
          int green = int.Parse(strArray2[j].Substring(2, 2), NumberStyles.AllowHexSpecifier);
          int blue = int.Parse(strArray2[j].Substring(4, 2), NumberStyles.AllowHexSpecifier);
          border.Color = Color.FromArgb(red, green, blue);
        }
        else if (this.IsBorderWidth(strArray2[j]))
        {
          border.LineWidth = this.CalculateBorderWidth(strArray2[j]);
        }
        else
        {
          foreach (string str in strArray1)
          {
            if (strArray2[j] == str)
              this.m_bBorderStyle = true;
          }
          if (this.m_bBorderStyle)
          {
            border.BorderType = this.ToBorderType(strArray2[j]);
            this.m_bBorderStyle = false;
          }
          else
          {
            border.Color = this.GetColor(strArray2[j]);
            if (border.Color.IsEmpty && this.StartsWithExt(strArray2[j], "rgb("))
              border.Color = this.GetRGBColor(strArray2, ref j);
          }
        }
      }
    }
  }

  private bool IsBorderWidth(string value)
  {
    return value.EndsWith("pt") || value.EndsWith("px") || value.EndsWith("in") || value.EndsWith("em") || value.EndsWith("cm") || value.EndsWith("pc") || value == "medium" || value == "thick" || value == "thin";
  }

  private float CalculateBorderWidth(string value)
  {
    float num = 0.0f;
    if (value.EndsWith("pt") || value.EndsWith("px") || value.EndsWith("em") || value.EndsWith("cm") || value.EndsWith("pc"))
      num = Convert.ToSingle(this.ExtractValue(value), (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.EndsWith("in"))
    {
      num = this.SeperateParamValue(value)[1] != null ? Convert.ToSingle(this.ExtractValue(value), (IFormatProvider) CultureInfo.InvariantCulture) : 0.75f;
    }
    else
    {
      switch (value)
      {
        case "medium":
          num = 3f;
          break;
        case "thick":
          num = 4.5f;
          break;
      }
    }
    return (float) (byte) Math.Round((double) num * 8.0) / 8f;
  }

  private string GetValidRGBHexedecimal(string value)
  {
    if (value.Length == 3)
    {
      string empty = string.Empty;
      foreach (char c in value)
        empty += new string(c, 2);
      value = empty;
    }
    return value;
  }

  private string[] SeperateParamValue(string paramValue)
  {
    string[] strArray1 = new string[2];
    for (int index = 0; index < paramValue.Length; ++index)
    {
      char c = paramValue[index];
      if (char.IsDigit(c))
      {
        string[] strArray2;
        (strArray2 = strArray1)[1] = strArray2[1] + (object) c;
      }
      else
      {
        string[] strArray3;
        (strArray3 = strArray1)[0] = strArray3[0] + (object) c;
      }
    }
    return strArray1;
  }

  private BorderStyle ToBorderType(string type)
  {
    switch (type.ToLower())
    {
      case "dashed":
        return BorderStyle.DashLargeGap;
      case "dotted":
        return BorderStyle.Dot;
      case "double":
        return BorderStyle.Double;
      case "groove":
        return BorderStyle.Engrave3D;
      case "inset":
        return BorderStyle.Inset;
      case "outset":
        return BorderStyle.Outset;
      case "ridge":
        return BorderStyle.Emboss3D;
      case "solid":
        return BorderStyle.Single;
      case "none":
      case "hidden":
        return BorderStyle.None;
      default:
        return BorderStyle.None;
    }
  }

  private void LeaveStyle(bool stylePresent)
  {
    if (!stylePresent)
      return;
    this.m_styleStack.Pop();
  }

  private void UpdateParaFormat(XmlNode node, WParagraphFormat pformat)
  {
    if (this.m_currParagraph == null)
      return;
    string val = this.GetAttributeValue(node, "align");
    string attributeValue = this.GetAttributeValue(node, "style");
    if (attributeValue.Length != 0)
    {
      string[] strArray = attributeValue.Split(';', ':');
      int index = 0;
      for (int length = strArray.Length; index < length - 1; index += 2)
      {
        char[] chArray = new char[2]{ '\'', '"' };
        string str1 = strArray[index].ToLower().Trim();
        string str2 = strArray[index + 1].ToLower().Trim().Trim(chArray);
        if (str1 == "text-align")
          val = str2;
      }
    }
    if (val != string.Empty && !node.Name.Equals("table", StringComparison.OrdinalIgnoreCase))
    {
      pformat.HorizontalAlignment = this.GetHorizontalAlignment(val);
    }
    else
    {
      if (!this.m_currParagraph.IsInCell)
        return;
      if (this.m_bIsAlignAttrDefinedInRowNode)
        pformat.HorizontalAlignment = this.m_horizontalAlignmentDefinedInRowNode;
      if (!this.m_bIsAlignAttriDefinedInCellNode)
        return;
      pformat.HorizontalAlignment = this.m_horizontalAlignmentDefinedInCellNode;
    }
  }

  private HTMLConverterImpl.TextFormat AddStyle()
  {
    HTMLConverterImpl.TextFormat textFormat = this.m_styleStack.Count > 0 ? this.m_styleStack.Peek().Clone() : new HTMLConverterImpl.TextFormat();
    this.m_styleStack.Push(textFormat);
    return textFormat;
  }

  private void UpdateHeightAndWidth(WPicture pic, bool isHeightSpecified, bool isWidthSpecified)
  {
    if (isHeightSpecified && !isWidthSpecified)
    {
      float num = (float) ((double) pic.Height / (double) pic.Image.Height * 100.0);
      pic.Width = (float) pic.Image.Width / (100f / num);
    }
    else
    {
      if (isHeightSpecified || !isWidthSpecified)
        return;
      float num = (float) ((double) pic.Width / (double) pic.Image.Width * 100.0);
      pic.Height = (float) pic.Image.Height / (100f / num);
    }
  }

  private string GetAttributeValue(XmlNode node, string attrName)
  {
    attrName = attrName.ToLower();
    for (int i = 0; i < node.Attributes.Count; ++i)
    {
      XmlAttribute attribute = node.Attributes[i];
      if (attribute.LocalName.Equals(attrName, StringComparison.OrdinalIgnoreCase))
        return attribute.Value;
    }
    return string.Empty;
  }

  private string GetStyleAttributeValue(string styleAttr, string styleAttrName)
  {
    string[] strArray1 = styleAttr.Split(';');
    for (int index = 0; index < strArray1.Length; ++index)
    {
      switch (styleAttrName)
      {
        case "list-style-image":
          if (strArray1[index].Contains("list-style-image") && strArray1[index].Contains("url"))
            return strArray1[index].Substring(strArray1[index].IndexOf("url"));
          break;
        default:
          string[] strArray2 = strArray1[index].Split(':');
          char[] chArray = new char[2]{ '\'', '"' };
          string str = strArray2[0].ToLower().Trim();
          string styleAttributeValue = strArray2[1].ToLower().Trim().Trim(chArray);
          if (str == styleAttrName)
            return styleAttributeValue;
          break;
      }
    }
    return string.Empty;
  }

  private bool ConvertToBoolValue(string paramvalue)
  {
    switch (paramvalue)
    {
      case "always":
      case "auto":
      case "left":
      case "right":
      case "initial":
        return true;
      default:
        return false;
    }
  }

  private double ConvertSize(string paramValue, float baseSize)
  {
    float result = 0.0f;
    if ((double) baseSize < 0.0)
      baseSize = 3f;
    switch (paramValue)
    {
      case "xx-small":
        return 7.5;
      case "x-small":
        return 10.0;
      case "small":
        return 12.0;
      case "medium":
        return 13.5;
      case "large":
        return 18.0;
      case "x-large":
        return 24.0;
      case "xx-large":
        return 36.0;
      case "smaller":
        return 10.0;
      case "bigger":
        return 12.0;
      case "larger":
        return 13.5;
      default:
        if (paramValue.EndsWith("%"))
          return (double) baseSize * (double) this.GetNumberBefore(paramValue, "%") / 100.0;
        if (paramValue.EndsWith("em"))
          return (double) baseSize * (double) this.GetNumberBefore(paramValue, "em");
        if (paramValue.EndsWith("ex"))
          return (double) baseSize / 2.0 * (double) this.GetNumberBefore(paramValue, "ex");
        if (paramValue.EndsWith("pt"))
          return (double) this.GetNumberBefore(paramValue, "pt");
        if (paramValue.EndsWith("in"))
        {
          float.TryParse(paramValue.Replace("in", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (double) PointsConverter.FromInch(result);
        }
        if (paramValue.EndsWith("cm"))
        {
          float.TryParse(paramValue.Replace("cm", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (double) PointsConverter.FromCm(result);
        }
        if (paramValue.EndsWith("mm"))
        {
          float.TryParse(paramValue.Replace("mm", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return UnitsConvertor.Instance.ConvertUnits((double) result, PrintUnits.Millimeter, PrintUnits.Point);
        }
        if (paramValue.EndsWith("pc"))
        {
          float.TryParse(paramValue.Replace("pc", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (double) (result * 12f);
        }
        if (paramValue.EndsWith("px"))
        {
          float.TryParse(paramValue.Replace("px", string.Empty), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (double) (result / 1.33f);
        }
        float.TryParse(paramValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
        return (double) (result / 1.33f);
    }
  }

  private float GetNumberBefore(string val, string end)
  {
    val = val.Substring(0, val.IndexOf(end));
    return float.Parse(val, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private void OnValidation(object sender, ValidationEventArgs args)
  {
    throw new NotSupportedException(args.Message, (Exception) args.Exception);
  }

  private void BuildListStyle(ListPatternType type, XmlNode node)
  {
    ListStyle style;
    if (this.ListStack.Count > 0)
    {
      style = this.ListStack.Peek();
    }
    else
    {
      this.CreateListStyle(node);
      style = this.ListStack.Peek();
    }
    if (this.IsListNodeStart(node))
      this.CreateListLevel(style, type, node);
    this.m_currParagraph.ListFormat.ApplyStyle(style.Name);
    if (this.LfoStack.Count > 0)
      this.m_currParagraph.ListFormat.LFOStyleName = this.LfoStack.Peek();
    this.m_currParagraph.ListFormat.ListLevelNumber = this.m_curListLevel;
    this.lastUsedLevelNo = this.m_curListLevel;
    this.isLastLevelSkipped = false;
  }

  public void CreateListLevel(ListStyle style, ListPatternType type, XmlNode node)
  {
    WListLevel listLevel = style.Levels[this.m_curListLevel];
    if (this.m_listLevelNo.Contains(this.m_curListLevel) && listLevel.PatternType != type && !this.isLastLevelSkipped)
      listLevel = this.CreateListOverrideStyle(this.m_curListLevel, node);
    if (!this.m_listLevelNo.Contains(this.m_curListLevel))
      this.m_listLevelNo.Add(this.m_curListLevel);
    listLevel.PatternType = type;
    listLevel.NumberPosition = -18f;
    listLevel.FollowCharacter = FollowCharacterType.Tab;
    listLevel.CharacterFormat.RemoveFontNames();
    if (type == ListPatternType.Bullet)
    {
      this.UpdateBulletChar(this.m_curListLevel, node.ParentNode, listLevel);
    }
    else
    {
      listLevel.NumberPrefix = string.Empty;
      listLevel.NumberSuffix = ".";
    }
    string attributeValue1 = this.GetAttributeValue(node, "VALUE");
    string attributeValue2 = this.GetAttributeValue(node.ParentNode, "START");
    if (!string.IsNullOrEmpty(attributeValue1))
    {
      try
      {
        listLevel.StartAt = Convert.ToInt32(attributeValue1);
      }
      catch
      {
        listLevel.StartAt = this.PrepareListStart(attributeValue1, this.GetAttributeValue(node.ParentNode, "TYPE"));
      }
    }
    else
    {
      if (string.IsNullOrEmpty(attributeValue2))
        return;
      try
      {
        listLevel.StartAt = Convert.ToInt32(attributeValue2);
      }
      catch
      {
        listLevel.StartAt = this.PrepareListStart(attributeValue2, this.GetAttributeValue(node.ParentNode, "TYPE"));
      }
    }
  }

  private string GetListStyleType(XmlNode node)
  {
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
    {
      switch (attribute.LocalName.ToLower())
      {
        case "type":
          if (node.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase))
            return attribute.Value.ToLower();
          continue;
        case "style":
          string lower = attribute.Value.ToLower();
          if (lower.Contains("list-style-type"))
          {
            int startIndex1 = lower.LastIndexOf("list-style-type");
            int startIndex2 = lower.IndexOf(":", startIndex1) + 1;
            int num = lower.Length;
            if (lower.Contains(";"))
              num = lower.IndexOf(";", startIndex2);
            return lower.Substring(startIndex2, num - startIndex2).Trim();
          }
          if (!lower.Contains("list-style"))
            return (string) null;
          int startIndex3 = lower.LastIndexOf("list-style");
          int startIndex4 = lower.IndexOf(":", startIndex3) + 1;
          int num1 = lower.Length;
          if (lower.Contains(";"))
            num1 = lower.IndexOf(";", startIndex4);
          if (num1 < 0)
            num1 = lower.Length;
          string listStyleType = lower.Substring(startIndex4, num1 - startIndex4).Trim();
          if (!listStyleType.Contains(" "))
            return listStyleType;
          int length = listStyleType.IndexOf(" ");
          return listStyleType.Substring(0, length).Trim();
        default:
          continue;
      }
    }
    return (string) null;
  }

  private string GetListStyleTypeFromContainer(XmlNode node)
  {
    string typeFromContainer = (string) null;
    for (; node != null && !node.LocalName.Equals("html", StringComparison.OrdinalIgnoreCase); node = node.ParentNode)
    {
      switch (node.LocalName.ToLower())
      {
        case "body":
        case "table":
        case "tr":
        case "td":
        case "div":
          string listStyleType = this.GetListStyleType(node);
          if (!string.IsNullOrEmpty(listStyleType))
          {
            typeFromContainer = listStyleType;
            break;
          }
          break;
      }
    }
    return typeFromContainer;
  }

  private void UpdateBulletChar(int listLevelNo, XmlNode node, WListLevel listLevel)
  {
    string str = this.GetListStyleType(node);
    if (string.IsNullOrEmpty(str))
      str = this.GetListStyleTypeFromContainer(node);
    switch (str)
    {
      case "disc":
        listLevelNo = 0;
        break;
      case "circle":
        listLevelNo = 1;
        break;
      case "square":
        listLevelNo = 2;
        break;
    }
    switch (listLevelNo % 3)
    {
      case 0:
        listLevel.BulletCharacter = "\uF0B7";
        listLevel.CharacterFormat.FontName = "Symbol";
        break;
      case 1:
        listLevel.BulletCharacter = "o";
        listLevel.CharacterFormat.FontName = "Courier New";
        break;
      default:
        listLevel.BulletCharacter = "\uF0A7";
        listLevel.CharacterFormat.FontName = "Wingdings";
        break;
    }
    listLevel.CharacterFormat.SetPropertyValue(3, (object) 10f);
  }

  private void CreateListStyle(XmlNode node)
  {
    ListStyle listStyle = (ListStyle) null;
    if (this.m_userListStyle != null)
    {
      this.ListStack.Push(this.m_userListStyle);
    }
    else
    {
      string styleName = "ListStyle" + this.m_bodyItems.Document.ListStyles.Count.ToString();
      listStyle = !(node.Name == "ol") ? this.m_bodyItems.Document.AddListStyle(ListType.Bulleted, styleName) : this.m_bodyItems.Document.AddListStyle(ListType.Numbered, styleName);
      listStyle.IsHybrid = true;
    }
    this.ListStack.Push(listStyle);
  }

  private WListLevel CreateListOverrideStyle(int levelNumber, XmlNode node)
  {
    ListOverrideStyle listOverrideStyle = new ListOverrideStyle(this.m_bodyItems.Document);
    listOverrideStyle.Name = "LfoStyle_" + (object) Guid.NewGuid();
    this.m_bodyItems.Document.ListOverrides.Add(listOverrideStyle);
    OverrideLevelFormat lfoLevel = new OverrideLevelFormat(this.m_bodyItems.Document);
    listOverrideStyle.OverrideLevels.Add(levelNumber, lfoLevel);
    lfoLevel.OverrideFormatting = true;
    this.LfoStack.Push(listOverrideStyle.Name);
    return lfoLevel.OverrideListLevel;
  }

  private int PrepareListStart(string start, string type)
  {
    if (type == "i" || type == "I")
      return this.RomanToArabic(start);
    byte num = (byte) start.ToCharArray()[0];
    if (num >= (byte) 65 && num <= (byte) 90)
      return (int) num - 64 /*0x40*/;
    return num >= (byte) 97 && num <= (byte) 122 ? (int) num - 96 /*0x60*/ : 1;
  }

  private bool IsListNodeEnd(XmlNode node)
  {
    for (; node.NextSibling != null; node = node.NextSibling)
    {
      if (!(node.NextSibling.Name == "#whitespace"))
        return false;
    }
    return true;
  }

  private bool IsListNodeStart(XmlNode node)
  {
    for (; node.PreviousSibling != null; node = node.PreviousSibling)
    {
      if (!(node.PreviousSibling.Name == "#whitespace"))
        return false;
    }
    return true;
  }

  private bool IsInnerList(XmlNode node)
  {
    return node.ParentNode == null || !(node.ParentNode.Name.ToUpper() == "OL") && !(node.ParentNode.Name.ToUpper() == "UL") || node.ParentNode.ParentNode == null || !(node.ParentNode.ParentNode.Name.ToUpper() == "BODY") && !(node.ParentNode.ParentNode.Name.ToUpper() == "HTML");
  }

  private void ParseTable(XmlNode node)
  {
    this.m_bIsBorderCollapse = false;
    HTMLConverterImpl.TableBorders tableBorders = new HTMLConverterImpl.TableBorders();
    HTMLConverterImpl.SpanHelper spanHelper = new HTMLConverterImpl.SpanHelper();
    spanHelper.TableGridCollection = new Dictionary<int, List<KeyValuePair<int, float>>>();
    this.tableGrid.TableGridStack.Push(spanHelper.TableGridCollection);
    this.m_currTable = new WTable((IWordDocument) this.m_bodyItems.Document, false);
    if (this.m_bodyItems.Count > 0 && this.m_bodyItems.LastItem.EntityType == EntityType.Table)
      this.m_bodyItems.Add((IEntity) new WParagraph((IWordDocument) this.m_bodyItems.Document)
      {
        BreakCharacterFormat = {
          Hidden = true
        }
      });
    this.m_bodyItems.Add((IEntity) this.m_currTable);
    this.IsTableStyle = false;
    this.m_currTable.TableFormat.IsAutoResized = true;
    BodyItemCollection bodyItems = this.m_bodyItems;
    HTMLConverterImpl.TextFormat textFormat = this.currDivFormat;
    if (this.m_bIsInDiv && this.currDivFormat != null && (!this.m_currTable.IsInCell || this.NodeIsInDiv(node)))
    {
      this.ApplyDivTableFormat(node);
      if (this.m_styleStack.Count > 0)
      {
        textFormat = this.m_styleStack.Pop();
        this.m_styleStack.Push(new HTMLConverterImpl.TextFormat());
      }
    }
    this.ParseTableAttrs(node, spanHelper, tableBorders);
    this.ParseTableRows(node, spanHelper, tableBorders);
    this.LeaveStyle(this.IsTableStyle);
    if (this.m_bIsInDiv && textFormat != null && this.m_styleStack.Count > 0 && (!this.m_currTable.IsInCell || this.NodeIsInDiv(node)))
    {
      this.m_styleStack.Pop();
      this.m_styleStack.Push(textFormat);
    }
    this.m_bodyItems = bodyItems;
    spanHelper.UpdateTable(this.m_bodyItems.LastItem as WTable, this.tableGrid.TableGridStack, this.ClientWidth);
    this.childTableWidth = !(node.ParentNode.Name == "td") ? 0.0f : this.m_currTable.Width;
    if (this.m_currTableFooterRowIndex != -1)
    {
      this.m_currTable.Rows.Add(this.m_currTable.Rows[this.m_currTableFooterRowIndex]);
      this.m_currTableFooterRowIndex = -1;
    }
    if (this.m_bIsBorderCollapse)
      return;
    this.m_currTable.TableFormat.CellSpacing = this.cellSpacing;
    for (int index = 0; index < this.m_currTable.Rows.Count; ++index)
      this.m_currTable.Rows[index].RowFormat.CellSpacing = this.cellSpacing;
  }

  private void ApplyDivTableFormat(XmlNode node)
  {
    if (this.currDivFormat == null)
      return;
    RowFormat tableFormat = this.m_currTable.TableFormat;
    if (this.currDivFormat.HasValue(7))
      tableFormat.BackColor = this.currDivFormat.BackColor;
    if (this.currDivFormat.HasValue(12))
      tableFormat.LeftIndent = this.currDivFormat.LeftMargin;
    if (this.currDivFormat.HasValue(10))
    {
      switch (this.currDivFormat.TextAlign)
      {
        case HorizontalAlignment.Center:
          tableFormat.HorizontalAlignment = RowAlignment.Center;
          break;
        case HorizontalAlignment.Right:
          tableFormat.HorizontalAlignment = RowAlignment.Right;
          break;
        default:
          tableFormat.HorizontalAlignment = RowAlignment.Left;
          break;
      }
    }
    if (this.currDivFormat.Borders.AllStyle != BorderStyle.None)
    {
      tableFormat.Borders.Bottom.BorderType = tableFormat.Borders.Top.BorderType = tableFormat.Borders.Left.BorderType = tableFormat.Borders.Right.BorderType = this.currDivFormat.Borders.AllStyle;
      if (this.currDivFormat.Borders.AllColor != Color.Empty)
        tableFormat.Borders.Bottom.Color = tableFormat.Borders.Top.Color = tableFormat.Borders.Left.Color = tableFormat.Borders.Right.Color = this.currDivFormat.Borders.AllColor;
      if ((double) this.currDivFormat.Borders.AllWidth != -1.0)
        tableFormat.Borders.Bottom.LineWidth = tableFormat.Borders.Top.LineWidth = tableFormat.Borders.Left.LineWidth = tableFormat.Borders.Right.LineWidth = this.currDivFormat.Borders.AllWidth;
    }
    if (this.currDivFormat.Borders.BottomStyle != BorderStyle.None)
    {
      tableFormat.Borders.Bottom.BorderType = this.currDivFormat.Borders.BottomStyle;
      tableFormat.Borders.Bottom.LineWidth = this.currDivFormat.Borders.BottomWidth;
      tableFormat.Borders.Bottom.Color = this.currDivFormat.Borders.BottomColor;
    }
    if (this.currDivFormat.Borders.TopStyle == BorderStyle.None)
      return;
    tableFormat.Borders.Top.BorderType = this.currDivFormat.Borders.TopStyle;
    tableFormat.Borders.Top.LineWidth = this.currDivFormat.Borders.TopWidth;
    tableFormat.Borders.Top.Color = this.currDivFormat.Borders.TopColor;
  }

  private void ParseTableRows(
    XmlNode node,
    HTMLConverterImpl.SpanHelper spanHelper,
    HTMLConverterImpl.TableBorders tblBorders)
  {
    foreach (XmlNode childNode1 in node.ChildNodes)
    {
      spanHelper.m_tblGrid = new List<KeyValuePair<int, float>>();
      this.m_bIsAlignAttrDefinedInRowNode = false;
      this.m_bIsVAlignAttriDefinedInRowNode = false;
      if (childNode1.NodeType != XmlNodeType.Whitespace && childNode1.NodeType != XmlNodeType.Comment)
      {
        if (childNode1.Name == "tbody" || childNode1.Name == "thead" || childNode1.Name == "tfoot")
          this.ParseTableRows(childNode1, spanHelper, tblBorders);
        else if (childNode1.Name == "caption")
        {
          WTableRow wtableRow = this.m_currTable.AddRow(false, false);
          wtableRow.HeightType = TableRowHeightType.AtLeast;
          wtableRow.IsHeader = true;
          this.m_currParagraph = (WParagraph) wtableRow.AddCell(false).AddParagraph();
          this.m_currParagraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
          this.TraverseChildNodes(childNode1.ChildNodes);
        }
        else if (!childNode1.Name.Equals("col", StringComparison.OrdinalIgnoreCase) && !childNode1.Name.Equals("colgroup", StringComparison.OrdinalIgnoreCase))
        {
          if (!childNode1.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
            throw new NotSupportedException("Html contains not wellformatted table");
          WTableRow row = this.m_currTable.AddRow(false, false);
          this.IsRowStyle = false;
          row.RowFormat.ImportContainer((FormatBase) this.m_currTable.TableFormat);
          if (childNode1.ParentNode.Name == "thead")
            row.IsHeader = true;
          else if (childNode1.ParentNode.Name == "tfoot")
            this.m_currTableFooterRowIndex = this.m_currTable.Rows.Count - 1;
          this.ParseRowAttrs(childNode1, row);
          this.UpdateHiddenPropertyBasedOnParentNode(childNode1, row);
          spanHelper.ResetCurrColumn();
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            this.m_bIsAlignAttriDefinedInCellNode = false;
            if (childNode2.NodeType != XmlNodeType.Whitespace && childNode2.NodeType != XmlNodeType.Comment && !childNode2.Name.Equals("col", StringComparison.OrdinalIgnoreCase) && !childNode2.Name.Equals("colgroup", StringComparison.OrdinalIgnoreCase))
            {
              if (!childNode2.Name.Equals("td", StringComparison.OrdinalIgnoreCase) && !childNode2.Name.Equals("th", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Html contains not wellformatted table");
              WTableCell cell = row.AddCell(false);
              this.IsCellStyle = false;
              this.m_bodyItems = cell.Items;
              this.m_currParagraph = (WParagraph) cell.AddParagraph();
              if (this.m_userStyle != null)
                this.m_currParagraph.ApplyStyle(this.m_userStyle, false);
              bool stylePresent = childNode2.Name.Equals("th", StringComparison.OrdinalIgnoreCase);
              if (stylePresent)
              {
                this.m_bIsAlignAttriDefinedInCellNode = true;
                HTMLConverterImpl.TextFormat textFormat = this.AddStyle();
                textFormat.TextAlign = HorizontalAlignment.Center;
                this.m_horizontalAlignmentDefinedInCellNode = HorizontalAlignment.Center;
                textFormat.Bold = true;
              }
              this.ParseCellAttrs(childNode2, cell, spanHelper, tblBorders);
              this.TraverseChildNodes(childNode2.ChildNodes);
              this.LeaveStyle(this.IsCellStyle);
              if (this.m_currParagraph != null)
                this.ApplyTextFormatting(this.m_currParagraph.BreakCharacterFormat);
              if (cell.Items.Count > 1 && cell.Items[0] is WParagraph && (cell.Items[0] as WParagraph).Items.Count == 0)
                cell.Items.RemoveAt(0);
              this.ApplyParagraphFormat(childNode2);
              this.LeaveStyle(stylePresent);
              this.m_horizontalAlignmentDefinedInCellNode = HorizontalAlignment.Left;
              if (!cell.CellFormat.HasValue(4) && !cell.CellFormat.HasValue(5) && !cell.CellFormat.HasValue(7))
              {
                WTable ownerTable = cell.OwnerRow.OwnerTable;
                WTableRow ownerRow = cell.OwnerRow;
                if (ownerRow.RowFormat.HasValue(108))
                  cell.CellFormat.BackColor = ownerRow.RowFormat.BackColor;
                else if (ownerTable.TableFormat.HasValue(108))
                  cell.CellFormat.BackColor = ownerTable.TableFormat.BackColor;
              }
            }
          }
          this.LeaveStyle(this.IsRowStyle);
          spanHelper.TableGridCollection = this.tableGrid.TableGridStack.Pop();
          List<KeyValuePair<int, float>> keyValuePairList1 = new List<KeyValuePair<int, float>>();
          foreach (KeyValuePair<int, float> keyValuePair in spanHelper.m_tblGrid)
            keyValuePairList1.Add(keyValuePair);
          if (this.m_currTable.Rows.Count - 1 != spanHelper.TableGridCollection.Count)
          {
            int num1 = 0;
            float num2 = 0.0f;
            List<KeyValuePair<int, float>> keyValuePairList2 = new List<KeyValuePair<int, float>>();
            for (int index = 0; index < spanHelper.m_tblGrid.Count; ++index)
            {
              num1 += spanHelper.m_tblGrid[index].Key;
              num2 += spanHelper.m_tblGrid[index].Value;
            }
            keyValuePairList2.Add(new KeyValuePair<int, float>(num1, num2));
            this.m_currTable.FirstRow.Cells[0].Width = num2;
            spanHelper.AddCellToGrid(this.m_currTable.FirstRow.Cells[0], num1);
            spanHelper.TableGridCollection.Add(spanHelper.TableGridCollection.Count, keyValuePairList2);
          }
          spanHelper.m_tblGrid.Clear();
          spanHelper.TableGridCollection.Add(spanHelper.TableGridCollection.Count, keyValuePairList1);
          this.tableGrid.TableGridStack.Push(spanHelper.TableGridCollection);
          this.m_horizontalAlignmentDefinedInRowNode = HorizontalAlignment.Left;
          if (this.m_bIsVAlignAttriDefinedInRowNode)
          {
            foreach (WTableCell cell in (CollectionImpl) row.Cells)
            {
              if (!cell.CellFormat.HasValue(2))
                cell.CellFormat.VerticalAlignment = this.m_verticalAlignmentDefinedInRowNode;
            }
          }
        }
      }
    }
  }

  private XmlNode GetOwnerTable(XmlNode node)
  {
    while (node != null && (node.NodeType != XmlNodeType.Element || !node.LocalName.Equals("table", StringComparison.OrdinalIgnoreCase)))
      node = node.ParentNode;
    return node;
  }

  private void ParseCellAttrs(
    XmlNode node,
    WTableCell cell,
    HTMLConverterImpl.SpanHelper spanHelper,
    HTMLConverterImpl.TableBorders tblBrdrs)
  {
    CellFormat cellFormat = cell.CellFormat;
    cellFormat.VerticalAlignment = VerticalAlignment.Middle;
    List<XmlAttribute> xmlAttributeList = new List<XmlAttribute>();
    cellFormat.Borders.IsHTMLRead = true;
    cellFormat.Borders.IsRead = true;
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
    {
      switch (attribute.Name.ToLower())
      {
        case "width":
          if (attribute.Value.Equals("auto", StringComparison.OrdinalIgnoreCase))
          {
            cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Auto;
            continue;
          }
          if (attribute.Value.EndsWith("%"))
          {
            cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Percentage;
            cell.CellFormat.PreferredWidth.Width = Convert.ToSingle(attribute.Value.Replace("%", string.Empty), (IFormatProvider) CultureInfo.InvariantCulture);
            float num = (float) ((double) spanHelper.m_tableWidth * (double) cell.CellFormat.PreferredWidth.Width / 100.0);
            cell.Width = num;
            continue;
          }
          if (attribute.Value != "initial" && attribute.Value != "inherit")
          {
            cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Point;
            cell.Width = cell.CellFormat.PreferredWidth.Width = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
            continue;
          }
          continue;
        case "border":
          float num1 = 0.0f;
          if (attribute.Value != "initial" && attribute.Value != "inherit")
            num1 = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
          if ((double) num1 == 0.0)
          {
            cellFormat.Borders.LineWidth = num1;
            cellFormat.Borders.BorderType = BorderStyle.None;
            cellFormat.Borders.Color = Color.Empty;
            continue;
          }
          cellFormat.Borders.LineWidth = num1;
          cellFormat.Borders.BorderType = BorderStyle.Outset;
          cellFormat.Borders.Color = Color.Empty;
          continue;
        case "bordercolor":
          cell.CellFormat.Borders.Color = this.GetColor(attribute.Value);
          continue;
        case "style":
          this.IsCellStyle = true;
          this.ParseCellStyle(attribute, cell, node, spanHelper);
          continue;
        case "colspan":
          xmlAttributeList.Add(attribute);
          continue;
        case "rowspan":
          xmlAttributeList.Add(attribute);
          continue;
        case "align":
          this.m_bIsAlignAttriDefinedInCellNode = true;
          this.m_horizontalAlignmentDefinedInCellNode = this.GetHorizontalAlignment(attribute.Value);
          continue;
        case "bgcolor":
          cell.CellFormat.BackColor = this.GetColor(attribute.Value);
          continue;
        case "valign":
          cell.CellFormat.VerticalAlignment = this.GetVerticalAlignment(attribute.Value);
          continue;
        case "border-collapse":
          if (attribute.Value.Equals("collapse", StringComparison.OrdinalIgnoreCase))
          {
            this.m_bIsBorderCollapse = true;
            continue;
          }
          continue;
        case "class":
          this.FindClassSelector((HTMLConverterImpl.TextFormat) null, cell.CellFormat, node);
          continue;
        default:
          continue;
      }
    }
    this.ApplyCellBorder(cellFormat, tblBrdrs);
    cellFormat.Borders.IsHTMLRead = false;
    cellFormat.Borders.IsRead = false;
    int colSpan = 1;
    foreach (XmlAttribute xmlAttribute in xmlAttributeList)
    {
      switch (xmlAttribute.Name.ToLower())
      {
        case "colspan":
          short int16 = Convert.ToInt16(xmlAttribute.Value);
          if (int16 > (short) 1)
          {
            colSpan = (int) int16;
            continue;
          }
          continue;
        case "rowspan":
          int int32 = Convert.ToInt32(xmlAttribute.Value);
          if (int32 != 1)
          {
            cell.CellFormat.VerticalMerge = CellMerge.Start;
            spanHelper.m_rowspans.Add(cell, int32);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    spanHelper.AddCellToGrid(cell, colSpan);
    spanHelper.NextColumn();
  }

  private void ParseCellStyle(
    XmlAttribute attr,
    WTableCell cell,
    XmlNode node,
    HTMLConverterImpl.SpanHelper spanHelper)
  {
    if (!attr.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
      return;
    HTMLConverterImpl.TextFormat format = this.AddStyle();
    format.Borders = new HTMLConverterImpl.TableBorders();
    CellFormat cellFormat = cell.CellFormat;
    string[] strArray1 = attr.Value.Split(';', ':');
    char[] chArray = new char[1]{ ' ' };
    int index = 0;
    for (int length1 = strArray1.Length; index < length1 - 1; index += 2)
    {
      string paramName = strArray1[index].ToLower().Trim();
      string str = strArray1[index + 1].ToLower().Trim();
      if (!str.ToLower().Contains("inherit"))
      {
        try
        {
          switch (paramName)
          {
            case "background":
            case "background-color":
              cellFormat.BackColor = this.GetColor(str);
              if (str == "transparent")
              {
                cellFormat.BackColor = Color.Empty;
                continue;
              }
              continue;
            case "width":
              if (str.Equals("auto", StringComparison.OrdinalIgnoreCase))
              {
                cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Auto;
                continue;
              }
              if (str.EndsWith("%"))
              {
                cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Percentage;
                cell.CellFormat.PreferredWidth.Width = Convert.ToSingle(str.Replace("%", string.Empty));
                float num = (float) ((double) spanHelper.m_tableWidth * (double) cell.CellFormat.PreferredWidth.Width / 100.0);
                cell.Width = num;
                continue;
              }
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Point;
                  cell.Width = cell.CellFormat.PreferredWidth.Width = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            case "valign":
            case "vertical-align":
              cell.CellFormat.VerticalAlignment = this.GetVerticalAlignment(str);
              continue;
            case "display":
              cell.CellFormat.Hidden = str.Equals("none", StringComparison.OrdinalIgnoreCase);
              continue;
            case "text-align":
              this.m_bIsAlignAttriDefinedInCellNode = true;
              this.m_horizontalAlignmentDefinedInCellNode = this.GetHorizontalAlignment(str);
              continue;
            case "border-bottom":
              this.ParseBorder(str, cellFormat.Borders.Bottom);
              continue;
            case "border-top":
              this.ParseBorder(str, cellFormat.Borders.Top);
              continue;
            case "border-left":
              this.ParseBorder(str, cellFormat.Borders.Left);
              continue;
            case "border-right":
              this.ParseBorder(str, cellFormat.Borders.Right);
              continue;
            case "border-color":
              this.ParseBorderColor(cellFormat.Borders, str);
              continue;
            case "border-left-color":
              cellFormat.Borders.Left.Color = this.GetColor(str);
              continue;
            case "border-right-color":
              cellFormat.Borders.Right.Color = this.GetColor(str);
              continue;
            case "border-top-color":
              cellFormat.Borders.Top.Color = this.GetColor(str);
              continue;
            case "border-bottom-color":
              cellFormat.Borders.Bottom.Color = this.GetColor(str);
              continue;
            case "border-width":
              this.ParseBorderLineWidth(cellFormat.Borders, str);
              continue;
            case "border-left-width":
              cellFormat.Borders.Left.LineWidth = this.CalculateBorderWidth(str);
              continue;
            case "border-right-width":
              cellFormat.Borders.Right.LineWidth = this.CalculateBorderWidth(str);
              continue;
            case "border-top-width":
              cellFormat.Borders.Top.LineWidth = this.CalculateBorderWidth(str);
              continue;
            case "border-bottom-width":
              cellFormat.Borders.Bottom.LineWidth = this.CalculateBorderWidth(str);
              continue;
            case "border-style":
              this.ParseBorderStyle(cellFormat.Borders, str);
              continue;
            case "border-left-style":
              cellFormat.Borders.Left.BorderType = this.ToBorderType(str);
              continue;
            case "border-right-style":
              cellFormat.Borders.Right.BorderType = this.ToBorderType(str);
              continue;
            case "border-top-style":
              cellFormat.Borders.Top.BorderType = this.ToBorderType(str);
              continue;
            case "border-bottom-style":
              cellFormat.Borders.Bottom.BorderType = this.ToBorderType(str);
              continue;
            case "border":
              this.ParseBorder(str, cellFormat.Borders.Bottom);
              this.ParseBorder(str, cellFormat.Borders.Top);
              this.ParseBorder(str, cellFormat.Borders.Left);
              this.ParseBorder(str, cellFormat.Borders.Right);
              continue;
            case "border-collapse":
              if (str.Equals("collapse", StringComparison.OrdinalIgnoreCase))
              {
                this.m_bIsBorderCollapse = true;
                continue;
              }
              continue;
            case "padding":
              string[] strArray2 = str.Split(chArray);
              int length2 = strArray2.Length;
              cellFormat.SamePaddingsAsTable = false;
              switch (length2)
              {
                case 1:
                  if (strArray2[0] != "initial")
                  {
                    if (strArray2[0] != "inherit")
                    {
                      cellFormat.Paddings.All = Convert.ToSingle(this.ExtractValue(strArray2[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                      continue;
                    }
                    continue;
                  }
                  continue;
                case 2:
                  if (strArray2[0] != "initial" && strArray2[0] != "inherit")
                    cellFormat.Paddings.Top = Convert.ToSingle(this.ExtractValue(strArray2[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  cellFormat.Paddings.Bottom = cellFormat.Paddings.Top;
                  if (strArray2[1] != "initial" && strArray2[1] != "inherit")
                    cellFormat.Paddings.Right = Convert.ToSingle(this.ExtractValue(strArray2[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  cellFormat.Paddings.Left = cellFormat.Paddings.Right;
                  continue;
                case 3:
                  if (strArray2[0] != "initial" && strArray2[0] != "inherit")
                    cellFormat.Paddings.Top = Convert.ToSingle(this.ExtractValue(strArray2[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray2[1] != "initial" && strArray2[1] != "inherit")
                    cellFormat.Paddings.Right = Convert.ToSingle(this.ExtractValue(strArray2[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  cellFormat.Paddings.Left = cellFormat.Paddings.Right;
                  if (strArray2[2] != "initial")
                  {
                    if (strArray2[2] != "inherit")
                    {
                      cellFormat.Paddings.Bottom = Convert.ToSingle(this.ExtractValue(strArray2[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                      continue;
                    }
                    continue;
                  }
                  continue;
                case 4:
                  if (strArray2[0] != "initial" && strArray2[0] != "inherit")
                    cellFormat.Paddings.Top = Convert.ToSingle(this.ExtractValue(strArray2[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray2[1] != "initial" && strArray2[1] != "inherit")
                    cellFormat.Paddings.Right = Convert.ToSingle(this.ExtractValue(strArray2[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray2[2] != "initial" && strArray2[2] != "inherit")
                    cellFormat.Paddings.Bottom = Convert.ToSingle(this.ExtractValue(strArray2[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray2[3] != "initial")
                  {
                    if (strArray2[3] != "inherit")
                    {
                      cellFormat.Paddings.Left = Convert.ToSingle(this.ExtractValue(strArray2[3]), (IFormatProvider) CultureInfo.InvariantCulture);
                      continue;
                    }
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            case "padding-left":
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  if (cellFormat.SamePaddingsAsTable)
                    cellFormat.SamePaddingsAsTable = false;
                  cellFormat.Paddings.Left = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            case "padding-right":
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  if (cellFormat.SamePaddingsAsTable)
                    cellFormat.SamePaddingsAsTable = false;
                  cellFormat.Paddings.Right = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            case "padding-top":
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  if (cellFormat.SamePaddingsAsTable)
                    cellFormat.SamePaddingsAsTable = false;
                  cellFormat.Paddings.Top = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            case "padding-bottom":
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  if (cellFormat.SamePaddingsAsTable)
                    cellFormat.SamePaddingsAsTable = false;
                  cellFormat.Paddings.Bottom = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            case "height":
              if (str != "auto")
              {
                if (str != "initial")
                {
                  if (str != "inherit")
                  {
                    cell.OwnerRow.Height = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            default:
              this.GetFormat(format, paramName, str, node);
              continue;
          }
        }
        catch
        {
        }
      }
    }
  }

  private void ApplyBorders(Borders borders, bool isTable)
  {
    this.ApplyBorder(borders.Top, isTable);
    this.ApplyBorder(borders.Bottom, isTable);
    this.ApplyBorder(borders.Left, isTable);
    this.ApplyBorder(borders.Right, isTable);
  }

  private void ApplyBorder(Border border, bool isTable)
  {
    if (border.BorderType != BorderStyle.None && !border.HasNoneStyle && border.BorderType != BorderStyle.Cleared && (double) border.LineWidth == 0.0)
      border.LineWidth = 1f;
    if ((double) border.LineWidth != 0.0 && border.BorderType == BorderStyle.None && !border.HasNoneStyle)
    {
      border.BorderType = BorderStyle.Outset;
    }
    else
    {
      if (!isTable)
        return;
      border.HasNoneStyle = true;
    }
  }

  private void ApplyTableBorder(RowFormat format)
  {
    this.ApplyBorders(format.Borders, true);
    format.Borders.Horizontal.BorderType = BorderStyle.Cleared;
    format.Borders.Vertical.BorderType = BorderStyle.Cleared;
  }

  private void ApplyCellBorder(CellFormat format, HTMLConverterImpl.TableBorders ownerTableFormat)
  {
    this.ApplyBorders(format.Borders, false);
    if ((double) format.Borders.Left.LineWidth == 0.0 && format.Borders.Left.BorderType == BorderStyle.None && !format.Borders.Left.HasNoneStyle && (double) ownerTableFormat.AllWidth != -1.0)
    {
      format.Borders.Left.LineWidth = ownerTableFormat.AllWidth;
      format.Borders.Left.BorderType = ownerTableFormat.AllStyle;
    }
    if (format.Borders.Left.Color == Color.Empty && ownerTableFormat.AllColor != Color.Empty)
      format.Borders.Left.Color = ownerTableFormat.AllColor;
    if ((double) format.Borders.Right.LineWidth == 0.0 && format.Borders.Right.BorderType == BorderStyle.None && !format.Borders.Right.HasNoneStyle && (double) ownerTableFormat.AllWidth != -1.0)
    {
      format.Borders.Right.LineWidth = ownerTableFormat.AllWidth;
      format.Borders.Right.BorderType = ownerTableFormat.AllStyle;
    }
    if (format.Borders.Right.Color == Color.Empty && ownerTableFormat.AllColor != Color.Empty)
      format.Borders.Right.Color = ownerTableFormat.AllColor;
    if ((double) format.Borders.Top.LineWidth == 0.0 && format.Borders.Top.BorderType == BorderStyle.None && !format.Borders.Top.HasNoneStyle && (double) ownerTableFormat.AllWidth != -1.0)
    {
      format.Borders.Top.LineWidth = ownerTableFormat.AllWidth;
      format.Borders.Top.BorderType = ownerTableFormat.AllStyle;
    }
    if (format.Borders.Top.Color == Color.Empty && ownerTableFormat.AllColor != Color.Empty)
      format.Borders.Top.Color = ownerTableFormat.AllColor;
    if ((double) format.Borders.Bottom.LineWidth == 0.0 && format.Borders.Bottom.BorderType == BorderStyle.None && !format.Borders.Bottom.HasNoneStyle && (double) ownerTableFormat.AllWidth != -1.0)
    {
      format.Borders.Bottom.LineWidth = ownerTableFormat.AllWidth;
      format.Borders.Bottom.BorderType = ownerTableFormat.AllStyle;
    }
    if (!(format.Borders.Bottom.Color == Color.Empty) || !(ownerTableFormat.AllColor != Color.Empty))
      return;
    format.Borders.Bottom.Color = ownerTableFormat.AllColor;
  }

  private void ParseBorderLineWidth(Borders borders, string paramValue)
  {
    string[] strArray = paramValue.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 1)
      borders.LineWidth = this.CalculateBorderWidth(paramValue);
    else if (strArray.Length == 2)
    {
      borders.Top.LineWidth = borders.Bottom.LineWidth = this.CalculateBorderWidth(strArray[0]);
      borders.Left.LineWidth = borders.Right.LineWidth = this.CalculateBorderWidth(strArray[1]);
    }
    else if (strArray.Length == 3)
    {
      borders.Top.LineWidth = this.CalculateBorderWidth(strArray[0]);
      borders.Left.LineWidth = borders.Right.LineWidth = this.CalculateBorderWidth(strArray[1]);
      borders.Bottom.LineWidth = this.CalculateBorderWidth(strArray[2]);
    }
    else
    {
      if (strArray.Length != 4)
        return;
      borders.Top.LineWidth = this.CalculateBorderWidth(strArray[0]);
      borders.Right.LineWidth = this.CalculateBorderWidth(strArray[1]);
      borders.Bottom.LineWidth = this.CalculateBorderWidth(strArray[2]);
      borders.Left.LineWidth = this.CalculateBorderWidth(strArray[3]);
    }
  }

  private void ParseBorderStyle(Borders borders, string paramValue)
  {
    string[] strArray = paramValue.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 1)
      borders.BorderType = this.ToBorderType(paramValue);
    else if (strArray.Length == 2)
    {
      borders.Top.BorderType = borders.Bottom.BorderType = this.ToBorderType(strArray[0]);
      borders.Left.BorderType = borders.Right.BorderType = this.ToBorderType(strArray[1]);
    }
    else if (strArray.Length == 3)
    {
      borders.Top.BorderType = this.ToBorderType(strArray[0]);
      borders.Left.BorderType = borders.Right.BorderType = this.ToBorderType(strArray[1]);
      borders.Bottom.BorderType = this.ToBorderType(strArray[2]);
    }
    else
    {
      if (strArray.Length != 4)
        return;
      borders.Top.BorderType = this.ToBorderType(strArray[0]);
      borders.Right.BorderType = this.ToBorderType(strArray[1]);
      borders.Bottom.BorderType = this.ToBorderType(strArray[2]);
      borders.Left.BorderType = this.ToBorderType(strArray[3]);
    }
  }

  private void ParseBorderColor(Borders borders, string paramValue)
  {
    string[] strArray = paramValue.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 1)
      borders.Color = this.GetColor(paramValue);
    else if (strArray.Length == 2)
    {
      borders.Top.Color = borders.Bottom.Color = this.GetColor(strArray[0]);
      borders.Left.Color = borders.Right.Color = this.GetColor(strArray[1]);
    }
    else if (strArray.Length == 3)
    {
      borders.Top.Color = this.GetColor(strArray[0]);
      borders.Left.Color = borders.Right.Color = this.GetColor(strArray[1]);
      borders.Bottom.Color = this.GetColor(strArray[2]);
    }
    else
    {
      if (strArray.Length != 4)
        return;
      borders.Top.Color = this.GetColor(strArray[0]);
      borders.Right.Color = this.GetColor(strArray[1]);
      borders.Bottom.Color = this.GetColor(strArray[2]);
      borders.Left.Color = this.GetColor(strArray[3]);
    }
  }

  private void ParseRowAttrs(XmlNode rowNode, WTableRow row)
  {
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) rowNode.Attributes)
    {
      switch (attribute.Name.ToLower())
      {
        case "height":
          if (attribute.Value != "auto" && attribute.Value != "initial" && attribute.Value != "inherit")
          {
            row.Height = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
            continue;
          }
          continue;
        case "align":
          this.m_bIsAlignAttrDefinedInRowNode = true;
          this.m_horizontalAlignmentDefinedInRowNode = this.GetHorizontalAlignment(attribute.Value);
          continue;
        case "style":
          this.IsRowStyle = true;
          this.ParseRowStyle(attribute, row);
          continue;
        case "valign":
        case "vertical-align":
          this.m_bIsVAlignAttriDefinedInRowNode = true;
          this.m_verticalAlignmentDefinedInRowNode = this.GetVerticalAlignment(attribute.Value.ToLower());
          continue;
        case "bgcolor":
          row.RowFormat.BackColor = this.GetColor(attribute.Value);
          continue;
        case "border-collapse":
          if (attribute.Value.Equals("collapse", StringComparison.OrdinalIgnoreCase))
          {
            this.m_bIsBorderCollapse = true;
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    row.HeightType = TableRowHeightType.AtLeast;
  }

  private void UpdateHiddenPropertyBasedOnParentNode(XmlNode rowNode, WTableRow row)
  {
    if (row.RowFormat.HasKey(121))
      return;
    WTableRow row1 = new WTableRow((IWordDocument) this.m_currTable.Document);
    for (XmlNode parentNode = rowNode.ParentNode; parentNode.LocalName == "thead" || parentNode.LocalName == "tbody" || parentNode.LocalName == "tfoot"; parentNode = parentNode.ParentNode)
    {
      this.ParseRowAttrs(parentNode, row1);
      if (row1.RowFormat.HasKey(121))
      {
        row.RowFormat.Hidden = row1.RowFormat.Hidden;
        break;
      }
    }
  }

  private void ParseTableAttrs(
    XmlNode node,
    HTMLConverterImpl.SpanHelper spanHelper,
    HTMLConverterImpl.TableBorders brdrs)
  {
    if (this.m_currTable == null)
      return;
    RowFormat tableFormat = this.m_currTable.TableFormat;
    this.cellSpacing = PointsConverter.FromCm(0.05f) / 2f;
    tableFormat.Paddings.All = PointsConverter.FromCm(0.03f);
    tableFormat.Borders.IsHTMLRead = true;
    tableFormat.Borders.IsRead = true;
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
    {
      switch (attribute.Name.ToLower())
      {
        case "border":
          if (attribute.Value != "initial" && attribute.Value != "inherit")
            brdrs.AllWidth = tableFormat.Borders.LineWidth = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
          if ((double) brdrs.AllWidth != -1.0 && (double) brdrs.AllWidth != 0.0)
          {
            brdrs.AllStyle = BorderStyle.Outset;
            continue;
          }
          continue;
        case "bordercolor":
          brdrs.AllColor = tableFormat.Borders.Color = this.GetColor(attribute.Value);
          continue;
        case "border-collapse":
          if (attribute.Value.Equals("collapse", StringComparison.OrdinalIgnoreCase))
          {
            this.m_bIsBorderCollapse = true;
            continue;
          }
          continue;
        case "cellpadding":
          float single = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
          tableFormat.Paddings.All = PointsConverter.FromPixel(single);
          continue;
        case "cellspacing":
          this.cellSpacing = PointsConverter.FromPixel(Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture)) / 2f;
          continue;
        case "title":
          this.m_currTable.Title = attribute.Value;
          continue;
        case "style":
          this.IsTableStyle = true;
          this.ParseTableStyle(attribute, spanHelper);
          continue;
        case "background":
        case "background-color":
        case "bgcolor":
          tableFormat.BackColor = this.GetColor(attribute.Value);
          if (attribute.Value == "transparent")
          {
            tableFormat.BackColor = Color.Empty;
            continue;
          }
          continue;
        case "align":
          switch (attribute.Value)
          {
            case "center":
              this.m_currTable.TableFormat.HorizontalAlignment = RowAlignment.Center;
              continue;
            case "right":
              this.m_currTable.TableFormat.HorizontalAlignment = RowAlignment.Right;
              continue;
            default:
              this.m_currTable.TableFormat.HorizontalAlignment = RowAlignment.Left;
              continue;
          }
        case "width":
          if (attribute.Value.Equals("auto", StringComparison.OrdinalIgnoreCase))
          {
            this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Auto;
            continue;
          }
          if (attribute.Value.EndsWith("%"))
          {
            this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Percentage;
            this.m_currTable.PreferredTableWidth.Width = Convert.ToSingle(attribute.Value.Replace("%", string.Empty), (IFormatProvider) CultureInfo.InvariantCulture);
            float num = (float) ((double) this.m_currTable.GetOwnerWidth() * (double) this.m_currTable.PreferredTableWidth.Width / 100.0);
            spanHelper.m_tableWidth = num;
            continue;
          }
          if (!attribute.Value.Equals("initial", StringComparison.OrdinalIgnoreCase) && !attribute.Value.Equals("inherit", StringComparison.OrdinalIgnoreCase))
          {
            this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Point;
            spanHelper.m_tableWidth = Convert.ToSingle(this.ExtractValue(attribute.Value), (IFormatProvider) CultureInfo.InvariantCulture);
            this.m_currTable.PreferredTableWidth.Width = spanHelper.m_tableWidth;
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    this.ApplyTableBorder(tableFormat);
    tableFormat.Borders.IsHTMLRead = false;
    tableFormat.Borders.IsRead = false;
  }

  private void ParseTableStyle(XmlAttribute attr, HTMLConverterImpl.SpanHelper spanHelper)
  {
    if (!attr.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
      return;
    HTMLConverterImpl.TextFormat textFormat = this.AddStyle();
    textFormat.Borders = new HTMLConverterImpl.TableBorders();
    RowFormat tableFormat = this.m_currTable.TableFormat;
    string[] strArray = attr.Value.Split(';', ':');
    string widthValue = string.Empty;
    string maxWidthValue = string.Empty;
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      string paramName = strArray[index].ToLower().Trim();
      string str = strArray[index + 1].ToLower().Trim();
      try
      {
        switch (paramName)
        {
          case "display":
            tableFormat.Hidden = str.Equals("none", StringComparison.OrdinalIgnoreCase);
            continue;
          case "background":
          case "background-color":
          case "bgcolor":
            tableFormat.BackColor = this.GetColor(str);
            if (str == "transparent")
            {
              tableFormat.BackColor = Color.Empty;
              continue;
            }
            continue;
          case "border-collapse":
            if (str.Equals("collapse", StringComparison.OrdinalIgnoreCase))
            {
              this.m_bIsBorderCollapse = true;
              continue;
            }
            continue;
          case "width":
            widthValue = str;
            continue;
          case "max-width":
            maxWidthValue = str;
            continue;
          case "margin-left":
            if (!str.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
              if (!str.Equals("initial", StringComparison.OrdinalIgnoreCase))
              {
                if (!str.Equals("inherit", StringComparison.OrdinalIgnoreCase))
                {
                  this.m_currTable.TableFormat.LeftIndent = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            this.ParseTableProperties(paramName, str, textFormat);
            continue;
        }
      }
      catch
      {
      }
    }
    if (string.IsNullOrEmpty(widthValue) && string.IsNullOrEmpty(maxWidthValue))
      return;
    this.SetTableWidthFromTableStyle(widthValue, maxWidthValue, spanHelper);
  }

  private void SetTableWidthFromTableStyle(
    string widthValue,
    string maxWidthValue,
    HTMLConverterImpl.SpanHelper spanHelper)
  {
    widthValue = !widthValue.EndsWith("%") || string.IsNullOrEmpty(maxWidthValue) || !maxWidthValue.EndsWith("px") && !maxWidthValue.EndsWith("pt") ? widthValue : maxWidthValue;
    if (widthValue.Equals("auto", StringComparison.OrdinalIgnoreCase))
      this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Auto;
    else if (widthValue.EndsWith("%"))
    {
      this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Percentage;
      this.m_currTable.PreferredTableWidth.Width = Convert.ToSingle(widthValue.Replace("%", string.Empty), (IFormatProvider) CultureInfo.InvariantCulture);
      float num = (float) ((double) this.m_currTable.GetOwnerWidth() * (double) this.m_currTable.PreferredTableWidth.Width / 100.0);
      spanHelper.m_tableWidth = num;
    }
    else
    {
      if (widthValue.Equals("initial", StringComparison.OrdinalIgnoreCase) || widthValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
        return;
      this.m_currTable.PreferredTableWidth.WidthType = FtsWidth.Point;
      spanHelper.m_tableWidth = Convert.ToSingle(this.ExtractValue(widthValue), (IFormatProvider) CultureInfo.InvariantCulture);
      this.m_currTable.PreferredTableWidth.Width = spanHelper.m_tableWidth;
    }
  }

  private void ParseTableProperties(
    string paramName,
    string paramValue,
    HTMLConverterImpl.TextFormat textFormat)
  {
    RowFormat tableFormat = this.m_currTable.TableFormat;
    switch (paramName)
    {
      case "border-color":
        this.ParseBorderColor(tableFormat.Borders, paramValue);
        break;
      case "border-left-color":
        tableFormat.Borders.Left.Color = this.GetColor(paramValue);
        break;
      case "border-right-color":
        tableFormat.Borders.Right.Color = this.GetColor(paramValue);
        break;
      case "border-top-color":
        tableFormat.Borders.Right.Color = this.GetColor(paramValue);
        break;
      case "border-bottom-color":
        tableFormat.Borders.Bottom.Color = this.GetColor(paramValue);
        break;
      case "border-width":
        this.ParseBorderLineWidth(tableFormat.Borders, paramValue);
        break;
      case "border-left-width":
        tableFormat.Borders.Left.LineWidth = this.CalculateBorderWidth(paramValue);
        break;
      case "border-right-width":
        tableFormat.Borders.Right.LineWidth = this.CalculateBorderWidth(paramValue);
        break;
      case "border-top-width":
        tableFormat.Borders.Top.LineWidth = this.CalculateBorderWidth(paramValue);
        break;
      case "border-bottom-width":
        tableFormat.Borders.Bottom.LineWidth = this.CalculateBorderWidth(paramValue);
        break;
      case "border-style":
        this.ParseBorderStyle(tableFormat.Borders, paramValue);
        break;
      case "border-left-style":
        tableFormat.Borders.Left.BorderType = this.ToBorderType(paramValue);
        break;
      case "border-right-style":
        tableFormat.Borders.Right.BorderType = this.ToBorderType(paramValue);
        break;
      case "border-top-style":
        tableFormat.Borders.Top.BorderType = this.ToBorderType(paramValue);
        break;
      case "border-bottom-style":
        tableFormat.Borders.Bottom.BorderType = this.ToBorderType(paramValue);
        break;
      case "border-bottom":
        this.ParseTableBorder(paramValue, tableFormat.Borders.Bottom);
        break;
      case "border-top":
        this.ParseTableBorder(paramValue, tableFormat.Borders.Top);
        break;
      case "border-left":
        this.ParseTableBorder(paramValue, tableFormat.Borders.Left);
        break;
      case "border-right":
        this.ParseTableBorder(paramValue, tableFormat.Borders.Right);
        break;
      case "border":
        this.ParseTableBorder(paramValue, tableFormat.Borders.Bottom);
        this.ParseTableBorder(paramValue, tableFormat.Borders.Top);
        this.ParseTableBorder(paramValue, tableFormat.Borders.Left);
        this.ParseTableBorder(paramValue, tableFormat.Borders.Right);
        break;
      default:
        this.GetTextFormat(textFormat, paramName, paramValue);
        break;
    }
  }

  private void ParseTableBorder(string paramValue, Border border)
  {
    string[] strArray1 = new string[10]
    {
      "dashed",
      "dotted",
      "double",
      "groove",
      "inset",
      "outset",
      "ridge",
      "solid",
      "hidden",
      "none"
    };
    string[] strArray2 = paramValue.Split(' ');
    if (paramValue == "none" || paramValue == "medium none")
    {
      border.BorderType = BorderStyle.None;
      border.Color = Color.Empty;
      border.LineWidth = 0.0f;
    }
    else
    {
      for (int j = 0; j < strArray2.Length; ++j)
      {
        if (this.StartsWithExt(strArray2[j], "#"))
        {
          strArray2[j] = strArray2[j].Replace("#", string.Empty);
          strArray2[j] = this.GetValidRGBHexedecimal(strArray2[j]);
          int red = int.Parse(strArray2[j].Substring(0, 2), NumberStyles.AllowHexSpecifier);
          int green = int.Parse(strArray2[j].Substring(2, 2), NumberStyles.AllowHexSpecifier);
          int blue = int.Parse(strArray2[j].Substring(4, 2), NumberStyles.AllowHexSpecifier);
          border.Color = Color.FromArgb(red, green, blue);
        }
        else if (this.IsBorderWidth(strArray2[j]))
        {
          border.LineWidth = this.CalculateBorderWidth(strArray2[j]);
        }
        else
        {
          foreach (string str in strArray1)
          {
            if (strArray2[j] == str)
              this.m_bBorderStyle = true;
          }
          if (this.m_bBorderStyle)
          {
            border.BorderType = this.ToBorderType(strArray2[j]);
            this.m_bBorderStyle = false;
          }
          else
          {
            border.Color = this.GetColor(strArray2[j]);
            if (border.Color.IsEmpty && this.StartsWithExt(strArray2[j], "rgb("))
              border.Color = this.GetRGBColor(strArray2, ref j);
          }
        }
      }
    }
  }

  private void ParseRowStyle(XmlAttribute attr, WTableRow row)
  {
    if (!attr.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
      return;
    HTMLConverterImpl.TextFormat format = this.AddStyle();
    format.Borders = new HTMLConverterImpl.TableBorders();
    RowFormat rowFormat = row.RowFormat;
    string[] strArray = attr.Value.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      string paramName = strArray[index].ToLower().Trim();
      string str = strArray[index + 1].ToLower().Trim();
      try
      {
        switch (paramName)
        {
          case "display":
            rowFormat.Hidden = str.Equals("none", StringComparison.OrdinalIgnoreCase);
            continue;
          case "height":
            if (str != "auto")
            {
              if (str != "initial")
              {
                if (str != "inherit")
                {
                  row.Height = Convert.ToSingle(this.ExtractValue(str), (IFormatProvider) CultureInfo.InvariantCulture);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case "text-align":
            this.m_bIsAlignAttrDefinedInRowNode = true;
            this.m_horizontalAlignmentDefinedInRowNode = this.GetHorizontalAlignment(str);
            continue;
          case "background":
          case "background-color":
            row.RowFormat.BackColor = this.GetColor(str);
            continue;
          default:
            this.GetTextFormat(format, paramName, str);
            continue;
        }
      }
      catch
      {
      }
    }
  }

  private void ApplyTableBorder(string paramName, string paramValue, Border border)
  {
    string[] strArray1 = new string[10]
    {
      "dashed",
      "dotted",
      "double",
      "groove",
      "inset",
      "outset",
      "ridge",
      "solid",
      "hidden",
      "none"
    };
    char[] chArray = new char[1]{ ' ' };
    string[] strArray2 = paramValue.Split(chArray);
    for (int j = 0; j < strArray2.Length; ++j)
    {
      if (this.StartsWithExt(strArray2[j], "#"))
      {
        strArray2[j] = strArray2[j].Replace("#", string.Empty);
        strArray2[j] = this.GetValidRGBHexedecimal(strArray2[j]);
        int red = int.Parse(strArray2[j].Substring(0, 2), NumberStyles.AllowHexSpecifier);
        int green = int.Parse(strArray2[j].Substring(2, 2), NumberStyles.AllowHexSpecifier);
        int blue = int.Parse(strArray2[j].Substring(4, 2), NumberStyles.AllowHexSpecifier);
        border.Color = Color.FromArgb(red, green, blue);
      }
      else if (this.IsBorderWidth(strArray2[j]))
      {
        border.LineWidth = this.CalculateBorderWidth(strArray2[j]);
      }
      else
      {
        foreach (string str in strArray1)
        {
          if (strArray2[j] == str)
            this.m_bBorderStyle = true;
        }
        if (this.m_bBorderStyle)
        {
          border.BorderType = this.ToBorderType(strArray2[j]);
          this.m_bBorderStyle = false;
        }
        else
        {
          border.Color = this.GetColor(strArray2[j]);
          if (border.Color.IsEmpty && this.StartsWithExt(strArray2[j], "rgb("))
            border.Color = this.GetRGBColor(strArray2, ref j);
        }
      }
    }
  }

  private float ToPoints(string val)
  {
    if (val.EndsWith("px"))
      val = val.Substring(0, val.Length - 2);
    return PointsConverter.FromPixel(Convert.ToSingle(val));
  }

  private VerticalAlignment GetVerticalAlignment(string val)
  {
    switch (val.ToLower())
    {
      case "top":
        return VerticalAlignment.Top;
      case "middle":
        return VerticalAlignment.Middle;
      case "bottom":
        return VerticalAlignment.Bottom;
      default:
        return VerticalAlignment.Top;
    }
  }

  private HorizontalAlignment GetHorizontalAlignment(string val)
  {
    switch (val.ToLower())
    {
      case "center":
        return HorizontalAlignment.Center;
      case "right":
        return HorizontalAlignment.Right;
      case "justify":
        return HorizontalAlignment.Justify;
      default:
        if (!val.Equals("left", StringComparison.OrdinalIgnoreCase))
          this.isPreserveBreakForInvalidStyles = true;
        return HorizontalAlignment.Left;
    }
  }

  private int RomanToArabic(string numberStr)
  {
    numberStr = numberStr.ToUpper();
    char[] charArray = numberStr.ToCharArray();
    int num1 = 0;
    int arabic = 0;
    for (int index = charArray.Length - 1; index >= 0; --index)
    {
      int num2 = charArray[index] != 'M' ? (charArray[index] != 'D' ? (charArray[index] != 'C' ? (charArray[index] != 'L' ? (charArray[index] != 'X' ? (charArray[index] != 'V' ? (charArray[index] != 'I' ? 0 : 1) : 5) : 10) : 50) : 100) : 500) : 1000;
      if (num1 > num2)
        arabic = num1 - num2;
      else
        arabic += num2;
      num1 = num2;
    }
    return arabic;
  }

  private void Init()
  {
    this.m_curListLevel = -1;
    this.lastUsedLevelNo = -1;
    this.lastSkippedLevelNo = -1;
    this.listCount = 0;
    this.m_listStack = (Stack<ListStyle>) null;
    this.m_listLeftIndentStack.Clear();
  }

  private void ParseCssStyle(XmlNode node)
  {
    string innerText = node.InnerText;
    char[] separator = new char[1]{ '}' };
    if (!(innerText != string.Empty))
      return;
    foreach (string str in innerText.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries))
    {
      char[] chArray = new char[1]{ '{' };
      string[] strArray1 = str.Split(chArray);
      if (strArray1.Length < 2)
        break;
      strArray1[0] = strArray1[0].Trim().ToLower();
      strArray1[1] = strArray1[1].Trim().ToLower();
      if (!string.IsNullOrEmpty(strArray1[0]))
      {
        CSSStyleItem.CssStyleType csSselectorType = this.FindCSSselectorType(strArray1[0]);
        if (csSselectorType != CSSStyleItem.CssStyleType.None)
        {
          strArray1[0] = csSselectorType == CSSStyleItem.CssStyleType.ClassSelector || csSselectorType == CSSStyleItem.CssStyleType.IdSelector ? strArray1[0].Substring(1) : strArray1[0];
          if (csSselectorType == CSSStyleItem.CssStyleType.GroupingSelector)
          {
            string[] strArray2 = strArray1[0].Split(',');
            for (int index = 0; index < strArray2.Length; ++index)
            {
              CSSStyleItem CSSItem = this.CSSStyle.GetCSSStyleItem(strArray2[index], CSSStyleItem.CssStyleType.ElementSelector);
              if (CSSItem == null)
              {
                CSSItem = new CSSStyleItem();
                this.CSSStyle.StyleCollection.Add(CSSItem);
              }
              CSSItem.StyleName = strArray2[index];
              CSSItem.StyleType = CSSStyleItem.CssStyleType.ElementSelector;
              this.ParseCSSTextFormatValue(strArray1[1], CSSItem);
            }
          }
          else
          {
            CSSStyleItem CSSItem = this.CSSStyle.GetCSSStyleItem(strArray1[0], csSselectorType);
            if (CSSItem == null)
            {
              CSSItem = new CSSStyleItem();
              this.CSSStyle.StyleCollection.Add(CSSItem);
            }
            CSSItem.StyleName = strArray1[0];
            CSSItem.StyleType = csSselectorType;
            this.ParseCSSTextFormatValue(strArray1[1], CSSItem);
          }
        }
      }
    }
  }

  private CSSStyleItem.CssStyleType FindCSSselectorType(string selectorName)
  {
    CSSStyleItem.CssStyleType csSselectorType = CSSStyleItem.CssStyleType.None;
    string[] strArray = selectorName.Split(' ');
    if (this.StartsWithExt(selectorName, "#"))
      csSselectorType = CSSStyleItem.CssStyleType.IdSelector;
    else if (this.StartsWithExt(selectorName, "."))
      csSselectorType = CSSStyleItem.CssStyleType.ClassSelector;
    else if (selectorName.Contains(">"))
      csSselectorType = CSSStyleItem.CssStyleType.ChildSelector;
    else if (selectorName.Contains("~"))
      csSselectorType = CSSStyleItem.CssStyleType.GeneralSiblingSelector;
    else if (selectorName.Contains("+"))
      csSselectorType = CSSStyleItem.CssStyleType.AdjacentSiblingSelector;
    else if (selectorName.Contains(","))
      csSselectorType = CSSStyleItem.CssStyleType.GroupingSelector;
    else if (selectorName.Contains(" "))
      csSselectorType = CSSStyleItem.CssStyleType.DescendantSelector;
    else if (strArray.Length == 1)
      csSselectorType = CSSStyleItem.CssStyleType.ElementSelector;
    return csSselectorType;
  }

  private void FindCSSstyleItem(XmlNode node, HTMLConverterImpl.TextFormat textFormat)
  {
    if (this.CSSStyle == null)
      return;
    this.FindIDSelector(textFormat, node);
    this.FindClassSelector(textFormat, (CellFormat) null, node);
    this.FindDescendantSelector(textFormat, node);
    this.FindElementSelector(textFormat, node);
    this.FindChildSelector(textFormat, node);
  }

  private void FindIDSelector(HTMLConverterImpl.TextFormat textFormat, XmlNode node)
  {
    string attributeValue = this.GetAttributeValue(node, "id");
    if (string.IsNullOrEmpty(attributeValue))
      return;
    CSSStyleItem cssStyleItem = this.CSSStyle.GetCSSStyleItem(attributeValue.ToLower(), CSSStyleItem.CssStyleType.IdSelector);
    if (cssStyleItem == null)
      return;
    this.ApplyCSSStyle(textFormat, node, cssStyleItem);
    this.ApplyImportantCSSStyle(textFormat, node, cssStyleItem);
  }

  private void FindClassSelector(
    HTMLConverterImpl.TextFormat textFormat,
    CellFormat cellFormat,
    XmlNode node)
  {
    if (textFormat != null)
    {
      string attributeValue = this.GetAttributeValue(node, "class");
      if (string.IsNullOrEmpty(attributeValue))
        return;
      CSSStyleItem cssStyleItem = this.CSSStyle.GetCSSStyleItem(attributeValue.ToLower(), CSSStyleItem.CssStyleType.ClassSelector);
      if (cssStyleItem == null)
        return;
      this.ApplyCSSStyle(textFormat, node, cssStyleItem);
      this.ApplyImportantCSSStyle(textFormat, node, cssStyleItem);
    }
    else
    {
      if (cellFormat == null)
        return;
      string attributeValue = this.GetAttributeValue(node, "class");
      if (string.IsNullOrEmpty(attributeValue))
        return;
      CSSStyleItem cssStyleItem = this.CSSStyle.GetCSSStyleItem(attributeValue.ToLower(), CSSStyleItem.CssStyleType.ClassSelector);
      if (cssStyleItem == null)
        return;
      this.ApplyCSSStyleForCell(cellFormat, node, cssStyleItem);
    }
  }

  private void ApplyCSSStyleForCell(CellFormat cellFormat, XmlNode node, CSSStyleItem styleItem)
  {
    foreach (KeyValuePair<CSSStyleItem.TextFormatKey, object> keyValuePair in styleItem.PropertiesHash)
    {
      if (keyValuePair.Key == CSSStyleItem.TextFormatKey.BackColor && !cellFormat.HasValue(7))
        cellFormat.BackColor = (Color) keyValuePair.Value;
    }
  }

  private void FindDescendantSelector(HTMLConverterImpl.TextFormat textFormat, XmlNode node)
  {
    XmlNode xmlNode = node;
    for (int index1 = 0; index1 < this.CSSStyle.StyleCollection.Count; ++index1)
    {
      CSSStyleItem style = this.CSSStyle.StyleCollection[index1];
      if (style.StyleType == CSSStyleItem.CssStyleType.DescendantSelector)
      {
        string[] strArray = style.StyleName.Split(' ');
        for (int index2 = 0; index2 < strArray.Length && strArray[strArray.Length - index2 - 1].Equals(xmlNode.Name, StringComparison.OrdinalIgnoreCase); ++index2)
        {
          if (xmlNode.ParentNode != null)
            xmlNode = xmlNode.ParentNode;
          if (strArray.Length - 1 == index2)
          {
            this.ApplyCSSStyle(textFormat, node, style);
            this.ApplyImportantCSSStyle(textFormat, node, style);
          }
        }
      }
    }
  }

  private void FindElementSelector(HTMLConverterImpl.TextFormat textFormat, XmlNode node)
  {
    CSSStyleItem cssStyleItem = this.CSSStyle.GetCSSStyleItem(node.Name.ToLower(), CSSStyleItem.CssStyleType.ElementSelector);
    if (cssStyleItem == null)
      return;
    this.ApplyCSSStyle(textFormat, node, cssStyleItem);
    this.ApplyImportantCSSStyle(textFormat, node, cssStyleItem);
  }

  private void FindChildSelector(HTMLConverterImpl.TextFormat textFormat, XmlNode node)
  {
    if (node.ParentNode == null)
      return;
    CSSStyleItem cssStyleItem = this.CSSStyle.GetCSSStyleItem(node.ParentNode.Name.ToLower() + node.Name.ToLower(), CSSStyleItem.CssStyleType.ChildSelector);
    if (cssStyleItem == null)
      return;
    this.ApplyCSSStyle(textFormat, node, cssStyleItem);
    this.ApplyImportantCSSStyle(textFormat, node, cssStyleItem);
  }

  private void ApplyCSSStyle(
    HTMLConverterImpl.TextFormat textFormat,
    XmlNode node,
    CSSStyleItem styleItem)
  {
    foreach (KeyValuePair<CSSStyleItem.TextFormatKey, object> keyValuePair in styleItem.PropertiesHash)
    {
      switch (keyValuePair.Key)
      {
        case CSSStyleItem.TextFormatKey.FontSize:
          if (!textFormat.HasValue(0))
          {
            textFormat.FontSize = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.FontFamily:
          if (!textFormat.HasValue(1))
          {
            textFormat.FontFamily = Convert.ToString(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.Underline:
          if (!textFormat.HasValue(3))
          {
            textFormat.Underline = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.Italic:
          if (!textFormat.HasValue(4))
          {
            textFormat.Italic = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.Strike:
          if (!textFormat.HasValue(5))
          {
            textFormat.Strike = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.FontColor:
          if (!textFormat.HasValue(6))
          {
            textFormat.FontColor = (Color) keyValuePair.Value;
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.BackColor:
          if (!textFormat.HasValue(7))
          {
            textFormat.BackColor = (Color) keyValuePair.Value;
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.TextAlign:
          if (!textFormat.HasValue(10))
          {
            textFormat.TextAlign = (HorizontalAlignment) keyValuePair.Value;
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.TopMargin:
          if (!textFormat.HasValue(11))
          {
            textFormat.TopMargin = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.LeftMargin:
          if (!textFormat.HasValue(12))
          {
            textFormat.LeftMargin = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.BottomMargin:
          if (!textFormat.HasValue(13))
          {
            textFormat.BottomMargin = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.RightMargin:
          if (!textFormat.HasValue(14))
          {
            textFormat.RightMargin = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.TextIndent:
          if (!textFormat.HasValue(15))
          {
            textFormat.TextIndent = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.PageBreakBefore:
          if (!textFormat.HasValue(17))
          {
            textFormat.PageBreakBefore = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.PageBreakAfter:
          if (!textFormat.HasValue(18))
          {
            textFormat.PageBreakAfter = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.LetterSpacing:
          if (!textFormat.HasValue(19))
          {
            textFormat.CharacterSpacing = Convert.ToSingle(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.AllCaps:
          if (!textFormat.HasValue(20))
          {
            textFormat.AllCaps = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.WhiteSpace:
          if (!textFormat.HasValue(21))
          {
            textFormat.IsPreserveWhiteSpace = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.Display:
          if (!textFormat.HasValue(23))
          {
            textFormat.Hidden = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatKey.FontWeight:
          if (!textFormat.HasValue(2))
          {
            textFormat.Bold = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void ApplyImportantCSSStyle(
    HTMLConverterImpl.TextFormat textFormat,
    XmlNode node,
    CSSStyleItem styleItem)
  {
    foreach (KeyValuePair<CSSStyleItem.TextFormatImportantKey, object> keyValuePair in styleItem.ImportantPropertiesHash)
    {
      switch (keyValuePair.Key)
      {
        case CSSStyleItem.TextFormatImportantKey.FontSize:
          textFormat.FontSize = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.FontFamily:
          textFormat.FontFamily = Convert.ToString(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.Underline:
          textFormat.Underline = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.Italic:
          textFormat.Italic = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.Strike:
          textFormat.Strike = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.FontColor:
          textFormat.FontColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BackColor:
          textFormat.BackColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.LineHeight:
          textFormat.LineHeight = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.TextAlign:
          textFormat.TextAlign = (HorizontalAlignment) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.TopMargin:
          textFormat.TopMargin = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.LeftMargin:
          textFormat.LeftMargin = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.BottomMargin:
          textFormat.BottomMargin = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.RightMargin:
          textFormat.RightMargin = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.TextIndent:
          textFormat.TextIndent = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PageBreakBefore:
          textFormat.PageBreakBefore = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PageBreakAfter:
          textFormat.PageBreakAfter = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.LetterSpacing:
          textFormat.CharacterSpacing = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.AllCaps:
          if (!textFormat.HasValue(20))
          {
            textFormat.AllCaps = Convert.ToBoolean(keyValuePair.Value);
            continue;
          }
          continue;
        case CSSStyleItem.TextFormatImportantKey.WhiteSpace:
          textFormat.IsPreserveWhiteSpace = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.Display:
          textFormat.Hidden = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderBottomColor:
          textFormat.Borders.BottomColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderBottomStyle:
          textFormat.Borders.BottomStyle = (BorderStyle) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderBottomWidth:
          textFormat.Borders.BottomWidth = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderLeftColor:
          textFormat.Borders.LeftColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderLeftStyle:
          textFormat.Borders.LeftStyle = (BorderStyle) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderLeftWidth:
          textFormat.Borders.LeftWidth = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderRightColor:
          textFormat.Borders.RightColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderRightStyle:
          textFormat.Borders.RightStyle = (BorderStyle) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderRightWidth:
          textFormat.Borders.RightWidth = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderTopColor:
          textFormat.Borders.TopColor = (Color) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderTopStyle:
          textFormat.Borders.TopStyle = (BorderStyle) keyValuePair.Value;
          continue;
        case CSSStyleItem.TextFormatImportantKey.BorderTopWidth:
          textFormat.Borders.TopWidth = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.FontWeight:
          textFormat.Bold = Convert.ToBoolean(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PaddingBottom:
          textFormat.Borders.BottomSpace = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PaddingLeft:
          textFormat.Borders.LeftSpace = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PaddingRight:
          textFormat.Borders.RightSpace = Convert.ToSingle(keyValuePair.Value);
          continue;
        case CSSStyleItem.TextFormatImportantKey.PaddingTop:
          textFormat.Borders.TopSpace = Convert.ToSingle(keyValuePair.Value);
          continue;
        default:
          continue;
      }
    }
  }

  private void ParseCSSTextFormatValue(string textFormat, CSSStyleItem CSSItem)
  {
    char[] chArray = new char[1]{ ' ' };
    string[] strArray1 = textFormat.Split(':', ';');
    HTMLConverterImpl.TextFormat textFormat1 = new HTMLConverterImpl.TextFormat();
    int index = 0;
    for (int length = strArray1.Length; index < length - 1; index += 2)
    {
      string lower = strArray1[index].Trim().ToLower();
      string str1 = strArray1[index + 1].Trim().ToLower();
      if (!string.IsNullOrEmpty(str1))
      {
        switch (lower)
        {
          case "font-size":
            if (str1.Contains("!important"))
            {
              string paramValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (paramValue != "initial" && paramValue != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.FontSize] = (object) (float) this.ConvertSize(paramValue, 12f);
                continue;
              }
              continue;
            }
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.FontSize] = (object) (float) this.ConvertSize(str1, 12f);
              continue;
            }
            continue;
          case "font-family":
            if (str1.Contains("!important"))
            {
              string paramValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.FontFamily] = (object) Convert.ToString(this.GetFontName(paramValue));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.FontFamily] = (object) Convert.ToString(this.GetFontName(str1));
            continue;
          case "font-weight":
            if (str1.Contains("!important"))
            {
              string str2 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.FontWeight] = (object) (!(str2 == "normal") && !(str2 == "lighter"));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.FontWeight] = (object) (!(str1 == "normal") && !(str1 == "lighter"));
            continue;
          case "display":
            if (str1.Contains("!important"))
            {
              string str3 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.Display] = (object) (str3 == "none");
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.Display] = (object) (bool) (str1 == "none" ? (Convert.ToBoolean(true) ? 1 : 0) : (Convert.ToBoolean(false) ? 1 : 0));
            continue;
          case "white-space":
            if (str1.Contains("!important"))
            {
              string str4 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.WhiteSpace] = (object) (str4 == "pre" || str4 == "normal" || str4 == "nowrap" || str4 == "pre - line" || str4 == "pre - wrap" || str4 == "initial" || str4 == "inherit");
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.WhiteSpace] = (object) (str1 == "pre" || str1 == "normal" || str1 == "nowrap" || str1 == "pre - line" || str1 == "pre - wrap" || str1 == "initial" || str1 == "inherit");
            continue;
          case "text-transform":
            if (str1.Contains("!important"))
            {
              string str5 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.AllCaps] = (object) (str5 == "uppercase");
              continue;
            }
            switch (str1)
            {
              case "uppercase":
                CSSItem[CSSStyleItem.TextFormatKey.AllCaps] = (object) true;
                continue;
              case "capitalize":
                CSSItem[CSSStyleItem.TextFormatKey.Capitalize] = (object) true;
                continue;
              case "lowercase":
                CSSItem[CSSStyleItem.TextFormatKey.Lowercase] = (object) true;
                continue;
              case "none":
              case "initial":
                CSSItem[CSSStyleItem.TextFormatKey.AllCaps] = (object) false;
                CSSItem[CSSStyleItem.TextFormatKey.Capitalize] = (object) false;
                CSSItem[CSSStyleItem.TextFormatKey.Lowercase] = (object) false;
                continue;
              default:
                continue;
            }
          case "letter-spacing":
            if (str1.Contains("!important"))
            {
              string str6 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.LetterSpacing] = str6 == "normal" || str6 == "initial" || str6 == "inherit" ? (object) false : (object) Convert.ToSingle(this.ExtractValue(str6), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.LetterSpacing] = str1 == "normal" || str1 == "initial" || str1 == "inherit" ? (object) false : (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
            continue;
          case "font-style":
            if (str1.Contains("!important"))
            {
              switch (str1.Substring(0, str1.IndexOf('!')).Trim())
              {
                case "italic":
                case "oblique":
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Italic] = (object) true;
                  continue;
                case "strike":
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Strike] = (object) true;
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              switch (str1)
              {
                case "italic":
                case "oblique":
                  CSSItem[CSSStyleItem.TextFormatKey.Italic] = (object) true;
                  continue;
                case "strike":
                  CSSItem[CSSStyleItem.TextFormatKey.Strike] = (object) true;
                  continue;
                default:
                  continue;
              }
            }
          case "text-align":
            if (str1.Contains("!important"))
            {
              string val = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.TextAlign] = (object) this.GetHorizontalAlignment(val);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.TextAlign] = (object) this.GetHorizontalAlignment(str1);
            continue;
          case "text-decoration":
            if (str1.Contains("!important"))
            {
              switch (str1.Substring(0, str1.IndexOf('!')).Trim())
              {
                case "underline":
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Underline] = (object) true;
                  continue;
                case "line-through":
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Strike] = (object) true;
                  continue;
                case "none":
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Strike] = (object) false;
                  CSSItem[CSSStyleItem.TextFormatImportantKey.Underline] = (object) false;
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              switch (str1)
              {
                case "underline":
                  CSSItem[CSSStyleItem.TextFormatKey.Underline] = (object) true;
                  continue;
                case "line-through":
                  CSSItem[CSSStyleItem.TextFormatKey.Strike] = (object) true;
                  continue;
                case "none":
                  CSSItem[CSSStyleItem.TextFormatKey.Strike] = (object) false;
                  CSSItem[CSSStyleItem.TextFormatKey.Underline] = (object) false;
                  continue;
                default:
                  continue;
              }
            }
          case "color":
            if (str1.Contains("!important"))
            {
              string attValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (attValue != "initial" && attValue != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.FontColor] = (object) this.GetColor(attValue);
                continue;
              }
              continue;
            }
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.FontColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "background":
            string[] strArray2 = str1.Split(chArray);
            if (strArray2.Length == 5)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackColor] = (object) this.GetColor(strArray2[0]);
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundImage] = (object) strArray2[1];
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundRepeat] = (object) strArray2[2];
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundAttachment] = (object) strArray2[3];
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundPosition] = (object) strArray2[4];
              continue;
            }
            continue;
          case "background-color":
            if (str1.Contains("!important"))
            {
              string attValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (CSSItem.StyleName != "table" && CSSItem.StyleName != "th" && CSSItem.StyleName != "td")
                CSSItem[CSSStyleItem.TextFormatImportantKey.BackColor] = (object) this.GetColor(attValue);
              if (attValue == "transparent")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BackColor] = (object) Color.Empty;
                continue;
              }
              continue;
            }
            if (CSSItem.StyleName != "table" && CSSItem.StyleName != "th" && CSSItem.StyleName != "td")
              CSSItem[CSSStyleItem.TextFormatKey.BackColor] = (object) this.GetColor(str1);
            if (str1 == "transparent")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackColor] = (object) Color.Empty;
              continue;
            }
            continue;
          case "text-indent":
            if (str1.Contains("!important"))
            {
              string str7 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str7 != "initial" && str7 != "inherit" && CSSItem.StyleName != "ul" && CSSItem.StyleName != "ol")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.TextIndent] = (object) Convert.ToSingle(this.ExtractValue(str7), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "initial" && str1 != "inherit" && CSSItem.StyleName != "ul" && CSSItem.StyleName != "ol")
            {
              CSSItem[CSSStyleItem.TextFormatKey.TextIndent] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "margin-left":
            if (str1.Contains("!important"))
            {
              string str8 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str8 != "auto" && str8 != "initial" && str8 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(str8), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "margin-right":
            if (str1.Contains("!important"))
            {
              string str9 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str9 != "auto" && str9 != "initial" && str9 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(str9), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "margin-top":
            if (str1.Contains("!important"))
            {
              string str10 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.TopMargin] = (object) (float) (!(str10 != "auto") || !(str10 != "initial") || !(str10 != "inherit") ? -1.0 : (double) Convert.ToSingle(this.ExtractValue(str10), (IFormatProvider) CultureInfo.InvariantCulture));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.TopMargin] = (object) (float) (!(str1 != "auto") || !(str1 != "initial") || !(str1 != "inherit") ? -1.0 : (double) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case "margin-bottom":
            if (str1.Contains("!important"))
            {
              string str11 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BottomMargin] = (object) (float) (!(str11 != "auto") || !(str11 != "initial") || !(str11 != "inherit") ? -1.0 : (double) Convert.ToSingle(this.ExtractValue(str11), (IFormatProvider) CultureInfo.InvariantCulture));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BottomMargin] = (object) (float) (!(str1 != "auto") || !(str1 != "initial") || !(str1 != "inherit") ? -1.0 : (double) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case "margin":
            if (str1.Contains("!important"))
            {
              string[] strArray3 = str1.Substring(0, str1.IndexOf('!')).Trim().Split(chArray);
              switch (strArray3.Length)
              {
                case 1:
                  if (strArray3[0] != "auto" && strArray3[0] != "initial" && strArray3[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 2:
                  if (strArray3[0] != "auto" && strArray3[0] != "initial" && strArray3[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray3[1] != "auto" && strArray3[1] != "initial" && strArray3[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 3:
                  if (strArray3[0] != "auto" && strArray3[0] != "initial" && strArray3[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray3[1] != "auto" && strArray3[1] != "initial" && strArray3[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray3[2] != "auto" && strArray3[2] != "initial" && strArray3[2] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 4:
                  if (strArray3[0] != "auto" && strArray3[0] != "initial" && strArray3[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray3[1] != "auto" && strArray3[1] != "initial" && strArray3[1] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray3[2] != "auto" && strArray3[2] != "initial" && strArray3[2] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray3[3] != "auto" && strArray3[3] != "initial" && strArray3[3] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray3[3]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              string[] strArray4 = str1.Split(chArray);
              switch (strArray4.Length)
              {
                case 1:
                  if (strArray4[0] != "auto" && strArray4[0] != "initial" && strArray4[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 2:
                  if (strArray4[0] != "auto" && strArray4[0] != "initial" && strArray4[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray4[1] != "auto" && strArray4[1] != "initial" && strArray4[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 3:
                  if (strArray4[0] != "auto" && strArray4[0] != "initial" && strArray4[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray4[1] != "auto" && strArray4[1] != "initial" && strArray4[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray4[2] != "auto" && strArray4[2] != "initial" && strArray4[2] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 4:
                  if (strArray4[0] != "auto" && strArray4[0] != "initial" && strArray4[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.TopMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray4[1] != "auto" && strArray4[1] != "initial" && strArray4[1] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.RightMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray4[2] != "auto" && strArray4[2] != "initial" && strArray4[2] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.BottomMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray4[3] != "auto" && strArray4[3] != "initial" && strArray4[3] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.LeftMargin] = (object) Convert.ToSingle(this.ExtractValue(strArray4[3]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          case "padding":
            if (str1.Contains("!important"))
            {
              string[] strArray5 = str1.Substring(0, str1.IndexOf('!')).Trim().Split(chArray);
              switch (strArray5.Length)
              {
                case 1:
                  if (strArray5[0] != "auto" && strArray5[0] != "initial" && strArray5[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 2:
                  if (strArray5[0] != "auto" && strArray5[0] != "initial" && strArray5[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray5[1] != "auto" && strArray5[1] != "initial" && strArray5[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 3:
                  if (strArray5[0] != "auto" && strArray5[0] != "initial" && strArray5[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray5[1] != "auto" && strArray5[1] != "initial" && strArray5[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray5[2] != "auto" && strArray5[2] != "initial" && strArray5[2] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray5[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 4:
                  if (strArray5[0] != "auto" && strArray5[0] != "initial" && strArray5[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray5[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray5[1] != "auto" && strArray5[1] != "initial" && strArray5[1] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray5[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray5[2] != "auto" && strArray5[2] != "initial" && strArray5[2] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray5[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray5[3] != "auto" && strArray5[3] != "initial" && strArray5[3] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray5[3]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              string[] strArray6 = str1.Split(chArray);
              switch (strArray6.Length)
              {
                case 1:
                  if (strArray6[0] != "auto" && strArray6[0] != "initial" && strArray6[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 2:
                  if (strArray6[0] != "auto" && strArray6[0] != "initial" && strArray6[0] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray6[1] != "auto" && strArray6[1] != "initial" && strArray6[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray6[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray6[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 3:
                  if (strArray6[0] != "auto" && strArray6[0] != "initial" && strArray6[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray6[1] != "auto" && strArray6[1] != "initial" && strArray6[1] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray6[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray6[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (strArray6[2] != "auto" && strArray6[2] != "initial" && strArray6[2] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray6[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                case 4:
                  if (strArray6[0] != "auto" && strArray6[0] != "initial" && strArray6[0] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(strArray6[0]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray6[1] != "auto" && strArray6[1] != "initial" && strArray6[1] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(strArray6[1]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray6[2] != "auto" && strArray6[2] != "initial" && strArray6[2] != "inherit")
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(strArray6[2]), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray6[3] != "auto" && strArray6[3] != "initial" && strArray6[3] != "inherit")
                  {
                    CSSItem[CSSStyleItem.TextFormatKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(strArray6[3]), (IFormatProvider) CultureInfo.InvariantCulture);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          case "padding-left":
            if (str1.Contains("!important"))
            {
              string str12 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str12 != "auto" && str12 != "initial" && str12 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(str12), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.PaddingLeft] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "padding-top":
            if (str1.Contains("!important"))
            {
              string str13 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str13 != "auto" && str13 != "initial" && str13 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(str13), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.PaddingTop] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "padding-right":
            if (str1.Contains("!important"))
            {
              string str14 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str14 != "auto" && str14 != "initial" && str14 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(str14), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.PaddingRight] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "padding-bottom":
            if (str1.Contains("!important"))
            {
              string str15 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str15 != "auto" && str15 != "initial" && str15 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(str15), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.PaddingBottom] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "word-break":
            CSSItem[CSSStyleItem.TextFormatKey.WordBreak] = (object) str1;
            continue;
          case "page-break-before":
            if (str1.Contains("!important"))
            {
              string paramvalue = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.PageBreakBefore] = (object) this.ConvertToBoolValue(paramvalue);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatImportantKey.PageBreakBefore] = (object) this.ConvertToBoolValue(str1);
            continue;
          case "page-break-inside":
            CSSItem[CSSStyleItem.TextFormatKey.PageBreakInside] = (object) this.ConvertToBoolValue(str1);
            continue;
          case "border-bottom":
            if (str1.Contains("!important"))
            {
              string paramValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              paramValue.Split(chArray);
              this.ParseParagraphBorder(paramValue, ref textFormat1.Borders.BottomColor, ref textFormat1.Borders.BottomWidth, ref textFormat1.Borders.BottomStyle);
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomWidth] = (object) textFormat1.Borders.BottomWidth;
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) textFormat1.Borders.BottomStyle;
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) textFormat1.Borders.BottomColor;
              continue;
            }
            str1.Split(chArray);
            this.ParseParagraphBorder(str1, ref textFormat1.Borders.BottomColor, ref textFormat1.Borders.BottomWidth, ref textFormat1.Borders.BottomStyle);
            CSSItem[CSSStyleItem.TextFormatKey.BorderBottomWidth] = (object) textFormat1.Borders.BottomWidth;
            CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) textFormat1.Borders.BottomStyle;
            CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) textFormat1.Borders.BottomColor;
            continue;
          case "border-top":
            if (str1.Contains("!important"))
            {
              string paramValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              paramValue.Split(chArray);
              this.ParseParagraphBorder(paramValue, ref textFormat1.Borders.TopColor, ref textFormat1.Borders.TopWidth, ref textFormat1.Borders.TopStyle);
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopWidth] = (object) textFormat1.Borders.TopWidth;
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) textFormat1.Borders.TopStyle;
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) textFormat1.Borders.TopColor;
              continue;
            }
            str1.Split(chArray);
            this.ParseParagraphBorder(str1, ref textFormat1.Borders.TopColor, ref textFormat1.Borders.TopWidth, ref textFormat1.Borders.TopStyle);
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopWidth] = (object) textFormat1.Borders.TopWidth;
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopStyle] = (object) textFormat1.Borders.TopStyle;
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) textFormat1.Borders.TopColor;
            continue;
          case "border-left":
            if (str1.Contains("!important"))
            {
              string[] strArray7 = str1.Substring(0, str1.IndexOf('!')).Trim().Split(chArray);
              if (strArray7.Length == 3)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomWidth] = (object) this.CalculateBorderWidth(strArray7[0]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) strArray7[1];
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) strArray7[2];
                continue;
              }
              continue;
            }
            string[] strArray8 = str1.Split(chArray);
            if (strArray8.Length == 3)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomWidth] = (object) this.CalculateBorderWidth(strArray8[0]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) strArray8[1];
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) strArray8[2];
              continue;
            }
            continue;
          case "border-right":
            if (str1.Contains("!important"))
            {
              string[] strArray9 = str1.Substring(0, str1.IndexOf('!')).Trim().Split(chArray);
              if (strArray9.Length == 3)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomWidth] = (object) this.CalculateBorderWidth(strArray9[0]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) strArray9[1];
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) strArray9[2];
                continue;
              }
              continue;
            }
            string[] strArray10 = str1.Split(chArray);
            if (strArray10.Length == 3)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomWidth] = (object) this.CalculateBorderWidth(strArray10[0]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) strArray10[1];
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) strArray10[2];
              continue;
            }
            continue;
          case "outline-color":
          case "border-color":
            if (str1.Contains("!important"))
            {
              string str16 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str16 != "initial" && str16 != "inherit" && str16 != "transparent")
              {
                string[] strArray11 = str16.Split(chArray);
                switch (strArray11.Length)
                {
                  case 1:
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(strArray11[0]);
                    continue;
                  case 2:
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftColor] = (object) this.GetColor(strArray11[1]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(strArray11[1]);
                    continue;
                  case 3:
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftColor] = (object) this.GetColor(strArray11[1]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(strArray11[1]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) this.GetColor(strArray11[2]);
                    continue;
                  case 4:
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) this.GetColor(strArray11[0]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(strArray11[1]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) this.GetColor(strArray11[2]);
                    CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(strArray11[3]);
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
            else if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
            {
              string[] strArray12 = str1.Split(chArray);
              switch (strArray12.Length)
              {
                case 1:
                  CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderLeftColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(strArray12[0]);
                  continue;
                case 2:
                  CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderLeftColor] = (object) this.GetColor(strArray12[1]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(strArray12[1]);
                  continue;
                case 3:
                  CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderLeftColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(strArray12[1]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) this.GetColor(strArray12[2]);
                  continue;
                case 4:
                  CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) this.GetColor(strArray12[0]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(strArray12[1]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) this.GetColor(strArray12[2]);
                  CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(strArray12[3]);
                  continue;
                default:
                  continue;
              }
            }
            else
              continue;
          case "border-left-color":
            if (str1.Contains("!important"))
            {
              string attValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (attValue != "initial" && attValue != "inherit" && attValue != "transparent")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftColor] = (object) this.GetColor(attValue);
                continue;
              }
              continue;
            }
            if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderLeftColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "border-right-color":
            if (str1.Contains("!important"))
            {
              string attValue = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (attValue != "initial" && attValue != "inherit" && attValue != "transparent")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightColor] = (object) this.GetColor(attValue);
                continue;
              }
              continue;
            }
            if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderRightColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "border-top-color":
            if (str1.Contains("!important"))
            {
              str1 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopColor] = (object) this.GetColor(str1);
            }
            if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderTopColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "border-bottom-color":
            if (str1.Contains("!important"))
            {
              str1 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomColor] = (object) this.GetColor(str1);
            }
            if (str1 != "initial" && str1 != "inherit" && str1 != "transparent")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "outline-width":
          case "border-width":
            if (str1.Contains("!important"))
            {
              string str17 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str17));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str1));
            continue;
          case "border-left-width":
            if (str1.Contains("!important"))
            {
              string str18 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str18));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderLeftWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str1));
            continue;
          case "border-right-width":
            if (str1.Contains("!important"))
            {
              string str19 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str19));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderRightWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str1));
            continue;
          case "border-top-width":
            if (str1.Contains("!important"))
            {
              string str20 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str20));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str1));
            continue;
          case "border-bottom-width":
            if (str1.Contains("!important"))
            {
              string str21 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str21));
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderBottomWidth] = (object) Convert.ToSingle(this.CalculateBorderWidth(str1));
            continue;
          case "outline-style":
          case "border-style":
            if (str1.Contains("!important"))
            {
              string type = str1.Substring(0, str1.IndexOf('!')).Trim();
              string[] strArray13 = type.Split(new char[1]
              {
                ' '
              }, StringSplitOptions.RemoveEmptyEntries);
              if (strArray13.Length == 1)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderStyle] = (object) this.ToBorderType(type);
                continue;
              }
              if (strArray13.Length == 2)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) this.ToBorderType(strArray13[0]);
                CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) this.ToBorderType(strArray13[0]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(strArray13[1]);
                CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(strArray13[1]);
                continue;
              }
              if (strArray13.Length == 3)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) this.ToBorderType(strArray13[0]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(strArray13[1]);
                CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(strArray13[1]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) this.ToBorderType(strArray13[2]);
                continue;
              }
              if (strArray13.Length == 4)
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) this.ToBorderType(strArray13[0]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightStyle] = (object) this.ToBorderType(strArray13[1]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) this.ToBorderType(strArray13[2]);
                CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(strArray13[3]);
                continue;
              }
              continue;
            }
            string[] strArray14 = str1.Split(new char[1]
            {
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray14.Length == 1)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderStyle] = (object) this.ToBorderType(str1);
              continue;
            }
            if (strArray14.Length == 2)
            {
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) this.ToBorderType(strArray14[0]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) this.ToBorderType(strArray14[0]);
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(strArray14[1]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(strArray14[1]);
              continue;
            }
            if (strArray14.Length == 3)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderTopStyle] = (object) this.ToBorderType(strArray14[0]);
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(strArray14[1]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(strArray14[1]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) this.ToBorderType(strArray14[2]);
              continue;
            }
            if (strArray14.Length == 4)
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderTopStyle] = (object) this.ToBorderType(strArray14[0]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(strArray14[1]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) this.ToBorderType(strArray14[2]);
              CSSItem[CSSStyleItem.TextFormatKey.BorderLeftStyle] = (object) this.ToBorderType(strArray14[3]);
              continue;
            }
            continue;
          case "border-left-style":
            if (str1.Contains("!important"))
            {
              string type = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderLeftStyle] = (object) this.ToBorderType(type);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderLeftStyle] = (object) this.ToBorderType(str1);
            continue;
          case "border-right-style":
            if (str1.Contains("!important"))
            {
              string type = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderRightStyle] = (object) this.ToBorderType(type);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderRightStyle] = (object) this.ToBorderType(str1);
            continue;
          case "border-top-style":
            if (str1.Contains("!important"))
            {
              string type = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderTopStyle] = (object) this.ToBorderType(type);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopStyle] = (object) this.ToBorderType(str1);
            continue;
          case "border-bottom-style":
            if (str1.Contains("!important"))
            {
              string type = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BorderBottomStyle] = (object) this.ToBorderType(type);
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.BorderBottomStyle] = (object) this.ToBorderType(str1);
            continue;
          case "border":
            if (str1.Contains("!important"))
            {
              string str22 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.Border] = (object) str22;
              continue;
            }
            CSSItem[CSSStyleItem.TextFormatKey.Border] = (object) str1;
            continue;
          case "line-height":
            if (str1.Contains("!important"))
            {
              string str23 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str23 == "normal")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.LineHeight] = (object) 12f;
                continue;
              }
              if (str23 != "initial" && str23 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.LineHeight] = (object) Convert.ToSingle(this.ExtractValue(str23), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 == "normal")
            {
              CSSItem[CSSStyleItem.TextFormatKey.LineHeight] = (object) 12f;
              continue;
            }
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.LineHeight] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "align-content":
            CSSItem[CSSStyleItem.TextFormatKey.ContentAlign] = (object) str1;
            continue;
          case "align-items":
            CSSItem[CSSStyleItem.TextFormatKey.ItemsAlign] = (object) str1;
            continue;
          case "align-self":
            CSSItem[CSSStyleItem.TextFormatKey.SelfAlign] = (object) str1;
            continue;
          case "animation":
            CSSItem[CSSStyleItem.TextFormatKey.Animation] = (object) str1;
            continue;
          case "animation-delay":
            if (str1 != "initial" && str1 != "inherit" && (str1.Contains("ms") || str1.Contains("s")))
            {
              if (str1.EndsWith("ms"))
              {
                string str24 = str1.TrimEnd('s').TrimEnd('m');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromMilliseconds(Convert.ToDouble(str24));
                continue;
              }
              if (str1.EndsWith("s"))
              {
                string str25 = str1.TrimEnd('s');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromSeconds(Convert.ToDouble(str25));
                continue;
              }
              continue;
            }
            continue;
          case "animation-direction":
            CSSItem[CSSStyleItem.TextFormatKey.AnimationDirection] = (object) str1;
            continue;
          case "animation-duration":
            if (str1 != "initial" && str1 != "inherit" && (str1.Contains("ms") || str1.Contains("s")))
            {
              if (str1.EndsWith("ms"))
              {
                string str26 = str1.TrimEnd('s').TrimEnd('m');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromMilliseconds(Convert.ToDouble(str26));
                continue;
              }
              if (str1.EndsWith("s"))
              {
                string str27 = str1.TrimEnd('s');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromSeconds(Convert.ToDouble(str27));
                continue;
              }
              continue;
            }
            continue;
          case "animation-fill-mode":
            CSSItem[CSSStyleItem.TextFormatKey.AnimationFillMode] = (object) str1;
            continue;
          case "animation-iteration-count":
            if (str1 != "initial" && str1 != "inherit" && str1 != "infinite")
            {
              CSSItem[CSSStyleItem.TextFormatKey.AnimationIterationCount] = (object) Convert.ToInt32(str1);
              continue;
            }
            continue;
          case "animation-name":
            if (str1 != "initial" && str1 != "inherit" && str1 != "none")
            {
              CSSItem[CSSStyleItem.TextFormatKey.AnimationName] = (object) str1;
              continue;
            }
            continue;
          case "animation-play-state":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.AnimationPlayState] = (object) str1;
              continue;
            }
            continue;
          case "animation-timing-function":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.AnimationTimingFunction] = (object) str1;
              continue;
            }
            continue;
          case "backface-visibility":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackfaceVisibility] = (object) str1;
              continue;
            }
            continue;
          case "background-attachment":
            if (str1.Contains("!important"))
            {
              string str28 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BackgroundAttachment] = (object) str28;
              continue;
            }
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundAttachment] = (object) str1;
              continue;
            }
            continue;
          case "background-clip":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundClip] = (object) str1;
              continue;
            }
            continue;
          case "background-image":
            if (str1 != "none" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundImage] = (object) str1;
              continue;
            }
            continue;
          case "background-origin":
            CSSItem[CSSStyleItem.TextFormatKey.BackgroundOrigin] = (object) str1;
            continue;
          case "background-position":
            CSSItem[CSSStyleItem.TextFormatKey.BackgroundPosition] = (object) str1;
            continue;
          case "background-repeat":
            if (str1.Contains("!important"))
            {
              string str29 = str1.Substring(0, str1.IndexOf('!')).Trim();
              CSSItem[CSSStyleItem.TextFormatImportantKey.BackgroundRepeat] = (object) str29;
              continue;
            }
            if (str1 != "no-repeat")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BackgroundRepeat] = (object) str1;
              continue;
            }
            continue;
          case "background-size":
            CSSItem[CSSStyleItem.TextFormatKey.BackgroundSize] = (object) str1;
            continue;
          case "border-bottom-left-radius":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomLeftRadius] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "border-bottom-right-radius":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderBottomRightRadius] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "border-collapse":
            CSSItem[CSSStyleItem.TextFormatKey.BorderCollapse] = (object) str1;
            continue;
          case "border-image":
            CSSItem[CSSStyleItem.TextFormatKey.BorderImage] = (object) str1;
            continue;
          case "border-image-outset":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderImageOutset] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "border-image-repeat":
            CSSItem[CSSStyleItem.TextFormatKey.BorderImageRepeat] = (object) str1;
            continue;
          case "border-image-slice":
            CSSItem[CSSStyleItem.TextFormatKey.BorderImageSlice] = (object) str1;
            continue;
          case "border-image-source":
            CSSItem[CSSStyleItem.TextFormatKey.BorderImageSource] = (object) str1;
            continue;
          case "border-image-width":
            if (str1 != "initial" && str1 != "inherit" && str1 != "auto")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderImageWidth] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "border-radius":
            CSSItem[CSSStyleItem.TextFormatKey.BorderRadius] = (object) str1;
            continue;
          case "border-spacing":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.BorderSpacing] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "border-top-left-radius":
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopLeftRadius] = (object) str1;
            continue;
          case "border-top-right-radius":
            CSSItem[CSSStyleItem.TextFormatKey.BorderTopRightRadius] = (object) str1;
            continue;
          case "bottom":
            CSSItem[CSSStyleItem.TextFormatKey.Bottom] = (object) str1;
            continue;
          case "box-shadow":
            CSSItem[CSSStyleItem.TextFormatKey.BoxShadow] = (object) str1;
            continue;
          case "box-sizing":
            CSSItem[CSSStyleItem.TextFormatKey.BoxSizing] = (object) str1;
            continue;
          case "caption-side":
            CSSItem[CSSStyleItem.TextFormatKey.CaptionSide] = (object) str1;
            continue;
          case "clear":
            CSSItem[CSSStyleItem.TextFormatKey.Clear] = (object) str1;
            continue;
          case "clip":
            CSSItem[CSSStyleItem.TextFormatKey.Clip] = (object) str1;
            continue;
          case "column-count":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.ColumnCount] = (object) Convert.ToInt32(str1);
              continue;
            }
            continue;
          case "column-fill":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnFill] = (object) str1;
            continue;
          case "column-gap":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnGap] = (object) str1;
            continue;
          case "column-rule":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnRule] = (object) str1;
            continue;
          case "column-rule-color":
            if (str1 != "initial" && str1 != " inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.ColumnRuleColor] = (object) this.GetColor(str1);
              continue;
            }
            continue;
          case "column-rule-style":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnRuleStyle] = (object) str1;
            continue;
          case "column-rule-width":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnRuleWidth] = (object) str1;
            continue;
          case "column-span":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnSpan] = (object) str1;
            continue;
          case "column-width":
            CSSItem[CSSStyleItem.TextFormatKey.ColumnWidth] = (object) str1;
            continue;
          case "columns":
            CSSItem[CSSStyleItem.TextFormatKey.Columns] = (object) str1;
            continue;
          case "content":
            CSSItem[CSSStyleItem.TextFormatKey.Content] = (object) str1;
            continue;
          case "counter-increment":
            CSSItem[CSSStyleItem.TextFormatKey.CounterIncrement] = (object) str1;
            continue;
          case "counter-reset":
            CSSItem[CSSStyleItem.TextFormatKey.CounterReset] = (object) str1;
            continue;
          case "cursor":
            CSSItem[CSSStyleItem.TextFormatKey.Cursor] = (object) str1;
            continue;
          case "direction":
            CSSItem[CSSStyleItem.TextFormatKey.Direction] = (object) str1;
            continue;
          case "empty-cells":
            if (str1.Contains("!important"))
            {
              string str30 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str30 != "hide" || str30 != "initial" || str30 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.Display] = (object) true;
                continue;
              }
              continue;
            }
            if (str1 != "hide" || str1 != "initial" || str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.Display] = (object) true;
              continue;
            }
            continue;
          case "flex":
            CSSItem[CSSStyleItem.TextFormatKey.Flex] = (object) str1;
            continue;
          case "flex-basis":
            CSSItem[CSSStyleItem.TextFormatKey.FlexBasis] = (object) str1;
            continue;
          case "flex-direction":
            CSSItem[CSSStyleItem.TextFormatKey.FlexDirection] = (object) str1;
            continue;
          case "flex-flow":
            CSSItem[CSSStyleItem.TextFormatKey.FlexFlow] = (object) str1;
            continue;
          case "flex-grow":
            CSSItem[CSSStyleItem.TextFormatKey.FlexGrow] = (object) str1;
            continue;
          case "flex-shrink":
            CSSItem[CSSStyleItem.TextFormatKey.FlexShrink] = (object) str1;
            continue;
          case "flex-wrap":
            CSSItem[CSSStyleItem.TextFormatKey.FlexWrap] = (object) str1;
            continue;
          case "float":
            CSSItem[CSSStyleItem.TextFormatKey.Float] = (object) str1;
            continue;
          case "font":
            CSSItem[CSSStyleItem.TextFormatKey.Display] = (object) str1;
            continue;
          case "@font-face":
            CSSItem[CSSStyleItem.TextFormatKey.FontFace] = (object) str1;
            continue;
          case "font-size-adjust":
            CSSItem[CSSStyleItem.TextFormatKey.FontSizeAdjust] = (object) str1;
            continue;
          case "font-stretch":
            CSSItem[CSSStyleItem.TextFormatKey.FontStretch] = (object) str1;
            continue;
          case "font-variant":
            CSSItem[CSSStyleItem.TextFormatKey.FontVariant] = (object) str1;
            continue;
          case "hanging-punctuation":
            CSSItem[CSSStyleItem.TextFormatKey.HangingPunctuation] = (object) str1;
            continue;
          case "height":
            CSSItem[CSSStyleItem.TextFormatKey.Height] = (object) str1;
            continue;
          case "icon":
            CSSItem[CSSStyleItem.TextFormatKey.Icon] = (object) str1;
            continue;
          case "justify-content":
            CSSItem[CSSStyleItem.TextFormatKey.JustifyContent] = (object) str1;
            continue;
          case "@keyframes":
            CSSItem[CSSStyleItem.TextFormatKey.KeyFrames] = (object) str1;
            continue;
          case "left":
            CSSItem[CSSStyleItem.TextFormatKey.Left] = (object) str1;
            continue;
          case "list-style":
            CSSItem[CSSStyleItem.TextFormatKey.ListStyle] = (object) str1;
            continue;
          case "list-style-image":
            CSSItem[CSSStyleItem.TextFormatKey.listStyleImage] = (object) str1;
            continue;
          case "list-style-position":
            CSSItem[CSSStyleItem.TextFormatKey.listStylePosition] = (object) str1;
            continue;
          case "list-style-type":
            CSSItem[CSSStyleItem.TextFormatKey.ListStyleType] = (object) str1;
            continue;
          case "max-height":
            CSSItem[CSSStyleItem.TextFormatKey.MaxHeight] = (object) str1;
            continue;
          case "max-width":
            CSSItem[CSSStyleItem.TextFormatKey.MaxWidth] = (object) str1;
            continue;
          case "min-height":
            CSSItem[CSSStyleItem.TextFormatKey.MinHeight] = (object) str1;
            continue;
          case "min-width":
            CSSItem[CSSStyleItem.TextFormatKey.MinWidth] = (object) str1;
            continue;
          case "nav-down":
            CSSItem[CSSStyleItem.TextFormatKey.NavDown] = (object) str1;
            continue;
          case "nav-index":
            CSSItem[CSSStyleItem.TextFormatKey.NavIndex] = (object) str1;
            continue;
          case "nav-left":
            CSSItem[CSSStyleItem.TextFormatKey.NavLeft] = (object) str1;
            continue;
          case "nav-right":
            CSSItem[CSSStyleItem.TextFormatKey.NavRight] = (object) str1;
            continue;
          case "nav-up":
            CSSItem[CSSStyleItem.TextFormatKey.NavUp] = (object) str1;
            continue;
          case "opacity":
            if (str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.Opacity] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "order":
            CSSItem[CSSStyleItem.TextFormatKey.Order] = (object) str1;
            continue;
          case "outline":
            CSSItem[CSSStyleItem.TextFormatKey.Outline] = (object) str1;
            continue;
          case "outline-offset":
            CSSItem[CSSStyleItem.TextFormatKey.OutlineOffset] = (object) str1;
            continue;
          case "overflow":
            CSSItem[CSSStyleItem.TextFormatKey.OverFlow] = (object) str1;
            continue;
          case "overflow-x":
            CSSItem[CSSStyleItem.TextFormatKey.Overflow_X] = (object) str1;
            continue;
          case "overflow-y":
            CSSItem[CSSStyleItem.TextFormatKey.Overflow_Y] = (object) str1;
            continue;
          case "perspective":
            CSSItem[CSSStyleItem.TextFormatKey.Perspective] = (object) str1;
            continue;
          case "perspective-origin":
            CSSItem[CSSStyleItem.TextFormatKey.PerspectiveOrigin] = (object) str1;
            continue;
          case "position":
            CSSItem[CSSStyleItem.TextFormatKey.Position] = (object) str1;
            continue;
          case "quotes":
            CSSItem[CSSStyleItem.TextFormatKey.Quotes] = (object) str1;
            continue;
          case "resize":
            CSSItem[CSSStyleItem.TextFormatKey.Resize] = (object) str1;
            continue;
          case "right":
            CSSItem[CSSStyleItem.TextFormatKey.Right] = (object) str1;
            continue;
          case "table-layout":
            CSSItem[CSSStyleItem.TextFormatKey.TableLayout] = (object) str1;
            continue;
          case "text-align-last":
            CSSItem[CSSStyleItem.TextFormatKey.TextAlignLast] = (object) str1;
            continue;
          case "text-decoration-color":
            CSSItem[CSSStyleItem.TextFormatKey.TextDecorationColor] = (object) str1;
            continue;
          case "text-decoration-line":
            CSSItem[CSSStyleItem.TextFormatKey.TextDecorationLine] = (object) str1;
            continue;
          case "text-justify":
            CSSItem[CSSStyleItem.TextFormatKey.TextJustify] = (object) str1;
            continue;
          case "text-overflow":
            CSSItem[CSSStyleItem.TextFormatKey.TextOverflow] = (object) str1;
            continue;
          case "text-shadow":
            CSSItem[CSSStyleItem.TextFormatKey.TextShadow] = (object) str1;
            continue;
          case "top":
            CSSItem[CSSStyleItem.TextFormatKey.Top] = (object) str1;
            continue;
          case "transform":
            CSSItem[CSSStyleItem.TextFormatKey.Transform] = (object) str1;
            continue;
          case "transform-origin":
            CSSItem[CSSStyleItem.TextFormatKey.TransformOrigin] = (object) str1;
            continue;
          case "transform-style":
            CSSItem[CSSStyleItem.TextFormatKey.TransformStyle] = (object) str1;
            continue;
          case "transition":
            CSSItem[CSSStyleItem.TextFormatKey.Transition] = (object) str1;
            continue;
          case "transition-delay":
            CSSItem[CSSStyleItem.TextFormatKey.TransitionDelay] = (object) str1;
            continue;
          case "transition-duration":
            if (str1 != "initial" && str1 != "inherit" && (str1.Contains("ms") || str1.Contains("s")))
            {
              if (str1.EndsWith("ms"))
              {
                string str31 = str1.TrimEnd('s').TrimEnd('m');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromMilliseconds(Convert.ToDouble(str31));
                continue;
              }
              if (str1.EndsWith("s"))
              {
                string str32 = str1.TrimEnd('s');
                CSSItem[CSSStyleItem.TextFormatKey.TransitionDuration] = (object) TimeSpan.FromSeconds(Convert.ToDouble(str32));
                continue;
              }
              continue;
            }
            continue;
          case "transition-property":
            CSSItem[CSSStyleItem.TextFormatKey.TransitionProperty] = (object) str1;
            continue;
          case "transition-timing-function":
            CSSItem[CSSStyleItem.TextFormatKey.TransitionTimingFunction] = (object) str1;
            continue;
          case "unicode-bidi":
            CSSItem[CSSStyleItem.TextFormatKey.UnicodeBidi] = (object) str1;
            continue;
          case "vertical-align":
            CSSItem[CSSStyleItem.TextFormatKey.VerticalAlign] = (object) str1;
            continue;
          case "visibility":
            CSSItem[CSSStyleItem.TextFormatKey.Visibility] = !(str1 == "visible") ? (object) false : (object) true;
            continue;
          case "width":
            if (str1.Contains("!important"))
            {
              str1 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
                CSSItem[CSSStyleItem.TextFormatImportantKey.Width] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.Width] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "word-spacing":
            if (str1.Contains("!important"))
            {
              string str33 = str1.Substring(0, str1.IndexOf('!')).Trim();
              if (str33 != "auto" && str33 != "initial" && str33 != "inherit")
              {
                CSSItem[CSSStyleItem.TextFormatImportantKey.WordSpacing] = (object) Convert.ToSingle(this.ExtractValue(str33), (IFormatProvider) CultureInfo.InvariantCulture);
                continue;
              }
              continue;
            }
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.WordSpacing] = (object) Convert.ToSingle(this.ExtractValue(str1), (IFormatProvider) CultureInfo.InvariantCulture);
              continue;
            }
            continue;
          case "word-wrap":
            CSSItem[CSSStyleItem.TextFormatKey.WordWrap] = (object) str1;
            continue;
          case "z-index":
            if (str1 != "auto" && str1 != "initial" && str1 != "inherit")
            {
              CSSItem[CSSStyleItem.TextFormatKey.Zindex] = (object) Convert.ToInt32(str1);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void TraverseComments(XmlNode node)
  {
    string text = node.Value;
    if (!this.StartsWithExt(text, "[if supportfields]") && !this.StartsWithExt(text, "[if]"))
      return;
    int num = 0;
    int count = text.IndexOf("<");
    string str = text.Remove(0, count);
    if (str.EndsWith("![endif]"))
      num = str.LastIndexOf(">");
    string xml = this.ReplaceHtmlConstantByUnicodeChar($"<comment>{str.Remove(num + 1)}</comment>");
    XmlNode documentElement;
    try
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      documentElement = (XmlNode) xmlDocument.DocumentElement;
    }
    catch
    {
      return;
    }
    if (!documentElement.LocalName.Equals("comment", StringComparison.OrdinalIgnoreCase))
      return;
    this.TraverseChildNodes(documentElement.ChildNodes);
  }

  private void InsertFieldBegin(string fieldCode)
  {
    FieldType fieldType = FieldTypeDefiner.GetFieldType(fieldCode);
    WField wfield;
    switch (fieldType)
    {
      case FieldType.FieldIf:
        wfield = (WField) new WIfField((IWordDocument) this.m_currParagraph.Document);
        break;
      case FieldType.FieldMergeField:
        wfield = (WField) new WMergeField((IWordDocument) this.m_currParagraph.Document);
        break;
      default:
        wfield = new WField((IWordDocument) this.m_currParagraph.Document);
        break;
    }
    wfield.FieldType = fieldType;
    this.m_currParagraph.Items.Add((IEntity) wfield);
    this.ApplyTextFormatting(wfield.CharacterFormat);
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_currParagraph.Document);
    wtextRange.Text = fieldCode;
    wtextRange.ApplyCharacterFormat(wfield.CharacterFormat);
    this.m_currParagraph.Items.Add((IEntity) wtextRange);
    this.FieldStack.Push(wfield);
  }

  private void ParseFieldSeparator()
  {
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_currParagraph.Document, FieldMarkType.FieldSeparator);
    this.m_currParagraph.Items.Add((IEntity) wfieldMark);
    this.CurrentField.FieldSeparator = wfieldMark;
  }

  private void ParseFieldEnd()
  {
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_currParagraph.Document, FieldMarkType.FieldEnd);
    this.m_currParagraph.Items.Add((IEntity) wfieldMark);
    this.CurrentField.FieldEnd = wfieldMark;
    WField wfield = this.FieldStack.Pop();
    wfield.UpdateFieldCode(wfield.GetFieldCodeForUnknownFieldType());
  }

  private void ParseFieldCode(FieldType fieldType, string fieldCode)
  {
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_currParagraph.Document);
    wtextRange.Text = fieldCode;
    WField wfield;
    switch (fieldType)
    {
      case FieldType.FieldIf:
        wfield = (WField) new WIfField((IWordDocument) this.m_currParagraph.Document);
        break;
      case FieldType.FieldMergeField:
        wfield = (WField) new WMergeField((IWordDocument) this.m_currParagraph.Document);
        break;
      default:
        wfield = new WField((IWordDocument) this.m_currParagraph.Document);
        break;
    }
    this.m_currParagraph.Items.Add((IEntity) wfield);
    this.m_currParagraph.Items.Add((IEntity) wtextRange);
    this.FieldStack.Push(wfield);
    wfield.FieldType = fieldType;
    this.ParseFieldSeparator();
    this.ApplyTextFormatting(wfield.CharacterFormat);
    wtextRange.ApplyCharacterFormat(wfield.CharacterFormat);
    this.IsStyleFieldCode = true;
  }

  private void ParseField(XmlNode node)
  {
    string str = this.RemoveNewLineCharacter(node.InnerText);
    if (this.IsPreviousItemFieldStart)
    {
      if (!(str.Trim() != string.Empty))
        return;
      this.InsertFieldBegin(str);
      this.IsPreviousItemFieldStart = false;
    }
    else
    {
      if (this.CurrentField == null)
        return;
      if (this.IsStyleFieldCode)
      {
        this.ApplyTextFormatting(this.CurrentPara.AppendText(str).CharacterFormat);
        if (node.NextSibling != null)
          return;
        this.ParseFieldEnd();
        this.IsStyleFieldCode = false;
      }
      else
        this.ApplyTextFormatting(this.CurrentPara.AppendText(str).CharacterFormat);
    }
  }

  private string RemoveNewLineCharacter(string text)
  {
    if (!this.CurrentFormat.IsPreserveWhiteSpace)
    {
      text = text.Replace(ControlChar.LineFeedChar, ' ').Replace('\r', ' ');
      if (text != " " && (this.m_styleStack.Count <= 0 || !this.m_styleStack.Peek().IsPreserveWhiteSpace) && !this.CurrentFormat.IsNonBreakingSpace)
        text = HTMLConverterImpl.m_removeSpaces.Replace(text, " ");
    }
    text = text.Replace("nbsp;", ' '.ToString());
    if ((this.m_styleStack.Count <= 0 || !this.m_styleStack.Peek().IsPreserveWhiteSpace) && !this.CurrentFormat.IsNonBreakingSpace)
      text = this.RemoveWhiteSpacesAtParagraphBegin(text, this.CurrentPara);
    return text;
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal class TableGrid
  {
    private Stack<Dictionary<int, List<KeyValuePair<int, float>>>> m_tblGridStack = new Stack<Dictionary<int, List<KeyValuePair<int, float>>>>();

    internal Stack<Dictionary<int, List<KeyValuePair<int, float>>>> TableGridStack
    {
      get => this.m_tblGridStack;
      set => this.m_tblGridStack = value;
    }
  }

  internal class SpanHelper
  {
    private int m_curCol;
    internal List<KeyValuePair<int, float>> m_tblGrid = new List<KeyValuePair<int, float>>();
    internal Dictionary<WTableCell, int> m_rowspans = new Dictionary<WTableCell, int>();
    internal float m_tableWidth;
    private Dictionary<int, List<KeyValuePair<int, float>>> m_tblGridCollection = new Dictionary<int, List<KeyValuePair<int, float>>>();

    internal Dictionary<int, List<KeyValuePair<int, float>>> TableGridCollection
    {
      get => this.m_tblGridCollection;
      set => this.m_tblGridCollection = value;
    }

    internal void ResetCurrColumn() => this.m_curCol = 0;

    internal void AddCellToGrid(WTableCell cell, int colSpan)
    {
      this.m_tblGrid.Add(new KeyValuePair<int, float>(colSpan, cell.Width));
    }

    internal void NextColumn() => ++this.m_curCol;

    private List<object> GetTableGrid(
      Dictionary<int, List<KeyValuePair<int, float>>> tableGridCollection,
      WTable table,
      float clientWidth)
    {
      int num1 = 0;
      for (int key = 0; key < tableGridCollection.Count; ++key)
      {
        List<KeyValuePair<int, float>> tableGrid = tableGridCollection[key];
        int num2 = 0;
        for (int index = 0; index < tableGrid.Count; ++index)
          num2 += tableGrid[index].Key;
        if (num1 < num2)
          num1 = num2;
      }
      float[] numArray = new float[num1 + 1];
      for (int key = 0; key < tableGridCollection.Count; ++key)
      {
        List<KeyValuePair<int, float>> tableGrid = tableGridCollection[key];
        float num3 = 0.0f;
        int index1 = 0;
        for (int index2 = 0; index2 < tableGrid.Count; ++index2)
        {
          num3 = (float) Math.Round((double) num3 + (double) tableGrid[index2].Value, 2);
          index1 += tableGrid[index2].Key;
          if ((double) numArray[index1] < (double) num3)
            numArray[index1] = num3;
        }
      }
      int index3 = numArray.Length - 1;
      float num4 = numArray[index3];
      for (int index4 = numArray.Length - 1; index4 >= 1; --index4)
      {
        if ((double) numArray[index4 - 1] >= (double) num4)
          index3 = index4 - 1;
        num4 = numArray[index4 - 1];
      }
      float num5 = (double) this.m_tableWidth != 0.0 ? this.m_tableWidth : clientWidth;
      float num6 = numArray[index3];
      if ((double) num6 == 0.0 && index3 > 0)
        --index3;
      if (index3 < numArray.Length - 1)
      {
        float num7 = (float) Math.Round(((double) num5 - (double) num6) / (double) (numArray.Length - 1 - index3), 2);
        if ((double) num5 <= (double) num6)
          num7 = 3f;
        for (int index5 = index3 + 1; index5 < numArray.Length; ++index5)
        {
          num6 = (float) Math.Round((double) num6 + (double) num7, 2);
          numArray[index5] = num6;
        }
      }
      if ((double) numArray[numArray.Length - 1] != (double) num5 && (double) this.m_tableWidth != 0.0)
      {
        float num8 = num5 / numArray[numArray.Length - 1];
        for (int index6 = 1; index6 < numArray.Length; ++index6)
          numArray[index6] = (float) Math.Round((double) numArray[index6] * (double) num8, 2);
        if ((double) numArray[numArray.Length - 1] > (double) num5)
          numArray[numArray.Length - 1] = num5;
      }
      List<object> tableGrid1 = new List<object>();
      for (int index7 = 1; index7 < numArray.Length; ++index7)
        tableGrid1.Add((object) (float) Math.Round((double) numArray[index7] - (double) numArray[index7 - 1], 2));
      return tableGrid1;
    }

    internal void UpdateTable(
      WTable table,
      Stack<Dictionary<int, List<KeyValuePair<int, float>>>> tableGridStack,
      float clientWidth)
    {
      this.m_tblGridCollection = tableGridStack.Pop();
      this.UpdateRowSpan(table);
      List<object> tableGrid = this.GetTableGrid(this.m_tblGridCollection, table, clientWidth);
      int num = 0;
      for (int index1 = 0; index1 < table.Rows.Count; ++index1)
      {
        WTableRow row = table.Rows[index1];
        List<KeyValuePair<int, float>> tblGrid = this.m_tblGridCollection[index1];
        int columnIndex = 0;
        for (int index2 = 0; index2 < row.Cells.Count; ++index2)
        {
          WTableCell cell = row.Cells[index2];
          int columnSpan1 = 1;
          if (index2 <= tblGrid.Count - 1)
            columnSpan1 = tblGrid[index2].Key;
          cell.Width = this.GetWidth(tableGrid, columnIndex, columnSpan1);
          columnIndex += columnSpan1;
          if (index2 + 1 == row.Cells.Count && index1 > 0 && columnIndex < num)
          {
            row.AddCell(false);
          }
          else
          {
            if (num < columnIndex)
              num = columnIndex;
            if (index2 + 1 == row.Cells.Count && columnIndex < tableGrid.Count)
            {
              int columnSpan2 = tableGrid.Count - num;
              row.RowFormat.AfterWidth = this.GetWidth(tableGrid, columnIndex, columnSpan2);
            }
          }
        }
      }
    }

    private float GetWidth(List<object> tblGrid, int columnIndex, int columnSpan)
    {
      float width = 0.0f;
      for (int index = columnIndex; index < columnIndex + columnSpan; ++index)
        width += (float) tblGrid[index];
      return width;
    }

    private void UpdateRowSpan(WTable table)
    {
      int index1 = 0;
      int num1 = 1;
      for (int index2 = 1; index2 <= num1; ++index2)
      {
        int key1 = 0;
        for (int index3 = 0; index3 < table.Rows.Count; ++index3)
        {
          WTableRow row = table.Rows[index3];
          if (index1 < row.Cells.Count)
          {
            if (table.Rows[index3].Cells.Count > num1)
              num1 = table.Rows[index3].Cells.Count;
            WTableCell cell1 = row.Cells[index1];
            this.m_tblGrid = this.m_tblGridCollection[key1];
            if (cell1.CellFormat.VerticalMerge == CellMerge.Start)
            {
              int num2 = 1;
              if (this.m_rowspans.ContainsKey(cell1))
              {
                foreach (WTableCell key2 in this.m_rowspans.Keys)
                {
                  if (key2 == cell1)
                  {
                    int num3;
                    this.m_rowspans.TryGetValue(key2, out num3);
                    num2 = num3;
                    break;
                  }
                }
              }
              int cellIndex = cell1.GetCellIndex();
              int num4 = cell1.OwnerRow.GetRowIndex() + 1;
              for (int index4 = 1; index4 < num2 && table.Rows.Count > num4; ++index4)
              {
                int num5 = 0;
                for (int index5 = 0; index5 < index1; ++index5)
                {
                  int key3 = this.m_tblGrid[index5].Key;
                  num5 += key3;
                }
                int index6 = 0;
                int num6 = 0;
                int num7 = 0;
                bool flag = false;
                for (int index7 = 0; index7 < this.m_tblGridCollection[num4].Count; ++index7)
                {
                  KeyValuePair<int, float> keyValuePair = this.m_tblGridCollection[num4][index7];
                  if (num5 < num6 + keyValuePair.Key)
                    flag = true;
                  else if (!flag)
                  {
                    num6 += keyValuePair.Key;
                    index6 = index7 + 1;
                  }
                  num7 += keyValuePair.Key;
                }
                if (num7 < num5)
                {
                  int num8 = num5 - num7;
                  for (int index8 = 0; index8 < num8; ++index8)
                  {
                    table.Rows[num4].AddCell(false);
                    table.Rows[num4].Cells[table.Rows[num4].Cells.Count - 1].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                    int count = table.Rows[num4].Cells.Count;
                    this.m_tblGridCollection[num4].Add(new KeyValuePair<int, float>(1, 0.0f));
                  }
                  int num9;
                  int num10 = num9 = num5;
                  index6 += num8;
                }
                WTableCell cell2 = (WTableCell) cell1.Clone();
                cell2.Items.Clear();
                KeyValuePair<int, float> keyValuePair1 = this.m_tblGrid[cellIndex];
                if (index6 < table.Rows[num4].Cells.Count)
                {
                  table.Rows[num4].Cells.Insert(index6, cell2);
                  this.m_tblGridCollection[num4].Insert(index6, new KeyValuePair<int, float>(keyValuePair1.Key, keyValuePair1.Value));
                }
                else
                {
                  table.Rows[num4].Cells.Add(cell2);
                  this.m_tblGridCollection[num4].Add(new KeyValuePair<int, float>(keyValuePair1.Key, keyValuePair1.Value));
                }
                cell2.CellFormat.VerticalMerge = CellMerge.Continue;
                ++num4;
              }
            }
          }
          ++key1;
        }
        ++index1;
      }
    }
  }

  internal class TextFormat
  {
    internal const short FontSizeKey = 0;
    internal const short FontFamilyKey = 1;
    internal const short BoldKey = 2;
    internal const short UnderlineKey = 3;
    internal const short ItalicKey = 4;
    internal const short StrikeKey = 5;
    internal const short FontColorKey = 6;
    internal const short BackColorKey = 7;
    internal const short LineHeightKey = 8;
    internal const short LineHeightNormalKey = 9;
    internal const short TextAlignKey = 10;
    internal const short TopMarginKey = 11;
    internal const short LeftMarginKey = 12;
    internal const short BottomMarginKey = 13;
    internal const short RightMarginKey = 14;
    internal const short TextIndentKey = 15;
    internal const short SubSuperScriptKey = 16 /*0x10*/;
    internal const short PageBreakBeforeKey = 17;
    internal const short PageBreakAfterKey = 18;
    internal const short CharacterSpacingKey = 19;
    internal const short AllCapsKey = 20;
    internal const short WhiteSpaceKey = 21;
    internal const short WordWrapKey = 22;
    internal const short HiddenKey = 23;
    internal const short SmallCapsKey = 24;
    internal const short Keeplinestogetherkey = 25;
    internal const short keepwithnextKey = 26;
    internal const short AfterSpaceAutoKey = 27;
    internal const short BeforeSpaceAutoKey = 28;
    internal const short LocaleIdASCIIKey = 29;
    internal Dictionary<int, object> m_propertiesHash;
    private LineSpacingRule m_lineSpacingRule;
    private float m_tabPos;
    private TabLeader m_tabLeader;
    private TabJustification m_tabJustification;
    private float m_tabWidth;
    private bool m_isInlineBlock;
    private bool m_isSpaceRun;
    private bool m_isListTab;
    private float m_listNumberWidth;
    private float m_paddingLeft;
    public bool NumBulleted;
    public bool DefBulleted;
    public BuiltinStyle Style;
    public HTMLConverterImpl.TableBorders Borders = new HTMLConverterImpl.TableBorders();

    public bool WordWrap
    {
      get => !this.HasKey(22) || (bool) this.m_propertiesHash[22];
      set => this.SetPropertyValue(22, value);
    }

    public bool IsPreserveWhiteSpace
    {
      get => this.HasKey(21) && (bool) this.m_propertiesHash[21];
      set => this.SetPropertyValue(21, value);
    }

    internal short LocaleIdASCII
    {
      get => (short) this.m_propertiesHash[29];
      set => this.SetPropertyValue(29, (object) value);
    }

    internal bool Hidden
    {
      get => this.HasKey(23) && (bool) this.m_propertiesHash[23];
      set => this.SetPropertyValue(23, value);
    }

    internal bool KeepLinesTogether
    {
      get => this.HasKey(25) && (bool) this.m_propertiesHash[25];
      set => this.SetPropertyValue(25, value);
    }

    internal bool KeepWithNext
    {
      get => this.HasKey(26) && (bool) this.m_propertiesHash[26];
      set => this.SetPropertyValue(26, value);
    }

    internal bool AfterSpaceAuto
    {
      get => this.HasKey(27) && (bool) this.m_propertiesHash[27];
      set => this.SetPropertyValue(27, value);
    }

    internal bool BeforeSpaceAuto
    {
      get => this.HasKey(28) && (bool) this.m_propertiesHash[28];
      set => this.SetPropertyValue(28, value);
    }

    public bool AllCaps
    {
      get => this.HasKey(20) && (bool) this.m_propertiesHash[20];
      set => this.SetPropertyValue(20, value);
    }

    internal bool SmallCaps
    {
      get => this.HasKey(24) && (bool) this.m_propertiesHash[24];
      set => this.SetPropertyValue(24, value);
    }

    public float CharacterSpacing
    {
      get => this.HasKey(19) ? (float) this.m_propertiesHash[19] : 0.0f;
      set => this.SetPropertyValue(19, (object) value);
    }

    public bool PageBreakBefore
    {
      get => this.HasKey(17) && (bool) this.m_propertiesHash[17];
      set => this.SetPropertyValue(17, value);
    }

    public bool PageBreakAfter
    {
      get => this.HasKey(18) && (bool) this.m_propertiesHash[18];
      set => this.SetPropertyValue(18, value);
    }

    public LineSpacingRule LineSpacingRule
    {
      get => this.m_lineSpacingRule;
      set => this.m_lineSpacingRule = value;
    }

    internal float TabPosition
    {
      get => this.m_tabPos;
      set => this.m_tabPos = value;
    }

    internal TabLeader TabLeader
    {
      get => this.m_tabLeader;
      set => this.m_tabLeader = value;
    }

    internal TabJustification TabJustification
    {
      get => this.m_tabJustification;
      set => this.m_tabJustification = value;
    }

    internal float TabWidth
    {
      get => this.m_tabWidth;
      set => this.m_tabWidth = value;
    }

    internal bool IsInlineBlock
    {
      get => this.m_isInlineBlock;
      set => this.m_isInlineBlock = value;
    }

    internal bool IsNonBreakingSpace
    {
      get => this.m_isSpaceRun;
      set => this.m_isSpaceRun = value;
    }

    internal bool IsListTab
    {
      get => this.m_isListTab;
      set => this.m_isListTab = value;
    }

    internal float ListNumberWidth
    {
      get => this.m_listNumberWidth;
      set => this.m_listNumberWidth = value;
    }

    internal float PaddingLeft
    {
      get => this.m_paddingLeft;
      set => this.m_paddingLeft = value;
    }

    public bool Bold
    {
      get => this.HasKey(2) && (bool) this.m_propertiesHash[2];
      set => this.SetPropertyValue(2, value);
    }

    public bool Italic
    {
      get => this.HasKey(4) && (bool) this.m_propertiesHash[4];
      set => this.SetPropertyValue(4, value);
    }

    public bool Underline
    {
      get => this.HasKey(3) && (bool) this.m_propertiesHash[3];
      set => this.SetPropertyValue(3, value);
    }

    public bool Strike
    {
      get => this.HasKey(5) && (bool) this.m_propertiesHash[5];
      set => this.SetPropertyValue(5, value);
    }

    public Color FontColor
    {
      get => this.HasKey(6) ? (Color) this.m_propertiesHash[6] : Color.Empty;
      set => this.SetPropertyValue(6, (object) value);
    }

    public Color BackColor
    {
      get => this.HasKey(7) ? (Color) this.m_propertiesHash[7] : Color.Empty;
      set => this.SetPropertyValue(7, (object) value);
    }

    public string FontFamily
    {
      get => this.HasKey(1) ? (string) this.m_propertiesHash[1] : string.Empty;
      set => this.SetPropertyValue(1, (object) value);
    }

    public float FontSize
    {
      get => this.HasKey(0) ? (float) this.m_propertiesHash[0] : 12f;
      set => this.SetPropertyValue(0, (object) value);
    }

    public float LineHeight
    {
      get => this.HasKey(8) ? (float) this.m_propertiesHash[8] : -1f;
      set => this.SetPropertyValue(8, (object) value);
    }

    public bool IsLineHeightNormal
    {
      get => this.HasKey(9) && (bool) this.m_propertiesHash[9];
      set => this.SetPropertyValue(9, value);
    }

    public HorizontalAlignment TextAlign
    {
      get
      {
        return this.HasKey(10) ? (HorizontalAlignment) this.m_propertiesHash[10] : HorizontalAlignment.Left;
      }
      set => this.SetPropertyValue(10, (object) value);
    }

    public float LeftMargin
    {
      get => this.HasKey(12) ? (float) this.m_propertiesHash[12] : 0.0f;
      set => this.SetPropertyValue(12, (object) value);
    }

    public float TextIndent
    {
      get => this.HasKey(15) ? (float) this.m_propertiesHash[15] : 0.0f;
      set => this.SetPropertyValue(15, (object) value);
    }

    public float RightMargin
    {
      get => this.HasKey(14) ? (float) this.m_propertiesHash[14] : 0.0f;
      set => this.SetPropertyValue(14, (object) value);
    }

    public float TopMargin
    {
      get => this.HasKey(11) ? (float) this.m_propertiesHash[11] : 0.0f;
      set => this.SetPropertyValue(11, (object) value);
    }

    public float BottomMargin
    {
      get => this.HasKey(13) ? (float) this.m_propertiesHash[13] : -1f;
      set => this.SetPropertyValue(13, (object) value);
    }

    public SubSuperScript SubSuperScript
    {
      get
      {
        return this.HasKey(16 /*0x10*/) ? (SubSuperScript) this.m_propertiesHash[16 /*0x10*/] : SubSuperScript.None;
      }
      set => this.SetPropertyValue(16 /*0x10*/, (object) value);
    }

    internal TextFormat() => this.m_propertiesHash = new Dictionary<int, object>();

    public HTMLConverterImpl.TextFormat Clone()
    {
      return new HTMLConverterImpl.TextFormat()
      {
        m_propertiesHash = new Dictionary<int, object>((IDictionary<int, object>) this.m_propertiesHash),
        NumBulleted = this.NumBulleted,
        DefBulleted = this.DefBulleted,
        Borders = this.Borders,
        LineSpacingRule = this.LineSpacingRule
      };
    }

    private void SetPropertyValue(int Key, bool value)
    {
      if (!this.m_propertiesHash.ContainsKey(Key))
        this.m_propertiesHash.Add(Key, (object) value);
      else
        this.m_propertiesHash[Key] = (object) value;
    }

    internal bool HasKey(int Key) => this.m_propertiesHash.ContainsKey(Key);

    internal bool HasValue(int key) => this.m_propertiesHash.ContainsKey(key);

    private void SetPropertyValue(int Key, object value)
    {
      if (!this.m_propertiesHash.ContainsKey(Key))
        this.m_propertiesHash.Add(Key, value);
      else
        this.m_propertiesHash[Key] = value;
    }
  }

  internal class TableBorders
  {
    public Color AllColor = Color.Empty;
    public float AllWidth = -1f;
    public BorderStyle AllStyle;
    public Color TopColor = Color.Empty;
    public Color BottomColor = Color.Empty;
    public Color LeftColor = Color.Empty;
    public Color RightColor = Color.Empty;
    public BorderStyle TopStyle;
    public BorderStyle BottomStyle;
    public BorderStyle LeftStyle;
    public BorderStyle RightStyle;
    public float TopWidth = -1f;
    public float BottomWidth = -1f;
    public float LeftWidth = -1f;
    public float RightWidth = -1f;
    public float BottomSpace;
    public float TopSpace;
    public float LeftSpace;
    public float RightSpace;
  }

  internal enum ThreeState
  {
    False,
    True,
    Unknown,
  }
}
