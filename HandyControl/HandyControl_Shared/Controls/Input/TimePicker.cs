// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TimePicker
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Root", Type = typeof (Grid))]
[TemplatePart(Name = "PART_TextBox", Type = typeof (WatermarkTextBox))]
[TemplatePart(Name = "PART_Button", Type = typeof (Button))]
[TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
public class TimePicker : Control
{
  private const string ElementRoot = "PART_Root";
  private const string ElementTextBox = "PART_TextBox";
  private const string ElementButton = "PART_Button";
  private const string ElementPopup = "PART_Popup";
  private string _defaultText;
  private ButtonBase _dropDownButton;
  private Popup _popup;
  private bool _disablePopupReopen;
  private WatermarkTextBox _textBox;
  private IDictionary<DependencyProperty, bool> _isHandlerSuspended;
  private DateTime? _originalSelectedTime;
  public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct, typeof (EventHandler<FunctionEventArgs<DateTime?>>), typeof (TimePicker));
  public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(nameof (TimeFormat), typeof (string), typeof (TimePicker), new PropertyMetadata((object) "HH:mm:ss"));
  public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(nameof (DisplayTime), typeof (DateTime), typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (PropertyChangedCallback) null, new CoerceValueCallback(TimePicker.CoerceDisplayTime)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TimePicker.OnIsDropDownOpenChanged), new CoerceValueCallback(TimePicker.OnCoerceIsDropDownOpen)));
  public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(nameof (SelectedTime), typeof (DateTime?), typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TimePicker.OnSelectedTimeChanged), new CoerceValueCallback(TimePicker.CoerceSelectedTime)));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(TimePicker.OnTextChanged)));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (TimePicker));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (TimePicker));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (TimePicker));
  public static readonly DependencyProperty ClockProperty = DependencyProperty.Register(nameof (Clock), typeof (ClockBase), typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(TimePicker.OnClockChanged)));

  public event EventHandler<FunctionEventArgs<DateTime?>> SelectedTimeChanged
  {
    add => this.AddHandler(TimePicker.SelectedTimeChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(TimePicker.SelectedTimeChangedEvent, (Delegate) value);
  }

  public event RoutedEventHandler ClockClosed;

  public event RoutedEventHandler ClockOpened;

  static TimePicker()
  {
    EventManager.RegisterClassHandler(typeof (TimePicker), UIElement.GotFocusEvent, (Delegate) new RoutedEventHandler(TimePicker.OnGotFocus));
    KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Once));
    KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof (TimePicker), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
  }

  public TimePicker()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this.SetCurrentValue(TimePicker.SelectedTimeProperty, (object) null);
      this.SetCurrentValue(TimePicker.TextProperty, (object) "");
      this._textBox.Text = string.Empty;
    })));
  }

  public string TimeFormat
  {
    get => (string) this.GetValue(TimePicker.TimeFormatProperty);
    set => this.SetValue(TimePicker.TimeFormatProperty, (object) value);
  }

  public DateTime DisplayTime
  {
    get => (DateTime) this.GetValue(TimePicker.DisplayTimeProperty);
    set => this.SetValue(TimePicker.DisplayTimeProperty, (object) value);
  }

  private static object CoerceDisplayTime(DependencyObject d, object value)
  {
    TimePicker timePicker = (TimePicker) d;
    timePicker.Clock.DisplayTime = (DateTime) value;
    return (object) timePicker.Clock.DisplayTime;
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(TimePicker.IsDropDownOpenProperty);
    set => this.SetValue(TimePicker.IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
  }

  private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
  {
    return !(d is TimePicker timePicker) || timePicker.IsEnabled ? baseValue : (object) false;
  }

  private static void OnIsDropDownOpenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TimePicker dp = d as TimePicker;
    bool newValue = (bool) e.NewValue;
    if (dp?._popup == null || dp._popup.IsOpen == newValue)
      return;
    dp._popup.IsOpen = newValue;
    if (!newValue)
      return;
    dp._originalSelectedTime = dp.SelectedTime;
    dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Delegate) (() => dp.Clock.Focus()));
  }

  public DateTime? SelectedTime
  {
    get => (DateTime?) this.GetValue(TimePicker.SelectedTimeProperty);
    set => this.SetValue(TimePicker.SelectedTimeProperty, (object) value);
  }

  private static void OnSelectedTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TimePicker source))
      return;
    if (source.SelectedTime.HasValue)
    {
      DateTime d1 = source.SelectedTime.Value;
      source.SetTextInternal(source.DateTimeToString(d1));
    }
    source.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<DateTime?>(TimePicker.SelectedTimeChangedEvent, (object) source)
    {
      Info = source.SelectedTime
    });
  }

  private static object CoerceSelectedTime(DependencyObject d, object value)
  {
    TimePicker timePicker = (TimePicker) d;
    if (timePicker.Clock == null)
      return (object) (DateTime?) value;
    timePicker.Clock.SelectedTime = (DateTime?) value;
    return (object) timePicker.Clock.SelectedTime;
  }

  public string Text
  {
    get => (string) this.GetValue(TimePicker.TextProperty);
    set => this.SetValue(TimePicker.TextProperty, (object) value);
  }

  private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TimePicker timePicker) || timePicker.IsHandlerSuspended(TimePicker.TextProperty))
      return;
    if (e.NewValue is string newValue)
    {
      if (timePicker._textBox != null)
        timePicker._textBox.Text = newValue;
      else
        timePicker._defaultText = newValue;
      timePicker.SetSelectedTime();
    }
    else
      timePicker.SetValueNoCallback(TimePicker.SelectedTimeProperty, (object) null);
  }

  private void SetTextInternal(string value)
  {
    this.SetCurrentValue(TimePicker.TextProperty, (object) value);
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(TimePicker.SelectionBrushProperty);
    set => this.SetValue(TimePicker.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(TimePicker.SelectionOpacityProperty);
    set => this.SetValue(TimePicker.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(TimePicker.CaretBrushProperty);
    set => this.SetValue(TimePicker.CaretBrushProperty, (object) value);
  }

  private static void OnClockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TimePicker timePicker = (TimePicker) d;
    if (e.OldValue is ClockBase oldValue)
    {
      oldValue.SelectedTimeChanged -= new EventHandler<FunctionEventArgs<DateTime?>>(timePicker.Clock_SelectedTimeChanged);
      oldValue.Confirmed -= new Action(timePicker.Clock_Confirmed);
      timePicker.SetPopupChild((UIElement) null);
    }
    if (!(e.NewValue is ClockBase newValue))
      return;
    newValue.ShowConfirmButton = true;
    newValue.SelectedTimeChanged += new EventHandler<FunctionEventArgs<DateTime?>>(timePicker.Clock_SelectedTimeChanged);
    newValue.Confirmed += new Action(timePicker.Clock_Confirmed);
    timePicker.SetPopupChild((UIElement) newValue);
  }

  public ClockBase Clock
  {
    get => (ClockBase) this.GetValue(TimePicker.ClockProperty);
    set => this.SetValue(TimePicker.ClockProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      return;
    if (this._popup != null)
    {
      this._popup.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.PopupPreviewMouseLeftButtonDown);
      this._popup.Opened -= new EventHandler(this.PopupOpened);
      this._popup.Closed -= new EventHandler(this.PopupClosed);
      this._popup.Child = (UIElement) null;
    }
    if (this._dropDownButton != null)
    {
      this._dropDownButton.Click -= new RoutedEventHandler(this.DropDownButton_Click);
      this._dropDownButton.MouseLeave -= new MouseEventHandler(this.DropDownButton_MouseLeave);
    }
    if (this._textBox != null)
    {
      this._textBox.KeyDown -= new KeyEventHandler(this.TextBox_KeyDown);
      this._textBox.TextChanged -= new TextChangedEventHandler(this.TextBox_TextChanged);
      this._textBox.LostFocus -= new RoutedEventHandler(this.TextBox_LostFocus);
    }
    base.OnApplyTemplate();
    this._popup = this.GetTemplateChild("PART_Popup") as Popup;
    this._dropDownButton = (ButtonBase) (this.GetTemplateChild("PART_Button") as Button);
    this._textBox = this.GetTemplateChild("PART_TextBox") as WatermarkTextBox;
    this.CheckNull();
    this._popup.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.PopupPreviewMouseLeftButtonDown);
    this._popup.Opened += new EventHandler(this.PopupOpened);
    this._popup.Closed += new EventHandler(this.PopupClosed);
    this._popup.Child = (UIElement) this.Clock;
    this._dropDownButton.Click += new RoutedEventHandler(this.DropDownButton_Click);
    this._dropDownButton.MouseLeave += new MouseEventHandler(this.DropDownButton_MouseLeave);
    DateTime? selectedTime = this.SelectedTime;
    if (this._textBox != null)
    {
      this._textBox.SetBinding(TimePicker.SelectionBrushProperty, (BindingBase) new Binding(TimePicker.SelectionBrushProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.SetBinding(TimePicker.SelectionOpacityProperty, (BindingBase) new Binding(TimePicker.SelectionOpacityProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.SetBinding(TimePicker.CaretBrushProperty, (BindingBase) new Binding(TimePicker.CaretBrushProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.KeyDown += new KeyEventHandler(this.TextBox_KeyDown);
      this._textBox.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
      this._textBox.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
      if (!selectedTime.HasValue)
      {
        if (!string.IsNullOrEmpty(this._defaultText))
        {
          this._textBox.Text = this._defaultText;
          this.SetSelectedTime();
        }
      }
      else
        this._textBox.Text = this.DateTimeToString(selectedTime.Value);
    }
    this.EnsureClock();
    if (!selectedTime.HasValue)
    {
      this._originalSelectedTime.GetValueOrDefault();
      if (!this._originalSelectedTime.HasValue)
        this._originalSelectedTime = new DateTime?(DateTime.Now);
      this.SetCurrentValue(TimePicker.DisplayTimeProperty, (object) this._originalSelectedTime);
    }
    else
      this.SetCurrentValue(TimePicker.DisplayTimeProperty, (object) selectedTime);
  }

  public override string ToString()
  {
    DateTime? selectedTime = this.SelectedTime;
    ref DateTime? local = ref selectedTime;
    return (local.HasValue ? local.GetValueOrDefault().ToString(this.TimeFormat) : (string) null) ?? string.Empty;
  }

  protected virtual void OnClockClosed(RoutedEventArgs e)
  {
    RoutedEventHandler clockClosed = this.ClockClosed;
    if (clockClosed != null)
      clockClosed((object) this, e);
    this.Clock?.OnClockClosed();
  }

  protected virtual void OnClockOpened(RoutedEventArgs e)
  {
    RoutedEventHandler clockOpened = this.ClockOpened;
    if (clockOpened != null)
      clockOpened((object) this, e);
    this.Clock?.OnClockOpened();
  }

  private void CheckNull()
  {
    if (this._dropDownButton == null || this._popup == null || this._textBox == null)
      throw new Exception();
  }

  private void TextBox_LostFocus(object sender, RoutedEventArgs e) => this.SetSelectedTime();

  private void SetIsHandlerSuspended(DependencyProperty property, bool value)
  {
    if (value)
    {
      if (this._isHandlerSuspended == null)
        this._isHandlerSuspended = (IDictionary<DependencyProperty, bool>) new Dictionary<DependencyProperty, bool>(2);
      this._isHandlerSuspended[property] = true;
    }
    else
      this._isHandlerSuspended?.Remove(property);
  }

  private void SetValueNoCallback(DependencyProperty property, object value)
  {
    this.SetIsHandlerSuspended(property, true);
    try
    {
      this.SetCurrentValue(property, value);
    }
    finally
    {
      this.SetIsHandlerSuspended(property, false);
    }
  }

  private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.SetValueNoCallback(TimePicker.TextProperty, (object) this._textBox.Text);
  }

  private bool ProcessTimePickerKey(KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Return:
        this.SetSelectedTime();
        return true;
      case Key.System:
        if (e.SystemKey == Key.Down && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
        {
          this.TogglePopup();
          return true;
        }
        break;
    }
    return false;
  }

  private void TextBox_KeyDown(object sender, KeyEventArgs e)
  {
    e.Handled = this.ProcessTimePickerKey(e) || e.Handled;
  }

  private void DropDownButton_MouseLeave(object sender, MouseEventArgs e)
  {
    this._disablePopupReopen = false;
  }

  private bool IsHandlerSuspended(DependencyProperty property)
  {
    return this._isHandlerSuspended != null && this._isHandlerSuspended.ContainsKey(property);
  }

  private void PopupPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is Popup popup) || popup.StaysOpen || this._dropDownButton?.InputHitTest(e.GetPosition((IInputElement) this._dropDownButton)) == null)
      return;
    this._disablePopupReopen = true;
  }

  private void Clock_SelectedTimeChanged(object sender, FunctionEventArgs<DateTime?> e)
  {
    this.SelectedTime = e.Info;
  }

  private void Clock_Confirmed() => this.TogglePopup();

  private void PopupOpened(object sender, EventArgs e)
  {
    if (!this.IsDropDownOpen)
      this.SetCurrentValue(TimePicker.IsDropDownOpenProperty, ValueBoxes.TrueBox);
    this.Clock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    this.OnClockOpened(new RoutedEventArgs());
  }

  private void PopupClosed(object sender, EventArgs e)
  {
    if (this.IsDropDownOpen)
      this.SetCurrentValue(TimePicker.IsDropDownOpenProperty, ValueBoxes.FalseBox);
    if (this.Clock.IsKeyboardFocusWithin)
      this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    this.OnClockClosed(new RoutedEventArgs());
  }

  private void DropDownButton_Click(object sender, RoutedEventArgs e) => this.TogglePopup();

  private void TogglePopup()
  {
    if (this.IsDropDownOpen)
      this.SetCurrentValue(TimePicker.IsDropDownOpenProperty, ValueBoxes.FalseBox);
    else if (this._disablePopupReopen)
    {
      this._disablePopupReopen = false;
    }
    else
    {
      this.SetSelectedTime();
      this.SetCurrentValue(TimePicker.IsDropDownOpenProperty, ValueBoxes.TrueBox);
    }
  }

  private void SafeSetText(string s)
  {
    if (string.Compare(this.Text, s, StringComparison.Ordinal) == 0)
      return;
    this.SetCurrentValue(TimePicker.TextProperty, (object) s);
  }

  private DateTime? ParseText(string text)
  {
    try
    {
      return new DateTime?(DateTime.Parse(text));
    }
    catch
    {
    }
    return new DateTime?();
  }

  private DateTime? SetTextBoxValue(string s)
  {
    if (string.IsNullOrEmpty(s))
    {
      this.SafeSetText(s);
      return this.SelectedTime;
    }
    DateTime? text = this.ParseText(s);
    if (text.HasValue)
    {
      this.SafeSetText(this.DateTimeToString(text.Value));
      return text;
    }
    if (this.SelectedTime.HasValue)
    {
      this.SafeSetText(this.DateTimeToString(this.SelectedTime.Value));
      return this.SelectedTime;
    }
    this.SafeSetText(this.DateTimeToString(this.DisplayTime));
    return new DateTime?(this.DisplayTime);
  }

  private void SetSelectedTime()
  {
    if (this._textBox != null)
    {
      if (!string.IsNullOrEmpty(this._textBox.Text))
      {
        string text = this._textBox.Text;
        DateTime? selectedTime = this.SelectedTime;
        if (selectedTime.HasValue)
        {
          selectedTime = this.SelectedTime;
          if (string.Compare(this.DateTimeToString(selectedTime.Value), text, StringComparison.Ordinal) == 0)
            return;
        }
        DateTime? nullable = this.SetTextBoxValue(text);
        selectedTime = this.SelectedTime;
        if (selectedTime.Equals((object) nullable))
          return;
        this.SetCurrentValue(TimePicker.SelectedTimeProperty, (object) nullable);
        this.SetCurrentValue(TimePicker.DisplayTimeProperty, (object) nullable);
      }
      else
      {
        if (!this.SelectedTime.HasValue)
          return;
        this.SetCurrentValue(TimePicker.SelectedTimeProperty, (object) null);
      }
    }
    else
    {
      DateTime? nullable = this.SetTextBoxValue(this._defaultText);
      if (this.SelectedTime.Equals((object) nullable))
        return;
      this.SetCurrentValue(TimePicker.SelectedTimeProperty, (object) nullable);
    }
  }

  private string DateTimeToString(DateTime d) => d.ToString(this.TimeFormat);

  private static void OnGotFocus(object sender, RoutedEventArgs e)
  {
    TimePicker objB = (TimePicker) sender;
    if (e.Handled || objB._textBox == null)
      return;
    if (object.Equals(e.OriginalSource, (object) objB))
    {
      objB._textBox.Focus();
      e.Handled = true;
    }
    else
    {
      if (!object.Equals(e.OriginalSource, (object) objB._textBox))
        return;
      objB._textBox.SelectAll();
      e.Handled = true;
    }
  }

  private void EnsureClock()
  {
    if (this.Clock != null)
      return;
    this.SetCurrentValue(TimePicker.ClockProperty, (object) new HandyControl.Controls.Clock());
    this.SetPopupChild((UIElement) this.Clock);
  }

  private void SetPopupChild(UIElement element)
  {
    if (this._popup == null)
      return;
    this._popup.Child = (UIElement) this.Clock;
  }
}
