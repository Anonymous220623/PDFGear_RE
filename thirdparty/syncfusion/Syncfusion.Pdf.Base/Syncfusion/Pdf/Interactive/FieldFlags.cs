// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.FieldFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

[Flags]
internal enum FieldFlags
{
  Default = 0,
  ReadOnly = 1,
  Required = 2,
  NoExport = 4,
  Multiline = 4096, // 0x00001000
  Password = 8192, // 0x00002000
  FileSelect = 1048576, // 0x00100000
  DoNotSpellCheck = 4194304, // 0x00400000
  DoNotScroll = 8388608, // 0x00800000
  Comb = 16777216, // 0x01000000
  RichText = 33554432, // 0x02000000
  NoToggleToOff = 16384, // 0x00004000
  Radio = 32768, // 0x00008000
  PushButton = 65536, // 0x00010000
  RadiosInUnison = RichText, // 0x02000000
  Combo = 131072, // 0x00020000
  Edit = 262144, // 0x00040000
  Sort = 524288, // 0x00080000
  MultiSelect = 2097152, // 0x00200000
  CommitOnSelChange = 67108864, // 0x04000000
}
