// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.NotifyingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class NotifyingSampleProvider : ISampleProvider, ISampleNotifier
{
  private readonly ISampleProvider source;
  private readonly SampleEventArgs sampleArgs = new SampleEventArgs(0.0f, 0.0f);
  private readonly int channels;

  public NotifyingSampleProvider(ISampleProvider source)
  {
    this.source = source;
    this.channels = this.WaveFormat.Channels;
  }

  public WaveFormat WaveFormat => this.source.WaveFormat;

  public int Read(float[] buffer, int offset, int sampleCount)
  {
    int num = this.source.Read(buffer, offset, sampleCount);
    if (this.Sample != null)
    {
      for (int index = 0; index < num; index += this.channels)
      {
        this.sampleArgs.Left = buffer[offset + index];
        this.sampleArgs.Right = this.channels > 1 ? buffer[offset + index + 1] : this.sampleArgs.Left;
        this.Sample((object) this, this.sampleArgs);
      }
    }
    return num;
  }

  public event EventHandler<SampleEventArgs> Sample;
}
