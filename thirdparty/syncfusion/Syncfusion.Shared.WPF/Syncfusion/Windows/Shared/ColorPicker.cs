// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorPicker
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2013Style.xaml")]
[DesignTimeVisible(true)]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/SyncOrange.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/ShinyRed.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/ShinyBlue.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ColorPicker), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ColorPicker/Themes/generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ColorPicker), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/TransparentStyle.xaml")]
public class ColorPicker : Control, IDisposable
{
  private const string C_colorToggleButton = "colorToggleButton";
  private const string C_colorEditPopup = "colorEditPopup";
  private const string C_colorEdit = "ColorEdit";
  private const string C_color = "Color";
  private const string C_defaultSkinName = "Default";
  private const string C_colorEditContainerBrush = "ColorEditContainerBrush";
  private const string C_systemColors = "systemColors";
  internal LinearGradientBrush m_gradient;
  internal ColorEdit m_colorEditor;
  private ComboBox m_systemColors;
  private Popup m_colorEditorPopup;
  private ToggleButton m_colorToggleButton;
  public static RoutedCommand M_displayPopup;
  private Window window;
  private Brush m_tempBrush;
  internal bool flag;
  private CommandBinding displayPopup;
  private ComboBox obj;
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Color), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.Transparent, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorPicker.OnColorChanged)));
  internal static readonly DependencyProperty IsColorPaletteVisibleProperty = DependencyProperty.Register(nameof (IsColorPaletteVisible), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  public static readonly DependencyProperty EnableSolidToGradientSwitchProperty = DependencyProperty.RegisterAttached(nameof (EnableSolidToGradientSwitch), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
  public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof (Brush), typeof (Brush), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(ColorPicker.OnSelectedBrushChanged)));
  public static readonly DependencyProperty VisualizationStyleProperty = DependencyProperty.Register(nameof (VisualizationStyle), typeof (ColorSelectionMode), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) ColorSelectionMode.RGB));
  public static readonly DependencyProperty BrushModeProperty = DependencyProperty.Register(nameof (BrushMode), typeof (BrushModes), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) BrushModes.Solid, new PropertyChangedCallback(ColorPicker.OnSelectedBrushModeChanged)));
  public static readonly DependencyProperty GradientPropertyEditorModeProperty = DependencyProperty.Register(nameof (GradientPropertyEditorMode), typeof (GradientPropertyEditorMode), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) GradientPropertyEditorMode.Extended, new PropertyChangedCallback(ColorPicker.OnGradientPropertyEditorModeChanged)));
  public static readonly DependencyProperty IsOpenGradientPropertyEditorProperty = DependencyProperty.Register(nameof (IsOpenGradientPropertyEditor), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(ColorPicker.OnIsOpenGradientPropertyEditorChanged)));
  public static readonly DependencyProperty GradientBrushDisplayModeProperty = DependencyProperty.Register(nameof (GradientBrushDisplayMode), typeof (GradientBrushDisplayMode), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) GradientBrushDisplayMode.Default, new PropertyChangedCallback(ColorPicker.OnGradientBrushDisplayModeChanged)));
  public static readonly DependencyProperty IsGradientPropertyEnabledProperty = DependencyProperty.Register(nameof (IsGradientPropertyEnabled), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(ColorPicker.OnIsGradientPropertyEnabledChanged)));
  public static readonly DependencyProperty ColorEditBackgroundProperty = DependencyProperty.Register(nameof (ColorEditBackground), typeof (Brush), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(ColorPicker.OnColorEditBackgroundChanged)));
  public static readonly DependencyProperty EnableToolTipProperty = DependencyProperty.Register(nameof (EnableToolTip), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(ColorPicker.OnEnableToolTipChanged)));
  public static readonly DependencyProperty IsAlphaVisibleProperty = DependencyProperty.Register(nameof (IsAlphaVisible), typeof (bool), typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(ColorPicker.IsAlphaVisiblePropertyChanged)));

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ColorPicker.HeaderTemplateProperty);
    set => this.SetValue(ColorPicker.HeaderTemplateProperty, (object) value);
  }

  public Color Color
  {
    get => (Color) this.GetValue(ColorPicker.ColorProperty);
    set => this.SetValue(ColorPicker.ColorProperty, (object) value);
  }

  public bool IsColorPaletteVisible
  {
    get => (bool) this.GetValue(ColorPicker.IsColorPaletteVisibleProperty);
    set => this.SetValue(ColorPicker.IsColorPaletteVisibleProperty, (object) value);
  }

  public ColorSelectionMode VisualizationStyle
  {
    get => (ColorSelectionMode) this.GetValue(ColorPicker.VisualizationStyleProperty);
    set => this.SetValue(ColorPicker.VisualizationStyleProperty, (object) value);
  }

  public Brush ColorEditBackground
  {
    get => (Brush) this.GetValue(ColorPicker.ColorEditBackgroundProperty);
    set => this.SetValue(ColorPicker.ColorEditBackgroundProperty, (object) value);
  }

  public bool IsAlphaVisible
  {
    get => (bool) this.GetValue(ColorPicker.IsAlphaVisibleProperty);
    set => this.SetValue(ColorPicker.IsAlphaVisibleProperty, (object) value);
  }

  public BrushModes BrushMode
  {
    get => (BrushModes) this.GetValue(ColorPicker.BrushModeProperty);
    set => this.SetValue(ColorPicker.BrushModeProperty, (object) value);
  }

  public bool EnableSolidToGradientSwitch
  {
    get => (bool) this.GetValue(ColorPicker.EnableSolidToGradientSwitchProperty);
    set => this.SetValue(ColorPicker.EnableSolidToGradientSwitchProperty, (object) value);
  }

  public Brush Brush
  {
    get => (Brush) this.GetValue(ColorPicker.BrushProperty);
    set
    {
      try
      {
        this.SetValue(ColorPicker.BrushProperty, (object) value);
      }
      catch (Exception ex)
      {
      }
    }
  }

  public GradientPropertyEditorMode GradientPropertyEditorMode
  {
    get
    {
      return (GradientPropertyEditorMode) this.GetValue(ColorPicker.GradientPropertyEditorModeProperty);
    }
    set => this.SetValue(ColorPicker.GradientPropertyEditorModeProperty, (object) value);
  }

  public GradientBrushDisplayMode GradientBrushDisplayMode
  {
    get => (GradientBrushDisplayMode) this.GetValue(ColorPicker.GradientBrushDisplayModeProperty);
    set => this.SetValue(ColorPicker.GradientBrushDisplayModeProperty, (object) value);
  }

  public bool IsOpenGradientPropertyEditor
  {
    get => (bool) this.GetValue(ColorPicker.IsOpenGradientPropertyEditorProperty);
    set => this.SetValue(ColorPicker.IsOpenGradientPropertyEditorProperty, (object) value);
  }

  public bool IsGradientPropertyEnabled
  {
    get => (bool) this.GetValue(ColorPicker.IsGradientPropertyEnabledProperty);
    set => this.SetValue(ColorPicker.IsGradientPropertyEnabledProperty, (object) value);
  }

  public bool EnableToolTip
  {
    get => (bool) this.GetValue(ColorPicker.EnableToolTipProperty);
    set => this.SetValue(ColorPicker.EnableToolTipProperty, (object) value);
  }

  static ColorPicker()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ColorPicker)));
    ColorPicker.M_displayPopup = new RoutedCommand();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ColorPicker()
  {
    if (ColorPicker.M_displayPopup != null)
    {
      this.displayPopup = new CommandBinding((ICommand) ColorPicker.M_displayPopup);
      this.displayPopup.Executed -= new ExecutedRoutedEventHandler(this.DisplayPopup);
      this.displayPopup.Executed += new ExecutedRoutedEventHandler(this.DisplayPopup);
      this.CommandBindings.Add(this.displayPopup);
    }
    this.m_colorEditor = new ColorEdit();
    Keyboard.AddKeyDownHandler((DependencyObject) this, new KeyEventHandler(this.OnKeyDown));
    Mouse.AddPreviewMouseDownOutsideCapturedElementHandler((DependencyObject) this, new MouseButtonEventHandler(this.OnMouseDownOutsideCapturedElement));
    Mouse.AddPreviewMouseDownHandler((DependencyObject) this, new MouseButtonEventHandler(this.OnMouseDown));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public event PropertyChangedCallback ColorChanged;

  public event PropertyChangedCallback EnableToolTipChanged;

  public event PropertyChangedCallback SelectedBrushChanged;

  public event PropertyChangedCallback SelectedBrushModeChanged;

  public event PropertyChangedCallback GradientPropertyEditorModeChanged;

  public event PropertyChangedCallback GradientBrushDisplayModeChanged;

  public event PropertyChangedCallback IsOpenGradientPropertyEditorChanged;

  public event PropertyChangedCallback IsGradientPropertyEnabledChanged;

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnColorChanged(e);
  }

  private static void OnSelectedBrushModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnSelectedBrushModeChanged(e);
  }

  private void OnSelectedBrushModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorEditor != null)
      this.m_colorEditor.BrushMode = this.BrushMode;
    if (this.SelectedBrushModeChanged == null)
      return;
    this.SelectedBrushModeChanged((DependencyObject) this, e);
  }

  private static void OnSelectedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ColorPicker colorPicker = (ColorPicker) d;
    if (e.NewValue is SolidColorBrush)
      colorPicker.BrushMode = BrushModes.Solid;
    else if (e.NewValue is LinearGradientBrush || e.NewValue is RadialGradientBrush)
      colorPicker.BrushMode = BrushModes.Gradient;
    colorPicker.OnSelectedBrushChanged(e);
  }

  private void OnSelectedBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue is SolidColorBrush)
    {
      if (this.m_colorEditor != null)
        this.m_colorEditor.BrushMode = BrushModes.Solid;
      if (this.IsGradientPropertyEnabled)
        this.Color = ((SolidColorBrush) e.NewValue).Color;
    }
    else if (e.NewValue is GradientBrush && this.m_colorEditor != null)
    {
      this.m_colorEditor.Brush = this.Brush;
      this.m_colorEditor.BrushMode = BrushModes.Gradient;
      this.m_tempBrush = e.NewValue as Brush;
      this.m_gradient = e.NewValue as LinearGradientBrush;
      object newValue = e.NewValue;
      if (!this.m_colorEditor.flag)
      {
        this.m_colorEditor.RefreshGradientStoppers(this.m_tempBrush);
        this.flag = true;
        if (((GradientBrush) e.NewValue).GradientStops.Count > 0)
          this.m_colorEditor.Color = ((GradientBrush) e.NewValue).GradientStops[((GradientBrush) e.NewValue).GradientStops.Count - 1].Color;
        if (!this.flag)
          this.m_colorEditor.setnocolor = true;
      }
      else
        this.m_colorEditor.flag = false;
    }
    if (this.m_colorEditor.SelectedColor != null && this.m_colorEditor.CurrentColor != null && this.m_colorEditor.count < 1)
    {
      this.m_colorEditor.CurrentColor.Fill = this.m_colorEditor.previousSelectedBrush;
      this.m_colorEditor.SelectedColor.Fill = (Brush) new SolidColorBrush(this.Color);
      ++this.m_colorEditor.count;
    }
    if (this.SelectedBrushChanged == null)
      return;
    this.SelectedBrushChanged((DependencyObject) this, e);
  }

  private static void OnGradientPropertyEditorModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnGradientPropertyEditorModeChanged(e);
  }

  private void OnGradientPropertyEditorModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.GradientPropertyEditorModeChanged == null)
      return;
    this.GradientPropertyEditorModeChanged((DependencyObject) this, e);
  }

  private static void OnIsOpenGradientPropertyEditorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnIsOpenGradientPropertyEditorChanged(e);
  }

  private void OnIsOpenGradientPropertyEditorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsOpenGradientPropertyEditorChanged == null)
      return;
    this.IsOpenGradientPropertyEditorChanged((DependencyObject) this, e);
  }

  private static void OnIsGradientPropertyEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnIsGradientPropertyEnabledChanged(e);
  }

  protected virtual void OnIsGradientPropertyEnabledChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.IsGradientPropertyEnabledChanged == null)
      return;
    this.IsGradientPropertyEnabledChanged((DependencyObject) this, e);
  }

  private static void OnGradientBrushDisplayModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnGradientBrushDisplayModeChanged(e);
  }

  private void OnGradientBrushDisplayModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.GradientBrushDisplayModeChanged == null)
      return;
    this.GradientBrushDisplayModeChanged((DependencyObject) this, e);
  }

  private static void OnColorEditBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnColorEditBackgroundChanged(e);
  }

  private static void OnEnableToolTipChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPicker) d).OnEnableToolTipChanged(e);
  }

  private static void OnEnableSwitchChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (d is ColorPicker)
    {
      ((ColorPicker) d).OnEnableSwitchChanged(e);
    }
    else
    {
      ColorEdit colorEdit = (ColorEdit) d;
      colorEdit.Loaded -= new RoutedEventHandler(ColorPicker.instance_Loaded);
      colorEdit.Loaded += new RoutedEventHandler(ColorPicker.instance_Loaded);
    }
  }

  private static void instance_Loaded(object sender, RoutedEventArgs e)
  {
  }

  private void OnEnableSwitchChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorEditor.enableSwitch == null)
      return;
    if ((bool) e.NewValue)
      this.m_colorEditor.enableSwitch.Visibility = Visibility.Visible;
    else
      this.m_colorEditor.enableSwitch.Visibility = Visibility.Collapsed;
  }

  private static void IsAlphaVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.m_colorToggleButton = this.GetTemplateChild("colorToggleButton") as ToggleButton;
    this.m_colorEditorPopup = this.GetTemplateChild("colorEditPopup") as Popup;
    this.m_colorEditor = this.GetTemplateChild("ColorEdit") as ColorEdit;
    if (this.m_gradient != null && this.m_colorEditor != null)
    {
      this.m_colorEditor.Startpoint = this.m_gradient.StartPoint;
      this.m_colorEditor.Endpoint = this.m_gradient.EndPoint;
    }
    if (this.m_colorEditor != null)
      this.m_colorEditor.m_colorPicker = this;
    this.m_systemColors = this.Template.FindName("systemColors", (FrameworkElement) this) as ComboBox;
    if (this.m_colorEditor != null)
    {
      this.m_colorEditor.SetBinding(ColorEdit.ColorProperty, (BindingBase) new Binding("Color")
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay
      });
      if (this.m_colorEditorPopup != null)
      {
        this.m_colorEditorPopup.Placement = PlacementMode.Bottom;
        this.m_colorEditorPopup.Opened += new EventHandler(this.ColorEditorPopup_Opened);
        this.m_colorEditorPopup.Closed += new EventHandler(this.OnColorEditPopupClosed);
        this.window = Window.GetWindow((DependencyObject) this);
        if (this.window != null)
          this.window.PreviewMouseDown += new MouseButtonEventHandler(this.Window_PreviewMouseDown);
      }
      this.SelectPaletteColor();
      if (this.flag && this.m_colorEditor != null)
      {
        this.m_colorEditor.RefreshGradientStoppers(this.m_tempBrush);
        this.m_colorEditor.Brush = this.Brush;
        if (this.BrushMode == BrushModes.Gradient)
        {
          this.m_colorEditor.BrushMode = this.BrushMode;
          this.Color = this.m_colorEditor.gradientItemCollection.gradientItem.color;
        }
        this.flag = false;
        this.m_colorEditor.flag = false;
      }
    }
    this.obj = this.Template.FindName("systemColors", (FrameworkElement) this) as ComboBox;
  }

  private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.m_colorEditorPopup == null || !this.m_colorEditorPopup.IsOpen || this.m_colorEditorPopup.IsMouseOver || this.IsMouseOver)
      return;
    this.m_colorEditorPopup.IsOpen = false;
  }

  private void OnColorEditPopupClosed(object sender, EventArgs e)
  {
    if (!this.IsKeyboardFocusWithin)
      return;
    this.Focus();
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
  }

  internal void SelectPaletteColor()
  {
    if (this.Template == null || !this.IsColorPaletteVisible || this.obj == null)
      return;
    this.obj.DropDownOpened += new EventHandler(this.obj_DropDownOpened);
    IList itemsSource = this.obj.ItemsSource as IList;
    int num = -1;
    if (itemsSource == null)
      return;
    int index = 0;
    for (int count = itemsSource.Count; index < count; ++index)
    {
      if ((itemsSource[index] as ColorItem).Name == ColorEdit.SuchColor(this.Color)[0])
      {
        num = index;
        break;
      }
    }
    if (num == -1)
      return;
    this.obj.SelectedIndex = num;
  }

  private void obj_DropDownOpened(object sender, EventArgs e)
  {
    if (this.m_colorEditor == null)
      return;
    this.m_colorEditor.changeColor = true;
  }

  private void OnColorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ColorChanged == null || this.m_colorEditor == null)
      return;
    this.ColorChanged((DependencyObject) this, e);
  }

  private void OnColorEditBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorEditor == null)
      return;
    this.m_colorEditor.Background = (Brush) e.NewValue;
  }

  private void OnEnableToolTipChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.EnableToolTipChanged == null)
      return;
    this.EnableToolTipChanged((DependencyObject) this, e);
  }

  public void ColorChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
  {
    this.Color = (sender as ColorEdit).Color;
  }

  private void DisplayPopup(object sender, RoutedEventArgs e)
  {
    bool? isChecked = this.m_colorToggleButton.IsChecked;
    if ((!isChecked.GetValueOrDefault() ? 0 : (isChecked.HasValue ? 1 : 0)) == 0)
      return;
    Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    this.m_colorEditorPopup.Width = this.ActualWidth;
    if (this.m_colorEditor.m_wordKnownColorPopup != null)
      this.m_colorEditor.m_wordKnownColorPopup.IsOpen = false;
    if (this.m_systemColors != null)
    {
      ComboBox systemColors = this.m_systemColors;
      if (systemColors != null)
      {
        IList<ColorItem> itemsSource = systemColors.ItemsSource as IList<ColorItem>;
        int num = -1;
        if (itemsSource != null)
        {
          int index = 0;
          for (int count = itemsSource.Count; index < count; ++index)
          {
            if (itemsSource[index].Name == ColorEdit.SuchColor(this.Color)[0])
            {
              num = index;
              break;
            }
          }
          if (num != -1 && systemColors.SelectedIndex != num)
            systemColors.SelectedIndex = num;
        }
      }
    }
    Keyboard.Focus((IInputElement) this.m_colorEditor);
  }

  private void OnKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Escape)
      return;
    this.m_colorToggleButton.IsChecked = new bool?(false);
    if (!this.IsMouseCaptured)
      return;
    Mouse.Capture((IInputElement) null);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt && (e.SystemKey == Key.Down || e.SystemKey == Key.Up) || e.SystemKey == Key.F4)
    {
      ToggleButton colorToggleButton = this.m_colorToggleButton;
      bool? isChecked = this.m_colorToggleButton.IsChecked;
      bool? nullable = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
      colorToggleButton.IsChecked = nullable;
      if (this.m_colorEditorPopup.IsOpen)
      {
        this.m_colorEditorPopup.Width = this.ActualWidth;
        Keyboard.Focus((IInputElement) this.m_colorEditor);
      }
    }
    base.OnPreviewKeyDown(e);
  }

  private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
  {
    ToggleButton colorToggleButton = this.m_colorToggleButton;
    bool? isChecked = this.m_colorToggleButton.IsChecked;
    bool? nullable = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
    colorToggleButton.IsChecked = nullable;
    if (!this.IsMouseCaptured)
      return;
    Mouse.Capture((IInputElement) null);
  }

  private void ColorEditorPopup_Opened(object sender, EventArgs e)
  {
    this.m_colorEditor.EnableGradientToSolidSwitch = this.EnableSolidToGradientSwitch;
    this.m_colorEditor.BrushMode = this.BrushMode;
    this.m_colorEditor.Background = this.ColorEditBackground;
    this.m_colorEditor.GradientPropertyEditorMode = this.GradientPropertyEditorMode;
    this.m_colorEditor.IsOpenGradientPropertyEditor = this.IsOpenGradientPropertyEditor;
    this.m_colorEditor.IsGradientPropertyEnabled = this.IsGradientPropertyEnabled;
    this.m_colorEditor.IsAlphaVisible = this.IsAlphaVisible;
    this.m_colorEditor.Background = this.ColorEditBackground;
    this.m_colorEditor.Hcount = 0;
    if (this.m_colorEditor.SelectedColor != null)
      this.m_colorEditor.SelectedColor.Fill = (Brush) new SolidColorBrush(this.Color);
    if (this.VisualizationStyle != ColorSelectionMode.RGB)
      return;
    this.m_colorEditor.CalculateHSVSelectorPosition();
    this.m_colorEditor.CalculateHSVBackground();
    this.m_colorEditor.CalculateBackground();
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (this.m_colorEditorPopup == null || !this.m_colorEditorPopup.IsOpen)
      return;
    e.Handled = true;
  }

  private void OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsMouseCaptured || e.RightButton != MouseButtonState.Pressed)
      return;
    Mouse.Capture((IInputElement) null);
    ToggleButton colorToggleButton = this.m_colorToggleButton;
    bool? isChecked = this.m_colorToggleButton.IsChecked;
    bool? nullable = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
    colorToggleButton.IsChecked = nullable;
  }

  public void Dispose() => this.Dispose(true);

  internal void Dispose(bool disposing)
  {
    if (this.m_colorEditor != null)
    {
      this.m_colorEditor.Dispose();
      this.m_colorEditor = (ColorEdit) null;
    }
    if (this.m_colorEditorPopup != null)
    {
      this.m_colorEditorPopup.Opened -= new EventHandler(this.ColorEditorPopup_Opened);
      this.m_colorEditorPopup.Closed -= new EventHandler(this.OnColorEditPopupClosed);
      this.m_colorEditorPopup = (Popup) null;
    }
    if (this.m_colorToggleButton != null)
      this.m_colorToggleButton = (ToggleButton) null;
    if (this.window != null)
    {
      this.window.PreviewMouseDown -= new MouseButtonEventHandler(this.Window_PreviewMouseDown);
      this.window = (Window) null;
    }
    if (this.displayPopup != null)
    {
      this.displayPopup.Executed -= new ExecutedRoutedEventHandler(this.DisplayPopup);
      this.displayPopup = (CommandBinding) null;
    }
    if (this.m_gradient != null)
      this.m_gradient = (LinearGradientBrush) null;
    if (this.m_systemColors != null)
      this.m_systemColors = (ComboBox) null;
    if (this.m_tempBrush != null)
      this.m_tempBrush = (Brush) null;
    if (this.obj != null)
    {
      this.obj.DropDownOpened -= new EventHandler(this.obj_DropDownOpened);
      if (this.obj.ItemsSource is IList itemsSource)
      {
        for (int index = 0; index < itemsSource.Count; ++index)
          (itemsSource[index] as ColorItem).Dispose();
      }
      if (this.obj.ItemsSource != null)
        this.obj.ItemsSource = (IEnumerable) null;
      else if (this.obj.Items.Count > 0)
        this.obj.Items.Clear();
      this.obj = (ComboBox) null;
    }
    Keyboard.RemoveKeyDownHandler((DependencyObject) this, new KeyEventHandler(this.OnKeyDown));
    Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler((DependencyObject) this, new MouseButtonEventHandler(this.OnMouseDownOutsideCapturedElement));
    Mouse.RemovePreviewMouseDownHandler((DependencyObject) this, new MouseButtonEventHandler(this.OnMouseDown));
  }
}
