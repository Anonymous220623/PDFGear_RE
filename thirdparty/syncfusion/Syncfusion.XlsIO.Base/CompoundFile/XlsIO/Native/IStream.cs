// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CLSCompliant(false)]
[Guid("0000000c-0000-0000-C000-000000000046")]
public interface IStream
{
  int Read([MarshalAs(UnmanagedType.LPArray)] byte[] pv, uint cb, ref uint pcbRead);

  int Write([MarshalAs(UnmanagedType.LPArray)] byte[] pv, uint cb, ref uint pcbWritten);

  int Seek(long dlibMove, SeekOrigin dwOrigin, out long plibNewPosition);

  int SetSize(ulong libNewSize);

  int CopyTo(IStream pstm, ulong cb, ref ulong pcbRead, ref ulong pcbWritten);

  int Commit(uint grfCommitFlags);

  int Revert();

  int LockRegion(ulong libOffset, ulong cb, uint dwLockType);

  int UnlockRegion(ulong libOffset, ulong cb, uint dwLockType);

  int Stat(ref STATSTG pstatstg, uint grfStatFlag);

  int Clone(ref IStream ppstm);
}
