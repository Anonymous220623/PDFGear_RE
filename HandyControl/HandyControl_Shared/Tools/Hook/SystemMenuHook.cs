// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.SystemMenuHook
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Tools;

public class SystemMenuHook
{
  private static readonly Dictionary<int, HwndSource> DataDic = new Dictionary<int, HwndSource>();

  public static event Action<int> Click;

  public static void Insert(int index, int id, string text, Window window)
  {
    IntPtr handle = WindowHelper.GetHandle(window);
    HwndSource hwndSource = HwndSource.FromHwnd(handle);
    if (hwndSource == null)
      return;
    SystemMenuHook.DataDic[id] = hwndSource;
    InteropMethods.InsertMenu(InteropMethods.GetSystemMenu(handle, false), index, 1024 /*0x0400*/, id, text);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    hwndSource.AddHook(SystemMenuHook.\u003C\u003EO.\u003C0\u003E__WinProc ?? (SystemMenuHook.\u003C\u003EO.\u003C0\u003E__WinProc = new HwndSourceHook(SystemMenuHook.WinProc)));
  }

  public static void InsertSeperator(int index, Window window)
  {
    InteropMethods.InsertMenu(InteropMethods.GetSystemMenu(WindowHelper.GetHandle(window), false), index, 3072 /*0x0C00*/, 0, "");
  }

  public static void Remove(int id)
  {
    HwndSource hwndSource;
    if (!SystemMenuHook.DataDic.TryGetValue(id, out hwndSource))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    hwndSource.RemoveHook(SystemMenuHook.\u003C\u003EO.\u003C0\u003E__WinProc ?? (SystemMenuHook.\u003C\u003EO.\u003C0\u003E__WinProc = new HwndSourceHook(SystemMenuHook.WinProc)));
    SystemMenuHook.DataDic.Remove(id);
  }

  private static IntPtr WinProc(
    IntPtr hwnd,
    int msg,
    IntPtr wparam,
    IntPtr lparam,
    ref bool handled)
  {
    if (msg == 274)
    {
      int int32 = wparam.ToInt32();
      if (SystemMenuHook.DataDic.ContainsKey(int32))
      {
        Action<int> click = SystemMenuHook.Click;
        if (click != null)
          click(int32);
      }
    }
    return IntPtr.Zero;
  }
}
