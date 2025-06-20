﻿// Decompiled with JetBrains decompiler
// Type: Standard.IShellFolder
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard;

[Guid("000214E6-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellFolder
{
  void ParseDisplayName(
    [In] IntPtr hwnd,
    [In] IBindCtx pbc,
    [MarshalAs(UnmanagedType.LPWStr), In] string pszDisplayName,
    [In, Out] ref int pchEaten,
    out IntPtr ppidl,
    [In, Out] ref uint pdwAttributes);

  IEnumIDList EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags);

  [return: MarshalAs(UnmanagedType.Interface)]
  object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

  [return: MarshalAs(UnmanagedType.Interface)]
  object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);

  [return: MarshalAs(UnmanagedType.Interface)]
  object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);

  void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref SFGAO rgfInOut);

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetUIObjectOf(
    [In] IntPtr hwndOwner,
    [In] uint cidl,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2, ArraySubType = UnmanagedType.SysInt), In] IntPtr apidl,
    [In] ref Guid riid,
    [In, Out] ref uint rgfReserved);

  void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDN uFlags, out IntPtr pName);

  void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr), In] string pszName, [In] SHGDN uFlags, out IntPtr ppidlOut);
}
