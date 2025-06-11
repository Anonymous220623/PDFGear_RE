// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HTMLExport
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class HTMLExport
{
  private const string DEF_HYPHEN = "-";
  private const char DEF_NONBREAK_HYPHEN = '\u001E';
  private const char DEF_SOFT_HYPHEN = '\u001F';
  private XmlTextWriter m_writer;
  private string m_fileNameWithoutExt;
  private bool m_bIsWriteListTab;
  private int m_imgCounter;
  private int m_currListLevel = -1;
  private Stack<int> listStack = new Stack<int>();
  private Stack<WField> m_fieldStack;
  private Stack<WField> m_nestedHyperlinkFieldStack;
  private bool isKeepValue;
  private Dictionary<int, WFootnote> m_footnotes;
  private Dictionary<int, WFootnote> m_endnotes;
  private int m_footnoteSecIndex;
  private Dictionary<string, Dictionary<int, int>> m_lists;
  private string m_ftntAttrStr;
  private string m_ftntString;
  private bool m_bUseAbsolutePath;
  private bool m_bSkipFieldItem;
  private bool m_bSkiPageRefFieldItem;
  private WParagraph m_currPara;
  private bool m_bIsFirstSection = true;
  private Dictionary<string, string> m_stylesColl;
  private WordDocument m_document;
  private string m_prefixedValue;
  private bool m_bIsPrefixedList;
  private bool m_bIsParaWithinDivision;
  private bool m_bIsPreserveListAsPara;
  private Dictionary<WPicture, int> m_behindWrapStyleFloatingItems;
  private bool m_cacheFilesInternally;
  private bool m_hasNavigationId;
  private bool m_hasOEBHeaderFooter;
  private int m_nameID;
  private string[] m_headingStyles;
  private MemoryStream m_styleSheet;
  private Dictionary<string, string> m_bookmarks;
  private string m_ftntRefAttrStr;
  private WCharacterFormat m_currListCharFormat;
  private Dictionary<string, Stream> m_imageFiles;
  private bool isepubConvertion;
  private bool m_isParaHasTab;
  private float m_currentLineWidth;
  private float m_defaultTabWidth;
  private string m_destFolder;
  private string m_imagesFolder;
  private string m_imageRelativePath;
  private bool m_bImagesFolderCreated;
  private WParagraphStyle m_normalStyle;
  private bool m_bIsFieldCode;
  private DrawingContext m_dc;
  private List<TabsLayoutInfo.LayoutTab> m_layoutTabList;
  private bool m_isNeedToWriteTabList;

  public bool UseAbsolutePath
  {
    get => this.m_bUseAbsolutePath;
    set => this.m_bUseAbsolutePath = Convert.ToBoolean(value);
  }

  private Dictionary<string, Dictionary<int, int>> Lists
  {
    get
    {
      if (this.m_lists == null)
        this.m_lists = new Dictionary<string, Dictionary<int, int>>();
      return this.m_lists;
    }
  }

  private Dictionary<WPicture, int> BehindWrapStyleFloatingItems
  {
    get
    {
      if (this.m_behindWrapStyleFloatingItems == null)
        this.m_behindWrapStyleFloatingItems = new Dictionary<WPicture, int>();
      return this.m_behindWrapStyleFloatingItems;
    }
  }

  private WField CurrentField
  {
    get
    {
      return this.m_fieldStack == null || this.m_fieldStack.Count <= 0 ? (WField) null : this.m_fieldStack.Peek();
    }
  }

  private WField PreviousField
  {
    get => this.FieldStack.Count > 1 ? this.FieldStack.ToArray()[1] : (WField) null;
  }

  private Dictionary<int, WFootnote> Footnotes
  {
    get
    {
      if (this.m_footnotes == null)
        this.m_footnotes = new Dictionary<int, WFootnote>();
      return this.m_footnotes;
    }
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

  private Dictionary<int, WFootnote> Endnotes
  {
    get
    {
      if (this.m_endnotes == null)
        this.m_endnotes = new Dictionary<int, WFootnote>();
      return this.m_endnotes;
    }
  }

  internal bool CacheFilesInternally
  {
    get => this.m_cacheFilesInternally;
    set => this.m_cacheFilesInternally = value;
  }

  internal bool HasNavigationId
  {
    get => this.m_hasNavigationId;
    set => this.m_hasNavigationId = value;
  }

  internal bool HasOEBHeaderFooter
  {
    get => this.m_hasOEBHeaderFooter;
    set => this.m_hasOEBHeaderFooter = value;
  }

  internal Stream EmbeddedStyleSheet => (Stream) this.m_styleSheet;

  internal Dictionary<string, Stream> EmbeddedImages => this.m_imageFiles;

  internal Dictionary<string, string> Bookmarks => this.m_bookmarks;

  internal bool IsParaHasTab
  {
    get => this.m_isParaHasTab;
    set => this.m_isParaHasTab = value;
  }

  internal float CurrentLineWidth
  {
    get => this.m_currentLineWidth;
    set => this.m_currentLineWidth = value;
  }

  internal float DefaultTabWidth
  {
    get => this.m_defaultTabWidth;
    set => this.m_defaultTabWidth = value;
  }

  internal DrawingContext DrawingContext
  {
    get
    {
      if (this.m_dc == null)
        this.m_dc = new DrawingContext();
      return this.m_dc;
    }
  }

  internal List<TabsLayoutInfo.LayoutTab> LayoutTabList
  {
    get => this.m_layoutTabList;
    set => this.m_layoutTabList = value;
  }

  internal bool IsNeedToWriteTabList
  {
    get => this.m_isNeedToWriteTabList;
    set => this.m_isNeedToWriteTabList = value;
  }

  public void SaveAsXhtml(WordDocument doc, string fileName)
  {
    this.SaveAsXhtml(doc, fileName, Encoding.UTF8);
  }

  public void SaveAsXhtml(WordDocument doc, string fileName, Encoding encoding)
  {
    this.m_document = doc;
    this.SortBehindWrapStyleItemByZindex();
    UnitsConvertor.Instance.InitDefProporsions();
    this.m_fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
    this.m_destFolder = Path.GetDirectoryName(fileName) + "\\";
    if (this.m_destFolder == "\\")
      this.m_destFolder = Environment.CurrentDirectory + "\\";
    this.m_imagesFolder = this.m_destFolder;
    this.m_stylesColl = new Dictionary<string, string>();
    string cssFileName = this.m_fileNameWithoutExt + "_styles.css";
    if (!string.IsNullOrEmpty(doc.SaveOptions.HtmlExportCssStyleSheetFileName))
      cssFileName = doc.SaveOptions.HtmlExportCssStyleSheetFileName;
    if (!string.IsNullOrEmpty(doc.SaveOptions.HtmlExportImagesFolder))
    {
      this.m_bUseAbsolutePath = true;
      if (!doc.SaveOptions.HtmlExportImagesFolder.EndsWith("\\"))
        doc.SaveOptions.HtmlExportImagesFolder += "\\";
      string directoryName = Path.GetDirectoryName(Path.GetFullPath(fileName));
      this.m_imagesFolder = this.m_imageRelativePath = Path.GetFullPath(doc.SaveOptions.HtmlExportImagesFolder);
      if (directoryName != null && this.m_imagesFolder.StartsWithExt(directoryName))
      {
        this.m_imageRelativePath = string.Join("", this.m_imagesFolder.Split(new string[1]
        {
          directoryName
        }, StringSplitOptions.RemoveEmptyEntries)).TrimStart('\\');
        if (!string.IsNullOrEmpty(this.m_imageRelativePath) && !this.m_imageRelativePath.EndsWith("\\"))
          this.m_imageRelativePath += "\\";
      }
    }
    else
      this.m_bUseAbsolutePath = false;
    if (doc.SaveOptions.HtmlExportCssStyleSheetType == CssStyleSheetType.External)
    {
      using (StreamWriter text = File.CreateText(this.m_destFolder + cssFileName))
        text.Write(this.GetStyleSheet(doc));
    }
    this.m_normalStyle = doc.Styles.FindByName("Normal") as WParagraphStyle;
    if (this.m_normalStyle == null)
      this.m_normalStyle = doc.Styles.FindByName("normal") as WParagraphStyle;
    using (this.m_writer = new XmlTextWriter(fileName, encoding))
      this.WriteXhtml(doc, cssFileName);
  }

  internal void SaveAsXhtml(WordDocument doc, Stream stream, bool ePub)
  {
    if (ePub)
    {
      this.m_bIsPreserveListAsPara = true;
      this.isepubConvertion = true;
    }
    this.m_document = doc;
    this.m_nameID = 1;
    this.m_imageFiles = new Dictionary<string, Stream>();
    int epubHeadingLevels = (int) this.m_document.SaveOptions.EPubHeadingLevels;
    this.m_headingStyles = new string[epubHeadingLevels];
    for (int index = 0; index < epubHeadingLevels; ++index)
      this.m_headingStyles[index] = "heading" + (object) (index + 1);
    this.m_bookmarks = new Dictionary<string, string>();
    this.SaveAsXhtml(doc, stream);
  }

  public void SaveAsXhtml(WordDocument doc, Stream stream)
  {
    this.SaveAsXhtml(doc, stream, Encoding.UTF8);
  }

  public void SaveAsXhtml(WordDocument doc, Stream stream, Encoding encoding)
  {
    this.m_document = doc;
    this.SortBehindWrapStyleItemByZindex();
    UnitsConvertor.Instance.InitDefProporsions();
    this.m_bUseAbsolutePath = !string.IsNullOrEmpty(doc.SaveOptions.HtmlExportImagesFolder);
    if (!doc.SaveOptions.HtmlExportImagesFolder.EndsWith("\\"))
      doc.SaveOptions.HtmlExportImagesFolder += "\\";
    this.m_destFolder = !(doc.SaveOptions.HtmlExportImagesFolder != "\\") ? doc.SaveOptions.HtmlExportImagesFolder : Path.GetFullPath(doc.SaveOptions.HtmlExportImagesFolder);
    this.m_imagesFolder = this.m_imageRelativePath = this.m_destFolder;
    this.m_stylesColl = new Dictionary<string, string>();
    this.m_writer = new XmlTextWriter(stream, encoding);
    if (!this.m_cacheFilesInternally)
      this.WriteXhtml(doc, string.Empty);
    else
      this.WriteXhtml(doc, doc.SaveOptions.HtmlExportCssStyleSheetFileName);
    this.m_writer.Flush();
  }

  private void WriteXhtml(WordDocument doc, string cssFileName)
  {
    this.WriteHead(doc, cssFileName);
    this.WriteBody(doc);
    this.m_writer.WriteEndElement();
    this.Close();
  }

  private void Close()
  {
    if (this.m_nestedHyperlinkFieldStack != null)
    {
      this.m_nestedHyperlinkFieldStack.Clear();
      this.m_nestedHyperlinkFieldStack = (Stack<WField>) null;
    }
    if (this.m_fieldStack != null)
    {
      this.m_fieldStack.Clear();
      this.m_fieldStack = (Stack<WField>) null;
    }
    if (this.listStack != null)
    {
      this.listStack.Clear();
      this.listStack = (Stack<int>) null;
    }
    if (this.m_footnotes != null)
    {
      this.m_footnotes.Clear();
      this.m_footnotes = (Dictionary<int, WFootnote>) null;
    }
    if (this.m_endnotes != null)
    {
      this.m_endnotes.Clear();
      this.m_endnotes = (Dictionary<int, WFootnote>) null;
    }
    if (this.m_lists != null)
    {
      this.m_lists.Clear();
      this.m_lists = (Dictionary<string, Dictionary<int, int>>) null;
    }
    if (this.m_stylesColl != null)
    {
      this.m_stylesColl.Clear();
      this.m_stylesColl = (Dictionary<string, string>) null;
    }
    if (this.m_behindWrapStyleFloatingItems != null)
    {
      this.m_behindWrapStyleFloatingItems.Clear();
      this.m_behindWrapStyleFloatingItems = (Dictionary<WPicture, int>) null;
    }
    if (this.m_bookmarks == null)
      return;
    this.m_bookmarks.Clear();
    this.m_bookmarks = (Dictionary<string, string>) null;
  }

  private void WriteHead(WordDocument doc, string cssFileName)
  {
    if (!doc.SaveOptions.HtmlExportOmitXmlDeclaration)
      this.m_writer.WriteStartDocument();
    this.m_writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", (string) null);
    this.m_writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
    this.m_writer.WriteStartElement("head");
    this.m_writer.WriteRaw("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />");
    this.m_writer.WriteRaw($"<title>{doc.BuiltinDocumentProperties.Title}</title>");
    if (doc.SaveOptions.HtmlExportCssStyleSheetType == CssStyleSheetType.Internal || string.IsNullOrEmpty(cssFileName))
    {
      this.m_writer.WriteStartElement("style");
      this.m_writer.WriteAttributeString("type", "text/css");
      this.m_writer.WriteRaw(this.GetStyleSheet(doc));
      this.m_writer.WriteEndElement();
    }
    else
    {
      if (this.m_cacheFilesInternally)
      {
        this.m_styleSheet = new MemoryStream();
        StreamWriter streamWriter = new StreamWriter((Stream) this.m_styleSheet);
        streamWriter.Write(this.GetStyleSheet(doc));
        streamWriter.Flush();
      }
      this.m_writer.WriteRaw($"<link href=\"{cssFileName}\" type=\"text/css\" rel=\"stylesheet\"/>");
    }
    this.m_writer.WriteEndElement();
  }

  private void WriteBody(WordDocument doc)
  {
    this.m_writer.WriteStartElement("body");
    if (doc.Background != null && doc.Background.Picture != null)
      this.WriteBackgroundImage(doc.Background.Picture);
    if (doc.WriteWarning)
    {
      WParagraph para = new WParagraph((IWordDocument) doc);
      IWTextRange wtextRange = para.AppendText("Created with a trial version of Syncfusion Essential DocIO.");
      wtextRange.CharacterFormat.FontName = "Calibri";
      wtextRange.CharacterFormat.FontSize = 11f;
      wtextRange.CharacterFormat.TextColor = Color.Black;
      wtextRange.CharacterFormat.Bold = true;
      para.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
      this.WriteParagraph(para);
    }
    this.m_bIsFirstSection = true;
    foreach (WSection section in (CollectionImpl) doc.Sections)
    {
      this.WriteSection(section);
      ++this.m_footnoteSecIndex;
    }
    this.WriteFootnotes(FootnoteType.Footnote);
    this.m_bIsFirstSection = true;
    if (!WordDocument.EnablePartialTrustCode)
      doc.ClearLists();
    this.WriteFootnotes(FootnoteType.Endnote);
    this.m_writer.WriteEndElement();
  }

  private void WriteBackgroundImage(System.Drawing.Image pic)
  {
    byte[] numArray = (byte[]) null;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      pic.Save((Stream) memoryStream, pic.RawFormat);
      numArray = memoryStream.ToArray();
    }
    Syncfusion.DocIO.DLS.Entities.ImageFormat imageFormat1 = Syncfusion.DocIO.DLS.Entities.ImageFormat.Bmp;
    System.Drawing.Image image = pic;
    System.Drawing.Imaging.ImageFormat imageFormat2 = this.GetImageFormat(string.Format((IFormatProvider) CultureInfo.InvariantCulture, image.RawFormat.Guid.ToString()));
    string str1 = "." + imageFormat2.ToString().ToLower(CultureInfo.InvariantCulture);
    string imgPath = this.GetImagePath() + str1;
    if (this.m_imagesFolder == "\\" || this.m_document.SaveOptions.HTMLExportImageAsBase64)
    {
      string str2 = string.Empty;
      if (WordDocument.EnablePartialTrustCode)
        str2 = "data:image/" + imageFormat1.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture);
      else if (imageFormat2 != null)
        str2 = "data:image/" + imageFormat2.ToString().ToLower(CultureInfo.InvariantCulture);
      string base64String;
      if (pic.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Emf) || pic.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Wmf))
      {
        Bitmap bitmap = this.ConvertEMFToBitmap(image);
        MemoryStream memoryStream = new MemoryStream();
        bitmap.Save((Stream) memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        base64String = Convert.ToBase64String(memoryStream.ToArray());
        memoryStream.Dispose();
        bitmap.Dispose();
      }
      else
        base64String = Convert.ToBase64String(numArray);
      this.m_writer.WriteAttributeString("background", $"{str2};base64,{base64String}");
    }
    else
    {
      this.EnsureImagesFolder();
      if (WordDocument.EnablePartialTrustCode || imageFormat2 == System.Drawing.Imaging.ImageFormat.Gif)
        this.ProcessImageUsingFileStream(imgPath, numArray);
      else
        this.ProcessImage(image, imgPath, pic.Width, pic.Height, false, imageFormat2);
      this.m_writer.WriteAttributeString("background", (this.UseAbsolutePath ? this.m_imageRelativePath : string.Empty) + imgPath);
    }
  }

  private string GetStyleSheet(WordDocument doc)
  {
    StringBuilder sb = new StringBuilder();
    if (doc.SaveOptions.EPubExportFont && doc.SaveOptions.FontFiles != null)
    {
      foreach (string fontFile in doc.SaveOptions.FontFiles)
        this.AppendLine(sb, fontFile);
    }
    this.AppendLine(sb, "body{ font-family:'Times New Roman'; font-size:1em; }");
    this.AppendLine(sb, "ul, ol{ margin-top: 0; margin-bottom: 0; }");
    foreach (Style style in (IEnumerable) doc.Styles)
    {
      switch (this.EncodeName(style.Name).ToLower(CultureInfo.InvariantCulture))
      {
        case "heading1":
        case "heading-1":
        case "heading 1":
        case "heading_1":
          sb.Append("h1");
          this.AppendStyleSheet(style, sb);
          break;
        case "heading2":
        case "heading-2":
        case "heading 2":
        case "heading_2":
          sb.Append("h2");
          this.AppendStyleSheet(style, sb);
          break;
        case "heading3":
        case "heading-3":
        case "heading 3":
        case "heading_3":
          sb.Append("h3");
          this.AppendStyleSheet(style, sb);
          break;
        case "heading4":
        case "heading-4":
        case "heading 4":
        case "heading_4":
          sb.Append("h4");
          this.AppendStyleSheet(style, sb);
          break;
        case "heading5":
        case "heading-5":
        case "heading 5":
        case "heading_5":
          sb.Append("h5");
          this.AppendStyleSheet(style, sb);
          break;
        case "heading6":
        case "heading-6":
        case "heading 6":
        case "heading_6":
          sb.Append("h6");
          this.AppendStyleSheet(style, sb);
          break;
      }
      sb.Append(".");
      sb.Append(this.EncodeName(style.Name));
      this.AppendStyleSheet(style, sb);
    }
    return sb.ToString();
  }

  private void AppendStyleSheet(Style style, StringBuilder sb)
  {
    string str = string.Empty;
    sb.Append("{");
    switch (style.StyleType)
    {
      case StyleType.ParagraphStyle:
        if (style is WParagraphStyle wparagraphStyle)
        {
          str = this.GetStyle(wparagraphStyle.ParagraphFormat, false, this.m_bIsPreserveListAsPara, (WListFormat) null) + this.GetStyle(wparagraphStyle.CharacterFormat);
          break;
        }
        break;
      case StyleType.CharacterStyle:
        if (style is WCharacterStyle wcharacterStyle)
        {
          str = this.GetStyle(wcharacterStyle.CharacterFormat);
          break;
        }
        break;
    }
    sb.Append(str);
    if (!this.m_stylesColl.ContainsKey(style.Name))
      this.m_stylesColl.Add(style.Name, str);
    this.AppendLine(sb, "}");
  }

  private void AppendLine(StringBuilder sb, string textline) => sb.AppendLine(textline);

  private void WritePageBreakBeforeSection()
  {
    this.m_writer.WriteStartElement("span");
    this.m_writer.WriteAttributeString("style", "font-size:12pt;font-family:Times New Roman");
    this.m_writer.WriteStartElement("br");
    this.m_writer.WriteAttributeString("clear", "all");
    this.m_writer.WriteAttributeString("style", "page-break-before:always");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void WriteSection(WSection sec)
  {
    bool exportHeadersFooters = sec.Document.SaveOptions.HtmlExportHeadersFooters;
    if (sec.PreviousSibling != null && sec.BreakCode == SectionBreakCode.NewPage)
      this.WritePageBreakBeforeSection();
    this.m_writer.WriteStartElement("div");
    this.m_writer.WriteAttributeString("class", "Section" + (object) sec.GetIndexInOwnerCollection());
    if (sec.PreviousSibling != null && sec.BreakCode == SectionBreakCode.NewPage)
      this.m_writer.WriteAttributeString("style", "clear: both; page-break-before: always");
    if (exportHeadersFooters && this.m_bIsFirstSection)
    {
      if (this.m_hasOEBHeaderFooter)
      {
        this.m_writer.WriteStartElement("div");
        this.m_writer.WriteAttributeString("style", "display: oeb-page-head");
      }
      this.WriteTextBody(sec.PageSetup.DifferentFirstPage ? (WTextBody) sec.HeadersFooters.FirstPageHeader : (WTextBody) sec.HeadersFooters.OddHeader);
      if (this.m_hasOEBHeaderFooter)
        this.m_writer.WriteEndElement();
    }
    this.WriteTextBody(sec.Body);
    if (exportHeadersFooters && sec.NextSibling == null)
    {
      sec = sec.Document.Sections[0];
      if (this.m_hasOEBHeaderFooter)
      {
        this.m_writer.WriteStartElement("div");
        this.m_writer.WriteAttributeString("style", "display: oeb-page-foot");
      }
      this.WriteTextBody(sec.PageSetup.DifferentFirstPage ? (WTextBody) sec.HeadersFooters.FirstPageFooter : (WTextBody) sec.HeadersFooters.Footer);
      if (this.m_hasOEBHeaderFooter)
        this.m_writer.WriteEndElement();
    }
    if (this.listStack.Count > 0)
      this.WriteEndElement(this.listStack.Count);
    this.m_writer.WriteEndElement();
    this.m_bIsFirstSection = false;
  }

  private void WriteFootnotes(FootnoteType ftnType)
  {
    Dictionary<int, WFootnote> dictionary = ftnType == FootnoteType.Footnote ? this.m_footnotes : this.m_endnotes;
    if (dictionary == null || dictionary.Count <= 0)
      return;
    this.m_writer.WriteElementString("hr", "");
    foreach (int key in dictionary.Keys)
    {
      string str = (key + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      WFootnote wfootnote = dictionary[key];
      if (ftnType == FootnoteType.Footnote)
      {
        this.m_ftntAttrStr = $"_ftn{(object) this.m_footnoteSecIndex}_{str}";
        this.m_ftntRefAttrStr = $"_ftnref{(object) this.m_footnoteSecIndex}_{str}";
      }
      else
      {
        this.m_ftntAttrStr = "_edn" + str;
        this.m_ftntRefAttrStr = "_ednref" + str;
      }
      this.m_ftntString = $"[{str}] ";
      this.WriteTextBody(wfootnote.TextBody);
    }
    if (ftnType != FootnoteType.Footnote)
      return;
    this.m_footnotes.Clear();
  }

  private void WriteTextBody(WTextBody body)
  {
    for (int index = 0; index < body.Items.Count; ++index)
      this.WriteBodyItem(body.Items[index]);
  }

  private void WriteBodyItem(TextBodyItem bodyItem)
  {
    switch (bodyItem.EntityType)
    {
      case EntityType.Paragraph:
        this.m_currPara = bodyItem as WParagraph;
        if (this.m_currPara != null)
          this.m_currPara.SplitTextRange();
        this.CurrentLineWidth = 0.0f;
        this.WriteParagraph(bodyItem as WParagraph);
        break;
      case EntityType.BlockContentControl:
        if (!(bodyItem is BlockContentControl blockContentControl))
          break;
        this.WriteTextBody(blockContentControl.TextBody);
        break;
      case EntityType.Table:
        this.WriteTable(bodyItem as WTable, false);
        break;
    }
  }

  private void WriteParagraph(WParagraph para)
  {
    if (para.Items.Count > 0 && para.Items[0].EntityType == EntityType.Break)
      this.WriteBreak(para.Items[0]);
    this.WriteParagraphOrList(para);
    if (((this.IsParaHasTab = para.Text.Contains(ControlChar.Tab)) || this.IsNeedToWriteTabList) && !WordDocument.EnablePartialTrustCode)
    {
      this.DefaultTabWidth = (float) para.GetDefaultTabWidth();
      WListFormat listFormatValue = para.GetListFormatValue();
      WListLevel level = (WListLevel) null;
      int tabLevelIndex = 0;
      if (listFormatValue != null && listFormatValue.CurrentListStyle != null)
      {
        if (para.ParagraphFormat.Tabs.Count > 0)
          ++tabLevelIndex;
        level = para.GetListLevel(listFormatValue, ref tabLevelIndex);
      }
      WParagraph.ListTabs listTabs = new WParagraph.ListTabs(para);
      if (level != null)
        listTabs.SortParagraphTabsCollection(para.ParagraphFormat, level.ParagraphFormat.Tabs, tabLevelIndex);
      else
        listTabs.SortParagraphTabsCollection(para.ParagraphFormat, (TabCollection) null, 0);
      this.LayoutTabList = listTabs.LayoutTabList;
      if (!this.IsNeedToWriteTabList && level != null && level.FollowCharacter != FollowCharacterType.Nothing)
        this.WriteListFollowText(level);
      if (this.m_bIsWriteListTab)
        this.WritePrefixListTab(level);
    }
    if (para.Items.Count == 0 || para.Text == "" && para.ChildEntities.Count == 1 && (para.ChildEntities[0].EntityType == EntityType.TextRange || para.ChildEntities[0].EntityType == EntityType.BookmarkStart || para.ChildEntities[0].EntityType == EntityType.BookmarkEnd || para.ChildEntities[0].EntityType == EntityType.FieldMark))
      this.WriteEmptyPara(para.BreakCharacterFormat);
    if (this.IsParaHasTab && (double) para.ParagraphFormat.LeftIndent > 0.0 && (double) para.ParagraphFormat.FirstLineIndent == 0.0)
      this.CurrentLineWidth += para.ParagraphFormat.LeftIndent;
    this.WriteParagraphItems(para.Items);
    this.m_writer.WriteEndElement();
    if (para.NextSibling is WParagraph && (para.NextSibling as WParagraph).StyleName != para.StyleName || para.NextSibling == null || !(para.NextSibling is WParagraph))
    {
      WParagraphStyle byName = this.m_document.Styles.FindByName(para.StyleName) as WParagraphStyle;
      if (byName.ParagraphFormat.BackColor != Color.Empty && byName.ParagraphFormat.BackColor != Color.White)
        this.m_writer.WriteEndElement();
    }
    if (para.ParagraphFormat.Bidi && para.ListFormat.ListType != ListType.NoList)
      this.m_writer.WriteEndElement();
    this.m_writer.WriteRaw(ControlChar.CrLf);
    if (!this.IsParaHasTab && !this.IsNeedToWriteTabList)
      return;
    this.IsParaHasTab = false;
    this.CurrentLineWidth = 0.0f;
    this.DefaultTabWidth = 0.0f;
    this.LayoutTabList = (List<TabsLayoutInfo.LayoutTab>) null;
    this.IsNeedToWriteTabList = false;
  }

  private bool SkipItem(ParagraphItem item)
  {
    WField currentField = this.CurrentField;
    if (this.FieldStack.Count == 1 && (currentField.FieldSeparator != null ? currentField.FieldSeparator : currentField.FieldEnd) == item)
      this.m_bIsFieldCode = false;
    if (this.m_bSkipFieldItem && currentField != null)
    {
      if (item is WFieldMark)
      {
        if ((item as WFieldMark).Type == FieldMarkType.FieldSeparator)
        {
          if (currentField.FieldSeparator == item)
          {
            if (currentField.FieldType == FieldType.FieldHyperlink)
              this.m_bSkipFieldItem = false;
            else if (currentField.FieldType == FieldType.FieldTOC || currentField.FieldType == FieldType.FieldEmbed)
            {
              this.m_bSkipFieldItem = false;
              this.FieldStack.Pop();
            }
          }
        }
        else if ((currentField.IsFormField() || this.m_bSkiPageRefFieldItem) && currentField.FieldEnd == item)
        {
          this.m_bSkipFieldItem = false;
          this.FieldStack.Pop();
        }
        else if ((item as WFieldMark).Type == FieldMarkType.FieldEnd && (item as WFieldMark).ParentField != null && (item as WFieldMark).ParentField.EntityType == EntityType.TOC && this.m_bSkiPageRefFieldItem)
          this.m_bSkiPageRefFieldItem = false;
      }
      return true;
    }
    if (this.IsWritingHyperinkFieldResult())
    {
      if (item is WField)
      {
        WField field = item as WField;
        if (this.m_bSkiPageRefFieldItem && field.FieldType == FieldType.FieldPageRef)
        {
          this.PushToFieldStack(field);
          this.m_bSkipFieldItem = true;
        }
        else
        {
          this.FieldStack.Push(field);
          if (field.FieldType == FieldType.FieldHyperlink)
          {
            this.m_bSkipFieldItem = true;
            if (this.m_nestedHyperlinkFieldStack == null)
              this.m_nestedHyperlinkFieldStack = new Stack<WField>();
            this.m_nestedHyperlinkFieldStack.Push(field);
          }
          return true;
        }
      }
      else if (item is WFieldMark && (currentField.FieldType != FieldType.FieldHyperlink ? (item == (currentField.FieldSeparator != null ? currentField.FieldSeparator : currentField.FieldEnd) ? 1 : 0) : (item == currentField.FieldEnd ? 1 : 0)) != 0)
      {
        this.FieldStack.Pop();
        if (currentField.FieldType == FieldType.FieldHyperlink)
        {
          if (this.m_nestedHyperlinkFieldStack != null && this.m_nestedHyperlinkFieldStack.Count != 0 && this.m_nestedHyperlinkFieldStack.Peek() == currentField)
            this.m_nestedHyperlinkFieldStack.Pop();
          else
            this.m_writer.WriteFullEndElement();
        }
        return true;
      }
      if (item.EntityType != EntityType.TextRange && item.EntityType != EntityType.Picture || currentField.FieldType != FieldType.FieldHyperlink)
        return true;
    }
    return false;
  }

  private bool IsWritingHyperinkFieldResult()
  {
    if (this.FieldStack.Count == 0)
      return false;
    foreach (WField field in this.FieldStack)
    {
      if (field.FieldType == FieldType.FieldHyperlink)
        return true;
    }
    return false;
  }

  private void WriteParagraphItems(ParagraphItemCollection paraItems)
  {
    for (int index = 0; index < paraItems.Count; ++index)
    {
      ParagraphItem paraItem = paraItems[index];
      if (!this.SkipItem(paraItem))
      {
        switch (paraItem.EntityType)
        {
          case EntityType.InlineContentControl:
            if (paraItem is InlineContentControl inlineContentControl)
            {
              this.WriteParagraphItems(inlineContentControl.ParagraphItems);
              continue;
            }
            continue;
          case EntityType.TextRange:
            string combinedText = this.CombineTextInSubsequentTextRanges(paraItems, ref index);
            this.WriteTextRange(paraItem as WTextRange, combinedText);
            continue;
          case EntityType.Picture:
            if (paraItem is WPicture pic && pic.ImageRecord != null)
            {
              this.WriteImage(pic);
              continue;
            }
            continue;
          case EntityType.Field:
          case EntityType.MergeField:
          case EntityType.SeqField:
          case EntityType.EmbededField:
          case EntityType.ControlField:
            if (paraItem is WMergeField)
              (paraItem as WMergeField).UpdateFieldMarks();
            this.WriteField(paraItem as WField);
            continue;
          case EntityType.FieldMark:
            this.WriteFieldMark(paraItem as WFieldMark);
            continue;
          case EntityType.TextFormField:
          case EntityType.DropDownFormField:
          case EntityType.CheckBox:
            if (paraItem is WFormField field && field.FieldEnd != null)
            {
              this.WriteFormField(field);
              this.m_bSkipFieldItem = true;
              this.FieldStack.Push((WField) field);
              continue;
            }
            continue;
          case EntityType.BookmarkStart:
            this.WriteBookmark(paraItem as BookmarkStart);
            continue;
          case EntityType.Footnote:
            this.WriteFootnote(paraItem as WFootnote);
            continue;
          case EntityType.TextBox:
            this.WriteTextBox(paraItem as WTextBox);
            continue;
          case EntityType.Break:
            ParagraphItemCollection paragraphItemCollection = paraItems;
            if (paraItem.Owner is InlineContentControl && paraItem.Owner.Owner is WParagraph owner)
              paragraphItemCollection = owner.GetParagraphItems();
            if (paragraphItemCollection.IndexOf((IEntity) paraItem) > 0)
            {
              this.WriteBreak(paraItem);
              continue;
            }
            continue;
          case EntityType.Symbol:
            if (paraItem is WSymbol wsymbol)
            {
              string data = $"&#{(object) wsymbol.CharacterCode};";
              string fontName = wsymbol.FontName;
              this.m_writer.WriteStartElement("font");
              this.m_writer.WriteAttributeString("face", fontName);
              this.m_writer.WriteRaw(data);
              this.m_writer.WriteEndElement();
              continue;
            }
            continue;
          case EntityType.TOC:
            WField tocField = (paraItem as TableOfContent).TOCField;
            if (tocField.FieldSeparator != null)
            {
              this.PushToFieldStack(tocField);
              this.m_bSkipFieldItem = true;
              if (tocField.m_formattingString.Contains("\\z"))
              {
                this.m_bSkiPageRefFieldItem = true;
                continue;
              }
              continue;
            }
            continue;
          case EntityType.XmlParaItem:
            if (paraItem is XmlParagraphItem xmlParagraphItem && xmlParagraphItem.MathParaItemsCollection != null && xmlParagraphItem.MathParaItemsCollection.Count > 0)
            {
              this.WriteParagraphItems(xmlParagraphItem.MathParaItemsCollection);
              continue;
            }
            continue;
          case EntityType.OleObject:
            if ((paraItem as WOleObject).Field.FieldSeparator != null)
            {
              this.PushToFieldStack((paraItem as WOleObject).Field);
              this.m_bSkipFieldItem = true;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  private string CombineTextInSubsequentTextRanges(
    ParagraphItemCollection paraItemCollection,
    ref int index)
  {
    WTextRange wtextRange = paraItemCollection[index] as WTextRange;
    StringBuilder stringBuilder = new StringBuilder();
    if (wtextRange != null)
    {
      stringBuilder.Append(wtextRange.Text);
      while (wtextRange.NextSibling != null && wtextRange.NextSibling.EntityType == EntityType.TextRange && wtextRange.CharacterFormat.Compare(((WTextRange) wtextRange.NextSibling).CharacterFormat))
      {
        wtextRange = (WTextRange) wtextRange.NextSibling;
        stringBuilder.Append(wtextRange.Text);
        ++index;
      }
    }
    return stringBuilder.ToString();
  }

  private void WriteBreak(ParagraphItem item)
  {
    if (!(item is Break @break))
      return;
    if (@break.BreakType == BreakType.LineBreak)
    {
      if (this.m_currPara.ChildEntities.Count > 0 && item == this.m_currPara.LastItem)
      {
        this.m_writer.WriteRaw("<br/>");
        this.m_writer.WriteRaw("<br/>");
      }
      else
        this.m_writer.WriteRaw("<br/>");
    }
    else
    {
      if (@break.BreakType != BreakType.PageBreak)
        return;
      this.m_writer.WriteRaw("<br style='clear:both;page-break-before:always'/>");
    }
  }

  private void WriteTextBox(WTextBox textBox)
  {
    WTable asTable = textBox.GetAsTable(0);
    asTable.SetOwner((OwnerHolder) textBox.Owner);
    asTable.Rows[0].Cells[0].CellFormat.Paddings.Left = textBox.TextBoxFormat.InternalMargin.Left;
    asTable.Rows[0].Cells[0].CellFormat.Paddings.Right = textBox.TextBoxFormat.InternalMargin.Right;
    asTable.Rows[0].Cells[0].CellFormat.Paddings.Bottom = textBox.TextBoxFormat.InternalMargin.Bottom;
    asTable.Rows[0].Cells[0].CellFormat.Paddings.Top = textBox.TextBoxFormat.InternalMargin.Top;
    if (!textBox.Visible)
      asTable.TableFormat.Hidden = !textBox.Visible;
    this.WriteTable(asTable, true);
  }

  private void WriteFootnote(WFootnote footnote)
  {
    if (footnote.FootnoteType == FootnoteType.Footnote)
    {
      this.Footnotes.Add(this.Footnotes.Count, footnote);
      this.m_writer.WriteStartElement("a");
      this.m_writer.WriteAttributeString("href", $"#_ftn{(object) this.m_footnoteSecIndex}_{(object) this.Footnotes.Count}");
      this.m_writer.WriteAttributeString("name", $"_ftnref{(object) this.m_footnoteSecIndex}_{(object) this.Footnotes.Count}");
      this.WriteFootnoteSpan(footnote.MarkerCharacterFormat);
      if (footnote.MarkerCharacterFormat.SubSuperScript == SubSuperScript.SubScript)
        this.m_writer.WriteStartElement("sub");
      this.m_writer.WriteRaw($"[{this.m_footnotes.Count.ToString((IFormatProvider) CultureInfo.InvariantCulture)}]");
      if (footnote.MarkerCharacterFormat.SubSuperScript == SubSuperScript.SubScript)
        this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
    else
    {
      this.Endnotes.Add(this.Endnotes.Count, footnote);
      this.m_writer.WriteStartElement("a");
      this.m_writer.WriteAttributeString("href", "#_edn" + (object) this.Endnotes.Count);
      this.m_writer.WriteAttributeString("name", "_ednref" + (object) this.Endnotes.Count);
      this.WriteFootnoteSpan(footnote.MarkerCharacterFormat);
      if (footnote.MarkerCharacterFormat.SubSuperScript == SubSuperScript.SubScript)
        this.m_writer.WriteStartElement("sub");
      this.m_writer.WriteRaw($"[{this.Endnotes.Count.ToString((IFormatProvider) CultureInfo.InvariantCulture)}]");
      if (footnote.MarkerCharacterFormat.SubSuperScript == SubSuperScript.SubScript)
        this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
  }

  private void WriteFootnoteSpan(WCharacterFormat charFormat)
  {
    this.m_writer.WriteStartElement("span");
    string style = this.GetStyle(charFormat, true);
    if (charFormat.CharStyleName != null && charFormat.CharStyleName.Length > 0)
    {
      this.m_writer.WriteAttributeString("class", this.GetClassAttr(charFormat.Document.Styles.FindByName(charFormat.CharStyleName) as Style));
      if (!string.IsNullOrEmpty(charFormat.CharStyleName))
        style = this.ValidateStyle(charFormat.CharStyleName, style);
      if (!style.Contains("font-size") && (double) charFormat.FontSize > 0.0)
        style = $"{style}font-size:{charFormat.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;";
    }
    if (style.Length <= 0)
      return;
    this.m_writer.WriteAttributeString("style", style);
  }

  private void WriteFtntAttributes(WCharacterFormat charFormat)
  {
    if (this.m_ftntString == null || this.m_ftntAttrStr == null)
      return;
    this.m_writer.WriteStartElement("a");
    this.m_writer.WriteAttributeString("id", this.m_ftntAttrStr);
    this.m_writer.WriteAttributeString("href", "#" + this.m_ftntRefAttrStr);
    this.WriteFootnoteSpan(charFormat);
    if (charFormat.SubSuperScript == SubSuperScript.SubScript)
      this.m_writer.WriteStartElement("sub");
    this.m_writer.WriteString(this.m_ftntString);
    if (charFormat.SubSuperScript == SubSuperScript.SubScript)
      this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_ftntAttrStr = (string) null;
    this.m_ftntRefAttrStr = (string) null;
    this.m_ftntString = (string) null;
  }

  private void WriteFormField(WFormField field)
  {
    switch (field.FieldType)
    {
      case FieldType.FieldFormTextInput:
        if (!(field is WTextFormField wtextFormField))
          break;
        this.m_writer.WriteStartElement("a");
        this.m_writer.WriteAttributeString("name", wtextFormField.Name);
        this.m_writer.WriteEndElement();
        if (field.Document.SaveOptions.HtmlExportTextInputFormFieldAsText)
        {
          this.m_writer.WriteElementString("span", wtextFormField.Text);
        }
        else
        {
          this.m_writer.WriteStartElement("input");
          this.m_writer.WriteAttributeString("type", "text");
          this.m_writer.WriteAttributeString("name", wtextFormField.Name);
          this.m_writer.WriteAttributeString("value", wtextFormField.Text);
          this.m_writer.WriteEndElement();
        }
        if (!this.IsParaHasTab)
          break;
        this.CurrentLineWidth += this.DrawingContext.MeasureString(wtextFormField.Text, wtextFormField.CharacterFormat.Font, (StringFormat) null).Width;
        break;
      case FieldType.FieldFormCheckBox:
        if (!(field is WCheckBox checkBox))
          break;
        this.m_writer.WriteStartElement("a");
        this.m_writer.WriteAttributeString("name", checkBox.Name);
        this.m_writer.WriteEndElement();
        this.m_writer.WriteStartElement("input");
        this.m_writer.WriteAttributeString("type", "checkbox");
        this.m_writer.WriteAttributeString("name", checkBox.Name);
        string style = this.GetStyle(checkBox.CharacterFormat);
        float checkBoxSize = 0.0f;
        this.SetCheckBoxSize(style, checkBox, ref checkBoxSize);
        if (checkBox.Checked)
          this.m_writer.WriteAttributeString("checked", "checked");
        this.m_writer.WriteEndElement();
        if (!this.IsParaHasTab)
          break;
        this.CurrentLineWidth += checkBoxSize;
        break;
      case FieldType.FieldFormDropDown:
        if (!(field is WDropDownFormField wdropDownFormField))
          break;
        this.m_writer.WriteStartElement("a");
        this.m_writer.WriteAttributeString("name", wdropDownFormField.Name);
        this.m_writer.WriteEndElement();
        this.m_writer.WriteStartElement("select");
        this.m_writer.WriteAttributeString("name", wdropDownFormField.Name);
        foreach (WDropDownItem dropDownItem in (CollectionImpl) wdropDownFormField.DropDownItems)
        {
          this.m_writer.WriteStartElement("option");
          if (wdropDownFormField.DropDownValue == dropDownItem.Text)
            this.m_writer.WriteAttributeString("selected", "selected");
          this.m_writer.WriteRaw(DocxSerializator.ReplaceInvalidSurrogateCharacters(dropDownItem.Text));
          this.m_writer.WriteEndElement();
        }
        this.m_writer.WriteEndElement();
        if (!this.IsParaHasTab)
          break;
        this.CurrentLineWidth += this.DrawingContext.MeasureString(wdropDownFormField.DropDownValue, wdropDownFormField.CharacterFormat.Font, (StringFormat) null, wdropDownFormField.CharacterFormat, false).Width;
        break;
    }
  }

  private void WriteParagraphOrList(WParagraph para)
  {
    WListFormat listFormat = this.GetListFormat(para);
    if (!this.m_bIsPreserveListAsPara)
      this.CloseList(this.GetLevelNumer(listFormat), para);
    this.m_prefixedValue = string.Empty;
    this.m_bIsPrefixedList = false;
    this.m_bIsWriteListTab = false;
    this.m_bIsParaWithinDivision = false;
    string style = string.Empty;
    this.m_currListLevel = this.GetLevelNumer(listFormat);
    if (!para.IsInCell)
      this.EnsureWithinDivision(para);
    WParagraphStyle byName = this.m_document.Styles.FindByName(para.StyleName) as WParagraphStyle;
    if (byName.ParagraphFormat.BackColor != Color.Empty && byName.ParagraphFormat.BackColor != Color.White && (para.PreviousSibling is WParagraph && (para.PreviousSibling as WParagraph).StyleName != byName.Name || para.PreviousSibling == null || !(para.PreviousSibling is WParagraph)))
    {
      this.m_writer.WriteStartElement("div");
      this.m_writer.WriteAttributeString("style", $"background-color:#{byName.ParagraphFormat.BackColor.Name.Substring(2)};");
    }
    if (para.SectionEndMark || listFormat.ListType == ListType.NoList || listFormat.CurrentListLevel != null && listFormat.CurrentListLevel.PatternType == ListPatternType.None)
    {
      if (para.ParaStyle != null)
      {
        switch (this.EncodeName(para.ParaStyle.Name).ToLower(CultureInfo.InvariantCulture))
        {
          case "heading1":
          case "heading-1":
          case "heading 1":
          case "heading_1":
            this.m_writer.WriteStartElement("h1");
            break;
          case "heading2":
          case "heading-2":
          case "heading 2":
          case "heading_2":
            this.m_writer.WriteStartElement("h2");
            break;
          case "heading3":
          case "heading-3":
          case "heading 3":
          case "heading_3":
            this.m_writer.WriteStartElement("h3");
            break;
          case "heading4":
          case "heading-4":
          case "heading 4":
          case "heading_4":
            this.m_writer.WriteStartElement("h4");
            break;
          case "heading5":
          case "heading-5":
          case "heading 5":
          case "heading_5":
            this.m_writer.WriteStartElement("h5");
            break;
          case "heading6":
          case "heading-6":
          case "heading 6":
          case "heading_6":
            this.m_writer.WriteStartElement("h6");
            break;
          default:
            this.m_writer.WriteStartElement("p");
            break;
        }
      }
      else
        this.m_writer.WriteStartElement("p");
      if (!this.isKeepValue)
      {
        style = this.GetStyle(para.ParagraphFormat, false, this.m_bIsPreserveListAsPara, (WListFormat) null);
      }
      else
      {
        if (!this.m_bIsParaWithinDivision)
          this.CreateNavigationPoint(para);
        style = this.ValidateStyle(para.StyleName, style);
      }
      this.WriteParaStyle(para, style, listFormat);
    }
    else if (this.m_bIsPreserveListAsPara)
    {
      this.m_currListLevel = -1;
      this.PreserveListAsPara(listFormat, para);
    }
    else
      style = this.WriteList(listFormat, para);
    if (this.m_bIsPreserveListAsPara || listFormat.ListType == ListType.NoList)
      return;
    this.WriteParaStyle(para, style, listFormat);
    if (!this.m_bIsPrefixedList)
      return;
    this.m_currListLevel = -1;
    this.m_writer.WriteStartElement("span");
    if (this.m_currListCharFormat != null)
      this.m_writer.WriteAttributeString("style", this.GetStyle(this.m_currListCharFormat));
    this.m_writer.WriteRaw(this.m_prefixedValue);
    this.m_writer.WriteEndElement();
    this.m_currListCharFormat = (WCharacterFormat) null;
  }

  private void EnsureWithinDivision(WParagraph para)
  {
    if (para.ParagraphFormat.Keep && !this.isKeepValue)
    {
      Borders borders = para.ParagraphFormat.Borders;
      if (borders.Left.IsBorderDefined && borders.Right.IsBorderDefined && borders.Top.IsBorderDefined && borders.Bottom.IsBorderDefined)
      {
        this.m_writer.WriteStartElement("div");
        string style = this.GetStyle(para.ParagraphFormat, false, this.m_bIsPreserveListAsPara, (WListFormat) null);
        if (para.Document.Styles.FindByName(para.StyleName) is Style byName && !this.IsHeadingStyleNeedToPreserveAsElementSelector(byName.Name))
          this.m_writer.WriteAttributeString("class", this.EncodeName(byName.Name));
        if (!string.IsNullOrEmpty(para.StyleName))
          this.CreateNavigationPoint(para);
        if (!string.IsNullOrEmpty(style))
        {
          string str = this.ValidateStyle(para.StyleName, style);
          if (str != string.Empty)
            this.m_writer.WriteAttributeString("style", str);
        }
        this.isKeepValue = true;
        this.m_bIsParaWithinDivision = true;
      }
    }
    if (para.ParagraphFormat.Keep || !this.isKeepValue)
      return;
    this.m_writer.WriteEndElement();
    this.isKeepValue = false;
  }

  private void WriteParaStyle(WParagraph para, string style, WListFormat listFormat)
  {
    if (!string.IsNullOrEmpty(para.StyleName) && para.StyleName != "Normal" && !this.isKeepValue)
    {
      if (para.Document.Styles.FindByName(para.StyleName) is Style byName && !this.IsHeadingStyleNeedToPreserveAsElementSelector(para.StyleName) || listFormat != null && listFormat.ListType != ListType.NoList)
      {
        string key = this.EncodeName(byName.Name);
        if (this.m_document.SaveOptions.HtmlExportCssStyleSheetType != CssStyleSheetType.Inline)
          this.m_writer.WriteAttributeString("class", key);
        else if (this.m_stylesColl != null && this.m_stylesColl.ContainsKey(key))
        {
          string style1 = this.m_stylesColl[key];
          style = this.AddInlineDecorationStyle(para, style1);
        }
      }
      this.CreateNavigationPoint(para);
      if (this.m_document.SaveOptions.HtmlExportCssStyleSheetType != CssStyleSheetType.Inline)
      {
        style = this.ValidateStyle(para.StyleName, style);
        style = this.AddInlineDecorationStyle(para, style);
      }
    }
    if (style.Length <= 0)
      return;
    this.m_writer.WriteAttributeString(nameof (style), style);
  }

  private string AddInlineDecorationStyle(WParagraph para, string style)
  {
    if (para.ChildEntities.Count == 1)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (para.ChildEntities.LastItem is WTextRange)
        empty1 += this.GetStyle((para.ChildEntities.LastItem as WTextRange).CharacterFormat);
      if (empty1.ToLower().Contains("text-decoration"))
      {
        string[] strArray = empty1.Split(';');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].ToLower().Contains("text-decoration"))
            empty2 = strArray[index];
        }
      }
      if (style.ToLower().Contains("text-decoration"))
      {
        string[] strArray = style.Split(';');
        style = string.Empty;
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].ToLower().Contains("text-decoration"))
            strArray[index] = empty2;
          style = $"{style}{strArray[index]};";
        }
      }
      else
        style += empty2;
    }
    return style;
  }

  private bool IsHeadingStyleNeedToPreserveAsElementSelector(string styleName)
  {
    switch (this.EncodeName(styleName).ToLower(CultureInfo.InvariantCulture))
    {
      case "heading1":
      case "heading-1":
      case "heading 1":
      case "heading2":
      case "heading-2":
      case "heading 2":
      case "heading3":
      case "heading-3":
      case "heading 3":
      case "heading4":
      case "heading-4":
      case "heading 4":
      case "heading5":
      case "heading-5":
      case "heading 5":
      case "heading6":
      case "heading-6":
      case "heading 6":
        return true;
      default:
        return false;
    }
  }

  private string WriteList(WListFormat listFormat, WParagraph para)
  {
    string currentStyle = (string) null;
    int startAt = 0;
    if (listFormat.CurrentListLevel.PatternType != ListPatternType.Bullet)
      startAt = this.GetStartValue(listFormat);
    this.WriteListStartTag(listFormat, startAt);
    if (listFormat.OwnerParagraph.ParagraphFormat.Bidi)
    {
      this.m_writer.WriteStartElement("div");
      this.m_writer.WriteAttributeString("dir", "RTL");
    }
    this.m_writer.WriteStartElement(this.m_bIsPrefixedList ? "p" : "li");
    string existingStyle = this.GetStyle(para.ParagraphFormat, true, this.m_bIsPreserveListAsPara, listFormat);
    if (listFormat.CurrentListLevel != null)
    {
      WCharacterFormat characterFormatOfList = this.GetCharacterFormatOfList(para);
      string style;
      if (characterFormatOfList != null)
      {
        style = this.GetStyle(characterFormatOfList);
      }
      else
      {
        if (para.BreakCharacterFormat != null)
          existingStyle += this.GetStyle(para.BreakCharacterFormat);
        style = this.GetStyle(listFormat.CurrentListLevel.CharacterFormat);
      }
      currentStyle = style.Replace("font-family:'Wingdings';", string.Empty);
    }
    if (currentStyle != null)
      existingStyle = this.EnsureStyle(currentStyle, existingStyle);
    return existingStyle;
  }

  private WCharacterFormat GetCharacterFormatOfList(WParagraph paragraph)
  {
    if (paragraph.ListFormat.IsEmptyList || paragraph.SectionEndMark)
      return (WCharacterFormat) null;
    WCharacterFormat destFormat = (WCharacterFormat) null;
    WListFormat wlistFormat = (WListFormat) null;
    WParagraphStyle paraStyle = paragraph.ParaStyle as WParagraphStyle;
    if (paragraph.ListFormat.ListType != ListType.NoList)
      wlistFormat = paragraph.ListFormat;
    else if (paraStyle != null && paraStyle.ListFormat.ListType != ListType.NoList)
      wlistFormat = paraStyle.ListFormat;
    if (wlistFormat != null && wlistFormat.CurrentListStyle != null)
    {
      ListStyle currentListStyle = wlistFormat.CurrentListStyle;
      int levelNumber = 0;
      if (paragraph.ListFormat.HasKey(0))
        levelNumber = paragraph.ListFormat.ListLevelNumber;
      else if (paraStyle != null && paraStyle.ListFormat.HasKey(0))
        levelNumber = paraStyle.ListFormat.ListLevelNumber;
      WListLevel wlistLevel = currentListStyle.GetNearLevel(levelNumber);
      ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
      if (!string.IsNullOrEmpty(wlistFormat.LFOStyleName))
        listOverrideStyle = this.m_document.ListOverrides.FindByName(wlistFormat.LFOStyleName);
      if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(levelNumber) && listOverrideStyle.OverrideLevels[levelNumber].OverrideFormatting)
        wlistLevel = listOverrideStyle.OverrideLevels[levelNumber].OverrideListLevel;
      destFormat = new WCharacterFormat((IWordDocument) this.m_document);
      destFormat.ImportContainer((FormatBase) paragraph.BreakCharacterFormat);
      destFormat.CopyProperties((FormatBase) paragraph.BreakCharacterFormat);
      destFormat.ApplyBase(paragraph.BreakCharacterFormat.BaseFormat);
      if (destFormat.PropertiesHash.ContainsKey(7))
      {
        destFormat.UnderlineStyle = UnderlineStyle.None;
        destFormat.PropertiesHash.Remove(7);
      }
      this.CopyCharacterFormatting(wlistLevel.CharacterFormat, destFormat);
    }
    return destFormat;
  }

  private void CopyCharacterFormatting(WCharacterFormat sourceFormat, WCharacterFormat destFormat)
  {
    if (sourceFormat.HasValue(3))
      destFormat.SetPropertyValue(3, (object) sourceFormat.FontSize);
    if (sourceFormat.HasValue(1))
      destFormat.TextColor = sourceFormat.TextColor;
    if (sourceFormat.HasValue(2))
      destFormat.FontName = sourceFormat.FontName;
    if (sourceFormat.HasValue(4))
      destFormat.Bold = sourceFormat.Bold;
    if (sourceFormat.HasValue(5))
      destFormat.Italic = sourceFormat.Italic;
    if (sourceFormat.HasValue(7))
      destFormat.UnderlineStyle = sourceFormat.UnderlineStyle;
    if (sourceFormat.HasValue(63 /*0x3F*/))
      destFormat.HighlightColor = sourceFormat.HighlightColor;
    if (sourceFormat.HasValue(50))
      destFormat.Shadow = sourceFormat.Shadow;
    if (sourceFormat.HasValue(18))
      destFormat.SetPropertyValue(18, (object) sourceFormat.CharacterSpacing);
    if (sourceFormat.HasValue(14))
      destFormat.DoubleStrike = sourceFormat.DoubleStrike;
    if (sourceFormat.HasValue(51))
      destFormat.Emboss = sourceFormat.Emboss;
    if (sourceFormat.HasValue(52))
      destFormat.Engrave = sourceFormat.Engrave;
    if (sourceFormat.HasValue(10))
      destFormat.SubSuperScript = sourceFormat.SubSuperScript;
    destFormat.TextBackgroundColor = sourceFormat.TextBackgroundColor;
    if (sourceFormat.HasValue(54))
      destFormat.AllCaps = sourceFormat.AllCaps;
    if (sourceFormat.Bidi)
    {
      destFormat.Bidi = true;
      destFormat.FontNameBidi = sourceFormat.FontNameBidi;
      destFormat.SetPropertyValue(62, (object) sourceFormat.FontSizeBidi);
    }
    if (sourceFormat.HasValue(59))
      destFormat.BoldBidi = sourceFormat.BoldBidi;
    if (sourceFormat.HasValue(109))
      destFormat.FieldVanish = sourceFormat.FieldVanish;
    if (sourceFormat.HasValue(53))
      destFormat.Hidden = sourceFormat.Hidden;
    if (sourceFormat.HasValue(24))
      destFormat.SpecVanish = sourceFormat.SpecVanish;
    if (!sourceFormat.HasValue(55))
      return;
    destFormat.SmallCaps = sourceFormat.SmallCaps;
  }

  private string EnsureStyle(string currentStyle, string existingStyle)
  {
    string[] strArray = existingStyle.Split(';');
    string str1 = currentStyle;
    char[] chArray = new char[1]{ ';' };
    foreach (string str2 in str1.Split(chArray))
    {
      bool flag = false;
      if (str2.Length > 0)
      {
        int length1 = str2.IndexOf(":");
        string str3 = str2.Substring(0, length1);
        string newValue1 = str2.Substring(length1 + 1);
        foreach (string oldValue1 in strArray)
        {
          if (oldValue1.Length > 0)
          {
            int length2 = oldValue1.IndexOf(":");
            if (oldValue1.Substring(0, length2).ToLower() == str3.ToLower())
            {
              flag = true;
              string oldValue2 = oldValue1.Substring(length2 + 1);
              string newValue2 = oldValue1.Replace(oldValue2, newValue1);
              existingStyle = existingStyle.Replace(oldValue1, newValue2);
            }
          }
        }
        if (!flag)
          existingStyle = $"{existingStyle}{str3}:{newValue1};";
      }
    }
    return existingStyle;
  }

  private string ValidateStyle(string p, string style)
  {
    if (!string.IsNullOrEmpty(p) && this.m_stylesColl.ContainsKey(p))
    {
      string str1 = this.m_stylesColl[p];
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.Length > 0 && style.Length > 0)
          style = style.Replace(str2 + ";", string.Empty);
      }
    }
    return style;
  }

  private void WriteListStartTag(WListFormat listFormat, int startAt)
  {
    bool flag1 = false;
    if (this.m_currListLevel < 0)
      return;
    bool flag2;
    if (!(flag2 = this.listStack.Contains(this.m_currListLevel)) && listFormat.CurrentListLevel.PatternType == ListPatternType.Bullet)
    {
      this.listStack.Push(this.m_currListLevel);
      flag1 = true;
    }
    if (listFormat.CurrentListLevel.PatternType == ListPatternType.Bullet)
    {
      if (flag1)
      {
        this.m_writer.WriteStartElement("ul");
        this.WriteListType(listFormat.CurrentListLevel.PatternType, listFormat);
        this.m_writer.WriteAttributeString("style", "margin:0pt; padding-left:0pt");
      }
      this.m_writer.WriteRaw(ControlChar.CrLf);
    }
    else
    {
      bool flag3;
      if ((flag3 = string.IsNullOrEmpty(listFormat.CurrentListLevel.NumberPrefix)) || !listFormat.CurrentListLevel.NumberPrefix.StartsWithExt("\0."))
      {
        if (flag3 && !string.IsNullOrEmpty(listFormat.CurrentListLevel.NumberSuffix) && listFormat.CurrentListLevel.NumberSuffix != ".")
          flag3 = false;
        if (flag3)
        {
          if (!flag2)
          {
            this.listStack.Push(this.m_currListLevel);
            this.m_writer.WriteStartElement("ol");
            this.WriteListType(listFormat.CurrentListLevel.PatternType, listFormat);
            if (startAt >= 0)
              this.m_writer.WriteAttributeString("start", startAt.ToString((IFormatProvider) CultureInfo.InvariantCulture));
            this.m_writer.WriteAttributeString("style", "margin:0pt; padding-left:0pt");
          }
          this.m_writer.WriteRaw(ControlChar.CrLf);
        }
        else
        {
          WListLevel listLevel = this.m_currPara.GetListLevel(listFormat);
          this.m_prefixedValue = this.m_document.UpdateListValue(this.m_currPara, listFormat, listLevel);
          this.m_bIsPrefixedList = true;
          this.m_bIsWriteListTab = true;
        }
      }
      else
      {
        this.m_bIsPrefixedList = true;
        this.m_prefixedValue = this.GetPrefixValue(listFormat, startAt);
      }
    }
  }

  private int GetStartValue(WListFormat listFormat)
  {
    if (listFormat.RestartNumbering)
      this.EnsureLvlRestart(listFormat, true);
    else if (listFormat.ListLevelNumber == 0)
      this.EnsureLvlRestart(listFormat, false);
    return this.GetLstStartVal(listFormat);
  }

  private void PreserveListAsPara(WListFormat listFormat, WParagraph para)
  {
    int startValue = this.GetStartValue(listFormat);
    this.m_writer.WriteStartElement("p");
    string style = this.GetStyle(para.ParagraphFormat, true, this.m_bIsPreserveListAsPara, listFormat);
    if (listFormat.CurrentListLevel.NumberPrefix != null && listFormat.CurrentListLevel.NumberPrefix.StartsWithExt("\0."))
    {
      this.m_bIsPrefixedList = true;
      this.m_prefixedValue = this.GetPrefixValue(listFormat, startValue);
    }
    this.WriteParaStyle(para, style, listFormat);
    this.PreserveBulletsAndNumberingAsText(listFormat, startValue);
  }

  private void PreserveBulletsAndNumberingAsText(WListFormat listFormat, int startAt)
  {
    string style = this.GetStyle(listFormat.CurrentListLevel.CharacterFormat);
    this.m_writer.WriteStartElement("span");
    this.m_writer.WriteAttributeString("style", style);
    if (listFormat.CurrentListLevel.PatternType == ListPatternType.Bullet)
    {
      if (listFormat.CurrentListLevel.CharacterFormat.FontName.ToLower(CultureInfo.InvariantCulture) == "symbol" || listFormat.CurrentListLevel.CharacterFormat.FontName.ToLower(CultureInfo.InvariantCulture) == "wingdings")
        this.m_writer.WriteRaw($"&#{((byte) listFormat.CurrentListLevel.BulletCharacter[0]).ToString((IFormatProvider) CultureInfo.InvariantCulture)};");
      else
        this.m_writer.WriteRaw(listFormat.CurrentListLevel.BulletCharacter);
    }
    else if (this.m_bIsPrefixedList)
      this.m_writer.WriteRaw(this.m_prefixedValue);
    else
      this.m_writer.WriteRaw(DocxSerializator.ReplaceInvalidSurrogateCharacters(this.GetNumberingsAsText(listFormat.CurrentListLevel.PatternType, startAt)));
    if (!this.m_bIsPrefixedList && listFormat.CurrentListLevel.NumberSuffix != null)
      this.m_writer.WriteRaw(DocxSerializator.ReplaceInvalidSurrogateCharacters(listFormat.CurrentListLevel.NumberSuffix));
    this.m_writer.WriteEndElement();
    this.WriteTabSpace(listFormat);
  }

  private void WriteTabSpace(WListFormat listFormat)
  {
    StringBuilder stringBuilder = new StringBuilder();
    float textPosition = listFormat.CurrentListLevel.TextPosition;
    float num1 = listFormat.CurrentListLevel.TextPosition + listFormat.CurrentListLevel.NumberPosition;
    int num2 = (double) listFormat.CurrentListLevel.TabSpaceAfter > 0.0 ? (int) Math.Round(((double) listFormat.CurrentListLevel.TabSpaceAfter - (double) num1) / 36.0) : (int) Math.Round(((double) textPosition - (double) num1) / 36.0);
    this.m_writer.WriteStartElement("span");
    stringBuilder.Append($"font-size:{Math.Round(7.0 / 12.0, 2).ToString((IFormatProvider) CultureInfo.InvariantCulture)}em;");
    stringBuilder.Append("font-family:'Times New Roman';");
    this.m_writer.WriteAttributeString("style", stringBuilder.ToString());
    if (num2 > 0)
    {
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < 22; ++index2)
          this.m_writer.WriteRaw("&#xa0;");
      }
    }
    else
      this.m_writer.WriteRaw("&#xa0;&#xa0;&#xa0;&#xa0;&#xa0;&#xa0;&#xa0;");
    this.m_writer.WriteEndElement();
  }

  private string GetNumberingsAsText(ListPatternType type, int startAt)
  {
    string numberingsAsText;
    switch (type)
    {
      case ListPatternType.UpRoman:
        numberingsAsText = this.ConvertArabicToRoman(startAt).ToUpper(CultureInfo.InvariantCulture);
        break;
      case ListPatternType.LowRoman:
        numberingsAsText = this.ConvertArabicToRoman(startAt).ToLower(CultureInfo.InvariantCulture);
        break;
      case ListPatternType.UpLetter:
        numberingsAsText = ((char) (65 + (startAt - 1))).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case ListPatternType.LowLetter:
        numberingsAsText = ((char) (97 + (startAt - 1))).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        break;
      default:
        numberingsAsText = startAt.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        break;
    }
    return numberingsAsText;
  }

  private string ConvertArabicToRoman(int arabic)
  {
    string roman = "";
    for (int index = 0; index < arabic; ++index)
    {
      for (; arabic >= 1000; arabic -= 1000)
        roman += "M";
      for (; arabic >= 900; arabic -= 900)
        roman += "CM";
      for (; arabic >= 500; arabic -= 500)
        roman += "D";
      for (; arabic >= 400; arabic -= 400)
        roman += "CD";
      for (; arabic >= 100; arabic -= 100)
        roman += "C";
      for (; arabic >= 90; arabic -= 90)
        roman += "XC";
      for (; arabic >= 50; arabic -= 50)
        roman += "L";
      for (; arabic >= 40; arabic -= 40)
        roman += "XL";
      for (; arabic >= 10; arabic -= 10)
        roman += "X";
      for (; arabic >= 9; arabic -= 9)
        roman += "IX";
      for (; arabic >= 5; arabic -= 5)
        roman += "V";
      for (; arabic >= 4; arabic -= 4)
        roman += "IV";
      for (; arabic >= 1; --arabic)
        roman += "I";
    }
    return roman;
  }

  private string GetPrefixValue(WListFormat listFormat, int startAt)
  {
    string prefixValue = string.Empty;
    int levelNumber = listFormat.CurrentListLevel.LevelNumber;
    if (this.Lists.ContainsKey(listFormat.CustomStyleName))
    {
      string str = string.Empty;
      Dictionary<int, int> list = this.Lists[listFormat.CustomStyleName];
      for (int key = 0; key < levelNumber; ++key)
      {
        if (list.ContainsKey(key))
          str = $"{str}{(Convert.ToInt32(list[key]) - 1).ToString((IFormatProvider) CultureInfo.InvariantCulture)}.";
      }
      prefixValue = str + startAt.ToString((IFormatProvider) CultureInfo.InvariantCulture) + listFormat.CurrentListLevel.NumberSuffix;
    }
    return prefixValue;
  }

  private void WriteBookmark(BookmarkStart bookmark)
  {
    if (bookmark.Name == "_GoBack")
      return;
    this.m_writer.WriteStartElement("a");
    this.m_writer.WriteAttributeString("id", bookmark.Name);
    this.m_writer.WriteRaw(string.Empty);
    this.m_writer.WriteEndElement();
  }

  private void WriteField(WField field)
  {
    if (field.FieldEnd == null)
      return;
    if (field.FieldType == FieldType.FieldHyperlink)
    {
      this.WriteHyperlink(new Hyperlink(field));
      this.m_bSkipFieldItem = true;
    }
    else
    {
      this.InsertFieldBegin(field);
      if (field.FieldType == FieldType.FieldEmbed)
        this.m_bSkipFieldItem = true;
    }
    this.PushToFieldStack(field);
  }

  private void PushToFieldStack(WField field)
  {
    if (field.FieldSeparator != null || field.FieldEnd != null)
      this.m_bIsFieldCode = true;
    this.FieldStack.Push(field);
  }

  private void InsertFieldBegin(WField field)
  {
    string empty = string.Empty;
    if (field.CharacterFormat != null)
      this.GetStyle(field.CharacterFormat);
    if (this.FieldStack.Count == 0 || this.CurrentField.IsInFieldResult)
      this.m_writer.WriteRaw("<!--[if supportFields]>");
    this.m_writer.WriteStartElement("span");
    this.m_writer.WriteAttributeString("style", "mso-element:field-begin");
    this.m_writer.WriteRaw("");
    this.m_writer.WriteEndElement();
  }

  private void WriteFieldMark(WFieldMark fieldMark)
  {
    bool flag = this.PreviousField != null && this.PreviousField.IsInFieldResult;
    if (this.CurrentField == null)
      return;
    if (fieldMark.Type == FieldMarkType.FieldSeparator)
    {
      this.m_writer.WriteStartElement("span");
      this.m_writer.WriteAttributeString("style", "mso-element:field-separator");
      this.m_writer.WriteRaw("");
      this.m_writer.WriteEndElement();
      if (this.FieldStack.Count != 1 && !flag)
        return;
      this.m_writer.WriteRaw("<![endif]-->");
      this.CurrentField.IsInFieldResult = true;
    }
    else
    {
      if (fieldMark.Type != FieldMarkType.FieldEnd)
        return;
      if (this.CurrentField.FieldType != FieldType.FieldTOC)
      {
        if (this.FieldStack.Count == 1 || flag)
          this.m_writer.WriteRaw("<!--[if supportFields]>");
        this.m_writer.WriteStartElement("span");
        this.m_writer.WriteAttributeString("style", "mso-element:field-end");
        this.m_writer.WriteRaw("");
        this.m_writer.WriteEndElement();
        if (this.FieldStack.Count == 1 || flag)
        {
          this.m_writer.WriteRaw("<![endif]-->");
          this.CurrentField.IsInFieldResult = false;
        }
      }
      this.FieldStack.Pop();
    }
  }

  private void WriteHyperlink(Hyperlink hyperlink)
  {
    this.m_writer.WriteStartElement("a");
    string charStyleName = hyperlink.Field.CharacterFormat.CharStyleName;
    if (charStyleName != null && charStyleName.Length > 0)
      this.m_writer.WriteAttributeString("class", this.GetClassAttr(hyperlink.Field.Document.Styles.FindByName(charStyleName) as Style));
    WField field = hyperlink.Field;
    WTextRange wtextRange = new WTextRange((IWordDocument) field.Document);
    if (field.FieldSeparator != null && field.FieldSeparator.NextSibling is WTextRange)
      wtextRange = field.FieldSeparator.NextSibling as WTextRange;
    string style = this.GetStyle(wtextRange.CharacterFormat, false);
    if (!string.IsNullOrEmpty(charStyleName))
      style = this.ValidateStyle(charStyleName, style);
    if (style.Length > 0)
      this.m_writer.WriteAttributeString("style", style);
    string str = string.Empty;
    if (field.IsLocal && field.LocalReference != null && field.LocalReference != string.Empty)
      str = "#" + field.LocalReference;
    switch (hyperlink.Type)
    {
      case HyperlinkType.FileLink:
        this.m_writer.WriteAttributeString("href", hyperlink.FilePath + str);
        break;
      case HyperlinkType.WebLink:
        this.m_writer.WriteAttributeString("href", hyperlink.Uri + str);
        break;
      case HyperlinkType.EMailLink:
        this.m_writer.WriteAttributeString("href", hyperlink.Uri);
        break;
      case HyperlinkType.Bookmark:
        this.m_writer.WriteAttributeString("href", "#" + hyperlink.BookmarkName);
        break;
    }
  }

  private void WriteImage(WPicture pic)
  {
    Syncfusion.DocIO.DLS.Entities.ImageFormat imageFormat = Syncfusion.DocIO.DLS.Entities.ImageFormat.Bmp;
    System.Drawing.Image image = (System.Drawing.Image) null;
    System.Drawing.Imaging.ImageFormat format = (System.Drawing.Imaging.ImageFormat) null;
    string str1;
    if (WordDocument.EnablePartialTrustCode)
    {
      imageFormat = pic.ImageForPartialTrustMode.Format;
      str1 = "." + imageFormat.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture);
    }
    else
    {
      image = pic.GetImage(pic.ImageBytes, false);
      format = this.GetImageFormat(string.Format((IFormatProvider) CultureInfo.InvariantCulture, image.RawFormat.Guid.ToString()));
      str1 = "." + format.ToString().ToLower(CultureInfo.InvariantCulture);
    }
    string str2 = this.GetImagePath() + str1;
    int width;
    int height;
    if (Math.Round((double) pic.WidthScale) > 0.0 || Math.Round((double) pic.HeightScale) > 0.0)
    {
      width = (int) Math.Round((double) UnitsConvertor.Instance.ConvertToPixels(pic.Width, PrintUnits.Point));
      height = (int) Math.Round((double) UnitsConvertor.Instance.ConvertToPixels(pic.Height, PrintUnits.Point));
    }
    else
    {
      width = Convert.ToInt32(pic.Size.Width);
      height = Convert.ToInt32(pic.Size.Height);
    }
    if (width < 0 || height < 0)
      return;
    if (pic.TextWrappingStyle == TextWrappingStyle.InFrontOfText || pic.TextWrappingStyle == TextWrappingStyle.Behind)
    {
      int num1 = pic.OrderIndex;
      if (this.BehindWrapStyleFloatingItems.ContainsKey(pic))
        num1 = this.BehindWrapStyleFloatingItems[pic];
      this.m_writer.WriteStartElement("span");
      if (pic.VerticalAlignment == ShapeVerticalAlignment.Center && pic.HorizontalAlignment == ShapeHorizontalAlignment.Center)
        this.m_writer.WriteAttributeString("style", $"position:{(object) ShapePosition.Absolute}; width:{(object) width}px; height:{(object) height}px; left:0px; margin-left:0px; margin-top:0px;z-index:{(object) num1}");
      else if (pic.HorizontalAlignment == ShapeHorizontalAlignment.Right)
      {
        int num2 = 1024 /*0x0400*/ - width;
        this.m_writer.WriteAttributeString("style", $"position:{(object) ShapePosition.Absolute}; width:{(object) width}px; height:{(object) height}px; left:0px; margin-left:{(object) num2}px; margin-top:0px;z-index:{(object) num1}");
      }
      else
        this.m_writer.WriteAttributeString("style", $"position:{(object) ShapePosition.Absolute}; width:{(object) width}px; height:{(object) height}px; left:0px; margin-left:{(object) Math.Round((double) UnitsConvertor.Instance.ConvertToPixels(pic.HorizontalPosition, PrintUnits.Point))}px; margin-top:{(object) Math.Round((double) UnitsConvertor.Instance.ConvertToPixels(pic.VerticalPosition, PrintUnits.Point))}px;z-index:{(object) num1}");
    }
    this.m_writer.WriteStartElement("img");
    if (pic.Document.SaveFormatType == FormatType.EPub)
    {
      this.m_imageFiles.Add(str2, (Stream) new MemoryStream(pic.ImageBytes));
      this.m_writer.WriteAttributeString("src", str2);
    }
    else if (pic.Document.SaveOptions.IsEventSubscribed)
    {
      ImageNodeVisitedEventArgs visitedEventArgs = pic.Document.SaveOptions.ExecuteSaveImageEvent((Stream) new MemoryStream(pic.ImageBytes), (string) null);
      if (visitedEventArgs != null && !string.IsNullOrEmpty(visitedEventArgs.Uri))
        this.m_writer.WriteAttributeString("src", visitedEventArgs.Uri);
    }
    else if (this.m_imagesFolder == "\\" || pic.Document.SaveOptions.HTMLExportImageAsBase64)
    {
      string str3 = string.Empty;
      if (WordDocument.EnablePartialTrustCode)
        str3 = "data:image/" + imageFormat.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture);
      else if (format != null)
        str3 = "data:image/" + format.ToString().ToLower(CultureInfo.InvariantCulture);
      string base64String;
      if (pic.IsMetaFile)
      {
        Bitmap bitmap = this.ConvertEMFToBitmap(image);
        MemoryStream memoryStream = new MemoryStream();
        bitmap.Save((Stream) memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        base64String = Convert.ToBase64String(memoryStream.ToArray());
        memoryStream.Dispose();
        bitmap.Dispose();
      }
      else
        base64String = Convert.ToBase64String(pic.ImageBytes);
      this.m_writer.WriteAttributeString("src", $"{str3};base64,{base64String}");
    }
    else
    {
      this.EnsureImagesFolder();
      if (WordDocument.EnablePartialTrustCode || format == System.Drawing.Imaging.ImageFormat.Gif)
        this.ProcessImageUsingFileStream(str2, pic.ImageBytes);
      else
        this.ProcessImage(image, str2, width, height, pic.IsMetaFile, format);
      this.m_writer.WriteAttributeString("src", (this.UseAbsolutePath ? this.m_imageRelativePath : string.Empty) + str2);
    }
    this.m_writer.WriteAttributeString("width", width.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    this.m_writer.WriteAttributeString("height", height.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    if (pic.TextWrappingStyle == TextWrappingStyle.Square || pic.TextWrappingStyle == TextWrappingStyle.Tight || pic.TextWrappingStyle == TextWrappingStyle.Through)
    {
      if (pic.HorizontalAlignment == ShapeHorizontalAlignment.Right)
        this.m_writer.WriteAttributeString("align", "right");
      else
        this.m_writer.WriteAttributeString("align", "left");
    }
    this.m_writer.WriteAttributeString("alt", pic.AlternativeText != null ? pic.AlternativeText : string.Empty);
    this.m_writer.WriteEndElement();
    if (pic.TextWrappingStyle == TextWrappingStyle.TopAndBottom)
    {
      this.m_writer.WriteStartElement("br");
      this.m_writer.WriteAttributeString("clear", "ALL");
      this.m_writer.WriteEndElement();
    }
    if (pic.TextWrappingStyle != TextWrappingStyle.InFrontOfText && pic.TextWrappingStyle != TextWrappingStyle.Behind)
      return;
    this.m_writer.WriteEndElement();
  }

  private System.Drawing.Imaging.ImageFormat GetImageFormat(string value)
  {
    return value == string.Format((IFormatProvider) CultureInfo.InvariantCulture, System.Drawing.Imaging.ImageFormat.Emf.Guid.ToString()) || value == string.Format((IFormatProvider) CultureInfo.InvariantCulture, System.Drawing.Imaging.ImageFormat.Wmf.Guid.ToString()) ? System.Drawing.Imaging.ImageFormat.Png : (!(value == string.Format((IFormatProvider) CultureInfo.InvariantCulture, System.Drawing.Imaging.ImageFormat.Png.Guid.ToString())) ? (!(value == string.Format((IFormatProvider) CultureInfo.InvariantCulture, System.Drawing.Imaging.ImageFormat.Gif.Guid.ToString())) ? System.Drawing.Imaging.ImageFormat.Jpeg : System.Drawing.Imaging.ImageFormat.Gif) : System.Drawing.Imaging.ImageFormat.Png);
  }

  private void WriteTextRange(WTextRange tr, string combinedText)
  {
    if (combinedText == ControlChar.Tab && tr.NextSibling is WField && (tr.NextSibling as WField).FieldType == FieldType.FieldPageRef && this.m_bSkiPageRefFieldItem)
      return;
    if (combinedText == '\u0002'.ToString((IFormatProvider) CultureInfo.InvariantCulture))
    {
      this.WriteFtntAttributes(tr.CharacterFormat);
    }
    else
    {
      if (string.IsNullOrEmpty(combinedText))
        return;
      this.m_writer.WriteStartElement("span");
      if (Enum.IsDefined(typeof (LocaleIDs), (object) (int) tr.CharacterFormat.LocaleIdASCII))
        this.m_writer.WriteAttributeString("lang", ((LocaleIDs) tr.CharacterFormat.LocaleIdASCII).ToString((IFormatProvider) CultureInfo.InvariantCulture).Replace('_', '-'));
      string style = this.GetStyle(tr.CharacterFormat, false);
      string text = combinedText;
      bool flag = false;
      if (this.IsParaHasTab)
      {
        if (text == ControlChar.Tab)
        {
          this.WriteTabText(tr, style);
          flag = true;
        }
        else if (text.Contains(ControlChar.Tab))
        {
          this.WriteTabContainedText(text, style, tr);
          flag = true;
        }
        else if (!this.m_bIsFieldCode)
          this.CurrentLineWidth += this.DrawingContext.MeasureString(text, tr.CharacterFormat.Font, (StringFormat) null).Width;
      }
      if (flag)
        return;
      this.WriteSpanText(text, style, tr);
      this.m_writer.WriteEndElement();
    }
  }

  private TabsLayoutInfo.LayoutTab GetCurrentLayoutTab()
  {
    if (this.LayoutTabList == null)
      return (TabsLayoutInfo.LayoutTab) null;
    TabsLayoutInfo.LayoutTab currentLayoutTab = (TabsLayoutInfo.LayoutTab) null;
    for (int index = 0; index < this.LayoutTabList.Count; ++index)
    {
      if ((double) this.LayoutTabList[index].Position > (double) this.CurrentLineWidth)
      {
        currentLayoutTab = this.LayoutTabList[index];
        break;
      }
    }
    return currentLayoutTab;
  }

  private string GetTabStyle(
    float width,
    WCharacterFormat format,
    TabsLayoutInfo.LayoutTab layoutTab)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"width:{(object) Math.Round((double) width, 2)}pt; text-indent:0pt;");
    if ((double) format.CharacterSpacing > 0.0)
      stringBuilder.Append($"letter-spacing:{(object) format.CharacterSpacing}pt;");
    if (format.TextColor != Color.Empty)
      stringBuilder.Append($"color:{this.GetColor(format.TextColor)};");
    stringBuilder.Append($"font-family:{format.FontName};");
    if ((double) format.FontSize > 0.0)
      stringBuilder.Append($"font-size:{format.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    if (!format.HighlightColor.IsEmpty)
      stringBuilder.Append($"background-color:{this.GetColor(format.HighlightColor)};");
    else if (!format.TextBackgroundColor.IsEmpty)
      stringBuilder.Append($"background-color:{this.GetColor(format.TextBackgroundColor)};");
    if (format.Bold || format.Emboss || format.OutLine)
      stringBuilder.Append("font-weight:bold;");
    else
      stringBuilder.Append("font-weight:normal;");
    if (this.m_currPara == null || this.m_currPara != null && this.m_currPara.Text != "")
    {
      if (this.m_fieldStack.Count > 0 && this.m_document.SaveFormatType != FormatType.EPub)
      {
        if (this.m_fieldStack.Peek().FieldType == FieldType.FieldHyperlink)
          stringBuilder.Append("text-decoration: underline;");
        else if (format.UnderlineStyle != UnderlineStyle.None)
          stringBuilder.Append("text-decoration: underline;");
      }
      else if (format.UnderlineStyle != UnderlineStyle.None)
        stringBuilder.Append("text-decoration: underline;");
      if (format.DoubleStrike || format.Strikeout)
        stringBuilder.Append("text-decoration: line-through;");
    }
    if (this.m_currPara != null && (double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) > 0.0)
    {
      if ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) != 12.0 && this.m_currPara.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple)
        stringBuilder.Append($"line-height:{((float) ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) / 12.0 * 100.0)).ToString((IFormatProvider) CultureInfo.InvariantCulture)}%;");
      else if ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) != 12.0 && (this.m_currPara.ParagraphFormat.LineSpacingRule != LineSpacingRule.AtLeast || (double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) > 10.0))
        stringBuilder.Append($"line-height:{Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    if (format.Hidden)
      stringBuilder.Append("display:none;");
    if (format.Italic)
      stringBuilder.Append("font-style:italic;");
    else
      stringBuilder.Append("font-style:normal;");
    if (layoutTab != null)
    {
      stringBuilder.Append($"-sf-tabstop-align:{layoutTab.Justification.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
      if (layoutTab.TabLeader != Syncfusion.Layouting.TabLeader.NoLeader)
        stringBuilder.Append($"-sf-tabstop-leader:{layoutTab.TabLeader.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
      if ((double) layoutTab.Position != 0.0)
        stringBuilder.Append($"-sf-tabstop-pos:{layoutTab.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)}pt;");
    }
    stringBuilder.Append(" display:inline-block;");
    return stringBuilder.ToString();
  }

  private string GetTabLeader(float width, Syncfusion.Layouting.TabLeader tabLeader, Font font)
  {
    string str = ControlChar.NonBreakingSpace;
    switch (tabLeader)
    {
      case Syncfusion.Layouting.TabLeader.Dotted:
        str = ".";
        break;
      case Syncfusion.Layouting.TabLeader.Hyphenated:
        str = "-";
        break;
      case Syncfusion.Layouting.TabLeader.Single:
        str = "_";
        break;
      case Syncfusion.Layouting.TabLeader.Heavy:
        str = "·";
        break;
    }
    string text = str;
    float width1 = this.DrawingContext.MeasureString(text, font, (StringFormat) null).Width;
    while ((double) width1 < (double) width)
    {
      text += str;
      width1 = this.DrawingContext.MeasureString(text, font, (StringFormat) null).Width;
      if ((double) width1 > (double) width && text.Length > 1)
        text = text.Substring(0, text.Length - 1);
    }
    return text;
  }

  private float GetTabWidth(TabsLayoutInfo.LayoutTab layoutTab)
  {
    return layoutTab == null ? ((double) this.CurrentLineWidth >= (double) this.DefaultTabWidth ? this.DefaultTabWidth * (float) ((int) Math.Truncate((double) (this.CurrentLineWidth / this.DefaultTabWidth)) + 1) - this.CurrentLineWidth : this.DefaultTabWidth - this.CurrentLineWidth) : layoutTab.Position - this.CurrentLineWidth;
  }

  private void WriteTabContainedText(string text, string style, WTextRange tr)
  {
    List<string> splittedText = this.GetSplittedText(text);
    for (int index = 0; index < splittedText.Count; ++index)
    {
      string style1 = style;
      if (splittedText[index] == string.Empty)
      {
        TabsLayoutInfo.LayoutTab currentLayoutTab = this.GetCurrentLayoutTab();
        if (currentLayoutTab == null || currentLayoutTab.Justification == Syncfusion.Layouting.TabJustification.Left)
        {
          float tabWidth = this.GetTabWidth(currentLayoutTab);
          style1 = this.GetTabStyle(tabWidth, tr.CharacterFormat, currentLayoutTab);
          this.CurrentLineWidth += tabWidth;
          splittedText[index] = currentLayoutTab == null || currentLayoutTab.TabLeader == Syncfusion.Layouting.TabLeader.NoLeader ? this.GetTabLeader(tabWidth, Syncfusion.Layouting.TabLeader.NoLeader, tr.CharacterFormat.Font) : this.GetTabLeader(tabWidth, currentLayoutTab.TabLeader, tr.CharacterFormat.Font);
        }
        else
        {
          string tabText = this.GetTabText();
          this.CurrentLineWidth += this.DrawingContext.MeasureString(tabText, tr.CharacterFormat.Font, (StringFormat) null).Width;
          splittedText[index] = tabText;
        }
      }
      else
        this.CurrentLineWidth += this.DrawingContext.MeasureString(splittedText[index], tr.CharacterFormat.Font, (StringFormat) null).Width;
      if (index != 0)
        this.m_writer.WriteStartElement("span");
      this.WriteSpanText(splittedText[index], style1, tr);
      this.m_writer.WriteEndElement();
    }
  }

  private List<string> GetSplittedText(string text)
  {
    List<string> splittedText = new List<string>();
    int length;
    for (; text.Contains("\t"); text = text.Substring(length + 1))
    {
      length = text.IndexOf('\t');
      if (length != 0)
        splittedText.Add(text.Substring(0, length));
      splittedText.Add(string.Empty);
    }
    if (text != string.Empty)
      splittedText.Add(text);
    return splittedText;
  }

  private void WriteTabText(WTextRange tr, string style)
  {
    TabsLayoutInfo.LayoutTab currentLayoutTab = this.GetCurrentLayoutTab();
    if (currentLayoutTab == null || currentLayoutTab.Justification == Syncfusion.Layouting.TabJustification.Left)
    {
      float tabWidth = this.GetTabWidth(currentLayoutTab);
      string tabStyle = this.GetTabStyle(tabWidth, tr.CharacterFormat, currentLayoutTab);
      this.CurrentLineWidth += tabWidth;
      string text = currentLayoutTab == null || currentLayoutTab.TabLeader == Syncfusion.Layouting.TabLeader.NoLeader ? this.GetTabLeader(tabWidth, Syncfusion.Layouting.TabLeader.NoLeader, tr.CharacterFormat.Font) : this.GetTabLeader(tabWidth, currentLayoutTab.TabLeader, tr.CharacterFormat.Font);
      if (tabStyle.Length > 0)
        this.m_writer.WriteAttributeString(nameof (style), tabStyle);
      if (tr.CharacterFormat.SubSuperScript == SubSuperScript.SubScript)
        this.m_writer.WriteStartElement("sub");
      else if (tr.CharacterFormat.SubSuperScript == SubSuperScript.SuperScript)
        this.m_writer.WriteStartElement("sup");
      this.WriteText(text);
      if (tr.CharacterFormat.SubSuperScript == SubSuperScript.SubScript || tr.CharacterFormat.SubSuperScript == SubSuperScript.SuperScript)
        this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
    else
    {
      string tabText = this.GetTabText();
      this.CurrentLineWidth += this.DrawingContext.MeasureString(tabText, tr.CharacterFormat.Font, (StringFormat) null).Width;
      this.WriteSpanText(tabText, style, tr);
      this.m_writer.WriteEndElement();
    }
  }

  private void WriteListFollowText(WListLevel level)
  {
    this.m_writer.WriteStartElement("span");
    if (level.FollowCharacter == FollowCharacterType.Tab)
    {
      Font font = level.Document.FontSettings.GetFont("Times New Roman", 7f, FontStyle.Regular);
      TabsLayoutInfo.LayoutTab currentLayoutTab = this.GetCurrentLayoutTab();
      if (currentLayoutTab == null || currentLayoutTab.Justification == Syncfusion.Layouting.TabJustification.Left || currentLayoutTab.Justification == (Syncfusion.Layouting.TabJustification.Right | Syncfusion.Layouting.TabJustification.Bar))
      {
        float tabWidth = this.GetTabWidth(currentLayoutTab);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("font: 7.0pt \"Times New Roman\";-sf-listtab:yes;");
        if (currentLayoutTab != null)
        {
          stringBuilder.Append($"-sf-tabstop-align:{((TabJustification) currentLayoutTab.Justification).ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
          if (currentLayoutTab.TabLeader != Syncfusion.Layouting.TabLeader.NoLeader)
            stringBuilder.Append($"-sf-tabstop-leader:{currentLayoutTab.TabLeader.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
          if ((double) currentLayoutTab.Position != 0.0)
            stringBuilder.Append($"-sf-tabstop-pos:{currentLayoutTab.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)}pt;");
        }
        this.CurrentLineWidth += tabWidth;
        string text = currentLayoutTab == null || currentLayoutTab.TabLeader == Syncfusion.Layouting.TabLeader.NoLeader ? this.GetTabLeader(tabWidth, Syncfusion.Layouting.TabLeader.NoLeader, font) : this.GetTabLeader(tabWidth, currentLayoutTab.TabLeader, font);
        this.m_writer.WriteAttributeString("style", stringBuilder.ToString());
        this.WriteText(text);
      }
      else
      {
        string tabText = this.GetTabText();
        this.CurrentLineWidth += this.DrawingContext.MeasureString(tabText, font, (StringFormat) null).Width;
        this.m_writer.WriteAttributeString("style", "font: 7.0pt \"Times New Roman\";-sf-listtab:yes;");
        this.WriteText(tabText);
      }
    }
    else
      this.m_writer.WriteRaw(ControlChar.NonBreakingSpace + ControlChar.Space);
    this.m_writer.WriteEndElement();
  }

  private void WritePrefixListTab(WListLevel level)
  {
    this.m_writer.WriteStartElement("span");
    if (level.FollowCharacter == FollowCharacterType.Tab)
    {
      Font font = level.Document.FontSettings.GetFont("Times New Roman", 7f, FontStyle.Regular);
      TabsLayoutInfo.LayoutTab currentLayoutTab = this.GetCurrentLayoutTab();
      if (currentLayoutTab == null || currentLayoutTab.Justification == Syncfusion.Layouting.TabJustification.Left || currentLayoutTab.Justification == (Syncfusion.Layouting.TabJustification.Right | Syncfusion.Layouting.TabJustification.Bar))
      {
        float tabWidth = this.GetTabWidth(currentLayoutTab);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("font: 7.0pt \"Times New Roman\";-sf-listtab:yes;");
        if (currentLayoutTab != null)
        {
          stringBuilder.Append($"-sf-tabstop-align:{((TabJustification) currentLayoutTab.Justification).ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
          if (currentLayoutTab.TabLeader != Syncfusion.Layouting.TabLeader.NoLeader)
            stringBuilder.Append($"-sf-tabstop-leader:{currentLayoutTab.TabLeader.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};");
          if ((double) currentLayoutTab.Position != 0.0)
            stringBuilder.Append($"-sf-tabstop-pos:{currentLayoutTab.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)}pt;");
        }
        this.CurrentLineWidth += tabWidth;
        string text = currentLayoutTab == null || currentLayoutTab.TabLeader == Syncfusion.Layouting.TabLeader.NoLeader ? this.GetTabLeader(tabWidth, Syncfusion.Layouting.TabLeader.NoLeader, font) : this.GetTabLeader(tabWidth, currentLayoutTab.TabLeader, font);
        if ((double) tabWidth < 6.0)
          text += ControlChar.NonBreakingSpace;
        else if ((double) tabWidth > 13.0)
          text = text.Substring(0, text.Length - 1);
        this.m_writer.WriteAttributeString("style", stringBuilder.ToString());
        this.WriteText(text);
      }
      else
      {
        string tabText = this.GetTabText();
        this.CurrentLineWidth += this.DrawingContext.MeasureString(tabText, font, (StringFormat) null).Width;
        this.m_writer.WriteAttributeString("style", "font: 7.0pt \"Times New Roman\";-sf-listtab:yes;");
        this.WriteText(tabText);
      }
    }
    else
      this.m_writer.WriteRaw(ControlChar.NonBreakingSpace + ControlChar.Space);
    this.m_writer.WriteEndElement();
  }

  private void WriteSpanText(string text, string style, WTextRange tr)
  {
    if (text.Contains(ControlChar.Space + ControlChar.Space))
    {
      style += "mso-spacerun:yes;";
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < text.Length; ++index)
      {
        if ((int) text[index] == (int) ControlChar.SpaceChar && index != text.Length - 1 && (int) text[index + 1] == (int) ControlChar.SpaceChar)
          stringBuilder.Append(ControlChar.NonBreakingSpaceChar);
        else
          stringBuilder.Append(text[index]);
      }
      text = stringBuilder.ToString();
    }
    if (text.StartsWithExt(ControlChar.Space) && tr.Owner is WParagraph && tr.OwnerParagraph.Items.FirstItem == tr)
    {
      if (!style.Contains("mso-spacerun:yes;"))
        style += "mso-spacerun:yes;";
      text = this.ReplaceEmptySpace(text);
    }
    if (tr.CharacterFormat.CharStyleName != null && tr.CharacterFormat.CharStyleName.Length > 0)
    {
      this.m_writer.WriteAttributeString("class", this.GetClassAttr(tr.Document.Styles.FindByName(tr.CharacterFormat.CharStyleName) as Style));
      if (!string.IsNullOrEmpty(tr.CharacterFormat.CharStyleName))
        style = this.ValidateStyle(tr.CharacterFormat.CharStyleName, style);
    }
    if (style.Length > 0)
      this.m_writer.WriteAttributeString(nameof (style), style);
    bool flag = tr.CharacterFormat.CharStyle != null && tr.CharacterFormat.CharStyle.Name == "Footnote Reference";
    if (tr.CharacterFormat.SubSuperScript == SubSuperScript.SubScript)
      this.m_writer.WriteStartElement("sub");
    else if (tr.CharacterFormat.SubSuperScript == SubSuperScript.SuperScript && !flag)
      this.m_writer.WriteStartElement("sup");
    this.WriteText(text);
    if (tr.CharacterFormat.SubSuperScript != SubSuperScript.SubScript && (tr.CharacterFormat.SubSuperScript != SubSuperScript.SuperScript || flag))
      return;
    this.m_writer.WriteEndElement();
  }

  private void WriteTable(WTable table, bool isTableCreatedFromTextBox)
  {
    this.ApplyTableGridStyle(table, isTableCreatedFromTextBox);
    table.ApplyBaseStyleFormats();
    table.UpdateGridSpan();
    if (table.Rows.Count == 0)
      return;
    if (this.listStack.Count > 0)
      this.WriteEndElement(this.listStack.Count);
    List<float> offsets = this.CalculateOffsets(table);
    this.m_writer.WriteStartElement("div");
    if (table.TableFormat.Bidi)
      this.m_writer.WriteAttributeString("dir", "RTL");
    if (table.TableFormat.HasValue(121) && table.TableFormat.Hidden)
      this.m_writer.WriteAttributeString("style", "display:none");
    this.m_writer.WriteStartElement(nameof (table));
    this.WriteTableAttributes(table, isTableCreatedFromTextBox);
    int index1 = 0;
    for (int count1 = table.Rows.Count; index1 < count1; ++index1)
    {
      WTableRow row = table.Rows[index1];
      this.m_writer.WriteStartElement("tr");
      float rowOffset = 0.0f;
      this.WriteRowAttributes(row);
      short gridBefore = row.RowFormat.GridBefore;
      if (gridBefore > (short) 0)
        this.WriteGridCell((int) gridBefore, row.RowFormat.GridBeforeWidth);
      int index2 = 0;
      for (int count2 = row.Cells.Count; index2 < count2; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        if (cell.CellFormat.VerticalMerge == CellMerge.Continue || cell.CellFormat.HorizontalMerge == CellMerge.Continue)
        {
          rowOffset += (float) Math.Round((double) cell.Width, 2);
        }
        else
        {
          if (row.IsHeader)
            this.m_writer.WriteStartElement("th");
          else
            this.m_writer.WriteStartElement("td");
          float num = this.WriteCellAttributes(cell);
          string style = this.GetStyle(cell.CellFormat);
          string str = !cell.CellFormat.SamePaddingsAsTable ? style + this.GetPaddings(cell.CellFormat.Paddings) : style + this.GetPaddings(cell);
          if ((double) num > 0.0)
            str = $"{str}width:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}px;";
          if (str.Length > 0)
            this.m_writer.WriteAttributeString("style", str);
          this.WriteSpanAttributes(offsets, rowOffset, cell);
          if (cell.Items.Count == 0)
          {
            this.m_writer.WriteStartElement("p");
            this.WriteEmptyPara(cell.CharacterFormat);
            this.m_writer.WriteEndElement();
          }
          this.WriteTextBody((WTextBody) cell);
          if (this.listStack.Count > 0)
            this.WriteEndElement(this.listStack.Count);
          this.m_writer.WriteEndElement();
          rowOffset = (float) Math.Round((double) rowOffset + (double) cell.Width, 2);
        }
      }
      short gridAfter = row.RowFormat.GridAfter;
      if (gridAfter > (short) 0)
        this.WriteGridCell((int) gridAfter, row.RowFormat.GridAfterWidth);
      this.m_writer.WriteEndElement();
    }
    if (table.Document.IsDOCX() && this.CheckTableContainsMisalignedColumns(table, offsets))
      this.WriteOffsetsRow(offsets);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private bool CheckTableContainsMisalignedColumns(WTable table, List<float> colOffsets)
  {
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      WTableRow row = table.Rows[index1];
      if (colOffsets.Count != row.Cells.Count)
        return true;
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        if (index2 == 0)
        {
          if ((double) cell.Width != Math.Round((double) colOffsets[index2], 2))
            return true;
        }
        else if ((double) cell.Width != Math.Round((double) colOffsets[index2] - (double) colOffsets[index2 - 1], 2))
          return true;
      }
    }
    return false;
  }

  private void WriteGridCell(int gridCount, PreferredWidthInfo gridWidth)
  {
    this.m_writer.WriteStartElement("td");
    this.m_writer.WriteAttributeString("style", "border:none;");
    if (gridWidth.WidthType == FtsWidth.Percentage)
      this.m_writer.WriteAttributeString("width", gridWidth.Width.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%");
    else if (gridWidth.WidthType == FtsWidth.Point)
      this.m_writer.WriteAttributeString("width", gridWidth.Width.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "pt");
    if (gridCount > 1)
      this.m_writer.WriteAttributeString("colspan", gridCount.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    this.m_writer.WriteRaw("&nbsp;");
    this.m_writer.WriteEndElement();
  }

  private void ApplyTableGridStyle(WTable table, bool isTableCreatedFromTextBox)
  {
    if (isTableCreatedFromTextBox || table.StyleName != null && !(table.StyleName == string.Empty))
      return;
    switch (table.Document.ActualFormatType)
    {
      case FormatType.Doc:
      case FormatType.Docx:
      case FormatType.Word2007:
      case FormatType.Word2010:
        if (table.DocxTableFormat.HasFormat)
          break;
        table.ApplyStyle(BuiltinTableStyle.TableGrid, false);
        break;
    }
  }

  private Border GetBottomBorderOfVerticallyMergedCell(WTableCell cell)
  {
    Border bottom = cell.CellFormat.Borders.Bottom;
    WTableRow ownerRow = cell.OwnerRow;
    WTable ownerTable = ownerRow.OwnerTable;
    int num = ownerTable.Rows.IndexOf((IEntity) ownerRow);
    int index1 = ownerRow.Cells.IndexOf((IEntity) cell);
    for (int index2 = num; index2 < ownerTable.Rows.Count; ++index2)
    {
      if (index1 < ownerTable.Rows[index2].Cells.Count && ownerTable.Rows[index2].Cells[index1].CellFormat.VerticalMerge == CellMerge.Continue)
        bottom = ownerTable.Rows[index2].Cells[index1].CellFormat.Borders.Bottom;
    }
    return bottom;
  }

  private Border GetRightBorderOfHorizontallyMergedCell(WTableCell cell)
  {
    Border right = cell.CellFormat.Borders.Right;
    WTableRow ownerRow = cell.OwnerRow;
    for (int index = ownerRow.Cells.IndexOf((IEntity) cell); index < ownerRow.Cells.Count; ++index)
    {
      if (ownerRow.Cells[index].CellFormat.HorizontalMerge == CellMerge.Continue)
        right = ownerRow.Cells[index].CellFormat.Borders.Right;
    }
    return right;
  }

  private void WriteOffsetsRow(List<float> offsets)
  {
    if (offsets.Count == 0)
      return;
    this.m_writer.WriteStartElement("tr");
    this.m_writer.WriteAttributeString("style", $"{"height"}:{"0"}px;");
    int index = 0;
    for (int count = offsets.Count; index < count; ++index)
    {
      this.m_writer.WriteStartElement("td");
      this.m_writer.WriteAttributeString("style", $"{"width"}:{UnitsConvertor.Instance.ConvertToPixels(index != 0 ? offsets[index] - offsets[index - 1] : offsets[index], PrintUnits.Point)}px;" + "border:none;padding:0pt;");
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void WriteSpanAttributes(List<float> colOffsets, float rowOffset, WTableCell cell)
  {
    int num;
    if (cell.CellFormat.HorizontalMerge == CellMerge.Start)
    {
      int colspan = this.GetColspan(cell, colOffsets, rowOffset + cell.Width);
      rowOffset = (float) Math.Round((double) rowOffset, 2);
      num = colspan + (this.GetColspan(colOffsets, rowOffset, cell.Width) - 1);
    }
    else
    {
      rowOffset = (float) Math.Round((double) rowOffset, 2);
      num = this.GetColspan(colOffsets, rowOffset, cell.Width);
    }
    if (num > 1)
      this.m_writer.WriteAttributeString("colspan", num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    if (cell.CellFormat.VerticalMerge != CellMerge.Start)
      return;
    this.m_writer.WriteAttributeString("rowspan", this.GetRowspan(cell, rowOffset).ToString((IFormatProvider) CultureInfo.InvariantCulture));
  }

  private int GetRowspan(WTableCell cell, float rowOffset)
  {
    int rowspan = 1;
    WTableRow ownerRow = cell.OwnerRow;
    WTable ownerTable = ownerRow.OwnerTable;
    int rowIndex = ownerRow.GetRowIndex();
    if (ownerRow.RowFormat.GridBefore > (short) 0)
    {
      if (ownerRow.RowFormat.GridBeforeWidth.WidthType == FtsWidth.Point)
        rowOffset += ownerRow.RowFormat.GridBeforeWidth.Width;
      else if (ownerRow.RowFormat.GridBeforeWidth.WidthType == FtsWidth.Percentage)
      {
        float num = (cell.OwnerRow.OwnerTable.OwnerTextBody.Owner as WSection).PageSetup.ClientWidth * (ownerRow.RowFormat.GridBeforeWidth.Width / 100f);
        rowOffset += num;
      }
    }
    int index = rowIndex + 1;
    for (int count = ownerTable.Rows.Count; index < count; ++index)
    {
      WTableCell cellByOffset = this.GetCellByOffset(ownerTable.Rows[index], rowOffset);
      if (cellByOffset != null && (double) cell.Width == (double) cellByOffset.Width && cellByOffset.CellFormat.VerticalMerge == CellMerge.Continue)
        ++rowspan;
      else
        break;
    }
    return rowspan;
  }

  private WTableCell GetCellByOffset(WTableRow row, float rowOffset)
  {
    float num = 0.0f;
    if (row.RowFormat.GridBefore > (short) 0)
    {
      if (row.RowFormat.GridBeforeWidth.WidthType == FtsWidth.Point)
        num = row.RowFormat.GridBeforeWidth.Width;
      else if (row.RowFormat.GridBeforeWidth.WidthType == FtsWidth.Percentage)
        num = (row.OwnerTable.OwnerTextBody.Owner as WSection).PageSetup.ClientWidth * (row.RowFormat.GridBeforeWidth.Width / 100f);
    }
    int index = 0;
    for (int count = row.Cells.Count; index < count; ++index)
    {
      if (Math.Round((double) num, 2) == (double) rowOffset)
        return row.Cells[index];
      num += (float) Math.Round((double) row.Cells[index].Width, 2);
    }
    return (WTableCell) null;
  }

  private List<float> CalculateOffsets(WTable table)
  {
    List<float> offsets = new List<float>();
    int index1 = 0;
    for (int count1 = table.Rows.Count; index1 < count1; ++index1)
    {
      WTableRow row = table.Rows[index1];
      float num = 0.0f;
      int index2 = 0;
      for (int count2 = row.Cells.Count; index2 < count2; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        num = (float) Math.Round((double) num + (double) cell.Width, 2);
        if (!offsets.Contains(num))
          offsets.Add(num);
      }
    }
    offsets.Sort();
    return offsets;
  }

  private int GetColspan(List<float> colOffsets, float startOffset, float colWidth)
  {
    int num1 = colOffsets.IndexOf(startOffset);
    if (num1 < 0 && (double) startOffset > 0.0)
      return 1;
    int colspan = 1;
    float num2 = startOffset + colWidth;
    if (colOffsets.Count > num1 + colspan)
    {
      while ((double) num2 - (double) colOffsets[num1 + colspan] > 0.0099999997764825821)
        ++colspan;
    }
    return colspan;
  }

  private int GetColspan(WTableCell cell, List<float> colOffsets, float startOffset)
  {
    int colspan = 1;
    int inOwnerCollection = cell.GetIndexInOwnerCollection();
    WTableRow ownerRow = cell.OwnerRow;
    int index = inOwnerCollection + 1;
    for (int count = ownerRow.Cells.Count; index < count && ownerRow.Cells[index].CellFormat.HorizontalMerge == CellMerge.Continue; ++index)
    {
      startOffset = (float) Math.Round((double) startOffset, 2);
      colspan += this.GetColspan(colOffsets, startOffset, ownerRow.Cells[index].Width);
      startOffset += ownerRow.Cells[index].Width;
    }
    return colspan;
  }

  private float WriteCellAttributes(WTableCell cell)
  {
    WTableRow ownerRow = cell.OwnerRow;
    int num1 = ownerRow.Cells.IndexOf(cell);
    float num2 = cell.Width;
    for (int index = num1 + 1; index < ownerRow.Cells.Count && ownerRow.Cells[index].CellFormat.HorizontalMerge == CellMerge.Continue; ++index)
      num2 += ownerRow.Cells[index].Width;
    if ((double) num2 > 0.0)
      num2 = UnitsConvertor.Instance.ConvertToPixels(num2, PrintUnits.Point);
    return num2;
  }

  private void WriteTableAttributes(WTable table, bool isTableCreatedFromTextBox)
  {
    StringBuilder sb = new StringBuilder();
    if ((double) table.TableFormat.CellSpacing >= 0.0)
    {
      string bordersStyle = this.GetBordersStyle(table.TableFormat.Borders, sb);
      if (this.IsBorderAttributeNeedToPreserve(table, bordersStyle))
        this.m_writer.WriteAttributeString("border", "1");
    }
    if (!string.IsNullOrEmpty(table.Title))
      this.m_writer.WriteAttributeString("title", table.Title);
    if (table.TableFormat.HasValue(108) && table.TableFormat.BackColor != Color.Empty)
      sb.Append($"background-color:{this.GetColor(table.TableFormat.BackColor)};");
    if (isTableCreatedFromTextBox && table.TableFormat.ForeColor != Color.Empty)
      sb.Append($"color:{this.GetColor(table.TableFormat.ForeColor)};");
    if ((double) table.IndentFromLeft != 0.0 && table.TableFormat.HorizontalAlignment == RowAlignment.Left)
      sb.Append($"margin-left:{table.IndentFromLeft.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    sb.Append(this.WriteTableWidth(table));
    this.WriteTableAlignment(table, isTableCreatedFromTextBox);
    sb.Append(this.WriteTableCellSpacing(table));
    if ((double) table.TableFormat.CellSpacing >= 0.0)
      this.WriteTableBorder(table, sb);
    if (!(sb.ToString() != string.Empty))
      return;
    this.m_writer.WriteAttributeString("style", sb.ToString());
  }

  private bool IsBorderAttributeNeedToPreserve(WTable table, string borderStyle)
  {
    return borderStyle == string.Empty && !table.TableFormat.Borders.Bottom.HasNoneStyle && !table.TableFormat.Borders.Right.HasNoneStyle && !table.TableFormat.Borders.Left.HasNoneStyle && !table.TableFormat.Borders.Top.HasNoneStyle && table.TableFormat.Borders.Top.BorderType != BorderStyle.Cleared && table.TableFormat.Borders.Left.BorderType != BorderStyle.Cleared && table.TableFormat.Borders.Bottom.BorderType != BorderStyle.Cleared && table.TableFormat.Borders.Right.BorderType != BorderStyle.Cleared;
  }

  private void WriteTableBorder(WTable table, StringBuilder sb)
  {
    this.GetTableborder(table.TableFormat.Borders.Bottom, "bottom", sb);
    this.GetTableborder(table.TableFormat.Borders.Top, "bottom", sb);
    this.GetTableborder(table.TableFormat.Borders.Left, "bottom", sb);
    this.GetTableborder(table.TableFormat.Borders.Right, "bottom", sb);
  }

  private void GetTableborder(Border border, string suffix, StringBuilder sb)
  {
    if (border.BorderType == BorderStyle.Cleared)
      return;
    if (border.BorderType != BorderStyle.None)
      sb.Append($"border-{suffix}-style:{this.ToBorderStyle(border.BorderType)};");
    if ((double) border.LineWidth > 0.0)
      sb.Append($"border-{suffix}-width:{border.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    if (border.Color != Color.Empty)
    {
      sb.Append($"border-{suffix}-color{this.GetColor(border.Color)};");
    }
    else
    {
      if (border.BorderType == BorderStyle.None)
        return;
      sb.Append($"border-{suffix}-color:#000000;");
    }
  }

  private string WriteTableCellSpacing(WTable table)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if ((double) table.TableFormat.CellSpacing > 0.0)
      this.m_writer.WriteAttributeString("cellspacing", UnitsConvertor.Instance.ConvertToPixels(table.TableFormat.CellSpacing * 2f, PrintUnits.Point).ToString((IFormatProvider) CultureInfo.InvariantCulture));
    else
      this.m_writer.WriteAttributeString("cellspacing", "0");
    if ((double) table.TableFormat.CellSpacing <= 0.0)
      stringBuilder.Append("border-collapse: collapse; ");
    return stringBuilder.ToString();
  }

  private string WriteTableWidth(WTable table)
  {
    StringBuilder stringBuilder = new StringBuilder();
    switch (table.PreferredTableWidth.WidthType)
    {
      case FtsWidth.Auto:
        stringBuilder.Append("width: auto; ");
        break;
      case FtsWidth.Percentage:
        stringBuilder.Append($"width: {table.PreferredTableWidth.Width.ToString((IFormatProvider) CultureInfo.InvariantCulture)}%; ");
        break;
      case FtsWidth.Point:
        stringBuilder.Append($"width: {table.PreferredTableWidth.Width.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt; ");
        break;
    }
    return stringBuilder.ToString();
  }

  private void WriteTableAlignment(WTable table, bool isTableCreatedFromTextBox)
  {
    switch (isTableCreatedFromTextBox ? (int) table.TableFormat.HorizontalAlignment : (int) this.GetTableAlignment(table.TableFormat))
    {
      case 1:
        this.m_writer.WriteAttributeString("align", "center");
        break;
      case 2:
        this.m_writer.WriteAttributeString("align", "right");
        break;
    }
  }

  private RowAlignment GetTableAlignment(RowFormat tableFormat)
  {
    if (tableFormat.PropertiesHash.ContainsKey(62))
      return (double) tableFormat.Positioning.HorizPosition == -8.0 ? RowAlignment.Right : RowAlignment.Left;
    if (!tableFormat.PropertiesHash.ContainsKey(105))
      return RowAlignment.Left;
    RowAlignment tableAlignment = (RowAlignment) tableFormat.PropertiesHash[105];
    if (tableFormat.Bidi && (tableFormat.Document.ActualFormatType == FormatType.Doc || tableFormat.Document.ActualFormatType == FormatType.Docx))
    {
      switch (tableAlignment)
      {
        case RowAlignment.Left:
          tableAlignment = RowAlignment.Right;
          break;
        case RowAlignment.Right:
          tableAlignment = RowAlignment.Left;
          break;
      }
    }
    return tableAlignment;
  }

  private void WriteRowAttributes(WTableRow row)
  {
    string str1 = "height: ";
    row.Height = Math.Abs(row.Height);
    string str2 = (double) row.Height <= 0.0 ? str1 + "2px" : $"{str1}{UnitsConvertor.Instance.ConvertToPixels(row.Height, PrintUnits.Point).ToString((IFormatProvider) CultureInfo.InvariantCulture)}px";
    if (row.RowFormat.HasValue(121) && row.RowFormat.Hidden)
      str2 += ";display:none";
    this.m_writer.WriteAttributeString("style", str2);
  }

  private void EnsureImagesFolder()
  {
    if (this.m_bImagesFolderCreated || this.m_cacheFilesInternally)
      return;
    Directory.CreateDirectory($"{this.m_imagesFolder}{this.m_fileNameWithoutExt}_images\\");
    this.m_bImagesFolderCreated = true;
  }

  private string GetImagePath()
  {
    ++this.m_imgCounter;
    string imagePath;
    if (!this.m_cacheFilesInternally)
      imagePath = $"{this.m_fileNameWithoutExt}_images\\{this.m_fileNameWithoutExt}_img{this.m_imgCounter.ToString((IFormatProvider) CultureInfo.InvariantCulture)}";
    else
      imagePath = "images/img" + this.m_imgCounter.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    return imagePath;
  }

  private string GetPaddings(Paddings paddings)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"padding-left:{paddings.Left.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-right:{paddings.Right.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-top:{paddings.Top.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-bottom:{paddings.Bottom.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    return stringBuilder.ToString();
  }

  private string GetPaddings(WTableCell cell)
  {
    StringBuilder stringBuilder = new StringBuilder();
    Paddings paddingBasedOnTable = this.GetCellPaddingBasedOnTable(cell);
    stringBuilder.Append($"padding-left:{paddingBasedOnTable.Left.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-right:{paddingBasedOnTable.Right.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-top:{paddingBasedOnTable.Top.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    stringBuilder.Append($"padding-bottom:{paddingBasedOnTable.Bottom.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    return stringBuilder.ToString();
  }

  private Paddings GetCellPaddingBasedOnTable(WTableCell cell)
  {
    return new Paddings()
    {
      Left = !cell.OwnerRow.RowFormat.Paddings.HasKey(1) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(1) ? (cell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Left) : cell.OwnerRow.RowFormat.Paddings.Left,
      Right = !cell.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(4) ? (cell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Right) : cell.OwnerRow.RowFormat.Paddings.Right,
      Top = !cell.OwnerRow.RowFormat.Paddings.HasKey(2) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(2) ? 0.0f : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Top) : cell.OwnerRow.RowFormat.Paddings.Top,
      Bottom = !cell.OwnerRow.RowFormat.Paddings.HasKey(3) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(3) ? 0.0f : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Bottom) : cell.OwnerRow.RowFormat.Paddings.Bottom
    };
  }

  private string GetStyle(CellFormat format)
  {
    WTableCell ownerBase = format.OwnerBase as WTableCell;
    StringBuilder sb = new StringBuilder();
    sb.Append($"vertical-align:{format.VerticalAlignment.ToString().ToLower(CultureInfo.InvariantCulture)};");
    if (format.ForeColor != Color.Empty || format.TextureStyle != TextureStyle.TextureNone || format.BackColor != Color.Empty)
      sb.Append($"background-color:{this.GetCellBackground(format)};");
    this.GetBordersStyle(format.Borders, format.OwnerRowFormat.Borders, sb, ownerBase);
    return sb.ToString();
  }

  private string GetCellBackground(CellFormat format)
  {
    float percent = this.Build_TextureStyle(format.TextureStyle);
    return this.GetColor(Color.FromArgb(this.GetColorValue((int) format.ForeColor.R, (int) format.BackColor.R, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty), this.GetColorValue((int) format.ForeColor.G, (int) format.BackColor.G, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty), this.GetColorValue((int) format.ForeColor.B, (int) format.BackColor.B, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty)));
  }

  private string GetParagraphBackground(WParagraphFormat format)
  {
    float percent = this.Build_TextureStyle(format.TextureStyle);
    return this.GetColor(Color.FromArgb(this.GetColorValue((int) format.ForeColor.R, (int) format.BackColor.R, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty), this.GetColorValue((int) format.ForeColor.G, (int) format.BackColor.G, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty), this.GetColorValue((int) format.ForeColor.B, (int) format.BackColor.B, percent, format.ForeColor.IsEmpty, format.BackColor.IsEmpty)));
  }

  private int GetColorValue(
    int foreColorValue,
    int backColorValue,
    float percent,
    bool isForeColorEmpty,
    bool isBackColorEmpty)
  {
    return (double) percent != 100.0 ? (!isForeColorEmpty ? (!isBackColorEmpty ? backColorValue + (int) Math.Round((double) foreColorValue * ((double) percent / 100.0)) - (int) Math.Round((double) backColorValue * ((double) percent / 100.0)) : (int) Math.Round((double) foreColorValue * ((double) percent / 100.0))) : (!isBackColorEmpty ? (int) Math.Round((double) backColorValue * (1.0 - (double) percent / 100.0)) : (int) Math.Round((double) byte.MaxValue * (1.0 - (double) percent / 100.0)))) : foreColorValue;
  }

  private void GetBordersStyle(
    Borders cellBorders,
    Borders rowBorders,
    StringBuilder sb,
    WTableCell ownerCell)
  {
    Border rowBorder1 = this.GetRowBorder(rowBorders, ownerCell, "top");
    this.GetBorderStyle(cellBorders.Top, rowBorder1, sb, "top", ownerCell);
    Border rowBorder2 = this.GetRowBorder(rowBorders, ownerCell, "left");
    this.GetBorderStyle(cellBorders.Left, rowBorder2, sb, "left", ownerCell);
    Border rowBorder3 = this.GetRowBorder(rowBorders, ownerCell, "right");
    if (ownerCell.CellFormat.HorizontalMerge == CellMerge.Start)
      this.GetBorderStyle(this.GetRightBorderOfHorizontallyMergedCell(ownerCell), rowBorder3, sb, "right", ownerCell);
    else
      this.GetBorderStyle(cellBorders.Right, rowBorder3, sb, "right", ownerCell);
    Border rowBorder4 = this.GetRowBorder(rowBorders, ownerCell, "bottom");
    if (ownerCell.CellFormat.VerticalMerge == CellMerge.Start)
      this.GetBorderStyle(this.GetBottomBorderOfVerticallyMergedCell(ownerCell), rowBorder4, sb, "bottom", ownerCell);
    else
      this.GetBorderStyle(cellBorders.Bottom, rowBorder4, sb, "bottom", ownerCell);
  }

  private Border GetRowBorder(Borders borders, WTableCell cell, string side)
  {
    switch (side)
    {
      case "top":
        WTableRow ownerRow1 = cell.OwnerRow;
        return ownerRow1 != null && ownerRow1.GetIndexInOwnerCollection() > 0 ? borders.Horizontal : borders.Top;
      case "left":
        return cell.GetIndexInOwnerCollection() > 0 ? borders.Vertical : borders.Left;
      case "right":
        WTableRow ownerRow2 = cell.OwnerRow;
        return ownerRow2 != null && cell.GetIndexInOwnerCollection() == ownerRow2.Cells.Count - 1 ? borders.Right : borders.Vertical;
      case "bottom":
        WTableRow ownerRow3 = cell.OwnerRow;
        WTable ownerTable = ownerRow3.OwnerTable;
        return ownerTable != null && ownerRow3.GetIndexInOwnerCollection() == ownerTable.Rows.Count - 1 ? borders.Bottom : borders.Horizontal;
      default:
        return (Border) null;
    }
  }

  private void GetBorderStyle(
    Border cellBorder,
    Border rowBorder,
    StringBuilder sb,
    string suffix,
    WTableCell cell)
  {
    if (cellBorder.BorderType == BorderStyle.Cleared)
    {
      sb.Append($"border-{suffix}:none;");
    }
    else
    {
      BorderStyle style = cellBorder.BorderType == BorderStyle.None || cellBorder.BorderType == BorderStyle.Cleared ? rowBorder.BorderType : cellBorder.BorderType;
      switch (style)
      {
        case BorderStyle.None:
        case BorderStyle.Cleared:
          if (!cellBorder.HasNoneStyle && !rowBorder.HasNoneStyle)
          {
            this.GetCellborderStyleBasedOnTableBorder(cell, suffix, sb);
            break;
          }
          break;
        default:
          sb.Append($"border-{suffix}-style:{this.ToBorderStyle(style)};");
          break;
      }
      Color color = cellBorder.Color != Color.Empty ? cellBorder.Color : rowBorder.Color;
      if (color != Color.Empty)
        sb.Append($"border-{suffix}-color:{this.GetColor(color)};");
      else if (!cellBorder.HasNoneStyle && !rowBorder.HasNoneStyle)
        this.GetCellBorderColorBasedOnTableBorder(cell, suffix, sb);
      float num = 0.0f;
      if ((double) cellBorder.LineWidth > 0.0)
        num = this.GetLineWidthBasedOnBorderStyle(cellBorder);
      else if ((double) rowBorder.LineWidth > 0.0)
        num = this.GetLineWidthBasedOnBorderStyle(rowBorder);
      if ((double) num > 0.0)
      {
        sb.Append($"border-{suffix}-width:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      }
      else
      {
        if (cellBorder.HasNoneStyle || rowBorder.HasNoneStyle)
          return;
        this.GetCellborderWidthBasedOnTableBorder(cell, suffix, sb);
      }
    }
  }

  private void GetCellborderStyleBasedOnTableBorder(
    WTableCell cell,
    string suffix,
    StringBuilder sb)
  {
    WTableRow ownerRow = cell.OwnerRow;
    WTable ownerTable = ownerRow.OwnerTable;
    int inOwnerCollection1 = cell.GetIndexInOwnerCollection();
    int inOwnerCollection2 = ownerRow.GetIndexInOwnerCollection();
    switch (suffix)
    {
      case "top":
        if (inOwnerCollection2 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if (ownerTable.TableFormat.Borders.Top.BorderType == BorderStyle.None)
            break;
          sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Top.BorderType)};");
          break;
        }
        if (ownerTable.TableFormat.Borders.Horizontal.BorderType == BorderStyle.None)
          break;
        sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Horizontal.BorderType)};");
        break;
      case "bottom":
        if (inOwnerCollection2 == ownerTable.Rows.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if (ownerTable.TableFormat.Borders.Bottom.BorderType == BorderStyle.None)
            break;
          sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Bottom.BorderType)};");
          break;
        }
        if (ownerTable.TableFormat.Borders.Horizontal.BorderType == BorderStyle.None)
          break;
        sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Horizontal.BorderType)};");
        break;
      case "left":
        if (inOwnerCollection1 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if (ownerTable.TableFormat.Borders.Left.BorderType == BorderStyle.None)
            break;
          sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Left.BorderType)};");
          break;
        }
        if (ownerTable.TableFormat.Borders.Vertical.BorderType == BorderStyle.None)
          break;
        sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Vertical.BorderType)};");
        break;
      case "right":
        if (inOwnerCollection1 == ownerRow.Cells.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if (ownerTable.TableFormat.Borders.Right.BorderType == BorderStyle.None)
            break;
          sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Right.BorderType)};");
          break;
        }
        if (ownerTable.TableFormat.Borders.Vertical.BorderType == BorderStyle.None)
          break;
        sb.Append($"border-{suffix}-style:{this.ToBorderStyle(ownerTable.TableFormat.Borders.Vertical.BorderType)};");
        break;
    }
  }

  private void GetCellBorderColorBasedOnTableBorder(
    WTableCell cell,
    string suffix,
    StringBuilder sb)
  {
    WTableRow ownerRow = cell.OwnerRow;
    WTable ownerTable = ownerRow.OwnerTable;
    int inOwnerCollection1 = cell.GetIndexInOwnerCollection();
    int inOwnerCollection2 = ownerRow.GetIndexInOwnerCollection();
    switch (suffix)
    {
      case "top":
        if (inOwnerCollection2 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          this.GetBorderColor(ownerTable.TableFormat.Borders.Top, sb, suffix);
          break;
        }
        this.GetBorderColor(ownerTable.TableFormat.Borders.Horizontal, sb, suffix);
        break;
      case "bottom":
        if (inOwnerCollection2 == ownerTable.Rows.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          this.GetBorderColor(ownerTable.TableFormat.Borders.Bottom, sb, suffix);
          break;
        }
        this.GetBorderColor(ownerTable.TableFormat.Borders.Horizontal, sb, suffix);
        break;
      case "left":
        if (inOwnerCollection1 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          this.GetBorderColor(ownerTable.TableFormat.Borders.Left, sb, suffix);
          break;
        }
        this.GetBorderColor(ownerTable.TableFormat.Borders.Vertical, sb, suffix);
        break;
      case "right":
        if (inOwnerCollection1 == ownerRow.Cells.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          this.GetBorderColor(ownerTable.TableFormat.Borders.Right, sb, suffix);
          break;
        }
        this.GetBorderColor(ownerTable.TableFormat.Borders.Vertical, sb, suffix);
        break;
    }
  }

  private void GetBorderColor(Border border, StringBuilder sb, string suffix)
  {
    if (border.Color != Color.Empty)
    {
      sb.Append($"border-{suffix}-color:{this.GetColor(border.Color)};");
    }
    else
    {
      if (border.BorderType == BorderStyle.None)
        return;
      sb.Append($"border-{suffix}-color:#000000;");
    }
  }

  private void GetCellborderWidthBasedOnTableBorder(
    WTableCell cell,
    string suffix,
    StringBuilder sb)
  {
    WTableRow ownerRow = cell.OwnerRow;
    WTable ownerTable = ownerRow.OwnerTable;
    int inOwnerCollection1 = cell.GetIndexInOwnerCollection();
    int inOwnerCollection2 = ownerRow.GetIndexInOwnerCollection();
    switch (suffix)
    {
      case "top":
        if (inOwnerCollection2 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if ((double) ownerTable.TableFormat.Borders.Top.LineWidth <= 0.0)
            break;
          sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Top).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
          break;
        }
        if ((double) ownerTable.TableFormat.Borders.Horizontal.LineWidth <= 0.0)
          break;
        sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Horizontal).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        break;
      case "bottom":
        if (inOwnerCollection2 == ownerTable.Rows.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if ((double) ownerTable.TableFormat.Borders.Bottom.LineWidth <= 0.0)
            break;
          sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Bottom).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
          break;
        }
        if ((double) ownerTable.TableFormat.Borders.Horizontal.LineWidth <= 0.0)
          break;
        sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Horizontal).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        break;
      case "left":
        if (inOwnerCollection1 == 0 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if ((double) ownerTable.TableFormat.Borders.Left.LineWidth <= 0.0)
            break;
          sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Left).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
          break;
        }
        if ((double) ownerTable.TableFormat.Borders.Vertical.LineWidth <= 0.0)
          break;
        sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Vertical).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        break;
      case "right":
        if (inOwnerCollection1 == ownerRow.Cells.Count - 1 && (double) ownerTable.TableFormat.CellSpacing <= 0.0)
        {
          if ((double) ownerTable.TableFormat.Borders.Right.LineWidth <= 0.0)
            break;
          sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Right).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
          break;
        }
        if ((double) ownerTable.TableFormat.Borders.Vertical.LineWidth <= 0.0)
          break;
        sb.Append($"border-{suffix}-width:{this.GetLineWidthBasedOnBorderStyle(ownerTable.TableFormat.Borders.Vertical).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        break;
    }
  }

  private float GetLineWidthBasedOnBorderStyle(Border border)
  {
    switch (border.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.DoubleWave:
        return (double) border.LineWidth <= 0.5 ? 1.5f : border.LineWidth * 3f;
      case BorderStyle.Triple:
        return border.LineWidth * 5f;
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
        return border.LineWidth + 1.5f;
      case BorderStyle.ThinThickThinSmallGap:
        return border.LineWidth + 3f;
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
        return border.LineWidth * 2f;
      case BorderStyle.ThickThickThinMediumGap:
        return border.LineWidth * 3f;
      case BorderStyle.ThinThickLargeGap:
        return border.LineWidth + 2.25f;
      case BorderStyle.ThickThinLargeGap:
      case BorderStyle.ThinThickThinLargeGap:
        return (float) ((double) border.LineWidth * 2.0 + 3.0);
      default:
        return (double) border.LineWidth < 1.0 ? 1f : border.LineWidth;
    }
  }

  private string GetStyle(
    WParagraphFormat format,
    bool isListLevel,
    bool isListAsPara,
    WListFormat listFormat)
  {
    StringBuilder sb = new StringBuilder();
    string str1 = "left";
    switch (format.HorizontalAlignment)
    {
      case HorizontalAlignment.Center:
        str1 = "center";
        break;
      case HorizontalAlignment.Right:
        str1 = "right";
        break;
      case HorizontalAlignment.Justify:
      case HorizontalAlignment.Distribute:
      case HorizontalAlignment.JustifyMedium:
      case HorizontalAlignment.JustifyHigh:
      case HorizontalAlignment.JustifyLow:
      case HorizontalAlignment.ThaiJustify:
        str1 = "justify";
        break;
      case HorizontalAlignment.Right | HorizontalAlignment.Distribute:
        if (format.Bidi)
        {
          str1 = "right";
          break;
        }
        break;
    }
    sb.Append($"text-align:{str1};");
    if (format.Keep)
      sb.Append("page-break-inside:avoid;");
    else
      sb.Append("page-break-inside:auto;");
    if (format.KeepFollow)
      sb.Append("page-break-after:avoid;");
    else
      sb.Append("page-break-after:auto;");
    if (format.PageBreakBefore)
      sb.Append("page-break-before:always;");
    else
      sb.Append("page-break-before:avoid;");
    float lineSpacing = format.LineSpacing;
    LineSpacingRule lineSpacingRule = format.LineSpacingRule;
    if ((double) Math.Abs(lineSpacing) > 0.0 && this.CheckParentFormat(format))
    {
      if ((double) Math.Abs(lineSpacing) == 12.0 && lineSpacingRule == LineSpacingRule.Multiple)
        sb.Append("line-height:normal;");
      else if (lineSpacingRule == LineSpacingRule.Multiple)
        sb.Append($"line-height:{((float) ((double) Math.Abs(lineSpacing) / 12.0 * 100.0)).ToString((IFormatProvider) CultureInfo.InvariantCulture)}%;");
      else if (lineSpacingRule != LineSpacingRule.AtLeast || (double) Math.Abs(lineSpacing) > 10.0)
        sb.Append($"line-height:{Math.Abs(lineSpacing).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    if (!format.ContextualSpacing || !this.ContextualSpacingChecking() || !this.m_document.SaveOptions.UseContextualSpacing)
    {
      if (format.SpaceBeforeAuto)
      {
        sb.Append("-sf-before-space-auto:yes;");
        if (!this.m_document.Settings.CompatibilityOptions[CompatibilityOption.DontUseHTMLParagraphAutoSpacing])
          sb.Append($"margin-top:{XmlConvert.ToString(14)}pt;");
        else
          sb.Append($"margin-top:{format.BeforeSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      }
      else
        sb.Append($"margin-top:{format.BeforeSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      if (format.SpaceAfterAuto)
      {
        sb.Append("-sf-after-space-auto:yes;");
        if (!this.m_document.Settings.CompatibilityOptions[CompatibilityOption.DontUseHTMLParagraphAutoSpacing])
          sb.Append($"margin-bottom:{XmlConvert.ToString(14)}pt;");
        else
          sb.Append($"margin-bottom:{format.AfterSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      }
      else
        sb.Append($"margin-bottom:{format.AfterSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    else
    {
      sb.Append("margin-top:0pt;");
      sb.Append("margin-bottom:0pt;");
    }
    if (!format.WordWrap)
      sb.Append("word-break:break-all;");
    WListLevel level = (WListLevel) null;
    if (format.OwnerBase is WParagraph ownerBase && listFormat != null)
      level = ownerBase.GetListLevel(listFormat);
    if (isListLevel && !isListAsPara)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (ownerBase != null)
      {
        WParagraphStyle paraStyle = ownerBase.ParaStyle as WParagraphStyle;
        float[] andFirstLineIndent = ownerBase.GetLeftRightMargindAndFirstLineIndent(listFormat, level, paraStyle);
        num1 = andFirstLineIndent[0];
        if (format.Bidi && (format.Document.ActualFormatType == FormatType.Doc || format.Document.ActualFormatType == FormatType.Docx))
          num1 = andFirstLineIndent[1];
        num2 = andFirstLineIndent[2];
      }
      float num3 = 0.0f;
      this.m_currListCharFormat = new WCharacterFormat((IWordDocument) ownerBase.Document);
      this.m_currListCharFormat.ImportContainer((FormatBase) ownerBase.BreakCharacterFormat);
      this.m_currListCharFormat.CopyProperties((FormatBase) ownerBase.BreakCharacterFormat);
      this.m_currListCharFormat.ApplyBase(ownerBase.BreakCharacterFormat.BaseFormat);
      if (this.m_currListCharFormat.PropertiesHash.ContainsKey(7))
      {
        this.m_currListCharFormat.UnderlineStyle = UnderlineStyle.None;
        this.m_currListCharFormat.PropertiesHash.Remove(7);
      }
      if (level != null)
        this.CopyCharacterFormatting(level.CharacterFormat, this.m_currListCharFormat);
      if (!WordDocument.EnablePartialTrustCode)
      {
        if (this.m_bIsPrefixedList)
        {
          num3 = this.DrawingContext.MeasureString(this.m_prefixedValue, this.m_currListCharFormat.Font, (StringFormat) null).Width;
        }
        else
        {
          string str2 = ownerBase == null || level == null ? string.Empty : ownerBase.Document.UpdateListValue(ownerBase, listFormat, level);
          num3 = this.DrawingContext.MeasureString((double) num2 >= 0.0 ? str2 : ControlChar.NonBreakingSpace + str2 + ControlChar.NonBreakingSpace, this.m_currListCharFormat.Font, (StringFormat) null).Width;
        }
      }
      if ((double) num2 >= 0.0 || this.m_bIsPrefixedList)
      {
        if (!this.m_bIsPrefixedList)
          sb.Append("list-style-position:inside;");
        sb.Append($"margin-left:{num1.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        sb.Append($"text-indent:{num2.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        if (!WordDocument.EnablePartialTrustCode)
        {
          this.IsNeedToWriteTabList = true;
          this.CurrentLineWidth += num1 + num2 + num3;
          if ((double) this.CurrentLineWidth < 0.0)
            this.CurrentLineWidth = 0.0f;
        }
      }
      else
      {
        if (!WordDocument.EnablePartialTrustCode)
        {
          num1 = num1 + num2 + num3;
          num2 = Math.Abs(num2) - num3;
          sb.Append($"-sf-number-width:{(object) num3}pt;");
        }
        sb.Append($"margin-left:{num1.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        sb.Append($"padding-left:{num2.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
        sb.Append("text-indent:0pt;");
        if (!WordDocument.EnablePartialTrustCode)
        {
          this.CurrentLineWidth += (double) num2 >= 0.0 || (double) num1 != 0.0 || ownerBase == null || ownerBase.ParagraphFormat.HasValue(2) ? num1 + num2 : Math.Abs(num2);
          if ((double) this.CurrentLineWidth < 0.0)
            this.CurrentLineWidth = 0.0f;
        }
      }
    }
    else if (isListLevel)
    {
      float num4 = format.LeftIndent;
      if (format.Bidi && (format.Document.ActualFormatType == FormatType.Doc || format.Document.ActualFormatType == FormatType.Docx))
        num4 = format.RightIndent;
      float num5 = num4 + format.FirstLineIndent;
      float num6 = 0.0f;
      if (level != null)
      {
        num5 = level.TextPosition;
        num6 = level.NumberPosition;
      }
      sb.Append($"margin-left:{num5.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      sb.Append($"text-indent:{num6.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    else
    {
      if (ownerBase != null && this.m_document.Settings.IsOptimizedForBrowser)
      {
        float num = format.LeftIndent;
        float firstLineIndent = format.FirstLineIndent;
        if (format.Bidi && (format.Document.ActualFormatType == FormatType.Doc || format.Document.ActualFormatType == FormatType.Docx))
          num = format.RightIndent;
        if ((double) num < 0.0)
          num = 0.0f;
        if ((double) firstLineIndent < 0.0)
          num += (double) num + (double) firstLineIndent < 0.0 ? Math.Abs(num + firstLineIndent) : 0.0f;
        if ((double) num > 0.0 || format.HasValue(2))
          sb.Append($"margin-left:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      }
      else if (format.HasValueWithParent(2))
      {
        float num = format.LeftIndent;
        if (format.Bidi && (format.Document.ActualFormatType == FormatType.Doc || format.Document.ActualFormatType == FormatType.Docx))
          num = format.RightIndent;
        sb.Append($"margin-left:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
      }
      if (format.HasValueWithParent(5))
        sb.Append($"text-indent:{format.FirstLineIndent.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    if (format.HasValueWithParent(3))
    {
      float num = format.RightIndent;
      if (format.Bidi && (format.Document.ActualFormatType == FormatType.Doc || format.Document.ActualFormatType == FormatType.Docx))
        num = format.LeftIndent;
      sb.Append($"margin-right:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    if (format.TextureStyle != TextureStyle.TextureNone)
      sb.Append($"background-color:{this.GetParagraphBackground(format)};");
    else if (format.BackColor != Color.Empty)
      sb.Append($"background-color:{this.GetColor(format.BackColor)};");
    return this.GetBordersStyle(format.Borders, sb);
  }

  private bool ContextualSpacingChecking()
  {
    return this.m_currPara != null && this.m_currPara.NextSibling != null && (this.m_currPara.NextSibling is WParagraph && this.m_currPara.StyleName == (this.m_currPara.NextSibling as WParagraph).StyleName || this.m_currPara.NextSibling is WTable && this.m_currPara.StyleName == this.GetParagraphFromTable(this.m_currPara.NextSibling as WTable).StyleName);
  }

  private WParagraph GetParagraphFromTable(WTable table)
  {
    WParagraph paragraphFromTable = (WParagraph) null;
    if (table.Rows[0] != null)
    {
      WTableCell cell = table.Rows[0].Cells[0];
      if (cell.Items[0] is WParagraph)
        paragraphFromTable = cell.Items[0] as WParagraph;
      else if (cell.Items[0] is WTable)
        paragraphFromTable = this.GetParagraphFromTable(cell.Items[0] as WTable);
    }
    return paragraphFromTable;
  }

  private string GetStyle(WCharacterFormat format) => this.GetStyle(format, true);

  private string GetStyle(WCharacterFormat format, bool style)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if ((double) format.CharacterSpacing != 0.0)
      stringBuilder.Append($"letter-spacing:{(object) format.CharacterSpacing}pt;");
    if (format.TextColor != Color.Empty)
      stringBuilder.Append($"color:{this.GetColor(format.TextColor)};");
    stringBuilder.Append($"font-family:{format.FontName};");
    if ((double) format.FontSize > 0.0)
      stringBuilder.Append($"font-size:{format.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    if (!format.HighlightColor.IsEmpty)
      stringBuilder.Append($"background-color:{this.GetColor(format.HighlightColor)};");
    else if (!format.TextBackgroundColor.IsEmpty)
      stringBuilder.Append($"background-color:{this.GetColor(format.TextBackgroundColor)};");
    stringBuilder.Append(format.AllCaps ? "text-transform:uppercase;" : "text-transform:none;");
    if (format.Bold || format.Emboss || format.OutLine)
      stringBuilder.Append("font-weight:bold;");
    else
      stringBuilder.Append("font-weight:normal;");
    if (format.Hidden)
      stringBuilder.Append("display:none;");
    stringBuilder.Append(format.Italic ? "font-style:italic;" : "font-style:normal;");
    stringBuilder.Append(format.SmallCaps ? "font-variant:small-caps;" : "font-variant:normal;");
    if (format.SubSuperScript == SubSuperScript.SubScript && style)
      stringBuilder.Append("vertical-align:sub;");
    else if (format.SubSuperScript == SubSuperScript.SuperScript && style)
      stringBuilder.Append("vertical-align:super;");
    if (this.m_currPara == null || this.m_currPara != null && this.m_currPara.Text != "")
    {
      if (format.UnderlineStyle != UnderlineStyle.None)
        stringBuilder.Append("text-decoration: underline;");
      else if (format.UnderlineStyle == UnderlineStyle.None)
        stringBuilder.Append("text-decoration: none;");
      if (format.DoubleStrike || format.Strikeout)
        stringBuilder.Append("text-decoration: line-through;");
    }
    if (this.m_currPara != null && (double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) > 0.0)
    {
      if ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) != 12.0 && this.m_currPara.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple)
        stringBuilder.Append($"line-height:{((float) ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) / 12.0 * 100.0)).ToString((IFormatProvider) CultureInfo.InvariantCulture)}%;");
      else if ((double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) != 12.0 && (this.m_currPara.ParagraphFormat.LineSpacingRule != LineSpacingRule.AtLeast || (double) Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing) > 10.0))
        stringBuilder.Append($"line-height:{Math.Abs(this.m_currPara.ParagraphFormat.LineSpacing).ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    return stringBuilder.ToString();
  }

  private void SetCheckBoxSize(string style, WCheckBox checkBox, ref float checkBoxSize)
  {
    style = $"{style}-sf-size-type:{checkBox.SizeType.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)};";
    checkBoxSize = checkBox.SizeType != CheckBoxSizeType.Auto ? (float) checkBox.CheckBoxSize : checkBox.CharacterFormat.FontSize;
    if (!WordDocument.EnablePartialTrustCode)
      checkBoxSize = checkBox.GetCheckBoxSize(DocumentLayouter.DrawingContext);
    style = $"{style}width:{checkBoxSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;";
    style = $"{style}height:{checkBoxSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;";
    if (style.Length <= 0)
      return;
    this.m_writer.WriteAttributeString(nameof (style), style);
  }

  private string GetColor(Color color)
  {
    return "#" + (color.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6");
  }

  private string GetBordersStyle(Borders borders, StringBuilder sb)
  {
    WParagraphFormat wparagraphFormat1 = (WParagraphFormat) null;
    WParagraphFormat wparagraphFormat2 = (WParagraphFormat) null;
    if (borders.ParentFormat is WParagraphFormat && (borders.ParentFormat as WParagraphFormat).OwnerBase is WParagraph ownerBase)
    {
      if (ownerBase.PreviousSibling is WParagraph)
        wparagraphFormat1 = (ownerBase.PreviousSibling as WParagraph).ParagraphFormat;
      if (ownerBase.NextSibling is WParagraph)
        wparagraphFormat2 = (ownerBase.NextSibling as WParagraph).ParagraphFormat;
    }
    if (wparagraphFormat1 == null || wparagraphFormat1.Borders.Top.BorderType != borders.Top.BorderType || (double) wparagraphFormat1.Borders.Top.LineWidth != (double) borders.Top.LineWidth)
      this.GetBorderStyle("top", borders.Top, sb);
    this.GetBorderStyle("left", borders.Left, sb);
    this.GetBorderStyle("right", borders.Right, sb);
    if (wparagraphFormat2 == null || wparagraphFormat2.Borders.Bottom.BorderType != borders.Bottom.BorderType || (double) wparagraphFormat2.Borders.Bottom.LineWidth != (double) borders.Bottom.LineWidth)
      this.GetBorderStyle("bottom", borders.Bottom, sb);
    return sb.ToString();
  }

  private void GetBorderStyle(string suffix, Border border, StringBuilder sb)
  {
    if (border.BorderType == BorderStyle.Cleared)
      return;
    if (border.Color != Color.Empty)
      sb.Append($"border-{suffix}-color:{this.GetColor(border.Color)};");
    else if (border.BorderType != BorderStyle.None)
      sb.Append($"border-{suffix}-color:#000000;");
    if (border.BorderType != BorderStyle.None || border.HasNoneStyle)
      sb.Append($"border-{suffix}-style:{this.ToParagraphBorderStyle(border.BorderType)};");
    if ((double) border.LineWidth > 0.0)
    {
      float num = border.BorderType == BorderStyle.Double ? border.LineWidth * 3f : border.LineWidth;
      if ((double) num < 1.0)
        num = 1f;
      sb.Append($"border-{suffix}-width:{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
    }
    if ((double) border.Space <= 0.0)
      return;
    sb.Append($"padding-{suffix}:{border.Space.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt;");
  }

  private string ToBorderStyle(BorderStyle style)
  {
    switch (style)
    {
      case BorderStyle.None:
        return "hidden";
      case BorderStyle.Single:
      case BorderStyle.Thick:
      case BorderStyle.Hairline:
      case BorderStyle.Wave:
      case BorderStyle.DashDotStroker:
        return "solid";
      case BorderStyle.Double:
        return "double";
      case BorderStyle.Dot:
        return "dotted";
      case BorderStyle.DashLargeGap:
        return "dashed";
      case BorderStyle.DotDash:
        return "dashed";
      case BorderStyle.DotDotDash:
        return "dashed";
      case BorderStyle.Triple:
        return "double";
      case BorderStyle.ThinThickSmallGap:
        return "double";
      case BorderStyle.ThinThinSmallGap:
        return "double";
      case BorderStyle.ThinThickThinSmallGap:
        return "double";
      case BorderStyle.ThinThickMediumGap:
        return "double";
      case BorderStyle.ThickThinMediumGap:
        return "double";
      case BorderStyle.ThickThickThinMediumGap:
        return "double";
      case BorderStyle.ThinThickLargeGap:
        return "double;";
      case BorderStyle.ThickThinLargeGap:
        return "double";
      case BorderStyle.ThinThickThinLargeGap:
        return "double";
      case BorderStyle.DoubleWave:
        return "double";
      case BorderStyle.DashSmallGap:
        return "dashed";
      case BorderStyle.Emboss3D:
        return "ridge";
      case BorderStyle.Engrave3D:
        return "groove";
      case BorderStyle.Outset:
        return "outset";
      case BorderStyle.Inset:
        return "inset";
      case BorderStyle.Cleared:
        return "none";
      default:
        return "solid";
    }
  }

  private string ToParagraphBorderStyle(BorderStyle style)
  {
    switch (style)
    {
      case BorderStyle.None:
        return "hidden";
      case BorderStyle.Double:
        return "double";
      case BorderStyle.Dot:
        return "dotted";
      case BorderStyle.DashLargeGap:
        return "dashed";
      case BorderStyle.DotDash:
        return "dashed";
      case BorderStyle.DotDotDash:
        return "dashed";
      case BorderStyle.Outset:
        return "outset";
      case BorderStyle.Inset:
        return "inset";
      case BorderStyle.Cleared:
        return "none";
      default:
        return "solid";
    }
  }

  private string EncodeName(string name)
  {
    name = name.Trim();
    name = this.CheckValidSymbols(name);
    if (name.StartsWithExt("-"))
      name = name.Remove(0, 1);
    if (char.IsDigit(name[0]))
      name = "Style_" + name;
    return name;
  }

  private string CheckValidSymbols(string name)
  {
    int index = 0;
    for (int length = name.Length; index < length; ++index)
    {
      if (!char.IsLetterOrDigit(name[index]) && name[index] != '_')
        name = name.Replace(name[index], '_');
    }
    return name;
  }

  private void ProcessImageUsingFileStream(string imgPath, byte[] imageBytes)
  {
    using (FileStream fileStream = new FileStream(this.m_imagesFolder + imgPath, FileMode.Create))
    {
      using (MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length))
        memoryStream.WriteTo((Stream) fileStream);
    }
  }

  private Bitmap ConvertEMFToBitmap(System.Drawing.Image emfImage)
  {
    System.Drawing.Imaging.Metafile metafile = emfImage as System.Drawing.Imaging.Metafile;
    GraphicsUnit pageUnit = GraphicsUnit.Display;
    Size size = Size.Ceiling(metafile.GetBounds(ref pageUnit).Size);
    Bitmap bitmap = new Bitmap(size.Width, size.Height);
    bitmap.SetResolution(metafile.HorizontalResolution, metafile.VerticalResolution);
    using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
    {
      graphics.DrawImageUnscaled((System.Drawing.Image) metafile, Point.Empty);
      graphics.Dispose();
    }
    return bitmap;
  }

  private void ProcessImage(
    System.Drawing.Image srcImage,
    string imgPath,
    int width,
    int height,
    bool isMetafile,
    System.Drawing.Imaging.ImageFormat format)
  {
    if (srcImage == null)
      return;
    System.Drawing.Image original = (System.Drawing.Image) srcImage.Clone();
    Bitmap bitmap;
    if (isMetafile)
    {
      bitmap = this.ConvertEMFToBitmap(srcImage);
      if (!this.m_cacheFilesInternally)
        bitmap.Save(this.m_imagesFolder + imgPath, format);
    }
    else
    {
      bitmap = new Bitmap(original, width, height);
      if (!this.m_cacheFilesInternally)
        bitmap.Save(this.m_imagesFolder + imgPath, format);
    }
    if (!this.m_cacheFilesInternally)
      return;
    MemoryStream memoryStream = new MemoryStream();
    bitmap.Save((Stream) memoryStream, format);
    this.m_imageFiles.Add(imgPath, (Stream) memoryStream);
  }

  private void WriteEmptyPara(WCharacterFormat chFormat)
  {
    this.m_writer.WriteStartElement("span");
    string style = this.GetStyle(chFormat);
    if (style.Length > 0)
      this.m_writer.WriteAttributeString("style", style);
    this.m_writer.WriteRaw("&#xa0;");
    this.m_writer.WriteEndElement();
  }

  private void WriteText(string text)
  {
    text = text.Replace("&", "&amp;");
    if (text.Contains(ControlChar.Tab))
    {
      string tabText = this.GetTabText();
      text = text.Replace(ControlChar.Tab, tabText);
    }
    text = text.Replace("<", "&lt;");
    text = text.Replace(">", "&gt;");
    text = text.Replace(ControlChar.LineBreak, "</br>");
    text = text.Replace('\u001E'.ToString(), "&#8209;");
    text = text.Replace('\u001F'.ToString(), "&#xad;");
    text = text.Replace("\"", "&quot;");
    this.m_writer.WriteRaw(DocxSerializator.ReplaceInvalidSurrogateCharacters(text));
  }

  private string GetTabText()
  {
    string empty = string.Empty;
    for (int index = 0; index < 15; ++index)
      empty += ControlChar.NonBreakingSpace;
    return empty + ControlChar.Space;
  }

  private string ReplaceEmptySpace(string text)
  {
    if (text.StartsWithExt(ControlChar.Space))
    {
      string str1 = text.TrimStart(' ');
      int length = text.IndexOf(str1);
      string str2 = text.Substring(0, length);
      text = str1.Length != 0 ? str2.Replace(ControlChar.Space, ControlChar.NonBreakingSpace) + str1 : text.Replace(ControlChar.Space, ControlChar.NonBreakingSpace);
    }
    return text;
  }

  private string GetClassAttr(Style style)
  {
    List<string> styleHirarchy = new List<string>();
    string empty = string.Empty;
    if (style != null)
    {
      styleHirarchy.Add(style.Name);
      this.UpdateStyleHierarchy(style.BaseStyle as Style, styleHirarchy);
      for (int index = styleHirarchy.Count - 1; index >= 0; --index)
      {
        empty += this.EncodeName(styleHirarchy[index]);
        if (index > 0)
          empty += " ";
      }
    }
    return empty;
  }

  private void UpdateStyleHierarchy(Style style, List<string> styleHirarchy)
  {
    if (style == null || style.StyleId == 0 || style.Name.StartsWithExt("Normal"))
      return;
    styleHirarchy.Add(style.Name);
    if (style.BaseStyle == null)
      return;
    this.UpdateStyleHierarchy(style.BaseStyle as Style, styleHirarchy);
  }

  private bool CheckParentFormat(WParagraphFormat format)
  {
    if (format.OwnerBase != null)
    {
      if (format.OwnerBase.OwnerBase is WTableCell && format.OwnerBase is WParagraph)
      {
        WTableCell ownerBase1 = format.OwnerBase.OwnerBase as WTableCell;
        WParagraph ownerBase2 = format.OwnerBase as WParagraph;
        WTextRange wtextRange = new WTextRange((IWordDocument) ownerBase2.Document);
        float height = ownerBase1.OwnerRow.Height;
        if (ownerBase2.Items.Count > 0 && ownerBase2.Items.FirstItem is WTextRange)
          wtextRange = ownerBase2.Items.FirstItem as WTextRange;
        if ((double) height <= (double) wtextRange.CharacterFormat.FontSize)
          return false;
      }
      else if (format.OwnerBase.OwnerBase is WTextBody && format.OwnerBase is WParagraph)
      {
        WParagraph ownerBase = format.OwnerBase as WParagraph;
        if (ownerBase.Items.Count > 0 && ownerBase.ListFormat.ListType == ListType.NoList)
        {
          IEntity entity = (IEntity) ownerBase.Items.FirstItem;
          while (entity != null && entity.NextSibling != null && entity.EntityType != EntityType.TextRange)
          {
            entity = (IEntity) (entity.NextSibling as Entity);
            if (entity is WTextRange)
              break;
          }
          if (entity is WTextRange)
          {
            WTextRange wtextRange = entity as WTextRange;
            if ((double) Math.Abs(format.LineSpacing) <= (double) wtextRange.CharacterFormat.FontSize)
              return false;
          }
        }
      }
    }
    return true;
  }

  private void CloseList(int paraLevelNum, WParagraph paragraph)
  {
    IEntity previousSibling = paragraph.PreviousSibling;
    if (previousSibling != null && previousSibling is WParagraph)
    {
      WListFormat listFormat1 = this.GetListFormat((WParagraph) previousSibling);
      WListFormat listFormat2 = this.GetListFormat(paragraph);
      if (paraLevelNum == listFormat1.ListLevelNumber && !(listFormat2.CustomStyleName != listFormat1.CustomStyleName) && !(listFormat2.LFOStyleName != listFormat1.LFOStyleName) || this.listStack.Count <= 0)
        return;
      this.WriteEndElement(this.listStack.Count);
    }
    else
    {
      if (this.listStack.Count <= 0)
        return;
      this.WriteEndElement(this.listStack.Count);
    }
  }

  private void WriteEndElement(int levelDiff)
  {
    for (int index = 0; index < levelDiff; ++index)
    {
      if (this.listStack.Count > 0)
      {
        this.m_writer.WriteEndElement();
        this.m_writer.WriteRaw(ControlChar.CrLf);
        this.listStack.Pop();
      }
    }
  }

  private void WriteListType(ListPatternType type, WListFormat listFormat)
  {
    string str;
    switch (type)
    {
      case ListPatternType.UpRoman:
        str = "I";
        break;
      case ListPatternType.LowRoman:
        str = "i";
        break;
      case ListPatternType.UpLetter:
        str = "A";
        break;
      case ListPatternType.LowLetter:
        str = "a";
        break;
      case ListPatternType.Bullet:
        switch (listFormat.CurrentListLevel.LevelNumber)
        {
          case 0:
            str = "disc";
            break;
          case 1:
            str = "circle";
            break;
          case 2:
            str = "square";
            break;
          case 3:
            str = "disc";
            break;
          case 4:
            str = "circle";
            break;
          case 5:
            str = "square";
            break;
          default:
            str = "disc";
            break;
        }
        break;
      default:
        str = "1";
        break;
    }
    this.m_writer.WriteAttributeString(nameof (type), str);
  }

  private int GetLevelNumer(WListFormat listFormat)
  {
    return listFormat.ListType != ListType.NoList ? listFormat.ListLevelNumber : -1;
  }

  private int GetLstStartVal(WListFormat format)
  {
    if (format.CurrentListLevel.PatternType == ListPatternType.Bullet)
      return 1;
    if (!this.Lists.ContainsKey(format.CustomStyleName))
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      this.Lists.Add(format.CustomStyleName, dictionary);
      WListLevel level = format.CurrentListStyle.Levels[format.ListLevelNumber];
      for (int index = 0; index <= level.LevelNumber; ++index)
        dictionary.Add(index, format.CurrentListStyle.Levels[index].StartAt + 1);
      return level.StartAt;
    }
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    OverrideLevelFormat overrideLevelFormat = (OverrideLevelFormat) null;
    if (!string.IsNullOrEmpty(format.LFOStyleName))
    {
      listOverrideStyle = this.m_document.ListOverrides.FindByName(format.LFOStyleName);
      overrideLevelFormat = listOverrideStyle.OverrideLevels.LevelIndex.ContainsKey((short) format.ListLevelNumber) ? listOverrideStyle.OverrideLevels[format.ListLevelNumber] : (OverrideLevelFormat) null;
    }
    Dictionary<int, int> list = this.Lists[format.CustomStyleName];
    if (list.ContainsKey(format.ListLevelNumber))
    {
      int startAt = list[format.ListLevelNumber];
      if (overrideLevelFormat != null && this.IsAnyLevelStartAtTrue(listOverrideStyle) && !this.Lists.ContainsKey(format.LFOStyleName))
      {
        startAt = overrideLevelFormat.OverrideListLevel.StartAt;
        this.Lists.Add(format.LFOStyleName, (Dictionary<int, int>) null);
      }
      list[format.ListLevelNumber] = startAt + 1;
      for (int listLevelNumber = format.ListLevelNumber; list.ContainsKey(listLevelNumber + 1); ++listLevelNumber)
        list[listLevelNumber + 1] = 1;
      return startAt;
    }
    if (overrideLevelFormat != null)
    {
      list.Add(format.ListLevelNumber, overrideLevelFormat.OverrideListLevel.StartAt + 1);
      return overrideLevelFormat.OverrideListLevel.StartAt;
    }
    WListLevel level1 = format.CurrentListStyle.Levels[format.ListLevelNumber];
    list.Add(format.ListLevelNumber, level1.StartAt + 1);
    return level1.StartAt;
  }

  private bool IsAnyLevelStartAtTrue(ListOverrideStyle listOverrideStyle)
  {
    foreach (OverrideLevelFormat overrideLevel in (CollectionImpl) listOverrideStyle.OverrideLevels)
    {
      if (overrideLevel.OverrideStartAtValue)
        return true;
    }
    return false;
  }

  private void EnsureLvlRestart(WListFormat format, bool fullRestart)
  {
    if (this.m_lists == null || !this.Lists.ContainsKey(format.CustomStyleName))
      return;
    Dictionary<int, int> list = this.Lists[format.CustomStyleName];
    ICollection keys = (ICollection) list.Keys;
    IEnumerator enumerator = keys.GetEnumerator();
    int count = keys.Count;
    int[] numArray = new int[count];
    int index1 = 0;
    while (enumerator.MoveNext())
    {
      if (enumerator.Current != null)
        numArray[index1] = (int) enumerator.Current;
      ++index1;
    }
    for (int index2 = 0; index2 < count; ++index2)
    {
      if (fullRestart || numArray[index2] != 0 && !format.CurrentListStyle.Levels[numArray[index2]].NoRestartByHigher)
        list[numArray[index2]] = format.CurrentListStyle.Levels[numArray[index2]].StartAt;
    }
  }

  private float Build_TextureStyle(TextureStyle ts)
  {
    switch (ts)
    {
      case TextureStyle.TextureSolid:
        return 100f;
      case TextureStyle.Texture5Percent:
      case TextureStyle.Texture2Pt5Percent:
      case TextureStyle.Texture7Pt5Percent:
        return 5f;
      case TextureStyle.Texture10Percent:
        return 10f;
      case TextureStyle.Texture20Percent:
        return 20f;
      case TextureStyle.Texture25Percent:
      case TextureStyle.Texture27Pt5Percent:
        return 27.5f;
      case TextureStyle.Texture30Percent:
      case TextureStyle.Texture32Pt5Percent:
        return 32.5f;
      case TextureStyle.Texture40Percent:
      case TextureStyle.Texture42Pt5Percent:
        return 40f;
      case TextureStyle.Texture50Percent:
      case TextureStyle.Texture52Pt5Percent:
        return 50f;
      case TextureStyle.Texture60Percent:
        return 60f;
      case TextureStyle.Texture70Percent:
      case TextureStyle.Texture72Pt5Percent:
        return 70f;
      case TextureStyle.Texture75Percent:
      case TextureStyle.Texture77Pt5Percent:
        return 75f;
      case TextureStyle.Texture80Percent:
      case TextureStyle.Texture82Pt5Percent:
        return 80f;
      case TextureStyle.Texture90Percent:
      case TextureStyle.Texture92Pt5Percent:
        return 90f;
      case TextureStyle.Texture12Pt5Percent:
        return 12.5f;
      case TextureStyle.Texture15Percent:
        return 15f;
      case TextureStyle.Texture17Pt5Percent:
        return 17.5f;
      case TextureStyle.Texture35Percent:
        return 35f;
      case TextureStyle.Texture37Pt5Percent:
        return 37.5f;
      case TextureStyle.Texture45Percent:
      case TextureStyle.Texture47Pt5Percent:
        return 45f;
      case TextureStyle.Texture55Percent:
      case TextureStyle.Texture57Pt5Percent:
        return 55f;
      case TextureStyle.Texture62Pt5Percent:
        return 62.5f;
      case TextureStyle.Texture65Percent:
      case TextureStyle.Texture67Pt5Percent:
        return 65f;
      case TextureStyle.Texture85Percent:
        return 85f;
      case TextureStyle.Texture87Pt5Percent:
        return 87.5f;
      case TextureStyle.Texture95Percent:
      case TextureStyle.Texture97Pt5Percent:
        return 95f;
      default:
        return 0.0f;
    }
  }

  private WListFormat GetListFormat(WParagraph para)
  {
    if (string.IsNullOrEmpty(para.StyleName) || para.StyleName == "Normal" || para.ListFormat.CurrentListLevel != null && para.ListFormat.CurrentListStyle != null || para.ListFormat.IsEmptyList)
      return para.ListFormat;
    WListFormat listFormat = new WListFormat((IWParagraph) para);
    WParagraphStyle wparagraphStyle1 = (WParagraphStyle) null;
    while (listFormat.CurrentListLevel == null && listFormat.CurrentListStyle == null)
    {
      wparagraphStyle1 = wparagraphStyle1 == null ? para.Document.Styles.FindByName(para.StyleName) as WParagraphStyle : wparagraphStyle1.BaseStyle;
      if (wparagraphStyle1 != null && wparagraphStyle1.ListFormat != null)
      {
        if (listFormat.CurrentListLevel == null && listFormat.ListLevelNumber == 0 && wparagraphStyle1.ListFormat.CurrentListLevel != null && wparagraphStyle1.ListFormat.CurrentListLevel.LevelNumber >= 0 && wparagraphStyle1.ListFormat.CurrentListLevel.LevelNumber <= 8)
        {
          listFormat.ListLevelNumber = wparagraphStyle1.ListFormat.ListLevelNumber;
        }
        else
        {
          for (WParagraphStyle wparagraphStyle2 = para.ParaStyle as WParagraphStyle; wparagraphStyle2 != null; wparagraphStyle2 = wparagraphStyle2.BaseStyle)
          {
            int outLineLevel = this.GetOutLineLevel(wparagraphStyle2.ParagraphFormat);
            if (outLineLevel != -1)
            {
              listFormat.ListLevelNumber = outLineLevel;
              break;
            }
          }
        }
        if (listFormat.CurrentListStyle == null && !string.IsNullOrEmpty(wparagraphStyle1.ListFormat.CustomStyleName))
          listFormat.ApplyStyle(wparagraphStyle1.ListFormat.CustomStyleName);
        if (!string.IsNullOrEmpty(wparagraphStyle1.ListFormat.LFOStyleName))
          listFormat.LFOStyleName = wparagraphStyle1.ListFormat.LFOStyleName;
      }
      else
        break;
    }
    return listFormat;
  }

  private int GetOutLineLevel(WParagraphFormat paraFormat)
  {
    switch (paraFormat.OutlineLevel)
    {
      case OutlineLevel.Level1:
        return 0;
      case OutlineLevel.Level2:
        return 1;
      case OutlineLevel.Level3:
        return 2;
      case OutlineLevel.Level4:
        return 3;
      case OutlineLevel.Level5:
        return 4;
      case OutlineLevel.Level6:
        return 5;
      case OutlineLevel.Level7:
        return 6;
      case OutlineLevel.Level8:
        return 7;
      case OutlineLevel.Level9:
        return 8;
      default:
        return -1;
    }
  }

  private void SortBehindWrapStyleItemByZindex()
  {
    this.m_document.SortByZIndex(true);
    List<WPicture> wpictureList = new List<WPicture>();
    List<int> intList = new List<int>();
    foreach (Entity floatingItem in this.m_document.FloatingItems)
    {
      if (floatingItem is WPicture && (floatingItem as WPicture).TextWrappingStyle == TextWrappingStyle.Behind)
      {
        WPicture wpicture = floatingItem as WPicture;
        int num = Math.Abs(wpicture.OrderIndex);
        wpictureList.Add(wpicture);
        intList.Add(-num);
      }
    }
    int index = intList.Count - 1;
    while (wpictureList.Count != 0)
    {
      this.BehindWrapStyleFloatingItems.Add(wpictureList[0], intList[index]);
      wpictureList.RemoveAt(0);
      --index;
    }
  }

  private void CreateNavigationPoint(WParagraph para)
  {
    if (!this.m_hasNavigationId || string.IsNullOrEmpty(para.Text))
      return;
    foreach (TableOfContent tableOfContent in this.m_document.TOC.Values)
    {
      if (this.m_document.HasTOC && !tableOfContent.UseHeadingStyles)
      {
        foreach (int key in tableOfContent.TOCStyles.Keys)
        {
          foreach (Style style in tableOfContent.TOCStyles[key])
          {
            if (style.Name == para.StyleName && key <= (int) this.m_document.SaveOptions.EPubHeadingLevels)
            {
              string navigationPoint = this.GetNavigationPoint();
              this.m_writer.WriteAttributeString("id", navigationPoint);
              this.m_bookmarks.Add($"{key};{navigationPoint}", this.GetParagraphText(para.Text));
              break;
            }
          }
        }
      }
      else if (this.CheckHeadingStyle(para.StyleName))
      {
        int headingLevel = this.GetHeadingLevel(para.StyleName);
        if (headingLevel <= (int) this.m_document.SaveOptions.EPubHeadingLevels)
        {
          string navigationPoint = this.GetNavigationPoint();
          this.m_writer.WriteAttributeString("id", navigationPoint);
          this.m_bookmarks.Add($"{headingLevel.ToString((IFormatProvider) CultureInfo.InvariantCulture)};{navigationPoint}", this.GetParagraphText(para.Text));
        }
      }
    }
  }

  private bool CheckHeadingStyle(string styleName)
  {
    bool flag = false;
    foreach (string headingStyle in this.m_headingStyles)
    {
      if (styleName.ToLower(CultureInfo.InvariantCulture).Replace(" ", string.Empty).Contains(headingStyle))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private string GetParagraphText(string text)
  {
    text = text.Replace(ControlChar.LineBreak, string.Empty);
    return text;
  }

  private int GetHeadingLevel(string p)
  {
    if (p.Contains(","))
      p = p.Split(',')[0];
    if (p.Contains("+"))
      p = p.Split('+')[0];
    char[] charArray = p.ToCharArray();
    string s = "";
    foreach (char ch in charArray)
    {
      if (ch != '_')
      {
        int result;
        if (int.TryParse(ch.ToString((IFormatProvider) CultureInfo.InvariantCulture), out result))
          s += result.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
      else
        break;
    }
    return int.Parse(s);
  }

  internal string GetNavigationPoint() => "nav_Point" + (object) this.m_nameID++;
}
