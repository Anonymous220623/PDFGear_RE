// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.Deflate64Stream
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.IO;

#nullable disable
namespace Ionic.Zip.Deflate64;

internal sealed class Deflate64Stream : Stream
{
  internal const int DefaultBufferSize = 8192 /*0x2000*/;
  private Stream _stream;
  private InflaterManaged _inflater;
  private readonly byte[] _buffer;

  internal Deflate64Stream(Stream stream, long uncompressedSize = -1)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (!stream.CanRead)
      throw new ArgumentException("NotSupported_UnreadableStream", nameof (stream));
    this._inflater = new InflaterManaged((IFileFormatReader) null, true, uncompressedSize);
    this._stream = stream;
    this._buffer = new byte[8192 /*0x2000*/];
  }

  public override bool CanRead => this._stream != null && this._stream.CanRead;

  public override bool CanWrite => false;

  public override bool CanSeek => false;

  public override long Length => throw new NotSupportedException();

  public override long Position
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public override void Flush() => this.EnsureNotDisposed();

  public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

  public override void SetLength(long value) => throw new NotSupportedException();

  public override int Read(byte[] array, int offset, int count)
  {
    this.ValidateParameters(array, offset, count);
    this.EnsureNotDisposed();
    int offset1 = offset;
    int length1 = count;
    while (true)
    {
      int num = this._inflater.Inflate(array, offset1, length1);
      offset1 += num;
      length1 -= num;
      if (length1 != 0 && !this._inflater.Finished())
      {
        int length2 = this._stream.Read(this._buffer, 0, this._buffer.Length);
        if (length2 > 0)
        {
          if (length2 <= this._buffer.Length)
            this._inflater.SetInput(this._buffer, 0, length2);
          else
            break;
        }
        else
          goto label_6;
      }
      else
        goto label_6;
    }
    throw new InvalidDataException();
label_6:
    return count - length1;
  }

  private void ValidateParameters(byte[] array, int offset, int count)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (array.Length - offset < count)
      throw new ArgumentException("InvalidArgumentOffsetCount");
  }

  private void EnsureNotDisposed()
  {
    if (this._stream != null)
      return;
    Deflate64Stream.ThrowStreamClosedException();
  }

  private static void ThrowStreamClosedException()
  {
    throw new ObjectDisposedException((string) null, "ObjectDisposed_StreamClosed");
  }

  public override IAsyncResult BeginRead(
    byte[] buffer,
    int offset,
    int count,
    AsyncCallback asyncCallback,
    object asyncState)
  {
    throw new NotImplementedException();
  }

  public override int EndRead(IAsyncResult asyncResult) => throw new NotImplementedException();

  public override void Write(byte[] array, int offset, int count)
  {
    throw new InvalidOperationException("CannotWriteToDeflateStream");
  }

  private void PurgeBuffers(bool disposing)
  {
    if (!disposing || this._stream == null)
      return;
    this.Flush();
  }

  protected override void Dispose(bool disposing)
  {
    try
    {
      this.PurgeBuffers(disposing);
    }
    finally
    {
      try
      {
        if (disposing)
        {
          if (this._stream != null)
            this._stream.Dispose();
        }
      }
      finally
      {
        this._stream = (Stream) null;
        try
        {
          if (this._inflater != null)
            this._inflater.Dispose();
        }
        finally
        {
          this._inflater = (InflaterManaged) null;
          base.Dispose(disposing);
        }
      }
    }
  }
}
