// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IEnumWIA_DEV_CAPS
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("1fcc4287-aca6-11d2-a093-00c04f72dc3c")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumWIA_DEV_CAPS
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] WIA_DEV_CAP[] rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  IEnumWIA_DEV_CAPS Clone();

  uint GetCount();
}
