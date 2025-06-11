// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal interface IXmlNode
{
  XmlNodeType NodeType { get; }

  string? LocalName { get; }

  List<IXmlNode> ChildNodes { get; }

  List<IXmlNode> Attributes { get; }

  IXmlNode? ParentNode { get; }

  string? Value { get; set; }

  IXmlNode AppendChild(IXmlNode newChild);

  string? NamespaceUri { get; }

  object? WrappedNode { get; }
}
