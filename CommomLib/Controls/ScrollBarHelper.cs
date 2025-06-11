// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ScrollBarHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Controls;

public static class ScrollBarHelper
{
  private static ComponentResourceKey modernScrollViewerStyleKey;
  public static readonly DependencyProperty CanExpandProperty = DependencyProperty.RegisterAttached("CanExpand", typeof (bool), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ForceExpandProperty = DependencyProperty.RegisterAttached("ForceExpand", typeof (bool), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ShowScrollBarImmediatelyProperty = DependencyProperty.RegisterAttached("ShowScrollBarImmediately", typeof (bool), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ScrollBarSizeProperty = DependencyProperty.RegisterAttached("ScrollBarSize", typeof (ScrollBarSize), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) ScrollBarSize.Default, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty IndicatorModeProperty = DependencyProperty.RegisterAttached("IndicatorMode", typeof (ScrollingIndicatorMode), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) ScrollingIndicatorMode.None, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty AutoHideProperty = DependencyProperty.RegisterAttached("AutoHide", typeof (bool), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty EnableAutoHideExtensionProperty = DependencyProperty.RegisterAttached("EnableAutoHideExtension", typeof (bool), typeof (ScrollBarHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(ScrollBarHelper.OnEnableAutoHideExtensionPropertyChanged)));
  public static readonly DependencyProperty DisableBoundaryFeedbackProperty = DependencyProperty.RegisterAttached("DisableBoundaryFeedback", typeof (bool), typeof (ScrollBarHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(ScrollBarHelper.OnDisableBoundaryFeedbackPropertyChanged)));

  public static ComponentResourceKey ModernScrollViewerStyleKey
  {
    get
    {
      return ScrollBarHelper.modernScrollViewerStyleKey ?? (ScrollBarHelper.modernScrollViewerStyleKey = new ComponentResourceKey(typeof (ScrollBarHelper), (object) nameof (ModernScrollViewerStyleKey)));
    }
  }

  public static bool GetCanExpand(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.CanExpandProperty);
  }

  public static void SetCanExpand(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.CanExpandProperty, (object) value);
  }

  public static bool GetForceExpand(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.ForceExpandProperty);
  }

  public static void SetForceExpand(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.ForceExpandProperty, (object) value);
  }

