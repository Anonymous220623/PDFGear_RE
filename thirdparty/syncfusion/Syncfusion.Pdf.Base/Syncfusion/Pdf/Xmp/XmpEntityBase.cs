// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpEntityBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public abstract class XmpEntityBase
{
  private XmlNode m_xmlParent;
  private string m_entityPrefix;
  private string m_localName;
  private string m_namespaceURI;

  protected internal XmpEntityBase(
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (localName == null)
      throw new ArgumentNullException(nameof (localName));
    if (!XmlReader.IsName(localName))
      localName = XmlConvert.EncodeName(localName);
    this.m_xmlParent = parent;
    this.m_entityPrefix = prefix;
    this.m_localName = localName;
    this.m_namespaceURI = namespaceURI;
  }

  public XmlElement XmlData => this.GetEntityXml();

  protected internal bool Exists => this.CheckIfExists();

  protected internal XmlNode EntityParent => this.m_xmlParent;

  protected internal string EntityPrefix => this.m_entityPrefix;

  protected internal string EntityName => this.m_localName;

  protected internal string EntityNamespaceURI => this.m_namespaceURI;

  protected bool SuspendInitialization => this.GetSuspend();

  protected virtual void Initialize()
  {
    if (this.SuspendInitialization || this.Exists)
      return;
    this.CreateEntity();
  }

  protected virtual bool CheckIfExists() => this.GetEntityXml() != null;

  protected virtual bool GetSuspend() => false;

  protected abstract void CreateEntity();

  protected abstract XmlElement GetEntityXml();

  internal void SetXmlParent(XmlNode parent)
  {
    this.m_xmlParent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
  }
}
