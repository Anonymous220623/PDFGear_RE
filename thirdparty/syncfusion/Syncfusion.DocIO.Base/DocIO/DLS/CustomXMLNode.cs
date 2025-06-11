// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CustomXMLNode
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CustomXMLNode
{
  private IEnumerable<CustomXMLNode> m_childNodes;
  private CustomXMLNode m_firstChild;
  private CustomXMLNode m_lastChild;
  private string m_text;
  private CustomXMLPart m_ownerPart;
  private CustomXMLNode m_parentNode;
  private string m_xml;
  private string m_xPath;

  public string XML
  {
    get => this.m_xml;
    internal set
    {
      this.m_xml = value;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(value);
      this.m_text = xmlDocument.InnerText;
      XmlNode firstChild = xmlDocument.FirstChild;
      this.m_firstChild = new CustomXMLNode();
      this.m_firstChild.m_xml = firstChild.OuterXml;
      this.m_firstChild.OwnerPart = this.OwnerPart;
      this.m_childNodes = this.AddChildNodes(firstChild);
      XmlNode lastChild = xmlDocument.LastChild;
      this.m_lastChild = new CustomXMLNode();
      this.m_lastChild.m_xml = lastChild.OuterXml;
      this.m_lastChild.OwnerPart = this.OwnerPart;
    }
  }

  public IEnumerable<CustomXMLNode> ChildNodes => this.m_childNodes;

  private IEnumerable<CustomXMLNode> AddChildNodes(XmlNode nodes)
  {
    List<CustomXMLNode> customXmlNodeList = (List<CustomXMLNode>) null;
    if (nodes is XmlElement)
    {
      customXmlNodeList = new List<CustomXMLNode>();
      foreach (XmlNode childNode in nodes.ChildNodes)
      {
        if (childNode is XmlElement)
        {
          CustomXMLNode customXmlNode = new CustomXMLNode();
          XmlElement xmlElement = childNode as XmlElement;
          customXmlNode.XML = xmlElement.OuterXml;
          customXmlNodeList.Add(customXmlNode);
        }
      }
    }
    return (IEnumerable<CustomXMLNode>) customXmlNodeList;
  }

  public string XPath
  {
    get => this.m_xPath;
    internal set => this.m_xPath = value;
  }

  public CustomXMLNode FirstChild => this.m_firstChild;

  public CustomXMLNode LastChild => this.m_lastChild;

  public string Text
  {
    get => this.m_text;
    set
    {
      this.m_text = value;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.OwnerPart.XML);
      XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
      xmlNode.InnerText = value;
      StringWriter w1 = new StringWriter();
      XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
      xmlDocument.WriteTo((XmlWriter) w2);
      this.OwnerPart.XML = w1.ToString();
      this.XML = xmlNode.OuterXml;
    }
  }

  public CustomXMLPart OwnerPart
  {
    get => this.m_ownerPart;
    internal set => this.m_ownerPart = value;
  }

  public CustomXMLNode ParentNode
  {
    get
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.OwnerPart.XML);
      XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
      if (xmlNode == null || xmlNode.ParentNode == null)
        return (CustomXMLNode) null;
      CustomXMLNode customXmlNode = new CustomXMLNode()
      {
        OwnerPart = this.OwnerPart,
        XML = xmlNode.ParentNode.OuterXml
      };
      return this.m_parentNode;
    }
  }

  internal CustomXMLNode()
  {
  }

  public void Delete()
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(this.OwnerPart.XML);
    XmlNode oldChild = xmlDocument.SelectSingleNode(this.XPath);
    if (oldChild == null)
      return;
    oldChild.ParentNode.RemoveChild(oldChild);
    StringWriter w1 = new StringWriter();
    XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
    xmlDocument.WriteTo((XmlWriter) w2);
    this.OwnerPart.XML = w1.ToString();
    w2.Close();
    w1.Close();
    this.OwnerPart = (CustomXMLPart) null;
  }

  public CustomXMLNode SelectSingleNode(string xPath)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(this.XML);
    XmlNode xmlNode = xmlDocument.SelectSingleNode(xPath);
    if (xmlNode == null)
      return (CustomXMLNode) null;
    return new CustomXMLNode()
    {
      OwnerPart = this.OwnerPart,
      XPath = xPath,
      XML = xmlNode.OuterXml
    };
  }

  public bool HasChildNodes() => this.m_childNodes.GetEnumerator().MoveNext();

  public void AppendChildNode(string name, CustomXMLNodeType nodeType, string nodeValue)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(this.OwnerPart.XML);
    XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
    XmlNodeType type = XmlNodeType.Element;
    switch (nodeType)
    {
      case CustomXMLNodeType.Element:
        type = XmlNodeType.Element;
        break;
      case CustomXMLNodeType.Attribute:
        type = XmlNodeType.Attribute;
        break;
      case CustomXMLNodeType.Text:
        type = XmlNodeType.Text;
        break;
      case CustomXMLNodeType.Document:
        type = XmlNodeType.Document;
        break;
    }
    XmlNode node = xmlDocument.CreateNode(type, name, "");
    node.InnerText = nodeValue;
    xmlNode.AppendChild(node);
    StringWriter w1 = new StringWriter();
    XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
    xmlNode.WriteTo((XmlWriter) w2);
    this.XML = w1.ToString();
    this.m_text = xmlNode.InnerText;
    StringWriter w3 = new StringWriter();
    XmlTextWriter w4 = new XmlTextWriter((TextWriter) w3);
    xmlDocument.WriteTo((XmlWriter) w4);
    this.OwnerPart.XML = w3.ToString();
    w4.Close();
    w3.Close();
  }

  public void RemoveChild(CustomXMLNode child)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(this.XML);
    XmlNode oldChild = xmlDocument.SelectSingleNode(child.XPath);
    if (oldChild == null)
      return;
    oldChild.ParentNode.RemoveChild(oldChild);
    this.XML = this.m_xml.Replace(oldChild.OuterXml, "");
    this.Text = this.m_text.Replace(oldChild.InnerText, "");
  }
}
