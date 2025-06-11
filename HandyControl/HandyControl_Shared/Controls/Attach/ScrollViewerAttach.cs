// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ScrollViewerAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class ScrollViewerAttach
{
  public static readonly DependencyProperty AutoHideProperty = DependencyProperty.RegisterAttached("AutoHide", typeof (bool), typeof (ScrollViewerAttach), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached("Orientation", typeof (Orientation), typeof (ScrollViewerAttach), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.VerticalBox, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(ScrollViewerAttach.OnOrientationChanged)));
  public static readonly DependencyProperty IsDisabledProperty = DependencyProperty.RegisterAttached("IsDisabled", typeof (bool), typeof (ScrollViewerAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(ScrollViewerAttach.OnIsDisabledChanged)));

  public static void SetAutoHide(DependencyObject element, bool value)
  {
    element.SetValue(ScrollViewerAttach.AutoHideProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAutoHide(DependencyObject element)
  {
    return (bool) element.GetValue(ScrollViewerAttach.AutoHideProperty);
  }

  private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    switch (d)
    {
      case System.Windows.Controls.ScrollViewer scrollViewer:
        if ((Orientation) e.NewValue == Orientation.Horizontal)
        {
          scrollViewer.PreviewMouseWheel += new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
          break;
        }
        scrollViewer.PreviewMouseWheel -= new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
        break;
    }

    static void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
    {
      System.Windows.Controls.ScrollViewer scrollViewer = (System.Windows.Controls.ScrollViewer) sender;
      scrollViewer.ScrollToHorizontalOffset(Math.Min(Math.Max(0.0, scrollViewer.HorizontalOffset - (double) args.Delta), scrollViewer.ScrollableWidth));
      args.Handled = true;
    }
  }

  public static void SetOrientation(DependencyObject element, Orientation value)
  {
    element.SetValue(ScrollViewerAttach.OrientationProperty, ValueBoxes.OrientationBox(value));
  }

  public static Orientation GetOrientation(DependencyObject element)
  {
    return (Orientation) element.GetValue(ScrollViewerAttach.OrientationProperty);
  }

  private static void OnIsDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is UIElement uiElement))
      return;
    if ((bool) e.NewValue)
      uiElement.PreviewMouseWheel += new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
    else
      uiElement.PreviewMouseWheel -= new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);

    static void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
    {
      if (args.Handled)
        return;
      args.Handled = true;
      System.Windows.Controls.ScrollViewer parent = VisualHelper.GetParent<System.Windows.Controls.ScrollViewer>((DependencyObject) sender);
      if (parent == null)
        return;
      System.Windows.Controls.ScrollViewer scrollViewer = parent;
      MouseWheelEventArgs e = new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta);
      e.RoutedEvent = UIElement.MouseWheelEvent;
      e.Source = sender;
      scrollViewer.RaiseEvent((RoutedEventArgs) e);
    }
  }

  public static void SetIsDisabled(DependencyObject element, bool value)
  {
    element.SetValue(ScrollViewerAttach.IsDisabledProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsDisabled(DependencyObject element)
  {
    return (bool) element.GetValue(ScrollViewerAttach.IsDisabledProperty);
  }
}
