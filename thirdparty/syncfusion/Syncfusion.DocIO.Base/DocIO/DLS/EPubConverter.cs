// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EPubConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EPubConverter
{
  private ZipArchive m_archieve;
  private WordDocument m_document;
  private Dictionary<string, Stream> m_embeddedFiles;
  private Dictionary<string, string> m_bookmarks;
  private string m_fileName;
  private string m_uid;
  private string m_title;
  private string m_author;
  private int m_previous;
  private int m_playOrder = 1;
  private WPicture m_coverImage;

  internal WPicture CoverImage
  {
    get => this.m_coverImage;
    set => this.m_coverImage = value;
  }

  internal string FileName
  {
    get => this.m_fileName;
    set => this.m_fileName = value;
  }

  public EPubConverter()
  {
    this.m_uid = Guid.NewGuid().ToString();
    this.m_previous = 0;
    this.m_archieve = new ZipArchive();
    this.m_embeddedFiles = new Dictionary<string, Stream>();
  }

  public void ConvertToEPub(string fileName, WordDocument document)
  {
    this.m_document = document;
    this.ConvertToEPub();
    this.Save(fileName);
  }

  public void ConvertToEPub(Stream stream, WordDocument document)
  {
    this.m_document = document;
    this.ConvertToEPub();
    this.Save(stream);
  }

  private void ConvertToEPub()
  {
    this.WriteMIME();
    this.GenerateOPS();
    this.GenerateOPF();
    this.GenerateNCX();
    this.GenerateContainer();
  }

  private void Save(string fileName)
  {
    this.m_archieve.Save(fileName);
    this.m_archieve.Close();
    this.m_archieve.Dispose();
  }

  private void Save(Stream stream)
  {
    this.m_archieve.Save(stream, false);
    this.m_archieve.Close();
    this.m_archieve.Dispose();
  }

  private void GenerateContainer()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    xmlTextWriter.WriteStartDocument();
    xmlTextWriter.WriteStartElement("container", "urn:oasis:names:tc:opendocument:xmlns:container");
    xmlTextWriter.WriteAttributeString("version", "1.0");
    xmlTextWriter.WriteStartElement("rootfiles");
    xmlTextWriter.WriteStartElement("rootfile");
    xmlTextWriter.WriteAttributeString("full-path", "content.opf");
    xmlTextWriter.WriteAttributeString("media-type", "application/oebps-package+xml");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.Flush();
    this.m_archieve.AddItem("META-INF\\container.xml", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void GenerateNCX()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter writer = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartDocument();
    writer.WriteDocType("ncx", "-//NISO//DTD ncx 2005-1//EN", "http://www.daisy.org/z3986/2005/ncx-2005-1.dtd", (string) null);
    writer.WriteStartElement("ncx", "http://www.daisy.org/z3986/2005/ncx/");
    writer.WriteAttributeString("version", "2005-1");
    writer.WriteStartElement("head");
    writer.WriteStartElement("meta");
    writer.WriteAttributeString("name", $"{"dtb"}:{"uid"}");
    writer.WriteAttributeString("content", this.m_uid);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("docTitle");
    writer.WriteStartElement("text");
    writer.WriteValue(this.m_title);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("docAuthor");
    writer.WriteStartElement("text");
    writer.WriteValue(this.m_author);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("navMap");
    writer.WriteStartElement("navPoint");
    writer.WriteAttributeString("id", Path.GetFileNameWithoutExtension(this.m_fileName));
    writer.WriteAttributeString("playOrder", this.m_playOrder++.ToString());
    writer.WriteStartElement("navLabel");
    writer.WriteStartElement("text");
    writer.WriteValue(this.m_title);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("content");
    writer.WriteAttributeString("src", this.m_fileName);
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (this.m_bookmarks != null && this.m_bookmarks.Count > 0)
    {
      int num = 0;
      foreach (string key in this.m_bookmarks.Keys)
      {
        string[] strArray = key.Split(';');
        num = int.Parse(strArray[0]);
        if (num >= this.m_previous)
        {
          if (num == this.m_previous)
            writer.WriteEndElement();
          while (num > ++this.m_previous)
            this.WriteBookmark($"{(object) this.m_previous};{strArray[1]}", "", (XmlWriter) writer);
          this.WriteBookmark(key, this.m_bookmarks[key], (XmlWriter) writer);
        }
        else
        {
          while (num <= this.m_previous--)
            writer.WriteEndElement();
          this.WriteBookmark(key, this.m_bookmarks[key], (XmlWriter) writer);
        }
        this.m_previous = num;
      }
      while (num-- != 0)
        writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.Flush();
    this.m_archieve.AddItem("toc.ncx", (Stream) memoryStream, true, FileAttributes.Archive);
    this.GenerateTOCNavigationalHTML();
  }

  private void GenerateTOCNavigationalHTML()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    xmlTextWriter.WriteStartDocument();
    xmlTextWriter.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", (string) null);
    xmlTextWriter.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
    xmlTextWriter.WriteStartElement("head");
    xmlTextWriter.WriteRaw("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />");
    xmlTextWriter.WriteRaw($"<title>{"TOC"}</title>");
    xmlTextWriter.WriteRaw("<style type=\"text/css\"></style>");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("body");
    xmlTextWriter.WriteRaw("<p class=\"title\">Table of contents</p>");
    if (this.m_bookmarks != null && this.m_bookmarks.Count > 0)
    {
      foreach (string key in this.m_bookmarks.Keys)
      {
        string[] strArray = key.Split(';');
        int.Parse(strArray[0]);
        xmlTextWriter.WriteStartElement("p");
        xmlTextWriter.WriteStartElement("a");
        xmlTextWriter.WriteAttributeString("href", $"{this.m_fileName}#{strArray[1]}");
        xmlTextWriter.WriteValue(this.m_bookmarks[key]);
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
      }
    }
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.Flush();
    this.m_archieve.AddItem("toc.html", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void WriteBookmark(string key, string value, XmlWriter writer)
  {
    string[] strArray = key.Split(';');
    string empty = string.Empty;
    string str = !string.IsNullOrEmpty(value) ? $"level{strArray[0]}_{strArray[1]}" : $"level{strArray[0]}_{"unknownLevel"}";
    writer.WriteStartElement("navPoint");
    writer.WriteAttributeString("id", str);
    writer.WriteAttributeString("playOrder", this.m_playOrder++.ToString());
    writer.WriteStartElement("navLabel");
    writer.WriteStartElement("text");
    if (string.IsNullOrEmpty(value))
    {
      value = "***";
      --this.m_playOrder;
    }
    writer.WriteValue(value);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("content");
    writer.WriteAttributeString("src", $"{this.m_fileName}#{strArray[1]}");
    writer.WriteEndElement();
  }

  private void GenerateOPF()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter writer = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartDocument();
    writer.WriteStartElement("package", "http://www.idpf.org/2007/opf");
    writer.WriteAttributeString("version", "2.0");
    writer.WriteAttributeString("unique-identifier", "guid");
    writer.WriteAttributeString("xmlns", "xsi", (string) null, "http://www.w3.org/2001/XMLSchema-instance");
    writer.WriteStartElement("metadata");
    writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    writer.WriteAttributeString("xmlns", "opf", (string) null, "http://www.idpf.org/2007/opf");
    writer.WriteStartElement("dc", "title", "http://purl.org/dc/elements/1.1/");
    writer.WriteValue(this.m_title);
    writer.WriteEndElement();
    writer.WriteStartElement("dc", "creator", "http://purl.org/dc/elements/1.1/");
    this.m_author = string.IsNullOrEmpty(this.m_document.BuiltinDocumentProperties.Author) ? "Administrator" : this.m_document.BuiltinDocumentProperties.Author;
    writer.WriteValue(this.m_author);
    writer.WriteEndElement();
    writer.WriteStartElement("dc", "identifier", "http://purl.org/dc/elements/1.1/");
    writer.WriteAttributeString("id", "guid");
    writer.WriteValue(this.m_uid);
    writer.WriteEndElement();
    writer.WriteStartElement("dc", "language", "http://purl.org/dc/elements/1.1/");
    writer.WriteAttributeString("xsi", "type", (string) null, $"{"dcterms"}:{"RFC3066"}");
    writer.WriteValue("en-US");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("manifest");
    writer.WriteStartElement("item");
    writer.WriteAttributeString("id", "ncx");
    writer.WriteAttributeString("href", "toc.ncx");
    writer.WriteAttributeString("media-type", "application/x-dtbncx+xml");
    writer.WriteEndElement();
    if (this.CoverImage != null)
    {
      writer.WriteStartElement("item");
      writer.WriteAttributeString("id", "cover.html");
      writer.WriteAttributeString("href", "cover.html");
      writer.WriteAttributeString("media-type", "application/xhtml+xml");
      writer.WriteEndElement();
    }
    writer.WriteStartElement("item");
    writer.WriteAttributeString("id", Path.GetFileNameWithoutExtension(this.m_fileName).Replace(' ', '_'));
    writer.WriteAttributeString("href", this.m_fileName);
    writer.WriteAttributeString("media-type", "application/xhtml+xml");
    writer.WriteEndElement();
    writer.WriteStartElement("item");
    writer.WriteAttributeString("id", "toc.html");
    writer.WriteAttributeString("href", "toc.html");
    writer.WriteAttributeString("media-type", "application/xhtml+xml");
    writer.WriteEndElement();
    this.WriteEmbeddedFiles(writer);
    writer.WriteEndElement();
    writer.WriteStartElement("spine");
    writer.WriteAttributeString(Path.GetFileNameWithoutExtension("toc.ncx"), "ncx");
    writer.WriteStartElement("itemref");
    writer.WriteAttributeString("idref", Path.GetFileNameWithoutExtension(this.m_fileName).Replace(' ', '_'));
    writer.WriteEndElement();
    foreach (string key in this.m_embeddedFiles.Keys)
    {
      if (Path.GetExtension(key) == ".html")
      {
        writer.WriteStartElement("itemref");
        writer.WriteAttributeString("idref", Path.GetFileNameWithoutExtension(key));
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.Flush();
    this.m_archieve.AddItem("content.opf", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void WriteEmbeddedFiles(XmlTextWriter writer)
  {
    if (this.m_embeddedFiles == null || this.m_embeddedFiles.Count <= 0)
      return;
    foreach (string key in this.m_embeddedFiles.Keys)
    {
      writer.WriteStartElement("item");
      writer.WriteAttributeString("id", Path.GetFileNameWithoutExtension(key));
      writer.WriteAttributeString("href", key.Replace('\\', '/'));
      writer.WriteAttributeString("media-type", this.GetFormat(Path.GetExtension(key)));
      writer.WriteEndElement();
    }
  }

  private void GenerateOPS()
  {
    HTMLExport htmlExport = new HTMLExport();
    htmlExport.CacheFilesInternally = true;
    htmlExport.HasNavigationId = true;
    htmlExport.HasOEBHeaderFooter = true;
    this.m_document.SaveOptions.HtmlExportCssStyleSheetType = CssStyleSheetType.External;
    this.m_document.SaveOptions.HtmlExportCssStyleSheetFileName = "styles.css";
    this.m_document.SaveOptions.HtmlExportTextInputFormFieldAsText = true;
    if (this.m_document.SaveOptions.EPubExportFont)
    {
      try
      {
        this.EmbedFontFiles();
      }
      catch (SecurityException ex)
      {
        throw new NotSupportedException("Embedding font files is not supported in medium trust");
      }
    }
    this.UpdateTitle();
    this.m_fileName += ".html";
    MemoryStream data = new MemoryStream();
    htmlExport.SaveAsXhtml(this.m_document, (Stream) data, true);
    data.Flush();
    this.GenerateCoverPage();
    this.m_archieve.AddItem(this.m_fileName, (Stream) data, true, FileAttributes.Archive);
    if (htmlExport.EmbeddedStyleSheet != null)
      this.m_embeddedFiles.Add(this.m_document.SaveOptions.HtmlExportCssStyleSheetFileName, htmlExport.EmbeddedStyleSheet);
    if (htmlExport.EmbeddedImages != null && htmlExport.EmbeddedImages.Count > 0)
    {
      foreach (string key in htmlExport.EmbeddedImages.Keys)
        this.m_embeddedFiles.Add(key, htmlExport.EmbeddedImages[key]);
    }
    if (this.CoverImage != null)
      this.m_embeddedFiles.Add("images/cover.png", (Stream) new MemoryStream(this.CoverImage.ImageBytes));
    foreach (string key in this.m_embeddedFiles.Keys)
      this.m_archieve.AddItem(key, this.m_embeddedFiles[key], true, FileAttributes.Archive);
    if (htmlExport.Bookmarks == null || htmlExport.Bookmarks.Count <= 0)
      return;
    this.m_bookmarks = htmlExport.Bookmarks;
  }

  private void GenerateCoverPage()
  {
    if (this.CoverImage == null)
      return;
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    xmlTextWriter.WriteStartDocument();
    xmlTextWriter.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", (string) null);
    xmlTextWriter.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
    xmlTextWriter.WriteStartElement("head");
    xmlTextWriter.WriteRaw("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />");
    xmlTextWriter.WriteRaw($"<title>{"Cover"}</title>");
    xmlTextWriter.WriteRaw("<style type=\"text/css\"></style>");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("body");
    xmlTextWriter.WriteStartElement("p");
    xmlTextWriter.WriteRaw($"<img height=\"{(object) ((double) this.CoverImage.Height * 1.33)}\" width=\"{(object) ((double) this.CoverImage.Width * 1.33)}\" src=\"../Images/cover.png\" />");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.Flush();
    this.m_archieve.AddItem("cover.html", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void EmbedFontFiles()
  {
    List<Font> usedFontNames = this.m_document.UsedFontNames;
    SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
    foreach (Font font in usedFontNames)
    {
      if (!(font.Name == "Times New Roman"))
      {
        bool bold = font.Bold;
        bool italic = font.Italic;
        string str1 = (bold ? "b" : "") + (italic ? "a" : "");
        string key = $"{font.Name.ToLower().Replace(" ", string.Empty)}{str1}.ttf";
        if (!this.m_embeddedFiles.ContainsKey(key))
        {
          IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
          IntPtr hfont = font.ToHfont();
          IntPtr hgdiobj = GdiApi.SelectObject(dc, hfont);
          uint fontData1 = GdiApi.GetFontData(dc, 0U, 0U, (byte[]) null, 0U);
          byte[] numArray = new byte[(IntPtr) fontData1];
          int fontData2 = (int) GdiApi.GetFontData(dc, 0U, 0U, numArray, fontData1);
          GdiApi.SelectObject(dc, hgdiobj);
          GdiApi.DeleteObject(hfont);
          GdiApi.DeleteDC(dc);
          MemoryStream memoryStream = new MemoryStream(numArray, 0, numArray.Length, false);
          this.m_embeddedFiles.Add(key, (Stream) memoryStream);
          string str2 = (italic ? "font-style:italic; " : "") + (bold ? "font-weight:bold; " : "");
          sortedDictionary.Add(key, $"@font-face {{ font-family:'{font.Name}'; {str2}src:url('{key}') }}");
        }
      }
    }
    if (sortedDictionary.Count <= 0)
      return;
    this.m_document.SaveOptions.FontFiles = new string[sortedDictionary.Count];
    sortedDictionary.Values.CopyTo(this.m_document.SaveOptions.FontFiles, 0);
  }

  private void UpdateTitle()
  {
    string empty1 = string.Empty;
    this.m_title = string.IsNullOrEmpty(this.m_document.BuiltinDocumentProperties.Title) ? "untitled" : this.m_document.BuiltinDocumentProperties.Title;
    this.m_fileName = string.IsNullOrEmpty(this.m_fileName) ? this.m_title : this.m_fileName;
    if (this.m_fileName == "untitled")
      return;
    Encoding ascii = Encoding.ASCII;
    byte[] bytes = ascii.GetBytes(this.m_fileName);
    this.m_fileName = ascii.GetString(bytes);
    string str = this.m_fileName.Trim('?', ' ').Replace("?", string.Empty);
    if (str == string.Empty)
    {
      this.m_fileName = "untitled";
    }
    else
    {
      this.m_fileName = str;
      string empty2 = string.Empty;
      foreach (char c in this.m_fileName.ToCharArray())
      {
        if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '_' || c == '-' || c == '&')
          empty2 += (string) (object) c;
      }
      this.m_fileName = empty2;
    }
  }

  private void WriteMIME()
  {
    this.m_archieve.AddItem(new ZipArchiveItem(this.m_archieve, "mimetype", (Stream) null, true, FileAttributes.Normal)
    {
      CompressionMethod = CompressionMethod.Stored
    });
    this.m_archieve.UpdateItem("mimetype", Encoding.Default.GetBytes("application/epub+zip"));
  }

  private string GetFormat(string extension)
  {
    string format = string.Empty;
    switch (extension)
    {
      case ".png":
        format = "image/png";
        break;
      case ".jpeg":
      case ".jpg":
        format = "image/jpeg";
        break;
      case ".css":
        format = "text/css";
        break;
      case ".ttf":
        format = "application/octet-stream";
        break;
    }
    return format;
  }
}
