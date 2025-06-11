// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HookKeyEventListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal class HookKeyEventListener : IKeyboardEventListener
{
  private object locker = new object();
  private HookKeyEventListener.KeyboardHook hook;
  private KeyboardEventListener.KeyDownEventHandler onKeyDownEvent;

  public event KeyboardEventListener.KeyDownEventHandler KeyDown
  {
    add
    {
      lock (this.locker)
      {
        this.VerifyAccess();
        this.onKeyDownEvent += value;
        if (this.onKeyDownEvent == null)
          return;
        this.SetHook();
      }
    }
    remove
    {
      lock (this.locker)
      {
        this.VerifyAccess();
        this.onKeyDownEvent -= value;
        if (this.onKeyDownEvent != null)
          return;
        this.UnsetHook();
      }
    }
  }

  private void Hook_KeyDown(object sender, KeyboardEventListener.KeyDownEventArgs e)
  {
    KeyboardEventListener.KeyDownEventHandler onKeyDownEvent = this.onKeyDownEvent;
    if (onKeyDownEvent == null)
      return;
    onKeyDownEvent((object) this, e);
  }

  private void VerifyAccess()
  {
    (Application.Current?.Dispatcher ?? throw new InvalidOperationException("Dispatcher")).VerifyAccess();
  }

  private void SetHook()
  {
    lock (this.locker)
    {
      if (this.hook != null)
        return;
      this.hook = new HookKeyEventListener.KeyboardHook();
      this.hook.KeyDown += new KeyboardEventListener.KeyDownEventHandler(this.Hook_KeyDown);
    }
  }

  private void UnsetHook()
  {
    lock (this.locker)
    {
      if (this.hook == null)
        return;
      this.hook.KeyDown -= new KeyboardEventListener.KeyDownEventHandler(this.Hook_KeyDown);
      this.hook.Dispose();
      this.hook = (HookKeyEventListener.KeyboardHook) null;
    }
  }

  private class KeyboardHook : IDisposable
  {
    private const uint WM_USER = 1024 /*0x0400*/;
    private const uint WM_POSTQUIT = 1154;
    private bool disposedValue;
    private IntPtr hHook;
    private Thread thread;
    private NativeMethods.KeyboardProc _keyboardProc;
    private uint targetThreadId;
    private uint messageThreadId;

    internal KeyboardHook()
    {
      this.targetThreadId = NativeMethods.GetCurrentThreadId();
      this._keyboardProc = new NativeMethods.KeyboardProc(this.OnKeyboardProc);
      this.thread = new Thread(new ParameterizedThreadStart(this.ThreadMain));
      this.thread.IsBackground = true;
      this.thread.SetApartmentState(ApartmentState.MTA);
      SemaphoreSlim parameter = new SemaphoreSlim(0, 1);
      this.thread.Start((object) parameter);
      parameter.Wait(1000);
    }

    private void ThreadMain(object state)
    {
      try
      {
        this.hHook = NativeMethods.SetWindowsHookExW(2, this._keyboardProc, IntPtr.Zero, this.targetThreadId);
      }
      finally
      {
        if (state is SemaphoreSlim semaphoreSlim)
          semaphoreSlim.Release();
      }
      if (this.disposedValue || !(this.hHook != IntPtr.Zero))
        return;
      this.messageThreadId = NativeMethods.GetCurrentThreadId();
      NativeMethods.MSG lpMsg = new NativeMethods.MSG();
      while (NativeMethods.GetMessage(ref lpMsg, IntPtr.Zero, 0U, 0U))
      {
        if (lpMsg.message == 1154U)
        {
          NativeMethods.UnhookWindowsHookEx(this.hHook);
          NativeMethods.PostQuitMessage(0);
        }
        NativeMethods.TranslateMessage(in lpMsg);
        NativeMethods.DispatchMessage(in lpMsg);
      }
      NativeMethods.UnhookWindowsHookEx(this.hHook);
    }

    private IntPtr OnKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        IntPtr hHook = this.hHook;
        HookKeyEventListener.KeyboardHook.KeyboardParameter keyboardParameter = new HookKeyEventListener.KeyboardHook.KeyboardParameter(wParam, lParam);
        if (keyboardParameter.IsPressed)
        {
          KeyboardEventListener.KeyDownEventArgs e = new KeyboardEventListener.KeyDownEventArgs(keyboardParameter.VirtualKey, keyboardParameter.Key, keyboardParameter.Count, keyboardParameter.ScanCode, keyboardParameter.ExtendKey, keyboardParameter.PrevKeyPressed);
          try
          {
            KeyboardEventListener.KeyDownEventHandler keyDown = this.KeyDown;
            if (keyDown != null)
              keyDown((object) this, e);
          }
          catch
          {
          }
          if (e.Handled)
            return (IntPtr) 1;
        }
      }
      return NativeMethods.CallNextHookEx(this.hHook, nCode, wParam, lParam);
    }

    public event KeyboardEventListener.KeyDownEventHandler KeyDown;

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (this.messageThreadId != 0U)
        NativeMethods.PostThreadMessage(this.messageThreadId, 1154U, UIntPtr.Zero, IntPtr.Zero);
      this.disposedValue = true;
    }

    ~KeyboardHook() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal struct KeyboardParameter
    {
      private ulong wParam;
      private ulong lParam;

      public KeyboardParameter(UIntPtr wParam, IntPtr lParam)
      {
        this.wParam = wParam.ToUInt64();
        this.lParam = (ulong) lParam.ToInt64();
      }

      public int VirtualKey => (int) this.wParam;

      public Key Key
      {
        get
        {
          Key key = KeyInterop.KeyFromVirtualKey(this.VirtualKey);
          if (this.ExtendKey)
          {
            switch (key)
            {
              case Key.LeftShift:
                return Key.RightShift;
              case Key.LeftCtrl:
                return Key.RightCtrl;
              case Key.LeftAlt:
                return Key.RightAlt;
            }
          }
          return key;
        }
      }

      public ushort Count => (ushort) (this.lParam & (ulong) byte.MaxValue);

      public ushort ScanCode => (ushort) (this.lParam >> 16 /*0x10*/ & (ulong) byte.MaxValue);

      public bool ExtendKey => (this.lParam >> 24 & 1UL) > 0UL;

      public bool PrevKeyPressed => (this.lParam >> 30 & 1UL) > 0UL;

      public bool IsPressed => ((long) (this.lParam >> 31 /*0x1F*/) & 1L) == 0L;

      public override string ToString()
      {
        return $"VirtualKey: 0x{this.VirtualKey:X2}, Key: {this.Key}, Count: {this.Count}, ScanCode: {this.ScanCode}, ExtendKey: {this.ExtendKey}, PrevKeyPressed: {this.PrevKeyPressed}, IsPressed: {this.IsPressed}";
      }
    }
  }
}
