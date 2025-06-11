// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RandomStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RandomStream : Stream
{
  private IRandom m_random;
  private long m_position;

  internal RandomStream(IRandom source) => this.m_random = source;

  public override bool CanRead => true;

  public override bool CanSeek => true;

  public override bool CanWrite => false;

  public override void Flush()
  {
  }

  public override long Length => this.m_random.Length;

  public override long Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public override int Read(byte[] buffer, int offset, int length)
  {
    int num = this.m_random.Get(this.m_position, buffer, offset, length);
    if (num == -1)
      return 0;
    this.m_position += (long) num;
    return num;
  }

  public override int ReadByte()
  {
    int num = this.m_random.Get(this.m_position);
    if (num >= 0)
      ++this.m_position;
    return num;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    switch (origin)
    {
      case SeekOrigin.Begin:
        this.m_position = offset;
        break;
      case SeekOrigin.Current:
        this.m_position += offset;
        break;
      default:
        this.m_position = offset + this.m_random.Length;
        break;
    }
    return this.m_position;
  }

  public override void SetLength(long value) => throw new Exception("Not supported.");

  public override void Write(byte[] buffer, int offset, int count)
  {
    throw new Exception("Not supported.");
  }
}
