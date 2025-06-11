// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveRecorder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class WaveRecorder : IWaveProvider, IDisposable
{
  private WaveFileWriter writer;
  private IWaveProvider source;

  public WaveRecorder(IWaveProvider source, string destination)
  {
    this.source = source;
    this.writer = new WaveFileWriter(destination, source.WaveFormat);
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    int count1 = this.source.Read(buffer, offset, count);
    this.writer.Write(buffer, offset, count1);
    return count1;
  }

  public WaveFormat WaveFormat => this.source.WaveFormat;

  public void Dispose()
  {
    if (this.writer == null)
      return;
    this.writer.Dispose();
    this.writer = (WaveFileWriter) null;
  }
}
