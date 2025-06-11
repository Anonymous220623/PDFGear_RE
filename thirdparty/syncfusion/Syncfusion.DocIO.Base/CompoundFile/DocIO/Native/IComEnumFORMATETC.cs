// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IComEnumFORMATETC
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Guid("00000103-0000-0000-c000-000000000046")]
[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IComEnumFORMATETC
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint Clone(ref IComEnumFORMATETC ppenum);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint RemoteNext(uint celt, ref FORMATETC rgelt, ref uint pceltFetched);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint Reset();

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint Skip(uint celt);
}
