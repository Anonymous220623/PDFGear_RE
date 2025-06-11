// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonEmpty
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Bson;

internal class BsonEmpty : BsonToken
{
  public static readonly BsonToken Null = (BsonToken) new BsonEmpty(BsonType.Null);
  public static readonly BsonToken Undefined = (BsonToken) new BsonEmpty(BsonType.Undefined);

  private BsonEmpty(BsonType type) => this.Type = type;

  public override BsonType Type { get; }
}
