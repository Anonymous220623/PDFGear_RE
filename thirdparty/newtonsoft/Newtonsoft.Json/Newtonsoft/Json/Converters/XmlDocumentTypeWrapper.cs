// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
{
  private readonly XmlDocumentType _documentType;

  public XmlDocumentTypeWrapper(XmlDocumentType documentType)
    : base((XmlNode) documentType)
  {
    this._documentType = documentType;
  }

  public string Name => this._documentType.Name;

  public string System => this._documentType.SystemId;

  public string Public => this._documentType.PublicId;

  public string InternalSubset => this._documentType.InternalSubset;

  public override string? LocalName => "DOCTYPE";
}
