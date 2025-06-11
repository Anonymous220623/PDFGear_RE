// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IPropertySetStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[ComConversionLoss]
[CLSCompliant(false)]
[Guid("0000013a-0000-0000-c000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPropertySetStorage
{
  void Create(
    ref Guid rfmtid,
    ref Guid pclsid,
    uint grfFlags,
    STGM grfMode,
    out IPropertyStorage ppprstg);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Open(ref Guid rfmtid, STGM grfMode, out IPropertyStorage ppprstg);

  void Delete(ref Guid rfmtid);

  void Enum(out IEnumSTATPROPSETSTG ppenum);
}
