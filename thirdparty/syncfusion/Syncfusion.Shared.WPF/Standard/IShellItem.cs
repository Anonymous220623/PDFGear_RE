// Decompiled with JetBrains decompiler
// Type: Standard.IShellItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard;

[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellItem
{
  [return: MarshalAs(UnmanagedType.Interface)]
  object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

  IShellItem GetParent();

  [return: MarshalAs(UnmanagedType.LPWStr)]
  string GetDisplayName(SIGDN sigdnName);

  SFGAO GetAttributes(SFGAO sfgaoMask);

  int Compare(IShellItem psi, SICHINT hint);
}
