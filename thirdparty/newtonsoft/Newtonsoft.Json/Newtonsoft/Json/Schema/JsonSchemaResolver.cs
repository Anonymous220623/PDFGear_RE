// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaResolver
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Newtonsoft.Json.Schema;

[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
public class JsonSchemaResolver
{
  public IList<JsonSchema> LoadedSchemas { get; protected set; }

  public JsonSchemaResolver() => this.LoadedSchemas = (IList<JsonSchema>) new List<JsonSchema>();

  public virtual JsonSchema GetSchema(string reference)
  {
    return this.LoadedSchemas.SingleOrDefault<JsonSchema>((Func<JsonSchema, bool>) (s => string.Equals(s.Id, reference, StringComparison.Ordinal))) ?? this.LoadedSchemas.SingleOrDefault<JsonSchema>((Func<JsonSchema, bool>) (s => string.Equals(s.Location, reference, StringComparison.Ordinal)));
  }
}
