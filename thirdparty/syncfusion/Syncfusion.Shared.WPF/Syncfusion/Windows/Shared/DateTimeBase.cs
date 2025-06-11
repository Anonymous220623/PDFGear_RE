// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DateTimeBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class DateTimeBase : TextBox
{
  [Obsolete("ReadOnly Property is deprecated, use IsReadOnly Property instead")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof (ReadOnly), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CanEditProperty = DependencyProperty.Register(nameof (CanEdit), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CustomPatternProperty = DependencyProperty.Register(nameof (CustomPattern), typeof (string), typeof (DateTimeBase), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(DateTimeBase.OnCustomPatternChanged)));
  public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(nameof (DateTimeFormat), typeof (DateTimeFormatInfo), typeof (DateTimeBase), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeBase.OnDateTimeFormatChanged)));
  public static readonly DependencyProperty MaskedTextProperty = DependencyProperty.Register(nameof (MaskedText), typeof (string), typeof (DateTimeBase), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register(nameof (CultureInfo), typeof (CultureInfo), typeof (DateTimeBase), new PropertyMetadata((object) CultureInfo.CurrentCulture, new PropertyChangedCallback(DateTimeBase.OnCultureChanged)));
  public static readonly DependencyProperty NoneDateTextProperty = DependencyProperty.Register(nameof (NoneDateText), typeof (string), typeof (DateTimeBase), new PropertyMetadata((object) "No date is selected"));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("IsScrollingOnCircle Property is deprecated, use EnableMouseWheelEdit Property instead")]
  [Browsable(false)]
  public static readonly DependencyProperty IsScrollingOnCircleProperty = DependencyProperty.Register(nameof (IsScrollingOnCircle), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsScrollingOnCircleChanged)));
  public static readonly DependencyProperty EnableMouseWheelEditProperty = DependencyProperty.Register(nameof (EnableMouseWheelEdit), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsScrollingOnCircleChanged)));
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static readonly DependencyProperty IncorrectForegroundProperty = DependencyProperty.Register(nameof (IncorrectForeground), typeof (Brush), typeof (DateTimeBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Red)));
  public static readonly DependencyProperty PatternProperty = DependencyProperty.Register(nameof (Pattern), typeof (DateTimePattern), typeof (DateTimeBase), new PropertyMetadata((object) DateTimePattern.ShortDate, new PropertyChangedCallback(DateTimeBase.OnPatternChanged)));
  public static readonly DependencyProperty IsEmptyDateEnabledProperty = DependencyProperty.Register(nameof (IsEmptyDateEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false, new PropertyChangedCallback(DateTimeBase.OnIsEmptyDateEnabledChanged)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false, new PropertyChangedCallback(DateTimeBase.OnIsDropDownOpenChanged)));
  public static readonly DependencyProperty IsEnabledRepeatButtonProperty = DependencyProperty.Register(nameof (IsEnabledRepeatButton), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsVisibleRepeatButtonProperty = DependencyProperty.Register(nameof (IsVisibleRepeatButton), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false, new PropertyChangedCallback(DateTimeBase.OnIsVisibleRepeatButtonChanged)));
  public static readonly DependencyProperty IsPopupEnabledProperty = DependencyProperty.Register(nameof (IsPopupEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsButtonPopUpEnabledProperty = DependencyProperty.Register(nameof (IsButtonPopUpEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsButtonPopUpEnabledChanged)));
  public static readonly DependencyProperty IsCalendarEnabledProperty = DependencyProperty.Register(nameof (IsCalendarEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsCalendarEnabledChanged)));
  public static readonly DependencyProperty FormatCalendarProperty = DependencyProperty.Register(nameof (FormatCalendar), typeof (System.Globalization.Calendar), typeof (DateTimeBase), new PropertyMetadata(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue.GetType().GetProperty("Calendar").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue, (object[]) null)));
  internal static readonly DependencyProperty UnderlyingDateTimeProperty = DependencyProperty.Register(nameof (UnderlyingDateTime), typeof (DateTime?), typeof (DateTimeBase), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeBase.OnUnderlyingDateTimeChanged)));
  [Obsolete("CorrectForeground property is deprecated, use Foreground instead")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public static readonly DependencyProperty CorrectForegroundProperty = DependencyProperty.Register(nameof (CorrectForeground), typeof (Brush), typeof (DateTimeBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static readonly DependencyProperty IsHoldMaxWidthProperty = DependencyProperty.Register(nameof (IsHoldMaxWidth), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsCultureRightToLeftProperty = DependencyProperty.Register(nameof (IsCultureRightToLeft), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public static readonly DependencyProperty IsAutoCorrectProperty = DependencyProperty.Register(nameof (IsAutoCorrect), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) true));
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public static readonly DependencyProperty UncertainForegroundProperty = DependencyProperty.Register(nameof (UncertainForeground), typeof (Brush), typeof (DateTimeBase), new PropertyMetadata((object) new SolidColorBrush()));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public static readonly DependencyProperty DateValidationModeProperty = DependencyProperty.Register(nameof (DateValidationMode), typeof (DateValidationMode), typeof (DateTimeBase), new PropertyMetadata((object) DateValidationMode.Warning));
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public static readonly DependencyProperty AutoCorrectedHiglightDurationProperty = DependencyProperty.Register(nameof (AutoCorrectedHiglightDuration), typeof (double), typeof (DateTimeBase), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof (IsEditable), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsEditableChanged)));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public static readonly DependencyProperty CaretTemplateProperty = DependencyProperty.Register(nameof (CaretTemplate), typeof (ControlTemplate), typeof (DateTimeBase), new PropertyMetadata((PropertyChangedCallback) null));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public static readonly DependencyProperty IsCaretAnimationProperty = DependencyProperty.Register(nameof (IsCaretAnimation), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register(nameof (IsAnimation), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public static readonly DependencyProperty ScrollDurationProperty = DependencyProperty.Register(nameof (ScrollDuration), typeof (double), typeof (DateTimeBase), new PropertyMetadata((object) 0.0));
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static readonly DependencyProperty IsMaskInputEnabledProperty = DependencyProperty.Register(nameof (IsMaskInputEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty RepeatButtonBorderBrushProperty = DependencyProperty.Register(nameof (RepeatButtonBorderBrush), typeof (Brush), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnRepeatButtonBorderBrushChanged)));
  public static readonly DependencyProperty RepeatButtonBackgroundProperty = DependencyProperty.Register(nameof (RepeatButtonBackground), typeof (Brush), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnRepeatButtonBackgroundChanged)));
  public static readonly DependencyProperty RepeatButtonBorderThicknessProperty = DependencyProperty.Register(nameof (RepeatButtonBorderThickness), typeof (Thickness), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnRepeatButtonBorderThicknessChanged)));
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public static readonly DependencyProperty DownRepeatButtonContentTemplateProperty = DependencyProperty.Register(nameof (DownRepeatButtonContentTemplate), typeof (DataTemplate), typeof (DateTimeBase), new PropertyMetadata((PropertyChangedCallback) null));
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static readonly DependencyProperty UpRepeatButtonContentTemplateProperty = DependencyProperty.Register(nameof (UpRepeatButtonContentTemplate), typeof (DataTemplate), typeof (DateTimeBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty UpRepeatButtonMarginProperty = DependencyProperty.Register(nameof (UpRepeatButtonMargin), typeof (Thickness), typeof (DateTimeBase), new PropertyMetadata((object) new Thickness(), new PropertyChangedCallback(DateTimeBase.OnUpRepeatButtonMarginChanged)));
  public static readonly DependencyProperty DownRepeatButtonMarginProperty = DependencyProperty.Register(nameof (DownRepeatButtonMargin), typeof (Thickness), typeof (DateTimeBase), new PropertyMetadata((object) new Thickness(), new PropertyChangedCallback(DateTimeBase.OnDownRepeatButtonMarginChanged)));
  public static readonly DependencyProperty SelectWholeContentProperty = DependencyProperty.Register(nameof (SelectWholeContent), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty PopupDelayProperty = DependencyProperty.Register(nameof (PopupDelay), typeof (TimeSpan), typeof (DateTimeBase), new PropertyMetadata((object) new TimeSpan()));
  public static readonly DependencyProperty UpRepeatButtonTemplateProperty = DependencyProperty.Register(nameof (UpRepeatButtonTemplate), typeof (ControlTemplate), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnUpRepeatButtonTemplateChanged)));
  public static readonly DependencyProperty DownRepeatButtonTemplateProperty = DependencyProperty.Register(nameof (DownRepeatButtonTemplate), typeof (ControlTemplate), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnDownRepeatButtonTemplateChanged)));
  public static readonly DependencyProperty DropDownButtonTemplateProperty = DependencyProperty.Register(nameof (DropDownButtonTemplate), typeof (ControlTemplate), typeof (DateTimeBase), new PropertyMetadata(new PropertyChangedCallback(DateTimeBase.OnDropDownButtonTemplateChanged)));
  public static readonly DependencyProperty ContentElementVisibilityProperty = DependencyProperty.Register(nameof (ContentElementVisibility), typeof (Visibility), typeof (DateTimeBase), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register(nameof (WatermarkVisibility), typeof (Visibility), typeof (DateTimeBase), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(DateTimeBase.OnWatermarkVisibilityChanged), new CoerceValueCallback(DateTimeBase.CoerceWatermarkVisibility)));
  public static readonly DependencyProperty IsWatchEnabledProperty = DependencyProperty.Register(nameof (IsWatchEnabled), typeof (bool), typeof (DateTimeBase), new PropertyMetadata((object) true, new PropertyChangedCallback(DateTimeBase.OnIsWatchEnabledChanged)));

  public event PropertyChangedCallback IsDropDownOpenChanged;

  public event PropertyChangedCallback UnderlyingDateTimeChanged;

  public event PropertyChangedCallback IsScrollingOnCircleChanged;

  public event PropertyChangedCallback CustomPatternChanged;

  public event PropertyChangedCallback PatternChanged;

  public event PropertyChangedCallback IsWatchEnabledChanged;

  public event PropertyChangedCallback IsEmptyDateEnabledChanged;

  public event PropertyChangedCallback IsButtonPopUpEnabledChanged;

  public event PropertyChangedCallback IsCalendarEnabledChanged;

  public event PropertyChangedCallback IsVisibleRepeatButtonChanged;

  public event PropertyChangedCallback RepeatButtonBackgroundChanged;

  public event PropertyChangedCallback RepeatButtonBorderBrushChanged;

  public event PropertyChangedCallback RepeatButtonBorderThicknessChanged;

  public event PropertyChangedCallback UpRepeatButtonMarginChanged;

  public event PropertyChangedCallback DownRepeatButtonMarginChanged;

  public event PropertyChangedCallback UpRepeatButtonTemplateChanged;

  public event PropertyChangedCallback DownRepeatButtonTemplateChanged;

  protected override void OnContextMenuOpening(ContextMenuEventArgs e)
  {
    e.Handled = false;
    base.OnContextMenuOpening(e);
  }

  protected override void OnDragEnter(DragEventArgs e)
  {
    e.Handled = true;
    base.OnDragEnter(e);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Use IsReadOnly Property")]
  public bool ReadOnly
  {
    get => (bool) this.GetValue(DateTimeBase.ReadOnlyProperty);
    set => this.SetValue(DateTimeBase.ReadOnlyProperty, (object) value);
  }

  public event PropertyChangedCallback CultureInfoChanged;

  public string CustomPattern
  {
    get => (string) this.GetValue(DateTimeBase.CustomPatternProperty);
    set => this.SetValue(DateTimeBase.CustomPatternProperty, (object) value);
  }

  public CultureInfo CultureInfo
  {
    get => (CultureInfo) this.GetValue(DateTimeBase.CultureInfoProperty);
    set => this.SetValue(DateTimeBase.CultureInfoProperty, (object) value);
  }

  public DateTimeFormatInfo DateTimeFormat
  {
    get => (DateTimeFormatInfo) this.GetValue(DateTimeBase.DateTimeFormatProperty);
    set => this.SetValue(DateTimeBase.DateTimeFormatProperty, (object) value);
  }

  public string NoneDateText
  {
    get => (string) this.GetValue(DateTimeBase.NoneDateTextProperty);
    set => this.SetValue(DateTimeBase.NoneDateTextProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("IsScrollingOnCircle property is deprecated, use EnableMouseWheelEdit instead")]
  public bool IsScrollingOnCircle
  {
    get => (bool) this.GetValue(DateTimeBase.IsScrollingOnCircleProperty);
    set => this.SetValue(DateTimeBase.IsScrollingOnCircleProperty, (object) value);
  }

  public bool EnableMouseWheelEdit
  {
    get => (bool) this.GetValue(DateTimeBase.EnableMouseWheelEditProperty);
    set => this.SetValue(DateTimeBase.EnableMouseWheelEditProperty, (object) value);
  }

  public DateTimePattern Pattern
  {
    get => (DateTimePattern) this.GetValue(DateTimeBase.PatternProperty);
    set => this.SetValue(DateTimeBase.PatternProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public string MaskedText
  {
    get => (string) this.GetValue(DateTimeBase.MaskedTextProperty);
    set
    {
      this.SetValue(TextBox.TextProperty, (object) value);
      this.SetValue(DateTimeBase.MaskedTextProperty, (object) value);
    }
  }

  public bool CanEdit
  {
    get => (bool) this.GetValue(DateTimeBase.CanEditProperty);
    set => this.SetValue(DateTimeBase.CanEditProperty, (object) value);
  }

  public static void OnCustomPatternChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnCustomPatternChanged(args);
  }

  protected void OnCustomPatternChanged(DependencyPropertyChangedEventArgs args)
  {
    this.BasePropertiesChanged();
    if (this.CustomPatternChanged == null)
      return;
    this.CustomPatternChanged((DependencyObject) this, args);
  }

  public static void OnIsScrollingOnCircleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsScrollingOnCircleChanged(args);
  }

  protected void OnIsScrollingOnCircleChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsScrollingOnCircleChanged == null)
      return;
    this.IsScrollingOnCircleChanged((DependencyObject) this, args);
  }

  public static void OnIsEmptyDateEnabledChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsEmptyDateEnabledChanged(args);
  }

  protected void OnIsEmptyDateEnabledChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsEmptyDateEnabledChanged != null)
      this.IsEmptyDateEnabledChanged((DependencyObject) this, args);
    this.BasePropertiesChanged();
  }

  public static void OnIsDropDownOpenChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    DateTimeBase dateTimeBase = (DateTimeBase) obj;
    dateTimeBase.CheckPopUpStatus((DependencyObject) dateTimeBase);
    dateTimeBase?.OnIsDropDownOpenChanged(args);
  }

  public void CheckPopUpStatus(DependencyObject obj)
  {
    if (!(obj is DateTimeEdit))
      return;
    DateTimeEdit dateTimeEdit = obj as DateTimeEdit;
    if (this.IsDropDownOpen)
      dateTimeEdit.OpenPopup();
    else
      dateTimeEdit.ClosePopup();
  }

  protected void OnIsDropDownOpenChanged(DependencyPropertyChangedEventArgs args)
  {
    this.IsPopupEnabled = this.IsDropDownOpen;
    if (this.IsDropDownOpenChanged == null)
      return;
    this.IsDropDownOpenChanged((DependencyObject) this, args);
  }

  public static void OnIsVisibleRepeatButtonChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsVisibleRepeatButtonChanged(args);
  }

  protected void OnIsVisibleRepeatButtonChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsVisibleRepeatButtonChanged == null)
      return;
    this.IsVisibleRepeatButtonChanged((DependencyObject) this, args);
  }

  public static void OnIsButtonPopUpEnabledChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsButtonPopUpEnabledChanged(args);
  }

  protected void OnIsButtonPopUpEnabledChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsButtonPopUpEnabledChanged == null)
      return;
    this.IsButtonPopUpEnabledChanged((DependencyObject) this, args);
  }

  public static void OnIsCalendarEnabledChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsCalendarEnabledChanged(args);
  }

  protected void OnIsCalendarEnabledChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsCalendarEnabledChanged == null)
      return;
    this.IsCalendarEnabledChanged((DependencyObject) this, args);
  }

  public System.Globalization.Calendar FormatCalendar
  {
    get => (System.Globalization.Calendar) this.GetValue(DateTimeBase.FormatCalendarProperty);
    set => this.SetValue(DateTimeBase.FormatCalendarProperty, (object) value);
  }

  internal DateTime? UnderlyingDateTime
  {
    get => (DateTime?) this.GetValue(DateTimeBase.UnderlyingDateTimeProperty);
    set => this.SetValue(DateTimeBase.UnderlyingDateTimeProperty, (object) value);
  }

  public static void OnUnderlyingDateTimeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnUnderlyingDateTimeChanged(args);
  }

  protected void OnUnderlyingDateTimeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.UnderlyingDateTimeChanged == null)
      return;
    this.UnderlyingDateTimeChanged((DependencyObject) this, args);
  }

  [Browsable(false)]
  [Obsolete("Use Foreground Property")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Brush CorrectForeground
  {
    get => (Brush) this.GetValue(DateTimeBase.CorrectForegroundProperty);
    set => this.SetValue(DateTimeBase.CorrectForegroundProperty, (object) value);
  }

  public bool IsCultureRightToLeft
  {
    get => (bool) this.GetValue(DateTimeBase.IsCultureRightToLeftProperty);
    set => this.SetValue(DateTimeBase.IsCultureRightToLeftProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public bool IsHoldMaxWidth
  {
    get => (bool) this.GetValue(DateTimeBase.IsHoldMaxWidthProperty);
    set => this.SetValue(DateTimeBase.IsHoldMaxWidthProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public bool IsAutoCorrect
  {
    get => (bool) this.GetValue(DateTimeBase.IsAutoCorrectProperty);
    set => this.SetValue(DateTimeBase.IsAutoCorrectProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public Brush UncertainForeground
  {
    get => (Brush) this.GetValue(DateTimeBase.UncertainForegroundProperty);
    set => this.SetValue(DateTimeBase.UncertainForegroundProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public DateValidationMode DateValidationMode
  {
    get => (DateValidationMode) this.GetValue(DateTimeBase.DateValidationModeProperty);
    set => this.SetValue(DateTimeBase.DateValidationModeProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public double AutoCorrectedHiglightDuration
  {
    get => (double) this.GetValue(DateTimeBase.AutoCorrectedHiglightDurationProperty);
    set => this.SetValue(DateTimeBase.AutoCorrectedHiglightDurationProperty, (object) value);
  }

  internal bool IsEditable
  {
    get => (bool) this.GetValue(DateTimeBase.IsEditableProperty);
    set => this.SetValue(DateTimeBase.IsEditableProperty, (object) value);
  }

  public static void OnIsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DateTimeBase) d)?.OnIsEditableChangedHanlde(e);
  }

  private void OnIsEditableChangedHanlde(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsEditable)
    {
      this.IsReadOnly = false;
    }
    else
    {
      if (this.IsEditable)
        return;
      this.IsReadOnly = true;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public ControlTemplate CaretTemplate
  {
    get => (ControlTemplate) this.GetValue(DateTimeBase.CaretTemplateProperty);
    set => this.SetValue(DateTimeBase.CaretTemplateProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  public bool IsCaretAnimation
  {
    get => (bool) this.GetValue(DateTimeBase.IsCaretAnimationProperty);
    set => this.SetValue(DateTimeBase.IsCaretAnimationProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public bool IsAnimation
  {
    get => (bool) this.GetValue(DateTimeBase.IsAnimationProperty);
    set => this.SetValue(DateTimeBase.IsAnimationProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public double ScrollDuration
  {
    get => (double) this.GetValue(DateTimeBase.ScrollDurationProperty);
    set => this.SetValue(DateTimeBase.ScrollDurationProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public bool IsMaskInputEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsMaskInputEnabledProperty);
    set => this.SetValue(DateTimeBase.IsMaskInputEnabledProperty, (object) value);
  }

  [Obsolete("Property will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public Brush IncorrectForeground
  {
    get => (Brush) this.GetValue(DateTimeBase.IncorrectForegroundProperty);
    set => this.SetValue(DateTimeBase.IncorrectForegroundProperty, (object) value);
  }

  public Brush RepeatButtonBorderBrush
  {
    get => (Brush) this.GetValue(DateTimeBase.RepeatButtonBorderBrushProperty);
    set => this.SetValue(DateTimeBase.RepeatButtonBorderBrushProperty, (object) value);
  }

  public static void OnRepeatButtonBorderBrushChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnRepeatButtonBorderBrushChanged(args);
  }

  protected void OnRepeatButtonBorderBrushChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.RepeatButtonBorderBrushChanged == null)
      return;
    this.RepeatButtonBorderBrushChanged((DependencyObject) this, args);
  }

  public Brush RepeatButtonBackground
  {
    get => (Brush) this.GetValue(DateTimeBase.RepeatButtonBackgroundProperty);
    set => this.SetValue(DateTimeBase.RepeatButtonBackgroundProperty, (object) value);
  }

  public static void OnRepeatButtonBackgroundChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnRepeatButtonBackgroundChanged(args);
  }

  protected void OnRepeatButtonBackgroundChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.RepeatButtonBackgroundChanged == null)
      return;
    this.RepeatButtonBackgroundChanged((DependencyObject) this, args);
  }

  public Thickness RepeatButtonBorderThickness
  {
    get => (Thickness) this.GetValue(DateTimeBase.RepeatButtonBorderThicknessProperty);
    set => this.SetValue(DateTimeBase.RepeatButtonBorderThicknessProperty, (object) value);
  }

  public static void OnRepeatButtonBorderThicknessChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnRepeatButtonBorderThicknessChanged(args);
  }

  protected void OnRepeatButtonBorderThicknessChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.RepeatButtonBorderThicknessChanged == null)
      return;
    this.RepeatButtonBorderThicknessChanged((DependencyObject) this, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public DataTemplate DownRepeatButtonContentTemplate
  {
    get => (DataTemplate) this.GetValue(DateTimeBase.DownRepeatButtonContentTemplateProperty);
    set => this.SetValue(DateTimeBase.DownRepeatButtonContentTemplateProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property will not help due to internal arhitecture changes")]
  [Browsable(false)]
  public DataTemplate UpRepeatButtonContentTemplate
  {
    get => (DataTemplate) this.GetValue(DateTimeBase.UpRepeatButtonContentTemplateProperty);
    set => this.SetValue(DateTimeBase.UpRepeatButtonContentTemplateProperty, (object) value);
  }

  public Thickness UpRepeatButtonMargin
  {
    get => (Thickness) this.GetValue(DateTimeBase.UpRepeatButtonMarginProperty);
    set => this.SetValue(DateTimeBase.UpRepeatButtonMarginProperty, (object) value);
  }

  public static void OnUpRepeatButtonMarginChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnUpRepeatButtonMarginChanged(args);
  }

  protected void OnUpRepeatButtonMarginChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.UpRepeatButtonMarginChanged == null)
      return;
    this.UpRepeatButtonMarginChanged((DependencyObject) this, args);
  }

  public Thickness DownRepeatButtonMargin
  {
    get => (Thickness) this.GetValue(DateTimeBase.DownRepeatButtonMarginProperty);
    set => this.SetValue(DateTimeBase.DownRepeatButtonMarginProperty, (object) value);
  }

  public static void OnDownRepeatButtonMarginChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnDownRepeatButtonMarginChanged(args);
  }

  protected void OnDownRepeatButtonMarginChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DownRepeatButtonMarginChanged == null)
      return;
    this.DownRepeatButtonMarginChanged((DependencyObject) this, args);
  }

  public bool SelectWholeContent
  {
    get => (bool) this.GetValue(DateTimeBase.SelectWholeContentProperty);
    set => this.SetValue(DateTimeBase.SelectWholeContentProperty, (object) value);
  }

  public TimeSpan PopupDelay
  {
    get => (TimeSpan) this.GetValue(DateTimeBase.PopupDelayProperty);
    set => this.SetValue(DateTimeBase.PopupDelayProperty, (object) value);
  }

  public ControlTemplate UpRepeatButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(DateTimeBase.UpRepeatButtonTemplateProperty);
    set => this.SetValue(DateTimeBase.UpRepeatButtonTemplateProperty, (object) value);
  }

  public static void OnUpRepeatButtonTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnUpRepeatButtonTemplateChanged(args);
  }

  protected void OnUpRepeatButtonTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.UpRepeatButtonTemplateChanged == null)
      return;
    this.UpRepeatButtonTemplateChanged((DependencyObject) this, args);
  }

  public ControlTemplate DownRepeatButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(DateTimeBase.DownRepeatButtonTemplateProperty);
    set => this.SetValue(DateTimeBase.DownRepeatButtonTemplateProperty, (object) value);
  }

  public static void OnDownRepeatButtonTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnDownRepeatButtonTemplateChanged(args);
  }

  protected void OnDownRepeatButtonTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DownRepeatButtonTemplateChanged == null)
      return;
    this.DownRepeatButtonTemplateChanged((DependencyObject) this, args);
  }

  public ControlTemplate DropDownButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(DateTimeBase.DropDownButtonTemplateProperty);
    set => this.SetValue(DateTimeBase.DropDownButtonTemplateProperty, (object) value);
  }

  public static void OnDropDownButtonTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnDropDownButtonTemplateChanged(args);
  }

  protected void OnDropDownButtonTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
  }

  public static void OnCultureChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnCultureChanged(args);
  }

  protected void OnCultureChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.CultureInfoChanged != null)
      this.CultureInfoChanged((DependencyObject) this, args);
    this.BasePropertiesChanged();
  }

  public static void OnPatternChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnPatternChanged(args);
  }

  protected void OnPatternChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.PatternChanged != null)
      this.PatternChanged((DependencyObject) this, args);
    this.BasePropertiesChanged();
  }

  public static void OnDateTimeFormatChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnDateTimeFormatChanged(args);
  }

  protected void OnDateTimeFormatChanged(DependencyPropertyChangedEventArgs args)
  {
    this.BasePropertiesChanged();
  }

  protected virtual void BasePropertiesChanged()
  {
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Visibility ContentElementVisibility
  {
    get => (Visibility) this.GetValue(DateTimeBase.ContentElementVisibilityProperty);
    set => this.SetValue(DateTimeBase.ContentElementVisibilityProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public Visibility WatermarkVisibility
  {
    get => (Visibility) this.GetValue(DateTimeBase.WatermarkVisibilityProperty);
    set => this.SetValue(DateTimeBase.WatermarkVisibilityProperty, (object) value);
  }

  private static object CoerceWatermarkVisibility(DependencyObject d, object baseValue)
  {
    DateTimeBase dateTimeBase = (DateTimeBase) d;
    if (dateTimeBase.IsEmptyDateEnabled && (Visibility) baseValue == Visibility.Visible)
    {
      dateTimeBase.ContentElementVisibility = Visibility.Collapsed;
      return (object) Visibility.Visible;
    }
    dateTimeBase.ContentElementVisibility = Visibility.Visible;
    return (object) Visibility.Collapsed;
  }

  public static void OnWatermarkVisibilityChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnWatermarkVisibilityChanged(args);
  }

  protected void OnWatermarkVisibilityChanged(DependencyPropertyChangedEventArgs args)
  {
    object obj = DateTimeBase.CoerceWatermarkVisibility((DependencyObject) this, (object) this.WatermarkVisibility);
    if (this.WatermarkVisibility == (Visibility) obj)
      return;
    this.WatermarkVisibility = (Visibility) obj;
  }

  public bool IsEmptyDateEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsEmptyDateEnabledProperty);
    set => this.SetValue(DateTimeBase.IsEmptyDateEnabledProperty, (object) value);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(DateTimeBase.IsDropDownOpenProperty);
    set => this.SetValue(DateTimeBase.IsDropDownOpenProperty, (object) value);
  }

  public bool IsEnabledRepeatButton
  {
    get => (bool) this.GetValue(DateTimeBase.IsEnabledRepeatButtonProperty);
    set => this.SetValue(DateTimeBase.IsEnabledRepeatButtonProperty, (object) value);
  }

  public bool IsPopupEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsPopupEnabledProperty);
    set => this.SetValue(DateTimeBase.IsPopupEnabledProperty, (object) value);
  }

  public bool IsWatchEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsWatchEnabledProperty);
    set => this.SetValue(DateTimeBase.IsWatchEnabledProperty, (object) value);
  }

  public static void OnIsWatchEnabledChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeBase) obj)?.OnIsWatchEnabledChanged(args);
  }

  protected void OnIsWatchEnabledChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsWatchEnabledChanged == null)
      return;
    this.IsWatchEnabledChanged((DependencyObject) this, args);
  }

  public bool IsCalendarEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsCalendarEnabledProperty);
    set => this.SetValue(DateTimeBase.IsCalendarEnabledProperty, (object) value);
  }

  public bool IsVisibleRepeatButton
  {
    get => (bool) this.GetValue(DateTimeBase.IsVisibleRepeatButtonProperty);
    set => this.SetValue(DateTimeBase.IsVisibleRepeatButtonProperty, (object) value);
  }

  public bool IsButtonPopUpEnabled
  {
    get => (bool) this.GetValue(DateTimeBase.IsButtonPopUpEnabledProperty);
    set => this.SetValue(DateTimeBase.IsButtonPopUpEnabledProperty, (object) value);
  }

  internal string GetStringPattern(
    DateTimeFormatInfo dateTimeFormatInfo,
    DateTimePattern dateTimePattern,
    string customPattern)
  {
    switch (dateTimePattern)
    {
      case DateTimePattern.CustomPattern:
        return customPattern;
      case DateTimePattern.ShortDate:
        return dateTimeFormatInfo.ShortDatePattern;
      case DateTimePattern.LongDate:
        return dateTimeFormatInfo.LongDatePattern;
      case DateTimePattern.ShortTime:
        return dateTimeFormatInfo.ShortTimePattern;
      case DateTimePattern.LongTime:
        return dateTimeFormatInfo.LongTimePattern;
      case DateTimePattern.FullDateTime:
        return dateTimeFormatInfo.FullDateTimePattern;
      case DateTimePattern.MonthDay:
        return dateTimeFormatInfo.MonthDayPattern;
      case DateTimePattern.RFC1123:
        return dateTimeFormatInfo.RFC1123Pattern;
      case DateTimePattern.SortableDateTime:
        return dateTimeFormatInfo.SortableDateTimePattern;
      case DateTimePattern.UniversalSortableDateTime:
        return dateTimeFormatInfo.UniversalSortableDateTimePattern;
      case DateTimePattern.YearMonth:
        return dateTimeFormatInfo.YearMonthPattern;
      default:
        throw new ArgumentOutOfRangeException("Enum implements incorrect.");
    }
  }
}
