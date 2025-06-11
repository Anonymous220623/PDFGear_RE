// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.FullScreenHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

public class FullScreenHelper
{
  private static ConcurrentDictionary<IntPtr, FullScreenHelper.WindowData> windowDict;
  public static readonly DependencyProperty IsFullScreenEnabledProperty = DependencyProperty.RegisterAttached("IsFullScreenEnabled", typeof (bool), typeof (FullScreenHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(FullScreenHelper.OnIsFullScreenPropertyChanged)));

  public static bool GetIsFullScreenEnabled(Window obj)
  {
    return (bool) obj.GetValue(FullScreenHelper.IsFullScreenEnabledProperty);
  }

  public static void SetIsFullScreenEnabled(Window obj, bool value)
  {
    obj.SetValue(FullScreenHelper.IsFullScreenEnabledProperty, (object) value);
  }

  private static void OnIsFullScreenPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue))
      return;
    Window sender = d as Window;
    if (sender == null)
      return;
    if (e.NewValue is bool newValue && newValue)
      sender.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => FullScreenHelper.EnterFullScreenMode(sender)));
    else
      sender.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => FullScreenHelper.ExitFullScreenMode(sender)));
  }

  private static void EnterFullScreenMode(Window window)
  {
    if (window == null)
      return;
    FullScreenHelper.WindowData fromWindow = FullScreenHelper.WindowData.CreateFromWindow(window);
    HandleRef hMonitor;
    Int32Rect bounds;
    if (fromWindow != null && (FullScreenHelper.windowDict == null || !FullScreenHelper.windowDict.ContainsKey(fromWindow.Handle)) && ScreenUtils.TryGetMonitorForWindow(window, out hMonitor) && ScreenUtils.GetMonitorInfo(hMonitor.Handle, out bounds, out Int32Rect _, out bool _))
    {
      if (FullScreenHelper.windowDict == null)
      {
        lock (typeof (FullScreenHelper))
        {
          if (FullScreenHelper.windowDict == null)
            FullScreenHelper.windowDict = new ConcurrentDictionary<IntPtr, FullScreenHelper.WindowData>();
        }
      }
      FullScreenHelper.windowDict[fromWindow.Handle] = fromWindow;
      window.Topmost = false;
      window.ResizeMode = ResizeMode.NoResize;
      window.WindowStyle = WindowStyle.None;
      window.WindowState = WindowState.Maximized;
      FullScreenHelper.SetWindowPos(fromWindow.Handle, (IntPtr) -1, bounds.X, bounds.Y, bounds.Width, bounds.Height, FullScreenHelper.SetWindowPosFlags.SWP_NOZORDER);
      window.Activate();
      window.StateChanged -= new EventHandler(FullScreenHelper.Window_StateChanged);
      window.StateChanged += new EventHandler(FullScreenHelper.Window_StateChanged);
    }
    else
      FullScreenHelper.SetIsFullScreenEnabled(window, false);
  }

  private static void ExitFullScreenMode(Window window)
  {
    if (window == null)
      return;
    window.StateChanged -= new EventHandler(FullScreenHelper.Window_StateChanged);
    IntPtr num = IntPtr.Zero;
    try
    {
      num = new WindowInteropHelper(window).Handle;
    }
    catch
    {
    }
    if (num == IntPtr.Zero)
      return;
    window.ResizeMode = ResizeMode.CanResize;
    window.WindowStyle = WindowStyle.SingleBorderWindow;
    window.Topmost = false;
    Int32Rect bounds = Int32Rect.Empty;
    WindowState windowState = WindowState.Normal;
    bool flag1 = false;
    FullScreenHelper.WindowData windowData;
    if (FullScreenHelper.windowDict != null && FullScreenHelper.windowDict.TryRemove(num, out windowData))
    {
      bounds = windowData.WindowBounds;
      windowState = windowData.WindowState;
      flag1 = true;
    }
    else
      ScreenUtils.TryGetWindowRect(num, out bounds);
    window.WindowState = windowState;
    bool flag2 = false;
    HandleRef hMonitor;
    Int32Rect workArea;
    if (ScreenUtils.TryGetMonitorForWindow(window, out hMonitor) && ScreenUtils.GetMonitorInfo(hMonitor.Handle, out Int32Rect _, out workArea, out bool _))
    {
      if (bounds.Width > workArea.Width)
      {
        flag2 = true;
        bounds.Width = workArea.Width;
      }
      if (bounds.Height > workArea.Height)
      {
        flag2 = true;
        bounds.Height = workArea.Height;
      }
    }
    if (!(flag1 | flag2))
      return;
    FullScreenHelper.SetWindowPos(num, (IntPtr) -2, bounds.X, bounds.Y, bounds.Width, bounds.Height, (FullScreenHelper.SetWindowPosFlags) 0);
  }

  private static void Window_StateChanged(object sender, EventArgs e)
  {
    FullScreenHelper.SetIsFullScreenEnabled((Window) sender, false);
  }

  internal static void SetWindowPositionAndSize(IntPtr hwnd, System.Drawing.Point? position, System.Drawing.Size? size)
  {
    if (!position.HasValue && !size.HasValue)
      return;
    FullScreenHelper.SetWindowPosFlags setWindowPosFlags = FullScreenHelper.SetWindowPosFlags.SWP_NOACTIVATE | FullScreenHelper.SetWindowPosFlags.SWP_NOZORDER;
    if (!position.HasValue)
      setWindowPosFlags |= FullScreenHelper.SetWindowPosFlags.SWP_NOMOVE;
    if (!size.HasValue)
      setWindowPosFlags |= FullScreenHelper.SetWindowPosFlags.SWP_NOSIZE;
    IntPtr hWnd = hwnd;
    IntPtr zero = IntPtr.Zero;
    System.Drawing.Point valueOrDefault1;
    int X;
    if (!position.HasValue)
    {
      X = 0;
    }
    else
    {
      valueOrDefault1 = position.GetValueOrDefault();
      X = valueOrDefault1.X;
    }
    int Y;
    if (!position.HasValue)
    {
      Y = 0;
    }
    else
    {
      valueOrDefault1 = position.GetValueOrDefault();
      Y = valueOrDefault1.Y;
    }
    System.Drawing.Size valueOrDefault2;
    int cx;
    if (!size.HasValue)
    {
      cx = 0;
    }
    else
    {
      valueOrDefault2 = size.GetValueOrDefault();
      cx = valueOrDefault2.Width;
    }
    int cy;
    if (!size.HasValue)
    {
      cy = 0;
    }
    else
    {
      valueOrDefault2 = size.GetValueOrDefault();
      cy = valueOrDefault2.Height;
    }
    int uFlags = (int) setWindowPosFlags;
    FullScreenHelper.SetWindowPos(hWnd, zero, X, Y, cx, cy, (FullScreenHelper.SetWindowPosFlags) uFlags);
  }

  [DllImport("user32.dll", SetLastError = true)]
  private static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int X,
    int Y,
    int cx,
    int cy,
    FullScreenHelper.SetWindowPosFlags uFlags);

  private class WindowData
  {
    public IntPtr Handle { get; private set; }

    public WindowState WindowState { get; private set; }

    public Int32Rect WindowBounds { get; private set; }

    public static FullScreenHelper.WindowData CreateFromWindow(Window window)
    {
      IntPtr hwnd = IntPtr.Zero;
      try
      {
        hwnd = new WindowInteropHelper(window).EnsureHandle();
      }
      catch
      {
      }
      if (hwnd == IntPtr.Zero)
        return (FullScreenHelper.WindowData) null;
      Int32Rect bounds;
      if (!ScreenUtils.TryGetWindowRect(hwnd, out bounds))
        return (FullScreenHelper.WindowData) null;
      return new FullScreenHelper.WindowData()
      {
        Handle = hwnd,
        WindowState = window.WindowState,
        WindowBounds = bounds
      };
    }
  }

  private enum SpecialWindowHandles
  {
    HWND_NOTOPMOST = -2, // 0xFFFFFFFE
    HWND_TOPMOST = -1, // 0xFFFFFFFF
    HWND_TOP = 0,
    HWND_BOTTOM = 1,
  }

  [Flags]
  private enum SetWindowPosFlags : uint
  {
    SWP_ASYNCWINDOWPOS = 16384, // 0x00004000
    SWP_DEFERERASE = 8192, // 0x00002000
    SWP_DRAWFRAME = 32, // 0x00000020
    SWP_FRAMECHANGED = SWP_DRAWFRAME, // 0x00000020
    SWP_HIDEWINDOW = 128, // 0x00000080
    SWP_NOACTIVATE = 16, // 0x00000010
    SWP_NOCOPYBITS = 256, // 0x00000100
    SWP_NOMOVE = 2,
    SWP_NOOWNERZORDER = 512, // 0x00000200
    SWP_NOREDRAW = 8,
    SWP_NOREPOSITION = SWP_NOOWNERZORDER, // 0x00000200
    SWP_NOSENDCHANGING = 1024, // 0x00000400
    SWP_NOSIZE = 1,
    SWP_NOZORDER = 4,
    SWP_SHOWWINDOW = 64, // 0x00000040
  }
}
