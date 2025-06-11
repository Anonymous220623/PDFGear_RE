// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CustomXMLPart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CustomXMLPart
{
  private string m_id;
  private string m_xml;
  private WordDocument m_document;

  internal string XML
  {
    get => this.m_xml;
    set => this.m_xml = value;
  }

  internal WordDocument Document => this.m_document;

  public string Id
  {
    get => this.m_id;
    internal set => this.m_id = value;
  }

  internal CustomXMLPart()
  {
  }

  public CustomXMLPart(WordDocument document) => this.m_document = document;

  public void LoadXML(string xml)
  {
    this.XML = xml;
    if (this.Id != null)
      return;
    this.Id = Guid.NewGuid().ToString();
    this.m_document.CustomXmlParts.Add(this.Id, this);
  }

  public void Load(string filePath)
  {
    if (this.Id == null)
    {
      this.Id = Guid.NewGuid().ToString();
      this.m_document.CustomXmlParts.Add(this.Id, this);
    }
    XmlDocument xmlDocument = new XmlDocument();
    StringWriter w1 = new StringWriter();
    XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
    using (XmlTextReader reader = new XmlTextReader(filePath))
    {
      reader.Namespaces = false;
      xmlDocument.Load((XmlReader) reader);
    }
    xmlDocument.WriteTo((XmlWriter) w2);
    this.XML = w1.ToString();
  }

  public void Load(Stream xmlStream)
  {
    if (this.Id == null)
    {
      this.Id = Guid.NewGuid().ToString();
      this.m_document.CustomXmlParts.Add(this.Id, this);
    }
    XmlDocument xmlDocument = new XmlDocument();
    StringWriter w1 = new StringWriter();
    XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
    using (XmlTextReader reader = new XmlTextReader(xmlStream))
    {
      reader.Namespaces = false;
      xmlDocument.Load((XmlReader) reader);
    }
    xmlDocument.WriteTo((XmlWriter) w2);
    this.XML = w1.ToString();
  }

  public void AddNode(
    CustomXMLNode customXmlNode,
    string name,
    CustomXMLNodeType nodeType,
    string nodeValue)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(customXmlNode.OwnerPart.XML);
    XmlNode xmlNode = xmlDocument.SelectSingleNode(customXmlNode.XPath);
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
    node.Value = nodeValue;
    xmlNode.AppendChild(node);
    StringWriter w1 = new StringWriter();
    XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
    xmlDocument.WriteTo((XmlWriter) w2);
    this.XML = w1.ToString();
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
      OwnerPart = this,
      XPath = xPath,
      XML = xmlNode.OuterXml
    };
  }
}
