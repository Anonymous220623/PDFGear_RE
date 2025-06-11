// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IComDataObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("0000010e-0000-0000-C000-000000000046")]
[CLSCompliant(false)]
[ComImport]
public interface IComDataObject
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint GetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pRemoteMedium);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint GetDataHere(ref FORMATETC pformatetc, ref STGMEDIUM pRemoteMedium);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint QueryGetData(ref FORMATETC pformatetc);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint GetCanonicalFormatEtc(ref FORMATETC pformatectIn, ref FORMATETC pformatetcOut);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, int fRelease);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint EnumFormatEtc(uint dwDirection, ref IComEnumFORMATETC ppenumFormatEtc);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint DAdvise(ref FORMATETC pformatetc, uint advf, IntPtr pAdvSink, ref uint pdwConnection);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint DUnadvise(uint dwConnection);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  uint EnumDAdvise(ref IntPtr ppenumAdvise);
}
