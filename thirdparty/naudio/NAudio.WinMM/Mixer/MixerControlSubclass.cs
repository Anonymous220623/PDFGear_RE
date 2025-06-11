// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerControlSubclass
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Mixer;

[Flags]
internal enum MixerControlSubclass
{
  SwitchBoolean = 0,
  SwitchButton = 16777216, // 0x01000000
  MeterPolled = 0,
  TimeMicrosecs = 0,
  TimeMillisecs = SwitchButton, // 0x01000000
  ListSingle = 0,
  ListMultiple = TimeMillisecs, // 0x01000000
  Mask = 251658240, // 0x0F000000
}
