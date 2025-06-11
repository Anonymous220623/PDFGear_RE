// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.MixerLineComponentType
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

#nullable disable
namespace NAudio.Mixer;

public enum MixerLineComponentType
{
  DestinationUndefined = 0,
  DestinationDigital = 1,
  DestinationLine = 2,
  DestinationMonitor = 3,
  DestinationSpeakers = 4,
  DestinationHeadphones = 5,
  DestinationTelephone = 6,
  DestinationWaveIn = 7,
  DestinationVoiceIn = 8,
  SourceUndefined = 4096, // 0x00001000
  SourceDigital = 4097, // 0x00001001
  SourceLine = 4098, // 0x00001002
  SourceMicrophone = 4099, // 0x00001003
  SourceSynthesizer = 4100, // 0x00001004
  SourceCompactDisc = 4101, // 0x00001005
  SourceTelephone = 4102, // 0x00001006
  SourcePcSpeaker = 4103, // 0x00001007
  SourceWaveOut = 4104, // 0x00001008
  SourceAuxiliary = 4105, // 0x00001009
  SourceAnalog = 4106, // 0x0000100A
}
