// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Badge
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class Badge : ContentControl
{
  private int? _originalValue;
  public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<int>>), typeof (Badge));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (Badge), new PropertyMetadata((object) "0"));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (int), typeof (Badge), new PropertyMetadata(ValueBoxes.Int0Box, new PropertyChangedCallback(Badge.OnValueChanged)));
  public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof (Status), typeof (BadgeStatus), typeof (Badge), new PropertyMetadata((object) BadgeStatus.Text));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (int), typeof (Badge), new PropertyMetadata(ValueBoxes.Int99Box, new PropertyChangedCallback(Badge.OnMaximumChanged)));
  public static readonly DependencyProperty BadgeMarginProperty = DependencyProperty.Register(nameof (BadgeMargin), typeof (Thickness), typeof (Badge), new PropertyMetadata((object) new Thickness()));
  public static readonly DependencyProperty ShowBadgeProperty = DependencyProperty.Register(nameof (ShowBadge), typeof (bool), typeof (Badge), new PropertyMetadata(ValueBoxes.TrueBox));

  public event EventHandler<FunctionEventArgs<int>> ValueChanged
  {
    add => this.AddHandler(Badge.ValueChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(Badge.ValueChangedEvent, (Delegate) value);
  }

  public string Text
  {
    get => (string) this.GetValue(Badge.TextProperty);
    set => this.SetValue(Badge.TextProperty, (object) value);
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Badge source = (Badge) d;
    int newValue = (int) e.NewValue;
    source.SetCurrentValue(Badge.TextProperty, newValue <= source.Maximum ? (object) newValue.ToString() : (object) $"{source.Maximum}+");
    if (source.IsLoaded)
      source.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<int>(Badge.ValueChangedEvent, (object) source)
      {
        Info = newValue
      });
    else
      source._originalValue = new int?(newValue);
  }

  public int Value
  {
    get => (int) this.GetValue(Badge.ValueProperty);
    set => this.SetValue(Badge.ValueProperty, (object) value);
  }

  public BadgeStatus Status
  {
    get => (BadgeStatus) this.GetValue(Badge.StatusProperty);
    set => this.SetValue(Badge.StatusProperty, (object) value);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Badge badge = (Badge) d;
    int num = badge.Value;
    badge.SetCurrentValue(Badge.TextProperty, num <= badge.Maximum ? (object) num.ToString() : (object) $"{badge.Maximum}+");
  }

  public int Maximum
  {
    get => (int) this.GetValue(Badge.MaximumProperty);
    set => this.SetValue(Badge.MaximumProperty, (object) value);
  }

  public Thickness BadgeMargin
  {
    get => (Thickness) this.GetValue(Badge.BadgeMarginProperty);
    set => this.SetValue(Badge.BadgeMarginProperty, (object) value);
  }

  public bool ShowBadge
  {
    get => (bool) this.GetValue(Badge.ShowBadgeProperty);
    set => this.SetValue(Badge.ShowBadgeProperty, ValueBoxes.BooleanBox(value));
  }

  protected override Geometry GetLayoutClip(Size layoutSlotSize) => (Geometry) null;

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (!this._originalValue.HasValue)
      return;
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<int>(Badge.ValueChangedEvent, (object) this)
    {
      Info = this._originalValue.Value
    });
    this._originalValue = new int?();
  }
}
