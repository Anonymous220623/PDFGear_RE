// Decompiled with JetBrains decompiler
// Type: Standard.IApplicationDocumentLists
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IApplicationDocumentLists
{
  void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

  [return: MarshalAs(UnmanagedType.IUnknown)]
  object GetList([In] APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);
}
