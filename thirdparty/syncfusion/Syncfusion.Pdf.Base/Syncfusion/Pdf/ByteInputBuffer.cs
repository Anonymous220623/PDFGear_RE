// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ByteInputBuffer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class ByteInputBuffer
{
  private byte[] buf;
  private int count;
  private int pos;

  public ByteInputBuffer(byte[] buf)
  {
    this.buf = buf;
    this.count = buf.Length;
  }

  public ByteInputBuffer(byte[] buf, int offset, int length)
  {
    this.buf = buf;
    this.pos = offset;
    this.count = offset + length;
  }

  public virtual void setByteArray(byte[] buf, int offset, int length)
  {
    if (buf == null)
    {
      if (length < 0 || this.count + length > this.buf.Length)
        throw new ArgumentException();
      if (offset < 0)
      {
        this.pos = this.count;
        this.count += length;
      }
      else
      {
        this.count = offset + length;
        this.pos = offset;
      }
    }
    else
    {
      if (offset < 0 || length < 0 || offset + length > buf.Length)
        throw new ArgumentException();
      this.buf = buf;
      this.count = offset + length;
      this.pos = offset;
    }
  }

  public virtual void addByteArray(byte[] data, int off, int len)
  {
    lock (this)
    {
      if (len < 0 || off < 0 || len + off > this.buf.Length)
        throw new ArgumentException();
      if (this.count + len <= this.buf.Length)
      {
        Array.Copy((Array) data, off, (Array) this.buf, this.count, len);
        this.count += len;
      }
      else
      {
        if (this.count - this.pos + len <= this.buf.Length)
        {
          Array.Copy((Array) this.buf, this.pos, (Array) this.buf, 0, this.count - this.pos);
        }
        else
        {
          byte[] buf = this.buf;
          this.buf = new byte[this.count - this.pos + len];
          Array.Copy((Array) buf, this.count, (Array) this.buf, 0, this.count - this.pos);
        }
        this.count -= this.pos;
        this.pos = 0;
        Array.Copy((Array) data, off, (Array) this.buf, this.count, len);
        this.count += len;
      }
    }
  }

  public virtual int readChecked()
  {
    if (this.pos < this.count)
      return (int) this.buf[this.pos++] & (int) byte.MaxValue;
    throw new EndOfStreamException();
  }

  public virtual int read()
  {
    return this.pos < this.count ? (int) this.buf[this.pos++] & (int) byte.MaxValue : -1;
  }
}
