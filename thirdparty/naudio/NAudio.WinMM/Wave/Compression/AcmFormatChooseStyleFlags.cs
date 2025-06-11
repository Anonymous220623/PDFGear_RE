// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatChooseStyleFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave.Compression;

[Flags]
internal enum AcmFormatChooseStyleFlags
{
  None = 0,
  ShowHelp = 4,
  EnableHook = 8,
  EnableTemplate = 16, // 0x00000010
  EnableTemplateHandle = 32, // 0x00000020
  InitToWfxStruct = 64, // 0x00000040
  ContextHelp = 128, // 0x00000080
}
