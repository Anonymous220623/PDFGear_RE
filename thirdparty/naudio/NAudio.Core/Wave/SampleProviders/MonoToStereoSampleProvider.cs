// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.MonoToStereoSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class MonoToStereoSampleProvider : ISampleProvider
{
  private readonly ISampleProvider source;
  private float[] sourceBuffer;

  public MonoToStereoSampleProvider(ISampleProvider source)
  {
    this.LeftVolume = 1f;
    this.RightVolume = 1f;
    if (source.WaveFormat.Channels != 1)
      throw new ArgumentException("Source must be mono");
    this.source = source;
    this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, 2);
  }

  public WaveFormat WaveFormat { get; }

  public int Read(float[] buffer, int offset, int count)
  {
    int count1 = count / 2;
    int num1 = offset;
    this.EnsureSourceBuffer(count1);
    int num2 = this.source.Read(this.sourceBuffer, 0, count1);
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float[] numArray1 = buffer;
      int index2 = num1;
      int num3 = index2 + 1;
      double num4 = (double) this.sourceBuffer[index1] * (double) this.LeftVolume;
      numArray1[index2] = (float) num4;
      float[] numArray2 = buffer;
      int index3 = num3;
      num1 = index3 + 1;
      double num5 = (double) this.sourceBuffer[index1] * (double) this.RightVolume;
      numArray2[index3] = (float) num5;
    }
    return num2 * 2;
  }

  public float LeftVolume { get; set; }

  public float RightVolume { get; set; }

  private void EnsureSourceBuffer(int count)
  {
    if (this.sourceBuffer != null && this.sourceBuffer.Length >= count)
      return;
    this.sourceBuffer = new float[count];
  }
}
