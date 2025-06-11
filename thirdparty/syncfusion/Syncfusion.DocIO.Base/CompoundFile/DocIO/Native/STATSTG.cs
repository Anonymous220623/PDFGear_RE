// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.STATSTG
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

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
