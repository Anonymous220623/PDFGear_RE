// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Helper.TouchDragMoveWindowHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Tools.Helper;

internal class TouchDragMoveWindowHelper
{
  private const int MaxMoveSpeed = 60;
  private readonly Window _window;
  private InteropValues.POINT? _lastPoint;

  public TouchDragMoveWindowHelper(Window window) => this._window = window;

  public void Start()
  {
    Window window = this._window;
    window.PreviewMouseMove += new MouseEventHandler(this.Window_PreviewMouseMove);
    window.PreviewMouseUp += new MouseButtonEventHandler(this.Window_PreviewMouseUp);
    window.LostMouseCapture += new MouseEventHandler(this.Window_LostMouseCapture);
  }

  public void Stop()
  {
    Window window = this._window;
    window.PreviewMouseMove -= new MouseEventHandler(this.Window_PreviewMouseMove);
    window.PreviewMouseUp -= new MouseButtonEventHandler(this.Window_PreviewMouseUp);
    window.LostMouseCapture -= new MouseEventHandler(this.Window_LostMouseCapture);
  }

  private void Window_LostMouseCapture(object sender, MouseEventArgs e) => this.Stop();

  private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e) => this.Stop();

  private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
  {
    InteropValues.POINT pt;
    InteropMethods.GetCursorPos(out pt);
    if (!this._lastPoint.HasValue)
    {
      this._lastPoint = new InteropValues.POINT?(pt);
      this._window.CaptureMouse();
    }
    int num1 = pt.X - this._lastPoint.Value.X;
    int num2 = pt.Y - this._lastPoint.Value.Y;
    if (Math.Abs(num1) < 60 && Math.Abs(num2) < 60)
    {
      IntPtr handle = new WindowInteropHelper(this._window).Handle;
      InteropValues.RECT lpRect;
      InteropMethods.GetWindowRect(handle, out lpRect);
      InteropMethods.SetWindowPos(handle, IntPtr.Zero, lpRect.Left + num1, lpRect.Top + num2, 0, 0, 5);
    }
    this._lastPoint = new InteropValues.POINT?(pt);
  }
}
