// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SampleProviderConverters
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

internal static class SampleProviderConverters
{
  public static ISampleProvider ConvertWaveProviderIntoSampleProvider(IWaveProvider waveProvider)
  {
    if (waveProvider.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
    {
      if (waveProvider.WaveFormat.BitsPerSample == 8)
        return (ISampleProvider) new Pcm8BitToSampleProvider(waveProvider);
      if (waveProvider.WaveFormat.BitsPerSample == 16 /*0x10*/)
        return (ISampleProvider) new Pcm16BitToSampleProvider(waveProvider);
      if (waveProvider.WaveFormat.BitsPerSample == 24)
        return (ISampleProvider) new Pcm24BitToSampleProvider(waveProvider);
      if (waveProvider.WaveFormat.BitsPerSample == 32 /*0x20*/)
        return (ISampleProvider) new Pcm32BitToSampleProvider(waveProvider);
      throw new InvalidOperationException("Unsupported bit depth");
    }
    if (waveProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Unsupported source encoding");
    return waveProvider.WaveFormat.BitsPerSample == 64 /*0x40*/ ? (ISampleProvider) new WaveToSampleProvider64(waveProvider) : (ISampleProvider) new WaveToSampleProvider(waveProvider);
  }
}
