// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IPropertyStorage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00000138-0000-0000-C000-000000000046")]
[CLSCompliant(false)]
[ComImport]
public interface IPropertyStorage
{
  void ReadMultiple(uint cpspec, [MarshalAs(UnmanagedType.LPArray), In] PROPSPEC[] rgpspec, [MarshalAs(UnmanagedType.LPArray), In, Out] PROPVARIANT[] rgpropvar);

  void WriteMultiple(
    uint cpspec,
    [MarshalAs(UnmanagedType.LPArray), In] PROPSPEC[] rgpspec,
    [MarshalAs(UnmanagedType.LPArray), In] PROPVARIANT[] rgpropvar,
    int propidNameFirst);

  void DeleteMultiple(uint cpspec, ref PROPSPEC rgpspec);

  void ReadPropertyNames(uint cpropid, ref uint rgpropid, [MarshalAs(UnmanagedType.LPWStr)] out string rglpwstrName);

  void WritePropertyNames(uint cpropid, ref uint rgpropid, [MarshalAs(UnmanagedType.LPWStr)] ref string rglpwstrName);

  void DeletePropertyNames(uint cpropid, ref uint rgpropid);

  void Commit(uint grfCommitFlags);

  void Revert();

  void Enum(out IEnumSTATPROPSTG ppenum);

  void SetTimes(ref System.Runtime.InteropServices.ComTypes.FILETIME pctime, ref System.Runtime.InteropServices.ComTypes.FILETIME patime, ref System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

  void SetClass(ref Guid clsid);

  void Stat(ref tagSTATPROPSETSTG pstatpsstg);

  void ReadMultiple(uint cpspec, ref PROPSPEC rgpspec, IntPtr rgpropvar);

  void WriteMultiple(uint cpspec, ref PROPSPEC rgpspec, IntPtr rgpropvar, PID propidNameFirst);

  void Commit(STGC grfCommitFlags);
}
