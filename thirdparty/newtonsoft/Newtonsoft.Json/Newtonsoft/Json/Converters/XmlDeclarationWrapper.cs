// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
{
  private readonly XmlDeclaration _declaration;

  public XmlDeclarationWrapper(XmlDeclaration declaration)
    : base((XmlNode) declaration)
  {
    this._declaration = declaration;
  }

  public string Version => this._declaration.Version;

  public string Encoding
  {
    get => this._declaration.Encoding;
    set => this._declaration.Encoding = value;
  }

  public string Standalone
  {
    get => this._declaration.Standalone;
    set => this._declaration.Standalone = value;
  }
}
