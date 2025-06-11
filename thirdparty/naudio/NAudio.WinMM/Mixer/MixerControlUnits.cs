// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerControlUnits
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Mixer;

[Flags]
internal enum MixerControlUnits
{
  Custom = 0,
  Boolean = 65536, // 0x00010000
  Signed = 131072, // 0x00020000
  Unsigned = Signed | Boolean, // 0x00030000
  Decibels = 262144, // 0x00040000
  Percent = Decibels | Boolean, // 0x00050000
  Mask = 16711680, // 0x00FF0000
}
