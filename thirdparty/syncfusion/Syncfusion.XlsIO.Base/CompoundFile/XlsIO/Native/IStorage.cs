// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("0000000b-0000-0000-C000-000000000046")]
public interface IStorage
{
  int CreateStream(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    STGM grfMode,
    uint reserved1,
    uint reserved2,
    ref IStream ppstm);

  int OpenStream(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    uint cbReserved1,
    STGM grfMode,
    uint reserved2,
    out IStream ppstm);

  int CreateStorage(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    STGM grfMode,
    uint reserved1,
    uint reserved2,
    out IStorage ppstg);

  int OpenStorage(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    IntPtr pstgPriority,
    STGM grfMode,
    IntPtr snbExclude,
    uint reserved,
    out IStorage ppstg);

  int CopyTo(uint ciidExclude, IntPtr rgiidExclude, IntPtr snbExclude, IStorage pstgDest);

  int MoveElementTo(string pwcsName, IStorage pstgDest, string pwcsNewName, uint grfFlags);

  int Commit(uint grfCommitFlags);

  int Revert();

  int EnumElements(uint reserved1, IntPtr reserved2, uint reserved3, ref IEnumSTATSTG ppenum);

  int DestroyElement(string pwcsName);

  int RenameElement(string pwcsOldName, string pwcsNewName);

  int SetElementTimes(
    string pwcsName,
    ref System.Runtime.InteropServices.ComTypes.FILETIME pctime,
    ref System.Runtime.InteropServices.ComTypes.FILETIME patime,
    ref System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

  int SetClass(ref Guid clsid);

  int SetStateBits(uint grfStateBits, uint grfMask);

  int Stat(ref STATSTG pstatstg, uint grfStatFlag);
}
