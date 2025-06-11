// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.EditorBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class EditorBase : TextBox, IDisposable
{
  internal bool minusPressed;
  private AdornerLayer aLayer;
  private TextBoxSelectionAdorner txtSelectionAdorner1;
  private ExtendedScrollingAdorner vAdorner;
  public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof (ReadOnly), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnCornerRadiusChanged)));
  public static readonly DependencyProperty ShowSpinButtonProperty = DependencyProperty.Register(nameof (ShowSpinButton), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnEnableTouchChanged)));
  public static readonly DependencyProperty EnableRangeAdornerProperty = DependencyProperty.Register(nameof (EnableRangeAdorner), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableExtendedScrollingProperty = DependencyProperty.Register(nameof (EnableExtendedScrolling), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnEnableExtendedScrollingChanged)));
  public static readonly DependencyProperty RangeAdornerBackgroundProperty = DependencyProperty.Register(nameof (RangeAdornerBackground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) Brushes.LightGray));
  internal static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(nameof (FocusedBackground), typeof (Brush), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedBackgroundChanged)));
  internal static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(nameof (FocusedForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedForegroundChanged)));
  public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(nameof (FocusedBorderBrush), typeof (Brush), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedBorderBrushChanged)));
  public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof (ReadOnlyBackground), typeof (Brush), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnReadOnlyBackgroundChanged)));
  public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register(nameof (SelectionForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  public static readonly DependencyProperty EnableFocusColorsProperty = DependencyProperty.Register(nameof (EnableFocusColors), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  internal bool allowchange;
  public static readonly DependencyProperty PasteModeProperty = DependencyProperty.Register(nameof (PasteMode), typeof (PasteMode), typeof (EditorBase), new PropertyMetadata((object) PasteMode.Default));
  public static readonly DependencyProperty PositiveForegroundProperty = DependencyProperty.Register(nameof (PositiveForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnPositiveForegroundChanged)));
  public static readonly DependencyProperty EditorForegroundProperty = DependencyProperty.Register(nameof (EditorForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Black), new PropertyChangedCallback(EditorBase.OnForegroundChanged)));
  public static readonly DependencyProperty NegativeForegroundProperty = DependencyProperty.Register(nameof (NegativeForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(EditorBase.OnNegativeForegroundChanged)));
  public static readonly DependencyProperty ApplyNegativeForegroundProperty = DependencyProperty.Register(nameof (ApplyNegativeForeground), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnApplyNegativeForegroundChanged)));
  public static readonly DependencyProperty IsNegativeProperty = DependencyProperty.Register(nameof (IsNegative), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnIsNegativeChanged)));
  public static readonly DependencyProperty IsZeroProperty = DependencyProperty.Register(nameof (IsZero), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnIsZeroChanged)));
  public static readonly DependencyProperty MaxValidationProperty = DependencyProperty.Register(nameof (MaxValidation), typeof (MaxValidation), typeof (EditorBase), new PropertyMetadata((object) MaxValidation.OnKeyPress));
  public static readonly DependencyProperty MinValidationProperty = DependencyProperty.Register(nameof (MinValidation), typeof (MinValidation), typeof (EditorBase), new PropertyMetadata((object) MinValidation.OnKeyPress, new PropertyChangedCallback(EditorBase.OnMinValidationChanged)));
  public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty = DependencyProperty.Register(nameof (MaxValueOnExceedMaxDigit), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty MinValueOnExceedMinDigitProperty = DependencyProperty.Register(nameof (MinValueOnExceedMinDigit), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsNullProperty = DependencyProperty.Register(nameof (IsNull), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnIsNullChanged)));
  public static readonly DependencyProperty UseNullOptionProperty = DependencyProperty.Register(nameof (UseNullOption), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnUseNullOptionChanged)));
  public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (EditorBase), new PropertyMetadata((object) CultureInfo.CurrentCulture, new PropertyChangedCallback(EditorBase.OnCultureChanged)));
  public static readonly DependencyProperty ZeroColorProperty = DependencyProperty.Register(nameof (ZeroColor), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(EditorBase.OnZeroNegativeColorChanged)));
  public static readonly DependencyProperty ApplyZeroColorProperty = DependencyProperty.Register(nameof (ApplyZeroColor), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false, new PropertyChangedCallback(EditorBase.OnApplyZeroColorChanged)));
  public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.Register(nameof (NumberFormat), typeof (NumberFormatInfo), typeof (EditorBase), new PropertyMetadata((object) null, new PropertyChangedCallback(EditorBase.OnNumberFormatChanged)));
  public new static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register(nameof (IsUndoEnabled), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) true, new PropertyChangedCallback(EditorBase.OnIsUndoEnabledChanged)));
  public static readonly DependencyProperty MaskedTextProperty = DependencyProperty.Register(nameof (MaskedText), typeof (string), typeof (EditorBase), new PropertyMetadata((object) string.Empty));
  public static DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(nameof (WatermarkTemplate), typeof (DataTemplate), typeof (EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnWaterMarkTemplateChanged)));
  public static DependencyProperty WatermarkTextProperty = DependencyProperty.Register(nameof (WatermarkText), typeof (string), typeof (EditorBase), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(EditorBase.OnWaterMarkTextChanged)));
  public static DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register(nameof (WatermarkVisibility), typeof (Visibility), typeof (EditorBase), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(EditorBase.OnWatermarkVisibilityPropertyChanged), new CoerceValueCallback(EditorBase.CoerceWatermarkVisibility)));
  public static readonly DependencyProperty ContentElementVisibilityProperty = DependencyProperty.Register(nameof (ContentElementVisibility), typeof (Visibility), typeof (EditorBase), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty WatermarkTextForegroundProperty = DependencyProperty.Register(nameof (WatermarkTextForeground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty WatermarkBackgroundProperty = DependencyProperty.Register(nameof (WatermarkBackground), typeof (Brush), typeof (EditorBase), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty WatermarkOpacityProperty = DependencyProperty.Register(nameof (WatermarkOpacity), typeof (double), typeof (EditorBase), new PropertyMetadata((object) 0.5));
  public static readonly DependencyProperty WatermarkTextIsVisibleProperty = DependencyProperty.Register(nameof (WatermarkTextIsVisible), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsScrollingOnCircleProperty = DependencyProperty.Register(nameof (IsScrollingOnCircle), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty EnterToMoveNextProperty = DependencyProperty.Register(nameof (EnterToMoveNext), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) true, new PropertyChangedCallback(EditorBase.OnEnterToMoveNextChanged)));
  public static readonly DependencyProperty TextSelectionOnFocusProperty = DependencyProperty.Register(nameof (TextSelectionOnFocus), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) true, new PropertyChangedCallback(EditorBase.OnTextSelectionOnFocusChanged)));
  public static readonly DependencyProperty IsCaretAnimationEnabledProperty = DependencyProperty.Register(nameof (IsCaretAnimationEnabled), typeof (bool), typeof (EditorBase), new PropertyMetadata((object) false));
  internal static readonly DependencyProperty CaretIndexProperty = DependencyProperty.Register(nameof (CaretIndex), typeof (int), typeof (EditorBase), new PropertyMetadata((object) 0));

  public event PropertyChangedCallback CultureChanged;

  public event PropertyChangedCallback NumberFormatChanged;

  public event PropertyChangedCallback WaterMarkTemplateChanged;

  public event PropertyChangedCallback WaterMarkTextChanged;

  public event PropertyChangedCallback IsUndoEnabledChanged;

  public event PropertyChangedCallback TextSelectionOnFocusChanged;

  public event PropertyChangedCallback NegativeForegroundChanged;

  public event PropertyChangedCallback IsValueNegativeChanged;

  public event PropertyChangedCallback EnterToMoveNextChanged;

  public EditorBase()
  {
    this.MouseDoubleClick -= new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
    this.MouseDoubleClick += new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
    this.Loaded -= new RoutedEventHandler(this.EditorBase_Loaded);
    this.Loaded += new RoutedEventHandler(this.EditorBase_Loaded);
    this.Unloaded += new RoutedEventHandler(this.EditorBase_Unloaded);
  }

  private void EditorBase_Unloaded(object sender, RoutedEventArgs e)
  {
    this.MouseDoubleClick -= new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
    this.Unloaded -= new RoutedEventHandler(this.EditorBase_Unloaded);
  }

  private void EditorBase_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.EnableExtendedScrolling)
    {
      if (this.aLayer != null && this.vAdorner != null)
      {
        this.aLayer.Remove((Adorner) this.vAdorner);
        this.vAdorner = (ExtendedScrollingAdorner) null;
        this.aLayer = (AdornerLayer) null;
      }
      if (this.aLayer == null)
        this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
      if (this.aLayer != null && this.vAdorner == null)
        this.vAdorner = new ExtendedScrollingAdorner((UIElement) this);
      if (this.aLayer != null && this.vAdorner != null && (this.aLayer.GetAdorners((UIElement) this) == null || this.aLayer.GetAdorners((UIElement) this).Length > 0 && this.aLayer.GetAdorners((UIElement) this)[0] != this.vAdorner))
        this.aLayer.Add((Adorner) this.vAdorner);
    }
    if (!this.EnableTouch)
      return;
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if (this.aLayer == null || this.txtSelectionAdorner1 != null)
      return;
    this.txtSelectionAdorner1 = new TextBoxSelectionAdorner((UIElement) this);
    this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
  }

  private void EditorBase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    this.SelectAll();
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

  [Browsable(false)]
  [Obsolete("Use IsReadOnly Property")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool ReadOnly
  {
    get => (bool) this.GetValue(EditorBase.ReadOnlyProperty);
    set => this.SetValue(EditorBase.ReadOnlyProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(EditorBase.CornerRadiusProperty);
    set => this.SetValue(EditorBase.CornerRadiusProperty, (object) value);
  }

  public static void OnCornerRadiusChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
  }

  public bool ShowSpinButton
  {
    get => (bool) this.GetValue(EditorBase.ShowSpinButtonProperty);
    set => this.SetValue(EditorBase.ShowSpinButtonProperty, (object) value);
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(EditorBase.EnableTouchProperty);
    set => this.SetValue(EditorBase.EnableTouchProperty, (object) value);
  }

  public static void OnEnableTouchChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is EditorBase))
      return;
    (obj as EditorBase).OnEnableTouchChanged(args);
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
      this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
    }
    else
    {
      if (this.aLayer == null)
        return;
      this.aLayer.Remove((Adorner) this.txtSelectionAdorner1);
    }
  }

  public bool EnableRangeAdorner
  {
    get => (bool) this.GetValue(EditorBase.EnableRangeAdornerProperty);
    set => this.SetValue(EditorBase.EnableRangeAdornerProperty, (object) value);
  }

  public bool EnableExtendedScrolling
  {
    get => (bool) this.GetValue(EditorBase.EnableExtendedScrollingProperty);
    set => this.SetValue(EditorBase.EnableExtendedScrollingProperty, (object) value);
  }

  public static void OnEnableExtendedScrollingChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is EditorBase))
      return;
    (obj as EditorBase).OnEnableExtendedScrollingChanged(args);
  }

  private void OnEnableExtendedScrollingChanged(DependencyPropertyChangedEventArgs args)
  {
    if (!this.IsLoaded)
      return;
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if ((bool) args.NewValue)
    {
      if (this.aLayer == null)
        return;
      this.vAdorner = new ExtendedScrollingAdorner((UIElement) this);
      this.aLayer.Add((Adorner) this.vAdorner);
    }
    else
    {
      if (this.aLayer == null || this.vAdorner == null)
        return;
      this.aLayer.Remove((Adorner) this.vAdorner);
    }
  }

  public Brush RangeAdornerBackground
  {
    get => (Brush) this.GetValue(EditorBase.RangeAdornerBackgroundProperty);
    set => this.SetValue(EditorBase.RangeAdornerBackgroundProperty, (object) value);
  }

  internal Brush FocusedBackground
  {
    get => (Brush) this.GetValue(EditorBase.FocusedBackgroundProperty);
    set => this.SetValue(EditorBase.FocusedBackgroundProperty, (object) value);
  }

  public static void OnFocusedBackgroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
  }

  internal Brush FocusedForeground
  {
    get => (Brush) this.GetValue(EditorBase.FocusedForegroundProperty);
    set => this.SetValue(EditorBase.FocusedForegroundProperty, (object) value);
  }

  public static void OnFocusedForegroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnFocusedForegroundChanged(args);
  }

  private void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush FocusedBorderBrush
  {
    get => (Brush) this.GetValue(EditorBase.FocusedBorderBrushProperty);
    set => this.SetValue(EditorBase.FocusedBorderBrushProperty, (object) value);
  }

  public static void OnFocusedBorderBrushChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush ReadOnlyBackground
  {
    get => (Brush) this.GetValue(EditorBase.ReadOnlyBackgroundProperty);
    set => this.SetValue(EditorBase.ReadOnlyBackgroundProperty, (object) value);
  }

  public static void OnReadOnlyBackgroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public Brush SelectionForeground
  {
    get => (Brush) this.GetValue(EditorBase.SelectionForegroundProperty);
    set => this.SetValue(EditorBase.SelectionForegroundProperty, (object) value);
  }

  public bool EnableFocusColors
  {
    get => (bool) this.GetValue(EditorBase.EnableFocusColorsProperty);
    set => this.SetValue(EditorBase.EnableFocusColorsProperty, (object) value);
  }

  public PasteMode PasteMode
  {
    get => (PasteMode) this.GetValue(EditorBase.PasteModeProperty);
    set => this.SetValue(EditorBase.PasteModeProperty, (object) value);
  }

  public CultureInfo Culture
  {
    get => (CultureInfo) this.GetValue(EditorBase.CultureProperty);
    set => this.SetValue(EditorBase.CultureProperty, (object) value);
  }

  public NumberFormatInfo NumberFormat
  {
    get => (NumberFormatInfo) this.GetValue(EditorBase.NumberFormatProperty);
    set => this.SetValue(EditorBase.NumberFormatProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Brush EditorForeground
  {
    get => (Brush) this.GetValue(EditorBase.EditorForegroundProperty);
    set => this.SetValue(EditorBase.EditorForegroundProperty, (object) value);
  }

  public Brush PositiveForeground
  {
    get => (Brush) this.GetValue(EditorBase.PositiveForegroundProperty);
    set => this.SetValue(EditorBase.PositiveForegroundProperty, (object) value);
  }

  public bool ApplyNegativeForeground
  {
    get => (bool) this.GetValue(EditorBase.ApplyNegativeForegroundProperty);
    set => this.SetValue(EditorBase.ApplyNegativeForegroundProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public bool IsNegative
  {
    get => (bool) this.GetValue(EditorBase.IsNegativeProperty);
    set => this.SetValue(EditorBase.IsNegativeProperty, (object) value);
  }

  public Brush NegativeForeground
  {
    get => (Brush) this.GetValue(EditorBase.NegativeForegroundProperty);
    set => this.SetValue(EditorBase.NegativeForegroundProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool IsZero
  {
    get => (bool) this.GetValue(EditorBase.IsZeroProperty);
    set => this.SetValue(EditorBase.IsZeroProperty, (object) value);
  }

  public bool ApplyZeroColor
  {
    get => (bool) this.GetValue(EditorBase.ApplyZeroColorProperty);
    set => this.SetValue(EditorBase.ApplyZeroColorProperty, (object) value);
  }

  public Brush ZeroColor
  {
    get => (Brush) this.GetValue(EditorBase.ZeroColorProperty);
    set => this.SetValue(EditorBase.ZeroColorProperty, (object) value);
  }

  public bool UseNullOption
  {
    get => (bool) this.GetValue(EditorBase.UseNullOptionProperty);
    set => this.SetValue(EditorBase.UseNullOptionProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool IsNull
  {
    get => (bool) this.GetValue(EditorBase.IsNullProperty);
    set => this.SetValue(EditorBase.IsNullProperty, (object) value);
  }

  public MaxValidation MaxValidation
  {
    get => (MaxValidation) this.GetValue(EditorBase.MaxValidationProperty);
    set => this.SetValue(EditorBase.MaxValidationProperty, (object) value);
  }

  public MinValidation MinValidation
  {
    get => (MinValidation) this.GetValue(EditorBase.MinValidationProperty);
    set => this.SetValue(EditorBase.MinValidationProperty, (object) value);
  }

  public bool MaxValueOnExceedMaxDigit
  {
    get => (bool) this.GetValue(EditorBase.MaxValueOnExceedMaxDigitProperty);
    set => this.SetValue(EditorBase.MaxValueOnExceedMaxDigitProperty, (object) value);
  }

  public bool MinValueOnExceedMinDigit
  {
    get => (bool) this.GetValue(EditorBase.MinValueOnExceedMinDigitProperty);
    set => this.SetValue(EditorBase.MinValueOnExceedMinDigitProperty, (object) value);
  }

  public new bool IsUndoEnabled
  {
    get => (bool) this.GetValue(EditorBase.IsUndoEnabledProperty);
    set => this.SetValue(EditorBase.IsUndoEnabledProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public string MaskedText
  {
    get => (string) this.GetValue(TextBox.TextProperty);
    set
    {
      this.allowchange = false;
      this.SetValue(TextBox.TextProperty, (object) value);
      this.SetValue(EditorBase.MaskedTextProperty, (object) value);
    }
  }

  public static void OnPositiveForegroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnPositiveForegroundChanged(args);
  }

  protected void OnPositiveForegroundChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsZero && this.ApplyZeroColor || !this.IsNegative)
      return;
    int num = this.ApplyNegativeForeground ? 1 : 0;
  }

  public static void OnApplyNegativeForegroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnApplyNegativeForegroundChanged(args);
  }

  protected void OnApplyNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnMinValidationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnZeroNegativeColorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnZeroNegativeColorChanged(args);
  }

  protected void OnZeroNegativeColorChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnApplyZeroColorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnApplyZeroColorChanged(args);
  }

  protected void OnApplyZeroColorChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public DataTemplate WatermarkTemplate
  {
    get => (DataTemplate) this.GetValue(EditorBase.WatermarkTemplateProperty);
    set => this.SetValue(EditorBase.WatermarkTemplateProperty, (object) value);
  }

  public static void OnWaterMarkTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnWaterMarkTemplateChanged(args);
  }

  protected void OnWaterMarkTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.WaterMarkTemplateChanged == null)
      return;
    this.WaterMarkTemplateChanged((DependencyObject) this, args);
  }

  public string WatermarkText
  {
    set => this.SetValue(EditorBase.WatermarkTextProperty, (object) value);
    get => (string) this.GetValue(EditorBase.WatermarkTextProperty);
  }

  public static void OnWaterMarkTextChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnWaterMarkTextChanged(args);
  }

  protected void OnWaterMarkTextChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.WaterMarkTextChanged == null)
      return;
    this.WaterMarkTextChanged((DependencyObject) this, args);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Visibility WatermarkVisibility
  {
    set => this.SetValue(EditorBase.WatermarkVisibilityProperty, (object) value);
    get => (Visibility) this.GetValue(EditorBase.WatermarkVisibilityProperty);
  }

  private static object CoerceWatermarkVisibility(DependencyObject d, object baseValue)
  {
    EditorBase editorBase = (EditorBase) d;
    if (editorBase.WatermarkTextIsVisible && (Visibility) baseValue == Visibility.Visible)
    {
      editorBase.ContentElementVisibility = Visibility.Collapsed;
      return (object) Visibility.Visible;
    }
    editorBase.ContentElementVisibility = Visibility.Visible;
    return (object) Visibility.Collapsed;
  }

  public static void OnWatermarkVisibilityPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnWatermarkVisibilityPropertyChanged(args);
  }

  protected void OnWatermarkVisibilityPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Visibility ContentElementVisibility
  {
    get => (Visibility) this.GetValue(EditorBase.ContentElementVisibilityProperty);
    set => this.SetValue(EditorBase.ContentElementVisibilityProperty, (object) value);
  }

  public Brush WatermarkTextForeground
  {
    get => (Brush) this.GetValue(EditorBase.WatermarkTextForegroundProperty);
    set => this.SetValue(EditorBase.WatermarkTextForegroundProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Brush WatermarkBackground
  {
    get => (Brush) this.GetValue(EditorBase.WatermarkBackgroundProperty);
    set => this.SetValue(EditorBase.WatermarkBackgroundProperty, (object) value);
  }

  public double WatermarkOpacity
  {
    get => (double) this.GetValue(EditorBase.WatermarkOpacityProperty);
    set => this.SetValue(EditorBase.WatermarkOpacityProperty, (object) value);
  }

  public bool WatermarkTextIsVisible
  {
    get => (bool) this.GetValue(EditorBase.WatermarkTextIsVisibleProperty);
    set => this.SetValue(EditorBase.WatermarkTextIsVisibleProperty, (object) value);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);
    if (!this.IsNull)
      return;
    this.WatermarkVisibility = Visibility.Visible;
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    DoubleValueHandler.doubleValueHandler.AllowSelectionStart = false;
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

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    this.WatermarkVisibility = Visibility.Collapsed;
    if (!this.TextSelectionOnFocus)
      return;
    this.SelectAll();
  }

  public static void OnIsUndoEnabledChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnIsUndoEnabledChanged(args);
  }

  protected void OnIsUndoEnabledChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsUndoEnabledChanged == null)
      return;
    this.IsUndoEnabledChanged((DependencyObject) this, args);
  }

  public static void OnNegativeForegroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnNegativeForegroundChanged(args);
  }

  protected void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NegativeForegroundChanged == null)
      return;
    this.NegativeForegroundChanged((DependencyObject) this, args);
  }

  public static void OnEnterToMoveNextChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnEnterToMoveNextChanged(args);
  }

  protected void OnEnterToMoveNextChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.EnterToMoveNextChanged == null)
      return;
    this.EnterToMoveNextChanged((DependencyObject) this, args);
  }

  public static void OnForegroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnForegroundChanged(args);
  }

  protected void OnForegroundChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsZero && this.ApplyZeroColor || !this.IsNegative)
      return;
    int num = this.ApplyNegativeForeground ? 1 : 0;
  }

  public static void OnUseNullOptionChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnUseNullOptionChanged(args);
  }

  public virtual void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnIsNegativeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnIsNegativeChanged(args);
  }

  protected void OnIsNegativeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsValueNegativeChanged == null)
      return;
    this.IsValueNegativeChanged((DependencyObject) this, args);
  }

  public static void OnIsZeroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnIsZeroChanged(args);
  }

  protected void OnIsZeroChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnIsNullChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnIsNullChanged(args);
  }

  protected void OnIsNullChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnCultureChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnCultureChanged(args);
  }

  protected void OnCultureChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CultureChanged != null)
      this.CultureChanged((DependencyObject) this, args);
    this.OnCultureChanged();
  }

  public static void OnNumberFormatChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnNumberFormatChanged(args);
  }

  protected void OnNumberFormatChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.NumberFormatChanged != null)
      this.NumberFormatChanged((DependencyObject) this, args);
    this.OnNumberFormatChanged();
  }

  public static void OnTextSelectionOnFocusChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EditorBase) obj)?.OnTextSelectionOnFocusChanged(args);
  }

  protected void OnTextSelectionOnFocusChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.TextSelectionOnFocusChanged == null)
      return;
    this.TextSelectionOnFocusChanged((DependencyObject) this, args);
  }

  public bool IsScrollingOnCircle
  {
    get => (bool) this.GetValue(EditorBase.IsScrollingOnCircleProperty);
    set => this.SetValue(EditorBase.IsScrollingOnCircleProperty, (object) value);
  }

  public bool EnterToMoveNext
  {
    get => (bool) this.GetValue(EditorBase.EnterToMoveNextProperty);
    set => this.SetValue(EditorBase.EnterToMoveNextProperty, (object) value);
  }

  public bool TextSelectionOnFocus
  {
    get => (bool) this.GetValue(EditorBase.TextSelectionOnFocusProperty);
    set => this.SetValue(EditorBase.TextSelectionOnFocusProperty, (object) value);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    if (e.Key == Key.Return && !this.EnterToMoveNext && this.SelectionStart + 1 <= this.MaskedText.Length)
      ++this.SelectionStart;
    if (ModifierKeys.Control != Keyboard.Modifiers)
      return;
    int key1 = (int) e.Key;
    int key2 = (int) e.Key;
    int key3 = (int) e.Key;
  }

  internal virtual void OnCultureChanged()
  {
  }

  internal virtual void OnNumberFormatChanged()
  {
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public bool IsCaretAnimationEnabled
  {
    get => (bool) this.GetValue(EditorBase.IsCaretAnimationEnabledProperty);
    set => this.SetValue(EditorBase.IsCaretAnimationEnabledProperty, (object) value);
  }

  internal new int CaretIndex
  {
    get => this.SelectionStart;
    set
    {
      this.SelectionStart = value;
      this.SetValue(EditorBase.CaretIndexProperty, (object) value);
    }
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.MouseDoubleClick -= new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
    this.Loaded -= new RoutedEventHandler(this.EditorBase_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.EditorBase_Unloaded);
    this.aLayer = (AdornerLayer) null;
    if (this.txtSelectionAdorner1 != null)
    {
      this.txtSelectionAdorner1.Dispose();
      this.txtSelectionAdorner1 = (TextBoxSelectionAdorner) null;
    }
    this.vAdorner = (ExtendedScrollingAdorner) null;
  }

  public void Dispose() => this.Dispose(true);
}
