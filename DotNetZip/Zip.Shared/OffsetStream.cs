// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.OffsetStream
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.IO;

#nullable disable
namespace Ionic.Zip;

internal class OffsetStream : Stream, IDisposable
{
  private long _originalPosition;
  private Stream _innerStream;

  public OffsetStream(Stream s)
  {
    this._originalPosition = s.Position;
    this._innerStream = s;
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    return this._innerStream.Read(buffer, offset, count);
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    throw new NotImplementedException();
  }

  public override bool CanRead => this._innerStream.CanRead;

  public override bool CanSeek => this._innerStream.CanSeek;

  public override bool CanWrite => false;

  public override void Flush() => this._innerStream.Flush();

  public override long Length => this._innerStream.Length;

  public override long Position
  {
    get => this._innerStream.Position - this._originalPosition;
    set => this._innerStream.Position = this._originalPosition + value;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
  }

  public override void SetLength(long value) => throw new NotImplementedException();

  void IDisposable.Dispose() => this.Close();

  public override void Close() => base.Close();
}
