// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.WindowLifetimeListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Commom;

internal class WindowLifetimeListener : IWindowLifetimeListener
{
  private object locker = new object();
  private bool exiting;
  private IntPtr hhook;
  private WindowLifetimeListener.HookProc hookProc;
  private Dictionary<IntPtr, WeakReference<WindowLifetimeListener.WindowPair>> windows;
  private Dispatcher dispatcher;
  private EventHandler<WindowLifetimeListenerEventArgs> _WindowCreated;
  private EventHandler<WindowLifetimeListenerEventArgs> _WindowDestroyed;

  public event EventHandler<WindowLifetimeListenerEventArgs> WindowCreated
  {
    add
    {
      lock (this.locker)
      {
        this._WindowCreated += value;
        this.UpdateHook();
      }
    }
    remove
    {
      lock (this.locker)
      {
        this._WindowCreated -= value;
        this.UpdateHook();
      }
    }
  }

  public event EventHandler<WindowLifetimeListenerEventArgs> WindowDestroyed
  {
    add
    {
      lock (this.locker)
      {
        this._WindowDestroyed += value;
        this.UpdateHook();
      }
    }
    remove
    {
      lock (this.locker)
      {
        this._WindowDestroyed -= value;
        this.UpdateHook();
      }
    }
  }

  private void UpdateHook()
  {
    lock (this.locker)
    {
      bool flag = this._WindowCreated != null || this._WindowDestroyed != null;
      if (flag)
      {
        Dispatcher dispatcher = this.dispatcher;
        if (dispatcher != null)
          flag = !dispatcher.HasShutdownStarted && !this.exiting;
      }
      if (flag)
        this.Initialize();
      else
        this.Uninitialize();
    }
  }

  private bool Initialize()
  {
    if (!(this.hhook == IntPtr.Zero))
      return true;
    lock (this.locker)
    {
      if (this.hhook == IntPtr.Zero)
      {
        if (this.dispatcher == null)
        {
          this.dispatcher = Application.Current.Dispatcher;
          this.dispatcher.ShutdownStarted += new EventHandler(this.Dispatcher_ShutdownStarted);
        }
        if (this.dispatcher.CheckAccess() && !this.dispatcher.HasShutdownStarted)
        {
          this.windows = new Dictionary<IntPtr, WeakReference<WindowLifetimeListener.WindowPair>>();
          WindowLifetimeListener.WindowPair[] array = PresentationSource.CurrentSources.OfType<HwndSource>().Where<HwndSource>((Func<HwndSource, bool>) (c => c.RootVisual is Window)).Select<HwndSource, WindowLifetimeListener.WindowPair>((Func<HwndSource, WindowLifetimeListener.WindowPair>) (c => new WindowLifetimeListener.WindowPair(c, (Window) c.RootVisual))).ToArray<WindowLifetimeListener.WindowPair>();
          for (int index = 0; index < array.Length; ++index)
            this.windows[array[index].HwndSource.Handle] = new WeakReference<WindowLifetimeListener.WindowPair>(array[index]);
          int currentThreadId = WindowLifetimeListener.GetCurrentThreadId();
          this.hookProc = new WindowLifetimeListener.HookProc(this.OnHookProc);
          this.hhook = WindowLifetimeListener.SetWindowsHookEx(WindowLifetimeListener.HookType.WH_CBT, this.hookProc, IntPtr.Zero, currentThreadId);
        }
      }
      return this.hhook != IntPtr.Zero;
    }
  }

  private void Uninitialize()
  {
    if (!(this.hhook != IntPtr.Zero))
      return;
    lock (this.locker)
    {
      if (!(this.hhook != IntPtr.Zero))
        return;
      this.windows = (Dictionary<IntPtr, WeakReference<WindowLifetimeListener.WindowPair>>) null;
      WindowLifetimeListener.UnhookWindowsHookEx(this.hhook);
    }
  }

  private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
  {
    this.exiting = true;
    this.UpdateHook();
  }

  private IntPtr OnHookProc(int nCode, IntPtr wParam, IntPtr lParam)
  {
    if (nCode < 0)
      return WindowLifetimeListener.CallNextHookEx(this.hhook, nCode, wParam, lParam);
    switch ((WindowLifetimeListener.HCBT) nCode)
    {
      case WindowLifetimeListener.HCBT.HCBT_CREATEWND:
        IntPtr hwnd = wParam;
        if (!this.dispatcher.HasShutdownStarted)
        {
          this.dispatcher.InvokeAsync((Action) (() =>
          {
            if (!WindowLifetimeListener.IsWindow(hwnd))
              return;
            HwndSource hwndSource = HwndSource.FromHwnd(hwnd);
            if (!(hwndSource?.RootVisual is Window rootVisual2))
              return;
            lock (this.windows)
              this.windows[hwnd] = new WeakReference<WindowLifetimeListener.WindowPair>(new WindowLifetimeListener.WindowPair(hwndSource, rootVisual2));
            EventHandler<WindowLifetimeListenerEventArgs> windowCreated = this._WindowCreated;
            if (windowCreated == null)
              return;
            windowCreated((object) this, new WindowLifetimeListenerEventArgs()
            {
              HwndSource = hwndSource,
              Window = rootVisual2
            });
          }), DispatcherPriority.Normal);
          break;
        }
        break;
      case WindowLifetimeListener.HCBT.HCBT_DESTROYWND:
        lock (this.windows)
        {
          IntPtr key = wParam;
          WeakReference<WindowLifetimeListener.WindowPair> weakReference;
          WindowLifetimeListener.WindowPair windowPair;
          if (this.windows.TryGetValue(key, out weakReference) && weakReference.TryGetTarget(out windowPair) && !this.dispatcher.HasShutdownStarted)
            this.dispatcher.InvokeAsync((Action) (() =>
            {
              EventHandler<WindowLifetimeListenerEventArgs> windowDestroyed = this._WindowDestroyed;
              if (windowDestroyed == null)
                return;
              windowDestroyed((object) this, new WindowLifetimeListenerEventArgs()
              {
                HwndSource = windowPair.HwndSource,
                Window = windowPair.Window
              });
            }), DispatcherPriority.Normal);
          this.windows.Remove(key);
          break;
        }
    }
    return IntPtr.Zero;
  }

  [DllImport("kernel32.dll")]
  private static extern int GetCurrentThreadId();

  [DllImport("user32.dll")]
  private static extern bool IsWindow(IntPtr hWnd);

  [DllImport("user32.dll")]
  private static extern IntPtr SetWindowsHookEx(
    WindowLifetimeListener.HookType code,
    WindowLifetimeListener.HookProc func,
    IntPtr hInstance,
    int threadID);

  [DllImport("user32.dll")]
  private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

  [DllImport("user32.dll")]
  private static extern bool UnhookWindowsHookEx(IntPtr hhk);

  private class WindowPair
  {
    public WindowPair(HwndSource hwndSource, Window window)
    {
      this.HwndSource = hwndSource;
      this.Window = window;
    }

    public HwndSource HwndSource { get; }

    public Window Window { get; }
  }

  private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

  private enum HookType
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

  private enum HCBT
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
