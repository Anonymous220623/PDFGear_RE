// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DateTimeConverterBase
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json.Converters;

public abstract class DateTimeConverterBase : JsonConverter
{
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof (DateTime) || objectType == typeof (DateTime?) || objectType == typeof (DateTimeOffset) || objectType == typeof (DateTimeOffset?);
  }
}
