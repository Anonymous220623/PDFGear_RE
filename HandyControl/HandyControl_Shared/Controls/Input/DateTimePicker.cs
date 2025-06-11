// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.DateTimePicker
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
public class DateTimePicker : Control
{
  private const string ElementRoot = "PART_Root";
  private const string ElementTextBox = "PART_TextBox";
  private const string ElementButton = "PART_Button";
  private const string ElementPopup = "PART_Popup";
  private CalendarWithClock _calendarWithClock;
  private string _defaultText;
  private ButtonBase _dropDownButton;
  private Popup _popup;
  private bool _disablePopupReopen;
  private WatermarkTextBox _textBox;
  private IDictionary<DependencyProperty, bool> _isHandlerSuspended;
  private DateTime? _originalSelectedDateTime;
  public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Direct, typeof (EventHandler<FunctionEventArgs<DateTime?>>), typeof (DateTimePicker));
  public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(nameof (DateTimeFormat), typeof (string), typeof (DateTimePicker), new PropertyMetadata((object) "yyyy-MM-dd HH:mm:ss"));
  public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register(nameof (CalendarStyle), typeof (Style), typeof (DateTimePicker), new PropertyMetadata((object) null));
  public static readonly DependencyProperty DisplayDateTimeProperty = DependencyProperty.Register(nameof (DisplayDateTime), typeof (DateTime), typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (PropertyChangedCallback) null, new CoerceValueCallback(DateTimePicker.CoerceDisplayDateTime)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimePicker.OnIsDropDownOpenChanged), new CoerceValueCallback(DateTimePicker.OnCoerceIsDropDownOpen)));
  public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(nameof (SelectedDateTime), typeof (DateTime?), typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimePicker.OnSelectedDateTimeChanged), new CoerceValueCallback(DateTimePicker.CoerceSelectedDateTime)));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(DateTimePicker.OnTextChanged)));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (DateTimePicker));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (DateTimePicker));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (DateTimePicker));

  public event EventHandler<FunctionEventArgs<DateTime?>> SelectedDateTimeChanged
  {
    add => this.AddHandler(DateTimePicker.SelectedDateTimeChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(DateTimePicker.SelectedDateTimeChangedEvent, (Delegate) value);
  }

  public event RoutedEventHandler PickerClosed;

  public event RoutedEventHandler PickerOpened;

  static DateTimePicker()
  {
    EventManager.RegisterClassHandler(typeof (DateTimePicker), UIElement.GotFocusEvent, (Delegate) new RoutedEventHandler(DateTimePicker.OnGotFocus));
    KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Once));
    KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof (DateTimePicker), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
  }

  public DateTimePicker()
  {
    this.InitCalendarWithClock();
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this.SetCurrentValue(DateTimePicker.SelectedDateTimeProperty, (object) null);
      this.SetCurrentValue(DateTimePicker.TextProperty, (object) "");
      this._textBox.Text = string.Empty;
    })));
  }

  public string DateTimeFormat
  {
    get => (string) this.GetValue(DateTimePicker.DateTimeFormatProperty);
    set => this.SetValue(DateTimePicker.DateTimeFormatProperty, (object) value);
  }

  public Style CalendarStyle
  {
    get => (Style) this.GetValue(DateTimePicker.CalendarStyleProperty);
    set => this.SetValue(DateTimePicker.CalendarStyleProperty, (object) value);
  }

  private static object CoerceDisplayDateTime(DependencyObject d, object value)
  {
    DateTimePicker dateTimePicker = (DateTimePicker) d;
    dateTimePicker._calendarWithClock.DisplayDateTime = (DateTime) value;
    return (object) dateTimePicker._calendarWithClock.DisplayDateTime;
  }

  public DateTime DisplayDateTime
  {
    get => (DateTime) this.GetValue(DateTimePicker.DisplayDateTimeProperty);
    set => this.SetValue(DateTimePicker.DisplayDateTimeProperty, (object) value);
  }

  private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
  {
    return !(d is DateTimePicker dateTimePicker) || dateTimePicker.IsEnabled ? baseValue : (object) false;
  }

  private static void OnIsDropDownOpenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DateTimePicker dp = d as DateTimePicker;
    bool newValue = (bool) e.NewValue;
    if (dp?._popup == null || dp._popup.IsOpen == newValue)
      return;
    dp._popup.IsOpen = newValue;
    if (!newValue)
      return;
    dp._originalSelectedDateTime = dp.SelectedDateTime;
    dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Delegate) (() => dp._calendarWithClock.Focus()));
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(DateTimePicker.IsDropDownOpenProperty);
    set => this.SetValue(DateTimePicker.IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
  }

  private static object CoerceSelectedDateTime(DependencyObject d, object value)
  {
    DateTimePicker dateTimePicker = (DateTimePicker) d;
    dateTimePicker._calendarWithClock.SelectedDateTime = (DateTime?) value;
    return (object) dateTimePicker._calendarWithClock.SelectedDateTime;
  }

  private static void OnSelectedDateTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimePicker source))
      return;
    if (source.SelectedDateTime.HasValue)
    {
      DateTime d1 = source.SelectedDateTime.Value;
      source.SetTextInternal(source.DateTimeToString(d1));
    }
    source.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<DateTime?>(DateTimePicker.SelectedDateTimeChangedEvent, (object) source)
    {
      Info = source.SelectedDateTime
    });
  }

  public DateTime? SelectedDateTime
  {
    get => (DateTime?) this.GetValue(DateTimePicker.SelectedDateTimeProperty);
    set => this.SetValue(DateTimePicker.SelectedDateTimeProperty, (object) value);
  }

  private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimePicker dateTimePicker) || dateTimePicker.IsHandlerSuspended(DateTimePicker.TextProperty))
      return;
    if (e.NewValue is string newValue)
    {
      if (dateTimePicker._textBox != null)
        dateTimePicker._textBox.Text = newValue;
      else
        dateTimePicker._defaultText = newValue;
      dateTimePicker.SetSelectedDateTime();
    }
    else
      dateTimePicker.SetValueNoCallback(DateTimePicker.SelectedDateTimeProperty, (object) null);
  }

  public string Text
  {
    get => (string) this.GetValue(DateTimePicker.TextProperty);
    set => this.SetValue(DateTimePicker.TextProperty, (object) value);
  }

  private void SetTextInternal(string value)
  {
    this.SetCurrentValue(DateTimePicker.TextProperty, (object) value);
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(DateTimePicker.SelectionBrushProperty);
    set => this.SetValue(DateTimePicker.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(DateTimePicker.SelectionOpacityProperty);
    set => this.SetValue(DateTimePicker.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(DateTimePicker.CaretBrushProperty);
    set => this.SetValue(DateTimePicker.CaretBrushProperty, (object) value);
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
    this._popup.Child = (UIElement) this._calendarWithClock;
    this._dropDownButton.Click += new RoutedEventHandler(this.DropDownButton_Click);
    this._dropDownButton.MouseLeave += new MouseEventHandler(this.DropDownButton_MouseLeave);
    DateTime? selectedDateTime = this.SelectedDateTime;
    if (this._textBox != null)
    {
      this._textBox.SetBinding(DateTimePicker.SelectionBrushProperty, (BindingBase) new Binding(DateTimePicker.SelectionBrushProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.SetBinding(DateTimePicker.SelectionOpacityProperty, (BindingBase) new Binding(DateTimePicker.SelectionOpacityProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.SetBinding(DateTimePicker.CaretBrushProperty, (BindingBase) new Binding(DateTimePicker.CaretBrushProperty.Name)
      {
        Source = (object) this
      });
      this._textBox.KeyDown += new KeyEventHandler(this.TextBox_KeyDown);
      this._textBox.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
      this._textBox.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
      if (!selectedDateTime.HasValue)
      {
        if (!string.IsNullOrEmpty(this._defaultText))
        {
          this._textBox.Text = this._defaultText;
          this.SetSelectedDateTime();
        }
      }
      else
        this._textBox.Text = this.DateTimeToString(selectedDateTime.Value);
    }
    if (!selectedDateTime.HasValue)
    {
      this._originalSelectedDateTime.GetValueOrDefault();
      if (!this._originalSelectedDateTime.HasValue)
        this._originalSelectedDateTime = new DateTime?(DateTime.Now);
      this.SetCurrentValue(DateTimePicker.DisplayDateTimeProperty, (object) this._originalSelectedDateTime);
    }
    else
      this.SetCurrentValue(DateTimePicker.DisplayDateTimeProperty, (object) selectedDateTime);
  }

  public override string ToString()
  {
    DateTime? selectedDateTime = this.SelectedDateTime;
    ref DateTime? local = ref selectedDateTime;
    return (local.HasValue ? local.GetValueOrDefault().ToString(this.DateTimeFormat) : (string) null) ?? string.Empty;
  }

  protected virtual void OnPickerClosed(RoutedEventArgs e)
  {
    RoutedEventHandler pickerClosed = this.PickerClosed;
    if (pickerClosed == null)
      return;
    pickerClosed((object) this, e);
  }

  protected virtual void OnPickerOpened(RoutedEventArgs e)
  {
    RoutedEventHandler pickerOpened = this.PickerOpened;
    if (pickerOpened == null)
      return;
    pickerOpened((object) this, e);
  }

  private void CheckNull()
  {
    if (this._dropDownButton == null || this._popup == null || this._textBox == null)
      throw new Exception();
  }

  private void InitCalendarWithClock()
  {
    this._calendarWithClock = new CalendarWithClock()
    {
      ShowConfirmButton = true
    };
    this._calendarWithClock.SelectedDateTimeChanged += new EventHandler<FunctionEventArgs<DateTime?>>(this.CalendarWithClock_SelectedDateTimeChanged);
    this._calendarWithClock.Confirmed += new Action(this.CalendarWithClock_Confirmed);
  }

  private void CalendarWithClock_Confirmed() => this.TogglePopup();

  private void CalendarWithClock_SelectedDateTimeChanged(
    object sender,
    FunctionEventArgs<DateTime?> e)
  {
    this.SelectedDateTime = e.Info;
  }

  private void TextBox_LostFocus(object sender, RoutedEventArgs e) => this.SetSelectedDateTime();

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
    this.SetValueNoCallback(DateTimePicker.TextProperty, (object) this._textBox.Text);
  }

  private bool ProcessDateTimePickerKey(KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Return:
        this.SetSelectedDateTime();
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
    e.Handled = this.ProcessDateTimePickerKey(e) || e.Handled;
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

  private void PopupOpened(object sender, EventArgs e)
  {
    if (!this.IsDropDownOpen)
      this.SetCurrentValue(DateTimePicker.IsDropDownOpenProperty, ValueBoxes.TrueBox);
    this._calendarWithClock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    this.OnPickerOpened(new RoutedEventArgs());
  }

  private void PopupClosed(object sender, EventArgs e)
  {
    if (this.IsDropDownOpen)
      this.SetCurrentValue(DateTimePicker.IsDropDownOpenProperty, ValueBoxes.FalseBox);
    if (this._calendarWithClock.IsKeyboardFocusWithin)
      this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    this.OnPickerClosed(new RoutedEventArgs());
  }

  private void DropDownButton_Click(object sender, RoutedEventArgs e) => this.TogglePopup();

  private void TogglePopup()
  {
    if (this.IsDropDownOpen)
      this.SetCurrentValue(DateTimePicker.IsDropDownOpenProperty, ValueBoxes.FalseBox);
    else if (this._disablePopupReopen)
    {
      this._disablePopupReopen = false;
    }
    else
    {
      this.SetSelectedDateTime();
      this.SetCurrentValue(DateTimePicker.IsDropDownOpenProperty, ValueBoxes.TrueBox);
    }
  }

  private void SafeSetText(string s)
  {
    if (string.Compare(this.Text, s, StringComparison.Ordinal) == 0)
      return;
    this.SetCurrentValue(DateTimePicker.TextProperty, (object) s);
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
      return this.SelectedDateTime;
    }
    DateTime? text = this.ParseText(s);
    if (text.HasValue)
    {
      this.SafeSetText(this.DateTimeToString(text.Value));
      return text;
    }
    if (this.SelectedDateTime.HasValue)
    {
      this.SafeSetText(this.DateTimeToString(this.SelectedDateTime.Value));
      return this.SelectedDateTime;
    }
    this.SafeSetText(this.DateTimeToString(this.DisplayDateTime));
    return new DateTime?(this.DisplayDateTime);
  }

  private void SetSelectedDateTime()
  {
    if (this._textBox != null)
    {
      if (!string.IsNullOrEmpty(this._textBox.Text))
      {
        string text = this._textBox.Text;
        DateTime? selectedDateTime = this.SelectedDateTime;
        if (selectedDateTime.HasValue)
        {
          selectedDateTime = this.SelectedDateTime;
          DateTime displayDateTime = this.DisplayDateTime;
          if ((selectedDateTime.HasValue ? (selectedDateTime.HasValue ? (selectedDateTime.GetValueOrDefault() != displayDateTime ? 1 : 0) : 0) : 1) != 0)
            this.SetCurrentValue(DateTimePicker.DisplayDateTimeProperty, (object) this.SelectedDateTime);
          selectedDateTime = this.SelectedDateTime;
          if (string.Compare(this.DateTimeToString(selectedDateTime.Value), text, StringComparison.Ordinal) == 0)
            return;
        }
        DateTime? nullable = this.SetTextBoxValue(text);
        selectedDateTime = this.SelectedDateTime;
        if (selectedDateTime.Equals((object) nullable))
          return;
        this.SetCurrentValue(DateTimePicker.SelectedDateTimeProperty, (object) nullable);
        this.SetCurrentValue(DateTimePicker.DisplayDateTimeProperty, (object) nullable);
      }
      else
      {
        if (!this.SelectedDateTime.HasValue)
          return;
        this.SetCurrentValue(DateTimePicker.SelectedDateTimeProperty, (object) null);
      }
    }
    else
    {
      DateTime? nullable = this.SetTextBoxValue(this._defaultText);
      if (this.SelectedDateTime.Equals((object) nullable))
        return;
      this.SetCurrentValue(DateTimePicker.SelectedDateTimeProperty, (object) nullable);
    }
  }

  private string DateTimeToString(DateTime d) => d.ToString(this.DateTimeFormat);

  private static void OnGotFocus(object sender, RoutedEventArgs e)
  {
    DateTimePicker objB = (DateTimePicker) sender;
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
}
