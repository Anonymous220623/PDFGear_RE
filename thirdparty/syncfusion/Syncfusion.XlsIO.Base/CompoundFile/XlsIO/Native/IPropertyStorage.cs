// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IPropertyStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CLSCompliant(false)]
[Guid("00000138-0000-0000-c000-000000000046")]
[ComConversionLoss]
public interface IPropertyStorage
{
  void ReadMultiple(uint cpspec, ref PROPSPEC rgpspec, IntPtr rgpropvar);

  void WriteMultiple(uint cpspec, ref PROPSPEC rgpspec, IntPtr rgpropvar, PID propidNameFirst);

  void DeleteMultiple(uint cpspec, ref PROPSPEC rgpspec);

  void ReadPropertyNames(uint cpropid, ref uint rgpropid, [MarshalAs(UnmanagedType.LPWStr)] out string rglpwstrName);

  void WritePropertyNames(uint cpropid, ref uint rgpropid, [MarshalAs(UnmanagedType.LPWStr)] ref string rglpwstrName);

  void DeletePropertyNames(uint cpropid, ref uint rgpropid);

  void Commit(STGC grfCommitFlags);

  void Revert();

  int Enum(out IEnumSTATPROPSTG ppenum);

  void SetTimes(ref System.Runtime.InteropServices.ComTypes.FILETIME pctime, ref System.Runtime.InteropServices.ComTypes.FILETIME patime, ref System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

  void SetClass(ref Guid clsid);

  void Stat(ref tagSTATPROPSETSTG pstatpsstg);
}
