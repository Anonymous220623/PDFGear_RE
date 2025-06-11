// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaPropertyStorage
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("98B5E8A0-29CC-491a-AAC0-E6DB4FDCCEB6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaPropertyStorage
{
  void ReadMultiple(uint cpspec, [MarshalAs(UnmanagedType.LPArray)] PROPSPEC[] rgpspec, [MarshalAs(UnmanagedType.LPArray), Out] PROPVARIANT[] rgpropvar);

  void WriteMultiple(
    uint cpspec,
    [MarshalAs(UnmanagedType.LPArray)] PROPSPEC[] rgpspec,
    [MarshalAs(UnmanagedType.LPArray)] PROPVARIANT[] rgpropvar,
    uint propidNameFirst);

  void DeleteMultiple(uint cpspec, [MarshalAs(UnmanagedType.LPArray)] PROPSPEC[] rgpspec);

  void ReadPropertyNames(uint cpropid, [MarshalAs(UnmanagedType.LPArray)] uint[] rgpropid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr), Out] string[] rglpwstrName);

  void WritePropertyNames(uint cpropid, [MarshalAs(UnmanagedType.LPArray)] uint[] rgpropid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] rglpwstrName);

  void DeletePropertyNames(uint cpropid, [MarshalAs(UnmanagedType.LPArray)] uint[] rgpropid);

  void Commit(uint grfCommitFlags);

  void Revert();

  IEnumSTATPROPSTG? Enum();

  void SetTimes(in System.Runtime.InteropServices.ComTypes.FILETIME pctime, in System.Runtime.InteropServices.ComTypes.FILETIME patime, in System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

  void SetClass(in Guid clsid);

  void Stat(out STATPROPSETSTG pstatpsstg);

  void GetPropertyAttributes(
    uint cpspec,
    [MarshalAs(UnmanagedType.LPArray)] PROPSPEC[] rgpspec,
    [MarshalAs(UnmanagedType.LPArray), Out] uint[] rgflags,
    [MarshalAs(UnmanagedType.LPArray), Out] PROPVARIANT[] rgpropvar);

  uint GetCount();

  void GetPropertyStream(out Guid pCompatibilityId, out IStream ppIStream);

  void SetPropertyStream(in Guid pCompatibilityId, IStream pIStream);
}
