// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveExtensionMethods
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;

#nullable disable
namespace NAudio.Wave;

public static class WaveExtensionMethods
{
  public static ISampleProvider ToSampleProvider(this IWaveProvider waveProvider)
  {
    return SampleProviderConverters.ConvertWaveProviderIntoSampleProvider(waveProvider);
  }

  public static void Init(
    this IWavePlayer wavePlayer,
    ISampleProvider sampleProvider,
    bool convertTo16Bit = false)
  {
    IWaveProvider waveProvider = convertTo16Bit ? (IWaveProvider) new SampleToWaveProvider16(sampleProvider) : (IWaveProvider) new SampleToWaveProvider(sampleProvider);
    wavePlayer.Init(waveProvider);
  }

  public static WaveFormat AsStandardWaveFormat(this WaveFormat waveFormat)
  {
    return !(waveFormat is WaveFormatExtensible formatExtensible) ? waveFormat : formatExtensible.ToStandardWaveFormat();
  }

  public static IWaveProvider ToWaveProvider(this ISampleProvider sampleProvider)
  {
    return (IWaveProvider) new SampleToWaveProvider(sampleProvider);
  }

  public static IWaveProvider ToWaveProvider16(this ISampleProvider sampleProvider)
  {
    return (IWaveProvider) new SampleToWaveProvider16(sampleProvider);
  }

  public static ISampleProvider FollowedBy(
    this ISampleProvider sampleProvider,
    ISampleProvider next)
  {
    return (ISampleProvider) new ConcatenatingSampleProvider((IEnumerable<ISampleProvider>) new ISampleProvider[2]
    {
      sampleProvider,
      next
    });
  }

  public static ISampleProvider FollowedBy(
    this ISampleProvider sampleProvider,
    TimeSpan silenceDuration,
    ISampleProvider next)
  {
    return (ISampleProvider) new ConcatenatingSampleProvider((IEnumerable<ISampleProvider>) new ISampleProvider[2]
    {
      (ISampleProvider) new OffsetSampleProvider(sampleProvider)
      {
        LeadOut = silenceDuration
      },
      next
    });
  }

  public static ISampleProvider Skip(this ISampleProvider sampleProvider, TimeSpan skipDuration)
  {
    return (ISampleProvider) new OffsetSampleProvider(sampleProvider)
    {
      SkipOver = skipDuration
    };
  }

  public static ISampleProvider Take(this ISampleProvider sampleProvider, TimeSpan takeDuration)
  {
    return (ISampleProvider) new OffsetSampleProvider(sampleProvider)
    {
      Take = takeDuration
    };
  }

  public static ISampleProvider ToMono(
    this ISampleProvider sourceProvider,
    float leftVol = 0.5f,
    float rightVol = 0.5f)
  {
    if (sourceProvider.WaveFormat.Channels == 1)
      return sourceProvider;
    return (ISampleProvider) new StereoToMonoSampleProvider(sourceProvider)
    {
      LeftVolume = leftVol,
      RightVolume = rightVol
    };
  }

  public static ISampleProvider ToStereo(
    this ISampleProvider sourceProvider,
    float leftVol = 1f,
    float rightVol = 1f)
  {
    if (sourceProvider.WaveFormat.Channels == 2)
      return sourceProvider;
    return (ISampleProvider) new MonoToStereoSampleProvider(sourceProvider)
    {
      LeftVolume = leftVol,
      RightVolume = rightVol
    };
  }
}
