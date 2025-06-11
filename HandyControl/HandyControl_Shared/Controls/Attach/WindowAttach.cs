// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.WindowAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Interop;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

#nullable disable
namespace HandyControl.Controls;

public static class WindowAttach
{
  public static readonly DependencyProperty IsDragElementProperty = DependencyProperty.RegisterAttached("IsDragElement", typeof (bool), typeof (WindowAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(WindowAttach.OnIsDragElementChanged)));
  public static readonly DependencyProperty IgnoreAltF4Property = DependencyProperty.RegisterAttached("IgnoreAltF4", typeof (bool), typeof (WindowAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(WindowAttach.OnIgnoreAltF4Changed)));
  public static readonly DependencyProperty ShowInTaskManagerProperty = DependencyProperty.RegisterAttached("ShowInTaskManager", typeof (bool), typeof (WindowAttach), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(WindowAttach.OnShowInTaskManagerChanged)));
  public static readonly DependencyProperty HideWhenClosingProperty = DependencyProperty.RegisterAttached("HideWhenClosing", typeof (bool), typeof (WindowAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(WindowAttach.OnHideWhenClosingChanged)));
  public static readonly DependencyProperty ExtendContentToNonClientAreaProperty = DependencyProperty.RegisterAttached("ExtendContentToNonClientArea", typeof (bool), typeof (WindowAttach), new PropertyMetadata(ValueBoxes.FalseBox));

  public static void SetIsDragElement(DependencyObject element, bool value)
  {
    element.SetValue(WindowAttach.IsDragElementProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsDragElement(DependencyObject element)
  {
    return (bool) element.GetValue(WindowAttach.IsDragElementProperty);
  }

  private static void OnIsDragElementChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is UIElement uiElement))
      return;
    if ((bool) e.NewValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uiElement.MouseLeftButtonDown += WindowAttach.\u003C\u003EO.\u003C0\u003E__DragElement_MouseLeftButtonDown ?? (WindowAttach.\u003C\u003EO.\u003C0\u003E__DragElement_MouseLeftButtonDown = new MouseButtonEventHandler(WindowAttach.DragElement_MouseLeftButtonDown));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uiElement.MouseLeftButtonDown -= WindowAttach.\u003C\u003EO.\u003C0\u003E__DragElement_MouseLeftButtonDown ?? (WindowAttach.\u003C\u003EO.\u003C0\u003E__DragElement_MouseLeftButtonDown = new MouseButtonEventHandler(WindowAttach.DragElement_MouseLeftButtonDown));
    }
  }

  private static void DragElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is DependencyObject dependencyObject) || e.ButtonState != MouseButtonState.Pressed)
      return;
    System.Windows.Window.GetWindow(dependencyObject)?.DragMove();
  }

  private static void OnIgnoreAltF4Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is System.Windows.Window window))
      return;
    if ((bool) e.NewValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.PreviewKeyDown += WindowAttach.\u003C\u003EO.\u003C1\u003E__Window_PreviewKeyDown ?? (WindowAttach.\u003C\u003EO.\u003C1\u003E__Window_PreviewKeyDown = new KeyEventHandler(WindowAttach.Window_PreviewKeyDown));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.PreviewKeyDown -= WindowAttach.\u003C\u003EO.\u003C1\u003E__Window_PreviewKeyDown ?? (WindowAttach.\u003C\u003EO.\u003C1\u003E__Window_PreviewKeyDown = new KeyEventHandler(WindowAttach.Window_PreviewKeyDown));
    }
  }

  private static void Window_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.System || e.SystemKey != Key.F4)
      return;
    e.Handled = true;
  }

  public static void SetIgnoreAltF4(DependencyObject element, bool value)
  {
    element.SetValue(WindowAttach.IgnoreAltF4Property, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIgnoreAltF4(DependencyObject element)
  {
    return (bool) element.GetValue(WindowAttach.IgnoreAltF4Property);
  }

  private static void OnShowInTaskManagerChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is System.Windows.Window window))
      return;
    bool newValue = (bool) e.NewValue;
    window.SetCurrentValue(System.Windows.Window.ShowInTaskbarProperty, (object) newValue);
    if (newValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.SourceInitialized -= WindowAttach.\u003C\u003EO.\u003C2\u003E__Window_SourceInitialized ?? (WindowAttach.\u003C\u003EO.\u003C2\u003E__Window_SourceInitialized = new EventHandler(WindowAttach.Window_SourceInitialized));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.SourceInitialized += WindowAttach.\u003C\u003EO.\u003C2\u003E__Window_SourceInitialized ?? (WindowAttach.\u003C\u003EO.\u003C2\u003E__Window_SourceInitialized = new EventHandler(WindowAttach.Window_SourceInitialized));
    }
  }

  private static void Window_SourceInitialized(object sender, EventArgs e)
  {
    if (!(sender is System.Windows.Window window))
      return;
    new WindowInteropHelper(window).Owner = InteropMethods.GetDesktopWindow();
  }

  public static void SetShowInTaskManager(DependencyObject element, bool value)
  {
    element.SetValue(WindowAttach.ShowInTaskManagerProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowInTaskManager(DependencyObject element)
  {
    return (bool) element.GetValue(WindowAttach.ShowInTaskManagerProperty);
  }

  private static void OnHideWhenClosingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is System.Windows.Window window))
      return;
    if ((bool) e.NewValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.Closing += WindowAttach.\u003C\u003EO.\u003C3\u003E__Window_Closing ?? (WindowAttach.\u003C\u003EO.\u003C3\u003E__Window_Closing = new CancelEventHandler(WindowAttach.Window_Closing));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      window.Closing -= WindowAttach.\u003C\u003EO.\u003C3\u003E__Window_Closing ?? (WindowAttach.\u003C\u003EO.\u003C3\u003E__Window_Closing = new CancelEventHandler(WindowAttach.Window_Closing));
    }
  }

  private static void Window_Closing(object sender, CancelEventArgs e)
  {
    if (!(sender is System.Windows.Window window))
      return;
    window.Hide();
    e.Cancel = true;
  }

  public static void SetHideWhenClosing(DependencyObject element, bool value)
  {
    element.SetValue(WindowAttach.HideWhenClosingProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetHideWhenClosing(DependencyObject element)
  {
    return (bool) element.GetValue(WindowAttach.HideWhenClosingProperty);
  }

  public static void SetExtendContentToNonClientArea(DependencyObject element, bool value)
  {
    element.SetValue(WindowAttach.ExtendContentToNonClientAreaProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetExtendContentToNonClientArea(DependencyObject element)
  {
    return (bool) element.GetValue(WindowAttach.ExtendContentToNonClientAreaProperty);
  }
}
