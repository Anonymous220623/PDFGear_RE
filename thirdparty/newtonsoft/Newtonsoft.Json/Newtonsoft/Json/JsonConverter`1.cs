// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter`1
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

#nullable enable
namespace Newtonsoft.Json;

public abstract class JsonConverter<T> : JsonConverter
{
  public sealed override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
  {
    if ((value != null ? (value is T ? 1 : 0) : (ReflectionUtils.IsNullable(typeof (T)) ? 1 : 0)) == 0)
      throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
    this.WriteJson(writer, (T) value, serializer);
  }

  public abstract void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer);

  public sealed override object? ReadJson(
    JsonReader reader,
    Type objectType,
    object? existingValue,
    JsonSerializer serializer)
  {
    bool flag = existingValue == null;
    if (!flag && !(existingValue is T))
      throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
    return (object) this.ReadJson(reader, objectType, flag ? default (T) : (T) existingValue, !flag, serializer);
  }

  public abstract T? ReadJson(
    JsonReader reader,
    Type objectType,
    T? existingValue,
    bool hasExistingValue,
    JsonSerializer serializer);

  public sealed override bool CanConvert(Type objectType)
  {
    return typeof (T).IsAssignableFrom(objectType);
  }
}
