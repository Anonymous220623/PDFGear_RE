// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaException
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Newtonsoft.Json.Schema;

[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
[Serializable]
public class JsonSchemaException : JsonException
{
  public int LineNumber { get; }

  public int LinePosition { get; }

  public string Path { get; }

  public JsonSchemaException()
  {
  }

  public JsonSchemaException(string message)
    : base(message)
  {
  }

  public JsonSchemaException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  public JsonSchemaException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }

  internal JsonSchemaException(
    string message,
    Exception innerException,
    string path,
    int lineNumber,
    int linePosition)
    : base(message, innerException)
  {
    this.Path = path;
    this.LineNumber = lineNumber;
    this.LinePosition = linePosition;
  }
}
