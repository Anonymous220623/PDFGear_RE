// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ScrollViewer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

public class ScrollViewer : System.Windows.Controls.ScrollViewer
{
  private double _totalVerticalOffset;
  private double _totalHorizontalOffset;
  private bool _isRunning;
  public static readonly DependencyProperty CanMouseWheelProperty = DependencyProperty.Register(nameof (CanMouseWheel), typeof (bool), typeof (ScrollViewer), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty IsInertiaEnabledProperty = DependencyProperty.RegisterAttached(nameof (IsInertiaEnabled), typeof (bool), typeof (ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsPenetratingProperty = DependencyProperty.RegisterAttached(nameof (IsPenetrating), typeof (bool), typeof (ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));
  internal static readonly DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register(nameof (CurrentVerticalOffset), typeof (double), typeof (ScrollViewer), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(ScrollViewer.OnCurrentVerticalOffsetChanged)));
  internal static readonly DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register(nameof (CurrentHorizontalOffset), typeof (double), typeof (ScrollViewer), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(ScrollViewer.OnCurrentHorizontalOffsetChanged)));

  public bool CanMouseWheel
  {
    get => (bool) this.GetValue(ScrollViewer.CanMouseWheelProperty);
    set => this.SetValue(ScrollViewer.CanMouseWheelProperty, ValueBoxes.BooleanBox(value));
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (!this.CanMouseWheel)
      return;
    if (!this.IsInertiaEnabled)
    {
      if (ScrollViewerAttach.GetOrientation((DependencyObject) this) == Orientation.Vertical)
      {
        base.OnMouseWheel(e);
      }
      else
      {
        this._totalHorizontalOffset = this.HorizontalOffset;
        this.CurrentHorizontalOffset = this.HorizontalOffset;
        this._totalHorizontalOffset = Math.Min(Math.Max(0.0, this._totalHorizontalOffset - (double) e.Delta), this.ScrollableWidth);
        this.CurrentHorizontalOffset = this._totalHorizontalOffset;
      }
    }
    else
    {
      e.Handled = true;
      if (ScrollViewerAttach.GetOrientation((DependencyObject) this) == Orientation.Vertical)
      {
        if (!this._isRunning)
        {
          this._totalVerticalOffset = this.VerticalOffset;
          this.CurrentVerticalOffset = this.VerticalOffset;
        }
        this._totalVerticalOffset = Math.Min(Math.Max(0.0, this._totalVerticalOffset - (double) e.Delta), this.ScrollableHeight);
        this.ScrollToVerticalOffsetWithAnimation(this._totalVerticalOffset);
      }
      else
      {
        if (!this._isRunning)
        {
          this._totalHorizontalOffset = this.HorizontalOffset;
          this.CurrentHorizontalOffset = this.HorizontalOffset;
        }
        this._totalHorizontalOffset = Math.Min(Math.Max(0.0, this._totalHorizontalOffset - (double) e.Delta), this.ScrollableWidth);
        this.ScrollToHorizontalOffsetWithAnimation(this._totalHorizontalOffset);
      }
    }
  }

  internal void ScrollToTopInternal(double milliseconds = 500.0)
  {
    if (!this._isRunning)
    {
      this._totalVerticalOffset = this.VerticalOffset;
      this.CurrentVerticalOffset = this.VerticalOffset;
    }
    this.ScrollToVerticalOffsetWithAnimation(0.0, milliseconds);
  }

  public void ScrollToVerticalOffsetWithAnimation(double offset, double milliseconds = 500.0)
  {
    DoubleAnimation animation = AnimationHelper.CreateAnimation(offset, milliseconds);
    DoubleAnimation doubleAnimation = animation;
    CubicEase cubicEase = new CubicEase();
    cubicEase.EasingMode = EasingMode.EaseOut;
    doubleAnimation.EasingFunction = (IEasingFunction) cubicEase;
    animation.FillBehavior = FillBehavior.Stop;
    animation.Completed += (EventHandler) ((s, e1) =>
    {
      this.CurrentVerticalOffset = offset;
      this._isRunning = false;
    });
    this._isRunning = true;
    this.BeginAnimation(ScrollViewer.CurrentVerticalOffsetProperty, (AnimationTimeline) animation, HandoffBehavior.Compose);
  }

  public void ScrollToHorizontalOffsetWithAnimation(double offset, double milliseconds = 500.0)
  {
    DoubleAnimation animation = AnimationHelper.CreateAnimation(offset, milliseconds);
    DoubleAnimation doubleAnimation = animation;
    CubicEase cubicEase = new CubicEase();
    cubicEase.EasingMode = EasingMode.EaseOut;
    doubleAnimation.EasingFunction = (IEasingFunction) cubicEase;
    animation.FillBehavior = FillBehavior.Stop;
    animation.Completed += (EventHandler) ((s, e1) =>
    {
      this.CurrentHorizontalOffset = offset;
      this._isRunning = false;
    });
    this._isRunning = true;
    this.BeginAnimation(ScrollViewer.CurrentHorizontalOffsetProperty, (AnimationTimeline) animation, HandoffBehavior.Compose);
  }

  protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
  {
    return !this.IsPenetrating ? base.HitTestCore(hitTestParameters) : (HitTestResult) null;
  }

  public static void SetIsInertiaEnabled(DependencyObject element, bool value)
  {
    element.SetValue(ScrollViewer.IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsInertiaEnabled(DependencyObject element)
  {
    return (bool) element.GetValue(ScrollViewer.IsInertiaEnabledProperty);
  }

  public bool IsInertiaEnabled
  {
    get => (bool) this.GetValue(ScrollViewer.IsInertiaEnabledProperty);
    set => this.SetValue(ScrollViewer.IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsPenetrating
  {
    get => (bool) this.GetValue(ScrollViewer.IsPenetratingProperty);
    set => this.SetValue(ScrollViewer.IsPenetratingProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetIsPenetrating(DependencyObject element, bool value)
  {
    element.SetValue(ScrollViewer.IsPenetratingProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsPenetrating(DependencyObject element)
  {
    return (bool) element.GetValue(ScrollViewer.IsPenetratingProperty);
  }

  private static void OnCurrentVerticalOffsetChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ScrollViewer scrollViewer) || !(e.NewValue is double newValue))
      return;
    scrollViewer.ScrollToVerticalOffset(newValue);
  }

  internal double CurrentVerticalOffset
  {
    get => (double) this.GetValue(ScrollViewer.CurrentVerticalOffsetProperty);
    set => this.SetValue(ScrollViewer.CurrentVerticalOffsetProperty, (object) value);
  }

  private static void OnCurrentHorizontalOffsetChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ScrollViewer scrollViewer) || !(e.NewValue is double newValue))
      return;
    scrollViewer.ScrollToHorizontalOffset(newValue);
  }

  internal double CurrentHorizontalOffset
  {
    get => (double) this.GetValue(ScrollViewer.CurrentHorizontalOffsetProperty);
    set => this.SetValue(ScrollViewer.CurrentHorizontalOffsetProperty, (object) value);
  }
}
