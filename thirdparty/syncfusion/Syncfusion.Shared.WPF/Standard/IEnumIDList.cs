// Decompiled with JetBrains decompiler
// Type: Standard.IEnumIDList
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("000214F2-0000-0000-C000-000000000046")]
[ComImport]
internal interface IEnumIDList
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  HRESULT Skip(uint celt);

  void Reset();

  void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
}
