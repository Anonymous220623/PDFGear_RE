// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SampleProviderConverterBase
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public abstract class SampleProviderConverterBase : ISampleProvider
{
  protected IWaveProvider source;
  private readonly WaveFormat waveFormat;
  protected byte[] sourceBuffer;

  public SampleProviderConverterBase(IWaveProvider source)
  {
    this.source = source;
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, source.WaveFormat.Channels);
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public abstract int Read(float[] buffer, int offset, int count);

  protected void EnsureSourceBuffer(int sourceBytesRequired)
  {
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, sourceBytesRequired);
  }
}
