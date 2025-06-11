// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpStructure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public abstract class XmpStructure : XmpType
{
  private Hashtable m_properties;
  private bool m_bInsideArray;
  private bool m_bSuspend = true;
  private bool m_bInitialized;

  protected internal XmlElement InnerXmlData
  {
    get
    {
      return (!this.m_bInsideArray ? this.GetDescriptionElement() : this.XmlData) ?? throw new ArgumentNullException("elm");
    }
  }

  protected abstract string StructurePrefix { get; }

  protected abstract string StructureURI { get; }

  internal XmpStructure(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
    : this(xmp, parent, prefix, localName, namespaceURI, false)
  {
  }

  internal XmpStructure(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI,
    bool insideArray)
    : base(xmp, parent, prefix, localName, namespaceURI)
  {
    this.m_bInsideArray = insideArray;
    this.m_bSuspend = false;
    this.m_properties = new Hashtable();
    this.Initialize();
  }

  protected override void CreateEntity()
  {
    if (this.m_properties == null)
      return;
    this.Xmp.AddNamespace(this.StructurePrefix, this.StructureURI);
    if (!this.m_bInsideArray)
      base.CreateEntity();
    this.CreateStructureContent();
    this.InitializeEntities();
    this.m_bInitialized = true;
  }

  protected override XmlElement GetEntityXml()
  {
    return (this.m_bInsideArray ? this.GetDescriptionElement() : base.GetEntityXml()) ?? throw new ArgumentNullException("elm");
  }

  protected override bool GetSuspend() => this.m_bSuspend;

  protected override bool CheckIfExists()
  {
    bool flag = false;
    if (this.m_bInitialized)
      flag = base.CheckIfExists();
    return flag;
  }

  protected abstract void InitializeEntities();

  protected XmpSimpleType CreateSimpleProperty(string name)
  {
    return name != null ? this.CreateSimpleProperty(name, (XmlNode) this.InnerXmlData) : throw new ArgumentNullException(nameof (name));
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

  protected XmpSimpleType CreateSimpleProperty(string name, XmlNode parent)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    return new XmpSimpleType(this.Xmp, parent, this.StructurePrefix, name, this.StructureURI);
  }

  protected XmpSimpleType GetSimpleProperty(string name, XmlNode parent)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (!(this.m_properties[(object) name] is XmpSimpleType simpleProperty))
    {
      simpleProperty = this.CreateSimpleProperty(name, parent);
      this.m_properties[(object) name] = (object) simpleProperty;
    }
    return simpleProperty;
  }

  protected XmpArray CreateArray(string name, XmpArrayType arrayType)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (arrayType == XmpArrayType.Unknown)
      throw new ArgumentException("Wrong array type", nameof (arrayType));
    return new XmpArray(this.Xmp, (XmlNode) this.InnerXmlData, this.StructurePrefix, name, this.StructureURI, arrayType);
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

  protected void CreateStructureContent()
  {
    XmlNode contentParent = this.GetContentParent();
    XmlElement element = this.Xmp.CreateElement("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    contentParent.AppendChild((XmlNode) element);
    XmlAttribute attribute = this.Xmp.CreateAttribute("xmlns:" + this.StructurePrefix, this.StructureURI);
    element.Attributes.Append(attribute);
  }

  private XmlElement GetDescriptionElement()
  {
    return this.GetContentParent().SelectSingleNode("./rdf:Description", this.Xmp.NamespaceManager) as XmlElement;
  }

  private XmlNode GetContentParent()
  {
    return this.m_bInsideArray ? this.EntityParent : (XmlNode) this.XmlData;
  }
}
