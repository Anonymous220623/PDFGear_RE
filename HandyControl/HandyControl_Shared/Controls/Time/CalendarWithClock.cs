// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CalendarWithClock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ButtonConfirm", Type = typeof (Button))]
[TemplatePart(Name = "PART_ClockPresenter", Type = typeof (ContentPresenter))]
[TemplatePart(Name = "PART_CalendarPresenter", Type = typeof (ContentPresenter))]
public class CalendarWithClock : Control
{
  private const string ElementButtonConfirm = "PART_ButtonConfirm";
  private const string ElementClockPresenter = "PART_ClockPresenter";
  private const string ElementCalendarPresenter = "PART_CalendarPresenter";
  private ContentPresenter _clockPresenter;
  private ContentPresenter _calendarPresenter;
  private Clock _clock;
  private Calendar _calendar;
  private Button _buttonConfirm;
  private bool _isLoaded;
  private IDictionary<DependencyProperty, bool> _isHandlerSuspended;
  public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Direct, typeof (EventHandler<FunctionEventArgs<DateTime?>>), typeof (CalendarWithClock));
  public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(nameof (DateTimeFormat), typeof (string), typeof (CalendarWithClock), new PropertyMetadata((object) "yyyy-MM-dd HH:mm:ss"));
  public static readonly DependencyProperty ShowConfirmButtonProperty = DependencyProperty.Register(nameof (ShowConfirmButton), typeof (bool), typeof (CalendarWithClock), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(nameof (SelectedDateTime), typeof (DateTime?), typeof (CalendarWithClock), new PropertyMetadata((object) null, new PropertyChangedCallback(CalendarWithClock.OnSelectedDateTimeChanged)));
  public static readonly DependencyProperty DisplayDateTimeProperty = DependencyProperty.Register(nameof (DisplayDateTime), typeof (DateTime), typeof (CalendarWithClock), (PropertyMetadata) new FrameworkPropertyMetadata((object) DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(CalendarWithClock.OnDisplayDateTimeChanged)));

  public event EventHandler<FunctionEventArgs<DateTime?>> SelectedDateTimeChanged
  {
    add => this.AddHandler(CalendarWithClock.SelectedDateTimeChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(CalendarWithClock.SelectedDateTimeChangedEvent, (Delegate) value);
  }

  public event EventHandler<FunctionEventArgs<DateTime>> DisplayDateTimeChanged;

  public event Action Confirmed;

  public CalendarWithClock()
  {
    this.InitCalendarAndClock();
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._isLoaded)
        return;
      this._isLoaded = true;
      this.DisplayDateTime = this.SelectedDateTime ?? DateTime.Now;
    });
  }

  public string DateTimeFormat
  {
    get => (string) this.GetValue(CalendarWithClock.DateTimeFormatProperty);
    set => this.SetValue(CalendarWithClock.DateTimeFormatProperty, (object) value);
  }

  public bool ShowConfirmButton
  {
    get => (bool) this.GetValue(CalendarWithClock.ShowConfirmButtonProperty);
    set => this.SetValue(CalendarWithClock.ShowConfirmButtonProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnSelectedDateTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarWithClock source = (CalendarWithClock) d;
    DateTime? newValue = (DateTime?) e.NewValue;
    source.OnSelectedDateTimeChanged(new FunctionEventArgs<DateTime?>(CalendarWithClock.SelectedDateTimeChangedEvent, (object) source)
    {
      Info = newValue
    });
  }

  public DateTime? SelectedDateTime
  {
    get => (DateTime?) this.GetValue(CalendarWithClock.SelectedDateTimeProperty);
    set => this.SetValue(CalendarWithClock.SelectedDateTimeProperty, (object) value);
  }

  private static void OnDisplayDateTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarWithClock calendarWithClock = (CalendarWithClock) d;
    if (calendarWithClock.IsHandlerSuspended(CalendarWithClock.DisplayDateTimeProperty))
      return;
    DateTime newValue = (DateTime) e.NewValue;
    calendarWithClock._clock.SelectedTime = new DateTime?(newValue);
    calendarWithClock._calendar.SelectedDate = new DateTime?(newValue);
    calendarWithClock._calendar.DisplayDate = newValue;
    calendarWithClock.OnDisplayDateTimeChanged(new FunctionEventArgs<DateTime>(newValue));
  }

  public DateTime DisplayDateTime
  {
    get => (DateTime) this.GetValue(CalendarWithClock.DisplayDateTimeProperty);
    set => this.SetValue(CalendarWithClock.DisplayDateTimeProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    if (this._buttonConfirm != null)
      this._buttonConfirm.Click -= new RoutedEventHandler(this.ButtonConfirm_OnClick);
    base.OnApplyTemplate();
    this._buttonConfirm = this.GetTemplateChild("PART_ButtonConfirm") as Button;
    this._clockPresenter = this.GetTemplateChild("PART_ClockPresenter") as ContentPresenter;
    this._calendarPresenter = this.GetTemplateChild("PART_CalendarPresenter") as ContentPresenter;
    this.CheckNull();
    this._clockPresenter.Content = (object) this._clock;
    this._calendarPresenter.Content = (object) this._calendar;
    this._buttonConfirm.Click += new RoutedEventHandler(this.ButtonConfirm_OnClick);
  }

  protected virtual void OnSelectedDateTimeChanged(FunctionEventArgs<DateTime?> e)
  {
    this.RaiseEvent((RoutedEventArgs) e);
  }

  protected virtual void OnDisplayDateTimeChanged(FunctionEventArgs<DateTime> e)
  {
    EventHandler<FunctionEventArgs<DateTime>> displayDateTimeChanged = this.DisplayDateTimeChanged;
    if (displayDateTimeChanged == null)
      return;
    displayDateTimeChanged((object) this, e);
  }

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

  private bool IsHandlerSuspended(DependencyProperty property)
  {
    return this._isHandlerSuspended != null && this._isHandlerSuspended.ContainsKey(property);
  }

  private void CheckNull()
  {
    if (this._buttonConfirm == null || this._clockPresenter == null || this._calendarPresenter == null)
      throw new Exception();
  }

  private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
  {
    this.SelectedDateTime = new DateTime?(this.DisplayDateTime);
    Action confirmed = this.Confirmed;
    if (confirmed == null)
      return;
    confirmed();
  }

  private void InitCalendarAndClock()
  {
    Clock clock = new Clock();
    clock.BorderThickness = new Thickness();
    clock.Background = (Brush) Brushes.Transparent;
    this._clock = clock;
    TitleElement.SetBackground((DependencyObject) this._clock, (Brush) Brushes.Transparent);
    this._clock.DisplayTimeChanged += new EventHandler<FunctionEventArgs<DateTime>>(this.Clock_DisplayTimeChanged);
    Calendar calendar = new Calendar();
    calendar.BorderThickness = new Thickness();
    calendar.Background = (Brush) Brushes.Transparent;
    calendar.Focusable = false;
    this._calendar = calendar;
    TitleElement.SetBackground((DependencyObject) this._calendar, (Brush) Brushes.Transparent);
    this._calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(this.Calendar_SelectedDatesChanged);
  }

  private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
  {
    Mouse.Capture((IInputElement) null);
    this.UpdateDisplayTime();
  }

  private void Clock_DisplayTimeChanged(object sender, FunctionEventArgs<DateTime> e)
  {
    this.UpdateDisplayTime();
  }

  private void UpdateDisplayTime()
  {
    if (!this._calendar.SelectedDate.HasValue)
      return;
    DateTime dateTime1 = this._calendar.SelectedDate.Value;
    DateTime displayTime = this._clock.DisplayTime;
    DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, displayTime.Hour, displayTime.Minute, displayTime.Second);
    this.SetValueNoCallback(CalendarWithClock.DisplayDateTimeProperty, (object) dateTime2);
  }
}
