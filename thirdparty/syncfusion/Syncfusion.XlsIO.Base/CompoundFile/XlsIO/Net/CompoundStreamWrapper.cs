// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.CompoundStreamWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

internal class CompoundStreamWrapper : CompoundStream
{
  private CompoundStream m_stream;

  public override bool CanRead => this.m_stream.CanRead;

  public override bool CanSeek => this.m_stream.CanSeek;

  public override bool CanWrite => this.m_stream.CanWrite;

  public override long Length => this.m_stream.Length;

  public override long Position
  {
    get => this.m_stream.Position;
    set => this.m_stream.Position = value;
  }

  public CompoundStreamWrapper(CompoundStream wrapped)
    : base(wrapped.Name)
  {
    this.m_stream = wrapped;
  }

  public override void Flush() => this.m_stream.Flush();

  public override int Read(byte[] buffer, int offset, int count)
  {
    return this.m_stream.Read(buffer, offset, count);
  }

  public override long Seek(long offset, SeekOrigin origin) => this.m_stream.Seek(offset, origin);

  public override void SetLength(long value) => this.m_stream.SetLength(value);

  public override void Write(byte[] buffer, int offset, int count)
  {
    this.m_stream.Write(buffer, offset, count);
  }

  protected override void Dispose(bool disposing)
  {
    if (this.m_stream == null)
      return;
    base.Dispose(disposing);
    this.m_stream = (CompoundStream) null;
    GC.SuppressFinalize((object) this);
  }
}
