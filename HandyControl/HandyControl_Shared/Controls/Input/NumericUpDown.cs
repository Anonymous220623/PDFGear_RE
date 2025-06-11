// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.NumericUpDown
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_TextBox", Type = typeof (TextBox))]
public class NumericUpDown : Control
{
  private const string ElementTextBox = "PART_TextBox";
  private TextBox _textBox;
  public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<double>>), typeof (NumericUpDown));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(NumericUpDown.OnValueChanged), new CoerceValueCallback(NumericUpDown.CoerceValue)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (NumericUpDown), new PropertyMetadata((object) double.MaxValue, new PropertyChangedCallback(NumericUpDown.OnMaximumChanged), new CoerceValueCallback(NumericUpDown.CoerceMaximum)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (NumericUpDown), new PropertyMetadata((object) double.MinValue, new PropertyChangedCallback(NumericUpDown.OnMinimumChanged), new CoerceValueCallback(NumericUpDown.CoerceMinimum)), new ValidateValueCallback(ValidateHelper.IsInRangeOfDouble));
  public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof (Increment), typeof (double), typeof (NumericUpDown), new PropertyMetadata(ValueBoxes.Double1Box));
  public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof (DecimalPlaces), typeof (int?), typeof (NumericUpDown), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(nameof (ValueFormat), typeof (string), typeof (NumericUpDown), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty ShowUpDownButtonProperty = DependencyProperty.Register(nameof (ShowUpDownButton), typeof (bool), typeof (NumericUpDown), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (NumericUpDown), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (NumericUpDown));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (NumericUpDown));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (NumericUpDown));

  public NumericUpDown()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Prev, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      if (this.IsReadOnly)
        return;
      this.SetCurrentValue(NumericUpDown.ValueProperty, (object) (this.Value + this.Increment));
    })));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Next, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      if (this.IsReadOnly)
        return;
      this.SetCurrentValue(NumericUpDown.ValueProperty, (object) (this.Value - this.Increment));
    })));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      if (this.IsReadOnly)
        return;
      this.SetCurrentValue(NumericUpDown.ValueProperty, ValueBoxes.Double0Box);
    })));
  }

  public override void OnApplyTemplate()
  {
    if (this._textBox != null)
    {
      this._textBox.PreviewKeyDown -= new KeyEventHandler(this.TextBox_PreviewKeyDown);
      this._textBox.TextChanged -= new TextChangedEventHandler(this.TextBox_TextChanged);
      this._textBox.LostFocus -= new RoutedEventHandler(this.TextBox_LostFocus);
    }
    base.OnApplyTemplate();
    this._textBox = this.GetTemplateChild("PART_TextBox") as TextBox;
    if (this._textBox == null)
      return;
    this._textBox.SetBinding(NumericUpDown.SelectionBrushProperty, (BindingBase) new Binding(NumericUpDown.SelectionBrushProperty.Name)
    {
      Source = (object) this
    });
    this._textBox.SetBinding(NumericUpDown.SelectionOpacityProperty, (BindingBase) new Binding(NumericUpDown.SelectionOpacityProperty.Name)
    {
      Source = (object) this
    });
    this._textBox.SetBinding(NumericUpDown.CaretBrushProperty, (BindingBase) new Binding(NumericUpDown.CaretBrushProperty.Name)
    {
      Source = (object) this
    });
    this._textBox.PreviewKeyDown += new KeyEventHandler(this.TextBox_PreviewKeyDown);
    this._textBox.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
    this._textBox.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
    this._textBox.Text = this.CurrentText;
  }

  private void TextBox_LostFocus(object sender, RoutedEventArgs e)
  {
    if (string.IsNullOrWhiteSpace(this._textBox.Text))
    {
      this.SetCurrentValue(NumericUpDown.ValueProperty, ValueBoxes.Double0Box);
    }
    else
    {
      double result;
      if (double.TryParse(this._textBox.Text, out result))
        this.SetCurrentValue(NumericUpDown.ValueProperty, (object) result);
      else
        this.SetText(true);
    }
  }

  private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    double result;
    if (!double.TryParse(this._textBox.Text, out result) || result < this.Minimum || result > this.Maximum)
      return;
    this.SetCurrentValue(NumericUpDown.ValueProperty, (object) result);
  }

  private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (this.IsReadOnly)
      return;
    if (e.Key == Key.Up)
    {
      this.Value += this.Increment;
    }
    else
    {
      if (e.Key != Key.Down)
        return;
      this.Value -= this.Increment;
    }
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (!this._textBox.IsFocused || this.IsReadOnly)
      return;
    this.Value += e.Delta > 0 ? this.Increment : -this.Increment;
    this.SetText(true);
    e.Handled = true;
  }

  private string CurrentText
  {
    get
    {
      if (!string.IsNullOrWhiteSpace(this.ValueFormat))
        return this.Value.ToString(this.ValueFormat);
      int? decimalPlaces = this.DecimalPlaces;
      if (!decimalPlaces.HasValue)
        return this.Value.ToString();
      double num = this.Value;
      ref double local = ref num;
      decimalPlaces = this.DecimalPlaces;
      string format = "#0." + new string('0', decimalPlaces.Value);
      return local.ToString(format);
    }
  }

  protected virtual void OnValueChanged(FunctionEventArgs<double> e)
  {
    this.RaiseEvent((RoutedEventArgs) e);
  }

  public event EventHandler<FunctionEventArgs<double>> ValueChanged
  {
    add => this.AddHandler(NumericUpDown.ValueChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(NumericUpDown.ValueChangedEvent, (Delegate) value);
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NumericUpDown source = (NumericUpDown) d;
    double newValue = (double) e.NewValue;
    source.SetText();
    source.OnValueChanged(new FunctionEventArgs<double>(NumericUpDown.ValueChangedEvent, (object) source)
    {
      Info = newValue
    });
  }

  private void SetText(bool force = false)
  {
    if (this._textBox == null || !(!this._textBox.IsFocused | force))
      return;
    this._textBox.Text = this.CurrentText;
    this._textBox.Select(this._textBox.Text.Length, 0);
  }

  private static object CoerceValue(DependencyObject d, object basevalue)
  {
    NumericUpDown numericUpDown = (NumericUpDown) d;
    double minimum = numericUpDown.Minimum;
    double num = (double) basevalue;
    if (num < minimum)
    {
      numericUpDown.Value = minimum;
      return (object) minimum;
    }
    double maximum = numericUpDown.Maximum;
    if (num > maximum)
      numericUpDown.Value = maximum;
    numericUpDown.SetText();
    return (object) (num > maximum ? maximum : num);
  }

  public double Value
  {
    get => (double) this.GetValue(NumericUpDown.ValueProperty);
    set => this.SetValue(NumericUpDown.ValueProperty, (object) value);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NumericUpDown numericUpDown = (NumericUpDown) d;
    numericUpDown.CoerceValue(NumericUpDown.MinimumProperty);
    numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
  }

  private static object CoerceMaximum(DependencyObject d, object basevalue)
  {
    double minimum = ((NumericUpDown) d).Minimum;
    return (double) basevalue >= minimum ? basevalue : (object) minimum;
  }

  public double Maximum
  {
    get => (double) this.GetValue(NumericUpDown.MaximumProperty);
    set => this.SetValue(NumericUpDown.MaximumProperty, (object) value);
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NumericUpDown numericUpDown = (NumericUpDown) d;
    numericUpDown.CoerceValue(NumericUpDown.MaximumProperty);
    numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
  }

  private static object CoerceMinimum(DependencyObject d, object basevalue)
  {
    double maximum = ((NumericUpDown) d).Maximum;
    return (double) basevalue <= maximum ? basevalue : (object) maximum;
  }

  public double Minimum
  {
    get => (double) this.GetValue(NumericUpDown.MinimumProperty);
    set => this.SetValue(NumericUpDown.MinimumProperty, (object) value);
  }

  public double Increment
  {
    get => (double) this.GetValue(NumericUpDown.IncrementProperty);
    set => this.SetValue(NumericUpDown.IncrementProperty, (object) value);
  }

  public int? DecimalPlaces
  {
    get => (int?) this.GetValue(NumericUpDown.DecimalPlacesProperty);
    set => this.SetValue(NumericUpDown.DecimalPlacesProperty, (object) value);
  }

  public string ValueFormat
  {
    get => (string) this.GetValue(NumericUpDown.ValueFormatProperty);
    set => this.SetValue(NumericUpDown.ValueFormatProperty, (object) value);
  }

  internal bool ShowUpDownButton
  {
    get => (bool) this.GetValue(NumericUpDown.ShowUpDownButtonProperty);
    set => this.SetValue(NumericUpDown.ShowUpDownButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsReadOnly
  {
    get => (bool) this.GetValue(NumericUpDown.IsReadOnlyProperty);
    set => this.SetValue(NumericUpDown.IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(NumericUpDown.SelectionBrushProperty);
    set => this.SetValue(NumericUpDown.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(NumericUpDown.SelectionOpacityProperty);
    set => this.SetValue(NumericUpDown.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(NumericUpDown.CaretBrushProperty);
    set => this.SetValue(NumericUpDown.CaretBrushProperty, (object) value);
  }
}
