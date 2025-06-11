// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SampleChannel
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SampleChannel : ISampleProvider
{
  private readonly VolumeSampleProvider volumeProvider;
  private readonly MeteringSampleProvider preVolumeMeter;
  private readonly WaveFormat waveFormat;

  public SampleChannel(IWaveProvider waveProvider)
    : this(waveProvider, false)
  {
  }

  public SampleChannel(IWaveProvider waveProvider, bool forceStereo)
  {
    ISampleProvider source = SampleProviderConverters.ConvertWaveProviderIntoSampleProvider(waveProvider);
    if (source.WaveFormat.Channels == 1 & forceStereo)
      source = (ISampleProvider) new MonoToStereoSampleProvider(source);
    this.waveFormat = source.WaveFormat;
    this.preVolumeMeter = new MeteringSampleProvider(source);
    this.volumeProvider = new VolumeSampleProvider((ISampleProvider) this.preVolumeMeter);
  }

  public int Read(float[] buffer, int offset, int sampleCount)
  {
    return this.volumeProvider.Read(buffer, offset, sampleCount);
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public float Volume
  {
    get => this.volumeProvider.Volume;
    set => this.volumeProvider.Volume = value;
  }

  public event EventHandler<StreamVolumeEventArgs> PreVolumeMeter
  {
    add => this.preVolumeMeter.StreamVolume += value;
    remove => this.preVolumeMeter.StreamVolume -= value;
  }
}
