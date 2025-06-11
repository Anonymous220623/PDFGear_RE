// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ClipboardHook
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Tools;

public class ClipboardHook
{
  private static HwndSource HWndSource;
  private static IntPtr HookId = IntPtr.Zero;
  private static int Count;

  public static event Action ContentChanged;

  public static void Start()
  {
    if (ClipboardHook.HookId == IntPtr.Zero)
    {
      ClipboardHook.HookId = WindowHelper.CreateHandle();
      ClipboardHook.HWndSource = HwndSource.FromHwnd(ClipboardHook.HookId);
      if (ClipboardHook.HWndSource != null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ClipboardHook.HWndSource.AddHook(ClipboardHook.\u003C\u003EO.\u003C0\u003E__WinProc ?? (ClipboardHook.\u003C\u003EO.\u003C0\u003E__WinProc = new HwndSourceHook(ClipboardHook.WinProc)));
        InteropMethods.AddClipboardFormatListener(ClipboardHook.HookId);
      }
    }
    if (!(ClipboardHook.HookId != IntPtr.Zero))
      return;
    ++ClipboardHook.Count;
  }

  public static void Stop()
  {
    --ClipboardHook.Count;
    if (ClipboardHook.Count >= 1)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ClipboardHook.HWndSource.RemoveHook(ClipboardHook.\u003C\u003EO.\u003C0\u003E__WinProc ?? (ClipboardHook.\u003C\u003EO.\u003C0\u003E__WinProc = new HwndSourceHook(ClipboardHook.WinProc)));
    InteropMethods.RemoveClipboardFormatListener(ClipboardHook.HookId);
    ClipboardHook.HookId = IntPtr.Zero;
  }

  private static IntPtr WinProc(
    IntPtr hwnd,
    int msg,
    IntPtr wparam,
    IntPtr lparam,
    ref bool handled)
  {
    if (msg == 797)
    {
      Action contentChanged = ClipboardHook.ContentChanged;
      if (contentChanged != null)
        contentChanged();
    }
    return IntPtr.Zero;
  }
}
