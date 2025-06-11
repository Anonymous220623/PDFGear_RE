// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlDocumentType
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Converters;

internal interface IXmlDocumentType : IXmlNode
{
  string Name { get; }

  string System { get; }

  string Public { get; }

  string InternalSubset { get; }
}
