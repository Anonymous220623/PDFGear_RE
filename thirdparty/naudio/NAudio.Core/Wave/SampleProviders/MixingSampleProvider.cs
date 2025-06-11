// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.MixingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class MixingSampleProvider : ISampleProvider
{
  private readonly List<ISampleProvider> sources;
  private float[] sourceBuffer;
  private const int MaxInputs = 1024 /*0x0400*/;

  public MixingSampleProvider(WaveFormat waveFormat)
  {
    if (waveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Mixer wave format must be IEEE float");
    this.sources = new List<ISampleProvider>();
    this.WaveFormat = waveFormat;
  }

  public MixingSampleProvider(IEnumerable<ISampleProvider> sources)
  {
    this.sources = new List<ISampleProvider>();
    foreach (ISampleProvider source in sources)
      this.AddMixerInput(source);
    if (this.sources.Count == 0)
      throw new ArgumentException("Must provide at least one input in this constructor");
  }

  public IEnumerable<ISampleProvider> MixerInputs => (IEnumerable<ISampleProvider>) this.sources;

  public bool ReadFully { get; set; }

  public void AddMixerInput(IWaveProvider mixerInput)
  {
    this.AddMixerInput(SampleProviderConverters.ConvertWaveProviderIntoSampleProvider(mixerInput));
  }

  public void AddMixerInput(ISampleProvider mixerInput)
  {
    lock (this.sources)
    {
      if (this.sources.Count >= 1024 /*0x0400*/)
        throw new InvalidOperationException("Too many mixer inputs");
      this.sources.Add(mixerInput);
    }
    if (this.WaveFormat == null)
      this.WaveFormat = mixerInput.WaveFormat;
    else if (this.WaveFormat.SampleRate != mixerInput.WaveFormat.SampleRate || this.WaveFormat.Channels != mixerInput.WaveFormat.Channels)
      throw new ArgumentException("All mixer inputs must have the same WaveFormat");
  }

  public event EventHandler<SampleProviderEventArgs> MixerInputEnded;

  public void RemoveMixerInput(ISampleProvider mixerInput)
  {
    lock (this.sources)
      this.sources.Remove(mixerInput);
  }

  public void RemoveAllMixerInputs()
  {
    lock (this.sources)
      this.sources.Clear();
  }

  public WaveFormat WaveFormat { get; private set; }

  public int Read(float[] buffer, int offset, int count)
  {
    int val2 = 0;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, count);
    lock (this.sources)
    {
      for (int index1 = this.sources.Count - 1; index1 >= 0; --index1)
      {
        ISampleProvider source = this.sources[index1];
        int val1 = source.Read(this.sourceBuffer, 0, count);
        int num = offset;
        for (int index2 = 0; index2 < val1; ++index2)
        {
          if (index2 >= val2)
            buffer[num++] = this.sourceBuffer[index2];
          else
            buffer[num++] += this.sourceBuffer[index2];
        }
        val2 = Math.Max(val1, val2);
        if (val1 < count)
        {
          EventHandler<SampleProviderEventArgs> mixerInputEnded = this.MixerInputEnded;
          if (mixerInputEnded != null)
            mixerInputEnded((object) this, new SampleProviderEventArgs(source));
          this.sources.RemoveAt(index1);
        }
      }
    }
    if (this.ReadFully && val2 < count)
    {
      int num = offset + val2;
      while (num < offset + count)
        buffer[num++] = 0.0f;
      val2 = count;
    }
    return val2;
  }
}
