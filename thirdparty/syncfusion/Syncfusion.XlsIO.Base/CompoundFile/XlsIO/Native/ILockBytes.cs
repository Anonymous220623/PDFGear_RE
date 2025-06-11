// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.ILockBytes
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[Guid("0000000a-0000-0000-C000-000000000046")]
[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
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
