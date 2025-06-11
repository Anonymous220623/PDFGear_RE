// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Clock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ButtonAm", Type = typeof (RadioButton))]
[TemplatePart(Name = "PART_ButtonPm", Type = typeof (RadioButton))]
[TemplatePart(Name = "PART_Canvas", Type = typeof (Canvas))]
[TemplatePart(Name = "PART_BorderTitle", Type = typeof (Border))]
[TemplatePart(Name = "PART_BorderClock", Type = typeof (Border))]
[TemplatePart(Name = "PART_PanelNum", Type = typeof (CirclePanel))]
[TemplatePart(Name = "PART_TimeStr", Type = typeof (TextBlock))]
public class Clock : ClockBase
{
  private const string ElementButtonAm = "PART_ButtonAm";
  private const string ElementButtonPm = "PART_ButtonPm";
  private const string ElementCanvas = "PART_Canvas";
  private const string ElementBorderTitle = "PART_BorderTitle";
  private const string ElementBorderClock = "PART_BorderClock";
  private const string ElementPanelNum = "PART_PanelNum";
  private const string ElementTimeStr = "PART_TimeStr";
  private RadioButton _buttonAm;
  private RadioButton _buttonPm;
  private Canvas _canvas;
  private Border _borderTitle;
  private Border _borderClock;
  private ClockRadioButton _currentButton;
  private RotateTransform _rotateTransformClock;
  private CirclePanel _circlePanel;
  private List<ClockRadioButton> _radioButtonList;
  private TextBlock _blockTime;
  private int _secValue;
  public static readonly DependencyProperty ClockRadioButtonStyleProperty = DependencyProperty.Register(nameof (ClockRadioButtonStyle), typeof (Style), typeof (Clock), new PropertyMetadata((object) null));

  public Style ClockRadioButtonStyle
  {
    get => (Style) this.GetValue(Clock.ClockRadioButtonStyleProperty);
    set => this.SetValue(Clock.ClockRadioButtonStyleProperty, (object) value);
  }

  private int SecValue
  {
    get => this._secValue;
    set
    {
      if (value < 0)
        this._secValue = 59;
      else if (value > 59)
        this._secValue = 0;
      else
        this._secValue = value;
    }
  }

