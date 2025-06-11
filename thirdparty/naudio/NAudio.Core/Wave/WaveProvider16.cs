// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveProvider16
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave;

public abstract class WaveProvider16 : IWaveProvider
{
  private WaveFormat waveFormat;

  public WaveProvider16()
    : this(44100, 1)
  {
  }

  public WaveProvider16(int sampleRate, int channels) => this.SetWaveFormat(sampleRate, channels);

  public void SetWaveFormat(int sampleRate, int channels)
  {
    this.waveFormat = new WaveFormat(sampleRate, 16 /*0x10*/, channels);
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    WaveBuffer waveBuffer = new WaveBuffer(buffer);
    int sampleCount = count / 2;
    return this.Read(waveBuffer.ShortBuffer, offset / 2, sampleCount) * 2;
  }

  public abstract int Read(short[] buffer, int offset, int sampleCount);

  public WaveFormat WaveFormat => this.waveFormat;
}
