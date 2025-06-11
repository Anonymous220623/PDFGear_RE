// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOffsetStream
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class WaveOffsetStream : WaveStream
{
  private WaveStream sourceStream;
  private long audioStartPosition;
  private long sourceOffsetBytes;
  private long sourceLengthBytes;
  private long length;
  private readonly int bytesPerSample;
  private long position;
  private TimeSpan startTime;
  private TimeSpan sourceOffset;
  private TimeSpan sourceLength;
  private readonly object lockObject = new object();

  public WaveOffsetStream(
    WaveStream sourceStream,
    TimeSpan startTime,
    TimeSpan sourceOffset,
    TimeSpan sourceLength)
  {
    if (sourceStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
      throw new ArgumentException("Only PCM supported");
    this.sourceStream = sourceStream;
    this.position = 0L;
    this.bytesPerSample = sourceStream.WaveFormat.BitsPerSample / 8 * sourceStream.WaveFormat.Channels;
    this.StartTime = startTime;
    this.SourceOffset = sourceOffset;
    this.SourceLength = sourceLength;
  }

  public WaveOffsetStream(WaveStream sourceStream)
    : this(sourceStream, TimeSpan.Zero, TimeSpan.Zero, sourceStream.TotalTime)
  {
  }

  public TimeSpan StartTime
  {
    get => this.startTime;
    set
    {
      lock (this.lockObject)
      {
        this.startTime = value;
        this.audioStartPosition = (long) (this.startTime.TotalSeconds * (double) this.sourceStream.WaveFormat.SampleRate) * (long) this.bytesPerSample;
        this.length = this.audioStartPosition + this.sourceLengthBytes;
        this.Position = this.Position;
      }
    }
  }

  public TimeSpan SourceOffset
  {
    get => this.sourceOffset;
    set
    {
      lock (this.lockObject)
      {
        this.sourceOffset = value;
        this.sourceOffsetBytes = (long) (this.sourceOffset.TotalSeconds * (double) this.sourceStream.WaveFormat.SampleRate) * (long) this.bytesPerSample;
        this.Position = this.Position;
      }
    }
  }

  public TimeSpan SourceLength
  {
    get => this.sourceLength;
    set
    {
      lock (this.lockObject)
      {
        this.sourceLength = value;
        this.sourceLengthBytes = (long) (this.sourceLength.TotalSeconds * (double) this.sourceStream.WaveFormat.SampleRate) * (long) this.bytesPerSample;
        this.length = this.audioStartPosition + this.sourceLengthBytes;
        this.Position = this.Position;
      }
    }
  }

  public override int BlockAlign => this.sourceStream.BlockAlign;

  public override long Length => this.length;

  public override long Position
  {
    get => this.position;
    set
    {
      lock (this.lockObject)
      {
        value -= value % (long) this.BlockAlign;
        if (value < this.audioStartPosition)
          this.sourceStream.Position = this.sourceOffsetBytes;
        else
          this.sourceStream.Position = this.sourceOffsetBytes + (value - this.audioStartPosition);
        this.position = value;
      }
    }
  }

  public override int Read(byte[] destBuffer, int offset, int numBytes)
  {
    lock (this.lockObject)
    {
      int num1 = 0;
      if (this.position < this.audioStartPosition)
      {
        num1 = (int) Math.Min((long) numBytes, this.audioStartPosition - this.position);
        for (int index = 0; index < num1; ++index)
          destBuffer[index + offset] = (byte) 0;
      }
      if (num1 < numBytes)
      {
        int count = (int) Math.Min((long) (numBytes - num1), this.sourceLengthBytes + this.sourceOffsetBytes - this.sourceStream.Position);
        int num2 = this.sourceStream.Read(destBuffer, num1 + offset, count);
        num1 += num2;
      }
      for (int index = num1; index < numBytes; ++index)
        destBuffer[offset + index] = (byte) 0;
      this.position += (long) numBytes;
      return numBytes;
    }
  }

  public override WaveFormat WaveFormat => this.sourceStream.WaveFormat;

  public override bool HasData(int count)
  {
    return this.position + (long) count >= this.audioStartPosition && this.position < this.length && this.sourceStream.HasData(count);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.sourceStream != null)
    {
      this.sourceStream.Dispose();
      this.sourceStream = (WaveStream) null;
    }
    base.Dispose(disposing);
  }
}
