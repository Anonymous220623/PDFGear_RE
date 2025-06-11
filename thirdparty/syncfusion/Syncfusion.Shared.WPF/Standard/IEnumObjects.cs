// Decompiled with JetBrains decompiler
// Type: Standard.IEnumObjects
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumObjects
{
  void Next(uint celt, [In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown), Out] object[] rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  IEnumObjects Clone();
}
