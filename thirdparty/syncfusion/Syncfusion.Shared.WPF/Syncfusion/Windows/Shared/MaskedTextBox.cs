// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MaskedTextBox
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/TransparentStyle.xaml")]
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Obsolete("This control is obsolete. Use new SfMaskedEdit in Syncfusion.SfInput.WPF assembly instead.")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (MaskedTextBox), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Editors/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (MaskedTextBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Editors/Themes/VS2010Style.xaml")]
public class MaskedTextBox : TextBox, IDisposable
{
  internal string mValue;
  internal bool mIsLoaded;
  internal bool mValueChanged = true;
  internal string oldValue;
  internal int tempSelectedLength;
  private AdornerLayer aLayer;
  private TextBoxSelectionAdorner txtSelectionAdorner1;
  public static DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(nameof (WatermarkTemplate), typeof (DataTemplate), typeof (MaskedTextBox), new PropertyMetadata(new PropertyChangedCallback(MaskedTextBox.OnWaterMarkTemplateChanged)));
  public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register(nameof (IsNumeric), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(MaskedTextBox.OnMinLengthChanged)));
  public static readonly DependencyProperty MinLengthProperty = DependencyProperty.Register(nameof (MinLength), typeof (int), typeof (MaskedTextBox), new PropertyMetadata((object) 0, new PropertyChangedCallback(MaskedTextBox.OnMinLengthChanged)));
  public static DependencyProperty WatermarkTextProperty = DependencyProperty.Register(nameof (WatermarkText), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) "Type here...", new PropertyChangedCallback(MaskedTextBox.OnWaterMarkTextChanged)));
  public static DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register(nameof (WatermarkVisibility), typeof (Visibility), typeof (MaskedTextBox), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty ContentElementVisibilityProperty = DependencyProperty.Register(nameof (ContentElementVisibility), typeof (Visibility), typeof (MaskedTextBox), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty WatermarkTextForegroundProperty = DependencyProperty.Register(nameof (WatermarkTextForeground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty WatermarkBackgroundProperty = DependencyProperty.Register(nameof (WatermarkBackground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty WatermarkOpacityProperty = DependencyProperty.Register(nameof (WatermarkOpacity), typeof (double), typeof (MaskedTextBox), new PropertyMetadata((object) 0.5));
  public static readonly DependencyProperty WatermarkTextIsVisibleProperty = DependencyProperty.Register(nameof (WatermarkTextIsVisible), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(MaskedTextBox.OnWatermarkTextIsVisibleChanged)));
  public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof (ReadOnly), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false));
  internal static readonly DependencyProperty CaretIndexProperty = DependencyProperty.Register(nameof (CaretIndex), typeof (int), typeof (MaskedTextBox), new PropertyMetadata((object) 0));
  public new static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register(nameof (IsUndoEnabled), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (MaskedTextBox), new PropertyMetadata((object) new CornerRadius(1.0)));
  public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(nameof (FocusedBackground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(nameof (FocusedForeground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(nameof (FocusedBorderBrush), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty IsCaretAnimationEnabledProperty = DependencyProperty.Register(nameof (IsCaretAnimationEnabled), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof (ReadOnlyBackground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register(nameof (SelectionForeground), typeof (Brush), typeof (MaskedTextBox), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty InvalidValueBehaviorProperty = DependencyProperty.Register(nameof (InvalidValueBehavior), typeof (InvalidInputBehavior), typeof (MaskedTextBox), new PropertyMetadata((object) InvalidInputBehavior.None, new PropertyChangedCallback(MaskedTextBox.OnInvalidValueBehaviorChanged)));
  private ObservableCollection<CharacterProperties> mCharCollection = new ObservableCollection<CharacterProperties>();
  public static readonly DependencyProperty EnterToMoveNextProperty = DependencyProperty.Register(nameof (EnterToMoveNext), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(MaskedTextBox.OnEnterToMoveNextChanged)));
  public static readonly DependencyProperty ValidationStringProperty = DependencyProperty.Register(nameof (ValidationString), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnValidationStringChanged)));
  public static readonly DependencyProperty MaxCharLengthProperty = DependencyProperty.Register(nameof (MaxCharLength), typeof (int), typeof (MaskedTextBox), new PropertyMetadata((object) int.MaxValue));
  public static readonly DependencyProperty StringValidationProperty = DependencyProperty.Register(nameof (StringValidation), typeof (StringValidation), typeof (MaskedTextBox), new PropertyMetadata((object) StringValidation.OnLostFocus));
  public static readonly DependencyProperty TextSelectionOnFocusProperty = DependencyProperty.Register(nameof (TextSelectionOnFocus), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(MaskedTextBox.OnTextSelectionOnFocusChanged)));
  public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register(nameof (PromptChar), typeof (char), typeof (MaskedTextBox), new PropertyMetadata((object) '_', new PropertyChangedCallback(MaskedTextBox.OnPromptCharChanged)));
  public static readonly DependencyProperty MaskedTextProperty = DependencyProperty.Register(nameof (MaskedText), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof (Mask), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnMaskChanged)));
  public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (MaskedTextBox), new PropertyMetadata((object) CultureInfo.CurrentCulture));
  public static readonly DependencyProperty CurrencySymbolProperty = DependencyProperty.Register(nameof (CurrencySymbol), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnCurrencySymbolChanged)));
  public static readonly DependencyProperty DateSeparatorProperty = DependencyProperty.Register(nameof (DateSeparator), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnDateSeparatorChanged)));
  public static readonly DependencyProperty TimeSeparatorProperty = DependencyProperty.Register(nameof (TimeSeparator), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnTimeSeparatorChanged)));
  public static readonly DependencyProperty DecimalSeparatorProperty = DependencyProperty.Register(nameof (DecimalSeparator), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnDecimalSeparatorChanged)));
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(MaskedTextBox.OnNumberGroupSeparatorChanged)));
  public static readonly DependencyProperty NumberGroupSeparatorProperty = DependencyProperty.Register(nameof (NumberGroupSeparator), typeof (string), typeof (MaskedTextBox), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MaskedTextBox.OnNumberGroupSeparatorChanged)));
  public static readonly DependencyProperty WatermarkTextModeProperty = DependencyProperty.Register(nameof (WatermarkTextMode), typeof (WatermarkTextMode), typeof (MaskedTextBox), new PropertyMetadata((object) WatermarkTextMode.HideTextOnFocus, new PropertyChangedCallback(MaskedTextBox.OnWatermarkTextModeChanged)));
  public static readonly DependencyProperty TextMaskFormatProperty = DependencyProperty.Register(nameof (TextMaskFormat), typeof (MaskFormat), typeof (MaskedTextBox), new PropertyMetadata((object) MaskFormat.IncludeLiterals, new PropertyChangedCallback(MaskedTextBox.OnTestMaskFormatChanged)));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (string), typeof (MaskedTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MaskedTextBox.OnValueChanged), new CoerceValueCallback(MaskedTextBox.CoerceMaskValue), false, UpdateSourceTrigger.LostFocus));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(MaskedTextBox.OnEnableTouchChanged)));
  public static readonly DependencyProperty MaskCompletedProperty = DependencyProperty.Register(nameof (MaskCompleted), typeof (bool), typeof (MaskedTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(MaskedTextBox.OnMaskCompletedPropertyChanged)));
  private bool fromlostfocus;
  private static bool isIME = false;
  private static string mtboxtext = string.Empty;
  private static int cindex = 0;
  private List<Key> KeyList = new List<Key>();
  private ContentControl PART_Watermark;
  private bool mMouseLeftButtonDown;

  public event PropertyChangedCallback MaskChanged;

  public event PropertyChangedCallback ValidationStringChanged;

  public event PropertyChangedCallback MaskCompletedChanged;

  [Obsolete("Event will not help due to internal arhitecture changes")]
  public event PropertyChangedCallback InvalidValueBehaviorChanged;

  public event PropertyChangedCallback DateSeparatorChanged;

  public event PropertyChangedCallback TimeSeparatorChanged;

  public event PropertyChangedCallback DecimalSeparatorChanged;

  public event PropertyChangedCallback NumberGroupSeparatorChanged;

  public event PropertyChangedCallback CurrencySymbolChanged;

  public event PropertyChangedCallback PromptCharChanged;

  public event PropertyChangedCallback WatermarkTextModeChanged;

  public event PropertyChangedCallback WatermarkTextIsVisibleChanged;

  public event CancelEventHandler Validating;

  public event EventHandler Validated;

  public event StringValidationCompletedEventHandler StringValidationCompleted;

  public event PropertyChangedCallback TextSelectionOnFocusChanged;

  public event PropertyChangedCallback EnterToMoveNextChanged;

  public event PropertyChangedCallback MinLengthChanged;

  public event PropertyChangedCallback WatermarkTemplateChanged;

  public event PropertyChangedCallback WatermarkTextChanged;

  public event PropertyChangedCallback ValueChanged;

  public DataTemplate WatermarkTemplate
  {
    get => (DataTemplate) this.GetValue(MaskedTextBox.WatermarkTemplateProperty);
    set => this.SetValue(MaskedTextBox.WatermarkTemplateProperty, (object) value);
  }

  public string WatermarkText
  {
    set => this.SetValue(MaskedTextBox.WatermarkTextProperty, (object) value);
    get => (string) this.GetValue(MaskedTextBox.WatermarkTextProperty);
  }

  public bool IsNumeric
  {
    get => (bool) this.GetValue(MaskedTextBox.IsNumericProperty);
    set => this.SetValue(MaskedTextBox.IsNumericProperty, (object) value);
  }

  public int MinLength
  {
    get => (int) this.GetValue(MaskedTextBox.MinLengthProperty);
    set => this.SetValue(MaskedTextBox.MinLengthProperty, (object) value);
  }

  public static void OnWaterMarkTextChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnWaterMarkTextChanged(args);
  }

  protected void OnWaterMarkTextChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.WatermarkTextChanged == null)
      return;
    this.WatermarkTextChanged((DependencyObject) this, args);
  }

  public Visibility WatermarkVisibility
  {
    set
    {
      value = MaskedTextBox.CoerceWatermarkVisibility((DependencyObject) this, (object) value);
      this.SetValue(MaskedTextBox.WatermarkVisibilityProperty, (object) value);
    }
    get => (Visibility) this.GetValue(MaskedTextBox.WatermarkVisibilityProperty);
  }

  private static Visibility CoerceWatermarkVisibility(DependencyObject d, object baseValue)
  {
    MaskedTextBox maskedTextBox = (MaskedTextBox) d;
    if (maskedTextBox.WatermarkTextIsVisible && (Visibility) baseValue == Visibility.Visible)
    {
      maskedTextBox.ContentElementVisibility = Visibility.Collapsed;
      return Visibility.Visible;
    }
    maskedTextBox.ContentElementVisibility = Visibility.Visible;
    return Visibility.Collapsed;
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Visibility ContentElementVisibility
  {
    get => (Visibility) this.GetValue(MaskedTextBox.ContentElementVisibilityProperty);
    set => this.SetValue(MaskedTextBox.ContentElementVisibilityProperty, (object) value);
  }

  public Brush WatermarkTextForeground
  {
    get => (Brush) this.GetValue(MaskedTextBox.WatermarkTextForegroundProperty);
    set => this.SetValue(MaskedTextBox.WatermarkTextForegroundProperty, (object) value);
  }

  public Brush WatermarkBackground
  {
    get => (Brush) this.GetValue(MaskedTextBox.WatermarkBackgroundProperty);
    set => this.SetValue(MaskedTextBox.WatermarkBackgroundProperty, (object) value);
  }

  public double WatermarkOpacity
  {
    get => (double) this.GetValue(MaskedTextBox.WatermarkOpacityProperty);
    set => this.SetValue(MaskedTextBox.WatermarkOpacityProperty, (object) value);
  }

  public bool WatermarkTextIsVisible
  {
    get => (bool) this.GetValue(MaskedTextBox.WatermarkTextIsVisibleProperty);
    set => this.SetValue(MaskedTextBox.WatermarkTextIsVisibleProperty, (object) value);
  }

  public static void OnWatermarkTextIsVisibleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnWatermarkTextIsVisibleChanged(args);
  }

  protected void OnWatermarkTextIsVisibleChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaskedText != null && this.CharCollection != null)
    {
      string str = MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, this.MaskedText, this.CharCollection);
      if (this.WatermarkTextIsVisible && string.IsNullOrEmpty(str))
      {
        this.ContentElementVisibility = Visibility.Collapsed;
        this.WatermarkVisibility = Visibility.Visible;
      }
      else
      {
        this.WatermarkVisibility = Visibility.Collapsed;
        this.ContentElementVisibility = Visibility.Visible;
      }
    }
    if (this.WatermarkTextIsVisibleChanged == null)
      return;
    this.WatermarkTextIsVisibleChanged((DependencyObject) this, args);
  }

  public static void OnMinLengthChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is MaskedTextBox maskedTextBox))
      return;
    maskedTextBox.OnMinLengthChanged(args);
  }

  protected void OnMinLengthChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinLengthChanged != null)
      this.MinLengthChanged((DependencyObject) this, args);
    if (this.MinLength > this.MaxLength || this.MinLength < 0)
      throw new InvalidOperationException("MinLength should be less than MaxLength");
  }

  public static void OnWaterMarkTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is MaskedTextBox maskedTextBox))
      return;
    maskedTextBox.OnWaterMarkTemplateChanged(args);
  }

  protected void OnWaterMarkTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.WatermarkTemplateChanged == null)
      return;
    this.WatermarkTemplateChanged((DependencyObject) this, args);
  }

  static MaskedTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MaskedTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MaskedTextBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private static void OnTextInputStart(object sender, TextCompositionEventArgs e)
  {
    MaskedTextBox maskedTextBox = (MaskedTextBox) sender;
    if (!MaskedTextBox.isIME || !(maskedTextBox.Mask != string.Empty))
      return;
    MaskedTextBox.mtboxtext = maskedTextBox.Text;
    MaskedTextBox.cindex = maskedTextBox.SelectionStart;
  }

  public ICommand pastecommand { get; private set; }

  public ICommand copycommand { get; private set; }

  public ICommand cutcommand { get; private set; }

  public MaskedTextBox()
  {
    this.AddHandler(TextCompositionManager.TextInputStartEvent, (Delegate) new TextCompositionEventHandler(MaskedTextBox.OnTextInputStart));
    this.pastecommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
    this.copycommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
    this.cutcommand = (ICommand) new DelegateCommand<object>(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
    this.Loaded -= new RoutedEventHandler(this.MaskedTextBox_Loaded);
    this.Loaded += new RoutedEventHandler(this.MaskedTextBox_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.MaskedTextBox_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.MaskedTextBox_Unloaded);
    this.LostFocus -= new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.LostFocus += new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.TextChanged -= new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.TextChanged += new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.KeyList.Add(Key.Tab);
    this.KeyList.Add(Key.Return);
    this.KeyList.Add(Key.Up);
    this.KeyList.Add(Key.Left);
    this.KeyList.Add(Key.Right);
    this.KeyList.Add(Key.Down);
    this.KeyList.Add(Key.Home);
    this.KeyList.Add(Key.End);
    this.KeyList.Add(Key.Prior);
    this.KeyList.Add(Key.Next);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void MaskedTextBox_Unloaded(object sender, RoutedEventArgs e)
  {
    this.Unloaded -= new RoutedEventHandler(this.MaskedTextBox_Unloaded);
    this.LostFocus -= new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.TextChanged -= new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
  }

  public void Dispose()
  {
    if (this.pastecommand != null)
      this.pastecommand = (ICommand) null;
    if (this.cutcommand != null)
      this.cutcommand = (ICommand) null;
    if (this.copycommand != null)
      this.copycommand = (ICommand) null;
    if (this.mCharCollection != null)
    {
      this.mCharCollection.Clear();
      this.mCharCollection = (ObservableCollection<CharacterProperties>) null;
    }
    if (this.CharCollection != null)
    {
      this.CharCollection.Clear();
      this.CharCollection = (ObservableCollection<CharacterProperties>) null;
    }
    if (this.PART_Watermark != null)
      this.PART_Watermark = (ContentControl) null;
    if (this.KeyList != null)
    {
      this.KeyList.Clear();
      this.KeyList = (List<Key>) null;
    }
    this.mIsLoaded = false;
    this.mValueChanged = false;
    this.Unloaded -= new RoutedEventHandler(this.MaskedTextBox_Unloaded);
    this.LostFocus -= new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.TextChanged -= new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.RemoveHandler(TextCompositionManager.TextInputStartEvent, (Delegate) new TextCompositionEventHandler(MaskedTextBox.OnTextInputStart));
    if (this.CharCollection != null)
    {
      this.CharCollection.Clear();
      this.CharCollection = (ObservableCollection<CharacterProperties>) null;
    }
    this.aLayer = (AdornerLayer) null;
    if (this.txtSelectionAdorner1 != null)
    {
      this.txtSelectionAdorner1.Dispose();
      this.txtSelectionAdorner1 = (TextBoxSelectionAdorner) null;
    }
    GC.Collect();
    GC.SuppressFinalize((object) this);
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
  }

  void IDisposable.Dispose() => this.Dispose();

  private void _pastecommand(object parameter) => MaskHandler.maskHandler.HandlePaste(this);

  private void _copycommand(object parameter) => this.copy();

  private void _cutcommand(object parameter) => this.cut();

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

  private bool Canpaste(object parameter) => true;

  private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    if (e.Command == ApplicationCommands.Paste)
    {
      MaskHandler.maskHandler.HandlePaste(this);
      e.Handled = true;
    }
    if (e.Command != ApplicationCommands.Cut)
      return;
    this.cut();
    e.Handled = true;
  }

  private void ExecuteClearCommand(object sender, ExecutedRoutedEventArgs e) => this.Clear();

  private void CanExecuteClearCommand(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.Text.Length > 0;
  }

  private void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (string.IsNullOrEmpty(this.Mask))
    {
      this.SetCurrentValue(MaskedTextBox.ValueProperty, (object) this.Text);
    }
    else
    {
      if (string.IsNullOrEmpty(this.Mask) || !(this.Text == ""))
        return;
      this.SetCurrentValue(MaskedTextBox.ValueProperty, (object) "");
    }
  }

  [Browsable(false)]
  [Obsolete("Use IsReadOnly Property")]
  public bool ReadOnly
  {
    get => (bool) this.GetValue(MaskedTextBox.ReadOnlyProperty);
    set => this.SetValue(MaskedTextBox.ReadOnlyProperty, (object) value);
  }

  private void MaskedTextBox_Loaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted));
    this.AddHandler(CommandManager.PreviewExecutedEvent, (Delegate) new ExecutedRoutedEventHandler(this.CommandExecuted), true);
    this.CommandBindings.Add(new CommandBinding((ICommand) EditorCommands.Clear, new ExecutedRoutedEventHandler(this.ExecuteClearCommand), new CanExecuteRoutedEventHandler(this.CanExecuteClearCommand)));
    this.mIsLoaded = true;
    this.CheckWaterMarkVisibility();
    int num = (int) MaskedTextBox.CoerceWatermarkVisibility((DependencyObject) this, (object) this.WatermarkVisibility);
    this.LostFocus -= new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.LostFocus += new RoutedEventHandler(this.MaskedTextBox_LostFocus);
    this.TextChanged -= new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.TextChanged += new TextChangedEventHandler(this.MaskedTextBox_TextChanged);
    this.LoadTextBox();
    if (!this.EnableTouch)
      return;
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if (this.aLayer == null || this.txtSelectionAdorner1 != null)
      return;
    this.txtSelectionAdorner1 = new TextBoxSelectionAdorner((UIElement) this);
    this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
  }

  internal new int CaretIndex
  {
    get => (int) this.GetValue(MaskedTextBox.CaretIndexProperty);
    set
    {
      this.SelectionStart = value;
      this.SetValue(MaskedTextBox.CaretIndexProperty, (object) value);
    }
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public new bool IsUndoEnabled
  {
    get => (bool) this.GetValue(MaskedTextBox.IsUndoEnabledProperty);
    set => this.SetValue(MaskedTextBox.IsUndoEnabledProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(MaskedTextBox.CornerRadiusProperty);
    set => this.SetValue(MaskedTextBox.CornerRadiusProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush FocusedBackground
  {
    get => (Brush) this.GetValue(MaskedTextBox.FocusedBackgroundProperty);
    set => this.SetValue(MaskedTextBox.FocusedBackgroundProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush FocusedForeground
  {
    get => (Brush) this.GetValue(MaskedTextBox.FocusedForegroundProperty);
    set => this.SetValue(MaskedTextBox.FocusedForegroundProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush FocusedBorderBrush
  {
    get => (Brush) this.GetValue(MaskedTextBox.FocusedBorderBrushProperty);
    set => this.SetValue(MaskedTextBox.FocusedBorderBrushProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public bool IsCaretAnimationEnabled
  {
    get => (bool) this.GetValue(MaskedTextBox.IsCaretAnimationEnabledProperty);
    set => this.SetValue(MaskedTextBox.IsCaretAnimationEnabledProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush ReadOnlyBackground
  {
    get => (Brush) this.GetValue(MaskedTextBox.ReadOnlyBackgroundProperty);
    set => this.SetValue(MaskedTextBox.ReadOnlyBackgroundProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush SelectionForeground
  {
    get => (Brush) this.GetValue(MaskedTextBox.SelectionForegroundProperty);
    set => this.SetValue(MaskedTextBox.SelectionForegroundProperty, (object) value);
  }

  public InvalidInputBehavior InvalidValueBehavior
  {
    get => (InvalidInputBehavior) this.GetValue(MaskedTextBox.InvalidValueBehaviorProperty);
    set => this.SetValue(MaskedTextBox.InvalidValueBehaviorProperty, (object) value);
  }

  public static void OnInvalidValueBehaviorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnInvalidValueBehaviorChanged(args);
  }

  protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.InvalidValueBehaviorChanged == null)
      return;
    this.InvalidValueBehaviorChanged((DependencyObject) this, args);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public string MaskedText
  {
    get => (string) this.GetValue(TextBox.TextProperty);
    set
    {
      this.SetValue(TextBox.TextProperty, (object) value);
      this.SetValue(MaskedTextBox.MaskedTextProperty, (object) value);
    }
  }

  public string Mask
  {
    get => (string) this.GetValue(MaskedTextBox.MaskProperty);
    set => this.SetValue(MaskedTextBox.MaskProperty, (object) value);
  }

  internal ObservableCollection<CharacterProperties> CharCollection
  {
    get => this.mCharCollection;
    set => this.mCharCollection = value;
  }

  public char PromptChar
  {
    get => (char) this.GetValue(MaskedTextBox.PromptCharProperty);
    set => this.SetValue(MaskedTextBox.PromptCharProperty, (object) value);
  }

  public CultureInfo Culture
  {
    get => (CultureInfo) this.GetValue(MaskedTextBox.CultureProperty);
    set => this.SetValue(MaskedTextBox.CultureProperty, (object) value);
  }

  public string CurrencySymbol
  {
    get => (string) this.GetValue(MaskedTextBox.CurrencySymbolProperty);
    set => this.SetValue(MaskedTextBox.CurrencySymbolProperty, (object) value);
  }

  public string DateSeparator
  {
    get => (string) this.GetValue(MaskedTextBox.DateSeparatorProperty);
    set => this.SetValue(MaskedTextBox.DateSeparatorProperty, (object) value);
  }

  public string TimeSeparator
  {
    get => (string) this.GetValue(MaskedTextBox.TimeSeparatorProperty);
    set => this.SetValue(MaskedTextBox.TimeSeparatorProperty, (object) value);
  }

  public string DecimalSeparator
  {
    get => (string) this.GetValue(MaskedTextBox.DecimalSeparatorProperty);
    set => this.SetValue(MaskedTextBox.DecimalSeparatorProperty, (object) value);
  }

  public string NumberGroupSeparator
  {
    get => (string) this.GetValue(MaskedTextBox.NumberGroupSeparatorProperty);
    set => this.SetValue(MaskedTextBox.NumberGroupSeparatorProperty, (object) value);
  }

  public WatermarkTextMode WatermarkTextMode
  {
    get => (WatermarkTextMode) this.GetValue(MaskedTextBox.WatermarkTextModeProperty);
    set => this.SetValue(MaskedTextBox.WatermarkTextModeProperty, (object) value);
  }

  public MaskFormat TextMaskFormat
  {
    get => (MaskFormat) this.GetValue(MaskedTextBox.TextMaskFormatProperty);
    set => this.SetValue(MaskedTextBox.TextMaskFormatProperty, (object) value);
  }

  public string Value
  {
    get => (string) this.GetValue(MaskedTextBox.ValueProperty);
    set => this.SetValue(MaskedTextBox.ValueProperty, (object) value);
  }

  private static object CoerceMaskValue(DependencyObject d, object baseValue)
  {
    MaskedTextBox maskedTextBox = (MaskedTextBox) d;
    if (maskedTextBox != null && (!string.IsNullOrEmpty(maskedTextBox.Mask) || baseValue == null))
    {
      if (maskedTextBox.mValueChanged && maskedTextBox.mIsLoaded)
      {
        if (baseValue == null)
          baseValue = (object) "";
        maskedTextBox.CharCollection = MaskHandler.maskHandler.CreateRegularExpression(maskedTextBox);
        string maskedText = MaskHandler.maskHandler.CoerceValue(maskedTextBox, baseValue.ToString(), MaskFormat.IncludePromptAndLiterals);
        if (maskedTextBox.CharCollection != null)
        {
          baseValue = (object) MaskHandler.maskHandler.ValueFromMaskedText(maskedTextBox, maskedTextBox.TextMaskFormat, maskedText, maskedTextBox.CharCollection);
          maskedTextBox.mValue = MaskHandler.maskHandler.ValueFromMaskedText(maskedTextBox, MaskFormat.ExcludePromptAndLiterals, maskedText, maskedTextBox.CharCollection);
        }
        if (!maskedTextBox.fromlostfocus)
          maskedTextBox.MaskedText = maskedText;
      }
    }
    else
      maskedTextBox?.SetCurrentValue(TextBox.TextProperty, baseValue);
    if (baseValue != null)
    {
      int result;
      int.TryParse(baseValue.ToString(), out result);
      if (result < 0 && maskedTextBox.Mask.StartsWith("(") && maskedTextBox.Mask.EndsWith(")"))
        return (object) result.ToString("N", (IFormatProvider) new NumberFormatInfo()
        {
          NumberDecimalDigits = 0,
          NumberNegativePattern = 0
        });
    }
    return baseValue;
  }

  public StringValidation StringValidation
  {
    get => (StringValidation) this.GetValue(MaskedTextBox.StringValidationProperty);
    set => this.SetValue(MaskedTextBox.StringValidationProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Use MaxLength property")]
  public int MaxCharLength
  {
    get => (int) this.GetValue(MaskedTextBox.MaxCharLengthProperty);
    set => this.SetValue(MaskedTextBox.MaxCharLengthProperty, (object) value);
  }

  public string ValidationString
  {
    get => (string) this.GetValue(MaskedTextBox.ValidationStringProperty);
    set => this.SetValue(MaskedTextBox.ValidationStringProperty, (object) value);
  }

  public bool EnterToMoveNext
  {
    get => (bool) this.GetValue(MaskedTextBox.EnterToMoveNextProperty);
    set => this.SetValue(MaskedTextBox.EnterToMoveNextProperty, (object) value);
  }

  public bool TextSelectionOnFocus
  {
    get => (bool) this.GetValue(MaskedTextBox.TextSelectionOnFocusProperty);
    set => this.SetValue(MaskedTextBox.TextSelectionOnFocusProperty, (object) value);
  }

  public static void OnTextSelectionOnFocusChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnTextSelectionOnFocusChanged(args);
  }

  protected void OnTextSelectionOnFocusChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.TextSelectionOnFocusChanged == null)
      return;
    this.TextSelectionOnFocusChanged((DependencyObject) this, args);
  }

  public static void OnEnterToMoveNextChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnEnterToMoveNextChanged(args);
  }

  protected void OnEnterToMoveNextChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.EnterToMoveNextChanged == null)
      return;
    this.EnterToMoveNextChanged((DependencyObject) this, args);
  }

  public static void OnValidationStringChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnValidationStringChanged(args);
  }

  protected void OnValidationStringChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ValidationStringChanged == null)
      return;
    this.ValidationStringChanged((DependencyObject) this, args);
  }

  public static void OnPromptCharChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnPromptCharChanged(args);
  }

  protected void OnPromptCharChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PromptCharChanged != null)
      this.PromptCharChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnMaskChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnMaskChanged(args);
  }

  protected void OnMaskChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaskChanged != null)
      this.MaskChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnCurrencySymbolChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnCurrencySymbolChanged(args);
  }

  protected void OnCurrencySymbolChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CurrencySymbolChanged != null)
      this.CurrencySymbolChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnDateSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnDateSeparatorChanged(args);
  }

  protected void OnDateSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DateSeparatorChanged != null)
      this.DateSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnTimeSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnTimeSeparatorChanged(args);
  }

  protected void OnTimeSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.TimeSeparatorChanged != null)
      this.TimeSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnDecimalSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnDecimalSeparatorChanged(args);
  }

  protected void OnDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DecimalSeparatorChanged != null)
      this.DecimalSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(MaskedTextBox.GroupSeperatorEnabledProperty);
    set => this.SetValue(MaskedTextBox.GroupSeperatorEnabledProperty, (object) value);
  }

  public static void OnNumberGroupSeparatorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnNumberGroupSeparatorChanged(args);
  }

  protected void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NumberGroupSeparatorChanged != null)
      this.NumberGroupSeparatorChanged((DependencyObject) this, args);
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnWatermarkTextModeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnWatermarkTextModeChanged(args);
  }

  protected void OnWatermarkTextModeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.WatermarkTextModeChanged == null)
      return;
    this.WatermarkTextModeChanged((DependencyObject) this, args);
  }

  public static void OnTestMaskFormatChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnTestMaskFormatChanged(args);
  }

  protected void OnTestMaskFormatChanged(DependencyPropertyChangedEventArgs args)
  {
    this.LoadTextBox();
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(MaskedTextBox.EnableTouchProperty);
    set => this.SetValue(MaskedTextBox.EnableTouchProperty, (object) value);
  }

  public static void OnEnableTouchChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is MaskedTextBox))
      return;
    (obj as MaskedTextBox).OnEnableTouchChanged(args);
  }

  private void OnEnableTouchChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if ((bool) args.NewValue)
    {
      if (this.aLayer == null)
        return;
      this.txtSelectionAdorner1 = new TextBoxSelectionAdorner((UIElement) this);
      if (this.txtSelectionAdorner1 == null)
        return;
      this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
    }
    else
    {
      if (this.aLayer == null || this.txtSelectionAdorner1 == null)
        return;
      this.aLayer.Remove((Adorner) this.txtSelectionAdorner1);
    }
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

  protected override void OnDrop(DragEventArgs e)
  {
    e.Handled = true;
    base.OnDrop(e);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.TextSelectionOnFocus && !this.IsFocused)
    {
      e.Handled = true;
      this.Focus();
    }
    base.OnPreviewMouseLeftButtonDown(e);
  }

  public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is MaskedTextBox maskedTextBox))
      return;
    maskedTextBox.OnValueChanged(args);
  }

  protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.mIsLoaded)
      this.MaskCompleted = !string.IsNullOrEmpty(this.Mask) ? MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.IncludeLiterals, this.MaskedText, this.CharCollection) == this.MaskedText : Regex.IsMatch(this.MaskedText, this.ValidationString);
    if (this.ValueChanged != null)
      this.ValueChanged((DependencyObject) this, args);
    this.oldValue = (string) args.OldValue;
    if (!string.IsNullOrEmpty(this.Value))
    {
      this.WatermarkVisibility = Visibility.Collapsed;
    }
    else
    {
      if (!this.IsFocused || string.IsNullOrEmpty(this.WatermarkText))
        return;
      this.WatermarkVisibility = Visibility.Visible;
    }
  }

  public bool MaskCompleted
  {
    get => (bool) this.GetValue(MaskedTextBox.MaskCompletedProperty);
    set => this.SetValue(MaskedTextBox.MaskCompletedProperty, (object) value);
  }

  public static void OnMaskCompletedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MaskedTextBox) obj)?.OnMaskCompletedPropertyChanged(args);
  }

  protected void OnMaskCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaskCompletedChanged == null)
      return;
    this.MaskCompletedChanged((DependencyObject) this, args);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    if (!this.mMouseLeftButtonDown)
    {
      if (string.IsNullOrEmpty(this.Mask) && !this.OnValidating(new CancelEventArgs(false)))
      {
        string maskedText = this.MaskedText;
        string message = "";
        if (this.MaskedText.Length < this.MinLength && this.MaskedText.Length > 0)
          this.MaskedText = this.oldValue.Length >= this.MinLength ? this.oldValue : "";
        bool bIsValidInput = Regex.IsMatch(maskedText, this.ValidationString);
        string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
        if (!bIsValidInput)
        {
          switch (this.InvalidValueBehavior)
          {
            case InvalidInputBehavior.None:
              this.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationString));
              this.OnValidated(EventArgs.Empty);
              break;
            case InvalidInputBehavior.DisplayErrorMessage:
              this.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationString));
              this.OnValidated(EventArgs.Empty);
              int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
              break;
            case InvalidInputBehavior.ResetValue:
              this.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationString));
              this.OnValidated(EventArgs.Empty);
              this.SetCurrentValue(MaskedTextBox.ValueProperty, (object) "");
              this.WatermarkVisibility = Visibility.Visible;
              return;
          }
        }
        else
        {
          this.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, this.ValidationString));
          this.OnValidated(EventArgs.Empty);
        }
      }
      base.OnLostFocus(e);
    }
    else
      this.Focus();
  }

  private void MaskedTextBox_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.mMouseLeftButtonDown)
      return;
    string mask = this.Mask;
    if (mask != null && string.IsNullOrEmpty(mask))
    {
      string maskedText = this.MaskedText;
      if (maskedText == null || !string.IsNullOrEmpty(maskedText))
        return;
      this.fromlostfocus = true;
      this.Text = string.Empty;
      this.Value = string.Empty;
      this.MaskedText = string.Empty;
      this.SelectedText = string.Empty;
      this.fromlostfocus = false;
      this.WatermarkVisibility = Visibility.Visible;
    }
    else
    {
      if (this.CharCollection == null || this.MaskedText == null || !string.IsNullOrEmpty(MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, this.MaskedText, this.CharCollection)))
        return;
      this.fromlostfocus = true;
      this.Value = string.Empty;
      this.Text = string.Empty;
      this.MaskedText = string.Empty;
      this.SelectedText = string.Empty;
      this.fromlostfocus = false;
      this.WatermarkVisibility = Visibility.Visible;
    }
  }

  internal void OnStringValidationCompleted(StringValidationEventArgs e)
  {
    if (this.StringValidationCompleted == null)
      return;
    this.StringValidationCompleted((object) this, e);
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
    return e.Cancel;
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (this.WatermarkTextMode == WatermarkTextMode.HideTextOnFocus)
    {
      this.WatermarkVisibility = Visibility.Collapsed;
      if (string.IsNullOrEmpty(this.Text))
        this.LoadTextBox();
    }
    if (!this.TextSelectionOnFocus)
      return;
    this.SelectAll();
  }

  internal void LoadTextBox()
  {
    string str1 = this.Value;
    string str2 = !string.IsNullOrEmpty(this.mValue) ? MaskedTextBox.CoerceMaskValue((DependencyObject) this, (object) this.mValue).ToString() : MaskedTextBox.CoerceMaskValue((DependencyObject) this, (object) this.Value).ToString();
    if (this.Value != str2 && this.Value != string.Empty)
    {
      this.SetValue(new bool?(false), (object) str2);
    }
    else
    {
      if (!(this.Value == string.Empty))
        return;
      this.SetValue(new bool?(false), (object) this.Value);
    }
  }

  private void CheckWaterMarkVisibility()
  {
    if (!string.IsNullOrEmpty(this.Value))
      this.WatermarkVisibility = Visibility.Collapsed;
    else if (this.WatermarkTextMode == WatermarkTextMode.HideTextOnTyping)
      this.WatermarkVisibility = Visibility.Visible;
    else if (!this.IsFocused && this.WatermarkTextMode == WatermarkTextMode.HideTextOnFocus)
      this.WatermarkVisibility = Visibility.Visible;
    else
      this.WatermarkVisibility = Visibility.Collapsed;
  }

  internal CultureInfo GetCulture()
  {
    CultureInfo culture = this.Culture == null || object.Equals((object) this.Culture, (object) CultureInfo.InvariantCulture) ? CultureInfo.CurrentCulture.Clone() as CultureInfo : this.Culture.Clone() as CultureInfo;
    if (culture != null)
    {
      if (this.CurrencySymbol != string.Empty)
        culture.NumberFormat.CurrencySymbol = this.CurrencySymbol;
      culture.NumberFormat.CurrencySymbol = culture.NumberFormat.CurrencySymbol[0].ToString();
      if (this.DecimalSeparator != string.Empty)
        culture.NumberFormat.NumberDecimalSeparator = this.DecimalSeparator;
      culture.NumberFormat.NumberDecimalSeparator = culture.NumberFormat.NumberDecimalSeparator[0].ToString();
      if (!this.GroupSeperatorEnabled)
        culture.NumberFormat.NumberGroupSeparator = string.Empty;
      if (this.GroupSeperatorEnabled)
      {
        if (this.NumberGroupSeparator != string.Empty)
          culture.NumberFormat.NumberGroupSeparator = this.NumberGroupSeparator;
        culture.NumberFormat.NumberGroupSeparator = culture.NumberFormat.NumberGroupSeparator[0].ToString();
      }
    }
    return culture;
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.ImeProcessed)
      MaskedTextBox.isIME = true;
    this.tempSelectedLength = this.SelectedText.Length;
    if (object.Equals((object) e.Key, (object) Key.Space))
    {
      e.Handled = MaskHandler.maskHandler.MatchWithMask(this, " ");
      if (e.Handled && e.Handled)
      {
        string valueFromText = MaskHandler.maskHandler.CreateValueFromText(this);
        if (valueFromText != this.Value)
          this.SetValue(new bool?(false), (object) valueFromText);
      }
    }
    else
    {
      if (this.SelectedText != string.Empty && !this.IsReadOnly && Keyboard.Modifiers == ModifierKeys.None && !this.KeyList.Contains(e.Key) && this.Mask != string.Empty)
      {
        MaskHandler.maskHandler.HandleBackSpaceKey(this);
        this.CaretIndex = this.SelectionStart <= 0 ? 0 : this.SelectionStart + 1;
      }
      e.Handled = MaskHandler.maskHandler.HandleKeyDown(this, e);
      if (e.Handled)
      {
        string valueFromText = MaskHandler.maskHandler.CreateValueFromText(this);
        if (valueFromText != this.Value)
          this.SetValue(new bool?(false), (object) valueFromText);
      }
      if (string.IsNullOrEmpty(this.WatermarkText) && this.WatermarkTextIsVisible)
        this.WatermarkVisibility = Visibility.Collapsed;
    }
    if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
      e.Handled = true;
    base.OnPreviewKeyDown(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (WatermarkTextMode.HideTextOnTyping == this.WatermarkTextMode)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (ModifierKeys.Control == Keyboard.Modifiers)
    {
      if (e.Key == Key.V)
      {
        MaskHandler.maskHandler.HandlePaste(this);
        e.Handled = true;
      }
      int key = (int) e.Key;
      if (e.Key == Key.X)
      {
        this.cut();
        e.Handled = true;
      }
    }
    base.OnKeyDown(e);
  }

  private void cut()
  {
    try
    {
      if (this.SelectionLength <= 0)
        return;
      Clipboard.SetText(this.SelectedText);
      MaskHandler.maskHandler.HandleDeleteKey(this);
    }
    catch (COMException ex)
    {
    }
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    if (this.CaretIndex >= 0 && MaskedTextBox.isIME && this.Mask != string.Empty)
    {
      this.Text = MaskedTextBox.mtboxtext;
      this.CaretIndex = MaskedTextBox.cindex;
      MaskedTextBox.mtboxtext = string.Empty;
      MaskedTextBox.cindex = 0;
      MaskedTextBox.isIME = false;
    }
    if (MaskHandler.maskHandler != null)
    {
      e.Handled = MaskHandler.maskHandler.MatchWithMask(this, e.Text);
      if (!string.IsNullOrEmpty(this.Mask) && e.Handled)
      {
        string valueFromText = MaskHandler.maskHandler.CreateValueFromText(this);
        if (valueFromText != this.Value)
          this.SetValue(new bool?(false), (object) valueFromText);
      }
    }
    base.OnTextInput(e);
  }

  protected override void OnTextChanged(TextChangedEventArgs e)
  {
    if (InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
      e.Handled = MaskHandler.maskHandler.MatchWithMask(this, this.Text);
    base.OnTextChanged(e);
  }

  internal void SetValue(bool? IsReload, object _Value)
  {
    this.mValueChanged = false;
    this.SetCurrentValue(MaskedTextBox.ValueProperty, (object) _Value.ToString());
    if (MaskHandler.maskHandler != null)
      this.mValue = MaskHandler.maskHandler.ValueFromMaskedText(this, MaskFormat.ExcludePromptAndLiterals, this.MaskedText, this.CharCollection);
    this.mValueChanged = true;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PART_Watermark = this.GetTemplateChild("PART_Watermark") as ContentControl;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.mMouseLeftButtonDown = true;
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.mMouseLeftButtonDown = false;
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    this.mMouseLeftButtonDown = false;
    base.OnMouseLeave(e);
  }
}
