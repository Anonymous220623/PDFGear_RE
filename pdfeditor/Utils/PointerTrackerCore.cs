// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PointerTrackerCore
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Utils;

public class PointerTrackerCore
{
  private int lastPointerId = -2;
  private Point lastPoint;
  private MouseButton lastClickButton;
  private PointerTrackerCore.PointerState state;
  private ConcurrentDictionary<int, PointerTrackerCore.PointerContext> pointerContexts;

  public PointerTrackerCore()
  {
    this.pointerContexts = new ConcurrentDictionary<int, PointerTrackerCore.PointerContext>();
  }

  public void ProcessDownEvent(Point point, MouseButton button)
  {
    this.ProcessDownEventCore(-1, point, button);
  }

  public void ProcessDownEvent(int pointerId, Point point)
  {
    this.ProcessDownEventCore(pointerId, point, MouseButton.Left);
  }

  public void ProcessUpEvent(Point point, MouseButton button)
  {
    this.ProcessUpEventCore(-1, point, button);
  }

  public void ProcessUpEvent(int pointerId, Point point)
  {
    this.ProcessUpEventCore(pointerId, point, MouseButton.Left);
  }

  public void ProcessMoveEvent(Point point) => this.ProcessMoveEventCore(-1, point);

  public void ProcessMoveEvent(int pointerId, Point point)
  {
    this.ProcessMoveEventCore(pointerId, point);
  }

  public void ProcessMouseWheelEvent(double delta, bool isShiftKeyDown, bool isControlKeyDown)
  {
    this.ProcessMouseWheelEventCore(0.0, delta, isShiftKeyDown, isControlKeyDown);
  }

  public void ProcessMouseWheelEvent(
    double deltaX,
    double deltaY,
    bool isShiftKeyDown,
    bool isControlKeyDown)
  {
    this.ProcessMouseWheelEventCore(deltaX, deltaY, isShiftKeyDown, isControlKeyDown);
  }

  private void ProcessDownEventCore(int pointerId, Point point, MouseButton button)
  {
    bool flag = pointerId == -1;
    lock (this.pointerContexts)
    {
      if (!flag)
        this.TryRemoveTouchPointer();
      PointerTrackerCore.PointerContext pointerContext;
      if (!this.pointerContexts.TryGetValue(pointerId, out pointerContext))
      {
        pointerContext = new PointerTrackerCore.PointerContext();
        this.pointerContexts[pointerId] = pointerContext;
      }
      this.lastPointerId = pointerId;
      this.lastPoint = point;
      this.lastClickButton = button;
      pointerContext.Position = point;
      pointerContext[button].State = PointerTrackerCore.PointerState.Pressed;
    }
  }

  private void ProcessUpEventCore(int pointerId, Point point, MouseButton button)
  {
    bool flag = pointerId == -1;
    lock (this.pointerContexts)
    {
      if (!flag)
        this.TryRemoveTouchPointer();
      PointerTrackerCore.PointerContext pointerContext1;
      if (!this.pointerContexts.TryGetValue(pointerId, out pointerContext1))
      {
        pointerContext1 = new PointerTrackerCore.PointerContext();
        this.pointerContexts[pointerId] = pointerContext1;
      }
      pointerContext1.Position = point;
      pointerContext1[button].State = PointerTrackerCore.PointerState.Normal;
      if (this.lastPointerId != pointerId || this.lastClickButton != button || !new Rect(this.lastPoint.X - 20.0, this.lastPoint.Y - 20.0, 40.0, 40.0).Contains(point))
        return;
      DateTime utcNow = DateTime.UtcNow;
      if (button == MouseButton.Left)
      {
        DateTime? nullable1 = pointerContext1.LastClickTime;
        if (!nullable1.HasValue)
        {
          pointerContext1.LastClickTime = new DateTime?(utcNow);
        }
        else
        {
          int doubleClickTime = PointerTrackerCore.GetDoubleClickTime();
          int millisecond1 = utcNow.Millisecond;
          nullable1 = pointerContext1.LastClickTime;
          int millisecond2 = nullable1.Value.Millisecond;
          if (millisecond1 - millisecond2 >= doubleClickTime)
            return;
          PointerTrackerCore.PointerContext pointerContext2 = pointerContext1;
          nullable1 = new DateTime?();
          DateTime? nullable2 = nullable1;
          pointerContext2.LastClickTime = nullable2;
          this.lastPointerId = -2;
          this.lastPoint = new Point();
        }
      }
      else
      {
        pointerContext1.LastClickTime = new DateTime?();
        this.lastPointerId = -2;
        this.lastPoint = new Point();
      }
    }
  }

