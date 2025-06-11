// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.WdlResamplingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Dsp;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class WdlResamplingSampleProvider : ISampleProvider
{
  private readonly WdlResampler resampler;
  private readonly WaveFormat outFormat;
  private readonly ISampleProvider source;
  private readonly int channels;

  public WdlResamplingSampleProvider(ISampleProvider source, int newSampleRate)
  {
    this.channels = source.WaveFormat.Channels;
    this.outFormat = WaveFormat.CreateIeeeFloatWaveFormat(newSampleRate, this.channels);
    this.source = source;
    this.resampler = new WdlResampler();
    this.resampler.SetMode(true, 2, false);
    this.resampler.SetFilterParms();
    this.resampler.SetFeedMode(false);
    this.resampler.SetRates((double) source.WaveFormat.SampleRate, (double) newSampleRate);
  }

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = count / this.channels;
    float[] inbuffer;
    int inbufferOffset;
    int num2 = this.resampler.ResamplePrepare(num1, this.outFormat.Channels, out inbuffer, out inbufferOffset);
    int nsamples_in = this.source.Read(inbuffer, inbufferOffset, num2 * this.channels) / this.channels;
    return this.resampler.ResampleOut(buffer, offset, nsamples_in, num1, this.channels) * this.channels;
  }

  public WaveFormat WaveFormat => this.outFormat;
}
