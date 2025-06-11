// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocIOtoWordMLConverter
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
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DocIOtoWordMLConverter
{
  private const string DEF_DOCIO_XSLT_RESOURCES = "Syncfusion.DocIO.Resources.dls-to-wordml-converter.xslt";
  private const string DEF_SECTIONS = "sections";
  private const string DEF_PATH_FORMAT = "{0}/{1}";
  private const string DEF_SECTION = "section";
  private const string DEF_BODY = "body";
  private const string DEF_PARAGRAPHS = "paragraphs";
  private const string DEF_PARAGRAPH = "paragraph";
  private const string DEF_BUILTIN_PROPERTIES = "builtin-properties";
  private const string DEF_PAGE_SETUP = "page-setup";
  private const string DEF_COLUMNS = "columns";
  private const string DEF_HEADERS_FOOTERS = "headers-footers";
  private const string DEF_ITEMS = "items";
  private const string DEF_ITEM = "item";
  private const string DEF_IMAGE_NAME_FORMAT = "wordml://{0}_{1}.png";
  private const string DEF_BREAKCODE = "BreakCode";
  private const string DEF_BREAKCODE_VALUE_NOBREAK = "NoBreak";
  private const string DEF_BREAKCODE_VALUE_NEWPAGE = "NewPage";
  private const string DEF_ATTRIBUTE_VALUE_TRUE = "True";
  private const string DEF_TYPE_PARAMETR = "type";
  private const string DEF_ITEM_TYPE_TABLE = "Table";
  private const string DEF_ITEM_TYPE_PICTURE = "Picture";
  private const string DEF_ITEM_TYPE_BOOKMARKSTART = "BookmarkStart";
  private const string DEF_ITEM_TYPE_BOOKMARKEND = "BookmarkEnd";
  private bool m_bNewPage;
  private int[] m_continue;
  private bool m_bBreakBefore;
  private int m_imageCount;
  private DocIOtoWordMLConverter.BookmarkCollection m_bookmarkList = new DocIOtoWordMLConverter.BookmarkCollection();
  private XmlDocument m_outXml = new XmlDocument();
  private WordDocument m_wordDoc;

  public void Convert(IWordDocument doc, string pathToWordML)
  {
    XslTransform xslTransform = new XslTransform();
    xslTransform.Load(DocIOtoWordMLConverter.GetXsltReader(), (XmlResolver) null, (Evidence) null);
    MemoryStream inStream = new MemoryStream();
    this.m_wordDoc = (WordDocument) doc;
    doc.Save((Stream) inStream, FormatType.Xml);
    inStream.Position = 0L;
    this.m_outXml.Load((Stream) inStream);
    this.CorrectDlsXml();
    XmlTextWriter output = new XmlTextWriter(pathToWordML, Encoding.UTF8);
    xslTransform.Transform((IXPathNavigable) this.m_outXml, (XsltArgumentList) null, (XmlWriter) output, (XmlResolver) null);
    output.Close();
  }

  private static XmlReader GetXsltReader()
  {
    return (XmlReader) new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Syncfusion.DocIO.Resources.dls-to-wordml-converter.xslt"));
  }

  private void CorrectDlsXml()
  {
    this.ModifySections();
    this.ModifyBuiltinProperties();
  }

  private void ModifySections()
  {
    XmlNodeList xmlNodeList = this.m_outXml.DocumentElement.SelectNodes($"{"sections"}/{"section"}");
    int index1 = 0;
    this.m_continue = new int[xmlNodeList.Count];
    foreach (XmlNode xmlNode in xmlNodeList)
    {
      this.m_continue[index1] = 0;
      if (xmlNode.Attributes["BreakCode"] != null && xmlNode.Attributes["BreakCode"].InnerText == "NoBreak")
        this.m_continue[index1] = 1;
      ++index1;
    }
    int index2 = 1;
    foreach (XmlNode node in xmlNodeList)
    {
      this.ModifySection(node, xmlNodeList.Count, index2);
      ++index2;
    }
  }

  private void ModifySection(XmlNode node, int count, int index)
  {
    if (node.Attributes["BreakCode"] != null)
      this.m_bNewPage = node.Attributes["BreakCode"].InnerText == "NewPage";
    if (this.m_continue[index - 1] == 1)
      this.m_bNewPage = true;
    if (count == 1 || index == count)
      this.m_bNewPage = false;
    XmlAttribute attribute1 = this.m_outXml.CreateAttribute("PropInEndPar");
    attribute1.InnerText = this.m_bNewPage.ToString();
    node.Attributes.Append(attribute1);
    XmlNode xmlNode = node.SelectSingleNode("body").SelectSingleNode("paragraphs");
    XmlNode oldChild1 = node.SelectSingleNode("page-setup");
    XmlNode oldChild2 = node.SelectSingleNode("columns");
    XmlNode oldChild3 = node.SelectSingleNode("headers-footers");
    if (xmlNode != null)
    {
      foreach (XmlNode childNode in xmlNode.ChildNodes)
        this.ModifyParagraph(childNode);
    }
    if (!this.m_bNewPage)
      return;
    XmlElement element = this.m_outXml.CreateElement("item");
    XmlAttribute attribute2 = this.m_outXml.CreateAttribute("type");
    attribute2.InnerText = "Paragraph";
    XmlAttribute attribute3 = this.m_outXml.CreateAttribute("SctPrp");
    attribute3.InnerText = "True";
    element.Attributes.Append(attribute2);
    element.Attributes.Append(attribute3);
    if (this.m_continue[index - 1] == 1)
      element.SetAttribute("Continue", "True");
    element.AppendChild(oldChild1.Clone());
    element.AppendChild(oldChild3.Clone());
    if (oldChild2 != null)
    {
      element.AppendChild(oldChild2.Clone());
      oldChild2.ParentNode.RemoveChild(oldChild2);
    }
    xmlNode.AppendChild((XmlNode) element);
    oldChild1.ParentNode.RemoveChild(oldChild1);
    oldChild3.ParentNode.RemoveChild(oldChild3);
  }

  private void ModifyParagraph(XmlNode node)
  {
    if (node.ChildNodes.Count > 0)
    {
      XmlNode xmlNode = node.SelectSingleNode("paragraph-format");
      if (xmlNode != null && xmlNode.Attributes.Count > 0 && xmlNode.Attributes["PageBreakAfter"] != null)
        this.m_bBreakBefore = xmlNode.Attributes["PageBreakAfter"].InnerText == "True".ToLower();
    }
    if (node.Attributes["type"].Value == "Table")
      this.ModifyTable(node);
    XmlNode node1 = node.SelectSingleNode("items");
    if (node1 != null)
      this.ModifyItems(node1);
    if (!this.m_bBreakBefore)
      return;
    XmlAttribute attribute = this.m_outXml.CreateAttribute("BreakBefore");
    attribute.InnerText = "True";
    node.Attributes.Append(attribute);
    this.m_bBreakBefore = false;
  }

  private void ModifyTable(XmlNode table)
  {
    DocIOtoWordMLConverter.TableGrid tableGrid = new DocIOtoWordMLConverter.TableGrid();
    tableGrid.Parce(table);
    tableGrid.Save();
  }

  private void ModifyItems(XmlNode node)
  {
    foreach (XmlNode selectNode in node.SelectNodes("item"))
    {
      if (selectNode.Attributes["type"] != null)
      {
        switch (selectNode.Attributes["type"].InnerText)
        {
          case "Picture":
            this.ModifyPicture(selectNode);
            continue;
          case "BookmarkStart":
            int num = this.m_bookmarkList.Add(selectNode.Attributes["BookmarkName"].InnerText);
            XmlAttribute attribute1 = this.m_outXml.CreateAttribute("bookmarkID");
            attribute1.InnerText = num.ToString();
            selectNode.Attributes.Append(attribute1);
            continue;
          case "BookmarkEnd":
            string bookmark = this.m_bookmarkList[selectNode.Attributes["BookmarkName"].InnerText];
            XmlAttribute attribute2 = this.m_outXml.CreateAttribute("bookmarkID");
            attribute2.InnerText = bookmark.ToString();
            selectNode.Attributes.Append(attribute2);
            continue;
          case "Break":
            this.m_bBreakBefore = true;
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void ModifyBuiltinProperties()
  {
    XmlNode xmlNode1 = this.m_outXml.DocumentElement.SelectSingleNode("builtin-properties");
    XmlAttribute attribute1 = xmlNode1.Attributes["EditTime"];
    if (xmlNode1.Attributes["DocSecurity"] == null || !(xmlNode1.Attributes["DocSecurity"].InnerText == 8.ToString()))
      return;
    XmlNode xmlNode2 = this.m_outXml.SelectSingleNode("DLS");
    XmlAttribute attribute2 = this.m_outXml.CreateAttribute("ProtectionType");
    attribute2.InnerText = "AllowOnlyComments";
    xmlNode2.Attributes.Append(attribute2);
  }

  private void ModifyPicture(XmlNode node)
  {
    NumberFormatInfo provider = new NumberFormatInfo();
    provider.CurrencyDecimalSeparator = ".";
    double num1 = 0.0;
    double num2 = 0.0;
    XmlNode node1 = node.SelectSingleNode("image");
    bool isMetaFile = node.Attributes["IsMetafile"].InnerText == "True".ToLower();
    XmlAttribute attribute1 = this.m_outXml.CreateAttribute("Name");
    attribute1.InnerText = $"wordml://{this.m_imageCount}_{(isMetaFile ? (object) "m" : (object) "o")}.png";
    node.Attributes.Append(attribute1);
    string empty = string.Empty;
    Image image = (Image) null;
    if (node1 != null)
      image = this.ReadImage(node1, isMetaFile);
    if (node.Attributes["width"] != null)
      num1 = System.Convert.ToDouble(node.Attributes["width"].InnerText, (IFormatProvider) provider);
    if (node.Attributes["WidthScale"] != null && !isMetaFile)
      num1 = System.Convert.ToDouble(node.Attributes["WidthScale"].InnerText, (IFormatProvider) provider) / 100.0 * (double) image.Width;
    string str1 = empty + $"width: {num1};";
    if (node.Attributes["height"] != null)
      num2 = System.Convert.ToDouble(node.Attributes["height"].InnerText, (IFormatProvider) provider);
    if (node.Attributes["HeightScale"] != null && !isMetaFile)
      num2 = System.Convert.ToDouble(node.Attributes["HeightScale"].InnerText, (IFormatProvider) provider) / 100.0 * (double) image.Height;
    string str2 = str1 + $"height: {num2}";
    XmlAttribute attribute2 = this.m_outXml.CreateAttribute("style");
    attribute2.InnerText = str2;
    node.Attributes.Append(attribute2);
    ++this.m_imageCount;
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

  internal class BookmarkCollection : List<DocIOtoWordMLConverter.Bookmark>
  {
    private int m_iID;

    public string this[string name]
    {
      get
      {
        string str = string.Empty;
        foreach (DocIOtoWordMLConverter.Bookmark bookmark in (List<DocIOtoWordMLConverter.Bookmark>) this)
        {
          if (bookmark.Name == name)
          {
            str = bookmark.ID;
            break;
          }
        }
        return str;
      }
    }

    public int Add(DocIOtoWordMLConverter.Bookmark bookmark)
    {
      bookmark.ID = this.m_iID.ToString();
      ++this.m_iID;
      base.Add(bookmark);
      return this.m_iID - 1;
    }

    public int Add(string name)
    {
      DocIOtoWordMLConverter.Bookmark bookmark = new DocIOtoWordMLConverter.Bookmark();
      bookmark.Name = name;
      bookmark.ID = this.m_iID.ToString();
      ++this.m_iID;
      base.Add(bookmark);
      return this.m_iID - 1;
    }
  }

  internal class Grid
  {
    private double m_dWidth;

    public double Width
    {
      get => this.m_dWidth;
      set => this.m_dWidth = value;
    }

    public Grid()
    {
    }

    public Grid(double width) => this.m_dWidth = width;
  }

  internal class GridList : List<DocIOtoWordMLConverter.Grid>
  {
    public int Add(DocIOtoWordMLConverter.Grid grid)
    {
      base.Add(grid);
      return this.Count - 1;
    }

    public int Add(double width) => this.Add(new DocIOtoWordMLConverter.Grid(width));

    public double GetMin(double end)
    {
      double num = 0.0;
      foreach (DocIOtoWordMLConverter.Grid grid in (List<DocIOtoWordMLConverter.Grid>) this)
      {
        num += grid.Width;
        if (num > end)
          return num - end;
      }
      return 0.0;
    }

    public int GetCollCount(double start, double end)
    {
      int collCount = 1;
      double num = 0.0;
      foreach (DocIOtoWordMLConverter.Grid grid in (List<DocIOtoWordMLConverter.Grid>) this)
      {
        num += grid.Width;
        if (end == num)
          return collCount;
        if (num > start)
          ++collCount;
      }
      return collCount - 1;
    }
  }

  internal class RowList : List<DocIOtoWordMLConverter.GridList>
  {
    public int Add(DocIOtoWordMLConverter.GridList gridlist)
    {
      int count = gridlist.Count;
      base.Add(gridlist);
      return count;
    }
  }

  internal class TableGrid
  {
    private DocIOtoWordMLConverter.GridList tableGrid = new DocIOtoWordMLConverter.GridList();
    private DocIOtoWordMLConverter.RowList rowList = new DocIOtoWordMLConverter.RowList();
    private XmlNode m_table;
    private NumberFormatInfo m_provider = new NumberFormatInfo();

    public TableGrid() => this.m_provider.CurrencyDecimalSeparator = ".";

    public void Parce(XmlNode table)
    {
      this.m_table = table;
      XmlNodeList xmlNodeList1 = this.m_table.SelectNodes($"{"rows"}/{"row"}");
      if (xmlNodeList1.Count <= 0)
        return;
      foreach (XmlNode xmlNode1 in xmlNodeList1)
      {
        XmlNodeList xmlNodeList2 = xmlNode1.SelectNodes($"{"cells"}/{"cell"}");
        if (xmlNodeList2.Count > 0)
        {
          DocIOtoWordMLConverter.GridList gridlist = new DocIOtoWordMLConverter.GridList();
          foreach (XmlNode xmlNode2 in xmlNodeList2)
            gridlist.Add(System.Convert.ToDouble(xmlNode2.Attributes["Width"].InnerText, (IFormatProvider) this.m_provider));
          this.rowList.Add(gridlist);
        }
      }
    }

    public void Save()
    {
      this.ParceTableGrid();
      XmlElement element1 = this.m_table.OwnerDocument.CreateElement("tblGrid");
      foreach (DocIOtoWordMLConverter.Grid grid in (List<DocIOtoWordMLConverter.Grid>) this.tableGrid)
      {
        XmlElement element2 = this.m_table.OwnerDocument.CreateElement("gridCol");
        element2.SetAttribute("w", (grid.Width * 20.0).ToString());
        element1.AppendChild((XmlNode) element2);
      }
      this.m_table.AppendChild((XmlNode) element1);
      XmlNodeList xmlNodeList1 = this.m_table.SelectNodes($"{"rows"}/{"row"}");
      if (xmlNodeList1.Count <= 0)
        return;
      foreach (XmlNode xmlNode1 in xmlNodeList1)
      {
        XmlNodeList xmlNodeList2 = xmlNode1.SelectNodes($"{"cells"}/{"cell"}");
        double end = 0.0;
        if (xmlNodeList2.Count > 0)
        {
          foreach (XmlNode xmlNode2 in xmlNodeList2)
          {
            double num = System.Convert.ToDouble(xmlNode2.Attributes["Width"].InnerText, (IFormatProvider) this.m_provider);
            double start = end;
            end += num;
            int collCount = this.tableGrid.GetCollCount(start, end);
            if (collCount > 1)
            {
              XmlAttribute attribute = xmlNode2.OwnerDocument.CreateAttribute("collcount");
              attribute.InnerText = collCount.ToString();
              xmlNode2.Attributes.Append(attribute);
            }
          }
        }
      }
    }

    private void ParceTableGrid()
    {
      double end = 0.0;
      if (this.rowList.Count <= 0)
        return;
      bool flag = true;
      while (flag)
      {
        double width = double.MaxValue;
        foreach (DocIOtoWordMLConverter.GridList row in (List<DocIOtoWordMLConverter.GridList>) this.rowList)
        {
          double min = row.GetMin(end);
          width = width > min ? min : width;
        }
        if (width == 0.0)
          break;
        end += width;
        this.tableGrid.Add(width);
      }
    }

    private void AddRow(DocIOtoWordMLConverter.GridList row) => this.rowList.Add(row);
  }
}
