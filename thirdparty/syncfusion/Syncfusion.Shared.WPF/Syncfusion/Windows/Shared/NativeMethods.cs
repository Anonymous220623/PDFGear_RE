// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.NativeMethods
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class NativeMethods
{
  internal const int WA_INACTIVE = 0;
  public const int SM_CXSCREEN = 0;
  public const int SM_CYSCREEN = 1;

  [SecurityCritical]
  [DllImport("user32")]
  public static extern int SetWindowPos(
    IntPtr hwnd,
    int hwndInsertAfter,
    int x,
    int y,
    int cx,
    int cy,
    int wFlags);

  public static int IntPtrToInt32(IntPtr intPtr) => (int) intPtr.ToInt64();

  public static int LoWord(int n) => n & (int) ushort.MaxValue;

  public static int LoWord(IntPtr n) => NativeMethods.LoWord((int) n);

  public static void ScreenToClient(IntPtr hWnd, [In, Out] POINT pt)
  {
    if (NativeMethods.IntScreenToClient(hWnd, pt) == 0)
      throw new Win32Exception();
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool ClipCursor(ref RECT rcClip);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool ClipCursor(object passNull);

  [SuppressUnmanagedCodeSecurity]
  [SecurityCritical]
  [DllImport("user32.dll")]
  public static extern bool MoveWindow(
    IntPtr hWnd,
    int X,
    int Y,
    int nWidth,
    int nHeight,
    bool bRepaint);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

  [SecurityCritical]
  [SecuritySafeCritical]
  public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
  {
    IntPtr num1 = IntPtr.Zero;
    int lastWin32Error;
    if (IntPtr.Size == 4)
    {
      int num2 = NativeMethods.IntSetWindowLong(hWnd, nIndex, NativeMethods.IntPtrToInt32(dwNewLong));
      lastWin32Error = Marshal.GetLastWin32Error();
      num1 = new IntPtr(num2);
    }
    else
    {
      num1 = NativeMethods.IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
      lastWin32Error = Marshal.GetLastWin32Error();
    }
    return num1;
  }

  public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
  {
    return 8 == IntPtr.Size ? NativeMethods.SetWindowLongPtr64(hwnd, nIndex, dwNewLong) : NativeMethods.SetWindowLongPtr32(hwnd, nIndex, dwNewLong);
  }

  [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
  private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

  [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
  private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

  [DllImport("dwmapi.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool DwmDefWindowProc(
    IntPtr hwnd,
    WM msg,
    IntPtr wParam,
    IntPtr lParam,
    out IntPtr plResult);

  [DllImport("user32.dll", EntryPoint = "EnableMenuItem")]
  private static extern int St_EnableMenuItem(
    IntPtr hMenu,
    SystemCommands uIDEnableItem,
    SystemMenuItemBehavior uEnable);

  [CLSCompliant(false)]
  public static SystemMenuItemBehavior EnableMenuItem(
    IntPtr hMenu,
    SystemCommands uIDEnableItem,
    SystemMenuItemBehavior uEnable)
  {
    return (SystemMenuItemBehavior) NativeMethods.St_EnableMenuItem(hMenu, uIDEnableItem, uEnable);
  }

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

  public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
  {
    WINDOWPLACEMENT lpwndpl = new WINDOWPLACEMENT();
    return NativeMethods.GetWindowPlacement(hwnd, lpwndpl) ? lpwndpl : throw new Win32Exception(Marshal.GetLastWin32Error());
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

  [CLSCompliant(false)]
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern int TrackPopupMenu(
    IntPtr hMenu,
    uint uFlags,
    int x,
    int y,
    int nReserved,
    IntPtr hWnd,
    IntPtr prcRect);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

  [DllImport("shell32.dll", CharSet = CharSet.Auto)]
  public static extern bool Shell_NotifyIcon(int dwMessage, ref NativeMethods.NotifyIconData pnid);

  [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
  public static extern int SHAppBarMessage(int dwMessage, ref NativeMethods.APPBARDATA pData);

  [DllImport("user32.dll", SetLastError = true)]
  public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll")]
  public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

  [DllImport("user32.dll")]
  public static extern IntPtr GetDesktopWindow();

  [DllImport("user32.dll")]
  public static extern IntPtr FindWindowEx(
    IntPtr parentHandle,
    IntPtr childAfter,
    string className,
    IntPtr windowTitle);

  [DllImport("user32.dll")]
  public static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("user32.dll")]
  public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

  [DllImport("gdi32.dll")]
  public static extern int GetPixel(IntPtr hdc, int x, int y);

  [DllImport("user32.dll")]
  public static extern bool GetCursorPos(ref Point lpPoint);

  [DllImport("gdi32.dll")]
  public static extern bool StretchBlt(
    IntPtr hdcDest,
    int nXOriginDest,
    int nYOriginDest,
    int nWidthDest,
    int nHeightDest,
    IntPtr hdcSrc,
    int nXOriginSrc,
    int nYOriginSrc,
    int nWidthSrc,
    int nHeightSrc,
    NativeMethods.TernaryRasterOperations dwRop);

  [DllImport("user32.dll")]
  public static extern int GetSystemMetrics(int abc);

  [DllImport("user32.dll")]
  public static extern IntPtr GetWindowDC(int ptr);

  [DllImport("gdi32.dll")]
  public static extern IntPtr DeleteDC(IntPtr hDc);

  [DllImport("gdi32.dll")]
  public static extern IntPtr DeleteObject(IntPtr hDc);

  [DllImport("gdi32.dll")]
  public static extern bool BitBlt(
    IntPtr hdcDest,
    int xDest,
    int yDest,
    int wDest,
    int hDest,
    IntPtr hdcSource,
    int xSrc,
    int ySrc,
    int RasterOp);

  [DllImport("gdi32.dll")]
  public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

  [DllImport("gdi32.dll")]
  public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

  [DllImport("gdi32.dll")]
  public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

  [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
  private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

  [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
  private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, GWL nIndex);

  [DllImport("gdi32.dll")]
  public static extern IntPtr CreateDC(
    string lpszDriver,
    string lpszDevice,
    string lpszOutput,
    IntPtr lpInitData);

  [DllImport("gdi32.dll")]
  public static extern uint GetFontData(
    IntPtr hdc,
    uint dwTable,
    uint dwOffset,
    [Out] byte[] lpvBuffer,
    uint cbData);

  [DllImport("gdi32.dll")]
  public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

  public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
  {
    IntPtr zero = IntPtr.Zero;
    IntPtr num = 8 != IntPtr.Size ? NativeMethods.GetWindowLongPtr32(hwnd, nIndex) : NativeMethods.GetWindowLongPtr64(hwnd, nIndex);
    return !(IntPtr.Zero == num) ? num : throw new Win32Exception(Marshal.GetLastWin32Error());
  }

  [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IntPtr IntSetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern int IntSetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", EntryPoint = "ScreenToClient", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern int IntScreenToClient(IntPtr hWnd, [In, Out] POINT pt);

  public struct APPBARDATA
  {
    public int cbSize;
    public IntPtr hWnd;
    public int uCallbackMessage;
    public int uEdge;
    public RECT rc;
    public bool lParam;
  }

  public enum ABEdge
  {
    ABE_LEFT,
    ABE_TOP,
    ABE_RIGHT,
    ABE_BOTTOM,
  }

  public struct NotifyIconData
  {
    public int cbSize;
    public IntPtr hWnd;
    public int uID;
    public int uFlags;
    public int uCallbackMessage;
    public IntPtr hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
    public string szTip;
    public int dwState;
    public int dwStateMask;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 /*0x0100*/)]
    public string szInfo;
    public int uTimeoutOrVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64 /*0x40*/)]
    public string szInfoTitle;
    public int dwInfoFlags;
  }

  public enum TernaryRasterOperations
  {
    BLACKNESS = 66, // 0x00000042
    NOTSRCERASE = 1114278, // 0x001100A6
    NOTSRCCOPY = 3342344, // 0x00330008
    SRCERASE = 4457256, // 0x00440328
    DSTINVERT = 5570569, // 0x00550009
    PATINVERT = 5898313, // 0x005A0049
    SRCINVERT = 6684742, // 0x00660046
    SRCAND = 8913094, // 0x008800C6
    MERGEPAINT = 12255782, // 0x00BB0226
    MERGECOPY = 12583114, // 0x00C000CA
    SRCCOPY = 13369376, // 0x00CC0020
    SRCPAINT = 15597702, // 0x00EE0086
    PATCOPY = 15728673, // 0x00F00021
    PATPAINT = 16452105, // 0x00FB0A09
    WHITENESS = 16711778, // 0x00FF0062
  }
}
