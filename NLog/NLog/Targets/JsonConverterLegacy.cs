// Decompiled with JetBrains decompiler
// Type: NLog.Targets.JsonConverterLegacy
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Text;

#nullable disable
namespace NLog.Targets;

internal class JsonConverterLegacy : IJsonConverter, IJsonSerializer
{
  private readonly IJsonSerializer _jsonSerializer;

  public JsonConverterLegacy(IJsonSerializer jsonSerializer)
  {
    this._jsonSerializer = jsonSerializer;
  }

  public bool SerializeObject(object value, StringBuilder builder)
  {
    string str = this._jsonSerializer.SerializeObject(value);
    if (str == null)
      return false;
    builder.Append(str);
    return true;
  }

  string IJsonSerializer.SerializeObject(object value)
  {
    return this._jsonSerializer.SerializeObject(value);
  }
}
