// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.WindowHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Tools;

public static class WindowHelper
{
  private static readonly BitArray _cacheValid = new BitArray(119);
  private static bool _setDpiX = true;
  private static bool _dpiInitialized;
  private static readonly object _dpiLock = new object();
  private static int _dpi;
  private static int _dpiX;
  private static Thickness _windowResizeBorderThickness;

  public static System.Windows.Window GetActiveWindow()
  {
    IntPtr activeWindow = InteropMethods.GetActiveWindow();
    return Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault<System.Windows.Window>((Func<System.Windows.Window, bool>) (x => WindowHelper.GetHandle(x) == activeWindow));
  }

  internal static int Dpi
  {
    [SecurityCritical, SecuritySafeCritical] get
    {
      if (!WindowHelper._dpiInitialized)
      {
        lock (WindowHelper._dpiLock)
        {
          if (!WindowHelper._dpiInitialized)
          {
            HandleRef hWnd = new HandleRef((object) null, IntPtr.Zero);
            IntPtr dc = InteropMethods.GetDC(hWnd);
            if (dc == IntPtr.Zero)
              throw new Win32Exception();
            try
            {
              WindowHelper._dpi = InteropMethods.GetDeviceCaps(new HandleRef((object) null, dc), 90);
              WindowHelper._dpiInitialized = true;
            }
            finally
            {
              InteropMethods.ReleaseDC(hWnd, new HandleRef((object) null, dc));
            }
          }
        }
      }
      return WindowHelper._dpi;
    }
  }

  internal static int DpiX
  {
    [SecurityCritical, SecuritySafeCritical] get
    {
      if (WindowHelper._setDpiX)
      {
        lock (WindowHelper._cacheValid)
        {
          if (WindowHelper._setDpiX)
          {
            WindowHelper._setDpiX = false;
            HandleRef hWnd = new HandleRef((object) null, IntPtr.Zero);
            IntPtr dc = InteropMethods.GetDC(hWnd);
            if (dc == IntPtr.Zero)
              throw new Win32Exception();
            try
            {
              WindowHelper._dpiX = InteropMethods.GetDeviceCaps(new HandleRef((object) null, dc), 88);
              WindowHelper._cacheValid[0] = true;
            }
            finally
            {
              InteropMethods.ReleaseDC(hWnd, new HandleRef((object) null, dc));
            }
          }
        }
      }
      return WindowHelper._dpiX;
    }
  }

  internal static Thickness WindowResizeBorderThickness
  {
    [SecurityCritical] get
    {
      lock (WindowHelper._cacheValid)
      {
        while (!WindowHelper._cacheValid[118])
        {
          WindowHelper._cacheValid[118] = true;
          Size logical = DpiHelper.DeviceSizeToLogical(new Size((double) InteropMethods.GetSystemMetrics(InteropValues.SM.CXFRAME), (double) InteropMethods.GetSystemMetrics(InteropValues.SM.CYFRAME)), (double) WindowHelper.DpiX / 96.0, (double) WindowHelper.Dpi / 96.0);
          WindowHelper._windowResizeBorderThickness = new Thickness(logical.Width, logical.Height, logical.Width, logical.Height);
        }
      }
      return WindowHelper._windowResizeBorderThickness;
    }
  }

  public static Thickness WindowMaximizedPadding
  {
    get
    {
      InteropValues.APPBARDATA pData = new InteropValues.APPBARDATA();
      return WindowHelper.WindowResizeBorderThickness.Add(new Thickness(InteropMethods.SHAppBarMessage(4, ref pData) > 0U ? -4.0 : 4.0));
    }
  }

  public static IntPtr CreateHandle() => new WindowInteropHelper(new System.Windows.Window()).EnsureHandle();

  public static IntPtr GetHandle(this System.Windows.Window window)
  {
    return new WindowInteropHelper(window).EnsureHandle();
  }

  public static HwndSource GetHwndSource(this System.Windows.Window window)
  {
    return HwndSource.FromHwnd(WindowHelper.GetHandle(window));
  }

  public static void SetWindowToForeground(System.Windows.Window window)
  {
    uint lpdwProcessId;
    uint thisWindowThreadId = InteropMethods.GetWindowThreadProcessId(new WindowInteropHelper(window).Handle, out lpdwProcessId);
    uint currentForegroundWindowThreadId = InteropMethods.GetWindowThreadProcessId(InteropMethods.GetForegroundWindow(), out lpdwProcessId);
    InteropMethods.AttachThreadInput(in currentForegroundWindowThreadId, in thisWindowThreadId, true);
    window.Show();
    window.Activate();
    InteropMethods.AttachThreadInput(in currentForegroundWindowThreadId, in thisWindowThreadId, false);
    if (window.Topmost)
      return;
    window.Topmost = true;
    window.Topmost = false;
  }

  public static void TouchDragMove(this System.Windows.Window window)
  {
    new TouchDragMoveWindowHelper(window).Start();
  }

  public static void StartFullScreen(this System.Windows.Window window)
  {
    FullScreenHelper.StartFullScreen(window);
  }

  public static void EndFullScreen(this System.Windows.Window window)
  {
    FullScreenHelper.EndFullScreen(window);
  }
}
