// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.ILockBytes
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Guid("0000000a-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CLSCompliant(false)]
public interface ILockBytes
{
  int ReadAt(ulong ulOffset, [MarshalAs(UnmanagedType.LPArray)] byte[] pv, uint cb, out uint pcbRead);

  int WriteAt(ulong ulOffset, [MarshalAs(UnmanagedType.LPArray)] byte[] pv, uint cb, out uint pcbWritten);

  int Flush();

  int SetSize(ulong cb);

  int LockRegion(ulong libOffset, ulong cb, uint dwLockType);

  int UnlockRegion(ulong libOffset, ulong cb, uint dwLockType);

  int Stat(out STATSTG pstatstg, uint grfStatFlag);
}
