// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.InteropMethods
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows;

#nullable disable
namespace HandyControl.Tools.Interop;

internal class InteropMethods
{
  internal const int E_FAIL = -2147467259 /*0x80004005*/;
  internal static readonly IntPtr HRGN_NONE = new IntPtr(-1);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern int RegisterWindowMessage(string msg);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool ReadProcessMemory(
    IntPtr hProcess,
    IntPtr lpBaseAddress,
    out InteropValues.TBBUTTON lpBuffer,
    int dwSize,
    out int lpNumberOfBytesRead);

  [DllImport("kernel32.dll", SetLastError = true)]
  internal static extern bool ReadProcessMemory(
    IntPtr hProcess,
    IntPtr lpBaseAddress,
    out InteropValues.RECT lpBuffer,
    int dwSize,
    out int lpNumberOfBytesRead);

  [DllImport("kernel32.dll", SetLastError = true)]
  internal static extern bool ReadProcessMemory(
    IntPtr hProcess,
    IntPtr lpBaseAddress,
    out InteropValues.TRAYDATA lpBuffer,
    int dwSize,
    out int lpNumberOfBytesRead);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern uint SendMessage(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool AttachThreadInput(
    in uint currentForegroundWindowThreadId,
    in uint thisWindowThreadId,
    bool isAttach);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr GetForegroundWindow();

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr OpenProcess(
    InteropValues.ProcessAccess dwDesiredAccess,
    [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
    uint dwProcessId);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr VirtualAllocEx(
    IntPtr hProcess,
    IntPtr lpAddress,
    int dwSize,
    InteropValues.AllocationType flAllocationType,
    InteropValues.MemoryProtection flProtect);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern int CloseHandle(IntPtr hObject);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool VirtualFreeEx(
    IntPtr hProcess,
    IntPtr lpAddress,
    int dwSize,
    InteropValues.FreeType dwFreeType);

  [DllImport("user32.dll", SetLastError = true)]
  internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll", SetLastError = true)]
  internal static extern IntPtr FindWindowEx(
    IntPtr hwndParent,
    IntPtr hwndChildAfter,
    string lpszClass,
    string lpszWindow);

  [DllImport("user32.dll")]
  internal static extern bool GetWindowRect(IntPtr hwnd, out InteropValues.RECT lpRect);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern bool GetCursorPos(out InteropValues.POINT pt);

  [DllImport("user32.dll")]
  internal static extern IntPtr GetDesktopWindow();

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool AddClipboardFormatListener(IntPtr hwnd);

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

  [DllImport("user32.dll")]
  internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

  [DllImport("user32.dll")]
  internal static extern bool EnableMenuItem(IntPtr hMenu, int UIDEnabledItem, int uEnable);

  [DllImport("user32.dll")]
  internal static extern bool InsertMenu(
    IntPtr hMenu,
    int wPosition,
    int wFlags,
    int wIDNewItem,
    string lpNewItem);

  [DllImport("user32.dll", EntryPoint = "DestroyMenu", CharSet = CharSet.Auto)]
  internal static extern bool IntDestroyMenu(HandleRef hMenu);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", EntryPoint = "GetDC", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr IntGetDC(HandleRef hWnd);

  [SecurityCritical]
  internal static IntPtr GetDC(HandleRef hWnd)
  {
    IntPtr dc = InteropMethods.IntGetDC(hWnd);
    return !(dc == IntPtr.Zero) ? HandleCollector.Add(dc, CommonHandles.HDC) : throw new Win32Exception();
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
  internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

  [SecurityCritical]
  internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
  {
    HandleCollector.Remove((IntPtr) hDC, CommonHandles.HDC);
    return InteropMethods.IntReleaseDC(hWnd, hDC);
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll")]
  internal static extern int GetSystemMetrics(InteropValues.SM nIndex);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", EntryPoint = "DestroyIcon", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool IntDestroyIcon(IntPtr hIcon);

  [SecurityCritical]
  internal static bool DestroyIcon(IntPtr hIcon) => InteropMethods.IntDestroyIcon(hIcon);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool IntDeleteObject(IntPtr hObject);

  [SecurityCritical]
  internal static bool DeleteObject(IntPtr hObject) => InteropMethods.IntDeleteObject(hObject);

  [SecurityCritical]
  internal static BitmapHandle CreateDIBSection(
    HandleRef hdc,
    ref InteropValues.BITMAPINFO bitmapInfo,
    int iUsage,
    ref IntPtr ppvBits,
    SafeFileMappingHandle hSection,
    int dwOffset)
  {
    if (hSection == null)
      hSection = new SafeFileMappingHandle(IntPtr.Zero);
    return InteropMethods.PrivateCreateDIBSection(hdc, ref bitmapInfo, iUsage, ref ppvBits, hSection, dwOffset);
  }

  [DllImport("kernel32.dll", EntryPoint = "CloseHandle", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool IntCloseHandle(HandleRef handle);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern BitmapHandle PrivateCreateDIBSection(
    HandleRef hdc,
    ref InteropValues.BITMAPINFO bitmapInfo,
    int iUsage,
    ref IntPtr ppvBits,
    SafeFileMappingHandle hSection,
    int dwOffset);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", EntryPoint = "CreateIconIndirect", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IconHandle PrivateCreateIconIndirect([MarshalAs(UnmanagedType.LPStruct), In] InteropValues.ICONINFO iconInfo);

  [SecurityCritical]
  internal static IconHandle CreateIconIndirect([MarshalAs(UnmanagedType.LPStruct), In] InteropValues.ICONINFO iconInfo)
  {
    return InteropMethods.PrivateCreateIconIndirect(iconInfo);
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("gdi32.dll", EntryPoint = "CreateBitmap", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern BitmapHandle PrivateCreateBitmap(
    int width,
    int height,
    int planes,
    int bitsPerPixel,
    byte[] lpvBits);

  [SecurityCritical]
  internal static BitmapHandle CreateBitmap(
    int width,
    int height,
    int planes,
    int bitsPerPixel,
    byte[] lpvBits)
  {
    return InteropMethods.PrivateCreateBitmap(width, height, planes, bitsPerPixel, lpvBits);
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("kernel32.dll", EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern int IntGetModuleFileName(
    HandleRef hModule,
    StringBuilder buffer,
    int length);

  [SecurityCritical]
  internal static string GetModuleFileName(HandleRef hModule)
  {
    StringBuilder buffer = new StringBuilder(260);
    while (true)
    {
      int moduleFileName = InteropMethods.IntGetModuleFileName(hModule, buffer, buffer.Capacity);
      if (moduleFileName != 0)
      {
        if (moduleFileName == buffer.Capacity)
          buffer.EnsureCapacity(buffer.Capacity * 2);
        else
          goto label_5;
      }
      else
        break;
    }
    throw new Win32Exception();
label_5:
    return buffer.ToString();
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("shell32.dll", CharSet = CharSet.Auto, ThrowOnUnmappableChar = true, BestFitMapping = false)]
  internal static extern int ExtractIconEx(
    string szExeFileName,
    int nIconIndex,
    out IconHandle phiconLarge,
    out IconHandle phiconSmall,
    int nIcons);

  [DllImport("shell32.dll", CharSet = CharSet.Auto)]
  internal static extern int Shell_NotifyIcon(int message, InteropValues.NOTIFYICONDATA pnid);

  [SecurityCritical]
  [DllImport("user32.dll", EntryPoint = "CreateWindowExW", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr CreateWindowEx(
    int dwExStyle,
    [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
    [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
    int dwStyle,
    int x,
    int y,
    int nWidth,
    int nHeight,
    IntPtr hWndParent,
    IntPtr hMenu,
    IntPtr hInstance,
    IntPtr lpParam);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
  internal static extern short RegisterClass(InteropValues.WNDCLASS4ICON wc);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern bool SetForegroundWindow(IntPtr hWnd);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr CallNextHookEx(
    IntPtr hhk,
    int nCode,
    IntPtr wParam,
    IntPtr lParam);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr GetModuleHandle(string lpModuleName);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern IntPtr SetWindowsHookEx(
    int idHook,
    InteropValues.HookProc lpfn,
    IntPtr hMod,
    uint dwThreadId);

  [DllImport("user32.dll", SetLastError = true)]
  internal static extern IntPtr GetWindowDC(IntPtr window);

  [DllImport("gdi32.dll", SetLastError = true)]
  internal static extern uint GetPixel(IntPtr dc, int x, int y);

  [DllImport("user32.dll", SetLastError = true)]
  internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern IntPtr GetDC(IntPtr ptr);

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetWindowPlacement(IntPtr hwnd, InteropValues.WINDOWPLACEMENT lpwndpl);

  internal static InteropValues.WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
  {
    InteropValues.WINDOWPLACEMENT lpwndpl = InteropValues.WINDOWPLACEMENT.Default;
    return InteropMethods.GetWindowPlacement(hwnd, lpwndpl) ? lpwndpl : throw new Win32Exception(Marshal.GetLastWin32Error());
  }

  internal static int GetXLParam(int lParam) => InteropMethods.LoWord(lParam);

  internal static int GetYLParam(int lParam) => InteropMethods.HiWord(lParam);

  internal static int HiWord(int value) => (int) (short) (value >> 16 /*0x10*/);

  internal static int LoWord(int value) => (int) (short) (value & (int) ushort.MaxValue);

  [DllImport("user32.dll")]
  internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool EnumThreadWindows(
    uint dwThreadId,
    InteropValues.EnumWindowsProc lpfn,
    IntPtr lParam);

  [DllImport("gdi32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool DeleteDC(IntPtr hdc);

  [DllImport("gdi32.dll", SetLastError = true)]
  internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

  [DllImport("gdi32.dll", SetLastError = true)]
  internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

  [DllImport("user32.dll")]
  internal static extern IntPtr MonitorFromPoint(InteropValues.POINT pt, int flags);

  [DllImport("user32.dll")]
  internal static extern IntPtr GetWindow(IntPtr hwnd, int nCmd);

  [DllImport("user32.dll")]
  internal static extern IntPtr GetActiveWindow();

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool IsWindowVisible(IntPtr hwnd);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool IsIconic(IntPtr hwnd);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool IsZoomed(IntPtr hwnd);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int x,
    int y,
    int cx,
    int cy,
    int flags);

  internal static Point GetCursorPos()
  {
    Point cursorPos = new Point();
    InteropValues.POINT pt;
    if (InteropMethods.GetCursorPos(out pt))
    {
      cursorPos.X = (double) pt.X;
      cursorPos.Y = (double) pt.Y;
    }
    return cursorPos;
  }

  [DllImport("user32.dll")]
  private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

  internal static int GetWindowLong(IntPtr hWnd, InteropValues.GWL nIndex)
  {
    return InteropMethods.GetWindowLong(hWnd, (int) nIndex);
  }

  internal static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
  {
    return IntPtr.Size != 4 ? InteropMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : InteropMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
  }

  [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
  internal static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
  internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

  [DllImport("user32.dll", CharSet = CharSet.Unicode)]
  private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32.dll", CharSet = CharSet.Unicode)]
  private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

  internal static IntPtr SetWindowLongPtr(IntPtr hWnd, InteropValues.GWLP nIndex, IntPtr dwNewLong)
  {
    return IntPtr.Size == 8 ? InteropMethods.SetWindowLongPtr(hWnd, (int) nIndex, dwNewLong) : new IntPtr(InteropMethods.SetWindowLong(hWnd, (int) nIndex, dwNewLong.ToInt32()));
  }

  internal static int SetWindowLong(IntPtr hWnd, InteropValues.GWL nIndex, int dwNewLong)
  {
    return InteropMethods.SetWindowLong(hWnd, (int) nIndex, dwNewLong);
  }

  [DllImport("user32.dll", CharSet = CharSet.Unicode)]
  internal static extern ushort RegisterClass(ref InteropValues.WNDCLASS lpWndClass);

  [DllImport("kernel32.dll")]
  internal static extern uint GetCurrentThreadId();

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr CreateWindowEx(
    int dwExStyle,
    IntPtr classAtom,
    string lpWindowName,
    int dwStyle,
    int x,
    int y,
    int nWidth,
    int nHeight,
    IntPtr hWndParent,
    IntPtr hMenu,
    IntPtr hInstance,
    IntPtr lpParam);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool DestroyWindow(IntPtr hwnd);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool UnregisterClass(IntPtr classAtom, IntPtr hInstance);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool UpdateLayeredWindow(
    IntPtr hwnd,
    IntPtr hdcDest,
    ref InteropValues.POINT pptDest,
    ref InteropValues.SIZE psize,
    IntPtr hdcSrc,
    ref InteropValues.POINT pptSrc,
    uint crKey,
    [In] ref InteropValues.BLENDFUNCTION pblend,
    uint dwFlags);

  [DllImport("user32.dll")]
  internal static extern bool RedrawWindow(
    IntPtr hWnd,
    IntPtr lprcUpdate,
    IntPtr hrgnUpdate,
    InteropValues.RedrawWindowFlags flags);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool EnumDisplayMonitors(
    IntPtr hdc,
    IntPtr lprcClip,
    InteropValues.EnumMonitorsDelegate lpfnEnum,
    IntPtr dwData);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool IntersectRect(
    out InteropValues.RECT lprcDst,
    [In] ref InteropValues.RECT lprcSrc1,
    [In] ref InteropValues.RECT lprcSrc2);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool GetMonitorInfo(
    IntPtr hMonitor,
    ref InteropValues.MONITORINFO monitorInfo);

  [DllImport("user32.dll")]
  public static extern IntPtr MonitorFromRect(ref InteropValues.RECT rect, int flags);

  [DllImport("gdi32.dll", SetLastError = true)]
  internal static extern IntPtr CreateDIBSection(
    IntPtr hdc,
    ref InteropValues.BITMAPINFO pbmi,
    uint iUsage,
    out IntPtr ppvBits,
    IntPtr hSection,
    uint dwOffset);

  [DllImport("msimg32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool AlphaBlend(
    IntPtr hdcDest,
    int xoriginDest,
    int yoriginDest,
    int wDest,
    int hDest,
    IntPtr hdcSrc,
    int xoriginSrc,
    int yoriginSrc,
    int wSrc,
    int hSrc,
    InteropValues.BLENDFUNCTION pfn);

  internal static int GET_SC_WPARAM(IntPtr wParam) => (int) wParam & 65520;

  [DllImport("user32.dll")]
  internal static extern IntPtr ChildWindowFromPointEx(
    IntPtr hwndParent,
    InteropValues.POINT pt,
    int uFlags);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

  [DllImport("gdi32.dll")]
  internal static extern bool BitBlt(
    IntPtr hDC,
    int x,
    int y,
    int nWidth,
    int nHeight,
    IntPtr hSrcDC,
    int xSrc,
    int ySrc,
    int dwRop);

  [DllImport("user32.dll")]
  internal static extern bool EnableWindow(IntPtr hWnd, bool enable);

  [DllImport("user32.dll")]
  public static extern bool ShowWindow(IntPtr hwnd, InteropValues.SW nCmdShow);

  [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
  [SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
  internal static object PtrToStructure(IntPtr lparam, Type cls)
  {
    return Marshal.PtrToStructure(lparam, cls);
  }

  [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
  [SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
  internal static void PtrToStructure(IntPtr lparam, object data)
  {
    Marshal.PtrToStructure(lparam, data);
  }

  [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
  internal static extern uint SHAppBarMessage(int dwMessage, ref InteropValues.APPBARDATA pData);

  [SecurityCritical]
  [DllImport("dwmapi.dll")]
  internal static extern int DwmGetColorizationColor(
    out uint pcrColorization,
    out bool pfOpaqueBlend);

  [DllImport("dwmapi.dll", SetLastError = true)]
  internal static extern int DwmSetWindowAttribute(
    IntPtr hwnd,
    InteropValues.DwmWindowAttribute dwAttribute,
    in int pvAttribute,
    uint cbAttribute);

  [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
  private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
  private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

  internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
  {
    return IntPtr.Size != 8 ? InteropMethods.GetWindowLongPtr32(hWnd, nIndex) : InteropMethods.GetWindowLongPtr64(hWnd, nIndex);
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern bool SetWindowPlacement(
    IntPtr hWnd,
    [In] ref InteropValues.WINDOWPLACEMENT placement);

  internal class Gdip
  {
    private const string ThreadDataSlotName = "system.drawing.threaddata";
    private static IntPtr InitToken;
    internal const int Ok = 0;
    internal const int GenericError = 1;
    internal const int InvalidParameter = 2;
    internal const int OutOfMemory = 3;
    internal const int ObjectBusy = 4;
    internal const int InsufficientBuffer = 5;
    internal const int NotImplemented = 6;
    internal const int Win32Error = 7;
    internal const int WrongState = 8;
    internal const int Aborted = 9;
    internal const int FileNotFound = 10;
    internal const int ValueOverflow = 11;
    internal const int AccessDenied = 12;
    internal const int UnknownImageFormat = 13;
    internal const int FontFamilyNotFound = 14;
    internal const int FontStyleNotFound = 15;
    internal const int NotTrueTypeFont = 16 /*0x10*/;
    internal const int UnsupportedGdiplusVersion = 17;
    internal const int GdiplusNotInitialized = 18;
    internal const int PropertyNotFound = 19;
    internal const int PropertyNotSupported = 20;
    internal const int E_UNEXPECTED = -2147418113;

    private static bool Initialized => InteropMethods.Gdip.InitToken != IntPtr.Zero;

    static Gdip() => InteropMethods.Gdip.Initialize();

    private static void Initialize()
    {
      InteropMethods.Gdip.StartupInput input = InteropMethods.Gdip.StartupInput.GetDefault();
      int status = InteropMethods.Gdip.GdiplusStartup(out InteropMethods.Gdip.InitToken, ref input, out InteropMethods.Gdip.StartupOutput _);
      if (status != 0)
        throw InteropMethods.Gdip.StatusException(status);
      AppDomain currentDomain = AppDomain.CurrentDomain;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.ProcessExit += InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit ?? (InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit = new EventHandler(InteropMethods.Gdip.OnProcessExit));
      if (currentDomain.IsDefaultAppDomain())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.DomainUnload += InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit ?? (InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit = new EventHandler(InteropMethods.Gdip.OnProcessExit));
    }

    [PrePrepareMethod]
    private static void OnProcessExit(object sender, EventArgs e) => InteropMethods.Gdip.Shutdown();

    private static void Shutdown()
    {
      if (!InteropMethods.Gdip.Initialized)
        return;
      InteropMethods.Gdip.ClearThreadData();
      AppDomain currentDomain = AppDomain.CurrentDomain;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.ProcessExit -= InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit ?? (InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit = new EventHandler(InteropMethods.Gdip.OnProcessExit));
      if (currentDomain.IsDefaultAppDomain())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.DomainUnload -= InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit ?? (InteropMethods.Gdip.\u003C\u003EO.\u003C0\u003E__OnProcessExit = new EventHandler(InteropMethods.Gdip.OnProcessExit));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ClearThreadData()
    {
      Thread.SetData(Thread.GetNamedDataSlot("system.drawing.threaddata"), (object) null);
    }

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);

    internal static Exception StatusException(int status)
    {
      Exception exception;
      switch (status)
      {
        case 1:
          exception = (Exception) new ExternalException("GdiplusGenericError");
          break;
        case 2:
          exception = (Exception) new ArgumentException("GdiplusInvalidParameter");
          break;
        case 3:
          exception = (Exception) new OutOfMemoryException("GdiplusOutOfMemory");
          break;
        case 4:
          exception = (Exception) new InvalidOperationException("GdiplusObjectBusy");
          break;
        case 5:
          exception = (Exception) new OutOfMemoryException("GdiplusInsufficientBuffer");
          break;
        case 6:
          exception = (Exception) new NotImplementedException("GdiplusNotImplemented");
          break;
        case 7:
          exception = (Exception) new ExternalException("GdiplusGenericError");
          break;
        case 8:
          exception = (Exception) new InvalidOperationException("GdiplusWrongState");
          break;
        case 9:
          exception = (Exception) new ExternalException("GdiplusAborted");
          break;
        case 10:
          exception = (Exception) new FileNotFoundException("GdiplusFileNotFound");
          break;
        case 11:
          exception = (Exception) new OverflowException("GdiplusOverflow");
          break;
        case 12:
          exception = (Exception) new ExternalException("GdiplusAccessDenied");
          break;
        case 13:
          exception = (Exception) new ArgumentException("GdiplusUnknownImageFormat");
          break;
        case 14:
          exception = (Exception) new ArgumentException("GdiplusFontFamilyNotFound");
          break;
        case 15:
          exception = (Exception) new ArgumentException("GdiplusFontStyleNotFound");
          break;
        case 16 /*0x10*/:
          exception = (Exception) new ArgumentException("GdiplusNotTrueTypeFont_NoName");
          break;
        case 17:
          exception = (Exception) new ExternalException("GdiplusUnsupportedGdiplusVersion");
          break;
        case 18:
          exception = (Exception) new ExternalException("GdiplusNotInitialized");
          break;
        case 19:
          exception = (Exception) new ArgumentException("GdiplusPropertyNotFoundError");
          break;
        case 20:
          exception = (Exception) new ArgumentException("GdiplusPropertyNotSupportedError");
          break;
        default:
          exception = (Exception) new ExternalException("GdiplusUnknown");
          break;
      }
      return exception;
    }

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipImageGetFrameDimensionsList(
      HandleRef image,
      IntPtr buffer,
      int count);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipImageGetFrameCount(
      HandleRef image,
      ref Guid dimensionId,
      int[] count);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetPropertyItem(
      HandleRef image,
      int propid,
      int size,
      IntPtr buffer);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipCreateHBITMAPFromBitmap(
      HandleRef nativeBitmap,
      out IntPtr hbitmap,
      int argbBackground);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipImageSelectActiveFrame(
      HandleRef image,
      ref Guid dimensionId,
      int frameIndex);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipImageForceValidation(HandleRef image);

    [DllImport("gdiplus.dll", EntryPoint = "GdipDisposeImage", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int IntGdipDisposeImage(HandleRef image);

    internal static int GdipDisposeImage(HandleRef image)
    {
      return !InteropMethods.Gdip.Initialized ? 0 : InteropMethods.Gdip.IntGdipDisposeImage(image);
    }

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GdiplusStartup(
      out IntPtr token,
      ref InteropMethods.Gdip.StartupInput input,
      out InteropMethods.Gdip.StartupOutput output);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(
      IntPtr hwnd,
      ref InteropValues.WINCOMPATTRDATA data);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipCreateBitmapFromStream(
      InteropValues.IStream stream,
      out IntPtr bitmap);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipCreateBitmapFromHBITMAP(
      HandleRef hbitmap,
      HandleRef hpalette,
      out IntPtr bitmap);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

    [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GdipSaveImageToStream(
      HandleRef image,
      InteropValues.IStream stream,
      ref Guid classId,
      HandleRef encoderParams);

    [DllImport("ntdll.dll")]
    internal static extern int RtlGetVersion(
      out InteropValues.RTL_OSVERSIONINFOEX lpVersionInformation);

    private struct StartupInput
    {
      private int GdiplusVersion;
      private readonly IntPtr DebugEventCallback;
      private bool SuppressBackgroundThread;
      private bool SuppressExternalCodecs;

      public static InteropMethods.Gdip.StartupInput GetDefault()
      {
        return new InteropMethods.Gdip.StartupInput()
        {
          GdiplusVersion = 1,
          SuppressBackgroundThread = false,
          SuppressExternalCodecs = false
        };
      }
    }

    private readonly struct StartupOutput
    {
      private readonly IntPtr hook;
      private readonly IntPtr unhook;
    }
  }
}
