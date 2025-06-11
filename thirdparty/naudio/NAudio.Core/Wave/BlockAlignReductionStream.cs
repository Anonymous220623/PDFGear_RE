// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.BlockAlignReductionStream
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave;

public class BlockAlignReductionStream : WaveStream
{
  private WaveStream sourceStream;
  private long position;
  private readonly CircularBuffer circularBuffer;
  private long bufferStartPosition;
  private byte[] sourceBuffer;
  private readonly object lockObject = new object();

  public BlockAlignReductionStream(WaveStream sourceStream)
  {
    this.sourceStream = sourceStream;
    this.circularBuffer = new CircularBuffer(sourceStream.WaveFormat.AverageBytesPerSecond * 4);
  }

  private byte[] GetSourceBuffer(int size)
  {
    if (this.sourceBuffer == null || this.sourceBuffer.Length < size)
      this.sourceBuffer = new byte[size * 2];
    return this.sourceBuffer;
  }

  public override int BlockAlign => this.WaveFormat.BitsPerSample / 8 * this.WaveFormat.Channels;

  public override WaveFormat WaveFormat => this.sourceStream.WaveFormat;

  public override long Length => this.sourceStream.Length;

  public override long Position
  {
    get => this.position;
    set
    {
      lock (this.lockObject)
      {
        if (this.position == value)
          return;
        if (this.position % (long) this.BlockAlign != 0L)
          throw new ArgumentException("Position must be block aligned");
        long num = value - value % (long) this.sourceStream.BlockAlign;
        if (this.sourceStream.Position != num)
        {
          this.sourceStream.Position = num;
          this.circularBuffer.Reset();
          this.bufferStartPosition = this.sourceStream.Position;
        }
        this.position = value;
      }
    }
  }

  private long BufferEndPosition => this.bufferStartPosition + (long) this.circularBuffer.Count;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.sourceStream != null)
    {
      this.sourceStream.Dispose();
      this.sourceStream = (WaveStream) null;
    }
    base.Dispose(disposing);
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    lock (this.lockObject)
    {
      while (this.BufferEndPosition < this.position + (long) count)
      {
        int num = count;
        if (num % this.sourceStream.BlockAlign != 0)
          num = count + this.sourceStream.BlockAlign - count % this.sourceStream.BlockAlign;
        int count1 = this.sourceStream.Read(this.GetSourceBuffer(num), 0, num);
        this.circularBuffer.Write(this.GetSourceBuffer(num), 0, count1);
        if (count1 == 0)
          break;
      }
      if (this.bufferStartPosition < this.position)
      {
        this.circularBuffer.Advance((int) (this.position - this.bufferStartPosition));
        this.bufferStartPosition = this.position;
      }
      int num1 = this.circularBuffer.Read(buffer, offset, count);
      this.position += (long) num1;
      this.bufferStartPosition = this.position;
      return num1;
    }
  }
}
