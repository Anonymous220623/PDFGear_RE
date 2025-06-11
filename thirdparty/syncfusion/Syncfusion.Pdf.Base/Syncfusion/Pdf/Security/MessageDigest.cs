// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class MessageDigest : IMessageDigest
{
  private const int c_byteLength = 64 /*0x40*/;
  private byte[] m_buf;
  private int m_bufOff;
  private long m_byteCount;

  internal MessageDigest() => this.m_buf = new byte[4];

  internal MessageDigest(MessageDigest t)
  {
    this.m_buf = new byte[t.m_buf.Length];
    Array.Copy((Array) t.m_buf, 0, (Array) this.m_buf, 0, t.m_buf.Length);
    this.m_bufOff = t.m_bufOff;
    this.m_byteCount = t.m_byteCount;
  }

  public void Update(byte input)
  {
    this.m_buf[this.m_bufOff++] = input;
    if (this.m_bufOff == this.m_buf.Length)
    {
      this.ProcessWord(this.m_buf, 0);
      this.m_bufOff = 0;
    }
    ++this.m_byteCount;
  }

  public void Update(byte[] bytes, int offset, int length)
  {
    for (; this.m_bufOff != 0 && length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
    while (length > this.m_buf.Length)
    {
      this.ProcessWord(bytes, offset);
      offset += this.m_buf.Length;
      length -= this.m_buf.Length;
      this.m_byteCount += (long) this.m_buf.Length;
    }
    for (; length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
  }

  public void BlockUpdate(byte[] bytes, int offset, int length)
  {
    for (; this.m_bufOff != 0 && length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
    while (length > this.m_buf.Length)
    {
      this.ProcessWord(bytes, offset);
      offset += this.m_buf.Length;
      length -= this.m_buf.Length;
      this.m_byteCount += (long) this.m_buf.Length;
    }
    for (; length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
  }

  internal void Finish()
  {
    long bitLength = this.m_byteCount << 3;
    this.Update((byte) 128 /*0x80*/);
    while (this.m_bufOff != 0)
      this.Update((byte) 0);
    this.ProcessLength(bitLength);
    this.ProcessBlock();
  }

  public virtual void Reset()
  {
    this.m_byteCount = 0L;
    this.m_bufOff = 0;
    Array.Clear((Array) this.m_buf, 0, this.m_buf.Length);
  }

  public int ByteLength => 64 /*0x40*/;

  internal abstract void ProcessWord(byte[] input, int inOff);

  internal abstract void ProcessLength(long bitLength);

  internal abstract void ProcessBlock();

  public abstract string AlgorithmName { get; }

  public abstract int MessageDigestSize { get; }

  public abstract int DoFinal(byte[] bytes, int offset);
}
