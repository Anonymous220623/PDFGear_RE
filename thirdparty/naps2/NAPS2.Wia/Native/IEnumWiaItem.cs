// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IEnumWiaItem
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("5e8383fc-3391-11d2-9a33-00c04fa36145")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumWiaItem
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] IWiaItem[] ppIWiaItem, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  IEnumWiaItem Clone();

  uint GetCount();
}
