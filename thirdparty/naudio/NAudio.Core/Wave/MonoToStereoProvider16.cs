// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.MonoToStereoProvider16
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave;

public class MonoToStereoProvider16 : IWaveProvider
{
  private readonly IWaveProvider sourceProvider;
  private byte[] sourceBuffer;

  public MonoToStereoProvider16(IWaveProvider sourceProvider)
  {
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
      throw new ArgumentException("Source must be PCM");
    if (sourceProvider.WaveFormat.Channels != 1)
      throw new ArgumentException("Source must be Mono");
    if (sourceProvider.WaveFormat.BitsPerSample != 16 /*0x10*/)
      throw new ArgumentException("Source must be 16 bit");
    this.sourceProvider = sourceProvider;
    this.WaveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 2);
    this.RightVolume = 1f;
    this.LeftVolume = 1f;
  }

  public float LeftVolume { get; set; }

  public float RightVolume { get; set; }

  public WaveFormat WaveFormat { get; }

  public int Read(byte[] buffer, int offset, int count)
  {
    int num1 = count / 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    WaveBuffer waveBuffer1 = new WaveBuffer(this.sourceBuffer);
    WaveBuffer waveBuffer2 = new WaveBuffer(buffer);
    int num2 = this.sourceProvider.Read(this.sourceBuffer, 0, num1) / 2;
    int num3 = offset / 2;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      short num4 = waveBuffer1.ShortBuffer[index1];
      short[] shortBuffer1 = waveBuffer2.ShortBuffer;
      int index2 = num3;
      int num5 = index2 + 1;
      int num6 = (int) (short) ((double) this.LeftVolume * (double) num4);
      shortBuffer1[index2] = (short) num6;
      short[] shortBuffer2 = waveBuffer2.ShortBuffer;
      int index3 = num5;
      num3 = index3 + 1;
      int num7 = (int) (short) ((double) this.RightVolume * (double) num4);
      shortBuffer2[index3] = (short) num7;
    }
    return num2 * 4;
  }
}
