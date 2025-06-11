// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.NativeMethods
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils;

internal class NativeMethods
{
  public const int GWL_STYLE = -16;

  [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
  private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
  private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
  private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
  private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

  public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
  {
    return IntPtr.Size == 8 ? NativeMethods.GetWindowLongPtr64(hWnd, nIndex) : NativeMethods.GetWindowLongPtr32(hWnd, nIndex);
  }

  public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
  {
    return IntPtr.Size == 8 ? NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : new IntPtr(NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
  }

  [DllImport("gdi32.dll")]
  public static extern bool DeleteObject(IntPtr hObject);

  [DllImport("user32.dll", SetLastError = true)]
  private static extern IntPtr MonitorFromPoint(
    NativeMethods.POINT pt,
    NativeMethods.MonitorOptions dwFlags);

  public static IntPtr MonitorFromPoint(Point pt, NativeMethods.MonitorOptions dwFlags)
  {
    return NativeMethods.MonitorFromPoint(new NativeMethods.POINT(pt.X, pt.Y), dwFlags);
  }

  [DllImport("shcore.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern uint GetDpiForMonitor(
    IntPtr hMonitor,
    int dpiType,
    out uint dpiX,
    out uint dpiY);

  public static uint GetDpiForMonitor(IntPtr hMonitor, out uint dpiX, out uint dpiY)
  {
    return NativeMethods.GetDpiForMonitor(hMonitor, 0, out dpiX, out dpiY);
  }

  [DllImport("user32.dll", SetLastError = true)]
  public static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int X,
    int Y,
    int cx,
    int cy,
    NativeMethods.SetWindowPosFlags uFlags);

  [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr ShellExecute(
    IntPtr hwnd,
    string lpOperation,
    string lpFile,
    string lpParameters,
    string lpDirectory,
    int nShowCmd);

  [DllImport("shell32.dll", EntryPoint = "ExtractIconW")]
  public static extern IntPtr ExtractIcon(IntPtr hInst, string pszExeFileName, int nIconIndex);

  [DllImport("shell32", EntryPoint = "ExtractIconExW")]
  public static extern int ExtractIconEx(
    string lpszFile,
    int nIconIndex,
    IntPtr[] phIconLarge,
    IntPtr[] phIconSmall,
    int nIcons);

  [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIconW")]
  public static extern IntPtr ExtractAssociatedIcon(
    IntPtr hInst,
    string lpIconPath,
    ref int lpiIcon);

  [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW")]
  public static extern IntPtr GetModuleHandle(string lpModuleName);

  [DllImport("user32.dll")]
  public static extern bool GetCursorPos(out NativeMethods.POINT lpPoint);

  [DllImport("user32.dll")]
  public static extern bool GetWindowRect(IntPtr hWnd, out NativeMethods.RECT lpRect);

  public enum MonitorOptions : uint
  {
    MONITOR_DEFAULTTONULL,
    MONITOR_DEFAULTTOPRIMARY,
    MONITOR_DEFAULTTONEAREST,
  }

  public struct POINT(int x, int y)
  {
    public int X = x;
    public int Y = y;
  }

  public struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;
  }

  [Flags]
  public enum SetWindowPosFlags : uint
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
