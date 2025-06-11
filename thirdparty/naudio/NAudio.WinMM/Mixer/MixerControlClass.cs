// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerControlClass
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Mixer;

[Flags]
internal enum MixerControlClass
{
  Custom = 0,
  Meter = 268435456, // 0x10000000
  Switch = 536870912, // 0x20000000
  Number = Switch | Meter, // 0x30000000
  Slider = 1073741824, // 0x40000000
  Fader = Slider | Meter, // 0x50000000
  Time = Slider | Switch, // 0x60000000
  List = Time | Meter, // 0x70000000
  Mask = List, // 0x70000000
}
