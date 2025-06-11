// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.StereoToMonoProvider16
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave;

public class StereoToMonoProvider16 : IWaveProvider
{
  private readonly IWaveProvider sourceProvider;
  private byte[] sourceBuffer;

  public StereoToMonoProvider16(IWaveProvider sourceProvider)
  {
    this.LeftVolume = 0.5f;
    this.RightVolume = 0.5f;
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
      throw new ArgumentException("Source must be PCM");
    if (sourceProvider.WaveFormat.Channels != 2)
      throw new ArgumentException("Source must be stereo");
    if (sourceProvider.WaveFormat.BitsPerSample != 16 /*0x10*/)
      throw new ArgumentException("Source must be 16 bit");
    this.sourceProvider = sourceProvider;
    this.WaveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 1);
  }

  public float LeftVolume { get; set; }

  public float RightVolume { get; set; }

  public WaveFormat WaveFormat { get; private set; }

  public int Read(byte[] buffer, int offset, int count)
  {
    int num1 = count * 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    WaveBuffer waveBuffer1 = new WaveBuffer(this.sourceBuffer);
    WaveBuffer waveBuffer2 = new WaveBuffer(buffer);
    int num2 = this.sourceProvider.Read(this.sourceBuffer, 0, num1);
    int num3 = num2 / 2;
    int num4 = offset / 2;
    for (int index = 0; index < num3; index += 2)
    {
      float num5 = (float) ((double) waveBuffer1.ShortBuffer[index] * (double) this.LeftVolume + (double) waveBuffer1.ShortBuffer[index + 1] * (double) this.RightVolume);
      if ((double) num5 > (double) short.MaxValue)
        num5 = (float) short.MaxValue;
      if ((double) num5 < (double) short.MinValue)
        num5 = (float) short.MinValue;
      waveBuffer2.ShortBuffer[num4++] = (short) num5;
    }
    return num2 / 2;
  }
}
