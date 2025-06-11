// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Primitives.CalendarButton
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Controls.Primitives;

[DesignTimeVisible(false)]
public sealed class CalendarButton : Button, IDisposable
{
  private bool _shouldCoerceContent;
  private object _coercedContent;
  internal static readonly DependencyPropertyKey HasSelectedDaysPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasSelectedDays), typeof (bool), typeof (CalendarButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty HasSelectedDaysProperty = CalendarButton.HasSelectedDaysPropertyKey.DependencyProperty;
  internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsInactive), typeof (bool), typeof (CalendarButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsInactiveProperty = CalendarButton.IsInactivePropertyKey.DependencyProperty;

  static CalendarButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CalendarButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CalendarButton)));
    ContentControl.ContentProperty.OverrideMetadata(typeof (CalendarButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(CalendarButton.OnCoerceContent)));
  }

  public CalendarButton()
  {
    this.Loaded += (RoutedEventHandler) ((param0, param1) => this.ChangeVisualState(false));
  }

  public bool HasSelectedDays
  {
    get => (bool) this.GetValue(CalendarButton.HasSelectedDaysProperty);
    internal set => this.SetValue(CalendarButton.HasSelectedDaysPropertyKey, (object) value);
  }

  public bool IsInactive
  {
    get => (bool) this.GetValue(CalendarButton.IsInactiveProperty);
    internal set => this.SetValue(CalendarButton.IsInactivePropertyKey, (object) value);
  }

  internal Syncfusion.Windows.Controls.Calendar Owner { get; set; }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ChangeVisualState(false);
  }

  public void Dispose()
  {
    if (this._coercedContent != null)
      this._coercedContent = (object) null;
    if (this.Owner != null)
      this.Owner = (Syncfusion.Windows.Controls.Calendar) null;
    BindingOperations.ClearAllBindings((DependencyObject) this);
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new Syncfusion.Windows.Automation.Peers.CalendarButtonAutomationPeer(this);
  }

  protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    this.ChangeVisualState(true);
    base.OnGotKeyboardFocus(e);
  }

  protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    this.ChangeVisualState(true);
    base.OnLostKeyboardFocus(e);
  }

  internal void SetContentInternal(string value)
  {
    if (BindingOperations.GetBindingExpressionBase((DependencyObject) this, ContentControl.ContentProperty) != null)
    {
      this.Content = (object) value;
    }
    else
    {
      this._shouldCoerceContent = true;
      this._coercedContent = (object) value;
      this.CoerceValue(ContentControl.ContentProperty);
    }
  }

  private new void ChangeVisualState(bool useTransitions)
  {
  }

  private new static void OnVisualStatePropertyChanged(
    DependencyObject dObject,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(dObject is CalendarButton calendarButton) || object.Equals(e.OldValue, e.NewValue))
      return;
    calendarButton.ChangeVisualState(true);
  }

  private static object OnCoerceContent(DependencyObject sender, object baseValue)
  {
    CalendarButton calendarButton = (CalendarButton) sender;
    if (!calendarButton._shouldCoerceContent)
      return baseValue;
    calendarButton._shouldCoerceContent = false;
    return calendarButton._coercedContent;
  }
}
