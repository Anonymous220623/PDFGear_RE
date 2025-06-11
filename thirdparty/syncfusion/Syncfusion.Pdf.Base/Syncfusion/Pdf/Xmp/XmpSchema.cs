// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public abstract class XmpSchema : XmpEntityBase
{
  internal const string c_schemaTagName = "Description";
  private const string c_xPathDescription = "/x:xmpmeta/rdf:RDF/rdf:Description";
  private XmpMetadata m_xmp;
  private static Dictionary<string, XmlNode> customNodeDic = new Dictionary<string, XmlNode>();
  private bool isLoadCustomPrefix;
  private Hashtable m_properties;

  public abstract XmpSchemaType SchemaType { get; }

  protected abstract string Prefix { get; }

  protected abstract string Name { get; }

  protected internal XmpMetadata Xmp => this.m_xmp;

  protected internal XmpSchema(XmpMetadata xmp)
    : base((XmlNode) xmp.Rdf, "rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
  {
    this.m_xmp = xmp != null ? xmp : throw new ArgumentNullException(nameof (xmp));
    this.m_properties = new Hashtable();
    if (this.Prefix == null)
      return;
    this.Initialize();
  }

  protected override void CreateEntity()
  {
    XmlElement element = this.Xmp.CreateElement(this.EntityPrefix, this.EntityName, this.EntityNamespaceURI);
    this.EntityParent.AppendChild((XmlNode) element);
    XmlAttribute attribute1 = this.Xmp.CreateAttribute(this.EntityPrefix, "about", this.EntityNamespaceURI, string.Empty);
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = this.Xmp.CreateAttribute("xmlns:" + this.Prefix, this.Name);
    element.Attributes.Append(attribute2);
    this.Xmp.AddNamespace(this.Prefix, this.Name);
  }

  protected override XmlElement GetEntityXml()
  {
    string xpath = $"./{this.EntityPrefix}:{this.EntityName}";
    if (!this.Xmp.NamespaceManager.HasNamespace(this.EntityPrefix))
      return (XmlElement) null;
    XmlNodeList xmlNodeList = this.EntityParent.SelectNodes(xpath, this.Xmp.NamespaceManager);
    XmlNode entityXml = (XmlNode) null;
    int i = 0;
    for (int count = xmlNodeList.Count; i < count; ++i)
    {
      XmlNode xmlNode = xmlNodeList[i];
      XmlAttribute attribute = xmlNode.Attributes[this.Prefix, "http://www.w3.org/2000/xmlns/"];
      if (attribute == null && this.Prefix != "xmp")
        attribute = xmlNode.Attributes["xmp", "http://www.w3.org/2000/xmlns/"];
      if (attribute != null && attribute.Value.Equals(this.Name))
      {
        entityXml = xmlNode;
        break;
      }
    }
    return entityXml as XmlElement;
  }

  protected XmpSimpleType CreateSimpleProperty(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    return new XmpSimpleType(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name);
  }

  protected XmpSimpleType GetSimpleProperty(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (!(this.m_properties[(object) name] is XmpSimpleType simpleProperty))
    {
      simpleProperty = this.CreateSimpleProperty(name);
      this.m_properties[(object) name] = (object) simpleProperty;
    }
    return simpleProperty;
  }

  protected void RemoveSimplePropertity(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    this.m_properties.Remove((object) name);
  }

  internal void GetCustomPefixNode(string key)
  {
    XmlNodeList xmlNodeList = (XmlNodeList) null;
    string xpath = $"./{this.EntityPrefix}:{this.EntityName}";
    if (this.Xmp.NamespaceManager.HasNamespace(this.EntityPrefix))
      xmlNodeList = this.EntityParent.SelectNodes(xpath, this.Xmp.NamespaceManager);
    int i1 = 0;
    for (int count = xmlNodeList.Count; i1 < count; ++i1)
    {
      XmlNode xmlNode = xmlNodeList[i1];
      if (xmlNode.LastChild != null && xmlNode.LastChild.Prefix != "pdfx" && xmlNode.LastChild.Prefix != "Author" && xmlNode.LastChild.Prefix != "dc" && xmlNode.LastChild.Prefix != "Subject" && xmlNode.LastChild.Prefix != "Trapped" && xmlNode.LastChild.Prefix != "Keywords" && xmlNode.LastChild.Prefix != "Producer" && xmlNode.FirstChild.LocalName != "Creator" && xmlNode.LastChild.LocalName != "ModifyDate" && xmlNode.LastChild.Prefix != "Creator")
      {
        for (int i2 = 0; i2 < xmlNode.ChildNodes.Count; ++i2)
        {
          if (xmlNode.ChildNodes[i2].LocalName.Equals(key))
            XmpSchema.customNodeDic[xmlNode.ChildNodes[i2].Prefix] = xmlNode;
        }
      }
    }
  }

  internal void SetCustomPrefixNode()
  {
    if (this.isLoadCustomPrefix)
      return;
    this.isLoadCustomPrefix = true;
    foreach (KeyValuePair<string, XmlNode> keyValuePair in XmpSchema.customNodeDic)
      this.EntityParent.AppendChild(this.EntityParent.OwnerDocument.ImportNode(keyValuePair.Value, true));
  }

  protected XmpArray CreateArray(string name, XmpArrayType arrayType)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (arrayType == XmpArrayType.Unknown)
      throw new ArgumentException("Wrong array type", nameof (arrayType));
    return new XmpArray(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, arrayType);
  }

  protected XmpArray GetArray(string name, XmpArrayType arrayType)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (!(this.m_properties[(object) name] is XmpArray array))
    {
      array = this.CreateArray(name, arrayType);
      this.m_properties[(object) name] = (object) array;
    }
    return array;
  }

  protected XmpLangArray CreateLangArray(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    return new XmpLangArray(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name);
  }

  protected XmpLangArray GetLangArray(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (!(this.m_properties[(object) name] is XmpLangArray langArray))
    {
      langArray = this.CreateLangArray(name);
      this.m_properties[(object) name] = (object) langArray;
    }
    return langArray;
  }

  protected XmpStructure GetStructure(string name, XmpStructureType type)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (!(this.m_properties[(object) name] is XmpStructure structure))
    {
      structure = this.CreateStructure(name, type);
      this.m_properties[(object) name] = (object) structure;
    }
    return structure;
  }

  protected XmpStructure CreateStructure(string name, XmpStructureType type)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    XmpStructure structure = (XmpStructure) null;
    bool insideArray = name.Length == 0;
    switch (type)
    {
      case XmpStructureType.Dimensions:
        structure = (XmpStructure) new XmpDimensionsStruct(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, insideArray);
        break;
      case XmpStructureType.Font:
        structure = (XmpStructure) new XmpFontStruct(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, insideArray);
        break;
      case XmpStructureType.Colorant:
        structure = (XmpStructure) new XmpColorantStruct(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, insideArray);
        break;
      case XmpStructureType.Thumbnail:
        structure = (XmpStructure) new XmpThumbnailStruct(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, insideArray);
        break;
      case XmpStructureType.Job:
        structure = (XmpStructure) new XmpJobStruct(this.Xmp, (XmlNode) this.XmlData, this.Prefix, name, this.Name, insideArray);
        break;
    }
    return structure;
  }

  public XmpStructure CreateStructure(XmpStructureType type)
  {
    return this.CreateStructure(string.Empty, type);
  }
}
