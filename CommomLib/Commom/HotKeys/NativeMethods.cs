// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.NativeMethods
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal static class NativeMethods
{
  private const string User32 = "user32.dll";
  private const string Comctl32 = "comctl32.dll";
  private const string Kernel32 = "kernel32.dll";
  internal const int GWL_EXSTYLE = -20;
  internal const int WS_EX_TRANSPARENT = 32 /*0x20*/;

  internal static long GetWindowLong(IntPtr hWnd, int nIndex)
  {
    return IntPtr.Size == 8 ? NativeMethods._GetWindowLongPtr(hWnd, nIndex) : (long) NativeMethods._GetWindowLong(hWnd, nIndex);
  }

  internal static long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong)
  {
    return IntPtr.Size == 8 ? NativeMethods._SetWindowLongPtr(hWnd, nIndex, dwNewLong) : (long) NativeMethods._SetWindowLong(hWnd, nIndex, (int) dwNewLong);
  }

  [DllImport("comctl32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool SetWindowSubclass(
    IntPtr hWnd,
    NativeMethods.SUBCLASSPROC pfnSubclass,
    uint uIdSubclass,
    IntPtr dwRefData);

  [DllImport("comctl32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool RemoveWindowSubclass(
    IntPtr hWnd,
    NativeMethods.SUBCLASSPROC pfnSubclass,
    uint uIdSubclass);

  [DllImport("comctl32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr DefSubclassProc(
    IntPtr hWnd,
    uint uMsg,
    UIntPtr wParam,
    IntPtr lParam);

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern uint GetCurrentThreadId();

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool IsWindow(IntPtr hWnd);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr SetWindowsHookEx(
    NativeMethods.HookType code,
    NativeMethods.HookProc func,
    IntPtr hInstance,
    uint threadID);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr CallNextHookEx(
    IntPtr hhk,
    int nCode,
    IntPtr wParam,
    IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr SetWindowsHookExW(
    int idHook,
    NativeMethods.KeyboardProc lpfn,
    IntPtr hmod,
    uint dwThreadId);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr CallNextHookEx(
    IntPtr hhk,
    int nCode,
    UIntPtr wParam,
    IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool GetMessage(
    ref NativeMethods.MSG lpMsg,
    IntPtr hWnd,
    uint wMsgFilterMin,
    uint wMsgFilterMax);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool TranslateMessage(in NativeMethods.MSG lpMsg);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern IntPtr DispatchMessage(in NativeMethods.MSG lpMsg);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool PostThreadMessage(
    uint idThread,
    uint Msg,
    UIntPtr wParam,
    IntPtr lParam);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern void PostQuitMessage(int nExitCode);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern bool IsWindowEnabled(IntPtr hWnd);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern uint MapVirtualKey(uint uCode, NativeMethods.MAPVK uMapType);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern unsafe int GetKeyNameText(IntPtr lParam, char* lpString, int cchSize);

  [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern short GetKeyState(int virtualKey);

  [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern int _GetWindowLong(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern long _GetWindowLongPtr(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern int _SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern long _SetWindowLongPtr(IntPtr hWnd, int nIndex, long dwNewLong);

  internal delegate IntPtr KeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam);

  internal delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

  internal delegate IntPtr SUBCLASSPROC(
    IntPtr hWnd,
    uint uMsg,
    UIntPtr wParam,
    IntPtr lParam,
    uint uIdSubclass,
    IntPtr dwRefData);

  internal enum MAPVK : uint
  {
    MAPVK_VK_TO_VSC,
    MAPVK_VSC_TO_VK,
    MAPVK_VK_TO_CHAR,
    MAPVK_VSC_TO_VK_EX,
    MAPVK_VK_TO_VSC_EX,
  }

  internal struct MSG
  {
    public IntPtr hwnd;
    public uint message;
    public UIntPtr wParam;
    public IntPtr lParam;
    public UIntPtr time;
    public NativeMethods.POINT pt;
    public uint lPrivate;
  }

  internal struct POINT
  {
    public int x;
    public int y;
  }

  internal enum HookType
  {
    WH_JOURNALRECORD,
    WH_JOURNALPLAYBACK,
    WH_KEYBOARD,
    WH_GETMESSAGE,
    WH_CALLWNDPROC,
    WH_CBT,
    WH_SYSMSGFILTER,
    WH_MOUSE,
    WH_HARDWARE,
    WH_DEBUG,
    WH_SHELL,
    WH_FOREGROUNDIDLE,
    WH_CALLWNDPROCRET,
    WH_KEYBOARD_LL,
    WH_MOUSE_LL,
  }

  internal enum HCBT
  {
    HCBT_MOVESIZE,
    HCBT_MINMAX,
    HCBT_QS,
    HCBT_CREATEWND,
    HCBT_DESTROYWND,
    HCBT_ACTIVATE,
    HCBT_CLICKSKIPPED,
    HCBT_KEYSKIPPED,
    HCBT_SYSCOMMAND,
    HCBT_SETFOCUS,
  }
}
