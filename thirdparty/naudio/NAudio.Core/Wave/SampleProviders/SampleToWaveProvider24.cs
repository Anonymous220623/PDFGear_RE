// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SampleToWaveProvider24
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SampleToWaveProvider24 : IWaveProvider
{
  private readonly ISampleProvider sourceProvider;
  private readonly WaveFormat waveFormat;
  private volatile float volume;
  private float[] sourceBuffer;

  public SampleToWaveProvider24(ISampleProvider sourceProvider)
  {
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Input source provider must be IEEE float", nameof (sourceProvider));
    if (sourceProvider.WaveFormat.BitsPerSample != 32 /*0x20*/)
      throw new ArgumentException("Input source provider must be 32 bit", nameof (sourceProvider));
    this.waveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 24, sourceProvider.WaveFormat.Channels);
    this.sourceProvider = sourceProvider;
    this.volume = 1f;
  }

  public int Read(byte[] destBuffer, int offset, int numBytes)
  {
    int num1 = numBytes / 3;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    int num2 = this.sourceProvider.Read(this.sourceBuffer, 0, num1);
    int num3 = offset;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float num4 = this.sourceBuffer[index1] * this.volume;
      if ((double) num4 > 1.0)
        num4 = 1f;
      if ((double) num4 < -1.0)
        num4 = -1f;
      int num5 = (int) ((double) num4 * 8388607.0);
      byte[] numArray1 = destBuffer;
      int index2 = num3;
      int num6 = index2 + 1;
      int num7 = (int) (byte) num5;
      numArray1[index2] = (byte) num7;
      byte[] numArray2 = destBuffer;
      int index3 = num6;
      int num8 = index3 + 1;
      int num9 = (int) (byte) (num5 >> 8);
      numArray2[index3] = (byte) num9;
      byte[] numArray3 = destBuffer;
      int index4 = num8;
      num3 = index4 + 1;
      int num10 = (int) (byte) (num5 >> 16 /*0x10*/);
      numArray3[index4] = (byte) num10;
    }
    return num2 * 3;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public float Volume
  {
    get => this.volume;
    set => this.volume = value;
  }
}
