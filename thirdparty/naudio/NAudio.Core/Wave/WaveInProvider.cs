// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveInProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class WaveInProvider : IWaveProvider
{
  private readonly IWaveIn waveIn;
  private readonly BufferedWaveProvider bufferedWaveProvider;

  public WaveInProvider(IWaveIn waveIn)
  {
    this.waveIn = waveIn;
    waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(this.OnDataAvailable);
    this.bufferedWaveProvider = new BufferedWaveProvider(this.WaveFormat);
  }

  private void OnDataAvailable(object sender, WaveInEventArgs e)
  {
    this.bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    return this.bufferedWaveProvider.Read(buffer, offset, count);
  }

  public WaveFormat WaveFormat => this.waveIn.WaveFormat;
}
