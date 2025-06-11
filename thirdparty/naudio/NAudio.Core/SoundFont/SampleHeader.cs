// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SampleHeader
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class SampleHeader
{
  public string SampleName;
  public uint Start;
  public uint End;
  public uint StartLoop;
  public uint EndLoop;
  public uint SampleRate;
  public byte OriginalPitch;
  public sbyte PitchCorrection;
  public ushort SampleLink;
  public SFSampleLink SFSampleLink;

  public override string ToString() => this.SampleName;
}
