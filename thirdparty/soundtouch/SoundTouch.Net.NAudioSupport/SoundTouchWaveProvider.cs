// Decompiled with JetBrains decompiler
// Type: SoundTouch.Net.NAudioSupport.SoundTouchWaveProvider
// Assembly: SoundTouch.Net.NAudioSupport, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 99206FE3-DB71-4C89-91A8-76F439C9BC37
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.NAudioSupport.dll

using NAudio.Wave;
using SoundTouch.Net.NAudioSupport.Assets;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable enable
namespace SoundTouch.Net.NAudioSupport;

public class SoundTouchWaveProvider : IWaveProvider
{
  private readonly IWaveProvider _sourceProvider;
  private readonly SoundTouchProcessor _processor;
  private readonly byte[] _buffer = new byte[4096 /*0x1000*/];

  public SoundTouchWaveProvider(IWaveProvider sourceProvider, SoundTouchProcessor? processor = null)
  {
    this._sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof (sourceProvider));
    if (sourceProvider.WaveFormat.Encoding != 3)
      throw new ArgumentException(Strings.Argument_WaveFormatIeeeFloat, nameof (sourceProvider));
    if (sourceProvider.WaveFormat.BitsPerSample != 32 /*0x20*/)
      throw new ArgumentException(Strings.Argument_WaveFormat32BitsPerSample, nameof (sourceProvider));
    int sampleRate = sourceProvider.WaveFormat.SampleRate;
    int channels = sourceProvider.WaveFormat.Channels;
    this._processor = processor ?? new SoundTouchProcessor();
    this._processor.SampleRate = sampleRate;
    this._processor.Channels = channels;
    if (processor != null)
      return;
    this._processor.Tempo = 1.0;
    this._processor.Pitch = 1.0;
    this._processor.Rate = 1.0;
  }

  public event EventHandler<UnobservedExceptionEventArgs> UnobservedException = (_, __) => { };

  public WaveFormat WaveFormat => this._sourceProvider.WaveFormat;

  public double Tempo
  {
    get => this._processor.Tempo;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.Tempo, value))
          return;
        this._processor.Tempo = value;
      }
    }
  }

  public double Pitch
  {
    get => this._processor.Pitch;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.Pitch, value))
          return;
        this._processor.Pitch = value;
      }
    }
  }

  public double Rate
  {
    get => this._processor.Rate;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.Rate, value))
          return;
        this._processor.Rate = value;
      }
    }
  }

  public double TempoChange
  {
    get => this._processor.TempoChange;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.TempoChange, value))
          return;
        this._processor.TempoChange = value;
      }
    }
  }

  public double PitchOctaves
  {
    get => this._processor.PitchOctaves;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.PitchOctaves, value))
          return;
        this._processor.PitchOctaves = value;
      }
    }
  }

  public double PitchSemiTones
  {
    get => this._processor.PitchSemiTones;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.PitchSemiTones, value))
          return;
        this._processor.PitchSemiTones = value;
      }
    }
  }

  public double RateChange
  {
    get => this._processor.RateChange;
    set
    {
      lock (this.SyncLock)
      {
        if (DoubleUtil.AreClose(this._processor.RateChange, value))
          return;
        this._processor.RateChange = value;
      }
    }
  }

  internal bool IsFlushed { get; private set; }

  internal object SyncLock { get; } = new object();

  public void OptimizeForSpeech()
  {
    this._processor.SetSetting(SettingId.SequenceDurationMs, 50);
    this._processor.SetSetting(SettingId.SeekWindowDurationMs, 10);
    this._processor.SetSetting(SettingId.OverlapDurationMs, 20);
    this._processor.SetSetting(SettingId.UseQuickSeek, 0);
  }

  public void Clear()
  {
    lock (this.SyncLock)
    {
      this._processor.Clear();
      this.IsFlushed = false;
    }
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    int num1 = count / 4;
    try
    {
      lock (this.SyncLock)
      {
        while (this._processor.AvailableSamples < num1)
        {
          int num2;
          try
          {
            num2 = this._sourceProvider.Read(this._buffer, 0, this._buffer.Length);
          }
          catch (EndOfStreamException ex)
          {
            num2 = 0;
          }
          if (num2 == 0)
          {
            if (!this.IsFlushed)
            {
              this.IsFlushed = true;
              this._processor.Flush();
              break;
            }
            break;
          }
          Span<float> span = MemoryMarshal.Cast<byte, float>(MemoryExtensions.AsSpan<byte>(this._buffer).Slice(0, num2));
          this._processor.PutSamples(Span<float>.op_Implicit(span), span.Length / this.WaveFormat.Channels);
        }
        Span<float> output = MemoryMarshal.Cast<byte, float>(MemoryExtensions.AsSpan<byte>(buffer).Slice(offset, count));
        output.Clear();
        return this._processor.ReceiveSamples(in output, output.Length / this.WaveFormat.Channels) * 4 * this.WaveFormat.Channels;
      }
    }
    catch (Exception ex)
    {
      UnobservedExceptionEventArgs e = new UnobservedExceptionEventArgs(ex);
      this.UnobservedException((object) this, e);
      if (e.IsObserved)
        return 0;
      throw;
    }
  }
}
