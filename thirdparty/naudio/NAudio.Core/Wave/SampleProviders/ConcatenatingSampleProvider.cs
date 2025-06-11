// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.ConcatenatingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class ConcatenatingSampleProvider : ISampleProvider
{
  private readonly ISampleProvider[] providers;
  private int currentProviderIndex;

  public ConcatenatingSampleProvider(IEnumerable<ISampleProvider> providers)
  {
    this.providers = providers != null ? providers.ToArray<ISampleProvider>() : throw new ArgumentNullException(nameof (providers));
    if (this.providers.Length == 0)
      throw new ArgumentException("Must provide at least one input", nameof (providers));
    if (((IEnumerable<ISampleProvider>) this.providers).Any<ISampleProvider>((Func<ISampleProvider, bool>) (p => p.WaveFormat.Channels != this.WaveFormat.Channels)))
      throw new ArgumentException("All inputs must have the same channel count", nameof (providers));
    if (((IEnumerable<ISampleProvider>) this.providers).Any<ISampleProvider>((Func<ISampleProvider, bool>) (p => p.WaveFormat.SampleRate != this.WaveFormat.SampleRate)))
      throw new ArgumentException("All inputs must have the same sample rate", nameof (providers));
  }

  public WaveFormat WaveFormat => this.providers[0].WaveFormat;

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = 0;
    while (num1 < count && this.currentProviderIndex < this.providers.Length)
    {
      int count1 = count - num1;
      int num2 = this.providers[this.currentProviderIndex].Read(buffer, offset + num1, count1);
      num1 += num2;
      if (num2 == 0)
        ++this.currentProviderIndex;
    }
    return num1;
  }
}
