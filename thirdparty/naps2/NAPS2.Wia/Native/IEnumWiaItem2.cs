// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IEnumWiaItem2
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("59970AF4-CD0D-44d9-AB24-52295630E582")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumWiaItem2
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int Next(uint cElt, [MarshalAs(UnmanagedType.LPArray), Out] IWiaItem2[] ppIWiaItem2, out uint pcEltFetched);

  void Skip(uint cElt);

  void Reset();

  IEnumWiaItem2 Clone();

  uint GetCount();
}
