// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaNodeCollection
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.ObjectModel;

#nullable disable
namespace Newtonsoft.Json.Schema;

[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
internal class JsonSchemaNodeCollection : KeyedCollection<string, JsonSchemaNode>
{
  protected override string GetKeyForItem(JsonSchemaNode item) => item.Id;
}
