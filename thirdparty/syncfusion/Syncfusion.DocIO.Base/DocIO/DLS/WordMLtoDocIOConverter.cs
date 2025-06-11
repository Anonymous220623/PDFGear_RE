// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WordMLtoDocIOConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WordMLtoDocIOConverter
{
  private const string DEF_WORDML_TO_DOCIO_XSLT_RESOURCES = "Syncfusion.DocIO.Resources.wordml-to-dls-converter.xslt";
  private const string DEF_BOOKMARK_START = "Word.Bookmark.Start";
  private const string DEF_BOOKMARK_END = "Word.Bookmark.End";
  private XmlDocument m_xmlWordML = new XmlDocument();
  private WordMLtoDocIOConverter.BookmarkCollection m_bookmarkList = new WordMLtoDocIOConverter.BookmarkCollection();
  private XmlNamespaceManager m_nsmng;

  public IWordDocument Convert(string pathToWordML)
  {
    this.m_xmlWordML.Load(pathToWordML);
    XslTransform xslTransform = new XslTransform();
    xslTransform.Load(WordMLtoDocIOConverter.GetXsltReader(), (XmlResolver) null, (Evidence) null);
    MemoryStream memoryStream = new MemoryStream();
    this.CorrectXML();
    xslTransform.Transform((IXPathNavigable) this.m_xmlWordML, (XsltArgumentList) null, (Stream) memoryStream, (XmlResolver) null);
    memoryStream.Position = 0L;
    IWordDocument wordDocument = (IWordDocument) new WordDocument();
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load((Stream) memoryStream);
    MemoryStream outStream = new MemoryStream((int) memoryStream.Length);
    xmlDocument.Save((Stream) outStream);
    outStream.Position = 0L;
    wordDocument.Open((Stream) outStream, FormatType.Xml);
    return wordDocument;
  }

  private void CorrectXML()
  {
    this.InitNamespaceManager();
    foreach (XmlNode selectNode1 in this.m_xmlWordML.DocumentElement.SelectSingleNode("w:body", this.m_nsmng).SelectNodes("wx:sect", this.m_nsmng))
    {
      foreach (XmlNode selectNode2 in selectNode1.SelectNodes("w:p", this.m_nsmng))
        this.ModifyParagraph(selectNode2);
    }
  }

  private void ModifyParagraph(XmlNode paragraph)
  {
    XmlNodeList xmlNodeList1 = paragraph.SelectNodes("aml:annotation", this.m_nsmng);
    XmlNodeList xmlNodeList2 = paragraph.SelectNodes("w:pict", this.m_nsmng);
    foreach (XmlNode bookmark in xmlNodeList1)
      this.ModifyBookmark(bookmark);
    foreach (XmlNode picture in xmlNodeList2)
      this.ModifyPicture(picture);
  }

  private void ModifyBookmark(XmlNode bookmark)
  {
    string innerText1 = bookmark.Attributes["type", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("w")].InnerText;
    string name = string.Empty;
    if (bookmark.Attributes["name", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("w")] != null)
      name = bookmark.Attributes["name", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("w")].InnerText;
    string innerText2 = bookmark.Attributes["id", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("aml")].InnerText;
    switch (innerText1)
    {
      case "Word.Bookmark.Start":
        this.m_bookmarkList.Add(name);
        break;
      case "Word.Bookmark.End":
        XmlAttribute attribute = this.m_xmlWordML.CreateAttribute("w:name", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("w"));
        attribute.InnerText = this.m_bookmarkList[innerText2];
        bookmark.Attributes.Append(attribute);
        break;
    }
  }

  private void ModifyPicture(XmlNode picture)
  {
    XmlNode node = picture.SelectSingleNode("w:binData", this.m_nsmng);
    XmlNode xmlNode = picture.SelectSingleNode("v:shape", this.m_nsmng);
    Image image = (Image) null;
    if (node != null)
      image = this.ReadImage(node, false);
    double width = (double) image.Width;
    double height = (double) image.Height;
    if (xmlNode == null)
      return;
    XmlAttribute attribute1 = this.m_xmlWordML.CreateAttribute("imgWidth");
    attribute1.InnerText = width.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    xmlNode.Attributes.Append(attribute1);
    XmlAttribute attribute2 = this.m_xmlWordML.CreateAttribute("imgHeight");
    attribute2.InnerText = height.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    xmlNode.Attributes.Append(attribute2);
  }

  private void InitNamespaceManager()
  {
    this.m_nsmng = new XmlNamespaceManager(this.m_xmlWordML.NameTable);
    this.m_nsmng.AddNamespace("w", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("w"));
    this.m_nsmng.AddNamespace("wx", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("wx"));
    this.m_nsmng.AddNamespace("aml", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("aml"));
    this.m_nsmng.AddNamespace("v", this.m_xmlWordML.DocumentElement.GetNamespaceOfPrefix("v"));
  }

  private static XmlReader GetXsltReader()
  {
    return (XmlReader) new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Syncfusion.DocIO.Resources.wordml-to-dls-converter.xslt"));
  }

  private byte[] ReadBinaryElement(XmlNode node)
  {
    XmlTextReader xmlTextReader = new XmlTextReader((TextReader) new StringReader(node.OuterXml));
    xmlTextReader.Read();
    byte[] numArray1 = new byte[0];
    byte[] numArray2 = new byte[1000];
    do
    {
      int length = xmlTextReader.ReadBase64(numArray2, 0, numArray2.Length);
      byte[] destinationArray = new byte[numArray1.Length + length];
      numArray1.CopyTo((Array) destinationArray, 0);
      Array.Copy((Array) numArray2, 0, (Array) destinationArray, numArray1.Length, length);
      numArray1 = destinationArray;
      if (length >= numArray2.Length)
        numArray2 = new byte[numArray1.Length * 2];
      else
        break;
    }
    while (!xmlTextReader.EOF);
    return numArray1;
  }

  private Image ReadImage(XmlNode node, bool isMetaFile)
  {
    byte[] buffer = this.ReadBinaryElement(node);
    Image image = (Image) null;
    if (buffer.Length > 0)
    {
      MemoryStream memoryStream = new MemoryStream(buffer);
      image = !isMetaFile ? (Image) new Bitmap((Stream) memoryStream) : (Image) new Metafile((Stream) memoryStream);
    }
    return image;
  }

  internal class Bookmark
  {
    private string m_strName;
    private string m_strID;

    public string Name
    {
      get => this.m_strName;
      set => this.m_strName = value;
    }

    public string ID
    {
      get => this.m_strID;
      set => this.m_strID = value;
    }
  }

  internal class BookmarkCollection : List<WordMLtoDocIOConverter.Bookmark>
  {
    private int m_iID;

    public string this[string id]
    {
      get
      {
        string str = string.Empty;
        foreach (WordMLtoDocIOConverter.Bookmark bookmark in (List<WordMLtoDocIOConverter.Bookmark>) this)
        {
          if (bookmark.ID == id)
          {
            str = bookmark.Name;
            break;
          }
        }
        return str;
      }
    }

    public int Add(WordMLtoDocIOConverter.Bookmark bookmark)
    {
      bookmark.ID = this.m_iID.ToString();
      ++this.m_iID;
      base.Add(bookmark);
      return this.m_iID - 1;
    }

    public int Add(string name)
    {
      WordMLtoDocIOConverter.Bookmark bookmark = new WordMLtoDocIOConverter.Bookmark();
      bookmark.Name = name;
      bookmark.ID = this.m_iID.ToString();
      ++this.m_iID;
      base.Add(bookmark);
      return this.m_iID - 1;
    }
  }
}
