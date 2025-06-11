// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SampleToWaveProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SampleToWaveProvider : IWaveProvider
{
  private readonly ISampleProvider source;

  public SampleToWaveProvider(ISampleProvider source)
  {
    if (source.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Must be already floating point");
    this.source = source;
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    int count1 = count / 4;
    return this.source.Read(new WaveBuffer(buffer).FloatBuffer, offset / 4, count1) * 4;
  }

  public WaveFormat WaveFormat => this.source.WaveFormat;
}
