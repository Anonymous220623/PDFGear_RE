// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.IntegerTextBox
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (IntegerTextBox), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Editors/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (IntegerTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/TransparentStyle.xaml")]
public class IntegerTextBox : EditorBase, IDisposable
{
  internal long? OldValue;
  internal long? mValue;
  internal bool? mValueChanged = new bool?(true);
  internal bool mIsLoaded;
  internal int count = 1;
  internal string checktext = "";
  private RepeatButton upButton;
  private RepeatButton downButton;
  private ScrollViewer PART_ContentHost;
  private bool _validatingrResult;
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (IntegerTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSeparatorChanged)));
  public static readonly DependencyProperty NumberGroupSizesProperty = DependencyProperty.Register(nameof (NumberGroupSizes), typeof (Int32Collection), typeof (IntegerTextBox), new PropertyMetadata((object) new Int32Collection(), new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSizesChanged)));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (long?), typeof (IntegerTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(IntegerTextBox.OnValueChanged), new CoerceValueCallback(IntegerTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (long), typeof (IntegerTextBox), new PropertyMetadata((object) long.MinValue, new PropertyChangedCallback(IntegerTextBox.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (long), typeof (IntegerTextBox), new PropertyMetadata((object) long.MaxValue, new PropertyChangedCallback(IntegerTextBox.OnMaxValueChanged)));
  public static readonly DependencyProperty NumberGroupSeparatorProperty = DependencyProperty.Register(nameof (NumberGroupSeparator), typeof (string), typeof (IntegerTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSeparatorChanged), new CoerceValueCallback(IntegerTextBox.CoerceNumberGroupSeperator)));
  public static readonly DependencyProperty ScrollIntervalProperty = DependencyProperty.Register(nameof (ScrollInterval), typeof (int), typeof (IntegerTextBox), new PropertyMetadata((object) 1));
  internal double percentage;
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (long?), typeof (IntegerTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(IntegerTextBox.OnNullValueChanged)));
  public static readonly DependencyProperty ValueValidationProperty = DependencyProperty.Register(nameof (ValueValidation), typeof (StringValidation), typeof (IntegerTextBox), new PropertyMetadata((object) StringValidation.OnLostFocus));
  public static readonly DependencyProperty InvalidValueBehaviorProperty = DependencyProperty.Register(nameof (InvalidValueBehavior), typeof (InvalidInputBehavior), typeof (IntegerTextBox), new PropertyMetadata((object) InvalidInputBehavior.None, new PropertyChangedCallback(IntegerTextBox.OnInvalidValueBehaviorChanged)));
  public static readonly DependencyProperty ValidationValueProperty = DependencyProperty.Register(nameof (ValidationValue), typeof (string), typeof (IntegerTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(IntegerTextBox.OnValidationValueChanged)));
  public static readonly DependencyProperty ValidationCompletedProperty = DependencyProperty.Register(nameof (ValidationCompleted), typeof (bool), typeof (IntegerTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(IntegerTextBox.OnValidationCompletedPropertyChanged)));

  public event PropertyChangedCallback ValidationCompletedChanged;

  public event PropertyChangedCallback InvalidValueBehaviorChanged;

  public event StringValidationCompletedEventHandler ValueValidationCompleted;

  public event PropertyChangedCallback ValidationValueChanged;

  public event CancelEventHandler Validating;

  public event EventHandler Validated;

  public event PropertyChangedCallback MinValueChanged;

  public event PropertyChangedCallback ValueChanged;

  public event PropertyChangedCallback MaxValueChanged;

  public event PropertyChangedCallback NumberGroupSizesChanged;

  public event PropertyChangedCallback NumberGroupSeparatorChanged;

  static IntegerTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (IntegerTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (IntegerTextBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ICommand pastecommand { get; private set; }

  public ICommand copycommand { get; private set; }

  public ICommand cutcommand { get; private set; }

  public IntegerTextBox()
  {
    this.pastecommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
    this.copycommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
    this.cutcommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
    this.Loaded -= new RoutedEventHandler(this.IntegerTextbox_Loaded);
    this.Loaded += new RoutedEventHandler(this.IntegerTextbox_Loaded);
    this.Unloaded += new RoutedEventHandler(this.IntegerTextBox_Unloaded);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void IntegerTextBox_Unloaded(object sender, RoutedEventArgs e)
  {
    this.Unloaded -= new RoutedEventHandler(this.IntegerTextBox_Unloaded);
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
  }

  public new void Dispose()
  {
    this.Unloaded -= new RoutedEventHandler(this.IntegerTextBox_Unloaded);
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.pastecommand = (ICommand) null;
    this.copycommand = (ICommand) null;
    this.cutcommand = (ICommand) null;
    base.Dispose();
  }

  void IDisposable.Dispose() => this.Dispose();

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

  private void copy()
  {
    try
    {
      Clipboard.SetText(this.SelectedText);
    }
    catch (SecurityException ex)
    {
    }
    catch (COMException ex)
    {
    }
  }

  private new void Paste()
  {
    if (this.IsReadOnly)
      return;
    try
    {
      string str1 = Clipboard.GetText();
      int selectionStart = this.SelectionStart;
      this.GetType().ToString();
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      if (!this.UseNullOption && this.Value.HasValue)
      {
        long num1 = this.Value.Value;
      }
      bool flag1 = str1.StartsWith("-");
      bool flag2 = false;
      string empty = string.Empty;
      if (this.NumberGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator && str1.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) && str1.IndexOf(this.NumberGroupSeparator) >= 0)
      {
        int startIndex = str1.IndexOf(this.NumberGroupSeparator);
        str1 = str1.Remove(startIndex);
      }
      for (int index = 0; index < str1.Length; ++index)
      {
        if (!char.IsDigit(str1[index]))
        {
          str1 = str1.Remove(index, 1);
          --index;
        }
      }
      if (str1 == string.Empty)
        return;
      string s;
      if (this.PasteMode != PasteMode.Default)
      {
        long? nullable = this.Value;
        if ((nullable.GetValueOrDefault() != 0L ? 0 : (nullable.HasValue ? 1 : 0)) == 0 && this.Value.HasValue && this.Text.Length != this.SelectedText.Length)
        {
          s = this.Text.Remove(this.SelectionStart, this.SelectedText.Length).Insert(this.SelectionStart, str1);
          goto label_15;
        }
      }
      flag2 = true;
      s = str1;
label_15:
      if (!string.IsNullOrEmpty(s))
        s = !flag1 || s.StartsWith("-") ? s : "-" + s;
      if (numberFormat.NumberGroupSeparator != string.Empty && numberFormat.NumberGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
        s = s.Replace(numberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
      double result;
      double.TryParse(s, out result);
      if (result > (double) this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress || result < (double) this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
      {
        if (result > (double) this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress && this.MaxValueOnExceedMaxDigit)
        {
          result = (double) this.MaxValue;
          this.SetValue(new bool?(false), new long?((long) result));
          this.Text = result.ToString("N", (IFormatProvider) numberFormat);
        }
        if (result >= (double) this.MinValue || this.MinValidation != MinValidation.OnKeyPress || !this.MinValueOnExceedMinDigit)
          return;
        double minValue = (double) this.MinValue;
        this.SetValue(new bool?(false), new long?((long) minValue));
        this.Text = minValue.ToString("N", (IFormatProvider) numberFormat);
      }
      else
      {
        string str2 = Convert.ToInt64(result).ToString();
        if (this.MaxLength > 0 && str2.Length > this.MaxLength)
          result = Convert.ToDouble(str2.Remove(this.MaxLength));
        this.SetValue(new bool?(false), new long?((long) result));
        this.Text = result.ToString("N", (IFormatProvider) numberFormat);
        int index = 0;
        int num2;
        for (num2 = flag2 ? this.Text.Length : selectionStart; num2 < this.Text.Length && index < str1.Length; ++num2)
        {
          if ((int) this.Text[num2] == (int) str1[index])
            ++index;
        }
        this.CaretIndex = num2;
        this.Select(num2, 0);
      }
    }
    catch (SecurityException ex)
    {
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
    long? nullable1 = new long?(0L);
    if (canIncrement)
    {
      long? nullable2 = this.Value;
      long maxValue = this.MaxValue;
      if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        return;
    }
    if (!canIncrement)
    {
      long? nullable3 = this.Value;
      long minValue = this.MinValue;
      if ((nullable3.GetValueOrDefault() >= minValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
        return;
    }
    long? nullable4;
    if (!canIncrement)
    {
      long? nullable5 = this.Value;
      long scrollInterval = (long) this.ScrollInterval;
      nullable4 = nullable5.HasValue ? new long?(nullable5.GetValueOrDefault() - scrollInterval) : new long?();
    }
    else
    {
      long? nullable6 = this.Value;
      long scrollInterval = (long) this.ScrollInterval;
      nullable4 = nullable6.HasValue ? new long?(nullable6.GetValueOrDefault() + scrollInterval) : new long?();
    }
    this.SetValue(new bool?(true), nullable4);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (!this.IsScrollingOnCircle)
      return;
    e.Handled = true;
    if (e.Delta > 0)
    {
      IntegerValueHandler.integerValueHandler.HandleUpDownKey(this, true);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      IntegerValueHandler.integerValueHandler.HandleUpDownKey(this, false);
    }
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    e.Handled = IntegerValueHandler.integerValueHandler.HandleKeyDown(this, e);
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
      if (e.Key == Key.C)
        this.copy();
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
      e.Handled = IntegerValueHandler.integerValueHandler.HandleKeyDown(this, e);
    base.OnKeyDown(e);
  }

  private void cut()
  {
    if (this.SelectionLength <= 0)
      return;
    try
    {
      Clipboard.SetText(this.SelectedText);
      this.count = 1;
      IntegerValueHandler.integerValueHandler.HandleDeleteKey(this);
    }
    catch (SecurityException ex)
    {
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
    e.Handled = IntegerValueHandler.integerValueHandler.MatchWithMask(this, e.Text);
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
          this.SetCurrentValue(IntegerTextBox.ValueProperty, (object) null);
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
    long? nullable1 = this.Value;
    if (nullable1.HasValue)
    {
      long? nullable2 = nullable1;
      long maxValue = this.MaxValue;
      if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
      {
        nullable1 = new long?(this.MaxValue);
      }
      else
      {
        long? mValue = this.mValue;
        long minValue = this.MinValue;
        if ((mValue.GetValueOrDefault() >= minValue ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          nullable1 = new long?(this.MinValue);
      }
      long? nullable3 = nullable1;
      long? nullable4 = this.Value;
      if ((nullable3.GetValueOrDefault() != nullable4.GetValueOrDefault() ? 1 : (nullable3.HasValue != nullable4.HasValue ? 1 : 0)) != 0)
        this.Value = nullable1;
    }
    base.OnLostFocus(e);
    if (string.IsNullOrEmpty(this.MaskedText) || !this.UseNullOption || !this.IsNull)
      return;
    this.MaskedText = string.Empty;
    this.checktext = "";
    this.minusPressed = false;
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
    this.InvalidateProperty(IntegerTextBox.ValueProperty);
    this.IsNull = false;
  }

  internal void FormatText()
  {
    if (this.Value.HasValue)
      this.MaskedText = this.mValue.Value.ToString("N", (IFormatProvider) this.GetCulture().NumberFormat);
    else
      this.MaskedText = "";
  }

  internal void SetValue(bool? IsReload, long? _Value)
  {
    bool? nullable1 = IsReload;
    if ((nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
    {
      this.mValueChanged = new bool?(false);
      this.Value = _Value;
      this.mValueChanged = new bool?(true);
    }
    else
    {
      bool? nullable2 = IsReload;
      if ((!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
        return;
      int caretIndex = this.CaretIndex;
      this.SetCurrentValue(IntegerTextBox.ValueProperty, (object) _Value);
      this.CaretIndex = caretIndex;
    }
  }

  internal long? ValidateValue(long? Val)
  {
    if (Val.HasValue)
    {
      long? nullable = Val;
      long maxValue = this.MaxValue;
      if ((nullable.GetValueOrDefault() <= maxValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        Val = new long?(this.MaxValue);
      }
      else
      {
        long? mValue = this.mValue;
        long minValue = this.MinValue;
        if ((mValue.GetValueOrDefault() >= minValue ? 0 : (mValue.HasValue ? 1 : 0)) != 0)
          Val = new long?(this.MinValue);
      }
    }
    return Val;
  }

  internal CultureInfo GetCulture()
  {
    CultureInfo culture = this.Culture == null || this.Culture == CultureInfo.InvariantCulture ? CultureInfo.CurrentCulture.Clone() as CultureInfo : this.Culture.Clone() as CultureInfo;
    if (this.NumberFormat != null)
      culture.NumberFormat = this.NumberFormat;
    culture.NumberFormat.NumberDecimalDigits = 0;
    if (!this.GroupSeperatorEnabled)
      culture.NumberFormat.NumberGroupSeparator = string.Empty;
    if (this.GroupSeperatorEnabled && !this.NumberGroupSeparator.Equals(string.Empty))
      culture.NumberFormat.NumberGroupSeparator = this.NumberGroupSeparator;
    int count = this.NumberGroupSizes.Count;
    if (count > 0)
    {
      int[] numArray = new int[count];
      for (int index = 0; index < count; ++index)
        numArray[index] = this.NumberGroupSizes[index];
      culture.NumberFormat.NumberGroupSizes = numArray;
    }
    return culture;
  }

  private static object CoerceValue(DependencyObject d, object baseValue)
  {
    IntegerTextBox integerTextBox1 = (IntegerTextBox) d;
    if (baseValue != null)
    {
      long? nullable1 = (long?) baseValue;
      bool? mValueChanged = integerTextBox1.mValueChanged;
      if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) != 0)
      {
        long? nullable2 = nullable1;
        long maxValue = integerTextBox1.MaxValue;
        if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 && !integerTextBox1.IsFocused && integerTextBox1.MaxValidation != MaxValidation.OnLostFocus)
          nullable1 = new long?(integerTextBox1.MaxValue);
        long? nullable3 = nullable1;
        long minValue = integerTextBox1.MinValue;
        if ((nullable3.GetValueOrDefault() >= minValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && !integerTextBox1.IsFocused && integerTextBox1.MinValidation != MinValidation.OnLostFocus)
          nullable1 = new long?(integerTextBox1.MinValue);
      }
      if (nullable1.HasValue)
      {
        IntegerTextBox integerTextBox2 = integerTextBox1;
        long? nullable4 = nullable1;
        int num1 = (nullable4.GetValueOrDefault() >= 0L ? 0 : (nullable4.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        integerTextBox2.IsNegative = num1 != 0;
        IntegerTextBox integerTextBox3 = integerTextBox1;
        long? nullable5 = nullable1;
        int num2 = (nullable5.GetValueOrDefault() != 0L ? 0 : (nullable5.HasValue ? 1 : 0)) != 0 ? 1 : 0;
        integerTextBox3.IsZero = num2 != 0;
        integerTextBox1.IsNull = false;
      }
      return (object) nullable1;
    }
    if (integerTextBox1.UseNullOption)
    {
      integerTextBox1.IsNull = true;
      integerTextBox1.IsNegative = false;
      integerTextBox1.IsZero = false;
      return (object) integerTextBox1.NullValue;
    }
    long num = 0;
    bool? mValueChanged1 = integerTextBox1.mValueChanged;
    if ((!mValueChanged1.GetValueOrDefault() ? 0 : (mValueChanged1.HasValue ? 1 : 0)) != 0)
    {
      if (num > integerTextBox1.MaxValue)
        num = integerTextBox1.MaxValue;
      if (num < integerTextBox1.MinValue)
        num = integerTextBox1.MinValue;
    }
    integerTextBox1.IsNegative = num < 0L;
    integerTextBox1.IsZero = num == 0L;
    integerTextBox1.IsNull = false;
    return (object) num;
  }

  private void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.AddHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted), true);
    this.mIsLoaded = true;
    long? nullable1 = (long?) IntegerTextBox.CoerceValue((DependencyObject) this, (object) this.Value);
    if (this.IsNull && !this.IsFocused)
      this.WatermarkVisibility = Visibility.Visible;
    long? nullable2 = nullable1;
    long? nullable3 = this.Value;
    if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 1 : (nullable2.HasValue != nullable3.HasValue ? 1 : 0)) != 0)
    {
      this.Value = nullable1;
    }
    else
    {
      this.mValue = this.Value;
      this.FormatText();
    }
  }

  public long? Value
  {
    get => (long?) this.GetValue(IntegerTextBox.ValueProperty);
    set => this.SetValue(IntegerTextBox.ValueProperty, (object) value);
  }

  public long MinValue
  {
    get => (long) this.GetValue(IntegerTextBox.MinValueProperty);
    set => this.SetValue(IntegerTextBox.MinValueProperty, (object) value);
  }

  public long MaxValue
  {
    get => (long) this.GetValue(IntegerTextBox.MaxValueProperty);
    set => this.SetValue(IntegerTextBox.MaxValueProperty, (object) value);
  }

  public string NumberGroupSeparator
  {
    get => (string) this.GetValue(IntegerTextBox.NumberGroupSeparatorProperty);
    set => this.SetValue(IntegerTextBox.NumberGroupSeparatorProperty, (object) value);
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(IntegerTextBox.GroupSeperatorEnabledProperty);
    set => this.SetValue(IntegerTextBox.GroupSeperatorEnabledProperty, (object) value);
  }

  public Int32Collection NumberGroupSizes
  {
    get => (Int32Collection) this.GetValue(IntegerTextBox.NumberGroupSizesProperty);
    set => this.SetValue(IntegerTextBox.NumberGroupSizesProperty, (object) value);
  }

  public double ProgressFactor => this.percentage / this.ActualWidth;

  public int ScrollInterval
  {
    get => (int) this.GetValue(IntegerTextBox.ScrollIntervalProperty);
    set => this.SetValue(IntegerTextBox.ScrollIntervalProperty, (object) value);
  }

  public static void OnNumberGroupSizesChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    IntegerTextBox integerTextBox = (IntegerTextBox) obj;
    if (integerTextBox == null || integerTextBox == null)
      return;
    integerTextBox.OnNumberGroupSizesChanged(args);
  }

  protected void OnNumberGroupSizesChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NumberGroupSizesChanged != null)
      this.NumberGroupSizesChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnValueChanged(args);
  }

  protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Value.HasValue)
    {
      long? nullable1 = this.Value;
      this.IsNegative = (nullable1.GetValueOrDefault() >= 0L ? 0 : (nullable1.HasValue ? 1 : 0)) != 0;
      long? nullable2 = this.Value;
      this.IsZero = (nullable2.GetValueOrDefault() != 0L ? 0 : (nullable2.HasValue ? 1 : 0)) != 0;
      this.IsNull = false;
    }
    else if (this.UseNullOption)
    {
      this.IsNull = true;
      this.IsNegative = false;
      this.IsZero = false;
    }
    this.OldValue = (long?) args.OldValue;
    this.mValue = this.Value;
    if (this.Value.HasValue)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (this.ValueChanged != null)
      this.ValueChanged((DependencyObject) this, args);
    long? nullable = this.Value;
    long minValue = this.MinValue;
    if ((nullable.GetValueOrDefault() <= minValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0 && this.MinValidation == MinValidation.OnKeyPress)
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
    ((IntegerTextBox) obj)?.OnMinValueChanged(args);
  }

  protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinValueChanged != null)
      this.MinValueChanged((DependencyObject) this, args);
    if (this.MaxValue < this.MinValue)
      this.MaxValue = this.MinValue;
    if (this.UseNullOption && this.Value.HasValue && this.MinValue != 0L)
    {
      long? nullable = this.Value;
      long minValue = this.MinValue;
      if ((nullable.GetValueOrDefault() >= minValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        this.Value = new long?(this.MinValue);
    }
    long? nullable1 = this.Value;
    long? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) != 0)
      this.Value = this.ValidateValue(this.Value);
    if (!this.Value.HasValue)
      return;
    long? nullable3 = this.Value;
    double num = this.ActualWidth / (double) this.MaxValue;
    this.percentage = (nullable3.HasValue ? new double?((double) nullable3.GetValueOrDefault() * num) : new double?()).Value;
  }

  public static void OnMaxValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnMaxValueChanged(args);
  }

  protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxValueChanged != null)
      this.MaxValueChanged((DependencyObject) this, args);
    if (this.MinValue > this.MaxValue)
      this.MinValue = this.MaxValue;
    long? nullable1 = this.Value;
    long? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
      return;
    this.Value = this.ValidateValue(this.Value);
  }

  private static void OnNumberGroupSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnNumberGroupSeparatorChanged(args);
  }

  private static object CoerceNumberGroupSeperator(DependencyObject d, object baseValue)
  {
    NumberFormatInfo numberFormat = ((IntegerTextBox) d).GetCulture().NumberFormat;
    return !baseValue.Equals((object) string.Empty) && !char.IsLetterOrDigit(baseValue.ToString(), 0) || baseValue.Equals((object) string.Empty) ? baseValue : (object) numberFormat.NumberGroupSeparator;
  }

  protected virtual void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.NumberGroupSeparatorChanged != null)
      this.NumberGroupSeparatorChanged((DependencyObject) this, e);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public long? NullValue
  {
    get => (long?) this.GetValue(IntegerTextBox.NullValueProperty);
    set => this.SetValue(IntegerTextBox.NullValueProperty, (object) value);
  }

  public static void OnNullValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnNullValueChanged(args);
  }

  protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
  {
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

  public StringValidation ValueValidation
  {
    get => (StringValidation) this.GetValue(IntegerTextBox.ValueValidationProperty);
    set => this.SetValue(IntegerTextBox.ValueValidationProperty, (object) value);
  }

  public InvalidInputBehavior InvalidValueBehavior
  {
    get => (InvalidInputBehavior) this.GetValue(IntegerTextBox.InvalidValueBehaviorProperty);
    set => this.SetValue(IntegerTextBox.InvalidValueBehaviorProperty, (object) value);
  }

  public static void OnInvalidValueBehaviorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnInvalidValueBehaviorChanged(args);
  }

  protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.InvalidValueBehaviorChanged == null)
      return;
    this.InvalidValueBehaviorChanged((DependencyObject) this, args);
  }

  public string ValidationValue
  {
    get => (string) this.GetValue(IntegerTextBox.ValidationValueProperty);
    set => this.SetValue(IntegerTextBox.ValidationValueProperty, (object) value);
  }

  public static void OnValidationValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnValidationValueChanged(args);
  }

  protected void OnValidationValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationValueChanged == null)
      return;
    this.ValidationValueChanged((DependencyObject) this, args);
  }

  public bool ValidationCompleted
  {
    get => (bool) this.GetValue(IntegerTextBox.ValidationCompletedProperty);
    set => this.SetValue(IntegerTextBox.ValidationCompletedProperty, (object) value);
  }

  public static void OnValidationCompletedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((IntegerTextBox) obj)?.OnValidationCompletedPropertyChanged(args);
  }

  protected void OnValidationCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationCompletedChanged == null)
      return;
    this.ValidationCompletedChanged((DependencyObject) this, args);
  }
}
