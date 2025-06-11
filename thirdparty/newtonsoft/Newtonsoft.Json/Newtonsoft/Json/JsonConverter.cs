// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json;

public abstract class JsonConverter
{
  public abstract void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer);

  public abstract object? ReadJson(
    JsonReader reader,
    Type objectType,
    object? existingValue,
    JsonSerializer serializer);

  public abstract bool CanConvert(Type objectType);

  public virtual bool CanRead => true;

  public virtual bool CanWrite => true;
}
