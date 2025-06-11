// Decompiled with JetBrains decompiler
// Type: Standard.ITaskbarList2
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface ITaskbarList2 : ITaskbarList
{
  new void HrInit();

  new void AddTab(IntPtr hwnd);

  new void DeleteTab(IntPtr hwnd);

  new void ActivateTab(IntPtr hwnd);

  new void SetActiveAlt(IntPtr hwnd);

  void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
}
