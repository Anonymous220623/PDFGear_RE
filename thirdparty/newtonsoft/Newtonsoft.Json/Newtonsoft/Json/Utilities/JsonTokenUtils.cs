// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.JsonTokenUtils
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Utilities;

internal static class JsonTokenUtils
{
  internal static bool IsEndToken(JsonToken token)
  {
    switch (token)
    {
      case JsonToken.EndObject:
      case JsonToken.EndArray:
      case JsonToken.EndConstructor:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsStartToken(JsonToken token)
  {
    switch (token)
    {
      case JsonToken.StartObject:
      case JsonToken.StartArray:
      case JsonToken.StartConstructor:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveToken(JsonToken token)
  {
    switch (token)
    {
      case JsonToken.Integer:
      case JsonToken.Float:
      case JsonToken.String:
      case JsonToken.Boolean:
      case JsonToken.Null:
      case JsonToken.Undefined:
      case JsonToken.Date:
      case JsonToken.Bytes:
        return true;
      default:
        return false;
    }
  }
}
