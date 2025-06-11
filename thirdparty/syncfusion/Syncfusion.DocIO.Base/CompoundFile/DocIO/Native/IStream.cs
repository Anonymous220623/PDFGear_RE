// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IStream
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
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
