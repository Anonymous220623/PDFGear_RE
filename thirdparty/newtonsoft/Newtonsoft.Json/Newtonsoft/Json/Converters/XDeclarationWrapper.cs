// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml;
using System.Xml.Linq;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
{
  internal XDeclaration Declaration { get; }

  public XDeclarationWrapper(XDeclaration declaration)
    : base((XObject) null)
  {
    this.Declaration = declaration;
  }

  public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

  public string Version => this.Declaration.Version;

  public string Encoding
  {
    get => this.Declaration.Encoding;
    set => this.Declaration.Encoding = value;
  }

  public string Standalone
  {
    get => this.Declaration.Standalone;
    set => this.Declaration.Standalone = value;
  }
}
