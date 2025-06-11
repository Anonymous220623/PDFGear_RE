// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.NativeStreamWrapper
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia;

internal class NativeStreamWrapper : Stream
{
  private readonly IStream _source;
  private readonly IStreamPointerRead _sourceReader;
  private readonly IntPtr _nativeLong;
  private long _position;

  public NativeStreamWrapper(IStream source)
  {
    this._source = source;
    this._sourceReader = (IStreamPointerRead) this._source;
    this._nativeLong = Marshal.AllocCoTaskMem(8);
  }

  ~NativeStreamWrapper() => Marshal.FreeCoTaskMem(this._nativeLong);

  public override bool CanRead => true;

  public override bool CanSeek => true;

  public override bool CanWrite => true;

  public override void Flush() => this._source.Commit(0);

  public override long Length
  {
    get
    {
      System.Runtime.InteropServices.ComTypes.STATSTG pstatstg;
      this._source.Stat(out pstatstg, 1);
      return pstatstg.cbSize;
    }
  }

  public override long Position
  {
    get => this._position;
    set => this.Seek(value, SeekOrigin.Begin);
  }

  public override unsafe int Read(byte[] buffer, int offset, int count)
  {
    if (buffer.Length < offset + count)
      throw new ArgumentException("Buffer too small");
    fixed (byte* numPtr = buffer)
      this._sourceReader.Read(numPtr + offset, count, this._nativeLong);
    int num = Marshal.ReadInt32(this._nativeLong);
    this._position += (long) num;
    return num;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    if (origin == SeekOrigin.Begin)
    {
      this._position = offset;
    }
    else
    {
      if (origin != SeekOrigin.Current)
        throw new NotImplementedException();
      this._position += offset;
    }
    this._source.Seek(offset, (int) origin, this._nativeLong);
    return Marshal.ReadInt64(this._nativeLong);
  }

  public override void SetLength(long value) => this._source.SetSize(value);

  public override void Write(byte[] buffer, int offset, int count)
  {
    if (offset != 0)
      throw new NotImplementedException();
    this._source.Write(buffer, count, IntPtr.Zero);
    this._position += (long) count;
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    Marshal.Release(Marshal.GetIUnknownForObject((object) this._source));
  }
}
