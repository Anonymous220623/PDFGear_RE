// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Clock
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (Clock), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/Clock/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/VS2010Style.xaml")]
[DesignTimeVisible(false)]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (Clock), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/Clock/Themes/ShinyBlueStyle.xaml")]
public class Clock : Control, IDisposable
{
  private const double C_firstBorderFrameRadius = 151.0;
  private const double C_secondInnerBorderFrameRadius = 135.0;
  private const double C_thirdBorderFrameRadius = 122.0;
  private const double C_centeredEllipseRadius = 10.0;
  private const double C_innerClockGeneralWidth = 125.0;
  private const double C_innerClockGeneralHeight = 130.0;
  private const double C_frameMinWidth = 174.0;
  private const int C_angleOffset = 180;
  private const int C_rotationOffset = 90;
  private const string C_hourHandName = "HourHand";
  private const string C_minuteHandName = "MinuteHand";
  private const string C_secondHandName = "SecondHand";
  private const string C_hourHandRotateTransformName = "HourHandRotateTransform";
  private const string C_minuteHandRotateTransformName = "MinuteHandRotateTransform";
  private const string C_secondHandRotateTransformHandName = "SecondHandRotateTransform";
  private const string C_centeredEllipseName = "CenteredEllipse";
  private const string C_upRepeatButtonName = "UpRepeatButton";
  private const string C_downRepeatButtonName = "DownRepeatButton";
  private const string C_upInnerRepeatButtonName = "UpInnerRepeatButton";
  private const string C_downInnerRepeatButtonName = "DownInnerRepeatButton";
  private const string C_firstBorderFrameColorName = "FirstBorderFrameColor";
  private const string C_thirdBorderFrameBackgroundColorName = "ThirdBorderFrameBackgroundColor";
  private const string C_thirdBorderFrameBrushColorName = "ThirdBorderFrameBrushColor";
  private const string C_dialBorderColorName = "DialBorderColor";
  private const string C_clockPanelBorderColorName = "ClockPanelBorderColor";
  private const string C_clockPanelInnerBorderColorName = "ClockPanelInnerBorderColor";
  private const string C_clockPanelBackgroundColorName = "ClockPanelBackgroundColor";
  private const string C_aMPMSelectorBorderBrushName = "ArrowBorderColor";
  private const string C_aMPMSelectorBackgroundName = "TextBlockBackground";
  private const string C_aMPMSelectorForegroundName = "TextBlockForeground";
  private const string C_aMPMSelectorButtonsArrowBrushName = "ArrowFill";
  private const string C_aMPMSelectorButtonsBackgroundName = "RectangleStyle";
  private const string C_aMPMSelectorButtonsBorderBrushName = "ArrowBorderColor";
  private const string C_aMPMMouseOverButtonsBorderBrushName = "ArrowBorderColorOver";
  private const string C_aMPMMouseOverButtonsArrowBrushName = "ArrowFill";
  private const string C_aMPMMouseOverButtonsBackgroundName = "RectangleOverStyle";
  private const string C_ClockPointBrushName = "ClockPointColor";
  private const string C_centerCircleBrushName = "CenteredEllipseColor";
  private const string C_secondHandBrushName = "SecondHandColor";
  private const string C_secondHandMouseOverBrushName = "SecondHandMouseOverColor";
  private const string C_minuteHandBrushName = "MinuteHandColor";
  private const string C_minuteHandBorderBrushName = "MinuteHandColor";
  private const string C_minuteHandMouseOverBrushName = "MinuteHandMouseOverColor";
  private const string C_minuteHandMouseOverBorderBrushName = "MinuteHandMouseOverColor";
  private const string C_hourHandBrushName = "HourHandColor";
  private const string C_hourHandBorderBrushName = "HourHandColor";
  private const string C_hourHandMouseOverBrushName = "HourHandMouseOverColor";
  private const string C_hourHandMouseOverBorderBrushName = "HourHandMouseOverColor";
  private const string C_hourHandPressedBrushName = "HourHandPressedColor";
  private const string C_minuteHandPressedBrushName = "MinuteHandPressedColor";
  private const string C_secondHandPressedBrushName = "SecondHandPressedColor";
  private DispatcherTimer m_timer1;
  private Path m_hourHand;
  private Path m_minuteHand;
  private Rectangle m_secondHand;
  private Ellipse m_centeredEllipse;
  private RotateTransform m_hourHandRotateTransform;
  private RotateTransform m_minuteHandRotateTransform;
  private RotateTransform m_secondHandRotateTransform;
  private int m_hoursAdded;
  private int m_minutesAdded;
  private int m_secondsAdded;
  public static RoutedCommand m_AMPMSelect = new RoutedCommand();
  private CommandBinding amPMSelectBinding;
  internal bool timerstatus = true;
  public static readonly DependencyProperty CanResizeProperty = DependencyProperty.Register(nameof (CanResize), typeof (bool), typeof (Clock), (PropertyMetadata) new UIPropertyMetadata((object) false));
  internal static readonly DependencyProperty IsPressedHourHandProperty = DependencyProperty.Register(nameof (IsPressedHourHand), typeof (bool), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  internal static readonly DependencyProperty IsPressedMinuteHandProperty = DependencyProperty.Register(nameof (IsPressedMinuteHand), typeof (bool), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  internal static readonly DependencyProperty IsPressedSecondHandProperty = DependencyProperty.Register(nameof (IsPressedSecondHand), typeof (bool), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  internal static readonly DependencyProperty FirstBorderFrameRadiusProperty = DependencyProperty.Register(nameof (FirstBorderFrameRadius), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 151.0));
  internal static readonly DependencyProperty SecondInnerBorderFrameRadiusProperty = DependencyProperty.Register(nameof (SecondInnerBorderFrameRadius), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 135.0));
  internal static readonly DependencyProperty ThirdBorderFrameRadiusProperty = DependencyProperty.Register(nameof (ThirdBorderFrameRadius), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 122.0));
  internal static readonly DependencyProperty CenteredEllipseRadiusProperty = DependencyProperty.Register(nameof (CenteredEllipseRadius), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 10.0));
  internal static readonly DependencyProperty InnerClockGeneralWidthProperty = DependencyProperty.Register(nameof (InnerClockGeneralWidth), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 125.0));
  internal static readonly DependencyProperty InnerClockGeneralHeightProperty = DependencyProperty.Register(nameof (InnerClockGeneralHeight), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 130.0));
  internal static readonly DependencyProperty SecondHandHeightProperty = DependencyProperty.Register(nameof (SecondHandHeight), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 52.0));
  internal static readonly DependencyProperty FrameWidthProperty = DependencyProperty.Register(nameof (FrameWidth), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0));
  internal static DependencyProperty LongTimeProperty = DependencyProperty.Register(nameof (LongTime), typeof (string), typeof (Clock));
  private static readonly Brush c_defaultBrushValue = (Brush) Brushes.Transparent;
  private static readonly Color c_defaultColorValue = Colors.Transparent;
  public static DependencyProperty DateTimeProperty = DependencyProperty.Register(nameof (DateTime), typeof (DateTime), typeof (Clock), new PropertyMetadata((object) DateTime.Now, new PropertyChangedCallback(Clock.OnDateTimeChanged)));
  public static readonly DependencyProperty ClockCornerRadiusProperty = DependencyProperty.Register(nameof (ClockCornerRadius), typeof (CornerRadius), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(90.0), new PropertyChangedCallback(Clock.OnClockCornerRadiusChanged)));
  public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(2.0), new PropertyChangedCallback(Clock.OnBorderThicknessChanged)));
  public static readonly DependencyProperty SecondHandThicknessProperty = DependencyProperty.Register(nameof (SecondHandThickness), typeof (double), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) 2.0, new PropertyChangedCallback(Clock.OnSecondHandThicknessChanged)));
  public static readonly DependencyProperty InnerBorderThicknessProperty = DependencyProperty.Register(nameof (InnerBorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(2.0), new PropertyChangedCallback(Clock.OnInnerBorderThicknessChanged)));
  public static readonly DependencyProperty DialBorderThicknessProperty = DependencyProperty.Register(nameof (DialBorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(25.0), new PropertyChangedCallback(Clock.OnDialBorderThicknessChanged)));
  public static readonly DependencyProperty AMPMSelectorPositionProperty = DependencyProperty.Register(nameof (AMPMSelectorPosition), typeof (Clock.Position), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.Position.Bottom, new PropertyChangedCallback(Clock.OnAMPMSelectorPositionChanged)));
  public static readonly DependencyProperty InnerBorderBrushProperty = DependencyProperty.Register(nameof (InnerBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnInnerBorderBrushChanged)));
  public static readonly DependencyProperty DialBackgroundProperty = DependencyProperty.Register(nameof (DialBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnDialBackgroundChanged)));
  public static readonly DependencyProperty DialCenterBackgroundProperty = DependencyProperty.Register(nameof (DialCenterBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnDialCenterBackgroundChanged)));
  public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof (BorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnBorderBrushChanged)));
  public static readonly DependencyProperty ClockFrameBrushProperty = DependencyProperty.Register(nameof (ClockFrameBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnClockFrameBrushChanged)));
  public static readonly DependencyProperty FrameBorderThicknessProperty = DependencyProperty.Register(nameof (FrameBorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(1.0), new PropertyChangedCallback(Clock.OnFrameBorderThicknessChanged)));
  public static readonly DependencyProperty FrameInnerBorderThicknessProperty = DependencyProperty.Register(nameof (FrameInnerBorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(1.0, 1.0, 1.0, 0.0), new PropertyChangedCallback(Clock.OnFrameInnerBorderThicknessChanged)));
  public static readonly DependencyProperty FrameBorderBrushProperty = DependencyProperty.Register(nameof (FrameBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnFrameBorderBrushChanged)));
  public static readonly DependencyProperty FrameInnerBorderBrushProperty = DependencyProperty.Register(nameof (FrameInnerBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnFrameInnerBorderBrushChanged)));
  public static readonly DependencyProperty FrameBackgroundProperty = DependencyProperty.Register(nameof (FrameBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnFrameBackgroundChanged)));
  public static readonly DependencyProperty FrameCornerRadiusProperty = DependencyProperty.Register(nameof (FrameCornerRadius), typeof (CornerRadius), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(0.0), new PropertyChangedCallback(Clock.OnFrameCornerRadiusChanged)));
  public static readonly DependencyProperty AMPMSelectorBorderThicknessProperty = DependencyProperty.Register(nameof (AMPMSelectorBorderThickness), typeof (Thickness), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(1.0), new PropertyChangedCallback(Clock.OnAMPMSelectorBorderThicknessChanged)));
  public static readonly DependencyProperty AMPMSelectorCornerRadiusProperty = DependencyProperty.Register(nameof (AMPMSelectorCornerRadius), typeof (CornerRadius), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(0.0), new PropertyChangedCallback(Clock.OnAMPMSelectorCornerRadiusChanged)));
  public static readonly DependencyProperty AMPMSelectorBorderBrushProperty = DependencyProperty.Register(nameof (AMPMSelectorBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorBorderBrushChanged)));
  public static readonly DependencyProperty AMPMSelectorBackgroundProperty = DependencyProperty.Register(nameof (AMPMSelectorBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorBackgroundChanged)));
  public static readonly DependencyProperty AMPMSelectorForegroundProperty = DependencyProperty.Register(nameof (AMPMSelectorForeground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorForegroundChanged)));
  public static readonly DependencyProperty AMPMSelectorButtonsArrowBrushProperty = DependencyProperty.Register(nameof (AMPMSelectorButtonsArrowBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorButtonsArrowBrushChanged)));
  public static readonly DependencyProperty AMPMSelectorButtonsBackgroundProperty = DependencyProperty.Register(nameof (AMPMSelectorButtonsBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorButtonsBackgroundChanged)));
  public static readonly DependencyProperty AMPMSelectorButtonsBorderBrushProperty = DependencyProperty.Register(nameof (AMPMSelectorButtonsBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMSelectorButtonsBorderBrushChanged)));
  public static readonly DependencyProperty AMPMMouseOverButtonsBorderBrushProperty = DependencyProperty.Register(nameof (AMPMMouseOverButtonsBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMMouseOverButtonsBorderBrushChanged)));
  public static readonly DependencyProperty AMPMMouseOverButtonsArrowBrushProperty = DependencyProperty.Register(nameof (AMPMMouseOverButtonsArrowBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMMouseOverButtonsArrowBrushChanged)));
  public static readonly DependencyProperty AMPMMouseOverButtonsBackgroundProperty = DependencyProperty.Register(nameof (AMPMMouseOverButtonsBackground), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnAMPMMouseOverButtonsBackgroundChanged)));
  public static readonly DependencyProperty ClockPointBrushProperty = DependencyProperty.Register(nameof (ClockPointBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnClockPointBrushChanged)));
  public static readonly DependencyProperty CenterCircleBrushProperty = DependencyProperty.Register(nameof (CenterCircleBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnCenterCircleBrushChanged)));
  public static readonly DependencyProperty SecondHandBrushProperty = DependencyProperty.Register(nameof (SecondHandBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnSecondHandBrushChanged)));
  public static readonly DependencyProperty SecondHandMouseOverBrushProperty = DependencyProperty.Register(nameof (SecondHandMouseOverBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnSecondHandMouseOverBrushChanged)));
  public static readonly DependencyProperty MinuteHandBrushProperty = DependencyProperty.Register(nameof (MinuteHandBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnMinuteHandBrushChanged)));
  public static readonly DependencyProperty MinuteHandBorderBrushProperty = DependencyProperty.Register(nameof (MinuteHandBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnMinuteHandBorderBrushChanged)));
  public static readonly DependencyProperty MinuteHandMouseOverBrushProperty = DependencyProperty.Register(nameof (MinuteHandMouseOverBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnMinuteHandMouseOverBrushChanged)));
  public static readonly DependencyProperty MinuteHandMouseOverBorderBrushProperty = DependencyProperty.Register(nameof (MinuteHandMouseOverBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnMinuteHandMouseOverBorderBrushChanged)));
  public static readonly DependencyProperty HourHandBrushProperty = DependencyProperty.Register(nameof (HourHandBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnHourHandBrushChanged)));
  public static readonly DependencyProperty HourHandBorderBrushProperty = DependencyProperty.Register(nameof (HourHandBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnHourHandBorderBrushChanged)));
  public static readonly DependencyProperty HourHandMouseOverBrushProperty = DependencyProperty.Register(nameof (HourHandMouseOverBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnHourHandMouseOverBrushChanged)));
  public static readonly DependencyProperty HourHandMouseOverBorderBrushProperty = DependencyProperty.Register(nameof (HourHandMouseOverBorderBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnHourHandMouseOverBorderBrushChanged)));
  public static readonly DependencyProperty HourHandPressedBrushProperty = DependencyProperty.Register(nameof (HourHandPressedBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnHourHandPressedBrushChanged)));
  public static readonly DependencyProperty MinuteHandPressedBrushProperty = DependencyProperty.Register(nameof (MinuteHandPressedBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnMinuteHandPressedBrushChanged)));
  public static readonly DependencyProperty SecondHandPressedBrushProperty = DependencyProperty.Register(nameof (SecondHandPressedBrush), typeof (Brush), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Clock.c_defaultBrushValue, new PropertyChangedCallback(Clock.OnSecondHandPressedBrushChanged)));
  public static readonly DependencyProperty IsInsideAmPmVisibleProperty = DependencyProperty.Register(nameof (IsInsideAmPmVisible), typeof (bool), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(Clock.OnIsInsideAmPmVisibleChanged)));
  public static readonly DependencyProperty IsDigitalAmPmVisibleProperty = DependencyProperty.Register(nameof (IsDigitalAmPmVisible), typeof (bool), typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(Clock.OnIsDigitalAmPmVisibleChanged)));

  static Clock()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Clock), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Clock)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public Clock()
  {
    this.amPMSelectBinding = new CommandBinding((ICommand) Clock.m_AMPMSelect);
    this.amPMSelectBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeAMPMSelectValue);
    this.amPMSelectBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeAMPMSelectValue);
    this.CommandBindings.Add(this.amPMSelectBinding);
    this.m_timer1 = new DispatcherTimer();
    this.m_timer1.Interval = TimeSpan.FromMilliseconds(1000.0);
    this.m_timer1.Tick -= new EventHandler(this.Timer_Tick);
    this.m_timer1.Tick += new EventHandler(this.Timer_Tick);
    this.Loaded += new RoutedEventHandler(this.Clock_Loaded);
    this.Unloaded += new RoutedEventHandler(this.Clock_Unloaded);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void Clock_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.m_timer1 != null)
    {
      this.m_timer1.IsEnabled = true;
      this.m_timer1.Start();
    }
    else
    {
      this.m_timer1 = new DispatcherTimer();
      this.m_timer1.Interval = TimeSpan.FromMilliseconds(1000.0);
      this.m_timer1.Tick -= new EventHandler(this.Timer_Tick);
      this.m_timer1.Tick += new EventHandler(this.Timer_Tick);
      this.m_timer1.IsEnabled = true;
      this.m_timer1.Start();
    }
  }

  private void Clock_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.m_timer1 == null)
      return;
    this.m_timer1.IsEnabled = false;
    this.m_timer1.Stop();
    this.m_timer1 = (DispatcherTimer) null;
  }

  public void Dispose()
  {
    if (this.m_timer1 != null)
    {
      this.m_timer1.Tick -= new EventHandler(this.Timer_Tick);
      this.m_timer1.IsEnabled = false;
      this.m_timer1.Stop();
      this.m_timer1 = (DispatcherTimer) null;
    }
    this.Loaded -= new RoutedEventHandler(this.Clock_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.Clock_Unloaded);
    if (this.amPMSelectBinding != null)
    {
      this.amPMSelectBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeAMPMSelectValue);
      this.CommandBindings.Remove(this.amPMSelectBinding);
    }
    this.CommandBindings.Clear();
    BindingOperations.ClearAllBindings((DependencyObject) this);
    if (this.m_hourHand != null)
    {
      this.m_hourHand.MouseLeftButtonDown -= new MouseButtonEventHandler(this.HourHand_MouseLeftButtonDown);
      this.m_hourHand = (Path) null;
    }
    if (this.m_minuteHand != null)
    {
      this.m_minuteHand.MouseLeftButtonDown -= new MouseButtonEventHandler(this.MinuteHand_MouseLeftButtonDown);
      this.m_minuteHand = (Path) null;
    }
    if (this.m_secondHand != null)
    {
      this.m_secondHand.MouseLeftButtonDown -= new MouseButtonEventHandler(this.SecondHand_MouseLeftButtonDown);
      this.m_secondHand = (Rectangle) null;
    }
    if (this.m_centeredEllipse != null)
      this.m_centeredEllipse = (Ellipse) null;
    if (this.m_hourHandRotateTransform != null)
      this.m_hourHandRotateTransform = (RotateTransform) null;
    if (this.m_minuteHandRotateTransform != null)
      this.m_minuteHandRotateTransform = (RotateTransform) null;
    if (this.m_secondHandRotateTransform == null)
      return;
    this.m_secondHandRotateTransform = (RotateTransform) null;
  }

  void IDisposable.Dispose() => this.Dispose();

  public void Stop()
  {
    if (this.m_timer1 != null)
      this.m_timer1.Stop();
    this.timerstatus = false;
  }

  public void Start()
  {
    if (this.m_timer1 != null)
      this.m_timer1.Start();
    this.timerstatus = true;
  }

  internal bool IsPressedHourHand
  {
    get => (bool) this.GetValue(Clock.IsPressedHourHandProperty);
    set => this.SetValue(Clock.IsPressedHourHandProperty, (object) value);
  }

  internal bool IsPressedMinuteHand
  {
    get => (bool) this.GetValue(Clock.IsPressedMinuteHandProperty);
    set => this.SetValue(Clock.IsPressedMinuteHandProperty, (object) value);
  }

  internal bool IsPressedSecondHand
  {
    get => (bool) this.GetValue(Clock.IsPressedSecondHandProperty);
    set => this.SetValue(Clock.IsPressedSecondHandProperty, (object) value);
  }

  internal double FirstBorderFrameRadius
  {
    get => (double) this.GetValue(Clock.FirstBorderFrameRadiusProperty);
    set => this.SetValue(Clock.FirstBorderFrameRadiusProperty, (object) value);
  }

  internal double SecondInnerBorderFrameRadius
  {
    get => (double) this.GetValue(Clock.SecondInnerBorderFrameRadiusProperty);
    set => this.SetValue(Clock.SecondInnerBorderFrameRadiusProperty, (object) value);
  }

  internal double ThirdBorderFrameRadius
  {
    get => (double) this.GetValue(Clock.ThirdBorderFrameRadiusProperty);
    set => this.SetValue(Clock.ThirdBorderFrameRadiusProperty, (object) value);
  }

  internal double CenteredEllipseRadius
  {
    get => (double) this.GetValue(Clock.CenteredEllipseRadiusProperty);
    set => this.SetValue(Clock.CenteredEllipseRadiusProperty, (object) value);
  }

  internal double InnerClockGeneralWidth
  {
    get => (double) this.GetValue(Clock.InnerClockGeneralWidthProperty);
    set => this.SetValue(Clock.InnerClockGeneralWidthProperty, (object) value);
  }

  public bool CanResize
  {
    get => (bool) this.GetValue(Clock.CanResizeProperty);
    set => this.SetValue(Clock.CanResizeProperty, (object) value);
  }

  internal double InnerClockGeneralHeight
  {
    get => (double) this.GetValue(Clock.InnerClockGeneralHeightProperty);
    set => this.SetValue(Clock.InnerClockGeneralHeightProperty, (object) value);
  }

  internal double SecondHandHeight
  {
    get => (double) this.GetValue(Clock.SecondHandHeightProperty);
    set => this.SetValue(Clock.SecondHandHeightProperty, (object) value);
  }

  internal double FrameWidth
  {
    get => (double) this.GetValue(Clock.FrameWidthProperty);
    set => this.SetValue(Clock.FrameWidthProperty, (object) value);
  }

  internal string LongTime
  {
    get => (string) this.GetValue(Clock.LongTimeProperty);
    private set => this.SetValue(Clock.LongTimeProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.FrameWidth = 174.0 + this.FrameBorderThickness.Left + this.FrameBorderThickness.Right + this.FrameInnerBorderThickness.Left + this.FrameInnerBorderThickness.Right;
    this.InitHands();
    this.InitHandsRotateTransform();
    this.m_centeredEllipse = this.Template.FindName("CenteredEllipse", (FrameworkElement) this) as Ellipse;
    this.LongTime = this.DateTime.ToString("T", (IFormatProvider) CultureInfo.CurrentUICulture);
  }

  protected override void OnInitialized(EventArgs e)
  {
    base.OnInitialized(e);
    this.UpdateDateTime();
  }

  private void InitHands()
  {
    this.m_hourHand = this.Template.FindName("HourHand", (FrameworkElement) this) as Path;
    this.m_hourHand.MouseLeftButtonDown += new MouseButtonEventHandler(this.HourHand_MouseLeftButtonDown);
    this.m_minuteHand = this.Template.FindName("MinuteHand", (FrameworkElement) this) as Path;
    this.m_minuteHand.MouseLeftButtonDown += new MouseButtonEventHandler(this.MinuteHand_MouseLeftButtonDown);
    this.m_secondHand = this.Template.FindName("SecondHand", (FrameworkElement) this) as Rectangle;
    this.m_secondHand.MouseLeftButtonDown += new MouseButtonEventHandler(this.SecondHand_MouseLeftButtonDown);
  }

  private void InitHandsRotateTransform()
  {
    this.m_hourHandRotateTransform = this.Template.FindName("HourHandRotateTransform", (FrameworkElement) this) as RotateTransform;
    this.m_minuteHandRotateTransform = this.Template.FindName("MinuteHandRotateTransform", (FrameworkElement) this) as RotateTransform;
    this.m_secondHandRotateTransform = this.Template.FindName("SecondHandRotateTransform", (FrameworkElement) this) as RotateTransform;
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    if (!this.timerstatus)
      return;
    this.UpdateDateTime();
  }

  private void UpdateDateTime()
  {
    if (this.m_secondsAdded != 0)
    {
      this.DateTime += TimeSpan.FromSeconds((double) this.m_secondsAdded);
      this.m_secondsAdded = 0;
    }
    if (this.m_minutesAdded != 0)
    {
      this.DateTime += TimeSpan.FromMinutes((double) this.m_minutesAdded);
      this.m_minutesAdded = 0;
    }
    if (this.m_hoursAdded != 0)
    {
      this.DateTime += TimeSpan.FromHours((double) this.m_hoursAdded);
      this.m_hoursAdded = 0;
    }
    if (!(this.DateTime != DateTime.MaxValue))
      return;
    this.DateTime += TimeSpan.FromSeconds(1.0);
  }

  protected virtual void OnDateTimeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DateTimeChanged == null)
      return;
    this.DateTimeChanged((DependencyObject) this, e);
  }

  private static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Clock clock = (Clock) d;
    DateTime newValue = (DateTime) e.NewValue;
    clock.LongTime = newValue.ToString("T", (IFormatProvider) CultureInfo.CurrentUICulture);
    clock.OnDateTimeChanged(e);
  }

  private void HourHand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.m_timer1.Stop();
    this.IsPressedHourHand = true;
  }

  private void MinuteHand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.m_timer1.Stop();
    this.IsPressedMinuteHand = true;
  }

  private void SecondHand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.m_timer1.Stop();
    this.IsPressedSecondHand = true;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    Point position = e.GetPosition((IInputElement) this.m_centeredEllipse);
    double num = Math.Atan2(position.Y, position.X) * 180.0 / Math.PI + 90.0;
    if (num < 0.0)
      num = 360.0 + num;
    TimeSpan timeSpan = new TimeSpan(this.DateTime.Hour, this.DateTime.Minute, this.DateTime.Second);
    if (this.IsPressedHourHand && this.DateTime != DateTime.MaxValue)
      this.DateTime += TimeSpan.FromHours((double) this.CorrectHourDifference((TimeSpan.FromHours(num / 30.0) - timeSpan).Hours));
    else if (this.IsPressedMinuteHand && this.DateTime != DateTime.MaxValue)
    {
      this.DateTime += TimeSpan.FromMinutes((double) this.CorrectTimeDifference(TimeSpan.FromMinutes(num / 6.0).Minutes - timeSpan.Minutes));
    }
    else
    {
      if (!this.IsPressedSecondHand || !(this.DateTime != DateTime.MaxValue))
        return;
      this.DateTime += TimeSpan.FromSeconds((double) this.CorrectTimeDifference(TimeSpan.FromSeconds(num / 6.0).Seconds - timeSpan.Seconds));
    }
  }

  private int CorrectHourDifference(int diff)
  {
    if (diff < -10)
      diff += 12;
    if (diff > 10)
      diff -= 12;
    return diff;
  }

  private int CorrectTimeDifference(int diff)
  {
    if (diff > 50)
      diff = -1;
    if (diff < -50)
      diff = 1;
    return diff;
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (this.IsCtrlAltPressed())
      this.SecondMouseWheelRotation(e.Delta);
    else if (Keyboard.Modifiers == ModifierKeys.Control)
    {
      this.MinuteMouseWheelRotation(e.Delta);
    }
    else
    {
      if (Keyboard.Modifiers != ModifierKeys.None)
        return;
      this.HourMouseWheelRotation(e.Delta);
    }
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    this.ClearDraggingFlags();
    this.m_timer1.Start();
  }

  private bool IsCtrlAltPressed()
  {
    if (!Keyboard.IsKeyDown(Key.RightAlt) && !Keyboard.IsKeyDown(Key.LeftAlt))
      return false;
    return Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl);
  }

  private void ClearDraggingFlags()
  {
    this.IsPressedHourHand = false;
    this.IsPressedMinuteHand = false;
    this.IsPressedSecondHand = false;
  }

  private void HourMouseWheelRotation(int delta)
  {
    if (delta < 0)
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, TimeSpan.FromHours(this.m_hourHandRotateTransform.Angle / 30.0).Hours, this.DateTime.Minute, this.DateTime.Second);
      ++this.m_hoursAdded;
    }
    else
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, TimeSpan.FromHours(this.m_hourHandRotateTransform.Angle / 30.0).Hours, this.DateTime.Minute, this.DateTime.Second);
      --this.m_hoursAdded;
    }
  }

  private void MinuteMouseWheelRotation(int delta)
  {
    if (delta < 0)
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, this.DateTime.Hour, TimeSpan.FromHours(this.m_minuteHandRotateTransform.Angle / 360.0).Minutes, this.DateTime.Second);
      ++this.m_minutesAdded;
    }
    else
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, this.DateTime.Hour, TimeSpan.FromHours(this.m_minuteHandRotateTransform.Angle / 360.0).Minutes, this.DateTime.Second);
      --this.m_minutesAdded;
    }
  }

  private void SecondMouseWheelRotation(int delta)
  {
    if (delta < 0)
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, this.DateTime.Hour, this.DateTime.Minute, TimeSpan.FromMinutes(this.m_secondHandRotateTransform.Angle / 360.0).Seconds);
      ++this.m_secondsAdded;
    }
    else
    {
      this.DateTime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, this.DateTime.Hour, this.DateTime.Minute, TimeSpan.FromMinutes(this.m_secondHandRotateTransform.Angle / 360.0).Seconds);
      --this.m_secondsAdded;
    }
  }

  private void ChangeAMPMSelectValue(object sender, ExecutedRoutedEventArgs e)
  {
    FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
    DateTime dateTime = DateTime.MinValue;
    dateTime = dateTime.AddDays(1.0);
    if (this.LongTime.Contains("PM") && (originalSource.Name == "UpRepeatButton" || originalSource.Name == "UpInnerRepeatButton") && this.DateTime != DateTime.MaxValue)
      this.DateTime += TimeSpan.FromHours(12.0);
    else if (this.LongTime.Contains("AM") && (originalSource.Name == "DownRepeatButton" || originalSource.Name == "DownInnerRepeatButton") && dateTime < this.DateTime)
      this.DateTime -= TimeSpan.FromHours(12.0);
    else if (originalSource.Name == "UpRepeatButton")
    {
      if (this.IsCtrlAltPressed())
      {
        if (this.timerstatus)
          --this.m_secondsAdded;
        else
          this.DateTime += TimeSpan.FromSeconds(1.0);
      }
      else if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        if (this.timerstatus)
          ++this.m_minutesAdded;
        else
          this.DateTime += TimeSpan.FromMinutes(1.0);
      }
      else if (this.timerstatus)
        ++this.m_hoursAdded;
      else
        this.DateTime += TimeSpan.FromHours(1.0);
    }
    else
    {
      if (!(originalSource.Name == "DownRepeatButton"))
        return;
      if (this.IsCtrlAltPressed())
      {
        if (this.timerstatus)
          --this.m_secondsAdded;
        else
          this.DateTime -= TimeSpan.FromSeconds(1.0);
      }
      else if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        if (this.timerstatus)
          --this.m_minutesAdded;
        else
          this.DateTime -= TimeSpan.FromMinutes(1.0);
      }
      else if (this.timerstatus)
        --this.m_hoursAdded;
      else
        this.DateTime -= TimeSpan.FromHours(1.0);
    }
  }

  public DateTime DateTime
  {
    get => (DateTime) this.GetValue(Clock.DateTimeProperty);
    set => this.SetValue(Clock.DateTimeProperty, (object) value);
  }

  public CornerRadius ClockCornerRadius
  {
    get => (CornerRadius) this.GetValue(Clock.ClockCornerRadiusProperty);
    set => this.SetValue(Clock.ClockCornerRadiusProperty, (object) value);
  }

  public new Thickness BorderThickness
  {
    get => (Thickness) this.GetValue(Clock.BorderThicknessProperty);
    set => this.SetValue(Clock.BorderThicknessProperty, (object) value);
  }

  public double SecondHandThickness
  {
    get => (double) this.GetValue(Clock.SecondHandThicknessProperty);
    set => this.SetValue(Clock.SecondHandThicknessProperty, (object) value);
  }

  public Thickness InnerBorderThickness
  {
    get => (Thickness) this.GetValue(Clock.InnerBorderThicknessProperty);
    set => this.SetValue(Clock.InnerBorderThicknessProperty, (object) value);
  }

  public Thickness DialBorderThickness
  {
    get => (Thickness) this.GetValue(Clock.DialBorderThicknessProperty);
    set => this.SetValue(Clock.DialBorderThicknessProperty, (object) value);
  }

  public Clock.Position AMPMSelectorPosition
  {
    get => (Clock.Position) this.GetValue(Clock.AMPMSelectorPositionProperty);
    set => this.SetValue(Clock.AMPMSelectorPositionProperty, (object) value);
  }

  public Brush InnerBorderBrush
  {
    get => (Brush) this.GetValue(Clock.InnerBorderBrushProperty);
    set => this.SetValue(Clock.InnerBorderBrushProperty, (object) value);
  }

  public Brush DialBackground
  {
    get => (Brush) this.GetValue(Clock.DialBackgroundProperty);
    set => this.SetValue(Clock.DialBackgroundProperty, (object) value);
  }

  public Brush DialCenterBackground
  {
    get => (Brush) this.GetValue(Clock.DialCenterBackgroundProperty);
    set => this.SetValue(Clock.DialCenterBackgroundProperty, (object) value);
  }

  public new Brush BorderBrush
  {
    get => (Brush) this.GetValue(Clock.BorderBrushProperty);
    set => this.SetValue(Clock.BorderBrushProperty, (object) value);
  }

  public Brush ClockFrameBrush
  {
    get => (Brush) this.GetValue(Clock.ClockFrameBrushProperty);
    set => this.SetValue(Clock.ClockFrameBrushProperty, (object) value);
  }

  public Thickness FrameBorderThickness
  {
    get => (Thickness) this.GetValue(Clock.FrameBorderThicknessProperty);
    set => this.SetValue(Clock.FrameBorderThicknessProperty, (object) value);
  }

  public Thickness FrameInnerBorderThickness
  {
    get => (Thickness) this.GetValue(Clock.FrameInnerBorderThicknessProperty);
    set => this.SetValue(Clock.FrameInnerBorderThicknessProperty, (object) value);
  }

  public Brush FrameBorderBrush
  {
    get => (Brush) this.GetValue(Clock.FrameBorderBrushProperty);
    set => this.SetValue(Clock.FrameBorderBrushProperty, (object) value);
  }

  public Brush FrameInnerBorderBrush
  {
    get => (Brush) this.GetValue(Clock.FrameInnerBorderBrushProperty);
    set => this.SetValue(Clock.FrameInnerBorderBrushProperty, (object) value);
  }

  public Brush FrameBackground
  {
    get => (Brush) this.GetValue(Clock.FrameBackgroundProperty);
    set => this.SetValue(Clock.FrameBackgroundProperty, (object) value);
  }

  public CornerRadius FrameCornerRadius
  {
    get => (CornerRadius) this.GetValue(Clock.FrameCornerRadiusProperty);
    set => this.SetValue(Clock.FrameCornerRadiusProperty, (object) value);
  }

  public Thickness AMPMSelectorBorderThickness
  {
    get => (Thickness) this.GetValue(Clock.AMPMSelectorBorderThicknessProperty);
    set => this.SetValue(Clock.AMPMSelectorBorderThicknessProperty, (object) value);
  }

  public Brush AMPMSelectorBorderBrush
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorBorderBrushProperty);
    set => this.SetValue(Clock.AMPMSelectorBorderBrushProperty, (object) value);
  }

  public Brush AMPMSelectorBackground
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorBackgroundProperty);
    set => this.SetValue(Clock.AMPMSelectorBackgroundProperty, (object) value);
  }

  public Brush AMPMSelectorForeground
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorForegroundProperty);
    set => this.SetValue(Clock.AMPMSelectorForegroundProperty, (object) value);
  }

  public Brush AMPMSelectorButtonsArrowBrush
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorButtonsArrowBrushProperty);
    set => this.SetValue(Clock.AMPMSelectorButtonsArrowBrushProperty, (object) value);
  }

  public Brush AMPMSelectorButtonsBackground
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorButtonsBackgroundProperty);
    set => this.SetValue(Clock.AMPMSelectorButtonsBackgroundProperty, (object) value);
  }

  public CornerRadius AMPMSelectorCornerRadius
  {
    get => (CornerRadius) this.GetValue(Clock.AMPMSelectorCornerRadiusProperty);
    set => this.SetValue(Clock.AMPMSelectorCornerRadiusProperty, (object) value);
  }

  public Brush AMPMSelectorButtonsBorderBrush
  {
    get => (Brush) this.GetValue(Clock.AMPMSelectorButtonsBorderBrushProperty);
    set => this.SetValue(Clock.AMPMSelectorButtonsBorderBrushProperty, (object) value);
  }

  public Brush AMPMMouseOverButtonsBorderBrush
  {
    get => (Brush) this.GetValue(Clock.AMPMMouseOverButtonsBorderBrushProperty);
    set => this.SetValue(Clock.AMPMMouseOverButtonsBorderBrushProperty, (object) value);
  }

  public Brush AMPMMouseOverButtonsArrowBrush
  {
    get => (Brush) this.GetValue(Clock.AMPMMouseOverButtonsArrowBrushProperty);
    set => this.SetValue(Clock.AMPMMouseOverButtonsArrowBrushProperty, (object) value);
  }

  public Brush AMPMMouseOverButtonsBackground
  {
    get => (Brush) this.GetValue(Clock.AMPMMouseOverButtonsBackgroundProperty);
    set => this.SetValue(Clock.AMPMMouseOverButtonsBackgroundProperty, (object) value);
  }

  public Brush ClockPointBrush
  {
    get => (Brush) this.GetValue(Clock.ClockPointBrushProperty);
    set => this.SetValue(Clock.ClockPointBrushProperty, (object) value);
  }

  public Brush CenterCircleBrush
  {
    get => (Brush) this.GetValue(Clock.CenterCircleBrushProperty);
    set => this.SetValue(Clock.CenterCircleBrushProperty, (object) value);
  }

  public Brush SecondHandBrush
  {
    get => (Brush) this.GetValue(Clock.SecondHandBrushProperty);
    set => this.SetValue(Clock.SecondHandBrushProperty, (object) value);
  }

  public Brush SecondHandMouseOverBrush
  {
    get => (Brush) this.GetValue(Clock.SecondHandMouseOverBrushProperty);
    set => this.SetValue(Clock.SecondHandMouseOverBrushProperty, (object) value);
  }

  public Brush MinuteHandBrush
  {
    get => (Brush) this.GetValue(Clock.MinuteHandBrushProperty);
    set => this.SetValue(Clock.MinuteHandBrushProperty, (object) value);
  }

  public Brush MinuteHandBorderBrush
  {
    get => (Brush) this.GetValue(Clock.MinuteHandBorderBrushProperty);
    set => this.SetValue(Clock.MinuteHandBorderBrushProperty, (object) value);
  }

  public Brush MinuteHandMouseOverBrush
  {
    get => (Brush) this.GetValue(Clock.MinuteHandMouseOverBrushProperty);
    set => this.SetValue(Clock.MinuteHandMouseOverBrushProperty, (object) value);
  }

  public Brush MinuteHandMouseOverBorderBrush
  {
    get => (Brush) this.GetValue(Clock.MinuteHandMouseOverBorderBrushProperty);
    set => this.SetValue(Clock.MinuteHandMouseOverBorderBrushProperty, (object) value);
  }

  public Brush HourHandBrush
  {
    get => (Brush) this.GetValue(Clock.HourHandBrushProperty);
    set => this.SetValue(Clock.HourHandBrushProperty, (object) value);
  }

  public Brush HourHandBorderBrush
  {
    get => (Brush) this.GetValue(Clock.HourHandBorderBrushProperty);
    set => this.SetValue(Clock.HourHandBorderBrushProperty, (object) value);
  }

  public Brush HourHandMouseOverBrush
  {
    get => (Brush) this.GetValue(Clock.HourHandMouseOverBrushProperty);
    set => this.SetValue(Clock.HourHandMouseOverBrushProperty, (object) value);
  }

  public Brush HourHandMouseOverBorderBrush
  {
    get => (Brush) this.GetValue(Clock.HourHandMouseOverBorderBrushProperty);
    set => this.SetValue(Clock.HourHandMouseOverBorderBrushProperty, (object) value);
  }

  public Brush HourHandPressedBrush
  {
    get => (Brush) this.GetValue(Clock.HourHandPressedBrushProperty);
    set => this.SetValue(Clock.HourHandPressedBrushProperty, (object) value);
  }

  public Brush MinuteHandPressedBrush
  {
    get => (Brush) this.GetValue(Clock.MinuteHandPressedBrushProperty);
    set => this.SetValue(Clock.MinuteHandPressedBrushProperty, (object) value);
  }

  public Brush SecondHandPressedBrush
  {
    get => (Brush) this.GetValue(Clock.SecondHandPressedBrushProperty);
    set => this.SetValue(Clock.SecondHandPressedBrushProperty, (object) value);
  }

  public bool IsInsideAmPmVisible
  {
    get => (bool) this.GetValue(Clock.IsInsideAmPmVisibleProperty);
    set => this.SetValue(Clock.IsInsideAmPmVisibleProperty, (object) value);
  }

  public bool IsDigitalAmPmVisible
  {
    get => (bool) this.GetValue(Clock.IsDigitalAmPmVisibleProperty);
    set => this.SetValue(Clock.IsDigitalAmPmVisibleProperty, (object) value);
  }

  public event PropertyChangedCallback ClockCornerRadiusChanged;

  public event PropertyChangedCallback BorderThicknessChanged;

  public event PropertyChangedCallback SecondHandThicknessChanged;

  public event PropertyChangedCallback InnerBorderThicknessChanged;

  public event PropertyChangedCallback DialBorderThicknessChanged;

  public event PropertyChangedCallback AMPMSelectorPositionChanged;

  public event PropertyChangedCallback DateTimeChanged;

  public event PropertyChangedCallback BorderBrushChanged;

  public event PropertyChangedCallback ClockFrameBrushChanged;

  public event PropertyChangedCallback DialBackgroundChanged;

  public event PropertyChangedCallback DialCenterBackgroundChanged;

  public event PropertyChangedCallback InnerBorderBrushChanged;

  public event PropertyChangedCallback FrameBorderThicknessChanged;

  public event PropertyChangedCallback FrameInnerBorderThicknessChanged;

  public event PropertyChangedCallback FrameBorderBrushChanged;

  public event PropertyChangedCallback FrameInnerBorderBrushChanged;

  public event PropertyChangedCallback FrameBackgroundChanged;

  public event PropertyChangedCallback FrameCornerRadiusChanged;

  public event PropertyChangedCallback AMPMSelectorBorderThicknessChanged;

  public event PropertyChangedCallback AMPMSelectorBorderBrushChanged;

  public event PropertyChangedCallback AMPMSelectorBackgroundChanged;

  public event PropertyChangedCallback AMPMSelectorForegroundChanged;

  public event PropertyChangedCallback AMPMSelectorButtonsArrowBrushChanged;

  public event PropertyChangedCallback AMPMSelectorButtonsBackgroundChanged;

  public event PropertyChangedCallback AMPMSelectorCornerRadiusChanged;

  public event PropertyChangedCallback AMPMSelectorButtonsBorderBrushChanged;

  public event PropertyChangedCallback AMPMMouseOverButtonsBorderBrushChanged;

  public event PropertyChangedCallback AMPMMouseOverButtonsArrowBrushChanged;

  public event PropertyChangedCallback AMPMMouseOverButtonsBackgroundChanged;

  public event PropertyChangedCallback ClockPointBrushChanged;

  public event PropertyChangedCallback CenterCircleBrushChanged;

  public event PropertyChangedCallback SecondHandBrushChanged;

  public event PropertyChangedCallback SecondHandMouseOverBrushChanged;

  public event PropertyChangedCallback MinuteHandBrushChanged;

  public event PropertyChangedCallback MinuteHandBorderBrushChanged;

  public event PropertyChangedCallback MinuteHandMouseOverBrushChanged;

  public event PropertyChangedCallback MinuteHandMouseOverBorderBrushChanged;

  public event PropertyChangedCallback HourHandBrushChanged;

  public event PropertyChangedCallback HourHandBorderBrushChanged;

  public event PropertyChangedCallback HourHandMouseOverBrushChanged;

  public event PropertyChangedCallback HourHandMouseOverBorderBrushChanged;

  public event PropertyChangedCallback HourHandPressedBrushChanged;

  public event PropertyChangedCallback MinuteHandPressedBrushChanged;

  public event PropertyChangedCallback SecondHandPressedBrushChanged;

  public event PropertyChangedCallback IsInsideAmPmVisibleChanged;

  public event PropertyChangedCallback IsDigitalAmPmVisibleChanged;

  protected virtual void OnClockCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ClockCornerRadiusChanged == null)
      return;
    this.ClockCornerRadiusChanged((DependencyObject) this, e);
  }

  private static void OnClockCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnClockCornerRadiusChanged(e);
  }

  protected virtual void OnBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BorderThicknessChanged == null)
      return;
    this.BorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnBorderThicknessChanged(e);
  }

  protected virtual void OnSecondHandThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SecondHandThicknessChanged == null)
      return;
    this.SecondHandThicknessChanged((DependencyObject) this, e);
  }

  private static void OnSecondHandThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnSecondHandThicknessChanged(e);
  }

  protected virtual void OnInnerBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.InnerBorderThicknessChanged == null)
      return;
    this.InnerBorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnInnerBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnInnerBorderThicknessChanged(e);
  }

  protected virtual void OnDialBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DialBorderThicknessChanged == null)
      return;
    this.DialBorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnDialBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnDialBorderThicknessChanged(e);
  }

  protected virtual void OnAMPMSelectorPositionChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorPositionChanged == null)
      return;
    this.AMPMSelectorPositionChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorPositionChanged(e);
  }

  protected virtual void OnInnerBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.InnerBorderBrushChanged == null)
      return;
    this.InnerBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnInnerBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnInnerBorderBrushChanged(e);
  }

  protected virtual void OnDialBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DialBackgroundChanged == null)
      return;
    this.DialBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnDialBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnDialBackgroundChanged(e);
  }

  protected virtual void OnDialCenterBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.DialCenterBackgroundChanged == null)
      return;
    this.DialCenterBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnDialCenterBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnDialCenterBackgroundChanged(e);
  }

  protected virtual void OnBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BorderBrushChanged == null)
      return;
    this.BorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnBorderBrushChanged(e);
  }

  protected virtual void OnClockFrameBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ClockFrameBrushChanged == null)
      return;
    this.ClockFrameBrushChanged((DependencyObject) this, e);
  }

  private static void OnClockFrameBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnClockFrameBrushChanged(e);
  }

  protected virtual void OnFrameBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameBorderThicknessChanged == null)
      return;
    this.FrameBorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnFrameBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameBorderThicknessChanged(e);
  }

  protected virtual void OnFrameInnerBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameInnerBorderThicknessChanged == null)
      return;
    this.FrameInnerBorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnFrameInnerBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameInnerBorderThicknessChanged(e);
  }

  protected virtual void OnFrameBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameBorderBrushChanged == null)
      return;
    this.FrameBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnFrameBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameBorderBrushChanged(e);
  }

  protected virtual void OnFrameInnerBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameInnerBorderBrushChanged == null)
      return;
    this.FrameInnerBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnFrameInnerBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameInnerBorderBrushChanged(e);
  }

  protected virtual void OnFrameBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameBackgroundChanged == null)
      return;
    this.FrameBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnFrameBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameBackgroundChanged(e);
  }

  protected virtual void OnFrameCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameCornerRadiusChanged == null)
      return;
    this.FrameCornerRadiusChanged((DependencyObject) this, e);
  }

  private static void OnFrameCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnFrameCornerRadiusChanged(e);
  }

  protected virtual void OnAMPMSelectorBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorBorderThicknessChanged == null)
      return;
    this.AMPMSelectorBorderThicknessChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorBorderThicknessChanged(e);
  }

  protected virtual void OnAMPMSelectorBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorBorderBrushChanged == null)
      return;
    this.AMPMSelectorBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorBorderBrushChanged(e);
  }

  protected virtual void OnAMPMSelectorBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorBackgroundChanged == null)
      return;
    this.AMPMSelectorBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorBackgroundChanged(e);
  }

  protected virtual void OnAMPMSelectorForegroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorForegroundChanged == null)
      return;
    this.AMPMSelectorForegroundChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorForegroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorForegroundChanged(e);
  }

  protected virtual void OnAMPMSelectorButtonsArrowBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorButtonsArrowBrushChanged == null)
      return;
    this.AMPMSelectorButtonsArrowBrushChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorButtonsArrowBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorButtonsArrowBrushChanged(e);
  }

  protected virtual void OnAMPMSelectorButtonsBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorButtonsBackgroundChanged == null)
      return;
    this.AMPMSelectorButtonsBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorButtonsBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorButtonsBackgroundChanged(e);
  }

  protected virtual void OnAMPMSelectorCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorCornerRadiusChanged == null)
      return;
    this.AMPMSelectorCornerRadiusChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorCornerRadiusChanged(e);
  }

  protected virtual void OnAMPMSelectorButtonsBorderBrushChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMSelectorButtonsBorderBrushChanged == null)
      return;
    this.AMPMSelectorButtonsBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnAMPMSelectorButtonsBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMSelectorButtonsBorderBrushChanged(e);
  }

  protected virtual void OnAMPMMouseOverButtonsBorderBrushChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMMouseOverButtonsBorderBrushChanged == null)
      return;
    this.AMPMMouseOverButtonsBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnAMPMMouseOverButtonsBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMMouseOverButtonsBorderBrushChanged(e);
  }

  protected virtual void OnAMPMMouseOverButtonsArrowBrushChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMMouseOverButtonsArrowBrushChanged == null)
      return;
    this.AMPMMouseOverButtonsArrowBrushChanged((DependencyObject) this, e);
  }

  private static void OnAMPMMouseOverButtonsArrowBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMMouseOverButtonsArrowBrushChanged(e);
  }

  protected virtual void OnAMPMMouseOverButtonsBackgroundChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.AMPMMouseOverButtonsBackgroundChanged == null)
      return;
    this.AMPMMouseOverButtonsBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnAMPMMouseOverButtonsBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnAMPMMouseOverButtonsBackgroundChanged(e);
  }

  protected virtual void OnClockPointBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ClockPointBrushChanged == null)
      return;
    this.ClockPointBrushChanged((DependencyObject) this, e);
  }

  private static void OnClockPointBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnClockPointBrushChanged(e);
  }

  protected virtual void OnCenterCircleBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.CenterCircleBrushChanged == null)
      return;
    this.CenterCircleBrushChanged((DependencyObject) this, e);
  }

  private static void OnCenterCircleBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnCenterCircleBrushChanged(e);
  }

  protected virtual void OnSecondHandBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SecondHandBrushChanged == null)
      return;
    this.SecondHandBrushChanged((DependencyObject) this, e);
  }

  private static void OnSecondHandBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnSecondHandBrushChanged(e);
  }

  protected virtual void OnSecondHandMouseOverBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SecondHandMouseOverBrushChanged == null)
      return;
    this.SecondHandMouseOverBrushChanged((DependencyObject) this, e);
  }

  private static void OnSecondHandMouseOverBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnSecondHandMouseOverBrushChanged(e);
  }

  protected virtual void OnMinuteHandBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinuteHandBrushChanged == null)
      return;
    this.MinuteHandBrushChanged((DependencyObject) this, e);
  }

  private static void OnMinuteHandBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnMinuteHandBrushChanged(e);
  }

  protected virtual void OnMinuteHandBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinuteHandBorderBrushChanged == null)
      return;
    this.MinuteHandBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnMinuteHandBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnMinuteHandBorderBrushChanged(e);
  }

  protected virtual void OnMinuteHandMouseOverBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinuteHandMouseOverBrushChanged == null)
      return;
    this.MinuteHandMouseOverBrushChanged((DependencyObject) this, e);
  }

  private static void OnMinuteHandMouseOverBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnMinuteHandMouseOverBrushChanged(e);
  }

  protected virtual void OnMinuteHandMouseOverBorderBrushChanged(
    DependencyPropertyChangedEventArgs e)
  {
    if (this.MinuteHandMouseOverBorderBrushChanged == null)
      return;
    this.MinuteHandMouseOverBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnMinuteHandMouseOverBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnMinuteHandMouseOverBorderBrushChanged(e);
  }

  protected virtual void OnHourHandBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HourHandBrushChanged == null)
      return;
    this.HourHandBrushChanged((DependencyObject) this, e);
  }

  private static void OnHourHandBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnHourHandBrushChanged(e);
  }

  protected virtual void OnHourHandBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HourHandBorderBrushChanged == null)
      return;
    this.HourHandBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnHourHandBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnHourHandBorderBrushChanged(e);
  }

  protected virtual void OnHourHandMouseOverBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HourHandMouseOverBrushChanged == null)
      return;
    this.HourHandMouseOverBrushChanged((DependencyObject) this, e);
  }

  private static void OnHourHandMouseOverBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnHourHandMouseOverBrushChanged(e);
  }

  protected virtual void OnHourHandMouseOverBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HourHandMouseOverBorderBrushChanged == null)
      return;
    this.HourHandMouseOverBorderBrushChanged((DependencyObject) this, e);
  }

  private static void OnHourHandMouseOverBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnHourHandMouseOverBorderBrushChanged(e);
  }

  protected virtual void OnHourHandPressedBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HourHandPressedBrushChanged == null)
      return;
    this.HourHandPressedBrushChanged((DependencyObject) this, e);
  }

  private static void OnHourHandPressedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnHourHandPressedBrushChanged(e);
  }

  protected virtual void OnMinuteHandPressedBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.MinuteHandPressedBrushChanged == null)
      return;
    this.MinuteHandPressedBrushChanged((DependencyObject) this, e);
  }

  private static void OnMinuteHandPressedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnMinuteHandPressedBrushChanged(e);
  }

  protected virtual void OnSecondHandPressedBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SecondHandPressedBrushChanged == null)
      return;
    this.SecondHandPressedBrushChanged((DependencyObject) this, e);
  }

  private static void OnSecondHandPressedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnSecondHandPressedBrushChanged(e);
  }

  protected virtual void OnIsInsideAmPmVisibleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsInsideAmPmVisibleChanged == null)
      return;
    this.IsInsideAmPmVisibleChanged((DependencyObject) this, e);
  }

  private static void OnIsInsideAmPmVisibleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnIsInsideAmPmVisibleChanged(e);
  }

  protected virtual void OnIsDigitalAmPmVisibleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsDigitalAmPmVisibleChanged == null)
      return;
    this.IsDigitalAmPmVisibleChanged((DependencyObject) this, e);
  }

  private static void OnIsDigitalAmPmVisibleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Clock) d).OnIsDigitalAmPmVisibleChanged(e);
  }

  public enum Position
  {
    Top,
    Bottom,
    Right,
    Left,
  }
}
