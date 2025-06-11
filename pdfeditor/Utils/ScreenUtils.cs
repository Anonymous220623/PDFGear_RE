// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ScreenUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public static class ScreenUtils
{
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetDpiForMonitor(HandleRef hMonitor, out uint dpiX, out uint dpiY)
  {
    return ScreenUtils.GetDpiForMonitor(hMonitor, ScreenUtils.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out dpiX, out dpiY) == 0U;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetDpiForWindow(Window window, out uint dpiX, out uint dpiY)
  {
    return ScreenUtils.TryGetDpiForVisual((Visual) window, out dpiX, out dpiY);
  }

  public static bool TryGetDpiForVisual(Visual visual, out uint dpiX, out uint dpiY)
  {
    dpiX = 0U;
    dpiY = 0U;
    if (visual == null)
      return false;
    try
    {
      PresentationSource presentationSource = PresentationSource.FromVisual(visual);
      if (presentationSource != null)
      {
        ref uint local1 = ref dpiX;
        Matrix transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        int num1 = (int) (uint) (transformToDevice.M11 * 96.0);
        local1 = (uint) num1;
        ref uint local2 = ref dpiY;
        transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        int num2 = (int) (uint) (transformToDevice.M22 * 96.0);
        local2 = (uint) num2;
        return true;
      }
    }
    catch
    {
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetMonitorForWindow(Window window, out HandleRef hMonitor)
  {
    ScreenUtils.RECT lpRect;
    if (ScreenUtils.GetWindowRect(new WindowInteropHelper(window).Handle, out lpRect))
    {
      IntPtr handle = ScreenUtils.MonitorFromPoint(new ScreenUtils.POINT((lpRect.left + lpRect.right) / 2, (lpRect.top + lpRect.bottom) / 2), ScreenUtils.MonitorOptions.MONITOR_DEFAULTTONEAREST);
      hMonitor = new HandleRef((object) null, handle);
      return true;
    }
    hMonitor = new HandleRef();
    return false;
  }

  public static bool TryGetWindowRect(IntPtr hwnd, out Int32Rect bounds)
  {
    bounds = Int32Rect.Empty;
    ScreenUtils.RECT lpRect;
    if (!ScreenUtils.GetWindowRect(hwnd, out lpRect))
      return false;
    bounds = (Int32Rect) lpRect;
    return true;
  }

  public static bool GetMonitorInfo(
    IntPtr hMonitor,
    out Int32Rect bounds,
    out Int32Rect workArea,
    out bool primary)
  {
    ScreenUtils.MONITORINFOEXW monitorInfoEx = CreateMonitorInfoEX();
    bounds = Int32Rect.Empty;
    workArea = Int32Rect.Empty;
    primary = false;
    int num = ScreenUtils.GetMonitorInfoW(hMonitor, ref monitorInfoEx) != 0 ? 1 : 0;
    if (num == 0)
      return num != 0;
    bounds = (Int32Rect) monitorInfoEx.rcMonitor;
    workArea = (Int32Rect) monitorInfoEx.rcWork;
    primary = (monitorInfoEx.dwFlags & ScreenUtils.MONITORINFOF.PRIMARY) > (ScreenUtils.MONITORINFOF) 0;
    return num != 0;

    static ScreenUtils.MONITORINFOEXW CreateMonitorInfoEX()
    {
      return new ScreenUtils.MONITORINFOEXW()
      {
        cbSize = (uint) sizeof (ScreenUtils.MONITORINFOEXW)
      };
    }
  }

  [DllImport("shcore.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern uint GetDpiForMonitor(
    HandleRef hMonitor,
    ScreenUtils.MONITOR_DPI_TYPE dpiType,
    out uint dpiX,
    out uint dpiY);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IntPtr MonitorFromPoint(
    ScreenUtils.POINT pt,
    ScreenUtils.MonitorOptions dwFlags);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool GetWindowRect(IntPtr hWnd, out ScreenUtils.RECT lpRect);

  [DllImport("user32")]
  private static extern int GetMonitorInfoW(IntPtr hMonitor, ref ScreenUtils.MONITORINFOEXW lpmi);

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  private struct MONITORINFOEXW
  {
    public uint cbSize;
    public ScreenUtils.RECT rcMonitor;
    public ScreenUtils.RECT rcWork;
    public ScreenUtils.MONITORINFOF dwFlags;
    public unsafe fixed char szDevice[32];
  }

  [Flags]
  public enum MONITORINFOF : uint
  {
    PRIMARY = 1,
  }

  private struct RECT(int left, int top, int right, int bottom)
  {
    public int left = left;
    public int top = top;
    public int right = right;
    public int bottom = bottom;

    public int X => this.left;

    public int Y => this.top;

    public int Width => this.right - this.left;

    public int Height => this.bottom - this.top;

    public Size Size => new Size((double) this.Width, (double) this.Height);

    public override string ToString()
    {
      return $"{{{this.left}, {this.top}, {this.right}, {this.bottom} (LTRB)}}";
    }

    public static implicit operator Int32Rect(ScreenUtils.RECT rect)
    {
      return new Int32Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }
  }

  private struct POINT(int x, int y)
  {
    public int X = x;
    public int Y = y;
  }

  private enum MONITOR_DPI_TYPE
  {
    MDT_DEFAULT = 0,
    MDT_EFFECTIVE_DPI = 0,
    MDT_ANGULAR_DPI = 1,
    MDT_RAW_DPI = 2,
  }

  private enum MonitorOptions : uint
  {
    MONITOR_DEFAULTTONULL,
    MONITOR_DEFAULTTOPRIMARY,
    MONITOR_DEFAULTTONEAREST,
  }
}
