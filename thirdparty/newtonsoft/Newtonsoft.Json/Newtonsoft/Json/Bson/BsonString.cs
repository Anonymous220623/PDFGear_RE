// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonString
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Bson;

internal class BsonString : BsonValue
{
  public int ByteCount { get; set; }

  public bool IncludeLength { get; }

  public BsonString(object value, bool includeLength)
    : base(value, BsonType.String)
  {
    this.IncludeLength = includeLength;
  }
}