  public override void OnApplyTemplate()
  {
    this.AppliedTemplate = false;
    if (this._buttonAm != null)
      this._buttonAm.Click -= new RoutedEventHandler(this.ButtonAm_OnClick);
    if (this._buttonPm != null)
      this._buttonPm.Click -= new RoutedEventHandler(this.ButtonPm_OnClick);
    if (this.ButtonConfirm != null)
      this.ButtonConfirm.Click -= new RoutedEventHandler(((ClockBase) this).ButtonConfirm_OnClick);
    if (this._borderTitle != null)
      this._borderTitle.MouseWheel -= new MouseWheelEventHandler(this.BorderTitle_OnMouseWheel);
    if (this._canvas != null)
    {
      this._canvas.MouseWheel -= new MouseWheelEventHandler(this.Canvas_OnMouseWheel);
      this._canvas.RemoveHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.Canvas_OnClick));
      this._canvas.MouseMove -= new MouseEventHandler(this.Canvas_OnMouseMove);
    }
    base.OnApplyTemplate();
    this._buttonAm = this.GetTemplateChild("PART_ButtonAm") as RadioButton;
    this._buttonPm = this.GetTemplateChild("PART_ButtonPm") as RadioButton;
    this.ButtonConfirm = this.GetTemplateChild("PART_ButtonConfirm") as Button;
    this._borderTitle = this.GetTemplateChild("PART_BorderTitle") as Border;
    this._canvas = this.GetTemplateChild("PART_Canvas") as Canvas;
    this._borderClock = this.GetTemplateChild("PART_BorderClock") as Border;
    this._circlePanel = this.GetTemplateChild("PART_PanelNum") as CirclePanel;
    this._blockTime = this.GetTemplateChild("PART_TimeStr") as TextBlock;
    if (!this.CheckNull())
      return;
    this._buttonAm.Click += new RoutedEventHandler(this.ButtonAm_OnClick);
    this._buttonPm.Click += new RoutedEventHandler(this.ButtonPm_OnClick);
    this.ButtonConfirm.Click += new RoutedEventHandler(((ClockBase) this).ButtonConfirm_OnClick);
    this._borderTitle.MouseWheel += new MouseWheelEventHandler(this.BorderTitle_OnMouseWheel);
    this._canvas.MouseWheel += new MouseWheelEventHandler(this.Canvas_OnMouseWheel);
    this._canvas.AddHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.Canvas_OnClick));
    this._canvas.MouseMove += new MouseEventHandler(this.Canvas_OnMouseMove);
    this._rotateTransformClock = new RotateTransform();
    this._borderClock.RenderTransform = (Transform) this._rotateTransformClock;
    this._radioButtonList = new List<ClockRadioButton>();
    for (int index = 0; index < 12; ++index)
    {
      int num = index + 1;
      ClockRadioButton clockRadioButton = new ClockRadioButton();
      clockRadioButton.Num = num;
      clockRadioButton.Content = (object) num;
      ClockRadioButton element = clockRadioButton;
      element.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding(Clock.ClockRadioButtonStyleProperty.Name)
      {
        Source = (object) this
      });
      this._radioButtonList.Add(element);
      this._circlePanel.Children.Add((UIElement) element);
    }
    this.AppliedTemplate = true;
    DateTime? selectedTime = this.SelectedTime;
    if (selectedTime.HasValue)
    {
      selectedTime = this.SelectedTime;
      this.Update(selectedTime.Value);
    }
    else
    {
      this.DisplayTime = DateTime.Now;
      this.Update(this.DisplayTime);
    }
  }

  private bool CheckNull()
  {
    return this._buttonPm != null && this._buttonAm != null && this.ButtonConfirm != null && this._canvas != null && this._borderTitle != null && this._borderClock != null && this._circlePanel != null && this._blockTime != null;
  }

  private void BorderTitle_OnMouseWheel(object sender, MouseWheelEventArgs e)
  {
    if (e.Delta < 0)
    {
      --this.SecValue;
      this.Update();
    }
    else
    {
      ++this.SecValue;
      this.Update();
    }
    e.Handled = true;
  }

  private void Canvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
  {
    int angle = (int) this._rotateTransformClock.Angle;
    int num = e.Delta >= 0 ? angle - 6 : angle + 6;
    if (num < 0)
      num += 360;
    this._rotateTransformClock.Angle = (double) num;
    this.Update();
    e.Handled = true;
  }

  private void Canvas_OnClick(object sender, RoutedEventArgs e)
  {
    this._currentButton = e.OriginalSource as ClockRadioButton;
    if (this._currentButton == null)
      return;
    this.Update();
  }

  private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    double num = ArithmeticHelper.CalAngle(new Point(85.0, 85.0), e.GetPosition((IInputElement) this._canvas)) + 90.0;
    if (num < 0.0)
      num += 360.0;
    this._rotateTransformClock.Angle = num - num % 6.0;
    this.Update();
  }

  private void Update()
  {
    if (!this.AppliedTemplate)
      return;
    int num = this._currentButton.Num;
    bool? isChecked = this._buttonPm.IsChecked;
    bool flag1 = true;
    if (isChecked.GetValueOrDefault() == flag1 & isChecked.HasValue)
    {
      num += 12;
      if (num == 24)
        num = 12;
    }
    else if (num == 12)
      num = 0;
    if (num == 12)
    {
      isChecked = this._buttonAm.IsChecked;
      bool flag2 = true;
      if (isChecked.GetValueOrDefault() == flag2 & isChecked.HasValue)
      {
        this._buttonPm.IsChecked = new bool?(true);
        this._buttonAm.IsChecked = new bool?(false);
      }
    }
    if (this._blockTime == null)
      return;
    this.DisplayTime = this.GetDisplayTime();
    this._blockTime.Text = this.DisplayTime.ToString(this.TimeFormat);
  }

  internal override void Update(DateTime time)
  {
    if (!this.AppliedTemplate)
      return;
    int hour = time.Hour;
    int minute = time.Minute;
    if (hour >= 12)
    {
      this._buttonPm.IsChecked = new bool?(true);
      this._buttonAm.IsChecked = new bool?(false);
    }
    else
    {
      this._buttonPm.IsChecked = new bool?(false);
      this._buttonAm.IsChecked = new bool?(true);
    }
    this._rotateTransformClock.Angle = (double) (minute * 6);
    int num = hour % 12;
    if (num == 0)
      num = 12;
    ClockRadioButton radioButton = this._radioButtonList[num - 1];
    radioButton.IsChecked = new bool?(true);
    radioButton.RaiseEvent(new RoutedEventArgs()
    {
      RoutedEvent = ButtonBase.ClickEvent
    });
    this._secValue = time.Second;
    this.Update();
  }

  private DateTime GetDisplayTime()
  {
    int hour = this._currentButton.Num;
    bool? isChecked = this._buttonPm.IsChecked;
    bool flag = true;
    if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
    {
      hour += 12;
      if (hour == 24)
        hour = 12;
    }
    else if (hour == 12)
      hour = 0;
    DateTime now = DateTime.Now;
    return new DateTime(now.Year, now.Month, now.Day, hour, (int) Math.Abs(this._rotateTransformClock.Angle) % 360 / 6, this._secValue);
  }

  private void ButtonAm_OnClick(object sender, RoutedEventArgs e) => this.Update();

  private void ButtonPm_OnClick(object sender, RoutedEventArgs e) => this.Update();
}
