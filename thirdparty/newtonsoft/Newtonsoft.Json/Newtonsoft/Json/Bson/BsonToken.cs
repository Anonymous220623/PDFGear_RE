// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonToken
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Bson;

internal abstract class BsonToken
{
  public abstract BsonType Type { get; }

  public BsonToken Parent { get; set; }

  public int CalculatedSize { get; set; }
}
