// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GPStream
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace HandyControl.Data;

internal class GPStream : InteropValues.IStream
{
  protected Stream DataStream;
  private long _virtualPosition = -1;

  internal GPStream(Stream stream)
  {
    if (!stream.CanSeek)
    {
      byte[] numArray = new byte[256 /*0x0100*/];
      int offset = 0;
      int num;
      do
      {
        if (numArray.Length < offset + 256 /*0x0100*/)
        {
          byte[] destinationArray = new byte[numArray.Length * 2];
          Array.Copy((Array) numArray, (Array) destinationArray, numArray.Length);
          numArray = destinationArray;
        }
        num = stream.Read(numArray, offset, 256 /*0x0100*/);
        offset += num;
      }
      while (num != 0);
      this.DataStream = (Stream) new MemoryStream(numArray);
    }
    else
      this.DataStream = stream;
  }

  private void ActualizeVirtualPosition()
  {
    if (this._virtualPosition == -1L)
      return;
    if (this._virtualPosition > this.DataStream.Length)
      this.DataStream.SetLength(this._virtualPosition);
    this.DataStream.Position = this._virtualPosition;
    this._virtualPosition = -1L;
  }

  public virtual InteropValues.IStream Clone()
  {
    GPStream.NotImplemented();
    return (InteropValues.IStream) null;
  }

  public virtual void Commit(int grfCommitFlags)
  {
    this.DataStream.Flush();
    this.ActualizeVirtualPosition();
  }

  [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
  [SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
  public virtual long CopyTo(InteropValues.IStream pstm, long cb, long[] pcbRead)
  {
    IntPtr num1 = Marshal.AllocHGlobal(4096 /*0x1000*/);
    if (num1 == IntPtr.Zero)
      throw new OutOfMemoryException();
    long num2 = 0;
    try
    {
      int len;
      for (; num2 < cb; num2 += (long) len)
      {
        int length = 4096 /*0x1000*/;
        if (num2 + (long) length > cb)
          length = (int) (cb - num2);
        len = this.Read(num1, length);
        if (len != 0)
        {
          if (pstm.Write(num1, len) != len)
            throw GPStream.EFail("Wrote an incorrect number of bytes");
        }
        else
          break;
      }
    }
    finally
    {
      Marshal.FreeHGlobal(num1);
    }
    if (pcbRead != null && pcbRead.Length != 0)
      pcbRead[0] = num2;
    return num2;
  }

  public virtual Stream GetDataStream() => this.DataStream;

  public virtual void LockRegion(long libOffset, long cb, int dwLockType)
  {
  }

  protected static ExternalException EFail(string msg)
  {
    throw new ExternalException(msg, -2147467259 /*0x80004005*/);
  }

  protected static void NotImplemented() => throw new ExternalException(nameof (NotImplemented));

  public virtual int Read(IntPtr buf, int length)
  {
    byte[] numArray = new byte[length];
    int num = this.Read(numArray, length);
    Marshal.Copy(numArray, 0, buf, length);
    return num;
  }

  public virtual int Read(byte[] buffer, int length)
  {
    this.ActualizeVirtualPosition();
    return this.DataStream.Read(buffer, 0, length);
  }

  public virtual void Revert() => GPStream.NotImplemented();

  public virtual long Seek(long offset, int origin)
  {
    long num = this._virtualPosition;
    if (this._virtualPosition == -1L)
      num = this.DataStream.Position;
    long length = this.DataStream.Length;
    switch (origin)
    {
      case 0:
        if (offset <= length)
        {
          this.DataStream.Position = offset;
          this._virtualPosition = -1L;
          break;
        }
        this._virtualPosition = offset;
        break;
      case 1:
        if (offset + num <= length)
        {
          this.DataStream.Position = num + offset;
          this._virtualPosition = -1L;
          break;
        }
        this._virtualPosition = offset + num;
        break;
      case 2:
        if (offset <= 0L)
        {
          this.DataStream.Position = length + offset;
          this._virtualPosition = -1L;
          break;
        }
        this._virtualPosition = length + offset;
        break;
    }
    return this._virtualPosition == -1L ? this.DataStream.Position : this._virtualPosition;
  }

  public virtual void SetSize(long value) => this.DataStream.SetLength(value);

  public void Stat(IntPtr pstatstg, int grfStatFlag)
  {
    Marshal.StructureToPtr<GPStream.STATSTG>(new GPStream.STATSTG()
    {
      cbSize = this.DataStream.Length
    }, pstatstg, true);
  }

  public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
  {
  }

  public virtual int Write(IntPtr buf, int length)
  {
    byte[] numArray = new byte[length];
    Marshal.Copy(buf, numArray, 0, length);
    return this.Write(numArray, length);
  }

  public virtual int Write(byte[] buffer, int length)
  {
    this.ActualizeVirtualPosition();
    this.DataStream.Write(buffer, 0, length);
    return length;
  }

  [StructLayout(LayoutKind.Sequential)]
  public class STATSTG
  {
    public IntPtr pwcsName = IntPtr.Zero;
    public int type;
    [MarshalAs(UnmanagedType.I8)]
    public long cbSize;
    [MarshalAs(UnmanagedType.I8)]
    public long mtime;
    [MarshalAs(UnmanagedType.I8)]
    public long ctime;
    [MarshalAs(UnmanagedType.I8)]
    public long atime;
    [MarshalAs(UnmanagedType.I4)]
    public int grfMode;
    [MarshalAs(UnmanagedType.I4)]
    public int grfLocksSupported;
    public int clsid_data1;
    [MarshalAs(UnmanagedType.I2)]
    public short clsid_data2;
    [MarshalAs(UnmanagedType.I2)]
    public short clsid_data3;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b0;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b1;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b2;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b3;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b4;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b5;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b6;
    [MarshalAs(UnmanagedType.U1)]
    public byte clsid_b7;
    [MarshalAs(UnmanagedType.I4)]
    public int grfStateBits;
    [MarshalAs(UnmanagedType.I4)]
    public int reserved;
  }
}
