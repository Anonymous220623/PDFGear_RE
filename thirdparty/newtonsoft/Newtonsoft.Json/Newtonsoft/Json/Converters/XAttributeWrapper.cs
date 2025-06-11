// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XAttributeWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XAttributeWrapper(XAttribute attribute) : XObjectWrapper((XObject) attribute)
{
  private XAttribute Attribute => (XAttribute) this.WrappedNode;

  public override string? Value
  {
    get => this.Attribute.Value;
    set => this.Attribute.Value = value;
  }

  public override string? LocalName => this.Attribute.Name.LocalName;

  public override string? NamespaceUri => this.Attribute.Name.NamespaceName;

  public override IXmlNode? ParentNode
  {
    get
    {
      return this.Attribute.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Attribute.Parent);
    }
  }
}
