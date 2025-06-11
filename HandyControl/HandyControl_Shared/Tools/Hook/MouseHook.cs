// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.MouseHook
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace HandyControl.Tools;

internal class MouseHook
{
  private static IntPtr HookId = IntPtr.Zero;
  private static readonly InteropValues.HookProc Proc = new InteropValues.HookProc(MouseHook.HookCallback);
  private static int Count;

  public static event EventHandler<MouseHookEventArgs> StatusChanged;

  public static void Start()
  {
    if (MouseHook.HookId == IntPtr.Zero)
      MouseHook.HookId = MouseHook.SetHook(MouseHook.Proc);
    if (!(MouseHook.HookId != IntPtr.Zero))
      return;
    ++MouseHook.Count;
  }

  public static void Stop()
  {
    --MouseHook.Count;
    if (MouseHook.Count >= 1)
      return;
    InteropMethods.UnhookWindowsHookEx(MouseHook.HookId);
    MouseHook.HookId = IntPtr.Zero;
  }

  private static IntPtr SetHook(InteropValues.HookProc proc)
  {
    using (Process currentProcess = Process.GetCurrentProcess())
    {
      using (ProcessModule mainModule = currentProcess.MainModule)
        return mainModule != null ? InteropMethods.SetWindowsHookEx(14, proc, InteropMethods.GetModuleHandle(mainModule.ModuleName), 0U) : IntPtr.Zero;
    }
  }

  private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
  {
    if (nCode < 0)
      return InteropMethods.CallNextHookEx(MouseHook.HookId, nCode, wParam, lParam);
    InteropValues.MOUSEHOOKSTRUCT structure = (InteropValues.MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof (InteropValues.MOUSEHOOKSTRUCT));
    EventHandler<MouseHookEventArgs> statusChanged = MouseHook.StatusChanged;
    if (statusChanged != null)
      statusChanged((object) null, new MouseHookEventArgs()
      {
        MessageType = (MouseHookMessageType) (int) wParam,
        Point = new InteropValues.POINT(structure.pt.X, structure.pt.Y)
      });
    return InteropMethods.CallNextHookEx(MouseHook.HookId, nCode, wParam, lParam);
  }
}
