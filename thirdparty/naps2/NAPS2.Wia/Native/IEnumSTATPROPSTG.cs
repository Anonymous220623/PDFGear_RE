// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IEnumSTATPROPSTG
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("00000139-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumSTATPROPSTG
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] STATPROPSTG[] rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  IEnumSTATPROPSTG Clone();
}
