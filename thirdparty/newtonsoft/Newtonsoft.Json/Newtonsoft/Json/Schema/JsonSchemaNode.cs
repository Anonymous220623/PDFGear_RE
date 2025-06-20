﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaNode
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Newtonsoft.Json.Schema;

[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
internal class JsonSchemaNode
{
  public string Id { get; }

  public ReadOnlyCollection<JsonSchema> Schemas { get; }

  public Dictionary<string, JsonSchemaNode> Properties { get; }

  public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

  public List<JsonSchemaNode> Items { get; }

  public JsonSchemaNode AdditionalProperties { get; set; }

  public JsonSchemaNode AdditionalItems { get; set; }

  public JsonSchemaNode(JsonSchema schema)
  {
    this.Schemas = new ReadOnlyCollection<JsonSchema>((IList<JsonSchema>) new JsonSchema[1]
    {
      schema
    });
    this.Properties = new Dictionary<string, JsonSchemaNode>();
    this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
    this.Items = new List<JsonSchemaNode>();
    this.Id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>) this.Schemas);
  }

  private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
  {
    this.Schemas = new ReadOnlyCollection<JsonSchema>((IList<JsonSchema>) source.Schemas.Union<JsonSchema>((IEnumerable<JsonSchema>) new JsonSchema[1]
    {
      schema
    }).ToList<JsonSchema>());
    this.Properties = new Dictionary<string, JsonSchemaNode>((IDictionary<string, JsonSchemaNode>) source.Properties);
    this.PatternProperties = new Dictionary<string, JsonSchemaNode>((IDictionary<string, JsonSchemaNode>) source.PatternProperties);
    this.Items = new List<JsonSchemaNode>((IEnumerable<JsonSchemaNode>) source.Items);
    this.AdditionalProperties = source.AdditionalProperties;
    this.AdditionalItems = source.AdditionalItems;
    this.Id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>) this.Schemas);
  }

  public JsonSchemaNode Combine(JsonSchema schema) => new JsonSchemaNode(this, schema);

  public static string GetId(IEnumerable<JsonSchema> schemata)
  {
    return string.Join("-", (IEnumerable<string>) schemata.Select<JsonSchema, string>((Func<JsonSchema, string>) (s => s.InternalId)).OrderBy<string, string>((Func<string, string>) (id => id), (IComparer<string>) StringComparer.Ordinal));
  }
}
