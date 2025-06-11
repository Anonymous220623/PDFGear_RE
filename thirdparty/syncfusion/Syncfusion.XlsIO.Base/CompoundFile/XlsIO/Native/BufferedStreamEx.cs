// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.BufferedStreamEx
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

public class BufferedStreamEx : Stream
{
  private const int _DefaultBufferSize = 4096 /*0x1000*/;
  private Stream _s;
  private byte[] _buffer;
  private int _readPos;
  private int _readLen;
  private int _writePos;
  private int _bufferSize;
  private long _streamPos;

  public override bool CanRead
  {
    get => this._s != null ? this._s.CanRead : throw new ArgumentNullException("stream");
  }

  public override bool CanWrite
  {
    get => this._s != null ? this._s.CanWrite : throw new ArgumentNullException("stream");
  }

  public override bool CanSeek
  {
    get => this._s != null ? this._s.CanSeek : throw new ArgumentNullException("stream");
  }

  public override long Length
  {
    get
    {
      if (this._s == null)
        throw new ArgumentNullException("stream");
      if (this._writePos > 0)
        this.FlushWrite();
      return this._s.Length;
    }
  }

  public override long Position
  {
    get => this._streamPos + (long) this._readPos + (long) this._writePos;
    set
    {
      if (value < 0L)
        throw new ArgumentOutOfRangeException(nameof (value));
      if (this._s == null)
        throw new ArgumentNullException("stream");
      if (!this._s.CanSeek)
        throw new ArgumentException("Stream does not support seek operation.");
      if (this._writePos > 0)
        this.FlushWrite();
      if (this._streamPos + (long) this._readLen > value && value >= this._streamPos)
      {
        this._readPos = (int) (value - this._streamPos);
      }
      else
      {
        this._readPos = 0;
        this._readLen = 0;
        this._streamPos = this._s.Seek(value, SeekOrigin.Begin);
      }
    }
  }

  public Stream BaseStream => this._s;

  private BufferedStreamEx()
  {
  }

  public BufferedStreamEx(Stream stream)
    : this(stream, 4096 /*0x1000*/)
  {
  }

