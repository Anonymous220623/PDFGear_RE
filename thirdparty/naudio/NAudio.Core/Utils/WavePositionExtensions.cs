// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.WavePositionExtensions
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Wave;
using System;

#nullable disable
namespace NAudio.Utils;

public static class WavePositionExtensions
{
  public static TimeSpan GetPositionTimeSpan(this IWavePosition @this)
  {
    return TimeSpan.FromMilliseconds((double) (@this.GetPosition() / (long) (@this.OutputWaveFormat.Channels * @this.OutputWaveFormat.BitsPerSample / 8)) * 1000.0 / (double) @this.OutputWaveFormat.SampleRate);
  }
}
