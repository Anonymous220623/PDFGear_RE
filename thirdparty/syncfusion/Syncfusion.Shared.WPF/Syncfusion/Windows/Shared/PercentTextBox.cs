// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PercentTextBox
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (PercentTextBox), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Editors/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (PercentTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/TransparentStyle.xaml")]
public class PercentTextBox : EditorBase, IDisposable
{
  internal double? OldValue;
  internal double? mValue;
  internal bool? mValueChanged = new bool?(true);
  internal bool mIsLoaded;
  internal string checktext = "";
  private RepeatButton upButton;
  private RepeatButton downButton;
  internal int percentDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits;
  internal bool IsExceedPercentDecimalDigits;
  private ScrollViewer PART_ContentHost;
  private bool _validatingrResult;
  public static readonly DependencyProperty ValidationOnLostFocusProperty = DependencyProperty.Register(nameof (ValidationOnLostFocus), typeof (bool), typeof (PercentTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(PercentTextBox.OnValidationOnLostFocusChanged)));
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (PercentTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(PercentTextBox.OnPercentGroupSeparatorChanged)));
  public static readonly DependencyProperty PercentEditModeProperty = DependencyProperty.Register(nameof (PercentEditMode), typeof (PercentEditMode), typeof (PercentTextBox), new PropertyMetadata((object) PercentEditMode.DoubleMode, new PropertyChangedCallback(PercentTextBox.OnPercentEditModeChanged)));
  public static readonly DependencyProperty PercentValueProperty = DependencyProperty.Register(nameof (PercentValue), typeof (double?), typeof (PercentTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(PercentTextBox.OnValueChanged), new CoerceValueCallback(PercentTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (double), typeof (PercentTextBox), new PropertyMetadata((object) double.MinValue, new PropertyChangedCallback(PercentTextBox.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (double), typeof (PercentTextBox), new PropertyMetadata((object) double.MaxValue, new PropertyChangedCallback(PercentTextBox.OnMaxValueChanged)));
  public static readonly DependencyProperty PercentDecimalDigitsProperty = DependencyProperty.Register(nameof (PercentDecimalDigits), typeof (int), typeof (PercentTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(PercentTextBox.OnPercentDecimalDigitsChanged)));
  public static readonly DependencyProperty PercentDecimalSeparatorProperty = DependencyProperty.Register(nameof (PercentDecimalSeparator), typeof (string), typeof (PercentTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(PercentTextBox.OnPercentDecimalSeparatorChanged)));
  public static readonly DependencyProperty PercentGroupSeparatorProperty = DependencyProperty.Register(nameof (PercentGroupSeparator), typeof (string), typeof (PercentTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(PercentTextBox.OnPercentGroupSeparatorChanged), new CoerceValueCallback(PercentTextBox.CoercePercentGroupSeperator)));
  public static readonly DependencyProperty ScrollIntervalProperty = DependencyProperty.Register(nameof (ScrollInterval), typeof (double), typeof (PercentTextBox), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty PercentGroupSizesProperty = DependencyProperty.Register(nameof (PercentGroupSizes), typeof (Int32Collection), typeof (PercentTextBox), new PropertyMetadata((object) new Int32Collection(), new PropertyChangedCallback(PercentTextBox.OnPercentGroupSizesChanged)));
  public static readonly DependencyProperty PercentNegativePatternProperty = DependencyProperty.Register(nameof (PercentNegativePattern), typeof (int), typeof (PercentTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(PercentTextBox.OnPercentNegativePatternChanged)));
  public static readonly DependencyProperty PercentPositivePatternProperty = DependencyProperty.Register(nameof (PercentPositivePattern), typeof (int), typeof (PercentTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(PercentTextBox.OnPercentPositivePatternChanged)));
  public static readonly DependencyProperty PercentageSymbolProperty = DependencyProperty.Register(nameof (PercentageSymbol), typeof (string), typeof (PercentTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(PercentTextBox.OnPercentageSymbolChanged)));
  public static readonly DependencyProperty MinPercentDecimalDigitsProperty = DependencyProperty.Register(nameof (MinPercentDecimalDigits), typeof (int), typeof (PercentTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(PercentTextBox.OnMinimumNumberDecimalDigitsChanged)));
  public static readonly DependencyProperty MaxPercentDecimalDigitsProperty = DependencyProperty.Register(nameof (MaxPercentDecimalDigits), typeof (int), typeof (PercentTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(PercentTextBox.OnMaximumNumberDecimalDigitsChanged)));
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (double?), typeof (PercentTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(PercentTextBox.OnNullValueChanged)));
  public static readonly DependencyProperty AllowMultipleSymbolProperty = DependencyProperty.Register(nameof (AllowMultipleSymbol), typeof (bool), typeof (PercentTextBox), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ValueValidationProperty = DependencyProperty.Register(nameof (ValueValidation), typeof (StringValidation), typeof (PercentTextBox), new PropertyMetadata((object) StringValidation.OnLostFocus));
  public static readonly DependencyProperty InvalidValueBehaviorProperty = DependencyProperty.Register(nameof (InvalidValueBehavior), typeof (InvalidInputBehavior), typeof (PercentTextBox), new PropertyMetadata((object) InvalidInputBehavior.None, new PropertyChangedCallback(PercentTextBox.OnInvalidValueBehaviorChanged)));
  public static readonly DependencyProperty ValidationValueProperty = DependencyProperty.Register(nameof (ValidationValue), typeof (string), typeof (PercentTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(PercentTextBox.OnValidationValueChanged)));
  public static readonly DependencyProperty ValidationCompletedProperty = DependencyProperty.Register(nameof (ValidationCompleted), typeof (bool), typeof (PercentTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(PercentTextBox.OnValidationCompletedPropertyChanged)));

  public event PropertyChangedCallback ValidationCompletedChanged;

  public event PropertyChangedCallback InvalidValueBehaviorChanged;

  public event StringValidationCompletedEventHandler ValueValidationCompleted;

  public event PropertyChangedCallback ValidationValueChanged;

  public event CancelEventHandler Validating;

  public event EventHandler Validated;

  public event PropertyChangedCallback PercentageSymbolChanged;

  public event PropertyChangedCallback PercentEditModeChanged;

  public event PropertyChangedCallback PercentValueChanged;

  public event PropertyChangedCallback MinValueChanged;

  public event PropertyChangedCallback MaxValueChanged;

  public event PropertyChangedCallback PercentDecimalDigitsChanged;

  public event PropertyChangedCallback PercentDecimalSeparatorChanged;

  public event PropertyChangedCallback PercentGroupSeparatorChanged;

  public event PropertyChangedCallback PercentGroupSizesChanged;

  private event PropertyChangedCallback MinimumNumberDecimalDigitsChanged;

  private event PropertyChangedCallback MaximumNumberDecimalDigitsChanged;

  static PercentTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PercentTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PercentTextBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ICommand pastecommand { get; private set; }

  public ICommand copycommand { get; private set; }

  public ICommand cutcommand { get; private set; }

  public PercentTextBox()
  {
    this.pastecommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
    this.copycommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._copycommmand), new Predicate<object>(this.Canpaste));
    this.cutcommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._cutcommmand), new Predicate<object>(this.Canpaste));
    this.Loaded -= new RoutedEventHandler(this.IntegerTextbox_Loaded);
    this.Loaded += new RoutedEventHandler(this.IntegerTextbox_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.PercentTextBox_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.PercentTextBox_Unloaded);
    this.SelectionChanged -= new RoutedEventHandler(this.PercentTextBox_SelectionChanged);
    this.SelectionChanged += new RoutedEventHandler(this.PercentTextBox_SelectionChanged);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void PercentTextBox_Unloaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.Unloaded -= new RoutedEventHandler(this.PercentTextBox_Unloaded);
    this.SelectionChanged -= new RoutedEventHandler(this.PercentTextBox_SelectionChanged);
  }

  public new void Dispose()
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.Unloaded -= new RoutedEventHandler(this.PercentTextBox_Unloaded);
    this.SelectionChanged -= new RoutedEventHandler(this.PercentTextBox_SelectionChanged);
    this.pastecommand = (ICommand) null;
    this.copycommand = (ICommand) null;
    this.cutcommand = (ICommand) null;
    base.Dispose();
  }

  void IDisposable.Dispose() => this.Dispose();

  private void _pastecommand(object parameter) => this.Paste();

  private void _copycommmand(object parameter) => this.copy();

  private void _cutcommmand(object parameter) => this.cut();

  private void copy()
  {
    try
    {
      Clipboard.SetText(this.SelectedText);
    }
    catch (COMException ex)
    {
    }
  }

  private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    if (e.Command == ApplicationCommands.Paste)
    {
      this.Paste();
      e.Handled = true;
    }
    if (e.Command != ApplicationCommands.Cut)
      return;
    this.cut();
    e.Handled = true;
  }

  private new void Paste()
  {
    if (this.IsReadOnly)
      return;
    try
    {
      string text1 = Clipboard.GetText();
      int selectionStart = this.SelectionStart;
      string text2 = this.Text;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int num1 = 0;
      if (!this.UseNullOption && this.PercentValue.HasValue)
      {
        double num2 = this.PercentValue.Value;
      }
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      string empty3 = string.Empty;
      bool flag = false;
      for (int index = 0; index < text1.Length; ++index)
      {
        if (numberFormat != null && numberFormat.PercentDecimalSeparator != null)
        {
          if (char.IsDigit(text1[index]) && index == num1)
          {
            num1 = index + 1;
            empty1 += (string) (object) text1[index];
          }
          else if (text1[index].ToString() == numberFormat.PercentDecimalSeparator || text1[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator)
            num1 = index;
          else if (char.IsDigit(text1[index]))
            empty2 += (string) (object) text1[index];
          else if (index <= num1)
            num1 = index + 1;
        }
        else if (char.IsDigit(text1[index]) && index == num1)
        {
          num1 = index + 1;
          empty1 += (string) (object) text1[index];
        }
      }
      if (empty1 == string.Empty && empty2 == string.Empty)
        return;
      string s;
      if (this.PasteMode != PasteMode.Default)
      {
        double? percentValue = this.PercentValue;
        if ((percentValue.GetValueOrDefault() != 0.0 ? 0 : (percentValue.HasValue ? 1 : 0)) == 0 && this.PercentValue.HasValue && this.Text.Length != this.SelectedText.Length)
        {
          if (this.SelectionLength > 0)
          {
            if ((this.SelectedText.Contains(numberFormat.PercentDecimalSeparator) || !(empty2 == string.Empty)) && (!this.SelectedText.Contains(numberFormat.PercentDecimalSeparator) || !(empty2 != string.Empty)))
              return;
            s = this.Text.Remove(this.SelectionStart, this.SelectedText.Length).Insert(this.SelectionStart, empty2 == string.Empty ? empty1 : empty1 + numberFormat.PercentDecimalSeparator + empty2);
            goto label_28;
          }
          if (!(empty2 == string.Empty))
            return;
          s = this.Text.Insert(this.SelectionStart, empty1);
          goto label_28;
        }
      }
      flag = true;
      s = empty1 + numberFormat.PercentDecimalSeparator + empty2;
label_28:
      if (numberFormat != null && (s.Contains(numberFormat.PercentSymbol) || s.Contains(" ") || s.Contains("-")))
      {
        for (int index = 0; index < s.Length; ++index)
        {
          if (s[index].ToString() == numberFormat.PercentSymbol || s[index].ToString() == " " || s[index].ToString() == "-")
          {
            s = s.Remove(index, 1);
            --index;
          }
        }
      }
      if (!string.IsNullOrEmpty(s))
        s = text1.StartsWith("-") || this.IsNegative ? "-" + s : s;
      if (numberFormat.PercentDecimalSeparator != string.Empty && numberFormat.PercentDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator)
        s = s.Replace(numberFormat.PercentDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator);
      if (numberFormat.PercentGroupSeparator != string.Empty && numberFormat.PercentGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.PercentGroupSeparator)
        s = s.Replace(numberFormat.PercentGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.PercentGroupSeparator);
      double result;
      double.TryParse(s, out result);
      if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress || result < this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
      {
        if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress && this.MaxValueOnExceedMaxDigit)
        {
          result = this.MaxValue;
          this.SetValue(new bool?(false), new double?(result));
          if (this.PercentEditMode == PercentEditMode.PercentMode)
            this.Text = result.ToString("P", (IFormatProvider) numberFormat);
          if (this.PercentEditMode == PercentEditMode.DoubleMode)
            this.Text = (result / 100.0).ToString("P", (IFormatProvider) numberFormat);
        }
        if (result >= this.MinValue || this.MinValidation != MinValidation.OnKeyPress || !this.MinValueOnExceedMinDigit)
          return;
        double minValue = this.MinValue;
        this.SetValue(new bool?(false), new double?(minValue));
        if (this.PercentEditMode == PercentEditMode.PercentMode)
          this.Text = minValue.ToString("P", (IFormatProvider) numberFormat);
        if (this.PercentEditMode != PercentEditMode.DoubleMode)
          return;
        this.Text = (minValue / 100.0).ToString("P", (IFormatProvider) numberFormat);
      }
      else
      {
        double num3 = this.MaxLengthValidation(result);
        this.SetValue(new bool?(false), new double?(num3));
        numberFormat.PercentDecimalDigits = this.percentDecimalDigits;
        if (this.PercentEditMode == PercentEditMode.PercentMode)
          this.Text = num3.ToString("P", (IFormatProvider) numberFormat);
        if (this.PercentEditMode == PercentEditMode.DoubleMode)
          this.Text = (num3 / 100.0).ToString("P", (IFormatProvider) numberFormat);
        int index = 0;
        string str = empty1 + empty2;
        int num4;
        for (num4 = flag ? 0 : selectionStart; num4 < this.Text.Length && index < str.Length; ++num4)
        {
          if ((int) this.Text[num4] == (int) str[index])
            ++index;
        }
        this.CaretIndex = num4;
        this.Select(num4, 0);
      }
    }
    catch (COMException ex)
    {
    }
  }

  private void SpinButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeValue((sender as RepeatButton).Name == "upbutton");
  }

  private void ChangeValue(bool canIncrement)
  {
    double? nullable1 = new double?(0.0);
    if (canIncrement)
    {
      double? percentValue = this.PercentValue;
      double maxValue = this.MaxValue;
      if ((percentValue.GetValueOrDefault() <= maxValue ? 0 : (percentValue.HasValue ? 1 : 0)) != 0)
        return;
    }
    if (!canIncrement)
    {
      double? percentValue = this.PercentValue;
      double minValue = this.MinValue;
      if ((percentValue.GetValueOrDefault() >= minValue ? 0 : (percentValue.HasValue ? 1 : 0)) != 0)
        return;
    }
    double? nullable2;
    if (!canIncrement)
    {
      double? percentValue = this.PercentValue;
      double scrollInterval = this.ScrollInterval;
      nullable2 = percentValue.HasValue ? new double?(percentValue.GetValueOrDefault() - scrollInterval) : new double?();
    }
    else
    {
      double? percentValue = this.PercentValue;
      double scrollInterval = this.ScrollInterval;
      nullable2 = percentValue.HasValue ? new double?(percentValue.GetValueOrDefault() + scrollInterval) : new double?();
    }
    this.SetValue(new bool?(true), nullable2);
  }

  private double MaxLengthValidation(double value)
  {
    string str = value.ToString();
    if (this.MaxLength > 0 && str.Length > 0)
    {
      if (str.Length >= this.MaxLength)
      {
        if (this.PercentDecimalDigits > 0)
          str = str.Remove(this.MaxLength - (this.PercentDecimalDigits + 1));
        else if (this.PercentDecimalDigits < 0)
          str = str.Remove(this.MaxLength - 3);
        else if (this.PercentDecimalDigits == 0 && str.Length != this.MaxLength)
          str = str.Remove(this.MaxLength);
      }
      else if (this.PercentDecimalDigits > 0 && str.Length + (this.PercentDecimalDigits + 1) > this.MaxLength)
        str = str.Remove(str.Length - (str.Length - this.MaxLength + (this.PercentDecimalDigits + 1)));
      else if (this.PercentDecimalDigits < 0 && str.Length + 3 > this.MaxLength)
        str = str.Remove(str.Length - (str.Length - this.MaxLength + 3));
      value = Convert.ToDouble(str);
    }
    return value;
  }

  private bool Canpaste(object parameter) => true;

  protected override void OnContextMenuOpening(ContextMenuEventArgs e)
  {
    if (this.IsReadOnly && this.SelectionLength <= 0)
    {
      e.Handled = true;
      base.OnContextMenuOpening(e);
    }
    else
      base.OnContextMenuOpening(e);
  }

  private void PercentTextBox_SelectionChanged(object sender, RoutedEventArgs e)
  {
  }

  public override void OnApplyTemplate()
  {
    AutomationProperties.SetName((DependencyObject) this, this.Name.ToString());
    this.PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
    base.OnApplyTemplate();
    this.upButton = this.GetTemplateChild("upbutton") is RepeatButton ? this.GetTemplateChild("upbutton") as RepeatButton : (RepeatButton) null;
    this.downButton = this.GetTemplateChild("downbutton") is RepeatButton ? this.GetTemplateChild("downbutton") as RepeatButton : (RepeatButton) null;
    if (this.upButton == null || this.downButton == null || !this.ShowSpinButton)
      return;
    this.upButton.Click += new RoutedEventHandler(this.SpinButton_Click);
    this.downButton.Click += new RoutedEventHandler(this.SpinButton_Click);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (!this.IsScrollingOnCircle)
      return;
    e.Handled = true;
    if (e.Delta > 0)
    {
      PercentValueHandler.percentValueHandler.HandleUpDownKey(this, true);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      PercentValueHandler.percentValueHandler.HandleUpDownKey(this, false);
    }
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    e.Handled = PercentValueHandler.percentValueHandler.HandleKeyDown(this, e);
    if (e.Key == Key.Z && !this.IsUndoEnabled)
      e.Handled = true;
    base.OnPreviewKeyDown(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (ModifierKeys.Control == Keyboard.Modifiers)
    {
      if (e.Key == Key.V)
      {
        this.Paste();
        e.Handled = true;
      }
      if (e.Key == Key.Z)
      {
        if (this.IsUndoEnabled)
          this.SetValue(new bool?(true), this.OldValue);
        e.Handled = true;
      }
      if (e.Key == Key.X)
      {
        this.cut();
        e.Handled = true;
      }
    }
    else if (e.Key == Key.Return)
    {
      if (this.EnterToMoveNext)
      {
        TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
        if (Keyboard.FocusedElement is UIElement focusedElement)
        {
          focusedElement.MoveFocus(request);
          e.Handled = true;
        }
      }
    }
    else
      e.Handled = PercentValueHandler.percentValueHandler.HandleKeyDown(this, e);
    base.OnKeyDown(e);
  }

  private void cut()
  {
    try
    {
      if (this.SelectionLength <= 0)
        return;
      Clipboard.SetText(this.SelectedText);
      PercentValueHandler.percentValueHandler.HandleDeleteKey(this);
    }
    catch (COMException ex)
    {
    }
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    if (InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
    {
      this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
      {
        int caretIndex = this.CaretIndex;
        if (this.CaretIndex > 0)
          this.MaskedText = this.MaskedText.Remove(this.CaretIndex - e.Text.Length, e.Text.Length);
        if (caretIndex - e.Text.Length >= 0)
          this.CaretIndex = caretIndex - e.Text.Length;
        e.Handled = PercentValueHandler.percentValueHandler.MatchWithMask(this, e.Text);
        this.allowchange = false;
      }));
    }
    else
    {
      e.Handled = PercentValueHandler.percentValueHandler.MatchWithMask(this, e.Text);
      base.OnTextInput(e);
    }
  }

  internal override void OnCultureChanged()
  {
    base.OnCultureChanged();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  internal override void OnNumberFormatChanged()
  {
    base.OnNumberFormatChanged();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    if (this._validatingrResult)
      e.Handled = true;
    base.OnPreviewLostKeyboardFocus(e);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    if (!this.OnValidating(new CancelEventArgs(false)))
    {
      string message = "";
      bool bIsValidInput = this.ValidationValue == this.PercentValue.ToString();
      string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
      if (!bIsValidInput)
      {
        if (this.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
        {
          int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          this.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationValue));
          this.OnValidated(EventArgs.Empty);
        }
        else if (this.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
        {
          this.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationValue));
          this.OnValidated(EventArgs.Empty);
          this.SetCurrentValue(PercentTextBox.PercentValueProperty, (object) null);
        }
        else if (this.InvalidValueBehavior == InvalidInputBehavior.None)
        {
          this.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationValue));
          this.OnValidated(EventArgs.Empty);
        }
      }
    }
    if (this.EnableFocusColors && this.PART_ContentHost != null)
      this.PART_ContentHost.Background = this.Background;
    if (this.mIsLoaded)
      this.ValidationCompleted = this.ValidationValue == this.PercentValue.ToString();
    double? nullable1 = this.PercentValue;
    if (nullable1.HasValue)
    {
      double? nullable2 = nullable1;
      double maxValue = this.MaxValue;
      if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
      {
        nullable1 = new double?(this.MaxValue);
      }
      else
      {
        double? mValue = this.mValue;
        double minValue = this.MinValue;
        if ((mValue.GetValueOrDefault() >= minValue ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          nullable1 = new double?(this.MinValue);
      }
      double? nullable3 = nullable1;
      double? percentValue = this.PercentValue;
      if ((nullable3.GetValueOrDefault() != percentValue.GetValueOrDefault() ? 1 : (nullable3.HasValue != percentValue.HasValue ? 1 : 0)) != 0)
        this.PercentValue = nullable1;
    }
    base.OnLostFocus(e);
    this.checktext = "";
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    if (this.EnableFocusColors && this.PART_ContentHost != null)
      this.PART_ContentHost.Background = this.FocusedBackground;
    base.OnGotFocus(e);
  }

  public override void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
  {
    if ((bool) args.NewValue || !this.IsNull)
      return;
    this.InvalidateProperty(PercentTextBox.PercentValueProperty);
    this.IsNull = false;
  }

  internal void FormatText()
  {
    if (this.PercentValue.HasValue && !double.IsNaN(this.PercentValue.Value))
    {
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      if (this.PercentEditMode == PercentEditMode.DoubleMode)
        this.MaskedText = (this.mValue.Value / 100.0).ToString("P", (IFormatProvider) numberFormat);
      else
        this.MaskedText = this.mValue.Value.ToString("P", (IFormatProvider) numberFormat);
    }
    else
      this.MaskedText = "";
  }

  internal void SetValue(bool? IsReload, double? _Value)
  {
    bool? nullable1 = IsReload;
    if ((nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
    {
      this.mValueChanged = new bool?(false);
      this.SetCurrentValue(PercentTextBox.PercentValueProperty, (object) _Value);
      this.mValueChanged = new bool?(true);
    }
    else
    {
      bool? nullable2 = IsReload;
      if ((!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
        return;
      int caretIndex = this.CaretIndex;
      this.PercentValue = _Value;
      this.CaretIndex = caretIndex;
    }
  }

  internal double? ValidateValue(double? Val)
  {
    if (Val.HasValue)
    {
      double? nullable = Val;
      double maxValue = this.MaxValue;
      if ((nullable.GetValueOrDefault() <= maxValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        Val = new double?(this.MaxValue);
      }
      else
      {
        double? mValue = this.mValue;
        double minValue = this.MinValue;
        if ((mValue.GetValueOrDefault() >= minValue ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          Val = new double?(this.MinValue);
      }
    }
    return Val;
  }

  internal CultureInfo GetCulture()
  {
    CultureInfo culture = this.Culture == null || this.Culture == CultureInfo.InvariantCulture ? CultureInfo.CurrentCulture.Clone() as CultureInfo : this.Culture.Clone() as CultureInfo;
    if (this.NumberFormat != null)
      culture.NumberFormat = this.NumberFormat;
    culture.NumberFormat.PercentDecimalDigits = this.percentDecimalDigits;
    if (!this.PercentDecimalSeparator.Equals(string.Empty))
      culture.NumberFormat.PercentDecimalSeparator = this.PercentDecimalSeparator;
    if (!this.GroupSeperatorEnabled)
      culture.NumberFormat.PercentGroupSeparator = string.Empty;
    if (this.GroupSeperatorEnabled && !this.PercentGroupSeparator.Equals(string.Empty))
      culture.NumberFormat.PercentGroupSeparator = this.PercentGroupSeparator;
    int count = this.PercentGroupSizes.Count;
    if (count > 0)
    {
      int[] numArray = new int[count];
      for (int index = 0; index < count; ++index)
        numArray[index] = this.PercentGroupSizes[index];
      culture.NumberFormat.PercentGroupSizes = numArray;
    }
    if (this.PercentNegativePattern >= 0)
      culture.NumberFormat.PercentNegativePattern = this.PercentNegativePattern;
    if (this.PercentPositivePattern >= 0)
      culture.NumberFormat.PercentPositivePattern = this.PercentPositivePattern;
    if (!this.PercentageSymbol.Equals(string.Empty))
      culture.NumberFormat.PercentSymbol = this.PercentageSymbol;
    return culture;
  }

  private void CheckIsExceedDecimalDigits()
  {
    if (this.PercentDecimalDigits < 0)
      this.IsExceedPercentDecimalDigits = true;
    else
      this.IsExceedPercentDecimalDigits = false;
  }

  private void UpdatePercentDecimalDigits()
  {
    if (this.PercentDecimalDigits >= 0)
      this.percentDecimalDigits = this.PercentDecimalDigits;
    if (!this.PercentValue.HasValue || !this.IsExceedPercentDecimalDigits)
      return;
    this.UpdatePercentDecimalDigits(PercentTextBox.CountDecimalDigits(this.PercentValue.ToString(), (DependencyObject) this));
  }

  internal static int CountDecimalDigits(string p, DependencyObject d)
  {
    if (string.IsNullOrEmpty(p) || !(d is PercentTextBox))
      return 0;
    int num = 0;
    NumberFormatInfo numberFormat = ((PercentTextBox) d).GetCulture().NumberFormat;
    if (!string.IsNullOrEmpty(numberFormat.PercentDecimalSeparator) && p.Contains(numberFormat.PercentDecimalSeparator) || p.Contains(CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator))
    {
      for (int index = p.Length - 1; index >= 0; --index)
      {
        if (numberFormat != null)
        {
          if (!(p[index].ToString() == numberFormat.PercentDecimalSeparator) && !(p[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator))
            ++num;
          else
            break;
        }
      }
    }
    return num;
  }

  internal void UpdatePercentDecimalDigits(int count)
  {
    if (this.MinPercentDecimalDigits >= 0 && this.MaxPercentDecimalDigits >= 0 && this.MaxPercentDecimalDigits > this.MinPercentDecimalDigits)
    {
      if (count >= this.MinPercentDecimalDigits && count <= this.MaxPercentDecimalDigits)
        this.percentDecimalDigits = count;
      else if (count <= this.MinPercentDecimalDigits)
      {
        this.percentDecimalDigits = this.MinPercentDecimalDigits;
      }
      else
      {
        if (count <= this.MaxPercentDecimalDigits)
          return;
        this.percentDecimalDigits = this.MaxPercentDecimalDigits;
      }
    }
    else if (this.MinPercentDecimalDigits >= 0 && this.MaxPercentDecimalDigits >= 0 && this.MinPercentDecimalDigits >= this.MaxPercentDecimalDigits)
      this.percentDecimalDigits = this.MinPercentDecimalDigits;
    else if (this.MinPercentDecimalDigits >= 0)
    {
      if (count >= this.MinPercentDecimalDigits)
      {
        this.percentDecimalDigits = count;
      }
      else
      {
        if (count >= this.MinPercentDecimalDigits)
          return;
        this.percentDecimalDigits = this.MinPercentDecimalDigits;
      }
    }
    else if (this.MaxPercentDecimalDigits >= 0)
    {
      if (count <= this.MaxPercentDecimalDigits)
      {
        this.percentDecimalDigits = count;
      }
      else
      {
        if (count <= this.MaxPercentDecimalDigits)
          return;
        this.percentDecimalDigits = this.MaxPercentDecimalDigits;
      }
    }
    else
    {
      if (this.MaxPercentDecimalDigits >= 0 || this.MinPercentDecimalDigits > 0 || this.PercentDecimalDigits >= 0)
        return;
      this.percentDecimalDigits = count;
    }
  }

  private static object CoerceValue(DependencyObject d, object baseValue)
  {
    PercentTextBox percentTextBox1 = (PercentTextBox) d;
    if (baseValue != null)
    {
      double? nullable1 = (double?) baseValue;
      bool? mValueChanged = percentTextBox1.mValueChanged;
      if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) != 0)
      {
        double? nullable2 = nullable1;
        double maxValue = percentTextBox1.MaxValue;
        if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 && percentTextBox1.MaxValidation != MaxValidation.OnLostFocus)
        {
          nullable1 = new double?(percentTextBox1.MaxValue);
        }
        else
        {
          double? nullable3 = nullable1;
          double minValue = percentTextBox1.MinValue;
          if ((nullable3.GetValueOrDefault() >= minValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && !percentTextBox1.ValidationOnLostFocus && percentTextBox1.MinValidation != MinValidation.OnLostFocus)
            nullable1 = new double?(percentTextBox1.MinValue);
        }
      }
      if (nullable1.HasValue)
      {
        PercentTextBox percentTextBox2 = percentTextBox1;
        double? nullable4 = nullable1;
        int num1 = (nullable4.GetValueOrDefault() >= 0.0 ? 0 : (nullable4.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        percentTextBox2.IsNegative = num1 != 0;
        PercentTextBox percentTextBox3 = percentTextBox1;
        double? nullable5 = nullable1;
        int num2 = (nullable5.GetValueOrDefault() != 0.0 ? 0 : (nullable5.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        percentTextBox3.IsZero = num2 != 0;
        percentTextBox1.IsNull = false;
      }
      return (object) nullable1;
    }
    if (percentTextBox1.UseNullOption)
    {
      percentTextBox1.IsNull = true;
      percentTextBox1.IsNegative = false;
      percentTextBox1.IsZero = false;
      return (object) percentTextBox1.NullValue;
    }
    double num = 0.0;
    bool? mValueChanged1 = percentTextBox1.mValueChanged;
    if ((!mValueChanged1.GetValueOrDefault() ? 0 : (mValueChanged1.HasValue ? 1 : 0)) != 0)
    {
      if (num > percentTextBox1.MaxValue)
        num = percentTextBox1.MaxValue;
      if (num < percentTextBox1.MinValue)
        num = percentTextBox1.MinValue;
    }
    percentTextBox1.IsNegative = num < 0.0;
    percentTextBox1.IsZero = num == 0.0;
    percentTextBox1.IsNull = false;
    return (object) num;
  }

  private void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.AddHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted), true);
    this.mIsLoaded = true;
    double? nullable1 = (double?) PercentTextBox.CoerceValue((DependencyObject) this, (object) this.PercentValue);
    if (this.IsNull && !this.IsFocused)
      this.WatermarkVisibility = Visibility.Visible;
    double? nullable2 = nullable1;
    double? percentValue = this.PercentValue;
    if ((nullable2.GetValueOrDefault() != percentValue.GetValueOrDefault() ? 1 : (nullable2.HasValue != percentValue.HasValue ? 1 : 0)) != 0)
      this.PercentValue = nullable1;
    else
      this.FormatText();
  }

  public bool ValidationOnLostFocus
  {
    get => (bool) this.GetValue(PercentTextBox.ValidationOnLostFocusProperty);
    set => this.SetValue(PercentTextBox.ValidationOnLostFocusProperty, (object) value);
  }

  public event PropertyChangedCallback ValidationOnLostFocusChanged;

  public PercentEditMode PercentEditMode
  {
    get => (PercentEditMode) this.GetValue(PercentTextBox.PercentEditModeProperty);
    set => this.SetValue(PercentTextBox.PercentEditModeProperty, (object) value);
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(PercentTextBox.GroupSeperatorEnabledProperty);
    set => this.SetValue(PercentTextBox.GroupSeperatorEnabledProperty, (object) value);
  }

  public double? PercentValue
  {
    get => (double?) this.GetValue(PercentTextBox.PercentValueProperty);
    set => this.SetValue(PercentTextBox.PercentValueProperty, (object) value);
  }

  public double MinValue
  {
    get => (double) this.GetValue(PercentTextBox.MinValueProperty);
    set => this.SetValue(PercentTextBox.MinValueProperty, (object) value);
  }

  public double MaxValue
  {
    get => (double) this.GetValue(PercentTextBox.MaxValueProperty);
    set => this.SetValue(PercentTextBox.MaxValueProperty, (object) value);
  }

  public int PercentDecimalDigits
  {
    get => (int) this.GetValue(PercentTextBox.PercentDecimalDigitsProperty);
    set => this.SetValue(PercentTextBox.PercentDecimalDigitsProperty, (object) value);
  }

  public string PercentDecimalSeparator
  {
    get => (string) this.GetValue(PercentTextBox.PercentDecimalSeparatorProperty);
    set => this.SetValue(PercentTextBox.PercentDecimalSeparatorProperty, (object) value);
  }

  public string PercentGroupSeparator
  {
    get => (string) this.GetValue(PercentTextBox.PercentGroupSeparatorProperty);
    set => this.SetValue(PercentTextBox.PercentGroupSeparatorProperty, (object) value);
  }

  public double ScrollInterval
  {
    get => (double) this.GetValue(PercentTextBox.ScrollIntervalProperty);
    set => this.SetValue(PercentTextBox.ScrollIntervalProperty, (object) value);
  }

  public Int32Collection PercentGroupSizes
  {
    get => (Int32Collection) this.GetValue(PercentTextBox.PercentGroupSizesProperty);
    set => this.SetValue(PercentTextBox.PercentGroupSizesProperty, (object) value);
  }

  public int PercentNegativePattern
  {
    get => (int) this.GetValue(PercentTextBox.PercentNegativePatternProperty);
    set => this.SetValue(PercentTextBox.PercentNegativePatternProperty, (object) value);
  }

  public static void OnPercentNegativePatternChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentNegativePatternChanged(args);
  }

  private static object CoercePercentGroupSeperator(DependencyObject d, object baseValue)
  {
    NumberFormatInfo numberFormat = ((PercentTextBox) d).GetCulture().NumberFormat;
    return !baseValue.Equals((object) string.Empty) && !char.IsLetterOrDigit(baseValue.ToString(), 0) || baseValue.Equals((object) string.Empty) ? baseValue : (object) numberFormat.PercentGroupSeparator;
  }

  protected void OnPercentNegativePatternChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public int PercentPositivePattern
  {
    get => (int) this.GetValue(PercentTextBox.PercentPositivePatternProperty);
    set => this.SetValue(PercentTextBox.PercentPositivePatternProperty, (object) value);
  }

  public static void OnPercentPositivePatternChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentPositivePatternChanged(args);
  }

  protected void OnPercentPositivePatternChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public string PercentageSymbol
  {
    get => (string) this.GetValue(PercentTextBox.PercentageSymbolProperty);
    set => this.SetValue(PercentTextBox.PercentageSymbolProperty, (object) value);
  }

  public int MinPercentDecimalDigits
  {
    get => (int) this.GetValue(PercentTextBox.MinPercentDecimalDigitsProperty);
    set => this.SetValue(PercentTextBox.MinPercentDecimalDigitsProperty, (object) value);
  }

  public int MaxPercentDecimalDigits
  {
    get => (int) this.GetValue(PercentTextBox.MaxPercentDecimalDigitsProperty);
    set => this.SetValue(PercentTextBox.MaxPercentDecimalDigitsProperty, (object) value);
  }

  public static void OnPercentDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentDecimalDigitsChanged(args);
  }

  protected void OnPercentDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentDecimalDigitsChanged != null)
      this.PercentDecimalDigitsChanged((DependencyObject) this, args);
    this.CheckIsExceedDecimalDigits();
    this.UpdatePercentDecimalDigits();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private static void OnValidationOnLostFocusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((PercentTextBox) d).OnValidationOnLostFocusChanged(e);
  }

  protected virtual void OnValidationOnLostFocusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ValidationOnLostFocusChanged == null)
      return;
    this.ValidationOnLostFocusChanged((DependencyObject) this, e);
  }

  public static void OnPercentDecimalSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentDecimalSeparatorChanged(args);
  }

  protected void OnPercentDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentDecimalSeparatorChanged != null)
      this.PercentDecimalSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnPercentGroupSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentGroupSeparatorChanged(args);
  }

  protected void OnPercentGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentGroupSeparatorChanged != null)
      this.PercentGroupSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnPercentGroupSizesChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentGroupSizesChanged(args);
  }

  protected void OnPercentGroupSizesChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentGroupSizesChanged != null)
      this.PercentGroupSizesChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnPercentageSymbolChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentageSymbolChanged(args);
  }

  protected void OnPercentageSymbolChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentageSymbolChanged != null)
      this.PercentageSymbolChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private static void OnMinimumNumberDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnMinimumNumberDecimalDigitsChanged(args);
  }

  private void OnMinimumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinimumNumberDecimalDigitsChanged != null)
      this.MinimumNumberDecimalDigitsChanged((DependencyObject) this, args);
    this.CheckIsExceedDecimalDigits();
    this.UpdatePercentDecimalDigits();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private static void OnMaximumNumberDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnMaximumNumberDecimalDigitsChanged(args);
  }

  private void OnMaximumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaximumNumberDecimalDigitsChanged != null)
      this.MaximumNumberDecimalDigitsChanged((DependencyObject) this, args);
    this.CheckIsExceedDecimalDigits();
    this.UpdatePercentDecimalDigits();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnPercentEditModeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnPercentEditModeChanged(args);
  }

  protected void OnPercentEditModeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PercentEditModeChanged != null)
      this.PercentEditModeChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnValueChanged(args);
  }

  protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdatePercentDecimalDigits();
    if (this.PercentValue.HasValue && !double.IsNaN(this.PercentValue.Value))
    {
      double? percentValue1 = this.PercentValue;
      this.IsNegative = (percentValue1.GetValueOrDefault() >= 0.0 ? 0 : (percentValue1.HasValue ? 1 : 0)) != 0;
      double? percentValue2 = this.PercentValue;
      this.IsZero = (percentValue2.GetValueOrDefault() != 0.0 ? 0 : (percentValue2.HasValue ? 1 : 0)) != 0;
      this.IsNull = false;
    }
    else if (this.UseNullOption)
    {
      this.IsNull = true;
      this.IsNegative = false;
      this.IsZero = false;
    }
    this.OldValue = (double?) args.OldValue;
    this.mValue = this.PercentValue;
    if (this.PercentValue.HasValue)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (this.PercentValueChanged != null)
      this.PercentValueChanged((DependencyObject) this, args);
    double? percentValue = this.PercentValue;
    double minValue = this.MinValue;
    if ((percentValue.GetValueOrDefault() <= minValue ? 0 : (percentValue.HasValue ? 1 : 0)) != 0 && this.MinValidation == MinValidation.OnKeyPress)
      this.checktext = "";
    if (!this.mIsLoaded)
      return;
    bool? mValueChanged = this.mValueChanged;
    if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) == 0)
      return;
    this.FormatText();
  }

  public static void OnMinValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnMinValueChanged(args);
  }

  protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinValueChanged != null)
      this.MinValueChanged((DependencyObject) this, args);
    if (this.MaxValue < this.MinValue)
      this.MaxValue = this.MinValue;
    if (this.ValidationOnLostFocus)
      return;
    double? percentValue = this.PercentValue;
    double? nullable = this.ValidateValue(this.PercentValue);
    if ((percentValue.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : (percentValue.HasValue != nullable.HasValue ? 1 : 0)) == 0)
      return;
    this.PercentValue = this.ValidateValue(this.PercentValue);
  }

  public static void OnMaxValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnMaxValueChanged(args);
  }

  protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxValueChanged != null)
      this.MaxValueChanged((DependencyObject) this, args);
    if (this.MinValue > this.MaxValue)
      this.MinValue = this.MaxValue;
    if (this.ValidationOnLostFocus)
      return;
    double? percentValue = this.PercentValue;
    double? nullable = this.ValidateValue(this.PercentValue);
    if ((percentValue.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : (percentValue.HasValue != nullable.HasValue ? 1 : 0)) == 0)
      return;
    this.PercentValue = this.ValidateValue(this.PercentValue);
  }

  public double? NullValue
  {
    get => (double?) this.GetValue(PercentTextBox.NullValueProperty);
    set => this.SetValue(PercentTextBox.NullValueProperty, (object) value);
  }

  public bool AllowMultipleSymbol
  {
    get => (bool) this.GetValue(PercentTextBox.AllowMultipleSymbolProperty);
    set => this.SetValue(PercentTextBox.AllowMultipleSymbolProperty, (object) value);
  }

  public static void OnNullValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnNullValueChanged(args);
  }

  protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public StringValidation ValueValidation
  {
    get => (StringValidation) this.GetValue(PercentTextBox.ValueValidationProperty);
    set => this.SetValue(PercentTextBox.ValueValidationProperty, (object) value);
  }

  public InvalidInputBehavior InvalidValueBehavior
  {
    get => (InvalidInputBehavior) this.GetValue(PercentTextBox.InvalidValueBehaviorProperty);
    set => this.SetValue(PercentTextBox.InvalidValueBehaviorProperty, (object) value);
  }

  public static void OnInvalidValueBehaviorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnInvalidValueBehaviorChanged(args);
  }

  protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.InvalidValueBehaviorChanged == null)
      return;
    this.InvalidValueBehaviorChanged((DependencyObject) this, args);
  }

  public string ValidationValue
  {
    get => (string) this.GetValue(PercentTextBox.ValidationValueProperty);
    set => this.SetValue(PercentTextBox.ValidationValueProperty, (object) value);
  }

  public static void OnValidationValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnValidationValueChanged(args);
  }

  protected void OnValidationValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationValueChanged == null)
      return;
    this.ValidationValueChanged((DependencyObject) this, args);
  }

  public bool ValidationCompleted
  {
    get => (bool) this.GetValue(PercentTextBox.ValidationCompletedProperty);
    set => this.SetValue(PercentTextBox.ValidationCompletedProperty, (object) value);
  }

  public static void OnValidationCompletedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((PercentTextBox) obj)?.OnValidationCompletedPropertyChanged(args);
  }

  protected void OnValidationCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationCompletedChanged == null)
      return;
    this.ValidationCompletedChanged((DependencyObject) this, args);
  }

  internal void OnValueValidationCompleted(StringValidationEventArgs e)
  {
    if (this.ValueValidationCompleted == null)
      return;
    this.ValueValidationCompleted((object) this, e);
  }

  internal void OnValidated(EventArgs e)
  {
    if (this.Validated == null)
      return;
    this.Validated((object) this, e);
  }

  internal bool OnValidating(CancelEventArgs e)
  {
    if (this.Validating == null)
      return false;
    this.Validating((object) this, e);
    this._validatingrResult = e.Cancel;
    return e.Cancel;
  }
}
