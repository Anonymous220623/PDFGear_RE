// Decompiled with JetBrains decompiler
// Type: Standard.IObjectArray
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
[ComImport]
internal interface IObjectArray
{
  uint GetCount();

  [return: MarshalAs(UnmanagedType.IUnknown)]
  object GetAt([In] uint uiIndex, [In] ref Guid riid);
}
