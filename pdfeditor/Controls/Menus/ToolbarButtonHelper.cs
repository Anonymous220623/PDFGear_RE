// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarButtonHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus;

internal static class ToolbarButtonHelper
{
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached("Header", typeof (object), typeof (ToolbarButtonHelper), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolbarButtonHelper.OnHeaderPropertyChanged)));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached("Orientation", typeof (Orientation), typeof (ToolbarButtonHelper), new PropertyMetadata((object) Orientation.Horizontal, new PropertyChangedCallback(ToolbarButtonHelper.OnOrientationPropertyChanged)));
  public static readonly DependencyProperty IsDropDownIconVisibleProperty = DependencyProperty.RegisterAttached("IsDropDownIconVisible", typeof (bool), typeof (ToolbarButtonHelper), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.RegisterAttached("IndicatorBrush", typeof (Brush), typeof (ToolbarButtonHelper), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.RegisterAttached("HeaderTemplate", typeof (DataTemplate), typeof (ToolbarButtonHelper), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolbarButtonHelper.OnHeaderTemplatePropertyChanged)));
  public static readonly DependencyProperty IsMouseOverInternalProperty = DependencyProperty.RegisterAttached("IsMouseOverInternal", typeof (bool), typeof (ToolbarButtonHelper), new PropertyMetadata((object) false));
  public static readonly DependencyProperty OverrideMouseOverProperty = DependencyProperty.RegisterAttached("OverrideMouseOver", typeof (bool), typeof (ToolbarButtonHelper), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsKeyboardFocusedInternalProperty = DependencyProperty.RegisterAttached("IsKeyboardFocusedInternal", typeof (bool), typeof (ToolbarButtonHelper), new PropertyMetadata((object) false));

  public static bool IsContentMouseOver(ButtonBase button, MouseEventArgs e)
  {
    if (button != null && button.IsHitTestVisible)
    {
      flag = true;
      if (!(button.Content is FrameworkElement relativeTo))
      {
        relativeTo = (FrameworkElement) null;
        if ((VisualTreeHelper.GetChildrenCount((DependencyObject) button) > 0 ? VisualTreeHelper.GetChild((DependencyObject) button, 0) as FrameworkElement : (FrameworkElement) null)?.FindName("contentPresenter") is ContentPresenter name)
          relativeTo = VisualTreeHelper.GetChildrenCount((DependencyObject) name) > 0 ? VisualTreeHelper.GetChild((DependencyObject) name, 0) as FrameworkElement : (FrameworkElement) null;
      }
      if (relativeTo is TextBlock)
      {
        if (!(relativeTo.ReadLocalValue(ToolbarButtonHelper.OverrideMouseOverProperty) is bool flag))
          flag = false;
      }
      else if (relativeTo != null)
        flag = ToolbarButtonHelper.GetOverrideMouseOver((DependencyObject) relativeTo);
      if (((relativeTo == null ? 0 : (relativeTo.IsHitTestVisible ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        Point position = e.GetPosition((IInputElement) relativeTo);
        return new Rect(0.0, 0.0, relativeTo.ActualWidth, relativeTo.ActualHeight).Contains(position);
      }
    }
    return false;
  }

  public static void UpdateContentStates(ButtonBase button)
  {
    switch (button)
    {
      case ToolbarButton _:
      case ToolbarToggleButton _:
      case ToolbarRadioButton _:
        if (button.ContentTemplate == null && button.Content is string)
        {
          VisualStateManager.GoToState((FrameworkElement) button, "ContentIsText", true);
          break;
        }
        VisualStateManager.GoToState((FrameworkElement) button, "ContentIsElement", true);
        break;
    }
  }

  public static void UpdateHeaderStates(ButtonBase button)
  {
    switch (button)
    {
      case ToolbarButton _:
      case ToolbarToggleButton _:
      case ToolbarRadioButton _:
        bool flag1 = false;
        bool flag2 = false;
        object obj = button.GetValue(ToolbarButton.HeaderProperty);
        if (!(button.GetValue(ToolbarButton.OrientationProperty) is Orientation orientation))
          orientation = Orientation.Vertical;
        object content = button.Content;
        if (obj != null)
          flag1 = true;
        if (content is string str && !string.IsNullOrEmpty(str))
          flag2 = true;
        else if (content != null)
          flag2 = true;
        if (flag1 & flag2)
        {
          VisualStateManager.GoToState((FrameworkElement) button, orientation.ToString(), true);
          break;
        }
        if (flag1)
        {
          VisualStateManager.GoToState((FrameworkElement) button, "NoContent", true);
          break;
        }
        VisualStateManager.GoToState((FrameworkElement) button, "NoIcon", true);
        break;
    }
  }

  public static void UpdateDropDownIconState(ButtonBase button)
  {
    switch (button)
    {
      case ToolbarChildButton _:
      case ToolbarChildToggleButton _:
        if (!(button.GetValue(ToolbarButtonHelper.IsDropDownIconVisibleProperty) is bool flag))
          flag = false;
        VisualStateManager.GoToState((FrameworkElement) button, flag ? "DropDownIconVisible" : "DropDownIconNotVisible", true);
        break;
    }
  }

  private static ContentPresenter GetButtonHeaderElement(ButtonBase button)
  {
    return button != null && VisualTreeHelper.GetChildrenCount((DependencyObject) button) > 0 && VisualTreeHelper.GetChild((DependencyObject) button, 0) is FrameworkElement child ? child.FindName("HeaderPresenter") as ContentPresenter : (ContentPresenter) null;
  }

  private static void OnHeaderPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is ButtonBase button))
      return;
    ToolbarButtonHelper.UpdateHeaderStates(button);
  }

  private static void OnOrientationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is ButtonBase button))
      return;
    ToolbarButtonHelper.UpdateHeaderStates(button);
  }

  public static Brush GetIndicatorBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(ToolbarButtonHelper.IndicatorBrushProperty);
  }

  public static void SetIndicatorBrush(DependencyObject obj, Brush value)
  {
    obj.SetValue(ToolbarButtonHelper.IndicatorBrushProperty, (object) value);
  }

  public static DataTemplate GetHeaderTemplate(DependencyObject obj)
  {
    return (DataTemplate) obj.GetValue(ToolbarButtonHelper.HeaderTemplateProperty);
  }

  public static void SetHeaderTemplate(DependencyObject obj, DataTemplate value)
  {
    obj.SetValue(ToolbarButtonHelper.HeaderTemplateProperty, (object) value);
  }

  private static void OnHeaderTemplatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is ButtonBase button))
      return;
    ToolbarButtonHelper.UpdateHeaderStates(button);
  }

  public static bool GetIsMouseOverInternal(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolbarButtonHelper.IsMouseOverInternalProperty);
  }

  public static void SetIsMouseOverInternal(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolbarButtonHelper.IsMouseOverInternalProperty, (object) value);
  }

  public static bool GetOverrideMouseOver(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolbarButtonHelper.OverrideMouseOverProperty);
  }

  public static void SetOverrideMouseOver(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolbarButtonHelper.OverrideMouseOverProperty, (object) value);
  }

  public static bool GetIsKeyboardFocusedInternal(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolbarButtonHelper.IsKeyboardFocusedInternalProperty);
  }

  public static void SetIsKeyboardFocusedInternal(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolbarButtonHelper.IsKeyboardFocusedInternalProperty, (object) value);
  }

  public static void RegisterIsKeyboardFocused(ButtonBase button)
  {
    if (button == null)
      throw new ArgumentNullException(nameof (button));
    ToolbarButtonHelper.SetIsKeyboardFocusedInternal((DependencyObject) button, false);
    button.PreviewMouseUp += (MouseButtonEventHandler) ((s, a) => ToolbarButtonHelper.SetIsKeyboardFocusedInternal((DependencyObject) button, false));
    button.GotFocus += (RoutedEventHandler) ((s, a) =>
    {
      if (((UIElement) s).IsMouseOver && (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed || Mouse.MiddleButton == MouseButtonState.Pressed || Mouse.XButton1 == MouseButtonState.Pressed || Mouse.XButton2 == MouseButtonState.Pressed))
        ToolbarButtonHelper.SetIsKeyboardFocusedInternal((DependencyObject) button, false);
      else
        ToolbarButtonHelper.SetIsKeyboardFocusedInternal((DependencyObject) button, true);
      ((FrameworkElement) s).BringIntoView();
    });
    button.LostFocus += (RoutedEventHandler) ((s, a) => ToolbarButtonHelper.SetIsKeyboardFocusedInternal((DependencyObject) button, false));
  }
}
