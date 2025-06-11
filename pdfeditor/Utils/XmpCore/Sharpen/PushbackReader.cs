// Decompiled with JetBrains decompiler
// Type: Sharpen.PushbackReader
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.IO;

#nullable disable
namespace Sharpen;

public class PushbackReader : StreamReader
{
  private readonly object _lock = new object();
  private readonly char[] _buf;
  private int _pos;

  public PushbackReader(StreamReader stream, int size)
    : base(stream.BaseStream)
  {
    this._buf = size > 0 ? new char[size] : throw new ArgumentOutOfRangeException(nameof (size), "size <= 0");
    this._pos = size;
  }

  public override int Read()
  {
    lock (this._lock)
      return this._pos < this._buf.Length ? (int) this._buf[this._pos++] : base.Read();
  }

  public override int Read(char[] buffer, int off, int len)
  {
    lock (this._lock)
    {
      try
      {
        if (len <= 0)
        {
          if (len < 0)
            throw new ArgumentException();
          if (off < 0 || off > buffer.Length)
            throw new ArgumentException();
          return 0;
        }
        int length = this._buf.Length - this._pos;
        if (length > 0)
        {
          if (len < length)
            length = len;
          Array.Copy((Array) this._buf, this._pos, (Array) buffer, off, length);
          this._pos += length;
          off += length;
          len -= length;
        }
        if (len <= 0)
          return length;
        len = base.Read(buffer, off, len);
        return len != -1 ? length + len : (length == 0 ? -1 : length);
      }
      catch (IndexOutOfRangeException ex)
      {
        throw new ArgumentException();
      }
    }
  }

  public void Unread(char[] buffer, int off, int len)
  {
    lock (this._lock)
    {
      if (len > this._pos)
        throw new IOException("Pushback buffer overflow");
      this._pos -= len;
      Array.Copy((Array) buffer, off, (Array) this._buf, this._pos, len);
    }
  }
}
