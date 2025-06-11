// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.ImaAdpcmWaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class ImaAdpcmWaveFormat : WaveFormat
{
  private short samplesPerBlock;

  private ImaAdpcmWaveFormat()
  {
  }

  public ImaAdpcmWaveFormat(int sampleRate, int channels, int bitsPerSample)
  {
    this.waveFormatTag = WaveFormatEncoding.DviAdpcm;
    this.sampleRate = sampleRate;
    this.channels = (short) channels;
    this.bitsPerSample = (short) bitsPerSample;
    this.extraSize = (short) 2;
    this.blockAlign = (short) 0;
    this.averageBytesPerSecond = 0;
    this.samplesPerBlock = (short) 0;
  }
}
