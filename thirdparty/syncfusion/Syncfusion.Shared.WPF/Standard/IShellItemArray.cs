// Decompiled with JetBrains decompiler
// Type: Standard.IShellItemArray
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard;

[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellItemArray
{
  [return: MarshalAs(UnmanagedType.Interface)]
  object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyStore(int flags, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

  uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

  uint GetCount();

  IShellItem GetItemAt(uint dwIndex);

  [return: MarshalAs(UnmanagedType.Interface)]
  object EnumItems();
}
