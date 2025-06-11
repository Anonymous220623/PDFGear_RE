// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.NativeStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

internal class NativeStream : CompoundStream
{
  private IStream m_stream;
  private long m_lPosition;
  private static bool m_bShown;

  public NativeStream(IStream stream, string name)
    : base(name)
  {
    this.m_stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
  }

  public override int Read(byte[] buffer, int offset, int length)
  {
    this.CheckBufferOffsetLength(buffer, offset, length);
    byte[] numArray = offset != 0 ? new byte[length] : buffer;
    uint pcbRead = 0;
    this.m_stream.Read(numArray, (uint) length, ref pcbRead);
    int count = (int) pcbRead;
    this.m_lPosition += (long) pcbRead;
    if (offset != 0)
      Buffer.BlockCopy((Array) numArray, 0, (Array) buffer, offset, count);
    return count;
  }

  public override void Write(byte[] buffer, int offset, int length)
  {
    this.CheckBufferOffsetLength(buffer, offset, length);
    byte[] numArray;
    if (offset == 0)
    {
      numArray = buffer;
    }
    else
    {
      numArray = new byte[length];
      Buffer.BlockCopy((Array) buffer, offset, (Array) numArray, 0, length);
    }
    uint pcbWritten = 0;
    this.m_stream.Write(numArray, (uint) length, ref pcbWritten);
    this.m_lPosition += (long) pcbWritten;
  }

  public override long Seek(long position, SeekOrigin origin)
  {
    long plibNewPosition;
    this.m_stream.Seek(position, origin, out plibNewPosition);
    this.m_lPosition = plibNewPosition;
    return plibNewPosition;
  }

  public override void SetLength(long length) => this.m_stream.SetSize((ulong) length);

  public override long Length
  {
    get
    {
      if (!NativeStream.m_bShown)
        NativeStream.m_bShown = true;
      long plibNewPosition;
      this.m_stream.Seek(0L, SeekOrigin.End, out plibNewPosition);
      this.m_stream.Seek(this.m_lPosition, SeekOrigin.Begin, out this.m_lPosition);
      return plibNewPosition;
    }
  }

  public override long Position
  {
    get => this.m_lPosition;
    set => this.m_lPosition = this.Seek(value, SeekOrigin.Begin);
  }

  public override bool CanRead => true;

  public override bool CanWrite => true;

  public override bool CanSeek => true;

  public override void Flush() => this.m_stream.Commit(0U);

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    this.m_stream.Commit(0U);
    Marshal.FinalReleaseComObject((object) this.m_stream);
    this.m_stream = (IStream) null;
    this.m_lPosition = -1L;
  }

  private void CheckBufferOffsetLength(byte[] buffer, int offset, int length)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset + length > buffer.Length)
      throw new ArgumentOutOfRangeException("Array size, offset and length doesn't match each other");
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length));
  }
}
