// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.KeyboardHook
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

public class KeyboardHook
{
  private static IntPtr HookId = IntPtr.Zero;
  private static readonly InteropValues.HookProc Proc = new InteropValues.HookProc(KeyboardHook.HookCallback);
  private static int VirtualKey;
  private static readonly IntPtr KeyDownIntPtr = (IntPtr) 256 /*0x0100*/;
  private static readonly IntPtr KeyUpIntPtr = (IntPtr) 257;
  private static readonly IntPtr SyskeyDownIntPtr = (IntPtr) 260;
  private static readonly IntPtr SyskeyUpIntPtr = (IntPtr) 261;
  private static int Count;

  public static event EventHandler<KeyboardHookEventArgs> KeyDown;

  public static event EventHandler<KeyboardHookEventArgs> KeyUp;

  public static void Start()
  {
    if (KeyboardHook.HookId == IntPtr.Zero)
      KeyboardHook.HookId = KeyboardHook.SetHook(KeyboardHook.Proc);
    if (!(KeyboardHook.HookId != IntPtr.Zero))
      return;
    ++KeyboardHook.Count;
  }

  public static void Stop()
  {
    --KeyboardHook.Count;
    if (KeyboardHook.Count >= 1)
      return;
    InteropMethods.UnhookWindowsHookEx(KeyboardHook.HookId);
    KeyboardHook.HookId = IntPtr.Zero;
  }

  private static IntPtr SetHook(InteropValues.HookProc proc)
  {
    using (Process currentProcess = Process.GetCurrentProcess())
    {
      using (ProcessModule mainModule = currentProcess.MainModule)
        return mainModule != null ? InteropMethods.SetWindowsHookEx(13, proc, InteropMethods.GetModuleHandle(mainModule.ModuleName), 0U) : IntPtr.Zero;
    }
  }

  private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
  {
    if (nCode >= 0)
    {
      if (wParam == KeyboardHook.KeyDownIntPtr)
      {
        int virtualKey = Marshal.ReadInt32(lParam);
        if (KeyboardHook.VirtualKey != virtualKey)
        {
          KeyboardHook.VirtualKey = virtualKey;
          EventHandler<KeyboardHookEventArgs> keyDown = KeyboardHook.KeyDown;
          if (keyDown != null)
            keyDown((object) null, new KeyboardHookEventArgs(virtualKey, false));
        }
      }
      else if (wParam == KeyboardHook.SyskeyDownIntPtr)
      {
        int virtualKey = Marshal.ReadInt32(lParam);
        if (KeyboardHook.VirtualKey != virtualKey)
        {
          KeyboardHook.VirtualKey = virtualKey;
          EventHandler<KeyboardHookEventArgs> keyDown = KeyboardHook.KeyDown;
          if (keyDown != null)
            keyDown((object) null, new KeyboardHookEventArgs(virtualKey, true));
        }
      }
      else if (wParam == KeyboardHook.KeyUpIntPtr)
      {
        int virtualKey = Marshal.ReadInt32(lParam);
        KeyboardHook.VirtualKey = -1;
        EventHandler<KeyboardHookEventArgs> keyUp = KeyboardHook.KeyUp;
        if (keyUp != null)
          keyUp((object) null, new KeyboardHookEventArgs(virtualKey, false));
      }
      else if (wParam == KeyboardHook.SyskeyUpIntPtr)
      {
        int virtualKey = Marshal.ReadInt32(lParam);
        KeyboardHook.VirtualKey = -1;
        EventHandler<KeyboardHookEventArgs> keyUp = KeyboardHook.KeyUp;
        if (keyUp != null)
          keyUp((object) null, new KeyboardHookEventArgs(virtualKey, true));
      }
    }
    return InteropMethods.CallNextHookEx(KeyboardHook.HookId, nCode, wParam, lParam);
  }
}
