// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CurrencyTextBox
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

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (CurrencyTextBox), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (CurrencyTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
public class CurrencyTextBox : EditorBase, IDisposable
{
  internal Decimal? OldValue;
  internal Decimal? mValue;
  internal bool? mValueChanged = new bool?(true);
  internal bool mIsLoaded;
  internal string checktext = "";
  private string currencySymbol;
  private RepeatButton upButton;
  private RepeatButton downButton;
  internal int currencyDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
  internal bool IsExceedCurrencyDecimalDigits;
  private ScrollViewer PART_ContentHost;
  private Border Focused_Border;
  private bool _validatingrResult;
  private bool IsGotFocus;
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (Decimal?), typeof (CurrencyTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(CurrencyTextBox.OnValueChanged), new CoerceValueCallback(CurrencyTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (Decimal), typeof (CurrencyTextBox), new PropertyMetadata((object) Decimal.MinValue, new PropertyChangedCallback(CurrencyTextBox.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (Decimal), typeof (CurrencyTextBox), new PropertyMetadata((object) Decimal.MaxValue, new PropertyChangedCallback(CurrencyTextBox.OnMaxValueChanged)));
  public static readonly DependencyProperty CurrencyDecimalDigitsProperty = DependencyProperty.Register(nameof (CurrencyDecimalDigits), typeof (int), typeof (CurrencyTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyDecimalDigitsChanged)));
  public static readonly DependencyProperty CurrencyDecimalSeparatorProperty = DependencyProperty.Register(nameof (CurrencyDecimalSeparator), typeof (string), typeof (CurrencyTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyDecimalSeparatorChanged)));
  public static readonly DependencyProperty CurrencyGroupSeparatorProperty = DependencyProperty.Register(nameof (CurrencyGroupSeparator), typeof (string), typeof (CurrencyTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyGroupSeparatorChanged), new CoerceValueCallback(CurrencyTextBox.CoerceCurrencyGroupSeperator)));
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (CurrencyTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyGroupSeparatorChanged)));
  public static readonly DependencyProperty CurrencyGroupSizesProperty = DependencyProperty.Register(nameof (CurrencyGroupSizes), typeof (Int32Collection), typeof (CurrencyTextBox), (PropertyMetadata) new UIPropertyMetadata((object) new Int32Collection(), new PropertyChangedCallback(CurrencyTextBox.OnCurrencyGroupSizesChanged)));
  public static readonly DependencyProperty CurrencyNegativePatternProperty = DependencyProperty.Register(nameof (CurrencyNegativePattern), typeof (int), typeof (CurrencyTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyNegativePatternChanged)));
  public static readonly DependencyProperty CurrencyPositivePatternProperty = DependencyProperty.Register(nameof (CurrencyPositivePattern), typeof (int), typeof (CurrencyTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(CurrencyTextBox.OnCurrencyPositivePatternChanged)));
  public static readonly DependencyProperty CurrencySymbolProperty = DependencyProperty.Register(nameof (CurrencySymbol), typeof (string), typeof (CurrencyTextBox), new PropertyMetadata((object) NumberFormatInfo.CurrentInfo.CurrencySymbol, new PropertyChangedCallback(CurrencyTextBox.OnCurrencySymbolChanged)));
  public static readonly DependencyProperty ScrollIntervalProperty = DependencyProperty.Register(nameof (ScrollInterval), typeof (double), typeof (CurrencyTextBox), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty MaximumCurrencyDecimalDigitsProperty = DependencyProperty.Register(nameof (MaximumCurrencyDecimalDigits), typeof (int), typeof (CurrencyTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(CurrencyTextBox.OnMaximumCurrencyDecimalDigitsChanged)));
  public static readonly DependencyProperty MinimumCurrencyDecimalDigitsProperty = DependencyProperty.Register(nameof (MinimumCurrencyDecimalDigits), typeof (int), typeof (CurrencyTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(CurrencyTextBox.OnMinimumCurrencyDecimalDigitsChanged)));
  internal bool IsValueChanged = true;
  public static readonly DependencyProperty CurrencySymbolPositionProperty = DependencyProperty.Register("CurrencySymbolPosition ", typeof (CurrencySymbolPosition), typeof (CurrencyTextBox), new PropertyMetadata((object) CurrencySymbolPosition.Left, new PropertyChangedCallback(CurrencyTextBox.OnCurrencySymbolPositionChanged)));
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (Decimal?), typeof (CurrencyTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(CurrencyTextBox.OnNullValueChanged)));
  public static readonly DependencyProperty ValueValidationProperty = DependencyProperty.Register(nameof (ValueValidation), typeof (StringValidation), typeof (CurrencyTextBox), new PropertyMetadata((object) StringValidation.OnLostFocus));
  public static readonly DependencyProperty InvalidValueBehaviorProperty = DependencyProperty.Register(nameof (InvalidValueBehavior), typeof (InvalidInputBehavior), typeof (CurrencyTextBox), new PropertyMetadata((object) InvalidInputBehavior.None, new PropertyChangedCallback(CurrencyTextBox.OnInvalidValueBehaviorChanged)));
  public static readonly DependencyProperty ValidationValueProperty = DependencyProperty.Register(nameof (ValidationValue), typeof (string), typeof (CurrencyTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(CurrencyTextBox.OnValidationValueChanged)));
  public static readonly DependencyProperty ValidationCompletedProperty = DependencyProperty.Register(nameof (ValidationCompleted), typeof (bool), typeof (CurrencyTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(CurrencyTextBox.OnValidationCompletedPropertyChanged)));

  public event PropertyChangedCallback MinValueChanged;

  public event PropertyChangedCallback CurrencySymbolPositionChanged;

  public event PropertyChangedCallback ValueChanged;

  public event PropertyChangedCallback MaxValueChanged;

  public event PropertyChangedCallback CurrencyDecimalDigitsChanged;

  public event PropertyChangedCallback CurrencyDecimalSeparatorChanged;

  public event PropertyChangedCallback CurrencyGroupSeparatorChanged;

  public event PropertyChangedCallback CurrencyGroupSizesChanged;

  public event PropertyChangedCallback CurrencyNegativePatternChanged;

  public event PropertyChangedCallback CurrencyPositivePatternChanged;

  public event PropertyChangedCallback CurrencySymbolChanged;

  public event PropertyChangedCallback ValidationCompletedChanged;

  public event PropertyChangedCallback InvalidValueBehaviorChanged;

  public event StringValidationCompletedEventHandler ValueValidationCompleted;

  public event PropertyChangedCallback ValidationValueChanged;

  public event CancelEventHandler Validating;

  public event EventHandler Validated;

  static CurrencyTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CurrencyTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CurrencyTextBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ICommand pastecommand { get; private set; }

  public ICommand copycommand { get; private set; }

  public ICommand cutcommand { get; private set; }

  public CurrencyTextBox()
  {
    this.pastecommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
    this.copycommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
    this.cutcommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
    this.Loaded += new RoutedEventHandler(this.IntegerTextbox_Loaded);
    this.currencySymbol = this.CurrencySymbol;
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void _pastecommand(object parameter) => this.Paste();

  private void _copycommand(object parameter) => this.copy();

  private void _cutcommand(object parameter) => this.cut();

  private bool Canpaste(object parameter) => true;

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    AutomationProperties.SetName((DependencyObject) this, this.Name.ToString());
    this.PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
    this.upButton = this.GetTemplateChild("upbutton") is RepeatButton ? this.GetTemplateChild("upbutton") as RepeatButton : (RepeatButton) null;
    this.downButton = this.GetTemplateChild("downbutton") is RepeatButton ? this.GetTemplateChild("downbutton") as RepeatButton : (RepeatButton) null;
    if (this.upButton == null || this.downButton == null || !this.ShowSpinButton)
      return;
    this.upButton.Click += new RoutedEventHandler(this.SpinButton_Click);
    this.downButton.Click += new RoutedEventHandler(this.SpinButton_Click);
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
      string text = Clipboard.GetText();
      int selectionStart = this.SelectionStart;
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int num1 = 0;
      if (!this.UseNullOption && this.Value.HasValue)
      {
        Decimal num2 = this.Value.Value;
      }
      string empty3 = string.Empty;
      bool flag = false;
      for (int index = 0; index < text.Length; ++index)
      {
        if (numberFormat != null && numberFormat.CurrencyDecimalSeparator != null)
        {
          if (char.IsDigit(text[index]) && index == num1)
          {
            num1 = index + 1;
            empty1 += (string) (object) text[index];
          }
          else if (text[index].ToString() == numberFormat.CurrencyDecimalSeparator || text[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
            num1 = index;
          else if (char.IsDigit(text[index]))
            empty2 += (string) (object) text[index];
          else if (index <= num1)
            num1 = index + 1;
        }
        else if (char.IsDigit(text[index]) && index == num1)
        {
          num1 = index + 1;
          empty1 += (string) (object) text[index];
        }
      }
      if (empty1 == string.Empty && empty2 == string.Empty)
        return;
      string s;
      if (this.PasteMode != PasteMode.Default)
      {
        Decimal? nullable = this.Value;
        if ((!(nullable.GetValueOrDefault() == 0M) ? 0 : (nullable.HasValue ? 1 : 0)) == 0 && this.Value.HasValue && this.Text.Length != this.SelectedText.Length)
        {
          if (this.SelectionLength > 0)
          {
            if ((this.SelectedText.Contains(numberFormat.CurrencyDecimalSeparator) || !(empty2 == string.Empty)) && (!this.SelectedText.Contains(numberFormat.CurrencyDecimalSeparator) || !(empty2 != string.Empty)))
              return;
            s = this.Text.Remove(this.SelectionStart, this.SelectedText.Length).Insert(this.SelectionStart, empty2 == string.Empty ? empty1 : empty1 + numberFormat.CurrencyDecimalSeparator + empty2);
            goto label_28;
          }
          if (!(empty2 == string.Empty))
            return;
          s = this.Text.Insert(this.SelectionStart, empty1);
          goto label_28;
        }
      }
      flag = true;
      s = empty1 + numberFormat.CurrencyDecimalSeparator + empty2;
label_28:
      if (numberFormat != null && (s.Contains(numberFormat.CurrencySymbol) || s.Contains(" ") || s.Contains("-") || s.Contains("(") || s.Contains(")")))
      {
        for (int index = 0; index < s.Length; ++index)
        {
          if (s[index].ToString() == numberFormat.CurrencySymbol || s[index].ToString() == " " || s[index].ToString() == "-" || s[index].ToString() == "(" || s[index].ToString() == ")")
          {
            s = s.Remove(index, 1);
            --index;
          }
        }
      }
      if (!string.IsNullOrEmpty(s))
        s = text.StartsWith("-") || this.IsNegative ? "-" + s : s;
      if (numberFormat.CurrencyDecimalSeparator != string.Empty && numberFormat.CurrencyDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
        s = s.Replace(numberFormat.CurrencyDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
      if (numberFormat.CurrencyGroupSeparator != string.Empty && numberFormat.CurrencyGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator)
        s = s.Replace(numberFormat.CurrencyGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);
      Decimal result;
      Decimal.TryParse(s, out result);
      if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress || result < this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
      {
        if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress && this.MaxValueOnExceedMaxDigit)
        {
          result = this.MaxValue;
          this.SetValue(new bool?(false), new Decimal?(result));
          this.Text = result.ToString("C", (IFormatProvider) numberFormat);
        }
        if (!(result < this.MinValue) || this.MinValidation != MinValidation.OnKeyPress || !this.MinValueOnExceedMinDigit)
          return;
        Decimal minValue = this.MinValue;
        this.SetValue(new bool?(false), new Decimal?(minValue));
        this.Text = minValue.ToString("C", (IFormatProvider) numberFormat);
      }
      else
      {
        string str1 = Convert.ToInt64(result).ToString();
        if (this.MaxLength > 0 && str1.Length > 0)
        {
          if (str1.Length >= this.MaxLength)
          {
            if (this.CurrencyDecimalDigits > 0)
              str1 = str1.Remove(this.MaxLength - (this.CurrencyDecimalDigits + 1));
            else if (this.CurrencyDecimalDigits < 0)
              str1 = str1.Remove(this.MaxLength - 3);
            else if (this.CurrencyDecimalDigits == 0 && str1.Length != this.MaxLength)
              str1 = str1.Remove(this.MaxLength);
          }
          else if (this.CurrencyDecimalDigits > 0 && str1.Length + (this.CurrencyDecimalDigits + 1) > this.MaxLength)
            str1 = str1.Remove(str1.Length - (str1.Length - this.MaxLength + (this.CurrencyDecimalDigits + 1)));
          else if (this.CurrencyDecimalDigits < 0 && str1.Length + 3 > this.MaxLength)
            str1 = str1.Remove(str1.Length - (str1.Length - this.MaxLength + 3));
          result = Convert.ToDecimal(str1);
        }
        this.SetValue(new bool?(false), new Decimal?(result));
        numberFormat.CurrencyDecimalDigits = this.currencyDecimalDigits;
        this.Text = result.ToString("C", (IFormatProvider) numberFormat);
        int index = 0;
        string str2 = empty1 + empty2;
        int num3;
        for (num3 = flag ? 0 : selectionStart; num3 < this.Text.Length && index < str2.Length; ++num3)
        {
          if ((int) this.Text[num3] == (int) str2[index])
            ++index;
        }
        this.CaretIndex = num3;
        this.Select(num3, 0);
      }
    }
    catch (COMException ex)
    {
    }
  }

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

  private void SpinButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeValue((sender as RepeatButton).Name == "upbutton");
  }

  private void ChangeValue(bool canIncrement)
  {
    Decimal? nullable1 = new Decimal?(0M);
    if (canIncrement)
    {
      Decimal? nullable2 = this.Value;
      Decimal maxValue = this.MaxValue;
      if ((!(nullable2.GetValueOrDefault() > maxValue) ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        return;
    }
    if (!canIncrement)
    {
      Decimal? nullable3 = this.Value;
      Decimal minValue = this.MinValue;
      if ((!(nullable3.GetValueOrDefault() < minValue) ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
        return;
    }
    Decimal? nullable4;
    if (!canIncrement)
    {
      Decimal? nullable5 = this.Value;
      Decimal scrollInterval = (Decimal) this.ScrollInterval;
      nullable4 = nullable5.HasValue ? new Decimal?(nullable5.GetValueOrDefault() - scrollInterval) : new Decimal?();
    }
    else
    {
      Decimal? nullable6 = this.Value;
      Decimal scrollInterval = (Decimal) this.ScrollInterval;
      nullable4 = nullable6.HasValue ? new Decimal?(nullable6.GetValueOrDefault() + scrollInterval) : new Decimal?();
    }
    this.SetValue(new bool?(true), nullable4);
  }

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

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (!this.IsScrollingOnCircle)
      return;
    e.Handled = true;
    if (e.Delta > 0)
    {
      CurrencyValueHandler.currencyValueHandler.HandleUpDownKey(this, true);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      CurrencyValueHandler.currencyValueHandler.HandleUpDownKey(this, false);
    }
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.TextSelectionOnFocus && !this.IsFocused)
    {
      e.Handled = true;
      if (this.ShowSpinButton)
      {
        Grid templateChild = this.GetTemplateChild("spinButtonGrid") is Grid ? this.GetTemplateChild("spinButtonGrid") as Grid : (Grid) null;
        if (templateChild != null && VisualTreeHelper.HitTest((Visual) this, e.GetPosition((IInputElement) templateChild)) != null)
          e.Handled = false;
      }
      this.Focus();
    }
    base.OnPreviewMouseLeftButtonDown(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    e.Handled = CurrencyValueHandler.currencyValueHandler.HandleKeyDown(this, e);
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
      e.Handled = CurrencyValueHandler.currencyValueHandler.HandleKeyDown(this, e);
    base.OnKeyDown(e);
  }

  private void cut()
  {
    try
    {
      if (this.SelectionLength <= 0)
        return;
      Clipboard.SetText(this.SelectedText);
      CurrencyValueHandler.currencyValueHandler.HandleDeleteKey(this);
    }
    catch (COMException ex)
    {
    }
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    int caretIndex = this.CaretIndex;
    if (this.CaretIndex > 0 && InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
      this.Text = this.Text.Remove(this.CaretIndex - e.Text.Length, e.Text.Length);
    this.CaretIndex = caretIndex;
    if (this.IsGotFocus && e.Text.ToString().Equals("\r"))
    {
      this.IsGotFocus = false;
      e.Handled = false;
    }
    else
      e.Handled = CurrencyValueHandler.currencyValueHandler.MatchWithMask(this, e.Text);
    this.IsValueChanged = false;
    base.OnTextInput(e);
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
      bool bIsValidInput = this.ValidationValue == this.Value.ToString();
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
          this.SetCurrentValue(CurrencyTextBox.ValueProperty, (object) null);
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
    if (this.Focused_Border != null)
      this.Focused_Border.Background = this.Background;
    if (this.mIsLoaded)
    {
      Decimal? nullable1 = new Decimal?();
      if (!string.IsNullOrEmpty(this.ValidationValue))
      {
        nullable1 = new Decimal?(Decimal.Parse(this.ValidationValue));
        if (nullable1.HasValue)
        {
          Decimal? nullable2 = nullable1;
          Decimal? nullable3 = this.Value;
          if ((!(nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault()) ? 0 : (nullable2.HasValue == nullable3.HasValue ? 1 : 0)) != 0)
          {
            this.ValidationCompleted = true;
            goto label_18;
          }
        }
        this.ValidationCompleted = false;
      }
    }
label_18:
    this.checktext = "";
    Decimal? nullable4 = this.Value;
    if (nullable4.HasValue)
    {
      Decimal? nullable5 = nullable4;
      Decimal maxValue = this.MaxValue;
      if ((!(nullable5.GetValueOrDefault() > maxValue) ? 0 : (nullable5.HasValue ? 1 : 0)) != 0)
      {
        nullable4 = new Decimal?(this.MaxValue);
      }
      else
      {
        Decimal? mValue = this.mValue;
        Decimal minValue = this.MinValue;
        if ((!(mValue.GetValueOrDefault() < minValue) ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          nullable4 = new Decimal?(this.MinValue);
      }
      Decimal? nullable6 = nullable4;
      Decimal? nullable7 = this.Value;
      if ((nullable6.GetValueOrDefault() != nullable7.GetValueOrDefault() ? 1 : (nullable6.HasValue != nullable7.HasValue ? 1 : 0)) != 0)
        this.Value = nullable4;
    }
    Decimal? nullable8 = this.Value;
    if ((!(nullable8.GetValueOrDefault() == 0M) ? 0 : (nullable8.HasValue ? 1 : 0)) != 0 && this.MaskedText.Contains("-"))
      this.MaskedText = "0.00";
    base.OnLostFocus(e);
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    this.IsGotFocus = true;
    if (this.Focused_Border != null)
      this.Focused_Border.Background = this.FocusedBackground;
    if (this.EnableFocusColors && this.PART_ContentHost != null)
      this.PART_ContentHost.Background = this.FocusedBackground;
    base.OnGotFocus(e);
  }

  public override void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
  {
    if ((bool) args.NewValue || !this.IsNull)
      return;
    this.InvalidateProperty(CurrencyTextBox.ValueProperty);
    this.IsNull = false;
  }

  internal void FormatText()
  {
    if (this.Value.HasValue && this.mValue.HasValue)
    {
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      if (this.NumberFormat == null && this.currencySymbol == numberFormat.CurrencySymbol)
        numberFormat.CurrencySymbol = this.CurrencySymbol;
      this.Text = this.mValue.Value.ToString("C", (IFormatProvider) numberFormat);
      this.MaskedText = this.Text;
    }
    else
      this.MaskedText = "";
  }

  internal void SetValue(bool? IsReload, Decimal? _Value)
  {
    bool? nullable1 = IsReload;
    if ((nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
    {
      this.mValueChanged = new bool?(false);
      this.SetCurrentValue(CurrencyTextBox.ValueProperty, (object) _Value);
      this.mValueChanged = new bool?(true);
      this.IsValueChanged = false;
    }
    else
    {
      bool? nullable2 = IsReload;
      if ((!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
        return;
      int caretIndex = this.CaretIndex;
      this.Value = _Value;
      this.CaretIndex = caretIndex;
    }
  }

  internal Decimal? ValidateValue(Decimal? Val)
  {
    if (Val.HasValue)
    {
      Decimal? nullable = Val;
      Decimal maxValue = this.MaxValue;
      if ((!(nullable.GetValueOrDefault() > maxValue) ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        Val = new Decimal?(this.MaxValue);
      }
      else
      {
        Decimal? mValue = this.mValue;
        Decimal minValue = this.MinValue;
        if ((!(mValue.GetValueOrDefault() < minValue) ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          Val = new Decimal?(this.MinValue);
      }
    }
    return Val;
  }

  internal CultureInfo GetCulture()
  {
    CultureInfo culture = this.Culture == null || this.Culture == CultureInfo.InvariantCulture ? CultureInfo.CurrentCulture.Clone() as CultureInfo : this.Culture.Clone() as CultureInfo;
    if (this.NumberFormat != null)
    {
      culture.NumberFormat = this.NumberFormat;
      this.CurrencySymbol = culture.NumberFormat.CurrencySymbol;
    }
    if (this.MaxLength != 0 && this.CurrencyDecimalDigits < this.MaxLength && this.currencyDecimalDigits >= 0 || this.MaxLength == 0 && this.currencyDecimalDigits >= 0)
      culture.NumberFormat.CurrencyDecimalDigits = this.currencyDecimalDigits;
    if (!this.CurrencyDecimalSeparator.Equals(string.Empty))
      culture.NumberFormat.CurrencyDecimalSeparator = this.CurrencyDecimalSeparator;
    if (!this.GroupSeperatorEnabled)
      culture.NumberFormat.CurrencyGroupSeparator = string.Empty;
    if (this.GroupSeperatorEnabled && !this.CurrencyGroupSeparator.Equals(string.Empty))
      culture.NumberFormat.CurrencyGroupSeparator = this.CurrencyGroupSeparator;
    int count = this.CurrencyGroupSizes.Count;
    if (count > 0)
    {
      int[] numArray = new int[count];
      for (int index = 0; index < count; ++index)
        numArray[index] = this.CurrencyGroupSizes[index];
      culture.NumberFormat.CurrencyGroupSizes = numArray;
    }
    if (this.CurrencyNegativePattern > -1 && this.CurrencyNegativePattern < 15)
      culture.NumberFormat.CurrencyNegativePattern = this.CurrencyNegativePattern;
    if (this.CurrencyPositivePattern > -1 && this.CurrencyPositivePattern < 4)
      culture.NumberFormat.CurrencyPositivePattern = this.CurrencyPositivePattern;
    if (this.CurrencySymbol != null && this.Culture == CultureInfo.CurrentCulture)
      culture.NumberFormat.CurrencySymbol = this.CurrencySymbol;
    if (this.CurrencySymbol != string.Empty && this.CurrencySymbol != CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol)
      culture.NumberFormat.CurrencySymbol = this.CurrencySymbol;
    if (!this.GroupSeperatorEnabled)
      culture.NumberFormat.CurrencyGroupSeparator = string.Empty;
    return culture;
  }

  private void CheckIsExceedDecimalDigits()
  {
    if (this.CurrencyDecimalDigits < 0)
      this.IsExceedCurrencyDecimalDigits = true;
    else
      this.IsExceedCurrencyDecimalDigits = false;
  }

  private void UpdateCurrencyDecimalDigits()
  {
    if (this.CurrencyDecimalDigits >= 0)
      this.currencyDecimalDigits = this.CurrencyDecimalDigits;
    if (!this.Value.HasValue || !this.IsExceedCurrencyDecimalDigits)
      return;
    this.UpdateCurrencyDecimalDigits(CurrencyTextBox.CountDecimalDigits(this.Value.ToString(), (DependencyObject) this));
  }

  internal static int CountDecimalDigits(string p, DependencyObject d)
  {
    if (string.IsNullOrEmpty(p) || !(d is CurrencyTextBox))
      return 0;
    int num = 0;
    NumberFormatInfo numberFormat = ((CurrencyTextBox) d).GetCulture().NumberFormat;
    if (!string.IsNullOrEmpty(numberFormat.CurrencyDecimalSeparator) && p.Contains(numberFormat.CurrencyDecimalSeparator) || p.Contains(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator))
    {
      for (int index = p.Length - 1; index >= 0; --index)
      {
        if (numberFormat != null)
        {
          if (!(p[index].ToString() == numberFormat.CurrencyDecimalSeparator) && !(p[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator))
            ++num;
          else
            break;
        }
      }
    }
    return num;
  }

  internal void UpdateCurrencyDecimalDigits(int count)
  {
    if (this.MinimumCurrencyDecimalDigits >= 0 && this.MaximumCurrencyDecimalDigits >= 0 && this.MaximumCurrencyDecimalDigits > this.MinimumCurrencyDecimalDigits)
    {
      if (count >= this.MinimumCurrencyDecimalDigits && count <= this.MaximumCurrencyDecimalDigits)
        this.currencyDecimalDigits = count;
      else if (count <= this.MinimumCurrencyDecimalDigits)
      {
        this.currencyDecimalDigits = this.MinimumCurrencyDecimalDigits;
      }
      else
      {
        if (count <= this.MaximumCurrencyDecimalDigits)
          return;
        this.currencyDecimalDigits = this.MaximumCurrencyDecimalDigits;
      }
    }
    else if (this.MinimumCurrencyDecimalDigits >= 0 && this.MaximumCurrencyDecimalDigits >= 0 && this.MinimumCurrencyDecimalDigits >= this.MaximumCurrencyDecimalDigits)
      this.currencyDecimalDigits = this.MinimumCurrencyDecimalDigits;
    else if (this.MinimumCurrencyDecimalDigits >= 0)
    {
      if (count >= this.MinimumCurrencyDecimalDigits)
      {
        this.currencyDecimalDigits = count;
      }
      else
      {
        if (count >= this.MinimumCurrencyDecimalDigits)
          return;
        this.currencyDecimalDigits = this.MinimumCurrencyDecimalDigits;
      }
    }
    else if (this.MaximumCurrencyDecimalDigits >= 0)
    {
      if (count <= this.MaximumCurrencyDecimalDigits)
      {
        this.currencyDecimalDigits = count;
      }
      else
      {
        if (count <= this.MaximumCurrencyDecimalDigits)
          return;
        this.currencyDecimalDigits = this.MaximumCurrencyDecimalDigits;
      }
    }
    else
    {
      if (this.MaximumCurrencyDecimalDigits >= 0 || this.MinimumCurrencyDecimalDigits > 0 || this.CurrencyDecimalDigits >= 0)
        return;
      this.currencyDecimalDigits = count;
    }
  }

  private static object CoerceValue(DependencyObject d, object baseValue)
  {
    CurrencyTextBox currencyTextBox1 = (CurrencyTextBox) d;
    if (baseValue != null)
    {
      Decimal? nullable1 = (Decimal?) baseValue;
      bool? mValueChanged = currencyTextBox1.mValueChanged;
      if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) != 0)
      {
        Decimal? nullable2 = nullable1;
        Decimal maxValue = currencyTextBox1.MaxValue;
        if ((!(nullable2.GetValueOrDefault() > maxValue) ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 && currencyTextBox1.MaxValidation != MaxValidation.OnLostFocus)
        {
          nullable1 = new Decimal?(currencyTextBox1.MaxValue);
        }
        else
        {
          Decimal? nullable3 = nullable1;
          Decimal minValue = currencyTextBox1.MinValue;
          if ((!(nullable3.GetValueOrDefault() < minValue) ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && currencyTextBox1.MinValidation != MinValidation.OnLostFocus)
            nullable1 = new Decimal?(currencyTextBox1.MinValue);
        }
      }
      if (nullable1.HasValue)
      {
        CurrencyTextBox currencyTextBox2 = currencyTextBox1;
        Decimal? nullable4 = nullable1;
        int num1 = (!(nullable4.GetValueOrDefault() < 0M) ? 0 : (nullable4.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        currencyTextBox2.IsNegative = num1 != 0;
        CurrencyTextBox currencyTextBox3 = currencyTextBox1;
        Decimal? nullable5 = nullable1;
        int num2 = (!(nullable5.GetValueOrDefault() == 0M) ? 0 : (nullable5.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        currencyTextBox3.IsZero = num2 != 0;
        currencyTextBox1.IsNull = false;
      }
      return (object) nullable1;
    }
    if (currencyTextBox1.UseNullOption)
    {
      currencyTextBox1.IsNull = true;
      currencyTextBox1.IsNegative = false;
      currencyTextBox1.IsZero = false;
      return (object) currencyTextBox1.NullValue;
    }
    Decimal num = 0M;
    bool? mValueChanged1 = currencyTextBox1.mValueChanged;
    if ((!mValueChanged1.GetValueOrDefault() ? 0 : (mValueChanged1.HasValue ? 1 : 0)) != 0)
    {
      if (num > currencyTextBox1.MaxValue)
        num = currencyTextBox1.MaxValue;
      if (num < currencyTextBox1.MinValue)
        num = currencyTextBox1.MinValue;
    }
    currencyTextBox1.IsNegative = num < 0M;
    currencyTextBox1.IsZero = num == 0M;
    currencyTextBox1.IsNull = false;
    return (object) num;
  }

  private void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.AddHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted), true);
    this.mIsLoaded = true;
    Decimal? nullable1 = (Decimal?) CurrencyTextBox.CoerceValue((DependencyObject) this, (object) this.Value);
    if (this.IsNull && !this.IsFocused)
      this.WatermarkVisibility = Visibility.Visible;
    Decimal? nullable2 = nullable1;
    Decimal? nullable3 = this.Value;
    if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 1 : (nullable2.HasValue != nullable3.HasValue ? 1 : 0)) != 0)
      this.Value = nullable1;
    else
      this.FormatText();
  }

  public new void Dispose()
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    if (this.upButton != null && this.downButton != null)
    {
      this.upButton.Click -= new RoutedEventHandler(this.SpinButton_Click);
      this.downButton.Click -= new RoutedEventHandler(this.SpinButton_Click);
    }
    this.pastecommand = (ICommand) null;
    this.copycommand = (ICommand) null;
    this.cutcommand = (ICommand) null;
    this.currencySymbol = (string) null;
    this.mValue = new Decimal?();
    this.mValueChanged = new bool?();
    this.PART_ContentHost = (ScrollViewer) null;
    this.Focused_Border = (Border) null;
    this.OldValue = new Decimal?();
    this.upButton = (RepeatButton) null;
    this.downButton = (RepeatButton) null;
    BindingOperations.ClearAllBindings((DependencyObject) this);
    this.Loaded -= new RoutedEventHandler(this.IntegerTextbox_Loaded);
    base.Dispose();
    GC.Collect();
  }

  void IDisposable.Dispose() => this.Dispose();

  public Decimal? Value
  {
    get => (Decimal?) this.GetValue(CurrencyTextBox.ValueProperty);
    set => this.SetValue(CurrencyTextBox.ValueProperty, (object) value);
  }

  public Decimal MinValue
  {
    get => (Decimal) this.GetValue(CurrencyTextBox.MinValueProperty);
    set => this.SetValue(CurrencyTextBox.MinValueProperty, (object) value);
  }

  public Decimal MaxValue
  {
    get => (Decimal) this.GetValue(CurrencyTextBox.MaxValueProperty);
    set => this.SetValue(CurrencyTextBox.MaxValueProperty, (object) value);
  }

  public int CurrencyDecimalDigits
  {
    get => (int) this.GetValue(CurrencyTextBox.CurrencyDecimalDigitsProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyDecimalDigitsProperty, (object) value);
  }

  public string CurrencyDecimalSeparator
  {
    get => (string) this.GetValue(CurrencyTextBox.CurrencyDecimalSeparatorProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyDecimalSeparatorProperty, (object) value);
  }

  public string CurrencyGroupSeparator
  {
    get => (string) this.GetValue(CurrencyTextBox.CurrencyGroupSeparatorProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyGroupSeparatorProperty, (object) value);
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(CurrencyTextBox.GroupSeperatorEnabledProperty);
    set => this.SetValue(CurrencyTextBox.GroupSeperatorEnabledProperty, (object) value);
  }

  public Int32Collection CurrencyGroupSizes
  {
    get => (Int32Collection) this.GetValue(CurrencyTextBox.CurrencyGroupSizesProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyGroupSizesProperty, (object) value);
  }

  public int CurrencyNegativePattern
  {
    get => (int) this.GetValue(CurrencyTextBox.CurrencyNegativePatternProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyNegativePatternProperty, (object) value);
  }

  public int CurrencyPositivePattern
  {
    get => (int) this.GetValue(CurrencyTextBox.CurrencyPositivePatternProperty);
    set => this.SetValue(CurrencyTextBox.CurrencyPositivePatternProperty, (object) value);
  }

  public string CurrencySymbol
  {
    get => (string) this.GetValue(CurrencyTextBox.CurrencySymbolProperty);
    set => this.SetValue(CurrencyTextBox.CurrencySymbolProperty, (object) value);
  }

  public double ScrollInterval
  {
    get => (double) this.GetValue(CurrencyTextBox.ScrollIntervalProperty);
    set => this.SetValue(CurrencyTextBox.ScrollIntervalProperty, (object) value);
  }

  public int MaximumCurrencyDecimalDigits
  {
    get => (int) this.GetValue(CurrencyTextBox.MaximumCurrencyDecimalDigitsProperty);
    set => this.SetValue(CurrencyTextBox.MaximumCurrencyDecimalDigitsProperty, (object) value);
  }

  public int MinimumCurrencyDecimalDigits
  {
    get => (int) this.GetValue(CurrencyTextBox.MinimumCurrencyDecimalDigitsProperty);
    set => this.SetValue(CurrencyTextBox.MinimumCurrencyDecimalDigitsProperty, (object) value);
  }

  public static void OnCurrencyDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyDecimalDigitsChanged(args);
  }

  protected void OnCurrencyDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateCurrencyDecimalDigits();
    if (this.CurrencyDecimalDigitsChanged != null)
      this.CurrencyDecimalDigitsChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnCurrencyDecimalSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyDecimalSeparatorChanged(args);
  }

  protected void OnCurrencyDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencyDecimalSeparatorChanged != null)
      this.CurrencyDecimalSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnCurrencyGroupSizesChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyGroupSizesChanged(args);
  }

  protected void OnCurrencyGroupSizesChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencyGroupSizesChanged != null)
      this.CurrencyGroupSizesChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private static object CoerceCurrencyGroupSeperator(DependencyObject d, object baseValue)
  {
    NumberFormatInfo numberFormat = ((CurrencyTextBox) d).GetCulture().NumberFormat;
    return !baseValue.Equals((object) string.Empty) && !char.IsLetterOrDigit(baseValue.ToString(), 0) || baseValue.Equals((object) string.Empty) ? baseValue : (object) numberFormat.CurrencyGroupSeparator;
  }

  public static void OnCurrencyGroupSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyGroupSeparatorChanged(args);
  }

  protected void OnCurrencyGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencyGroupSeparatorChanged != null)
      this.CurrencyGroupSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnCurrencyNegativePatternChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyNegativePatternChanged(args);
  }

  protected void OnCurrencyNegativePatternChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencyNegativePatternChanged != null)
      this.CurrencyNegativePatternChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnCurrencyPositivePatternChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencyPositivePatternChanged(args);
  }

  protected void OnCurrencyPositivePatternChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencyPositivePatternChanged != null)
      this.CurrencyPositivePatternChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnCurrencySymbolChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencySymbolChanged(args);
  }

  protected void OnCurrencySymbolChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencySymbolChanged != null)
      this.CurrencySymbolChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnValueChanged(args);
  }

  protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
    {
      this.CheckIsExceedDecimalDigits();
      this.UpdateCurrencyDecimalDigits();
    }
    if (this.Value.HasValue)
    {
      Decimal? nullable1 = this.Value;
      this.IsNegative = (!(nullable1.GetValueOrDefault() < 0M) ? 0 : (nullable1.HasValue ? 1 : 0)) != 0;
      Decimal? nullable2 = this.Value;
      this.IsZero = (!(nullable2.GetValueOrDefault() == 0M) ? 0 : (nullable2.HasValue ? 1 : 0)) != 0;
      this.IsNull = false;
      if (this.MaxLength != 0 && args.NewValue.ToString().Length > this.MaxLength && this.IsValueChanged)
        CurrencyValueHandler.currencyValueHandler.MatchWithMask(this, this.Value.ToString());
    }
    else if (this.UseNullOption)
    {
      this.IsNull = true;
      this.IsNegative = false;
      this.IsZero = false;
    }
    this.OldValue = (Decimal?) args.OldValue;
    this.mValue = this.Value;
    if (this.ValueChanged != null)
      this.ValueChanged((DependencyObject) this, args);
    if (this.Value.HasValue)
      this.WatermarkVisibility = Visibility.Collapsed;
    Decimal? nullable = this.Value;
    Decimal minValue = this.MinValue;
    if ((!(nullable.GetValueOrDefault() > minValue) ? 0 : (nullable.HasValue ? 1 : 0)) != 0 && this.MinValidation == MinValidation.OnKeyPress)
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
    ((CurrencyTextBox) obj)?.OnMinValueChanged(args);
  }

  protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinValueChanged != null)
      this.MinValueChanged((DependencyObject) this, args);
    if (this.MaxValue < this.MinValue)
      this.MaxValue = this.MinValue;
    Decimal? nullable1 = this.Value;
    Decimal? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
      return;
    this.Value = this.ValidateValue(this.Value);
  }

  public static void OnMaxValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnMaxValueChanged(args);
  }

  protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxValueChanged != null)
      this.MaxValueChanged((DependencyObject) this, args);
    if (this.MinValue > this.MaxValue)
      this.MinValue = this.MaxValue;
    Decimal? nullable1 = this.Value;
    Decimal? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
      return;
    this.Value = this.ValidateValue(this.Value);
  }

  private static void OnMaximumCurrencyDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnMaximumCurrencyDecimalDigitsChanged(args);
  }

  protected void OnMaximumCurrencyDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateCurrencyDecimalDigits();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private static void OnMinimumCurrencyDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnMinimumCurrencyDecimalDigitsChanged(args);
  }

  protected void OnMinimumCurrencyDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateCurrencyDecimalDigits();
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  public CurrencySymbolPosition CurrencySymbolPosition
  {
    get => (CurrencySymbolPosition) this.GetValue(CurrencyTextBox.CurrencySymbolPositionProperty);
    set => this.SetValue(CurrencyTextBox.CurrencySymbolPositionProperty, (object) value);
  }

  public static void OnCurrencySymbolPositionChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnCurrencySymbolPositionChanged(args);
  }

  protected void OnCurrencySymbolPositionChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencySymbolPositionChanged != null)
      this.CurrencySymbolPositionChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public Decimal? NullValue
  {
    get => (Decimal?) this.GetValue(CurrencyTextBox.NullValueProperty);
    set => this.SetValue(CurrencyTextBox.NullValueProperty, (object) value);
  }

  public static void OnNullValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnNullValueChanged(args);
  }

  protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public StringValidation ValueValidation
  {
    get => (StringValidation) this.GetValue(CurrencyTextBox.ValueValidationProperty);
    set => this.SetValue(CurrencyTextBox.ValueValidationProperty, (object) value);
  }

  public InvalidInputBehavior InvalidValueBehavior
  {
    get => (InvalidInputBehavior) this.GetValue(CurrencyTextBox.InvalidValueBehaviorProperty);
    set => this.SetValue(CurrencyTextBox.InvalidValueBehaviorProperty, (object) value);
  }

  public static void OnInvalidValueBehaviorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnInvalidValueBehaviorChanged(args);
  }

  protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.InvalidValueBehaviorChanged == null)
      return;
    this.InvalidValueBehaviorChanged((DependencyObject) this, args);
  }

  public string ValidationValue
  {
    get => (string) this.GetValue(CurrencyTextBox.ValidationValueProperty);
    set => this.SetValue(CurrencyTextBox.ValidationValueProperty, (object) value);
  }

  public static void OnValidationValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnValidationValueChanged(args);
  }

  protected void OnValidationValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationValueChanged == null)
      return;
    this.ValidationValueChanged((DependencyObject) this, args);
  }

  public bool ValidationCompleted
  {
    get => (bool) this.GetValue(CurrencyTextBox.ValidationCompletedProperty);
    set => this.SetValue(CurrencyTextBox.ValidationCompletedProperty, (object) value);
  }

  public static void OnValidationCompletedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CurrencyTextBox) obj)?.OnValidationCompletedPropertyChanged(args);
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
