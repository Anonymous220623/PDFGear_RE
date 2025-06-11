// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.ImageCodecFlags
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Data;

[Flags]
internal enum ImageCodecFlags
{
  Encoder = 1,
  Decoder = 2,
  SupportBitmap = 4,
  SupportVector = 8,
  SeekableEncode = 16, // 0x00000010
  BlockingDecode = 32, // 0x00000020
  Builtin = 65536, // 0x00010000
  System = 131072, // 0x00020000
  User = 262144, // 0x00040000
}
