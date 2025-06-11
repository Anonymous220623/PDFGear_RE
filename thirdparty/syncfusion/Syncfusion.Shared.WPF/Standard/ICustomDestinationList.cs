// Decompiled with JetBrains decompiler
// Type: Standard.ICustomDestinationList
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
[ComImport]
internal interface ICustomDestinationList
{
  void SetAppID([MarshalAs(UnmanagedType.LPWStr), In] string pszAppID);

  [return: MarshalAs(UnmanagedType.Interface)]
  object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

  void AppendKnownCategory(KDC category);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  HRESULT AddUserTasks(IObjectArray poa);

  void CommitList();

  [return: MarshalAs(UnmanagedType.Interface)]
  object GetRemovedDestinations([In] ref Guid riid);

  void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

  void AbortList();
}
