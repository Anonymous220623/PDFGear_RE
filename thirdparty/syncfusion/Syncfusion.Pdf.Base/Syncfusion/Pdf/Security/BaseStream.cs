// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BaseStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class BaseStream : Stream
{
  protected readonly Stream m_input;
  private int m_limit;
  private bool m_closed;

  internal BaseStream(Stream stream, int limit)
  {
    this.m_input = stream;
    this.m_limit = limit;
  }

  internal int Remaining => this.m_limit;

  protected virtual void SetParentEndOfFileDetect(bool isDetect)
  {
    if (!(this.m_input is Asn1LengthStream))
      return;
    ((Asn1LengthStream) this.m_input).SetEndOfFileOnStart(isDetect);
  }

  public sealed override bool CanRead => !this.m_closed;

  public sealed override bool CanSeek => false;

  public sealed override bool CanWrite => false;

  public override void Close() => this.m_closed = true;

  public sealed override void Flush()
  {
  }

  public sealed override long Length => throw new NotSupportedException();

  public sealed override long Position
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    int num1 = offset;
    try
    {
      int num2;
      for (int index = offset + count; num1 < index; buffer[num1++] = (byte) num2)
      {
        num2 = this.ReadByte();
        if (num2 == -1)
          break;
      }
    }
    catch (IOException ex)
    {
      if (num1 == offset)
        throw;
    }
    return num1 - offset;
  }

  public sealed override long Seek(long offset, SeekOrigin origin)
  {
    throw new NotSupportedException();
  }

  public sealed override void SetLength(long value) => throw new NotSupportedException();

  public sealed override void Write(byte[] buffer, int offset, int count)
  {
    throw new NotSupportedException();
  }
}
