// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFSerialization.ODFWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.Compression.Zip;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ODF.Base.ODFImplementation;
using Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFSerialization;

internal class ODFWriter
{
  private ZipArchive m_archieve;
  private XmlWriter m_writer;
  private ODocument m_document;
  private ODFStyleCollection m_odfStyles;
  private Dictionary<string, string> m_dateFormat;

  public ODFWriter()
  {
    this.m_archieve = new ZipArchive();
    this.m_archieve.DefaultCompressionLevel = CompressionLevel.Best;
  }

  private XmlWriter CreateWriter(Stream data)
  {
    XmlWriter writer = XmlWriter.Create(data, new XmlWriterSettings()
    {
      Encoding = (Encoding) new UTF8Encoding(false)
    });
    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"");
    return writer;
  }

  internal void SaveDocument(string fileName)
  {
    this.m_archieve.Save(fileName, true);
    this.m_archieve.Dispose();
    this.Dispose();
  }

  internal void SaveDocument(Stream stream)
  {
    this.m_archieve.Save(stream, false);
    this.m_archieve.Dispose();
  }

  internal void SerializeDocumentManifest()
  {
    this.m_archieve.AddItem("META-INF/", (Stream) new MemoryStream(), true, FileAttributes.Archive);
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("manifest", "manifest", "urn:oasis:names:tc:opendocument:xmlns:manifest:1.0");
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "/");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "application/vnd.oasis.opendocument.text");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "styles.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "content.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "settings.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "meta.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    if (this.m_document != null && this.m_document.DocumentImages != null && this.m_document.DocumentImages.Count > 0)
    {
      foreach (KeyValuePair<string, ImageRecord> documentImage in this.m_document.DocumentImages)
      {
        string str1 = string.Empty;
        ImageRecord imageRecord = documentImage.Value;
        if (imageRecord == null)
        {
          str1 = "media/image0.jpeg";
        }
        else
        {
          string str2 = imageRecord.IsMetafile ? ".wmf" : ".jpeg";
          if (imageRecord.ImageFormat.Equals((object) ImageFormat.Bmp))
            str2 = ".bmp";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Emf))
            str2 = ".emf";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Exif))
            str2 = ".exif";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Gif))
            str2 = ".gif";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Icon))
            str2 = ".ico";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Jpeg))
            str2 = ".jpeg";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.MemoryBmp))
            str2 = ".bmp";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Png))
            str2 = ".png";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Tiff))
            str2 = ".tif";
          else if (imageRecord.ImageFormat.Equals((object) ImageFormat.Wmf))
            str2 = ".wmf";
          string itemName = $"media/image{documentImage.Key.Replace("rId", "")}{str2}";
          this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
          this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, itemName);
          this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "image/" + str2.Substring(1));
          this.m_writer.WriteEndElement();
          if (this.m_archieve.Find(itemName.Replace("\\", "/")) == -1)
            this.m_archieve.AddItem(itemName, (Stream) new MemoryStream(imageRecord.ImageBytes), false, FileAttributes.Archive);
        }
      }
    }
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("META-INF/manifest.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal void SerializeMimeType()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, (Encoding) new UTF8Encoding(false));
    xmlTextWriter.WriteString("application/vnd.oasis.opendocument.text");
    xmlTextWriter.Flush();
    this.m_archieve.AddItem("mimetype", (Stream) memoryStream, false, FileAttributes.Archive);
  }

  internal void SerializeContent(MemoryStream stream)
  {
    this.m_archieve.AddItem("content.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  internal void SerializeMetaData()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-meta", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("meta.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal void SerializeSettings()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-settings", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "anim", (string) null, "urn:oasis:names:tc:opendocument:xmlns:animation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "chart", (string) null, "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
    this.m_writer.WriteAttributeString("xmlns", "onfig", (string) null, "urn:oasis:names:tc:opendocument:xmlns:config:1.0");
    this.m_writer.WriteAttributeString("xmlns", "db", (string) null, "urn:oasis:names:tc:opendocument:xmlns:database:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dr3d", (string) null, "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "form", (string) null, "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
    this.m_writer.WriteAttributeString("xmlns", "meta", (string) null, "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "presentation", (string) null, "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "script", (string) null, "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
    this.m_writer.WriteAttributeString("xmlns", "smil", (string) null, "urn:oasis:names:tc:opendocument:xmlns:smil-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "text", (string) null, "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "xhtml", (string) null, "http://www.w3.org/1999/xhtml");
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("settings.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal MemoryStream SerializeContentNameSpace()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-content", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "text", (string) null, "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
    this.m_writer.WriteAttributeString("xmlns", "of", (string) null, "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
    this.m_writer.WriteAttributeString("xmlns", "anim", (string) null, "urn:oasis:names:tc:opendocument:xmlns:animation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "chart", (string) null, "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
    this.m_writer.WriteAttributeString("xmlns", "onfig", (string) null, "urn:oasis:names:tc:opendocument:xmlns:config:1.0");
    this.m_writer.WriteAttributeString("xmlns", "db", (string) null, "urn:oasis:names:tc:opendocument:xmlns:database:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dr3d", (string) null, "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
    this.m_writer.WriteAttributeString("xmlns", "form", (string) null, "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
    this.m_writer.WriteAttributeString("xmlns", "meta", (string) null, "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
    this.m_writer.WriteAttributeString("xmlns", "presentation", (string) null, "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "script", (string) null, "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
    this.m_writer.WriteAttributeString("xmlns", "smil", (string) null, "urn:oasis:names:tc:opendocument:xmlns:smil-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xhtml", (string) null, "http://www.w3.org/1999/xhtml");
    return data;
  }

  internal void SerializeContentEnd(MemoryStream stream)
  {
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("content.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  internal void SerializeBodyStart()
  {
    this.m_writer.WriteStartElement("office", "body", (string) null);
  }

  internal void SerializeHeaderFooterContent(HeaderFooterContent headerFooter)
  {
    Stack<int> listStack = (Stack<int>) null;
    string empty = string.Empty;
    for (int index = 0; index < headerFooter.ChildItems.Count; ++index)
    {
      OTextBodyItem childItem = headerFooter.ChildItems[index];
      if (childItem is OParagraph)
        this.SerializeOParagraph((OParagraph) childItem, ref listStack, ref empty);
      if (childItem is OTable)
        this.SerializeTables(new List<OTable>()
        {
          childItem as OTable
        });
    }
  }

  internal void SerializeDocIOContent(ODocument document)
  {
    this.m_document = document;
    this.SerializeBodyStart();
    Stack<int> listStack = (Stack<int>) null;
    this.m_writer.WriteStartElement("office", "text", (string) null);
    this.m_writer.WriteAttributeString("text", "use-soft-page-breaks", (string) null, "true");
    string empty = string.Empty;
    for (int index = 0; index < document.Body.TextBodyItems.Count; ++index)
    {
      OTextBodyItem textBodyItem = document.Body.TextBodyItems[index];
      if (textBodyItem.IsFirstItemOfSection)
      {
        this.m_writer.WriteStartElement("text", "section", (string) null);
        this.m_writer.WriteAttributeString("text", "name", (string) null, "Sect" + Regex.Match(textBodyItem.SectionStyleName, "\\d+").Value);
        this.m_writer.WriteAttributeString("text", "style-name", (string) null, textBodyItem.SectionStyleName);
      }
      if (textBodyItem is OParagraph)
        this.SerializeOParagraph((OParagraph) textBodyItem, ref listStack, ref empty);
      if (textBodyItem is OTable)
        this.SerializeTables(new List<OTable>()
        {
          textBodyItem as OTable
        });
      if (textBodyItem.IsLastItemOfSection)
        this.m_writer.WriteEndElement();
    }
    if (listStack != null && listStack.Count > 0)
      this.SerializeEndList(ref listStack);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeList(
    OParagraph paragraph,
    ref Stack<int> listStack,
    ref string m_previousParaListStyleName)
  {
    if (!string.IsNullOrEmpty(paragraph.ListStyleName))
    {
      if (listStack == null)
      {
        m_previousParaListStyleName = paragraph.ListStyleName;
        this.SerializeListStartStyle(ref listStack, paragraph);
      }
      else if (paragraph.ListStyleName == m_previousParaListStyleName)
      {
        int num = listStack.Peek();
        int listLevelNumber = paragraph.ListLevelNumber;
        if (num == listLevelNumber)
        {
          this.m_writer.WriteEndElement();
          this.m_writer.WriteStartElement("text", "list-item", (string) null);
        }
        else if (num > listLevelNumber)
        {
          for (; num > listLevelNumber; --num)
          {
            this.m_writer.WriteEndElement();
            this.m_writer.WriteEndElement();
          }
          this.m_writer.WriteEndElement();
          listStack.Pop();
          listStack.Push(listLevelNumber);
          this.m_writer.WriteStartElement("text", "list-item", (string) null);
        }
        else
        {
          if (num >= listLevelNumber)
            return;
          this.m_writer.WriteStartElement("text", "list", (string) null);
          this.m_writer.WriteAttributeString("text", "continue-numbering", (string) null, "true");
          this.m_writer.WriteStartElement("text", "list-item", (string) null);
          for (int index = num + 1; index < listLevelNumber; ++index)
          {
            this.m_writer.WriteStartElement("text", "list", (string) null);
            this.m_writer.WriteStartElement("text", "list-item", (string) null);
          }
          listStack.Push(listLevelNumber);
        }
      }
      else
      {
        this.SerializeEndList(ref listStack);
        m_previousParaListStyleName = paragraph.ListStyleName;
        this.SerializeListStartStyle(ref listStack, paragraph);
      }
    }
    else
    {
      if (listStack == null || listStack.Count <= 0)
        return;
      this.SerializeEndList(ref listStack);
      listStack = (Stack<int>) null;
      m_previousParaListStyleName = string.Empty;
    }
  }

  private void SerializeListStartStyle(ref Stack<int> listStack, OParagraph paragraph)
  {
    listStack = new Stack<int>();
    listStack.Push(paragraph.ListLevelNumber);
    this.m_writer.WriteStartElement("text", "list", (string) null);
    this.m_writer.WriteAttributeString("text", "style-name", (string) null, paragraph.ListStyleName);
    this.m_writer.WriteAttributeString("text", "continue-numbering", (string) null, "true");
    this.m_writer.WriteStartElement("text", "list-item", (string) null);
    int num = listStack.Peek();
    for (int index = 0; index < num; ++index)
    {
      this.m_writer.WriteStartElement("text", "list", (string) null);
      this.m_writer.WriteStartElement("text", "list-item", (string) null);
    }
  }

  private void SerializeEndList(ref Stack<int> listStack)
  {
    for (int index = listStack.Peek(); index > -1; --index)
    {
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
    listStack.Clear();
  }

  private void SerializeTableOfContentSource()
  {
    this.m_writer.WriteStartElement("text", "table-of-content-source", (string) null);
    this.m_writer.WriteAttributeString("text", "outline-level", (string) null, "9");
    this.m_writer.WriteAttributeString("text", "use-outline-level", (string) null, "true");
    this.m_writer.WriteAttributeString("text", "use-index-marks", (string) null, "false");
    this.m_writer.WriteAttributeString("text", "use-index-source-styles", (string) null, "false");
    this.m_writer.WriteAttributeString("text", "index-scope", (string) null, "document");
    for (int index = 1; index < 10; ++index)
    {
      this.m_writer.WriteStartElement("text", "table-of-content-entry-template", (string) null);
      this.m_writer.WriteAttributeString("text", "outline-level", (string) null, index.ToString());
      this.m_writer.WriteAttributeString("text", "style-name", (string) null, index < this.m_document.TOCStyles.Count ? "TOC" + index.ToString() : "Normal");
      this.m_writer.WriteStartElement("text", "index-entry-text", (string) null);
      this.m_writer.WriteEndElement();
      TabStops tabStops = (TabStops) null;
      foreach (ODFStyle tocStyle in this.m_document.TOCStyles)
      {
        if (tocStyle.Name.EndsWith(index.ToString()))
        {
          tabStops = tocStyle.ParagraphProperties.TabStops[0];
          this.m_writer.WriteStartElement("text", "index-entry-tab-stop", (string) null);
          this.m_writer.WriteAttributeString("style", "type", (string) null, tabStops.TextAlignType.ToString());
          if (tabStops.TabStopLeader != TabStopLeader.NoLeader)
            this.m_writer.WriteAttributeString("style", "leader-char", (string) null, tabStops.TabStopLeader == TabStopLeader.Dotted ? "." : "");
          this.m_writer.WriteAttributeString("style", "position", (string) null, tabStops.TextPosition.ToString() + "in");
          this.m_writer.WriteEndElement();
          break;
        }
      }
      if (tabStops == null)
      {
        this.m_writer.WriteStartElement("text", "index-entry-tab-stop", (string) null);
        this.m_writer.WriteAttributeString("style", "type", (string) null, "right");
        this.m_writer.WriteAttributeString("style", "leader-char", (string) null, ".");
        this.m_writer.WriteEndElement();
        break;
      }
      this.m_writer.WriteStartElement("text", "index-entry-page-number", (string) null);
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private string GetNumberFormat(PageNumberFormat pageNumberFormat)
  {
    switch (pageNumberFormat)
    {
      case PageNumberFormat.LowerRoman:
        return "i";
      case PageNumberFormat.UpperRoman:
        return "I";
      case PageNumberFormat.LowerCase:
        return "";
      case PageNumberFormat.UpperCase:
        return "";
      case PageNumberFormat.UpperAlphabet:
        return "A";
      case PageNumberFormat.LowerAlphabet:
        return "a";
      case PageNumberFormat.Arabic:
        return "1";
      case PageNumberFormat.Ordinal:
        return "1st, 2nd, 3rd, ...";
      case PageNumberFormat.CardinalText:
        return "One, Two, Three, ...";
      case PageNumberFormat.OrdinalText:
        return "First, Second, Third, ...";
      case PageNumberFormat.Hexa:
        return "1, A, B, ...";
      case PageNumberFormat.DollorText:
        return "One, Two, Three, ...";
      case PageNumberFormat.ArabicDash:
        return "- 1 -, - 2 -, - 3 -, ...";
      default:
        return "1";
    }
  }

  internal void SerializePicture(OPicture picture)
  {
    this.m_writer.WriteStartElement("draw", "frame", (string) null);
    this.m_writer.WriteAttributeString("draw", "z-index", (string) null, picture.OrderIndex.ToString());
    if (!string.IsNullOrEmpty(picture.Name))
      this.m_writer.WriteAttributeString("draw", "name", (string) null, picture.Name);
    this.m_writer.WriteAttributeString("text", "anchor-type", (string) null, picture.TextWrappingStyle == Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Inline ? "as-char" : "paragraph");
    this.m_writer.WriteAttributeString("svg", "x", (string) null, (picture.HorizontalPosition / 72f).ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "y", (string) null, (picture.VerticalPosition / 72f).ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "height", (string) null, (picture.Height / 72f).ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "width", (string) null, (picture.Width / 72f).ToString() + "in");
    this.m_writer.WriteAttributeString("style", "rel-height", (string) null, "scale");
    this.m_writer.WriteAttributeString("style", "rel-width", (string) null, "scale");
    this.m_writer.WriteStartElement("draw", "image", (string) null);
    if (!string.IsNullOrEmpty(picture.OPictureHRef) && !picture.OPictureHRef.Contains("rId"))
    {
      this.m_writer.WriteAttributeString("xlink", "href", (string) null, picture.OPictureHRef);
    }
    else
    {
      string key = picture.OPictureHRef.Substring(picture.OPictureHRef.IndexOf("rId"));
      if (this.m_document != null && this.m_document.DocumentImages != null && this.m_document.DocumentImages.Count > 0)
      {
        ImageRecord documentImage = this.m_document.DocumentImages[key];
        if (documentImage == null)
        {
          string str1 = picture.OPictureHRef.Replace("rId", "") + "0.jpeg";
        }
        else
        {
          string str2 = documentImage.IsMetafile ? ".wmf" : ".jpeg";
          if (documentImage.ImageFormat.Equals((object) ImageFormat.Bmp))
            str2 = ".bmp";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Emf))
            str2 = ".emf";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Exif))
            str2 = ".exif";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Gif))
            str2 = ".gif";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Icon))
            str2 = ".ico";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Jpeg))
            str2 = ".jpeg";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.MemoryBmp))
            str2 = ".bmp";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Png))
            str2 = ".png";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Tiff))
            str2 = ".tif";
          else if (documentImage.ImageFormat.Equals((object) ImageFormat.Wmf))
            str2 = ".wmf";
          picture.OPictureHRef = picture.OPictureHRef.Replace("rId", "") + str2;
        }
      }
      this.m_writer.WriteAttributeString("xlink", "href", (string) null, picture.OPictureHRef);
    }
    this.m_writer.WriteAttributeString("xlink", "type", (string) null, "simple");
    this.m_writer.WriteAttributeString("xlink", "show", (string) null, "embed");
    this.m_writer.WriteAttributeString("xlink", "actuate", (string) null, "onLoad");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  internal void SerializeMergeField(OMergeField mergeField)
  {
    string empty = string.Empty;
    if (!string.IsNullOrEmpty(mergeField.TextBefore))
    {
      this.m_writer.WriteString(mergeField.TextBefore);
      if (!string.IsNullOrEmpty(mergeField.Text))
        empty += mergeField.Text.Replace(mergeField.TextBefore, "");
    }
    else if (!string.IsNullOrEmpty(mergeField.Text))
      empty += mergeField.Text;
    this.m_writer.WriteStartElement("text", "database-display", (string) null);
    this.m_writer.WriteAttributeString("text", "table-name", (string) null, "");
    this.m_writer.WriteAttributeString("text", "table-type", (string) null, "table");
    this.m_writer.WriteAttributeString("text", "column-name", (string) null, mergeField.FieldName);
    this.m_writer.WriteString(!string.IsNullOrEmpty(mergeField.TextAfter) ? empty.Replace(mergeField.TextAfter, "") : empty);
    this.m_writer.WriteEndElement();
    if (string.IsNullOrEmpty(mergeField.TextAfter))
      return;
    this.m_writer.WriteString(mergeField.TextAfter);
  }

  internal void SerializeDateTimeField(OField field)
  {
    string formattingString = field.FormattingString;
    string empty = string.Empty;
    this.m_writer.WriteStartElement("text", "date", (string) null);
    this.m_writer.WriteString(field.Text);
    this.m_writer.WriteEndElement();
  }

  internal void SerializeHyperlink(OField field)
  {
    this.m_writer.WriteStartElement("text", "a", (string) null);
    this.m_writer.WriteAttributeString("xlink", "href", (string) null, field.FieldValue.TrimStart('"').TrimEnd('"'));
    this.m_writer.WriteAttributeString("office", "target-frame-name", (string) null, "_top");
    this.m_writer.WriteAttributeString("xlink", "show", (string) null, "replace");
    this.m_writer.WriteStartElement("text", "span", (string) null);
    this.m_writer.WriteAttributeString("text", "style-name", (string) null, "Hyperlink");
    this.m_writer.WriteString(field.Text);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  internal Dictionary<string, string> DateFormat
  {
    get
    {
      if (this.m_dateFormat == null)
        this.m_dateFormat = new Dictionary<string, string>();
      return this.m_dateFormat;
    }
  }

  private void DateStyle(string formattingString, string styleName, CultureInfo culture)
  {
    this.m_writer.WriteStartElement("number", "date-style", (string) null);
    this.m_writer.WriteAttributeString("style", "name", (string) null, styleName);
    this.m_writer.WriteAttributeString("number", "language", (string) null, culture.Parent.ToString());
    this.m_writer.WriteAttributeString("number", "country", (string) null, culture.Name.Substring(3, 2));
    if (formattingString.Contains("dddd"))
    {
      this.m_writer.WriteStartElement("number", "day-of-week", (string) null);
      this.m_writer.WriteAttributeString("number", "number:style", (string) null, "long");
      this.m_writer.WriteAttributeString("number", "calendar", (string) null, "gregorian");
      this.m_writer.WriteEndElement();
    }
    if (!formattingString.Contains("MMMM"))
      return;
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDefaultStyles(DefaultStyleCollection defaultStyle)
  {
    int count = defaultStyle.DefaultStyles.Values.Count;
    DefaultStyle[] array = new DefaultStyle[count];
    defaultStyle.DefaultStyles.Values.CopyTo(array, 0);
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    for (int index = 0; index < count; ++index)
    {
      DefaultStyle defaultStyle1 = array[index];
      this.m_writer.WriteStartElement("style", "default-style", (string) null);
      this.m_writer.WriteAttributeString("style", "family", (string) null, "paragraph");
      this.SerializeDefaultParagraphProperties(defaultStyle1.ParagraphProperties);
      this.SerializeDefaultTextProperties(defaultStyle1.Textproperties);
      this.m_writer.WriteEndElement();
    }
  }

  private void SerializeCalculationSettings()
  {
    this.m_writer.WriteStartElement("table", "calculation-settings", (string) null);
    this.m_writer.WriteAttributeString("table", "use-regular-expressions", (string) null, bool.FalseString.ToLower());
    this.m_writer.WriteEndElement();
  }

  private void SerializeDefaultParagraphProperties(ODFParagraphProperties paragraphProperties)
  {
    if (paragraphProperties == null)
      return;
    this.m_writer.WriteStartElement("style", "paragraph-properties", (string) null);
    if (paragraphProperties.HasKey(9, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, paragraphProperties.KeepTogether.ToString());
    else
      this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, "auto");
    if (paragraphProperties.HasKey(3, (int) paragraphProperties.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "keep-with-next", (string) null, paragraphProperties.KeepWithNext.ToString());
    else
      this.m_writer.WriteAttributeString("fo", "keep-with-next", (string) null, "auto");
    if (paragraphProperties.BeforeBreak == BeforeBreak.page)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "page");
    else if (paragraphProperties.BeforeBreak == BeforeBreak.column)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "column");
    else
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "auto");
    if (paragraphProperties.HasKey(2, (int) paragraphProperties.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paragraphProperties.MarginTop.ToString() + "in");
    if (paragraphProperties.HasKey(3, (int) paragraphProperties.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paragraphProperties.MarginBottom.ToString() + "in");
    if (paragraphProperties.HasKey(0, (int) paragraphProperties.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paragraphProperties.MarginBottom.ToString() + "in");
    if (paragraphProperties.HasKey(1, (int) paragraphProperties.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paragraphProperties.MarginRight.ToString() + "in");
    if (paragraphProperties.HasKey(0, (int) paragraphProperties.borderFlags) && paragraphProperties.Border != null)
    {
      this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{paragraphProperties.Border.LineWidth}in {(object) paragraphProperties.Border.LineStyle} {ODFWriter.HexConverter(paragraphProperties.Border.LineColor)}");
      this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paragraphProperties.PaddingLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paragraphProperties.PaddingRight.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paragraphProperties.PaddingTop.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paragraphProperties.PaddingBottom.ToString() + "in");
    }
    if (paragraphProperties.HasKey(1, (int) paragraphProperties.borderFlags) || paragraphProperties.HasKey(2, (int) paragraphProperties.borderFlags) || paragraphProperties.HasKey(3, (int) paragraphProperties.borderFlags) || paragraphProperties.HasKey(4, (int) paragraphProperties.borderFlags))
    {
      if (paragraphProperties.BorderLeft != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{paragraphProperties.BorderLeft.LineWidth}in {(object) paragraphProperties.BorderLeft.LineStyle} {ODFWriter.HexConverter(paragraphProperties.BorderLeft.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paragraphProperties.PaddingLeft.ToString() + "in");
      }
      if (paragraphProperties.BorderRight != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{paragraphProperties.BorderRight.LineWidth}in {(object) paragraphProperties.BorderRight.LineStyle} {ODFWriter.HexConverter(paragraphProperties.BorderRight.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paragraphProperties.PaddingRight.ToString() + "in");
      }
      if (paragraphProperties.BorderTop != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{paragraphProperties.BorderTop.LineWidth}in {(object) paragraphProperties.BorderTop.LineStyle} {ODFWriter.HexConverter(paragraphProperties.BorderTop.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paragraphProperties.PaddingTop.ToString() + "in");
      }
      if (paragraphProperties.BorderBottom != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{paragraphProperties.BorderBottom.LineWidth}in {(object) paragraphProperties.BorderBottom.LineStyle} {ODFWriter.HexConverter(paragraphProperties.BorderBottom.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paragraphProperties.PaddingBottom.ToString() + "in");
      }
    }
    if (paragraphProperties.HasKey(21, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-align", (string) null, paragraphProperties.TextAlign.ToString().ToLower());
    else
      this.m_writer.WriteAttributeString("fo", "text-align", (string) null, "start");
    if (paragraphProperties.HasKey(6, (int) paragraphProperties.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, paragraphProperties.BackgroundColor);
    else
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, "transparent");
    if (paragraphProperties.HasKey(19, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-indent", (string) null, paragraphProperties.TextIndent.ToString() + "in");
    if (paragraphProperties.HasKey(10, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("style", "line-height-at-least", (string) null, paragraphProperties.LineHeightAtLeast.ToString() + "in");
    if (paragraphProperties.HasKey(28, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paragraphProperties.LineHeight.ToString() + "in");
    else if (paragraphProperties.HasKey(9, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paragraphProperties.LineSpacing.ToString() + "%");
    else
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, "100%");
    if (paragraphProperties.HasKey(0, (int) paragraphProperties.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paragraphProperties.BeforeSpacing.ToString() + "in");
    if (paragraphProperties.HasKey(1, (int) paragraphProperties.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paragraphProperties.AfterSpacing.ToString() + "in");
    if (paragraphProperties.HasKey(2, (int) paragraphProperties.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paragraphProperties.LeftIndent.ToString() + "in");
    if (paragraphProperties.HasKey(3, (int) paragraphProperties.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paragraphProperties.RightIndent.ToString() + "in");
    if (paragraphProperties.WritingMode == WritingMode.RLTB)
      this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "rl-tb");
    if (paragraphProperties != null && paragraphProperties.TabStops != null && paragraphProperties.TabStops.Count > 0)
    {
      this.m_writer.WriteStartElement("style", "tab-stops", (string) null);
      for (int index = 0; index < paragraphProperties.TabStops.Count; ++index)
      {
        TabStops tabStop = paragraphProperties.TabStops[index];
        this.m_writer.WriteStartElement("style", "tab-stop", (string) null);
        this.m_writer.WriteAttributeString("style", "type", (string) null, tabStop.TextAlignType.ToString());
        this.m_writer.WriteAttributeString("style", "position", (string) null, tabStop.TextPosition.ToString() + "in");
        if (tabStop.TabStopLeader != TabStopLeader.NoLeader)
          this.m_writer.WriteAttributeString("style", "leader-char", (string) null, tabStop.TabStopLeader == TabStopLeader.Dotted ? "." : "");
        this.m_writer.WriteEndElement();
      }
      this.m_writer.WriteEndElement();
    }
    if (paragraphProperties.HasKey(18, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "widows", (string) null, paragraphProperties.Windows.ToString());
    else
      this.m_writer.WriteAttributeString("fo", "widows", (string) null, "2");
    if (paragraphProperties.HasKey(27, paragraphProperties.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "orphans", (string) null, paragraphProperties.Orphans.ToString());
    else
      this.m_writer.WriteAttributeString("fo", "orphans", (string) null, "2");
    this.m_writer.WriteAttributeString("style", "tab-stop-distance", (string) null, "0.5in");
    this.m_writer.WriteEndElement();
  }

  private void SerializeDefaultTextProperties(TextProperties textProperties)
  {
    if (textProperties == null)
      return;
    this.m_writer.WriteStartElement("style", "text-properties", (string) null);
    if (textProperties.HasKey(16 /*0x10*/, textProperties.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("style", "font-name", (string) null, textProperties.FontName);
    }
    else
    {
      this.m_writer.WriteAttributeString("style", "font-name", (string) null, "Calibri");
      this.m_writer.WriteAttributeString("style", "font-name-asian", (string) null, "Calibri");
      this.m_writer.WriteAttributeString("style", "font-name-complex", (string) null, "Times New Roman");
    }
    if (textProperties.HasKey(17, textProperties.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("fo", "font-size", (string) null, textProperties.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-asian", (string) null, textProperties.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-complex", (string) null, textProperties.FontSize.ToString() + "pt");
    }
    else
    {
      this.m_writer.WriteAttributeString("fo", "font-size", (string) null, "10pt");
      this.m_writer.WriteAttributeString("style", "font-size-asian", (string) null, "10pt");
      this.m_writer.WriteAttributeString("style", "font-size-complex", (string) null, "10pt");
    }
    if (textProperties.HasKey(23, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "color", (string) null, ODFWriter.HexConverter(textProperties.Color));
    if (textProperties.HasKey(22, textProperties.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("fo", "font-weight", (string) null, textProperties.FontWeight.ToString());
      this.m_writer.WriteAttributeString("style", "font-weight-asian", (string) null, textProperties.FontWeightAsian.ToString());
      this.m_writer.WriteAttributeString("style", "font-style-asian", (string) null, textProperties.FontStyleAsian.ToString());
    }
    else
    {
      this.m_writer.WriteAttributeString("fo", "font-weight", (string) null, "normal");
      this.m_writer.WriteAttributeString("style", "font-weight-asian", (string) null, "normal");
      this.m_writer.WriteAttributeString("style", "font-weight-complex", (string) null, "normal");
    }
    if (textProperties.FontStyle == ODFFontStyle.italic)
      this.m_writer.WriteAttributeString("fo", "font-style", (string) null, textProperties.FontStyle.ToString());
    if (textProperties.HasKey(5, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-underline-type", (string) null, textProperties.TextUnderlineType.ToString().ToLower());
    if (textProperties.HasKey(6, textProperties.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("style", "text-underline-syle", (string) null, textProperties.TextUnderlineStyle.ToString());
      this.m_writer.WriteAttributeString("style", "text-underline-color", (string) null, textProperties.TextUnderlineColor);
      this.m_writer.WriteAttributeString("style", "text-underline-type", (string) null, "single");
    }
    if (textProperties.HasKey(0, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "font-relief", (string) null, textProperties.FontRelief.ToString());
    if (textProperties.HasKey(20, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, ODFWriter.HexConverter(textProperties.BackgroundColor));
    else
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, "transparent");
    if (textProperties.HasKey(0, textProperties.m_textFlag3))
      this.m_writer.WriteAttributeString("style", "letter-kerning", (string) null, textProperties.LetterKerning.ToString().ToLower());
    if (textProperties.HasKey(9, textProperties.m_textFlag3))
    {
      this.m_writer.WriteAttributeString("style", "text-line-through-type", (string) null, textProperties.LinethroughType.ToString());
      if (textProperties.LinethroughType != LineType.none)
      {
        this.m_writer.WriteAttributeString("style", "text-line-through-style", (string) null, textProperties.LinethroughStyle.ToString());
        this.m_writer.WriteAttributeString("style", "text-line-through-color", (string) null, textProperties.LinethroughColor);
      }
    }
    if (textProperties.HasKey(3, textProperties.m_textFlag2) && !textProperties.HasKey(28, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "text-transform", (string) null, textProperties.TextTransform.ToString());
    if (textProperties.HasKey(9, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-scale", (string) null, textProperties.TextScale.ToString() + "%");
    if (textProperties.HasKey(3, textProperties.m_textFlag3))
      this.m_writer.WriteAttributeString("style", "text-outline", (string) null, textProperties.TextOutline.ToString().ToLower());
    if (textProperties.HasKey(27, textProperties.m_textFlag1) && !textProperties.IsTextDisplay)
      this.m_writer.WriteAttributeString("text", "display", (string) null, "none");
    if (textProperties.HasKey(21, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-position", (string) null, textProperties.TextPosition.ToString());
    if (textProperties.HasKey(28, textProperties.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "font-variant", (string) null, "small-caps");
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTables(List<OTable> tables)
  {
    for (int index1 = 0; index1 < tables.Count; ++index1)
    {
      OTable table = tables[index1];
      this.m_writer.WriteStartElement("table", "table", (string) null);
      this.m_writer.WriteAttributeString("table", "style-name", (string) null, table.StyleName);
      int count1 = table.Columns.Count;
      this.m_writer.WriteStartElement("table", "table-columns", (string) null);
      for (int index2 = 0; index2 < count1; ++index2)
      {
        OTableColumn column = table.Columns[index2];
        this.m_writer.WriteStartElement("table", "table-column", (string) null);
        if (!string.IsNullOrEmpty(column.StyleName))
          this.m_writer.WriteAttributeString("table", "style-name", (string) null, column.StyleName);
        this.m_writer.WriteEndElement();
      }
      this.m_writer.WriteEndElement();
      int count2 = table.Rows.Count;
      for (int index3 = 0; index3 < count2; ++index3)
      {
        OTableRow row = table.Rows[index3];
        this.m_writer.WriteStartElement("table", "table-row", (string) null);
        if (!string.IsNullOrEmpty(row.StyleName))
          this.m_writer.WriteAttributeString("table", "style-name", (string) null, row.StyleName);
        if (!string.IsNullOrEmpty(row.DefaultCellStyleName))
          this.m_writer.WriteAttributeString("table", "default-cell-style-name", (string) null, row.DefaultCellStyleName);
        if (row.IsCollapsed)
          this.m_writer.WriteAttributeString("table", "visibility", (string) null, "collapse");
        int count3 = row.Cells.Count;
        for (int index4 = 0; index4 < count3; ++index4)
        {
          OTableCell cell = row.Cells[index4];
          this.m_writer.WriteStartElement("table", "table-cell", (string) null);
          if (!string.IsNullOrEmpty(cell.StyleName))
            this.m_writer.WriteAttributeString("table", "style-name", (string) null, cell.StyleName);
          if (cell.ColumnsSpanned > 1)
            this.m_writer.WriteAttributeString("table", "number-columns-spanned", (string) null, cell.ColumnsSpanned.ToString());
          if (!cell.IsBlank)
          {
            Stack<int> listStack = (Stack<int>) null;
            string empty = string.Empty;
            for (int index5 = 0; index5 < cell.TextBodyIetm.Count; ++index5)
            {
              OTextBodyItem paragraph = cell.TextBodyIetm[index5];
              if (paragraph is OParagraph)
                this.SerializeOParagraph((OParagraph) paragraph, ref listStack, ref empty);
              if (paragraph is OTable)
                this.SerializeTables(new List<OTable>()
                {
                  paragraph as OTable
                });
            }
          }
          this.m_writer.WriteEndElement();
        }
        this.m_writer.WriteEndElement();
      }
      this.m_writer.WriteEndElement();
    }
  }

  private void WriteCellType(OTableCell curCell)
  {
    switch (curCell.Type)
    {
      case CellValueType.Float:
      case CellValueType.Percentage:
      case CellValueType.Currency:
        this.m_writer.WriteAttributeString("office", "value", (string) null, curCell.Value != null ? curCell.Value.ToString() : string.Empty);
        break;
      case CellValueType.Date:
        this.m_writer.WriteAttributeString("office", "date-value", (string) null, curCell.DateValue.ToString("yyyy-MM-ddTHH:mm:ss"));
        break;
      case CellValueType.Time:
        this.m_writer.WriteAttributeString("office", "time-value", (string) null, ODFWriter.ToReadableString(curCell.TimeValue));
        break;
      case CellValueType.Boolean:
        this.m_writer.WriteAttributeString("office", "boolean-value", (string) null, curCell.BooleanValue.ToString());
        break;
    }
  }

  private void WriteRepeatedCells(OTableRow row, OTableCell cell, int colsRepeated)
  {
    if (cell == null)
      return;
    this.m_writer.WriteStartElement("table", "table-cell", (string) null);
    if (cell.ColumnsRepeated != 0 && cell.ColumnsRepeated > 1)
      this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, cell.ColumnsRepeated.ToString());
    else
      this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, colsRepeated.ToString());
    if (!string.IsNullOrEmpty(row.DefaultCellStyleName))
      this.m_writer.WriteAttributeString("table", "style-name", (string) null, row.DefaultCellStyleName);
    this.m_writer.WriteEndElement();
  }

  private void SerializeOParagraph(
    OParagraph paragraph,
    ref Stack<int> listStack,
    ref string m_previousParaListStyleName)
  {
    this.SerializeList(paragraph, ref listStack, ref m_previousParaListStyleName);
    if (!string.IsNullOrEmpty(paragraph.TocMark) && paragraph.TocMark == this.m_document.TOCStyles[0].Name)
    {
      this.m_writer.WriteStartElement("text", "table-of-content", (string) null);
      this.m_writer.WriteAttributeString("text", "name", (string) null, "_TOC0");
      this.SerializeTableOfContentSource();
      this.m_writer.WriteStartElement("text", "index-body", (string) null);
    }
    if (paragraph.Header != null && paragraph.Header.StyleName != null)
    {
      this.m_writer.WriteStartElement("text", "h", (string) null);
      this.m_writer.WriteAttributeString("text", "style-name", (string) null, paragraph.StyleName);
    }
    else
    {
      this.m_writer.WriteStartElement("text", "p", (string) null);
      if (paragraph.StyleName != null)
        this.m_writer.WriteAttributeString("text", "style-name", (string) null, paragraph.StyleName);
    }
    List<OParagraphItem> oparagraphItemCollection = paragraph.OParagraphItemCollection;
    int count = oparagraphItemCollection.Count;
    bool flag = false;
    if (count > 0)
    {
      for (int index = 0; index < oparagraphItemCollection.Count; ++index)
      {
        OParagraphItem oparagraphItem = oparagraphItemCollection[index];
        if (oparagraphItem is OBreak)
        {
          OBreakType breakType = (oparagraphItem as OBreak).BreakType;
          if (breakType == OBreakType.LineBreak && oparagraphItem.ParagraphProperties.LineBreak)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            this.m_writer.WriteStartElement("text", "line-break", (string) null);
            this.m_writer.WriteEndElement();
            this.m_writer.WriteEndElement();
          }
          else if (breakType == OBreakType.ColumnBreak || breakType == OBreakType.PageBreak)
            flag = true;
        }
        if (oparagraphItem is OTextRange)
        {
          if (oparagraphItem.Text == "\t")
          {
            this.m_writer.WriteStartElement("text", "tab", (string) null);
            this.m_writer.WriteEndElement();
          }
          else if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            if (oparagraphItem.StyleName != null)
              this.m_writer.WriteAttributeString("text", "style-name", (string) null, oparagraphItem.StyleName);
            this.m_writer.WriteString(oparagraphItem.Text);
            this.m_writer.WriteEndElement();
          }
          else
            this.m_writer.WriteString(oparagraphItem.Text);
        }
        if (oparagraphItem is OPicture)
        {
          if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            if (oparagraphItem.StyleName != null)
              this.m_writer.WriteAttributeString("text", "style-name", (string) null, oparagraphItem.StyleName);
          }
          this.SerializePicture(oparagraphItem as OPicture);
          if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
            this.m_writer.WriteEndElement();
          if (flag)
            this.m_writer.WriteElementString("text", "soft-page-break");
        }
        if (oparagraphItem is OMergeField)
        {
          if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            if (oparagraphItem.StyleName != null)
              this.m_writer.WriteAttributeString("text", "style-name", (string) null, oparagraphItem.StyleName);
          }
          this.SerializeMergeField(oparagraphItem as OMergeField);
          this.m_writer.WriteEndElement();
        }
        if (oparagraphItem is OField)
        {
          if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            if (oparagraphItem.StyleName != null)
              this.m_writer.WriteAttributeString("text", "style-name", (string) null, oparagraphItem.StyleName);
          }
          if ((oparagraphItem as OField).OFieldType == OFieldType.FieldDate)
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldHyperlink)
            this.SerializeHyperlink(oparagraphItem as OField);
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldNumPages)
          {
            this.m_writer.WriteStartElement("text", "page-count", (string) null);
            string numberFormat = this.GetNumberFormat((oparagraphItem as OField).PageNumberFormat);
            if (!string.IsNullOrEmpty(numberFormat))
              this.m_writer.WriteAttributeString("style", "num-format", (string) null, numberFormat);
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldPage)
          {
            this.m_writer.WriteStartElement("text", "page-number", (string) null);
            string numberFormat = this.GetNumberFormat((oparagraphItem as OField).PageNumberFormat);
            if (!string.IsNullOrEmpty(numberFormat))
              this.m_writer.WriteAttributeString("style", "num-format", (string) null, numberFormat);
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldAuthor)
          {
            this.m_writer.WriteStartElement("text", "initial-creator", (string) null);
            this.m_writer.WriteAttributeString("style", "fixed", (string) null, "false");
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldTitle)
          {
            this.m_writer.WriteStartElement("text", "title", (string) null);
            this.m_writer.WriteAttributeString("style", "fixed", (string) null, "false");
            if (!string.IsNullOrEmpty((oparagraphItem as OField).Text))
              this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          else if ((oparagraphItem as OField).OFieldType == OFieldType.FieldTOC || (oparagraphItem as OField).OFieldType == OFieldType.FieldPageRef)
          {
            this.m_writer.WriteStartElement("text", "tab", (string) null);
            this.m_writer.WriteEndElement();
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            if (paragraph.TocMark == this.m_document.TOCStyles[this.m_document.TOCStyles.Count - 1].Name)
            {
              this.m_writer.WriteEndElement();
              this.m_writer.WriteEndElement();
            }
          }
          else
          {
            this.m_writer.WriteStartElement("text", "span", (string) null);
            this.m_writer.WriteString((oparagraphItem as OField).Text);
            this.m_writer.WriteEndElement();
          }
          if (oparagraphItem.TextProperties != null && oparagraphItem.TextProperties.CharStyleName != null || oparagraphItem.StyleName != null)
            this.m_writer.WriteEndElement();
        }
        if (oparagraphItem is OBookmarkStart)
        {
          this.m_writer.WriteStartElement("text", "bookmark-start", (string) null);
          this.m_writer.WriteAttributeString("text", "name", (string) null, (oparagraphItem as OBookmarkStart).Name);
          this.m_writer.WriteEndElement();
        }
        if (oparagraphItem is OBookmarkEnd)
        {
          this.m_writer.WriteStartElement("text", "bookmark-end", (string) null);
          this.m_writer.WriteAttributeString("text", "name", (string) null, (oparagraphItem as OBookmarkEnd).Name);
          this.m_writer.WriteEndElement();
        }
      }
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeParagraph(OParagraph para)
  {
    if (para == null)
      return;
    this.m_writer.WriteStartElement("text", "p", (string) null);
    if (para.StyleName != null)
      this.m_writer.WriteAttributeString("text", "style-name", (string) null, para.StyleName);
    if (para.OParagraphItemCollection.Count > 0)
    {
      for (int index = 0; index < para.OParagraphItemCollection.Count; ++index)
      {
        OParagraphItem oparagraphItem = para.OParagraphItemCollection[index];
        if (oparagraphItem != null)
          this.m_writer.WriteString(oparagraphItem.Text);
      }
    }
    this.m_writer.WriteEndElement();
  }

  internal void SerializeExcelBody(List<OTable> tables)
  {
    this.m_writer.WriteStartElement("office", "spreadsheet", (string) null);
    this.SerializeCalculationSettings();
    this.SerializeTables(tables);
    this.m_writer.WriteEndElement();
  }

  internal MemoryStream SerializeStyleStart()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-styles", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "of", (string) null, "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
    this.m_writer.WriteAttributeString("xmlns", "anim", (string) null, "urn:oasis:names:tc:opendocument:xmlns:animation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "chart", (string) null, "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
    this.m_writer.WriteAttributeString("xmlns", "onfig", (string) null, "urn:oasis:names:tc:opendocument:xmlns:config:1.0");
    this.m_writer.WriteAttributeString("xmlns", "db", (string) null, "urn:oasis:names:tc:opendocument:xmlns:database:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dr3d", (string) null, "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
    this.m_writer.WriteAttributeString("xmlns", "form", (string) null, "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
    this.m_writer.WriteAttributeString("xmlns", "meta", (string) null, "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
    this.m_writer.WriteAttributeString("xmlns", "presentation", (string) null, "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "script", (string) null, "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
    this.m_writer.WriteAttributeString("xmlns", "smil", (string) null, "urn:oasis:names:tc:opendocument:xmlns:smil-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "text", (string) null, "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xhtml", (string) null, "http://www.w3.org/1999/xhtml");
    return data;
  }

  internal void SerializeStylesEnd(MemoryStream stream)
  {
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("styles.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  public static string ToReadableString(TimeSpan span)
  {
    return "PT" + $"{(span.TotalHours > 0.0 ? (object) $"{span.TotalHours}{"H"}" : (object) string.Empty)}{(span.TotalMinutes > 0.0 ? (object) $"{span.Minutes}{"M"}" : (object) string.Empty)}{(span.TotalSeconds > 0.0 ? (object) $"{span.Seconds}{"S"}" : (object) string.Empty)}";
  }

  internal void SerializeFontFaceDecls(List<FontFace> fonts)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    if (fonts == null)
      return;
    this.m_writer.WriteStartElement("office", "font-face-decls", (string) null);
    for (int index = 0; index < fonts.Count; ++index)
      this.SerializeFontface(fonts[index]);
    this.m_writer.WriteEndElement();
  }

  internal void SerializeFontface(FontFace font)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    this.m_writer.WriteStartElement("style", "font-face", (string) null);
    this.m_writer.WriteAttributeString("style", "name", (string) null, font.Name);
    this.m_writer.WriteAttributeString("svg", "font-family", (string) null, font.Name.ToString());
    this.m_writer.WriteAttributeString("style", "font-family-generic", (string) null, font.FontFamilyGeneric.ToString());
    this.m_writer.WriteAttributeString("style", "font-pitch", (string) null, font.FontPitch.ToString());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDataStyles(ODFStyleCollection styles)
  {
    this.SerializeCommonStyles(styles);
  }

  internal void SerializeDataStylesStart()
  {
    this.m_writer.WriteStartElement("office", "styles", (string) null);
  }

  internal void SerializeGeneralStyle(NumberStyle style)
  {
    if (style.Number.nFormatFlags == (byte) 0)
      return;
    this.m_writer.WriteStartElement("number", "number", (string) null);
    this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, style.Number.MinIntegerDigits.ToString());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeNumberStyle(DataStyle nFormat)
  {
    switch (nFormat)
    {
      case NumberStyle _:
        NumberStyle numberStyle = nFormat as NumberStyle;
        this.m_writer.WriteStartElement("number", "number-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, numberStyle.Name);
        if (numberStyle.ScientificNumber.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "scientific-number", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.ScientificNumber.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, numberStyle.ScientificNumber.DecimalPlaces.ToString());
          this.m_writer.WriteAttributeString("number", "min-exponent-digits", (string) null, numberStyle.ScientificNumber.MinExponentDigits.ToString());
          this.m_writer.WriteAttributeString("number", "grouping", (string) null, numberStyle.ScientificNumber.Grouping.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        else if (numberStyle.Number.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "number", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.Number.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, numberStyle.Number.DecimalPlaces.ToString());
          this.m_writer.WriteAttributeString("number", "grouping", (string) null, numberStyle.Number.Grouping.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        else if (numberStyle.Fraction.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "fraction", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.Fraction.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "min-numerator-digits", (string) null, numberStyle.Fraction.MinNumeratorDigits.ToString());
          if (numberStyle.Fraction.DenominatorValue > 0)
            this.m_writer.WriteAttributeString("number", "denominator-value", (string) null, numberStyle.Fraction.DenominatorValue.ToString().ToLower());
          else
            this.m_writer.WriteAttributeString("number", "min-denominator-digits", (string) null, numberStyle.Fraction.MinDenominatorDigits.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        this.m_writer.WriteEndElement();
        break;
      case PercentageStyle _:
        PercentageStyle percentageStyle = nFormat as PercentageStyle;
        this.m_writer.WriteStartElement("number", "percentage-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, percentageStyle.Name);
        this.m_writer.WriteStartElement("number", "number", (string) null);
        this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, percentageStyle.Number.MinIntegerDigits.ToString());
        this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, percentageStyle.Number.DecimalPlaces.ToString());
        this.m_writer.WriteAttributeString("number", "grouping", (string) null, percentageStyle.Number.Grouping.ToString().ToLower());
        this.m_writer.WriteEndElement();
        this.m_writer.WriteEndElement();
        break;
      case CurrencyStyle _:
        CurrencyStyle style = nFormat as CurrencyStyle;
        this.m_writer.WriteStartElement("number", "currency-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, style.Name);
        this.m_writer.WriteStartElement("number", "currency-symbol", (string) null);
        this.m_writer.WriteString(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
        this.m_writer.WriteEndElement();
        this.SerializeNumberToken(style);
        if (style.HasSections)
        {
          for (int index = 0; index < style.Map.Count; ++index)
          {
            this.m_writer.WriteStartElement("style", "map", (string) null);
            this.m_writer.WriteAttributeString("style", "condition", (string) null, style.Map[index].Condition);
            this.m_writer.WriteAttributeString("style", "apply-style-name", (string) null, style.Map[index].ApplyStyleName);
            this.m_writer.WriteEndElement();
          }
        }
        this.m_writer.WriteEndElement();
        break;
      case TextStyle _:
        TextStyle textStyle = nFormat as TextStyle;
        this.m_writer.WriteStartElement("number", "text-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, textStyle.Name);
        if (textStyle.TextContent)
          this.m_writer.WriteElementString("number", "text-content", (string) null, string.Empty);
        if (textStyle.HasSections)
        {
          for (int index = 0; index < textStyle.Map.Count; ++index)
          {
            this.m_writer.WriteStartElement("style", "map", (string) null);
            this.m_writer.WriteAttributeString("style", "condition", (string) null, textStyle.Map[index].Condition);
            this.m_writer.WriteAttributeString("style", "apply-style-name", (string) null, textStyle.Map[index].ApplyStyleName);
            this.m_writer.WriteEndElement();
          }
        }
        this.m_writer.WriteEndElement();
        break;
      case Syncfusion.DocIO.ODF.Base.ODFImplementation.DateStyle _:
        Syncfusion.DocIO.ODF.Base.ODFImplementation.DateStyle dateStyle = nFormat as Syncfusion.DocIO.ODF.Base.ODFImplementation.DateStyle;
        this.m_writer.WriteStartElement("number", "date-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, dateStyle.Name);
        this.SerializeDateToken();
        this.m_writer.WriteEndElement();
        break;
    }
  }

  internal void SerializeNumberToken(CurrencyStyle style)
  {
    this.m_writer.WriteStartElement("number", "number", (string) null);
    this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, style.Number.MinIntegerDigits.ToString());
    this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, style.Number.DecimalPlaces.ToString());
    this.m_writer.WriteAttributeString("number", "grouping", (string) null, style.Number.Grouping.ToString().ToLower());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDateToken()
  {
  }

  internal void SerializeCommonStyles(ODFStyleCollection styles)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    this.SerializeODFStyles(styles);
  }

  internal void SerializeODFStyles(ODFStyleCollection ODFStyles)
  {
    this.m_odfStyles = ODFStyles;
    int count = ODFStyles.DictStyles.Values.Count;
    ODFStyle[] array = new ODFStyle[count];
    ODFStyles.DictStyles.Values.CopyTo(array, 0);
    for (int index = 0; index < count; ++index)
    {
      ODFStyle odfStyle = array[index];
      this.m_writer.WriteStartElement("style", "style", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, odfStyle.Name.Replace('_', '-'));
      this.m_writer.WriteAttributeString("style", "family", (string) null, odfStyle.Family.ToString().ToLower().Replace('_', '-'));
      switch (odfStyle.Family)
      {
        case ODFFontFamily.Paragraph:
          if (!string.IsNullOrEmpty(odfStyle.ParentStyleName))
            this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.ParentStyleName);
          if (odfStyle.MasterPageName != null)
            this.m_writer.WriteAttributeString("style", "master-page-name", (string) null, odfStyle.MasterPageName);
          this.SerializeParagraphProperties(odfStyle.ParagraphProperties);
          break;
        case ODFFontFamily.Table:
          this.m_writer.WriteAttributeString("style", "master-page-name", (string) null, odfStyle.MasterPageName);
          this.SerializeTableProperties(odfStyle.TableProperties);
          break;
        case ODFFontFamily.Table_Column:
          this.SerializeColumnProprties(odfStyle.TableColumnProperties);
          break;
        case ODFFontFamily.Table_Row:
          this.SerializeRowProprties(odfStyle.TableRowProperties);
          break;
        case ODFFontFamily.Table_Cell:
          if (!string.IsNullOrEmpty(odfStyle.ParentStyleName))
            this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.ParentStyleName);
          this.m_writer.WriteAttributeString("style", "data-style-name", (string) null, odfStyle.DataStyleName);
          this.SerializeTableCellProperties(odfStyle.TableCellProperties);
          this.SerializeParagraphProperties(odfStyle.ParagraphProperties);
          break;
        case ODFFontFamily.Section:
          this.SerializeSectionProperties(odfStyle.ODFSectionProperties);
          break;
      }
      if (odfStyle.Textproperties != null && odfStyle.Textproperties.CharStyleName != null)
        this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.Textproperties.CharStyleName);
      this.SerializeTextProperties(odfStyle.Textproperties);
      this.m_writer.WriteEndElement();
    }
  }

  internal void SerializeTableDefaultStyle()
  {
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table");
    this.m_writer.WriteStartElement("style", "table-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, "0in");
    this.m_writer.WriteAttributeString("table", "border-model", (string) null, "collapsing");
    this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "lr-tb");
    this.m_writer.WriteAttributeString("table", "align", (string) null, "left");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-column");
    this.m_writer.WriteStartElement("style", "table-column-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "use-optimal-column-width", (string) null, "true");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-row");
    this.m_writer.WriteStartElement("style", "table-row-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "min-row-height", (string) null, "0in");
    this.m_writer.WriteAttributeString("style", "use-optimal-column-height", (string) null, "true");
    this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, "auto");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-cell");
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "background-color", (string) null, "transparent");
    this.m_writer.WriteAttributeString("style", "glyph-orientation-vertical", (string) null, "auto");
    this.m_writer.WriteAttributeString("fo", "vertical-align", (string) null, "top");
    this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDefaultGraphicStyle()
  {
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "graphic");
    this.m_writer.WriteAttributeString("draw", "fill", (string) null, "solid");
    this.m_writer.WriteAttributeString("draw", "fill-color", (string) null, "#5b9bd5");
    this.m_writer.WriteAttributeString("draw", "opacity", (string) null, "100%");
    this.m_writer.WriteAttributeString("draw", "stroke", (string) null, "solid");
    this.m_writer.WriteAttributeString("draw", "stroke-width", (string) null, "0.01389in");
    this.m_writer.WriteAttributeString("svg", "stroke-color", (string) null, "#41719c");
    this.m_writer.WriteAttributeString("draw", "stoke-opacity", (string) null, "100%");
    this.m_writer.WriteAttributeString("draw", "stroke-linejoin", (string) null, "miter");
    this.m_writer.WriteAttributeString("draw", "stroke-linecap", (string) null, "butt");
    this.m_writer.WriteEndElement();
  }

  private void SerializeTableProperties(OTableProperties tableProp)
  {
    this.m_writer.WriteStartElement("style", "table-properties", (string) null);
    if (tableProp != null)
    {
      this.m_writer.WriteAttributeString("style", "width", (string) null, tableProp.TableWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, tableProp.MarginLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("table", "align", (string) null, "left");
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeColumnProprties(OTableColumnProperties tableColumnProp)
  {
    this.m_writer.WriteStartElement("style", "table-column-properties", (string) null);
    if (tableColumnProp != null)
      this.m_writer.WriteAttributeString("style", "column-width", (string) null, tableColumnProp.ColumnWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
    this.m_writer.WriteEndElement();
  }

  private void SerializeRowProprties(OTableRowProperties tableRowProp)
  {
    this.m_writer.WriteStartElement("style", "table-row-properties", (string) null);
    if (tableRowProp != null)
    {
      if (tableRowProp.RowHeight > 0.0)
        this.m_writer.WriteAttributeString("style", "row-height", (string) null, tableRowProp.RowHeight.ToString() + "in");
      this.m_writer.WriteAttributeString("style", "min-row-height", (string) null, tableRowProp.RowHeight.ToString() + "in");
      if (tableRowProp.IsBreakAcrossPages)
        this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, "always");
      if (tableRowProp.IsHeaderRow)
        this.m_writer.WriteAttributeString("style", "use-optimal-row-height", (string) null, "false");
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeSectionProperties(SectionProperties sectionProps)
  {
    if (sectionProps == null)
      return;
    this.m_writer.WriteStartElement("style", "section-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, sectionProps.MarginLeft.ToString() + "in");
    this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, sectionProps.MarginRight.ToString() + "in");
    this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "lr-tb");
    this.m_writer.WriteEndElement();
  }

  private void SerializeParagraphProperties(ODFParagraphProperties paraProp)
  {
    if (paraProp == null)
      return;
    this.m_writer.WriteStartElement("style", "paragraph-properties", (string) null);
    if (paraProp.HasKey(9, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, paraProp.KeepTogether.ToString());
    if (paraProp.HasKey(3, (int) paraProp.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "keep-with-next", (string) null, paraProp.KeepWithNext.ToString());
    if (paraProp.BeforeBreak == BeforeBreak.page)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "page");
    if (paraProp.BeforeBreak == BeforeBreak.column)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "column");
    if (paraProp.HasKey(2, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paraProp.MarginTop.ToString() + "in");
    if (paraProp.HasKey(3, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paraProp.MarginBottom.ToString() + "in");
    if (paraProp.HasKey(0, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paraProp.MarginBottom.ToString() + "in");
    if (paraProp.HasKey(1, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paraProp.MarginRight.ToString() + "in");
    if (paraProp.HasKey(0, (int) paraProp.borderFlags) && paraProp.Border != null)
    {
      this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{paraProp.Border.LineWidth}in {(object) paraProp.Border.LineStyle} {ODFWriter.HexConverter(paraProp.Border.LineColor)}");
      this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paraProp.PaddingLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paraProp.PaddingRight.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paraProp.PaddingTop.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paraProp.PaddingBottom.ToString() + "in");
    }
    if (paraProp.HasKey(1, (int) paraProp.borderFlags) || paraProp.HasKey(2, (int) paraProp.borderFlags) || paraProp.HasKey(3, (int) paraProp.borderFlags) || paraProp.HasKey(4, (int) paraProp.borderFlags))
    {
      if (paraProp.BorderLeft != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{paraProp.BorderLeft.LineWidth}in {(object) paraProp.BorderLeft.LineStyle} {ODFWriter.HexConverter(paraProp.BorderLeft.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paraProp.PaddingLeft.ToString() + "in");
      }
      if (paraProp.BorderRight != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{paraProp.BorderRight.LineWidth}in {(object) paraProp.BorderRight.LineStyle} {ODFWriter.HexConverter(paraProp.BorderRight.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paraProp.PaddingRight.ToString() + "in");
      }
      if (paraProp.BorderTop != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{paraProp.BorderTop.LineWidth}in {(object) paraProp.BorderTop.LineStyle} {ODFWriter.HexConverter(paraProp.BorderTop.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paraProp.PaddingTop.ToString() + "in");
      }
      if (paraProp.BorderBottom != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{paraProp.BorderBottom.LineWidth}in {(object) paraProp.BorderBottom.LineStyle} {ODFWriter.HexConverter(paraProp.BorderBottom.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paraProp.PaddingBottom.ToString() + "in");
      }
    }
    if (paraProp.HasKey(21, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-align", (string) null, paraProp.TextAlign.ToString().ToLower());
    if (paraProp.HasKey(6, (int) paraProp.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, paraProp.BackgroundColor);
    if (paraProp.HasKey(19, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-indent", (string) null, paraProp.TextIndent.ToString() + "in");
    if (paraProp.HasKey(10, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("style", "line-height-at-least", (string) null, paraProp.LineHeightAtLeast.ToString() + "in");
    if (paraProp.HasKey(28, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paraProp.LineHeight.ToString() + "in");
    if (paraProp.HasKey(9, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paraProp.LineSpacing.ToString() + "%");
    if (paraProp.HasKey(0, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paraProp.BeforeSpacing.ToString() + "in");
    if (paraProp.HasKey(1, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paraProp.AfterSpacing.ToString() + "in");
    if (paraProp.HasKey(2, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paraProp.LeftIndent.ToString() + "in");
    if (paraProp.HasKey(3, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paraProp.RightIndent.ToString() + "in");
    if (paraProp.WritingMode == WritingMode.RLTB)
      this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "rl-tb");
    if (paraProp != null && paraProp.TabStops != null && paraProp.TabStops.Count > 0)
    {
      this.m_writer.WriteStartElement("style", "tab-stops", (string) null);
      for (int index = 0; index < paraProp.TabStops.Count; ++index)
      {
        TabStops tabStop = paraProp.TabStops[index];
        this.m_writer.WriteStartElement("style", "tab-stop", (string) null);
        this.m_writer.WriteAttributeString("style", "type", (string) null, tabStop.TextAlignType.ToString());
        this.m_writer.WriteAttributeString("style", "position", (string) null, tabStop.TextPosition.ToString() + "in");
        if (tabStop.TabStopLeader != TabStopLeader.NoLeader)
          this.m_writer.WriteAttributeString("style", "leader-char", (string) null, tabStop.TabStopLeader == TabStopLeader.Dotted ? "." : "");
        this.m_writer.WriteEndElement();
      }
      this.m_writer.WriteEndElement();
    }
    if (paraProp.HasKey(18, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "widows", (string) null, paraProp.Windows.ToString());
    if (paraProp.HasKey(27, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "orphans", (string) null, paraProp.Orphans.ToString());
    this.m_writer.WriteEndElement();
  }

  private void SerializeExcelTableCellProperties(OTableCellProperties cellProp)
  {
    if (cellProp == null)
      return;
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    VerticalAlign? verticalAlign = cellProp.VerticalAlign;
    if (verticalAlign.HasValue)
      this.m_writer.WriteAttributeString("style", "vertical-align", (string) null, verticalAlign.ToString());
    if (cellProp.HasKey(8, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, cellProp.BackColor == Color.Transparent ? cellProp.BackColor.Name.ToString().ToLower() : ODFWriter.HexConverter(cellProp.BackColor));
    if (cellProp.HasKey(7, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "shrink-to-fit", (string) null, true.ToString().ToLower());
    bool flag = false;
    if (cellProp.BorderLeft != null && cellProp.BorderRight != null && cellProp.BorderBottom != null && cellProp.BorderRight != null)
      flag = cellProp.BorderLeft.Equals((object) cellProp.BorderRight) && cellProp.BorderRight.Equals((object) cellProp.BorderTop) && cellProp.BorderTop.Equals((object) cellProp.BorderBottom);
    if (cellProp.Border != null || flag)
    {
      ODFBorder borderLeft = cellProp.BorderLeft;
      if (borderLeft != null && borderLeft.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{borderLeft.LineWidth} {borderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderLeft.LineColor)}");
    }
    else
    {
      if (cellProp.BorderLeft != null)
      {
        ODFBorder borderLeft = cellProp.BorderLeft;
        if (borderLeft != null && borderLeft.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{borderLeft.LineWidth} {borderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderLeft.LineColor)}");
      }
      if (cellProp.BorderRight != null)
      {
        ODFBorder borderRight = cellProp.BorderRight;
        if (borderRight != null && borderRight.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{borderRight.LineWidth} {borderRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderRight.LineColor)}");
      }
      if (cellProp.BorderTop != null)
      {
        ODFBorder borderTop = cellProp.BorderTop;
        if (borderTop != null && borderTop.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{borderTop.LineWidth} {borderTop.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderTop.LineColor)}");
      }
      if (cellProp.BorderBottom != null)
      {
        ODFBorder borderBottom = cellProp.BorderBottom;
        if (borderBottom != null && borderBottom.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{borderBottom.LineWidth} {borderBottom.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderBottom.LineColor)}");
      }
    }
    if (cellProp.DiagonalLeft != null)
    {
      ODFBorder diagonalLeft = cellProp.DiagonalLeft;
      if (diagonalLeft != null && diagonalLeft.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("style", "diagonal-tl-br", (string) null, $"{diagonalLeft.LineWidth} {diagonalLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(diagonalLeft.LineColor)}");
    }
    if (cellProp.DiagonalRight != null)
    {
      ODFBorder diagonalRight = cellProp.DiagonalRight;
      if (diagonalRight != null && diagonalRight.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("style", "diagonal-bl-tr", (string) null, $"{diagonalRight.LineWidth} {diagonalRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(diagonalRight.LineColor)}");
    }
    if (cellProp.HasKey(1, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    if (cellProp.HasKey(0, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "rotation-angle", (string) null, cellProp.RotationAngle.ToString());
    if (cellProp.HasKey(15, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "direction", (string) null, cellProp.Direction.ToString());
    if (cellProp.HasKey(14, (int) cellProp.tableCellFlags) && cellProp.RepeatContent)
      this.m_writer.WriteAttributeString("style", "repeat-content", (string) null, "true");
    this.m_writer.WriteEndElement();
  }

  private void SerializeTableCellProperties(OTableCellProperties cellProp)
  {
    if (cellProp == null)
      return;
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    VerticalAlign? verticalAlign = cellProp.VerticalAlign;
    if (verticalAlign.HasValue)
      this.m_writer.WriteAttributeString("style", "vertical-align", (string) null, verticalAlign.ToString());
    if (cellProp.HasKey(8, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, cellProp.BackColor == Color.Transparent ? cellProp.BackColor.Name.ToString().ToLower() : ODFWriter.HexConverter(cellProp.BackColor));
    if (cellProp.HasKey(7, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "shrink-to-fit", (string) null, true.ToString().ToLower());
    if (cellProp.Border != null && cellProp.Border.LineStyle != BorderLineStyle.none)
    {
      if (cellProp.Border != null)
        this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{cellProp.Border.LineWidth}in {(object) cellProp.Border.LineStyle} {ODFWriter.HexConverter(cellProp.Border.LineColor)}");
      this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, cellProp.PaddingTop.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, cellProp.PaddingBottom.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, cellProp.PaddingLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, cellProp.PaddingRight.ToString() + "in");
    }
    else
    {
      if (cellProp.BorderLeft != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{cellProp.BorderLeft.LineWidth}in {cellProp.BorderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderLeft.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, cellProp.PaddingLeft.ToString() + "in");
      }
      if (cellProp.BorderRight != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{cellProp.BorderRight.LineWidth}in {cellProp.BorderRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderRight.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, cellProp.PaddingRight.ToString() + "in");
      }
      if (cellProp.BorderTop != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{cellProp.BorderTop.LineWidth}in {cellProp.BorderTop.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderTop.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, cellProp.PaddingTop.ToString() + "in");
      }
      if (cellProp.BorderBottom != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{cellProp.BorderBottom.LineWidth}in {cellProp.BorderBottom.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderBottom.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, cellProp.PaddingBottom.ToString() + "in");
      }
    }
    if (cellProp.HasKey(1, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTextProperties(TextProperties txtProp)
  {
    if (txtProp == null)
      return;
    this.m_writer.WriteStartElement("style", "text-properties", (string) null);
    if (txtProp.HasKey(16 /*0x10*/, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "font-name", (string) null, txtProp.FontName);
    if (txtProp.HasKey(17, txtProp.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("fo", "font-size", (string) null, txtProp.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-asian", (string) null, txtProp.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-complex", (string) null, txtProp.FontSize.ToString() + "pt");
    }
    if (txtProp.HasKey(23, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "color", (string) null, ODFWriter.HexConverter(txtProp.Color));
    if (txtProp.HasKey(22, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "font-weight", (string) null, txtProp.FontWeight.ToString());
    if (txtProp.FontStyle == ODFFontStyle.italic)
      this.m_writer.WriteAttributeString("fo", "font-style", (string) null, txtProp.FontStyle.ToString());
    if (txtProp.HasKey(5, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-underline-type", (string) null, txtProp.TextUnderlineType.ToString().ToLower());
    if (txtProp.HasKey(6, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-underline-syle", (string) null, txtProp.TextUnderlineStyle.ToString());
    if (txtProp.HasKey(22, txtProp.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("style", "font-weight-asian", (string) null, txtProp.FontWeightAsian.ToString());
      this.m_writer.WriteAttributeString("style", "font-style-asian", (string) null, txtProp.FontStyleAsian.ToString());
    }
    if (txtProp.HasKey(0, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "font-relief", (string) null, txtProp.FontRelief.ToString());
    if (txtProp.HasKey(20, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, ODFWriter.HexConverter(txtProp.BackgroundColor));
    if (txtProp.HasKey(0, txtProp.m_textFlag3))
      this.m_writer.WriteAttributeString("style", "letter-kerning", (string) null, txtProp.LetterKerning.ToString().ToLower());
    if (txtProp.HasKey(9, txtProp.m_textFlag3))
    {
      this.m_writer.WriteAttributeString("style", "text-line-through-type", (string) null, txtProp.LinethroughType.ToString());
      if (txtProp.LinethroughType != LineType.none)
      {
        this.m_writer.WriteAttributeString("style", "text-line-through-style", (string) null, txtProp.LinethroughStyle.ToString());
        this.m_writer.WriteAttributeString("style", "text-line-through-color", (string) null, txtProp.LinethroughColor);
      }
    }
    if (txtProp.HasKey(6, txtProp.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("style", "text-underline-style", (string) null, txtProp.TextUnderlineStyle.ToString());
      this.m_writer.WriteAttributeString("style", "text-underline-color", (string) null, txtProp.TextUnderlineColor);
      this.m_writer.WriteAttributeString("style", "text-underline-type", (string) null, "single");
    }
    if (txtProp.HasKey(3, txtProp.m_textFlag2) && !txtProp.HasKey(28, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "text-transform", (string) null, txtProp.TextTransform.ToString());
    if (txtProp.HasKey(9, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-scale", (string) null, txtProp.TextScale.ToString() + "%");
    if (txtProp.HasKey(3, txtProp.m_textFlag3))
      this.m_writer.WriteAttributeString("style", "text-outline", (string) null, txtProp.TextOutline.ToString().ToLower());
    if (txtProp.HasKey(27, txtProp.m_textFlag1) && !txtProp.IsTextDisplay)
      this.m_writer.WriteAttributeString("text", "display", (string) null, "none");
    if (txtProp.HasKey(21, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-position", (string) null, txtProp.TextPosition.ToString());
    if (txtProp.HasKey(28, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "font-variant", (string) null, "small-caps");
    this.m_writer.WriteEndElement();
  }

  internal void SerializeAutomaticStyles(PageLayoutCollection layouts)
  {
    this.m_writer.WriteStartElement("office", "automatic-styles", (string) null);
    this.SerializePageLayouts(layouts);
  }

  internal void SerializeAutoStyleStart()
  {
    this.m_writer.WriteStartElement("office", "automatic-styles", (string) null);
  }

  internal void SerializeContentAutoStyles(ODFStyleCollection styles)
  {
    this.SerializeODFStyles(styles);
  }

  internal void SerializeContentListStyles(List<OListStyle> listStyles)
  {
    for (int index = 0; index < listStyles.Count; ++index)
    {
      OListStyle listStyle = listStyles[index];
      this.m_writer.WriteStartElement("text", "list-style", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, listStyle.CurrentStyleName);
      int num = 1;
      foreach (ListLevelProperties listLevel in listStyle.ListLevels)
      {
        this.m_writer.WriteStartElement("text", listLevel.NumberFormat != ListNumberFormat.Bullet || listLevel.IsPictureBullet || listLevel.PictureBullet != null ? (listLevel.PictureBullet != null ? "list-level-style-image" : "list-level-style-number") : "list-level-style-bullet", (string) null);
        this.m_writer.WriteAttributeString("text", "level", (string) null, num.ToString());
        if (listLevel.Style != null && !string.IsNullOrEmpty(listLevel.Style.Name))
          this.m_writer.WriteAttributeString("text", "style-name", (string) null, listLevel.Style.Name.ToString());
        string str = listLevel.NumberFormat == ListNumberFormat.Decimal ? "1" : (listLevel.NumberFormat == ListNumberFormat.LowerLetter ? "a" : (listLevel.NumberFormat == ListNumberFormat.UpperLetter ? "A" : (listLevel.NumberFormat == ListNumberFormat.UpperRoman ? "I" : (listLevel.NumberFormat == ListNumberFormat.LowerRoman ? "i" : ""))));
        if (!string.IsNullOrEmpty(str))
          this.m_writer.WriteAttributeString("style", "num-format", (string) null, str);
        if (listLevel.NumberFormat == ListNumberFormat.Bullet && !string.IsNullOrEmpty(listLevel.BulletCharacter))
          this.m_writer.WriteAttributeString("text", "bullet-char", (string) null, listLevel.BulletCharacter);
        if (listLevel.IsPictureBullet && listLevel.PictureBullet != null && !string.IsNullOrEmpty(listLevel.PictureHRef))
        {
          this.m_writer.WriteAttributeString("xlink", "href", (string) null, listLevel.PictureHRef);
          this.m_writer.WriteAttributeString("xlink", "type", (string) null, "simple");
          this.m_writer.WriteAttributeString("xlink", "show", (string) null, "embed");
          this.m_writer.WriteAttributeString("xlink", "actuate", (string) null, "onLoad");
        }
        if (!string.IsNullOrEmpty(listLevel.NumberSufix))
          this.m_writer.WriteAttributeString("style", "num-suffix", (string) null, listLevel.NumberSufix);
        this.m_writer.WriteAttributeString("style", "num-letter-sync", (string) null, "true");
        this.m_writer.WriteStartElement("style", "list-level-properties", (string) null);
        if (listLevel.TextAlignment != TextAlign.start)
          this.m_writer.WriteAttributeString("fo", "text-align", (string) null, listLevel.TextAlignment.ToString().ToLower());
        this.m_writer.WriteAttributeString("text", "space-before", (string) null, listLevel.SpaceBefore.ToString() + "in");
        this.m_writer.WriteAttributeString("text", "min-label-width", (string) null, listLevel.MinimumLabelWidth.ToString() + "in");
        this.m_writer.WriteAttributeString("text", "list-level-position-and-space-mode", (string) null, "label-alignment");
        this.m_writer.WriteStartElement("style", "list-level-label-alignment", (string) null);
        this.m_writer.WriteAttributeString("text", "label-followed-by", (string) null, "listtab");
        this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, listLevel.LeftMargin.ToString() + "in");
        this.m_writer.WriteAttributeString("fo", "text-indent", (string) null, listLevel.TextIndent.ToString() + "in");
        this.m_writer.WriteEndElement();
        this.m_writer.WriteEndElement();
        if (listLevel.TextProperties != null)
          this.SerializeTextProperties(listLevel.TextProperties);
        this.m_writer.WriteEndElement();
        ++num;
      }
      this.m_writer.WriteEndElement();
    }
  }

  internal void SerializeMasterStyles(MasterPageCollection mPages, List<string> pageNames)
  {
    MasterPage[] array = new MasterPage[mPages.DictMasterPages.Values.Count];
    mPages.DictMasterPages.Values.CopyTo(array, 0);
    for (int index1 = 0; index1 < array.Length; ++index1)
    {
      MasterPage masterPage = array[index1];
      this.m_writer.WriteStartElement("style", "master-page", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, masterPage.Name);
      this.m_writer.WriteAttributeString("style", "page-layout-name", (string) null, masterPage.PageLayoutName);
      if (masterPage.Header != null)
        this.SerializeHeaderFooter(masterPage.Header, true);
      if (masterPage.HeaderLeft != null)
      {
        this.SerializeHeaderLeftStart();
        this.SerializeHeaderFooterContent(masterPage.HeaderLeft);
        this.SerializeEnd();
      }
      if (masterPage.Footer != null)
        this.SerializeHeaderFooter(masterPage.Footer, false);
      if (masterPage.FooterLeft != null)
      {
        this.SerializeFooterLeftStart();
        this.SerializeHeaderFooterContent(masterPage.FooterLeft);
        this.SerializeEnd();
      }
      this.SerializeEnd();
      if (masterPage.FirstPageHeader != null || masterPage.FirstPageFooter != null)
      {
        this.m_writer.WriteStartElement("style", "master-page", (string) null);
        this.m_writer.WriteAttributeString("style", "next-style-name", (string) null, masterPage.Name);
        int index2 = pageNames.IndexOf(masterPage.Name);
        pageNames.RemoveAt(index2);
        masterPage.Name = "MPF" + Regex.Match(masterPage.Name, "\\d+").Value;
        pageNames.Insert(index2, masterPage.Name);
        this.m_writer.WriteAttributeString("style", "name", (string) null, masterPage.Name);
        this.m_writer.WriteAttributeString("style", "page-layout-name", (string) null, masterPage.PageLayoutName);
        if (masterPage.FirstPageHeader != null)
          this.SerializeHeaderFooter(masterPage.FirstPageHeader, true);
        if (masterPage.FirstPageFooter != null)
          this.SerializeHeaderFooter(masterPage.FirstPageFooter, false);
        this.SerializeEnd();
      }
    }
  }

  private void SerializeHeaderFooter(HeaderFooterContent headerFooter, bool isHeader)
  {
    if (isHeader)
      this.SerializeHeaderStart();
    else
      this.SerializeFooterStart();
    this.SerializeHeaderFooterContent(headerFooter);
    this.SerializeEnd();
  }

  internal void SerializeHeaderLeftStart()
  {
    this.m_writer.WriteStartElement("style", "header-left", (string) null);
  }

  internal void SerializeFooterLeftStart()
  {
    this.m_writer.WriteStartElement("style", "footer-left", (string) null);
  }

  internal void SerializeHeaderStart()
  {
    this.m_writer.WriteStartElement("style", "header", (string) null);
  }

  internal void SerializeFooterStart()
  {
    this.m_writer.WriteStartElement("style", "footer", (string) null);
  }

  internal void SerializeMasterStylesStart()
  {
    this.m_writer.WriteStartElement("office", "master-styles", (string) null);
  }

  internal void SerializeEnd() => this.m_writer.WriteEndElement();

  private void SerializePageLayouts(PageLayoutCollection layouts)
  {
    PageLayout[] array = new PageLayout[layouts.DictStyles.Values.Count];
    layouts.DictStyles.Values.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      this.m_writer.WriteStartElement("style", "page-layout", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, array[index].Name);
      this.m_writer.WriteStartElement("style", "page-layout-properties", (string) null);
      PageLayoutProperties layoutProperties = array[index].PageLayoutProperties;
      this.m_writer.WriteAttributeString("fo", "page-width", (string) null, layoutProperties.PageWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "page-height", (string) null, layoutProperties.PageHeight.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      if (layoutProperties.Border != null)
      {
        this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{layoutProperties.Border.LineWidth}in {(object) layoutProperties.Border.LineStyle} {ODFWriter.HexConverter(layoutProperties.Border.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, layoutProperties.PaddingTop.ToString() + "in");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, layoutProperties.PaddingBottom.ToString() + "in");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, layoutProperties.PaddingLeft.ToString() + "in");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, layoutProperties.PaddingRight.ToString() + "in");
      }
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, layoutProperties.MarginTop.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, layoutProperties.MarginBottom.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, layoutProperties.MarginLeft.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, layoutProperties.MarginRight.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("style", "print-orientation", (string) null, layoutProperties.PageOrientation.ToString());
      if (array[index].ColumnsCount > 1)
      {
        this.m_writer.WriteStartElement("style", "columns", (string) null);
        this.m_writer.WriteAttributeString("fo", "column-count", (string) null, array[index].ColumnsCount.ToString());
        this.m_writer.WriteAttributeString("fo", "column-gap", (string) null, array[index].ColumnsGap.ToString());
        this.m_writer.WriteEndElement();
      }
      this.m_writer.WriteEndElement();
      this.SerializeHeaderFooterStyles(array[index]);
      this.m_writer.WriteEndElement();
    }
  }

  private void SerializeHeaderFooterStyles(PageLayout layout)
  {
    HeaderFooterStyle headerStyle = layout.HeaderStyle;
    this.m_writer.WriteStartElement("style", "header-style", (string) null);
    this.SerializeHeaderFooterProperties(headerStyle);
    this.m_writer.WriteEndElement();
    HeaderFooterStyle footerStyle = layout.FooterStyle;
    this.m_writer.WriteStartElement("style", "footer-style", (string) null);
    this.SerializeHeaderFooterProperties(footerStyle);
    this.m_writer.WriteEndElement();
  }

  private void SerializeHeaderFooterProperties(HeaderFooterStyle HFStyle)
  {
    this.m_writer.WriteStartElement("style", "header-footer-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, HFStyle.HeaderFooterproperties.MarginLeft.ToString() + "in");
    this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, HFStyle.HeaderFooterproperties.MarginRight.ToString() + "in");
    if (HFStyle.IsHeader)
    {
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, HFStyle.HeaderFooterproperties.MarginBottom.ToString() + "in");
      if (HFStyle.HeaderDistance != 0.0)
        this.m_writer.WriteAttributeString("fo", "min-height", (string) null, HFStyle.HeaderDistance.ToString() + "in");
    }
    else
    {
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, HFStyle.HeaderFooterproperties.MarginTop.ToString() + "in");
      if (HFStyle.FooterDistance != 0.0)
        this.m_writer.WriteAttributeString("fo", "min-height", (string) null, HFStyle.FooterDistance.ToString() + "in");
    }
    this.m_writer.WriteEndElement();
  }

  private static string HexConverter(Color c)
  {
    return $"#{c.R.ToString("X2")}{c.G.ToString("X2")}{c.B.ToString("X2")}";
  }

  internal void Dispose()
  {
    if (this.m_archieve != null)
    {
      this.m_archieve.Dispose();
      this.m_archieve = (ZipArchive) null;
    }
    if (this.m_writer == null)
      return;
    this.m_writer = (XmlWriter) null;
  }
}
