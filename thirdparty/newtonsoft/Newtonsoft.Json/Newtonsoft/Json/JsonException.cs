// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonException
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable enable
namespace Newtonsoft.Json;

[Serializable]
public class JsonException : Exception
{
  public JsonException()
  {
  }

  public JsonException(string message)
    : base(message)
  {
  }

  public JsonException(string message, Exception? innerException)
    : base(message, innerException)
  {
  }

  public JsonException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }

  internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
  {
    message = JsonPosition.FormatMessage(lineInfo, path, message);
    return new JsonException(message);
  }
}
