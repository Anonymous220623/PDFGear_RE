﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonSerializationException
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable enable
namespace Newtonsoft.Json;

[Serializable]
public class JsonSerializationException : JsonException
{
  public int LineNumber { get; }

  public int LinePosition { get; }

  public string? Path { get; }

  public JsonSerializationException()
  {
  }

  public JsonSerializationException(string message)
    : base(message)
  {
  }

  public JsonSerializationException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  public JsonSerializationException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }

  public JsonSerializationException(
    string message,
    string path,
    int lineNumber,
    int linePosition,
    Exception? innerException)
    : base(message, innerException)
  {
    this.Path = path;
    this.LineNumber = lineNumber;
    this.LinePosition = linePosition;
  }

  internal static JsonSerializationException Create(JsonReader reader, string message)
  {
    return JsonSerializationException.Create(reader, message, (Exception) null);
  }

  internal static JsonSerializationException Create(
    JsonReader reader,
    string message,
    Exception? ex)
  {
    return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
  }

  internal static JsonSerializationException Create(
    IJsonLineInfo? lineInfo,
    string path,
    string message,
    Exception? ex)
  {
    message = JsonPosition.FormatMessage(lineInfo, path, message);
    int lineNumber;
    int linePosition;
    if (lineInfo != null && lineInfo.HasLineInfo())
    {
      lineNumber = lineInfo.LineNumber;
      linePosition = lineInfo.LinePosition;
    }
    else
    {
      lineNumber = 0;
      linePosition = 0;
    }
    return new JsonSerializationException(message, path, lineNumber, linePosition, ex);
  }
}
