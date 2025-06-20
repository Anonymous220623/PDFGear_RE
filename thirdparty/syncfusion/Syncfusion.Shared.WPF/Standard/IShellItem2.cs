﻿// Decompiled with JetBrains decompiler
// Type: Standard.IShellItem2
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
[ComImport]
internal interface IShellItem2 : IShellItem
{
  [return: MarshalAs(UnmanagedType.Interface)]
  new object BindToHandler([In] IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

  new IShellItem GetParent();

  [return: MarshalAs(UnmanagedType.LPWStr)]
  new string GetDisplayName(SIGDN sigdnName);

  new SFGAO GetAttributes(SFGAO sfgaoMask);

  new int Compare(IShellItem psi, SICHINT hint);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyStore(GPS flags, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

  void Update(IBindCtx pbc);

  PROPVARIANT GetProperty(IntPtr key);

  Guid GetCLSID(IntPtr key);

  System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

  int GetInt32(IntPtr key);

  [return: MarshalAs(UnmanagedType.LPWStr)]
  string GetString(IntPtr key);

  uint GetUInt32(IntPtr key);

  ulong GetUInt64(IntPtr key);

  [return: MarshalAs(UnmanagedType.Bool)]
  void GetBool(IntPtr key);
}
