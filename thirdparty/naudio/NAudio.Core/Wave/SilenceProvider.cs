// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SilenceProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class SilenceProvider : IWaveProvider
{
  public SilenceProvider(WaveFormat wf) => this.WaveFormat = wf;

  public int Read(byte[] buffer, int offset, int count)
  {
    Array.Clear((Array) buffer, offset, count);
    return count;
  }

  public WaveFormat WaveFormat { get; private set; }
}
