// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.VolumeSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class VolumeSampleProvider : ISampleProvider
{
  private readonly ISampleProvider source;

  public VolumeSampleProvider(ISampleProvider source)
  {
    this.source = source;
    this.Volume = 1f;
  }

  public WaveFormat WaveFormat => this.source.WaveFormat;

  public int Read(float[] buffer, int offset, int sampleCount)
  {
    int num = this.source.Read(buffer, offset, sampleCount);
    if ((double) this.Volume != 1.0)
    {
      for (int index = 0; index < sampleCount; ++index)
        buffer[offset + index] *= this.Volume;
    }
    return num;
  }

  public float Volume { get; set; }
}
