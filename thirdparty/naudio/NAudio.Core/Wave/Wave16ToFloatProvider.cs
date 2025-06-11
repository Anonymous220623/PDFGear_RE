// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Wave16ToFloatProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave;

public class Wave16ToFloatProvider : IWaveProvider
{
  private IWaveProvider sourceProvider;
  private readonly WaveFormat waveFormat;
  private volatile float volume;
  private byte[] sourceBuffer;

  public Wave16ToFloatProvider(IWaveProvider sourceProvider)
  {
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
      throw new ArgumentException("Only PCM supported");
    if (sourceProvider.WaveFormat.BitsPerSample != 16 /*0x10*/)
      throw new ArgumentException("Only 16 bit audio supported");
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceProvider.WaveFormat.SampleRate, sourceProvider.WaveFormat.Channels);
    this.sourceProvider = sourceProvider;
    this.volume = 1f;
  }

  public int Read(byte[] destBuffer, int offset, int numBytes)
  {
    int num1 = numBytes / 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    int num2 = this.sourceProvider.Read(this.sourceBuffer, offset, num1);
    WaveBuffer waveBuffer1 = new WaveBuffer(this.sourceBuffer);
    WaveBuffer waveBuffer2 = new WaveBuffer(destBuffer);
    int num3 = num2 / 2;
    int num4 = offset / 4;
    for (int index = 0; index < num3; ++index)
      waveBuffer2.FloatBuffer[num4++] = (float) waveBuffer1.ShortBuffer[index] / 32768f * this.volume;
    return num3 * 4;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public float Volume
  {
    get => this.volume;
    set => this.volume = value;
  }
}
