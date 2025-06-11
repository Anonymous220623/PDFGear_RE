// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.DoubleClickHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public class DoubleClickHelper
{
  private readonly UIElement anchorElement;
  private long doubleClickDeltaTime;
  private Size doubleClickDeltaSize;
  private long? lastClickTick;
  private Point? lastPos;

  public DoubleClickHelper(UIElement anchorElement)
  {
    this.anchorElement = anchorElement ?? throw new ArgumentNullException(nameof (anchorElement));
    this.doubleClickDeltaTime = (long) DoubleClickHelper.GetDoubleClickTime() * 10000L;
    this.doubleClickDeltaSize = DoubleClickHelper.GetDoubleClickDeltaSize();
  }

  public bool WaitingForSecondClick
  {
    get
    {
      return this.lastClickTick.HasValue && Stopwatch.GetTimestamp() - this.lastClickTick.Value < this.doubleClickDeltaTime;
    }
  }

  public bool ProcessMouseClick(MouseButtonEventArgs e)
  {
    if (e == null)
      return false;
    if (e.Handled)
    {
      this.lastClickTick = new long?();
      this.lastPos = new Point?();
      return false;
    }
    Point physicalPoint = DoubleClickHelper.GetPhysicalPoint(this.anchorElement, e);
    if (!this.WaitingForSecondClick || !this.lastPos.HasValue)
    {
      this.lastClickTick = new long?(Stopwatch.GetTimestamp());
      this.lastPos = new Point?(physicalPoint);
      return false;
    }
    if (Stopwatch.GetTimestamp() - this.lastClickTick.Value < this.doubleClickDeltaTime && Math.Abs(this.lastPos.Value.X - physicalPoint.X) <= this.doubleClickDeltaSize.Width && Math.Abs(this.lastPos.Value.Y - physicalPoint.Y) <= this.doubleClickDeltaSize.Height)
    {
      MouseButtonEventHandler mouseDoubleClick = this.MouseDoubleClick;
      if (mouseDoubleClick != null)
        mouseDoubleClick((object) this, e);
    }
    this.lastClickTick = new long?();
    this.lastPos = new Point?();
    int num = e.Handled ? 1 : 0;
    e.Handled = false;
    return num != 0;
  }

  public event MouseButtonEventHandler MouseDoubleClick;

  private static Point GetPhysicalPoint(UIElement anchorElement, MouseButtonEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) anchorElement);
    DpiScale dpi = VisualTreeHelper.GetDpi((Visual) anchorElement);
    return new Point(position.X * dpi.PixelsPerDip, position.Y * dpi.PixelsPerDip);
  }

  private static int GetDoubleClickTime()
  {
    int doubleClickTime = DoubleClickHelper.GetDoubleClickTimeNative();
    if (doubleClickTime < 1)
      doubleClickTime = 100;
    return doubleClickTime;
  }

  private static Size GetDoubleClickDeltaSize()
  {
    return new Size((double) Math.Max(1, DoubleClickHelper.GetSystemMetricsNative(36) / 2), (double) Math.Max(1, DoubleClickHelper.GetSystemMetricsNative(37) / 2));
  }

  [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime", CharSet = CharSet.Auto)]
  private static extern int GetDoubleClickTimeNative();

  [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
  private static extern int GetSystemMetricsNative(int nIndex);
}
