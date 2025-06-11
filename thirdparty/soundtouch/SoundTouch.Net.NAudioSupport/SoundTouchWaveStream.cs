// Decompiled with JetBrains decompiler
// Type: SoundTouch.Net.NAudioSupport.SoundTouchWaveStream
// Assembly: SoundTouch.Net.NAudioSupport, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 99206FE3-DB71-4C89-91A8-76F439C9BC37
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.NAudioSupport.dll

using NAudio.Wave;
using SoundTouch.Net.NAudioSupport.Assets;
using System;
using System.IO;

#nullable enable
namespace SoundTouch.Net.NAudioSupport;

public class SoundTouchWaveStream : WaveStream
{
  private readonly SoundTouchWaveProvider _provider;
  private WaveStream? _sourceStream;

  public SoundTouchWaveStream(WaveStream sourceStream)
    : this(sourceStream, (SoundTouchProcessor) null)
  {
  }

  public SoundTouchWaveStream(WaveStream sourceStream, SoundTouchProcessor? processor)
  {
    this._sourceStream = sourceStream ?? throw new ArgumentNullException(nameof (sourceStream));
    this._provider = new SoundTouchWaveProvider((IWaveProvider) sourceStream, processor);
  }

  public virtual WaveFormat WaveFormat => this._provider.WaveFormat;

  public virtual long Length => ((Stream) this.SourceStream).Length;

  public virtual bool CanRead => this._sourceStream != null && base.CanRead;

  public virtual bool CanSeek => this._sourceStream != null && base.CanSeek;

  public double Tempo
  {
    get => this._provider.Tempo;
    set => this._provider.Tempo = value;
  }

  public double Pitch
  {
    get => this._provider.Pitch;
    set => this._provider.Pitch = value;
  }

  public double Rate
  {
    get => this._provider.Rate;
    set => this._provider.Rate = value;
  }

  public double TempoChange
  {
    get => this._provider.TempoChange;
    set => this._provider.TempoChange = value;
  }

  public double PitchOctaves
  {
    get => this._provider.PitchOctaves;
    set => this._provider.PitchOctaves = value;
  }

  public double PitchSemiTones
  {
    get => this._provider.PitchSemiTones;
    set => this._provider.PitchSemiTones = value;
  }

  public double RateChange
  {
    get => this._provider.RateChange;
    set => this._provider.RateChange = value;
  }

  public virtual long Position
  {
    get => ((Stream) this.SourceStream).Position;
    set
    {
      lock (this._provider.SyncLock)
      {
        ((Stream) this.SourceStream).Position = value;
        this._provider.Clear();
      }
    }
  }

  private WaveStream SourceStream
  {
    get
    {
      return this._sourceStream != null ? this._sourceStream : throw new ObjectDisposedException((string) null, Strings.ObjectDisposed_StreamClosed);
    }
  }

  public virtual int Read(byte[] buffer, int offset, int count)
  {
    return this._provider.Read(buffer, offset, count);
  }

  public virtual void Flush()
  {
    this._provider.Clear();
    base.Flush();
  }

  protected virtual void Dispose(bool disposing)
  {
    try
    {
      if (!disposing)
        return;
      if (this._sourceStream == null)
        return;
      try
      {
        ((Stream) this._sourceStream).Flush();
      }
      finally
      {
        ((Stream) this._sourceStream).Dispose();
      }
    }
    finally
    {
      this._sourceStream = (WaveStream) null;
      // ISSUE: explicit non-virtual call
      __nonvirtual (((Stream) this).Dispose(disposing));
    }
  }
}
