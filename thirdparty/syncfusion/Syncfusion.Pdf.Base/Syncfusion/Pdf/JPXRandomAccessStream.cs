// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JPXRandomAccessStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class JPXRandomAccessStream
{
  private Stream is_Renamed;
  private int maxsize;
  private int inc;
  private byte[] buf;
  private int len;
  private int pos;
  private bool complete;

  internal virtual int Pos => this.pos;

  internal JPXRandomAccessStream()
  {
  }

  internal JPXRandomAccessStream(Stream is_Renamed, int size, int inc, int maxsize)
  {
    if (size < 0 || inc <= 0 || maxsize <= 0 || is_Renamed == null)
      throw new ArgumentException();
    this.is_Renamed = is_Renamed;
    if (size < int.MaxValue)
      ++size;
    this.buf = new byte[size];
    this.inc = inc;
    if (maxsize < int.MaxValue)
      ++maxsize;
    this.maxsize = maxsize;
    this.pos = 0;
    this.len = 0;
    this.complete = false;
  }

  private void growBuffer()
  {
    int num = this.inc;
    if (this.buf.Length + num > this.maxsize)
      num = this.maxsize - this.buf.Length;
    if (num <= 0)
      throw new IOException($"Reached maximum cache size ({(object) this.maxsize})");
    byte[] destinationArray;
    try
    {
      destinationArray = new byte[this.buf.Length + this.inc];
    }
    catch (OutOfMemoryException ex)
    {
      throw new IOException("Out of memory to cache input data");
    }
    Array.Copy((Array) this.buf, 0, (Array) destinationArray, 0, this.len);
    this.buf = destinationArray;
  }

  private void readInput()
  {
    if (this.complete)
      throw new ArgumentException("Already reached EOF");
    int count = (int) (this.is_Renamed.Length - this.is_Renamed.Position);
    if (count == 0)
      count = 1;
    while (this.len + count > this.buf.Length)
      this.growBuffer();
    int num;
    do
    {
      num = this.is_Renamed.Read(this.buf, this.len, count);
      if (num > 0)
      {
        this.len += num;
        count -= num;
      }
    }
    while (count > 0 && num > 0);
    if (num > 0)
      return;
    this.complete = true;
    this.is_Renamed = (Stream) null;
  }

  internal virtual void close()
  {
    this.buf = (byte[]) null;
    if (this.complete)
      return;
    this.is_Renamed = (Stream) null;
  }

  internal virtual void seek(int off)
  {
    if (this.complete && off > this.len)
      throw new EndOfStreamException();
    this.pos = off;
  }

  internal virtual int length()
  {
    while (!this.complete)
      this.readInput();
    return this.len;
  }

  internal virtual byte readByte() => this.read();

  internal virtual byte read()
  {
    if (this.pos < this.len)
      return this.buf[this.pos++];
    while (!this.complete && this.pos >= this.len)
      this.readInput();
    if (this.pos != this.len)
    {
      int pos = this.pos;
      int len = this.len;
    }
    return this.buf[this.pos++];
  }

  internal virtual void readFully(byte[] b, int off, int n)
  {
    if (this.pos + n <= this.len)
    {
      Array.Copy((Array) this.buf, this.pos, (Array) b, off, n);
      this.pos += n;
    }
    else
    {
      while (!this.complete && this.pos + n > this.len)
        this.readInput();
      if (this.pos + n > this.len)
        throw new EndOfStreamException();
      Array.Copy((Array) this.buf, this.pos, (Array) b, off, n);
      this.pos += n;
    }
  }

  internal virtual byte readUnsignedByte()
  {
    return this.pos < this.len ? this.buf[this.pos++] : this.read();
  }

  internal virtual short readShort()
  {
    return this.pos + 1 < this.len ? (short) ((int) this.buf[this.pos++] << 8 | (int) byte.MaxValue & (int) this.buf[this.pos++]) : (short) ((int) this.read() << 8 | (int) this.read());
  }

  internal virtual int readUnsignedShort()
  {
    return this.pos + 1 < this.len ? ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 8 | (int) byte.MaxValue & (int) this.buf[this.pos++] : (int) this.read() << 8 | (int) this.read();
  }

  internal virtual int readInt()
  {
    return this.pos + 3 < this.len ? (int) this.buf[this.pos++] << 24 | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 16 /*0x10*/ | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 8 | (int) byte.MaxValue & (int) this.buf[this.pos++] : (int) this.read() << 24 | (int) this.read() << 16 /*0x10*/ | (int) this.read() << 8 | (int) this.read();
  }

  internal virtual long readUnsignedInt()
  {
    return this.pos + 3 < this.len ? -1L & (long) ((int) this.buf[this.pos++] << 24 | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 16 /*0x10*/ | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 8 | (int) byte.MaxValue & (int) this.buf[this.pos++]) : -1L & (long) ((int) this.read() << 24 | (int) this.read() << 16 /*0x10*/ | (int) this.read() << 8 | (int) this.read());
  }

  internal virtual long readLong()
  {
    return this.pos + 7 < this.len ? (long) this.buf[this.pos++] << 56 | (long) this.buf[this.pos++] << 48 /*0x30*/ | (long) this.buf[this.pos++] << 40 | (long) this.buf[this.pos++] << 32 /*0x20*/ | (long) this.buf[this.pos++] << 24 | (long) this.buf[this.pos++] << 16 /*0x10*/ | (long) this.buf[this.pos++] << 8 | (long) this.buf[this.pos++] : (long) this.read() << 56 | (long) this.read() << 48 /*0x30*/ | (long) this.read() << 40 | (long) this.read() << 32 /*0x20*/ | (long) this.read() << 24 | (long) this.read() << 16 /*0x10*/ | (long) this.read() << 8 | (long) this.read();
  }

  internal virtual float readFloat()
  {
    return BitConverter.ToSingle(BitConverter.GetBytes(this.pos + 3 >= this.len ? (int) this.read() << 24 | (int) this.read() << 16 /*0x10*/ | (int) this.read() << 8 | (int) this.read() : (int) this.buf[this.pos++] << 24 | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 16 /*0x10*/ | ((int) byte.MaxValue & (int) this.buf[this.pos++]) << 8 | (int) byte.MaxValue & (int) this.buf[this.pos++]), 0);
  }

  internal virtual double readDouble()
  {
    return BitConverter.ToDouble(BitConverter.GetBytes(this.pos + 7 >= this.len ? (long) this.read() << 56 | (long) this.read() << 48 /*0x30*/ | (long) this.read() << 40 | (long) this.read() << 32 /*0x20*/ | (long) this.read() << 24 | (long) this.read() << 16 /*0x10*/ | (long) this.read() << 8 | (long) this.read() : (long) this.buf[this.pos++] << 56 | (long) this.buf[this.pos++] << 48 /*0x30*/ | (long) this.buf[this.pos++] << 40 | (long) this.buf[this.pos++] << 32 /*0x20*/ | (long) this.buf[this.pos++] << 24 | (long) this.buf[this.pos++] << 16 /*0x10*/ | (long) this.buf[this.pos++] << 8 | (long) this.buf[this.pos++]), 0);
  }

  internal virtual int skipBytes(int n)
  {
    if (this.complete && this.pos + n > this.len)
      throw new EndOfStreamException();
    this.pos += n;
    return n;
  }

  internal virtual void flush()
  {
  }

  internal virtual void write(byte b) => throw new IOException("read-only");

  internal virtual void writeByte(int v) => throw new IOException("read-only");

  internal virtual void writeShort(int v) => throw new IOException("read-only");

  internal virtual void writeInt(int v) => throw new IOException("read-only");

  internal virtual void writeLong(long v) => throw new IOException("read-only");

  internal virtual void writeFloat(float v) => throw new IOException("read-only");

  internal virtual void writeDouble(double v) => throw new IOException("read-only");
}
