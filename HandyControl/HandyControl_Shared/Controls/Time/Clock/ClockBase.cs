// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ClockBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ButtonConfirm", Type = typeof (Button))]
public abstract class ClockBase : Control
{
  protected const string ElementButtonConfirm = "PART_ButtonConfirm";
  protected Button ButtonConfirm;
  protected bool AppliedTemplate;
  public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct, typeof (EventHandler<FunctionEventArgs<DateTime?>>), typeof (ClockBase));
  public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(nameof (TimeFormat), typeof (string), typeof (ClockBase), new PropertyMetadata((object) "HH:mm:ss"));
  public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(nameof (SelectedTime), typeof (DateTime?), typeof (ClockBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ClockBase.OnSelectedTimeChanged)));
  public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(nameof (DisplayTime), typeof (DateTime), typeof (ClockBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ClockBase.OnDisplayTimeChanged)));
  internal static readonly DependencyProperty ShowConfirmButtonProperty = DependencyProperty.Register(nameof (ShowConfirmButton), typeof (bool), typeof (ClockBase), new PropertyMetadata(ValueBoxes.FalseBox));

  public event Action Confirmed;

  public event EventHandler<FunctionEventArgs<DateTime>> DisplayTimeChanged;

  public event EventHandler<FunctionEventArgs<DateTime?>> SelectedTimeChanged
  {
    add => this.AddHandler(ClockBase.SelectedTimeChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(ClockBase.SelectedTimeChangedEvent, (Delegate) value);
  }

  public string TimeFormat
  {
    get => (string) this.GetValue(ClockBase.TimeFormatProperty);
    set => this.SetValue(ClockBase.TimeFormatProperty, (object) value);
  }

  private static void OnSelectedTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ClockBase source = (ClockBase) d;
    DateTime? newValue = (DateTime?) e.NewValue;
    source.DisplayTime = newValue ?? DateTime.Now;
    source.OnSelectedTimeChanged(new FunctionEventArgs<DateTime?>(ClockBase.SelectedTimeChangedEvent, (object) source)
    {
      Info = newValue
    });
  }

  public DateTime? SelectedTime
  {
    get => (DateTime?) this.GetValue(ClockBase.SelectedTimeProperty);
    set => this.SetValue(ClockBase.SelectedTimeProperty, (object) value);
  }

  private static void OnDisplayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ClockBase clockBase = (ClockBase) d;
    DateTime newValue = (DateTime) e.NewValue;
    clockBase.Update(newValue);
    clockBase.OnDisplayTimeChanged(new FunctionEventArgs<DateTime>(newValue));
  }

  public DateTime DisplayTime
  {
    get => (DateTime) this.GetValue(ClockBase.DisplayTimeProperty);
    set => this.SetValue(ClockBase.DisplayTimeProperty, (object) value);
  }

  internal bool ShowConfirmButton
  {
    get => (bool) this.GetValue(ClockBase.ShowConfirmButtonProperty);
    set => this.SetValue(ClockBase.ShowConfirmButtonProperty, ValueBoxes.BooleanBox(value));
  }

  protected virtual void OnSelectedTimeChanged(FunctionEventArgs<DateTime?> e)
  {
    this.RaiseEvent((RoutedEventArgs) e);
  }

  protected virtual void OnDisplayTimeChanged(FunctionEventArgs<DateTime> e)
  {
    EventHandler<FunctionEventArgs<DateTime>> displayTimeChanged = this.DisplayTimeChanged;
    if (displayTimeChanged == null)
      return;
    displayTimeChanged((object) this, e);
  }

  protected void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
  {
    this.SelectedTime = new DateTime?(this.DisplayTime);
    Action confirmed = this.Confirmed;
    if (confirmed == null)
      return;
    confirmed();
  }

  internal abstract void Update(DateTime time);

  protected void Clock_SelectedTimeChanged(object sender, FunctionEventArgs<DateTime?> e)
  {
    this.SelectedTime = e.Info;
  }

  public virtual void OnClockClosed()
  {
  }

  public virtual void OnClockOpened()
  {
  }
}