  private void ProcessMoveEventCore(int pointerId, Point point)
  {
    bool flag = pointerId == -1;
    lock (this.pointerContexts)
    {
      if (!flag)
        this.TryRemoveTouchPointer();
      PointerTrackerCore.PointerContext pointerContext;
      if (!this.pointerContexts.TryGetValue(pointerId, out pointerContext))
      {
        pointerContext = new PointerTrackerCore.PointerContext();
        this.pointerContexts[pointerId] = pointerContext;
      }
      pointerContext.Position = point;
    }
  }

  public void ProcessMouseWheelEventCore(
    double deltaX,
    double deltaY,
    bool isShiftKeyDown,
    bool isControlKeyDown)
  {
    lock (this.pointerContexts)
      this.TryRemoveTouchPointer();
  }

  private void TryRemoveTouchPointer()
  {
    if (this.pointerContexts.Count == 0 || this.pointerContexts.Count == 1 && this.pointerContexts.Keys.First<int>() == -1)
      return;
    foreach (KeyValuePair<int, PointerTrackerCore.PointerContext> pair in this.pointerContexts.Where<KeyValuePair<int, PointerTrackerCore.PointerContext>>((Func<KeyValuePair<int, PointerTrackerCore.PointerContext>, bool>) (c => c.Key != -1)).ToArray<KeyValuePair<int, PointerTrackerCore.PointerContext>>())
    {
      int key;
      PointerTrackerCore.PointerContext pointerContext;
      pair.Deconstruct<int, PointerTrackerCore.PointerContext>(out key, out pointerContext);
      this.pointerContexts.TryRemove(key, out pointerContext);
    }
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern int GetDoubleClickTime();

  private class PointerContext
  {
    private PointerTrackerCore.ButtonContext leftButtonContext;
    private PointerTrackerCore.ButtonContext middleButtonContext;
    private PointerTrackerCore.ButtonContext rightButtonContext;
    private PointerTrackerCore.ButtonContext xButton1Context;
    private PointerTrackerCore.ButtonContext xButton2Context;

    public PointerContext()
    {
      this.leftButtonContext.MouseButton = MouseButton.Left;
      this.middleButtonContext.MouseButton = MouseButton.Middle;
      this.rightButtonContext.MouseButton = MouseButton.Right;
      this.xButton1Context.MouseButton = MouseButton.XButton1;
      this.xButton2Context.MouseButton = MouseButton.XButton2;
    }

    public ref PointerTrackerCore.ButtonContext this[MouseButton button]
    {
      get
      {
        switch (button)
        {
          case MouseButton.Left:
            return ref this.leftButtonContext;
          case MouseButton.Middle:
            return ref this.middleButtonContext;
          case MouseButton.Right:
            return ref this.rightButtonContext;
          case MouseButton.XButton1:
            return ref this.xButton1Context;
          case MouseButton.XButton2:
            return ref this.xButton2Context;
          default:
            throw new ArgumentException(nameof (button));
        }
      }
    }

    public DateTime? LastClickTime { get; set; }

    public Point Position { get; set; }
  }

  private struct ButtonContext
  {
    public MouseButton MouseButton { get; set; }

    public PointerTrackerCore.PointerState State { get; set; }
  }

  private enum PointerState
  {
    Normal,
    PointerOver,
    Pressed,
  }
}
