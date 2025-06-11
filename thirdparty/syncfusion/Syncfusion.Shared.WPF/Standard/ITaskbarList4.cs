// Decompiled with JetBrains decompiler
// Type: Standard.ITaskbarList4
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
{
  new void HrInit();

  new void AddTab(IntPtr hwnd);

  new void DeleteTab(IntPtr hwnd);

  new void ActivateTab(IntPtr hwnd);

  new void SetActiveAlt(IntPtr hwnd);

  new void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT UnregisterTab(IntPtr hwndTab);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  new HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);

  void SetTabProperties(IntPtr hwndTab, STPF stpFlags);
}
