// Decompiled with JetBrains decompiler
// Type: Standard.ITaskbarList
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface ITaskbarList
{
  void HrInit();

  void AddTab(IntPtr hwnd);

  void DeleteTab(IntPtr hwnd);

  void ActivateTab(IntPtr hwnd);

  void SetActiveAlt(IntPtr hwnd);
}
