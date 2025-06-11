// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ColorPickerPalette
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2007Blue.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2007Silver.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2003.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ColorPickerPalette), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ColorPickerPalette/Themes/generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ColorPickerPalette), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPickerPalette/Themes/Office2007Black.xaml")]
public class ColorPickerPalette : Control, IDisposable
{
  public static readonly DependencyProperty AutomaticColorProperty = DependencyProperty.Register(nameof (AutomaticColor), typeof (Brush), typeof (ColorPickerPalette), new PropertyMetadata((object) new SolidColorBrush(Colors.Black), new PropertyChangedCallback(ColorPickerPalette.IsAutomaticColorChanged)));
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Color), typeof (ColorPickerPalette), new PropertyMetadata((object) Colors.Black, new PropertyChangedCallback(ColorPickerPalette.IsColorChanged)));
  public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(nameof (SelectedBrush), typeof (Brush), typeof (ColorPickerPalette), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(ColorPickerPalette.OnBrushChanged)));
  public static readonly DependencyProperty ThemeHeaderBackGroundProperty = DependencyProperty.Register(nameof (ThemeHeaderBackGround), typeof (Brush), typeof (ColorPickerPalette), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 231, (byte) 238))));
  public static readonly DependencyProperty SetCustomColorsProperty = DependencyProperty.Register(nameof (SetCustomColors), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false, new PropertyChangedCallback(ColorPickerPalette.OnSetCustomColorsChanged)));
  public static readonly DependencyProperty CustomColorsCollectionProperty = DependencyProperty.Register(nameof (CustomColorsCollection), typeof (ObservableCollection<CustomColor>), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CustomHeaderVisibilityProperty = DependencyProperty.Register(nameof (CustomHeaderVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty AutomaticColorVisibilityProperty = DependencyProperty.Register(nameof (AutomaticColorVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty NoColorVisibilityProperty = DependencyProperty.Register(nameof (NoColorVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty MoreColorOptionVisibilityProperty = DependencyProperty.Register(nameof (MoreColorOptionVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty CustomHeaderTextProperty = DependencyProperty.Register(nameof (CustomHeaderText), typeof (string), typeof (ColorPickerPalette), new PropertyMetadata((object) "CustomColors"));
  public static readonly DependencyProperty ThemeHeaderForeGroundProperty = DependencyProperty.Register(nameof (ThemeHeaderForeGround), typeof (Brush), typeof (ColorPickerPalette), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 50, (byte) 21, (byte) 110))));
  public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(nameof (MouseOverBackground), typeof (Brush), typeof (ColorPickerPalette));
  internal static readonly DependencyProperty ExpandedMoreColorsBorderPressedProperty = DependencyProperty.Register(nameof (ExpandedMoreColorsBorderPressed), typeof (bool), typeof (ColorPickerPalette));
  internal static readonly DependencyProperty ExpandedAutomaticBorderPressedProperty = DependencyProperty.Register(nameof (ExpandedAutomaticBorderPressed), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false));
  internal static readonly DependencyProperty IsAutomaticBorderPressedProperty = DependencyProperty.Register(nameof (IsAutomaticBorderPressed), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false));
  internal static readonly DependencyProperty IsMoreColorsBorderPressedProperty = DependencyProperty.Register(nameof (IsMoreColorsBorderPressed), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false));
  public static readonly DependencyProperty PopupWidthProperty = DependencyProperty.Register("PopUpWidth", typeof (double), typeof (ColorPickerPalette), new PropertyMetadata((object) 180.0));
  public static readonly DependencyProperty PopupHeightProperty = DependencyProperty.Register("PopUpHeight", typeof (double), typeof (ColorPickerPalette), new PropertyMetadata((object) 200.0, new PropertyChangedCallback(ColorPickerPalette.OnPopupHeightChanged)));
  public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register(nameof (BorderWidth), typeof (double), typeof (ColorPickerPalette), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty BorderHeightProperty = DependencyProperty.Register(nameof (BorderHeight), typeof (double), typeof (ColorPickerPalette), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register(nameof (ColorName), typeof (string), typeof (ColorPickerPalette), new PropertyMetadata((object) nameof (Color)));
  internal static readonly DependencyProperty ColorGroupItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (ColorGroupItem), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty MoreColorProperty = DependencyProperty.Register(nameof (SelectedMoreColor), typeof (PolygonItem), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ThemePanelVisibilityProperty = DependencyProperty.Register(nameof (ThemePanelVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ColorPickerPalette.ThemeVisibilityChanged)));
  public static readonly DependencyProperty StandardPanelVisibilityProperty = DependencyProperty.Register(nameof (StandardPanelVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ColorPickerPalette.standard_visibility_changed)));
  public static readonly DependencyProperty IsStandardTabVisibleProperty = DependencyProperty.Register(nameof (IsStandardTabVisible), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty IsCustomTabVisibleProperty = DependencyProperty.Register(nameof (IsCustomTabVisible), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty RecentlyUsedPanelVisibilityProperty = DependencyProperty.Register(nameof (RecentlyUsedPanelVisibility), typeof (Visibility), typeof (ColorPickerPalette), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ColorPickerPalette.recentlyusedvisibilitychanged)));
  public static readonly DependencyProperty ThemesProperty = DependencyProperty.Register(nameof (Themes), typeof (PaletteTheme), typeof (ColorPickerPalette), new PropertyMetadata((object) PaletteTheme.Office, new PropertyChangedCallback(ColorPickerPalette.ThemeColorChanged)));
  public static readonly DependencyProperty BlackWhiteVisibilityProperty = DependencyProperty.Register(nameof (BlackWhiteVisibility), typeof (BlackWhiteVisible), typeof (ColorPickerPalette), new PropertyMetadata((object) BlackWhiteVisible.None, new PropertyChangedCallback(ColorPickerPalette.BlackWhiteVisibilityChanged)));
  public static readonly DependencyProperty GenerateThemeVariantsProperty = DependencyProperty.Register(nameof (GenerateThemeVariants), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) true, new PropertyChangedCallback(ColorPickerPalette.ThemeVariantsChanged)));
  public static readonly DependencyProperty GenerateStandardVariantsProperty = DependencyProperty.Register(nameof (GenerateStandardVariants), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false, new PropertyChangedCallback(ColorPickerPalette.StandardVariantsChanged)));
  public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (ColorPickerPalette), new PropertyMetadata((object) false, new PropertyChangedCallback(ColorPickerPalette.OnIsExpandedChanged)));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof (IconSize), typeof (Size), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MoreColorsIconProperty = DependencyProperty.Register(nameof (MoreColorsIcon), typeof (ImageSource), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MoreColorsIconSizeProperty = DependencyProperty.Register(nameof (MoreColorsIconSize), typeof (Size), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectedCommandProperty = DependencyProperty.Register(nameof (SelectedCommand), typeof (ICommand), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (PickerMode), typeof (ColorPickerPalette), new PropertyMetadata((object) PickerMode.DropDown, new PropertyChangedCallback(ColorPickerPalette.OnModeChanged)));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ColorPickerPalette), new PropertyMetadata((PropertyChangedCallback) null));
  public ObservableCollection<ColorGroupItem>[] col = new ObservableCollection<ColorGroupItem>[16 /*0x10*/];
  public ObservableCollection<ColorGroupItem>[] CustomCol = new ObservableCollection<ColorGroupItem>[20];
  public ObservableCollection<ColorGroupItem> RecentlyUsedCollection = new ObservableCollection<ColorGroupItem>();
  public ObservableCollection<ColorGroupItem> StdColorCollection;
  public ObservableCollection<ColorGroupItem> ColorGroupCollection = new ObservableCollection<ColorGroupItem>();
  public ObservableCollection<ColorGroupItem> CustomColorGroupCollection = new ObservableCollection<ColorGroupItem>();
  private ColorGroup colorGroup = new ColorGroup();
  private ColorGroup colorGroup1 = new ColorGroup();
  private int row;
  private int column;
  private int popupItemIndex = -1;
  internal bool IsAutomaticSelected;
  internal bool IsSelected;
  private Window window;
  internal MoreColorsWindow child;
  internal bool IsChecked;
  internal bool updownclick;
  internal bool Isloaded;
  internal bool click;
  internal bool Loadfinished;
  internal Border morecolorsborder;
  internal Border automaticborder;
  private Border autoColorBorder;
  private Border Nocolorsborder;
  private ColorGroup item1;
  private ColorGroup item2;
  private ColorGroup item3;
  internal bool isPopupclosedOnAutomaticClick;
  private Border updown;
  private Border colorBorder;
  private Border popupBorder;
  private Border OutBorder;
  private Grid outerGrid;
  private Border LayoutBorder;
  internal ItemsControl ColorArea;
  internal Grid Colorgrid;
  internal string themestyle;
  internal double RefWidth;
  internal bool widthchanged;
  private double width;
  private double height;

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(ColorPickerPalette.IconProperty);
    set => this.SetValue(ColorPickerPalette.IconProperty, (object) value);
  }

  public Size IconSize
  {
    get => (Size) this.GetValue(ColorPickerPalette.IconSizeProperty);
    set => this.SetValue(ColorPickerPalette.IconSizeProperty, (object) value);
  }

  public ImageSource MoreColorsIcon
  {
    get => (ImageSource) this.GetValue(ColorPickerPalette.MoreColorsIconProperty);
    set => this.SetValue(ColorPickerPalette.MoreColorsIconProperty, (object) value);
  }

  public Size MoreColorsIconSize
  {
    get => (Size) this.GetValue(ColorPickerPalette.MoreColorsIconSizeProperty);
    set => this.SetValue(ColorPickerPalette.MoreColorsIconSizeProperty, (object) value);
  }

  public ICommand SelectedCommand
  {
    get => (ICommand) this.GetValue(ColorPickerPalette.SelectedCommandProperty);
    set => this.SetValue(ColorPickerPalette.SelectedCommandProperty, (object) value);
  }

  public PickerMode Mode
  {
    get => (PickerMode) this.GetValue(ColorPickerPalette.ModeProperty);
    set => this.SetValue(ColorPickerPalette.ModeProperty, (object) value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ColorPickerPalette.HeaderTemplateProperty);
    set => this.SetValue(ColorPickerPalette.HeaderTemplateProperty, (object) value);
  }

  [Obsolete("This Event is deprecated, please use SelectedBrushChanged property")]
  public event PropertyChangedCallback ColorChanged;

  public event PropertyChangedCallback PopupHeightChanged;

  public Brush AutomaticColor
  {
    get => (Brush) this.GetValue(ColorPickerPalette.AutomaticColorProperty);
    set => this.SetValue(ColorPickerPalette.AutomaticColorProperty, (object) value);
  }

  public bool SetCustomColors
  {
    get => (bool) this.GetValue(ColorPickerPalette.SetCustomColorsProperty);
    set => this.SetValue(ColorPickerPalette.SetCustomColorsProperty, (object) value);
  }

  public ObservableCollection<CustomColor> CustomColorsCollection
  {
    get
    {
      return (ObservableCollection<CustomColor>) this.GetValue(ColorPickerPalette.CustomColorsCollectionProperty);
    }
    set => this.SetValue(ColorPickerPalette.CustomColorsCollectionProperty, (object) value);
  }

  public Color Color
  {
    get => (Color) this.GetValue(ColorPickerPalette.ColorProperty);
    set => this.SetValue(ColorPickerPalette.ColorProperty, (object) value);
  }

  public Brush SelectedBrush
  {
    get => (Brush) this.GetValue(ColorPickerPalette.SelectedBrushProperty);
    set => this.SetValue(ColorPickerPalette.SelectedBrushProperty, (object) value);
  }

  public double PopupWidth
  {
    get => (double) this.GetValue(ColorPickerPalette.PopupWidthProperty);
    set => this.SetValue(ColorPickerPalette.PopupWidthProperty, (object) value);
  }

  public double PopupHeight
  {
    get => (double) this.GetValue(ColorPickerPalette.PopupHeightProperty);
    set => this.SetValue(ColorPickerPalette.PopupHeightProperty, (object) value);
  }

  public double BorderWidth
  {
    get => (double) this.GetValue(ColorPickerPalette.BorderWidthProperty);
    set => this.SetValue(ColorPickerPalette.BorderWidthProperty, (object) value);
  }

  public double BorderHeight
  {
    get => (double) this.GetValue(ColorPickerPalette.BorderHeightProperty);
    set => this.SetValue(ColorPickerPalette.BorderHeightProperty, (object) value);
  }

  public string ColorName
  {
    get => (string) this.GetValue(ColorPickerPalette.ColorNameProperty);
    set => this.SetValue(ColorPickerPalette.ColorNameProperty, (object) value);
  }

  public ColorGroupItem SelectedItem
  {
    get => (ColorGroupItem) this.GetValue(ColorPickerPalette.ColorGroupItemProperty);
    internal set => this.SetValue(ColorPickerPalette.ColorGroupItemProperty, (object) value);
  }

  public Brush ThemeHeaderBackGround
  {
    get => (Brush) this.GetValue(ColorPickerPalette.ThemeHeaderBackGroundProperty);
    set => this.SetValue(ColorPickerPalette.ThemeHeaderBackGroundProperty, (object) value);
  }

  public bool IsAutomaticBorderPressed
  {
    get => (bool) this.GetValue(ColorPickerPalette.IsAutomaticBorderPressedProperty);
    private set
    {
      this.SetValue(ColorPickerPalette.IsAutomaticBorderPressedProperty, (object) value);
    }
  }

  public bool IsMoreColorsBorderPressed
  {
    get => (bool) this.GetValue(ColorPickerPalette.IsMoreColorsBorderPressedProperty);
    private set
    {
      this.SetValue(ColorPickerPalette.IsMoreColorsBorderPressedProperty, (object) value);
    }
  }

  public bool ExpandedAutomaticBorderPressed
  {
    get => (bool) this.GetValue(ColorPickerPalette.ExpandedAutomaticBorderPressedProperty);
    private set
    {
      this.SetValue(ColorPickerPalette.ExpandedAutomaticBorderPressedProperty, (object) value);
    }
  }

  public Brush MouseOverBackground
  {
    get => (Brush) this.GetValue(ColorPickerPalette.MouseOverBackgroundProperty);
    set => this.SetValue(ColorPickerPalette.MouseOverBackgroundProperty, (object) value);
  }

  public bool ExpandedMoreColorsBorderPressed
  {
    get => (bool) this.GetValue(ColorPickerPalette.ExpandedMoreColorsBorderPressedProperty);
    private set
    {
      this.SetValue(ColorPickerPalette.ExpandedMoreColorsBorderPressedProperty, (object) value);
    }
  }

  public Brush ThemeHeaderForeGround
  {
    get => (Brush) this.GetValue(ColorPickerPalette.ThemeHeaderForeGroundProperty);
    set => this.SetValue(ColorPickerPalette.ThemeHeaderForeGroundProperty, (object) value);
  }

  internal PolygonItem SelectedMoreColor
  {
    get => (PolygonItem) this.GetValue(ColorPickerPalette.MoreColorProperty);
    set => this.SetValue(ColorPickerPalette.MoreColorProperty, (object) value);
  }

  public Visibility AutomaticColorVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.AutomaticColorVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.AutomaticColorVisibilityProperty, (object) value);
  }

  public Visibility NoColorVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.NoColorVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.NoColorVisibilityProperty, (object) value);
  }

  public Visibility MoreColorOptionVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.MoreColorOptionVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.MoreColorOptionVisibilityProperty, (object) value);
  }

  public Visibility ThemePanelVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.ThemePanelVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.ThemePanelVisibilityProperty, (object) value);
  }

  public Visibility CustomHeaderVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.CustomHeaderVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.CustomHeaderVisibilityProperty, (object) value);
  }

  public string CustomHeaderText
  {
    get => (string) this.GetValue(ColorPickerPalette.CustomHeaderTextProperty);
    set => this.SetValue(ColorPickerPalette.CustomHeaderTextProperty, (object) value);
  }

  public Visibility IsStandardTabVisible
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.IsStandardTabVisibleProperty);
    set => this.SetValue(ColorPickerPalette.IsStandardTabVisibleProperty, (object) value);
  }

  public Visibility IsCustomTabVisible
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.IsCustomTabVisibleProperty);
    set => this.SetValue(ColorPickerPalette.IsCustomTabVisibleProperty, (object) value);
  }

  public Visibility StandardPanelVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.StandardPanelVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.StandardPanelVisibilityProperty, (object) value);
  }

  public Visibility RecentlyUsedPanelVisibility
  {
    get => (Visibility) this.GetValue(ColorPickerPalette.RecentlyUsedPanelVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.RecentlyUsedPanelVisibilityProperty, (object) value);
  }

  public PaletteTheme Themes
  {
    get => (PaletteTheme) this.GetValue(ColorPickerPalette.ThemesProperty);
    set => this.SetValue(ColorPickerPalette.ThemesProperty, (object) value);
  }

  public BlackWhiteVisible BlackWhiteVisibility
  {
    get => (BlackWhiteVisible) this.GetValue(ColorPickerPalette.BlackWhiteVisibilityProperty);
    set => this.SetValue(ColorPickerPalette.BlackWhiteVisibilityProperty, (object) value);
  }

  public bool GenerateStandardVariants
  {
    get => (bool) this.GetValue(ColorPickerPalette.GenerateStandardVariantsProperty);
    set => this.SetValue(ColorPickerPalette.GenerateStandardVariantsProperty, (object) value);
  }

  public bool GenerateThemeVariants
  {
    get => (bool) this.GetValue(ColorPickerPalette.GenerateThemeVariantsProperty);
    set => this.SetValue(ColorPickerPalette.GenerateThemeVariantsProperty, (object) value);
  }

  public Popup Popup { get; internal set; }

  [Obsolete("This property is deprecated, please use Mode property")]
  public bool IsExpanded
  {
    get => (bool) this.GetValue(ColorPickerPalette.IsExpandedProperty);
    set => this.SetValue(ColorPickerPalette.IsExpandedProperty, (object) value);
  }

  public event ColorPickerPalette.MoreColorWindowEventHandler MoreColorWindowOpened;

  public event ColorPickerPalette.MoreColorWindowEventHandler MoreColorWindowOpening;

  public event EventHandler<SelectedBrushChangedEventArgs> SelectedBrushChanged;

  public ColorPickerPalette()
  {
    this.DefaultStyleKey = (object) typeof (ColorPickerPalette);
    this.SetValue(ColorPickerPalette.CustomColorsCollectionProperty, (object) new ObservableCollection<CustomColor>());
    Mouse.AddPreviewMouseDownOutsideCapturedElementHandler((DependencyObject) this, new MouseButtonEventHandler(this.OnMouseDownOutsideCapturedElement));
    this.Loaded += new RoutedEventHandler(this.ColorPickerPalette_Loaded);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  static ColorPickerPalette() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  public event ColorPickerPalette.DropDownOpenedEventHandler DropDownOpened;

  public override void OnApplyTemplate()
  {
    if (this.automaticborder != null)
    {
      this.automaticborder.MouseLeftButtonUp -= new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonUp);
      this.automaticborder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonDown);
    }
    if (this.updown != null)
      this.updown.MouseLeftButtonDown -= new MouseButtonEventHandler(this.updownMouseLeftButtonDown);
    if (this.colorBorder != null)
      this.colorBorder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.updownMouseLeftButtonDown);
    this.LostFocus -= new RoutedEventHandler(this.ColorPickerPalette_LostFocus);
    if (this.morecolorsborder != null)
    {
      this.morecolorsborder.MouseLeftButtonUp -= new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonUp);
      this.morecolorsborder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonDown);
    }
    base.OnApplyTemplate();
    this.OutBorder = this.GetTemplateChild("ColorPaletteBorder") as Border;
    this.outerGrid = this.GetTemplateChild("OuterGrid") as Grid;
    if (this.item3 != null)
      this.item3.DataSource = (ObservableCollection<ColorGroupItem>) null;
    this.item1 = this.GetTemplateChild("item1") as ColorGroup;
    this.item2 = this.GetTemplateChild("item2") as ColorGroup;
    this.item3 = this.GetTemplateChild("item3") as ColorGroup;
    this.ColorArea = this.GetTemplateChild("ColorArea") as ItemsControl;
    this.automaticborder = this.GetTemplateChild("Automatic1") as Border;
    this.autoColorBorder = this.GetTemplateChild("aborder") as Border;
    this.morecolorsborder = this.GetTemplateChild("MoreColors1") as Border;
    this.Nocolorsborder = this.GetTemplateChild("NoColor") as Border;
    if (this.Nocolorsborder != null)
    {
      this.Nocolorsborder.MouseDown += new MouseButtonEventHandler(this.Nocolorsborder_MouseDown);
      this.Nocolorsborder.MouseUp += new MouseButtonEventHandler(this.Nocolorsborder_MouseUp);
    }
    this.popupBorder = this.GetTemplateChild("b") as Border;
    this.ThemeColors();
    this.StandardColors();
    this.RecentlyUsed();
    if (this.ColorArea != null && !this.ColorArea.Items.Contains((object) this.colorGroup))
    {
      this.colorGroup = new ColorGroup();
      this.colorGroup1 = new ColorGroup();
    }
    if (this.automaticborder != null)
    {
      this.automaticborder.MouseLeftButtonUp += new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonUp);
      this.automaticborder.MouseLeftButtonDown += new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonDown);
    }
    this.Colorgrid = this.GetTemplateChild("lay") as Grid;
    this.Popup = this.GetTemplateChild("pop") as Popup;
    if (this.Popup != null)
      this.Popup.Closed += new EventHandler(this.OnPopupClosed);
    this.updown = this.GetTemplateChild("UpDownBorder") as Border;
    this.LayoutBorder = this.GetTemplateChild("ColorPickerBorder") as Border;
    if (this.updown != null)
      this.updown.MouseLeftButtonDown += new MouseButtonEventHandler(this.updownMouseLeftButtonDown);
    this.colorBorder = this.GetTemplateChild("ColorBorder") as Border;
    if (this.colorBorder != null)
      this.colorBorder.MouseLeftButtonDown += new MouseButtonEventHandler(this.colorBorder_MouseLeftButtonDown);
    this.LostFocus += new RoutedEventHandler(this.ColorPickerPalette_LostFocus);
    if (this.morecolorsborder != null)
    {
      this.morecolorsborder.MouseLeftButtonUp += new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonUp);
      this.morecolorsborder.MouseLeftButtonDown += new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonDown);
    }
    if (this.Mode != PickerMode.Palette)
      return;
    this.Popup.Child = (UIElement) null;
    this.OutBorder.Visibility = Visibility.Collapsed;
    this.outerGrid.Children.Add((UIElement) this.popupBorder);
    this.LoadInExpandedMode(true);
  }

  private void Nocolorsborder_MouseUp(object sender, MouseButtonEventArgs e)
  {
    if (this.Popup == null)
      return;
    this.Popup.IsOpen = false;
  }

  private void Nocolorsborder_MouseDown(object sender, MouseButtonEventArgs e)
  {
    this.Color = Colors.Transparent;
  }

  private void OnPopupClosed(object sender, EventArgs e)
  {
    if (this.IsKeyboardFocusWithin)
    {
      if (this.IsMouseCaptured)
        Mouse.Capture((IInputElement) null);
      this.Focus();
    }
    this.row = 0;
    this.column = 0;
    this.popupItemIndex = -1;
    this.IsChecked = false;
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt && (e.SystemKey == Key.Down || e.SystemKey == Key.Up))
    {
      if (!this.Popup.IsOpen)
      {
        this.OpenPopup();
        this.Popup.IsOpen = true;
      }
      else
        this.Popup.IsOpen = false;
    }
    else if (e.Key == Key.Escape)
    {
      this.Popup.IsOpen = false;
      this.IsChecked = false;
    }
    else
    {
      if (e.Key != Key.Return && e.Key != Key.Space)
        return;
      FrameworkElement focusedElement = Keyboard.FocusedElement as FrameworkElement;
      if (focusedElement.Name == "Automatic1")
        this.SelectAutomaticColor();
      else if (focusedElement.Name == "MoreColors1")
        this.OpenMoreColorsWindow();
      else if (focusedElement.Name == "NoColor")
        this.Color = Colors.Transparent;
      else if (focusedElement.Name == "ItemBorder")
      {
        if (!(focusedElement.TemplatedParent is ColorGroupItem))
          return;
        ColorGroupItem templatedParent = focusedElement.TemplatedParent as ColorGroupItem;
        templatedParent.SelectColor(templatedParent.colorGroupItemBorder);
      }
      else
        this.Popup.IsOpen = false;
    }
  }

  private void morecolorsborder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsMoreColorsBorderPressed = true;
  }

  private void automaticborder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsAutomaticBorderPressed = true;
  }

  private void ColorPickerPalette_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.window != null)
    {
      this.window.MouseDown -= new MouseButtonEventHandler(this.MainWindow_MouseDown);
      this.window.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.window.Deactivated -= new EventHandler(this.window_Deactivated);
    }
    this.Unloaded -= new RoutedEventHandler(this.ColorPickerPalette_Unloaded);
  }

  internal void RaiseCommand()
  {
    if (this.SelectedCommand == null || !this.SelectedCommand.CanExecute((object) true))
      return;
    this.SelectedCommand.Execute((object) new ColorSelectedCommandArgs()
    {
      Brush = new SolidColorBrush(this.Color)
    });
  }

  private void colorBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.Mode == PickerMode.Split)
    {
      this.RaiseCommand();
    }
    else
    {
      if (!this.isPopupclosedOnAutomaticClick && this.Mode != PickerMode.Palette)
        this.OpenPopup();
      this.isPopupclosedOnAutomaticClick = false;
    }
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    try
    {
      return base.MeasureOverride(availableSize);
    }
    catch
    {
      return availableSize;
    }
  }

  protected virtual void OnMoreColorWindowOpening(MoreColorCancelEventArgs e)
  {
    if (this.MoreColorWindowOpening == null)
      return;
    this.MoreColorWindowOpening((object) this, e);
  }

  protected virtual void OnMoreColorWindowOpened(MoreColorCancelEventArgs e)
  {
    if (this.MoreColorWindowOpened == null)
      return;
    this.MoreColorWindowOpened((object) this, e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.Popup != null && this.Popup.IsOpen)
      e.Handled = true;
    base.OnMouseWheel(e);
  }

  private void automaticborder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.SelectAutomaticColor();
    this.IsAutomaticBorderPressed = false;
  }

  private void SelectAutomaticColor()
  {
    this.isPopupclosedOnAutomaticClick = true;
    this.ColorName = "Automatic color";
    this.IsAutomaticSelected = true;
    this.Color = ((SolidColorBrush) this.autoColorBorder.Background).Color;
    this.RaiseCommand();
    this.IsChecked = false;
    this.IsSelected = true;
    if (this.Popup != null)
    {
      this.Popup.IsOpen = false;
      this.isPopupclosedOnAutomaticClick = false;
    }
    if (this.IsAutomaticBorderPressed)
      this.UnwiredEvents();
    if (this.SelectedItem == null)
      return;
    this.SelectedItem.IsSelected = false;
    this.SelectedItem = (ColorGroupItem) null;
  }

  private void morecolorsborder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.isPopupclosedOnAutomaticClick = true;
    this.IsMoreColorsBorderPressed = false;
    MoreColorCancelEventArgs e1 = new MoreColorCancelEventArgs();
    e1.Source = sender;
    this.OnMoreColorWindowOpening(e1);
    if (!e1.Cancel)
    {
      this.OpenMoreColorsWindow();
      this.OnMoreColorWindowOpened(e1);
    }
    this.isPopupclosedOnAutomaticClick = false;
  }

  private void OpenMoreColorsWindow()
  {
    this.child = new MoreColorsWindow();
    this.child.FlowDirection = this.FlowDirection;
    this.child.palette = this;
    if (this.Popup != null)
      this.Popup.IsOpen = false;
    this.UnwiredEvents();
    int num1 = 1;
    this.child.palette = this;
    this.child.Opacity = 1.0;
    this.child.polygonitem = (PolygonItem) null;
    IEnumerable<PolygonItem> polygonItems = this.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => ((SolidColorBrush) more.color).Color.Equals(this.Color)));
    int num2 = 0;
    foreach (PolygonItem polygonItem in polygonItems)
    {
      this.SelectedMoreColor = polygonItem;
      this.child.polygonitem = polygonItem;
      if (num2 == 0)
      {
        this.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.child.path.Data = (Geometry) this.child.polygonitem.DrawPath(this.child.polygonitem.Points);
        num2 = 1;
      }
      else
      {
        this.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.child.path1.Data = (Geometry) this.child.polygonitem.DrawPath(this.child.polygonitem.Points);
      }
      Binding binding = new Binding();
      binding.Source = (object) this.SelectedMoreColor.color;
      binding.Mode = BindingMode.OneWay;
      this.child.Current.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
      this.child.New.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
      num1 = 0;
    }
    if (num1 == 1)
    {
      this.child.polygonitem = (PolygonItem) null;
      this.SelectedMoreColor = (PolygonItem) null;
      this.child.tab.SelectedIndex = 1;
      this.child.path.Data = (Geometry) null;
      this.child.path1.Data = (Geometry) null;
      this.child.Current.Background = (Brush) new SolidColorBrush(this.Color);
      this.child.New.Background = (Brush) new SolidColorBrush(this.Color);
    }
    else
      this.child.tab.SelectedIndex = 0;
    if (this.child.tab.SelectedIndex == 0 && this.IsStandardTabVisible != Visibility.Visible)
      this.child.tab.SelectedIndex = 1;
    if (this.child.tab.SelectedIndex == 1 && this.IsCustomTabVisible != Visibility.Visible)
      this.child.tab.SelectedIndex = 0;
    if (this.SelectedMoreColor != null && ((SolidColorBrush) this.SelectedMoreColor.color).Color != Colors.White)
      this.child.path1.Data = (Geometry) null;
    this.child.asd.Brush = (Brush) new SolidColorBrush(this.Color);
    this.IsChecked = false;
    this.child.custompanel.Visibility = this.IsCustomTabVisible;
    this.child.custom.Visibility = this.IsCustomTabVisible;
    this.child.standard.Visibility = this.IsStandardTabVisible;
    this.child.standardPanel.Visibility = this.IsStandardTabVisible;
    this.child.WindowGrid.Height = 350.0;
    this.child.hyp = 10.0;
    this.child.x = 90.0;
    this.child.y = 20.0;
    this.child.WindowGrid.Width = 424.0;
    this.themestyle = SkinStorage.GetVisualStyle((DependencyObject) this);
    if (this.themestyle != "")
      SkinStorage.SetVisualStyle((DependencyObject) this.child, this.themestyle);
    SfSkinManagerExtension.SetTheme((DependencyObject) this, (DependencyObject) this.child);
    this.child.ShowActivated = true;
    this.child.ShowInTaskbar = false;
    this.child.Owner = Window.GetWindow((DependencyObject) this);
    this.child.ShowDialog();
  }

  private void ColorPickerPalette_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.Popup != null && !this.Popup.IsKeyboardFocusWithin)
      this.Popup.IsOpen = false;
    this.UnwiredEvents();
  }

  protected override void OnGotFocus(RoutedEventArgs e) => base.OnGotFocus(e);

  private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
  {
    this.Popup.IsOpen = false;
    if (!this.IsMouseCaptured)
      return;
    Mouse.Capture((IInputElement) null);
  }

  private static void OnModeChanged(
    DependencyObject target,
    DependencyPropertyChangedEventArgs args)
  {
    ColorPickerPalette colorPickerPalette = target as ColorPickerPalette;
    if (!colorPickerPalette.IsLoaded)
      return;
    if (colorPickerPalette.Popup != null && colorPickerPalette.OutBorder != null && colorPickerPalette.outerGrid != null)
    {
      if (colorPickerPalette.Mode == PickerMode.Palette)
      {
        colorPickerPalette.Popup.Child = (UIElement) null;
        colorPickerPalette.OutBorder.Visibility = Visibility.Collapsed;
        colorPickerPalette.outerGrid.Children.Add((UIElement) colorPickerPalette.popupBorder);
      }
      else
      {
        colorPickerPalette.outerGrid.Children.Remove((UIElement) colorPickerPalette.popupBorder);
        colorPickerPalette.OutBorder.Visibility = Visibility.Visible;
        colorPickerPalette.Popup.Child = (UIElement) colorPickerPalette.popupBorder;
      }
    }
    colorPickerPalette.LoadInExpandedMode(colorPickerPalette.Mode == PickerMode.Palette);
  }

  private static void OnIsExpandedChanged(
    DependencyObject target,
    DependencyPropertyChangedEventArgs args)
  {
    ColorPickerPalette colorPickerPalette = target as ColorPickerPalette;
    if ((bool) args.NewValue)
      colorPickerPalette.Mode = PickerMode.Palette;
    else
      colorPickerPalette.Mode = PickerMode.DropDown;
  }

  private void e_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.Loadfinished)
    {
      this.click = false;
      this.Loadfinished = true;
    }
    if (this.Popup.IsOpen && !this.click)
    {
      this.Popup.IsOpen = false;
      this.UnwiredEvents();
      this.IsChecked = false;
      this.updownclick = false;
      e.Handled = true;
    }
    if (!this.click)
      return;
    this.click = false;
    e.Handled = true;
  }

  internal void UnwiredEvents()
  {
    FrameworkElement parent1 = this.Parent as FrameworkElement;
    FrameworkElement parent2 = this.Parent as FrameworkElement;
    while (parent1 != null)
    {
      parent1.MouseLeftButtonDown -= new MouseButtonEventHandler(this.e_MouseLeftButtonDown);
      parent1 = VisualTreeHelper.GetParent((DependencyObject) parent1) as FrameworkElement;
      this.Loadfinished = false;
    }
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    BindingOperations.ClearAllBindings((DependencyObject) this);
    this.UnwiredEvents();
    this.Loaded -= new RoutedEventHandler(this.ColorPickerPalette_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.ColorPickerPalette_Unloaded);
    if (this.automaticborder != null)
    {
      this.automaticborder.MouseLeftButtonUp -= new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonUp);
      this.automaticborder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.automaticborder_MouseLeftButtonDown);
    }
    if (this.updown != null)
      this.updown.MouseLeftButtonDown -= new MouseButtonEventHandler(this.updownMouseLeftButtonDown);
    if (this.colorBorder != null)
      this.colorBorder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.colorBorder_MouseLeftButtonDown);
    if (this.morecolorsborder != null)
    {
      this.morecolorsborder.MouseLeftButtonUp -= new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonUp);
      this.morecolorsborder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.morecolorsborder_MouseLeftButtonDown);
    }
    if (this.window != null)
    {
      this.window.MouseDown -= new MouseButtonEventHandler(this.MainWindow_MouseDown);
      this.window.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.window.Deactivated -= new EventHandler(this.window_Deactivated);
      this.window = (Window) null;
    }
    if (this.child != null)
      this.child = (MoreColorsWindow) null;
    if (this.Nocolorsborder != null)
    {
      this.Nocolorsborder.MouseDown -= new MouseButtonEventHandler(this.Nocolorsborder_MouseDown);
      this.Nocolorsborder.MouseUp -= new MouseButtonEventHandler(this.Nocolorsborder_MouseUp);
      this.Nocolorsborder = (Border) null;
    }
    if (this.morecolorsborder != null)
      this.morecolorsborder = (Border) null;
    if (this.automaticborder != null)
      this.automaticborder = (Border) null;
    if (this.autoColorBorder != null)
      this.autoColorBorder = (Border) null;
    if (this.item1 != null)
    {
      this.item1.Dispose();
      this.item1 = (ColorGroup) null;
    }
    if (this.item2 != null)
    {
      this.item2.Dispose();
      this.item2 = (ColorGroup) null;
    }
    if (this.item3 != null)
    {
      this.item3.Dispose();
      this.item3 = (ColorGroup) null;
    }
    if (this.updown != null)
      this.updown = (Border) null;
    if (this.colorBorder != null)
      this.colorBorder = (Border) null;
    if (this.popupBorder != null)
      this.popupBorder = (Border) null;
    if (this.OutBorder != null)
      this.OutBorder = (Border) null;
    if (this.LayoutBorder != null)
      this.LayoutBorder = (Border) null;
    if (this.ColorArea != null)
    {
      for (int index = 0; index < this.ColorArea.Items.Count; ++index)
      {
        if (this.ColorArea.Items[index] is ColorGroup)
          (this.ColorArea.Items[index] as ColorGroup).Dispose();
      }
      this.ColorArea.ItemsSource = (IEnumerable) null;
      if (this.ColorArea.ItemsSource == null)
        this.ColorArea.Items.Clear();
      this.ColorArea = (ItemsControl) null;
    }
    if (this.col != null)
    {
      for (int index1 = 0; index1 < ((IEnumerable<ObservableCollection<ColorGroupItem>>) this.col).Count<ObservableCollection<ColorGroupItem>>(); ++index1)
      {
        if (this.col[index1] != null && this.col[index1] != null)
        {
          for (int index2 = 0; index2 < this.col[index1].Count<ColorGroupItem>(); ++index2)
            this.col[index1][index2].Dispose();
          this.col[index1].Clear();
          this.col[index1] = (ObservableCollection<ColorGroupItem>) null;
        }
      }
      this.col = (ObservableCollection<ColorGroupItem>[]) null;
    }
    if (this.CustomCol != null)
    {
      for (int index3 = 0; index3 < ((IEnumerable<ObservableCollection<ColorGroupItem>>) this.CustomCol).Count<ObservableCollection<ColorGroupItem>>(); ++index3)
      {
        if (this.CustomCol[index3] != null && this.CustomCol[index3] != null)
        {
          for (int index4 = 0; index4 < this.CustomCol[index3].Count<ColorGroupItem>(); ++index4)
            this.CustomCol[index3][index4].Dispose();
          this.CustomCol[index3].Clear();
          this.CustomCol[index3] = (ObservableCollection<ColorGroupItem>) null;
        }
      }
      this.CustomCol = (ObservableCollection<ColorGroupItem>[]) null;
    }
    if (this.ColorGroupCollection != null)
    {
      for (int index = 0; index < this.ColorGroupCollection.Count<ColorGroupItem>(); ++index)
        this.ColorGroupCollection[index].Dispose();
      this.ColorGroupCollection.Clear();
      this.ColorGroupCollection = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.CustomColorGroupCollection != null)
    {
      for (int index = 0; index < this.CustomColorGroupCollection.Count<ColorGroupItem>(); ++index)
        this.CustomColorGroupCollection[index].Dispose();
      this.CustomColorGroupCollection.Clear();
      this.CustomColorGroupCollection = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.RecentlyUsedCollection != null)
    {
      for (int index = 0; index < this.RecentlyUsedCollection.Count<ColorGroupItem>(); ++index)
        this.RecentlyUsedCollection[index].Dispose();
      this.RecentlyUsedCollection.Clear();
      this.RecentlyUsedCollection = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.StdColorCollection != null)
    {
      for (int index = 0; index < this.StdColorCollection.Count<ColorGroupItem>(); ++index)
        this.StdColorCollection[index].Dispose();
      this.StdColorCollection.Clear();
      this.StdColorCollection = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.CustomColorGroupCollection != null)
    {
      for (int index = 0; index < this.CustomColorGroupCollection.Count<ColorGroupItem>(); ++index)
        this.CustomColorGroupCollection[index].Dispose();
      this.CustomColorGroupCollection.Clear();
      this.CustomColorGroupCollection = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.CustomColorsCollection != null)
    {
      for (int index = 0; index < this.CustomColorsCollection.Count<CustomColor>(); ++index)
        this.CustomColorsCollection[index].ColorName = (string) null;
      this.CustomColorsCollection.Clear();
      this.CustomColorsCollection = (ObservableCollection<CustomColor>) null;
    }
    if (this.themestyle != null)
      this.themestyle = (string) null;
    if (this.Colorgrid != null)
      this.Colorgrid = (Grid) null;
    if (this.colorGroup != null)
    {
      this.colorGroup.Dispose();
      this.colorGroup = (ColorGroup) null;
    }
    if (this.colorGroup1 != null)
    {
      this.colorGroup1.Dispose();
      this.colorGroup1 = (ColorGroup) null;
    }
    if (this.Popup == null)
      return;
    this.Popup.Closed -= new EventHandler(this.OnPopupClosed);
    this.Popup = (Popup) null;
  }

  public void Dispose() => this.Dispose(true);

  private void RootVisual_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.updownclick)
    {
      this.Popup.IsOpen = false;
      this.UnwiredEvents();
      this.IsChecked = false;
    }
    this.updownclick = false;
  }

  private void ColorBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.Popup.IsOpen = false;
    this.UnwiredEvents();
    this.IsChecked = false;
  }

  private void LoadInExpandedMode(bool isexpanded)
  {
    double num = 180.0;
    if (SkinStorage.GetEnableTouch((DependencyObject) this))
    {
      num = 300.0;
      this.PopupWidth = 300.0;
    }
    if (!double.IsNaN(this.Width))
      num = this.Width;
    if (isexpanded)
    {
      if (this.LayoutBorder != null)
        this.width = this.BlackWhiteVisibility != BlackWhiteVisible.None ? (this.BlackWhiteVisibility != BlackWhiteVisible.Both ? Math.Floor((num - 38.0) / 9.0) : Math.Floor((num - 42.0) / 10.0)) : Math.Floor((num - 34.0) / 8.0);
    }
    else if (this.LayoutBorder != null)
      this.width = this.BlackWhiteVisibility != BlackWhiteVisible.None ? (this.BlackWhiteVisibility != BlackWhiteVisible.Both ? Math.Floor((this.PopupWidth - 38.0) / 9.0) : Math.Floor((this.PopupWidth - 42.0) / 10.0)) : Math.Floor((this.PopupWidth - 34.0) / 8.0);
    this.width = this.width < 0.0 ? 0.0 : this.width;
    this.width = this.width % 2.0 == 0.0 ? this.width : this.width - 1.0;
    this.RefWidth = this.width;
    this.height = this.width;
    this.BorderHeight = this.BorderHeight == 0.0 ? this.height : this.BorderHeight;
    this.BorderWidth = this.BorderWidth == 0.0 ? this.width : this.BorderWidth;
    this.ThemeColors();
    this.StandardColors();
    this.RecentlyUsed();
    if (!this.SetCustomColors)
      return;
    this.LoadCustomColors();
  }

  private void updownMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.isPopupclosedOnAutomaticClick && this.Mode != PickerMode.Palette)
      this.OpenPopup();
    this.isPopupclosedOnAutomaticClick = false;
  }

  private void OpenPopup()
  {
    this.Focus();
    Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    this.updownclick = true;
    if (this.Popup == null)
      return;
    if (this.width != this.RefWidth)
    {
      this.width = this.RefWidth;
      this.widthchanged = true;
    }
    else
      this.widthchanged = false;
    this.IsChecked = !this.IsChecked;
    if (this.IsChecked)
    {
      if (this.Mode != PickerMode.Palette)
        this.Popup.IsOpen = true;
      Size size = new Size(180.0, 200.0);
      this.Popup.Margin = new Thickness(0.0, this.OutBorder.ActualHeight, 0.0, 0.0);
      if (SkinStorage.GetEnableTouch((DependencyObject) this))
        this.PopupWidth = 300.0;
      if (this.width == 0.0)
      {
        if (this.LayoutBorder != null)
          this.width = this.BlackWhiteVisibility != BlackWhiteVisible.None ? (this.BlackWhiteVisibility != BlackWhiteVisible.Both ? Math.Floor((this.PopupWidth - 38.0) / 9.0) : Math.Floor((this.PopupWidth - 42.0) / 10.0)) : Math.Floor((this.PopupWidth - 34.0) / 8.0);
        this.width = this.width < 0.0 ? 0.0 : this.width;
        this.width = this.width % 2.0 == 0.0 ? this.width : this.width - 1.0;
        this.RefWidth = this.width;
        this.height = this.width;
        this.BorderHeight = this.BorderHeight == 0.0 ? this.height : this.BorderHeight;
        this.BorderWidth = this.BorderWidth == 0.0 ? this.width : this.BorderWidth;
        this.ThemeColors();
        this.StandardColors();
        this.RecentlyUsed();
      }
      else if (this.widthchanged)
      {
        this.height = this.width;
        this.ThemeColors();
        this.StandardColors();
        this.RecentlyUsed();
      }
      if (this.SetCustomColors)
        this.LoadCustomColors();
      if (this.Mode != PickerMode.Palette)
        this.Popup.IsOpen = true;
      this.click = true;
      this.Popup.Placement = PlacementMode.Bottom;
      if (this.SelectedItem != null && this.Mode != PickerMode.DropDown)
        this.SelectedItem.FocusBorder();
      else if (this.AutomaticColorVisibility == Visibility.Visible)
        Keyboard.Focus((IInputElement) this.automaticborder);
      else if (this.ThemePanelVisibility == Visibility.Visible)
      {
        if (!this.item1.DataSource[0].IsLoaded)
          this.item1.DataSource[0].setFocus = true;
        else
          Keyboard.Focus((IInputElement) this.item1.DataSource[0].colorGroupItemBorder);
      }
      else if (this.StandardPanelVisibility == Visibility.Visible)
      {
        if (!this.item2.DataSource[0].IsLoaded)
          this.item2.DataSource[0].setFocus = true;
        else
          Keyboard.Focus((IInputElement) this.item2.DataSource[0].colorGroupItemBorder);
      }
      else if (this.RecentlyUsedPanelVisibility == Visibility.Visible && this.RecentlyUsedCollection.Count > 0)
      {
        if (!this.item3.DataSource[0].IsLoaded)
          this.item3.DataSource[0].setFocus = true;
        else
          Keyboard.Focus((IInputElement) this.item3.DataSource[0].colorGroupItemBorder);
      }
      else if (this.SetCustomColors && this.CustomColorsCollection.Count > 0)
      {
        if (!this.colorGroup.DataSource[0].IsLoaded)
          this.colorGroup.DataSource[0].setFocus = true;
        else
          Keyboard.Focus((IInputElement) this.colorGroup.DataSource[0]);
      }
      else if (this.NoColorVisibility == Visibility.Visible)
        Keyboard.Focus((IInputElement) this.Nocolorsborder);
      else if (this.MoreColorOptionVisibility == Visibility.Visible)
        Keyboard.Focus((IInputElement) this.morecolorsborder);
    }
    else
    {
      this.Popup.IsOpen = false;
      this.IsChecked = false;
      this.UnwiredEvents();
    }
    if (this.DropDownOpened == null)
      return;
    this.DropDownOpened((object) this, new RoutedEventArgs());
  }

  private void LoadCustomColors()
  {
    if (this.CustomColorsCollection == null || this.ColorArea == null || this.ColorArea.Items.Contains((object) this.colorGroup))
      return;
    ObservableCollection<ColorGroupItem> observableCollection1 = new ObservableCollection<ColorGroupItem>();
    ObservableCollection<ColorGroupItem> observableCollection2 = new ObservableCollection<ColorGroupItem>();
    ColorGroup newItem = new ColorGroup();
    ColorGroup colorGroup = new ColorGroup();
    newItem.HeaderName = this.CustomHeaderText;
    newItem.HeaderVisibility = this.CustomHeaderVisibility;
    colorGroup.HeaderName = this.CustomHeaderText;
    colorGroup.HeaderVisibility = this.CustomHeaderVisibility;
label_2:
    foreach (object removeItem in (IEnumerable) this.ColorArea.Items)
    {
      if ((removeItem as ColorGroup).HeaderName == "CustomColors" || (removeItem as ColorGroup).HeaderName == this.CustomHeaderText)
      {
        this.ColorArea.Items.Remove(removeItem);
        goto label_2;
      }
    }
    for (int index = 0; index < this.CustomColorsCollection.Count; ++index)
    {
      if (index != 0 && index % 8 == 0)
      {
        newItem.DataSource = observableCollection1;
        newItem.colorpicker = this;
        this.colorGroup.DataSource = observableCollection1;
        if (!this.ColorArea.Items.Contains((object) newItem))
          this.ColorArea.Items.Add((object) newItem);
        newItem = new ColorGroup();
        newItem.HeaderVisibility = Visibility.Collapsed;
        newItem.HeaderName = "CustomColors";
        observableCollection1 = new ObservableCollection<ColorGroupItem>();
        colorGroup.DataSource = observableCollection2;
        colorGroup.colorpicker = this;
        colorGroup = new ColorGroup();
        colorGroup.HeaderVisibility = Visibility.Collapsed;
        colorGroup.HeaderName = "CustomColors";
        observableCollection2 = new ObservableCollection<ColorGroupItem>();
      }
      observableCollection1.Add(new ColorGroupItem()
      {
        ColorName = this.CustomColorsCollection[index].ColorName,
        Color = (Brush) new SolidColorBrush(this.CustomColorsCollection[index].Color),
        Variants = false,
        BorderMargin = new Thickness(2.0, 1.0, 2.0, 1.0),
        ItemMargin = new Thickness(2.0, 1.0, 2.0, 1.0)
      });
      observableCollection2.Add(new ColorGroupItem()
      {
        ColorName = this.CustomColorsCollection[index].ColorName,
        Color = (Brush) new SolidColorBrush(this.CustomColorsCollection[index].Color),
        Variants = false,
        BorderMargin = new Thickness(2.0, 1.0, 2.0, 1.0),
        ItemMargin = new Thickness(2.0, 1.0, 2.0, 1.0)
      });
    }
    newItem.DataSource = observableCollection1;
    this.colorGroup1.DataSource = observableCollection1;
    newItem.colorpicker = this;
    ObservableCollection<ColorGroup> observableCollection3 = new ObservableCollection<ColorGroup>(this.ColorArea.Items.OfType<ColorGroup>().Where<ColorGroup>((Func<ColorGroup, bool>) (p => p.HeaderName == "Custom Color" || p.HeaderName == this.CustomHeaderText)));
    if (observableCollection3.Count > 0 && observableCollection3[0].DataSource.Count != newItem.DataSource.Count)
      this.ColorArea.Items.Add((object) newItem);
    if (this.ColorArea.Items.OfType<ColorGroup>().Count<ColorGroup>((Func<ColorGroup, bool>) (p => (p.HeaderName == "Custom Color" || p.HeaderName == this.CustomHeaderText) && p.HeaderVisibility == Visibility.Visible)) == 0)
      this.ColorArea.Items.Add((object) newItem);
    colorGroup.DataSource = observableCollection2;
    colorGroup.colorpicker = this;
  }

  public void LoadStandardColors(bool variant)
  {
    this.StdColorCollection = new ObservableCollection<ColorGroupItem>();
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red"),
      Color = (Brush) new SolidColorBrush(Colors.Red),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green"),
      Color = (Brush) new SolidColorBrush(Colors.Green),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow"),
      Color = (Brush) new SolidColorBrush(Colors.Yellow),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue"),
      Color = (Brush) new SolidColorBrush(Colors.Blue),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown"),
      Color = (Brush) new SolidColorBrush(Colors.Brown),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange"),
      Color = (Brush) new SolidColorBrush(Colors.Orange),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple"),
      Color = (Brush) new SolidColorBrush(Colors.Purple),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Sky")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 176 /*0xB0*/, (byte) 240 /*0xF0*/)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "White"),
      Color = (Brush) new SolidColorBrush(Colors.White),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.StdColorCollection.Add(new ColorGroupItem()
    {
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black"),
      Color = (Brush) new SolidColorBrush(Colors.Black),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
  }

  public void LoadThemeColors(bool variant)
  {
    if (this.col == null)
      return;
    this.col[0] = new ObservableCollection<ColorGroupItem>();
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 238, (byte) 236, (byte) 225)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 31 /*0x1F*/, (byte) 73, (byte) 125)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 79, (byte) 129, (byte) 189)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 192 /*0xC0*/, (byte) 80 /*0x50*/, (byte) 77)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "OliveGreen")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 155, (byte) 187, (byte) 89)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 100, (byte) 162)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Aqua")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 75, (byte) 172, (byte) 198)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[0].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 247, (byte) 150, (byte) 70)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1] = new ObservableCollection<ColorGroupItem>();
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "White")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 248, (byte) 248)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 10, (byte) 10, (byte) 10)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-25%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 221, (byte) 221)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-25%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 178, (byte) 178, (byte) 178)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 150, (byte) 150, (byte) 150)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-80%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 95, (byte) 95, (byte) 95)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[1].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-80%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 77, (byte) 77, (byte) 77)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2] = new ObservableCollection<ColorGroupItem>();
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 201, (byte) 194, (byte) 209)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 105, (byte) 103, (byte) 109)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 206, (byte) 185, (byte) 102)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "OliveGreen")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 156, (byte) 176 /*0xB0*/, (byte) 132)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Aqua")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 107, (byte) 177, (byte) 201)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 101, (byte) 133, (byte) 207)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 126, (byte) 107, (byte) 201)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[2].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 163, (byte) 121, (byte) 187)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3] = new ObservableCollection<ColorGroupItem>();
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 227, (byte) 222, (byte) 209)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-80%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 50, (byte) 50, (byte) 50)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 240 /*0xF0*/, (byte) 127 /*0x7F*/, (byte) 9)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 159, (byte) 41, (byte) 54)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 88, (byte) 124)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 78, (byte) 133, (byte) 66)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 96 /*0x60*/, (byte) 72, (byte) 120)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[3].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 193, (byte) 152, (byte) 89)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4] = new ObservableCollection<ColorGroupItem>();
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Ice")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 197, (byte) 209, (byte) 215)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}-{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 107, (byte) 134)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 209, (byte) 99, (byte) 73)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 180, (byte) 0)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 140, (byte) 173, (byte) 174)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 140, (byte) 123, (byte) 112 /*0x70*/)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 143, (byte) 176 /*0xB0*/, (byte) 140)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[4].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 209, (byte) 144 /*0x90*/, (byte) 73)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5] = new ObservableCollection<ColorGroupItem>();
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 233, (byte) 229, (byte) 220)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 105, (byte) 100, (byte) 100)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 72, (byte) 23)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 155, (byte) 45, (byte) 31 /*0x1F*/)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 162, (byte) 142, (byte) 106)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 149, (byte) 98, (byte) 81)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 145, (byte) 132, (byte) 133)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[5].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 133, (byte) 93, (byte) 93)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6] = new ObservableCollection<ColorGroupItem>();
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 245, (byte) 249)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 4, (byte) 97, (byte) 123)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 15, (byte) 111, (byte) 198)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 157, (byte) 217)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 11, (byte) 208 /*0xD0*/, (byte) 217)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BrightText")}{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 16 /*0x10*/, (byte) 207, (byte) 155)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 124, (byte) 202, (byte) 98)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[6].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lime")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 165, (byte) 194, (byte) 73)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7] = new ObservableCollection<ColorGroupItem>();
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 234, (byte) 235, (byte) 222)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "OliveGreen")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 103, (byte) 106, (byte) 85)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 114, (byte) 163, (byte) 118)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")}{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 176 /*0xB0*/, (byte) 204, (byte) 176 /*0xB0*/)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Sky")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 168, (byte) 205, (byte) 215)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 192 /*0xC0*/, (byte) 190, (byte) 175)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 206, (byte) 197, (byte) 151)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[7].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Rose")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 232, (byte) 183, (byte) 183)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8] = new ObservableCollection<ColorGroupItem>();
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 235, (byte) 221, (byte) 195)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 119, (byte) 95, (byte) 85)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Ice")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 148, (byte) 182, (byte) 210)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 128 /*0x80*/, (byte) 71)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "OliveGreen")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 165, (byte) 171, (byte) 129)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gold")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 216, (byte) 178, (byte) 92)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 123, (byte) 167, (byte) 157)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[8].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}-50%, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 150, (byte) 140, (byte) 140)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9] = new ObservableCollection<ColorGroupItem>();
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "BackgroundText")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 214, (byte) 236, byte.MaxValue)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}-{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "TextName")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 78, (byte) 91, (byte) 111)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 1",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 127 /*0x7F*/, (byte) 209, (byte) 59)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 2",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 234, (byte) 21, (byte) 122)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gold")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 3",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 184, (byte) 10)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 4",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 173, (byte) 220)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Periwinkle")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 5",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 115, (byte) 138, (byte) 200)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
    this.col[9].Add(new ColorGroupItem()
    {
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Accent")} 6",
      Color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 26, (byte) 179, (byte) 159)),
      Variants = variant,
      BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
      ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
    });
  }

  private void ThemeColors()
  {
    if (this.item1 == null)
      return;
    this.item1.colorpicker = this;
    this.LoadThemeColors(this.GenerateThemeVariants);
    if (this.col != null)
      this.ColorGroupCollection = this.col[(int) this.Themes];
    this.item1.DataSource = this.ColorGroupCollection;
    if (this.BlackWhiteVisibility == BlackWhiteVisible.Both)
    {
      this.ColorGroupCollection.Insert(0, new ColorGroupItem()
      {
        ColorName = "White, Background 1",
        Color = (Brush) new SolidColorBrush(Colors.White),
        Variants = this.GenerateThemeVariants,
        BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
        ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
      });
      this.ColorGroupCollection.Insert(1, new ColorGroupItem()
      {
        ColorName = "Black, Text 1",
        Color = (Brush) new SolidColorBrush(Colors.Black),
        Variants = this.GenerateThemeVariants,
        BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
        ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
      });
    }
    else if (this.BlackWhiteVisibility == BlackWhiteVisible.White)
      this.ColorGroupCollection.Insert(0, new ColorGroupItem()
      {
        ColorName = "White, Background 1",
        Color = (Brush) new SolidColorBrush(Colors.White),
        Variants = this.GenerateThemeVariants,
        BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
        ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
      });
    else if (this.BlackWhiteVisibility == BlackWhiteVisible.Black)
      this.ColorGroupCollection.Insert(0, new ColorGroupItem()
      {
        ColorName = "Black, Text 1",
        Color = (Brush) new SolidColorBrush(Colors.Black),
        Variants = this.GenerateThemeVariants,
        BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
        ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
      });
    this.item1.PanelVisibility = this.ThemePanelVisibility;
  }

  private void StandardColors()
  {
    if (this.item2 == null)
      return;
    this.item2.colorpicker = this;
    this.LoadStandardColors(this.GenerateStandardVariants);
    this.ColorGroupCollection = this.StdColorCollection;
    if (this.BlackWhiteVisibility == BlackWhiteVisible.Black || this.BlackWhiteVisibility == BlackWhiteVisible.White)
      this.ColorGroupCollection.RemoveAt(this.ColorGroupCollection.Count - 1);
    if (this.BlackWhiteVisibility == BlackWhiteVisible.None)
    {
      this.ColorGroupCollection.RemoveAt(this.ColorGroupCollection.Count - 1);
      this.ColorGroupCollection.RemoveAt(this.ColorGroupCollection.Count - 1);
    }
    this.item2.DataSource = this.ColorGroupCollection;
    this.item2.PanelVisibility = this.StandardPanelVisibility;
  }

  private void RecentlyUsed()
  {
    if (this.item3 == null)
      return;
    this.item3.colorpicker = this;
    this.item3.DataSource = this.RecentlyUsedCollection;
    this.item3.PanelVisibility = this.RecentlyUsedPanelVisibility;
    if (this.item3.PanelVisibility != Visibility.Visible || this.RecentlyUsedCollection == null || this.RecentlyUsedCollection.Count != 0)
      return;
    this.item3.PanelVisibility = Visibility.Collapsed;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    e.Handled = true;
  }

  private void ColorPickerPalette_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.Color.Equals(Colors.Black))
    {
      this.ColorName = "Automatic Color";
      this.Color = ((SolidColorBrush) this.AutomaticColor).Color;
      this.IsSelected = true;
      if (this.col == null)
        this.col = new ObservableCollection<ColorGroupItem>[16 /*0x10*/];
    }
    if (this.ColorName.Equals("Color") || this.IsSelected)
      this.Isloaded = true;
    this.Unloaded += new RoutedEventHandler(this.ColorPickerPalette_Unloaded);
    this.window = VisualUtils.FindAncestor((Visual) this, typeof (Window)) as Window;
    if (this.window == null)
      return;
    this.window.MouseDown += new MouseButtonEventHandler(this.MainWindow_MouseDown);
    this.window.LocationChanged += new EventHandler(this.MainWindow_LocationChanged);
    this.window.Deactivated += new EventHandler(this.window_Deactivated);
  }

  private void window_Deactivated(object sender, EventArgs e)
  {
    if (this.Popup == null || !this.Popup.IsOpen || this.Mode == PickerMode.Palette)
      return;
    this.Popup.IsOpen = false;
  }

  private void MainWindow_LocationChanged(object sender, EventArgs e)
  {
    if (this.Popup == null || !this.Popup.IsOpen || this.Mode == PickerMode.Palette)
      return;
    this.Popup.IsOpen = false;
  }

  private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.Popup == null || !this.Popup.IsOpen || this.Mode == PickerMode.Palette)
      return;
    this.Popup.IsOpen = false;
    this.IsChecked = false;
  }

  private static void IsAutomaticColorChanged(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).IsAutomaticColorChanged(e);
  }

  protected virtual void IsAutomaticColorChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void IsColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).IsColorChanged(e);
  }

  private static void OnBrushChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ColorPickerPalette sender = (ColorPickerPalette) o;
    if (e.NewValue is SolidColorBrush && sender.Color != (e.NewValue as SolidColorBrush).Color)
    {
      sender.IsSelected = false;
      sender.Color = (e.NewValue as SolidColorBrush).Color;
    }
    if (sender.SelectedBrushChanged == null)
      return;
    sender.SelectedBrushChanged((object) sender, new SelectedBrushChangedEventArgs()
    {
      NewBrush = e.NewValue as Brush,
      OldBrush = e.OldValue as Brush,
      NewColor = (e.NewValue as SolidColorBrush).Color,
      OldColor = (e.OldValue as SolidColorBrush).Color
    });
  }

  protected virtual void IsColorChanged(DependencyPropertyChangedEventArgs e)
  {
    bool flag = false;
    foreach (ColorGroupItem recentlyUsed in (Collection<ColorGroupItem>) this.RecentlyUsedCollection)
    {
      if (((SolidColorBrush) recentlyUsed.Color).Color == this.Color)
      {
        flag = true;
        break;
      }
    }
    if (!flag && !this.IsSelected && this.Color != Colors.Transparent && this.Color != ((SolidColorBrush) this.AutomaticColor).Color)
    {
      if (this.RecentlyUsedCollection.Count == 8)
        this.RecentlyUsedCollection.RemoveAt(0);
      this.IsSelected = false;
      if (this.Isloaded)
      {
        if (this.RecentlyUsedCollection.Count > 0)
        {
          ColorGroupItem recentlyUsed = this.RecentlyUsedCollection[0];
        }
        if (this.RecentlyUsedCollection.Count == 8)
          this.RecentlyUsedCollection.RemoveAt(0);
        if (this.RecentlyUsedCollection.Count < 8)
        {
          this.RecentlyUsedCollection.Add(new ColorGroupItem()
          {
            ColorName = this.ColorName,
            Color = (Brush) new SolidColorBrush(this.Color),
            Variants = false,
            BorderMargin = new Thickness(2.0, 2.0, 2.0, 2.0),
            ItemMargin = new Thickness(2.0, 2.0, 2.0, 2.0)
          });
          if (this.item3 != null && this.RecentlyUsedCollection != null && this.RecentlyUsedCollection.Count > 0)
            this.item3.PanelVisibility = this.RecentlyUsedPanelVisibility;
        }
      }
      if (!this.Isloaded)
        this.Isloaded = true;
    }
    if (flag && !this.IsSelected && this.Color != ((SolidColorBrush) this.AutomaticColor).Color)
      this.IsSelected = false;
    Color color = this.Color;
    if (this.Color.ToString() != this.SelectedBrush.ToString())
      this.SelectedBrush = (Brush) new SolidColorBrush(this.Color);
    if (this.ColorChanged == null)
      return;
    this.ColorChanged((DependencyObject) this, e);
  }

  private static void standard_visibility_changed(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ColorPickerPalette colorPickerPalette = (ColorPickerPalette) o;
    if (colorPickerPalette.item2 == null)
      return;
    colorPickerPalette.item2.PanelVisibility = colorPickerPalette.StandardPanelVisibility;
    if (colorPickerPalette.Mode != PickerMode.Palette)
      return;
    colorPickerPalette.StandardColors();
  }

  private static void ThemeVariantsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).ThemeVariantsChanged(e);
  }

  protected virtual void ThemeVariantsChanged(DependencyPropertyChangedEventArgs e)
  {
    this.ThemeColors();
  }

  private static void ThemeColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).ThemeColorChanged(e);
  }

  protected virtual void ThemeColorChanged(DependencyPropertyChangedEventArgs e)
  {
    this.ThemeColors();
  }

  private static void BlackWhiteVisibilityChanged(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ColorPickerPalette colorPickerPalette = (ColorPickerPalette) o;
    if (colorPickerPalette == null)
      return;
    colorPickerPalette.ThemeColors();
    colorPickerPalette.StandardColors();
  }

  protected virtual void BlackWhiteVisibilityChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void StandardVariantsChanged(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).StandardVariantsChanged(e);
  }

  protected virtual void StandardVariantsChanged(DependencyPropertyChangedEventArgs e)
  {
    this.StandardColors();
  }

  private static void ThemeVisibilityChanged(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).ThemeVisibilityChanged(e);
  }

  protected virtual void ThemeVisibilityChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.item1 == null)
      return;
    this.item1.PanelVisibility = this.ThemePanelVisibility;
    if (this.Mode != PickerMode.Palette)
      return;
    this.ThemeColors();
  }

  private static void OnPopupHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorPickerPalette) o).OnPopupHeightChanged(e);
  }

  protected virtual void OnPopupHeightChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.PopupHeightChanged == null)
      return;
    this.PopupHeightChanged((DependencyObject) this, e);
  }

  private static void recentlyusedvisibilitychanged(
    DependencyObject o,
    DependencyPropertyChangedEventArgs e)
  {
    ColorPickerPalette colorPickerPalette = (ColorPickerPalette) o;
    if (colorPickerPalette.item1 == null)
      return;
    colorPickerPalette.item3.PanelVisibility = colorPickerPalette.RecentlyUsedPanelVisibility;
    if (colorPickerPalette.Mode != PickerMode.Palette)
      return;
    colorPickerPalette.RecentlyUsed();
  }

  private static void OnSetCustomColorsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ColorPickerPalette colorPickerPalette) || !(bool) e.NewValue)
      return;
    colorPickerPalette.LoadCustomColors();
  }

  public delegate void MoreColorWindowEventHandler(object sender, MoreColorCancelEventArgs args);

  public delegate void DropDownOpenedEventHandler(object sender, RoutedEventArgs args);
}
