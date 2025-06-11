// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.ControllerSourceEnum
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public enum ControllerSourceEnum
{
  NoController = 0,
  NoteOnVelocity = 2,
  NoteOnKeyNumber = 3,
  PolyPressure = 10, // 0x0000000A
  ChannelPressure = 13, // 0x0000000D
  PitchWheel = 14, // 0x0000000E
  PitchWheelSensitivity = 16, // 0x00000010
}
