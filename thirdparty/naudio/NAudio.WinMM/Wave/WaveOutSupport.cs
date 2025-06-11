// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOutSupport
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave;

[Flags]
internal enum WaveOutSupport
{
  Pitch = 1,
  PlaybackRate = 2,
  Volume = 4,
  LRVolume = 8,
  Sync = 16, // 0x00000010
  SampleAccurate = 32, // 0x00000020
}
