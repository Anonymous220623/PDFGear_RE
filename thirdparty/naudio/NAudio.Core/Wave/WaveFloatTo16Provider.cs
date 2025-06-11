// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFloatTo16Provider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave;

public class WaveFloatTo16Provider : IWaveProvider
{
  private readonly IWaveProvider sourceProvider;
  private readonly WaveFormat waveFormat;
  private volatile float volume;
  private byte[] sourceBuffer;

  public WaveFloatTo16Provider(IWaveProvider sourceProvider)
  {
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Input wave provider must be IEEE float", nameof (sourceProvider));
    if (sourceProvider.WaveFormat.BitsPerSample != 32 /*0x20*/)
      throw new ArgumentException("Input wave provider must be 32 bit", nameof (sourceProvider));
    this.waveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 16 /*0x10*/, sourceProvider.WaveFormat.Channels);
    this.sourceProvider = sourceProvider;
    this.volume = 1f;
  }

  public int Read(byte[] destBuffer, int offset, int numBytes)
  {
    int num1 = numBytes * 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    int num2 = this.sourceProvider.Read(this.sourceBuffer, 0, num1);
    WaveBuffer waveBuffer1 = new WaveBuffer(this.sourceBuffer);
    WaveBuffer waveBuffer2 = new WaveBuffer(destBuffer);
    int num3 = num2 / 4;
    int num4 = offset / 2;
    for (int index = 0; index < num3; ++index)
    {
      float num5 = waveBuffer1.FloatBuffer[index] * this.volume;
      if ((double) num5 > 1.0)
        num5 = 1f;
      if ((double) num5 < -1.0)
        num5 = -1f;
      waveBuffer2.ShortBuffer[num4++] = (short) ((double) num5 * (double) short.MaxValue);
    }
    return num3 * 2;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public float Volume
  {
    get => this.volume;
    set => this.volume = value;
  }
}
