// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.FullScreenHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Controls;

internal static class FullScreenHelper
{
  private static readonly DependencyProperty BeforeFullScreenWindowPlacementProperty = DependencyProperty.RegisterAttached("BeforeFullScreenWindowPlacement", typeof (InteropValues.WINDOWPLACEMENT?), typeof (FullScreenHelper));
  private static readonly DependencyProperty BeforeFullScreenWindowStyleProperty = DependencyProperty.RegisterAttached("BeforeFullScreenWindowStyle", typeof (InteropValues.WindowStyles?), typeof (FullScreenHelper));

  public static void StartFullScreen(System.Windows.Window window)
  {
    if (window == null)
      throw new ArgumentNullException(nameof (window), "window 不能为 null");
    if (window.GetValue(FullScreenHelper.BeforeFullScreenWindowPlacementProperty) != null || window.GetValue(FullScreenHelper.BeforeFullScreenWindowStyleProperty) != null)
      return;
    IntPtr num = new WindowInteropHelper(window).EnsureHandle();
    HwndSource hwndSource = HwndSource.FromHwnd(num);
    InteropValues.WINDOWPLACEMENT windowPlacement = InteropMethods.GetWindowPlacement(num);
    window.SetValue(FullScreenHelper.BeforeFullScreenWindowPlacementProperty, (object) windowPlacement);
    InteropValues.WindowStyles windowLongPtr = (InteropValues.WindowStyles) (int) InteropMethods.GetWindowLongPtr(num, -16);
    window.SetValue(FullScreenHelper.BeforeFullScreenWindowStyleProperty, (object) windowLongPtr);
    InteropValues.WindowStyles dwNewLong = windowLongPtr & ~(InteropValues.WindowStyles.WS_MAXIMIZE | InteropValues.WindowStyles.WS_MAXIMIZEBOX | InteropValues.WindowStyles.WS_THICKFRAME);
    InteropMethods.SetWindowLong(num, -16, (IntPtr) (int) dwNewLong);
    InteropMethods.DwmSetWindowAttribute(num, InteropValues.DwmWindowAttribute.DWMWA_TRANSITIONS_FORCEDISABLED, 1, 4U);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    hwndSource.AddHook(FullScreenHelper.\u003C\u003EO.\u003C0\u003E__KeepFullScreenHook ?? (FullScreenHelper.\u003C\u003EO.\u003C0\u003E__KeepFullScreenHook = new HwndSourceHook(FullScreenHelper.KeepFullScreenHook)));
    InteropValues.RECT lpRect;
    if (!InteropMethods.GetWindowRect(num, out lpRect))
      return;
    InteropMethods.SetWindowPos(num, (IntPtr) 0, lpRect.Left, lpRect.Top, lpRect.Width, lpRect.Height, 4);
  }

  public static void EndFullScreen(System.Windows.Window window)
  {
    if (window == null)
      throw new ArgumentNullException(nameof (window), "window 不能为 null");
    if (!(window.GetValue(FullScreenHelper.BeforeFullScreenWindowPlacementProperty) is InteropValues.WINDOWPLACEMENT placement) || !(window.GetValue(FullScreenHelper.BeforeFullScreenWindowStyleProperty) is InteropValues.WindowStyles windowStyles))
      return;
    IntPtr handle = new WindowInteropHelper(window).Handle;
    if (handle == IntPtr.Zero)
      return;
    HwndSource hwndSource = HwndSource.FromHwnd(handle);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    hwndSource.RemoveHook(FullScreenHelper.\u003C\u003EO.\u003C0\u003E__KeepFullScreenHook ?? (FullScreenHelper.\u003C\u003EO.\u003C0\u003E__KeepFullScreenHook = new HwndSourceHook(FullScreenHelper.KeepFullScreenHook)));
    InteropMethods.SetWindowLong(handle, -16, (IntPtr) (int) (windowStyles & ~(InteropValues.WindowStyles.WS_MAXIMIZE | InteropValues.WindowStyles.WS_MINIMIZE)));
    if ((windowStyles & InteropValues.WindowStyles.WS_MINIMIZE) != (InteropValues.WindowStyles) 0)
      placement.showCmd = InteropValues.SW.RESTORE;
    if ((windowStyles & InteropValues.WindowStyles.WS_MAXIMIZE) != (InteropValues.WindowStyles) 0)
      InteropMethods.ShowWindow(handle, InteropValues.SW.SHOWMAXIMIZED);
    InteropMethods.SetWindowPlacement(handle, ref placement);
    InteropValues.RECT lpRect;
    if ((windowStyles & InteropValues.WindowStyles.WS_MAXIMIZE) == (InteropValues.WindowStyles) 0 && InteropMethods.GetWindowRect(handle, out lpRect))
    {
      Point point1 = hwndSource.CompositionTarget.TransformFromDevice.Transform(new Point((double) lpRect.Left, (double) lpRect.Top));
      Point point2 = hwndSource.CompositionTarget.TransformFromDevice.Transform(new Point((double) lpRect.Width, (double) lpRect.Height));
      window.Left = point1.X;
      window.Top = point1.Y;
      window.Width = point2.X;
      window.Height = point2.Y;
    }
    InteropMethods.DwmSetWindowAttribute(handle, InteropValues.DwmWindowAttribute.DWMWA_TRANSITIONS_FORCEDISABLED, 0, 4U);
    window.ClearValue(FullScreenHelper.BeforeFullScreenWindowPlacementProperty);
    window.ClearValue(FullScreenHelper.BeforeFullScreenWindowStyleProperty);
  }

