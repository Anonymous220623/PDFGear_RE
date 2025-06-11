// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DoubleTextBox
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared.Controls.Editors.AutomationPeer;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (DoubleTextBox), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Editors/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (DoubleTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2003Style.xaml")]
public class DoubleTextBox : EditorBase, IDisposable
{
  internal double? OldValue;
  internal double? mValue;
  internal string utext;
  internal int uval;
  internal bool? mValueChanged = new bool?(true);
  internal bool mIsLoaded;
  internal int count = 1;
  internal bool negativeFlag;
  internal string checktext = "";
  internal int numberDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
  internal string lostfocusmasktext;
  internal bool isUpDownDoubleTextBox;
  private RepeatButton upButton;
  private RepeatButton downButton;
  private ScrollViewer PART_ContentHost;
  private bool _validatingrResult;
  private bool MinMaxchanged;
  public static readonly DependencyProperty NumberGroupSizesProperty = DependencyProperty.Register(nameof (NumberGroupSizes), typeof (Int32Collection), typeof (DoubleTextBox), new PropertyMetadata((object) new Int32Collection(), new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSizesChanged)));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double?), typeof (DoubleTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DoubleTextBox.OnValueChanged), new CoerceValueCallback(DoubleTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
  public static readonly DependencyProperty MinimumNumberDecimalDigitsProperty = DependencyProperty.Register(nameof (MinimumNumberDecimalDigits), typeof (int), typeof (DoubleTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(DoubleTextBox.OnMinimumNumberDecimalDigitsChanged)));
  public static readonly DependencyProperty MaximumNumberDecimalDigitsProperty = DependencyProperty.Register(nameof (MaximumNumberDecimalDigits), typeof (int), typeof (DoubleTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(DoubleTextBox.OnMaximumNumberDecimalDigitsChanged)));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (double), typeof (DoubleTextBox), new PropertyMetadata((object) double.NegativeInfinity, new PropertyChangedCallback(DoubleTextBox.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (double), typeof (DoubleTextBox), new PropertyMetadata((object) double.PositiveInfinity, new PropertyChangedCallback(DoubleTextBox.OnMaxValueChanged)));
  public static readonly DependencyProperty NumberGroupSeparatorProperty = DependencyProperty.Register(nameof (NumberGroupSeparator), typeof (string), typeof (DoubleTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSeparatorChanged), new CoerceValueCallback(DoubleTextBox.CoerceNumberGroupSeperator)));
  public static readonly DependencyProperty NumberDecimalDigitsProperty = DependencyProperty.Register(nameof (NumberDecimalDigits), typeof (int), typeof (DoubleTextBox), new PropertyMetadata((object) -1, new PropertyChangedCallback(DoubleTextBox.OnNumberDecimalDigitsChanged)));
  public static readonly DependencyProperty NumberDecimalSeparatorProperty = DependencyProperty.Register(nameof (NumberDecimalSeparator), typeof (string), typeof (DoubleTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(DoubleTextBox.OnNumberDecimalSeparatorChanged)));
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (DoubleTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSeparatorChanged)));
  internal static readonly DependencyProperty IsExceedDecimalDigitsProperty = DependencyProperty.Register(nameof (IsExceedDecimalDigits), typeof (bool), typeof (DoubleTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(DoubleTextBox.OnIsExceedDecimalDigits)));
  public static readonly DependencyProperty ScrollIntervalProperty = DependencyProperty.Register(nameof (ScrollInterval), typeof (double), typeof (DoubleTextBox), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof (Step), typeof (double), typeof (DoubleTextBox), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (double?), typeof (DoubleTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(DoubleTextBox.OnNullValueChanged)));
  public static readonly DependencyProperty ValueValidationProperty = DependencyProperty.Register(nameof (ValueValidation), typeof (StringValidation), typeof (DoubleTextBox), new PropertyMetadata((object) StringValidation.OnLostFocus));
  public static readonly DependencyProperty InvalidValueBehaviorProperty = DependencyProperty.Register(nameof (InvalidValueBehavior), typeof (InvalidInputBehavior), typeof (DoubleTextBox), new PropertyMetadata((object) InvalidInputBehavior.None, new PropertyChangedCallback(DoubleTextBox.OnInvalidValueBehaviorChanged)));
  public static readonly DependencyProperty ValidationValueProperty = DependencyProperty.Register(nameof (ValidationValue), typeof (string), typeof (DoubleTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(DoubleTextBox.OnValidationValueChanged)));
  public static readonly DependencyProperty ValidationCompletedProperty = DependencyProperty.Register(nameof (ValidationCompleted), typeof (bool), typeof (DoubleTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(DoubleTextBox.OnValidationCompletedPropertyChanged)));

  public event PropertyChangedCallback ValidationCompletedChanged;

  public event PropertyChangedCallback InvalidValueBehaviorChanged;

  public event StringValidationCompletedEventHandler ValueValidationCompleted;

  public event PropertyChangedCallback ValidationValueChanged;

  public event CancelEventHandler Validating;

  public event EventHandler Validated;

  public event PropertyChangedCallback MinValueChanged;

  public event PropertyChangedCallback ValueChanged;

  public event DoubleTextBox.ValueChangingEventHandler ValueChanging;

  public event PropertyChangedCallback MaxValueChanged;

  public event PropertyChangedCallback NumberDecimalDigitsChanged;

  public event PropertyChangedCallback NumberDecimalSeparatorChanged;

  public event PropertyChangedCallback NumberGroupSizesChanged;

  public event PropertyChangedCallback NumberGroupSeparatorChanged;

  internal event PropertyChangedCallback IsExceedDecimalDigitsChanged;

  public event PropertyChangedCallback MinimumNumberDecimalDigitsChanged;

  public event PropertyChangedCallback MaximumNumberDecimalDigitsChanged;

  static DoubleTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DoubleTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DoubleTextBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ICommand pastecommand { get; private set; }

  public ICommand copycommand { get; private set; }

  public ICommand cutcommand { get; private set; }

  public DoubleTextBox()
  {
    this.pastecommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
    this.copycommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
    this.cutcommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
    this.Loaded -= new RoutedEventHandler(this.DoubleTextbox_Loaded);
    this.Loaded += new RoutedEventHandler(this.DoubleTextbox_Loaded);
    this.Unloaded += new RoutedEventHandler(this.DoubleTextBox_Unloaded);
    this.NumberFormatChanged += new PropertyChangedCallback(this.DoubleTextBox_NumberFormatChanged);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void DoubleTextBox_NumberFormatChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    this.UpdateNumberDecimalDigits();
  }

  private void DoubleTextBox_Unloaded(object sender, RoutedEventArgs e) => this.mIsLoaded = false;

  public new void Dispose()
  {
    if (this.upButton != null && this.downButton != null)
    {
      this.upButton.Click -= new RoutedEventHandler(this.SpinButton_Click);
      this.downButton.Click -= new RoutedEventHandler(this.SpinButton_Click);
    }
    this.upButton = (RepeatButton) null;
    this.downButton = (RepeatButton) null;
    this.pastecommand = (ICommand) null;
    this.copycommand = (ICommand) null;
    this.cutcommand = (ICommand) null;
    this.Unloaded -= new RoutedEventHandler(this.DoubleTextBox_Unloaded);
    this.NumberFormatChanged -= new PropertyChangedCallback(this.DoubleTextBox_NumberFormatChanged);
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
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
    this.PART_ContentHost = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
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
      DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this, true);
    }
    else
    {
      if (e.Delta >= 0)
        return;
      DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this, false);
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

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    e.Handled = DoubleValueHandler.doubleValueHandler.HandleKeyDown(this, e);
    if (e.Key == Key.Z)
    {
      if (!this.IsUndoEnabled)
      {
        e.Handled = true;
      }
      else
      {
        if (this.CanUndo && (this.uval > 1 || !(this.MaskedText == this.utext)))
        {
          this.Undo();
          double result;
          if (double.TryParse(this.Text, out result))
          {
            this.mValueChanged = new bool?(false);
            this.SetValue(DoubleTextBox.ValueProperty, (object) result);
            this.mValueChanged = new bool?(true);
          }
        }
        --this.uval;
        e.Handled = true;
      }
    }
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
      else if (!this.EnterToMoveNext)
      {
        if (VisualUtils.FindAncestor((Visual) this, typeof (Window)) is Window ancestor)
        {
          if (DoubleTextBox.FindChild((Visual) ancestor, typeof (Button)) is Button)
            e.Handled = false;
          else
            e.Handled = true;
        }
        else
        {
          if (PresentationSource.FromVisual((Visual) this) is HwndSource)
            e.Handled = true;
          Window.GetWindow((DependencyObject) this);
          Visual rootVisual = VisualUtils.FindRootVisual((Visual) this);
          if (rootVisual is Popup || rootVisual.GetType().Name.Contains("Popup"))
          {
            if (DoubleTextBox.FindChild(rootVisual, typeof (Button)) is Button)
              e.Handled = false;
            else
              e.Handled = true;
          }
        }
      }
      else
        e.Handled = true;
    }
    else
      e.Handled = DoubleValueHandler.doubleValueHandler.HandleKeyDown(this, e);
    base.OnKeyDown(e);
  }

  public static Visual FindChild(Visual startingFrom, Type typeDescendant)
  {
    Visual child1 = (Visual) null;
    int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) startingFrom);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      Visual child2 = VisualTreeHelper.GetChild((DependencyObject) startingFrom, childIndex) as Visual;
      if (typeDescendant.IsInstanceOfType((object) child2) && child2 is Button child3 && child3.IsDefault)
        return (Visual) child3;
      if (child2 != null)
      {
        child1 = DoubleTextBox.FindChild(child2, typeDescendant);
        if (child1 != null)
          break;
      }
    }
    return child1;
  }

  private void cut()
  {
    try
    {
      if (this.SelectionLength <= 0)
        return;
      Clipboard.SetText(this.SelectedText);
      this.count = 1;
      DoubleValueHandler.doubleValueHandler.HandleDeleteKey(this);
    }
    catch (COMException ex)
    {
    }
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    if (InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
      this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
      {
        int caretIndex = this.CaretIndex;
        if (this.CaretIndex > 0)
          this.MaskedText = this.MaskedText.Remove(this.CaretIndex - e.Text.Length, e.Text.Length);
        if (caretIndex - e.Text.Length >= 0)
          this.CaretIndex = caretIndex - e.Text.Length;
        e.Handled = DoubleValueHandler.doubleValueHandler.MatchWithMask(this, e.Text);
        this.allowchange = false;
      }));
    else
      e.Handled = DoubleValueHandler.doubleValueHandler.MatchWithMask(this, e.Text);
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
          this.SetCurrentValue(DoubleTextBox.ValueProperty, (object) null);
        }
        else if (this.InvalidValueBehavior == InvalidInputBehavior.None)
        {
          this.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationValue));
          this.OnValidated(EventArgs.Empty);
        }
      }
      else
        this.OnValidated(EventArgs.Empty);
    }
    if (this.EnableFocusColors && this.PART_ContentHost != null)
      this.PART_ContentHost.Background = this.Background;
    double? nullable1 = this.Value;
    if (this.mIsLoaded)
      this.ValidationCompleted = this.ValidationValue == this.Value.ToString();
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
      double? nullable4 = this.Value;
      if ((nullable3.GetValueOrDefault() != nullable4.GetValueOrDefault() ? 1 : (nullable3.HasValue != nullable4.HasValue ? 1 : 0)) != 0)
      {
        if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
        {
          OldValue = (object) this.Value,
          NewValue = (object) nullable1
        }))
          this.Value = nullable1;
      }
    }
    if (this.MaskedText.Length >= 15 && this.MaxValidation == MaxValidation.OnLostFocus)
    {
      this.lostfocusmasktext = this.MaskedText;
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      double num = double.Parse(this.MaskedText);
      this.MaskedText = num.ToString("N", (IFormatProvider) numberFormat);
      this.Value = new double?(num);
    }
    if (this.Text == "-")
    {
      double? nullable5 = this.Value;
      if ((nullable5.GetValueOrDefault() != 0.0 ? 0 : (nullable5.HasValue ? 1 : 0)) != 0)
      {
        this.minusPressed = false;
        this.SetValue(new bool?(true), new double?(0.0));
      }
    }
    base.OnLostFocus(e);
    this.checktext = "";
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    if (this.EnableFocusColors)
    {
      if (this.PART_ContentHost != null)
        this.PART_ContentHost.Background = this.FocusedBackground;
    }
    try
    {
      if (this.MaskedText.Length >= 15)
      {
        if (this.MaxValidation == MaxValidation.OnLostFocus)
          this.MaskedText = this.lostfocusmasktext;
      }
    }
    catch
    {
    }
    base.OnGotFocus(e);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new DoubleTextBoxAutomationPeer(this);
  }

  internal void FormatText()
  {
    if (this.Value.HasValue && !double.IsNaN(this.Value.Value))
    {
      double? mValue = this.mValue;
      if ((mValue.GetValueOrDefault() != 0.0 ? 0 : (mValue.HasValue ? 1 : 0)) != 0 && !this.Value.HasValue)
      {
        if (this.UseNullOption)
        {
          this.SetValue(new bool?(true), new double?());
        }
        else
        {
          this.SetValue(new bool?(true), new double?(0.0));
          this.MaskedText = this.mValue.Value.ToString("N", (IFormatProvider) this.GetCulture().NumberFormat);
        }
      }
      else
      {
        NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
        if (this.UseNullOption)
        {
          double? nullable = this.Value;
          if ((nullable.GetValueOrDefault() != 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 && this.MaskedText == "-")
            return;
        }
        this.MaskedText = this.mValue.Value.ToString("N", (IFormatProvider) numberFormat);
      }
    }
    else
      this.MaskedText = "";
  }

  internal bool SetValue(bool? IsReload, double? _Value)
  {
    NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
    if (this.IsUndoEnabled)
      ++this.uval;
    bool? nullable1 = IsReload;
    if ((nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
    {
      this.mValueChanged = new bool?(false);
      if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
      {
        OldValue = (object) this.Value,
        NewValue = (object) _Value
      }))
      {
        this.SetCurrentValue(DoubleTextBox.ValueProperty, (object) _Value);
        if (this.Value.HasValue)
        {
          double? nullable2 = this.Value;
          if ((nullable2.GetValueOrDefault() != 0.0 ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0 && !this.MaskedText.Contains("-"))
            this.MaskedText = _Value.Value.ToString("N", (IFormatProvider) numberFormat);
        }
        else if (!this.Value.HasValue && this.UseNullOption)
          this.MaskedText = "";
        this.mValueChanged = new bool?(true);
        return true;
      }
      if (this.Value.HasValue)
        this.MaskedText = this.Value.Value.ToString("N", (IFormatProvider) numberFormat);
      return true;
    }
    bool? nullable3 = IsReload;
    if ((!nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
      return false;
    if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
    {
      OldValue = (object) this.Value,
      NewValue = (object) _Value
    }))
    {
      int caretIndex = this.CaretIndex;
      this.Value = _Value;
      this.CaretIndex = caretIndex;
      return true;
    }
    if (this.Value.HasValue)
      this.MaskedText = this.Value.Value.ToString("N", (IFormatProvider) numberFormat);
    return true;
  }

  internal bool TriggerValueChangingEvent(ValueChangingEventArgs args)
  {
    if (this.ValueChanging == null)
      return false;
    this.ValueChanging((object) this, args);
    return args.Cancel;
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
    if (!this.GroupSeperatorEnabled)
      culture.NumberFormat.NumberGroupSeparator = string.Empty;
    if (this.GroupSeperatorEnabled && !this.NumberGroupSeparator.Equals(string.Empty) && culture.NumberFormat.NumberGroupSeparator != this.NumberGroupSeparator)
      culture.NumberFormat.NumberGroupSeparator = this.NumberGroupSeparator;
    if (culture.NumberFormat.NumberDecimalDigits != this.numberDecimalDigits)
      culture.NumberFormat.NumberDecimalDigits = this.numberDecimalDigits;
    if (!this.NumberDecimalSeparator.Equals(string.Empty) && culture.NumberFormat.NumberDecimalSeparator != this.NumberDecimalSeparator)
      culture.NumberFormat.NumberDecimalSeparator = this.NumberDecimalSeparator;
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
    DoubleTextBox doubleTextBox1 = (DoubleTextBox) d;
    double? nullable1;
    if (baseValue != null)
    {
      nullable1 = (double?) baseValue;
      if (double.IsNaN(nullable1.Value))
        return (object) double.NaN;
      bool? mValueChanged = doubleTextBox1.mValueChanged;
      if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) != 0)
      {
        double? nullable2 = nullable1;
        double maxValue = doubleTextBox1.MaxValue;
        if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 && doubleTextBox1.MaxValidation == MaxValidation.OnKeyPress)
        {
          doubleTextBox1.MinMaxchanged = true;
          nullable1 = new double?(doubleTextBox1.MaxValue);
        }
        else
        {
          double? nullable3 = nullable1;
          double minValue = doubleTextBox1.MinValue;
          if ((nullable3.GetValueOrDefault() >= minValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && doubleTextBox1.MinValidation == MinValidation.OnKeyPress)
          {
            doubleTextBox1.MinMaxchanged = true;
            nullable1 = new double?(doubleTextBox1.MinValue);
          }
        }
      }
    }
    else
    {
      if (doubleTextBox1.UseNullOption)
      {
        doubleTextBox1.IsNull = true;
        doubleTextBox1.IsNegative = false;
        doubleTextBox1.IsZero = false;
        return (object) doubleTextBox1.NullValue;
      }
      nullable1 = new double?(0.0);
      bool? mValueChanged = doubleTextBox1.mValueChanged;
      if ((!mValueChanged.GetValueOrDefault() ? 0 : (mValueChanged.HasValue ? 1 : 0)) != 0)
      {
        double? nullable4 = nullable1;
        double maxValue = doubleTextBox1.MaxValue;
        if ((nullable4.GetValueOrDefault() <= maxValue ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
        {
          nullable1 = new double?(doubleTextBox1.MaxValue);
          doubleTextBox1.MinMaxchanged = true;
        }
        double? nullable5 = nullable1;
        double minValue = doubleTextBox1.MinValue;
        if ((nullable5.GetValueOrDefault() >= minValue ? 0 : (nullable5.HasValue ? 1 : 0)) != 0)
        {
          nullable1 = new double?(doubleTextBox1.MinValue);
          doubleTextBox1.MinMaxchanged = true;
        }
      }
    }
    DoubleTextBox doubleTextBox2 = doubleTextBox1;
    double? nullable6 = nullable1;
    int num1 = (nullable6.GetValueOrDefault() >= 0.0 ? 0 : (nullable6.HasValue ? 1 : 0)) != 0 ? 1 : 0;
    doubleTextBox2.IsNegative = num1 != 0;
    DoubleTextBox doubleTextBox3 = doubleTextBox1;
    double? nullable7 = nullable1;
    int num2 = (nullable7.GetValueOrDefault() != 0.0 ? 0 : (nullable7.HasValue ? 1 : 0)) != 0 ? 1 : 0;
    doubleTextBox3.IsZero = num2 != 0;
    doubleTextBox1.IsNull = false;
    return (object) nullable1;
  }

  internal static int CountDecimalDigits(string p, DependencyObject d)
  {
    if (string.IsNullOrEmpty(p) || !(d is DoubleTextBox))
      return 0;
    int num = 0;
    NumberFormatInfo numberFormat = ((DoubleTextBox) d).GetCulture().NumberFormat;
    if (!string.IsNullOrEmpty(numberFormat.NumberDecimalSeparator) && p.Contains(numberFormat.NumberDecimalSeparator) || p.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
    {
      for (int index = p.Length - 1; index >= 0; --index)
      {
        if (numberFormat != null)
        {
          if (!(p[index].ToString() == numberFormat.NumberDecimalSeparator) && !(p[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            ++num;
          else
            break;
        }
      }
    }
    return num;
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

  private new void Paste()
  {
    if (this.IsReadOnly)
      return;
    try
    {
      if (!this.UseNullOption && this.Value.HasValue)
      {
        double num1 = this.Value.Value;
      }
      string text = Clipboard.GetText();
      int selectionStart = this.SelectionStart;
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int num2 = 0;
      string empty3 = string.Empty;
      bool flag1 = false;
      for (int index = 0; index < text.Length; ++index)
      {
        if (numberFormat != null && numberFormat.NumberDecimalSeparator != null)
        {
          if (char.IsDigit(text[index]) && index == num2)
          {
            num2 = index + 1;
            empty1 += (string) (object) text[index];
          }
          else if (text[index].ToString() == numberFormat.NumberDecimalSeparator || text[index].ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
            num2 = index;
          else if (char.IsDigit(text[index]))
            empty2 += (string) (object) text[index];
          else if (index <= num2)
            num2 = index + 1;
        }
        else if (char.IsDigit(text[index]) && index == num2)
        {
          num2 = index + 1;
          empty1 += (string) (object) text[index];
        }
      }
      if (empty1 == string.Empty && empty2 == string.Empty)
        return;
      string str1;
      if (this.PasteMode != PasteMode.Default)
      {
        double? nullable = this.Value;
        if ((nullable.GetValueOrDefault() != 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 && this.Value.HasValue && this.Text.Length != this.SelectedText.Length)
        {
          if (this.SelectionLength > 0)
          {
            if ((this.SelectedText.Contains(numberFormat.NumberDecimalSeparator) || !(empty2 == string.Empty)) && (!this.SelectedText.Contains(numberFormat.NumberDecimalSeparator) || !(empty2 != string.Empty)))
              return;
            str1 = this.Text.Remove(this.SelectionStart, this.SelectedText.Length).Insert(this.SelectionStart, empty2 == string.Empty ? empty1 : empty1 + numberFormat.NumberDecimalSeparator + empty2);
            goto label_27;
          }
          if (!(empty2 == string.Empty))
            return;
          str1 = this.Text.Insert(this.SelectionStart, empty1);
          goto label_27;
        }
      }
      flag1 = true;
      str1 = empty1 + numberFormat.NumberDecimalSeparator + empty2;
label_27:
      if (this.IsExceedDecimalDigits)
      {
        this.count = DoubleTextBox.CountDecimalDigits(str1, (DependencyObject) this);
        this.UpdateNumberDecimalDigits(this.count);
        if (numberFormat != null && numberFormat.NumberDecimalDigits != this.numberDecimalDigits)
          numberFormat.NumberDecimalDigits = this.numberDecimalDigits;
      }
      if (!string.IsNullOrEmpty(str1))
        str1 = !text.StartsWith("-") || str1.StartsWith("-") ? str1 : "-" + str1;
      if (str1.Length >= 15)
      {
        if (this.MaxValidation == MaxValidation.OnLostFocus)
        {
          try
          {
            this.MaskedText = Convert.ToDecimal(this.MaxLengthValidation(Convert.ToDouble(Decimal.Parse(str1)))).ToString("N", (IFormatProvider) numberFormat);
            return;
          }
          catch
          {
            return;
          }
        }
      }
      bool flag2 = false;
      bool flag3 = false;
      if (numberFormat.NumberDecimalSeparator != string.Empty && numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
      {
        str1 = str1.Replace(numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        flag2 = true;
      }
      if (numberFormat.NumberGroupSeparator != string.Empty && numberFormat.NumberGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
      {
        str1 = str1.Replace(numberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
        flag3 = true;
      }
      double result;
      double.TryParse(str1, out result);
      if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress || result < this.MinValue && this.MinValidation == MinValidation.OnKeyPress)
      {
        if (result > this.MaxValue && this.MaxValidation == MaxValidation.OnKeyPress)
        {
          if (this.MaxValueOnExceedMaxDigit)
          {
            double maxValue = this.MaxValue;
            this.SetValue(new bool?(false), new double?(maxValue));
            if (flag2 || flag3)
              this.MaskedText = maxValue.ToString("N", (IFormatProvider) numberFormat);
            this.Text = maxValue.ToString("N", (IFormatProvider) numberFormat);
          }
        }
        else if (result < this.MinValue && this.MinValidation == MinValidation.OnKeyPress && this.MinValueOnExceedMinDigit)
        {
          double minValue = this.MinValue;
          this.SetValue(new bool?(false), new double?(minValue));
          if (flag2 || flag3)
            this.MaskedText = minValue.ToString("N", (IFormatProvider) numberFormat);
          this.Text = minValue.ToString("N", (IFormatProvider) numberFormat);
        }
      }
      else if (!double.IsNaN(result))
      {
        double num3 = this.MaxLengthValidation(result);
        bool flag4 = this.SetValue(new bool?(false), new double?(num3));
        if (flag2 || flag3)
          this.MaskedText = num3.ToString("N", (IFormatProvider) numberFormat);
        if (!flag4)
        {
          if (this.Culture.Name == "vi-VN" && this.NumberGroupSeparator == string.Empty)
            this.Text = num3.ToString("N", (IFormatProvider) numberFormat).Replace(".", "");
          else
            this.Text = num3.ToString("N", (IFormatProvider) numberFormat);
          int index = 0;
          string str2 = empty1 + empty2;
          int num4;
          for (num4 = flag1 ? 0 : selectionStart; num4 < this.Text.Length && index < str2.Length; ++num4)
          {
            if ((int) this.Text[num4] == (int) str2[index])
              ++index;
          }
          this.CaretIndex = num4;
          this.Select(num4, 0);
        }
        else
        {
          this.CaretIndex = 0;
          this.Select(0, 0);
        }
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
      double? nullable2 = this.Value;
      double maxValue = this.MaxValue;
      if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        return;
    }
    if (!canIncrement)
    {
      double? nullable3 = this.Value;
      double minValue = this.MinValue;
      if ((nullable3.GetValueOrDefault() >= minValue ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
        return;
    }
    double? nullable4;
    if (!canIncrement)
    {
      double? nullable5 = this.Value;
      double scrollInterval = this.ScrollInterval;
      nullable4 = nullable5.HasValue ? new double?(nullable5.GetValueOrDefault() - scrollInterval) : new double?();
    }
    else
    {
      double? nullable6 = this.Value;
      double scrollInterval = this.ScrollInterval;
      nullable4 = nullable6.HasValue ? new double?(nullable6.GetValueOrDefault() + scrollInterval) : new double?();
    }
    this.SetValue(new bool?(true), nullable4);
  }

  private double MaxLengthValidation(double value)
  {
    string str = value.ToString();
    if (this.MaxLength > 0 && str.Length > 0)
    {
      if (str.Length >= this.MaxLength)
      {
        if (this.NumberDecimalDigits > 0)
          str = str.Remove(this.MaxLength - (this.NumberDecimalDigits + 1));
        else if (this.NumberDecimalDigits < 0)
          str = str.Remove(this.MaxLength - 3);
        else if (this.NumberDecimalDigits == 0 && str.Length != this.MaxLength)
          str = str.Remove(this.MaxLength);
      }
      else if (this.NumberDecimalDigits > 0 && str.Length + (this.NumberDecimalDigits + 1) > this.MaxLength)
        str = str.Remove(str.Length - (str.Length - this.MaxLength + (this.NumberDecimalDigits + 1)));
      else if (this.NumberDecimalDigits < 0 && str.Length + 3 > this.MaxLength)
        str = str.Remove(str.Length - (str.Length - this.MaxLength + 3));
      value = Convert.ToDouble(str);
    }
    return value;
  }

  private void UpdateNumberDecimalDigits()
  {
    this.numberDecimalDigits = this.NumberDecimalDigits >= 0 ? this.NumberDecimalDigits : (this.NumberFormat == null || this.NumberFormat.NumberDecimalDigits < 0 ? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits : this.NumberFormat.NumberDecimalDigits);
    if (!this.Value.HasValue || !this.IsExceedDecimalDigits)
      return;
    this.UpdateNumberDecimalDigits(DoubleTextBox.CountDecimalDigits(this.Value.ToString(), (DependencyObject) this));
  }

  internal void UpdateNumberDecimalDigits(int count)
  {
    int num = this.NumberDecimalDigits >= 0 ? this.NumberDecimalDigits : (this.NumberFormat == null || this.NumberFormat.NumberDecimalDigits < 0 ? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits : this.NumberFormat.NumberDecimalDigits);
    if (this.MinimumNumberDecimalDigits >= 0 && this.MaximumNumberDecimalDigits >= 0 && this.MaximumNumberDecimalDigits > this.MinimumNumberDecimalDigits)
    {
      if (count >= this.MinimumNumberDecimalDigits && count <= this.MaximumNumberDecimalDigits)
        this.numberDecimalDigits = count;
      else if (count < this.MinimumNumberDecimalDigits)
      {
        this.numberDecimalDigits = this.MinimumNumberDecimalDigits;
      }
      else
      {
        if (count <= this.MaximumNumberDecimalDigits)
          return;
        this.numberDecimalDigits = this.MaximumNumberDecimalDigits;
      }
    }
    else if (this.MinimumNumberDecimalDigits >= 0 && this.MinimumNumberDecimalDigits > num && this.MaximumNumberDecimalDigits < this.MinimumNumberDecimalDigits)
      this.numberDecimalDigits = this.MinimumNumberDecimalDigits;
    else if (this.MinimumNumberDecimalDigits >= 0 && this.MaximumNumberDecimalDigits >= 0 && this.MinimumNumberDecimalDigits == this.MaximumNumberDecimalDigits)
      this.numberDecimalDigits = this.MinimumNumberDecimalDigits;
    else if (this.MinimumNumberDecimalDigits >= 0)
    {
      if (count >= this.MinimumNumberDecimalDigits && count <= num)
        this.numberDecimalDigits = count;
      else if (count < this.MinimumNumberDecimalDigits)
      {
        this.numberDecimalDigits = this.MinimumNumberDecimalDigits;
      }
      else
      {
        if (count <= num)
          return;
        this.numberDecimalDigits = num;
      }
    }
    else
    {
      if (this.MaximumNumberDecimalDigits < 0)
        return;
      if (count >= num && count <= this.MaximumNumberDecimalDigits)
        this.numberDecimalDigits = count;
      else if (count < num)
      {
        this.numberDecimalDigits = num;
      }
      else
      {
        if (count <= this.MaximumNumberDecimalDigits)
          return;
        this.numberDecimalDigits = this.MaximumNumberDecimalDigits;
      }
    }
  }

  private void DoubleTextbox_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateNumberDecimalDigits();
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.AddHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted), true);
    this.mIsLoaded = true;
    double? nullable1 = (double?) DoubleTextBox.CoerceValue((DependencyObject) this, (object) this.Value);
    if (this.IsNull && !this.IsFocused)
      this.WatermarkVisibility = Visibility.Visible;
    if (nullable1.HasValue && !double.IsNaN(nullable1.Value))
    {
      double? nullable2 = nullable1;
      double? nullable3 = this.Value;
      if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 1 : (nullable2.HasValue != nullable3.HasValue ? 1 : 0)) != 0)
      {
        if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
        {
          OldValue = (object) this.Value,
          NewValue = (object) nullable1
        }))
        {
          if (this.UseNullOption)
          {
            if (nullable1.HasValue)
            {
              this.Value = nullable1;
              goto label_10;
            }
            goto label_10;
          }
          this.Value = nullable1;
          goto label_10;
        }
        goto label_10;
      }
    }
    this.FormatText();
label_10:
    if (this.Value.HasValue && double.IsNaN(this.Value.Value))
    {
      NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
      if (this.mValue.HasValue)
        this.MaskedText = this.mValue.Value.ToString("N", (IFormatProvider) numberFormat);
    }
    if (this.TextSelectionOnFocus && this.IsFocused)
    {
      e.Handled = true;
      this.Focus();
      this.SelectAll();
    }
    if (this.IsUndoEnabled)
      this.utext = this.MaskedText;
    double? nullable4 = this.Value;
    if (!nullable4.HasValue || !this.MinMaxchanged)
      return;
    this.SetValue(DoubleTextBox.ValueProperty, (object) nullable4);
    this.MinMaxchanged = false;
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

  public double? Value
  {
    get => (double?) this.GetValue(DoubleTextBox.ValueProperty);
    set => this.SetValue(DoubleTextBox.ValueProperty, (object) value);
  }

  public double MinValue
  {
    get => (double) this.GetValue(DoubleTextBox.MinValueProperty);
    set => this.SetValue(DoubleTextBox.MinValueProperty, (object) value);
  }

  public double MaxValue
  {
    get => (double) this.GetValue(DoubleTextBox.MaxValueProperty);
    set => this.SetValue(DoubleTextBox.MaxValueProperty, (object) value);
  }

  public string NumberGroupSeparator
  {
    get => (string) this.GetValue(DoubleTextBox.NumberGroupSeparatorProperty);
    set => this.SetValue(DoubleTextBox.NumberGroupSeparatorProperty, (object) value);
  }

  public Int32Collection NumberGroupSizes
  {
    get => (Int32Collection) this.GetValue(DoubleTextBox.NumberGroupSizesProperty);
    set => this.SetValue(DoubleTextBox.NumberGroupSizesProperty, (object) value);
  }

  public int MinimumNumberDecimalDigits
  {
    get => (int) this.GetValue(DoubleTextBox.MinimumNumberDecimalDigitsProperty);
    set => this.SetValue(DoubleTextBox.MinimumNumberDecimalDigitsProperty, (object) value);
  }

  public int MaximumNumberDecimalDigits
  {
    get => (int) this.GetValue(DoubleTextBox.MaximumNumberDecimalDigitsProperty);
    set => this.SetValue(DoubleTextBox.MaximumNumberDecimalDigitsProperty, (object) value);
  }

  public int NumberDecimalDigits
  {
    get => (int) this.GetValue(DoubleTextBox.NumberDecimalDigitsProperty);
    set => this.SetValue(DoubleTextBox.NumberDecimalDigitsProperty, (object) value);
  }

  public string NumberDecimalSeparator
  {
    get => (string) this.GetValue(DoubleTextBox.NumberDecimalSeparatorProperty);
    set => this.SetValue(DoubleTextBox.NumberDecimalSeparatorProperty, (object) value);
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(DoubleTextBox.GroupSeperatorEnabledProperty);
    set => this.SetValue(DoubleTextBox.GroupSeperatorEnabledProperty, (object) value);
  }

  internal bool IsExceedDecimalDigits
  {
    get => (bool) this.GetValue(DoubleTextBox.IsExceedDecimalDigitsProperty);
    set => this.SetValue(DoubleTextBox.IsExceedDecimalDigitsProperty, (object) value);
  }

  public static void OnIsExceedDecimalDigits(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnIsExceedDecimalDigitsChanged(args);
  }

  protected void OnIsExceedDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsExceedDecimalDigitsChanged == null)
      return;
    this.IsExceedDecimalDigitsChanged((DependencyObject) this, args);
  }

  public static void OnMaximumNumberDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnMaximumNumberDecimalDigitsChanged(args);
  }

  protected void OnMaximumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateNumberDecimalDigits();
    if (this.MaximumNumberDecimalDigitsChanged != null)
      this.MaximumNumberDecimalDigitsChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnMinimumNumberDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnMinimumNumberDecimalDigitsChanged(args);
  }

  protected void OnMinimumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateNumberDecimalDigits();
    if (this.MinimumNumberDecimalDigitsChanged != null)
      this.MinimumNumberDecimalDigitsChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  private void CheckIsExceedDecimalDigits()
  {
    if (this.NumberDecimalDigits < 0)
    {
      int numberDecimalDigits1 = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
    }
    else
    {
      int numberDecimalDigits2 = this.NumberDecimalDigits;
    }
    if (this.NumberDecimalDigits < 0)
      this.IsExceedDecimalDigits = true;
    else
      this.IsExceedDecimalDigits = false;
  }

  public double ScrollInterval
  {
    get => (double) this.GetValue(DoubleTextBox.ScrollIntervalProperty);
    set => this.SetValue(DoubleTextBox.ScrollIntervalProperty, (object) value);
  }

  public double Step
  {
    get => (double) this.GetValue(DoubleTextBox.StepProperty);
    set => this.SetValue(DoubleTextBox.StepProperty, (object) value);
  }

  public static void OnNumberGroupSizesChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnNumberGroupSizesChanged(args);
  }

  protected void OnNumberGroupSizesChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NumberGroupSizesChanged != null)
      this.NumberGroupSizesChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnNumberDecimalSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnNumberDecimalSeparatorChanged(args);
  }

  protected void OnNumberDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NumberDecimalSeparatorChanged != null)
      this.NumberDecimalSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnNumberDecimalDigitsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnNumberDecimalDigitsChanged(args);
  }

  protected void OnNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CheckIsExceedDecimalDigits();
    this.UpdateNumberDecimalDigits();
    if (this.NumberDecimalDigitsChanged != null)
      this.NumberDecimalDigitsChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.FormatText();
  }

  public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnValueChanged(args);
  }

  public override void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
  {
    if (!(bool) args.NewValue && this.IsNull)
    {
      this.Value = new double?(this.MinValue);
      this.IsNull = false;
    }
    base.OnUseNullOptionChanged(args);
  }

  protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
      this.UpdateNumberDecimalDigits();
    if (this.Value.HasValue && !double.IsNaN(this.Value.Value))
    {
      double? nullable1 = this.Value;
      this.IsNegative = (nullable1.GetValueOrDefault() >= 0.0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0;
      double? nullable2 = this.Value;
      this.IsZero = (nullable2.GetValueOrDefault() != 0.0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0;
      this.IsNull = false;
    }
    else if (this.UseNullOption)
    {
      this.IsNull = true;
      this.IsNegative = false;
      this.IsZero = false;
    }
    this.OldValue = (double?) args.OldValue;
    this.mValue = this.Value;
    if (this.Value.HasValue)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (this.ValueChanged != null)
      this.ValueChanged((DependencyObject) this, args);
    double? nullable = this.Value;
    double minValue = this.MinValue;
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
    ((DoubleTextBox) obj)?.OnMinValueChanged(args);
  }

  protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinValueChanged != null)
      this.MinValueChanged((DependencyObject) this, args);
    if (this.UseNullOption && this.Value.HasValue && !double.IsNaN(this.Value.Value) && this.MinValue != 0.0)
    {
      double? nullable = this.Value;
      double minValue = this.MinValue;
      if ((nullable.GetValueOrDefault() >= minValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        this.Value = new double?(this.MinValue);
    }
    double? nullable1 = this.Value;
    double? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) != 0)
    {
      double? nullable3 = this.ValidateValue(this.Value);
      if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
      {
        OldValue = (object) this.Value,
        NewValue = (object) nullable3
      }))
        this.Value = nullable3;
    }
    if (BindingOperations.GetBindingExpression((DependencyObject) this, DoubleTextBox.ValueProperty) == null)
      return;
    BindingOperations.GetBindingExpression((DependencyObject) this, DoubleTextBox.ValueProperty).UpdateTarget();
  }

  public static void OnMaxValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnMaxValueChanged(args);
  }

  protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxValueChanged != null)
      this.MaxValueChanged((DependencyObject) this, args);
    double? nullable1 = this.Value;
    double? nullable2 = this.ValidateValue(this.Value);
    if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) != 0)
    {
      double? nullable3 = this.ValidateValue(this.Value);
      if (!this.TriggerValueChangingEvent(new ValueChangingEventArgs()
      {
        OldValue = (object) this.Value,
        NewValue = (object) nullable3
      }))
        this.Value = nullable3;
    }
    if (BindingOperations.GetBindingExpression((DependencyObject) this, DoubleTextBox.ValueProperty) == null)
      return;
    BindingOperations.GetBindingExpression((DependencyObject) this, DoubleTextBox.ValueProperty).UpdateTarget();
  }

  private static void OnNumberGroupSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnNumberGroupSeparatorChanged(args);
  }

  private static object CoerceNumberGroupSeperator(DependencyObject d, object baseValue)
  {
    NumberFormatInfo numberFormat = ((DoubleTextBox) d).GetCulture().NumberFormat;
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

  public double? NullValue
  {
    get => (double?) this.GetValue(DoubleTextBox.NullValueProperty);
    set => this.SetValue(DoubleTextBox.NullValueProperty, (object) value);
  }

  public static void OnNullValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnNullValueChanged(args);
  }

  protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public StringValidation ValueValidation
  {
    get => (StringValidation) this.GetValue(DoubleTextBox.ValueValidationProperty);
    set => this.SetValue(DoubleTextBox.ValueValidationProperty, (object) value);
  }

  public InvalidInputBehavior InvalidValueBehavior
  {
    get => (InvalidInputBehavior) this.GetValue(DoubleTextBox.InvalidValueBehaviorProperty);
    set => this.SetValue(DoubleTextBox.InvalidValueBehaviorProperty, (object) value);
  }

  public static void OnInvalidValueBehaviorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnInvalidValueBehaviorChanged(args);
  }

  protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.InvalidValueBehaviorChanged == null)
      return;
    this.InvalidValueBehaviorChanged((DependencyObject) this, args);
  }

  public string ValidationValue
  {
    get => (string) this.GetValue(DoubleTextBox.ValidationValueProperty);
    set => this.SetValue(DoubleTextBox.ValidationValueProperty, (object) value);
  }

  public static void OnValidationValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnValidationValueChanged(args);
  }

  protected void OnValidationValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationValueChanged == null)
      return;
    this.ValidationValueChanged((DependencyObject) this, args);
  }

  public bool ValidationCompleted
  {
    get => (bool) this.GetValue(DoubleTextBox.ValidationCompletedProperty);
    set => this.SetValue(DoubleTextBox.ValidationCompletedProperty, (object) value);
  }

  public static void OnValidationCompletedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DoubleTextBox) obj)?.OnValidationCompletedPropertyChanged(args);
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

  public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);
}
