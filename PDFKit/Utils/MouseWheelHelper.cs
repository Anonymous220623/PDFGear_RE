// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.MouseWheelHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.Utils;

internal class MouseWheelHelper : IDisposable
{
  private const int WM_MOUSEWHEEL = 522;
  private const int WM_MOUSEHWHEEL = 526;
  private WeakReference<Visual> visual;
  private HwndSource source;
  private bool throttled;
  private int curHDelta = 0;
  private int curVDelta = 0;
  private DispatcherTimer timer;
  private bool disposedValue;

  public MouseWheelHelper(Visual visual)
  {
    if (visual == null)
      return;
    this.visual = new WeakReference<Visual>(visual);
    this.source = PresentationSource.FromVisual(visual) as HwndSource;
    this.source?.AddHook(new HwndSourceHook(this.Hook));
    this.timer = new DispatcherTimer();
    this.timer.Tick += new EventHandler(this.Timer_Tick);
    this.timer.Interval = TimeSpan.FromSeconds(0.08);
  }

  public bool Throttled
  {
    get => this.throttled;
    set
    {
      if (this.throttled)
        this.ProcessThrottledDelta();
      this.throttled = value;
    }
  }

  private void Timer_Tick(object sender, EventArgs e) => this.ProcessThrottledDelta();

  private void ProcessThrottledDelta()
  {
    this.timer.Stop();
    if (this.curHDelta != 0)
    {
      this.OnMouseTilt(this.curHDelta);
      this.curHDelta = 0;
    }
    if (this.curVDelta == 0)
      return;
    this.OnMouseWheel(this.curVDelta);
    this.curVDelta = 0;
  }

  private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
  {
    Visual target;
    if (!this.visual.TryGetTarget(out target))
    {
      this.timer.Stop();
      this.curHDelta = 0;
      this.curVDelta = 0;
      this.source?.RemoveHook(new HwndSourceHook(this.Hook));
      return IntPtr.Zero;
    }
    switch (msg)
    {
      case 522:
        if (target is FrameworkElement frameworkElement1 && frameworkElement1.IsMouseOver)
        {
          int num = (int) (short) MouseWheelHelper.HIWORD(wParam);
          if (this.Throttled)
            this.curVDelta += num;
          else
            this.curVDelta = num;
          if (this.curVDelta != 0 && (Math.Abs(this.curVDelta) >= 120 || !this.Throttled))
          {
            this.timer.Stop();
            this.OnMouseWheel(this.curVDelta);
            this.curVDelta = 0;
          }
          if (this.curVDelta != 0 && this.Throttled)
            this.timer.Start();
          handled = true;
          return (IntPtr) 1;
        }
        break;
      case 526:
        if (target is FrameworkElement frameworkElement2 && frameworkElement2.IsMouseOver)
        {
          int num = (int) (short) MouseWheelHelper.HIWORD(wParam);
          if (this.Throttled)
            this.curHDelta += num;
          else
            this.curHDelta = num;
          if (this.curHDelta != 0 && (Math.Abs(this.curHDelta) >= 120 || !this.Throttled))
          {
            this.timer.Stop();
            this.OnMouseTilt(this.curHDelta);
            this.curHDelta = 0;
          }
          if (this.curHDelta != 0 && this.Throttled)
            this.timer.Start();
          handled = true;
          return (IntPtr) 1;
        }
        break;
    }
    return IntPtr.Zero;
  }

  private static int HIWORD(IntPtr ptr)
  {
    return (int) ptr.ToInt64() >> 16 /*0x10*/ & (int) ushort.MaxValue;
  }

  private static int LOWORD(IntPtr ptr) => (int) ptr.ToInt64() & (int) ushort.MaxValue;

  private void OnMouseTilt(int delta)
  {
    Visual target;
    if (!this.visual.TryGetTarget(out target))
      return;
    MouseTiltEventArgs mouseTiltEventArgs = new MouseTiltEventArgs(Mouse.PrimaryDevice, 0, delta);
    mouseTiltEventArgs.RoutedEvent = UIElement.PreviewMouseWheelEvent;
    mouseTiltEventArgs.Source = (object) this.visual;
    EventHandler<MouseTiltEventArgs> mouseTilt = this.MouseTilt;
    if (mouseTilt != null)
      mouseTilt((object) this, mouseTiltEventArgs);
    if (mouseTiltEventArgs.Handled || !(target is UIElement))
      return;
    InputManager.Current.ProcessInput((InputEventArgs) mouseTiltEventArgs);
  }

  private void OnMouseWheel(int delta)
  {
    Visual target;
    if (!this.visual.TryGetTarget(out target))
      return;
    MouseWheelEventArgs mouseWheelEventArgs = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, delta);
    mouseWheelEventArgs.RoutedEvent = UIElement.PreviewMouseWheelEvent;
    mouseWheelEventArgs.Source = (object) this.visual;
    EventHandler<MouseWheelEventArgs> mouseWheel = this.MouseWheel;
    if (mouseWheel != null)
      mouseWheel((object) this, mouseWheelEventArgs);
    if (mouseWheelEventArgs.Handled || !(target is UIElement))
      return;
    InputManager.Current.ProcessInput((InputEventArgs) mouseWheelEventArgs);
  }

  public event EventHandler<MouseTiltEventArgs> MouseTilt;

  public event EventHandler<MouseWheelEventArgs> MouseWheel;

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      this.timer.Stop();
      this.timer.Tick += new EventHandler(this.Timer_Tick);
      this.visual = (WeakReference<Visual>) null;
      this.source?.RemoveHook(new HwndSourceHook(this.Hook));
      this.source = (HwndSource) null;
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
