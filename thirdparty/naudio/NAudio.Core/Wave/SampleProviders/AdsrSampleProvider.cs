// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.AdsrSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Dsp;
using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class AdsrSampleProvider : ISampleProvider
{
  private readonly ISampleProvider source;
  private readonly EnvelopeGenerator adsr;
  private float attackSeconds;
  private float releaseSeconds;

  public AdsrSampleProvider(ISampleProvider source)
  {
    if (source.WaveFormat.Channels > 1)
      throw new ArgumentException("Currently only supports mono inputs");
    this.source = source;
    this.adsr = new EnvelopeGenerator();
    this.AttackSeconds = 0.01f;
    this.adsr.SustainLevel = 1f;
    this.adsr.DecayRate = 0.0f * (float) this.WaveFormat.SampleRate;
    this.ReleaseSeconds = 0.3f;
    this.adsr.Gate(true);
  }

  public float AttackSeconds
  {
    get => this.attackSeconds;
    set
    {
      this.attackSeconds = value;
      this.adsr.AttackRate = this.attackSeconds * (float) this.WaveFormat.SampleRate;
    }
  }

  public float ReleaseSeconds
  {
    get => this.releaseSeconds;
    set
    {
      this.releaseSeconds = value;
      this.adsr.ReleaseRate = this.releaseSeconds * (float) this.WaveFormat.SampleRate;
    }
  }

  public int Read(float[] buffer, int offset, int count)
  {
    if (this.adsr.State == EnvelopeGenerator.EnvelopeState.Idle)
      return 0;
    int num = this.source.Read(buffer, offset, count);
    for (int index = 0; index < num; ++index)
      buffer[offset++] *= this.adsr.Process();
    return num;
  }

  public void Stop() => this.adsr.Gate(false);

  public WaveFormat WaveFormat => this.source.WaveFormat;
}
