// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.InputClickHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Tools;

public static class InputClickHelper
{
  private static readonly DependencyProperty InputInfoProperty = DependencyProperty.RegisterAttached("InputInfo", typeof (InputClickHelper.InputInfo), typeof (InputClickHelper), new PropertyMetadata((object) null));

  private static void SetInputInfo(DependencyObject element, InputClickHelper.InputInfo value)
  {
    element.SetValue(InputClickHelper.InputInfoProperty, (object) value);
  }

  private static InputClickHelper.InputInfo GetInputInfo(DependencyObject element)
  {
    return (InputClickHelper.InputInfo) element.GetValue(InputClickHelper.InputInfoProperty);
  }

  public static void AttachMouseDownMoveUpToClick(
    UIElement element,
    EventHandler clickEventHandler,
    EventHandler dragStarted = null)
  {
    InputClickHelper.InputInfo inputInfo = InputClickHelper.GetOrCreateInputInfo(element);
    inputInfo.ClickEventHandler += clickEventHandler;
    inputInfo.DragStarted += dragStarted;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseDown -= InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown ?? (InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown = new MouseButtonEventHandler(InputClickHelper.Element_MouseDown));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseDown += InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown ?? (InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown = new MouseButtonEventHandler(InputClickHelper.Element_MouseDown));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseMove -= InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove ?? (InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove = new MouseEventHandler(InputClickHelper.Element_MouseMove));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseMove += InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove ?? (InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove = new MouseEventHandler(InputClickHelper.Element_MouseMove));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseUp -= InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp ?? (InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp = new MouseButtonEventHandler(InputClickHelper.Element_MouseUp));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseUp += InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp ?? (InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp = new MouseButtonEventHandler(InputClickHelper.Element_MouseUp));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.LostMouseCapture -= InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture ?? (InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture = new MouseEventHandler(InputClickHelper.Element_LostMouseCapture));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.LostMouseCapture += InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture ?? (InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture = new MouseEventHandler(InputClickHelper.Element_LostMouseCapture));
  }

  public static void DetachMouseDownMoveUpToClick(
    UIElement element,
    EventHandler clickEventHandler,
    EventHandler dragStarted = null)
  {
    InputClickHelper.InputInfo inputInfo = InputClickHelper.GetInputInfo((DependencyObject) element);
    if (inputInfo == null)
      return;
    inputInfo.ClickEventHandler -= clickEventHandler;
    inputInfo.DragStarted -= dragStarted;
    if (!inputInfo.IsEmpty())
      return;
    element.ClearValue(InputClickHelper.InputInfoProperty);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseDown -= InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown ?? (InputClickHelper.\u003C\u003EO.\u003C0\u003E__Element_MouseDown = new MouseButtonEventHandler(InputClickHelper.Element_MouseDown));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseMove -= InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove ?? (InputClickHelper.\u003C\u003EO.\u003C1\u003E__Element_MouseMove = new MouseEventHandler(InputClickHelper.Element_MouseMove));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.MouseUp -= InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp ?? (InputClickHelper.\u003C\u003EO.\u003C2\u003E__Element_MouseUp = new MouseButtonEventHandler(InputClickHelper.Element_MouseUp));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.LostMouseCapture -= InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture ?? (InputClickHelper.\u003C\u003EO.\u003C3\u003E__Element_LostMouseCapture = new MouseEventHandler(InputClickHelper.Element_LostMouseCapture));
  }

  private static void Element_LostMouseCapture(object sender, MouseEventArgs e)
  {
    InputClickHelper.GetInputInfo((DependencyObject) sender)?.LostCapture();
  }

  private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
  {
    UIElement uiElement = (UIElement) sender;
    InputClickHelper.GetInputInfo((DependencyObject) uiElement)?.Up(e.GetPosition((IInputElement) uiElement));
  }

  private static void Element_MouseMove(object sender, MouseEventArgs e)
  {
    UIElement uiElement = (UIElement) sender;
    InputClickHelper.GetInputInfo((DependencyObject) uiElement)?.Move(e.GetPosition((IInputElement) uiElement));
  }

  private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
  {
    UIElement uiElement = (UIElement) sender;
    InputClickHelper.GetInputInfo((DependencyObject) uiElement)?.Down(e.GetPosition((IInputElement) uiElement));
  }

  private static InputClickHelper.InputInfo GetOrCreateInputInfo(UIElement element)
  {
    InputClickHelper.InputInfo inputInfo = InputClickHelper.GetInputInfo((DependencyObject) element);
    if (inputInfo == null)
    {
      inputInfo = new InputClickHelper.InputInfo();
      InputClickHelper.SetInputInfo((DependencyObject) element, inputInfo);
    }
    return inputInfo;
  }

  private class InputInfo
  {
    private const double ToleranceSquared = 0.01;
    private Point _downedPosition;
    private bool _isClick;

    public event EventHandler ClickEventHandler;

    public event EventHandler DragStarted;

    public void Down(Point position)
    {
      this._downedPosition = position;
      this._isClick = true;
    }

    public void Move(Point position)
    {
      if (!this._isClick || (position - this._downedPosition).LengthSquared <= 0.01)
        return;
      this._isClick = false;
      EventHandler dragStarted = this.DragStarted;
      if (dragStarted == null)
        return;
      dragStarted((object) null, EventArgs.Empty);
    }

    public void Up(Point position)
    {
      this._isClick = this._isClick && (position - this._downedPosition).LengthSquared <= 0.01;
      if (!this._isClick)
        return;
      EventHandler clickEventHandler = this.ClickEventHandler;
      if (clickEventHandler != null)
        clickEventHandler((object) null, EventArgs.Empty);
      this._isClick = false;
    }

    public void LostCapture() => this._isClick = false;

    public bool IsEmpty() => this.ClickEventHandler == null && this.DragStarted == null;
  }
}
