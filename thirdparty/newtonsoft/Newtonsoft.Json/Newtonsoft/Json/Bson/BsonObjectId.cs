// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

#nullable disable
namespace Newtonsoft.Json.Bson;

[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
public class BsonObjectId
{
  public byte[] Value { get; }

  public BsonObjectId(byte[] value)
  {
    ValidationUtils.ArgumentNotNull((object) value, nameof (value));
    this.Value = value.Length == 12 ? value : throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
  }
}
