// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ScrollUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace pdfeditor.Utils;

public static class ScrollUtils
{
  private static ExponentialEase defaultSmoothScrollEasingFunction;
  private static ConcurrentDictionary<WeakReference<ScrollViewer>, Storyboard> animatingScrollViewers;

  public static ScrollViewer GetScrollViewer(ListBox listBox)
  {
    if (listBox == null)
      return (ScrollViewer) null;
    if (!listBox.IsLoaded)
      return (ScrollViewer) null;
    return VisualTreeHelper.GetChildrenCount((DependencyObject) listBox) > 0 && (VisualTreeHelper.GetChild((DependencyObject) listBox, 0) is Border child1 ? child1.Child : (UIElement) null) is ScrollViewer child2 ? child2 : (ScrollViewer) null;
  }

  public static ScrollViewer GetScrollViewerFromItemContainer(DependencyObject container)
  {
    ItemsControl reference = ItemsControl.ItemsControlFromItemContainer(container);
    if (reference == null || VisualTreeHelper.GetChildrenCount((DependencyObject) reference) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) reference, 0) is FrameworkElement child) || VisualTreeHelper.GetChildrenCount((DependencyObject) child) <= 0)
      return (ScrollViewer) null;
    if (!(VisualTreeHelper.GetChild((DependencyObject) child, 0) is ScrollViewer fromItemContainer))
      fromItemContainer = child.FindName("ScrollViewer") as ScrollViewer;
    return fromItemContainer;
  }

  public static void SmoothScrollToHorizontalOffset(
    this ScrollViewer scrollViewer,
    double offset,
    double maxSmoothScrollLength = 500.0,
    IEasingFunction easingFunction = null)
  {
    if (scrollViewer == null)
      return;
    ScrollUtils.TryStopScroll(scrollViewer, Orientation.Horizontal);
    if (offset < 0.0)
      offset = 0.0;
    if (offset > scrollViewer.ScrollableWidth)
      offset = scrollViewer.ScrollableWidth;
    double horizontalOffset = scrollViewer.HorizontalOffset;
    if (horizontalOffset + 8.0 > offset && horizontalOffset - 8.0 < offset || Math.Abs(horizontalOffset - offset) > Math.Abs(maxSmoothScrollLength))
      scrollViewer.ScrollToHorizontalOffset(offset);
    else
      ScrollUtils.SmoothScroll(scrollViewer, offset, easingFunction, Orientation.Horizontal);
  }

  public static void SmoothScrollToVerticalOffset(
    this ScrollViewer scrollViewer,
    double offset,
    double maxSmoothScrollLength = 500.0,
    IEasingFunction easingFunction = null)
  {
    if (scrollViewer == null)
      return;
    ScrollUtils.TryStopScroll(scrollViewer, Orientation.Vertical);
    if (offset < 0.0)
      offset = 0.0;
    if (offset > scrollViewer.ScrollableHeight)
      offset = scrollViewer.ScrollableHeight;
    double verticalOffset = scrollViewer.VerticalOffset;
    if (verticalOffset + 8.0 > offset && verticalOffset - 8.0 < offset || Math.Abs(verticalOffset - offset) > Math.Abs(maxSmoothScrollLength))
      scrollViewer.ScrollToVerticalOffset(offset);
    else
      ScrollUtils.SmoothScroll(scrollViewer, offset, easingFunction, Orientation.Vertical);
  }

  private static void TryStopScroll(ScrollViewer scrollViewer, Orientation orientation)
  {
    if (scrollViewer == null || ScrollUtils.animatingScrollViewers == null)
      return;
    lock (ScrollUtils.animatingScrollViewers)
    {
      WeakReference<ScrollViewer> key = (WeakReference<ScrollViewer>) null;
      foreach (KeyValuePair<WeakReference<ScrollViewer>, Storyboard> animatingScrollViewer in ScrollUtils.animatingScrollViewers)
      {
        ScrollViewer target;
        if (animatingScrollViewer.Key != null && animatingScrollViewer.Key.TryGetTarget(out target) && target == scrollViewer)
        {
          key = animatingScrollViewer.Key;
          Storyboard storyboard = animatingScrollViewer.Value;
          if (storyboard.GetCurrentState() == ClockState.Active)
          {
            storyboard.Pause();
            if (orientation == Orientation.Horizontal)
            {
              double horizontalOffset = target.HorizontalOffset;
              storyboard.Stop();
              if (target.HorizontalOffset != horizontalOffset)
              {
                target.ScrollToHorizontalOffset(horizontalOffset);
                target.UpdateLayout();
              }
            }
            else
            {
              double verticalOffset = target.VerticalOffset;
              storyboard.Stop();
              if (target.VerticalOffset != verticalOffset)
              {
                target.ScrollToVerticalOffset(verticalOffset);
                target.UpdateLayout();
              }
            }
          }
        }
      }
      if (key == null)
        return;
      ScrollUtils.animatingScrollViewers.TryRemove(key, out Storyboard _);
    }
  }

  private static void SmoothScroll(
    ScrollViewer scrollViewer,
    double offset,
    IEasingFunction easingFunction,
    Orientation orientation)
  {
    IEasingFunction easingFunction1 = easingFunction ?? CreateDefaultEasingFunction();
    double initValue;
    Action<double> action;
    switch (orientation)
    {
      case Orientation.Vertical:
        initValue = scrollViewer.VerticalOffset;
        action = (Action<double>) (d => scrollViewer.ScrollToVerticalOffset(d));
        break;
      default:
        initValue = scrollViewer.HorizontalOffset;
        action = (Action<double>) (d => scrollViewer.ScrollToHorizontalOffset(d));
        break;
    }
    ScrollUtils.ValueWrapper<double> valueWrapper = new ScrollUtils.ValueWrapper<double>(initValue, action);
    DoubleAnimation doubleAnimation = new DoubleAnimation();
    doubleAnimation.To = new double?(offset);
    doubleAnimation.FillBehavior = FillBehavior.HoldEnd;
    doubleAnimation.EasingFunction = easingFunction1;
    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
    DoubleAnimation element = doubleAnimation;
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) valueWrapper);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("Value", Array.Empty<object>()));
    Storyboard storyboard1 = new Storyboard();
    storyboard1.Children.Add((Timeline) element);
    Storyboard storyboard2 = storyboard1;
    if (ScrollUtils.animatingScrollViewers == null)
      ScrollUtils.animatingScrollViewers = new ConcurrentDictionary<WeakReference<ScrollViewer>, Storyboard>();
    ScrollUtils.animatingScrollViewers[new WeakReference<ScrollViewer>(scrollViewer)] = storyboard2;
    storyboard2.Completed += (EventHandler) ((s, a) => ScrollUtils.TryStopScroll(scrollViewer, orientation));
    storyboard2.Begin();

    static IEasingFunction CreateDefaultEasingFunction()
    {
      if (ScrollUtils.defaultSmoothScrollEasingFunction == null)
      {
        ExponentialEase exponentialEase = new ExponentialEase();
        exponentialEase.EasingMode = EasingMode.EaseOut;
        exponentialEase.Exponent = 7.0;
        exponentialEase.Freeze();
        ScrollUtils.defaultSmoothScrollEasingFunction = exponentialEase;
      }
      return (IEasingFunction) ScrollUtils.defaultSmoothScrollEasingFunction;
    }
  }

  private class ValueWrapper<T> : DependencyObject
  {
    private readonly Action<T> action;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (T), typeof (ScrollUtils.ValueWrapper<T>), new PropertyMetadata((object) default (T), new PropertyChangedCallback(ScrollUtils.ValueWrapper<T>.OnValuePropertyChanged)));

    public ValueWrapper(T initValue, Action<T> action)
    {
      this.Value = initValue;
      this.action = action;
    }

    public T Value
    {
      get => (T) this.GetValue(ScrollUtils.ValueWrapper<T>.ValueProperty);
      set => this.SetValue(ScrollUtils.ValueWrapper<T>.ValueProperty, (object) value);
    }

    private static void OnValuePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (object.Equals(e.NewValue, e.OldValue) || !(d is ScrollUtils.ValueWrapper<T> valueWrapper))
        return;
      Action<T> action = valueWrapper.action;
      if (action == null)
        return;
      action((T) e.NewValue);
    }
  }
}
