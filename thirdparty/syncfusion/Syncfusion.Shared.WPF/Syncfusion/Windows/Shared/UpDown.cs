// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.UpDown
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2007SilverStyle.xaml")]
[DesignTimeVisible(true)]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (UpDown), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/UpDown/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (UpDown), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/UpDown/Themes/Office2013Style.xaml")]
public class UpDown : Control, IDisposable
{
  public static RoutedCommand m_downValue;
  public static RoutedCommand m_upValue;
  private Border border;
  private DoubleTextBox t1;
  private DoubleTextBox textbox;
  private RepeatButton Upbutton;
  private RepeatButton Downbutton;
  private double? m_value;
  private double? m_oldvalue;
  private double? m_exvalue;
  private CommandBinding downValueBinding;
  private CommandBinding upValueBinding;
  public static readonly DependencyProperty IsScrollingOnCircleProperty = DependencyProperty.Register(nameof (IsScrollingOnCircle), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true, new PropertyChangedCallback(UpDown.OnIsScrollingOnClicleChanged)));
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false));
  public static readonly DependencyProperty NumberDecimalDigitsProperty = DependencyProperty.Register(nameof (NumberDecimalDigits), typeof (int), typeof (UpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) -1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  public static readonly DependencyProperty GroupSeperatorEnabledProperty = DependencyProperty.Register(nameof (GroupSeperatorEnabled), typeof (bool), typeof (UpDown), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public new static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof (IsFocused), typeof (bool), typeof (UpDown), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty NullValueTextProperty = DependencyProperty.Register(nameof (NullValueText), typeof (string), typeof (UpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(UpDown.OnNullValueTextChanged)));
  internal static readonly DependencyProperty AnimationShiftProperty = DependencyProperty.Register(nameof (AnimationShift), typeof (double), typeof (UpDown), (PropertyMetadata) new UIPropertyMetadata((object) 0.0));
  public static readonly DependencyProperty IsValueNegativeProperty = DependencyProperty.Register(nameof (IsValueNegative), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false, new PropertyChangedCallback(UpDown.OnIsValueNegativeChanged)));
  public static readonly DependencyProperty CursorBackgroundProperty = DependencyProperty.Register(nameof (CursorBackground), typeof (Brush), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CursorBorderBrushProperty = DependencyProperty.Register(nameof (CursorBorderBrush), typeof (Brush), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CursorWidthProperty = DependencyProperty.Register(nameof (CursorWidth), typeof (double), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CursorBorderThicknessProperty = DependencyProperty.Register(nameof (CursorBorderThickness), typeof (Thickness), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CursorTemplateProperty = DependencyProperty.Register(nameof (CursorTemplate), typeof (ControlTemplate), typeof (UpDown), new PropertyMetadata((object) null, new PropertyChangedCallback(UpDown.OnCursorTemplateChanged)));
  public static readonly DependencyProperty CursorVisibleProperty = DependencyProperty.Register(nameof (CursorVisible), typeof (bool), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty CursorPositionProperty = DependencyProperty.Register(nameof (CursorPosition), typeof (Thickness), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register(nameof (SelectionBrush), typeof (Brush), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EnableRangeAdornerProperty = DependencyProperty.Register(nameof (EnableRangeAdorner), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false));
  public static readonly DependencyProperty RangeAdornerBackgroundProperty = DependencyProperty.Register(nameof (RangeAdornerBackground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) Brushes.LightGray));
  public static readonly DependencyProperty EnableExtendedScrollingProperty = DependencyProperty.Register(nameof (EnableExtendedScrolling), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false));
  public static readonly DependencyProperty UpDownForegroundProperty = DependencyProperty.Register(nameof (UpDownForeground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));
  public static readonly DependencyProperty UpDownBackgroundProperty = DependencyProperty.Register(nameof (UpDownBackground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));
  public static readonly DependencyProperty UpDownBorderBrushProperty = DependencyProperty.Register(nameof (UpDownBorderBrush), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));
  public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof (TextAlignment), typeof (TextAlignment), typeof (UpDown), new PropertyMetadata((object) TextAlignment.Left));
  public static readonly DependencyProperty ApplyZeroColorProperty = DependencyProperty.Register(nameof (ApplyZeroColor), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty EnableNegativeColorsProperty = DependencyProperty.Register(nameof (EnableNegativeColors), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (UpDown), new PropertyMetadata((object) CultureInfo.CurrentCulture));
  public static readonly DependencyProperty EnableFocusedColorsProperty = DependencyProperty.Register(nameof (EnableFocusedColors), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(nameof (FocusedBackground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) Brushes.White, new PropertyChangedCallback(UpDown.OnFocusedBackgroundChanged)));
  public static readonly DependencyProperty IsUpdownFocusedProperty = DependencyProperty.Register(nameof (IsUpdownFocused), typeof (bool), typeof (UpDown));
  public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(nameof (FocusedForeground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(UpDown.OnFocusedForegroundChanged)));
  public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(nameof (FocusedBorderBrush), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(UpDown.OnFocusedBorderBrushChanged)));
  public static readonly DependencyProperty NegativeBackgroundProperty = DependencyProperty.Register(nameof (NegativeBackground), typeof (Brush), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty NegativeBorderBrushProperty = DependencyProperty.Register(nameof (NegativeBorderBrush), typeof (Brush), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AllowEditProperty = DependencyProperty.Register(nameof (AllowEdit), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true, new PropertyChangedCallback(UpDown.OnAllowEditChanged)));
  public static readonly DependencyProperty MinValidationProperty = DependencyProperty.Register(nameof (MinValidation), typeof (MinValidation), typeof (UpDown), new PropertyMetadata((object) MinValidation.OnKeyPress, new PropertyChangedCallback(UpDown.OnMinValidationChanged)));
  public static readonly DependencyProperty MaxValidationProperty = DependencyProperty.Register(nameof (MaxValidation), typeof (MaxValidation), typeof (UpDown), new PropertyMetadata((object) MaxValidation.OnKeyPress, new PropertyChangedCallback(UpDown.OnMaxValidationChanged)));
  public static readonly DependencyProperty MinValueOnExceedMinDigitProperty = DependencyProperty.Register(nameof (MinValueOnExceedMinDigit), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty = DependencyProperty.Register(nameof (MaxValueOnExceedMaxDigit), typeof (bool), typeof (UpDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty NegativeForegroundProperty = DependencyProperty.Register(nameof (NegativeForeground), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(UpDown.OnNegativeForegroundChanged)));
  public static readonly DependencyProperty ZeroColorProperty = DependencyProperty.Register(nameof (ZeroColor), typeof (Brush), typeof (UpDown), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(UpDown.OnZeroColorChanged)));
  public static readonly DependencyProperty UseNullOptionProperty = DependencyProperty.Register(nameof (UseNullOption), typeof (bool), typeof (UpDown), new PropertyMetadata((object) false, new PropertyChangedCallback(UpDown.OnUseNullOptionChanged)));
  public static readonly DependencyProperty NumberFormatInfoProperty = DependencyProperty.Register(nameof (NumberFormatInfo), typeof (NumberFormatInfo), typeof (UpDown), new PropertyMetadata((object) null, new PropertyChangedCallback(UpDown.OnNumberFormatInfoChanged)));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (double?), typeof (UpDown), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double?), typeof (UpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(UpDown.OnValueChanged), new CoerceValueCallback(UpDown.CoerceValue)));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (double), typeof (UpDown), new PropertyMetadata((object) double.MinValue, new PropertyChangedCallback(UpDown.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (double), typeof (UpDown), new PropertyMetadata((object) double.MaxValue, new PropertyChangedCallback(UpDown.OnMaxValueChanged)));
  public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof (Step), typeof (double), typeof (UpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(UpDown.OnStepChanged)));
  public static readonly DependencyProperty AnimationSpeedProperty = DependencyProperty.Register(nameof (AnimationSpeed), typeof (double), typeof (UpDown), new PropertyMetadata((object) 0.1));
  private bool nagativevaluechanged;
  private bool minMaxChanged;

  static UpDown()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (UpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (UpDown)));
    UpDown.m_downValue = new RoutedCommand();
    UpDown.m_upValue = new RoutedCommand();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public UpDown()
  {
    this.DefaultStyleKey = (object) typeof (UpDown);
    this.downValueBinding = new CommandBinding((ICommand) UpDown.m_downValue);
    this.downValueBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeDownValue);
    this.upValueBinding = new CommandBinding((ICommand) UpDown.m_upValue);
    this.upValueBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeUpValue);
    this.CommandBindings.Add(this.downValueBinding);
    this.CommandBindings.Add(this.upValueBinding);
    this.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.UpDown_LostKeyboardFocus);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.Loaded += new RoutedEventHandler(this.UpDown_Loaded);
    this.Unloaded += new RoutedEventHandler(this.UpDown_Unloaded);
  }

  private void UpDown_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.textbox == null)
      return;
    this.textbox.mIsLoaded = false;
  }

  private void UpDown_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.textbox == null)
      return;
    this.textbox.mIsLoaded = true;
    double? nullable1 = this.Value;
    if (this.minMaxChanged)
    {
      this.SetValue(UpDown.ValueProperty, (object) nullable1);
      this.minMaxChanged = false;
    }
    if (!this.UseNullOption || this.Value.HasValue)
      return;
    double? nullable2 = this.textbox.Value;
    double? nullable3 = this.Value;
    if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 1 : (nullable2.HasValue != nullable3.HasValue ? 1 : 0)) == 0)
      return;
    this.textbox.SetValue(DoubleTextBox.ValueProperty, (object) nullable1);
  }

  private void UpDown_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
  {
    if (this.MaxValidation == MaxValidation.OnLostFocus)
    {
      NumberFormatInfo numberFormat = this.textbox.GetCulture().NumberFormat;
      if (this.textbox != null && double.TryParse(this.textbox.MaskedText, out double _) && this.MaxValue != double.MaxValue)
      {
        double? nullable1 = this.Value;
        double maxValue1 = this.MaxValue;
        if ((nullable1.GetValueOrDefault() <= maxValue1 ? 0 : (nullable1.HasValue ? 1 : 0)) == 0)
        {
          double? nullable2 = this.textbox.Value;
          double maxValue2 = this.MaxValue;
          if ((nullable2.GetValueOrDefault() <= maxValue2 ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
            goto label_6;
        }
        this.Value = new double?(this.MaxValue);
        if (this.textbox != null)
        {
          this.textbox.Value = this.Value;
          this.textbox.MaskedText = this.Value.Value.ToString("N", (IFormatProvider) numberFormat);
        }
      }
    }
label_6:
    if (this.MinValidation != MinValidation.OnLostFocus)
      return;
    NumberFormatInfo numberFormat1 = this.textbox.GetCulture().NumberFormat;
    if (this.textbox == null || !double.TryParse(this.textbox.MaskedText, out double _) || this.MinValue == double.MinValue)
      return;
    double? nullable3 = this.Value;
    double minValue1 = this.MinValue;
    if ((nullable3.GetValueOrDefault() >= minValue1 ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
    {
      double? nullable4 = this.textbox.Value;
      double minValue2 = this.MinValue;
      if ((nullable4.GetValueOrDefault() >= minValue2 ? 0 : (nullable4.HasValue ? 1 : 0)) == 0)
        return;
    }
    this.Value = new double?(this.MinValue);
    if (this.textbox == null)
      return;
    this.textbox.Value = this.Value;
    this.textbox.MaskedText = this.Value.Value.ToString("N", (IFormatProvider) numberFormat1);
  }

  public event PropertyChangedCallback AllowEditChanged;

  public event PropertyChangedCallback StepChanged;

  public event PropertyChangedCallback UseNullOptionChanged;

  public event PropertyChangedCallback ValueChanged;

  public event UpDown.ValueChangingEventHandler ValueChanging;

  public event PropertyChangedCallback MinValueChanged;

  public event PropertyChangedCallback IsScrollingOnCircleChanged;

  public event PropertyChangedCallback MaxValueChanged;

  public event PropertyChangedCallback NumberFormatInfoChanged;

  public event PropertyChangedCallback ZeroColorChanged;

  public event PropertyChangedCallback NegativeForegroundChanged;

  public event PropertyChangedCallback MinValidationChanged;

  public event PropertyChangedCallback MaxValidationChanged;

  [Obsolete("Event will not help due to internal arhitecture changes")]
  public event PropertyChangedCallback CursorTemplateChanged;

  [Obsolete("Event will not help due to internal arhitecture changes")]
  public event PropertyChangedCallback IsValueNegativeChanged;

  public event PropertyChangedCallback NullValueTextChanged;

  public event PropertyChangedCallback FocusedBackgroundChanged;

  public event PropertyChangedCallback FocusedForegroundChanged;

  public event PropertyChangedCallback FocusedBorderBrushChanged;

  [Obsolete("Property will not help due to internal arhitecture changes")]
  internal double AnimationShift
  {
    get => (double) this.GetValue(UpDown.AnimationShiftProperty);
    set => this.SetValue(UpDown.AnimationShiftProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public bool IsValueNegative
  {
    get => (bool) this.GetValue(UpDown.IsValueNegativeProperty);
    set => this.SetValue(UpDown.IsValueNegativeProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush CursorBackground
  {
    get => (Brush) this.GetValue(UpDown.CursorBackgroundProperty);
    set => this.SetValue(UpDown.CursorBackgroundProperty, (object) value);
  }

  public bool IsScrollingOnCircle
  {
    get => (bool) this.GetValue(UpDown.IsScrollingOnCircleProperty);
    set => this.SetValue(UpDown.IsScrollingOnCircleProperty, (object) value);
  }

  public bool IsReadOnly
  {
    get => (bool) this.GetValue(UpDown.IsReadOnlyProperty);
    set => this.SetValue(UpDown.IsReadOnlyProperty, (object) value);
  }

  public int NumberDecimalDigits
  {
    get => (int) this.GetValue(UpDown.NumberDecimalDigitsProperty);
    set => this.SetValue(UpDown.NumberDecimalDigitsProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush CursorBorderBrush
  {
    get => (Brush) this.GetValue(UpDown.CursorBorderBrushProperty);
    set => this.SetValue(UpDown.CursorBorderBrushProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public double CursorWidth
  {
    get => (double) this.GetValue(UpDown.CursorWidthProperty);
    set => this.SetValue(UpDown.CursorWidthProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Thickness CursorBorderThickness
  {
    get => (Thickness) this.GetValue(UpDown.CursorBorderThicknessProperty);
    set => this.SetValue(UpDown.CursorBorderThicknessProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public ControlTemplate CursorTemplate
  {
    get => (ControlTemplate) this.GetValue(UpDown.CursorTemplateProperty);
    set => this.SetValue(UpDown.CursorTemplateProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool CursorVisible
  {
    get => (bool) this.GetValue(UpDown.CursorVisibleProperty);
    set => this.SetValue(UpDown.CursorVisibleProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  internal Thickness CursorPosition
  {
    get => (Thickness) this.GetValue(UpDown.CursorPositionProperty);
    set => this.SetValue(UpDown.CursorPositionProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(UpDown.SelectionBrushProperty);
    set => this.SetValue(UpDown.SelectionBrushProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Brush UpDownForeground
  {
    get => (Brush) this.GetValue(UpDown.UpDownForegroundProperty);
    set => this.SetValue(UpDown.UpDownForegroundProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Brush UpDownBackground
  {
    get => (Brush) this.GetValue(UpDown.UpDownBackgroundProperty);
    set => this.SetValue(UpDown.UpDownBackgroundProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public Brush UpDownBorderBrush
  {
    get => (Brush) this.GetValue(UpDown.UpDownBorderBrushProperty);
    set => this.SetValue(UpDown.UpDownBorderBrushProperty, (object) value);
  }

  public TextAlignment TextAlignment
  {
    get => (TextAlignment) this.GetValue(UpDown.TextAlignmentProperty);
    set => this.SetValue(UpDown.TextAlignmentProperty, (object) value);
  }

  public bool ApplyZeroColor
  {
    get => (bool) this.GetValue(UpDown.ApplyZeroColorProperty);
    set => this.SetValue(UpDown.ApplyZeroColorProperty, (object) value);
  }

  public bool EnableNegativeColors
  {
    get => (bool) this.GetValue(UpDown.EnableNegativeColorsProperty);
    set => this.SetValue(UpDown.EnableNegativeColorsProperty, (object) value);
  }

  public CultureInfo Culture
  {
    get => (CultureInfo) this.GetValue(UpDown.CultureProperty);
    set => this.SetValue(UpDown.CultureProperty, (object) value);
  }

  public bool EnableFocusedColors
  {
    get => (bool) this.GetValue(UpDown.EnableFocusedColorsProperty);
    set => this.SetValue(UpDown.EnableFocusedColorsProperty, (object) value);
  }

  public Brush FocusedBackground
  {
    get => (Brush) this.GetValue(UpDown.FocusedBackgroundProperty);
    set => this.SetValue(UpDown.FocusedBackgroundProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public bool IsUpdownFocused
  {
    get => (bool) this.GetValue(UpDown.IsUpdownFocusedProperty);
    set => this.SetValue(UpDown.IsUpdownFocusedProperty, (object) value);
  }

  public Brush FocusedForeground
  {
    get => (Brush) this.GetValue(UpDown.FocusedForegroundProperty);
    set => this.SetValue(UpDown.FocusedForegroundProperty, (object) value);
  }

  public Brush FocusedBorderBrush
  {
    get => (Brush) this.GetValue(UpDown.FocusedBorderBrushProperty);
    set => this.SetValue(UpDown.FocusedBorderBrushProperty, (object) value);
  }

  public Brush NegativeBackground
  {
    get => (Brush) this.GetValue(UpDown.NegativeBackgroundProperty);
    set => this.SetValue(UpDown.NegativeBackgroundProperty, (object) value);
  }

  public Brush NegativeBorderBrush
  {
    get => (Brush) this.GetValue(UpDown.NegativeBorderBrushProperty);
    set => this.SetValue(UpDown.NegativeBorderBrushProperty, (object) value);
  }

  public bool AllowEdit
  {
    get => (bool) this.GetValue(UpDown.AllowEditProperty);
    set => this.SetValue(UpDown.AllowEditProperty, (object) value);
  }

  public MinValidation MinValidation
  {
    get => (MinValidation) this.GetValue(UpDown.MinValidationProperty);
    set => this.SetValue(UpDown.MinValidationProperty, (object) value);
  }

  public MaxValidation MaxValidation
  {
    get => (MaxValidation) this.GetValue(UpDown.MaxValidationProperty);
    set => this.SetValue(UpDown.MaxValidationProperty, (object) value);
  }

  public bool MinValueOnExceedMinDigit
  {
    get => (bool) this.GetValue(UpDown.MinValueOnExceedMinDigitProperty);
    set => this.SetValue(UpDown.MinValueOnExceedMinDigitProperty, (object) value);
  }

  public bool MaxValueOnExceedMaxDigit
  {
    get => (bool) this.GetValue(UpDown.MaxValueOnExceedMaxDigitProperty);
    set => this.SetValue(UpDown.MaxValueOnExceedMaxDigitProperty, (object) value);
  }

  public Brush NegativeForeground
  {
    get => (Brush) this.GetValue(UpDown.NegativeForegroundProperty);
    set => this.SetValue(UpDown.NegativeForegroundProperty, (object) value);
  }

  public Brush ZeroColor
  {
    get => (Brush) this.GetValue(UpDown.ZeroColorProperty);
    set => this.SetValue(UpDown.ZeroColorProperty, (object) value);
  }

  public bool UseNullOption
  {
    get => (bool) this.GetValue(UpDown.UseNullOptionProperty);
    set => this.SetValue(UpDown.UseNullOptionProperty, (object) value);
  }

  public NumberFormatInfo NumberFormatInfo
  {
    get => (NumberFormatInfo) this.GetValue(UpDown.NumberFormatInfoProperty);
    set => this.SetValue(UpDown.NumberFormatInfoProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(UpDown.CornerRadiusProperty);
    set => this.SetValue(UpDown.CornerRadiusProperty, (object) value);
  }

  public double? NullValue
  {
    get => (double?) this.GetValue(UpDown.NullValueProperty);
    set => this.SetValue(UpDown.NullValueProperty, (object) value);
  }

  public double? Value
  {
    get => (double?) this.GetValue(UpDown.ValueProperty);
    set => this.SetValue(UpDown.ValueProperty, (object) value);
  }

  public double MinValue
  {
    get => (double) this.GetValue(UpDown.MinValueProperty);
    set => this.SetValue(UpDown.MinValueProperty, (object) value);
  }

  public double MaxValue
  {
    get => (double) this.GetValue(UpDown.MaxValueProperty);
    set => this.SetValue(UpDown.MaxValueProperty, (object) value);
  }

  public double Step
  {
    get => (double) this.GetValue(UpDown.StepProperty);
    set => this.SetValue(UpDown.StepProperty, (object) value);
  }

  public double AnimationSpeed
  {
    get => (double) this.GetValue(UpDown.AnimationSpeedProperty);
    set => this.SetValue(UpDown.AnimationSpeedProperty, (object) value);
  }

  public new bool IsKeyboardFocused
  {
    get
    {
      return this.textbox != null && this.textbox.IsKeyboardFocused || this.Upbutton != null && this.Upbutton.IsKeyboardFocused || this.Downbutton != null && this.Downbutton.IsKeyboardFocused;
    }
  }

  public bool GroupSeperatorEnabled
  {
    get => (bool) this.GetValue(UpDown.GroupSeperatorEnabledProperty);
    set => this.SetValue(UpDown.GroupSeperatorEnabledProperty, (object) value);
  }

  public new bool IsFocused
  {
    get => (bool) this.GetValue(UpDown.IsFocusedProperty);
    internal set => this.SetValue(UpDown.IsFocusedProperty, (object) value);
  }

  public string NullValueText
  {
    get => (string) this.GetValue(UpDown.NullValueTextProperty);
    set => this.SetValue(UpDown.NullValueTextProperty, (object) value);
  }

  public bool EnableRangeAdorner
  {
    get => (bool) this.GetValue(UpDown.EnableRangeAdornerProperty);
    set => this.SetValue(UpDown.EnableRangeAdornerProperty, (object) value);
  }

  public Brush RangeAdornerBackground
  {
    get => (Brush) this.GetValue(UpDown.RangeAdornerBackgroundProperty);
    set => this.SetValue(UpDown.RangeAdornerBackgroundProperty, (object) value);
  }

  public bool EnableExtendedScrolling
  {
    get => (bool) this.GetValue(UpDown.EnableExtendedScrollingProperty);
    set => this.SetValue(UpDown.EnableExtendedScrollingProperty, (object) value);
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(UpDown.EnableTouchProperty);
    set => this.SetValue(UpDown.EnableTouchProperty, (object) value);
  }

  private static void OnFocusedBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnFocusedBackgroundChanged(e);
  }

  private static void OnFocusedForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnFocusedForegroundChanged(e);
  }

  private static void OnFocusedBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnFocusedBorderBrushChanged(e);
  }

  private static void OnIsValueNegativeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnIsValueNegativeChanged(e);
  }

  private static void OnNullValueTextChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnNullValueTextChanged(e);
  }

  private static void OnCursorTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnCursorTemplateChanged(e);
  }

  private static void OnAllowEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnAllowEditChanged(e);
  }

  private static void OnNegativeForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnNegativeForegroundChanged(e);
  }

  private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d).OnMinValueChanged(e);
  }

  private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnMaxValueChanged(e);
  }

  private static void OnNumberFormatInfoChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnNumberFormatInfoChanged(e);
  }

  private static void OnZeroColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnZeroColorChanged(e);
  }

  private static void OnIsScrollingOnClicleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnIsScrollingOnClicleChanged(e);
  }

  private static void OnMaxValidationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnMaxValidationChanged(e);
  }

  private static void OnMinValidationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnMinValidationChanged(e);
  }

  private static void OnUseNullOptionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnUseNullOptionChanged(e);
  }

  private static void OnStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnStepChanged(e);
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UpDown) d)?.OnValueChanged(e);
  }

  protected virtual void OnFocusedBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FocusedBackgroundChanged == null || !(e.OldValue.ToString() != e.NewValue.ToString()))
      return;
    this.FocusedBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FocusedForegroundChanged == null || !(e.OldValue.ToString() != e.NewValue.ToString()))
      return;
    this.FocusedForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnFocusedBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FocusedBorderBrushChanged == null || !(e.OldValue.ToString() != e.NewValue.ToString()))
      return;
    this.FocusedBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnCursorTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CursorTemplateChanged == null)
      return;
    this.CursorTemplateChanged((DependencyObject) this, e);
  }

  protected virtual void OnIsValueNegativeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsValueNegativeChanged == null)
      return;
    this.IsValueNegativeChanged((DependencyObject) this, e);
  }

  protected virtual void OnNullValueTextChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.NullValueTextChanged == null)
      return;
    this.NullValueTextChanged((DependencyObject) this, e);
  }

  private static object CoerceValue(DependencyObject d, object baseValue)
  {
    UpDown upDown = (UpDown) d;
    if (baseValue != null && upDown != null)
    {
      upDown.nagativevaluechanged = false;
      double? nullable1 = (double?) baseValue;
      double? nullable2 = nullable1;
      double minValue = upDown.MinValue;
      if ((nullable2.GetValueOrDefault() >= minValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 && upDown.MinValidation == MinValidation.OnKeyPress)
      {
        upDown.nagativevaluechanged = true;
        upDown.m_oldvalue = nullable1;
        upDown.minMaxChanged = true;
        return (object) upDown.MinValue;
      }
      double? nullable3 = nullable1;
      double maxValue = upDown.MaxValue;
      if ((nullable3.GetValueOrDefault() <= maxValue ? 0 : (nullable3.HasValue ? 1 : 0)) == 0 || upDown.MaxValidation != MaxValidation.OnKeyPress)
        return (object) nullable1;
      upDown.minMaxChanged = true;
      return (object) upDown.MaxValue;
    }
    return upDown != null && upDown.UseNullOption ? (object) upDown.NullValue : baseValue;
  }

  protected virtual void OnAllowEditChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AllowEditChanged == null)
      return;
    this.AllowEditChanged((DependencyObject) this, e);
  }

  protected virtual void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.textbox != null)
    {
      double? nullable = this.Value;
      if ((nullable.GetValueOrDefault() >= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        this.textbox.Foreground = this.NegativeForeground;
      this.textbox.NegativeForeground = this.NegativeForeground;
    }
    if (this.NegativeForegroundChanged == null)
      return;
    this.NegativeForegroundChanged((DependencyObject) this, e);
  }

  private void OnIsScrollingOnClicleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsScrollingOnCircleChanged == null)
      return;
    this.IsScrollingOnCircleChanged((DependencyObject) this, e);
  }

  protected virtual void OnMinValueChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinValueChanged == null)
      return;
    this.MinValueChanged((DependencyObject) this, e);
  }

  protected virtual void OnMaxValueChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MaxValueChanged == null)
      return;
    this.MaxValueChanged((DependencyObject) this, e);
  }

  protected virtual void OnNumberFormatInfoChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.NumberFormatInfoChanged == null)
      return;
    this.NumberFormatInfoChanged((DependencyObject) this, e);
  }

  protected virtual void OnZeroColorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ZeroColorChanged == null)
      return;
    this.ZeroColorChanged((DependencyObject) this, e);
  }

  protected virtual void OnMaxValidationChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MaxValidationChanged == null)
      return;
    this.MaxValidationChanged((DependencyObject) this, e);
  }

  protected virtual void OnMinValidationChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinValidationChanged == null)
      return;
    this.MinValidationChanged((DependencyObject) this, e);
  }

  protected virtual void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.UseNullOptionChanged == null)
      return;
    this.UseNullOptionChanged((DependencyObject) this, e);
  }

  protected virtual void OnStepChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.StepChanged == null)
      return;
    this.StepChanged((DependencyObject) this, e);
  }

  protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
  {
    this.m_exvalue = (double?) e.OldValue;
    this.m_value = (double?) e.NewValue;
    double num = 0.0;
    double? nullable1 = this.m_value;
    double minValue = this.MinValue;
    if ((nullable1.GetValueOrDefault() >= minValue ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
      num = this.MinValue;
    double? nullable2 = this.m_value;
    double maxValue = this.MaxValue;
    if ((nullable2.GetValueOrDefault() <= maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
      num = this.MaxValue;
    if (!this.UseNullOption && !this.Value.HasValue)
      this.Value = new double?(num);
    if (num < 0.0)
    {
      double? nullable3 = this.Value;
      if ((nullable3.GetValueOrDefault() >= 0.0 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && this.EnableNegativeColors && this.textbox != null)
      {
        this.textbox.Foreground = this.NegativeForeground;
        this.textbox.NegativeForeground = this.NegativeForeground;
      }
    }
    this.UpdateBackground();
    if (this.Upbutton != null && this.Downbutton != null && (this.Upbutton.IsPressed || this.Downbutton.IsPressed))
      this.Animation();
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((DependencyObject) this, e);
  }

  private void ChangeUpValue(object sender, ExecutedRoutedEventArgs e) => this.ChangeValue(true);

  private void ChangeDownValue(object sender, ExecutedRoutedEventArgs e) => this.ChangeValue(false);

  private void ChangeValue(bool IsUp)
  {
    if (this.textbox == null)
      return;
    if (IsUp)
    {
      if (this.Value.HasValue && !double.IsNaN(this.Value.Value))
      {
        double? nullable1 = this.Value;
        double step = this.Step;
        double? nullable2 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() + step) : new double?();
        double maxValue = this.MaxValue;
        if ((nullable2.GetValueOrDefault() > maxValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        {
          if (DoubleValueHandler.doubleValueHandler == null)
            return;
          DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this.textbox, true);
          return;
        }
      }
      if (!this.Value.HasValue && this.Step <= this.MaxValue && this.Step >= this.MinValue)
      {
        if (DoubleValueHandler.doubleValueHandler == null)
          return;
        DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this.textbox, true);
      }
      else if (!this.Value.HasValue && this.Step <= this.MaxValue && this.Step <= this.MinValue)
      {
        this.Value = new double?(this.MinValue);
      }
      else
      {
        double? nullable = this.Value;
        double maxValue = this.MaxValue;
        if ((nullable.GetValueOrDefault() != maxValue ? 1 : (!nullable.HasValue ? 1 : 0)) == 0)
          return;
        this.Value = new double?(this.MaxValue);
      }
    }
    else
    {
      if (this.Value.HasValue && !double.IsNaN(this.Value.Value))
      {
        double? nullable3 = this.Value;
        double step = this.Step;
        double? nullable4 = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() - step) : new double?();
        double minValue = this.MinValue;
        if ((nullable4.GetValueOrDefault() < minValue ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
        {
          if (DoubleValueHandler.doubleValueHandler == null)
            return;
          DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this.textbox, false);
          return;
        }
      }
      if (!this.Value.HasValue && this.Step >= this.MinValue)
      {
        if (DoubleValueHandler.doubleValueHandler == null)
          return;
        DoubleValueHandler.doubleValueHandler.HandleUpDownKey(this.textbox, false);
      }
      else
      {
        double? nullable = this.Value;
        double minValue = this.MinValue;
        if ((nullable.GetValueOrDefault() != minValue ? 1 : (!nullable.HasValue ? 1 : 0)) == 0)
          return;
        this.Value = new double?(this.MinValue);
      }
    }
  }

  private void UpdateBackground()
  {
    if (this.textbox == null)
      return;
    if (this.Downbutton != null && this.Upbutton != null && (this.Upbutton.IsPressed || this.Downbutton.IsPressed || this.textbox.IsFocused) && this.EnableFocusedColors)
      this.IsUpdownFocused = true;
    else if (this.Downbutton != null && this.Upbutton != null && !this.Upbutton.IsPressed && !this.Downbutton.IsPressed && !this.textbox.IsFocused && this.EnableFocusedColors)
    {
      this.IsUpdownFocused = false;
    }
    else
    {
      double? nullable1 = this.Value;
      if ((nullable1.GetValueOrDefault() >= 0.0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0 && this.EnableNegativeColors && this.NegativeBackground != null)
      {
        this.IsValueNegative = true;
      }
      else
      {
        double? nullable2 = this.Value;
        if ((nullable2.GetValueOrDefault() <= 0.0 ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
          return;
        this.IsValueNegative = false;
      }
    }
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (!this.AllowEdit)
      e.Handled = true;
    base.OnPreviewKeyDown(e);
    if (ModifierKeys.Shift == Keyboard.Modifiers && e.Key == Key.Tab)
    {
      this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
      e.Handled = true;
    }
    if (!Keyboard.IsKeyDown(Key.Delete) || !this.UseNullOption || this.Value.HasValue)
      return;
    this.SetValue(UpDown.ValueProperty, (object) this.NullValue);
  }

  private void UpDown_LostFocus(object sender, RoutedEventArgs e)
  {
    this.IsFocused = false;
    if (this.EnableFocusedColors)
      this.IsUpdownFocused = false;
    DoubleTextBox textbox = this.textbox;
    if (textbox != null && textbox.IsNegative && this.EnableNegativeColors && this.NegativeBackground != null)
      this.IsValueNegative = true;
    if (!this.nagativevaluechanged)
      return;
    double? oldvalue = this.m_oldvalue;
    double minValue = this.MinValue;
    if ((oldvalue.GetValueOrDefault() >= minValue ? 0 : (oldvalue.HasValue ? 1 : 0)) == 0)
      return;
    this.textbox.MaskedText = this.MinValue.ToString();
    this.Value = new double?(this.MinValue);
  }

  private void UpDown_GotFocus(object sender, RoutedEventArgs e)
  {
    this.IsFocused = true;
    if (this.EnableFocusedColors)
      this.IsUpdownFocused = true;
    if (this.UseNullOption && !this.Value.HasValue)
      return;
    DoubleTextBox textbox1 = this.textbox;
    if (textbox1 != null && !textbox1.IsNegative)
      this.IsValueNegative = false;
    DoubleTextBox textbox2 = this.textbox;
    if (textbox2 == null || textbox2.Text == null || !textbox2.TextSelectionOnFocus || !this.IsFocused)
      return;
    e.Handled = true;
    this.textbox.Focus();
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new UpDownAutomationPeer(this);
  }

  private void Animation()
  {
    if (!this.IsLoaded || this.textbox == null || this.t1 == null)
      return;
    this.textbox.WatermarkVisibility = Visibility.Collapsed;
    this.textbox.Visibility = Visibility.Visible;
    this.textbox.Opacity = 1.0;
    this.textbox.RenderTransform = (Transform) new TranslateTransform();
    this.t1.RenderTransform = (Transform) new TranslateTransform();
    this.t1.Foreground = this.textbox.Foreground;
    this.t1.Visibility = Visibility.Visible;
    this.t1.Background = this.textbox.Background;
    if (this.NumberDecimalDigits >= 0)
      this.t1.NumberDecimalDigits = this.NumberDecimalDigits;
    this.t1.Value = this.m_exvalue;
    Storyboard storyboard = new Storyboard();
    DoubleAnimation animation1 = new DoubleAnimation();
    TranslateTransform renderTransform1 = (TranslateTransform) this.textbox.RenderTransform;
    DoubleAnimation animation2 = new DoubleAnimation();
    TranslateTransform renderTransform2 = (TranslateTransform) this.t1.RenderTransform;
    storyboard.Children.Add((Timeline) animation1);
    storyboard.Children.Add((Timeline) animation2);
    animation1.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
    animation2.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
    animation1.From = new double?(this.Upbutton.IsPressed ? this.border.ActualHeight : -this.border.ActualHeight);
    animation1.To = new double?(0.0);
    renderTransform1.BeginAnimation(TranslateTransform.YProperty, (AnimationTimeline) animation1);
    animation2.From = new double?(0.0);
    animation2.To = new double?(this.Upbutton.IsPressed ? -this.border.ActualHeight : this.border.ActualHeight);
    renderTransform2.BeginAnimation(TranslateTransform.YProperty, (AnimationTimeline) animation2);
    this.m_exvalue = this.m_value;
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Loaded -= new RoutedEventHandler(this.UpDown_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.UpDown_Unloaded);
    if (this.textbox != null)
    {
      this.textbox.ValueChanging -= new DoubleTextBox.ValueChangingEventHandler(this.textbox_ValueChanging);
      this.textbox.Dispose();
      this.textbox = (DoubleTextBox) null;
    }
    if (this.t1 != null)
    {
      this.t1.Dispose();
      this.t1 = (DoubleTextBox) null;
    }
    if (this.downValueBinding != null)
    {
      this.downValueBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeDownValue);
      this.downValueBinding = (CommandBinding) null;
    }
    if (this.upValueBinding != null)
    {
      this.upValueBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeUpValue);
      this.upValueBinding = (CommandBinding) null;
    }
    if (this.m_value.HasValue)
      this.m_value = new double?();
    if (this.m_exvalue.HasValue)
      this.m_exvalue = new double?();
    if (this.m_oldvalue.HasValue)
      this.m_oldvalue = new double?();
    if (this.Upbutton != null)
      this.Upbutton = (RepeatButton) null;
    if (this.Downbutton != null)
      this.Downbutton = (RepeatButton) null;
    this.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.UpDown_LostKeyboardFocus);
    this.GotFocus -= new RoutedEventHandler(this.UpDown_GotFocus);
    this.LostFocus -= new RoutedEventHandler(this.UpDown_LostFocus);
    this.CommandBindings.Clear();
  }

  public void SelectAll()
  {
    if (this.textbox == null)
      return;
    this.textbox.Focus();
    this.textbox.SelectAll();
  }

  public void Dispose() => this.Dispose(true);

  public override void OnApplyTemplate()
  {
    if (this.textbox != null)
      this.textbox.ValueChanging -= new DoubleTextBox.ValueChangingEventHandler(this.textbox_ValueChanging);
    this.GotFocus -= new RoutedEventHandler(this.UpDown_GotFocus);
    this.LostFocus -= new RoutedEventHandler(this.UpDown_LostFocus);
    base.OnApplyTemplate();
    this.textbox = (DoubleTextBox) this.GetTemplateChild("DoubleTextBox");
    if (this.textbox != null)
      this.textbox.isUpDownDoubleTextBox = true;
    this.Upbutton = (RepeatButton) this.GetTemplateChild("upbutton");
    this.Downbutton = (RepeatButton) this.GetTemplateChild("downbutton");
    if (this.Upbutton != null)
      AutomationProperties.SetName((DependencyObject) this.Upbutton, "Up");
    if (this.Downbutton != null)
      AutomationProperties.SetName((DependencyObject) this.Downbutton, "Down");
    this.border = (Border) this.GetTemplateChild("Border");
    this.t1 = (DoubleTextBox) this.GetTemplateChild("textBox");
    this.GotFocus += new RoutedEventHandler(this.UpDown_GotFocus);
    this.LostFocus += new RoutedEventHandler(this.UpDown_LostFocus);
    this.UpDownForeground = this.Foreground;
    if (this.textbox != null)
    {
      double? nullable = this.Value;
      if ((nullable.GetValueOrDefault() >= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 && this.EnableNegativeColors)
        this.textbox.Foreground = this.NegativeForeground;
      this.textbox.NegativeForeground = this.NegativeForeground;
      if (this.NumberFormatInfo != null)
      {
        string numberGroupSeparator = this.NumberFormatInfo.NumberGroupSeparator;
        this.textbox.NumberGroupSeparator = this.NumberFormatInfo.NumberGroupSeparator.Equals(string.Empty) || char.IsLetterOrDigit(numberGroupSeparator, 0) || numberGroupSeparator.Length != 1 ? this.Culture.NumberFormat.NumberGroupSeparator : this.NumberFormatInfo.NumberGroupSeparator;
      }
      BindingUtils.SetBinding((DependencyObject) this.textbox, (object) this, DoubleTextBox.ScrollIntervalProperty, (object) UpDown.StepProperty);
    }
    if (this.textbox == null)
      return;
    this.textbox.ValueChanging += new DoubleTextBox.ValueChangingEventHandler(this.textbox_ValueChanging);
  }

  private void textbox_ValueChanging(object sender, ValueChangingEventArgs e)
  {
    if (this.ValueChanging == null)
      return;
    this.ValueChanging((object) this, e);
  }

  public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);
}
