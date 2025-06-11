// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveStream
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.IO;

#nullable disable
namespace NAudio.Wave;

public abstract class WaveStream : Stream, IWaveProvider
{
  public abstract WaveFormat WaveFormat { get; }

  public override bool CanRead => true;

  public override bool CanSeek => true;

  public override bool CanWrite => false;

  public override void Flush()
  {
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    switch (origin)
    {
      case SeekOrigin.Begin:
        this.Position = offset;
        break;
      case SeekOrigin.Current:
        this.Position += offset;
        break;
      default:
        this.Position = this.Length + offset;
        break;
    }
    return this.Position;
  }

  public override void SetLength(long length)
  {
    throw new NotSupportedException("Can't set length of a WaveFormatString");
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    throw new NotSupportedException("Can't write to a WaveFormatString");
  }

  public virtual int BlockAlign => this.WaveFormat.BlockAlign;

  public void Skip(int seconds)
  {
    long num = this.Position + (long) (this.WaveFormat.AverageBytesPerSecond * seconds);
    if (num > this.Length)
      this.Position = this.Length;
    else if (num < 0L)
      this.Position = 0L;
    else
      this.Position = num;
  }

  public virtual TimeSpan CurrentTime
  {
    get
    {
      return TimeSpan.FromSeconds((double) this.Position / (double) this.WaveFormat.AverageBytesPerSecond);
    }
    set
    {
      this.Position = (long) (value.TotalSeconds * (double) this.WaveFormat.AverageBytesPerSecond);
    }
  }

  public virtual TimeSpan TotalTime
  {
    get
    {
      return TimeSpan.FromSeconds((double) this.Length / (double) this.WaveFormat.AverageBytesPerSecond);
    }
  }

  public virtual bool HasData(int count) => this.Position < this.Length;
}
