// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Primitives.CalendarDayButton
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Automation.Peers;
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
public sealed class CalendarDayButton : Button, IDisposable
{
  private const int DEFAULTCONTENT = 1;
  internal const string StateToday = "Today";
  internal const string StateRegularDay = "RegularDay";
  internal const string GroupDay = "DayStates";
  internal const string StateBlackoutDay = "BlackoutDay";
  internal const string StateNormalDay = "NormalDay";
  internal const string GroupBlackout = "BlackoutDayStates";
  private bool _shouldCoerceContent;
  private object _coercedContent;
  internal static readonly DependencyPropertyKey IsTodayPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsToday), typeof (bool), typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarDayButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsTodayProperty = CalendarDayButton.IsTodayPropertyKey.DependencyProperty;
  internal static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsSelected), typeof (bool), typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarDayButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsSelectedProperty = CalendarDayButton.IsSelectedPropertyKey.DependencyProperty;
  internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsInactive), typeof (bool), typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarDayButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsInactiveProperty = CalendarDayButton.IsInactivePropertyKey.DependencyProperty;
  internal static readonly DependencyPropertyKey IsBlackedOutPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsBlackedOut), typeof (bool), typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarDayButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsBlackedOutProperty = CalendarDayButton.IsBlackedOutPropertyKey.DependencyProperty;
  internal static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsHighlighted), typeof (bool), typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarDayButton.OnVisualStatePropertyChanged)));
  public static readonly DependencyProperty IsHighlightedProperty = CalendarDayButton.IsHighlightedPropertyKey.DependencyProperty;

  static CalendarDayButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CalendarDayButton)));
    ContentControl.ContentProperty.OverrideMetadata(typeof (CalendarDayButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(CalendarDayButton.OnCoerceContent)));
  }

  public CalendarDayButton()
  {
    this.Loaded += (RoutedEventHandler) ((param0, param1) => this.ChangeVisualState(false));
  }

  public bool IsToday => (bool) this.GetValue(CalendarDayButton.IsTodayProperty);

  public bool IsSelected => (bool) this.GetValue(CalendarDayButton.IsSelectedProperty);

  public bool IsInactive => (bool) this.GetValue(CalendarDayButton.IsInactiveProperty);

  public bool IsBlackedOut => (bool) this.GetValue(CalendarDayButton.IsBlackedOutProperty);

  public bool IsHighlighted => (bool) this.GetValue(CalendarDayButton.IsHighlightedProperty);

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
    return (AutomationPeer) new CalendarDayButtonAutomationPeer(this);
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

  internal new void ChangeVisualState(bool useTransitions)
  {
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

  private new static void OnVisualStatePropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is CalendarDayButton calendarDayButton))
      return;
    calendarDayButton.ChangeVisualState(true);
  }

  private static object OnCoerceContent(DependencyObject sender, object baseValue)
  {
    CalendarDayButton calendarDayButton = (CalendarDayButton) sender;
    if (!calendarDayButton._shouldCoerceContent)
      return baseValue;
    calendarDayButton._shouldCoerceContent = false;
    return calendarDayButton._coercedContent;
  }
}
