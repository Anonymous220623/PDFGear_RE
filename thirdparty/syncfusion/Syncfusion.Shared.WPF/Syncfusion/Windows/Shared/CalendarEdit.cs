// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CalendarEdit
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (CalendarEdit), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Calendar/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/ShinyBlueStyle.xaml")]
[DesignTimeVisible(true)]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (CalendarEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Calendar/Themes/SyncOrangeStyle.xaml")]
public class CalendarEdit : Control, IDisposable
{
  private const string CmainGrid = "MainGrid";
  private const string CweekNumbers = "PART_WeekNumbers";
  private const string CWeekNumbersGridCurrent = "WeekNumbersForYearCurrent";
  private const string CWeekNumbersGridFollow = "WeekNumbersForYearFollow";
  private const string CyearUpDown = "PART_YearUpDown";
  private const string CyearUpDownPanel = "PART_YearUpDownPanel";
  private const string CeditMonthName = "PART_EditMonthName";
  private const string CnextMonthButtonName = "PART_NextMonthButton";
  private const string CprevMonthButtonName = "PART_PrevMonthButton";
  internal bool yearpressed;
  internal bool monthpressed;
  internal bool yearrangepress;
  internal bool mousescroll;
  internal bool mouseDayscroll;
  private bool isTouchSelection;
  internal bool mousemonthscroll;
  internal bool mouseyearscroll;
  internal bool mouseyearrangescroll;
  internal bool mouseweeknumberscroll;
  private Storyboard mmonthStoryboard;
  private Storyboard mmoveStoryboard;
  private Storyboard mvisualModeStoryboard;
  private DateTime m_shiftDate;
  private bool m_shiftDateChangeEnabled = true;
  private bool mbselectedDatesUpdateLocked;
  private int milockCounter;
  private Cell mpressedCell;
  private bool isMonthNavigated;
  private List<Syncfusion.Windows.Shared.Date> mselectedDatesList = new List<Syncfusion.Windows.Shared.Date>();
  private List<Syncfusion.Windows.Shared.Date> mInvalidDateList = new List<Syncfusion.Windows.Shared.Date>();
  private int m_iscrollCounter;
  private bool m_dateSetManual;
  private bool isControlSelection;
  internal NavigateButton m_nextButton;
  internal NavigateButton m_prevButton;
  internal MonthButton m_monthButton1;
  internal MonthButton m_monthButton2;
  private MonthPopup m_popup;
  private CalendarEdit.VisualModeHistory mvisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.Days);
  private Queue<CalendarEdit.VisualModeHistory> m_visualModeQueue = new Queue<CalendarEdit.VisualModeHistory>();
  private bool m_postScrollNeed;
  internal Button m_todayButton;
  private bool m_cellClicked;
  private bool wcellClicked;
  private CalendarVisualMode viewmode;
  private bool isNextModeSet;
  private CalendarVisualMode calendarVisualMode = CalendarVisualMode.WeekNumbers | CalendarVisualMode.Days | CalendarVisualMode.Months | CalendarVisualMode.Years | CalendarVisualMode.YearsRange;
  private CalendarVisualMode nextMode;
  private bool isTemplateApplied;
  internal DateTime miDate;
  internal DateTime mxDate;
  private static DateTime m_currentCultureMinDate = CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
  private DateTime m_oldMinDate = CalendarEdit.m_currentCultureMinDate;
  private DateTime m_newMinDate = CalendarEdit.m_currentCultureMinDate;
  private bool m_bsuspendEventFire;
  private Grid m_mainGrid;
  private ContentPresenter m_weekNumbersContainer;
  private ContentPresenter wcurrentweekNumbersContainer;
  private ContentPresenter wfollowweekNumbersContainer;
  private UpDown m_yearUpDown;
  private StackPanel m_yearUpDownPanel;
  private DispatcherTimer m_timer;
  private TextBlock m_editMonthName;
  private Hashtable m_toolTipDates = new Hashtable();
  public static int startYear;
  public static int endYear;
  public static int primaryDate;
  public static string clickedweeknumber;
  public bool Invalidateflag = true;
  public static readonly RoutedUICommand NextCommand;
  public static readonly RoutedUICommand PrevCommand;
  public static readonly RoutedUICommand UpCommand;
  public static readonly DependencyProperty SpecialDatesProperty = DependencyProperty.Register(nameof (SpecialDates), typeof (SpecialDatesCollection), typeof (CalendarEdit), new PropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnSpecialDatesCollectionChanged)));
  private bool updateSelection = true;
  public static readonly DependencyProperty CalendarProperty = DependencyProperty.Register(nameof (Calendar), typeof (System.Globalization.Calendar), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) CultureInfo.CurrentCulture.Calendar, new PropertyChangedCallback(CalendarEdit.OnCalendarChanged)));
  public static readonly DependencyProperty IsTodayButtonClickedProperty = DependencyProperty.Register(nameof (IsTodayButtonClicked), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnIsTodayButtonClickedChanged)));
  public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID), new PropertyChangedCallback(CalendarEdit.OnCultureChanged)));
  public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register(nameof (CalendarStyle), typeof (CalendarStyle), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) CalendarStyle.Vista, new PropertyChangedCallback(CalendarEdit.OnCalendarStyleChanged)));
  public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof (Date), typeof (DateTime), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) DateTime.Now.Date, new PropertyChangedCallback(CalendarEdit.OnDateChanged), new CoerceValueCallback(CalendarEdit.OnCoerceDate)));
  public static readonly DependencyProperty SelectedDatesProperty = DependencyProperty.Register(nameof (SelectedDates), typeof (DatesCollection), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnDatesCollectionChanged)));
  public static readonly DependencyProperty BlackoutDatesProperty = DependencyProperty.Register(nameof (BlackoutDates), typeof (BlackDatesCollection), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnBlackDatesCollectionChanged)));
  public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(nameof (MouseOverBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMouseOverBorderBrushChanged)));
  public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(nameof (MouseOverBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMouseOverBackgroundChanged)));
  public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(nameof (MouseOverForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMouseOverForegroundChanged)));
  public static readonly DependencyProperty SelectedDayCellBorderBrushProperty = DependencyProperty.Register(nameof (SelectedDayCellBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectedDayCellBorderBrushChanged)));
  public static readonly DependencyProperty TodayCellBorderBrushProperty = DependencyProperty.Register(nameof (TodayCellBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnTodayCellBorderBrushChanged)));
  public static readonly DependencyProperty TodayCellForegroundProperty = DependencyProperty.Register(nameof (TodayCellForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnTodayCellForegroundChanged)));
  public static readonly DependencyProperty TodayCellBackgroundProperty = DependencyProperty.Register(nameof (TodayCellBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnTodayCellBackgroundChanged)));
  public static readonly DependencyProperty TodayCellSelectedBorderBrushProperty = DependencyProperty.Register(nameof (TodayCellSelectedBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnTodayCellSelectedBorderBrushChanged)));
  public static readonly DependencyProperty TodayCellSelectedBackgroundProperty = DependencyProperty.Register(nameof (TodayCellSelectedBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnTodayCellSelectedBackgroundChanged)));
  public static readonly DependencyProperty SelectedDayCellHoverBackgroundProperty = DependencyProperty.Register(nameof (SelectedDayCellHoverBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectedDayCellHoverBackgroundChanged)));
  public static readonly DependencyProperty NotCurrentMonthForegroundProperty = DependencyProperty.Register(nameof (NotCurrentMonthForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnNotCurrentMonthForegroundChanged)));
  public static readonly DependencyProperty SelectedDayCellBackgroundProperty = DependencyProperty.Register(nameof (SelectedDayCellBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectedDayCellBackgroundChanged)));
  public static readonly DependencyProperty SelectedDayCellForegroundProperty = DependencyProperty.Register(nameof (SelectedDayCellForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectedDayCellForegroundChanged)));
  public static readonly DependencyProperty SelectionBorderBrushProperty = DependencyProperty.Register(nameof (SelectionBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectionBorderBrushChanged)));
  public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register(nameof (SelectionForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnSelectionForegroundChanged)));
  [Obsolete("InValidDateBorderBrush property is deprecated, use BlackoutDatesBorderBrush instead")]
  public static readonly DependencyProperty InValidDateBorderBrushProperty = DependencyProperty.Register(nameof (InValidDateBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, new PropertyChangedCallback(CalendarEdit.OnInValidDateBorderBrushChanged)));
  [Obsolete("InValidDateForeGround property is deprecated, use BlackoutDatesForeground instead")]
  public static readonly DependencyProperty InValidDateForeGroundProperty = DependencyProperty.Register(nameof (InValidDateForeGround), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.White, new PropertyChangedCallback(CalendarEdit.OnInValidDateForeGroundChanged)));
  [Obsolete("InValidDateBackground property is deprecated, use BlackoutDatesBackground instead")]
  public static readonly DependencyProperty InValidDateBackgroundProperty = DependencyProperty.Register(nameof (InValidDateBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, new PropertyChangedCallback(CalendarEdit.OnInValidDateBackgroundChanged)));
  [Obsolete("InValidDateCrossBackground property is deprecated, use BlackoutDatesCrossBrush instead")]
  public static readonly DependencyProperty InValidDateCrossBackgroundProperty = DependencyProperty.Register(nameof (InValidDateCrossBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(CalendarEdit.OnInValidDateCrossBackgroundChanged)));
  public static readonly DependencyProperty BlackoutDatesBorderBrushProperty = DependencyProperty.Register(nameof (BlackoutDatesBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, new PropertyChangedCallback(CalendarEdit.OnBlackoutDatesBorderBrushChanged)));
  public static readonly DependencyProperty BlackoutDatesForegroundProperty = DependencyProperty.Register(nameof (BlackoutDatesForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.White, new PropertyChangedCallback(CalendarEdit.OnBlackoutDatesForegroundChanged)));
  public static readonly DependencyProperty BlackoutDatesBackgroundProperty = DependencyProperty.Register(nameof (BlackoutDatesBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, new PropertyChangedCallback(CalendarEdit.OnBlackoutDatesBackgroundChanged)));
  public static readonly DependencyProperty BlackoutDatesCrossBrushProperty = DependencyProperty.Register(nameof (BlackoutDatesCrossBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(CalendarEdit.OnBlackoutDatesCrossBrushChanged)));
  public static readonly DependencyProperty WeekNumberSelectionBorderBrushProperty = DependencyProperty.Register(nameof (WeekNumberSelectionBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberSelectionBorderBrushChanged)));
  public static readonly DependencyProperty WeekNumberSelectionBorderThicknessProperty = DependencyProperty.Register(nameof (WeekNumberSelectionBorderThickness), typeof (Thickness), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberSelectionBorderThicknessChanged)));
  public static readonly DependencyProperty WeekNumberSelectionBorderCornerRadiusProperty = DependencyProperty.Register(nameof (WeekNumberSelectionBorderCornerRadius), typeof (CornerRadius), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberSelectionBorderCornerRadiusChanged)));
  public static readonly DependencyProperty WeekNumberBackgroundProperty = DependencyProperty.Register(nameof (WeekNumberBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberBackgroundChanged)));
  public static readonly DependencyProperty WeekNumberSelectionBackgroundProperty = DependencyProperty.Register(nameof (WeekNumberSelectionBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberSelectionBackgroundChanged)));
  public static readonly DependencyProperty WeekNumberHoverBackgroundProperty = DependencyProperty.Register(nameof (WeekNumberHoverBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberHoverBackgroundChanged)));
  public static readonly DependencyProperty WeekNumberHoverBorderBrushProperty = DependencyProperty.Register(nameof (WeekNumberHoverBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberHoverBorderBrushChanged)));
  public static readonly DependencyProperty WeekNumberHoverForegroundProperty = DependencyProperty.Register(nameof (WeekNumberHoverForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberHoverForegroundChanged)));
  public static readonly DependencyProperty WeekNumberSelectionForegroundProperty = DependencyProperty.Register(nameof (WeekNumberSelectionForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberSelectionForegroundChanged)));
  public static readonly DependencyProperty WeekNumberBorderBrushProperty = DependencyProperty.Register(nameof (WeekNumberBorderBrush), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberBorderBrushChanged)));
  public static readonly DependencyProperty WeekNumberForegroundProperty = DependencyProperty.Register(nameof (WeekNumberForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberForegroundChanged)));
  public static readonly DependencyProperty WeekNumberCornerRadiusProperty = DependencyProperty.Register(nameof (WeekNumberCornerRadius), typeof (CornerRadius), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberCornerRadiusChanged)));
  public static readonly DependencyProperty WeekNumberBorderThicknessProperty = DependencyProperty.Register(nameof (WeekNumberBorderThickness), typeof (Thickness), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnWeekNumberBorderThicknessChanged)));
  public static readonly DependencyProperty SelectionBorderCornerRadiusProperty = DependencyProperty.Register(nameof (SelectionBorderCornerRadius), typeof (CornerRadius), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(5.0), new PropertyChangedCallback(CalendarEdit.OnSelectionBorderCornerRadiusChanged)));
  public static readonly DependencyProperty AllowSelectionProperty = DependencyProperty.Register(nameof (AllowSelection), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnAllowSelectionChanged)));
  public static readonly DependencyProperty AllowMultiplySelectionProperty = DependencyProperty.Register(nameof (AllowMultiplySelection), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnAllowMultiplySelectionChanged)));
  [Obsolete("IsDayNamesAbbreviated property is deprecated, use ShowAbbreviatedDayNames instead")]
  public static readonly DependencyProperty IsDayNamesAbbreviatedProperty = DependencyProperty.Register(nameof (IsDayNamesAbbreviated), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnIsDayNamesAbbreviatedChanged)));
  [Obsolete("IsMonthNameAbbreviated property is deprecated, use ShowAbbreviatedMonthNames instead")]
  public static readonly DependencyProperty IsMonthNameAbbreviatedProperty = DependencyProperty.Register(nameof (IsMonthNameAbbreviated), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnIsMonthNamesAbbreviatedChanged)));
  public static readonly DependencyProperty ShowAbbreviatedDayNamesProperty = DependencyProperty.Register(nameof (ShowAbbreviatedDayNames), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnShowAbbreviatedDayNamesChanged)));
  public static readonly DependencyProperty ShowAbbreviatedMonthNamesProperty = DependencyProperty.Register(nameof (ShowAbbreviatedMonthNames), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnShowAbbreviatedMonthNamesChanged)));
  public static readonly DependencyProperty SelectionRangeModeProperty = DependencyProperty.Register(nameof (SelectionRangeMode), typeof (SelectionRangeMode), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) SelectionRangeMode.CurrentMonth, new PropertyChangedCallback(CalendarEdit.OnSelectionRangeModeChanged)));
  public static readonly DependencyProperty FrameMovingTimeProperty = DependencyProperty.Register(nameof (FrameMovingTime), typeof (int), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata((object) 300, new PropertyChangedCallback(CalendarEdit.OnFrameMovingTimeChanged)), new ValidateValueCallback(CalendarEdit.ValidateFrameMovingTime));
  public static readonly DependencyProperty ChangeModeTimeProperty = DependencyProperty.Register(nameof (ChangeModeTime), typeof (int), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 300, new PropertyChangedCallback(CalendarEdit.OnChangeModeTimeChanged)));
  public static readonly DependencyProperty MonthChangeDirectionProperty = DependencyProperty.Register(nameof (MonthChangeDirection), typeof (AnimationDirection), typeof (CalendarEdit), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMonthChangeDirectionChanged)));
  public static readonly DependencyProperty DayNameCellsDataTemplateProperty = DependencyProperty.Register(nameof (DayNameCellsDataTemplate), typeof (DataTemplate), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayNameCellsDataTemplateChanged)));
  public static readonly DependencyProperty NextScrollButtonTemplateProperty = DependencyProperty.Register(nameof (NextScrollButtonTemplate), typeof (ControlTemplate), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnNextScrollButtonTemplateChanged)));
  public static readonly DependencyProperty PreviousScrollButtonTemplateProperty = DependencyProperty.Register(nameof (PreviousScrollButtonTemplate), typeof (ControlTemplate), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnPreviousScrollButtonTemplateChanged)));
  public static readonly DependencyProperty DayCellsDataTemplateProperty = DependencyProperty.Register(nameof (DayCellsDataTemplate), typeof (DataTemplate), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayCellsDataTemplateChanged)));
  public static readonly DependencyProperty DayCellsStyleProperty = DependencyProperty.Register(nameof (DayCellsStyle), typeof (Style), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayCellsStyleChanged)));
  public static readonly DependencyProperty DayNameCellsStyleProperty = DependencyProperty.Register(nameof (DayNameCellsStyle), typeof (Style), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayNameCellsStyleChanged)));
  public static readonly DependencyProperty DayCellsDataTemplateSelectorProperty = DependencyProperty.Register(nameof (DayCellsDataTemplateSelector), typeof (DataTemplateSelector), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayCellsDataTemplateSelectorChanged)));
  public static readonly DependencyProperty DayNameCellsDataTemplateSelectorProperty = DependencyProperty.Register(nameof (DayNameCellsDataTemplateSelector), typeof (DataTemplateSelector), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(CalendarEdit.OnDayNameCellsDataTemplateSelectorChanged)));
  public static readonly DependencyProperty DateDataTemplatesProperty = DependencyProperty.Register(nameof (DateDataTemplates), typeof (DataTemplatesDictionary), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnDateDataTemplatesChanged)));
  public static readonly DependencyProperty DateStylesProperty = DependencyProperty.Register(nameof (DateStyles), typeof (StylesDictionary), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnDateStylesChanged)));
  public static readonly DependencyProperty DisableDateSelectionProperty = DependencyProperty.Register(nameof (DisableDateSelection), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnDisableDateSelectionChanged)));
  public static readonly DependencyProperty ScrollToDateEnabledProperty = DependencyProperty.Register(nameof (ScrollToDateEnabled), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnScrollToDateEnabledChanged)));
  protected internal static readonly DependencyProperty DayNamesGridProperty = DependencyProperty.Register(nameof (DayNamesGrid), typeof (DayNamesGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty WeekNumbersGridProperty = DependencyProperty.Register(nameof (WeekNumbersGrid), typeof (WeekNumbersGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty CurrentDayGridProperty = DependencyProperty.Register(nameof (CurrentDayGrid), typeof (DayGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty FollowingDayGridProperty = DependencyProperty.Register(nameof (FollowingDayGrid), typeof (DayGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty CurrentMonthGridProperty = DependencyProperty.Register(nameof (CurrentMonthGrid), typeof (MonthGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty CurrentYearGridProperty = DependencyProperty.Register(nameof (CurrentYearGrid), typeof (YearGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty CurrentYearRangeGridProperty = DependencyProperty.Register(nameof (CurrentYearRangeGrid), typeof (YearRangeGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty FollowingMonthGridProperty = DependencyProperty.Register(nameof (FollowingMonthGrid), typeof (MonthGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty FollowingYearGridProperty = DependencyProperty.Register(nameof (FollowingYearGrid), typeof (YearGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty FollowingYearRangeGridProperty = DependencyProperty.Register(nameof (FollowingYearRangeGrid), typeof (YearRangeGrid), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty CurrentWeekNumbersGridProperty = DependencyProperty.Register(nameof (CurrentWeekNumbersGrid), typeof (WeekNumberGridPanel), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty FollowingWeekNumbersGridProperty = DependencyProperty.Register(nameof (FollowingWeekNumbersGrid), typeof (WeekNumberGridPanel), typeof (CalendarEdit));
  protected internal static readonly DependencyProperty VisualModeProperty = DependencyProperty.Register(nameof (VisualMode), typeof (CalendarVisualMode), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) CalendarVisualMode.Days, new PropertyChangedCallback(CalendarEdit.OnVisualModeChanged)));
  public static readonly DependencyProperty MinDateProperty = DependencyProperty.Register(nameof (MinDate), typeof (DateTime), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMinDateChanged), new CoerceValueCallback(CalendarEdit.OnCoerceMinDateProperty)));
  public static readonly DependencyProperty MaxDateProperty = DependencyProperty.Register(nameof (MaxDate), typeof (DateTime), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnMaxDateChanged), new CoerceValueCallback(CalendarEdit.OnCoerceMaxDateProperty)));
  protected internal static readonly DependencyProperty VisibleDataProperty = DependencyProperty.Register(nameof (VisibleData), typeof (VisibleDate), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(CalendarEdit.OnVisibleDataChanged), new CoerceValueCallback(CalendarEdit.OnCoerceVisibleData)));
  public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register(nameof (HeaderForeground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Black));
  public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(nameof (HeaderBackground), typeof (Brush), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent));
  protected static readonly DependencyPropertyKey TodayDatePropertyKey = DependencyProperty.RegisterReadOnly(nameof (TodayDate), typeof (string), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty TodayDateProperty = CalendarEdit.TodayDatePropertyKey.DependencyProperty;
  public static readonly DependencyProperty TodayRowIsVisibleProperty = DependencyProperty.Register(nameof (TodayRowIsVisible), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnTodayRowIsVisibleChanged)));
  public static readonly DependencyProperty MinMaxHiddenProperty = DependencyProperty.Register(nameof (MinMaxHidden), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnMinMaxHiddenChanged)));
  [Obsolete("IsShowWeekNumbers property is deprecated, use ShowWeekNumbers instead")]
  public static readonly DependencyProperty IsShowWeekNumbersProperty = DependencyProperty.Register(nameof (IsShowWeekNumbers), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnIsShowWeekNumbersChanged)));
  [Obsolete("IsShowWeekNumbersGrid property is deprecated")]
  public static readonly DependencyProperty IsShowWeekNumbersGridProperty = DependencyProperty.Register(nameof (IsShowWeekNumbersGrid), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnShowWeekNumbersGridChanged)));
  [Obsolete("IsAllowYearSelection property is deprecated, use AllowYearEditing instead")]
  public static readonly DependencyProperty IsAllowYearSelectionProperty = DependencyProperty.Register(nameof (IsAllowYearSelection), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnIsAllowYearSelectionChanged)));
  public static readonly DependencyProperty ShowWeekNumbersProperty = DependencyProperty.Register(nameof (ShowWeekNumbers), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnShowWeekNumbersChanged)));
  public static readonly DependencyProperty AllowYearEditingProperty = DependencyProperty.Register(nameof (AllowYearEditing), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(CalendarEdit.OnAllowYearEditingChanged)));
  public static readonly DependencyProperty ShowPreviousMonthDaysProperty = DependencyProperty.Register(nameof (ShowPreviousMonthDays), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnShowPreviousMonthDaysChanged)));
  public static readonly DependencyProperty ShowNextMonthDaysProperty = DependencyProperty.Register(nameof (ShowNextMonthDays), typeof (bool), typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(CalendarEdit.OnShowNextMonthDaysChanged)));

  static CalendarEdit()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CalendarEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CalendarEdit)));
    CalendarEdit.NextCommand = new RoutedUICommand("Next month", nameof (NextCommand), typeof (CalendarEdit));
    CalendarEdit.PrevCommand = new RoutedUICommand("Prev month", nameof (PrevCommand), typeof (CalendarEdit));
    CalendarEdit.UpCommand = new RoutedUICommand("VisualMode level up", nameof (UpCommand), typeof (CalendarEdit));
    CalendarEdit.NextCommand.InputGestures.Add((InputGesture) new KeyGesture(Key.Right, ModifierKeys.Alt));
    CalendarEdit.PrevCommand.InputGestures.Add((InputGesture) new KeyGesture(Key.Left, ModifierKeys.Alt));
    CalendarEdit.UpCommand.InputGestures.Add((InputGesture) new KeyGesture(Key.Back, ModifierKeys.Alt));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public CalendarEdit()
  {
    CommandBinding commandBinding1 = new CommandBinding((ICommand) CalendarEdit.NextCommand, new ExecutedRoutedEventHandler(this.NextCommandExecute), new CanExecuteRoutedEventHandler(this.NextCommandCanExecute));
    CommandBinding commandBinding2 = new CommandBinding((ICommand) CalendarEdit.PrevCommand, new ExecutedRoutedEventHandler(this.PrevCommandExecute), new CanExecuteRoutedEventHandler(this.PrevCommandCanExecute));
    CommandBinding commandBinding3 = new CommandBinding((ICommand) CalendarEdit.UpCommand, new ExecutedRoutedEventHandler(this.UpCommandExecute), new CanExecuteRoutedEventHandler(this.UpCommandCanExecute));
    this.InitializeGrid();
    this.m_shiftDate = this.Date;
    this.SelectedDates = new DatesCollection();
    this.BlackoutDates = new BlackDatesCollection();
    this.SpecialDates = new SpecialDatesCollection();
    this.SelectedDates.AllowInsert = this.AllowSelection;
    this.CommandBindings.Add(commandBinding1);
    this.CommandBindings.Add(commandBinding2);
    this.CommandBindings.Add(commandBinding3);
    if (this.Calendar != null)
    {
      this.miDate = this.Calendar.MinSupportedDateTime;
      this.mxDate = this.Calendar.MaxSupportedDateTime;
      this.MinDate = this.Calendar.MinSupportedDateTime;
      this.MaxDate = this.Calendar.MaxSupportedDateTime;
    }
    this.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
    this.BlackoutDates.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
    this.SpecialDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SpecialDates_CollectionChanged);
    this.SpecialDates.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SpecialDates_CollectionChanged);
    this.Loaded += new RoutedEventHandler(this.CalendarEdit_Loaded);
    this.Unloaded += new RoutedEventHandler(this.CalendarEdit_Unloaded);
    this.IsManipulationEnabled = true;
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void SpecialDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action != NotifyCollectionChangedAction.Reset)
    {
      if (this.CurrentDayGrid != null)
        this.CurrentDayGrid.SetSpecialDatesTemplate();
      if (this.FollowingDayGrid == null)
        return;
      this.FollowingDayGrid.SetSpecialDatesTemplate();
    }
    else
    {
      if (this.CurrentDayGrid == null)
        return;
      this.CurrentDayGrid.UpdateTemplateAndSelector((DataTemplate) null, (DataTemplateSelector) null, (DataTemplatesDictionary) null);
    }
  }

  private void CurrentDayGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.updateSelection = true;
    if (e.Handled || e.Source == null || !(e.Source is DayCell))
      return;
    this.HandleDayGridButtonUp(e.Source as DayCell);
  }

  private void DayGrid_StylusButtonUp(object sender, StylusButtonEventArgs e)
  {
    this.updateSelection = true;
    if (e.Handled || e.Source == null || !(e.Source is DayCell))
      return;
    this.HandleDayGridButtonUp(e.Source as DayCell);
  }

  private void HandleDayGridButtonUp(DayCell releasedCell)
  {
    if (releasedCell == null || releasedCell == null)
      return;
    if (!releasedCell.IsCurrentMonth)
      this.ScrollMonth(releasedCell.Date.Month);
    if (!releasedCell.IsSelected && this.AllowMultiplySelection)
      releasedCell.IsDate = false;
    this.mpressedCell = (Cell) null;
  }

  private void DayCell_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    e.Handled = true;
  }

  private void CalendarEdit_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.SelectedDates != null)
    {
      this.SelectedDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedDates_OnCollectionChanged);
      this.SelectedDates.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedDates_OnCollectionChanged);
    }
    if (this.SelectedDates == null || this.SelectedDates.Count != 0 || !this.AllowSelection || !this.IsInitializeComplete)
      return;
    this.SelectedDates.Add(DateTime.Now);
  }

  private void CalendarEdit_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.BlackoutDates != null)
      this.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
    if (this.SelectedDates == null)
      return;
    this.SelectedDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedDates_OnCollectionChanged);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditAutomationPeer(this);
  }

  private void BlackoutDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.OnBlackoutDatesCollectionChanged(e);
  }

  private void OnBlackoutDatesCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
    if (this.Calendar == null || e == null)
      return;
    if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null && e.NewItems[0] != null)
    {
      List<DateTime> dateRange = this.GetDateRange((e.NewItems[0] as BlackoutDatesRange).StartDate, (e.NewItems[0] as BlackoutDatesRange).EndDate);
      for (int index = 0; index < dateRange.Count; ++index)
        this.InvalidDates.Add(new Syncfusion.Windows.Shared.Date(dateRange[index], this.Calendar));
      this.InvalidDates.Sort();
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
      this.InvalidDates.Clear();
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      List<DateTime> dateRange = this.GetDateRange((e.OldItems[0] as BlackoutDatesRange).StartDate, (e.OldItems[0] as BlackoutDatesRange).EndDate);
      for (int index = 0; index < dateRange.Count; ++index)
        this.InvalidDates.Remove(new Syncfusion.Windows.Shared.Date(dateRange[index], this.Calendar));
    }
    else if (e.Action == NotifyCollectionChangedAction.Replace)
    {
      int num = 0;
      List<DateTime> dateRange1 = this.GetDateRange((e.OldItems[0] as BlackoutDatesRange).StartDate, (e.OldItems[0] as BlackoutDatesRange).EndDate);
      List<DateTime> dateRange2 = this.GetDateRange((e.NewItems[0] as BlackoutDatesRange).StartDate, (e.NewItems[0] as BlackoutDatesRange).EndDate);
      for (int index = 0; index < dateRange1.Count; ++index)
      {
        Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date(dateRange1[index], this.Calendar);
        num = this.InvalidDates.IndexOf(date);
        this.InvalidDates.Remove(date);
      }
      for (int index = 0; index < dateRange2.Count; ++index)
      {
        Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date(dateRange2[index], this.Calendar);
        this.InvalidDates.Insert(num + index, date);
      }
      this.InvalidDates.Sort();
    }
    if (!this.IsLoaded)
      return;
    if (this.CurrentDayGrid != null)
      this.CurrentDayGrid.SetIsInvalid();
    if (this.FollowingDayGrid == null)
      return;
    this.FollowingDayGrid.SetIsInvalid();
  }

  private void OnBlackoutDatesCollectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Calendar == null || this.BlackoutDates == null || this.BlackoutDates.Count <= 0)
      return;
    for (int index1 = 0; index1 < this.BlackoutDates.Count; ++index1)
    {
      List<DateTime> dateRange = this.GetDateRange(this.BlackoutDates[index1].StartDate, this.BlackoutDates[index1].EndDate);
      for (int index2 = 0; index2 < dateRange.Count; ++index2)
        this.InvalidDates.Add(new Syncfusion.Windows.Shared.Date(dateRange[index2], this.Calendar));
      this.InvalidDates.Sort();
      if (this.IsLoaded)
      {
        if (this.CurrentDayGrid != null)
          this.CurrentDayGrid.SetIsInvalid();
        if (this.FollowingDayGrid != null)
          this.FollowingDayGrid.SetIsInvalid();
      }
    }
  }

  public event PropertyChangedCallback DateChanged;

  public event CalendarEdit.MonthChangedEventHandler MonthChanged;

  public event PropertyChangedCallback CultureChanged;

  internal event PropertyChangedCallback IsTodayButtonClickedChanged;

  public event PropertyChangedCallback CalendarChanged;

  public event PropertyChangedCallback CalendarStyleChanged;

  public event PropertyChangedCallback DisableDateSelectionChanged;

  public event PropertyChangedCallback AllowSelectionChanged;

  public event PropertyChangedCallback AllowMultiplySelectionChanged;

  [Obsolete("IsDayNamesAbbreviatedChanged  event is deprecated")]
  public event PropertyChangedCallback IsDayNamesAbbreviatedChanged;

  [Obsolete("IsMonthNameAbbreviatedChanged  event is deprecated")]
  public event PropertyChangedCallback IsMonthNameAbbreviatedChanged;

  public event PropertyChangedCallback SelectionRangeModeChanged;

  public event PropertyChangedCallback SelectionBorderBrushChanged;

  public event PropertyChangedCallback MouseOverBorderBrushChanged;

  public event PropertyChangedCallback MouseOverBackgroundChanged;

  public event PropertyChangedCallback MouseOverForegroundChanged;

  public event PropertyChangedCallback SelectedDayCellBorderBrushChanged;

  public event PropertyChangedCallback TodayCellBorderBrushChanged;

  public event PropertyChangedCallback TodayCellForegroundChanged;

  public event PropertyChangedCallback TodayCellBackgroundChanged;

  public event PropertyChangedCallback TodayCellSelectedBorderBrushChanged;

  public event PropertyChangedCallback TodayCellSelectedBackgroundChanged;

  public event PropertyChangedCallback SelectedDayCellHoverBackgroundChanged;

  public event PropertyChangedCallback SelectedDayCellBackgroundChanged;

  public event PropertyChangedCallback SelectedDayCellForegroundChanged;

  public event PropertyChangedCallback NotCurrentMonthForegroundChanged;

  public event PropertyChangedCallback SelectionForegroundChanged;

  public event PropertyChangedCallback WeekNumberSelectionBorderBrushChanged;

  public event PropertyChangedCallback WeekNumberSelectionBorderThicknessChanged;

  public event PropertyChangedCallback WeekNumberSelectionBorderCornerRadiusChanged;

  public event PropertyChangedCallback WeekNumberBackgroundChanged;

  public event PropertyChangedCallback WeekNumberForegroundChanged;

  public event PropertyChangedCallback WeekNumberHoverForegroundChanged;

  public event PropertyChangedCallback WeekNumberSelectionForegroundChanged;

  public event PropertyChangedCallback WeekNumberSelectionBackgroundChanged;

  public event PropertyChangedCallback WeekNumberHoverBackgroundChanged;

  public event PropertyChangedCallback WeekNumberHoverBorderBrushChanged;

  public event PropertyChangedCallback WeekNumberBorderBrushChanged;

  public event PropertyChangedCallback WeekNumberBorderThicknessChanged;

  public event PropertyChangedCallback WeekNumberCornerRadiusChanged;

  public event PropertyChangedCallback SelectionBorderCornerRadiusChanged;

  public event PropertyChangedCallback FrameMovingTimeChanged;

  public event PropertyChangedCallback ChangeModeTimeChanged;

  public event PropertyChangedCallback MonthChangeDirectionChanged;

  public event PropertyChangedCallback DayNameCellsDataTemplateChanged;

  public event PropertyChangedCallback PreviousScrollButtonTemplateChanged;

  public event PropertyChangedCallback NextScrollButtonTemplateChanged;

  public event PropertyChangedCallback DayCellsDataTemplateChanged;

  public event PropertyChangedCallback DayCellsStyleChanged;

  public event PropertyChangedCallback DayNameCellsStyleChanged;

  public event PropertyChangedCallback DayCellsDataTemplateSelectorChanged;

  public event PropertyChangedCallback DayNameCellsDataTemplateSelectorChanged;

  public event PropertyChangedCallback DateDataTemplatesChanged;

  public event PropertyChangedCallback DateStylesChanged;

  public event PropertyChangedCallback ScrollToDateEnabledChanged;

  public event PropertyChangedCallback VisualModeChanged;

  public event PropertyChangedCallback TodayRowIsVisibleChanged;

  public event PropertyChangedCallback MinMaxHiddenChanged;

  [Obsolete("IsShowWeekNumbersChanged event is deprecated")]
  public event PropertyChangedCallback IsShowWeekNumbersChanged;

  [Obsolete("IsShowWeekNumbersGridChanged event is deprecated")]
  public event PropertyChangedCallback IsShowWeekNumbersGridChanged;

  [Obsolete("IsAllowYearSelectionChanged event is deprecated")]
  public event PropertyChangedCallback IsAllowYearSelectionChanged;

  public event PropertyChangedCallback ShowPreviousMonthDaysChanged;

  public event PropertyChangedCallback ShowNextMonthDaysChanged;

  protected internal event MouseButtonEventHandler YearRangeCellMouseLeftButtonUp;

  protected internal event MouseButtonEventHandler YearRangeCellMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler YearCellMouseLeftButtonUp;

  protected internal event MouseButtonEventHandler YearCellMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler MonthCellMouseLeftButtonUp;

  protected internal event MouseButtonEventHandler MonthCellMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler DayCellMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler DayCellMouseLeftButtonUp;

  protected internal event MouseButtonEventHandler DayNameCellMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler WeekNumberCellPanelMouseLeftButtonDown;

  protected internal event MouseButtonEventHandler WeekNumberCellMouseLeftButtonDown;

  protected internal event PropertyChangedCallback VisibleDataChanged;

  public List<Syncfusion.Windows.Shared.Date> SelectedDatesList
  {
    get => this.mselectedDatesList;
    set => this.mselectedDatesList = value;
  }

  public List<Syncfusion.Windows.Shared.Date> InvalidDates
  {
    get => this.mInvalidDateList;
    set => this.mInvalidDateList = value;
  }

  public bool IsInitializeComplete
  {
    get
    {
      return this.CurrentDayGrid != null && this.CurrentDayGrid.ParentCalendar != null && this.FollowingDayGrid != null && this.FollowingDayGrid.ParentCalendar != null;
    }
  }

  public CalendarVisualMode VisualMode
  {
    get => (CalendarVisualMode) this.GetValue(CalendarEdit.VisualModeProperty);
    set => this.SetValue(CalendarEdit.VisualModeProperty, (object) value);
  }

  public CalendarVisualMode ViewMode
  {
    get => this.viewmode;
    set
    {
      if (this.viewmode == value)
        return;
      this.viewmode = value;
      if (this.VisualMode != value && value != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
        this.VisualMode = value;
      if (!this.HasFlag((Enum) this.calendarVisualMode, (Enum) value))
        return;
      if ((value & CalendarVisualMode.WeekNumbers) == CalendarVisualMode.WeekNumbers)
        this.VisualMode = CalendarVisualMode.WeekNumbers;
      else if ((value & CalendarVisualMode.Days) == CalendarVisualMode.Days)
        this.VisualMode = CalendarVisualMode.Days;
      else if ((value & CalendarVisualMode.Months) == CalendarVisualMode.Months)
        this.VisualMode = CalendarVisualMode.Months;
      else if ((value & CalendarVisualMode.Years) == CalendarVisualMode.Years)
      {
        this.VisualMode = CalendarVisualMode.Years;
      }
      else
      {
        if ((value & CalendarVisualMode.YearsRange) != CalendarVisualMode.YearsRange)
          return;
        this.VisualMode = CalendarVisualMode.YearsRange;
      }
    }
  }

  protected CalendarEdit.VisualModeHistory VisualModeInfo
  {
    get => this.mvisualModeInfo;
    set => this.mvisualModeInfo = value;
  }

  internal bool IsWeekCellClicked
  {
    get => this.wcellClicked;
    set => this.wcellClicked = value;
  }

  internal bool IsCellClicked
  {
    get => this.m_cellClicked;
    set => this.m_cellClicked = value;
  }

  internal Hashtable TooltipDates => this.m_toolTipDates;

  internal bool IsNextModeSet
  {
    get => this.isNextModeSet;
    set => this.isNextModeSet = value;
  }

  internal CalendarVisualMode NextMode
  {
    get => this.nextMode;
    set => this.nextMode = value;
  }

  public void SetToolTip(int rowIndex, int colIndex, System.Windows.Controls.ToolTip tooltip)
  {
    this.SetCellToolTip(rowIndex, colIndex, this.CurrentDayGrid, tooltip);
    this.SetCellToolTip(rowIndex, colIndex, this.FollowingDayGrid, tooltip);
  }

  public void SetToolTip(Syncfusion.Windows.Shared.Date date, System.Windows.Controls.ToolTip tooltip)
  {
    if (this.m_toolTipDates.ContainsKey((object) date))
      this.m_toolTipDates.Remove((object) date);
    this.m_toolTipDates.Add((object) date, (object) tooltip);
    this.CurrentDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    this.FollowingDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
  }

  internal int GetWeekNumber(DateTime dt)
  {
    int dayOfYear = dt.DayOfYear;
    int dayOfWeek = (int) this.Culture.Calendar.GetDayOfWeek(new DateTime(dt.Year, 1, 1));
    int num = dayOfWeek <= 4 ? 4 - dayOfWeek : 11 - dayOfWeek;
    return (dayOfYear - num) / 7 + 1;
  }

  public void LockSelectedDatesUpdate()
  {
    ++this.milockCounter;
    this.mbselectedDatesUpdateLocked = true;
  }

  public void UnLockSelectedDatesUpdate()
  {
    int num;
    if ((num = this.milockCounter - 1) != 0)
      return;
    this.mbselectedDatesUpdateLocked = false;
    this.milockCounter = 0;
  }

  public void SetVisibleMonth(int month)
  {
    this.VisibleData = month >= 1 && month <= 12 ? this.VisibleData with
    {
      VisibleMonth = month
    } : throw new ArgumentOutOfRangeException(nameof (month), (object) month, "Not a valid parameter value, it must be in the range 1-12");
  }

  public void SetVisibleYear(int year)
  {
    int year1 = this.Calendar.GetYear(this.Calendar.MaxSupportedDateTime);
    int year2 = this.Calendar.GetYear(this.Calendar.MinSupportedDateTime);
    if (year < year2 || year > year1)
      throw new ArgumentOutOfRangeException(nameof (year), (object) year, $"Not a valid parameter value, it must be in the range {year2.ToString()}- {year1.ToString()}");
    this.VisibleData = this.VisibleData with
    {
      VisibleYear = year
    };
  }

  protected virtual void ScrollToDate()
  {
    Syncfusion.Windows.Shared.Date date1 = new Syncfusion.Windows.Shared.Date(this.Date.Year, this.Date.Month, this.Date.Day);
    VisibleDate date2 = new VisibleDate(date1.Year, date1.Month, date1.Day);
    if (this.VisualMode == CalendarVisualMode.Days)
    {
      if (!this.IsStoryboardActive(this.mmonthStoryboard))
      {
        int visibleYear = this.VisibleData.VisibleYear;
        int visibleMonth = this.VisibleData.VisibleMonth;
        int year = this.Calendar.GetYear(this.Date);
        int month = this.Calendar.GetMonth(this.Date);
        DateTime dateTime1;
        DateTime dateTime2;
        if (this.MinMaxHidden)
        {
          dateTime1 = new DateTime(this.Calendar.GetYear(this.MaxDate), this.Calendar.GetMonth(this.MaxDate), this.Calendar.GetDayOfMonth(this.MaxDate));
          dateTime2 = new DateTime(this.Calendar.GetYear(this.MinDate), this.Calendar.GetMonth(this.MinDate), this.Calendar.GetDayOfMonth(this.MinDate));
        }
        else
        {
          dateTime1 = new DateTime(this.Calendar.GetYear(this.mxDate), this.Calendar.GetMonth(this.mxDate), this.Calendar.GetDayOfMonth(this.mxDate));
          dateTime2 = new DateTime(this.Calendar.GetYear(this.miDate), this.Calendar.GetMonth(this.miDate), this.Calendar.GetDayOfMonth(this.miDate));
        }
        DateTime startDate = new DateTime(visibleYear, visibleMonth, 1);
        DateTime endDate;
        if (startDate > dateTime1 || startDate < dateTime2)
        {
          startDate = new DateTime(this.Calendar.GetYear(this.Date), this.Calendar.GetMonth(this.Date), 1, this.Calendar);
          endDate = new DateTime(year, month, 1, this.Calendar);
        }
        else
          endDate = new DateTime(year, month, 1);
        int monthDelta = this.CalculateMonthDelta(startDate, endDate);
        if (monthDelta == 0)
          return;
        if (this.IsAnimationRequired())
        {
          if (startDate < endDate)
            this.BeginMoving(CalendarEdit.MoveDirection.Next, monthDelta);
          else
            this.BeginMoving(CalendarEdit.MoveDirection.Prev, monthDelta);
        }
        else
          this.VisibleData = date2;
      }
      else
        this.m_postScrollNeed = true;
    }
    else
    {
      if (this.DisableDateSelection)
        return;
      if (this.IsAnimationRequired())
      {
        if (this.m_visualModeQueue.Count == 0 || this.IsStoryboardActive(this.mvisualModeStoryboard))
          return;
        CalendarEdit.VisualModeHistory visualModeHistory = this.m_visualModeQueue.Dequeue();
        this.VisualMode = visualModeHistory.NewMode;
        this.VisualModeInfo = visualModeHistory;
        this.ChangeVisualModePreview(date2);
      }
      else
      {
        this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Hidden;
        this.VisualMode = CalendarVisualMode.Days;
        this.DayNamesGrid.RenderTransform = (Transform) new ScaleTransform(1.0, 1.0);
        this.FindCurrentGrid(this.VisualMode).RenderTransform = (Transform) new ScaleTransform(1.0, 1.0);
        this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
        this.DayNamesGrid.Visibility = Visibility.Visible;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.Days);
        this.VisibleData = date2;
      }
    }
  }

  protected virtual void BeginMoving(CalendarEdit.MoveDirection direction, int month)
  {
    if (this.IsOutOfDateRange(month))
      return;
    TranslateTransform scopedElement1 = new TranslateTransform();
    TranslateTransform scopedElement2 = new TranslateTransform();
    DoubleAnimation element1 = new DoubleAnimation();
    DoubleAnimation element2 = new DoubleAnimation();
    DoubleAnimation element3 = new DoubleAnimation();
    DoubleAnimation element4 = new DoubleAnimation();
    DoubleAnimation element5 = new DoubleAnimation();
    VisibleDate visibleData = this.VisibleData;
    this.mmonthStoryboard = new Storyboard();
    Timeline.SetDesiredFrameRate((Timeline) this.mmonthStoryboard, new int?(24));
    this.mmonthStoryboard.AccelerationRatio = 0.7;
    this.mmonthStoryboard.DecelerationRatio = 0.3;
    element1.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FrameMovingTime));
    element2.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FrameMovingTime));
    element3.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FrameMovingTime));
    element4.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FrameMovingTime));
    element5.Duration = new Duration(TimeSpan.FromMilliseconds((double) (this.FrameMovingTime / 2)));
    NameScope.SetNameScope((DependencyObject) this, (INameScope) new NameScope());
    if (!this.DisableDateSelection)
    {
      this.FollowingDayGrid.Visibility = Visibility.Visible;
      this.CurrentDayGrid.Visibility = Visibility.Visible;
      this.m_monthButton1.Visibility = Visibility.Visible;
      this.m_monthButton2.Visibility = Visibility.Visible;
      this.CurrentDayGrid.SelectionBorder.Visibility = Visibility.Hidden;
      this.FollowingDayGrid.SelectionBorder.Visibility = Visibility.Hidden;
      this.AddMonth(month);
      this.FollowingDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.WeekNumbersGrid.SetWeekNumbers(this.FollowingDayGrid.WeekNumbers);
      this.CurrentDayGrid.Initialize(visibleData, this.Culture, this.Calendar);
    }
    else
      this.AddMonth(month);
    this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    this.m_monthButton2.Initialize(visibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    element1.To = new double?(0.0);
    if (direction == CalendarEdit.MoveDirection.Next)
    {
      if (this.MonthChangeDirection == AnimationDirection.Horizontal)
      {
        scopedElement1.X = this.CurrentDayGrid.ActualWidth;
        element2.To = new double?(-this.CurrentDayGrid.ActualWidth);
      }
      if (this.MonthChangeDirection == AnimationDirection.Vertical)
      {
        double layOnValue = this.CalculateLayOnValue(this.CurrentDayGrid);
        scopedElement1.Y = this.CurrentDayGrid.ActualHeight - layOnValue;
        element2.To = new double?(-this.CurrentDayGrid.ActualHeight + layOnValue);
      }
    }
    if (direction == CalendarEdit.MoveDirection.Prev)
    {
      if (this.MonthChangeDirection == AnimationDirection.Horizontal)
      {
        scopedElement1.X = -this.CurrentDayGrid.ActualWidth;
        element2.To = new double?(this.CurrentDayGrid.ActualWidth);
      }
      if (this.MonthChangeDirection == AnimationDirection.Vertical)
      {
        double layOnValue = this.CalculateLayOnValue(this.FollowingDayGrid);
        scopedElement1.Y = -this.CurrentDayGrid.ActualHeight + layOnValue;
        element2.To = new double?(this.CurrentDayGrid.ActualHeight - layOnValue);
      }
    }
    element3.To = new double?(1.0);
    element4.To = new double?(1.0);
    element5.To = new double?(0.0);
    this.FollowingDayGrid.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
    this.m_monthButton1.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
    this.m_monthButton2.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
    this.FollowingDayGrid.Opacity = 0.01;
    this.m_monthButton1.Opacity = 0.01;
    Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
    Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath((object) UIElement.OpacityProperty));
    Storyboard.SetTargetProperty((DependencyObject) element5, new PropertyPath((object) UIElement.OpacityProperty));
    this.CurrentDayGrid.RenderTransform = (Transform) scopedElement2;
    this.FollowingDayGrid.RenderTransform = (Transform) scopedElement1;
    this.RegisterName("MoveFollowing", (object) scopedElement1);
    this.RegisterName("MoveCurrent", (object) scopedElement2);
    this.RegisterName("NextDayGrid", (object) this.FollowingDayGrid);
    this.RegisterName("MonthButton1", (object) this.m_monthButton1);
    this.RegisterName("MonthButton2", (object) this.m_monthButton2);
    Storyboard.SetTargetName((DependencyObject) element1, "MoveFollowing");
    Storyboard.SetTargetName((DependencyObject) element2, "MoveCurrent");
    Storyboard.SetTargetName((DependencyObject) element3, "NextDayGrid");
    Storyboard.SetTargetName((DependencyObject) element4, "MonthButton1");
    Storyboard.SetTargetName((DependencyObject) element5, "MonthButton2");
    if (this.MonthChangeDirection == AnimationDirection.Horizontal)
    {
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) TranslateTransform.XProperty));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) TranslateTransform.XProperty));
    }
    if (this.MonthChangeDirection == AnimationDirection.Vertical)
    {
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) TranslateTransform.YProperty));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) TranslateTransform.YProperty));
    }
    this.mmonthStoryboard.Children.Add((Timeline) element1);
    this.mmonthStoryboard.Children.Add((Timeline) element2);
    this.mmonthStoryboard.Children.Add((Timeline) element3);
    this.mmonthStoryboard.Children.Add((Timeline) element4);
    this.mmonthStoryboard.Children.Add((Timeline) element5);
    Style style = new Style(typeof (Control));
    ControlTemplate controlTemplate = new ControlTemplate();
    Setter setter = new Setter(Control.TemplateProperty, (object) null);
    style.Setters.Add((SetterBase) setter);
    this.CurrentDayGrid.FocusVisualStyle = style;
    this.FollowingDayGrid.FocusVisualStyle = style;
    this.mmonthStoryboard.Completed += new EventHandler(this.OnAnimationCompleted);
    this.mmonthStoryboard.Begin((FrameworkElement) this, true);
    foreach (DayCell cells in this.FollowingDayGrid.CellsCollection)
    {
      if (cells.IsFirstDayofMonth && cells.IsCurrentMonth)
      {
        DateTime dateTime = cells.Date.ToDateTime(this.Calendar);
        if (this.AllowSelection && this.Invalidateflag && this.SelectedDates != null)
        {
          if (this.SelectedDates.Count == 0)
          {
            this.SelectedDates.Add(dateTime);
            this.m_dateSetManual = true;
            this.Date = dateTime;
          }
          else
          {
            this.ProcessSelectedDatesCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList) this.SelectedDates));
            this.m_dateSetManual = true;
            this.UpdateCellClickedValue();
            if (this.IsCellClicked || this.isMonthNavigated)
            {
              this.Date = dateTime;
              this.isMonthNavigated = false;
            }
          }
        }
      }
    }
    if (this.MonthChanged == null)
      return;
    this.MonthChanged((object) this, new MonthChangedEventArgs(visibleData.VisibleMonth, this.Date.Month, (object) this));
  }

  protected virtual void Move(CalendarEdit.MoveDirection direction)
  {
    CalendarEditGrid calendarEditGrid1 = (CalendarEditGrid) null;
    CalendarEditGrid calendarEditGrid2 = (CalendarEditGrid) null;
    Syncfusion.Windows.Shared.Date returnDate = new Syncfusion.Windows.Shared.Date();
    CalendarVisualMode mode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      mode = this.ViewMode;
    if (this.IsOutOfDateRange(direction, ref returnDate))
      return;
    if (mode == CalendarVisualMode.Months)
    {
      calendarEditGrid1 = (CalendarEditGrid) this.CurrentMonthGrid;
      calendarEditGrid2 = (CalendarEditGrid) this.FollowingMonthGrid;
    }
    if (mode == CalendarVisualMode.Years)
    {
      calendarEditGrid1 = (CalendarEditGrid) this.CurrentYearGrid;
      calendarEditGrid2 = (CalendarEditGrid) this.FollowingYearGrid;
    }
    if (mode == CalendarVisualMode.YearsRange)
    {
      calendarEditGrid1 = (CalendarEditGrid) this.CurrentYearRangeGrid;
      calendarEditGrid2 = (CalendarEditGrid) this.FollowingYearRangeGrid;
    }
    if (mode == CalendarVisualMode.WeekNumbers)
    {
      calendarEditGrid1 = (CalendarEditGrid) this.CurrentWeekNumbersGrid;
      calendarEditGrid2 = (CalendarEditGrid) this.FollowingWeekNumbersGrid;
    }
    VisibleDate visibleDate = new VisibleDate(returnDate.Year, returnDate.Month, returnDate.Day);
    VisibleDate visibleData = this.VisibleData;
    TranslateTransform scopedElement1 = new TranslateTransform();
    TranslateTransform scopedElement2 = new TranslateTransform();
    DoubleAnimation element1 = new DoubleAnimation();
    DoubleAnimation element2 = new DoubleAnimation();
    DoubleAnimation element3 = new DoubleAnimation();
    DoubleAnimation element4 = new DoubleAnimation();
    element2.Duration = (Duration) TimeSpan.FromMilliseconds((double) this.FrameMovingTime);
    element1.Duration = (Duration) TimeSpan.FromMilliseconds((double) this.FrameMovingTime);
    element3.Duration = (Duration) TimeSpan.FromMilliseconds((double) this.FrameMovingTime);
    element4.Duration = (Duration) TimeSpan.FromMilliseconds((double) this.FrameMovingTime);
    this.VisibleData = visibleDate;
    calendarEditGrid1.Initialize(visibleData, this.Culture, this.Calendar);
    calendarEditGrid2.Initialize(visibleDate, this.Culture, this.Calendar);
    if (this.DisableDateSelection && calendarEditGrid2 != this.CurrentMonthGrid)
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
    calendarEditGrid1.Visibility = Visibility.Visible;
    calendarEditGrid2.Visibility = Visibility.Visible;
    this.m_monthButton1.Visibility = Visibility.Visible;
    this.m_monthButton2.Visibility = Visibility.Visible;
    this.m_monthButton1.Initialize(visibleDate, this.Calendar, this.Culture, this.ShowAbbreviatedDayNames, mode);
    this.m_monthButton2.Initialize(visibleData, this.Calendar, this.Culture, this.ShowAbbreviatedDayNames, mode);
    element1.To = new double?(0.0);
    element3.To = new double?(1.0);
    element4.To = new double?(0.0);
    this.VisibleData = visibleDate;
    this.m_monthButton1.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
    this.m_monthButton2.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
    this.m_monthButton1.Opacity = 0.01;
    if (direction == CalendarEdit.MoveDirection.Next)
    {
      if (this.MonthChangeDirection == AnimationDirection.Horizontal)
      {
        scopedElement1.X = calendarEditGrid1.ActualWidth;
        element2.To = new double?(-calendarEditGrid1.ActualWidth);
      }
      if (this.MonthChangeDirection == AnimationDirection.Vertical)
      {
        scopedElement1.Y = calendarEditGrid1.ActualHeight;
        element2.To = new double?(-calendarEditGrid1.ActualHeight);
      }
    }
    if (direction == CalendarEdit.MoveDirection.Prev)
    {
      if (this.MonthChangeDirection == AnimationDirection.Horizontal)
      {
        scopedElement1.X = -calendarEditGrid1.ActualWidth;
        element2.To = new double?(calendarEditGrid1.ActualWidth);
      }
      if (this.MonthChangeDirection == AnimationDirection.Vertical)
      {
        scopedElement1.Y = -calendarEditGrid1.ActualHeight;
        element2.To = new double?(calendarEditGrid1.ActualHeight);
      }
    }
    NameScope.SetNameScope((DependencyObject) this, (INameScope) new NameScope());
    calendarEditGrid1.RenderTransform = (Transform) scopedElement2;
    calendarEditGrid2.RenderTransform = (Transform) scopedElement1;
    this.RegisterName("MoveFollowing", (object) scopedElement1);
    this.RegisterName("MoveCurrent", (object) scopedElement2);
    this.RegisterName("MonthButton1", (object) this.m_monthButton1);
    this.RegisterName("MonthButton2", (object) this.m_monthButton2);
    Storyboard.SetTargetName((DependencyObject) element1, "MoveFollowing");
    Storyboard.SetTargetName((DependencyObject) element2, "MoveCurrent");
    Storyboard.SetTargetName((DependencyObject) element3, "MonthButton1");
    Storyboard.SetTargetName((DependencyObject) element4, "MonthButton2");
    if (this.MonthChangeDirection == AnimationDirection.Horizontal)
    {
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) TranslateTransform.XProperty));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) TranslateTransform.XProperty));
    }
    if (this.MonthChangeDirection == AnimationDirection.Vertical)
    {
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) TranslateTransform.YProperty));
      Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) TranslateTransform.YProperty));
    }
    Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
    Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath((object) UIElement.OpacityProperty));
    this.mmoveStoryboard = new Storyboard();
    Timeline.SetDesiredFrameRate((Timeline) this.mmoveStoryboard, new int?(28));
    this.mmoveStoryboard.Children.Add((Timeline) element1);
    this.mmoveStoryboard.Children.Add((Timeline) element2);
    this.mmoveStoryboard.Children.Add((Timeline) element3);
    this.mmoveStoryboard.Children.Add((Timeline) element4);
    this.mmoveStoryboard.Completed += new EventHandler(this.MoveStoryboard_Completed);
    this.mmoveStoryboard.Begin((FrameworkElement) this, true);
  }

  protected virtual void ChangeVisualModePreview(VisibleDate date)
  {
    VisibleDate visibleData = this.VisibleData;
    this.VisibleData = date;
    if (this.VisibleData.Equals((object) visibleData))
      this.OnVisibleDataChanged(new DependencyPropertyChangedEventArgs());
    this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
    this.ChangeVisualModeIndeed();
  }

  protected virtual void ChangeVisualModeIndeed()
  {
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = this.DayNamesGrid.ActualWidth / 24.0;
    double actualWidth1 = this.FindCurrentGrid(CalendarVisualMode.Days).ActualWidth;
    double actualHeight1 = this.FindCurrentGrid(CalendarVisualMode.Days).ActualHeight;
    double actualWidth2 = this.DayNamesGrid.ActualWidth;
    double actualHeight2 = this.DayNamesGrid.ActualHeight;
    ArrayList arrayList = new ArrayList();
    CalendarVisualMode mode1 = this.VisualModeInfo.OldMode;
    CalendarVisualMode mode2 = this.VisualModeInfo.NewMode;
    Cell cell1 = (Cell) null;
    Duration duration = (Duration) TimeSpan.FromMilliseconds((double) this.ChangeModeTime);
    ScaleTransform scopedElement1 = new ScaleTransform();
    ScaleTransform scopedElement2 = new ScaleTransform();
    ScaleTransform scopedElement3 = new ScaleTransform();
    DoubleAnimation element1 = new DoubleAnimation();
    DoubleAnimation element2 = new DoubleAnimation();
    DoubleAnimation element3 = new DoubleAnimation();
    DoubleAnimation element4 = new DoubleAnimation();
    DoubleAnimation element5 = new DoubleAnimation();
    DoubleAnimation element6 = new DoubleAnimation();
    DoubleAnimation element7 = new DoubleAnimation();
    NameScope.SetNameScope((DependencyObject) this, (INameScope) new NameScope());
    element1.Duration = duration;
    element2.Duration = duration;
    element3.Duration = duration;
    element4.Duration = duration;
    element5.Duration = duration;
    element6.Duration = duration;
    element7.Duration = duration;
    element1.FillBehavior = FillBehavior.Stop;
    if (mode1 == CalendarVisualMode.Days && mode2 == CalendarVisualMode.WeekNumbers)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.WeekNumbers).CellsCollection;
    if (mode1 == CalendarVisualMode.WeekNumbers && mode2 == CalendarVisualMode.Months)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
    if (mode1 == CalendarVisualMode.WeekNumbers && mode2 == CalendarVisualMode.Days)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection;
    if (mode1 == CalendarVisualMode.Days && mode2 == CalendarVisualMode.Months)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
    if (mode1 == CalendarVisualMode.Months && mode2 == CalendarVisualMode.Days)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
    if (mode1 == CalendarVisualMode.Months && mode2 == CalendarVisualMode.Years)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Years).CellsCollection;
    if (mode1 == CalendarVisualMode.Years && mode2 == CalendarVisualMode.Months)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.Years).CellsCollection;
    if (mode1 == CalendarVisualMode.Years && mode2 == CalendarVisualMode.YearsRange)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.YearsRange).CellsCollection;
    if (mode1 == CalendarVisualMode.YearsRange && mode2 == CalendarVisualMode.Years)
      arrayList = this.FindCurrentGrid(CalendarVisualMode.YearsRange).CellsCollection;
    if (this.IsNextModeSet && !this.IsFlagValueSet(this.ViewMode) && this.ViewMode == this.NextMode)
      this.IsNextModeSet = false;
    if (this.IsFlagValueSet(this.ViewMode) && mode1 != mode2)
    {
      if (!this.IsNextModeSet && mode1 == this.VisualMode && mode2 == this.NextMode)
      {
        if (mode2 == CalendarVisualMode.Days)
        {
          this.DayNamesGrid.Visibility = Visibility.Visible;
          arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
        }
        else
          arrayList = this.FindCurrentGrid(mode2).CellsCollection;
        mode1 = this.VisualModeInfo.NewMode;
        mode2 = this.VisualMode;
      }
      else if (this.GetPreviousValue(this.VisualMode) == this.VisualMode)
      {
        if (this.VisualMode == CalendarVisualMode.Days)
        {
          this.DayNamesGrid.Visibility = Visibility.Visible;
          arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
        }
        else
          arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
        mode1 = this.VisualMode;
        mode2 = mode1;
      }
      else
      {
        if (this.VisualMode == CalendarVisualMode.Days)
        {
          this.DayNamesGrid.Visibility = Visibility.Visible;
          arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
        }
        else
          arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
        if (this.GetNextValue(mode1) != mode1)
          mode1 = this.GetPreviousValue(this.VisualMode);
      }
    }
    else if (this.ViewMode != CalendarVisualMode.All && this.VisualMode != CalendarVisualMode.All && !this.IsNextModeSet)
    {
      if (this.ViewMode == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.Days)
      {
        this.DayNamesGrid.Visibility = Visibility.Visible;
        arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
      }
      else if (this.ViewMode != CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && this.VisualMode != CalendarVisualMode.Days)
        arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
      mode1 = this.VisualModeInfo.NewMode;
      mode2 = this.VisualMode;
    }
    foreach (Cell cell2 in arrayList)
    {
      if (cell2.IsSelected)
        cell1 = cell2;
    }
    int num4 = cell1 != null ? Grid.GetColumn((UIElement) cell1) : throw new ArgumentNullException("Selected cell can not be null");
    int row = Grid.GetRow((UIElement) cell1);
    switch (num4)
    {
      case 0:
        num1 = 0.0;
        break;
      case 1:
        num1 = (cell1.ActualWidth + cell1.ActualWidth) / 2.0 - num3;
        break;
      case 2:
        num1 = 2.0 * ((cell1.ActualWidth + cell1.ActualWidth) / 2.0 + num3);
        break;
      case 3:
        num1 = 4.0 * cell1.ActualWidth;
        break;
      case 4:
        num1 = 6.0 * cell1.ActualWidth;
        break;
      case 5:
        num1 = 8.0 * cell1.ActualWidth;
        break;
      case 6:
        num1 = 10.0 * cell1.ActualWidth;
        break;
      case 7:
        num1 = 12.0 * cell1.ActualWidth;
        break;
    }
    switch (row)
    {
      case 0:
        num2 = 0.0;
        break;
      case 1:
        num2 = (cell1.ActualHeight + cell1.ActualHeight) / 2.0;
        break;
      case 2:
        num2 = 3.0 * cell1.ActualHeight;
        break;
      case 3:
        num2 = 5.0 * cell1.ActualHeight;
        break;
      case 4:
        num2 = 7.0 * cell1.ActualHeight;
        break;
      case 5:
        num2 = 9.0 * cell1.ActualHeight;
        break;
      case 6:
        num2 = 11.0 * cell1.ActualHeight;
        break;
    }
    scopedElement1.CenterX = num1;
    scopedElement1.CenterY = num2;
    scopedElement2.CenterX = num1;
    scopedElement2.CenterY = num2;
    scopedElement3.CenterX = num1;
    scopedElement3.CenterY = num2;
    if (mode1 == CalendarVisualMode.Days && mode2 == CalendarVisualMode.Months)
    {
      double num5 = cell1.ActualWidth / actualWidth1;
      double num6 = cell1.ActualHeight / (actualHeight1 + actualHeight2);
      scopedElement1.CenterX = num1;
      scopedElement1.CenterY = num2 - 15.0;
      element2.To = new double?(num5);
      element3.To = new double?(num6);
      scopedElement2.ScaleX = 4.0;
      scopedElement2.ScaleY = 3.0;
      element4.To = new double?(1.0);
      element5.To = new double?(1.0);
      double num7 = cell1.ActualWidth / actualWidth2;
      double num8 = cell1.ActualHeight / (actualHeight1 + actualHeight2);
      element6.To = new double?(num7);
      element7.To = new double?(num8);
      cell1.Opacity = 0.0;
      element1.To = new double?(1.0);
      this.FindCurrentGrid(CalendarVisualMode.Months).RenderTransform = (Transform) scopedElement2;
      this.FindCurrentGrid(CalendarVisualMode.Days).RenderTransform = (Transform) scopedElement1;
      this.DayNamesGrid.RenderTransform = (Transform) scopedElement3;
    }
    if (mode1 == CalendarVisualMode.Months && mode2 == CalendarVisualMode.Days || this.ViewMode == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.Days && mode1 != CalendarVisualMode.WeekNumbers)
    {
      double num9 = cell1.ActualWidth / actualWidth1;
      double num10 = cell1.ActualHeight / (actualHeight1 + actualHeight2);
      scopedElement1.ScaleX = num9;
      scopedElement1.ScaleY = num10;
      scopedElement1.CenterX = num1;
      scopedElement1.CenterY = num2 - 15.0;
      element2.To = new double?(1.0);
      element3.To = new double?(1.0);
      scopedElement2.ScaleX = 1.0;
      scopedElement2.ScaleY = 1.0;
      element4.To = new double?(4.0);
      element5.To = new double?(3.0);
      double num11 = cell1.ActualWidth / actualWidth2;
      double num12 = cell1.ActualHeight / (actualHeight1 + actualHeight2);
      scopedElement3.ScaleX = num11;
      scopedElement3.ScaleY = num12;
      element6.To = new double?(1.0);
      element7.To = new double?(1.0);
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.Months).RenderTransform = (Transform) scopedElement2;
      this.FindCurrentGrid(CalendarVisualMode.Days).RenderTransform = (Transform) scopedElement1;
      this.DayNamesGrid.RenderTransform = (Transform) scopedElement3;
      this.DayNamesGrid.Visibility = Visibility.Visible;
    }
    if (mode1 == CalendarVisualMode.Months && mode2 == CalendarVisualMode.Years)
    {
      double num13 = cell1.ActualWidth / this.CurrentMonthGrid.ActualWidth;
      double num14 = cell1.ActualHeight / this.CurrentMonthGrid.ActualHeight;
      element2.To = new double?(num13);
      element3.To = new double?(num14);
      scopedElement2.ScaleX = 4.0;
      scopedElement2.ScaleY = 3.0;
      element4.To = new double?(1.0);
      element5.To = new double?(1.0);
      cell1.Opacity = 0.0;
      element1.To = new double?(1.0);
      this.FindCurrentGrid(CalendarVisualMode.Months).RenderTransform = (Transform) scopedElement1;
      this.FindCurrentGrid(CalendarVisualMode.Years).RenderTransform = (Transform) scopedElement2;
      if (this.DisableDateSelection)
      {
        this.CurrentDayGrid.Visibility = Visibility.Hidden;
        this.DayNamesGrid.Visibility = Visibility.Hidden;
      }
      if (this.DayNamesGrid.Visibility == Visibility.Visible)
        this.DayNamesGrid.Visibility = Visibility.Hidden;
    }
    if (mode1 == CalendarVisualMode.Years && mode2 == CalendarVisualMode.Months || this.ViewMode == CalendarVisualMode.Months || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.Months && mode1 != CalendarVisualMode.WeekNumbers)
    {
      double num15 = cell1.ActualWidth / this.CurrentMonthGrid.ActualWidth;
      double num16 = cell1.ActualHeight / this.CurrentMonthGrid.ActualHeight;
      element2.To = new double?(4.0);
      element3.To = new double?(3.0);
      scopedElement2.ScaleX = num15;
      scopedElement2.ScaleY = num16;
      element4.To = new double?(1.0);
      element5.To = new double?(1.0);
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.Years).RenderTransform = (Transform) scopedElement1;
      this.FindCurrentGrid(CalendarVisualMode.Months).RenderTransform = (Transform) scopedElement2;
    }
    if (mode1 == CalendarVisualMode.Years && mode2 == CalendarVisualMode.YearsRange || this.ViewMode == CalendarVisualMode.YearsRange || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.YearsRange && mode1 != CalendarVisualMode.WeekNumbers)
    {
      double num17 = cell1.ActualWidth / this.CurrentYearGrid.ActualWidth;
      double num18 = cell1.ActualHeight / this.CurrentYearGrid.ActualHeight;
      element2.To = new double?(num17);
      element3.To = new double?(num18);
      scopedElement2.ScaleX = 4.0;
      scopedElement2.ScaleY = 3.0;
      element4.To = new double?(1.0);
      element5.To = new double?(1.0);
      cell1.Opacity = 0.0;
      element1.To = new double?(1.0);
      this.FindCurrentGrid(CalendarVisualMode.Years).RenderTransform = (Transform) scopedElement1;
      this.FindCurrentGrid(CalendarVisualMode.YearsRange).RenderTransform = (Transform) scopedElement2;
    }
    if (mode1 == CalendarVisualMode.YearsRange && mode2 == CalendarVisualMode.Years || this.ViewMode == CalendarVisualMode.Years || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.Years && mode1 != CalendarVisualMode.WeekNumbers)
    {
      double num19 = cell1.ActualWidth / this.CurrentYearGrid.ActualWidth;
      double num20 = cell1.ActualHeight / this.CurrentYearGrid.ActualHeight;
      element2.To = new double?(4.0);
      element3.To = new double?(3.0);
      scopedElement2.ScaleX = num19;
      scopedElement2.ScaleY = num20;
      element4.To = new double?(1.0);
      element5.To = new double?(1.0);
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.YearsRange).RenderTransform = (Transform) scopedElement1;
      this.FindCurrentGrid(CalendarVisualMode.Years).RenderTransform = (Transform) scopedElement2;
    }
    if (mode1 == CalendarVisualMode.Days && mode2 == CalendarVisualMode.WeekNumbers)
    {
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.Days).RenderTransform = (Transform) scopedElement2;
      this.FindCurrentGrid(CalendarVisualMode.WeekNumbers).RenderTransform = (Transform) scopedElement1;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentWeekNumbersGrid.Visibility = Visibility.Visible;
      this.UpdateWeekNumbersContainer();
    }
    if (mode1 == CalendarVisualMode.WeekNumbers && mode2 == CalendarVisualMode.Months)
    {
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.WeekNumbers).RenderTransform = (Transform) scopedElement2;
      this.FindCurrentGrid(CalendarVisualMode.Months).RenderTransform = (Transform) scopedElement1;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
    }
    if (mode1 == CalendarVisualMode.WeekNumbers && mode2 == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && mode1 == CalendarVisualMode.WeekNumbers)
    {
      cell1.Opacity = 1.0;
      element1.To = new double?(0.0);
      this.FindCurrentGrid(CalendarVisualMode.WeekNumbers).RenderTransform = (Transform) scopedElement2;
      this.FindCurrentGrid(mode2).RenderTransform = (Transform) scopedElement1;
    }
    this.RegisterName("Mode1Scale", (object) scopedElement1);
    this.RegisterName("Mode2Scale", (object) scopedElement2);
    this.RegisterName("SelecteCell", (object) cell1);
    Storyboard.SetTargetName((DependencyObject) element2, "Mode1Scale");
    Storyboard.SetTargetName((DependencyObject) element3, "Mode1Scale");
    Storyboard.SetTargetName((DependencyObject) element4, "Mode2Scale");
    Storyboard.SetTargetName((DependencyObject) element5, "Mode2Scale");
    Storyboard.SetTargetName((DependencyObject) element1, "SelecteCell");
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) ScaleTransform.ScaleXProperty));
    Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) ScaleTransform.ScaleYProperty));
    Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath((object) ScaleTransform.ScaleXProperty));
    Storyboard.SetTargetProperty((DependencyObject) element5, new PropertyPath((object) ScaleTransform.ScaleYProperty));
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) UIElement.OpacityProperty));
    this.mvisualModeStoryboard = new Storyboard();
    Timeline.SetDesiredFrameRate((Timeline) this.mvisualModeStoryboard, new int?(28));
    this.mvisualModeStoryboard.Children.Add((Timeline) element2);
    this.mvisualModeStoryboard.Children.Add((Timeline) element3);
    this.mvisualModeStoryboard.Children.Add((Timeline) element4);
    this.mvisualModeStoryboard.Children.Add((Timeline) element5);
    this.mvisualModeStoryboard.Children.Add((Timeline) element1);
    if (mode2 == CalendarVisualMode.Days)
    {
      this.RegisterName("DayNameScale", (object) scopedElement3);
      Storyboard.SetTargetName((DependencyObject) element6, "DayNameScale");
      Storyboard.SetTargetName((DependencyObject) element7, "DayNameScale");
      Storyboard.SetTargetProperty((DependencyObject) element6, new PropertyPath((object) ScaleTransform.ScaleXProperty));
      Storyboard.SetTargetProperty((DependencyObject) element7, new PropertyPath((object) ScaleTransform.ScaleYProperty));
      this.mvisualModeStoryboard.Children.Add((Timeline) element6);
      this.mvisualModeStoryboard.Children.Add((Timeline) element7);
    }
    this.mvisualModeStoryboard.Completed += new EventHandler(this.VisualModeStoryboard_OnCompleted);
    this.mvisualModeStoryboard.Begin((FrameworkElement) this, true);
  }

  protected virtual void ChangeMode(CalendarEdit.ChangeVisualModeDirection direction)
  {
    if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
    {
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(this.VisualMode, this.NextMode);
      this.VisualMode = this.NextMode;
    }
    if (this.VisualModeInfo.OldMode == this.VisualModeInfo.NewMode)
      return;
    this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    this.m_monthButton2.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    CalendarEditGrid currentGrid = this.FindCurrentGrid(this.VisualMode);
    currentGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    if (this.VisualMode != CalendarVisualMode.All)
      currentGrid.Visibility = Visibility.Visible;
    currentGrid.Focus();
    this.ChangeVisualModeIndeed();
    this.UpdateWeekNumbersContainer();
  }

  protected void ClearSelectedCell()
  {
    ArrayList cellsCollection = this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection;
    for (int index = 0; index < cellsCollection.Count; ++index)
    {
      DayCell dayCell = (DayCell) cellsCollection[index];
      dayCell.IsDate = false;
      dayCell.IsSelected = false;
    }
    if (this.SelectedDates.Count == 0)
      return;
    this.SelectedDates.Clear();
  }

  protected virtual void ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection direction)
  {
    CalendarVisualMode visualMode = this.VisualMode;
    this.HideWeekNumbersForYearContainer();
    if (visualMode == CalendarVisualMode.Days & this.IsWeekCellClicked)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualMode = CalendarVisualMode.WeekNumbers;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.WeekNumbers);
        this.ShowWeekNumbersForYearContainer();
      }
      else
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.WeekNumbers, CalendarVisualMode.WeekNumbers);
    }
    if (visualMode == CalendarVisualMode.Days || visualMode == CalendarVisualMode.All)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualMode = CalendarVisualMode.Months;
        if (this.VisualMode != CalendarVisualMode.Days && this.GetTemplateChild("rect") is Rectangle templateChild)
          templateChild.Visibility = Visibility.Collapsed;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.Months);
      }
      else
      {
        if (this.VisualMode != CalendarVisualMode.Days && this.GetTemplateChild("rect") is Rectangle templateChild)
          templateChild.Visibility = Visibility.Collapsed;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.Days);
      }
    }
    if (visualMode == CalendarVisualMode.Months)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualMode = CalendarVisualMode.Years;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Years);
      }
      else
      {
        this.VisualMode = CalendarVisualMode.Days;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days);
      }
    }
    if (visualMode == CalendarVisualMode.Years)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualMode = CalendarVisualMode.YearsRange;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, CalendarVisualMode.YearsRange);
      }
      else
      {
        this.VisualMode = CalendarVisualMode.Months;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, CalendarVisualMode.Months);
      }
    }
    if (visualMode == CalendarVisualMode.YearsRange)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.YearsRange, CalendarVisualMode.YearsRange);
      }
      else
      {
        this.VisualMode = CalendarVisualMode.Years;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.YearsRange, CalendarVisualMode.Years);
      }
    }
    if (visualMode == CalendarVisualMode.WeekNumbers)
    {
      if (direction == CalendarEdit.ChangeVisualModeDirection.Up)
      {
        this.VisualMode = CalendarVisualMode.Months;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.WeekNumbers, CalendarVisualMode.Months);
      }
      else
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.WeekNumbers, CalendarVisualMode.Days);
    }
    if (this.VisualModeInfo.OldMode == this.VisualModeInfo.NewMode)
      return;
    this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    this.m_monthButton2.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    CalendarEditGrid currentGrid = this.FindCurrentGrid(this.VisualMode);
    currentGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    currentGrid.Visibility = Visibility.Visible;
    currentGrid.Focus();
    this.ChangeVisualModeIndeed();
    this.UpdateWeekNumbersContainer();
  }

  private void HideWeekNumbersContainer()
  {
    this.m_weekNumbersContainer.Visibility = Visibility.Collapsed;
    this.m_mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;
  }

  private void ShowWeekNumbersContainer()
  {
    this.m_weekNumbersContainer.Visibility = Visibility.Visible;
    if (this.VisualMode == CalendarVisualMode.WeekNumbers)
    {
      this.m_mainGrid.ColumnDefinitions[1].Width = new GridLength(1.0, GridUnitType.Auto);
      this.m_mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Auto);
    }
    else
    {
      this.m_mainGrid.ColumnDefinitions[1].Width = new GridLength(1.0, GridUnitType.Star);
      this.m_mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
    }
  }

  private void ShowWeekNumbersForYearContainer()
  {
    this.wcurrentweekNumbersContainer.Visibility = Visibility.Visible;
    this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
  }

  private void HideWeekNumbersForYearContainer()
  {
    this.wcurrentweekNumbersContainer.Visibility = Visibility.Hidden;
    this.wfollowweekNumbersContainer.Visibility = Visibility.Hidden;
  }

  protected virtual void NavigateButtonVerify()
  {
    DateTime dateTime = new DateTime();
    Syncfusion.Windows.Shared.Date date1 = new Syncfusion.Windows.Shared.Date();
    Syncfusion.Windows.Shared.Date date2 = new Syncfusion.Windows.Shared.Date();
    if (this.Calendar == null)
      return;
    Syncfusion.Windows.Shared.Date date3;
    Syncfusion.Windows.Shared.Date date4;
    if (this.MinMaxHidden)
    {
      date3 = new Syncfusion.Windows.Shared.Date(this.MinDate, this.Calendar);
      if (this.MaxDate > this.Calendar.MaxSupportedDateTime || dateTime == this.MaxDate)
        this.MaxDate = this.Calendar.MaxSupportedDateTime;
      date4 = new Syncfusion.Windows.Shared.Date(this.MaxDate, this.Calendar);
    }
    else
    {
      date3 = new Syncfusion.Windows.Shared.Date(this.miDate, this.Calendar);
      date4 = new Syncfusion.Windows.Shared.Date(this.mxDate, this.Calendar);
    }
    Syncfusion.Windows.Shared.Date date5 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1);
    Cell cell1 = (Cell) null;
    Cell cell2 = (Cell) null;
    CalendarVisualMode mode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      mode = this.ViewMode;
    ArrayList arrayList = new ArrayList();
    if (this.FindCurrentGrid(mode) != null)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.WeekNumbers)
    {
      date1 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear + 1, this.VisibleData.VisibleMonth, 1);
      date2 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear - 1, this.VisibleData.VisibleMonth, 1);
    }
    if (mode == CalendarVisualMode.Days || mode == CalendarVisualMode.All)
    {
      date1 = date5.AddMonthToDate(1);
      date2 = date5.AddMonthToDate(-1);
    }
    else
    {
      switch (mode)
      {
        case CalendarVisualMode.Months:
          date1 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear + 1, this.VisibleData.VisibleMonth, 1);
          date2 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear - 1, this.VisibleData.VisibleMonth, 1);
          break;
        case CalendarVisualMode.Years:
          cell1 = (Cell) (arrayList[arrayList.Count - 1] as YearCell);
          cell2 = (Cell) (arrayList[0] as YearCell);
          int year1 = (cell1 as YearCell).Year;
          int year2 = (cell2 as YearCell).Year;
          date1 = new Syncfusion.Windows.Shared.Date(year1 + 1, this.VisibleData.VisibleMonth, 1);
          date2 = new Syncfusion.Windows.Shared.Date(year2 - 1, this.VisibleData.VisibleMonth, 1);
          break;
        case CalendarVisualMode.YearsRange:
          cell1 = (Cell) (arrayList[arrayList.Count - 1] as YearRangeCell);
          cell2 = (Cell) (arrayList[0] as YearRangeCell);
          int startYear1 = (cell1 as YearRangeCell).Years.StartYear;
          int startYear2 = (cell2 as YearRangeCell).Years.StartYear;
          date1 = new Syncfusion.Windows.Shared.Date(startYear1 + 10, this.VisibleData.VisibleMonth, 1);
          date2 = new Syncfusion.Windows.Shared.Date(startYear2 - 10, this.VisibleData.VisibleMonth, 1);
          break;
      }
    }
    if (this.m_nextButton == null || this.m_prevButton == null)
      return;
    if (mode != CalendarVisualMode.Days && mode != CalendarVisualMode.All && mode != CalendarVisualMode.Months && mode != CalendarVisualMode.WeekNumbers)
    {
      if (date1 > date4 || cell1.Visibility != Visibility.Visible)
        this.m_nextButton.IsEnabled = false;
      else
        this.m_nextButton.IsEnabled = true;
      if (date2 < date3 || cell2.Visibility != Visibility.Visible)
        this.m_prevButton.IsEnabled = false;
      else
        this.m_prevButton.IsEnabled = true;
    }
    else
    {
      if (date1 > date4)
        this.m_nextButton.IsEnabled = false;
      else
        this.m_nextButton.IsEnabled = true;
      if (date2 < date3)
        this.m_prevButton.IsEnabled = false;
      else
        this.m_prevButton.IsEnabled = true;
    }
  }

  protected internal void SetBackground(
    DayGrid dayGrid,
    SolidColorBrush currentBrush,
    SolidColorBrush followingBrush,
    bool isCurrent)
  {
    foreach (DayCell cells in this.CurrentDayGrid.CellsCollection)
    {
      if (isCurrent)
      {
        if (cells.IsCurrentMonth)
          cells.Background = (Brush) currentBrush;
        else
          cells.Background = (Brush) followingBrush;
      }
      else if (cells.IsCurrentMonth)
        cells.Background = (Brush) followingBrush;
      else
        cells.Background = (Brush) currentBrush;
    }
  }

  protected virtual double CalculateLayOnValue(DayGrid current)
  {
    int num = 0;
    int count = current.CellsCollection.Count;
    double layOnValue = ((FrameworkElement) current.CellsCollection[0]).ActualHeight;
    for (int index = count / 2; index < count; ++index)
    {
      if (!((DayCell) current.CellsCollection[index]).IsCurrentMonth)
        ++num;
    }
    if (num > 7)
      layOnValue = 2.0 * layOnValue;
    return layOnValue;
  }

  protected virtual void Highlight(Border border, CalendarEdit.HighlightSate state)
  {
    Duration duration = new Duration(TimeSpan.FromMilliseconds(600.0));
    DoubleAnimation element1 = new DoubleAnimation();
    ThicknessAnimation element2 = new ThicknessAnimation();
    if (state == CalendarEdit.HighlightSate.Begin)
    {
      element1.To = new double?(1.0);
      element2.To = new Thickness?(new Thickness(1.0));
    }
    else
    {
      element1.To = new double?(0.0);
      element2.To = new Thickness?(new Thickness(0.0));
    }
    element1.Duration = duration;
    element2.Duration = duration;
    NameScope.SetNameScope((DependencyObject) this, (INameScope) new NameScope());
    this.RegisterName("SelectionBorder", (object) border);
    Storyboard storyboard = new Storyboard();
    Timeline.SetDesiredFrameRate((Timeline) storyboard, new int?(30));
    Storyboard.SetTargetName((DependencyObject) element1, "SelectionBorder");
    Storyboard.SetTargetName((DependencyObject) element2, "SelectionBorder");
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) UIElement.OpacityProperty));
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) Border.BorderThicknessProperty));
    storyboard.Children.Add((Timeline) element1);
    storyboard.Children.Add((Timeline) element2);
    storyboard.Begin((FrameworkElement) border);
  }

  protected virtual void SetDateDataTemplates(
    DayGrid current,
    CalendarEdit.CollectionChangedAction action,
    DataTemplateItem item)
  {
    DataTemplate cellsDataTemplate = this.DayCellsDataTemplate;
    DataTemplateSelector templateSelector = this.DayCellsDataTemplateSelector;
    if (current == null)
      throw new ArgumentNullException(nameof (current));
    if (item == null && action != CalendarEdit.CollectionChangedAction.Reset)
      throw new ArgumentNullException(nameof (item));
    if (action == CalendarEdit.CollectionChangedAction.Reset)
    {
      current.UpdateTemplateAndSelector(cellsDataTemplate, templateSelector, (DataTemplatesDictionary) null);
    }
    else
    {
      Hashtable dateCells = current.DateCells;
      if (!dateCells.ContainsKey((object) item.Date))
        return;
      DayCell dayCell = (DayCell) dateCells[(object) item.Date];
      if (action == CalendarEdit.CollectionChangedAction.Add)
        dayCell.SetTemplate(item.Template);
      if (action != CalendarEdit.CollectionChangedAction.Remove)
        return;
      dayCell.UpdateCellTemplateAndSelector(cellsDataTemplate, templateSelector);
    }
  }

  protected virtual void SetDateStyles(
    DayGrid current,
    CalendarEdit.CollectionChangedAction action,
    StyleItem item)
  {
    if (current == null)
      throw new ArgumentNullException(nameof (current));
    if (item == null && action != CalendarEdit.CollectionChangedAction.Reset)
      throw new ArgumentNullException(nameof (item));
    if (action == CalendarEdit.CollectionChangedAction.Reset)
    {
      current.UpdateStyles(this.DayCellsStyle, (StylesDictionary) null);
    }
    else
    {
      Hashtable dateCells = current.DateCells;
      if (!dateCells.ContainsKey((object) item.Date))
        return;
      DayCell dayCell = (DayCell) dateCells[(object) item.Date];
      if (action == CalendarEdit.CollectionChangedAction.Add)
        dayCell.SetStyle(item.Style);
      if (action != CalendarEdit.CollectionChangedAction.Remove)
        return;
      dayCell.SetStyle(this.DayCellsStyle);
    }
  }

  protected internal virtual void InitilizeDayCellTemplates(DayGrid current)
  {
    current.UpdateTemplateAndSelector(this.DayCellsDataTemplate, this.DayCellsDataTemplateSelector, this.DateDataTemplates);
  }

  protected internal virtual void InitilizeDayCellStyles(DayGrid current)
  {
    current.UpdateStyles(this.DayCellsStyle, this.DateStyles);
  }

  private static bool ValidateFrameMovingTime(object value) => 0 <= (int) value;

  private void SetCellToolTip(int rowIndex, int colIndex, DayGrid current, System.Windows.Controls.ToolTip tooltip)
  {
    ArrayList cellsCollection = current.CellsCollection;
    int index = 0;
    for (int count = cellsCollection.Count; index < count; ++index)
    {
      int row = Grid.GetRow((UIElement) cellsCollection[index]);
      int column = Grid.GetColumn((UIElement) cellsCollection[index]);
      if (row == rowIndex && column == colIndex)
        (current.CellsCollection[index] as DayCell).ToolTip = (object) tooltip;
    }
  }

  private bool IsOutOfDateRange(int month)
  {
    if (this.VisualMode != CalendarVisualMode.Days && this.VisualMode != CalendarVisualMode.All)
      throw new NotSupportedException("This function should be called in Days mode only.");
    Syncfusion.Windows.Shared.Date date1;
    Syncfusion.Windows.Shared.Date date2;
    if (this.MinMaxHidden)
    {
      date1 = new Syncfusion.Windows.Shared.Date(this.MinDate, this.Calendar);
      date2 = new Syncfusion.Windows.Shared.Date(this.MaxDate, this.Calendar);
    }
    else
    {
      date1 = new Syncfusion.Windows.Shared.Date(this.miDate, this.Calendar);
      date2 = new Syncfusion.Windows.Shared.Date(this.mxDate, this.Calendar);
    }
    Syncfusion.Windows.Shared.Date date3 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, month > 0 ? 1 : 31 /*0x1F*/);
    Syncfusion.Windows.Shared.Date date4 = new Syncfusion.Windows.Shared.Date();
    Syncfusion.Windows.Shared.Date date5 = date3.AddMonthToDate(month);
    return date5 > date2 || date5 < date1;
  }

  private bool IsOutOfDateRange(CalendarEdit.MoveDirection direction, ref Syncfusion.Windows.Shared.Date returnDate)
  {
    CalendarVisualMode mode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      mode = this.ViewMode;
    if (mode == CalendarVisualMode.Days)
      throw new NotSupportedException("This function should not be called in Days mode.");
    int year = 0;
    int num = 0;
    Syncfusion.Windows.Shared.Date date1;
    Syncfusion.Windows.Shared.Date date2;
    if (this.MinMaxHidden)
    {
      date1 = new Syncfusion.Windows.Shared.Date(this.MinDate, this.Calendar);
      date2 = new Syncfusion.Windows.Shared.Date(this.MaxDate, this.Calendar);
    }
    else
    {
      date1 = new Syncfusion.Windows.Shared.Date(this.miDate, this.Calendar);
      date2 = new Syncfusion.Windows.Shared.Date(this.mxDate, this.Calendar);
    }
    Syncfusion.Windows.Shared.Date date3 = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1);
    Syncfusion.Windows.Shared.Date date4 = new Syncfusion.Windows.Shared.Date();
    Syncfusion.Windows.Shared.Date date5 = new Syncfusion.Windows.Shared.Date();
    ArrayList cellsCollection = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.WeekNumbers)
      year = direction != CalendarEdit.MoveDirection.Next ? date3.Year - 1 : date3.Year + 1;
    if (mode == CalendarVisualMode.Months)
    {
      year = direction != CalendarEdit.MoveDirection.Next ? date3.Year - 1 : date3.Year + 1;
      date5 = date4;
    }
    if (mode == CalendarVisualMode.Years)
    {
      Cell cell;
      if (direction == CalendarEdit.MoveDirection.Next)
      {
        num = 1;
        cell = (Cell) (cellsCollection[cellsCollection.Count - 1] as YearCell);
        year = (cell as YearCell).Year;
      }
      else
      {
        num = -1;
        cell = (Cell) (cellsCollection[0] as YearCell);
        year = (cell as YearCell).Year;
      }
      if (cell.Visibility != Visibility.Visible)
        return true;
    }
    if (mode == CalendarVisualMode.YearsRange)
    {
      Cell cell;
      if (direction == CalendarEdit.MoveDirection.Next)
      {
        num = 10;
        cell = (Cell) (cellsCollection[cellsCollection.Count - 1] as YearRangeCell);
        year = (cell as YearRangeCell).Years.StartYear;
      }
      else
      {
        num = -10;
        cell = (Cell) (cellsCollection[0] as YearRangeCell);
        year = (cell as YearRangeCell).Years.StartYear;
      }
      if (cell.Visibility != Visibility.Visible)
        return true;
    }
    date4 = new Syncfusion.Windows.Shared.Date(year, date3.Month, 1);
    date5 = new Syncfusion.Windows.Shared.Date(year + num, date3.Month, 1);
    returnDate = date4;
    return date5 > date2 || date5 < date1;
  }

  private void AddMonth(int month)
  {
    VisibleDate visibleData = this.VisibleData;
    int num1 = Math.Abs(month);
    int num2 = 0;
    int num3;
    if (num1 >= 12)
    {
      num2 = month / 12;
      num3 = month % 12;
    }
    else
      num3 = month;
    int num4 = this.VisibleData.VisibleMonth + num3;
    visibleData.VisibleYear += num2;
    if (num4 > 12)
    {
      visibleData.VisibleYear += num4 / 12;
      visibleData.VisibleMonth = num4 % 12;
    }
    if (num4 < 1)
    {
      visibleData.VisibleMonth = 12 + num4;
      --visibleData.VisibleYear;
    }
    if (num4 >= 1 && num4 <= 12)
      visibleData.VisibleMonth = num4;
    this.VisibleData = visibleData;
  }

  private void ScrollMonth(int month)
  {
    int num = DateUtils.AddMonth(this.VisibleData.VisibleMonth, 1);
    if (this.IsStoryboardActive(this.mmonthStoryboard))
      return;
    if (month == num)
      this.BeginMoving(CalendarEdit.MoveDirection.Next, 1);
    else
      this.BeginMoving(CalendarEdit.MoveDirection.Prev, -1);
  }

  private void AddYear(int year)
  {
    VisibleDate visibleData = this.VisibleData;
    visibleData.VisibleYear += year;
    this.VisibleData = visibleData;
  }

  private int CalculateMonthDelta(DateTime startDate, DateTime endDate)
  {
    int monthDelta = 0;
    while (startDate != endDate)
    {
      if (startDate < endDate)
      {
        ++monthDelta;
        startDate = startDate.AddMonths(1);
      }
      else
      {
        --monthDelta;
        startDate = startDate.AddMonths(-1);
      }
    }
    return monthDelta;
  }

  private void MultiplySelect(DateTime date, ModifierKeys modifiers)
  {
    if (this.SelectedDates == null)
      return;
    if (modifiers == ModifierKeys.Control)
    {
      if (this.isControlSelection)
      {
        if (this.SelectedDates.Contains(date))
        {
          this.SelectedDates.Remove(date);
          this.m_dateSetManual = true;
          this.Date = date;
        }
        else
        {
          this.SelectedDates.Add(date);
          this.m_dateSetManual = true;
          this.Date = date;
        }
        this.isControlSelection = false;
      }
      else if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.Right))
        this.ProcessSelection(date);
    }
    else
    {
      DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
      DateTime date1 = this.m_shiftDate;
      DateTime dateTime = date;
      this.LockSelectedDatesUpdate();
      if (modifiers == ModifierKeys.Shift)
        this.SelectedDates.Clear();
      if (date1 <= dateTime)
      {
        for (; date1 <= dateTime; date1 = date1.AddDays(1.0))
        {
          Syncfusion.Windows.Shared.Date date2 = new Syncfusion.Windows.Shared.Date(date1, this.Calendar);
          if (this.InvalidDates == null || this.InvalidDates.Count == 0 || !this.InvalidDates.Contains(date2))
            this.SelectedDates.Add(date1);
        }
      }
      else
      {
        for (; date1 >= dateTime; date1 = date1.AddDays(-1.0))
        {
          Syncfusion.Windows.Shared.Date date3 = new Syncfusion.Windows.Shared.Date(date1, this.Calendar);
          if (this.InvalidDates == null || this.InvalidDates.Count == 0 || !this.InvalidDates.Contains(date3))
            this.SelectedDates.Add(date1);
        }
      }
      this.m_dateSetManual = true;
      this.Date = date;
      this.UnLockSelectedDatesUpdate();
      this.SelectedDatesList.Clear();
    }
    this.ProcessSelectedDatesCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList) this.SelectedDates));
  }

  private bool IsStoryboardActive(Storyboard sb)
  {
    return sb != null && sb.GetCurrentState((FrameworkElement) this) == ClockState.Active;
  }

  private int GetFocusCellIndex()
  {
    DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
    int focusCellIndex = 0;
    for (int index = 0; index < currentGrid.CellsCollection.Count; ++index)
    {
      if (((UIElement) currentGrid.CellsCollection[index]).IsFocused)
      {
        focusCellIndex = index;
        break;
      }
    }
    return focusCellIndex;
  }

  private void SetKeyboardfocus(
    int index,
    ArrayList cellsCollection,
    ModifierKeys modifierKeys,
    Key key,
    DayGrid currentDayGrid)
  {
    DayCell dayCell = new DayCell();
    if (index >= 0 && index < cellsCollection.Count)
      dayCell = (DayCell) cellsCollection[index];
    if (dayCell.Visibility != Visibility.Visible || !(dayCell.Date.ToDateTime(this.Calendar) >= this.MinDate) || !(dayCell.Date.ToDateTime(this.Calendar) <= this.MaxDate))
      return;
    if (index >= 0 && index < cellsCollection.Count && dayCell.IsCurrentMonth)
    {
      if (dayCell.IsToday && !dayCell.Focusable)
        dayCell.Focusable = true;
      dayCell.Focus();
    }
    else
    {
      Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date();
      if (index >= 0 && index < cellsCollection.Count)
      {
        date = dayCell.Date;
      }
      else
      {
        switch (key)
        {
          case Key.Up:
            date = (cellsCollection[index + currentDayGrid.ColumnsCount] as DayCell).Date;
            break;
          case Key.Down:
            date = (cellsCollection[index - currentDayGrid.ColumnsCount] as DayCell).Date;
            break;
        }
      }
      if (key == Key.Right || key == Key.Down)
        this.BeginMoving(CalendarEdit.MoveDirection.Next, 1);
      else if (key == Key.Left || key == Key.Up)
        this.BeginMoving(CalendarEdit.MoveDirection.Prev, -1);
      currentDayGrid = this.FollowingDayGrid;
      for (int index1 = 0; index1 < currentDayGrid.CellsCollection.Count; ++index1)
      {
        DayCell cells = (DayCell) currentDayGrid.CellsCollection[index1];
        if (index >= 0 && index < currentDayGrid.CellsCollection.Count)
        {
          if (cells.Date == date && cells.IsCurrentMonth)
          {
            (currentDayGrid.CellsCollection[index1] as DayCell).Focus();
            break;
          }
        }
        else if (cells.Date == date)
        {
          if (index < 0)
          {
            (currentDayGrid.CellsCollection[index1 - currentDayGrid.ColumnsCount] as DayCell).Focus();
            break;
          }
          if (index >= this.CurrentDayGrid.CellsCollection.Count)
          {
            (currentDayGrid.CellsCollection[index1 + currentDayGrid.ColumnsCount] as DayCell).Focus();
            break;
          }
        }
      }
    }
  }

  private int ValidateFocusIndex()
  {
    int num = 0;
    DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
    bool flag = false;
    for (int index = 0; index < currentGrid.CellsCollection.Count; ++index)
    {
      DayCell cells = (DayCell) currentGrid.CellsCollection[index];
      if (cells.IsDate && cells.IsFocused)
      {
        flag = true;
        num = index;
        break;
      }
      if (cells.IsFocused)
      {
        flag = true;
        num = index;
        break;
      }
    }
    if (!flag)
    {
      this.VisibleData = this.VisibleData with
      {
        VisibleMonth = this.Calendar.GetMonth(this.Date),
        VisibleYear = this.Calendar.GetYear(this.Date)
      };
      for (int index = 0; index < currentGrid.CellsCollection.Count; ++index)
      {
        if (((DayCell) currentGrid.CellsCollection[index]).IsDate)
          num = index;
      }
    }
    return num;
  }

  private int SetFocusedCellIndex(
    int oldValue,
    int direction,
    Orientation orientation,
    out bool moved)
  {
    if (direction != 1 && direction != -1)
      throw new ArgumentException("admissible values are: 1 and -1");
    int index1 = oldValue;
    DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
    if (orientation == Orientation.Horizontal)
      index1 += direction;
    moved = false;
    if (index1 < 0 || index1 >= this.CurrentDayGrid.CellsCollection.Count || index1 >= 0 && index1 < currentGrid.CellsCollection.Count && !(currentGrid.CellsCollection[index1] as DayCell).IsCurrentMonth)
    {
      Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date();
      if (index1 >= 0 && index1 < currentGrid.CellsCollection.Count)
        date = (currentGrid.CellsCollection[index1] as DayCell).Date;
      if (direction == -1)
        this.BeginMoving(CalendarEdit.MoveDirection.Prev, direction);
      else
        this.BeginMoving(CalendarEdit.MoveDirection.Next, direction);
      moved = true;
      DayGrid followingDayGrid = this.FollowingDayGrid;
      for (int index2 = 0; index2 < followingDayGrid.CellsCollection.Count; ++index2)
      {
        DayCell cells = (DayCell) followingDayGrid.CellsCollection[index2];
        if (index1 >= 0 && index1 < followingDayGrid.CellsCollection.Count)
        {
          if (cells.Date == date && cells.IsCurrentMonth)
          {
            index1 = index2;
            break;
          }
        }
        else if (cells.Date.ToDateTime(this.Calendar) == this.Date)
        {
          if (index1 < 0)
          {
            index1 = index2 - this.CurrentDayGrid.ColumnsCount;
            break;
          }
          if (index1 >= this.CurrentDayGrid.CellsCollection.Count)
          {
            index1 = index2 + this.CurrentDayGrid.ColumnsCount;
            break;
          }
        }
      }
    }
    return index1;
  }

  private void CoerceVisibleData(System.Globalization.Calendar calendar)
  {
    this.SetCorrectDate();
    VisibleDate visibleData = this.VisibleData with
    {
      VisibleMonth = calendar.GetMonth(this.Date),
      VisibleYear = calendar.GetYear(this.Date)
    };
    this.UpdateSelectedDatesList();
    if (this.VisibleData.Equals((object) visibleData))
      this.OnVisibleDataChanged(new DependencyPropertyChangedEventArgs());
    this.VisibleData = visibleData;
  }

  private void SetCorrectDate()
  {
    if (this.MinMaxHidden)
    {
      if (this.Date > this.MaxDate)
        this.Date = this.MaxDate;
      if (!(this.Date < this.MinDate))
        return;
      this.Date = this.MinDate;
    }
    else
    {
      if (this.Date > this.mxDate)
        this.Date = this.mxDate;
      if (!(this.Date < this.miDate))
        return;
      this.Date = this.miDate;
    }
  }

  public virtual void OnDatesCollectionChanged(DependencyPropertyChangedEventArgs e)
  {
    INotifyCollectionChanged newValue = (INotifyCollectionChanged) e.NewValue;
    if (newValue != null)
    {
      newValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedDates_OnCollectionChanged);
      newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedDates_OnCollectionChanged);
    }
    if ((DatesCollection) e.NewValue == null)
      return;
    if (((Collection<DateTime>) e.NewValue).Count > 0)
    {
      this.SelectedDatesList.Clear();
      for (int index = 0; index < ((Collection<DateTime>) e.NewValue).Count; ++index)
        this.SelectedDatesList.Add(new Syncfusion.Windows.Shared.Date(((Collection<DateTime>) e.NewValue)[index], this.Calendar));
      this.SelectedDatesList.Sort();
    }
    if (this.CurrentDayGrid != null)
      this.CurrentDayGrid.SetIsSelected();
    if (this.FollowingDayGrid == null)
      return;
    this.FollowingDayGrid.SetIsSelected();
  }

  private void InitializeGrid()
  {
    this.CurrentMonthGrid = new MonthGrid();
    this.CurrentYearGrid = new YearGrid();
    this.CurrentYearRangeGrid = new YearRangeGrid();
    this.FollowingMonthGrid = new MonthGrid();
    this.FollowingYearGrid = new YearGrid();
    this.FollowingYearRangeGrid = new YearRangeGrid();
    this.DayNamesGrid = new DayNamesGrid();
    this.FollowingDayGrid = new DayGrid();
    this.CurrentDayGrid = new DayGrid();
    this.WeekNumbersGrid = new WeekNumbersGrid();
    this.CurrentWeekNumbersGrid = new WeekNumberGridPanel();
    this.FollowingWeekNumbersGrid = new WeekNumberGridPanel();
  }

  private List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
  {
    if (StartingDate > EndingDate)
      return (List<DateTime>) null;
    List<DateTime> dateRange = new List<DateTime>();
    DateTime dateTime = StartingDate;
    do
    {
      dateRange.Add(dateTime);
      dateTime = dateTime.AddDays(1.0);
    }
    while (dateTime <= EndingDate);
    return dateRange;
  }

  private void ProcessSelectedDatesCollectionChange(NotifyCollectionChangedEventArgs e)
  {
    if (!this.AllowMultiplySelection)
      return;
    VisibleDate visibleData = this.VisibleData;
    if (e.Action == NotifyCollectionChangedAction.Reset && this.SelectedDatesList != null)
      this.SelectedDatesList.Clear();
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewItems.Count > 1)
      {
        foreach (DateTime newItem in (IEnumerable) e.NewItems)
        {
          Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date(newItem, this.Calendar);
          if (!this.SelectedDatesList.Contains(date))
            this.SelectedDatesList.Add(date);
        }
        this.SelectedDatesList.Sort();
      }
      else if (e.NewItems.Count != 0)
      {
        Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date((DateTime) e.NewItems[0], this.Calendar);
        if (!this.SelectedDatesList.Contains(date))
          this.SelectedDatesList.Insert(~this.SelectedDatesList.BinarySearch(date), date);
      }
    }
    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      this.SelectedDatesList.Clear();
      foreach (DateTime selectedDate in (Collection<DateTime>) this.SelectedDates)
      {
        Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date(selectedDate, this.Calendar);
        if (!this.SelectedDatesList.Contains(date))
          this.SelectedDatesList.Add(date);
      }
    }
    if (e.Action == NotifyCollectionChangedAction.Move)
      throw new NotImplementedException();
    if (e.Action == NotifyCollectionChangedAction.Replace)
      throw new NotImplementedException();
    this.CurrentDayGrid.SetIsSelected();
    this.FollowingDayGrid.SetIsSelected();
  }

  internal CalendarEditGrid FindCurrentGrid(CalendarVisualMode mode)
  {
    if (mode == CalendarVisualMode.WeekNumbers && this.FollowingWeekNumbersGrid != null)
    {
      int numberOfWeeks = WeekNumberGridPanel.NumberOfWeeks;
      for (int index = 1; index <= this.FollowingWeekNumbersGrid.CellsCollection.Count; ++index)
      {
        Cell cells = this.FollowingWeekNumbersGrid.CellsCollection[index - 1] as Cell;
        if (index <= numberOfWeeks)
        {
          cells.BorderBrush = this.WeekNumberSelectionBorderBrush;
        }
        else
        {
          cells.BorderBrush = (Brush) null;
          cells.IsEnabled = false;
        }
        cells.Background = (Brush) null;
      }
      for (int index = 1; index <= numberOfWeeks; ++index)
      {
        Cell cells = this.FollowingWeekNumbersGrid.CellsCollection[index - 1] as Cell;
        if (index == this.FollowingWeekNumbersGrid.FocusedCellIndex)
        {
          cells.IsSelected = true;
          cells.Background = this.WeekNumberSelectionBorderBrush;
        }
        else
          cells.IsSelected = false;
      }
      return this.CurrentWeekNumbersGrid != null && this.CurrentWeekNumbersGrid.Visibility == Visibility.Visible ? (CalendarEditGrid) this.CurrentWeekNumbersGrid : (CalendarEditGrid) this.FollowingWeekNumbersGrid;
    }
    switch (mode)
    {
      case CalendarVisualMode.Days:
        return this.CurrentDayGrid != null && this.CurrentDayGrid.Visibility == Visibility.Visible ? (CalendarEditGrid) this.CurrentDayGrid : (CalendarEditGrid) this.FollowingDayGrid;
      case CalendarVisualMode.Months:
        return this.CurrentMonthGrid != null && this.CurrentMonthGrid.Visibility == Visibility.Visible ? (CalendarEditGrid) this.CurrentMonthGrid : (CalendarEditGrid) this.FollowingMonthGrid;
      case CalendarVisualMode.Years:
        return this.CurrentYearGrid != null && this.CurrentYearGrid.Visibility == Visibility.Visible ? (CalendarEditGrid) this.CurrentYearGrid : (CalendarEditGrid) this.FollowingYearGrid;
      case CalendarVisualMode.YearsRange:
        return this.CurrentYearRangeGrid != null && this.CurrentYearRangeGrid.Visibility == Visibility.Visible ? (CalendarEditGrid) this.CurrentYearRangeGrid : (CalendarEditGrid) this.FollowingYearRangeGrid;
      default:
        return (CalendarEditGrid) null;
    }
  }

  private void UpdateSelectedDatesList()
  {
    if (this.Calendar == null || this.SelectedDatesList == null)
      return;
    this.SelectedDatesList.Clear();
    foreach (DateTime selectedDate in (Collection<DateTime>) this.SelectedDates)
    {
      if (this.MinMaxHidden)
      {
        if (selectedDate < this.MaxDate && selectedDate > this.MinDate)
          this.SelectedDatesList.Add(new Syncfusion.Windows.Shared.Date(selectedDate, this.Calendar));
      }
      else if (selectedDate < this.miDate && selectedDate > this.miDate)
        this.SelectedDatesList.Add(new Syncfusion.Windows.Shared.Date(selectedDate, this.Calendar));
    }
    this.SelectedDatesList.Sort();
  }

  private void ProcessSelection(DateTime newDate)
  {
    if (this.SelectedDates == null)
      return;
    if (this.SelectedDates.Count == 0)
    {
      this.SelectedDates.Add(newDate);
      this.m_dateSetManual = true;
      this.Date = newDate;
    }
    else
    {
      this.SelectedDates.Clear();
      this.SelectedDates.Add(newDate);
      this.SelectedDatesList.Clear();
      this.ProcessSelectedDatesCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList) this.SelectedDates));
      this.m_dateSetManual = true;
      this.Date = newDate;
      this.UpdateCellClickedValue();
    }
  }

  private void OnDayCellClick(
    DayCell sender,
    CalendarEdit.ChangeMonthMode mode,
    ModifierKeys modifiers)
  {
    Syncfusion.Windows.Shared.Date date = sender.Date;
    sender.Focus();
    this.Invalidateflag = true;
    for (int index = 0; index < this.InvalidDates.Count; ++index)
    {
      if (date.CompareTo(this.InvalidDates[index]) == 0)
      {
        this.Invalidateflag = false;
        break;
      }
    }
    DateTime dateTime = date.ToDateTime(this.Calendar);
    if (!sender.IsCurrentMonth && mode != CalendarEdit.ChangeMonthMode.Enabled)
    {
      int month = date.Month;
      if (this.updateSelection)
        this.ScrollMonth(month);
    }
    if (this.AllowSelection && this.Invalidateflag)
    {
      if (this.AllowMultiplySelection && modifiers != ModifierKeys.None)
      {
        if (modifiers == ModifierKeys.None)
          return;
        this.MultiplySelect(dateTime, modifiers);
      }
      else
        this.ProcessSelection(dateTime);
    }
    else
      this.m_dateSetManual = true;
  }

  private void UpdateCellClickedValue()
  {
    if (!this.MinMaxHidden || !(this.Date == this.MinDate))
      return;
    this.IsCellClicked = true;
  }

  private void OnYearCellClick(YearCell sender)
  {
    sender.Focus();
    VisibleDate date = new VisibleDate(sender.Year, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    this.yearpressed = true;
    if (this.IsFlagValueSet(this.ViewMode) && !this.IsNextModeSet)
    {
      this.VisualMode = this.VisualModeInfo.OldMode;
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, this.VisualMode);
      this.DayNamesGrid.Visibility = Visibility.Visible;
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      this.FindCurrentGrid(CalendarVisualMode.Months).Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.ChangeVisualModePreview(date);
    }
    else
    {
      this.VisualMode = CalendarVisualMode.Months;
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, CalendarVisualMode.Months);
      this.CurrentYearGrid.Visibility = Visibility.Hidden;
      this.FollowingYearGrid.Visibility = Visibility.Hidden;
      this.ChangeVisualModePreview(date);
    }
  }

  private void OnYearRangeCellClick(YearRangeCell sender)
  {
    sender.Focus();
    this.yearrangepress = true;
    if (this.IsFlagValueSet(this.ViewMode) && !this.IsNextModeSet)
    {
      this.VisualMode = this.VisualModeInfo.OldMode;
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.YearsRange, this.VisualMode);
      VisibleDate date = new VisibleDate(sender.Years.StartYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      if (this.VisualMode == CalendarVisualMode.Days)
        this.DayNamesGrid.Visibility = Visibility.Visible;
      this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
      this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.ChangeVisualModePreview(date);
    }
    else
    {
      this.VisualMode = CalendarVisualMode.Years;
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.YearsRange, CalendarVisualMode.Years);
      VisibleDate date;
      if (this.VisibleData.VisibleYear >= sender.Years.StartYear && this.VisibleData.VisibleYear <= sender.Years.EndYear)
      {
        CalendarEdit.startYear = sender.Years.StartYear;
        CalendarEdit.endYear = sender.Years.EndYear;
        CalendarEdit.primaryDate = this.VisibleData.VisibleYear;
        date = new VisibleDate(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
      }
      else
        date = CalendarEdit.startYear == sender.Years.StartYear || CalendarEdit.endYear == sender.Years.EndYear ? new VisibleDate(CalendarEdit.primaryDate, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay) : new VisibleDate(sender.Years.StartYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
      this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
      this.FollowingYearGrid.Visibility = Visibility.Hidden;
      this.ChangeVisualModePreview(date);
    }
  }

  private void OnMonthCellClick(MonthCell sender)
  {
    sender.Focus();
    this.monthpressed = true;
    if (!this.DisableDateSelection)
    {
      this.VisualMode = CalendarVisualMode.Days;
      this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days);
      VisibleDate date = new VisibleDate(this.VisibleData.VisibleYear, sender.MonthNumber, this.VisibleData.VisibleDay);
      this.DayNamesGrid.Visibility = Visibility.Visible;
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.ChangeVisualModePreview(date);
    }
    else
    {
      if (!this.AllowSelection || !this.Invalidateflag)
        return;
      if (!sender.IsSelected)
      {
        ArrayList arrayList = new ArrayList();
        foreach (Cell cells in this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection)
        {
          if (cells.IsSelected && cells != this.mpressedCell)
            cells.IsSelected = false;
        }
        sender.IsSelected = true;
        sender.Focus();
        Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date();
        date.Month = sender.MonthNumber;
        string s = this.m_monthButton1.Content.ToString();
        date.Year = int.Parse(s);
        date.Day = DateTime.DaysInMonth(date.Year, date.Month);
        DateTime dateTime = date.ToDateTime(this.Calendar);
        this.VisualMode = CalendarVisualMode.Months;
        this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Months);
        this.Date = dateTime;
        DateTimeFormatInfo dateTimeFormat = this.Culture.DateTimeFormat;
        this.VisibleData = new VisibleDate(this.Date.Year, this.Date.Month, this.Date.Day);
        this.VisibleDataToMinSupportedDate(dateTimeFormat);
      }
      this.monthpressed = false;
    }
  }

  private void OnWeekNumbersCellClick(WeekNumberCell sender)
  {
    if (this.DisableDateSelection)
      return;
    this.CurrentDayGrid.Visibility = Visibility.Hidden;
    this.FollowingDayGrid.Visibility = Visibility.Hidden;
    this.VisualMode = CalendarVisualMode.WeekNumbers;
    this.UpdateWeekNumbersContainer();
    this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Days, CalendarVisualMode.WeekNumbers);
    VisibleDate date = new VisibleDate(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    int numberOfWeeks = WeekNumberGridPanel.NumberOfWeeks;
    WeekNumberGridPanel followingWeekNumbersGrid = this.FollowingWeekNumbersGrid;
    this.ChangeVisualModePreview(date);
  }

  private void UpdateVisibleData()
  {
    VisibleDate visibleDate;
    visibleDate.VisibleMonth = this.Calendar.GetMonth(this.Date);
    visibleDate.VisibleYear = this.Calendar.GetYear(this.Date);
    visibleDate.VisibleDay = this.Calendar.GetDayOfMonth(this.Date);
    this.VisibleData = visibleDate;
  }

  private bool IsAnimationRequired() => this.Visibility == Visibility.Visible;

  private void TodayButton_Click(object sender, RoutedEventArgs e)
  {
    this.SelectedDates.Clear();
    DateTime date = DateTime.Now.Date;
    this.Date = date;
    if (this.AllowSelection)
      this.SelectedDates.Add(date);
    this.IsTodayButtonClicked = true;
    this.UpdateVisibleData();
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.updateSelection = false;
    base.OnMouseLeftButtonDown(e);
    if (!this.IsStoryboardActive(this.mvisualModeStoryboard) && !this.IsStoryboardActive(this.mmoveStoryboard) && !this.IsStoryboardActive(this.mmonthStoryboard))
    {
      if (this.VisualMode == CalendarVisualMode.Months)
        this.HideWeekNumbersForYearContainer();
      if (!this.DisableDateSelection)
      {
        if (this.VisualMode == CalendarVisualMode.Days)
        {
          this.HideWeekNumbersForYearContainer();
          this.FindCurrentGrid(CalendarVisualMode.Days).Focus();
        }
        if (e.Source is DayCell)
          this.FireDayCellMouseLeftButtonDown(e);
        if (e.Source is DayNameCell)
          this.FireDayNameCellMouseLeftButtonDown(e);
      }
      if (e.Source is MonthCell)
        this.FireMonthCellMouseLeftButtonDown(e);
      if (e.Source is YearCell)
        this.FireYearCellMouseLeftButtonDown(e);
      if (e.Source is YearRangeCell)
        this.FireYearRangeCellMouseLeftButtonDown(e);
      if (!this.DisableDateSelection)
      {
        if (e.Source is WeekNumberCell)
          this.FireWeekNumberCellMouseLeftButtonDown(e);
        if (e.Source is WeekNumberCellPanel)
          this.FireWeekNumberCellPanelMouseLeftButtonDown(e);
      }
    }
    e.Handled = false;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.updateSelection = true;
    base.OnMouseLeftButtonUp(e);
    if (!this.IsStoryboardActive(this.mvisualModeStoryboard))
    {
      if (!this.DisableDateSelection && e.Source is DayCell)
        this.FireDayCellMouseLeftButtonUp(e);
      if (e.Source is MonthCell)
        this.FireMonthCellMouseLeftButtonUp(e);
      if (e.Source is YearCell)
        this.FireYearCellMouseLeftButtonUp(e);
      if (!(e.Source is YearRangeCell))
        return;
      this.FireYearRangeCellMouseLeftButtonUp(e);
    }
    else
      e.Handled = true;
  }

  protected override void OnStylusButtonUp(StylusButtonEventArgs e)
  {
    if (!this.IsStoryboardActive(this.mvisualModeStoryboard))
    {
      if (e.Handled)
        return;
      if (!this.DisableDateSelection && e.Source is DayCell)
        this.HandleDayCellUp((DayCell) e.Source);
      if (e.Source is MonthCell)
        this.HandleMonthCellUp((MonthCell) e.Source);
      if (e.Source is YearCell)
        this.HandleYearCellUp((YearCell) e.Source);
      if (e.Source is YearRangeCell)
        this.HandleYearRangeCellUp((YearRangeCell) e.Source);
      e.Handled = true;
    }
    else
      e.Handled = true;
  }

  protected override void OnStylusDown(StylusDownEventArgs e)
  {
    base.OnStylusDown(e);
    this.updateSelection = false;
    if (this.IsStoryboardActive(this.mvisualModeStoryboard) || this.IsStoryboardActive(this.mmoveStoryboard) || this.IsStoryboardActive(this.mmonthStoryboard))
      return;
    if (this.VisualMode == CalendarVisualMode.Months)
      this.HideWeekNumbersForYearContainer();
    if (e.Handled)
      return;
    if (!this.DisableDateSelection)
    {
      if (this.VisualMode == CalendarVisualMode.Days)
      {
        this.HideWeekNumbersForYearContainer();
        this.FindCurrentGrid(CalendarVisualMode.Days).Focus();
      }
      if (e.Source is DayCell)
        this.HandleDayCellDown((DayCell) e.Source);
      if (e.Source is DayNameCell)
        this.HandleDayNameCellDown(e.Source as UIElement);
    }
    if (e.Source is MonthCell)
      this.HandleMonthCellDown((MonthCell) e.Source);
    if (e.Source is YearCell)
      this.mpressedCell = (Cell) e.Source;
    if (e.Source is YearRangeCell)
      this.mpressedCell = (Cell) e.Source;
    if (this.DisableDateSelection)
      return;
    if (e.Source is WeekNumberCell && e.OriginalSource is TextBlock)
      this.HandleWeekNumberCellDown((WeekNumberCell) e.Source, (TextBlock) e.OriginalSource);
    if (!(e.Source is WeekNumberCellPanel))
      return;
    this.HandleWeekNumberCellPanelDown(e.Source as WeekNumberCellPanel);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (Keyboard.Modifiers == ModifierKeys.Control)
    {
      if (this.CalendarStyle != CalendarStyle.Vista || this.IsStoryboardActive(this.mvisualModeStoryboard))
        return;
      if (e.Delta > 0)
      {
        this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
      }
      else
      {
        Cell sender = (Cell) null;
        foreach (Cell cells in this.FindCurrentGrid(this.VisualMode).CellsCollection)
        {
          if (cells.IsMouseOver)
            sender = cells;
        }
        if (sender is YearRangeCell)
          this.OnYearRangeCellClick(sender as YearRangeCell);
        if (sender is YearCell)
          this.OnYearCellClick(sender as YearCell);
        if (sender is MonthCell)
          this.OnMonthCellClick(sender as MonthCell);
        if (!(sender is WeekNumberCellPanel))
          return;
        this.OnWeekNumberCellPanelClick(sender as WeekNumberCellPanel);
      }
    }
    else
    {
      if (e.Handled)
        return;
      if (e.Source is DayCell)
        this.mouseDayscroll = true;
      else if (e.Source is MonthCell)
        this.mousemonthscroll = true;
      else if (e.Source is YearCell)
        this.mouseyearscroll = true;
      else if (e.Source is YearRangeCell)
        this.mouseyearrangescroll = true;
      else if (e.Source is WeekNumberCellPanel)
      {
        this.mouseweeknumberscroll = true;
        this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
      }
      if (e.Delta > 0)
        CalendarEdit.PrevCommand.Execute((object) null, (IInputElement) this);
      else
        CalendarEdit.NextCommand.Execute((object) null, (IInputElement) this);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.DayNamesGrid != null)
    {
      this.DayNamesGrid.MouseMove -= new MouseEventHandler(this.DayNamesGrid_OnMouseMove);
      this.DayNamesGrid.MouseLeave -= new MouseEventHandler(this.DayNamesGrid_OnMouseLeave);
    }
    if (this.FollowingDayGrid != null)
    {
      this.FollowingDayGrid.KeyDown -= new KeyEventHandler(this.DayGrid_OnKeyDown);
      this.FollowingDayGrid.MouseMove -= new MouseEventHandler(this.DayGrid_OnMouseMove);
      this.FollowingDayGrid.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
      this.CurrentDayGrid.StylusButtonUp -= new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
      this.FollowingDayGrid.StylusMove -= new StylusEventHandler(this.DayGrid_StylusMove);
      this.FollowingDayGrid.StylusUp -= new StylusEventHandler(this.DayGrid_StylusUp);
      this.FollowingDayGrid.StylusSystemGesture -= new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
    }
    if (this.CurrentDayGrid != null)
    {
      this.CurrentDayGrid.KeyDown -= new KeyEventHandler(this.DayGrid_OnKeyDown);
      this.CurrentDayGrid.MouseMove -= new MouseEventHandler(this.DayGrid_OnMouseMove);
      this.CurrentDayGrid.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
      this.CurrentDayGrid.StylusButtonUp -= new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
      this.CurrentDayGrid.StylusMove -= new StylusEventHandler(this.DayGrid_StylusMove);
      this.CurrentDayGrid.StylusUp -= new StylusEventHandler(this.DayGrid_StylusUp);
      this.CurrentDayGrid.StylusSystemGesture -= new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
    }
    if (this.CurrentMonthGrid != null)
      this.CurrentMonthGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    if (this.CurrentYearGrid != null)
      this.CurrentYearGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    if (this.CurrentYearRangeGrid != null)
      this.CurrentYearRangeGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    if (this.FollowingMonthGrid != null)
      this.FollowingMonthGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    if (this.FollowingYearGrid != null)
      this.FollowingYearGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    if (this.FollowingYearRangeGrid != null)
      this.FollowingYearRangeGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.InitializeGrid();
    if (this.DayNameCellsStyle != null)
      this.DayNamesGrid.UpdateStyles(this.DayNameCellsStyle);
    this.AddLogicalChild((object) this.DayNamesGrid);
    this.DayNamesGrid.MouseLeave += new MouseEventHandler(this.DayNamesGrid_OnMouseLeave);
    this.DayNamesGrid.MouseMove += new MouseEventHandler(this.DayNamesGrid_OnMouseMove);
    this.AddLogicalChild((object) this.FollowingDayGrid);
    this.FollowingDayGrid.Visibility = Visibility.Hidden;
    this.FollowingDayGrid.SelectionBorder.CornerRadius = this.SelectionBorderCornerRadius;
    this.FollowingDayGrid.MouseLeftButtonUp += new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
    this.FollowingDayGrid.StylusButtonUp += new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
    this.FollowingDayGrid.MouseMove += new MouseEventHandler(this.DayGrid_OnMouseMove);
    this.FollowingDayGrid.KeyDown += new KeyEventHandler(this.DayGrid_OnKeyDown);
    this.FollowingDayGrid.StylusMove += new StylusEventHandler(this.DayGrid_StylusMove);
    this.FollowingDayGrid.StylusSystemGesture += new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
    this.FollowingDayGrid.StylusUp += new StylusEventHandler(this.DayGrid_StylusUp);
    this.AddLogicalChild((object) this.CurrentDayGrid);
    this.CurrentDayGrid.SelectionBorder.CornerRadius = this.SelectionBorderCornerRadius;
    this.CurrentDayGrid.Visibility = Visibility.Visible;
    this.CurrentDayGrid.MouseLeftButtonUp += new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
    this.CurrentDayGrid.StylusButtonUp += new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
    this.CurrentDayGrid.MouseMove += new MouseEventHandler(this.DayGrid_OnMouseMove);
    this.CurrentDayGrid.StylusMove += new StylusEventHandler(this.DayGrid_StylusMove);
    this.CurrentDayGrid.StylusSystemGesture += new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
    this.CurrentDayGrid.StylusUp += new StylusEventHandler(this.DayGrid_StylusUp);
    this.CurrentDayGrid.KeyDown += new KeyEventHandler(this.DayGrid_OnKeyDown);
    this.AddLogicalChild((object) this.WeekNumbersGrid);
    this.AddLogicalChild((object) this.CurrentMonthGrid);
    this.CurrentMonthGrid.Visibility = Visibility.Hidden;
    this.CurrentMonthGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.AddLogicalChild((object) this.CurrentYearGrid);
    this.CurrentYearGrid.Visibility = Visibility.Hidden;
    this.CurrentYearGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.AddLogicalChild((object) this.CurrentYearRangeGrid);
    this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
    this.CurrentYearRangeGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.AddLogicalChild((object) this.FollowingYearGrid);
    this.FollowingYearGrid.Visibility = Visibility.Hidden;
    this.FollowingYearGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.AddLogicalChild((object) this.FollowingMonthGrid);
    this.FollowingMonthGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.FollowingMonthGrid.Visibility = Visibility.Hidden;
    this.AddLogicalChild((object) this.FollowingYearRangeGrid);
    this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
    this.FollowingYearRangeGrid.KeyDown += new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
    this.AddLogicalChild((object) this.CurrentWeekNumbersGrid);
    this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
    this.AddLogicalChild((object) this.FollowingWeekNumbersGrid);
    this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
    this.UpdateVisibleData();
    this.TodayDate = DateTime.Now.ToString("D", (IFormatProvider) this.Culture.DateTimeFormat);
    this.DateDataTemplates = new DataTemplatesDictionary();
    this.DateStyles = new StylesDictionary();
    this.DayNameCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.DayNameCell_OnMouseLeftButtonDown);
    this.DayNameCellMouseLeftButtonDown += new MouseButtonEventHandler(this.DayNameCell_OnMouseLeftButtonDown);
    this.DayCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonDown);
    this.DayCellMouseLeftButtonDown += new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonDown);
    this.DayCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonUp);
    this.DayCellMouseLeftButtonUp += new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonUp);
    this.MonthCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonDown);
    this.MonthCellMouseLeftButtonDown += new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonDown);
    this.MonthCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonUp);
    this.MonthCellMouseLeftButtonUp += new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonUp);
    this.YearCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonDown);
    this.YearCellMouseLeftButtonDown += new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonDown);
    this.YearCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonUp);
    this.YearCellMouseLeftButtonUp += new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonUp);
    this.YearRangeCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonDown);
    this.YearRangeCellMouseLeftButtonDown += new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonDown);
    this.YearRangeCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonUp);
    this.YearRangeCellMouseLeftButtonUp += new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonUp);
    this.WeekNumberCellPanelMouseLeftButtonDown -= new MouseButtonEventHandler(this.WeekNumberCellPanel_OnMouseLeftButtonDown);
    this.WeekNumberCellPanelMouseLeftButtonDown += new MouseButtonEventHandler(this.WeekNumberCellPanel_OnMouseLeftButtonDown);
    this.WeekNumberCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.WeekNumberCell_OnMouseLeftButtonDown);
    this.WeekNumberCellMouseLeftButtonDown += new MouseButtonEventHandler(this.WeekNumberCell_OnMouseLeftButtonDown);
    this.MouseDown -= new MouseButtonEventHandler(this.DayCell_MouseDown);
    this.MouseDown += new MouseButtonEventHandler(this.DayCell_MouseDown);
    this.ApplyYearEditingTemplate();
    this.ApplyEditMonthTemplate();
    this.ApplyWeekNumbersTemplate();
    this.ApplyWeekNumbersForYearTemplate();
    if (this.GetTemplateChild("rect") is Rectangle templateChild)
    {
      if (this.VisualMode != CalendarVisualMode.Days)
        templateChild.Visibility = Visibility.Collapsed;
      if (this.VisualMode == CalendarVisualMode.Days)
        templateChild.Visibility = Visibility.Visible;
    }
    foreach (Visual visual in VisualUtils.EnumChildrenOfType((Visual) this, typeof (NavigateButton)))
    {
      if (visual is NavigateButton navigateButton && navigateButton.Name == "PART_NextMonthButton")
      {
        this.m_nextButton = navigateButton;
        AutomationProperties.SetName((DependencyObject) this.m_nextButton, "Next");
      }
      if (navigateButton != null && navigateButton.Name == "PART_PrevMonthButton")
      {
        this.m_prevButton = navigateButton;
        AutomationProperties.SetName((DependencyObject) this.m_prevButton, "Previous");
      }
    }
    this.NextButtonUpdate();
    this.PrevButtonUpdate();
    this.NavigateButtonVerify();
    foreach (Visual visual in VisualUtils.EnumChildrenOfType((Visual) this, typeof (MonthButton)))
    {
      if (visual is MonthButton monthButton && monthButton.Name == "PART_Month1")
      {
        this.m_monthButton1 = monthButton;
        this.AddMonthButtonsEvents();
      }
      if (monthButton != null && monthButton.Name == "PART_Month2")
        this.m_monthButton2 = monthButton;
    }
    foreach (Visual visual in VisualUtils.EnumChildrenOfType((Visual) this, typeof (Popup)))
    {
      if (!(visual is Popup popup) || !(popup.Name == "PART_MonthPopup"))
        throw new ArgumentException("MonthPopup is not found");
      popup.PlacementTarget = (UIElement) this.m_monthButton1;
      this.m_popup = !this.MinMaxHidden ? new MonthPopup(popup, new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1), this.Culture.DateTimeFormat, new Syncfusion.Windows.Shared.Date(this.miDate, this.Calendar), new Syncfusion.Windows.Shared.Date(this.mxDate, this.Calendar)) : new MonthPopup(popup, new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1), this.Culture.DateTimeFormat, new Syncfusion.Windows.Shared.Date(this.MinDate, this.Calendar), new Syncfusion.Windows.Shared.Date(this.MaxDate, this.Calendar));
      this.m_popup.HidePopup += new EventHandler<HidePopupEventArgs>(this.Popup_OnHidePopup);
    }
    if (this.ScrollToDateEnabled)
      this.UpdateVisibleData();
    if (this.m_monthButton1 != null)
    {
      this.m_monthButton1.MouseLeftButtonDown += new MouseButtonEventHandler(this.MonthButton_OnMouseLeftButtonDown);
      this.m_monthButton1.StylusDown += new StylusDownEventHandler(this.monthButton_StylusDown);
      if (this.VisualMode == CalendarVisualMode.All)
        this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, CalendarVisualMode.Days);
      else
        this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    }
    if (this.m_monthButton2 != null)
      this.m_monthButton2.Visibility = Visibility.Hidden;
    this.m_todayButton = this.GetTemplateChild("PART_TodayButton") as Button;
    if (this.m_todayButton != null)
      this.m_todayButton.Click += new RoutedEventHandler(this.TodayButton_Click);
    if (this.VisualMode == CalendarVisualMode.Months)
    {
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentMonthGrid.Visibility = Visibility.Visible;
    }
    else if (this.VisualMode == CalendarVisualMode.Years)
    {
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentYearGrid.Visibility = Visibility.Visible;
    }
    else if (this.VisualMode == CalendarVisualMode.YearsRange)
    {
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentYearRangeGrid.Visibility = Visibility.Visible;
    }
    else
    {
      if (this.VisualMode != CalendarVisualMode.WeekNumbers)
        return;
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentWeekNumbersGrid.Visibility = Visibility.Visible;
    }
  }

  private void PrevButtonUpdate()
  {
    if (this.m_prevButton == null)
      return;
    this.m_prevButton.MouseEnter += new MouseEventHandler(this.NavigateButton_OnMouseEnter);
    this.m_prevButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.M_prevButton_PreviewMouseLeftButtonUp);
    this.m_prevButton.PreviewTouchDown += new EventHandler<TouchEventArgs>(this.M_prevButton_PreviewTouchDown);
    this.m_prevButton.UpdateCellTemplate(this.PreviousScrollButtonTemplate);
  }

  private void M_prevButton_PreviewTouchDown(object sender, TouchEventArgs e)
  {
    this.isMonthNavigated = true;
    if (this.VisualMode == CalendarVisualMode.Days)
      this.mouseDayscroll = true;
    else if (this.VisualMode == CalendarVisualMode.Months)
      this.mousemonthscroll = true;
    else if (this.VisualMode == CalendarVisualMode.Years)
      this.mouseyearscroll = true;
    else if (this.VisualMode == CalendarVisualMode.YearsRange)
      this.mouseyearrangescroll = true;
    else if (this.VisualMode == CalendarVisualMode.WeekNumbers)
    {
      this.mouseweeknumberscroll = true;
      this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
    }
    if (this.VisualMode == CalendarVisualMode.Days)
    {
      if (this.IsStoryboardActive(this.mmonthStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
      {
        --this.m_iscrollCounter;
      }
      else
      {
        this.BeginMoving(CalendarEdit.MoveDirection.Prev, -1);
        if (this.SelectedDates == null || this.SelectedDates.Count != 1)
          return;
        foreach (DayCell cells in this.FollowingDayGrid.CellsCollection)
        {
          if (cells.Date.Day > 0 && cells.Date.ToDateTime(this.Calendar) == this.SelectedDates[0].Date)
            cells.Focus();
        }
      }
    }
    else
    {
      if (this.IsStoryboardActive(this.mmoveStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
        return;
      this.Move(CalendarEdit.MoveDirection.Prev);
    }
  }

  private void M_prevButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.isMonthNavigated = true;
    CalendarVisualMode calendarVisualMode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      calendarVisualMode = this.ViewMode;
    switch (calendarVisualMode)
    {
      case CalendarVisualMode.WeekNumbers:
        this.mouseweeknumberscroll = true;
        this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
        break;
      case CalendarVisualMode.Days:
        this.mouseDayscroll = true;
        break;
      case CalendarVisualMode.Months:
        this.mousemonthscroll = true;
        break;
      case CalendarVisualMode.Years:
        this.mouseyearscroll = true;
        break;
      case CalendarVisualMode.YearsRange:
        this.mouseyearrangescroll = true;
        break;
    }
  }

  private void NextButtonUpdate()
  {
    if (this.m_nextButton == null)
      return;
    this.m_nextButton.MouseEnter += new MouseEventHandler(this.NavigateButton_OnMouseEnter);
    this.m_nextButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.M_nextButton_PreviewMouseLeftButtonUp);
    this.m_nextButton.PreviewTouchDown += new EventHandler<TouchEventArgs>(this.M_nextButton_PreviewTouchDown);
    this.m_nextButton.UpdateCellTemplate(this.NextScrollButtonTemplate);
  }

  private void M_nextButton_PreviewTouchDown(object sender, TouchEventArgs e)
  {
    this.isMonthNavigated = true;
    if (this.VisualMode == CalendarVisualMode.Days)
      this.mouseDayscroll = true;
    else if (this.VisualMode == CalendarVisualMode.Months)
      this.mousemonthscroll = true;
    else if (this.VisualMode == CalendarVisualMode.Years)
      this.mouseyearscroll = true;
    else if (this.VisualMode == CalendarVisualMode.YearsRange)
      this.mouseyearrangescroll = true;
    else if (this.VisualMode == CalendarVisualMode.WeekNumbers)
    {
      this.mouseweeknumberscroll = true;
      this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
    }
    if (this.VisualMode == CalendarVisualMode.Days)
    {
      if (this.IsStoryboardActive(this.mmonthStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
      {
        ++this.m_iscrollCounter;
      }
      else
      {
        this.BeginMoving(CalendarEdit.MoveDirection.Next, 1);
        if (this.SelectedDates == null || this.SelectedDates.Count != 1)
          return;
        foreach (DayCell cells in this.FollowingDayGrid.CellsCollection)
        {
          if (cells.Date.Day > 0 && cells.Date.ToDateTime(this.Calendar) == this.SelectedDates[0].Date)
            cells.Focus();
        }
      }
    }
    else
    {
      if (this.IsStoryboardActive(this.mmoveStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
        return;
      this.Move(CalendarEdit.MoveDirection.Next);
    }
  }

  private void M_nextButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.isMonthNavigated = true;
    CalendarVisualMode calendarVisualMode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      calendarVisualMode = this.ViewMode;
    switch (calendarVisualMode)
    {
      case CalendarVisualMode.WeekNumbers:
        this.mouseweeknumberscroll = true;
        this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
        break;
      case CalendarVisualMode.Days:
        this.mouseDayscroll = true;
        break;
      case CalendarVisualMode.Months:
        this.mousemonthscroll = true;
        break;
      case CalendarVisualMode.Years:
        this.mouseyearscroll = true;
        break;
      case CalendarVisualMode.YearsRange:
        this.mouseyearrangescroll = true;
        break;
    }
  }

  private void AddMonthButtonsEvents()
  {
    if (!this.IsYearSelectionInStandardStyle() || this.m_monthButton1 == null)
      return;
    this.m_monthButton1.MouseEnter += new MouseEventHandler(this.MonthButton1_MouseEnter);
    this.m_monthButton1.MouseLeave += new MouseEventHandler(this.MonthButton1_MouseLeave);
  }

  private void DeleteMonthButtonsEvents()
  {
    if (this.IsYearSelectionInStandardStyle() || this.m_monthButton1 == null)
      return;
    this.m_monthButton1.MouseEnter -= new MouseEventHandler(this.MonthButton1_MouseEnter);
    this.m_monthButton1.MouseLeave -= new MouseEventHandler(this.MonthButton1_MouseLeave);
  }

  private void ApplyWeekNumbersTemplate()
  {
    this.m_weekNumbersContainer = this.GetWeekNumbersContainer();
    this.m_mainGrid = this.GetMainGrid();
    if (this.ShowWeekNumbers)
    {
      this.ShowWeekNumbersContainer();
    }
    else
    {
      if (this.VisualMode != CalendarVisualMode.WeekNumbers)
        return;
      this.m_mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Auto);
    }
  }

  private void ApplyWeekNumbersForYearTemplate()
  {
    this.wcurrentweekNumbersContainer = this.GetWeekNumbersPanelCurrent();
    this.wfollowweekNumbersContainer = this.GetWeekNumbersPanelFollow();
    if (!this.IsShowWeekNumbersGrid)
      return;
    this.ShowWeekNumbersForYearContainer();
  }

  private void ApplyYearEditingTemplate()
  {
    this.m_yearUpDown = this.GetYearUpDown();
    this.m_yearUpDownPanel = this.GetYearUpDownPanel();
  }

  private void ApplyEditMonthTemplate()
  {
    this.m_editMonthName = this.Template != null ? this.Template.FindName("PART_EditMonthName", (FrameworkElement) this) as TextBlock : (TextBlock) null;
    if (!this.IsYearSelectionInStandardStyle() || this.m_editMonthName == null)
      return;
    this.SetEditMonthText();
  }

  private bool IsYearSelectionInStandardStyle()
  {
    return this.AllowYearEditing && this.CalendarStyle == CalendarStyle.Standard;
  }

  private void SetEditMonthText()
  {
    int visibleMonth = this.VisibleData.VisibleMonth;
    int visibleYear = this.VisibleData.VisibleYear;
    DateTimeFormatInfo dateTimeFormat = this.Culture.DateTimeFormat;
    if (this.VisualMode == CalendarVisualMode.Days)
    {
      if (this.IsMonthNameAbbreviated || this.ShowAbbreviatedMonthNames)
        this.m_editMonthName.Text = dateTimeFormat.AbbreviatedMonthNames[visibleMonth - 1];
      else
        this.m_editMonthName.Text = dateTimeFormat.MonthNames[visibleMonth - 1];
    }
    else
      this.m_editMonthName.Text = string.Empty;
  }

  private StackPanel GetYearUpDownPanel()
  {
    StackPanel name = this.Template != null ? this.Template.FindName("PART_YearUpDownPanel", (FrameworkElement) this) as StackPanel : (StackPanel) null;
    if (name != null)
      name.MouseLeave += new MouseEventHandler(this.UpDownPanel_MouseLeave);
    return name;
  }

  private UpDown GetYearUpDown()
  {
    UpDown name = this.Template != null ? this.Template.FindName("PART_YearUpDown", (FrameworkElement) this) as UpDown : (UpDown) null;
    if (name != null && name.NumberFormatInfo != null)
    {
      name.NumberFormatInfo.NumberDecimalDigits = 0;
      name.NumberFormatInfo.NumberGroupSeparator = string.Empty;
      name.Value = new double?((double) this.VisibleData.VisibleYear);
    }
    return name;
  }

  private void MonthButton1_MouseLeave(object sender, MouseEventArgs e)
  {
    if (this.IsUpDownPanelVisible())
      return;
    this.m_timer.Stop();
  }

  private void UpDownPanel_MouseLeave(object sender, MouseEventArgs e)
  {
    if (!this.IsUpDownPanelVisible())
      return;
    this.m_yearUpDownPanel.Visibility = Visibility.Collapsed;
    this.m_monthButton1.Visibility = Visibility.Visible;
    this.VisibleData = new VisibleDate((int) this.m_yearUpDown.Value.Value, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
  }

  private void MonthButton1_MouseEnter(object sender, MouseEventArgs e)
  {
    this.m_timer = new DispatcherTimer();
    this.m_timer.Interval = TimeSpan.FromSeconds(1.0);
    this.m_timer.Tick += new EventHandler(this.Timer_Tick);
    this.m_timer.Start();
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    ((DispatcherTimer) sender).Stop();
    this.m_yearUpDownPanel.Visibility = Visibility.Visible;
    this.m_monthButton1.Visibility = Visibility.Collapsed;
    this.SetEditMonthText();
    this.m_yearUpDown.Value = new double?((double) this.VisibleData.VisibleYear);
  }

  private bool IsUpDownPanelVisible() => this.m_yearUpDownPanel.Visibility == Visibility.Visible;

  private ContentPresenter GetWeekNumbersContainer()
  {
    return this.Template == null ? (ContentPresenter) null : this.Template.FindName("PART_WeekNumbers", (FrameworkElement) this) as ContentPresenter;
  }

  private ContentPresenter GetWeekNumbersPanelCurrent()
  {
    return this.Template.FindName("WeekNumbersForYearCurrent", (FrameworkElement) this) as ContentPresenter;
  }

  private ContentPresenter GetWeekNumbersPanelFollow()
  {
    return this.Template.FindName("WeekNumbersForYearFollow", (FrameworkElement) this) as ContentPresenter;
  }

  private Grid GetMainGrid()
  {
    return this.Template == null ? (Grid) null : this.Template.FindName("MainGrid", (FrameworkElement) this) as Grid;
  }

  private void UpdateSelectionBorderBrush()
  {
    this.CurrentDayGrid.SelectionBorder.BorderBrush = this.SelectionBorderBrush;
    this.FollowingDayGrid.SelectionBorder.BorderBrush = this.SelectionBorderBrush;
  }

  protected virtual void FireYearRangeCellMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.YearRangeCellMouseLeftButtonUp == null)
      return;
    this.YearRangeCellMouseLeftButtonUp(e.Source, e);
  }

  protected virtual void FireYearRangeCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.YearRangeCellMouseLeftButtonDown == null)
      return;
    this.YearRangeCellMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireYearCellMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.YearCellMouseLeftButtonUp == null)
      return;
    this.YearCellMouseLeftButtonUp(e.Source, e);
  }

  protected virtual void FireYearCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.YearCellMouseLeftButtonDown == null)
      return;
    this.YearCellMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireMonthCellMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.MonthCellMouseLeftButtonUp == null)
      return;
    this.MonthCellMouseLeftButtonUp(e.Source, e);
  }

  protected virtual void FireMonthCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.MonthCellMouseLeftButtonDown == null)
      return;
    this.MonthCellMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireDayCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.DayCellMouseLeftButtonDown == null)
      return;
    this.DayCellMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireWeekNumberCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.WeekNumberCellMouseLeftButtonDown == null)
      return;
    this.WeekNumberCellMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireWeekNumberCellPanelMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.WeekNumberCellPanelMouseLeftButtonDown == null)
      return;
    this.WeekNumberCellPanelMouseLeftButtonDown(e.Source, e);
  }

  protected virtual void FireDayCellMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.DayCellMouseLeftButtonUp == null)
      return;
    this.DayCellMouseLeftButtonUp(e.Source, e);
  }

  protected virtual void FireDayNameCellMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.DayNameCellMouseLeftButtonDown == null)
      return;
    this.DayNameCellMouseLeftButtonDown(e.Source, e);
  }

  private void MonthButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsStoryboardActive(this.mvisualModeStoryboard))
      this.HandleHeaderButtonDown();
    else
      e.Handled = true;
  }

  internal CalendarVisualMode GetNextValue(CalendarVisualMode mode)
  {
    Array values = Enum.GetValues(mode.GetType());
    int num = Array.IndexOf(values, (object) mode);
    if (num == values.Length - 1)
      return mode;
    CalendarVisualMode nextValue = mode;
    for (; num < values.Length - 1; ++num)
    {
      if (this.HasFlag((Enum) this.ViewMode, (Enum) (CalendarVisualMode) values.GetValue(num + 1)))
      {
        nextValue = (CalendarVisualMode) values.GetValue(num + 1);
        if (num + 1 - Array.IndexOf(values, (object) mode) == 1)
        {
          this.IsNextModeSet = true;
          break;
        }
        if (Array.IndexOf(values, (object) nextValue) - Array.IndexOf(values, (object) mode) > 1)
        {
          this.IsNextModeSet = false;
          break;
        }
        break;
      }
    }
    return nextValue;
  }

  internal CalendarVisualMode GetPreviousValue(CalendarVisualMode mode)
  {
    Array values = Enum.GetValues(mode.GetType());
    int num = Array.IndexOf(values, (object) mode);
    if (num <= 0)
      return mode;
    CalendarVisualMode previousValue = mode;
    for (; num > 1; --num)
    {
      if (this.HasFlag((Enum) this.ViewMode, (Enum) (CalendarVisualMode) values.GetValue(num - 1)))
      {
        previousValue = (CalendarVisualMode) values.GetValue(num - 1);
        break;
      }
    }
    return previousValue;
  }

  internal bool IsFlagValueSet(CalendarVisualMode mode)
  {
    Array values = Enum.GetValues(mode.GetType());
    for (int index = Array.IndexOf(values, (object) mode); index < values.Length - 1; ++index)
    {
      if (this.HasFlag((Enum) this.ViewMode, (Enum) (CalendarVisualMode) values.GetValue(index + 1)))
        return true;
    }
    return false;
  }

  internal bool HasFlag(Enum variable, Enum value)
  {
    if (variable == null)
      return false;
    ulong num = value != null ? Convert.ToUInt64((object) value) : throw new ArgumentNullException(nameof (value));
    return ((long) Convert.ToUInt64((object) variable) & (long) num) == (long) num;
  }

  private void VisualModeStoryboard_OnCompleted(object sender, EventArgs e)
  {
    CalendarVisualMode mode = this.VisualModeInfo.OldMode;
    CalendarVisualMode newMode = this.VisualModeInfo.NewMode;
    ArrayList arrayList = new ArrayList();
    if (mode == CalendarVisualMode.Days && newMode == CalendarVisualMode.Months)
    {
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    }
    if (mode == CalendarVisualMode.Months && newMode == CalendarVisualMode.Days)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.Months && newMode == CalendarVisualMode.Years)
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    if (mode == CalendarVisualMode.Years && newMode == CalendarVisualMode.Months)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.Years && newMode == CalendarVisualMode.YearsRange)
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    if (mode == CalendarVisualMode.YearsRange && newMode == CalendarVisualMode.Years)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.Days && newMode == CalendarVisualMode.WeekNumbers)
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    if (mode == CalendarVisualMode.WeekNumbers && newMode == CalendarVisualMode.Days)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.WeekNumbers && newMode == CalendarVisualMode.Days)
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    if (mode == CalendarVisualMode.Days && newMode == CalendarVisualMode.WeekNumbers)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (mode == CalendarVisualMode.WeekNumbers && newMode == CalendarVisualMode.Months)
      arrayList = this.FindCurrentGrid(newMode).CellsCollection;
    if (mode == CalendarVisualMode.Months && newMode == CalendarVisualMode.WeekNumbers)
      arrayList = this.FindCurrentGrid(mode).CellsCollection;
    if (this.IsFlagValueSet(this.ViewMode) && mode != newMode)
    {
      if (this.VisualMode == CalendarVisualMode.Days)
      {
        this.DayNamesGrid.Visibility = Visibility.Visible;
        arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
      }
      else
        arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
    }
    if (this.ViewMode != CalendarVisualMode.All && this.VisualMode != CalendarVisualMode.All)
    {
      if (!this.IsNextModeSet)
      {
        if (this.ViewMode == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && this.VisualMode == CalendarVisualMode.Days)
        {
          this.DayNamesGrid.Visibility = Visibility.Visible;
          arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
        }
        else if (this.ViewMode != CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) && this.VisualMode != CalendarVisualMode.Days)
          arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
        if (this.VisualModeInfo.NewMode != this.VisualMode)
        {
          mode = this.VisualModeInfo.NewMode;
          CalendarVisualMode visualMode = this.VisualMode;
        }
      }
      else if (this.VisualMode == CalendarVisualMode.Days)
      {
        this.DayNamesGrid.Visibility = Visibility.Visible;
        arrayList = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
      }
      else
        arrayList = this.FindCurrentGrid(this.VisualMode).CellsCollection;
    }
    if (mode != this.VisualMode)
      this.FindCurrentGrid(mode).Visibility = Visibility.Hidden;
    foreach (Cell cell in arrayList)
    {
      if (cell.IsSelected)
        cell.Opacity = 1.0;
    }
    this.NavigateButtonVerify();
    if (this.m_visualModeQueue.Count != 0)
      this.ScrollToDate();
    if (this.FindCurrentGrid(this.VisualMode) == null || this.FindCurrentGrid(this.VisualMode).CellsCollection.Count <= 0)
      return;
    foreach (Cell cells in this.FindCurrentGrid(this.VisualMode).CellsCollection)
    {
      if (cells.IsSelected)
        cells.Focus();
    }
  }

  private void MoveStoryboard_Completed(object sender, EventArgs e)
  {
    CalendarEditGrid calendarEditGrid = (CalendarEditGrid) null;
    CalendarVisualMode calendarVisualMode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      calendarVisualMode = this.ViewMode;
    if (calendarVisualMode == CalendarVisualMode.WeekNumbers)
      calendarEditGrid = (CalendarEditGrid) this.CurrentWeekNumbersGrid;
    if (calendarVisualMode == CalendarVisualMode.Months)
      calendarEditGrid = (CalendarEditGrid) this.CurrentMonthGrid;
    if (calendarVisualMode == CalendarVisualMode.Years)
      calendarEditGrid = (CalendarEditGrid) this.CurrentYearGrid;
    if (calendarVisualMode == CalendarVisualMode.YearsRange)
      calendarEditGrid = (CalendarEditGrid) this.CurrentYearRangeGrid;
    this.m_monthButton2.Visibility = Visibility.Hidden;
    if (calendarEditGrid == null)
      return;
    calendarEditGrid.Visibility = Visibility.Hidden;
  }

  private void Popup_OnHidePopup(object sender, HidePopupEventArgs e)
  {
    Syncfusion.Windows.Shared.Date selectedDate = e.SelectedDate;
    Syncfusion.Windows.Shared.Date date = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1);
    DateTime startDate = new DateTime(date.Year, date.Month, 1);
    DateTime endDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
    int monthDelta = this.CalculateMonthDelta(startDate, endDate);
    this.isMonthNavigated = true;
    if (monthDelta == 0)
      return;
    if (startDate < endDate)
      this.BeginMoving(CalendarEdit.MoveDirection.Next, monthDelta);
    else
      this.BeginMoving(CalendarEdit.MoveDirection.Prev, monthDelta);
  }

  private void NavigateButton_OnMouseEnter(object sender, MouseEventArgs e)
  {
    if (e.Handled)
      return;
    if (!this.IsStoryboardActive(this.mmonthStoryboard))
    {
      NavigateButton navigateButton = (NavigateButton) sender;
      int num = 0;
      int visibleMonth = this.VisibleData.VisibleMonth;
      if (navigateButton.Name == "PART_NextMonthButton")
        num = DateUtils.AddMonth(visibleMonth, 1);
      if (navigateButton.Name == "PART_PrevMonthButton")
        num = DateUtils.AddMonth(visibleMonth, -1);
      foreach (DayCell cells in this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection)
      {
        if (cells.Date.Month == num)
          cells.FireHighlightEvent();
      }
    }
    e.Handled = true;
  }

  private void DayGrid_OnKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Handled)
      return;
    if (!this.IsStoryboardActive(this.mmonthStoryboard))
    {
      bool flag = false;
      this.updateSelection = false;
      int num = 0;
      int columnsCount = this.FindCurrentGrid(CalendarVisualMode.Days).ColumnsCount;
      DayCell sender1 = new DayCell();
      DayGrid dayGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
      ArrayList cellsCollection = dayGrid.CellsCollection;
      if (e.KeyboardDevice.Modifiers != ModifierKeys.Control)
        num = this.ValidateFocusIndex();
      if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Space))
      {
        int focusCellIndex = this.GetFocusCellIndex();
        if (e.Key == Key.Down)
          focusCellIndex += columnsCount;
        else if (e.Key == Key.Up)
          focusCellIndex -= columnsCount;
        else if (e.Key == Key.Left)
          focusCellIndex += -1;
        else if (e.Key == Key.Right)
          ++focusCellIndex;
        else if (e.Key == Key.Space)
        {
          DayCell dayCell = cellsCollection[focusCellIndex] as DayCell;
          if (dayCell.IsFocused)
          {
            if (dayCell.IsSelected)
            {
              this.SelectedDates.Remove(dayCell.Date.ToDateTime(this.Calendar));
              if (!dayCell.IsSelected)
                dayCell.IsDate = false;
            }
            else
              this.SelectedDates.Add(dayCell.Date.ToDateTime(this.Calendar));
            this.ProcessSelectedDatesCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList) this.SelectedDates));
          }
        }
        if (focusCellIndex >= 0 && focusCellIndex < cellsCollection.Count || (focusCellIndex < 0 || focusCellIndex >= cellsCollection.Count) && (e.Key == Key.Up || e.Key == Key.Down))
          this.SetKeyboardfocus(focusCellIndex, cellsCollection, e.KeyboardDevice.Modifiers, e.Key, dayGrid);
        e.Handled = true;
      }
      else
      {
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
        {
          if (e.SystemKey != Key.Up)
            return;
          this.ProcessUpKey();
          e.Handled = true;
          return;
        }
        switch (e.Key)
        {
          case Key.Return:
            flag = true;
            num = this.ValidateFocusIndex();
            e.Handled = true;
            break;
          case Key.Prior:
            int previousCell1 = (dayGrid.CellsCollection[num] as DayCell).Date.Day;
            this.ProcessPageUpKey();
            if (previousCell1 > DateTime.DaysInMonth(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth))
              previousCell1 = DateTime.DaysInMonth(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth);
            num = this.GetPreviousSelectedCell((CalendarEditGrid) dayGrid, previousCell1);
            e.Handled = true;
            break;
          case Key.Next:
            int previousCell2 = (dayGrid.CellsCollection[num] as DayCell).Date.Day;
            this.ProcessPageDownKey();
            if (previousCell2 > DateTime.DaysInMonth(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth))
              previousCell2 = DateTime.DaysInMonth(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth);
            num = this.GetPreviousSelectedCell((CalendarEditGrid) dayGrid, previousCell2);
            e.Handled = true;
            break;
          case Key.End:
            num = this.ProcessEndKey((CalendarEditGrid) dayGrid, num);
            e.Handled = true;
            break;
          case Key.Home:
            num = this.ProcessHomeKey((CalendarEditGrid) dayGrid, num);
            e.Handled = true;
            break;
          case Key.Left:
            bool moved1;
            num = this.SetFocusedCellIndex(num, -1, Orientation.Horizontal, out moved1);
            if (moved1)
              dayGrid = this.FollowingDayGrid;
            e.Handled = true;
            break;
          case Key.Up:
            num -= columnsCount;
            if (num < 0 || num >= 0 && num < cellsCollection.Count && !(cellsCollection[num] as DayCell).IsCurrentMonth)
            {
              bool moved2;
              num = this.SetFocusedCellIndex(num, -1, Orientation.Vertical, out moved2);
              if (moved2)
                dayGrid = this.FollowingDayGrid;
            }
            e.Handled = true;
            break;
          case Key.Right:
            bool moved3;
            num = this.SetFocusedCellIndex(num, 1, Orientation.Horizontal, out moved3);
            if (moved3)
              dayGrid = this.FollowingDayGrid;
            e.Handled = true;
            break;
          case Key.Down:
            num += columnsCount;
            if (num >= cellsCollection.Count || num >= 0 && num < cellsCollection.Count && !(cellsCollection[num] as DayCell).IsCurrentMonth)
            {
              bool moved4;
              num = this.SetFocusedCellIndex(num, 1, Orientation.Vertical, out moved4);
              if (moved4)
                dayGrid = this.FollowingDayGrid;
            }
            e.Handled = true;
            break;
        }
      }
      if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        return;
      if (!flag)
        dayGrid.FocusedCellIndex = num;
      if (num >= 0 && num < dayGrid.CellsCollection.Count)
        sender1 = (DayCell) dayGrid.CellsCollection[num];
      if (sender1 != null && (e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Down))
      {
        if (sender1.IsInvalidDate)
        {
          for (; sender1.IsInvalidDate && num > 0 && sender1.Visibility == Visibility.Visible && sender1.Date.ToDateTime(this.Calendar) >= this.MinDate && sender1.Date.ToDateTime(this.Calendar) <= this.MaxDate; sender1 = (DayCell) dayGrid.CellsCollection[num])
            num = e.Key == Key.Left || e.Key == Key.Up ? num - 1 : num + 1;
          if (num > 0)
            dayGrid.FocusedCellIndex = num;
        }
        else if (sender1.IsToday && !sender1.Focusable)
          sender1.Focusable = true;
      }
      if (sender1.Visibility != Visibility.Visible)
        return;
      if (flag)
        this.OnDayCellClick(sender1, CalendarEdit.ChangeMonthMode.Enabled, Keyboard.Modifiers);
      else
        this.OnDayCellClick(sender1, CalendarEdit.ChangeMonthMode.Disabled, Keyboard.Modifiers);
    }
    else
    {
      if (Keyboard.Modifiers == ModifierKeys.Alt || Keyboard.GetKeyStates(Key.Right) == KeyStates.Down && Keyboard.GetKeyStates(Key.Left) == KeyStates.Down)
        return;
      e.Handled = true;
    }
  }

  private void VisualModeGrid_OnKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Handled)
      return;
    if (!this.IsStoryboardActive(this.mmoveStoryboard) && !this.IsStoryboardActive(this.mvisualModeStoryboard))
    {
      CalendarEditGrid currentGrid = this.FindCurrentGrid(this.VisualMode);
      int index1 = currentGrid.FocusedCellIndex;
      int count = currentGrid.CellsCollection.Count;
      int columnsCount = currentGrid.ColumnsCount;
      if (index1 < 0)
        index1 = currentGrid.CellsCollection.Count - 1;
      if (index1 > currentGrid.CellsCollection.Count - 1)
        index1 = 0;
      int num = 3;
      Cell cells1 = (Cell) currentGrid.CellsCollection[index1];
      if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
      {
        switch (e.SystemKey)
        {
          case Key.Up:
            this.ProcessUpKey();
            e.Handled = true;
            break;
          case Key.Down:
            this.ProcessViewNavigation(cells1);
            cells1.Focus();
            e.Handled = true;
            break;
        }
      }
      else
      {
        switch (e.Key)
        {
          case Key.Return:
            this.ProcessViewNavigation(currentGrid.CellsCollection[currentGrid.FocusedCellIndex] as Cell);
            e.Handled = true;
            break;
          case Key.Prior:
            int focusedCellIndex1 = currentGrid.FocusedCellIndex;
            this.ProcessPageUpKey();
            currentGrid.FocusedCellIndex = focusedCellIndex1;
            e.Handled = true;
            break;
          case Key.Next:
            int focusedCellIndex2 = currentGrid.FocusedCellIndex;
            this.ProcessPageDownKey();
            currentGrid.FocusedCellIndex = focusedCellIndex2;
            e.Handled = true;
            break;
          case Key.End:
            currentGrid.FocusedCellIndex = this.ProcessEndKey(currentGrid, currentGrid.FocusedCellIndex);
            e.Handled = true;
            break;
          case Key.Home:
            currentGrid.FocusedCellIndex = this.ProcessHomeKey(currentGrid, currentGrid.FocusedCellIndex);
            e.Handled = true;
            break;
          case Key.Left:
            int index2;
            if ((index2 = index1 - 1) <= 0)
            {
              CalendarEdit.PrevCommand.Execute((object) null, (IInputElement) this);
              index2 = count - 1;
            }
            Cell cells2 = currentGrid.CellsCollection[index2] as Cell;
            if (!this.ValidateCurrentCell(cells2) && !this.ProcesssGridNavigation(cells2, e.Key))
              currentGrid.FocusedCellIndex = index2;
            e.Handled = true;
            break;
          case Key.Up:
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
              if (this.CalendarStyle == CalendarStyle.Vista)
              {
                if (this.IsFlagValueSet(this.ViewMode))
                  this.NextMode = this.GetNextValue(this.VisualMode);
                if (this.ViewMode == CalendarVisualMode.All || this.IsNextModeSet && this.VisualMode != this.NextMode)
                  this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
                else if (this.IsFlagValueSet(this.ViewMode) && !this.IsNextModeSet && this.VisualMode != this.NextMode)
                  this.ChangeMode(CalendarEdit.ChangeVisualModeDirection.Up);
              }
              e.Handled = true;
              return;
            }
            int index3 = index1 - columnsCount;
            if (index3 < 0)
            {
              this.ProcessPageUpKey();
              if (this.VisibleData.VisibleYear > this.MinDate.Year)
                index3 += columnsCount * num;
              else
                index3 += columnsCount;
            }
            Cell cells3 = currentGrid.CellsCollection[index3] as Cell;
            if (!this.ValidateCurrentCell(cells3) && !this.ProcesssGridNavigation(cells3, e.Key))
              currentGrid.FocusedCellIndex = index3;
            currentGrid.FocusedCellIndex = index3;
            e.Handled = true;
            break;
          case Key.Right:
            int index4;
            if ((index4 = index1 + 1) >= count)
            {
              CalendarEdit.NextCommand.Execute((object) null, (IInputElement) this);
              index4 = 0;
            }
            Cell cells4 = currentGrid.CellsCollection[index4] as Cell;
            if (!this.ValidateCurrentCell(cells4) && !this.ProcesssGridNavigation(cells4, e.Key))
              currentGrid.FocusedCellIndex = index4;
            e.Handled = true;
            break;
          case Key.Down:
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
              this.ProcessViewNavigation(cells1);
              cells1.Focus();
              e.Handled = true;
              return;
            }
            int index5 = index1 + columnsCount;
            if (index5 >= count)
            {
              this.ProcessPageDownKey();
              if (this.VisibleData.VisibleYear < this.MaxDate.Year)
                index5 -= columnsCount * num;
              else
                index5 -= columnsCount;
            }
            Cell cells5 = currentGrid.CellsCollection[index5] as Cell;
            if (!this.ValidateCurrentCell(cells5) && !this.ProcesssGridNavigation(cells5, e.Key))
              currentGrid.FocusedCellIndex = index5;
            e.Handled = true;
            break;
        }
        Cell cells6 = currentGrid.CellsCollection[currentGrid.FocusedCellIndex] as Cell;
        for (int index6 = 0; index6 < currentGrid.CellsCollection.Count; ++index6)
        {
          Cell cells7 = currentGrid.CellsCollection[index6] as Cell;
          if (index6 == currentGrid.FocusedCellIndex)
          {
            cells7.IsSelected = true;
            cells7.Focus();
          }
          else
            cells7.IsSelected = false;
        }
      }
    }
    else
    {
      if (Keyboard.Modifiers == ModifierKeys.Alt || Keyboard.GetKeyStates(Key.Right) == KeyStates.Down && Keyboard.GetKeyStates(Key.Left) == KeyStates.Down)
        return;
      e.Handled = true;
    }
  }

  private void ProcessUpKey()
  {
    if (this.CalendarStyle != CalendarStyle.Vista)
      return;
    if (this.IsFlagValueSet(this.ViewMode))
      this.NextMode = this.GetNextValue(this.VisualMode);
    if (this.ViewMode == CalendarVisualMode.All || this.IsNextModeSet && this.VisualMode != this.NextMode)
    {
      this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
    }
    else
    {
      if (!this.IsFlagValueSet(this.ViewMode) || this.IsNextModeSet || this.VisualMode == this.NextMode)
        return;
      this.ChangeMode(CalendarEdit.ChangeVisualModeDirection.Up);
    }
  }

  private void ProcessPageUpKey()
  {
    switch (this.VisualMode)
    {
      case CalendarVisualMode.Days:
        this.AddMonth(-1);
        break;
      case CalendarVisualMode.Months:
        this.AddYear(-1);
        break;
      case CalendarVisualMode.Years:
        this.AddYear(-10);
        break;
      case CalendarVisualMode.YearsRange:
        this.AddYear(-100);
        break;
    }
  }

  private void ProcessPageDownKey()
  {
    switch (this.VisualMode)
    {
      case CalendarVisualMode.Days:
        this.AddMonth(1);
        break;
      case CalendarVisualMode.Months:
        this.AddYear(1);
        break;
      case CalendarVisualMode.Years:
        this.AddYear(10);
        break;
      case CalendarVisualMode.YearsRange:
        this.AddYear(100);
        break;
    }
  }

  private int ProcessHomeKey(CalendarEditGrid currentGrid, int previousSelectedIndex)
  {
    DateTime date = new DateTime(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    for (int index = 0; index < currentGrid.CellsCollection.Count; ++index)
    {
      Cell cells1 = currentGrid.CellsCollection[index] as Cell;
      if (date.Year < this.MinDate.Year)
        date = this.MinDate;
      if (this.VisualMode == CalendarVisualMode.Days && currentGrid is DayGrid)
      {
        DayCell cells2 = (DayCell) currentGrid.CellsCollection[index];
        if (cells2.IsCurrentMonth)
          return cells2 != null && !cells2.IsInvalidDate ? index : previousSelectedIndex;
      }
      DateTime? nullable;
      if (this.VisualMode == CalendarVisualMode.Months)
      {
        if ((cells1 as MonthCell).MonthNumber == this.MinDate.Month)
          return index;
      }
      else if (this.VisualMode == CalendarVisualMode.Years)
      {
        if ((cells1 as YearCell).Year < this.MinDate.Year)
          return previousSelectedIndex;
        nullable = new DateTime?(new DateTime(this.DecadeOfDate(date, this.Calendar), 1, 1));
        if ((cells1 as YearCell).Year == nullable.Value.Year)
          return index;
      }
      else if (this.VisualMode == CalendarVisualMode.YearsRange)
      {
        if ((cells1 as YearRangeCell).Years.StartYear < this.MinDate.Year)
          return previousSelectedIndex;
        nullable = new DateTime?(new DateTime(this.CenturyOfDate(date, this.Calendar), 1, 1));
        if ((cells1 as YearRangeCell).Years.StartYear == nullable.Value.Year)
          return index;
      }
    }
    return 0;
  }

  private int ProcessEndKey(CalendarEditGrid currentGrid, int previousSelectedIndex)
  {
    DateTime date = new DateTime(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    for (int index = currentGrid.CellsCollection.Count - 1; index >= 0; --index)
    {
      Cell cells1 = currentGrid.CellsCollection[index] as Cell;
      DateTime? nullable;
      if (this.VisualMode == CalendarVisualMode.Days)
      {
        DayCell cells2 = (DayCell) currentGrid.CellsCollection[index];
        if (cells2.IsCurrentMonth)
          return cells2 != null && !cells2.IsInvalidDate ? index : previousSelectedIndex;
      }
      else if (this.VisualMode == CalendarVisualMode.Months)
      {
        if ((cells1 as MonthCell).MonthNumber == this.MaxDate.Month)
          return index;
      }
      else if (this.VisualMode == CalendarVisualMode.Years)
      {
        if ((cells1 as YearCell).Year > this.MaxDate.Year)
          return previousSelectedIndex;
        nullable = new DateTime?(new DateTime(this.DecadeOfDate(date, this.Calendar) + 9, 1, 1));
        if ((cells1 as YearCell).Year == nullable.Value.Year)
          return index;
      }
      else if (this.VisualMode == CalendarVisualMode.YearsRange)
      {
        if ((cells1 as YearRangeCell).Years.StartYear > this.MaxDate.Year)
          return previousSelectedIndex;
        nullable = new DateTime?(new DateTime(this.CenturyOfDate(date, this.Calendar) + 90, 1, 1));
        if ((cells1 as YearRangeCell).Years.StartYear == nullable.Value.Year)
          return index;
      }
    }
    return 0;
  }

  private void ProcessViewNavigation(Cell cell)
  {
    if (this.VisualMode == CalendarVisualMode.Months)
    {
      if (cell is MonthCell)
        this.OnMonthCellClick((MonthCell) cell);
      this.Date = this.Date.Day <= DateTime.DaysInMonth(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth) ? new DateTime(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.Date.Day) : new DateTime(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, 1);
      if (this.FindCurrentGrid(CalendarVisualMode.Days) != null && this.Calendar != null)
      {
        foreach (DayCell cells in this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection)
        {
          if (cells.Date.ToDateTime(this.Calendar) == this.Date)
            cells.Focus();
        }
      }
    }
    if (this.VisualMode == CalendarVisualMode.Years && cell is YearCell)
      this.OnYearCellClick((YearCell) cell);
    if (this.VisualMode != CalendarVisualMode.YearsRange || !(cell is YearRangeCell))
      return;
    this.OnYearRangeCellClick((YearRangeCell) cell);
  }

  private int CenturyOfDate(DateTime date, System.Globalization.Calendar calendar)
  {
    date = Syncfusion.Windows.Controls.DateTimeHelper.GetValidDateTime(date, calendar);
    return calendar.GetYear(date) - calendar.GetYear(date) % 100 == 0 ? 1 : calendar.GetYear(date) - calendar.GetYear(date) % 100;
  }

  private int DecadeOfDate(DateTime date, System.Globalization.Calendar calendar)
  {
    date = Syncfusion.Windows.Controls.DateTimeHelper.GetValidDateTime(date, calendar);
    return calendar.GetYear(date) - calendar.GetYear(date) % 10 == 0 ? 1 : calendar.GetYear(date) - calendar.GetYear(date) % 10;
  }

  private int GetPreviousSelectedCell(CalendarEditGrid currentGrid, int previousCell)
  {
    for (int index = 0; index < currentGrid.CellsCollection.Count; ++index)
    {
      DayCell cells = (DayCell) currentGrid.CellsCollection[index];
      if (cells.IsCurrentMonth && cells.Date.Day == previousCell)
        return index;
    }
    return 0;
  }

  private bool ProcesssGridNavigation(Cell cell, Key key)
  {
    DateTime date = new DateTime(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    if (this.VisualMode == CalendarVisualMode.Years)
    {
      switch (key)
      {
        case Key.Left:
        case Key.Up:
          DateTime dateTime1 = new DateTime(this.DecadeOfDate(date, this.Calendar), 1, 1);
          if ((cell as YearCell).Year < dateTime1.Year)
          {
            this.Move(CalendarEdit.MoveDirection.Prev);
            return true;
          }
          break;
        case Key.Right:
        case Key.Down:
          DateTime dateTime2 = new DateTime(this.DecadeOfDate(date, this.Calendar) + 9, 1, 1);
          if ((cell as YearCell).Year > dateTime2.Year)
          {
            this.Move(CalendarEdit.MoveDirection.Next);
            return true;
          }
          break;
      }
    }
    else if (this.VisualMode == CalendarVisualMode.YearsRange)
    {
      switch (key)
      {
        case Key.Left:
        case Key.Up:
          DateTime dateTime3 = new DateTime(this.CenturyOfDate(date, this.Calendar), 1, 1);
          if ((cell as YearRangeCell).Years.StartYear < dateTime3.Year)
          {
            this.Move(CalendarEdit.MoveDirection.Prev);
            return true;
          }
          break;
        case Key.Right:
        case Key.Down:
          DateTime dateTime4 = new DateTime(this.CenturyOfDate(date, this.Calendar) + 90, 1, 1);
          if ((cell as YearRangeCell).Years.StartYear > dateTime4.Year)
          {
            this.Move(CalendarEdit.MoveDirection.Next);
            return true;
          }
          break;
      }
    }
    return false;
  }

  private bool ValidateCurrentCell(Cell cell)
  {
    if (this.VisualMode == CalendarVisualMode.Months)
    {
      if (cell is MonthCell && (cell as MonthCell).MonthNumber > this.MaxDate.Month || (cell as MonthCell).MonthNumber < this.MinDate.Month)
        return true;
    }
    else if (this.VisualMode == CalendarVisualMode.Years)
    {
      if (cell is YearCell && (cell as YearCell).Year >= this.MaxDate.Year || (cell as YearCell).Year <= this.MinDate.Year)
        return true;
    }
    else if (this.VisualMode == CalendarVisualMode.YearsRange && (cell is YearRangeCell && (cell as YearRangeCell).Years.StartYear >= this.MaxDate.Year || (cell as YearRangeCell).Years.StartYear <= this.MinDate.Year))
      return true;
    return false;
  }

  private void YearRangeCell_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleYearRangeCellUp((YearRangeCell) sender);
    this.mpressedCell = (Cell) null;
    e.Handled = true;
  }

  private void HandleYearRangeCellUp(YearRangeCell releasedCell)
  {
    if (this.ViewMode == CalendarVisualMode.YearsRange && !this.IsFlagValueSet(this.ViewMode) || this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.WeekNumbers)
    {
      ArrayList arrayList = new ArrayList();
      ArrayList cellsCollection = this.FindCurrentGrid(CalendarVisualMode.YearsRange).CellsCollection;
      for (int index = 0; index < cellsCollection.Count; ++index)
      {
        YearRangeCell yearRangeCell = (YearRangeCell) cellsCollection[index];
        if (yearRangeCell.IsSelected)
          yearRangeCell.IsSelected = false;
      }
      releasedCell.IsSelected = true;
      this.yearrangepress = true;
      this.Date = new DateTime(releasedCell.Years.StartYear, 1, 1);
    }
    else if (this.mpressedCell == releasedCell)
      this.OnYearRangeCellClick(releasedCell);
    this.mpressedCell = (Cell) null;
  }

  internal bool CanScrollToView()
  {
    return (this.ViewMode != CalendarVisualMode.YearsRange || this.IsFlagValueSet(this.ViewMode)) && this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.WeekNumbers && (this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.Years && this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.WeekNumbers || !this.IsFlagValueSet(this.ViewMode)) && this.ViewMode != CalendarVisualMode.Years && (!this.IsFlagValueSet(this.ViewMode) || this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.Months && this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.WeekNumbers) && this.ViewMode != CalendarVisualMode.Months && (this.GetPreviousValue(this.VisualMode) != CalendarVisualMode.WeekNumbers || !this.IsFlagValueSet(this.ViewMode)) && this.ViewMode != CalendarVisualMode.WeekNumbers;
  }

  private void YearRangeCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.mpressedCell = (Cell) sender;
    e.Handled = true;
  }

  private void YearCell_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleYearCellUp((YearCell) sender);
    this.mpressedCell = (Cell) null;
    e.Handled = true;
  }

  private void HandleYearCellUp(YearCell releasedCell)
  {
    if ((this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.Years || this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.WeekNumbers) && this.IsFlagValueSet(this.ViewMode) || this.ViewMode == CalendarVisualMode.Years)
    {
      ArrayList arrayList = new ArrayList();
      ArrayList cellsCollection = this.FindCurrentGrid(CalendarVisualMode.Years).CellsCollection;
      for (int index = 0; index < cellsCollection.Count; ++index)
      {
        YearCell yearCell = (YearCell) cellsCollection[index];
        if (yearCell.IsSelected)
          yearCell.IsSelected = false;
      }
      releasedCell.IsSelected = true;
      this.yearpressed = true;
      this.Date = new DateTime(releasedCell.Year, 1, 1);
    }
    else if (this.mpressedCell == releasedCell)
      this.OnYearCellClick(releasedCell);
    this.mpressedCell = (Cell) null;
  }

  private void YearCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.mpressedCell = (Cell) sender;
    e.Handled = true;
  }

  private void MonthCell_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleMonthCellUp((MonthCell) sender);
    e.Handled = true;
  }

  private void HandleMonthCellUp(MonthCell releasedCell)
  {
    if (this.IsFlagValueSet(this.ViewMode) && (this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.Months || this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.WeekNumbers) || this.ViewMode == CalendarVisualMode.Months)
    {
      ArrayList cellsCollection = this.FindCurrentGrid(CalendarVisualMode.Months).CellsCollection;
      for (int index = 0; index < cellsCollection.Count; ++index)
      {
        MonthCell monthCell = (MonthCell) cellsCollection[index];
        if (monthCell.IsSelected)
          monthCell.IsSelected = false;
      }
      releasedCell.IsSelected = true;
      this.monthpressed = true;
      this.Date = new DateTime(this.VisibleData.VisibleYear, releasedCell.MonthNumber, 1);
    }
    else if (this.mpressedCell == releasedCell)
      this.OnMonthCellClick(releasedCell);
    this.mpressedCell = (Cell) null;
  }

  private void MonthCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleMonthCellDown((MonthCell) sender);
    e.Handled = true;
  }

  private void HandleMonthCellDown(MonthCell currentCell)
  {
    if (!this.DisableDateSelection)
    {
      this.mpressedCell = (Cell) currentCell;
    }
    else
    {
      this.mpressedCell = (Cell) currentCell;
      this.OnMonthCellClick(currentCell);
    }
  }

  private void WeekNumberCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleWeekNumberCellDown((WeekNumberCell) sender, (TextBlock) e.OriginalSource);
    e.Handled = true;
  }

  private void HandleWeekNumberCellDown(WeekNumberCell currentCell, TextBlock weekNumberBlock)
  {
    if (!this.IsShowWeekNumbers)
      return;
    this.mpressedCell = (Cell) currentCell;
    try
    {
      WeekNumberCell sender = currentCell;
      CalendarEdit.clickedweeknumber = weekNumberBlock.Text;
      this.wcellClicked = true;
      if (this.Culture.Equals((object) new CultureInfo("ar-SA")))
        return;
      this.FollowingWeekNumbersGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      if (this.mpressedCell == sender)
      {
        this.OnWeekNumbersCellClick(sender);
        this.ShowWeekNumbersForYearContainer();
      }
      this.mpressedCell = (Cell) null;
    }
    catch (Exception ex)
    {
    }
  }

  private void WeekNumberCellPanel_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled || !(sender is WeekNumberCellPanel))
      return;
    this.HandleWeekNumberCellPanelDown(sender as WeekNumberCellPanel);
    e.Handled = true;
  }

  private void HandleWeekNumberCellPanelDown(WeekNumberCellPanel weekNumberCellPanel)
  {
    this.mpressedCell = (Cell) weekNumberCellPanel;
    CalendarEdit.clickedweeknumber = weekNumberCellPanel.WeekNumber;
    CultureInfo cultureInfo = new CultureInfo("ar-SA");
    if (this.GetPreviousValue(this.VisualMode) == CalendarVisualMode.WeekNumbers && this.IsFlagValueSet(this.ViewMode) || this.ViewMode == CalendarVisualMode.WeekNumbers)
    {
      ArrayList arrayList = new ArrayList();
      ArrayList cellsCollection = this.FindCurrentGrid(this.VisualMode).CellsCollection;
      for (int index = 0; index < cellsCollection.Count; ++index)
      {
        WeekNumberCellPanel weekNumberCellPanel1 = (WeekNumberCellPanel) cellsCollection[index];
        if (weekNumberCellPanel1.IsSelected)
          weekNumberCellPanel1.IsSelected = false;
      }
      int month = this.GetMonth(this.VisibleData.VisibleYear, Convert.ToInt32(weekNumberCellPanel.WeekNumber));
      weekNumberCellPanel.IsSelected = true;
      this.Date = new DateTime(this.VisibleData.VisibleYear, month, 1);
    }
    else
    {
      if (this.Culture.Equals((object) cultureInfo))
        return;
      this.FollowingWeekNumbersGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      if (this.mpressedCell == weekNumberCellPanel)
        this.OnWeekNumberCellPanelClick(weekNumberCellPanel);
      this.mpressedCell = (Cell) null;
    }
  }

  internal int GetMonth(int Year, int Week)
  {
    DateTime time = new DateTime(Year, 1, 1);
    time.AddDays((double) ((Week - 1) * 7));
    int num = DateTime.IsLeapYear(Year) ? 366 : 365;
    for (int index = 0; index <= num; ++index)
    {
      if (CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == Week)
        return time.Month;
      time = time.AddDays(1.0);
    }
    return 0;
  }

  private void OnWeekNumberCellPanelClick(WeekNumberCellPanel sender)
  {
    this.UpdateWeekNumbersContainer();
    int int16 = (int) Convert.ToInt16(CalendarEdit.clickedweeknumber);
    System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.Calendar;
    DateTime time = new DateTime(this.VisibleData.VisibleYear, 1, 1);
    int num = (int) (1 - time.DayOfWeek);
    DateTime dateTime1 = time.AddDays((double) num);
    CalendarWeekRule calendarWeekRule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
    if (dateTime1.DayOfWeek == DayOfWeek.Monday && calendar.GetWeekOfYear(time, calendarWeekRule, DayOfWeek.Monday) <= 1)
      --int16;
    DateTime dateTime2 = dateTime1.AddDays((double) (int16 * 7));
    this.VisualMode = CalendarVisualMode.Days;
    this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days);
    VisibleDate date = this.VisibleData.VisibleYear != dateTime2.Year ? (Convert.ToInt32(CalendarEdit.clickedweeknumber) != 53 ? new VisibleDate(this.VisibleData.VisibleYear, dateTime2.Month % 12 + 1, this.VisibleData.VisibleDay) : new VisibleDate(this.VisibleData.VisibleYear, dateTime2.Month % 12 + 11, this.VisibleData.VisibleDay)) : new VisibleDate(this.VisibleData.VisibleYear, dateTime2.Month, this.VisibleData.VisibleDay);
    this.DayNamesGrid.Visibility = Visibility.Visible;
    this.ChangeVisualModePreview(date);
    this.HideWeekNumbersForYearContainer();
    this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
    this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
    if (!this.mouseweeknumberscroll)
      return;
    this.wcurrentweekNumbersContainer.Visibility = Visibility.Hidden;
    this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
  }

  private void DayCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleDayCellDown((DayCell) sender);
    e.Handled = true;
  }

  private void HandleDayCellDown(DayCell currentCell)
  {
    if (Keyboard.Modifiers != ModifierKeys.Shift && this.m_shiftDateChangeEnabled)
      this.m_shiftDate = this.Date;
    this.mpressedCell = (Cell) currentCell;
    if (Keyboard.Modifiers == ModifierKeys.Control)
      this.isControlSelection = true;
    this.OnDayCellClick(currentCell, CalendarEdit.ChangeMonthMode.Disabled, Keyboard.Modifiers);
  }

  private void DayCell_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (e.Handled)
      return;
    this.HandleDayCellUp((DayCell) sender);
    e.Handled = true;
  }

  private void HandleDayCellUp(DayCell releasedCell)
  {
    if (releasedCell == this.mpressedCell && !releasedCell.IsCurrentMonth)
      this.ScrollMonth(releasedCell.Date.Month);
    this.mpressedCell = (Cell) null;
  }

  private void DayNameCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!e.Handled)
      this.HandleDayNameCellDown(sender as UIElement);
    e.Handled = true;
  }

  private void HandleDayNameCellDown(UIElement DayNameCell)
  {
    if (!this.AllowSelection || !this.AllowMultiplySelection || ModifierKeys.Control != Keyboard.Modifiers)
      return;
    ArrayList arrayList = new ArrayList();
    int column = Grid.GetColumn(DayNameCell);
    DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
    for (int index = 0; index < currentGrid.RowsCount; ++index)
    {
      DayCell cells = (DayCell) currentGrid.CellsCollection[column];
      if (cells.Visibility != Visibility.Hidden)
        arrayList.Add((object) cells);
      column += currentGrid.ColumnsCount;
    }
    foreach (DayCell dayCell in arrayList)
    {
      DateTime dateTime = dayCell.Date.ToDateTime(this.Calendar);
      if (this.SelectionRangeMode == SelectionRangeMode.WholeColumn)
      {
        if (this.SelectedDates != null)
          this.SelectedDates.Add(dateTime);
      }
      else if (dayCell.IsCurrentMonth && this.SelectedDates != null)
        this.SelectedDates.Add(dateTime);
    }
  }

  private void DayNamesGrid_OnMouseLeave(object sender, MouseEventArgs e)
  {
    if (!e.Handled)
      this.Highlight(((DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days)).SelectionBorder, CalendarEdit.HighlightSate.Stop);
    e.Handled = true;
  }

  private void DayNamesGrid_OnMouseMove(object sender, MouseEventArgs e)
  {
    if (!e.Handled && e.Source is DayNameCell)
    {
      int column = Grid.GetColumn((UIElement) e.Source);
      DayGrid currentGrid = (DayGrid) this.FindCurrentGrid(CalendarVisualMode.Days);
      Grid.SetColumn((UIElement) currentGrid.SelectionBorder, column);
      this.Highlight(currentGrid.SelectionBorder, CalendarEdit.HighlightSate.Begin);
    }
    e.Handled = true;
  }

  private void DayGrid_OnMouseMove(object sender, MouseEventArgs e)
  {
    if (!e.Handled && e.Source is DayCell source)
    {
      bool flag = false;
      foreach (DayCell cells in this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection)
      {
        if (cells.IsDate && cells != source)
          flag = true;
      }
      if (flag && e.LeftButton == MouseButtonState.Pressed)
      {
        this.m_shiftDateChangeEnabled = false;
        ModifierKeys modifiers = Keyboard.Modifiers != ModifierKeys.None ? Keyboard.Modifiers : ModifierKeys.Shift;
        this.OnDayCellClick(source, CalendarEdit.ChangeMonthMode.Disabled, modifiers);
      }
      else
        this.m_shiftDateChangeEnabled = true;
    }
    e.Handled = true;
  }

  private void SelectedDates_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.mbselectedDatesUpdateLocked || !this.IsInitializeComplete)
      return;
    if (e.Action != NotifyCollectionChangedAction.Reset)
      this.ProcessSelectedDatesCollectionChange(new NotifyCollectionChangedEventArgs(e.Action, (IList) this.SelectedDates));
    else
      this.ProcessSelectedDatesCollectionChange(e);
  }

  private void DateStyles_OnPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    StyleItem styleItem = (StyleItem) null;
    CalendarEdit.CollectionChangedAction action = CalendarEdit.CollectionChangedAction.Reset;
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        action = CalendarEdit.CollectionChangedAction.Add;
        styleItem = (StyleItem) e.NewItems[0];
        break;
      case NotifyCollectionChangedAction.Remove:
        action = CalendarEdit.CollectionChangedAction.Remove;
        styleItem = (StyleItem) e.OldItems[0];
        break;
      case NotifyCollectionChangedAction.Reset:
        action = CalendarEdit.CollectionChangedAction.Reset;
        break;
    }
    if (this.CurrentDayGrid != null)
      this.SetDateStyles(this.CurrentDayGrid, action, styleItem);
    if (this.FollowingDayGrid == null)
      return;
    this.SetDateStyles(this.FollowingDayGrid, action, styleItem);
  }

  private void DateDataTemplates_OnPropertyChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    DataTemplateItem dataTemplateItem = (DataTemplateItem) null;
    CalendarEdit.CollectionChangedAction action = CalendarEdit.CollectionChangedAction.Reset;
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        action = CalendarEdit.CollectionChangedAction.Add;
        dataTemplateItem = (DataTemplateItem) e.NewItems[0];
        break;
      case NotifyCollectionChangedAction.Remove:
        action = CalendarEdit.CollectionChangedAction.Remove;
        dataTemplateItem = (DataTemplateItem) e.OldItems[0];
        break;
      case NotifyCollectionChangedAction.Reset:
        action = CalendarEdit.CollectionChangedAction.Reset;
        break;
    }
    if (this.CurrentDayGrid != null)
      this.SetDateDataTemplates(this.CurrentDayGrid, action, dataTemplateItem);
    if (this.FollowingDayGrid == null)
      return;
    this.SetDateDataTemplates(this.FollowingDayGrid, action, dataTemplateItem);
  }

  private void UpdateWeekNumbersContainer()
  {
    if (this.VisualModeInfo.NewMode == CalendarVisualMode.Days && this.ShowWeekNumbers)
      this.ShowWeekNumbersContainer();
    else
      this.HideWeekNumbersContainer();
  }

  public void NextCommandExecute(object sender, ExecutedRoutedEventArgs e)
  {
    if (this.VisualMode == CalendarVisualMode.Days && (this.ViewMode == CalendarVisualMode.Days || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)) || this.VisualMode == CalendarVisualMode.All && (this.ViewMode == CalendarVisualMode.All || this.ViewMode == CalendarVisualMode.Days))
    {
      if (this.IsStoryboardActive(this.mmonthStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
      {
        ++this.m_iscrollCounter;
      }
      else
      {
        this.isMonthNavigated = true;
        this.BeginMoving(CalendarEdit.MoveDirection.Next, 1);
        this.SelectedDates.Clear();
        this.SelectedDates.Add(this.Date);
        if (this.SelectedDates == null || this.SelectedDates.Count != 1)
          return;
        foreach (DayCell cells in this.FollowingDayGrid.CellsCollection)
        {
          if (cells.Date.Day > 0 && cells.Date.ToDateTime(this.Calendar) == this.SelectedDates[0].Date)
            cells.Focus();
        }
      }
    }
    else
    {
      if (this.IsStoryboardActive(this.mmoveStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
        return;
      this.Move(CalendarEdit.MoveDirection.Next);
    }
  }

  public void NextCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = true;
  }

  public void PrevCommandExecute(object sender, ExecutedRoutedEventArgs e)
  {
    if (this.VisualMode == CalendarVisualMode.Days && (this.ViewMode == CalendarVisualMode.Days || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)) || this.VisualMode == CalendarVisualMode.All && (this.ViewMode == CalendarVisualMode.All || this.ViewMode == CalendarVisualMode.Days))
    {
      if (this.IsStoryboardActive(this.mmonthStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
      {
        --this.m_iscrollCounter;
      }
      else
      {
        this.isMonthNavigated = true;
        this.BeginMoving(CalendarEdit.MoveDirection.Prev, -1);
        this.SelectedDates.Clear();
        this.SelectedDates.Add(this.Date);
        if (this.SelectedDates == null || this.SelectedDates.Count != 1)
          return;
        foreach (DayCell cells in this.FollowingDayGrid.CellsCollection)
        {
          if (cells.Date.Day > 0 && cells.Date.ToDateTime(this.Calendar) == this.SelectedDates[0].Date)
            cells.Focus();
        }
      }
    }
    else
    {
      if (this.IsStoryboardActive(this.mmoveStoryboard) || this.IsStoryboardActive(this.mvisualModeStoryboard))
        return;
      this.Move(CalendarEdit.MoveDirection.Prev);
    }
  }

  public void PrevCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = true;
  }

  public void UpCommandExecute(object sender, ExecutedRoutedEventArgs e)
  {
    if (this.IsStoryboardActive(this.mvisualModeStoryboard) || this.CalendarStyle != CalendarStyle.Vista)
      return;
    this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
  }

  public void UpCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = true;
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    if (!this.isTouchSelection)
    {
      if (Math.Abs(e.DeltaManipulation.Translation.X) > Math.Abs(e.DeltaManipulation.Translation.Y))
      {
        if (this.MonthChangeDirection == AnimationDirection.Horizontal)
        {
          if (e.DeltaManipulation.Translation.X < 0.0)
            CalendarEdit.NextCommand.Execute((object) null, (IInputElement) this);
          else if (e.DeltaManipulation.Translation.Y > 0.0)
            CalendarEdit.PrevCommand.Execute((object) null, (IInputElement) this);
        }
      }
      else if (this.MonthChangeDirection == AnimationDirection.Vertical)
      {
        if (e.DeltaManipulation.Translation.Y < 0.0)
          CalendarEdit.PrevCommand.Execute((object) null, (IInputElement) this);
        else if (e.DeltaManipulation.Translation.Y > 0.0)
          CalendarEdit.NextCommand.Execute((object) null, (IInputElement) this);
      }
    }
    base.OnManipulationDelta(e);
  }

  private void monthButton_StylusDown(object sender, StylusDownEventArgs e)
  {
    if (!this.IsStoryboardActive(this.mvisualModeStoryboard))
      this.HandleHeaderButtonDown();
    else
      e.Handled = true;
  }

  private void HandleHeaderButtonDown()
  {
    if (this.IsFlagValueSet(this.ViewMode))
      this.NextMode = this.GetNextValue(this.VisualMode);
    if (this.CalendarStyle == CalendarStyle.Standard)
    {
      if (this.VisualMode == CalendarVisualMode.Days && (this.ViewMode == CalendarVisualMode.All || this.ViewMode == CalendarVisualMode.Days))
        this.m_popup.Show();
      else if (this.ViewMode == CalendarVisualMode.All || this.IsNextModeSet && this.VisualMode != this.NextMode)
        this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
    }
    if (this.CalendarStyle == CalendarStyle.Vista && (this.ViewMode == CalendarVisualMode.All || this.IsNextModeSet && this.VisualMode != this.NextMode))
    {
      this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
    }
    else
    {
      if (!this.IsFlagValueSet(this.ViewMode) || this.IsNextModeSet || this.VisualMode == this.NextMode)
        return;
      this.ChangeMode(CalendarEdit.ChangeVisualModeDirection.Up);
    }
  }

  private void DayGrid_StylusMove(object sender, StylusEventArgs e)
  {
    if (e.Handled || !this.isTouchSelection || !(e.Source is DayCell source))
      return;
    bool flag = false;
    foreach (DayCell cells in this.FindCurrentGrid(CalendarVisualMode.Days).CellsCollection)
    {
      if (cells.IsDate && cells != source)
        flag = true;
    }
    if (flag)
    {
      this.m_shiftDateChangeEnabled = false;
      ModifierKeys modifiers = Keyboard.Modifiers != ModifierKeys.None ? Keyboard.Modifiers : ModifierKeys.Shift;
      this.OnDayCellClick(source, CalendarEdit.ChangeMonthMode.Disabled, modifiers);
    }
    else
      this.m_shiftDateChangeEnabled = true;
  }

  private void DayGrid_StylusUp(object sender, StylusEventArgs e)
  {
    this.updateSelection = true;
    DayCell dayCell = (DayCell) null;
    if (e.Source != null && e.Source is DayCell)
      dayCell = e.Source as DayCell;
    if (dayCell == null || dayCell == null || e.Handled)
      return;
    if (!dayCell.IsCurrentMonth)
      this.ScrollMonth(dayCell.Date.Month);
    if (!dayCell.IsSelected)
      dayCell.IsDate = false;
    this.mpressedCell = (Cell) null;
    e.Handled = true;
  }

  private void DayGrid_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
  {
    if (e.SystemGesture == SystemGesture.Tap)
    {
      this.isTouchSelection = true;
    }
    else
    {
      if (e.SystemGesture != SystemGesture.Drag)
        return;
      this.isTouchSelection = false;
    }
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Calendar = (System.Globalization.Calendar) null;
    this.yearpressed = false;
    this.monthpressed = false;
    this.yearrangepress = false;
    this.mousescroll = false;
    this.mouseDayscroll = false;
    this.mousemonthscroll = false;
    this.mouseyearscroll = false;
    this.mouseyearrangescroll = false;
    this.mouseweeknumberscroll = false;
    this.m_bsuspendEventFire = false;
    this.isMonthNavigated = false;
    this.m_cellClicked = false;
    this.wcellClicked = false;
    this.isTemplateApplied = false;
    if (this.m_popup != null)
    {
      this.m_popup.HidePopup -= new EventHandler<HidePopupEventArgs>(this.Popup_OnHidePopup);
      this.m_popup.Dispose();
      this.m_popup = (MonthPopup) null;
    }
    this.mpressedCell = (Cell) null;
    if (this.m_monthButton2 != null)
    {
      this.m_monthButton2.Dispose();
      this.m_monthButton2 = (MonthButton) null;
    }
    if (this.mselectedDatesList != null)
    {
      this.mselectedDatesList.Clear();
      this.mselectedDatesList = (List<Syncfusion.Windows.Shared.Date>) null;
    }
    if (this.mInvalidDateList != null)
    {
      this.mInvalidDateList.Clear();
      this.mInvalidDateList = (List<Syncfusion.Windows.Shared.Date>) null;
    }
    if (this.m_toolTipDates != null)
    {
      this.m_toolTipDates.Clear();
      this.m_toolTipDates = (Hashtable) null;
    }
    if (this.m_editMonthName != null)
      this.m_editMonthName = (TextBlock) null;
    if (this.m_timer != null)
    {
      this.m_timer.Tick -= new EventHandler(this.Timer_Tick);
      this.m_timer = (DispatcherTimer) null;
    }
    if (this.m_yearUpDownPanel != null)
      this.m_yearUpDownPanel = (StackPanel) null;
    if (this.m_yearUpDown != null)
      this.m_yearUpDown = (UpDown) null;
    if (this.wfollowweekNumbersContainer != null)
      this.wfollowweekNumbersContainer = (ContentPresenter) null;
    if (this.wcurrentweekNumbersContainer != null)
      this.wcurrentweekNumbersContainer = (ContentPresenter) null;
    if (this.m_weekNumbersContainer != null)
      this.m_weekNumbersContainer = (ContentPresenter) null;
    if (this.m_mainGrid != null)
      this.m_mainGrid = (Grid) null;
    if (this.m_mainGrid != null)
      this.m_mainGrid = (Grid) null;
    if (this.SelectedDates != null)
    {
      this.SelectedDates.Dispose();
      this.SelectedDates = (DatesCollection) null;
    }
    if (this.SpecialDates != null)
    {
      for (int index = 0; index < this.SpecialDates.Count; ++index)
        this.SpecialDates[index].Dispose();
      this.SpecialDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SpecialDates_CollectionChanged);
      this.SpecialDates.Clear();
      this.SpecialDates = (SpecialDatesCollection) null;
    }
    if (this.BlackoutDates != null)
    {
      this.BlackoutDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.BlackoutDates_CollectionChanged);
      this.BlackoutDates.Clear();
      this.BlackoutDates = (BlackDatesCollection) null;
    }
    if (this.CurrentWeekNumbersGrid != null)
    {
      this.CurrentWeekNumbersGrid.Dispose();
      this.CurrentWeekNumbersGrid = (WeekNumberGridPanel) null;
    }
    if (this.FollowingWeekNumbersGrid != null)
    {
      this.FollowingWeekNumbersGrid.Dispose();
      this.FollowingWeekNumbersGrid = (WeekNumberGridPanel) null;
    }
    if (this.WeekNumbersGrid != null)
    {
      this.WeekNumbersGrid.Dispose();
      this.WeekNumbersGrid = (WeekNumbersGrid) null;
    }
    if (this.DayNamesGrid != null)
    {
      this.DayNamesGrid.MouseMove -= new MouseEventHandler(this.DayNamesGrid_OnMouseMove);
      this.DayNamesGrid.MouseLeave -= new MouseEventHandler(this.DayNamesGrid_OnMouseLeave);
      this.DayNamesGrid.Dispose();
      this.DayNamesGrid = (DayNamesGrid) null;
    }
    if (this.CurrentDayGrid != null)
    {
      this.CurrentDayGrid.KeyDown -= new KeyEventHandler(this.DayGrid_OnKeyDown);
      this.CurrentDayGrid.MouseMove -= new MouseEventHandler(this.DayGrid_OnMouseMove);
      this.CurrentDayGrid.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
      this.CurrentDayGrid.StylusButtonUp -= new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
      this.CurrentDayGrid.StylusMove -= new StylusEventHandler(this.DayGrid_StylusMove);
      this.CurrentDayGrid.StylusUp -= new StylusEventHandler(this.DayGrid_StylusUp);
      this.CurrentDayGrid.StylusSystemGesture -= new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
      this.CurrentDayGrid.Dispose();
      this.CurrentDayGrid = (DayGrid) null;
    }
    if (this.FollowingDayGrid != null)
    {
      this.FollowingDayGrid.KeyDown -= new KeyEventHandler(this.DayGrid_OnKeyDown);
      this.FollowingDayGrid.MouseMove -= new MouseEventHandler(this.DayGrid_OnMouseMove);
      this.FollowingDayGrid.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentDayGrid_MouseLeftButtonUp);
      this.FollowingDayGrid.StylusMove -= new StylusEventHandler(this.DayGrid_StylusMove);
      this.FollowingDayGrid.StylusUp -= new StylusEventHandler(this.DayGrid_StylusUp);
      this.FollowingDayGrid.StylusSystemGesture -= new StylusSystemGestureEventHandler(this.DayGrid_StylusSystemGesture);
      this.FollowingDayGrid.StylusButtonUp -= new StylusButtonEventHandler(this.DayGrid_StylusButtonUp);
      this.FollowingDayGrid.Dispose();
      this.FollowingDayGrid = (DayGrid) null;
    }
    if (this.CurrentMonthGrid != null)
    {
      this.CurrentMonthGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.CurrentMonthGrid.Dispose();
      this.CurrentMonthGrid = (MonthGrid) null;
    }
    if (this.FollowingMonthGrid != null)
    {
      this.FollowingMonthGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.FollowingMonthGrid.Dispose();
      this.FollowingMonthGrid = (MonthGrid) null;
    }
    if (this.CurrentYearGrid != null)
    {
      this.CurrentYearGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.CurrentYearGrid.Dispose();
      this.CurrentYearGrid = (YearGrid) null;
    }
    if (this.FollowingYearGrid != null)
    {
      this.FollowingYearGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.FollowingYearGrid.Dispose();
      this.FollowingYearGrid = (YearGrid) null;
    }
    if (this.CurrentYearRangeGrid != null)
    {
      this.CurrentYearRangeGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.CurrentYearRangeGrid.Dispose();
      this.CurrentYearRangeGrid = (YearRangeGrid) null;
    }
    if (this.FollowingYearRangeGrid != null)
    {
      this.FollowingYearRangeGrid.KeyDown -= new KeyEventHandler(this.VisualModeGrid_OnKeyDown);
      this.FollowingYearRangeGrid.Dispose();
      this.FollowingYearRangeGrid = (YearRangeGrid) null;
    }
    if (this.DayCellMouseLeftButtonDown != null)
    {
      this.DayCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonDown);
      this.DayCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.DayCell_OnMouseLeftButtonUp);
      this.DayCellMouseLeftButtonDown = (MouseButtonEventHandler) null;
      this.DayCellMouseLeftButtonUp = (MouseButtonEventHandler) null;
    }
    if (this.MonthCellMouseLeftButtonDown != null)
    {
      this.MonthCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonDown);
      this.MonthCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.MonthCell_OnMouseLeftButtonUp);
      this.MonthCellMouseLeftButtonDown = (MouseButtonEventHandler) null;
      this.MonthCellMouseLeftButtonUp = (MouseButtonEventHandler) null;
    }
    if (this.YearCellMouseLeftButtonDown != null)
    {
      this.YearCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonDown);
      this.YearCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.YearCell_OnMouseLeftButtonUp);
      this.YearCellMouseLeftButtonDown = (MouseButtonEventHandler) null;
      this.YearCellMouseLeftButtonUp = (MouseButtonEventHandler) null;
    }
    if (this.YearRangeCellMouseLeftButtonDown != null)
    {
      this.YearRangeCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonDown);
      this.YearRangeCellMouseLeftButtonUp -= new MouseButtonEventHandler(this.YearRangeCell_OnMouseLeftButtonUp);
      this.YearRangeCellMouseLeftButtonDown = (MouseButtonEventHandler) null;
      this.YearRangeCellMouseLeftButtonUp = (MouseButtonEventHandler) null;
    }
    if (this.m_visualModeQueue != null)
    {
      this.m_visualModeQueue.Clear();
      this.m_visualModeQueue = (Queue<CalendarEdit.VisualModeHistory>) null;
    }
    if (this.mmonthStoryboard != null)
    {
      this.mmonthStoryboard.Completed -= new EventHandler(this.OnAnimationCompleted);
      this.mmonthStoryboard = (Storyboard) null;
    }
    if (this.mmoveStoryboard != null)
    {
      this.mmoveStoryboard.Completed -= new EventHandler(this.MoveStoryboard_Completed);
      this.mmoveStoryboard = (Storyboard) null;
    }
    if (this.mvisualModeStoryboard != null)
    {
      this.mvisualModeStoryboard.Completed -= new EventHandler(this.VisualModeStoryboard_OnCompleted);
      this.mvisualModeStoryboard = (Storyboard) null;
    }
    if (this.m_todayButton != null)
    {
      this.m_todayButton.Click -= new RoutedEventHandler(this.TodayButton_Click);
      this.m_todayButton = (Button) null;
    }
    if (this.m_nextButton != null)
    {
      this.m_nextButton.PreviewTouchDown -= new EventHandler<TouchEventArgs>(this.M_nextButton_PreviewTouchDown);
      this.m_nextButton.MouseEnter -= new MouseEventHandler(this.NavigateButton_OnMouseEnter);
      this.m_nextButton.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.M_nextButton_PreviewMouseLeftButtonUp);
      this.m_nextButton.Dispose();
      this.m_nextButton = (NavigateButton) null;
    }
    if (this.m_prevButton != null)
    {
      this.m_prevButton.PreviewTouchDown -= new EventHandler<TouchEventArgs>(this.M_prevButton_PreviewTouchDown);
      this.m_prevButton.MouseEnter -= new MouseEventHandler(this.NavigateButton_OnMouseEnter);
      this.m_prevButton.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.M_prevButton_PreviewMouseLeftButtonUp);
      this.m_prevButton.Dispose();
      this.m_prevButton = (NavigateButton) null;
    }
    if (this.m_monthButton1 != null)
    {
      this.m_monthButton1.StylusDown -= new StylusDownEventHandler(this.monthButton_StylusDown);
      this.m_monthButton1.MouseEnter -= new MouseEventHandler(this.MonthButton1_MouseEnter);
      this.m_monthButton1.MouseLeave -= new MouseEventHandler(this.MonthButton1_MouseLeave);
      this.m_monthButton1.MouseLeftButtonDown -= new MouseButtonEventHandler(this.MonthButton_OnMouseLeftButtonDown);
      this.m_monthButton1.Dispose();
      this.m_monthButton1 = (MonthButton) null;
    }
    if (this.WeekNumberCellPanelMouseLeftButtonDown != null)
    {
      this.WeekNumberCellPanelMouseLeftButtonDown -= new MouseButtonEventHandler(this.WeekNumberCellPanel_OnMouseLeftButtonDown);
      this.WeekNumberCellPanelMouseLeftButtonDown = (MouseButtonEventHandler) null;
    }
    if (this.WeekNumberCellMouseLeftButtonDown != null)
    {
      this.WeekNumberCellMouseLeftButtonDown -= new MouseButtonEventHandler(this.WeekNumberCell_OnMouseLeftButtonDown);
      this.WeekNumberCellMouseLeftButtonDown = (MouseButtonEventHandler) null;
    }
    this.PreviousScrollButtonTemplate = (ControlTemplate) null;
    this.NextScrollButtonTemplate = (ControlTemplate) null;
    this.DayCellsDataTemplate = (DataTemplate) null;
    this.DayCellsStyle = (Style) null;
    this.DayNameCellsStyle = (Style) null;
    this.DayCellsDataTemplateSelector = (DataTemplateSelector) null;
    this.DayNameCellsDataTemplateSelector = (DataTemplateSelector) null;
    if (this.DateDataTemplates != null)
    {
      this.DateDataTemplates.Clear();
      this.DateDataTemplates = (DataTemplatesDictionary) null;
    }
    if (this.DateStyles != null)
    {
      this.DateStyles.Clear();
      this.DateStyles = (StylesDictionary) null;
    }
    this.MouseDown -= new MouseButtonEventHandler(this.DayCell_MouseDown);
    this.Loaded -= new RoutedEventHandler(this.CalendarEdit_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.CalendarEdit_Unloaded);
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.SuppressFinalize((object) this);
    GC.Collect();
  }

  public void Dispose() => this.Dispose(true);

  public System.Globalization.Calendar Calendar
  {
    get => (System.Globalization.Calendar) this.GetValue(CalendarEdit.CalendarProperty);
    set => this.SetValue(CalendarEdit.CalendarProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [TypeConverter(typeof (CultureInfoTypeConverter))]
  [Browsable(false)]
  public CultureInfo Culture
  {
    get => (CultureInfo) this.GetValue(CalendarEdit.CultureProperty);
    set => this.SetValue(CalendarEdit.CultureProperty, (object) value);
  }

  [Browsable(false)]
  [Obsolete("This property is deprecated, the default CalendarStyle is changed to Vista style")]
  public CalendarStyle CalendarStyle
  {
    get => (CalendarStyle) this.GetValue(CalendarEdit.CalendarStyleProperty);
    set => this.SetValue(CalendarEdit.CalendarStyleProperty, (object) value);
  }

  public DateTime Date
  {
    get => (DateTime) this.GetValue(CalendarEdit.DateProperty);
    set => this.SetValue(CalendarEdit.DateProperty, (object) value);
  }

  public DatesCollection SelectedDates
  {
    get => (DatesCollection) this.GetValue(CalendarEdit.SelectedDatesProperty);
    set
    {
      if (!this.AllowSelection)
        return;
      this.SetValue(CalendarEdit.SelectedDatesProperty, (object) value);
    }
  }

  public BlackDatesCollection BlackoutDates
  {
    get => (BlackDatesCollection) this.GetValue(CalendarEdit.BlackoutDatesProperty);
    set => this.SetValue(CalendarEdit.BlackoutDatesProperty, (object) value);
  }

  public SpecialDatesCollection SpecialDates
  {
    get => (SpecialDatesCollection) this.GetValue(CalendarEdit.SpecialDatesProperty);
    set => this.SetValue(CalendarEdit.SpecialDatesProperty, (object) value);
  }

  private static void OnSpecialDatesCollectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    if (calendarEdit.SpecialDates == null)
      return;
    if (calendarEdit.CurrentDayGrid != null)
      calendarEdit.CurrentDayGrid.SetSpecialDatesTemplate();
    if (calendarEdit.FollowingDayGrid != null)
      calendarEdit.FollowingDayGrid.SetSpecialDatesTemplate();
    calendarEdit.SpecialDates.CollectionChanged -= new NotifyCollectionChangedEventHandler(calendarEdit.SpecialDates_CollectionChanged);
    calendarEdit.SpecialDates.CollectionChanged += new NotifyCollectionChangedEventHandler(calendarEdit.SpecialDates_CollectionChanged);
  }

  public Brush SelectionBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectionBorderBrushProperty);
    set => this.SetValue(CalendarEdit.SelectionBorderBrushProperty, (object) value);
  }

  [Obsolete("InValidDateBorderBrush is deprecated, use BlackoutDatesBorderBrush instead")]
  public Brush InValidDateBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.InValidDateBorderBrushProperty);
    set => this.SetValue(CalendarEdit.InValidDateBorderBrushProperty, (object) value);
  }

  [Obsolete("InValidDateForeGround is deprecated, use BlackoutDatesForeground instead")]
  public Brush InValidDateForeGround
  {
    get => (Brush) this.GetValue(CalendarEdit.InValidDateForeGroundProperty);
    set => this.SetValue(CalendarEdit.InValidDateForeGroundProperty, (object) value);
  }

  [Obsolete("InValidDateBackground is deprecated, use BlackoutDatesBackground instead")]
  public Brush InValidDateBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.InValidDateBackgroundProperty);
    set => this.SetValue(CalendarEdit.InValidDateBackgroundProperty, (object) value);
  }

  [Obsolete("InValidDateCrossBackground is deprecated, use BlackoutDatesCrossBrush instead")]
  public Brush InValidDateCrossBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.InValidDateCrossBackgroundProperty);
    set => this.SetValue(CalendarEdit.InValidDateCrossBackgroundProperty, (object) value);
  }

  public Brush BlackoutDatesBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.BlackoutDatesBorderBrushProperty);
    set => this.SetValue(CalendarEdit.BlackoutDatesBorderBrushProperty, (object) value);
  }

  public Brush BlackoutDatesForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.BlackoutDatesForegroundProperty);
    set => this.SetValue(CalendarEdit.BlackoutDatesForegroundProperty, (object) value);
  }

  public Brush BlackoutDatesBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.BlackoutDatesBackgroundProperty);
    set => this.SetValue(CalendarEdit.BlackoutDatesBackgroundProperty, (object) value);
  }

  public Brush BlackoutDatesCrossBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.BlackoutDatesCrossBrushProperty);
    set => this.SetValue(CalendarEdit.BlackoutDatesCrossBrushProperty, (object) value);
  }

  public Brush SelectionForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectionForegroundProperty);
    set => this.SetValue(CalendarEdit.SelectionForegroundProperty, (object) value);
  }

  public Brush WeekNumberSelectionBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberSelectionBorderBrushProperty);
    set => this.SetValue(CalendarEdit.WeekNumberSelectionBorderBrushProperty, (object) value);
  }

  public Thickness WeekNumberSelectionBorderThickness
  {
    get => (Thickness) this.GetValue(CalendarEdit.WeekNumberSelectionBorderThicknessProperty);
    set => this.SetValue(CalendarEdit.WeekNumberSelectionBorderThicknessProperty, (object) value);
  }

  public Thickness WeekNumberBorderThickness
  {
    get => (Thickness) this.GetValue(CalendarEdit.WeekNumberBorderThicknessProperty);
    set => this.SetValue(CalendarEdit.WeekNumberBorderThicknessProperty, (object) value);
  }

  public CornerRadius WeekNumberSelectionBorderCornerRadius
  {
    get => (CornerRadius) this.GetValue(CalendarEdit.WeekNumberSelectionBorderCornerRadiusProperty);
    set
    {
      this.SetValue(CalendarEdit.WeekNumberSelectionBorderCornerRadiusProperty, (object) value);
    }
  }

  public Brush WeekNumberBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberBackgroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberBackgroundProperty, (object) value);
  }

  public Brush MouseOverBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.MouseOverBorderBrushProperty);
    set => this.SetValue(CalendarEdit.MouseOverBorderBrushProperty, (object) value);
  }

  public Brush NotCurrentMonthForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.NotCurrentMonthForegroundProperty);
    set => this.SetValue(CalendarEdit.NotCurrentMonthForegroundProperty, (object) value);
  }

  public Brush MouseOverForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.MouseOverForegroundProperty);
    set => this.SetValue(CalendarEdit.MouseOverForegroundProperty, (object) value);
  }

  public Brush MouseOverBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.MouseOverBackgroundProperty);
    set => this.SetValue(CalendarEdit.MouseOverBackgroundProperty, (object) value);
  }

  public Brush SelectedDayCellBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectedDayCellBorderBrushProperty);
    set => this.SetValue(CalendarEdit.SelectedDayCellBorderBrushProperty, (object) value);
  }

  public Brush SelectedDayCellBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectedDayCellBackgroundProperty);
    set => this.SetValue(CalendarEdit.SelectedDayCellBackgroundProperty, (object) value);
  }

  public Brush SelectedDayCellHoverBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectedDayCellHoverBackgroundProperty);
    set => this.SetValue(CalendarEdit.SelectedDayCellHoverBackgroundProperty, (object) value);
  }

  public Brush TodayCellBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.TodayCellBorderBrushProperty);
    set => this.SetValue(CalendarEdit.TodayCellBorderBrushProperty, (object) value);
  }

  public Brush TodayCellBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.TodayCellBackgroundProperty);
    set => this.SetValue(CalendarEdit.TodayCellBackgroundProperty, (object) value);
  }

  public Brush TodayCellForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.TodayCellForegroundProperty);
    set => this.SetValue(CalendarEdit.TodayCellForegroundProperty, (object) value);
  }

  public Brush TodayCellSelectedBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.TodayCellSelectedBorderBrushProperty);
    set => this.SetValue(CalendarEdit.TodayCellSelectedBorderBrushProperty, (object) value);
  }

  public Brush TodayCellSelectedBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.TodayCellSelectedBackgroundProperty);
    set => this.SetValue(CalendarEdit.TodayCellSelectedBackgroundProperty, (object) value);
  }

  public Brush SelectedDayCellForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.SelectedDayCellForegroundProperty);
    set => this.SetValue(CalendarEdit.SelectedDayCellForegroundProperty, (object) value);
  }

  public Brush WeekNumberForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberForegroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberForegroundProperty, (object) value);
  }

  public Brush WeekNumberBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberBorderBrushProperty);
    set => this.SetValue(CalendarEdit.WeekNumberBorderBrushProperty, (object) value);
  }

  public Brush WeekNumberHoverBorderBrush
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberHoverBorderBrushProperty);
    set => this.SetValue(CalendarEdit.WeekNumberHoverBorderBrushProperty, (object) value);
  }

  public Brush WeekNumberSelectionBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberSelectionBackgroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberSelectionBackgroundProperty, (object) value);
  }

  public Brush WeekNumberHoverBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberHoverBackgroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberHoverBackgroundProperty, (object) value);
  }

  public Brush WeekNumberHoverForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberHoverForegroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberHoverForegroundProperty, (object) value);
  }

  public Brush WeekNumberSelectionForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.WeekNumberSelectionForegroundProperty);
    set => this.SetValue(CalendarEdit.WeekNumberSelectionForegroundProperty, (object) value);
  }

  public CornerRadius SelectionBorderCornerRadius
  {
    get => (CornerRadius) this.GetValue(CalendarEdit.SelectionBorderCornerRadiusProperty);
    set => this.SetValue(CalendarEdit.SelectionBorderCornerRadiusProperty, (object) value);
  }

  public CornerRadius WeekNumberCornerRadius
  {
    get => (CornerRadius) this.GetValue(CalendarEdit.WeekNumberCornerRadiusProperty);
    set => this.SetValue(CalendarEdit.WeekNumberCornerRadiusProperty, (object) value);
  }

  public bool AllowSelection
  {
    get => (bool) this.GetValue(CalendarEdit.AllowSelectionProperty);
    set => this.SetValue(CalendarEdit.AllowSelectionProperty, (object) value);
  }

  public bool AllowMultiplySelection
  {
    get => (bool) this.GetValue(CalendarEdit.AllowMultiplySelectionProperty);
    set => this.SetValue(CalendarEdit.AllowMultiplySelectionProperty, (object) value);
  }

  [Browsable(false)]
  public bool IsTodayButtonClicked
  {
    get => (bool) this.GetValue(CalendarEdit.IsTodayButtonClickedProperty);
    set => this.SetValue(CalendarEdit.IsTodayButtonClickedProperty, (object) value);
  }

  [Obsolete("IsDayNamesAbbreviated is deprecated, use ShowAbbreviatedDayNames instead")]
  public bool IsDayNamesAbbreviated
  {
    get => (bool) this.GetValue(CalendarEdit.IsDayNamesAbbreviatedProperty);
    set => this.SetValue(CalendarEdit.IsDayNamesAbbreviatedProperty, (object) value);
  }

  public bool ShowAbbreviatedDayNames
  {
    get => (bool) this.GetValue(CalendarEdit.ShowAbbreviatedDayNamesProperty);
    set => this.SetValue(CalendarEdit.ShowAbbreviatedDayNamesProperty, (object) value);
  }

  [Obsolete("IsMonthNameAbbreviated is deprecated, use ShowAbbreviatedMonthNames instead")]
  public bool IsMonthNameAbbreviated
  {
    get => (bool) this.GetValue(CalendarEdit.IsMonthNameAbbreviatedProperty);
    set => this.SetValue(CalendarEdit.IsMonthNameAbbreviatedProperty, (object) value);
  }

  public bool ShowAbbreviatedMonthNames
  {
    get => (bool) this.GetValue(CalendarEdit.ShowAbbreviatedMonthNamesProperty);
    set => this.SetValue(CalendarEdit.ShowAbbreviatedMonthNamesProperty, (object) value);
  }

  [Obsolete("IsShowWeekNumbers is deprecated, use ShowWeekNumbers instead")]
  public bool IsShowWeekNumbers
  {
    get => (bool) this.GetValue(CalendarEdit.IsShowWeekNumbersProperty);
    set => this.SetValue(CalendarEdit.IsShowWeekNumbersProperty, (object) value);
  }

  public bool ShowWeekNumbers
  {
    get => (bool) this.GetValue(CalendarEdit.ShowWeekNumbersProperty);
    set => this.SetValue(CalendarEdit.ShowWeekNumbersProperty, (object) value);
  }

  [Obsolete(" IsShowWeekNumbersGrid is deprecated")]
  public bool IsShowWeekNumbersGrid
  {
    get => (bool) this.GetValue(CalendarEdit.IsShowWeekNumbersGridProperty);
    set => this.SetValue(CalendarEdit.IsShowWeekNumbersGridProperty, (object) value);
  }

  [Obsolete("IsAllowYearSelection is deprecated, use IsAllowYearSelection instead")]
  public bool IsAllowYearSelection
  {
    get => (bool) this.GetValue(CalendarEdit.IsAllowYearSelectionProperty);
    set => this.SetValue(CalendarEdit.IsAllowYearSelectionProperty, (object) value);
  }

  [Obsolete("This property is deprecated, you can navigate to year view and choose required year")]
  [Browsable(false)]
  public bool AllowYearEditing
  {
    get => (bool) this.GetValue(CalendarEdit.AllowYearEditingProperty);
    set => this.SetValue(CalendarEdit.AllowYearEditingProperty, (object) value);
  }

  public bool ShowPreviousMonthDays
  {
    get => (bool) this.GetValue(CalendarEdit.ShowPreviousMonthDaysProperty);
    set => this.SetValue(CalendarEdit.ShowPreviousMonthDaysProperty, (object) value);
  }

  public bool ShowNextMonthDays
  {
    get => (bool) this.GetValue(CalendarEdit.ShowNextMonthDaysProperty);
    set => this.SetValue(CalendarEdit.ShowNextMonthDaysProperty, (object) value);
  }

  public SelectionRangeMode SelectionRangeMode
  {
    get => (SelectionRangeMode) this.GetValue(CalendarEdit.SelectionRangeModeProperty);
    set => this.SetValue(CalendarEdit.SelectionRangeModeProperty, (object) value);
  }

  public int FrameMovingTime
  {
    get => (int) this.GetValue(CalendarEdit.FrameMovingTimeProperty);
    set => this.SetValue(CalendarEdit.FrameMovingTimeProperty, (object) value);
  }

  public int ChangeModeTime
  {
    get => (int) this.GetValue(CalendarEdit.ChangeModeTimeProperty);
    set => this.SetValue(CalendarEdit.ChangeModeTimeProperty, (object) value);
  }

  public AnimationDirection MonthChangeDirection
  {
    get => (AnimationDirection) this.GetValue(CalendarEdit.MonthChangeDirectionProperty);
    set => this.SetValue(CalendarEdit.MonthChangeDirectionProperty, (object) value);
  }

  public DataTemplate DayNameCellsDataTemplate
  {
    get => (DataTemplate) this.GetValue(CalendarEdit.DayNameCellsDataTemplateProperty);
    set => this.SetValue(CalendarEdit.DayNameCellsDataTemplateProperty, (object) value);
  }

  public ControlTemplate PreviousScrollButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(CalendarEdit.PreviousScrollButtonTemplateProperty);
    set => this.SetValue(CalendarEdit.PreviousScrollButtonTemplateProperty, (object) value);
  }

  public ControlTemplate NextScrollButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(CalendarEdit.NextScrollButtonTemplateProperty);
    set => this.SetValue(CalendarEdit.NextScrollButtonTemplateProperty, (object) value);
  }

  public DataTemplate DayCellsDataTemplate
  {
    get => (DataTemplate) this.GetValue(CalendarEdit.DayCellsDataTemplateProperty);
    set => this.SetValue(CalendarEdit.DayCellsDataTemplateProperty, (object) value);
  }

  public Style DayCellsStyle
  {
    get => (Style) this.GetValue(CalendarEdit.DayCellsStyleProperty);
    set => this.SetValue(CalendarEdit.DayCellsStyleProperty, (object) value);
  }

  public Style DayNameCellsStyle
  {
    get => (Style) this.GetValue(CalendarEdit.DayNameCellsStyleProperty);
    set => this.SetValue(CalendarEdit.DayNameCellsStyleProperty, (object) value);
  }

  public DataTemplateSelector DayCellsDataTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(CalendarEdit.DayCellsDataTemplateSelectorProperty);
    set => this.SetValue(CalendarEdit.DayCellsDataTemplateSelectorProperty, (object) value);
  }

  public DataTemplateSelector DayNameCellsDataTemplateSelector
  {
    get
    {
      return (DataTemplateSelector) this.GetValue(CalendarEdit.DayNameCellsDataTemplateSelectorProperty);
    }
    set => this.SetValue(CalendarEdit.DayNameCellsDataTemplateSelectorProperty, (object) value);
  }

  public DataTemplatesDictionary DateDataTemplates
  {
    get => (DataTemplatesDictionary) this.GetValue(CalendarEdit.DateDataTemplatesProperty);
    set => this.SetValue(CalendarEdit.DateDataTemplatesProperty, (object) value);
  }

  public StylesDictionary DateStyles
  {
    get => (StylesDictionary) this.GetValue(CalendarEdit.DateStylesProperty);
    set => this.SetValue(CalendarEdit.DateStylesProperty, (object) value);
  }

  public bool DisableDateSelection
  {
    get => (bool) this.GetValue(CalendarEdit.DisableDateSelectionProperty);
    set => this.SetValue(CalendarEdit.DisableDateSelectionProperty, (object) value);
  }

  public bool ScrollToDateEnabled
  {
    get => (bool) this.GetValue(CalendarEdit.ScrollToDateEnabledProperty);
    set => this.SetValue(CalendarEdit.ScrollToDateEnabledProperty, (object) value);
  }

  protected internal DayNamesGrid DayNamesGrid
  {
    get => (DayNamesGrid) this.GetValue(CalendarEdit.DayNamesGridProperty);
    set => this.SetValue(CalendarEdit.DayNamesGridProperty, (object) value);
  }

  protected internal WeekNumbersGrid WeekNumbersGrid
  {
    get => (WeekNumbersGrid) this.GetValue(CalendarEdit.WeekNumbersGridProperty);
    set => this.SetValue(CalendarEdit.WeekNumbersGridProperty, (object) value);
  }

  protected internal DayGrid CurrentDayGrid
  {
    get => (DayGrid) this.GetValue(CalendarEdit.CurrentDayGridProperty);
    set => this.SetValue(CalendarEdit.CurrentDayGridProperty, (object) value);
  }

  protected internal DayGrid FollowingDayGrid
  {
    get => (DayGrid) this.GetValue(CalendarEdit.FollowingDayGridProperty);
    set => this.SetValue(CalendarEdit.FollowingDayGridProperty, (object) value);
  }

  protected internal MonthGrid CurrentMonthGrid
  {
    get => (MonthGrid) this.GetValue(CalendarEdit.CurrentMonthGridProperty);
    set => this.SetValue(CalendarEdit.CurrentMonthGridProperty, (object) value);
  }

  protected internal YearGrid CurrentYearGrid
  {
    get => (YearGrid) this.GetValue(CalendarEdit.CurrentYearGridProperty);
    set => this.SetValue(CalendarEdit.CurrentYearGridProperty, (object) value);
  }

  protected internal YearRangeGrid CurrentYearRangeGrid
  {
    get => (YearRangeGrid) this.GetValue(CalendarEdit.CurrentYearRangeGridProperty);
    set => this.SetValue(CalendarEdit.CurrentYearRangeGridProperty, (object) value);
  }

  protected internal WeekNumberGridPanel CurrentWeekNumbersGrid
  {
    get => (WeekNumberGridPanel) this.GetValue(CalendarEdit.CurrentWeekNumbersGridProperty);
    set => this.SetValue(CalendarEdit.CurrentWeekNumbersGridProperty, (object) value);
  }

  protected internal WeekNumberGridPanel FollowingWeekNumbersGrid
  {
    get => (WeekNumberGridPanel) this.GetValue(CalendarEdit.FollowingWeekNumbersGridProperty);
    set => this.SetValue(CalendarEdit.FollowingWeekNumbersGridProperty, (object) value);
  }

  protected internal MonthGrid FollowingMonthGrid
  {
    get => (MonthGrid) this.GetValue(CalendarEdit.FollowingMonthGridProperty);
    set => this.SetValue(CalendarEdit.FollowingMonthGridProperty, (object) value);
  }

  protected internal YearGrid FollowingYearGrid
  {
    get => (YearGrid) this.GetValue(CalendarEdit.FollowingYearGridProperty);
    set => this.SetValue(CalendarEdit.FollowingYearGridProperty, (object) value);
  }

  protected internal YearRangeGrid FollowingYearRangeGrid
  {
    get => (YearRangeGrid) this.GetValue(CalendarEdit.FollowingYearRangeGridProperty);
    set => this.SetValue(CalendarEdit.FollowingYearRangeGridProperty, (object) value);
  }

  public DateTime MinDate
  {
    get => (DateTime) this.GetValue(CalendarEdit.MinDateProperty);
    set => this.SetValue(CalendarEdit.MinDateProperty, (object) value);
  }

  public DateTime MaxDate
  {
    get => (DateTime) this.GetValue(CalendarEdit.MaxDateProperty);
    set => this.SetValue(CalendarEdit.MaxDateProperty, (object) value);
  }

  protected internal VisibleDate VisibleData
  {
    get => (VisibleDate) this.GetValue(CalendarEdit.VisibleDataProperty);
    set => this.SetValue(CalendarEdit.VisibleDataProperty, (object) value);
  }

  public Brush HeaderForeground
  {
    get => (Brush) this.GetValue(CalendarEdit.HeaderForegroundProperty);
    set => this.SetValue(CalendarEdit.HeaderForegroundProperty, (object) value);
  }

  public Brush HeaderBackground
  {
    get => (Brush) this.GetValue(CalendarEdit.HeaderBackgroundProperty);
    set => this.SetValue(CalendarEdit.HeaderBackgroundProperty, (object) value);
  }

  public string TodayDate
  {
    get => (string) this.GetValue(CalendarEdit.TodayDateProperty);
    private set => this.SetValue(CalendarEdit.TodayDatePropertyKey, (object) value);
  }

  public bool TodayRowIsVisible
  {
    get => (bool) this.GetValue(CalendarEdit.TodayRowIsVisibleProperty);
    set => this.SetValue(CalendarEdit.TodayRowIsVisibleProperty, (object) value);
  }

  public bool MinMaxHidden
  {
    get => (bool) this.GetValue(CalendarEdit.MinMaxHiddenProperty);
    set => this.SetValue(CalendarEdit.MinMaxHiddenProperty, (object) value);
  }

  private static void OnCalendarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnCalendarChanged(e);
  }

  private static void OnCalendarStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnCalendarStyleChanged(e);
  }

  private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnCultureChanged(e);
  }

  private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDateChanged(e);
  }

  private static void OnAllowSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    if (!calendarEdit.AllowSelection)
      calendarEdit.ClearSelectedCell();
    calendarEdit.OnAllowSelectionChanged(e);
  }

  private static void OnAllowMultiplySelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    if (calendarEdit.AllowSelection && !calendarEdit.AllowMultiplySelection)
      calendarEdit.ClearSelectedCell();
    calendarEdit.OnAllowMultiplySelectionChanged(e);
  }

  private static void OnShowAbbreviatedMonthNamesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnMonthNameAbbreviatedChanged(e);
  }

  private static void OnIsTodayButtonClickedChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnIsTodayButtonClickedChanged(e);
  }

  private static void OnSelectionRangeModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectionRangeModeChanged(e);
  }

  private static void OnSelectionBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectionBorderBrushChanged(e);
  }

  private static void OnBlackoutDatesBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnBlackoutDatesBorderBrushChanged(e);
  }

  private void OnBlackoutDatesBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnBlackoutDatesForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnBlackoutDatesForegroundChanged(e);
  }

  private void OnBlackoutDatesForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnBlackoutDatesBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnBlackoutDatesBackgroundChanged(e);
  }

  private void OnBlackoutDatesBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnBlackoutDatesCrossBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).BlackoutDatesCrossBrushChanged(e);
  }

  private void BlackoutDatesCrossBrushChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnMouseOverBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnMouseOverBorderBrushChanged(e);
  }

  private static void OnMouseOverForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnMouseOverForegroundChanged(e);
  }

  private static void OnMouseOverBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnMouseOverBackgroundChanged(e);
  }

  private static void OnSelectedDayCellBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectedDayCellBorderBrushChanged(e);
  }

  private static void OnSelectedDayCellHoverBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectedDayCellHoverBackgroundChanged(e);
  }

  private static void OnTodayCellBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayCellBorderBrushChanged(e);
  }

  private static void OnTodayCellForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayCellForegroundChanged(e);
  }

  private static void OnTodayCellBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayCellBackgroundChanged(e);
  }

  private static void OnTodayCellSelectedBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayCellSelectedBorderBrushChanged(e);
  }

  private static void OnTodayCellSelectedBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayCellSelectedBackgroundChanged(e);
  }

  private static void OnNotCurrentMonthForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnNotCurrentMonthForegroundChanged(e);
  }

  private static void OnSelectedDayCellBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectedDayCellBackgroundChanged(e);
  }

  private static void OnSelectedDayCellForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectedDayCellForegroundChanged(e);
  }

  private static void OnSelectionForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectionForegroundChanged(e);
  }

  private static void OnWeekNumberSelectionBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberSelectionBorderBrushChanged(e);
  }

  private static void OnWeekNumberSelectionBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberSelectionBorderThicknessChanged(e);
  }

  private static void OnWeekNumberSelectionBorderCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberSelectionBorderCornerRadiusChanged(e);
  }

  private static void OnWeekNumberBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberBackgroundChanged(e);
  }

  private static void OnWeekNumberHoverBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberHoverBackgroundChanged(e);
  }

  private static void OnWeekNumberHoverBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberHoverBorderBrushChanged(e);
  }

  private static void OnWeekNumberSelectionBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberSelectionBackgroundChanged(e);
  }

  private static void OnWeekNumberBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberBorderBrushChanged(e);
  }

  private static void OnWeekNumberBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberBorderThicknessChanged(e);
  }

  private static void OnWeekNumberCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberCornerRadiusChanged(e);
  }

  private static void OnWeekNumberForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberForegroundChanged(e);
  }

  private static void OnWeekNumberSelectionForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberSelectionForegroundChanged(e);
  }

  private static void OnWeekNumberHoverForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnWeekNumberHoverForegroundChanged(e);
  }

  private static void OnSelectionBorderCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnSelectionBorderCornerRadiusChanged(e);
  }

  private static void OnFrameMovingTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnFrameMovingTimeChanged(e);
  }

  private static void OnChangeModeTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnChangeModeTimeChanged(e);
  }

  private static void OnMonthChangeDirectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnMonthChangeDirectionChanged(e);
  }

  private static void OnShowAbbreviatedDayNamesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayNamesAbbreviatedChanged(e);
  }

  private static void OnDayNameCellsDataTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayNameCellsDataTemplateChanged(e);
  }

  private static void OnPreviousScrollButtonTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnPreviousScrollButtonTemplateChanged(e);
  }

  private static void OnNextScrollButtonTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnNextScrollButtonTemplateChanged(e);
  }

  private static void OnDayCellsDataTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayCellsDataTemplateChanged(e);
  }

  private static void OnDayCellsStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayCellsStyleChanged(e);
  }

  private static void OnDayNameCellsStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayNameCellsStyleChanged(e);
  }

  private static void OnDayCellsDataTemplateSelectorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayCellsDataTemplateSelectorChanged(e);
  }

  private static void OnDayNameCellsDataTemplateSelectorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDayNameCellsDataTemplateSelectorChanged(e);
  }

  private static void OnDateDataTemplatesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDateDataTemplatesChanged(e);
  }

  private static void OnShowWeekNumbersChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnShowWeekNumbersChanged(e);
  }

  private static void OnShowWeekNumbersGridChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnShowWeekNumbersGridChanged(e);
  }

  private static void OnAllowYearEditingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnAllowYearSelectionChanged(e);
  }

  private static void OnShowPreviousMonthDaysChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnShowPreviousMonthDaysChanged(e);
  }

  private static void OnShowNextMonthDaysChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnShowNextMonthDaysChanged(e);
  }

  private static void OnTodayRowIsVisibleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnTodayRowIsVisibleChanged(e);
  }

  private static void OnMinMaxHiddenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.OnMinMaxHiddenChanged(e);
    calendarEdit.NavigateButtonVerify();
  }

  private static void OnMinDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.MinDate = (DateTime) e.NewValue;
    calendarEdit.CoerceValue(CalendarEdit.VisibleDataProperty);
    if (calendarEdit.VisualMode == CalendarVisualMode.Months)
    {
      calendarEdit.CurrentMonthGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingMonthGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
    else if (calendarEdit.VisualMode == CalendarVisualMode.Years)
    {
      calendarEdit.CurrentYearGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingYearGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
    else
    {
      if (calendarEdit.VisualMode != CalendarVisualMode.YearsRange)
        return;
      calendarEdit.CurrentYearRangeGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingYearRangeGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
  }

  private static object OnCoerceMinDateProperty(DependencyObject sender, object data) => data;

  private static void OnMaxDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.MaxDate = (DateTime) e.NewValue;
    calendarEdit.CoerceValue(CalendarEdit.VisibleDataProperty);
    if (calendarEdit.VisualMode == CalendarVisualMode.Months)
    {
      calendarEdit.CurrentMonthGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingMonthGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
    else if (calendarEdit.VisualMode == CalendarVisualMode.Years)
    {
      calendarEdit.CurrentYearGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingYearGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
    else
    {
      if (calendarEdit.VisualMode != CalendarVisualMode.YearsRange)
        return;
      calendarEdit.CurrentYearRangeGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
      calendarEdit.FollowingYearRangeGrid.Initialize(calendarEdit.VisibleData, calendarEdit.Culture, calendarEdit.Calendar);
    }
  }

  private static object OnCoerceMaxDateProperty(DependencyObject sender, object data) => data;

  private static object OnCoerceVisibleData(DependencyObject d, object value)
  {
    return (object) (d as CalendarEdit).OnCoerceVisibleData(value);
  }

  private static void OnVisibleDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnVisibleDataChanged(e);
  }

  private static void OnVisualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnVisualModeChanged(e);
  }

  private static void OnScrollToDateEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnScrollToDateEnabledChanged(e);
  }

  private static void OnDateStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDateStylesChanged(e);
  }

  private static void OnDisableDateSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDisableDateSelectionChanged(e);
  }

  private static void OnDatesCollectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnDatesCollectionChanged(e);
  }

  private static void OnBlackDatesCollectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((CalendarEdit) d).OnBlackoutDatesCollectionChanged(e);
  }

  private static object OnCoerceDate(DependencyObject d, object value)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    DateTime date = (DateTime) value;
    DateTime dateTime = calendarEdit.OnCoerceDate(date);
    return date != dateTime ? (object) dateTime : value;
  }

  protected virtual void OnShowWeekNumbersChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsShowWeekNumbersChanged != null)
      this.IsShowWeekNumbersChanged((DependencyObject) this, e);
    if (this.m_mainGrid == null && this.m_weekNumbersContainer == null)
    {
      this.m_mainGrid = this.GetMainGrid();
      this.m_weekNumbersContainer = this.GetWeekNumbersContainer();
    }
    if (this.m_mainGrid == null || this.m_weekNumbersContainer == null)
      return;
    if ((bool) e.NewValue && this.VisualMode == CalendarVisualMode.Days)
      this.ShowWeekNumbersContainer();
    else
      this.HideWeekNumbersContainer();
  }

  protected virtual void OnShowWeekNumbersGridChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsShowWeekNumbersGridChanged != null)
      this.IsShowWeekNumbersGridChanged((DependencyObject) this, e);
    if (this.wcurrentweekNumbersContainer == null || this.wfollowweekNumbersContainer == null)
      return;
    if (this.IsShowWeekNumbersGrid)
    {
      if ((bool) e.NewValue && this.VisualMode == CalendarVisualMode.WeekNumbers)
        this.ShowWeekNumbersForYearContainer();
      if ((bool) e.NewValue && this.VisualMode == CalendarVisualMode.Days)
        this.HideWeekNumbersForYearContainer();
    }
    if ((bool) e.NewValue || this.VisualMode != CalendarVisualMode.WeekNumbers)
      return;
    this.ChangeVisualMode(CalendarEdit.ChangeVisualModeDirection.Up);
  }

  protected virtual void OnAllowYearSelectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsAllowYearSelectionChanged != null)
      this.IsAllowYearSelectionChanged((DependencyObject) this, e);
    if (this.m_yearUpDownPanel == null)
    {
      this.ApplyYearEditingTemplate();
      this.ApplyEditMonthTemplate();
    }
    else if ((bool) e.NewValue)
      this.AddMonthButtonsEvents();
    else
      this.DeleteMonthButtonsEvents();
  }

  protected virtual void OnShowPreviousMonthDaysChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ShowPreviousMonthDaysChanged != null)
      this.ShowPreviousMonthDaysChanged((DependencyObject) this, e);
    this.InitVisibleDayGrid();
  }

  protected virtual void OnShowNextMonthDaysChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ShowNextMonthDaysChanged != null)
      this.ShowNextMonthDaysChanged((DependencyObject) this, e);
    this.InitVisibleDayGrid();
  }

  protected virtual void OnTodayRowIsVisibleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.TodayRowIsVisibleChanged == null)
      return;
    this.TodayRowIsVisibleChanged((DependencyObject) this, e);
  }

  protected virtual void OnMinMaxHiddenChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CurrentDayGrid == null || this.FollowingDayGrid == null)
      return;
    this.CurrentDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    this.FollowingDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    if (this.MinMaxHiddenChanged == null)
      return;
    this.MinMaxHiddenChanged((DependencyObject) this, e);
  }

  protected virtual VisibleDate OnCoerceVisibleData(object obj)
  {
    VisibleDate visibleDate = (VisibleDate) obj;
    if (this.MinMaxHidden)
    {
      int year = this.Calendar.GetYear(this.MinDate);
      int month = this.Calendar.GetMonth(this.MinDate);
      this.Calendar.GetDayOfMonth(this.MinDate);
      if (new Syncfusion.Windows.Shared.Date(visibleDate.VisibleYear, visibleDate.VisibleMonth, 31 /*0x1F*/) < new Syncfusion.Windows.Shared.Date(year, month, this.MinDate.Day))
      {
        DateTime time = new DateTime(this.MinDate.Year, this.MinDate.Month, this.MinDate.Day);
        return new VisibleDate(this.Calendar.GetYear(time), this.Calendar.GetMonth(time), this.Calendar.GetDayOfMonth(time));
      }
      if (new Syncfusion.Windows.Shared.Date(visibleDate.VisibleYear, visibleDate.VisibleMonth, 1) > new Syncfusion.Windows.Shared.Date(this.MaxDate.Year, this.MaxDate.Month, this.MaxDate.Day))
      {
        DateTime time = new DateTime(this.MaxDate.Year, this.MaxDate.Month, this.MaxDate.Day);
        return new VisibleDate(this.Calendar.GetYear(time), this.Calendar.GetMonth(time), this.Calendar.GetDayOfMonth(time));
      }
    }
    else
    {
      int year = this.Calendar.GetYear(this.miDate);
      int month = this.Calendar.GetMonth(this.miDate);
      this.Calendar.GetDayOfMonth(this.miDate);
      if (new Syncfusion.Windows.Shared.Date(visibleDate.VisibleYear, visibleDate.VisibleMonth, 31 /*0x1F*/) < new Syncfusion.Windows.Shared.Date(year, month, this.miDate.Day))
      {
        DateTime time = new DateTime(this.miDate.Year, this.miDate.Month, this.miDate.Day);
        return new VisibleDate(this.Calendar.GetYear(time), this.Calendar.GetMonth(time), this.Calendar.GetDayOfMonth(time));
      }
      if (new Syncfusion.Windows.Shared.Date(visibleDate.VisibleYear, visibleDate.VisibleMonth, 1) > new Syncfusion.Windows.Shared.Date(this.mxDate.Year, this.mxDate.Month, this.mxDate.Day))
      {
        DateTime time = new DateTime(this.mxDate.Year, this.mxDate.Month, this.mxDate.Day);
        return new VisibleDate(this.Calendar.GetYear(time), this.Calendar.GetMonth(time), this.Calendar.GetDayOfMonth(time));
      }
    }
    return visibleDate;
  }

  protected virtual void OnVisibleDataChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.VisibleDataChanged != null)
      this.VisibleDataChanged((DependencyObject) this, e);
    CalendarVisualMode mode = this.VisualMode;
    if (this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      mode = this.ViewMode;
    DateTimeFormatInfo dateTimeFormat = this.Culture.DateTimeFormat;
    if (this.DayNamesGrid != null && mode == CalendarVisualMode.Days)
      this.DayNamesGrid.SetDayNames(dateTimeFormat);
    if (this.Calendar != null)
    {
      if (this.VisualModeInfo.NewMode != this.VisualModeInfo.OldMode)
      {
        if (this.FindCurrentGrid(this.VisualModeInfo.NewMode) != null)
          this.FindCurrentGrid(this.VisualModeInfo.NewMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
        if (this.FindCurrentGrid(this.VisualModeInfo.OldMode) != null)
          this.FindCurrentGrid(this.VisualModeInfo.OldMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
      }
      else
      {
        this.VisibleDataToMinSupportedDate(dateTimeFormat);
        if (mode == CalendarVisualMode.All)
        {
          if (this.FindCurrentGrid(CalendarVisualMode.Days) != null)
            this.FindCurrentGrid(CalendarVisualMode.Days).Initialize(this.VisibleData, this.Culture, this.Calendar);
        }
        else if (this.FindCurrentGrid(mode) != null)
          this.FindCurrentGrid(mode).Initialize(this.VisibleData, this.Culture, this.Calendar);
      }
      if (this.m_monthButton1 != null)
      {
        if (mode == CalendarVisualMode.All)
          this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, CalendarVisualMode.Days);
        else
          this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, mode);
      }
    }
    if (this.VisualMode == CalendarVisualMode.Days && this.WeekNumbersGrid != null && this.FollowingDayGrid != null && this.FollowingDayGrid.WeekNumbers != null)
    {
      this.WeekNumbersGrid.SetWeekNumbers(this.FollowingDayGrid.WeekNumbers);
      this.UpdateWeekNumbersContainer();
    }
    this.InitializePopup();
    this.NavigateButtonVerify();
  }

  protected virtual void OnScrollToDateEnabledChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ScrollToDateEnabledChanged == null)
      return;
    this.ScrollToDateEnabledChanged((DependencyObject) this, e);
  }

  protected virtual void OnVisualModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.VisualModeChanged != null)
      this.VisualModeChanged((DependencyObject) this, e);
    if (this.GetTemplateChild("rect") is Rectangle templateChild)
    {
      if (this.VisualMode != CalendarVisualMode.Days)
        templateChild.Visibility = Visibility.Collapsed;
      if (this.VisualMode == CalendarVisualMode.Days)
        templateChild.Visibility = Visibility.Visible;
    }
    if (this.VisualMode == CalendarVisualMode.Days && (this.ViewMode == CalendarVisualMode.Days || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)) || this.VisualMode == CalendarVisualMode.All && (this.ViewMode == CalendarVisualMode.All || this.ViewMode == CalendarVisualMode.Days))
    {
      if (!this.monthpressed || this.ViewMode == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode) || this.VisualMode == CalendarVisualMode.All)
      {
        this.DayNamesGrid.Visibility = Visibility.Visible;
        this.CurrentDayGrid.Visibility = Visibility.Visible;
        this.FollowingDayGrid.Visibility = Visibility.Hidden;
        if (this.mouseDayscroll)
        {
          this.FollowingDayGrid.Visibility = Visibility.Visible;
          this.CurrentDayGrid.Visibility = Visibility.Hidden;
        }
        this.CurrentMonthGrid.Visibility = Visibility.Hidden;
        this.FollowingMonthGrid.Visibility = Visibility.Hidden;
        this.CurrentYearGrid.Visibility = Visibility.Hidden;
        this.FollowingYearGrid.Visibility = Visibility.Hidden;
        this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
        this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
        this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
        this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
        if (this.FollowingDayGrid.RenderTransform is ScaleTransform)
          this.FollowingDayGrid.RenderTransform = (Transform) new ScaleTransform(1.0, 1.0);
        if ((this.ViewMode == CalendarVisualMode.Days || this.IsFlagValueSet(this.ViewMode)) && this.VisualMode == CalendarVisualMode.Days && this.VisualModeInfo.OldMode != this.VisualModeInfo.NewMode)
        {
          this.DayNamesGrid.Visibility = Visibility.Visible;
          this.FollowingMonthGrid.Visibility = Visibility.Hidden;
          this.CurrentMonthGrid.Visibility = Visibility.Hidden;
          if (this.FindCurrentGrid(this.VisualMode) != null)
          {
            this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
            this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
          }
          this.NextMode = this.GetNextValue(this.VisualMode);
          this.ChangeVisualModeIndeed();
        }
        else
          this.CurrentDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      }
      this.monthpressed = false;
    }
    else if (this.VisualMode == CalendarVisualMode.Months && (this.ViewMode == CalendarVisualMode.All || this.ViewMode == CalendarVisualMode.Months || this.IsFlagValueSet(this.ViewMode)))
    {
      this.CurrentMonthGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.FollowingMonthGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      if (!this.yearpressed || this.ViewMode == CalendarVisualMode.Months || this.IsFlagValueSet(this.ViewMode))
      {
        this.CurrentMonthGrid.Visibility = Visibility.Visible;
        this.FollowingMonthGrid.Visibility = Visibility.Hidden;
        if (this.mousemonthscroll)
        {
          this.FollowingMonthGrid.Visibility = Visibility.Visible;
          this.CurrentMonthGrid.Visibility = Visibility.Hidden;
        }
        this.DayNamesGrid.Visibility = Visibility.Hidden;
        this.CurrentDayGrid.Visibility = Visibility.Hidden;
        this.FollowingDayGrid.Visibility = Visibility.Hidden;
        this.CurrentYearGrid.Visibility = Visibility.Hidden;
        this.FollowingYearGrid.Visibility = Visibility.Hidden;
        this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
        this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
        this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
        this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
        if ((this.ViewMode == CalendarVisualMode.Months || this.IsFlagValueSet(this.ViewMode)) && this.VisualMode == CalendarVisualMode.Months && this.VisualModeInfo.OldMode != this.VisualModeInfo.NewMode)
        {
          this.FollowingMonthGrid.Visibility = Visibility.Hidden;
          this.CurrentMonthGrid.Visibility = Visibility.Hidden;
          if (this.FindCurrentGrid(this.VisualMode) != null)
          {
            this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
            this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
          }
          this.NextMode = this.GetNextValue(this.VisualMode);
          this.ChangeVisualModeIndeed();
        }
        else
          this.CurrentMonthGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      }
      this.yearpressed = false;
    }
    else if (this.VisualMode == CalendarVisualMode.Years && (this.ViewMode == CalendarVisualMode.Years || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)))
    {
      this.CurrentYearGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.FollowingYearGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.FollowingDayGrid.Visibility = Visibility.Hidden;
      this.CurrentYearGrid.Visibility = Visibility.Visible;
      this.FollowingYearGrid.Visibility = Visibility.Hidden;
      if (this.mouseyearscroll)
      {
        this.FollowingYearGrid.Visibility = Visibility.Visible;
        this.CurrentYearGrid.Visibility = Visibility.Hidden;
      }
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
      this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
      this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
      this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
      if ((this.ViewMode == CalendarVisualMode.Years || this.IsFlagValueSet(this.ViewMode)) && this.VisualMode == CalendarVisualMode.Years && this.VisualModeInfo.OldMode != this.VisualModeInfo.NewMode)
      {
        this.FollowingYearGrid.Visibility = Visibility.Hidden;
        this.CurrentYearGrid.Visibility = Visibility.Hidden;
        if (this.FindCurrentGrid(this.VisualMode) != null)
        {
          this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
          this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
        }
        this.NextMode = this.GetNextValue(this.VisualMode);
        this.ChangeVisualModeIndeed();
      }
      else
        this.CurrentYearGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    }
    else if (this.VisualMode == CalendarVisualMode.YearsRange && (this.ViewMode == CalendarVisualMode.YearsRange || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)))
    {
      this.CurrentYearRangeGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.FollowingYearRangeGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.FollowingDayGrid.Visibility = Visibility.Hidden;
      this.CurrentYearGrid.Visibility = Visibility.Hidden;
      this.FollowingYearGrid.Visibility = Visibility.Hidden;
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.CurrentWeekNumbersGrid.Visibility = Visibility.Hidden;
      this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
      this.CurrentYearRangeGrid.Visibility = Visibility.Visible;
      this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
      if (this.mouseyearrangescroll)
      {
        this.FollowingYearRangeGrid.Visibility = Visibility.Visible;
        this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
      }
      if (this.ViewMode == CalendarVisualMode.YearsRange && this.VisualModeInfo.OldMode != this.VisualModeInfo.NewMode)
      {
        this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
        this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
        if (this.FindCurrentGrid(this.VisualMode) != null)
        {
          this.FindCurrentGrid(this.VisualMode).Visibility = Visibility.Visible;
          this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
        }
        this.NextMode = this.GetNextValue(this.VisualMode);
        this.ChangeVisualModeIndeed();
      }
      else
        this.CurrentYearRangeGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    }
    else if (this.VisualMode == CalendarVisualMode.WeekNumbers && (this.ViewMode == CalendarVisualMode.WeekNumbers || this.ViewMode == CalendarVisualMode.All || this.IsFlagValueSet(this.ViewMode)))
    {
      this.DayNamesGrid.Visibility = Visibility.Hidden;
      this.CurrentDayGrid.Visibility = Visibility.Hidden;
      this.FollowingDayGrid.Visibility = Visibility.Hidden;
      this.CurrentYearGrid.Visibility = Visibility.Hidden;
      this.FollowingYearGrid.Visibility = Visibility.Hidden;
      this.CurrentMonthGrid.Visibility = Visibility.Hidden;
      this.FollowingMonthGrid.Visibility = Visibility.Hidden;
      this.CurrentYearRangeGrid.Visibility = Visibility.Hidden;
      this.FollowingYearRangeGrid.Visibility = Visibility.Hidden;
      this.CurrentWeekNumbersGrid.Visibility = Visibility.Visible;
      this.FollowingWeekNumbersGrid.Visibility = Visibility.Hidden;
      if (this.wcurrentweekNumbersContainer != null && this.wfollowweekNumbersContainer != null)
      {
        this.wcurrentweekNumbersContainer.Visibility = Visibility.Visible;
        this.wfollowweekNumbersContainer.Visibility = Visibility.Hidden;
      }
      if (this.mouseweeknumberscroll)
      {
        this.FollowingWeekNumbersGrid.Visibility = Visibility.Visible;
        this.wfollowweekNumbersContainer.Visibility = Visibility.Visible;
      }
      if (this.ViewMode == CalendarVisualMode.WeekNumbers)
        this.NextMode = this.GetNextValue(this.VisualMode);
      this.CurrentWeekNumbersGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    }
    if (this.m_monthButton1 == null || this.m_monthButton2 == null || this.ViewMode != this.VisualMode && this.ViewMode != CalendarVisualMode.All && !this.IsFlagValueSet(this.ViewMode))
      return;
    this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
    this.m_monthButton2.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
  }

  protected virtual void OnDateStylesChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DateStylesChanged != null)
      this.DateStylesChanged((DependencyObject) this, e);
    if (e.OldValue != null)
      ((StylesDictionary) e.OldValue).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.DateStyles_OnPropertyChanged);
    if ((StylesDictionary) e.NewValue != null)
      ((StylesDictionary) e.NewValue).CollectionChanged += new NotifyCollectionChangedEventHandler(this.DateStyles_OnPropertyChanged);
    if (!this.IsInitializeComplete)
      return;
    this.InitilizeDayCellStyles(this.CurrentDayGrid);
    this.InitilizeDayCellStyles(this.FollowingDayGrid);
  }

  protected virtual void OnDisableDateSelectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DisableDateSelectionChanged != null)
      this.DisableDateSelectionChanged((DependencyObject) this, e);
    if (!this.DisableDateSelection)
      return;
    DateTimeFormatInfo dateTimeFormat = this.Culture.DateTimeFormat;
    this.DayNamesGrid.Visibility = Visibility.Hidden;
    this.CurrentDayGrid.Visibility = Visibility.Hidden;
    this.CurrentMonthGrid.Visibility = Visibility.Visible;
    this.FollowingMonthGrid.Visibility = Visibility.Hidden;
    this.WeekNumbersGrid.Visibility = Visibility.Hidden;
    this.VisualMode = CalendarVisualMode.Months;
    this.VisualModeInfo = new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Months);
    this.TodayRowIsVisible = false;
    this.IsWeekCellClicked = false;
    this.VisibleDataToMinSupportedDate(dateTimeFormat);
    if (this.FindCurrentGrid(this.VisualMode) == null)
      return;
    this.FindCurrentGrid(this.VisualMode).Initialize(this.VisibleData, this.Culture, this.Calendar);
  }

  protected virtual void OnCalendarChanged(DependencyPropertyChangedEventArgs e)
  {
    DateTime dateTime = new DateTime();
    if (this.CalendarChanged != null)
      this.CalendarChanged((DependencyObject) this, e);
    if (e.NewValue == null)
      this.ClearValue(CalendarEdit.CalendarProperty);
    if (this.Date == this.MinDate)
    {
      this.MinDate = this.Calendar.MinSupportedDateTime;
      this.Date = this.Calendar.MinSupportedDateTime;
    }
    if (this.Date == this.miDate)
    {
      this.miDate = this.Calendar.MinSupportedDateTime;
      this.Date = this.Calendar.MinSupportedDateTime;
    }
    if (this.MinDate < this.Calendar.MinSupportedDateTime || this.MinDate == dateTime)
      this.MinDate = this.Calendar.MinSupportedDateTime;
    if (this.MaxDate > this.Calendar.MaxSupportedDateTime || this.MaxDate == dateTime)
      this.MaxDate = this.Calendar.MaxSupportedDateTime;
    if (this.miDate < this.Calendar.MinSupportedDateTime)
      this.miDate = this.Calendar.MinSupportedDateTime;
    if (this.mxDate > this.Calendar.MaxSupportedDateTime)
      this.mxDate = this.Calendar.MaxSupportedDateTime;
    if (!this.IsInitializeComplete)
      return;
    this.CoerceVisibleData(this.Calendar);
  }

  protected virtual void OnCalendarStyleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CalendarStyleChanged != null)
      this.CalendarStyleChanged((DependencyObject) this, e);
    if ((CalendarStyle) e.NewValue == CalendarStyle.Standard)
    {
      if (this.VisualMode != CalendarVisualMode.Days)
      {
        this.m_bsuspendEventFire = true;
        this.OnDateChanged(new DependencyPropertyChangedEventArgs());
        this.m_bsuspendEventFire = false;
      }
      if (!this.AllowYearEditing)
        return;
      this.AddMonthButtonsEvents();
    }
    else
      this.DeleteMonthButtonsEvents();
  }

  protected virtual void OnCultureChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CultureChanged != null)
      this.CultureChanged((DependencyObject) this, e);
    CultureInfo newValue = (CultureInfo) e.NewValue;
    if (newValue != null && newValue.IsNeutralCulture)
      this.Culture = new CultureInfo(newValue.LCID + 1024 /*0x0400*/);
    this.UpdateMinDate(e);
    this.CoerceVisibleData(this.Calendar);
    this.TodayDate = DateTime.Now.ToString("D", (IFormatProvider) this.Culture.DateTimeFormat);
  }

  protected virtual void OnDateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DateChanged != null && !this.m_bsuspendEventFire && this.Invalidateflag)
      this.DateChanged((DependencyObject) this, e);
    if (!this.IsInitializeComplete || !this.CanScrollToView())
      return;
    if (this.VisualMode == CalendarVisualMode.Days)
    {
      this.CurrentDayGrid.SetIsDate(this.Calendar);
      this.FollowingDayGrid.SetIsDate(this.Calendar);
      if (Keyboard.Modifiers != ModifierKeys.Shift && this.m_shiftDateChangeEnabled)
        this.m_shiftDate = this.Date;
      if (!this.m_dateSetManual && this.ScrollToDateEnabled)
        this.ScrollToDate();
      this.m_dateSetManual = false;
    }
    else
    {
      if (this.VisualMode == CalendarVisualMode.Months)
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days));
      if (this.VisualMode == CalendarVisualMode.Years)
      {
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, CalendarVisualMode.Months));
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days));
      }
      if (this.VisualMode == CalendarVisualMode.YearsRange)
      {
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.YearsRange, CalendarVisualMode.Years));
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.Years, CalendarVisualMode.Months));
        this.m_visualModeQueue.Enqueue(new CalendarEdit.VisualModeHistory(CalendarVisualMode.Months, CalendarVisualMode.Days));
      }
      this.ScrollToDate();
    }
  }

  protected virtual void OnAllowSelectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AllowSelectionChanged != null)
      this.AllowSelectionChanged((DependencyObject) this, e);
    this.SelectedDates.AllowInsert = (bool) e.NewValue;
  }

  protected virtual void OnSelectionRangeModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SelectionRangeModeChanged == null)
      return;
    this.SelectionRangeModeChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectionBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectionBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectionBorderBrushChanged == null)
      return;
    this.SelectionBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectionForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectionForeground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectionForegroundChanged == null)
      return;
    this.SelectionForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnMouseOverBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("MouseOverBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.MouseOverBorderBrushChanged == null)
      return;
    this.MouseOverBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnMouseOverBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("MouseOverBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.MouseOverBackgroundChanged == null)
      return;
    this.MouseOverBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnMouseOverForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("MouseOverForeground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.MouseOverForegroundChanged == null)
      return;
    this.MouseOverForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectedDayCellBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectedDayCellBorderBrushChanged == null)
      return;
    this.SelectedDayCellBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectedDayCellHoverBackgroundChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellHoverBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectedDayCellHoverBackgroundChanged == null)
      return;
    this.SelectedDayCellHoverBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnTodayCellBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellHoverBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.TodayCellBorderBrushChanged == null)
      return;
    this.TodayCellBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnTodayCellForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("TodayCellForeground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.TodayCellForegroundChanged == null)
      return;
    this.TodayCellForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnTodayCellBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("TodayCellBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.TodayCellBackgroundChanged == null)
      return;
    this.TodayCellBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnTodayCellSelectedBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("TodayCellSelectedBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.TodayCellSelectedBorderBrushChanged == null)
      return;
    this.TodayCellSelectedBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnTodayCellSelectedBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("TodayCellSelectedBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.TodayCellSelectedBackgroundChanged == null)
      return;
    this.TodayCellSelectedBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnNotCurrentMonthForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.NotCurrentMonthForegroundChanged == null)
      return;
    this.NotCurrentMonthForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectedDayCellBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellBackground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectedDayCellBackgroundChanged == null)
      return;
    this.SelectedDayCellBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectedDayCellForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("SelectedDayCellForeground property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.SelectedDayCellForegroundChanged == null)
      return;
    this.SelectedDayCellForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberSelectionBorderBrushChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberSelectionBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.WeekNumberSelectionBorderBrushChanged == null)
      return;
    this.WeekNumberSelectionBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberBorderBrush property can be assigned only by a Brush or a Brush inherited type value.");
    if (this.WeekNumberBorderBrushChanged == null)
      return;
    this.WeekNumberBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberSelectionBorderThicknessChanged(
    DependencyPropertyChangedEventArgs e)
  {
    Thickness newValue = (Thickness) e.NewValue;
    if (this.WeekNumberSelectionBorderThicknessChanged == null)
      return;
    this.WeekNumberSelectionBorderThicknessChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    Thickness newValue = (Thickness) e.NewValue;
    if (this.WeekNumberBorderThicknessChanged == null)
      return;
    this.WeekNumberBorderThicknessChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberSelectionBorderCornerRadiusChanged(
    DependencyPropertyChangedEventArgs e)
  {
    CornerRadius newValue = (CornerRadius) e.NewValue;
    if (this.WeekNumberSelectionBorderCornerRadiusChanged == null)
      return;
    this.WeekNumberSelectionBorderCornerRadiusChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    CornerRadius newValue = (CornerRadius) e.NewValue;
    if (this.WeekNumberCornerRadiusChanged == null)
      return;
    this.WeekNumberCornerRadiusChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberBackground property can be assigned only by a Brush type value.");
    if (this.WeekNumberBackgroundChanged == null)
      return;
    this.WeekNumberBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberSelectionBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberBackground property can be assigned only by a Brush type value.");
    if (this.WeekNumberSelectionBackgroundChanged == null)
      return;
    this.WeekNumberSelectionBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberHoverBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberBackground property can be assigned only by a Brush type value.");
    if (this.WeekNumberHoverBackgroundChanged == null)
      return;
    this.WeekNumberHoverBackgroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberHoverBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberBackground property can be assigned only by a Brush type value.");
    if (this.WeekNumberHoverBorderBrushChanged == null)
      return;
    this.WeekNumberHoverBorderBrushChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberForeground property can be assigned only by a Brush type value.");
    if (this.WeekNumberForegroundChanged == null)
      return;
    this.WeekNumberForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberSelectionForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberForeground property can be assigned only by a Brush type value.");
    if (this.WeekNumberSelectionForegroundChanged == null)
      return;
    this.WeekNumberSelectionForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnWeekNumberHoverForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!(e.NewValue is Brush))
      throw new ArgumentException("WeekNumberForeground property can be assigned only by a Brush type value.");
    if (this.WeekNumberHoverForegroundChanged == null)
      return;
    this.WeekNumberHoverForegroundChanged((DependencyObject) this, e);
  }

  protected virtual void OnSelectionBorderCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SelectionBorderCornerRadiusChanged != null)
      this.SelectionBorderCornerRadiusChanged((DependencyObject) this, e);
    CornerRadius newValue = (CornerRadius) e.NewValue;
    this.CurrentDayGrid.SelectionBorder.CornerRadius = newValue;
    this.FollowingDayGrid.SelectionBorder.CornerRadius = newValue;
  }

  protected virtual void OnFrameMovingTimeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameMovingTimeChanged == null)
      return;
    this.FrameMovingTimeChanged((DependencyObject) this, e);
  }

  protected virtual void OnChangeModeTimeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ChangeModeTimeChanged == null)
      return;
    this.ChangeModeTimeChanged((DependencyObject) this, e);
  }

  protected virtual void OnMonthChangeDirectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MonthChangeDirectionChanged == null)
      return;
    this.MonthChangeDirectionChanged((DependencyObject) this, e);
  }

  protected virtual void OnMonthNameAbbreviatedChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsMonthNameAbbreviatedChanged != null)
      this.IsMonthNameAbbreviatedChanged((DependencyObject) this, e);
    if (this.m_monthButton1 == null)
      return;
    this.m_monthButton1.Initialize(this.VisibleData, this.Calendar, this.Culture, this.ShowAbbreviatedMonthNames, this.VisualMode);
  }

  protected virtual void OnIsTodayButtonClickedChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsTodayButtonClickedChanged == null)
      return;
    this.IsTodayButtonClickedChanged((DependencyObject) this, e);
  }

  protected virtual void OnAllowMultiplySelectionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AllowMultiplySelectionChanged != null)
      this.AllowMultiplySelectionChanged((DependencyObject) this, e);
    if (this.SelectedDates == null)
      return;
    this.SelectedDates.Clear();
  }

  protected virtual void OnDayNamesAbbreviatedChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsDayNamesAbbreviatedChanged != null)
      this.IsDayNamesAbbreviatedChanged((DependencyObject) this, e);
    this.DayNamesGrid.SetDayNames(this.Culture.DateTimeFormat);
  }

  protected virtual void OnDayNameCellsDataTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DayNameCellsDataTemplateChanged != null)
      this.DayNameCellsDataTemplateChanged((DependencyObject) this, e);
    this.DayNamesGrid.UpdateTemplateAndSelector((DataTemplate) e.NewValue, this.DayNameCellsDataTemplateSelector);
  }

  protected virtual void OnNextScrollButtonTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.NextScrollButtonTemplateChanged != null)
      this.NextScrollButtonTemplateChanged((DependencyObject) this, e);
    if (this.m_nextButton == null)
      return;
    this.m_nextButton.UpdateCellTemplate((ControlTemplate) e.NewValue);
  }

  protected virtual void OnPreviousScrollButtonTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.PreviousScrollButtonTemplateChanged != null)
      this.PreviousScrollButtonTemplateChanged((DependencyObject) this, e);
    if (this.m_prevButton == null)
      return;
    this.m_prevButton.UpdateCellTemplate((ControlTemplate) e.NewValue);
  }

  protected virtual void OnDayCellsDataTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DayCellsDataTemplateChanged != null)
      this.DayCellsDataTemplateChanged((DependencyObject) this, e);
    DataTemplate newValue = (DataTemplate) e.NewValue;
    DataTemplateSelector templateSelector = this.DayCellsDataTemplateSelector;
    DataTemplatesDictionary dateDataTemplates = this.DateDataTemplates;
    this.CurrentDayGrid.UpdateTemplateAndSelector(newValue, templateSelector, dateDataTemplates);
    this.FollowingDayGrid.UpdateTemplateAndSelector(newValue, templateSelector, dateDataTemplates);
  }

  protected virtual void OnDayCellsStyleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DayCellsStyleChanged != null)
      this.DayCellsStyleChanged((DependencyObject) this, e);
    Style newValue = (Style) e.NewValue;
    if (this.CurrentDayGrid != null)
      this.CurrentDayGrid.UpdateStyles(newValue, this.DateStyles);
    if (this.FollowingDayGrid == null)
      return;
    this.FollowingDayGrid.UpdateStyles(newValue, this.DateStyles);
  }

  protected virtual void OnDayNameCellsStyleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DayNameCellsStyleChanged != null)
      this.DayNameCellsStyleChanged((DependencyObject) this, e);
    this.DayNamesGrid.UpdateStyles((Style) e.NewValue);
  }

  protected virtual void OnDayCellsDataTemplateSelectorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DayCellsDataTemplateSelectorChanged != null)
      this.DayCellsDataTemplateSelectorChanged((DependencyObject) this, e);
    DataTemplate cellsDataTemplate = this.DayCellsDataTemplate;
    DataTemplateSelector newValue = (DataTemplateSelector) e.NewValue;
    DataTemplatesDictionary dateDataTemplates = this.DateDataTemplates;
    this.CurrentDayGrid.UpdateTemplateAndSelector(cellsDataTemplate, newValue, dateDataTemplates);
    this.FollowingDayGrid.UpdateTemplateAndSelector(cellsDataTemplate, newValue, dateDataTemplates);
  }

  protected virtual void OnDayNameCellsDataTemplateSelectorChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.DayNameCellsDataTemplateSelectorChanged != null)
      this.DayNameCellsDataTemplateSelectorChanged((DependencyObject) this, e);
    this.DayNamesGrid.UpdateTemplateAndSelector(this.DayNameCellsDataTemplate, (DataTemplateSelector) e.NewValue);
  }

  protected virtual void OnDateDataTemplatesChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DateDataTemplatesChanged != null)
      this.DateDataTemplatesChanged((DependencyObject) this, e);
    if (e.OldValue != null)
      ((DataTemplatesDictionary) e.OldValue).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.DateDataTemplates_OnPropertyChanged);
    if ((DataTemplatesDictionary) e.NewValue != null)
      ((DataTemplatesDictionary) e.NewValue).CollectionChanged += new NotifyCollectionChangedEventHandler(this.DateDataTemplates_OnPropertyChanged);
    if (!this.IsInitializeComplete)
      return;
    this.InitilizeDayCellTemplates(this.CurrentDayGrid);
    this.InitilizeDayCellTemplates(this.FollowingDayGrid);
  }

  protected virtual DateTime OnCoerceDate(DateTime date)
  {
    if (date > this.MaxDate)
      return this.MaxDate;
    return date < this.MinDate ? this.MinDate : date;
  }

  private void OnAnimationCompleted(object sender, EventArgs e)
  {
    this.m_monthButton2.Visibility = Visibility.Hidden;
    this.CurrentDayGrid.Visibility = Visibility.Hidden;
    this.CurrentDayGrid.SelectionBorder.Visibility = Visibility.Visible;
    this.FollowingDayGrid.SelectionBorder.Visibility = Visibility.Visible;
    this.CurrentDayGrid.ClearValue(FrameworkElement.FocusVisualStyleProperty);
    this.FollowingDayGrid.ClearValue(FrameworkElement.FocusVisualStyleProperty);
    if (this.CurrentDayGrid.IsFocused || this.FollowingDayGrid.IsFocused)
      this.FollowingDayGrid.Focus();
    AdornerLayer.GetAdornerLayer((Visual) this.FollowingDayGrid)?.Update();
    if (this.m_iscrollCounter != 0)
    {
      if (this.m_iscrollCounter > 0)
        this.BeginMoving(CalendarEdit.MoveDirection.Next, this.m_iscrollCounter);
      else
        this.BeginMoving(CalendarEdit.MoveDirection.Prev, this.m_iscrollCounter);
      this.m_iscrollCounter = 0;
    }
    if (!this.m_postScrollNeed)
      return;
    this.m_postScrollNeed = false;
    this.ScrollToDate();
  }

  private void UpdateMinDate(DependencyPropertyChangedEventArgs e)
  {
    CultureInfo newValue = (CultureInfo) e.NewValue;
    CultureInfo oldValue = (CultureInfo) e.OldValue;
    if (newValue == null)
      return;
    this.Calendar = !(newValue.Name == "ar-SA") ? newValue.Calendar : oldValue.Calendar;
    this.m_oldMinDate = oldValue.Calendar.MinSupportedDateTime;
    this.m_newMinDate = newValue.Calendar.MinSupportedDateTime;
    if (this.MinDate == this.m_oldMinDate)
      this.MinDate = this.m_newMinDate;
    if (!(this.miDate == this.m_oldMinDate))
      return;
    this.miDate = this.m_newMinDate;
  }

  private void VisibleDataToMinSupportedDate(DateTimeFormatInfo format)
  {
    DateTime supportedDateTime = format.Calendar.MinSupportedDateTime;
    if (!(this.Date == supportedDateTime))
      return;
    this.VisibleData = new VisibleDate(supportedDateTime.Year, supportedDateTime.Month, supportedDateTime.Day);
  }

  private void InitializePopup()
  {
    if (this.m_popup == null || this.Calendar == null)
      return;
    this.m_popup.Format = this.Culture.DateTimeFormat;
    this.m_popup.CurrentDate = new Syncfusion.Windows.Shared.Date(this.VisibleData.VisibleYear, this.VisibleData.VisibleMonth, this.VisibleData.VisibleDay);
    if (this.MinMaxHidden)
    {
      this.m_popup.MinDate = new Syncfusion.Windows.Shared.Date(this.MinDate, this.Calendar);
      this.m_popup.MaxDate = new Syncfusion.Windows.Shared.Date(this.MaxDate, this.Calendar);
    }
    else
    {
      this.m_popup.MinDate = new Syncfusion.Windows.Shared.Date(this.miDate, this.Calendar);
      this.m_popup.MaxDate = new Syncfusion.Windows.Shared.Date(this.mxDate, this.Calendar);
    }
  }

  private void InitVisibleDayGrid()
  {
    if (this.FollowingDayGrid.Visibility == Visibility.Visible)
      this.FollowingDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
    else
      this.CurrentDayGrid.Initialize(this.VisibleData, this.Culture, this.Calendar);
  }

  private static void OnInValidDateBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.BlackoutDatesBorderBrush = calendarEdit.InValidDateBorderBrush;
  }

  private static void OnInValidDateForeGroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.BlackoutDatesForeground = calendarEdit.InValidDateForeGround;
  }

  private static void OnInValidDateBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.BlackoutDatesBackground = calendarEdit.InValidDateBackground;
  }

  private static void OnInValidDateCrossBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.BlackoutDatesCrossBrush = calendarEdit.InValidDateCrossBackground;
  }

  private static void OnIsDayNamesAbbreviatedChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.ShowAbbreviatedDayNames = calendarEdit.IsDayNamesAbbreviated;
  }

  private static void OnIsMonthNamesAbbreviatedChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.ShowAbbreviatedMonthNames = calendarEdit.IsMonthNameAbbreviated;
  }

  private static void OnIsShowWeekNumbersChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.ShowWeekNumbers = calendarEdit.IsShowWeekNumbers;
  }

  private static void OnIsAllowYearSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CalendarEdit calendarEdit = (CalendarEdit) d;
    calendarEdit.AllowYearEditing = calendarEdit.IsAllowYearSelection;
  }

  protected enum MoveDirection
  {
    Next,
    Prev,
  }

  protected enum HighlightSate
  {
    Begin,
    Stop,
  }

  protected enum ChangeMonthMode
  {
    Enabled,
    Disabled,
  }

  protected enum CollectionChangedAction
  {
    Add,
    Remove,
    Reset,
    Replace,
  }

  protected enum ChangeVisualModeDirection
  {
    Up,
    Down,
  }

  protected struct VisualModeHistory(CalendarVisualMode oldMode, CalendarVisualMode newMode)
  {
    public CalendarVisualMode OldMode = oldMode;
    public CalendarVisualMode NewMode = newMode;
  }

  public delegate void MonthChangedEventHandler(object sender, MonthChangedEventArgs args);
}