  [HandleProcessCorruptedStateExceptions]
  private static IntPtr KeepFullScreenHook(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (msg == 70)
    {
      try
      {
        InteropValues.WindowPosition structure1 = (InteropValues.WindowPosition) Marshal.PtrToStructure(lParam, typeof (InteropValues.WindowPosition));
        InteropValues.RECT lpRect;
        if ((structure1.Flags & InteropValues.WindowPositionFlags.SWP_NOMOVE) != (InteropValues.WindowPositionFlags) 0 && (structure1.Flags & InteropValues.WindowPositionFlags.SWP_NOSIZE) != (InteropValues.WindowPositionFlags) 0 || InteropMethods.IsIconic(hwnd) || !InteropMethods.GetWindowRect(hwnd, out lpRect))
          return IntPtr.Zero;
        InteropValues.RECT rect = lpRect;
        if ((structure1.Flags & InteropValues.WindowPositionFlags.SWP_NOMOVE) == (InteropValues.WindowPositionFlags) 0)
        {
          rect.Left = structure1.X;
          rect.Top = structure1.Y;
        }
        if ((structure1.Flags & InteropValues.WindowPositionFlags.SWP_NOSIZE) == (InteropValues.WindowPositionFlags) 0)
        {
          rect.Right = rect.Left + structure1.Width;
          rect.Bottom = rect.Top + structure1.Height;
        }
        else
        {
          rect.Right = rect.Left + lpRect.Width;
          rect.Bottom = rect.Top + lpRect.Height;
        }
        IntPtr hMonitor = InteropMethods.MonitorFromRect(ref rect, 1);
        InteropValues.MONITORINFO structure2 = new InteropValues.MONITORINFO();
        structure2.cbSize = (uint) Marshal.SizeOf<InteropValues.MONITORINFO>(structure2);
        ref InteropValues.MONITORINFO local = ref structure2;
        if (InteropMethods.GetMonitorInfo(hMonitor, ref local))
        {
          structure1.X = structure2.rcMonitor.Left;
          structure1.Y = structure2.rcMonitor.Top;
          structure1.Width = structure2.rcMonitor.Right - structure2.rcMonitor.Left;
          structure1.Height = structure2.rcMonitor.Bottom - structure2.rcMonitor.Top;
          structure1.Flags &= ~(InteropValues.WindowPositionFlags.SWP_NOMOVE | InteropValues.WindowPositionFlags.SWP_NOREDRAW | InteropValues.WindowPositionFlags.SWP_NOSIZE);
          structure1.Flags |= InteropValues.WindowPositionFlags.SWP_NOCOPYBITS;
          if (lpRect == structure2.rcMonitor)
          {
            HwndSource hwndSource = HwndSource.FromHwnd(hwnd);
            if (hwndSource?.RootVisual is Window rootVisual)
            {
              Point point1 = hwndSource.CompositionTarget.TransformFromDevice.Transform(new Point((double) structure1.X, (double) structure1.Y));
              Point point2 = hwndSource.CompositionTarget.TransformFromDevice.Transform(new Point((double) structure1.Width, (double) structure1.Height));
              rootVisual.Left = point1.X;
              rootVisual.Top = point1.Y;
              rootVisual.Width = point2.X;
              rootVisual.Height = point2.Y;
            }
          }
          Marshal.StructureToPtr<InteropValues.WindowPosition>(structure1, lParam, false);
        }
      }
      catch
      {
      }
    }
    return IntPtr.Zero;
  }
}
