// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaPropertyFlags
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;

#nullable disable
namespace NAPS2.Wia;

[Flags]
public enum WiaPropertyFlags
{
  Empty = 0,
  Read = 1,
  Write = 2,
  ReadWrite = Write | Read, // 0x00000003
  None = 8,
  Range = 16, // 0x00000010
  List = 32, // 0x00000020
  Flag = 64, // 0x00000040
  Cacheable = 65536, // 0x00010000
}
