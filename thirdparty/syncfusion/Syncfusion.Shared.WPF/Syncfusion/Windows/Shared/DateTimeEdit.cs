// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DateTimeEdit
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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

[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (DateTimeEdit), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/DateTimeEdit/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (DateTimeEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/DateTimeEdit/Themes/Office2007BlackStyle.xaml")]
public class DateTimeEdit : DateTimeBase, IDisposable
{
  public static readonly DependencyProperty CloseCalendarActionProperty = DependencyProperty.Register(nameof (CloseCalendarAction), typeof (CloseCalendarAction), typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) CloseCalendarAction.SingleClick, new PropertyChangedCallback(DateTimeEdit.OnCloseCalendarActionChanged)));
  internal bool mValueChanged = true;
  internal bool mIsLoaded;
  internal bool mtextboxclicked;
  internal System.DateTime? mValue;
  internal System.DateTime? OldValue;
  internal int mSelectedCollection;
  internal bool mTextInputpartended = true;
  internal string checktext;
  internal bool checkday;
  internal bool checkday1;
  internal bool checkday2;
  internal bool checkmonth;
  internal bool ignoreDateChange;
  internal double calendarHeight;
  private bool IsValidationSuccess = true;
  private ObservableCollection<Syncfusion.Windows.Shared.DateTimeProperties> mDateTimeProperties = new ObservableCollection<Syncfusion.Windows.Shared.DateTimeProperties>();
  public static readonly DependencyProperty EnableAlphaKeyNavigationProperty = DependencyProperty.Register(nameof (EnableAlphaKeyNavigation), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  public static readonly DependencyProperty AbbreviatedMonthNamesProperty = DependencyProperty.Register(nameof (AbbreviatedMonthNames), typeof (string[]), typeof (DateTimeEdit), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeEdit.OnAbbrevaiatedMonthNamesChanged)));
  public static readonly DependencyProperty ShortestDayNamesProperty = DependencyProperty.Register(nameof (ShortestDayNames), typeof (string[]), typeof (DateTimeEdit), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeEdit.OnShortestDayNamesChanged)));
  public static readonly DependencyProperty AllowEnterProperty = DependencyProperty.Register(nameof (AllowEnter), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) true));
  public static readonly DependencyProperty FreezeClockOnEditProperty = DependencyProperty.Register(nameof (FreezeClockOnEdit), typeof (bool), typeof (DateTimeEdit), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty EnableClassicStyleProperty = DependencyProperty.Register(nameof (EnableClassicStyle), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("EnableCombinedStyle property is deprecated, use DropDownView property instead")]
  public static readonly DependencyProperty EnableCombinedStyleProperty = DependencyProperty.Register(nameof (EnableCombinedStyle), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableDeleteKeyProperty = DependencyProperty.Register(nameof (EnableDeleteKey), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(nameof (FocusedBorderBrush), typeof (Brush), typeof (DateTimeEdit), new PropertyMetadata((object) new SolidColorBrush()));
  public static readonly DependencyProperty EnableBackspaceKeyProperty = DependencyProperty.Register(nameof (EnableBackspaceKey), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  public static readonly DependencyProperty OnFocusBehaviorProperty = DependencyProperty.Register(nameof (OnFocusBehavior), typeof (OnFocusBehavior), typeof (DateTimeEdit), new PropertyMetadata((object) OnFocusBehavior.CursorOnFirstCharacter));
  internal bool selectionChanged = true;
  public bool lp;
  public bool lp1;
  private bool isdirectionchangedfromculture;
  private ToggleButton PART_DropDown;
  private Popup PART_Popup;
  private Grid PART_PopupGrid;
  private Border footerBorder;
  private Grid PART_OptionGrid;
  private Grid PART_PopupGrid_Classic;
  private Button todayButton;
  private Button noneButton;
  private Button okButton;
  private Button cancelButton;
  private Canvas _outsidePopupCanvas;
  private Canvas _OutsideCalendarPopupCanvas;
  private Canvas _outsideCanvas;
  private Grid _popupGrid;
  private Grid _root;
  private Syncfusion.Windows.Controls.Calendar _calendar;
  private Syncfusion.Windows.Controls.Calendar _Calendar;
  public static readonly DependencyProperty DateTimeCalenderProperty = DependencyProperty.Register(nameof (DateTimeCalender), typeof (FrameworkElement), typeof (DateTimeEdit), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeEdit.OnPropertyChanged)));
  private Syncfusion.Windows.Controls.Calendar Calendar_Classic;
  private Syncfusion.Windows.Shared.Clock clock;
  private Border PART_ClockBorder;
  private DispatcherTimer DT = new DispatcherTimer();
  public static readonly DependencyProperty ClockProperty = DependencyProperty.Register(nameof (Clock), typeof (FrameworkElement), typeof (DateTimeEdit), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeEdit.OnClockPropertyChanged)));
  private RepeatButton PART_UpButton;
  private RepeatButton PART_DownButton;
  public static readonly DependencyProperty ShowMaskOnNullValueProperty = DependencyProperty.Register(nameof (ShowMaskOnNullValue), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) true));
  public static readonly DependencyProperty TodayButtonActionProperty = DependencyProperty.Register(nameof (TodayButtonAction), typeof (TodayButtonAction), typeof (DateTimeEdit), new PropertyMetadata((object) TodayButtonAction.Date));
  public static readonly DependencyProperty IsNullProperty = DependencyProperty.Register(nameof (IsNull), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false));
  public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(nameof (NullValue), typeof (System.DateTime?), typeof (DateTimeEdit), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(nameof (DateTime), typeof (System.DateTime?), typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimeEdit.OnDateTimeChanged), new CoerceValueCallback(DateTimeEdit.CoerceDateTime)));
  public static readonly DependencyProperty MinDateTimeProperty = DependencyProperty.Register(nameof (MinDateTime), typeof (System.DateTime), typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue.GetType().GetProperty("Calendar").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue, (object[]) null).GetType().GetProperty("MinSupportedDateTime").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue.GetType().GetProperty("Calendar").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue, (object[]) null), (object[]) null), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimeEdit.OnMinDateTimeChanged)));
  public static readonly DependencyProperty MaxDateTimeProperty = DependencyProperty.Register(nameof (MaxDateTime), typeof (System.DateTime), typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue.GetType().GetProperty("Calendar").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue, (object[]) null).GetType().GetProperty("MaxSupportedDateTime").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue.GetType().GetProperty("Calendar").GetValue(DateTimeBase.CultureInfoProperty.DefaultMetadata.DefaultValue, (object[]) null), (object[]) null), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimeEdit.OnMaxDateTimeChanged)));
  public static readonly DependencyProperty DefaultDatePartProperty = DependencyProperty.Register(nameof (DefaultDatePart), typeof (DateParts), typeof (DateTimeEdit), new PropertyMetadata((object) DateParts.None));
  public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register(nameof (CalendarStyle), typeof (Style), typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DateTimeEdit.OnCalendarStyleChanged)));
  public static readonly DependencyProperty DisableDateSelectionProperty = DependencyProperty.Register(nameof (DisableDateSelection), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) false, new PropertyChangedCallback(DateTimeEdit.OnDisableDateSelectionChanged)));
  public static readonly DependencyProperty DropDownViewProperty = DependencyProperty.Register(nameof (DropDownView), typeof (DropDownViews), typeof (DateTimeEdit), new PropertyMetadata((object) DropDownViews.Calendar, new PropertyChangedCallback(DateTimeEdit.OnDropDownViewChanged)));
  public static readonly DependencyProperty AutoForwardingProperty = DependencyProperty.Register(nameof (AutoForwarding), typeof (bool), typeof (DateTimeEdit), new PropertyMetadata((object) true));

  public event RoutedEventHandler CalendarPopupOpened;

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("ClockPopupOpenedEvent event is deprecated, use ClockPopupOpened event instead")]
  public event RoutedEventHandler ClockPopupOpenedEvent;

  public event RoutedEventHandler ClockPopupOpened;

  [Obsolete("Event will not help due to internal arhitecture changes")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public event PropertyChangedCallback CloseCalendarActionChanged;

  public CloseCalendarAction CloseCalendarAction
  {
    get => (CloseCalendarAction) this.GetValue(DateTimeEdit.CloseCalendarActionProperty);
    set => this.SetValue(DateTimeEdit.CloseCalendarActionProperty, (object) value);
  }

  private static void OnCloseCalendarActionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((DateTimeEdit) d).OnCloseCalendarActionChanged(e);
  }

  protected virtual void OnCloseCalendarActionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CloseCalendarActionChanged == null)
      return;
    this.CloseCalendarActionChanged((DependencyObject) this, e);
  }

  public event PropertyChangedCallback DateTimeChanged;

  public event PropertyChangedCallback MaxDateTimeChanged;

  public event PropertyChangedCallback MinDateTimeChanged;

  public event PropertyChangedCallback DisableDateSelectionChanged;

  public event PropertyChangedCallback DropDownViewChanged;

  public event RoutedEventHandler MonthChanged;

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  internal ObservableCollection<Syncfusion.Windows.Shared.DateTimeProperties> DateTimeProperties
  {
    get
    {
      if (this.mDateTimeProperties == null && DateTimeHandler.dateTimeHandler != null)
        this.mDateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
      return this.mDateTimeProperties;
    }
    set => this.mDateTimeProperties = value;
  }

  public bool EnableAlphaKeyNavigation
  {
    get => (bool) this.GetValue(DateTimeEdit.EnableAlphaKeyNavigationProperty);
    set => this.SetValue(DateTimeEdit.EnableAlphaKeyNavigationProperty, (object) value);
  }

  public string[] AbbreviatedMonthNames
  {
    get => (string[]) this.GetValue(DateTimeEdit.AbbreviatedMonthNamesProperty);
    set => this.SetValue(DateTimeEdit.AbbreviatedMonthNamesProperty, (object) value);
  }

  private static void OnAbbrevaiatedMonthNamesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeEdit).OnAbbrevaiatedMonthNamesChanged(e);
  }

  private void OnAbbrevaiatedMonthNamesChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AbbreviatedMonthNames != null && ((IEnumerable<string>) this.AbbreviatedMonthNames).Count<string>() != 12)
      throw new Exception("12 Month names should be provided");
  }

  public string[] ShortestDayNames
  {
    get => (string[]) this.GetValue(DateTimeEdit.ShortestDayNamesProperty);
    set => this.SetValue(DateTimeEdit.ShortestDayNamesProperty, (object) value);
  }

  private static void OnShortestDayNamesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeEdit).OnShortestDayNamesChanged(e);
  }

  private void OnShortestDayNamesChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ShortestDayNames != null && ((IEnumerable<string>) this.ShortestDayNames).Count<string>() != 7)
      throw new Exception("7 Day names should be provided");
  }

  [Obsolete("AllowEnter property is deprecated, use F4 or Alt + Down or click dropdown button to open popup")]
  public bool AllowEnter
  {
    get => (bool) this.GetValue(DateTimeEdit.AllowEnterProperty);
    set => this.SetValue(DateTimeEdit.AllowEnterProperty, (object) value);
  }

  public bool FreezeClockOnEdit
  {
    get => (bool) this.GetValue(DateTimeEdit.FreezeClockOnEditProperty);
    set => this.SetValue(DateTimeEdit.FreezeClockOnEditProperty, (object) value);
  }

  static DateTimeEdit()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DateTimeEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DateTimeEdit)));
    LicenseHelper.ValidateLicense();
  }

  public DateTimeEdit()
  {
    this.SelectionChanged += new RoutedEventHandler(this.DateTimeEdit_SelectionChanged);
    this.Loaded += new RoutedEventHandler(this.DateTimeEdit_Loaded);
    this.Unloaded += new RoutedEventHandler(this.DateTimeEdit_Unloaded);
    if (DateTimeHandler.dateTimeHandler == null)
      DateTimeHandler.dateTimeHandler = new DateTimeHandler();
    this.DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
    LicenseHelper.ValidateLicense();
  }

  private void DateTimeEdit_Unloaded(object sender, RoutedEventArgs e)
  {
    if (!this.mIsLoaded)
      return;
    this.Unloaded -= new RoutedEventHandler(this.DateTimeEdit_Unloaded);
  }

  private void DisposeCalendar(Syncfusion.Windows.Controls.Calendar calendar)
  {
    if (calendar == null)
      return;
    if (calendar.SelectedDate.HasValue)
      calendar.SelectedDate = new System.DateTime?();
    if (calendar.SelectedDates != null && calendar.SelectedDates.Count > 0)
      calendar.SelectedDates.Clear();
    calendar.MouseLeftButtonDown -= new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
    calendar.MouseRightButtonDown -= new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
    calendar.DayOrMonthPreviewKeyDown -= new RoutedEventHandler(this.Calendar_DayOrMonthPreviewKeyDown);
    calendar.NewDateChanged -= new PropertyChangedCallback(this._calendar_NewDateChanged);
    calendar.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>(this.calender_SelectedDatesChanged);
    if (calendar.BlackoutDates != null)
    {
      calendar.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      calendar.BlackoutDates = (Syncfusion.Windows.Controls.CalendarBlackoutDatesCollection) null;
    }
    calendar.Dispose();
  }

  private void DisposeClock(Syncfusion.Windows.Shared.Clock clock)
  {
    if (this.clock == null)
      return;
    clock.DateTimeChanged -= new PropertyChangedCallback(this.clock_DateTimeChanged);
    clock.Loaded -= new RoutedEventHandler(this.Clock_Loaded);
    clock.Dispose();
  }

  public void Dispose()
  {
    if (this.Calendar_Classic != null)
    {
      this.DisposeCalendar(this.Calendar_Classic);
      this.Calendar_Classic = (Syncfusion.Windows.Controls.Calendar) null;
    }
    if (this._calendar != null)
    {
      this.DisposeCalendar(this._calendar);
      this._calendar = (Syncfusion.Windows.Controls.Calendar) null;
    }
    if (this._Calendar != null)
    {
      this.DisposeCalendar(this._Calendar);
      this._Calendar = (Syncfusion.Windows.Controls.Calendar) null;
    }
    if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar)
    {
      this.DisposeCalendar(this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar);
      this.DateTimeCalender = (FrameworkElement) null;
    }
    if (this.clock != null)
    {
      this.DisposeClock(this.clock);
      this.clock = (Syncfusion.Windows.Shared.Clock) null;
    }
    if (this.Clock != null && this.Clock is Syncfusion.Windows.Shared.Clock)
    {
      this.DisposeClock(this.Clock as Syncfusion.Windows.Shared.Clock);
      this.Clock = (FrameworkElement) null;
    }
    if (this.okButton != null)
    {
      this.okButton.Click -= new RoutedEventHandler(this.OkButton_Click);
      this.okButton = (Button) null;
    }
    if (this.cancelButton != null)
    {
      this.cancelButton.Click -= new RoutedEventHandler(this.CancelButton_Click);
      this.cancelButton = (Button) null;
    }
    if (this.PART_DownButton != null)
    {
      this.PART_DownButton.Click -= new RoutedEventHandler(this.PART_DownButton_Click);
      this.PART_DownButton.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.PART_DownButton_PreviewMouseLeftButtonUp);
      this.PART_DownButton = (RepeatButton) null;
    }
    if (this.PART_UpButton != null)
    {
      this.PART_UpButton.Click -= new RoutedEventHandler(this.PART_UpButton_Click);
      this.PART_UpButton.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.PART_UpButton_PreviewMouseLeftButtonUp);
      this.PART_UpButton = (RepeatButton) null;
    }
    if (this.mValue.HasValue)
      this.mValue = new System.DateTime?();
    if (this.OldValue.HasValue)
      this.OldValue = new System.DateTime?();
    this.ClearValue(DateTimeEdit.OnFocusBehaviorProperty);
    this.ClearValue(DateTimeEdit.DateTimeProperty);
    this.ClearValue(DateTimeEdit.MinDateTimeProperty);
    this.ClearValue(DateTimeEdit.MaxDateTimeProperty);
    if (this.mDateTimeProperties != null)
    {
      this.mDateTimeProperties.Clear();
      this.mDateTimeProperties = (ObservableCollection<Syncfusion.Windows.Shared.DateTimeProperties>) null;
    }
    if (this.DateTimeProperties != null)
    {
      this.DateTimeProperties.Clear();
      this.DateTimeProperties = (ObservableCollection<Syncfusion.Windows.Shared.DateTimeProperties>) null;
    }
    DateTimeHandler.dateTimeHandler = (DateTimeHandler) null;
    this.Resources = (ResourceDictionary) null;
    this.Loaded -= new RoutedEventHandler(this.DateTimeEdit_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.DateTimeEdit_Unloaded);
    this.SelectionChanged -= new RoutedEventHandler(this.DateTimeEdit_SelectionChanged);
    if (this.PART_Popup != null)
    {
      this.PART_Popup.Closed -= new EventHandler(this.PART_Popup_Closed);
      this.PART_Popup = (Popup) null;
    }
    if (this.PART_DropDown != null)
    {
      this.PART_DropDown.Checked -= new RoutedEventHandler(this.PART_DropDown_Checked);
      this.PART_DropDown.Unchecked -= new RoutedEventHandler(this.PART_DropDown_Unchecked);
      this.PART_DropDown = (ToggleButton) null;
    }
    if (this.todayButton != null)
    {
      this.todayButton.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.Button_Today_PreviewMouseLeftButtonUp);
      this.todayButton = (Button) null;
    }
    if (this.noneButton != null)
    {
      this.noneButton.Click -= new RoutedEventHandler(this.Button_NoDate_Click);
      this.noneButton = (Button) null;
    }
    if (this._outsidePopupCanvas != null)
    {
      this._outsidePopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(this._outsidePopupCanvas_MouseLeftButtonDown);
      this._outsidePopupCanvas = (Canvas) null;
    }
    if (this._OutsideCalendarPopupCanvas != null)
    {
      this._OutsideCalendarPopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(this._OutsideCalendarPopupCanvas_MouseLeftButtonDown);
      this._OutsideCalendarPopupCanvas = (Canvas) null;
    }
    if (this.PART_PopupGrid_Classic != null && this.PART_PopupGrid_Classic.Children != null)
    {
      this.PART_PopupGrid_Classic.Children.Clear();
      this.PART_PopupGrid_Classic = (Grid) null;
    }
    if (this.PART_PopupGrid != null && this.PART_PopupGrid.Children != null)
    {
      this.PART_PopupGrid.Children.Clear();
      this.PART_PopupGrid = (Grid) null;
    }
    if (this._popupGrid != null && this._popupGrid.Children != null)
    {
      this._popupGrid.Children.Clear();
      this._popupGrid = (Grid) null;
    }
    if (this.DT != null)
    {
      this.DT.Tick -= new EventHandler(this.DT_Tick);
      this.DT = (DispatcherTimer) null;
    }
    BindingOperations.ClearAllBindings((DependencyObject) this);
  }

  private void DateTimeEdit_Loaded(object sender, RoutedEventArgs e)
  {
    this.mIsLoaded = true;
    System.DateTime? nullable1 = (System.DateTime?) DateTimeEdit.CoerceDateTime((DependencyObject) this, (object) this.DateTime);
    this.DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
    this.CheckPopUpStatus((DependencyObject) this);
    System.DateTime? nullable2 = nullable1;
    System.DateTime? dateTime = this.DateTime;
    if ((nullable2.HasValue != dateTime.HasValue ? 1 : (!nullable2.HasValue ? 0 : (nullable2.GetValueOrDefault() != dateTime.GetValueOrDefault() ? 1 : 0))) != 0)
    {
      this.DateTime = nullable1;
    }
    else
    {
      if (this.todayButton != null)
      {
        if (System.DateTime.Today < this.MinDateTime || System.DateTime.Today > this.MaxDateTime)
          this.todayButton.IsEnabled = false;
        else
          this.todayButton.IsEnabled = true;
      }
      this.mValue = this.DateTime;
      this.LoadTextBox();
    }
  }

  public bool EnableClassicStyle
  {
    get => (bool) this.GetValue(DateTimeEdit.EnableClassicStyleProperty);
    set => this.SetValue(DateTimeEdit.EnableClassicStyleProperty, (object) value);
  }

  [Obsolete("EnableCombinedStyle property is deprecated, use DropDownView property instead")]
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool EnableCombinedStyle
  {
    get => (bool) this.GetValue(DateTimeEdit.EnableCombinedStyleProperty);
    set => this.SetValue(DateTimeEdit.EnableCombinedStyleProperty, (object) value);
  }

  public bool EnableDeleteKey
  {
    get => (bool) this.GetValue(DateTimeEdit.EnableDeleteKeyProperty);
    set => this.SetValue(DateTimeEdit.EnableDeleteKeyProperty, (object) value);
  }

  public bool EnableBackspaceKey
  {
    get => (bool) this.GetValue(DateTimeEdit.EnableBackspaceKeyProperty);
    set => this.SetValue(DateTimeEdit.EnableBackspaceKeyProperty, (object) value);
  }

  public Brush FocusedBorderBrush
  {
    get => (Brush) this.GetValue(DateTimeEdit.FocusedBorderBrushProperty);
    set => this.SetValue(DateTimeEdit.FocusedBorderBrushProperty, (object) value);
  }

  public OnFocusBehavior OnFocusBehavior
  {
    get => (OnFocusBehavior) this.GetValue(DateTimeEdit.OnFocusBehaviorProperty);
    set => this.SetValue(DateTimeEdit.OnFocusBehaviorProperty, (object) value);
  }

  private void DateTimeEdit_SelectionChanged(object sender, RoutedEventArgs e)
  {
    if (this.DateTimeProperties != null && this.DateTimeProperties.Count > 0 && this.IsFocused && !this.CanEdit && this.selectionChanged && this.Text.Length != this.SelectionLength)
    {
      for (int index = 0; index < this.DateTimeProperties.Count; ++index)
      {
        Syncfusion.Windows.Shared.DateTimeProperties dateTimeProperty = this.DateTimeProperties[index];
        if ((dateTimeProperty != null && dateTimeProperty.StartPosition <= this.SelectionStart && this.SelectionStart < dateTimeProperty.StartPosition + dateTimeProperty.Lenghth || this.SelectionStart == this.Text.Length) && (dateTimeProperty.Type == DateTimeType.others || this.mValueChanged && dateTimeProperty.Type == DateTimeType.Dayname || this.SelectionStart == this.Text.Length))
        {
          int num = index;
          if (this.SelectionStart == this.Text.Length)
            num = this.DateTimeProperties.Count;
          if (this.SelectionStart != this.Text.Length && num != 0 && this.DateTimeProperties[num - 1].Type != DateTimeType.others && this.DateTimeProperties[num - 1].Type != DateTimeType.Dayname)
            dateTimeProperty = this.DateTimeProperties[num - 1];
          else if (this.SelectionStart != this.Text.Length && num < this.DateTimeProperties.Count - 1)
          {
            while ((dateTimeProperty.Type == DateTimeType.Dayname || dateTimeProperty.Pattern == null) && num < this.DateTimeProperties.Count - 1)
              dateTimeProperty = this.DateTimeProperties[++num];
          }
          else
          {
            while ((dateTimeProperty.Type == DateTimeType.Dayname || dateTimeProperty.Pattern == null || num == this.DateTimeProperties.Count && this.SelectionStart == this.Text.Length) && num > 0)
              dateTimeProperty = this.DateTimeProperties[--num];
          }
          if (dateTimeProperty.Pattern != null)
          {
            this.selectionChanged = false;
            if (num >= 0 && this.mTextInputpartended)
              this.mSelectedCollection = num;
            this.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
          }
        }
      }
    }
    if (this.selectionChanged && DateTimeHandler.dateTimeHandler != null)
      DateTimeHandler.dateTimeHandler.HandleSelection(this);
    else
      this.selectionChanged = true;
  }

  internal CultureInfo GetCulture()
  {
    CultureInfo culture = this.CultureInfo == null ? CultureInfo.CurrentCulture.Clone() as CultureInfo : this.CultureInfo.Clone() as CultureInfo;
    if (this.DateTimeFormat != null)
      culture.DateTimeFormat = this.DateTimeFormat;
    if (culture.DateTimeFormat != null && this.Calendar_Classic != null)
      this.Calendar_Classic.FirstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
    this.IsCultureRightToLeft = culture.TextInfo.IsRightToLeft;
    if (!this.IsCultureRightToLeft)
    {
      if (this.lp || this.lp1)
      {
        if (this.FlowDirection == FlowDirection.LeftToRight)
          this.FlowDirection = FlowDirection.RightToLeft;
        else
          this.FlowDirection = FlowDirection.LeftToRight;
        this.lp = false;
        this.lp1 = false;
      }
      if (this.FlowDirection == FlowDirection.RightToLeft && (this.ReadLocalValue(DateTimeBase.IsCultureRightToLeftProperty) == DependencyProperty.UnsetValue || this.isdirectionchangedfromculture))
      {
        this.isdirectionchangedfromculture = false;
        this.FlowDirection = FlowDirection.LeftToRight;
      }
    }
    if (this.IsCultureRightToLeft)
    {
      if (this.lp || this.lp1)
      {
        this.FlowDirection = FlowDirection.RightToLeft;
        this.lp = false;
        this.lp1 = false;
        this.isdirectionchangedfromculture = true;
      }
      if (this.FlowDirection == FlowDirection.LeftToRight)
      {
        this.lp = !this.lp;
        this.FlowDirection = FlowDirection.RightToLeft;
      }
    }
    return culture;
  }

  internal void ReloadTextBox()
  {
    this.DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
    this.LoadTextBox();
  }

  internal System.DateTime? ValidateValue(System.DateTime? Val)
  {
    if (Val.HasValue)
    {
      System.DateTime? nullable = Val;
      System.DateTime maxDateTime = this.MaxDateTime;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() > maxDateTime ? 1 : 0) : 0) != 0)
      {
        Val = new System.DateTime?(this.MaxDateTime);
      }
      else
      {
        System.DateTime? mValue = this.mValue;
        System.DateTime minDateTime = this.MinDateTime;
        if ((mValue.HasValue ? (mValue.GetValueOrDefault() < minDateTime ? 1 : 0) : 0) != 0)
          Val = new System.DateTime?(this.MinDateTime);
      }
    }
    return Val;
  }

  internal void LoadOnValueChanged()
  {
    if (this.DateTime.ToString() == "")
    {
      System.DateTime now = System.DateTime.Now;
    }
    else
    {
      System.DateTime dateTime = this.DateTime.Value;
    }
    int selectedCollection = this.mSelectedCollection;
    this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
    this.selectionChanged = false;
    this.Select(this.DateTimeProperties[selectedCollection].StartPosition, this.DateTimeProperties[selectedCollection].Lenghth);
    this.mSelectedCollection = selectedCollection;
    this.selectionChanged = true;
  }

  internal void LoadTextBox()
  {
    this.UnderlyingDateTime = this.DateTime;
    if (this.DateTime.HasValue)
    {
      int selectedCollection = this.mSelectedCollection;
      if (DateTimeHandler.dateTimeHandler != null)
        this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
      this.selectionChanged = false;
      if (!this.CanEdit && selectedCollection > -1 && selectedCollection < this.DateTimeProperties.Count)
      {
        Syncfusion.Windows.Shared.DateTimeProperties dateTimeProperty = this.DateTimeProperties[selectedCollection];
        while ((dateTimeProperty.Type == DateTimeType.Dayname || dateTimeProperty.Pattern == null) && selectedCollection < this.DateTimeProperties.Count - 1)
        {
          dateTimeProperty = this.DateTimeProperties[++selectedCollection];
          this.mSelectedCollection = selectedCollection;
        }
      }
      if (selectedCollection > -1 && this.DateTimeProperties != null && selectedCollection < this.DateTimeProperties.Count && !this.IsNull)
        this.Select(this.DateTimeProperties[selectedCollection].StartPosition, this.DateTimeProperties[selectedCollection].Lenghth);
      this.mSelectedCollection = selectedCollection;
      this.selectionChanged = true;
      this.WatermarkVisibility = Visibility.Collapsed;
    }
    else if (this.IsFocused && this.CanEdit && !this.IsNull)
      this.WatermarkVisibility = Visibility.Collapsed;
    else if (!this.CanEdit && this.IsNull && this.WatermarkVisibility != Visibility.Visible && this.ShowMaskOnNullValue && DateTimeHandler.dateTimeHandler != null)
      this.MaskedText = DateTimeHandler.dateTimeHandler.CreateDisplayText(this);
    else if (this.CanEdit && !this.NullValue.HasValue && this.IsFocused && this.IsNull)
      this.WatermarkVisibility = Visibility.Collapsed;
    else
      this.WatermarkVisibility = Visibility.Visible;
  }

  public FrameworkElement DateTimeCalender
  {
    get => (FrameworkElement) this.GetValue(DateTimeEdit.DateTimeCalenderProperty);
    set => this.SetValue(DateTimeEdit.DateTimeCalenderProperty, (object) value);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    DateTimeEdit dateTimeEdit = d as DateTimeEdit;
    if (e.NewValue is Syncfusion.Windows.Controls.Calendar)
      return;
    if (e.OldValue is Syncfusion.Windows.Controls.Calendar)
      BindingOperations.ClearAllBindings((DependencyObject) (e.OldValue as Syncfusion.Windows.Controls.Calendar));
    dateTimeEdit.UpdateFooterVisiblity();
  }

  private void Popup_OnApplyTemplate()
  {
    if (this.Calendar_Classic != null)
    {
      this.Calendar_Classic.MouseLeftButtonDown -= new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
      this.Calendar_Classic.MouseRightButtonDown -= new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
    }
    if (this.clock != null)
      this.clock.DateTimeChanged -= new PropertyChangedCallback(this.clock_DateTimeChanged);
    this.PART_ClockBorder = this.GetTemplateChild("ClockBorder") as Border;
    if (!DesignerProperties.GetIsInDesignMode((DependencyObject) this) && this.Clock is Syncfusion.Windows.Shared.Clock)
    {
      this.clock = this.Clock as Syncfusion.Windows.Shared.Clock;
      if (this.clock != null)
      {
        string visualStyle = SkinStorage.GetVisualStyle((DependencyObject) this);
        if (!string.IsNullOrEmpty(visualStyle))
          SkinStorage.SetVisualStyle((DependencyObject) this.clock, visualStyle);
        this.clock.DateTimeChanged += new PropertyChangedCallback(this.clock_DateTimeChanged);
        this.clock.Loaded += new RoutedEventHandler(this.Clock_Loaded);
      }
    }
    if (this.PART_Popup != null)
      this.PART_Popup.Closed -= new EventHandler(this.PART_Popup_Closed);
    if (this.PART_DropDown != null)
    {
      this.PART_DropDown.Checked -= new RoutedEventHandler(this.PART_DropDown_Checked);
      this.PART_DropDown.Unchecked -= new RoutedEventHandler(this.PART_DropDown_Unchecked);
    }
    if (this.PART_DropDown != null)
    {
      this.PART_DropDown.Checked -= new RoutedEventHandler(this.PART_DropDown_Checked);
      this.PART_DropDown.Unchecked -= new RoutedEventHandler(this.PART_DropDown_Unchecked);
    }
    if (this.todayButton != null)
      this.todayButton.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.Button_Today_PreviewMouseLeftButtonUp);
    if (this.noneButton != null)
      this.noneButton.Click -= new RoutedEventHandler(this.Button_NoDate_Click);
    if (this._calendar != null)
    {
      this._calendar.DayOrMonthPreviewKeyDown -= new RoutedEventHandler(this.Calendar_DayOrMonthPreviewKeyDown);
      if (this._calendar.BlackoutDates != null)
        this._calendar.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      this._calendar.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>(this.calender_SelectedDatesChanged);
    }
    if (this.Calendar_Classic != null)
    {
      this.Calendar_Classic.DayOrMonthPreviewKeyDown -= new RoutedEventHandler(this.Calendar_DayOrMonthPreviewKeyDown);
      if (this.Calendar_Classic.BlackoutDates != null)
        this.Calendar_Classic.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      this.Calendar_Classic.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>(this.calender_SelectedDatesChanged);
    }
    if (this._outsidePopupCanvas != null)
      this._outsidePopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(this._outsidePopupCanvas_MouseLeftButtonDown);
    if (this._OutsideCalendarPopupCanvas != null)
      this._OutsideCalendarPopupCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(this._OutsideCalendarPopupCanvas_MouseLeftButtonDown);
    this.PART_DropDown = this.GetTemplateChild("PART_DropDown") as ToggleButton;
    this.PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
    this.PART_PopupGrid = this.GetTemplateChild("PART_PopupGrid") as Grid;
    this.footerBorder = this.GetTemplateChild("Footer") as Border;
    this.PART_OptionGrid = this.GetTemplateChild("PART_OptionGrid") as Grid;
    this.PART_PopupGrid_Classic = this.GetTemplateChild("PART_PopupGrid_Classic") as Grid;
    this.todayButton = this.GetTemplateChild("todayButton") as Button;
    this.noneButton = this.GetTemplateChild("noneButton") as Button;
    this.okButton = this.GetTemplateChild("okButton") as Button;
    this.cancelButton = this.GetTemplateChild("cancelButton") as Button;
    this.Calendar_Classic = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
    this._Calendar = this.GetTemplateChild("Calendar") as Syncfusion.Windows.Controls.Calendar;
    if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar)
    {
      Syncfusion.Windows.Controls.Calendar dateTimeCalender = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
      if (this._calendar != null)
      {
        this._calendar.BlackoutDates = dateTimeCalender.BlackoutDates;
        this._calendar.CanBlockWeekEnds = dateTimeCalender.CanBlockWeekEnds;
      }
      if (this.Calendar_Classic != null)
      {
        this.Calendar_Classic.BlackoutDates = dateTimeCalender.BlackoutDates;
        this.Calendar_Classic.CanBlockWeekEnds = dateTimeCalender.CanBlockWeekEnds;
      }
    }
    if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar)
    {
      (this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar).BorderThickness = new Thickness(0.0);
      (this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar).Background = (Brush) Brushes.Transparent;
    }
    if (this.Clock != null && this.Clock is Syncfusion.Windows.Shared.Clock)
    {
      (this.Clock as Syncfusion.Windows.Shared.Clock).FrameBorderThickness = new Thickness(0.0);
      (this.Clock as Syncfusion.Windows.Shared.Clock).FrameBackground = (Brush) Brushes.Transparent;
      (this.Clock as Syncfusion.Windows.Shared.Clock).FrameInnerBorderBrush = (Brush) Brushes.Transparent;
    }
    this.UpdateFooterVisiblity();
    if (this.DisableDateSelection && this._calendar != null)
    {
      this._calendar.DisableDateSelection = this.DisableDateSelection;
      this._calendar.DisplayMode = Syncfusion.Windows.Controls.CalendarMode.Year;
    }
    if (this.PART_DropDown != null)
    {
      AutomationProperties.SetName((DependencyObject) this.PART_DropDown, "Show Calendar");
      this.PART_DropDown.Checked += new RoutedEventHandler(this.PART_DropDown_Checked);
      this.PART_DropDown.Unchecked += new RoutedEventHandler(this.PART_DropDown_Unchecked);
    }
    if (this.noneButton != null)
      this.noneButton.Click += new RoutedEventHandler(this.Button_NoDate_Click);
    if (this.todayButton != null)
      this.todayButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.Button_Today_PreviewMouseLeftButtonUp);
    if (this._calendar != null)
    {
      this._calendar.DayOrMonthPreviewKeyDown += new RoutedEventHandler(this.Calendar_DayOrMonthPreviewKeyDown);
      if (this._calendar.BlackoutDates != null)
        this._calendar.BlackoutDates.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      this._calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(this.calender_SelectedDatesChanged);
      this._calendar.NewDateChanged += new PropertyChangedCallback(this._calendar_NewDateChanged);
    }
    if (this.okButton != null)
    {
      this.okButton.Click -= new RoutedEventHandler(this.OkButton_Click);
      this.okButton.Click += new RoutedEventHandler(this.OkButton_Click);
    }
    if (this.cancelButton != null)
    {
      this.cancelButton.Click -= new RoutedEventHandler(this.CancelButton_Click);
      this.cancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
    }
    if (this.Calendar_Classic != null)
    {
      this.Calendar_Classic.MouseLeftButtonDown += new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
      this.Calendar_Classic.MouseRightButtonDown += new MouseButtonEventHandler(this.Calendar_Classic_MouseButtonDown);
      this.Calendar_Classic.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(this.calender_SelectedDatesChanged);
      this.Calendar_Classic.NewDateChanged += new PropertyChangedCallback(this._calendar_NewDateChanged);
      if (this.Calendar_Classic.BlackoutDates != null)
        this.Calendar_Classic.BlackoutDates.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      this.Calendar_Classic.DayOrMonthPreviewKeyDown += new RoutedEventHandler(this.Calendar_DayOrMonthPreviewKeyDown);
    }
    this._root = this.GetTemplateChild("RootElement") as Grid;
    this._outsideCanvas = this.GetTemplateChild("OutsideCanvas") as Canvas;
    this._outsidePopupCanvas = this.GetTemplateChild("OutsidePopupCanvas") as Canvas;
    this._popupGrid = this.GetTemplateChild("PART_PopupGrid") as Grid;
    this._OutsideCalendarPopupCanvas = this.GetTemplateChild("OutsideCalendarPopupCanvas") as Canvas;
    if (this._OutsideCalendarPopupCanvas != null)
      this._OutsideCalendarPopupCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(this._OutsideCalendarPopupCanvas_MouseLeftButtonDown);
    if (this._outsidePopupCanvas != null)
      this._outsidePopupCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(this._outsidePopupCanvas_MouseLeftButtonDown);
    if (this.PART_Popup == null)
      return;
    this.PART_Popup.Closed += new EventHandler(this.PART_Popup_Closed);
  }

  private void UpdateFooterVisiblity()
  {
    if (this.footerBorder == null)
      return;
    if (this.DateTimeCalender != null && this.DropDownView == DropDownViews.Calendar && !(this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar) || this.Clock != null && this.DropDownView == DropDownViews.Clock && !(this.Clock is Syncfusion.Windows.Shared.Clock))
      this.footerBorder.Visibility = Visibility.Collapsed;
    else
      this.footerBorder.Visibility = Visibility.Visible;
  }

  private void BlackoutDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
    if (calendar != null && calendar.BlackoutDates != null && !string.IsNullOrEmpty(this.DateTime.ToString()) && calendar.BlackoutDates.Contains(this.DateTime.Value))
      throw new ArgumentOutOfRangeException("Value is not valid", "Specified argument was out of the range of valid values");
  }

  private void Calendar_DayOrMonthPreviewKeyDown(object sender, RoutedEventArgs e)
  {
    Syncfusion.Windows.Controls.Calendar calendar = sender as Syncfusion.Windows.Controls.Calendar;
    KeyEventArgs keyEventArgs = (KeyEventArgs) e;
    if (calendar == null || keyEventArgs == null || keyEventArgs.Key != Key.Return && keyEventArgs.Key != Key.Space || calendar.DisplayMode != Syncfusion.Windows.Controls.CalendarMode.Month || !this.IsDropDownOpen)
      return;
    if (calendar != null)
    {
      if (this.DateTime.HasValue)
      {
        System.DateTime dateTime = this.DateTime.Value;
        this.DateTime = new System.DateTime?(new System.DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, calendar.SelectedDate.Value.Day, this.DateTime.Value.Hour, this.DateTime.Value.Minute, this.DateTime.Value.Second, this.DateTime.Value.Millisecond));
      }
      else
        this.DateTime = calendar.SelectedDate;
    }
    this.IsDropDownOpen = false;
  }

  private void Clock_Loaded(object sender, RoutedEventArgs e)
  {
    if (!this.DateTime.HasValue)
      return;
    this.clock.DateTime = this.DateTime.Value;
  }

  private void Button_Today_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsReadOnly)
      this.DateTime = !(System.DateTime.Today < this.MinDateTime) ? (!(System.DateTime.Today > this.MaxDateTime) ? (this.TodayButtonAction != TodayButtonAction.Date ? new System.DateTime?(System.DateTime.Now) : new System.DateTime?(System.DateTime.Today)) : new System.DateTime?(this.MaxDateTime)) : new System.DateTime?(this.MinDateTime);
    this.IsDropDownOpen = false;
    e.Handled = true;
  }

  private void _calendar_NewDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!this.DisableDateSelection)
      return;
    this.DateTime = (System.DateTime?) e.NewValue;
    this.FireMonthChanged();
    if (this.Calendar_Classic == null || this.Calendar_Classic.AllowClose || this.Calendar_Classic.DisplayMode != Syncfusion.Windows.Controls.CalendarMode.Year)
      return;
    this.IsDropDownOpen = false;
  }

  internal bool CheckLeapYear(int value) => value % 4 == 0 && value % 100 != 0 || value % 400 == 0;

  private System.DateTime ValidateYearField(
    Key key,
    System.DateTime currentDate,
    Syncfusion.Windows.Shared.DateTimeProperties characterProperty)
  {
    int result;
    if (int.TryParse(this.SelectedText.ToString(), out result))
    {
      if (key == Key.Up || key == Key.Down)
      {
        if (result == 0)
        {
          if ((key == Key.Up && !this.CheckLeapYear(this.MinDateTime.Year) || key == Key.Down && !this.CheckLeapYear(this.MaxDateTime.Year)) && currentDate.Month == 2 && currentDate.Day > 28)
            currentDate = currentDate.AddDays(-1.0);
          currentDate = key == Key.Up ? new System.DateTime(this.MinDateTime.Year, currentDate.Month, currentDate.Day) : new System.DateTime(this.MaxDateTime.Year, currentDate.Month, currentDate.Day);
        }
        else
        {
          this.selectionChanged = false;
          this.SelectedText = key == Key.Up ? (result + 1).ToString() : (result - 1).ToString();
          this.selectionChanged = true;
          characterProperty.KeyPressCount = 0;
        }
      }
      else
      {
        if (!this.CheckLeapYear(result) && currentDate.Month == 2 && currentDate.Day > 28)
          currentDate = currentDate.AddDays(-1.0);
        string str1 = currentDate.ToString("M/d/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
        string str2 = str1.Remove(str1.Length - 4, 4);
        System.DateTime.TryParse((result != 0 ? (object) str2.Insert(str2.Length, this.SelectedText) : (object) str2.Insert(str2.Length, result.ToString())).ToString(), (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out currentDate);
      }
    }
    return currentDate;
  }

  internal bool ValidateDateTimeField(Key key)
  {
    if (!this.IsReadOnly && !this.CanEdit && this.Text.ToString() != this.MaskedText.ToString() && !string.IsNullOrEmpty(this.mValue.ToString()))
    {
      System.DateTime dateTime1 = this.mValue.Value;
      int month = dateTime1.Month;
      int day = dateTime1.Day;
      int hour = dateTime1.Hour;
      int year = dateTime1.Year;
      int result;
      int.TryParse(this.SelectedText.ToString(), out result);
      for (int index = 0; index < this.DateTimeProperties.Count; ++index)
      {
        Syncfusion.Windows.Shared.DateTimeProperties dateTimeProperty = this.DateTimeProperties[index];
        if (dateTimeProperty != null && dateTimeProperty.StartPosition == this.SelectionStart && !string.IsNullOrEmpty(dateTime1.ToString()))
        {
          if (dateTimeProperty.Type == DateTimeType.year)
          {
            System.DateTime dateTime2 = this.ValidateYearField(key, dateTime1, dateTimeProperty);
            if (result.ToString() != this.SelectedText.ToString() && (key == Key.Up || key == Key.Down))
              return true;
            year = dateTime2.Year;
            month = dateTime2.Month;
            day = dateTime2.Day;
            hour = dateTime1.Hour;
          }
          else
          {
            if (result != 0)
              return false;
            if (dateTimeProperty.Type == DateTimeType.Day)
              day = key == Key.Down ? System.DateTime.DaysInMonth(this.DateTime.Value.Year, this.DateTime.Value.Month) : 1;
            else if (dateTimeProperty.Type == DateTimeType.Hour12)
            {
              hour = key != Key.Down ? 1 : 12;
            }
            else
            {
              if (dateTimeProperty.Type != DateTimeType.Month && dateTimeProperty.Type != DateTimeType.monthname)
                return false;
              month = key != Key.Down ? 1 : 12;
            }
          }
          if (this.mIsLoaded)
          {
            dateTime1 = new System.DateTime(year, month, day, hour, dateTime1.Minute, dateTime1.Second);
            this.DateTime = new System.DateTime?(this.IsBlackoutDate(Key.None, dateTime1));
            this.LoadTextBox();
            this.selectionChanged = false;
            this.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
            this.selectionChanged = true;
            dateTimeProperty.KeyPressCount = 0;
            if (key == Key.Up || key == Key.Down)
              return true;
          }
        }
      }
    }
    return false;
  }

  internal System.DateTime GetValidDateTime()
  {
    System.DateTime result = new System.DateTime();
    bool flag = this.Pattern == DateTimePattern.CustomPattern ? System.DateTime.TryParseExact(this.Text.ToString(), this.CustomPattern, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) : System.DateTime.TryParseExact(this.Text.ToString(), this.GetStringPattern(this.CultureInfo.DateTimeFormat, this.Pattern, ""), (IFormatProvider) this.CultureInfo.DateTimeFormat, DateTimeStyles.None, out result);
    if (!flag)
    {
      flag = System.DateTime.TryParse(this.Text.ToString(), out result);
      if (this.Pattern != DateTimePattern.CustomPattern && flag)
        result = System.DateTime.Parse(this.Text.ToString(), (IFormatProvider) this.GetCulture());
    }
    if (flag && string.IsNullOrEmpty(this.DateTime.ToString()))
      result = this.IsBlackoutDate(Key.None, result);
    return result;
  }

  private Syncfusion.Windows.Controls.Calendar GetCalendar()
  {
    Syncfusion.Windows.Controls.Calendar calendar = (Syncfusion.Windows.Controls.Calendar) null;
    if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar)
      calendar = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
    else if (this._calendar != null)
      calendar = this._calendar;
    return calendar;
  }

  internal System.DateTime IsBlackoutDate(Key key, System.DateTime date)
  {
    Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
    if (calendar != null && calendar.BlackoutDates != null && !string.IsNullOrEmpty(date.ToString()) && calendar.BlackoutDates.Contains(date))
    {
      foreach (Syncfusion.Windows.Controls.CalendarDateRange blackoutDate in (Collection<Syncfusion.Windows.Controls.CalendarDateRange>) calendar.BlackoutDates)
      {
        if (blackoutDate != null && date.Date >= blackoutDate.Start.Date && date.Date <= blackoutDate.End.Date)
        {
          switch (key)
          {
            case Key.None:
              date = blackoutDate.Start.Date.AddDays(-1.0);
              continue;
            case Key.Up:
              date = blackoutDate.End.Date.AddDays(1.0);
              continue;
            case Key.Down:
              date = blackoutDate.Start.Date.AddDays(-1.0);
              continue;
            default:
              continue;
          }
        }
      }
    }
    return date;
  }

  internal bool IsValidDate()
  {
    System.DateTime result = new System.DateTime();
    bool flag = this.Pattern == DateTimePattern.CustomPattern ? System.DateTime.TryParseExact(this.Text.ToString(), this.CustomPattern, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) : System.DateTime.TryParseExact(this.Text.ToString(), this.GetStringPattern(this.CultureInfo.DateTimeFormat, this.Pattern, ""), (IFormatProvider) this.CultureInfo.DateTimeFormat, DateTimeStyles.None, out result);
    if (!flag)
      flag = System.DateTime.TryParse(this.Text.ToString(), out result);
    return flag;
  }

  private bool CanNavigateCalendarElement()
  {
    return !this.IsReadOnly && this.PART_Popup != null && this.PART_Popup.IsOpen && this.IsDropDownOpen && this.DropDownView != DropDownViews.Clock;
  }

  private void UpdateCalendarFocus()
  {
    if (this.PART_Popup == null || !this.PART_Popup.IsOpen || !this.IsDropDownOpen)
      return;
    if (this.DropDownView != DropDownViews.Clock && this.DateTimeCalender != null && !this.DateTimeCalender.IsFocused)
    {
      this.DateTimeCalender.Focus();
    }
    else
    {
      if (this._calendar == null || this._calendar.IsFocused)
        return;
      this._calendar.Focus();
    }
  }

  private bool UpdateCalendarItemFocus(
    Syncfusion.Windows.Controls.Calendar calendar,
    Button noneButton,
    bool CanFocusCalendarItem)
  {
    if (CanFocusCalendarItem)
    {
      if (calendar.MonthControl.GetCalendarDayButtons() != null && calendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month)
      {
        foreach (Syncfusion.Windows.Controls.Primitives.CalendarDayButton calendarDayButton in calendar.MonthControl.GetCalendarDayButtons())
        {
          if (calendarDayButton != null && calendarDayButton.IsSelected)
          {
            calendarDayButton.Focus();
            return true;
          }
        }
      }
      else if (calendar.MonthControl.GetCalendarButtons() != null)
      {
        foreach (Syncfusion.Windows.Controls.Primitives.CalendarButton calendarButton in calendar.MonthControl.GetCalendarButtons())
        {
          if (calendarButton != null && calendarButton.HasSelectedDays)
          {
            calendarButton.Focus();
            return true;
          }
        }
      }
      calendar.MonthControl.FocusDate(calendar.DisplayDate);
      return true;
    }
    if (this.IsEmptyDateEnabled && noneButton != null && noneButton.IsVisible)
    {
      noneButton.Focusable = true;
      noneButton.Focus();
      return true;
    }
    calendar.MonthControl.PreviousButton.Focusable = true;
    calendar.MonthControl.PreviousButton.Focus();
    return true;
  }

  private bool UpdateCalendarElementFocus(
    bool isShiftPressed,
    Syncfusion.Windows.Controls.Calendar calendar,
    Button noneButton,
    Button todayButton,
    Button okButton,
    Button cancelButton)
  {
    Syncfusion.Windows.Controls.Primitives.CalendarDayButton calendarDayButton = calendar.MonthControl.GetFocusedCalendarDayButton();
    Syncfusion.Windows.Controls.Primitives.CalendarButton focusedCalendarButton = calendar.MonthControl.GetFocusedCalendarButton();
    if (!isShiftPressed)
    {
      if (todayButton != null && (calendarDayButton != null && calendarDayButton.IsFocused || focusedCalendarButton != null && focusedCalendarButton.IsFocused))
      {
        if (!todayButton.IsVisible || !todayButton.IsEnabled)
          return this.UpdateCalendarItemFocus(calendar, noneButton, false);
        todayButton.Focusable = true;
        todayButton.Focus();
        return true;
      }
      if (todayButton != null && todayButton.IsFocused)
      {
        if (this.DropDownView != DropDownViews.Combined || this.IsEmptyDateEnabled || noneButton.IsVisible)
          return this.UpdateCalendarItemFocus(calendar, noneButton, false);
        okButton.Focusable = true;
        okButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Calendar && this.IsEmptyDateEnabled && noneButton.IsVisible && noneButton.IsFocused)
      {
        calendar.MonthControl.PreviousButton.Focusable = true;
        calendar.MonthControl.PreviousButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Combined && this.IsEmptyDateEnabled && noneButton.IsVisible && noneButton.IsFocused)
      {
        okButton.Focusable = true;
        okButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Combined && okButton.IsVisible && okButton.IsFocused)
      {
        cancelButton.Focusable = true;
        cancelButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Combined && cancelButton.IsVisible && cancelButton.IsFocused)
      {
        calendar.MonthControl.PreviousButton.Focusable = true;
        calendar.MonthControl.PreviousButton.Focus();
        return true;
      }
      if (calendar.MonthControl.PreviousButton.IsFocused)
      {
        if (calendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Decade)
        {
          calendar.MonthControl.NextButton.Focusable = true;
          calendar.MonthControl.NextButton.Focus();
        }
        else
        {
          calendar.MonthControl.HeaderButton.Focusable = true;
          calendar.MonthControl.HeaderButton.Focus();
        }
        return true;
      }
      if (calendar.MonthControl.HeaderButton.IsFocused)
      {
        calendar.MonthControl.NextButton.Focusable = true;
        calendar.MonthControl.NextButton.Focus();
        return true;
      }
      if (calendar.MonthControl.NextButton.IsFocused)
        return this.UpdateCalendarItemFocus(calendar, noneButton, true);
    }
    else if (isShiftPressed)
    {
      if (calendarDayButton != null && calendarDayButton.IsFocused || focusedCalendarButton != null && focusedCalendarButton.IsFocused)
      {
        calendar.MonthControl.NextButton.Focusable = true;
        calendar.MonthControl.NextButton.Focus();
        return true;
      }
      if (calendar.MonthControl.NextButton.IsFocused)
      {
        if (calendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Decade)
        {
          calendar.MonthControl.PreviousButton.Focusable = true;
          calendar.MonthControl.PreviousButton.Focus();
        }
        else
        {
          calendar.MonthControl.HeaderButton.Focusable = true;
          calendar.MonthControl.HeaderButton.Focus();
        }
        return true;
      }
      if (calendar.MonthControl.HeaderButton.IsFocused)
      {
        calendar.MonthControl.PreviousButton.Focusable = true;
        calendar.MonthControl.PreviousButton.Focus();
        return true;
      }
      if (calendar.MonthControl.PreviousButton.IsFocused)
      {
        if (this.DropDownView == DropDownViews.Combined && cancelButton != null && cancelButton.IsVisible)
        {
          cancelButton.Focusable = true;
          cancelButton.Focus();
          return true;
        }
        if (this.DropDownView == DropDownViews.Combined && okButton != null && okButton.IsVisible)
        {
          okButton.Focusable = true;
          okButton.Focus();
          return true;
        }
        if (this.IsEmptyDateEnabled && noneButton != null && noneButton.IsVisible)
        {
          noneButton.Focusable = true;
          noneButton.Focus();
          return true;
        }
        if (todayButton == null || !todayButton.IsVisible || !todayButton.IsEnabled)
          return this.UpdateCalendarItemFocus(calendar, noneButton, true);
        todayButton.Focusable = true;
        todayButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Combined && cancelButton != null && cancelButton != null && cancelButton.IsFocused)
      {
        okButton.Focusable = true;
        okButton.Focus();
        return true;
      }
      if (this.DropDownView == DropDownViews.Combined && okButton != null && okButton.IsFocused)
      {
        if (this.DropDownView != DropDownViews.Combined || this.IsEmptyDateEnabled || noneButton.IsVisible)
          return this.UpdateCalendarItemFocus(calendar, noneButton, false);
        todayButton.Focusable = true;
        todayButton.Focus();
        return true;
      }
      if (this.IsEmptyDateEnabled && noneButton != null && todayButton != null && noneButton.IsFocused)
      {
        if (!todayButton.IsVisible || !todayButton.IsEnabled)
          return this.UpdateCalendarItemFocus(calendar, noneButton, true);
        todayButton.Focusable = true;
        todayButton.Focus();
        return true;
      }
      if (todayButton != null && todayButton.IsFocused)
        return this.UpdateCalendarItemFocus(calendar, noneButton, true);
    }
    return false;
  }

  private bool ProcessTabKey(Key key, bool isShiftPressed)
  {
    Syncfusion.Windows.Controls.Calendar calendar = (Syncfusion.Windows.Controls.Calendar) null;
    Button noneButton = (Button) null;
    Button todayButton = (Button) null;
    Button okButton = (Button) null;
    Button cancelButton = (Button) null;
    if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar && this.noneButton != null && this.todayButton != null)
    {
      calendar = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
      noneButton = this.noneButton;
      todayButton = this.todayButton;
      okButton = this.okButton;
      cancelButton = this.cancelButton;
    }
    else if (this._calendar != null)
      calendar = this._calendar;
    if (calendar != null && calendar.MonthControl != null && calendar.MonthControl.PreviousButton != null && calendar.MonthControl.HeaderButton != null && calendar.MonthControl.NextButton != null)
    {
      if (key == Key.Tab)
        return this.UpdateCalendarElementFocus(isShiftPressed, calendar, noneButton, todayButton, okButton, cancelButton);
      if (okButton != null && okButton.IsFocused || cancelButton != null && cancelButton.IsFocused || noneButton != null && noneButton.IsFocused || todayButton != null && todayButton.IsFocused || calendar.MonthControl.PreviousButton.IsFocused || calendar.MonthControl.HeaderButton.IsFocused || calendar.MonthControl.NextButton.IsFocused)
      {
        if (key != Key.Return && key != Key.Space)
          return true;
        if (todayButton != null && todayButton.IsFocused)
        {
          if (!this.IsReadOnly)
            this.DateTime = this.TodayButtonAction != TodayButtonAction.Date ? new System.DateTime?(System.DateTime.Now) : new System.DateTime?(System.DateTime.Today);
          this.IsDropDownOpen = false;
          return true;
        }
        if (noneButton != null && noneButton.IsFocused)
        {
          if (!this.IsReadOnly)
          {
            if (!this.DateTime.HasValue)
              this.LoadTextBox();
            this.DateTime = new System.DateTime?();
          }
          this.IsDropDownOpen = false;
          return true;
        }
        if (okButton != null && okButton.IsFocused)
        {
          if (!this.IsReadOnly)
            this.PerformOkClick();
          return true;
        }
        if (cancelButton != null && cancelButton.IsFocused)
        {
          if (!this.IsReadOnly)
            this.ClosePopup();
          return true;
        }
        if (calendar.MonthControl.PreviousButton.IsFocused)
        {
          calendar.OnPreviousClick();
          calendar.MonthControl.PreviousButton.Focus();
          return true;
        }
        if (calendar.MonthControl.HeaderButton.IsFocused)
        {
          if (calendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month)
          {
            calendar.DisplayMode = Syncfusion.Windows.Controls.CalendarMode.Year;
          }
          else
          {
            calendar.DisplayMode = Syncfusion.Windows.Controls.CalendarMode.Decade;
            calendar.MonthControl.NextButton.Focusable = true;
            calendar.MonthControl.NextButton.Focus();
          }
          return true;
        }
        if (calendar.MonthControl.NextButton.IsFocused)
        {
          calendar.OnNextClick();
          calendar.MonthControl.NextButton.Focus();
          return true;
        }
      }
    }
    return false;
  }

  internal void UpdateDateTimeValue(
    System.DateTime date,
    DateTimeEdit dateTimeEdit,
    Syncfusion.Windows.Shared.DateTimeProperties characterProperty)
  {
    dateTimeEdit.mValueChanged = false;
    date = dateTimeEdit.IsBlackoutDate(Key.None, date);
    dateTimeEdit.DateTime = new System.DateTime?(date);
    dateTimeEdit.LoadTextBox();
    dateTimeEdit.mValueChanged = true;
    dateTimeEdit.Select(characterProperty.StartPosition, characterProperty.Lenghth);
  }

  private void UpdateDateTimeValue()
  {
    if (this.Calendar_Classic != null && this.Calendar_Classic.SelectedDate.HasValue)
      this.DateTime = this.Calendar_Classic.SelectedDate;
    else if (this._calendar != null && this._calendar.SelectedDate.HasValue)
      this.DateTime = this._calendar.SelectedDate;
    if (this.mIsLoaded)
      this.LoadTextBox();
    if (!this.CanEdit || this._calendar == null || this.Calendar_Classic == null || string.IsNullOrEmpty(this.Text) || this.DateTime.HasValue || this._calendar.SelectedDate.HasValue || this.Calendar_Classic.SelectedDate.HasValue)
      return;
    this.Text = string.Empty;
    this.WatermarkVisibility = Visibility.Visible;
  }

  private void FireMonthChanged()
  {
    if (this.MonthChanged == null)
      return;
    this.MonthChanged((object) this, new RoutedEventArgs());
  }

  private void PART_Popup_Closed(object sender, EventArgs e)
  {
    this.DT.Tick -= new EventHandler(this.DT_Tick);
    this.Focus();
    this.PART_DropDown.IsChecked = new bool?(false);
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
  }

  private System.DateTime ValidateDateTimeValue(System.DateTime data)
  {
    if (this.Pattern == DateTimePattern.ShortDate || this.Pattern == DateTimePattern.LongDate || this.Pattern == DateTimePattern.MonthDay || this.Pattern == DateTimePattern.YearMonth)
      data = new System.DateTime(data.Year, data.Month, data.Day, this.DateTime.Value.Hour, this.DateTime.Value.Minute, this.DateTime.Value.Second, this.DateTime.Value.Millisecond);
    else if (this.Pattern == DateTimePattern.ShortTime || this.Pattern == DateTimePattern.LongTime)
      data = new System.DateTime(this.DateTime.Value.Year, this.DateTime.Value.Month, this.DateTime.Value.Day, data.Hour, data.Minute, this.DateTime.Value.Second, this.DateTime.Value.Millisecond);
    data = this.IsBlackoutDate(Key.None, data);
    return data;
  }

  private void PART_DropDown_Checked(object sender, RoutedEventArgs e)
  {
    if (this.Text != string.Empty && this.CanEdit)
    {
      System.DateTime result;
      if ((this.Pattern == DateTimePattern.CustomPattern ? System.DateTime.TryParseExact(this.Text, this.CustomPattern, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) : System.DateTime.TryParseExact(this.Text, this.GetStringPattern(this.CultureInfo.DateTimeFormat, this.Pattern, ""), (IFormatProvider) this.CultureInfo.DateTimeFormat, DateTimeStyles.None, out result)) && this.DateTime.HasValue)
      {
        System.DateTime dateTime = this.DateTime.Value;
        result = this.ValidateDateTimeValue(result);
      }
      if (result < this.MinDateTime)
      {
        if (!System.DateTime.TryParse(this.Text, out result))
          this.DateTime = new System.DateTime?();
        this.DateTime = new System.DateTime?(this.MinDateTime);
      }
      else
        this.DateTime = !(result > this.MaxDateTime) ? new System.DateTime?(result) : new System.DateTime?(this.MaxDateTime);
    }
    this.DT.Interval = this.PopupDelay;
    this.DT.Tick += new EventHandler(this.DT_Tick);
    this.DT.Start();
  }

  private void DT_Tick(object sender, EventArgs e)
  {
    this.IsDropDownOpen = true;
    this.DT.Stop();
    this.UpdateCalendarFocus();
  }

  private void Button_NoDate_Click(object sender, RoutedEventArgs e)
  {
    if (!this.IsReadOnly)
    {
      if (!this.DateTime.HasValue)
        this.LoadTextBox();
      this.DateTime = new System.DateTime?();
    }
    this.IsDropDownOpen = false;
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.ClosePopup();

  private void OkButton_Click(object sender, RoutedEventArgs e) => this.PerformOkClick();

  private void PerformOkClick()
  {
    Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
    if (calendar != null)
    {
      if (this.DateTime.HasValue)
      {
        System.DateTime dateTime = this.DateTime.Value;
        this.DateTime = new System.DateTime?(new System.DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, calendar.SelectedDate.Value.Day, this.DateTime.Value.Hour, this.DateTime.Value.Minute, this.DateTime.Value.Second, this.DateTime.Value.Millisecond));
      }
      else
        this.DateTime = calendar.SelectedDate;
    }
    if (this.PART_Popup != null)
    {
      if (this.PART_Popup.IsOpen && this.clock != null)
      {
        System.DateTime dateTime = this.clock.DateTime;
        this.DateTime = !this.DateTime.HasValue ? new System.DateTime?(this.clock.DateTime) : new System.DateTime?(new System.DateTime(this.DateTime.Value.Year, this.DateTime.Value.Month, this.DateTime.Value.Day, this.clock.DateTime.Hour, this.clock.DateTime.Minute, this.clock.DateTime.Second, this.clock.DateTime.Millisecond));
      }
    }
    else if (this.PART_Popup.IsOpen && this.clock != null)
    {
      System.DateTime dateTime = this.clock.DateTime;
      this.DateTime = !this.DateTime.HasValue ? new System.DateTime?(this.clock.DateTime) : new System.DateTime?(new System.DateTime(this.DateTime.Value.Year, this.DateTime.Value.Month, this.DateTime.Value.Day, this.clock.DateTime.Hour, this.clock.DateTime.Minute, this.clock.DateTime.Second, this.clock.DateTime.Millisecond));
    }
    this.ClosePopup();
  }

  private void _OutsideCalendarPopupCanvas_MouseLeftButtonDown(
    object sender,
    MouseButtonEventArgs e)
  {
    this.IsDropDownOpen = false;
  }

  private void clock_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
    if (calendar == null || calendar.BlackoutDates == null || this.clock == null || !string.IsNullOrEmpty(this.DateTime.ToString()) || string.IsNullOrEmpty(this.clock.DateTime.ToString()))
      return;
    calendar.BlackoutDates.Contains(this.clock.DateTime);
  }

  private void Button_Clock_Unchecked(object sender, RoutedEventArgs e)
  {
    if (this.PART_ClockBorder == null)
      return;
    this.PART_ClockBorder.Visibility = Visibility.Collapsed;
    if (this.PART_OptionGrid == null)
      return;
    this.PART_OptionGrid.HorizontalAlignment = HorizontalAlignment.Center;
  }

  private void PART_DropDown_Unchecked(object sender, RoutedEventArgs e)
  {
    this.IsDropDownOpen = false;
  }

  public FrameworkElement Clock
  {
    get => (FrameworkElement) this.GetValue(DateTimeEdit.ClockProperty);
    set => this.SetValue(DateTimeEdit.ClockProperty, (object) value);
  }

  private static void OnClockPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeEdit).UpdateFooterVisiblity();
  }

  public void OpenPopup()
  {
    if ((this.ReadLocalValue(DateTimeBase.FormatCalendarProperty) == DependencyProperty.UnsetValue || this.ReadLocalValue(DateTimeBase.FormatCalendarProperty) == this.FormatCalendar) && this.CultureInfo != null && this.CultureInfo.Calendar != null && this.CultureInfo.Calendar != this.FormatCalendar)
      this.FormatCalendar = this.CultureInfo.Calendar;
    this.IsDropDownOpen = true;
    Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
    if (calendar != null && calendar.BlackoutDates != null && !string.IsNullOrEmpty(System.DateTime.Today.ToString()) && calendar.BlackoutDates.Contains(System.DateTime.Today) && this.DropDownView != DropDownViews.Combined && this.todayButton != null)
      this.todayButton.IsEnabled = false;
    if (this.PART_PopupGrid_Classic != null)
      this.PART_PopupGrid_Classic.Visibility = Visibility.Visible;
    this.ignoreDateChange = true;
    if (this._calendar != null)
    {
      this._calendar.DisplayDateStart = new System.DateTime?(this.MinDateTime);
      this._calendar.DisplayDateEnd = new System.DateTime?(this.MaxDateTime);
      this._calendar.SelectedDate = this.DateTime;
      this._calendar.DisplayDate = !this.DateTime.HasValue ? System.DateTime.Now : this.DateTime.Value;
      this._calendar.DisplayMode = this.DisableDateSelection ? Syncfusion.Windows.Controls.CalendarMode.Year : Syncfusion.Windows.Controls.CalendarMode.Month;
    }
    else if (this._calendar == null && (this._calendar = this.GetTemplateChild("Calendar") as Syncfusion.Windows.Controls.Calendar) != null && this.DateTime.HasValue)
      this._calendar.SelectedDate = new System.DateTime?(this.DateTime.Value);
    if (this.Calendar_Classic != null)
    {
      this.Calendar_Classic.DisplayDateStart = new System.DateTime?(this.MinDateTime);
      this.Calendar_Classic.DisplayDateEnd = new System.DateTime?(this.MaxDateTime);
      this.Calendar_Classic.SelectedDate = this.DateTime;
      if (this.DateTimeCalender != null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar)
      {
        Syncfusion.Windows.Controls.Calendar dateTimeCalender = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
        this.Calendar_Classic.BlackoutDates = dateTimeCalender.BlackoutDates;
        this.Calendar_Classic.CanBlockWeekEnds = dateTimeCalender.CanBlockWeekEnds;
      }
      this.Calendar_Classic.DisplayDate = !this.DateTime.HasValue ? System.DateTime.Now : this.DateTime.Value;
      this.Calendar_Classic.DisplayMode = this.DisableDateSelection ? Syncfusion.Windows.Controls.CalendarMode.Year : Syncfusion.Windows.Controls.CalendarMode.Month;
    }
    else if (this.Calendar_Classic == null && this.DateTimeCalender is Syncfusion.Windows.Controls.Calendar && (this.Calendar_Classic = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar) != null && this.DateTime.HasValue)
      this.Calendar_Classic.SelectedDate = new System.DateTime?(this.DateTime.Value);
    this.ignoreDateChange = false;
    if (this.PART_Popup != null)
      this.PART_Popup.IsOpen = true;
    if (this._calendar != null && this.DisableDateSelection)
      this._calendar.DisableDateSelection = true;
    else if (this._calendar != null && !this.DisableDateSelection)
      this._calendar.DisableDateSelection = false;
    if (this.Calendar_Classic != null && this.DisableDateSelection)
      this.Calendar_Classic.DisableDateSelection = true;
    else if (this.Calendar_Classic != null && !this.DisableDateSelection)
      this.Calendar_Classic.DisableDateSelection = false;
    if (this.PART_Popup != null)
      this.PART_Popup.Placement = PlacementMode.Bottom;
    if (this.DateTimeProperties.Count > 0)
      this.DateTimeProperties[this.DateTimeProperties.Count - 1].KeyPressCount = 0;
    if (this.PART_Popup == null)
      return;
    if (this.clock != null)
      this.clock.Stop();
    if (this.DropDownView == DropDownViews.Combined)
    {
      if (this.CalendarPopupOpened != null)
        this.CalendarPopupOpened((object) this, new RoutedEventArgs());
      if (this.ClockPopupOpened == null)
        return;
      this.ClockPopupOpened((object) this, new RoutedEventArgs());
    }
    else if (this.DropDownView == DropDownViews.Calendar)
    {
      if (this.CalendarPopupOpened == null)
        return;
      this.CalendarPopupOpened((object) this, new RoutedEventArgs());
    }
    else
    {
      if (this.DropDownView != DropDownViews.Clock || this.ClockPopupOpened == null)
        return;
      this.ClockPopupOpened((object) this, new RoutedEventArgs());
    }
  }

  public void ClosePopup()
  {
    this.IsDropDownOpen = false;
    if (this.PART_Popup == null)
      return;
    this.PART_Popup.IsOpen = false;
  }

  private void _outsidePopupCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsDropDownOpen = false;
  }

  private void calender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
  {
    Syncfusion.Windows.Controls.Calendar calendar = (Syncfusion.Windows.Controls.Calendar) sender;
    if (this.IsReadOnly)
      return;
    System.DateTime? selectedDate1 = calendar.SelectedDate;
    System.DateTime minDateTime = this.MinDateTime;
    if ((selectedDate1.HasValue ? (selectedDate1.GetValueOrDefault() >= minDateTime ? 1 : 0) : 0) == 0)
      return;
    System.DateTime? selectedDate2 = calendar.SelectedDate;
    System.DateTime maxDateTime = this.MaxDateTime;
    if ((selectedDate2.HasValue ? (selectedDate2.GetValueOrDefault() <= maxDateTime ? 1 : 0) : 0) == 0 || this.DropDownView == DropDownViews.Combined)
      return;
    if (this.DateTime.HasValue)
      this.DateTime = new System.DateTime?(new System.DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, calendar.SelectedDate.Value.Day, this.DateTime.Value.Hour, this.DateTime.Value.Minute, this.DateTime.Value.Second, this.DateTime.Value.Millisecond));
    else
      this.DateTime = calendar.SelectedDate;
  }

  protected override void OnContextMenuOpening(ContextMenuEventArgs e)
  {
    e.Handled = false;
    base.OnContextMenuOpening(e);
  }

  protected override void OnDrop(DragEventArgs e)
  {
    e.Handled = true;
    base.OnDrop(e);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.ReadLocalValue(DateTimeEdit.DateTimeCalenderProperty) == DependencyProperty.UnsetValue)
    {
      this.DateTimeCalender = (FrameworkElement) new Syncfusion.Windows.Controls.Calendar();
      this.Calendar_Classic = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
      BindingOperations.SetBinding((DependencyObject) this.Calendar_Classic, Syncfusion.Windows.Controls.Calendar.CultureProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("CultureInfo", new object[0]),
        Mode = BindingMode.TwoWay
      });
      BindingOperations.SetBinding((DependencyObject) this.Calendar_Classic, Syncfusion.Windows.Controls.Calendar.FormatCalendarProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("FormatCalendar", new object[0]),
        Mode = BindingMode.TwoWay
      });
      BindingOperations.SetBinding((DependencyObject) this.Calendar_Classic, Syncfusion.Windows.Controls.Calendar.AbbreviatedMonthNamesProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("AbbreviatedMonthNames", new object[0]),
        Mode = BindingMode.TwoWay
      });
      BindingOperations.SetBinding((DependencyObject) this.Calendar_Classic, Syncfusion.Windows.Controls.Calendar.ShortestDayNamesProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("ShortestDayNames", new object[0]),
        Mode = BindingMode.TwoWay
      });
      BindingOperations.SetBinding((DependencyObject) this.Calendar_Classic, FrameworkElement.StyleProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("CalendarStyle", new object[0]),
        Mode = BindingMode.TwoWay
      });
    }
    if (this.ReadLocalValue(DateTimeEdit.ClockProperty) == DependencyProperty.UnsetValue)
      this.Clock = (FrameworkElement) new Syncfusion.Windows.Shared.Clock();
    this._calendar = this.GetTemplateChild("Calendar") as Syncfusion.Windows.Controls.Calendar;
    this.Calendar_Classic = this.DateTimeCalender as Syncfusion.Windows.Controls.Calendar;
    if (this.PART_UpButton != null)
      this.PART_UpButton.Click -= new RoutedEventHandler(this.PART_UpButton_Click);
    if (this.PART_DownButton != null)
      this.PART_DownButton.Click -= new RoutedEventHandler(this.PART_DownButton_Click);
    this.PART_UpButton = this.GetTemplateChild("PART_UpArrow") as RepeatButton;
    this.PART_DownButton = this.GetTemplateChild("PART_DownArrow") as RepeatButton;
    if (this.PART_UpButton != null)
    {
      AutomationProperties.SetName((DependencyObject) this.PART_UpButton, "Up");
      this.PART_UpButton.Click += new RoutedEventHandler(this.PART_UpButton_Click);
      this.PART_UpButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.PART_UpButton_PreviewMouseLeftButtonUp);
    }
    if (this.PART_DownButton != null)
    {
      AutomationProperties.SetName((DependencyObject) this.PART_DownButton, "Down");
      this.PART_DownButton.Click += new RoutedEventHandler(this.PART_DownButton_Click);
      this.PART_DownButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.PART_DownButton_PreviewMouseLeftButtonUp);
    }
    if (this.CanEdit && this.PART_UpButton != null && this.PART_DownButton != null)
    {
      this.PART_UpButton.Visibility = Visibility.Collapsed;
      this.PART_DownButton.Visibility = Visibility.Collapsed;
    }
    this.Popup_OnApplyTemplate();
  }

  private void PART_DownButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.Focus())
      return;
    if (this.mSelectedCollection >= 0 && this.mSelectedCollection <= this.DateTimeProperties.Count)
    {
      if (this.IsEditable)
      {
        this.SelectionStart = this.DateTimeProperties[this.mSelectedCollection].StartPosition;
        this.SelectionLength = this.DateTimeProperties[this.mSelectedCollection].Lenghth;
      }
      this.selectionChanged = true;
    }
    this.selectionChanged = true;
  }

  private void PART_UpButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.Focus() || this.mSelectedCollection < 0 || this.mSelectedCollection > this.DateTimeProperties.Count || !this.IsEditable)
      return;
    this.SelectionStart = this.DateTimeProperties[this.mSelectedCollection].StartPosition;
    this.SelectionLength = this.DateTimeProperties[this.mSelectedCollection].Lenghth;
  }

  private void Calendar_Classic_MouseButtonDown(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
  }

  private void PART_DownButton_Click(object sender, RoutedEventArgs e)
  {
    this.mTextInputpartended = false;
    System.DateTime? dateTime1 = this.DateTime;
    System.DateTime minDateTime1 = this.MinDateTime;
    if ((dateTime1.HasValue ? (dateTime1.GetValueOrDefault() > minDateTime1 ? 1 : 0) : 0) == 0)
    {
      if (this.CanEdit)
        return;
      System.DateTime? dateTime2 = this.DateTime;
      System.DateTime minDateTime2 = this.MinDateTime;
      if ((dateTime2.HasValue ? (dateTime2.GetValueOrDefault() >= minDateTime2 ? 1 : 0) : 0) == 0)
        return;
    }
    KeyHandler.keyHandler.HandleDownKey(this);
  }

  private void PART_UpButton_Click(object sender, RoutedEventArgs e)
  {
    this.mTextInputpartended = false;
    System.DateTime? dateTime1 = this.DateTime;
    System.DateTime maxDateTime1 = this.MaxDateTime;
    if ((dateTime1.HasValue ? (dateTime1.GetValueOrDefault() < maxDateTime1 ? 1 : 0) : 0) == 0)
    {
      if (this.CanEdit)
        return;
      System.DateTime? dateTime2 = this.DateTime;
      System.DateTime maxDateTime2 = this.MaxDateTime;
      if ((dateTime2.HasValue ? (dateTime2.GetValueOrDefault() <= maxDateTime2 ? 1 : 0) : 0) == 0)
        return;
    }
    KeyHandler.keyHandler.HandleUpKey(this);
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    if (!this.CanEdit && (this.DateTime.HasValue || this.WatermarkVisibility == Visibility.Collapsed && this.ShowMaskOnNullValue))
      e.Handled = DateTimeHandler.dateTimeHandler.MatchWithMask(this, e.Text);
    base.OnTextInput(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (this.CanNavigateCalendarElement())
    {
      if (ModifierKeys.Shift == Keyboard.Modifiers && e.Key == Key.Tab)
        e.Handled = this.ProcessTabKey(e.Key, true);
      else
        e.Handled = this.ProcessTabKey(e.Key, false);
      if (e.Handled)
        return;
      if (this.CanNavigateCalendarElement() && (e.Key == Key.Return || e.Key == Key.Space || e.Key == Key.Home || ModifierKeys.Shift == Keyboard.Modifiers || ModifierKeys.Alt == Keyboard.Modifiers && e.SystemKey != Key.Down || ModifierKeys.Control == Keyboard.Modifiers || e.Key == Key.End || e.Key == Key.Prior || e.Key == Key.Next || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right))
      {
        this.UpdateCalendarFocus();
        return;
      }
    }
    if (!this.CanEdit && !this.IsNull && (e.Key == Key.Space || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Return || e.Key == Key.Tab || e.Key == Key.Escape || e.Key == Key.F4 || (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && e.SystemKey == Key.Down))
    {
      System.DateTime result = this.GetValidDateTime();
      if (result < this.MinDateTime)
      {
        if (!System.DateTime.TryParse(this.Text, out result))
          this.DateTime = new System.DateTime?();
        this.DateTime = new System.DateTime?(this.MinDateTime);
      }
      else
        this.DateTime = !(result > this.MaxDateTime) ? new System.DateTime?(result) : new System.DateTime?(this.MaxDateTime);
      e.Handled = this.ValidateDateTimeField(e.Key);
      if (e.Handled)
        return;
    }
    if (ModifierKeys.Control == Keyboard.Modifiers)
    {
      if (e.Key == Key.V)
      {
        System.DateTime dateTime = new System.DateTime();
        bool flag = false;
        try
        {
          dateTime = Convert.ToDateTime(Clipboard.GetText());
        }
        catch (Exception ex)
        {
          flag = true;
        }
        if (!flag && !this.CanEdit)
          this.DateTime = new System.DateTime?(dateTime);
        if (this.CanEdit)
          e.Handled = false;
        else
          e.Handled = true;
      }
      if (e.Key == Key.X)
      {
        if (this.CanEdit)
          e.Handled = false;
        else
          e.Handled = true;
      }
      if (e.Key == Key.Z)
      {
        this.DateTime = this.OldValue;
        if (this.CanEdit)
          e.Handled = false;
        else
          e.Handled = true;
      }
      if (!this.CanEdit && !this.IsNull && (e.Key == Key.Left || e.Key == Key.Right))
      {
        System.DateTime result = this.GetValidDateTime();
        if (result < this.MinDateTime)
        {
          if (!System.DateTime.TryParse(this.Text, out result))
            this.DateTime = new System.DateTime?();
          this.DateTime = new System.DateTime?(this.MinDateTime);
        }
        else
          this.DateTime = !(result > this.MaxDateTime) ? new System.DateTime?(result) : new System.DateTime?(this.MaxDateTime);
        e.Handled = KeyHandler.keyHandler.HandleKeyDown(this, e);
      }
    }
    else if (this.CanEdit)
      KeyHandler.keyHandler.HandleKeyDown(this, e);
    else
      e.Handled = KeyHandler.keyHandler.HandleKeyDown(this, e);
    if (e.Key == Key.Return)
    {
      if (this.Text != string.Empty && this.CanEdit)
      {
        if ((this.Pattern == DateTimePattern.LongDate || this.Pattern == DateTimePattern.FullDateTime) && this.Text.Contains(","))
        {
          string[] source = this.Text.Split(',');
          if (((IEnumerable<string>) source).Count<string>() == 3)
            this.Text = $"{source[1]},{source[2]}";
          else if (this.DateTime.HasValue)
            this.Text = $"{source[1]},{(object) this.DateTime.Value.Year}";
          else
            this.Text = source[1] + ",";
        }
        System.DateTime result = this.GetValidDateTime();
        if (this.IsValidDate() && this.DateTime.HasValue)
        {
          System.DateTime dateTime = this.DateTime.Value;
          result = this.ValidateDateTimeValue(result);
        }
        if (!this.IsValidDate())
          this.UpdateDateTimeValue();
        else if (result < this.MinDateTime)
        {
          if (!System.DateTime.TryParse(this.Text, out result))
            this.DateTime = new System.DateTime?();
          this.DateTime = new System.DateTime?(this.MinDateTime);
        }
        else if (result > this.MaxDateTime)
        {
          this.DateTime = new System.DateTime?(this.MaxDateTime);
        }
        else
        {
          if (!this.IsValidDate())
          {
            if (this.Pattern == DateTimePattern.CustomPattern || this.Pattern == DateTimePattern.FullDateTime)
              this.Text = this.MinDateTime.ToString();
            else if (this.Pattern == DateTimePattern.LongDate)
              this.Text = this.MinDateTime.ToLongDateString();
            else if (this.Pattern == DateTimePattern.LongTime)
              this.Text = this.MinDateTime.ToLongTimeString();
            else if (this.Pattern == DateTimePattern.ShortDate)
              this.Text = this.MinDateTime.ToShortDateString();
            else if (this.Pattern == DateTimePattern.ShortTime)
              this.Text = this.MinDateTime.ToShortTimeString();
          }
          this.DateTime = new System.DateTime?(result);
        }
        System.DateTime? dateTime1 = this.DateTime;
        System.DateTime dateTime2 = result;
        if ((!dateTime1.HasValue ? 0 : (dateTime1.GetValueOrDefault() == dateTime2 ? 1 : 0)) != 0 && this.mIsLoaded)
          this.LoadTextBox();
        if (this._calendar != null && this.Calendar_Classic != null && string.IsNullOrEmpty(this.Text) && !this.DateTime.HasValue && !this._calendar.SelectedDate.HasValue && !this.Calendar_Classic.SelectedDate.HasValue && this.WatermarkVisibility == Visibility.Visible)
        {
          this.WatermarkVisibility = Visibility.Collapsed;
          this.Focus();
        }
      }
    }
    else if (e.Key == Key.Tab && this.Text != string.Empty && this.CanEdit)
    {
      System.DateTime result = this.GetValidDateTime();
      if (this.IsValidDate() && this.DateTime.HasValue)
      {
        System.DateTime dateTime = this.DateTime.Value;
        result = this.ValidateDateTimeValue(result);
      }
      if (!this.IsValidDate())
        this.UpdateDateTimeValue();
      else if (result < this.MinDateTime)
      {
        if (!System.DateTime.TryParse(this.Text, out result))
          this.DateTime = new System.DateTime?();
        this.DateTime = new System.DateTime?(this.MinDateTime);
      }
      else if (result > this.MaxDateTime)
      {
        this.DateTime = new System.DateTime?(this.MaxDateTime);
      }
      else
      {
        if (!this.IsValidDate())
        {
          if (this.Pattern == DateTimePattern.CustomPattern || this.Pattern == DateTimePattern.FullDateTime)
            this.Text = this.MinDateTime.ToString();
          else if (this.Pattern == DateTimePattern.LongDate)
            this.Text = this.MinDateTime.ToLongDateString();
          else if (this.Pattern == DateTimePattern.LongTime)
            this.Text = this.MinDateTime.ToLongTimeString();
          else if (this.Pattern == DateTimePattern.ShortDate)
            this.Text = this.MinDateTime.ToShortDateString();
          else if (this.Pattern == DateTimePattern.ShortTime)
            this.Text = this.MinDateTime.ToShortTimeString();
        }
        this.DateTime = new System.DateTime?(result);
        if (this.CustomPattern == "MM/dd/yyyy h:m tt" || this.CustomPattern == "MM/dd/yyyy hh:m tt" || this.CustomPattern == "MM/dd/yyyy h:mm tt")
          this.CustomPattern = "MM/dd/yyyy hh:mm tt";
      }
    }
    if (!this.CanEdit)
    {
      for (int index1 = 0; index1 < this.DateTimeProperties.Count; ++index1)
      {
        Syncfusion.Windows.Shared.DateTimeProperties dateTimeProperty1 = this.DateTimeProperties[index1];
        int index2 = index1 + 1;
        if (e.Key != Key.Space && dateTimeProperty1.StartPosition == this.SelectionStart && index2 < this.DateTimeProperties.Count - 1)
        {
          string str = ModifierKeys.Shift == Keyboard.Modifiers ? KeyCode.KeycodeToChar(e.Key, true) : KeyCode.KeycodeToChar(e.Key, false);
          Syncfusion.Windows.Shared.DateTimeProperties dateTimeProperty2 = this.DateTimeProperties[index2];
          if (dateTimeProperty2.Type == DateTimeType.others && str == dateTimeProperty2.Content.ToString())
          {
            while ((dateTimeProperty2.Type == DateTimeType.Dayname || dateTimeProperty2.Pattern == null) && index2 < this.DateTimeProperties.Count - 1)
              dateTimeProperty2 = this.DateTimeProperties[++index2];
            if (dateTimeProperty2.Pattern != null)
            {
              this.ValidateDateTimeField(e.Key);
              this.selectionChanged = false;
              this.mSelectedCollection = index2;
              this.Select(dateTimeProperty2.StartPosition, dateTimeProperty2.Lenghth);
              this.selectionChanged = true;
              break;
            }
            break;
          }
        }
      }
    }
    if (e.Key == Key.F4 || (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && e.SystemKey == Key.Down)
    {
      if (!this.IsDropDownOpen && this.CanEdit)
      {
        if (this.Text != string.Empty)
        {
          System.DateTime result = this.GetValidDateTime();
          if (this.IsValidDate() && this.DateTime.HasValue)
          {
            System.DateTime dateTime = this.DateTime.Value;
            result = this.ValidateDateTimeValue(result);
          }
          if (!this.IsValidDate())
            this.UpdateDateTimeValue();
          else if (result < this.MinDateTime)
          {
            if (!System.DateTime.TryParse(this.Text, out result))
              this.DateTime = new System.DateTime?();
            this.DateTime = new System.DateTime?(this.MinDateTime);
          }
          else
            this.DateTime = !(result > this.MaxDateTime) ? new System.DateTime?(result) : new System.DateTime?(this.MaxDateTime);
        }
        else if (string.IsNullOrEmpty(this.Text))
          this.DateTime = new System.DateTime?();
      }
      this.IsDropDownOpen = !this.IsDropDownOpen;
      if (this.IsDropDownOpen)
        this.UpdateCalendarFocus();
    }
    base.OnPreviewKeyDown(e);
  }

  protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
  {
    if (this.PART_Popup != null && !this.PART_Popup.IsOpen)
    {
      if (!this.IsFocused && this.PART_Popup != null && this.PART_Popup.IsOpen || !this.EnableMouseWheelEdit)
        return;
      if (e.Delta > 0)
      {
        System.DateTime? dateTime1 = this.DateTime;
        System.DateTime maxDateTime1 = this.MaxDateTime;
        if ((dateTime1.HasValue ? (dateTime1.GetValueOrDefault() < maxDateTime1 ? 1 : 0) : 0) == 0)
        {
          if (!this.CanEdit)
          {
            System.DateTime? dateTime2 = this.DateTime;
            System.DateTime maxDateTime2 = this.MaxDateTime;
            if ((dateTime2.HasValue ? (dateTime2.GetValueOrDefault() <= maxDateTime2 ? 1 : 0) : 0) == 0)
              goto label_7;
          }
          else
            goto label_7;
        }
        KeyHandler.keyHandler.HandleUpKey(this);
      }
label_7:
      if (e.Delta < 0)
      {
        System.DateTime? dateTime3 = this.DateTime;
        System.DateTime minDateTime1 = this.MinDateTime;
        if ((dateTime3.HasValue ? (dateTime3.GetValueOrDefault() > minDateTime1 ? 1 : 0) : 0) == 0)
        {
          if (!this.CanEdit)
          {
            System.DateTime? dateTime4 = this.DateTime;
            System.DateTime minDateTime2 = this.MinDateTime;
            if ((dateTime4.HasValue ? (dateTime4.GetValueOrDefault() >= minDateTime2 ? 1 : 0) : 0) == 0)
              goto label_12;
          }
          else
            goto label_12;
        }
        KeyHandler.keyHandler.HandleDownKey(this);
      }
    }
label_12:
    base.OnMouseWheel(e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    e.Handled = true;
    base.OnMouseWheel(e);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.mTextInputpartended = true;
    if (e.OriginalSource.GetType().FullName == "System.Windows.Controls.TextBoxView")
      this.mtextboxclicked = true;
    this.ValidateDateTimeField(Key.None);
    if (this.SelectionLength == this.Text.Length)
      DateTimeHandler.dateTimeHandler.HandleSelection(this);
    if (this.CanEdit && this.IsNull)
    {
      this.IsEditable = true;
      this.Text = string.Empty;
      this.IsNull = false;
      this.LoadTextBox();
    }
    if (!this.CanEdit)
      this.checktext = "";
    base.OnPreviewMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    if (e.OriginalSource.GetType().FullName == "System.Windows.Controls.TextBoxView")
      this.mtextboxclicked = true;
    if (!this.IsPopupEnabled)
      return;
    this.IsDropDownOpen = true;
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (this.CanEdit && string.IsNullOrEmpty(this.Text.ToString()) && this.WatermarkVisibility == Visibility.Visible)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (this.OnFocusBehavior == OnFocusBehavior.CursorOnFirstCharacter)
    {
      if (this.IsEditable)
        this.SelectionStart = 0;
      if (Keyboard.IsKeyDown(Key.Tab))
      {
        if (this.CanEdit)
        {
          this.IsEditable = true;
          this.Text = string.Empty;
          this.IsNull = false;
          this.LoadTextBox();
        }
        this.selectionChanged = false;
        this.SelectAll();
        this.selectionChanged = true;
      }
    }
    else if (this.OnFocusBehavior == OnFocusBehavior.CursorAtEnd && this.IsEditable)
      this.SelectionStart = this.Text.Length;
    if (!this.CanEdit || !this.IsNull)
      return;
    this.IsEditable = true;
    this.Text = string.Empty;
    this.IsNull = false;
    this.LoadTextBox();
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    if (this.Text != string.Empty && this.CanEdit && !this.IsNull)
    {
      System.DateTime result = this.GetValidDateTime();
      if (this.IsValidDate() && this.DateTime.HasValue)
      {
        System.DateTime dateTime = this.DateTime.Value;
        result = this.ValidateDateTimeValue(result);
      }
      if (!this.IsValidDate())
        this.UpdateDateTimeValue();
      else if (result < this.MinDateTime)
      {
        if (!System.DateTime.TryParse(this.Text, out result))
          this.DateTime = new System.DateTime?();
        this.DateTime = new System.DateTime?(this.MinDateTime);
      }
      else
        this.DateTime = !(result > this.MaxDateTime) ? new System.DateTime?(result) : new System.DateTime?(this.MaxDateTime);
    }
    else if (string.IsNullOrEmpty(this.Text) && this.CanEdit)
      this.DateTime = new System.DateTime?();
    bool ctrl;
    bool shift;
    KeyboardHelper.GetMetaKeyState(out ctrl, out shift);
    bool flag = this.CanNavigateCalendarElement() || Keyboard.FocusedElement != null;
    if (!ctrl && !shift && !flag)
      this.IsDropDownOpen = false;
    if (string.IsNullOrEmpty(this.Text))
      this.IsNull = true;
    if (!this.CanEdit && !this.IsNull)
    {
      System.DateTime? dateTime1 = this.DateTime;
      System.DateTime minDateTime = this.MinDateTime;
      if ((dateTime1.HasValue ? (dateTime1.GetValueOrDefault() < minDateTime ? 1 : 0) : 0) != 0)
      {
        this.DateTime = new System.DateTime?(this.MinDateTime);
      }
      else
      {
        System.DateTime? dateTime2 = this.DateTime;
        System.DateTime maxDateTime = this.MaxDateTime;
        if ((dateTime2.HasValue ? (dateTime2.GetValueOrDefault() > maxDateTime ? 1 : 0) : 0) != 0)
          this.DateTime = new System.DateTime?(this.MaxDateTime);
      }
    }
    this.ValidateDateTimeField(Key.None);
    this.LoadTextBox();
    base.OnLostFocus(e);
  }

  protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    if (e.NewFocus != null && e.NewFocus.ToString() != "System.Windows.Controls.Primitives.RepeatButton" && e.OldFocus.ToString() != "System.Windows.Controls.Primitives.RepeatButton")
      this.mSelectedCollection = 0;
    base.OnLostKeyboardFocus(e);
  }

  protected override void BasePropertiesChanged()
  {
    base.BasePropertiesChanged();
    if (this.CultureInfo != null)
    {
      if (this.MaxDateTime > this.CultureInfo.Calendar.MaxSupportedDateTime)
        this.SetValue(DateTimeEdit.MaxDateTimeProperty, (object) this.CultureInfo.Calendar.MaxSupportedDateTime);
      if (this.MinDateTime < this.CultureInfo.Calendar.MinSupportedDateTime)
        this.SetValue(DateTimeEdit.MinDateTimeProperty, (object) this.CultureInfo.Calendar.MinSupportedDateTime);
    }
    this.DateTimeProperties = DateTimeHandler.dateTimeHandler.CreateDateTimePatteren(this);
    if (!this.IsEmptyDateEnabled)
    {
      System.DateTime? nullable1 = (System.DateTime?) DateTimeEdit.CoerceDateTime((DependencyObject) this, (object) this.DateTime);
      System.DateTime? nullable2 = nullable1;
      System.DateTime? dateTime = this.DateTime;
      if ((nullable2.HasValue != dateTime.HasValue ? 1 : (!nullable2.HasValue ? 0 : (nullable2.GetValueOrDefault() != dateTime.GetValueOrDefault() ? 1 : 0))) != 0)
        this.DateTime = nullable1;
    }
    if (!this.mIsLoaded)
      return;
    this.ReloadTextBox();
  }

  public System.DateTime? NullValue
  {
    get => (System.DateTime?) this.GetValue(DateTimeEdit.NullValueProperty);
    set => this.SetValue(DateTimeEdit.NullValueProperty, (object) value);
  }

  public System.DateTime? DateTime
  {
    get => (System.DateTime?) this.GetValue(DateTimeEdit.DateTimeProperty);
    set
    {
      if (value.HasValue)
        value = new System.DateTime?((System.DateTime) DateTimeEdit.CoerceDateTime((DependencyObject) this, (object) value));
      Syncfusion.Windows.Controls.Calendar calendar = this.GetCalendar();
      if (calendar != null && calendar.BlackoutDates != null && !string.IsNullOrEmpty(this.DateTime.ToString()) && !string.IsNullOrEmpty(value.ToString()) && calendar.BlackoutDates.Contains(value.Value))
        throw new ArgumentOutOfRangeException("d", "DateTime value is not valid");
      this.SetValue(DateTimeEdit.DateTimeProperty, (object) value);
    }
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new DateTimeEditAutomationPeer(this);
  }

  public DateParts DefaultDatePart
  {
    get => (DateParts) this.GetValue(DateTimeEdit.DefaultDatePartProperty);
    set => this.SetValue(DateTimeEdit.DefaultDatePartProperty, (object) value);
  }

  public TodayButtonAction TodayButtonAction
  {
    get => (TodayButtonAction) this.GetValue(DateTimeEdit.TodayButtonActionProperty);
    set => this.SetValue(DateTimeEdit.TodayButtonActionProperty, (object) value);
  }

  public bool ShowMaskOnNullValue
  {
    get => (bool) this.GetValue(DateTimeEdit.ShowMaskOnNullValueProperty);
    set => this.SetValue(DateTimeEdit.ShowMaskOnNullValueProperty, (object) value);
  }

  public System.DateTime MinDateTime
  {
    get => (System.DateTime) this.GetValue(DateTimeEdit.MinDateTimeProperty);
    set => this.SetValue(DateTimeEdit.MinDateTimeProperty, (object) value);
  }

  public System.DateTime MaxDateTime
  {
    get => (System.DateTime) this.GetValue(DateTimeEdit.MaxDateTimeProperty);
    set => this.SetValue(DateTimeEdit.MaxDateTimeProperty, (object) value);
  }

  public DropDownViews DropDownView
  {
    get => (DropDownViews) this.GetValue(DateTimeEdit.DropDownViewProperty);
    set => this.SetValue(DateTimeEdit.DropDownViewProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool IsNull
  {
    get => (bool) this.GetValue(DateTimeEdit.IsNullProperty);
    set => this.SetValue(DateTimeEdit.IsNullProperty, (object) value);
  }

  public Style CalendarStyle
  {
    get => (Style) this.GetValue(DateTimeEdit.CalendarStyleProperty);
    set => this.SetValue(DateTimeEdit.CalendarStyleProperty, (object) value);
  }

  private static void OnCalendarStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  public bool DisableDateSelection
  {
    get => (bool) this.GetValue(DateTimeEdit.DisableDateSelectionProperty);
    set => this.SetValue(DateTimeEdit.DisableDateSelectionProperty, (object) value);
  }

  public bool AutoForwarding
  {
    get => (bool) this.GetValue(DateTimeEdit.AutoForwardingProperty);
    set => this.SetValue(DateTimeEdit.AutoForwardingProperty, (object) value);
  }

  public static void OnDateTimeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeEdit) obj)?.OnDateTimeChanged(args);
  }

  protected void OnDateTimeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DateTimeChanged != null)
    {
      if (args.NewValue != null)
        this.Text = args.NewValue.ToString();
      else
        this.Text = this.NoneDateText;
      if (this.clock != null && this.DateTime.HasValue)
        this.clock.DateTime = this.DateTime.Value;
      this.DateTimeChanged((DependencyObject) this, args);
    }
    if (this.DateTime.HasValue)
      this.WatermarkVisibility = Visibility.Collapsed;
    if (this.todayButton != null)
    {
      if (System.DateTime.Today < this.MinDateTime || System.DateTime.Today > this.MaxDateTime)
        this.todayButton.IsEnabled = false;
      else
        this.todayButton.IsEnabled = true;
    }
    this.mValue = this.DateTime;
    this.OldValue = (System.DateTime?) args.OldValue;
    this.mValue = this.DateTime;
    if (!this.mIsLoaded)
      return;
    this.LoadTextBox();
  }

  public static void OnMinDateTimeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeEdit) obj)?.OnMinDateTimeChanged(args);
  }

  protected void OnMinDateTimeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinDateTimeChanged != null)
      this.MinDateTimeChanged((DependencyObject) this, args);
    if (this.MinDateTime < this.CultureInfo.Calendar.MinSupportedDateTime)
      this.SetValue(DateTimeEdit.MinDateTimeProperty, (object) this.CultureInfo.Calendar.MinSupportedDateTime);
    if (this.MaxDateTime < this.MinDateTime)
      this.MaxDateTime = this.MinDateTime;
    System.DateTime? dateTime = this.DateTime;
    System.DateTime? nullable = this.ValidateValue(this.DateTime);
    if ((dateTime.HasValue != nullable.HasValue ? 1 : (!dateTime.HasValue ? 0 : (dateTime.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0))) != 0)
      this.DateTime = this.ValidateValue(this.DateTime);
    if (this.todayButton == null)
      return;
    if (System.DateTime.Today < this.MinDateTime)
      this.todayButton.IsEnabled = false;
    else
      this.todayButton.IsEnabled = true;
  }

  public static void OnDropDownViewChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    DateTimeEdit dateTimeEdit = (DateTimeEdit) obj;
    if (dateTimeEdit == null)
      return;
    dateTimeEdit.OnDropDownViewChanged(args);
    dateTimeEdit.UpdateFooterVisiblity();
  }

  protected void OnDropDownViewChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DropDownViewChanged == null)
      return;
    this.DropDownViewChanged((DependencyObject) this, args);
  }

  public static void OnDisableDateSelectionChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeEdit) obj)?.OnDisableDateSelectionChanged(args);
  }

  protected void OnDisableDateSelectionChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.DisableDateSelectionChanged != null)
      this.DisableDateSelectionChanged((DependencyObject) this, args);
    if (this._calendar == null)
      return;
    this._calendar.DisableDateSelection = this.DisableDateSelection;
  }

  public static void OnMaxDateTimeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeEdit) obj)?.OnMaxDateTimeChanged(args);
  }

  protected void OnMaxDateTimeChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxDateTimeChanged != null)
      this.MaxDateTimeChanged((DependencyObject) this, args);
    if (this.MaxDateTime > this.CultureInfo.Calendar.MaxSupportedDateTime)
      this.SetValue(DateTimeEdit.MaxDateTimeProperty, (object) this.CultureInfo.Calendar.MaxSupportedDateTime);
    if (this.MinDateTime > this.MaxDateTime)
      this.MinDateTime = this.MaxDateTime;
    System.DateTime? dateTime = this.DateTime;
    System.DateTime? nullable = this.ValidateValue(this.DateTime);
    if ((dateTime.HasValue != nullable.HasValue ? 1 : (!dateTime.HasValue ? 0 : (dateTime.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0))) != 0)
      this.DateTime = this.ValidateValue(this.DateTime);
    if (this.todayButton == null)
      return;
    if (System.DateTime.Today > this.MaxDateTime)
      this.todayButton.IsEnabled = false;
    else
      this.todayButton.IsEnabled = true;
  }

  private static object CoerceDateTime(DependencyObject d, object baseValue)
  {
    DateTimeEdit dateTimeEdit = (DateTimeEdit) d;
    if (baseValue != null)
    {
      dateTimeEdit.IsNull = false;
      System.DateTime? nullable1 = (System.DateTime?) baseValue;
      if (dateTimeEdit.CultureInfo != null)
      {
        System.DateTime? nullable2 = nullable1;
        System.DateTime supportedDateTime1 = dateTimeEdit.CultureInfo.Calendar.MaxSupportedDateTime;
        if ((nullable2.HasValue ? (nullable2.GetValueOrDefault() > supportedDateTime1 ? 1 : 0) : 0) != 0)
        {
          nullable1 = new System.DateTime?(dateTimeEdit.CultureInfo.Calendar.MaxSupportedDateTime);
        }
        else
        {
          System.DateTime? nullable3 = nullable1;
          System.DateTime supportedDateTime2 = dateTimeEdit.CultureInfo.Calendar.MinSupportedDateTime;
          if ((nullable3.HasValue ? (nullable3.GetValueOrDefault() < supportedDateTime2 ? 1 : 0) : 0) != 0)
            nullable1 = new System.DateTime?(dateTimeEdit.CultureInfo.Calendar.MinSupportedDateTime);
        }
      }
      if (dateTimeEdit != null && dateTimeEdit.Calendar_Classic != null)
      {
        System.DateTime? newDate = dateTimeEdit.Calendar_Classic.NewDate;
        System.DateTime? nullable4 = nullable1;
        if ((newDate.HasValue != nullable4.HasValue ? 1 : (!newDate.HasValue ? 0 : (newDate.GetValueOrDefault() != nullable4.GetValueOrDefault() ? 1 : 0))) != 0)
          dateTimeEdit.Calendar_Classic.NewDate = nullable1;
      }
      return (object) nullable1;
    }
    if (dateTimeEdit.IsEmptyDateEnabled)
    {
      dateTimeEdit.IsNull = true;
      if (dateTimeEdit != null && dateTimeEdit.Calendar_Classic != null)
      {
        System.DateTime? newDate = dateTimeEdit.Calendar_Classic.NewDate;
        System.DateTime? nullValue = dateTimeEdit.NullValue;
        if ((newDate.HasValue != nullValue.HasValue ? 1 : (!newDate.HasValue ? 0 : (newDate.GetValueOrDefault() != nullValue.GetValueOrDefault() ? 1 : 0))) != 0)
          dateTimeEdit.Calendar_Classic.NewDate = dateTimeEdit.NullValue;
      }
      return (object) dateTimeEdit.NullValue;
    }
    dateTimeEdit.IsNull = false;
    if (dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText) && dateTimeEdit.NullValue.HasValue)
    {
      dateTimeEdit.IsNull = true;
      if (dateTimeEdit != null && dateTimeEdit.Calendar_Classic != null)
      {
        System.DateTime? newDate = dateTimeEdit.Calendar_Classic.NewDate;
        System.DateTime? nullValue = dateTimeEdit.NullValue;
        if ((newDate.HasValue != nullValue.HasValue ? 1 : (!newDate.HasValue ? 0 : (newDate.GetValueOrDefault() != nullValue.GetValueOrDefault() ? 1 : 0))) != 0)
          dateTimeEdit.Calendar_Classic.NewDate = dateTimeEdit.NullValue;
      }
      return (object) dateTimeEdit.NullValue;
    }
    System.DateTime? nullable5;
    if (dateTimeEdit.TemplatedParent == null)
    {
      Syncfusion.Windows.Controls.Calendar calendar = dateTimeEdit.GetCalendar();
      if (calendar != null && calendar.BlackoutDates != null && string.IsNullOrEmpty(dateTimeEdit.DateTime.ToString()) && !string.IsNullOrEmpty(System.DateTime.Today.ToString()) && calendar.BlackoutDates.Contains(System.DateTime.Today))
        return (object) new System.DateTime?();
      nullable5 = new System.DateTime?(System.DateTime.Parse(System.DateTime.Today.ToString((IFormatProvider) (d as DateTimeEdit).CultureInfo), (IFormatProvider) (d as DateTimeEdit).CultureInfo));
      System.DateTime? nullable6 = nullable5;
      System.DateTime maxDateTime = dateTimeEdit.MaxDateTime;
      if ((nullable6.HasValue ? (nullable6.GetValueOrDefault() > maxDateTime ? 1 : 0) : 0) != 0)
        nullable5 = new System.DateTime?(dateTimeEdit.MaxDateTime);
      System.DateTime? nullable7 = nullable5;
      System.DateTime minDateTime = dateTimeEdit.MinDateTime;
      if ((nullable7.HasValue ? (nullable7.GetValueOrDefault() < minDateTime ? 1 : 0) : 0) != 0)
        nullable5 = new System.DateTime?(dateTimeEdit.MinDateTime);
    }
    else
      nullable5 = new System.DateTime?();
    if (dateTimeEdit != null && dateTimeEdit.Calendar_Classic != null)
    {
      System.DateTime? newDate = dateTimeEdit.Calendar_Classic.NewDate;
      System.DateTime? nullable8 = nullable5;
      if ((newDate.HasValue != nullable8.HasValue ? 1 : (!newDate.HasValue ? 0 : (newDate.GetValueOrDefault() != nullable8.GetValueOrDefault() ? 1 : 0))) != 0)
        dateTimeEdit.Calendar_Classic.NewDate = nullable5;
    }
    return (object) nullable5;
  }

  private static object CoerceMinValue(DependencyObject d, object baseValue)
  {
    DateTimeEdit dateTimeEdit = (DateTimeEdit) d;
    return (System.DateTime) baseValue > dateTimeEdit.MaxDateTime ? (object) dateTimeEdit.MaxDateTime : baseValue;
  }

  private static object CoerceMaxValue(DependencyObject d, object baseValue)
  {
    DateTimeEdit dateTimeEdit = (DateTimeEdit) d;
    System.DateTime dateTime = (System.DateTime) baseValue;
    return dateTimeEdit.MinDateTime > dateTime ? (object) dateTimeEdit.MinDateTime : baseValue;
  }
}