  public BufferedStreamEx(Stream stream, int bufferSize)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (bufferSize <= 0)
      throw new ArgumentOutOfRangeException(nameof (bufferSize));
    this._s = stream;
    this._bufferSize = bufferSize;
    if (!this._s.CanRead && !this._s.CanWrite)
      throw new ArgumentException("Stream read / write operations is closed.", nameof (stream));
  }

  public override int Read(byte[] array, int offset, int count)
  {
    int count1 = this._readLen - this._readPos;
    if (count1 == 0)
    {
      if (!this._s.CanRead)
        throw new ArgumentException("Stream does not support read operation.");
      if (this._writePos > 0)
        this.FlushWrite();
      if (count >= this._bufferSize)
      {
        int num = this._s.Read(array, offset, count);
        this._streamPos = this._s.Position;
        this._readPos = 0;
        this._readLen = 0;
        return num;
      }
      if (this._buffer == null)
        this._buffer = new byte[this._bufferSize];
      long streamPos = this._streamPos;
      this._streamPos = this._s.Position;
      count1 = this._s.Read(this._buffer, 0, this._bufferSize);
      if (count1 == 0)
      {
        this._streamPos = streamPos;
        return 0;
      }
      this._readPos = 0;
      this._readLen = count1;
    }
    if (count1 > count)
      count1 = count;
    Buffer.BlockCopy((Array) this._buffer, this._readPos, (Array) array, offset, count1);
    this._readPos += count1;
    if (count1 < count)
    {
      int num = this._s.Read(array, offset + count1, count - count1);
      count1 += num;
      this._streamPos = this._s.Position;
      this._readPos = 0;
      this._readLen = 0;
    }
    return count1;
  }

  public override int ReadByte()
  {
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (this._readLen == 0 && !this._s.CanRead)
      throw new ArgumentException("Stream does not support Read operation.");
    if (this._readPos == this._readLen)
    {
      if (this._writePos > 0)
        this.FlushWrite();
      if (this._buffer == null)
        this._buffer = new byte[this._bufferSize];
      this._streamPos = this._s.Position;
      this._readLen = this._s.Read(this._buffer, 0, this._bufferSize);
      this._readPos = 0;
    }
    return this._readPos == this._readLen ? -1 : (int) this._buffer[this._readPos++];
  }

  public override void Write(byte[] array, int offset, int count)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (array.Length - offset < count)
      throw new ArgumentException();
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (this._writePos == 0)
    {
      if (!this._s.CanWrite)
        throw new ArgumentException("Stream does not support write operation.");
      if (this._readPos < this._readLen)
        this.FlushRead();
    }
    if (this._writePos > 0)
    {
      int count1 = this._bufferSize - this._writePos;
      if (count1 > 0)
      {
        if (count1 > count)
          count1 = count;
        Buffer.BlockCopy((Array) array, offset, (Array) this._buffer, this._writePos, count1);
        this._writePos += count1;
        if (count == count1)
          return;
        offset += count1;
        count -= count1;
      }
      this._s.Write(this._buffer, 0, this._writePos);
      this._streamPos = this._s.Position;
      this._writePos = 0;
    }
    if (count >= this._bufferSize)
    {
      this._s.Write(array, offset, count);
      this._streamPos = this._s.Position;
    }
    else
    {
      if (count == 0)
        return;
      if (this._buffer == null)
        this._buffer = new byte[this._bufferSize];
      Buffer.BlockCopy((Array) array, offset, (Array) this._buffer, 0, count);
      this._writePos += count;
    }
  }

  public override void WriteByte(byte value)
  {
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (this._writePos == 0)
    {
      if (!this._s.CanWrite)
        throw new ArgumentException("Stream does not support write operation.");
      if (this._readPos < this._readLen)
        this.FlushRead();
      if (this._buffer == null)
        this._buffer = new byte[this._bufferSize];
    }
    if (this._writePos == this._bufferSize)
      this.FlushWrite();
    this._buffer[this._writePos++] = value;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (!this._s.CanSeek)
      throw new ArgumentException("Stream does not support Seek operation.");
    if (this._writePos > 0)
      this.FlushWrite();
    else if (origin == SeekOrigin.Current)
      offset -= (long) (this._readLen - this._readPos);
    long num1 = this._s.Position + (long) (this._readPos - this._readLen);
    long num2 = this._s.Seek(offset, origin);
    if (this._readLen > 0)
    {
      if (num1 == num2)
      {
        if (this._readPos > 0)
        {
          Buffer.BlockCopy((Array) this._buffer, this._readPos, (Array) this._buffer, 0, this._readLen - this._readPos);
          this._readLen -= this._readPos;
          this._readPos = 0;
        }
        if (this._readLen > 0)
          this._s.Seek((long) this._readLen, SeekOrigin.Current);
      }
      else if (num1 - (long) this._readPos < num2 && num2 < num1 + (long) this._readLen - (long) this._readPos)
      {
        int num3 = (int) (num2 - num1);
        Buffer.BlockCopy((Array) this._buffer, this._readPos + num3, (Array) this._buffer, 0, this._readLen - (this._readPos + num3));
        this._readLen -= this._readPos + num3;
        this._readPos = 0;
        if (this._readLen > 0)
          this._s.Seek((long) this._readLen, SeekOrigin.Current);
      }
      else
      {
        this._readPos = 0;
        this._readLen = 0;
      }
    }
    this._streamPos = num2;
    return num2;
  }

  public override void Close()
  {
    if (this._s != null)
    {
      this.Flush();
      this._s.Close();
    }
    this._s = (Stream) null;
    this._buffer = (byte[]) null;
  }

  public override void Flush()
  {
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (this._writePos > 0)
    {
      this.FlushWrite();
    }
    else
    {
      if (this._readPos >= this._readLen || !this._s.CanSeek)
        return;
      this.FlushRead();
    }
  }

  private void FlushRead()
  {
    if (this._readPos - this._readLen != 0)
      this._s.Seek((long) (this._readPos - this._readLen), SeekOrigin.Current);
    this._readPos = 0;
    this._readLen = 0;
  }

  private void FlushWrite()
  {
    this._s.Write(this._buffer, 0, this._writePos);
    this._writePos = 0;
    this._streamPos = this._s.Position;
    this._s.Flush();
  }

  public override void SetLength(long value)
  {
    if (value < 0L)
      throw new ArgumentOutOfRangeException(nameof (value));
    if (this._s == null)
      throw new ArgumentNullException("stream");
    if (!this._s.CanSeek)
      throw new ArgumentException("Stream does not support Seek operation.");
    if (!this._s.CanWrite)
      throw new ArgumentException("Stream does not support Write operation.");
    if (this._writePos > 0)
      this.FlushWrite();
    else if (this._readPos < this._readLen)
      this.FlushRead();
    this._s.SetLength(value);
  }
}
