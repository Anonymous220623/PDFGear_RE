// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.STATSTG
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[CLSCompliant(false)]
public struct STATSTG
{
  [MarshalAs(UnmanagedType.LPWStr)]
  public string pwcsName;
  public STGTY type;
  public ulong cbSize;
  public System.Runtime.InteropServices.ComTypes.FILETIME mtime;
  public System.Runtime.InteropServices.ComTypes.FILETIME ctime;
  public System.Runtime.InteropServices.ComTypes.FILETIME atime;
  public uint grfMode;
  public LOCKTYPE grfLocksSupported;
  public Guid clsid;
  public uint grfStateBits;
  public uint reserved;
}
