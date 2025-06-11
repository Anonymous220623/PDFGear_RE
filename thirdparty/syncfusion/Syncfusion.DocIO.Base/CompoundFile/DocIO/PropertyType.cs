// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.PropertyType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

[Flags]
public enum PropertyType
{
  Bool = 11, // 0x0000000B
  Int = 22, // 0x00000016
  Int32 = 3,
  Int16 = 2,
  UInt32 = 19, // 0x00000013
  String = 31, // 0x0000001F
  AsciiString = 30, // 0x0000001E
  DateTime = 64, // 0x00000040
  Blob = 65, // 0x00000041
  Vector = 4096, // 0x00001000
  Object = 12, // 0x0000000C
  Double = 5,
  Empty = 0,
  Null = 1,
  ClipboardData = 71, // 0x00000047
  AsciiStringArray = 4126, // 0x0000101E
  StringArray = AsciiStringArray | Null, // 0x0000101F
  ObjectArray = Object | Vector, // 0x0000100C
}
