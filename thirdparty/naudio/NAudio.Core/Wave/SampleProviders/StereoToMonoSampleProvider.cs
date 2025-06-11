// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.StereoToMonoSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class StereoToMonoSampleProvider : ISampleProvider
{
  private readonly ISampleProvider sourceProvider;
  private float[] sourceBuffer;

  public StereoToMonoSampleProvider(ISampleProvider sourceProvider)
  {
    this.LeftVolume = 0.5f;
    this.RightVolume = 0.5f;
    if (sourceProvider.WaveFormat.Channels != 2)
      throw new ArgumentException("Source must be stereo");
    this.sourceProvider = sourceProvider;
    this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceProvider.WaveFormat.SampleRate, 1);
  }

  public float LeftVolume { get; set; }

  public float RightVolume { get; set; }

  public WaveFormat WaveFormat { get; }

  public int Read(float[] buffer, int offset, int count)
  {
    int count1 = count * 2;
    if (this.sourceBuffer == null || this.sourceBuffer.Length < count1)
      this.sourceBuffer = new float[count1];
    int num1 = this.sourceProvider.Read(this.sourceBuffer, 0, count1);
    int num2 = offset;
    for (int index = 0; index < num1; index += 2)
    {
      double num3 = (double) this.sourceBuffer[index];
      float num4 = this.sourceBuffer[index + 1];
      double leftVolume = (double) this.LeftVolume;
      float num5 = (float) (num3 * leftVolume + (double) num4 * (double) this.RightVolume);
      buffer[num2++] = num5;
    }
    return num1 / 2;
  }
}
