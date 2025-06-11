// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SFSampleLink
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public enum SFSampleLink : ushort
{
  MonoSample = 1,
  RightSample = 2,
  LeftSample = 4,
  LinkedSample = 8,
  RomMonoSample = 32769, // 0x8001
  RomRightSample = 32770, // 0x8002
  RomLeftSample = 32772, // 0x8004
  RomLinkedSample = 32776, // 0x8008
}
