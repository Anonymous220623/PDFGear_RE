// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonToken
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json;

public enum JsonToken
{
  None,
  StartObject,
  StartArray,
  StartConstructor,
  PropertyName,
  Comment,
  Raw,
  Integer,
  Float,
  String,
  Boolean,
  Null,
  Undefined,
  EndObject,
  EndArray,
  EndConstructor,
  Date,
  Bytes,
}
