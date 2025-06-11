// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.OffsetSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class OffsetSampleProvider : ISampleProvider
{
  private readonly ISampleProvider sourceProvider;
  private int phase;
  private int phasePos;
  private int delayBySamples;
  private int skipOverSamples;
  private int takeSamples;
  private int leadOutSamples;

  private int TimeSpanToSamples(TimeSpan time)
  {
    return (int) (time.TotalSeconds * (double) this.WaveFormat.SampleRate) * this.WaveFormat.Channels;
  }

  private TimeSpan SamplesToTimeSpan(int samples)
  {
    return TimeSpan.FromSeconds((double) (samples / this.WaveFormat.Channels) / (double) this.WaveFormat.SampleRate);
  }

  public int DelayBySamples
  {
    get => this.delayBySamples;
    set
    {
      if (this.phase != 0)
        throw new InvalidOperationException("Can't set DelayBySamples after calling Read");
      if (value % this.WaveFormat.Channels != 0)
        throw new ArgumentException("DelayBySamples must be a multiple of WaveFormat.Channels");
      this.delayBySamples = value;
    }
  }

  public TimeSpan DelayBy
  {
    get => this.SamplesToTimeSpan(this.delayBySamples);
    set => this.delayBySamples = this.TimeSpanToSamples(value);
  }

  public int SkipOverSamples
  {
    get => this.skipOverSamples;
    set
    {
      if (this.phase != 0)
        throw new InvalidOperationException("Can't set SkipOverSamples after calling Read");
      if (value % this.WaveFormat.Channels != 0)
        throw new ArgumentException("SkipOverSamples must be a multiple of WaveFormat.Channels");
      this.skipOverSamples = value;
    }
  }

  public TimeSpan SkipOver
  {
    get => this.SamplesToTimeSpan(this.skipOverSamples);
    set => this.skipOverSamples = this.TimeSpanToSamples(value);
  }

  public int TakeSamples
  {
    get => this.takeSamples;
    set
    {
      if (this.phase != 0)
        throw new InvalidOperationException("Can't set TakeSamples after calling Read");
      if (value % this.WaveFormat.Channels != 0)
        throw new ArgumentException("TakeSamples must be a multiple of WaveFormat.Channels");
      this.takeSamples = value;
    }
  }

  public TimeSpan Take
  {
    get => this.SamplesToTimeSpan(this.takeSamples);
    set => this.takeSamples = this.TimeSpanToSamples(value);
  }

  public int LeadOutSamples
  {
    get => this.leadOutSamples;
    set
    {
      if (this.phase != 0)
        throw new InvalidOperationException("Can't set LeadOutSamples after calling Read");
      if (value % this.WaveFormat.Channels != 0)
        throw new ArgumentException("LeadOutSamples must be a multiple of WaveFormat.Channels");
      this.leadOutSamples = value;
    }
  }

  public TimeSpan LeadOut
  {
    get => this.SamplesToTimeSpan(this.leadOutSamples);
    set => this.leadOutSamples = this.TimeSpanToSamples(value);
  }

  public OffsetSampleProvider(ISampleProvider sourceProvider)
  {
    this.sourceProvider = sourceProvider;
  }

  public WaveFormat WaveFormat => this.sourceProvider.WaveFormat;

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = 0;
    if (this.phase == 0)
      ++this.phase;
    if (this.phase == 1)
    {
      int num2 = Math.Min(count, this.DelayBySamples - this.phasePos);
      for (int index = 0; index < num2; ++index)
        buffer[offset + index] = 0.0f;
      this.phasePos += num2;
      num1 += num2;
      if (this.phasePos >= this.DelayBySamples)
      {
        ++this.phase;
        this.phasePos = 0;
      }
    }
    if (this.phase == 2)
    {
      if (this.SkipOverSamples > 0)
      {
        float[] buffer1 = new float[this.WaveFormat.SampleRate * this.WaveFormat.Channels];
        int num3;
        for (int index = 0; index < this.SkipOverSamples; index += num3)
        {
          int count1 = Math.Min(this.SkipOverSamples - index, buffer1.Length);
          num3 = this.sourceProvider.Read(buffer1, 0, count1);
          if (num3 == 0)
            break;
        }
      }
      ++this.phase;
      this.phasePos = 0;
    }
    if (this.phase == 3)
    {
      int num4 = count - num1;
      if (this.takeSamples != 0)
        num4 = Math.Min(num4, this.takeSamples - this.phasePos);
      int num5 = this.sourceProvider.Read(buffer, offset + num1, num4);
      this.phasePos += num5;
      num1 += num5;
      if (num5 < num4 || this.takeSamples > 0 && this.phasePos >= this.takeSamples)
      {
        ++this.phase;
        this.phasePos = 0;
      }
    }
    if (this.phase == 4)
    {
      int num6 = Math.Min(count - num1, this.LeadOutSamples - this.phasePos);
      for (int index = 0; index < num6; ++index)
        buffer[offset + num1 + index] = 0.0f;
      this.phasePos += num6;
      num1 += num6;
      if (this.phasePos >= this.LeadOutSamples)
      {
        ++this.phase;
        this.phasePos = 0;
      }
    }
    return num1;
  }
}
