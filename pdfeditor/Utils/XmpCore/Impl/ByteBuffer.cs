// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.ByteBuffer
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.IO;
using System.Text;

#nullable disable
namespace XmpCore.Impl;

public sealed class ByteBuffer
{
  private byte[] _buffer;
  private Encoding _encoding;

  public int Length { get; private set; }

  public ByteBuffer(int initialCapacity) => this._buffer = new byte[initialCapacity];

  public ByteBuffer(byte[] buffer)
  {
    this._buffer = buffer;
    this.Length = buffer.Length;
  }

  public ByteBuffer(byte[] buffer, int length)
  {
    if (length > buffer.Length)
      throw new IndexOutOfRangeException("Valid length exceeds the buffer length.");
    this._buffer = buffer;
    this.Length = length;
  }

  public ByteBuffer(Stream stream)
  {
    this._buffer = new byte[16384 /*0x4000*/];
    int num;
    while ((num = stream.Read(this._buffer, this.Length, 16384 /*0x4000*/)) > 0)
    {
      this.Length += num;
      if (num != 16384 /*0x4000*/)
        break;
      this.EnsureCapacity(this.Length + 16384 /*0x4000*/);
    }
  }

  public ByteBuffer(byte[] buffer, int offset, int length)
  {
    if (length > buffer.Length - offset)
      throw new IndexOutOfRangeException("Valid length exceeds the buffer length.");
    this._buffer = new byte[length];
    Array.Copy((Array) buffer, offset, (Array) this._buffer, 0, length);
    this.Length = length;
  }

  public Stream GetByteStream() => (Stream) new MemoryStream(this._buffer, 0, this.Length);

  public byte ByteAt(int index)
  {
    return index < this.Length ? this._buffer[index] : throw new IndexOutOfRangeException("The index exceeds the valid buffer area");
  }

  public int CharAt(int index)
  {
    if (index >= this.Length)
      throw new IndexOutOfRangeException("The index exceeds the valid buffer area");
    return (int) this._buffer[index] & (int) byte.MaxValue;
  }

  public void Append(byte b)
  {
    this.EnsureCapacity(this.Length + 1);
    this._buffer[this.Length++] = b;
  }

  public void Append(byte[] bytes, int offset, int len)
  {
    this.EnsureCapacity(this.Length + len);
    Array.Copy((Array) bytes, offset, (Array) this._buffer, this.Length, len);
    this.Length += len;
  }

  public void Append(byte[] bytes) => this.Append(bytes, 0, bytes.Length);

  public void Append(ByteBuffer anotherBuffer)
  {
    this.Append(anotherBuffer._buffer, 0, anotherBuffer.Length);
  }

  public Encoding GetEncoding()
  {
    if (this._encoding == null)
    {
      if (this.Length < 2)
        this._encoding = Encoding.UTF8;
      else if (this._buffer[0] == (byte) 0)
      {
        if (this.Length < 4 || this._buffer[1] != (byte) 0)
        {
          this._encoding = Encoding.BigEndianUnicode;
        }
        else
        {
          if (((int) this._buffer[2] & (int) byte.MaxValue) == 254 && ((int) this._buffer[3] & (int) byte.MaxValue) == (int) byte.MaxValue)
            throw new NotSupportedException("UTF-32BE is not a supported encoding.");
          throw new NotSupportedException("UTF-32 is not a supported encoding.");
        }
      }
      else if (((int) this._buffer[0] & (int) byte.MaxValue) < 128 /*0x80*/)
      {
        if (this._buffer[1] != (byte) 0)
        {
          this._encoding = Encoding.UTF8;
        }
        else
        {
          if (this.Length >= 4 && this._buffer[2] == (byte) 0)
            throw new NotSupportedException("UTF-32LE is not a supported encoding.");
          this._encoding = Encoding.Unicode;
        }
      }
      else
      {
        switch ((int) this._buffer[0] & (int) byte.MaxValue)
        {
          case 239:
            this._encoding = Encoding.UTF8;
            break;
          case 254:
            this._encoding = Encoding.BigEndianUnicode;
            break;
          default:
            if (this.Length < 4 || this._buffer[2] != (byte) 0)
              throw new NotSupportedException("UTF-16 is not a supported encoding.");
            throw new NotSupportedException("UTF-32 is not a supported encoding.");
        }
      }
    }
    return this._encoding;
  }

  private void EnsureCapacity(int requestedLength)
  {
    if (requestedLength <= this._buffer.Length)
      return;
    byte[] buffer = this._buffer;
    this._buffer = new byte[buffer.Length * 2];
    Array.Copy((Array) buffer, 0, (Array) this._buffer, 0, buffer.Length);
  }
}
