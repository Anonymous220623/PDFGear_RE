// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.NumberBox
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Controls;

public partial class NumberBox : Slider
{
  private TextBox _PART_TextBox;
  private ButtonBase _DownButton;
  private ButtonBase _UpButton;
  private bool innerSet;
  public static readonly DependencyProperty IsArrowEnabledProperty = DependencyProperty.Register(nameof (IsArrowEnabled), typeof (bool), typeof (NumberBox), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is NumberBox numberBox2))
      return;
    numberBox2.UpdateArrowState();
  })));
  public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.Register(nameof (NumberFormat), typeof (string), typeof (NumberBox), new PropertyMetadata((object) null, new PropertyChangedCallback(NumberBox.OnNumberFormatPropertyChanged)));
  public static readonly DependencyProperty SelectAllOnFocusedProperty = DependencyProperty.Register(nameof (SelectAllOnFocused), typeof (bool), typeof (NumberBox), new PropertyMetadata((object) false));
  public static readonly DependencyProperty FallbackToValidValueOnErrorProperty = DependencyProperty.Register(nameof (FallbackToValidValueOnError), typeof (bool), typeof (NumberBox), new PropertyMetadata((object) true));
  public static readonly DependencyProperty FallbackToValidValueOnFocusedProperty = DependencyProperty.Register(nameof (FallbackToValidValueOnFocused), typeof (bool), typeof (NumberBox), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is NumberBox numberBox4) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is bool newValue2) || !newValue2 || !numberBox4.FallbackToValidValueOnError || numberBox4._PART_TextBox == null || !numberBox4._PART_TextBox.IsFocused || string.IsNullOrEmpty(numberBox4._PART_TextBox.Text))
      return;
    numberBox4.SetValueFromText();
  })));
  public static readonly DependencyProperty IsValidProperty;
  private static readonly DependencyPropertyKey IsValidPropertyKey;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (NumberBox), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is NumberBox numberBox6) || object.Equals(a.NewValue, a.OldValue))
      return;
    if (a.NewValue == null)
      throw new ArgumentException((string) null, nameof (Text));
    if (numberBox6.innerSet)
      return;
    numberBox6.OnTextChanged();
  })));

  static NumberBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NumberBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NumberBox)));
    NumberBox.IsValidPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsValid), typeof (bool), typeof (NumberBox), new PropertyMetadata((object) true));
    NumberBox.IsValidProperty = NumberBox.IsValidPropertyKey.DependencyProperty;
  }

  private TextBox PART_TextBox
  {
    get => this._PART_TextBox;
    set
    {
      if (this._PART_TextBox == value)
        return;
      if (this._PART_TextBox != null)
      {
        this._PART_TextBox.PreviewMouseDown -= new MouseButtonEventHandler(this._PART_TextBox_PreviewMouseDown);
        this._PART_TextBox.PreviewKeyDown -= new KeyEventHandler(this._PART_TextBox_PreviewKeyDown);
        this._PART_TextBox.TextChanged -= new TextChangedEventHandler(this._PART_TextBox_TextChanged);
        this._PART_TextBox.GotFocus -= new RoutedEventHandler(this._PART_TextBox_GotFocus);
        this._PART_TextBox.LostFocus -= new RoutedEventHandler(this._PART_TextBox_LostFocus);
      }
      this._PART_TextBox = value;
      if (this._PART_TextBox != null)
      {
        this._PART_TextBox.PreviewMouseDown += new MouseButtonEventHandler(this._PART_TextBox_PreviewMouseDown);
        this._PART_TextBox.PreviewKeyDown += new KeyEventHandler(this._PART_TextBox_PreviewKeyDown);
        this._PART_TextBox.TextChanged += new TextChangedEventHandler(this._PART_TextBox_TextChanged);
        this._PART_TextBox.GotFocus += new RoutedEventHandler(this._PART_TextBox_GotFocus);
        this._PART_TextBox.LostFocus += new RoutedEventHandler(this._PART_TextBox_LostFocus);
      }
      this.SyncTextValue();
    }
  }

  private ButtonBase DownButton
  {
    get => this._DownButton;
    set => this._DownButton = value;
  }

  private ButtonBase UpButton
  {
    get => this._UpButton;
    set => this._UpButton = value;
  }

  public bool IsArrowEnabled
  {
    get => (bool) this.GetValue(NumberBox.IsArrowEnabledProperty);
    set => this.SetValue(NumberBox.IsArrowEnabledProperty, (object) value);
  }

  public string NumberFormat
  {
    get => (string) this.GetValue(NumberBox.NumberFormatProperty);
    set => this.SetValue(NumberBox.NumberFormatProperty, (object) value);
  }

  private static void OnNumberFormatPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is NumberBox numberBox))
      return;
    numberBox.SetValueFromText();
  }

  public bool SelectAllOnFocused
  {
    get => (bool) this.GetValue(NumberBox.SelectAllOnFocusedProperty);
    set => this.SetValue(NumberBox.SelectAllOnFocusedProperty, (object) value);
  }

  public bool FallbackToValidValueOnError
  {
    get => (bool) this.GetValue(NumberBox.FallbackToValidValueOnErrorProperty);
    set => this.SetValue(NumberBox.FallbackToValidValueOnErrorProperty, (object) value);
  }

  public bool FallbackToValidValueOnFocused
  {
    get => (bool) this.GetValue(NumberBox.FallbackToValidValueOnFocusedProperty);
    set => this.SetValue(NumberBox.FallbackToValidValueOnFocusedProperty, (object) value);
  }

  public bool IsValid
  {
    get => (bool) this.GetValue(NumberBox.IsValidProperty);
    private set => this.SetValue(NumberBox.IsValidPropertyKey, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(NumberBox.TextProperty);
    set => this.SetValue(NumberBox.TextProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PART_TextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
    this.UpButton = this.GetTemplateChild("UpButton") as ButtonBase;
    this.DownButton = this.GetTemplateChild("DownButton") as ButtonBase;
    this.UpdateArrowState();
    this.UpdateArrowEnabled();
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (this._PART_TextBox == null)
      return;
    this._PART_TextBox.Focus();
    this.Focusable = false;
  }

  protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    base.OnPreviewLostKeyboardFocus(e);
    this.Focusable = true;
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (!this.IsKeyboardFocusWithin)
      return;
    bool flag = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
    if (e.Delta > 0)
    {
      if (flag)
        ExecuteCommand(Slider.IncreaseLarge);
      else
        ExecuteCommand(Slider.IncreaseSmall);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      if (flag)
        ExecuteCommand(Slider.DecreaseLarge);
      else
        ExecuteCommand(Slider.DecreaseSmall);
    }

    void ExecuteCommand(RoutedCommand _command)
    {
      if (_command == null || !_command.CanExecute((object) this.Value, (IInputElement) this))
        return;
      _command.Execute((object) this.Value, (IInputElement) this);
      e.Handled = true;
    }
  }

  protected override void OnDecreaseSmall()
  {
    base.OnDecreaseSmall();
    this._PART_TextBox?.SelectAll();
  }

  protected override void OnIncreaseSmall()
  {
    base.OnIncreaseSmall();
    this._PART_TextBox?.SelectAll();
  }

  protected override void OnDecreaseLarge()
  {
    base.OnDecreaseLarge();
    this._PART_TextBox?.SelectAll();
  }

  protected override void OnIncreaseLarge()
  {
    base.OnIncreaseLarge();
    this._PART_TextBox?.SelectAll();
  }

  private void _PART_TextBox_GotFocus(object sender, RoutedEventArgs e)
  {
    if (!this.SelectAllOnFocused)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => this._PART_TextBox?.SelectAll()));
  }

  private void _PART_TextBox_LostFocus(object sender, RoutedEventArgs e) => this.SetValueFromText();

  private void _PART_TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.PART_TextBox == null || this.PART_TextBox.IsFocused)
      return;
    this.PART_TextBox.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => this.PART_TextBox.SelectAll()));
  }

  private void _PART_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Return || e.Key == Key.Escape)
    {
      e.Handled = true;
      this.SetValueFromText();
      if (e.Key != Key.Escape)
        return;
      Keyboard.ClearFocus();
    }
    else if (e.Key == Key.Home || e.Key == Key.End)
    {
      if (e.Key == Key.Home)
      {
        Slider.MinimizeValue.Execute((object) this.Value, (IInputElement) this);
      }
      else
      {
        if (e.Key != Key.End)
          return;
        Slider.MaximizeValue.Execute((object) this.Value, (IInputElement) this);
      }
    }
    else
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (e.Key == Key.Up || e.Key == Key.Down)
      {
        flag3 = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
        flag1 = e.Key == Key.Up;
        flag2 = e.Key == Key.Down;
      }
      else if (e.Key == Key.Prior || e.Key == Key.Next)
      {
        flag3 = true;
        flag1 = e.Key == Key.Prior;
        flag2 = e.Key == Key.Next;
      }
      if (flag3)
      {
        if (flag1)
        {
          Slider.IncreaseLarge.Execute((object) this.Value, (IInputElement) this);
        }
        else
        {
          if (!flag2)
            return;
          Slider.DecreaseLarge.Execute((object) this.Value, (IInputElement) this);
        }
      }
      else if (flag1)
      {
        Slider.IncreaseSmall.Execute((object) this.Value, (IInputElement) this);
      }
      else
      {
        if (!flag2)
          return;
        Slider.DecreaseSmall.Execute((object) this.Value, (IInputElement) this);
      }
    }
  }

  private void _PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.Text = ((TextBox) sender).Text;
  }

  private void UpButton_Click(object sender, RoutedEventArgs e)
  {
    Slider.IncreaseSmall.Execute((object) this.Value, (IInputElement) this);
  }

  private void DownButton_Click(object sender, RoutedEventArgs e)
  {
    Slider.DecreaseSmall.Execute((object) this.Value, (IInputElement) this);
  }

  private bool SetValueFromText()
  {
    if (this.PART_TextBox == null)
      return false;
    double num;
    if (this.ValidCore(this.FallbackToValidValueOnError, out num))
    {
      this.Value = num;
      this.IsValid = true;
      this.SyncTextValue();
      return true;
    }
    if (!this.FallbackToValidValueOnError)
      this.IsValid = false;
    return false;
  }

  private bool ValidCore(bool fallback, out double value)
  {
    string text = this.PART_TextBox.Text;
    if (double.TryParse(text, out value))
    {
      if (!string.IsNullOrEmpty(this.NumberFormat) && !double.TryParse(string.Format(this.NumberFormat, (object) value), out value))
        throw new ArgumentException("NumberFormat");
      if (value >= this.Minimum && value <= this.Maximum)
        return true;
      value = 0.0;
      if (fallback)
        this.SyncTextValue();
      EventHandler<NumberBoxInputValueInvalidEventArgs> inputValueInvalid = this.InputValueInvalid;
      if (inputValueInvalid != null)
        inputValueInvalid((object) this, new NumberBoxInputValueInvalidEventArgs(text));
    }
    else
    {
      if (fallback)
        this.SyncTextValue();
      EventHandler<NumberBoxInputValueInvalidEventArgs> inputValueInvalid = this.InputValueInvalid;
      if (inputValueInvalid != null)
        inputValueInvalid((object) this, new NumberBoxInputValueInvalidEventArgs(text));
    }
    value = 0.0;
    return false;
  }

  private void SyncTextValue()
  {
    this.innerSet = true;
    try
    {
      double num1 = Math.Min(this.Maximum, Math.Max(this.Minimum, this.Value));
      TextBox partTextBox = this.PART_TextBox;
      int num2 = partTextBox != null ? partTextBox.SelectionStart : -1;
      string text = this.PART_TextBox?.Text;
      string str = !string.IsNullOrEmpty(this.NumberFormat) ? string.Format(this.NumberFormat, (object) num1) : $"{num1:G0}";
      if (this.Text != str)
        this.Text = str;
      if (this.PART_TextBox != null)
      {
        if (this.PART_TextBox.Text != str)
          this.PART_TextBox.Text = str;
        if (text != str)
        {
          if (num2 > str.Length)
            num2 = str.Length;
          if (num2 != 0)
            this.PART_TextBox.Select(this.PART_TextBox.Text.Length, 0);
        }
      }
      this.UpdateArrowEnabled();
    }
    finally
    {
      this.innerSet = false;
    }
  }

  private void OnTextChanged()
  {
    if (this.PART_TextBox != null)
    {
      this.PART_TextBox.Text = this.Text;
      if (this.PART_TextBox.IsFocused && (!this.FallbackToValidValueOnError || !this.FallbackToValidValueOnFocused || string.IsNullOrEmpty(this.Text)))
        return;
    }
    this.SetValueFromText();
  }

  protected override void OnValueChanged(double oldValue, double newValue)
  {
    base.OnValueChanged(oldValue, newValue);
    if (!string.IsNullOrEmpty(this.NumberFormat))
    {
      double result;
      if (!double.TryParse(string.Format(this.NumberFormat, (object) newValue), out result))
        throw new ArgumentException("NumberFormat");
      if (result != newValue)
      {
        this.Value = result;
        return;
      }
    }
    this.SyncTextValue();
  }

  private void UpdateArrowState()
  {
    if (this.IsArrowEnabled)
      VisualStateManager.GoToState((FrameworkElement) this, "ArrowVisible", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "ArrowInvisible", true);
  }

  private void UpdateArrowEnabled()
  {
    double num = this.Value;
    if (this.UpButton != null)
      this.UpButton.IsEnabled = num + this.SmallChange <= this.Maximum;
    if (this.DownButton == null)
      return;
    this.DownButton.IsEnabled = num - this.SmallChange >= this.Minimum;
  }

  public bool SetFallbackValue() => this.ValidCore(true, out double _);

  public void SelectAll() => this._PART_TextBox?.SelectAll();

  public void SetInputValue(string text)
  {
    this.Text = text ?? "";
    if (this._PART_TextBox == null)
      return;
    this._PART_TextBox.Focus();
    Keyboard.Focus((IInputElement) this._PART_TextBox);
  }

  public event EventHandler<NumberBoxInputValueInvalidEventArgs> InputValueInvalid;
}
