// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.IWavePlayer
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public interface IWavePlayer : IDisposable
{
  void Play();

  void Stop();

  void Pause();

  void Init(IWaveProvider waveProvider);

  float Volume { get; set; }

  PlaybackState PlaybackState { get; }

  event EventHandler<StoppedEventArgs> PlaybackStopped;

  WaveFormat OutputWaveFormat { get; }
}
