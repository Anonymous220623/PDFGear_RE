// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.TrueSpeechWaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class TrueSpeechWaveFormat : WaveFormat
{
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16 /*0x10*/)]
  private short[] unknown;

  public TrueSpeechWaveFormat()
  {
    this.waveFormatTag = WaveFormatEncoding.DspGroupTrueSpeech;
    this.channels = (short) 1;
    this.averageBytesPerSecond = 1067;
    this.bitsPerSample = (short) 1;
    this.blockAlign = (short) 32 /*0x20*/;
    this.sampleRate = 8000;
    this.extraSize = (short) 32 /*0x20*/;
    this.unknown = new short[16 /*0x10*/];
    this.unknown[0] = (short) 1;
    this.unknown[1] = (short) 240 /*0xF0*/;
  }

  public override void Serialize(BinaryWriter writer)
  {
    base.Serialize(writer);
    foreach (short num in this.unknown)
      writer.Write(num);
  }
}
