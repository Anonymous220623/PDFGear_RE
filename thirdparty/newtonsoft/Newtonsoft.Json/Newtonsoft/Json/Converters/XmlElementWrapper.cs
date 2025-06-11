// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlElementWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
{
  private readonly XmlElement _element;

  public XmlElementWrapper(XmlElement element)
    : base((XmlNode) element)
  {
    this._element = element;
  }

  public void SetAttributeNode(IXmlNode attribute)
  {
    this._element.SetAttributeNode((XmlAttribute) ((XmlNodeWrapper) attribute).WrappedNode);
  }

  public string GetPrefixOfNamespace(string namespaceUri)
  {
    return this._element.GetPrefixOfNamespace(namespaceUri);
  }

  public bool IsEmpty => this._element.IsEmpty;
}
