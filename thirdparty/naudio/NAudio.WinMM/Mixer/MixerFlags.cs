// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Mixer;

[Flags]
public enum MixerFlags
{
  Handle = -2147483648, // 0x80000000
  Mixer = 0,
  MixerHandle = Handle, // 0x80000000
  WaveOut = 268435456, // 0x10000000
  WaveOutHandle = WaveOut | MixerHandle, // 0x90000000
  WaveIn = 536870912, // 0x20000000
  WaveInHandle = WaveIn | MixerHandle, // 0xA0000000
  MidiOut = WaveIn | WaveOut, // 0x30000000
  MidiOutHandle = MidiOut | MixerHandle, // 0xB0000000
  MidiIn = 1073741824, // 0x40000000
  MidiInHandle = MidiIn | MixerHandle, // 0xC0000000
  Aux = MidiIn | WaveOut, // 0x50000000
  Value = 0,
  ListText = 1,
  QueryMask = 15, // 0x0000000F
  All = 0,
  OneById = ListText, // 0x00000001
  OneByType = 2,
  GetLineInfoOfDestination = 0,
  GetLineInfoOfSource = OneById, // 0x00000001
  GetLineInfoOfLineId = OneByType, // 0x00000002
  GetLineInfoOfComponentType = GetLineInfoOfLineId | GetLineInfoOfSource, // 0x00000003
  GetLineInfoOfTargetType = 4,
  GetLineInfoOfQueryMask = 15, // 0x0000000F
}
