// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.API
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[CLSCompliant(false)]
public sealed class API
{
  private API()
  {
  }

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int StgOpenStorage(
    [MarshalAs(UnmanagedType.LPWStr)] string wcsName,
    IntPtr stgPriority,
    STGM grfMode,
    IntPtr snbExclude,
    uint reserved,
    out IStorage storage);

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int StgOpenStorageEx(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    STGM grfMode,
    STGFMT stgfmt,
    uint grfAttrs,
    IntPtr stgOptions,
    IntPtr reserved2,
    ref Guid riid,
    out IStorage ppObjectOpen);

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int StgCreateDocfile(
    [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
    STGM grfMode,
    uint reserved,
    out IStorage ppstgOpen);

  [DllImport("ole32.dll", EntryPoint = "StgCreatePropSetStg", SetLastError = true)]
  private static extern int _StgCreatePropSetStg64(
    IStorage pStorage,
    uint dwReserved,
    out IPropertySetStorage ppPropSetStg);

  [DllImport("iprop.dll", EntryPoint = "StgCreatePropSetStg", SetLastError = true)]
  private static extern int _StgCreatePropSetStg32(
    IStorage pStorage,
    uint dwReserved,
    out IPropertySetStorage ppPropSetStg);

  public static int StgCreatePropSetStg(
    IStorage pStorage,
    uint dwReserved,
    out IPropertySetStorage ppPropSetStg)
  {
    return IntPtr.Size == 8 ? API._StgCreatePropSetStg64(pStorage, dwReserved, out ppPropSetStg) : API._StgCreatePropSetStg32(pStorage, dwReserved, out ppPropSetStg);
  }

  public static int StgCreatePropSetStgOle(
    IStorage pStorage,
    uint dwReserved,
    out IPropertySetStorage ppPropSetStg)
  {
    return IntPtr.Size == 8 ? API._StgCreatePropSetStg64(pStorage, dwReserved, out ppPropSetStg) : API._StgCreatePropSetStg32(pStorage, dwReserved, out ppPropSetStg);
  }

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int CreateILockBytesOnHGlobal(
    IntPtr hGlobal,
    bool fDeleteOnRelease,
    out ILockBytes ppLkbyt);

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int StgCreateDocfileOnILockBytes(
    ILockBytes plkbyt,
    STGM grfMode,
    int reserved,
    out IStorage ppstgOpen);

  [DllImport("ole32.dll", SetLastError = true)]
  public static extern int StgOpenStorageOnILockBytes(
    ILockBytes plkbyt,
    IStorage pStgPriority,
    STGM grfMode,
    int snbExclude,
    int reserved,
    out IStorage ppstgOpen);

  [DllImport("kernel32.dll", SetLastError = true)]
  public static extern IntPtr GlobalAlloc(API.GlobalAllocFlags flags, int size);

  [DllImport("kernel32.dll", SetLastError = true)]
  public static extern IntPtr GlobalReAlloc(IntPtr hMem, int bytes, API.GlobalAllocFlags flags);

  [DllImport("kernel32.dll", SetLastError = true)]
  public static extern IntPtr GlobalFree(IntPtr hMem);

  [DllImport("ole32.dll")]
  internal static extern int StgCreateStorageEx(
    [MarshalAs(UnmanagedType.LPWStr)] string wcsName,
    STGM grfMode,
    STGFMT stgfmt,
    int grfAttrs,
    IntPtr pStgOptions,
    IntPtr reserved2,
    [In] ref Guid riid,
    out IStorage storage);

  [Flags]
  public enum GlobalAllocFlags
  {
    GMEM_FIXED = 0,
    GMEM_MOVEABLE = 2,
    GMEM_ZEROINIT = 64, // 0x00000040
    GMEM_NODISCARD = 32, // 0x00000020
  }
}
