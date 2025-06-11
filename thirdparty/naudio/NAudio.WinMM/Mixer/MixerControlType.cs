// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerControlType
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

#nullable disable
namespace NAudio.Mixer;

public enum MixerControlType
{
  Custom = 0,
  BooleanMeter = 268500992, // 0x10010000
  SignedMeter = 268566528, // 0x10020000
  PeakMeter = 268566529, // 0x10020001
  UnsignedMeter = 268632064, // 0x10030000
  Boolean = 536936448, // 0x20010000
  OnOff = 536936449, // 0x20010001
  Mute = 536936450, // 0x20010002
  Mono = 536936451, // 0x20010003
  Loudness = 536936452, // 0x20010004
  StereoEnhance = 536936453, // 0x20010005
  Button = 553713664, // 0x21010000
  Signed = 805437440, // 0x30020000
  Unsigned = 805502976, // 0x30030000
  Decibels = 805568512, // 0x30040000
  Percent = 805634048, // 0x30050000
  Slider = 1073872896, // 0x40020000
  Pan = 1073872897, // 0x40020001
  QSoundPan = 1073872898, // 0x40020002
  Fader = 1342373888, // 0x50030000
  Volume = 1342373889, // 0x50030001
  Bass = 1342373890, // 0x50030002
  Treble = 1342373891, // 0x50030003
  Equalizer = 1342373892, // 0x50030004
  MicroTime = 1610809344, // 0x60030000
  MilliTime = 1627586560, // 0x61030000
  SingleSelect = 1879113728, // 0x70010000
  Mux = 1879113729, // 0x70010001
  MultipleSelect = 1895890944, // 0x71010000
  Mixer = 1895890945, // 0x71010001
}
