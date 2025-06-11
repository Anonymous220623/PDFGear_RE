// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ChromelessWindow
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2013Style.xaml")]
[DesignTimeVisible(false)]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ChromelessWindow), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ChromelessWindow/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ChromelessWindow), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ChromelessWindow/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ChromelessWindow), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/TransparentStyle.xaml")]
public class ChromelessWindow : Window
{
  private const int HORZRES = 8;
  private const int VERTRES = 10;
  private const int LOGPIXELSX = 88;
  private const int LOGPIXELSY = 90;
  private bool dragIsIn;
  private bool m_dwmEnabled;
  private bool m_isSizing;
  private bool m_isInitialized;
  private TitleButton restButton;
  private TitleButton closeButton;
  internal bool updateNew;
  private TitleButton maxButton;
  private Border contentArea;
  private StackPanel panel;
  private TitleButton minButton;
  private Point m_lastTitlebarPoint;
  private DateTime m_lastIconClick;
  private Point m_lastIconPoint;
  private Image m_icon;
  private Image m_iconLeft;
  private IntPtr window_hwnd;
  private Label ResizeGrip;
  private Border resizeborder;
  private WindowState lastMenuState;
  private bool suspendLocationUpdate;
  private static double m_lastTitlebarPointXTemp;
  private bool flag;
  private bool iscustommaxhghtused;
  private bool iscustommaxwidthused;
  public static RoutedUICommand CloseWindow = new RoutedUICommand("Close", nameof (CloseWindow), typeof (ChromelessWindow));
  public static RoutedUICommand ToggleMaximizedState = new RoutedUICommand("Maximize", nameof (ToggleMaximizedState), typeof (ChromelessWindow));
  public static RoutedUICommand ToggleMinimizedState = new RoutedUICommand("Minimize", nameof (ToggleMinimizedState), typeof (ChromelessWindow));
  public static readonly DependencyProperty TitleTextAlignmentProperty = DependencyProperty.Register(nameof (TitleTextAlignment), typeof (HorizontalAlignment), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Left));
  public static readonly DependencyProperty IconAlignmentProperty = DependencyProperty.Register(nameof (IconAlignment), typeof (IconAlignment), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) IconAlignment.Left));
  public static readonly DependencyProperty ResizeGripStyleProperty = DependencyProperty.Register(nameof (ResizeGripStyle), typeof (Style), typeof (ChromelessWindow), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(nameof (ShowMaximizeButton), typeof (bool), typeof (ChromelessWindow), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(nameof (ShowMinimizeButton), typeof (bool), typeof (ChromelessWindow), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty UseNativeChromeProperty = DependencyProperty.Register(nameof (UseNativeChrome), typeof (bool), typeof (ChromelessWindow), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(ChromelessWindow.OnUseNativeChromeChanged)));
  internal static readonly DependencyProperty IsGlassActiveProperty = DependencyProperty.Register(nameof (IsGlassActive), typeof (bool), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ChromelessWindow.OnIsGlassActiveChanged)));
  public static readonly DependencyProperty TitleBarTemplateProperty = DependencyProperty.Register(nameof (TitleBarTemplate), typeof (ControlTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MaximizeButtonTemplateProperty = DependencyProperty.Register(nameof (MaximizeButtonTemplate), typeof (ControlTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinimizeButtonTemplateProperty = DependencyProperty.Register(nameof (MinimizeButtonTemplate), typeof (ControlTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RestoreButtonTemplateProperty = DependencyProperty.Register(nameof (RestoreButtonTemplate), typeof (ControlTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CloseButtonTemplateProperty = DependencyProperty.Register(nameof (CloseButtonTemplate), typeof (ControlTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LeftHeaderItemsSourceProperty = DependencyProperty.Register(nameof (LeftHeaderItemsSource), typeof (IEnumerable), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RightHeaderItemsSourceProperty = DependencyProperty.Register(nameof (RightHeaderItemsSource), typeof (IEnumerable), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LeftHeaderItemTemplateProperty = DependencyProperty.Register(nameof (LeftHeaderItemTemplate), typeof (DataTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RightHeaderItemTemplateProperty = DependencyProperty.Register(nameof (RightHeaderItemTemplate), typeof (DataTemplate), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ResizeBorderBrushProperty = DependencyProperty.Register(nameof (ResizeBorderBrush), typeof (Brush), typeof (ChromelessWindow));
  public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(nameof (TitleBarBackground), typeof (Brush), typeof (ChromelessWindow));
  public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(nameof (TitleBarHeight), typeof (double), typeof (ChromelessWindow), new PropertyMetadata((object) 30.0));
  public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(nameof (TitleFontSize), typeof (double), typeof (ChromelessWindow), new PropertyMetadata((object) 12.0));
  public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(nameof (TitleBarForeground), typeof (Brush), typeof (ChromelessWindow));
  public static readonly DependencyProperty NavigationBarBackgroundProperty = DependencyProperty.Register(nameof (NavigationBarBackground), typeof (Brush), typeof (ChromelessWindow));
  public static readonly DependencyProperty WindowContentAreaBorderBrushProperty = DependencyProperty.Register(nameof (WindowContentAreaBorderBrush), typeof (Brush), typeof (ChromelessWindow));
  public static readonly DependencyProperty ResizeBorderThicknessProperty;
  public static readonly DependencyProperty WindowCornerRadiusProperty;
  public static readonly DependencyProperty CornerRadiusProperty;
  public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(nameof (ShowIcon), typeof (bool), typeof (ChromelessWindow), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof (ShowTitle), typeof (bool), typeof (ChromelessWindow), new PropertyMetadata((object) true));
  public static readonly DependencyProperty HideTaskBarProperty = DependencyProperty.Register(nameof (HideTaskBar), typeof (bool), typeof (ChromelessWindow), (PropertyMetadata) new UIPropertyMetadata((object) false));
  private static readonly DependencyProperty WindowSizePropertyDescriptor = DependencyProperty.RegisterAttached("WindowSizeDescriptor", typeof (DependencyPropertyDescriptor), typeof (ChromelessWindow));

  private void MaximizeWindowExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    this.SystemButtonsUpdate();
  }

  private void MinimizeWindowExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    this.WindowState = this.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
  }

  private void CloseWindowExecuted(object sender, ExecutedRoutedEventArgs e) => this.Close();

  internal bool IsGlassActive
  {
    get => (bool) this.GetValue(ChromelessWindow.IsGlassActiveProperty);
    set => this.SetValue(ChromelessWindow.IsGlassActiveProperty, (object) value);
  }

  private bool CanEnableGlass => this.m_dwmEnabled && this.IsGlassActive;

  private double DPIOffset
  {
    get
    {
      double dpiOffset = 1.0;
      Point transformedPoint = ChromelessWindowInterop.GetTransformedPoint((Visual) this);
      if (transformedPoint.Y != 96.0)
        dpiOffset += (transformedPoint.Y - 96.0) / 96.0;
      return dpiOffset;
    }
  }

  public Brush ResizeBorderBrush
  {
    get => (Brush) this.GetValue(ChromelessWindow.ResizeBorderBrushProperty);
    set => this.SetValue(ChromelessWindow.ResizeBorderBrushProperty, (object) value);
  }

  public Brush TitleBarBackground
  {
    get => (Brush) this.GetValue(ChromelessWindow.TitleBarBackgroundProperty);
    set => this.SetValue(ChromelessWindow.TitleBarBackgroundProperty, (object) value);
  }

  public double TitleBarHeight
  {
    get => (double) this.GetValue(ChromelessWindow.TitleBarHeightProperty);
    set => this.SetValue(ChromelessWindow.TitleBarHeightProperty, (object) value);
  }

  public double TitleFontSize
  {
    get => (double) this.GetValue(ChromelessWindow.TitleFontSizeProperty);
    set => this.SetValue(ChromelessWindow.TitleFontSizeProperty, (object) value);
  }

  public Brush TitleBarForeground
  {
    get => (Brush) this.GetValue(ChromelessWindow.TitleBarForegroundProperty);
    set => this.SetValue(ChromelessWindow.TitleBarForegroundProperty, (object) value);
  }

  public LinearGradientBrush NavigationBarBackground
  {
    get => (LinearGradientBrush) this.GetValue(ChromelessWindow.NavigationBarBackgroundProperty);
    set => this.SetValue(ChromelessWindow.NavigationBarBackgroundProperty, (object) value);
  }

  public Brush WindowContentAreaBorderBrush
  {
    get => (Brush) this.GetValue(ChromelessWindow.WindowContentAreaBorderBrushProperty);
    set => this.SetValue(ChromelessWindow.WindowContentAreaBorderBrushProperty, (object) value);
  }

  public Thickness ResizeBorderThickness
  {
    get => (Thickness) this.GetValue(ChromelessWindow.ResizeBorderThicknessProperty);
    set => this.SetValue(ChromelessWindow.ResizeBorderThicknessProperty, (object) value);
  }

  public ControlTemplate TitleBarTemplate
  {
    get => (ControlTemplate) this.GetValue(ChromelessWindow.TitleBarTemplateProperty);
    set => this.SetValue(ChromelessWindow.TitleBarTemplateProperty, (object) value);
  }

  public ControlTemplate MaximizeButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(ChromelessWindow.MaximizeButtonTemplateProperty);
    set => this.SetValue(ChromelessWindow.MaximizeButtonTemplateProperty, (object) value);
  }

  public ControlTemplate MinimizeButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(ChromelessWindow.MinimizeButtonTemplateProperty);
    set => this.SetValue(ChromelessWindow.MinimizeButtonTemplateProperty, (object) value);
  }

  public ControlTemplate RestoreButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(ChromelessWindow.RestoreButtonTemplateProperty);
    set => this.SetValue(ChromelessWindow.RestoreButtonTemplateProperty, (object) value);
  }

  public ControlTemplate CloseButtonTemplate
  {
    get => (ControlTemplate) this.GetValue(ChromelessWindow.CloseButtonTemplateProperty);
    set => this.SetValue(ChromelessWindow.CloseButtonTemplateProperty, (object) value);
  }

  public TitleBar TitleBar => (TitleBar) this.GetTemplateChild("PART_TitleBar");

  public HorizontalAlignment TitleTextAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ChromelessWindow.TitleTextAlignmentProperty);
    set => this.SetValue(ChromelessWindow.TitleTextAlignmentProperty, (object) value);
  }

  public IconAlignment IconAlignment
  {
    get => (IconAlignment) this.GetValue(ChromelessWindow.IconAlignmentProperty);
    set => this.SetValue(ChromelessWindow.IconAlignmentProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(ChromelessWindow.CornerRadiusProperty);
    set => this.SetValue(ChromelessWindow.CornerRadiusProperty, (object) value);
  }

  internal CornerRadius cornerRadius { get; set; }

  public bool ShowIcon
  {
    get => (bool) this.GetValue(ChromelessWindow.ShowIconProperty);
    set => this.SetValue(ChromelessWindow.ShowIconProperty, (object) value);
  }

  public bool ShowTitle
  {
    get => (bool) this.GetValue(ChromelessWindow.ShowTitleProperty);
    set => this.SetValue(ChromelessWindow.ShowTitleProperty, (object) value);
  }

  public IEnumerable LeftHeaderItemsSource
  {
    get => (IEnumerable) this.GetValue(ChromelessWindow.LeftHeaderItemsSourceProperty);
    set => this.SetValue(ChromelessWindow.LeftHeaderItemsSourceProperty, (object) value);
  }

  public IEnumerable RightHeaderItemsSource
  {
    get => (IEnumerable) this.GetValue(ChromelessWindow.RightHeaderItemsSourceProperty);
    set => this.SetValue(ChromelessWindow.RightHeaderItemsSourceProperty, (object) value);
  }

  public DataTemplate LeftHeaderItemTemplate
  {
    get => (DataTemplate) this.GetValue(ChromelessWindow.LeftHeaderItemTemplateProperty);
    set => this.SetValue(ChromelessWindow.LeftHeaderItemTemplateProperty, (object) value);
  }

  public DataTemplate RightHeaderItemTemplate
  {
    get => (DataTemplate) this.GetValue(ChromelessWindow.RightHeaderItemTemplateProperty);
    set => this.SetValue(ChromelessWindow.RightHeaderItemTemplateProperty, (object) value);
  }

  public Style ResizeGripStyle
  {
    get => (Style) this.GetValue(ChromelessWindow.ResizeGripStyleProperty);
    set => this.SetValue(ChromelessWindow.ResizeGripStyleProperty, (object) value);
  }

  public bool UseNativeChrome
  {
    get => (bool) this.GetValue(ChromelessWindow.UseNativeChromeProperty);
    set => this.SetValue(ChromelessWindow.UseNativeChromeProperty, (object) value);
  }

  public bool ShowMaximizeButton
  {
    get => (bool) this.GetValue(ChromelessWindow.ShowMaximizeButtonProperty);
    set => this.SetValue(ChromelessWindow.ShowMaximizeButtonProperty, (object) value);
  }

  public bool ShowMinimizeButton
  {
    get => (bool) this.GetValue(ChromelessWindow.ShowMinimizeButtonProperty);
    set => this.SetValue(ChromelessWindow.ShowMinimizeButtonProperty, (object) value);
  }

  public bool HideTaskBar
  {
    get => (bool) this.GetValue(ChromelessWindow.HideTaskBarProperty);
    set => this.SetValue(ChromelessWindow.HideTaskBarProperty, (object) value);
  }

  private static void OnIsGlassActiveChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ChromelessWindow) d).OnIsGlassActiveChanged(e);
  }

  private void OnWindowSizeDpChanged()
  {
    if (BindingOperations.IsDataBound((DependencyObject) this, Window.ResizeModeProperty))
      return;
    DependencyPropertyDescriptor propertyDescriptor = (DependencyPropertyDescriptor) this.GetValue(ChromelessWindow.WindowSizePropertyDescriptor);
    if (propertyDescriptor == null)
      return;
    propertyDescriptor.RemoveValueChanged((object) this, new EventHandler(this.ChromelessWindow_StateChanged));
    this.SetValue(ChromelessWindow.WindowSizePropertyDescriptor, (object) null);
  }

  protected virtual void OnIsGlassActiveChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsGlassActiveChanged == null)
      return;
    this.IsGlassActiveChanged((DependencyObject) this, e);
  }

  private Syncfusion.Windows.WindowChrome GetDefaultNativeChrome()
  {
    Syncfusion.Windows.WindowChrome target = new Syncfusion.Windows.WindowChrome();
    target.UseAeroCaptionButtons = false;
    target.GlassFrameThickness = new Thickness(0.0);
    BindingOperations.SetBinding((DependencyObject) target, ChromelessWindow.CornerRadiusProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("CornerRadius", new object[0]),
      Source = (object) this
    });
    BindingOperations.SetBinding((DependencyObject) target, ChromelessWindow.ResizeBorderThicknessProperty, (BindingBase) new Binding()
    {
      Path = new PropertyPath("ResizeBorderThickness", new object[0]),
      Source = (object) this
    });
    return target;
  }

  private static void OnUseNativeChromeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChromelessWindow chromelessWindow = (ChromelessWindow) d;
    if (!chromelessWindow.UseNativeChrome)
      return;
    Syncfusion.Windows.WindowChrome.SetWindowChrome((Window) chromelessWindow, chromelessWindow.GetDefaultNativeChrome());
  }

  public event PropertyChangedCallback IsGlassActiveChanged;

  private static void OnResizePropertyThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  static ChromelessWindow()
  {
    FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
    KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
    KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
    KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ChromelessWindow)));
    ChromelessWindow.ResizeBorderThicknessProperty = DependencyProperty.Register(nameof (ResizeBorderThickness), typeof (Thickness), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(SystemParameters.ResizeFrameVerticalBorderWidth, SystemParameters.ResizeFrameHorizontalBorderHeight, SystemParameters.ResizeFrameVerticalBorderWidth, SystemParameters.ResizeFrameHorizontalBorderHeight), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(ChromelessWindow.OnResizePropertyThicknessChanged)));
    ChromelessWindow.CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (ChromelessWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(0.0)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ChromelessWindow()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ChromelessWindow.CloseWindow, new ExecutedRoutedEventHandler(this.CloseWindowExecuted)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ChromelessWindow.ToggleMinimizedState, new ExecutedRoutedEventHandler(this.MinimizeWindowExecuted)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ChromelessWindow.ToggleMaximizedState, new ExecutedRoutedEventHandler(this.MaximizeWindowExecuted)));
    this.Icon = (ImageSource) BitmapFrame.Create(new Uri("pack://application:,,,/Syncfusion.Shared.WPF;component/Controls/ChromelessWindow/Resources/app.png", UriKind.RelativeOrAbsolute));
    this.StateChanged += new EventHandler(this.ChromelessWindow_StateChanged);
    this.Loaded += new RoutedEventHandler(this.ChromelessWindow_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ChromelessWindow_Unloaded);
    this.WindowStyle = WindowStyle.None;
    this.SetValue(RenderOptions.ClearTypeHintProperty, (object) ClearTypeHint.Enabled);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void ChromelessWindow_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.m_icon != null)
    {
      this.m_icon.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Imgicon_MouseLeftButtonUp);
      this.m_icon.MouseDown -= new MouseButtonEventHandler(this.OnIconMouseDown);
    }
    if (this.m_iconLeft != null)
    {
      this.m_iconLeft.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Imgicon_MouseLeftButtonUp);
      this.m_iconLeft.MouseDown -= new MouseButtonEventHandler(this.OnIconMouseDown);
    }
    this.Unloaded -= new RoutedEventHandler(this.ChromelessWindow_Unloaded);
    this.OnWindowSizeDpChanged();
  }

  private void ChromelessWindow_Loaded(object sender, RoutedEventArgs e)
  {
    DependencyPropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(Window.ResizeModeProperty, this.GetType());
    propertyDescriptor.AddValueChanged((object) this, new EventHandler(this.ChromelessWindow_StateChanged));
    this.SetValue(ChromelessWindow.WindowSizePropertyDescriptor, (object) propertyDescriptor);
    this.SystemButtonsUpdate();
    if (!this.UseNativeChrome)
    {
      this.IsGlassActive = false;
      this.window_hwnd = new WindowInteropHelper((Window) this).Handle;
      this.m_dwmEnabled = ChromelessWindowInterop.CanEnableDwm();
      if (!this.m_dwmEnabled)
        this.IsGlassActive = false;
    }
    this.CheckVisualStyle();
  }

  private void ChromelessWindow_StateChanged(object sender, EventArgs e)
  {
    this.SystemButtonsUpdate();
    this.UpdateGlassChange();
  }

  protected internal void SystemButtonsUpdate()
  {
    if (this.resizeborder != null && (this.ResizeMode == ResizeMode.CanResizeWithGrip || this.ResizeMode == ResizeMode.CanResize))
    {
      Binding binding = new Binding()
      {
        Path = new PropertyPath("ResizeBorderThickness", new object[0]),
        Mode = BindingMode.TwoWay,
        Source = (object) this
      };
      this.resizeborder.SetBinding(Border.BorderThicknessProperty, (BindingBase) binding);
    }
    else if (this.resizeborder != null && this.ResizeMode != ResizeMode.NoResize)
      this.resizeborder.BorderThickness = !(this.ResizeBorderThickness == new Thickness(0.0)) ? new Thickness(2.0) : new Thickness(0.0);
    if (this.ResizeMode != ResizeMode.NoResize || this.resizeborder == null)
      return;
    this.resizeborder.BorderThickness = new Thickness(1.0);
  }

  private void OnIconMouseDown(object sender, MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (!e.Handled && e.ChangedButton == MouseButton.Left && this.IsMouseOver)
    {
      Point position = e.GetPosition((IInputElement) this);
      if (DateTime.Now.Subtract(this.m_lastIconClick).TotalMilliseconds < 500.0 && Math.Abs(this.m_lastIconPoint.X - position.X) <= 2.0 && Math.Abs(this.m_lastIconPoint.Y - position.Y) <= 2.0)
        this.Close();
      else
        this.m_lastIconPoint = e.GetPosition((IInputElement) this);
      this.m_lastIconClick = DateTime.Now;
    }
    e.Handled = true;
  }

  private void Imgicon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!e.Handled && e.ChangedButton == MouseButton.Left && this.IsMouseOver)
    {
      this.dragIsIn = false;
      e.Handled = true;
      Point screen = this.contentArea.PointToScreen(new Point(0.0, 0.0));
      screen.X += 2.0;
      this._UpdateSystemMenu(new WindowState?(this.WindowState));
      ChromelessWindowInterop.ShowSystemMenu(this.window_hwnd, screen);
    }
    e.Handled = true;
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    if (this.TitleBar == null || e.Handled || e.ChangedButton != MouseButton.Right || !this.TitleBar.IsMouseOver)
      return;
    e.Handled = true;
    IntPtr handle = new WindowInteropHelper((Window) this).Handle;
    Point screen = this.PointToScreen(e.GetPosition((IInputElement) this));
    this._UpdateSystemMenu(new WindowState?(this.WindowState));
    ChromelessWindowInterop.ShowSystemMenu(handle, screen);
  }

  private void _UpdateSystemMenu(WindowState? assumeState)
  {
    WindowState windowState = (WindowState) ((int) assumeState ?? (int) this._GetHwndState());
    if (!assumeState.HasValue && this.lastMenuState == windowState)
      return;
    this.lastMenuState = windowState;
    bool flag1 = this._ModifyStyle(WindowStyleValues.VISIBLE, WindowStyleValues.OVERLAPPED);
    IntPtr systemMenu = NativeMethods.GetSystemMenu(this.window_hwnd, false);
    if (IntPtr.Zero != systemMenu)
    {
      WindowStyleValues windowStyleValues = IntPtr.Size == 4 ? (WindowStyleValues) NativeMethods.GetWindowLongPtr(this.window_hwnd, GWL.STYLE).ToInt32() : (WindowStyleValues) NativeMethods.GetWindowLongPtr(this.window_hwnd, GWL.STYLE).ToInt64();
      bool flag2 = this.IsFlagSet((int) windowStyleValues, 131072 /*0x020000*/);
      bool flag3 = this.IsFlagSet((int) windowStyleValues, 65536 /*0x010000*/);
      bool flag4 = this.IsFlagSet((int) windowStyleValues, 262144 /*0x040000*/);
      switch (windowState)
      {
        case WindowState.Minimized:
          int num1 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.RESTORE, SystemMenuItemBehavior.ENABLED);
          int num2 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MOVE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num3 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.SIZE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num4 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MINIMIZE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num5 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MAXIMIZE, flag3 ? SystemMenuItemBehavior.ENABLED : SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          break;
        case WindowState.Maximized:
          if (this.ResizeMode == ResizeMode.NoResize)
          {
            int num6 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.RESTORE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
            int num7 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MOVE, SystemMenuItemBehavior.ENABLED);
          }
          else
          {
            int num8 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.RESTORE, SystemMenuItemBehavior.ENABLED);
            int num9 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MOVE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          }
          int num10 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.SIZE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num11 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MINIMIZE, flag2 ? SystemMenuItemBehavior.ENABLED : SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num12 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MAXIMIZE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          break;
        default:
          int num13 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.RESTORE, SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num14 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MOVE, SystemMenuItemBehavior.ENABLED);
          int num15 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.SIZE, flag4 ? SystemMenuItemBehavior.ENABLED : SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num16 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MINIMIZE, flag2 ? SystemMenuItemBehavior.ENABLED : SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          int num17 = (int) NativeMethods.EnableMenuItem(systemMenu, SystemCommands.MAXIMIZE, flag3 ? SystemMenuItemBehavior.ENABLED : SystemMenuItemBehavior.GRAYED | SystemMenuItemBehavior.DISABLED);
          break;
      }
    }
    if (!flag1)
      return;
    this._ModifyStyle(WindowStyleValues.OVERLAPPED, WindowStyleValues.VISIBLE);
  }

  internal bool IsFlagSet(int value, int mask) => 0 != (value & mask);

  private WindowState _GetHwndState()
  {
    switch (NativeMethods.GetWindowPlacement(new WindowInteropHelper((Window) this).Handle).showCmd)
    {
      case ShowWindowOptions.SHOWMINIMIZED:
        return WindowState.Minimized;
      case ShowWindowOptions.SHOWMAXIMIZED:
        return WindowState.Maximized;
      default:
        return WindowState.Normal;
    }
  }

  private bool _ModifyStyle(WindowStyleValues removeStyle, WindowStyleValues addStyle)
  {
    WindowStyleValues windowStyleValues1 = IntPtr.Size == 4 ? (WindowStyleValues) NativeMethods.GetWindowLongPtr(this.window_hwnd, GWL.STYLE).ToInt32() : (WindowStyleValues) NativeMethods.GetWindowLongPtr(this.window_hwnd, GWL.STYLE).ToInt64();
    WindowStyleValues windowStyleValues2 = windowStyleValues1 & ~removeStyle | addStyle;
    if (windowStyleValues1 == windowStyleValues2)
      return false;
    NativeMethods.SetWindowLongPtr(this.window_hwnd, GWL.STYLE, new IntPtr((int) windowStyleValues2));
    return true;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    this.dragIsIn = false;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (this.TitleBar == null || this.UseNativeChrome || e.Handled || e.ChangedButton != MouseButton.Left || !this.TitleBar.IsMouseOver)
      return;
    Point position = e.GetPosition((IInputElement) this);
    if (e.ClickCount == 2 && Math.Abs(this.m_lastTitlebarPoint.X - position.X) <= 2.0 && Math.Abs(this.m_lastTitlebarPoint.Y - position.Y) <= 2.0)
    {
      if (this.ResizeMode != ResizeMode.NoResize && this.ResizeMode != ResizeMode.CanMinimize)
      {
        if (this.WindowState != WindowState.Maximized)
        {
          this.WindowState = WindowState.Maximized;
        }
        else
        {
          this.WindowState = WindowState.Normal;
          --this.Width;
          ++this.Width;
        }
      }
      else
      {
        if (this.WindowState != WindowState.Maximized || this.WindowState == WindowState.Normal)
          return;
        this.WindowState = WindowState.Normal;
      }
    }
    else
    {
      this.m_lastTitlebarPoint = e.GetPosition((IInputElement) this);
      this.DragMove();
      if (this.WindowState != WindowState.Maximized)
        return;
      this.dragIsIn = true;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.resizeborder = this.GetTemplateChild("OuterBorder") as Border;
    this.maxButton = (TitleButton) this.Template.FindName("PART_MaximizeButton", (FrameworkElement) this);
    this.restButton = (TitleButton) this.Template.FindName("PART_RestoreButton", (FrameworkElement) this);
    this.closeButton = (TitleButton) this.Template.FindName("CloseButton", (FrameworkElement) this);
    this.ResizeGrip = this.GetTemplateChild("PART_Resizegrip") as Label;
    this.m_icon = (Image) this.Template.FindName("PART_Icon", (FrameworkElement) this);
    this.m_iconLeft = (Image) this.Template.FindName("PART_IconLeft", (FrameworkElement) this);
    this.contentArea = this.GetTemplateChild("ContentAreaBorder") as Border;
    this.panel = this.GetTemplateChild("MinMaxCloseStackPanel") as StackPanel;
    if (SfSkinManagerExtension.GetThemeDesign((DependencyObject) this) != null && this.panel != null && this.CornerRadius.TopLeft > 0.0 && this.CornerRadius.TopRight > 0.0 && this.CornerRadius.BottomLeft > 0.0 && this.CornerRadius.BottomRight > 0.0)
    {
      Thickness margin = this.panel.Margin;
      margin.Right += this.CornerRadius.TopRight / 2.0;
      this.panel.Margin = margin;
    }
    if (this.contentArea != null && this.contentArea.BorderThickness == new Thickness(0.0))
      this.contentArea.BorderThickness = new Thickness(2.0, 0.0, 2.0, 2.0);
    if (this.m_icon != null)
    {
      this.m_icon.MouseLeftButtonUp += new MouseButtonEventHandler(this.Imgicon_MouseLeftButtonUp);
      this.m_icon.MouseDown += new MouseButtonEventHandler(this.OnIconMouseDown);
    }
    if (this.m_iconLeft != null)
    {
      this.m_iconLeft.MouseLeftButtonUp += new MouseButtonEventHandler(this.Imgicon_MouseLeftButtonUp);
      this.m_iconLeft.MouseDown += new MouseButtonEventHandler(this.OnIconMouseDown);
    }
    if (this.UseNativeChrome)
    {
      Syncfusion.Windows.WindowChrome chrome = Syncfusion.Windows.WindowChrome.GetWindowChrome((Window) this) ?? this.GetDefaultNativeChrome();
      if (SfSkinManagerExtension.GetThemeDesign((DependencyObject) this) == "Fluent")
      {
        System.Windows.Shell.WindowChrome windowChrome = new System.Windows.Shell.WindowChrome();
        windowChrome.GlassFrameThickness = new Thickness(1.0);
        windowChrome.UseAeroCaptionButtons = false;
        BindingOperations.SetBinding((DependencyObject) windowChrome, ChromelessWindow.CornerRadiusProperty, (BindingBase) new Binding()
        {
          Path = new PropertyPath("CornerRadius", new object[0]),
          Source = (object) this
        });
        BindingOperations.SetBinding((DependencyObject) windowChrome, ChromelessWindow.ResizeBorderThicknessProperty, (BindingBase) new Binding()
        {
          Path = new PropertyPath("ResizeBorderThickness", new object[0]),
          Source = (object) this
        });
        System.Windows.Shell.WindowChrome.SetWindowChrome((Window) this, windowChrome);
      }
      else
        Syncfusion.Windows.WindowChrome.SetWindowChrome((Window) this, chrome);
      this.HandleSizeToContent();
    }
    if (this.MaxHeight != double.PositiveInfinity)
      this.iscustommaxhghtused = true;
    if (this.MaxWidth == double.PositiveInfinity)
      return;
    this.iscustommaxwidthused = true;
  }

  private void CheckVisualStyle()
  {
    if (!(SkinStorage.GetVisualStyle((DependencyObject) this) != "Default") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2007Blue") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2007Black") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2007Silver") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2003") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Blend") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Luna.NormalColor") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Luna.Homestead") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Luna.Metallic") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Aero.NormalColor") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Royale.NormalColor") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Zune.NormalColor") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "WMPClassic") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "ForestGreen") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "CoolBlue") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "LawnGreen") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "OrangeRed") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "SyncOrange") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "ChocolateYellow") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "SpringGreen") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "BrightGray") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "BlueWave") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "ShinyRed") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "ShinyBlue") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "VS2010") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2010Blue") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2010Black") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2010Silver") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Metro") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Transparent") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "MixedGreen") || !(SkinStorage.GetVisualStyle((DependencyObject) this) != "Office2013"))
      return;
    SkinStorage.SetVisualStyle((DependencyObject) this, "Default");
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    this.window_hwnd = new WindowInteropHelper((Window) this).Handle;
    if (!(this.window_hwnd != IntPtr.Zero))
      return;
    this.m_isInitialized = true;
    HwndSource.FromHwnd(this.window_hwnd).AddHook(new HwndSourceHook(this.HookMethod));
    this.UpdateGlassChange();
    if (this.resizeborder != null && (this.ResizeMode == ResizeMode.CanResizeWithGrip || this.ResizeMode == ResizeMode.CanResize))
    {
      Binding binding = new Binding()
      {
        Path = new PropertyPath("ResizeBorderThickness", new object[0]),
        Mode = BindingMode.TwoWay,
        Source = (object) this
      };
      this.resizeborder.SetBinding(Border.BorderThicknessProperty, (BindingBase) binding);
    }
    else
    {
      if (this.resizeborder == null || this.ResizeMode == ResizeMode.NoResize)
        return;
      if (this.ResizeBorderThickness == new Thickness(0.0))
        this.resizeborder.BorderThickness = new Thickness(0.0);
      else
        this.resizeborder.BorderThickness = new Thickness(2.0);
    }
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.suspendLocationUpdate)
      return;
    if (this.WindowStyle != WindowStyle.None && this.AllowsTransparency)
    {
      if (!this.UseNativeChrome)
      {
        try
        {
          this.WindowStyle = WindowStyle.None;
        }
        catch (Exception ex)
        {
          throw new Exception("Window Style.None is the only valid value when Allowstransparency is set to true");
        }
      }
    }
    base.OnPropertyChanged(e);
    if (e.Property != SkinStorage.VisualStyleProperty)
      return;
    ResourceDictionary resourceDictionary = new ResourceDictionary();
    if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Blend")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/BlendStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "BlendChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2007Blue")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007BlueStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2007BlueChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2007Black")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007BlackStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2007BlackChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2007Silver")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2007SilverStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2007SilverChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2003")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2003Style.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2003ChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "ShinyRed")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/ShinyRedStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "ShinyRedChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "ShinyBlue")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/ShinyBlueStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "ShinyBlueChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "SyncOrange")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/SyncOrangeStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "SyncOrangeChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "VS2010")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/VS2010Style.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "VS2010ChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2010Blue")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010BlueStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2010BlueChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2010Black")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010BlackStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2010BlackChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2010Silver")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2010SilverStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2010SilverChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Metro")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf;component/Controls/ChromelessWindow/Themes/MetroStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "MetroChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Transparent")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/TransparentStyle.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "TransparentChromelessWindowStyle"] as Style;
    }
    else if (SkinStorage.GetVisualStyle((DependencyObject) this) == "Office2013")
    {
      resourceDictionary.Source = new Uri("pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ChromelessWindow/Themes/Office2013Style.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "Office2013ChromelessWindowStyle"] as Style;
    }
    else
    {
      resourceDictionary.Source = new Uri("/Syncfusion.Shared.WPF;component/Controls/ChromelessWindow/Themes/Generic.xaml", UriKind.RelativeOrAbsolute);
      this.Style = resourceDictionary[(object) "DefaultChromelessWindowStyle"] as Style;
    }
  }

  protected override Size ArrangeOverride(Size arrangeBounds)
  {
    if (!this.UseNativeChrome)
    {
      if (this.CanEnableGlass)
      {
        double num1 = this.DPIOffset;
        int num2 = 0;
        if (num1 >= 1.0 && num1 < 1.3)
          num2 = 2;
        else if (num1 >= 1.0)
          num2 = 4;
        if (num1 >= 1.0)
          num1 = 1.0 - Math.Abs(num1 - 1.0);
        arrangeBounds.Height += Math.Ceiling(28.0 * num1) + (double) num2;
      }
      if (this.WindowState == WindowState.Maximized && this.updateNew || this.SizeToContent != SizeToContent.Manual && this.WindowState == WindowState.Maximized)
        arrangeBounds = new Size(this.MaxWidth, this.MaxHeight);
    }
    else if (this.SizeToContent != SizeToContent.Manual && this.WindowState == WindowState.Maximized)
      arrangeBounds = new Size(this.MaxWidth, this.MaxHeight);
    return base.ArrangeOverride(arrangeBounds);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.WindowState == WindowState.Maximized && this.updateNew && !this.UseNativeChrome || this.SizeToContent != SizeToContent.Manual && this.WindowState == WindowState.Maximized)
      availableSize = new Size(this.MaxWidth, this.MaxHeight);
    return base.MeasureOverride(availableSize);
  }

  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);
    Mouse.OverrideCursor = (Cursor) null;
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (Mouse.DirectlyOver is Visual && VisualUtils.FindAncestor(Mouse.DirectlyOver as Visual, typeof (Syncfusion.Windows.Shared.ResizeGripStyle)) is Syncfusion.Windows.Shared.ResizeGripStyle && this.ResizeMode == ResizeMode.CanResizeWithGrip)
      Mouse.OverrideCursor = Cursors.SizeNWSE;
    if (e.Source is Window)
      this.SendSizingMessage(e.GetPosition((IInputElement) this), e);
    else if (e.Source is FrameworkElement && (e.Source as FrameworkElement).GetType().BaseType == typeof (ChromelessWindow))
      this.SendSizingMessage(e.GetPosition((IInputElement) this), e);
    else if (e.Source is FrameworkElement && (e.Source as FrameworkElement).GetType() != typeof (Window) && (e.Source as FrameworkElement).TemplatedParent != null && (e.Source as FrameworkElement).TemplatedParent.GetType().BaseType == typeof (ChromelessWindow))
      this.SendSizingMessage(e.GetPosition((IInputElement) this), e);
    base.OnPreviewMouseLeftButtonDown(e);
  }

  private void SendSizingMessage(Point point, MouseButtonEventArgs e)
  {
    ChromelessWindowInterop.SizingDirection sizingDirection = this.GetSizingDirection(point);
    if (sizingDirection == ChromelessWindowInterop.SizingDirection.None)
      return;
    this.SendSizingMessage(sizingDirection);
    e.Handled = true;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (e.LeftButton != MouseButtonState.Pressed || this.TitleBar == null || !this.TitleBar.IsMouseOver || this.WindowState != WindowState.Maximized || !this.dragIsIn)
      return;
    this.dragIsIn = false;
    this.m_lastTitlebarPoint = e.GetPosition((IInputElement) this);
    double num1 = this.m_lastTitlebarPoint.X * 100.0 / (this.Width - 72.0);
    double num2 = this.m_lastTitlebarPoint.Y * 100.0 / this.Height;
    if (this.ActualWidth != 1280.0)
    {
      if (!this.flag)
      {
        ChromelessWindow.m_lastTitlebarPointXTemp = this.m_lastTitlebarPoint.X + this.Left;
        this.m_lastTitlebarPoint.X = ChromelessWindow.m_lastTitlebarPointXTemp;
        this.flag = true;
      }
      else
        this.m_lastTitlebarPoint.X = ChromelessWindow.m_lastTitlebarPointXTemp;
    }
    double num3 = (this.Width - 72.0) * num1 / 100.0;
    double num4 = this.Height * num2 / 100.0;
    this.suspendLocationUpdate = true;
    this.WindowState = WindowState.Normal;
    this.SetValue(Window.LeftProperty, (object) (this.m_lastTitlebarPoint.X - num3));
    this.SetValue(Window.TopProperty, (object) (this.m_lastTitlebarPoint.Y - num4));
    this.suspendLocationUpdate = false;
    this.OnLocationChanged(EventArgs.Empty);
    this.DragMove();
  }

  protected override void OnPreviewMouseMove(MouseEventArgs e)
  {
    base.OnPreviewMouseMove(e);
    Cursor cursor = (Cursor) null;
    if (e.LeftButton != MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
    {
      switch (this.GetSizingDirection(e.GetPosition((IInputElement) this)))
      {
        case ChromelessWindowInterop.SizingDirection.None:
          if (this.Cursor != null)
          {
            Mouse.OverrideCursor = (Cursor) null;
            this.Cursor = (Cursor) null;
            goto label_13;
          }
          goto label_13;
        case ChromelessWindowInterop.SizingDirection.West:
        case ChromelessWindowInterop.SizingDirection.East:
          cursor = Cursors.SizeWE;
          break;
        case ChromelessWindowInterop.SizingDirection.North:
        case ChromelessWindowInterop.SizingDirection.South:
          cursor = Cursors.SizeNS;
          break;
        case ChromelessWindowInterop.SizingDirection.NorthWest:
          cursor = Cursors.SizeNWSE;
          break;
        case ChromelessWindowInterop.SizingDirection.NorthEast:
          cursor = Cursors.SizeNESW;
          break;
        case ChromelessWindowInterop.SizingDirection.SouthWest:
          cursor = Cursors.SizeNESW;
          break;
        case ChromelessWindowInterop.SizingDirection.SouthEast:
          cursor = Cursors.SizeNWSE;
          break;
      }
      if (cursor != null)
        this.Cursor = cursor;
    }
label_13:
    if (this.m_isSizing)
      e.Handled = true;
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    this.Cursor = (Cursor) null;
  }

  private IntPtr HookMethod(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
  {
    switch ((WindowsMessages) msg)
    {
      case WindowsMessages.WM_SIZE:
        return this.HandleWM_SIZE(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_GETMINMAXINFO:
        return this.HandleWM_GETMINMAXINFO(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_NCCALCSIZE:
        return this.HandleWM_NCCALCSIZE(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_NCHITTEST:
        return this.HandleWM_NCHITTEST(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_SIZING:
        return this.HandleWM_SIZING(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_ENTERSIZEMOVE:
        return this.HandleWM_ENTERSIZEMOVE(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_EXITSIZEMOVE:
        return this.HandleWM_EXITSIZEMOVE(hWnd, msg, wParam, lParam, ref handled);
      case WindowsMessages.WM_DWMCOMPOSITIONCHANGED:
        return this.HandleWM_DWMCOMPOSITIONCHANGED(hWnd, msg, wParam, lParam, ref handled);
      default:
        return IntPtr.Zero;
    }
  }

  private IntPtr HandleWM_SIZE(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    switch (IntPtr.Size == 4 ? wParam.ToInt32() : (int) wParam.ToInt64())
    {
      case 0:
      case 2:
        this.UpdateWindowRegion(true);
        break;
    }
    return IntPtr.Zero;
  }

  private IntPtr HandleWM_SIZING(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
    if (virtualScreenWidth != 0.0 && !this.iscustommaxwidthused)
      this.MaxWidth = virtualScreenWidth;
    this.UpdateWindowRegion(true);
    return IntPtr.Zero;
  }

  private IntPtr HandleWM_EXITSIZEMOVE(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    this.m_isSizing = false;
    this.UpdateWindowRegion(true);
    return IntPtr.Zero;
  }

  private IntPtr HandleWM_ENTERSIZEMOVE(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    this.m_isSizing = true;
    return IntPtr.Zero;
  }

  private IntPtr HandleWM_GETMINMAXINFO(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (!this.CanEnableGlass && !this.HideTaskBar)
    {
      IntPtr hMonitor = ChromelessWindowInterop.MonitorFromWindow(hWnd, 2);
      ChromelessWindowInterop.MINMAXINFO structure = (ChromelessWindowInterop.MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (ChromelessWindowInterop.MINMAXINFO));
      bool flag = false;
      ChromelessWindowInterop.MONITORINFO lpmi = new ChromelessWindowInterop.MONITORINFO();
      if (hMonitor != IntPtr.Zero)
      {
        ChromelessWindowInterop.GetMonitorInfo(hMonitor, lpmi);
        ChromelessWindowInterop.RECT rcWork = lpmi.rcWork;
        ChromelessWindowInterop.RECT rcMonitor = lpmi.rcMonitor;
        structure.ptMaxPosition.x = !(Environment.OSVersion.Platform.ToString() == "Win32NT") || !(Environment.OSVersion.Version.Major.ToString() == "6") || !(Environment.OSVersion.Version.Minor.ToString() == "1") ? Math.Abs(rcWork.left - rcMonitor.left) : Math.Abs(rcWork.left - rcMonitor.left);
        structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
        structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
        structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top) + (rcMonitor.Height == rcWork.Height ? 2 : 3);
        IntPtr num1 = ChromelessWindowInterop.MonitorFromWindow(ChromelessWindowInterop.FindWindow("Shell_TrayWnd", (string) null), 2);
        if (hMonitor.Equals((object) num1))
        {
          ChromelessWindowInterop.APPBARDATA pData = new ChromelessWindowInterop.APPBARDATA();
          pData.cbSize = Marshal.SizeOf<ChromelessWindowInterop.APPBARDATA>(pData);
          pData.hWnd = new WindowInteropHelper((Window) this).EnsureHandle();
          int num2 = (int) ChromelessWindowInterop.SHAppBarMessage(5, ref pData);
          if (Convert.ToBoolean(ChromelessWindowInterop.SHAppBarMessage(4, ref pData)))
          {
            switch (pData.uEdge)
            {
              case 0:
                ++structure.ptMaxPosition.x;
                --structure.ptMaxTrackSize.x;
                --structure.ptMaxSize.x;
                break;
              case 1:
                --structure.ptMaxSize.x;
                --structure.ptMaxTrackSize.x;
                break;
              case 2:
                structure.ptMaxPosition.y += 3;
                structure.ptMaxTrackSize.y -= 3;
                structure.ptMaxSize.y -= 3;
                break;
              case 3:
                structure.ptMaxSize.y -= 3;
                structure.ptMaxTrackSize.y -= 3;
                break;
            }
          }
        }
        if (rcMonitor.Location.X < 0 || rcMonitor.Location.Y < 0)
          flag = true;
      }
      if (this.MaxHeight == double.PositiveInfinity || this.MaxWidth == double.PositiveInfinity || flag || this.MaxHeight != (double) structure.ptMaxSize.y || this.MaxWidth != (double) structure.ptMaxSize.x)
      {
        if ((this.MaxHeight == double.PositiveInfinity || this.MaxHeight != (double) structure.ptMaxSize.y) && !this.iscustommaxhghtused)
          this.MaxHeight = (double) structure.ptMaxSize.y;
        if ((this.MaxWidth == double.PositiveInfinity || this.MaxWidth != (double) structure.ptMaxSize.x) && !this.iscustommaxwidthused)
          this.MaxWidth = (double) structure.ptMaxSize.x;
        if (SystemParameters.PrimaryScreenWidth < SystemParameters.VirtualScreenWidth)
        {
          if (!this.iscustommaxwidthused)
          {
            if (SystemParameters.PrimaryScreenWidth != (double) lpmi.rcWork.Width)
              this.MaxWidth = (double) lpmi.rcWork.Width;
            else if (SystemParameters.PrimaryScreenWidth != (double) lpmi.rcMonitor.Width)
              this.MaxWidth = Math.Abs(SystemParameters.VirtualScreenWidth - SystemParameters.PrimaryScreenWidth);
          }
          if (!this.iscustommaxhghtused)
          {
            double num = (double) (lpmi.cbSize - 2);
            if (SystemParameters.VirtualScreenHeight - (double) structure.ptMaxSize.y < (double) lpmi.cbSize)
              num = SystemParameters.VirtualScreenHeight - (double) structure.ptMaxSize.y;
            this.MaxHeight = (double) this.DPIcalculationX(SystemParameters.VirtualScreenHeight) - num;
            if (SystemParameters.PrimaryScreenHeight != (double) lpmi.rcMonitor.Height)
              this.MaxHeight = (double) lpmi.rcWork.Height;
            else
              this.MaxHeight = (double) this.DPIcalculationX(SystemParameters.VirtualScreenHeight) - num;
          }
        }
      }
      structure.ptMinTrackSize = new ChromelessWindowInterop.POINT(this.MinWidth > 0.0 ? this.DPIcalculationX(Convert.ToDouble(this.MinWidth)) : this.DPIcalculationX(Convert.ToDouble(160.0)), this.MinHeight > 0.0 ? this.DPIcalculationY(Convert.ToDouble(this.MinHeight)) : this.DPIcalculationY(Convert.ToDouble(38.0)));
      if (!this.iscustommaxhghtused && !this.iscustommaxwidthused)
        structure.ptMaxTrackSize = new ChromelessWindowInterop.POINT((int) this.MaxWidth, (int) this.MaxHeight);
      Marshal.StructureToPtr<ChromelessWindowInterop.MINMAXINFO>(structure, lParam, true);
    }
    return IntPtr.Zero;
  }

  private int DPIcalculationX(double desiredPixels)
  {
    return (int) (desiredPixels * (double) this.DpiX / 96.0);
  }

  private int DPIcalculationY(double desiredPixels)
  {
    return (int) (desiredPixels * (double) this.DpiY / 96.0);
  }

  private int DpiX => ChromelessWindow.GetDeviceCaps(ChromelessWindow.GetDC(IntPtr.Zero), 88);

  private int DpiY => ChromelessWindow.GetDeviceCaps(ChromelessWindow.GetDC(IntPtr.Zero), 90);

  [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
  private static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
  private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

  private IntPtr HandleWM_DWMCOMPOSITIONCHANGED(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    this.m_dwmEnabled = ChromelessWindowInterop.CanEnableDwm();
    this.UpdateGlassChange();
    return IntPtr.Zero;
  }

  private IntPtr HandleWM_NCCALCSIZE(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    IntPtr num = IntPtr.Zero;
    if (this.CanEnableGlass)
    {
      if (wParam == IntPtr.Zero)
      {
        Marshal.StructureToPtr<ChromelessWindowInterop.RECT>(ChromelessWindowInterop.RECT.FromRectangle(this.CalculateClientRectangle(((ChromelessWindowInterop.RECT) Marshal.PtrToStructure(lParam, typeof (ChromelessWindowInterop.RECT))).ToRectangle())), lParam, false);
        num = IntPtr.Zero;
      }
      else
      {
        ChromelessWindowInterop.NCCALCSIZE_PARAMS structure1 = (ChromelessWindowInterop.NCCALCSIZE_PARAMS) Marshal.PtrToStructure(lParam, typeof (ChromelessWindowInterop.NCCALCSIZE_PARAMS));
        ChromelessWindowInterop.WINDOWPOS structure2 = (ChromelessWindowInterop.WINDOWPOS) Marshal.PtrToStructure(structure1.lppos, typeof (ChromelessWindowInterop.WINDOWPOS));
        ChromelessWindowInterop.RECT rect = ChromelessWindowInterop.RECT.FromRectangle(this.CalculateClientRectangle(new Rect((double) structure2.x, (double) structure2.y, (double) structure2.cx, (double) structure2.cy)));
        structure1.rgrc0 = rect;
        structure1.rgrc1 = rect;
        Marshal.StructureToPtr<ChromelessWindowInterop.NCCALCSIZE_PARAMS>(structure1, lParam, false);
        num = new IntPtr(1024 /*0x0400*/);
      }
    }
    return num;
  }

  private IntPtr HandleWM_NCHITTEST(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (!this.CanEnableGlass)
      return IntPtr.Zero;
    IntPtr plResult = IntPtr.Zero;
    this.PointFromScreen(new Point((double) ChromelessWindowInterop.GetX(lParam), (double) ChromelessWindowInterop.GetY(lParam)));
    ChromelessWindowInterop.DwmDefWindowProc(hWnd, 132, wParam, lParam, out plResult);
    int num = IntPtr.Size == 4 ? plResult.ToInt32() : (int) plResult.ToInt64();
    handled = true;
    if (num == 20 || num == 8 || num == 9 || num == 21)
      return plResult;
    if (this.ResizeMode != ResizeMode.NoResize)
      handled = false;
    return IntPtr.Zero;
  }

  private void UpdateWindowRegion(bool bRedraw)
  {
    if (this.CanEnableGlass)
      return;
    IntPtr handle = new WindowInteropHelper((Window) this).Handle;
    int width = (int) this.Width + 1;
    int height = (int) this.Height + 1;
    if (handle != IntPtr.Zero)
    {
      ChromelessWindowInterop.RECT r = new ChromelessWindowInterop.RECT();
      if (this.WindowState == WindowState.Maximized)
      {
        IntPtr hMonitor = ChromelessWindowInterop.MonitorFromWindow(handle, 2);
        ChromelessWindowInterop.MONITORINFO lpmi = new ChromelessWindowInterop.MONITORINFO();
        ChromelessWindowInterop.GetMonitorInfo(hMonitor, lpmi);
        r = lpmi.rcWork;
      }
      else
        ChromelessWindowInterop.GetWindowRect(handle, ref r);
      width = r.Width + 1;
      height = r.Height + 1;
      if (this.SizeToContent == SizeToContent.WidthAndHeight && !this.UseNativeChrome)
      {
        if (this.WindowState == WindowState.Maximized)
          this.InvalidateMeasure();
        if (this.WindowState == WindowState.Normal)
          this.InvalidateMeasure();
      }
    }
    this.DefineWindowRegion(bRedraw, width, height);
  }

  private static int Offset(int value) => value << 16 /*0x10*/ >> 16 /*0x10*/;

  private void DefineWindowRegion(bool bRedraw, int width, int height)
  {
    if (this.WindowState == WindowState.Minimized)
      return;
    IntPtr handle = new WindowInteropHelper((Window) this).Handle;
    IntPtr hRgn = IntPtr.Zero;
    if (this.WindowState != WindowState.Maximized)
      hRgn = ChromelessWindowInterop.CreateRoundRectRgn(0, 0, width, height, 0, 0);
    ChromelessWindowInterop.SetWindowRgn(handle, hRgn, bRedraw);
  }

  private void UpdateGlassChange()
  {
    if (!this.m_isInitialized)
      return;
    if (!this.CanEnableGlass)
    {
      if (this.WindowStyle != WindowStyle.None)
        this.WindowStyle = WindowStyle.None;
      if (!this.UseNativeChrome)
      {
        IntPtr handle = new WindowInteropHelper((Window) this).Handle;
        if (handle != IntPtr.Zero)
          HwndSource.FromHwnd(handle).CompositionTarget.BackgroundColor = Colors.Transparent;
        int windowLong = ChromelessWindowInterop.GetWindowLong(handle, -16);
        int dwNewLong = windowLong & ~(windowLong & 262144 /*0x040000*/);
        ChromelessWindowInterop.SetWindowLong(handle, -16, dwNewLong);
        ChromelessWindowInterop.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39);
      }
      this.UpdateWindowRegion(false);
      this.IsGlassActive = false;
    }
    else
    {
      IntPtr handle = new WindowInteropHelper((Window) this).Handle;
      ChromelessWindowInterop.SetWindowRgn(handle, IntPtr.Zero, false);
      int windowLong = ChromelessWindowInterop.GetWindowLong(handle, -16);
      if ((windowLong & 262144 /*0x040000*/) == 0)
      {
        int dwNewLong = windowLong | 262144 /*0x040000*/;
        ChromelessWindowInterop.SetWindowLong(handle, -16, dwNewLong);
        ChromelessWindowInterop.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39);
      }
      if (this.WindowStyle == WindowStyle.None)
        this.WindowStyle = WindowStyle.SingleBorderWindow;
      this.ExtendWindow();
      this.IsGlassActive = true;
    }
  }

  private void ExtendWindow()
  {
    if (!this.CanEnableGlass || !this.m_isInitialized)
      return;
    int size = (int) Math.Ceiling(29.0 * this.DPIOffset) - 1;
    IntPtr handle = new WindowInteropHelper((Window) this).Handle;
    HwndSource.FromHwnd(handle).CompositionTarget.BackgroundColor = Colors.Transparent;
    ChromelessWindowInterop.ExtendWindow(handle, size);
  }

  private Rect CalculateClientRectangle(Rect rect)
  {
    double num1 = SystemParameters.ResizeFrameVerticalBorderWidth;
    double num2 = SystemParameters.ResizeFrameHorizontalBorderHeight;
    double dpiOffset = this.DPIOffset;
    if (dpiOffset >= 1.5)
    {
      double num3 = 1.0 - Math.Abs(dpiOffset - 1.0);
      num1 = num3 * num1;
      num2 = num3 * num2;
    }
    rect.X += num1;
    rect.Width -= num1 * 2.0;
    rect.Height -= num2;
    return rect;
  }

  private ChromelessWindowInterop.SizingDirection GetSizingDirection(Point point)
  {
    if (this.ResizeMode == ResizeMode.NoResize || this.WindowState != WindowState.Normal)
      return ChromelessWindowInterop.SizingDirection.None;
    Size size = new Size(this.ActualWidth, this.ActualHeight);
    if (ChromelessWindowInterop.RECT.GetExtendedRect(new Rect(0.0, 0.0, size.Width, size.Height), this.ResizeBorderThickness, this.Padding, this.Margin).Contains(point))
    {
      if (this.ResizeGrip != null && this.ResizeMode == ResizeMode.CanResizeWithGrip && this.ResizeGrip.IsMouseOver)
        return ChromelessWindowInterop.SizingDirection.SouthEast;
      if (this.closeButton != null && this.closeButton.IsMouseOver)
        return ChromelessWindowInterop.SizingDirection.None;
    }
    this.cornerRadius = this.CornerRadius.BottomLeft != 0.0 || this.CornerRadius.BottomRight != 0.0 || this.CornerRadius.TopLeft != 0.0 || this.CornerRadius.TopRight != 0.0 ? this.CornerRadius : new CornerRadius(5.0, 5.0, 5.0, 5.0);
    if (point.Y > this.ResizeBorderThickness.Top + this.Padding.Top + this.Margin.Top || point.Y < 0.0 || point.X > this.CornerRadius.TopLeft + this.Padding.Top + this.Padding.Left + this.Margin.Left || point.X < 0.0)
    {
      if (point.Y <= this.cornerRadius.TopRight + this.Padding.Top + this.Padding.Right + this.Margin.Top && point.X >= size.Width - (this.cornerRadius.TopRight + this.Padding.Top + this.Padding.Right + this.Margin.Left + this.Margin.Right))
        return ChromelessWindowInterop.SizingDirection.NorthEast;
      if (point.Y >= size.Height - (this.cornerRadius.BottomLeft + this.Padding.Bottom + this.Padding.Left + this.Margin.Top + this.Margin.Bottom) && point.Y <= size.Height && point.X <= this.cornerRadius.BottomLeft + this.Padding.Bottom + this.Padding.Left + this.Margin.Left && point.X >= 0.0)
        return ChromelessWindowInterop.SizingDirection.SouthWest;
      if (point.Y >= size.Height - (this.cornerRadius.BottomRight + this.Padding.Bottom + this.Padding.Right + this.Margin.Top + this.Margin.Bottom) && point.Y <= size.Height && point.X >= size.Width - (this.cornerRadius.BottomRight + this.Padding.Bottom + this.Padding.Right + this.Margin.Left + this.Margin.Right) && point.X <= size.Width)
        return ChromelessWindowInterop.SizingDirection.SouthEast;
      if (point.Y - 1.0 <= this.ResizeBorderThickness.Top + this.Padding.Top + this.Margin.Top && point.Y >= 0.0)
        return ChromelessWindowInterop.SizingDirection.North;
      if (point.X - 1.0 <= this.ResizeBorderThickness.Left + this.Padding.Left + this.Margin.Left && point.X >= 0.0)
        return ChromelessWindowInterop.SizingDirection.West;
      if (point.X + 1.0 >= size.Width - (this.ResizeBorderThickness.Right + this.Padding.Right + this.Margin.Right + this.Margin.Left) && point.X <= size.Width)
        return ChromelessWindowInterop.SizingDirection.East;
      return point.Y + 1.0 < size.Height - (this.ResizeBorderThickness.Bottom + this.Padding.Bottom + this.Margin.Top + this.Margin.Bottom) || point.Y > size.Height ? ChromelessWindowInterop.SizingDirection.None : ChromelessWindowInterop.SizingDirection.South;
    }
    return point.Y <= this.cornerRadius.TopLeft + this.Padding.Top + this.Margin.Top && point.X <= this.cornerRadius.TopLeft + this.Padding.Left + this.Margin.Left ? ChromelessWindowInterop.SizingDirection.NorthWest : ChromelessWindowInterop.SizingDirection.None;
  }

  public event ChromelessWindow.ReSizeGripMouseEventHandler ReSizeGripMouseEvent;

  private void SendSizingMessage(ChromelessWindowInterop.SizingDirection sizing)
  {
    if (Mouse.LeftButton == MouseButtonState.Pressed)
    {
      if (this.ReSizeGripMouseEvent != null)
        this.ReSizeGripMouseEvent((object) this, new ReSizeGripMouseEventArgs()
        {
          ButtonState = MouseButtonState.Pressed
        });
      IntPtr handle = new WindowInteropHelper((Window) this).Handle;
      ChromelessWindowInterop.SendMessage(handle, 274, (int) (61440 /*0xF000*/ + sizing), 0);
      ChromelessWindowInterop.SendMessage(handle, 514, 0, 0);
    }
    if (Mouse.LeftButton != MouseButtonState.Released || this.ReSizeGripMouseEvent == null)
      return;
    this.ReSizeGripMouseEvent((object) this, new ReSizeGripMouseEventArgs()
    {
      ButtonState = MouseButtonState.Released
    });
  }

  private void HandleSizeToContent()
  {
    if (this.SizeToContent == SizeToContent.Manual)
      return;
    SizeToContent previousSizeToContent = this.SizeToContent;
    this.SizeToContent = SizeToContent.Manual;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
    {
      this.SizeToContent = previousSizeToContent;
      this.Visibility = Visibility.Visible;
    }));
  }

  public delegate void ReSizeGripMouseEventHandler(object sender, ReSizeGripMouseEventArgs e);
}
