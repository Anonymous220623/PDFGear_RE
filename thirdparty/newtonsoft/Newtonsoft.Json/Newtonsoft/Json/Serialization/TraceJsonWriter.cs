﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.TraceJsonWriter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.IO;
using System.Numerics;

#nullable enable
namespace Newtonsoft.Json.Serialization;

internal class TraceJsonWriter : JsonWriter
{
  private readonly JsonWriter _innerWriter;
  private readonly JsonTextWriter _textWriter;
  private readonly StringWriter _sw;

  public TraceJsonWriter(JsonWriter innerWriter)
  {
    this._innerWriter = innerWriter;
    this._sw = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
    this._sw.Write("Serialized JSON: " + Environment.NewLine);
    this._textWriter = new JsonTextWriter((TextWriter) this._sw);
    this._textWriter.Formatting = Formatting.Indented;
    this._textWriter.Culture = innerWriter.Culture;
    this._textWriter.DateFormatHandling = innerWriter.DateFormatHandling;
    this._textWriter.DateFormatString = innerWriter.DateFormatString;
    this._textWriter.DateTimeZoneHandling = innerWriter.DateTimeZoneHandling;
    this._textWriter.FloatFormatHandling = innerWriter.FloatFormatHandling;
  }

  public string GetSerializedJsonMessage() => this._sw.ToString();

  public override void WriteValue(Decimal value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(Decimal? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(bool value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(bool? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(byte value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(byte? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(char value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(char? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(byte[]? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value == null)
      base.WriteUndefined();
    else
      base.WriteValue(value);
  }

  public override void WriteValue(DateTime value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(DateTime? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(DateTimeOffset value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(DateTimeOffset? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(double value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(double? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteUndefined()
  {
    this._textWriter.WriteUndefined();
    this._innerWriter.WriteUndefined();
    base.WriteUndefined();
  }

  public override void WriteNull()
  {
    this._textWriter.WriteNull();
    this._innerWriter.WriteNull();
    base.WriteUndefined();
  }

  public override void WriteValue(float value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(float? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(Guid value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(Guid? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(int value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(int? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(long value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(long? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(object? value)
  {
    if (value is BigInteger)
    {
      this._textWriter.WriteValue(value);
      this._innerWriter.WriteValue(value);
      this.InternalWriteValue(JsonToken.Integer);
    }
    else
    {
      this._textWriter.WriteValue(value);
      this._innerWriter.WriteValue(value);
      if (value == null)
        base.WriteUndefined();
      else
        this.InternalWriteValue(JsonToken.String);
    }
  }

  public override void WriteValue(sbyte value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(sbyte? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(short value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(short? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(string? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(TimeSpan value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(TimeSpan? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(uint value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(uint? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(ulong value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(ulong? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteValue(Uri? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value == (Uri) null)
      base.WriteUndefined();
    else
      base.WriteValue(value);
  }

  public override void WriteValue(ushort value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    base.WriteValue(value);
  }

  public override void WriteValue(ushort? value)
  {
    this._textWriter.WriteValue(value);
    this._innerWriter.WriteValue(value);
    if (value.HasValue)
      base.WriteValue(value.GetValueOrDefault());
    else
      base.WriteUndefined();
  }

  public override void WriteWhitespace(string ws)
  {
    this._textWriter.WriteWhitespace(ws);
    this._innerWriter.WriteWhitespace(ws);
    base.WriteWhitespace(ws);
  }

  public override void WriteComment(string? text)
  {
    this._textWriter.WriteComment(text);
    this._innerWriter.WriteComment(text);
    base.WriteComment(text);
  }

  public override void WriteStartArray()
  {
    this._textWriter.WriteStartArray();
    this._innerWriter.WriteStartArray();
    base.WriteStartArray();
  }

  public override void WriteEndArray()
  {
    this._textWriter.WriteEndArray();
    this._innerWriter.WriteEndArray();
    base.WriteEndArray();
  }

  public override void WriteStartConstructor(string name)
  {
    this._textWriter.WriteStartConstructor(name);
    this._innerWriter.WriteStartConstructor(name);
    base.WriteStartConstructor(name);
  }

  public override void WriteEndConstructor()
  {
    this._textWriter.WriteEndConstructor();
    this._innerWriter.WriteEndConstructor();
    base.WriteEndConstructor();
  }

  public override void WritePropertyName(string name)
  {
    this._textWriter.WritePropertyName(name);
    this._innerWriter.WritePropertyName(name);
    base.WritePropertyName(name);
  }

  public override void WritePropertyName(string name, bool escape)
  {
    this._textWriter.WritePropertyName(name, escape);
    this._innerWriter.WritePropertyName(name, escape);
    base.WritePropertyName(name);
  }

  public override void WriteStartObject()
  {
    this._textWriter.WriteStartObject();
    this._innerWriter.WriteStartObject();
    base.WriteStartObject();
  }

  public override void WriteEndObject()
  {
    this._textWriter.WriteEndObject();
    this._innerWriter.WriteEndObject();
    base.WriteEndObject();
  }

  public override void WriteRawValue(string? json)
  {
    this._textWriter.WriteRawValue(json);
    this._innerWriter.WriteRawValue(json);
    this.InternalWriteValue(JsonToken.Undefined);
  }

  public override void WriteRaw(string? json)
  {
    this._textWriter.WriteRaw(json);
    this._innerWriter.WriteRaw(json);
    base.WriteRaw(json);
  }

  public override void Close()
  {
    this._textWriter.Close();
    this._innerWriter.Close();
    base.Close();
  }

  public override void Flush()
  {
    this._textWriter.Flush();
    this._innerWriter.Flush();
  }
}
