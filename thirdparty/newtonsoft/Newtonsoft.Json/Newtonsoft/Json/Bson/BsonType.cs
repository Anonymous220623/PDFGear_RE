﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonType
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Bson;

internal enum BsonType : sbyte
{
  MinKey = -1, // 0xFF
  Number = 1,
  String = 2,
  Object = 3,
  Array = 4,
  Binary = 5,
  Undefined = 6,
  Oid = 7,
  Boolean = 8,
  Date = 9,
  Null = 10, // 0x0A
  Regex = 11, // 0x0B
  Reference = 12, // 0x0C
  Code = 13, // 0x0D
  Symbol = 14, // 0x0E
  CodeWScope = 15, // 0x0F
  Integer = 16, // 0x10
  TimeStamp = 17, // 0x11
  Long = 18, // 0x12
  MaxKey = 127, // 0x7F
}
