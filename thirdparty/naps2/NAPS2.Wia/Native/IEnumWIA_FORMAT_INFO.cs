// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IEnumWIA_FORMAT_INFO
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("81BEFC5B-656D-44f1-B24C-D41D51B4DC81")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumWIA_FORMAT_INFO
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] WIA_FORMAT_INFO[] rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  IEnumWIA_FORMAT_INFO Clone();

  uint GetCount();
}
