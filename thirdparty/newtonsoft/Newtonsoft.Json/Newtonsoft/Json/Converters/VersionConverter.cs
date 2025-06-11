// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.VersionConverter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

#nullable enable
namespace Newtonsoft.Json.Converters;

public class VersionConverter : JsonConverter
{
  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
  {
    if (value == null)
    {
      writer.WriteNull();
    }
    else
    {
      if ((object) (value as Version) == null)
        throw new JsonSerializationException("Expected Version object value");
      writer.WriteValue(value.ToString());
    }
  }

  public override object? ReadJson(
    JsonReader reader,
    Type objectType,
    object? existingValue,
    JsonSerializer serializer)
  {
    if (reader.TokenType == JsonToken.Null)
      return (object) null;
    if (reader.TokenType != JsonToken.String)
      throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing version. Token: {0}, Value: {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
    try
    {
      return (object) new Version((string) reader.Value);
    }
    catch (Exception ex)
    {
      throw JsonSerializationException.Create(reader, "Error parsing version string: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, reader.Value), ex);
    }
  }

  public override bool CanConvert(Type objectType) => objectType == typeof (Version);
}
