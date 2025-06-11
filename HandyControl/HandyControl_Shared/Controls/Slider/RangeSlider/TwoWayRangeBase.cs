// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TwoWayRangeBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class TwoWayRangeBase : Control
{
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(TwoWayRangeBase.OnMinimumChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double10Box, new PropertyChangedCallback(TwoWayRangeBase.OnMaximumChanged), new CoerceValueCallback(TwoWayRangeBase.CoerceMaximum)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty ValueStartProperty = DependencyProperty.Register(nameof (ValueStart), typeof (double), typeof (TwoWayRangeBase), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TwoWayRangeBase.OnValueStartChanged), new CoerceValueCallback(TwoWayRangeBase.ConstrainToRange)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty ValueEndProperty = DependencyProperty.Register(nameof (ValueEnd), typeof (double), typeof (TwoWayRangeBase), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TwoWayRangeBase.OnValueEndChanged), new CoerceValueCallback(TwoWayRangeBase.ConstrainToRange)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof (LargeChange), typeof (double), typeof (TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double1Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof (SmallChange), typeof (double), typeof (TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double01Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (RoutedPropertyChangedEventHandler<DoubleRange>), typeof (TwoWayRangeBase));

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TwoWayRangeBase twoWayRangeBase = (TwoWayRangeBase) d;
    twoWayRangeBase.CoerceValue(TwoWayRangeBase.MaximumProperty);
    twoWayRangeBase.CoerceValue(TwoWayRangeBase.ValueStartProperty);
    twoWayRangeBase.CoerceValue(TwoWayRangeBase.ValueEndProperty);
    twoWayRangeBase.OnMinimumChanged((double) e.OldValue, (double) e.NewValue);
  }

  protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
  {
  }

  public double Minimum
  {
    get => (double) this.GetValue(TwoWayRangeBase.MinimumProperty);
    set => this.SetValue(TwoWayRangeBase.MinimumProperty, (object) value);
  }

  private static object CoerceMaximum(DependencyObject d, object basevalue)
  {
    double minimum = ((TwoWayRangeBase) d).Minimum;
    return (double) basevalue < minimum ? (object) minimum : basevalue;
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TwoWayRangeBase twoWayRangeBase = (TwoWayRangeBase) d;
    twoWayRangeBase.CoerceValue(TwoWayRangeBase.ValueStartProperty);
    twoWayRangeBase.CoerceValue(TwoWayRangeBase.ValueEndProperty);
    twoWayRangeBase.OnMaximumChanged((double) e.OldValue, (double) e.NewValue);
  }

  protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
  {
  }

  public double Maximum
  {
    get => (double) this.GetValue(TwoWayRangeBase.MaximumProperty);
    set => this.SetValue(TwoWayRangeBase.MaximumProperty, (object) value);
  }

  private static void OnValueStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TwoWayRangeBase twoWayRangeBase = (TwoWayRangeBase) d;
    twoWayRangeBase.OnValueChanged(new DoubleRange()
    {
      Start = (double) e.OldValue,
      End = twoWayRangeBase.ValueEnd
    }, new DoubleRange()
    {
      Start = (double) e.NewValue,
      End = twoWayRangeBase.ValueEnd
    });
  }

  protected virtual void OnValueChanged(DoubleRange oldValue, DoubleRange newValue)
  {
    RoutedPropertyChangedEventArgs<DoubleRange> e = new RoutedPropertyChangedEventArgs<DoubleRange>(oldValue, newValue);
    e.RoutedEvent = TwoWayRangeBase.ValueChangedEvent;
    this.RaiseEvent((RoutedEventArgs) e);
  }

  public double ValueStart
  {
    get => (double) this.GetValue(TwoWayRangeBase.ValueStartProperty);
    set => this.SetValue(TwoWayRangeBase.ValueStartProperty, (object) value);
  }

  private static void OnValueEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TwoWayRangeBase twoWayRangeBase = (TwoWayRangeBase) d;
    twoWayRangeBase.OnValueChanged(new DoubleRange()
    {
      Start = twoWayRangeBase.ValueStart,
      End = (double) e.OldValue
    }, new DoubleRange()
    {
      Start = twoWayRangeBase.ValueStart,
      End = (double) e.NewValue
    });
  }

  public double ValueEnd
  {
    get => (double) this.GetValue(TwoWayRangeBase.ValueEndProperty);
    set => this.SetValue(TwoWayRangeBase.ValueEndProperty, (object) value);
  }

  internal static object ConstrainToRange(DependencyObject d, object value)
  {
    TwoWayRangeBase twoWayRangeBase = (TwoWayRangeBase) d;
    double minimum = twoWayRangeBase.Minimum;
    double num = (double) value;
    if (num < minimum)
      return (object) minimum;
    double maximum = twoWayRangeBase.Maximum;
    return num > maximum ? (object) maximum : value;
  }

  public double LargeChange
  {
    get => (double) this.GetValue(TwoWayRangeBase.LargeChangeProperty);
    set => this.SetValue(TwoWayRangeBase.LargeChangeProperty, (object) value);
  }

  public double SmallChange
  {
    get => (double) this.GetValue(TwoWayRangeBase.SmallChangeProperty);
    set => this.SetValue(TwoWayRangeBase.SmallChangeProperty, (object) value);
  }

  public event RoutedPropertyChangedEventHandler<DoubleRange> ValueChanged
  {
    add => this.AddHandler(TwoWayRangeBase.ValueChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(TwoWayRangeBase.ValueChangedEvent, (Delegate) value);
  }
}
