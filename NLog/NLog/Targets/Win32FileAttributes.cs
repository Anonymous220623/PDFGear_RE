// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Win32FileAttributes
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Targets;

[Flags]
public enum Win32FileAttributes
{
  ReadOnly = 1,
  Hidden = 2,
  System = 4,
  Archive = 32, // 0x00000020
  Device = 64, // 0x00000040
  Normal = 128, // 0x00000080
  Temporary = 256, // 0x00000100
  SparseFile = 512, // 0x00000200
  ReparsePoint = 1024, // 0x00000400
  Compressed = 2048, // 0x00000800
  NotContentIndexed = 8192, // 0x00002000
  Encrypted = 16384, // 0x00004000
  WriteThrough = -2147483648, // 0x80000000
  NoBuffering = 536870912, // 0x20000000
  DeleteOnClose = 67108864, // 0x04000000
  PosixSemantics = 16777216, // 0x01000000
}
