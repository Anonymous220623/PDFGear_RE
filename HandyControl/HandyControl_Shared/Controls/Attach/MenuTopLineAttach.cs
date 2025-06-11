// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.MenuTopLineAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class MenuTopLineAttach
{
  public static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached("Popup", typeof (Popup), typeof (MenuTopLineAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(MenuTopLineAttach.OnPopupChanged)));
  internal static readonly DependencyProperty TopLineProperty = DependencyProperty.RegisterAttached("TopLine", typeof (FrameworkElement), typeof (MenuTopLineAttach), new PropertyMetadata((object) null));

  private static void OnPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    FrameworkElement frameworkElement = (FrameworkElement) d;
    if (!(e.NewValue is Popup newValue) || !(newValue.TemplatedParent is MenuItem templatedParent))
      return;
    MenuTopLineAttach.SetTopLine((DependencyObject) templatedParent, frameworkElement);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    templatedParent.Loaded += MenuTopLineAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Loaded ?? (MenuTopLineAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Loaded = new RoutedEventHandler(MenuTopLineAttach.MenuItem_Loaded));
  }

  private static void MenuItem_Loaded(object sender, RoutedEventArgs e)
  {
    FrameworkElement element = (FrameworkElement) sender;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.Unloaded += MenuTopLineAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Unloaded ?? (MenuTopLineAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Unloaded = new RoutedEventHandler(MenuTopLineAttach.MenuItem_Unloaded));
    Popup popup = MenuTopLineAttach.GetPopup((DependencyObject) MenuTopLineAttach.GetTopLine((DependencyObject) element));
    if (popup == null)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    popup.Opened += MenuTopLineAttach.\u003C\u003EO.\u003C2\u003E__Popup_Opened ?? (MenuTopLineAttach.\u003C\u003EO.\u003C2\u003E__Popup_Opened = new EventHandler(MenuTopLineAttach.Popup_Opened));
  }

  private static void MenuItem_Unloaded(object sender, RoutedEventArgs e)
  {
    FrameworkElement element = (FrameworkElement) sender;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.Unloaded -= MenuTopLineAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Unloaded ?? (MenuTopLineAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Unloaded = new RoutedEventHandler(MenuTopLineAttach.MenuItem_Unloaded));
    Popup popup = MenuTopLineAttach.GetPopup((DependencyObject) MenuTopLineAttach.GetTopLine((DependencyObject) element));
    if (popup == null)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    popup.Opened -= MenuTopLineAttach.\u003C\u003EO.\u003C2\u003E__Popup_Opened ?? (MenuTopLineAttach.\u003C\u003EO.\u003C2\u003E__Popup_Opened = new EventHandler(MenuTopLineAttach.Popup_Opened));
  }

  private static void Popup_Opened(object sender, EventArgs e)
  {
    if (!(((FrameworkElement) sender).TemplatedParent is MenuItem templatedParent))
      return;
    FrameworkElement topLine = MenuTopLineAttach.GetTopLine((DependencyObject) templatedParent);
    if (topLine == null)
      return;
    topLine.HorizontalAlignment = HorizontalAlignment.Left;
    topLine.Width = templatedParent.ActualWidth;
    topLine.Margin = new Thickness();
    Point screen1 = templatedParent.PointToScreen(new Point());
    Point screen2 = templatedParent.PointToScreen(new Point(templatedParent.ActualWidth, templatedParent.ActualHeight));
    Rect workAreaRect;
    ScreenHelper.FindMonitorRectsFromPoint(screen1, out Rect _, out workAreaRect);
    Panel parent = VisualHelper.GetParent<Panel>((DependencyObject) topLine);
    Thickness thickness1;
    if (screen1.X < 0.0)
    {
      FrameworkElement frameworkElement = topLine;
      double x = screen1.X;
      thickness1 = parent.Margin;
      double left = thickness1.Left;
      Thickness thickness2 = new Thickness(x - left, 0.0, 0.0, 0.0);
      frameworkElement.Margin = thickness2;
    }
    else if (screen1.X + parent.ActualWidth > workAreaRect.Right)
    {
      double num1 = screen2.X - workAreaRect.Right;
      if (num1 > 0.0)
        topLine.Width -= num1 + parent.Margin.Right;
      topLine.HorizontalAlignment = HorizontalAlignment.Left;
      FrameworkElement frameworkElement = topLine;
      double num2 = screen1.X + parent.ActualWidth - workAreaRect.Right;
      thickness1 = parent.Margin;
      double right = thickness1.Right;
      Thickness thickness3 = new Thickness(num2 + right, 0.0, 0.0, 0.0);
      frameworkElement.Margin = thickness3;
    }
    if (screen2.Y <= workAreaRect.Bottom)
      return;
    topLine.Width = 0.0;
    topLine.HorizontalAlignment = HorizontalAlignment.Stretch;
    FrameworkElement frameworkElement1 = topLine;
    thickness1 = new Thickness();
    Thickness thickness4 = thickness1;
    frameworkElement1.Margin = thickness4;
  }

  public static void SetPopup(DependencyObject element, Popup value)
  {
    element.SetValue(MenuTopLineAttach.PopupProperty, (object) value);
  }

  public static Popup GetPopup(DependencyObject element)
  {
    return (Popup) element.GetValue(MenuTopLineAttach.PopupProperty);
  }

  internal static void SetTopLine(DependencyObject element, FrameworkElement value)
  {
    element.SetValue(MenuTopLineAttach.TopLineProperty, (object) value);
  }

  internal static FrameworkElement GetTopLine(DependencyObject element)
  {
    return (FrameworkElement) element.GetValue(MenuTopLineAttach.TopLineProperty);
  }
}