  public static bool GetShowScrollBarImmediately(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.ShowScrollBarImmediatelyProperty);
  }

  public static void SetShowScrollBarImmediately(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.ShowScrollBarImmediatelyProperty, (object) value);
  }

  public static ScrollBarSize GetScrollBarSize(DependencyObject obj)
  {
    return (ScrollBarSize) obj.GetValue(ScrollBarHelper.ScrollBarSizeProperty);
  }

  public static void SetScrollBarSize(DependencyObject obj, ScrollBarSize value)
  {
    obj.SetValue(ScrollBarHelper.ScrollBarSizeProperty, (object) value);
  }

  public static ScrollingIndicatorMode GetIndicatorMode(DependencyObject obj)
  {
    return (ScrollingIndicatorMode) obj.GetValue(ScrollBarHelper.IndicatorModeProperty);
  }

  public static void SetIndicatorMode(DependencyObject obj, ScrollingIndicatorMode value)
  {
    obj.SetValue(ScrollBarHelper.IndicatorModeProperty, (object) value);
  }

  public static bool GetAutoHide(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.AutoHideProperty);
  }

  public static void SetAutoHide(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.AutoHideProperty, (object) value);
  }

  internal static bool GetEnableAutoHideExtension(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.EnableAutoHideExtensionProperty);
  }

  internal static void SetEnableAutoHideExtension(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.EnableAutoHideExtensionProperty, (object) value);
  }

  private static void OnEnableAutoHideExtensionPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ScrollViewer sv))
      return;
    sv.Loaded -= new RoutedEventHandler(Sender_Loaded);
    sv.IsVisibleChanged += new DependencyPropertyChangedEventHandler(Sender_IsVisibleChanged);
    sv.MouseEnter -= new MouseEventHandler(Sender_MouseEnter);
    sv.PreviewMouseMove -= new MouseEventHandler(Sender_PreviewMouseMove);
    sv.MouseLeave -= new MouseEventHandler(Sender_MouseLeave);
    if (e.NewValue is bool newValue1 && newValue1)
    {
      sv.Loaded += new RoutedEventHandler(Sender_Loaded);
      sv.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(Sender_IsVisibleChanged);
      sv.MouseEnter += new MouseEventHandler(Sender_MouseEnter);
      sv.PreviewMouseMove += new MouseEventHandler(Sender_PreviewMouseMove);
      sv.MouseLeave += new MouseEventHandler(Sender_MouseLeave);
    }
    UpdateAutoHideState(sv);

    static void UpdateAutoHideState(ScrollViewer sv, bool initialize = false)
    {
      if (sv == null)
        return;
      if (ScrollBarHelper.GetAutoHide((DependencyObject) sv))
      {
        if (sv.IsMouseOver)
          VisualStateManager.GoToState((FrameworkElement) sv, "AutoHidePointerOver", true);
        else if (initialize)
          sv.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
          {
            VisualStateManager.GoToState((FrameworkElement) sv, "AutoHidePointerOver", false);
            VisualStateManager.GoToState((FrameworkElement) sv, "AutoHide", true);
          }));
        else
          VisualStateManager.GoToState((FrameworkElement) sv, "AutoHide", true);
      }
      else
        VisualStateManager.GoToState((FrameworkElement) sv, "IndicatorVisible", true);
    }

    static void Sender_Loaded(object _sender, RoutedEventArgs _e)
    {
      UpdateAutoHideState(_sender as ScrollViewer, true);
    }

    static void Sender_IsVisibleChanged(object _sender, DependencyPropertyChangedEventArgs _e)
    {
      if (!(_e.NewValue is bool newValue2) || !newValue2)
        return;
      ((FrameworkElement) _sender).Loaded -= new RoutedEventHandler(Sender_Loaded);
      ((UIElement) _sender).IsVisibleChanged += new DependencyPropertyChangedEventHandler(Sender_IsVisibleChanged);
      UpdateAutoHideState(_sender as ScrollViewer, true);
    }

    static void Sender_MouseLeave(object _sender, MouseEventArgs _e)
    {
      UpdateAutoHideState(_sender as ScrollViewer);
    }

    static void Sender_PreviewMouseMove(object _sender, MouseEventArgs _e)
    {
      UpdateAutoHideState(_sender as ScrollViewer);
    }

    static void Sender_MouseEnter(object _sender, MouseEventArgs _e)
    {
      UpdateAutoHideState(_sender as ScrollViewer);
    }
  }

  public static bool GetDisableBoundaryFeedback(DependencyObject obj)
  {
    return (bool) obj.GetValue(ScrollBarHelper.DisableBoundaryFeedbackProperty);
  }

  public static void SetDisableBoundaryFeedback(DependencyObject obj, bool value)
  {
    obj.SetValue(ScrollBarHelper.DisableBoundaryFeedbackProperty, (object) value);
  }

  private static void OnDisableBoundaryFeedbackPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ScrollViewer scrollViewer))
      return;
    scrollViewer.ManipulationBoundaryFeedback -= new EventHandler<ManipulationBoundaryFeedbackEventArgs>(Sender_ManipulationBoundaryFeedback);
    if (!(e.NewValue is bool newValue) || !newValue)
      return;
    scrollViewer.ManipulationBoundaryFeedback += new EventHandler<ManipulationBoundaryFeedbackEventArgs>(Sender_ManipulationBoundaryFeedback);

    static void Sender_ManipulationBoundaryFeedback(
      object _sender,
      ManipulationBoundaryFeedbackEventArgs _e)
    {
      _e.Handled = true;
    }
  }
}
