// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.WindowRandom
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class WindowRandom : IRandom, IDisposable
{
  private readonly IRandom m_source;
  private readonly long m_offset;
  private readonly long m_length;

  internal WindowRandom(IRandom source, long offset, long length)
  {
    this.m_source = source;
    this.m_offset = offset;
    this.m_length = length;
  }

  public virtual int Get(long position)
  {
    return position >= this.m_length ? -1 : this.m_source.Get(this.m_offset + position);
  }

  public virtual int Get(long position, byte[] bytes, int off, int len)
  {
    if (position >= this.m_length)
      return -1;
    long length = Math.Min((long) len, this.m_length - position);
    return this.m_source.Get(this.m_offset + position, bytes, off, (int) length);
  }

  public virtual long Length => this.m_length;

  public virtual void Close() => this.m_source.Close();

  public virtual void Dispose() => this.Close();
}
