// Decompiled with JetBrains decompiler
// Type: XmpCore.IXmpSchemaRegistry
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace XmpCore;

public interface IXmpSchemaRegistry
{
  string RegisterNamespace(string namespaceUri, string suggestedPrefix);

  string GetNamespacePrefix(string namespaceUri);

  string GetNamespaceUri(string namespacePrefix);

  IDictionary<string, string> Namespaces { get; }

  IDictionary<string, string> Prefixes { get; }

  void DeleteNamespace(string namespaceUri);

  IXmpAliasInfo ResolveAlias(string aliasNs, string aliasProp);

  IEnumerable<IXmpAliasInfo> FindAliases(string aliasNs);

  IXmpAliasInfo FindAlias(string qname);

  IDictionary<string, IXmpAliasInfo> Aliases { get; }
}
