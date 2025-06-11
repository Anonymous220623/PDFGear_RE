// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RangeThumb
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class RangeThumb : Thumb
{
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (RangeThumb), new PropertyMetadata((object) null));

  public object Content
  {
    get => this.GetValue(RangeThumb.ContentProperty);
    set => this.SetValue(RangeThumb.ContentProperty, value);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
  }

  public void StartDrag()
  {
    this.IsDragging = true;
    this.Focus();
    this.CaptureMouse();
    MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left);
    e.RoutedEvent = UIElement.PreviewMouseLeftButtonDownEvent;
    e.Source = (object) this;
    this.RaiseEvent((RoutedEventArgs) e);
  }

  public new void CancelDrag()
  {
    base.CancelDrag();
    MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left);
    e.RoutedEvent = UIElement.PreviewMouseLeftButtonUpEvent;
    this.RaiseEvent((RoutedEventArgs) e);
  }
}
