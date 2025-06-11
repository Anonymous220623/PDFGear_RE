// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonValue
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Bson;

internal class BsonValue : BsonToken
{
  private readonly object _value;
  private readonly BsonType _type;

  public BsonValue(object value, BsonType type)
  {
    this._value = value;
    this._type = type;
  }

  public object Value => this._value;

  public override BsonType Type => this._type;
}
