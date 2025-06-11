// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveProvider32
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave;

public abstract class WaveProvider32 : IWaveProvider, ISampleProvider
{
  private WaveFormat waveFormat;

  public WaveProvider32()
    : this(44100, 1)
  {
  }

  public WaveProvider32(int sampleRate, int channels) => this.SetWaveFormat(sampleRate, channels);

  public void SetWaveFormat(int sampleRate, int channels)
  {
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    WaveBuffer waveBuffer = new WaveBuffer(buffer);
    int sampleCount = count / 4;
    return this.Read(waveBuffer.FloatBuffer, offset / 4, sampleCount) * 4;
  }

  public abstract int Read(float[] buffer, int offset, int sampleCount);

  public WaveFormat WaveFormat => this.waveFormat;
}
