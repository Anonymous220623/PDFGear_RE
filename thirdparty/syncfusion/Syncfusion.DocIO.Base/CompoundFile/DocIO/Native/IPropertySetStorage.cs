// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IPropertySetStorage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComConversionLoss]
[Guid("0000013a-0000-0000-c000-000000000046")]
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
